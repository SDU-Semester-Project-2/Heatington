namespace Heatington.Controllers;

/// <summary>
/// Generic Interface for all controllers performing I/O operations
/// </summary>
public interface IReadWriteController
{
    /// <summary>
    /// Function for reading data out of a model
    /// </summary>
    /// <returns>
    /// task to wait for Data
    /// </returns>
    public Task<T?> ReadData<T>();

    /// <summary>
    /// Function for Writing data into the model
    /// </summary>
    /// <param name="content">
    /// content to write into the model, generic type
    /// </param>
    /// <returns>
    /// Task to wait for status of the operation
    /// </returns>
    public Task<OperationStatus> WriteData<T>(T content);
}
