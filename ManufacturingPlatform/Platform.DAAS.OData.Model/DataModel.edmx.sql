
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/22/2016 15:22:00
-- Generated from EDMX file: D:\RallyFoundation\ManufacturingSolutions\DIS-Open.Org\Platform.DAAS.OData.Model\DataModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_ApplicationService]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Services] DROP CONSTRAINT [FK_ApplicationService];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceServiceConsumption]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceConsumptions] DROP CONSTRAINT [FK_ServiceServiceConsumption];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceServiceSubscription]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceSubscriptions] DROP CONSTRAINT [FK_ServiceServiceSubscription];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Applications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Applications];
GO
IF OBJECT_ID(N'[dbo].[Services]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Services];
GO
IF OBJECT_ID(N'[dbo].[ServiceSubscriptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceSubscriptions];
GO
IF OBJECT_ID(N'[dbo].[ServiceConsumptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceConsumptions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Applications'
CREATE TABLE [dbo].[Applications] (
    [ID] nvarchar(36)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Description] nvarchar(800)  NULL,
    [Owner] nvarchar(50)  NOT NULL,
    [Status] int  NOT NULL,
    [CreationTime] datetime  NOT NULL
);
GO

-- Creating table 'Services'
CREATE TABLE [dbo].[Services] (
    [ID] nvarchar(36)  NOT NULL,
    [ApplicationID] nvarchar(36)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [DBType] int  NOT NULL,
    [ServiceType] int  NOT NULL,
    [ResourceName] nvarchar(100)  NOT NULL,
    [DBConnectionString] nvarchar(100)  NOT NULL,
    [Url] nvarchar(250)  NULL,
    [Address] nvarchar(150)  NULL,
    [Port] nvarchar(20)  NULL,
    [Parameters] nvarchar(250)  NULL,
    [UserName] nvarchar(50)  NULL,
    [Password] nvarchar(50)  NULL,
    [Charset] nvarchar(30)  NULL,
    [ContentType] nvarchar(30)  NULL,
    [DomainName] nvarchar(20)  NULL,
    [Size] bigint  NULL,
    [ModelMeta] nvarchar(max)  NULL,
    [ServiceCode] nvarchar(max)  NULL,
    [Binary] varbinary(max)  NULL,
    [Version] nvarchar(25)  NULL,
    [Status] int  NOT NULL,
    [Description] nvarchar(800)  NULL,
    [CreationTime] datetime  NOT NULL
);
GO

-- Creating table 'ServiceSubscriptions'
CREATE TABLE [dbo].[ServiceSubscriptions] (
    [ID] nvarchar(36)  NOT NULL,
    [Subscriber] nvarchar(50)  NOT NULL,
    [Status] int  NOT NULL,
    [ServiceID] nvarchar(36)  NOT NULL,
    [CreationTime] datetime  NOT NULL
);
GO

-- Creating table 'ServiceConsumptions'
CREATE TABLE [dbo].[ServiceConsumptions] (
    [ID] nvarchar(36)  NOT NULL,
    [Consumer] nvarchar(50)  NOT NULL,
    [UrlReferrer] nvarchar(250)  NULL,
    [Result] nvarchar(max)  NOT NULL,
    [ServiceID] nvarchar(36)  NOT NULL,
    [CreationTime] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Applications'
ALTER TABLE [dbo].[Applications]
ADD CONSTRAINT [PK_Applications]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [PK_Services]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServiceSubscriptions'
ALTER TABLE [dbo].[ServiceSubscriptions]
ADD CONSTRAINT [PK_ServiceSubscriptions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServiceConsumptions'
ALTER TABLE [dbo].[ServiceConsumptions]
ADD CONSTRAINT [PK_ServiceConsumptions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ApplicationID] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [FK_ApplicationService]
    FOREIGN KEY ([ApplicationID])
    REFERENCES [dbo].[Applications]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationService'
CREATE INDEX [IX_FK_ApplicationService]
ON [dbo].[Services]
    ([ApplicationID]);
GO

-- Creating foreign key on [ServiceID] in table 'ServiceConsumptions'
ALTER TABLE [dbo].[ServiceConsumptions]
ADD CONSTRAINT [FK_ServiceServiceConsumption]
    FOREIGN KEY ([ServiceID])
    REFERENCES [dbo].[Services]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceServiceConsumption'
CREATE INDEX [IX_FK_ServiceServiceConsumption]
ON [dbo].[ServiceConsumptions]
    ([ServiceID]);
GO

-- Creating foreign key on [ServiceID] in table 'ServiceSubscriptions'
ALTER TABLE [dbo].[ServiceSubscriptions]
ADD CONSTRAINT [FK_ServiceServiceSubscription]
    FOREIGN KEY ([ServiceID])
    REFERENCES [dbo].[Services]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceServiceSubscription'
CREATE INDEX [IX_FK_ServiceServiceSubscription]
ON [dbo].[ServiceSubscriptions]
    ([ServiceID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------