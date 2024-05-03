using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Heatington.AssetManager;
using Heatington.Controllers.Interfaces;
using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;

namespace AssetManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionUnitsController : ControllerBase
    {
        private AssetManager AM;
        public ProductionUnitsController(){
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

        [HttpGet]
        public ActionResult<List<ProductionUnit>> Get()
        {
            return AM.ProductionUnits!.Values.ToList();
        }
    }
}
