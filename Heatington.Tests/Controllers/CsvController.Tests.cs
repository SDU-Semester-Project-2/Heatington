using Heatington.Controllers;
using Heatington.Controllers.Interfaces;
using Heatington.Models;

namespace Heatington.Tests.Controllers;

/// <summary>
/// Documentation in Documents/Heatington.Tests/Controllers/FileController.Tests.md
/// </summary>
public class CsvControllerTests : IDisposable
{
    private const string TestsDirectory = "tests";
    private readonly string _testsDirPath = Path.Combine(Path.GetTempPath(), TestsDirectory);

    public CsvControllerTests() // NOT A TEST
    {
        // create temporary test folder
        if (!Directory.Exists(_testsDirPath))
        {
            Directory.CreateDirectory(_testsDirPath);
        }
        else
        {
            ClearTestsDirectory();
        }
    }

    [Fact]
    public async void RegularTimeSeriesData_GetDataAsync_ReadsDataCorrectly()
    {
        //Arrange
        string TestFilePath = Path.Combine(_testsDirPath, Path.GetRandomFileName());
        string fileContent = String.Join("\n",
            "2/8/23 2:00,2/8/23 3:00,6.98,1116.22",
            "2/8/23 3:00,2/8/23 4:00,7.04,1101.12",
            "2/8/23 4:00,2/8/23 5:00,7.72,1086.24",
            "2/8/23 5:00,2/8/23 6:00,7.85,1109.53",
            "2/8/23 6:00,2/8/23 7:00,8.15,1307.40",
            "2/8/23 7:00,2/8/23 8:00,7.62,1463.30"
        );
        List<DataPoint>? expectedOutput = new List<DataPoint>
        {
            new("2/8/23 2:00","2/8/23 3:00","6.98","1116.22"),
            new("2/8/23 3:00","2/8/23 4:00","7.04","1101.12"),
            new("2/8/23 4:00","2/8/23 5:00","7.72","1086.24"),
            new("2/8/23 5:00","2/8/23 6:00","7.85","1109.53"),
            new("2/8/23 6:00","2/8/23 7:00","8.15","1307.40"),
            new("2/8/23 7:00","2/8/23 8:00","7.62","1463.30")
        };
        CsvController mockCsvController = new CsvController(TestFilePath);
        await File.WriteAllTextAsync(TestFilePath, fileContent);

        //Act
        List<DataPoint>? actualOutput = await mockCsvController.GetDataAsync();

        //Assert
        Assert.Equal(expectedOutput, actualOutput);
    }

    [Fact]
    public void RegularTimeSeriesData_SaveData_WritesCorrectContent()
    {
        //Arrange
        string TestFilePath = Path.Combine(_testsDirPath, Path.GetRandomFileName());
        string expectedContent = String.Join("\n",
            "StartTime,EndTime,HeatDemand,ElectricityPrice",
            "2/8/23 2:00,2/8/23 3:00,6.98,1116.22",
            "2/8/23 3:00,2/8/23 4:00,7.04,1101.12",
            "2/8/23 4:00,2/8/23 5:00,7.72,1086.24",
            "2/8/23 5:00,2/8/23 6:00,7.85,1109.53",
            "2/8/23 6:00,2/8/23 7:00,8.15,1307.40",
            "2/8/23 7:00,2/8/23 8:00,7.62,1463.30"
        );
        List<DataPoint>? timeSeries = new List<DataPoint>
        {
            new("2/8/23 2:00","2/8/23 3:00","6.98","1116.22"),
            new("2/8/23 3:00","2/8/23 4:00","7.04","1101.12"),
            new("2/8/23 4:00","2/8/23 5:00","7.72","1086.24"),
            new("2/8/23 5:00","2/8/23 6:00","7.85","1109.53"),
            new("2/8/23 6:00","2/8/23 7:00","8.15","1307.40"),
            new("2/8/23 7:00","2/8/23 8:00","7.62","1463.30")
        };
        CsvController mockCsvController = new CsvController(TestFilePath);

        //Act
        mockCsvController.SaveData(timeSeries);

        //Assert
        string actualContent = File.ReadAllText(TestFilePath);
        Assert.Equal(expectedContent, actualContent);
    }

    private void ClearTestsDirectory() // NOT A TEST
    {
        // clear and remove temporary test folder
        DirectoryInfo di = new DirectoryInfo(_testsDirPath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    public void Dispose()
    {
        //Clear Tests Directory
        ClearTestsDirectory();
    }
}
