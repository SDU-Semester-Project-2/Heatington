namespace Heatington.Models
{
    public class DataPoint
    {
        public DataPoint(DateTime startTime, DateTime endTime, double heatDemand,double electricityPrice)
        {
            StartTime = startTime;
            EndTime = endTime;
            HeatDemand = heatDemand / 100.0; // Convert to kWh
            ElectricityPrice = electricityPrice / 100.0; // Convert to DKK
        }

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double HeatDemand { get; }
        public double ElectricityPrice { get; }
    }
}
