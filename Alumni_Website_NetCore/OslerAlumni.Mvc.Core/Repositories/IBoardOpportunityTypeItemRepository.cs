using System;
using System.Collections.Generic;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface IBoardOpportunityTypeItemRepository
        : IRepository
    {
        List<CustomTable_BoardOpportunityTypeItem> GetAllBoardOpportunityTypeItems(
            string cultureName = null);

        CustomTable_BoardOpportunityTypeItem GetByGuid(
            Guid guid,
            string cultureName = null);

        CustomTable_BoardOpportunityTypeItem GetByCodeName(
            string codeName,
            string cultureName = null);
    }
}
