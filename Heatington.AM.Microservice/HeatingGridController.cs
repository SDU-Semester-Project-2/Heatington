using Heatington.AssetManager;
using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeatingGridController : ControllerBase
    {
        public HeatingGridController()
        {
        }

        [HttpGet]
        public ActionResult<HeatingGrid> Get()
        {
            return AssetManagerModel.AM.HeatingGridInformation;
        }
    }
}
