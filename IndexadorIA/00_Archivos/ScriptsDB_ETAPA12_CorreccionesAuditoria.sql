-- ============================================================================
-- ETAPA 12: Registro de correcciones realizadas durante la auditoria
--    en TD_CORRECIONES (feAuditoria / cdUsuarioAuditoria)
-- ============================================================================

-- 1) Agregar columnas feAuditoria y cdUsuarioAuditoria a TD_CORRECIONES
IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('TD_CORRECIONES') AND name = 'feAuditoria'
)
BEGIN
	ALTER TABLE TD_CORRECIONES ADD feAuditoria DATETIME NULL;
END
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('TD_CORRECIONES') AND name = 'cdUsuarioAuditoria'
)
BEGIN
	ALTER TABLE TD_CORRECIONES ADD cdUsuarioAuditoria INT NULL;
END
GO

-- 2) feControl y cdUsuarioControl deben permitir NULL, ya que una correccion
--    puede provenir unicamente de la auditoria (sin datos de control)
IF EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('TD_CORRECIONES') AND name = 'feControl' AND is_nullable = 0
)
BEGIN
	ALTER TABLE TD_CORRECIONES ALTER COLUMN feControl DATETIME NULL;
END
GO

IF EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('TD_CORRECIONES') AND name = 'cdUsuarioControl' AND is_nullable = 0
)
BEGIN
	ALTER TABLE TD_CORRECIONES ALTER COLUMN cdUsuarioControl INT NULL;
END
GO

-- 3) FK opcional para cdUsuarioAuditoria -> TD_USUARIOS
IF NOT EXISTS (
	SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_CORRECIONES_USUARIO_AUDITORIA'
)
BEGIN
	ALTER TABLE TD_CORRECIONES
	ADD CONSTRAINT FK_TD_CORRECIONES_USUARIO_AUDITORIA
		FOREIGN KEY (cdUsuarioAuditoria) REFERENCES TD_USUARIOS(cdUsuario);
END
GO
