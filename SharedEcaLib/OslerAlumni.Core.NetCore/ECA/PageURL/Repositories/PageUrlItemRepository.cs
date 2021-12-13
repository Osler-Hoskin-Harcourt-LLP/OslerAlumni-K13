using System;
using System.Collections.Generic;
using System.Linq;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Internal;
using CMS.SiteProvider;
using ECA.Content.Repositories;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.PageURL.Kentico.Models;

namespace ECA.PageURL.Repositories
{
    /// <inheritdoc />
    public class PageUrlItemRepository
        : IPageUrlItemRepository
    {
        #region "Constants"

        protected const string CustomResultRankColumnName = "CustomResultRank";

        protected static readonly string[] BaseColumnNames =
        {
            nameof(CustomTable_PageURLItem.ItemID),
            nameof(CustomTable_PageURLItem.SiteID),
            nameof(CustomTable_PageURLItem.NodeGUID),
            nameof(CustomTable_PageURLItem.Culture),
            nameof(CustomTable_PageURLItem.URLPath),
            nameof(CustomTable_PageURLItem.IsMainURL),
            nameof(CustomTable_PageURLItem.RedirectType)
        };

        #endregion

        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly ContextConfig _context;
        private readonly IDocumentRepository _documentRepository;

        #endregion

        #region "Properties"

        public virtual string[] DefaultColumnNames
            => BaseColumnNames;

        #endregion

        public PageUrlItemRepository(
            IEventLogRepository eventLogRepository,
            IDocumentRepository documentRepository,
            ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;
            _documentRepository = documentRepository;
            _context = context;
        }

        #region "Methods"

        public bool Delete(
            Guid nodeGuid,
            string cultureName,
            int siteId)
        {
            try
            {
                var where = new WhereCondition()
                    .WhereEquals(
                        nameof(CustomTable_PageURLItem.NodeGUID),
                        nodeGuid)
                    .WhereEquals(
                        nameof(CustomTable_PageURLItem.Culture),
                        cultureName)
                    .WhereEquals(
                        nameof(CustomTable_PageURLItem.SiteID),
                        siteId);

                CustomTableItemProvider.DeleteItems(
                    CustomTable_PageURLItem.CLASS_NAME,
                    where.ToString(true));

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(Delete),
                    ex);

                return false;
            }
        }

        public string GetPageUrl(
            Guid nodeGuid,
            string cultureName,
            string siteName = null,
            IEnumerable<string> columnNames = null)
        {
            try
            {
                string url = string.Empty;
                var page = _documentRepository.GetDocument(nodeGuid, cultureName : cultureName, siteName : siteName);

                if (page != null)
                {
                    url = DocumentURLProvider.GetUrl(page).TrimStart('~');
                }

                return url;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetPageUrl),
                    ex);

