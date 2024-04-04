using Heatington.Data;
using Heatington.Models;
using Heatington.SourceDataManager;

namespace Heatington.Optimizer;

public class Optimizer2
{
    private List<DataPoint>? _dataPoints = new List<DataPoint>();
    private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();

    public Optimizer2()
    {
        GetDataPoints();
        GetProductionUnits();

        _productionUnits = _productionUnits.OrderBy(o => o.ProductionCost).ToList();
    }
    
    public void OptimizeScenario1()
    {

    }


    public void GetDataPoints()
    private void GetDataPoints()
    {
        IDataSource dataSource = new CsvDataSource();

        SourceDataManager.SourceDataManager sourceDataManager = new(dataSource, "../../../../Assets/winter_period.csv");

        Task fetchTimeSeriesDataAsync = sourceDataManager.FetchTimeSeriesDataAsync();

        fetchTimeSeriesDataAsync.Wait();

        //sourceDataManager.LogTimeSeriesData();

        _dataPoints = sourceDataManager.TimeSeriesData;
    }

    public void LogDataPoints()
    {
        if (_dataPoints == null)
        {
            return;
        }

        foreach (DataPoint dataPoint in _dataPoints)
        {
            Console.WriteLine(dataPoint);
        }
    }

    // Will call Asset Manager eventually.
    private void GetProductionUnits()
    {
        ProductionUnit controlBoiler = new ProductionUnit("Control Boiler", "", 5,800,0,1.5,310);
        ProductionUnit gasBoiler = new ProductionUnit("Gas Boiler", "", 5,500,0,1.1,215);
        ProductionUnit oilBoiler = new ProductionUnit("Oil Boiler", "", 4,700,0,1.2,265);

        _productionUnits.Add(controlBoiler);
        _productionUnits.Add(oilBoiler);
        _productionUnits.Add(gasBoiler);
    }

    public void LogProductionUnits()
    {
        int i = 0;
        foreach (ProductionUnit productionUnit in _productionUnits)
        {
            Console.WriteLine($"{i} " + productionUnit);
            i++;
        }
    }
}
