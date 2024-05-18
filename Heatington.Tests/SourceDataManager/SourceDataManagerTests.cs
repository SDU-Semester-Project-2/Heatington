using System.Globalization;
using Heatington.Models;
using Heatington.Services.Interfaces;
using Heatington.SourceDataManager;

namespace Heatington.Tests.SourceDataManager
{
    public class StubDataSource : IDataSource
    {
        public List<DataPoint>? Data { get; set; }

        public Task<List<DataPoint>?> GetDataAsync()
        {
            return Task.FromResult((List<DataPoint>?)Data);
        }

        public void SaveData(List<DataPoint> data)
        {
            Data = data;
        }
    }

    public class SourceDataManagerTest
    {
        private readonly SDM _sourceDataManager;
        private readonly StubDataSource _stubDataSource;

        public SourceDataManagerTest()
        {
            _stubDataSource = new StubDataSource();
            _sourceDataManager = new(_stubDataSource);
        }

        [Theory]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97",
            "2/8/23 3:00", "2/8/23 4:00", "6.71", "1190.99",
            "2/8/23 4:00", "2/8/23 5:00", "6.73", "1191.01")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97",
            "2/8/23 3:00", "2/8/23 4:00", "6.71", "1190.99",
            "2/8/23 4:00", "2/8/23 5:00", "6.73", "1191.01",
            "2/8/23 5:00", "2/8/23 6:00", "6.75", "1191.03",
            "2/8/23 6:00", "2/8/23 7:00", "6.77", "1191.05")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97",
            "2/8/23 3:00", "2/8/23 4:00", "6.71", "1190.99",
            "2/8/23 4:00", "2/8/23 5:00", "6.73", "1191.01",
            "2/8/23 5:00", "2/8/23 6:00", "6.75", "1191.03",
            "2/8/23 6:00", "2/8/23 7:00", "6.77", "1191.05",
            "2/8/23 7:00", "2/8/23 8:00", "6.79", "1191.07",
            "2/8/23 8:00", "2/8/23 9:00", "6.81", "1191.09")]
        public async Task FetchTimeSeriesDataAsync_ShouldFetchDataSuccessfully(params string[] data)
        {
            // Arrange
            _stubDataSource.Data = [];
            for (int i = 0; i < data.Length; i += 4)
            {
                _stubDataSource.Data.Add(
                    new DataPoint(
                        DateTime.ParseExact(data[i], "M/d/yy H:mm", CultureInfo.InvariantCulture),
                        DateTime.ParseExact(data[i + 1], "M/d/yy H:mm", CultureInfo.InvariantCulture),
                        double.Parse(data[i + 2]),
                        double.Parse(data[i + 3])
                    ));
            }

            // Act
            await _sourceDataManager.FetchTimeSeriesDataAsync();

            // Assert
            Assert.Equal(_stubDataSource.Data.Count, _sourceDataManager.TimeSeriesData?.Count);
        }

        [Theory]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97",
            "2/8/23 3:00", "2/8/23 4:00", "6.71", "1190.99",
            "2/8/23 4:00", "2/8/23 5:00", "6.73", "1191.01")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97",
            "2/8/23 3:00", "2/8/23 4:00", "6.71", "1190.99",
            "2/8/23 4:00", "2/8/23 5:00", "6.73", "1191.01",
            "2/8/23 5:00", "2/8/23 6:00", "6.75", "1191.03",
            "2/8/23 6:00", "2/8/23 7:00", "6.77", "1191.05")]
        [InlineData("2/8/23 0:00", "2/8/23 1:00", "6.62", "1190.94",
            "2/8/23 1:00", "2/8/23 2:00", "6.65", "1190.99",
            "2/8/23 2:00", "2/8/23 3:00", "6.69", "1190.97",
            "2/8/23 3:00", "2/8/23 4:00", "6.71", "1190.99",
            "2/8/23 4:00", "2/8/23 5:00", "6.73", "1191.01",
            "2/8/23 5:00", "2/8/23 6:00", "6.75", "1191.03",
            "2/8/23 6:00", "2/8/23 7:00", "6.77", "1191.05",
            "2/8/23 7:00", "2/8/23 8:00", "6.79", "1191.07",
            "2/8/23 8:00", "2/8/23 9:00", "6.81", "1191.09")]
        public async Task VerifyDataPoints_ShouldMatchExpectedData(params string[] data)
        {
            // Arrange
            var expectedData = new List<DataPoint>();
            for (int i = 0; i < data.Length; i += 4)
            {
                expectedData.Add(
                    new DataPoint(
                        DateTime.ParseExact(data[i], "M/d/yy H:mm", CultureInfo.InvariantCulture),
                        DateTime.ParseExact(data[i + 1], "M/d/yy H:mm", CultureInfo.InvariantCulture),
                        double.Parse(data[i + 2]),
                        double.Parse(data[i + 3])
                    ));
            }

            _stubDataSource.Data = expectedData;

            // Act
            await _sourceDataManager.FetchTimeSeriesDataAsync();

            // Assert
            List<DataPoint>? actualData = _sourceDataManager.TimeSeriesData;
            Assert.NotNull(actualData);
            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                DataPoint expected = expectedData[i];
                DataPoint actual = actualData[i];
                Assert.Equal(expected.StartTime, actual.StartTime);
                Assert.Equal(expected.EndTime, actual.EndTime);
                Assert.Equal(expected.HeatDemand, actual.HeatDemand);
                Assert.Equal(expected.ElectricityPrice, actual.ElectricityPrice);
            }
        }
    }
}
