DECLARE @ExistingSchemaSeparated nvarchar(50) = (SELECT sys.schemas.name FROM sys.objects INNER JOIN sys.schemas ON sys.objects.schema_id = sys.schemas.schema_id WHERE sys.objects.name LIKE 'CMS_Class'),
		@CurrentSchemaSeparated nvarchar(50) = SCHEMA_NAME(),
		@CurrentUserSeparated nvarchar(200) = USER_NAME(),
		@querySeparated nvarchar(100) = NULL;

IF ((SELECT 1 FROM sys.database_principals WHERE name = @CurrentUserSeparated) IS NULL)
BEGIN
	SET @querySeparated = 'CREATE USER [' + @CurrentUserSeparated + '] FOR LOGIN [' + @CurrentUserSeparated + '] WITH DEFAULT_SCHEMA = [' + @ExistingSchemaSeparated + ']';
END
ELSE IF (@CurrentSchemaSeparated IS NULL OR @CurrentSchemaSeparated != @ExistingSchemaSeparated)
BEGIN
	SET @querySeparated = 'ALTER USER [' + @CurrentUserSeparated + '] WITH DEFAULT_SCHEMA = [' + @ExistingSchemaSeparated + ']';
END

IF (NOT @querySeparated IS NULL)
BEGIN
	EXEC(@querySeparated);
END
GO

DROP VIEW [View_OM_ContactGroupMember_ContactJoined]
GO

IF NOT EXISTS (SELECT 1 FROM sys.table_types WHERE name = 'Type_OM_OrderedIntegerTable_DuplicatesAllowed')
BEGIN
  CREATE TYPE [Type_OM_OrderedIntegerTable_DuplicatesAllowed] AS TABLE(
    [Value] [int] NOT NULL,
    PRIMARY KEY CLUSTERED 
    (
      [Value] ASC
    )WITH (IGNORE_DUP_KEY = ON)
  )
END
GO

ALTER PROCEDURE [Proc_OM_ActivityRecalculationQueue_FetchActivityIDs]
AS
BEGIN 
	SET NOCOUNT ON;

	DECLARE @DeletedIDs Type_OM_OrderedIntegerTable_DuplicatesAllowed

	DELETE TOP (10000) FROM [OM_ActivityRecalculationQueue] 
	OUTPUT deleted.ActivityRecalculationQueueActivityID 
	INTO @DeletedIDs

	SELECT * FROM OM_Activity INNER JOIN @DeletedIDs ON [OM_Activity].[ActivityID] = [@DeletedIDs].[Value]
END
GO

