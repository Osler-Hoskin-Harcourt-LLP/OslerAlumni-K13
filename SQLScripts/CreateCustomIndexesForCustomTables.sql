-- Get rid of old index
IF EXISTS(
		SELECT TOP 1 * 
		FROM sys.indexes 
		WHERE object_id = OBJECT_ID('dbo.ECA_CustomTable_PageURL')
		AND name = 'IX_PageURL_SiteID_Culture_URLPath_NodeGUID')
BEGIN
	DROP INDEX IX_PageURL_SiteID_Culture_URLPath_NodeGUID
	ON ECA_CustomTable_PageURL;

	PRINT('IX_PageURL_SiteID_Culture_URLPath_NodeGUID has been dropped')
END

-- Page URL - Non-Clustered Composite Index (SiteID, Culture, URLPath, NodeGUID)
IF EXISTS(
		SELECT TOP 1 * 
		FROM sys.indexes 
		WHERE object_id = OBJECT_ID('dbo.ECA_CustomTable_PageURL')
		AND name = 'IX_ECA_CustomTable_PageURL_SiteID_Culture_URLPath_NodeGUID_IsMainURL_RedirectType')
BEGIN
	PRINT('IX_ECA_CustomTable_PageURL_SiteID_Culture_URLPath_NodeGUID_IsMainURL_RedirectType already exists')
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX IX_ECA_CustomTable_PageURL_SiteID_Culture_URLPath_NodeGUID_IsMainURL_RedirectType
	ON ECA_CustomTable_PageURL(
		SiteID ASC,
		Culture ASC,
		URLPath ASC,
		NodeGUID
	)
	INCLUDE (
		IsMainURL,
		RedirectType
	)
END
GO

-- Page URL - Non-Clustered Composite Index (SiteID, Culture, URLPath)
IF EXISTS(
		SELECT TOP 1 * 
		FROM sys.indexes 
		WHERE object_id = OBJECT_ID('dbo.ECA_CustomTable_PageURL')
		AND name = 'IX_ECA_CustomTable_PageURL_SiteID_Culture_URLPath')
BEGIN
	PRINT('IX_ECA_CustomTable_PageURL_SiteID_Culture_URLPath already exists')
END
ELSE
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX IX_ECA_CustomTable_PageURL_SiteID_Culture_URLPath
	ON ECA_CustomTable_PageURL(
		SiteID ASC,
		Culture ASC,
		URLPath ASC
	)
END
GO

-- Page URL - Non-Clustered Composite Index (SiteID, Culture, URLPath)
IF EXISTS(
		SELECT TOP 1 * 
		FROM sys.indexes 
		WHERE object_id = OBJECT_ID('dbo.ECA_CustomTable_PageURL')
		AND name = 'IX_ECA_CustomTable_PageURL_MainUrl_SiteID_Culture_NodeGUID')
BEGIN
	PRINT('IX_ECA_CustomTable_PageURL_MainUrl_SiteID_Culture_NodeGUID already exists')
END
ELSE
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX IX_ECA_CustomTable_PageURL_MainUrl_SiteID_Culture_NodeGUID
	ON ECA_CustomTable_PageURL(
		SiteID ASC,
		Culture ASC,
		NodeGUID ASC
	)
	WHERE IsMainURL = 1
END
GO

-- Data Submission Queue - Non-Clustered Index (ContextObjectId, IsProcessed, ItemID, TotalAttempts)
IF EXISTS(
		SELECT TOP 1 * 
		FROM sys.indexes 
		WHERE object_id = OBJECT_ID('dbo.OslerAlumni_CustomTable_DataSubmissionQueue')
		AND name = 'IX_OslerAlumni_CustomTable_DataSubmissionQueue_ContextObjectType_ContextObjectId_Processed_ItemID_TotalAttempts')
BEGIN
	PRINT('IX_OslerAlumni_CustomTable_DataSubmissionQueue_ContextObjectType_ContextObjectId_Processed_ItemID_TotalAttempts already exists')
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX IX_OslerAlumni_CustomTable_DataSubmissionQueue_ContextObjectType_ContextObjectId_Processed_ItemID_TotalAttempts
	ON OslerAlumni_CustomTable_DataSubmissionQueue(
		ContextObjectType,
		ContextObjectId,
		IsProcessed,
		ItemID,
		TotalAttempts)
END
GO

-- Data Submission Queue - Non-Clustered Index (ItemID, IsProcessed, DependsOnItemIds, TotalAttempts)
IF EXISTS(
		SELECT TOP 1 * 
		FROM sys.indexes 
		WHERE object_id = OBJECT_ID('dbo.OslerAlumni_CustomTable_DataSubmissionQueue')
		AND name = 'IX_OslerAlumni_CustomTable_DataSubmissionQueue_ItemModifiedWhen_ItemID_IsProcessed_DependsOnItemIds_TotalAttempts')
BEGIN
	PRINT('IX_OslerAlumni_CustomTable_DataSubmissionQueue_ItemModifiedWhen_ItemID_IsProcessed_DependsOnItemIds_TotalAttempts already exists')
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX IX_OslerAlumni_CustomTable_DataSubmissionQueue_ItemModifiedWhen_ItemID_IsProcessed_DependsOnItemIds_TotalAttempts
	ON OslerAlumni_CustomTable_DataSubmissionQueue (
		ItemModifiedWhen)
	INCLUDE (
		ItemID,
		IsProcessed,
		DependsOnItemIds,
		TotalAttempts)
END
GO

-- Data Submission Queue - Non-Clustered Index (IsProcessed, DependsOnItemIds)
IF EXISTS(
		SELECT TOP 1 * 
		FROM sys.indexes 
		WHERE object_id = OBJECT_ID('dbo.OslerAlumni_CustomTable_DataSubmissionQueue')
		AND name = 'IX_OslerAlumni_CustomTable_DataSubmissionQueue_IsProcessed_DependsOnItemIds')
BEGIN
	PRINT('IX_OslerAlumni_CustomTable_DataSubmissionQueue_IsProcessed_DependsOnItemIds already exists')
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX IX_OslerAlumni_CustomTable_DataSubmissionQueue_IsProcessed_DependsOnItemIds
	ON OslerAlumni_CustomTable_DataSubmissionQueue (
		IsProcessed,
		DependsOnItemIds)
END
GO