namespace Heatington.Models
{
    /// <summary>
    /// Represents a data point containing information about heat demand and electricity price.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// Initializes a new instance of the DataPoint class with the specified start time, end time, heat demand, and electricity price.
        /// </summary>
        /// <param name="startTime">The start time of the data point.</param>
        /// <param name="endTime">The end time of the data point.</param>
        /// <param name="heatDemand">The heat demand of the data point in kWh.</param>
        /// <param name="electricityPrice">The electricity price of the data point in DKK.</param>
        public DataPoint(DateTime startTime, DateTime endTime, double heatDemand,double electricityPrice)
        {
            StartTime = startTime;
            EndTime = endTime;
            HeatDemand = heatDemand / 100.0; // Convert to kWh
            ElectricityPrice = electricityPrice / 100.0; // Convert to DKK
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
