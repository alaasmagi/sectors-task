using BLL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(PersonService personService) : ControllerBase
{

    [HttpGet("Sectors")]
    public async Task<IActionResult> GetAllSectors()
    {
        var sectors = await personService.GetAllSectorsAsync();

        if (sectors.Count == 0)
        {
            return NotFound(new { message = "Sectors not found"});
        }

        return Ok(sectors);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPersonById(int id)
    {
        var person = await personService.GetPersonByIdAsync(id);

        if (person == null)
        {
            return NotFound(new { message = "Person not found"});
        }
        
        return Ok(person);
    }
    
    [HttpPost("Add")]
    public async Task<IActionResult> AddPerson([FromBody] PersonModel model)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(new { message = "Invalid credentials"});
        }

        var newPerson = new PersonEntity
        {
            FullName = model.FullName,
            SectorId = model.SectorId,
            Agreement = model.Agreement,
            CreatedBy = model.Origin,
            UpdatedBy = model.Origin
        };
        
        var personId = await personService.AddPersonToDbAsync(newPerson);
        if (personId == 0)
        {
            return BadRequest(new { message = "Person was not added"});
        }
        
        return Ok(personId);
    }
    
    [HttpPatch("Update")]
    public async Task<IActionResult> UpdatePerson([FromBody] PersonModel model)
    {
        if (ModelState.IsValid == false || model.PersonId <= 0)
        {
            return BadRequest(new { message = "Invalid credentials"});
        }
        
        var newPerson = new PersonEntity
        {
            FullName = model.FullName,
            SectorId = model.SectorId,
            Agreement = model.Agreement,
            CreatedBy = model.Origin,
            UpdatedBy = model.Origin
        };

        var personId = await personService.UpdatePersonAsync(model.PersonId!.Value, newPerson);
        if (personId == 0)
        {
            return BadRequest(new { message = "Person was not updated"});
        }
        
        return Ok(personId);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var status = await personService.DeletePersonAsync(id);
        if (!status)
        {
            return BadRequest(new { message = "Person was not deleted"});
        }
        
        return Ok();
    }
}