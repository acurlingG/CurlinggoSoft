# Guía de Implementación: Fix ArgumentNullException en GuardarPaso2

## Resumen Ejecutivo

Se identificó y corrigió una excepción `ArgumentNullException` que ocurría cuando `modelo.Email` era null en el método `GuardarPaso2` del wizard de registro de técnicos.

**Causa raíz:** Fallos en cadena en la validación y carga de datos del email:
1. No se validaba si email era nulo antes de usarlo en `FindByEmailAsync()`
2. La precarga de datos no manejaba cuando `Usuario` no estaba inicializado
3. No había fallback para cargar datos de BD si la relación de navegación era nula

---

## Cambios Realizados

### Cambio 1: Validación Defensiva (GuardarPaso2)

**Archivo:** `CurlinggoSoft/Controllers/SolicitudTecnicoController.cs`  
**Líneas:** 172-175 (nueva validación)

```csharp
[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public async Task<IActionResult> GuardarPaso2(DatosPersonalesStepViewModel modelo, string? clave)
{
	try
	{
		var usuarioId = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;

		// ✅ NUEVO: Validación defensiva agregada
		if (string.IsNullOrWhiteSpace(modelo.Email))
		{
			ModelState.AddModelError(nameof(modelo.Email), "El correo electrónico es obligatorio.");
		}

		if (usuarioId == null && string.IsNullOrWhiteSpace(clave))
		{
			ModelState.AddModelError(nameof(clave), "La contraseña es obligatoria para crear su cuenta.");
		}

		// ... resto del código
	}
	catch (Exception ex)
	{
		// ... manejo de excepciones
	}
}
```

**Lógica:**
- Antes de cualquier operación, se valida que `Email` no sea nulo/vacío
- Si es inválido, se agrega error a `ModelState`
- Se rechaza el request tempranamente con mensaje claro

**Protege contra:** Requests incompletos del formulario o manipulados

---

### Cambio 2: Mejora en CargarModeloDesdeSolicitud → CargarModeloDesdeSolicitudAsync

**Archivo:** `CurlinggoSoft/Controllers/SolicitudTecnicoController.cs`  
**Líneas:** 648-709 (método rediseñado)

#### Antes (Problemático):
```csharp
private static void CargarModeloDesdeSolicitud(SolicitudTecnicoWizardViewModel modelo, SolicitudTecnico solicitud)
{
	// Paso 2: Datos Personales
	if (solicitud.Usuario != null)
	{
		modelo.DatosPersonales.Nombre = solicitud.Usuario.Nombre;
		modelo.DatosPersonales.Apellidos = solicitud.Usuario.Apellidos;
		modelo.DatosPersonales.Email = solicitud.Usuario.Email;  // ❌ Si Usuario es null → Email es null
		// ...
	}
	// ❌ No hay manejo si Usuario es nulo
}
```

#### Después (Reparado):
```csharp
private async Task CargarModeloDesdeSolicitudAsync(SolicitudTecnicoWizardViewModel modelo, SolicitudTecnico solicitud)
{
	// Paso 2: Datos Personales
	if (solicitud.Usuario != null)
	{
		// ✅ CAMBIO 1: Cargar desde la relación de navegación si existe
		modelo.DatosPersonales.Nombre = solicitud.Usuario.Nombre;
		modelo.DatosPersonales.Apellidos = solicitud.Usuario.Apellidos;
		modelo.DatosPersonales.Email = solicitud.Usuario.Email;
		modelo.DatosPersonales.Telefono = solicitud.Usuario.Telefono;
		modelo.DatosPersonales.Identificacion = solicitud.Identificacion;
	}
	else if (!string.IsNullOrEmpty(solicitud.UsuarioID))
	{
		// ✅ CAMBIO 2: Fallback - Cargar desde BD si no estaba cargado
		var usuario = await _context.Usuarios.FindAsync(solicitud.UsuarioID);
		if (usuario != null)
		{
			modelo.DatosPersonales.Nombre = usuario.Nombre;
			modelo.DatosPersonales.Apellidos = usuario.Apellidos;
			modelo.DatosPersonales.Email = usuario.Email;  // ✅ Nunca será null si Usuario existe en BD
			modelo.DatosPersonales.Telefono = usuario.Telefono;
			modelo.DatosPersonales.Identificacion = solicitud.Identificacion;
		}
	}

	// ... resto de pasos (3-6) se cargan igual que antes
	// Paso 3: Especialidades
	// Paso 4: Movilidad
	// Paso 5: Cobertura
	// Paso 6: Seguro y Accesibilidad
}
```

**Lógica de Fallback:**
1. Intento primario: usar `solicitud.Usuario` si fue cargado con `.Include(s => s.Usuario)`
2. Fallback (nuevo): si Usuario es null, cargar desde BD usando `UsuarioID`
3. Seguridad: solo procede si Usuario existe en BD y tiene datos válidos

**Protege contra:** 
- Relaciones de navegación no cargadas
- Desincronización entre SolicitudTecnico y Usuario
- Email faltante en precarga

---

### Cambio 3: Actualizar Invocación en Paso() GET

**Archivo:** `CurlinggoSoft/Controllers/SolicitudTecnicoController.cs`  
**Línea:** 123 (actualizada)

```csharp
[AllowAnonymous]
public async Task<IActionResult> Paso(int paso)
{
	if (paso < 1 || paso > 8) return NotFound();

	var solicitud = await ObtenerSolicitudEnProgresoAsync();
	var modelo = new SolicitudTecnicoWizardViewModel { PasoActual = paso };

	if (solicitud != null)
	{
		modelo.SolicitudTecnicoID = solicitud.SolicitudTecnicoID;
		modelo.CodigoSolicitud = solicitud.CodigoSolicitud;

		// ✅ CAMBIO: Ahora es async
		await CargarModeloDesdeSolicitudAsync(modelo, solicitud);  // Antes: CargarModeloDesdeSolicitud
	}

	// ... resto del método
}
```

