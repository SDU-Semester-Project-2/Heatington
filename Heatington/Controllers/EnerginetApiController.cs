using System.Net.Http.Headers;
using System.Text.Json;
using Heatington.Models;

namespace Heatington.Controllers
{
    class EnerginetApiContorller
    {
        private HttpClient _client;

        public EnerginetApiContorller()
        {
            _client = new();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> ProcessRepositoriesAsync(string dataset, DateTime start, DateTime end, Dictionary<string, string[]>? filters = null, string[]? columns = null)
        {
            filters = filters ?? new();
            columns = columns ?? new string[] { };
            string startString = "start=" + start.ToString("yyyy-MM-ddTHH:mm");
            string endString = "end=" + end.ToString("yyyy-MM-ddTHH:mm");
            string filterString = "filter=%7B" +
                    String.Join("%2C", filters.Select(pair => "%22" + pair.Key + "%22" + "%3A%5B%22" + String.Join("%22%2C%22", pair.Value) + "%22%5D")) +
                "%7D";
            string columnsString = "columns=" + String.Join("%2C", columns);

            string rawJson = await _client.GetStringAsync(
                $"https://api.energidataservice.dk/dataset/" + dataset + '?' + startString + '&' + endString + '&' + filterString + '&' + columnsString);
            Console.WriteLine($"https://api.energidataservice.dk/dataset/" + dataset + '?' + startString + '&' + endString + '&' + filterString + '&' + columnsString);
            return rawJson;
        }
    }

}
