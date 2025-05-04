using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class SectorRepository(AppDbContext context)
{
    public async Task<List<SectorEntity>> GetAllSectorsAsync()
    {
        return await context.Sectors.Include(s => s.Children).ToListAsync();
    }
}