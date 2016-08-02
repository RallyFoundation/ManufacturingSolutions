
-- ----------------------------

-- Table structure for `Logs`

-- ----------------------------

--DROP TABLE IF EXISTS Logs;

/*
CREATE TABLE Logs (

  ID INT IDENTITY NOT NULL PRIMARY KEY ,
  
  [TimeStamp] datetime NOT NULL,
  
  Category nvarchar(25) NULL,

  LogLevel nvarchar(25) NOT NULL,

  CallSite nvarchar(max) DEFAULT NULL,

  [Message] nvarchar(max),

  StackTrace nvarchar(max) DEFAULT NULL,
  
  Exception nvarchar(max) DEFAULT NULL,
  
  MachineName nvarchar(30) DEFAULT NULL,

  [Identity] nvarchar(40) DEFAULT NULL,
   
  ProcessName nvarchar(40) DEFAULT NULL,
  
  ThreadName nvarchar(40) DEFAULT NULL,
  
  LoggerName nvarchar(40) DEFAULT NULL,

  Business nvarchar(40) DEFAULT NULL,

  Configuration nvarchar(40) DEFAULT NULL,

  [Application] nvarchar(40) DEFAULT NULL,

  [Service] nvarchar(40) DEFAULT NULL
);
*/

CREATE TABLE [dbo].[Logs] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [TimeStamp]     DATETIME       NOT NULL,
    [Category]      NVARCHAR (25)  NULL,
    [LogLevel]      NVARCHAR (25)  NOT NULL,
    [CallSite]      NVARCHAR (MAX) DEFAULT (NULL) NULL,
    [Message]       NVARCHAR (MAX) NULL,
    [StackTrace]    NVARCHAR (MAX) DEFAULT (NULL) NULL,
    [Exception]     NVARCHAR (MAX) DEFAULT (NULL) NULL,
    [MachineName]   NVARCHAR (30)  DEFAULT (NULL) NULL,
    [Identity]      NVARCHAR (40)  DEFAULT (NULL) NULL,
    [ProcessName]   NVARCHAR (40)  DEFAULT (NULL) NULL,
    [ThreadName]    NVARCHAR (40)  DEFAULT (NULL) NULL,
    [LoggerName]    NVARCHAR (40)  DEFAULT (NULL) NULL,
    [Business]      NVARCHAR (40)  DEFAULT (NULL) NULL,
    [Configuration] NVARCHAR (40)  DEFAULT (NULL) NULL,
    [Application]   NVARCHAR (40)  DEFAULT (NULL) NULL,
    [Service]       NVARCHAR (40)  DEFAULT (NULL) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
