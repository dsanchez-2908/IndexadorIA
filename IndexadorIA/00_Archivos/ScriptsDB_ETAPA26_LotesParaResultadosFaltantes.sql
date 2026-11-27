-- ETAPA 26: Resultados IA faltantes para paginas que no pertenecen a lotes finales (complemento de ETAPA 25)
-- Para cada pagina de TD_ARCHIVOS_PAGINAS sin registro en TD_001_RESULTADO_IA:
--   1) Si ya esta en un lote (TD_LOTE_ARCHIVOS) o un hermano del mismo original tiene resultado, usa ese lote.
--   2) Si no, la agrupa por archivo original en LOTES NUEVOS (TD_LOTE) con cdEstadoLote = 10 (Pendiente de Enviar),
--      las agrega a TD_LOTE_ARCHIVOS y crea el resultado IA vacio, para completarlo desde Control Final.
-- Los lotes nuevos se llaman LOTE_HUERFANOS_0001, 0002, ... (continua la numeracion si se vuelve a ejecutar).
-- Un archivo original nunca se parte entre lotes nuevos.
--
-- USO: ejecutar con @Confirmar = 0 (simula y revierte), revisar los conteos, luego @Confirmar = 1. Hacer backup antes.

SET NOCOUNT ON;

DECLARE @Confirmar BIT = 0;               -- 0 = simulacion (ROLLBACK), 1 = aplica (COMMIT)
DECLARE @CdUsuario INT = 1;               -- usuario de alta (debe existir en TD_USUARIOS)
DECLARE @CdEstadoControl INT = 1;         -- estado CONTROL inicial (1 = pendiente)
DECLARE @CdEstadoLote INT = 10;           -- Pendiente de Enviar
DECLARE @OriginalesPorLote INT = 200;     -- archivos originales por lote nuevo
DECLARE @Prefijo VARCHAR(20) = 'LOTE_HUERFANOS_';

IF OBJECT_ID('tempdb..#P') IS NOT NULL DROP TABLE #P;
IF OBJECT_ID('tempdb..#Grupos') IS NOT NULL DROP TABLE #Grupos;

SELECT p.cdArchivoPagina,
	   p.cdArchivoOriginal,
	   COALESCE(la.cdLote,
				(SELECT TOP 1 r2.cdLote
				 FROM TD_ARCHIVOS_PAGINAS p2
				 JOIN TD_001_RESULTADO_IA r2 ON r2.cdArchivoPagina = p2.cdArchivoPagina
				 WHERE p2.cdArchivoOriginal = p.cdArchivoOriginal
				 ORDER BY r2.cdResultado DESC),
				(SELECT TOP 1 la2.cdLote
				 FROM TD_ARCHIVOS_PAGINAS p3
				 JOIN TD_LOTE_ARCHIVOS la2 ON la2.cdArchivoPagina = p3.cdArchivoPagina
				 WHERE p3.cdArchivoOriginal = p.cdArchivoOriginal
				 ORDER BY la2.id DESC)) AS cdLote,
	   CAST(NULL AS INT) AS nuGrupo,
	   CAST(CASE WHEN la.cdLote IS NULL THEN 1 ELSE 0 END AS BIT) AS faltaEnLoteArchivos
INTO #P
FROM TD_ARCHIVOS_PAGINAS p
LEFT JOIN TD_001_RESULTADO_IA r ON r.cdArchivoPagina = p.cdArchivoPagina
LEFT JOIN TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = p.cdArchivoPagina
WHERE r.cdResultado IS NULL
  AND NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_PAGINAS f WHERE f.cdArchivoPagina = p.cdArchivoPagina);

