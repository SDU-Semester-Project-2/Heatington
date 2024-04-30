using System.Text.Json;
using Heatington.Controllers;
using Heatington.Controllers.Enums;
using Heatington.Controllers.Interfaces;
using Heatington.Models;
using Heatington.Tests;

namespace Heatington.Tests.Controllers;
// Replace with your actual namespace

public class JsonControllerTests : UseTestDirectory
{
    [Fact]
    public async Task ReadProductionUnitFromFile_ReadData_ReadsCorrectJson()
    {
        // Arrange
        string TestFilePath = Path.Combine(TestsDirPath, Path.GetRandomFileName());
        ProductionUnit expectedProductionUnit = new("TU", "", 10, 250, 7, 1.1, 8.0);
        IReadWriteController jsonController = new JsonController(TestFilePath);
        ProductionUnit actualProductionUnit;

        // Act
        File.WriteAllText(TestFilePath, JsonSerializer.Serialize(expectedProductionUnit));
        actualProductionUnit = await jsonController.ReadData<ProductionUnit>();

        // Assert
        Assert.Equivalent(expectedProductionUnit, actualProductionUnit);
    }

    [Fact]
    public async Task WriteData_ShouldSerializeData()
    {
        // // Arrange
        // var controller = new JsonController(TestFilePath);
        // var testData = new MyDataModel(); // Replace with your test data
        //
        // // Act
        // var status = await controller.WriteData(testData);
        //
        // // Assert
        // Assert.Equal(OperationStatus.Success, status);
        // // Add more specific assertions or verify the file content
    }
}
