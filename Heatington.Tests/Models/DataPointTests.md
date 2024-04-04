# `DataPoint` Unit Tests

This document describes the unit tests for the `DataPoint` class.

## Test Class

The `DataPointTests` class in the `Heatington.Tests.Models` namespace contains the unit tests for the `DataPoint` constructor.

## Test Methods

### `Constructor_ShouldParse_ValidArguments`

This test method verifies that the `DataPoint` constructor correctly parses strings to the appropriate data types (e.g., `DateTime` and `double`).

It provides a set of valid arguments to the `DataPoint` constructor, and compares the properties of the created object to the expected values. The test will pass if all properties match the expected values.

Useful output indicating the expected and gotten values for every property are also provided during test execution.

### `Constructor_ShouldThrowFormatException_ForInvalidArguments`

This test method verifies that the `DataPoint` constructor throws a FormatException when an argument cannot be parsed to the correct data type.

It provides sets of invalid arguments to the `DataPoint` constructor in various combinations. The test will pass if a `FormatException` is thrown in each case.

## Setup and using `ITestOutputHelper`

An `ITestOutputHelper` is being used in this class to help with producing diagnostic output. This can be especially useful when trying to debug failures in the tests.

Its output will be present in the test runner's output window whenever the unit test is run. This output will contain statements showing the expected and actual values for each property during the execution of `Constructor_ShouldParse_ValidArguments`.

## Running the Tests

Run these tests by using your preferred .NET test runner and navigating to the `DataPointTests` class.

These tests ensure that the `DataPoint` class and its constructor are working as expected under different circumstances.
