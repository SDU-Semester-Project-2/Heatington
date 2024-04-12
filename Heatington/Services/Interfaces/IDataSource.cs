using Heatington.Models;

namespace Heatington.Services.Interfaces
{
    public interface IDataSource
    {
        Task<List<DataPoint>?> GetDataAsync(string filePath);
        void SaveData(List<DataPoint> data, string filePath);
    }
}
