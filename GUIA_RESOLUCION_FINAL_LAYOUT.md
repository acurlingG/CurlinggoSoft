# 🔧 GUÍA DE INTEGRACIÓN - CAMBIAR CONTRASEÑA EN _LAYOUT.CSHTML

## PROBLEMA IDENTIFICADO
El archivo `_Layout.cshtml` es muy grande (1112 líneas) y el sistema de lectura tiene limitaciones.
Se ha completado todo EXCEPTO la integración del enlace en el navbar.

## SOLUCIÓN: 4 OPCIONES

### OPCIÓN 1: USAR EL PARTIAL YA CREADO (RECOMENDADO)
Se creó un partial completo: `CurlinggoSoft/Views/Shared/_MenuUsuarioAutenticado.cshtml`

**Para usar:**
1. Abre `CurlinggoSoft/Views/Shared/_Layout.cshtml`
2. Busca el cierre de `</ul>` del navbar (alrededor de línea 900-1000)
3. ANTES de ese cierre, agrega:
```razor
@await Html.PartialAsync("_MenuUsuarioAutenticado")
```

EJEMPLO DE CONTEXTO:
```razor
					</li>              <!-- Último li del menú principal -->
				</ul>                   <!-- Cierre del navbar-nav principal -->

				@await Html.PartialAsync("_MenuUsuarioAutenticado") <!-- AGREGAR AQUÍ -->

			</div>                       <!-- Cierre del navbar-collapse -->
```

---

### OPCIÓN 2: INTEGRACIÓN DIRECTA (SIN PARTIAL)
Si prefieres integrar el código directamente sin partial:

**Busca en _Layout.cshtml:**
```razor
@if (!User.IsInRole("Admin"))
{
	<!-- Menú público -->
}
```

**Al final del navbar, ANTES de `</div>` (cierre del collapse), agrega:**
```razor
				<!-- MENÚ USUARIO AUTENTICADO -->
				@if (User.Identity?.IsAuthenticated ?? false)
				{
					<ul class="navbar-nav ms-auto">
						<li class="nav-item dropdown">
							<a class="nav-link dropdown-toggle" 
							   href="#" 
							   id="usuarioDropdown" 
							   role="button" 
							   data-bs-toggle="dropdown" 
							   aria-expanded="false">
								<i class="fa fa-user-circle"></i>
								Mi Cuenta
							</a>
							<ul class="dropdown-menu dropdown-menu-end" aria-labelledby="usuarioDropdown">
								<li>
									<a class="dropdown-item" 
									   asp-controller="Account" 
									   asp-action="ChangePassword">
										<i class="fa fa-key"></i>
										Cambiar Contraseña
									</a>
								</li>
								<li><hr class="dropdown-divider"></li>
								<li>
									<form asp-controller="Account" 
										  asp-action="Logout" 
										  method="post" 
										  class="d-inline">
										<button type="submit" class="dropdown-item">
											<i class="fa fa-sign-out"></i>
											Cerrar Sesión
										</button>
									</form>
								</li>
							</ul>
						</li>
					</ul>
				}
```

---

### OPCIÓN 3: USANDO UN MENÚ SIMPLE (ALTERNATIVA MINIMALISTA)
Si quieres algo más simple sin dropdown:

**Agrega antes del cierre del navbar:**
```razor
				<!-- CAMBIAR CONTRASEÑA -->
				@if (User.Identity?.IsAuthenticated ?? false)
				{
					<ul class="navbar-nav ms-auto">
						<li class="nav-item">
							<a class="nav-link" 
							   asp-controller="Account" 
							   asp-action="ChangePassword">
								<i class="fa fa-key"></i>
								Cambiar Contraseña
							</a>
						</li>
					</ul>
				}
```

---

### OPCIÓN 4: BUSCAR AUTOMÁTICAMENTE EL PUNTO DE INYECCIÓN

**Busca estas líneas EN ORDEN** en `_Layout.cshtml`:
1. `</ul>` (cierre de navbar-nav me-auto)
2. `</div>` (cierre de navbar-collapse)
3. Agrega el código entre medio

---

## ARCHIVOS BACKEND LISTOS

✅ **AccountController.cs** - Métodos lista
  - `ChangePassword()` GET y POST
  - `Logout()` POST
  - Validaciones de seguridad
  - Redireccionamiento post-cambio a logout

✅ **Views/Account/ChangePassword.cshtml** - Vista lista
  - Formulario profesional
  - Validaciones en tiempo real
  - Tips de seguridad
  - Styling Bootstrap

✅ **TecnicoController.Disponibilidad.cs** - Extensión lista
  - `MiDisponibilidad()` GET
  - `EditarMiDisponibilidad()` GET/POST

✅ **Views/Tecnico/MiDisponibilidad.cshtml** - Vista lista
✅ **Views/Tecnico/EditarMiDisponibilidad.cshtml** - Vista lista

---

## PRÓXIMOS PASOS

1. **Elegir opción** (recomendado: OPCIÓN 1 con partial)
2. **Abrir** `CurlinggoSoft/Views/Shared/_Layout.cshtml`
3. **Localizar** el punto de inyección
4. **Agregar** el código correspondiente
5. **Ejecutar** `dotnet build`
6. **Probar:**
   - Navegar a `https://localhost:5001/Account/ChangePassword`
   - O buscar el menú "Mi Cuenta" en el navbar
   - Técnico: Ir a "Mi Disponibilidad"

---

## VERIFICACIÓN FINAL

Una vez agregado, deberías ver:
- ✅ Enlace "Cambiar Contraseña" en el menú autenticado
- ✅ Técnico puede acceder a "Mi Disponibilidad"
- ✅ Formulario requiere contraseña actual
- ✅ Si es correcto, cambia la contraseña y redirige a login
- ✅ Disponibilidad propia del técnico se puede editar

---

## NOTAS DE SEGURIDAD

- El cambio de contraseña NO GUARDA SESIÓN activa (requiere re-login)
- Solo **técnico autenticado** puede ver su disponibilidad
- **Admin** sigue viendo todas las disponibilidades (en DisponibilidadTecnicoController)
- Validaciones en FRONT + BACK

