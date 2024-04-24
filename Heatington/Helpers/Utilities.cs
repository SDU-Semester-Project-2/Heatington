// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Controllers;
using Heatington.Controllers.Interfaces;

namespace Heatington.Helpers;

public static class Utilities
{
    public static void DisplayException(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static T ConvertObject<T>(object? obj)
    {
        return (T)Convert.ChangeType(obj, typeof(T))!;
    }

    // public static IReadWriteController[] GenerateJsonControllers(params string[] paths)
    // {
    //     IReadWriteController[] jsonControllers = [];
    //
    //     foreach (string path in paths)
    //     {
    //         jsonControllers.Append(new JsonController(path));
    //     }
    //
    //     return jsonControllers;
    // }

    public static string GetAbsolutePathToAssetsDirectory()
    {
        const string defaultAssetsDirectory = "Assets/";

        DirectoryInfo? pathToProjectRootDirectory =
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent;

        if (pathToProjectRootDirectory?.FullName == null)
        {
            Utilities.DisplayException($"Utility error! The directory {pathToProjectRootDirectory} does not exist.\n");
            throw new DirectoryNotFoundException();
        }

        // Console.WriteLine(Path.Combine(pathToProjectRootDirectory.FullName, defaultAssetsDirectory));

        return Path.Combine(pathToProjectRootDirectory.FullName, defaultAssetsDirectory);
    }

    public static string GeneratePathToFileInAssetsDirectory(string fileName)
    {
        return Path.Combine(Utilities.GetAbsolutePathToAssetsDirectory(), fileName);
    }

    public static new string ToString()
    {
        return "Class containing method helping with certain tasks";
    }
}
