namespace Heatington.Tests;

public abstract class UseTestDirectory : IDisposable
{
    private const string TestsDirectory = "tests";
    protected readonly string TestsDirPath = Path.Combine(Path.GetTempPath(), TestsDirectory);

    public UseTestDirectory() // NOT A TEST
    {
        // create temporary test folder
        if (!Directory.Exists(TestsDirPath))
        {
            Directory.CreateDirectory(TestsDirPath);
        }
        // else
        // {
        //     ClearTestsDirectory();
        // }
    }

    public void ClearTestsDirectory() // NOT A TEST
    {
        // clear and remove temporary test folder
        DirectoryInfo di = new DirectoryInfo(TestsDirPath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    void IDisposable.Dispose()
    {
        //Clear Tests Directory
        ClearTestsDirectory();
    }
}
