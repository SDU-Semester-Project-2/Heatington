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

        public override string ToString()
        {
            TimeZoneInfo danishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            DateTime startTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(StartTime, danishTimeZone);
            DateTime endTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(EndTime, danishTimeZone);

            string formattedStart = startTimeInDanish.ToString("dd.MM.yyyy HH:mm");
            string formattedEnd = endTimeInDanish.ToString("dd.MM.yyyy HH:mm");

            string s = string.Concat($"Start Time: {formattedStart} ", $"End Time: {formattedEnd}; ",
                $"Heat Demand: {HeatDemand} MWh; ", $"Electricity Price: {ElectricityPrice} DKK/MWh");

            return s;
        }
    }
}
