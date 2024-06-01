namespace Heatington.Models;

public class ResultHolder(
    DateTime startTime,
    DateTime endTime,
    double heatDemand,
    double electricityPrice,
    List<ProductionUnit> boilers) : ICloneable
{
    public DateTime StartTime { get; } = startTime;
    public DateTime EndTime { get; } = endTime;
    public double HeatDemand { get; } = heatDemand;
    public double ElectricityPrice { get; } = electricityPrice;
    public List<ProductionUnit> Boilers { get; set; } = boilers;
    public double NetProductionCost { get; set; } = Math.Round(boilers.Sum(x => x.OperationPoint * (x.ProductionCost * x.MaxHeat - x.MaxElectricity * electricityPrice)), 4);

    public object Clone()
    {
        return this.MemberwiseClone();
    }

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
            boilers = string.Concat(boilers, "\n", $"Name: {productionUnit.Name}; ",
                $"Net Production cost: {Math.Round(productionUnit.ProductionCost - productionUnit.MaxElectricity * electricityPrice, 4)}; ",
                $"Operation point: {productionUnit.OperationPoint}");
        }

        string s = string.Concat($"\n\nStart Time: {formattedStart}; ", $"End Time: {formattedEnd}; ",
            $"Heat Demand: {HeatDemand} MWh; ", $"Electricity Price: {ElectricityPrice} DKK/MWh; ",
            $"Net Production Cost {NetProductionCost} ", boilers);

        return s;
    }

}
