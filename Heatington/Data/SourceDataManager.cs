/*
 * SourceDataManager uses an IDataSource for all its data access needs. This approach adheres to the Dependency
 * Inversion Principle, part of SOLID principles, ensuring that SourceDataManager depends on abstractions and not
 * on concrete implementation of data access.
 *
 * IDataSource is an interface which defines the contracts for fetching and saving data. Any data source in our
 * application should implement this interface. This gives us the flexibility to easily switch or add
 * different data sources (like CsvDataSource, XmlDataSource, etc) without changing much of the existing code.
 *
 * CsvDataSource is an implementation of IDataSource specifically designed for handling CSV files. It can read data
 * from a CSV file and also save data to a CSV file. If in future, we need to fetch data from a different file format
 * or a database, we can create a new implementation of IDataSource for that.
 *
 * This design not only makes our code more flexible and extensible, but also makes it easier to write unit tests as
 * we can easily mock the IDataSource interface.
 */

using Heatington.Models;

namespace Heatington.Data
{
    /// <summary>
    /// Manages the retrieval and conversion of data from a data source for the Heatington application.
    /// </summary>
    public class SourceDataManager
    {
        /// <summary>
        /// Represents the data source for fetching and saving time series data.
        /// </summary>
        private readonly IDataSource _dataSource;

        /// <summary>
        /// Gets the file path for the data source.
        /// </summary>
        private string FilePath { get; }

        /// <summary>
        /// Represents a time series data for heat demand and electricity price.
        /// </summary>
        /// <remarks>
        /// The time series data contains a list of <see cref="DataPoint"/> objects,
        /// each representing a data point with information about the start time, end time,
        /// heat demand (in kWh) and electricity price (in DKK/MWh).
        /// </remarks>
        private List<DataPoint>? TimeSeriesData { get; set; }

        /// <summary>
        /// The SourceDataManager class manages the data retrieval and conversion operations from a data source.
        /// </summary>
        public SourceDataManager(IDataSource dataSource, string filePath)
        {
            _dataSource = dataSource;
            FilePath = filePath;
        }

        /// <summary>
        /// Converts data from an API to a CSV file and saves it using the specified file path.
        /// </summary>
        /// <param name="dataFromApi">The data retrieved from the API.</param>
        public void ConvertApiToCsv(List<DataPoint> dataFromApi)
        {
            _dataSource.SaveData(dataFromApi, FilePath);
        }

        /// <summary>
        /// Fetches time series data from the data source.
        /// </summary>
        public void FetchTimeSeriesData()
        {
            Console.WriteLine("Start fetching time series data from the data source...");
            try
            {
                TimeSeriesData = _dataSource.GetData(FilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching time series data: " + ex.Message);
            }
        }

        /// <summary>
        /// Logs the time series data to the console.
        /// </summary>
        /// <remarks>
        /// This for testing purpose only
        /// </remarks>
        /// TODO: Remove this method, when moving to Blazor
        public void LogTimeSeriesData()
        {
            if (TimeSeriesData == null)
            {
                return;
            }

            foreach (DataPoint dataPoint in TimeSeriesData)
            {
                // Convert UTC time to Danish local time
                TimeZoneInfo danishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                DateTime startTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.StartTime, danishTimeZone);
                DateTime endTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.EndTime, danishTimeZone);

                // Format times as "dd.MM.yyyy HH:mm"
                string formattedStart = startTimeInDanish.ToString("dd.MM.yyyy HH:mm");
                string formattedEnd = endTimeInDanish.ToString("dd.MM.yyyy HH:mm");

                // Log the data in console and add "MWh" to Heat Demand and Electricity Price
                Console.WriteLine(
                    $"Index: {TimeSeriesData.IndexOf(dataPoint)}; "+
                    $"Start Time: {formattedStart}; " +
                    $"End Time: {formattedEnd}; " +
                    $"Heat Demand: {dataPoint.HeatDemand} MWh; " +
                    $"Electricity Price: {dataPoint.ElectricityPrice} DKK/MWh");
            }
        }
    }
}
