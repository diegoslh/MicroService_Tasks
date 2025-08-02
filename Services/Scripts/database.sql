DROP DATABASE IF EXISTS DB_TareasColaboradores;
GO

CREATE DATABASE DB_TareasColaboradores;
GO

USE DB_TareasColaboradores;
GO

----------------------------------------------------------------------------------------------------
--------------                               DDL                                    ----------------
----------------------------------------------------------------------------------------------------

-- Tabla: Diccionario de Roles
-- Esta tabla almacena los roles de los usuarios del sistema
CREATE TABLE Tc_TblDicRol (
    rol_idRolPk INT IDENTITY(1,1),
    rol_nombreRol NVARCHAR(30) NOT NULL,
    rol_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (rol_idRolPk)
);

-- Tabla: Diccionario de Estados para Tareas
-- Esta tabla almacena los estados posibles de una tarea
CREATE TABLE Tc_TblDicEstadoTarea (
    est_idEstadoPk INT IDENTITY(1,1),
    est_nombre NVARCHAR(30) NOT NULL,
    est_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (est_idEstadoPk)
);

-- Tabla: Colaborador
-- Esta tabla almacena los colaboradores que pueden ser asignados a tareas
CREATE TABLE Tc_TblColaborador (
    col_idColaboradorPk INT IDENTITY(1,1),
    col_nombre NVARCHAR(80) NOT NULL,
    col_email NVARCHAR(70) NOT NULL,
    col_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (col_idColaboradorPk),
);

-- Tabla: Tareas
-- Esta tabla almacena las tareas asignadas a los colaboradores
CREATE TABLE Tc_TblTarea (
    tar_idTareaPk INT IDENTITY(1,1),
    tar_titulo NVARCHAR(100) NOT NULL,
    tar_descripcion TEXT,
    tar_fechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    tar_fechaLimite DATETIME NOT NULL,
    tar_colaboradorFk INT NOT NULL,
    tar_estadoFk INT NOT NULL,
    tar_estado BIT NOT NULL DEFAULT 1,
    
    PRIMARY KEY (tar_idTareaPk),
    FOREIGN KEY (tar_colaboradorFk) REFERENCES Tc_TblColaborador(col_idColaboradorPk),
    FOREIGN KEY (tar_estadoFk) REFERENCES Tc_TblDicEstadoTarea(est_idEstadoPk)
);

-- Tabla: Usuarios del sistema
-- Esta tabla almacena los usuarios que pueden ingresar al sistema
CREATE TABLE Tc_TblUsuario (
    us_idUsuarioPkFK INT NOT NULL,
    us_alias NVARCHAR(10) NOT NULL,
    us_contrasena NVARCHAR(100) NOT NULL,
    us_rolFk INT NOT NULL,
    us_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (us_idUsuarioPkFK),
    FOREIGN KEY (us_idUsuarioPkFK) REFERENCES Tc_TblColaborador(col_idColaboradorPk),
    FOREIGN KEY (us_rolFk) REFERENCES Tc_TblDicRol(rol_idRolPk)
);

----------------------------------------------------------------------------------------------------
--------------                               DML                                    ----------------
----------------------------------------------------------------------------------------------------

-- Insertar datos iniciales en la tabla de estados de tarea
INSERT INTO Tc_TblDicEstadoTarea (est_nombre, est_estado) 
VALUES
    ('Pendiente', 1),
    ('En Progreso', 1),
    ('Completada', 1),
    ('Cancelada', 1);

-- Insertar datos iniciales en la tabla de roles
INSERT INTO Tc_TblDicRol (rol_nombreRol, rol_estado) 
VALUES
    ('Administrador', 1),
    ('Colaborador', 1);

-- Insertar datos iniciales en la tabla de colaboradores
INSERT INTO Tc_TblColaborador (col_nombre, col_email, col_estado) 
VALUES
    ('Sharit Bedoya', 'sharit.bedoya@example.com', 1),
    ('Victoria Mejia', 'victoria.mejia@example.com', 1),
    ('Manuel Narvaez', 'manuel.narvaez@example.com', 1);

-- Insertar datos iniciales en la tabla de usuarios
INSERT INTO Tc_TblUsuario (us_idUsuarioPkFK, us_alias, us_contrasena, us_rolFk, us_estado)
VALUES
    (
        (SELECT TOP 1 col_idColaboradorPk FROM Tc_TblColaborador),
        'admin', 
        '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', -- admin123
        (SELECT rol_idRolPk FROM Tc_TblDicRol WHERE rol_nombreRol = 'Administrador'), 
        1
    );

-- Insertar datos iniciales en la tabla de tareas
INSERT INTO Tc_TblTarea (tar_titulo, tar_descripcion, tar_fechaLimite, tar_colaboradorFk, tar_estadoFk, tar_estado)
VALUES
    (
        'Enviar solicitud de requerimientos papeler�a',
        'Solicitar a los colaboradores la lista de papeler�a necesaria para el pr�ximo mes',
        '2025-08-15',
        (SELECT col_idColaboradorPk FROM Tc_TblColaborador WHERE col_nombre = 'Sharit Bedoya'),
        (SELECT est_idEstadoPk FROM Tc_TblDicEstadoTarea WHERE est_nombre = 'Pendiente'),
        1
    ),
    (
        'Revisar avances del proyecto ERP',
        NULL,
        '2025-08-02',
        (SELECT col_idColaboradorPk FROM Tc_TblColaborador WHERE col_nombre = 'Victoria Mejia'),
        (SELECT est_idEstadoPk FROM Tc_TblDicEstadoTarea WHERE est_nombre = 'En Progreso'),
        1
    ),
    (
        'Finalizar informe de ventas',
        'Elaborar el informe de ventas del segundo trimestre del a�o 2025',
        '2025-07-30',
        (SELECT col_idColaboradorPk FROM Tc_TblColaborador WHERE col_nombre = 'Manuel Narvaez'),
        (SELECT est_idEstadoPk FROM Tc_TblDicEstadoTarea WHERE est_nombre = 'Completada'),
        1
    );
