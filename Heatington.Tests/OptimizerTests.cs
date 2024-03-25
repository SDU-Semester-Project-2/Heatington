using Xunit.Abstractions; // ITestOutputHelper interface is used to send diagnostic information to the test output

namespace Heatington.Tests;

/// <summary>
/// Contains unit tests for the Optimizer class.
/// </summary>
public class OptimizerTests(ITestOutputHelper testOutputHelper) // Constructor injection of ITestOutputHelper
{

    // TODO: Replace the StubOptimizer and StubProductionUnit classes with the actual implementation.
    // I followed TDD approach to write the tests first and then implement the actual classes.
    private readonly StubOptimizer _optimizer = new();

    /// <summary>
    /// Represents a gas boiler production unit.
    /// </summary>
    private readonly StubProductionUnit _gasBoiler = new(
        Guid.NewGuid(),
        "Gas Boiler",
        "img/gas-boiler.png", // fake path
        0,
        5, // MWh(th)
        500, // DKK/MWh(th)
        0,
        1.1, // MWh(gas)/MWh(th)
        215); // kgCO2/MWh(th)

    /// <summary>
    /// Represents an oil boiler for heating.
    /// </summary>
    private readonly StubProductionUnit _oilBoiler = new(
        Guid.NewGuid(),
        "Oil Boiler",
        "img/oil-boiler.png", // fake path
        0,
        4, // MWh(th)
        700, // DKK/MWh(th)
        0,
        1.2, // MWh(oil)/MWh(th)
        265); // kgCO2/MWh(th)


    /// <summary>
    /// The StubOptimizer class is a placeholder class that needs to be replaced with the actual implementation.
    /// It provides stub methods for calculating net production cost, comparing production units, and creating time series data.
    /// </summary>
    private class StubOptimizer
    {
        /// The _optimizedData variable is a private field of type Dictionary<double, double> in the StubOptimizer class.
        /// It represents the optimized data for different heat demand levels.
        private Dictionary<double, double> _optimizedData = new();

        /// The `_sortedUnitsByProductionCost` variable is a sorted list of production units based on their production cost.
        /// The list is sorted in ascending order, with the production unit with the lowest cost at the beginning of the list.
        /// The variable is initially an empty list.
        /// The list is populated and updated when the `CompareUnits` method is called.
        private List<StubProductionUnit> _sortedUnitsByProductionCost = [];


        /// <summary>
        /// Calculates the net production cost for a given production unit.
        /// </summary>
        /// <param name="unit">The production unit for which to calculate the net production cost.</param>
        /// <returns>The net production cost for the given production unit.</returns>
        public double CalculateNetProductionCost(StubProductionUnit unit)
        {
            // The net production cost for heat only boilers are just the production cost. see the project description.
            if (unit.Name is "Oil Boiler" or "Gas Boiler")
            {
                return unit.ProductionCost;
            }

            return 0; // TODO: Implement the calculation for other production units in the second iteration.
        }

        /// <summary>
        /// Compares a list of production units based on their production cost and returns the sorted list.
        /// </summary>
        /// <param name="units">The production units to compare.</param>
        /// <returns>The sorted list of production units based on their production cost.</returns>
        public List<StubProductionUnit> CompareUnits(params StubProductionUnit[] units)
        {
            _sortedUnitsByProductionCost = units.OrderBy(unit => unit.ProductionCost).ToList();
            return _sortedUnitsByProductionCost;
        }

        /// <summary>
        /// Creates time series data for the optimized boiler operation based on heat demand.
        /// Steps:
        /// 1. Starts from the most efficient unit (the first in the sorted list).
        /// 2. Tries to supply the current heat demand with this unit.
        /// 3. If the heat demand is higher than the max heat this unit can produce, move to add next efficient unit.
        /// 4. Continue this process until the heat demand is fulfilled or all units are used.
        /// </summary>
        /// <returns>A dictionary representing the time series data, where the key is the heat demand and the value is a dictionary of boiler operation data.</returns>
        public Dictionary<double, Dictionary<string, double>>? CreateTimeSeriesData()
        {
            Dictionary<double, Dictionary<string, double>> timeSeriesData = new();
            ;
            const int stubMaxHeatDemand = 9;

            for (int i = 0; i <= stubMaxHeatDemand * 10; ++i)
            {
                Dictionary<string, double> boilerOperationData = new();
                double heatDemand = i / 10.0;
                double remainingHeatDemand = heatDemand;
                foreach (StubProductionUnit unit in _sortedUnitsByProductionCost)
                {
                    if (remainingHeatDemand <= 0)
                    {
                        break;
                    }

                    double operationPoint = Math.Min(1, remainingHeatDemand / unit.MaxHeat);
                    boilerOperationData[unit.Name] = operationPoint;
                    remainingHeatDemand -= operationPoint * unit.MaxHeat;

                }
                timeSeriesData[heatDemand] = boilerOperationData;
            }

            return timeSeriesData;
        }
    }

