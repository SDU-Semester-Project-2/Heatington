// Imports omitted for brevity

using System.Net.Http.Json;
using Heatington.Models;
using Microsoft.AspNetCore.Components;

namespace Heatington.Web.Client.Pages
{
    public partial class Optimizer : ComponentBase
    {
        private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
        private string _selectedScenario = "Scenario 1";
        private string _selectedSeason = "Winter";

        // TODO:Replace it
        public bool SwitchState { get; set; } = true;

        [Inject] public HttpClient Http { get; set; }
        [Inject] public ILogger<Optimizer> Logger { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Logger.LogInformation("OnInitializedAsync in Optimizer.razor started");
                await base.OnInitializedAsync();
                _productionUnits = await LoadProductionUnits();
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
