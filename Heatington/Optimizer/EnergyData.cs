using CsvHelper.Configuration.Attributes;

namespace Heatington.Optimizer;
public struct EnergyData
{
    [Index(0)]
    public DateTime StartTime { get; set; }
    [Index(1)]
    public DateTime EndTime { get; set; }
    [Index(2)]
    public double HeatDemandMwh { get; set; }
    [Index(3)]
    public double ElectricityPrice { get; set; }

    public override string ToString()
    {
        string s = string.Concat(StartTime," ", EndTime," ",HeatDemandMwh," ",ElectricityPrice);
        return s;
    }
}
