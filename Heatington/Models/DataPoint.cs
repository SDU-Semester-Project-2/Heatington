namespace Heatington.Models
{
    public class DataPoint
    {
        [CsvConstructor]
        public DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)
        {
            StartTime = startTime;
            EndTime = endTime;
            HeatDemand = heatDemand / 100.0; // Convert to kWh
            ElectricityPrice = electricityPrice / 100.0; // Convert to DKK
        }

        // TODO: Maybe implement factory method to increase the abstraction

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double HeatDemand { get; }
        public double ElectricityPrice { get; }
    }
}
