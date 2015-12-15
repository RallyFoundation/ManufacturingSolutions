ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [WCF_log], FILENAME = '$(DefaultDataPath)$(DatabaseName)_log.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);



