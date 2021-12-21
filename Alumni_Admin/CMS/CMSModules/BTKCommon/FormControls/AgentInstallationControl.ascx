<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BizStreamToolkit.Tools" Inherits="BizStreamToolkit.Tools.Common.UI.AgentInstallationControl" %>
<div>
    <asp:Panel runat="server" ID="panelVersion">
        <div class="cms-bootstrap">
            <div class="alert-error alert">
                <span class="alert-icon"><i class="icon-exclamation-triangle"></i><span class="sr-only">Info</span></span>
                <asp:Label runat="server" CssClass="alert-label" ID="labelVersionInstructions" ClientIDMode="Static">
            <div>Before we can use this site, we must <strong>Create a Secure Connection</strong>, and ensure that you have Global Administrator rights to the site.</div>
                </asp:Label>
            </div>
        </div>
        <asp:Button CssClass="btn btn-primary" runat="server" ID="btnCheckVersion" ClientIDMode="Static" Text="Create Secure Connection" OnClick="btnCheckVersion_Click1" />
        <br />
    </asp:Panel>
</div>
<div>
    <asp:Panel runat="server" ID="panelAuthorization" Visible="false">
        <div class="cms-bootstrap">
            <div class="alert-info alert">
                <span class="alert-icon"><i class="icon-i-circle"></i><span class="sr-only">Info</span></span>

                <div class="alert-label">
                <asp:Literal runat="server" ID="ltlAuthorizationInstructions" ClientIDMode="Static"></asp:Literal>
            <div>To create a secure connection, you must log in to the site as a Kentico user with Global Administrator privileges.</div>
            <div><em>Note:</em> Your username/password will not be stored by Compare for Kentico. This is simply to ensure you are a global admin of the site you are attempting to connect to.</div>
                </div>
            </div>
        </div>
        <strong>Username:</strong>
        <br />
        <asp:TextBox CssClass="form-control" runat="server" ID="textAdminUser" />
        <br /><br />
        <strong>Password:</strong>
        <br />
        <asp:TextBox TextMode="Password" CssClass="form-control" runat="server" ID="textAdminPassword" />
        <br /><br />
        <asp:Button CssClass="btn btn-primary" runat="server" ID="btnCheckPermissions" ClientIDMode="Static" Text="Create Secure Connection" OnClick="btnCheckPermissions_Click" OnClientClick="onClickDelayLoader($cmsj, $cmsj('#btnCheckPermissions'));" />
    </asp:Panel>
</div>
<div>
    <asp:Panel runat="server" ID="panelEncryption" Visible="false">
        <asp:TextBox CssClass="form-control" ReadOnly="true" runat="server" ID="textEncryptionKey" />
        <asp:Button CssClass="btn btn-primary" runat="server" ID="btnResetKey" Text="Clear / Reset Secure Connection" OnClick="btnClearReset_Click" />
    </asp:Panel>
</div>
