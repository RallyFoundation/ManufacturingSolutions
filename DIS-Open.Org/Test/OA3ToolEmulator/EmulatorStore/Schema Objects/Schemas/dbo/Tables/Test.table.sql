CREATE TABLE [dbo].[Test] (
    [TestID]      INT          IDENTITY (1, 1) NOT NULL,
    [TestName]    VARCHAR (32) NOT NULL,
    [IsPositive]  BIT          NOT NULL,
    [Status]      TINYINT      NOT NULL,
    [ReadyDate]   DATETIME     NULL,
    [UpdatedDate] DATETIME   NULL
);

