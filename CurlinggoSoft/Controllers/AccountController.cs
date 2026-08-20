using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CurlinggoSoft.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Login/Index.cshtml");
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string usuario, string clave, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                ModelState.AddModelError(string.Empty, "El usuario y la contraseña son obligatorios.");
                return View("~/Views/Login/Index.cshtml");
            }

            // Se permite iniciar sesión con correo electrónico o nombre de usuario
            var identityUser = await _userManager.FindByEmailAsync(usuario)
                                ?? await _userManager.FindByNameAsync(usuario);

            if (identityUser == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos. Por favor, intenta de nuevo.");
                return View("~/Views/Login/Index.cshtml");
            }

            var result = await _signInManager.PasswordSignInAsync(
                identityUser,
                clave,
                isPersistent: false,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Los roles asignados al usuario (Admin, Cliente, Tecnico) quedan
                // disponibles automáticamente en el ClaimsPrincipal (User.IsInRole(...))
                // gracias a AddIdentity + PasswordSignInAsync.
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (await _userManager.IsInRoleAsync(identityUser, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }

                if (await _userManager.IsInRoleAsync(identityUser, "Tecnico"))
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "La cuenta se encuentra bloqueada temporalmente por múltiples intentos fallidos.");
                return View("~/Views/Login/Index.cshtml");
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos. Por favor, intenta de nuevo.");
            return View("~/Views/Login/Index.cshtml");
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult RecoverPassword()
        {
            return View("~/Views/Login/RecoverPassword.cshtml");
        }
    }
}