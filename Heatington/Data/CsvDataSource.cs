using Heatington.Models;
using Heatington.Utility;

namespace Heatington.Data
{
    /// <summary>
    /// Represents a data source for reading and writing data in CSV format.
    /// </summary>
    public class CsvDataSource : IDataSource
    {
        /// <summary>
        /// Retrieves the time series data from the specified data source.
        /// </summary>
        /// <param name="filePath">The file path of the CSV file containing the data.</param>
        /// <returns>A list of DataPoint objects representing the heat demand and electricity price data.</returns>
        public List<DataPoint>? GetData(string filePath)
        {
            string rawData = File.ReadAllText(filePath);
            CsvData csvData = CsvController.Deserialize(rawData, false);
            return csvData.ConvertRecords<DataPoint>();
        }

        /// <summary>
        /// Saves the given data points to a CSV file at the specified file path.
        /// </summary>
        /// <param name="data">The list of data points to be saved.</param>
        /// <param name="filePath">The file path where the CSV file will be saved.</param>
        /// <remarks>We may not use this</remarks>
        /// TODO: Check if this method is actually required
        public void SaveData(List<DataPoint> data, string filePath) => throw new NotImplementedException();
    }
}
