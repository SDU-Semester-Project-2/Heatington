using Heatington.Controllers;
using Heatington.Contollers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager
{
    public class ResultDataManager
    {
        private FileController _fileController;

        private readonly List<string[]> _data = new List<string[]>();
        private List<ResultHolder> _optResults;

        public ResultDataManager(string filePath)
        {
            _fileController = new FileController(filePath);
        }

        public void FetchOptimizationData(Opt opt)
        {
            _optResults = opt.Results;
        }

        public string FormatDateTime(DateTime time)
        {
            TimeZoneInfo danishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            DateTime startTimeInDanish = TimeZoneInfo.ConvertTimeFromUtc(time, danishTimeZone);
            return startTimeInDanish.ToString("dd.MM.yyyy HH:mm");
        }

        public void ConvertOptResultsToData()
        {
            foreach (ResultHolder result in _optResults)
            {
                List<string> list = new List<string>();

                list.Add(FormatDateTime(result.StartTime));
                list.Add(FormatDateTime(result.EndTime));
                list.Add(result.HeatDemand.ToString());
                list.Add(result.ElectricityPrice.ToString());
                list.Add(result.NetProductionCost.ToString());
                foreach (ProductionUnit boiler in result.Boilers)
                {
                    list.Add(boiler.Name.ToString());
                }
                _data.Add(list.ToArray());
            }
        }

        public void WriteDataToCsv()
        {
            CsvData csvData = new CsvData(_data);
            string serializedString = CsvController.Serialize(csvData);
            _fileController.WriteData(serializedString);
        }
    }
}
