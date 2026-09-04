# 🎯 ACTUALIZAR MENÚ - INSTRUCCIÓN EXACTA

## 🔴 PASO CRÍTICO - Dónde Copiar/Pegar

Archivo a editar: **`CurlinggoSoft/Views/Tecnico/Index.cshtml`**

(Como solo técnicos ven esta vista, NO necesitamos protección de rol)

---

## 📍 OPCIÓN 1 - SIMPLE (Recomendado)

### Busca esta línea en `_Layout.cshtml`:
```razor
@if (User.IsInRole("Tecnico"))
```

### Si existe, DENTRO de ese bloque, agrega esto:

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
	<!-- Aquí puedes tener otras opciones técnicas si las hay -->
}
```

**Resultado visual:**
```
Menú Técnico
├─ Zonas de Cobertura  ← Nueva opción
├─ [Otras opciones si existen]
└─ ...
```

---

## 📍 OPCIÓN 2 - DROPDOWN (Si quieres agrupar mejor)

### Busca:
```razor
@if (User.IsInRole("Tecnico"))
```

### Reemplaza COMPLETAMENTE ese bloque con esto:

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

			<!-- Opción 1: Panel Principal -->
			<li>
				<a class="dropdown-item" 
				   asp-controller="Tecnico" 
				   asp-action="Index">
					<i class="fa fa-home"></i> Mi Panel
				</a>
			</li>

			<!-- NUEVA OPCIÓN: Zonas de Cobertura -->
			<li>
				<a class="dropdown-item" 
				   asp-controller="Tecnico" 
				   asp-action="MisZonasCobertura">
					<i class="fa fa-map"></i> Zonas de Cobertura
				</a>
			</li>

			<!-- Opción 3: Disponibilidad -->
			<li>
				<a class="dropdown-item" 
				   asp-controller="Tecnico" 
				   asp-action="MiDisponibilidad">
					<i class="fa fa-calendar"></i> Mi Disponibilidad
				</a>
			</li>

			<li>
				<hr class="dropdown-divider">
			</li>

		</ul>
	</li>
}
```

**Resultado visual:**
```
▼ Técnico
  ├─ Mi Panel
  ├─ Zonas de Cobertura  ← Nueva opción aquí
  ├─ Mi Disponibilidad
  └─ ─────────────
```

---

## 📍 OPCIÓN 3 - SI NO EXISTE ESE BLOQUE

Si en tu `_Layout.cshtml` **NO existe** `@if (User.IsInRole("Tecnico"))`:

### Busca esta línea:
```razor
@Html.Partial("_MenuUsuarioAutenticado")
```

### JUSTO ANTES de esa línea, agrega:

```razor
<!-- OPCIONES PARA TÉCNICOS -->
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

<!-- Menú de Usuario -->
@Html.Partial("_MenuUsuarioAutenticado")
```

---

## ✅ VALIDACIÓN - Después de guardar

### 1. Guarda el archivo

### 2. Compila
```bash
dotnet clean
dotnet build
```
Debe terminar con: **✅ Build succeeded**

### 3. Ejecuta
```bash
dotnet run
```

### 4. En navegador
- Logúeate como técnico
- Abre menú
- Debes ver: **"Zonas de Cobertura"**
- Click: Navega a `/Tecnico/MisZonasCobertura`

---

## 🔍 ¿NO ENCUENTRAS DÓNDE EDITAR?

### Busca por estas palabras en `_Layout.cshtml`:

| Búsqueda | Ubicación |
|----------|-----------|
| `User.IsInRole("Tecnico")` | Aquí va la opción |
| `nav-item` | Dentro de este elemento |
| `_MenuUsuarioAutenticado` | Agregalo ANTES de esto |
| `navbar-nav` | En esta sección es |
| `MiDisponibilidad` | Otra opción técnica |

---

## 💡 AYUDA RÁPIDA

### "No veo User.IsInRole("Tecnico") en mi archivo"
→ Usa Opción 3 (agregar antes de _MenuUsuarioAutenticado)

### "Veo User.IsInRole("Tecnico") pero está vacío"
→ Usa Opción 1 (agregar dentro del bloque)

### "Quiero un menú bonito con dropdown"
→ Usa Opción 2 (reemplazar todo el bloque)

### "No sé dónde está el archivo"
→ Ruta: `CurlinggoSoft/Views/Shared/_Layout.cshtml`

---

## 🎯 RESUMO DE 3 OPCIONES

| Opción | Uso | Complejidad |
|--------|-----|------------|
| 1 - Simple | Lisa y llanamente | ⭐ Fácil |
| 2 - Dropdown | Agrupa opciones | ⭐⭐ Medio |
| 3 - Si no existe | Si no hay bloque técnico | ⭐ Fácil |

---

## ✨ CÓDIGO LISTO PARA COPIAR

### Versión Minimal (3 líneas):
```razor
<li class="nav-item">
	<a class="nav-link" asp-controller="Tecnico" asp-action="MisZonasCobertura">
		<i class="fa fa-map"></i> Zonas de Cobertura
	</a>
</li>
```

### Versión Full (con dropdown):
```razor
<li class="nav-item dropdown">
	<a class="nav-link dropdown-toggle" href="#" id="tecnicoDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
		<i class="fa fa-briefcase"></i> Técnico
	</a>
	<ul class="dropdown-menu" aria-labelledby="tecnicoDropdown">
		<li><a class="dropdown-item" asp-controller="Tecnico" asp-action="Index"><i class="fa fa-home"></i> Mi Panel</a></li>
		<li><a class="dropdown-item" asp-controller="Tecnico" asp-action="MisZonasCobertura"><i class="fa fa-map"></i> Zonas de Cobertura</a></li>
		<li><a class="dropdown-item" asp-controller="Tecnico" asp-action="MiDisponibilidad"><i class="fa fa-calendar"></i> Mi Disponibilidad</a></li>
	</ul>
</li>
```

---

## 📋 CHECKLIST FINAL

- [ ] Abriste `Views/Shared/_Layout.cshtml`
- [ ] Encontraste el lugar correcto (`User.IsInRole("Tecnico")` o `_MenuUsuarioAutenticado`)
- [ ] Copiaste una de nuestras opciones
- [ ] Pegaste el código
- [ ] Guardaste el archivo
- [ ] Ejecutaste `dotnet clean && dotnet build`
- [ ] Ejecutaste `dotnet run`
- [ ] El menú muestra "Zonas de Cobertura"
- [ ] Click en "Zonas de Cobertura" → Navega correctamente
- [ ] ✅ Listo!

---

## 🎉 ¡LISTO!

Después de hacer esto, solo falta:

```bash
# 1. Migración
dotnet ef migrations add AddTecnicoCobertura
dotnet ef database update

# 2. Compilar (ya lo hiciste arriba)
dotnet clean && dotnet build

# 3. Ejecutar y probar
dotnet run
```

---

**Ahora ve a: `CHECKLIST_FINAL.md` para los siguientes pasos** ✅

🚀
