using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

public class CategoriasServicioController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriasServicioController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CATEGORIASSERVICIO
    public async Task<IActionResult> Index()
    {
        return View(await _context.CategoriasServicio.ToListAsync());
    }

    // GET: CATEGORIASSERVICIO/Details/5
    public async Task<IActionResult> Details(int? categoriaid)
    {
        if (categoriaid == null)
        {
            return NotFound();
        }

        var categoria = await _context.CategoriasServicio
            .FirstOrDefaultAsync(m => m.CategoriaID == categoriaid);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // GET: CATEGORIASSERVICIO/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CATEGORIASSERVICIO/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CategoriaID,NombreCategoria,Descripcion,Activa")] CategoriaServicio categoria)
    {
        if (ModelState.IsValid)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    // GET: CATEGORIASSERVICIO/Edit/5
    public async Task<IActionResult> Edit(int? categoriaid)
    {
        if (categoriaid == null)
        {
            return NotFound();
        }

        var categoria = await _context.CategoriasServicio.FindAsync(categoriaid);
        if (categoria == null)
        {
            return NotFound();
        }
        return View(categoria);
    }

    // POST: CATEGORIASSERVICIO/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? categoriaid, [Bind("CategoriaID,NombreCategoria,Descripcion,Activa")] CategoriaServicio categoria)
    {
        if (categoriaid != categoria.CategoriaID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria.CategoriaID))
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
        return View(categoria);
    }

    // GET: CATEGORIASSERVICIO/Delete/5
    public async Task<IActionResult> Delete(int? categoriaid)
    {
        if (categoriaid == null)
        {
            return NotFound();
        }

        var categoria = await _context.CategoriasServicio
            .FirstOrDefaultAsync(m => m.CategoriaID == categoriaid);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // POST: CATEGORIASSERVICIO/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? categoriaid)
    {
        var categoria = await _context.CategoriasServicio.FindAsync(categoriaid);
        if (categoria != null)
        {
            _context.CategoriasServicio.Remove(categoria);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(int? categoriaid)
    {
        return _context.CategoriasServicio.Any(e => e.CategoriaID == categoriaid);
    }
}
