CREATE TABLE [dbo].[Table_3] (
    [Column01] INT          IDENTITY (1, 1) NOT NULL,
    [Column02] VARCHAR (50) NOT NULL,
    [Column03] VARCHAR (50) NULL,
    [Column04] VARCHAR (50) NULL,
    [Column05] DATETIME     NULL,
    CONSTRAINT [PK_Table_3] PRIMARY KEY CLUSTERED ([Column01] ASC),
    CONSTRAINT [FK_Table_3_Table_2] FOREIGN KEY ([Column02]) REFERENCES [dbo].[Table_2] ([Column03])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Table_3]
    ON [dbo].[Table_3]([Column04] ASC);

