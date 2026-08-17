-- ============================================================================
-- ETAPA 11: Auditoria de correcciones manuales y nuevo estado de lote
-- "Pendiente de Finalizar"
-- ============================================================================

-- 1) Tabla TD_CORRECIONES: registra cada campo modificado manualmente
--    al controlar un resultado de IA (para fines estadisticos)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_CORRECIONES')
BEGIN
	CREATE TABLE TD_CORRECIONES
	(
		cdCorreccion       INT IDENTITY(1,1) NOT NULL,
		cdResultado        INT NOT NULL,
		dsCampo            VARCHAR(50) NOT NULL,
		dsValorAnterior    NVARCHAR(500) NULL,
		dsValorNuevo       NVARCHAR(500) NULL,
		feControl          DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioControl   INT NOT NULL,

		CONSTRAINT PK_TD_CORRECIONES PRIMARY KEY (cdCorreccion)
	);
END
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_CORRECIONES_RESULTADO_IA'
)
BEGIN
	ALTER TABLE TD_CORRECIONES
	ADD CONSTRAINT FK_TD_CORRECIONES_RESULTADO_IA
		FOREIGN KEY (cdResultado) REFERENCES TD_001_RESULTADO_IA(cdResultado);
END
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_CORRECIONES_USUARIO'
)
BEGIN
	ALTER TABLE TD_CORRECIONES
	ADD CONSTRAINT FK_TD_CORRECIONES_USUARIO
		FOREIGN KEY (cdUsuarioControl) REFERENCES TD_USUARIOS(cdUsuario);
END
GO

-- 2) Nuevo estado de lote: "Pendiente de Finalizar" (dsProceso = 'LOTE')
IF NOT EXISTS (
	SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 7
)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 7, 'Pendiente de Finalizar');
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTE' ORDER BY cdEstado;
GO
