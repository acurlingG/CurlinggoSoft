# 🔨 INSTRUCCIONES DE VALIDACIÓN POST-CORRECCIÓN

**Estado:** Corrección aplicada al archivo  
**Próximo Paso:** Compilar y validar  
**Tiempo Estimado:** 5 minutos  

---

## ⚡ ACCIONES INMEDIATAS (AHORA MISMO)

### Acción 1: Abre Terminal
```bash
# Windows (PowerShell):
Start-Process -FilePath "powershell.exe" -ArgumentList "-NoExit"

# o simplemente abre Terminal/CMD
```

### Acción 2: Navega a la Carpeta del Proyecto
```bash
cd C:\Users\CURLING\source\repos\CurlinggoSoft\CurlinggoSoft
```

### Acción 3: Limpia el Proyecto
```bash
dotnet clean
```
**Espera que termine (puede tomar 10-30 segundos)**

### Acción 4: Reconstruye
```bash
dotnet build
```

---

## ✅ RESULTADOS ESPERADOS

### Escenario 1: ✅ ÉXITO (Lo que deberías ver)
```
Determinando proyectos para restauración...
Restaurando C:\Users\CURLING\source\repos\CurlinggoSoft\CurlinggoSoft\CurlinggoSoft.csproj...
Proyecto CurlinggoSoft restaurado (en ...ms).
Compilando CurlinggoSoft [net10.0]...
Compilación completada exitosamente en ...ms

✅ Build succeeded.
   0 Warning(s)
   0 Error(s)

Time Elapsed 00:00:15.23
```

**Acción:**
```
→ Ejecuta: dotnet run
→ Prueba en navegador: https://localhost:5298
```

---

### Escenario 2: ❌ FALLO (Errores Persistentes)
```
...
error CS0103: El nombre 'usuarioPendiente' no existe en el contexto actual
error CS0111: El tipo 'AccountController' ya define un miembro denominado 'Logout'
...
❌ Build failed. 2 error(s), 0 warning(s)
```

**Acción:**
```
1. Presiona Ctrl+C para detener
2. Ejecuta: dir AccountController.cs
3. Verifica que el archivo exista
4. Abre Visual Studio
5. Edita AccountController.cs manualmente
6. Presiona Ctrl+S (guardar)
7. Intenta dotnet build nuevamente
```

---

### Escenario 3: ⚠️ ERRORES DIFERENTES
```
error CS1002: Se esperaba ;
error CS1519: Token '...' inválido en declaración de clase/estructura/interfaz
```

**Acción:**
```
1. Archivo está corrompido
2. Solución: Restaurar desde:
   https://github.com/.../AccountController.cs.backup
3. O contactar soporte
```

---

## 🧪 PRUEBA DE LAS FUNCIONALIDADES

### Si `dotnet build` exitoso → Ejecuta

```bash
dotnet run
```

**Espera mensaje:**
```
Now listening on: https://localhost:5298
Starting update to https://localhost:5298/server-error
Application started. Press Ctrl+C to shut down.
```

---

### Prueba 1: Cambio de Contraseña

**En navegador (pestaña nueva), ve a:**
```
https://localhost:5298/Account/ChangePassword
```

**Deberías ver:**
```
┌─────────────────────────────────────────┐
│  Cambiar Contraseña                     │
│                                         │
│  Contraseña Actual    [_____________]   │
│  Contraseña Nueva     [_____________]   │
│  Confirmar Contraseña [_____________]   │
│                                         │
│  [Cambiar Contraseña] [Cancelar]        │
│                                         │
└─────────────────────────────────────────┘
```

✅ Si ves esto: Cambio de contraseña está funcionando

❌ Si ves Error 404: Falta la vista. Ve a "Troubleshooting"

---

### Prueba 2: Login y Logout

**En navegador, ve a:**
```
https://localhost:5298/Account/Login
```

**Paso 1: Login**
- Email: `cliente@curlinggo.com`
- Contraseña: `ClientPassword123!`
- Click "Iniciar sesión"

**Paso 2: Verifica 2FA**
- Deberías recibir código por email
- Ingresa el código
- Click "Verificar"

**Paso 3: Deberías llegar a Dashboard**
- Si es cliente → /Cliente/Index
- Si es técnico → /Tecnico/Index
- Si es admin → /Admin/Index

✅ Si llega al dashboard: Login funciona

