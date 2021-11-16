using System;
using System.Web.UI.WebControls;
using Autofac;
using CMS.Core;
using CMS.FormEngine.Web.UI;
using ECA.Admin.Core.Extensions;
using ECA.Core.Repositories;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Services;


public partial class CMSFormControls_OnePlaceCampaignSelector : FormEngineUserControl
{
    #region "Private fields"

    private ILifetimeScope _scope;

    #endregion


    #region "Properties"

    public IOnePlaceFunctionService OnePlaceFunctionService { get; set; }
    public IEventLogRepository EventLogRepository { get; set; }

    /// <summary>
    /// Gets or sets form control value.
    /// </summary>
    public override object Value
    {
        get { return ddlCampaigns.SelectedValue; }
        set { ddlCampaigns.SelectedValue = value as string; }
    }


    #endregion

    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        _scope = this.InjectDependencies();

        base.OnInit(e);

        if (!IsPostBack)
        {
            SetupDropDown();
        }
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

    private void SetupDropDown()
    {
        try
        {
            ddlCampaigns.DataSource = OnePlaceFunctionService.GetFunctions();
            ddlCampaigns.DataValueField = nameof(OnePlaceFunction.Id);
            ddlCampaigns.DataTextField = nameof(OnePlaceFunction.Name);
            ddlCampaigns.DataBind();
        }
        catch (Exception e)
        {
            //Sometimes related Oneplace functions get deleted from Oneplace, so rebinding these cause exceptions,

            EventLogRepository.LogError(GetType(), nameof(SetupDropDown), $"{ddlCampaigns.SelectedValue} does not exist.");
        }

        ddlCampaigns.Items.Insert(0, new ListItem("<---Select a Function--->", String.Empty));
    }

    #endregion
}

