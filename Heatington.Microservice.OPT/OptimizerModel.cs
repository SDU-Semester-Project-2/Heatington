using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Optimizer;
using Heatington.Services.Interfaces;

namespace Heatington.Microservice.OPT;

// TODO: For calling api inside of api
// https://stackoverflow.com/questions/69124747/how-to-call-an-api-inside-another-api-on-asp-net-core-2-1
public class OptimizerModel
{
    public static Opt OPT;

    static OptimizerModel()
    {
        // Mock asset manager
        // TODO: Change to API CALl
        string pathToHeatingGrid =
            Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
        string pathToProductionUnits =
            Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

        IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
        IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);
        AssetManager.AssetManager am = new(
            heatingGridJsonController,
            productionUnitsJsonController
        );

        // Mock source data manager
        // TODO: Change to API CALl
        string fileName = "winter_period.csv";
        string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
        IDataSource dataSource = new CsvController(filePath);
        SourceDataManager.SourceDataManager sdm = new(dataSource);

        // Optimizer
        OPT = new Opt(am, sdm);
        OPT.LoadData();
    }
}
