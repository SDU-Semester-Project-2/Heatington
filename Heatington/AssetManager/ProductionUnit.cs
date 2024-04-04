namespace Heatington.AssetManager;

public class ProductionUnit
{
    public Guid Id { get; }
    public string Name { get; set; }
    public string PicturePath { get; }
    private double _operationPoint;
    public double MaxHeat { get; } // MW
    public double ProductionCost { get; } // DKK/MWh
    public double MaxElectricity { get; } // MW
    public double GasConsumption { get; } // MWh(gas/oil)/MWh
    public double Co2Emission { get; } // kg/MWh

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
        OperationPoint = 0;                 // operation point set to 0 on default, meaning when created it is turned off
        MaxHeat = maxHeat;
        ProductionCost = productionCost;
        MaxElectricity = maxElectricity;    // not every unit has electricity, in that case set the value to 0
        GasConsumption = gasConsumption;    // not every unit has gas, in that case set the value to 0
        Co2Emission = co2Emission;          // not every unit has co2 emission, in that case set the value to 0
    }

    public override string ToString()
    {
        if (MaxElectricity != 0 && GasConsumption == 0)
        {
            return $"ID:{Id}, Name:{Name}, Operation Point:{OperationPoint}, Max Heat:{MaxHeat}, Production Cost:{ProductionCost}\n" +
                   $"Max Electricity:{MaxElectricity}";
        }
        else if (MaxElectricity == 0 && GasConsumption != 0)
        {
            return $"ID:{Id}, Name:{Name}, Operation Point:{OperationPoint}, Max Heat:{MaxHeat}, Production Cost:{ProductionCost}\n" +
                   $"Gas Consumption:{GasConsumption}, CO2 Emission:{Co2Emission}";
        }
        else
        {
            return $"ID:{Id}, Name:{Name}, Operation Point:{OperationPoint}, Max Heat:{MaxHeat}, Production Cost:{ProductionCost}\n" +
                   $"Max Electricity:{MaxElectricity}, Gas Consumption:{GasConsumption}, CO2 Emission:{Co2Emission}";
        }
    }
}
