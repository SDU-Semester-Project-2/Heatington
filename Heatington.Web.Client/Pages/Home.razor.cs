using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;


namespace Heatington.Web.Client.Pages;

public partial class Home : ComponentBase
{
    readonly List<Boiler> _boilers =
    [
        new Boiler
        {
            Name = "Gas Boiler",
            MaxHeat = "5.0 MW",
            MaxElectricity = "",
            ProductionCosts = "500 DKK / MWh(th)",
            CO2Emissions = "215 kg / MWh(th)",
            PrimaryEnergy = "1,1 MWh(gas) / MWh(th)"
        },

        new Boiler
        {
            Name = "Oil Boiler",
            MaxHeat = "4.0 MW",
            MaxElectricity = "",
            ProductionCosts = "700 DKK / MWh(th)",
            CO2Emissions = "265 kg / MWh(th)",
            PrimaryEnergy = "1,2 MWh(oil) / MWh(th)"
        },

        new Boiler
        {
            Name = "Gas Motor",
            MaxHeat = "3.6 MW",
            MaxElectricity = "2.7 MW",
            ProductionCosts = "1,100 DKK / MWh(th)",
            CO2Emissions = "640 kg / MWh(th)",
            PrimaryEnergy = "1.9 MWh(gas) / MWh(th)"
        },

        new Boiler
        {
            Name = "Electric Boiler",
            MaxHeat = "8.0 MW",
            MaxElectricity = "-8.0 MW",
            ProductionCosts = "50 DKK / MWh(th)",
            CO2Emissions = "", // As no value provided
            PrimaryEnergy = "" // As no value provided
        }
    ];

    private readonly ChartOptions _options = new ChartOptions();


    // for electricity chart

    private readonly List<ChartSeries> _seriesElectricity = new List<ChartSeries>()
    {
        new ChartSeries()
        {
            Name = "Electricity Production", Data = new double[] { 110, 105, 115, 120, 108, 112, 118 },
        },
        new ChartSeries()
        {
            Name = "Electricity Consumption", Data = new double[] { 115, 100, 113, 118, 109, 115, 117 },
        },
        new ChartSeries() { Name = "Electricity Price", Data = new double[] { 50, 55, 60, 53, 57, 54, 59 }, }
    };

    private readonly string[] _xAxisLabels1 =
    {
        "01.01.2024", "02.01.2024", "03.01.2024", "04.01.2024", "05.01.2024", "06.01.2024", "07.01.2024"
    };

    private readonly string[] _xAxisLabels3 =
    {
        "01.01.2024", "02.01.2024", "03.01.2024", "04.01.2024", "05.01.2024", "06.01.2024", "07.01.2024"
    };

    readonly Random random = new Random();
    private int _indexBoilers = -1;
    private int _indexElectricity = -1;


    // for heat chart

    private int _indexHeat = -1;


    // for optimizer chart

    private List<ChartSeries> _seriesBoilers = new List<ChartSeries>();

    private List<ChartSeries> _seriesHeat = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "Heat Demand", Data = new double[] { 90, 79, 72, 69, 62, 68, 89 }, },
        new ChartSeries() { Name = "Heat Produced", Data = new double[] { 92, 78, 73, 75, 60, 68, 89 }, }
    };

    private double totalCO2Produced = new Random().Next(20000, 50000);

    private double TotalExpenses = new Random().Next(50000, 100000);
    private double totalHeatProduced = new Random().Next(5000, 10000);
    private double totalProfit = new Random().Next(100000, 200000);

    private void ViewMore(Boiler boiler)
    {
        NavManager.NavigateTo("/resource-manager");
    }

    private void GoTo(string pageName)
    {
        NavManager.NavigateTo($"/{pageName}");
    }

    private string DisplayData(string data) => data.Length != 0 ? data : "No Data";

    protected override void OnInitialized()
    {
        base.OnInitialized();
        RandomizeData();
    }

    private void RandomizeData()
    {
        var newSeries = new List<ChartSeries>();
        foreach (var boiler in _boilers)
        {
            var series = new ChartSeries() { Name = boiler.Name, Data = new double[7] };
            for (int i = 0; i < 7; i++)
                series.Data[i] = random.NextDouble() * 100;
            newSeries.Add(series);
        }

        _seriesBoilers = newSeries;
        StateHasChanged();
    }

    // Trying to implement sorting TODO: not working on mobile version?!
    public async Task<TableData<Boiler>> GetBoilersAsync(TableState state)
    {
        Func<Boiler, object> sortBy = null;
        switch (state.SortLabel)
        {
            case "Max Heat":
                sortBy = (boiler) => boiler.MaxHeat;
                break;
            case "Production Costs":
                sortBy = (boiler) => boiler.ProductionCosts;
                break;
            case "CO2 Emissions":
                sortBy = (boiler) => boiler.CO2Emissions;
                break;
            case "Primary Energy Consumption":
                sortBy = (boiler) => boiler.PrimaryEnergy;
                break;
            default:
                sortBy = (boiler) => boiler.Name;
                break;
        }

        var sorted = state.SortDirection switch
        {
            SortDirection.Ascending => _boilers.OrderBy(sortBy).ToList(),
            SortDirection.Descending => _boilers.OrderByDescending(sortBy).ToList(),
            _ => _boilers
        };

        return await Task.FromResult(new TableData<Boiler>
        {
            TotalItems = sorted.Count,
            Items = sorted.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList()
        });
    }

    public class Boiler
    {
        public string Name { get; set; }
        public string MaxHeat { get; set; }
        public string MaxElectricity { get; set; }
        public string ProductionCosts { get; set; }
        public string CO2Emissions { get; set; }
        public string PrimaryEnergy { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
}
