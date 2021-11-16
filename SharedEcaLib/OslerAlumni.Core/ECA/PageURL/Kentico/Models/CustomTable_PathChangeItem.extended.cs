using CMS.DataEngine;

namespace ECA.PageURL.Kentico.Models
{
    public partial class CustomTable_PathChangeItem
    {
        #region "Custom properties"

        public string SiteName
            => ObjectSiteName;

        #endregion

        #region "Custom methods"

        protected override GeneralizedInfo GetGeneralizedInfo()
        {
            var generalizedInfo = base.GetGeneralizedInfo();

            // NOTE: Without this site name will not be populated
            generalizedInfo.TypeInfo.SiteIDColumn = nameof(SiteID);

            return generalizedInfo;
        }

        #endregion
    }
}
