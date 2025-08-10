using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class PersonRepository(AppDbContext context)
{
    public async Task<PersonEntity?> GetPersonByExternalId(Guid externalId)
    {
        return await context.Persons
            .FirstOrDefaultAsync(p => p.ExternalId == externalId) ?? null;
    }

    public async Task<bool> DoesPersonExistByName(string fullName)
    {
        return await context.Persons
            .AnyAsync(p => p.FullName.Trim().ToLower() == fullName.Trim().ToLower());    }
    
    public async Task<bool> DoesPersonExistByExternalId(Guid externalId)
    {
        return await context.Persons.AnyAsync(p => p.ExternalId == externalId);
    }
    
    public async Task<Guid?> InsertPerson(PersonEntity person)
    {
        person.CreatedAt = DateTime.Now.ToUniversalTime();
        person.UpdatedAt = DateTime.Now.ToUniversalTime();
        await context.Persons.AddAsync(person);
        
        if (await context.SaveChangesAsync() == 0)
        {
            return null;
        }
        
        return person.ExternalId;
    }

    public async Task<Guid?> UpdatePerson(Guid externalId, PersonEntity newPersonData)
    {
        var existingPerson = await GetPersonByExternalId(externalId);

        if (existingPerson == null)
        {
            return null;
        }
        
        existingPerson.FullName = newPersonData.FullName;
        existingPerson.SectorId = newPersonData.SectorId;
        existingPerson.Agreement = newPersonData.Agreement;
        existingPerson.UpdatedBy = newPersonData.UpdatedBy;
        existingPerson.UpdatedAt = DateTime.Now.ToUniversalTime();
        
        await context.SaveChangesAsync();
        return existingPerson.ExternalId;  
    }
    
    public async Task<bool> DeletePerson(Guid externalId)
    {
        var existingPerson = await GetPersonByExternalId(externalId);

        if (existingPerson == null)
        {
            return false;
        }

        existingPerson.Deleted = true;
        return await context.SaveChangesAsync() > 0;  
    }
}