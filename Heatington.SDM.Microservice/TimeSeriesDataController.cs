using Microsoft.AspNetCore.Mvc;
using Heatington.Models;

namespace SourceDataManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSeriesDataController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<DataPoint>> Get()
        {
            return SourceDataManagerModel.SDM.TimeSeriesData;
        }
    }
}
