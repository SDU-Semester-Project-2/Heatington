using Heatington.SourceDataManager;
using Heatington.Services.Interfaces;
using Heatington.Helpers;
using Heatington.Controllers;

namespace SourceDataManagerAPI
{
    public class SourceDataManagerModel
    {
        public static SDM SDM_Winter;
        public static SDM SDM_Summer;
        public static SDM SDM_WinterReal;
        public static SDM SDM_SummerReal;

        static SourceDataManagerModel()
        {
            string fileNameWinter = "winter_period.csv";
            string filePathWinter = Utilities.GeneratePathToFileInAssetsDirectory(fileNameWinter);
            string fileNameSummer = "summer_period.csv";
            string filePathSummer = Utilities.GeneratePathToFileInAssetsDirectory(fileNameSummer);

            IDataSource dataSourceWinter = new CsvController(filePathWinter);
            IDataSource dataSourceSummer = new CsvController(filePathSummer);
            
            SDM_Winter = new SDM(dataSourceWinter);
            SDM_Summer = new SDM(dataSourceSummer);

            Task loadTimeSeriesWinter = SDM_Winter.FetchTimeSeriesDataAsync();
            Task loadTimeSeriesSummer = SDM_Summer.FetchTimeSeriesDataAsync();

            loadTimeSeriesWinter.Wait();
            loadTimeSeriesSummer.Wait();

            
            IDataSource dataSourceWinterReal = new RealDataController(filePathWinter);
            IDataSource dataSourceSummerReal = new RealDataController(filePathSummer);

            Task loadRealDataSourceWinter = dataSourceWinterReal.GetDataAsync();
            Task loadRealDataSourceSummer = dataSourceSummerReal.GetDataAsync();

            loadRealDataSourceWinter.Wait();
            loadRealDataSourceSummer.Wait();
            
            SDM_WinterReal = new SDM(dataSourceWinterReal);
            SDM_SummerReal = new SDM(dataSourceWinterReal);

            Task loadTimeSeriesWinterReal = SDM_WinterReal.FetchTimeSeriesDataAsync();
            Task loadTimeSeriesSummerReal = SDM_SummerReal.FetchTimeSeriesDataAsync();
            
            loadTimeSeriesWinterReal.Wait();
            loadTimeSeriesSummerReal.Wait();

            
        }
    }
}
