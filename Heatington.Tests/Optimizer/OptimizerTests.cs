using Heatington.Models;
using Heatington.Optimizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace Heatington.Tests
{
    public class OPTTests
    {
        [Fact]
        public void Optimize_Scenario1_ShouldRankProductionUnitsByProductionCost()
        {
            // Arrange
            var testUnits = new List<ProductionUnit>
            {
                new ProductionUnit("TestProductionUnit1", "Test Production Unit 1", "path1", 5, 100, 0, 0.5, 10),
                new ProductionUnit("TestProductionUnit2", "Test Production Unit 2", "path2", 4, 150, 0, 0.6, 15),
                new ProductionUnit("TestProductionUnit3", "Test Production Unit 3", "path3", 3.6, 200, 2, 0.8, 20)
            };

            var testDataPoints = new List<DataPoint> { new DataPoint(DateTime.Now, DateTime.Now.AddHours(1), 10, 50) };

            var opt = new OPT(testUnits, testDataPoints);

            // Act
            opt.Optimize(OptimizationMode.Scenario1);

            // Assert
            var results = opt.Results;
            Assert.NotNull(results);
            Assert.Single(results);
            Debug.WriteLine($"Expected: {testUnits[0].Id}, Actual: {results[0].Boilers[0].Id}");
            Assert.Equal(testUnits[0].Id, results[0].Boilers[0].Id);
        }

        [Fact]
        public void Optimize_Scenario2_ShouldRankProductionUnitsByNetProductionCost()
        {
            // Arrange
            var testUnits = new List<ProductionUnit>
            {
                new ProductionUnit("TestProductionUnit1", "Test Production Unit 1", "path1", 5, 100, 0, 0.5, 10),
                new ProductionUnit("TestProductionUnit2", "Test Production Unit 2", "path2", 4, 150, 0, 0.6, 15),
                new ProductionUnit("TestProductionUnit3", "Test Production Unit 3", "path3", 3.6, 200, 2, 0.8, 20)
            };

            var testDataPoints = new List<DataPoint> { new DataPoint(DateTime.Now, DateTime.Now.AddHours(1), 10, 50) };

            var opt = new OPT(testUnits, testDataPoints);

            // Act
            opt.Optimize(OptimizationMode.Scenario2);

            // Assert
            var results = opt.Results;
            Assert.NotNull(results);
            Assert.Single(results);
            Debug.WriteLine($"Expected: {testUnits[0].Id}, Actual: {results[0].Boilers[0].Id}");
            Assert.Equal(testUnits[0].Id, results[0].Boilers[0].Id);
        }

        [Fact]
        public void Optimize_Co2_ShouldRankProductionUnitsByCo2Emission()
        {
            // Arrange
            var testUnits = new List<ProductionUnit>
            {
                new ProductionUnit("TestProductionUnit1", "Test Production Unit 1", "path1", 5, 100, 0, 0.5, 10),
                new ProductionUnit("TestProductionUnit2", "Test Production Unit 2", "path2", 4, 150, 0, 0.6, 15),
                new ProductionUnit("TestProductionUnit3", "Test Production Unit 3", "path3", 3.6, 200, 2, 0.8, 20)
            };

            var testDataPoints = new List<DataPoint> { new DataPoint(DateTime.Now, DateTime.Now.AddHours(1), 10, 50) };

            var opt = new OPT(testUnits, testDataPoints);

            // Act
            opt.Optimize(OptimizationMode.Co2);

            // Assert
            var results = opt.Results;
            Assert.NotNull(results);
            Assert.Single(results);
            Debug.WriteLine($"Expected: {testUnits[0].Id}, Actual: {results[0].Boilers[0].Id}");
            Assert.Equal(testUnits[0].Id, results[0].Boilers[0].Id);
        }

        [Fact]
        public void SetOperationPoint_ShouldCorrectlySetOperationPointBasedOnHeatDemand()
        {
            // Arrange
            var testUnits = new List<ProductionUnit>
            {
                new ProductionUnit("TestProductionUnit1", "Test Production Unit 1", "path1", 5, 100, 0, 0.5, 10),
                new ProductionUnit("TestProductionUnit2", "Test Production Unit 2", "path2", 4, 150, 0, 0.6, 15)
            };

            var opt = new OPT(testUnits, null);

            // Act
            var workingUnits = opt.SetOperationPoint(testUnits, 6);

            // Assert
            Assert.NotNull(workingUnits);
            Assert.Equal(2, workingUnits.Count);
            Assert.Equal(1, workingUnits[0].OperationPoint);
            Assert.Equal(0.25, workingUnits[1].OperationPoint);
            Debug.WriteLine(
                $"Expected: 1, 0.25; Actual: {workingUnits[0].OperationPoint}, {workingUnits[1].OperationPoint}");
        }

        [Fact]
        public void CalculateHeatUnitsRequired_ShouldReturnCorrectResult()
        {
            // Arrange
            var testUnits = new List<ProductionUnit>
            {
                new ProductionUnit("TestProductionUnit1", "Test Production Unit 1", "path1", 5, 100, 0, 0.5, 10),
                new ProductionUnit("TestProductionUnit2", "Test Production Unit 2", "path2", 4, 150, 0, 0.6, 15)
            };

            var testDataPoint = new DataPoint(DateTime.Now, DateTime.Now.AddHours(1), 6, 50);

            var opt = new OPT(testUnits, null);

            // Act
            var result = opt.CalculateHeatUnitsRequired(testDataPoint, testUnits);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.HeatDemand);
            Assert.Equal(2, result.Boilers.Count);
            Assert.Equal(1, result.Boilers[0].OperationPoint);
            Assert.Equal(0.25, result.Boilers[1].OperationPoint);
            Debug.WriteLine(
                $"Expected: 1, 0.25; Actual: {result.Boilers[0].OperationPoint}, {result.Boilers[1].OperationPoint}");
        }
    }
}
