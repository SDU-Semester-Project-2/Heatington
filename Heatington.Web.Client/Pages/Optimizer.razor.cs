using System.Net.Http.Json;
using Heatington.Models;
using Heatington.Optimizer;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Heatington.Web.Client.Pages
{
    public partial class Optimizer : ComponentBase
    {
        private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
        public List<FormatedResultHolder>? resultData;

        // TODO:Replace it
        public bool SwitchState { get; set; } = true;

        [Inject] public HttpClient? Http { get; set; }
        [Inject] public ILogger<Optimizer>? Logger { get; set; }
        bool IsDataLoaded;

        public string SelectedScenario { get; set; } = "Scenario 1";

        public string SelectedSeason { get; set; } = "Winter";

        private int SelectedScenarioInt
        {
            get
            {
                switch (SelectedScenario)
                {
                    case "Scenario 1":
                        return (int)OptimizationMode.Scenario1;
                    case "Scenario 2":
                        return (int)OptimizationMode.Scenario2;
                    case "Scenario 3 (CO2)":
                        return (int)OptimizationMode.Co2;
                    default:
                        return -1;
                }
            }
        }


        public async Task OnParamChange()
        {
            IsDataLoaded = false;
            // Console.WriteLine("Loading data");
            // Console.WriteLine(SelectedSeason.ToLower());

            resultData = await LoadOptimizer(SelectedSeason.ToLower(), SelectedScenarioInt);
            IsDataLoaded = true;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Logger?.LogInformation("OnInitializedAsync in Optimizer.razor started");

                await base.OnInitializedAsync();
                _productionUnits = await LoadProductionUnits();

                resultData = await LoadOptimizer(SelectedSeason.ToLower(), (int)OptimizationMode.Scenario1);
                IsDataLoaded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GenerateOptApiUri(string season, int mode)
        {
            return $"http://localhost:5143/api/ResultDataManager?season={season}&mode={mode}";
        }

        private static string GenerateOptCsvDataUri(string season, int mode)
        {
            return $"http://localhost:5143/api/CsvFormat?season={season}&mode={mode}";
        }

        private async Task<List<FormatedResultHolder>?> LoadOptimizer(string season, int mode)
        {
            try
            {
                string uri = GenerateOptApiUri(season, mode);

                if (Http == null)
                {
                    throw new Exception();
                }

                List<FormatedResultHolder>? rawResultData =
                    await Http.GetFromJsonAsync<List<FormatedResultHolder>>(uri);

                return rawResultData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<List<ProductionUnit>> LoadProductionUnits()
        {
            try
            {
                if (Http == null)
                {
                    throw new Exception();
                }

                ProductionUnit[]? productionUnitsArray = await Http.GetFromJsonAsync<ProductionUnit[]>
                    ("http://localhost:5271/api/productionunits");

                return productionUnitsArray?.ToList() ?? new List<ProductionUnit>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private async Task DownloadCsv()
        {
            string? csvContent;
            try
            {
                if (Http == null)
                {
                    throw new Exception("Http is null!");
                }

                string uri = GenerateOptCsvDataUri(SelectedSeason.ToLower(), SelectedScenarioInt);
                csvContent = await Http.GetFromJsonAsync<string>(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            if (csvContent != null)
            {
                // Trigger file download
                await JSRuntime.InvokeVoidAsync("downloadFile", "data.csv", csvContent);
            }
        }
    }
}
