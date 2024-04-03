using Heatington.Controllers;
using Heatington.Models;

namespace Heatington.Data
{

    public class CsvDataSource : IDataSource
    {

        public List<DataPoint>? GetData(string filePath)
        {
            string rawData = File.ReadAllText(filePath);
            CsvData csvData = CsvController.Deserialize(rawData, false);
            return csvData.ConvertRecords<DataPoint>();
        }

        // TODO: Check if this method is actually required
        public void SaveData(List<DataPoint> data, string filePath) => throw new NotImplementedException();
    }
}
