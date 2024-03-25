
namespace Heatington.Tests;

/// <summary>
/// Contains unit tests for the Optimizer class.
/// </summary>
public class OptimizerTests
{

    /// <summary>
    /// TODO: Delete the stub optimizer class and its methods and replace them with the actual implementation.
    /// I'm not sure what the actual implementation should look like, so I'm just going to leave this here for now.
    /// This is after my research the one way of TDD with a class that has no implementation yet.
    /// </summary>
    public class Optimizer
    {
        private Dictionary<string, double> _optimizedData = new Dictionary<string, double>();
        public double CalculateNetProductionCost(ProductionUnit unit)
        {
            return 0;
        }

        public bool CompareUnits(ProductionUnit unit1, ProductionUnit unit2)
        {
            return true;
        }

        public bool CreateTimeSeriesData()
        {
            return true;
        }
    }

    public class ProductionUnit(
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
        private Guid _id = id;
        private string _name = name;
        private string _imgPath = imgPath;
        private double _operationPoint = operationPoint;
        private double _maxHeat = maxHeat;
        private double _productionCost = productionCost;
        private double _maxElectricity = maxElectricity;
        private double _gasConsumption = gasConsumption;
        private double _co2Emission = co2Emission;
    }


    /// <summary>
    /// Calculates the net production cost for a gas boiler and returns the correct value.
    /// </summary>
    /// <returns>The net production cost for a gas boiler.</returns>
    [Fact]
    public void CalculateNetProductionCost_GasBoiler_ReturnsCorrectValue()
    {
        // Arrange

        // Act

        // Assert

    }

    /// <summary>
    /// Calculates the net production cost for a gas boiler and ensures the result is a positive number.
    /// </summary>
    /// <returns>The net production cost for a gas boiler, which should be positive</returns>
    [Fact]
    public void CalculateNetProductionCost_GasBoiler_ReturnsPositiveNumber()
    {
        // Arrange

        // Act

        // Assert

    }

    /// <summary>
    /// Calculates the net production cost for an oil boiler and returns the correct value.
    /// </summary>
    /// <returns>The net production cost for an oil boiler.</returns>
    [Fact]
    public void CalculateNetProductionCost_OilBoiler_ReturnsCorrectValue()
    {
        // Arrange

        // Act

        // Assert
    }

    /// <summary>
    /// Calculates the net production cost for an oil boiler and verifies that it returns a positive number.
    /// </summary>
    /// <return>
    /// The net production cost for an oil boiler, which should be positive.
    /// </return>
    [Fact]
    public void CalculateNetProductionCost_OilBoiler_ReturnsPositiveNumber()
    {
        // Arrange

        // Act

        // Assert
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
    public void CompareUnits_GasAndOilBoiler_ReturnsCorrectComparisionResult()
    {
        // Arrange

        // Act

        // Assert
    }

    /// <summary>
    /// Tests the method to create time series data with optimized data and returns a boolean indicating success.
    /// </summary>
    /// <returns>
    /// true if the method successfully creates time series data with optimized data, false otherwise.
    /// </returns>
    [Fact]
    public void CreateTimeSeriesData_OptimizedData_ReturnsSuccessBool()
    {
        // Arrange

        // Act

        // Assert
    }
}


