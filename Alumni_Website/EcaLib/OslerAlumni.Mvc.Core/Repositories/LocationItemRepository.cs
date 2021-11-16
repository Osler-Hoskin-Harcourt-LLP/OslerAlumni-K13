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
    public class LocationItemRepository
        : ILocationItemRepository
    {
        #region "Private fields"

        private readonly ICustomTableRepository _customTableRepository;
        private readonly ICacheService _cacheService;

        private readonly ContextConfig _context;

        #endregion

        public LocationItemRepository(
            ICustomTableRepository customTableRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _customTableRepository = customTableRepository;
            _cacheService = cacheService;

            _context = context;

            _customTableRepository.EnabledColumnName =
                nameof(LocationItem.Active);
        }

        #region "Methods"

        public List<LocationItem> GetAllLocationItems(
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
                CacheKey = GlobalConstants.Caching.Events.LocationItemsAll,
                CultureCode = cultureName,
                IsCultureSpecific = true,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the Location custom table items is modified
                CacheDependencies = new List<string>
                {
                    $"customtableitem.{LocationItem.CLASS_NAME}|all"
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var locationItems =
                        _customTableRepository.GetAllCustomTableItems<LocationItem>(
                            GetColumns(),
                            orderBy: nameof(LocationItem.ItemOrder));

                    if (DataHelper.DataSourceIsEmpty(locationItems))
                    {
                        return null;
                    }

                    // Bust the cache whenever any of the resource strings associated with the locations is updated
                    var resStringKeys = locationItems
                        .GetResourceStringCacheKeys(
                            nameof(LocationItem.Location));

                    if (!DataHelper.DataSourceIsEmpty(resStringKeys))
                    {
                        cp.CacheDependencies.AddRange(
                            resStringKeys);
                    }

                    locationItems.ForEach(locationItem =>
                        locationItem.Location = ResHelper.LocalizeString(locationItem.Location, cultureName));

                    return locationItems;
                },
                cacheParameters);

            return result;
        }
        
        public LocationItem GetByGuid(
            Guid guid,
            string cultureName = null)
        {
            return GetAllLocationItems(cultureName)?
                .FirstOrDefault(li => li.ItemGUID == guid);
        }

        #endregion

        #region "Helper methods"

        private List<string> GetColumns()
        {
            return new List<string>
            {
                nameof(LocationItem.ItemGUID),
                nameof(LocationItem.Location)
            };
        }

        #endregion
    }
}
