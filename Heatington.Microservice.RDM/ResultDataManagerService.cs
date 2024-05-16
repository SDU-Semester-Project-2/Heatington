// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Models;

namespace Heatington.Microservice.RDM;

public class ResultDataManagerService
{
    public async Task<List<ResultHolder>?> GetDataFromOpt()
    {
        //Http Client
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:5019");
        string apiPath = "/api/Optimizer";


        Uri optUri = new Uri("http://localhost:5019/api/Optimizer");

        //Call Opt Api
        List<ResultHolder>? repliedResults = new();

        var response = await httpClient.GetAsync(apiPath);

        if (response.IsSuccessStatusCode)
        {
            repliedResults = await response.Content.ReadFromJsonAsync<List<ResultHolder>>();
        }
        else
        {
            Console.WriteLine("We fucked up");
        }

        return repliedResults;
    }
}
