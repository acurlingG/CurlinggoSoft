# Agregación de Menú para Zonas de Cobertura

## 🎯 Ubicación Exacta en _Layout.cshtml

En el archivo `CurlinggoSoft/Views/Shared/_Layout.cshtml`, busca la sección que contiene:

```razor
@if (User.IsInRole("Tecnico"))
{
	<!-- Tus trabajos asignados, ofertas, etc. -->
}
```

Si NO existe esa sección (más probable), busca la línea:

```razor
@* Menu de Usuario Autenticado *@
@Html.Partial("_MenuUsuarioAutenticado")
```

Justo **ANTES** de esa línea (o dentro de la navbar), agrega:

---

## ✅ Fragmento a Copiar/Pegar

```razor
<!-- MENU PARA TÉCNICOS -->
@if (User.IsInRole("Tecnico"))
{
	<li class="nav-item dropdown">
		<a class="nav-link dropdown-toggle" 
		   href="#" 
		   id="tecnicoDropdown" 
		   role="button" 
		   data-bs-toggle="dropdown" 
		   aria-expanded="false">
			<i class="fa fa-briefcase"></i> Panel Técnico
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
				   asp-action="OfertasDisponibles">
					<i class="fa fa-star"></i> Ofertas Disponibles
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
			<li>
				<a class="dropdown-item" 
				   asp-controller="Tecnico" 
				   asp-action="MisZonasCobertura">
					<i class="fa fa-map"></i> Zonas de Cobertura
				</a>
			</li>
		</ul>
	</li>
}
```

---

## 📍 Alternativa Simple (Sin Dropdown)

Si prefieres solo añadir líneas simples, agrega estas 3 líneas juntas:

```razor
<li class="nav-item">
	<a class="nav-link" asp-controller="Tecnico" asp-action="MisZonasCobertura">
		<i class="fa fa-map"></i> Zonas de Cobertura
	</a>
</li>
```

---

## 🎁 Opción Completa para Regalo

Si quieres que vuelva a copiar el archivo `_Layout.cshtml` completo actualizado, solo dime y lo haré.
