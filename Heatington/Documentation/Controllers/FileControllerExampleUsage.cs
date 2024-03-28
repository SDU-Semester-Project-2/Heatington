// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Contollers;

namespace Heatington.Documentation.Controllers;

unsafe class FileControllerExampleUsage
{
    FileControllerExampleUsage()
    {
        string DefaultAssetsDirectory = "Assets/";

        string? pathToParentDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
        string? path;
        string fileName = "testFile.json";

        if (pathToParentDirectory != null)
        {
            path = Path.Combine(pathToParentDirectory, DefaultAssetsDirectory + fileName);
        }
        else
        {
            throw new DirectoryNotFoundException($"The directory {pathToParentDirectory} does not exist.\n" +
                                                 $"This is an error caused by FileController, something went wrong when getting the parent directory");
        }

        IReadWriteController fileController = new FileController(path);
        string? data = fileController.ReadData();

        Console.WriteLine(data);
    }
}
