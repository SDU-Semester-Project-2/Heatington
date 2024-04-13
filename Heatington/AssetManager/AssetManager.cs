using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;

namespace Heatington.AssetManager;

/// <example>
/// <code>
/// AssetManager AM = new AssetManager();
/// await AM.LoadAssets();
/// </code>
/// </example>
public class AssetManager
{
    public HeatingGrid? HeatingGridInformation { get; private set; }
    public Dictionary<ProductionUnitsEnum, ProductionUnit>? ProductionUnits { get; private set; }

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

        // Only if there is no other way then we can load assets in constructor
        // it's a bad idea tho bcs there is no option to wait for a method inside of a constructor
        // LoadAssets();
    }

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

    private static ArgumentException ThrowExceptionProductionUnitsEmpty()
    {
        Utilities.DisplayException("ProductionUnits are empty!\nDid you forget to run LoadAssets() before?");
        throw new ArgumentException("ProductionUnits empty.");
    }

    public Dictionary<ProductionUnitsEnum, ProductionUnit> ReadHeatingUnits()
    {
        if (ProductionUnits == null)
        {
            throw ThrowExceptionProductionUnitsEmpty();
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


    // Pass unitId and body to update unit.
    public void WriteHeatingUnit(Guid unitId, ProductionUnit heatingUnitNewbBody)
    {
        if (ProductionUnits == null)
        {
            throw ThrowExceptionProductionUnitsEmpty();
        }

        KeyValuePair<ProductionUnitsEnum, ProductionUnit> productionUnitToWrite =
            ProductionUnits.FirstOrDefault(value => value.Value.Id == unitId);

        ProductionUnits[productionUnitToWrite.Key] = heatingUnitNewbBody;
    }

    // Pass key to the unit(the key that you would use to access the Dictonary) and body to update unit.
    public void WriteHeatingUnit(ProductionUnitsEnum productionUnitKey, ProductionUnit heatingUnitNewbBody)
    {
        if (ProductionUnits == null)
        {
            throw ThrowExceptionProductionUnitsEmpty();
        }

        ProductionUnits[productionUnitKey] = heatingUnitNewbBody;
    }

    // Pass only edited HeatingUnit and it should update automatically
    public void WriteHeatingUnit(ProductionUnit editedHeatingUnit)
    {
        if (ProductionUnits == null)
        {
            throw ThrowExceptionProductionUnitsEmpty();
        }

        KeyValuePair<ProductionUnitsEnum, ProductionUnit> productionUnitToWrite =
            ProductionUnits.FirstOrDefault(value => value.Value.Id == editedHeatingUnit.Id);

        ProductionUnits[productionUnitToWrite.Key] = editedHeatingUnit;
    }

    public override string ToString()
    {
        string strRepresentation = $"HeatingGridInformation:\n{HeatingGridInformation}";


        if (ProductionUnits != null)
        {
            strRepresentation += "ProductionUnits:\n";
            foreach ((ProductionUnitsEnum _, ProductionUnit prodUnit) in ProductionUnits)
            {
                strRepresentation += $"\t{prodUnit}\n";
            }
        }
        else
        {
            strRepresentation += "\nError: No string represantation of ProductionUnits(equals null)";
        }

        return strRepresentation;
    }
}
