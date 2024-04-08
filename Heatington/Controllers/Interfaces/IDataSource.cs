using Heatington.Models;

namespace Heatington.Data
{

    public interface IDataSource
    {

        Task<List<DataPoint>?> GetDataAsync(string filePath);
        void SaveData(List<DataPoint> data, string filePath);
    }
}
