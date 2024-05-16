using Heatington.AssetManager;
using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionUnitsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<ProductionUnit>> Get()
        {
            return AssetManagerModel.am.ProductionUnits!.Values.ToList();
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
            AssetManagerModel.am.WriteHeatingUnit(id, updated);

            return NoContent();
        }

        [HttpPost]
        public ActionResult PostProductionUnit(ProductionUnit newHeatingUnit)
        {
            try
            {
                AssetManagerModel.am.AddHeatingUnit(ProductionUnitsEnum.CustomBoiler, newHeatingUnit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }

            return NoContent();
        }
    }
}
