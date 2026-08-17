-- =====================================================
-- Script: ETAPA 9 - Categor�a de Plano y Tipo de Plano (nuevo modelo) + N�mero de Plano
-- Descripci�n: 
--   El usuario ya renombr� manualmente la tabla TD_TIPOS_PLANO (Obra/Mensura/Instalaciones)
--   a TD_CATEGORIA_PLANO (campos cdCategoriaPlano, dsCategoriaPlano) y cre� una NUEVA
--   tabla TD_TIPOS_PLANO con el listado real de tipos de plano por categor�a.
--   Este script:
--     1. Verifica/crea la FK entre TD_TIPOS_PLANO.cdCategoriaPlano y TD_CATEGORIA_PLANO.cdCategoriaPlano
--     2. Verifica/crea las PK de ambas tablas
--     3. Renombra en TD_001_RESULTADO_IA la columna cdTipoPlano (y su confianza) a cdCategoriaPlano,
--        ya que hist�ricamente esa columna apuntaba a la tabla de 3 valores (ahora TD_CATEGORIA_PLANO)
--     4. Agrega las columnas nuevas: cdTipoPlano (FK a la nueva TD_TIPOS_PLANO), nuConfianzaTipoPlano,
--        dsNumeroPlano, nuConfianzaNumeroPlano
--     5. Recrea las FK de TD_001_RESULTADO_IA apuntando correctamente
--     6. Borra los datos de desarrollo existentes en TD_001_RESULTADO_IA y TD_TOKEN relacionado
-- Fecha: 2026-08-01
-- =====================================================

USE [IndexadorIA]
GO

-- =====================================================
-- 1. FK entre TD_TIPOS_PLANO (nueva) y TD_CATEGORIA_PLANO
-- =====================================================
PRINT 'Verificando FK entre TD_TIPOS_PLANO y TD_CATEGORIA_PLANO...'

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_TIPOS_PLANO_CATEGORIA_PLANO')
BEGIN
	IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_TIPOS_PLANO]') AND name = 'cdCategoriaPlano')
	BEGIN
		ALTER TABLE [dbo].[TD_TIPOS_PLANO]
		ADD CONSTRAINT [FK_TD_TIPOS_PLANO_CATEGORIA_PLANO] FOREIGN KEY([cdCategoriaPlano])
			REFERENCES [dbo].[TD_CATEGORIA_PLANO] ([cdCategoriaPlano])

		PRINT 'FK_TD_TIPOS_PLANO_CATEGORIA_PLANO creada correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'ADVERTENCIA: TD_TIPOS_PLANO no tiene columna cdCategoriaPlano. Revisar estructura manualmente.'
	END
END
ELSE
BEGIN
	PRINT 'La FK FK_TD_TIPOS_PLANO_CATEGORIA_PLANO ya existe.'
END
GO

-- =====================================================
-- 2. Verificar PK de TD_CATEGORIA_PLANO
-- =====================================================
PRINT 'Verificando PK de TD_CATEGORIA_PLANO...'

IF NOT EXISTS (
	SELECT 1 FROM sys.key_constraints 
	WHERE type = 'PK' AND parent_object_id = OBJECT_ID(N'[dbo].[TD_CATEGORIA_PLANO]')
)
BEGIN
	ALTER TABLE [dbo].[TD_CATEGORIA_PLANO]
	ADD CONSTRAINT [PK_TD_CATEGORIA_PLANO] PRIMARY KEY CLUSTERED ([cdCategoriaPlano] ASC)

	PRINT 'PK_TD_CATEGORIA_PLANO creada correctamente.'
END
ELSE
BEGIN
	PRINT 'TD_CATEGORIA_PLANO ya tiene PK definida.'
END
GO

-- =====================================================
-- 3. Verificar PK de TD_TIPOS_PLANO (nueva)
-- =====================================================
PRINT 'Verificando PK de TD_TIPOS_PLANO...'

IF NOT EXISTS (
	SELECT 1 FROM sys.key_constraints 
	WHERE type = 'PK' AND parent_object_id = OBJECT_ID(N'[dbo].[TD_TIPOS_PLANO]')
)
BEGIN
	ALTER TABLE [dbo].[TD_TIPOS_PLANO]
	ADD CONSTRAINT [PK_TD_TIPOS_PLANO] PRIMARY KEY CLUSTERED ([cdTipoPlano] ASC)

	PRINT 'PK_TD_TIPOS_PLANO creada correctamente.'
END
ELSE
BEGIN
	PRINT 'TD_TIPOS_PLANO ya tiene PK definida.'
END
GO

-- =====================================================
-- 4. Borrado de datos de desarrollo (ambiente sin datos productivos)
--    Se borra primero TD_TOKEN (depende de TD_001_RESULTADO_IA) y luego TD_001_RESULTADO_IA
-- =====================================================
PRINT 'Borrando datos de desarrollo de TD_TOKEN y TD_001_RESULTADO_IA...'

DELETE FROM TD_TOKEN WHERE cdResultado IN (SELECT cdResultado FROM TD_001_RESULTADO_IA)
DELETE FROM TD_001_RESULTADO_IA

