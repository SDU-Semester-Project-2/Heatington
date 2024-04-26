using Heatington;
using Heatington.Controllers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager;

public class ResultDataManager(CsvController _csvController)
{
    private List<ResultHolder> _optResults;

    public ResultDataManager(string filePath)
    {
        _csvController = new CsvController(filePath);
    }

    public void FetchOptimizationData(Opt opt)
    {
        _optResults = opt.Results;
    }

    private List<ProductionUnit> GetProductionUnits()
    {
        AssetManager.AssetManager assetManager = new();

        Task loadAssets = assetManager.LoadAssets();
        loadAssets.Wait();
        List<ProductionUnit> pUnits = assetManager.ProductionUnits!.Values.ToList();

        return pUnits;
    }

    public void WriteDataToCsv()
    {
        _csvController.SaveData(_optResults);
    }
}
