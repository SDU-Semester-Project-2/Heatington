using System.Reflection;

namespace CsvHandle
{
    class CsvData
    {
        public List<string[]> Table { get; set; }

        public CsvData(List<string[]> data)
        {
            Table = data;
            int numberOfFields = Table[0].Length;
            if (!Table.TrueForAll(x => x.Length == numberOfFields))
            {
                throw new Exception("Number of fields not consistent throughout csv.");
            }
        }

        public static CsvData Create<T>(List<T> data)
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
            var properties = typeof(T).GetProperties();
            if (properties.Length != Table[0].Length)
            {
                throw new Exception($"Number of properties in {typeof(T)} does not match number of entries in one record of the CsvTable.");
            }
            foreach (string[] values in Table)
            {
                var obj = new T();
                for (int i = 0; i < properties.Length && i < values.Length; i++)
                {
                    var propertyType = properties[i].PropertyType;
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
        public static CsvData Deserialize(string rawData)
        {
            return new CsvData(
                rawData.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(",", StringSplitOptions.TrimEntries))
                .ToList()
            );
        }

        public static string Serialize(CsvData data)
        {
            return String.Join("\n", data.Table.Select(x => String.Join(", ", x)));
        }
    }
}
