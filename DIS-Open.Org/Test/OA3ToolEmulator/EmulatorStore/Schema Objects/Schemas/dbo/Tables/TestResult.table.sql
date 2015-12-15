CREATE TABLE [dbo].[TestResult] (
    [TestResultID] INT            IDENTITY (1, 1) NOT NULL,
    [TestID]       INT            NOT NULL,
    [ActualResult] BIT            NOT NULL,
    [Name]         VARCHAR (32)   NULL,
    [Index]        INT            NULL,
    [Value]        NVARCHAR (MAX) NULL,
    [UpdatedDate]  DATETIME       NULL,
    [Comments]     VARCHAR (MAX)  NULL
);

