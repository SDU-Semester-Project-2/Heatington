using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.OPT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptimizerController : ControllerBase
    {
        // http://localhost:5019/api/optimizer?orderBy=a&orderBy=b&orderBy=c
        // passes: string[] orderby = ["a", "b", "c"]

        [HttpGet]
        public async Task<ActionResult<List<ResultHolder>?>> Get([FromQuery] string[] orderBy)
        {
            // foreach (string se in orderBy)
            // {
            //     Console.WriteLine(se);
            // }

            await OptimizerModel.LoadOptimizer();
            OptimizerModel.OPT?.Optimize(orderBy);

            return Ok(OptimizerModel.OPT?.Results);
        }
    }
}
