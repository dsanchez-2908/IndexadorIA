-- Elimina resultados duplicados en TD_001_RESULTADO_IA, conservando por cada
-- cdArchivoPagina el registro con mayor cdResultado (el más reciente).
-- Primero se eliminan los tokens de consumo asociados a los duplicados a borrar.
;WITH Duplicados AS (
	SELECT cdResultado,
		   ROW_NUMBER() OVER (PARTITION BY cdArchivoPagina ORDER BY cdResultado DESC) AS rn
	FROM TD_001_RESULTADO_IA
)
DELETE FROM TD_TOKEN
WHERE cdResultado IN (SELECT cdResultado FROM Duplicados WHERE rn > 1);
GO

;WITH Duplicados AS (
	SELECT cdResultado,
		   ROW_NUMBER() OVER (PARTITION BY cdArchivoPagina ORDER BY cdResultado DESC) AS rn
	FROM TD_001_RESULTADO_IA
)
DELETE FROM TD_001_RESULTADO_IA
WHERE cdResultado IN (SELECT cdResultado FROM Duplicados WHERE rn > 1);
GO