    /// <summary>
    /// Represents a production unit in the system. This is a stub class that needs to be replaced with the actual implementation.
    /// </summary>
    private class StubProductionUnit(
        Guid id,
        string name,
        string imgPath,
        double operationPoint,
        double maxHeat,
        double productionCost,
        double maxElectricity,
        double gasConsumption,
        double co2Emission)
    {
        /// <summary>
        /// Represents the identifier of a production unit.
        /// </summary>
        public Guid Id = id;

        /// <summary>
        /// Represents the name of a production unit.
        /// </summary>
        public readonly string Name = name;

        /// <summary>
        /// The path of the image associated with a production unit.
        /// </summary>
        public readonly string ImgPath = imgPath;

        /// <summary>
        /// Represents an operation point of a production unit.
        /// </summary>
        public readonly double OperationPoint = operationPoint;

        /// <summary>
        /// Represents the maximum heat for a production unit in the system.
        /// </summary>
        public readonly double MaxHeat = maxHeat;

        /// <summary>
        /// Represents the production cost of a production unit.
        /// </summary>
        public readonly double ProductionCost = productionCost;

        /// <summary>
        /// Represents the maximum electricity value of a production unit.
        /// </summary>
        public readonly double MaxElectricity = maxElectricity;

        /// <summary>
        /// Represents the gas consumption of a production unit.
        /// </summary>
        public readonly double GasConsumption = gasConsumption;

        /// <summary>
        /// Represents the CO2 emission value of a production unit.
        /// </summary>
        public readonly double Co2Emission = co2Emission;
    }


    /// <summary>
    /// Calculates the net production cost for a gas boiler and returns the correct value.
    /// </summary>
    /// <returns>The net production cost for a gas boiler.</returns>
    [Fact]
    public void CalculateNetProductionCost_GasBoiler_ReturnsCorrectValue()
    {
        // Arrange
        double expected = _gasBoiler.ProductionCost;

        // Act
        double result = _optimizer.CalculateNetProductionCost(_gasBoiler);

        // Assert
        testOutputHelper.WriteLine($"Expected: {expected}, Result: {result}");
        Assert.Equal(expected, result);

    }

    /// <summary>
    /// Calculates the net production cost for a gas boiler and ensures the result is a positive number.
    /// </summary>
    /// <returns>The net production cost for a gas boiler, which should be positive.</returns>
    [Fact]
    public void CalculateNetProductionCost_GasBoiler_ReturnsPositiveNumber()
    {
        // Arrange

        // Act
        double result = _optimizer.CalculateNetProductionCost(_gasBoiler);
        // Assert
        testOutputHelper.WriteLine($"Result: {result}");
        Assert.True(result > 0);

    }

    /// <summary>
    /// Calculates the net production cost for an oil boiler and returns the correct value.
    /// </summary>
    /// <returns>The net production cost for an oil boiler.</returns>
    [Fact]
    public void CalculateNetProductionCost_OilBoiler_ReturnsCorrectValue()
    {
        // Arrange
        double expected = _oilBoiler.ProductionCost;
        // Act
        double result = _optimizer.CalculateNetProductionCost(_oilBoiler);

        // Assert
        testOutputHelper.WriteLine($"Expected: {expected}, Result: {result}");
        Assert.Equal(_oilBoiler.ProductionCost, result);
    }

    /// <summary>
    /// Calculates the net production cost for an oil boiler and verifies that it returns a positive number.
    /// </summary>
    /// <returns>The net production cost for an oil boiler, which should be positive.</returns>
    [Fact]
    public void CalculateNetProductionCost_OilBoiler_ReturnsPositiveNumber()
    {
        // Arrange

        // Act
        double result = _optimizer.CalculateNetProductionCost(_oilBoiler);

        // Assert
        testOutputHelper.WriteLine($"Result: {result}");
        Assert.True(result > 0);
    }


