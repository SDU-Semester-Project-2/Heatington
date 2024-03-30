using Heatington.Models;

namespace Heatington.Data
{
    public interface IDataSource
    {
        List<DataPoint>? GetData(string filePath);
        void SaveData(List<DataPoint> data, string filePath);
    }
}
