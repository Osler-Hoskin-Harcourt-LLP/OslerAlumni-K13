<%@ Page Language="C#" AutoEventWireup="true" Async="true"
    MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master" Title="Extension Installer/Upgrader"
    CodeBehind="BizStreamToolkit.Tools"
    Inherits="BizStreamToolkit.Tools.Common.UI.ExtensionInstallerControl" 
    Theme="Default" 
     %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">

    <div class="form-horizontal">

        <asp:Panel runat="server" ID="btk_pnlMain" CssClass="btn-extention-installer">

            <h4><asp:Literal ID="btk_ltlAction" EnableViewState="true" runat="server" Text="Updating ExtentionName on" />:<br /><asp:Literal ID="btk_ltlServerInfo" EnableViewState="true" runat="server" /></h4>

            <br /><asp:Literal ID="btk_ltlVersionAction" EnableViewState="true" runat="server"></asp:Literal><strong><asp:Literal ID="btk_ltlCurrentVersion" EnableViewState="true" runat="server" /></strong>
            <br />
            <br />You are installing this zip:<br /><strong><asp:Literal ID="btk_ltlZip" EnableViewState="true" runat="server" /></strong>

            <!-- TODO, Make this a User Control -->
            <asp:Panel runat="server" ID="btk_pnlWindowsAuth" CssClass="btk-windows-auth">
            
                <h5 class="editing-form-category-caption">Windows Authentication for the server (optional)</h5>
                <div class="information InfoLabel">When installing Extensions (importing Kentico Objects), you may get write permission issues due to the App Pool user not having write access to the file system. 
                    To prevent this, you can enter the Windows User Credentials for a user that has write permissions on the server.</div>
                <div class="form-group">
                    <div class="editing-form-label-cell"><label class="control-label editing-form-label" for="txtDomain">Domain:</label></div>
                    <div class="editing-form-value-cell"><div class="EditingFormControlNestedControl editing-form-control-nested-control">
                        <asp:TextBox runat="server" ID="btk_txtDomain" EnableViewState="true" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
		            </div></div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell"><label class="control-label editing-form-label" for="txtUsername">Username:</label></div>
                    <div class="editing-form-value-cell"><div class="EditingFormControlNestedControl editing-form-control-nested-control">
                        <asp:TextBox runat="server" ID="btk_txtUsername" EnableViewState="true" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
		            </div></div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell"><label class="control-label editing-form-label" for="txtDomain">Password:</label></div>
                    <div class="editing-form-value-cell"><div class="EditingFormControlNestedControl editing-form-control-nested-control">
                        <asp:TextBox runat="server" ID="btk_txtPassword" TextMode="Password" EnableViewState="true" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
		            </div></div>
                </div>
            </asp:Panel>
            <!-- End Future User control -->

        </asp:Panel>
        <asp:Panel runat="server" ID="btk_pnlError" Visible="false">
        </asp:Panel>

    </div>

 </asp:Content>

<asp:Content ID="cntFooter" ContentPlaceHolderID="plcFooter" runat="server">
    <cms:LocalizedButton runat="server" ClientIDMode="Static" UseSubmitBehavior="false" ID="btk_btnAutoUpdate" OnClientClick="return false;" data-msg="This might cause a RECOMPILE on this Site." ButtonStyle="Primary" Enabled="true" EnableViewState="false" ResourceString="Install Now" />
    <cms:LocalizedButton runat="server" ClientIDMode="Static" UseSubmitBehavior="false" ID="btk_btnAutoUpdaterClose" Visible="false" ButtonStyle="Primary" Enabled="true" EnableViewState="false" ResourceString="Close" />
</asp:Content>
