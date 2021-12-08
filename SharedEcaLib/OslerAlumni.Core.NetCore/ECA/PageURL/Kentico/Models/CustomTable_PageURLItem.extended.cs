using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace ECA.PageURL.Kentico.Models
{
    public partial class CustomTable_PageURLItem
    {
        #region "Custom properties"

        //TODO##
        //public AliasActionModeEnum RedirectTypeEnum
        //{
        //    get
        //    {
        //        return RedirectType.ToEnum<AliasActionModeEnum>();
        //    }
        //    set
        //    {
        //        RedirectType = value.ToStringRepresentation();
        //    }
        //}


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
