using Heatington.Helpers;

namespace Heatington.Controllers.Interfaces;

public interface ISerializeDeserialize
{
    public static abstract string? Serialize<T>(T obj);
    public static abstract T? Deserialize<T>(string file);
}
