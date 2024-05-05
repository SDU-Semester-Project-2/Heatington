using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;

namespace Heatington.Tests;

public class AssetManagerTest
{

    private readonly AssetManager.AssetManager _assetManager;

    public AssetManagerTest()
    {
        string pathToHeatingGrid =
            Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
        string pathToProductionUnits =
            Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

        IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
        IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);

        _assetManager = new AssetManager.AssetManager(heatingGridJsonController, productionUnitsJsonController);
    }

    [Fact]
    public void AsyncLoadAssets_LoadsHeatingGridCorrectly()
    {
        //Arrange
        HeatingGrid expectedHeatingGrid = new HeatingGrid("AssetManager/heating-grid.png",
            "Heatington Grid / Heating Area");
        HeatingGrid loadedHeatingGrid;

        //Act
        Task loadAssets = _assetManager.LoadAssets();
        loadAssets.Wait();
        loadedHeatingGrid = _assetManager.HeatingGridInformation;

        //Assert
        Assert.NotNull(loadedHeatingGrid);
        Assert.Equivalent(expectedHeatingGrid, loadedHeatingGrid);
    }

    [Fact]
    public void AsyncLoadAssets_LoadsProductionUnitsCorrectly()
    {
        

    }
}
