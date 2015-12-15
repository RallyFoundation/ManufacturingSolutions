CREATE TABLE [dbo].[KeyOperationHistory] (
    [OperationID]  INT            IDENTITY (1, 1) NOT NULL,
    [ProductKeyID] BIGINT         NOT NULL,
    [ProductKey]   NVARCHAR (29)  NOT NULL,
    [HardwareHash] NVARCHAR (512) NULL,
    [KeyStateFrom] TINYINT        NOT NULL,
    [KeyStateTo]   TINYINT        NOT NULL,
    [Operator]     NVARCHAR (20)  NOT NULL,
    [Message]      NVARCHAR (200) NOT NULL,
    [CreatedDate]  DATETIME       NULL
);





