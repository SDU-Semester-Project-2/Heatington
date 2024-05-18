using Heatington.AssetManager;
using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;

namespace Heatington.Tests.AssetManager;

public class AssetManagerTest
{
    private readonly AM _assetManager;

    public AssetManagerTest()
    {
        string pathToHeatingGrid =
            Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
        string pathToProductionUnits =
            Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

        IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
        IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);

        _assetManager = new AM(heatingGridJsonController, productionUnitsJsonController);
    }

    [Fact]
    public async void AsyncLoadAssets_LoadsHeatingGridCorrectly()
    {
        //Arrange
        HeatingGrid expectedHeatingGrid = new HeatingGrid("AssetManager/heating-grid.png",
            "Heatington Grid / Heating Area");
        HeatingGrid? loadedHeatingGrid;

        //Act
        Task loadAssets = _assetManager.LoadAssets();
        await loadAssets;
        loadedHeatingGrid = _assetManager.HeatingGridInformation;

        //Assert
        Assert.NotNull(loadedHeatingGrid);
        Assert.Equivalent(expectedHeatingGrid, loadedHeatingGrid);
    }

    [Fact]
    public async void AsyncLoadAssets_LoadsProductionUnitsCorrectly()
    {
        //Arrange
        List<ProductionUnit> expectedProductionUnits = new List<ProductionUnit>()
        {
            new ProductionUnit("GB", "AssetManager/gas-boiler.jpg", 5, 500, 0, 1.1, 0),
            new ProductionUnit("OB", "AssetManager/oil-boiler.jpg", 4, 700, 0, 1.2, 265),
            new ProductionUnit("GM", "AssetManager/gas-motor.jpg", 3.6, 1100, 2.7, 1.9, 640),
            new ProductionUnit("EK", "AssetManager/electric-boiler.jpg", 8, 50, -8, 0, 0)
        };

        List<ProductionUnit> loadedProductionUnits;

        //Act
        Task loadAssets = _assetManager.LoadAssets();
        await loadAssets;
        loadedProductionUnits = _assetManager.ProductionUnits!.Values.ToList();

        //Assert
        Assert.NotNull(loadedProductionUnits);
        Assert.Equivalent(expectedProductionUnits, loadedProductionUnits);
    }

    [Fact]
    public async void AssetManager_ModifyingHeatUnitWorksCorrectly()
    {
        //Arrange
        ProductionUnit newProductionUnit = new ProductionUnit("TT", "", 77, 690, 0, 4.20, 0);

        //Act
        Task loadAssets = _assetManager.LoadAssets();
        await loadAssets;
        _assetManager.WriteHeatingUnit(_assetManager.ProductionUnits![(ProductionUnitsEnum)1].Id, newProductionUnit);

        //Assert
        Assert.Equivalent(newProductionUnit, _assetManager.ProductionUnits[(ProductionUnitsEnum)1]);
    }
}
