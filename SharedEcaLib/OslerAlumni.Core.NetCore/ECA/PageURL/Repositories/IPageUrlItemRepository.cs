using System;
using System.Collections.Generic;
using ECA.Core.Repositories;
using ECA.PageURL.Kentico.Models;

namespace ECA.PageURL.Repositories
{
    /// <remarks>
    /// This class and its implementations do NOT use caching.
    /// Caching should be applied on the results of corrensponding methods,
    /// depending on the individual usage context.
    /// </remarks>
    public interface IPageUrlItemRepository
        : IRepository
    {
        string[] DefaultColumnNames { get; }

        bool Delete(
            Guid nodeGuid,
            string cultureName,
            int siteId);

        string GetPageUrl(
            Guid nodeGuid,
            string cultureName,
            string siteName = null,
            IEnumerable<string> columnNames = null);

        CustomTable_PageURLItem GetPageUrlItem(
            int itemId,
            IEnumerable<string> columnNames = null);

        IEnumerable<CustomTable_PageURLItem> GetPageUrlItems(
            Guid nodeGuid,
            IList<string> cultureNames,
            bool isMainOnly,
            string siteName = null,
            IEnumerable<string> columnNames = null);

        IList<CustomTable_PageURLItem> GetPageUrlItems(
            string urlPathPrefix,
            string cultureName,
            bool isMainOnly,
            bool isNonCustomOnly,
            string nodeAliasPath = null,
            int? count = null,
            string siteName = null,
            IEnumerable<string> columnNames = null);

        CustomTable_PageURLItem GetPageUrlItemByUrlPath(
            string urlPath,
            string cultureName,
            Guid? targetNodeGuid = null,
            string siteName = null,
            IEnumerable<string> columnNames = null);

        void Save(
            CustomTable_PageURLItem pageUrlItem);
    }
}
