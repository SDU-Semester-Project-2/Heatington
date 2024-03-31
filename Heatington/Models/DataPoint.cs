using System.Globalization;

namespace Heatington.Models
{
    /// <summary>
    /// Represents a data point containing information about heat demand and electricity price.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// Represents a data point containing information about heat demand and electricity price.
        /// </summary>
        [CsvHandle.CsvConstructorAttribute]
        public DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)
        {
            this.StartTime = DateTime.ParseExact(startTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            this.EndTime = DateTime.ParseExact(endTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            this.HeatDemand = double.Parse(heatDemand);
            this.ElectricityPrice = double.Parse(electricityPrice);
        }

        /// <summary>
        /// Gets the start time of a data point.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Gets the end time of the data point.
        /// </summary>
        public DateTime EndTime { get; }

        /// <summary>
        /// Represents a data point with heat demand information.
        /// </summary>
        public double HeatDemand { get; }

        /// <summary>
        /// Represents the electricity price for a given data point.
        /// </summary>
        public double ElectricityPrice { get; }
    }
}
