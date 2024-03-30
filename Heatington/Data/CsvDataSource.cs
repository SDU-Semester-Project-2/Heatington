using System.Globalization;
using Heatington.Models;

namespace Heatington.Data
{
    public class CsvDataSource : IDataSource
    {
        public List<DataPoint>? GetData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            CultureInfo provider = CultureInfo.InvariantCulture;
            const string format = "M/d/yy H:mm";

            return lines.Select(line => line.Split(','))
                .Select(parts => new DataPoint(
                    startTime: DateTime.ParseExact(parts[0], format, provider),
                    endTime: DateTime.ParseExact(parts[1], format, provider),
                    heatDemand: double.Parse(parts[2]),
                    electricityPrice: double.Parse(parts[3])
                )).ToList();
        }

        public void SaveData(List<DataPoint> data, string filePath) => throw new NotImplementedException();
    }
}
