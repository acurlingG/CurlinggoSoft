# 🚀 VERIFICACIÓN EN VIVO - PASO A PASO

**Objetivo:** Validar que ambos problemas están resueltos  
**Tiempo:** 10 minutos  
**Requisitos:** Aplicación corriendo en `https://localhost:5298/`

---

## ✅ VERIFICACIÓN 1: Código CR-XXXXX del Cliente

### Paso 1: Inicia la aplicación
```bash
cd CurlinggoSoft
dotnet run
```

**Espera que veas:**
```
Now listening on: https://localhost:5298
Now listening on: http://localhost:5298
```

---

### Paso 2: Login como CLIENTE
```
URL: https://localhost:5298/Account/Login

Email:    cliente@curlinggo.com
Contraseña: ClientPassword123!

Haz clic en "Iniciar sesión"
```

---

### Paso 3: Navega a "Mis Reservas"
```
Opción A: Click en navbar → "Mis Reservas"
Opción B: URL directa: https://localhost:5298/Cliente/MisReservas
```

---

### Paso 4: **VERIFICA LA TABLA**

**Deberías VER:**
```
┌─────────────────────────────────────────────┐
│ N° Reserva │ Servicio      │ Fecha │ Estado│
├─────────────┼───────────────┼───────┼───────┤
│ CR-000145   │ Fumigación    │ ...   │  ✅   │
│ CR-000124   │ Reparación    │ ...   │  ⏳   │
│ CR-000100   │ Instalación   │ ...   │  ✅   │
│ CR-000056   │ Limpieza      │ ...   │  ✅   │
│ CR-000012   │ Consulta      │ ...   │  ✅   │
└─────────────────────────────────────────────┘

✅ CORRECTO: Códigos en formato "CR-XXXXXX"
✅ CORRECTO: Ordenados descendente (145 > 124 > 100 > 56 > 12)
```

**NO deberías VER:**
```
❌ GUID largo: 7af2e8c4-c3f5-4d1a-a9e2-1b8c7d6e5f4a
❌ Número simple: 145, 124, 100 (sin CR- al inicio)
❌ Orden ascendente: 12 > 56 > 100 > 124 > 145
```

---

### ✅ RESULTADO
```
SI VES: "CR-000145", "CR-000124", etc.
   → ✅ PROBLEMA 2 RESUELTO

SI VES: GUID largo o número simple
   → ❌ Hay un problema, contacta soporte
```

---

## ✅ VERIFICACIÓN 2: Disponibilidad Técnico

### Paso 1: Logout
```
Click en navbar → "Cerrar sesión"
```

---

### Paso 2: Login como TÉCNICO
```
URL: https://localhost:5298/Account/Login

Email:     tecnico@curlinggo.com
Contraseña: TecnicoPassword123!

Haz clic en "Iniciar sesión"
```

---

### Paso 3A: Intenta acceder a `/DisponibilidadTecnico` (DEBE FALLAR)
```
URL: https://localhost:5298/DisponibilidadTecnico/Index

DEBERÍAS VER:
❌ Error 403 - Forbidden
ó
❌ "No tienen permiso para acceder a este recurso"

✅ CORRECTO: El técnico está bloqueado
```

---

### Paso 3B: Accede a `/Tecnico/MiDisponibilidad` (DEBE FUNCIONAR)
```
Opción A: Click en navbar → "Mi Panel" → "Mi Disponibilidad"
Opción B: URL directa: https://localhost:5298/Tecnico/MiDisponibilidad
```

**DEBERÍAS VER:**
```
┌──────────────────────────────────────────────┐
│ 🕐 Mi Disponibilidad                         │
│                                              │
│ Configura tu disponibilidad horaria para     │
│ recibir ofertas de servicios.                │
│                                              │
├──────────────────────────────────────────────┤
│ Día    │ Inicio │ Fin   │ Estado   │ Acc.   │
├────────┼────────┼───────┼──────────┼────────┤
│ Lunes  │ 08:00  │ 17:00 │ ✅ Activa│ Editar │
│ Martes │ 08:00  │ 17:00 │ ✅ Activa│ Editar │
│ Miér.  │ 10:00  │ 18:00 │ ❌ Inac. │ Editar │
│ Jueves │ 08:00  │ 17:00 │ ✅ Activa│ Editar │
│ Viernes│ 08:00  │ 17:00 │ ✅ Activa│ Editar │
│ Sábado │ 09:00  │ 13:00 │ ✅ Activa│ Editar │
│ Domingo│ ---    │ ---   │ ❌ Inac. │ Editar │
│                                              │
│ ✅ NO hay dropdown de técnicos               │
│ ✅ Solo ve su propia disponibilidad          │
│ ✅ Botón Editar está disponible              │
│                                              │
└──────────────────────────────────────────────┘
```

---

### Paso 4: Prueba editar un horario
```
Haz clic en botón "Editar" de "Lunes"
```

**DEBERÍAS VER:**
```
┌──────────────────────────────────────────┐
│ Editar Disponibilidad                    │
│                                          │
│ Día: Lunes  (solo lectura)               │
│                                          │
│ Hora Inicio: [08:00] ← EDITABLE         │
│ Hora Fin:    [17:00] ← EDITABLE         │
│ ☑ Activa                                  │
│                                          │
│ [Guardar] [Cancelar]                     │
│                                          │
│ ✅ Puedo cambiar horas                   │
│ ✅ Puedo cambiar estado                  │
│ ✅ Día NO se puede cambiar               │
│                                          │
└──────────────────────────────────────────┘
```

---

