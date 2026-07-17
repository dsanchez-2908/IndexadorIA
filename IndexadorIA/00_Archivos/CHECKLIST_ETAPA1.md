# ✅ CHECKLIST DE VERIFICACIÓN - ETAPA 1

## 📋 Lista de Comprobación

Usa este checklist para verificar que todo funciona correctamente después de la instalación.

---

## 1️⃣ Instalación de Base de Datos

- [ ] Base de datos "IndexadorIA" creada en SQL Server
- [ ] Script `ScriptsDB.sql` ejecutado sin errores
- [ ] Tabla TD_ESTADOS creada correctamente
- [ ] Tabla TD_USUARIOS creada correctamente
- [ ] Stored Procedure SP_VALIDAR_LOGIN creado
- [ ] Estados "Activo" e "Inactivo" insertados
- [ ] Usuario "admin" con clave "123" creado

### Verificación Manual (SQL)
```sql
-- Verificar tablas
SELECT * FROM TD_ESTADOS
SELECT * FROM TD_USUARIOS

-- Debe mostrar:
-- TD_ESTADOS: 2 registros (Activo, Inactivo)
-- TD_USUARIOS: 1 registro (admin)
```

---

## 2️⃣ Configuración del Proyecto

- [ ] Paquete Microsoft.Data.SqlClient (5.2.2) instalado
- [ ] Paquete System.Configuration.ConfigurationManager (9.0.0) instalado
- [ ] Archivo App.config configurado con cadena de conexión correcta
- [ ] Proyecto compila sin errores
- [ ] Proyecto compila sin warnings críticos

### Verificación Manual
```bash
# Verificar compilación
dotnet build

# Debe mostrar: "Compilación correcta"
```

---

## 3️⃣ Funcionalidad de Login

- [ ] Aplicación se inicia y muestra pantalla de Login
- [ ] Formulario de login tiene diseño oscuro moderno
- [ ] Login con "admin" / "123" funciona correctamente
- [ ] Login con credenciales incorrectas muestra error apropiado
- [ ] Campos vacíos muestran mensaje de validación
- [ ] Presionar Enter en campo de contraseña ejecuta el login
- [ ] Después de login exitoso, muestra pantalla principal

### Pruebas a Realizar
1. ✅ Intentar login sin usuario → Debe pedir ingresar usuario
2. ✅ Intentar login sin contraseña → Debe pedir ingresar contraseña
3. ✅ Login con "admin" / "incorrecto" → Debe mostrar "Usuario o contraseña incorrectos"
4. ✅ Login con "admin" / "123" → Debe ingresar al sistema

---

## 4️⃣ Pantalla Principal

- [ ] Menú "Configuración" visible en la parte superior
- [ ] Barra de estado muestra nombre del usuario actual
- [ ] Barra de estado muestra fecha actual
- [ ] Mensaje de bienvenida "Bienvenido a IndexadorIA" visible
- [ ] Diseño oscuro aplicado correctamente
- [ ] Ventana se maximiza correctamente

### Opciones del Menú
- [ ] Menú Configuración → Usuarios (visible)
- [ ] Menú Configuración → Cambiar Clave (visible)
- [ ] Menú Configuración → Cerrar Sesión (visible)

---

## 5️⃣ Gestión de Usuarios - Listar

- [ ] Al hacer clic en "Configuración → Usuarios" se abre el formulario
- [ ] DataGridView muestra el usuario "admin"
- [ ] Columnas visibles: Código, Usuario, Nombre Completo, Estado, Fecha Alta
- [ ] Botones visibles: Editar, Restablecer, Eliminar
- [ ] Botón "+ NUEVO" visible en la parte superior derecha
- [ ] DataGridView con tema oscuro aplicado
- [ ] Datos se cargan correctamente

---

## 6️⃣ Gestión de Usuarios - Crear

- [ ] Botón "+ NUEVO" abre el formulario de creación
- [ ] Formulario tiene título "Nuevo Usuario"
- [ ] Campos: Usuario, Nombre Completo (Estado no visible en creación)
- [ ] Botones: GUARDAR, CANCELAR
- [ ] Validación: Campo usuario vacío muestra error
- [ ] Validación: Campo nombre completo vacío muestra error
- [ ] Validación: Usuario duplicado muestra error
- [ ] Al guardar, muestra mensaje con clave temporal generada
- [ ] Usuario nuevo aparece en la lista después de crear

