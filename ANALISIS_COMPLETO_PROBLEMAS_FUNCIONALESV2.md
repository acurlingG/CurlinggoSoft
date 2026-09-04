# ANÁLISIS COMPLETO - PROBLEMAS FUNCIONALES DE CURLINGGO

**Fecha**: 31 Agosto 2026  
**Proyecto**: CURLINGgo.exe (PID: 4572)  
**Versión**: .NET 10.0, EF Core 10.0.11  
**Versión ASP.NET Core**: 10.0.0

---

## 📋 PROBLEMAS IDENTIFICADOS (Orden de Criticidad)

### 🔴 CRÍTICOS (Seguridad / Acceso a Datos)

#### 1. **Sin Control de Acceso en DireccionesClienteController** ⚠️ SEGURIDAD ALTA
**Archivo**: `CurlinggoSoft/Controllers/DireccionesClienteController.cs`

**Problema**:
- Método `Index()` retorna **TODAS** las direcciones de **TODOS** los clientes sin filtración
- No valida que el usuario logueado solo vea sus propias direcciones
- Cualquier cliente puede acceder a direcciones de otros clientes
- **Línea problemática**: `await _context.DireccionesCliente.Include(...).ToListAsync();`

**Código Actual (INSEGURO)**:
```csharp
public async Task<IActionResult> Index() 
	=> View(await _context.DireccionesCliente
		.Include(d => d.Cliente)
		.Include(d => d.Provincia)
		.Include(d => d.Canton)
		.Include(d => d.Distrito)
		.ToListAsync()); // ❌ SIN FILTRO
```

**Impacto**: 
- Exposición de datos personales de todos los clientes
- Violación de privacidad GDPR/equivalentes

---

#### 2. **Filtro de Disponibilidad sin Validación de Rol** ⚠️ SEGURIDAD ALTA
**Archivo**: `CurlinggoSoft/Controllers/DisponibilidadTecnicoController.cs`

**Problema**:
- El controlador NO tiene atributo `[Authorize(Roles = "...")]`
- Técnicos pueden ver/filtrar disponibilidad de **TODOS** los técnicos
- Debería ser acción solo para Admins (excepto ver la propia disponibilidad)
- Falta control: un técnico solo debe ver su propia disponibilidad

**Código Actual (sin protección)**:
```csharp
public class DisponibilidadTecnicoController : Controller // ❌ SIN [Authorize]
{
	// GET: /DisponibilidadTecnico/Index?tecnicoId=xxx
	public async Task<IActionResult> Index(string? tecnicoId)
	{
		var query = _context.DisponibilidadTecnico.Include(d => d.Tecnico).AsQueryable();

		if (!string.IsNullOrEmpty(tecnicoId))
		{
			query = query.Where(d => d.TecnicoID == tecnicoId); // Permite filtrar otros técnicos
		}
		// ... retorna datos de todos
	}
}
```

**Impacto**:
- Técnicos pueden manipular horarios de otros técnicos
- Admin no tiene control centralizado

---

### 🟠 ALTOS (Funcionalidad Crítica)

#### 3. **Códigos de Reserva Inconsistentes Entre Rol Cliente y Técnico** 
**Archivos**: 
- `Views/Cliente/MisReservas.cshtml` (línea 24)
- `Views/Tecnico/OfertasDisponibles.cshtml` (NO muestra código)

**Problema**:
- **Cliente VE**: Primeros 8 caracteres del Guid: `@item.CodigoSeguimiento.ToString().Substring(0, 8)...`
  - Ejemplo: `a1b2c3d4...` (parcial Guid)
- **Técnico VE**: NADA parecido a un código de reserva, solo información del servicio
- Ambos debería ver formato consistente: `CR-` + identificador legible (Ej: `CR-145`)

**Cliente MisReservas.cshtml**:
```razor
<td><code>@item.CodigoSeguimiento.ToString().Substring(0, 8)...</code></td>
```

**Técnico OfertasDisponibles.cshtml** (NO muestra código):
```razor
<td><strong>@(oferta.Reserva?.Servicio?.NombreServicio ?? "Servicio Técnico")</strong></td>
<!-- ❌ SIN CÓDIGO DE SEGUIMIENTO -->
```

**Impacto**:
- Comunicación confusa cliente-técnico
- Imposible referenciar reservas por número intuitivo
- Discrepancia con captura de pantalla donde técnico ve "CR-XXX"

---

#### 4. **Ordenamiento Incorrecto de Reservas (Cliente y Técnico)**
**Archivo**: `CurlinggoSoft/Controllers/ClienteController.cs` (líneas 33-36, 56-60)

