using System.Reflection;

namespace Heatington.Controllers

{
    [System.AttributeUsage(System.AttributeTargets.Constructor, AllowMultiple = false)]
    public sealed class CsvConstructorAttribute : Attribute;

    class CsvData
    {
        public List<string[]> Table { get; private set; }
        public string[]? Header { get; private set; }

        public CsvData(List<string[]> data, string[]? header = null)
        {
            int numberOfFields = data[0].Length;
            if (!data.TrueForAll(x => x.Length == numberOfFields))
            {
                throw new Exception("Number of fields not consistent throughout csv.");
            }
            if (header != null && header.Length != numberOfFields)
            {
                throw new Exception("Number of fields in csv header does not match number of fields in csv body.");
            }

            Table = data;
            Header = header;
        }

        public static CsvData Create<T>(List<T> data, string[]? header = null)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            header = header ?? props.Select(x => x.Name).ToArray();
            List<string[]> res = data.Select(
                x => props.Select(
                    prop =>
                    {
                        object? value = prop.GetValue(x);
                        return value != null ? (value.ToString() ?? "") : "";
                    }
                ).ToArray()
            ).ToList();
            return new CsvData(res, header);
        }

        public List<T> ConvertRecords<T>()
        {
            ConstructorInfo ctor;
            ConstructorInfo[] allCtors = typeof(T).GetConstructors();
            Console.WriteLine(allCtors.Length);
            if (allCtors.Length == 1)
            {
                ctor = allCtors[0];
            }
            else
            {
                ConstructorInfo[] csvConstructors = allCtors.Where(x => Attribute.IsDefined(x, typeof(CsvConstructorAttribute))).ToArray();
                if (csvConstructors.Length == 0)
                {
                    throw new Exception(
                        $"The type '{typeof(T)}' must have at one constructor with CsvConsturcotrAttribute.");
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
            Dictionary<string, ParameterInfo> paramDict = parameters.ToDictionary(param => param.Name ?? throw new Exception("Expected parameter from parameter list, got return parameter."), param => param);

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
                    ParameterInfo currentParam = useHeader ? paramDict[Header[i]] : parameters[i];
                    var value = Convert.ChangeType(values[i], currentParam.ParameterType);
                    parameterValues[currentParam.Position] = value;
                }
                res.Add((T)ctor.Invoke(parameterValues));
            }

            return res;
        }

        private bool checkMatchesParameters(Dictionary<string, ParameterInfo> paramDict)
        {

            return (Header != null) && (Header.Distinct().Count() == Header.Length) && Array.TrueForAll(Header, x => paramDict.ContainsKey(x));
        }
    }

    static class CsvController
    {
        public static CsvData Deserialize(string rawData, bool includesHeader)
        {
            int i = 0;
            List<string> currentRecord = new();
            List<string[]> all = new();
            bool isDoubleQuoted = false;
            string current = "";
            while (i < rawData.Length)
            {
                isDoubleQuoted = false;
                current = "";
                if (rawData[i] == '"')
                {
                    isDoubleQuoted = true;
                }
                else if (rawData[i] == '\n')
                {
                    throw new Exception("Entries can't start with new line.");
                }
                else
                {
                    current += rawData[i];
                }

                i++;
                bool inEntry = true;
                while (inEntry && i < rawData.Length)
                {
                    if (rawData[i] == '"')
                    {
                        if (isDoubleQuoted)
                        {
                            if (i + 1 < rawData.Length)
                            {
                                i++;
                                if (rawData[i] == '"')
                                {
                                    current += rawData[i];
                                }
                                else if (rawData[i] == ',')
                                {
                                    currentRecord.Add(current);
                                    inEntry = false;
                                }
                                else if (rawData[i] == '\n')
                                {
                                    currentRecord.Add(current);
                                    all.Add(currentRecord.ToArray());
                                    currentRecord.Clear();
                                    inEntry = false;
                                }
                                else
                                {
                                    throw new Exception("'\"' characters have to be escaped with '\"' characters.");
                                }
                            }
                            else
                            {
                                currentRecord.Add(current);
                                all.Add(currentRecord.ToArray());
                                currentRecord.Clear();
                                inEntry = false;
                            }
                        }
                        else
                        {
                            throw new Exception(
                                "To have a '\"' character in a CSV entry you need to encoles the entry with '\"' and escape the required '\"' with another '\"'.");
                        }
                    }
                    else if (rawData[i] == ',')
                    {
                        if (isDoubleQuoted)
                        {
                            current += rawData[i];
                        }
                        else
                        {
                            currentRecord.Add(current);
                            inEntry = false;
                        }
                    }
                    else if (rawData[i] == '\n')
                    {
                        if (isDoubleQuoted)
                        {
                            current += rawData[i];
                        }
                        else
                        {
                            currentRecord.Add(current);
                            all.Add(currentRecord.ToArray());
                            currentRecord.Clear();
                            inEntry = false;
                        }
                    }
                    else
                    {
                        current += rawData[i];
                    }

                    i++;
                }
                if (inEntry)
                {
                    if (isDoubleQuoted)
                    {
                        throw new Exception("All entries have to close the opened '\"'.");
                    }
                    else
                    {
                        currentRecord.Add(current);
                        all.Add(currentRecord.ToArray());
                        currentRecord.Clear();
                        inEntry = false;
                    }
                }
            }

            return new CsvData(
                includesHeader ? all[1..] : all,
                includesHeader ? all[0] : null
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
                        ?
                        '"' + String.Join("\"\"", str.Split('"')) + '"'
                        :
                        str
                    ).ToArray()
                ).ToList();

            string[]? escapedHeader = data.Header != null ?
                data.Header.Select(
                    str =>
                    requiresQuotes.Any(chr => str.Contains(chr))
                    ?
                    '"' + String.Join("\"\"", str.Split('"')) + '"'
                    :
                    str
                ).ToArray()
                : null;

            return ((escapedHeader != null && includeHeaderIfNotNull) ? String.Join(",", escapedHeader) : "") + '\n' +
                    String.Join("\n", escapedTable.Select(x => String.Join(",", x)));
        }
    }
}