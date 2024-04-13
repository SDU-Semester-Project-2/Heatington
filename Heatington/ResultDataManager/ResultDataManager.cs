using Heatington;
using Heatington.Controllers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager;

public class ResultDataManager
{
    private CsvController _csvController;
    private List<ResultHolder> _optResults;


    public ResultDataManager(string filePath)
    {
        _csvController = new CsvController(filePath);
    }
    public void FetchOptimizationData(Opt opt)
    {
        _optResults = opt.Results;
    }

    public void WriteDataToCsv()
    {
        _csvController.SaveData(_optResults);
    }
}
