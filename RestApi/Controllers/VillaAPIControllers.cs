using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("getById/{id}", Name = "GetById")]
    public ActionResult<VillaDTO> GetVillaById(string id)
    {
        // if (!int.TryParse(id, out int parsedId))
        // {
        //     return BadRequest("Invalid id");
        // }

        // validate
        try
        {
            var result = Convert.ToInt32(id);
            Console.WriteLine("{0}", result);
        }
        catch (Exception e)
        {
            return BadRequest("Invalid parameter: " + e.Message);
        }

        var foundVilla = VillaStore.villaStore().Find(v => v.Id == id);

        if (foundVilla != null) return Ok(foundVilla);

        return NotFound();
    }

    [HttpPost("create")]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        var isExistingname = VillaStore.villaStore()
                                 .FirstOrDefault(v => v.Name.ToLower() == villa.Name)
                             == null;

        if (isExistingname)
        {
            ModelState.AddModelError("CustomerError", "Villa name already existed");

            return BadRequest(ModelState);
        }


        if (villa == null) return BadRequest("villa is empty");

        if (Convert.ToInt32(villa.Id) < 0) return StatusCode(StatusCodes.Status403Forbidden);

        var currentMaxId = VillaStore.villaStore().OrderByDescending(v => v.Id).First().Id;
        var newId = Convert.ToString(Convert.ToInt32(currentMaxId) + 1);

        villa.Id = newId;
        VillaStore.villaStore().Add(villa);

        // return Ok(villa);
        // change will appear in the header
        return CreatedAtRoute("GetById", new { Id = newId }, villa);
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteVillaById(string id)
    {
        var villas = VillaStore.villaStore();

        var foundVilla = VillaStore.villaStore().FirstOrDefault(v => v.Id == id);

        VillaStore.villaStore().Remove(foundVilla);

        return NoContent();


        return Ok("Deleted successfully");
    }

    [HttpPut("updateById/{id}")]
    public IActionResult UpdateVillaById(string id, [FromBody] VillaDTO villa)
    {
        if (villa.Id == null || villa.Name == null)
        {
            ModelState.AddModelError("CustomError", "Input of the villa is invalid");
            return BadRequest();
        }

        var list = VillaStore.villaStore();
        var foundVilla = list.FirstOrDefault(v => v.Id == id);
        if (foundVilla == null) return NotFound();
        foundVilla.Name = villa.Name;

        foreach (var villaq in list) Console.WriteLine("{0}", villaq.Name);

        return NoContent();
    }

    [HttpPatch("patchById/{id}")]
    public IActionResult PatchDataById(string id, JsonPatchDocument<VillaDTO> patchDocument)
    {
        if (id == null)
        {
            ModelState.AddModelError(ModelError.CUSTOM_ERROR, "Id is invalid");
            return BadRequest(ModelState);
        }

        var foundVilla = VillaStore.villaStore().FirstOrDefault(v => v.Id == id);
        if (foundVilla == null) return NotFound("CustomError: villa is not found");

        patchDocument.ApplyTo(foundVilla);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok();
    }
}