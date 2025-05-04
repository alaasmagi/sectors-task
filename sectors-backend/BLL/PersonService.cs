using DAL;
using Domain;

namespace BLL;

public class PersonService(AppDbContext context)
{
    //private readonly PersonRepository repository;
    private readonly SectorRepository _sectorRepository = new SectorRepository(context);
    public async Task<List<SectorDto>> GetAllSectors()
    {
        return await _sectorRepository.GetAllSectorsAsync();
    }
}