// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Contollers;

namespace Heatington.Tests.Controllers;

public class FileController_Tests : IDisposable
{
    private const string TestsDirectory = "tests";
    public string TestsDirPath = Path.Combine(Path.GetTempPath(), TestsDirectory);

    public FileController_Tests()
    {
        if (!Directory.Exists(TestsDirPath))
        {
            Directory.CreateDirectory(TestsDirPath);
        }
        else
        {
            ClearTestsDirectory();
        }
    }

    [Fact]
    public void ReadFileFromPath_ReadFile_ReadsCorrectContent()
    {
        string TestFilePath = Path.Combine(TestsDirPath, Path.GetRandomFileName());
        string content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque euismod";
        IReadWriteController fileController = new FileController(TestFilePath);

        File.WriteAllText(TestFilePath, content);

        Assert.Equal(content, fileController.ReadData());
    }

    [Fact]
    public void WriteFileFromPath_WriteFile_WritesCorrectContent()
    {
        string TestFilePath = Path.Combine(TestsDirPath, Path.GetRandomFileName());
        string content = "Test content with some \t formatting and \n stuff to see! if  everything works. Fine :)";

        IReadWriteController fileController = new FileController(TestFilePath);
        fileController.WriteData(content);

        Assert.Equal(content, File.ReadAllText(TestFilePath));
    }

    [Fact]
    public void WriteFileFromPath_WriteEmptyStringToFile_CreatesFile()
    {
        string TestFilePath = Path.Combine(TestsDirPath, Path.GetRandomFileName());
        string content = "";

        IReadWriteController fileController = new FileController(TestFilePath);
        fileController.WriteData(content);

        Assert.True(File.Exists(TestFilePath));
    }

    [Fact]
    public void WriteToFileFromPath_WriteToTheSameFileTwice_CreatesTwo()
    {
        string TestFilePath = Path.Combine(TestsDirPath, "file1.txt");
        string fakeContent = "asfsadf";
        IReadWriteController fileController1 = new FileController(TestFilePath);
        fileController1.WriteData(fakeContent);
        IReadWriteController fileController2 = new FileController(TestFilePath);
        fileController2.WriteData(fakeContent);

        foreach (var file in Directory.GetFiles(TestsDirPath))
        {
            Console.WriteLine(file + "\n");
        }

        Assert.Equal(2, Directory.GetParent(TestFilePath)!.GetFiles().Length);
    }

    [Theory]
    [InlineData("aaa1")]
    [InlineData("test.cvs")]
    [InlineData("csv.ctest")]
    [InlineData("./adsfsaf.json")]
    [InlineData("../../asets")]
    [InlineData(@"c:\78fe9lk")]
    [InlineData("/root12")]
    [InlineData("./Jsons")]
    [InlineData("./123")]
    public void ReadFileFromPath_ReadNotExistingFile_FileNotFound(string wrongFileName)
    {
        IReadWriteController fileController = new FileController(wrongFileName);

        Assert.Throws<FileNotFoundException>(() =>
        {
            fileController.ReadData();
        });
    }

    private void ClearTestsDirectory()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(TestsDirPath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    public void Dispose()
    {
        //Clear Tests Directory
        ClearTestsDirectory();
    }
}
