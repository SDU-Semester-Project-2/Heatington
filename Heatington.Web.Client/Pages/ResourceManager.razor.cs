using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Heatington.Web.Client.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Heatington.Web.Client.Pages
{
    public partial class ResourceManager : ComponentBase
    {
        [Inject] public required IDialogService DialogService { get; set; }

        //TODO: fix it in future te
        readonly List<Boiler> _boilers =
        [
            new Boiler
            {
                Name = "Gas Boiler",
                ImagePath = "gas-boiler",
                MaxHeat = "5.0 MW",
                MaxElectricity = "",
                ProductionCosts = "500 DKK / MWh(th)",
                CO2Emissions = "215 kg / MWh(th)",
                PrimaryEnergy = "1,1 MWh(gas) / MWh(th)"
            },

            new Boiler
            {
                Name = "Oil Boiler",
                ImagePath = "oil-boiler",
                MaxHeat = "4.0 MW",
                MaxElectricity = "",
                ProductionCosts = "700 DKK / MWh(th)",
                CO2Emissions = "265 kg / MWh(th)",
                PrimaryEnergy = "1,2 MWh(oil) / MWh(th)"
            },

            new Boiler
            {
                Name = "Gas Motor",
                ImagePath = "gas-motor",
                MaxHeat = "3.6 MW",
                MaxElectricity = "2.7 MW",
                ProductionCosts = "1,100 DKK / MWh(th)",
                CO2Emissions = "640 kg / MWh(th)",
                PrimaryEnergy = "1.9 MWh(gas) / MWh(th)"
            },

            new Boiler
            {
                Name = "Electric Boiler",
                ImagePath = "electric-boiler",
                MaxHeat = "8.0 MW",
                MaxElectricity = "-8.0 MW",
                ProductionCosts = "50 DKK / MWh(th)",
                CO2Emissions = "", // As no value provided
                PrimaryEnergy = "" // As no value provided
            }
        ];

        public class Boiler
        {
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public string MaxHeat { get; set; }
            public string MaxElectricity { get; set; }
            public string ProductionCosts { get; set; }
            public string CO2Emissions { get; set; }
            public string PrimaryEnergy { get; set; }
            public SortDirection? SortDirection { get; set; }
            public string? ImageData { get; set; }
        }

        async void OpenDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("Boilers", _boilers);

            var dialog = DialogService.Show<AddBoilerDialog>("Add Boiler", parameters,
                new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                StateHasChanged();
            }
        }

        // Method overload to open dialog with selected boiler
        async void OpenDialog(Boiler boiler)
        {
            var parameters = new DialogParameters();
            parameters.Add("Boiler", boiler); // Pass selected boiler

            var dialog = DialogService.Show<EditBoilerDialog>(($"Edit {boiler.Name}"), parameters,
                new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true });

            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                StateHasChanged();
            }
        }


        private string DisplayData(string data) => data.Length != 0 ? data : "No Data";
        private List<HeatDemandData> heatDemandDataList = new List<HeatDemandData>();

        public class HeatDemandData
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public float Value1 { get; set; }
            public float Value2 { get; set; }
        }


        // TODO: fix csv data loading one and then disappering
        //Maybe this can help
        //https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle?view=aspnetcore-7.0#stateful-reconnection-after-prerendering
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StateHasChanged();
            DirectoryInfo? pathToProjectRootDirectory =
                Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent;

            if (pathToProjectRootDirectory != null)
            {
                StateHasChanged();
                string pathToFile = Path.Combine(pathToProjectRootDirectory.FullName,
                    "Heatington.Web.Client/wwwroot/Assets/Data/winter-data.csv");
                await ReadCsvDataOnLoad(pathToFile);
            }
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

    }
}
