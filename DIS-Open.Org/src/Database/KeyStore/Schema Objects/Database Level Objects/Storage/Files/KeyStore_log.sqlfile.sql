ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [KeyStore_log], FILENAME = '$(DefaultLogPath)$(DatabaseName)_log.ldf', SIZE = 12352 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

