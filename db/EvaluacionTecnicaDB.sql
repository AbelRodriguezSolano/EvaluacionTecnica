-- ============================================
-- Base de Datos: EvaluacionTecnicaDB
-- Sistema de Gestión de Usuarios y Roles
-- ============================================

USE master;
GO

-- Crear base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EvaluacionTecnicaDB')
BEGIN
    CREATE DATABASE EvaluacionTecnicaDB;
END
GO

USE EvaluacionTecnicaDB;
GO

-- Tabla: Roles
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Usuario_Transaccion NVARCHAR(100) NOT NULL,
    Fecha_Transaccion DATETIME2 NOT NULL
);

-- Tabla: Users
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleId INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Cedula NVARCHAR(50) NOT NULL,
    Usuario_Nombre NVARCHAR(100) NOT NULL,
    Contraseña NVARCHAR(255) NOT NULL,
    Fecha_Nacimiento DATETIME2 NOT NULL,
    Usuario_Transaccion NVARCHAR(100) NOT NULL,
    Fecha_Transaccion DATETIME2 NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

-- Índices
CREATE UNIQUE INDEX IX_Roles_Nombre ON Roles(Nombre);
CREATE UNIQUE INDEX IX_Users_Cedula ON Users(Cedula);
CREATE UNIQUE INDEX IX_Users_Usuario_Nombre ON Users(Usuario_Nombre);
CREATE INDEX IX_Users_RoleId ON Users(RoleId);
GO

-- Datos iniciales: Roles
INSERT INTO Roles (Nombre, Usuario_Transaccion, Fecha_Transaccion)
VALUES
    ('ADMIN', 'SYSTEM', '2024-01-01'),
    ('DESARROLLADOR', 'SYSTEM', '2024-01-01');

-- Datos iniciales: Usuarios (contraseñas hasheadas con BCrypt)
-- ADMIN / admin123
-- DESARROLLADOR / aplicante123
INSERT INTO Users (RoleId, Nombre, Apellido, Cedula, Usuario_Nombre, Contraseña, Fecha_Nacimiento, Usuario_Transaccion, Fecha_Transaccion)
VALUES
    (1, 'User', 'Admin', '25322522135', 'ADMIN', '$2a$11$586UeUGFE0ZLYo2uXhpFe.i.0kVwO7L38KRjhLj3XkwpcaGvQwjRy', '1990-01-01', 'SYSTEM', '2024-01-01'),
    (2, 'User', 'Developer', '0000000000', 'DESARROLLADOR', '$2a$11$lywmS6Q.40p/cZf3RCyiEeRy6JnJv7I/ZA26u1J8GpEyheLPkeseO', '2000-02-25', 'SYSTEM', '2024-01-01');
GO

PRINT 'Base de datos creada exitosamente con datos iniciales.';
GO
