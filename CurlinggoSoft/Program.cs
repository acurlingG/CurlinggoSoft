using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar el DbContext en los servicios del proyecto
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));



// Registrar el motor de asignación
builder.Services.AddScoped<CurlinggoSoft.Services.IDispatchEngineService, CurlinggoSoft.Services.DispatchEngineService>();

// Registro de SignalR
builder.Services.AddSignalR();

// Configuración de ASP.NET Core Identity con roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuración de la cookie de autenticación
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Login/Index";
//    options.LogoutPath = "/Login/Logout";
//    options.AccessDeniedPath = "/Login/Index";
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
//    options.SlidingExpiration = true;
//});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Redirige a la página de inicio de sesión si el usuario no está autenticado
        options.LogoutPath = "/Home/Index"; // Redirige a la página de inicio después de cerrar sesión
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Tiempo de inactividad
        options.SlidingExpiration = true;                  // Renueva si hay actividad
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        // ❗ Esto hace que la cookie NO sea persistente
        options.Cookie.MaxAge = null;
    });

builder.Services.AddAuthorization();

// --- Cultura fija de la aplicación: es-CR ---
// Sin esto, .NET usa la cultura del sistema operativo del servidor donde
// corra el proyecto en producción. Fue justo lo que causó el bug de
// "9838721,000000" — el servidor tenía configurado pt-BR (coma como
// separador decimal), así que "9.838721" (formato JS con punto) se leyó mal
// en el model binding automático de cualquier parámetro decimal.
//
// es-CR usa punto como separador decimal (igual que el formato que manda el
// navegador), así que fijarla aquí resuelve el problema de raíz para TODOS
// los decimales del proyecto (precios, montos, coordenadas), no solo los dos
// que arreglamos a mano en SolicitudServicioController. De todas formas, deja
// el parseo manual con InvariantCulture que ya está en el controlador —
// es una segunda capa de seguridad barata si algún día el servidor cambia de
// configuración regional otra vez.
var cultura = new CultureInfo("es-CR");
var opcionesLocalizacion = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new List<CultureInfo> { cultura },
    SupportedUICultures = new List<CultureInfo> { cultura }
};

// Session: se necesita el cache en memoria + el registro de AddSession.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// UseRequestLocalization debe ir de los primeros middlewares del pipeline,
// antes de UseRouting, para que la cultura ya esté fijada cuando MVC haga el
// model binding de cualquier decimal/fecha en los controladores.
app.UseRequestLocalization(opcionesLocalizacion);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

// El middleware de Session debe ir después de UseRouting y antes de
// Authentication/Authorization (y antes de cualquier controlador que use HttpContext.Session).
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Mapeo del Hub
app.MapHub<CurlinggoSoft.Hubs.NotificacionesHub>("/hubs/notificaciones");

// Seeding inicial de roles y usuario administrador al arrancar la aplicación
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedRolesAsync(scope.ServiceProvider);
    await DbInitializer.SeedAdminUserAsync(scope.ServiceProvider);
}


app.Run();