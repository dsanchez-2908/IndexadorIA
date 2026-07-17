-- ============================================
-- Script de Base de Datos - ETAPA 1 SOLAMENTE
-- IndexadorIA - Sistema de Autenticación y Usuarios
-- ============================================

USE IndexadorIA
GO

-- ============================================
-- Tabla: TD_ESTADOS
-- Descripción: Almacena todos los estados utilizados en la solución
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_ESTADOS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_ESTADOS(
		idEstado INT IDENTITY(1,1) PRIMARY KEY,
		dsProceso VARCHAR(50) NOT NULL,
		cdEstado INT NOT NULL,
		dsEstado VARCHAR(50) NOT NULL
	)
	PRINT 'Tabla TD_ESTADOS creada correctamente'
END
ELSE
BEGIN
	PRINT 'Tabla TD_ESTADOS ya existe'
END
GO

-- ============================================
-- Tabla: TD_ROLES
-- Descripción: Almacena los roles del sistema
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_ROLES]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_ROLES(
		idRol INT IDENTITY(1,1) PRIMARY KEY,
		cdRol VARCHAR(50) NOT NULL UNIQUE,
		dsRol VARCHAR(100) NOT NULL,
		cdEstado INT NOT NULL
	)
	PRINT 'Tabla TD_ROLES creada correctamente'
END
ELSE
BEGIN
	PRINT 'Tabla TD_ROLES ya existe'
END
GO

-- ============================================
-- Tabla: TD_USUARIOS
-- Descripción: Almacena los usuarios del sistema
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_USUARIOS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_USUARIOS(
		cdUsuario INT IDENTITY(1,1) PRIMARY KEY,
		dsUsuario VARCHAR(50) NOT NULL UNIQUE,
		dsClave VARCHAR(255) NOT NULL,
		dsNombreCompleto VARCHAR(100) NOT NULL,
		snClaveTemporal BIT NOT NULL DEFAULT 1,
		snPrimerIngreso BIT NOT NULL DEFAULT 1,
		idRol INT NOT NULL,
		cdEstado INT NOT NULL,
		feAlta DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioAlta INT NULL,
		CONSTRAINT FK_USUARIOS_ROLES FOREIGN KEY (idRol) REFERENCES TD_ROLES(idRol)
	)
	PRINT 'Tabla TD_USUARIOS creada correctamente'
END
ELSE
BEGIN
	-- Si la tabla ya existe, verificar si tiene la columna idRol
	IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_USUARIOS]') AND name = 'idRol')
	BEGIN
		-- Agregar columna idRol como nullable primero
		ALTER TABLE TD_USUARIOS ADD idRol INT NULL
		PRINT 'Columna idRol agregada a TD_USUARIOS'

		-- Agregar la FK después de poblar los datos
	END
	ELSE
	BEGIN
		PRINT 'Tabla TD_USUARIOS ya tiene columna idRol'
	END
END
GO

-- ============================================
-- Insertar Estados para USUARIOS
-- ============================================
IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'USUARIOS' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('USUARIOS', 1, 'Activo')
	PRINT 'Estado USUARIOS - Activo insertado'
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'USUARIOS' AND cdEstado = 0)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('USUARIOS', 0, 'Inactivo')
	PRINT 'Estado USUARIOS - Inactivo insertado'
END
GO

-- ============================================
-- Insertar Estados para ROLES
-- ============================================
IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'ROLES' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('ROLES', 1, 'Activo')
	PRINT 'Estado ROLES - Activo insertado'
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'ROLES' AND cdEstado = 0)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('ROLES', 0, 'Inactivo')
	PRINT 'Estado ROLES - Inactivo insertado'
END
GO

-- ============================================
-- Insertar Rol Administrador
-- ============================================
IF NOT EXISTS (SELECT * FROM TD_ROLES WHERE cdRol = 'ADMIN')
BEGIN
	INSERT INTO TD_ROLES (cdRol, dsRol, cdEstado) VALUES ('ADMIN', 'Administrador', 1)
	PRINT 'Rol Administrador insertado'
END
ELSE
BEGIN
	PRINT 'Rol Administrador ya existe'
END
GO

