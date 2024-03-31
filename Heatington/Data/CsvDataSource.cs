using System.Globalization;
using Heatington.Models;

namespace Heatington.Data
{
    /// <summary>
    /// Represents a data source for reading and writing data in CSV format.
    /// <remarks>
    /// This is replaced by CsvController.cs, the current existence is only for the purpose of the documentation
    /// </remarks>
    /// TODO: Delete this class in the refactoring phase
    /// </summary>
    public class CsvDataSource : IDataSource
    {
        /// <summary>
        /// Retrieves the data points from the specified file path.
        /// </summary>
        /// <param name="filePath">The file path of the CSV file.</param>
        /// <returns>A list of DataPoint objects representing the heat demand and electricity price data.</returns>
        public List<DataPoint>? GetData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            CultureInfo provider = CultureInfo.InvariantCulture;
            const string format = "M/d/yy H:mm";

            return lines.Select(line => line.Split(','))
                .Select(parts => new DataPoint(
                    startTime: DateTime.ParseExact(parts[0], format, provider),
                    endTime: DateTime.ParseExact(parts[1], format, provider),
                    heatDemand: double.Parse(parts[2]),
                    electricityPrice: double.Parse(parts[3])
                )).ToList();
        }

        /// <summary>
        /// Saves the given data points to a CSV file at the specified file path.
        /// </summary>
        /// <param name="data">The list of data points to be saved.</param>
        /// <param name="filePath">The file path where the CSV file will be saved.</param>
        public void SaveData(List<DataPoint> data, string filePath) => throw new NotImplementedException();
    }
}
