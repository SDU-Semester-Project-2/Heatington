using System.Reflection;

namespace CsvHandle
{
    [System.AttributeUsage(System.AttributeTargets.Constructor, AllowMultiple=false)]
    public sealed class CsvConstructorAttribute : Attribute;

    class CsvData
    {
        public List<string[]> Table { get; set; }
        public string[] Header { get; set; }

        public CsvData(List<string[]> data, string[] header = null)
        {
            int numberOfFields = data[0].Length;
            if(!data.TrueForAll(x => x.Length == numberOfFields))
            {
                throw new Exception("Number of fields not consistent throughout csv.");
            }
            if(header != null && header.Length != numberOfFields)
            {
                throw new Exception("Number of fields in csv header does not match number of fields in csv body.");
            }
            Table = data;
            Header = header;
        }

        public static CsvData Create<T>(List<T> data, string[] header = null)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            List<string[]> res = data.Select(
                x => props.Select(
                    prop => {
                        object? value = prop.GetValue(x);
                        return value != null ? value.ToString() : "";
                    }
                ).ToArray()
            ).ToList();
            return new CsvData(res);
        }

        public List<T> ConvertRecords<T>()
        {
            ConstructorInfo ctor;
            ConstructorInfo[] allCtors = typeof(T).GetConstructors();
            Console.WriteLine(allCtors.Length);
            if(allCtors.Length == 1)
            {
                ctor = allCtors[0];
            }
            else
            {
                ConstructorInfo[] csvConstructors = allCtors.Where(x => Attribute.IsDefined(x, typeof(CsvConstructorAttribute))).ToArray();
                if(csvConstructors.Length == 0)
                {
                    throw new Exception($"The type '{typeof(T)}' must have at one constructor with CsvConsturcotrAttribute.");
                }
                else if(csvConstructors.Length > 1)
                {
                    throw new Exception($"The type '{typeof(T)}' must have at most one constructor with CsvConsturcotrAttribute.");
                }
                else
                {
                    ctor = csvConstructors[0];
                }
            }
            ParameterInfo[] parameters = ctor.GetParameters();
            Dictionary<string, ParameterInfo> paramDict = parameters.ToDictionary(param => param.Name, param => param);

            List<T> res = new();
            if (parameters.Length != Table[0].Length)
            {
                throw new Exception($"Number of parameters in {typeof(T)}'s constructor does not match number of entries in a record of the CsvTable.");
            }
            bool useHeader = Header != null && checkMatchesParameters(paramDict);
            foreach (string[] values in Table)
            {
                object[] parameterValues = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo currentParam = useHeader ? paramDict[Header[i]] : parameters[i];
                    var value = Convert.ChangeType(values[i], currentParam.ParameterType);
                    parameterValues[currentParam.Position] = value;
                }
                res.Add((T) ctor.Invoke(parameterValues));
            }
            return res;
        }

        private bool checkMatchesParameters(Dictionary<string, ParameterInfo> paramDict)
        {
            return (Header.Distinct().Count() == Header.Length) && Array.TrueForAll(Header, x => paramDict.ContainsKey(x));
        }
    }

    static class CsvController
    {
        public static CsvData Deserialize(string rawData, bool includesHeader)
        {
            List<string[]> all = rawData
                                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Split(",", StringSplitOptions.TrimEntries))
                                .ToList();
            return new CsvData(
                includesHeader ? all[1..] : all,
                includesHeader ? all[0] : null
            );
        }

        public static string Serialize(CsvData data)
        {
            return (data.Header != null ? String.Join(", ", data.Header) : "") + '\n' +
                    String.Join("\n", data.Table.Select(x => String.Join(", ", x)));
        }
    }
}
