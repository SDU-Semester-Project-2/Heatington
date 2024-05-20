using MudBlazor;
using System.Net.Http.Json;
using Heatington.Models;
using Heatington.Optimizer;
using Microsoft.AspNetCore.Components;

namespace Heatington.Web.Client.Pages;

public partial class Home : ComponentBase
{
    // heat demand
    private static List<ChartData>? _heatDemandWinterSeries;
    private static List<ChartData>? _heatDemandSummerSeries;

    // electricity price
    private static List<ChartData>? _electricityPriceWinterSeries;
    private static List<ChartData>? _electricityPriceSummerSeries;

    // net production cost
    private static List<ChartData>? _netProductionCostScenario1Winter;
    private static List<ChartData>? _netProductionCostScenario1Summer;
    private static List<ChartData>? _netProductionCostScenario2Winter;
    private static List<ChartData>? _netProductionCostScenario2Summer;
    private static List<ChartData>? _netProductionCostScenarioCo2Winter;
    private static List<ChartData>? _netProductionCostScenarioCo2Summer;

    // co2 emission
    private Dictionary<string, List<ChartData>> _co2EmissionScenario1Summer;
    private Dictionary<string, List<ChartData>> _co2EmissionScenario1Winter;
    private Dictionary<string, List<ChartData>> _co2EmissionScenario2Summer;
    private Dictionary<string, List<ChartData>> _co2EmissionScenario2Winter;
    private Dictionary<string, List<ChartData>> _co2EmissionScenarioCo2Summer;
    private Dictionary<string, List<ChartData>> _co2EmissionScenarioCo2Winter;

    // operation points
    private List<ChartData>? _operationPointsScenario1Summer;
    private List<ChartData>? _operationPointsScenario1Winter;
    private List<ChartData>? _operationPointsScenario2Summer;
    private List<ChartData>? _operationPointsScenario2Winter;
    private List<ChartData>? _operationPointsScenarioCo2Summer;
    private List<ChartData>? _operationPointsScenarioCo2Winter;

    private List<ProductionUnit> _productionUnits = [];

    // Statistics
    // heat demand
    private double _totalSummerHeatDemand;

    private double _totalWinterHeatDemand;

    // production cost
    private double _totalSummerNetProductionCost;

    private double _totalWinterNetProductionCost;

    // profit
    private double _totalSummerProft;

    private double _totalWinterProfit;

    // co2 emission
    private double _totalSummerCo2Emission;
    private double _totalWinterCo2Emission;

    public ChartOptions Co2EmissionChartOptions = new ChartOptions { YAxisTicks = 100, };
    public ChartOptions ElectricityChartOptions = new ChartOptions();
    public ChartOptions HeatDemandChartOptions = new ChartOptions { YAxisTicks = 1 };
    private int Index = -1;
    private bool isDataReady = false;
    public ChartOptions OperationPointsChartOptions = new ChartOptions { YAxisTicks = 1 };
    public ChartOptions ProductionCostChartOptions = new ChartOptions();


    //TODO: REPLACE WITH REAL DATA
    private List<ChartSeries> HeatDemandSeries { get; set; }
    private List<ChartSeries> ElectricityPriceSeries { get; set; }
    private List<ChartSeries> NetProductionCostSeries { get; set; }

    // Co2 Emission Series
    private List<ChartSeries> Co2SeriesWinter1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesWinter2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesWinterCo2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesSummer1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesSummer2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesSummerCo2 { get; set; } = new List<ChartSeries>();

    // OPERATION POINTS SERIES
    private List<ChartSeries> OperationPointsSeriesWinter1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesWinter2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesWinterCo2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummer1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummer2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummerCo2 { get; set; } = new List<ChartSeries>();


