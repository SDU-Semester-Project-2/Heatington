using Heatington.Models;
using Heatington.Data;

namespace Heatington.Tests.Data
{
    public class CsvDataSourceTests : IDisposable
    {
        private readonly string _tempFilePath;

        // I've to create tempFile because I am not allowed to use Moq, which is usually from what I've read, the way to go.
        public CsvDataSourceTests()
        {
            // Setup - create a temp CSV file
            _tempFilePath = Path.GetTempFileName();
            const string sampleCsvData = "1/1/24 12:00,1/1/24 13:00,6.86,625.29";
            File.WriteAllText(_tempFilePath, sampleCsvData);
        }

        public void Dispose()
        {
            // Teardown - delete the temp file
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        [Fact]
        public async Task GetDataAsync_ShouldReturnDistData_GivenValidCsvFile()
        {
            // Arrange
            CsvDataSource dataSource = new();

            // Act
            List<DataPoint>? result = await dataSource.GetDataAsync(_tempFilePath);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(new DateTime(2024, 1, 1, 12, 0, 0), result[0].StartTime);
            Assert.Equal(new DateTime(2024, 1, 1, 13, 0, 0), result[0].EndTime);
            Assert.Equal(6.86, result[0].HeatDemand);
            Assert.Equal(625.29, result[0].ElectricityPrice);
        }

        [Fact]
        public async Task GetDataAsync_ShouldThrowException_GivenNonExistFile()
        {
            // Arrange
            var dataSource = new CsvDataSource();

            // Act and Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => dataSource.GetDataAsync("non-exist.csv"));
        }

        // TODO: Implement this after SaveData() is actually use
        // [Fact]
        // public void SaveData_ShouldThrowNotImplementedException()
        // {
        //     // Arrange
        //     var dataSource = new CsvDataSource();
        //
        //     // Act and Assert
        //     Assert.Throws<NotImplementedException>(() => dataSource.SaveData(new List<DataPoint>(), _tempFilePath));
        // }
    }
}
