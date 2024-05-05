using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Optimizer;
using Heatington.Services.Interfaces;

namespace Heatington.Microservice.OPT;

public class OptimizerModel
{
    public static Opt? OPT;

    static OptimizerModel()
    {
        LoadOptimizer().Wait();
    }

    public static async Task LoadOptimizer()
    {
        // Asset Manager
        AssetManager.AssetManager am = await GetAssetManager();

        // SourceDataManager
        SourceDataManager.SourceDataManager sdm = await GetSourceDataManager();

        // Optimizer
        OPT = new Opt(am, sdm);
        OPT.LoadData();
    }

    private static async Task<AssetManager.AssetManager> GetAssetManager()
    {
        try
        {
            string uri = "http://www.contoso.com/";
            // string uri = "http://localhost:5012";

            using HttpResponseMessage response = await Program.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);
            // string responseBody = await Program.Client.GetStringAsync(uri);

            // TODO: change to actual asset manager
            string pathToHeatingGrid =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
            string pathToProductionUnits =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

            IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
            IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);

            return new(
                heatingGridJsonController,
                productionUnitsJsonController
            );
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }

    private static async Task<SourceDataManager.SourceDataManager> GetSourceDataManager()
    {
        try
        {
            string uri = "http://www.contoso.com/";
            // string uri = "http://localhost:5012";
            using HttpResponseMessage response = await Program.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);

            //TODO: Change to actual sdm
            string fileName = "winter_period.csv";
            string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
            IDataSource dataSource = new CsvController(filePath);
            return new(dataSource);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }

    private static async Task<AssetManager.AssetManager> GetAssetManager()
    {
        try
        {
            string uri = "http://www.contoso.com/";
            // string uri = "http://localhost:5012";

            using HttpResponseMessage response = await Program.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);
            // string responseBody = await Program.Client.GetStringAsync(uri);

            // TODO: change to actual asset manager
            string pathToHeatingGrid =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
            string pathToProductionUnits =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

            IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
            IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);

            return new(
                heatingGridJsonController,
                productionUnitsJsonController
            );
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }

    private static async Task<SourceDataManager.SourceDataManager> GetSourceDataManager()
    {
        try
        {
            string uri = "http://www.contoso.com/";
            // string uri = "http://localhost:5012";
            using HttpResponseMessage response = await Program.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);

            //TODO: Change to actual sdm
            string fileName = "winter_period.csv";
            string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
            IDataSource dataSource = new CsvController(filePath);
            return new(dataSource);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }
}