**Paso 4: Logout**
- Click en navbar → "Cerrar sesión"
- Deberías regresar a página de login

✅ Si regresa a login: Logout funciona

---

### Prueba 3: Cambio de Contraseña Completo

**Desde dashboard (ya logueado):**
```
1. Click en navbar → "Cambiar Contraseña"
2. Ingresa contraseña actual (la que usaste para login)
3. Ingresa nueva contraseña (ej: NuevaPassword123!)
4. Confirma la nueva contraseña
5. Click "Cambiar Contraseña"
```

**Resultado esperado:**
```
✅ Mensaje: "Contraseña cambiada exitosamente"
✅ Redirigido a Login
✅ Ya NO puedes login con contraseña vieja
✅ Debes login con contraseña nueva
```

---

## 🐛 TROUBLESHOOTING

### Problema 1: Error 404 en /Account/ChangePassword
**Causa:** Falta la vista  
**Solución:**
```
1. Verifica que exista:
   C:\...\CurlinggoSoft\Views\Account\ChangePassword.cshtml

2. Si NO existe, créala:
   Click derecho en Views/Account/
   Add → Razor View
   Nombre: ChangePassword.cshtml

3. Agrega contenido básico:
@{
	ViewData["Title"] = "Cambiar Contraseña";
}
<div class="container my-4">
	<h2>Cambiar Contraseña</h2>
	<form method="post">
		<div class="mb-3">
			<label>Contraseña Actual</label>
			<input type="password" name="currentPassword" class="form-control" required/>
		</div>
		<div class="mb-3">
			<label>Contraseña Nueva</label>
			<input type="password" name="newPassword" class="form-control" required/>
		</div>
		<div class="mb-3">
			<label>Confirmar Contraseña</label>
			<input type="password" name="confirmPassword" class="form-control" required/>
		</div>
		<button type="submit" class="btn btn-primary">Cambiar Contraseña</button>
		<a href="/" class="btn btn-secondary">Cancelar</a>
	</form>
</div>

4. Presiona Ctrl+S
5. Reinicia dotnet run
```

---

### Problema 2: "Build failed" después de la corrección
**Causa:** Cambios no se guardaron o hay conflicto  
**Solución:**
```bash
1. Presiona Ctrl+C (detener dotnet run)
2. Ejecuta:
   dotnet clean

3. Si eso no funciona:
   del /Q bin\
   del /Q obj\

4. Vuelve a intentar:
   dotnet build
```

---

### Problema 3: "Error en VerifyCode" después del cambio
**Causa:** El método aún tiene problemas
**Solución:**
```
1. Abre AccountController.cs
2. Ve a línea 155 (Ctrl+G)
3. Verifica que sea:
   return View("~/Views/Login/VerifyCode.cshtml");
   }

4. Si no está, edita hasta que veas eso
5. Presiona Ctrl+S
6. Intenta dotnet build nuevamente
```

---

## ✳️ CHECKLIST FINAL

```
COMPILACIÓN:
- [ ] dotnet clean ejecutado sin errores
- [ ] dotnet build ejecutado sin errores CS0103/CS0111
- [ ] Mensaje "Build succeeded" visible

EJECUCIÓN:
- [ ] dotnet run inicia sin problemas
- [ ] Mensaje "Application started" visible
- [ ] Puede navegar a https://localhost:5298

PRUEBAS:
- [ ] Login funciona
- [ ] 2FA funciona
- [ ] Cambio de contraseña visible en menu
- [ ] Cambio de contraseña funciona
- [ ] Logout funciona
- [ ] Re-login con nueva contraseña funciona

RESULTADO FINAL:
- [ ] TODOS los checkboxes marcados = ✅ ÉXITO
- [ ] Alguno sin marcar = ❌ Revisar ese punto
```

---

## 🎯 CONCLUSIÓN

```
┌──────────────────────────────────────┐
│  ESTADO: CORRECCIÓN APLICADA         │
│  ACCIÓN SIGUIENTE: dotnet build      │
│  TIEMPO: ~5-10 minutos               │
│  COMPLEJIDAD: Baja                   │
└──────────────────────────────────────┘
```

**¡Ahora procede a ejecutar `dotnet build` en tu terminal!**

Si tienes cualquier error, reporta el mensaje exacto y aquí te ayudaré a resolverlo.

