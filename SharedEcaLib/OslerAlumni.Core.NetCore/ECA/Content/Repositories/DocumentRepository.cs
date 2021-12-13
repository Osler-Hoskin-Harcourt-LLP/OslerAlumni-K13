using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using ECA.Content.Extensions;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Http;

namespace ECA.Content.Repositories
{
    /// <inheritdoc />
    public class DocumentRepository
        : IDocumentRepository
    {
        #region "Constants"

        protected static readonly string[] BaseColumnNames =
        {
            nameof(TreeNode.ClassName),
            nameof(TreeNode.NodeID),
            nameof(TreeNode.NodeGUID),
            nameof(TreeNode.NodeAlias),
            nameof(TreeNode.NodeAliasPath),
            nameof(TreeNode.NodeSiteID),
            nameof(TreeNode.NodeHasChildren),
            nameof(TreeNode.DocumentID),
            nameof(TreeNode.DocumentCulture),
            //nameof(TreeNode.DocumentMenuItemHideInNavigation), TODO##
            nameof(TreeNode.DocumentName),
            //nameof(TreeNode.DocumentNamePath),
            nameof(TreeNode.DocumentPageDescription),
            nameof(TreeNode.DocumentPageTitle)
        };

        protected static readonly string[] TreeNodeColumnNames =
            typeof(TreeNode)
                .GetProperties(
                    BindingFlags.Public | BindingFlags.Instance)
                .Select(p =>
                {
                    var dbAttribute = p.GetCustomAttribute<DatabaseFieldAttribute>();

                    if (dbAttribute == null)
                    {
                        return null;
                    }

                    if (!string.IsNullOrWhiteSpace(dbAttribute.ColumnName))
                    {
                        return dbAttribute.ColumnName;
                    }

                    return p.Name;
                })
                .Where(cn => !string.IsNullOrWhiteSpace(cn))
                .ToArray();

        #endregion

        #region "Private fields"

        protected readonly IDocumentRelationshipRepository _documentRelationshipRepository;
        protected readonly IEventLogRepository _eventLogRepository;
        protected readonly ContextConfig _context;

        #endregion

        #region "Properties"

        public virtual string[] DefaultColumnNames
            => BaseColumnNames;

        #endregion

        public DocumentRepository(
            IDocumentRelationshipRepository documentRelationshipRepository,
            IEventLogRepository eventLogRepository,
            ContextConfig context)
        {
            _documentRelationshipRepository = documentRelationshipRepository;
            _eventLogRepository = eventLogRepository;
            _context = context;
        }

        #region "Methods"

        public virtual TreeNode GetDocument(
            Guid nodeGuid,
            string pageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false,
            string cultureName = null,
            string siteName = null)
        {
            return GetDocuments(
                    new[] { nodeGuid },
                    pageTypeName,
                    ignorePublishedState,
                    columnNames,
                    includeAllCoupledColumns,
                    cultureName,
                    siteName)
                ?.FirstOrDefault();
        }

        public virtual string GetDocumentClassName(
            int nodeId,
            out string siteName)
        {
            siteName = SiteContext.CurrentSiteName;

            try
            {
                var query = DocumentHelper.GetDocuments()
                    .TopN(1)
                    .Columns(nameof(TreeNode.ClassName))
                    .WhereEquals(
                        nameof(TreeNode.NodeID),
                        nodeId);

                if (!string.IsNullOrWhiteSpace(siteName))
                {
                    query = query.OnSite(siteName);
                }

                return query.FirstOrDefault()
                    ?.ClassName;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetDocumentClassName),
                    ex);

                return null;
            }
        }

        public virtual IEnumerable<TreeNode> GetDocuments(
            string pageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false,
            string cultureName = null,
            string siteName = null,
            IEnumerable<Guid> nodeGuids = null,
            string path = null,
            WhereCondition whereCondition = null,
            int? top = null,
            OrderDirection? orderDirection  = null,
            string[] orderByColumns = null)
        {
            try
            {
                columnNames = columnNames?.ToList();

                var querySettings =
                    new DocumentQuerySettings(
                        columnNames,
                        cultureName,
                        siteName,
                        _context,
                        DefaultColumnNames)
                    {
                        IgnorePublishedState = ignorePublishedState,
                        IncludeAllCoupledColumns = includeAllCoupledColumns,
                        NodeGuids = nodeGuids?.ToList(),
                        Path = path,
                        Top = top,
                        OrderDirection = orderDirection,
                        OrderByColumns = orderByColumns
                    };

                InfoDataSet<TreeNode> docs;

                //TODO : Clean this

                if (string.IsNullOrWhiteSpace(pageTypeName))
                {
                    if (whereCondition != null)
                    {
                        docs = GetMultiDocumentQueryResults(
                            querySettings,  query => query.Where(whereCondition) 
                        );
                    }
                    else
                    {
                        docs = GetMultiDocumentQueryResults(
                            querySettings
                        );
                    }
                }
                else
                {
                    if (whereCondition != null)
                    {
                        docs = GetDocumentQueryResults(
                            pageTypeName,
                            querySettings, query => query.Where(whereCondition));
                    }
                    else
                    {
                        docs = GetDocumentQueryResults(
                            pageTypeName,
                            querySettings);
                    }
                }


                return docs.ToList();
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetDocuments),
                    ex);

                return null;
            }
        }

        public virtual IEnumerable<TreeNode> GetDocuments(
            IEnumerable<Guid> nodeGuids,
            string pageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false,
            string cultureName = null,
            string siteName = null)
        {
            // If the list of Node GUIDs has not been provided, there is no point in querying the DB
            if (DataHelper.DataSourceIsEmpty(nodeGuids))
            {
                return null;
            }

            return GetDocuments(pageTypeName,
                ignorePublishedState,
                columnNames,
                includeAllCoupledColumns,
                cultureName,
                siteName,
                nodeGuids);
        }

        public virtual IEnumerable<TreeNode> GetDocuments(
            string where,
            string pageTypeName,
            IEnumerable<string> cultureNames = null,
            string path = "/%")
        {
            try
            {
                var query = DocumentHelper.GetDocuments(pageTypeName)
                    .Path(path)
                    .Where(where);

                if (cultureNames != null)
                {
                    query.Culture(cultureNames.ToArray());
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetDocuments),
                    ex);

                return null;
            }
        }

        public virtual TResult GetChildDocument<TResult>(
            TreeNode page,
            string nodeAlias,
            string childPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new()
        {
            try
            {
                var querySettings =
                    new DocumentQuerySettings(
                        columnNames,
                        page.DocumentCulture,
                        page.NodeSiteName,
                        _context,
                        DefaultColumnNames)
                    {
                        IgnorePublishedState = ignorePublishedState,
                        IncludeAllCoupledColumns = includeAllCoupledColumns
                    };

                InfoDataSet<TreeNode> childPages;

                if (string.IsNullOrWhiteSpace(childPageTypeName))
                {
                    childPages = GetMultiDocumentQueryResults(
                            querySettings,
                            query => query
                                .WhereEquals(
                                    nameof(TreeNode.NodeParentID),
                                    page.NodeID)
                                .WhereEquals(
                                    nameof(TreeNode.NodeAlias),
                                    nodeAlias)
                                .TopN(1));
                }
                else
                {
                    childPages = GetDocumentQueryResults(
                            childPageTypeName,
                            querySettings,
                            query => query
                                .WhereEquals(
                                    nameof(TreeNode.NodeParentID),
                                    page.NodeID)
                                .WhereEquals(
                                    nameof(TreeNode.NodeAlias),
                                    nodeAlias)
                                .TopN(1));
                }

                return childPages
                    ?.FirstOrDefault()
                    ?.ToPageType<TResult>();
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetChildDocument),
                    ex);

                return null;
            }
        }

        public virtual IEnumerable<TResult> GetChildDocuments<TResult>(
            TreeNode page,
            string childPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new()
        {
            try
            {
                var querySettings =
                    new DocumentQuerySettings(
                        columnNames,
                        page.DocumentCulture,
                        page.NodeSiteName,
                        _context,
                        DefaultColumnNames)
                    {
                        IgnorePublishedState = ignorePublishedState,
                        IncludeAllCoupledColumns = includeAllCoupledColumns
                    };

                InfoDataSet<TreeNode> childPages;

                if (string.IsNullOrWhiteSpace(childPageTypeName))
                {
                    childPages = GetMultiDocumentQueryResults(
                            querySettings,
                            query => query
                                .WhereEquals(
                                    nameof(TreeNode.NodeParentID),
                                    page.NodeID)
                                .OrderBy(nameof(TreeNode.NodeOrder)));
                }
                else
                {
                    childPages = GetDocumentQueryResults(
                            childPageTypeName,
                            querySettings,
                            query => query
                                .WhereEquals(
                                    nameof(TreeNode.NodeParentID),
                                    page.NodeID)
                                .OrderBy(nameof(TreeNode.NodeOrder)));
                }

                return childPages
                    ?.ToList()
                    .Select(cp => cp?.ToPageType<TResult>())
                    .Where(cp => cp != null);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetChildDocuments),
                    ex);

                return null;
            }
        }

        public virtual IEnumerable<TResult> GetChildDocumentsByPath<TResult>(
            TreeNode page,
            string childPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new()
        {
            try
            {
                var querySettings =
                    new DocumentQuerySettings(
                        columnNames,
                        page.DocumentCulture,
                        page.NodeSiteName,
                        _context,
                        DefaultColumnNames)
                    {
                        IgnorePublishedState = ignorePublishedState,
                        IncludeAllCoupledColumns = includeAllCoupledColumns
                    };

                InfoDataSet<TreeNode> childPages;

                if (string.IsNullOrWhiteSpace(childPageTypeName))
                {
                    childPages = GetMultiDocumentQueryResults(
                            querySettings,
                            query => query
                                .Path($"{page.NodeAliasPath}/%")
                                .OrderBy(nameof(TreeNode.NodeOrder)));
                }
                else
                {
                    childPages = GetDocumentQueryResults(
                            childPageTypeName,
                            querySettings,
                            query => query
                                .Path($"{page.NodeAliasPath}/%")
                                .OrderBy(nameof(TreeNode.NodeOrder)));
                }

                return childPages
                    ?.ToList()
                    .Select(cp => cp?.ToPageType<TResult>())
                    .Where(cp => cp != null);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetChildDocumentsByPath),
                    ex);

                return null;
            }
        }

        public virtual IEnumerable<TResult> GetRelatedDocuments<TResult>(
            TreeNode page,
            string relatedFieldName,
            out IList<RelationshipInfo> relationships,
            string relatedPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new()
        {
            relationships = null;

            try
            {
                var relationshipNameInfo = _documentRelationshipRepository
                    .GetRelationshipNameByPageField(
                        page,
                        relatedFieldName);

                if (relationshipNameInfo == null)
                {
                    return null;
                }

                relationships = _documentRelationshipRepository
                    .GetDocumentRelationships(
                        page,
                        relationshipNameInfo);

                var querySettings =
                    new DocumentQuerySettings(
                        columnNames,
                        page.DocumentCulture,
                        page.NodeSiteName,
                        _context,
                        DefaultColumnNames)
                    {
                        IgnorePublishedState = ignorePublishedState,
                        IncludeAllCoupledColumns = includeAllCoupledColumns,
                        // NOTE: We still need to pass this in, even though
                        // we have already queried the relationship infos separately,
                        // so that the list can be ordered properly according to relationship order
                        RelationshipNameId = relationshipNameInfo.RelationshipNameId
                    };

                InfoDataSet<TreeNode> relatedPageSet;

                if (string.IsNullOrWhiteSpace(relatedPageTypeName))
                {
                    relatedPageSet =
                        GetRelatedMultiDocumentQueryResults(
                            page,
                            querySettings);
                }
                else
                {
                    relatedPageSet =
                        GetRelatedDocumentQueryResults(
                            page,
                            relatedPageTypeName,
                            querySettings);
                }

                return relatedPageSet
                    ?.ToList()
                    .Select(rp => rp?.ToPageType<TResult>())
                    .Where(rp => rp != null);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetRelatedDocuments),
                    ex);

                return null;
            }
        }

        public virtual bool InitilizeDocument<TResult>(
            string className,
            UserInfo user,
            out TResult page)
            where TResult : TreeNode, new()
        {
            page = null;

            try
            {
                if (user == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(InitilizeDocument),
                        $"Cannot initialize a page of '{className}' page type without a user context.");

                    return false;
                }

                if (string.IsNullOrWhiteSpace(className))
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(InitilizeDocument),
                        "Page type (class name) was not provided.");

                    return false;
                }

                page = (TResult)TreeNode.New(
                    className,
                    new TreeProvider(user));

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(InitilizeDocument),
                    ex);

                return false;
            }
        }

        public virtual bool CreateDocument(
            TreeNode document,
            TreeNode parent)
        {
            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(CreateDocument),
                        "Page object was not provided.");

                    return false;
                }

                if (parent == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(CreateDocument),
                        $"Page parent object was not provided for page '{document.DocumentName}'.");

                    return false;
                }

                document.Insert(parent);

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(CreateDocument),
                    ex);

                return false;
            }
        }

        public virtual bool CreateNewVersion(
            TreeNode document,
            UserInfo user)
        {
            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(CreateNewVersion),
                        "Page object was not provided.");

                    return false;
                }

                if (user == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(CreateNewVersion),
                        $"Cannot create a new document version for page '{document.NodeAliasPath}' without a user context.");

                    return false;
                }

                var versionManager =
                    VersionManager.GetInstance(
                        new TreeProvider(user));

                versionManager.CreateNewVersion(document);

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(CreateNewVersion),
                    ex);

                return false;
            }
        }

        public virtual bool InsertCultureVersion(
            TreeNode document,
            string culture)
        {
            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(InsertCultureVersion),
                        "Page object was not provided.");

                    return false;
                }

                if (string.IsNullOrWhiteSpace(culture))
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(InsertCultureVersion),
                        $"Culture code for page '{document.NodeAliasPath}' was not provided.");

                    return false;
                }

                document.InsertAsNewCultureVersion(culture);

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(InsertCultureVersion),
                    ex);

                return false;
            }
        }

        public virtual bool DeleteAllCultures(
            TreeNode document)
        {
            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(DeleteAllCultures),
                        "Page object was not provided.");

                    return false;
                }

                document.DeleteAllCultures();

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(DeleteAllCultures),
                    ex);

                return false;
            }
        }

        public virtual bool UpdateDocument(
            TreeNode document)
        {
            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(UpdateDocument),
                        "Page object was not provided.");

                    return false;
                }

                document.Update();

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(UpdateDocument),
                    ex);

                return false;
            }
        }

        public virtual bool IsWorkflowEnabled(
            TreeNode document,
            UserInfo user,
            out bool isWorkflowEnabled)
        {
            isWorkflowEnabled = false;

            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(IsWorkflowEnabled),
                        "Page object was not provided.");

                    return false;
                }

                if (user == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(IsWorkflowEnabled),
                        $"Cannot obtain workflow information for page '{document.NodeAliasPath}' without a user context.");

                    return false;
                }

                var workflowManager =
                    WorkflowManager.GetInstance(new TreeProvider(user));

                var workflow = workflowManager
                    .GetNodeWorkflow(document);

                isWorkflowEnabled = (workflow != null);

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(IsWorkflowEnabled),
                    ex);

                return false;
            }
        }

        public virtual bool ArchiveDocument(
            TreeNode document)
        {
            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(ArchiveDocument),
                        "Page object was not provided.");

                    return false;
                }

                document.Archive();

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(ArchiveDocument),
                    ex);

                return false;
            }
        }

        public virtual bool PublishDocument(
            TreeNode document)
        {
            try
            {
                if (document == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(PublishDocument),
                        "Page object was not provided.");

                    return false;
                }

                document.Publish();

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(PublishDocument),
                    ex);

                return false;
            }
        }

        #endregion

        #region "Helper methods"

        protected InfoDataSet<TreeNode> GetDocumentQueryResults(
            string pageTypeName,
            DocumentQuerySettings querySettings,
            Func<DocumentQuery, DocumentQuery> configureQuery = null
            )
        {
            var docQuery = DocumentHelper
                .GetDocuments(pageTypeName)
                .OnSite(querySettings.SiteName)
                .Culture(querySettings.CultureName)
                .CombineWithDefaultCulture(false)
                .LatestVersion(
                    querySettings.IgnorePublishedState || (CMS.Core.Service.Resolve<IHttpContextAccessor>()?.HttpContext?.Kentico().Preview().Enabled ?? false))
                .Published(
                    !querySettings.IgnorePublishedState && (!CMS.Core.Service.Resolve<IHttpContextAccessor>()?.HttpContext?.Kentico().Preview().Enabled ?? false))
                .OrderBy();

            if (!string.IsNullOrWhiteSpace(querySettings.Path))
            {
                docQuery = docQuery.Path(querySettings.Path);
            }

            if (querySettings.IncludeAllCoupledColumns)
            {
                // At the very least this will contain the default list of global column names
                var requestedGlobalColumnNames = querySettings.ColumnNames
                    .Where(cn => !IsPageTypeSpecificColumnName(cn))
                    .ToList();

                docQuery = docQuery
                    .Columns(requestedGlobalColumnNames)
                    // NOTE: This is admittedly wacky, but I couldn't find any other way of telling Kentico 
                    // to pull up all of the coupled columns from the Page Type table, and "C" is the alias
                    // that Kentico consistently uses for a Page Type table in a typed query
                    .AddColumns("[C].*");
            }
            else
            {
                docQuery = docQuery
                    .Columns(querySettings.ColumnNames);
            }

            var nodeGuids = querySettings.NodeGuids;

            if ((nodeGuids?.Count ?? 0) > 0)
            {
                docQuery = docQuery
                    .TopN(nodeGuids.Count)
                    .WhereIn(
                        nameof(TreeNode.NodeGUID),
                        nodeGuids);
            }
            else
            {
                if (querySettings.Top.HasValue)
                {
                    docQuery = docQuery
                        .TopN(querySettings.Top.Value);
                }
            }

            if (configureQuery != null)
            {
                docQuery = configureQuery(docQuery);
            }

            if (!querySettings.OrderDirection.HasValue)
            {
                querySettings.OrderDirection = OrderDirection.Ascending;
            }


            if (querySettings.OrderByColumns != null)
            {
                docQuery.OrderBy(querySettings.OrderDirection.Value, querySettings.OrderByColumns);
            }

            return docQuery.TypedResult;
        }

        protected InfoDataSet<TreeNode> GetMultiDocumentQueryResults(
            DocumentQuerySettings querySettings,
            Func<MultiDocumentQuery, MultiDocumentQuery> configureQuery = null)
        {
            var docQuery = DocumentHelper
                .GetDocuments()
                .OnSite(querySettings.SiteName)
                .Culture(querySettings.CultureName)
                .CombineWithDefaultCulture(false)
                .LatestVersion(
                    querySettings.IgnorePublishedState || (CMS.Core.Service.Resolve<IHttpContextAccessor>()?.HttpContext?.Kentico().Preview().Enabled ?? false))
                .Published(
                    !querySettings.IgnorePublishedState && (CMS.Core.Service.Resolve<IHttpContextAccessor>()?.HttpContext?.Kentico().Preview().Enabled ?? false));

            if (!string.IsNullOrWhiteSpace(querySettings.Path))
            {
                docQuery = docQuery.Path(querySettings.Path);
            }

            // At the very least this will contain the default list of global column names
            var requestedGlobalColumnNames = querySettings.ColumnNames
                .Where(cn => !IsPageTypeSpecificColumnName(cn))
                .ToList();

            // NOTE: If all coupled columns were requested
            // OR if any of the requested columns is a coupled (i.e. Page Type-specific) column,
            // then we need to include ALL of the coupled columns in the query
            // The reason for this is that if a specific page we are looking for doesn't exist
            // or doesn't match our conditions, Kentico will not actually be able to bind to its Page Type table,
            // which means that it would throw an error if any of the Page Type-specific columns
            // were listed in the SELECT clause at that time.
            // This way, we are relying on Kentico's internal logic to pull the coupled columns when the page exists,
            // and ignore them when it doesn't - all in the name of avoiding an exception...
            if (querySettings.IncludeAllCoupledColumns
                || (querySettings.ColumnNames.Count != requestedGlobalColumnNames.Count))
            {
                docQuery = docQuery
                    .Columns(requestedGlobalColumnNames)
                    // NOTE: Without this, Kentico will not include Page Type-specific columns
                    .WithCoupledColumns(IncludeCoupledDataEnum.Complete);
            }
            else
            {
                // We will come here ONLY when global (non-Page Type-specific) page fields are being requested
                docQuery = docQuery
                    .Columns(querySettings.ColumnNames);
            }

            var nodeGuids = querySettings.NodeGuids;

            if ((nodeGuids?.Count ?? 0) > 0)
            {
                docQuery = docQuery
                    .TopN(nodeGuids.Count)
                    .WhereIn(
                        nameof(TreeNode.NodeGUID),
                        nodeGuids);
            }
            else
            {
                if (querySettings.Top.HasValue)
                {
                    docQuery = docQuery
                        .TopN(querySettings.Top.Value);
                }
            }

            if (configureQuery != null)
            {
                docQuery = configureQuery(docQuery);
            }

            if (!querySettings.OrderDirection.HasValue)
            {
                querySettings.OrderDirection = OrderDirection.Ascending;
            }


            if (querySettings.OrderByColumns != null)
            {
                docQuery.OrderBy(querySettings.OrderDirection.Value, querySettings.OrderByColumns);
            }

            return docQuery.TypedResult;
        }

        protected InfoDataSet<TreeNode> GetRelatedDocumentQueryResults(
            TreeNode page,
            string relatedPageTypeName,
            DocumentQuerySettings querySettings,
            Func<DocumentQuery, DocumentQuery> configureQuery = null)
        {
            return GetDocumentQueryResults(
                    relatedPageTypeName,
                    querySettings,
                    query =>
                    {
                        // NOTE: We still need to pass this in, even though
                        // we have already queried the relationship infos separately,
                        // so that the list can be ordered properly according to relationship order
                        query = query
                            .Source(s => s.InnerJoin<RelationshipInfo>(
                                // NOTE: Without the explicit table alias name,
                                // Kentico will try to reference the incorrect table (COM_SKU)
                                "[V].NodeID",
                                "RightNodeID",
                                new WhereCondition()
                                    .WhereEquals(
                                        "RelationshipNameID",
                                        querySettings.RelationshipNameId)
                                    .WhereEquals("LeftNodeID", page.NodeID)))
                            .OrderBy("RelationshipOrder");

                        if (configureQuery != null)
                        {
                            query = configureQuery(query);
                        }

                        return query;
                    });
        }

        protected InfoDataSet<TreeNode> GetRelatedMultiDocumentQueryResults(
            TreeNode page,
            DocumentQuerySettings querySettings,
            Func<MultiDocumentQuery, MultiDocumentQuery> configureQuery = null)
        {
            return GetMultiDocumentQueryResults(
                    querySettings,
                    query =>
                    {
                        // NOTE: We still need to pass this in, even though
                        // we have already queried the relationship infos separately,
                        // so that the list can be ordered properly according to relationship order
                        query = query
                            .Source(s => s.InnerJoin<RelationshipInfo>(
                                // NOTE: Without the explicit table alias name,
                                // Kentico will try to reference the incorrect table (COM_SKU)
                                "[V].NodeID",
                                "RightNodeID",
                                new WhereCondition()
                                    .WhereEquals(
                                        "RelationshipNameID",
                                        querySettings.RelationshipNameId)
                                    .WhereEquals("LeftNodeID", page.NodeID)))
                            .OrderBy("RelationshipOrder");

                        if (configureQuery != null)
                        {
                            query = configureQuery(query);
                        }

                        return query;
                    });
        }

        protected bool IsPageTypeSpecificColumnName(
            string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                return false;
            }

            return !TreeNodeColumnNames
                .ContainsCaseInsensitive(columnName);
        }

        #endregion

        #region "Helper classes"

        protected class DocumentQuerySettings
        {
            #region "Properties"

            public IList<string> ColumnNames { get; }

            public string CultureName { get; }

            public string SiteName { get; }

            public bool IgnorePublishedState { get; set; }

            /// <summary>
            /// Ignores any Page Type-specific column names in <see cref="ColumnNames"/> value
            /// and requests all Page Type-specific column names instead.
            /// </summary>
            public bool IncludeAllCoupledColumns { get; set; }

            public IList<Guid> NodeGuids { get; set; }

            public int RelationshipNameId { get; set; }

            public string Path { get; set; }

            public int? Top { get; set; }

            public OrderDirection? OrderDirection { get; set; }
            public string[] OrderByColumns { get; set; }

            #endregion

            public DocumentQuerySettings(
                IEnumerable<string> columnNames,
                string cultureName,
                string siteName,
                ContextConfig context,
                IEnumerable<string> defaultColumnNames)
            {
                var columnNameList = columnNames?.ToList() ?? new List<string>();

                columnNameList
                    .AddRange(defaultColumnNames);

                ColumnNames = columnNameList
                    .Distinct()
                    .ToList();

                SiteName =
                    siteName.ReplaceIfEmpty(SiteContext.CurrentSiteName);

                CultureName =
                    cultureName.ReplaceIfEmpty(LocalizationContext.CurrentCulture.CultureCode);
            }
        }

        #endregion
    }
}