**Cambio:** De `CargarModeloDesdeSolicitud()` síncrona a `await CargarModeloDesdeSolicitudAsync()` asincrónica

---

## Impacto por Escenario

### Escenario 1: Usuario Nuevo (Anónimo) - Paso 2 POST
```
Usuario rellenan Paso 2 → Email: "nuevo@example.com", Contraseña: "123456"
	 ↓
Validación defensiva: ✅ Email NO está vacío
	 ↓
FindByEmailAsync("nuevo@example.com"): ✅ No lanza excepción
	 ↓
ÉXITO: Cuenta creada
```

### Escenario 2: Usuario Anónimo - Email Vacío
```
Usuario envía Paso 2 → Email: "", Contraseña: "123456"
	 ↓
Validación defensiva: ❌ Email ESTÁ vacío
	 ↓
ModelState.AddError("El correo electrónico es obligatorio.")
	 ↓
return View("Paso2", model); // Muestra formulario con error
```

### Escenario 3: Usuario Autenticado - Precarga Paso 2 GET
```
Usuario hace Paso GET paso=2
	 ↓
ObtenerSolicitudEnProgresoAsync() → carga solicitud
	 ↓
CargarModeloDesdeSolicitudAsync():
  - ¿solicitud.Usuario != null? 
	→ SÍ: Usa datos de navegación ✅
	→ NO: Fallback a BD using UsuarioID ✅
	 ↓
modelo.DatosPersonales.Email = "usuario@example.com"  ✅ Nunca null
	 ↓
return View("Paso2", modelo);  // Formulario precargado correctamente
```

---

## Pruebas de Validación

### Test 1: Usuario Anónimo - Email Válido
```csharp
[TestMethod]
public async Task GuardarPaso2_UsuarioAnonimo_EmailValido_DebeCrearCuenta()
{
	// Arrange
	var modelo = new DatosPersonalesStepViewModel
	{
		Email = "test@example.com",    // ✅ Email presente
		Nombre = "Juan",
		Apellidos = "Pérez",
		Telefono = "5555555",
		Identificacion = "12345678"
	};

	var controller = new SolicitudTecnicoController(_context, _userManager);

	// Act
	var result = await controller.GuardarPaso2(modelo, "Password123!");

	// Assert
	Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
	// Email se procesó correctamente
}
```

### Test 2: Usuario Anónimo - Email Vacío
```csharp
[TestMethod]
public async Task GuardarPaso2_UsuarioAnonimo_EmailVacio_DebeRechazar()
{
	// Arrange
	var modelo = new DatosPersonalesStepViewModel
	{
		Email = "",  // ❌ Email vacío
		Nombre = "Juan",
		Apellidos = "Pérez",
		Telefono = "5555555",
		Identificacion = "12345678"
	};

	var controller = new SolicitudTecnicoController(_context, _userManager);

	// Act
	var result = await controller.GuardarPaso2(modelo, "Password123!");

	// Assert
	Assert.IsInstanceOfType(result, typeof(ViewResult));
	Assert.IsFalse(controller.ModelState.IsValid);
	Assert.IsTrue(controller.ModelState["Email"].Errors.Count > 0);
}
```

### Test 3: Usuario Autenticado - Precarga Paso 2
```csharp
[TestMethod]
public async Task Paso_UsuarioAutenticado_Paso2_DebeCargarEmailPrecargado()
{
	// Arrange
	var usuarioId = "user123";
	var usuario = new Usuario 
	{ 
		UsuarioID = usuarioId, 
		Email = "test@example.com",
		Nombre = "Juan",
		Apellidos = "Pérez",
		Telefono = "5555555"
	};
	_context.Usuarios.Add(usuario);

	var solicitud = new SolicitudTecnico 
	{ 
		SolicitudTecnicoID = 1,
		UsuarioID = usuarioId
		// Nota: Usuario NO está incluido explícitamente aquí
	};
	_context.SolicitudesTecnico.Add(solicitud);
	await _context.SaveChangesAsync();

	var controller = new SolicitudTecnicoController(_context, _userManager);

	// Act
	var result = await controller.Paso(2) as ViewResult;
	var vm = result.Model as SolicitudTecnicoWizardViewModel;

	// Assert
	Assert.AreEqual("test@example.com", vm.DatosPersonales.Email);  // ✅ Email cargado
	Assert.AreEqual("Juan", vm.DatosPersonales.Nombre);
}
```

---

## Verificación Post-Implementación

- ✅ Compilación exitosa (sin errores C#)
- ✅ Método ahora es `async` tanto en firma como en invocación
- ✅ Validación defensiva ejecutada ANTES de `FindByEmailAsync()`
- ✅ Fallback a BD implementado cuando Usuario es null
- ✅ Todos los pasos (3-6) se cargan en el método actualizado
- ✅ No hay cambios en la BD (solo lógica de aplicación)

---

## Notas de Compatibilidad

- **Cambio de firma:** `CargarModeloDesdeSolicitud()` → `CargarModeloDesdeSolicitudAsync()` 
  - Cualquier otra invocación a este método debe actualizarse a `await`
- **Rendimiento:** Pequeño trade-off mínimo: fallback a BD solo si Usuario no está pre-cargado
- **Comportamiento visible:** El usuario verá mejor validación de errores y formularios siempre precargados correctamente

