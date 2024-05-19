/// TODO:
/// - Get the statistics all in one or for 6 scenarios individually
/// - Operation Points
/// - CO2
/// - Fetching is done for all 6, you have to write your own formatting and init methods
/// - TLDR: We need Operation Points graph, CO2 Emission Graphs for all 6 scenarios. After that it should be easy to
///   calculate statistics by using the formatted data.

using MudBlazor;
using System.Net.Http.Json;
using Heatington.Models;
using Heatington.Optimizer;
using Microsoft.AspNetCore.Components;

namespace Heatington.Web.Client.Pages;

public partial class Home : ComponentBase
{
    private static List<ChartData>? _heatDemandWinterSeries;
    private static List<ChartData>? _heatDemandSummerSeries;

    private static List<ChartData>? _electricityPriceWinterSeries;
    private static List<ChartData>? _electricityPriceSummerSeries;

    private static List<ChartData>? _productionCostScenario1Winter;
    private static List<ChartData>? _productionCostScenario1Summer;
    private static List<ChartData>? _productionCostScenario2Winter;
    private static List<ChartData>? _productionCostScenario2Summer;
    private static List<ChartData>? _productionCostScenarioCo2Winter;
    private static List<ChartData>? _productionCostScenarioCo2Summer;

    private List<ProductionUnit> _productionUnits = [];
    public ChartOptions HeatAndElectricityChartOptions = new ChartOptions { YAxisTicks = 1 };
    private int Index = -1;
    private bool isDataReady = false;
    public ChartOptions ProductionCostChartOptions = new ChartOptions();
    public ChartOptions Co2EmissionChartOptions = new ChartOptions();


    //TODO: Use Real Data!
    private double totalCO2Produced = new Random().Next(20000, 50000);
    private double TotalExpenses = new Random().Next(50000, 100000);
    private double totalHeatProduced = new Random().Next(5000, 10000);
    private double totalProfit = new Random().Next(100000, 200000);

    private List<ChartSeries> HeatDemandSeries { get; set; }
    private List<ChartSeries> ElectricityPriceSeries { get; set; }
    private List<ChartSeries> ProductionCostSeries { get; set; }
    private List<ChartSeries> Co2EmissionSeries { get; set; }

    private string[] XAxisLabels { get; set; }

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

            List<DataPoint>? winterData =
                await Http.GetFromJsonAsync<List<DataPoint>>("http://localhost:5165/api/TimeSeriesData?season=winter");
            _heatDemandWinterSeries = GetHeatDemandSeries(winterData);
            _electricityPriceWinterSeries = GetElectricityPriceSeries(winterData);

            List<DataPoint>? summerData =
                await Http.GetFromJsonAsync<List<DataPoint>>("http://localhost:5165/api/TimeSeriesData?season=summer");
            _heatDemandSummerSeries = GetHeatDemandSeries(summerData);
            _electricityPriceSummerSeries = GetElectricityPriceSeries(summerData);

            // Winter period
            string baseWinterOptUri = "http://localhost:5019/api/optimizer?season=winter&mode=";
            string optWinterScenario1Uri = $"{baseWinterOptUri}{(int)OptimizationMode.Scenario1}";
            string optWinterScenario2Uri = $"{baseWinterOptUri}{(int)OptimizationMode.Scenario2}";
            string optWinterCO2Uri = $"{baseWinterOptUri}{(int)OptimizationMode.Co2}";

            // Summer period
            string baseSummerOptUri = "http://localhost:5019/api/optimizer?season=summer&mode=";
            string optSummerScenario1Uri = $"{baseSummerOptUri}{(int)OptimizationMode.Scenario1}";
            string optSummerScenario2Uri = $"{baseSummerOptUri}{(int)OptimizationMode.Scenario2}";
            string optSummerCO2Uri = $"{baseSummerOptUri}{(int)OptimizationMode.Co2}";

