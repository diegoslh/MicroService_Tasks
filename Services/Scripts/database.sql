DROP DATABASE IF EXISTS DB_MicroServiceTasks;
CREATE DATABASE DB_MicroServiceTasks;
USE DB_MicroServiceTasks;

----------------------------------------------------------------------------------------------------
--------------                               DDL                                    ----------------
----------------------------------------------------------------------------------------------------

-- Eliminar si existen
DROP TABLE IF EXISTS Task_TblTarea;
DROP TABLE IF EXISTS Task_TblUsuario;
DROP TABLE IF EXISTS Task_TblDicEstadoTarea;
DROP TABLE IF EXISTS Task_TblDicRol;
DROP TABLE IF EXISTS Task_TblColaborador;


-- Tabla: Diccionario de Roles
-- Esta tabla almacena los roles de los usuarios del sistema
CREATE TABLE Task_TblDicRol (
    rol_idRolPk INT IDENTITY(1,1),
    rol_nombreRol NVARCHAR(30) NOT NULL,
    rol_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (rol_idRolPk)
);

-- Tabla: Diccionario de Estados para Tareas
-- Esta tabla almacena los estados posibles de una tarea
CREATE TABLE Task_TblDicEstadoTarea (
    est_idEstadoPk INT IDENTITY(1,1),
    est_nombre NVARCHAR(30) NOT NULL,
    est_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (est_idEstadoPk)
);

-- Tabla: Colaborador
-- Esta tabla almacena los colaboradores que pueden ser asignados a tareas
CREATE TABLE Task_TblColaborador (
    col_idColaboradorPk INT IDENTITY(1,1),
    col_nombre NVARCHAR(80) NOT NULL,
    col_email NVARCHAR(70) NOT NULL,
    col_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (col_idColaboradorPk),
);

-- Tabla: Tareas
-- Esta tabla almacena las tareas asignadas a los colaboradores
CREATE TABLE Task_TblTarea (
    tar_idTareaPk INT IDENTITY(1,1),
    tar_titulo NVARCHAR(100) NOT NULL,
    tar_descripcion TEXT,
    tar_fechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    tar_fechaLimite DATETIME NOT NULL,
    tar_colaboradorFk INT NOT NULL,
    tar_estadoFk INT NOT NULL,
    tar_estado BIT NOT NULL DEFAULT 1,
    
    PRIMARY KEY (tar_idTareaPk),
    FOREIGN KEY (tar_colaboradorFk) REFERENCES Task_TblColaborador(col_idColaboradorPk),
    FOREIGN KEY (tar_estadoFk) REFERENCES Task_TblDicEstadoTarea(est_idEstadoPk)
);

-- Tabla: Usuarios del sistema
-- Esta tabla almacena los usuarios que pueden ingresar al sistema
CREATE TABLE Task_TblUsuario (
    us_idUsuarioPkFK INT IDENTITY(1,1),
    us_alias NVARCHAR(10) NOT NULL,
    us_contrasena NVARCHAR(100) NOT NULL,
    us_rolFk INT NOT NULL,
    us_estado BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (us_idUsuarioPkFK),
    FOREIGN KEY (us_rolFk) REFERENCES Task_TblDicRol(rol_idRolPk)
);

----------------------------------------------------------------------------------------------------
--------------                               DML                                    ----------------
----------------------------------------------------------------------------------------------------

-- Insertar datos iniciales en la tabla de estados de tarea
INSERT INTO Task_TblDicEstadoTarea (est_nombre, est_estado) 
VALUES
    ('Pendiente', 1),
    ('En Progreso', 1),
    ('Completada', 1),
    ('Cancelada', 1);

-- Insertar datos iniciales en la tabla de roles
INSERT INTO Task_TblDicRol (rol_nombreRol, rol_estado) 
VALUES
    ('Administrador', 1),
    ('Colaborador', 1);

-- Insertar datos iniciales en la tabla de colaboradores
INSERT INTO Task_TblColaborador (col_nombre, col_email, col_estado) 
VALUES
    ('Sharit Bedoya', 'sharit.bedoya@example.com', 1),
    ('Victoria Mejia', 'victoria.mejia@example.com', 1),
    ('Manuel Narvaez', 'manuel.narvaez@example.com', 1);

-- Insertar datos iniciales en la tabla de usuarios
INSERT INTO Task_TblUsuario (us_alias, us_contrasena, us_rolFk, us_estado) 
VALUES
    ('admin', 'admin123', (SELECT rol_idRolPk FROM Task_TblDicRol WHERE rol_nombreRol = 'Administrador'), 1);

-- Insertar datos iniciales en la tabla de tareas
INSERT INTO Task_TblTarea (tar_titulo, tar_descripcion, tar_fechaLimite, tar_colaboradorFk, tar_estadoFk, tar_estado)
VALUES
    (
        'Enviar solicitud de requerimientos papelería',
        'Solicitar a los colaboradores la lista de papelería necesaria para el próximo mes',
        '2025-08-15',
        (SELECT col_idColaboradorPk FROM Task_TblColaborador WHERE col_nombre = 'Sharit Bedoya'),
        (SELECT est_idEstadoPk FROM Task_TblDicEstadoTarea WHERE est_nombre = 'Pendiente'),
        1
    ),
    (
        'Revisar avances del proyecto ERP',
        NULL,
        '2025-08-02',
        (SELECT col_idColaboradorPk FROM Task_TblColaborador WHERE col_nombre = 'Victoria Mejia'),
        (SELECT est_idEstadoPk FROM Task_TblDicEstadoTarea WHERE est_nombre = 'En Progreso'),
        1
    ),
    (
        'Finalizar informe de ventas',
        'Elaborar el informe de ventas del segundo trimestre del año 2025',
        '2025-07-30',
        (SELECT col_idColaboradorPk FROM Task_TblColaborador WHERE col_nombre = 'Manuel Narvaez'),
        (SELECT est_idEstadoPk FROM Task_TblDicEstadoTarea WHERE est_nombre = 'Completada'),
        1
    );