    private string[] XAxisLabels { get; set; }
    private string?[] XAxisCo2EmissionLabels { get; set; }
    [Inject] public HttpClient Http { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public ILogger<Home> Logger { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Logger.LogInformation("OnInitializedAsync in Home.razor started");
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
            string optWinterCo2Uri = $"{baseWinterOptUri}{(int)OptimizationMode.Co2}";

            // Summer period
            string baseSummerOptUri = "http://localhost:5019/api/optimizer?season=summer&mode=";
            string optSummerScenario1Uri = $"{baseSummerOptUri}{(int)OptimizationMode.Scenario1}";
            string optSummerScenario2Uri = $"{baseSummerOptUri}{(int)OptimizationMode.Scenario2}";
            string optSummerCo2Uri = $"{baseSummerOptUri}{(int)OptimizationMode.Co2}";

            List<ResultHolder>? rawResultDataWinterScenario1 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterScenario1Uri);

            List<ResultHolder>? rawResultDataWinterScenario2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterScenario2Uri);

            List<ResultHolder>? rawResultDataWinterScenarioCo2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterCo2Uri);

            List<ResultHolder>? rawResultDataSummerScenario1 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerScenario1Uri);

            List<ResultHolder>? rawResultDataSummerScenario2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerScenario2Uri);

            List<ResultHolder>? rawResultDataSummerScenarioCo2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerCo2Uri);

            // Net Production Cost
            _netProductionCostScenario1Winter = GetNetProductionCostSeries(rawResultDataWinterScenario1);
            _netProductionCostScenario2Winter = GetNetProductionCostSeries(rawResultDataWinterScenario2);
            _netProductionCostScenarioCo2Winter = GetNetProductionCostSeries(rawResultDataWinterScenarioCo2);
            _netProductionCostScenario1Summer = GetNetProductionCostSeries(rawResultDataSummerScenario1);
            _netProductionCostScenario2Summer = GetNetProductionCostSeries(rawResultDataSummerScenario2);
            _netProductionCostScenarioCo2Summer = GetNetProductionCostSeries(rawResultDataSummerScenarioCo2);

            // Co2 Emission
            _co2EmissionScenario1Winter = GetCo2EmissionSeries(rawResultDataWinterScenario1);
            _co2EmissionScenario2Winter = GetCo2EmissionSeries(rawResultDataWinterScenario2);
            _co2EmissionScenarioCo2Winter = GetCo2EmissionSeries(rawResultDataWinterScenarioCo2);
            _co2EmissionScenario1Summer = GetCo2EmissionSeries(rawResultDataSummerScenario1);
            _co2EmissionScenario2Summer = GetCo2EmissionSeries(rawResultDataSummerScenario2);
            _co2EmissionScenarioCo2Summer = GetCo2EmissionSeries(rawResultDataSummerScenarioCo2);

            // Operation Points TODO: NOT FULLY FINISHED
            _operationPointsScenario1Winter = await GetOperationPoints("winter", OptimizationMode.Scenario1);
            _operationPointsScenario2Winter = await GetOperationPoints("winter", OptimizationMode.Scenario2);
            _operationPointsScenarioCo2Winter = await GetOperationPoints("winter", OptimizationMode.Co2);
            _operationPointsScenario1Summer = await GetOperationPoints("summer", OptimizationMode.Scenario1);
            _operationPointsScenario2Summer = await GetOperationPoints("summer", OptimizationMode.Scenario2);
            _operationPointsScenarioCo2Summer = await GetOperationPoints("summer", OptimizationMode.Co2);

            // Sum
            // heat demand
            _totalWinterHeatDemand = SumHeatDemand(_heatDemandWinterSeries);
            _totalSummerHeatDemand = SumHeatDemand(_heatDemandSummerSeries);
            // net production cost
            _totalWinterNetProductionCost = SumProductionCost(_netProductionCostScenario2Winter);
            _totalSummerNetProductionCost = SumProductionCost(_netProductionCostScenario2Summer);
            // profit
            _totalSummerProft = 0;
            _totalWinterProfit = 0;
            // co2 emission
            _totalSummerCo2Emission = SumCo2Emission(_co2EmissionScenario2Summer);
            _totalWinterCo2Emission = SumCo2Emission(_co2EmissionScenario2Winter);

            // initialize the charts
            InitializeHeatDemandChartData();
            InitializeElectricityPriceChartData();
            InitializeProductionCostChartData();
            InitializeCo2EmissionChartData();
            InitializeOperationPointsChartData();
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
        NetProductionCostSeries = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Winter Scenario 1",
                Data = _netProductionCostScenario1Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Winter Scenario 2",
                Data = _netProductionCostScenario2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Winter Scenario Co2",
                Data = _netProductionCostScenarioCo2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario 1",
                Data = _netProductionCostScenario1Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario 2",
                Data = _netProductionCostScenario2Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Scenario Co2",
                Data = _netProductionCostScenarioCo2Summer!.Select(x => x.YData).ToArray()
            }
        };
        XAxisLabels = Enumerable.Range(1, _netProductionCostScenario1Winter.Count)
            .Select(i => i % 12 == 0 ? $"T+{i}" : string.Empty)
            .ToArray();
    }

    private List<ChartSeries> addBoilerCo2EmissionCharSeries(Dictionary<string, List<ChartData>> boilersChartData)
    {
        List<ChartSeries> co2EmissionChartSeries = new();

        foreach (var (key, value) in boilersChartData)
        {
            co2EmissionChartSeries.Add(
                new ChartSeries() { Name = key.ToString(), Data = value.Select(x => x.YData).ToArray() }
            );
        }

        return co2EmissionChartSeries;
    }

    // CO2
    void InitializeCo2EmissionChartData()
    {
        // WINTER
        Co2SeriesWinter1 = addBoilerCo2EmissionCharSeries(_co2EmissionScenario1Winter);
        Co2SeriesWinter2 = addBoilerCo2EmissionCharSeries(_co2EmissionScenario2Winter);
        Co2SeriesWinterCo2 = addBoilerCo2EmissionCharSeries(_co2EmissionScenarioCo2Winter);

        // SUMMER
        Co2SeriesSummer1 = addBoilerCo2EmissionCharSeries(_co2EmissionScenario1Summer);
        Co2SeriesSummer2 = addBoilerCo2EmissionCharSeries(_co2EmissionScenario2Summer);
        Co2SeriesSummerCo2 = addBoilerCo2EmissionCharSeries(_co2EmissionScenarioCo2Summer);

        // Assuming all scenarios have the same number of data points
        int numDataPoints = _co2EmissionScenario1Winter?.Count ?? 0;
        Console.WriteLine(_co2EmissionScenario1Winter?.First().Value[12].XData);
        XAxisCo2EmissionLabels = Enumerable.Range(1, numDataPoints)
            .Select(i => i % 12 == 0
                ? _co2EmissionScenario1Winter?.First().Value[i].XData
                : string.Empty)
            .ToArray();

        // XAxisCo2EmissionLabels = _co2EmissionScenario1Summer.First().Value.Select(
        //      x => x.XData
        //     ).ToArray();
    }

    private void InitializeOperationPointsChartData()
    {
        OperationPointsSeriesWinter1 = new List<ChartSeries>();
        OperationPointsSeriesWinter2 = new List<ChartSeries>();
        OperationPointsSeriesWinterCo2 = new List<ChartSeries>();
        OperationPointsSeriesSummer1 = new List<ChartSeries>();
        OperationPointsSeriesSummer2 = new List<ChartSeries>();
        OperationPointsSeriesSummerCo2 = new List<ChartSeries>();

        // Adding series for each scenario
        AddOperationPointSeries(_operationPointsScenario1Winter, OperationPointsSeriesWinter1);
        AddOperationPointSeries(_operationPointsScenario2Winter, OperationPointsSeriesWinter2);
        AddOperationPointSeries(_operationPointsScenarioCo2Winter, OperationPointsSeriesWinterCo2);
        AddOperationPointSeries(_operationPointsScenario1Summer, OperationPointsSeriesSummer1);
        AddOperationPointSeries(_operationPointsScenario2Summer, OperationPointsSeriesSummer2);
        AddOperationPointSeries(_operationPointsScenarioCo2Summer, OperationPointsSeriesSummerCo2);

        // Assuming all scenarios have the same number of data points
        int numDataPoints = _operationPointsScenario1Winter?.Count ?? 0;
        XAxisLabels = Enumerable.Range(1, numDataPoints)
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

    private Dictionary<string, List<ChartData>> GetCo2EmissionSeries(List<ResultHolder>? rawResultData)
    {
        Dictionary<string, List<ChartData>> co2Emission = new Dictionary<string, List<ChartData>>();

        if (rawResultData != null)
        {
            foreach (var result in rawResultData)
            {
                foreach (var boiler in result.Boilers)
                {
                    if (co2Emission.ContainsKey(boiler.FullName))
                    {
                        // Boiler already exists in the dictionary, add data to the existing list
                        co2Emission[boiler.FullName].Add(new ChartData
                        {
                            XData = FormatDate(result.StartTime), YData = boiler.Co2Emission,
                        });
                    }
                    else
                    {
                        // Boiler does not exist in the dictionary, create a new entry
                        co2Emission[boiler.FullName] = new List<ChartData>
                        {
                            new ChartData { XData = FormatDate(result.StartTime), YData = boiler.Co2Emission, }
                        };
                    }
                }
            }
        }

        foreach (var (key, value) in co2Emission)
        {
            co2Emission[key]?.Insert(0, new ChartData() { XData = co2Emission[key][0].XData, YData = 0 });
        }

        return co2Emission;
    }

    private List<ChartData>? GetNetProductionCostSeries(List<ResultHolder>? rawResultData)
    {
        var productionCostDataList = rawResultData?.Select(item => new ChartData()
        {
            XData = FormatDate(item.StartTime), YData = item.NetProductionCost
        }).ToList();

        return productionCostDataList;
    }

    private async Task<List<ChartData>> GetOperationPoints(string season, OptimizationMode mode)
    {
        string uri = $"http://localhost:5019/api/optimizer?season={season}&mode={(int)mode}";
        List<ResultHolder>? rawResultData = await Http.GetFromJsonAsync<List<ResultHolder>>(uri);

        if (rawResultData != null)
        {
            // Log the operation points
            LogOperationPoints(rawResultData);

            List<ChartData> operationPoints = new List<ChartData>();

            foreach (var result in rawResultData)
            {
                foreach (var boiler in result.Boilers)
                {
                    // Add operation point data to the list
                    operationPoints.Add(new ChartData { XData = boiler.FullName, YData = boiler.OperationPoint });
                }
            }

            return operationPoints;
        }

        return new List<ChartData>();
    }

    private void AddOperationPointSeries(List<ChartData>? operationPoints, List<ChartSeries> chartSeries)
    {
        if (operationPoints == null)
            return;

        var groupedOperationPoints = operationPoints.GroupBy(x => x.XData);

        foreach (var group in groupedOperationPoints)
        {
            chartSeries.Add(new ChartSeries { Name = group.Key, Data = group.Select(x => x.YData).ToArray() });
        }
    }

    private void LogOperationPoints(List<ResultHolder> rawResultData)
    {
        foreach (var result in rawResultData)
        {
            foreach (var boiler in result.Boilers)
            {
                // Construct a string representation of the data for logging
                string logMessage =
                    $"StartTime: {result.StartTime}, Boiler: {boiler.FullName}, OperationPoint: {boiler.OperationPoint}";
                // Logger.LogInformation(logMessage);
            }
        }
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

    private double SumHeatDemand(List<ChartData>? series)
    {
        if (series == null || series.Count == 0)
            return 0;

        double sum = series.Sum(data => data.YData);
        return (int)Math.Round(sum);
    }

    private double SumProductionCost(List<ChartData>? series)
    {
        if (series == null || series.Count == 0)
            return 0;

        double sum = series.Sum(data => data.YData);
        return (int)Math.Round(sum);
    }

    private double SumCo2Emission(Dictionary<string, List<ChartData>> seriesDict)
    {
        double sum = 0;
        foreach (var (seriesKey, seriesValue) in seriesDict)
        {
            sum += seriesValue.Sum(data => data.YData);
        }

        return (int)Math.Round(sum);
    }

    private double SumProfit(List<ChartData>? series)
    {
        if (series == null || series.Count == 0)
            return 0;

        double sum = series.Sum(data => data.YData);
        return (int)Math.Round(sum);
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
