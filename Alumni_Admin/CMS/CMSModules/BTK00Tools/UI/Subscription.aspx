<%@ Page Language="C#" AutoEventWireup="true" 
	CodeBehind="BizStreamToolkit.Tools" 
    Inherits="BizStreamToolkit.Tools.Extensions.UI.SubscriptionWebForm"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Toolkit Subscription Settings"
    Theme="Default" %>

<%@ Register TagPrefix="cms" TagName="SettingsGroupViewer" Src="~/CMSModules/Settings/Controls/SettingsGroupViewer.ascx" %>
<asp:content contentplaceholderid="plcContent" id="content" runat="server">
    <cms:SettingsGroupViewer ID="SettingsGroupViewer" runat="server" AllowGlobalInfoMessage="false" />
</asp:content>