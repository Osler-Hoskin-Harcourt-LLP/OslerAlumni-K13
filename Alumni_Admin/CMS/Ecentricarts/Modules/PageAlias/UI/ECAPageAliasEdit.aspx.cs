using System;
using Autofac;
using CMS.Helpers;
using CMS.Localization;
using CMS.UIControls;
using ECA.Admin.Core.Extensions;
using ECA.PageURL.Kentico.Models;
using ECA.PageURL.Repositories;
using ECA.PageURL.Services;

namespace CMSApp.Ecentricarts.Modules.PageAlias
{
    public partial class ECAPageAliasEdit : CMSPropertiesPage
    {
        #region "Constants"

        private const string ListPageUrl = "~/Ecentricarts/Modules/PageAlias/UI/ECAPageAliasList.aspx";

        #endregion

        #region "Private fields"

        private readonly ILifetimeScope _scope;

        #endregion

        #region "Properties"

        public IPageUrlItemRepository PageUrlItemRepository { get; set; }

        public IPageUrlService PageUrlService { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ECAPageAliasList"/> class.
        /// </summary>
        public ECAPageAliasEdit()
        {
            _scope = this.InjectDependencies();
        }

        public override void Dispose()
        {
            // Dispose of managed resources

            // NOTE: It is unclear if this is still the case, but there are posts out there that claim 
            // that the Autofac lifetime scope in which you resolve components (dependencies) has to stay alive
            // while you are still using those resolved instances. Which is why we are
            // taking responsibility over the scope disposal in this way.
            _scope?.Dispose();

            base.Dispose();
        }

        #region "Page events"

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var itemId = ValidationHelper.GetInteger(Request.QueryString["aliasid"], 0);

                if (itemId != 0)
                {
                    var item = PageUrlItemRepository.GetPageUrlItem(itemId);

                    if (item != null)
                    {
                        txtUrlPath.Text = item.GetStringValue("URLPath", string.Empty);
                        chbIsMainURL.Checked = item.GetBooleanValue("IsMainURL", false);
                        chbIsCanonicalURL.Checked = item.GetBooleanValue("IsCanonicalURL", false);
                        ddlRedirectType.SelectedValue = item.GetStringValue("RedirectType", string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var itemId = ValidationHelper.GetInteger(Request.QueryString["aliasid"], 0);

            var nodeId = ValidationHelper.GetInteger(Request.QueryString["nodeid"], 0);
            var culture = ValidationHelper.GetString(Request.QueryString["culture"],
                LocalizationContext.CurrentCulture.CultureCode);

            var urlItem = new CustomTable_PageURLItem
            {
                ItemID = itemId,
                Culture = culture,
                URLPath = txtUrlPath.Text.Trim(),
                IsMainURL = chbIsMainURL.Checked,
                IsCustomURL = true,
                IsCanonicalURL = chbIsCanonicalURL.Checked,
                RedirectType = ddlRedirectType.SelectedValue
            };

            if (PageUrlService.TrySetPageUrl(
                    Node,
                    urlItem))
            {
                Response.Redirect($"{ListPageUrl}?nodeid={nodeId}&culture={culture}", true);
            }
        }

        #endregion
    }
}
