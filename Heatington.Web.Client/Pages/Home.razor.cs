using MudBlazor;
using System.Net.Http.Json;
using Heatington.Models;
using Heatington.Optimizer;
using Microsoft.AspNetCore.Components;
using static System.Linq.Enumerable;

namespace Heatington.Web.Client.Pages;

public partial class Home : ComponentBase
{
    // heat demand
    private static List<ChartData>? s_heatDemandWinterSeries;
    private static List<ChartData>? s_heatDemandSummerSeries;
    private static List<ChartData>? s_realheatDemandWinterSeries;
    private static List<ChartData>? s_realheatDemandSummerSeries;

    // electricity price
    private static List<ChartData>? _electricityPriceWinterSeries;
    private static List<ChartData>? _electricityPriceSummerSeries;
    private static List<ChartData>? _realElectricityPriceWinterSeries;
    private static List<ChartData>? _realElectricityPriceSummerSeries;

    // net production cost
    private static List<ChartData>? _netProductionCostScenario1Winter;
    private static List<ChartData>? _netProductionCostScenario1Summer;
    private static List<ChartData>? _netProductionCostScenario2Winter;
    private static List<ChartData>? _netProductionCostScenario2Summer;
    private static List<ChartData>? _netProductionCostScenarioCo2Winter;
    private static List<ChartData>? _netProductionCostScenarioCo2Summer;
    private static List<ChartData>? _realNetProductionCostScenario1Winter;
    private static List<ChartData>? _realNetProductionCostScenario1Summer;
    private static List<ChartData>? _realNetProductionCostScenario2Winter;
    private static List<ChartData>? _realNetProductionCostScenario2Summer;
    private static List<ChartData>? _realNetProductionCostScenarioCo2Winter;
    private static List<ChartData>? _realNetProductionCostScenarioCo2Summer;

    // co2 emission
    private Dictionary<string, List<ChartData>>? _co2EmissionScenario1Summer;
    private Dictionary<string, List<ChartData>>? _co2EmissionScenario1Winter;
    private Dictionary<string, List<ChartData>>? _co2EmissionScenario2Summer;
    private Dictionary<string, List<ChartData>>? _co2EmissionScenario2Winter;
    private Dictionary<string, List<ChartData>>? _co2EmissionScenarioCo2Summer;
    private Dictionary<string, List<ChartData>>? _co2EmissionScenarioCo2Winter;

    // operation points
    private List<ChartData>? _operationPointsScenario1Summer;
    private List<ChartData>? _operationPointsScenario1Winter;
    private List<ChartData>? _operationPointsScenario2Summer;
    private List<ChartData>? _operationPointsScenario2Winter;
    private List<ChartData>? _operationPointsScenarioCo2Summer;
    private List<ChartData>? _operationPointsScenarioCo2Winter;
    private List<ProductionUnit> _productionUnits = [];


    // co2 emission - real
    private Dictionary<string, List<ChartData>>? _realCo2EmissionScenario1Summer;
    private Dictionary<string, List<ChartData>>? _realCo2EmissionScenario1Winter;
    private Dictionary<string, List<ChartData>>? _realCo2EmissionScenario2Summer;
    private Dictionary<string, List<ChartData>>? _realCo2EmissionScenario2Winter;
    private Dictionary<string, List<ChartData>>? _realCo2EmissionScenarioCo2Summer;
    private Dictionary<string, List<ChartData>>? _realCo2EmissionScenarioCo2Winter;

    // operation points - real data
    private List<ChartData>? _realOperationPointsScenario1Summer;
    private List<ChartData>? _realOperationPointsScenario1Winter;
    private List<ChartData>? _realOperationPointsScenario2Summer;
    private List<ChartData>? _realOperationPointsScenario2Winter;
    private List<ChartData>? _realOperationPointsScenarioCo2Summer;
    private List<ChartData>? _realOperationPointsScenarioCo2Winter;


    // Statistics
    // co2 emission
    private double _totalSummerCo2Emission;

    // heat demand
    private double _totalSummerHeatDemand;

    // production cost
    private double _totalSummerNetProductionCost;

    // profit
    private double _totalWinterCo2Emission;

    private double _totalWinterHeatDemand;

    private double _totalWinterNetProductionCost;

