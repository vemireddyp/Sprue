<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_AuditLogs" CodeBehind="AuditLogs.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">

    <div class="col-md-12 margin-top15 borderBlack">
        <telerik:RadAjaxPanel runat="server">
            <telerik:RadGrid ID="rgAuditLogs" runat="server" RenderMode="Lightweight" AllowPaging="true" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" AllowFilteringByColumn="true" CssClass="row hideHeaderBar rwPadLR0 itemstyle" AutoGenerateColumns="false"
                MasterTableView-ShowHeadersWhenNoRecords="true">
                <ClientSettings AllowKeyboardNavigation="true">
                    <KeyboardNavigationSettings AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" />
                </ClientSettings>
                <PagerStyle HorizontalAlign="Center" CssClass="dataPager" PagerTextFormat="<%$Resources:PageGlobalResources,GridPagerTextFormat %>"
                    PageSizeLabelText="<%$Resources:PageGlobalResources,GridPageSizeText %>" NextPageToolTip="<%$Resources:PageGlobalResources,GridNextPageTooltip %>"
                    PrevPageToolTip="<%$Resources:PageGlobalResources,GridPreviousPageTooltip %>" LastPageToolTip="<%$Resources:PageGlobalResources,GridLastPageTooltip %>"
                    FirstPageToolTip="<%$Resources:PageGlobalResources,GridFirstPageTooltip %>" NextPagesToolTip="<%$Resources:PageGlobalResources,GridNextPagesTooltip %>"
                    PrevPagesToolTip="<%$Resources:PageGlobalResources,GridPreviousPagesTooltip %>" />
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView>
                    <FilterItemStyle CssClass="bgTheme" ForeColor="White" />
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="DateEnterred" DataField="DateEntered" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" ItemStyle-Width="4em" HeaderStyle-Width="2em">
                            <FilterTemplate>
                                <div class="row" style="margin-top: 20px; width:250px;">
                                    <div class="col-md-12">
                                        <asp:Label runat="server" CssClass="col-md-2 padL0" Text="<%$Resources:PageGlobalResources,FromLabel %>"></asp:Label>
                                        <div class="col-md-4 padL0">
                                            <telerik:RadDatePicker RenderMode="Lightweight" runat="server" ID="dpSearchDateFrom" ClientEvents-OnDateSelected="FromDateSelected" Width="12em" />
                                        </div>

                                    </div>
                                    <div class="col-md-12 margin-top10">
                                        <asp:Label runat="server" CssClass="col-md-2 padL0" Text="<%$Resources:PageGlobalResources,ToLabel %>"></asp:Label>
                                        <div class="col-md-4 padL0">
                                            <telerik:RadDatePicker RenderMode="Lightweight" runat="server" ID="dpSearchDateTo" ClientEvents-OnDateSelected="ToDateSelected" DateInput-ClientEvents-OnBlur="ToDateExited" Width="12em" />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:CompareValidator ID="dateCompareValidator" CssClass="col-md-12" runat="server" ControlToValidate="dpSearchDateTo" ForeColor="Red"
                                            ControlToCompare="dpSearchDateFrom" Operator="GreaterThanEqual" Type="Date" ErrorMessage="<%$Resources:DateError %>">
                                        </asp:CompareValidator>
                                    </div>
                                </div>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function ToDateExited(sender, args) {
                                            var tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");

                                        }
                                        function FromDateSelected(sender, args) {
                                            if (Page_ClientValidate()) {

                                                var tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");
                                                var ToPicker = $find('<%# TryCast(Container, GridItem).FindControl("dpSearchDateTo").ClientID%>');

                                                var fromDate = FormatSelectedDate(sender, -1);
                                                var toDate = FormatSelectedDate(ToPicker, +1);

                                                if (fromDate != null && toDate != null) {
                                                    tableView.filter("DateEnterred", fromDate + " " + toDate, "Between");
                                                }
                                            }

                                        }
                                        function ToDateSelected(sender, args) {
                                            if (Page_ClientValidate()) {

                                                var tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");
                                                var FromPicker = $find('<%# TryCast(Container, GridItem).FindControl("dpSearchDateFrom").ClientID%>');

                                                var fromDate = FormatSelectedDate(FromPicker, -1);
                                                var toDate = FormatSelectedDate(sender, +1);

                                                if (fromDate != null && toDate != null) {
                                                    tableView.filter("DateEnterred", fromDate + " " + toDate, "Between");
                                                }
                                            }
                                        }

                                        function applyFilterDate(fromDate, toDate, tableView) {
                                            if (!tableView) {
                                                tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");
                                            }

                                            if (fromDate && toDate) {
                                                tableView.filter("DateEnterred", fromDate + " " + toDate, "Between");

                                            }
                                            else if (fromDate) {
                                                tableView.filter("DateEnterred", fromDate, "Between");
                                            }
                                            else if (toDate) {
                                            }
                                            else {
                                            }
                                        }
                                        function FormatSelectedDate(picker, day) {
                                            var formattedDate;
                                            var date = picker.get_selectedDate();

                                            if (date) {
                                                if (day && day != 0) {
                                                    date.setDate(date.getDate() + day);
                                                }
                                                var dateInput = picker.get_dateInput();
                                                formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
                                            }

                                            return formattedDate;
                                        }
                                    </script>
                                </telerik:RadScriptBlock>

                            </FilterTemplate>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Username" UniqueName="Username" AllowFiltering="true" ItemStyle-Width="10em" HeaderStyle-Width="10em">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cboSearchUsername" RenderMode="Lightweight" runat="server" Width="10em" EmptyMessage="<%$Resources:PageGlobalResources,CreatedByHeader %>" CssClass="input-md rwPadLR5"
                                    OnClientSelectedIndexChanged="PropertyUsernameChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" DataTextField="Username" OnClientKeyPressing="RadComboKeyPress"
                                    OnClientBlur="PropertyUsernameBlurred">
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function PropertyUsernameChanged(sender, args) {
                                            FilterUsername(args.get_item().get_value(), false);
                                        }
                                        function PropertyUsernameBlurred(sender, args) {
                                            RadComboBlurred(sender, args);
                                            var comboValue = sender.get_value();
                                            var useLike = false

                                            if (comboValue == null || comboValue == "") {
                                                comboValue = sender.get_text();
                                                useLike = true;
                                            }

                                            FilterUsername(comboValue, useLike);
                                        }
                                        function FilterUsername(filterValue, useLike) {
                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                            if (filterValue == null || filterValue == "") {
                                                tableView.filter("Username", filterValue, "NoFilter");
                                            }
                                            else if (useLike) {
                                                tableView.filter("Username", filterValue, "StartsWith");
                                            }
                                            else {
                                                tableView.filter("Username", filterValue, "EqualTo");
                                            }
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>

                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="AuditType" UniqueName="AuditTypeDesc" ItemStyle-Width="10em" HeaderStyle-Width="10em">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cboSearchAuditType" RenderMode="Lightweight" runat="server" Width="10em" EmptyMessage="<%$Resources:PageGlobalResources,AuditTypeHeader %>" CssClass="input-md rwPadLR5"
                                    OnClientSelectedIndexChanged="AuditTypeChanged" MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress"
                                    OnClientBlur="AuditTypeBlurred">
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function AuditTypeChanged(sender, args) {
                                            FilterAuditType(args.get_item().get_value());
                                        }
                                        function AuditTypeBlurred(sender, args) {
                                            RadComboBlurred(sender, args);
                                            var comboValue = sender.get_value();

                                            FilterAuditType(comboValue);
                                        }
                                        function FilterAuditType(filterValue) {
                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                            if (filterValue == null || filterValue == "") {
                                                tableView.filter("AuditTypeDesc", filterValue, "NoFilter");
                                            }
                                            else {
                                                tableView.filter("AuditTypeDesc", filterValue, "EqualTo");
                                            }
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblAuditDescription" />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="Notes" UniqueName="Notes" ItemStyle-Width="15em" HeaderStyle-Width="15em">
                            <FilterTemplate>
                                <telerik:RadTextBox ID="txtSearchNotes" runat="server" Width="30em" EmptyMessage="<%$Resources:PageGlobalResources,NotesHeader %>">
                                    <ClientEvents OnValueChanged="NotesChanged" />
                                </telerik:RadTextBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function NotesChanged(sender, args) {
                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                            tableView.filter("Notes", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.Contains);
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                               <ItemTemplate>
                                <asp:Label runat="server" ID="lblNotes" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </div>
</asp:Content>