    /// <summary>
    /// Compares the gas boiler and oil boiler units and returns whether the comparison is correct or not.
    /// </summary>
    /// <returns>
    /// True if the comparison is correct, false otherwise.
    /// </returns>
    /// <remarks>
    /// This method is used to test the correctness of comparing gas boiler and oil boiler units.
    /// </remarks>
    [Fact]
    public void CompareUnits_GasAndOilBoiler_ReturnsCorrectComparisonResult()
    {
        // Arrange
        List<StubProductionUnit> expected = [_gasBoiler, _oilBoiler];

        // Act
        List<StubProductionUnit> result = _optimizer.CompareUnits(_gasBoiler, _oilBoiler);

        // Represent each list as a string
        string expectedToStr = string.Join(", ", expected.Select(x => $"Name: {x.Name}, ProductionCost: {x.ProductionCost}"));
        string resultToStr = string.Join(", ", result.Select(x => $"Name: {x.Name}, ProductionCost: {x.ProductionCost}"));

        // Assert
        testOutputHelper.WriteLine($"Expected: {expectedToStr}, Result: {resultToStr}");
        Assert.Equal(expected, result);
    }


    /// <summary>
    /// Generates time series data for heat demand step, ensuring correct data is returned.
    /// </summary>
    /// <remarks>
    /// This method tests the <see cref="StubOptimizer.CreateTimeSeriesData"/> method by comparing the expected keys against the actual data.
    /// It verifies that the generated time series data contains all the expected keys from 0 to 9.0 in steps of 0.1.
    /// </remarks>
    [Fact]
    public void CreateTimeSeriesData_HeatDemandStepIsCorrect()
    {
        // Arrange
        _optimizer.CompareUnits(_gasBoiler, _oilBoiler);
        Dictionary<double, Dictionary<string, double>>? actualData = _optimizer.CreateTimeSeriesData();

        // Act
        // Create expectedKeys list from 0 to 9.0 in 0.1 steps.
        List<double> expectedKeys = new List<double>();
        for (int i = 0; i <= 90; i++)
        {
            expectedKeys.Add(i / 10.0);
        }

        // Assert
        // Check if all expected keys are in the actual data.
        foreach (double key in expectedKeys)
        {
            bool keyExists = actualData?.ContainsKey(key) ?? false;
            Assert.True(keyExists, $"Key {key} missing in actual data");

            if (keyExists)
            {
                // Log the key
                testOutputHelper.WriteLine($"Key {key} is present in the actual data.");
            }
        }

        testOutputHelper.WriteLine("All expected keys are present in the actual data.");
    }
    /// <summary>
    /// Test method to verify that the CreateTimeSeriesData method returns the correct data when the heat demand is nine.
    /// </summary>
    /// <remarks>
    ///<see cref="StubOptimizer.CreateTimeSeriesData"/>
    /// This test method creates a StubOptimizer object and two StubProductionUnit objects: _gasBoiler and _oilBoiler.
    /// The expected operation points are defined in the dictionary expectedOperationPoints.
    /// The CompareUnits method of the StubOptimizer object is called with the _gasBoiler and _oilBoiler as parameters.
    /// The CreateTimeSeriesData method of the StubOptimizer object is then called. The returned data is stored in the actualData variable.
    /// If the actualData dictionary contains the key 9.0, the actual operation points for the gas boiler and oil boiler are retrieved.
    /// Finally, the test asserts that the actual operation points match the expected operation points.
    /// </remarks>
    [Fact]
    public void CreateTimeSeriesData_WhenHeatDemandIsNine_ReturnsCorrectData()
    {
        // Arrange
        var expectedOperationPoints = new Dictionary<double, Dictionary<string, double>>()
        {
            { 9.0, new Dictionary<string, double> { { _gasBoiler.Name, 1.0 }, { _oilBoiler.Name, 1.0 } } }
        };

        // Act
        _optimizer.CompareUnits(_gasBoiler, _oilBoiler);
        Dictionary<double, Dictionary<string, double>>? actualData = _optimizer.CreateTimeSeriesData();

        if (actualData?.ContainsKey(9.0) == true)
        {
            double actualGasBoilerOperationPoint = actualData[9.0][_gasBoiler.Name];
            double actualOilBoilerOperationPoint = actualData[9.0][_oilBoiler.Name];

            testOutputHelper.WriteLine(
                $"Gas boiler actual operation point at 9.0 heat demand: {actualGasBoilerOperationPoint}");
            testOutputHelper.WriteLine(
                $"Oil boiler actual operation point at 9.0 heat demand: {actualOilBoilerOperationPoint}");
        }

        // Assert
        Assert.Equal(expectedOperationPoints[9.0][_gasBoiler.Name], actualData?[9.0][_gasBoiler.Name]);
        Assert.Equal(expectedOperationPoints[9.0][_oilBoiler.Name], actualData?[9.0][_oilBoiler.Name]);
    }

    // TODO: Add more unit tests for the Optimizer class. E.g. test cases for edge cases, boundary conditions, and exceptions.


}


