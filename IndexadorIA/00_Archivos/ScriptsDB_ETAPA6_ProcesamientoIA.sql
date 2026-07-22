-- =====================================================
-- Script: ETAPA 6 - Procesamiento por OpenAI
-- Descripción: Creación de tablas para procesamiento
--              de planos con OpenAI Batch API
-- Fecha: 2026-07-19
-- =====================================================

USE IndexadorIA
GO

-- =====================================================
-- 1. PARÁMETROS DE OPENAI
-- =====================================================
PRINT 'Insertando parámetros de OpenAI...'

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'OPENAI_API_KEY')
BEGIN
	INSERT INTO TD_PARAMETROS (dsClaveParametro, dsValorParametro, dsDescripcion, feUltimaModificacion, cdUsuarioModificacion)
	VALUES ('OPENAI_API_KEY', 
			'REEMPLAZAR_CON_SU_API_KEY_DE_OPENAI', 
			'API Key de OpenAI para integración', 
			GETDATE(),
			1)
END

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'OPENAI_MODEL')
BEGIN
	INSERT INTO TD_PARAMETROS (dsClaveParametro, dsValorParametro, dsDescripcion, feUltimaModificacion, cdUsuarioModificacion)
	VALUES ('OPENAI_MODEL', 'gpt-4o-mini', 'Modelo de OpenAI a utilizar', GETDATE(), 1)
END

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'MAX_REINTENTOS_API')
BEGIN
	INSERT INTO TD_PARAMETROS (dsClaveParametro, dsValorParametro, dsDescripcion, feUltimaModificacion, cdUsuarioModificacion)
	VALUES ('MAX_REINTENTOS_API', '3', 'Cantidad máxima de reintentos en llamadas a la API', GETDATE(), 1)
END

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'OPENAI_BATCH_TIMEOUT_SECONDS')
BEGIN
	INSERT INTO TD_PARAMETROS (dsClaveParametro, dsValorParametro, dsDescripcion, feUltimaModificacion, cdUsuarioModificacion)
	VALUES ('OPENAI_BATCH_TIMEOUT_SECONDS', '3600', 'Timeout en segundos para esperar resultados de batch (1 hora)', GETDATE(), 1)
END

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'OPENAI_BATCH_CHECK_INTERVAL_SECONDS')
BEGIN
	INSERT INTO TD_PARAMETROS (dsClaveParametro, dsValorParametro, dsDescripcion, feUltimaModificacion, cdUsuarioModificacion)
	VALUES ('OPENAI_BATCH_CHECK_INTERVAL_SECONDS', '30', 'Intervalo en segundos para verificar estado del batch', GETDATE(), 1)
END

PRINT 'Parámetros de OpenAI insertados correctamente.'
GO

-- =====================================================
-- 2. TABLA TD_PROMPT
-- =====================================================
PRINT 'Creando tabla TD_PROMPT...'

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_PROMPT]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TD_PROMPT](
		[cdPrompt] [int] IDENTITY(1,1) NOT NULL,
		[cdProyecto] [int] NOT NULL,
		[dsDescripcion] [nvarchar](500) NULL,
		[dsPrompt] [nvarchar](MAX) NOT NULL,
		[feAlta] [datetime] NOT NULL,
		[cdUsuarioAlta] [int] NOT NULL,
		[feUltimaModificacion] [datetime] NULL,
		[cdUsuarioModificacion] [int] NULL,
		CONSTRAINT [PK_TD_PROMPT] PRIMARY KEY CLUSTERED ([cdPrompt] ASC)
	)

	PRINT 'Tabla TD_PROMPT creada correctamente.'
END
ELSE
BEGIN
	PRINT 'La tabla TD_PROMPT ya existe.'
END
GO

-- Insertar prompt inicial
PRINT 'Insertando prompt inicial...'

