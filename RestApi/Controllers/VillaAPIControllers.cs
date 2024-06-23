using AutoMapper;
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
    private readonly IMapper _mapper;

    public VillaApiControllers(ILogging logger, ApplicationDbContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    [HttpGet("villas")]
    public async Task<ActionResult<List<VillaDTO>>> GetVillas()
    {
        IEnumerable<VillaDTO> villas = await _db.Villa.ToListAsync();
        // return Ok(villas);
        // return Ok(VillaStore.villaStore());

        return Ok(_mapper.Map<List<VillaResponse>>(villas));
    }


    [HttpGet("getById/{id}", Name = "GetById")]
    public async Task<ActionResult<VillaDTO>> GetVillaById(string id)
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
        var foundVilla = await _db.Villa.FirstOrDefaultAsync(v => v.Id == id);

        if (foundVilla != null) return Ok(_mapper.Map<VillaResponse>(foundVilla));

        return NotFound();
    }


    [HttpPost("create")]
    public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villa)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        // var isExistingname = VillaStore.villaStore()
        //                          .FirstOrDefault(v => v.Name.ToLower() == villa.Name)
        //                      == null;

        var isAvailableName = await _db.Villa.FirstOrDefaultAsync(v => v.Name == villa.Name) == null;

        if (!isAvailableName)
        {
            ModelState.AddModelError("CustomerError", "Villa name already existed");

            return BadRequest(ModelState);
        }


        if (villa == null) return BadRequest("villa is empty");

        // if (Convert.ToInt32(villa.Id) < 0) return StatusCode(StatusCodes.Status403Forbidden);

        // var currentMaxId = VillaStore.villaStore().OrderByDescending(v => v.Id).First().Id;
        // var newId = Convert.ToString(Convert.ToInt32(currentMaxId) + 1);

        // VillaStore.villaStore().Add(villa);

        VillaDTO createVilla = new()
        {
            Name = villa.Name,
            Age = villa.Age
        };

        await _db.Villa.AddAsync(createVilla);
        await _db.SaveChangesAsync();


        // return Ok(villa);
        // change will appear in the header
        return CreatedAtRoute("GetById", new { createVilla.Id }, villa);
    }


    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteVillaById(string id)
    {
        var villas = VillaStore.villaStore();

        // var foundVilla = VillaStore.villaStore().FirstOrDefault(v => v.Id == id);
        var foundVilla = await _db.Villa.FirstOrDefaultAsync(v => v.Id == id);

        if (foundVilla is not null)
        {
            // VillaStore.villaStore().Remove(foundVilla);
            _db.Villa.Remove(foundVilla);
            await _db.SaveChangesAsync();
        }


        return NoContent();


        return Ok("Deleted successfully");
    }


    [HttpPut("updateById/{id}")]
    public async Task<IActionResult> UpdateVillaById(string id, [FromBody] VillaUpdateDto villa)
    {
        if (string.IsNullOrEmpty(id))
        {
            ModelState.AddModelError("CustomError", "Param id should not be empty");
            return BadRequest(ModelState);
        }

        // var list = VillaStore.villaStore();
        // var foundVilla = list.FirstOrDefault(v => v.Id == id);
        // if (foundVilla == null) return NotFound();
        // foundVilla.Name = villa.Name;

        var foundVilla = await _db.Villa.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        if (foundVilla == null) return NotFound();

        // convert
        VillaDTO updatedVillaDto = new()
        {
            Id = id,
            Name = villa.Name,
            Age = villa.Age
        };

        _db.Villa.Update(updatedVillaDto);
        await _db.SaveChangesAsync();


        // foreach (var villaq in list) Console.WriteLine("{0}", villaq.Name);
        return NoContent();
    }


    [HttpPatch("patchById/{id}")]
    public async Task<IActionResult> PatchDataById(string id, [FromBody] JsonPatchDocument<VillaDTO> villaJson)
    {
        Console.WriteLine("{0}", villaJson.Operations);
        if (id == null)
        {
            ModelState.AddModelError(ModelError.CUSTOM_ERROR, "Id is invalid");
            return BadRequest(ModelState);
        }

        var foundVilla = await _db.Villa.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
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
        await _db.SaveChangesAsync();


        return Ok();
    }
}