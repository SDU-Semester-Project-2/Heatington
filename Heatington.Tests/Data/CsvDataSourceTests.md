# CsvDataSourceTests

This document includes the unit tests for the `CsvDataSource` class.

## Test Class

The `CsvDataSourceTests` class is in the `Heatington.Tests.Data` namespace, testing the `CsvDataSource` class's data retrieval functionality.

## Test Methods

### `GetDataAsync_ShouldReturnDistData_GivenValidCsvFile`

The test verifies that the `GetDataAsync` function can correctly fetch and process valid CSV data.
A temporary file containing pre-defined CSV data is used for this purpose. The test validates the properties of the
first returned `DataPoint` against expected values. To pass, the function needs to return a non-empty list
of `DataPoint` objects.

### `GetDataAsync_ShouldThrowException_GivenNonExistFile`

This method tests if `GetDataAsync` throws a `FileNotFoundException` when given a file path that does not exist.
The test will pass when `GetDataAsync` throws the exception.

## Class Constructor and Dispose Method

The `CsvDataSourceTests` constructor creates a temporary file with a sample CSV data string for the class's test methods.
Once the tests have run, the `Dispose` method deletes this file. This approach ensures the tests do not leave residual
data or have side effects.

## Future Testing

The test method `SaveData_ShouldThrowNotImplementedException()` is commented out and can be used after the `SaveData()`
method is implemented.


