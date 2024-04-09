# FileController Class

Namespace: `Heatington.Controllers`

## Example

```csharp
string pathToFile = Utilities.GeneratePathToFileInAssetsDirectory("testFile.json");
IReadWriteController fileController = new FileController(pathToFile);
await fileController.WriteData("1");
string? data = await fileController.ReadData();
```


## Description

Class for performing read/write actions on local files.

## Constructor

### `FileController(string pathToFile)`

Class constructor. Gets a path to the file to control.

#### Parameters

- `pathToFile` (string): Path to the location of a file. Follows the pattern `file.json`.

## Members

### `TryFileOperationRunner<T>(Func<Task<T>> funcToTry)`

Helper method performing try-catch clauses in file-oriented manner.

#### Parameters

- `funcToTry` (Func<Task<T>>): Function to run inside of try-catch block.

#### Type Parameters

- `T`: Return type of a function.

#### Returns

Return the outcome of the run function. If the function returns void, the lambda function should return 0.

### `ReadFileFromPath()`

Function for reading the contents of a local file. Reads from the path passed during object initialization.

#### Returns

Content of the file as a string.

#### Exceptions

- `FileNotFoundException`: If the file does not exist or the path is not correct, the exception is thrown.

### `WriteToFileFromPath(string content)`

Function for writing string data into a local file from a path.

#### Parameters

- `content` (string): Content to be written to a file as a string.

### `ReadData<T>()`

Function for reading the data of a local file. Reads from the path passed during object initialization.

#### Returns

Data of the file as a string.

### `WriteData<T>(T content)`

Function for writing data into a local file.

#### Parameters

- `content` (T): Content to be written to a file as a string.
