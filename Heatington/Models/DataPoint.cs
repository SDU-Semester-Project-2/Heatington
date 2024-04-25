using System.Globalization;
using Heatington.Controllers;
using Heatington.Services.Serializers;

namespace Heatington.Models
{
    public class DataPoint
    {
        [CsvConstructor]
        public DataPoint(DateTime startTime, DateTime endTime, double heatDemand, double electricityPrice)
        {
            StartTime = startTime;
            EndTime = endTime;
            HeatDemand = heatDemand;
            ElectricityPrice = electricityPrice;
        }

        public DataPoint()
        {
        }


        // TODO: Maybe implement factory method to increase the abstraction

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public double HeatDemand { get; init; }

        public double ElectricityPrice { get; init; }

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
