using Heatington.Models;

namespace Heatington.Optimizer;

public enum OptimizationMode
{
    Scenario1,
    Scenario2,
    Co2
}

public class OPT()
{
    private List<DataPoint>? _dataPoints = new List<DataPoint>();

    private readonly Delegate[] evals = new Delegate[]
    {
        new Func<ProductionUnit, double>((unit) => unit.ProductionCost), new Func<ProductionUnit, DataPoint, double>(
            (unit, dataPoint) =>
                unit.ProductionCost - unit.MaxElectricity * dataPoint.ElectricityPrice),
        new Func<ProductionUnit, double>((unit) => unit.Co2Emission)
    };

    private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
    public List<ResultHolder>? Results { get; private set; } = new List<ResultHolder>();

    public OPT(List<ProductionUnit> productionUnits, List<DataPoint>? dataPoints) : this()
    {
        _productionUnits = productionUnits;
        _dataPoints = dataPoints;
    }

    private void RankHeatingUnits(Func<ProductionUnit, double> evaluate)
    {
        _productionUnits = _productionUnits.OrderBy(o => evaluate(o)).ToList();

        OrderProductionUnits();
    }

    public void OrderProductionUnits()
    {
        _productionUnits = _productionUnits.OrderBy(o => o.ProductionCost).ToList();
    }

    // TODO: expand optimize with capability to optimize for co2
    public void Optimize(string[]? orderBy = null)
    {
        Console.WriteLine(_productionUnits.Count);
        Console.WriteLine(_dataPoints.Count);
        //checks if there are no boilers that use electricity
        bool onlyFossil = _productionUnits.TrueForAll(x => x.MaxElectricity == 0);
        Optimize((int a, int b) => a + b);
        /*
        if(!onlyFossil)
        {
            if(evals[(int)OptimizationMode.Scenario2] is Func<ProductionUnit, DataPoint, double> eval)
            {
                OptimizeForEachDataPoint(eval);
            }
            Optimize(OptimizationMode.Scenario2);
        }
        else
        {
            Optimize(OptimizationMode.Scenario1);
            if(evals[(int)OptimizationMode.Co2] is Func<ProductionUnit, double> eval)
            {
                OptimizeOnce(eval);
            }
        }
        */
    }

    public void Optimize(OptimizationMode mode)
    {
        Optimize(evals[(int)mode]);
    }

    public void Optimize(Delegate evaluation)
    {
        if (evaluation is Func<ProductionUnit, double> evalOnce)
        {
            OptimizeOnce(evalOnce);
            return;
        }
        else if (evaluation is Func<ProductionUnit, DataPoint, double> evalForEach)
        {
            OptimizeForEachDataPoint(evalForEach);
            return;
        }
        else
        {
            throw new Exception("Not a valid evaluation method.");
        }
    }

    private void OptimizeForEachDataPoint(Func<ProductionUnit, DataPoint, double> eval)
    {
        foreach (DataPoint dataPoint in _dataPoints)
        {
            RankHeatingUnits((x => eval(x, dataPoint)));

            ResultHolder result = CalculateHeatUnitsRequired(dataPoint, _productionUnits);
            Results?.Add(result);
        }
    }

    private void OptimizeOnce(Func<ProductionUnit, double> eval)
    {
        RankHeatingUnits(eval);
        foreach (DataPoint dataPoint in _dataPoints)
        {
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
        List<ProductionUnit> selectedBoilers = productionUnits.GetRange(0, nOfBoilers); // SelectBoiler(nOfBoilers);

        //Console.WriteLine("Boilers selected:");
        //selectedBoilers.ForEach(Console.WriteLine);

        // Sets the operation point of the selected boilers
        selectedBoilers = SetOperationPoint(selectedBoilers, dataPoint.HeatDemand);

        // Creates an object which holds the result
        ResultHolder result = new ResultHolder(dataPoint.StartTime, dataPoint.EndTime, dataPoint.HeatDemand,
            dataPoint.ElectricityPrice, selectedBoilers);
        // Console.WriteLine(result);

        return result;

        int SatisfyHeatDemand(DataPoint dataPointToSatisfy)
        {
            double currentProductionCapacity = 0;
            int i = 0;

            while (dataPointToSatisfy.HeatDemand > currentProductionCapacity)
            {
                currentProductionCapacity = currentProductionCapacity + productionUnits[i].MaxHeat;
                i++;

                if (i >= productionUnits.Count)
                {
                    Console.WriteLine("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
                    throw new Exception("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
                }
            }

            return i;
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

    public void LogDataPoints()
    {
        if (_dataPoints == null)
        {
            return;
        }

        _dataPoints.ForEach(Console.WriteLine);
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
