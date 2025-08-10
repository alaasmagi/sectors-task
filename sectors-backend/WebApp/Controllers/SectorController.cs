using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class SectorController : BaseController
    {
        private readonly AppDbContext _context;

        public SectorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sector
        public async Task<IActionResult> Index()
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            var appDbContext = _context.Sectors.IgnoreQueryFilters().Include(s => s.Parent);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Sector/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id == null)
            {
                return NotFound();
            }

            var sectorEntity = await _context.Sectors
                .IgnoreQueryFilters()
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectorEntity == null)
            {
                return NotFound();
            }

            return View(sectorEntity);
        }

        // GET: Sector/Create
        public IActionResult Create()
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            ViewData["ParentId"] = GetParentSelectList();
            return View();
        }

        // POST: Sector/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ParentId,Id,CreatedBy,UpdatedBy,Deleted")] SectorEntity sectorEntity)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (ModelState.IsValid)
            {
                sectorEntity.CreatedAt = DateTime.Now.ToUniversalTime();
                sectorEntity.UpdatedAt = DateTime.Now.ToUniversalTime();
                
                _context.Add(sectorEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = GetParentSelectList(sectorEntity.ParentId);
            return View(sectorEntity);
        }

        // GET: Sector/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id == null)
            {
                return NotFound();
            }

            var sectorEntity = await _context.Sectors.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == id);
            if (sectorEntity == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = GetParentSelectList(sectorEntity.ParentId);
            return View(sectorEntity);
        }

        // POST: Sector/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,ParentId,Id,CreatedBy,UpdatedBy,Deleted")] SectorEntity sectorEntity)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id != sectorEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sectorEntity.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _context.Update(sectorEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectorEntityExists(sectorEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = GetParentSelectList(sectorEntity.ParentId);
            return View(sectorEntity);
        }

        // GET: Sector/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id == null)
            {
                return NotFound();
            }

            var sectorEntity = await _context.Sectors
                .IgnoreQueryFilters()
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectorEntity == null)
            {
                return NotFound();
            }

            return View(sectorEntity);
        }

        // POST: Sector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            var sectorEntity = await _context.Sectors.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == id);
            if (sectorEntity != null)
            {
                _context.Sectors.Remove(sectorEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectorEntityExists(int id)
        {
            return _context.Sectors.IgnoreQueryFilters().Any(e => e.Id == id);
        }
        
        private List<SelectListItem> GetParentSelectList(int? selectedParentId = null)
        {
            var sectors = _context.Sectors
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Id.ToString()
                })
                .ToList();

            sectors.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "No Parent"
            });

            if (selectedParentId != null)
            {
                var selected = sectors.FirstOrDefault(s => s.Value == selectedParentId.ToString());
                if (selected != null)
                {
                    selected.Selected = true;
                }
            }
            return sectors;
        }
    }
}
