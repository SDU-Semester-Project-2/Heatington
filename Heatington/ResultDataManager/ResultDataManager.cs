using Heatington.Controllers;

namespace Heatington.ResultDataManager
{

    public class ResultDataManager
    {
        private readonly string _filePath;
        private readonly List<string> _data = new List<string>();

        public ResultDataManager(string filePath)
        {
            _filePath = filePath;
        }

        public void WriteDataToCsv()
        {
            CsvData csvData = new CsvData(_data);

            String serializedString = CsvController.Serialize(_csvData);


        }
    }
}
