// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace FrancescoDummyClasses;

public class ProductionUnitFs(
    string name,
    double operationPoint,
    double maxHeat,
    double productionCost,
    double maxElectricity,
    double gasConsumption,
    double co2Emissions)
{
    public Guid Id = new Guid();
    public string Name { get; set; } = name;
    public double OperationPoint { get; set; } = operationPoint;
    public double MaxHeat { get; set; } = maxHeat;
    public double ProductionCost { get; set; } = productionCost;
    public double MaxElectricity { get; set; } = maxElectricity;
    public double GasConsumption { get; set; } = gasConsumption;
    public double Co2Emissions { get; set; } = co2Emissions;

    public override string ToString()
    {
        string s = string.Concat("Name: ", Name, " Max heat: ", MaxHeat, " Production cost: ", productionCost,
            " Max electricity: ", MaxElectricity, " Gas consumption: ", gasConsumption, " Co2 emissions: ",
            co2Emissions);
        return s;
    }
}

public class FrancescoEnergyData
{
    [Index(0)] public DateTime StartTime { get; set; }
    [Index(1)] public DateTime EndTime { get; set; }
    [Index(2)] public double HeatDemandMwh { get; set; }
    [Index(3)] public double ElectricityPrice { get; set; }

    public override string ToString()
    {
        string s = string.Concat(StartTime, " ", EndTime, " ", HeatDemandMwh, " ", ElectricityPrice);
        return s;
    }
}

public class FrancescoCsvController(string pathToFile)
{
    public List<FrancescoEnergyData> ReadTimeSeriesEnergyData()
    {
        IEnumerable<FrancescoEnergyData> timeSeries;

        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine, HasHeaderRecord = false
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
