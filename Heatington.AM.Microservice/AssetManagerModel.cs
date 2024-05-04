using Heatington.Controllers.Interfaces;
using Heatington.AssetManager;
using Heatington.Controllers;
using Heatington.Helpers;

namespace AssetManagerAPI
{
    public class AssetManagerModel
    {
        public static AssetManager AM;
        static AssetManagerModel(){
            string pathToHeatingGrid =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
            string pathToProductionUnits =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

            IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
            IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);
            AM =
                new AssetManager(
                    heatingGridJsonController,
                    productionUnitsJsonController
                );
            Task loadAssets = AM.LoadAssets();
            loadAssets.Wait();
        }
    }
}
