ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [OA3ToolEmulator], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

