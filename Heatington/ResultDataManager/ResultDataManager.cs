using Heatington.Contollers;

namespace Heatington.ResultDataManager
{

    public class ResultDataManager
    {
        private readonly FileController _fileController;

        public ResultDataManager(string filePath)
        {
            _fileController = new FileController(filePath);
        }

        public void WriteOptimizationResultToCSV()
        {
            _fileController.WriteData("");
        }
    }
}
