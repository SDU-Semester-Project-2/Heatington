using System.Buffers;
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
    private readonly string _pathToProductionUnits=
        Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

    private IReadWriteController _heatingGridJsonController;
    private IReadWriteController _productionUnitsJsonController;

    public AssetManager()
    {
        _heatingGridJsonController = new JsonController(_pathToHeatingGrid);
        _productionUnitsJsonController = new JsonController(_pathToProductionUnits);
        LoadAssets();
    }

    private async void LoadAssets()
    {
        HeatingGridInformation = await _heatingGridJsonController.ReadData<HeatingGrid>();
        List<ProductionUnit> units = new();
        units = await _productionUnitsJsonController.ReadData<List<ProductionUnit>>();
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
