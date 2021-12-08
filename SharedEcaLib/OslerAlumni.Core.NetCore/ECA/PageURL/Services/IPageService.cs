using System;
using System.Collections.Generic;
using CMS.DocumentEngine;
using ECA.Core.Services;
using ECA.PageURL.Definitions;
using ECA.PageURL.Kentico.Models;

namespace ECA.PageURL.Services
{
    public interface IPageService
        : IService
    {
        bool TryGetPage(
            Guid nodeGuid,
            string cultureName,
            string siteName,
            out TreeNode page,
            string pageTypeName = null,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false);

        bool TryGetPageCultureUrls(
            TreeNode page,
            bool excludeCurrent,
            bool ignorePublishedState,
            out Dictionary<string, string> cultureUrls,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false);

        bool TryGetStandalonePage(
            StandalonePageType standalonePageType,
            string cultureName,
            string siteName,
            out TreeNode page,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false);

        bool TryGetUrlItemPage(
            CustomTable_PageURLItem urlItem,
            out TreeNode page,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false);
    }
}
