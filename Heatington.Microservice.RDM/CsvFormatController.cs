using Heatington.Models;
using Heatington.ResultDataManager;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.RDM;

[Route("api/[controller]")]
[ApiController]
public class CsvFormatController : ControllerBase
{
    [HttpGet]
    public ActionResult<RdmCsvResponse> GetCsvFormatedResults(
        [FromQuery] int mode = 2,
        [FromQuery] string season = "summer"
    )
    {
        // optimizer uri
        string uri = ResultDataManagerService.GenerateOptimizerUri(mode, season);

        // load the RDM and call optimzier
        ResultDataManagerModel.LoadResultDataManager(uri);
        List<FormatedResultHolder>? results = ResultDataManagerModel.Rdm?.FormatResults();
        string csvData = ResultDataManagerService.FormatToCsv(results!);


        RdmCsvResponse response = new RdmCsvResponse() { result = csvData };

        return Ok(response);
    }
}
