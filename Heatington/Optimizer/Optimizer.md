# Documentation for optimizer

## Scenario 1

The optimizer class has three private fields.
- `_productionUnits` holds the production units objects
- `_energyDataEntries` hold the data eventually provided by the SDM, which reads them from the CSV
- `_resultEntries` currently holds how many boilers need to be activated for any given time period

### Optimize scenario 1
`OptimzeScenario1`does what it sounds like. It optimizes the first scenario, there are only heat boilers,
no electricity has to be taken into consideration
1. After getting the production units and the time series data which holds how many Mwh are to be produced it orders
the production units by their production cost
2. For each "hour" it calls `CalculateHeatingUnitsRequired` which calculates how many heating units have to be activated
to satisfy the heating demand

**BEWARE** `CalculateHeatingUnitsRequired` returns how many heating units need to be activated. This number references
the index of the productionUnit list, meaning that 0 means only the production unit with index 0.
2 would mean all the production units with index 0, 1, 2

### Net Production cost
Since heat only boilers have no further income or expenses it useful to know what the cost of production is.
Once the `OptimzeScenario1()` has run `NetProductionCost()` can be executed.
Currently it just calculates it and displays it on screen. Once RDM is implemented it will
correctly format the data and make it available to the RDM.


### General expedients
- The class is not completed, this is only the initial implementation
- Most classes with which it works with, are going to be changed calling for major refactoring
- It tried to adhere to the SOLID principle as much as possible, refactoring should work ok
