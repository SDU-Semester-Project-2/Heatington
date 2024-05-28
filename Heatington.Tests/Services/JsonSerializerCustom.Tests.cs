using System.Text.Json;
using Heatington.Models;
using Heatington.Services.Serializers;

namespace Heatington.Tests.Services;

public class JsonSerializerCustomTests
{
    class EmptyClass
    {
    }

    // STRING INTO JSON
    [Fact]
    public void Deserialize_DeserializeEmptyString_GetsEmptyJson()
    {
        //Arrange
        const string testString = "{}";
        EmptyClass expectedValue = new();
        EmptyClass actualValue;

        //Act
        actualValue = JsonSerializerCustom.Deserialize<EmptyClass>(testString);

        //Assert
        Assert.Equivalent(expectedValue, actualValue);
    }

    [Fact]
    public void Deserialize_DeserializeEmptyString_ConvertsToTheRightType()
    {
        //Arrange
        const string testString = "{}";
        EmptyClass actualValue;

        //Act
        actualValue = JsonSerializerCustom.Deserialize<EmptyClass>(testString);

        //Assert
        Assert.IsType<EmptyClass>(actualValue);
    }

    [InlineData("""
                    {
                        "Name": "TU",
                        "FullName": "",
                        "PicturePath": "",
                        "MaxHeat": 10,
                        "ProductionCost": 250,
                        "MaxElectricity": 7,
                        "GasConsumption": 1.1,
                        "Co2Emission":8.0
                    }
                """)]
    [InlineData("""
                    {
                        "Name": "TU",
                        "FullName": "",
                        "PicturePath": "",
                        "MaxHeat": 10,
                        "ProductionCost": 250.0,
                        "MaxElectricity": 7,
                        "GasConsumption": 1.1,
                        "Co2Emission":8
                    }
                """)]
    // TODO: fix this
    [Theory]
    public void Deserialize_DeserializeProductionUnitString_GetsProductionUnit(string testUnitString)
    {
        //Arrange
        ProductionUnit expectedUnit = new("TU", "", "", 10, 250, 7, 1.1, 8.0);
        ProductionUnit actualUnit;

        //Act
        actualUnit = JsonSerializerCustom.Deserialize<ProductionUnit>(testUnitString);

        //Assert
        Assert.Equivalent(expectedUnit, actualUnit);
    }

    [InlineData("""
                {
                    "PicturePath": "",
                    "Name": "Heating Grid Test",
                    "FullName":"",
                    "Architecture":	"Test description, testing architecture",
                    "Size":	"0 homes, 2 pipes",
                    "City":	"Funfoss"
                }
                """)]
    [InlineData("""
                {
                    "PicturePath": "",
                    "Name": "Heating Grid Test"
                }
                """)]
    [Theory]
    public void Deserialize_DeserializeHeatingGridString_GetsHeatingGrid(string testString)
    {
        //Arrange
        HeatingGrid expectedGrid = new("", "Heating Grid Test");
        HeatingGrid actualGrid;

        //Act
        actualGrid = JsonSerializerCustom.Deserialize<HeatingGrid>(testString);

        //Assert
        Assert.Equivalent(expectedGrid, actualGrid);
    }


    [Fact]
    public void Deserialize_DeserializeInt_GetsInt()
    {
        //Arrange
        const string testString = "1";
        int expectedValue = 1;
        int actualValue;

        //Act
        actualValue = JsonSerializerCustom.Deserialize<int>(testString);

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void Deserialize_DeserializeTextIntoInt_Error()
    {
        //Arrange
        const string testString = "Lorem Ipsum";

        //Act
        void _getTheValue()
        {
            JsonSerializerCustom.Deserialize<int>(testString);
        }

        //Assert
        Assert.Throws<JsonException>(_getTheValue);
    }

    [Fact]
    public void Deserialize_DeserializeAnonFunc_Error()
    {
        //Arrange
        const string anonFuncString = "(int x) => x * x";

        //Act
        void _getTheValue()
        {
            JsonSerializerCustom.Deserialize<Func<int, int>>(anonFuncString);
        }

        //Assert
        Assert.Throws<JsonException>(_getTheValue);
    }

    string FormatJsonString(string jsonString, int skipItems = 0)
    {
        return string.Join("",
            jsonString
                .Normalize()
                .Replace("{", "")
                .Replace("}", "")
                .ReplaceLineEndings("")
                .Split(",")
                .Skip(skipItems)
        ).Replace(" ", "");
    }

    // JSON INTO STRING
    // TODO: fix this method
    [Fact]
    public void Serialize_SerializeProductionUnit_Success()
    {
        //Arrange
        ProductionUnit unit = new("TU", "", "", 10, 250, 7, 1.1, 8.0);
        const string expectedUnitString = """
                                              {
                                                  "Name": "TU",
                                                  "FullName": "",
                                                  "PicturePath": "",
                                                  "MaxHeat": 10,
                                                  "ProductionCost": 250,
                                                  "MaxElectricity": 7,
                                                  "GasConsumption": 1.1,
                                                  "Co2Emission":8,
                                                  "PictureBase64Url": null,
                                                  "OperationPoint":0
                                              }
                                          """;
        string actualUnitString;

        Console.WriteLine(unit);
        //Act
        actualUnitString = JsonSerializerCustom.Serialize(unit);

        //Assert
        Assert.Equal(
            //                                                                    Skip the generated Id field
            FormatJsonString(expectedUnitString), FormatJsonString(actualUnitString, 1)
        );
    }

    [Fact]
    public void Serialize_SerializeHeatingGrid_Success()
    {
        //Arrange
        HeatingGrid grid = new("", "Heating Grid Test");
        const string expectedGridString = """
                                          {
                                              "PicturePath": "",
                                              "Name": "Heating Grid Test"
                                          }
                                          """;
        string actualGridString;

        //Act
        actualGridString = JsonSerializerCustom.Serialize(grid);

        //Assert
        Assert.Equal(
            //                                                                    Skip the generated Id field
            FormatJsonString(expectedGridString), FormatJsonString(actualGridString, 1)
        );
    }
}
