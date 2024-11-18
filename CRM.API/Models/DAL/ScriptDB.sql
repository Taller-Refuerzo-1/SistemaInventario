-- Crear la base de datos CRMDB
CREATE DATABASE Inventario
GO

-- Utilizar la base de datos CRMDB
USE Inventario
GO

-- Crear la tabla Customers (anteriormente Clients)
CREATE TABLE Customers
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Address VARCHAR(255)
)
GO

CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    Phone VARCHAR(50) NOT NULL,
    Password VARCHAR(50) NOT NULL,   
)
GO

CREATE TABLE Providers
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Empresa VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    Phone VARCHAR(50) NOT NULL,
)