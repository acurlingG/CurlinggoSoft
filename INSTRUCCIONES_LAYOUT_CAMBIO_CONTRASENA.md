# Ubicación del Enlace de Cambiar Contraseña en _Layout.cshtml

## Instrucciones de Integración Manual

Debe buscar en `CurlinggoSoft\Views\Shared\_Layout.cshtml` la sección donde aparecen:
- Menú de usuario autenticado (típicamente `@if (User.Identity?.IsAuthenticated)`)
- Botón/enlace de "Cerrar Sesión" o "Logout"
- Dropdown de usuario (generalmente al final del navbar, lado derecho)

En esa sección, ANTES del botón de "Cerrar Sesión", debe agregarse:

```html
<!-- CAMBIAR CONTRASEÑA -->
<li class="nav-item">
	<a class="nav-link" 
	   asp-controller="Account" 
	   asp-action="ChangePassword">
		<i class="fa fa-key"></i>
		Cambiar Contraseña
	</a>
</li>
```

O si es dropdown:

```html
<div class="dropdown-item">
	<a asp-controller="Account" asp-action="ChangePassword">
		<i class="fa fa-key"></i> Cambiar Contraseña
	</a>
</div>
```

## Archivos Ya Creados & Listos:

✅ `CurlinggoSoft\Controllers\AccountController.cs` - Métodos `ChangePassword()` GET/POST y `Logout()` agregados
✅ `CurlinggoSoft\Views\Account\ChangePassword.cshtml` - Vista del formulario
✅ `CurlinggoSoft\Controllers\TecnicoController.Disponibilidad.cs` - Extensión parcial para `MiDisponibilidad()`
✅ `CurlinggoSoft\Views\Tecnico\MiDisponibilidad.cshtml` - Vista de disponibilidad propia del técnico
✅ `CurlinggoSoft\Views\Tecnico\EditarMiDisponibilidad.cshtml` - Vista de edición

## Próximos Pasos:

1. Localizar MANUALMENTE en `_Layout.cshtml` la sección de menú autenticado
2. Agregar el enlace de "Cambiar Contraseña" 
3. Ejecutar `dotnet build` y probar ambas funcionalidades:
   - Técnico accediendo a "Mi Disponibilidad"
   - Cualquier usuario accediendo a "Cambiar Contraseña"

## Nota de Seguridad:

El método `ChangePassword()` POST ya redirige a `Logout()` después de cambiar la contraseña
por seguridad, obligando al usuario a re-loguearse con la nueva contraseña.
