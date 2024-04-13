using Heatington.Models;

namespace Heatington.Controllers.Interfaces
{

    public interface IDataSource
    {

        Task<List<DataPoint>?> GetDataAsync(string filePath);
        void SaveData(List<DataPoint> data, string filePath);
    }
}
