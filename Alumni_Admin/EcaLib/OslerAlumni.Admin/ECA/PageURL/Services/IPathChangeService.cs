using System.Collections.Generic;
using CMS.DocumentEngine;
using ECA.Core.Services;
using ECA.PageURL.Kentico.Models;

namespace ECA.Admin.PageURL.Services
{
    public interface IPathChangeService
        : IService
    {
        bool IsPathChangeReverted(
            CustomTable_PathChangeItem pathChange,
            out TreeNode pathChangePage);

        void LogPathChange(
            TreeNode page);

        bool TryGetPagesInPath(
            CustomTable_PathChangeItem pathChange,
            TreeNode pathChangePage,
            int count,
            out IList<TreeNode> pages);
    }
}
