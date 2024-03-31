using Heatington.Models;

namespace Heatington.Data
{
    /// <summary>
    /// Represents a data source for reading and writing data.
    /// <remarks>
    /// This is replaced by CsvController.cs, the current existence is only for the purpose of the documentation
    /// </remarks>
    /// TODO: Delete this interface in the refactoring phase
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Retrieves the time series data from the specified data source.
        /// </summary>
        /// <param name="filePath">The file path of the CSV file containing the data.</param>
        /// <returns>A list of DataPoint objects representing the heat demand and electricity price data.</returns>
        List<DataPoint>? GetData(string filePath);

        /// <summary>
        /// Saves the given data points to a CSV file at the specified file path.
        /// </summary>
        /// <param name="data">The list of data points to be saved.</param>
        /// <param name="filePath">The file path where the CSV file will be saved.</param>
        void SaveData(List<DataPoint> data, string filePath);
    }
}
