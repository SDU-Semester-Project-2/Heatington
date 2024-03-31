using Heatington.Data;

namespace Heatington
{
    internal static class Program
    {
        // This is only for the purpose of testing the implementation
        // TODO: Rewrite this method and implement the actual application logic
        static void Main(string[] args)
        {
            const string filePath = "../Assets/winter_period.csv";

            // Create a new CsvDataSource
            IDataSource dataSource = new CsvDataSource();

            SourceDataManager sdm = new(dataSource, filePath);

            // Fetch data from dataSource
            sdm.FetchTimeSeriesData();

            // Log the loaded data to the console
            sdm.LogTimeSeriesData();

            Console.WriteLine("Data loaded successfully.");
        }
    }
}
