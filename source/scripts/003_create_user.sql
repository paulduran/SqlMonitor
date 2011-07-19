USE [master]
GO
CREATE LOGIN [SqlMonitor] WITH PASSWORD=N'$q1m0n', DEFAULT_DATABASE=[SqlMonitor], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [SqlMonitor]
GO
CREATE USER [SqlMonitor] FOR LOGIN [SqlMonitor]
GO
USE [SqlMonitor]
GO
EXEC sp_addrolemember N'db_owner', N'SqlMonitor'
GO
