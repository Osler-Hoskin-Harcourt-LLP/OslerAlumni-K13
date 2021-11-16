<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true" CodeBehind="ECAPageAliasList.aspx.cs" Inherits="CMSApp.Ecentricarts.Modules.ECAPageAliasList" Theme="Default" %>
<%@ Import Namespace="ECA.PageURL.Kentico.Models" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<asp:Content runat="server" ContentPlaceHolderID="plcContent" ID="plcAliasList">
    <asp:Literal ID="ltlMessage" runat="server" Visible="false"></asp:Literal>
    <asp:Panel ID="pnlList" runat="server">
        <p><strong>Main URL: </strong><asp:Literal ID="ltlMainUrl" runat="server"></asp:Literal></p>

        <cms:UniGrid ID="ugdAliasList" runat="server" ObjectType="<%# CustomTable_PageURLItem.CLASS_NAME %>">

            <GridActions>
                <ug:Action Name="edit" Caption="$General.Edit$" FontIconClass="icon-edit" FontIconStyle="allow" CommandArgument="ItemID" />
                <ug:Action Name="delete" Caption="$General.Delete$" FontIconClass="icon-bin" FontIconStyle="critical" Confirmation="$General.ConfirmDelete$" CommandArgument="ItemID" />
            </GridActions>

            <GridColumns>
                <ug:Column Source="Culture" Caption="Culture" Localize="true" Width="15%" />
                <ug:Column Source="URLPath" Caption="Alias URL" Localize="true" Width="75%" />
                <ug:Column Source="IsMainURL" Caption="Is Main URL" Localize="true" Width="5%" />
                <ug:Column Source="IsCanonicalURL" Caption="Is Canonical URL" Localize="true" Width="5%" />
            </GridColumns>

            <GridOptions DisplayFilter="true" />

        </cms:UniGrid>
    </asp:Panel>
</asp:Content>