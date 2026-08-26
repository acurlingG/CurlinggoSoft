using Microsoft.AspNetCore.Identity;

namespace CurlinggoSoft.Models
{
    /// <summary>
    /// Clase encargada de inicializar datos base de la aplicación,
    /// como los roles requeridos por el sistema de autenticación
    /// y el usuario administrador inicial.
    /// </summary>
    public static class DbInitializer
    {
        private static readonly string[] Roles = { "Admin", "Cliente", "Tecnico" };

        private const string AdminEmail = "admin@curlinggosoft.com";
        private const string AdminUserName = "admin";
        private const string AdminPassword = "Admin1";

        /// <summary>
        /// Garantiza que los roles del sistema existan en AspNetRoles.
        /// Debe invocarse una vez al arrancar la aplicación.
        /// </summary>
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var roleName in Roles)
            {
                var existe = await roleManager.RoleExistsAsync(roleName);
                if (!existe)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        /// <summary>
        /// Garantiza que exista un usuario administrador (usuario: admin, clave: Admin1)
        /// con el rol "Admin" asignado, tanto en AspNetUsers como en la tabla de negocio Usuarios.
        /// </summary>
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var adminUser = await userManager.FindByNameAsync(AdminUserName)
                            ?? await userManager.FindByEmailAsync(AdminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = AdminUserName,
                    Email = AdminEmail,
                    EmailConfirmed = true,
                    TwoFactorEnabled = true
                };

                var createResult = await userManager.CreateAsync(adminUser, AdminPassword);
                if (!createResult.Succeeded)
                {
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var usuarioNegocio = await dbContext.Usuarios.FindAsync(adminUser.Id);
            if (usuarioNegocio == null)
            {
                dbContext.Usuarios.Add(new Usuario
                {
                    UsuarioID = adminUser.Id,
                    Email = AdminEmail,
                    Nombre = "Administrador",
                    Apellidos = "Sistema",
                    EstadoUsuario = "Activo",
                    FechaCreacion = DateTime.Now
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}