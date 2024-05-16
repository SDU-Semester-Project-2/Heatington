using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Services.Interfaces;
using Heatington.SourceDataManager;

namespace Heatington.Microservice.OPT.Services;

public static class DependenciesService
{
    public static async Task<List<ProductionUnit>> GetProductionUnits()
    {
        try
        {
            string uri = "http://localhost:5271/api/productionunits";

            using HttpResponseMessage res = await Program.Client.GetAsync(uri);
            res.EnsureSuccessStatusCode();

            List<ProductionUnit>? productionUnits;

            productionUnits = await res.Content.ReadFromJsonAsync<List<ProductionUnit>>();

            return productionUnits ?? new List<ProductionUnit>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }

    public static async Task<List<DataPoint>?> GetDataPoints()
    {
        try
        {
            // string uri = "http://www.contoso.com/";
            // string uri = "http://localhost:5012";
            // using HttpResponseMessage response = await Program.Client.GetAsync(uri);
            // response.EnsureSuccessStatusCode();
            // string responseBody = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);

            //TODO: CHANGE TO API CALL WHEN API IS READY
            string fileName = "winter_period.csv";
            string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
            IDataSource dataSource = new CsvController(filePath);
            SDM sdm = new(dataSource);
            await sdm.FetchTimeSeriesDataAsync();

            return sdm.TimeSeriesData;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }
}
