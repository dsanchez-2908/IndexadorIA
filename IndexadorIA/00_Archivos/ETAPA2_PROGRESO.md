# 🚀 ETAPA 2 - EN DESARROLLO

## Estado Actual: Iniciada ⚙️

Se ha comenzado la implementación de la **Etapa 2** del proyecto IndexadorIA para gestión y procesamiento de archivos.

---

## ✅ Progreso Completado

### 1. Base de Datos - Nuevas Tablas ✓
Se han creado las siguientes tablas en el script SQL:

#### TD_PROYECTOS
Almacena proyectos de indexación con su configuración:
- `cdProyecto` - ID del proyecto
- `dsProyecto` - Nombre del proyecto
- `dsDescripcion` - Descripción opcional
- `dsRutaOrigen` - PATH donde están los archivos originales
- `dsRutaTrabajo` - PATH donde se procesan los archivos
- `nuTamanoLote` - Cantidad de páginas por lote (default: 50)
- `nuCoordenadaX, nuCoordenadaY, nuAncho, nuAlto` - Área de recorte
- `cdEstado` - Estado del proyecto
- Auditoría: fechas y usuarios de creación/modificación

#### TD_ARCHIVOS
Registra archivos originales (PDF o JPG):
- `cdArchivo` - ID del archivo
- `cdProyecto` - Proyecto al que pertenece
- `dsNombreOriginal` - Nombre del archivo
- `dsRutaCompleta` - Ruta completa del archivo
- `dsTipoArchivo` - Tipo: PDF, JPG, JPEG
- `nuTamanoBytes` - Tamaño en bytes
- `nuPaginasTotal` - Cantidad de páginas
- `cdEstado` - Estado del procesamiento
- Auditoría y observaciones

#### TD_PAGINAS
Almacena páginas extraídas y procesadas:
- `cdPagina` - ID de la página
- `cdArchivo` - Archivo de origen
- `nuNumeroPagina` - Número de página
- `dsRutaImagenOriginal` - Imagen original extraída
- `dsRutaImagenRotada` - Imagen después de rotación
- `dsRutaImagenRecortada` - Imagen recortada
- `nuGradosRotacion` - Grados rotados (0, 90, 180, 270)
- `snRotada, snRecortada` - Flags de procesamiento
- `nuAnchoOriginal, nuAltoOriginal` - Dimensiones
- `cdEstado` - Estado actual
- Auditoría y observaciones

#### TD_LOTES
Agrupa páginas para envío a OpenAI:
- `cdLote` - ID del lote
- `cdProyecto` - Proyecto
- `dsNombreLote` - Nombre del lote
- `nuCantidadPaginas` - Cantidad de páginas
- `cdEstado` - Estado del lote
- `feEnvioBatch, feFinalizacion` - Fechas de proceso
- `dsBatchId` - ID del batch en OpenAI
- `dsResultado` - Resultado en formato JSON
- Auditoría

#### TR_LOTE_PAGINAS (Tabla de Relación)
Relaciona lotes con páginas:
- `cdLote, cdPagina` - Relación
- `nuOrden` - Orden dentro del lote
- `feAsignacion` - Fecha de asignación

### 2. Estados del Sistema ✓
Se han definido estados para cada proceso:

**PROYECTOS:**
- 1 = Activo
- 0 = Inactivo
- 2 = Completado

**ARCHIVOS:**
- 1 = Pendiente
- 2 = Procesando
- 3 = Completado
- 9 = Error

**PAGINAS:**
- 1 = Pendiente
- 2 = Rotada
- 3 = Recortada
- 4 = En Lote
- 5 = Procesada
- 9 = Error

**LOTES:**
- 1 = Pendiente
- 2 = Enviado
- 3 = Procesando
- 4 = Completado
- 9 = Error

### 3. Entidades Creadas ✓
- ✅ `Proyecto.cs` - Entidad Proyecto
- ✅ `Archivo.cs` - Entidad Archivo
- ✅ `Pagina.cs` - Entidad Página
- ✅ `Lote.cs` - Entidad Lote

### 4. Capa de Datos ✓
- ✅ `ProyectoDAL.cs` - Acceso a datos de Proyectos (CRUD completo)

