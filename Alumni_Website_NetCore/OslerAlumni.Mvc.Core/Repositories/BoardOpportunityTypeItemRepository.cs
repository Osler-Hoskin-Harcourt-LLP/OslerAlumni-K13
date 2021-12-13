using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.Localization;
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
    public class BoardOpportunityTypeItemRepository
        : IBoardOpportunityTypeItemRepository
    {
        #region "Private fields"

        private readonly ICustomTableRepository _customTableRepository;
        private readonly ICacheService _cacheService;

        private readonly ContextConfig _context;

        #endregion

        public BoardOpportunityTypeItemRepository(
            ICustomTableRepository customTableRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _customTableRepository = customTableRepository;
            _cacheService = cacheService;

            _context = context;
        }

        #region "Methods"
        
        public List<CustomTable_BoardOpportunityTypeItem> GetAllBoardOpportunityTypeItems(
            string cultureName = null)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                cultureName = LocalizationContext.CurrentCulture.CultureCode;
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
                CacheKey = GlobalConstants.Caching.BoardOpportunities.BoardOpportunityItemsAll,
                CultureCode = cultureName,
                IsCultureSpecific = true,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the Job Category custom table items is modified
                CacheDependencies = new List<string>
                {
                    $"customtableitem.{CustomTable_BoardOpportunityTypeItem.CLASS_NAME}|all"
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var boardOpportunityTypeItems = _customTableRepository
                        .GetAllCustomTableItems<CustomTable_BoardOpportunityTypeItem>(
                            GetColumns(),
                            orderBy: nameof(CustomTable_BoardOpportunityTypeItem.ItemOrder));

                    if (DataHelper.DataSourceIsEmpty(boardOpportunityTypeItems))
                    {
                        return null;
                    }

                    // Bust the cache whenever any of the resource strings associated with the job categories is updated
                    var resStringKeys = boardOpportunityTypeItems
                        .GetResourceStringCacheKeys(
                            nameof(CustomTable_BoardOpportunityTypeItem.DisplayName));

                    if (!DataHelper.DataSourceIsEmpty(resStringKeys))
                    {
                        cp.CacheDependencies.AddRange(
                            resStringKeys);
                    }

                    boardOpportunityTypeItems.ForEach(boardOpportunityTypeItem =>
                        boardOpportunityTypeItem.DisplayName = ResHelper.LocalizeString(boardOpportunityTypeItem.DisplayName, cultureName));

                    return boardOpportunityTypeItems;
                },
                cacheParameters);

            return result;
        }

        public CustomTable_BoardOpportunityTypeItem GetByGuid(
            Guid guid,
            string cultureName = null)
        {
            return GetAllBoardOpportunityTypeItems(cultureName)?
                .FirstOrDefault(jci => jci.ItemGUID == guid);
        }

        public CustomTable_BoardOpportunityTypeItem GetByCodeName(
            string codeName,
            string cultureName = null)
        {
            return GetAllBoardOpportunityTypeItems(cultureName)?
                .FirstOrDefault(jci => string.Equals(jci.CodeName, codeName, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region "Helper methods"

        private List<string> GetColumns()
        {
            return new List<string>
            {
                nameof(CustomTable_BoardOpportunityTypeItem.ItemGUID),
                nameof(CustomTable_BoardOpportunityTypeItem.CodeName),
                nameof(CustomTable_BoardOpportunityTypeItem.DisplayName),
            };
        }

        #endregion
    }
}
