CREATE TABLE [dbo].[Configuration] (
    [ConfigurationID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50)  NOT NULL,
    [Value]           XML NOT NULL,
    [Type]            NVARCHAR (50)  NOT NULL
);

