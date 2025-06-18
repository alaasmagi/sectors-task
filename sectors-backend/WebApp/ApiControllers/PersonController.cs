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
            return Ok(new { message = "Sectors not found"});
        }

        return Ok(sectors);
    }
    
    [HttpGet("{externalId}")]
    public async Task<IActionResult> GetPersonById(Guid externalId)
    {
        var person = await personService.GetPersonByExternalIdAsync(externalId);

        if (person == null)
        {
            return Ok(new { message = "Person not found"});
        }
        
        return Ok(person);
    }
    
    [HttpPost("Add")]
    public async Task<IActionResult> AddPerson([FromBody] PersonModel model)
    {
        if (ModelState.IsValid == false)
        {
            return Ok(new { message = "Invalid credentials"});
        }

        var newPerson = new PersonEntity
        {
            FullName = model.FullName,
            SectorId = model.SectorId,
            Agreement = model.Agreement,
            CreatedBy = model.Origin,
            UpdatedBy = model.Origin
        };
        
        var personExternalId = await personService.AddPersonToDbAsync(newPerson);
        if (personExternalId == null)
        {
            return Ok(new { message = "Person was not added"});
        }
        
        return Ok(personExternalId);
    }
    
    [HttpPatch("Update")]
    public async Task<IActionResult> UpdatePerson([FromBody] PersonModel model)
    {
        if (ModelState.IsValid == false || model.ExternalId == null)
        {
            return Ok(new { message = "Invalid credentials"});
        }
        
        var newPerson = new PersonEntity
        {
            FullName = model.FullName,
            SectorId = model.SectorId,
            Agreement = model.Agreement,
            CreatedBy = model.Origin,
            UpdatedBy = model.Origin
        };

        var personExternalId = await personService.UpdatePersonAsync(model.ExternalId!.Value, newPerson);
        if (personExternalId == null)
        {
            return Ok(new { message = "Person was not updated"});
        }
        
        return Ok(personExternalId);
    }
    
    [HttpDelete("{externalId}")]
    public async Task<IActionResult> DeletePerson(Guid externalId)
    {
        var status = await personService.DeletePersonAsync(externalId);
        if (!status)
        {
            return Ok(new { message = "Person was not deleted"});
        }
        
        return Ok();
    }
}