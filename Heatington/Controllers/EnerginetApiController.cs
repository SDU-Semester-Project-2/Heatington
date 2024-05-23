using System.Globalization;
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

        public async Task<string> ProcessRepositoriesAsync(string dataset, DateTime start, DateTime end,
            Dictionary<string, string[]>? filters = null, string[]? columns = null)
        {
            filters = filters ?? new();
            columns = columns ?? new string[] { };
            string startString = "start=" + start.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            string endString = "end=" + end.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            string filterString = "filter={" +
                                  String.Join(",",
                                      filters.Select(pair =>
                                          '"' + pair.Key + '"' + ":[\"" + String.Join("\",\"", pair.Value) + "\"]")) +
                                  "}";
            string columnsString = "columns=" + String.Join(",", columns);
            try
            {
                string rawJson = await _client.GetStringAsync(
                    $"https://api.energidataservice.dk/dataset/" + dataset + '?' + startString + '&' + endString + '&' +
                    filterString + '&' + columnsString);
                Console.WriteLine($"https://api.energidataservice.dk/dataset/" + dataset + '?' + startString + '&' +
                                  endString + '&' + filterString + '&' + columnsString);
                return rawJson;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
                throw;
            }
        }
    }
}
