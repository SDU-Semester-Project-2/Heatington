// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Heatington.Models;

public class FormatedResultHolder(
    DateTime startTime,
    DateTime endTime,
    double heatDemand,
    double electricityPrice,
    ProductionUnit boiler,
    double netProductionCost)
{
    public DateTime StartTime { get; } = startTime;
    public DateTime EndTime { get; } = endTime;
    public double HeatDemand { get; } = heatDemand;
    public double ElectricityPrice { get; } = electricityPrice;
    public ProductionUnit Boilers { get; set; } = boiler;
    public double NetProductionCost { get; set; } = netProductionCost;
}
