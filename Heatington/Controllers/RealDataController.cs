using Heatington.Models;
using System.Text.Json;
using Heatington.Controllers;
using Heatington.Services.Interfaces;
using Heatington.Helpers;

namespace Heatington.Controllers
{
    class RealDataContorller : IDataSource
    {
        private EnerginetApiContorller _apiContorller;

        public RealDataContorller(){
            _apiContorller=new();
        }


        public async Task<List<DataPoint>?> GetDataAsync()
        {
            try{
                string fileName = "winter_period.csv";
                string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);
                CsvController staticDataController = new CsvController(filePath);
                List<DataPoint>? staticData = await staticDataController.GetDataAsync();
                if(staticData is null || staticData.Count < 0){
                    throw new Exception("Didn't get data from API.");
                }
                else{
                    List<double> electricityPrices = await GetElectricityPricesAsync(staticData[0].StartTime, staticData[^1].EndTime, "DK2");
                    electricityPrices.Reverse();
                    return staticData.Zip(electricityPrices, (data, price) => new DataPoint(data.StartTime, data.EndTime, data.HeatDemand, price)).ToList();
                }
            }
            catch(Exception e){
                Utilities.DisplayException(e.Message);
                throw;
            }
        }

        public void SaveData(List<DataPoint> data){
            string filePath = data[0].StartTime.ToString("yyyy_MM_dd_hh:mm")+'-'+data[^1].EndTime.ToString("yyyy_MM_dd_hh:mm");
            CsvController controller = new CsvController(filePath);
            controller.SaveData(data);
        }

        public async Task<List<double>> GetElectricityPricesAsync(DateTime start, DateTime end, string priceArea)
        {
            Dictionary<string, string[]> filters = new();
            filters.Add("PriceArea", new string[] {priceArea});
            string[] columns = new string[] {"HourDK","PriceArea","SpotPriceDKK"};
            string rawJson = await _apiContorller.ProcessRepositoriesAsync("Elspotprices", start, end, filters, columns);
            JsonDocument responseJson = JsonDocument.Parse(rawJson);
            JsonElement records = responseJson.RootElement.GetProperty("records");
            return records.EnumerateArray().Select((x) => x.GetProperty("SpotPriceDKK").GetDouble()).ToList();
        }
    }

}
