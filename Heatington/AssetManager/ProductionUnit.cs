namespace Heatington.AssetManager;

public class ProductionUnit
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PicturePath { get; set; }
    private double _operationPoint;
    public double MaxHeat { get; set; } // MW
    public double ProductionCost { get; set; } // DKK/MWh
    public double MaxElectricity { get; set; } // MW
    public double GasConsumption { get; set; } // MWh(gas/oil)/MWh
    public double Co2Emission { get; set; } // kg/MWh

    public double OperationPoint
    {
        get { return _operationPoint; }
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException("OperationPoint must be between 0 and 1");
            }
            _operationPoint = value;
        }
    }

    public ProductionUnit(string name, string picturePath, double maxHeat, double productionCost, double maxElectricity,
        double gasConsumption, double co2Emission)
    {
        Id = Guid.NewGuid();
        Name = name;
        PicturePath = picturePath;
        OperationPoint = 1; // operation point set to 1 on default, meaning when created it is on maximum capacity
        MaxHeat = maxHeat;
        ProductionCost = productionCost;
        MaxElectricity = maxElectricity;    // not every unit has electricity, in that case set the value to 0
        GasConsumption = gasConsumption;    // not every unit has gas, in that case set the value to 0
        Co2Emission = co2Emission;          // not every unit has co2 emission, in that case set the value to 0
    }
}
