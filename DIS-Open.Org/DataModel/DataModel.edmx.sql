
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/18/2014 16:14:48
-- Generated from EDMX file: E:\DIS2.0\SourceCode\DIS2.0\DISNinja\src\Cloud\DataModel\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DISConfigurationCloud];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ConfigurationCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Configurations] DROP CONSTRAINT [FK_ConfigurationCustomer];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Configurations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Configurations];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] nvarchar(36)  NOT NULL,
    [Name] nvarchar(300)  NOT NULL,
    [ReferenceId] nvarchar(max)  NULL
);
GO

-- Creating table 'Configurations'
CREATE TABLE [dbo].[Configurations] (
    [Id] nvarchar(36)  NOT NULL,
    [CustomerId] nvarchar(36)  NOT NULL,
    [TypeId] int  NOT NULL,
    [DbConnectionString] nvarchar(450)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Configurations'
ALTER TABLE [dbo].[Configurations]
ADD CONSTRAINT [PK_Configurations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CustomerId] in table 'Configurations'
ALTER TABLE [dbo].[Configurations]
ADD CONSTRAINT [FK_ConfigurationCustomer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ConfigurationCustomer'
CREATE INDEX [IX_FK_ConfigurationCustomer]
ON [dbo].[Configurations]
    ([CustomerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------