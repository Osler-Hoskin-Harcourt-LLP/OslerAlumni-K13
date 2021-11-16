using System;
using System.Collections.Generic;
using System.Linq;
using CMS.CustomTables;
using CMS.DataEngine;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.PageURL.Kentico.Models;

namespace ECA.Admin.PageURL.Repositories
{
    public class PathChangeItemRepository
        : IPathChangeItemRepository
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly ContextConfig _context;

        #endregion

        public PathChangeItemRepository(
            IEventLogRepository eventLogRepository,
            ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;

            _context = context;
        }
        
        #region "Methods"

        public bool Delete(
            IList<CustomTable_PathChangeItem> items)
        {
            if ((items == null) || (items.Count < 1))
            {
                return true;
            }

            try
            {
                var where = new WhereCondition()
                    .WhereIn(
                        nameof(CustomTable_PathChangeItem.ItemGUID),
                        items.Select(item => item.ItemGUID).ToList());

                CustomTableItemProvider.DeleteItems(
                    CustomTable_PathChangeItem.CLASS_NAME,
                    where.ToString(true));

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(Delete),
                    ex);

                return false;
            }
        }

        public IList<CustomTable_PathChangeItem> GetPathChangeItems(
            int count,
            string siteName = null)
        {
            return CustomTableItemProvider
                .GetItems<CustomTable_PathChangeItem>()
                .OnSite(siteName.ReplaceIfEmpty(_context.Site?.SiteName))
                .TopN(count)
                .OrderByAscending(nameof(CustomTable_PageURLItem.ItemID))
                .ToList();
        }

        public void Save(
            CustomTable_PathChangeItem pathChangeItem)
        {
            try
            {
                CustomTableItemProvider.SetItem(pathChangeItem);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(Save),
                    ex);
            }
        }

        #endregion
    }
}
