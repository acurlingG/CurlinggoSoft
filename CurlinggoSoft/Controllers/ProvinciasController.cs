
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class ProvinciasController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProvinciasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PROVINCIAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Provincias.ToListAsync());
    }

    // GET: PROVINCIAS/Details/5
    public async Task<IActionResult> Details(int? provinciaid)
    {
        if (provinciaid == null)
        {
            return NotFound();
        }

        var provincia = await _context.Provincias
            .FirstOrDefaultAsync(m => m.ProvinciaID == provinciaid);
        if (provincia == null)
        {
            return NotFound();
        }

        return View(provincia);
    }

    // GET: PROVINCIAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PROVINCIAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProvinciaID,Nombre,CodigoDTA,Activa")] Provincia provincia)
    {
        if (ModelState.IsValid)
        {
            _context.Add(provincia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(provincia);
    }

    // GET: PROVINCIAS/Edit/5
    public async Task<IActionResult> Edit(int? provinciaid)
    {
        if (provinciaid == null)
        {
            return NotFound();
        }

        var provincia = await _context.Provincias.FindAsync(provinciaid);
        if (provincia == null)
        {
            return NotFound();
        }
        return View(provincia);
    }

    // POST: PROVINCIAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? provinciaid, [Bind("ProvinciaID,Nombre,CodigoDTA,Activa")] Provincia provincia)
    {
        if (provinciaid != provincia.ProvinciaID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(provincia);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciaExists(provincia.ProvinciaID))
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
        return View(provincia);
    }

    // GET: PROVINCIAS/Delete/5
    public async Task<IActionResult> Delete(int? provinciaid)
    {
        if (provinciaid == null)
        {
            return NotFound();
        }

        var provincia = await _context.Provincias
            .FirstOrDefaultAsync(m => m.ProvinciaID == provinciaid);
        if (provincia == null)
        {
            return NotFound();
        }

        return View(provincia);
    }

    // POST: PROVINCIAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? provinciaid)
    {
        var provincia = await _context.Provincias.FindAsync(provinciaid);
        if (provincia != null)
        {
            _context.Provincias.Remove(provincia);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProvinciaExists(int? provinciaid)
    {
        return _context.Provincias.Any(e => e.ProvinciaID == provinciaid);
    }
}
