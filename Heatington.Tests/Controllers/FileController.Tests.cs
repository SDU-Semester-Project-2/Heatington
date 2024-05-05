using Heatington.Controllers;
using Heatington.Controllers.Interfaces;

namespace Heatington.Tests.Controllers;

/// <summary>
/// Documentation in Documents/Heatington.Tests/Controllers/FileController.Tests.md
/// </summary>
public class FileControllerTests : UseTestDirectory
{

    [Fact]
    public async void ReadFileFromPath_ReadFile_ReadsCorrectContent()
    {
        //Arrange
        string TestFilePath = Path.Combine(TestsDirPath, Path.GetRandomFileName());
        string expectedContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque euismod";
        IReadWriteController mockFileController = new FileController(TestFilePath);

        //Act
        await File.WriteAllTextAsync(TestFilePath, expectedContent);

        //Assert
        string actualContent = await mockFileController.ReadData<string>();
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]
    public void WriteFileFromPath_WriteFile_WritesCorrectContent()
    {
        //Arrange
        string TestFilePath = Path.Combine(TestsDirPath, Path.GetRandomFileName());
        string expectedContent =
            "Lorem ipsum dolor\t sit amet\n, consectetur\r adipiscing elit\t. Quisque euismod";
        IReadWriteController fileController = new FileController(TestFilePath);

        //Act
        fileController.WriteData(expectedContent);

        //Assert
        string actualContent = File.ReadAllText(TestFilePath);
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]
    public void WriteFileFromPath_WriteEmptyStringToFile_CreatesFile()
    {
        //Arrange
        string TestFilePath = Path.Combine(TestsDirPath, Path.GetRandomFileName());
        string emptyContent = "";
        IReadWriteController fileController = new FileController(TestFilePath);

        //Act
        fileController.WriteData(emptyContent);

        //Assert
        Assert.True(File.Exists(TestFilePath));
    }

    [Fact]
    public void WriteToFileFromPath_WriteToTheSameFileTwice_CreatesTwo()
    {
        //Arrange
        string TestFilePath = Path.Combine(TestsDirPath, "file1.txt");
        string fakeContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque euismod";
        IReadWriteController fileController1;
        IReadWriteController fileController2;

        //Act
        fileController1 = new FileController(TestFilePath);
        fileController1.WriteData(fakeContent);

        fileController2 = new FileController(TestFilePath);
        fileController2.WriteData(fakeContent);

        //Assert
        int expectedNumOfFiles = 2;
        int actualNumOfFiles = Directory.GetParent(TestFilePath)!.GetFiles().Length;
        Assert.Equal(expectedNumOfFiles, actualNumOfFiles);
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
        //Arrange
        IReadWriteController fileController = new FileController(wrongFileName);

        //Act
        Func<Task<string?>> readNotExistingData = async () => await fileController.ReadData<string>();

        //Assert
        Assert.ThrowsAsync<FileNotFoundException>(async () => await readNotExistingData());
    }
}
