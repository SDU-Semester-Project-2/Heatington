using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Services.Interfaces;
using Heatington.Services.Serializers;

namespace Heatington.Controllers
{
    /// <summary>
    /// Class for controlling a csv file
    /// </summary>
    /// <param name="filePath">Path to a file to control</param>
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

        public void SaveData(List<DataPoint> data)
        {
            try
            {
                CsvData csv = CsvData.Create<DataPoint>(data);
                string rawCsvData = CsvSerializer.Serialize(csv);
                _fileController.WriteData<string>(rawCsvData);
            }
            catch (Exception e)
            {
                Utilities.DisplayException(e.Message);
                throw;
            }
        }

        // Overload function
        public void SaveData(List<ResultHolder> data)
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

        //TODO rewrite with generics
        // Another overload function, it should be done with generics
        public void SaveData(List<FormatedResultHolder> data)
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
