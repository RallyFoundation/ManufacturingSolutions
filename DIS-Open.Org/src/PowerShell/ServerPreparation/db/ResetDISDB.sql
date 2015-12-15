--Disabling Remote Connection
USE [master]
EXEC sys.sp_configure N'remote access', N'0'
GO
RECONFIGURE WITH OVERRIDE
GO

--Changing DB Authentication Mode to "Windows Integrated Mode"
USE [master]
GO
EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'LoginMode', REG_DWORD, 1
GO

USE [master]
GO
DROP LOGIN [DIS]
GO

--Creating a New Login of "DIS", and setting its default password as "D!S@OMSG.msft"
--USE [master]
--GO
--CREATE LOGIN [DIS] WITH PASSWORD=N'D!S@OMSG.msft', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
--GO

--USE [master]
--GO
--ALTER LOGIN [DIS] ENABLE
--GO

--EXEC sp_addsrvrolemember N'DIS', N'sysadmin'
--GO

--USE [master]
--GO
--CREATE USER [DIS] FOR LOGIN [DIS]
--GO

--USE [master]
--GO
--EXEC sp_addrolemember N'db_owner', N'DIS'
--GO



