-- Script de corrección: Usar cdEstado en lugar de idEstado para TD_ARCHIVOS_ORIGINAL
-- Parte del Etapa 2

USE IndexadorIA;
GO

-- 1. Eliminar FK existente
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ARCHIVOS_ORIGINAL_ESTADO')
BEGIN
	ALTER TABLE TD_ARCHIVOS_ORIGINAL DROP CONSTRAINT FK_ARCHIVOS_ORIGINAL_ESTADO;
	PRINT 'FK_ARCHIVOS_ORIGINAL_ESTADO eliminada';
END
GO

-- 2. Crear índice único en TD_ESTADOS para (dsProceso, cdEstado) si no existe
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_ESTADOS_PROCESO_CODIGO')
BEGIN
	ALTER TABLE TD_ESTADOS 
	ADD CONSTRAINT UQ_ESTADOS_PROCESO_CODIGO UNIQUE (dsProceso, cdEstado);
	PRINT 'Índice único UQ_ESTADOS_PROCESO_CODIGO creado';
END
GO

-- 3. Agregar columna dsProceso a TD_ARCHIVOS_ORIGINAL si no existe
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
			   WHERE TABLE_NAME = 'TD_ARCHIVOS_ORIGINAL' AND COLUMN_NAME = 'dsProceso')
BEGIN
	ALTER TABLE TD_ARCHIVOS_ORIGINAL 
	ADD dsProceso VARCHAR(50) NOT NULL DEFAULT 'ARCHIVO_ORIGINAL';
	PRINT 'Columna dsProceso agregada a TD_ARCHIVOS_ORIGINAL';
END
GO

-- 4. Crear nueva FK usando (dsProceso, cdEstadoArchivo) -> (dsProceso, cdEstado)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ARCHIVOS_ORIGINAL_ESTADO')
BEGIN
	ALTER TABLE TD_ARCHIVOS_ORIGINAL 
	ADD CONSTRAINT FK_ARCHIVOS_ORIGINAL_ESTADO 
	FOREIGN KEY (dsProceso, cdEstadoArchivo) 
	REFERENCES TD_ESTADOS(dsProceso, cdEstado);
	PRINT 'Nueva FK_ARCHIVOS_ORIGINAL_ESTADO creada usando cdEstado';
END
GO

PRINT 'Corrección completada exitosamente';
GO
