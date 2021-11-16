using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Routing.Internal;
using CMS.Scheduler;

namespace OslerAlumni.Admin.Core.Tasks
{
    public class UpdateFrenchUrl : ITask
    {
        public string Execute(TaskInfo task)
        {
            var frPages = DocumentHelper.GetDocuments()
                .Culture("fr-CA")
                .OrderBy("NodeLevel");

            foreach (var p in frPages)
            {
                string currentUrl = DocumentURLProvider.GetUrl(p);

                if (!string.IsNullOrEmpty(currentUrl))
                {
                    PageUrlPathSlugUpdater pageUrlPathSlugUpdater = new PageUrlPathSlugUpdater(p, p.DocumentCulture);

                    IEnumerable<CollisionData> collidingPaths;
                    string newSlug = p.DocumentName;
                    int i = 1;
                    do
                    {
                        pageUrlPathSlugUpdater.TryUpdate(newSlug, out collidingPaths);
                        if (collidingPaths.Any())
                        {
                            newSlug += $"-{++i}";
                        }
                    }
                    while (collidingPaths.Count() != 0);
                }
            }

            return "Done";
        }
    }
}
