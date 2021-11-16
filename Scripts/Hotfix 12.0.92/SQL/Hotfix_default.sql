DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN
	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Temp_FormDefinition'))
	BEGIN
	    CREATE TABLE [Temp_FormDefinition]
		(
			[TempID] [int] NOT NULL IDENTITY (1, 1),
			[ObjectName] [nvarchar] (200) NOT NULL,
			[FormDefinition] [nvarchar] (max) NULL,
			[IsAltForm] [bit] NULL,
			CONSTRAINT [PK_Temp_FormDefinition] PRIMARY KEY CLUSTERED ([TempID])
		) ON [PRIMARY];
	END
	ELSE
	BEGIN
		DELETE FROM [Temp_FormDefinition];
	END
END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

ALTER TABLE [CMS_Document]
    ADD
        [DocumentPageTemplateConfiguration] [nvarchar] (max) NULL,
		[DocumentABTestConfiguration] [nvarchar] (max) NULL;

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

CREATE TABLE [CMS_PageTemplateConfiguration]
(
    [PageTemplateConfigurationID] [int] NOT NULL IDENTITY (1, 1),
    [PageTemplateConfigurationGUID] [uniqueidentifier] NOT NULL,
    [PageTemplateConfigurationSiteID] [int] NOT NULL,
    [PageTemplateConfigurationLastModified] [datetime2] (7) NOT NULL,
    [PageTemplateConfigurationName] [nvarchar] (200) NOT NULL,
    [PageTemplateConfigurationDescription] [nvarchar] (max) NULL,
    [PageTemplateConfigurationThumbnailGUID] [uniqueidentifier] NULL,
    [PageTemplateConfigurationTemplate] [nvarchar] (max) NOT NULL,
    [PageTemplateConfigurationWidgets] [nvarchar] (max) NULL
) ON [PRIMARY];

ALTER TABLE [CMS_PageTemplateConfiguration] ADD

CONSTRAINT [DEFAULT_CMS_PageTemplateConfiguration_PageTemplateConfigurationGUID]
    DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [PageTemplateConfigurationGUID],

CONSTRAINT [DEFAULT_CMS_PageTemplateConfiguration_PageTemplateConfigurationLastModified]
    DEFAULT ('1/1/0001 12:00:00 AM') FOR [PageTemplateConfigurationLastModified],

CONSTRAINT [DEFAULT_CMS_PageTemplateConfiguration_PageTemplateConfigurationName]
    DEFAULT (N'') FOR [PageTemplateConfigurationName],

CONSTRAINT [DEFAULT_CMS_PageTemplateConfiguration_PageTemplateConfigurationSiteID]
    DEFAULT ((0)) FOR [PageTemplateConfigurationSiteID],

CONSTRAINT [DEFAULT_CMS_PageTemplateConfiguration_PageTemplateConfigurationTemplate]
    DEFAULT (N'') FOR [PageTemplateConfigurationTemplate],

CONSTRAINT [FK_CMS_PageTemplateConfiguration_PageTemplateConfigurationSiteID_CMS_Site]
    FOREIGN KEY ([PageTemplateConfigurationSiteID]) REFERENCES [CMS_Site] ([SiteID]),

CONSTRAINT [PK_CMS_PageTemplateConfiguration]
    PRIMARY KEY CLUSTERED ([PageTemplateConfigurationID])

CREATE NONCLUSTERED INDEX [IX_CMS_PageTemplateConfiguration_PageTemplateConfigurationSiteID]
    ON [CMS_PageTemplateConfiguration] ([PageTemplateConfigurationSiteID]) ON [PRIMARY]

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

ALTER TABLE [CMS_MacroRule]
    ADD
        [MacroRuleAvailability] [int] NULL,
	 	  CONSTRAINT [DEFAULT_CMS_MacroRule_MacroRuleAvailability]
	 	  		DEFAULT ((0)) FOR [MacroRuleAvailability];
END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

ALTER TABLE [Temp_PageBuilderWidgets]
    ADD
        [PageBuilderTemplateConfiguration] [nvarchar] (max) NULL;
END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

CREATE TABLE [OM_ABVariantData]
(
    [ABVariantID] [int] NOT NULL IDENTITY (1, 1),
    [ABVariantDisplayName] [nvarchar] (100) NOT NULL,
    [ABVariantGUID] [uniqueidentifier] NOT NULL,   
    [ABVariantTestID] [int] NOT NULL,  
    [ABVariantIsOriginal] [bit] NOT NULL    
) ON [PRIMARY];

ALTER TABLE [OM_ABVariantData] ADD

CONSTRAINT [DEFAULT_OM_ABVariantData_ABVariantDisplayName]
    DEFAULT (N'') FOR [ABVariantDisplayName],

CONSTRAINT [DEFAULT_OM_ABVariantData_ABVariantGUID]
    DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ABVariantGUID],

CONSTRAINT [DEFAULT_OM_ABVariantData_ABVariantTestID]
    DEFAULT ((0)) FOR [ABVariantTestID],
    
CONSTRAINT [DEFAULT_OM_ABVariantData_ABVariantIsOriginal]
    DEFAULT ((0)) FOR [ABVariantIsOriginal],

CONSTRAINT [FK_OM_ABVariantData_ABVariantTestID_OM_ABTest]
    FOREIGN KEY ([ABVariantTestID]) REFERENCES [OM_ABTest] ([ABTestID]),

CONSTRAINT [PK_OM_ABVariantData]
    PRIMARY KEY CLUSTERED ([ABVariantID])

CREATE NONCLUSTERED INDEX [IX_OM_ABVariantData_ABVariantTestID_ABVariantGUID]
    ON [OM_ABVariantData] ([ABVariantTestID], [ABVariantGUID]) ON [PRIMARY]

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

CREATE TABLE [CMS_AlternativeUrl]
(
    [AlternativeUrlID] [int] NOT NULL IDENTITY (1, 1),
    [AlternativeUrlGUID] [uniqueidentifier] NOT NULL,
    [AlternativeUrlDocumentID] [int] NOT NULL,
    [AlternativeUrlSiteID] [int] NOT NULL,
    [AlternativeUrlUrl] [nvarchar] (450) NOT NULL,
    [AlternativeUrlLastModified] [datetime2] (7) NOT NULL
) ON [PRIMARY];

ALTER TABLE [CMS_AlternativeUrl] ADD

CONSTRAINT [DEFAULT_CMS_AlternativeUrl_AlternativeUrlDocumentID]
    DEFAULT ((0)) FOR [AlternativeUrlDocumentID],

CONSTRAINT [DEFAULT_CMS_AlternativeUrl_AlternativeUrlGUID]
    DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [AlternativeUrlGUID],

CONSTRAINT [DEFAULT_CMS_AlternativeUrl_AlternativeUrlLastModified]
    DEFAULT ('1/1/0001 12:00:00 AM') FOR [AlternativeUrlLastModified],

CONSTRAINT [DEFAULT_CMS_AlternativeUrl_AlternativeUrlSiteID]
    DEFAULT ((0)) FOR [AlternativeUrlSiteID],

CONSTRAINT [DEFAULT_CMS_AlternativeUrl_AlternativeUrlUrl]
    DEFAULT (N'') FOR [AlternativeUrlUrl],

CONSTRAINT [FK_CMS_AlternativeUrl_CMS_Document]
    FOREIGN KEY ([AlternativeUrlDocumentID]) REFERENCES [CMS_Document] ([DocumentID]),

CONSTRAINT [FK_CMS_AlternativeUrl_CMS_Site]
    FOREIGN KEY ([AlternativeUrlSiteID]) REFERENCES [CMS_Site] ([SiteID]),

CONSTRAINT [PK_CMS_AlternativeUrl]
    PRIMARY KEY CLUSTERED ([AlternativeUrlID])

CREATE NONCLUSTERED INDEX [IX_CMS_AlternativeUrl_AlternativeUrlDocumentID]
    ON [CMS_AlternativeUrl] ([AlternativeUrlDocumentID]) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [IX_CMS_AlternativeUrl_AlternativeUrlSiteID_AlternativeUrlUrl]
    ON [CMS_AlternativeUrl] ([AlternativeUrlSiteID], [AlternativeUrlUrl]) ON [PRIMARY]

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints where name = 'DEFAULT_COM_GiftCard_GiftCardSiteID' AND parent_object_id = OBJECT_ID(N'COM_GiftCard'))
BEGIN

	ALTER TABLE [COM_GiftCard] ADD  CONSTRAINT [DEFAULT_COM_GiftCard_GiftCardSiteID]  DEFAULT ((0)) FOR [GiftCardSiteID]
	
END


END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN
	
	DECLARE @TREE_SCHEMA NVARCHAR(MAX)
	SELECT @TREE_SCHEMA = TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'CMS_Tree'

	DECLARE @DOCUMENT_SCHEMA NVARCHAR(MAX)
	SELECT @DOCUMENT_SCHEMA = TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'CMS_Document'

	DECLARE @CLASS_SCHEMA NVARCHAR(MAX)
	SELECT @CLASS_SCHEMA = TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'CMS_Class'

	DECLARE @ALTERVIEW VARCHAR(MAX) = 'ALTER VIEW View_CMS_Tree_Joined WITH SCHEMABINDING AS 
SELECT C.ClassName, C.ClassDisplayName, T.[NodeID], T.[NodeAliasPath], T.[NodeName], T.[NodeAlias], T.[NodeClassID], T.[NodeParentID], T.[NodeLevel], T.[NodeACLID], T.[NodeSiteID], T.[NodeGUID], T.[NodeOrder], T.[IsSecuredNode], T.[NodeCacheMinutes], T.[NodeSKUID], T.[NodeDocType], T.[NodeHeadTags], T.[NodeBodyElementAttributes], T.[NodeInheritPageLevels], T.[RequiresSSL], T.[NodeLinkedNodeID], T.[NodeOwner], T.[NodeCustomData], T.[NodeGroupID], T.[NodeLinkedNodeSiteID], T.[NodeTemplateID], T.[NodeTemplateForAllCultures], T.[NodeInheritPageTemplate], T.[NodeAllowCacheInFileSystem], T.[NodeHasChildren], T.[NodeHasLinks], T.[NodeOriginalNodeID], T.[NodeIsContentOnly], T.[NodeIsACLOwner], T.[NodeBodyScripts], D.[DocumentID], D.[DocumentName], D.[DocumentNamePath], D.[DocumentModifiedWhen], D.[DocumentModifiedByUserID], D.[DocumentForeignKeyValue], D.[DocumentCreatedByUserID], D.[DocumentCreatedWhen], D.[DocumentCheckedOutByUserID], D.[DocumentCheckedOutWhen], D.[DocumentCheckedOutVersionHistoryID], D.[DocumentPublishedVersionHistoryID], D.[DocumentWorkflowStepID], D.[DocumentPublishFrom], D.[DocumentPublishTo], D.[DocumentUrlPath], D.[DocumentCulture], D.[DocumentNodeID], D.[DocumentPageTitle], D.[DocumentPageKeyWords], D.[DocumentPageDescription], D.[DocumentShowInSiteMap], D.[DocumentMenuItemHideInNavigation], D.[DocumentMenuCaption], D.[DocumentMenuStyle], D.[DocumentMenuItemImage], D.[DocumentMenuItemLeftImage], D.[DocumentMenuItemRightImage], D.[DocumentPageTemplateID], D.[DocumentMenuJavascript], D.[DocumentMenuRedirectUrl], D.[DocumentUseNamePathForUrlPath], D.[DocumentStylesheetID], D.[DocumentContent], D.[DocumentMenuClass], D.[DocumentMenuStyleHighlighted], D.[DocumentMenuClassHighlighted], D.[DocumentMenuItemImageHighlighted], D.[DocumentMenuItemLeftImageHighlighted], D.[DocumentMenuItemRightImageHighlighted], D.[DocumentMenuItemInactive], D.[DocumentCustomData], D.[DocumentExtensions], D.[DocumentTags], D.[DocumentTagGroupID], D.[DocumentWildcardRule], D.[DocumentWebParts], D.[DocumentRatingValue], D.[DocumentRatings], D.[DocumentPriority], D.[DocumentType], D.[DocumentLastPublished], D.[DocumentUseCustomExtensions], D.[DocumentGroupWebParts], D.[DocumentCheckedOutAutomatically], D.[DocumentTrackConversionName], D.[DocumentConversionValue], D.[DocumentSearchExcluded], D.[DocumentLastVersionNumber], D.[DocumentIsArchived], D.[DocumentHash], D.[DocumentLogVisitActivity], D.[DocumentGUID], D.[DocumentWorkflowCycleGUID], D.[DocumentSitemapSettings], D.[DocumentIsWaitingForTranslation], D.[DocumentSKUName], D.[DocumentSKUDescription], D.[DocumentSKUShortDescription], D.[DocumentWorkflowActionStatus], D.[DocumentMenuRedirectToFirstChild], D.[DocumentCanBePublished], D.[DocumentInheritsStylesheet], D.[DocumentPageBuilderWidgets], D.[DocumentPageTemplateConfiguration], D.[DocumentABTestConfiguration]
FROM [' + @TREE_SCHEMA + '].[CMS_Tree] T INNER JOIN [' + @DOCUMENT_SCHEMA + '].CMS_Document D ON T.NodeOriginalNodeID = D.DocumentNodeID INNER JOIN [' + @CLASS_SCHEMA + '].CMS_Class C ON T.NodeClassID = C.ClassID';

	EXEC (@ALTERVIEW);

	IF EXISTS(SELECT TOP 1 * FROM sys.views where name = N'View_CMS_Tree_Joined') 
	AND NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'IX_View_CMS_Tree_Joined_NodeSiteID_DocumentCulture_NodeID' AND object_id = OBJECT_ID('View_CMS_Tree_Joined'))
	BEGIN
		CREATE UNIQUE CLUSTERED INDEX [IX_View_CMS_Tree_Joined_NodeSiteID_DocumentCulture_NodeID] ON [View_CMS_Tree_Joined]
		(
			[NodeSiteID] ASC,
			[DocumentCulture] ASC,
			[NodeID] ASC
		)
	END	

	IF EXISTS(SELECT TOP 1 * FROM sys.views where name = N'View_CMS_Tree_Joined') 
	AND NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'IX_View_CMS_Tree_Joined_ClassName_NodeSiteID_DocumentForeignKeyValue_DocumentCulture' AND object_id = OBJECT_ID('View_CMS_Tree_Joined'))
	BEGIN
		CREATE NONCLUSTERED INDEX [IX_View_CMS_Tree_Joined_ClassName_NodeSiteID_DocumentForeignKeyValue_DocumentCulture] ON [View_CMS_Tree_Joined]
		(
			[ClassName] ASC,
			[NodeSiteID] ASC,
			[DocumentForeignKeyValue] ASC,
			[DocumentCulture] ASC
		)
	END
END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Proc_CMS_EventLog_DeleteOlderLogs]') AND type = (N'P'))
	BEGIN
	EXEC('ALTER PROCEDURE [Proc_CMS_EventLog_DeleteOlderLogs]
	@SiteId int,
	@LogMaxSize int,
	@MaxToDelete int,
	@DeleteEventsOfNonexistingSites bit
AS
BEGIN
	IF @SiteId > 0 
	BEGIN
		;WITH SiteEventsRowNumber AS (
			SELECT TOP (@MaxToDelete+@LogMaxSize)
				ROW_NUMBER() OVER(ORDER BY EventID DESC) RowNumber
			FROM CMS_EventLog
			WHERE SiteID = @SiteId
		)
		DELETE
		FROM SiteEventsRowNumber
		WHERE RowNumber > @LogMaxSize

		SELECT COUNT(EventID) AS RemainEventsCount
		FROM CMS_EventLog
		WHERE SiteID = @SiteId
	END
	ELSE
	BEGIN
		;WITH GlobalEventsRowNumber AS (
			SELECT TOP (@MaxToDelete+@LogMaxSize)
				ROW_NUMBER() OVER(ORDER BY EventID DESC) RowNumber
			FROM CMS_EventLog
			WHERE SiteID IS NULL
		)
		DELETE
		FROM GlobalEventsRowNumber
		WHERE RowNumber > @LogMaxSize
		
		SELECT COUNT(EventID) AS RemainEventsCount
		FROM CMS_EventLog
		WHERE SiteID IS NULL
	END

	IF @DeleteEventsOfNonexistingSites = 1
	BEGIN
		DELETE E
		FROM CMS_EventLog E
		LEFT JOIN CMS_Site S
			ON S.SiteID = E.SiteID
		WHERE E.SiteID IS NOT NULL
			AND S.SiteID IS NULL
	END
END')
	END
END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

CREATE TYPE [Type_OM_ABVariantDataTable] AS TABLE(
	[ABVariantGUID] [uniqueidentifier] NOT NULL,
	[ABVariantDisplayName] [nvarchar](100) NOT NULL,
	[ABVariantIsOriginal] [bit] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ABVariantGUID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)

END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

EXEC('CREATE PROCEDURE [Proc_OM_UpsertABVariantData]
		@ABTestID int,
		@VariantsTable [Type_OM_ABVariantDataTable] READONLY
	AS
	BEGIN	
		-- SET XACT_ABORT - Always abort execution on unexpected errors - do not persist incorrect data
		-- SET NOCOUNT ON - Added to prevent extra result sets from interfering with SELECT statements.
		SET XACT_ABORT, NOCOUNT ON

		BEGIN TRY
			BEGIN TRANSACTION

				DELETE FROM [OM_ABVariantData] WHERE [ABVariantTestID] = @ABTestID AND NOT EXISTS (SELECT 1 FROM @VariantsTable t WHERE [OM_ABVariantData].[ABVariantGUID] = t.[ABVariantGUID])
		
				UPDATE [OM_ABVariantData]
				SET [ABVariantDisplayName] = t.[ABVariantDisplayName], 
					[ABVariantIsOriginal] = t.[ABVariantIsOriginal]
				FROM @VariantsTable as t
				WHERE [OM_ABVariantData].[ABVariantTestID] = @ABTestID AND [OM_ABVariantData].[ABVariantGUID] = t.[ABVariantGUID]

				INSERT INTO [OM_ABVariantData]
					   ([ABVariantDisplayName]
					   ,[ABVariantGUID]
					   ,[ABVariantIsOriginal]
					   ,[ABVariantTestID])
				SELECT [ABVariantDisplayName], [ABVariantGUID], [ABVariantIsOriginal], @ABTestID
				FROM @VariantsTable t WHERE NOT EXISTS (SELECT 1 FROM [OM_ABVariantData] d WHERE d.[ABVariantTestID] = @ABTestID AND t.[ABVariantGUID] = d.[ABVariantGUID])

			COMMIT TRANSACTION		
		END TRY
		BEGIN CATCH
			IF @@trancount > 0 ROLLBACK TRANSACTION
			
			-- Get the details of the error that invoked the CATCH block
			 DECLARE
			  @ErMessage NVARCHAR(2048),
			  @ErSeverity INT,
			  @ErState INT
	 
			 SELECT
			  @ErMessage = ERROR_MESSAGE(),
			  @ErSeverity = ERROR_SEVERITY(),
			  @ErState = ERROR_STATE()
	 
			 RAISERROR (@ErMessage,
						 @ErSeverity,
						 @ErState )
		END CATCH
	END')

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @keyCategoryID int;
SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.System.General')
IF @keyCategoryID IS NOT NULL BEGIN

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSHotfixDataVersion', '{$settingskey.cmshotfixdataversion$}', '{$settingskey.cmshotfixdataversion.description$}', '0', 'int', @keyCategoryID, NULL, '1b9c9cfc-994a-4fc5-9964-55ae2bd41226', getDate(), 12, '0', NULL, NULL, 1, 0, 1, NULL, '')

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSHotfixProcedureInProgress', '{$settingskey.cmshotfixprocedureinprogress$}', '{$settingskey.cmshotfixprocedureinprogress.description$}', 'False', 'boolean', @keyCategoryID, NULL, '9cbccb00-a6e9-4caf-93f5-9b574a362377', getDate(), 13, 'False', NULL, NULL, 1, 0, 1, NULL, '')


END

END


GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 2
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '79077afd-9f91-40fa-8926-7904f0e498f9')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = '226b13a3-97c2-4895-99e4-39ea6247a399')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '16e96e6c-f16f-49dc-a640-2357418668b8')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementAccessCondition] = '{% CurrentUser.IsAuthorizedPerGroup(EditedObjectParent.GroupGroupID, "Read", CurrentSiteID) @%}'
    WHERE [ElementGUID] = 'de463408-6153-447b-b7ac-785479d98087'


END

END

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

UPDATE [CMS_WorkflowAction] SET 
        [ActionParameters] = '<form version="2"><field column="Site" columnsize="200" columntype="text" guid="c8831e05-2006-4cc1-a507-7b5d3222c597" hasdependingfields="true" publicfield="false" visible="true"><properties><defaultvalue>{% SiteContext.CurrentSiteName |(identity)GlobalAdministrator|(hash)ff4693d82ee7c6bb1b379a81b107ad6180a04cb5402207e5363f2053195bbae3%}</defaultvalue><fieldcaption>{$general.site$}</fieldcaption><fielddescription>{$siteselect.selectitem$}</fielddescription></properties><settings><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix><AllowAll>False</AllowAll><AllowDefault>False</AllowDefault><AllowEditTextBox>False</AllowEditTextBox><AllowEmpty>False</AllowEmpty><controlname>Uni_selector</controlname><DialogWindowName>SelectionDialog</DialogWindowName><EditDialogWindowHeight>700</EditDialogWindowHeight><EditDialogWindowWidth>1000</EditDialogWindowWidth><EditWindowName>EditWindow</EditWindowName><EncodeOutput>True</EncodeOutput><GlobalObjectSuffix ismacro="true">{$general.global$}</GlobalObjectSuffix><ItemsPerPage>25</ItemsPerPage><LocalizeItems>True</LocalizeItems><MaxDisplayedItems>25</MaxDisplayedItems><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ObjectType>cms.site</ObjectType><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>SiteName</ReturnColumnName><ReturnColumnType>id</ReturnColumnType><SelectionMode>1</SelectionMode><ValuesSeparator>;</ValuesSeparator></settings></field><field column="NewsletterName" columnsize="200" columntype="text" dependsonanotherfield="true" guid="08ce1c91-cf93-41cd-86de-833768c3b7b4" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>{$ma.action.newslettersubscription.newslettername.caption$}</fieldcaption><fielddescription>{$ma.action.newslettersubscription.newslettername.description$}</fielddescription></properties><settings><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix><AllowAll>False</AllowAll><AllowDefault>False</AllowDefault><AllowEditTextBox>False</AllowEditTextBox><AllowEmpty>False</AllowEmpty><controlname>uni_selector</controlname><DialogWindowName>SelectionDialog</DialogWindowName><FilterControl>~/CMSFormControls/Filters/SiteFilter.ascx</FilterControl><GlobalObjectSuffix>(global)</GlobalObjectSuffix><ItemsPerPage>25</ItemsPerPage><LocalizeItems>True</LocalizeItems><MaxDisplayedItems>25</MaxDisplayedItems><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ObjectSiteName ismacro="true">{% Site.Value |(identity)GlobalAdministrator|(hash)3648950c87730d232de2dd9d37a972c3778a73939508e5abfb37b2f6892f052b%}</ObjectSiteName><ObjectType>newsletter.newsletter</ObjectType><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>NewsletterName</ReturnColumnName><ReturnColumnType>id</ReturnColumnType><SelectionMode>1</SelectionMode><ValuesSeparator>;</ValuesSeparator><WhereCondition>NewsletterType = 0</WhereCondition></settings></field><field column="Action" columnsize="20" columntype="text" guid="128094a1-4e62-444a-bdb6-0430ac5e73f6" hasdependingfields="true" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>0</defaultvalue><explanationtext ismacro="true">{% Action.Value == &quot;0&quot; ? GetResourceString(&quot;ma.action.newslettersubscription.action.explanation&quot;) : &quot;&quot; |(identity)GlobalAdministrator|(hash)3307a443df73948f962c52481c0281ef088055b991f50f4b2988ab6c9f73daf7%}</explanationtext><fieldcaption>{$general.action$}</fieldcaption><fielddescription>{$ma.action.newslettersubscription.action.description$}</fielddescription></properties><settings><controlname>RadioButtonsControl</controlname><Options>0;{$newsletter.subscriber.addto$}
1;{$newsletter.subscriber.removefrom$}</Options><RepeatDirection>vertical</RepeatDirection><RepeatLayout>Flow</RepeatLayout></settings></field><field column="InheritDoubleOptIn" columntype="boolean" guid="89948853-18f8-479a-9cba-32ba41b9cf83" publicfield="false" visible="true"><properties><defaultvalue>True</defaultvalue><fieldcaption>{$ma.action.newslettersubscription.inheritdoubleoptin.caption$}</fieldcaption><fielddescription>{$ma.action.newslettersubscription.inheritdoubleoptin.description$}</fielddescription></properties><settings><controlname>CheckBoxControl</controlname></settings></field></form>'
    WHERE [ActionGUID] = '2d3729a5-f9ea-4552-9ff4-0c6ed5215ea1'


END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '72c0cefd-aa08-4a6a-b11c-2891ee14325a')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = '226b13a3-97c2-4895-99e4-39ea6247a399')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '98c6ee00-230a-4207-a6d3-03677b83b245')
IF @elementResourceID IS NOT NULL BEGIN

DECLARE @newElementGUID uniqueidentifier = '0ebd31b7-73bc-4ed3-a06e-aa2f41a94c3c';

-- Insert a new UI element
INSERT [CMS_UIElement] ([ElementDisplayName], [ElementName], [ElementCaption], [ElementTargetURL], [ElementResourceID], [ElementParentID], [ElementChildCount], [ElementOrder], [ElementLevel], [ElementIDPath], [ElementIconPath], [ElementIsCustom], [ElementLastModified], [ElementGUID], [ElementSize], [ElementDescription], [ElementFromVersion], [ElementPageTemplateID], [ElementType], [ElementProperties], [ElementIsMenu], [ElementFeature], [ElementIconClass], [ElementIsGlobalApplication], [ElementCheckModuleReadPermission], [ElementAccessCondition], [ElementVisibilityCondition], [ElementRequiresGlobalAdminPriviligeLevel])
 VALUES ('{$app.pagetemplatesmvc.caption$}', 'PageTemplatesMvc', '', '', @elementResourceID, @elementParentID, 0, 20, 3, '', '', 0, getDate(), @newElementGUID, 0, '{$app.pagetemplatesmvc.description$}', '12.0', @elementPageTemplateID, 'PageTemplate', '<Data><category_name_Header>False</category_name_Header><DescriptionLink>page_templates_using_mvc</DescriptionLink><DisplayBreadcrumbs>False</DisplayBreadcrumbs><EditInDialog>False</EditInDialog><ExtenderClassName>CMS.UIControls.MVCPageTemplatesListingExtender</ExtenderClassName><GridExtender>CMS.UIControls</GridExtender><ObjectType>cms.pagetemplateconfiguration</ObjectType><OpenInDialog>False</OpenInDialog><OrderBy>PageTemplateConfigurationName</OrderBy><SmartTipExpandedHeader ismacro="True">{$app.pagetemplatesmvc.smarttip.header$}</SmartTipExpandedHeader><SmartTipText>{$app.pagetemplatesmvc.smarttip.content$}</SmartTipText><WhereCondition>PageTemplateConfigurationSiteID = {% CurrentSite.SiteID @%}</WhereCondition></Data>', 0, '', 'icon-layouts', 0, 0, '{% CurrentUser.IsAuthorizedPerResource("CMS.Content", "ManagePageTemplates") @%}', '{%CurrentSite.SiteIsContentOnly @%}', 0)


-- Update ID path
UPDATE [CMS_UIElement] SET
	[ElementIDPath] = COALESCE((SELECT TOP 1 [ElementIDPath] FROM [CMS_UIElement] AS [Parent] WHERE [Parent].ElementID = @elementParentID), '')
						  + '/'
						  + REPLICATE('0', 8 - LEN([ElementID]))
						  + CAST([ElementID] AS NVARCHAR(8))

WHERE [ElementGUID] = @newElementGUID


-- Update parent's child count
UPDATE [CMS_UIElement] SET
	[ElementChildCount] = (SELECT COUNT(*)
									FROM [CMS_UIElement] AS [Children]
									WHERE [Children].[ElementParentID] = @elementParentID)
WHERE [ElementID] = @elementParentID



END

END

END

END



GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '2432a6d5-1d93-4952-8e58-5952b5e28ccd')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = '226b13a3-97c2-4895-99e4-39ea6247a399')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '8089f9a7-95d9-4e3c-b95a-91a49ccf10c4')
IF @elementResourceID IS NOT NULL BEGIN

DECLARE @ELEMENTGUID uniqueidentifier;
SET @ELEMENTGUID = '69a37c22-1121-42d6-bd26-77831082015d';
IF NOT EXISTS (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = @ELEMENTGUID)
BEGIN

INSERT [CMS_UIElement] ([ElementDisplayName], [ElementName], [ElementCaption], [ElementTargetURL], [ElementResourceID], [ElementParentID], [ElementChildCount], [ElementOrder], [ElementLevel], [ElementIconPath], [ElementIsCustom], [ElementLastModified], [ElementGUID], [ElementSize], [ElementDescription], [ElementFromVersion], [ElementPageTemplateID], [ElementType], [ElementProperties], [ElementIsMenu], [ElementFeature], [ElementIconClass], [ElementIsGlobalApplication], [ElementCheckModuleReadPermission], [ElementAccessCondition], [ElementVisibilityCondition], [ElementRequiresGlobalAdminPriviligeLevel], [ElementIDPath])
 VALUES ('{$dataprotection.consents.consentagreements$}', 'Consents.ConsentAgreements', NULL, NULL, @elementResourceID, @elementParentID, 0, 4, 6, NULL, 0, getDate(), @ELEMENTGUID, 0, NULL, '12.0', @elementPageTemplateID, 'PageTemplate', '<Data><DisplayBreadcrumbs>False</DisplayBreadcrumbs><EditInDialog>False</EditInDialog><ExtenderClassName>CMS.UIControls.ConsentAgreementListUniGridExtender</ExtenderClassName><GridExtender>CMS.UIControls</GridExtender><GridName>~/App_Data/CMSModules/DataProtection/UI/Grids/CMS_ConsentAgreement/list.xml</GridName><ObjectType>cms.consentagreement</ObjectType><OpenInDialog>False</OpenInDialog><ZeroRowsText>{$dataprotection.consents.consentagreements.nodata$}</ZeroRowsText></Data>', 0, NULL, '', 0, 1, NULL, NULL, 0, '')

UPDATE [CMS_UIElement] SET
	[ElementChildCount] = (SELECT COUNT(*)
									FROM [CMS_UIElement] AS [Children]
									WHERE [Children].[ElementParentID] = @elementParentID)
WHERE [ElementID] = @elementParentID


UPDATE [CMS_UIElement] SET
	[ElementIDPath] = COALESCE((SELECT TOP 1 [ElementIDPath] FROM [CMS_UIElement] AS [Parent] WHERE [Parent].ElementID = @elementParentID), '')
						  + '/'
						  + REPLICATE('0', 8 - LEN([ElementID]))
						  + CAST([ElementID] AS NVARCHAR(8))

WHERE [ElementGUID] = @ELEMENTGUID
 
END

END

END

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 5
BEGIN

DECLARE @taskResourceID int;
SET @taskResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '6b0a5d42-3671-4eec-8d05-0d3d249ef207')
IF @taskResourceID IS NOT NULL BEGIN

DECLARE @interval nvarchar(1000);
SET @interval = (SELECT TOP 1 [TaskInterval] FROM [CMS_ScheduledTask] WHERE [TaskGUID] = '5efb0637-041c-4260-b570-05eff059ee91')
  IF (@interval LIKE N'hour;%' AND @interval LIKE N'%;4;%')
  BEGIN
   -- Shorten the interval from 4 hours to 1 minute, if not already customized
	SET @interval = REPLACE(@interval, N'hour;', N'minute;')
	SET @interval = REPLACE(@interval, N';4;', N';1;')

	UPDATE [CMS_ScheduledTask] SET [TaskInterval] = @interval WHERE [TaskGUID] = '5efb0637-041c-4260-b570-05eff059ee91'
  END

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '83ff58cf-d7ed-4567-a68c-439daf7e85cf')
IF @classResourceID IS NOT NULL BEGIN

INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES ('cms.document',
        '<form version="2"><field column="DocumentID" columntype="integer" guid="04c53ea8-89c6-45fe-b9f8-11c869742937" isPK="true" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>DocumentID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field allowempty="true" column="DocumentGUID" columntype="guid" guid="b0dc7e57-96dd-4e5b-829a-4ba9bd84ac0f" publicfield="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field><field column="DocumentName" columnsize="100" columntype="text" guid="1e0f27f5-f59a-4fa1-871f-5c2d946453ca" publicfield="false" system="true" translatefield="true" visible="true"><properties><fieldcaption>DocumentName</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentNamePath" columnsize="1500" columntype="text" guid="4afd853c-e3da-46dd-87c1-aa931e249b99" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentNamePath</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentModifiedWhen" columntype="datetime" guid="d86fd91f-9650-459d-a6a9-f101ec936cdf" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentModifiedWhen</fieldcaption></properties><settings><controlname>calendarcontrol</controlname></settings></field><field allowempty="true" column="DocumentModifiedByUserID" columntype="integer" guid="3db3f1c9-02b4-4d37-abd5-1298a9068ac1" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentModifiedByUserID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentForeignKeyValue" columntype="integer" guid="1ab4dfd9-6e8c-4b4d-9526-528890bc9c47" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentForeignKeyValue</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentCreatedByUserID" columntype="integer" guid="89690d5f-2c54-4788-8926-b38692719e0e" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentCreatedByUserID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentCreatedWhen" columntype="datetime" guid="82f0e9c1-9dfa-42a1-9b4a-a0090bbfad73" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentCreatedWhen</fieldcaption></properties><settings><controlname>calendarcontrol</controlname></settings></field><field allowempty="true" column="DocumentCheckedOutByUserID" columntype="integer" guid="d561f437-cf47-4681-a94e-085d9632b926" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentCheckedOutByUserID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentCheckedOutWhen" columntype="datetime" guid="65160c58-f425-4370-baf6-b47dc987611d" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentCheckedOutWhen</fieldcaption></properties><settings><controlname>calendarcontrol</controlname></settings></field><field allowempty="true" column="DocumentCheckedOutVersionHistoryID" columntype="integer" guid="31bf940c-d22c-4bf8-a430-776b6d4488c7" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentCheckedOutVersionHistoryID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentPublishedVersionHistoryID" columntype="integer" guid="8229ebd2-c4d2-43bd-82e9-ae9af6146c97" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentPublishedVersionHistoryID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentWorkflowStepID" columntype="integer" guid="b7238cf1-94ac-4c59-87f4-f472f78245b2" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentWorkflowStepID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentPublishFrom" columntype="datetime" guid="5998e7fe-a503-4c5a-8711-9a4cbe77d8a3" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentPublishFrom</fieldcaption></properties><settings><controlname>calendarcontrol</controlname></settings></field><field allowempty="true" column="DocumentPublishTo" columntype="datetime" guid="75b22166-d757-485f-901a-6636cabe930e" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentPublishTo</fieldcaption></properties><settings><controlname>calendarcontrol</controlname></settings></field><field allowempty="true" column="DocumentUrlPath" columnsize="450" columntype="text" guid="ce4e33fb-c401-409f-bd58-d43bf642b1af" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentUrlPath</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field column="DocumentCulture" columnsize="10" columntype="text" guid="e123ee40-049c-48b6-9dd8-a51b1e7da6b1" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentCulture</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field column="DocumentNodeID" columntype="integer" guid="3bc79b36-ba8f-4fea-85a4-c72fdbc315d2" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentNodeID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentPageTitle" columntype="longtext" guid="a2df3057-5b8d-481b-9247-88b970c57a0b" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentPageTitle</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textareacontrol</controlname><FilterMode>False</FilterMode><IsTextArea>True</IsTextArea><Wrap>True</Wrap></settings></field><field allowempty="true" column="DocumentPageKeyWords" columntype="longtext" guid="e6a709b2-d460-4f13-9abf-c0f100e97033" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentPageKeyWords</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textareacontrol</controlname><FilterMode>False</FilterMode><IsTextArea>True</IsTextArea><Wrap>True</Wrap></settings></field><field allowempty="true" column="DocumentPageDescription" columntype="longtext" guid="4af8a60e-4d17-42c6-a830-1f6655aa807e" publicfield="false" system="true" translatefield="true" visible="true"><properties><fieldcaption>DocumentPageDescription</fieldcaption></properties><settings><controlname>textareacontrol</controlname></settings></field><field column="DocumentShowInSiteMap" columntype="boolean" guid="b82973af-c753-43c4-b98a-1e33382f381f" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentShowInSiteMap</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field column="DocumentMenuItemHideInNavigation" columntype="boolean" guid="2551533d-3329-445f-a008-b2912c36b7d5" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuItemHideInNavigation</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuCaption" columnsize="200" columntype="text" guid="b057698c-757f-478f-8e69-4218eff45127" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuCaption</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuStyle" columnsize="100" columntype="text" guid="35cafdb7-af8b-4243-bdce-babe46387a61" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuStyle</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuItemImage" columnsize="200" columntype="text" guid="722c4b74-aa68-4ab9-8a34-7cbc711ee0be" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>DocumentMenuItemImage</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DocumentMenuItemLeftImage" columnsize="200" columntype="text" guid="06f35e92-e38e-4af4-a7fd-db3a7e625310" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuItemLeftImage</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuItemRightImage" columnsize="200" columntype="text" guid="8bf6ec0a-3fcc-4024-a9e4-ce022a762b02" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuItemRightImage</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentPageTemplateID" columntype="integer" guid="0d26da2b-4ac7-489b-a91b-88034794f569" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentPageTemplateID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuJavascript" columnsize="450" columntype="text" guid="538ab879-552c-4d63-b4e0-f41307286544" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuJavascript</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuRedirectUrl" columnsize="450" columntype="text" guid="6df14253-cc1c-4ea7-8cd7-6c8375c3c44c" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuRedirectUrl</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentUseNamePathForUrlPath" columntype="boolean" guid="3091667f-22b4-4a4e-96ba-709e31f17a8f" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentUseNamePathForUrlPath</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentStylesheetID" columntype="integer" guid="1d74c412-a60d-4ff9-9966-549f3f9483fd" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentStylesheetID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field column="DocumentInheritsStylesheet" columntype="boolean" guid="b6a00eca-4134-4297-8b1b-ada4dc6398c8" publicfield="false" system="true"><properties><defaultvalue>True</defaultvalue></properties></field><field allowempty="true" column="DocumentContent" columntype="longtext" guid="7cc90d0b-19c9-49fa-a7ce-14104dd32586" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentContent</fieldcaption></properties><settings><controlname>textareacontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuClass" columnsize="100" columntype="text" guid="d47569fa-a1a5-476a-bb3f-25751812a790" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuClass</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuStyleHighlighted" columnsize="200" columntype="text" guid="e53e6cda-97c2-4fe4-9d04-8a9e639c1ee8" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuStyleHighlighted</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuClassHighlighted" columnsize="100" columntype="text" guid="ddaaa43a-c322-4d5e-9cd5-d7139f679883" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuClassHighlighted</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuItemImageHighlighted" columnsize="200" columntype="text" guid="dd5a4cb8-0fc7-4759-b062-6088b67b04ee" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuItemImageHighlighted</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuItemLeftImageHighlighted" columnsize="200" columntype="text" guid="240a6893-095d-44f0-aaa6-e32443d64f34" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuItemLeftImageHighlighted</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuItemRightImageHighlighted" columnsize="200" columntype="text" guid="3eaff1dd-1492-42f8-8035-1214d1dc0a6f" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuItemRightImageHighlighted</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentMenuItemInactive" columntype="boolean" guid="e0b5fe75-ad44-4221-a175-54706d85f2ae" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentMenuItemInactive</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentCustomData" columntype="longtext" guid="8c76cc14-a486-44f9-a1f2-1947192ca6a2" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentCustomData</fieldcaption></properties><settings><controlname>textareacontrol</controlname></settings></field><field allowempty="true" column="DocumentExtensions" columnsize="100" columntype="text" guid="1c1cbb47-7832-4158-9d56-fbb34d6ae87a" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentExtensions</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><Trim>False</Trim></settings></field><field allowempty="true" column="DocumentTags" columntype="longtext" guid="6a33a0ff-ffd9-4615-a1c8-5e1bba780964" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentTags</fieldcaption></properties><settings><controlname>textareacontrol</controlname></settings></field><field allowempty="true" column="DocumentTagGroupID" columntype="integer" guid="82d94a1c-a0e9-4496-bad0-6192217212bf" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentTagGroupID</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentWildcardRule" columnsize="440" columntype="text" guid="c827cdfb-bd56-4fe3-a781-5b6b12a3e79f" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>DocumentWildcardRule</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentWebParts" columntype="longtext" guid="770989d4-ef83-48ac-b6a7-9800cedeb67a" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentWebParts</fieldcaption></properties><settings><controlname>textareacontrol</controlname></settings></field><field allowempty="true" column="DocumentGroupWebParts" columntype="longtext" guid="596af945-57d7-432f-a12a-7b16bf385504" publicfield="false" system="true" visibility="none"><settings><controlname>labelcontrol</controlname></settings></field><field allowempty="true" column="DocumentRatingValue" columntype="double" guid="11320c2e-1c67-49e2-bd7a-7273e53f42db" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentRatingValue</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentRatings" columntype="integer" guid="abd2bd0d-8911-4e29-abcb-896d7e8cb8bc" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentRatings</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentPriority" columntype="integer" guid="97bfb6a2-ab54-416e-9578-a38844a4b48e" publicfield="false" spellcheck="false" system="true" visibility="none" visible="true"><properties><fieldcaption>DocumentPriority</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentType" columnsize="50" columntype="text" guid="bb21f9de-f10c-43e9-b200-cc4c25c97acf" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentType</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentLastPublished" columntype="datetime" guid="834785f6-f532-435f-b76f-c20dfd9399f6" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentLastPublished</fieldcaption></properties><settings><controlname>calendarcontrol</controlname></settings></field><field allowempty="true" column="DocumentUseCustomExtensions" columntype="boolean" guid="aeae798e-898e-428c-8468-eff4126e0299" publicfield="false" system="true" visible="true"><properties><fieldcaption>DocumentUseCustomExtensions</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentCheckedOutAutomatically" columntype="boolean" guid="dbb95f9e-3efd-485d-a5a5-1a4ccb455fbc" publicfield="false" spellcheck="false" system="true" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>DocumentCheckedOutAutomatically</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentTrackConversionName" columnsize="200" columntype="text" guid="ee4ecdbc-988a-4b25-9a35-e048e0bc0dea" publicfield="false" spellcheck="false" system="true" visibility="none" visible="true"><properties><fieldcaption>Track conversion name</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DocumentConversionValue" columnsize="100" columntype="text" guid="84eb8c75-f049-475a-8eae-c69a962afda6" publicfield="false" spellcheck="false" system="true" visibility="none" visible="true"><properties><fieldcaption>Page conversion value</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DocumentSearchExcluded" columntype="boolean" guid="05110f5f-a8dd-4e30-828c-ef767cfdb1e4" publicfield="false" spellcheck="false" system="true" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>Page search excluded</fieldcaption></properties><settings><controlname>CheckBoxControl</controlname></settings></field><field allowempty="true" column="DocumentLastVersionNumber" columnsize="50" columntype="text" guid="88338d99-7f91-4c20-8c84-a3dd97cb53a1" publicfield="false" spellcheck="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field><field allowempty="true" column="DocumentIsArchived" columntype="boolean" guid="41cad1db-9aa7-4818-ae85-a9651578e554" publicfield="false" system="true" visibility="none"><properties><defaultvalue>false</defaultvalue><fieldcaption>DocumentID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field allowempty="true" column="DocumentHash" columnsize="32" columntype="text" guid="7311e7a0-7113-4b2a-b136-39b13464eba0" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>DocumentHash</fieldcaption></properties><settings><controlname>textboxcontrol</controlname><FilterEnabled>False</FilterEnabled><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DocumentLogVisitActivity" columntype="boolean" guid="4c1517e1-c1bc-4341-b879-a8e29349b23b" publicfield="false" system="true" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>DocumentLogVisitActivity</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="DocumentWorkflowCycleGUID" columntype="guid" guid="7a8dbf8e-a387-4068-b255-247eff08351a" publicfield="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field><field allowempty="true" column="DocumentSitemapSettings" columnsize="100" columntype="text" guid="c46a652c-9206-45c6-bb29-006ead6091c6" publicfield="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field><field allowempty="true" column="DocumentIsWaitingForTranslation" columntype="boolean" guid="ab09e46a-07b9-4bc2-be12-7c5c54840ba9" publicfield="false" system="true" visibility="none"><properties><defaultvalue>false</defaultvalue></properties><settings><controlname>dropdownlistcontrol</controlname></settings></field><field allowempty="true" column="DocumentSKUName" columnsize="440" columntype="text" guid="32f0a3cf-cb9f-490e-a737-362123ec3cd4" publicfield="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field><field allowempty="true" column="DocumentSKUDescription" columntype="longtext" guid="1f04c541-6ebe-412f-94ee-ddd370c46044" publicfield="false" system="true" visibility="none"><settings><Autoresize_Hashtable>True</Autoresize_Hashtable><controlname>bbeditorcontrol</controlname><Dialogs_Anchor_Hide>False</Dialogs_Anchor_Hide><Dialogs_Attachments_Hide>False</Dialogs_Attachments_Hide><Dialogs_Content_Hide>False</Dialogs_Content_Hide><Dialogs_Email_Hide>False</Dialogs_Email_Hide><Dialogs_Libraries_Hide>False</Dialogs_Libraries_Hide><Dialogs_Web_Hide>False</Dialogs_Web_Hide><MediaDialogConfiguration>True</MediaDialogConfiguration><ShowAdvancedImage>False</ShowAdvancedImage><ShowAdvancedUrl>False</ShowAdvancedUrl></settings></field><field allowempty="true" column="DocumentSKUShortDescription" columntype="longtext" guid="6a14a367-6f85-4da9-9430-5aa4a6f037c4" publicfield="false" system="true" visibility="none"><settings><Autoresize_Hashtable>True</Autoresize_Hashtable><controlname>bbeditorcontrol</controlname><Dialogs_Anchor_Hide>False</Dialogs_Anchor_Hide><Dialogs_Attachments_Hide>False</Dialogs_Attachments_Hide><Dialogs_Content_Hide>False</Dialogs_Content_Hide><Dialogs_Email_Hide>False</Dialogs_Email_Hide><Dialogs_Libraries_Hide>False</Dialogs_Libraries_Hide><Dialogs_Web_Hide>False</Dialogs_Web_Hide><ShowAdvancedImage>False</ShowAdvancedImage><ShowAdvancedUrl>False</ShowAdvancedUrl></settings></field><field allowempty="true" column="DocumentWorkflowActionStatus" columnsize="450" columntype="text" guid="11d7b5dd-fae8-4f61-af02-ca3a196abcb3" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>DocumentWorkflowActionStatus</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DocumentMenuRedirectToFirstChild" columntype="boolean" guid="e64e00ad-f9a2-46af-a945-1f7b259d105d" publicfield="false" system="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>Redirect to first child</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field column="DocumentCanBePublished" columntype="boolean" guid="8d5d0089-266a-4f9d-9810-05cdb52f8cde" publicfield="false" system="true"><properties><defaultvalue>True</defaultvalue></properties></field><field allowempty="true" column="DocumentPageBuilderWidgets" columntype="longtext" guid="b3a3768e-3dfa-4f7c-9e62-de84dd322812" publicfield="false" system="true" /><field allowempty="true" column="DocumentPageTemplateConfiguration" columntype="longtext" guid="27ca67f3-bdeb-491e-b96b-8d52bac7d57a" publicfield="false" system="true" /><field allowempty="true" column="DocumentABTestConfiguration" columntype="longtext" guid="639cb356-b9c0-4714-99a8-faf4924d06da" publicfield="false" system="true" /></form>',
        0);

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '98c6ee00-230a-4207-a6d3-03677b83b245')
IF @classResourceID IS NOT NULL BEGIN

DECLARE @formDefinition nvarchar(max);
SET @formDefinition = '<form version="2"><field column="PageTemplateConfigurationID" columntype="integer" guid="8e91a6c7-f065-45f0-bc0f-9a559f347dcc" isPK="true" publicfield="false" system="true"><properties><fieldcaption>PageTemplateConfigurationID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="PageTemplateConfigurationGUID" columntype="guid" guid="a88502e2-f3bd-4719-95ee-e9bfcd8a5c89" publicfield="false" system="true"><properties><fieldcaption>GUID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="PageTemplateConfigurationSiteID" columntype="integer" guid="1b5d4637-60bd-4c69-8cb4-b75ca602b8cb" publicfield="false" system="true" /><field column="PageTemplateConfigurationLastModified" columnprecision="7" columntype="datetime" guid="f06efbab-9053-4ed6-a94c-90f558a96a0a" publicfield="false" system="true"><properties><fieldcaption>Last modified</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="PageTemplateConfigurationName" columnsize="200" columntype="text" guid="bba0b1ab-beef-4e95-b52c-cf138d223db9" publicfield="false" system="true" /><field allowempty="true" column="PageTemplateConfigurationDescription" columntype="longtext" guid="7cb27baa-1764-4880-a37a-df955bde0f1a" publicfield="false" system="true" /><field allowempty="true" column="PageTemplateConfigurationThumbnailGUID" columntype="guid" guid="0a090dad-0984-4643-b253-a03a187b17b4" publicfield="false" system="true" /><field column="PageTemplateConfigurationTemplate" columntype="longtext" guid="4d25c2dc-f0dc-49da-a5fa-a6da4459fe82" publicfield="false" system="true" /><field allowempty="true" column="PageTemplateConfigurationWidgets" columntype="longtext" guid="8657d5bd-e2ac-4869-bf69-9310f64f63fb" publicfield="false" system="true" /></form>'

INSERT [CMS_Class] ([ClassDisplayName], [ClassName], [ClassUsesVersioning], [ClassIsDocumentType], [ClassIsCoupledClass], [ClassXmlSchema], [ClassFormDefinition], [ClassEditingPageUrl], [ClassListPageUrl], [ClassNodeNameSource], [ClassTableName], [ClassViewPageUrl], [ClassPreviewPageUrl], [ClassFormLayout], [ClassNewPageUrl], [ClassShowAsSystemTable], [ClassUsePublishFromTo], [ClassShowTemplateSelection], [ClassSKUMappings], [ClassIsMenuItemType], [ClassNodeAliasSource], [ClassDefaultPageTemplateID], [ClassLastModified], [ClassGUID], [ClassCreateSKU], [ClassIsProduct], [ClassIsCustomTable], [ClassShowColumns], [ClassSearchTitleColumn], [ClassSearchContentColumn], [ClassSearchImageColumn], [ClassSearchCreationDateColumn], [ClassSearchSettings], [ClassInheritsFromClassID], [ClassSearchEnabled], [ClassSKUDefaultDepartmentName], [ClassSKUDefaultDepartmentID], [ClassContactMapping], [ClassContactOverwriteEnabled], [ClassSKUDefaultProductType], [ClassConnectionString], [ClassIsProductSection], [ClassPageTemplateCategoryID], [ClassFormLayoutType], [ClassVersionGUID], [ClassDefaultObjectType], [ClassIsForm], [ClassResourceID], [ClassCustomizedColumns], [ClassCodeGenerationSettings], [ClassIconClass], [ClassIsContentOnly], [ClassURLPattern])
 VALUES ('Page template configuration', 'CMS.PageTemplateConfiguration', 0, 0, 1, '', '', '', NULL, '', 'CMS_PageTemplateConfiguration', NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 0, NULL, NULL, getDate(), 'f3f579ea-6ea3-4ba8-a0ac-ccc9d0ba8bc4', NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CMSConnectionString', NULL, NULL, NULL, '', NULL, 0, @classResourceID, '', NULL, NULL, 0, NULL)

INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES ('CMS.PageTemplateConfiguration',
        @formDefinition,
        0);


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 7
BEGIN

DECLARE @pageTemplateCategoryID int;
SET @pageTemplateCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_PageTemplateCategory] WHERE [CategoryGUID] = '0cd9d6f5-4393-4f6d-9273-c7e667809496')
IF @pageTemplateCategoryID IS NOT NULL BEGIN

UPDATE [CMS_PageTemplate] SET 
        [PageTemplateProperties] = '<form version="2"><category name="Sselector"><properties><caption>Selector</caption><visible>True</visible></properties></category><field allowempty="true" column="SelectionMode" columnsize="10" columntype="text" displayinsimplemode="true" guid="9bebe338-6d8f-4e05-84ff-26f576d10d68" publicfield="false" visible="true"><properties><defaultvalue>1</defaultvalue><fieldcaption>Selection mode</fieldcaption><fielddescription>{$documentation.property.selectionmode$}</fielddescription></properties><settings><controlname>dropdownlistcontrol</controlname><EditText>False</EditText><Options>0;Single text box
1;Single drop down list
2;Multiple
3;Multiple text box
4;Single button
5;Multiple button</Options><SortItems>False</SortItems></settings></field><field allowempty="true" column="SelectorObjectType" columnsize="100" columntype="text" displayinsimplemode="true" guid="c0e14831-33fd-48cc-b2ab-35badbdda37f" publicfield="false" visible="true"><properties><fieldcaption>Selector object type</fieldcaption><fielddescription>{$documentation.property.objecttype$}</fielddescription></properties><settings><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix><AllowAll>False</AllowAll><AllowDefault>False</AllowDefault><AllowEditTextBox>False</AllowEditTextBox><AllowEmpty>True</AllowEmpty><controlname>objectselector</controlname><DialogWindowHeight>590</DialogWindowHeight><DialogWindowName>SelectionDialog</DialogWindowName><DialogWindowWidth>668</DialogWindowWidth><GlobalObjectSuffix>(global)</GlobalObjectSuffix><ItemsPerPage>25</ItemsPerPage><LocalizeItems>True</LocalizeItems><MaxDisplayedItems>25</MaxDisplayedItems><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ObjectType>cms.class</ObjectType><RemoveMultipleCommas>False</RemoveMultipleCommas><ResourcePrefix>objectselect</ResourcePrefix><ReturnColumnName>ClassName</ReturnColumnName><SelectionMode>1</SelectionMode><ValuesSeparator>;</ValuesSeparator></settings></field><field allowempty="true" column="ReturnColumnName" columnsize="100" columntype="text" displayinsimplemode="true" guid="5ebf33d9-7af2-462d-9ae3-4ae724ed6566" publicfield="false" visible="true"><properties><fieldcaption>Return column name</fieldcaption><fielddescription>{$documentation.property.returncolumnname$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DisplayNameFormat" columnsize="1000" columntype="text" displayinsimplemode="true" guid="6d5f6c56-25c4-414e-b924-9a74a989a772" publicfield="false" visible="true"><properties><fieldcaption>Display name format</fieldcaption><fielddescription>Specifies the name of the column that will be stored by the selector. If empty, the ID column is used. To ensure correct functionality, the column must be a unique identifier for the given object type.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="SelectedValue" columnsize="1000" columntype="text" displayinsimplemode="true" guid="910cf47f-2311-469e-bf90-ebb041e50c6f" publicfield="false" visible="true"><properties><fieldcaption>Selected value</fieldcaption><fielddescription>{$documentation.property.selectedvalue$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="SelectorLabel" columnsize="300" columntype="text" displayinsimplemode="true" guid="f6e588e9-cbdb-411a-a1de-8fb487404b34" publicfield="false" visible="true"><properties><fieldcaption>Selector label</fieldcaption><fielddescription>{$documentation.property.selectorlabel$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="ContextName" columnsize="100" columntype="text" displayinsimplemode="true" guid="d1d75bf7-632d-441d-8bb8-580e5c43d97e" publicfield="false" visible="true"><properties><defaultvalue>SelectorValue</defaultvalue><fieldcaption>Context name</fieldcaption><fielddescription>{$documentation.property.contextname$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="PostbackOnChange" columntype="boolean" displayinsimplemode="true" guid="e8b3e35a-c280-4f00-917a-2dddd1a2cfb6" publicfield="false" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>Postback on change</fieldcaption><fielddescription>{$documentation.property.postbackonchange$}</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="SelectorWhereCondition" columnsize="400" columntype="text" displayinsimplemode="true" guid="81e29d5c-6489-472f-a909-5a0e3adeccbf" publicfield="false" resolvedefaultvalue="False" visible="true"><properties><fieldcaption>Selector where condtion</fieldcaption><fielddescription>{$documentation.property.uniselectorwherecondition$}</fielddescription></properties><settings><AutoSize>False</AutoSize><controlname>macroeditor</controlname><EnablePositionMember>False</EnablePositionMember><EnableSections>False</EnableSections><EnableViewState>False</EnableViewState><Height>100</Height><Language>0</Language><ShowBookmarks>False</ShowBookmarks><ShowLineNumbers>False</ShowLineNumbers><ShowMacroSelector>False</ShowMacroSelector><SingleLineMode>False</SingleLineMode><SingleMacroMode>False</SingleMacroMode><SupportPasteImages>False</SupportPasteImages><Width>100%</Width></settings></field><field allowempty="true" column="SelectorOrderBy" columnsize="500" columntype="text" guid="c48adebd-8027-4489-8839-6dec9b6e23f1" publicfield="false" resolvedefaultvalue="False" visible="true"><properties><fieldcaption>Order by</fieldcaption><fielddescription>{$documentation.property.uniselectororderby$}</fielddescription></properties><settings><controlname>OrderBy</controlname></settings></field><category name="DropDownSettings"><properties><caption>Drop down settings</caption><visible>True</visible></properties></category><field allowempty="true" column="AllowEmpty" columntype="boolean" displayinsimplemode="true" guid="5d720369-c65e-496a-8387-dde801aad0d6" publicfield="false" visible="true"><properties><fieldcaption>Allow none</fieldcaption><fielddescription>{$documentation.property.uniselectorallowempty$}</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="AllowDefault" columntype="boolean" displayinsimplemode="true" guid="8135a068-6a0f-4899-8019-c45a655e7b83" publicfield="false" visible="true"><properties><fieldcaption>Allow default</fieldcaption><fielddescription>{$documentation.property.uniselectorallowdefault$}</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="AllowAll" columntype="boolean" displayinsimplemode="true" guid="d1586193-3c17-4f8c-9ab7-d4d481dd7f5f" publicfield="false" visible="true"><properties><fieldcaption>Allow all</fieldcaption><fielddescription>{$documentation.property.uniselectorallowall$}</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><category name="DialogSettings"><properties><caption>Dialog settings</caption><visible>True</visible></properties></category><field allowempty="true" column="SelectorGridName" columnsize="100" columntype="text" displayinsimplemode="true" guid="6e1db597-9cf8-4b84-b2c2-c1ce02a7e299" publicfield="false" visible="true"><properties><fieldcaption>Selector grid name</fieldcaption><fielddescription>Sets the path to the UniGrid XML definition file used by the list of selected items in Multiple selection mode.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DialogGridName" columnsize="400" columntype="text" displayinsimplemode="true" guid="edfc6e6f-0e7b-402d-8b87-45af2019e2e8" publicfield="false" visible="true"><properties><fieldcaption>Dialog grid name</fieldcaption><fielddescription>{$documentation.property.DialogGridName$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="NewItemPageUrl" columnsize="400" columntype="text" displayinsimplemode="true" guid="72407864-db1c-46b6-b699-773003827d60" publicfield="false" visible="true"><properties><fieldcaption>New item page URL</fieldcaption><fielddescription>{$documentation.property.NewItemPageUrl$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="EditItemPageUrl" columnsize="400" columntype="text" displayinsimplemode="true" guid="a37dbdeb-a0d9-4598-880a-fbc5e56cdfc0" publicfield="false" visible="true"><properties><fieldcaption>Edit item page URL</fieldcaption><fielddescription>{$documentation.property.editpageurl$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="SelectItemPageUrl" columnsize="400" columntype="text" displayinsimplemode="true" guid="1cb77352-41e4-42e9-8bfd-e154e96b5a5a" publicfield="false" visible="true"><properties><fieldcaption>Select item page URL</fieldcaption><fielddescription>{$documentation.property.selectitempageurl$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="VisibleCondition" columnsize="1000" columntype="text" displayinsimplemode="true" guid="8db8ba88-8dfd-47ec-9cf9-a5f0ede58d28" publicfield="false" visible="true"><properties><fieldcaption>Visible condition</fieldcaption><fielddescription>{$documentation.property.visiblecondition$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textareacontrol</controlname><FilterMode>False</FilterMode><IsTextArea>True</IsTextArea><Wrap>True</Wrap></settings></field><field allowempty="true" column="SelectorExtender" columnsize="200" columntype="text" displayinsimplemode="true" guid="9603f872-c799-4d53-a360-7677f2fae0fe" publicfield="false" visible="true"><properties><fieldcaption>Selector Extender</fieldcaption><fielddescription>{$documentation.property.uniselectorextender$}</fielddescription></properties><settings><ClassNameColumnName>SelectorExtenderClassName</ClassNameColumnName><controlname>assemblyclassselector</controlname><ShowClasses>True</ShowClasses><ShowEnumerations>False</ShowEnumerations><ShowInterfaces>False</ShowInterfaces></settings></field><field allowempty="true" column="SelectorExtenderClassName" columnsize="100" columntype="text" displayinsimplemode="true" guid="2630dbd6-07c8-4aaf-8132-faa97d0621fa" publicfield="false"><settings><controlname>textboxcontrol</controlname></settings></field><category name="HeaderActions"><properties><caption>Header actions</caption><visible>False</visible></properties></category><field allowempty="true" column="NewElement" columnsize="400" columntype="text" displayinsimplemode="true" guid="b397c1de-a932-489b-a8cb-fb38fc4cf99a" publicfield="false"><properties><fieldcaption>New element</fieldcaption><fielddescription>{$documentation.property.newelement$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><category name="Listing"><properties><visible>True</visible></properties></category><field allowempty="true" column="GridName" columnsize="200" columntype="text" displayinsimplemode="true" guid="d207fc9c-ac8e-4dff-8b4f-5a27024d1100" publicfield="false" visible="true"><properties><fieldcaption>Grid definition path</fieldcaption><fielddescription>{$documentation.property.gridname$}</fielddescription></properties><settings><AllowManage>True</AllowManage><controlname>filesystemselector</controlname><DefaultPath>App_Data/CMSModules</DefaultPath><NewTextFileExtension>xml</NewTextFileExtension><ShowFolders>False</ShowFolders></settings></field><field allowempty="true" column="WhereCondition" columnsize="600" columntype="text" displayinsimplemode="true" guid="4add8dfd-200a-47b8-8f05-289a9f37528c" publicfield="false" visible="true"><properties><fieldcaption>Where condition</fieldcaption><fielddescription>{$documentation.webpartproperties.where$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><AutoSize>False</AutoSize><controlname>macroeditor</controlname><EditorMode>1</EditorMode><EnablePositionMember>False</EnablePositionMember><EnableSections>False</EnableSections><EnableViewState>False</EnableViewState><FilterMode>False</FilterMode><Height>100</Height><IsTextArea>True</IsTextArea><Language>6</Language><ShowBookmarks>False</ShowBookmarks><ShowLineNumbers>False</ShowLineNumbers><ShowMacroSelector>False</ShowMacroSelector><SingleLineMode>False</SingleLineMode><SingleMacroMode>False</SingleMacroMode><SupportPasteImages>False</SupportPasteImages><Width>100</Width><Wrap>True</Wrap></settings></field><field allowempty="true" column="OrderBy" columnsize="200" columntype="text" displayinsimplemode="true" guid="72448c91-8bad-4533-ac1d-404471320e53" publicfield="false" visible="true"><properties><fieldcaption>Order by</fieldcaption><fielddescription>{$documentation.webpartproperties.orderby$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="EditActionURL" columnsize="400" columntype="text" displayinsimplemode="true" guid="1563e46e-3840-4665-b729-e7124f131b63" publicfield="false" visible="true"><properties><fieldcaption>Edit action URL</fieldcaption><fielddescription>{$documentation.property.editactionurl$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AfterDeleteScript" columnsize="400" columntype="text" displayinsimplemode="true" guid="2b6fa7c5-e84a-4883-94f4-02c546fef347" publicfield="false" visible="true"><properties><fieldcaption>After delete script</fieldcaption><fielddescription>{$documentation.property.afterdeletescript$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><AutoSize>False</AutoSize><controlname>macroeditor</controlname><EditorMode>0</EditorMode><EnablePositionMember>False</EnablePositionMember><EnableSections>False</EnableSections><EnableViewState>False</EnableViewState><FilterMode>False</FilterMode><Height>100</Height><Language>3</Language><ShowBookmarks>False</ShowBookmarks><ShowLineNumbers>False</ShowLineNumbers><ShowMacroSelector>False</ShowMacroSelector><SingleLineMode>False</SingleLineMode><SingleMacroMode>False</SingleMacroMode><SupportPasteImages>False</SupportPasteImages><Trim>False</Trim><Width>100</Width></settings></field><field allowempty="true" column="Text" columnsize="400" columntype="text" guid="005b3063-9594-4a78-8d75-7587ca19f5b5" publicfield="false" visible="true"><properties><fieldcaption>Text</fieldcaption><fielddescription>{$test.TextDisplayedAboveListing$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><AutoSize>False</AutoSize><controlname>macroeditor</controlname><EditorMode>0</EditorMode><EnablePositionMember>False</EnablePositionMember><EnableSections>False</EnableSections><EnableViewState>False</EnableViewState><FilterMode>False</FilterMode><Height>100</Height><Language>0</Language><ShowBookmarks>False</ShowBookmarks><ShowLineNumbers>False</ShowLineNumbers><ShowMacroSelector>False</ShowMacroSelector><SingleLineMode>False</SingleLineMode><SingleMacroMode>False</SingleMacroMode><SupportPasteImages>False</SupportPasteImages><Trim>False</Trim><Width>100</Width></settings></field><field allowempty="true" column="ZeroRowsText" columnsize="200" columntype="text" displayinsimplemode="true" guid="54d9c040-d13e-4af7-bca4-441b04d94474" publicfield="false" visible="true"><properties><fieldcaption>Zero rows text</fieldcaption><fielddescription>{$documentation.property.zerorowstext$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="GridExtender" columnsize="200" columntype="text" displayinsimplemode="true" guid="5437442f-ce1b-4295-8e75-9de9afd3bc78" publicfield="false" visible="true"><properties><fieldcaption>Grid extender</fieldcaption><fielddescription>{$documentation.property.gridextender$}</fielddescription></properties><settings><ClassNameColumnName>GridExtenderClassName</ClassNameColumnName><controlname>assemblyclassselector</controlname><ShowClasses>True</ShowClasses><ShowEnumerations>False</ShowEnumerations><ShowInterfaces>False</ShowInterfaces></settings></field><field allowempty="true" column="GridExtenderClassName" columnsize="100" columntype="text" displayinsimplemode="true" guid="5ef2d69d-23ac-4e02-9bc3-7e94836ea36c" publicfield="false"><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><category name="SettingKeys"><properties><caption>{$webpart.documentation.DisabledModuleInfo$}</caption><collapsedbydefault>true</collapsedbydefault><collapsible>true</collapsible><visible>True</visible></properties></category><field allowempty="true" column="SettingKeys" columnsize="200" columntype="text" displayinsimplemode="true" guid="7af87d6b-d6a1-4290-838b-4ee66850b066" publicfield="false" visible="true"><properties><fieldcaption>Checked setting keys</fieldcaption><fielddescription>{$webpart.settingkeys.description$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field></form>'
    WHERE [PageTemplateGUID] = '46008814-f594-420e-b391-1c6e5abb26bd'


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'ce1a65a0-80dc-4c53-b0e7-bdecf0aa8c02')
IF @classResourceID IS NOT NULL BEGIN

INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES ('CMS.MacroRule',
        '<form version="2"><field column="MacroRuleID" columntype="integer" guid="9c84abda-f7a5-4acb-b3b2-f53125e089dd" isPK="true" publicfield="false" system="true" visibility="none"><properties><fieldcaption>MacroRuleID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><category name="general.general"><properties><caption>{$general.general$}</caption><visible>True</visible></properties></category><field column="MacroRuleDisplayName" columnsize="500" columntype="text" guid="47507682-a9b6-46f9-9873-bfcaaff3ab99" publicfield="false" system="true" translatefield="true" visible="true"><properties><fieldcaption>Display name</fieldcaption></properties><settings><controlname>localizabletextbox</controlname><ValueIsContent>False</ValueIsContent></settings></field><field column="MacroRuleName" columnsize="200" columntype="text" guid="1dbfae33-b237-4ef9-bfc2-c7ccbf3a85f0" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$macros.macrorule.name$}</fieldcaption></properties><settings><controlname>codename</controlname></settings></field><field allowempty="true" column="MacroRuleDescription" columnsize="450" columntype="text" guid="f5100e73-9900-4788-9827-4c9fe54f56ab" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$general.description$}</fieldcaption></properties><settings><controlname>localizabletextarea</controlname><MaxLength>450</MaxLength><TextMode>MultiLine</TextMode></settings></field><field allowempty="true" column="MacroRuleAvailability" columntype="integer" guid="97ea06d6-75ad-4565-8712-4357ade5e12a" publicfield="false" system="true"><properties><defaultvalue>0</defaultvalue><fieldcaption>{$macros.macrorule.availability$}</fieldcaption><fielddescription>{$macros.macrorule.availability.description$}</fielddescription></properties><settings><AssemblyName>CMS.MacroEngine</AssemblyName><controlname>EnumSelector</controlname><DisplayType>0</DisplayType><Sort>False</Sort><TypeName>CMS.MacroEngine.MacroRuleAvailabilityEnum</TypeName><UseStringRepresentation>False</UseStringRepresentation></settings></field><field allowempty="true" column="MacroRuleEnabled" columntype="boolean" guid="c5f86a6e-0c2f-47de-a10a-459b06e48598" publicfield="false" system="true" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>{$general.enabled$}</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><category name="Rule_data"><properties><caption>Rule data</caption><visible>True</visible></properties></category><field column="MacroRuleText" columnsize="1000" columntype="text" guid="89a3fd80-b6a8-42bb-9634-f2d2dae25367" publicfield="false" system="true" translatefield="true" visible="true"><properties><fieldcaption>{$macros.macrorule.text$}</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextAreaControl</controlname><FilterMode>False</FilterMode><Wrap>True</Wrap></settings></field><field column="MacroRuleCondition" columntype="longtext" guid="8303608d-c1b0-4157-a719-79419d73cb02" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$macros.macrorule.condition$}</fieldcaption></properties><settings><AutoSize>False</AutoSize><controlname>macroeditor</controlname><EnablePositionMember>False</EnablePositionMember><EnableSections>False</EnableSections><EnableViewState>False</EnableViewState><Height>150</Height><Language>5</Language><ResolverName ismacro="true">{% UIContext.ResolverName |(identity)GlobalAdministrator|(hash)7dc979c71bf64c8af54e0aa45d790bb1ba4c46eb4e51d1614ca7a36ec2be2c23%}</ResolverName><ShowBookmarks>False</ShowBookmarks><ShowLineNumbers>False</ShowLineNumbers><ShowMacroSelector>False</ShowMacroSelector><SingleLineMode>False</SingleLineMode><SingleMacroMode>True</SingleMacroMode><SupportPasteImages>False</SupportPasteImages><Width>100</Width></settings></field><field allowempty="true" column="MacroRuleRequiredData" columnsize="2500" columntype="text" guid="2decf262-1140-4674-ad07-2a162b14848f" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$macros.macrorule.requireddata$}</fieldcaption><fielddescription>List of data items (separated with semicolon) which are required to be present in the resolving context for the rule to be displayed. If any of the specified data items is not present in the underlying resolver, the rule won''t be available within the list of rules.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field column="MacroRuleRequiresContext" columntype="boolean" guid="65a9edb3-5cda-47b6-b464-caab233b6be1" publicfield="false" system="true" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>{$macros.macrorule.requirescontext$}</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="MacroRuleParameters" columntype="longtext" guid="121f077f-1d84-4988-931f-0ab8647cc34c" publicfield="false" system="true" visibility="none"><properties><fieldcaption>MacroRuleParameters</fieldcaption></properties><settings><controlname>textareacontrol</controlname><IsTextArea>True</IsTextArea></settings></field><field allowempty="true" column="MacroRuleResourceName" columnsize="100" columntype="text" guid="3fac3f63-d64e-410a-9943-0726e5dbb036" publicfield="false" system="true" visibility="none"><properties><fieldcaption>Rule category</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="MacroRuleIsCustom" columntype="boolean" guid="c95f8b92-72f1-40c6-af9c-56eb32201f23" publicfield="false" system="true" visibility="none"><properties><defaultvalue>false</defaultvalue><fieldcaption>Macro rule is custom</fieldcaption></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field column="MacroRuleLastModified" columntype="datetime" guid="9cc32607-8d18-4275-802f-9c3c4c8b8f60" publicfield="false" system="true" visibility="none"><properties><fieldcaption>MacroRuleText</fieldcaption></properties><settings><controlname>textboxcontrol</controlname></settings></field><field column="MacroRuleGUID" columntype="guid" guid="d23ff3a0-659e-4c3a-ab95-80c8345a003b" publicfield="false" system="true" visibility="none"><properties><fieldcaption>MacroRuleID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field></form>',
        0);

END

END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '6fc0ed53-4054-4edf-9f01-a415fb7ff791')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = '226b13a3-97c2-4895-99e4-39ea6247a399')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'ce1a65a0-80dc-4c53-b0e7-bdecf0aa8c02')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><DisplayBreadcrumbs>False</DisplayBreadcrumbs><EditInDialog>False</EditInDialog><GridExtender>App_Code</GridExtender><includejquery>False</includejquery><OpenInDialog>False</OpenInDialog><WhereCondition>[MacroRuleResourceName] = ''cms.formengine''</WhereCondition></Data>'
    WHERE [ElementGUID] = 'c41439ab-0e59-4676-a67f-a7a91afc5878'

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><DisplayBreadcrumbs>False</DisplayBreadcrumbs><EditInDialog>False</EditInDialog><ExtenderClassName>MacroRuleExtender</ExtenderClassName><GridExtender>App_Code</GridExtender><GridName>~/CMSModules/OnlineMarketing/Controls/UI/Grids/MacroRule.xml</GridName><includejquery>False</includejquery><OpenInDialog>False</OpenInDialog><WhereCondition>[MacroRuleResourceName] IS NULL
OR
[MacroRuleResourceName] = ''''</WhereCondition></Data>'
    WHERE [ElementGUID] = 'c9669912-5a93-409a-8f28-317702a23b4c'


END

END

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'c41439ab-0e59-4676-a67f-a7a91afc5878')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = 'bc86a286-996d-4445-ac8a-68ed45d115b3')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'ce1a65a0-80dc-4c53-b0e7-bdecf0aa8c02')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><DefaultFieldLayout>default</DefaultFieldLayout><DefaultFormLayout>Divs</DefaultFormLayout><DisplayBreadcrumbs>True</DisplayBreadcrumbs><EditExtender>App_Code</EditExtender><ExtenderClassName>MacroRuleEditExtender</ExtenderClassName><includejquery>False</includejquery><informationtext>{$macros.macrorule.ruletextinfo$}</informationtext><parentcolumn>MacroRuleResourceName</parentcolumn><parentobjectid>cms.formengine</parentobjectid></Data>'
    WHERE [ElementGUID] = '07ca8da0-13ae-4bd1-b578-b85af3fd98af'


END

END

END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'c9669912-5a93-409a-8f28-317702a23b4c')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = 'bc86a286-996d-4445-ac8a-68ed45d115b3')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'ce1a65a0-80dc-4c53-b0e7-bdecf0aa8c02')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><AlternativeFormName>GlobalRule</AlternativeFormName><DefaultFieldLayout>default</DefaultFieldLayout><DefaultFormLayout>Divs</DefaultFormLayout><DisplayBreadcrumbs>True</DisplayBreadcrumbs><EditExtender>App_Code</EditExtender><ExtenderClassName>MacroRuleEditExtender</ExtenderClassName><includejquery>False</includejquery><informationtext>{$macros.macrorule.ruletextinfo$}</informationtext></Data>'
    WHERE [ElementGUID] = '641b5f4d-42df-4a6e-9984-01dbb5ffb225'


END

END

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'f216f215-272c-474c-ba88-38d0fec60550')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = 'bc86a286-996d-4445-ac8a-68ed45d115b3')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'ce1a65a0-80dc-4c53-b0e7-bdecf0aa8c02')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><AlternativeFormName>GlobalRule</AlternativeFormName><DefaultFieldLayout>default</DefaultFieldLayout><DefaultFormLayout>Divs</DefaultFormLayout><DisplayBreadcrumbs>False</DisplayBreadcrumbs><EditExtender>App_Code</EditExtender><ExtenderClassName>MacroRuleEditExtender</ExtenderClassName><includejquery>False</includejquery><informationtext>{$macros.macrorule.ruletextinfo$}</informationtext></Data>'
    WHERE [ElementGUID] = '0560c9e7-f884-4ebf-90c7-84b797aec777'


END

END

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN


UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 0

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '60b340ee-4b74-4b6b-9f10-e79cf378a406'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '35e625d4-dc0e-46e0-871e-11959b1a6520'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '0a6597a1-c899-45a7-8d94-27ef8b67b620'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'fdf51cf1-0b21-4aac-b98b-71125944377f'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'f01d1fdc-954b-41f7-affb-073993039369'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '8a3e7552-ad5f-41eb-a3b6-1fb74eb4fda3'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'a99cc193-134d-4efb-8f6f-3986b9bab17e'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'a2f1e889-d12f-482f-a1fd-3ea4834e6bc4'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'f15ab50d-c0e2-4e39-a406-4b8493dd0801'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '8c4d35d2-9a94-4f4d-8362-148f468892d2'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '6921ea30-0cd0-4ab3-b5a9-5582fa499904'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '78f662de-147b-4835-857c-cf28f3292795'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'bf6e3304-2974-4e6a-8aa0-734e62c4fdad'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'bf285ca9-ba59-4085-b6ef-f97e69e335ac'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 0
    WHERE [MacroRuleGUID] = 'd310cdb3-0862-4486-9a9f-abdc4df7201a'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '1c250b40-34be-4994-927f-6c1fb74fa05b'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '91b7d652-9db9-415b-94ef-9d7e5d93753d'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '84be6fde-8146-4822-93d3-f987b8e1188f'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'f10bd718-a8ed-461f-95ac-a9554e7ee7e4'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'a1ec3d51-4dc9-4300-88fc-6e03eff3f101'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'ff3d6a61-293d-4ac0-aa52-688e2e214317'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '1699e77d-251f-472f-8058-f4432cf05fe5'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'a88bf9fa-756b-4d3e-8e23-134ef09de08c'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '856da497-dfa0-4f68-af9d-3ec72b25a86e'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '037d4ff9-e441-4a3d-b541-f6be974a46d9'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '4f43752e-e4bc-465d-89f0-59c45b432dab'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '9dddb501-940f-46a2-9b7a-ead8ef7962bc'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'd8d47dc7-9e40-4095-876b-227460f09e76'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'c4a4c554-831d-4974-902a-3b335d2767ba'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '02b12520-8fd5-49a8-8b94-ae56c3436eff'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '52bebbc6-9e68-42ce-87b0-0b52cb29bacd'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '0bade63b-fe26-4d54-aa6b-7c6460fede20'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'cde29a5b-2405-4c52-9fc0-fc1b931f2672'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 0
    WHERE [MacroRuleGUID] = '1a3125f6-59e6-4093-af82-5df147c8f91a'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '4c200d26-d118-4567-aa98-e4e1fed176a9'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '94aac7df-1dd6-496c-92fd-91aeba1b6e7d'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'da07fa9d-2ffd-4253-836e-6ffce3270041'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '0689a747-2874-4c09-8ee1-32794e030ad5'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'e775b355-70ad-42fe-9a75-ecd912221157'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleRequiredData] = NULL,
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'b0a36af7-5dc2-4b6c-bc27-2f20c97b6be9'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'ce39a78c-339c-4be8-b355-70bd42eec51a'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '625c2375-5644-49cb-ae6d-a82db4f60934'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 0
    WHERE [MacroRuleGUID] = '34505fd2-a8ff-4e8b-bed5-8b7577afee07'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '5246ecce-2ca4-408a-b183-38d2fed172b6'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleRequiredData] = NULL,
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '5440e355-9ef0-4f56-b82b-6c5a6a06475f'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '9929ac22-f9a3-4e62-b99d-96973ef24fa9'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '3375a24f-94d4-444f-86ef-3199521fbda2'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '8dd2be4a-64b0-46d0-ba65-9f9600ad1849'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '9e0fbd4f-ea4f-42c0-a137-d2b9cec97f75'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '7397e29b-5c5b-49f2-9cf8-c7e93c4d67b8'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '6678ed8a-fbfb-43af-982a-7c80d54a5531'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'bce49790-f642-4039-8748-e55b2b95964d'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '6df797d9-c66d-43c9-9623-405670dfb5fa'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'c6bc81f7-dd28-41c5-b918-087532b1d14f'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'f3aee60a-2c75-4630-9876-05732f648670'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'bcdd932e-7007-44b6-ad2c-433280503ef4'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'b1086b7a-0db1-4b16-a1ad-857f06804fe0'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '5960f7ed-9ed6-43dc-a16a-8dc9761226b4'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 0
    WHERE [MacroRuleGUID] = 'b96b39c9-e3e2-4fd5-a6c4-0e9954f2b33f'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '5fa08104-9eeb-47db-8ace-906d445eac86'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '82dfb916-ee65-4bde-8ab5-04bbbc5c3518'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 0
    WHERE [MacroRuleGUID] = '139f90eb-bd99-4817-998b-7525300198f6'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '44d701f7-5536-44f5-9089-06d934d71124'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '7d3e93dc-d4d4-4450-b9e7-cf8f559daf2b'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = '8a9b22df-aea7-4e3f-8bd7-92c5e88cb9da'

UPDATE [CMS_MacroRule] SET 
        [MacroRuleAvailability] = 2
    WHERE [MacroRuleGUID] = 'f5205f87-541d-4829-b301-c86c2a3ef805'


END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

UPDATE [CMS_FormUserControl] SET 
        [UserControlParameters] = '<form version="2"><field allowempty="true" column="MaxWidth" columntype="integer" displayinsimplemode="true" guid="6381af71-905b-4fb6-8e0c-9fc5cf4ade0a" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>600</defaultvalue><fieldcaption>Maximum width (pixels)</fieldcaption><fielddescription>Sets the maximum width of conditional builder.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings><rules><rule>{%Rule("Value &gt;= 0", "&lt;rules&gt;&lt;r pos=\"0\" par=\"\" op=\"and\" n=\"MinValue\" &gt;&lt;p n=\"minvalue\"&gt;&lt;t&gt;0&lt;/t&gt;&lt;v&gt;0&lt;/v&gt;&lt;r&gt;false&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;integer&lt;/vt&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;")%}</rule></rules></field><field allowempty="true" column="ResolverName" columnsize="100" columntype="text" guid="8e4ba147-5467-491a-9f54-265c9b11f66b" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Resolver name</fieldcaption><fielddescription>Macro resolver name which should be used for macro editor (if no name is given, default context resolver is used).</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="MacroRuleAvailability" columntype="integer" guid="c798bbe3-7801-4d86-ae00-5fc1e5e5a3f9" publicfield="false" resolvedefaultvalue="False" visible="true"><properties><fieldcaption>{$macros.macrorule.availability$}</fieldcaption><fielddescription>{$macros.macrorule.availability.conditionbuilder.description$}</fielddescription></properties><settings><AssemblyName>CMS.MacroEngine</AssemblyName><controlname>EnumSelector</controlname><DisplayType>0</DisplayType><Sort>False</Sort><TypeName>CMS.MacroEngine.MacroRuleAvailabilityEnum</TypeName><UseStringRepresentation>False</UseStringRepresentation></settings></field><field allowempty="true" column="RuleCategoryNames" columnsize="200" columntype="text" guid="66b1e439-eb6b-48ea-a5fd-ec5bfe6b7326" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Rule category name(s)</fieldcaption><fielddescription>Name of the macro rule category(ies) which should be displayed in Rule designer. Items should be separated by semicolon.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="DisplayRuleType" columntype="integer" guid="a868490d-41c1-4037-930b-a06238999e88" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Display rules</fieldcaption><fielddescription>Selects the types of macro rules that should be displayed.</fielddescription></properties><settings><controlname>dropdownlistcontrol</controlname><EditText>False</EditText><Options>0;All rules
1;Only rules which do not require request context
2;Only rules which require request context</Options></settings></field><field allowempty="true" column="ShowGlobalRules" columntype="boolean" guid="d470e64f-9184-4a59-8bbc-5db156fd0516" publicfield="false" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>Show global rules</fieldcaption><fielddescription>Determines whether the global rules are shown among with the specific rules defined in the ''Rule category name(s)'' property.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="AddDataMacroBrackets" columntype="boolean" guid="544a3893-1a5d-43ca-b562-0d277a02f2eb" publicfield="false" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>Add data macro brackets</fieldcaption><fielddescription>Indicates if value should be signed and wrapped in data macro brackets.</fielddescription></properties><settings><controlname>CheckBoxControl</controlname></settings></field><field allowempty="true" column="DefaultConditionText" columnsize="500" columntype="text" guid="4ab4eecd-0d76-49eb-bb67-e0a2ff579160" publicfield="false" visible="true"><properties><fieldcaption>Default condition text</fieldcaption><fielddescription>Text which is displayed by default when there is no rule defined.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field column="ShowAutoCompletionAbove" columntype="boolean" guid="e80e1534-7311-40aa-94c8-dde68072e307" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>Show auto completion above</fieldcaption><fielddescription>If enabled, the macro auto completion list will be opened above the editor, otherwise it will be below it (the default position is below).</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="SingleLineMode" columntype="boolean" guid="63b62bb5-b405-4420-a26a-0fe105276c55" publicfield="false" resolvedefaultvalue="False" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>Single line mode</fieldcaption><fielddescription>Indicates whether macro is multiline.</fielddescription></properties><settings><controlname>CheckBoxControl</controlname></settings></field></form>'
    WHERE [UserControlGUID] = 'ba8f3ba6-14ab-4558-b891-1ccd388b489f' AND [UserControlParentID] IS NULL


END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'f23e944e-1a51-46f0-8a2e-75bcfc2a70ea')
IF @classResourceID IS NOT NULL BEGIN

INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES ('OM.ABTest',
        '<form version="2"><field column="ABTestID" columntype="integer" guid="86655528-dafb-45db-83d4-dd0b75569349" isPK="true" isunique="true" publicfield="false" system="true" visibility="none" /><category name="general.general"><properties><caption>{$general.general$}</caption><visible>True</visible></properties></category><field column="ABTestDisplayName" columnsize="100" columntype="text" guid="19a599a7-6256-476a-8d37-9faf5ac33dbf" publicfield="false" system="true" translatefield="true" visibility="none" visible="true"><properties><fieldcaption>{$general.displayname$}</fieldcaption><fielddescription>{$abtesting.displayname.description$}</fielddescription></properties><settings><controlname>localizabletextbox</controlname><ValueIsContent>False</ValueIsContent></settings></field><field column="ABTestName" columnsize="50" columntype="text" guid="1d1b3ae7-1a63-48f7-ae8c-5984d7063c8f" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$general.codename$}</fieldcaption><fielddescription>{$abtesting.name.description$}</fielddescription><visiblemacro ismacro="true">{%false%}</visiblemacro></properties><settings><controlname>codename</controlname></settings></field><field allowempty="true" column="ABTestDescription" columntype="longtext" guid="559f6e45-2cc3-4326-9f5c-f66cb4e45cce" publicfield="false" system="true" translatefield="true" visibility="none" visible="true"><properties><fieldcaption>{$general.description$}</fieldcaption><fielddescription>{$abtesting.description.description$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textareacontrol</controlname><Rows>4</Rows><WatermarkText ismacro="true">{%  if(EditedObject.ID == 0)
  {
    GetResourceString(&quot;abtesting.description.watermark&quot;)
  } |(identity)GlobalAdministrator|(hash)c774195b3fb997c956ee3a4c39d2b7d1ad5f4dd40bf53c5273986a9e661fc547%}</WatermarkText><Wrap>True</Wrap></settings></field><field column="ABTestOriginalPage" columnsize="450" columntype="text" guid="68d756ae-ca0e-4cc0-b903-f79d10f2f701" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$abtesting.originalpage$}</fieldcaption><fielddescription>{$abtesting.originalpage.description$}</fielddescription></properties><settings><AllowSetPermissions>False</AllowSetPermissions><controlname>selectsinglepath</controlname><SelectablePageTypes>1</SelectablePageTypes><SinglePathMode>True</SinglePathMode></settings></field><field allowempty="true" column="ABTestCulture" columnsize="50" columntype="text" guid="0e01436b-dc1c-4bc0-a57b-a2e6a55dfac1" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$abtest.culture$}</fieldcaption><fielddescription>{$abtesting.culture.description$}</fielddescription></properties><settings><AllowAll ismacro="true">{% !CurrentSite.SiteIsContentOnly |(identity)GlobalAdministrator|(hash)b9a229331043dc93e3820b7450367409b2647e578aa351ce43f1274c61406814%}</AllowAll><AllowDefault>False</AllowDefault><AllowEmpty>False</AllowEmpty><AllRecordValue>{%""%}</AllRecordValue><controlname>SiteCultureSelector</controlname></settings></field><field allowempty="true" column="ABTestOpenFrom" columntype="datetime" guid="fab79e13-0177-4616-bd9b-a1a48ba32fc1" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$general.start$}</fieldcaption><fielddescription>{$abtesting.openfrom.description$}</fielddescription></properties><settings><CheckRange>True</CheckRange><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>True</EditTime><TimeZoneType>inherit</TimeZoneType></settings></field><field allowempty="true" column="ABTestOpenTo" columntype="datetime" guid="a99b03df-10da-4804-ae9c-dd8924733318" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$general.end$}</fieldcaption><fielddescription>{$abtesting.opento.description$}</fielddescription></properties><settings><CheckRange>True</CheckRange><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>True</EditTime><TimeZoneType>inherit</TimeZoneType></settings></field><category name="abtesting.conversionsandtraffic"><properties><caption>{$abtesting.conversionsandtraffic$}</caption><visible>True</visible></properties></category><field allowempty="true" column="ABTestConversions" columntype="longtext" guid="374f0b4f-20ba-4b45-945c-728ae3f13005" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$abtesting.settings.conversions$}</fieldcaption><fielddescription>{$abtesting.goals.description$}</fielddescription></properties><settings><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix><AllowAll>False</AllowAll><AllowDefault>False</AllowDefault><AllowEditTextBox>False</AllowEditTextBox><AllowEmpty>False</AllowEmpty><controlname>uni_selector</controlname><DialogWindowHeight>590</DialogWindowHeight><DialogWindowName>SelectionDialog</DialogWindowName><EditDialogWindowHeight>700</EditDialogWindowHeight><EditDialogWindowWidth>1000</EditDialogWindowWidth><EditWindowName>EditWindow</EditWindowName><EncodeOutput>True</EncodeOutput><GlobalObjectSuffix>(global)</GlobalObjectSuffix><ItemsPerPage>25</ItemsPerPage><LocalizeItems>True</LocalizeItems><MaxDisplayedItems>25</MaxDisplayedItems><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ObjectSiteName>#currentsite</ObjectSiteName><ObjectType>analytics.conversion</ObjectType><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><ReturnColumnType>id</ReturnColumnType><SelectionMode>3</SelectionMode><UseAutocomplete>False</UseAutocomplete><ValuesSeparator>;</ValuesSeparator></settings></field><field allowempty="true" column="ABTestVisitorTargeting" columntype="longtext" guid="34a3dd50-b49b-45a7-b08e-0fc1dfc179df" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$abtesting.visitortargeting$}</fieldcaption><fielddescription>{$abtesting.visitortargeting.description$}</fielddescription></properties><settings><AddDataMacroBrackets>True</AddDataMacroBrackets><controlname>ConditionBuilder</controlname><DefaultConditionText>(all visitors)</DefaultConditionText><DisplayRuleType>0</DisplayRuleType><MacroRuleAvailability ismacro="true">{% if(CurrentSite.SiteIsContentOnly) {
 return 1
}else{
 return 0
}
|(identity)GlobalAdministrator|(hash)74395b98b9631dc5a31d42b173f5a0f7c5463616af355370714790b84407cebd%}</MacroRuleAvailability><MaxWidth>600</MaxWidth><ResolverName>ContactResolver</ResolverName><RuleCategoryNames>cms.onlinemarketing</RuleCategoryNames><ShowAutoCompletionAbove>True</ShowAutoCompletionAbove><ShowGlobalRules>True</ShowGlobalRules><SingleLineMode>True</SingleLineMode></settings></field><field column="ABTestIncludedTraffic" columntype="integer" guid="5791e2c4-9985-43fa-a362-9edaf7025b77" publicfield="false" system="true" visible="true"><properties><contentafter>&lt;span class="form-control-text"&gt;%&lt;/span&gt;</contentafter><defaultvalue>100</defaultvalue><fieldcaption>{$abtesting.includedtraffic$}</fieldcaption><fielddescription>{$abtesting.includedtraffic.description$}</fielddescription><validationerrormessage>{$general.percentagebetween$}</validationerrormessage></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings><rules><rule>{%Rule("Value &gt;= 0", "&lt;rules&gt;&lt;r pos=\"0\" par=\"\" op=\"and\" n=\"MinValue\" &gt;&lt;p n=\"minvalue\"&gt;&lt;t&gt;0&lt;/t&gt;&lt;v&gt;0&lt;/v&gt;&lt;r&gt;false&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;integer&lt;/vt&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;")%}</rule><rule>{%Rule("Value &lt;= 100", "&lt;rules&gt;&lt;r pos=\"0\" par=\"\" op=\"and\" n=\"MaxValue\" &gt;&lt;p n=\"maxvalue\"&gt;&lt;t&gt;100&lt;/t&gt;&lt;v&gt;100&lt;/v&gt;&lt;r&gt;false&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;integer&lt;/vt&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;")%}</rule></rules></field><field column="ABTestSiteID" columntype="integer" guid="b8c25b1d-6ab4-46e7-994a-5d844cbb5625" publicfield="false" system="true" visibility="none"><properties><fieldcaption>ABTestDisplayName</fieldcaption></properties><settings><controlname>localizabletextbox</controlname></settings></field><field allowempty="true" column="ABTestWinnerGUID" columntype="guid" guid="f21d2a3f-019c-443f-b91e-a2fc9218efab" publicfield="false" system="true" /><field column="ABTestGUID" columntype="guid" guid="eab35f06-804c-4193-8830-dee55dae8deb" publicfield="false" spellcheck="false" system="true" visibility="none"><properties><fieldcaption>AB test target cinversuin type</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field column="ABTestLastModified" columntype="datetime" guid="80955796-c133-49d8-a7eb-1f9b10399e25" publicfield="false" system="true" /></form>',
        0);

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @formClassID int;
SET @formClassID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassGUID] = 'c59a88e0-c6e5-4537-b35d-0cc3d2b294c9')
IF @formClassID IS NOT NULL BEGIN

INSERT [CMS_AlternativeForm] ([FormDisplayName], [FormName], [FormClassID], [FormDefinition], [FormLayout], [FormGUID], [FormLastModified], [FormCoupledClassID], [FormHideNewParentFields], [FormLayoutType], [FormVersionGUID], [FormCustomizedColumns], [FormIsCustom])
 VALUES ('GlobalRule', 'GlobalRule', @formClassID, '<form version="2"><field column="MacroRuleID" guid="9c84abda-f7a5-4acb-b3b2-f53125e089dd" /><field column="MacroRuleDisplayName" guid="47507682-a9b6-46f9-9873-bfcaaff3ab99" /><field column="MacroRuleName" guid="1dbfae33-b237-4ef9-bfc2-c7ccbf3a85f0" /><field column="MacroRuleDescription" guid="f5100e73-9900-4788-9827-4c9fe54f56ab" /><field column="MacroRuleAvailability" guid="97ea06d6-75ad-4565-8712-4357ade5e12a" visible="true" /><field column="MacroRuleEnabled" guid="c5f86a6e-0c2f-47de-a10a-459b06e48598" /><field column="MacroRuleText" guid="89a3fd80-b6a8-42bb-9634-f2d2dae25367" /><field column="MacroRuleCondition" guid="8303608d-c1b0-4157-a719-79419d73cb02" /><field column="MacroRuleRequiredData" guid="2decf262-1140-4674-ad07-2a162b14848f" /><field column="MacroRuleRequiresContext" guid="65a9edb3-5cda-47b6-b464-caab233b6be1" /><field column="MacroRuleParameters" guid="121f077f-1d84-4988-931f-0ab8647cc34c" /><field column="MacroRuleResourceName" guid="3fac3f63-d64e-410a-9943-0726e5dbb036" /><field column="MacroRuleIsCustom" guid="c95f8b92-72f1-40c6-af9c-56eb32201f23" /><field column="MacroRuleLastModified" guid="9cc32607-8d18-4275-802f-9c3c4c8b8f60" /><field column="MacroRuleGUID" guid="d23ff3a0-659e-4c3a-ab95-80c8345a003b" /></form>', NULL, '3fea5dd6-d488-487e-8b8c-e8164767860e', getDate(), NULL, 0, NULL, '', '', 0)


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @formClassID int;
SET @formClassID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassGUID] = 'c59a88e0-c6e5-4537-b35d-0cc3d2b294c9')
IF @formClassID IS NOT NULL BEGIN

DECLARE @className nvarchar(100);
SET @className = (SELECT TOP 1 [ClassName] FROM [CMS_Class] INNER JOIN [CMS_AlternativeForm] ON [ClassID] = [FormClassID] WHERE [FormGUID] = 'e7e5aaee-8e2f-4a73-bd72-be5c75186646')
INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES (@className + '.OnlineMarketingRule',
        '<form version="2"><field column="MacroRuleID" guid="9c84abda-f7a5-4acb-b3b2-f53125e089dd" /><field column="MacroRuleDisplayName" guid="47507682-a9b6-46f9-9873-bfcaaff3ab99" /><field column="MacroRuleName" guid="1dbfae33-b237-4ef9-bfc2-c7ccbf3a85f0" /><field column="MacroRuleDescription" guid="f5100e73-9900-4788-9827-4c9fe54f56ab" /><field column="MacroRuleAvailability" guid="97ea06d6-75ad-4565-8712-4357ade5e12a" visible="true" /><field column="MacroRuleEnabled" guid="c5f86a6e-0c2f-47de-a10a-459b06e48598" /><field column="MacroRuleText" guid="89a3fd80-b6a8-42bb-9634-f2d2dae25367" /><field column="MacroRuleCondition" guid="8303608d-c1b0-4157-a719-79419d73cb02" /><field column="MacroRuleRequiredData" guid="2decf262-1140-4674-ad07-2a162b14848f" /><field column="MacroRuleRequiresContext" guid="65a9edb3-5cda-47b6-b464-caab233b6be1" /><field column="MacroRuleParameters" guid="121f077f-1d84-4988-931f-0ab8647cc34c" /><field column="MacroRuleResourceName" guid="3fac3f63-d64e-410a-9943-0726e5dbb036"><settings><AutoCompleteEnableCaching /><AutoCompleteFirstRowSelected /><AutoCompleteShowOnlyCurrentWordInCompletionListItem /><controlname /><FilterMode /><Trim /></settings><properties><defaultvalue>CMS.OnlineMarketing</defaultvalue></properties></field><field column="MacroRuleIsCustom" guid="c95f8b92-72f1-40c6-af9c-56eb32201f23" /><field column="MacroRuleLastModified" guid="9cc32607-8d18-4275-802f-9c3c4c8b8f60" /><field column="MacroRuleGUID" guid="d23ff3a0-659e-4c3a-ab95-80c8345a003b" /></form>',
        1);

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @formClassID int;
SET @formClassID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassGUID] = '71a1d617-6161-4ad6-aee2-be5756048223')
IF @formClassID IS NOT NULL BEGIN

INSERT [CMS_AlternativeForm] ([FormDisplayName], [FormName], [FormClassID], [FormDefinition], [FormLayout], [FormGUID], [FormLastModified], [FormCoupledClassID], [FormHideNewParentFields], [FormLayoutType], [FormCustomizedColumns], [FormIsCustom])
VALUES ('Insert', 'Insert', @formClassID, '<form version="2"><field column="ABTestID" guid="86655528-dafb-45db-83d4-dd0b75569349" /><field column="ABTestDisplayName" guid="19a599a7-6256-476a-8d37-9faf5ac33dbf" /><field column="ABTestName" guid="1d1b3ae7-1a63-48f7-ae8c-5984d7063c8f" /><field column="ABTestDescription" guid="559f6e45-2cc3-4326-9f5c-f66cb4e45cce" /><field column="ABTestOriginalPage" guid="68d756ae-ca0e-4cc0-b903-f79d10f2f701" /><field column="ABTestCulture" guid="0e01436b-dc1c-4bc0-a57b-a2e6a55dfac1" visible="" /><field column="ABTestOpenFrom" columnprecision="7" guid="fab79e13-0177-4616-bd9b-a1a48ba32fc1" visible="" /><field column="ABTestOpenTo" columnprecision="7" guid="a99b03df-10da-4804-ae9c-dd8924733318" visible="" /><category name="abtesting.conversionsandtraffic"><properties><visible>False</visible></properties></category><field column="ABTestConversions" guid="374f0b4f-20ba-4b45-945c-728ae3f13005" visible="" /><field column="ABTestVisitorTargeting" guid="34a3dd50-b49b-45a7-b08e-0fc1dfc179df" visible="" /><field column="ABTestIncludedTraffic" guid="5791e2c4-9985-43fa-a362-9edaf7025b77" visible="" /><field column="ABTestSiteID" guid="b8c25b1d-6ab4-46e7-994a-5d844cbb5625" /><field column="ABTestWinnerGUID" guid="f21d2a3f-019c-443f-b91e-a2fc9218efab" /><field column="ABTestGUID" guid="eab35f06-804c-4193-8830-dee55dae8deb" /><field column="ABTestLastModified" guid="80955796-c133-49d8-a7eb-1f9b10399e25" /></form>', NULL, 'cdced093-0cc1-4116-a311-e7ddcac0c7b0', getDate(), NULL, 1, NULL, '', 0)

DELETE FROM [CMS_AlternativeForm] WHERE [FormGUID] = '96f2eb15-1d01-4935-b8d9-257c11aa6806'

END

END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '3f457537-ad36-4a89-a443-089ad5bc9cdb')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'f23e944e-1a51-46f0-8a2e-75bcfc2a70ea')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementVisibilityCondition] = '{%!CurrentSite.SiteIsContentOnly|(identity)GlobalAdministrator|(hash)fa60732c2cb1eb14933d5245a0786e9cbffcb243a57614ddf0c131ecf6c2e43e%}'
    WHERE [ElementGUID] = '281a6791-6179-4a92-a86d-2fd410a19b45'


END

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '83ff58cf-d7ed-4567-a68c-439daf7e85cf')
IF @classResourceID IS NOT NULL BEGIN

INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES ('Temp.PageBuilderWidgets',
        '<form version="2"><field column="PageBuilderWidgetsID" columntype="integer" guid="355398b4-0973-4d28-8c3b-fb89b14b63cf" isPK="true" publicfield="false" system="true"><properties><fieldcaption>PageBuilderWidgetsID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field allowempty="true" column="PageBuilderWidgetsConfiguration" columntype="longtext" guid="4d69dcdf-34fa-4b61-976f-67197c9a5af8" publicfield="false" system="true"><settings><controlname>LabelControl</controlname><ResolveMacros>True</ResolveMacros></settings></field><field allowempty="true" column="PageBuilderTemplateConfiguration" columntype="longtext" guid="bc74f698-52a9-47b7-a469-a0425367717f" publicfield="false" system="true" /><field column="PageBuilderWidgetsGuid" columntype="guid" guid="05680a9f-20e5-4dd2-a5c6-75316bb711b1" publicfield="false" system="true"><properties><fieldcaption>GUID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="PageBuilderWidgetsLastModified" columnprecision="7" columntype="datetime" guid="3962d803-b795-4475-96ad-5936e24405bb" publicfield="false" system="true"><properties><fieldcaption>Last modified</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field></form>',
        0);
END
END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @resourceID int;
SET @resourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '98c6ee00-230a-4207-a6d3-03677b83b245')
IF @resourceID IS NOT NULL BEGIN

INSERT [CMS_Permission] ([PermissionDisplayName], [PermissionName], [ClassID], [ResourceID], [PermissionGUID], [PermissionLastModified], [PermissionDescription], [PermissionDisplayInMatrix], [PermissionOrder], [PermissionEditableByGlobalAdmin])
 VALUES ('Manage page templates (MVC)', 'ManagePageTemplates', NULL, @resourceID, '58a26172-486a-4282-8039-855309a80112', getDate(), '{$content.permission.managepagetemplates.description$}', 1, 11, 0)


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 11
BEGIN

DECLARE @userControlResourceID int;
SET @userControlResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '385e528a-e013-4a84-bad9-d5956408741a')
IF @userControlResourceID IS NOT NULL BEGIN

UPDATE [CMS_FormUserControl] SET 
        [UserControlParameters] = '<form version="2"><field column="FieldsDataType" columnsize="200" columntype="text" guid="afc0323b-5fbc-429a-9afb-5547bad90c30" publicfield="false" resolvedefaultvalue="False" visible="true"><properties><explanationtext>{$formfieldselector.fieldsdatatype.explanation$}</explanationtext><fieldcaption>{$formfieldselector.fieldsdatatype.caption$}</fieldcaption><fielddescription>{$formfieldselector.fieldsdatatype.description$}</fielddescription></properties><settings><controlname>DropDownListControl</controlname><DisplayActualValueAsItem>False</DisplayActualValueAsItem><EditText>False</EditText><Options>all;All
text;Text
longtext;LongText
integer;Integer
longinteger;LongInteger
double;Double
datetime;DateTime
boolean;Boolean
file;File
guid;Guid
binary;Binary
decimal;Decimal
timespan;Timespan
date;Date</Options><SortItems>False</SortItems></settings></field></form>'
    WHERE [UserControlGUID] = 'd4bc018e-e9b9-4784-a211-a91359ea6cd5' AND [UserControlParentID] IS NULL


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '3f457537-ad36-4a89-a443-089ad5bc9cdb')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'f23e944e-1a51-46f0-8a2e-75bcfc2a70ea')
IF @elementResourceID IS NOT NULL BEGIN

DECLARE @newElementGUID uniqueidentifier = '60d1cc5f-1232-4012-a2b6-46160c630ac8';

INSERT [CMS_UIElement] ([ElementDisplayName], [ElementName], [ElementCaption], [ElementTargetURL], [ElementResourceID], [ElementParentID], [ElementChildCount], [ElementOrder], [ElementLevel], [ElementIDPath], [ElementIconPath], [ElementIsCustom], [ElementLastModified], [ElementGUID], [ElementSize], [ElementDescription], [ElementFromVersion], [ElementPageTemplateID], [ElementType], [ElementProperties], [ElementIsMenu], [ElementFeature], [ElementIconClass], [ElementIsGlobalApplication], [ElementCheckModuleReadPermission], [ElementAccessCondition], [ElementVisibilityCondition], [ElementRequiresGlobalAdminPriviligeLevel])
 VALUES ('{$general.overview$} (MVC)', 'OverviewMVC', '{$general.overview$}', '~/CMSModules/OnlineMarketing/Pages/Content/ABTesting/ABTest/OverviewMVC.aspx', @elementResourceID, @elementParentID, 0, 2, 5, '', '', 0, getDate(), @newElementGUID, 0, '', '12.0', NULL, 'Url', '<Data><DisplayBreadcrumbs>False</DisplayBreadcrumbs><includejquery>false</includejquery></Data>', 0, '', '', 0, 1, '', '{%CurrentSite.SiteIsContentOnly|(identity)GlobalAdministrator|(hash)4a9f4d4ed27bf619470d31ae893ef16f423b7e43fe664c9d18c84654629a32ab%}', 0)

UPDATE [CMS_UIElement] SET
    [ElementIDPath] = COALESCE((SELECT TOP 1 [ElementIDPath] FROM [CMS_UIElement] AS [Parent] WHERE [Parent].ElementID = @elementParentID), '')
                          + '/'
                          + REPLICATE('0', 8 - LEN([ElementID]))
                          + CAST([ElementID] AS NVARCHAR(8))
WHERE [ElementGUID] = @newElementGUID

UPDATE [CMS_UIElement] SET
    [ElementChildCount] = (SELECT COUNT(*)
                                    FROM [CMS_UIElement] AS [Children]
                                    WHERE [Children].[ElementParentID] = @elementParentID)
WHERE [ElementID] = @elementParentID

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 4
    WHERE [ElementGUID] = '281a6791-6179-4a92-a86d-2fd410a19b45'

UPDATE [CMS_UIElement] SET 
        [ElementVisibilityCondition] = '{%!CurrentSite.SiteIsContentOnly|(identity)GlobalAdministrator|(hash)fa60732c2cb1eb14933d5245a0786e9cbffcb243a57614ddf0c131ecf6c2e43e%}'
    WHERE [ElementGUID] = '7dc5113e-991a-447c-8783-3d7e1472ebac'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 3
    WHERE [ElementGUID] = '2cce21c5-4ecd-4aef-a10a-84da1be499dd'


END

END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'bbc600ae-e00d-4d82-a548-875800839488')
IF @elementParentID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET
    [ElementChildCount] = (SELECT COUNT(*)
                                    FROM [CMS_UIElement] AS [Children]
                                    WHERE [Children].[ElementParentID] = @elementParentID)
WHERE [ElementID] = @elementParentID

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'f23e944e-1a51-46f0-8a2e-75bcfc2a70ea')
IF @classResourceID IS NOT NULL BEGIN


DECLARE @classXmlSchema nvarchar(max);

SET @classXmlSchema = '<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
  <xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
    <xs:complexType>
      <xs:choice minOccurs="0" maxOccurs="unbounded">
        <xs:element name="OM_ABVariantData">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ABVariantID" msdata:ReadOnly="true" msdata:AutoIncrement="true" type="xs:int" />
              <xs:element name="ABVariantDisplayName">
                <xs:simpleType>
                  <xs:restriction base="xs:string">
                    <xs:maxLength value="100" />
                  </xs:restriction>
                </xs:simpleType>
              </xs:element>
              <xs:element name="ABVariantGUID" msdata:DataType="System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" type="xs:string" />
              <xs:element name="ABVariantTestID" type="xs:int" />
              <xs:element name="ABVariantIsOriginal" type="xs:boolean" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
    <xs:unique name="Constraint1" msdata:PrimaryKey="true">
      <xs:selector xpath=".//OM_ABVariantData" />
      <xs:field xpath="ABVariantID" />
    </xs:unique>
  </xs:element>
</xs:schema>'

INSERT [CMS_Class] ([ClassDisplayName], [ClassName], [ClassUsesVersioning], [ClassIsDocumentType], [ClassIsCoupledClass], [ClassXmlSchema], [ClassFormDefinition], [ClassEditingPageUrl], [ClassListPageUrl], [ClassNodeNameSource], [ClassTableName], [ClassViewPageUrl], [ClassPreviewPageUrl], [ClassFormLayout], [ClassNewPageUrl], [ClassShowAsSystemTable], [ClassUsePublishFromTo], [ClassShowTemplateSelection], [ClassSKUMappings], [ClassIsMenuItemType], [ClassNodeAliasSource], [ClassDefaultPageTemplateID], [ClassLastModified], [ClassGUID], [ClassCreateSKU], [ClassIsProduct], [ClassIsCustomTable], [ClassShowColumns], [ClassSearchTitleColumn], [ClassSearchContentColumn], [ClassSearchImageColumn], [ClassSearchCreationDateColumn], [ClassSearchSettings], [ClassInheritsFromClassID], [ClassSearchEnabled], [ClassSKUDefaultDepartmentName], [ClassSKUDefaultDepartmentID], [ClassContactMapping], [ClassContactOverwriteEnabled], [ClassSKUDefaultProductType], [ClassConnectionString], [ClassIsProductSection], [ClassPageTemplateCategoryID], [ClassFormLayoutType], [ClassVersionGUID], [ClassDefaultObjectType], [ClassIsForm], [ClassResourceID], [ClassCustomizedColumns], [ClassCodeGenerationSettings], [ClassIconClass], [ClassIsContentOnly], [ClassURLPattern])
 VALUES ('A/B variant data', 'OM.ABVariantData', 0, 0, 1, @classXmlSchema, '<form version="2"><field column="ABVariantID" columntype="integer" guid="3e802b82-ffd3-4ff2-b5d2-2021374314d3" isPK="true" publicfield="false" system="true"><properties><fieldcaption>ABVariantID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="ABVariantDisplayName" columnsize="100" columntype="text" guid="8d97b4b4-ee41-4e28-bfcd-eff3c3d05b65" publicfield="false" system="true" /><field column="ABVariantGUID" columntype="guid" guid="7b2bcc27-3035-49ec-85ea-373091751b22" publicfield="false" system="true" /><field column="ABVariantTestID" columntype="integer" guid="5b2d1674-8e59-402a-993b-0087c4ef6aef" publicfield="false" refobjtype="om.abtest" reftype="Required" system="true" /><field column="ABVariantIsOriginal" columntype="boolean" guid="aac9e276-6eb7-4980-bed2-6010f941016d" publicfield="false" system="true"><properties><defaultvalue>False</defaultvalue></properties></field></form>', '', NULL, '', 'OM_ABVariantData', NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 0, NULL, NULL, getDate(), 'ce9a1d18-20d0-460b-9755-7fd0dade46b7', NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, 0, @classResourceID, '', NULL, NULL, 0, NULL)


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @formClassID int;
SET @formClassID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassGUID] = '71a1d617-6161-4ad6-aee2-be5756048223')
IF @formClassID IS NOT NULL BEGIN

INSERT [CMS_AlternativeForm] ([FormDisplayName], [FormName], [FormClassID], [FormDefinition], [FormLayout], [FormGUID], [FormLastModified], [FormCoupledClassID], [FormHideNewParentFields], [FormLayoutType], [FormVersionGUID], [FormCustomizedColumns], [FormIsCustom])
 VALUES ('Update (MVC)', 'UpdateMvc', @formClassID, '<form version="2"><field column="ABTestID" guid="86655528-dafb-45db-83d4-dd0b75569349" /><field column="ABTestDisplayName" guid="19a599a7-6256-476a-8d37-9faf5ac33dbf" /><field column="ABTestName" guid="1d1b3ae7-1a63-48f7-ae8c-5984d7063c8f" /><field column="ABTestDescription" guid="559f6e45-2cc3-4326-9f5c-f66cb4e45cce" /><field column="ABTestOriginalPage" guid="68d756ae-ca0e-4cc0-b903-f79d10f2f701" /><field column="ABTestCulture" guid="0e01436b-dc1c-4bc0-a57b-a2e6a55dfac1"><settings><controlname>LabelControl</controlname><ResolveMacros>True</ResolveMacros><Transformation>#culturename</Transformation><AllowAll /><AllowDefault /><AllowEmpty /><AllRecordValue /></settings></field><field column="ABTestOpenFrom" guid="fab79e13-0177-4616-bd9b-a1a48ba32fc1" /><field column="ABTestOpenTo" guid="a99b03df-10da-4804-ae9c-dd8924733318" /><field column="ABTestConversions" guid="374f0b4f-20ba-4b45-945c-728ae3f13005" spellcheck="false"><settings><controlname>ABTestConversionSelector</controlname><AddGlobalObjectNamePrefix /><AddGlobalObjectSuffix /><AllowAll /><AllowDefault /><AllowEditTextBox /><AllowEmpty /><DialogWindowHeight /><DialogWindowName /><EditDialogWindowHeight /><EditDialogWindowWidth /><EditWindowName /><EncodeOutput /><GlobalObjectSuffix /><ItemsPerPage /><LocalizeItems /><MaxDisplayedItems /><MaxDisplayedTotalItems /><ObjectSiteName /><ObjectType /><RemoveMultipleCommas /><ReturnColumnName /><ReturnColumnType /><SelectionMode /><UseAutocomplete /><ValuesSeparator /></settings><properties><fieldcaption>{$abtesting.conversions$}</fieldcaption><fielddescription>{$abtesting.conversions.description$}</fielddescription></properties></field><field column="ABTestVisitorTargeting" guid="34a3dd50-b49b-45a7-b08e-0fc1dfc179df" /><field column="ABTestIncludedTraffic" guid="5791e2c4-9985-43fa-a362-9edaf7025b77" /><field column="ABTestSiteID" guid="b8c25b1d-6ab4-46e7-994a-5d844cbb5625" /><field column="ABTestWinnerGUID" guid="f21d2a3f-019c-443f-b91e-a2fc9218efab" /><field column="ABTestGUID" guid="eab35f06-804c-4193-8830-dee55dae8deb" /><field column="ABTestLastModified" guid="80955796-c133-49d8-a7eb-1f9b10399e25" /></form>', NULL, 'ec151054-15f9-4931-a5e2-2deb35df4bc9', getDate(), NULL, 1, NULL, NULL, '', 0)


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @userControlResourceID int;
SET @userControlResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'f23e944e-1a51-46f0-8a2e-75bcfc2a70ea')
IF @userControlResourceID IS NOT NULL BEGIN

INSERT [CMS_FormUserControl] ([UserControlDisplayName], [UserControlCodeName], [UserControlFileName], [UserControlForText], [UserControlForLongText], [UserControlForInteger], [UserControlForDecimal], [UserControlForDateTime], [UserControlForBoolean], [UserControlForFile], [UserControlShowInBizForms], [UserControlDefaultDataType], [UserControlDefaultDataTypeSize], [UserControlShowInDocumentTypes], [UserControlShowInSystemTables], [UserControlShowInWebParts], [UserControlShowInReports], [UserControlGUID], [UserControlLastModified], [UserControlForGuid], [UserControlShowInCustomTables], [UserControlForVisibility], [UserControlParameters], [UserControlForDocAttachments], [UserControlResourceID], [UserControlType], [UserControlParentID], [UserControlDescription], [UserControlThumbnailGUID], [UserControlPriority], [UserControlIsSystem], [UserControlForBinary], [UserControlForDocRelationships], [UserControlAssemblyName], [UserControlClassName])
 VALUES ('A/B test conversion selector', 'ABTestConversionSelector', '', 0, 1, 0, 0, 0, 0, 0, 0, 'Text', 0, 0, 1, 0, 0, 'e42fd75d-136f-4708-be32-f7268404574c', getDate(), 0, 0, 0, '<form version="2" />', 0, @userControlResourceID, 2, NULL, '', NULL, 0, 0, 0, 0, 'CMS.OnlineMarketing.Web.UI', 'CMS.OnlineMarketing.Web.UI.ConversionDesigner')


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '7aa16531-4f60-4aee-85df-89409c275610')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = 'bc86a286-996d-4445-ac8a-68ed45d115b3')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '98c6ee00-230a-4207-a6d3-03677b83b245')
IF @elementResourceID IS NOT NULL BEGIN

DECLARE @newElementGUID uniqueidentifier = '1abbaa44-2832-4be9-8848-ff0c911b4e8b';

-- Insert a new UI element
INSERT [CMS_UIElement] ([ElementDisplayName], [ElementName], [ElementCaption], [ElementTargetURL], [ElementResourceID], [ElementParentID], [ElementChildCount], [ElementOrder], [ElementLevel], [ElementIDPath], [ElementIconPath], [ElementIsCustom], [ElementLastModified], [ElementGUID], [ElementSize], [ElementDescription], [ElementFromVersion], [ElementPageTemplateID], [ElementType], [ElementProperties], [ElementIsMenu], [ElementFeature], [ElementIconClass], [ElementIsGlobalApplication], [ElementCheckModuleReadPermission], [ElementAccessCondition], [ElementVisibilityCondition], [ElementRequiresGlobalAdminPriviligeLevel])
 VALUES ('{$pagetemplatesmvc.saveastemplate$}', 'Page.SaveAsTemplate', '{$pagetemplatesmvc.saveastemplate$}', '', @elementResourceID, @elementParentID, 0, 1, 6, '', '', 0, getDate(), @newElementGUID, 0, '', '12.0', @elementPageTemplateID, 'PageTemplate', '<Data><AfterSaveScript>cmsrequire([''CMS/EventHub''], function (eventHub) {
  eventHub.publish({name: ''pagesavedastemplate'', onlySubscribed: true });
  CloseDialog(false);
})</AfterSaveScript><AlternativeFormName>SaveAsTemplateDialog</AlternativeFormName><DefaultFieldLayout>twocolumns</DefaultFieldLayout><DefaultFormLayout>Divs</DefaultFormLayout><DisplayBreadcrumbs>False</DisplayBreadcrumbs><EditExtender>CMS.UIControls</EditExtender><ExtenderClassName>CMS.UIControls.SaveAsTemplateFormExtender</ExtenderClassName><ObjectType>cms.pagetemplateconfiguration</ObjectType><ParentObjectType>cms.document</ParentObjectType></Data>', 0, '', '', 0, 1, '{%CurrentUser.IsAuthorizedPerResource("CMS.Content","ManagePageTemplates") @%}', '', 0)

-- Update ID path
UPDATE [CMS_UIElement] SET
	[ElementIDPath] = COALESCE((SELECT TOP 1 [ElementIDPath] FROM [CMS_UIElement] AS [Parent] WHERE [Parent].ElementID = @elementParentID), '')
						  + '/'
						  + REPLICATE('0', 8 - LEN([ElementID]))
						  + CAST([ElementID] AS NVARCHAR(8))

WHERE [ElementGUID] = @newElementGUID


-- Update parent's child count
UPDATE [CMS_UIElement] SET
	[ElementChildCount] = (SELECT COUNT(*)
									FROM [CMS_UIElement] AS [Children]
									WHERE [Children].[ElementParentID] = @elementParentID)
WHERE [ElementID] = @elementParentID


END

END

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @formClassID int;
SET @formClassID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassGUID] = 'f3f579ea-6ea3-4ba8-a0ac-ccc9d0ba8bc4')
IF @formClassID IS NOT NULL BEGIN

INSERT [CMS_AlternativeForm] ([FormDisplayName], [FormName], [FormClassID], [FormDefinition], [FormLayout], [FormGUID], [FormLastModified], [FormCoupledClassID], [FormHideNewParentFields], [FormLayoutType], [FormVersionGUID], [FormCustomizedColumns], [FormIsCustom])
 VALUES ('Save as template dialog', 'SaveAsTemplateDialog', @formClassID, '<form version="2"><field column="PageTemplateConfigurationID" guid="8e91a6c7-f065-45f0-bc0f-9a559f347dcc" /><field column="PageTemplateConfigurationGUID" guid="a88502e2-f3bd-4719-95ee-e9bfcd8a5c89" /><field column="PageTemplateConfigurationSiteID" guid="1b5d4637-60bd-4c69-8cb4-b75ca602b8cb" /><field column="PageTemplateConfigurationLastModified" guid="f06efbab-9053-4ed6-a94c-90f558a96a0a" /><field column="PageTemplateConfigurationName" guid="bba0b1ab-beef-4e95-b52c-cf138d223db9" visible="true"><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings><properties><fieldcaption>{$pagetemplatesmvc.displayname$}</fieldcaption></properties></field><field column="PageTemplateConfigurationDescription" guid="7cb27baa-1764-4880-a37a-df955bde0f1a" visible="true"><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextAreaControl</controlname><FilterMode>False</FilterMode><Wrap>True</Wrap></settings><properties><fieldcaption>{$pagetemplatesmvc.description$}</fieldcaption></properties></field><field column="PageTemplateConfigurationThumbnailGUID" guid="0a090dad-0984-4643-b253-a03a187b17b4" visible="true"><settings><controlname>MetafileUploaderControl</controlname><ObjectCategory>Thumbnail</ObjectCategory></settings><properties><fieldcaption>{$pagetemplatesmvc.thumbnail$}</fieldcaption></properties></field><field column="PageTemplateConfigurationTemplate" guid="4d25c2dc-f0dc-49da-a5fa-a6da4459fe82" /><field column="PageTemplateConfigurationWidgets" guid="8657d5bd-e2ac-4869-bf69-9310f64f63fb" /><field column="PageTemplateConfigurationCreatedByUserID" guid="e0928811-0702-4c08-a220-2d73af367a6c" order="9" /></form>', NULL, '056a20bd-fa4e-4d98-9559-e2d525a5de0b', getDate(), NULL, 1, NULL, NULL, '', 0)


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @categoryParentID int;
SET @categoryParentID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = '5e7ffa76-b570-45b0-80f5-a56cba487a39')
IF @categoryParentID IS NOT NULL BEGIN

INSERT [Reporting_ReportCategory] ([CategoryDisplayName], [CategoryCodeName], [CategoryGUID], [CategoryLastModified], [CategoryParentID], [CategoryImagePath], [CategoryPath], [CategoryLevel], [CategoryChildCount], [CategoryReportChildCount])
 VALUES ('A/B testing (MVC)', 'A_BTesting_MVC', '03954bd1-3b62-489d-8cc3-bfdb7dd61ea3', getDate(), @categoryParentID, NULL, '/A_BTesting_MVC', 1, 4, 0)


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @categoryParentID int;
SET @categoryParentID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = '03954bd1-3b62-489d-8cc3-bfdb7dd61ea3')
IF @categoryParentID IS NOT NULL BEGIN

INSERT [Reporting_ReportCategory] ([CategoryDisplayName], [CategoryCodeName], [CategoryGUID], [CategoryLastModified], [CategoryParentID], [CategoryImagePath], [CategoryPath], [CategoryLevel], [CategoryChildCount], [CategoryReportChildCount])
 VALUES ('Average conversion value (MVC)', 'AverageConversionValue_MVC', 'e771dfce-a2a4-4fc2-aaca-122b4c3d8624', getDate(), @categoryParentID, NULL, '/A_BTesting_MVC/AverageConversionValue_MVC', 2, 0, 3)

INSERT [Reporting_ReportCategory] ([CategoryDisplayName], [CategoryCodeName], [CategoryGUID], [CategoryLastModified], [CategoryParentID], [CategoryImagePath], [CategoryPath], [CategoryLevel], [CategoryChildCount], [CategoryReportChildCount])
 VALUES ('Conversion rate (MVC)', 'ConversionRate_MVC', 'd0134957-fcf7-4631-9a72-33263197ac00', getDate(), @categoryParentID, NULL, '/A_BTesting_MVC/ConversionRate_MVC', 2, 0, 3)

INSERT [Reporting_ReportCategory] ([CategoryDisplayName], [CategoryCodeName], [CategoryGUID], [CategoryLastModified], [CategoryParentID], [CategoryImagePath], [CategoryPath], [CategoryLevel], [CategoryChildCount], [CategoryReportChildCount])
 VALUES ('Conversion value (MVC)', 'ConversionValue_MVC', 'd06190c9-e9cc-4db7-ac05-3fe3fc0ecd61', getDate(), @categoryParentID, NULL, '/A_BTesting_MVC/ConversionValue_MVC', 2, 0, 3)

INSERT [Reporting_ReportCategory] ([CategoryDisplayName], [CategoryCodeName], [CategoryGUID], [CategoryLastModified], [CategoryParentID], [CategoryImagePath], [CategoryPath], [CategoryLevel], [CategoryChildCount], [CategoryReportChildCount])
 VALUES ('Conversions count (MVC)', 'ConversionsCount_MVC', '66d9c283-a08f-4aec-9cb6-4c57f185691b', getDate(), @categoryParentID, NULL, '/A_BTesting_MVC/ConversionsCount_MVC', 2, 0, 3)


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @parentCategoryID int;
SET @parentCategoryID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = '5e7ffa76-b570-45b0-80f5-a56cba487a39')

IF @parentCategoryID IS NOT NULL BEGIN

DECLARE @childCount int;
SET @childCount = (SELECT COUNT (1) FROM [Reporting_ReportCategory] WHERE [CategoryParentID] = @parentCategoryID)

UPDATE [Reporting_ReportCategory] SET 
        [CategoryChildCount] = @childCount
    WHERE [CategoryID] = @parentCategoryID
    
END


END


GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @reportCategoryID int;
SET @reportCategoryID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = '66d9c283-a08f-4aec-9cb6-4c57f185691b')
IF @reportCategoryID IS NOT NULL BEGIN

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversioncount.dayreport', 'Conversion count -  Daily', '<p>%%control:ReportGraph?mvcabtestconversioncount.dayreport.graph%%</p>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><FilterMode>False</FilterMode><controlname>textboxcontrol</controlname><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '661973b3-2247-424a-8b1c-939bb441eadf', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversioncount.monthreport', 'Conversion count -  Monthly', '<p>
	%%control:ReportGraph?mvcabtestconversioncount.monthreport.graph%%</p>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '7d1b4de6-f27e-465b-ab9a-0b6f4dbc9278', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversioncount.weekreport', 'Conversion count - Weekly', '<p>%%control:ReportGraph?mvcabtestconversioncount.weekreport.graph%%</p>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '265faf9e-0cdf-4f6d-80bc-b0e373da643f', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @reportCategoryID int;
SET @reportCategoryID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = 'd0134957-fcf7-4631-9a72-33263197ac00')
IF @reportCategoryID IS NOT NULL BEGIN

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversionrate.dayreport', 'Conversion rate - Daily', '<p>
	%%control:ReportGraph?mvcabtestconversionrate.dayreport.graph%%</p>
<div firebugversion="1.5.4" id="_firebugConsole" style="display: none;">
	&nbsp;</div>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><FilterMode>False</FilterMode><controlname>textboxcontrol</controlname><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '5f600553-d314-4e99-bf16-4a0ea51fbc13', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversionrate.monthreport', 'Conversion rate - Monthly', '<p>%%control:ReportGraph?mvcabtestconversionrate.monthreport.graph%%</p>

<div firebugversion="1.5.4" id="_firebugConsole" style="display: none;">&nbsp;</div>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '1cc5415a-31bd-4824-b540-9d34846ce35a', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversionrate.weekreport', 'Conversion rate - Weekly', '<p>
	%%control:ReportGraph?mvcabtestconversionrate.weekreport.graph%%</p>
<div firebugversion="1.5.4" id="_firebugConsole" style="display: none;">
	&nbsp;</div>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, 'ed3cb1dc-4b0d-48b7-b45d-a30cb9944058', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @reportCategoryID int;
SET @reportCategoryID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = 'd06190c9-e9cc-4db7-ac05-3fe3fc0ecd61')
IF @reportCategoryID IS NOT NULL BEGIN

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversionvalue.dayreport', 'Conversion value - Daily', '<p>
	%%control:ReportGraph?mvcabtestconversionvalue.dayreport.graph%%</p>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><FilterMode>False</FilterMode><controlname>textboxcontrol</controlname><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '73f30508-1bbc-450b-8d48-2829b2b00eec', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversionvalue.monthreport', 'Conversion value -  Monthly', '<p>
	%%control:ReportGraph?mvcabtestconversionvalue.monthreport.graph%%</p>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '9567651c-efe0-4f8a-9a6b-c36a126923dc', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestconversionvalue.weekreport', 'Conversion value - Weekly', '<p>
	%%control:ReportGraph?mvcabtestconversionvalue.weekreport.graph%%</p>
', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, 'c30a7bd2-6c53-4c77-bb0e-afcce8384d16', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @reportCategoryID int;
SET @reportCategoryID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = 'e771dfce-a2a4-4fc2-aaca-122b4c3d8624')
IF @reportCategoryID IS NOT NULL BEGIN

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestaverageconversionvalue.dayreport', 'Average conversion value - Daily', '%%control:ReportGraph?mvcabtestaverageconversionvalue.dayreport.Graph%%', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '86686015-2bdb-430a-bf26-0adabc4eca68', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestaverageconversionvalue.monthreport', 'Average conversion value - Monthly', '%%control:ReportGraph?mvcabtestaverageconversionvalue.monthreport.Graph%%', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '5a438e0f-8ed4-44e6-a9f1-9a06931cb826', getDate(), 1, '')

INSERT [Reporting_Report] ([ReportName], [ReportDisplayName], [ReportLayout], [ReportParameters], [ReportCategoryID], [ReportAccess], [ReportGUID], [ReportLastModified], [ReportEnableSubscription], [ReportConnectionString])
 VALUES ('mvcabtestaverageconversionvalue.weekreport', 'Average conversion value - Weekly', '%%control:ReportGraph?mvcabtestaverageconversionvalue.weekreport.graph%%', '<form><field column="FromDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="f3ee01cd-1abc-4f24-b402-990e9ace0840" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(-1).Date|(identity)GlobalAdministrator|(hash)213884fe81a6cf7798d59658f44ef7ae7e36478aa2646d511ce058c0c158e52d%}</defaultvalue><fieldcaption>From</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="ToDate" visible="true" columntype="datetime" fieldtype="CustomUserControl" publicfield="false" guid="eba38d8b-5083-4c5a-978a-fca08d6a9985" reftype="Required"><properties><defaultvalue ismacro="true">{%CurrentDateTime.AddMonths(1).Date|(identity)GlobalAdministrator|(hash)aa7b285f8d35ed937e05acf9ccf0667c8aeb34ecdb85e5d0196431208730c4cb%}</defaultvalue><fieldcaption>To</fieldcaption></properties><settings><TimeZoneType>inherit</TimeZoneType><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>False</EditTime></settings></field><field column="GraphType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="8da7de27-72eb-411c-b5ee-5911945a4aca" reftype="Required"><properties><fieldcaption>Graph type</fieldcaption></properties><settings><controlname>dropdownlistcontrol</controlname><SortItems>False</SortItems><EditText>False</EditText><Options>cumulative;Cumulative
daywise;DayWise</Options></settings></field><field column="TestName" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="bc6c792d-9f72-4ce4-9cca-8ba06e54ca62" reftype="Required"><properties><fieldcaption>AB Test name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><ValuesSeparator>;</ValuesSeparator><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><UseAutocomplete>True</UseAutocomplete><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ReturnColumnType>id</ReturnColumnType><LocalizeItems>True</LocalizeItems><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestID" visible="true" columntype="integer" fieldtype="CustomUserControl" publicfield="false" guid="25054d98-5657-4836-88e3-439bceb95afc" reftype="Required"><properties><fieldcaption>AB Test ID</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ABTestID</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>om.abtest</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>0</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ABTestCulture" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="5f3345d9-e883-4537-accc-f317ea13fa96" reftype="Required"><properties><fieldcaption>Culture</fieldcaption></properties><settings><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><Trim>False</Trim><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><FilterMode>False</FilterMode><controlname>textboxcontrol</controlname><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching></settings></field><field column="ConversionName" visible="true" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3da264f5-af35-4af1-a0aa-b6887af26b61" reftype="Required"><properties><fieldcaption>Conversion name</fieldcaption></properties><settings><ObjectSiteName>#currentsite</ObjectSiteName><controlname>uni_selector</controlname><AllowEmpty>False</AllowEmpty><MaxDisplayedItems>25</MaxDisplayedItems><GlobalObjectSuffix>(global)</GlobalObjectSuffix><RemoveMultipleCommas>False</RemoveMultipleCommas><ReturnColumnName>ConversionName</ReturnColumnName><DialogWindowName>SelectionDialog</DialogWindowName><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><ObjectType>Analytics.Conversion</ObjectType><DialogWindowWidth>668</DialogWindowWidth><ItemsPerPage>25</ItemsPerPage><AllowEditTextBox>False</AllowEditTextBox><SelectionMode>1</SelectionMode><AllowAll>False</AllowAll><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><LocalizeItems>True</LocalizeItems><AllowDefault>False</AllowDefault><EncodeOutput>True</EncodeOutput><DialogWindowHeight>590</DialogWindowHeight><UseAutocomplete>True</UseAutocomplete><ReturnColumnType>id</ReturnColumnType><ValuesSeparator>;</ValuesSeparator><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix></settings></field><field column="ConversionType" visible="true" columntype="text" fieldtype="CustomUserControl" columnsize="100" publicfield="false" guid="b62f6c39-321b-4d4e-9c16-68662cc8dd7e" reftype="Required"><properties><fieldcaption>Conversion type</fieldcaption></properties><settings><Options>abconversion;{$abtesting.conversiontype.transaction$}
absessionconversion%;{$abtesting.conversiontype.session$}
absessionconversionfirst;{$abtesting.conversiontype.visitor$}</Options><SortItems>False</SortItems><EditText>False</EditText><controlname>dropdownlistcontrol</controlname></settings></field><field column="VariationName" columntype="text" fieldtype="CustomUserControl" allowempty="true" columnsize="100" publicfield="false" guid="3e177e39-584b-4192-94ee-4b3b697f8926" reftype="Required" /></form>', @reportCategoryID, 0, '9908d225-250d-4bc7-ae7a-97d8ebf3b59b', getDate(), 1, '')


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '1cc5415a-31bd-4824-b540-9d34846ce35a')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '	-- This SQL script is used for getting conversion rates.
	-- Required variables are:
	-- @FromDate, @ToDate - used for specifying date range
	-- @GraphType - can be Cumulative or DayWise
	-- @TestName - ABTestName
	-- @ABTestID - ABTestID
	-- @ABTestCulture - ABTestCulture
	-- @ConversionName - selected conversion or empty for all conversions
	-- @VariationName - selected variation (GUID) or empty for all variations
	-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

	EXEC Proc_Analytics_RemoveTempTable
	CREATE TABLE #AnalyticsTempTable (
		StartTime DATETIME,
		Hits DECIMAL(20,5),
		Visits DECIMAL(20,5),
		Name NVARCHAR(300) COLLATE DATABASE_DEFAULT,
		RunningHits DECIMAL(20,5),
		RunningVisits DECIMAL(20,5),
		CumulativeRate DECIMAL(20,5),
		DayWiseRate DECIMAL(20,5)
	);

	DECLARE @VisitType NVARCHAR(MAX)
    SET @VisitType = ''abvisit%''

    IF @ConversionType = ''absessionconversionfirst''
	BEGIN
		SET @VisitType = ''abvisitfirst''
    END

	SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'');
 
	-- Get hits and visits
	INSERT INTO #AnalyticsTempTable (StartTime, Hits, Visits, Name)
	SELECT [Date], SUM(Hits) as Hits, SUM(Visits) as Visits, ABVariantDisplayName FROM
	(
		SELECT [DATE], Hits, 0 as Visits, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''month'') AS Dates
		LEFT JOIN
			(
			SELECT HitsStartTime, SUM(HitsCount) as Hits, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_MonthHits ON Analytics_Statistics.StatisticsID = Analytics_MonthHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
			) AS T1
		ON Dates.Date= T1.HitsStartTime
	
		UNION
	
		SELECT [DATE], 0 as Hits, Visits, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''month'') AS Dates
		LEFT JOIN
			(SELECT HitsStartTime, SUM(HitsCount) as Visits, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @VisitType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_MonthHits ON Analytics_Statistics.StatisticsID = Analytics_MonthHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
		) AS T2
		ON Dates.Date=T2.HitsStartTime
	) as T3
	GROUP BY T3.[DATE], T3.ABVariantDisplayName
	
	-- Fill in missing values
	-- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative rates are calculated the right way	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, 0, T2.Name, NULL, NULL, NULL, NULL
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of rates
	DECLARE @RunningVisits DECIMAL(20,5)
	SET @RunningVisits = 0

	DECLARE @RunningHits DECIMAL(20,5)
	SET @RunningHits = 0

	DECLARE @LastName NVARCHAR(MAX)
		
	-- Sum running totals and rates using ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningVisits = RunningVisits =  ISNULL(Visits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningVisits,0) ELSE 0 END,
			@RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
			CumulativeRate = @RunningHits / NULLIF(@RunningVisits, 0) * 100,
			DayWiseRate = Hits / NULLIF(Visits,0) * 100,
			@LastName = Name

	-- Prepare for PIVOT - delete remaining columns
	ALTER TABLE #AnalyticsTempTable DROP COLUMN Visits
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningVisits

	DECLARE @engineEdition int;
	SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

	-- Drop columns based on graph type
	IF @GraphType = ''DayWise''
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate		
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = DayWiseRate WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits			
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.DayWiseRate'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END
	ELSE --Cumulative
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate		
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = CumulativeRate WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits		
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.CumulativeRate'''', ''''Hits'''', ''''COLUMN'''''')		
		END
	END
	
	EXEC Proc_Analytics_Pivot ''month''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY%</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>y</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxisformat>{0.0\%}</yaxisformat><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '0ab24fe2-55f9-46f5-be19-44a9c4950830', getDate(), NULL, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '265faf9e-0cdf-4f6d-80bc-b0e373da643f')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '-- This SQL script is used for getting conversion counts.
    -- Required variables are:
    -- @FromDate, @ToDate - used for specifying date range
    -- @GraphType - can be Cumulative or DayWise
    -- @TestName - ABTestName
    -- @ABTestID - ABTestID
    -- @ABTestCulture - ABTestCulture
    -- @ConversionName - selected conversion or empty for all conversions
    -- @VariationName - selected variation (GUID) or empty for all variations
-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

    EXEC Proc_Analytics_RemoveTempTable
    CREATE TABLE #AnalyticsTempTable (
     StartTime DATETIME,
     Hits INT,
     Name NVARCHAR(300),
     RunningHits INT
    );

    SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'');

    INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
    SELECT [DATE], Hits, ABVariantDisplayName
    FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''week'') AS Dates
    LEFT JOIN
    (
        SELECT HitsStartTime, SUM(HitsCount) as Hits, ABVariantDisplayName
        FROM OM_ABVariantData
        JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
        JOIN Analytics_WeekHits ON Analytics_Statistics.StatisticsID = Analytics_WeekHits.HitsStatisticsID
        WHERE ABVariantTestID = @ABTestID AND StatisticsSiteID = @CMSContextCurrentSiteID
        AND (@VariationName IS NULL OR @VariationName IN (CAST(ABVariantGUID AS char(36)), ''''))
        AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
        AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR @ABTestCulture IS NULL OR @ABTestCulture IN (StatisticsObjectCulture, ''''))
        GROUP BY HitsStartTime, ABVariantDisplayName
    ) as X on [DATE] = HitsStartTime
    

    -- Fill in missing values
    -- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative hits are calculated the right way    
    INSERT INTO #AnalyticsTempTable
        SELECT T1.StartTime, 0, T2.Name, 0
        FROM #AnalyticsTempTable as T1
        JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL
        WHERE NOT EXISTS
            (
            SELECT T3.StartTime, T3.Name
            FROM #AnalyticsTempTable as T3
            WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
            )
        GROUP BY T1.StartTime, T2.Name

    -- Delete NULL values
    DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

    -- Declare variables for calculation of hits
    DECLARE @RunningHits INT
    SET @RunningHits = 0

    DECLARE @LastName NVARCHAR(MAX)
        
    -- Sum running hits using the ''quirky update''
    -- Uses view to define order
    ;WITH TempView AS
    (
        SELECT TOP (2147483647) *
        FROM #AnalyticsTempTable
        ORDER BY
                Name, StartTime
    )
    UPDATE TempView
    SET     @RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
            @LastName = Name


    -- Drop columns based on graph type
    IF @GraphType = ''Cumulative''
    BEGIN    
        DECLARE @engineEdition int;
        SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

        IF @engineEdition = 5
        BEGIN
            ---------------
            -- Azure SQL --
            ---------------
            -- Rename using the update, because Azure do not support rename of columns after CTE
            UPDATE #AnalyticsTempTable SET Hits = RunningHits WHERE Name = Name AND StartTime = StartTime
            ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
        END
        ELSE BEGIN
            ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
            -- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''    
            EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.RunningHits'''', ''''Hits'''', ''''COLUMN'''''')            
        END
    END
    ELSE
    BEGIN
        ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
    END          

    EXEC Proc_Analytics_Pivot ''week''
    EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '409ebc0a-e1b3-4e4d-a1ee-d755f94b2d9b', getDate(), NULL, '')

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph_detail', 'graph_detail', 'EXEC Proc_Analytics_RemoveTempTable
CREATE TABLE #AnalyticsTempTable (
  StartTime DATETIME,
  Hits INT,
  Name NVARCHAR(300)
);

 SET @FromDate ={%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'');
  INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)

   SELECT [Date] AS StartTime ,T1.Hits AS Hits  ,T1.Name AS Name
FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''Week'') AS Dates
 LEFT JOIN
  (SELECT HitsStartTime AS Interval, SUM(HitsCount) AS Hits ,ABVariantDisplayName AS Name
  FROM Analytics_Statistics 
  INNER JOIN Analytics_WeekHits ON StatisticsID = Analytics_WeekHits.HitsStatisticsID
  INNER JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  INNER JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  WHERE StatisticsSiteID = @CMSContextCurrentSiteID   AND  StatisticsCode LIKE ''abconversion;%'' 
  AND (@TestName IS NULL OR @TestName = ''''  OR  @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14))
  AND (@ConversionName IS NULL OR @ConversionName = '''' OR @ConversionName = StatisticsObjectName)
  GROUP BY HitsStartTime,ABVariantDisplayName
  ) AS T1
 ON Dates.Date=T1.Interval;
  
  EXEC Proc_Analytics_Pivot ''week''
  EXEC Proc_Analytics_RemoveTempTable', 0, '', @graphReportID, 'Conversions count', '', '', 600, 400, 100, '<CustomData><barorientation>Horizontal</barorientation><baroverlay>False</baroverlay><columnwidth>20</columnwidth><displayitemvalue>True</displayitemvalue><displaylegend>True</displaylegend><exportenabled>True</exportenabled><itemvalueformat>{%Format(ToDouble(pval, &quot;0.0&quot;), &quot;{0:0.0}&quot;)|(identity)GlobalAdministrator|(hash)5551e18976364394c0ee130476cd25d1d35fc90b9ab6212932d2c48efee71aae%}% ({%yval%})</itemvalueformat><legendinside>False</legendinside><legendposition>Right</legendposition><legendtitle>Variants</legendtitle><linedrawinstyle>Line</linedrawinstyle><pieshowpercentage>False</pieshowpercentage><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesgradient>None</seriesgradient><seriesitemtooltip>{%ser%}</seriesitemtooltip><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisformat>{yyyy}</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', 'e8cb4eb3-e273-4e0e-9a91-52390abeb4d9', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '5a438e0f-8ed4-44e6-a9f1-9a06931cb826')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('Graph', 'graph', '-- This SQL script is used for getting average conversion value.
-- Required variables are:
-- @FromDate, @ToDate - used for specifying date range
-- @GraphType - can be Cumulative or DayWise
-- @TestName - ABTestName
-- @ABTestID - ABTestID
-- @ABTestCulture - ABTestCulture
-- @ConversionName - selected conversion or empty for all conversions
-- @VariationName - selected variation (GUID) or empty for all variations
-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

EXEC Proc_Analytics_RemoveTempTable	
	CREATE TABLE #AnalyticsTempTable (
		StartTime DATETIME,
		HitsCount DECIMAL(20,5),
		HitsValue FLOAT,
		Name NVARCHAR(300) COLLATE DATABASE_DEFAULT,
		RunningHitsCount DECIMAL(20,5),
		RunningHitsValue FLOAT,
		CumulativeAverage FLOAT,
		DayWiseAverage FLOAT,
		Hits FLOAT
	);

    SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'');
 
	-- Get conversion value and count
	INSERT INTO #AnalyticsTempTable (StartTime, HitsCount, HitsValue, Name)
		SELECT [DATE], HitsCount, HitsValue, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''month'') AS Dates
		LEFT JOIN
			(
			SELECT HitsStartTime,  Sum(HitsCount) as HitsCount, SUM(HitsValue) as HitsValue, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_MonthHits ON Analytics_Statistics.StatisticsID = Analytics_MonthHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
			) AS T1
		ON Dates.Date= T1.HitsStartTime	

	-- Select dates and names missing in the original data to fill the gaps
    -- This is needed to ensure that cumulative average conversion values are calculated correctly	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, 0, T2.Name, NULL, NULL, NULL, NULL, NULL
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of average value
	DECLARE @RunningHitsCount DECIMAL(20,5)
	SET @RunningHitsCount = 0

	DECLARE @RunningHitsValue FLOAT
	SET @RunningHitsValue = 0

	DECLARE @LastName NVARCHAR(MAX)
		
	-- Sum running totals and average conversion values using ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningHitsCount = RunningHitsCount =  ISNULL(HitsCount,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHitsCount,0) ELSE 0 END,
			@RunningHitsValue = RunningHitsValue = ISNULL(HitsValue,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHitsValue,0) ELSE 0 END,
			CumulativeAverage = @RunningHitsValue / NULLIF(@RunningHitsCount, 0),
			DayWiseAverage = HitsValue / NULLIF(HitsCount,0),
			@LastName = Name

	-- Prepare for PIVOT - delete all remaining columns
	ALTER TABLE #AnalyticsTempTable DROP COLUMN HitsValue
	ALTER TABLE #AnalyticsTempTable DROP COLUMN HitsCount
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHitsValue
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHitsCount

	DECLARE @engineEdition int;
	SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

	-- Drop columns based on graph type
	IF @GraphType = ''DayWise''
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage					
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = DayWiseAverage WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.DayWiseAverage'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END    	
	ELSE -- GraphType is everything else -> use Cumulative version	
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage					
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = CumulativeAverage WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.CumulativeAverage'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END

	EXEC Proc_Analytics_Pivot ''month''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No results. The test has no visits or the selected filter combination yields no results.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '89004552-80ac-461c-b60e-eeab6d6ab881', getDate(), NULL, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '5f600553-d314-4e99-bf16-4a0ea51fbc13')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '-- This SQL script is used for getting conversion rates.
-- Required variables are:
-- @FromDate, @ToDate - used for specifying date range
-- @GraphType - can be Cumulative or DayWise
-- @TestName - ABTestName
-- @ABTestID - ABTestID
-- @ABTestCulture - ABTestCulture
-- @ConversionName - selected conversion or empty for all conversions
-- @VariationName - selected variation (GUID) or empty for all variations
-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

EXEC Proc_Analytics_RemoveTempTable
	CREATE TABLE #AnalyticsTempTable (
		StartTime DATETIME,
		Hits DECIMAL(20,5),
		Visits DECIMAL(20,5),
		Name NVARCHAR(300) COLLATE DATABASE_DEFAULT,
		RunningHits DECIMAL(20,5),
		RunningVisits DECIMAL(20,5),
		CumulativeRate DECIMAL(20,5),
		DayWiseRate DECIMAL(20,5)
	);


	DECLARE @VisitType NVARCHAR(MAX)
    SET @VisitType = ''abvisit%''

    IF @ConversionType = ''absessionconversionfirst''
	BEGIN
		SET @VisitType = ''abvisitfirst''
    END

	SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'');
 
	-- Get hits and visits
	INSERT INTO #AnalyticsTempTable (StartTime, Hits, Visits, Name)
	SELECT [Date], SUM(Hits) as Hits, SUM(Visits) as Visits, ABVariantDisplayName FROM
	(
		SELECT [DATE], Hits, 0 as Visits, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''day'') AS Dates
		LEFT JOIN
			(
			SELECT HitsStartTime, SUM(HitsCount) as Hits, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_DayHits ON Analytics_Statistics.StatisticsID = Analytics_DayHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
			) AS T1
		ON Dates.Date= T1.HitsStartTime
	
		UNION
	
		SELECT [DATE], 0 as Hits, Visits, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''day'') AS Dates
		LEFT JOIN
			(SELECT HitsStartTime, SUM(HitsCount) as Visits, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @VisitType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_DayHits ON Analytics_Statistics.StatisticsID = Analytics_DayHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
		) AS T2
		ON Dates.Date=T2.HitsStartTime
	) as T3
	GROUP BY T3.[DATE], T3.ABVariantDisplayName
	
	-- Fill in missing values
	-- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative rates are calculated the right way	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, 0, T2.Name, NULL, NULL, NULL, NULL
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of rates
	DECLARE @RunningVisits DECIMAL(20,5)
	SET @RunningVisits = 0

	DECLARE @RunningHits DECIMAL(20,5)
	SET @RunningHits = 0

	DECLARE @LastName NVARCHAR(MAX)
		
	-- Sum running totals and rates using ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningVisits = RunningVisits =  ISNULL(Visits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningVisits,0) ELSE 0 END,
			@RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
			CumulativeRate = @RunningHits / NULLIF(@RunningVisits, 0) * 100,
			DayWiseRate = Hits / NULLIF(Visits,0) * 100,
			@LastName = Name

	-- Prepare for PIVOT - delete remaining columns
	ALTER TABLE #AnalyticsTempTable DROP COLUMN Visits
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningVisits

	DECLARE @engineEdition int;
	SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

	-- Drop columns based on graph type
	IF @GraphType = ''DayWise''
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = DayWiseRate WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			exec(''exec tempdb..sp_rename ''''#AnalyticsTempTable.DayWiseRate'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END
	ELSE IF @GraphType = ''Cumulative''
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = CumulativeRate WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			exec(''exec tempdb..sp_rename ''''#AnalyticsTempTable.CumulativeRate'''', ''''Hits'''', ''''COLUMN'''''')		
		END
	END    	
	ELSE -- GraphType is everything else -> use DayWise version	
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = DayWiseRate WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits

			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			exec(''exec tempdb..sp_rename ''''#AnalyticsTempTable.DayWiseRate'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END
	
	EXEC Proc_Analytics_Pivot ''day''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY%</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxisformat>{0.0\%}</yaxisformat><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '1f9bcfe1-1327-406f-8258-d50a0139317b', getDate(), NULL, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '661973b3-2247-424a-8b1c-939bb441eadf')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '-- This SQL script is used for getting conversion counts.
    -- Required variables are:
    -- @FromDate, @ToDate - used for specifying date range
    -- @GraphType - can be Cumulative or DayWise
    -- @TestName - ABTestName
    -- @ABTestID - ABTestID
    -- @ABTestCulture - ABTestCulture
    -- @ConversionName - selected conversion or empty for all conversions
    -- @VariationName - selected variation (GUID) or empty for all variations
-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

    EXEC Proc_Analytics_RemoveTempTable
    CREATE TABLE #AnalyticsTempTable (
     StartTime DATETIME,
     Hits INT,
     Name NVARCHAR(300),
     RunningHits INT
    );

    SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'');

    INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
    SELECT [DATE], Hits, ABVariantDisplayName
    FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''day'') AS Dates
    LEFT JOIN
    (
        SELECT HitsStartTime, SUM(HitsCount) as Hits, ABVariantDisplayName
        FROM OM_ABVariantData
        JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
        JOIN Analytics_DayHits ON Analytics_Statistics.StatisticsID = Analytics_DayHits.HitsStatisticsID
        WHERE ABVariantTestID = @ABTestID AND StatisticsSiteID = @CMSContextCurrentSiteID
        AND (@VariationName IS NULL OR @VariationName IN (CAST(ABVariantGUID AS char(36)), ''''))
        AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
        AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR @ABTestCulture IS NULL OR @ABTestCulture IN (StatisticsObjectCulture, ''''))
        GROUP BY HitsStartTime, ABVariantDisplayName
    ) as X on [DATE] = HitsStartTime
    

    -- Fill in missing values
    -- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative hits are calculated the right way    
    INSERT INTO #AnalyticsTempTable
        SELECT T1.StartTime, 0, T2.Name, 0
        FROM #AnalyticsTempTable as T1
        JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL
        WHERE NOT EXISTS
            (
            SELECT T3.StartTime, T3.Name
            FROM #AnalyticsTempTable as T3
            WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
            )
        GROUP BY T1.StartTime, T2.Name

    -- Delete NULL values
    DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

    -- Declare variables for calculation of hits
    DECLARE @RunningHits INT
    SET @RunningHits = 0

    DECLARE @LastName NVARCHAR(MAX)
        
    -- Sum running hits using the ''quirky update''
    -- Uses view to define order
    ;WITH TempView AS
    (
        SELECT TOP (2147483647) *
        FROM #AnalyticsTempTable
        ORDER BY
                Name, StartTime
    )
    UPDATE TempView
    SET     @RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
            @LastName = Name


    -- Drop columns based on graph type
    IF @GraphType = ''Cumulative''
    BEGIN    
        DECLARE @engineEdition int;
        SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

        IF @engineEdition = 5
        BEGIN
            ---------------
            -- Azure SQL --
            ---------------
            -- Rename using the update, because Azure do not support rename of columns after CTE
            UPDATE #AnalyticsTempTable SET Hits = RunningHits WHERE Name = Name AND StartTime = StartTime
            EXEC(''ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits'')
        END
        ELSE BEGIN
            ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits

            -- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''    
            exec(''exec tempdb..sp_rename ''''#AnalyticsTempTable.RunningHits'''', ''''Hits'''', ''''COLUMN'''''')            
        END
    END
    ELSE
    BEGIN
        EXEC(''ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits'')
    END          

    EXEC Proc_Analytics_Pivot ''day''
    EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', 'ab98e4ca-f7e5-4fd7-97b2-0a305a8f2b15', getDate(), NULL, '')

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph_detail', 'graph_detail', 'EXEC Proc_Analytics_RemoveTempTable
CREATE TABLE #AnalyticsTempTable (
  StartTime DATETIME,
  Hits INT,
  Name NVARCHAR(300)
);

 SET @FromDate ={%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'');
  INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)

   SELECT [Date] AS StartTime ,T1.Hits AS Hits  ,T1.Name AS Name
FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''Day'') AS Dates
 LEFT JOIN
  (SELECT HitsStartTime AS Interval, SUM(HitsCount) AS Hits ,ABVariantDisplayName AS Name
  FROM Analytics_Statistics 
  INNER JOIN Analytics_DayHits ON StatisticsID = Analytics_DayHits.HitsStatisticsID
  INNER JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  INNER JOIN OM_ABTest ON ABTestID = ABVariantTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  WHERE StatisticsSiteID = @CMSContextCurrentSiteID   AND  StatisticsCode LIKE ''abconversion;%'' 
  AND (@TestName IS NULL OR @TestName = ''''  OR  @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14))
  AND (@ConversionName IS NULL OR @ConversionName = '''' OR @ConversionName = StatisticsObjectName)
  GROUP BY HitsStartTime,ABVariantDisplayName
  ) AS T1
 ON Dates.Date=T1.Interval;
  
  EXEC Proc_Analytics_Pivot ''day''
  EXEC Proc_Analytics_RemoveTempTable', 0, '', @graphReportID, 'Conversions count', '', '', 600, 400, 100, '<CustomData><barorientation>Horizontal</barorientation><baroverlay>False</baroverlay><columnwidth>20</columnwidth><displayitemvalue>True</displayitemvalue><displaylegend>True</displaylegend><exportenabled>True</exportenabled><itemvalueformat>{%Format(ToDouble(pval, &quot;0.0&quot;), &quot;{0:0.0}&quot;)|(identity)GlobalAdministrator|(hash)5551e18976364394c0ee130476cd25d1d35fc90b9ab6212932d2c48efee71aae%}% ({%yval%})</itemvalueformat><legendinside>False</legendinside><legendposition>Right</legendposition><legendtitle>Variants</legendtitle><linedrawinstyle>Line</linedrawinstyle><pieshowpercentage>False</pieshowpercentage><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesgradient>None</seriesgradient><seriesitemnameformat>d</seriesitemnameformat><seriesitemtooltip>{%ser%}</seriesitemtooltip><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', 'd366af72-be8a-42e4-9213-b1188201af7e', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '73f30508-1bbc-450b-8d48-2829b2b00eec')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '	-- This SQL script is used for getting conversion values.
	-- Required variables are:
	-- @FromDate, @ToDate - used for specifying date range
	-- @GraphType - can be Cumulative or dayWise
	-- @TestName - ABTestName
	-- @ABTestID - ABTestID
	-- @ABTestCulture - ABTestCulture
	-- @ConversionName - selected conversion or empty for all conversions
	-- @VariationName - selected variation (GUID) or empty for all variations
    -- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)
	
	EXEC Proc_Analytics_RemoveTempTable
	CREATE TABLE #AnalyticsTempTable (
	  StartTime DATETIME,
	  Hits INT,
	  Name NVARCHAR(300),
	  RunningHits INT
	)

	SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'')

	INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
	SELECT [DATE], Hits, ABVariantDisplayName
	FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''day'') AS Dates
	LEFT JOIN 
	(
		SELECT HitsStartTime, SUM(HitsValue) as Hits, ABVariantDisplayName
		FROM OM_ABVariantData 
		JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
		JOIN Analytics_dayHits ON Analytics_Statistics.StatisticsID = Analytics_dayHits.HitsStatisticsID
		WHERE ABVariantTestID = @ABTestID AND StatisticsSiteID = @CMSContextCurrentSiteID
		AND (@VariationName IS NULL OR @VariationName IN (CAST(ABVariantGUID AS char(36)), ''''))
		AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
		AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR @ABTestCulture IS NULL OR @ABTestCulture IN (StatisticsObjectCulture, ''''))
		GROUP BY HitsStartTime, ABVariantDisplayName
	) as X on [DATE] = HitsStartTime
	

	-- Fill in missing values
	-- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative hits are calculated the right way	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, T2.Name, 0
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of hits
	DECLARE @RunningHits INT
	SET @RunningHits = 0

	DECLARE @LastName NVARCHAR(MAX)
		

	-- Sum running hits using the ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
			@LastName = Name;

	-- Drop columns based on graph type
	IF @GraphType = ''Cumulative''
	BEGIN	
		DECLARE @engineEdition int;
		SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = RunningHits WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.RunningHits'''', ''''Hits'''', ''''COLUMN'''''')			
		END
	END
	ELSE
	BEGIN
		ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
	END    	

	EXEC Proc_Analytics_Pivot ''day''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '06eb886d-e8d1-41b6-8293-713254a66ab6', getDate(), NULL, '')

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph_detail', 'graph_detail', 'EXEC Proc_Analytics_RemoveTempTable
CREATE TABLE #AnalyticsTempTable (
  StartTime DATETIME,
  Hits INT,
  Name NVARCHAR(300)
);

 SET @FromDate ={%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'');
  INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
   SELECT [Date] AS StartTime ,T1.Hits AS Hits  ,T1.Name AS Name

FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''Day'') AS Dates
 LEFT JOIN
  (SELECT HitsStartTime AS Interval, SUM(HitsValue) AS Hits ,ABVariantDisplayName AS Name
  FROM Analytics_Statistics 
  INNER JOIN Analytics_DayHits ON StatisticsID = Analytics_DayHits.HitsStatisticsID
  INNER JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  INNER JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  WHERE StatisticsSiteID = @CMSContextCurrentSiteID   AND  StatisticsCode LIKE ''abconversion;%'' 
  AND (@TestName IS NULL OR @TestName = ''''  OR  @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14))
  AND (@ConversionName IS NULL OR @ConversionName = '''' OR @ConversionName = StatisticsObjectName)
  GROUP BY HitsStartTime,ABVariantDisplayName
  ) AS T1
 ON Dates.Date=T1.Interval;
  
  EXEC Proc_Analytics_Pivot ''day''
  EXEC Proc_Analytics_RemoveTempTable', 0, '', @graphReportID, 'Conversions value', '', '', 600, 400, 100, '<CustomData><barorientation>Horizontal</barorientation><baroverlay>False</baroverlay><displayitemvalue>True</displayitemvalue><displaylegend>True</displaylegend><exportenabled>True</exportenabled><itemvalueformat>{%Format(ToDouble(pval, &quot;0.0&quot;), &quot;{0:0.0}&quot;)|(identity)GlobalAdministrator|(hash)5551e18976364394c0ee130476cd25d1d35fc90b9ab6212932d2c48efee71aae%}% ({%yval%})</itemvalueformat><legendinside>False</legendinside><legendposition>Right</legendposition><legendtitle>Variants</legendtitle><linedrawinstyle>Line</linedrawinstyle><pieshowpercentage>False</pieshowpercentage><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesgradient>None</seriesgradient><seriesitemnameformat>d</seriesitemnameformat><seriesitemtooltip>{%ser%}</seriesitemtooltip><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '663dc240-f623-4e66-975b-2f512509fd76', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '7d1b4de6-f27e-465b-ab9a-0b6f4dbc9278')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '-- This SQL script is used for getting conversion counts.
	-- Required variables are:
	-- @FromDate, @ToDate - used for specifying date range
	-- @GraphType - can be Cumulative or DayWise
	-- @TestName - ABTestName
	-- @ABTestID - ABTestID
	-- @ABTestCulture - ABTestCulture
	-- @ConversionName - selected conversion or empty for all conversions
	-- @VariationName - selected variation (GUID) or empty for all variations
    -- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

	EXEC Proc_Analytics_RemoveTempTable
	CREATE TABLE #AnalyticsTempTable (
	  StartTime DATETIME,
	  Hits INT,
	  Name NVARCHAR(300),
	  RunningHits INT
	);

	SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'');

	INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
	SELECT [DATE], Hits, ABVariantDisplayName
	FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''month'') AS Dates
	LEFT JOIN 
	(
		SELECT HitsStartTime, SUM(HitsCount) as Hits, ABVariantDisplayName
		FROM OM_ABVariantData 
		JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
		JOIN Analytics_MonthHits ON Analytics_Statistics.StatisticsID = Analytics_MonthHits.HitsStatisticsID
		WHERE ABVariantTestID = @ABTestID AND StatisticsSiteID = @CMSContextCurrentSiteID
		AND (@VariationName IS NULL OR @VariationName IN (CAST(ABVariantGUID AS char(36)), ''''))
		AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
		AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR @ABTestCulture IS NULL OR @ABTestCulture IN (StatisticsObjectCulture, ''''))
		GROUP BY HitsStartTime, ABVariantDisplayName
	) as X on [DATE] = HitsStartTime
	

	-- Fill in missing values
	-- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative hits are calculated the right way	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, T2.Name, 0
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of hits
	DECLARE @RunningHits INT
	SET @RunningHits = 0

	DECLARE @LastName NVARCHAR(MAX)
		
	-- Sum running hits using the ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
			@LastName = Name


	-- Drop columns based on graph type
	IF @GraphType = ''Cumulative''
	BEGIN	
		DECLARE @engineEdition int;
		SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = RunningHits WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.RunningHits'''', ''''Hits'''', ''''COLUMN'''''')			
		END
	END
	ELSE
	BEGIN
		ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
	END    	  	

	EXEC Proc_Analytics_Pivot ''month''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><columnwidth>20</columnwidth><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>y</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '6a6a10d2-7d31-48b0-9893-abedd9d7e051', getDate(), NULL, '')

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph_detail', 'graph_detail', 'EXEC Proc_Analytics_RemoveTempTable
CREATE TABLE #AnalyticsTempTable (
  StartTime DATETIME,
  Hits INT,
  Name NVARCHAR(300)
);

 SET @FromDate ={%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'');
  INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)

   SELECT [Date] AS StartTime ,T1.Hits AS Hits  ,T1.Name AS Name
FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''Month'') AS Dates
 LEFT JOIN
  (SELECT HitsStartTime AS Interval, SUM(HitsCount) AS Hits ,ABVariantDisplayName AS Name
  FROM Analytics_Statistics 
  INNER JOIN Analytics_MonthHits ON StatisticsID = Analytics_MonthHits.HitsStatisticsID
  INNER JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  INNER JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  WHERE StatisticsSiteID = @CMSContextCurrentSiteID   AND  StatisticsCode LIKE ''abconversion;%'' 
  AND (@TestName IS NULL OR @TestName = ''''  OR  @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14))
  AND (@ConversionName IS NULL OR @ConversionName = '''' OR @ConversionName = StatisticsObjectName)
  GROUP BY HitsStartTime,ABVariantDisplayName
  ) AS T1
 ON Dates.Date=T1.Interval;
  
  EXEC Proc_Analytics_Pivot ''month''
  EXEC Proc_Analytics_RemoveTempTable', 0, '', @graphReportID, 'Conversion count detail', NULL, NULL, NULL, NULL, NULL, '<CustomData><displaylegend>True</displaylegend><exportenabled>True</exportenabled><itemvalueformat>{%Format(ToDouble(pval, &quot;0.0&quot;), &quot;{0:0.0}&quot;)|(identity)GlobalAdministrator|(hash)5551e18976364394c0ee130476cd25d1d35fc90b9ab6212932d2c48efee71aae%}% ({%yval%})</itemvalueformat><legendtitle>Variants</legendtitle><querynorecordtext>No data found</querynorecordtext><seriesitemnameformat>y</seriesitemnameformat><seriesitemtooltip>{%ser%}</seriesitemtooltip><subscriptionenabled>True</subscriptionenabled><yaxistitleposition>Center</yaxistitleposition></CustomData>', '2f94c2de-3f18-47de-ba33-3993896487f4', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '86686015-2bdb-430a-bf26-0adabc4eca68')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('Graph', 'graph', '-- This SQL script is used for getting average conversion value.
-- Required variables are:
-- @FromDate, @ToDate - used for specifying date range
-- @GraphType - can be Cumulative or DayWise
-- @TestName - ABTestName
-- @ABTestID - ABTestID
-- @ABTestCulture - ABTestCulture
-- @ConversionName - selected conversion or empty for all conversions
-- @VariationName - selected variation (GUID) or empty for all variations
-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

EXEC Proc_Analytics_RemoveTempTable	
	CREATE TABLE #AnalyticsTempTable (
		StartTime DATETIME,
		HitsCount DECIMAL(20,5),
		HitsValue FLOAT,
		Name NVARCHAR(300) COLLATE DATABASE_DEFAULT,
		RunningHitsCount DECIMAL(20,5),
		RunningHitsValue FLOAT,
		CumulativeAverage FLOAT,
		DayWiseAverage FLOAT,
		Hits FLOAT
	);

    SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'');
 
	-- Get conversion value and count
	INSERT INTO #AnalyticsTempTable (StartTime, HitsCount, HitsValue, Name)
		SELECT [DATE], HitsCount, HitsValue, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''day'') AS Dates
		LEFT JOIN
			(
			SELECT HitsStartTime,  Sum(HitsCount) as HitsCount, SUM(HitsValue) as HitsValue, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_DayHits ON Analytics_Statistics.StatisticsID = Analytics_DayHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
			) AS T1
		ON Dates.Date= T1.HitsStartTime	

	-- Select dates and names missing in the original data to fill the gaps
    -- This is needed to ensure that cumulative average conversion values are calculated correctly	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, 0, T2.Name, NULL, NULL, NULL, NULL, NULL
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of average value
	DECLARE @RunningHitsCount DECIMAL(20,5)
	SET @RunningHitsCount = 0

	DECLARE @RunningHitsValue FLOAT
	SET @RunningHitsValue = 0

	DECLARE @LastName NVARCHAR(MAX)
		
	-- Sum running totals and average conversion values using ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningHitsCount = RunningHitsCount =  ISNULL(HitsCount,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHitsCount,0) ELSE 0 END,
			@RunningHitsValue = RunningHitsValue = ISNULL(HitsValue,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHitsValue,0) ELSE 0 END,
			CumulativeAverage = @RunningHitsValue / NULLIF(@RunningHitsCount, 0),
			DayWiseAverage = HitsValue / NULLIF(HitsCount,0),
			@LastName = Name

	-- Prepare for PIVOT - delete all remaining columns
	ALTER TABLE #AnalyticsTempTable DROP COLUMN HitsValue
	ALTER TABLE #AnalyticsTempTable DROP COLUMN HitsCount
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHitsValue
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHitsCount

	DECLARE @engineEdition int;
	SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

	-- Drop columns based on graph type
	IF @GraphType = ''DayWise''
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage			
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = DayWiseAverage WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			exec(''exec tempdb..sp_rename ''''#AnalyticsTempTable.DayWiseAverage'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END    	
	ELSE -- GraphType is everything else -> use Cumulative version	
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage			
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = CumulativeAverage WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			exec(''exec tempdb..sp_rename ''''#AnalyticsTempTable.CumulativeAverage'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END

	EXEC Proc_Analytics_Pivot ''day''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No results. The test has no visits or the selected filter combination yields no results.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '9be0ab2f-a355-4d31-a7c0-85e8136b8847', getDate(), NULL, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '9567651c-efe0-4f8a-9a6b-c36a126923dc')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '	-- This SQL script is used for getting conversion values.
	-- Required variables are:
	-- @FromDate, @ToDate - used for specifying date range
	-- @GraphType - can be Cumulative or dayWise
	-- @TestName - ABTestName
	-- @ABTestID - ABTestID
	-- @ABTestCulture - ABTestCulture
	-- @ConversionName - selected conversion or empty for all conversions
	-- @VariationName - selected variation (GUID) or empty for all variations
    -- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)
	
	EXEC Proc_Analytics_RemoveTempTable
	CREATE TABLE #AnalyticsTempTable (
	  StartTime DATETIME,
	  Hits INT,
	  Name NVARCHAR(300),
	  RunningHits INT
	)

	SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'')

	INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
	SELECT [DATE], Hits, ABVariantDisplayName
	FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''month'') AS Dates
	LEFT JOIN 
	(
		SELECT HitsStartTime, SUM(HitsValue) as Hits, ABVariantDisplayName
		FROM OM_ABVariantData 
		JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
		JOIN Analytics_MonthHits ON Analytics_Statistics.StatisticsID = Analytics_MonthHits.HitsStatisticsID
		WHERE ABVariantTestID = @ABTestID AND StatisticsSiteID = @CMSContextCurrentSiteID
		AND (@VariationName IS NULL OR @VariationName IN (CAST(ABVariantGUID AS char(36)), ''''))
		AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
		AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR @ABTestCulture IS NULL OR @ABTestCulture IN (StatisticsObjectCulture, ''''))
		GROUP BY HitsStartTime, ABVariantDisplayName
	) as X on [DATE] = HitsStartTime
	

	-- Fill in missing values
	-- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative hits are calculated the right way	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, T2.Name, 0
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of hits
	DECLARE @RunningHits INT
	SET @RunningHits = 0

	DECLARE @LastName NVARCHAR(MAX)
		

	-- Sum running hits using the ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
			@LastName = Name;

	-- Drop columns based on graph type
	IF @GraphType = ''Cumulative''
	BEGIN	
		DECLARE @engineEdition int;
		SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = RunningHits WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.RunningHits'''', ''''Hits'''', ''''COLUMN'''''')			
		END
	END
	ELSE
	BEGIN
		ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
	END    	

	EXEC Proc_Analytics_Pivot ''month''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>y</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '139aaef2-b31b-45de-acee-4b86054eb5ea', getDate(), NULL, '')

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph_detail', 'graph_detail', 'EXEC Proc_Analytics_RemoveTempTable
CREATE TABLE #AnalyticsTempTable (
  StartTime DATETIME,
  Hits INT,
  Name NVARCHAR(300)
);

 SET @FromDate ={%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'');
  INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
   SELECT [Date] AS StartTime ,T1.Hits AS Hits  ,T1.Name AS Name

FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''Month'') AS Dates
 LEFT JOIN
  (SELECT HitsStartTime AS Interval, SUM(HitsValue) AS Hits ,ABVariantDisplayName AS Name
  FROM Analytics_Statistics 
  INNER JOIN Analytics_MonthHits ON StatisticsID = Analytics_MonthHits.HitsStatisticsID
  INNER JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  INNER JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  WHERE StatisticsSiteID = @CMSContextCurrentSiteID   AND  StatisticsCode LIKE ''abconversion;%'' 
  AND (@TestName IS NULL OR @TestName = ''''  OR  @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14))
  AND (@ConversionName IS NULL OR @ConversionName = '''' OR @ConversionName = StatisticsObjectName)
  GROUP BY HitsStartTime,ABVariantDisplayName
  ) AS T1
 ON Dates.Date=T1.Interval;
  
  EXEC Proc_Analytics_Pivot ''month''
  EXEC Proc_Analytics_RemoveTempTable', 0, '', @graphReportID, 'Conversions value', '', '', 600, 400, 100, '<CustomData><barorientation>Horizontal</barorientation><baroverlay>False</baroverlay><displayitemvalue>True</displayitemvalue><displaylegend>True</displaylegend><exportenabled>True</exportenabled><itemvalueformat>{%Format(ToDouble(pval, &quot;0.0&quot;), &quot;{0:0.0}&quot;)|(identity)GlobalAdministrator|(hash)5551e18976364394c0ee130476cd25d1d35fc90b9ab6212932d2c48efee71aae%}% ({%yval%})</itemvalueformat><legendinside>False</legendinside><legendposition>Right</legendposition><legendtitle>Variants</legendtitle><linedrawinstyle>Line</linedrawinstyle><pieshowpercentage>False</pieshowpercentage><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesgradient>None</seriesgradient><seriesitemnameformat>y</seriesitemnameformat><seriesitemtooltip>{%ser%}</seriesitemtooltip><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><xaxisformat>y</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '71623e82-d137-4eb8-8e90-ddc69334756b', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '9908d225-250d-4bc7-ae7a-97d8ebf3b59b')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('Graph', 'graph', '-- This SQL script is used for getting average conversion value.
-- Required variables are:
-- @FromDate, @ToDate - used for specifying date range
-- @GraphType - can be Cumulative or DayWise
-- @TestName - ABTestName
-- @ABTestID - ABTestID
-- @ABTestCulture - ABTestCulture
-- @ConversionName - selected conversion or empty for all conversions
-- @VariationName - selected variation (GUID) or empty for all variations
-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

EXEC Proc_Analytics_RemoveTempTable	
	CREATE TABLE #AnalyticsTempTable (
		StartTime DATETIME,
		HitsCount DECIMAL(20,5),
		HitsValue FLOAT,
		Name NVARCHAR(300) COLLATE DATABASE_DEFAULT,
		RunningHitsCount DECIMAL(20,5),
		RunningHitsValue FLOAT,
		CumulativeAverage FLOAT,
		DayWiseAverage FLOAT,
		Hits FLOAT
	);

    SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'');
 
	-- Get conversion value and count
	INSERT INTO #AnalyticsTempTable (StartTime, HitsCount, HitsValue, Name)
		SELECT [DATE], HitsCount, HitsValue, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''week'') AS Dates
		LEFT JOIN
			(
			SELECT HitsStartTime,  Sum(HitsCount) as HitsCount, SUM(HitsValue) as HitsValue, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_WeekHits ON Analytics_Statistics.StatisticsID = Analytics_WeekHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
			) AS T1
		ON Dates.Date= T1.HitsStartTime	

	-- Select dates and names missing in the original data to fill the gaps
    -- This is needed to ensure that cumulative average conversion values are calculated correctly	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, 0, T2.Name, NULL, NULL, NULL, NULL, NULL
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of average value
	DECLARE @RunningHitsCount DECIMAL(20,5)
	SET @RunningHitsCount = 0

	DECLARE @RunningHitsValue FLOAT
	SET @RunningHitsValue = 0

	DECLARE @LastName NVARCHAR(MAX)
		
	-- Sum running totals and average conversion values using ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningHitsCount = RunningHitsCount =  ISNULL(HitsCount,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHitsCount,0) ELSE 0 END,
			@RunningHitsValue = RunningHitsValue = ISNULL(HitsValue,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHitsValue,0) ELSE 0 END,
			CumulativeAverage = @RunningHitsValue / NULLIF(@RunningHitsCount, 0),
			DayWiseAverage = HitsValue / NULLIF(HitsCount,0),
			@LastName = Name

	-- Prepare for PIVOT - delete all remaining columns
	ALTER TABLE #AnalyticsTempTable DROP COLUMN HitsValue
	ALTER TABLE #AnalyticsTempTable DROP COLUMN HitsCount
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHitsValue
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHitsCount

	DECLARE @engineEdition int;
	SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

	-- Drop columns based on graph type
	IF @GraphType = ''DayWise''
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage					
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = DayWiseAverage WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.DayWiseAverage'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END    	
	ELSE -- GraphType is everything else -> use Cumulative version	
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage					
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = CumulativeAverage WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeAverage
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseAverage
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.CumulativeAverage'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END

	EXEC Proc_Analytics_Pivot ''week''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No results. The test has no visits or the selected filter combination yields no results.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', 'f8c8fa65-bd08-4ed6-aa38-13767a6ca261', getDate(), NULL, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = 'c30a7bd2-6c53-4c77-bb0e-afcce8384d16')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '	-- This SQL script is used for getting conversion values.
	-- Required variables are:
	-- @FromDate, @ToDate - used for specifying date range
	-- @GraphType - can be Cumulative or dayWise
	-- @TestName - ABTestName
	-- @ABTestID - ABTestID
	-- @ABTestCulture - ABTestCulture
	-- @ConversionName - selected conversion or empty for all conversions
	-- @VariationName - selected variation (GUID) or empty for all variations
    -- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)
	
	EXEC Proc_Analytics_RemoveTempTable
	CREATE TABLE #AnalyticsTempTable (
	  StartTime DATETIME,
	  Hits INT,
	  Name NVARCHAR(300),
	  RunningHits INT
	)

	SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'')

	INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
	SELECT [DATE], Hits, ABVariantDisplayName
	FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''week'') AS Dates
	LEFT JOIN 
	(
		SELECT HitsStartTime, SUM(HitsValue) as Hits, ABVariantDisplayName
		FROM OM_ABVariantData
		JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
		JOIN Analytics_WeekHits ON Analytics_Statistics.StatisticsID = Analytics_WeekHits.HitsStatisticsID
		WHERE ABVariantTestID = @ABTestID AND StatisticsSiteID = @CMSContextCurrentSiteID
		AND (@VariationName IS NULL OR @VariationName IN (CAST(ABVariantGUID AS char(36)), ''''))
		AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
		AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR @ABTestCulture IS NULL OR @ABTestCulture IN (StatisticsObjectCulture, ''''))
		GROUP BY HitsStartTime, ABVariantDisplayName
	) as X on [DATE] = HitsStartTime
	

	-- Fill in missing values
	-- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative hits are calculated the right way	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, T2.Name, 0
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of hits
	DECLARE @RunningHits INT
	SET @RunningHits = 0

	DECLARE @LastName NVARCHAR(MAX)
		

	-- Sum running hits using the ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
			@LastName = Name;

	-- Drop columns based on graph type
	IF @GraphType = ''Cumulative''
	BEGIN	
		DECLARE @engineEdition int;
		SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = RunningHits WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.RunningHits'''', ''''Hits'''', ''''COLUMN'''''')			
		END
	END
	ELSE
	BEGIN
		ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
	END    	

	EXEC Proc_Analytics_Pivot ''week''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>{yyyy}</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '1acb3067-cffb-41ca-b3b1-e3b32a68659f', getDate(), NULL, '')

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph_detail', 'graph_detail', 'EXEC Proc_Analytics_RemoveTempTable
CREATE TABLE #AnalyticsTempTable (
  StartTime DATETIME,
  Hits INT,
  Name NVARCHAR(300)
);

 SET @FromDate ={%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'');
  INSERT INTO #AnalyticsTempTable (StartTime, Hits, Name)
   SELECT [Date] AS StartTime ,T1.Hits AS Hits  ,T1.Name AS Name

FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''Week'') AS Dates
 LEFT JOIN
  (SELECT HitsStartTime AS Interval, SUM(HitsValue) AS Hits ,ABVariantDisplayName AS Name
  FROM Analytics_Statistics 
  INNER JOIN Analytics_WeekHits ON StatisticsID = Analytics_WeekHits.HitsStatisticsID
  INNER JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  INNER JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  WHERE StatisticsSiteID = @CMSContextCurrentSiteID   AND  StatisticsCode LIKE ''abconversion;%'' 
  AND (@TestName IS NULL OR @TestName = ''''  OR  @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14))
  AND (@ConversionName IS NULL OR @ConversionName = '''' OR @ConversionName = StatisticsObjectName)
  GROUP BY HitsStartTime,ABVariantDisplayName
  ) AS T1
 ON Dates.Date=T1.Interval;
  
  EXEC Proc_Analytics_Pivot ''week''
  EXEC Proc_Analytics_RemoveTempTable', 0, '', @graphReportID, 'Conversions value', '', '', 600, 400, 100, '<CustomData><barorientation>Horizontal</barorientation><baroverlay>False</baroverlay><displayitemvalue>True</displayitemvalue><displaylegend>True</displaylegend><exportenabled>True</exportenabled><itemvalueformat>{%Format(ToDouble(pval, &quot;0.0&quot;), &quot;{0:0.0}&quot;)|(identity)GlobalAdministrator|(hash)5551e18976364394c0ee130476cd25d1d35fc90b9ab6212932d2c48efee71aae%}% ({%yval%})</itemvalueformat><legendinside>False</legendinside><legendposition>Right</legendposition><legendtitle>Variants</legendtitle><linedrawinstyle>Line</linedrawinstyle><pieshowpercentage>False</pieshowpercentage><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesgradient>None</seriesgradient><seriesitemtooltip>{%ser%}</seriesitemtooltip><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><xaxisformat>{yyyy}</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '09c72577-206d-4551-9233-7495ad1c621f', getDate(), 1, '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = 'ed3cb1dc-4b0d-48b7-b45d-a30cb9944058')
IF @graphReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportGraph] ([GraphName], [GraphDisplayName], [GraphQuery], [GraphQueryIsStoredProcedure], [GraphType], [GraphReportID], [GraphTitle], [GraphXAxisTitle], [GraphYAxisTitle], [GraphWidth], [GraphHeight], [GraphLegendPosition], [GraphSettings], [GraphGUID], [GraphLastModified], [GraphIsHtml], [GraphConnectionString])
 VALUES ('graph', 'graph', '	-- This SQL script is used for getting conversion rates.
	-- Required variables are:
	-- @FromDate, @ToDate - used for specifying date range
	-- @GraphType - can be Cumulative or DayWise
	-- @TestName - ABTestName
	-- @ABTestID - ABTestID
	-- @ABTestCulture - ABTestCulture
	-- @ConversionName - selected conversion or empty for all conversions
	-- @VariationName - selected variation (GUID) or empty for all variations
	-- @ConversionType - selected conversion type (abconversion, absessionconversionfirst, absessionconversion%)

	EXEC Proc_Analytics_RemoveTempTable
	CREATE TABLE #AnalyticsTempTable (
		StartTime DATETIME,
		Hits DECIMAL(20,5),
		Visits DECIMAL(20,5),
		Name NVARCHAR(300) COLLATE DATABASE_DEFAULT,
		RunningHits DECIMAL(20,5),
		RunningVisits DECIMAL(20,5),
		CumulativeRate DECIMAL(20,5),
		DayWiseRate DECIMAL(20,5)
	);

	DECLARE @VisitType NVARCHAR(MAX)
    SET @VisitType = ''abvisit%''

    IF @ConversionType = ''absessionconversionfirst''
	BEGIN
		SET @VisitType = ''abvisitfirst''
    END

	SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'');
 
	-- Get hits and visits
	INSERT INTO #AnalyticsTempTable (StartTime, Hits, Visits, Name)
	SELECT [Date], SUM(Hits) as Hits, SUM(Visits) as Visits, ABVariantDisplayName FROM
	(
		SELECT [DATE], Hits, 0 as Visits, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''week'') AS Dates
		LEFT JOIN
			(
			SELECT HitsStartTime, SUM(HitsCount) as Hits, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @ConversionType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_WeekHits ON Analytics_Statistics.StatisticsID = Analytics_WeekHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@ConversionName IS NULL OR @ConversionName IN ('''',StatisticsObjectName) OR '';'' + @ConversionName + '';'' LIKE ''%;'' + StatisticsObjectName + '';%'')
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
			) AS T1
		ON Dates.Date= T1.HitsStartTime
	
		UNION
	
		SELECT [DATE], 0 as Hits, Visits, ABVariantDisplayName
		FROM {%DatabaseSchema%}.Func_Analytics_EnsureDates (@FromDate,@ToDate,''week'') AS Dates
		LEFT JOIN
			(SELECT HitsStartTime, SUM(HitsCount) as Visits, ABVariantDisplayName
			FROM OM_ABVariantData		
			INNER JOIN Analytics_Statistics ON Analytics_Statistics.StatisticsCode LIKE @VisitType + '';'' + @TestName + '';'' + CAST(ABVariantGUID AS char(36))
			INNER JOIN Analytics_WeekHits ON Analytics_Statistics.StatisticsID = Analytics_WeekHits.HitsStatisticsID
			WHERE StatisticsSiteID = @CMSContextCurrentSiteID
			AND (ABVariantTestID = @ABTestID)			
			AND (@VariationName IS NULL OR @VariationName IN ('''', CAST(ABVariantGUID AS char(36))))
			AND (StatisticsObjectCulture IS NULL OR StatisticsObjectCulture = '''' OR StatisticsObjectCulture = @ABTestCulture OR @ABTestCulture IS NULL OR @ABTestCulture = '''')
			GROUP BY HitsStartTime, ABVariantDisplayName
		) AS T2
		ON Dates.Date=T2.HitsStartTime
	) as T3
	GROUP BY T3.[DATE], T3.ABVariantDisplayName
	
	-- Fill in missing values
	-- Select dates and names, which are missing in original data to fill gaps that are needed to ensure, that cumulative rates are calculated the right way	
	INSERT INTO #AnalyticsTempTable
		SELECT T1.StartTime, 0, 0, T2.Name, NULL, NULL, NULL, NULL
		FROM #AnalyticsTempTable as T1 
		JOIN #AnalyticsTempTable T2 ON T2.Name IS NOT NULL 
		WHERE NOT EXISTS 
			(
			SELECT T3.StartTime, T3.Name 
			FROM #AnalyticsTempTable as T3 
			WHERE T3.Name = T2.Name AND T3.StartTime = T1.StartTime
			) 
		GROUP BY T1.StartTime, T2.Name

	-- Delete NULL values
	DELETE FROM #AnalyticsTempTable WHERE Name IS NULL

	-- Declare variables for calculation of rates
	DECLARE @RunningVisits DECIMAL(20,5)
	SET @RunningVisits = 0

	DECLARE @RunningHits DECIMAL(20,5)
	SET @RunningHits = 0

	DECLARE @LastName NVARCHAR(MAX)
		
	-- Sum running totals and rates using ''quirky update''
	-- Uses view to define order
	;WITH TempView AS 
	(
		SELECT TOP (2147483647) *
		FROM    #AnalyticsTempTable
		ORDER BY
				Name, StartTime
	)            
	UPDATE  TempView
	SET    	@RunningVisits = RunningVisits =  ISNULL(Visits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningVisits,0) ELSE 0 END,
			@RunningHits = RunningHits = ISNULL(Hits,0) + CASE WHEN Name=@LastName THEN ISNULL(@RunningHits,0) ELSE 0 END,
			CumulativeRate = @RunningHits / NULLIF(@RunningVisits, 0) * 100,
			DayWiseRate = Hits / NULLIF(Visits,0) * 100,
			@LastName = Name

	-- Prepare for PIVOT - delete remaining columns
	ALTER TABLE #AnalyticsTempTable DROP COLUMN Visits
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningHits
	ALTER TABLE #AnalyticsTempTable DROP COLUMN RunningVisits

	DECLARE @engineEdition int;
	SET @engineEdition = CAST(SERVERPROPERTY(''EngineEdition'') AS INT);

	-- Drop columns based on graph type
	IF @GraphType = ''DayWise''
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate		
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = DayWiseRate WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits			
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.DayWiseRate'''', ''''Hits'''', ''''COLUMN'''''')
		END
	END
	ELSE --Cumulative
	BEGIN
		IF @engineEdition = 5 
		BEGIN 
			---------------
			-- Azure SQL --
			---------------
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate		
			-- Rename using the update, because Azure do not support rename of columns after CTE
			UPDATE #AnalyticsTempTable SET Hits = CumulativeRate WHERE Name = Name AND StartTime = StartTime
			ALTER TABLE #AnalyticsTempTable DROP COLUMN CumulativeRate
		END
		ELSE BEGIN
			ALTER TABLE #AnalyticsTempTable DROP COLUMN DayWiseRate
			ALTER TABLE #AnalyticsTempTable DROP COLUMN Hits		
			-- Calling in inner exec, because Azure does not allow to access tempdb using the ''temdb..''	
			EXEC(''exec tempdb..sp_rename ''''#AnalyticsTempTable.CumulativeRate'''', ''''Hits'''', ''''COLUMN'''''')		
		END
	END
	
	EXEC Proc_Analytics_Pivot ''week''
	EXEC Proc_Analytics_RemoveTempTable', 0, 'line', @graphReportID, '', '', '', 600, 500, 100, '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY%</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>{yyyy}</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxisformat>{0.0\%}</yaxisformat><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>', '52ef44c0-3166-4dda-a85e-2a10460ab43a', getDate(), NULL, '')

	
END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @tableReportID int;
SET @tableReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '265faf9e-0cdf-4f6d-80bc-b0e373da643f')
IF @tableReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportTable] ([TableName], [TableDisplayName], [TableQuery], [TableQueryIsStoredProcedure], [TableReportID], [TableSettings], [TableGUID], [TableLastModified], [TableConnectionString])
 VALUES ('table', 'table', 'SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'');
SET @ToDate = {%DatabaseSchema%}.Func_Analytics_EndDateTrim(@ToDate,''week'');

SELECT X.Name AS ''{$om.variant.tabletitle$}'',ISNULL (Y.Hits,0) AS ''{$om.selectedperiod$}'',
ISNULL(X.Hits,0) AS ''{$om.total$}''  FROM
(
SELECT ABVariantDisplayName AS Name, ISNULL(SUM (HitsCount),0) AS Hits,ABVariantGUID FROM Analytics_Statistics
  LEFT JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  LEFT JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  LEFT JOIN Analytics_WeekHits ON StatisticsID = HitsSTatisticsID    

 WHERE   StatisticsSiteID = @CMSContextCurrentSiteID AND StatisticsCode LIKE ''abconversion;%'' AND
 @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14) AND
 ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
 
 GROUP BY ABVariantDisplayName,ABVariantGUID
)
 AS X
LEFT JOIN (SELECT
  CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier) AS ABVariantGUID, SUM(HitsCount) AS Hits FROM Analytics_Statistics
  LEFT JOIN Analytics_WeekHits ON HitsStatisticsID = StatisticsID
  
  WHERE (StatisticsSiteID = @CMSContextCurrentSiteID) AND StatisticsCode LIKE ''abconversion;%''
    AND (HitsStartTime >= @FromDate) AND (HitsEndTime <= @ToDate)
    AND ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
     
  GROUP BY SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode))
)
AS Y ON X.ABVariantGUID = Y.ABVariantGUID

 
ORDER BY X.Hits Desc', 0, @tableReportID, '<CustomData><enablepaging>False</enablepaging><exportenabled>True</exportenabled><pagemode>1</pagemode><pagesize>15</pagesize><querynorecordtext>No data found</querynorecordtext><SkinID>ReportGridAnalytics</SkinID><subscriptionenabled>True</subscriptionenabled></CustomData>', 'b4032f7f-b1c0-498b-a04c-84f5586694b2', getDate(), '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @tableReportID int;
SET @tableReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '661973b3-2247-424a-8b1c-939bb441eadf')
IF @tableReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportTable] ([TableName], [TableDisplayName], [TableQuery], [TableQueryIsStoredProcedure], [TableReportID], [TableSettings], [TableGUID], [TableLastModified], [TableConnectionString])
 VALUES ('table', 'table', 'SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'');
SET @ToDate = {%DatabaseSchema%}.Func_Analytics_EndDateTrim(@ToDate,''day'');

SELECT X.Name AS ''{$om.variant.tabletitle$}'',ISNULL (Y.Hits,0) AS ''{$om.selectedperiod$}'',
ISNULL(X.Hits,0) AS ''{$om.total$}''  FROM
(
SELECT ABVariantDisplayName AS Name, ISNULL(SUM (HitsCount),0) AS Hits,ABVariantGUID FROM Analytics_Statistics
  LEFT JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  LEFT JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  LEFT JOIN Analytics_DayHits ON StatisticsID = HitsSTatisticsID    

 WHERE   StatisticsSiteID = @CMSContextCurrentSiteID AND StatisticsCode LIKE ''abconversion;%'' AND
 @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14) AND
 ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
 
 GROUP BY ABVariantDisplayName,ABVariantGUID
)
 AS X
LEFT JOIN (SELECT
  CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier) AS ABVariantGUID, SUM(HitsCount) AS Hits FROM Analytics_Statistics
  LEFT JOIN Analytics_DayHits ON HitsStatisticsID = StatisticsID
  
  WHERE (StatisticsSiteID = @CMSContextCurrentSiteID) AND StatisticsCode LIKE ''abconversion;%''
    AND (HitsStartTime >= @FromDate) AND (HitsEndTime <= @ToDate)
    AND ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
     
  GROUP BY SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode))
)
AS Y ON X.ABVariantGUID = Y.ABVariantGUID

 
ORDER BY X.Hits Desc', 0, @tableReportID, '<CustomData><enablepaging>False</enablepaging><exportenabled>True</exportenabled><pagemode>1</pagemode><pagesize>15</pagesize><querynorecordtext>No data found</querynorecordtext><SkinID>ReportGridAnalytics</SkinID><subscriptionenabled>True</subscriptionenabled></CustomData>', 'dfbc1deb-bfb8-4e11-99f3-ac7eb747d148', getDate(), '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @tableReportID int;
SET @tableReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '73f30508-1bbc-450b-8d48-2829b2b00eec')
IF @tableReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportTable] ([TableName], [TableDisplayName], [TableQuery], [TableQueryIsStoredProcedure], [TableReportID], [TableSettings], [TableGUID], [TableLastModified], [TableConnectionString])
 VALUES ('table', 'table', 'SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''day'');
SET @ToDate = {%DatabaseSchema%}.Func_Analytics_EndDateTrim(@ToDate,''day'');

SELECT X.Name AS ''{$om.variant.tabletitle$}'',ISNULL (Y.Hits,0) AS ''{$om.selectedperiod$}'',
ISNULL(X.Hits,0) AS ''{$om.total$}''  FROM
(
SELECT ABVariantDisplayName AS Name, ISNULL(SUM (HitsValue),0) AS Hits,ABVariantGUID FROM Analytics_Statistics
  LEFT JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  LEFT JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  LEFT JOIN Analytics_DayHits ON StatisticsID = HitsSTatisticsID    

 WHERE   StatisticsSiteID = @CMSContextCurrentSiteID AND StatisticsCode LIKE ''abconversion;%'' AND
 @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14) AND
 ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
 
 GROUP BY ABVariantDisplayName,ABVariantGUID
)
 AS X
LEFT JOIN (SELECT
  CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier) AS ABVariantGUID, SUM(HitsValue) AS Hits FROM Analytics_Statistics
  LEFT JOIN Analytics_DayHits ON HitsStatisticsID = StatisticsID
  
  WHERE (StatisticsSiteID = @CMSContextCurrentSiteID) AND StatisticsCode LIKE ''abconversion;%''
    AND (HitsStartTime >= @FromDate) AND (HitsEndTime <= @ToDate)
    AND ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
     
  GROUP BY SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode))
)
AS Y ON X.ABVariantGUID = Y.ABVariantGUID

 
ORDER BY X.Hits Desc', 0, @tableReportID, '<CustomData><enablepaging>False</enablepaging><exportenabled>True</exportenabled><pagemode>1</pagemode><pagesize>15</pagesize><querynorecordtext>No data found</querynorecordtext><SkinID>ReportGridAnalytics</SkinID><subscriptionenabled>True</subscriptionenabled></CustomData>', '524c6b89-9ebe-4d21-976b-878fa1811f90', getDate(), '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @tableReportID int;
SET @tableReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '7d1b4de6-f27e-465b-ab9a-0b6f4dbc9278')
IF @tableReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportTable] ([TableName], [TableDisplayName], [TableQuery], [TableQueryIsStoredProcedure], [TableReportID], [TableSettings], [TableGUID], [TableLastModified], [TableConnectionString])
 VALUES ('table', 'table', 'SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'');
SET @ToDate = {%DatabaseSchema%}.Func_Analytics_EndDateTrim(@ToDate,''month'');

SELECT X.Name AS ''{$om.variant.tabletitle$}'',ISNULL (Y.Hits,0) AS ''{$om.selectedperiod$}'',
ISNULL(X.Hits,0) AS ''{$om.total$}''  FROM
(
SELECT ABVariantDisplayName AS Name, ISNULL(SUM (HitsCount),0) AS Hits,ABVariantGUID FROM Analytics_Statistics
  LEFT JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  LEFT JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  LEFT JOIN Analytics_MonthHits ON StatisticsID = HitsSTatisticsID    

 WHERE   StatisticsSiteID = @CMSContextCurrentSiteID AND StatisticsCode LIKE ''abconversion;%'' AND
 @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14) AND
 ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
 
 GROUP BY ABVariantDisplayName,ABVariantGUID
)
 AS X
LEFT JOIN (SELECT
  CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier) AS ABVariantGUID, SUM(HitsCount) AS Hits FROM Analytics_Statistics
  LEFT JOIN Analytics_MonthHits ON HitsStatisticsID = StatisticsID
  
  WHERE (StatisticsSiteID = @CMSContextCurrentSiteID) AND StatisticsCode LIKE ''abconversion;%''
    AND (HitsStartTime >= @FromDate) AND (HitsEndTime <= @ToDate)
    AND ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
     
  GROUP BY SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode))
)
AS Y ON X.ABVariantGUID = Y.ABVariantGUID

 
ORDER BY X.Hits Desc', 0, @tableReportID, '<CustomData><enablepaging>False</enablepaging><exportenabled>True</exportenabled><pagemode>1</pagemode><pagesize>15</pagesize><querynorecordtext>No data found</querynorecordtext><SkinID>ReportGridAnalytics</SkinID><subscriptionenabled>True</subscriptionenabled></CustomData>', 'd42ca0dd-4ef1-4cc4-81dd-0723e47acb01', getDate(), '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @tableReportID int;
SET @tableReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '9567651c-efe0-4f8a-9a6b-c36a126923dc')
IF @tableReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportTable] ([TableName], [TableDisplayName], [TableQuery], [TableQueryIsStoredProcedure], [TableReportID], [TableSettings], [TableGUID], [TableLastModified], [TableConnectionString])
 VALUES ('table', 'table', 'SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''month'');
SET @ToDate = {%DatabaseSchema%}.Func_Analytics_EndDateTrim(@ToDate,''month'');

SELECT X.Name AS ''{$om.variant.tabletitle$}'',ISNULL (Y.Hits,0) AS ''{$om.selectedperiod$}'',
ISNULL(X.Hits,0) AS ''{$om.total$}''  FROM
(
SELECT ABVariantDisplayName AS Name, ISNULL(SUM (HitsValue),0) AS Hits,ABVariantGUID FROM Analytics_Statistics
  LEFT JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  LEFT JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  LEFT JOIN Analytics_MonthHits ON StatisticsID = HitsSTatisticsID    

 WHERE   StatisticsSiteID = @CMSContextCurrentSiteID AND StatisticsCode LIKE ''abconversion;%'' AND
 @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14) AND
 ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
 
 GROUP BY ABVariantDisplayName,ABVariantGUID
)
 AS X
LEFT JOIN (SELECT
  CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier) AS ABVariantGUID, SUM(HitsValue) AS Hits FROM Analytics_Statistics
  LEFT JOIN Analytics_MonthHits ON HitsStatisticsID = StatisticsID
  
  WHERE (StatisticsSiteID = @CMSContextCurrentSiteID) AND StatisticsCode LIKE ''abconversion;%''
    AND (HitsStartTime >= @FromDate) AND (HitsEndTime <= @ToDate)
    AND ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
     
  GROUP BY SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode))
)
AS Y ON X.ABVariantGUID = Y.ABVariantGUID

 
ORDER BY X.Hits Desc', 0, @tableReportID, '<CustomData><enablepaging>False</enablepaging><exportenabled>True</exportenabled><pagemode>1</pagemode><pagesize>15</pagesize><querynorecordtext>No data found</querynorecordtext><SkinID>ReportGridAnalytics</SkinID><subscriptionenabled>True</subscriptionenabled></CustomData>', 'e6ed1f8e-6908-443a-bded-2eb8aaa06efa', getDate(), '')


END

END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @tableReportID int;
SET @tableReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = 'c30a7bd2-6c53-4c77-bb0e-afcce8384d16')
IF @tableReportID IS NOT NULL BEGIN

INSERT [Reporting_ReportTable] ([TableName], [TableDisplayName], [TableQuery], [TableQueryIsStoredProcedure], [TableReportID], [TableSettings], [TableGUID], [TableLastModified], [TableConnectionString])
 VALUES ('table', 'table', 'SET @FromDate = {%DatabaseSchema%}.Func_Analytics_DateTrim(@FromDate,''week'');
SET @ToDate = {%DatabaseSchema%}.Func_Analytics_EndDateTrim(@ToDate,''week'');

SELECT X.Name AS ''{$om.variant.tabletitle$}'',ISNULL (Y.Hits,0) AS ''{$om.selectedperiod$}'',
ISNULL(X.Hits,0) AS ''{$om.total$}''  FROM
(
SELECT ABVariantDisplayName AS Name, ISNULL(SUM (HitsValue),0) AS Hits,ABVariantGUID FROM Analytics_Statistics
  LEFT JOIN OM_ABVariantData ON ABVariantGUID = CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier)
  LEFT JOIN OM_ABTest ON ABVariantTestID = ABTestID AND ABTestSiteID = @CMSContextCurrentSiteID
  LEFT JOIN Analytics_WeekHits ON StatisticsID = HitsSTatisticsID    

 WHERE   StatisticsSiteID = @CMSContextCurrentSiteID AND StatisticsCode LIKE ''abconversion;%'' AND
 @TestName = SUBSTRING(StatisticsCode, 14, CHARINDEX('';'',StatisticsCode,14)-14) AND
 ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
 
 GROUP BY ABVariantDisplayName,ABVariantGUID
)
 AS X
LEFT JOIN (SELECT
  CAST(SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode)) AS uniqueidentifier) AS ABVariantGUID, SUM(HitsValue) AS Hits FROM Analytics_Statistics
  LEFT JOIN Analytics_WeekHits ON HitsStatisticsID = StatisticsID
  
  WHERE (StatisticsSiteID = @CMSContextCurrentSiteID) AND StatisticsCode LIKE ''abconversion;%''
    AND (HitsStartTime >= @FromDate) AND (HitsEndTime <= @ToDate)
    AND ISNULL(@ConversionName,'''') IN ('''',StatisticsObjectName)
     
  GROUP BY SUBSTRING(StatisticsCode, LEN(StatisticsCode)-35, LEN(StatisticsCode))
)
AS Y ON X.ABVariantGUID = Y.ABVariantGUID

 
ORDER BY X.Hits Desc', 0, @tableReportID, '<CustomData><enablepaging>False</enablepaging><exportenabled>True</exportenabled><pagemode>1</pagemode><pagesize>15</pagesize><querynorecordtext>No data found</querynorecordtext><SkinID>ReportGridAnalytics</SkinID><subscriptionenabled>True</subscriptionenabled></CustomData>', 'a7654600-a507-4f25-aec4-b8f4ca5ffa89', getDate(), '')


END

END

GO


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 13
BEGIN

UPDATE [CMS_SearchEngine] SET
        [SearchEngineDomainRule] = 'search.seznam.cz'
    WHERE [SearchEngineGUID] = '0178936b-99ae-4257-b87e-7059421e35e7' AND [SearchEngineDomainRule] = 'seznam.cz'


END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '684e021b-8b56-4cbf-8fd5-b7a791fd2dbc')
IF @classResourceID IS NOT NULL BEGIN

INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES ('Ecommerce.GiftCard',
        '<form version="2"><category name="com.giftcard.fieldscategory.general"><properties><caption>{$com.giftcard.fieldscategory.general$}</caption><visible>True</visible></properties></category><field column="GiftCardID" columnprecision="0" columntype="integer" guid="7942b9b7-baca-4b80-bedb-f8176abc75c8" isPK="true" publicfield="false" system="true"><properties><fieldcaption>GiftCardID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="GiftCardGuid" columntype="guid" guid="6abc2a41-a5c9-4909-9b7f-08c9f0e3adf0" publicfield="false" system="true"><properties><fieldcaption>GUID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="GiftCardDisplayName" columnsize="200" columntype="text" guid="d38da7c4-f0ad-4181-bd0e-d21feb866ff7" publicfield="false" system="true" translatefield="true" visible="true"><properties><fieldcaption>{$com.giftcard.displayname$}</fieldcaption><fielddescription>{$com.giftcard.displayname.description$}</fielddescription></properties><settings><controlname>LocalizableTextBox</controlname><ValueIsContent>False</ValueIsContent></settings></field><field column="GiftCardName" columnsize="200" columntype="text" guid="f1b69023-ad8f-4bd6-a70c-f99c2b2431a0" isunique="true" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.giftcard.name$}</fieldcaption><fielddescription>{$com.giftcard.name.description$}</fielddescription></properties><settings><controlname>CodeName</controlname><RequireIdentifier>False</RequireIdentifier></settings></field><field allowempty="true" column="GiftCardDescription" columntype="longtext" guid="f76711bb-bc07-41c9-be49-46798fbb0015" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.giftcard.description$}</fieldcaption><fielddescription>{$com.giftcard.description.description$}</fielddescription></properties><settings><controlname>LocalizableTextArea</controlname><ValueIsContent>False</ValueIsContent></settings></field><field column="GiftCardEnabled" columntype="boolean" guid="4ba556e3-43b3-4372-b13c-effdebc3164c" publicfield="false" system="true" visible="true"><properties><defaultvalue>True</defaultvalue><fieldcaption>{$com.giftcard.enabled$}</fieldcaption><fielddescription>{$com.giftcard.enabled.description$}</fielddescription></properties><settings><controlname>CheckBoxControl</controlname></settings></field><field column="GiftCardLastModified" columnprecision="7" columntype="datetime" guid="fc08d122-1acd-4863-ad16-7f6f0b915570" publicfield="false" system="true"><properties><fieldcaption>Last modified</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="GiftCardSiteID" columntype="integer" guid="121f6aeb-fbf4-4a67-a7a1-6ee689f2a7ab" publicfield="false" refobjtype="cms.site" reftype="Required" system="true" /><category name="com.giftcard.fieldscategory.basics"><properties><caption>{$com.giftcard.fieldscategory.value$}</caption><visible>True</visible></properties></category><field column="GiftCardValue" columnprecision="9" columnsize="18" columntype="decimal" guid="02cea2a4-edd0-440f-bc8b-e4135ae656a0" publicfield="false" system="true" visible="true"><properties><defaultvalue>0</defaultvalue><fieldcaption>{$com.giftcard.value$}</fieldcaption><fielddescription>{$com.giftcard.value.description$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><category name="com.giftcard.fieldscategory.condition"><properties><caption>{$com.giftcard.fieldscategory.condition$}</caption><visible>True</visible></properties></category><field allowempty="true" column="GiftCardMinimumOrderPrice" columnprecision="9" columnsize="18" columntype="decimal" guid="d7fef411-c168-4778-8351-065cc02ce9d5" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.giftcard.minimumorderprice$}</fieldcaption><fielddescription>{$com.giftcard.minimumorderprice.description$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="GiftCardCartCondition" columntype="longtext" guid="3dab3f5b-6872-49f2-84ae-62e9e33ff2db" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.giftcard.cartcondition$}</fieldcaption><fielddescription>{$com.giftcard.cartcondition.description$}</fielddescription></properties><settings><AddDataMacroBrackets>True</AddDataMacroBrackets><controlname>ConditionBuilder</controlname><DisplayRuleType>1</DisplayRuleType><MaxWidth>600</MaxWidth><ResolverName>CalculationResolver</ResolverName><RuleCategoryNames>com.orderdiscount</RuleCategoryNames><ShowAutoCompletionAbove>True</ShowAutoCompletionAbove><ShowGlobalRules>False</ShowGlobalRules><SingleLineMode>True</SingleLineMode></settings></field><category name="com.giftcard.fieldscategory.duration"><properties><caption>{$com.giftcard.fieldscategory.duration$}</caption><visible>True</visible></properties></category><field allowempty="true" column="GiftCardValidFrom" columnprecision="7" columntype="datetime" guid="74087089-b8c7-4267-88a4-6d937d7ac375" publicfield="false" spellcheck="false" system="true" visible="true"><properties><fieldcaption>{$com.giftcard.validfrom$}</fieldcaption><fielddescription>{$com.giftcard.validfrom.description$}</fielddescription><validationerrormessage>{$general.dateoverlaps$}</validationerrormessage></properties><settings><CheckRange>True</CheckRange><controlname>CalendarControl</controlname><DisplayNow>True</DisplayNow><EditTime>True</EditTime><TimeZoneType>inherit</TimeZoneType></settings></field><field allowempty="true" column="GiftCardValidTo" columnprecision="7" columntype="datetime" guid="14ea2dfb-4231-46ac-bc90-fc7f5a2c04d9" publicfield="false" spellcheck="false" system="true" visible="true"><properties><fieldcaption>{$com.giftcard.validto$}</fieldcaption><fielddescription>{$com.giftcard.validto.description$}</fielddescription><validationerrormessage>{$general.dateoverlaps$}</validationerrormessage></properties><settings><CheckRange>True</CheckRange><controlname>CalendarControl</controlname><DisplayNow>True</DisplayNow><EditTime>True</EditTime><TimeZoneType>inherit</TimeZoneType></settings><rules><rule errormsg="{$general.dateoverlaps$}">{%Rule(&quot;(Value &gt; Fields[\&quot;74087089-b8c7-4267-88a4-6d937d7ac375\&quot;].Value)&quot;, &quot;&lt;rules&gt;&lt;r pos=\&quot;0\&quot; par=\&quot;\&quot; op=\&quot;and\&quot; n=\&quot;CompareToField\&quot; &gt;&lt;p n=\&quot;field\&quot;&gt;&lt;t&gt;Valid from&lt;/t&gt;&lt;v&gt;74087089-b8c7-4267-88a4-6d937d7ac375&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;p n=\&quot;operator\&quot;&gt;&lt;t&gt;Is greater than&lt;/t&gt;&lt;v&gt;&amp;gt;&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;&quot;)|(identity)GlobalAdministrator|(hash)3848d342f974730e455e8831d0fcaae21c4a635c8bcd1088abf5d15d04600438%}</rule></rules></field><category name="com.giftcard.fieldscategory.targetcustomers"><properties><caption>{$com.giftcard.fieldscategory.targetcustomers$}</caption><visible>True</visible></properties></category><field allowempty="true" column="GiftCardCustomerRestriction" columnsize="200" columntype="text" guid="f6599af5-0f4b-4ff4-bc5d-91b1b162d9af" hasdependingfields="true" publicfield="false" spellcheck="false" system="true" visible="true"><properties><defaultvalue>enum1</defaultvalue><fieldcaption>{$com.giftcard.customerrestriction$}</fieldcaption><fielddescription>{$com.giftcard.customerrestriction.description$}</fielddescription></properties><settings><AssemblyName>CMS.Ecommerce</AssemblyName><controlname>EnumSelector</controlname><DisplayType>2</DisplayType><Sort>False</Sort><TypeName>CMS.Ecommerce.DiscountCustomerEnum</TypeName><UseStringRepresentation>True</UseStringRepresentation></settings></field><field allowempty="true" column="GiftCardRoles" columnsize="400" columntype="text" dependsonanotherfield="true" guid="5d7e55d8-5589-4a1b-9448-af0f58883edb" publicfield="false" spellcheck="false" system="true" visible="true"><properties><enabledmacro ismacro="true">{%GiftCodeCustomerRestriction.Value == &quot;SelectedRoles&quot;|(identity)GlobalAdministrator|(hash)e583fa1d5d148768d5898f672cbc2c9d29ed151f072b2895f554e47da4de981f%}</enabledmacro><fielddescription>{$com.giftcard.roles$}</fielddescription></properties><settings><controlname>RoleCheckboxSelector</controlname></settings></field></form>',
        0);

END

END


GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_CMS_Class_CMS_Class') AND parent_object_id = OBJECT_ID(N'CMS_Class'))
BEGIN

	ALTER TABLE CMS_Class DROP CONSTRAINT FK_CMS_Class_CMS_Class
	
END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

UPDATE [CMS_FormUserControl] SET 
        [UserControlParameters] = '<form version="2"><field column="ShowSiteFilter" columntype="boolean" guid="a55adc13-796d-4f87-b2b6-a69081ac3902" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>Show site filter</fieldcaption><fielddescription>Indicates, if the selector should also display the site selecting drop-down list.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="SelectionMode" columntype="integer" displayinsimplemode="true" guid="a7506cf0-9b0c-4352-8194-674d955118b5" publicfield="false" resolvedefaultvalue="False" visible="true"><properties><defaultvalue>1</defaultvalue><fieldcaption>Selection mode</fieldcaption><fielddescription>Determines the design and behavior of the selection dialog.</fielddescription></properties><settings><controlname>DropDownListControl</controlname><DisplayActualValueAsItem>False</DisplayActualValueAsItem><EditText>False</EditText><Options>0;Single text box
1;Single drop down list
2;Multiple
3;Multiple text box
4;Single button
5;Multiple button
6;Single transformation</Options><SortItems>False</SortItems></settings></field><field allowempty="true" column="AllowEmpty" columntype="boolean" displayinsimplemode="true" guid="689e32fc-259a-4269-bda4-3476473fe748" publicfield="false" resolvedefaultvalue="False" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>Allow none</fieldcaption><fielddescription>Specifies, whether the selector allows empty selection. If the dialog allows empty selection, it displays the (none) field in the DDL variant and a Clear button in Textbox mode (default true). With multiple selection it returns an empty string, otherwise it returns 0.</fielddescription></properties><settings><controlname>CheckBoxControl</controlname></settings></field></form>'
    WHERE [UserControlGUID] = 'e68d5707-ffdf-42eb-b0e2-ec11b09a1e1f' AND [UserControlParentID] IS NULL

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '684e021b-8b56-4cbf-8fd5-b7a791fd2dbc')
IF @classResourceID IS NOT NULL BEGIN

INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES ('ecommerce.sku',
        '<form version="2"><category name="com.sku.generalcategory"><properties><caption>{$com.sku.generalcategory$}</caption><visible>True</visible></properties></category><field column="SKUID" columntype="integer" guid="95abe990-8663-4a8d-8db4-a4d104579424" isPK="true" publicfield="false" system="true" visibility="none"><properties><fieldcaption>SKUID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="SKUGUID" columntype="guid" guid="99228497-3209-44bd-8e5c-cd9de56e7fbd" publicfield="false" system="true" visibility="none" /><field allowempty="true" column="SKUOptionCategoryID" columntype="integer" guid="5781a44f-28d4-4ac6-b393-55c9fb2b1f3a" publicfield="false" system="true"><settings><controlname>labelcontrol</controlname></settings></field><field allowempty="true" column="SKUOrder" columntype="integer" guid="3e41eb69-f9f4-42ea-a51b-f47ebb489b6e" publicfield="false" system="true"><settings><controlname>labelcontrol</controlname></settings></field><field allowempty="true" column="SKUSiteID" columntype="integer" guid="69b49087-e19b-4b94-990c-f174f146b7db" publicfield="false" spellcheck="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field><field column="SKUName" columnsize="440" columntype="text" guid="161e6482-fbd6-41df-b251-5ef9603f8576" publicfield="false" system="true" translatefield="true" visible="true"><properties><fieldcaption>{$com.sku.name$}</fieldcaption><fielddescription>The name of the product that the system displays to your customers on the live site and to the users in the administration interface.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>True</Trim></settings></field><field allowempty="true" column="SKUNumber" columnsize="200" columntype="text" guid="c37e01b4-9477-4ccd-86f8-b6f000743184" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.skunumber$}</fieldcaption><fielddescription>{$com.sku.skunumbertooltip$}</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>True</Trim></settings></field><field column="SKUPrice" columnprecision="9" columnsize="18" columntype="decimal" guid="a3c3478f-0507-44ef-a8be-26f7fd3833c5" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.price$}</fieldcaption></properties><settings><AllowEmpty>False</AllowEmpty><AllowNegative>False</AllowNegative><AllowZero>True</AllowZero><controlname>PriceSelector</controlname><EmptyErrorMessage>com.productedit.priceinvalid</EmptyErrorMessage><FormattedPrice>True</FormattedPrice><FormatValueAsInteger>False</FormatValueAsInteger><RangeErrorMessage>com.productedit.priceinvalid</RangeErrorMessage><ShowCurrencyCode>True</ShowCurrencyCode></settings></field><field allowempty="true" column="SKURetailPrice" columnprecision="9" columnsize="18" columntype="decimal" guid="93d5d6bd-c3c4-4108-b886-24540b17ed56" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.listprice$}</fieldcaption><fielddescription>The recommended retail price (RRP) or manufacturer''s suggested retail price (MSRP), of the product.</fielddescription></properties><settings><AllowEmpty>True</AllowEmpty><AllowNegative>False</AllowNegative><AllowZero>True</AllowZero><controlname>PriceSelector</controlname><EmptyErrorMessage>com.productedit.priceinvalid</EmptyErrorMessage><FormattedPrice>True</FormattedPrice><FormatValueAsInteger>False</FormatValueAsInteger><RangeErrorMessage>com.productedit.priceinvalid</RangeErrorMessage><ShowCurrencyCode>True</ShowCurrencyCode><ShowErrorsOnNewLine>False</ShowErrorsOnNewLine></settings></field><field allowempty="true" column="SKUDepartmentID" columntype="integer" guid="f97d35fc-6b5a-4c23-a666-15f8cfe7faa6" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.departmentid$}</fieldcaption><fielddescription>Internal section for products. You can specify administrators, filter the products and configure taxes for particular departments.</fielddescription></properties><settings><AddAllItemsRecord>False</AddAllItemsRecord><AddAllRecord>False</AddAllRecord><AddNoneRecord>True</AddNoneRecord><AddWithoutDepartmentRecord>False</AddWithoutDepartmentRecord><AllowCreate>False</AllowCreate><controlname>DepartmentSelector</controlname><DropDownListMode>True</DropDownListMode><EnsureSelectedItem>True</EnsureSelectedItem><ReflectGlobalProductsUse>False</ReflectGlobalProductsUse><ShowAllSites>False</ShowAllSites><UseDepartmentNameForSelection>False</UseDepartmentNameForSelection></settings></field><field allowempty="true" column="SKUBrandID" columntype="integer" guid="a02c2a5c-05b3-4ac2-93ab-c5f0fa89888a" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.brandid$}</fieldcaption><visiblemacro ismacro="true">{%SKUSiteID != null%}</visiblemacro></properties><settings><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix><AllowAll>False</AllowAll><AllowDefault>False</AllowDefault><AllowEditTextBox>False</AllowEditTextBox><AllowEmpty>True</AllowEmpty><controlname>Uni_selector</controlname><DialogWindowName>SelectionDialog</DialogWindowName><EditDialogWindowHeight>700</EditDialogWindowHeight><EditDialogWindowWidth>1000</EditDialogWindowWidth><EditWindowName>EditWindow</EditWindowName><EncodeOutput>True</EncodeOutput><GlobalObjectSuffix ismacro="true">{$general.global$}</GlobalObjectSuffix><ItemsPerPage>25</ItemsPerPage><LocalizeItems>True</LocalizeItems><MaxDisplayedItems>25</MaxDisplayedItems><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ObjectSiteName>#currentsite</ObjectSiteName><ObjectType>ecommerce.brand</ObjectType><OrderBy>BrandDisplayName</OrderBy><RemoveMultipleCommas>False</RemoveMultipleCommas><ResourcePrefix>brandselector</ResourcePrefix><ReturnColumnType>id</ReturnColumnType><SelectionMode>1</SelectionMode><UseDefaultNameFilter>True</UseDefaultNameFilter><ValuesSeparator>;</ValuesSeparator></settings></field><field allowempty="true" column="SKUManufacturerID" columntype="integer" guid="808244ef-6595-42f2-90a0-9bf0717645e1" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.manufacturerid$}</fieldcaption></properties><settings><AddAllItemsRecord>False</AddAllItemsRecord><AddNoneRecord>True</AddNoneRecord><AllowCreate>True</AllowCreate><controlname>manufacturerselector</controlname><DisplayOnlyEnabled>True</DisplayOnlyEnabled><EnsureSelectedItem>True</EnsureSelectedItem><ShowAllSites>False</ShowAllSites><UseManufacturerNameForSelection>False</UseManufacturerNameForSelection></settings></field><field allowempty="true" column="SKUSupplierID" columntype="integer" guid="57cc81c5-1f0c-45d4-9741-e37d7f35db34" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.supplierid$}</fieldcaption></properties><settings><AddAllItemsRecord>False</AddAllItemsRecord><AddNoneRecord>True</AddNoneRecord><AllowCreate>False</AllowCreate><controlname>supplierselector</controlname><DisplayOnlyEnabled>True</DisplayOnlyEnabled><EnsureSelectedItem>True</EnsureSelectedItem><ReflectGlobalProductsUse>False</ReflectGlobalProductsUse><UseSupplierNameForSelection>False</UseSupplierNameForSelection></settings></field><field allowempty="true" column="SKUCollectionID" columntype="integer" guid="84966628-35f8-42e2-9361-0c2069dbae27" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.collectionid$}</fieldcaption><visiblemacro ismacro="true">{%SKUSiteID != null%}</visiblemacro></properties><settings><AddGlobalObjectNamePrefix>False</AddGlobalObjectNamePrefix><AddGlobalObjectSuffix>False</AddGlobalObjectSuffix><AllowAll>False</AllowAll><AllowDefault>False</AllowDefault><AllowEditTextBox>False</AllowEditTextBox><AllowEmpty>True</AllowEmpty><controlname>Uni_selector</controlname><DialogWindowName>SelectionDialog</DialogWindowName><EditDialogWindowHeight>700</EditDialogWindowHeight><EditDialogWindowWidth>1000</EditDialogWindowWidth><EditWindowName>EditWindow</EditWindowName><EncodeOutput>True</EncodeOutput><GlobalObjectSuffix ismacro="true">{$general.global$}</GlobalObjectSuffix><ItemsPerPage>25</ItemsPerPage><LocalizeItems>True</LocalizeItems><MaxDisplayedItems>25</MaxDisplayedItems><MaxDisplayedTotalItems>50</MaxDisplayedTotalItems><ObjectSiteName>#currentsite</ObjectSiteName><ObjectType>ecommerce.collection</ObjectType><OrderBy>CollectionDisplayName</OrderBy><RemoveMultipleCommas>False</RemoveMultipleCommas><ResourcePrefix>collectionselector</ResourcePrefix><ReturnColumnType>id</ReturnColumnType><SelectionMode>1</SelectionMode><UseDefaultNameFilter>True</UseDefaultNameFilter><ValuesSeparator>;</ValuesSeparator></settings></field><field allowempty="true" column="SKUTaxClassID" columntype="integer" guid="df64d505-986d-41fb-9c72-1496eb095889" publicfield="false" system="true" /><field allowempty="true" column="SKUImagePath" columnsize="450" columntype="text" guid="6174e45a-ca37-4946-a8b9-a53bd4fb76d2" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.imagepath$}</fieldcaption></properties><settings><controlname>productimageselector</controlname></settings></field><field allowempty="true" column="SKUShortDescription" columntype="longtext" guid="0e18113d-59b6-4fba-9c46-796465b2251e" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.shortdescription$}</fieldcaption><fielddescription>You can use the short description on the product listing page to briefly describe the product.</fielddescription></properties><settings><Autoresize_Hashtable>True</Autoresize_Hashtable><controlname>htmlareacontrol</controlname><Dialogs_Content_Hide>False</Dialogs_Content_Hide><Height>50</Height><HeightUnitType>PX</HeightUnitType><MediaDialogConfiguration>True</MediaDialogConfiguration><ShowAddStampButton>False</ShowAddStampButton><ToolbarLocation>Out:CKToolbar</ToolbarLocation><Width>100</Width><WidthUnitType>PERCENTAGE</WidthUnitType></settings></field><field allowempty="true" column="SKUDescription" columntype="longtext" guid="b13a8a29-f8a9-49ed-8cb4-4db417e174ab" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.description$}</fieldcaption><fielddescription>Longer description of the product. It can include media content (images, video, YouTube video, etc.); commonly used on the product details page.</fielddescription></properties><settings><Autoresize_Hashtable>True</Autoresize_Hashtable><controlname>htmlareacontrol</controlname><Dialogs_Content_Hide>False</Dialogs_Content_Hide><HeightUnitType>PX</HeightUnitType><MediaDialogConfiguration>True</MediaDialogConfiguration><ShowAddStampButton>False</ShowAddStampButton><ToolbarLocation>Out:CKToolbar</ToolbarLocation><Width>100</Width><WidthUnitType>PERCENTAGE</WidthUnitType></settings></field><field allowempty="true" column="SKUProductType" columnsize="50" columntype="text" guid="33da4205-7ff2-46f9-ae88-3f28ae663ff8" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.producttype$}</fieldcaption></properties><settings><AllowAll>False</AllowAll><AllowBundle>True</AllowBundle><AllowEproduct>True</AllowEproduct><AllowMembership>True</AllowMembership><AllowNone>False</AllowNone><AllowStandardProduct>True</AllowStandardProduct><AllowText>True</AllowText><AutoPostBack>True</AutoPostBack><controlname>producttypeselector</controlname></settings></field><field allowempty="true" column="SKUCustomData" columntype="longtext" guid="29e8c9c5-d3c7-4846-a18f-057d5cd0a352" publicfield="false" system="true"><properties><fieldcaption>SKUCustomData</fieldcaption></properties><settings><controlname>textareacontrol</controlname></settings></field><field allowempty="true" column="SKUCreated" columntype="datetime" guid="046115eb-09d5-4c9f-b4ae-e5da3761b436" publicfield="false" system="true" visibility="none"><settings><controlname>labelcontrol</controlname><timezonetype>inherit</timezonetype></settings></field><field column="SKULastModified" columntype="datetime" guid="87e7a912-8806-4971-9912-af8711f71707" publicfield="false" system="true"><settings><controlname>calendarcontrol</controlname></settings></field><category name="com.sku.representingcategory"><properties><caption>{$com.sku.representingcategory$}</caption><visible>true</visible></properties></category><field allowempty="true" column="SKUMembershipGUID" columntype="guid" guid="1bdbe0d4-4142-4aac-9eff-51709cfa8b06" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.membershipguid$}</fieldcaption></properties><settings><AddNoneRecord>True</AddNoneRecord><controlname>membershipselector</controlname><UseCodeNameForSelection>False</UseCodeNameForSelection><UseGUIDForSelection>True</UseGUIDForSelection></settings></field><field allowempty="true" column="SKUValidity" columnsize="50" columntype="text" guid="24a8c5b1-f065-4a4b-a608-836e06d07083" publicfield="false" system="true" visible="true"><properties><fieldcaption>{$com.sku.validity$}</fieldcaption></properties><settings><AutoPostBack>False</AutoPostBack><controlname>validityselector</controlname><EnableTimeZones>True</EnableTimeZones><ValidForFieldName>SKUValidFor</ValidForFieldName><ValidUntilFieldName>SKUValidUntil</ValidUntilFieldName></settings></field><field allowempty="true" column="SKUValidFor" columntype="integer" guid="e708cdc4-502c-46a3-b8a7-120167863a7e" publicfield="false" system="true" visibility="none"><settings><controlname>checkboxlistcontrol</controlname></settings></field><field allowempty="true" column="SKUValidUntil" columntype="datetime" guid="fea9d168-8b89-4b05-8243-2cd26c6b9773" publicfield="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field><field allowempty="true" column="SKUEproductFilesCount" columntype="integer" guid="4b93bdbb-4ef6-44b5-8f24-371438ce8945" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.eproductfilescount$}</fieldcaption></properties><settings><controlname>eproductfilesselector</controlname></settings></field><field allowempty="true" column="SKUBundleInventoryType" columnsize="50" columntype="text" guid="4f095aab-f4fe-409b-9990-fd491bdfa7f8" publicfield="false" system="true" visible="true"><properties><defaultvalue>REMOVEBUNDLE</defaultvalue><fieldcaption>{$com.sku.bundleinventorytype$}</fieldcaption></properties><settings><AssemblyName>CMS.Ecommerce</AssemblyName><controlname>BundleInventoryTypeSelector</controlname><DisplayType>2</DisplayType><Sort>False</Sort><TypeName>CMS.Ecommerce.BundleInventoryTypeEnum</TypeName><UseStringRepresentation>True</UseStringRepresentation></settings></field><field allowempty="true" column="SKUBundleItemsCount" columntype="integer" guid="0b485e64-28e3-44f1-a4d7-52a2116b9007" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.bundleitemscount$}</fieldcaption></properties><settings><controlname>bundleitemsselector</controlname></settings></field><category name="com.sku.statuscategory"><properties><caption>{$com.sku.statuscategory$}</caption><visible>true</visible></properties></category><field allowempty="true" column="SKUInStoreFrom" columntype="datetime" guid="09e82b4e-75c1-4cbb-958a-9b7c5a3ff7b2" publicfield="false" system="true" visible="true"><properties><defaultvalue>##TODAY##</defaultvalue><fieldcaption>{$com.sku.instorefrom$}</fieldcaption></properties><settings><controlname>calendarcontrol</controlname><DisplayNow>True</DisplayNow><EditTime>True</EditTime><TimeZoneType>inherit</TimeZoneType></settings></field><field allowempty="true" column="SKUPublicStatusID" columntype="integer" guid="9e5eaa25-43b3-40af-b651-94387a5b77ef" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.publicstatusid$}</fieldcaption><fielddescription>Status of the product, for example "Featured", displayed to the visitors of your website.</fielddescription></properties><settings><AddAllItemsRecord>False</AddAllItemsRecord><AddNoneRecord>True</AddNoneRecord><AllowCreate>False</AllowCreate><controlname>publicstatusselector</controlname><DisplayOnlyEnabled>True</DisplayOnlyEnabled><EnsureSelectedItem>True</EnsureSelectedItem><ReflectGlobalProductsUse>False</ReflectGlobalProductsUse><ShowAllSites>False</ShowAllSites><UseStatusNameForSelection>False</UseStatusNameForSelection></settings></field><field allowempty="true" column="SKUInternalStatusID" columntype="integer" guid="e6053672-514c-4f66-8c2e-87da9dd3f310" publicfield="false" system="true" visibility="none" visible="true"><properties><fieldcaption>{$com.sku.internalstatusid$}</fieldcaption><fielddescription>Status of the product, for example "New model", used for your internal business purposes; invisible to your website visitors.</fielddescription></properties><settings><AddAllItemsRecord>False</AddAllItemsRecord><AddNoneRecord>True</AddNoneRecord><AllowCreate>False</AllowCreate><controlname>internalstatusselector</controlname><DisplayOnlyEnabled>True</DisplayOnlyEnabled><EnsureSelectedItem>True</EnsureSelectedItem><ReflectGlobalProductsUse>False</ReflectGlobalProductsUse><ShowAllSites>False</ShowAllSites><UseStatusNameForSelection>False</UseStatusNameForSelection></settings></field><field column="SKUEnabled" columntype="boolean" guid="bedcaf24-ad52-4293-9418-f7fcecbb9811" publicfield="false" system="true" visibility="none" visible="true"><properties><defaultvalue>True</defaultvalue><fieldcaption>{$com.sku.enabled$}</fieldcaption><fielddescription>Indicates if your customers can see the product on the live site and add it to the shopping cart.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><category name="com.sku.shippingcategory"><properties><caption>{$com.sku.shippingcategory$}</caption><visible>true</visible></properties></category><field allowempty="true" column="SKUNeedsShipping" columntype="boolean" guid="0fd1ef55-4dd6-4c0e-8030-244dda90ffa3" publicfield="false" system="true" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>{$com.sku.needsshipping$}</fieldcaption><fielddescription>Indicates if the product requires shipping. If unchecked, the customer does not need to select the shipping method.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="SKUWeight" columntype="double" guid="433ec6bf-2a24-46f7-ab64-413723f8d2d4" publicfield="false" system="true" visibility="none" visible="true"><properties><contentafter ismacro="true">&lt;span class="form-control-text"&gt;{% HTMLEncode(GetMassUnit()) %}&lt;/span&gt;</contentafter><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.weight$}</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0|3</FilterType><Trim>True</Trim><ValidChars>.,</ValidChars></settings></field><field allowempty="true" column="SKUHeight" columntype="double" guid="960b12da-a78a-47af-b30a-dd5d4c963c7f" publicfield="false" system="true" visibility="none" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.height$}</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0|3</FilterType><Trim>True</Trim><ValidChars>.,</ValidChars></settings></field><field allowempty="true" column="SKUWidth" columntype="double" guid="cada95d5-f56f-47b8-8ec5-2311f6420e8f" publicfield="false" system="true" visibility="none" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.width$}</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0|3</FilterType><Trim>True</Trim><ValidChars>.,</ValidChars></settings></field><field allowempty="true" column="SKUDepth" columntype="double" guid="01b371cb-8598-4409-82a5-955e5178dbfa" publicfield="false" system="true" visibility="none" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.depth$}</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0|3</FilterType><Trim>True</Trim><ValidChars>.,</ValidChars></settings></field><category name="com.sku.inventorycategory"><properties><caption>{$com.sku.inventorycategory$}</caption><visible>True</visible></properties></category><field allowempty="true" column="SKUTrackInventory" columnsize="50" columntype="text" guid="b008a40a-2651-4341-8779-872030271467" hasdependingfields="true" publicfield="false" system="true" visible="true"><properties><defaultvalue>ByProduct</defaultvalue><fieldcaption>Track inventory</fieldcaption><fielddescription>If allowed (the "Yes" or "By variants" options), you can specify inventory-related information, such as the number of available product items.</fielddescription></properties><settings><controlname>radiobuttonscontrol</controlname><Options ismacro="true">
        Disabled;{$general.no$}
        ByProduct;{$general.yes$}
        ByVariants;{$com.productedit.trackbyvariants$}
      </Options><RepeatDirection>horizontal</RepeatDirection><RepeatLayout>Flow</RepeatLayout></settings></field><field allowempty="true" column="SKUSellOnlyAvailable" columntype="boolean" dependsonanotherfield="true" guid="ca3ef874-bccc-401d-ae31-f5b40c99f900" publicfield="false" system="true" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>{$com.sku.sellonlyavailable$}</fieldcaption><fielddescription>If checked, your customers can purchase only quantity of the product that is in stock.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="SKUAvailableItems" columntype="integer" dependsonanotherfield="true" guid="db263b45-b660-45e4-b02e-fa0819e75472" publicfield="false" system="true" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.availableitems$}</fieldcaption><visiblemacro ismacro="true">{%SKUTrackInventory.Value == &quot;ByProduct&quot;|(identity)GlobalAdministrator|(hash)416523726a9e6efe09d7c4f8370596e90da2ff04cbdeddd6dca100964a9b9bd0%}</visiblemacro></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0|3</FilterType><Trim>True</Trim><ValidChars>-</ValidChars></settings></field><field allowempty="true" column="SKUReorderAt" columntype="integer" dependsonanotherfield="true" guid="8ff5a58e-6a4f-4a3d-9b98-0dc5e0c0b57f" publicfield="false" system="true" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.reorderat$}</fieldcaption><fielddescription>Indicates at which quantity you/your on-line store administrators should reorder the product.</fielddescription><visiblemacro ismacro="true">{%SKUTrackInventory.Value == &quot;ByProduct&quot;|(identity)GlobalAdministrator|(hash)416523726a9e6efe09d7c4f8370596e90da2ff04cbdeddd6dca100964a9b9bd0%}</visiblemacro></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0|3</FilterType><Trim>False</Trim><ValidChars>-</ValidChars></settings></field><field allowempty="true" column="SKUAvailableInDays" columntype="integer" dependsonanotherfield="true" guid="7d56b109-15d2-41ed-ab41-c505b50b8386" publicfield="false" system="true"><properties><captionstyle>margin-bottom: 20px; display: block;</captionstyle><fieldcaption>{$com.sku.availableindays$}</fieldcaption><inputcontrolstyle>margin-bottom: 20px;</inputcontrolstyle></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><FilterType>0|3</FilterType><Trim>True</Trim><ValidChars>-</ValidChars></settings></field><field allowempty="true" column="SKUMinItemsInOrder" columntype="integer" guid="429d99b0-8c88-4a2c-9022-4e71e96d2998" publicfield="false" system="true" visibility="none" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.minitemsinorder$}</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0</FilterType><Trim>True</Trim></settings></field><field allowempty="true" column="SKUMaxItemsInOrder" columntype="integer" guid="cf170044-35ac-4710-b82f-4c0162d805ff" publicfield="false" system="true" visibility="none" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><fieldcaption>{$com.sku.maxitemsinorder$}</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><FilterType>0</FilterType><Trim>True</Trim></settings></field><category name="com.sku.analyticscategory"><properties><caption>{$com.sku.analyticscategory$}</caption><collapsedbydefault>True</collapsedbydefault><collapsible>True</collapsible><visible ismacro="true">{%ProductSiteID &gt; 0 &amp;&amp; !SiteContext.CurrentSite.SiteIsContentOnly |(identity)GlobalAdministrator|(hash)06330c809eb93c16d800382634a3ca86bf7f3f3d0e13be7d1c4c1be6d3272387%}</visible></properties></category><field allowempty="true" column="SKUConversionName" columnsize="100" columntype="text" guid="0132fdb3-0cfc-4966-99ce-a7d8cfd4bf9a" publicfield="false" system="true" visibility="none" visible="true"><properties><controlcssclass>NoWrap</controlcssclass><fieldcaption>{$com.sku.conversionname$}</fieldcaption><fielddescription>Selects the web analytics conversion that the system logs when a customer purchases (orders) the given product.</fielddescription></properties><settings><controlname>ConversionSelector</controlname></settings></field><field allowempty="true" column="SKUConversionValue" columnsize="200" columntype="text" guid="870a3279-5120-4bc6-817c-318360a7fbb3" publicfield="false" system="true" visibility="none" visible="true"><properties><controlcssclass>input-width-20</controlcssclass><defaultvalue>0</defaultvalue><fieldcaption>{$com.sku.conversionvalue$}</fieldcaption><fielddescription>The specified value will be recorded for each conversion hit when the product is purchased. In addition to numeric values, you can enter macro expressions here.</fielddescription></properties><settings><controlname>textbox_double_validator</controlname></settings></field><category name="com.sku.variantcategory"><properties><caption>{$com.sku.variantcategory$}</caption><visible>true</visible></properties></category><field allowempty="true" column="SKUParentSKUID" columntype="integer" guid="cfda202f-ccbf-47b8-9b24-d6feae3a470c" publicfield="false" system="true" visibility="none"><settings><controlname>dropdownlistcontrol</controlname></settings></field></form>',
        0);

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

UPDATE [CMS_Document]
  SET [DocumentWorkflowCycleGUID] = NEWID()
  WHERE [DocumentWorkflowCycleGUID] IS NULL
  
END

GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @userControlResourceID int;
SET @userControlResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'bd03e629-e677-42a6-8b12-5277fa04add7')
IF @userControlResourceID IS NOT NULL BEGIN

INSERT [CMS_FormUserControl] ([UserControlDisplayName], [UserControlCodeName], [UserControlFileName], [UserControlForText], [UserControlForLongText], [UserControlForInteger], [UserControlForDecimal], [UserControlForDateTime], [UserControlForBoolean], [UserControlForFile], [UserControlShowInBizForms], [UserControlDefaultDataType], [UserControlDefaultDataTypeSize], [UserControlShowInDocumentTypes], [UserControlShowInSystemTables], [UserControlShowInWebParts], [UserControlShowInReports], [UserControlGUID], [UserControlLastModified], [UserControlForGuid], [UserControlShowInCustomTables], [UserControlForVisibility], [UserControlParameters], [UserControlForDocAttachments], [UserControlResourceID], [UserControlType], [UserControlParentID], [UserControlDescription], [UserControlThumbnailGUID], [UserControlPriority], [UserControlIsSystem], [UserControlForBinary], [UserControlForDocRelationships], [UserControlAssemblyName], [UserControlClassName])
 VALUES ('A/B test URL input', 'ABTestUrlInput', '', 0, 0, 0, 0, 0, 0, 0, 0, 'Text', 0, 0, 0, 0, 0, '8abb998d-9541-49d7-90d3-8f77e85727b3', getDate(), 0, 0, 0, '<form version="2" />', 0, @userControlResourceID, 0, NULL, 'Url input for selecting page url in A/B testing application.', NULL, 0, 0, 0, 0, 'CMS.OnlineMarketing.Web.UI', 'CMS.OnlineMarketing.Web.UI.Internal.ABTestingUrlInput')


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @formClassID int;
SET @formClassID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassGUID] = 'f1349c42-bae7-4614-a2ec-a7e61d8867c5')
IF @formClassID IS NOT NULL BEGIN

DECLARE @className nvarchar(100);
SET @className = (SELECT TOP 1 [ClassName] FROM [CMS_Class] INNER JOIN [CMS_AlternativeForm] ON [ClassID] = [FormClassID] WHERE [FormGUID] = '4afa3a64-f525-4e2f-b7e7-784f969d333c')
INSERT INTO [Temp_FormDefinition] ([ObjectName], [FormDefinition], [IsAltForm])
VALUES (@className + '.UpdateOther',
        '<form version="2"><field column="SKUID" guid="95abe990-8663-4a8d-8db4-a4d104579424" /><field column="SKUGUID" guid="99228497-3209-44bd-8e5c-cd9de56e7fbd" /><field column="SKUOptionCategoryID" guid="5781a44f-28d4-4ac6-b393-55c9fb2b1f3a" /><field column="SKUOrder" guid="3e41eb69-f9f4-42ea-a51b-f47ebb489b6e" /><field column="SKUSiteID" guid="69b49087-e19b-4b94-990c-f174f146b7db" /><field column="SKUName" guid="161e6482-fbd6-41df-b251-5ef9603f8576" visible="" /><field column="SKUNumber" guid="c37e01b4-9477-4ccd-86f8-b6f000743184" visible=""><properties><fieldcaption>Product number</fieldcaption></properties></field><field column="SKUPrice" guid="a3c3478f-0507-44ef-a8be-26f7fd3833c5" visible=""><properties><fieldcaption>Price</fieldcaption></properties></field><field column="SKURetailPrice" guid="93d5d6bd-c3c4-4108-b886-24540b17ed56" visible=""><properties><fieldcaption>Retail price</fieldcaption></properties></field><field column="SKUDepartmentID" guid="f97d35fc-6b5a-4c23-a666-15f8cfe7faa6" visible=""><properties><fieldcaption>Department</fieldcaption></properties></field><field column="SKUBrandID" guid="a02c2a5c-05b3-4ac2-93ab-c5f0fa89888a" visible="" /><field column="SKUManufacturerID" guid="808244ef-6595-42f2-90a0-9bf0717645e1" visible=""><properties><fieldcaption>Manufacturer</fieldcaption></properties></field><field column="SKUSupplierID" guid="57cc81c5-1f0c-45d4-9741-e37d7f35db34" visible=""><properties><fieldcaption>Supplier</fieldcaption></properties></field><field column="SKUCollectionID" guid="84966628-35f8-42e2-9361-0c2069dbae27" visible="" /><field column="SKUTaxClassID" guid="df64d505-986d-41fb-9c72-1496eb095889" /><field column="SKUImagePath" guid="6174e45a-ca37-4946-a8b9-a53bd4fb76d2" visible=""><properties><fieldcaption>Image</fieldcaption></properties></field><field column="SKUShortDescription" guid="0e18113d-59b6-4fba-9c46-796465b2251e" visible=""><settings><Dialogs_Anchor_Hide>False</Dialogs_Anchor_Hide><Dialogs_Attachments_Hide>False</Dialogs_Attachments_Hide><Dialogs_Email_Hide>False</Dialogs_Email_Hide><Dialogs_Libraries_Hide>False</Dialogs_Libraries_Hide><Dialogs_Web_Hide>False</Dialogs_Web_Hide></settings><properties><fieldcaption>Short description</fieldcaption></properties></field><field column="SKUDescription" guid="b13a8a29-f8a9-49ed-8cb4-4db417e174ab" visible=""><properties><fieldcaption>Description</fieldcaption></properties></field><field column="SKUProductType" guid="33da4205-7ff2-46f9-ae88-3f28ae663ff8" visible=""><properties><fieldcaption>This product represents</fieldcaption></properties></field><field column="SKUCustomData" guid="29e8c9c5-d3c7-4846-a18f-057d5cd0a352" /><field column="SKUCreated" guid="046115eb-09d5-4c9f-b4ae-e5da3761b436" /><field column="SKULastModified" guid="87e7a912-8806-4971-9912-af8711f71707" /><field column="SKUMembershipGUID" guid="1bdbe0d4-4142-4aac-9eff-51709cfa8b06" visible=""><properties><fieldcaption>Membership group</fieldcaption></properties></field><field column="SKUValidity" guid="24a8c5b1-f065-4a4b-a608-836e06d07083" visible=""><properties><fieldcaption>Validity</fieldcaption></properties></field><field column="SKUValidFor" guid="e708cdc4-502c-46a3-b8a7-120167863a7e" /><field column="SKUValidUntil" guid="fea9d168-8b89-4b05-8243-2cd26c6b9773" /><field column="SKUEproductFilesCount" guid="4b93bdbb-4ef6-44b5-8f24-371438ce8945" visible=""><properties><fieldcaption>Files</fieldcaption></properties></field><field column="SKUBundleInventoryType" guid="4f095aab-f4fe-409b-9990-fd491bdfa7f8" visible=""><properties><fieldcaption>Remove from inventory</fieldcaption></properties></field><field column="SKUBundleItemsCount" guid="0b485e64-28e3-44f1-a4d7-52a2116b9007" visible=""><properties><fieldcaption>Products</fieldcaption></properties></field><field column="SKUInStoreFrom" guid="09e82b4e-75c1-4cbb-958a-9b7c5a3ff7b2" /><field column="SKUPublicStatusID" guid="9e5eaa25-43b3-40af-b651-94387a5b77ef" visibility=""><settings><AllowCreate>True</AllowCreate></settings></field><field column="SKUInternalStatusID" guid="e6053672-514c-4f66-8c2e-87da9dd3f310" visibility=""><settings><AllowCreate>True</AllowCreate></settings></field><field column="SKUEnabled" guid="bedcaf24-ad52-4293-9418-f7fcecbb9811" /><field column="SKUNeedsShipping" guid="0fd1ef55-4dd6-4c0e-8030-244dda90ffa3" /><field column="SKUWeight" guid="433ec6bf-2a24-46f7-ab64-413723f8d2d4"><settings><ValidChars>,.</ValidChars></settings></field><field column="SKUHeight" guid="960b12da-a78a-47af-b30a-dd5d4c963c7f" /><field column="SKUWidth" guid="cada95d5-f56f-47b8-8ec5-2311f6420e8f" /><field column="SKUDepth" guid="01b371cb-8598-4409-82a5-955e5178dbfa" /><field column="SKUTrackInventory" guid="b008a40a-2651-4341-8779-872030271467"><settings><controlname>RadioButtonsControl</controlname><Options ismacro="true">
        Disabled;{$general.no$}
        ByProduct;{$general.yes$}
        ByVariants;{$com.productedit.trackbyvariants$}

      </Options></settings></field><field column="SKUSellOnlyAvailable" guid="ca3ef874-bccc-401d-ae31-f5b40c99f900"><properties><visiblemacro ismacro="true">{%Fields.skutrackinventory.Value != &quot;Disabled&quot;|(identity)GlobalAdministrator|(hash)8dbb276ff91727568e1d7419eb2f4314813199af58c0842bb208a4fafd31b7e9%}</visiblemacro></properties></field><field column="SKUAvailableItems" guid="db263b45-b660-45e4-b02e-fa0819e75472"><properties><visiblemacro>{%SKUTrackInventory.Value == &quot;ByProduct&quot;|(identity)GlobalAdministrator|(hash)416523726a9e6efe09d7c4f8370596e90da2ff04cbdeddd6dca100964a9b9bd0%}</visiblemacro></properties></field><field column="SKUReorderAt" guid="8ff5a58e-6a4f-4a3d-9b98-0dc5e0c0b57f" /><field column="SKUAvailableInDays" guid="7d56b109-15d2-41ed-ab41-c505b50b8386"><properties><visiblemacro ismacro="true">{%Fields.skutrackinventory.Value != &quot;Disabled&quot;|(identity)GlobalAdministrator|(hash)8dbb276ff91727568e1d7419eb2f4314813199af58c0842bb208a4fafd31b7e9%}</visiblemacro></properties></field><field column="SKUMinItemsInOrder" guid="429d99b0-8c88-4a2c-9022-4e71e96d2998" /><field column="SKUMaxItemsInOrder" guid="cf170044-35ac-4710-b82f-4c0162d805ff" /><field column="SKUConversionName" guid="0132fdb3-0cfc-4966-99ce-a7d8cfd4bf9a" /><field column="SKUConversionValue" guid="870a3279-5120-4bc6-817c-318360a7fbb3" /><field column="SKUParentSKUID" guid="cfda202f-ccbf-47b8-9b24-d6feae3a470c" /></form>',
        1);

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @classResourceID int;
SET @classResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '83ff58cf-d7ed-4567-a68c-439daf7e85cf')
IF @classResourceID IS NOT NULL BEGIN

INSERT [CMS_Class] ([ClassDisplayName], [ClassName], [ClassUsesVersioning], [ClassIsDocumentType], [ClassIsCoupledClass], [ClassXmlSchema], [ClassFormDefinition], [ClassEditingPageUrl], [ClassListPageUrl], [ClassNodeNameSource], [ClassTableName], [ClassViewPageUrl], [ClassPreviewPageUrl], [ClassFormLayout], [ClassNewPageUrl], [ClassShowAsSystemTable], [ClassUsePublishFromTo], [ClassShowTemplateSelection], [ClassSKUMappings], [ClassIsMenuItemType], [ClassNodeAliasSource], [ClassDefaultPageTemplateID], [ClassLastModified], [ClassGUID], [ClassCreateSKU], [ClassIsProduct], [ClassIsCustomTable], [ClassShowColumns], [ClassSearchTitleColumn], [ClassSearchContentColumn], [ClassSearchImageColumn], [ClassSearchCreationDateColumn], [ClassSearchSettings], [ClassInheritsFromClassID], [ClassSearchEnabled], [ClassSKUDefaultDepartmentName], [ClassSKUDefaultDepartmentID], [ClassContactMapping], [ClassContactOverwriteEnabled], [ClassSKUDefaultProductType], [ClassConnectionString], [ClassIsProductSection], [ClassPageTemplateCategoryID], [ClassFormLayoutType], [ClassVersionGUID], [ClassDefaultObjectType], [ClassIsForm], [ClassResourceID], [ClassCustomizedColumns], [ClassCodeGenerationSettings], [ClassIconClass], [ClassIsContentOnly], [ClassURLPattern])
 VALUES ('Page alternative url', 'CMS.AlternativeUrl', 0, 0, 1, '<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
  <xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
    <xs:complexType>
      <xs:choice minOccurs="0" maxOccurs="unbounded">
        <xs:element name="CMS_AlternativeUrl">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="AlternativeUrlID" msdata:ReadOnly="true" msdata:AutoIncrement="true" type="xs:int" />
              <xs:element name="AlternativeUrlGUID" msdata:DataType="System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" type="xs:string" />
              <xs:element name="AlternativeUrlDocumentID" type="xs:int" />
              <xs:element name="AlternativeUrlSiteID" type="xs:int" />
              <xs:element name="AlternativeUrlUrl">
                <xs:simpleType>
                  <xs:restriction base="xs:string">
                    <xs:maxLength value="450" />
                  </xs:restriction>
                </xs:simpleType>
              </xs:element>
              <xs:element name="AlternativeUrlLastModified" type="xs:dateTime" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
    <xs:unique name="Constraint1" msdata:PrimaryKey="true">
      <xs:selector xpath=".//CMS_AlternativeUrl" />
      <xs:field xpath="AlternativeUrlID" />
    </xs:unique>
  </xs:element>
</xs:schema>', '<form version="2"><field column="AlternativeUrlID" columntype="integer" guid="0eac93a7-493b-4d68-a6cd-fdcd5d37bed2" isPK="true" publicfield="false" system="true"><properties><fieldcaption>AlternativeUrlID</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field><field column="AlternativeUrlGUID" columntype="guid" guid="92d82a94-e7d8-4bc4-b545-5207b2eedf10" publicfield="false" system="true"><settings><controlname>LabelControl</controlname><ResolveMacros>False</ResolveMacros></settings></field><field column="AlternativeUrlDocumentID" columntype="integer" guid="98c1d2b0-844f-44b3-9398-235581c18ab5" publicfield="false" refobjtype="cms.document" reftype="Required" system="true"><properties><fieldcaption>Document</fieldcaption></properties><settings><controlname>selectdocument</controlname></settings></field><field column="AlternativeUrlSiteID" columntype="integer" guid="984ca4d9-dcc9-4652-b31f-a3161ff47776" publicfield="false" refobjtype="cms.site" reftype="Required" system="true"><properties><fieldcaption>Site</fieldcaption></properties><settings><AllowAll>False</AllowAll><AllowEmpty>True</AllowEmpty><AllowGlobal>False</AllowGlobal><AllowMultipleSelection>False</AllowMultipleSelection><controlname>selectsite</controlname><OnlyRunningSites>False</OnlyRunningSites></settings></field><field column="AlternativeUrlUrl" columnsize="450" columntype="text" guid="302d2881-c882-496c-8657-23552a0b1c11" publicfield="false" system="true"><properties><fieldcaption>Url</fieldcaption></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>TextBoxControl</controlname><FilterMode>False</FilterMode><Trim>True</Trim></settings></field><field column="AlternativeUrlLastModified" columnprecision="7" columntype="datetime" guid="f6662c48-3876-4c55-b480-da8167e73374" publicfield="false" system="true"><properties><fieldcaption>Last modified</fieldcaption></properties><settings><controlname>labelcontrol</controlname></settings></field></form>', '', NULL, '', 'CMS_AlternativeUrl', NULL, NULL, NULL, NULL, 0, 0, 0, NULL, 0, NULL, NULL, getDate(), 'b58f5a5d-8871-43f6-98cd-798ed4b128da', NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CMSConnectionString', NULL, NULL, NULL, NULL, NULL, 0, @classResourceID, '', NULL, NULL, 0, NULL)


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'cc744db0-dc8c-45de-ab30-bd36f67b2eb9')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '98c6ee00-230a-4207-a6d3-03677b83b245')
IF @elementResourceID IS NOT NULL BEGIN

DECLARE @newElementGUID uniqueidentifier = '79deea0a-98a5-4df6-86b1-d5b7a859fc43';

INSERT [CMS_UIElement] ([ElementDisplayName], [ElementName], [ElementCaption], [ElementTargetURL], [ElementResourceID], [ElementParentID], [ElementChildCount], [ElementOrder], [ElementLevel], [ElementIDPath], [ElementIconPath], [ElementIsCustom], [ElementLastModified], [ElementGUID], [ElementSize], [ElementDescription], [ElementFromVersion], [ElementPageTemplateID], [ElementType], [ElementProperties], [ElementIsMenu], [ElementFeature], [ElementIconClass], [ElementIsGlobalApplication], [ElementCheckModuleReadPermission], [ElementAccessCondition], [ElementVisibilityCondition], [ElementRequiresGlobalAdminPriviligeLevel])
 VALUES ('{$content.ui.propertiesalternativeurls$}', 'Properties.AlternativeURLs', '{$content.ui.propertiesalternativeurls$}', '~/CMSModules/Content/CMSDesk/MVC/Properties/AlternativeUrls.aspx?nodeid={?nodeid?}&culture={?culture?}', @elementResourceID, @elementParentID, 0, 3, 6, '', '', 0, getDate(), @newElementGUID, 0, '', '12.0', NULL, 'Url', '<Data><DisplayBreadcrumbs>False</DisplayBreadcrumbs></Data>', 0, '', '', 0, 0, '{%CurrentUser.IsAuthorizedPerResource("CMS.Content", "ManageAlternativeURLs")|(identity)GlobalAdministrator|(hash)f59a34adc2e147a3428b1e21d7a7cd53be100aa05ebd35e13056918c9277f68c%}', '{%Settings.CMSAlternativeUrlUIEnabled && CurrentSite.SiteIsContentOnly && DocumentContext.EditedDocument.NodeClass.ClassURLPattern != null|(identity)GlobalAdministrator|(hash)fb537f6586fa5604718456cedb280917a0c7298a277a8076b9b2255919975cad%}', 0)

UPDATE [CMS_UIElement] SET
    [ElementIDPath] = COALESCE((SELECT TOP 1 [ElementIDPath] FROM [CMS_UIElement] AS [Parent] WHERE [Parent].ElementID = @elementParentID), '')
                         + '/'
                          + REPLICATE('0', 8 - LEN([ElementID]))
                          + CAST([ElementID] AS NVARCHAR(8))
WHERE [ElementGUID] = @newElementGUID

UPDATE [CMS_UIElement] SET
    [ElementChildCount] = (SELECT COUNT(*)
                                    FROM [CMS_UIElement] AS [Children]
                                    WHERE [Children].[ElementParentID] = @elementParentID)
WHERE [ElementID] = @elementParentID

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 13
    WHERE [ElementGUID] = 'eb0b937f-1091-41cb-8044-1c448918b768'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 6
    WHERE [ElementGUID] = '3ab01379-31f4-4ec0-9cdd-9f70a1ea4b7c'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 14
    WHERE [ElementGUID] = 'cf358507-ab15-40c2-b81a-bc107dc7549c'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 11
    WHERE [ElementGUID] = '8a6e22e9-25c9-4cd3-922c-07790409ad62'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 7
    WHERE [ElementGUID] = '33920356-cdc2-42ad-b177-bb1015bb9a95'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 5
    WHERE [ElementGUID] = 'a6ce8824-a436-4f04-bc6e-ee5faf36ba3c'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 10
    WHERE [ElementGUID] = 'eafc6d95-c558-43b1-ae16-df2df69d4222'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 12
    WHERE [ElementGUID] = 'c05c8c26-0a3b-4855-822d-7b3d5dcf1ff1'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 4
    WHERE [ElementGUID] = '5bcc2097-7efa-49a7-9bb2-1c53b6bbbcc4'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 9
    WHERE [ElementGUID] = '41591449-926b-43d8-992a-66498cbb4862'

UPDATE [CMS_UIElement] SET 
        [ElementOrder] = 8
    WHERE [ElementGUID] = '261168e1-29f2-4c82-8ee7-f7a54dae9b74'


END

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'eeba18a6-c6d0-4c58-a5fa-91e629540986')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = '8136b750-a785-438f-a412-32212cd4dde6')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '98c6ee00-230a-4207-a6d3-03677b83b245')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET
    [ElementChildCount] = (SELECT COUNT(*)
                                    FROM [CMS_UIElement] AS [Children]
                                    WHERE [Children].[ElementParentID] = @elementParentID)
WHERE [ElementID] = @elementParentID

END

END

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @resourceID int;
SET @resourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '98c6ee00-230a-4207-a6d3-03677b83b245')
IF @resourceID IS NOT NULL BEGIN

INSERT [CMS_Permission] ([PermissionDisplayName], [PermissionName], [ClassID], [ResourceID], [PermissionGUID], [PermissionLastModified], [PermissionDescription], [PermissionDisplayInMatrix], [PermissionOrder], [PermissionEditableByGlobalAdmin])
 VALUES ('Manage alternative URLs (MVC)', 'ManageAlternativeURLs', NULL, @resourceID, 'cb9b5d83-fe70-4daf-a019-92e7d2791ce5', getDate(), '{$content.permission.managealternativeurls.description$}', 1, 12, 0)


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 17
BEGIN

DECLARE @classID int;
SET @classID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassName] = 'cms.emailtemplate');

DECLARE @classFormDefinition nvarchar(max);
SELECT @classFormDefinition = [ClassFormDefinition] FROM [CMS_Class] WHERE [ClassID] = @classID;

SET @classFormDefinition = REPLACE(@classFormDefinition, N'<ResolverName ismacro="true">{%EmailTemplateType%}</ResolverName>', N'<ResolverName ismacro="true">{%EmailTemplateType%}resolver</ResolverName>')

UPDATE [CMS_Class] SET [ClassFormDefinition] = @classFormDefinition WHERE [ClassID] = @classID;

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 25
BEGIN

DECLARE @classID int;
SET @classID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassName] = 'newsletter.issue');

DECLARE @alternativeFormDefinition nvarchar(max);
SELECT @alternativeFormDefinition = [FormDefinition] FROM [CMS_AlternativeForm] WHERE [FormClassID] = @classID AND [FormGUID] = '5adc8936-7ff2-4585-89f3-cd22cb86dbcf';

SET @alternativeFormDefinition = REPLACE(@alternativeFormDefinition, N'<ResolverName>Newsletter</ResolverName>', N'<ResolverName>NewsletterResolver</ResolverName>')

UPDATE [CMS_AlternativeForm] SET [FormDefinition] = @alternativeFormDefinition WHERE [FormClassID] = @classID AND [FormGUID] = '5adc8936-7ff2-4585-89f3-cd22cb86dbcf';

END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @categoryParentID int;
SET @categoryParentID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.URLs')
IF @categoryParentID IS NOT NULL BEGIN

DECLARE @categoryResourceID int;
SET @categoryResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'ce1a65a0-80dc-4c53-b0e7-bdecf0aa8c02')
IF @categoryResourceID IS NOT NULL BEGIN

-- Insert new Settings category
INSERT [CMS_SettingsCategory] ([CategoryDisplayName], [CategoryOrder], [CategoryName], [CategoryParentID], [CategoryIDPath], [CategoryLevel], [CategoryChildCount], [CategoryIconPath], [CategoryIsGroup], [CategoryIsCustom], [CategoryResourceID])
 VALUES ('{$settingscategory.cmsurlalternativeurlmvc$}', 0, 'CMS.URL.AlternativeURLMvc', @categoryParentID, '', 2, 0, '', 1, 0, @categoryResourceID)

-- Update new Settings category ID path
DECLARE @categoryID int;
SELECT @categoryID = [CategoryID]
FROM [CMS_SettingsCategory]
WHERE [CategoryParentID] = @categoryParentID AND [CategoryName] = 'CMS.URL.AlternativeURLMvc' AND [CategoryResourceID] = @categoryResourceID

UPDATE [CMS_SettingsCategory] SET
	[CategoryIDPath] = COALESCE((SELECT TOP 1 [CategoryIDPath] FROM [CMS_SettingsCategory] AS [Parent] WHERE [Parent].CategoryID = @categoryParentID), '')
						  + '/'
						  + REPLICATE('0', 8 - LEN(@categoryID))
						  + CAST(@categoryID AS NVARCHAR(8))
WHERE [CategoryID] = @categoryID

-- Update child count of parent Settings category
UPDATE [CMS_SettingsCategory] SET 
        [CategoryChildCount] = (SELECT COUNT(*)
									FROM [CMS_SettingsCategory] AS [Children]
									WHERE [Children].[CategoryParentID] = @categoryParentID)
    WHERE [CategoryID] = @categoryParentID

END

END

END

GO

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @userControlResourceID int;
SET @userControlResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'ce1a65a0-80dc-4c53-b0e7-bdecf0aa8c02')
IF @userControlResourceID IS NOT NULL BEGIN

INSERT [CMS_FormUserControl] ([UserControlDisplayName], [UserControlCodeName], [UserControlFileName], [UserControlForText], [UserControlForLongText], [UserControlForInteger], [UserControlForDecimal], [UserControlForDateTime], [UserControlForBoolean], [UserControlForFile], [UserControlShowInBizForms], [UserControlDefaultDataType], [UserControlDefaultDataTypeSize], [UserControlShowInDocumentTypes], [UserControlShowInSystemTables], [UserControlShowInWebParts], [UserControlShowInReports], [UserControlGUID], [UserControlLastModified], [UserControlForGuid], [UserControlShowInCustomTables], [UserControlForVisibility], [UserControlParameters], [UserControlForDocAttachments], [UserControlResourceID], [UserControlType], [UserControlParentID], [UserControlDescription], [UserControlThumbnailGUID], [UserControlPriority], [UserControlIsSystem], [UserControlForBinary], [UserControlForDocRelationships], [UserControlAssemblyName], [UserControlClassName])
 VALUES ('Alternative URLs Constraint Editor', 'AlternativeURLsConstraintEditor', '', 1, 1, 0, 0, 0, 0, 0, 1, 'text', 500, 1, 1, 1, 1, 'dd6a916d-ca66-4e98-ae08-85d022e4e92e', getDate(), 0, 1, 0, '<form version="2"><field column="IsTextArea" columntype="boolean" guid="a5213bce-bebf-47ad-bbf2-02919c975dc3" publicfield="false" visibility="none"><properties><defaultvalue>true</defaultvalue></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="Cols" columntype="integer" displayinsimplemode="true" guid="225ffe99-bfdc-4e3b-9b49-3f2f70ceebd3" publicfield="false" spellcheck="false" visibility="none" visible="true"><properties><fieldcaption>Columns</fieldcaption><fielddescription>Sets the number of columns (width in characters) for the text editing area.</fielddescription><validationerrormessage>The value must be greater than 0.</validationerrormessage></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings><rules><rule>{%Rule("Value &gt;= 1", "&lt;rules&gt;&lt;r pos=\"0\" par=\"\" op=\"and\" n=\"MinValue\" &gt;&lt;p n=\"minvalue\"&gt;&lt;t&gt;1&lt;/t&gt;&lt;v&gt;1&lt;/v&gt;&lt;r&gt;false&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;double&lt;/vt&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;")%}</rule></rules></field><field allowempty="true" column="Rows" columntype="integer" displayinsimplemode="true" guid="55daad11-7dfd-4034-bbd7-d89ca26b92cd" publicfield="false" spellcheck="false" visibility="none" visible="true"><properties><fieldcaption>Rows</fieldcaption><fielddescription>Sets the number of rows for the text editing area.</fielddescription><validationerrormessage>The value must be greater than 0.</validationerrormessage></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings><rules><rule>{%Rule("Value &gt;= 1", "&lt;rules&gt;&lt;r pos=\"0\" par=\"\" op=\"and\" n=\"MinValue\" &gt;&lt;p n=\"minvalue\"&gt;&lt;t&gt;1&lt;/t&gt;&lt;v&gt;1&lt;/v&gt;&lt;r&gt;false&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;double&lt;/vt&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;")%}</rule></rules></field><field allowempty="true" column="Size" columntype="integer" displayinsimplemode="true" guid="33cc0894-254b-4d1e-8eca-5887efd7a869" publicfield="false" spellcheck="false" visibility="none" visible="true"><properties><fieldcaption>Maximum text length</fieldcaption><fielddescription>Maximum number of characters allowed in the text area.</fielddescription><validationerrormessage>The Size field must be greater than 0.</validationerrormessage></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings><rules><rule>{%Rule("Value &gt;= 1", "&lt;rules&gt;&lt;r pos=\"0\" par=\"\" op=\"and\" n=\"MinValue\" &gt;&lt;p n=\"minvalue\"&gt;&lt;t&gt;1&lt;/t&gt;&lt;v&gt;1&lt;/v&gt;&lt;r&gt;false&lt;/r&gt;&lt;d&gt;&lt;/d&gt;&lt;vt&gt;double&lt;/vt&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;")%}</rule></rules></field><field allowempty="true" column="Wrap" columntype="boolean" displayinsimplemode="true" guid="50fb7a72-0909-40f1-88cf-b48b6169b4ce" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>true</defaultvalue><fieldcaption>Wrap text</fieldcaption><fielddescription>If checked, the text area wraps the text, if unchecked, the horizontal scrollbar is displayed if text is longer than the line.</fielddescription><validationerrormessage>The Size field must be greater than 0.</validationerrormessage></properties><settings><controlname>checkboxcontrol</controlname></settings></field><category name="Watermark"><properties><visible>True</visible></properties></category><field allowempty="true" column="WatermarkText" columnsize="500" columntype="text" guid="2ff75e17-5380-40a0-9ddf-c52c75c06a62" publicfield="false" reftype="Required" resolvedefaultvalue="False" spellcheck="false" visibility="none" visible="true"><properties><fieldcaption>Text</fieldcaption><fielddescription>Sets the text that is displayed when the text editing area has no content entered.</fielddescription></properties><settings><controlname>LocalizableTextBox</controlname><ValueIsContent>False</ValueIsContent></settings></field><field allowempty="true" column="WatermarkCssClass" columnsize="50" columntype="text" guid="cf6c9ebd-9474-44cc-be73-85e5167121c4" publicfield="false" spellcheck="false" visibility="none" visible="true"><properties><fieldcaption>CSS class</fieldcaption><fielddescription>Specifies the CSS class applied to the watermark text. The default value is ''WatermarkText''.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><category name="Filter"><properties><visible>True</visible></properties></category><field allowempty="true" column="FilterType" columnsize="50" columntype="text" guid="f5047e7c-f195-4d0e-a151-48e8c979ef01" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Type</fieldcaption><fielddescription>Specifies which character types can be entered into the text area. If Custom is selected, the Valid characters field will be used in addition to other settings such as Numbers.</fielddescription></properties><settings><controlname>multiplechoicecontrol</controlname><Options>0;Numbers
1;Lowercase letters
2;Uppercase letters
3;Custom</Options><RepeatDirection>vertical</RepeatDirection></settings></field><field allowempty="true" column="FilterMode" columntype="boolean" guid="4ad60133-4d18-490d-b10c-094b53fb2526" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>Mode</fieldcaption><fielddescription>Sets the mode of the filter. If set to Invalid characters, the Filter type must be set to Custom and the forbidden characters must be entered into the Invalid characters field.</fielddescription></properties><settings><controlname>radiobuttonscontrol</controlname><Options>0;Valid characters
1;Invalid characters</Options><RepeatDirection>horizontal</RepeatDirection></settings></field><field allowempty="true" column="ValidChars" columnsize="100" columntype="text" guid="6e1454a7-76e5-48f8-919e-a93dc8311b3f" publicfield="false" spellcheck="false" visibility="none" visible="true"><properties><fieldcaption>Valid characters</fieldcaption><fielddescription>Enter a string consisting of all characters considered valid for the text field. Only applied if "Custom" is specified as the Filter type.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="InvalidChars" columnsize="100" columntype="text" guid="cec48853-d917-4d0a-b870-246905fa0360" publicfield="false" spellcheck="false" visibility="none" visible="true"><properties><fieldcaption>Invalid characters</fieldcaption><fielddescription>Enter a string consisting of all characters considered invalid for the text field. Only applied if "Custom" is specified as the Filter type and "Invalid characters" as the Filter mode.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><category name="Autocomplete"><properties><visible>True</visible></properties></category><field allowempty="true" column="AutoCompleteServicePath" columnsize="1000" columntype="text" guid="a16ceedd-3d93-449a-ae7c-835d62461875" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Service path</fieldcaption><fielddescription>The path to the web service that the extender will pull the word\sentence completions from. If this is not provided, the service method should be a page method.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterEnabled>False</FilterEnabled><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteServiceMethod" columnsize="100" columntype="text" guid="3ffa1146-53db-42ef-ad9b-50bb04679c16" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Service method</fieldcaption><fielddescription>The web service method to be called. The signature of this method must match the following: 
[System.Web.Services.WebMethod]
[System.Web.Script.Services.ScriptMethod]
public string[] GetCompletionList(string prefixText, int count) { ... }
Note that you can replace "GetCompletionList" with a name of your choice, but the return type and parameter name and type must exactly match, including case.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterEnabled>False</FilterEnabled><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteMinimumPrefixLength" columntype="integer" guid="777816d2-ec3e-4e9f-a117-57e980caca0a" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Minimum prefix length</fieldcaption><fielddescription>Minimum number of characters that must be entered before getting suggestions from the web service. The default value is 2.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteCompletionSetCount" columntype="integer" guid="9c2c37bf-429f-48ac-9781-4f0d56463637" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Completion set count</fieldcaption><fielddescription>Number of suggestions to be retrieved from the web service. The default value is 2.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteCompletionInterval" columntype="integer" guid="37108c09-fc27-4852-9158-148cfbe08c64" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Completion interval</fieldcaption><fielddescription>Time in milliseconds before the timer will kick in to get suggestions using the web service. The default value is 2.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteFirstRowSelected" columntype="boolean" guid="766cdd9f-c3e2-43e4-8fd6-ef8d4950bb1c" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>First row selected</fieldcaption><fielddescription>Determines if the first option in the AutoComplete list will be selected by default.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><category name="Autocomplete - design"><properties><visible>True</visible></properties></category><field allowempty="true" column="AutoCompleteCompletionListCssClass" columnsize="200" columntype="text" guid="4674d9fe-436d-42ec-a139-70bee2e4c7a8" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Completion list CSS class</fieldcaption><fielddescription>CSS class that will be used to style the completion list flyout.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterEnabled>False</FilterEnabled><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteCompletionListItemCssClass" columnsize="200" columntype="text" guid="9b03eee5-f1e3-44bf-bee9-49238f0c91d2" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Completion list item CSS class</fieldcaption><fielddescription>CSS class that will be used to style items in the AutoComplete list flyout.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteCompletionListHighlightedItemCssClass" columnsize="200" columntype="text" guid="579196ce-0d2d-4be6-9e43-ed000a96006f" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Completion list highlighted item CSS class</fieldcaption><fielddescription>CSS class that will be used to style highlighted items in the AutoComplete list flyout.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><category name="Autocomplete - advanced settings"><properties><visible>True</visible></properties></category><field allowempty="true" column="AutoCompleteContextKey" columnsize="400" columntype="text" guid="0b99bdf7-7965-407f-ad23-af0a46616a03" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Context key</fieldcaption><fielddescription>User/page specific context provided to an optional overload of the web method described by ServiceMethod/ServicePath. If the context key is used, it should have the same signature with an additional parameter named contextKey of type string:</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterEnabled>False</FilterEnabled><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteDelimiterCharacters" columnsize="50" columntype="text" guid="23235b1b-fc1f-47ef-b6f0-c7b33632fe8f" publicfield="false" visibility="none" visible="true"><properties><fieldcaption>Delimiter characters</fieldcaption><fielddescription>Specifies one or more character(s) used to separate words. The text in the AutoComplete textbox is tokenized using these characters and the webservice completes the last token.</fielddescription></properties><settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><controlname>textboxcontrol</controlname><FilterEnabled>False</FilterEnabled><FilterMode>False</FilterMode><Trim>False</Trim></settings></field><field allowempty="true" column="AutoCompleteShowOnlyCurrentWordInCompletionListItem" columntype="boolean" guid="f1024339-2745-4719-a388-81299fc63d10" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>Show only current word in completion list item</fieldcaption><fielddescription>If true and DelimiterCharacters are specified, then the AutoComplete list items display suggestions for the current word to be completed and do not display the rest of the tokens.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field><field allowempty="true" column="AutoCompleteEnableCaching" columntype="boolean" guid="b175b538-528d-4b29-863c-557925616350" publicfield="false" visibility="none" visible="true"><properties><defaultvalue>false</defaultvalue><fieldcaption>Enable client caching</fieldcaption><fielddescription>Indicates if client side caching should be enabled for the field''s Autocomplete data.</fielddescription></properties><settings><controlname>checkboxcontrol</controlname></settings></field></form>', 0, @userControlResourceID, 0, NULL, 'Provides a field of adjustable size for entering text.', NULL, 0, 0, 0, 0, 'CMS.DocumentEngine.Web.UI', 'CMS.DocumentEngine.Web.UI.AlternativeUrlsConstraintControl')


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @keyCategoryID int;
SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.URL.AlternativeURLMvc')
IF @keyCategoryID IS NOT NULL BEGIN

-- Insert new Setting key
INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSAlternativeUrlUIEnabled', '{$settingskey.cmsalternativeurluienabled$}', '{$settingskey.cmsalternativeurluienabled.description$}', 'False', 'boolean', @keyCategoryID, NULL, 'e9496a8b-02af-4346-acdd-3a02c64ca4ef', getDate(), 1, 'True', NULL, NULL, 0, 0, 0, NULL, '')

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @keyCategoryID int;
SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.URL.AlternativeURLMvc')
IF @keyCategoryID IS NOT NULL BEGIN

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSAlternativeURLsMode', '{$settingskey.cmsalternativeurlsmode$}', '{$settingskey.cmsalternativeurlsmode.description$}', 'REDIRECT', 'string', @keyCategoryID, NULL, '7f5faee3-8e3c-4f19-b107-05a327fc7565', getDate(), 2, 'REDIRECT', NULL, 'RadioButtonsControl', 0, 0, 0, '<settings><Options>REDIRECT;{$settingskey.cmsalternativeurlsmode.redirect$}
REWRITE;{$settingskey.cmsalternativeurlsmode.rewrite$}</Options><RepeatDirection>vertical</RepeatDirection><RepeatLayout>Flow</RepeatLayout><SortItems>False</SortItems></settings>', '')


END

END


GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
          BEGIN

          DECLARE @keyCategoryID int;
          SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.URL.AlternativeURLMvc')
          IF @keyCategoryID IS NOT NULL BEGIN

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
VALUES ('CMSAlternativeURLsErrorMessage', '{$settingskey.cmsalternativeurlserrormessage$}', '{$settingskey.cmsalternativeurlserrormessagedescription$}', NULL, 'longtext', @keyCategoryID, NULL, 'dd237c7d-7b88-42bd-b630-43662da5b25b', getDate(), 5, NULL, NULL, 'LocalizableTextArea', 0, 0, 0, '<settings><ValueIsContent>True</ValueIsContent></settings>', '')


          END

          END

          GO
        
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @keyCategoryID int;
SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.URL.AlternativeURLMvc')
IF @keyCategoryID IS NOT NULL BEGIN

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSAlternativeURLsExcludedURLs', '{$settingskey.cmsalternativeurlsexcludedurls$}', '{$settingskey.cmsalternativeurlsexcludedurlsdescription$}', NULL, 'longtext', @keyCategoryID, NULL, '709338c5-4533-42ae-bb37-af2b3aaee6f0', getDate(), 3, NULL, NULL, NULL, 0, 0, 0, NULL, '')


END

END

GO

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @keyCategoryID int;
SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.URL.AlternativeURLMvc')
IF @keyCategoryID IS NOT NULL BEGIN

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSAlternativeURLsConstraint', '{$settingskey.cmsalternativeurlsconstraint$}', '{$settingskey.cmsalternativeurlsconstraintdescription$}', '^[\w\-\/]+$', 'longtext', @keyCategoryID, NULL, '50a81c99-8273-43ce-a976-cd57b0414796', getDate(), 4, '^[\w\-\/]+$', NULL, 'AlternativeURLsConstraintEditor', 0, 0, 0, '<settings><AutoCompleteEnableCaching>False</AutoCompleteEnableCaching><AutoCompleteFirstRowSelected>False</AutoCompleteFirstRowSelected><AutoCompleteShowOnlyCurrentWordInCompletionListItem>False</AutoCompleteShowOnlyCurrentWordInCompletionListItem><FilterMode>False</FilterMode><Wrap>True</Wrap></settings>', '')


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '7396a5d9-c69d-4a9a-a03f-41d5eee51be2')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'f23e944e-1a51-46f0-8a2e-75bcfc2a70ea')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><category_name_Custom>False</category_name_Custom><category_name_Header>False</category_name_Header><DescriptionLink ismacro="True">{% if (CurrentSite.SiteIsContentOnly) {"ab_testing_mvc"} else {"ab_tests"} @%}</DescriptionLink><DisplayBreadcrumbs>False</DisplayBreadcrumbs><includejquery>False</includejquery></Data>'
    WHERE [ElementGUID] = 'bbc600ae-e00d-4d82-a548-875800839488'


END

END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @helpTopicUIElementID int;
SET @helpTopicUIElementID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'bbc600ae-e00d-4d82-a548-875800839488')
IF @helpTopicUIElementID IS NOT NULL BEGIN

INSERT [CMS_HelpTopic] ([HelpTopicUIElementID], [HelpTopicName], [HelpTopicLink], [HelpTopicLastModified], [HelpTopicGUID], [HelpTopicOrder], [HelpTopicVisibilityCondition])
 VALUES (@helpTopicUIElementID, 'Create and evaluate A/B tests', 'ab_testing_mvc', getDate(), 'e92bd85a-dcdf-48da-aba6-6f027925463e', 2, '{%CurrentSite.SiteIsContentOnly@%}')

INSERT [CMS_HelpTopic] ([HelpTopicUIElementID], [HelpTopicName], [HelpTopicLink], [HelpTopicLastModified], [HelpTopicGUID], [HelpTopicOrder], [HelpTopicVisibilityCondition])
 VALUES (@helpTopicUIElementID, 'Create custom conversion types', 'ab_testing_custom_conversions_mvc', getDate(), 'f42767ea-0074-4a0a-8660-8d7be241bb42', 3, '{%CurrentSite.SiteIsContentOnly@%}')

INSERT [CMS_HelpTopic] ([HelpTopicUIElementID], [HelpTopicName], [HelpTopicLink], [HelpTopicLastModified], [HelpTopicGUID], [HelpTopicOrder], [HelpTopicVisibilityCondition])
 VALUES (@helpTopicUIElementID, 'Enable A/B testing', 'ab_testing_enabling_mvc', getDate(), '2ae84bcd-04df-43fa-8992-c93536ed8a7e', 1, '{%CurrentSite.SiteIsContentOnly@%}')

INSERT [CMS_HelpTopic] ([HelpTopicUIElementID], [HelpTopicName], [HelpTopicLink], [HelpTopicLastModified], [HelpTopicGUID], [HelpTopicOrder], [HelpTopicVisibilityCondition])
 VALUES (@helpTopicUIElementID, 'Log conversions in code', 'ab_testing_custom_conversions_mvc', getDate(), 'c96ea5c9-fbae-4191-b50f-b383548ddf92', 4, '{%CurrentSite.SiteIsContentOnly@%}')


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @helpTopicUIElementID int;
SET @helpTopicUIElementID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'bbc600ae-e00d-4d82-a548-875800839488')
IF @helpTopicUIElementID IS NOT NULL BEGIN

UPDATE [CMS_HelpTopic] SET 
        [HelpTopicOrder] = 7,
        [HelpTopicVisibilityCondition] = '{%!CurrentSite.SiteIsContentOnly@%}'
    WHERE [HelpTopicGUID] = 'b19b3797-568b-4fb6-9350-e7185f7be179'

UPDATE [CMS_HelpTopic] SET 
        [HelpTopicOrder] = 6,
        [HelpTopicVisibilityCondition] = '{%!CurrentSite.SiteIsContentOnly@%}'
    WHERE [HelpTopicGUID] = 'c924b592-83d9-4a8b-8c85-aa3d79d3974b'

UPDATE [CMS_HelpTopic] SET 
        [HelpTopicOrder] = 5,
        [HelpTopicVisibilityCondition] = '{%!CurrentSite.SiteIsContentOnly@%}'
    WHERE [HelpTopicGUID] = '512683dc-b2b6-4c03-a78d-dee03642924e'

UPDATE [CMS_HelpTopic] SET 
        [HelpTopicOrder] = 8,
        [HelpTopicVisibilityCondition] = '{%!CurrentSite.SiteIsContentOnly@%}'
    WHERE [HelpTopicGUID] = '7953544b-8f47-41df-a6f6-6c33b0b6296b'


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

INSERT INTO [CMS_ScheduledTask]
           ([TaskName]
           ,[TaskDisplayName]
           ,[TaskAssemblyName]
           ,[TaskClass]
           ,[TaskInterval]
           ,[TaskData]
           ,[TaskLastRunTime]
           ,[TaskNextRunTime]
           ,[TaskLastResult]
           ,[TaskSiteID]
           ,[TaskDeleteAfterLastRun]
           ,[TaskServerName]
           ,[TaskGUID]
           ,[TaskLastModified]
           ,[TaskExecutions]
           ,[TaskResourceID]
           ,[TaskRunInSeparateThread]
           ,[TaskUseExternalService]
           ,[TaskAllowExternalService]
           ,[TaskLastExecutionReset]
           ,[TaskCondition]
           ,[TaskRunIndividually]
           ,[TaskUserID]
           ,[TaskType]
           ,[TaskObjectType]
           ,[TaskObjectID]
           ,[TaskExecutingServerName]
           ,[TaskEnabled]
           ,[TaskIsRunning])
     SELECT
           'ValidateAlternativeURLs'
           ,'Validate alternative URLs'
           ,'CMS.DocumentEngine'
           ,'CMS.DocumentEngine.AlternativeUrlValidatorTask'
           ,'day;5/15/2019 3:00:00 AM;1;Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday'
           ,''
           ,NULL
           ,NULL
           ,NULL
           ,[SiteID]
           ,0
           ,''
           ,NEWID()
           ,GETDATE()
           ,0
           ,NULL
           ,1
           ,0
           ,1
           ,NULL
           ,''
           ,0
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,1
           ,0 
       FROM [CMS_Site] WHERE [SiteIsContentOnly] = 1 
				AND NOT EXISTS (SELECT 1 FROM [CMS_ScheduledTask] WHERE [TaskName] = 'ValidateAlternativeURLs' AND [TaskSiteID] = [SiteID])
END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

UPDATE [CMS_Country] SET 
        [CountryDisplayName] = 'North Macedonia'
    WHERE [CountryGUID] = '9dbeaf9b-495a-49e8-8084-543124d66344'


END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

UPDATE [CMS_ScheduledTask] SET 
        [TaskInterval] = 'hour;8/16/2011 12:00:30 PM;1;00:00:00;23:59:00;Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday'
    WHERE [TaskGUID] = '7c93f262-fdad-4571-a568-eb0ad882825d' AND [TaskResourceID] IS NULL


END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @reportCategoryID int;
SET @reportCategoryID = (SELECT TOP 1 [CategoryID] FROM [Reporting_ReportCategory] WHERE [CategoryGUID] = 'e771dfce-a2a4-4fc2-aaca-122b4c3d8624')
IF @reportCategoryID IS NOT NULL BEGIN

UPDATE [Reporting_Report] SET 
        [ReportLayout] = '<p>%%control:ReportGraph?mvcabtestaverageconversionvalue.dayreport.Graph%%</p>
'
    WHERE [ReportGUID] = '86686015-2bdb-430a-bf26-0adabc4eca68'

UPDATE [Reporting_Report] SET 
        [ReportLayout] = '<p>%%control:ReportGraph?mvcabtestaverageconversionvalue.monthreport.Graph%%</p>
'
    WHERE [ReportGUID] = '5a438e0f-8ed4-44e6-a9f1-9a06931cb826'

UPDATE [Reporting_Report] SET 
        [ReportLayout] = '<p>%%control:ReportGraph?mvcabtestaverageconversionvalue.weekreport.graph%%</p>
'
    WHERE [ReportGUID] = '9908d225-250d-4bc7-ae7a-97d8ebf3b59b'


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '5a438e0f-8ed4-44e6-a9f1-9a06931cb826')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '89004552-80ac-461c-b60e-eeab6d6ab881'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '86686015-2bdb-430a-bf26-0adabc4eca68')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '9be0ab2f-a355-4d31-a7c0-85e8136b8847'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '9908d225-250d-4bc7-ae7a-97d8ebf3b59b')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = 'f8c8fa65-bd08-4ed6-aa38-13767a6ca261'


END

END


GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29 BEGIN

DECLARE @keyCategoryID1 int;
SET @keyCategoryID1 = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.Files.Security')
IF @keyCategoryID1 IS NOT NULL BEGIN

UPDATE [CMS_SettingsKey] SET 
        [KeyDefaultValue] = 'pdf;doc;docx;ppt;pptx;xls;xlsx;xml;bmp;gif;jpg;jpeg;png;wav;mp3;mp4;mpg;mpeg;mov;avi;rar;zip;txt;rtf'
    WHERE [KeyName] = 'CMSUploadExtensions' AND [SiteID] IS NULL AND [KeyCategoryID] = @keyCategoryID1
END

DECLARE @keyCategoryID2 int;
SET @keyCategoryID2 = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.MediaLibraries.Security')
IF @keyCategoryID2 IS NOT NULL BEGIN

UPDATE [CMS_SettingsKey] SET 
        [KeyDefaultValue] = 'pdf;doc;docx;ppt;pptx;xls;xlsx;xml;bmp;gif;jpg;jpeg;png;wav;mp3;mp4;mpg;mpeg;mov;avi;rar;zip;txt;rtf;webm;ogg;wav;ogv;oga;flv;m4v'
    WHERE [KeyName] = 'CMSMediaFileAllowedExtensions' AND [SiteID] IS NULL AND [KeyCategoryID] = @keyCategoryID2
END

DECLARE @keyCategoryID3 int;
SET @keyCategoryID3 = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.Versioning.UseObjectVersioningFor')
IF @keyCategoryID3 IS NOT NULL BEGIN

UPDATE [CMS_SettingsKey] SET 
        [KeyDefaultValue] = 'pdf;doc;docx;ppt;pptx;xls;xlsx;xml;bmp;gif;jpg;jpeg;png;txt;rtf'
    WHERE [KeyName] = 'CMSVersioningExtensionsMediaFile' AND [SiteID] IS NULL AND [KeyCategoryID] = @keyCategoryID3
END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @keyCategoryID int;
SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.SocialNetworks.AuthenticationLinkedIn')
IF @keyCategoryID IS NOT NULL BEGIN

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSLinkedInSignInPermissionScope', '{$settingskey.cmslinkedinsigninpermissionscope$}', '{$settingskey.cmslinkedinsigninpermissionscope.description$}', 'r_liteprofile', 'string', @keyCategoryID, NULL, 'bfcf7ad6-164d-4713-b552-e13fc9bc3db1', getDate(), 4, 'r_liteprofile', NULL, NULL, 0, 0, 0, NULL, '')


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '1cc5415a-31bd-4824-b540-9d34846ce35a')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY%</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>y</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxisformat>{0.0\%}</yaxisformat><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '0ab24fe2-55f9-46f5-be19-44a9c4950830'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '265faf9e-0cdf-4f6d-80bc-b0e373da643f')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '409ebc0a-e1b3-4e4d-a1ee-d755f94b2d9b'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '5a438e0f-8ed4-44e6-a9f1-9a06931cb826')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '89004552-80ac-461c-b60e-eeab6d6ab881'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '5f600553-d314-4e99-bf16-4a0ea51fbc13')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY%</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxisformat>{0.0\%}</yaxisformat><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '1f9bcfe1-1327-406f-8258-d50a0139317b'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '661973b3-2247-424a-8b1c-939bb441eadf')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = 'ab98e4ca-f7e5-4fd7-97b2-0a305a8f2b15'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '73f30508-1bbc-450b-8d48-2829b2b00eec')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '06eb886d-e8d1-41b6-8293-713254a66ab6'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '7d1b4de6-f27e-465b-ab9a-0b6f4dbc9278')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><columnwidth>20</columnwidth><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>y</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '6a6a10d2-7d31-48b0-9893-abedd9d7e051'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '86686015-2bdb-430a-bf26-0adabc4eca68')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{dddd, MMMM d, yyyy}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '9be0ab2f-a355-4d31-a7c0-85e8136b8847'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '9567651c-efe0-4f8a-9a6b-c36a126923dc')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX{y}  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>y</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '139aaef2-b31b-45de-acee-4b86054eb5ea'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = '9908d225-250d-4bc7-ae7a-97d8ebf3b59b')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>d</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = 'f8c8fa65-bd08-4ed6-aa38-13767a6ca261'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = 'c30a7bd2-6c53-4c77-bb0e-afcce8384d16')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>{yyyy}</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '1acb3067-cffb-41ca-b3b1-e3b32a68659f'


END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @graphReportID int;
SET @graphReportID = (SELECT TOP 1 [ReportID] FROM [Reporting_Report] WHERE [ReportGUID] = 'ed3cb1dc-4b0d-48b7-b45d-a30cb9944058')
IF @graphReportID IS NOT NULL BEGIN

UPDATE [Reporting_ReportGraph] SET 
        [GraphSettings] = '<CustomData><bardrawingstyle>Bar</bardrawingstyle><barorientation>Vertical</barorientation><baroverlay>False</baroverlay><borderskinstyle>None</borderskinstyle><chartareaborderstyle>NotSet</chartareaborderstyle><chartareagradient>None</chartareagradient><displayitemvalue>False</displayitemvalue><exportenabled>True</exportenabled><legendbordersize>0</legendbordersize><legendborderstyle>NotSet</legendborderstyle><legendinside>False</legendinside><legendposition>Bottom</legendposition><linedrawinstyle>Line</linedrawinstyle><piedoughnutradius>70</piedoughnutradius><piedrawingdesign>Default</piedrawingdesign><piedrawingstyle>Doughnut</piedrawingstyle><pielabelstyle>Outside</pielabelstyle><pieshowpercentage>False</pieshowpercentage><plotareabordersize>0</plotareabordersize><plotareaborderstyle>NotSet</plotareaborderstyle><plotareagradient>None</plotareagradient><querynorecordtext>No data found.</querynorecordtext><reverseyaxis>False</reverseyaxis><seriesbordersize>4</seriesbordersize><seriesborderstyle>Solid</seriesborderstyle><seriesgradient>None</seriesgradient><seriesitemtooltip>#VALX  -  #SER: #VALY%</seriesitemtooltip><seriessymbols>Circle</seriessymbols><showas3d>False</showas3d><showmajorgrid>True</showmajorgrid><stackedbardrawingstyle>Bar</stackedbardrawingstyle><stackedbarmaxstacked>False</stackedbarmaxstacked><subscriptionenabled>True</subscriptionenabled><tenpowers>False</tenpowers><titleposition>Center</titleposition><valuesaspercent>False</valuesaspercent><xaxisangle>0</xaxisangle><xaxisformat>{yyyy}</xaxisformat><xaxissort>True</xaxissort><xaxistitleposition>Center</xaxistitleposition><yaxisformat>{0.0\%}</yaxisformat><yaxistitleposition>Center</yaxistitleposition><yaxisusexaxissettings>True</yaxisusexaxissettings></CustomData>'
    WHERE [GraphGUID] = '52ef44c0-3166-4dda-a85e-2a10460ab43a'


END

END


GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @helpTopicUIElementID int;
SET @helpTopicUIElementID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = '0ebd31b7-73bc-4ed3-a06e-aa2f41a94c3c')
IF @helpTopicUIElementID IS NOT NULL BEGIN

INSERT [CMS_HelpTopic] ([HelpTopicUIElementID], [HelpTopicName], [HelpTopicLink], [HelpTopicLastModified], [HelpTopicGUID], [HelpTopicOrder], [HelpTopicVisibilityCondition])
 VALUES (@helpTopicUIElementID, 'Work with page templates', 'page_templates_using_mvc', getDate(), '8ff749e1-fe6f-4c01-93b4-08a9f63bdebd', 1, NULL)

INSERT [CMS_HelpTopic] ([HelpTopicUIElementID], [HelpTopicName], [HelpTopicLink], [HelpTopicLastModified], [HelpTopicGUID], [HelpTopicOrder], [HelpTopicVisibilityCondition])
 VALUES (@helpTopicUIElementID, 'Develop page templates', 'page_templates_developing_mvc', getDate(), 'b71e3914-bd59-4ad4-9b41-3aaedcc81b33', 2, NULL)

INSERT [CMS_HelpTopic] ([HelpTopicUIElementID], [HelpTopicName], [HelpTopicLink], [HelpTopicLastModified], [HelpTopicGUID], [HelpTopicOrder], [HelpTopicVisibilityCondition])
 VALUES (@helpTopicUIElementID, 'Limit which page templates are available', 'page_templates_filtering_mvc', getDate(), '6fbf1cb2-43cc-40ed-b30a-b8d1aaa5dd73', 3, NULL)

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 29
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'bbc600ae-e00d-4d82-a548-875800839488')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = 'f23e944e-1a51-46f0-8a2e-75bcfc2a70ea')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><AllowSubTabs>True</AllowSubTabs><category_name_Custom>False</category_name_Custom><DisplayBreadcrumbs>True</DisplayBreadcrumbs><DisplayTitleInTabs>False</DisplayTitleInTabs><EnableSingleObjectsOnDashboard>True</EnableSingleObjectsOnDashboard><includejquery>False</includejquery><Javascript>// Refresh page after dialog close
onCloseDialog = function() {
  if (wopener) {
    if (wopener.RefreshTree) { wopener.RefreshTree(); }
    wopener.location.replace(wopener.location);
  }
  return true;
}</Javascript><ObjectType>om.abtest</ObjectType><RememberSelectedTab>False</RememberSelectedTab><TitleText>{$analytics_codename.abtests$}</TitleText></Data>'
    WHERE [ElementGUID] = '3f457537-ad36-4a89-a443-089ad5bc9cdb'


END

END

END
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 36
BEGIN

DECLARE @elementParentID int;
SET @elementParentID = (SELECT TOP 1 [ElementID] FROM [CMS_UIElement] WHERE [ElementGUID] = 'd88037da-8491-49c5-bcf5-93fc5b63f83b')
IF @elementParentID IS NOT NULL BEGIN

DECLARE @elementPageTemplateID int;
SET @elementPageTemplateID = (SELECT TOP 1 [PageTemplateID] FROM [CMS_PageTemplate] WHERE [PageTemplateGUID] = '8136b750-a785-438f-a412-32212cd4dde6')
IF @elementPageTemplateID IS NOT NULL BEGIN

DECLARE @elementResourceID int;
SET @elementResourceID = (SELECT TOP 1 [ResourceID] FROM [CMS_Resource] WHERE [ResourceGUID] = '4dcb3b9b-8bee-4a3e-97dd-610f6e5623a4')
IF @elementResourceID IS NOT NULL BEGIN

UPDATE [CMS_UIElement] SET 
        [ElementProperties] = '<Data><AllowSubTabs>True</AllowSubTabs><DisplayBreadcrumbs>True</DisplayBreadcrumbs><DisplayTitleInTabs>False</DisplayTitleInTabs><includejquery>False</includejquery><ObjectParameterID>roleid</ObjectParameterID><RememberSelectedTab>False</RememberSelectedTab></Data>'
    WHERE [ElementGUID] = '229e2259-b750-49ad-917c-147a916d26b1'

END

END

END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 36
BEGIN

UPDATE [CMS_Query] SET 
		[QueryText] = '-- Delete server tasks that are violating foreign key policy because of nolocks in inserting servertaks
DELETE FROM [CMS_WebFarmServerTask] WHERE [ServerID] NOT IN (SELECT [ServerID] FROM [CMS_WebFarmServer])
-- Delete old tasks that were already processed
DELETE TOP(@deleteTaskCount) FROM [CMS_WebFarmTask] WHERE [TaskIsAnonymous] = 0 AND [TaskID] NOT IN (SELECT [TaskID] FROM [CMS_WebFarmServerTask]) AND [TaskCreated] < @deleteOlderThan
-- Return number of deleted tasks
SELECT @@ROWCOUNT' 
	WHERE [QueryGUID] = '5EDB9E70-B9C0-4684-9930-7BDC214D65E9'
	
END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 38
BEGIN

DECLARE @keyCategoryID int;
SET @keyCategoryID = (SELECT TOP 1 [CategoryID] FROM [CMS_SettingsCategory] WHERE [CategoryName] = 'CMS.Content.General')
IF @keyCategoryID IS NOT NULL BEGIN

DECLARE @keyGUID uniqueidentifier;
SET @keyGUID = 'fca0ad94-9233-428e-b46a-0fe64d745cdc';
IF NOT EXISTS (SELECT TOP 1 [KeyID] FROM [CMS_SettingsKey] WHERE [KeyGUID] = @keyGUID) BEGIN

INSERT [CMS_SettingsKey] ([KeyName], [KeyDisplayName], [KeyDescription], [KeyValue], [KeyType], [KeyCategoryID], [SiteID], [KeyGUID], [KeyLastModified], [KeyOrder], [KeyDefaultValue], [KeyValidation], [KeyEditingControlPath], [KeyIsGlobal], [KeyIsCustom], [KeyIsHidden], [KeyFormControlSettings], [KeyExplanationText])
 VALUES ('CMSRichTextEditorLicense', '{$settingkey.richtexteditorlicense$}', '', '8JF3bG3A5D3E2F2B2yQNDMIJd1IQNSEa2EUAf1XVFQa1EaD4D3B2C2A18A14B3C9B3E3==', 'string', @keyCategoryID, NULL, @keyGUID, getDate(), 5, '8JF3bG3A5D3E2F2B2yQNDMIJd1IQNSEa2EUAf1XVFQa1EaD4D3B2C2A18A14B3C9B3E3==', NULL, NULL, 1, 0, 1, NULL, '')

END

END

END
	
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 52
BEGIN

DECLARE @reportCursor CURSOR;
SET @reportCursor = CURSOR FORWARD_ONLY FOR SELECT [ReportGUID], [ReportParameters] FROM [Reporting_Report] 
WHERE [ReportGUID] IN ('E1AF956A-51EA-484A-B3DF-839777763A67','C42056E1-ABE2-4C66-A5B0-0794625C3607','1D1EBF2A-B2CE-464F-AB25-86FC8EFA9BBD','5EBBF498-45A3-4B78-8715-345875609F30','1667CA64-F0DC-420F-AEEC-702D8DD1EE38','065359C7-A6FC-4486-867D-D64B0BE7799B','D6885F43-CE12-45E4-AF3F-01AA358CA6CD','A53E7930-E7FB-4585-B50B-C864D309CE43','6AE76DA6-0E70-469C-908C-03981E99BDDE','DF8D1C5A-CCB4-4424-81F3-32605D98AAE5','E4DA6FC0-1322-4517-9997-1D7434DBF2CF','DFB92F4B-4AF0-4BB6-B516-1F376DD4617A','3C464397-CEC6-48F2-98E4-D6832FA96637','C8D74EF4-4C82-4AB6-B45B-E165FFF89E8C','566E7F3D-A119-4EB0-A9C5-3B7AB21064DF','51CBDE00-8030-47BF-926C-8BAB8FB8DF9B','E3CE2ABE-283B-4221-B104-6926709253A3','6BA74765-5214-4348-BEE8-A15EDC1DE794','24E1E6F9-33DA-47EA-A5E5-C7EEC0266E5A','769E210F-20A6-4001-BAD7-4B86EC079027')

DECLARE @currentGuid uniqueidentifier;
DECLARE @reportParameters xml;

OPEN @reportCursor

FETCH NEXT FROM @reportCursor INTO @currentGuid, @reportParameters
WHILE @@FETCH_STATUS = 0
BEGIN

	IF (@reportParameters.exist('form/field[@column="PeriodType"]/properties/defaultvalue') = 1)
	BEGIN
		SET @reportParameters.modify('replace value of (form/field[@column="PeriodType"]/properties/defaultvalue/text())[1] with "day"');
		
		UPDATE [Reporting_Report] SET [ReportParameters] = CONVERT(nvarchar(max), @reportParameters) WHERE [ReportGUID] = @currentGuid
	END

	FETCH NEXT FROM @reportCursor INTO @currentGuid, @reportParameters
END

END
	
GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 55
BEGIN
	-- SM_LinkedInPost - Update LinkedInPostUpdateKey to the new API v2 format
	-- Old Format: UPDATE-c{entityid}-{activityid}, for example: 'UPDATE-c6177438-6252923622642540544'
	-- New Format: urn:li:share:{activityid}
	UPDATE [SM_LinkedInPost]
	SET [LinkedInPostUpdateKey] = 'urn:li:share:' + SUBSTRING([LinkedInPostUpdateKey], CHARINDEX('-', [LinkedInPostUpdateKey], CHARINDEX('-', [LinkedInPostUpdateKey]) + 1) + 1, LEN([LinkedInPostUpdateKey]))
	WHERE [LinkedInPostUpdateKey] LIKE 'UPDATE-%'

END
	
GO


DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 70
BEGIN

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Proc_CMS_Email_DeleteArchived]') AND type = (N'P'))
	BEGIN
	EXEC('ALTER PROCEDURE [Proc_CMS_Email_DeleteArchived]
	@SiteID int,
	@ExpirationDate datetime2(7),
	@BatchSize int
AS
BEGIN
SET NOCOUNT ON;

-- Code for archived email status
DECLARE @Archived AS int = 3
	
-- Declare table for emails
CREATE TABLE #Emails (EmailID int NOT NULL, PRIMARY KEY(EmailID))
BEGIN
    -- Get global e-mails
    IF ((@SiteID = 0) OR (@SiteID IS NULL))
		INSERT INTO #Emails 
            SELECT TOP(@BatchSize) EmailID FROM CMS_Email 
				WHERE (EmailLastSendAttempt <= @ExpirationDate) AND (EmailSiteID IS NULL OR EmailSiteID = 0) AND (EmailStatus = @Archived)

	-- Get all e-mails attached to the site			
	ELSE                   
		INSERT INTO #Emails 
			SELECT TOP(@BatchSize) EmailID FROM CMS_Email 
				WHERE (EmailLastSendAttempt <= @ExpirationDate) AND (EmailSiteID = @SiteID) AND (EmailStatus = @Archived)
END

-- Delete attachment binding 
DELETE FROM [CMS_AttachmentForEmail] WHERE EmailID IN (SELECT EmailID FROM #Emails)
    
-- Delete user binding
DELETE FROM [CMS_EmailUser] WHERE EmailID IN (SELECT EmailID FROM #Emails)
	
-- Delete all attachments that have no bindings
DELETE FROM CMS_EmailAttachment WHERE AttachmentID NOT IN (SELECT DISTINCT AttachmentID FROM CMS_AttachmentForEmail)
    
-- Delete e-mails
DELETE FROM [CMS_Email] WHERE EmailID IN (SELECT EmailID FROM #Emails)
      
DROP TABLE #Emails
END')
END

END
	
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 81
BEGIN

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Proc_CMS_Email_DeleteArchived]') AND type = (N'P'))
BEGIN
	EXEC('ALTER PROCEDURE [Proc_CMS_Email_DeleteArchived]
	@SiteID int,
	@ExpirationDate datetime2(7),
	@BatchSize int
AS
BEGIN
SET NOCOUNT ON;

-- Code for archived email status
DECLARE @Archived AS int = 3

-- Declare table for emails
CREATE TABLE #Emails (EmailID int NOT NULL, PRIMARY KEY(EmailID))

-- Get global e-mails
IF ((@SiteID = 0) OR (@SiteID IS NULL))
	INSERT INTO #Emails
        SELECT TOP(@BatchSize) EmailID FROM CMS_Email
			WHERE (EmailLastSendAttempt <= @ExpirationDate) AND (EmailSiteID IS NULL OR EmailSiteID = 0) AND (EmailStatus = @Archived)

-- Get all e-mails attached to the site
ELSE
	INSERT INTO #Emails
		SELECT TOP(@BatchSize) EmailID FROM CMS_Email
			WHERE (EmailLastSendAttempt <= @ExpirationDate) AND (EmailSiteID = @SiteID) AND (EmailStatus = @Archived)

-- Delete attachment binding
DELETE FROM [CMS_AttachmentForEmail] WHERE EmailID IN (SELECT EmailID FROM #Emails)

-- Delete user binding
DELETE FROM [CMS_EmailUser] WHERE EmailID IN (SELECT EmailID FROM #Emails)

-- Delete all attachments that have no bindings
DELETE FROM CMS_EmailAttachment WHERE AttachmentID NOT IN (SELECT DISTINCT AttachmentID FROM CMS_AttachmentForEmail)

-- Delete e-mails
DELETE FROM [CMS_Email] WHERE EmailID IN (SELECT EmailID FROM #Emails)

-- Return number of deleted emails
SELECT @@ROWCOUNT

DROP TABLE #Emails
END')
END

END
	
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 85
BEGIN

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'View_CMS_Relationship_Joined')
BEGIN
	EXEC('ALTER VIEW [View_CMS_Relationship_Joined] AS
	SELECT	LeftTree.NodeID AS LeftNodeID, LeftTree.NodeGUID AS LeftNodeGUID, LeftTree.NodeName AS LeftNodeName, LeftTree.NodeSiteID as LeftNodeSiteID,
		CMS_RelationshipName.RelationshipName, CMS_RelationshipName.RelationshipNameID, RightTree.NodeID AS RightNodeID, RightTree.NodeGUID AS RightNodeGUID, 
		RightTree.NodeName AS RightNodeName, RightTree.NodeSiteID as RightNodeSiteID, CMS_RelationshipName.RelationshipDisplayName,
		CMS_Relationship.RelationshipCustomData, LeftTree.NodeClassID AS LeftClassID, RightTree.NodeClassID AS RightClassID, CMS_Relationship.RelationshipID, 
		CMS_Relationship.RelationshipOrder
	FROM CMS_Relationship INNER JOIN
		CMS_Tree AS LeftTree ON CMS_Relationship.LeftNodeID = LeftTree.NodeID INNER JOIN
		CMS_Tree AS RightTree ON CMS_Relationship.RightNodeID = RightTree.NodeID INNER JOIN
		CMS_RelationshipName ON CMS_Relationship.RelationshipNameID = CMS_RelationshipName.RelationshipNameID')
END

END

GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 89
BEGIN

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Proc_CMS_Email_DeleteArchived]') AND type = (N'P'))
BEGIN
	EXEC('ALTER PROCEDURE [Proc_CMS_Email_DeleteArchived]
	@SiteID int,
	@ExpirationDate datetime2(7),
	@BatchSize int
AS
BEGIN
SET NOCOUNT ON;

-- Code for archived email status
DECLARE @Archived AS int = 3

-- Declare table for emails
CREATE TABLE #Emails (EmailID int NOT NULL, PRIMARY KEY(EmailID))

-- Get global e-mails
IF ((@SiteID = 0) OR (@SiteID IS NULL))
	INSERT INTO #Emails
        SELECT TOP(@BatchSize) EmailID FROM CMS_Email
			WHERE (EmailLastSendAttempt <= @ExpirationDate) AND (EmailSiteID IS NULL OR EmailSiteID = 0) AND (EmailStatus = @Archived)

-- Get all e-mails attached to the site
ELSE
	INSERT INTO #Emails
		SELECT TOP(@BatchSize) EmailID FROM CMS_Email
			WHERE (EmailLastSendAttempt <= @ExpirationDate) AND (EmailSiteID = @SiteID) AND (EmailStatus = @Archived)

-- Delete attachment binding
DELETE FROM [CMS_AttachmentForEmail] WHERE EmailID IN (SELECT EmailID FROM #Emails)

-- Delete user binding
DELETE FROM [CMS_EmailUser] WHERE EmailID IN (SELECT EmailID FROM #Emails)

-- Delete e-mails
DELETE FROM [CMS_Email] WHERE EmailID IN (SELECT EmailID FROM #Emails)

-- Return number of deleted emails
SELECT @@ROWCOUNT

DROP TABLE #Emails
END')
END

END
	
GO

DECLARE @HOTFIXVERSION INT;
SET @HOTFIXVERSION = (SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = N'CMSHotfixVersion')
IF @HOTFIXVERSION < 89
BEGIN

DECLARE @classID int;
SET @classID = (SELECT TOP 1 [ClassID] FROM [CMS_Class] WHERE [ClassName] = 'cms.EmailAttachment')
IF @classID IS NOT NULL BEGIN

INSERT [CMS_Query] ([QueryName], [QueryTypeID], [QueryText], [QueryRequiresTransaction], [ClassID], [QueryIsLocked], [QueryLastModified], [QueryGUID], [QueryIsCustom], [QueryConnectionString])
 VALUES ('deleteattachmentswithnobindings', 0, 'DELETE FROM CMS_EmailAttachment WHERE AttachmentID NOT IN (SELECT DISTINCT AttachmentID FROM CMS_AttachmentForEmail)', 0, @classID, 0, getDate(), '7aa953a6-95e3-44c6-b57f-82735ce322eb', 0, 'CMSConnectionString')

END

END

GO
/* ----------------------------------------------------------------------------*/
/* This SQL command must be at the end and must contain current hotfix version */
/* ----------------------------------------------------------------------------*/
UPDATE [CMS_SettingsKey] SET KeyValue = '92' WHERE KeyName = N'CMSHotfixVersion'
GO
