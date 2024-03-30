using Heatington.Data;

namespace Heatington
{
    class Program
    {
        // This is only for the purpose of testing the implementation
        // TODO: Remove this method and implement the actual application logic
        static void Main(string[] args)
        {
            const string filePath = "../Assets/winter_period.csv";

            // Create a new CsvDataSource
            IDataSource dataSource = new CsvDataSource();

            SourceDataManager SDM = new SourceDataManager(dataSource, filePath);

            // Fetch data from dataSource
            SDM.FetchTimeSeriesData();

            // Log the loaded data to the console
            SDM.LogTimeSeriesData();

            Console.WriteLine("Data loaded successfully.");
        }
    }
}
