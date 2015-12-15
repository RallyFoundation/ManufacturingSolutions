CREATE TABLE [dbo].[TestParameter] (
    [TestParameterID] INT           IDENTITY (1, 1) NOT NULL,
    [TestID]          INT           NOT NULL,
    [Name]            VARCHAR (32)  NULL,
    [Index]           INT           NULL,
    [Value]           VARCHAR (MAX) NULL
);

