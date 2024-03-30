namespace Heatington.Models;

/// <summary>
/// Represents a data point containing information about time, heat demand, and electricity price.
/// </summary>
public class DataPoint
{
    /// <summary>
    /// Represents the time of a data point.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Represents the heat demand value for a specific time.
    /// </summary>
    public double HeatDemand { get; set; }

    /// <summary>
    /// Represents the electricity price for a specific data point.
    /// </summary>
    public double ElectricityPrice  { get; set; }
}
