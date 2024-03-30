using FrancescoDummyClasses;

namespace Heatington.Optimizer;

public class Optimizer
{
    private List<ProductionUnitFs> _pUnits = new List<ProductionUnitFs>();
    private List<FrancescoEnergyData> _energyDataEntries = new List<FrancescoEnergyData>();
    private List<ResultHolder> _resultEntries = new List<ResultHolder>();

    public void OptimizerScenario1()
    {
        GetProductionUnits();
        GetTimeSeriesData();

        // Sorts the production Units by their production cost.
        _pUnits = _pUnits.OrderBy(o => o.ProductionCost).ToList();

        int numberOfBoilers = 0;

        foreach (var entry in _energyDataEntries)
        {
            numberOfBoilers = CalculateHeatUnitsRequired(entry);
            _resultEntries.Add(new ResultHolder(entry.StartTime, entry.EndTime, numberOfBoilers));
        }
    }

    private int CalculateHeatUnitsRequired(FrancescoEnergyData entry)
    {
        double totProductionCapacity = 0;
        int i = 0;

        totProductionCapacity += _pUnits[i].MaxHeat;

        while (entry.HeatDemandMwh > totProductionCapacity)
        {
            totProductionCapacity += _pUnits[i].MaxHeat;
            i++;

            if (i > _pUnits.Count)
            {
                Console.WriteLine("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
                throw new Exception("WARNING: HEAT DEMAND CAN NOT BE SATISFIED");
            }
        }
        return i;
    }

    // Method which calculates Hourly NetProductionCost for heat only boilers
    public void CalculateNetProductionCost()
    {
        double hourlyProductionCost = 0;

        foreach (var entry in _resultEntries)
        {
            hourlyProductionCost = GetCost(entry.NumberOfBoilers);
            Console.WriteLine(hourlyProductionCost + "\tkr");
        }

        double GetCost(int numberOfBoilers)
        {
            double totalCost = 0;

            for (int i = 0; i <= numberOfBoilers; i++)
            {
                totalCost += _pUnits[i].ProductionCost;
            }

            return totalCost;
        }
    }

    // Tiny class used to hold results calculated by the Scenario 1 optimizer.
    // Needs to be refactored deleted once RDM is implemented
    private class ResultHolder(DateTime startTime, DateTime endTime, int numberOfBoilers)
    {
        public DateTime StartTime { get; set; } = startTime;
        public DateTime EndTime { get; set; } = endTime;
        public int NumberOfBoilers { get; set; } = numberOfBoilers;

        public override string ToString()
        {
            string s = string.Concat(StartTime, " ", EndTime, " ", NumberOfBoilers);
            return s;
        }
    }

    // Will eventually call AssetManager to get the production units.
    // Right now it just initialises two and adds them to the list.
    // SOLID is applied: if I add productionUnits by calling the assetManager it will behave in the same way.
    private bool GetProductionUnits()
    {
        ProductionUnitFs oilBoiler = new ProductionUnitFs("Oil boiler", 0, 4, 700, 0, 1.2, 265);
        ProductionUnitFs gasBoiler = new ProductionUnitFs("Gas boiler", 0, 5, 500, 0, 1.1, 215);
        ProductionUnitFs fantasyBoiler = new ProductionUnitFs("Fantasy boiler", 0, 3, 1000, 0, 1.4, 300);
        _pUnits.Add(oilBoiler);
        _pUnits.Add(fantasyBoiler);
        _pUnits.Add(gasBoiler);

        return true;
    }

    // Will eventually call SDM to get the time series.
    // Right now it just uses the tiny csv controller to load all data into a list
    private bool GetTimeSeriesData()
    {
        string path = "../Assets/winter_period.csv";
        FrancescoCsvController francescoCsvController = new FrancescoCsvController(path);

        _energyDataEntries = francescoCsvController.ReadTimeSeriesEnergyData();
        Console.WriteLine("Loaded entries");

        return true;
    }
}
