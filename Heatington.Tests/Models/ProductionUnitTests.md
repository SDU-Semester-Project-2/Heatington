# ProductionUnit Tests

This document provides an overview of the unit tests for the `ProductionUnit` class in the `Heatington.Models` namespace.

## Tests

### Constructor_Works_Properly

Checks if the constructor initializes the properties correctly.

It creates a new instance of `ProductionUnit` with a set of predefined values and then checks if each property of the instance is initialized with the correct value.

### OperationPoint_ValidRange

Checks that the `OperationPoint` property can be set within a valid range.

It sets the `OperationPoint` of a `ProductionUnit` instance to a value within the valid range (0 to 1) and then checks if it is updated to the correct value.

### OperationPoint_OutOfRange

Ensures that setting the `OperationPoint` property outside of the valid range throws an `ArgumentOutOfRangeException`.

It attempts to set the `OperationPoint` of a `ProductionUnit` instance to a value outside the valid range (less than 0 or greater than 1) and checks if an `ArgumentOutOfRangeException` is thrown.
