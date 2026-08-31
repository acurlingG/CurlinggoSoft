using CURLINGgo.API.DTOs;
using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CURLINGgo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/servicios/categorias
        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _context.CategoriasServicio
                .AsNoTracking()
                .Where(c => c.Activa)
                .Select(c => new CategoriaServicioDto
                {
                    CategoriaID = c.CategoriaID,
                    NombreCategoria = c.NombreCategoria,
                    Descripcion = c.Descripcion
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // GET: api/servicios
        [HttpGet]
        public async Task<IActionResult> GetServicios([FromQuery] int? categoriaId)
        {
            var query = _context.Servicios
                .AsNoTracking()
                .Where(s => s.Activo);

            if (categoriaId.HasValue)
            {
                query = query.Where(s => s.CategoriaID == categoriaId.Value);
            }

            var servicios = await query
                .Select(s => new ServicioDto
                {
                    ServicioID = s.ServicioID,
                    NombreServicio = s.NombreServicio,
                    Descripcion = s.Descripcion,
                    TarifaDiagnosticoBase = s.TarifaDiagnosticoBase,
                    CategoriaID = s.CategoriaID,
                    NombreCategoria = s.Categoria != null ? s.Categoria.NombreCategoria : string.Empty,
                    Moneda = s.Moneda
                })
                .ToListAsync();

            return Ok(servicios);
        }
    }
}