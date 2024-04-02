namespace Heatington.Contollers;

using Heatington.Documentation.Controllers;
using Heatington.Helpers;

//TODO: add global path for imports?

/// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="FileControllerClass"]/*' />
/// <example> <see cref="FileControllerExampleUsage"/> </example>
public class FileController : IReadWriteController
{
    /// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="_path"]/*' />
    private readonly string _path;

    /// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="FileController"]/*' />
    public FileController(string pathToFile)
    {
        _path = pathToFile;
    }

    /// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="TryFileOperationRunner"]/*' />
    private T TryFileOperationRunner<T>(Func<T> funcToTry)
    {
        try
        {
            return funcToTry();
        }
        catch (FileNotFoundException e)
        {
            Utilities.DisplayException($"File does not exist!\nPath: {_path}\n{e.FileName}");
            throw;
        }
        catch (DirectoryNotFoundException e)
        {
            Utilities.DisplayException(
                $"You passed a wrong path! Directory does not exist.\nPath: {_path}\n{e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Utilities.DisplayException($"Unknown exception: {e}");
            throw;
        }
    }

    /// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="ReadFileFromPath"]/*' />
    private string? ReadFileFromPath()
    {
        string? fileContent = TryFileOperationRunner(() =>
        {
            return File.ReadAllText(_path);
        });

        return fileContent;
    }

    /// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="WriteToFileFromPath"]/*' />
    private void WriteToFileFromPath(string content)
    {
        TryFileOperationRunner(() =>
        {
            File.WriteAllText(_path, content);
            return 0;
        });
    }

    /// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="ReadData"]/*' />
    public string? ReadData()
    {
        return ReadFileFromPath();
    }

    /// <include file='../Documentation/Controllers/FileControllerDocs.xml' path='/doc/members[@name="WriteData"]/*' />
    public void WriteData(string content)
    {
        WriteToFileFromPath(content);
    }
}
