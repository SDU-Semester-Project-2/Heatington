using Heatington.SourceDataManager;
using Heatington.Services.Interfaces;
using Heatington.Helpers;
using Heatington.Controllers;



namespace SourceDataManagerAPI
{
    public class SourceDataManagerModel
    {
        public static SourceDataManager SDM;
        static SourceDataManagerModel()
        {
            string fileName = "winter_period.csv";
            string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
            IDataSource dataSource = new CsvController(filePath);
            SDM = new SourceDataManager(dataSource);
            Task loadTimeSeries = SDM.FetchTimeSeriesDataAsync();
            loadTimeSeries.Wait();
        }
    }
}
