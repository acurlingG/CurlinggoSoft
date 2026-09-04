// ============================================================
// Agregar este método al final de TecnicoController.cs
// ANTES del cierre de la clase (antes del último })
// ============================================================

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
            if (id != modelo.DisponibilidadID) return NotFound();

            if (modelo.TecnicoID != TecnicoId)
                return Unauthorized();

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
                    if (!Exists(modelo.DisponibilidadID))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(MiDisponibilidad));
            }
            return View(modelo);
        }

        private bool Exists(long id) => _context.DisponibilidadTecnico.Any(e => e.DisponibilidadID == id);
