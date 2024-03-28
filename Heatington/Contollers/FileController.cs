namespace Heatington.Contollers;

/// <summary>
/// Class for performing read/write actions on local files
/// </summary>
public class FileController : IReadWriteController
{
    /// <summary>
    /// Path to a file
    /// </summary>
    private readonly string _path;


    /// <summary>
    /// Class constructor. Gets a path to the file to control.
    /// </summary>
    /// <param name="pathToFile"> Path to the location of a file.
    /// Follows the pattern <c>file.json</c>
    /// </param>
    public FileController(string pathToFile)
    {
        _path = pathToFile;
    }

    private static void DisplayException(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private T TryFileOperationRunner<T>(Func<T> funcToTry)
    {
        try
        {
            return funcToTry();
        }
        catch (FileNotFoundException e)
        {
            DisplayException($"File does not exist!\nPath: {_path}\n{e.FileName}");
            throw;
        }
        catch (DirectoryNotFoundException e)
        {
            DisplayException($"You passed a wrong path! Directory does not exist.\nPath: {_path}\n{e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown exception: {e}");
            throw;
        }
    }

    /// <summary>
    /// Function for reading the contents of a local file.
    /// Reads from the path passed during object initialization.
    /// </summary>
    /// <returns>Returns content of the file as a string</returns>
    /// <exception cref="FileNotFoundException">If file does not exist or path is not correct the exception is thrown</exception>
    private string? ReadFileFromPath()
    {
        string? fileContent = TryFileOperationRunner(() =>
        {
            return File.ReadAllText(_path);
        });

        return fileContent;
    }

    private void WriteToFileFromPath(string content)
    {
        TryFileOperationRunner(() =>
        {
            File.WriteAllText(_path, content);
            return 0;
        });
    }

    /// <summary>
    /// Function for reading the data of a local file.
    /// Reads from the path passed during object initialization.
    /// </summary>
    /// <returns>Returns data of the file as a string</returns>
    public string? ReadData()
    {
        return ReadFileFromPath();
    }

    /// <summary>
    /// Function for writing data into a local file
    /// </summary>
    /// <param name="content">Content to be written to a file as a string</param>
    public void WriteData(string content)
    {
        WriteToFileFromPath(content);
    }
}
