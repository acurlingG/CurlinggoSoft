# 📋 GUÍA DE ACCIONES PENDIENTES - PASO A PASO

**Última Actualización**: 31 Agosto 2026  
**Prioridad**: ALTA  
**Tiempo Total**: ~40 minutos  

---

## 🎯 ACCIÓN 1: TecnicoController.MiDisponibilidad() 

### Paso 1.1: Agregar métodos a TecnicoController.cs
**Ubicación**: Al final de `TecnicoController.cs`, ANTES del último cierre `}`

```csharp
// GET: /Tecnico/MiDisponibilidad
// Acción PRIVADA: Técnico solo ve SU PROPIA disponibilidad
[HttpGet]
public async Task<IActionResult> MiDisponibilidad()
{
	var disponibilidadTecnico = await _context.DisponibilidadTecnico
		.Include(d => d.Tecnico)
		.Where(d => d.TecnicoID == TecnicoId)
		.OrderBy(d => d.DiaSemana)
		.ThenBy(d => d.HoraInicio)
		.ToListAsync();

	return View(disponibilidadTecnico);
}

// GET: /Tecnico/EditarMiDisponibilidad/{id}
[HttpGet]
public async Task<IActionResult> EditarMiDisponibilidad(long? id)
{
	if (id == null) return NotFound();

	var disponibilidad = await _context.DisponibilidadTecnico.FindAsync(id);
	if (disponibilidad == null || disponibilidad.TecnicoID != TecnicoId)
		return Unauthorized();

	return View(disponibilidad);
}

// POST: /Tecnico/EditarMiDisponibilidad/{id}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditarMiDisponibilidad(long? id, 
	[Bind("DisponibilidadID,TecnicoID,DiaSemana,HoraInicio,HoraFin,Activa")] DisponibilidadTecnico modelo)
{
	if (id != modelo.DisponibilidadID) return NotFound();

	if (modelo.TecnicoID != TecnicoId)
		return Unauthorized();

	if (ModelState.IsValid)
	{
		try
		{
			_context.Update(modelo);
			await _context.SaveChangesAsync();
			TempData["Success"] = "Disponibilidad actualizada correctamente.";
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!Exists(modelo.DisponibilidadID))
				return NotFound();
			throw;
		}
		return RedirectToAction(nameof(MiDisponibilidad));
	}
	return View(modelo);
}

private bool Exists(long id) => _context.DisponibilidadTecnico.Any(e => e.DisponibilidadID == id);
```

**Validación**:
```
Verificar que compila: dotnet build
```

---

### Paso 1.2: Crear Vista MiDisponibilidad.cshtml
**Ubicación**: `CurlinggoSoft/Views/Tecnico/MiDisponibilidad.cshtml` (NUEVO ARCHIVO)

```razor
@model IEnumerable<CurlinggoSoft.Models.DisponibilidadTecnico>

@{
	ViewData["Title"] = "Mi Disponibilidad";

	string NombreDia(byte dia) => dia switch
	{
		1 => "Lunes",
		2 => "Martes",
		3 => "Miércoles",
		4 => "Jueves",
		5 => "Viernes",
		6 => "Sábado",
		7 => "Domingo",
		_ => "Desconocido"
	};
}

<div class="container my-4">
	<h2 class="fw-bold mb-3">
		<i class="fa fa-clock-o"></i> Mi Disponibilidad
	</h2>
	<p class="text-muted">Configura tu disponibilidad horaria para recibir ofertas de servicios.</p>

	@if (TempData["Success"] != null)
	{
		<div class="alert alert-success">@TempData["Success"]</div>
	}

	@if (!Model.Any())
	{
		<div class="alert alert-info">
			Aún no has configurado tu disponibilidad. 
			Contacta al administrador para agregar tus horarios.
		</div>
	}
	else
	{
		<div class="table-responsive">
			<table class="table table-striped table-hover align-middle">
				<thead class="table-dark">
					<tr>
						<th>Día</th>
						<th>Hora Inicio</th>
						<th>Hora Fin</th>
						<th>Estado</th>
						<th>Acciones</th>
					</tr>
				</thead>
				<tbody>
					@foreach (var item in Model)
					{
						<tr>
							<td><strong>@NombreDia(item.DiaSemana)</strong></td>
							<td>@item.HoraInicio</td>
							<td>@item.HoraFin</td>
							<td>
								<span class="badge @(item.Activa ? "bg-success" : "bg-danger")">
									@(item.Activa ? "Activa" : "Inactiva")
								</span>
							</td>
							<td>
								<a asp-action="EditarMiDisponibilidad" asp-route-id="@item.DisponibilidadID" 
								   class="btn btn-warning btn-sm">
									<i class="fa fa-edit"></i> Editar
								</a>
							</td>
						</tr>
					}
				</tbody>
			</table>
		</div>
	}
</div>
```

