using CurlinggoSoft.Models;
using CurlinggoSoft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CurlinggoSoft.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        // ====================================
        // LOGIN
        // ====================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Login/Index.cshtml");
        }

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

            var identityUser = await _userManager.FindByEmailAsync(usuario)
                                ?? await _userManager.FindByNameAsync(usuario);

            if (identityUser == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos. Por favor, intenta de nuevo.");
                return View("~/Views/Login/Index.cshtml");
            }

            if (!identityUser.TwoFactorEnabled)
            {
                identityUser.TwoFactorEnabled = true;
                await _userManager.UpdateAsync(identityUser);
            }

            var result = await _signInManager.PasswordSignInAsync(
                identityUser,
                clave,
                isPersistent: false,
                lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                var codigo = await _userManager.GenerateTwoFactorTokenAsync(identityUser, TokenOptions.DefaultEmailProvider);
                await _emailService.SendTwoFactorCodeAsync(identityUser.Email!, codigo);

                return RedirectToAction("VerifyCode", new { returnUrl });
            }

            if (result.Succeeded)
            {
                await _emailService.SendLoginAlertAsync(identityUser.Email!, identityUser.UserName!, exitoso: true, motivo: "Inicio de sesión exitoso");
                return await RedirigirSegunRolAsync(identityUser, returnUrl);
            }

            if (result.IsLockedOut)
            {
                await _emailService.SendLoginAlertAsync(identityUser.Email!, identityUser.UserName!, exitoso: false, motivo: "Cuenta bloqueada temporalmente por múltiples intentos fallidos");
                ModelState.AddModelError(string.Empty, "La cuenta se encuentra bloqueada temporalmente por múltiples intentos fallidos.");
                return View("~/Views/Login/Index.cshtml");
            }

            await _emailService.SendLoginAlertAsync(identityUser.Email!, identityUser.UserName!, exitoso: false, motivo: "Contraseña incorrecta");
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos. Por favor, intenta de nuevo.");
            return View("~/Views/Login/Index.cshtml");
        }

        // ====================================
        // VERIFICACIÓN DE 2FA
        // ====================================

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string? returnUrl = null)
        {
            var usuarioPendiente = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (usuarioPendiente == null)
            {
                ModelState.AddModelError(string.Empty, "Tu sesión de verificación expiró. Inicia sesión de nuevo.");
                return RedirectToAction("Login");
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["CorreoDestino"] = OcultarCorreo(usuarioPendiente.Email);
            return View("~/Views/Login/VerifyCode.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(string codigo, string? returnUrl = null)
        {
            var usuarioPendiente = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (usuarioPendiente == null)
            {
                ModelState.AddModelError(string.Empty, "Tu sesión de verificación expiró. Inicia sesión de nuevo.");
                return RedirectToAction("Login");
            }

            if (string.IsNullOrWhiteSpace(codigo))
            {
                ModelState.AddModelError(string.Empty, "Ingresa el código que te enviamos por correo.");
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["CorreoDestino"] = OcultarCorreo(usuarioPendiente.Email);
                return View("~/Views/Login/VerifyCode.cshtml");
            }

            var result = await _signInManager.TwoFactorSignInAsync(
                TokenOptions.DefaultEmailProvider,
                codigo,
                isPersistent: false,
                rememberClient: false);

            if (result.Succeeded)
            {
                await _emailService.SendLoginAlertAsync(usuarioPendiente.Email!, usuarioPendiente.UserName!, exitoso: true, motivo: "Inicio de sesión exitoso");
                return await RedirigirSegunRolAsync(usuarioPendiente, returnUrl);
            }

            if (result.IsLockedOut)
            {
                await _emailService.SendLoginAlertAsync(usuarioPendiente.Email!, usuarioPendiente.UserName!, exitoso: false, motivo: "Cuenta bloqueada por múltiples códigos incorrectos");
                ModelState.AddModelError(string.Empty, "La cuenta se encuentra bloqueada temporalmente por múltiples intentos fallidos.");
                return View("~/Views/Login/Index.cshtml");
            }

            await _emailService.SendLoginAlertAsync(usuarioPendiente.Email!, usuarioPendiente.UserName!, exitoso: false, motivo: "Código de verificación incorrecto");
            ModelState.AddModelError(string.Empty, "El código ingresado no es válido o ya expiró.");
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["CorreoDestino"] = OcultarCorreo(usuarioPendiente.Email);
            return View("~/Views/Login/VerifyCode.cshtml");
        }

        // ====================================
        // CAMBIO DE CONTRASEÑA
        // ====================================

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View("~/Views/Account/ChangePassword.cshtml");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            string currentPassword,
            string newPassword,
            string confirmPassword)
        {
            // Validaciones de entrada
            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                ModelState.AddModelError(nameof(currentPassword), "La contraseña actual es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError(nameof(newPassword), "La contraseña nueva es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError(nameof(confirmPassword), "La confirmación de contraseña es obligatoria.");
            }

            if ((newPassword ?? string.Empty) != confirmPassword)
            {
                ModelState.AddModelError(nameof(confirmPassword), "Las contraseñas nuevas no coinciden.");
            }

            if ((newPassword ?? string.Empty).Length < 6)
            {
                ModelState.AddModelError(nameof(newPassword), "La contraseña debe tener al menos 6 caracteres.");
            }

            if (!ModelState.IsValid)
            {
                return View("~/Views/Account/ChangePassword.cshtml");
            }

            // Obtener usuario actual
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return Unauthorized();
            }

            // Cambiar contraseña
            var result = await _userManager.ChangePasswordAsync(usuario, currentPassword, newPassword ?? string.Empty);

            if (result.Succeeded)
            {
                await _emailService.SendLoginAlertAsync(
                    usuario.Email!,
                    usuario.UserName!,
                    exitoso: true,
                    motivo: "Tu contraseña fue cambiada exitosamente");

                TempData["Success"] = "Tu contraseña ha sido actualizada correctamente. Por tu seguridad, deberás iniciar sesión nuevamente.";
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }

            // Mostrar errores de Identity
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            ModelState.AddModelError(string.Empty, "No se pudo cambiar la contraseña. Verifica que la contraseña actual sea correcta.");
            return View("~/Views/Account/ChangePassword.cshtml");
        }

        // ====================================
        // CIERRE DE SESIÓN
        // ====================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Has cerrado sesión exitosamente.";
            return RedirectToAction("Login");
        }

        // ====================================
        // MÉTODOS AUXILIARES PRIVADOS
        // ====================================

        private string OcultarCorreo(string? email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains('@'))
            {
                return "tu correo registrado";
            }

            var partes = email.Split('@');
            var usuarioParte = partes[0];
            var visible = usuarioParte.Length <= 2 ? usuarioParte : usuarioParte[..2];
            return $"{visible}***@{partes[1]}";
        }

        private async Task<IActionResult> RedirigirSegunRolAsync(IdentityUser identityUser, string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var roles = await _userManager.GetRolesAsync(identityUser);

            if (roles.Contains("Admin"))
                return RedirectToAction("Index", "Admin");

            if (roles.Contains("Tecnico"))
                return RedirectToAction("Index", "Tecnico");

            if (roles.Contains("Cliente"))
                return RedirectToAction("Index", "Cliente");

            return RedirectToAction("Index", "Home");
        }
    }
}
