using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.data;
using RestApi.enums;
using RestApi.logging;
using RestApi.Models;

namespace RestApi.Controllers;

[ApiController]
[Route("api/villa-api")]
public class VillaApiControllers : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogging _logger;

    public VillaApiControllers(ILogging logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet("villas")]
    public ActionResult<List<VillaDTO>> GetVillas()
    {
        var villas = _db.Villa.ToList();
        return Ok(villas);
        // return Ok(VillaStore.villaStore());
    }


    [HttpGet("getById/{id}", Name = "GetById")]
    public ActionResult<VillaDTO> GetVillaById(string id)
    {
        _logger.Log("testing for customized logging - error", "Error");
        _logger.Log("testing for customized logging - info", "Info");
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

        // var foundVilla = VillaStore.villaStore().Find(v => v.Id == id);
        var foundVilla = _db.Villa.FirstOrDefault(v => v.Id == id);

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
        // var isExistingname = VillaStore.villaStore()
        //                          .FirstOrDefault(v => v.Name.ToLower() == villa.Name)
        //                      == null;

        var isAvailableName = _db.Villa.FirstOrDefault(v => v.Name == villa.Name) == null;

        if (!isAvailableName)
        {
            ModelState.AddModelError("CustomerError", "Villa name already existed");

            return BadRequest(ModelState);
        }


        if (villa == null) return BadRequest("villa is empty");

        if (Convert.ToInt32(villa.Id) < 0) return StatusCode(StatusCodes.Status403Forbidden);

        // var currentMaxId = VillaStore.villaStore().OrderByDescending(v => v.Id).First().Id;
        // var newId = Convert.ToString(Convert.ToInt32(currentMaxId) + 1);

        // VillaStore.villaStore().Add(villa);

        _db.Villa.Add(villa);
        _db.SaveChanges();


        // return Ok(villa);
        // change will appear in the header
        return CreatedAtRoute("GetById", new { villa.Id }, villa);
    }


    [HttpDelete("delete/{id}")]
    public IActionResult DeleteVillaById(string id)
    {
        var villas = VillaStore.villaStore();

        // var foundVilla = VillaStore.villaStore().FirstOrDefault(v => v.Id == id);
        var foundVilla = _db.Villa.FirstOrDefault(v => v.Id == id);

        if (foundVilla is not null)
        {
            // VillaStore.villaStore().Remove(foundVilla);
            _db.Villa.Remove(foundVilla);
            _db.SaveChanges();
        }


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

        // var list = VillaStore.villaStore();
        // var foundVilla = list.FirstOrDefault(v => v.Id == id);
        // if (foundVilla == null) return NotFound();
        // foundVilla.Name = villa.Name;

        var foundVilla = _db.Villa.FirstOrDefault(v => v.Id == id);
        if (foundVilla == null) return NotFound();


        _db.Villa.Update(villa);
        _db.SaveChanges();


        // foreach (var villaq in list) Console.WriteLine("{0}", villaq.Name);
        return NoContent();
    }


    [HttpPatch("patchById/{id}")]
    public IActionResult PatchDataById(string id, [FromBody] JsonPatchDocument<VillaDTO> villaJson)
    {
        Console.WriteLine("{0}", villaJson.Operations);
        if (id == null)
        {
            ModelState.AddModelError(ModelError.CUSTOM_ERROR, "Id is invalid");
            return BadRequest(ModelState);
        }

        var foundVilla = _db.Villa.AsNoTracking().FirstOrDefault(v => v.Id == id);
        if (foundVilla == null) return NotFound("CustomError: villa is not found");

        // ..
        var villaToPatch = new VillaDTO
        {
            Id = foundVilla.Id,
            Name = foundVilla.Name,
            Age = foundVilla.Age
        };

        // .. 
        villaJson.ApplyTo(villaToPatch, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        Console.WriteLine("{0} - {1} - {2}", villaToPatch.Name, villaToPatch.Age, villaToPatch.Id);

        _db.Villa.Update(villaToPatch);
        _db.SaveChanges();


        return Ok();
    }
}