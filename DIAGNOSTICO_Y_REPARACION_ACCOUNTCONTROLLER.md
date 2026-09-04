// SCRIPT PARA DIAGNOSTICAR Y REPARAR AccountController.cs
// Ejecuta estas instrucciones en orden

===============================================================
PASO 1: IDENTIFICAR EL PROBLEMA EXACTO
===============================================================

En Visual Studio, abre:
File → AccountController.cs

Presiona: Ctrl + G (Go to Line)

Ingresa: 239
Presiona Enter

VERÁS: Línea 239 debe mostrar código que usa "usuarioPendiente"

Si ves algo como:
	ViewData["CorreoDestino"] = OcultarCorreo(usuarioPendiente.Email);
	return View("~/Views/Login/VerifyCode.cshtml");  // SIN cierre }

SIGNIFICA: El método VerifyCode(POST) nunca fue cerrado correctamente


===============================================================
PASO 2: ENCONTRAR EL CIERRE DE VerifyCode POST
===============================================================

Busca (Ctrl + F): "public async Task<IActionResult> VerifyCode"

Deberías ver DOS definiciones:
1. [HttpGet] VerifyCode  (GET) - ~línea 99-105
2. [HttpPost] VerifyCode (POST) - ~línea 123-150

La del POST debe terminar con:
	return View("~/Views/Login/VerifyCode.cshtml");
}  ← Búscame a mí

Si NO hay cierre }, ese es tu problema #1


===============================================================
PASO 3: ENCONTRAR LOGOUT DUPLICADO
===============================================================

Busca (Ctrl + F): "public async Task<IActionResult> Logout"

¿Cuántas apariciones hay?

Si hay 2 o más:
	❌ PROBLEMA ENCONTRADO: Logout está duplicado

Si hay 1:
	✅ BIEN: Solo una definición


===============================================================
PASO 4: LISTA DE MÉTODOS QUE DEBEN EXISTIR
===============================================================

Al final del AccountController (antes del cierre final }), 
debes tener EXACTAMENTE estos métodos:

✅ OcultarCorreo(string? email)
✅ RedirigirSegunRolAsync(IdentityUser usuario, string? returnUrl)
✅ ChangePassword() GET
✅ ChangePassword(...) POST
✅ Logout() POST [UNO SOLO, no duplicado]

Si falta alguno → AGREGA

Si está duplicado → ELIMINA UNO


===============================================================
PASO 5: ESTRUCTURA CORRECTA DEL ARCHIVO
===============================================================

Tu AccountController.cs debe tener esta estructura:

namespace CurlinggoSoft.Controllers
{
	public class AccountController : Controller
	{
		// 1. Constructor
		// 2. Login GET
		// 3. Login POST
		// 4. VerifyCode GET
		// 5. VerifyCode POST  ← DEBE TERMINAR CON }
		// 6. ChangePassword GET
		// 7. ChangePassword POST
		// 8. Logout POST  ← UNO SOLO
		// 9. OcultarCorreo (helper)
		// 10. RedirigirSegunRolAsync (helper)
	}  ← Cierre de clase
}   ← Cierre de namespace


===============================================================
SOLUCIÓN RÁPIDA (Si todo lo anterior falla)
===============================================================

1. Backup tu archivo original:
   cp AccountController.cs AccountController.cs.bak

2. Confronta el archivo contra el código en:
   FIX_ACCOUNTCONTROLLER_ANALYSIS.md

3. Compara línea por línea tu versión con la correcta

4. Busca diferencias:
   - Llaves faltantes { }
   - Métodos duplicados
   - Variables fuera de scope

5. Aplica correcciones

6. Guarda y compila:
   dotnet clean
   dotnet build


===============================================================
VERIFICACIÓN FINAL
===============================================================

Si la compilación expresa:
✅ Build succeeded (o similar)
   → PROBLEMA RESUELTO

Si aún hay errores:
❌ Reporta el mensaje EXACTO del error
   → Necesitamos línea específica
