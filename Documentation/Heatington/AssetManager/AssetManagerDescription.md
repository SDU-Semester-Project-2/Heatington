# Asset Manager

The AM is a repository for static system information and is responsible for making this
information available for other modules. The AM can hold its data in local files.

Through the AM, other modules can retrieve heating grid information such as name
and a graphical representation of the grid.

The AM provides all configuration data for the production units such as name, possible
operation points and a graphical representation of the units itself.

An operation point defines how a device can be operated. Parameters are produced
heat, produced / consumed electricity, production costs, consumption of primary
energy, produced CO2 emissions. There is typically a minimum and a maximum
operation point but, in this case, we assume that there is a maximum operation point
for each unit which can be regulated to zero, meaning the device is switched off.
