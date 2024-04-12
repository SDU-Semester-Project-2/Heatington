using Heatington.Models;

namespace Heatington.Controllers.Interfaces
{
    public interface IDataSource
    {
        Task<List<DataPoint>?> GetDataAsync();
        void SaveData(List<DataPoint> data);
    }
}