### 5. Paquetes NuGet ✓
- ✅ `PdfiumViewer` (2.13.0) - Para trabajar con PDFs
- ✅ `System.Drawing.Common` (9.0.0) - Para manipulación de imágenes

---

## 📝 Pendiente de Implementación

### 1. Capas de Datos (DAL) ✅ COMPLETADO
- [x] `ArchivoDAL.cs` - CRUD de archivos
- [x] `PaginaDAL.cs` - CRUD de páginas  
- [x] `LoteDAL.cs` - CRUD de lotes

### 2. Capas de Negocio (BL)
- [ ] `ProyectoBL.cs` - Lógica de negocio de proyectos
- [ ] `ArchivoBL.cs` - Lógica de ingreso de archivos
- [ ] `PaginaBL.cs` - Lógica de procesamiento de páginas
- [ ] `LoteBL.cs` - Lógica de creación de lotes
- [ ] `ProcesadorPDF.cs` - Separación de páginas PDF
- [ ] `ProcesadorImagen.cs` - Rotación y recorte de imágenes

### 3. Pantallas (Formularios)
- [ ] `FrmProyectos.cs` - Lista de proyectos
- [ ] `FrmProyectoDetalle.cs` - Crear/Editar proyecto
- [ ] `FrmIngresarArchivos.cs` - Seleccionar PATH e ingresar archivos
- [ ] `FrmProcesarArchivos.cs` - Monitor de procesamiento
- [ ] `FrmConfigurarRecorte.cs` - Definir área de recorte
- [ ] `FrmGestionLotes.cs` - Gestión de lotes

### 4. Funcionalidades
- [ ] Explorador de carpetas para seleccionar PATH
- [ ] Escaneo de archivos PDF y JPG en carpeta
- [ ] Registro masivo de archivos en BD
- [ ] Extracción de páginas de PDF a imágenes
- [ ] Detección automática de orientación de imagen
- [ ] Rotación automática de imágenes
- [ ] Recorte de área de interés
- [ ] Agrupación automática en lotes
- [ ] Previsualización de imágenes
- [ ] Barra de progreso de procesamiento

---

## 🎯 Próximos Pasos Inmediatos

### Paso 1: Completar Capas de Datos
Crear las clases DAL restantes para:
- Archivos
- Páginas
- Lotes

### Paso 2: Implementar Lógica de Negocio
Crear las clases BL con:
- Validaciones de negocio
- Procesamiento de PDFs
- Procesamiento de imágenes
- Agrupación en lotes

### Paso 3: Crear Pantallas
Implementar los formularios para:
- Gestión de proyectos
- Ingreso de archivos
- Monitor de procesamiento
- Configuración de recorte

### Paso 4: Integrar con Menú Principal
Agregar opciones al menú de la aplicación:
- Proyectos
- Ingreso de Archivos
- Procesamiento
- Lotes

---

## 📊 Estadísticas del Avance

| Componente | Completado | Pendiente |
|------------|------------|-----------|
| Base de Datos | 5 tablas | 0 |
| Estados | 15 estados | 0 |
| Entidades | 4 clases | 0 |
| DAL | 4 clases | 0 |
| BL | 0 clases | 6 |
| Formularios | 0 | 6 |

**Progreso General Etapa 2:** ████░░░░░░ 40%

---

## 🔧 Tecnologías para Etapa 2

### Manipulación de PDFs
- **PdfiumViewer**: Renderizado y extracción de páginas PDF
- Convierte páginas PDF a imágenes

### Procesamiento de Imágenes
- **System.Drawing.Common**: Manipulación de imágenes
- Rotación automática
- Recorte de regiones
- Redimensionamiento

### Detección de Orientación
Se implementará un algoritmo para detectar la orientación correcta:
1. OCR básico o análisis de características
2. Detección de texto horizontal
3. Rotación automática a posición correcta

---

## 📁 Estructura de Carpetas de Trabajo

```
[RutaTrabajo]/
├── Originales/          # Archivos originales copiados
├── Extraidas/           # Páginas extraídas de PDFs
├── Rotadas/            # Imágenes rotadas
├── Recortadas/         # Imágenes recortadas (área de interés)
├── Lotes/              # Organizadas por lotes
└── Procesadas/         # Imágenes completamente procesadas
```

