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

    [HttpPost("create")]
    public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villa)
    {
        if (villa == null)
        {
            return BadRequest("villa is empty");
        }

        if (Convert.ToInt32(villa.Id) < 0)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        string currentMaxId = VillaStore.villaStore().OrderByDescending(v => v.Id).First().Id;
        string newId = Convert.ToString(Convert.ToInt32(currentMaxId) + 1);

        villa.Id = newId;
        VillaStore.villaStore().Add(villa);
        
        return Ok(villa);
    }
}