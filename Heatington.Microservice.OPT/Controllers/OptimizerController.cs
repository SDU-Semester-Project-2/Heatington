<<<<<<< HEAD
using Heatington.Microservice.OPT.Services;
=======
>>>>>>> c7d5462 (SP2-160-initialized OPT microservice and fixed OPT merge errors)
using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.OPT.Controllers
{
<<<<<<< HEAD
    // FOR TESTING
    // http://localhost:5019/swagger/index.html

=======
>>>>>>> c7d5462 (SP2-160-initialized OPT microservice and fixed OPT merge errors)
    [Route("api/[controller]")]
    [ApiController]
    public class OptimizerController : ControllerBase
    {
<<<<<<< HEAD
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
=======
        // pass string[] orderby = ["a", "b", "c"]
        // http://localhost:5019/api/optimizer?orderBy=a&orderBy=b&orderBy=c

        [HttpGet]
        public ActionResult<List<ResultHolder>?> Get([FromQuery] string[] orderBy)
        {
            // foreach (string se in orderBy)
            // {
            //     Console.WriteLine(se);
            // }

            OptimizerModel.OPT.Optimize();
            return Ok(OptimizerModel.OPT.Results);
>>>>>>> c7d5462 (SP2-160-initialized OPT microservice and fixed OPT merge errors)
        }
    }
}
