CREATE TABLE [dbo].[DuplicatedKey] (
    [DuplicatedKeyID]   INT           IDENTITY (1, 1) NOT NULL,
    [ProductKeyID]      BIGINT        NOT NULL,
    [ProductKey]        NVARCHAR (29) NOT NULL,
    [Handled]           BIT           NOT NULL,
    [OperationID]       INT           NULL
);

