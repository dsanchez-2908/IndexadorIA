-- ============================================================================
-- Actualiza la vista VW_001_RESULTADO_IA para incluir dsObservaciones
-- (usado en el CSV de finalización de lote - columna "Observaciones").
-- ============================================================================
USE [IndexadorIA]
GO

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
	r.cdEstadoControl,
	r.dsObservaciones
FROM TD_001_RESULTADO_IA r
INNER JOIN TD_ARCHIVOS_PAGINAS ap ON ap.cdArchivoPagina = r.cdArchivoPagina
INNER JOIN TD_ARCHIVOS_ORIGINAL ao ON ao.cdArchivo = ap.cdArchivoOriginal
LEFT JOIN TD_CATEGORIA_PLANO cp ON cp.cdCategoriaPlano = r.cdCategoriaPlano
LEFT JOIN TD_TIPOS_PLANO tp ON tp.cdTipoPlano = r.cdTipoPlano
GO
