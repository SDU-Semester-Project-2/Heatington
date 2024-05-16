using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Services.Interfaces;
using Heatington.Helpers;
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
            AssetManager.AssetManager assetManager =
                new AssetManager.AssetManager(
                    heatingGridJsonController,
                    productionUnitsJsonController
                );

            // Source Data Manager with csv data and controller
            string fileName = "winter_period.csv";
            string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
            IDataSource dataSource = new CsvController(filePath);
            SourceDataManager.SourceDataManager sourceDataManager = new(dataSource);

            // Optimizer
            Opt optimizer = new Opt(assetManager, sourceDataManager);

            // Console UI
            ConsoleUI consoleUi = new ConsoleUI(assetManager, sourceDataManager, optimizer);
            consoleUi.StartUi();
        }

        private static async Task RunDocFx()
        {
            //updates the documentation on dotnet run
            // await Docfx.Docset.Build("../docfx.json");
        }
    }
}
