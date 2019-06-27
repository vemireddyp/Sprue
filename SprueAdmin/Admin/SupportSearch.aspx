<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_SupportSearch" CodeBehind="SupportSearch.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">

    <script>
        $(document).ready(function () {
            $(".mainContent #userdetails").removeClass("margin-left-125");
            $(".mainContent #userdetails").addClass("col-md-offset-2");

            function pageLoad() {
                $('.datePicker').each(function () {
                    $(this).datepicker({ dateFormat: 'dd/mm/yy' });
                });
            };

        });

        function clearSearchFields() {
            //clear text fields
            $("#<%=txtAccountID.ClientID%>").val("");
            $("#<%=txtSurname.ClientID%>").val("");
            $("#<%=txtMACAddress.ClientID%>").val("");
            $("#<%=txtDeviceID.ClientID%>").val("");
            $("#<%=txtPostCode.ClientID%>").val("");

            //clear date fields
            var fromDateCtrl = $find("<%=dpFromDate.ClientID %>");
            fromDateCtrl.clear();

            var toDateCtrl = $find("<%=dpToDate.ClientID%>");
            toDateCtrl.clear();

            //clear drop down lists to default value
            $("#<%=cboUserName.ClientID%>").val("");
            $("#<%=cboStatus.ClientID%>").val(0);
        }

    </script>
    <style>
        .RadGrid .RadPicker div.RadInput {
            width: 136px !important;
            font-family: Arial;
            font-size: 14px;
            font-style: normal;
        }
    </style>


    <div class=" row  bgwhite" style="margin: 0px">
        <div class="col-md-12" style="border: groove;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                <ContentTemplate>
                    <div class="col-md-12 lblsubHeader">
                        <asp:Label ID="lblSearch" runat="server" Text="<%$Resources:PageGlobalResources,SelectSearchFieldsPrompt %>" CssClass="col-md-12 text-left"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <div class="col-md-12 form-group">
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtAccountID" runat="server" CssClass="form-control inputhgt"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="lblSupportRef" runat="server" Text="<%$Resources:PageGlobalResources,SupportRefNoLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control inputhgt"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="lblLastName" runat="server" Text="<%$Resources:PageGlobalResources,CustLastNameLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control inputhgt"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:PageGlobalResources,PostCodeLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control inputhgt" MaxLength="9"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:PageGlobalResources,MACLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtMACAddress" runat="server" CssClass="form-control inputhgt" MaxLength="17"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 form-group">
                            <div class="col-md-12 form-group">
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:PageGlobalResources,AdminUserLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <telerik:RadComboBox ID="cboUserName" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" AutoPostBack="true" Filter="StartsWith" DataTextField="username" DataValueField="UserID"
                                        EmptyMessage="<%$Resources:PageGlobalResources,AllSelectItemText %>" MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="lblStatus" runat="server" Text="<%$Resources:PageGlobalResources,TicketStatusLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <telerik:RadComboBox ID="cboStatus" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" AutoPostBack="true" Filter="StartsWith" DataTextField="StatusDescription" DataValueField="StatusID"
                                        EmptyMessage="<%$Resources:PageGlobalResources,AllSelectItemText %>" MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="lblDeviceID" runat="server" Text="<%$Resources:PageGlobalResources,DeviceIDLabel %>" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtDeviceID" runat="server" CssClass="form-control inputhgt"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="Label8" runat="server" CssClass="col-md-4 control-label text-left" Text="<%$Resources:PageGlobalResources,DateLabel %>"></asp:Label>
                                <div class="col-md-8 input-group date datestyle">
                                    <asp:Label ID="lblStartDate" runat="server" CssClass="control-label text-left input-group-addon" Style="padding: 5px 6px;" Text="<%$Resources:PageGlobalResources,StartDateLabel %>"></asp:Label>
                                    <%--<asp:TextBox runat="server" ID="txtStartDate" CssClass="datePicker setHidden form-control inputhgt"></asp:TextBox>--%>
                                    <telerik:RadDatePicker RenderMode="Lightweight" runat="server" EnableEmbeddedSkins="false" ID="dpFromDate" />
                                </div>
                            </div>

                            <div class="col-md-12 form-group">
                                <asp:Label ID="Label6" runat="server" Text="" CssClass="control-label col-md-4 text-left"></asp:Label>
                                <div class="col-md-8 input-group date datestyle">
                                    <asp:Label ID="lblEndDate" runat="server" CssClass="control-label text-left input-group-addon" Style="padding: 5px 15px;" Text="<%$Resources:PageGlobalResources,EndDateLabel %>"></asp:Label>
                                    <%--<asp:TextBox runat="server" ID="txtEndDate" CssClass="datePicker setHidden form-control inputhgt"></asp:TextBox>--%>
                                    <telerik:RadDatePicker RenderMode="Lightweight" runat="server" ID="dpToDate" />
                                </div>
                                <div class="col-md-offset-4">
                                    <asp:CompareValidator ID="valStartEndCompare" runat="server" ControlToValidate="dptoDate" ControlToCompare="dpFromDate" Display="Dynamic" ForeColor="Red" Text="<%$Resources:ValidationCtlResources,ToFromMismatchMsg %>"
                                        Operator="GreaterThan" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="col-md-12 rWPT15">
                <div class="col-md-4 text-left">
                    <asp:Button ID="btnExport" runat="server" CssClass="btn  btn-warning btnLogOff pull-left" Text="<%$Resources:PageGlobalResources,ExportDataButton %>" UseSubmitBehavior="false" />
                </div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="always">
                    <ContentTemplate>
                        <div class="col-md-1 col-md-offset-1" style="left: -15px;">
                            <asp:Button ID="btnClear" runat="server" CssClass="btn  btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,ClearButton %>" CausesValidation="false" />
                        </div>
                        <div class="col-md-2 col-md-offset-2">
                            <asp:Button ID="btnRaiseTicket" runat="server" CssClass="btn btn-warning btnLogOff marL5" Text="<%$Resources:PageGlobalResources,RaiseTicketButton %>" />
                        </div>
                        <div class="col-md-1 pull-right marR30">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:PageGlobalResources,SearchButton %>" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="always">
                <ContentTemplate>
                    <div class=" col-md-10 col-md-offset-2">
                        <asp:Label ID="lblNoData" runat="server" Text="<%$Resources:PageGlobalResources,SearchEmptyText %>"></asp:Label>
                    </div>
                    <div class=" col-md-12" id="grdRequests" style="padding-right: 15px; padding-top: 15px;">
                    </div>
                    <telerik:RadAjaxLoadingPanel runat="server" ID="lpGridLoading" />
                    <telerik:RadAjaxPanel runat="server" ID="apGridUpdate">
                        <div id="grdAccount" class="row">
                            <telerik:RadGrid RenderMode="Lightweight" ID="rgSupportTickets" runat="server" AllowPaging="true" AllowSorting="true" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" AutoGenerateColumns="false" CellPadding="5" Font-Overline="false" ForeColor="#333333" PageSize="20" BorderStyle="None"
                                BorderWidth="0" GridLines="None" Font-Size="Small" CssClass="col-md-12 hideHeaderBar" AllowFilteringByColumn="true"
                                ShowHeaderWhenEmpty="true" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>"
                                MasterTableView-DataKeyNames="SupportRequestID,AccountID,IntaMacAddress">
                                <FooterStyle BackColor="#333333" Font-Bold="False" ForeColor="White" />
                                <GroupingSettings CaseSensitive="false" />
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
                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="white" Height="0" Width="200" CssClass="bgTheme" />
                                <FilterItemStyle CssClass="bgTheme" />
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="SupportRequestID" UniqueName="SupportRequestID" AllowFiltering="true" 
                                            HeaderText="<%$Resources:PageGlobalResources,SupportRequestHeader %>" DataType="System.String">
                                            <FilterTemplate>
                                                <telerik:RadTextBox ID="txtSearchSupportRequestID" runat="server" Width="80px" EmptyMessage="<%$Resources:PageGlobalResources,SupportRequestHeader %>">
                                                    <ClientEvents OnValueChanged="SupportRequestIDChanged" />
                                                </telerik:RadTextBox>
                                                <telerik:RadScriptBlock runat="server">
                                                    <script type="text/javascript">
                                                        function SupportRequestIDChanged(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            tableView.filter("SupportRequestID", args.get_newValue(), "StartsWith");
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="AccountID" DataField="AccountID" HeaderText="<%$Resources:PageGlobalResources,AccountHeader %>">
                                            <FilterTemplate>
                                                <telerik:RadTextBox ID="txtSearchAccountID" runat="server" Width="80px" EmptyMessage="<%$Resources:PageGlobalResources,AccountHeader %>">
                                                    <ClientEvents OnValueChanged="AccountIDChanged" />
                                                </telerik:RadTextBox>
                                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                                    <script type="text/javascript">
                                                        function AccountIDChanged(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            tableView.filter("AccountID", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>

                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DeviceID" UniqueName="DeviceID" HeaderText="<%$Resources:PageGlobalResources,DeviceIDHeader %>">
                                            <FilterTemplate>
                                                <telerik:RadTextBox ID="txtSearchDeviceID" runat="server" Width="90px" EmptyMessage="<%$Resources:PageGlobalResources,DeviceIDHeader %>">
                                                    <ClientEvents OnValueChanged="DeviceIDChanged" />
                                                </telerik:RadTextBox>
                                                <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                                    <script type="text/javascript">
                                                        function DeviceIDChanged(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            tableView.filter("DeviceID", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IntaSurname" UniqueName="IntaSurname" HeaderText="<%$Resources:PageGlobalResources,LastNameHeader %>">
                                            <FilterTemplate>
                                                <telerik:RadTextBox ID="txtSearchLastName" runat="server" Width="90px" EmptyMessage="<%$Resources:PageGlobalResources,LastNameHeader %>">
                                                    <ClientEvents OnValueChanged="SurnameChanged" />
                                                </telerik:RadTextBox>
                                                <telerik:RadScriptBlock runat="server">
                                                    <script type="text/javascript">
                                                        function SurnameChanged(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            tableView.filter("IntaSurname", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IntaPostCode" UniqueName="IntaPostCode" HeaderText="<%$Resources:PageGlobalResources,PostCodeHeader %>">
                                            <FilterTemplate>
                                                <telerik:RadTextBox ID="txtSearchPostcode" runat="server" Width="90px" EmptyMessage="<%$Resources:PageGlobalResources,PostCodeHeader %>">
                                                    <ClientEvents OnValueChanged="PostcodeSearchChanged" />
                                                </telerik:RadTextBox>
                                                <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                                    <script type="text/javascript">
                                                        function PostcodeSearchChanged(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            tableView.filter("IntaPostCode", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="UpdatedBy" UniqueName="UpdatedBy" HeaderText="<%$Resources:PageGlobalResources,AdminUserHeader %>">
                                            <FilterTemplate>
                                                <telerik:RadTextBox ID="txtSearchUpdatedBy" runat="server" Width="110px" EmptyMessage="<%$Resources:PageGlobalResources,AdminUserHeader %>">
                                                    <ClientEvents OnValueChanged="AdminUserSearchChanged" />
                                                </telerik:RadTextBox>
                                                <telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
                                                    <script type="text/javascript">
                                                        function AdminUserSearchChanged(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            tableView.filter("UpdatedBy", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="StatusDescription" DataField="StatusDescription" HeaderText="<%$Resources:PageGlobalResources,SupportStatusHeader %>">
                                            <FilterTemplate>
                                                <telerik:RadComboBox ID="cboSearchStatus" RenderMode="Lightweight" runat="server" Width="95px" EmptyMessage="<%$Resources:PageGlobalResources,SupportStatusHeader %>" CssClass="rwPadLR5"
                                                    OnClientSelectedIndexChanged="StatusDescriptionChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true"
                                                    OnClientBlur="StatusDescriptionBlurred" OnClientKeyPressing="RadComboKeyPress">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,SupportStatusOpen %>" Value="<%$Resources:PageGlobalResources,SupportStatusOpen %>" />
                                                        <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,SupportStatusClosed %>" Value="<%$Resources:PageGlobalResources,SupportStatusClosed %>" />
                                                        <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,SupportStatusEscalatedToServiceProvider %>" Value="<%$Resources:PageGlobalResources,SupportStatusEscalatedToServiceProvider %>" />
                                                        <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,SupportStatusEscalatedToDistibutor %>" Value="<%$Resources:PageGlobalResources,SupportStatusEscalatedToDistibutor %>" />
                                                        <telerik:RadComboBoxItem Text="<%$Resources:PageGlobalResources,SupportStatusEscalatedToSprue %>" Value="<%$Resources:PageGlobalResources,SupportStatusEscalatedToSprue %>" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                                <telerik:RadScriptBlock ID="radscriptB2" runat="server">
                                                    <script type="text/javascript">
                                                        function StatusDescriptionChanged(sender, args) {
                                                            FilterStatusID(args.get_item().get_value());
                                                        }
                                                        function StatusDescriptionBlurred(sender, args) {
                                                            RadComboBlurred(sender, args);
                                                            FilterStatusID(sender.get_value());
                                                        }
                                                        function FilterStatusID(filterValue) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            if (filterValue == null || filterValue == "") {
                                                                tableView.filter("StatusDescription", filterValue, "NoFilter");
                                                            }
                                                            else {
                                                                tableView.filter("StatusDescription", filterValue, "EqualTo");
                                                            }
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IntaMacAddress" UniqueName="IntaMacAddress" HeaderText="<%$Resources:PageGlobalResources,MACAddressHeader %>">
                                            <FilterTemplate>
                                                <telerik:RadTextBox ID="txtSearchMacAddress" runat="server" Width="135px" EmptyMessage="<%$Resources:PageGlobalResources,MACAddressHeader %>">
                                                    <ClientEvents OnValueChanged="MacAddressSearchChanged" />
                                                </telerik:RadTextBox>
                                                <telerik:RadScriptBlock ID="RadScriptBlock5" runat="server">
                                                    <script type="text/javascript">
                                                        function MacAddressSearchChanged(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                            tableView.filter("IntaMacAddress", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn UniqueName="LastUpdated" ItemStyle-Width="400px" DataField="LastUpdated" DataType="System.DateTime" HeaderText="<%$Resources:PageGlobalResources,DateRaised %>">
                                            <FilterTemplate>
                                                <headerstyle width="48px" wrap="False" />
                                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" ID="dpSearchSystemTestDate" DateInput-EmptyMessage="<%$Resources:PageGlobalResources,DateRaised %>"
                                                    ClientEvents-OnDateSelected="LastUpdatedSelected" />

                                                <telerik:RadScriptBlock ID="RadScriptBlock6" runat="server">
                                                    <script type="text/javascript">
                                                        function LastUpdatedSelected(sender, args) {
                                                            //clearSearchFields();
                                                            var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");

                                                            var fromDate = sender.get_selectedDate();
                                                            if (fromDate) {
                                                                var dateInput = sender.get_dateInput();
                                                                var toDate = new Date(fromDate);
                                                                toDate.setDate(toDate.getDate() + 1);

                                                                var formattedFrom = dateInput.get_dateFormatInfo().FormatDate(fromDate, dateInput.get_displayDateFormat());
                                                                var formattedTo = dateInput.get_dateFormatInfo().FormatDate(toDate, dateInput.get_displayDateFormat());

                                                                tableView.filter("LastUpdated", formattedFrom + " " + formattedTo, "Between");
                                                            }
                                                            else {
                                                                tableView.filter("LastUpdated", "", "NoFilter");
                                                            }
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>



                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn ButtonType="LinkButton" Text="<%$Resources:PageGlobalResources,Selectbutton %>" UniqueName="View" CommandName="EditSupportTicket">
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                    </telerik:RadAjaxPanel>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
               
        
</asp:Content>
