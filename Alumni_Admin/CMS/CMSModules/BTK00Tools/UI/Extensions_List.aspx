<%@ Page Language="C#" AutoEventWireup="true" 
	CodeBehind="BizStreamToolkit.Tools" 
    Inherits="BizStreamToolkit.Tools.Extensions.UI.ExtensionsListWebForm"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Toolkit for Kentico Extension Manager"
    Theme="Default" %>

<%@ Register TagPrefix="bzs" Namespace="BizStreamToolkit.Tools.Common.UI" Assembly="BizStreamToolkit.Tools" %>


<asp:Content ID="plcContent" ContentPlaceHolderID="plcContent" runat="server">

    <div class="form-horizontal header">
        <div class="btk-extension-header">
            <img src="<%= URLHelper.ResolveUrl("~/CMSModules/BTK00Tools/Icons/toolkit-64.png") %>">
            <div class="btk-extension-name">
                <h1>Toolkit</h1>
                <p>Easily install, upgrade and manage your Toolkit Extensions.</p>
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
        <div class="alert-buttons">
            <asp:HyperLink runat="server" ID="lnkGeneralErrorAction" CssClass="btn btn-primary" Text="Go Here" NavigateUrl="#" />
        </div>
    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="plcNuget" runat="server" Visible="false" EnableViewState="false">
        <div class="btk-instructions">
            <h4><asp:Literal runat="server" ID="ltlNugetHeader"></asp:Literal></h4>
            <p>
                It is <strong>highly recommended</strong> that you install, update and uninstall 
                your Toolkit for Kentico extensions 
                via the <strong>NuGet Package Manager</strong> in Visual Studio. However, you can install and update using the "Classic Installation" method below.
            </p>
            <p>
                For more information, please <a target="_support" href="<%= BizStreamToolkit.Core.Constants.Urls.HelpForInstallationUrl %>">review our documentation</a> regarding NuGet Installation.
            </p>
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="plcInstalled" runat="server" Visible="true">
        <bzs:ToolkitGridControl ID="gridInstalled" ItemName="Extension" CssClass="btk-extension-list btk-installed"
            CountText="installed on this site" runat="server" MaxRecordsPerGrid="500" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="plcAvailable" runat="server" Visible="true">
        <bzs:ToolkitGridControl ID="gridAvailable" ItemName="Extension" CssClass="btk-extension-list btk-available"
            CountText="available for installation" runat="server" MaxRecordsPerGrid="500" />
    </asp:PlaceHolder>
</asp:Content>