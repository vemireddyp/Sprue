<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_LoginSearch" CodeBehind="LoginSearch.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <div class=" col-md-12  bgwhite borderBlack">
        <div class="row rwPadTB9 bgTheme txtcolor margin-top15">
            <div class="col-md-5">
                <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="<%$Resources:Accounts %>" />
            </div>
        </div>
        <telerik:RadAjaxPanel runat="server" ID="apLSSearch">
            <div class="col-sm-12 rwPadLR0">
                <div class="col-sm-12 lblsubHeader">
                    <asp:Label ID="lblSearch" runat="server" Text="<%$Resources:PageGlobalResources,SelectSearchFieldsPrompt %>" CssClass="col-sm-12 text-left"></asp:Label>
                </div>

                <div class="col-sm-12 form-group rwPadLR0 txtHeight">
                    <div class="col-sm-6">
                        <div class="col-sm-12">
                            <asp:Label ID="lblUsername" runat="server" Text="<%$Resources:PageGlobalResources,UsernameLabel %>" CssClass="control-label col-sm-3 text-left"></asp:Label>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txtUsername" Width="100%" runat="server" CssClass="form-control inputhgt retCtl"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="col-sm-12 margin-top15">
                            <asp:Label ID="lblFirstname" runat="server" Text="<%$Resources:PageGlobalResources,FirstNameLabel %>" CssClass="control-label col-sm-3 text-left"></asp:Label>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txtFirstName" Width="100%" runat="server" CssClass="form-control inputhgt retCtl"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="col-sm-12 margin-top15">
                            <asp:Label ID="lblLastName" runat="server" Text="<%$Resources:PageGlobalResources,LastNameLabel %>" CssClass="control-label col-sm-3 text-left"></asp:Label>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txtLastName" Width="100%" runat="server" CssClass="form-control inputhgt retCtl"></telerik:RadTextBox>
                            </div>
                        </div>

                    </div>
                    <div class="col-sm-6">
                        <div class="col-sm-12 margin-left-15">
                            <div class="col-md-4">
                                <asp:Label ID="lblEmail" runat="server" Text="<%$Resources:PageGlobalResources,EmailLabel %>"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txtEmail" Width="100%" runat="server" CssClass="form-control inputhgt retCtl"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="col-sm-12 margin-top15 margin-left-15">
                            <div class="col-sm-4">
                                <asp:Label ID="lblPermLevel" runat="server" Text="<%$Resources:PageGlobalResources,PermissionLevelLabel  %>" CssClass="padR0"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cboPermLevel" RenderMode="Lightweight" Width="100%" runat="server" CssClass="retCtl" Filter="Contains"
                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="OnPermKeyPressing" OnClientBlur="RadComboBlurred" />
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function OnPermKeyPressing(sender, args) {
                                            RadComboKeyPress(sender, args);
                                            var e = args.get_domEvent();
                                            var id = "";

                                            if (e.keyCode == 13) {
                                                sender._raiseClientBlur(e);
                                                var nextCombo = $find($("[id$='cboStatus']").prop('id'));

                                                if (nextCombo) {
                                                    nextCombo.get_inputDomElement().focus();
                                                }
                                            }
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </div>
                        </div>
                        <div class="col-sm-12 margin-top15 margin-left-15">
                            <div class="col-sm-4">
                                <asp:Label ID="lblStatus" runat="server" Text="<%$Resources:PageGlobalResources,StatusLabel  %>"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cboStatus" RenderMode="Lightweight" runat="server" Width="100%" CssClass="retCtl" Filter="Contains"
                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="OnStatusKeyPressing" OnClientBlur="RadComboBlurred" />
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function OnStatusKeyPressing(sender, args) {
                                            RadComboKeyPress(sender, args);
                                            var e = args.get_domEvent();

                                            if (e.keyCode == 13) {
                                                sender._raiseClientBlur(e);
                                                $("[id$='btnSearch']").focus();
                                            }
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 rwPadTB15">
                    <asp:Button ID="btnAddUser" runat="server" class="btn btn-warning btnLogOff pull-left marL30 retCtl" Text="<%$Resources:PageGlobalResources,AddNewUserButton %>" />
                    <asp:Button ID="btnSearch" runat="server" class="btn btn-warning btnLogOff pull-right marR15" Text="<%$Resources:PageGlobalResources,SearchButton %>" />
                     <div class="pull-right marR30">
                            <asp:Button ID="btnClear" runat="server" CssClass="btn  btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,ClearButton %>" CausesValidation="false" />
                         </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
        <div class="col-sm-12" id="grdLogins">
            <telerik:RadAjaxPanel runat="server" ID="apLSGrid">
                <telerik:RadGrid RenderMode="Lightweight" ID="rgUsers" runat="server" AllowPaging="true" AutoGenerateColumns="false" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" CssClass="col-md-12 hideHeaderBar rwPadLR0" MasterTableView-ShowHeadersWhenNoRecords="true"
                    GroupingSettings-CaseSensitive="false" AllowFilteringByColumn="true" FilterItemStyle-CssClass="bgTheme" BorderStyle="None"
                    BorderWidth="0" GridLines="none" PageSize="20">
                    <ClientSettings AllowKeyboardNavigation="true">
                        <KeyboardNavigationSettings AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" />
                    </ClientSettings>
                    <PagerStyle HorizontalAlign="Center" CssClass="dataPager" PagerTextFormat="<%$Resources:PageGlobalResources,GridPagerTextFormat %>"
                        PageSizeLabelText="<%$Resources:PageGlobalResources,GridPageSizeText %>" NextPageToolTip="<%$Resources:PageGlobalResources,GridNextPageTooltip %>"
                        PrevPageToolTip="<%$Resources:PageGlobalResources,GridPreviousPageTooltip %>" LastPageToolTip="<%$Resources:PageGlobalResources,GridLastPageTooltip %>"
                        FirstPageToolTip="<%$Resources:PageGlobalResources,GridFirstPageTooltip %>" NextPagesToolTip="<%$Resources:PageGlobalResources,GridNextPagesTooltip %>"
                        PrevPagesToolTip="<%$Resources:PageGlobalResources,GridPreviousPagesTooltip %>" />

                    <MasterTableView AllowFilteringByColumn="true" DataKeyNames="UserID" AdditionalDataFieldNames="UserID">
                        <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                        <AlternatingItemStyle BackColor="#F5F5F5" ForeColor="Black" />
                        <FilterItemStyle CssClass="bgTheme" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="Username" UniqueName="Username">
                                <FilterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtSearchUsername" EmptyMessage="<%$Resources:PageGlobalResources,UsernameHeader %>">
                                        <ClientEvents OnValueChanged="UsernameChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function UsernameChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("Username", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="PermLevel" UniqueName="PermLevel">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cboSearchPermLevel" RenderMode="Lightweight" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,PermissionLevelHeader %>" CssClass="rwPadLR5"
                                        OnClientSelectedIndexChanged="PermLevelChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress"
                                        OnClientBlur="PermLevelBlurred">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function PermLevelChanged(sender, args) {
                                                FilterPermLevel(args.get_item().get_value());
                                            }
                                            function PermLevelBlurred(sender, args) {
                                                RadComboBlurred(sender, args);
                                                FilterPermLevel(sender.get_value());
                                            }
                                            function FilterPermLevel(filterValue) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                if (filterValue == null || filterValue == "") {
                                                    tableView.filter("PermLevel", filterValue, "NoFilter");
                                                }
                                                else {
                                                    tableView.filter("PermLevel", filterValue, "EqualTo");
                                                }
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                <ItemTemplate>
                                    <div class="col-md-12">
                                        <asp:Label runat="server" ID="lblPermissionLevel" />
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Firstname" UniqueName="Firstname">
                                <FilterTemplate>
                                    <telerik:RadTextBox runat="server" ID="txtSearchFirstname" Width="7em" EmptyMessage="<%$Resources:PageGlobalResources,FirstNameHeader %>">
                                        <ClientEvents OnValueChanged="FirstNameChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function FirstNameChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("Firstname", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>

                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Lastname" UniqueName="Lastname">
                                <FilterTemplate>
                                    <telerik:RadTextBox runat="server" ID="txtSearchLastname" Width="7em" EmptyMessage="<%$Resources:PageGlobalResources,LastNameHeader %>">
                                        <ClientEvents OnValueChanged="LastNameChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function LastNameChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("Lastname", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>

                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Email" UniqueName="Email">
                                <HeaderStyle Width="10em" />
                                <FilterTemplate>
                                    <telerik:RadTextBox runat="server" ID="txtSearchEmail" EmptyMessage="<%$Resources:PageGlobalResources,EmailHeader %>">
                                        <ClientEvents OnValueChanged="EmailChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function EmailChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("Email", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>

                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn DataField="UserStatusID" UniqueName="UserStatusID">
                                <HeaderStyle Width="5em" />
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cboSearchUserStatus" RenderMode="Lightweight" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,StatusHeader %>" CssClass="rwPadLR5"
                                        OnClientSelectedIndexChanged="UserStatusChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress"
                                        OnClientBlur="UserStatusBlurred">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function UserStatusChanged(sender, args) {
                                                FilterUserStatus(args.get_item().get_value());
                                            }
                                            function UserStatusBlurred(sender, args) {
                                                RadComboBlurred(sender, args);
                                                FilterUserStatus(sender.get_value());
                                            }
                                            function FilterUserStatus(filterValue) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                if (filterValue == null || filterValue == "") {
                                                    tableView.filter("UserStatusID", filterValue, "NoFilter");
                                                }
                                                else {
                                                    tableView.filter("UserStatusID", filterValue, "EqualTo");
                                                }
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                <ItemTemplate>
                                    <div class="col-md-12">
                                        <asp:Label runat="server" ID="lblUserStatus" />
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Select" Text="<%$Resources:PageGlobalResources,SelectButtonText %>" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
        </div>

    </div>
</asp:Content>
