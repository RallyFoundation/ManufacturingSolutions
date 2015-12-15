CREATE TABLE [dbo].[Table_1] (
    [Column01] INT          IDENTITY (1, 1) NOT NULL,
    [Column02] VARCHAR (50) NOT NULL,
    [Column03] VARCHAR (50) NULL,
    [Column04] VARCHAR (50) NULL,
    [Column05] DATETIME     NULL,
    CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED ([Column01] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Table_1]
    ON [dbo].[Table_1]([Column02] ASC);