            List<ResultHolder>? rawResultDataWinterScenario1 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterScenario1Uri);
            _productionCostScenario1Winter = GetProductionCostSeries(rawResultDataWinterScenario1);
            Logger.LogInformation(_productionCostScenario1Winter.ToString());

            List<ResultHolder>? rawResultDataWinterScenario2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterScenario2Uri);
            _productionCostScenario2Winter = GetProductionCostSeries(rawResultDataWinterScenario2);

            List<ResultHolder>? rawResultDataWinterScenarioCo2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterCO2Uri);
            _productionCostScenarioCo2Winter = GetProductionCostSeries(rawResultDataWinterScenarioCo2);

            List<ResultHolder>? rawResultDataSummerScenario1 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerScenario1Uri);
            _productionCostScenario1Summer = GetProductionCostSeries(rawResultDataSummerScenario1);

            List<ResultHolder>? rawResultDataSummerScenario2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerScenario2Uri);
            _productionCostScenario2Summer = GetProductionCostSeries(rawResultDataSummerScenario2);

            List<ResultHolder>? rawResultDataSummerScenarioCo2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerCO2Uri);
            _productionCostScenarioCo2Summer = GetProductionCostSeries(rawResultDataSummerScenarioCo2);


            InitializeProductionCostChartData();
            InitializeHeatDemandChartData();
            InitializeCo2EmissionChartData();
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

    void InitializeProductionCostChartData()
    {
        ProductionCostSeries = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Winter Scenario 1",
                Data = _productionCostScenario1Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Winter Scenario 2",
                Data = _productionCostScenario2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Winter Scenario Co2",
                Data = _productionCostScenarioCo2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario 1",
                Data = _productionCostScenario1Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario 2",
                Data = _productionCostScenario2Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario Co2",
                Data = _productionCostScenarioCo2Summer!.Select(x => x.YData).ToArray()
            }
        };
        XAxisLabels = Enumerable.Range(1, _productionCostScenario1Winter.Count)
            .Select(i => i % 12 == 0 ? $"T+{i}" : string.Empty)
            .ToArray();
    }

    void InitializeCo2EmissionChartData()
    {
        Co2EmissionSeries = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Winter Scenario 1",
                Data = _productionCostScenario1Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Winter Scenario 2",
                Data = _productionCostScenario2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Winter Scenario Co2",
                Data = _productionCostScenarioCo2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario 1",
                Data = _productionCostScenario1Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario 2",
                Data = _productionCostScenario2Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario Co2",
                Data = _productionCostScenarioCo2Summer!.Select(x => x.YData).ToArray()
            }
        };
        XAxisLabels = Enumerable.Range(1, _productionCostScenario1Winter.Count)
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

        return productionUnits;
    }

    private List<ChartData>? GetHeatDemandSeries(List<DataPoint>? dataPoints)
    {
        var heatDemandChartDataList = dataPoints?.Select(dataPoint => new ChartData
        {
            XData = FormatDate(dataPoint.StartTime), YData = dataPoint.HeatDemand
        }).ToList();

        return heatDemandChartDataList;
    }

    private List<ChartData>? GetElectricityPriceSeries(List<DataPoint>? dataPoints)
    {
        var electricityPriceDataList = dataPoints?.Select(dataPoint => new ChartData()
        {
            XData = FormatDate(dataPoint.StartTime), YData = dataPoint.ElectricityPrice
        }).ToList();

        return electricityPriceDataList;
    }

    private List<ChartData>? GetProductionCostSeries(List<ResultHolder>? rawResultData)
    {
        var productionCostDataList = rawResultData?.Select(item => new ChartData()
        {
            XData = FormatDate(item.StartTime), YData = item.NetProductionCost
        }).ToList();

        return productionCostDataList;
    }


    // TimeZone data is not included with .NET's WebAssembly runtime.
    // To bypass this, we directly convert UTC DateTime to Danish Time (CET/CEST) during series generation.
    // This offset-based conversion ensures we avoid timezone exceptions and retain compatibility across different OS.

    private string FormatDate(DateTime datetime)
    {
        var offset = TimeSpan.FromHours(2); // Offset for Central European Summer (+2 GMT)
        DateTime datetimeInDanish = datetime.ToUniversalTime().Add(offset);
        string formattedDate = datetimeInDanish.ToString("yyyy-MM-ddTHH:mm:ss");
        return formattedDate;
    }

    private void ViewMore(ProductionUnit productionUnit)
    {
        NavManager.NavigateTo("/resource-manager");
    }

    public class ChartData
    {
        public string? XData { get; set; }
        public double YData { get; set; }
    }
}
