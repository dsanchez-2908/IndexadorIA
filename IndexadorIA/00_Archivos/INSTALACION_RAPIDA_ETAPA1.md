# 🚀 GUÍA RÁPIDA - INSTALACIÓN ETAPA 1

## ⚠️ IMPORTANTE
Esta guía es SOLO para la **Etapa 1**: Login, Pantalla Principal y CRUD de Usuarios.

---

## 📋 PASO 1: Crear la Base de Datos

### Opción A: Desde SQL Server Management Studio (SSMS) - RECOMENDADO

1. **Abrir SSMS**
2. **Conectarse** a `localhost\SQLEXPRESS` con:
   - Usuario: `sa`
   - Contraseña: `123`

3. **Crear la base de datos**:
   ```sql
   CREATE DATABASE IndexadorIA
   ```

4. **Ejecutar el script de Etapa 1**:
   - Abrir el archivo: `IndexadorIA\00_Archivos\ScriptsDB_ETAPA1.sql`
   - Seleccionar la base de datos `IndexadorIA` en el dropdown
   - Presionar **F5** o clic en **Ejecutar**
   - Deberías ver mensajes confirmando la creación de tablas

### Opción B: Desde PowerShell (Línea de comandos)

Abrir PowerShell y ejecutar:

```powershell
# Crear la base de datos
sqlcmd -S localhost\SQLEXPRESS -U sa -P 123 -Q "CREATE DATABASE IndexadorIA"

# Ejecutar el script (ajusta la ruta si es necesario)
sqlcmd -S localhost\SQLEXPRESS -U sa -P 123 -d IndexadorIA -i "C:\Users\danie\source\repos\IndexadorIA\IndexadorIA\00_Archivos\ScriptsDB_ETAPA1.sql"
```

---

## ✅ PASO 2: Verificar que todo se creó correctamente

En SSMS, ejecuta esta consulta para verificar:

```sql
USE IndexadorIA
GO

-- Ver tablas creadas
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
GO

-- Ver estados
SELECT * FROM TD_ESTADOS
GO

-- Ver usuario admin
SELECT cdUsuario, dsUsuario, dsNombreCompleto, cdEstado FROM TD_USUARIOS
GO
```

**Deberías ver:**
- ✅ 2 tablas: `TD_ESTADOS` y `TD_USUARIOS`
- ✅ 2 estados: Activo (1) e Inactivo (0)
- ✅ 1 usuario: admin

---

## 🎯 PASO 3: Ejecutar la Aplicación

### Desde Visual Studio

1. Presionar **F5** o clic en ▶️ **Iniciar**
2. Se abrirá la pantalla de Login

### Desde Terminal

```powershell
cd C:\Users\danie\source\repos\IndexadorIA
dotnet run --project IndexadorIA
```

---

## 🔐 PASO 4: Probar el Login

En la pantalla de Login, ingresar:
- **Usuario:** `admin`
- **Contraseña:** `123`
- Clic en **INGRESAR** o presionar **Enter**

✅ Deberías ver la **Pantalla Principal** con el menú "Configuración"

---

## 🧪 PASO 5: Probar el CRUD de Usuarios

### 5.1 Ver Lista de Usuarios
1. Menú **Configuración** → **Usuarios**
2. Deberías ver el usuario "admin" en la lista

### 5.2 Crear Nuevo Usuario
1. Clic en botón **+ NUEVO**
2. Ingresar:
   - Usuario: `prueba`
   - Nombre Completo: `Usuario de Prueba`
3. Clic en **GUARDAR**
4. ✅ Se muestra un mensaje con la **clave temporal** generada
5. ✅ El nuevo usuario aparece en la lista

### 5.3 Editar Usuario
1. Clic en **Editar** del usuario "prueba"
2. Cambiar nombre a: `Usuario Modificado`
3. Clic en **GUARDAR**
4. ✅ Se actualiza en la lista

### 5.4 Restablecer Contraseña
1. Clic en **Restablecer** del usuario "prueba"
2. Confirmar la acción
3. ✅ Se genera una nueva clave temporal

### 5.5 Desactivar Usuario
1. Clic en **Eliminar** del usuario "prueba"
2. Confirmar la acción
3. ✅ El usuario pasa a estado "Inactivo"

### 5.6 Cambiar Tu Contraseña
1. Menú **Configuración** → **Cambiar Clave**
2. Ingresar:
   - Contraseña Actual: `123`
   - Nueva Contraseña: `admin123`
   - Confirmar: `admin123`
3. Clic en **GUARDAR**
4. ✅ Contraseña cambiada

### 5.7 Cerrar Sesión
1. Menú **Configuración** → **Cerrar Sesión**
2. Confirmar
3. ✅ Vuelves al Login

---

## ❌ Solución de Problemas

### Problema 1: "Cannot open database IndexadorIA"
**Solución:** La base de datos no existe. Ejecutar PASO 1.

### Problema 2: "Login failed for user 'sa'"
**Solución:** 
- Verificar que SQL Server esté ejecutándose
- Verificar usuario/contraseña en App.config
- Intentar con autenticación de Windows

### Problema 3: "Usuario o contraseña incorrectos"
**Solución:**
- Verificar que el script se ejecutó correctamente
- En SSMS ejecutar: `SELECT * FROM TD_USUARIOS WHERE dsUsuario = 'admin'`
- Si no existe, ejecutar de nuevo el script

### Problema 4: Error al compilar
**Solución:**
```powershell
dotnet restore
dotnet build
```

---

## 📊 ¿Qué deberías tener funcionando?

Marca cada item que funcione correctamente:

- [ ] Base de datos IndexadorIA creada
- [ ] Tablas TD_ESTADOS y TD_USUARIOS creadas
- [ ] Usuario admin existe en la base de datos
- [ ] Aplicación inicia sin errores
- [ ] Pantalla de Login se muestra
- [ ] Login con admin/123 funciona
- [ ] Pantalla Principal se muestra
- [ ] Menú Configuración visible
- [ ] Opción "Usuarios" abre el formulario
- [ ] Lista de usuarios se carga
- [ ] Puedes crear un nuevo usuario
- [ ] Se genera clave temporal
- [ ] Puedes editar un usuario
- [ ] Puedes restablecer contraseña
- [ ] Puedes desactivar usuario
- [ ] Puedes cambiar tu contraseña
- [ ] Cerrar sesión funciona

---

## ✅ Si TODO funciona correctamente

**¡Felicitaciones!** La Etapa 1 está funcionando perfectamente.

Puedes confirmarme que todo funciona y entonces:
1. Continuamos con la Etapa 2 (cuando lo solicites)
2. O hacemos ajustes a la Etapa 1 si lo necesitas

---

## 📞 ¿Necesitas Ayuda?

Si algo no funciona, dime:
1. ¿En qué paso estás?
2. ¿Qué error ves exactamente?
3. ¿Qué pantalla/mensaje aparece?

Y te ayudaré a solucionarlo paso a paso.

---

**NOTA IMPORTANTE:** Ignora por ahora todos los archivos de "Etapa 2" que se crearon. Esos son para más adelante cuando termines de probar la Etapa 1 y decidas continuar.
