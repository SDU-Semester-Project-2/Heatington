using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Services.Interfaces;

namespace Heatington.Optimizer;

public class Opt
{
    private List<DataPoint>? _dataPoints = new List<DataPoint>();
    private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
    public List<ResultHolder>? Results { get; private set; } = new List<ResultHolder>();

    public void LoadData()
    {
        GetDataPoints();
        GetProductionUnits();

        _productionUnits = _productionUnits.OrderBy(o => o.ProductionCost).ToList();
    }

    // TODO: expand optimize with capability to optimize for co2
    public void Optimize()
    {
        //checks if there are no boilers that use electricity
        bool onlyFossil = _productionUnits.Sum(x => x.MaxElectricity) == 0;
        foreach (DataPoint dataPoint in _dataPoints)
        {
            if (!onlyFossil)
            {
                //orders production units by net production cost
                _productionUnits = _productionUnits.OrderBy(x =>
                    x.ProductionCost - x.MaxElectricity * dataPoint.ElectricityPrice).ToList();
            }

            ResultHolder result = CalculateHeatUnitsRequired(dataPoint, _productionUnits);
            Results?.Add(result);
        }
    }

    private List<ProductionUnit> SetOperationPoint(List<ProductionUnit> productionUnits, double heatDemand)
    {
        List<ProductionUnit> workingUnits = new();

        foreach (ProductionUnit unit in productionUnits)
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

    //this method now only calculates the heating units for a specified datapoint (before it was the whole datapoints set)
    private ResultHolder CalculateHeatUnitsRequired(DataPoint dataPoint, List<ProductionUnit> productionUnits)
    {
        // Calculates how many boilers are needed to satisfy heat demand
        int nOfBoilers = SatisfyHeatDemand(dataPoint);

        //Console.WriteLine("Number of boilers: {0}", nOfBoilers);

        // Selects the correct boilers based on how many need to be activated
        List<ProductionUnit> selectedBoilers = SelectBoiler(nOfBoilers);

        //Console.WriteLine("Boilers selected:");
        //selectedBoilers.ForEach(Console.WriteLine);

        // Sets the operation point of the selected boilers
        selectedBoilers = SetOperationPoint(selectedBoilers, dataPoint.HeatDemand);

        // Creates an object which holds the result
        ResultHolder result = new ResultHolder(dataPoint.StartTime, dataPoint.EndTime, dataPoint.HeatDemand,
            dataPoint.ElectricityPrice, selectedBoilers);
        Console.WriteLine(result);

        return result;

        int SatisfyHeatDemand(DataPoint dataPoint)
        {
            double currentProductionCapacity = 0;
            int i = 0;

            while (dataPoint.HeatDemand > currentProductionCapacity)
            {
                currentProductionCapacity = currentProductionCapacity + productionUnits[i].MaxHeat;
                i++;

                if (i > productionUnits.Count)
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
                selectedBoilers.Add(productionUnits[i]);
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

        Results.ForEach(Console.WriteLine);
    }

    private void GetDataPoints()
    {
        string fileName = "winter_period.csv";
        string filePath = Utilities.GeneratePathToFileInAssetsDirectory(fileName);

        IDataSource dataSource = new CsvController(filePath);

        SourceDataManager.SourceDataManager sourceDataManager = new(dataSource);

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
        // ProductionUnit controlBoiler = new ProductionUnit("Control Boiler", "", 5, 800, 0, 1.5, 310);
        // ProductionUnit gasBoiler = new ProductionUnit("Gas Boiler", "", 5, 500, 0, 1.1, 215);
        // ProductionUnit oilBoiler = new ProductionUnit("Oil Boiler", "", 4, 700, 0, 1.2, 265);
        //
        // _productionUnits.Add(controlBoiler);
        // _productionUnits.Add(oilBoiler);
        // _productionUnits.Add(gasBoiler);

        ProductionUnit gb = new ProductionUnit("Gas Boiler", "AssetManager/gas-boiler.jpg", 5, 500, 0, 1.1, 0);
        ProductionUnit ob = new ProductionUnit("Oil Boiler", "AssetManager/oil-boiler.jpg", 4, 700, 0, 1.2, 265);
        ProductionUnit gm = new ProductionUnit("Gas Motor", "AssetManager/gas-motor.jpg", 3.6, 1100, 2.7, 1.9, 640);
        ProductionUnit ek = new ProductionUnit("Electric Boiler", "AssetManager/electric-boiler.jpg", 8, 50, -8, 0, 0);

        _productionUnits.Add(gb);
        _productionUnits.Add(ob);
        _productionUnits.Add(gm);
        _productionUnits.Add(ek);
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
