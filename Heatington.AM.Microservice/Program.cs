using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.AssetManager;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Asset Manager with controllers and paths to assets
string pathToHeatingGrid =
    Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/HeatingGrid.json");
string pathToProductionUnits =
    Utilities.GeneratePathToFileInAssetsDirectory("AssetManager/ProductionUnits.json");

IReadWriteController heatingGridJsonController = new JsonController(pathToHeatingGrid);
IReadWriteController productionUnitsJsonController = new JsonController(pathToProductionUnits);
AssetManager assetManager =
    new AssetManager(
        heatingGridJsonController,
        productionUnitsJsonController
    );


app.MapGet("/", () => "Hello World!");
app.MapGet("/productionUnits", () => {
    Task loadAssets = assetManager.LoadAssets();
    loadAssets.Wait();
    return assetManager.ProductionUnits!.Values.ToList();
});

app.Run();
