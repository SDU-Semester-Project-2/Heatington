namespace CsvHandle
{
    class CsvData
    {
        public List<string[]> Table { get; set; }

        public CsvData(List<string[]> data)
        {
            Table = data.Select(x => x.Select(y => y.ToString()).ToArray()).ToList();
            int numberOfFields = Table[0].Length;
            if (!Table.TrueForAll(x => x.Length == numberOfFields))
            {
                throw new Exception("Number of fields not consistent throughout csv.");
            }
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

    }
}
