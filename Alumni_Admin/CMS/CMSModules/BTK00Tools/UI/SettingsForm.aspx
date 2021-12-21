<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="BizStreamToolkit.Tools"
    Inherits="BizStreamToolkit.Tools.UI.SettingsForm"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Toolkit Tools Settings"
    Theme="Default" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">

    <div class="form-horizontal">
        <div class="btn-actions">
            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" />
        </div>
    </div>
    <div class="form-horizontal">
        <h4 class="anchor">Toolkit for Kentico Settings</h4>

        <div class="form-group">
            <div class="btk-editing-form-text">
             
            </div>
        </div>

        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label runat="server" AssociatedControlID="txtSerialNumber" CssClass="control-label">Serial Number</asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control" />
                <div class="explanation-text-settings">You can get your Serial Number at:
                    <br /><a target="_toolkit_admin" href="<%= BizStreamToolkit.Core.Constants.Urls.ToolkitAdminAccountInformationUrl %>" target="_blank"><%= BizStreamToolkit.Core.Constants.Urls.ToolkitAdminAccountInformationUrl %></a></div>
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label runat="server" AssociatedControlID="txtApiSecret" CssClass="control-label">API Secret</asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:TextBox ID="txtApiSecret" runat="server" CssClass="form-control" TextMode="Multiline" Rows="5" />
            </div>

        </div>
        <!-- Deprecated -->
        <!-- 
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label runat="server" CssClass="control-label">License</asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:Label ID="lblLicense" runat="server" CssClass="form-control-text" />
            </div>

        </div>
        -->
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label runat="server" CssClass="control-label">Review your Subscription</asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <div class="explanation-text-settings">Check out the status of all of your extensions here: 
                    <br /><a target="_toolkit_admin" href="<%= BizStreamToolkit.Core.Constants.Urls.ToolkitAdminManageSubscriptionsUrl %>"><%= BizStreamToolkit.Core.Constants.Urls.ToolkitAdminManageSubscriptionsUrl %></a></div>
            </div>
        </div>
    </div>


    <asp:PlaceHolder ID="plcGeneralError" runat="server" Visible="false" EnableViewState="false">
        <div class="alert-dismissable alert-error alert">
            <span class="alert-icon"><i class="icon-times-circle"></i><span class="sr-only">Error</span></span>
            <div class="alert-label">
                <strong>
                    <asp:Literal runat="server" ID="ltlGeneralErrorHeader"></asp:Literal></strong>
            </div>
            <div class="alert-description">
                <asp:Literal runat="server" ID="ltlGeneralErrorDetail"></asp:Literal>
            </div>
        </div>
    </asp:PlaceHolder>

   <asp:PlaceHolder ID="plcBtkHelper" runat="server" Visible="false" EnableViewState="false">
       <div class="btk-instructions">
            <h4><asp:Literal runat="server" ID="ltlHelperHeader"></asp:Literal></h4>
            <div class="btk-helper-description">
                <asp:Literal runat="server" ID="ltlHelperDetail"></asp:Literal>
            </div>
        </div>
   </asp:PlaceHolder>
</asp:Content>
