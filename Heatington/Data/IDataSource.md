# IDataSource Interface

## Overview

The `IDataSource` interface represents a data source for reading and writing data in the Heatington application. Any classes implementing this interface are expected to provide methods for retrieving and saving data.

## Methods

### `List<DataPoint>? GetData(string filePath)`

This method is intended to retrieve time series data from the specified data source. The input parameter is a `string` that represents the file path of the CSV file containing the data. The return value of this method is a list of `DataPoint` objects representing the heat demand and electricity price data. If no data could be retrieved, the method may return `null`.

### `void SaveData(List<DataPoint> data, string filePath)`

This method is intended to save data points to a CSV file at the specified file path. The two input parameters are a `List<DataPoint>` representing the data points to be saved and a `string` that provides the file path where the CSV file will be saved.

The method does not return a value. As the return type is `void`, any errors that occur during data saving operations should be communicated through exceptions.

## Implementations

All data sources in the Heatington application should implement this interface to ensure consistency and enable easier switching between different data sources. Examples of potential implementations include `CsvDataSource`, `XmlDataSource`, etc.