**Problema**:
- ClienteController ordena por `FechaHoraSolicitud` (fecha de creación)
- Debería ordenar por `ReservaID` descendente (más nuevas primero)
- Las capturas muestran reservas "viejas" arriba, "nuevas" abajo

**Código Actual**:
```csharp
// GET: /Cliente/MisReservas
public async Task<IActionResult> MisReservas()
{
	var clienteId = _userManager.GetUserId(User);
	var reservas = await _context.SolicitudesReserva
		.Include(r => r.Servicio)
		.Include(r => r.EstadoReserva)
		.Include(r => r.Tecnico)
		.Where(r => r.ClienteID == clienteId)
		.OrderByDescending(r => r.FechaHoraSolicitud) // ❌ DÉBIL: ordena por fecha, no por ID
		.ToListAsync();
}

// Mismo en Index (líneas 27-36):
.OrderByDescending(r => r.FechaHoraSolicitud)
```

**Técnico similar** (TecnicoController.cs línea 58):
```csharp
.OrderByDescending(r => r.FechaHoraProgramada) // ❌ Por fecha programada, no ID
```

---

### 🟡 MEDIANOS (Funcionalidad Menor/Seguridad Débil)

#### 5. **Falta: Cambio de Contraseña en Menú**
**Archivo**: `CurlinggoSoft/Controllers/AccountController.cs`

**Problema**:
- No existe acción `ChangePassword` (revisado hasta línea 196)
- No hay opción en menú para cambiar contraseña
- Usuarios no pueden actualizar credenciales

**Necesario**:
- Acción GET: mostrar formulario
- Acción POST: validar contraseña actual, actualizar con nueva
- Agregar enlace en navbar `_Layout.cshtml`

---

#### 6. **MisDirecciones no Filtra por Cliente Logueado** (Seguridad Media)
**Archivo**: `CurlinggoSoft/Controllers/ClienteController.cs` (línea 59-68)

