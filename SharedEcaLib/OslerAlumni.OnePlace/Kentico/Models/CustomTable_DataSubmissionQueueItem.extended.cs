using System;
using System.Linq;
using CMS.Helpers;

namespace OslerAlumni.OnePlace.Kentico.Models
{
    public partial class CustomTable_DataSubmissionQueueItem
    {
        #region "Custom properties"

        public int[] DependsOnItemIdsArray
        {
            get
            {
                return string.IsNullOrWhiteSpace(DependsOnItemIds)
                    ? null
                    : DependsOnItemIds
                        .Split(
                            new[] { ";" },
                            StringSplitOptions.RemoveEmptyEntries)
                        .Select(
                            id => ValidationHelper.GetInteger(id, 0))
                        .Where(
                            id => id > 0)
                        .ToArray();
            }
            set
            {
                DependsOnItemIds = (value == null)
                    ? null
                    : string.Join(";", value);
            }
        }

        public CustomTable_DataSubmissionQueueItem[] DependsOnItemsArray { get; set; }

        #endregion
    }
}