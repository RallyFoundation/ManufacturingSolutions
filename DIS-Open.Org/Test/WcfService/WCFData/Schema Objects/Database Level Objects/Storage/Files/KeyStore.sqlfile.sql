ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [WCF], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', SIZE = 5376 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];



