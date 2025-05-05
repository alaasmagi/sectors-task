using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class PersonRepository(AppDbContext context)
{
    public async Task<PersonEntity?> GetPersonById(int id)
    {
        return await context.Persons.FindAsync(id) ?? null;
    }

    public async Task<bool> DoesPersonExistByName(string fullName)
    {
        return await context.Persons.AnyAsync(p => p.FullName == fullName);
    }
    
    public async Task<bool> DoesPersonExistById(int id)
    {
        return await context.Persons.AnyAsync(p => p.Id == id);
    }
    
    public async Task<int> InsertPerson(PersonEntity person)
    {
        person.CreatedAt = DateTime.Now.ToUniversalTime();
        person.UpdatedAt = DateTime.Now.ToUniversalTime();
        await context.Persons.AddAsync(person);
        
        if (await context.SaveChangesAsync() == 0)
        {
            return 0;
        }
        
        return person.Id;
    }

    public async Task<int> UpdatePerson(int personId, PersonEntity newPersonData)
    {
        var existingPerson = await context.Persons.FindAsync(personId);

        if (existingPerson == null)
        {
            return 0;
        }
        
        existingPerson.FullName = newPersonData.FullName;
        existingPerson.SectorId = newPersonData.SectorId;
        existingPerson.Agreement = newPersonData.Agreement;
        existingPerson.UpdatedBy = newPersonData.UpdatedBy;
        existingPerson.UpdatedAt = DateTime.Now.ToUniversalTime();
        
        await context.SaveChangesAsync();
        return existingPerson.Id;  
    }
    
    public async Task<bool> DeletePerson(int personId)
    {
        var existingPerson = await context.Persons.FindAsync(personId);

        if (existingPerson == null)
        {
            return false;
        }

        existingPerson.Deleted = true;
        return await context.SaveChangesAsync() > 0;  
    }
}