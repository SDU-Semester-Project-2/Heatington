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
                list.Add(result.StartTime.ToString());
                list.Add(result.EndTime.ToString());
                list.Add(result.HeatDemand.ToString());
                list.Add(result.ElectricityPrice.ToString());
                list.Add(result.NetProductionCost.ToString());
                foreach (ProductionUnit boiler in result.Boilers)
                {
                    list.Add(boiler.Name.ToString());
                }

                _data.Add(list.ToArray());
            }

            //Console.WriteLine(_data.Count());
        }

        public void WriteDataToCsv()
        {
            CsvData csvData = new CsvData(_data);

            string serializedString = CsvController.Serialize(csvData);


        }
    }
}
