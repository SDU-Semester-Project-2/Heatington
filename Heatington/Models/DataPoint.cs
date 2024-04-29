using System.Globalization;
using System.Text.Json.Serialization;
using Heatington.Services.Serializers;

namespace Heatington.Models
{
    public class DataPoint : IEquatable<DataPoint>
    {
        [CsvConstructor]
        public DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)
        {
            try
            {
                StartTime = DateTime.ParseExact(startTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
                EndTime = DateTime.ParseExact(endTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                // var formatTime = (string time) => Regex.Replace(time, @"[^0-9]", "");
                // startTime = formatTime(startTime);
                // endTime = formatTime(endTime);

                string dateTimeFormat = "M/d/yyyy H:mm:ss tt";

                if ((DateTime.TryParseExact(
                         startTime,
                         dateTimeFormat,
                         CultureInfo.InvariantCulture,
                         DateTimeStyles.AssumeLocal,
                         out var start)
                     &&
                     DateTime.TryParseExact(
                         endTime,
                         dateTimeFormat,
                         CultureInfo.InvariantCulture,
                         DateTimeStyles.AssumeLocal,
                         out var end)
                    ))
                {
                    StartTime = start;
                    EndTime = end;
                }
                else
                {
                    // last ditch try bcs we still getting errors
                    StartTime = DateTime.Parse(startTime);
                    EndTime = DateTime.Parse(endTime);
                }
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

        //needed for tests
        public override bool Equals(object obj) => this.Equals(obj as DataPoint);

        public bool Equals(DataPoint p)
        {
            if (p is null)
            {
                return false;
            }
            if (Object.ReferenceEquals(this, p))
            {
                return true;
            }
            if (this.GetType() != p.GetType())
            {
                return false;
            }
            return (StartTime == p.StartTime) && (EndTime == p.EndTime) && (HeatDemand == p.HeatDemand) && (ElectricityPrice == p.ElectricityPrice);
        }

        public override int GetHashCode() => (StartTime, EndTime, HeatDemand, ElectricityPrice).GetHashCode();

        public static bool operator ==(DataPoint lhs, DataPoint rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(DataPoint lhs, DataPoint rhs) => !(lhs == rhs);
    }
}
