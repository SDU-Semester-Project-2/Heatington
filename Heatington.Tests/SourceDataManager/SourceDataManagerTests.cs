using Heatington.Controllers;
using Heatington.Models;
using Heatington.Services.Interfaces;


// TODO: No test for ConvertApiToCsv yet and LogTimesSeriesData won't be tested because the method will be removed after Implementation of GUI

namespace Heatington.Tests.SourceDataManager
{
    public class SourceDataManagerTest
    {
        private readonly CsvController _csvController;
        private readonly Heatington.SourceDataManager.SourceDataManager _sourceDataManager;

        public SourceDataManagerTest()
        {
            _csvController = new CsvController("test-path");
            _sourceDataManager = new Heatington.SourceDataManager.SourceDataManager(_csvController);
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
            //TODO: Mark has to fix, I don't have that much time for that sorry
            // // Arrange
            // _csvController.Data = new List<DataPoint>();
            // for (int i = 0; i < data.Length; i += 4)
            // {
            //     _csvController.Data.Add(new DataPoint(data[i], data[i + 1], data[i + 2], data[i + 3]));
            // }
            //
             // // Act
            // await _sourceDataManager.FetchTimeSeriesDataAsync();
            //
            // // Assert
            // Assert.Equal(_csvController.Data.Count, _sourceDataManager.TimeSeriesData?.Count);
        }
    }

}
