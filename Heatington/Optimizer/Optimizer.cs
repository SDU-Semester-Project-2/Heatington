using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Services.Interfaces;

namespace Heatington.Optimizer;

public class Opt
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
    }

    private List<ProductionUnit> SetOperationPoint(List<ProductionUnit> productionUnits, double heatDemand)
    {
        List<ProductionUnit> workingUnits = new();

        foreach (var unit in productionUnits)
        {
            // For some reason if I don't clone the object C# confuses to which object I am referring to
            // and messes up all the objects in the "results" list
            ProductionUnit unitClone = (ProductionUnit)unit.Clone();

            if (unitClone.MaxHeat > heatDemand)
            {
                double operationPoint = CalculateOperationPoint(heatDemand, unit.MaxHeat);


                unitClone.OperationPoint = operationPoint;

                workingUnits.Add(unitClone);
                return workingUnits;
            }

            unitClone.OperationPoint = 1;
            heatDemand = heatDemand - unitClone.MaxHeat;
            workingUnits.Add(unitClone);
        }

        return workingUnits;

        double CalculateOperationPoint(double demand, double productionCapacity)
        {
            return Math.Round((demand / productionCapacity), 4);
        }
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

            //Console.WriteLine("Number of boilers: {0}", nOfBoilers);

            // Selects the correct boilers based on how many need to be activated
            // Again implies that they are ordered by efficiency
            List<ProductionUnit> selectedBoilers = SelectBoiler(nOfBoilers);

            //Console.WriteLine("Boilers selected:");
            //selectedBoilers.ForEach(Console.WriteLine);

            // Sets the operation point of the selected boilers
            selectedBoilers = SetOperationPoint(selectedBoilers, dataPoint.HeatDemand);

            // Creates an object which holds the result
            ResultHolder result = new ResultHolder(dataPoint.StartTime, dataPoint.EndTime, dataPoint.HeatDemand,
                dataPoint.ElectricityPrice, 0, selectedBoilers);
            // Adds the object to the list
            results.Add(result);
            Console.WriteLine(results[^1]);
            Console.WriteLine(result);
        }

        Results = results;
        results.ForEach(Console.WriteLine);


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

            for (int i = 0; i < j; i++)
            {
                selectedBoilers.Add(_productionUnits[i]);
            }

            return selectedBoilers;
        }
    }

    public void CalculateNetProductionCost()
    {
        if (Results == null)
        {
            return;
        }

        List<ResultHolder> workingResults = new List<ResultHolder>();

        foreach (ResultHolder result in Results)
        {
            ResultHolder resultClone = (ResultHolder)result.Clone();

            double hourlyProductionCost = resultClone.Boilers.Sum(o => o.ProductionCost);

            resultClone.NetProductionCost = hourlyProductionCost;

            workingResults.Add(resultClone);
        }

        Results = workingResults;
    }

    public void LogResults()
    {
        if (Results == null)
        {
            return;
        }

        Results.ForEach(Console.WriteLine);
    }

    private void GetDataPoints()
    {
        IDataSource dataSource = new CsvController();

        string fileName = "winter_period.csv";
        string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);

        SourceDataManager.SourceDataManager sourceDataManager = new(dataSource, filePath);

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
        ProductionUnit controlBoiler = new ProductionUnit("Control Boiler", "", 5, 800, 0, 1.5, 310);
        ProductionUnit gasBoiler = new ProductionUnit("Gas Boiler", "", 5, 500, 0, 1.1, 215);
        ProductionUnit oilBoiler = new ProductionUnit("Oil Boiler", "", 4, 700, 0, 1.2, 265);

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
