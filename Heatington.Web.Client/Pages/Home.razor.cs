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

    private List<ChartData>? _operationPointsScenario1Summer;
    private List<ChartData>? _operationPointsScenario1Winter;
    private List<ChartData>? _operationPointsScenario2Summer;
    private List<ChartData>? _operationPointsScenario2Winter;
    private List<ChartData>? _operationPointsScenarioCo2Summer;
    private List<ChartData>? _operationPointsScenarioCo2Winter;

    private List<ProductionUnit> _productionUnits = [];
    public ChartOptions Co2EmissionChartOptions = new ChartOptions();
    public ChartOptions HeatAndElectricityChartOptions = new ChartOptions { YAxisTicks = 1 };
    private int Index = -1;
    private bool isDataReady = false;
    public ChartOptions OperationPointsChartOptions = new ChartOptions { YAxisTicks = 1 };
    public ChartOptions ProductionCostChartOptions = new ChartOptions();


    //TODO: Use Real Data!
    private double totalCO2Produced = new Random().Next(20000, 50000);
    private double TotalExpenses = new Random().Next(50000, 100000);
    private double totalHeatProduced = new Random().Next(5000, 10000);
    private double totalProfit = new Random().Next(100000, 200000);

    private List<ChartSeries> HeatDemandSeries { get; set; }
    private List<ChartSeries> ElectricityPriceSeries { get; set; }
    private List<ChartSeries> ProductionCostSeries { get; set; }
    private List<ChartSeries> Co2EmissionSeries { get; set; }

    private List<ChartSeries> OperationPointsSeriesWinter1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesWinter2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesWinterCo2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummer1 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummer2 { get; set; } = new List<ChartSeries>();
    private List<ChartSeries> OperationPointsSeriesSummerCo2 { get; set; } = new List<ChartSeries>();


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

            List<ResultHolder>? rawResultDataWinterScenario2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterScenario2Uri);

            List<ResultHolder>? rawResultDataWinterScenarioCo2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optWinterCO2Uri);

            List<ResultHolder>? rawResultDataSummerScenario1 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerScenario1Uri);

            List<ResultHolder>? rawResultDataSummerScenario2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerScenario2Uri);

            List<ResultHolder>? rawResultDataSummerScenarioCo2 =
                await Http.GetFromJsonAsync<List<ResultHolder>>(optSummerCO2Uri);

            _productionCostScenario1Winter = GetProductionCostSeries(rawResultDataWinterScenario1);
            _productionCostScenario2Winter = GetProductionCostSeries(rawResultDataWinterScenario2);
            _productionCostScenarioCo2Winter = GetProductionCostSeries(rawResultDataWinterScenarioCo2);
            _productionCostScenario1Summer = GetProductionCostSeries(rawResultDataSummerScenario1);
            _productionCostScenario2Summer = GetProductionCostSeries(rawResultDataSummerScenario2);
            _productionCostScenarioCo2Summer = GetProductionCostSeries(rawResultDataSummerScenarioCo2);

            _operationPointsScenario1Winter = await GetOperationPoints("winter", OptimizationMode.Scenario1);
            _operationPointsScenario2Winter = await GetOperationPoints("winter", OptimizationMode.Scenario2);
            _operationPointsScenarioCo2Winter = await GetOperationPoints("winter", OptimizationMode.Co2);
            _operationPointsScenario1Summer = await GetOperationPoints("summer", OptimizationMode.Scenario1);
            _operationPointsScenario2Summer = await GetOperationPoints("summer", OptimizationMode.Scenario2);
            _operationPointsScenarioCo2Summer = await GetOperationPoints("summer", OptimizationMode.Co2);


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

    private void InitializeOperationPointsChartData()
    {
        OperationPointsSeriesWinter1 = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Electric Boiler",
                Data = _operationPointsScenario1Winter?.Select(x => x.YData).ToArray() ?? new double[0]
            },
        };

        OperationPointsSeriesWinter2 = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Electric Boiler",
                Data = _operationPointsScenario2Winter?.Select(x => x.YData).ToArray() ?? new double[0]
            },
        };

        OperationPointsSeriesWinterCo2 = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Electric Boiler",
                Data = _operationPointsScenarioCo2Winter?.Select(x => x.YData).ToArray() ?? new double[0]
            },
        };

        OperationPointsSeriesSummer1 = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Electric Boiler",
                Data = _operationPointsScenario1Summer?.Select(x => x.YData).ToArray() ?? new double[0]
            },
        };

        OperationPointsSeriesSummer2 = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Electric Boiler",
                Data = _operationPointsScenario2Summer?.Select(x => x.YData).ToArray() ?? new double[0]
            },
        };

        OperationPointsSeriesSummerCo2 = new List<ChartSeries>()
        {
            new ChartSeries()
            {
                Name = "Electric Boiler",
                Data = _operationPointsScenarioCo2Summer?.Select(x => x.YData).ToArray() ?? new double[0]
            },
        };


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

    private List<ChartData>? GetProductionCostSeries(List<ResultHolder>? rawResultData)
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
                    operationPoints.Add(new ChartData { YData = boiler.OperationPoint, });
                }
            }

            return operationPoints;
        }

        return new List<ChartData>();
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
                Logger.LogInformation(logMessage);
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
