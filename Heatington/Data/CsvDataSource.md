# CsvDataSource Class

## Overview

The `CsvDataSource` class serves as a concrete implementation of the `IDataSource` interface. It structures its operations around data that is specifically in CSV format, enabling the reading and writing of such data in the Heatington application.

## Methods

### `List<DataPoint>? GetData(string filePath)`

This method attempts to fetch time series data from a CSV file residing at the provided `filePath`.

The process begins by reading all the content of the file as a string (`rawData`). Afterwards, using the `CsvController` utility class, it deserializes this `rawData` into a `CsvData` object. The `CsvData` object can specifically handle and manipulate data that resides in a CSV format.

Subsequently, these deserialized records are converted into a list of `DataPoint` objects, which encapsulate the heat demand and electricity price data. If there is an issue in retrieving the data or if no data exists at the provided file path, the method returns `null`.

### `void SaveData(List<DataPoint> data, string filePath)`

This method is currently not implemented and throws a `NotImplementedException` when called. In future development, it's designed to save a list of `DataPoint` objects to a CSV file at the specified `filePath`.

The `List<DataPoint> data` represents the data points to be stored while the `string filePath` indicates the destination file path where the corresponding CSV file will be written. However, this method might not be used in the project depending on the application requirements.

## Remarks

Though the intention of the `CsvDataSource` class is to provide a concrete way to handle CSV data within the application, it's important to note that its ability to *write* data is currently not implemented, and may or may not be required depending on future development decisions. Future iterations may also explore options such as factory or builder patterns to enhance object creation or modification.
