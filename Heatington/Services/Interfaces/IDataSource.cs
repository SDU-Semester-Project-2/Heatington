using Heatington.Models;

namespace Heatington.Services.Interfaces
{

    public interface IDataSource
    {
        Task<List<DataPoint>?> GetDataAsync();
        void SaveData(List<DataPoint> data);
    }
}
