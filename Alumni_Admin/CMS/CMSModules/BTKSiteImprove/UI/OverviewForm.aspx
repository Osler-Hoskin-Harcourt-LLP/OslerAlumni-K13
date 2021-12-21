<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="BizStreamToolkit.Siteimprove"
    Inherits="BizStreamToolkit.Siteimprove.UI.OverviewForm"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Toolkit Siteimprove Confirguration"
    Theme="Default"
    Async="true" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <div class="form-horizontal header">
        <div class="btk-extension-header">
            <div class="btk-extension-name-full-logo">
                <img style="width:237.5px;" src="<%= URLHelper.ResolveUrl("~/CMSModules/BTKSiteimprove/Icons/siteimprove-transparent-logo.png") %>" />
                <div>Seamlessly integrate the Siteimprove Intelligence Platform with Kentico.</div>
            </div>
        </div>
    </div>

    There are two ways that the Siteimprove platform can be integrated with Kentico in this <strong>Toolkit for Kentico</strong> extension. The Basic extension adds in the Siteimprove CMS plugin to the Kentico Pages application. Optionally, the Premium extension  adds Siteimprove Pre-Publishing Checks into a Kentico Advanced Workflow. Details of both options are below.
    <div class="row">
        <div class="col-md-6">
            <h4>Siteimprove (Basic extension)</h4>
            <ul>
                <li>Integrate Siteimprove CMS Plugin into your Kentico Pages application. </li>
                <li>Automatically re-check pages when they are published.
                    <ul>
                        <li>No need to wait for results till the next day</li>
                    </ul>
                </li>
                <li>The Siteimprove CMS Plugin provides insights into:
                    <ul>
                        <li>Misspellings and broken links</li>
                        <li>Readability levels</li>
                        <li>Accessibility issues (A, AA, AAA conformance level)</li>
                        <li>High-priority policies</li>
                        <li>SEO: technical, content, UX, and mobile</li>
                        <li>Page visits and page views</li>
                    </ul>
                </li>
            </ul>
        </div> <!-- end col-md-6 -->

        <div class="col-md-6">
            <h4>Siteimprove (Premium extension)</h4>
            <ul>
                <li>Includes all Basic extension features</li>
                <li>Requires Kentico EMS edition</li>
                <li>Requires Siteimprove subscription with Policies feature enabled</li>
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

        </div> <!-- end col-md-6 -->
    </div> <!-- end row -->

    <div class="row">
        <div class="col-md-12">
            <asp:PlaceHolder ID="plcPremiumExplainer" runat="server" Visible="true">
                    You currently have the <strong>Siteimprove (Basic extension)</strong>
            </asp:PlaceHolder>
    
            <asp:PlaceHolder ID="plcPremiumHasIt" runat="server" Visible="true">
                    You currently have the <strong>Siteimprove (Premium extension)</strong>
            </asp:PlaceHolder>

            <div class="btn-actions" style="margin-top:6px;">
                <asp:HyperLink runat="server" ID="lnkPrimary" CssClass="btn btn-primary" />
            </div>
        </div> <!-- end col-md-12 -->
    </div> <!-- end row -->

</asp:Content>
