using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class SectorRepository(AppDbContext context)
{
    public async Task<List<SectorDto>> GetAllSectorsAsync()
    {
        var query = context.Sectors
            .AsQueryable();

        var sectors = await query
            .AsNoTracking()
            .ToListAsync();

        var lookup = sectors.ToLookup(s => s.ParentId);

        List<SectorDto> BuildTree(int? parentId)
        {
            return lookup[parentId]
                .Select(s => new SectorDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Children = BuildTree(s.Id)
                })
                .ToList();
        }

        return BuildTree(null);
    }
}