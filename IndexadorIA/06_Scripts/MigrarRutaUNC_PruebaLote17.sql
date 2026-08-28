-- Script de PRUEBA: migra las rutas de un solo lote de letra de unidad mapeada (P:\)
-- a ruta UNC (\\FS-CLUSTER01\Recurso3\), para validar que la API (corriendo bajo
-- NT AUTHORITY\NETWORK SERVICE en APPS-01) pueda resolver los archivos JPG/PDF.
--
-- IMPORTANTE: antes de correr en otros lotes, verificar que la cuenta de equipo
-- APPS-01$ (o la identidad real del Application Pool) tenga permisos de lectura
-- sobre el recurso compartido \\FS-CLUSTER01\Recurso3.

DECLARE @cdLoteAProbar INT = 17;

-- 1) Ver el estado actual (antes del cambio), para poder comparar/revertir si hace falta
SELECT ap.cdArchivoPagina, ap.dsRutaCompleta
FROM TD_ARCHIVOS_PAGINAS ap
INNER JOIN TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = ap.cdArchivoPagina
WHERE la.cdLote = @cdLoteAProbar;

-- 2) Actualizar solo las filas del lote indicado, reemplazando el prefijo P:\ por la UNC
UPDATE ap
SET ap.dsRutaCompleta = REPLACE(ap.dsRutaCompleta, 'P:\', '\\FS-CLUSTER01\Recurso3\')
FROM TD_ARCHIVOS_PAGINAS ap
INNER JOIN TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = ap.cdArchivoPagina
WHERE la.cdLote = @cdLoteAProbar
  AND ap.dsRutaCompleta LIKE 'P:\%';

-- 3) Verificar el resultado
SELECT ap.cdArchivoPagina, ap.dsRutaCompleta
FROM TD_ARCHIVOS_PAGINAS ap
INNER JOIN TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = ap.cdArchivoPagina
WHERE la.cdLote = @cdLoteAProbar;

-- ROLLBACK manual si hiciera falta revertir la prueba:
-- UPDATE ap
-- SET ap.dsRutaCompleta = REPLACE(ap.dsRutaCompleta, '\\FS-CLUSTER01\Recurso3\', 'P:\')
-- FROM TD_ARCHIVOS_PAGINAS ap
-- INNER JOIN TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = ap.cdArchivoPagina
-- WHERE la.cdLote = @cdLoteAProbar
--   AND ap.dsRutaCompleta LIKE '\\FS-CLUSTER01\Recurso3\%';
