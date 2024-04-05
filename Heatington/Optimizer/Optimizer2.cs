using Heatington.Data;
using Heatington.Models;

namespace Heatington.Optimizer;

public class Optimizer2
{
    private List<DataPoint>? _dataPoints = new List<DataPoint>();
    private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
    public List<ResultHolder>? Results { get; private set; }

    public void LoadData()
    {
        GetDataPoints();
        GetProductionUnits();

        _productionUnits = _productionUnits.OrderBy(o => o.ProductionCost).ToList();
    }

    public void OptimizeScenario1()
    {
        CalculateHeatUnitsRequired();
        CalculateNetProductionCost();
    }

    private List<ProductionUnit> SetOperationPoint(List<ProductionUnit> productionUnits, double heatDemand)
    {
        // double totalProductionCapacity = productionUnits.Sum(o => o.MaxHeat);

        /*if (productionUnits.Count == 1)
        {
            double operationPoint = CalculateOperationPoint(HeatDemand, productionUnits[0].MaxHeat);
            productionUnits[0].OperationPoint = operationPoint;
            return productionUnits;
        }*/

        for (int i = 0; i < productionUnits.Count; i++)
        {
            heatDemand -= productionUnits[i].MaxHeat;

            // As long as it is not the last boiler in the list it sets the operation point to max (1)
            if (productionUnits.Count != (i - 1))
            {
                productionUnits[i].OperationPoint = 1;
            }
            else
            {
                double operationPoint = CalculateOperationPoint(heatDemand, productionUnits[i].MaxHeat);
                productionUnits[i].OperationPoint = operationPoint;
            }
        }

        double CalculateOperationPoint(double demand, double productionCapacity)
        {
            return demand / productionCapacity;
        }

        return productionUnits;
    }

    private void CalculateHeatUnitsRequired()
    {
        if (_dataPoints == null)
        {
            return;
        }

        List<ResultHolder> results = new List<ResultHolder>();

        foreach (DataPoint dataPoint in _dataPoints)
        {
            // Calculates how many boilers are needed to satisfy heat demand
            // Implies that they are ordered by efficiency
            int nOfBoilers = SatisfyHeatDemand(dataPoint);

            // Selects the correct boilers based on how many need to be activated
            // Again implies that they are ordered by efficiency
            List<ProductionUnit> selectedBoilers = SelectBoiler(nOfBoilers);

            // Sets the operation point of the selected boilers
            selectedBoilers = SetOperationPoint(selectedBoilers, dataPoint.HeatDemand);

            // Creates an object which holds the result
            ResultHolder result = new ResultHolder(dataPoint.StartTime, dataPoint.EndTime, dataPoint.HeatDemand,
                dataPoint.ElectricityPrice, 0, selectedBoilers);

            // Adds the object to the list
            results.Add(result);
        }

        Results = results;

        int SatisfyHeatDemand(DataPoint dataPoint)
        {
            double currentProductionCapacity = 0;
            int i = 0;

            while (dataPoint.HeatDemand > currentProductionCapacity)
            {
                currentProductionCapacity = currentProductionCapacity + _productionUnits[i].MaxHeat;
                i++;

                if (i > _productionUnits.Count)
                {
                    Console.WriteLine("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
                    throw new Exception("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
                }
            }
            return i;
        }

        List<ProductionUnit> SelectBoiler(int j)
        {
            List<ProductionUnit> selectedBoilers = new List<ProductionUnit>();

            // I do believe that some optimization (as in performance) could be done here
            // Right now focusing on a working product
            for (int i = 0; i < j; i++)
            {
                selectedBoilers.Add(_productionUnits[i]);
            }

            return selectedBoilers;
        }
    }
    public void CalculateNetProductionCost()
    {
        // Should I make a copy of the list, set the Production Cost and update the public one OR
        // keep manipulating it directly like I am now
        if (Results == null)
        {
            return;
        }

        int i = 0;

        foreach (var entry in Results)
        {
            double hourlyProductionCost = Results[i].Boilers.Sum(o => o.ProductionCost);

            Results[i].NetProductionCost = hourlyProductionCost;

            i++;
        }
    }

    public void LogResults()
    {
        if (Results == null)
        {
            return;
        }

        foreach (ResultHolder resultHolder in Results)
        {
            Console.WriteLine(resultHolder);
        }
    }

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
