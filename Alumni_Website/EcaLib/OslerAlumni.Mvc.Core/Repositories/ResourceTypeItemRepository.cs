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
    public class ResourceTypeItemRepository
        : IResourceTypeItemRepository
    {
        #region "Private fields"

        private readonly ICustomTableRepository _customTableRepository;
        private readonly ICacheService _cacheService;

        private readonly ContextConfig _context;

        #endregion

        public ResourceTypeItemRepository(
            ICustomTableRepository customTableRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _customTableRepository = customTableRepository;
            _cacheService = cacheService;

            _context = context;
        }

        #region "Methods"

        public List<CustomTable_ResourceTypeItem> GetAllResourceTypeItems(
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
                CacheKey = GlobalConstants.Caching.Resources.ResourceTypeItemsAll,
                CultureCode = cultureName,
                IsCultureSpecific = true,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the Resource Type custom table items is modified
                CacheDependencies = new List<string>
                {
                    $"customtableitem.{CustomTable_ResourceTypeItem.CLASS_NAME}|all"
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var resourceTypeItems = _customTableRepository
                        .GetAllCustomTableItems<CustomTable_ResourceTypeItem>(
                            GetColumns(),
                            orderBy: nameof(CustomTable_ResourceTypeItem.ItemOrder));

                    if (DataHelper.DataSourceIsEmpty(resourceTypeItems))
                    {
                        return null;
                    }

                    // Bust the cache whenever any of the resource strings associated with the resource types is updated
                    var resStringKeys = resourceTypeItems
                        .GetResourceStringCacheKeys(
                            nameof(CustomTable_ResourceTypeItem.DisplayName));

                    if (!DataHelper.DataSourceIsEmpty(resStringKeys))
                    {
                        cp.CacheDependencies.AddRange(
                            resStringKeys);
                    }

                    resourceTypeItems.ForEach(resourceTypeItem =>
                        resourceTypeItem.DisplayName = ResHelper.LocalizeString(resourceTypeItem.DisplayName, cultureName));

                    return resourceTypeItems;
                },
                cacheParameters);

            return result;
        }

        public CustomTable_ResourceTypeItem GetByCodeName(
            string codeName,
            string cultureName = null)
        {
            return GetAllResourceTypeItems(cultureName)?
                .FirstOrDefault(rti =>
                    string.Equals(rti.CodeName, codeName, StringComparison.OrdinalIgnoreCase));
        }

        public List<CustomTable_ResourceTypeItem> GetByCodeNames(IEnumerable<string> codeNames, string cultureName = null)
        {
            return codeNames.Select(codeName => GetByCodeName(codeName, cultureName)).ToList();
        }

        #endregion

        #region "Helper methods"

        private List<string> GetColumns()
        {
            return new List<string>
            {
                nameof(CustomTable_ResourceTypeItem.DisplayName),
                nameof(CustomTable_ResourceTypeItem.CodeName)
            };
        }

        #endregion
    }
}
