using System.Text;
using Heatington.Controllers.Enums;
using Heatington.Controllers.Interfaces;
using Heatington.Helpers;

namespace Heatington.Controllers;

/// <summary>
/// Documentation in Documents/Heatington/Controllers/FileController.md
/// </summary>
public class FileController(string pathToFile) : IReadWriteController
{
    public string FilePath => pathToFile;

    private static string FormatFileName(string filePath)
    {
        return Path.Combine(
            Path.GetDirectoryName(filePath) ?? Path.GetTempPath(),
            Path.GetFileNameWithoutExtension(filePath) + "-" +
            DateTime.Now.ToString("yyyyMMddTHHmmss") + "-" +
            Random.Shared.Next().ToString() +
            Path.GetExtension(filePath)
        );
    }

    private async Task<T> TryFileOperationAsync<T>(Func<Task<T>> funcToTry)
    {
        try
        {
            return await funcToTry();
        }
        catch (FileNotFoundException e)
        {
            Utilities.DisplayException($"File does not exist!\nPath: {pathToFile}\n{e.FileName}");
            throw;
        }
        catch (DirectoryNotFoundException e)
        {
            Utilities.DisplayException(
                $"You passed a wrong path! Directory does not exist.\nPath: {pathToFile}\n{e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Utilities.DisplayException($"Unknown exception: {e}");
            throw;
        }
    }

    private async Task<string?> ReadFileFromPath()
    {
        string fileContent = await TryFileOperationAsync(async () =>
        {
            if (!File.Exists(pathToFile)) // TODO: Explicit check if file exists = more readable?
            {
                throw new FileNotFoundException();
            }

            return await File.ReadAllTextAsync(pathToFile);
        });

        return fileContent;
    }

    private async Task<OperationStatus> WriteToFileFromPath(string content)
    {
        ArgumentNullException.ThrowIfNull(content); // if content == null throw exception

        OperationStatus status = await TryFileOperationAsync(async () =>
        {
            await File.WriteAllTextAsync(pathToFile, content, Encoding.UTF8);

            if (!File.Exists(pathToFile) || new FileInfo(pathToFile).Length == 0)
            {
                return OperationStatus.FAILURE;
            }

            return OperationStatus.SUCCESS;
        });

        return status;
    }

    // make it generic --> actual endpoints for others
    public async Task<T> ReadData<T>()
    {
        string? fileData = await ReadFileFromPath();

        return Utilities.ConvertObject<T>(fileData);
    }

    public async Task<OperationStatus> WriteData<T>(T content)
    {
        pathToFile =
            File.Exists(pathToFile) ? FormatFileName(pathToFile) : pathToFile; //if file exists write to a new one
        string contentAsString = Utilities.ConvertObject<string>(content); //convert to string

        return await WriteToFileFromPath(contentAsString);
    }

    public override string ToString()
    {
        return $"Controlling file: ${pathToFile}";
    }
}