### Prueba de Creación
```
1. Clic en "+ NUEVO"
2. Usuario: "prueba"
3. Nombre: "Usuario de Prueba"
4. Clic en GUARDAR
5. ✅ Debe mostrar: "Usuario creado exitosamente. Clave temporal: XXXXXXXX"
6. ✅ Usuario "prueba" debe aparecer en la lista
```

---

## 7️⃣ Gestión de Usuarios - Editar

- [ ] Botón "Editar" abre el formulario de edición
- [ ] Formulario tiene título "Editar Usuario"
- [ ] Campos pre-rellenados con datos del usuario
- [ ] Campo "Estado" visible con opciones Activo/Inactivo
- [ ] Permite modificar usuario, nombre y estado
- [ ] Validación de campos funciona
- [ ] Al guardar, muestra mensaje de éxito
- [ ] Cambios reflejados en la lista

### Prueba de Edición
```
1. Clic en "Editar" del usuario "prueba"
2. Cambiar nombre a "Usuario Modificado"
3. Clic en GUARDAR
4. ✅ Debe mostrar: "Usuario actualizado exitosamente"
5. ✅ Lista debe mostrar el nombre actualizado
```

---

## 8️⃣ Gestión de Usuarios - Restablecer Clave

- [ ] Botón "Restablecer" solicita confirmación
- [ ] Al confirmar, genera nueva clave temporal
- [ ] Muestra mensaje con la nueva clave temporal
- [ ] Usuario puede ingresar con la nueva clave

### Prueba de Restablecimiento
```
1. Clic en "Restablecer" del usuario "prueba"
2. Confirmar acción
3. ✅ Debe mostrar: "Contraseña restablecida exitosamente. Clave temporal: XXXXXXXX"
4. Anotar la clave
5. Cerrar sesión y probar login con usuario "prueba" y la clave temporal
6. ✅ Debe solicitar cambio de contraseña
```

---

## 9️⃣ Gestión de Usuarios - Eliminar

- [ ] Botón "Eliminar" solicita confirmación
- [ ] No permite eliminar el usuario actual (debe mostrar advertencia)
- [ ] Al confirmar, usuario pasa a estado "Inactivo"
- [ ] Usuario inactivo no puede hacer login
- [ ] Usuario sigue visible en la lista con estado "Inactivo"

### Prueba de Eliminación
```
1. Intentar eliminar usuario "admin" (usuario actual)
2. ✅ Debe mostrar: "No puede eliminar su propio usuario"
3. Crear otro usuario de prueba
4. Clic en "Eliminar" del usuario de prueba
5. Confirmar acción
6. ✅ Debe mostrar: "Usuario desactivado exitosamente"
7. ✅ Usuario debe aparecer con estado "Inactivo"
```

---

## 🔟 Cambio de Contraseña

- [ ] Menú "Configuración → Cambiar Clave" abre el formulario
- [ ] Campos: Contraseña Actual, Nueva Contraseña, Confirmar Contraseña
- [ ] Validación: Contraseña actual vacía muestra error
- [ ] Validación: Nueva contraseña vacía muestra error
- [ ] Validación: Contraseña menor a 4 caracteres muestra error
- [ ] Validación: Contraseñas no coinciden muestra error
- [ ] Validación: Contraseña actual incorrecta muestra error
- [ ] Al cambiar correctamente, muestra mensaje de éxito
- [ ] Puede ingresar con la nueva contraseña

### Prueba de Cambio de Contraseña
```
1. Menú Configuración → Cambiar Clave
2. Contraseña Actual: "123"
3. Nueva Contraseña: "admin123"
4. Confirmar: "admin123"
5. Clic en GUARDAR
6. ✅ Debe mostrar: "Contraseña cambiada exitosamente"
7. Cerrar sesión
8. Intentar login con "admin" / "123"
9. ✅ Debe fallar
10. Intentar login con "admin" / "admin123"
11. ✅ Debe ingresar correctamente
```

---

## 1️⃣1️⃣ Primer Ingreso (Clave Temporal)

