# 🎯 INSTRUCCIÓN EXACTA: AGREGAR AL MENÚ

## Problemática:
No pudimos leer las líneas exactas de `_Layout.cshtml` donde existe el menú técnico.

## Solución:
Aquí hay 3 opciones que deberían funcionar. **Eligee una según tu estructura:**

---

## OPCIÓN 1️⃣ - SI EXISTE "@if (User.IsInRole("Tecnico"))" YA EN EL LAYOUT

En `CurlinggoSoft/Views/Shared/_Layout.cshtml`

**Busca esta línea:**
```razor
@if (User.IsInRole("Tecnico"))
{
```

**Agrega dentro cualquiera de estas dos cosas:**

### A) Simple (una sola línea):
```razor
@if (User.IsInRole("Tecnico"))
{
	<li class="nav-item">
		<a class="nav-link" 
		   asp-controller="Tecnico" 
		   asp-action="MisZonasCobertura">
			<i class="fa fa-map"></i> Zonas de Cobertura
		</a>
	</li>
}
```

### B) Con Dropdown (si quieres agrupar opciones técnicas):
```razor
@if (User.IsInRole("Tecnico"))
{
	<li class="nav-item dropdown">
		<a class="nav-link dropdown-toggle" 
		   href="#" 
		   id="tecnicoDropdown" 
		   role="button" 
		   data-bs-toggle="dropdown" 
		   aria-expanded="false">
			<i class="fa fa-briefcase"></i> Técnico
		</a>
		<ul class="dropdown-menu" aria-labelledby="tecnicoDropdown">
			<li>
				<a class="dropdown-item" 
				   asp-controller="Tecnico" 
				   asp-action="Index">
					<i class="fa fa-home"></i> Mi Panel
				</a>
			</li>
			<li>
				<a class="dropdown-item" 
				   asp-controller="Tecnico" 
				   asp-action="MisZonasCobertura">
					<i class="fa fa-map"></i> Zonas de Cobertura
				</a>
			</li>
			<li>
				<hr class="dropdown-divider">
			</li>
			<li>
				<a class="dropdown-item" 
				   asp-controller="Tecnico" 
				   asp-action="MiDisponibilidad">
					<i class="fa fa-calendar"></i> Mi Disponibilidad
				</a>
			</li>
		</ul>
	</li>
}
```

---

## OPCIÓN 2️⃣ - SI NO EXISTE, AGRÉGALO ANTES DEL MEN DE USUARIO

En `CurlinggoSoft/Views/Shared/_Layout.cshtml`

**Busca esta línea (algo como):**
```razor
@Html.Partial("_MenuUsuarioAutenticado")
```

**JUSTO ANTES, agrega esto:**

```razor
<!-- MENU TÉCNICO -->
@if (User.IsInRole("Tecnico"))
{
	<li class="nav-item">
		<a class="nav-link" 
		   asp-controller="Tecnico" 
		   asp-action="MisZonasCobertura">
			<i class="fa fa-map"></i> Zonas de Cobertura
		</a>
	</li>
}

<!-- Menu Usuario - Cambio de Contraseña, Logout -->
@Html.Partial("_MenuUsuarioAutenticado")
```

---

## OPCIÓN 3️⃣ - BÚSQUEDA CON KEYWORDS

**Si no encuentra, busca en el archivo por estos textos:**

- Busca: `User.IsInRole("Tecnico")`
  → Aquí agregar opción técnica

- Busca: `MiDisponibilidad`
  → Agregar "Zonas de Cobertura" al lado

- Busca: `navbar-nav`
  → Dentro de este elemento agregar nuestro `<li>`

---

## ✅ VALIDACIÓN: DESPUÉS DE GUARDAR

Después de hacer el cambio:

1. **Guarda el archivo**

2. **Compila:**
   ```bash
   dotnet clean
   dotnet build
   ```
   Debe compilar sin errores

3. **Ejecuta:**
   ```bash
   dotnet run
   ```

4. **En navegador:**
   - Logueate como técnico
   - Abre menú
   - Debes ver "Zonas de Cobertura"
   - Click: debe ir a `/Tecnico/MisZonasCobertura`

---

## 🔍 SI AÚN NO FUNCIONA

**Plan B:** Manda captura de pantalla de dónde está el menú técnico actual en tu `_Layout.cshtml` y te digo exactamente dónde copiar/pegar.

**Líneas a buscar que puen indicar:**
- `nav-item`
- `MiDisponibilidad`
- `asp-controller="Tecnico"`
- `TecnicoDropdown`
- `Role("Tecnico")`

---

## 💡 NOTAS IMPORTANTES

✅ El `<i class="fa fa-map"></i>` es el icono (mapa)
✅ El `asp-controller="Tecnico"` y `asp-action="MisZonasCobertura"` generan la URL automáticamente
✅ El `data-bs-toggle="dropdown"` hace que sea dropdown (Bootstrap 5)
✅ Todo está en español pero el código en inglés (convención ASP.NET)

---

## 🎯 RESUMEN

| Paso | Acción |
|------|--------|
| 1 | Abre `Views/Shared/_Layout.cshtml` |
| 2 | Busca `@if (User.IsInRole("Tecnico"))` O `_MenuUsuarioAutenticado` |
| 3 | Copia una de nuestras opciones (A o B, o C) |
| 4 | Pega en el lugar correcto |
| 5 | Guarda el archivo |
| 6 | Compila y prueba |

---

¡Eso es! 🚀
