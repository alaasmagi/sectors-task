using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class SectorRepository(AppDbContext context)
{
    public async Task<List<SectorDto>> GetAllSectors()
    {
        var query = context.Sectors
            .AsQueryable();

        var sectors = await query
            .AsNoTracking()
            .ToListAsync();

        var lookup = sectors.ToLookup(s => s.ParentId);

        List<SectorDto> BuildHierarchy(int? parentId)
        {
            return lookup[parentId]
                .Select(s => new SectorDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Children = BuildHierarchy(s.Id)
                })
                .ToList();
        }

        return BuildHierarchy(null);
    }

    public async Task<bool> DoesSectorExist(int id)
    {
        return await context.Sectors.AnyAsync(s => s.Id == id);
    }
}