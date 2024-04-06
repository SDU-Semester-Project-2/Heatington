using Heatington.Controllers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager
{
    public class ResultDataManager
    {
        private readonly string _filePath;
        private readonly List<string[]> _data = new List<string[]>();
        private List<ResultHolder> _optResults;

        public ResultDataManager(string filePath)
        {
            _filePath = filePath;
        }

        public void FetchOptimizationData(Opt opt)
        {
            _optResults = opt.Results;
        }

        public void ConvertOptResultsToData()
        {
            foreach (ResultHolder result in _optResults)
            {
                List<string> list = new List<string>();
                list.Append(result.StartTime.ToString());
                list.Append(result.EndTime.ToString());
                list.Append(result.HeatDemand.ToString());
                list.Append(result.ElectricityPrice.ToString());
                list.Append(result.NetProductionCost.ToString());
                foreach (ProductionUnit boiler in result.Boilers)
                {
                    list.Append(boiler.ToString());
                }

                _data.Append(list.ToArray());
            }

            //Console.WriteLine(_data.Count());
        }

        public void WriteDataToCsv()
        {
            CsvData csvData = new CsvData(_data);

            String serializedString = CsvController.Serialize(csvData);


        }
    }
}
