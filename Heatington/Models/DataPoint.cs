using System.Globalization;
using Heatington.Utility;

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
        [CsvConstructor]
        public DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)
        {
            StartTime = DateTime.ParseExact(startTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(endTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            HeatDemand = double.Parse(heatDemand) / 100; // else 100x bigger, formatting issue TODO: Fix this
            ElectricityPrice = double.Parse(electricityPrice) / 100; // else 100x bigger, formatting issue TODO: Fix this
        }

        // TODO: Maybe implement factory method to increase the abstraction

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
