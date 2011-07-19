USE [master]
GO
CREATE LOGIN [SqlMonitor] WITH PASSWORD=N'h4rr13r', DEFAULT_DATABASE=[Harrier], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [Harrier]
GO
CREATE USER [Harrier] FOR LOGIN [Harrier]
GO
USE [Harrier]
GO
EXEC sp_addrolemember N'db_owner', N'Harrier'
GO
