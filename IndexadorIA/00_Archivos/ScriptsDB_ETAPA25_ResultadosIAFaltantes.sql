-- ETAPA 25: Alta de registros faltantes en TD_001_RESULTADO_IA
-- Paginas de TD_ARCHIVOS_PAGINAS sin resultado de IA impiden finalizar el archivo madre en Control Final
-- (ObtenerPaginas hace JOIN con TD_001_RESULTADO_IA). Este script crea un registro vacio para cada una,
-- para que el usuario complete los datos desde la pantalla Control Final.
--
-- USO: ejecutar primero con @Confirmar = 0 (solo informa y revierte). Revisar los conteos y luego
--      ejecutar con @Confirmar = 1. Hacer backup antes.
--
-- cdLote (NOT NULL, FK a TD_LOTE) se obtiene de TD_LOTE_ARCHIVOS; si la pagina no esta, se toma el lote de
-- otra pagina del mismo original que si tenga resultado. Las que no se puedan resolver se informan y se omiten.

SET NOCOUNT ON;

DECLARE @Confirmar BIT = 0;               -- 0 = simulacion (ROLLBACK), 1 = aplica (COMMIT)
DECLARE @SoloLotesFinales BIT = 1;        -- 1 = solo originales incluidos en lotes finales (TD_LOTE_FINAL_ARCHIVOS)
DECLARE @CdUsuario INT = 1;               -- usuario de alta
DECLARE @CdEstadoControl INT = 1;         -- estado CONTROL inicial (1 = pendiente). Verificar en TD_ESTADOS

IF OBJECT_ID('tempdb..#Faltantes') IS NOT NULL DROP TABLE #Faltantes;

SELECT p.cdArchivoPagina,
	   p.cdArchivoOriginal,
	   COALESCE(la.cdLote,
				(SELECT TOP 1 r2.cdLote
				 FROM TD_ARCHIVOS_PAGINAS p2
				 JOIN TD_001_RESULTADO_IA r2 ON r2.cdArchivoPagina = p2.cdArchivoPagina
				 WHERE p2.cdArchivoOriginal = p.cdArchivoOriginal
				 ORDER BY r2.cdResultado DESC)) AS cdLote
INTO #Faltantes
FROM TD_ARCHIVOS_PAGINAS p
LEFT JOIN TD_001_RESULTADO_IA r ON r.cdArchivoPagina = p.cdArchivoPagina
LEFT JOIN TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = p.cdArchivoPagina
WHERE r.cdResultado IS NULL
  AND NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_PAGINAS f WHERE f.cdArchivoPagina = p.cdArchivoPagina)
  AND (@SoloLotesFinales = 0
	   OR EXISTS (SELECT 1 FROM TD_LOTE_FINAL_ARCHIVOS lfa WHERE lfa.cdArchivoOriginal = p.cdArchivoOriginal));

SELECT COUNT(*) AS PaginasSinResultado,
	   SUM(CASE WHEN cdLote IS NULL THEN 1 ELSE 0 END) AS SinLoteResoluble
FROM #Faltantes;

BEGIN TRY
	BEGIN TRAN;

	INSERT INTO TD_001_RESULTADO_IA (cdLote, cdArchivoPagina, cdEstadoControl, feAlta, cdUsuarioAlta)
	SELECT f.cdLote, f.cdArchivoPagina, @CdEstadoControl, GETDATE(), @CdUsuario
	FROM #Faltantes f
	WHERE f.cdLote IS NOT NULL
	  AND NOT EXISTS (SELECT 1 FROM TD_001_RESULTADO_IA r WHERE r.cdArchivoPagina = f.cdArchivoPagina);

	PRINT 'Registros insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

	IF @Confirmar = 1
	BEGIN
		COMMIT TRAN;
		PRINT 'Cambios aplicados (COMMIT).';
	END
	ELSE
	BEGIN
		ROLLBACK TRAN;
		PRINT 'SIMULACION: cambios revertidos (ROLLBACK). Usar @Confirmar = 1 para aplicar.';
	END
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN;
	PRINT 'Error: ' + ERROR_MESSAGE();
END CATCH;

-- Paginas que no se pudieron resolver (sin lote)
SELECT cdArchivoPagina, cdArchivoOriginal FROM #Faltantes WHERE cdLote IS NULL;

DROP TABLE #Faltantes;
GO
