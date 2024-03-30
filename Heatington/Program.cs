using Heatington.Data;
using Heatington.Models;
using System;
using System.Globalization;

namespace Heatington
{
    class Program
    {
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
