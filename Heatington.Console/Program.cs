using Heatington.AssetManager;
using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Services.Interfaces;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.Console
{
    internal static class Program
    {
        // TODO: Rewrite this method and implement the actual application logic
        static async Task Main(string[] args)
        {
            // update the documentation
            await RunDocFx();

            // Asset Manager with controllers and paths to assets
            string pathToHeatingGrid =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
            string pathToProductionUnits =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

            IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
            IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);
            AM am =
                new AM(
                    heatingGridJsonController,
                    productionUnitsJsonController
                );

            // Source Data Manager with csv data and controller
            string fileName = "winter_period.csv";
            string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
            IDataSource dataSource = new CsvController(filePath);
            SourceDataManager.SDM sdm = new(dataSource);


            // Get the production units
            Task loadAssets = am.LoadAssets();
            loadAssets.Wait();
            List<ProductionUnit> productionUnits = am.ProductionUnits!.Values.ToList();

            // Get the data points
            Task fetchTimeSeries = sdm.FetchTimeSeriesDataAsync();
            fetchTimeSeries.Wait();
            List<DataPoint>? dataPoints = sdm.TimeSeriesData;

            // Optimizer
            OPT optimizer = new OPT(productionUnits, dataPoints);

            // Console UI
            ConsoleUI consoleUi = new ConsoleUI(am, sdm, optimizer);
            consoleUi.StartUi();
        }


        private static async Task RunDocFx()
        {
            //updates the documentation on dotnet run
            await Docfx.Docset.Build("../docfx.json");
        }
    }
}
