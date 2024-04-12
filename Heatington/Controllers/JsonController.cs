using System.Text.Json;
using System.Text.Json.Serialization;
using Heatington.Controllers.Enums;
using Heatington.Controllers.Interfaces;
using Heatington.Services.Interfaces;
using Heatington.Helpers;
using Heatington.Models;

namespace Heatington.Controllers;

public class JsonController(string filePath) : ISerializeDeserialize, IReadWriteController
{
    //TODO: for now both JsonController and JsonDataSource implement IReadWriteController. Remove one, after talking with team.
    private readonly IReadWriteController _fileController = new FileController(filePath);

    public async Task<T> ReadData<T>()
    {
        string data = await _fileController.ReadData<string>();
        T result = JsonController.Deserialize<T>(data);

        return result;
    }

    public async Task<OperationStatus> WriteData<T>(T content)
    {
        string serializedData = JsonController.Serialize(content);
        return await _fileController.WriteData(serializedData);
    }

    public override string ToString()
    {
        return $"Path to file {filePath}";
    }

    public static string Serialize<T>(T obj)
    {
        var serializeOptions = new JsonSerializerOptions { WriteIndented = true };

        // TODO: use async serialzier?
        // Then you have to pass the StreamWriter to the jsonSerializer
        // serialzeOptions.Converters.Add(new ProductionJsonConverter());

        try
        {
            // byte[] jsonUtf8Bytes =JsonSerializer.SerializeToUtf8Bytes(obj); // --> it's 10% faster
            // string sdd = JsonSerializer.SerializeAsync(obj, serializeOptions);
            return JsonSerializer.Serialize(obj, serializeOptions);
        }
        catch (Exception e)
        {
            Utilities.DisplayException($"Unknown exception occured: {e.Message}");
            throw;
        }
    }

    public static T Deserialize<T>(string file)
    {
        var deserializeOptions = new JsonSerializerOptions();

        // TODO: use async deserialzier?
        // deserializeOptions.Converters.Add(new ProductionUnitJsonConverter());

        try
        {
            // JsonSerializer.DeserializeAsync<T>(file, deserializeOptions);
            T? result = JsonSerializer.Deserialize<T>(file, deserializeOptions);

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new JsonException("Couldn't deserialize the object");
            }
        }
        catch (Exception e)
        {
            Utilities.DisplayException($"Unknown exception occured: {e.Message}");
            throw;
        }
    }
}

// WORK IN PROGRESS
public class ProductionUnitJsonConverter : JsonConverter<ProductionUnit>
{
    public override ProductionUnit Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => new ProductionUnit("", "", 1, 1, 1, 1, 1);

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
