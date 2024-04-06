using Heatington.Controllers;
using Heatington.Models;
using Heatington.Optimizer;

namespace Heatington.ResultDataManager
{

    public class ResultDataManager
    {
        private readonly string _filePath;
        private readonly List<string> _data = new List<string>();
        private List<ResultHolder> _optResults;

        public ResultDataManager(string filePath)
        {
            _filePath = filePath;

        }

        public void FetchOptimizationData(Opt opt)
        {
            _optResults = opt.Results;
        }

        public void WriteDataToCsv()
        {
            CsvData csvData = new CsvData(_data);

            String serializedString = CsvController.Serialize(_csvData);


        }
    }
}
