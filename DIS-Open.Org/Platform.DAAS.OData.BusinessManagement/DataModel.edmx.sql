
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/05/2016 11:45:27
-- Generated from EDMX file: D:\RallyFoundation\ManufacturingSolutions\DIS-Open.Org\Platform.DAAS.OData.BusinessManagement\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DISOpenDataPlatform];
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

IF OBJECT_ID(N'[dbo].[Businesses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Businesses];
GO
IF OBJECT_ID(N'[dbo].[Configurations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Configurations];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Businesses'
CREATE TABLE [dbo].[Businesses] (
    [Id] nvarchar(36)  NOT NULL,
    [Name] nvarchar(300)  NOT NULL,
    [TypeId] int  NOT NULL,
    [ReferenceId] nvarchar(max)  NULL
);
GO

-- Creating table 'Configurations'
CREATE TABLE [dbo].[Configurations] (
    [Id] nvarchar(36)  NOT NULL,
    [BusinessId] nvarchar(36)  NOT NULL,
    [TypeId] int  NOT NULL,
    [DbConnectionString] nvarchar(450)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Businesses'
ALTER TABLE [dbo].[Businesses]
ADD CONSTRAINT [PK_Businesses]
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

-- Creating foreign key on [BusinessId] in table 'Configurations'
ALTER TABLE [dbo].[Configurations]
ADD CONSTRAINT [FK_ConfigurationBusiness]
    FOREIGN KEY ([BusinessId])
    REFERENCES [dbo].[Businesses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConfigurationBusiness'
CREATE INDEX [IX_FK_ConfigurationBusiness]
ON [dbo].[Configurations]
    ([BusinessId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------