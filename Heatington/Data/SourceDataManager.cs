using Heatington.Models;
using System.Globalization;

namespace Heatington.Data
{
    public class SourceDataManager
    {
        private readonly IDataSource _dataSource;
        private string FilePath { get; }
        private List<DataPoint>? TimeSeriesData { get; set; }

        public SourceDataManager(IDataSource dataSource, string filePath)
        {
            _dataSource = dataSource;
            FilePath = filePath;
        }

        public void ConvertApiToCsv(List<DataPoint> dataFromApi)
        {
            _dataSource.SaveData(dataFromApi, FilePath);
        }

        public void FetchTimeSeriesData()
        {
            TimeSeriesData = _dataSource.GetData(FilePath);
        }

        public void LogTimeSeriesData()
        {
            if (TimeSeriesData == null)
            {
                return;
            }

            // Define Danish culture
            CultureInfo danishCulture = new CultureInfo("da-DK");

            // Define Danish time zone (Central European Time)
            TimeZoneInfo danishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

            foreach (DataPoint dataPoint in TimeSeriesData)
            {
                // Convert UTC time to Danish local time
                DateTime startTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.StartTime, danishTimeZone);
                DateTime endTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.EndTime, danishTimeZone);

                // Log the data in console with Danish culture and Danish local time, and add "MWh" to Heat Demand and Electricity Price
                Console.WriteLine(
                    $"Start Time: {startTimeInDanish.ToString(danishCulture)}; " +
                    $"End Time: {endTimeInDanish.ToString(danishCulture)}; " +
                    $"Heat Demand: {dataPoint.HeatDemand.ToString(danishCulture)} MWh; " +
                    $"Electricity Price: {dataPoint.ElectricityPrice.ToString(danishCulture)} DKK/MWh");
            }
        }
    }
}
