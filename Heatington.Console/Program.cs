namespace Heatington.Console
{
    internal static class Program
    {
        // TODO: Rewrite this method and implement the actual application logic
        static async Task Main(string[] args) //DONT REMOVE asnyc Task
        {
            //DONT REMOVE IT'S IMPORTANT FOR THE DOCUMENTATION SERVER
            await RunDocFx();
        }
        private static async Task RunDocFx()
        {
            //updates the documentation on dotnet run
            await Docfx.Docset.Build("../docfx.json");
        }
    }
}
