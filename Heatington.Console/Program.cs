using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Optimizer;

namespace Heatington.Console
{
    internal static class Program
    {
        // TODO: Rewrite this method and implement the actual application logic
        static async Task Main(string[] args) //DONT REMOVE asnyc Task
        {
            //DONT REMOVE IT'S IMPORTANT FOR THE DOCUMENTATION SERVER
            await RunDocFx();

            AssetManager.AssetManager am = new AssetManager.AssetManager();

            string fileName = "winter_period.csv";
            string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
            IDataSource dataSource = new CsvController(filePath);
            SourceDataManager.SourceDataManager srm = new(dataSource);

            Opt opt = new Opt();

            ConsoleUI consoleUi = new ConsoleUI(am, srm, opt);

            consoleUi.StartUi();
        }

        private static async Task RunDocFx()
        {
            //updates the documentation on dotnet run
            await Docfx.Docset.Build("../docfx.json");
        }
    }
}
