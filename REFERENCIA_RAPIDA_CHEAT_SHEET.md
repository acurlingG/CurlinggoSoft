# 🎯 REFERENCIA RÁPIDA - CHEAT SHEET

## ⚡ TL;DR (TOO LONG; DIDN'T READ)

### En 30 segundos:
1. **Compilar:** `dotnet build`
2. **Ejecutar:** `dotnet run`
3. **Probar:**
   - Cambiar contraseña: Navbar → "Cambiar Contraseña"
   - Mi disponibilidad (técnico): Navbar → "Mi Panel" → "Mi Disponibilidad"

---

## 📍 UBICACIONES CLAVE

| Qué | Dónde |
|-----|-------|
| **Cambiar Contraseña** | `/Account/ChangePassword` |
| **Mi Disponibilidad (Técnico)** | `/Tecnico/MiDisponibilidad` |
| **Editar Disponibilidad** | `/Tecnico/EditarMiDisponibilidad/{id}` |
| **Admin ver todas** | `/DisponibilidadTecnico/Index` |
| **Cerrar sesión** | POST `/Account/Logout` |

---

## 👤 ACCIONES POR ROL

### Admin
```
✅ Cambiar su contraseña
✅ Ver toda disponibilidad de técnicos
✅ Editar disponibilidad de técnicos
❌ Ver "Mi Disponibilidad" (no tiene)
```

### Cliente
```
✅ Cambiar su contraseña
✅ Ver sus direcciones
✅ Ver sus reservas
❌ Ver disponibilidad de técnicos
❌ Acceder a Mi Disponibilidad
```

### Técnico
```
✅ Cambiar su contraseña
✅ Ver SU PROPIA disponibilidad
✅ Editar SU PROPIA disponibilidad
✅ Ver ofertas disponibles
❌ Ver disponibilidad de otros técnicos
❌ Ver panel admin
```

---

## 🔒 VALIDACIONES CLAVE

### Cambiar Contraseña
```
✅ Contraseña actual: DEBE ser correcta
✅ Longitud mínima: 6 caracteres
✅ Coincidencia: Nueva debe = Confirmar
❌ Sin estos, error
```

### Mi Disponibilidad
```
✅ Solo accesible para TÉCNICO autenticado
✅ Solo ve/edita SU disponibilidad (por TecnicoID)
✅ Día de semana: NO editable
✅ Hora inicio/fin: SÍ editables
✅ Estado: SÍ editable (activa/inactiva)
```

---

## 🗂️ ARCHIVOS IMPORTANTES

```
Controllers/
├── AccountController.cs
│   ├── ChangePassword() GET
│   ├── ChangePassword() POST
│   └── Logout() POST
│
└── TecnicoController.Disponibilidad.cs
	├── MiDisponibilidad() GET
	└── EditarMiDisponibilidad() GET/POST

Views/
├── Account/ChangePassword.cshtml
├── Tecnico/
│   ├── MiDisponibilidad.cshtml
│   └── EditarMiDisponibilidad.cshtml
└── Shared/_Layout.cshtml (MODIFICADO)
```

---

## 🐛 ERRORES COMUNES Y SOLUCIONES

| Error | Causa | Solución |
|-------|-------|----------|
| "Contraseña actual incorrecta" | Escribiste mal | Intenta de nuevo |
| "Las contraseñas no coinciden" | Nueva ≠ Confirmar | Escribe igual en ambos |
| "Mínimo 6 caracteres" | Contraseña muy corta | Usa 6+ caracteres |
| 403 Forbidden | Acceso no autorizado | Verifica rol correcto |
| Enlace no aparece | No autenticado | Inicia sesión primero |
| Error 500 | Bug en servidor | Revisa logs Visual Studio |

---

## ✅ TESTS RÁPIDOS

### Test 1: Cambiar Contraseña (1 min)
```
1. Login como client@curlinggo.com / ClientPassword123!
2. Click "Cambiar Contraseña"
3. Ingresa: ClientPassword123! → NewPass123! → NewPass123!
4. Click "Cambiar Contraseña"
5. Login con NewPass123!
✅ Si funciona: OK
❌ Si no funciona: Revisa AccountController
```

