using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using OslerAlumni.Core.Kentico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OslerAlumni.Mvc.Core.Authorization
{
    public class PublicPageHandler : AuthorizationHandler<PublicPageRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, PublicPageRequirement requirement)
        {
            var pageDataRetriever = CMS.Core.Service.Resolve<IPageDataContextRetriever>();
            var page = pageDataRetriever.Retrieve<TreeNode>().Page;

            if (page != null)
            {
                if (PageIsPublic(page))
                {
                    context.Succeed(requirement);
                }
            }


            return Task.CompletedTask;
        }

        private bool PageIsPublic(TreeNode page)
        {
            return page.GetBooleanValue("IsPublic", false);
        }
    }
}
