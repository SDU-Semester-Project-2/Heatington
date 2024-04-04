# `DataPoint` Class

## Introduction

The `DataPoint` class in the Heatington application represents a single piece of data. Each instance of this class indicates the amount of heat used and the cost of electricity at a particular time.

## Structure

### `DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)`

To create a new data point, we have a constructor that accepts four `string` parameters.

- `startTime` and `endTime`: These represent the start and end of a time frame. They must follow the "M/d/yy H:mm" pattern. To keep things standardized, we use `DateTime.ParseExact` and `CultureInfo.InvariantCulture`, which convert the strings into `DateTime` objects.

- `heatDemand` and `electricityPrice`: These values tell us how much heat was used and how much the electricity cost at that time. They are converted into `double` data types using `double.Parse` and `CultureInfo.InvariantCulture`.

## Properties

### `StartTime`

This `DateTime` property marks the start of a data point's time frame.

### `EndTime`

This `DateTime` property marks the end of a time frame for the data point.

### `HeatDemand`

This `double` property shows the amount of heat used in the given time frame.

### `ElectricityPrice`

This `double` reflects the cost of electricity during that time period.

## Future Considerations

Adding a factory method to this class would allow us to manage specific object creation scenarios better, like parameter validation or offering more ways to create a `DataPoint`.

## Note
We are not sure about the formatting of the HeatDemand and ElectricityPrice. Because of the different cultures, we might
need to consider different decimal separators. We will need to test this in the future.
