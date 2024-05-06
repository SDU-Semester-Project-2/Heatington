using Heatington.AssetManager;
using Heatington.Controllers;
using Heatington.Helpers;
using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionUnitsController : ControllerBase
    {
        public ProductionUnitsController()
        {
        }

        [HttpGet]
        public ActionResult<List<ProductionUnit>> Get()
        {
            return AssetManagerModel.AM.ProductionUnits!.Values.ToList();
        }

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

        [HttpPut("{id}")]
        public ActionResult PutProductionUnit(Guid id, ProductionUnit updated)
        {
            //The request gets a JSON that is deserialized into a ProductionUnit, but
            //since the constructor for ProductionUnit creates a new GUID they won't match.
            //Maybe overload the function with a JSONSerializer attribute?
            /*
            if (id != updated.Id)
            {
                return BadRequest();
            }
            */

            //If id is not in the list it will update the first one. I think it would be nicer
            //to throw an Exception we can catch if the Production unit with that id is not
            //present in the list.
            AssetManagerModel.AM.WriteHeatingUnit(id, updated);

            return NoContent();
        }

        [HttpPost]
        public ActionResult PostProductionUnit(ProductionUnit newHeatingUnit)
        {
            try
            {
                AssetManagerModel.AM.AddHeatingUnit(ProductionUnitsEnum.CustomBoiler, newHeatingUnit);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
