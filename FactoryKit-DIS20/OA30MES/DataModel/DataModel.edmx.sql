
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/28/2015 15:33:02
-- Generated from EDMX file: E:\D\home\v-rawang\Documents\MAB\SourceCode\OA30MES\DataModel\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [OA3DPKSN];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ModelWorkPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkPackages] DROP CONSTRAINT [FK_ModelWorkPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkPackageTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions] DROP CONSTRAINT [FK_WorkPackageTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_TransactionOriginalFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OriginalFiles] DROP CONSTRAINT [FK_TransactionOriginalFile];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkPackageUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkPackages] DROP CONSTRAINT [FK_WorkPackageUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserRole];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ProductKeyIDSerialNumberPairs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductKeyIDSerialNumberPairs];
GO
IF OBJECT_ID(N'[dbo].[Models]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Models];
GO
IF OBJECT_ID(N'[dbo].[WorkPackages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkPackages];
GO
IF OBJECT_ID(N'[dbo].[Transactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions];
GO
IF OBJECT_ID(N'[dbo].[OriginalFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OriginalFiles];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ProductKeyIDSerialNumberPairs'
CREATE TABLE [dbo].[ProductKeyIDSerialNumberPairs] (
    [PairID] uniqueidentifier  NOT NULL,
    [ProductKeyID] bigint  NULL,
    [SerialNumber] nvarchar(200)  NOT NULL,
    [CreationTime] datetime  NULL,
    [TransactionID] nvarchar(150)  NULL
);
GO

-- Creating table 'Models'
CREATE TABLE [dbo].[Models] (
    [ID] nvarchar(150)  NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [BusinessID] nvarchar(150)  NULL,
    [Description] nvarchar(500)  NULL
);
GO

-- Creating table 'WorkPackages'
CREATE TABLE [dbo].[WorkPackages] (
    [ID] nvarchar(150)  NOT NULL,
    [ModelID] nvarchar(150)  NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [State] int  NOT NULL,
    [User_ID] nvarchar(150)  NOT NULL
);
GO

-- Creating table 'Transactions'
CREATE TABLE [dbo].[Transactions] (
    [ID] nvarchar(150)  NOT NULL,
    [WorkPackageID] nvarchar(150)  NOT NULL,
    [ReferenceID] nvarchar(200)  NULL,
    [ProductKeyID] bigint  NOT NULL,
    [SerialNumber] nvarchar(200)  NOT NULL,
    [CreationTime] datetime  NOT NULL,
    [SynchronizationTime] datetime  NOT NULL
);
GO

-- Creating table 'OriginalFiles'
CREATE TABLE [dbo].[OriginalFiles] (
    [ID] nvarchar(150)  NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Content] varbinary(max)  NULL,
    [TransactionID] nvarchar(150)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [ID] nvarchar(150)  NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Description] nvarchar(500)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] nvarchar(150)  NOT NULL,
    [FullName] nvarchar(150)  NOT NULL,
    [LoginName] nvarchar(150)  NOT NULL,
    [PasswordHash] nvarchar(150)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [State] int  NOT NULL,
    [Role_ID] nvarchar(150)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [PairID] in table 'ProductKeyIDSerialNumberPairs'
ALTER TABLE [dbo].[ProductKeyIDSerialNumberPairs]
ADD CONSTRAINT [PK_ProductKeyIDSerialNumberPairs]
    PRIMARY KEY CLUSTERED ([PairID] ASC);
GO

-- Creating primary key on [ID] in table 'Models'
ALTER TABLE [dbo].[Models]
ADD CONSTRAINT [PK_Models]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'WorkPackages'
ALTER TABLE [dbo].[WorkPackages]
ADD CONSTRAINT [PK_WorkPackages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [PK_Transactions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'OriginalFiles'
ALTER TABLE [dbo].[OriginalFiles]
ADD CONSTRAINT [PK_OriginalFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ModelID] in table 'WorkPackages'
ALTER TABLE [dbo].[WorkPackages]
ADD CONSTRAINT [FK_ModelWorkPackage]
    FOREIGN KEY ([ModelID])
    REFERENCES [dbo].[Models]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModelWorkPackage'
CREATE INDEX [IX_FK_ModelWorkPackage]
ON [dbo].[WorkPackages]
    ([ModelID]);
GO

-- Creating foreign key on [WorkPackageID] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [FK_WorkPackageTransaction]
    FOREIGN KEY ([WorkPackageID])
    REFERENCES [dbo].[WorkPackages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkPackageTransaction'
CREATE INDEX [IX_FK_WorkPackageTransaction]
ON [dbo].[Transactions]
    ([WorkPackageID]);
GO

-- Creating foreign key on [TransactionID] in table 'OriginalFiles'
ALTER TABLE [dbo].[OriginalFiles]
ADD CONSTRAINT [FK_TransactionOriginalFile]
    FOREIGN KEY ([TransactionID])
    REFERENCES [dbo].[Transactions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TransactionOriginalFile'
CREATE INDEX [IX_FK_TransactionOriginalFile]
ON [dbo].[OriginalFiles]
    ([TransactionID]);
GO

-- Creating foreign key on [User_ID] in table 'WorkPackages'
ALTER TABLE [dbo].[WorkPackages]
ADD CONSTRAINT [FK_WorkPackageUser]
    FOREIGN KEY ([User_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkPackageUser'
CREATE INDEX [IX_FK_WorkPackageUser]
ON [dbo].[WorkPackages]
    ([User_ID]);
GO

-- Creating foreign key on [Role_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserRole]
    FOREIGN KEY ([Role_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole'
CREATE INDEX [IX_FK_UserRole]
ON [dbo].[Users]
    ([Role_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------