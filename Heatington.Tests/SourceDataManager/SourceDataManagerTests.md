# SourceDataManager Unit Test

This document provides an overview of a unit test for `SourceDataManager`.

## Source DataManager Test Class

`SourceDataManagerTest` is a class that contains unit tests for the `SourceDataManager` class. The class includes a setup to establish and initialize the dependencies for the test.

```csharp
private readonly FakeDataSource _fakeDataSource;
private readonly Heatington.SourceDataManager.SourceDataManager _sourceDataManager;
```

`_fakeDataSource` is an instance of `FakeDataSource`, a stub class that mimics the behavior of a real-world data storage.
`_sourceDataManager` is the class instance we are testing, `SourceDataManager`.

## Test Method: FetchTimeSeriesDataAsync_ShouldFetchDataSuccessfully

This method tests the behavior of the `FetchTimeSeriesDataAsync` method under different scenarios. To facilitate this, we are using the `Theory` attribute along with multiple `InlineData` attributes.

```csharp
[Theory] ...
public async Task FetchTimeSeriesDataAsync_ShouldFetchDataSuccessfully(params string[] data)
{ ...
```

This means that this test method will be run multiple times, once for each `InlineData` attribute. The parameters for each `InlineData` attribute are mapped to the `data` array.

Inside the test method:
- In the Arrange step, an array of data points (timestamps and the corresponding heat and electricity readings) are created and added to `_fakeDataSource.Data`,
- The Act step calls the `FetchTimeSeriesDataAsync` method, and
- The Assert step asserts that the data fetched from the `FetchTimeSeriesDataAsync` method and the `_fakeDataSource.Data` counts are equal, confirming the fetch function's correct functioning.

## Fake Data Source

The `FakeDataSource` class is used to simulate the `IDataSource` interface's behaviors. This "mock" class allows for testing the `SourceDataManager` in isolation, without requiring any actual data source.

The `GetDataAsync` method in `FakeDataSource` returns the data objects set in the test method. The `SaveData` method is not used in these tests and therefore throws a `NotImplementedException` when called. This class enables a controlled environment that makes the tests more predictable and easier to debug.

## Notes

The team has decided against using a mocking framework for this project, opting instead for manual stubs and fakes.
This decision was made to keep the project lightweight and avoid introducing unnecessary dependencies.
