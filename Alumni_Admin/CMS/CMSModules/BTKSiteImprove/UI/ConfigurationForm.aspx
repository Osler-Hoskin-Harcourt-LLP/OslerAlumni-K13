<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="BizStreamToolkit.Siteimprove"
    Inherits="BizStreamToolkit.Siteimprove.UI.ConfigurationForm"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Toolkit Siteimprove Confirguration"
    Theme="Default"
    Async="true" %>

<%--    CodeFile="ConfigurationForm.aspx.cs"
    Inherits="CMSModules_BTKSiteImprove_UI_Configuration"--%>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">

    <div class="form-horizontal header">
        <div class="btk-extension-header">
            <div class="btk-extension-name-full-logo">
                <img style="width:237.5px;" src="<%= URLHelper.ResolveUrl("~/CMSModules/BTKSiteimprove/Icons/siteimprove-transparent-logo.png") %>" />
                <h4>Siteimprove (Premium extension)</h4>
                <div>Seamlessly integrate the Siteimprove Intelligence Platform with Kentico via a pre-publish advanced workflow.</div>
            </div>
        </div>
    </div>

    <div class="form-horizontal" runat="server">
        <div class="btn-actions">
            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" />
        </div>

        <div class="form-group">
            <div class="btk-editing-form-text">
                To get started, enter in a valid Siteimprove API User Name and API Key
            </div>
        </div>

        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label runat="server" AssociatedControlID="txtApiUserName" CssClass="control-label">API User Name</asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:TextBox ID="txtApiUserName" runat="server" CssClass="form-control" /> 
                <span class="explanation-text">Example: email@domain.com</span>
            </div>
        </div>

        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label runat="server" AssociatedControlID="txtApiKey" CssClass="control-label">API Key</asp:Label>
            </div>
            <div class="editing-form-value-cell">
                <asp:TextBox ID="txtApiKey" runat="server" CssClass="form-control" /> 
            </div>
        </div>
    </div>

    <asp:PlaceHolder ID="plcContentCheck" runat="server" Visible="false">
        <div class="form-horizontal" runat="server">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" CssClass="control-label">Content Check Enabled?</asp:Label>
                </div>

                <asp:PlaceHolder ID="plcContentCheckNotEnabled" runat="server" Visible="false" EnableViewState="false">
                    <div class="btk-editing-form-text">
                        <i aria-hidden="true" class="icon-ban-sign cms-icon-64"></i> Content Check is not enabled on Siteimprove
                        <asp:Button runat="server" ID="btnEnableContentCheck" Text="Enable Content Check on Siteimprove" CssClass="btn btn-primary" />
                    </div>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="plcContentCheckEnabled" runat="server" Visible="false" EnableViewState="false">
                    <div class="btk-editing-form-text">
                        <i aria-hidden="true" class="icon-check cms-icon-64"></i> Content Check is enabled on Siteimprove
                    </div>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="plcContentCheckError" runat="server" Visible="false" EnableViewState="false">
			        <div class="alert-dismissable alert-error alert">
				        <span class="alert-icon"><i class="icon-times-circle"></i><span class="sr-only">Error</span></span>
                        <div class="alert-label"><strong><asp:Literal runat="server" ID="ltlErrorHeader"></asp:Literal></strong>
                            <div class="alert-description"><asp:Literal runat="server" ID="ltlErrorDetail"></asp:Literal></div>
                        </div>
			        </div>
                </asp:PlaceHolder>

            </div>
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="plcGeneralError" runat="server" Visible="false" EnableViewState="false">
		<div class="alert-dismissable alert-error alert">
			<span class="alert-icon"><i class="icon-times-circle"></i><span class="sr-only">Error</span></span>
            <div class="alert-label"><strong><asp:Literal runat="server" ID="ltlGeneralErrorHeader"></asp:Literal></strong></div>
            <div class="alert-description"><asp:Literal runat="server" ID="ltlGeneralErrorDetail"></asp:Literal></div>
		</div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="plcHowTo" runat="server" Visible="true" EnableViewState="false">
        <div class="btk-instructions btk-top-buffer btk-allow-full-width">
            <h4>Looking for some help on setting up your Siteimprove integration?</h4>
            To get API settings from your Siteimprove Dashboard, <a href="javascript:$cmsj('#details').toggleClass('hide')">follow these instructions</a>
            <div class="hide" id="details">
                <br />
                <ol>
                    <li>Go to: <a href="https://my2.siteimprove.com/Integrations/Api/ManageApiKeys" target="_blank">Siteimprove Dashboard > Main Menu > Integrations > APIs > API Keys</a>.
                        <br />&nbsp;</li>
                    <li>If you  <strong>do not have an API Key for Content Check</strong>, we suggest you create a new one:
                        <ol>
                            <li>Click the <strong>Create API Key</strong> button.</li>
                            <li>Enter a <strong>Description</strong>. It can be anything, but we recommend something like: "Content Check".</li>
                            <li>Now either <strong>select user</strong> or create a new user.</li>
                            <li>When you are done click the <strong>Create API Key</strong> button.</li>
                        </ol>
                        <br />
                    </li>
                    <li>Once you have an <strong>API User Name</strong> and <strong>API Key</strong>:
                        <ol>
                            <li>Copy the <strong>API User Name</strong> from Siteimprove to this form (not the Description or User name).</li>
                            <li>Copy the <strong>API Key</strong> from Siteimprove to this form.</li>
                            <li>Click the <strong>Save</strong> button on this form.</li>
                        </ol>
                        <br />
                    </li>
                    <li>If <strong>Content Check</strong> is not enabled, follow the prompts on this form to get it enabled</li>
                </ol>
                <br />For more info, check out our <a href="<%= BizStreamToolkit.Siteimprove.Constants.Urls.GetHelpUrl %>" target="_blank">documentation page</a>.
            </div>
        </div>
    </asp:PlaceHolder>

    <div class="form-horizontal btk-siteimprove-features">
        <h4 style="margin-top:2px;color: #666;">Siteimprove (Premium extension):</h4>
        <ul style="margin:8px 12px;">
            <li>Ensure consistency and compliance on your website</li>
            <li>Pre-Publishing Checks into a Kentico <strong>Advanced Workflow</strong>
                <ul>
                    <li>Immediately validate your content against Siteimprove Policies <strong>before it is published</strong> on the live site.</li>
                    <li>Customize the provided advanced workflow to fit your needs</li>
                    <li>Customize approvals or rejections based on policy matches</li>
                    <li>Automatic re-check</li>
                </ul>
            </li>
            <li>With Siteimprove Policies you can create checks for your content that:
                <ul>
                        <li>Finds all images on your product site that are older than a certain timeframe to make sure your product images are up to date.</li>
                        <li>Finds every instance of a former employee's name to check whether it's been removed from the site.</li>
                        <li>Looks for a specific HTML tag on your website to remove old code that doesn't meet best practice.</li>
                        <li>Much more</li>
                </ul>
            </li>
        </ul>
        <asp:PlaceHolder ID="plcPremiumExplainer" runat="server" Visible="true" EnableViewState="false">
            <a class="btn btn-primary" target="_blank" href="<%= BizStreamToolkit.Core.Constants.Urls.BizStreamToolkitMarketingSite %>">Get Siteimprove Premium</a>
        </asp:PlaceHolder>
    </div>


</asp:Content>
