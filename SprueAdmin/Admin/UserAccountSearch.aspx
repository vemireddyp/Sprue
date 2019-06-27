<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_UserAccountSearch" CodeBehind="UserAccountSearch.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <%-- Account Section Start --%>
    <div id="divAccountDetails">
        <div class="col-md-12 rwPadTB9 rwPadR7 bgTheme" style="margin-top: 10px; color: white; font-weight: bold;">
            <div class="nav navbar-nav col-md-4 col-md-4-left">
                <asp:Label ID="lblAccounts" runat="server" Text="<%$Resources:Accounts %>"></asp:Label>
            </div>
        </div>

        <telerik:RadAjaxPanel ID="radAjaxPanelSearch" runat="server">

            <div class="col-md-12 rwPadLR0 borderBlack" style="background-color: #FFFFFF; padding: 10px;">
                <div class="col-md-12  margin-top15">
                    <div class="col-md-6">
                        <div class="col-md-12 ">
                            <div class="col-md-4">
                                <asp:Label ID="lblAccountID" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control inputhgt " ID="txtAccountID" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12  rwPadTB15">
                            <div class="col-md-4">
                                <asp:Label ID="lblAccountType" runat="server" Text="<%$Resources:PageGlobalResources,AccountTypeLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-control inputhgt  col-md-12">
                                    <asp:ListItem Value="" runat="server" Text="<%$Resources:PageGlobalResources,AllSelectItemText %>" />
                                    <asp:ListItem Value="3" runat="server" Text="<%$Resources:PageGlobalResources,DistributorText %>"></asp:ListItem>
                                    <asp:ListItem Value="4" runat="server" Text="<%$Resources:PageGlobalResources,ServiceProText %>"></asp:ListItem>
                                    <asp:ListItem Value="E" runat="server" Text="<%$Resources:PageGlobalResources,EndUserText %>"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-12 ">
                            <div class="col-md-4">
                                <asp:Label ID="lblCoName" runat="server" Text="<%$Resources:PageGlobalResources,CoNameLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control inputhgt " ID="txtCoName" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12  rwPadTB15">
                            <div class="col-md-4">
                                <asp:Label ID="lblStatus" runat="server" Text="<%$Resources:PageGlobalResources,StatusLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control inputhgt">
                                    <asp:ListItem Text="<%$Resources:PageGlobalResources,NoFilterText %>" Value="" />
                                    <asp:ListItem Text="<%$Resources:StatusResources,ActiveList %>" Value="Active" />
                                    <asp:ListItem Text="<%$Resources:StatusResources,PendingList %>" Value="Pending" />
                                    <asp:ListItem Text="<%$Resources:StatusResources,DisabledList %>" Value="Disabled" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="col-md-12 ">
                            <div class="col-md-4">
                                <asp:Label ID="lblFirstName" runat="server" Text="<%$Resources:PageGlobalResources,FirstNameLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control inputhgt " ID="txtFirstName" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12  rwPadTB15">
                            <div class="col-md-4">
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:PageGlobalResources,LastNameLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control inputhgt " ID="txtLastName" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12 ">
                            <div class="col-md-4">
                                <asp:Label ID="lblEmail" runat="server" Text="<%$Resources:PageGlobalResources,EmailLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control inputhgt " ID="txtEmail" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div class="pull-left marL30">
                            <asp:Button ID="btnCreateAccount" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:PageGlobalResources,CreateAccountButton %>" />
                        </div>
                        <div class="pull-right marR30">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:PageGlobalResources,SearchButton %>" />
                        </div>
                         <div class="pull-right marR30">
                            <asp:Button ID="btnClear" runat="server" CssClass="btn  btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,ClearButton %>" CausesValidation="false" />
                         </div>
                    </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
        <%-- Account Section End --%>

        <%-- Company details Section Start --%>
        <telerik:RadAjaxPanel ID="radAjaxPanelCompanies" runat="server">
            <div id="divPropCompanies" runat="server">
                <div class="col-md-12 rwPadTB9 rwPadR7 bgTheme" style="margin-top: 10px; margin-bottom: 10px; color: white; font-weight: bold;">
                    <div class="nav navbar-nav col-md-4 col-md-4-left">

                        <asp:Label ID="lblHub" runat="server" Text="<%$Resources:CompanyDetsLegend %>"></asp:Label>
                    </div>
                    <div class="pull-right">
                        <asp:Button ID="btnCompanydetailsOpen" runat="server" OnClick="btnCompanydetailsOpen_Click" CssClass="btn btn-default btnEdit btnTB3 pull-right" Text="<%$Resources:Open %>" />
                    </div>
                </div>

                <div id="divCompanies" runat="server">
                    <div class="col-md-12 rwPadLR0 borderBlack" style="top: -2px;">
                        <telerik:RadAjaxLoadingPanel runat="server" ID="lpCoGridLoading" />
                        <telerik:RadGrid RenderMode="Lightweight" ID="rgCompanies" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" CellPadding="5"
                            Font-Overline="false" ForeColor="#333333" BorderStyle="None" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>"
                            BorderWidth="0" GridLines="None" Font-Size="Small" CssClass="col-md-12 rwPadLR0 hideHeaderBar" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false"
                            ShowHeaderWhenEmpty="true" Width="100%" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>" PageSize="20">
                            <FooterStyle BackColor="#333333" Font-Bold="False" ForeColor="White" />
                            <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                            <AlternatingItemStyle BackColor="#F5F5F5" ForeColor="Black" />
                            <ClientSettings AllowKeyboardNavigation="true">
                                <KeyboardNavigationSettings AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" />
                            </ClientSettings>
                            <PagerStyle HorizontalAlign="Center" CssClass="dataPager" PagerTextFormat="<%$Resources:PageGlobalResources,GridPagerTextFormat %>"
                                PageSizeLabelText="<%$Resources:PageGlobalResources,GridPageSizeText %>" NextPageToolTip="<%$Resources:PageGlobalResources,GridNextPageTooltip %>"
                                PrevPageToolTip="<%$Resources:PageGlobalResources,GridPreviousPageTooltip %>" LastPageToolTip="<%$Resources:PageGlobalResources,GridLastPageTooltip %>"
                                FirstPageToolTip="<%$Resources:PageGlobalResources,GridFirstPageTooltip %>" NextPagesToolTip="<%$Resources:PageGlobalResources,GridNextPagesTooltip %>"
                                PrevPagesToolTip="<%$Resources:PageGlobalResources,GridPreviousPagesTooltip %>" />
                            <FilterItemStyle CssClass="bgTheme" />
                            <MasterTableView AutoGenerateColumns="false" TableLayout="fixed" DataKeyNames="MasterCoID">
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="CompanyTypeID" HeaderStyle-Width="12em">
                                        <FilterTemplate>
                                            <telerik:RadComboBox RenderMode="Lightweight" ID="cboSearchAccountType" runat="server" Width="12em" EmptyMessage="<%$Resources:PageGlobalResources,AccountTypeHeader %>" CssClass="rwPadLR5"
                                                OnClientSelectedIndexChanged="AccountTypeChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true"
                                                OnClientBlur="AccountTypeBlurred" DataTextField="CompanyType" DataValueField="CompanyTypeID" OnClientKeyPressing="RadComboKeyPress">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,ApplicationOwnerText %>" Value="2" />
                                                    <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,DistributorText %>" Value="3" />
                                                    <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,ServiceProText %>" Value="4" />

                                                </Items>
                                            </telerik:RadComboBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock7" runat="server">
                                                <script type="text/javascript">
                                                    function AccountTypeChanged(sender, args) {
                                                        FilterAccountType(args.get_item().get_value());
                                                    }
                                                    function AccountTypeBlurred(sender, args) {
                                                        RadComboBlurred(sender, args);
                                                        FilterAccountType(sender.get_value());
                                                    }
                                                    function FilterAccountType(filterValue) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        if (filterValue != null) {
                                                            tableView.filter("CompanyTypeID", filterValue, "NoFilter");
                                                        }
                                                        //else {
                                                        //  tableView.filter("CompanyTypeID", filterValue, "EqualTo");
                                                        //}
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                        <ItemTemplate>
                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="lblAccountType" />
                                            </div>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Name" UniqueName="Name" AllowFiltering="true" HeaderStyle-Width="13em">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtCompanyName" runat="server" RenderMode="Lightweight" Width="12.5em" EmptyMessage="<%$Resources:PageGlobalResources,CoNameHeader %>">
                                                <ClientEvents OnValueChanged="CompanyNameChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                <script type="text/javascript">
                                                    function CompanyNameChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("Name", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                        $('#<%=btnSearch.ClientID%>').focus();
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="StatusID" UniqueName="StatusID" HeaderStyle-Width="8em">
                                        <FilterTemplate>
                                            <telerik:RadComboBox ID="cboSearchCoStatus" runat="server" RenderMode="Lightweight" EmptyMessage="<%$Resources:PageGlobalResources,StatusHeader %>" CssClass="rwPadR15" Width="7.5em"
                                                OnClientSelectedIndexChanged="StatusIDChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true"
                                                OnClientBlur="StatusIDChangedBlurred" OnClientKeyPressing="RadComboKeyPress">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="<%$Resources:StatusResources,ActiveList %>" Value="1" />
                                                    <telerik:RadComboBoxItem Text="<%$Resources:StatusResources,DisabledList %>" Value="2" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <telerik:RadScriptBlock ID="radscriptB2" runat="server">
                                                <script type="text/javascript">
                                                    function StatusIDChanged(sender, args) {
                                                        FilterStatusID(args.get_item().get_value());
                                                    }
                                                    function StatusIDChangedBlurred(sender, args) {
                                                        RadComboBlurred(sender, args);
                                                        FilterStatusID(sender.get_value());
                                                    }
                                                    function FilterStatusID(filterValue) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        if (filterValue == null || filterValue == "") {
                                                            tableView.filter("StatusID", filterValue, "NoFilter");
                                                        }
                                                        else {
                                                            tableView.filter("StatusID", filterValue, "EqualTo");
                                                        }
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="ParentName" AllowFiltering="true" HeaderStyle-Width="14em">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtParentName" runat="server" RenderMode="Lightweight" Width="13.8em" EmptyMessage="<%$Resources:PageGlobalResources,ParentNameHeader %>">
                                                <ClientEvents OnValueChanged="ParentNameChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock runat="server">
                                                <script type="text/javascript">
                                                    function ParentNameChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("ParentName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FirstName" AllowFiltering="true" HeaderStyle-Width="11em">
                                        <FilterTemplate>
                                            <telerik:RadTextBox runat="server" ID="txtSearchCoFirstname" RenderMode="Lightweight" Width="10.5em" EmptyMessage="<%$Resources:PageGlobalResources,FirstNameHeader %>">
                                                <ClientEvents OnValueChanged="CoFirstNameChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock runat="server">
                                                <script type="text/javascript">
                                                    function CoFirstNameChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("FirstName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>

                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LastName" AllowFiltering="true" HeaderStyle-Width="11em">
                                        <FilterTemplate>
                                            <telerik:RadTextBox runat="server" ID="txtSearchCoLastname" RenderMode="Lightweight" Width="10.5em" EmptyMessage="<%$Resources:PageGlobalResources,LastNameHeader %>">
                                                <ClientEvents OnValueChanged="CoLastNameChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock runat="server">
                                                <script type="text/javascript">
                                                    function CoLastNameChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("LastName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>

                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Email" UniqueName="Email" HeaderStyle-Width="14em">
                                        <HeaderStyle Width="14em" />
                                        <FilterTemplate>
                                            <telerik:RadTextBox runat="server" ID="txtSearchCoEmail" RenderMode="Lightweight" EmptyMessage="<%$Resources:PageGlobalResources,EmailHeader %>" Width="13.6em">
                                                <ClientEvents OnValueChanged="CoEmailChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock runat="server">
                                                <script type="text/javascript">
                                                    function CoEmailChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("Email", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>

                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn ButtonType="LinkButton" Text="<%$Resources:PageGlobalResources,EditButtonText %>" CommandName="EditAccount" HeaderStyle-Width="5em">
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>

                </div>
            </div>
        </telerik:RadAjaxPanel>

        <%-- Company details Section End --%>

        <%-- End Users Details Section Start --%>
        <telerik:RadAjaxPanel ID="radAjaxPanelEndUsers" runat="server">
            <div id="divPropEndUsers" runat="server">
                <div class="col-md-12 rwPadTB9 rwPadR7 bgTheme" style="margin-top: 10px; margin-bottom: 10px; color: white; font-weight: bold;">
                    <div class="nav navbar-nav col-md-4 col-md-4-left">
                        <span class="mydevices-headertext">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:PageGlobalResources,EndUsersDetsLegend %>"></asp:Label></span>
                    </div>
                    <div class="pull-right">
                        <asp:Button ID="btnEndUsersDetailsOpen" runat="server" OnClick="btnEndUsersDetailsOpen_Click" CssClass="btn btn-default btnEdit btnTB3 pull-right" Text="<%$Resources:Open %>" />
                    </div>
                </div>
                <div id="divEndUsers" runat="server">
                    <div class=" col-md-12 rwPadLR0 borderBlack" id="Div3" style="top: -1px;">
                        <telerik:RadAjaxLoadingPanel runat="server" ID="lpUserGridLoading" />

                        <telerik:RadGrid RenderMode="Lightweight" ID="rgEndUsers" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" CellPadding="5"
                            Font-Overline="false" ForeColor="#333333" BorderStyle="None" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>"
                            BorderWidth="0" GridLines="None" Font-Size="Small" CssClass="col-md-12 rwPadLR0 hideHeaderBar" AllowFilteringByColumn="true" GroupingSettings-CaseSensitive="false"
                            ShowHeaderWhenEmpty="true" Width="100%" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>" PageSize="20">
                            <FooterStyle BackColor="#333333" Font-Bold="False" ForeColor="White" />
                            <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                            <AlternatingItemStyle BackColor="#F5F5F5" ForeColor="Black" />
                            <ClientSettings AllowKeyboardNavigation="true">
                                <KeyboardNavigationSettings AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" />
                            </ClientSettings>
                            <PagerStyle HorizontalAlign="Center" CssClass="dataPager" PagerTextFormat="<%$Resources:PageGlobalResources,GridPagerTextFormat %>"
                                PageSizeLabelText="<%$Resources:PageGlobalResources,GridPageSizeText %>" NextPageToolTip="<%$Resources:PageGlobalResources,GridNextPageTooltip %>"
                                PrevPageToolTip="<%$Resources:PageGlobalResources,GridPreviousPageTooltip %>" LastPageToolTip="<%$Resources:PageGlobalResources,GridLastPageTooltip %>"
                                FirstPageToolTip="<%$Resources:PageGlobalResources,GridFirstPageTooltip %>" NextPagesToolTip="<%$Resources:PageGlobalResources,GridNextPagesTooltip %>"
                                PrevPagesToolTip="<%$Resources:PageGlobalResources,GridPreviousPagesTooltip %>" />
                            <FilterItemStyle CssClass="bgTheme" />
                            <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="IntaAccountID">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="IntaAccountID" UniqueName="IntaAccountID" AllowFiltering="true" HeaderStyle-Width="10em">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtIntaAccountID" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,AccountHeader %>" Width="8em">
                                                <ClientEvents OnValueChanged="IntaAccountIDChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                <script type="text/javascript">
                                                    function IntaAccountIDChanged(sender, args) {
                                                        var tableView = $find("<%# TryCast(Container,GridItem).OwnerTableView.ClientID %>");
                                                        tableView.filter("IntaAccountID", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);

                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UserName" AllowFiltering="true">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtSearchUsername" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,UsernameHeader %>" Width="11em">
                                                <ClientEvents OnValueChanged="UsernameChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                                <script type="text/javascript">
                                                    function UsernameChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("UserName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="IntaAddress1" AllowFiltering="true">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtSearchAddress" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,AddressHeader %>" Width="11em">
                                                <ClientEvents OnValueChanged="AddressChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                                <script type="text/javascript">
                                                    function AddressChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("IntaAddress1", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="IntaFirstName" AllowFiltering="true">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtSearchFirstName" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,FirstNameHeader %>" Width="11em">
                                                <ClientEvents OnValueChanged="FirstNameChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
                                                <script type="text/javascript">
                                                    function FirstNameChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("IntaFirstName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="IntaSurname" AllowFiltering="true">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtSearchLastName" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,LastNameHeader %>" Width="11em">
                                                <ClientEvents OnValueChanged="LastNameChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock5" runat="server">
                                                <script type="text/javascript">
                                                    function LastNameChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("IntaSurname", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="IntaEmail" AllowFiltering="true">
                                        <FilterTemplate>
                                            <telerik:RadTextBox ID="txtSearchEmail" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,EmailHeader %>" Width="15em">
                                                <ClientEvents OnValueChanged="EmailChanged" />
                                            </telerik:RadTextBox>
                                            <telerik:RadScriptBlock ID="RadScriptBlock6" runat="server">
                                                <script type="text/javascript">
                                                    function EmailChanged(sender, args) {
                                                        var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                        tableView.filter("IntaEmail", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn ButtonType="LinkButton" Text="<%$Resources:PageGlobalResources,EditButtonText %>" CommandName="EditAccount" HeaderStyle-Width="5em">
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </div>
</asp:Content>
