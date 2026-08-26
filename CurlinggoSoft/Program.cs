using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
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

// Registrar el servicio de creación de evaluaciones (llama a usp_Evaluacion_Crear),
// compartido entre ClienteController y TecnicoController.
builder.Services.AddScoped<CurlinggoSoft.Services.EvaluacionService>();

// Registrar el servicio de correo (2FA por email + alertas de inicio de sesión).
// Antes existía la clase pero nunca se registraba aquí, así que nunca se inyectaba.
builder.Services.AddScoped<CurlinggoSoft.Services.IEmailService, CurlinggoSoft.Services.EmailService>();

// En Development, las Data Protection Keys se generan en memoria (efímeras) en
// lugar de persistirse en %APPDATA%\ASP.NET\DataProtection-Keys. Esto evita que,
// al detener y volver a correr el proyecto desde Visual Studio, las cookies de
// autenticación que ya tenía el navegador (del último cliente o técnico que
// inició sesión) sigan siendo válidas para el nuevo proceso del servidor y
// parezca que "ya quedaste conectado" con el usuario anterior sin haber hecho
// login. En Production se mantiene el comportamiento normal (persistente).
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
}

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

app.UseRequestLocalization(opcionesLocalizacion);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

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