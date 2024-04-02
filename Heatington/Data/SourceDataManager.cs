using Heatington.Models;

namespace Heatington.Data
{

    public class SourceDataManager
    {

        private readonly IDataSource _dataSource;
        private readonly string _filePath;
        public List<DataPoint>? TimeSeriesData { get; set; }
        public SourceDataManager(IDataSource dataSource, string filePath)
        {
            _dataSource = dataSource;
            _filePath = filePath;
        }

        /// TODO: Take a look at this method again
        public void ConvertApiToCsv(List<DataPoint> dataFromApi)
        {
            _dataSource.SaveData(dataFromApi, _filePath);
        }

        public void FetchTimeSeriesData()
        {
            Console.WriteLine("Start fetching time series data from the data source...");
            try
            {
                TimeSeriesData = _dataSource.GetData(_filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching time series data: " + ex.Message);
            }
        }

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
