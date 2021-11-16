using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IJobsService
        : IService
    {
        List<PageType_Job> GetLatestJobs(int top);
    }
}
