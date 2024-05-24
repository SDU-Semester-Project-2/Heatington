# OptimizerTests.cs

This documentation provides an overview of the unit tests implemented in the `OPTTests` class.
## Test Methods

### 1. Optimize_Scenario1_ShouldRankProductionUnitsByProductionCost

- **Description**: This test method verifies that the production units are ranked based on their production cost in Scenario 1 optimization mode.
- **Arrange**: Initialize test production units and data points.
- **Act**: Call the `Optimize` method with Scenario 1 optimization mode.
- **Assert**: Ensure that the results contain the expected production unit ranked first based on production cost.

### 2. Optimize_Scenario2_ShouldRankProductionUnitsByNetProductionCost

- **Description**: This test method verifies that the production units are ranked based on their net production cost in Scenario 2 optimization mode.
- **Arrange**: Initialize test production units and data points.
- **Act**: Call the `Optimize` method with Scenario 2 optimization mode.
- **Assert**: Ensure that the results contain the expected production unit ranked first based on net production cost.

### 3. Optimize_Co2_ShouldRankProductionUnitsByCo2Emission

- **Description**: This test method verifies that the production units are ranked based on their CO2 emissions in CO2 optimization mode.
- **Arrange**: Initialize test production units and data points.
- **Act**: Call the `Optimize` method with CO2 optimization mode.
- **Assert**: Ensure that the results contain the expected production unit ranked first based on CO2 emission.

### 4. SetOperationPoint_ShouldCorrectlySetOperationPointBasedOnHeatDemand

- **Description**: This test method verifies that the operation points of production units are correctly set based on heat demand.
- **Arrange**: Initialize test production units.
- **Act**: Call the `SetOperationPoint` method with a specific heat demand.
- **Assert**: Ensure that the operation points of production units are set correctly.

### 5. CalculateHeatUnitsRequired_ShouldReturnCorrectResult

- **Description**: This test method verifies that the calculation of heat units required produces the correct result.
- **Arrange**: Initialize test production units and a test data point.
- **Act**: Call the `CalculateHeatUnitsRequired` method.
- **Assert**: Ensure that the calculated heat units required match the expected result.

## Note

- Debug output is used in some test methods to provide additional logging during test execution.
- This test is really lightweight and may be expanded in the future with Moq and more comprehensive testing.
