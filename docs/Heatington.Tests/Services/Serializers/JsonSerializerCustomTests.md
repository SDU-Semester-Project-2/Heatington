# JsonSerializerCustomTests Documentation
***AI Generated***

## Overview

The `JsonSerializerCustomTests` class contains unit tests for the `JsonSerializerCustom` class, which handles custom serialization and deserialization using `System.Text.Json`. These tests cover various scenarios related to serialization, deserialization, and error handling.

## Test Methods

### 1. Deserialize Empty String: Gets Empty JSON

- **Method Name**: `Deserialize_DeserializeEmptyString_GetsEmptyJson`
- **Purpose**: Verifies that an empty JSON object (`{}`) can be correctly deserialized into an instance of `EmptyClass`.
- **Test Steps**:
    1. Arrange: Set up an empty JSON string.
    2. Act: Deserialize the JSON string using `JsonSerializerCustom`.
    3. Assert: Ensure that the deserialized object matches the expected empty object.

### 2. Deserialize Empty String: Converts to the Right Type

- **Method Name**: `Deserialize_DeserializeEmptyString_ConvertsToTheRightType`
- **Purpose**: Validates that the deserialized empty JSON string is of the expected type (`EmptyClass`).
- **Test Steps**:
    1. Arrange: Set up an empty JSON string.
    2. Act: Deserialize the JSON string using `JsonSerializerCustom`.
    3. Assert: Confirm that the deserialized object is of type `EmptyClass`.

### 3. Deserialize Production Unit String: Gets ProductionUnit

- **Method Name**: `Deserialize_DeserializeProductionUnitString_GetsProductionUnit`
- **Purpose**: Tests the deserialization of a JSON string representing a `ProductionUnit`.
- **Test Steps**:
    1. Arrange: Prepare a sample JSON string representing a `ProductionUnit`.
    2. Act: Deserialize the JSON string using `JsonSerializerCustom`.
    3. Assert: Compare the deserialized `ProductionUnit` with the expected values.

### 4. Deserialize Heating Grid String: Gets HeatingGrid

- **Method Name**: `Deserialize_DeserializeHeatingGridString_GetsHeatingGrid`
- **Purpose**: Validates the deserialization of a JSON string representing a `HeatingGrid`.
- **Test Steps**:
    1. Arrange: Create a sample JSON string for a `HeatingGrid`.
    2. Act: Deserialize the JSON string using `JsonSerializerCustom`.
    3. Assert: Ensure that the deserialized `HeatingGrid` matches the expected properties.

### 5. Deserialize Integer: Gets Int

- **Method Name**: `Deserialize_DeserializeInt_GetsInt`
- **Purpose**: Confirms that deserializing a valid integer string (`"1"`) results in the expected integer value.
- **Test Steps**:
    1. Arrange: Set up a valid integer JSON string.
    2. Act: Deserialize the JSON string using `JsonSerializerCustom`.
    3. Assert: Check that the deserialized integer matches the expected value.

### 6. Deserialize Text into Int: Error Handling

- **Method Name**: `Deserialize_DeserializeTextIntoInt_Error`
- **Purpose**: Tests error handling when attempting to deserialize a non-integer string.
- **Test Steps**:
    1. Arrange: Prepare a non-integer JSON string (e.g., `"Lorem Ipsum"`).
    2. Act: Attempt to deserialize the string using `JsonSerializerCustom`.
    3. Assert: Expect a `JsonException` to be thrown.

### 7. Deserialize Anonymous Function: Error Handling

- **Method Name**: `Deserialize_DeserializeAnonFunc_Error`
- **Purpose**: Ensures that invalid input (e.g., an anonymous function) raises a `JsonException`.
- **Test Steps**:
    1. Arrange: Set up an invalid JSON string (e.g., an anonymous function).
    2. Act: Try to deserialize the string using `JsonSerializerCustom`.
    3. Assert: Verify that a `JsonException` is thrown.

---
