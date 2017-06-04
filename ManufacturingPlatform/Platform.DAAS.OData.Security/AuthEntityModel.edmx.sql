
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/27/2016 16:11:26
-- Generated from EDMX file: D:\RallyFoundation\ManufacturingSolutions\DIS-Open.Org\Platform.DAAS.OData.Security\AuthEntityModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_OperationObjectOperationAuthItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObjectOperationAuthItems] DROP CONSTRAINT [FK_OperationObjectOperationAuthItem];
GO
IF OBJECT_ID(N'[dbo].[FK_DataScopeRoleDataScope]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleDataScopes] DROP CONSTRAINT [FK_DataScopeRoleDataScope];
GO
IF OBJECT_ID(N'[dbo].[FK_OperationRoleOperation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleOperations] DROP CONSTRAINT [FK_OperationRoleOperation];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Operations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Operations];
GO
IF OBJECT_ID(N'[dbo].[DataScopes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DataScopes];
GO
IF OBJECT_ID(N'[dbo].[ObjectOperationAuthItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ObjectOperationAuthItems];
GO
IF OBJECT_ID(N'[dbo].[RoleOperations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleOperations];
GO
IF OBJECT_ID(N'[dbo].[RoleDataScopes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleDataScopes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Operations'
CREATE TABLE [dbo].[Operations] (
    [Id] nvarchar(250)  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [DataType] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'DataScopes'
CREATE TABLE [dbo].[DataScopes] (
    [Id] nvarchar(250)  NOT NULL,
    [ScopeName] nvarchar(250)  NOT NULL,
    [ScopeType] nvarchar(250)  NOT NULL,
    [DataType] nvarchar(250)  NOT NULL,
    [DataIdentifier] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'ObjectOperationAuthItems'
CREATE TABLE [dbo].[ObjectOperationAuthItems] (
    [Id] nvarchar(250)  NOT NULL,
    [ObjectId] nvarchar(250)  NOT NULL,
    [ActorId] nvarchar(250)  NOT NULL,
    [OperationId] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'RoleOperations'
CREATE TABLE [dbo].[RoleOperations] (
    [Id] nvarchar(250)  NOT NULL,
    [RoleId] nvarchar(250)  NOT NULL,
    [OperationId] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'RoleDataScopes'
CREATE TABLE [dbo].[RoleDataScopes] (
    [Id] nvarchar(250)  NOT NULL,
    [RoleId] nvarchar(250)  NOT NULL,
    [DataScopeId] nvarchar(250)  NOT NULL,
    [ScopeValue] nvarchar(250)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Operations'
ALTER TABLE [dbo].[Operations]
ADD CONSTRAINT [PK_Operations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DataScopes'
ALTER TABLE [dbo].[DataScopes]
ADD CONSTRAINT [PK_DataScopes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ObjectOperationAuthItems'
ALTER TABLE [dbo].[ObjectOperationAuthItems]
ADD CONSTRAINT [PK_ObjectOperationAuthItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleOperations'
ALTER TABLE [dbo].[RoleOperations]
ADD CONSTRAINT [PK_RoleOperations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleDataScopes'
ALTER TABLE [dbo].[RoleDataScopes]
ADD CONSTRAINT [PK_RoleDataScopes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [OperationId] in table 'ObjectOperationAuthItems'
ALTER TABLE [dbo].[ObjectOperationAuthItems]
ADD CONSTRAINT [FK_OperationObjectOperationAuthItem]
    FOREIGN KEY ([OperationId])
    REFERENCES [dbo].[Operations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OperationObjectOperationAuthItem'
CREATE INDEX [IX_FK_OperationObjectOperationAuthItem]
ON [dbo].[ObjectOperationAuthItems]
    ([OperationId]);
GO

-- Creating foreign key on [DataScopeId] in table 'RoleDataScopes'
ALTER TABLE [dbo].[RoleDataScopes]
ADD CONSTRAINT [FK_DataScopeRoleDataScope]
    FOREIGN KEY ([DataScopeId])
    REFERENCES [dbo].[DataScopes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DataScopeRoleDataScope'
CREATE INDEX [IX_FK_DataScopeRoleDataScope]
ON [dbo].[RoleDataScopes]
    ([DataScopeId]);
GO

-- Creating foreign key on [OperationId] in table 'RoleOperations'
ALTER TABLE [dbo].[RoleOperations]
ADD CONSTRAINT [FK_OperationRoleOperation]
    FOREIGN KEY ([OperationId])
    REFERENCES [dbo].[Operations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OperationRoleOperation'
CREATE INDEX [IX_FK_OperationRoleOperation]
ON [dbo].[RoleOperations]
    ([OperationId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------