PRINT 'Datos de desarrollo eliminados.'
GO

-- =====================================================
-- 5. Quitar la FK vieja de TD_001_RESULTADO_IA hacia la tabla de categor�as
--    (fue creada originalmente como FK_TD_001_RESULTADO_IA_TIPO_PLANO -> TD_TIPOS_PLANO,
--     pero esa tabla es ahora TD_CATEGORIA_PLANO)
-- =====================================================
PRINT 'Quitando FK antigua FK_TD_001_RESULTADO_IA_TIPO_PLANO...'

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_001_RESULTADO_IA_TIPO_PLANO')
BEGIN
	ALTER TABLE [dbo].[TD_001_RESULTADO_IA]
	DROP CONSTRAINT [FK_TD_001_RESULTADO_IA_TIPO_PLANO]

	PRINT 'FK antigua eliminada.'
END
GO

-- =====================================================
-- 6. Renombrar cdTipoPlano -> cdCategoriaPlano (y su confianza) en TD_001_RESULTADO_IA
--    Hist�ricamente esta columna representaba la categor�a (Obra/Mensura/Instalaciones)
-- =====================================================
PRINT 'Renombrando columnas cdTipoPlano/nuConfianzaTipoPlano a cdCategoriaPlano/nuConfianzaCategoriaPlano...'

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'cdTipoPlano')
   AND NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'cdCategoriaPlano')
BEGIN
	EXEC sp_rename 'dbo.TD_001_RESULTADO_IA.cdTipoPlano', 'cdCategoriaPlano', 'COLUMN'
	PRINT 'Columna cdTipoPlano renombrada a cdCategoriaPlano.'
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'nuConfianzaTipoPlano')
   AND NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'nuConfianzaCategoriaPlano')
BEGIN
	EXEC sp_rename 'dbo.TD_001_RESULTADO_IA.nuConfianzaTipoPlano', 'nuConfianzaCategoriaPlano', 'COLUMN'
	PRINT 'Columna nuConfianzaTipoPlano renombrada a nuConfianzaCategoriaPlano.'
END
GO

-- =====================================================
-- 7. Agregar columnas nuevas: cdTipoPlano (real), nuConfianzaTipoPlano, dsNumeroPlano, nuConfianzaNumeroPlano
-- =====================================================
PRINT 'Agregando columnas nuevas a TD_001_RESULTADO_IA...'

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'cdTipoPlano')
BEGIN
	ALTER TABLE [dbo].[TD_001_RESULTADO_IA] ADD [cdTipoPlano] [int] NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'nuConfianzaTipoPlano')
BEGIN
	ALTER TABLE [dbo].[TD_001_RESULTADO_IA] ADD [nuConfianzaTipoPlano] [decimal](5, 4) NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'dsNumeroPlano')
BEGIN
	ALTER TABLE [dbo].[TD_001_RESULTADO_IA] ADD [dsNumeroPlano] [nvarchar](100) NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_001_RESULTADO_IA]') AND name = 'nuConfianzaNumeroPlano')
BEGIN
	ALTER TABLE [dbo].[TD_001_RESULTADO_IA] ADD [nuConfianzaNumeroPlano] [decimal](5, 4) NULL
END
GO

PRINT 'Columnas nuevas agregadas correctamente.'
GO

-- =====================================================
-- 8. Recrear las FK de TD_001_RESULTADO_IA hacia TD_CATEGORIA_PLANO y TD_TIPOS_PLANO
-- =====================================================
PRINT 'Creando FKs de TD_001_RESULTADO_IA hacia TD_CATEGORIA_PLANO y TD_TIPOS_PLANO...'

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_001_RESULTADO_IA_CATEGORIA_PLANO')
BEGIN
	ALTER TABLE [dbo].[TD_001_RESULTADO_IA]
	ADD CONSTRAINT [FK_TD_001_RESULTADO_IA_CATEGORIA_PLANO] FOREIGN KEY([cdCategoriaPlano])
		REFERENCES [dbo].[TD_CATEGORIA_PLANO] ([cdCategoriaPlano])

	PRINT 'FK_TD_001_RESULTADO_IA_CATEGORIA_PLANO creada correctamente.'
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_001_RESULTADO_IA_TIPO_PLANO_NUEVO')
BEGIN
	ALTER TABLE [dbo].[TD_001_RESULTADO_IA]
	ADD CONSTRAINT [FK_TD_001_RESULTADO_IA_TIPO_PLANO_NUEVO] FOREIGN KEY([cdTipoPlano])
		REFERENCES [dbo].[TD_TIPOS_PLANO] ([cdTipoPlano])

	PRINT 'FK_TD_001_RESULTADO_IA_TIPO_PLANO_NUEVO creada correctamente.'
END
GO

PRINT '========================================================='
PRINT 'ETAPA 9 completada: TD_001_RESULTADO_IA actualizada.'
PRINT 'Verificar manualmente el contenido de TD_CATEGORIA_PLANO y TD_TIPOS_PLANO antes de reprocesar lotes.'
PRINT '========================================================='
GO
