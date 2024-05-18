using Microsoft.AspNetCore.Mvc;
using Heatington.Models;

namespace SourceDataManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSeriesDataController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<DataPoint>> Get(string season)
        {
            if(season.ToLower() == "winter")        //http://localhost:5165/api/TimeSeriesData?season=winter
            {
                return SourceDataManagerModel.SDM_Winter.TimeSeriesData;
            }
            else if(season.ToLower() == "summer")   //http://localhost:5165/api/TimeSeriesData?season=summer
            {
                return SourceDataManagerModel.SDM_Summer.TimeSeriesData;
            }
            else
            {
                return BadRequest("Invalid season.");
            }
        }
    }
}
