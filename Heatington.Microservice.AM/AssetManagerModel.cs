using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.AssetManager;

namespace AssetManagerAPI
{
    public class AssetManagerModel
    {
        public static AM am;

        static AssetManagerModel()
        {
            string pathToHeatingGrid =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
            string pathToProductionUnits =
                Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

            IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
            IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);
            am =
                new AM(
                    heatingGridJsonController,
                    productionUnitsJsonController
                );
            Task loadAssets = am.LoadAssets();
            loadAssets.Wait();
        }
    }
}
