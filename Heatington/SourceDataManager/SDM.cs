using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Services.Interfaces;

namespace Heatington.SourceDataManager
{
    public class SDM
    {
        private readonly IDataSource _dataSource;
        public List<DataPoint>? TimeSeriesData { get; set; }

        public SDM(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        /// TODO: Take a look at this method again
        public void ConvertApiToCsv(List<DataPoint> dataFromApi)
        {
            _dataSource.SaveData(dataFromApi);
        }

        public async Task FetchTimeSeriesDataAsync()
        {
            Console.WriteLine("Start fetching time series data from the data source...");
            try
            {
                TimeSeriesData = await _dataSource.GetDataAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Utilities.DisplayException(ex.Message);
                throw;
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
                TimeZoneInfo danishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                DateTime startTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.StartTime, danishTimeZone);
                DateTime endTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.EndTime, danishTimeZone);

                string formattedStart = startTimeInDanish.ToString("dd.MM.yyyy HH:mm");
                string formattedEnd = endTimeInDanish.ToString("dd.MM.yyyy HH:mm");

                Console.WriteLine(
                    $"Index: {TimeSeriesData.IndexOf(dataPoint)}; " +
                    $"Start Time: {formattedStart}; " +
                    $"End Time: {formattedEnd}; " +
                    $"Heat Demand: {dataPoint.HeatDemand} MWh; " +
                    $"Electricity Price: {dataPoint.ElectricityPrice} DKK/MWh");
            }
        }
    }
}
