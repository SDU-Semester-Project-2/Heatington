using Heatington.Controllers;
using Heatington.Controllers.Enums;
using Heatington.Controllers.Interfaces;
using Xunit.Abstractions;

namespace Heatington.Tests.Controllers;

/// <summary>
/// Documentation in Documents/Heatington.Tests/Controllers/FileController.Tests.md
/// </summary>
public class FileControllerTests : UseTestDirectory
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FileControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ReadFileFromPath_ReadFile_ReadsCorrectContent()
    {
        try
        {
            //Arrange
            string TestFilePath = Path.Combine(TestsDirPath, "randomwasdf123.txt");
            string expectedContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque euismod";
            IReadWriteController mockFileController = new FileController(TestFilePath);

            //Act
            await File.WriteAllTextAsync(TestFilePath, expectedContent);

            //Assert
            string actualContent = await mockFileController.ReadData<string>();
            Assert.Equal(expectedContent, actualContent);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
        }
    }

    [Fact]
    public async Task WriteFileFromPath_WriteFile_WritesCorrectContent()
    {
        try
        {
            //Arrange
            string TestFilePath = Path.Combine(TestsDirPath, "testFile.txt");
            string expectedContent =
                "Lorem ipsum dolor\t sit amet\n, consectetur\r adipiscing elit\t. Quisque euismod";
            FileController fileController = new FileController(TestFilePath);

            //Act
            OperationStatus status = await fileController.WriteData(expectedContent);

            //Assert
            string actualContent = File.ReadAllText(fileController.FilePath);
            Assert.Equal(expectedContent, actualContent);
            Assert.Equal(OperationStatus.SUCCESS, status);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
        }
    }

    [Fact]
    public async Task WriteFileFromPath_WriteEmptyStringToFile_CreatesFile()
    {
        try
        {
            //Arrange
            string TestFilePath = Path.Combine(TestsDirPath, "testFile");
            string emptyContent = "";
            IReadWriteController fileController = new FileController(TestFilePath);

            //Act
            OperationStatus status = await fileController.WriteData(emptyContent);

            //Assert
            Assert.True(File.Exists(TestFilePath));
            Assert.Equal(OperationStatus.SUCCESS, status);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
        }
    }

    [Fact]
    public async Task WriteToFileFromPath_WriteToTheSameFileTwice_CreatesTwo()
    {
        try
        {
            //Arrange
            string TestFilePath = Path.Combine(TestsDirPath, "file1.txt");
            string fakeContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque euismod";
            IReadWriteController fileController1;
            IReadWriteController fileController2;

            //Act
            fileController1 = new FileController(TestFilePath);
            OperationStatus status1 = await fileController1.WriteData(fakeContent);

            fileController2 = new FileController(TestFilePath);
            OperationStatus status2 = await fileController2.WriteData(fakeContent);

            //Assert
            Assert.Equal(OperationStatus.SUCCESS, status1);
            Assert.Equal(OperationStatus.SUCCESS, status2);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
        }
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
        try
        {
            //Arrange
            IReadWriteController fileController = new FileController(wrongFileName);

            //Act
            Func<Task<string?>> readNotExistingData = async () => await fileController.ReadData<string>();

            //Assert
            Assert.ThrowsAsync<FileNotFoundException>(async () => await readNotExistingData());
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
        }
    }
}
