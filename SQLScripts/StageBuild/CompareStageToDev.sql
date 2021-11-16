USE OslerAlumni_Stage
GO

/*
* Site Settings
*/
SELECT 
	stage.KeyName, 
	stage.KeyValue AS Stage, 
	stage.KeyLastModified AS StageLastModified, 
	dev.KeyValue AS DEV, 
	dev.KeyLastModified AS DEVLastModified
FROM CMS_SettingsKey stage
INNER JOIN OslerAlumni_Dev.dbo.CMS_SettingsKey dev
ON stage.KeyName = dev.KeyName
AND (stage.SiteID = dev.SiteID
OR stage.SiteID IS NULL
AND dev.SiteID IS NULL)
WHERE stage.KeyValue != dev.KeyValue
and stage.KeyName NOT LIKE 'CMSDebug%'
and stage.KeyName NOT LIKE '%Email%From'
and stage.KeyName NOT LIKE '%Invitation%From'
and stage.KeyName NOT LIKE '%EmailAddress'
and stage.KeyName NOT IN (
	'CMSEmailsEnabled',
	'CMSScreenLockEnabled',
	'CMSSMTPServer',
	'CMSUseSSL');

/*
* Resource strings
*/
SELECT 
	stageRS.StringKey, 
	stageRT.TranslationCultureID,
	stageRT.TranslationText AS Stage, 
	devRT.TranslationText AS DEV
FROM CMS_ResourceString stageRS
INNER JOIN OslerAlumni_Dev.dbo.CMS_ResourceString devRS
ON stageRS.StringKey = devRS.StringKey
LEFT JOIN CMS_ResourceTranslation stageRT
ON stageRS.StringID = stageRT.TranslationStringID
LEFT JOIN OslerAlumni_Dev.dbo.CMS_ResourceTranslation devRT
ON devRS.StringID = devRT.TranslationStringID
WHERE stageRT.TranslationText != devRT.TranslationText
AND stageRT.TranslationCultureID = devRT.TranslationCultureID;

