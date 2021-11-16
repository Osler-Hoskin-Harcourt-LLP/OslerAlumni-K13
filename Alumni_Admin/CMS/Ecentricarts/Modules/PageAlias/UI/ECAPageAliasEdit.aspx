<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true" CodeBehind="ECAPageAliasEdit.aspx.cs" Inherits="CMSApp.Ecentricarts.Modules.PageAlias.ECAPageAliasEdit" Theme="Default" %>

<asp:Content runat="server" ContentPlaceHolderID="plcContent" ID="plcChartDataDetail">
    
    <p style="color:red"><asp:Literal ID="ltlError" runat="server"></asp:Literal></p>

    <div class="form-horizontal">

        <h4>Add/Edit URL Alias</h4>

        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label ID="lblUrlPath" runat="server" Text="URL Path:" AssociatedControlID="txtUrlPath" CssClass="control-label editing-form-label"></asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:TextBox ID="txtUrlPath" runat="server" Width="500px" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
                
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label ID="lblIsMainURL" runat="server" Text="Is Main URL?:" AssociatedControlID="chbIsMainURL" CssClass="control-label editing-form-label"></asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox ID="chbIsMainURL" runat="server" CssClass="field-value-override-checkbox" />
            </div>
        </div>
                
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label ID="lblIsCanonicalURL" runat="server" Text="Is Canonical URL?:" AssociatedControlID="chbIsCanonicalURL" CssClass="control-label editing-form-label"></asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox ID="chbIsCanonicalURL" runat="server"  />
            </div>
        </div>
                
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label ID="lblRedirectType" runat="server" Text="Redirect to the main URL?:" AssociatedControlID="ddlRedirectType" CssClass="control-label editing-form-label"></asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:DropDownList ID="ddlRedirectType" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Use site settings (default)" Value=""></asp:ListItem>
                    <asp:ListItem Text="Redirect to main URL" Value="redirect"></asp:ListItem>
                    <asp:ListItem Text="Do not redirect" Value="donothing"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label ID="lblSave" runat="server" AssociatedControlID="btnSave" CssClass="control-label editing-form-label"></asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />

            </div>
        </div>
    </div>
</asp:Content>