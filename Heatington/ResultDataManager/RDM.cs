using System.Buffers;
using Heatington;
using Heatington.Controllers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager;

public class RDM(CsvController _csvController)
{
    private List<ResultHolder> _optResults;
    public List<FormatedResultHolder> FormatedResults;

    public void FetchOptimizationData(OPT opt)
    {
        _optResults = opt.Results;
    }

    public void FetchOptimizationData(List<ResultHolder> rawResults)
    {
        _optResults = rawResults;
    }

    public List<FormatedResultHolder> FormatResults(List<ResultHolder> rawResults)
    {
        List<FormatedResultHolder> formatedResults = new();

        // I am very sorry for the O(n^2)
        foreach (var entry in rawResults)
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

    public List<FormatedResultHolder> FormatResults()
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
}
