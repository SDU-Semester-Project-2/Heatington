using Heatington.Microservice.OPT.Services;
using Heatington.Models;

namespace Heatington.Microservice.OPT;

public class OptimizerModel
{
    public static Optimizer.OPT? opt;

    public static async Task LoadOptimizer()
    {
        // production units
        List<ProductionUnit> productionUnits = await DependenciesService.GetProductionUnits();

        // data points
        List<DataPoint>? dataPoints = await DependenciesService.GetDataPoints();

        // Optimizer
        opt = new Optimizer.OPT(productionUnits, dataPoints);
    }
}
