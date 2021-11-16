using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Autofac;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.UIControls;
using ECA.Admin.Core.Extensions;
using ECA.PageURL.Kentico.Models;
using ECA.PageURL.Repositories;
using ECA.PageURL.Services;
using OslerAlumni.Admin.ECA.Core.Extensions;

namespace CMSApp.Ecentricarts.Modules
{
    [UIElement("Ecentricarts", "PageAliasList")]
    public partial class ECAPageAliasList : CMSPropertiesPage
    {
        #region "Constants"

        private const string EditPageUrl = "~/Ecentricarts/Modules/PageAlias/UI/ECAPageAliasEdit.aspx";

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
        public ECAPageAliasList()
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
            var newAlias = new HeaderAction
            {
                Text = GetString("New Alias Url"),
                RedirectUrl = $"{EditPageUrl}?nodeid={NodeID}&culture={CultureCode}"
            };

            var resetMainUrl = new HeaderAction
            {
                Text = GetString("Reset Main Url"),
                CommandName = "ResetMainUrl",
            };

            CurrentMaster.HeaderActions.ActionPerformed += HeaderActionsOnActionPerformed;

            CurrentMaster.HeaderActions.AddAction(newAlias);
            CurrentMaster.HeaderActions.AddAction(resetMainUrl);

            ugdAliasList.OnAction += ugdAliasList_OnAction;
            ugdAliasList.OnBeforeDataReload += LoadData;

            LoadData();

            DataBind();
        }

        /// <summary>
        /// Ugds the alias list on action.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionArgument">The action argument.</param>
        private void ugdAliasList_OnAction(string actionName, object actionArgument)
        {
            var itemId = ValidationHelper.GetInteger(actionArgument, 0);

            if (itemId != 0)
            {
                switch (actionName.ToLower())
                {
                    case "delete":
                        var customTable = DataClassInfoProvider.GetDataClassInfo(CustomTable_PageURLItem.CLASS_NAME);

                        if (customTable != null)
                        {
                            var item = PageUrlItemRepository.GetPageUrlItem(itemId);

                            if (item != null)
                            {
                                item.Delete();
                            }
                        }
                        break;

                    case "edit":
                        Response.Redirect($"{EditPageUrl}?nodeid={NodeID}&culture={CultureCode}&aliasid={itemId}");
                        break;
                }
            }
        }

        /// <summary>
        /// Proccess header action click.
        /// </summary>
        private void HeaderActionsOnActionPerformed(object sender, CommandEventArgs commandEventArgs)
        {
            switch (commandEventArgs.CommandName)
            {
                case "ResetMainUrl":
                    ResetMainUrl();
                    break;
            }
        }

        #endregion

        #region "Helper methods"

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            var mainUrlItem =
                PageUrlItemRepository.GetMainPageUrlItem(
                    Node.NodeGUID,
                    CultureCode,
                    columnNames: new[] { "URLPath" });

            if (mainUrlItem != null)
            {
                ltlMainUrl.Text = mainUrlItem.URLPath;
            }

            // UniGrid must be bound to a dataset
            ugdAliasList.DataSource = PageUrlItemRepository
                .GetPageUrlItems(
                    Node.NodeGUID,
                    new List<string> { CultureCode },
                    false,
                    columnNames: new List<string>
                    {
                        "ItemID",
                        "Culture",
                        "URLPath",
                        "IsMainURL",
                        "IsCanonicalURL"
                    })
                .Where(t => t.IsMainURL == false)
                .ToList()
                .ConvertListToDataSet();
        }

        /// <summary>
        /// Resets the main url of the page
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ResetMainUrl()
        {
            if (!PageUrlService.TrySetPageMainUrl(Node, true))
            {
                ltlMessage.Text = "Error has occured.";
                ltlMessage.Visible = true;
            }

            //Reload the page.
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        #endregion
    }
}
