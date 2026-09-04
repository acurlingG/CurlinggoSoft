# 🔨 COMPILAR Y PROBAR - EDITAR/BORRAR COMPLETO

**Estado:** Listo para compilar  
**Tiempo Estimado:** 10 minutos  
**Complejidad:** Media

---

## ⚡ PASO 1: COMPILAR

### Abre Terminal en la carpeta del proyecto

```bash
cd C:\Users\CURLING\source\repos\CurlinggoSoft\CurlinggoSoft
```

### Limpia compilaciones anteriores

```bash
dotnet clean
```

Espera 10-30 segundos.

### Compila el proyecto

```bash
dotnet build
```

### ✅ Resultado Esperado

```
========== Recompilar todo: 3 correcto, 0 con errores ===========
========== Recompilar completado a las [hora]
```

Si ves **"Build succeeded"** → ✅ Continúa al Paso 2

Si ves **errores** → Reporta el mensaje exacto

---

## 🚀 PASO 2: EJECUTAR

```bash
dotnet run
```

### ✅ Esperado

```
Now listening on: https://localhost:5298
Application started. Press Ctrl+C to shut down.
```

---

## 🧪 PASO 3: PRUEBAS EN NAVEGADOR

### Abre: https://localhost:5298

Inicia sesión con un usuario cliente o técnico.

---

## 📋 PRUEBA 1: CLIENTE - MIS DIRECCIONES

### URL
```
https://localhost:5298/Cliente/MisDirecciones
```

### Verifica que veas:
- [ ] Tarjetas de direcciones con información completa
- [ ] Botón "Editar" (amarillo)
- [ ] Botón "Deshabilitar" (gris)
- [ ] Botón "Eliminar" (rojo)

### Prueba: Editar Dirección
1. Haz click en "Editar" de cualquier dirección
2. Deberías llegar a: `/Cliente/EditarDireccion/{id}`
3. Modifica uno o más campos:
   - Nombre de dirección
   - Provincia
   - Cantón
   - Distrito
   - Dirección exacta
   - Checkbox "Activa"
4. Haz click "Guardar Cambios"
5. **Esperado:** 
   - ✅ Mensaje de éxito
   - ✅ Redirigido a MisDirecciones
   - ✅ Cambios guardados en BD

### Prueba: Deshabilitar Dirección
1. Haz click en "Deshabilitar" 
2. Confirma en el modal
3. **Esperado:**
   - ✅ Mensaje "Dirección deshabilitada"
   - ✅ Dirección desaparece de la lista (porque solo muestra activas)
   - ✅ La dirección NO se elimina de BD

### Prueba: Eliminar Dirección
1. Haz click en "Eliminar"
2. Confirma en el modal
3. **Esperado:**
   - ✅ Mensaje "Dirección eliminada"
   - ✅ Dirección desaparece de la lista
   - ✅ Se elimina de BD definitivamente

---

## 📋 PRUEBA 2: TÉCNICO - MI DISPONIBILIDAD

### URL
```
https://localhost:5298/Tecnico/MiDisponibilidad
```

### Verifica que veas:
- [ ] Botón "Agregar Nueva Disponibilidad" (verde)
- [ ] Tabla con disponibilidades existentes
- [ ] Botón "Editar" en cada fila
- [ ] Botón "Eliminar" en cada fila
- [ ] Estados (Activa/Inactiva) como badges

### Prueba: Agregar Disponibilidad
1. Haz click en "Agregar Nueva Disponibilidad"
2. Deberías llegar a: `/Tecnico/AgregarDisponibilidad`
3. Completa el formulario:
   - Día de la semana: Selecciona uno (Lunes-Domingo)
   - Hora Inicio: Ej: 08:00
   - Hora Fin: Ej: 17:00
   - Activa: Checkbox marcado
4. Haz click "Agregar Disponibilidad"
5. **Esperado:**
   - ✅ Mensaje de éxito
   - ✅ Nueva fila aparece en la tabla
   - ✅ Se muestra en el formato correcto

### Validación: Hora Fin Menor que Inicio
1. Intenta agregar:
   - Hora Inicio: 15:00
   - Hora Fin: 10:00
2. Haz click "Agregar"
3. **Esperado:**
   - ✅ Error: "La hora de fin debe ser posterior a la de inicio"
   - ✅ Se queda en el formulario

### Prueba: Editar Disponibilidad
1. Haz click en "Editar" de cualquier fila
2. Modifica hora inicio o hora fin
3. Haz click "Guardar Cambios"
4. **Esperado:**
   - ✅ Mensaje de éxito
   - ✅ Tabla actualizada con nuevos datos
   - ✅ Cambios en BD

### Prueba: Eliminar Disponibilidad
1. Haz click en "Eliminar" en una fila
2. Confirma en el modal
3. **Esperado:**
   - ✅ Mensaje "Disponibilidad eliminada"
   - ✅ Fila desaparece de la tabla
   - ✅ Se elimina de BD

---

