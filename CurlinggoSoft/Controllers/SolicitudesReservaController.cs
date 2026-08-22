using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using CurlinggoSoft.Models;

public class SolicitudesReservaController : Controller
{
    private readonly ApplicationDbContext _context;
    public SolicitudesReservaController(ApplicationDbContext context) => _context = context;

    // Regla de negocio CURLINGgo: los cambios de estado de una reserva NO deben
    // hacerse con _context.Update(); deben pasar por usp_Reserva_CambiarEstado
    // para conservar la maquina de estados y el historial transaccional.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(long reservaId, int estadoNuevoId, string usuarioModificadorId, string? observaciones)
    {
        await using var connection = new SqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new SqlCommand("dbo.usp_Reserva_CambiarEstado", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@ReservaID", SqlDbType.BigInt) { Value = reservaId });
        command.Parameters.Add(new SqlParameter("@EstadoNuevoID", SqlDbType.Int) { Value = estadoNuevoId });
        command.Parameters.Add(new SqlParameter("@UsuarioModificadorID", SqlDbType.NVarChar, 450) { Value = usuarioModificadorId });
        command.Parameters.Add(new SqlParameter("@Observaciones", SqlDbType.NVarChar, 500) { Value = (object?)observaciones ?? DBNull.Value });

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Details), new { id = reservaId });
        }

        return RedirectToAction(nameof(Details), new { id = reservaId });
    }

    // GET: /SolicitudesReserva/Index?clienteId=xxx&servicioId=5&estadoId=3
    // Se agregan filtros por cliente, servicio y estado; el combo de cliente
    // muestra nombre/email (join con Usuarios) en vez del ClienteID crudo.
    public async Task<IActionResult> Index(string? clienteId, int? servicioId, int? estadoId)
    {
        var query = _context.SolicitudesReserva
            .Include(r => r.Cliente)
            .Include(r => r.Tecnico)
            .Include(r => r.Servicio)
            .Include(r => r.EstadoReserva)
            .AsQueryable();

        if (!string.IsNullOrEmpty(clienteId))
        {
            query = query.Where(r => r.ClienteID == clienteId);
        }
        if (servicioId.HasValue)
        {
            query = query.Where(r => r.ServicioID == servicioId.Value);
        }
        if (estadoId.HasValue)
        {
            query = query.Where(r => r.EstadoReservaID == estadoId.Value);
        }

        ViewBag.ClienteIDSeleccionado = clienteId;
        ViewBag.ServicioIDSeleccionado = servicioId;
        ViewBag.EstadoIDSeleccionado = estadoId;

        ViewBag.Clientes = await ObtenerListaClientesAsync(clienteId);
        ViewBag.Servicios = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            await _context.Servicios.OrderBy(s => s.NombreServicio).ToListAsync(), "ServicioID", "NombreServicio", servicioId);
        ViewBag.Estados = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            await _context.EstadosReserva.OrderBy(e => e.Nombre).ToListAsync(), "EstadoReservaID", "Nombre", estadoId);

        var resultado = await query.OrderByDescending(r => r.FechaHoraSolicitud).ToListAsync();

        // ClientePerfil no tiene navegación a Usuario, así que se arma un
        // diccionario ClienteID -> "Nombre Apellidos (email)" para la vista.
        var clienteIds = resultado.Select(r => r.ClienteID).Distinct().ToList();
        ViewBag.ClienteNombres = await _context.Usuarios
            .Where(u => clienteIds.Contains(u.UsuarioID))
            .ToDictionaryAsync(u => u.UsuarioID, u => $"{u.Nombre} {u.Apellidos} ({u.Email})");

        return View(resultado);
    }

    // Combo de clientes mostrando "Nombre Apellidos (email)" en vez de solo
    // el ClienteID, uniendo ClientesPerfil con Usuarios por el mismo Id.
    private async Task<Microsoft.AspNetCore.Mvc.Rendering.SelectList> ObtenerListaClientesAsync(string? seleccionado)
    {
        var clientes = await (
            from c in _context.ClientesPerfil
            join u in _context.Usuarios on c.ClienteID equals u.UsuarioID into gu
            from u in gu.DefaultIfEmpty()
            orderby u != null ? u.Nombre : c.ClienteID
            select new
            {
                c.ClienteID,
                Texto = u != null ? $"{u.Nombre} {u.Apellidos} ({u.Email})" : c.ClienteID
            }).ToListAsync();

        return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(clientes, "ClienteID", "Texto", seleccionado);
    }

    public async Task<IActionResult> Details(long? id) => id == null ? NotFound() : View(await _context.SolicitudesReserva.Include(r => r.Cliente).Include(r => r.Tecnico).Include(r => r.Servicio).Include(r => r.EstadoReserva).FirstOrDefaultAsync(m => m.ReservaID == id) ?? new());

    public IActionResult Create()
    {
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta");
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula");
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio");
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ReservaID,CodigoSeguimiento,ClienteID,TecnicoID,ServicioID,EstadoReservaID,DireccionID,ProvinciaID,CantonID,DistritoID,MontoBaseCotizado,MontoAjustes,MontoTotalCotizado,Moneda,DuracionEstimadaMinutos,FechaHoraProgramada,LatitudServicio,LongitudServicio,FechaHoraSolicitud,FechaHoraCompletada,DireccionServicio,DescripcionProblema,NotasCliente,FechaModificacion")] SolicitudReserva modelo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoReservaID);
        return View(modelo);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var modelo = await _context.SolicitudesReserva.FindAsync(id);
        if (modelo == null) return NotFound();
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoReservaID);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("ReservaID,CodigoSeguimiento,ClienteID,TecnicoID,ServicioID,EstadoReservaID,DireccionID,ProvinciaID,CantonID,DistritoID,MontoBaseCotizado,MontoAjustes,MontoTotalCotizado,Moneda,DuracionEstimadaMinutos,FechaHoraProgramada,LatitudServicio,LongitudServicio,FechaHoraSolicitud,FechaHoraCompletada,DireccionServicio,DescripcionProblema,NotasCliente,FechaModificacion")] SolicitudReserva modelo)
    {
        if (id != modelo.ReservaID) return NotFound();
        if (ModelState.IsValid)
        {
            try { _context.Update(modelo); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!Exists(modelo.ReservaID)) return NotFound(); throw; }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.ClientesPerfil, "ClienteID", "DireccionExacta", modelo.ClienteID);
        ViewData["TecnicoID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TecnicosPerfil, "TecnicoID", "IdentificacionCedula", modelo.TecnicoID);
        ViewData["ServicioID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Servicios, "ServicioID", "NombreServicio", modelo.ServicioID);
        ViewData["EstadoReservaID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.EstadosReserva, "EstadoReservaID", "Nombre", modelo.EstadoReservaID);
        return View(modelo);
    }

    public async Task<IActionResult> Delete(long? id) => id == null ? NotFound() : View(await _context.SolicitudesReserva.Include(r => r.Cliente).Include(r => r.Tecnico).Include(r => r.Servicio).Include(r => r.EstadoReserva).FirstOrDefaultAsync(m => m.ReservaID == id) ?? new());

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var modelo = await _context.SolicitudesReserva.FindAsync(id);
        if (modelo != null) _context.SolicitudesReserva.Remove(modelo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(long? id) => _context.SolicitudesReserva.Any(e => e.ReservaID == id);
}
