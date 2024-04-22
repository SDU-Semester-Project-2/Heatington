using System.Net.Http.Headers;
using Heatington.Models;
using System.Text.Json;

namespace Heatington.Controllers
{
    class EnerginetApiContorller
    {
        private HttpClient _client;

        public EnerginetApiContorller(){
            _client = new();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*public static async Task DoSomething(){

            await ProcessRepositoriesAsync(client);

        }*/

        public async Task<string> ProcessRepositoriesAsync(string dataset, DateTime start, DateTime end, Dictionary<string, string[]>? filters = null, string[]? columns = null)
        {
            //Console.WriteLine("{" + String.Join(",", filters.Select(pair => pair.Key + ":[\"" + String.Join("\",\"", pair.Value)  + "\"]" )) + "}");
            filters = filters ?? new();
            columns = columns ?? new string[] {};
            string startString = "start=" + start.ToString("yyyy-MM-ddTHH:mm");
            string endString = "end=" + end.ToString("yyyy-MM-ddTHH:mm");
            string filterString = "filter={" +
                    String.Join(",", filters.Select(pair => '"' + pair.Key+ '"' + ":[\"" + String.Join("\",\"", pair.Value)  + "\"]" )) +
                "}";
            string columnsString = "columns=" + String.Join(",", columns);//HourDK,PriceArea,SpotPriceDKK";

            string rawJson = await _client.GetStringAsync(
                $"https://api.energidataservice.dk/dataset/" + dataset + '?' + startString+ '&' + endString + '&' + filterString +'&' + columnsString);
                /*"filter={\"PriceArea\":[\""+String.Join("\",\"", priceAreas)+"\"]}*/
            Console.WriteLine($"https://api.energidataservice.dk/dataset/" + dataset + '?' + startString+ '&' + endString + '&' + filterString +'&' + columnsString);
            //Console.WriteLine(rawJson);
            return rawJson;
            /*
            List<double> result;
            JsonDocument responseJson = JsonDocument.Parse(rawJson);
            JsonElement records = responseJson.RootElement.GetProperty("records");
            result = records.EnumerateArray().Select((x) => {return x.GetProperty("SpotPriceDKK").GetDouble();}).ToList();
            //result.Select((double x) => {Console.WriteLine(x); return x;}).ToList();
            return result;
            */
        }
    }

}
