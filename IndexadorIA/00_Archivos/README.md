# IndexadorIA - Sistema de Indexación con IA

## Descripción
Sistema de indexación de documentos utilizando inteligencia artificial para captura automática de metadatos.

## Etapa 1 - Completada ✓

### Características Implementadas
- ✅ Base de datos con tablas TD_ESTADOS y TD_USUARIOS
- ✅ Sistema de autenticación y seguridad
- ✅ Encriptación de contraseñas con SHA256
- ✅ CRUD completo de usuarios
- ✅ Gestión de claves temporales y primer ingreso
- ✅ Interfaz moderna con modo oscuro
- ✅ Arquitectura en capas (Pantallas, Negocio, Datos, Entidades)

## Requisitos

### Software Necesario
- Visual Studio 2026 o superior
- .NET 10
- SQL Server Express o superior

### Paquetes NuGet
- Microsoft.Data.SqlClient (5.2.2)
- System.Configuration.ConfigurationManager (9.0.0)

## Instalación

### 1. Configurar Base de Datos

#### Opción A: Usando SQL Server Management Studio (SSMS)
1. Abrir SSMS y conectarse a `localhost\SQLEXPRESS`
2. Crear la base de datos:
   ```sql
   CREATE DATABASE IndexadorIA
   ```
3. Ejecutar el script completo ubicado en: `IndexadorIA/00_Archivos/ScriptsDB.sql`

#### Opción B: Usando sqlcmd (línea de comandos)
```bash
# Crear la base de datos
sqlcmd -S localhost\SQLEXPRESS -U sa -P 123 -Q "CREATE DATABASE IndexadorIA"

# Ejecutar el script
sqlcmd -S localhost\SQLEXPRESS -U sa -P 123 -d IndexadorIA -i "IndexadorIA/00_Archivos/ScriptsDB.sql"
```

### 2. Configurar Cadena de Conexión

Editar el archivo `App.config` si tu configuración de SQL Server es diferente:

```xml
<connectionStrings>
	<add name="IndexadorIA" 
		 connectionString="Server=TU_SERVIDOR;Database=IndexadorIA;User Id=TU_USUARIO;Password=TU_CLAVE;TrustServerCertificate=True;" 
		 providerName="Microsoft.Data.SqlClient" />
</connectionStrings>
```

### 3. Restaurar Paquetes y Compilar

```bash
# Restaurar paquetes NuGet
dotnet restore

# Compilar el proyecto
dotnet build
```

### 4. Ejecutar la Aplicación

```bash
dotnet run --project IndexadorIA
```

O desde Visual Studio: presionar F5

## Credenciales de Acceso Inicial

- **Usuario:** admin
- **Contraseña:** 123

## Estructura del Proyecto

```
IndexadorIA/
├── 00_Archivos/          # Scripts SQL y archivos auxiliares
├── 01_Pantallas/         # Formularios de la aplicación
│   └── Configuracion/    # Formularios de configuración
├── 02_Negocio/           # Capa de lógica de negocio
├── 03_Datos/             # Capa de acceso a datos
└── 04_Entidades/         # Clases de entidades
```

## Nomenclatura de Base de Datos

### Tablas
- **TD_** - Tablas de datos (ej: TD_USUARIOS)
- **TR_** - Tablas de relación
- **TV_** - Tablas de valores fijos
- **TMP_** - Tablas temporales

### Campos
- **cd** - Campos ID (ej: cdUsuario)
- **fe** - Campos fecha (ej: feAlta)
- **nu** - Campos numéricos (ej: nuOrden)
- **ds** - Campos texto (ej: dsUsuario)
- **sn** - Campos booleanos (ej: snActivo)

### Stored Procedures y Vistas
- **SP_** - Stored Procedures
- **VW_** - Vistas

## Funcionalidades Disponibles

### Gestión de Usuarios
- Crear nuevos usuarios con clave temporal
- Editar información de usuarios
- Activar/Desactivar usuarios
- Restablecer contraseñas
- Cambio de contraseña personal
- Validación de primer ingreso

### Seguridad
- Autenticación con usuario y contraseña
- Contraseñas encriptadas con SHA256
- Claves temporales para nuevos usuarios
- Forzar cambio de contraseña en primer ingreso
- Control de sesiones

## Próximas Etapas

### Etapa 2 (Pendiente)
- Ingreso de archivos desde PATH seleccionado
- Separación de páginas de PDF
- Rotación automática de imágenes

### Etapa 3 (Pendiente)
- Preparación de lotes
- Área de recorte automático
- Procesamiento con API BATCH de OpenAI

### Etapa 4 (Pendiente)
- Control de calidad
- Renombrado de archivos
- Generación de CSV con metadatos

## Notas de Desarrollo

- El proyecto utiliza .NET 10 con Windows Forms
- Se implementó modo oscuro en toda la interfaz
- La arquitectura es en capas pero separadas por carpetas (no proyectos)
- Las contraseñas NUNCA se almacenan en texto plano
- El usuario admin inicial tiene todos los permisos

## Solución de Problemas

### Error de conexión a la base de datos
- Verificar que SQL Server esté ejecutándose
- Confirmar usuario y contraseña en App.config
- Verificar que la instancia sea `localhost\SQLEXPRESS`

### Error al restaurar paquetes
```bash
dotnet nuget locals all --clear
dotnet restore
```

### La base de datos no se crea
- Verificar permisos del usuario de SQL Server
- Intentar con autenticación de Windows si es posible
- Revisar logs de SQL Server para errores específicos

## Contacto y Soporte

Para reportar problemas o sugerencias sobre el desarrollo del proyecto, consulta con el equipo de desarrollo.

---
**Versión:** 1.0 (Etapa 1)  
**Fecha:** 2024  
**Desarrollado en:** C# .NET 10 - Windows Forms
