using System.Globalization;
using System.Text.Json.Serialization;
using Heatington.Controllers;
using Heatington.Services.Serializers;

namespace Heatington.Models
{
    public class DataPoint
    {
        [CsvConstructor]
        public DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)
        {
            try
            {
                StartTime = DateTime.ParseExact(startTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
                EndTime = DateTime.ParseExact(endTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            }
            catch(FormatException)
            {
                StartTime = DateTime.ParseExact(startTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                EndTime = DateTime.ParseExact(endTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            }
            HeatDemand = double.Parse(heatDemand, CultureInfo.InvariantCulture);
            ElectricityPrice = double.Parse(electricityPrice, CultureInfo.InvariantCulture);
        }

        [JsonConstructor]
        public DataPoint(DateTime startTime, DateTime endTime, double heatDemand, double electricityPrice)
        {
            StartTime = startTime;
            EndTime = endTime;
            HeatDemand = heatDemand;
            ElectricityPrice = electricityPrice;
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
