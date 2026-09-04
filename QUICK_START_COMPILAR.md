# ⚡ QUICK START - COMPILAR Y PROBAR YA

**Tiempo Total:** ~10 minutos  
**Pasos:** 5  
**Dificultad:** Fácil

---

## 1️⃣ ABRE TERMINAL

```bash
# Windows PowerShell o CMD
cd C:\Users\CURLING\source\repos\CurlinggoSoft\CurlinggoSoft
```

---

## 2️⃣ LIMPIA Y COMPILA

```bash
dotnet clean && dotnet build
```

⏳ Espera ~2-3 minutos

### ✅ Esperado: 
```
========== Recompilar todo: 3 correcto, 0 con errores
```

### ❌ Si falla:
Reporta el error exacto

---

## 3️⃣ EJECUTA

```bash
dotnet run
```

⏳ Espera ~30 segundos

### ✅ Esperado:
```
Now listening on: https://localhost:5298
Application started. Press Ctrl+C to shut down.
```

---

## 4️⃣ ABRE NAVEGADOR

```
https://localhost:5298/Account/Login
```

---

## 5️⃣ PRUEBAS (5 MINUTOS)

### A. Cliente - Editar Dirección
```
1. Login como cliente
2. Navega: /Cliente/MisDirecciones
3. Click "Editar"
4. Modifica y guarda
✅ Debe actualizar y mostrar mensaje de éxito
```

### B. Cliente - Deshabilitar Dirección
```
1. En MisDirecciones
2. Click "Deshabilitar"
3. Confirma
✅ Dirección desaparece pero no se elimina
```

### C. Cliente - Eliminar Dirección
```
1. En MisDirecciones
2. Click "Eliminar"
3. Confirma
✅ Dirección se borra permanentemente
```

### D. Técnico - Agregar Disponibilidad
```
1. Login como técnico
2. Navega: /Tecnico/MiDisponibilidad
3. Click "Agregar Nueva Disponibilidad"
4. Completa el formulario
5. Click "Agregar"
✅ Nueva fila aparece en la tabla
```

### E. Técnico - Eliminar Disponibilidad
```
1. En MiDisponibilidad
2. Click "Eliminar" en una fila
3. Confirma
✅ Fila desaparece
```

---

## 📊 VERIFICACIÓN RÁPIDA

| Funcionalidad | ✅ | ❌ | Línea |
|---|---|---|---|
| Editar Dirección | [ ] | [ ] | 1 |
| Deshabilitar Dirección | [ ] | [ ] | 2 |
| Eliminar Dirección | [ ] | [ ] | 3 |
| Agregar Disponibilidad | [ ] | [ ] | 4 |
| Eliminar Disponibilidad | [ ] | [ ] | 5 |

**Todos ✅?** → **¡ÉXITO!**

---

## 🆘 PROBLEMAS

### "Build failed"
```
1. dotnet clean
2. Espera
3. dotnet build
4. Reporta el error
```

### "Error 404 en EditarDireccion"
```
Archivo existe, solo toma tiempo en cargar.
Reinicia dotnet run.
```

### "No se ve el botón Agregar"
```
Presiona F5 (refresh) en el navegador.
```

---

## ✅ LISTO

**¡COMPILA AHORA!**

```bash
dotnet clean && dotnet build
```

Luego reporta si pasó o qué error tuvo.

