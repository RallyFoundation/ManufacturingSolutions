CREATE TABLE [dbo].[Table_2] (
    [Column01] INT          IDENTITY (1, 1) NOT NULL,
    [Column02] VARCHAR (50) NOT NULL,
    [Column03] VARCHAR (50) NULL,
    [Column04] VARCHAR (50) NULL,
    [Column05] DATETIME     NULL,
    CONSTRAINT [PK_Table_2] PRIMARY KEY CLUSTERED ([Column01] ASC),
    CONSTRAINT [FK_Table_2_Table_1] FOREIGN KEY ([Column02]) REFERENCES [dbo].[Table_1] ([Column02])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Table_2]
    ON [dbo].[Table_2]([Column03] ASC);

