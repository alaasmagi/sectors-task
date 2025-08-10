using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class PersonController : BaseController
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            var appDbContext = _context.Persons.IgnoreQueryFilters().Include(p => p.Sector);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id == null)
            {
                return NotFound();
            }

            var personEntity = await _context.Persons
                .IgnoreQueryFilters()
                .Include(p => p.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personEntity == null)
            {
                return NotFound();
            }

            return View(personEntity);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Id");
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExternalId,FullName,SectorId,Agreement,Id,CreatedBy,UpdatedBy,Deleted")] PersonEntity personEntity)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (ModelState.IsValid)
            {
                personEntity.CreatedAt = DateTime.Now.ToUniversalTime();
                personEntity.UpdatedAt = DateTime.Now.ToUniversalTime();

                _context.Add(personEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Id", personEntity.SectorId);
            return View(personEntity);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id == null)
            {
                return NotFound();
            }

            var personEntity = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
            if (personEntity == null)
            {
                return NotFound();
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Id", personEntity.SectorId);
            return View(personEntity);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExternalId,FullName,SectorId,Agreement,Id,CreatedBy,UpdatedBy,Deleted")] PersonEntity personEntity)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id != personEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    personEntity.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _context.Update(personEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonEntityExists(personEntity.Id))
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
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Id", personEntity.SectorId);
            return View(personEntity);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            if (id == null)
            {
                return NotFound();
            }

            var personEntity = await _context.Persons
                .IgnoreQueryFilters()
                .Include(p => p.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personEntity == null)
            {
                return NotFound();
            }

            return View(personEntity);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsTokenValidAsync(HttpContext))
                return Unauthorized("You cannot access admin panel without logging in!");
            
            var personEntity = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
            if (personEntity != null)
            {
                _context.Persons.Remove(personEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonEntityExists(int id)
        {
            return _context.Persons.IgnoreQueryFilters().Any(e => e.Id == id);
        }
    }
}