### ✅ RESULTADO
```
SI VES: Tabla de disponibilidad solo tuya
   → ✅ PROBLEMA 1 RESUELTO

SI VES: Error 403 en /DisponibilidadTecnico
   → ✅ BLOQUEADO CORRECTAMENTE

SI VES: Dropdown de técnicos
   → ❌ Hay un problema, recarga (F5) la página
```

---

## ✅ VERIFICACIÓN 3: Admin Ver Todas

### Paso 1: Logout
```
Click en navbar → "Cerrar sesión"
```

---

### Paso 2: Login como ADMIN
```
URL: https://localhost:5298/Account/Login

Email:     admin@curlinggo.com
Contraseña: AdminPassword123!

Haz clic en "Iniciar sesión"
```

---

### Paso 3: Ve a `/DisponibilidadTecnico` (DEBE FUNCIONAR)
```
URL: https://localhost:5298/DisponibilidadTecnico/Index
```

**DEBERÍAS VER:**
```
┌──────────────────────────────────────────────┐
│ DISPONIBILIDAD - ADMIN                       │
│                                              │
│ Filtrar por Técnico:                         │
│ [Dropdown v]                                 │
│  ├─ (Todos)                                  │
│  ├─ Carlos Rodríguez (carlos@...)           │
│  ├─ Pedro López (pedro@...)                 │
│  ├─ María González (maria@...)              │
│  └─ Juan Pérez (juan@...)                   │
│                                              │
├──────────────────────────────────────────────┤
│ Técnico │ Día   │ Inicio │ Fin   │ Estado   │
├─────────┼───────┼────────┼───────┼──────────┤
│ Carlos  │ Lunes │ 08:00  │ 17:00 │ ✅ Activa│
│ Carlos  │ Martes│ 08:00  │ 17:00 │ ✅ Activa│
│ Pedro   │ Lunes │ 10:00  │ 20:00 │ ✅ Activa│
│ María   │ Lunes │ 06:00  │ 14:00 │ ✅ Activa│
│ Juan    │ Lunes │ 09:00  │ 18:00 │ ✅ Activa│
│                                              │
│ ✅ Admin VE dropdown de técnicos            │
│ ✅ VE TODAS las disponibilidades            │
│ ✅ Puede filtrar por técnico                │
│ ✅ Puede editar todas                       │
│                                              │
└──────────────────────────────────────────────┘
```

---

### ✅ RESULTADO
```
SI VES: Dropdown de técnicos y todas sus disponibilidades
   → ✅ ADMIN CORRECTO

SI VES: Error 403
   → ❌ Hay un problema, verifica rol de usuario
```

---

## 📋 CHECKLIST FINAL

### Cliente - Mis Reservas
- [ ] Veo códigos en formato `CR-000145`
- [ ] Reservas ordenadas de nueva a vieja (descendente)
- [ ] Primero está `CR-000145`, último está `CR-000001`
- [ ] No veo GUID largo o números simples

### Técnico - Disponibilidad
- [ ] `/Tecnico/MiDisponibilidad` funciona
- [ ] Solo veo MI disponibilidad
- [ ] No hay dropdown de técnicos
- [ ] Puedo editar mis horarios
- [ ] `/DisponibilidadTecnico` me da Error 403

### Admin - Disponibilidad
- [ ] `/DisponibilidadTecnico/Index` funciona
- [ ] Veo dropdown de técnicos
- [ ] Veo TODAS las disponibilidades
- [ ] Puedo filtrar y editar
- [ ] `/Tecnico/MiDisponibilidad` me da Error 403

---

## 🐛 TROUBLESHOOTING

### Problema: Sigo viendo código largo
**Solución:**
```bash
# Paso 1: Para la aplicación
Ctrl + C

# Paso 2: Limpia caché
dotnet clean

# Paso 3: Reconstruye
dotnet build

# Paso 4: Ejecuta
dotnet run

# Paso 5: Limpia caché navegador
Ctrl + Shift + Del (Windows) o Cmd + Shift + Del (Mac)

# Paso 6: Recarga página
F5 o Ctrl + F5
```

---

### Problema: Técnico puede ver `/DisponibilidadTecnico`
**Solución:**
```bash
# Verifica que hayas hecho logout/login
1. Cierra sesión (Cerrar sesión)
2. Abre incógnito (Ctrl + Shift + N / Cmd + Shift + N)
3. Login de nuevo como técnico
4. Intenta acceder a /DisponibilidadTecnico
```

---

### Problema: Dropdown de técnicos sigue visible en Mi Disponibilidad
**Solución:**
```
1. Recarga página (F5)
2. Si persiste, limpia caché (Ctrl + Shift + Del)
3. Si persiste, reinicia navegador completamente
```

---

### Problema: Reservas no están en orden descendente
**Solución:**
```bash
# Verifica que la BD tenga datos
dotnet ef database update

# Recarga la aplicación
dotnet run
```

---

## ✅ RESULTADO ESPERADO FINAL

Cuando termines las 3 verificaciones, deberías ver:

```
✅ Verificación 1: Código CR-XXXXX         APROBADO
✅ Verificación 2: Técnico bloqueado       APROBADO
✅ Verificación 3: Admin ve todas          APROBADO

🎉 TODOS LOS PROBLEMAS RESUELTOS
```

---

## 📞 SOPORTE

Si algo no funciona:

| Paso | Si no funciona | Haz esto |
|------|---|---|
| 1 | Código sigue largo | `dotnet clean && dotnet build && dotnet run` |
| 2 | Técnico accede a DisponibilidadTecnico | Logout + Login en incógnito |
| 3 | Admin no ve disponibilidades | F5 + Ctrl + Shift + Del |

---

**Última Actualización:** [Hoy]  
**Status:** ✅ VERIFICACIÓN MANUAL  
**Tiempo Estimado:** 10 minutos

¡Éxito! 🚀

