namespace Heatington.SourceDataManager
using Heatington.Controllers;

using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Services.Interfaces;
using Heatington.Helpers;
using Heatington.Optimizer;


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
            SourceDataManager.SourceDataManager SDM = new(dataSource);
        }
    }
}
