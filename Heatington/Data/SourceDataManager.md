# SourceDataManager Class

## Overview

The `SourceDataManager` class is central to the Heatington application's data management. It coordinates data retrieval from and saving to a specific data source instance, packaged as the `IDataSource` interface.

Following the Dependency Inversion Principle, this class depends on an abstraction, the `IDataSource` interface, and not on a concrete class. This abstraction defines the getFile() and saveFile() methods and can be implemented by any data storage solution (e.g., `CsvDataSource`, `XmlDataSource`).

## Properties

- `_dataSource`: A private, read-only `IDataSource` instance that provides methods for accessing a specific data source.

- `_filePath`: A private, read-only string for storing the path of the file used for the data operations.

- `TimeSeriesData`: A public `List<DataPoint>` instance. It represents the collection of data, each entry indicating a specific point in time for which data is available.

## Constructor

- `SourceDataManager(IDataSource dataSource, string filePath)`

The class constructor accepts an instance of `IDataSource` and a file path as a string. These are stored in the `_dataSource` and `_filePath` properties, respectively. This approach enables the flexibility of the data source type and location.

## Methods

- `ConvertApiToCsv(List<DataPoint> dataFromApi)`

This method accepts a list of `DataPoint` items. These data points are then passed to the `IDataSource` instance's `SaveData` method along with the `_filePath`, and saved in the relevant data format.

- `FetchTimeSeriesData()`

This method uses the `_dataSource` instance to call its `GetData` method, passing the `_filePath`. The result is then stored in the public `TimeSeriesData` property.

- `LogTimeSeriesData()`

This method logs each `DataPoint` in `TimeSeriesData`. The log message includes the index, formatted start and end times, heat demand, and electricity price for each `DataPoint`. **This method will be removed after moving to the iteration with GUI**.

## Note

The implementation of the `SourceDataManager` class provides flexibility and extensibility for the Heatington application. Furthermore, this approach simplifies testing as `IDataSource` can be mocked easily.

## Usage Example

Here is an example of how to use the `SourceDataManager` class:

    ```csharp
        // Import the necessary namespaces
        using Heatington.Data;

        // Define the file path
        const string filePath = "../Assets/winter_period.csv";

        // Create a new CsvDataSource
        IDataSource dataSource = new CsvDataSource();

        SourceDataManager sdm = new(dataSource, filePath);

        // Fetch data from dataSource
        sdm.FetchTimeSeriesData();

        // Log the loaded data to the console
        sdm.LogTimeSeriesData();

        Console.WriteLine("Data loaded successfully.");
    ```
