CREATE TABLE [dbo].[Table_4] (
    [Column01] INT          IDENTITY (1, 1) NOT NULL,
    [Column02] VARCHAR (50) NOT NULL,
    [Column03] VARCHAR (50) NULL,
    [Column04] VARCHAR (50) NULL,
    [Column05] DATETIME     NULL,
    CONSTRAINT [PK_Table_4] PRIMARY KEY CLUSTERED ([Column01] ASC),
    CONSTRAINT [FK_Table_4_Table_3] FOREIGN KEY ([Column03]) REFERENCES [dbo].[Table_3] ([Column04])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Table_4]
    ON [dbo].[Table_4]([Column04] ASC);

