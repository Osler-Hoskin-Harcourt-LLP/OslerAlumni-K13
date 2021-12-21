<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="BizStreamToolkit.Siteimprove"
    Inherits="BizStreamToolkit.Siteimprove.UI.PagesTabWebForm"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Toolkit Siteimprove Pages Tab"
    Theme="Default"
    Async="true" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <div class="btn-actions">
        <asp:Button runat="server" ID="btnRecheckPage" Text="Re-check this page" CssClass="btn btn-primary" />
        <asp:Button runat="server" ID="btnRecrawlSite" Text="Re-crawl website" CssClass="btn btn-default" />
        <asp:HyperLink runat="server" ID="btnSettings" Text="Go to Settings" CssClass="btn btn-default" Target="settings" />
        <asp:HyperLink runat="server" ID="btnGetPremium" Text="Get Siteimprove (Premium extension)" CssClass="btn btn-primary" Target="_blank" />
    </div>
    <div class="PageContent">

        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" AssociatedControlID="txtSiteUrl" CssClass="control-label">Live URL</asp:Label>
                </div>
                <div class="editing-form-value-cell">
                    <asp:TextBox ID="txtSiteUrl" runat="server" CssClass="form-control" /> 
                </div>
            </div>
        </div>

    </div>

</asp:Content>
