using Heatington.Microservice.OPT.Services;
using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.OPT.Controllers
{
    // FOR TESTING
    // http://localhost:5019/swagger/index.html

    [Route("api/[controller]")]
    [ApiController]
    public class OptimizerController : ControllerBase
    {
        // http://localhost:5019/api/optimizer?orderBy=a&orderBy=b&orderBy=c
        // passes: string[] orderby = ["a", "b", "c"]

        [HttpGet]
        public async Task<ActionResult<List<ResultHolder>?>> Get([FromQuery] string[] orderBy)
        {
            List<ProductionUnit> productionUnits = await DependenciesService.GetProductionUnits();

            // data points
            List<DataPoint>? dataPoints = await DependenciesService.GetDataPoints();

            // Optimizer
            Optimizer.OPT opt = new Optimizer.OPT(productionUnits, dataPoints);

            try
            {
                opt?.Optimize(orderBy);
                return Ok(opt?.Results);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
