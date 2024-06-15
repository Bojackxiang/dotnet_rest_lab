using Microsoft.AspNetCore.Mvc;
using RestApi.data;
using RestApi.Models;

namespace RestApi.Controllers;

[ApiController]
[Route("api/villa-api")]
public class VillaApiControllers : ControllerBase
{
    [HttpGet("villas")]
    public ActionResult<List<VillaDTO>> GetVillas()
    {
        return Ok(VillaStore.villaStore());
    }

    [HttpGet("getById/{id}")]
    public ActionResult<VillaDTO> GetVillaById(string id)
    {
        // if (!int.TryParse(id, out int parsedId))
        // {
        //     return BadRequest("Invalid id");
        // }

        // validate
        try
        {
            int result = Convert.ToInt32(id);
            Console.WriteLine("{0}", result);
        }
        catch (Exception e)
        {
            return BadRequest("Invalid parameter: "+ e.Message);
        }
        
        var foundVilla = VillaStore.villaStore().Find(v => v.Id == id);

        if (foundVilla != null)
        {
            return Ok(foundVilla);
        }

        return NotFound();
        
    }

    
}