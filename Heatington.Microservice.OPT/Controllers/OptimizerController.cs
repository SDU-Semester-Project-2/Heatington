using Heatington.Microservice.OPT.Services;
using Heatington.Models;
using Heatington.Optimizer;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.OPT.Controllers
{
    /// <summary>
    /// Endpoint for using optimizer
    /// </summary>
    /// <a href="http://localhost:5019/swagger/index.html">User interface for endpoint</a>
    [Route("api/[controller]")]
    [ApiController]
    public class OptimizerController : ControllerBase
    {
        /// <summary>
        /// Get the optimized data
        /// </summary>
        /// <a href="http://localhost:5019/api/optimizer?orderBy=a">Example usage</a>
        [HttpGet]
        public async Task<ActionResult<List<ResultHolder>?>> Get([FromQuery] string[] orderBy)
        {
            // production units
            List<ProductionUnit> productionUnits = await DependenciesService.GetProductionUnits();

            // data points
            List<DataPoint>? dataPoints = await DependenciesService.GetDataPoints();

            // Optimizer
            Optimizer.OPT opt = new Optimizer.OPT(productionUnits, dataPoints);

            try
            {
                opt.Optimize(OptimizationMode.Scenario2);
                return Ok(opt.Results);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
