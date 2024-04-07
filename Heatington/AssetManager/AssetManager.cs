using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;

namespace Heatington.AssetManager;

/// <summary>
///
/// </summary>
/// <example>
/// <code>
/// AssetManager AM = new AssetManager();
/// await AM.LoadAssets();
/// </code>
/// </example>
public class AssetManager
{
    public HeatingGrid? HeatingGridInformation;
    public Dictionary<ProductionUnitsEnum, ProductionUnit>? ProductionUnits;

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
        Console.WriteLine("Loading assets...");

        HeatingGridInformation = await _heatingGridJsonController.ReadData<HeatingGrid>();

        //TODO: Discuss if ProductionUnits should be stored separately in different files or all in one
        Dictionary<string, ProductionUnit> prodUnits =
            await _productionUnitsJsonController.ReadData<Dictionary<string, ProductionUnit>>();

        ProductionUnits = new();

        // In fetched dictionary string keys are converted into enum keys for code intellisense and readability
        foreach ((KeyValuePair<string, ProductionUnit> prodUnit, int i) in prodUnits.Select((value, i) => (value, i)))
        {
            ProductionUnits.Add((ProductionUnitsEnum)i, prodUnit.Value);
        }
    }

    public Dictionary<ProductionUnitsEnum, ProductionUnit> ReadHeatingUnits()
    {
        if (ProductionUnits == null)
        {
            Utilities.DisplayException("ProductionUnits are empty!\nDid you forget to LoadAssets before?");
            throw new MissingMemberException();
        }

        // TODO: right now it does nothing but may be useful in different scenarios?
        Dictionary<ProductionUnitsEnum, ProductionUnit> heatingUnits =
            ProductionUnits.Where(
                kvp => kvp.Value.MaxHeat != 0
            )
            .ToDictionary(item =>
                    item.Key, item => item.Value
            );

        return heatingUnits;
    }

    public void WriteHeatingUnit(Guid UnitId)
    {
        // TODO: I'm sure in which way we'll be writing the Heating Units
        throw new NotImplementedException();
    }
}
