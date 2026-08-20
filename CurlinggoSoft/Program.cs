using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar el DbContext en los servicios del proyecto
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));



// Registrar el motor de asignación
builder.Services.AddScoped<CurlinggoSoft.Services.IDispatchEngineService, CurlinggoSoft.Services.DispatchEngineService>();

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

// Session: se necesita el cache en memoria + el registro de AddSession.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

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

// Seeding inicial de roles y usuario administrador al arrancar la aplicación
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedRolesAsync(scope.ServiceProvider);
    await DbInitializer.SeedAdminUserAsync(scope.ServiceProvider);
}


app.Run();