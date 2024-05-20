// Imports omitted for brevity

using System.Net.Http.Json;
using Heatington.Models;
using Heatington.Optimizer;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Heatington.Web.Client.Pages
{
    public partial class Optimizer : ComponentBase
    {
        private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
        private string _selectedScenario = "Scenario 1";
        private string _selectedSeason = "Winter";
        public List<ResultHolder>? resultData;

        // TODO:Replace it
        public bool SwitchState { get; set; } = true;

        [Inject] public HttpClient Http { get; set; }
        [Inject] public ILogger<Optimizer> Logger { get; set; }
        public bool isDataLoaded = false;

        public string SelectedScenario
        {
            get => _selectedScenario;
            set
            {
                if (_selectedScenario != value)
                {
                    _selectedScenario = value;
                }
            }
        }

        public string SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                if (_selectedSeason != value)
                {
                    _selectedSeason = value;
                }
            }
        }


        public async Task OnParamChange()
        {
            isDataLoaded = false;
            Console.WriteLine("Loading data");
            Console.WriteLine(SelectedSeason.ToLower());
            switch (SelectedScenario)
            {
                case "Scenario 1":
                    resultData = await LoadOptimizer(SelectedSeason.ToLower(), (int)OptimizationMode.Scenario1);
                    isDataLoaded = true;
                    break;
                case "Scenario 2":
                    resultData = await LoadOptimizer(SelectedSeason.ToLower(), (int)OptimizationMode.Scenario2);
                    isDataLoaded = true;
                    break;
                case "Scenario 3 (CO2)":
                    resultData = await LoadOptimizer(SelectedSeason.ToLower(), (int)OptimizationMode.Co2);
                    isDataLoaded = true;
                    break;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Logger.LogInformation("OnInitializedAsync in Optimizer.razor started");
                await base.OnInitializedAsync();
                _productionUnits = await LoadProductionUnits();

                resultData = await LoadOptimizer(SelectedSeason.ToLower(), (int)OptimizationMode.Scenario1);
                isDataLoaded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string generateOptAPIUri(string season, int mode)
        {
            return $"http://localhost:5019/api/optimizer?season={season}&mode={mode}";
        }

        private async Task<List<ResultHolder>?> LoadOptimizer(string season, int mode)
        {
            try
            {
                string uri = generateOptAPIUri(season, mode);
                List<ResultHolder>? rawResultData =
                    await Http.GetFromJsonAsync<List<ResultHolder>>(uri);

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
            ProductionUnit[] productionUnitsArray = await Http.GetFromJsonAsync<ProductionUnit[]>
                ("http://localhost:5271/api/productionunits");

            return productionUnitsArray?.ToList() ?? new List<ProductionUnit>();
        }
    }
}
