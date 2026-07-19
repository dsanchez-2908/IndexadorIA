# ETAPA 4 - Preparación de Lotes

## Descripción
Esta etapa implementa la funcionalidad de preparación de lotes para agrupar archivos página y enviarlos posteriormente en batch a la IA para captura de datos.

## Componentes Creados

### 1. Base de Datos
**Archivo:** `IndexadorIA/00_Archivos/ScriptsDB_ETAPA4_PreparacionLotes.sql`

- **Nuevos Estados:**
  - `ARCHIVO_PAGINA`, `cdEstado=2`: "Pendiente de Preparar imágenes"
  - `LOTE`, `cdEstado=1`: "Pendiente de Preparar imágenes"
  - `LOTE`, `cdEstado=2`: "En proceso"
  - `LOTE`, `cdEstado=3`: "Finalizado"

- **Tabla TD_LOTE:**
  - `cdLote` (PK, IDENTITY)
  - `dsNombreLote` (VARCHAR(50), UNIQUE)
  - `nuCantidadArchivos` (INT)
  - `cdEstadoLote` (INT, FK a TD_ESTADOS)
  - `feAltaLote` (DATETIME)
  - `cdUsuarioAltaLote` (INT, FK a TD_USUARIOS)

- **Tabla TD_LOTE_ARCHIVOS (relación):**
  - `id` (PK, IDENTITY)
  - `cdLote` (INT, FK a TD_LOTE)
  - `cdArchivoPagina` (INT, FK a TD_ARCHIVOS_PAGINAS, UNIQUE)

- **Secuencia SEQ_LOTE:**
  - Genera números secuenciales para nombres de lote (LOTE_00000001, LOTE_00000002, etc.)

### 2. Entidades
- **`Lote.cs`**: Entidad principal de lote
- **`LoteArchivo.cs`**: Relación lote-archivo
- **`ArchivoPaginaGridDto.cs`**: DTO para mostrar archivos en la grilla con columna de selección

### 3. Capa de Datos (DAL)
**Archivo:** `IndexadorIA/03_Datos/LoteDAL.cs`

Métodos principales:
- `ObtenerSiguienteSecuencia()`: Obtiene el siguiente número de la secuencia
- `CrearLote(...)`: Crea un lote con transacción completa (inserta lote, relaciones y actualiza estados)
- `ObtenerArchivosPaginaParaLote(...)`: Consulta archivos página en estado 1 con filtros opcionales

### 4. Capa de Negocio (BL)
**Archivo:** `IndexadorIA/02_Negocio/LoteBL.cs`

Métodos principales:
- `ObtenerArchivosPaginaParaLote(...)`: Obtiene archivos disponibles con manejo de excepciones y logging
- `CrearLotes(...)`: Agrupa archivos seleccionados en lotes según la cantidad configurada

### 5. Interfaz de Usuario
**Archivo:** `IndexadorIA/01_Pantallas/PlanosGCBA/FrmPreparacionLotes.cs`

#### Características:
- **Panel de Filtros:**
  - Nombre de archivo (filtro parcial)
  - Fecha Alta desde/hasta
  - Botones: Buscar, Limpiar

- **DataGridView:**
  - Columna de selección (checkbox)
  - ID, Fecha Alta, Estado, Proyecto
  - Archivo Original, Carpeta, Página, Total Páginas
  - Nombre del archivo página

- **Panel de Acciones:**
  - Botones de selección:
	- Seleccionar Todo
	- Deseleccionar Todo
	- Seleccionar 50
  - Campo "Archivos por Lote" (default: 100)
  - Botón "Crear Lotes"
  - Label informativo con totales

#### Flujo de Creación de Lotes:
1. Usuario filtra/busca archivos página en estado 1
2. Selecciona registros usando checkboxes o botones de selección
3. Configura cantidad de "Archivos por Lote"
4. Presiona "Crear Lotes"
5. El sistema:
   - Agrupa archivos según cantidad configurada
   - Genera nombres secuenciales (LOTE_00000001, LOTE_00000002, ...)
   - Inserta registros en TD_LOTE
   - Inserta relaciones en TD_LOTE_ARCHIVOS
   - Actualiza TD_ARCHIVOS_PAGINAS a cdEstado=2
   - Registra logs de operación

