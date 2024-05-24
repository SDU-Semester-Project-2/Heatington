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

    public static async Task<List<DataPoint>?> GetDataPoints(string season)
    {
        try
        {
            string uri = $"http://localhost:5165/api/TimeSeriesData?season={season}";

            using HttpResponseMessage res = await Program.Client.GetAsync(uri);
            res.EnsureSuccessStatusCode();

            List<DataPoint>? dataPoints;

            dataPoints = await res.Content.ReadFromJsonAsync<List<DataPoint>>();

            return dataPoints ?? new List<DataPoint>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }
}
