using System.Globalization;
using Heatington.AssetManager;
using Heatington.Models;
using Spectre.Console;
using static System.Console;

namespace Heatington;

public class ConsoleUI(AM am, SourceDataManager.SDM sdm, Optimizer.OPT opt)
{
    private List<DataPoint> _dataPoints = new List<DataPoint>();
    private List<ProductionUnit> _productionUnits = new List<ProductionUnit>();
    private List<ResultHolder> _results = new List<ResultHolder>();

    public void StartUi()
    {
        UiLoop();
    }

    private void UiLoop()
    {
        bool run = true;

        while (run)
        {
            Clear();
            AnsiConsole.Write(new FigletText("HEATINGTON")
                .Centered()
                .Color(Color.Red));

            string[] options = new[] { "Load Data", "Production Units", "Time Series Data", "Results", "quit" };

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select Action")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(options));

            int choice = GetSelectionFromText(selection);

            run = LoadUi(choice);

            int GetSelectionFromText(string sel)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    if (sel == options[i])
                    {
                        return i + 1;
                    }
                }

                return 0;
            }
        }
    }

    private bool LoadUi(int i)
    {
        switch (i)
        {
            case 1:
                LoadData();
                Read();
                return true;
            case 2:
                DisplayProductionUnits();
                Read();
                return true;
            case 3:
                DisplayData();
                Read();
                return true;
            case 4:
                DisplayResults();
                Read();
                return true;
            case 5:
                Clear();
                return false;
        }

        return true;
    }

    private void LoadData()
    {
        Task loadAssets = am.LoadAssets();
        loadAssets.Wait();
        _productionUnits = am.ProductionUnits!.Values.ToList();

        WriteLine();

        Task fetchTimeSeriesDataAsync = sdm.FetchTimeSeriesDataAsync();
        fetchTimeSeriesDataAsync.Wait();
        _dataPoints = sdm.TimeSeriesData!;

        opt.LoadData();
        opt.Optimize();

        _results = opt.Results!;
    }

    private void DisplayProductionUnits()
    {
        var table = new Table { Border = TableBorder.MinimalHeavyHead };

        table.AddColumn("Name");
        table.AddColumn("Max Heat (MWh");
        table.AddColumn("Production Cost (dkk)");
        table.AddColumn("Max Electricity (MW)");
        table.AddColumn("Gas / Oil Consumption MWh");
        table.AddColumn("CO2 Emissions kg/MWh");

        foreach (var unit in _productionUnits)
        {
            table.AddRow(unit.Name, $"{unit.MaxHeat}", $"{unit.ProductionCost}", $"{unit.MaxElectricity}",
                $"{unit.GasConsumption}", $"{unit.Co2Emission}");
        }

        AnsiConsole.Write(table);
    }

    private void DisplayData()
    {
        var table = new Table { Border = TableBorder.MinimalHeavyHead };

        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Heat Demand");
        table.AddColumn("Electricity Price");

        foreach (var data in _dataPoints)
        {
            table.AddRow($"{data.StartTime.ToString(CultureInfo.InvariantCulture)}",
                $"{data.EndTime.ToString(CultureInfo.InvariantCulture)}",
                $"{data.HeatDemand}", $"{data.ElectricityPrice}");
        }

        table.Columns[2].RightAligned();
        table.Columns[3].RightAligned();
        AnsiConsole.Write(table);
    }

    private void DisplayResults()
    {
        var table = new Table { Border = TableBorder.MinimalHeavyHead };

        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Heat Demand");
        table.AddColumn("Electricity Price");
        table.AddColumn("Net Production Cost");
        table.AddColumn("Boilers");
        table.AddColumn("Operation Point");

        foreach (var result in _results)
        {
            string boilers = string.Empty;
            string op = string.Empty;

            boilers = BoilerString(result.Boilers);
            op = OperationPointString(result.Boilers);

            table.AddRow(
                $"{result.StartTime.ToString(CultureInfo.InvariantCulture)}",
                $"{result.EndTime.ToString(CultureInfo.InvariantCulture)}",
                $"{result.HeatDemand}",
                $"{result.ElectricityPrice}",
                $"{result.NetProductionCost}",
                $"{boilers}",
                $"{op}");
        }

        AnsiConsole.Write(table);

        string OperationPointString(List<ProductionUnit> units)
        {
            string oPointString = string.Empty;

            foreach (var unit in units)
            {
                oPointString = string.Concat(unit.OperationPoint, "\n", oPointString);
            }

            return oPointString;
        }

        string BoilerString(List<ProductionUnit> units)
        {
            string boilerString = string.Empty;

            foreach (var unit in units)
            {
                boilerString = String.Concat(unit.Name, "\n", boilerString);
            }

            return boilerString;
        }
    }
}