    public ChartOptions Co2EmissionChartOptions = new ChartOptions { YAxisTicks = 100, };
    public ChartOptions ElectricityChartOptions = new ChartOptions();
    public ChartOptions HeatDemandChartOptions = new ChartOptions { YAxisTicks = 1 };
    private int Index;
    private bool _isDataReady;
    public readonly ChartOptions OperationPointsChartOptions = new ChartOptions { YAxisTicks = 1 };
    private ChartOptions ProductionCostChartOptions = new ChartOptions();

    private List<ChartSeries>? HeatDemandSeries { get; set; }
    private List<ChartSeries>? ElectricityPriceSeries { get; set; }
    private List<ChartSeries>? NetProductionCostSeries { get; set; }

    // Co2 Emission Series
    private List<ChartSeries> Co2SeriesWinter1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesWinter2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesWinterCo2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesSummer1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesSummer2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> Co2SeriesSummerCo2 { get; set; } = new List<ChartSeries>();

    // Co2 Emission Series - Real
    private List<ChartSeries> RealCo2SeriesWinter1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealCo2SeriesWinter2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealCo2SeriesWinterCo2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealCo2SeriesSummer1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealCo2SeriesSummer2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealCo2SeriesSummerCo2 { get; set; } = new List<ChartSeries>();

    // OPERATION POINTS SERIES
    private List<ChartSeries> OperationPointsSeriesWinter1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesWinter2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesWinterCo2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummer1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummer2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummerCo2 { get; set; } = new List<ChartSeries>();

    // OPERATION POINTS SERIES - REAL DATA
    private List<ChartSeries> RealOperationPointsSeriesWinter1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealOperationPointsSeriesWinter2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealOperationPointsSeriesWinterCo2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealOperationPointsSeriesSummer1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealOperationPointsSeriesSummer2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> RealOperationPointsSeriesSummerCo2 { get; set; } = new List<ChartSeries>();

