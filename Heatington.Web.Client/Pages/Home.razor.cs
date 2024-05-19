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
    private static List<ChartData> _heatDemandWinterSeries;
    private static List<ChartData> _heatDemandSummerSeries;
    private static List<ChartData> _electricityPriceWinterSeries;
    private static List<ChartData> _electricityPriceSummerSeries;
    private List<ProductionUnit> _productionUnits = [];

    public ChartOptions HeatAndElectricityChartOptions = new ChartOptions { YAxisTicks = 1 };

    // Charts
    private int Index = -1;

    private bool isDataReady = false;


    //TODO: Use Real Data!
    private double totalCO2Produced = new Random().Next(20000, 50000);
    private double TotalExpenses = new Random().Next(50000, 100000);
    private double totalHeatProduced = new Random().Next(5000, 10000);
    private double totalProfit = new Random().Next(100000, 200000);

    public List<ChartSeries> HeatDemandSeries { get; set; }
    public List<ChartSeries> ElectricityPriceSeries { get; set; }
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

            _heatDemandWinterSeries = [];
            _heatDemandSummerSeries = [];
            _electricityPriceSummerSeries = [];
            _electricityPriceWinterSeries = [];

            List<DataPoint> winterData =
                await Http.GetFromJsonAsync<List<DataPoint>>("http://localhost:5165/api/TimeSeriesData?season=winter");
            ;
            _heatDemandWinterSeries = GetHeatDemandSeries(winterData);
            _electricityPriceWinterSeries = GetElectricityPriceSeries(winterData);

            List<DataPoint> summerData =
                await Http.GetFromJsonAsync<List<DataPoint>>("http://localhost:5165/api/TimeSeriesData?season=summer");
            ;
            _heatDemandSummerSeries = GetHeatDemandSeries(summerData);
            _electricityPriceSummerSeries = GetElectricityPriceSeries(summerData);


            InitializeHeatDemandChartData();
            InitializeElectricityPriceChartData();
            isDataReady = true;

            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    void InitializeHeatDemandChartData()
    {
        if (_heatDemandWinterSeries == null || _heatDemandWinterSeries.Count == 0 ||
            _heatDemandSummerSeries == null || _heatDemandSummerSeries.Count == 0)
        {
            return;
        }

        HeatDemandSeries = new List<ChartSeries>()
        {
            new ChartSeries() { Name = "Winter", Data = _heatDemandWinterSeries.Select(x => x.YData).ToArray() },
            new ChartSeries() { Name = "Summer", Data = _heatDemandSummerSeries.Select(x => x.YData).ToArray() }
        };
        XAxisLabels = Enumerable.Range(1, _heatDemandWinterSeries.Count)
            .Select(i => i % 12 == 0 ? $"T+{i}" : string.Empty)
            .ToArray();
    }

    void InitializeElectricityPriceChartData()
    {
        if (_electricityPriceWinterSeries == null || _electricityPriceWinterSeries.Count == 0 ||
            _electricityPriceSummerSeries == null || _electricityPriceSummerSeries.Count == 0)
        {
            return;
        }

        ElectricityPriceSeries = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Winter", Data = _electricityPriceWinterSeries.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer", Data = _electricityPriceSummerSeries.Select(x => x.YData).ToArray()
            }
        };
        XAxisLabels = Enumerable.Range(1, _electricityPriceWinterSeries.Count)
            .Select(i => i % 12 == 0 ? $"T+{i}" : string.Empty)
            .ToArray();
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
    private void LogHeatDemandSeriesData(List<ChartData> seriesData)
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

    private List<ChartData> GetHeatDemandSeries(List<DataPoint> dataPoints)
    {
        var heatDemandChartDataList = dataPoints.Select(dataPoint => new ChartData
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

    private List<ChartData> GetElectricityPriceSeries(List<DataPoint> dataPoints)
    {
        var electricityPriceDataList = dataPoints.Select(dataPoint => new ChartData()
        {
            XData = FormatDate(dataPoint.StartTime), YData = dataPoint.ElectricityPrice
        }).ToList();

        return electricityPriceDataList;
    }


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

    public class ChartData
    {
        public string XData { get; set; }
        public double YData { get; set; }
    }
}
