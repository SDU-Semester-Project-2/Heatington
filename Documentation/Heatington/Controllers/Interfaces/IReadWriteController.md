# IReadWriteController Interface

Namespace: `Heatington.Controllers.Interfaces`

## Description

Generic Interface for all controllers performing I/O operations.

## Members

### `Task<T?> ReadData<T>()`

Function for reading data out of a model.

#### Returns

Task to wait for Data.

### `Task<OperationStatus> WriteData<T>(T content)`

Function for Writing data into the model.

#### Parameters

- `content`: Content to write into the model, generic type.

#### Returns

Task to wait for status of the operation.
