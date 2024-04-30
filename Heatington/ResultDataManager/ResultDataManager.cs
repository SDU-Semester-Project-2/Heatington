using System.Buffers;
using Heatington;
using Heatington.Controllers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager;

public class ResultDataManager(CsvController _csvController)
{
    private List<ResultHolder> _optResults;
    private List<FormatedResultHolder> _formatedResult;

    public ResultDataManager(string filePath)
    {
        _csvController = new CsvController(filePath);
    }

    public void FetchOptimizationData(Opt opt)
    {
        _optResults = opt.Results;
    }

    private List<FormatedResultHolder> FormatResults(List<ResultHolder> rawResults)
    {
        List<FormatedResultHolder> formatedResults = new();

        // I am very sorry for the O(n^2)
        foreach (var entry in _optResults)
        {
            foreach (var unit in entry.Boilers)
            {
                formatedResults.Add(new FormatedResultHolder(
                    entry.StartTime, entry.EndTime, entry.HeatDemand, entry.ElectricityPrice, unit,
                    entry.NetProductionCost));
            }
        }

        return formatedResults;
    }

    public void WriteDataToCsv()
    {
        if (_optResults == null)
        {
            return;
        }

        List<FormatedResultHolder> resultsToWrite = FormatResults(_optResults);

        _csvController.SaveData(resultsToWrite);
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
