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
            switch (season.ToLower())
            {
                case "winter":              //http://localhost:5165/api/TimeSeriesData?season=winter
                    return SourceDataManagerModel.SDM_Winter.TimeSeriesData;

                case "summer":              //http://localhost:5165/api/TimeSeriesData?season=summer
                    return SourceDataManagerModel.SDM_Summer.TimeSeriesData;

                case "winter-real":         //http://localhost:5165/api/TimeSeriesData?season=winter-real
                    return SourceDataManagerModel.SDM_WinterReal.TimeSeriesData;

                case "summer-real":         //http://localhost:5165/api/TimeSeriesData?season=summer-real
                    return SourceDataManagerModel.SDM_SummerReal.TimeSeriesData;

                default:
                    return BadRequest("Invalid season.");
            }
        }
    }
}
