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
        /// <a href="http://localhost:5019/api/optimizer?season=winter&amp;mode=2">Example usage</a>
        /// <a href="http://localhost:5019/api/optimizer">Simplest usage with default values</a>
        /// <param name="mode">modes: 1->Scenario1, 2->Scenario2, 3->co2 </param>
        /// <param name="season">seasons: winter, summer </param>
        [HttpGet]
        public async Task<ActionResult<List<ResultHolder>?>> Get(
            [FromQuery] int mode = 2,
            [FromQuery] string season = "summer"
        )
        {
            if (!Enum.IsDefined(typeof(OptimizationMode), mode))
            {
                return StatusCode(400, "Wrong argument value: mode. Value has to be in (1,2,3) set");
            }

            // production units
            List<ProductionUnit> productionUnits = await DependenciesService.GetProductionUnits();

            // data points
            List<DataPoint>? dataPoints = await DependenciesService.GetDataPoints(season);

            //this is sad
            if (mode == (int)OptimizationMode.Scenario1)
            {
                productionUnits = productionUnits.Where(unit => unit.MaxElectricity == 0).ToList();
            }

            Console.WriteLine(productionUnits.Count);
            foreach (var boiler in (productionUnits))
            {
                Console.WriteLine(boiler);
            }


            // Optimizer
            Optimizer.OPT opt = new Optimizer.OPT(productionUnits, dataPoints);

            try
            {
                opt.Optimize((OptimizationMode)mode);
                return Ok(opt.Results);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
