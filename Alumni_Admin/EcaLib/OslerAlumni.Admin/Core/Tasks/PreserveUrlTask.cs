using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.CustomTables;
using CMS.DocumentEngine;
using CMS.Scheduler;
using ECA.PageURL.Kentico.Models;

namespace OslerAlumni.Admin.Core.Tasks
{
    public class PreserveUrlTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            var urlItems = CustomTableItemProvider.GetItems<CustomTable_PageURLItem>();

            foreach (CustomTable_PageURLItem item in urlItems)
            {
                var page = DocumentHelper.GetDocuments()
                    .OnSite(item.SiteID)
                    .Culture(item.Culture)
                    .WhereEquals("NodeGUID", item.NodeGUID)
                    .FirstOrDefault();

                if (page != null)
                {
                    PageFormerUrlPathInfo.Provider.Set(new PageFormerUrlPathInfo()
                    {
                        PageFormerUrlPathCulture = page.DocumentCulture,
                        PageFormerUrlPathNodeID = page.NodeID,
                        PageFormerUrlPathSiteID = page.NodeSiteID,
                        PageFormerUrlPathUrlPath = page.DocumentCulture.Split('-')[0] + item.URLPath
                    });
                }
            }

            return "Done";
        }
    }
}
