using System.Collections.Generic;
using CMS.DocumentEngine;
using ECA.Core.Services;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IGenericPageService
        : IService
    {
        PageType_Page GetSubPageByAlias(
            TreeNode page,
            string nodeAlias);

        IList<PageType_Page> GetSubPages(
            TreeNode page);
    }
}