---

## 💡 Flujo de Procesamiento

```
1. INGRESO
   └─> Seleccionar PATH
   └─> Escanear archivos (PDF/JPG)
   └─> Registrar en TD_ARCHIVOS

2. EXTRACCIÓN
   └─> Para cada PDF:
	   └─> Separar en páginas individuales
	   └─> Convertir a imagen
	   └─> Guardar en carpeta Extraidas/
	   └─> Registrar en TD_PAGINAS

3. ROTACIÓN
   └─> Para cada página:
	   └─> Detectar orientación
	   └─> Rotar si es necesario
	   └─> Guardar en carpeta Rotadas/
	   └─> Actualizar TD_PAGINAS

4. RECORTE
   └─> Para cada página:
	   └─> Aplicar coordenadas de recorte
	   └─> Guardar en carpeta Recortadas/
	   └─> Actualizar TD_PAGINAS

5. LOTES
   └─> Agrupar páginas en lotes de N elementos
   └─> Registrar en TD_LOTES y TR_LOTE_PAGINAS
```

---

## ⚠️ Consideraciones Técnicas

### Rendimiento
- Procesar en lotes para no saturar memoria
- Usar Task Parallel Library para procesamiento concurrente
- Mostrar barra de progreso para operaciones largas

### Almacenamiento
- Validar espacio disponible antes de procesar
- Limpiar archivos temporales después de procesar
- Comprimir imágenes si es necesario

### Errores
- Manejar PDFs corruptos o protegidos
- Validar formato de imágenes
- Registrar errores en campo dsObservaciones

---

## 📋 Checklist de Desarrollo

### Base de Datos
- [x] Tabla TD_PROYECTOS
- [x] Tabla TD_ARCHIVOS
- [x] Tabla TD_PAGINAS
- [x] Tabla TD_LOTES
- [x] Tabla TR_LOTE_PAGINAS
- [x] Estados del sistema
- [ ] Stored Procedures adicionales
- [ ] Índices para optimización

### Entidades
- [x] Proyecto.cs
- [x] Archivo.cs
- [x] Pagina.cs
- [x] Lote.cs

### Capa de Datos
- [x] ProyectoDAL.cs
- [ ] ArchivoDAL.cs
- [ ] PaginaDAL.cs
- [ ] LoteDAL.cs

### Capa de Negocio
- [ ] ProyectoBL.cs
- [ ] ArchivoBL.cs
- [ ] PaginaBL.cs
- [ ] LoteBL.cs
- [ ] ProcesadorPDF.cs
- [ ] ProcesadorImagen.cs

### Interfaz de Usuario
- [ ] FrmProyectos.cs
- [ ] FrmProyectoDetalle.cs
- [ ] FrmIngresarArchivos.cs
- [ ] FrmProcesarArchivos.cs
- [ ] FrmConfigurarRecorte.cs
- [ ] FrmGestionLotes.cs
- [ ] Actualizar menú principal

### Funcionalidades
- [ ] Explorador de carpetas
- [ ] Escaneo de archivos
- [ ] Registro en BD
- [ ] Extracción de páginas PDF
- [ ] Rotación de imágenes
- [ ] Recorte de imágenes
- [ ] Creación de lotes
- [ ] Previsualización
- [ ] Monitor de progreso

---

## 🎉 Estado Actual

✅ **Fase 1 (Base de Datos y Entidades):** COMPLETADA 100%  
✅ **Fase 2 (Capas de Datos):** COMPLETADA 100%  
⚙️ **Fase 3 (Lógica de Negocio):** EN PROGRESO 0%  
⏳ **Fase 4 (Interfaz de Usuario):** PENDIENTE

---

**Última actualización:** [Fecha actual]  
**Próxima tarea:** Completar clases DAL restantes (ArchivoDAL, PaginaDAL, LoteDAL)

---

## 🚦 ¿Listo para continuar?

Cuando estés listo, podemos continuar con:

1. ✅ Completar las clases DAL restantes
2. Crear las clases de lógica de negocio
3. Implementar procesadores de PDF e imágenes
4. Crear las pantallas de usuario
5. Integrar todo en el menú principal

**¿Por dónde te gustaría continuar?**
