using System.Globalization;
using Heatington.Controllers;

namespace Heatington.Models
{

    public class DataPoint
    {

        [CsvConstructor]
        public DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)
        {
            StartTime = DateTime.ParseExact(startTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(endTime, "M/d/yy H:mm", CultureInfo.InvariantCulture);
            HeatDemand = double.Parse(heatDemand) / 100; // else 100x bigger, formatting issue TODO: Fix this
            ElectricityPrice = double.Parse(electricityPrice) / 100; // else 100x bigger, formatting issue TODO: Fix this
        }

        // TODO: Maybe implement factory method to increase the abstraction

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public double HeatDemand { get; }

        public double ElectricityPrice { get; }
    }
}
