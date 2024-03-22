using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Heatington.Optimizer;

public class FrancescoCsvController(string pathToFile)
{
    public List<FrancescoEnergyData> ReadTimeSeriesEnergyData()
    {
        IEnumerable<FrancescoEnergyData> timeSeries;

        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine,
            HasHeaderRecord = false
        };

        using StreamReader streamReader = new StreamReader(pathToFile);
        using CsvReader csvReader = new CsvReader(streamReader, config);

        try
        {
            timeSeries = csvReader.GetRecords<FrancescoEnergyData>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        List<FrancescoEnergyData> timeDataList = timeSeries.ToList();
        return timeDataList;
    }
}

