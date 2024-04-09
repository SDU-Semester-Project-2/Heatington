# Utilities Class

Namespace: `Heatington.Helpers`

## Description

Utility class providing various helper methods.

## Members

### `DisplayException(string message)`

Displays an exception message in red color in the console.

#### Parameters

- `message` (string): The exception message to display.

### `ConvertObject<T>(object? obj)`

Converts an object to the specified type `T`.

#### Parameters

- `obj` (object): The object to convert.

#### Returns

The converted object of type `T`.

### `GetAbsolutePathToAssetsDirectory()`

Gets the absolute path to the assets directory of the project.

#### Returns

The absolute path to the assets directory.

### `GeneratePathToFileInAssetsDirectory(string fileName)`

Generates the path to a file in the assets directory based on the provided file name.

#### Parameters

- `fileName` (string): The name of the file.

#### Returns

The full path to the file in the assets directory.
