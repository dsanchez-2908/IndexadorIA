-- ============================================
-- ETAPA 13: Nuevo rol "Data Entry"
-- ============================================
-- Este rol solo debe tener acceso a:
--   Planos de GCBA / Control y Finalización
--   Configuración / Cambiar Clave y Cerrar Sesión
-- ============================================

IF NOT EXISTS (SELECT * FROM TD_ROLES WHERE cdRol = 'DATAENTRY')
BEGIN
	INSERT INTO TD_ROLES (cdRol, dsRol, cdEstado) VALUES ('DATAENTRY', 'Data Entry', 1)
	PRINT 'Rol Data Entry insertado'
END
ELSE
BEGIN
	PRINT 'Rol Data Entry ya existe'
END
GO
