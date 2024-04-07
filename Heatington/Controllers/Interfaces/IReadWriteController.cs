using Heatington.Controllers.Enums;

namespace Heatington.Controllers.Interfaces;

/// <summary>
/// Documentation in Documents/Heatington/Controllers/Interfaces/IReadWriteController.md
/// </summary>
public interface IReadWriteController
{
    public Task<T> ReadData<T>();
    public Task<OperationStatus> WriteData<T>(T content);
    public abstract string? ToString();
}
