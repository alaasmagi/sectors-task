using DAL;
using Domain;

namespace BLL;

public class PersonService(AppDbContext context)
{
    private readonly PersonRepository _personRepository = new PersonRepository(context);
    private readonly SectorRepository _sectorRepository = new SectorRepository(context);
    
    public async Task<List<SectorDto>> GetAllSectorsAsync()
    {
        return await _sectorRepository.GetAllSectors();
    }
    
    public async Task<PersonDto?> GetPersonByExternalIdAsync(Guid externalId)
    {
        var person = await _personRepository.GetPersonByExternalId(externalId);
        
        if (person == null)
        {
            return null;
        }
        
        return new PersonDto(person);
    }
    
    public async Task<Guid?> AddPersonToDbAsync(PersonEntity person)
    {
        var doesPersonExist = await _personRepository.DoesPersonExistByName(person.FullName);
        var doesSectorExist = await _sectorRepository.DoesSectorExist(person.SectorId);
        
        if (doesPersonExist || !doesSectorExist)
        {
            return null;
        }
        
        return await _personRepository.InsertPerson(person);
    }
   
    public async Task<Guid?> UpdatePersonAsync(Guid externalId, PersonEntity newPersonData)
    {
        var doesPersonExist = await _personRepository.DoesPersonExistByExternalId(externalId);
        var doesSectorExist = await _sectorRepository.DoesSectorExist(newPersonData.SectorId);
        
        if (!doesPersonExist || !doesSectorExist)
        {
            return null;
        }
        
        return await _personRepository.UpdatePerson(externalId, newPersonData);
    }
    
    public async Task<bool> DeletePersonAsync(Guid externalId)
    {
        return await _personRepository.DeletePerson(externalId);
    }
    
}