IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'KLP')
	BEGIN
		CREATE DATABASE KLP;
		END
		GO
		USE KLP
		GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'bruger')
begin

create table bruger
(
navn varchar(255),
dato varchar(255),
nr_plade varchar(255)
)

end

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'koersels_log')
begin

create table koersels_log
(
navn varchar(255),
dato varchar(255),
nr_plade varchar(255),
opgave_beskrivelse text
)

end