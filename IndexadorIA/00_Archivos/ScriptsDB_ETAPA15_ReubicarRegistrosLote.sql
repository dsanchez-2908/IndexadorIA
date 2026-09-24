-- ETAPA 15: Reubicación de registros pendientes (Ilegibles/Datos Ilegibles/Casos Especiales/Parcelas Múltiples)
-- de los lotes finalizados (cdEstadoLote=11) del día 2026-09-23 hacia un nuevo lote
-- en estado 7 (Pendiente Asignar Auditoria).
--
-- Reglas de negocio (ver FrmPreparacionLotes.btnCrearLotes_Click / LoteBL.CrearLotes / LoteDAL.CrearLote):
--   * El nombre de lote se arma como LOTE_{secuencia:D8} usando SEQ_LOTE.
--   * TD_LOTE.feAltaLote se completa con GETDATE() y cdUsuarioAltaLote con el usuario que ejecuta el proceso.
--   * TD_LOTE_ARCHIVOS relaciona cdLote <-> cdArchivoPagina.
--
-- Este script:
--   1. Crea un nuevo lote (cdEstadoLote = 7) con la cantidad de registros a reubicar.
--   2. Actualiza TD_LOTE_ARCHIVOS para mover cdArchivoPagina al nuevo lote.
--   3. Resta la cantidad reubicada de TD_LOTE.nuCantidadArchivos en los lotes originales.
--   4. Actualiza TD_001_RESULTADO_IA.cdLote de los registros reubicados al nuevo lote.
--
-- IMPORTANTE: Ajustar @feEnvioFiltro, @cdUsuarioAltaLote y @dsEstadosControlFiltro según corresponda
-- antes de ejecutar en producción. Se recomienda ejecutar primero el SELECT de verificación (bloque 0)
-- y correr todo dentro de una transacción para poder hacer ROLLBACK si algo no coincide con lo esperado.

SET NOCOUNT ON;

DECLARE @feEnvioFiltro DATE = '2026-09-23';
DECLARE @cdEstadoLoteOrigen INT = 11;
DECLARE @cdEstadoLoteNuevo INT = 7; -- Pendiente Asignar Auditoria
DECLARE @cdUsuarioAltaLote INT = 1; -- TODO: reemplazar por el cdUsuario correspondiente
DECLARE @nombreLoteNuevo NVARCHAR(100);
DECLARE @secuencia INT;
DECLARE @cdLoteNuevo INT;
DECLARE @nuCantidadArchivosNuevoLote INT;

-- Tabla temporal con los cdArchivoPagina a reubicar (estados: 3=Ilegible, 4=Datos Ilegibles, 5=Casos Especiales, 6=Parcelas Multiples)
IF OBJECT_ID('tempdb..#ArchivosAReubicar') IS NOT NULL DROP TABLE #ArchivosAReubicar;

SELECT r.cdArchivoPagina, r.cdLote AS cdLoteOriginal
INTO #ArchivosAReubicar
FROM TD_001_RESULTADO_IA r
WHERE r.cdLote IN (
		SELECT cdLote FROM TD_LOTE
		WHERE cdEstadoLote = @cdEstadoLoteOrigen
		  AND CAST(feEnvio AS DATE) = @feEnvioFiltro
	  )
  AND r.cdEstadoControl IN (3, 4, 5, 6);

SELECT @nuCantidadArchivosNuevoLote = COUNT(*) FROM #ArchivosAReubicar;

-- ===================== Bloque 0: Verificación previa =====================
PRINT 'Cantidad total de registros a reubicar: ' + CAST(@nuCantidadArchivosNuevoLote AS VARCHAR(10));

SELECT cdLoteOriginal AS cdLote, COUNT(*) AS Cantidad
FROM #ArchivosAReubicar
GROUP BY cdLoteOriginal
ORDER BY 1;

IF @nuCantidadArchivosNuevoLote = 0
BEGIN
	PRINT 'No hay registros para reubicar. Proceso finalizado sin cambios.';
	RETURN;
END

BEGIN TRANSACTION;

BEGIN TRY
	-- ===================== 1. Crear el nuevo lote =====================
	SET @secuencia = NEXT VALUE FOR SEQ_LOTE;
	SET @nombreLoteNuevo = 'LOTE_' + RIGHT('00000000' + CAST(@secuencia AS VARCHAR(8)), 8);

	INSERT INTO TD_LOTE (dsNombreLote, nuCantidadArchivos, cdEstadoLote, feAltaLote, cdUsuarioAltaLote)
	OUTPUT INSERTED.cdLote
	VALUES (@nombreLoteNuevo, @nuCantidadArchivosNuevoLote, @cdEstadoLoteNuevo, GETDATE(), @cdUsuarioAltaLote);

	SELECT @cdLoteNuevo = SCOPE_IDENTITY();

	PRINT 'Nuevo lote creado: ' + @nombreLoteNuevo + ' (cdLote = ' + CAST(@cdLoteNuevo AS VARCHAR(10)) + ')';

	-- ===================== 2. Reasignar TD_LOTE_ARCHIVOS al nuevo lote =====================
	UPDATE la
	SET la.cdLote = @cdLoteNuevo
	FROM TD_LOTE_ARCHIVOS la
	INNER JOIN #ArchivosAReubicar ar ON ar.cdArchivoPagina = la.cdArchivoPagina;

	-- ===================== 3. Restar la cantidad reubicada de los lotes originales =====================
	UPDATE l
	SET l.nuCantidadArchivos = l.nuCantidadArchivos - resumen.Cantidad
	FROM TD_LOTE l
	INNER JOIN (
		SELECT cdLoteOriginal, COUNT(*) AS Cantidad
		FROM #ArchivosAReubicar
		GROUP BY cdLoteOriginal
	) resumen ON resumen.cdLoteOriginal = l.cdLote;

	-- ===================== 4. Actualizar TD_001_RESULTADO_IA.cdLote al nuevo lote =====================
	UPDATE r
	SET r.cdLote = @cdLoteNuevo
	FROM TD_001_RESULTADO_IA r
	INNER JOIN #ArchivosAReubicar ar ON ar.cdArchivoPagina = r.cdArchivoPagina;

	COMMIT TRANSACTION;

	PRINT 'Reubicación finalizada correctamente. cdLote nuevo: ' + CAST(@cdLoteNuevo AS VARCHAR(10));
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	PRINT 'Error durante la reubicación. Se realizó ROLLBACK.';
	THROW;
END CATCH

IF OBJECT_ID('tempdb..#ArchivosAReubicar') IS NOT NULL DROP TABLE #ArchivosAReubicar;
GO
