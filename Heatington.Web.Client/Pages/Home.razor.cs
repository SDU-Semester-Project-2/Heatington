using MudBlazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using Heatington.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Heatington.Services.Serializers;


namespace Heatington.Web.Client.Pages;

public partial class Home : ComponentBase
{
    private static List<HeatDemandChartData> _heatDemandWinterSeries;

    private List<ProductionUnit> _productionUnits = [];

    // Charts
    private int Index = -1;
    public ChartOptions Options = new ChartOptions();


    //TODO: Use Real Data!
    private double totalCO2Produced = new Random().Next(20000, 50000);
    private double TotalExpenses = new Random().Next(50000, 100000);
    private double totalHeatProduced = new Random().Next(5000, 10000);
    private double totalProfit = new Random().Next(100000, 200000);

    public List<ChartSeries> Series { get; set; }
    public string[] XAxisLabels { get; set; }

    [Inject] public HttpClient Http { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public ILogger<Home> Logger { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Logger.LogInformation("OnInitializedAsync in Home.razor started");
            await base.OnInitializedAsync();
            _productionUnits = await LoadProductionUnits();

            List<DataPoint> winterData = await LoadCsvData("Assets/Data/winter-data.csv");
            _heatDemandWinterSeries = GetHeatDemandSeries(winterData);
            LogHeatDemandSeriesData(_heatDemandWinterSeries);
            InitializeChartData();

            List<ChartSeries> winterElectricityDataSeries = GetElectricityPriceSeries(winterData);
            // List<DataPoint> summerData = await LoadCsvData("Assets/Data/summer-data.csv");
            // List<ChartSeries> summerHeatDataSeries = GetHeatDemandSeries(summerData);
            // List<ChartSeries> summerElectricityDataSeries = GetElectricityPriceSeries(summerData);

            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    void InitializeChartData()
    {
        if (_heatDemandWinterSeries == null)
        {
            throw new InvalidOperationException(
                "The heat demand winter series must be initialized before calling this method.");
        }

        Series = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Heat Demand", Data = _heatDemandWinterSeries.Select(x => x.YData).ToArray()
            },
        };

        XAxisLabels = _heatDemandWinterSeries.Select(x => x.XData).ToArray();
    }

    private async Task<List<ProductionUnit>> LoadProductionUnits()
    {
        ProductionUnit[]? productionUnitsArray =
            await Http.GetFromJsonAsync<ProductionUnit[]>("http://localhost:5271/api/productionunits");

        List<ProductionUnit> productionUnits = new List<ProductionUnit>();

        if (productionUnitsArray != null)
        {
            productionUnits = productionUnitsArray.ToList();
        }

        Logger.LogInformation($"Fetched {productionUnits.Count} units.");
        foreach (ProductionUnit productionUnit in productionUnits)
        {
            Logger.LogInformation($"Production unit: {productionUnit.Name}, " +
                                  $"MaxHeat: {productionUnit.MaxHeat}, " +
                                  $"MaxElectricity: {productionUnit.MaxElectricity}, " +
                                  $"ProductionCost: {productionUnit.ProductionCost}, " +
                                  $"CO2 emissions: {productionUnit.Co2Emission}, " +
                                  $"Primary energy consumption: {productionUnit.GasConsumption}");
        }

        return productionUnits;
    }

    // TODO: Testing purpose only, may delete this later
    private void LogHeatDemandSeriesData(List<HeatDemandChartData> seriesData)
    {
        var seriesDataStr = string.Join(", ",
            seriesData.Select(data => $"(XData: {data.XData}, YData: {data.YData})"));
        Logger.LogInformation($"_heatDemandWinterSeries: {seriesDataStr}");
    }


    private async Task<List<DataPoint>> LoadCsvData(string path)
    {
        try
        {
            string csvRawData = await Http.GetStringAsync(path);
            Logger.LogInformation($"Successfully fetched CSV data from {path}");
            CsvData csvData = CsvSerializer.Deserialize(csvRawData, true);
            Logger.LogInformation("Successfully deserialized CSV data to CsvData object");
            List<DataPoint> dataPoints = csvData.ConvertRecords<DataPoint>();
            Logger.LogInformation($"Successfully converted CsvData to List<DataPoint> with {dataPoints.Count} items");

            // Log entire list as one entry
            var dataPointsStr = string.Join(", ",
                dataPoints.Select(dp =>
                    $"(StartTime: {dp.StartTime}, EndTime: {dp.EndTime}, HeatDemand: {dp.HeatDemand}, ElectricityPrice: {dp.ElectricityPrice})"));
            Logger.LogInformation($"DataPoints: {dataPointsStr}");

            return dataPoints;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred when trying to load CSV data from {path}");
            throw;
        }
    }

    private List<HeatDemandChartData> GetHeatDemandSeries(List<DataPoint> dataPoints)
    {
        var heatDemandChartDataList = dataPoints.Select(dataPoint => new HeatDemandChartData
        {
            XData = FormatDate(dataPoint.StartTime), YData = dataPoint.HeatDemand
        }).ToList();

        // Logging each HeatDemandChartData
        // foreach (var data in heatDemandChartDataList)
        // {
        //     Logger.LogInformation($"GetXData: {data.XData}, GetYData: {data.YData}");
        // }

        return heatDemandChartDataList;
    }

    private List<ChartSeries> GetElectricityPriceSeries(List<DataPoint> dataPoints) =>
        dataPoints.Select(dataPoint =>
            new ChartSeries
            {
                // Format DateTime within the method
                Name = $"{FormatDate(dataPoint.StartTime)} - {FormatDate(dataPoint.EndTime)}",
                Data = new[] { dataPoint.ElectricityPrice }
            }).ToList();


    // TimeZone data is not included with .NET's WebAssembly runtime.
    // To bypass this, we directly convert UTC DateTime to Danish Time (CET/CEST) during series generation.
    // This offset-based conversion ensures we avoid timezone exceptions and retain compatibility across different OS.

    private string FormatDate(DateTime datetime)
    {
        var offset = TimeSpan.FromHours(2); // Offset for Central European Summer (+2 GMT)
        DateTime datetimeInDanish = datetime.ToUniversalTime().Add(offset);
        string formattedDate = datetimeInDanish.ToString("yyyy-MM-ddTHH:mm:ss");

        // Logger.LogInformation($"FormatDate: {formattedDate}"); // Logging formatted date
        return formattedDate;
    }


    // To many x-labels, want to clean it
    public IEnumerable<string> GetUniqueDays(IEnumerable<string> dates)
    {
        return dates
            .Select(d =>
                DateTime.ParseExact(d, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)) // Parse the datetime strings
            .Select(d => d.Date) // Select just the Date part of the DateTime
            .Distinct() // Get only unique dates
            .Select(d => d.ToString("dd.MM.yyyy")); // Format the DateTime as a string
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

    public class HeatDemandChartData
    {
        public string XData { get; set; }
        public double YData { get; set; }
    }
}
