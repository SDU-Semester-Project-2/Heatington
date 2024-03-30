using System.Reflection;

namespace CsvHandle
{
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

        public List<T> ConvertRecords<T>() where T : new()
        {
            List<T> res = new();
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (properties.Length != Table[0].Length)
            {
                throw new Exception($"Number of properties in {typeof(T)} does not match number of entries in one record of the CsvTable.");
            }
            foreach (string[] values in Table)
            {
                T obj = new T();
                for (int i = 0; i < properties.Length; i++)
                {
                    Type propertyType = properties[i].PropertyType;
                    var value = Convert.ChangeType(values[i], propertyType);
                    properties[i].SetValue(obj, value);
                }
                res.Add(obj);
            }
            return res;
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