## 🔒 PRUEBAS DE SEGURIDAD

### Prueba: Cliente NO puede editar dirección de otro
1. Copia la URL de edición: `/Cliente/EditarDireccion/123`
2. Cambia el ID por uno que NO sea del cliente actual
3. Intenta acceder directamente
4. **Esperado:** ✅ Error 401 Unauthorized o redirigido

### Prueba: Técnico NO puede editar disponibilidad de otro
1. Copia la URL: `/Tecnico/EditarMiDisponibilidad/123`
2. Cambia el ID por uno que NO sea del técnico actual
3. Intenta acceder
4. **Esperado:** ✅ Error 401 Unauthorized

---

## ✅ CHECKLIST DE PRUEBAS

```
COMPILACIÓN:
- [ ] dotnet build exitoso
- [ ] 0 errores
- [ ] 0 warnings (o warnings menores sin impacto)

CLIENTE - MIS DIRECCIONES:
- [ ] Página carga correctamente
- [ ] Se ven todas las direcciones
- [ ] Botón Editar funciona
- [ ] Botón Deshabilitar funciona
- [ ] Botón Eliminar funciona
- [ ] Editar guarda correctamente
- [ ] Deshabilitar no elimina la dirección
- [ ] Eliminar borra permanentemente
- [ ] Confirmación de eliminación funciona
- [ ] Mensajes de éxito/error aparecen

TÉCNICO - MI DISPONIBILIDAD:
- [ ] Página carga correctamente
- [ ] Se ven todas las disponibilidades
- [ ] Botón Agregar funciona
- [ ] Botón Editar funciona
- [ ] Botón Eliminar funciona
- [ ] Agregar crea nueva entrada
- [ ] Editar actualiza correctamente
- [ ] Eliminar borra permanentemente
- [ ] Validación de horas funciona
- [ ] Confirmación de eliminación funciona
- [ ] Mensajes de éxito/error aparecen

SEGURIDAD:
- [ ] Cliente solo ve sus direcciones
- [ ] Técnico solo ve su disponibilidad
- [ ] No se puede acceder URL de otro usuario
- [ ] Anti-CSRF funcionando

RESULTADO:
- [ ] TODOS los checkboxes marcados = ✅ ÉXITO
```

---

## 🐛 TROUBLESHOOTING

### Error: "Build failed"
```
Solución:
1. dotnet clean
2. Espera a que termine
3. dotnet build
4. Si persiste, revisa el error exacto
```

### Error 404 en EditarDireccion.cshtml
```
Significa que falta la vista. Pero ya la creé, así que:
1. Verifica que exista: Views/Cliente/EditarDireccion.cshtml
2. Presiona Ctrl+S en Visual Studio (guardar todos)
3. Intenta de nuevo
```

### Error 404 en AgregarDisponibilidad.cshtml
```
Igual que arriba:
1. Verifica: Views/Tecnico/AgregarDisponibilidad.cshtml
2. Guarda todos los archivos
3. Reinicia dotnet run
```

### Error: "Posible argumento nulo"
```
Esto es un warning, no error. No afecta la funcionalidad.
Solo indica que el sistema sugiere más validaciones.
```

### La dirección no se actualiza
```
Causas posibles:
1. No presionaste guardar correctamente
2. Hay un error de validación no visible
3. El usuario no es propietario de la dirección
4. Falta CSRF token en el formulario

Solución:
1. Abre el navegador F12 (consola)
2. Mira errores
3. Reporta el error exacto
```

---

## 📊 RESUMEN DE RUTAS

| Ruta | Método | Descripción |
|------|--------|-------------|
| /Cliente/MisDirecciones | GET | Listar direcciones |
| /Cliente/EditarDireccion/{id} | GET/POST | Editar dirección |
| /Cliente/DeshabilitarDireccion/{id} | POST | Deshabilitar dirección |
| /Cliente/EliminarDireccion/{id} | POST | Eliminar dirección |
| /Tecnico/MiDisponibilidad | GET | Listar disponibilidades |
| /Tecnico/AgregarDisponibilidad | GET/POST | Agregar disponibilidad |
| /Tecnico/EditarMiDisponibilidad/{id} | GET/POST | Editar disponibilidad |
| /Tecnico/EliminarDisponibilidad/{id} | POST | Eliminar disponibilidad |

---

## 🎯 CONCLUSIÓN

```
┌────────────────────────────────────────┐
│   COMPILACION LISTA PARA PROBAR        │
│                                        │
│  Paso 1: dotnet clean && dotnet build  │
│  Paso 2: dotnet run                    │
│  Paso 3: Pruebas en navegador          │
│                                        │
│  Tiempo: ~5-10 minutos                 │
│                                        │
│  🚀 ¡COMIENZA AHORA!                   │
│                                        │
└────────────────────────────────────────┘
```

---

**Instrucciones:** Sigue los pasos en orden.  
**Reporta:** Cualquier error o problema.  
**Valida:** Usa el checklist para confirmar todo funciona.

