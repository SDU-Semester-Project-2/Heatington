using System.Text.Json;
using Heatington.Helpers;
using Heatington.Services.Interfaces;

namespace Heatington.Services.Serializers;

public abstract class JsonSerializerCustom : ISerializeDeserialize
{
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

    public static T Deserialize<T>(string content)
    {
        var deserializeOptions = new JsonSerializerOptions();

        // TODO: use async deserialzier?
        // deserializeOptions.Converters.Add(new ProductionUnitJsonConverter());

        try
        {
            // JsonSerializer.DeserializeAsync<T>(file, deserializeOptions);
            T? result = JsonSerializer.Deserialize<T>(content, deserializeOptions);

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
