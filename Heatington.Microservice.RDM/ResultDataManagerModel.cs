using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;

namespace Heatington.Microservice.RDM;

public class ResultDataManagerModel
{
    public static ResultDataManager.RDM? Rdm;

    public static void LoadResultDataManager(string uri)
    {
        //CSV Controller
        string path = Utilities.GetAbsolutePathToAssetsDirectory();
        CsvController csvController = new CsvController(path);

        //Result Data Manager
        Rdm = new ResultDataManager.RDM(csvController);

        //Load data
        Task<List<ResultHolder>?> optData = ResultDataManagerService.GetDataFromOpt(uri);
        optData.Wait();

        if (optData.Result != null)
        {
            Rdm.FetchOptimizationData(optData.Result);
        }

        //optData.Result.ForEach(Console.WriteLine);
    }
}
