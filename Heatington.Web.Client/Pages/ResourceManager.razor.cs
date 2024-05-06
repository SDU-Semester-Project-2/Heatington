using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Heatington.Web.Client.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Json;
using Heatington.AssetManager;
using Heatington.Models;

namespace Heatington.Web.Client.Pages
{
    public partial class ResourceManager : ComponentBase
    {
        [Inject] public required IDialogService DialogService { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [Inject] public ILogger<ResourceManager> Logger { get; set; }



        List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
        private bool _isLoading = false;
        //TODO: Take a look at this, should it be like this or just list because of GUID
        // public Dictionary<ProductionUnitsEnum, ProductionUnit> _productionUnits;




        async void OpenDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("ProductionUnits", _productionUnits);

            var dialog = DialogService.Show<AddBoilerDialog>("Add Production Unit", parameters,
                new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                StateHasChanged();
            }
        }

        // Method overload to open dialog with selected boiler
        async void OpenDialog(ProductionUnit productionUnit)
        {
            var parameters = new DialogParameters();
            parameters.Add("ProductionUnit", productionUnit);  // Pass selected unit

            var dialog = DialogService.Show<EditBoilerDialog>(($"Edit {productionUnit.Name}"), parameters,
                new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                StateHasChanged();
            }
        }


        private string DisplayData(double data) => Convert.ToString(data.ToString().Length != 0 ? data.ToString().Length : "No Data");
        private List<HeatDemandData> heatDemandDataList = new List<HeatDemandData>();

        public class HeatDemandData
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public float Value1 { get; set; }
            public float Value2 { get; set; }
        }

        private async Task ReadCsvDataOnLoad(string pathToFile)
        {
            Console.WriteLine(pathToFile);
            string csvContent = await File.ReadAllTextAsync(pathToFile);

            ParseCsvData(csvContent);
        }
        private async Task HandleCSVUpload(InputFileChangeEventArgs e)
        {
            var file = e.File;

            var reader = new StreamReader(file.OpenReadStream());
            var csvContent = await reader.ReadToEndAsync();

            ParseCsvData(csvContent);
        }
        private void ParseCsvData(string csvContent)
        {
            var rows = csvContent.Split('\n');

            // Use CultureInfo to handle potential localization issues with decimal point/comma
            var culture = CultureInfo.InvariantCulture;

            foreach (var row in rows)
            {
                var values = row.Split(',');
                if (values.Length == 4)
                {
                    heatDemandDataList.Add(new HeatDemandData
                    {
                        StartDate = DateTime.Parse(values[0], culture),
                        EndDate = DateTime.Parse(values[1], culture),
                        Value1 = float.Parse(values[2], culture),
                        Value2 = float.Parse(values[3], culture),
                    });
                }
            }

            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Logger.LogInformation("OnInitializedAsync started.");

                await base.OnInitializedAsync();

                var productionUnitsArray = await Http.GetFromJsonAsync<ProductionUnit[]>("http://localhost:5271/api/productionunits");

                if (productionUnitsArray != null)
                {
                    _productionUnits = productionUnitsArray.ToList();
                    Logger.LogInformation($"Fetched {_productionUnits.Count} units.");
                    foreach (var unit in _productionUnits)
                    {
                        Logger.LogInformation($"Unit: {unit.Name}, MaxHeat: {unit.MaxHeat}, MaxElectricity: {unit.MaxElectricity}");
                    }
                }

                var pathToProjectRootDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent;

                if (pathToProjectRootDirectory != null)
                {
                    string pathToFile = Path.Combine(pathToProjectRootDirectory.FullName, "Heatington.Web.Client/wwwroot/Assets/Data/winter-data.csv");
                    await ReadCsvDataOnLoad(pathToFile);
                }

                StateHasChanged();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, "Error occurred in OnInitializedAsync.");
            }
        }

    }
}
