using System.Buffers;
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

    public void WriteDataToCsv()
    {
        _csvController.SaveData(_optResults);
    }

    public void FormatResults()
    {
        List<ResultModel> formatedResults = new();

        foreach (var result in _optResults)
        {
            foreach (var unit in result.Boilers)
            {
                formatedResults.Add(new ResultModel(unit, result.StartTime, result.EndTime, unit.OperationPoint));
            }
        }

        formatedResults.ForEach(Console.WriteLine);
    }

    private List<ProductionUnit> GetProductionUnits()
    {
        AssetManager.AssetManager assetManager = new();

        Task loadAssets = assetManager.LoadAssets();
        loadAssets.Wait();
        List<ProductionUnit> pUnits = assetManager.ProductionUnits!.Values.ToList();

        return pUnits;
    }
}

public class ResultModel(ProductionUnit boiler, DateTime startTime, DateTime endTime, double operationPoint)
{
    public ProductionUnit Boiler { get; set; } = boiler;
    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;
    public double OperationPoint { get; set; } = operationPoint;

    public override string ToString()
    {
        string s = string.Concat($"{Boiler.Name} ", StartTime, EndTime, $" {OperationPoint}");
        return s;
    }
}
