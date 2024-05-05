using Heatington.Models;

namespace Heatington.Optimizer;

public class OPT(AssetManager.AM am, SourceDataManager.SDM sdm)
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
    public void Optimize(string[]? orderBy = null)
    {
        //checks if there are no boilers that use electricity
        bool onlyFossil = _productionUnits.TrueForAll(x => x.MaxElectricity == 0);
        if (_dataPoints != null)
        {
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
        List<ProductionUnit> selectedBoilers = SelectBoilers(nOfBoilers);

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

                if (i > productionUnits.Count)
                {
                    Console.WriteLine("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
                    throw new Exception("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
                }
            }

            return i;
        }

        List<ProductionUnit> SelectBoilers(int j) => productionUnits[..j];
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
        Task fetchTimeSeries = sdm.FetchTimeSeriesDataAsync();
        fetchTimeSeries.Wait();
        _dataPoints = sdm.TimeSeriesData;
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
        Task loadAssets = am.LoadAssets();
        loadAssets.Wait();
        _productionUnits = am.ProductionUnits!.Values.ToList();
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