    private string[]? XAxisLabels { get; set; }
    [Inject] public required HttpClient Http { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Logger.LogInformation("OnInitializedAsync in Home.razor started");
            await base.OnInitializedAsync();
            _productionUnits = await LoadProductionUnits();

            s_heatDemandWinterSeries = [];
            s_heatDemandSummerSeries = [];
            _electricityPriceSummerSeries = [];
            _electricityPriceWinterSeries = [];

            List<DataPoint>? winterData =
                await Http.GetFromJsonAsync<List<DataPoint>>("http://localhost:5165/api/TimeSeriesData?season=winter");
            s_heatDemandWinterSeries = GetHeatDemandSeries(winterData);
            _electricityPriceWinterSeries = GetElectricityPriceSeries(winterData);

            List<DataPoint>? realWinterData =
                await Http.GetFromJsonAsync<List<DataPoint>>(
                    "http://localhost:5165/api/TimeSeriesData?season=winter-real");
            s_realheatDemandWinterSeries = GetHeatDemandSeries(realWinterData);
            _realElectricityPriceWinterSeries = GetElectricityPriceSeries(realWinterData);

            List<DataPoint>? summerData =
                await Http.GetFromJsonAsync<List<DataPoint>>("http://localhost:5165/api/TimeSeriesData?season=summer");
            s_heatDemandSummerSeries = GetHeatDemandSeries(summerData);
            _electricityPriceSummerSeries = GetElectricityPriceSeries(summerData);

            List<DataPoint>? realSummerData =
                await Http.GetFromJsonAsync<List<DataPoint>>(
                    "http://localhost:5165/api/TimeSeriesData?season=summer-real");
            s_realheatDemandSummerSeries = GetHeatDemandSeries(realSummerData);
            _realElectricityPriceSummerSeries = GetElectricityPriceSeries(realSummerData);

            string winter = "winter";
            string summer = "summer";
            string winterReal = "winter-real";
            string summerReal = "summer-real";
            int mode1 = (int)OptimizationMode.Scenario1;
            int mode2 = (int)OptimizationMode.Scenario2;
            int mode3 = (int)OptimizationMode.Co2;

            // Winter period
            string optWinterScenario1Uri = GenerateOptApiUri(winter, mode1);
            string optWinterScenario2Uri = GenerateOptApiUri(winter, mode2);
            string optWinterCo2Uri = GenerateOptApiUri(winter, mode3);

            // Summer period
            string optSummerScenario1Uri = GenerateOptApiUri(summer, mode1);
            string optSummerScenario2Uri = GenerateOptApiUri(summer, mode2);
            string optSummerCo2Uri = GenerateOptApiUri(summer, mode3);

            // Real OPT

            // Winter period
            string realOptWinterScenario1Uri = GenerateOptApiUri(winterReal, mode1);
            string realOptWinterScenario2Uri = GenerateOptApiUri(winterReal, mode2);
            string realOptWinterCo2Uri = GenerateOptApiUri(winterReal, mode3);

            // Summer period
            string realOptSummerScenario1Uri = GenerateOptApiUri(summerReal, mode1);
            string realOptSummerScenario2Uri = GenerateOptApiUri(summerReal, mode2);
            string realOptSummerCo2Uri = GenerateOptApiUri(summerReal, mode3);

            // Raw Result from Danfoss
            List<FormatedResultHolder>? rawResultDataWinterScenario1 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(optWinterScenario1Uri);

            List<FormatedResultHolder>? rawResultDataWinterScenario2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(optWinterScenario2Uri);

            List<FormatedResultHolder>? rawResultDataWinterScenarioCo2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(optWinterCo2Uri);

            List<FormatedResultHolder>? rawResultDataSummerScenario1 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(optSummerScenario1Uri);

            List<FormatedResultHolder>? rawResultDataSummerScenario2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(optSummerScenario2Uri);

            List<FormatedResultHolder>? rawResultDataSummerScenarioCo2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(optSummerCo2Uri);

            // Raw Result from real data

            List<FormatedResultHolder>? rawRealResultDataWinterScenario1 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(realOptWinterScenario1Uri);

            List<FormatedResultHolder>? rawRealResultDataWinterScenario2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(realOptWinterScenario2Uri);

            List<FormatedResultHolder>? rawRealResultDataWinterScenarioCo2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(realOptWinterCo2Uri);

            List<FormatedResultHolder>? rawRealResultDataSummerScenario1 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(realOptSummerScenario1Uri);

            List<FormatedResultHolder>? rawRealResultDataSummerScenario2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(realOptSummerScenario2Uri);

            List<FormatedResultHolder>? rawRealResultDataSummerScenarioCo2 =
                await Http.GetFromJsonAsync<List<FormatedResultHolder>>(realOptSummerCo2Uri);

            // Net Production Cost
            _netProductionCostScenario1Winter = GetNetProductionCostSeries(rawResultDataWinterScenario1);
            _netProductionCostScenario2Winter = GetNetProductionCostSeries(rawResultDataWinterScenario2);
            _netProductionCostScenarioCo2Winter = GetNetProductionCostSeries(rawResultDataWinterScenarioCo2);
            _netProductionCostScenario1Summer = GetNetProductionCostSeries(rawResultDataSummerScenario1);
            _netProductionCostScenario2Summer = GetNetProductionCostSeries(rawResultDataSummerScenario2);
            _netProductionCostScenarioCo2Summer = GetNetProductionCostSeries(rawResultDataSummerScenarioCo2);

            // Net Production Cost - Real Data
            _realNetProductionCostScenario1Winter = GetNetProductionCostSeries(rawRealResultDataWinterScenario1);
            _realNetProductionCostScenario2Winter = GetNetProductionCostSeries(rawRealResultDataWinterScenario2);
            _realNetProductionCostScenarioCo2Winter = GetNetProductionCostSeries(rawRealResultDataWinterScenarioCo2);
            _realNetProductionCostScenario1Summer = GetNetProductionCostSeries(rawRealResultDataSummerScenario1);
            _realNetProductionCostScenario2Summer = GetNetProductionCostSeries(rawRealResultDataSummerScenario2);
            _realNetProductionCostScenarioCo2Summer = GetNetProductionCostSeries(rawRealResultDataSummerScenarioCo2);

            // Co2 Emission
            _co2EmissionScenario1Winter = GetCo2EmissionSeries(rawResultDataWinterScenario1);
            _co2EmissionScenario2Winter = GetCo2EmissionSeries(rawResultDataWinterScenario2);
            _co2EmissionScenarioCo2Winter = GetCo2EmissionSeries(rawResultDataWinterScenarioCo2);
            _co2EmissionScenario1Summer = GetCo2EmissionSeries(rawResultDataSummerScenario1);
            _co2EmissionScenario2Summer = GetCo2EmissionSeries(rawResultDataSummerScenario2);
            _co2EmissionScenarioCo2Summer = GetCo2EmissionSeries(rawResultDataSummerScenarioCo2);


            // Co2 Emission - Real Data
            _realCo2EmissionScenario1Winter = GetCo2EmissionSeries(rawRealResultDataWinterScenario1);
            _realCo2EmissionScenario2Winter = GetCo2EmissionSeries(rawRealResultDataWinterScenario2);
            _realCo2EmissionScenarioCo2Winter = GetCo2EmissionSeries(rawRealResultDataWinterScenarioCo2);
            _realCo2EmissionScenario1Summer = GetCo2EmissionSeries(rawRealResultDataSummerScenario1);
            _realCo2EmissionScenario2Summer = GetCo2EmissionSeries(rawRealResultDataSummerScenario2);
            _realCo2EmissionScenarioCo2Summer = GetCo2EmissionSeries(rawRealResultDataSummerScenarioCo2);

            // Operation Points
            _operationPointsScenario1Winter = await GetOperationPoints("winter", OptimizationMode.Scenario1);
            _operationPointsScenario2Winter = await GetOperationPoints("winter", OptimizationMode.Scenario2);
            _operationPointsScenarioCo2Winter = await GetOperationPoints("winter", OptimizationMode.Co2);
            _operationPointsScenario1Summer = await GetOperationPoints("summer", OptimizationMode.Scenario1);
            _operationPointsScenario2Summer = await GetOperationPoints("summer", OptimizationMode.Scenario2);
            _operationPointsScenarioCo2Summer = await GetOperationPoints("summer", OptimizationMode.Co2);

            // Operation Points - real data
            _realOperationPointsScenario1Winter = await GetOperationPoints("winter-real", OptimizationMode.Scenario1);
            _realOperationPointsScenario2Winter = await GetOperationPoints("winter-real", OptimizationMode.Scenario2);
            _realOperationPointsScenarioCo2Winter = await GetOperationPoints("winter-real", OptimizationMode.Co2);
            _realOperationPointsScenario1Summer = await GetOperationPoints("summer-real", OptimizationMode.Scenario1);
            _realOperationPointsScenario2Summer = await GetOperationPoints("summer-real", OptimizationMode.Scenario2);
            _realOperationPointsScenarioCo2Summer = await GetOperationPoints("summer-real", OptimizationMode.Co2);

            // SUMMARIZE
            // heat demand
            _totalWinterHeatDemand = SumHeatDemand(s_heatDemandWinterSeries);
            _totalSummerHeatDemand = SumHeatDemand(s_heatDemandSummerSeries);
            // net production cost
            _totalWinterNetProductionCost = SumProductionCost(_netProductionCostScenario2Winter);
            _totalSummerNetProductionCost = SumProductionCost(_netProductionCostScenario2Summer);
            // co2 emission
            _totalSummerCo2Emission = SumCo2Emission(_co2EmissionScenario2Summer);
            _totalWinterCo2Emission = SumCo2Emission(_co2EmissionScenario2Winter);

            // initialize the charts
            InitializeHeatDemandChartData();
            InitializeElectricityPriceChartData();
            InitializeProductionCostChartData();
            InitializeCo2EmissionChartData();
            InitializeOperationPointsChartData();
            _isDataReady = true;
            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static string GenerateOptApiUri(string season, int mode)
    {
        return $"http://localhost:5143/api/ResultDataManager?season={season}&mode={mode}";
    }

    void InitializeHeatDemandChartData()
    {
        if (s_heatDemandWinterSeries == null || s_heatDemandWinterSeries.Count == 0 ||
            s_heatDemandSummerSeries == null || s_heatDemandSummerSeries.Count == 0)
        {
            return;
        }

        if (s_realheatDemandWinterSeries != null && s_realheatDemandSummerSeries != null)
        {
            HeatDemandSeries = new List<ChartSeries>()
            {
                new ChartSeries() { Name = "Winter", Data = s_heatDemandWinterSeries.Select(x => x.YData).ToArray() },
                new ChartSeries() { Name = "Summer", Data = s_heatDemandSummerSeries.Select(x => x.YData).ToArray() },
                new ChartSeries()
                {
                    Name = "Real Winter Data", Data = s_realheatDemandWinterSeries.Select(x => x.YData).ToArray()
                },
                new ChartSeries()
                {
                    Name = "Real Summer Data", Data = s_realheatDemandSummerSeries.Select(x => x.YData).ToArray()
                },
            };
        }

        XAxisLabels = Range(1, s_heatDemandWinterSeries.Count)
            .Select(i => i % 12 == 0 ? $"T+{i}" : string.Empty)
            .ToArray();
    }

    void InitializeElectricityPriceChartData()
    {
        if (_electricityPriceWinterSeries == null || _electricityPriceWinterSeries.Count == 0 ||
            _electricityPriceSummerSeries == null || _electricityPriceSummerSeries.Count == 0 ||
            _realElectricityPriceWinterSeries == null || _realElectricityPriceSummerSeries == null)
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
            },
            new ChartSeries()
            {
                Name = "Winter Real Data",
                Data = _realElectricityPriceWinterSeries.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Summer Real Data",
                Data = _realElectricityPriceSummerSeries.Select(x => x.YData).ToArray()
            }
        };
        XAxisLabels = Range(1, _electricityPriceWinterSeries.Count)
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
            },
            new ChartSeries()
            {
                Name = "Real Winter Scenario 1",
                Data = _realNetProductionCostScenario1Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Real Winter Scenario 2",
                Data = _realNetProductionCostScenario2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Real Winter Scenario Co2",
                Data = _realNetProductionCostScenarioCo2Winter!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Real Summer Scenario 1",
                Data = _realNetProductionCostScenario1Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Real Summer Scenario 2",
                Data = _realNetProductionCostScenario2Summer!.Select(x => x.YData).ToArray()
            },
            new ChartSeries()
            {
                Name = "Real Summer Scenario Co2",
                Data = _realNetProductionCostScenarioCo2Summer!.Select(x => x.YData).ToArray()
            }
        };
        if (_netProductionCostScenario1Winter != null)
        {
            XAxisLabels = Range(1, _netProductionCostScenario1Winter.Count)
                .Select(i => i % 12 == 0 ? $"T+{i}" : string.Empty)
                .ToArray();
        }
    }

    private List<ChartSeries> addBoilerCo2EmissionCharSeries(Dictionary<string, List<ChartData>>? boilersChartData)
    {
        List<ChartSeries> co2EmissionChartSeries = new();

        if (boilersChartData == null)
        {
            return co2EmissionChartSeries;
        }

        foreach ((string key, List<ChartData> value) in boilersChartData)
        {
            co2EmissionChartSeries.Add(
                new ChartSeries() { Name = key, Data = value.Select(x => x.YData).ToArray() }
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

        // REAL DATA

        // WINTER
        RealCo2SeriesWinter1 = addBoilerCo2EmissionCharSeries(_realCo2EmissionScenario1Winter);
        RealCo2SeriesWinter2 = addBoilerCo2EmissionCharSeries(_realCo2EmissionScenario2Winter);
        RealCo2SeriesWinterCo2 = addBoilerCo2EmissionCharSeries(_realCo2EmissionScenarioCo2Winter);

        // SUMMER
        RealCo2SeriesSummer1 = addBoilerCo2EmissionCharSeries(_realCo2EmissionScenario1Summer);
        RealCo2SeriesSummer2 = addBoilerCo2EmissionCharSeries(_realCo2EmissionScenario2Summer);
        RealCo2SeriesSummerCo2 = addBoilerCo2EmissionCharSeries(_realCo2EmissionScenarioCo2Summer);

        // Assuming all scenarios have the same number of data points
        if (_co2EmissionScenario1Winter == null)
        {
            return;
        }

        int numDataPoints = _co2EmissionScenario1Winter.Count;
        Console.WriteLine(_co2EmissionScenario1Winter.First().Value[12].XData);
        XAxisLabels = Range(1, numDataPoints)
            .Select(i => i % 12 == 0
                ? _co2EmissionScenario1Winter.First().Value[i].XData
                : string.Empty)
            .ToArray()!;

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
        RealOperationPointsSeriesWinter1 = new List<ChartSeries>();
        RealOperationPointsSeriesWinter2 = new List<ChartSeries>();
        RealOperationPointsSeriesWinterCo2 = new List<ChartSeries>();
        RealOperationPointsSeriesSummer1 = new List<ChartSeries>();
        RealOperationPointsSeriesSummer2 = new List<ChartSeries>();
        RealOperationPointsSeriesSummerCo2 = new List<ChartSeries>();

        // Adding series for each scenario
        AddOperationPointSeries(_operationPointsScenario1Winter, OperationPointsSeriesWinter1);
        AddOperationPointSeries(_operationPointsScenario2Winter, OperationPointsSeriesWinter2);
        AddOperationPointSeries(_operationPointsScenarioCo2Winter, OperationPointsSeriesWinterCo2);
        AddOperationPointSeries(_operationPointsScenario1Summer, OperationPointsSeriesSummer1);
        AddOperationPointSeries(_operationPointsScenario2Summer, OperationPointsSeriesSummer2);
        AddOperationPointSeries(_operationPointsScenarioCo2Summer, OperationPointsSeriesSummerCo2);
        AddOperationPointSeries(_realOperationPointsScenario1Winter, RealOperationPointsSeriesWinter1);
        AddOperationPointSeries(_realOperationPointsScenario2Winter, RealOperationPointsSeriesWinter2);
        AddOperationPointSeries(_realOperationPointsScenarioCo2Winter, RealOperationPointsSeriesWinterCo2);
        AddOperationPointSeries(_realOperationPointsScenario1Summer, RealOperationPointsSeriesSummer1);
        AddOperationPointSeries(_realOperationPointsScenario2Summer, RealOperationPointsSeriesSummer2);
        AddOperationPointSeries(_realOperationPointsScenarioCo2Summer, RealOperationPointsSeriesSummerCo2);

        // Assuming all scenarios have the same number of data points
        int numDataPoints = _operationPointsScenario1Winter?.Count ?? 0;
        XAxisLabels = Range(1, numDataPoints)
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

            return dataPoints;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"An error occurred when trying to load CSV data from {path}");
            throw;
        }
    }

    private ChartSeries GetHeatDemandSeries(List<DataPoint> dataPoints)
    {
        return new ChartSeries
        {
            Name = "Heat Demand Winter", Data = dataPoints.Select(dataPoint => dataPoint.HeatDemand).ToArray()
        };
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
        string formattedDate = datetimeInDanish.ToString("dd.MM.yyyy HH:mm");

        return formattedDate;
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

    private Dictionary<string, List<ChartData>> GetCo2EmissionSeries(List<FormatedResultHolder>? rawResultData)
    {
        Dictionary<string, List<ChartData>> co2Emission = new Dictionary<string, List<ChartData>>();

        if (rawResultData != null)
        {
            foreach (var result in rawResultData)
            {
                if (co2Emission.ContainsKey(result.Boiler.FullName))
                {
                    // Boiler already exists in the dictionary, add data to the existing list
                    co2Emission[result.Boiler.FullName].Add(new ChartData
                    {
                        XData = FormatDate(result.StartTime), YData = result.Boiler.Co2Emission,
                    });
                }
                else
                {
                    // Boiler does not exist in the dictionary, create a new entry
                    co2Emission[result.Boiler.FullName] = new List<ChartData>
                    {
                        new ChartData { XData = FormatDate(result.StartTime), YData = result.Boiler.Co2Emission, }
                    };
                }
            }
        }

        foreach (var (key, _) in co2Emission)
        {
            co2Emission[key].Insert(0, new ChartData() { XData = co2Emission[key][0].XData, YData = 0 });
        }

        return co2Emission;
    }

    private List<ChartData>? GetNetProductionCostSeries(List<FormatedResultHolder>? rawResultData)
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
            // LogOperationPoints(rawResultData);

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

    // private void LogOperationPoints(List<ResultHolder> rawResultData)
    // {
    //     foreach (var result in rawResultData)
    //     {
    //         foreach (var boiler in result.Boilers)
    //         {
    //             // Construct a string representation of the data for logging
    //             string logMessage =
    //                 $"StartTime: {result.StartTime}, Boiler: {boiler.FullName}, OperationPoint: {boiler.OperationPoint}";
    //             // Logger.LogInformation(logMessage);
    //         }
    //     }
    // }

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

    private double SumCo2Emission(Dictionary<string, List<ChartData>>? seriesDict)
    {
        double sum = 0;
        if (seriesDict == null)
        {
            return (int)Math.Round(sum);
        }

        foreach ((_, List<ChartData> seriesValue) in seriesDict)
        {
            sum += seriesValue.Sum(data => data.YData);
        }

        return (int)Math.Round(sum);
    }

    // private double SumProfit(List<ChartData>? series)
    // {
    //     if (series == null || series.Count == 0)
    //         return 0;
    //
    //     double sum = series.Sum(data => data.YData);
    //     return (int)Math.Round(sum);
    // }

    private void ViewMore()
    {
        NavManager.NavigateTo("/resource-manager");
    }


    public class ChartData
    {
        public string? XData { get; set; }
        public double YData { get; set; }
    }
}
