namespace CsvHandle
{
    class CsvData
    {
        public List<string[]> Data {get; set;}

        public CsvData(List<string[]> data)
        {
            Data = data;
        }

        public List<T> ConvertRecords<T>() where T : new()
        {
            List<T> res = new();
            var properties = typeof(T).GetProperties();
            if (properties.Length != Data[0].Length)
            {
                throw new Exception($"Number of properties in {typeof(T)} does not match number of entries in one record of the CsvData.");
            }
            foreach (string[] values in Data)
            {
                var obj = new T();

                Console.WriteLine(properties.Length);
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

    class CsvController// : IReadWriteController
    {
        private string _PathToFile;

        public CsvController(string _PathToFile)
        {
            this._PathToFile = _PathToFile;
        }

        public CsvData ReadData()
        {
            List<string[]> res = new();
            int? numberOfFields = null;
            using (StreamReader sr = new(_PathToFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(",");
                    numberOfFields = numberOfFields ?? data.Length;
                    if (data.Length != numberOfFields)
                    {
                        throw new Exception("Number of fields not consistent.");
                    }
                    res.Add(data);
                }
            }
            return new CsvData(res);
        }
    }
}
