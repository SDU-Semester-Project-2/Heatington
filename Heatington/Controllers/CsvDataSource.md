# `CsvDataSource` Class

## Overview

The `CsvDataSource` class provides a concrete implementation of the `IDataSource` interface specific to data in CSV
format, allowing for the reading of such data within the Heatington application.

## Methods

### `Task<List<DataPoint>?> GetDataAsync(string filePath)`

The `GetDataAsync` method is a function for asynchronously fetching data from a CSV file located at a provided file path.

The method initiates by asynchronously reading the file’s entire content into a string (`rawData`). The `rawData` is
then deserialized by the `CsvController` utility class into a `CsvData` object – a controller specifically designed to handle
and manipulate data in CSV format.

Following the successful deserialization of the `rawData`, the `CsvData` object is converted into a `List` of `DataPoint`
objects. Each `DataPoint` object encapsulates heat demand and electricity price data.

Should the method fail to retrieve data from the file or if no data exists at the provided file path, then the method
will return `null`.

Upon the occurrence of an exception, the exception's message is displayed using the
`Utilities.DisplayException(e.Message)` method and the exception is then re-thrown.

### `void SaveData(List<DataPoint> data, string filePath)`

The `SaveData` method remains unimplemented and consequently triggers a `NotImplementedException` when called.

It is projected that in the future, the method will save a `List` of `DataPoint` objects into a CSV file located at a
specified file path. The `List<DataPoint> data` parameter represents the data points to be stored. The `string filePath`
parameter provides the location of where the CSV file will be created or rewritten.

The utilization of this method remains dependent upon the needs of the Heatington project.

## Remarks

While the `CsvDataSource` class is intended to offer a concrete manner for handling CSV data in the application,
it's worth to mention that its ability to write data is not implemented. Depending on future development decisions,
this feature could potentially remain so. The future of object creation and modification may also leverage design
patterns such as the Factory or Builder.