IF NOT EXISTS (SELECT 1 FROM TD_PROMPT WHERE cdProyecto = 1)
BEGIN
	INSERT INTO TD_PROMPT (cdProyecto, dsDescripcion, dsPrompt, feAlta, cdUsuarioAlta)
	VALUES (1, 
			'Prompt para extracción de datos de planos arquitectónicos de Buenos Aires',
			'Eres un asistente experto en extracción de datos de planos arquitectónicos de Buenos Aires.
Extraer los siguientes campos del texto OCR del plano:

Reglas de extracción:
- **TipoPlano** solo puede ser: 
  "Obra": En la identificación de Planos de obra, la carátula puede presentar dos variantes de rotulacion: (1) Denominación Genérica: Antecedida por ''Plano de modificacion, ampliacion, conforme, subsistencia, obra, refaccion...'' seguida de su especialidad. (2) Mención Directa: Nombre del proyecto (ej. ''Obra nueva'', ''Ampliacion'', ''Refaccion'')
  "Mensura": En la identificación de Planos de mensura, la carátula puede presentar dos variantes de rotulacion: (1) Denominación Genérica: Antecedida por ''Plano de mensura, subdivision, propiedad horizontal, subsistencia, obra, refaccion...'' seguida de su especialidad. (2) Mención Directa: Nombre del proyecto (ej. ''Mensura particular'', ''Mensura'', ''Subdivision'', ''Propiedad horizontal'').
  "Instalaciones": En la identificación de Planos de Instalación, la carátula puede presentar dos variantes de rotulación: (1) Denominación Genérica: Antecedida por ''Plano de Instalación...'' seguida de su especialidad. (2) Mención Directa: Nombre del servicio proyectado (ej. ''Plano de Electricidad'', ''Plano de Ventilación'').

- **Expediente** puede ser null si no está presente. Sino: Identificación de Planos (Expediente) Para identificar el número de expediente, el indexador debe rastrear la zona inferior del rótulo, teniendo en cuenta las siguientes particularidades (1) Suele ubicarse en el margen inferior derecho o central. Es muy común que el número esté pisado por sellos de mesa de entradas, firmas de analistas/gerentes o sellos de "Aprobado", por lo que requiere una inspección visual detallada (2) Estructura del Dato: El número se identifica por el uso de guiones (-) o barras (/) que separan el número de orden del año (ej. 45.123/78). O bien, Identificación de Planos (Expediente de mensura) En él caso de los expedientes de mensura no debe tomarse él numero de expediente sino qué debe usarse de referencia él numero de MH o M (1) Suele ubicarse en el margen inferior derecho o central. Es muy común que el número esté pisado por sellos de mesa de entradas, firmas de analistas/gerentes o sellos de "Aprobado", por lo que requiere una inspección visual detallada (2) Estructura del Dato: El número se identifica por qué comienza con MH o M.

- **Seccion** puede aparecer como: Secc, Sección, S, Secc:, Sección:
- **Manzana** puede aparecer como: Manz, M, Manz:
- **Parcela** puede aparecer como: Par, P, Parc, Por, Parcela, Par:, P:
- **Direccion**: En la identificación de Planos, la carátula puede presentar dos variantes de titulación: (1) Identificación Explícita: La dirección aparece antecedida por etiquetas como "Calle:", "Ubicación:" o "Lugar:". (2) Identificación Directa: Los datos del domicilio (calle y número) figuran de forma aislada, sin referencias previas, generalmente en la parte superior o central del rótulo. Puede ser una o mas calles o una o mas alturas.

Además, para los campos "Seccion", "Manzana" y "Parcela": En la identificación de Planos de Instalación, la carátula puede presentar tres variantes de datos catastrales: (1) Identificación SMP (3 grupos): Cuando se observan 3 juegos de números, corresponden correlativamente a Sección, Manzana y Parcela. Estos datos deben cargarse en su totalidad. (2) Identificación con Circunscripción (4 grupos): En algunos planos, pueden aparecer 4 juegos de números. El primero de ellos corresponde a la Circunscripción, dato que no debe cargarse. En estos casos, se deben omitir los primeros dígitos y cargar únicamente los 3 grupos restantes (SMP). (3) Ausencia de Etiquetas: Los valores pueden aparecer identificados con sus nombres ("S:", "M:", "P:") o figurar simplemente como números aislados dentro de los casilleros correspondientes del rótulo.

Para cada campo extraído, proporciona un nivel de confianza entre 0.00 y 1.00.

Responder SOLO con JSON en el siguiente formato exacto:
{
  "archivo": "nombre_del_archivo.pdf",
  "tipoPlano": "Obra",
  "expediente": "EX-2006-00041690-MGEYA-DGROC",
  "seccion": "27",
  "manzana": "101",
  "parcela": "006 A",
  "direccion": "AV. DEL LIBERTADOR 6755/57",
  "confianza": {
	"tipoPlano": 0.99,
	"expediente": 0.95,
	"seccion": 0.98,
	"manzana": 0.98,
	"parcela": 0.97,
	"direccion": 0.96
  }
}',
			GETDATE(),
			1) -- Usuario sistema

	PRINT 'Prompt inicial insertado correctamente.'