### 6. Menú Principal
**Archivo modificado:** `IndexadorIA/01_Pantallas/FrmPrincipal.cs`

Se agregó el submenú "Preparación de Lotes" bajo "Planos de GCBA"

## Flujo de Estados

```
TD_ARCHIVOS_PAGINAS:
  cdEstado=1 (Separado correctamente) 
	→ [Selección en grilla]
	→ cdEstado=2 (Pendiente de Preparar imágenes)

TD_LOTE:
  cdEstadoLote=1 (Pendiente de Preparar imágenes) [creación]
  → cdEstadoLote=2 (En proceso) [futuro: procesamiento IA]
  → cdEstadoLote=3 (Finalizado) [futuro: completado]
```

## Consulta Principal

```sql
SELECT a.cdArchivoPagina,
	   a.feAlta,
	   a.cdEstado,
	   c.dsEstado,
	   a.dsNombreArchivoPagina,
	   a.nuPagina,
	   a.dsRutaCompleta,
	   a.cdArchivoOriginal,
	   b.cdProyecto,
	   d.dsProyecto,
	   b.dsNombreArchivo,
	   b.dsNombreUltimaCarpeta,
	   b.nuCantidadPaginas
FROM TD_ARCHIVOS_PAGINAS a
LEFT JOIN TD_ARCHIVOS_ORIGINAL b ON a.cdArchivoOriginal = b.cdArchivo
LEFT JOIN TD_ESTADOS c ON c.dsProceso = 'ARCHIVO_PAGINA' AND a.cdEstado = c.cdEstado
LEFT JOIN TD_PROYECTOS d ON b.cdProyecto = d.cdProyecto
WHERE b.cdProyecto = 1 AND a.cdEstado = 1
```

## Validaciones Implementadas

1. Al menos un archivo debe estar seleccionado
2. Cantidad de archivos por lote debe ser > 0
3. Confirmación antes de crear lotes
4. Manejo de excepciones con logging completo
5. Transacción completa (rollback en caso de error)

## Logs Generados

Todos los logs se registran en `TD_LOGS` con módulo "PREPARACION_LOTE":
- Errores al obtener datos
- Creación individual de cada lote
- Resumen de operación completada
- Errores en creación de lotes

## Próximos Pasos (Futuras Etapas)

1. Preparación de imágenes en lote
2. Envío de lotes a IA (OpenAI Batch API)
3. Monitoreo de estado de procesamiento
4. Recuperación de resultados
5. Carga de datos capturados

## Notas de Implementación

- Los nombres de lote son únicos y secuenciales con 8 dígitos
- Cada archivo página solo puede estar en un lote (índice único)
- Se utiliza transacción para garantizar consistencia
- La grilla se actualiza automáticamente después de crear lotes
- Los filtros son opcionales (null si no se aplican)

## Ejecución del Script SQL

### Opción 1: SQL Server Management Studio
1. Abrir SQL Server Management Studio
2. Conectarse a tu instancia (ej: `.\SQLEXPRESS` o `localhost`)
3. Seleccionar base de datos `IndexadorDB`
4. Abrir y ejecutar: `ScriptsDB_ETAPA4_PreparacionLotes.sql`

### Opción 2: Línea de comandos (PowerShell)
```powershell
# Navegar a la carpeta de scripts
cd "C:\Users\danie\source\repos\IndexadorIA\IndexadorIA\00_Archivos"

# Ejecutar el script (ajustar instancia según tu configuración)
sqlcmd -S .\SQLEXPRESS -d IndexadorDB -i ScriptsDB_ETAPA4_PreparacionLotes.sql
```

### Verificación de la instalación

Ejecutar el script de verificación:
```powershell
sqlcmd -S .\SQLEXPRESS -d IndexadorDB -i VERIFICAR_ETAPA4.sql
```

Este script verificará:
- ✅ Secuencia `SEQ_LOTE` creada
- ✅ Tabla `TD_LOTE` creada
- ✅ Tabla `TD_LOTE_ARCHIVOS` creada
- ✅ Estados configurados correctamente
- ✅ Índices creados
- ✅ Archivos disponibles para agrupar

Si todos los checks están en verde (✓), la funcionalidad está lista para usar.
