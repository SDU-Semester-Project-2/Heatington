using Heatington.Optimizer;

namespace Heatington.Models;

public class ResultHolder
{
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    public double HeatDemand { get; }
    public double ElectricityPrice { get; }
    public double NetProductionCost { get; }
    public List<ProductionUnit> Boilers { get; set; }

    public override string ToString()
    {
        TimeZoneInfo danishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
        DateTime startTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(StartTime, danishTimeZone);
        DateTime endTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(EndTime, danishTimeZone);

        string formattedStart = startTimeInDanish.ToString("dd.MM.yyyy HH:mm");
        string formattedEnd = endTimeInDanish.ToString("dd.MM.yyyy HH:mm");

        string boilers = string.Empty;

        foreach (ProductionUnit productionUnit in Boilers)
        {
            boilers = string.Concat(boilers, "\n", $"Name: {productionUnit.Name} ",
                $"Production cost: {productionUnit.ProductionCost }",
                $"Operation point: {productionUnit.OperationPoint}");
        }

        string s = string.Concat($"Start Time: {formattedStart} ", $"End Time: {formattedEnd}; ",
            $"Heat Demand: {HeatDemand} MWh; ", $"Electricity Price: {ElectricityPrice} DKK/MWh",
            $"Net Production Cost {NetProductionCost}", boilers);

        return s;
    }
}
