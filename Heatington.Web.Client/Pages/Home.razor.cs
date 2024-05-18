using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using Heatington.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;


namespace Heatington.Web.Client.Pages;

public partial class Home : ComponentBase
{
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


    private List<ProductionUnit> _productionUnits = [];


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

    [Inject] public HttpClient Http { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public ILogger<Home> Logger { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Logger.LogInformation("OnInitializedAsync in Home.razor started");
            await base.OnInitializedAsync();

            ProductionUnit[]? productionUnitsArray =
                await Http.GetFromJsonAsync<ProductionUnit[]>("http://localhost:5271/api/productionunits");

            if (productionUnitsArray != null)
            {
                _productionUnits = productionUnitsArray.ToList();
            }

            Logger.LogInformation($"Fetched {_productionUnits.Count} units.");
            foreach (ProductionUnit productionUnit in _productionUnits)
            {
                Logger.LogInformation($"Production unit: {productionUnit.Name}, " +
                                      $"MaxHeat: {productionUnit.MaxHeat}, " +
                                      $"MaxElectricity: {productionUnit.MaxElectricity}, " +
                                      $"ProductionCost: {productionUnit.ProductionCost}, " +
                                      $"CO2 emissions: {productionUnit.Co2Emission}, " +
                                      $"Primary energy consumption: {productionUnit.GasConsumption}");
            }

            RandomizeData();
            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void ViewMore(ProductionUnit productionUnit)
    {
        NavManager.NavigateTo("/resource-manager");
    }

    private void GoTo(string pageName)
    {
        NavManager.NavigateTo($"/{pageName}");
    }

    private string DisplayData(string data) => data.Length != 0 ? data : "No Data";


    private void RandomizeData()
    {
        var newSeries = new List<ChartSeries>();
        foreach (var productionUnit in _productionUnits)
        {
            var series = new ChartSeries() { Name = productionUnit.Name, Data = new double[7] };
            for (int i = 0; i < 7; i++)
                series.Data[i] = random.NextDouble() * 100;
            newSeries.Add(series);
        }

        _seriesBoilers = newSeries;
        StateHasChanged();
    }

    // Trying to implement sorting TODO: not working on mobile version?!
    public async Task<TableData<ProductionUnit>> GetBoilersAsync(TableState state)
    {
        Func<ProductionUnit, object> sortBy = null;
        switch (state.SortLabel)
        {
            case "Max Heat":
                sortBy = (boiler) => boiler.MaxHeat;
                break;
            case "Production Costs":
                sortBy = (boiler) => boiler.ProductionCost;
                break;
            case "CO2 Emissions":
                sortBy = (boiler) => boiler.Co2Emission;
                break;
            case "Primary Energy Consumption":
                sortBy = (boiler) => boiler.GasConsumption;
                break;
            default:
                sortBy = (boiler) => boiler.Name;
                break;
        }

        var sorted = state.SortDirection switch
        {
            SortDirection.Ascending => _productionUnits.OrderBy(sortBy).ToList(),
            SortDirection.Descending => _productionUnits.OrderByDescending(sortBy).ToList(),
            _ => _productionUnits
        };

        return await Task.FromResult(new TableData<ProductionUnit>
        {
            TotalItems = sorted.Count,
            Items = sorted.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList()
        });
    }
}
