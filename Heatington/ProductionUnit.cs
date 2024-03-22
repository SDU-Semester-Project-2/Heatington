namespace Heatington;

public class ProductionUnit(string name, double operationPoint, double maxHeat, double productionCost, double maxElectricity, double gasConsumption, double co2Emissions)
{
    public Guid Id = new Guid();
    public string Name { get; set; } = name;
    public double OperationPoint { get; set; } = operationPoint;
    public double MaxHeat { get; set; } = maxHeat;
    public double ProductionCost { get; set; } = productionCost;
    public double MaxElectricity { get; set; } = maxElectricity;
    public double GasConsumption { get; set; } = gasConsumption;
    public double Co2Emissions { get; set; } = co2Emissions;

    public override string ToString()
    {
        string s = string.Concat("Name: ", Name, " Operation point: ", operationPoint, " Production cost: ", productionCost,
            " Max electricity: ", maxElectricity, " Gas consumption: ", gasConsumption, " Co2 emissions: ", co2Emissions);
        return s;
    }
}
