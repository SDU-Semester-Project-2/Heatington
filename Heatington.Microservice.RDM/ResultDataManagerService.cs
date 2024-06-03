using Heatington.Models;
using Heatington.Services.Serializers;

namespace Heatington.Microservice.RDM;

public static class ResultDataManagerService
{
    public static async Task<List<ResultHolder>?> GetDataFromOpt(string uri)
    {
        List<ResultHolder>? repliedResults;

        //Http Client
        using var httpClient = new HttpClient();

        //Call Opt Api
        var response = await httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        repliedResults = await response.Content.ReadFromJsonAsync<List<ResultHolder>>();

        //return value
        return repliedResults;
    }

    public static string FormatToCsv(List<FormatedResultHolder> data)
    {
        // csv data header
        string[] header =
            ["Start Time", "End Time", "Heat Demand", "Electricity Price", "Boiler", "Net Production Cost"];

        // format data
        CsvData csv = CsvData.Create<FormatedResultHolder>(data, header);

        // serialize it to a string
        string rawCsvData = CsvSerializer.Serialize(csv);

        return rawCsvData;
    }

    public static string GenerateOptimizerUri(int mode, string season)
    {
        string uri = $"http://localhost:5019/api/optimizer?season={season}&mode={mode}";
        return uri;
    }
}
