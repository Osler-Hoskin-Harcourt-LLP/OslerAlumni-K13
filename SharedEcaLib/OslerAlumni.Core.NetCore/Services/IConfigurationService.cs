using System;
using System.Collections.Specialized;
using ECA.Core.Models;
using ECA.Core.Services;

namespace OslerAlumni.Core.Services
{
    public interface IConfigurationService 
        : IService
    {
        object GetWebConfigSetting(
            Type type,
            string key,
            string sectionName = null);

        T GetWebConfigSetting<T>(
            string key, 
            string sectionName = null);
        
        T GetConfig<T>() 
            where T : class, IConfig, new();

        NameValueCollection GetWebConfigSection(string sectionName);

    }
}