---

### Paso 1.3: Crear Vista EditarMiDisponibilidad.cshtml
**Ubicación**: `CurlinggoSoft/Views/Tecnico/EditarMiDisponibilidad.cshtml` (NUEVO ARCHIVO)

```razor
@model CurlinggoSoft.Models.DisponibilidadTecnico

@{
	ViewData["Title"] = "Editar Mi Disponibilidad";
}

<div class="container my-4">
	<div class="row justify-content-center">
		<div class="col-md-6">
			<h2 class="fw-bold mb-3">Editar Disponibilidad</h2>

			<form asp-action="EditarMiDisponibilidad" method="post">
				<input type="hidden" asp-for="DisponibilidadID" />
				<input type="hidden" asp-for="TecnicoID" />
				<input type="hidden" asp-for="DiaSemana" />

				<div class="mb-3">
					<label class="form-label">Día: <strong id="diaNombre"></strong></label>
					<small class="text-muted d-block">
						Este campo no puede modificarse.
					</small>
				</div>

				<div class="mb-3">
					<label asp-for="HoraInicio" class="form-label">Hora Inicio</label>
					<input asp-for="HoraInicio" class="form-control" type="time" />
					<span asp-validation-for="HoraInicio" class="text-danger"></span>
				</div>

				<div class="mb-3">
					<label asp-for="HoraFin" class="form-label">Hora Fin</label>
					<input asp-for="HoraFin" class="form-control" type="time" />
					<span asp-validation-for="HoraFin" class="text-danger"></span>
				</div>

				<div class="mb-3 form-check">
					<input asp-for="Activa" class="form-check-input" type="checkbox" />
					<label asp-for="Activa" class="form-check-label">Activa</label>
				</div>

				<button type="submit" class="btn btn-primary">
					<i class="fa fa-save"></i> Guardar
				</button>
				<a asp-action="MiDisponibilidad" class="btn btn-secondary">
					<i class="fa fa-times"></i> Cancelar
				</a>
			</form>
		</div>
	</div>
</div>

<script>
	const dias = {
		1: "Lunes", 2: "Martes", 3: "Miércoles", 4: "Jueves",
		5: "Viernes", 6: "Sábado", 7: "Domingo"
	};
	const diaSemana = parseInt(document.querySelector('input[name="DiaSemana"]').value);
	document.getElementById('diaNombre').innerText = dias[diaSemana];
</script>
```

**Validación**:
```
Verificar que compila: dotnet build
```

---

## 🎯 ACCIÓN 2: Implementar Cambio de Contraseña

### Paso 2.1: Agregar métodos a AccountController.cs
**Ubicación**: Al final de `AccountController.cs`, ANTES del último cierre `}`

```csharp
// GET: /Account/ChangePassword
[HttpGet]
[Authorize]
public IActionResult ChangePassword()
{
	return View();
}

// POST: /Account/ChangePassword
[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ChangePassword(string passwordActual, string passwordNueva, string passwordConfirmar)
{
	if (string.IsNullOrWhiteSpace(passwordActual) || string.IsNullOrWhiteSpace(passwordNueva))
	{
		ModelState.AddModelError(string.Empty, "Todos los campos son obligatorios.");
		return View();
	}

	if (passwordNueva != passwordConfirmar)
	{
		ModelState.AddModelError(string.Empty, "Las contraseñas nuevas no coinciden.");
		return View();
	}

	if (passwordNueva.Length < 6)
	{
		ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 6 caracteres.");
		return View();
	}

	var user = await _userManager.GetUserAsync(User);
	if (user == null)
		return Unauthorized();

	var result = await _userManager.ChangePasswordAsync(user, passwordActual, passwordNueva);

	if (result.Succeeded)
	{
		TempData["Success"] = "Contraseña actualizada exitosamente.";
		return RedirectToAction("Index", "Home");
	}

	foreach (var error in result.Errors)
		ModelState.AddModelError(string.Empty, error.Description);

	return View();
}
```

---

### Paso 2.2: Crear Vista ChangePassword.cshtml
**Ubicación**: `CurlinggoSoft/Views/Account/ChangePassword.cshtml` (NUEVO ARCHIVO)

