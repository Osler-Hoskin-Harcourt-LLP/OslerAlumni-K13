-- TODO: UPDATE THE DB NAME
DECLARE @dbName AS NVARCHAR(100)
	= N'OslerAlumni_Dev';

DECLARE @backupLocation AS NVARCHAR(1000)
	= N'F:\ecentricarts_data\vismatullaeva\';

DECLARE @timestamp AS VARCHAR(13)
	= REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(16), GETDATE(), 120), ':', ''), '-', ''), ' ', '_');

DECLARE @backupFilePath AS NVARCHAR(1500)
	= CONCAT(@backupLocation, @dbName, '_', @timestamp, '.bak');

DECLARE @backupName AS NVARCHAR(200)
	= CONCAT(@dbName, N'-Full Database Backup');

-- TODO: UPDATE THE DB NAME
BACKUP DATABASE [OslerAlumni_Dev] 
TO  DISK = @backupFilePath
WITH 
	NOFORMAT, 
	INIT,  
	NAME = @backupName, 
	SKIP, 
	NOREWIND, 
	NOUNLOAD,  
	STATS = 10;

DECLARE @backupSetId AS INT

SELECT @backupSetId = position 
FROM msdb..backupset 
WHERE database_name = @dbName
AND backup_set_id = 
(
	SELECT max(backup_set_id) 
	FROM msdb..backupset 
	WHERE database_name = @dbName
)

DECLARE @errorMessage AS NVARCHAR(200)
	= CONCAT(N'Verify failed. Backup information for database ''', @dbName, ''' not found.');

IF @backupSetId IS NULL 
BEGIN 
	RAISERROR(@errorMessage, 16, 1) 
END 
RESTORE VERIFYONLY 
FROM DISK = @backupFilePath
WITH 
	FILE = @backupSetId, 
	NOUNLOAD, 
	NOREWIND
GO
