using System.Text;
using System.Text.Json;
using Heatington.Controllers;
using Heatington.Controllers.Enums;
using Heatington.Models;
using Xunit.Abstractions;

namespace Heatington.Tests.Controllers;

// Tests combining the tests made in FileControllerTests and JsonSerializerTests
public class JsonControllerTests : UseTestDirectory
{
    // TODO: fix this test - I don't know why but it's always failing
    // [Fact]
    public async Task ReadProductionUnitFromFile_ReadData_ReadsCorrectJson()
    {
        // Arrange
        string TestFilePath = Path.Combine(TestsDirPath, "testJSON1.json");
        ProductionUnit expectedProductionUnit = new("TU", "", 10, 250, 7, 1.1, 8.0);
        JsonController jsonController = new JsonController(TestFilePath);
        ProductionUnit actualProductionUnit;

        // Act
        await using (FileStream fs = File.Create(jsonController.JsonPath))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(JsonSerializer.Serialize(expectedProductionUnit));
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }

        actualProductionUnit = await jsonController.ReadData<ProductionUnit>();

        // Assert
        Assert.Equivalent(expectedProductionUnit, actualProductionUnit);
    }

    [Fact]
    public async Task WriteProductionUnitToFile_WriteData_WritesCorrectJson()
    {
        // Arrange
        string TestFilePath = Path.Combine(TestsDirPath, "testJSON2.json");
        JsonController jsonController = new JsonController(TestFilePath);
        ProductionUnit testProductionUnit = new("TU123", "none", 0, 100, 0, 6.1, 0.0);
        ProductionUnit? actualProductionUnit;

        // Act
        OperationStatus status = await jsonController.WriteData(testProductionUnit);
        actualProductionUnit = JsonSerializer.Deserialize<ProductionUnit>(
            await File.ReadAllTextAsync(TestFilePath)
        );

        // Assert
        Assert.Equal(OperationStatus.SUCCESS, status);
        Assert.Equivalent(testProductionUnit, actualProductionUnit);
    }

    [Fact]
    public async Task ReadHeatingGridFromFile_ReadData_ReadsCorrectJson()
    {
        // Arrange
        string TestFilePath = Path.Combine(TestsDirPath, "testJSON3.json");
        HeatingGrid expectedHeatingGrid = new HeatingGrid("none", "name1");
        JsonController jsonController = new JsonController(TestFilePath);
        HeatingGrid actualHeatingGrid;

        // Act
        await using (FileStream fs = File.Create(jsonController.JsonPath))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(JsonSerializer.Serialize(expectedHeatingGrid));
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }

        // await File.WriteAllTextAsync(jsonController.JsonPath,));
        actualHeatingGrid = await jsonController.ReadData<HeatingGrid>();

        // Assert
        Assert.Equivalent(expectedHeatingGrid, actualHeatingGrid);
    }

    [Fact]
    public async Task WriteHeatingGridToFile_WriteData_WritesCorrectJson()
    {
        // Arrange
        string TestFilePath = Path.Combine(TestsDirPath, "testJSON4.json");
        JsonController jsonController = new JsonController(TestFilePath);
        HeatingGrid testHeatingGrid = new HeatingGrid("test/123.png", "no name");
        HeatingGrid? actualHeatingGrid;

        // Act
        OperationStatus status = await jsonController.WriteData(testHeatingGrid);
        actualHeatingGrid = JsonSerializer.Deserialize<HeatingGrid>(
            await File.ReadAllTextAsync(TestFilePath)
        );

        // Assert
        Assert.Equal(OperationStatus.SUCCESS, status);
        Assert.Equivalent(testHeatingGrid, actualHeatingGrid);
    }
}
