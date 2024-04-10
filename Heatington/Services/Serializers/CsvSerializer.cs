using System.Globalization;
using System.Reflection;

namespace Heatington.Services.Serializers
{
    [System.AttributeUsage(System.AttributeTargets.Constructor, AllowMultiple = false)]
    public sealed class CsvConstructorAttribute : Attribute;

    public class CsvData
    {
        public List<string[]> Table { get; private set; }
        public string[]? Header { get; private set; }

        public CsvData(List<string[]> data, string[]? header = null)
        {
            int numberOfFields = data.Count == 0 ? 0 : data[0].Length;
            if (!data.TrueForAll(x => x.Length == numberOfFields))
            {
                throw new Exception("Number of fields not consistent throughout csv.");
            }

            if (header != null && numberOfFields != null && header.Length != numberOfFields)
            {
                throw new Exception("Number of fields in csv header does not match number of fields in csv body.");
            }

            Table = data;
            Header = header;
        }


        public static CsvData Create<T>(List<T> data, string[]? header = null, CultureInfo? culture = null)
        {
            culture = culture ?? new("da-da");
            PropertyInfo[] props = typeof(T).GetProperties();
            header = header ?? props.Select(x => x.Name).ToArray();
            List<string[]> res = data.Select(
                x => props.Select(
                    prop =>
                    {
                        object? value = prop.GetValue(x);
                        return value != null ? (Convert.ToString(value, culture) ?? "") : "";
                    }
                ).ToArray()
            ).ToList();
            return new CsvData(res, header);
        }

        public List<T> ConvertRecords<T>(CultureInfo? culture = null)
        {
            culture = culture ?? new("da-da");
            ConstructorInfo ctor;
            ConstructorInfo[] allCtors = typeof(T).GetConstructors();
            if (allCtors.Length == 1)
            {
                ctor = allCtors[0];
            }
            else
            {
                ConstructorInfo[] csvConstructors =
                    allCtors.Where(x => Attribute.IsDefined(x, typeof(CsvConstructorAttribute))).ToArray();
                if (csvConstructors.Length == 0)
                {
                    throw new Exception(
                        $"The type '{typeof(T)}' must have at least one constructor.");
                }
                else if (csvConstructors.Length > 1)
                {
                    throw new Exception(
                        $"The type '{typeof(T)}' must have at most one constructor with CsvConsturcotrAttribute.");
                }
                else
                {
                    ctor = csvConstructors[0];
                }
            }

            ParameterInfo[] parameters = ctor.GetParameters();
            Dictionary<string, ParameterInfo> paramDict = parameters.ToDictionary(
                param => param.Name ??
                         throw new Exception("Expected parameter from parameter list, got return parameter."),
                param => param);

            List<T> res = new();
            if (parameters.Length != Table[0].Length)
            {
                throw new Exception(
                    $"Number of parameters in {typeof(T)}'s constructor does not match number of entries in a record of the CsvTable.");
            }

            bool useHeader = checkMatchesParameters(paramDict);
            foreach (string[] values in Table)
            {
                object[] parameterValues = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo currentParam = Header != null && useHeader ? paramDict[Header[i]] : parameters[i];
                    var value = Convert.ChangeType(values[i], currentParam.ParameterType, culture);
                    parameterValues[currentParam.Position] = value;
                }

                res.Add((T)ctor.Invoke(parameterValues));
            }

            return res;
        }

