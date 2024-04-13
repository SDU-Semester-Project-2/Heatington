using Heatington;
using Heatington.Controllers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager;

public class ResultDataManager
{
    private CsvController _fileController;
    private List<ResultHolder> _optResults;


    public ResultDataManager(string filePath)
    {
        _fileController = new CsvController(filePath);
    }
    public void FetchOptimizationData(Opt opt)
    {
        _optResults = opt.Results;
    }

    public void WriteDataToCsv()
    {
        _fileController.SaveData(_optResults);
    }
}
