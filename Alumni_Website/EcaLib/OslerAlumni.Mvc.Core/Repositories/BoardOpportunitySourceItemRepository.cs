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
    public class BoardOpportunitySourceItemRepository
        : IBoardOpportunitySourceItemRepository
    {
        #region "Private fields"

        private readonly ICustomTableRepository _customTableRepository;
        private readonly ICacheService _cacheService;

        private readonly ContextConfig _context;

        #endregion

        public BoardOpportunitySourceItemRepository(
            ICustomTableRepository customTableRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _customTableRepository = customTableRepository;
            _cacheService = cacheService;

            _context = context;
        }

        #region "Methods"
        public List<CustomTable_BoardOpportunitySourceItem> GetAllBoardOpportunitySourceItems(string cultureName = null)
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
                CacheKey = GlobalConstants.Caching.BoardOpportunities.BoardOpportunitySourceItemsAll,
                CultureCode = cultureName,
                IsCultureSpecific = true,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the custom table items is modified
                CacheDependencies = new List<string>
                {
                    $"customtableitem.{CustomTable_BoardOpportunitySourceItem.CLASS_NAME}|all"
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var boardOpportunitySourceItems = _customTableRepository
                        .GetAllCustomTableItems<CustomTable_BoardOpportunitySourceItem>(
                            GetColumns(),
                            orderBy: nameof(CustomTable_BoardOpportunitySourceItem.ItemOrder));

                    if (DataHelper.DataSourceIsEmpty(boardOpportunitySourceItems))
                    {
                        return null;
                    }

                    // Bust the cache whenever any of the resource strings associated with the job categories is updated
                    var resStringKeys = boardOpportunitySourceItems
                        .GetResourceStringCacheKeys(
                            nameof(CustomTable_BoardOpportunitySourceItem.CompanyName));

                    if (!DataHelper.DataSourceIsEmpty(resStringKeys))
                    {
                        cp.CacheDependencies.AddRange(
                            resStringKeys);
                    }

                    boardOpportunitySourceItems.ForEach(boardOpportunityTypeItem =>
                        boardOpportunityTypeItem.CompanyName = ResHelper.LocalizeString(boardOpportunityTypeItem.CompanyName, cultureName));

                    return boardOpportunitySourceItems;
                },
                cacheParameters);

            return result;
        }

        public CustomTable_BoardOpportunitySourceItem GetByGuid(Guid guid, string cultureName = null)
        {
            return GetAllBoardOpportunitySourceItems(cultureName)?
                .FirstOrDefault(jci => jci.ItemGUID == guid);
        }

        public CustomTable_BoardOpportunitySourceItem GetByCodeName(string codeName, string cultureName = null)
        {
            return GetAllBoardOpportunitySourceItems(cultureName)?
                .FirstOrDefault(jci => string.Equals(jci.CodeName, codeName, StringComparison.OrdinalIgnoreCase));
        }


        #endregion

        #region "Helper methods"

        private List<string> GetColumns()
        {
            return new List<string>
            {
                nameof(CustomTable_BoardOpportunitySourceItem.ItemGUID),
                nameof(CustomTable_BoardOpportunitySourceItem.CodeName),
                nameof(CustomTable_BoardOpportunitySourceItem.CompanyName),
                nameof(CustomTable_BoardOpportunitySourceItem.LogoEn),
                nameof(CustomTable_BoardOpportunitySourceItem.LogoFr),
                nameof(CustomTable_BoardOpportunitySourceItem.LogoAltText)
            };
        }

        #endregion

       
    }
}
