CREATE TABLE [dbo].[User](
    [UserID]              [INT]             IDENTITY(1,1) NOT NULL,
    [Password]            [NVARCHAR](128)   NOT NULL,
    [PasswordRev]         [INT]             NOT NULL,
    [Salt]                [CHAR](10)        NULL,
    [LoginID]             [NVARCHAR](20)    NOT NULL,
    [Department]          [NVARCHAR](50)    NULL,
    [Phone]               [NVARCHAR](20)    NULL,
    [Email]               [NVARCHAR](50)    NULL,
    [CreateDate]          [DATETIME]        NOT NULL,
    [UpdateDate]          [DATETIME]        NOT NULL,
    [FirstName]           [NVARCHAR](20)    NULL,
    [SecondName]          [NVARCHAR](20)    NULL,
    [Position]            [NVARCHAR](20)    NULL,
    [Language]            [NVARCHAR](15)    NULL
);