END
ELSE
BEGIN
	PRINT 'Ya existe un prompt para el proyecto 1.'
END
GO

-- =====================================================
-- 3. TABLA TD_TIPOS_PLANO
-- =====================================================
PRINT 'Creando tabla TD_TIPOS_PLANO...'

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_TIPOS_PLANO]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TD_TIPOS_PLANO](
		[cdTipoPlano] [int] IDENTITY(1,1) NOT NULL,
		[dsTipoPlano] [nvarchar](50) NOT NULL,
		[snActivo] [bit] NOT NULL DEFAULT 1,
		[dsDescripcion] [nvarchar](500) NULL,
		[feAlta] [datetime] NOT NULL DEFAULT GETDATE(),
		CONSTRAINT [PK_TD_TIPOS_PLANO] PRIMARY KEY CLUSTERED ([cdTipoPlano] ASC)
	)

	PRINT 'Tabla TD_TIPOS_PLANO creada correctamente.'
END
ELSE
BEGIN
	PRINT 'La tabla TD_TIPOS_PLANO ya existe.'
END
GO

-- Insertar tipos de plano
PRINT 'Insertando tipos de plano...'

SET IDENTITY_INSERT [dbo].[TD_TIPOS_PLANO] ON 
GO

IF NOT EXISTS (SELECT 1 FROM TD_TIPOS_PLANO WHERE cdTipoPlano = 1)
BEGIN
	INSERT [dbo].[TD_TIPOS_PLANO] ([cdTipoPlano], [dsTipoPlano], [snActivo], [dsDescripcion], [feAlta]) 
	VALUES (1, N'Obra', 1, N'Plano de obra nueva, ampliación, refacción, etc.', GETDATE())
END

IF NOT EXISTS (SELECT 1 FROM TD_TIPOS_PLANO WHERE cdTipoPlano = 2)
BEGIN
	INSERT [dbo].[TD_TIPOS_PLANO] ([cdTipoPlano], [dsTipoPlano], [snActivo], [dsDescripcion], [feAlta]) 
	VALUES (2, N'Mensura', 1, N'Plano de mensura catastral, subdivisión, propiedad horizontal', GETDATE())
END

IF NOT EXISTS (SELECT 1 FROM TD_TIPOS_PLANO WHERE cdTipoPlano = 3)
BEGIN
	INSERT [dbo].[TD_TIPOS_PLANO] ([cdTipoPlano], [dsTipoPlano], [snActivo], [dsDescripcion], [feAlta]) 
	VALUES (3, N'Instalaciones', 1, N'Plano de instalaciones (electricidad, gas, ventilación, etc.)', GETDATE())
END

SET IDENTITY_INSERT [dbo].[TD_TIPOS_PLANO] OFF
GO

PRINT 'Tipos de plano insertados correctamente.'
GO

