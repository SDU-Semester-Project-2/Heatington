using Microsoft.AspNetCore.Mvc;

namespace SourceDataManagerAPI
{
    public class TimeSeriesDataController
    {
        [Route("api/[controller]")]
        [ApiController]
        public class CsvController : ControllerBase
        {
            [HttpGet]
            public ContentResult Get()
            {
                var csvData = "Column1,Column2,Column3\nValue1,Value2,Value3";
                return Content(csvData, "text/plain");
            }
        }
    }
}
