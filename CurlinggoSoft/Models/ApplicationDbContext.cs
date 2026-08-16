using Microsoft.EntityFrameworkCore;

namespace CurlinggoSoft.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Canton> Cantones { get; set; }
        public DbSet<Distrito> Distritos { get; set; }
        public DbSet<CategoriaServicio> CategoriasServicio { get; set; }
        public DbSet<SubcategoriaServicio> SubcategoriasServicio { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }
        public DbSet<EstadoReserva> EstadosReserva { get; set; }
        public DbSet<EstadoPago> EstadosPago { get; set; }
        public DbSet<TipoEvaluacion> TiposEvaluacion { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ClientePerfil> ClientesPerfil { get; set; }
        public DbSet<TecnicoPerfil> TecnicosPerfil { get; set; }
        public DbSet<DireccionCliente> DireccionesCliente { get; set; }
        public DbSet<DisponibilidadTecnico> DisponibilidadTecnico { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<MenuPermiso> MenuPermisos { get; set; }
        public DbSet<TecnicoEspecialidad> TecnicoEspecialidades { get; set; }
        public DbSet<PreguntaServicio> PreguntasServicio { get; set; }
        public DbSet<OpcionPregunta> OpcionesPregunta { get; set; }
        public DbSet<EstadoOfertaTecnico> EstadosOfertaTecnico { get; set; }
        public DbSet<SolicitudReserva> SolicitudesReserva { get; set; }
        public DbSet<HistorialEstadoReserva> HistorialEstadosReserva { get; set; }
        public DbSet<OfertaTecnico> OfertasTecnico { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<RespuestaReserva> RespuestasReserva { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<IntentoPago> IntentosPago { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }
        public DbSet<LogIntervencionOperativa> LogsIntervencionOperativa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuPermiso>().HasKey(mp => new { mp.MenuID, mp.PermisoID });
            modelBuilder.Entity<TecnicoEspecialidad>().HasKey(te => new { te.TecnicoID, te.ServicioID });

            modelBuilder.Entity<SolicitudReserva>()
                .HasOne(r => r.Cliente).WithMany().HasForeignKey(r => r.ClienteID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SolicitudReserva>()
                .HasOne(r => r.Tecnico).WithMany().HasForeignKey(r => r.TecnicoID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SolicitudReserva>()
                .HasOne(r => r.Servicio).WithMany().HasForeignKey(r => r.ServicioID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SolicitudReserva>()
                .HasOne(r => r.EstadoReserva).WithMany().HasForeignKey(r => r.EstadoReservaID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistorialEstadoReserva>()
                .HasOne(h => h.Reserva).WithMany().HasForeignKey(h => h.ReservaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HistorialEstadoReserva>()
                .HasOne(h => h.EstadoAnterior).WithMany().HasForeignKey(h => h.EstadoAnteriorID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HistorialEstadoReserva>()
                .HasOne(h => h.EstadoNuevo).WithMany().HasForeignKey(h => h.EstadoNuevoID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HistorialEstadoReserva>()
                .HasOne(h => h.UsuarioModificador).WithMany().HasForeignKey(h => h.UsuarioModificadorID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TecnicoEspecialidad>()
                .HasOne(te => te.Tecnico).WithMany().HasForeignKey(te => te.TecnicoID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TecnicoEspecialidad>()
                .HasOne(te => te.Servicio).WithMany().HasForeignKey(te => te.ServicioID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PreguntaServicio>()
                .HasOne(p => p.Servicio).WithMany().HasForeignKey(p => p.ServicioID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OpcionPregunta>()
                .HasOne(o => o.Pregunta).WithMany().HasForeignKey(o => o.PreguntaServicioID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientePerfil>()
                .HasOne(c => c.Provincia).WithMany().HasForeignKey(c => c.ProvinciaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClientePerfil>()
                .HasOne(c => c.Canton).WithMany().HasForeignKey(c => c.CantonID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClientePerfil>()
                .HasOne(c => c.Distrito).WithMany().HasForeignKey(c => c.DistritoID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TecnicoPerfil>()
                .HasOne(t => t.ProvinciaCobertura).WithMany().HasForeignKey(t => t.ProvinciaCoberturaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TecnicoPerfil>()
                .HasOne(t => t.CantonCobertura).WithMany().HasForeignKey(t => t.CantonCoberturaID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DireccionCliente>()
                .HasOne(d => d.Cliente).WithMany().HasForeignKey(d => d.ClienteID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DireccionCliente>()
                .HasOne(d => d.Provincia).WithMany().HasForeignKey(d => d.ProvinciaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DireccionCliente>()
                .HasOne(d => d.Canton).WithMany().HasForeignKey(d => d.CantonID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DireccionCliente>()
                .HasOne(d => d.Distrito).WithMany().HasForeignKey(d => d.DistritoID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DisponibilidadTecnico>()
                .HasOne(d => d.Tecnico).WithMany().HasForeignKey(d => d.TecnicoID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Menu>()
                .HasOne(m => m.MenuPadre).WithMany().HasForeignKey(m => m.MenuPadreID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuPermiso>()
                .HasOne(mp => mp.Menu).WithMany().HasForeignKey(mp => mp.MenuID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MenuPermiso>()
                .HasOne(mp => mp.Permiso).WithMany().HasForeignKey(mp => mp.PermisoID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfertaTecnico>()
                .HasOne(o => o.Reserva).WithMany().HasForeignKey(o => o.ReservaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OfertaTecnico>()
                .HasOne(o => o.Tecnico).WithMany().HasForeignKey(o => o.TecnicoID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OfertaTecnico>()
                .HasOne(o => o.EstadoOferta).WithMany().HasForeignKey(o => o.EstadoOfertaID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Usuario).WithMany().HasForeignKey(n => n.UsuarioID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Reserva).WithMany().HasForeignKey(n => n.ReservaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.OfertaTecnico).WithMany().HasForeignKey(n => n.OfertaTecnicoID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RespuestaReserva>()
                .HasOne(r => r.Reserva).WithMany().HasForeignKey(r => r.ReservaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RespuestaReserva>()
                .HasOne(r => r.Pregunta).WithMany().HasForeignKey(r => r.PreguntaServicioID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RespuestaReserva>()
                .HasOne(r => r.Opcion).WithMany().HasForeignKey(r => r.OpcionPreguntaID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Reserva).WithMany().HasForeignKey(p => p.ReservaID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IntentoPago>()
                .HasOne(i => i.Pago).WithMany().HasForeignKey(i => i.PagoID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<IntentoPago>()
                .HasOne(i => i.MetodoPago).WithMany().HasForeignKey(i => i.MetodoPagoID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<IntentoPago>()
                .HasOne(i => i.EstadoPago).WithMany().HasForeignKey(i => i.EstadoPagoID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Reserva).WithMany().HasForeignKey(e => e.ReservaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Evaluador).WithMany().HasForeignKey(e => e.EvaluadorUsuarioID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Evaluado).WithMany().HasForeignKey(e => e.EvaluadoUsuarioID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Servicio).WithMany().HasForeignKey(e => e.ServicioID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.TipoEvaluacion).WithMany().HasForeignKey(e => e.TipoEvaluacionID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.Usuario).WithMany().HasForeignKey(a => a.UsuarioID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LogIntervencionOperativa>()
                .HasOne(l => l.Reserva).WithMany().HasForeignKey(l => l.ReservaID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LogIntervencionOperativa>()
                .HasOne(l => l.UsuarioIntervencion).WithMany().HasForeignKey(l => l.UsuarioIntervencionID).OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones si es necesario
            modelBuilder.Entity<Canton>()
                .HasOne(c => c.Provincia)
                .WithMany()
                .HasForeignKey(c => c.ProvinciaID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Distrito>()
                .HasOne(d => d.Canton)
                .WithMany()
                .HasForeignKey(d => d.CantonID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubcategoriaServicio>()
                .HasOne(s => s.Categoria)
                .WithMany(c => c.Subcategorias)
                .HasForeignKey(s => s.CategoriaID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Servicio>()
                .HasOne(s => s.Categoria)
                .WithMany(c => c.Servicios)
                .HasForeignKey(s => s.CategoriaID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Servicio>()
                .HasOne(s => s.Subcategoria)
                .WithMany(sc => sc.Servicios)
                .HasForeignKey(s => s.SubcategoriaID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