**Problema**:
- CORRECTO: `ClienteController.MisDirecciones()` SÍ filtra por `ClienteID == clienteId`
- INCORRECTO: `DireccionesClienteController.Index()` NO filtra (ver problema #1)
- Existe dualidad: ClienteController es seguro, pero DireccionesClienteController no

**Código Correcto (En ClienteController)**:
```csharp
public async Task<IActionResult> MisDirecciones()
{
	var clienteId = _userManager.GetUserId(User);
	var direcciones = await _context.DireccionesCliente
		.Where(d => d.ClienteID == clienteId && d.Activa)
		.ToListAsync(); // ✅ SÍ FILTRA
}
```

---

## 🔧 SOLUCIONES PROPUESTAS

### SOLUCIÓN 1: Asegurar DireccionesClienteController
**Prioridad**: CRÍTICA
**Archivo**: `CurlinggoSoft/Controllers/DireccionesClienteController.cs`

Cambiar método `Index()`:
```csharp
[Authorize(Roles = "Cliente")]
public async Task<IActionResult> Index()
{
	var clienteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

	var direcciones = await _context.DireccionesCliente
		.Include(d => d.Cliente)
		.Include(d => d.Provincia)
		.Include(d => d.Canton)
		.Include(d => d.Distrito)
		.Where(d => d.ClienteID == clienteId && d.Activa) // ✅ FILTRO OBLIGATORIO
		.ToListAsync();

	return View(direcciones);
}
```

---

### SOLUCIÓN 2: Proteger DisponibilidadTecnicoController
**Prioridad**: CRÍTICA
**Archivo**: `CurlinggoSoft/Controllers/DisponibilidadTecnicoController.cs`

Opción A - Para Admins SOLAMENTE:
```csharp
[Authorize(Roles = "Admin")]
public class DisponibilidadTecnicoController : Controller
{
	// Index actual: SOLO Admin puede verlo
}
```

Opción B - Crear acción independiente para Técnico:
```csharp
public class DisponibilidadTecnicoController : ControllerBase
{
	// Admin:
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Index(string? tecnicoId)
	{
		// Puede ver todos, con filtro
	}

	// Técnico:
	[Authorize(Roles = "Tecnico")]
	public async Task<IActionResult> MiDisponibilidad()
	{
		var tecnicoId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var disponibilidad = await _context.DisponibilidadTecnico
			.Where(d => d.TecnicoID == tecnicoId) // ✅ SOLO SUYA
			.ToListAsync();
	}
}
```

---

### SOLUCIÓN 3: Unificar Código de Reserva
**Prioridad**: ALTA
**Archivos**: 
- `Models/SolicitudReserva.cs`
- `Views/Cliente/MisReservas.cshtml`
- `Views/Tecnico/OfertasDisponibles.cshtml`

**Paso 1**: Crear propiedad computed en modelo:
```csharp
[NotMapped]
public string CodigoReservaFormato => $"CR-{ReservaID:D6}"; // Ejemplo: CR-000145
```

**Paso 2**: Actualizar vistas:
```razor
<!-- Cliente y Técnico AMBOS verán: -->
<td><code>@item.Reserva.CodigoReservaFormato</code></td>
<!-- Ahora: CR-000145 -->
```

---

### SOLUCIÓN 4: Corregir Ordenamiento
**Prioridad**: MEDIA
**Archivos**:
- `CurlinggoSoft/Controllers/ClienteController.cs`
- `CurlinggoSoft/Controllers/TecnicoController.cs`

```csharp
// ClienteController - MisReservas (línea 56-60)
.OrderByDescending(r => r.ReservaID) // Mayor ID primero = más reciente

// TecnicoController - Index (línea 58)
.OrderByDescending(r => r.ReservaID) // Mayor ID primero
```

---

### SOLUCIÓN 5: Implementar Cambio de Contraseña
**Prioridad**: MEDIA
**Archivo**: `CurlinggoSoft/Controllers/AccountController.cs`

```csharp
[HttpGet]
[Authorize]
public IActionResult ChangePassword()
{
	return View("~/Views/Account/ChangePassword.cshtml");
}

[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ChangePassword(string actualpwd, string newpwd, string confirmpwd)
{
	if (newpwd != confirmpwd)
	{
		ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
		return View("~/Views/Account/ChangePassword.cshtml");
	}

	var user = await _userManager.GetUserAsync(User);
	var result = await _userManager.ChangePasswordAsync(user!, actualpwd, newpwd);

	if (result.Succeeded)
	{
		TempData["Success"] = "Contraseña actualizada exitosamente.";
		return RedirectToAction("Index", "Home");
	}

	foreach (var error in result.Errors)
		ModelState.AddModelError(string.Empty, error.Description);

	return View("~/Views/Account/ChangePassword.cshtml");
}
```

Agregar enlace en `_Layout.cshtml`:
```razor
@if (User.Identity?.IsAuthenticated ?? false)
{
	<li class="nav-item">
		<a class="nav-link" asp-action="ChangePassword" asp-controller="Account">
			<i class="fa fa-key"></i> Cambiar Contraseña
		</a>
	</li>
}
```

---

## 📊 MATRIZ DE ACCIONES

| # | Problema | Severidad | Esfuerzo | Archivo(s) Afectados | Líneas Aprox |
|---|----------|-----------|----------|----------------------|--------------|
| 1 | DireccionesClienteController sin filtro | CRÍTICA | 5 min | DireccionesClienteController.cs | 9-17 |
| 2 | DisponibilidadTecnicoController sin Authorize | CRÍTICA | 10 min | DisponibilidadTecnicoController.cs | 1-20 |
| 3 | Códigos reserva inconsistentes | ALTA | 15 min | SolicitudReserva.cs, vistas | 52-68, 24, 18 |
| 4 | Ordenamiento por fecha vs ID | MEDIA | 5 min | ClienteController.cs, TecnicoController.cs | 56, 58 |
| 5 | Sin cambio contraseña | MEDIA | 20 min | AccountController.cs, _Layout.cshtml | NUEVO |
| 6 | Vista Técnico sin código reserva | MEDIA | 5 min | OfertasDisponibles.cshtml | 24-26 |

---

## ✅ RESUMEN RECOMENDACIONES

1. **INMEDIATO (Hoy)**:
   - Agregar `[Authorize(Roles = "Cliente")]` a `DireccionesClienteController`
   - Filtrar por `ClienteID == userId` en `Index()`
   - Proteger `DisponibilidadTecnicoController` con `[Authorize(Roles = "Admin")]`

2. **CORTO PLAZO (Esta semana)**:
   - Unificar código de reserva a formato `CR-NNNNNN`
   - Corregir ordenamiento a `OrderByDescending(r => r.ReservaID)`
   - Agregar pantalla de cambio de contraseña

3. **VALIDACIÓN**:
   - Test: Cliente A no puede ver direcciones de Cliente B
   - Test: Técnico A no puede ver disponibilidad de Técnico B
   - Test: Códigos reserva visibles consistentemente

---

**Estado**: Análisis Completo  
**Recomendación**: Implementar soluciones en orden de severidad (CRÍTICAS primero)