                return null;
            }
        }

        public CustomTable_PageURLItem GetPageUrlItem(
            int itemId,
            IEnumerable<string> columnNames = null)
        {
            try
            {
                return
                    GetPageUrlItemsQuery(
                            itemId,
                            columnNames)
                        ?.TopN(1)
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetPageUrlItem),
                    ex);

                return null;
            }
        }

        public IEnumerable<CustomTable_PageURLItem> GetPageUrlItems(
            Guid nodeGuid,
            IList<string> cultureNames,
            bool isMainOnly,
            string siteName = null,
            IEnumerable<string> columnNames = null)
        {
            try
            {
                var query = GetPageUrlItemsQuery(
                    nodeGuid,
                    cultureNames,
                    siteName.ReplaceIfEmpty(SiteContext.CurrentSiteName),
                    columnNames);

                if (isMainOnly)
                {
                    query = query?
                        .WhereTrue(nameof(CustomTable_PageURLItem.IsMainURL));
                }

                return query?.ToList();
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetPageUrlItems),
                    ex);

                return null;
            }
        }

        public IList<CustomTable_PageURLItem> GetPageUrlItems(
            string urlPathPrefix,
            string cultureName,
            bool isMainOnly,
            bool isNonCustomOnly,
            string nodeAliasPath = null,
            int? count = null,
            string siteName = null,
            IEnumerable<string> columnNames = null)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return null;
            }

            try
            {
                // We have to use table name prefix for columns to work around "ambiguous column name" issue,
                // when requesting columns such as "NodeGUID" that have counterparts in CMS_Tree,
                // which we might be joining with later
                columnNames = GetColumnNames(columnNames)
                    .Select(cn =>
                        $"{CustomTable_PageURLItem.CLASS_NAME.Replace('.', '_')}.{cn}")
                    .ToList();

                var query = CustomTableItemProvider
                    .GetItems<CustomTable_PageURLItem>()
                    .Columns(columnNames)
                    .OnSite(siteName.ReplaceIfEmpty(SiteContext.CurrentSiteName))
                    .WhereEquals(
                        nameof(CustomTable_PageURLItem.Culture),
                        cultureName)
                    .WhereStartsWith(
                        nameof(CustomTable_PageURLItem.URLPath),
                        urlPathPrefix);

                if (isMainOnly)
                {
                    query = query
                        .WhereTrue(nameof(CustomTable_PageURLItem.IsMainURL));
                }

                if (isNonCustomOnly)
                {
                    query = query
                        .WhereFalse(nameof(CustomTable_PageURLItem.IsCustomURL));
                }

                if (!string.IsNullOrWhiteSpace(nodeAliasPath))
                {
                    query = query
                        // NOTE: Can't use TreeNode, as that result in join with CMS_Document, instead of CMS_Tree
                        .Source(s => s.InnerJoin<DocumentNodeDataInfo>(
                            nameof(CustomTable_PageURLItem.NodeGUID),
                            nameof(DocumentNodeDataInfo.NodeGUID)))
                        .WhereLike(
                            nameof(DocumentNodeDataInfo.NodeAliasPath),
                            $"{nodeAliasPath}/%");
                }

                if (count.HasValue)
                {
                    query = query
                        .TopN(count.Value);
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetPageUrlItems),
                    ex);

                return null;
            }
        }

        public CustomTable_PageURLItem GetPageUrlItemByUrlPath(
            string urlPath,
            string cultureName,
            Guid? targetNodeGuid = null,
            string siteName = null,
            IEnumerable<string> columnNames = null)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return null;
            }

            try
            {
                // If the url path is missing the leading slash, just prepend it
                if (!(urlPath ?? string.Empty).StartsWith("/"))
                {
                    urlPath = $"/{urlPath}";
                }

                // If the url path has a trailing slash, remove it before DB lookup
                if ((urlPath.Length > 1) && urlPath.EndsWith("/"))
                {
                    urlPath = urlPath.TrimEnd('/');
                }

                columnNames = GetColumnNames(columnNames);

                var query = CustomTableItemProvider
                    .GetItems<CustomTable_PageURLItem>()
                    .Columns(columnNames)
                    .OnSite(siteName.ReplaceIfEmpty(SiteContext.CurrentSiteName))
                    .WhereEquals(
                        nameof(CustomTable_PageURLItem.Culture),
                        cultureName)
                    .WhereEquals(
                        nameof(CustomTable_PageURLItem.URLPath),
                        urlPath)
                    .TopN(1);

                if (targetNodeGuid.HasValue
                    && (targetNodeGuid != Guid.Empty))
                {
                    query
                        .AddColumn(
                            $@"CASE 
                               WHEN {nameof(CustomTable_PageURLItem.NodeGUID)} = '{targetNodeGuid.Value}' 
                               THEN 1 
                               ELSE 0 
                           END AS {CustomResultRankColumnName}")
                        .OrderByDescending(
                            CustomResultRankColumnName);
                }

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetPageUrlItemByUrlPath),
                    ex);

                return null;
            }
        }

        public void Save(
            CustomTable_PageURLItem pageUrlItem)
        {
            try
            {
                CustomTableItemProvider.SetItem(pageUrlItem);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(Save),
                    ex);
            }
        }

        #endregion

        #region "Helper methods"

        protected IEnumerable<string> GetColumnNames(
            IEnumerable<string> columnNames)
        {
            var columnNameList = columnNames?.ToList() ?? new List<string>();

            columnNameList
                .AddRange(DefaultColumnNames);

            return columnNameList.Distinct();
        }

        protected ObjectQuery<CustomTable_PageURLItem> GetPageUrlItemsQuery(
            Guid nodeGuid,
            string cultureName,
            string siteName,
            IEnumerable<string> columnNames)
        {
            return GetPageUrlItemsQuery(
                nodeGuid,
                new List<string> { cultureName },
                siteName,
                columnNames);
        }

        protected ObjectQuery<CustomTable_PageURLItem> GetPageUrlItemsQuery(
            Guid nodeGuid,
            IEnumerable<string> cultureNames,
            string siteName,
            IEnumerable<string> columnNames)
        {
            if (nodeGuid == Guid.Empty)
            {
                return null;
            }

            columnNames = GetColumnNames(columnNames);

            var query = CustomTableItemProvider
                .GetItems<CustomTable_PageURLItem>()
                .Columns(columnNames)
                .OnSite(siteName.ReplaceIfEmpty(SiteContext.CurrentSiteName))
                .WhereEquals(
                    nameof(CustomTable_PageURLItem.NodeGUID),
                    nodeGuid);

            var cultureNameList = cultureNames?.ToList();

            if ((cultureNameList != null) && (cultureNameList.Count > 0))
            {
                query = query
                    .WhereIn(
                        nameof(CustomTable_PageURLItem.Culture),
                        cultureNameList);
            }

            return query;
        }

        protected ObjectQuery<CustomTable_PageURLItem> GetPageUrlItemsQuery(
            int itemId,
            IEnumerable<string> columnNames)
        {
            if (itemId == 0)
            {
                return null;
            }

            columnNames = GetColumnNames(columnNames);

            var query = CustomTableItemProvider
                .GetItems<CustomTable_PageURLItem>()
                .Columns(columnNames)
                .WhereEquals(
                    nameof(CustomTable_PageURLItem.ItemID),
                    itemId);

            return query;
        }

        #endregion
    }
}
