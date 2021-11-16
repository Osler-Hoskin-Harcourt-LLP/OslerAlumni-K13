using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using ECA.Caching.Extensions;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Extensions;
using ECA.Core.Models;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public class JobCategoryItemRepository
        : IJobCategoryItemRepository
    {
        #region "Private fields"

        private readonly ICustomTableRepository _customTableRepository;
        private readonly ICacheService _cacheService;

        private readonly ContextConfig _context;

        #endregion

        public JobCategoryItemRepository(
            ICustomTableRepository customTableRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _customTableRepository = customTableRepository;
            _cacheService = cacheService;

            _context = context;
        }

        #region "Methods"
        
        public List<CustomTable_JobCategoryItem> GetAllJobCategoryItems(
            string cultureName = null)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                cultureName = _context.CultureName;
            }

            string cultureKey;

            if (!_context.AllowedCultureCodes
                    .TryGetKeyByOrdinalValue(
                        cultureName,
                        out cultureKey))
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Jobs.JobCategoryItemsAll,
                CultureCode = cultureName,
                IsCultureSpecific = true,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the Job Category custom table items is modified
                CacheDependencies = new List<string>
                {
                    $"customtableitem.{CustomTable_JobCategoryItem.CLASS_NAME}|all"
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var jobCategoryItems = _customTableRepository
                        .GetAllCustomTableItems<CustomTable_JobCategoryItem>(
                            GetColumns(),
                            orderBy: nameof(CustomTable_JobCategoryItem.ItemOrder));

                    if (DataHelper.DataSourceIsEmpty(jobCategoryItems))
                    {
                        return null;
                    }

                    // Bust the cache whenever any of the resource strings associated with the job categories is updated
                    var resStringKeys = jobCategoryItems
                        .GetResourceStringCacheKeys(
                            nameof(CustomTable_JobCategoryItem.DisplayName));

                    if (!DataHelper.DataSourceIsEmpty(resStringKeys))
                    {
                        cp.CacheDependencies.AddRange(
                            resStringKeys);
                    }

                    jobCategoryItems.ForEach(jobCategoryItem =>
                        jobCategoryItem.DisplayName = ResHelper.LocalizeString(jobCategoryItem.DisplayName, cultureName));

                    return jobCategoryItems;
                },
                cacheParameters);

            return result;
        }

        public CustomTable_JobCategoryItem GetByGuid(
            Guid guid,
            string cultureName = null)
        {
            return GetAllJobCategoryItems(cultureName)?
                .FirstOrDefault(jci => jci.ItemGUID == guid);
        }

        public CustomTable_JobCategoryItem GetByCodeName(
            string codeName,
            string cultureName = null)
        {
            return GetAllJobCategoryItems(cultureName)?
                .FirstOrDefault(jci => string.Equals(jci.CodeName, codeName, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region "Helper methods"

        private List<string> GetColumns()
        {
            return new List<string>
            {
                nameof(CustomTable_JobCategoryItem.ItemGUID),
                nameof(CustomTable_JobCategoryItem.CodeName),
                nameof(CustomTable_JobCategoryItem.DisplayName),
            };
        }

        #endregion
    }
}
