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

        private async Task<IActionResult> RedirigirSegunRolAsync(IdentityUser identityUser, string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (await _userManager.IsInRoleAsync(identityUser, "Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        private static string OcultarCorreo(string? email)
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
    }
}