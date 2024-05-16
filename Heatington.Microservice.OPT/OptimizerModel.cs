using Heatington.AssetManager;
using Heatington.Microservice.OPT.Services;
using Heatington.SourceDataManager;


namespace Heatington.Microservice.OPT;
public class OptimizerModel
{
    public static Optimizer.OPT? opt;

    static OptimizerModel()
    {
        LoadOptimizer().Wait();
    }

    public static async Task LoadOptimizer()
    {
        // Asset Manager
        AM am = await DependenciesService.GetAssetManager();

        // SourceDataManager
        SDM sdm = await DependenciesService.GetSourceDataManager();

        // Optimizer
        opt = new Optimizer.OPT(am, sdm);
        opt.LoadData();
    }
}
