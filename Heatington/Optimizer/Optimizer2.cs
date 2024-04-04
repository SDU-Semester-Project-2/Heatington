using Heatington.Data;
using Heatington.Models;
using Heatington.SourceDataManager;

namespace Heatington.Optimizer;

public class Optimizer2
{
    private List<DataPoint>? _dataPoints = new List<DataPoint>();





    public void GetDataPoints()
    {
        IDataSource dataSource = new CsvDataSource();

        SourceDataManager.SourceDataManager sourceDataManager = new(dataSource, "../../../../Assets/winter_period.csv");

        Task fetchTimeSeriesDataAsync = sourceDataManager.FetchTimeSeriesDataAsync();

        fetchTimeSeriesDataAsync.Wait();

        //sourceDataManager.LogTimeSeriesData();

        _dataPoints = sourceDataManager.TimeSeriesData;
    }
}
