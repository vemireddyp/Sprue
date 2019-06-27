<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_EventLog" CodeBehind="EventLog.aspx.vb" %>

<%@ Register Src="~/UserControls/Dashboard.ascx" TagName="Dashboard" TagPrefix="uc1" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <div class="col-md-12 margin-top15 borderBlack">
        <div class="row rwPadTB9 bgTheme txtcolor">
            <div class="col-md-5">
                <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="<%$Resources:SearchEventLog %>" />
            </div>
        </div>

        <div class="col-md-12 rwPadTB9" runat="server" id="divCompaniesFilter">
            <div class="col-md-6 rwPadLR0">
                <div class="col-md-12 rwPadTB9" runat="server" id="divDistList">
                    <div class="col-md-4">
                        <asp:Label ID="lblDistributor" Text="<%$Resources:AllDistributorsText %>" runat="server" />
                    </div>
                    <div class="col-md-8" runat="server">
                        <telerik:RadComboBox ID="cboDistributors" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md txtcolor" AutoPostBack="true" Filter="StartsWith"
                            MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                    </div>
                </div>
                <div class="col-md-12 rwPadTB9">
                    <div class="col-md-4">
                        <asp:Label ID="Label1" Text="<%$Resources:AllProvidersText %>" runat="server" />
                    </div>
                    <div class="col-md-8" runat="server" id="divSProList">
                        <telerik:RadComboBox ID="cboServiceProvs" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                            MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                    </div>
                </div>
                <div class="col-md-12 rwPadTB9">
                    <div class="col-md-4 padR0">
                        <asp:Label ID="Label2" Text="<%$Resources:AllInstallersText %>" runat="server" />
                    </div>
                    <div class="col-md-8" runat="server" id="divInstList">
                        <telerik:RadComboBox ID="cboInstallers" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md" Filter="StartsWith"
                            MarkFirstMatch="true" AllowCustomText="true" AutoPostBack="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                    </div>
                </div>
            </div>

        </div>

        <div class="col-md-12" id="divAccountDets" runat="server" style="font-size: 14px;">
            <div class="col-md-12 rwPadTB9">
                <div class="col-md-6">
                    <div class="col-md-6">
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:PageGlobalResources,AccountHolderLabel %>" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" ID="lblAccountHolder" />
                    </div>
                </div>
            </div>
            <div class="col-md-12 rwPadTB9 ">
                <div class="col-md-6 ">
                    <div class="col-md-6">
                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" ID="lblAccountID" />
                    </div>
                </div>
            </div>
            <div id="divMACAddress" class="col-md-12 rwPadTB9" runat="server">
                <div class="col-md-6">
                    <div class="col-md-6">
                        <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,MACAddressLabel %>" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" ID="lblMACAddress" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-12 margin-top10" id="divPropertyAddress" runat="server" style="font-size: 14px;">
            <div class="col-md-12">
                <div class="col-md-6">
                    <div class="col-md-6">
                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:PageGlobalResources,PropertyAddressLabel %>" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" ID="lblPropertyAddress1" /><br />
                        <asp:Label runat="server" ID="lblPropertyAddress2" CssClass="rwPadTB9" /><br />
                        <asp:Label runat="server" ID="lblPostCode" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-12 rwPadTB15">
            <div class="col-md-5 marL15">
                <asp:Button ID="btnExport" runat="server" CssClass="btn  btn-warning btnLogOff pull-left" Text="<%$Resources:PageGlobalResources,ExportDataButton %>" UseSubmitBehavior="false" CausesValidation="false" />
            </div>
            <div class="pull-right marR30">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:PageGlobalResources,SearchButton %>" CausesValidation="false" />
            </div>
            <div class="pull-right marR30">
                <asp:Button ID="btnClear" runat="server" CssClass="btn  btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,ClearButton %>" CausesValidation="false" />
            </div>
        </div>

        <telerik:RadAjaxPanel runat="server" ID="apGridUpdate">
            <telerik:RadAjaxLoadingPanel runat="server" ID="lpGridLoading" />
            <div id="grdAccount" class="row rwMargLR0">
                <telerik:RadGrid RenderMode="Lightweight" ID="rgAccounts" runat="server" AllowPaging="true" AllowSorting="true" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" AutoGenerateColumns="false" CellPadding="5" Font-Overline="false" ForeColor="#333333" PageSize="20" BorderStyle="None"
                    BorderWidth="0" GridLines="None" Font-Size="Small" CssClass="row hideHeaderBar" AllowFilteringByColumn="true"
                    MasterTableView-ShowHeadersWhenNoRecords="true" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>"
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
                    <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="white" Height="0" Width="200" CssClass="bgTheme" />
                    <FilterItemStyle CssClass="bgTheme" />
                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto">
                        <Columns>
                            <telerik:GridBoundColumn DataField="IntaAccountID" UniqueName="IntaAccountID" AllowFiltering="true" HeaderText="<%$Resources:PageGlobalResources,AccountHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchAccountID" runat="server" Width="6em" EmptyMessage="<%$Resources:PageGlobalResources,AccountHeader %>">
                                        <ClientEvents OnValueChanged="AccountIDChanged" OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
                                        <script type="text/javascript">
                                            function AccountIDChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaAccountID", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="IntaEventDate" DataField="LocalEventDateTime" DataType="System.DateTime" DataFormatString="{0:d}" HeaderText="<%$Resources:PageGlobalResources,CreatedDate %>">
                                <FilterTemplate>
                                    <telerik:RadDatePicker RenderMode="Lightweight" runat="server" ID="dpSearchCreatedDate" ClientEvents-OnDateSelected="FilterGrid" ClientEvents-OnPopupOpening="datePickerPopupOpening" ClientEvents-OnPopupClosing="datePickerPopupClosing"
                                        Width="7em" DateInput-EmptyMessage="<%$Resources:PageGlobalResources,CreatedDate %>">
                                        <DateInput runat="server">
                                            <ClientEvents OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" OnValueChanged="textFilterLostFocus" />
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                        <script type="text/javascript">
                                            function FilterGrid(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaEventDate", "", "NoFilter");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="IntaEventTime" DataField="LocalEventDateTime" DataType="System.DateTime" DataFormatString="{0:HH:mm:ss}" HeaderText="<%$Resources:PageGlobalResources,CreatedTime %>">
                                <FilterTemplate>
                                    <telerik:RadTimePicker ID="tpSearchCreatedTime" RenderMode="Lightweight" runat="server" TimeView-StartTime="00:00" TimeView-EndTime="23:45" TimeView-Interval="00:15" TimeView-TimeFormat="HH:mm"
                                        DateInput-EmptyMessage="<%$Resources:PageGlobalResources,CreatedTime %>" DateInput-DisplayDateFormat="HH:mm" ClientEvents-OnDateSelected="FilterGrid" Width="7em"
                                        ClientEvents-OnPopupOpening="datePickerPopupOpening" ClientEvents-OnPopupClosing="datePickerPopupClosing">
                                        <DateInput runat="server">
                                            <ClientEvents OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" OnValueChanged="textFilterLostFocus" />
                                        </DateInput>
                                    </telerik:RadTimePicker>

                                </FilterTemplate>
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="SIADesc" DataField="SIADesc" HeaderText="<%$Resources:PageGlobalResources,EventDetailsHeader %>">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cboSearchSIA" RenderMode="Lightweight" runat="server" Width="13em" EmptyMessage="<%$Resources:PageGlobalResources,EventDetailsHeader %>" CssClass="input-md rwPadLR5"
                                        OnClientSelectedIndexChanged="PropertySIADescChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true"  OnClientKeyPressing="RadComboKeyPress"
                                        OnClientBlur="PropertySIADescBlurred" MaxHeight="175px" OnClientFocus="comboFilterGotFocus">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="radscriptB2" runat="server">
                                        <script type="text/javascript">
                                            function PropertySIADescChanged(sender, args) {
                                                FilterSIADesc(args.get_item().get_value());
                                            }
                                            function PropertySIADescBlurred(sender, args) {
                                                comboFilterLostFocus(sender, args);
                                                FilterSIADesc(sender.get_value());
                                            }
                                            function FilterSIADesc(filterValue) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                if (filterValue == null || filterValue == "") {
                                                    tableView.filter("SIADesc", filterValue, "NoFilter");
                                                }
                                                else {
                                                    tableView.filter("SIADesc", filterValue, "EqualTo");
                                                }
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn DataField="DeviceName" UniqueName="DeviceName" AllowFiltering="true" HeaderText="<%$Resources:PageGlobalResources,DeviceNameHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchPropZoneDesc" runat="server" Width="8em" EmptyMessage="<%$Resources:PageGlobalResources,DeviceNameHeader %>">
                                        <ClientEvents OnValueChanged="DeviceNameChanged" OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                        <script type="text/javascript">
                                            function DeviceNameChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("DeviceName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPropZoneDesc" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="IntaSensorType" DataField="IntaSensorType" HeaderText="<%$Resources:PageGlobalResources,DeviceHeader %>">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cboSearchSensorDesc" RenderMode="Lightweight" runat="server" Width="10em" EmptyMessage="<%$Resources:PageGlobalResources,DeviceHeader %>" CssClass="input-md rwPadLR5"
                                        OnClientSelectedIndexChanged="SensorTypeDescChanged" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress"
                                        OnClientBlur="SensorTypeDescBlurred" OnClientFocus="comboFilterGotFocus">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock5" runat="server">
                                        <script type="text/javascript">
                                            function SensorTypeDescChanged(sender, args) {
                                                FilterSensorTypeDesc(args.get_item().get_value());
                                            }
                                            function SensorTypeDescBlurred(sender, args) {
                                                comboFilterLostFocus(sender, args);
                                                FilterSensorTypeDesc(sender.get_value());
                                            }
                                            function FilterSensorTypeDesc(filterValue) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                if (filterValue == null || filterValue == "") {
                                                    tableView.filter("IntaSensorType", filterValue, "NoFilter");
                                                }
                                                else {
                                                    tableView.filter("IntaSensorType", filterValue, "EqualTo");
                                                }
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSensorDesc" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridBoundColumn DataField="IntaDeviceID" UniqueName="IntaDeviceID" AllowFiltering="true" HeaderText="<%$Resources:PageGlobalResources,DeviceIDHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchDeviceID" runat="server" Width="6em" EmptyMessage="<%$Resources:PageGlobalResources,DeviceIDHeader %>">
                                        <ClientEvents OnValueChanged="DeviceIDChanged" OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                        <script type="text/javascript">
                                            function DeviceIDChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaDeviceID", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="IntaAreaName" UniqueName="IntaAreaName" HeaderText="<%$Resources:PageGlobalResources,ZoneHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchAreaName" runat="server" Width="7em" EmptyMessage="<%$Resources:PageGlobalResources,ZoneHeader %>">
                                        <ClientEvents OnValueChanged="AreaNameChanged" OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock runat="server">
                                        <script type="text/javascript">
                                            function AreaNameChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaAreaName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="IntaAlarmType" DataField="IntaAlarmType" Visible="false">
                                <FilterTemplate>
                                </FilterTemplate>

                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="IntaRoomName" UniqueName="IntaRoomName" HeaderText="<%$Resources:PageGlobalResources,LocationHeader %>">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchRoomName" runat="server" Width="7em" EmptyMessage="<%$Resources:PageGlobalResources,LocationHeader %>">
                                        <ClientEvents OnValueChanged="RoomNameChanged" OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock6" runat="server">
                                        <script type="text/javascript">
                                            function RoomNameChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaRoomName", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.StartsWith);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>




                            <telerik:GridBoundColumn DataField="IntaTemperaturePPM" UniqueName="IntaTemperaturePPM" AllowFiltering="true" HeaderText="<%$Resources:Value %>" Visible="false">
                                <FilterTemplate>
                                    <telerik:RadTextBox ID="txtSearchTempID" runat="server" Width="5em" EmptyMessage="<%$Resources:Value %>">
                                        <ClientEvents OnValueChanged="TempChanged" OnFocus="textFilterGotFocus" OnBlur="textFilterLostFocus" />
                                    </telerik:RadTextBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock7" runat="server">
                                        <script type="text/javascript">
                                            function TempChanged(sender, args) {
                                                var tableView = $find("<%# (DirectCast(Container, GridItem)).OwnerTableView.ClientID%>");
                                                tableView.filter("IntaTemperaturePPM", args.get_newValue(), Telerik.Web.UI.GridFilterFunction.EqualTo);
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>

                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </telerik:RadAjaxPanel>
        <asp:Panel ID="pnlEventLogGrid" runat="server">
            <asp:Timer ID="tmrEventLogGrid" runat="server" Interval="10000" OnTick="tmrEventLogGrid_Tick">
            </asp:Timer>
            <telerik:RadScriptBlock runat="server">
                <script type="text/javascript">
                    function setTimerEnabled(timerID, boolEnabled) {
                        var timer = $find(timerID);
                        if (timer && timer.get_enabled() != boolEnabled) {
                            timer._update(boolEnabled, timer.get_interval());
                        }
                    }

                    function gridEventStopTimer(sender, args) {
                        setTimerEnabled($("[id$='tmrEventLogGrid'").prop('id'), false);
                    }

                    function gridEventStartTimer(sender, args) {
                        setTimerEnabled($("[id$='tmrEventLogGrid'").prop('id'), true);
                    }

                    var lastTextVal = '$$NOT SAVED$$';
                    var lastComboVal = '$$NOT SAVED$$';

                    function comboFilterGotFocus(sender, args) {
                        lastTextVal = sender.get_text().toLowerCase();
                        gridEventStopTimer(sender, args)
                    }

                    function comboFilterLostFocus(sender, args) {
                        RadComboBlurred(sender, args);
                        if (lastTextVal == sender.get_text().toLowerCase()) {
                            lastTextVal = '$$NOT SAVED$$';
                            gridEventStartTimer(sender, args);
                        }
                    }

                    function textFilterGotFocus(sender, args) {
                        lastTextVal = sender.get_value().toLowerCase();
                        gridEventStopTimer(sender, args)
                    }

                    function textFilterLostFocus(sender, args) {
                        if (lastTextVal == sender.get_value().toLowerCase()) {
                            lastTextVal = '$$NOT SAVED$$';
                            gridEventStartTimer(sender, args);
                        }
                    }

                    function datePickerPopupOpening(sender, args) {
                        textFilterGotFocus(sender.get_dateInput(), args);
                    }

                    function datePickerPopupClosing(sender, args) {
                        textFilterLostFocus(sender.get_dateInput(), args);

                    }
                </script>
            </telerik:RadScriptBlock>
        </asp:Panel>
    </div>

</asp:Content>
