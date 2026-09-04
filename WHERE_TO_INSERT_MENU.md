# 🎯 WHERE TO INSERT: Menú de Zonas de Cobertura

En `CurlinggoSoft/Views/Shared/_Layout.cshtml`

## OPCIÓN 1: SI EXISTE "@if (User.IsInRole("Tecnico"))"

Busca esta sección y agrega la línea dentro:

```razor
@if (User.IsInRole("Tecnico"))
{
	<!-- ZONAS DE COBERTURA - AGREGAR ESTA LÍNEA -->
	<li class="nav-item">
		<a class="nav-link" 
		   asp-controller="Tecnico" 
		   asp-action="MisZonasCobertura">
			<i class="fa fa-map"></i> Zonas de Cobertura
		</a>
	</li>
}
```

---

## OPCIÓN 2: AGREGAR DROPDOWN COM PLETO (RECOMENDADO)

Si quieres agrupar todas las opciones técnicas en un dropdown, busca dónde está:

```razor
@if (User.IsInRole("Tecnico"))
{
```

Y usa este bloque completo:

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
		<ul class="dropdown-menu dropdown-menu-end" aria-labelledby="tecnicoDropdown">
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

## OPCIÓN 3: UBICACIÓN ALTERNATIVA - DENTRO DEL NAVBAR

Si no existe la sección de Tecnico, busca estas líneas:

```razor
@Html.Partial("_MenuUsuarioAutenticado")
```

Justo ANTES de esa línea, inserta:

```razor
<!-- MENU PARA TÉCNICOS - Agregar esto -->
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

---

## 📍 LÍNEA DE CONTEXTO PARA BUSCAR

En `_Layout.cshtml`, busca este comentario o línea:

```
<!-- MEN</UTF></UTF></UTF> DE USUARIO AUTENTICADO -->
```

O busca: `_MenuUsuarioAutenticado`

Debería estar alrededor de la línea 350-400 en el archivo.

---

## ✅ VERIFICAR QUE FUNCIONA

Después de agregar, compila y:

1. Logueate como técnico
2. El menú debe mostrar "Zonas de Cobertura"
3. Click debe llevar a `/Tecnico/MisZonasCobertura`

---

## 🚨 SI NO VES DÓNDE AGREGAR

Manda el mensaje:
> "No encuentro dónde insertar en _Layout.cshtml, mándame el archivo completo actualizado"

Y te lo envío listo para copiar/pegar