```razor
@{
	ViewData["Title"] = "Cambiar Contraseña";
}

<div class="container my-5">
	<div class="row justify-content-center">
		<div class="col-md-5">
			<div class="card shadow">
				<div class="card-header bg-primary text-white">
					<h4 class="mb-0">
						<i class="fa fa-key"></i> Cambiar Contraseña
					</h4>
				</div>
				<div class="card-body">
					@if (!ViewData.ModelState.IsValid)
					{
						<div class="alert alert-danger">
							<strong>Errores encontrados:</strong>
							<ul class="mb-0">
								@foreach (var error in ViewData.ModelState.Values.SelectMany(v => v.Errors))
								{
									<li>@error.ErrorMessage</li>
								}
							</ul>
						</div>
					}

					@if (TempData["Success"] != null)
					{
						<div class="alert alert-success">@TempData["Success"]</div>
					}

					<form method="post" asp-action="ChangePassword">

						<div class="mb-3">
							<label for="passwordActual" class="form-label">
								Contraseña Actual
							</label>
							<input type="password" class="form-control" id="passwordActual" 
								   name="passwordActual" required />
						</div>

						<div class="mb-3">
							<label for="passwordNueva" class="form-label">
								Contraseña Nueva
							</label>
							<input type="password" class="form-control" id="passwordNueva" 
								   name="passwordNueva" required minlength="6" />
							<small class="form-text text-muted">
								Mínimo 6 caracteres
							</small>
						</div>

						<div class="mb-3">
							<label for="passwordConfirmar" class="form-label">
								Confirmar Contraseña Nueva
							</label>
							<input type="password" class="form-control" id="passwordConfirmar" 
								   name="passwordConfirmar" required minlength="6" />
						</div>

						<button type="submit" class="btn btn-primary w-100">
							<i class="fa fa-save"></i> Actualizar Contraseña
						</button>

						<a href="/" class="btn btn-secondary w-100 mt-2">
							<i class="fa fa-times"></i> Cancelar
						</a>

					</form>
				</div>
			</div>
		</div>
	</div>
</div>
```

---

### Paso 2.3: Agregar Enlace en _Layout.cshtml
**Ubicación**: `CurlinggoSoft/Views/Shared/_Layout.cshtml`

Busca la sección de menú de usuario (usualmente al final del navbar donde está el logout) y agrega:

```razor
@if (User.Identity?.IsAuthenticated ?? false)
{
	<li class="nav-item">
		<a class="nav-link" asp-controller="Account" asp-action="ChangePassword" title="Cambiar contraseña">
			<i class="fa fa-key"></i> Cambiar Contraseña
		</a>
	</li>
}
```

---

## ✅ VALIDACIÓN FINAL

### Test 1: Compilación
```bash
cd CurlinggoSoft
dotnet clean
dotnet build
```
Debe completar SIN errores ✅

### Test 2: TecnicoController.MiDisponibilidad
```
1. Login como Técnico
2. Navegar a /Tecnico/MiDisponibilidad
3. Debe ver tabla con su disponibilidad
4. Hacer clic en Editar
5. Cambiar hora, guardar
6. Verificar que cambio se aplicó
```

### Test 3: Cambio de Contraseña
```
1. Login como cualquier usuario
2. Navegar a /Account/ChangePassword
3. Ingresar contraseña actual incorrecta → Debe mostrar error
4. Ingresar contraseña actual correcta + nueva (2 veces igual)
5. Guardar
6. Logout
7. Login con contraseña nueva → Debe funcionar ✅
```

### Test 4: Seguridad DireccionesCliente
```
1. Login como Cliente A
2. Intentar acceder directamente a: /DireccionesCliente/Details/999 (ID de otro cliente)
3. Debe recibir NotFound ✅
```

### Test 5: Seguridad DisponibilidadTecnico
```
1. Login como Técnico
2. Intentar acceder a: /DisponibilidadTecnico/Index
3. Debe recibir Unauthorized ✅
```

---

## ⚠️ NOTAS CRÍTICAS

1. **Nombres exactos**: Los nombres de vistas y acciones deben ser exactamente como se escriben (CaseSensitive)

2. **Validación SIEMPRE**: Cada acción que reciba POST debe validar la propiedad del usuario

3. **Seguridad de contraseña**: Usar SIEMPRE `_userManager.ChangePasswordAsync()`, NUNCA guardar directamente

4. **Testing**: Prueba CADA cambio inmediatamente después de implementarlo

---

**Tiempo Total**: ~40 minutos  
**Prioridad**: ⚠️ ALTA - Implementar HOY  
**Recomendación Final**: Hacer un `git commit` después de cada acción

