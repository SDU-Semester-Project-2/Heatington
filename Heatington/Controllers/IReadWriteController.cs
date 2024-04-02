namespace Heatington.Contollers;

/// <summary>
/// Generic Interface for all controllers performing I/O operations
/// </summary>
public interface IReadWriteController
{
    /// <summary>
    /// Function for reading data out of a model
    /// </summary>
    /// <returns>
    /// Data as a string
    /// </returns>
    public string? ReadData();

    /// <summary>
    /// Function for Writing data into the model
    /// </summary>
    /// <param name="content">
    /// content to write into the model, as a string
    /// </param>
    public void WriteData(string content);
}
