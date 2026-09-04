// ANÁLISIS Y SOLUCIÓN PARA AccountController.cs
// Basado en errores CS0103 y CS0111

// =================================================================
// PROBLEMA 1: CS0103 - 'usuarioPendiente' no existe en el contexto
// LOCALIZACIÓN: Línea 239
// 
// CAUSA: El método VerifyCode POST (HttpPost) termina incorrectamente
// alrededor de línea 150, dejando código suelto que intenta usar
// 'usuarioPendiente' fuera de su scope.
// 
// El método POST de VerifyCode debe terminar así (alrededor línea 150):
// =================================================================

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

	// ✅ AQUÍ DEBE CERRAR: }
}

// =================================================================
// PROBLEMA 2: CS0111 - 'Logout' ya está definido
// LOCALIZACIÓN: Línea 245
//
// CAUSA: Método Logout() definido DOS VECES
//
// SOLUCIÓN: Mantener UNA SOLA definición (eliminar duplicados)
// =================================================================

// ✅ ÚNICA DEFINICIÓN DE LOGOUT
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Logout()
{
	await _signInManager.SignOutAsync();
	TempData["Success"] = "Has cerrado sesión exitosamente.";
	return RedirectToAction("Login");
}

// ❌ ELIMINAR cualquier otra definición de Logout() duplicada

// =================================================================
// MÉTODOS REQUERIDOS QUE DEBEN EXISTIR (al final antes del cierre })
// =================================================================

private string OcultarCorreo(string? email)
{
	if (string.IsNullOrEmpty(email)) 
		return "correo@ejemplo.com";

	var partes = email.Split('@');
	if (partes.Length != 2) 
		return email;

	var usuario = partes[0];
	var dominio = partes[1];

	if (usuario.Length <= 2)
		return $"{usuario[0]}***@{dominio}";

	return $"{usuario[0]}***{usuario[^1]}@{dominio}";
}

private async Task<IActionResult> RedirigirSegunRolAsync(IdentityUser usuario, string? returnUrl)
{
	var roles = await _userManager.GetRolesAsync(usuario);

	if (roles.Contains("Admin"))
		return RedirectToAction("Index", "Admin");

	if (roles.Contains("Cliente"))
		return RedirectToAction("Index", "Cliente");

	if (roles.Contains("Tecnico"))
		return RedirectToAction("Index", "Tecnico");

	if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
		return Redirect(returnUrl);

	return RedirectToAction("Index", "Home");
}

[HttpGet]
[Authorize]
public IActionResult ChangePassword()
{
	return View();
}

[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ChangePassword(
	string currentPassword,
	string newPassword,
	string confirmPassword)
{
	if (string.IsNullOrWhiteSpace(currentPassword) ||
		string.IsNullOrWhiteSpace(newPassword) ||
		string.IsNullOrWhiteSpace(confirmPassword))
	{
		ModelState.AddModelError(string.Empty, "Todos los campos son obligatorios.");
		return View();
	}

	if (newPassword != confirmPassword)
	{
		ModelState.AddModelError(string.Empty, "Las contraseñas nuevas no coinciden.");
		return View();
	}

	if (newPassword.Length < 6)
	{
		ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 6 caracteres.");
		return View();
	}

	var userId = _userManager.GetUserId(User);
	var user = await _userManager.FindByIdAsync(userId!);

	if (user == null)
	{
		return NotFound();
	}

	var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

	if (!result.Succeeded)
	{
		foreach (var error in result.Errors)
		{
			ModelState.AddModelError(string.Empty, error.Description);
		}
		return View();
	}

	await _emailService.SendLoginAlertAsync(
		user.Email!,
		user.UserName!,
		exitoso: true,
		motivo: "Tu contraseña fue cambiada exitosamente");

	TempData["Success"] = "Contraseña cambiada exitosamente. Por seguridad, necesitas volver a iniciar sesión.";
	return RedirectToAction(nameof(Logout));
}
