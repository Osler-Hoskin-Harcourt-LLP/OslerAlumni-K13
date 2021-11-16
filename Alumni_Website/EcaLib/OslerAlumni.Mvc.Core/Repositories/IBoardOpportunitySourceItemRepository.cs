using System;
using System.Collections.Generic;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface IBoardOpportunitySourceItemRepository
        : IRepository
    {
        List<CustomTable_BoardOpportunitySourceItem> GetAllBoardOpportunitySourceItems(
            string cultureName = null);

        CustomTable_BoardOpportunitySourceItem GetByGuid(
            Guid guid,
            string cultureName = null);

        CustomTable_BoardOpportunitySourceItem GetByCodeName(
            string codeName,
            string cultureName = null);
    }
}
