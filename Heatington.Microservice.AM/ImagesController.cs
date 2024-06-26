using Heatington.AssetManager;
using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet("{imageName}")]
        public ActionResult<string> GetImage(string imageName)
        {
            try
            {
                string imagePath = Utilities.GeneratePathToFileInAssetsDirectory($"AssetManager/{imageName}");

                if (!System.IO.File.Exists(imagePath))
                    return NotFound();

                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);
                return Ok(base64String);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
