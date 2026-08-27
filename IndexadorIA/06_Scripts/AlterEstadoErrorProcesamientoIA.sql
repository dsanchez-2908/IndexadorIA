-- =====================================================================
-- Script: AlterEstadoErrorProcesamientoIA.sql
-- Objetivo: Blindar el procesamiento de resultados de OpenAI para que
--           las paginas que fallan el parseo de JSON (o vienen con error
--           de OpenAI) queden marcadas explicitamente con un estado de
--           error, en lugar de perderse silenciosamente.
-- =====================================================================

-- 1) Nuevo estado de error para TD_ARCHIVOS_PAGINAS (dsProceso = 'ARCHIVO_PAGINA')
IF NOT EXISTS (
	SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 5
)
BEGIN
	INSERT INTO TD_ESTADOS (cdEstado, dsProceso, dsEstado)
	VALUES (5, 'ARCHIVO_PAGINA', 'Error de Procesamiento IA');
END
GO

-- 2) Tabla para registrar el detalle de los fallos de parseo/procesamiento
IF NOT EXISTS (
	SELECT 1 FROM sys.tables WHERE name = 'TD_001_RESULTADO_IA_ERROR'
)
BEGIN
	CREATE TABLE TD_001_RESULTADO_IA_ERROR
	(
		cdResultadoError    INT IDENTITY(1,1) PRIMARY KEY,
		cdLote              INT NOT NULL,
		cdArchivoPagina     INT NOT NULL,
		dsMotivoError       NVARCHAR(500) NOT NULL,
		dsRespuestaCruda    NVARCHAR(MAX) NULL,
		feAlta              DATETIME NOT NULL DEFAULT GETDATE()
	);
END
GO
