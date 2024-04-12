using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Services.Serializers;

namespace Heatington.Controllers
{
    public class CsvController(string filePath) : IDataSource
    {
        private IReadWriteController _fileController = new FileController(filePath);

        public async Task<List<DataPoint>?> GetDataAsync()
        {
            try
            {
                string rawData = await _fileController.ReadData<string>();
                CsvData csvData = CsvSerializer.Deserialize(rawData, false);
                return csvData.ConvertRecords<DataPoint>();
            }
            catch (Exception e)
            {
                Utilities.DisplayException(e.Message);
                throw;
            }
        }

        // TODO: Check if this method is actually required
        public void SaveData(List<DataPoint> data)
        {
            try
            {
                CsvData csv = CsvData.Create(data);
                string rawCsvData = CsvSerializer.Serialize(csv);
                _fileController.WriteData<string>(rawCsvData);
            }
            catch (Exception e)
            {
                Utilities.DisplayException(e.Message);
                throw;
            }
        }
    }
}
