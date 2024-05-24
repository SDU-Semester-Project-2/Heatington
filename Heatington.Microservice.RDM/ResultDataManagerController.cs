using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.RDM;

[Route("api/[controller]")]
[ApiController]
public class ResultDataManagerController : ControllerBase
{
    /// <summary>
    /// To Test <a href="http://localhost:5143/swagger/index.html">Swagger</a>
    /// Get Result <a href="http://localhost:5143/api/resultdatamanager">Results</a>
    /// </summary>
    /// <returns>List of <see cref="FormatedResultHolder">FormatedResultHolder</see> </returns>
    [HttpGet]
    public ActionResult<IEnumerable<FormatedResultHolder>> GetFormatedResults(
        [FromQuery] int mode = 2,
        [FromQuery] string season = "summer"
    )
    {
        // optimizer uri
        string uri = ResultDataManagerService.GenerateOptimizerUri(mode, season);

        // load the RDM and call optimzier
        ResultDataManagerModel.LoadResultDataManager(uri);
        return Ok(ResultDataManagerModel.Rdm?.FormatResults());
    }
}
