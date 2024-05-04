# JsonControllerTests Documentation

The `JsonControllerTests` class contains unit tests for the `JsonController` class, which handles reading and writing data to JSON files. These tests cover scenarios related to reading and writing different types of data.

## Test 1: ReadProductionUnitFromFile_ReadData_ReadsCorrectJson

### Description
This test verifies that the `JsonController` can correctly read a `ProductionUnit` object from a JSON file.

### Test Steps
1. **Arrange**: Set up the test environment by creating a test JSON file (`testJSON1.json`) and creating an expected `ProductionUnit` object.
2. **Act**: Write the expected `ProductionUnit` object to the test JSON file and read it back using the `JsonController`.
3. **Assert**: Verify that the actual `ProductionUnit` object matches the expected one.

## Test 2: WriteProductionUnitToFile_WriteData_WritesCorrectJson

### Description
This test ensures that the `JsonController` can correctly write a `ProductionUnit` object to a JSON file.

### Test Steps
1. **Arrange**: Create a test JSON file (`testJSON2.json`) and prepare a `ProductionUnit` object to be written.
2. **Act**: Write the `ProductionUnit` object to the test JSON file using the `JsonController`.
3. **Assert**:
    - Verify that the write operation was successful (status should be `SUCCESS`).
    - Deserialize the JSON content from the file and compare it with the original `ProductionUnit` object.

## Test 3: ReadHeatingGridFromFile_ReadData_ReadsCorrectJson

### Description
This test checks if the `JsonController` can correctly read a `HeatingGrid` object from a JSON file.

### Test Steps
1. **Arrange**: Create a test JSON file (`testJSON3.json`) and create an expected `HeatingGrid` object.
2. **Act**: Write the expected `HeatingGrid` object to the test JSON file and read it back using the `JsonController`.
3. **Assert**: Ensure that the actual `HeatingGrid` object matches the expected one.

## Test 4: WriteHeatingGridToFile_WriteData_WritesCorrectJson

### Description
This test validates that the `JsonController` can properly write a `HeatingGrid` object to a JSON file.

### Test Steps
1. **Arrange**: Prepare a test JSON file (`testJSON4.json`) and create a `HeatingGrid` object to be written.
2. **Act**: Write the `HeatingGrid` object to the test JSON file using the `JsonController`.
3. **Assert**:
    - Confirm that the write operation was successful (status should be `SUCCESS`).
    - Deserialize the JSON content from the file and compare it with the original `HeatingGrid` object.
