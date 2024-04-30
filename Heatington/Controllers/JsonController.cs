using System.Text.Json;
using System.Text.Json.Serialization;
using Heatington.Controllers.Enums;
using Heatington.Controllers.Interfaces;
using Heatington.Models;
using Heatington.Services.Serializers;

namespace Heatington.Controllers;

public class JsonController(string filePath) : IReadWriteController
{
    private readonly FileController _fileController = new FileController(filePath);
    public string JsonPath => _fileController.FilePath;

    public async Task<T> ReadData<T>()
    {
        string data = await _fileController.ReadData<string>();
        T result = JsonSerializerCustom.Deserialize<T>(data);

        return result;
    }

    public async Task<OperationStatus> WriteData<T>(T content)
    {
        string serializedData = JsonSerializerCustom.Serialize(content);
        return await _fileController.WriteData(serializedData);
    }

    public override string ToString()
    {
        return $"Path to file {filePath}";
    }
}

// WORK IN PROGRESS
public class ProductionUnitJsonConverter : JsonConverter<ProductionUnit>
{
    public override ProductionUnit Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => new ProductionUnit("", "", "", 1, 1, 1, 1, 1);

    public override void Write(
        Utf8JsonWriter writer,
        ProductionUnit productionUnit,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(productionUnit.ToString());
}

class AssetManagerJsonConverterFactory : JsonConverterFactory
{
    //TODO: go back and create a factory or overkill?
    //https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-8-0#sample-basic-converter

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.GetGenericTypeDefinition() == typeof(List<ProductionUnit>) &&
               typeToConvert.GetGenericTypeDefinition() == typeof(HeatingGrid);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotImplementedException();
}