### Test 2: Mi Disponibilidad (1 min)
```
1. Login como tecnico@curlinggo.com / TecnicoPassword123!
2. Click "Mi Panel" → "Mi Disponibilidad"
3. Click "Editar" en un horario
4. Cambia hora inicio 08:00 → 09:00
5. Click "Guardar Cambios"
✅ Si funciona: OK
❌ Si no funciona: Revisa TecnicoController.Disponibilidad.cs
```

### Test 3: Seguridad (1 min)
```
1. Como CLIENTE, accede a /Tecnico/MiDisponibilidad
✅ Debe dar error (correcto)

2. Logout, accede a /Account/ChangePassword
✅ Debe ir a Login (correcto)

3. Como TÉCNICO, accede a /DisponibilidadTecnico/Index
✅ Debe dar error (correcto)
```

---

## 🚨 DEBUGGING RÁPIDO

### Cambiar Contraseña no funciona
```
1. Verifica AccountController.cs tenga los 3 métodos
2. Verifica ChangePassword.cshtml exista
3. Limpia cache: Ctrl+Shift+Del
4. Reconstruye: dotnet clean && dotnet build
5. Revisa Visual Studio Output window
```

### Mi Disponibilidad no aparece
```
1. Verifica TecnicoController.Disponibilidad.cs exista
2. Verifica MiDisponibilidad.cshtml exista
3. Verifica estés autenticado como TÉCNICO
4. Verifica _Layout.cshtml apunte a TecnicoController
5. Revisa that el usuario tenga rol "Tecnico"
```

### Navbar no se actualiza
```
1. Recarga página: F5
2. Limpiar caché: Ctrl+Shift+Del
3. Close navegador completamente
4. Abre en nueva pestaña/ventana
5. Try diferente navegador
```

---

## 🔄 FLUJO TÍPICO DE CAMBIO DE CONTRASEÑA

```
Usuario → "Cambiar Contraseña" → Ingresa datos → Valida
   → Cambia en DB → Email alerta → Logout → Login
   → Re-autentica → Sesión nueva
```

---

## 🔄 FLUJO TÍPICO DE EDITAR DISPONIBILIDAD

```
Técnico → "Mi Disponibilidad" → Tabla → "Editar"
   → Formulario → Cambios → "Guardar" → Valida
   → Update DB → Mensaje éxito → Regresa a tabla
```

---

## 📱 RESPONSIVE (Móvil)

✅ Formularios adaptables
✅ Tablas con scroll horizontal
✅ Navbar colapsa en mobile
✅ Botones táctiles grandes

---

## 🌐 NAVEGADORES COMPATIBLES

✅ Chrome 100+
✅ Firefox 100+
✅ Safari 15+
✅ Edge 100+
❌ IE 11 (no soportado)

---

## 📞 CONTACTO RÁPIDO

- **Bug en AccountController:** Revisa cambio de contraseña
- **Bug en TecnicoController:** Revisa disponibilidad técnico
- **Bug en NavBar:** Revisa _Layout.cshtml
- **General:** Consulta `CHECKLIST_VERIFICACION_FINAL.md`

---

## 🎓 APRENDE MÁS

- `IMPLEMENTACION_FINAL_COMPLETADA.md` → Guía completa
- `GUIA_RESOLUCION_FINAL_LAYOUT.md` → Detalles navbar
- `PASOS_FINALES_COMPILAR_PROBAR.md` → Paso a paso
- `CHECKLIST_VERIFICACION_FINAL.md` → 50+ pruebas

---

## ⚡ COMANDOS CLI IMPORTANTES

```bash
# Compilar
dotnet build

# Ejecutar
dotnet run

# Limpiar
dotnet clean

# Limpiar caché
rm -r bin obj

# Ver errores
dotnet build --verbose

# Restaurar paquetes
dotnet restore
```

---

## 📊 RESUMEN FINAL

```
IMPLEMENTADO:
✅ Cambiar Contraseña → Funcional + Seguro
✅ Mi Disponibilidad → Solo técnico ve la suya
✅ Navbar Integrado → Enlaces agregados/corregidos

VALIDACIONES:
✅ Backend: UserManager, Authorize, Roles
✅ Frontend: JavaScript, Bootstrap, validaciones UI

DOCUMENTACIÓN:
✅ 5 guías completas
✅ Tests verificados
✅ Ejemplo de uso

ESTADO:
✅✅✅ LISTO PARA PRODUCCIÓN
```

---

**Última actualización:** [Hoy]  
**Versión:** 1.0  
**Status:** ✅ COMPLETADO