-- Grupo (lote nuevo) por archivo original, solo para las que no tienen lote resoluble
;WITH Orig AS (
	SELECT cdArchivoOriginal, (DENSE_RANK() OVER (ORDER BY cdArchivoOriginal) - 1) / @OriginalesPorLote AS g
	FROM (SELECT DISTINCT cdArchivoOriginal FROM #P WHERE cdLote IS NULL) x
)
UPDATE p SET p.nuGrupo = o.g
FROM #P p JOIN Orig o ON o.cdArchivoOriginal = p.cdArchivoOriginal
WHERE p.cdLote IS NULL;

SELECT COUNT(*) AS PaginasSinResultado,
	   SUM(CASE WHEN cdLote IS NOT NULL THEN 1 ELSE 0 END) AS ConLoteExistente,
	   SUM(CASE WHEN cdLote IS NULL THEN 1 ELSE 0 END) AS ParaLotesNuevos,
	   COUNT(DISTINCT nuGrupo) AS LotesNuevos
FROM #P;

BEGIN TRY
	BEGIN TRAN;

	DECLARE @Base INT = ISNULL((SELECT MAX(CAST(RIGHT(dsNombreLote, 4) AS INT))
								FROM TD_LOTE
								WHERE dsNombreLote LIKE @Prefijo + '[0-9][0-9][0-9][0-9]'), 0);

	SELECT g.nuGrupo,
		   @Prefijo + RIGHT('0000' + CAST(@Base + 1 + ROW_NUMBER() OVER (ORDER BY g.nuGrupo) - 1 AS VARCHAR(10)), 4) AS dsNombreLote,
		   CAST(NULL AS INT) AS cdLote
	INTO #Grupos
	FROM (SELECT DISTINCT nuGrupo FROM #P WHERE nuGrupo IS NOT NULL) g;

	INSERT INTO TD_LOTE (dsNombreLote, nuCantidadArchivos, cdEstadoLote, feAltaLote, cdUsuarioAltaLote)
	SELECT gr.dsNombreLote,
		   (SELECT COUNT(*) FROM #P p WHERE p.nuGrupo = gr.nuGrupo),
		   @CdEstadoLote, GETDATE(), @CdUsuario
	FROM #Grupos gr;
	PRINT 'Lotes creados: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

	UPDATE gr SET gr.cdLote = l.cdLote
	FROM #Grupos gr JOIN TD_LOTE l ON l.dsNombreLote = gr.dsNombreLote;

	UPDATE p SET p.cdLote = gr.cdLote
	FROM #P p JOIN #Grupos gr ON gr.nuGrupo = p.nuGrupo
	WHERE p.cdLote IS NULL;

	-- Relacion lote <-> pagina (indice unico por pagina: solo las que aun no estan en ningun lote)
	INSERT INTO TD_LOTE_ARCHIVOS (cdLote, cdArchivoPagina)
	SELECT p.cdLote, p.cdArchivoPagina
	FROM #P p
	WHERE p.faltaEnLoteArchivos = 1 AND p.nuGrupo IS NOT NULL
	  AND NOT EXISTS (SELECT 1 FROM TD_LOTE_ARCHIVOS x WHERE x.cdArchivoPagina = p.cdArchivoPagina);
	PRINT 'Relaciones TD_LOTE_ARCHIVOS insertadas: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

	INSERT INTO TD_001_RESULTADO_IA (cdLote, cdArchivoPagina, cdEstadoControl, feAlta, cdUsuarioAlta)
	SELECT p.cdLote, p.cdArchivoPagina, @CdEstadoControl, GETDATE(), @CdUsuario
	FROM #P p
	WHERE p.cdLote IS NOT NULL
	  AND NOT EXISTS (SELECT 1 FROM TD_001_RESULTADO_IA r WHERE r.cdArchivoPagina = p.cdArchivoPagina);
	PRINT 'Resultados IA insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

	-- Control: no deben quedar paginas sin resultado
	SELECT COUNT(*) AS PaginasAunSinResultado
	FROM TD_ARCHIVOS_PAGINAS p
	WHERE NOT EXISTS (SELECT 1 FROM TD_001_RESULTADO_IA r WHERE r.cdArchivoPagina = p.cdArchivoPagina)
	  AND NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_PAGINAS f WHERE f.cdArchivoPagina = p.cdArchivoPagina);

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

DROP TABLE #P;
IF OBJECT_ID('tempdb..#Grupos') IS NOT NULL DROP TABLE #Grupos;
GO
