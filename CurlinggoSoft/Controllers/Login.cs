using Microsoft.AspNetCore.Mvc;

namespace CurlinggoSoft.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string usuario, string clave)
        {
            // Validación básica en el servidor
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                ModelState.AddModelError("", "El usuario y la contraseña son obligatorios.");
                return View("Index");
            }

            // IMPORTANTE: Aquí debes implementar la validación real contra tu base de datos
            // Este es un ejemplo básico que siempre falla (para demostración)

            // Ejemplo: verificar contra una lista de usuarios (SOLO PARA DESARROLLO)
            // En producción, debes usar un sistema de autenticación seguro como ASP.NET Identity

            ModelState.AddModelError("", "Usuario o contraseña incorrectos. Por favor, intenta de nuevo.");
            return View("Index");
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }
    }
}
