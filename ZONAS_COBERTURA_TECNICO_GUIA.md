# Guía de Integración: Zonas de Cobertura del Técnico

## ✅ COMPLETADO

### 1. Modelo EF Core
- ✅ `CurlinggoSoft/Models/TecnicoCobertura.cs` - Creado con propiedades completas

### 2. DbContext
- ✅ `CurlinggoSoft/Models/ApplicationDbContext.cs` - Agregado `DbSet<TecnicoCobertura> TecnicoCoberturas`

### 3. Controlador (Partial Class)
- ✅ `CurlinggoSoft/Controllers/TecnicoController.Cobertura.cs` - Implementadas TODAS las acciones:
  - `MisZonasCobertura()` GET - Listar zonas
  - `AgregarZonaCobertura()` GET/POST - Crear zona
  - `EditarZonaCobertura()` GET/POST - Editar zona
  - `EliminarZonaCobertura()` POST - Eliminar zona
  - `DesactivarZonaCobertura()` POST - Desactivar sin eliminar
  - `ObtenerCantones()` GET (JSON) - Dropdown dinámico
  - `ObtenerDistritos()` GET (JSON) - Dropdown dinámico

### 4. Vistas Razor
- ✅ `CurlinggoSoft/Views/Tecnico/MisZonasCobertura.cshtml` - Listado con tarjetas
- ✅ `CurlinggoSoft/Views/Tecnico/AgregarZonaCobertura.cshtml` - Formulario con JS para dropdowns
- ✅ `CurlinggoSoft/Views/Tecnico/EditarZonaCobertura.cshtml` - Edición pre-poblada

---

## ⏳ PENDIENTE: Integración en el Menú

Necesitas agregar la opción "Zonas de Cobertura" al menú del técnico en:

### Archivo: `CurlinggoSoft/Views/Shared/_Layout.cshtml`

Busca la sección donde está:
```razor
@if (User.IsInRole("Tecnico"))
{
	<!-- Aquí irán las opciones del técnico -->
}
```

Y agrega esta opción junto a "Mi Disponibilidad", "Mis Reservas", etc.:

```razor
<!-- ZONAS DE COBERTURA -->
<li class="nav-item">
	<a class="nav-link" 
	   asp-controller="Tecnico" 
	   asp-action="MisZonasCobertura">
		<i class="fa fa-map"></i> Zonas de Cobertura
	</a>
</li>
```

---

## 🔧 PASOS FINALES

### 1. Actualizar el Layout
Busca en `_Layout.cshtml` la sección de menú para técnicos y agrega la opción anterior.

### 2. Crear Migración EF Core
```bash
cd CurlinggoSoft
dotnet ef migrations add AddTecnicoCobertura
dotnet ef database update
```

### 3. Compilar y Probar
```bash
dotnet clean
dotnet build
dotnet run
```

### 4. Probar en Navegador
1. Loguearse como técnico
2. En el menú, ir a "Zonas de Cobertura"
3. Probar agregar, editar, desactivar y eliminar zonas

---

## 📋 Checklist de Seguridad

✅ Cada acción valida `TecnicoID` del usuario logueado
✅ Retorna `Unauthorized()` si intenta acceder a zonas ajenas
✅ `[ValidateAntiForgeryToken]` en todos los POST
✅ `[Authorize(Roles = "Tecnico")]` en todo el controlador
✅ Validación de duplicados al agregar zona

---

## 🗺️ Rutas de la Aplicación

- `/Tecnico/MisZonasCobertura` - Listar todas las zonas
- `/Tecnico/AgregarZonaCobertura` - Nueva zona (GET/POST)
- `/Tecnico/EditarZonaCobertura/{id}` - Editar zona (GET/POST)
- `/Tecnico/EliminarZonaCobertura/{id}` - Eliminar zona (POST)
- `/Tecnico/DesactivarZonaCobertura/{id}` - Desactivar zona (POST)
- `/Tecnico/ObtenerCantones?provinciaId=X` - API JSON para dropdown (GET)
- `/Tecnico/ObtenerDistritos?cantonId=X` - API JSON para dropdown (GET)

---

## 📊 Estructura de Datos

**Tabla: TecnicoCobertura**
```sql
CREATE TABLE TecnicoCobertura (
	TecnicoCoberturaID BIGINT PRIMARY KEY IDENTITY,
	TecnicoID NVARCHAR(450) FOREIGN KEY,
	ProvinciaID INT FOREIGN KEY,
	CantonID INT FOREIGN KEY,
	DistritoID INT FOREIGN KEY (NULL),
	RadioCoberturaKm DECIMAL(5,2) (NULL),
	Activa BIT DEFAULT 1,
	FechaCreacion DATETIME DEFAULT GETDATE()
)
```

---

## 🎨 Características de la UI

✨ Tarjetas Bootstrap con estados visuales
✨ Dropdown dinámicos (Provincia → Cantón → Distrito)
✨ Indicadores de zona activa/inactiva con badges
✨ Botones de acción: Editar, Desactivar, Eliminar
✨ Mensajes de éxito y error con TempData
✨ Formularios con validación del lado cliente
✨ Fecha de creación formateada (dd/MM/yyyy HH:mm)

---

## 🚀 Próximos Pasos (Opcional)

1. Agregar búsqueda/filtrado de zonas
2. Mostrar número de solicitudes por zona
3. Gráfico de cobertura en mapa
4. Importar múltiples zonas desde CSV
5. Reportes de zonas activas vs. inactivas
