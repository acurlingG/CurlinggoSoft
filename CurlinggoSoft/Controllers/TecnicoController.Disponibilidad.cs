using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurlinggoSoft.Models;

namespace CurlinggoSoft.Controllers
{
    // Extensión parcial de TecnicoController para gestión de disponibilidad propia
    public partial class TecnicoController : Controller
    {
        // GET: /Tecnico/MiDisponibilidad
        // Acción PRIVADA: Técnico solo ve SU PROPIA disponibilidad
        [HttpGet]
        public async Task<IActionResult> MiDisponibilidad()
        {
            var disponibilidadTecnico = await _context.DisponibilidadTecnico
                .Include(d => d.Tecnico)
                .Where(d => d.TecnicoID == TecnicoId)
                .OrderBy(d => d.DiaSemana)
                .ThenBy(d => d.HoraInicio)
                .ToListAsync();

            return View(disponibilidadTecnico);
        }

        // GET: /Tecnico/AgregarDisponibilidad
        [HttpGet]
        public async Task<IActionResult> AgregarDisponibilidad()
        {
            var disponibilidad = new DisponibilidadTecnico 
            { 
                TecnicoID = TecnicoId,
                Activa = true
            };

            return View(disponibilidad);
        }

        // POST: /Tecnico/AgregarDisponibilidad
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDisponibilidad(
            [Bind("TecnicoID,DiaSemana,HoraInicio,HoraFin,Activa")] DisponibilidadTecnico modelo)
        {
            // Seguridad: asegurar que el TecnicoID sea el del usuario autenticado
            modelo.TecnicoID = TecnicoId;

            // Validaciones básicas
            if (modelo.DiaSemana < 1 || modelo.DiaSemana > 7)
            {
                ModelState.AddModelError(nameof(modelo.DiaSemana), "Selecciona un día válido.");
            }

            if (modelo.HoraInicio >= modelo.HoraFin)
            {
                ModelState.AddModelError(nameof(modelo.HoraFin), "La hora de fin debe ser posterior a la de inicio.");
            }

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                _context.Add(modelo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Disponibilidad agregada correctamente.";
                return RedirectToAction(nameof(MiDisponibilidad));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al agregar: {ex.Message}");
                return View(modelo);
            }
        }

        // GET: /Tecnico/EditarMiDisponibilidad/{id}
        [HttpGet]
        public async Task<IActionResult> EditarMiDisponibilidad(long? id)
        {
            if (id == null) return NotFound();

            var disponibilidad = await _context.DisponibilidadTecnico.FindAsync(id);
            if (disponibilidad == null || disponibilidad.TecnicoID != TecnicoId)
                return Unauthorized();

            return View(disponibilidad);
        }

        // POST: /Tecnico/EditarMiDisponibilidad/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarMiDisponibilidad(long? id, 
            [Bind("DisponibilidadID,TecnicoID,DiaSemana,HoraInicio,HoraFin,Activa")] DisponibilidadTecnico modelo)
        {
            if (id != modelo.DisponibilidadID) 
                return NotFound();

            // Validación crítica de seguridad: solo puede editar la suya
            if (modelo.TecnicoID != TecnicoId)
                return Unauthorized();

            // Validaciones
            if (modelo.HoraInicio >= modelo.HoraFin)
            {
                ModelState.AddModelError(nameof(modelo.HoraFin), "La hora de fin debe ser posterior a la de inicio.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Disponibilidad actualizada correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExistsDisponibilidad(modelo.DisponibilidadID))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(MiDisponibilidad));
            }
            return View(modelo);
        }

        // POST: /Tecnico/EliminarDisponibilidad/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDisponibilidad(long? id)
        {
            if (id == null) return NotFound();

            var disponibilidad = await _context.DisponibilidadTecnico.FindAsync(id);
            if (disponibilidad == null || disponibilidad.TecnicoID != TecnicoId)
                return Unauthorized();

            try
            {
                _context.DisponibilidadTecnico.Remove(disponibilidad);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Disponibilidad eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar: {ex.Message}";
            }

            return RedirectToAction(nameof(MiDisponibilidad));
        }

        private bool ExistsDisponibilidad(long id) 
            => _context.DisponibilidadTecnico.Any(e => e.DisponibilidadID == id);
    }
}

