<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_AccountSearch" CodeBehind="AccountSearch.aspx.vb" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/Dashboard.ascx" TagName="Dashboard" TagPrefix="uc1" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container borderBlack">
        <div class="row rwPadTB9 rwPadR7 bgTheme padL0">
            <div class="col-md-9 rwPadLR0">
                <div class="col-md-12">
                    <div class="col-md-1 rwPadLR0">
                        <span style="color: white; font-weight: bold; font-size: 16px;">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:PageGlobalResources,FilterByLabel %>" />
                        </span>
                    </div>
                    <telerik:RadAjaxPanel runat="server" ID="apCompanies">
                        <div class="col-md-11" style="left: 15px;">
                            <div class="col-md-4 padL0" runat="server" id="divDistList">
                                <telerik:RadComboBox ID="cboDistributors" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                                    EmptyMessage="<%$Resources:PageGlobalResources,AllDistributorsText %>" MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                            </div>
                            <div class="col-md-4" runat="server" id="divSProList">
                                <telerik:RadComboBox ID="cboServiceProvs" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                                    EmptyMessage="<%$Resources:PageGlobalResources,AllProvidersText %>" MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                            </div>
                            <div class="col-md-4" runat="server" id="divInstList">
                                <telerik:RadComboBox ID="cboInstallers" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md" AutoPostBack="true" Filter="Contains"
                                    EmptyMessage="<%$Resources:PageGlobalResources,AllInstallersText %>" MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                            </div>
                        </div>
                    </telerik:RadAjaxPanel>
                </div>
                <div class="col-md-12" runat="server" id="divDevRange" style="margin-top: 10px;">
                    <div class="col-md-1 rwPadLR0" style="width: 100px">
                        <span style="color: white; font-weight: bold; font-size: 16px;">
                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:PageGlobalResources,SearchLabel %>" />
                        </span>
                    </div>
                    <telerik:RadAjaxPanel runat="server" ID="apUnitRange">
                        <div class="col-md-7 padL0">
                            <div class="col-md-6 rwPadLR0 txtFromUnitID padL0">
                                <telerik:RadTextBox ID="txtFromUnitID" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,FromUnitEmptyText %>" />
                                <asp:RegularExpressionValidator ID="revFromUnitID" runat="server" ErrorMessage="<%$Resources:PageGlobalResources,ValidSensorIDMsg %>" ValidationExpression="^[0-9A-Fa-f]{1,6}$" ControlToValidate="txtFromUnitID" ForeColor="red" Display="dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-6 txtToUnitID">
                                <telerik:RadTextBox ID="txtToUnitID" runat="server" EmptyMessage="<%$Resources:PageGlobalResources,ToUnitEmptyText %>" />
                                <asp:RegularExpressionValidator ID="revToUnitID" runat="server" ErrorMessage="<%$Resources:PageGlobalResources,ValidSensorIDMsg %>" ValidationExpression="^[0-9A-Fa-f]{1,6}$" ControlToValidate="txtToUnitID" ForeColor="red" Display="dynamic"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="col-md-12 margin-top10">
                            <script type="text/javascript">
                                function valIDRange(source, arguments) {

                                    var toVal = $('#' + $(source).prop("toTxtID")).val();
                                    toVal = ('000000' + toVal).slice(-6);

                                    var fromVal = $('#' + $(source).prop("fromTxtID")).val();
                                    fromVal = ('000000' + fromVal).slice(-6);

                                    arguments.IsValid = (toVal >= fromVal);
                                }
                            </script>
                            <asp:CustomValidator ID="cvToID" runat="server" ErrorMessage="<%$Resources:PageGlobalResources,ValidSensorRangeMsg %>" ClientValidationFunction="valIDRange" ControlToValidate="txtToUnitID" ForeColor="red" Display="Dynamic"></asp:CustomValidator>

                        </div>
                    </telerik:RadAjaxPanel>
                </div>
            </div>
            <div class="col-md-3 padL0">
                <div class="col-md-6 rwPadLR0">
                    <div class="col-md-12 rwPadLR0" style="padding-bottom: 9px;">
                        <asp:Button ID="btnClear" runat="server" CssClass="btn btnimgstyle btnClearImage" CausesValidation="false" OnClientClick="Page_BlockSubmit = false;" Text="<%$Resources:PageGlobalResources,ClearButton %>" />
                    </div>
                    <div class="col-md-12 rwPadLR0">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btnimgstyle btnSearchImage validateAllNoMsg" Text="<%$Resources:PageGlobalResources,SearchButton %>" />
                    </div>
                </div>
                <div class="col-md-5 padL0">
                    <div class="row">
                        <div class="col-md-12 padL0">
                            <asp:Button ID="btnExport" runat="server" CssClass="btn btnAccountexport" Text="<%$Resources:PageGlobalResources,ExportDataButton %>" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <uc1:Dashboard ID="ucDashboard" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="lpGridLoading" />
        <telerik:RadAjaxPanel runat="server" ID="apGridUpdate">
            <div id="grdAccount" class="row">
                <telerik:RadGrid RenderMode="Lightweight" ID="rgAccounts" runat="server" AllowPaging="true" AllowSorting="true" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" AutoGenerateColumns="false" CellPadding="5" Font-Overline="false" ForeColor="#333333" PageSize="20" BorderStyle="None"
                    BorderWidth="0" GridLines="None" Font-Size="Small" CssClass="col-md-12 hideHeaderBar" AllowFilteringByColumn="true"
                    ShowHeaderWhenEmpty="true" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>"
                    MasterTableView-DataKeyNames="IntaAccountID,IntaPropertyID">
                    <GroupingSettings CaseSensitive="false" />
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
                    <HeaderStyle CssClass="bgTheme" BackColor="#333333" Font-Bold="False" ForeColor="white" Height="0" Width="200" />
                    <FilterItemStyle CssClass="bgTheme" />
                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto">
                        <Columns>
                            <telerik:GridBoundColumn DataField="IntaAccountID" UniqueName="IntaAccountID" AllowFiltering="true" HeaderText="<%$Resources:PageGlobalResources,AccountHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchAccountID" runat="server" Width="85px" EmptyMessage="<%$Resources:PageGlobalResources,AccountHeader %>">
                                        <ClientEvents OnValueChanged="AccountIDChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function AccountIDChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaAccountID", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="PropertyStatusID" DataField="PropertyStatusID" HeaderText="<%$Resources:PageGlobalResources,StatusHeader %>">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cboSearchStatus" RenderMode="Lightweight" runat="server" Width="100px" EmptyMessage="<%$Resources:PageGlobalResources,StatusHeader %>" CssClass="rwPadLR5"
                                        OnClientSelectedIndexChanged="PropertyStatusIDChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress"
                                        OnClientBlur="PropertyStatusIDBlurred">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,NotSet %>" Value="0" />
                                            <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Active %>" Value="1" />
                                            <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Suspended %>" Value="2" />
                                            <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Test %>" Value="3" />
                                            <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Disabled %>" Value="4" />
                                            <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Deleted %>" Value="5" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="radscriptB2" runat="server">
                                        <script type="text/javascript">
                                            function PropertyStatusIDChanged(sender, args) {
                                                FilterStatusID(args.get_item().get_value());
                                            }
                                            function PropertyStatusIDBlurred(sender, args) {
                                                RadComboBlurred(sender, args);
                                                FilterStatusID(sender.get_value());
                                            }
                                            function FilterStatusID(filterValue) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                if (filterValue == null || filterValue == "") {
                                                    tableView.filter("PropertyStatusID", filterValue, "NoFilter");
                                                }
                                                else {
                                                    tableView.filter("PropertyStatusID", filterValue, "EqualTo");
                                                }
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPropertyStatus" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="IntaSurname" UniqueName="IntaSurname" HeaderText="<%$Resources:PageGlobalResources,LastNameHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchLastName" runat="server" Width="90px" EmptyMessage="<%$Resources:PageGlobalResources,LastNameHeader %>">
                                        <ClientEvents OnValueChanged="SurnameChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function SurnameChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaSurname", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="IntaAddress1" UniqueName="IntaAddress1" HeaderText="<%$Resources:PageGlobalResources,AddressHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchAddress1" runat="server" Width="130px" EmptyMessage="<%$Resources:PageGlobalResources,AddressHeader %>">
                                        <ClientEvents OnValueChanged="AddressChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function AddressChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaAddress1", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="IntaPostcode" UniqueName="IntaPostcode" HeaderText="<%$Resources:PageGlobalResources,PostCodeHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchPostcode" runat="server" Width="90px" EmptyMessage="<%$Resources:PageGlobalResources,PostCodeHeader %>">
                                        <ClientEvents OnValueChanged="PostcodeChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function PostcodeChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaPostcode", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="IntaMACAddress" UniqueName="IntaMACAddress" HeaderStyle-Width="11em" HeaderText="<%$Resources:PageGlobalResources,MACAddressHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchMacAddress" runat="server" Width="10.5em" EmptyMessage="<%$Resources:PageGlobalResources,MACAddressHeader %>">
                                        <ClientEvents OnValueChanged="MacAddressChanged" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function MacAddressChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaMACAddress", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMacAddress" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="IntaLastSystemTestDateTime" DataField="IntaLastSystemTestDateTime" DataType="System.DateTime" HeaderStyle-Width="8.5em" HeaderText="<%$Resources:PageGlobalResources,LastTest %>">
                                <FilterTemplate>
                                    <telerik:RadDatePicker RenderMode="Lightweight" runat="server" ID="dpSystemTestDate" Width="8.5em" DateInput-EmptyMessage="<%$Resources:PageGlobalResources,TestDue %>"
                                        ClientEvents-OnDateSelected="TestDateSelected" />
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function TestDateSelected(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");

                                                var fromDate = sender.get_selectedDate();

                                                if (fromDate) {
                                                    fromDate.setDate(fromDate.getDate() - <%# mintSystemTestDueDays%>);
                                                    var dateInput = sender.get_dateInput();
                                                    var toDate = new Date(fromDate);
                                                    toDate.setDate(toDate.getDate() + 1);

                                                    var formattedFrom = dateInput.get_dateFormatInfo().FormatDate(fromDate, dateInput.get_displayDateFormat());
                                                    var formattedTo = dateInput.get_dateFormatInfo().FormatDate(toDate, dateInput.get_displayDateFormat());

                                                    tableView.filter("IntaLastSystemTestDateTime", formattedFrom + " " + formattedTo, "Between");
                                                }
                                                else {
                                                    tableView.filter("IntaLastSystemTestDateTime", "", "NoFilter");
                                                }
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSystemTestDueDate" />
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="LicenseRegisteredDate" UniqueName="LicenseRegisteredDate" DataType="System.DateTime" DataFormatString="{0:d}" HeaderStyle-Width="12.5em" HeaderText="<%$Resources:PageGlobalResources,LicenceRegistered %>">
                                <FilterTemplate>
                                    <telerik:RadDatePicker RenderMode="Lightweight" runat="server" ID="dpLicensedDate" CssClass="LicensedDateWidth" Width="8.5em" DateInput-EmptyMessage="<%$Resources:PageGlobalResources,LicenceRegistered %>"
                                        ClientEvents-OnDateSelected="LicensedDateSelected" />
                                    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                        <script type="text/javascript">
                                            function LicensedDateSelected(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");

                                                var fromDate = sender.get_selectedDate();

                                                if (fromDate) {
                                                    var dateInput = sender.get_dateInput();
                                                    var toDate = new Date(fromDate);
                                                    toDate.setDate(toDate.getDate() + 1);

                                                    var formattedFrom = dateInput.get_dateFormatInfo().FormatDate(fromDate, dateInput.get_displayDateFormat());
                                                    var formattedTo = dateInput.get_dateFormatInfo().FormatDate(toDate, dateInput.get_displayDateFormat());

                                                    tableView.filter("LicenseRegisteredDate", formattedFrom + " " + formattedTo, "Between");
                                                }
                                                else {
                                                    tableView.filter("LicenseRegisteredDate", "", "NoFilter");
                                                }
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../common/img/view.png" UniqueName="Edit" CommandName="EditAccount">
                            </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../common/img/notes.png" UniqueName="Notes" CommandName="AccountNotes">
                            </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../common/img/ticket.png" UniqueName="AddTicket" CommandName="AccountTickets">
                            </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </telerik:RadAjaxPanel>
    </div>
</asp:Content>
