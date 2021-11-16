using System.Linq;
using CMS;
using CMS.DocumentEngine;
using ECA.Admin.Core.Modules;
using ECA.Admin.PageURL.Modules;
using ECA.Core.Definitions;
using ECA.PageURL.Services;

[assembly: RegisterModule(typeof(MainPageUrlModule))]
namespace ECA.Admin.PageURL.Modules
{
    public class MainPageUrlModule
        : BaseModule
    {
        #region "Properties"

        public IPageUrlService PageUrlService { get; set; }

        #endregion

        public MainPageUrlModule()
            : base($"{ECAGlobalConstants.Modules.PageURL}.{nameof(MainPageUrlModule)}")
        { }

        protected override void OnInit()
        {
            base.OnInit();

            DocumentEvents.Insert.After += Document_Insert_After;
            DocumentEvents.InsertNewCulture.After += Document_Insert_After;
            DocumentEvents.Update.After += Document_Update_After;
            DocumentEvents.Move.After += Document_Move_After;

            WorkflowEvents.SaveVersion.After += Document_SaveVersion_After;
            WorkflowEvents.Publish.After += Document_Publish_After;

            DocumentEvents.Delete.After += Document_Delete_After;
        }

        #region "Events"

        protected void Document_Insert_After(object sender, DocumentEventArgs e)
        {
            SetOrDeletePageUrls(e.Node);
        }

        protected void Document_Update_After(object sender, DocumentEventArgs e)
        {
            var page = e.Node;

            // This ensures that the page is NOT under workflow.
            // For pages that ARE under a workflow, we should handle main URL creation on Publish event.
            if (!page.IsLastVersion)
            {
                return;
            }

            SetOrDeletePageUrls(page);
        }

        protected void Document_Move_After(object sender, DocumentEventArgs e)
        {
            SetOrDeletePageUrls(e.Node);
        }

        protected void Document_Publish_After(object sender, WorkflowEventArgs e)
        {
            SetOrDeletePageUrls(e.PublishedDocument);
        }

        private void Document_SaveVersion_After(object sender, WorkflowEventArgs e)
        {
            var page = e.Document;


            var changedColumns = page.ChangedColumns();

            if (!(changedColumns?.Any() ?? false))
            {
                return;
            }


            SetOrDeletePageUrls(page);
        }

        protected void Document_Delete_After(object sender, DocumentEventArgs e)
        {
            var page = e.Node;

            PageUrlService.DeleteUrls(page);
        }

        #endregion

        #region "Methods"

        protected void SetOrDeletePageUrls(
            TreeNode page)
        {
            if (PageUrlService.IsNavigable(page))
            {
                PageUrlService.TrySetPageMainUrl(page);
            }
            else
            {
                PageUrlService.DeleteUrls(page);
            }
        }

        #endregion
    }
}
