# Unit Testing the SourceDataManager Class

This documentation provides insights into the unit testing approach for the `SourceDataManager` class,

## Structure of `SourceDataManagerTest` Class

The `SourceDataManagerTest` class encapsulates the unit tests for `SourceDataManager`, where we set up instances of the
necessary dependencies.

We utilize _stubDataSource as a stub for our actual data source. The _sourceDataManager
is the instance of SourceDataManager, the class subjected to testing.

# The Test Method `FetchTimeSeriesDataAsync_ShouldFetchDataSuccessfully`

We designed `FetchTimeSeriesDataAsync_ShouldFetchDataSuccessfully` to simulate the behaviour
of `FetchTimeSeriesDataAsync`.
This is facilitated via the `Theory` attribute in with several `InlineData` attributes,
allowing the execution of this test method multiple times with different argument sets.

During each execution:

- **Arrange**: Similar to the previous test, we populate the `_stubDataSource.Data` with `DataPoint` objects.

- **Act**: We invoke the `FetchTimeSeriesDataAsync` method.

- **Assert**: We check if the `DataPoint` objects fetched by `FetchTimeSeriesDataAsync` match the `DataPoint` objects
- in `_stubDataSource.Data`.

## The Test Method `VerifyDataPoints_ShouldMatchExpectedData`

The test method `VerifyDataPoints_ShouldMatchExpectedData` checks if the fetched data by `FetchTimeSeriesDataAsync`
matches the expected data.


During each execution:

- **Arrange**: Initially, `DataPoint` objects, consisting of timestamps and corresponding heat and electricity readings,
- are populated into `_stubDataSource.Data` using multiple sets of data provided by the InlineData attribute
- **Act**: Subsequently, the `FetchTimeSeriesDataAsync` method is invoked.
- **Assert**: Finally, we verify the equality between the counts of the data fetched by `FetchTimeSeriesDataAsync and
- **_stubDataSource.Data**, thereby establishing the successful operation of the fetch function.

## The Stub Data Source `StubDataSource`

We introduced `StubDataSource` to simulate behaviors corresponding to the `IDataSource` interface.
This stub class enables us to test `SourceDataManager` in isolation, eliminating any dependencies on actual data
sources.

Calling `GetDataAsync` on `StubDataSource` yields the custom data objects set in the test method.
However, invoking SaveData is futile since the tests do not utilize it, and it throws NotImplementedException when
called.

## Concluding Remarks

Our preference for manual stubs (instead of a mocking framework) ensures that our project remains lightweight without
any surplus dependencies. This simplifies and clarifies the test scenarios, leading to more straightforward debugging
and maintenance endeavours.
