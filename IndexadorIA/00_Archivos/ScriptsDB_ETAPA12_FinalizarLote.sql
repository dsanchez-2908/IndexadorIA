-- ============================================================================
-- ETAPA 12: Pantalla "Finalizar Lote"
-- Vista de metadatos para generacion de CSV y renombrado final de archivos.
-- ============================================================================

USE [IndexadorIA]
GO

-- ============================================================================
-- 1) Validacion defensiva: columna dsNombreArchivoFinal en TD_ARCHIVOS_PAGINAS
--    (el usuario indico que ya la creo manualmente; este bloque es solo
--     un resguardo por si faltara en algun ambiente)
-- ============================================================================
IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID(N'[dbo].[TD_ARCHIVOS_PAGINAS]') AND name = 'dsNombreArchivoFinal'
)
BEGIN
	ALTER TABLE [dbo].[TD_ARCHIVOS_PAGINAS] ADD [dsNombreArchivoFinal] [varchar](500) NULL
	PRINT 'Columna dsNombreArchivoFinal agregada a TD_ARCHIVOS_PAGINAS.'
END
ELSE
BEGIN
	PRINT 'Columna dsNombreArchivoFinal ya existe en TD_ARCHIVOS_PAGINAS.'
END
GO

-- ============================================================================
-- 2) Vista VW_001_RESULTADO_IA: metadatos completos por pagina/resultado,
--    usada para generar los CSV de finalizacion de lote.
-- ============================================================================
IF EXISTS (SELECT 1 FROM sys.views WHERE name = 'VW_001_RESULTADO_IA')
BEGIN
	DROP VIEW [dbo].[VW_001_RESULTADO_IA]
END
GO

CREATE VIEW [dbo].[VW_001_RESULTADO_IA]
AS
SELECT
	r.cdResultado,
	r.cdLote,
	r.cdArchivoPagina,
	ap.dsRutaCompleta,
	ao.dsNombreArchivo   AS dsNombreArchivoOriginal,
	ap.dsNombreArchivoFinal,
	r.cdCategoriaPlano,
	cp.dsCategoriaPlano,
	r.cdTipoPlano,
	tp.dsTipoPlano,
	tp.dsAcronimo,
	r.dsDireccion,
	r.dsSeccion,
	r.dsManzana,
	r.dsParcela,
	r.dsExpediente,
	r.dsNumeroPlano,
	r.cdEstadoControl
FROM TD_001_RESULTADO_IA r
INNER JOIN TD_ARCHIVOS_PAGINAS ap ON ap.cdArchivoPagina = r.cdArchivoPagina
INNER JOIN TD_ARCHIVOS_ORIGINAL ao ON ao.cdArchivo = ap.cdArchivoOriginal
LEFT JOIN TD_CATEGORIA_PLANO cp ON cp.cdCategoriaPlano = r.cdCategoriaPlano
LEFT JOIN TD_TIPOS_PLANO tp ON tp.cdTipoPlano = r.cdTipoPlano
GO

SELECT TOP 20 * FROM VW_001_RESULTADO_IA;
GO
