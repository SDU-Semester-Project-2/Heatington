using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;
using Heatington.Services.Interfaces;

namespace Heatington.Optimizer;

public class Opt(AssetManager.AssetManager assetManager, SourceDataManager.SourceDataManager sourceDataManager)
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
        bool onlyFossil = _productionUnits.TrueForAll(x => x.MaxElectricity == 0);
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
        Task fetchTimeSeries = sourceDataManager.FetchTimeSeriesDataAsync();
        fetchTimeSeries.Wait();
        _dataPoints = sourceDataManager.TimeSeriesData;
    }

    public void LogDataPoints()
    {
        if (_dataPoints == null)
        {
            return;
        }

        _dataPoints.ForEach(Console.WriteLine);
    }

    private void GetProductionUnits()
    {
        Task loadAssets = assetManager.LoadAssets();
        loadAssets.Wait();
        _productionUnits = assetManager.ProductionUnits!.Values.ToList();
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
