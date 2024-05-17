using Heatington.Models;

namespace Heatington.Microservice.RDM;

public class ResultDataManagerService
{
    public async Task<List<ResultHolder>?> GetDataFromOpt()
    {
        List<ResultHolder>? repliedResults = new();

        //Http Client
        using var httpClient = new HttpClient();
        string uri = "http://localhost:5019/api/Optimizer";

        //Call Opt Api
        var response = await httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        repliedResults = await response.Content.ReadFromJsonAsync<List<ResultHolder>>();

        //return value
        return repliedResults;
    }
}