- [ ] Usuario nuevo con clave temporal es detectado
- [ ] Al iniciar sesión, solicita automáticamente cambio de clave
- [ ] Formulario de cambio NO muestra campo "Contraseña Actual"
- [ ] Solo solicita: Nueva Contraseña y Confirmar Contraseña
- [ ] Mensaje indica que debe cambiar clave temporal
- [ ] Validaciones funcionan correctamente
- [ ] Si cancela, no permite continuar
- [ ] Al cambiar clave exitosamente, ingresa al sistema

### Prueba de Primer Ingreso
```
1. Crear usuario nuevo (anotar clave temporal)
2. Cerrar sesión del admin
3. Login con usuario nuevo y clave temporal
4. ✅ Debe mostrar: "Debe cambiar su contraseña temporal"
5. ✅ Formulario solo debe mostrar 2 campos (nueva y confirmar)
6. Ingresar nueva contraseña
7. Clic en GUARDAR
8. ✅ Debe ingresar al sistema
```

---

## 1️⃣2️⃣ Cerrar Sesión

- [ ] Menú "Configuración → Cerrar Sesión" solicita confirmación
- [ ] Al confirmar, cierra sesión y vuelve al login
- [ ] Al cancelar, permanece en el sistema
- [ ] Datos de sesión se limpian correctamente

### Prueba de Cierre de Sesión
```
1. Menú Configuración → Cerrar Sesión
2. ✅ Debe solicitar confirmación
3. Clic en "Sí"
4. ✅ Debe volver a la pantalla de Login
5. ✅ No debe recordar sesión anterior
```

---

## 1️⃣3️⃣ Interfaz y Diseño

- [ ] Todos los formularios usan tema oscuro
- [ ] Colores consistentes en toda la aplicación
- [ ] Textos legibles (blanco sobre fondo oscuro)
- [ ] Botones destacados correctamente
- [ ] Controles bien alineados
- [ ] Sin elementos cortados o superpuestos
- [ ] Fuentes uniformes (Segoe UI)
- [ ] Iconos o botones con cursor de mano

---

## 1️⃣4️⃣ Seguridad

- [ ] Contraseñas NO visibles en campos (mostrar •••)
- [ ] Contraseñas encriptadas en base de datos (verificar en SQL)
- [ ] No se puede acceder a pantallas sin login
- [ ] Sesión persiste mientras esté abierta la aplicación
- [ ] No hay contraseñas en texto plano en el código
- [ ] Cadena de conexión parametrizable en App.config

### Verificación de Encriptación
```sql
-- Verificar que las contraseñas estén encriptadas
SELECT dsUsuario, dsClave FROM TD_USUARIOS

-- La columna dsClave debe mostrar un hash SHA256 (64 caracteres hexadecimales)
-- Ejemplo: a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3
-- NO debe mostrar: "123" o texto plano
```

---

## 1️⃣5️⃣ Rendimiento y Estabilidad

- [ ] Aplicación inicia en menos de 5 segundos
- [ ] No hay lag al navegar entre formularios
- [ ] DataGridView carga rápido (< 2 segundos)
- [ ] No hay errores en consola o logs
- [ ] Aplicación no se cuelga al realizar operaciones
- [ ] Cursor de espera se muestra en operaciones largas
- [ ] No hay memory leaks (cerrar y abrir múltiples formularios)

---

## ✅ RESULTADO FINAL

### Estadísticas de Verificación
- **Total de checks**: 115
- **Completados**: _____
- **Fallidos**: _____
- **Pendientes**: _____

### Estado General
- [ ] ✅ Todo funciona correctamente
- [ ] ⚠️ Funciona con advertencias menores
- [ ] ❌ Hay errores críticos que resolver

---

## 🔧 Registro de Problemas Encontrados

Si encuentras algún problema durante la verificación, regístralo aquí:

### Problema 1
- **Descripción**: 
- **Severidad**: Alta / Media / Baja
- **Pasos para reproducir**:
- **Solución aplicada**:

### Problema 2
- **Descripción**: 
- **Severidad**: Alta / Media / Baja
- **Pasos para reproducir**:
- **Solución aplicada**:

---

## 📝 Notas Adicionales

Espacio para notas, observaciones o mejoras sugeridas:

```
[Tus notas aquí]
```

---

## 🎯 Siguiente Paso

Una vez que todos los checks estén completados y verificados:

✅ **ETAPA 1 COMPLETADA** → Listo para comenzar **ETAPA 2**

---

**Fecha de verificación**: _______________  
**Verificado por**: _______________  
**Firma**: _______________