-- ============================================
-- Actualizar usuarios existentes con rol por defecto
-- ============================================
DECLARE @idRolAdmin INT
SELECT @idRolAdmin = idRol FROM TD_ROLES WHERE cdRol = 'ADMIN'

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_USUARIOS]') AND name = 'idRol')
BEGIN
	UPDATE TD_USUARIOS SET idRol = @idRolAdmin WHERE idRol IS NULL
	PRINT 'Usuarios existentes actualizados con rol Administrador'

	-- Agregar FK si no existe
	IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_USUARIOS_ROLES')
	BEGIN
		ALTER TABLE TD_USUARIOS ALTER COLUMN idRol INT NOT NULL
		ALTER TABLE TD_USUARIOS ADD CONSTRAINT FK_USUARIOS_ROLES FOREIGN KEY (idRol) REFERENCES TD_ROLES(idRol)
		PRINT 'Foreign Key FK_USUARIOS_ROLES agregada'
	END
END
GO

-- ============================================
-- Insertar Usuario Administrador Inicial
-- Usuario: admin
-- Clave: 123 (SHA256: a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3)
-- ============================================
IF NOT EXISTS (SELECT * FROM TD_USUARIOS WHERE dsUsuario = 'admin')
BEGIN
	DECLARE @idRolAdmin INT
	SELECT @idRolAdmin = idRol FROM TD_ROLES WHERE cdRol = 'ADMIN'

	INSERT INTO TD_USUARIOS (dsUsuario, dsClave, dsNombreCompleto, snClaveTemporal, snPrimerIngreso, idRol, cdEstado, feAlta, cdUsuarioAlta)
	VALUES ('admin', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Administrador del Sistema', 0, 0, @idRolAdmin, 1, GETDATE(), NULL)
	PRINT 'Usuario admin creado correctamente'
END
ELSE
BEGIN
	PRINT 'Usuario admin ya existe'
END
GO

-- ============================================
-- Stored Procedure: SP_VALIDAR_LOGIN
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_VALIDAR_LOGIN]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[SP_VALIDAR_LOGIN]
	PRINT 'SP_VALIDAR_LOGIN eliminado para recrear'
END
GO

CREATE PROCEDURE SP_VALIDAR_LOGIN
	@dsUsuario VARCHAR(50),
	@dsClave VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		u.cdUsuario,
		u.dsUsuario,
		u.dsNombreCompleto,
		u.snClaveTemporal,
		u.snPrimerIngreso,
		u.idRol,
		r.cdRol,
		r.dsRol,
		u.cdEstado,
		e.dsEstado
	FROM TD_USUARIOS u
	INNER JOIN TD_ROLES r ON u.idRol = r.idRol
	INNER JOIN TD_ESTADOS e ON e.dsProceso = 'USUARIOS' AND e.cdEstado = u.cdEstado
	WHERE u.dsUsuario = @dsUsuario 
		AND u.dsClave = @dsClave
		AND u.cdEstado = 1
END
GO

PRINT '============================================'
PRINT 'Script de Etapa 1 ejecutado correctamente'
PRINT '============================================'
PRINT ''
PRINT 'Tablas creadas:'
PRINT '  - TD_ESTADOS'
PRINT '  - TD_ROLES'
PRINT '  - TD_USUARIOS'
PRINT ''
PRINT 'Stored Procedures creados:'
PRINT '  - SP_VALIDAR_LOGIN'
PRINT ''
PRINT 'Datos iniciales:'
PRINT '  - 4 Estados (2 USUARIOS, 2 ROLES)'
PRINT '  - 1 Rol: Administrador'
PRINT '  - 1 Usuario: admin (clave: 123)'
PRINT ''
PRINT 'Puede iniciar la aplicación y hacer login con:'
PRINT '  Usuario: admin'
PRINT '  Clave: 123'
PRINT '============================================'
GO

-- Verificación final
SELECT 'ESTADOS' as Tabla, COUNT(*) as Registros FROM TD_ESTADOS
UNION ALL
SELECT 'ROLES' as Tabla, COUNT(*) as Registros FROM TD_ROLES
UNION ALL
SELECT 'USUARIOS' as Tabla, COUNT(*) as Registros FROM TD_USUARIOS
GO
