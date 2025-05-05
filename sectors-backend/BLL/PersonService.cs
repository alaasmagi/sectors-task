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
    
    public async Task<PersonEntity?> GetPersonByIdAsync(int personId)
    {
        return await _personRepository.GetPersonById(personId);
    }
    
    public async Task<int?> AddPersonToDbAsync(PersonEntity person)
    {
        var doesPersonExist = await _personRepository.DoesPersonExistByName(person.FullName);

        if (doesPersonExist)
        {
            return 0;
        }
        
        return await _personRepository.InsertPerson(person);
    }
   
    public async Task<int> UpdatePersonAsync(int personId, PersonEntity newPersonData)
    {
        return await _personRepository.UpdatePerson(personId, newPersonData);
    }
    
    public async Task<bool> DeletePersonAsync(int personId)
    {
        return await _personRepository.DeletePerson(personId);
    }
    
}