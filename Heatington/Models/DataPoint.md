# DataPoint Class

## Overview

The `DataPoint` class encapsulates a single unit of data in the Heatington application. Each instance represents a snapshot of both the heat demand and the electricity price at a certain point in time, defined by a time interval (start time to end time).

## Constructor

### `DataPoint(string startTime, string endTime, string heatDemand, string electricityPrice)`

The constructor takes four string arguments: `startTime`, `endTime`, `heatDemand` and `electricityPrice`. These arguments represent the data for that specific point:

- `startTime` and `endTime`: These strings should represent the start and end times of the data point, formatted using the pattern "M/d/yy H:mm". They are transformed into `DateTime` instances using the `DateTime.ParseExact` method. `CultureInfo.InvariantCulture` is used here to ensure consistent date and time parsing irrespective of the culture settings of the user's environment.
- `heatDemand` and `electricityPrice`: These strings represent the heat demand and electricity prices at the given time interval. They are parsed into `double` data types, with a division by 100 applied to correct a data formatting issue.

## Properties

### `StartTime`

A `DateTime` property that represents the start of the time interval for this data point.

### `EndTime`

A `DateTime` property that represents the end of the time interval for this data point.

### `HeatDemand`

A `double` property that represents the amount of heat demanded in the corresponding time interval.

### `ElectricityPrice`

A `double` property that represents the electricity price for the corresponding time interval.

## Future Implementation

Consideration has been made for a future factory method implementation to increase abstraction and handle specific object creation scenarios such as parameter validation or support for additional constructors.