        private bool checkMatchesParameters(Dictionary<string, ParameterInfo> paramDict)
        {
            return (Header != null) && (Header.Distinct().Count() == Header.Length) &&
                   Array.TrueForAll(Header, x => paramDict.ContainsKey(x));
        }
    }

    public static class CsvSerializer
    {
        private enum State
        {
            Start,
            UnquotedEntry,
            QuotedEntry,
            AnotherQuote
        }
        public static CsvData Deserialize(string rawData, bool includesHeader)
        {
            State currentState = State.Start;

            int i = 0;
            List<string> currentRecord = new();
            List<string[]> all = new();
            string currentEntry = "";
            while (true)
            {
                if (currentState == State.Start)
                {
                    if (!(i < rawData.Length))
                    {
                        if (currentEntry == "" && currentRecord.Count == 0)
                        {
                            break;
                        }

                        currentState = State.Start;
                        currentRecord.Add(currentEntry);
                        all.Add(currentRecord.ToArray());
                        currentRecord.Clear();
                        break;
                    }
                    else if (rawData[i] == '"')
                    {
                        currentState = State.QuotedEntry;
                    }
                    else if (rawData[i] == '\n')
                    {
                        currentRecord.Add(currentEntry);
                        all.Add(currentRecord.ToArray());
                        currentRecord.Clear();
                        currentEntry = "";
                    }
                    else if (rawData[i] == ',')
                    {
                        currentRecord.Add(currentEntry);
                        currentEntry = "";
                    }
                    else
                    {
                        currentState = State.UnquotedEntry;
                        currentEntry += rawData[i];
                    }
                }
                else if (currentState == State.UnquotedEntry)
                {
                    if (!(i < rawData.Length))
                    {
                        currentState = State.Start;
                        currentRecord.Add(currentEntry);
                        currentEntry = "";
                        all.Add(currentRecord.ToArray());
                        currentRecord.Clear();
                        break;
                    }
                    else if (rawData[i] == '"')
                    {
                        throw new Exception(
                            "To have a '\"' character in a CSV entry you need to encoles the entry with '\"' and escape the required '\"' with another '\"'.");
                    }
                    else if (rawData[i] == '\n')
                    {
                        currentState = State.Start;
                        currentRecord.Add(currentEntry);
                        currentEntry = "";
                        all.Add(currentRecord.ToArray());
                        currentRecord.Clear();
                    }
                    else if (rawData[i] == ',')
                    {
                        currentState = State.Start;
                        currentRecord.Add(currentEntry);
                        currentEntry = "";
                    }
                    else
                    {
                        currentEntry += rawData[i];
                    }
                }
                else if (currentState == State.QuotedEntry)
                {
                    if (!(i < rawData.Length))
                    {
                        throw new Exception("All entries opened with '\"' have to be closed.");
                    }
                    else if (rawData[i] == '"')
                    {
                        currentState = State.AnotherQuote;
                    }
                    else
                    {
                        currentEntry += rawData[i];
                    }
                }
                else if (currentState == State.AnotherQuote)
                {
                    if (!(i < rawData.Length))
                    {
                        currentState = State.Start;
                        currentRecord.Add(currentEntry);
                        currentEntry = "";
                        all.Add(currentRecord.ToArray());
                        currentRecord.Clear();
                        break;
                    }
                    else if (rawData[i] == '"')
                    {
                        currentState = State.QuotedEntry;
                        currentEntry += rawData[i];
                    }
                    else if (rawData[i] == '\n')
                    {
                        currentState = State.Start;
                        currentRecord.Add(currentEntry);
                        currentEntry = "";
                        all.Add(currentRecord.ToArray());
                        currentRecord.Clear();
                    }
                    else if (rawData[i] == ',')
                    {
                        currentState = State.Start;
                        currentRecord.Add(currentEntry);
                        currentEntry = "";
                    }
                    else
                    {
                        throw new Exception("'\"' characters have to be escaped with '\"' characters.");
                    }
                }

                i++;
            }

            List<string[]> table;
            string[]? header = null;
            if (includesHeader)
            {
                if (all.Count == 0)
                {
                    header = null;
                    table = new List<string[]> { };
                }
                else if (all.Count == 1)
                {
                    header = all[0];
                    table = new List<string[]> { };
                }
                else
                {
                    header = all[0];
                    table = all[1..];
                }
            }
            else
            {
                table = all;
            }

            return new CsvData(
                table,
                header
            );
        }

        public static string Serialize(CsvData data, bool includeHeaderIfNotNull = true)
        {
            char[] requiresQuotes = new[] { ',', '\n', '"' };
            List<string[]> escapedTable =
                data.Table.Select(
                    arr => arr.Select(
                        str =>
                            requiresQuotes.Any(chr => str.Contains(chr))
                                ? '"' + String.Join("\"\"", str.Split('"')) + '"'
                                : str
                    ).ToArray()
                ).ToList();

            string[]? escapedHeader = data.Header != null
                ? data.Header.Select(
                    str =>
                        requiresQuotes.Any(chr => str.Contains(chr))
                            ? '"' + String.Join("\"\"", str.Split('"')) + '"'
                            : str
                ).ToArray()
                : null;

            return ((escapedHeader != null && includeHeaderIfNotNull) ? String.Join(",", escapedHeader) + '\n' : "") +
                   String.Join("\n", escapedTable.Select(x => String.Join(",", x)));
        }
    }
}
