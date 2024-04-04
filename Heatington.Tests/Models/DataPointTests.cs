using Heatington.Models;
using System.Globalization;
using Xunit.Abstractions;

namespace Heatington.Tests.Models
{
    public class DataPointTests
    {
        private readonly ITestOutputHelper _output;

        public DataPointTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Constructor_ShouldParse_ValidArguments()
        {
            const string startTime = "1/1/21 12:00";
            const string endTime = "1/1/21 13:00";
            const string heatDemand = "1.23";
            const string electricityPrice = "4.56";

            var dataPoint = new DataPoint(startTime, endTime, heatDemand, electricityPrice);

            _output.WriteLine(
                $"StartTime: Expected {startTime}, got {dataPoint.StartTime.ToString("M/d/yy H:mm", CultureInfo.InvariantCulture)}");
            _output.WriteLine(
                $"EndTime: Expected {endTime}, got {dataPoint.EndTime.ToString("M/d/yy H:mm", CultureInfo.InvariantCulture)}");
            _output.WriteLine(
                $"HeatDemand: Expected {heatDemand}, got {dataPoint.HeatDemand.ToString(CultureInfo.InvariantCulture)}");
            _output.WriteLine(
                $"ElectricityPrice: Expected {electricityPrice}, got {dataPoint.ElectricityPrice.ToString(CultureInfo.InvariantCulture)}");

            Assert.Equal(DateTime.ParseExact(startTime, "M/d/yy H:mm", CultureInfo.InvariantCulture),
                dataPoint.StartTime);
            Assert.Equal(DateTime.ParseExact(endTime, "M/d/yy H:mm", CultureInfo.InvariantCulture), dataPoint.EndTime);
            Assert.Equal(double.Parse(heatDemand, CultureInfo.InvariantCulture), dataPoint.HeatDemand);
            Assert.Equal(double.Parse(electricityPrice, CultureInfo.InvariantCulture), dataPoint.ElectricityPrice);
        }

        [Fact]
        public void Constructor_ShouldThrowFormatException_ForInvalidArguments()
        {
            const string invalidTime = "invalid time";
            const string invalidDouble = "invalid double";

            Assert.Throws<FormatException>(() => new DataPoint(invalidTime, "1/1/21 13:00", "1.23", "4.56"));
            Assert.Throws<FormatException>(() => new DataPoint("1/1/21 12:00", invalidTime, "1.23", "4.56"));
            Assert.Throws<FormatException>(() => new DataPoint("1/1/21 12:00", "1/1/21 13:00", invalidDouble, "4.56"));
            Assert.Throws<FormatException>(() => new DataPoint("1/1/21 12:00", "1/1/21 13:00", "1.23", invalidDouble));
        }
    }
}
