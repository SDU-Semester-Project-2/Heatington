using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.OPT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptimizerController : ControllerBase
    {
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
        }
    }
}
