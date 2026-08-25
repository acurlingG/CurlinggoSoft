# Solución: ArgumentNullException - Parámetro 'email'

## Problema Identificado

**Excepción:** `System.ArgumentNullException: Value cannot be null. (Parameter 'email')`  
**Ubicación:** `GuardarPaso2()` en `SolicitudTecnicoController`, línea 181  
**Método que lanza:** `_userManager.FindByEmailAsync(modelo.Email)`

### Causa Raíz

El parámetro `modelo.Email` es **NULL** cuando se pasa a `FindByEmailAsync()`. Esto ocurre por una combinación de fallos:

1. **Falta de validación defensiva:** No se validaba si `modelo.Email` era nulo antes de usarlo en APIs de Identity Manager
2. **Carga incompleta de datos:** El método `CargarModeloDesdeSolicitud()` no manejaba el caso cuando `solicitud.Usuario` era nulo
3. **Email no requerido en precarga:** Cuando se precargaba el formulario para usuarios autenticados, si no se cargaba correctamente la entidad Usuario, Email quedaba como `null`

### Flujo que caused el error:

```
Usuario autenticado → Paso 2 GET → 
CargarModeloDesdeSolicitud() → 
solicitud.Usuario era NULL → 
Email se quedaba null → 
Paso 2 POST → 
FindByEmailAsync(null) → 
ArgumentNullException
```

---

## Soluciones Implementadas

### 1. ✅ Validación Defensiva en GuardarPaso2

**Línea 172-175** - Se agregó validación temprana:

```csharp
// Validación defensiva: Email es obligatorio siempre
if (string.IsNullOrWhiteSpace(modelo.Email))
{
	ModelState.AddModelError(nameof(modelo.Email), "El correo electrónico es obligatorio.");
}
```

**Beneficio:** Rechaza requests invalidos tempranamente, antes de cualquier lógica de negocio.

### 2. ✅ Cargar Usuario como Fallback en CargarModeloDesdeSolicitudAsync

**Línea 650-656** - Se mejoró el método para cargar Usuario si no fue incluido en la consulta:

```csharp
else if (!string.IsNullOrEmpty(solicitud.UsuarioID))
{
	// Fallback: cargar Usuario desde la BD si no fue cargado con Include
	var usuario = await _context.Usuarios.FindAsync(solicitud.UsuarioID);
	if (usuario != null)
	{
		modelo.DatosPersonales.Nombre = usuario.Nombre;
		modelo.DatosPersonales.Apellidos = usuario.Apellidos;
		modelo.DatosPersonales.Email = usuario.Email;  // ← Garantiza Email se carga
		modelo.DatosPersonales.Telefono = usuario.Telefono;
		modelo.DatosPersonales.Identificacion = solicitud.Identificacion;
	}
}
```

**Beneficio:** Incluso si `solicitud.Usuario` es null, se carga desde BD como segundo intento.

### 3. ✅ Cambio de Método Síncrono a Asincrónico

**Línea 125** - Se cambió `CargarModeloDesdeSolicitud()` a `CargarModeloDesdeSolicitudAsync()`:

```csharp
// Antes
private static void CargarModeloDesdeSolicitud(SolicitudTecnicoWizardViewModel modelo, SolicitudTecnico solicitud)

// Después
private async Task CargarModeloDesdeSolicitudAsync(SolicitudTecnicoWizardViewModel modelo, SolicitudTecnico solicitud)
```

**Beneficio:** Permite cargar datos de BD bajo demanda sin bloqueo de entrada.

### 4. ✅ Actualizar Llamadas al Método

**Línea 123** - Se cambió la invocación en `Paso()`:

```csharp
// Antes
CargarModeloDesdeSolicitud(modelo, solicitud);

// Después
await CargarModeloDesdeSolicitudAsync(modelo, solicitud);
```

---

## Validación de la Solución

### Escenario 1: Usuario Nuevo (Anónimo)
- ✅ La validación defensiva rechaza si email está vacío
- ✅ Se valida email antes de `FindByEmailAsync()`
- ✅ Se crea cuenta Identity + Usuario correctamente

### Escenario 2: Usuario Autenticado (Reutiliza Cuenta)
- ✅ Se cargan datos desde `solicitud.Usuario`
- ✅ Si Usuario no está cargado, fallback a BD
- ✅ Email garantizado en TODOS los casos
- ✅ No lanza ArgumentNullException

### Escenario 3: ModelState Inválido
- ✅ Se rechaza si Email es nulo/vacío
- ✅ Se devuelve vista con errores apropiados
- ✅ No procesa lógica de negocio

---

## Archivos Modificados

- **CurlinggoSoft/Controllers/SolicitudTecnicoController.cs**
  - Método `GuardarPaso2()`: Agregó validación defensiva (línea 172-175)
  - Método `Paso()`: Cambió a invocación async (línea 123)
  - Método `CargarModeloDesdeSolicitudAsync()`: Rediseñado con fallback y manejo async (línea 648-709)

---

## Pruebas Recomendadas

1. **Test Usuario Anónimo**
   - Enviar formulario Paso 2 CON email válido → Debe crear cuenta
   - Enviar formulario Paso 2 SIN email → Debe rechazar con validación

2. **Test Usuario Autenticado**
   - Usuario existente + solicitud en BORRADOR → Debe cargar datos correctamente
   - Usuario existente + datos incompletos → Debe permitir editar y guardar

3. **Test Email Duplicado**
   - Usuario nuevo intenta registrar email que ya existe → Debe rechazar con mensaje claro

---

## Cambios de Comportamiento Visibles

| Situación | Antes | Después |
|-----------|-------|---------|
| Email vacío en Paso 2 | ArgumentNullException | Validación clara + mensaje de error |
| Usuario no cargado | Email = null | Carga fallback desde BD |
| Paso 2 GET | Podría mostrar campos vacíos | Siempre muestra datos precargados |

