<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BizStreamToolkit.Tools" Inherits="BizStreamToolkit.Tools.Common.UI.ObjectFilterControl" %>
<%@ Register TagPrefix="btCustomControls" Namespace="BizStreamToolkit.Tools.Common.CustomControls" Assembly="BizStreamToolkit.Tools" %>
<%@ Register TagPrefix="cms" TagName="RadioButtonsControl" Src="~/CMSFormControls/Basic/RadioButtonsControl.ascx" %>

    <div class="form-filter" style="margin-bottom:0">
        <div class="form-group">
            <div class="filter-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblUseExisting" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="kenticocompare.useexistingsettings" />
            </div>
            <div style="clear:left">
            <cms:LocalizedDropDownList runat="server" ID="drpSavedConfigurations" ClientIDMode="Static" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="drpSavedConfigurations_SelectedIndexChanged" />
            <cms:LocalizedLiteral ID="ltlFilterDescription" ClientIDMode="Static" runat="server" />
            </div>
        </div>
    </div>

    <span class="InfoLabel" id="showHideFilter" data-state="closed" data-icon-opened="icon-caret-up" data-icon-closed="icon-caret-down" data-text-opened="<%= GetString("kenticocompare.hidefilter") %>" data-text-closed="<%= GetString("kenticocompare.displayfilter") %>">
        <i class="icon-caret-down cms-icon-30"></i>
        <a href='#' onclick="javascript:return false;"><%= GetString("kenticocompare.displayfilter") %></a>
    </span>

    <div id="plcFilter" style="display:none">
        <div class="nav-tabs-container-horizontal">
            <ul id="filter-tabs" class="nav nav-tabs">
                <li data-tab="tab-objects" class="active"><a class="LinkSelected"><span class="tab-title"><i class="icon-kentico"></i> <span class="wide-only">Kentico </span>Objects</span></a></li>
                <li data-tab="tab-pages"><a class="Link"><span class="tab-title"><i class="icon-doc"></i> Pages</span></a></li>
                <li data-tab="tab-table-data"><a class="Link"><span class="tab-title"><i class="icon-table"></i> Custom Table<span class="wide-only"> Items</span></span></a></li>
                <li data-tab="tab-fields"><a class="Link"><span class="tab-title"><i class="icon-clipboard-list"></i> Fields/Columns</span></a></li>
                <li data-tab="tab-files"><a class="Link"><span class="tab-title"><i class="icon-folder-opened"></i> Folders/Files</span></a></li>
                <li data-tab="tab-database"><a class="Link"><span class="tab-title"><i class="icon-database"></i> Database<span class="wide-only"> Objects</span></span></a></li>
                <li data-tab="tab-time"><a class="Link"><span class="tab-title"><i class="icon-calendar"></i> Date/Time</span></a></li>
                <li data-tab="tab-other"><a class="Link"><span class="tab-title"><i class="icon-list"></i> Other</span></a></li>
            </ul>
            <div class="filter-tab-content">
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-objects">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblObjectOptions" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="kenticocompare.objectsoptions" />
                    </div>
                    <div class="form-group dynamic-options" data-filter-controlid="fltObjectOptions" data-class="filter-Objects" data-callabck="BZSObjectFilter.OnAfterObjectFilterSelected">
                        <cms:RadioButtonsControl runat="server" ID="fltObjectOptions" />
                    </div>
                    <div class="form-group filter-Objects" data-filter="|1|2|" style="display:none">
                        <btCustomControls:CustomListBox SelectionMode="Multiple" runat="server" ID="fltObjectListChosen" CssClass="chosen-select form-control" data-placeholder="Select Kentico Objects..."></btCustomControls:CustomListBox>
                   </div>

                    <div class="form-group two-col-checks object-checkboxes filter-Objects" data-filter="|0|1|2|" style="display:none">
                        <div class="single-check" title="cms.user, cms.usersite, cms.userculture" style="clear:left"><asp:CheckBox runat="server" ID="chkExcludeUsers" AutoPostBack="False"/> <span class="filter-Objects" data-filter="|1|">Include</span><span class="filter-Objects" data-filter="|0|2|">Exclude</span> Users</div>
                        <div class="single-check" title="cms.role"><asp:CheckBox runat="server" ID="chkExcludeRoles" AutoPostBack="False"/> <span class="filter-Objects" data-filter="|1|">Include</span><span class="filter-Objects" data-filter="|0|2|">Exclude</span> Roles</div>
                        <div class="single-check" title="cms.documentalias" style="clear:left"><asp:CheckBox runat="server" ID="chkExcludeDocumentAliases" AutoPostBack="False"/> <span class="filter-Objects" data-filter="|1|">Include</span><span class="filter-Objects" data-filter="|0|2|">Exclude</span> Page Aliases</div>
                        <div class="single-check" title="cms.acl, cms.aclitem"><asp:CheckBox runat="server" ID="chkExcludeACLs" AutoPostBack="False"/> <span class="filter-Objects" data-filter="|1|">Include</span><span class="filter-Objects" data-filter="|0|2|">Exclude</span> Page Security (ACLs)</div>
                        <div class="single-check" style="clear:left" title="cms.classsite, cms.cssstylesheetsite, cms.pagetemplatesite, cms.*site"><asp:CheckBox runat="server" ID="chkExcludeSiteAssociations" AutoPostBack="False"/> <span class="filter-Objects" data-filter="|1|">Include</span><span class="filter-Objects" data-filter="|0|2|">Exclude</span> Site Associations</div>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkExcludeBizStreamToolkit" AutoPostBack="False"/> <span class="filter-Objects" data-filter="|1|">Include</span><span class="filter-Objects" data-filter="|0|2|">Exclude</span> Toolkit for Kentico Objects</div>
                    </div>

                    <div class="form-group filter-Objects" data-filter="|0|1|2|">
                        <div class="filter-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel4" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="kenticocompare.keywordoptions" />
                        </div>
                        <div class="form-group dynamic-options" data-filter-controlid="fltKeywordOptions" data-class="filter-ObjectKeywords">
                            <cms:RadioButtonsControl runat="server" ID="fltKeywordOptions" />
                        </div>
                    </div>
                    <div class="form-group filter-ObjectKeywords" style="display:none">
                        <table class="two-col-textarea">
                            <tr>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel5" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Include object keywords:" /></td>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel7" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Exclude object keywords:" /></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="form-control" runat="server" ID="fltKeywordList" TextMode="MultiLine" style="height: 90px;margin-top: 8px;"></asp:TextBox></td>
                                <td><asp:TextBox CssClass="form-control" runat="server" ID="fltKeywordExcludeList" TextMode="MultiLine" style="height: 90px;margin-top: 8px;"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-objects" >
                    <span class="control-label">Kentico Objects Help:</span>
                    <p>There are hundreds of Objects in Kentico (Page Templates, Page Types, Transformations, Queries, Web Parts, ...). Most likely, you don't want to compare <em>all</em> of them. Use the options on the left to filter out the things you do not want to see. You can filter by <strong>Object Type (ClassName)</strong> (use the selectors to include or exclude types).</p>
                    <p>Or, you can filter by <strong>object keyword</strong>. The keyword filter looks at an object's Display Name, Code Name and Full Name.</p>
                    <%= GenerateContainsNote( ObjectName : "Include and Exclude object keywords", ClassName: "ObjectKeywordsRelated") %>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->

                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-pages">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblPageOptions" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Page Content Options" />
                    </div>
                    <div class="form-group dynamic-options" data-filter-controlid="fltPageOptions" data-class="filter-Pages">
                        <cms:RadioButtonsControl runat="server" ID="fltPageOptions" />
                    </div>
                    <div class="form-group filter-Pages" style="display:none">
                        <cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel9" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Include these Page Types" />
                        <br /><btCustomControls:CustomListBox SelectionMode="Multiple" runat="server" ID="fltPageTypeListIncludeChosen" CssClass="chosen-select form-control" data-placeholder="Select Page Types to include..."></btCustomControls:CustomListBox>
                        <br /><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel14" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Exclude these Page Types" />
                        <br /><btCustomControls:CustomListBox SelectionMode="Multiple" runat="server" ID="fltPageTypeListExcludeChosen" CssClass="chosen-select form-control" data-placeholder="Select Page Types to exclude..."></btCustomControls:CustomListBox>

                        <table class="two-col-textarea">
                            <tr>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel17" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Include these Node Alias Paths:" /></td>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel18" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Exclude these Node Alias Paths:" /></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="form-control" runat="server" ID="fltPageNodeAliasPathIncludeList" TextMode="MultiLine" style="height: 90px;margin-top: 8px;"></asp:TextBox></td>
                                <td><asp:TextBox CssClass="form-control" runat="server" ID="fltPageNodeAliasPathExcludeList" TextMode="MultiLine" style="height: 90px;margin-top: 8px;"></asp:TextBox></td>
                            </tr>
                        </table>
                        <cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel15" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Maximum Node Level/Depth" />
                        <asp:DropDownList ID="fltPageMaxNodeLevel" runat="server" CssClass="form-control">
                            <asp:ListItem Value="-1" Text="-1 (0+, All Levels/No Limit)"></asp:ListItem>
                            <asp:ListItem Value="0" Text="0 (Root Only)"></asp:ListItem>
                            <asp:ListItem Value="1" Text="0-1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="0-2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="0-3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="0-4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="0-5"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-pages" >
                    <span class="control-label">Page Content Help:</span>
                    <p class="em">Only partially implemented. At this time only some of the "base" fields are available (more fields will be coming soon).</p>
                    <p>Use these filter options to specify which <strong>Page Content</strong> you want to see.</p>
                    <p class="listOfPageOptions">You can include or exclude certain Page Types (Document Types), you can limit the Node Alias Paths you want (ex: /Blogs/), and/or limit the "depth of the tree" by setting the max Node Level.</p>
                    <p>If no filters are used, Compare will attempt to look at ALL of the pages in the tree. If you have thousands of pages, this might timeout. You may need to do multiple compares against multiple sections of your tree.</p>
                    <p>Note: If you are interested in compairing the <strong>Page "Structure" (Fields/Settings)</strong> and not the page content. Go to the "<span class="wide-only">Kentico </span>Objects" Tab.</p>
                    <%= GenerateContainsNote("Include and Exclude Node Alias Path", true, "filter-Pages") %>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->

                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-table-data">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel13" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Custom Table Items Options" />
                    </div>
                    <div class="form-group dynamic-options" data-filter-controlid="fltCustomTableDataOptions" data-class="filter-TableData">
                        <cms:RadioButtonsControl runat="server" ID="fltCustomTableDataOptions" />
                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-table-data" >
                    <span class="control-label">Custom Table Items (Custom Table Data) Help:</span>
                    <p class="em">Only partially implemented. At this time there are no filters available (these will be coming soon).</p>
                    <!--
                    <p>Use these filter options to specify which <strong>Page Content</strong> you want to see.</p>
                    <p>You can include or exclude vertain Page Types (Document Types), you can limit the Node Alias Paths you want (ex: /Blogs/), and/or limit the "depth of the tree" by setting the max Node Level.</p>
                    -->
                    <p>Note: If you are interested in compaing the <strong>Custom Table "Structure" (Fields/Settings)</strong> and not the Date Items (records). Go to the "Kentico Objects" Tab.</p>
                    <%--<%= GenerateContainsNote("Include and Exclude Node Alias Path") %>--%>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->

                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-fields">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel140" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Field/Column Options" />
                    </div>
                    <div class="form-group dynamic-options" data-filter-controlid="fltColumnOptions" data-class="filter-Columns">
                        <cms:RadioButtonsControl runat="server" ID="fltColumnOptions" ClientIDMode="Predictable" />
                    </div>
                    <div class="form-group filter-Columns" style="display:none">
                        <table class="two-col-textarea">
                            <tr>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel16" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Exclude fields/columns:" /></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="form-control" runat="server" ID="fltColumnExcludeList" TextMode="MultiLine" ClientIDMode="Static" style="height: 90px;margin-top: 8px;"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-fields" >
                    <span class="control-label">Fields/Columns Help:</span>
                    <p>Occasionally, there are columns that you want to ignore. You can either list them here. Or, you can open a <em>View Detail</em> dialog, hover over a field name label. Then click the red "x" to exclude that field.</p>
                    <%= GenerateContainsNote( ObjectName : "Exclude field list", Multiple: false, ClassName: "filter-Columns") %>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->

                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-files">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel2" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Folders/Files Options" />
                    </div>
                    <div class="form-group dynamic-options" data-filter-controlid="fltFolderOptions" data-class="filter-Files">
                        <cms:RadioButtonsControl runat="server" ID="fltFolderOptions" />
                    </div>
                    <div class="form-group filter-Files" style="display:none">
                        <table class="two-col-textarea">
                            <tr>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel1" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Include file/folder keywords:" ToolTipResourceString="Example:
        CMSScripts/Custom
        CMSVirtualFiles
        App_Code
        .js
        .config" /></td>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel6" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Exclude file/folder keywords:" ToolTipResourceString="Example:
        CMSSiteUtils
        .bak
        .tmp
        " /> <a id="bzs-add-default-file-keyowrds" data-exclude=".bak,.tmp,.zip,.dll,.log,.orig,/CMSSiteUtils/,/CMSTemp/,/App_Data/" style="float:right" href="#">Add default keywords</a></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="form-control" runat="server" ID="fltFolderList" TextMode="MultiLine" style="height: 90px;margin-top: 8px;"></asp:TextBox></td>
                                <td><asp:TextBox CssClass="form-control" runat="server" ID="fltFolderExcludeList" TextMode="MultiLine" style="height: 90px;margin-top: 8px;"></asp:TextBox></td>
                            </tr>
                    
                        </table>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreConnectionStringInWebConfig" AutoPostBack="False"/> Ignore connection string in web.config</div>
                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-files" >
                    <span class="control-label">Folders/Files Help:</span>
                    <p>Use these filter options to specify which <strong>files and folders</strong> you want to see.</p>
                    <p>If no filters are used, Compare will attempt to look at ALL of the files in the root. Reminder: The default installation of Kentico has 10's of thousands of files.</p>
                    <p class="listOfFolders">You can include or exclude certain paths or extensions. Example: You can choose to only include <strong>/App_Code/</strong> and/or <strong>/CMSScripts/Custom/</strong>, you could also exclude <strong>.less</strong> and <strong>.scss</strong> and <strong>CMSTemp</strong> and <strong>/App_Data/</strong>.</p>
                    <%= GenerateContainsNote("Include and Exclude file/folder keywords", true, "filter-Files") %>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->

                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-database">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel8" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="kenticocompare.databaseoptions" />
                    </div>
                    <div class="form-group dynamic-options" data-filter-controlid="fltDatabaseOptions" data-class="filter-Database" style="display:block">
                        <cms:RadioButtonsControl runat="server" ID="fltDatabaseOptions" />
                    </div>
                    <div class="form-group two-col-checks filter-Database" style="display:none">
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreDatabaseDboSchema" AutoPostBack="False"/> Ignore Database "[dbo] schema diffs"</div>
                        <div class="single-check" style="clear:left"><asp:CheckBox runat="server" ID="chkIgnoreDatabaseCreateDiffs" AutoPostBack="False"/> Ignore Database "CREATE [OBJECT] diffs"</div>
                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-database" >
                    <span class="control-label">Database Objects (Views, Stored Procedures and Functions) Help:</span>
                    <p>You can choose to include comparing database object (Custom Views, Stored Procedures and User-Defined Functions).</p>
                    <p>At this time there are no filters. More features will be coming soon.</p>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->

                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-time">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel10" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Last Modified Date/Time" />
                    </div>
                    <div class="form-group dynamic-options" data-filter-controlid="fltLastModifiedDateTime" data-class="filter-Time">
                        <cms:RadioButtonsControl runat="server" ID="fltLastModifiedDateTime" />
                    </div>
                    <div class="form-group filter-Time" style="display:none">
                        <table class="two-col-textarea">
                            <tr>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel11" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Date/Time from:" /></td>
                            </tr>
                            <tr>
                                <td><cms:DateTimePicker runat="server" ID="dtpLastModifiedFrom" EditTime="false" AllowEmptyValue="true" /></td>
                            </tr>
                        </table>
                        <table class="two-col-textarea">
                            <tr>
                                <td><cms:LocalizedLabel CssClass="textbox-header" ID="LocalizedLabel12" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Date/Time to:" /></td>
                            </tr>
                            <tr>
                                <td><cms:DateTimePicker runat="server" ID="dtpLastModifiedTo" EditTime="false" AllowEmptyValue="true" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-time" >
                    <span class="control-label">Last Modified Date/Time Help:</span>
                    <p>Use this to filter items down based on create/last modified date. This could be vey userful if you know you did development during a particlar time frame.</p>
                    <p class="em">Note: There is a small bug with this. For now, you may get a few more results than you wanted, but you will not get less.</p>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->

                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-content" data-tab-content="tab-other">
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel3" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="kenticocompare.advancedcompareoptions" />
                    </div>
                    <div class="form-group two-col-checks">
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkTrimValues" AutoPostBack="False"/> Trim field values</div>
                    </div>
                    <div class="filter-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="LocalizedLabel19" runat="server" EnableViewState="false" DisplayColon="true" ResourceString="Ignore the following:" />
                    </div>
                    <div class="form-group two-col-checks" id="AdvacnedCompareOptions">
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreNewLineDifferences" AutoPostBack="False"/> New line differences</div>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreWhitespace" AutoPostBack="False" Enabled="false" ToolTip="Not implemented at this time. Coming soon..." /> Whitespace Differences</div>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreMacroSecuritySignature" AutoPostBack="False"/> Macro security signatures</div>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreRefTypeRequired" AutoPostBack="False"/> "reftype=Required"<span class="wide-only"> (in Classes)</span></div>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreFieldTypeCustomUserControl" AutoPostBack="False"/> "fieldtype=CustomUserControl"<span class="wide-only"> (in Classes)</span></div>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreWebPartPropertyIsMacroAttribute" AutoPostBack="False"/> "<i aria-hidden="true" style="font-size: 12px;" class="icon-caret-right"></i> property uses a macro" <span class="wide-only"> (in WebPart Properties)</span></div>
                        <div class="single-check"><asp:CheckBox runat="server" ID="chkIgnoreClassXmlSchemaOrder" AutoPostBack="False" Enabled="false" ToolTip="Not implemented at this time. Coming soon..." /> Field Order for Classes<span class="wide-only"> (ClassXmlSchema)</span></div>
                    </div>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <div class="tab-help" data-tab-content="tab-other" >
                    <span class="control-label">Advanced Options Help:</span>
                    <p>This is a set of global options to "clean up" the values. We reccomend keeping all of these turned on, but feel free to uncheck items.</p>
                    <p class="em">Note: Some of these items are disabled because they have not been implemented yet.</p>
                </div>
                <!-- ------------------------------------------------------------------------------------------------------------------ -->


                <!-- ------------------------------------------------------------------------------------------------------------------ -->
                <!-- ------------------------------------------------------------------------------------------------------------------ -->
            </div>
            <!-- /filter-tab-content -->

            <div class="form-group-buttons">
                <div class="filter-form-buttons-cell-wide">
                <asp:Panel ID="ButtonPanel" runat="server" EnableViewState="true">
                    <asp:HiddenField ID="hdnSaveConfig" ClientIDMode="Static" Value="0" runat="server" />
                    <asp:HiddenField ID="hdnConfigName" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnProfileID" ClientIDMode="Static" Value="" runat="server" />
                    <asp:HiddenField ID="hdnTargetSiteID" ClientIDMode="Static" Value="" EnableViewState="true" runat="server" />
                    <%--<cms:LocalizedButton ClientIDMode="Static" ID="btnSaveAndFilter" runat="server" OnClick="btnFilter_Clicked" ButtonStyle="Primary" EnableViewState="true" ResourceString="kenticocompare.saveandexecute" />--%>
                    <%--<cms:LocalizedButton ClientIDMode="Static" ID="btnFilter" runat="server" OnClick="btnFilter_Clicked" ButtonStyle="Primary" EnableViewState="false" ResourceString="kenticocompare.execute" />--%>
                </asp:Panel>
                </div>
            </div>

        </div>
        <!-- /nav-tabs-container-horizontal -->
    </div>
