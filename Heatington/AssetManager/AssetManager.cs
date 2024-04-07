using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;

namespace Heatington.AssetManager;

public class AssetManager
{
    public HeatingGrid? HeatingGridInformation;
    public Dictionary<string, ProductionUnit>? ProductionUnits;

    private readonly string _pathToHeatingGrid =
        Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");

    private readonly string _pathToProductionUnits =
        Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

    private readonly IReadWriteController _heatingGridJsonController;
    private readonly IReadWriteController _productionUnitsJsonController;

    public AssetManager()
    {
        _heatingGridJsonController = new JsonController(_pathToHeatingGrid);
        _productionUnitsJsonController = new JsonController(_pathToProductionUnits);
    }

    // Alternative version:
    // public AssetManager(IReadWriteController heatingGridJsonController, IReadWriteController productionUnitsJsonController)
    // {
    //     _heatingGridJsonController = heatingGridJsonController;
    //     _productionUnitsJsonController = productionUnitsJsonController;
    // }

    public async Task LoadAssets()
    {
        Console.WriteLine("Loading assets");
        HeatingGridInformation = await _heatingGridJsonController.ReadData<HeatingGrid>();
        Console.WriteLine(HeatingGridInformation.Name);

        ProductionUnit[] units;
        units = await _productionUnitsJsonController.ReadData<ProductionUnit[]>();


        foreach (var unit in units)
        {
            Console.WriteLine(unit);
        }
    }

    public ProductionUnit ReadHeatingUnits()
    {
        throw new NotImplementedException();
    }

    public void WriteHeatingUnit()
    {
        throw new NotImplementedException();
    }
}
