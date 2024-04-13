using Heatington.Controllers.Serializers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;

namespace Heatington.Controllers
{
    public class CsvController : IDataSource
    {
        public async Task<List<DataPoint>?> GetDataAsync(string filePath)
        {
            try
            {
                string rawData = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
                CsvData csvData = Controllers.Serializers.CsvSerializer.Deserialize(rawData, false);
                return csvData.ConvertRecords<DataPoint>();
            }
            catch (Exception e)
            {
                Utilities.DisplayException(e.Message);
                throw;
            }
        }

        // TODO: Check if this method is actually required
        public void SaveData(List<DataPoint> data, string filePath) => throw new NotImplementedException();
    }
}