-- =====================================================
-- 4. TABLA TD_001_RESULTADO_IA
-- =====================================================
PRINT 'Creando tabla TD_001_RESULTADO_IA...'

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TD_001_RESULTADO_IA](
		[cdResultado] [int] IDENTITY(1,1) NOT NULL,
		[cdLote] [int] NOT NULL,
		[cdArchivoPagina] [int] NOT NULL,

		[cdTipoPlano] [int] NULL,
		[dsExpediente] [nvarchar](100) NULL,
		[dsSeccion] [nvarchar](50) NULL,
		[dsManzana] [nvarchar](50) NULL,
		[dsParcela] [nvarchar](50) NULL,
		[dsDireccion] [nvarchar](500) NULL,

		[nuConfianzaTipoPlano] [decimal](5, 4) NULL,
		[nuConfianzaExpediente] [decimal](5, 4) NULL,
		[nuConfianzaSeccion] [decimal](5, 4) NULL,
		[nuConfianzaManzana] [decimal](5, 4) NULL,
		[nuConfianzaParcela] [decimal](5, 4) NULL,
		[nuConfianzaDireccion] [decimal](5, 4) NULL,

		[feAlta] [datetime] NOT NULL DEFAULT GETDATE(),
		[cdUsuarioAlta] [int] NOT NULL,
		[feUltimaModificacion] [datetime] NULL,
		[cdUsuarioModificacion] [int] NULL,

		CONSTRAINT [PK_TD_001_RESULTADO_IA] PRIMARY KEY CLUSTERED ([cdResultado] ASC),
		CONSTRAINT [FK_TD_001_RESULTADO_IA_LOTE] FOREIGN KEY([cdLote]) 
			REFERENCES [dbo].[TD_LOTE] ([cdLote]),
		CONSTRAINT [FK_TD_001_RESULTADO_IA_ARCHIVO_PAGINA] FOREIGN KEY([cdArchivoPagina]) 
			REFERENCES [dbo].[TD_ARCHIVOS_PAGINAS] ([cdArchivoPagina]),
		CONSTRAINT [FK_TD_001_RESULTADO_IA_TIPO_PLANO] FOREIGN KEY([cdTipoPlano]) 
			REFERENCES [dbo].[TD_TIPOS_PLANO] ([cdTipoPlano])
	)

	-- Índices para mejorar rendimiento
	CREATE NONCLUSTERED INDEX [IX_TD_001_RESULTADO_IA_LOTE] 
		ON [dbo].[TD_001_RESULTADO_IA]([cdLote] ASC)

	CREATE NONCLUSTERED INDEX [IX_TD_001_RESULTADO_IA_ARCHIVO_PAGINA] 
		ON [dbo].[TD_001_RESULTADO_IA]([cdArchivoPagina] ASC)

	PRINT 'Tabla TD_001_RESULTADO_IA creada correctamente.'
END
ELSE
BEGIN
	PRINT 'La tabla TD_001_RESULTADO_IA ya existe.'
END
GO

-- =====================================================
-- 5. TABLA TD_TOKEN
-- =====================================================
PRINT 'Creando tabla TD_TOKEN...'

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_TOKEN]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TD_TOKEN](
		[cdToken] [int] IDENTITY(1,1) NOT NULL,
		[cdResultado] [int] NOT NULL,
		[nuTokensPrompt] [int] NULL,
		[nuTokensCompletion] [int] NULL,
		[nuTokensTotal] [int] NULL,
		[feAlta] [datetime] NOT NULL DEFAULT GETDATE(),

		CONSTRAINT [PK_TD_TOKEN] PRIMARY KEY CLUSTERED ([cdToken] ASC),
		CONSTRAINT [FK_TD_TOKEN_RESULTADO] FOREIGN KEY([cdResultado]) 
			REFERENCES [dbo].[TD_001_RESULTADO_IA] ([cdResultado])
	)

	-- Índice para reportes de consumo
	CREATE NONCLUSTERED INDEX [IX_TD_TOKEN_RESULTADO] 
		ON [dbo].[TD_TOKEN]([cdResultado] ASC)

	PRINT 'Tabla TD_TOKEN creada correctamente.'
END
ELSE
BEGIN
	PRINT 'La tabla TD_TOKEN ya existe.'
END
GO

-- =====================================================
-- 6. ESTADOS PARA PROCESAMIENTO IA
-- =====================================================
PRINT 'Actualizando estados en TD_ESTADOS...'

-- Estado 3: Procesando con IA
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 3)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 3, 'Procesando con IA')
	PRINT 'Estado LOTE 3 (Procesando con IA) creado.'
END

-- Estado 4: Procesado por IA
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 4)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 4, 'Procesado por IA')
	PRINT 'Estado LOTE 4 (Procesado por IA) creado.'
END

-- Estado 4 para ARCHIVO_PAGINA: Procesado por IA
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 4)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('ARCHIVO_PAGINA', 4, 'Procesado por IA')
	PRINT 'Estado ARCHIVO_PAGINA 4 (Procesado por IA) creado.'
END

PRINT 'Estados actualizados correctamente.'
GO

-- =====================================================
-- FIN DEL SCRIPT
-- =====================================================
PRINT ''
PRINT '======================================================'
PRINT 'ETAPA 6 - Procesamiento por OpenAI'
PRINT 'Script ejecutado correctamente.'
PRINT '======================================================'
PRINT ''
GO
