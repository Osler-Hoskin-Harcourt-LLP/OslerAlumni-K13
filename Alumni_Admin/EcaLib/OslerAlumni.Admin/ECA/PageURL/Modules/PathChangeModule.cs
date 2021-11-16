using System.Collections.Generic;
using CMS;
using CMS.DocumentEngine;
using ECA.Admin.Core.Modules;
using ECA.Admin.PageURL.Modules;
using ECA.Admin.PageURL.Services;
using ECA.Content.Repositories;
using ECA.Core.Definitions;

[assembly: RegisterModule(typeof(PathChangeModule))]
namespace ECA.Admin.PageURL.Modules
{
    public class PathChangeModule
        : BaseModule
    {
        #region "Properties"

        public IDocumentRepository DocumentRepository { get; set; }

        public IPathChangeService PathChangeService { get; set; }

        #endregion

        public PathChangeModule()
            : base($"{ECAGlobalConstants.Modules.PageURL}.{nameof(PathChangeModule)}")
        { }

        protected override void OnInit()
        {
            base.OnInit();

            DocumentEvents.Update.Before += Document_Update_Before;
            DocumentEvents.Move.Before += Document_Move_Before;

            WorkflowEvents.Publish.Before += Publish_Before;
        }

        #region "Events"

        protected void Document_Update_Before(object sender, DocumentEventArgs e)
        {
            var page = e.Node;

            // This ensures that the page is NOT under workflow.
            // For pages that ARE under a workflow, we should handle this logic on Publish event.
            if (!page.IsLastVersion)
            {
                return;
            }

            LogPathChange(page);
        }

        protected void Document_Move_Before(object sender, DocumentEventArgs e)
        {
            var page = e.Node;

            // A page move doesn't return any changed columns so using a flag
            LogPathChange(
                page,
                isPageMove: true);
        }

        protected void Publish_Before(object sender, WorkflowEventArgs e)
        {
            var page = e.Document;

            // We want the details of the original state of the page (original page name),
            // so that we know what to look for in existing URLs before updating them to the new path,
            // which is why we need to request it from DB
            var publishedPage = DocumentRepository.GetDocument(
                page.NodeGUID,
                page.NodeClassName,
                ignorePublishedState: false,
                cultureName: page.DocumentCulture,
                siteName: page.NodeSiteName);

            LogPathChange(
                publishedPage,
                page.ChangedColumns());
        }

        #endregion

        #region "Methods"

        protected void LogPathChange(
            TreeNode page,
            IList<string> changedColumns = null,
            bool isPageMove = false)
        {
            if (page == null || !page.NodeHasChildren)
            {
                return;
            }

            if (!isPageMove)
            {
                changedColumns = changedColumns ?? page.ChangedColumns();

                if (changedColumns == null 
                    || !changedColumns.Contains(nameof(TreeNode.DocumentName)))
                {
                    return;
                }
            }

            PathChangeService.LogPathChange(page);
        }

        #endregion
    }
}
