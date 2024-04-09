# FileControllerTests Class

Namespace: `Heatington.Tests.Controllers`

## Description

Unit tests for the `FileController` class.

## Constructor

### `FileControllerTests()`

Creates a temporary test folder for testing purposes.

## Members

### `ReadFileFromPath_ReadFile_ReadsCorrectContent()`

Unit test to verify that reading a file from a specified path reads the correct content.

### `WriteFileFromPath_WriteFile_WritesCorrectContent()`

Unit test to verify that writing to a file from a specified path writes the correct content.

### `WriteFileFromPath_WriteEmptyStringToFile_CreatesFile()`

Unit test to verify that writing an empty string to a file creates the file.

### `WriteToFileFromPath_WriteToTheSameFileTwice_CreatesTwo()`

Unit test to verify that writing to the same file twice creates two files with the same name.

### `ReadFileFromPath_ReadNotExistingFile_FileNotFound(string wrongFileName)`

Unit test to verify that reading from a path to a file that does not exist throws a `FileNotFoundException`.

## IDisposable Implementation

### `Dispose()`

Clears the temporary test directory after all tests have been run.
