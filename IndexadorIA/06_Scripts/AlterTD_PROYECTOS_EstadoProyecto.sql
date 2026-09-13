-- Agrega a TD_PROYECTOS las columnas necesarias para el reporte "Estado Proyecto Planos"
ALTER TABLE TD_PROYECTOS ADD nuTotalArchivos INT NULL;
ALTER TABLE TD_PROYECTOS ADD nuTotalPlanos INT NULL;
GO

UPDATE TD_PROYECTOS SET nuTotalArchivos = 55000, nuTotalPlanos = 1000000 WHERE cdProyecto = 1;
GO
