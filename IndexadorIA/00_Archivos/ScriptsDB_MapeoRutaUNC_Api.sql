-- =====================================================
-- Script: Mapeo de ruta de unidad mapeada a UNC para la API
-- Descripcion: La API (IIS/Application Pool) no ve unidades de red
--              mapeadas por sesion interactiva (ej: "P:\"), por lo que
--              se define un mapeo configurable de prefijo de ruta que
--              ArchivosController usa para resolver la ruta fisica real
--              antes de leer PDFs/JPGs desde el filesystem.
-- Fecha: 2026-08-28
-- =====================================================

USE IndexadorIA
GO

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'RUTA_ORIGEN_UNIDAD')
BEGIN
	INSERT INTO TD_PARAMETROS (dsClaveParametro, dsValorParametro, dsDescripcion, feUltimaModificacion, cdUsuarioModificacion)
	VALUES ('RUTA_ORIGEN_UNIDAD',
			'P:\',
			'Prefijo de ruta (unidad mapeada) tal como esta guardado en TD_002_ARCHIVOS_PAGINA.dsRutaCompleta',
			GETDATE(),
			1)
END

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'RUTA_DESTINO_UNC')
BEGIN
	INSERT INTO TD_PARAMETROS (dsClaveParametro, dsValorParametro, dsDescripcion, feUltimaModificacion, cdUsuarioModificacion)
	VALUES ('RUTA_DESTINO_UNC',
			'\\FS-CLUSTER01\Recursos3\',
			'Prefijo UNC equivalente que usa la API (proceso de IIS) para acceder al mismo recurso compartido',
			GETDATE(),
			1)
END
