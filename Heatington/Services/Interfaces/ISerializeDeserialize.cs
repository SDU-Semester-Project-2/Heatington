namespace Heatington.Services.Interfaces;

public interface ISerializeDeserialize
{
    public static abstract string? Serialize<T>(T obj);
    public static abstract T? Deserialize<T>(string file);
}
