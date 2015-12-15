ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [KeyStore], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', SIZE = 51456 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];
