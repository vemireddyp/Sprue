<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_SupportDetail" CodeBehind="SupportDetail.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">

            function enableSearchButton(enabled) {
                var btnElem = $("[id$='btnSearch']");

                if (btnElem) {
                    if (enabled) {
                        btnElem.removeAttr('disabled');
                        btnElem.removeClass('aspNetDisabled');
                    }
                    else {
                        btnElem.attr('disabled', 'disabled');
                        btnElem.addClass('aspNetDisabled');

                    }
                }
            }

            function setupHandlers() {
                //$("[id$='btnSearch']").attr('disabled', 'disabled');

                $('.btnSaveEnable').on("keyup focus change", function () {
                    if ($('.textlength').val().length > 0) {
                        $('.btnSave').removeAttr('disabled');
                    }
                    else {
                        $('.btnSave').attr('disabled', 'disabled');
                    }
                });

                $('.searchEnable').keypress(function (e) {
                    if ($(e.target).val() != "") {
                        enableSearchButton(true);
                    }

                });
            }

            function filterTextChanged(sender, args) {
                if (sender.get_text() === "") {
                    filterValueSelected(sender);
                }

            }

            function clearCombo(combo) {
                combo.clearSelection();
                combo.set_text('');

            }

            var filterKeys = [];
            var itemsShown = 0;
            var shownIndex = -1;

            function filterComboList(combo) {
                var items = combo.get_items();
                shownIndex = -1;
                if (items && typeof items.get_count != 'undefined' && typeof items.getItem != 'undefined') {
                    itemsShown = 0;
                    for (j = 0; j < items.get_count() ; j++) {
                        var item = items.getItem(j);
                        if (item && typeof item.get_value != 'undefined') {
                            var itemVal = item.get_value();
                            var hideItem = false;
                            for (k = 0; (k < filterKeys.length) ; k++) {
                                if (!itemVal.includes(filterKeys[k])) {
                                    hideItem = true;
                                }
                            }

                            if (hideItem) {
                                item.hide();
                            }
                            else {
                                item.show();
                                shownIndex = j;
                                itemsShown++;
                            }
                        }
                    }
                }
            }

            function filterDropList(sender) {
                filterComboList(sender);
            }

            function filterValueSelected(sender, args) {
                var senderID = sender.get_id();

                var combos = [$find($("[id$='cboLastname']").prop('id')), $find($("[id$='cboPostcode']").prop('id')), $find($("[id$='cboAccountID']").prop('id'))];
                filterKeys = [];

                var newItem = sender.get_selectedItem();
                var selectedText = '';

                if (newItem) {
                    selectedText = newItem.get_text();
                }

                if (selectedText != '') {
                    filterKeys.push(',' + selectedText + ',');
                }
                else {
                    var clearBox = false;
                    for (i = 0; i < combos.length; i++) {
                        if (clearBox) {
                            clearCombo(combos[i])
                        }
                        else {
                            if (sender.get_id() === combos[i].get_id()) {
                                clearBox = true;
                            }
                        }
                    }
                }

                for (i = 0; i < combos.length; i++) {
                    if (combos[i]) {
                        if (combos[i].get_id() !== sender.get_id()) {
                            var currItem = combos[i].get_selectedItem();
                            if (currItem) {
                                if (filterKeys.length > 0 && filterKeys[0] !== '' && !currItem.get_value().includes(filterKeys[0])) {
                                    clearCombo(combos[i])
                                }
                                else if (currItem.get_text() === combos[i].get_text()) {
                                    filterKeys.push(',' + currItem.get_text() + ',');
                                }
                            }
                        }
                    }
                }

                for (i = 0; i < combos.length; i++) {
                    if (combos[i]) {
                        if (!filterKeys.includes(',' + combos[i].get_text() + ',')) {
                            filterComboList(combos[i]);
                            var items = combos[i].get_items();
                            if (itemsShown === 1) {
                                var item = items.getItem(shownIndex);
                                if (item) {
                                    item.set_selected(true);
                                    combos[i].set_text(item.get_text());
                                    combos[i].set_selectedIndex(item.get_index());
                                    combos[i].set_value(item.get_value());
                                    combos[i].set_selectedItem(item);
                                }
                            }
                            combos[i].commitChanges();
                        }
                    }
                }

                var enableSearch = true;

                for (i = 0; i < combos.length && enableSearch; i++) {
                    if (!combos[i].get_selectedItem()) {
                        enableSearch = false;
                    }
                }

                enableSearchButton(enableSearch);


            }

        </script>
    </telerik:RadScriptBlock>
    <div class="row bgwhite" style="border: groove; margin: 0% 0% 0% 0%">
        <div class="col-sm-12 rwpadT35">
            <telerik:RadAjaxPanel ID="apAccountInfo" runat="server">
                <div class="row">
                    <div class="col-sm-4">

                        <div id="divNotNew" runat="server" class="row form-group">
                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:PageGlobalResources,RefNoLabel %>" CssClass="col-sm-4"></asp:Label>
                            <asp:Label runat="server" ID="lblRefNo" CssClass="col-sm-5"></asp:Label>
                            <asp:Label ID="lblNotes" runat="server" Text="<%$Resources:Details %>" CssClass="col-sm-3 align-right col-md-push-1"></asp:Label>
                        </div>
                        <div class="row form-group">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:PageGlobalResources,EnteredByLabel %>" CssClass="text-label col-sm-4"></asp:Label>
                            <asp:Label runat="server" ID="lblEnteredBy" CssClass="col-sm-5"></asp:Label>
                            <div id="divNew" runat="server">
                                <asp:Label ID="lblNotes2" runat="server" Text="<%$Resources:Details %>" CssClass="col-sm-3 align-right col-md-push-1"></asp:Label>
                            </div>
                        </div>
                        <div class="row form-group">
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:PageGlobalResources,DateEnteredLabel %>" CssClass="text-label col-sm-4"></asp:Label>
                            <asp:Label runat="server" ID="lblDateEntered" CssClass="col-sm-8"></asp:Label>
                        </div>
                        <div class="row form-group" runat="server" id="divLastname">
                            <asp:Label ID="lblLastName" runat="server" Text="<%$Resources:PageGlobalResources,Lastname %>" CssClass="text-label col-sm-4"></asp:Label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblLastNameDisplay" CssClass="nameFixed"></asp:Label>
                                <asp:TextBox ID="txtLastNameEnter" runat="server" CssClass="form-control inputhgt nameEnter searchEnable" Style="align-self: center"></asp:TextBox>
                                <telerik:RadComboBox ID="cboLastname" runat="server" RenderMode="Lightweight" CssClass="nameSelect" OnClientSelectedIndexChanged="filterValueSelected" AllowCustomText="true" MarkFirstMatch="true" Filter="StartsWith"
                                    OnClientBlur="RadComboBlurred" OnClientKeyPressing="RadComboKeyPress" OnClientTextChange="filterTextChanged" OnClientDropDownOpening="filterComboList">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="row form-group" runat="server" id="divPostCode">
                            <asp:Label ID="lblPostCode" runat="server" Text="<%$Resources:PageGlobalResources,PostCodeLabel %>" CssClass="text-label col-sm-4"></asp:Label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblPostcodeDisplay" CssClass="postcodeDisplay"></asp:Label>
                                <asp:TextBox ID="txtPostcodeEnter" runat="server" CssClass="form-control inputhgt postcodeEnter searchEnable" Style="align-self: center"></asp:TextBox>
                                <telerik:RadComboBox ID="cboPostcode" runat="server" RenderMode="Lightweight" CssClass="postcodeSelect" OnClientSelectedIndexChanged="filterValueSelected" AllowCustomText="true" MarkFirstMatch="true" Filter="StartsWith"
                                     OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" OnClientTextChange="filterTextChanged" OnClientDropDownOpening="filterDropList">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="row form-group" id="divSearch" runat="server">
                            <div class="col-sm-8" id="divSearchResultInvalid" runat="server" style="visibility: hidden">
                                <asp:Label ID="lblNoSearchResult" runat="server" ForeColor="Red" Text="<%$Resources:PageGlobalResources,SearchUnsuccessfulText %>" CssClass="text-label pull-right"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,SearchButton %>" UseSubmitBehavior="false" />
                            </div>
                        </div>
                        <%--                         <div class="col-sm-12 form-group rwPadLR0" id="divSearch" runat="server">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,SearchButton %>" UseSubmitBehavior="false" />
                        </div>--%>
                        <div class="row form-group">
                            <asp:Label ID="lblAccountID" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>" CssClass="text-label col-sm-4"></asp:Label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblAccountIDdisplay" CssClass="accountIDDisplay"></asp:Label>
                                <asp:TextBox ID="txtAccountEnter" runat="server" CssClass="form-control inputhgt accountIDEnter searchEnable"></asp:TextBox>
                                <telerik:RadComboBox ID="cboAccountID" runat="server" RenderMode="Lightweight" CssClass="accountIDSelect" OnClientSelectedIndexChanged="filterValueSelected" AllowCustomText="true" MarkFirstMatch="true" Filter="StartsWith"
                                    OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" OnClientTextChange="filterTextChanged" OnClientDropDownOpening="filterDropList">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="row form-group" runat="server" id="divGateway">
                            <asp:Label ID="lblGateway" runat="server" Text="<%$Resources:PageGlobalResources,GatewayLabel %>" CssClass="text-label col-sm-4"></asp:Label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblGatewayDisplay" CssClass="gatewayDisplay"></asp:Label>
                                <telerik:RadComboBox ID="cboGatewaySelect" runat="server" RenderMode="Lightweight" CssClass="GatewaySelect" AllowCustomText="true" MarkFirstMatch="true" Filter="StartsWith" AutoPostBack="true" Width="100%"
                                    OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="row form-group" runat="server" id="divDeviceID">
                            <asp:Label ID="lblDeviceID" runat="server" Text="<%$Resources:PageGlobalResources,DeviceIDLabel %>" CssClass="control-label col-sm-4"></asp:Label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblDeviceIDDisplay" CssClass="deviceIDDisplay"></asp:Label>
                                <telerik:RadComboBox ID="cboDeviceIDSelect" runat="server" RenderMode="Lightweight" CssClass="deviceIDSelect" AllowCustomText="true" MarkFirstMatch="true" Filter="StartsWith" Width="100%"
                                    OnClientSelectedIndexChanged="DeviceSelected" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="DeviceSelectedBlurred">
                                </telerik:RadComboBox>
                                <telerik:RadCodeBlock runat="server">
                                    <script type="text/javascript">
                                        function DeviceSelected(sender, args) {

                                            var item = args.get_item();
                                            var type = "";
                                            var zone = "";

                                            if (item) {
                                                itemVal = item.get_value();

                                                if (itemVal) {
                                                    var valParts = itemVal.split('|');

                                                    if (valParts) {
                                                        if (valParts.length > 1) {
                                                            type = valParts[1];
                                                        }
                                                        if (valParts.length > 2) {
                                                            zone = valParts[2];
                                                        }

                                                    }
                                                }
                                            }
                                            DeviceTypeGet(type);
                                            $("[id$='lblZoneDisplay']").text(zone);
                                            $("[id$='lblZoneDisplay']").css("visibility", (zone != "" ? "visible" : "hidden"));
                                            $("[id$='lblDeviceTypeDisplay']").css("visibility", (type != "" ? "visible" : "hidden"));
                                        }

                                        function DeviceSelectedBlurred(sender, args) {
                                            RadComboBlurred(sender, args);
                                            var comboValue = sender.get_value();
                                            var useLike = false

                                            if (comboValue == null || comboValue == "") {
                                                DeviceTypeGet();
                                            }
                                        }

                                        function DeviceTypeGet(type) {

                                            if (type === "ColdAlarm") {
                                                Devicetype = '<%= GetLocalResourceObject("ColdAlarmType")%>';
                                            }
                                            else if (type === "SmokeAlarm") {
                                                Devicetype = '<%= GetLocalResourceObject("SmokeAlarmType")%>';
                                            }
                                            else if (type === "HeatAlarm") {
                                                Devicetype = '<%= GetLocalResourceObject("HeatAlarmType")%>';
                                            }
                                            else if (type === "COAlarm") {
                                                Devicetype = '<%= GetLocalResourceObject("COAlarmType")%>';
                                            }
                                            else if (type === "ACSmokeAlarm") {
                                                Devicetype = '<%= GetLocalResourceObject("ACSmokeAlarmType")%>';
                                            }
                                            else if (type === "ACHeatAlarm") {
                                                Devicetype = '<%= GetLocalResourceObject("ACHeatAlarmType")%>';
                                            }
                                            else if (type === "PadStrobe") {
                                                Devicetype = '<%= GetLocalResourceObject("PadStrobeType")%>';
                                            }
                                            else if (type === "LowFreqSounder") {
                                                Devicetype = '<%= GetLocalResourceObject("LowFreqSounderType")%>';
                                            }
                                            else if (type === "AlarmControlUnit") {
                                                Devicetype = '<%= GetLocalResourceObject("AlarmControlUnitType")%>';
                                            }
                                            else if (type === "InterfaceGateway") {
                                                Devicetype = '<%= GetLocalResourceObject("InterfaceGatewayType")%>';
                                            }
                                            else if (type === "InterfaceGateway200") {
                                                Devicetype = '<%= GetLocalResourceObject("InterfaceGateway200Type")%>';
                                            }
                                            else {
                                                Devicetype = "";
                                            }
        $("[id$='lblDeviceTypeDisplay']").text(Devicetype);
    }
                                    </script>
                                </telerik:RadCodeBlock>
                            </div>
                        </div>
                        <div class="row form-group" runat="server" id="divDeviceType">
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:PageGlobalResources,DeviceTypeLabel %>" CssClass="text-label col-sm-4 text-left"></asp:Label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblDeviceTypeDisplay" CssClass="deviceTypeDisplay"></asp:Label>
                            </div>
                        </div>

                        <div class="row form-group" runat="server">
                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:PageGlobalResources,ZoneLabel %>" CssClass="text-label col-sm-4 text-left"></asp:Label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblZoneDisplay" CssClass="zoneDisplay"></asp:Label>
                            </div>
                        </div>
                        <div id="divNewTicket" runat="server">
                            <div class="row form-group">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,StatusLabel %>" CssClass="control-label col-sm-4 text-left"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="cboStatus" CssClass="form-control inputddl btnSaveEnable" runat="server" DataTextField="StatusDescription" DataValueField="StatusID">
                                    </asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="cboStatus" CssClass="col-sm-8"
                                    ForeColor="Red" Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,StatusRequiredError %>" InitialValue="0"
                                    SetFocusOnError="True" ValidationGroup="Support"></asp:RequiredFieldValidator>
                            </div>
                            <div class="row form-group">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,ReferenceLabel %>" CssClass="control-label col-sm-4 text-left"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtReference" runat="server" CssClass="form-control inputhgt"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <asp:Label ID="lblValidation" runat="server" CssClass="col-sm-12 form-group" ForeColor="Red" Visible="False" EnableViewState="false"></asp:Label>
                            </div>
                        </div>
                        <div class="row form-group" id="divAddrRow">
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:PageGlobalResources,PropertyAddressLabel %>" CssClass="text-label col-sm-4 text-left"></asp:Label>
                            <div id="divAddress" runat="server" class="col-sm-8">
                                <asp:Label runat="server" ID="lblAddress1" CssClass="row col-sm-12" />

                                <asp:Label runat="server" ID="lblAddress2" CssClass="row col-sm-12" />

                                <asp:Label runat="server" ID="lblAddress3" CssClass="row col-sm-12" />
                                <asp:Label runat="server" ID="lblAddress4" CssClass="row col-sm-12" />
                                <asp:Label runat="server" ID="lblPostCode5" CssClass="row col-sm-12" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-8">
                        <div class=" col-sm-6">
                            <asp:TextBox ID="txtNewNotes" CssClass="form-control btnSaveEnable textlength" runat="server" TextMode="MultiLine" Rows="20" Style="resize: none"></asp:TextBox>
                            <%-- <asp:RequiredFieldValidator ID="rfvNewNotes" runat="server" ControlToValidate="txtNewNotes" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,NotesRequiredError %>"
                                SetFocusOnError="True" ValidationGroup="Support"></asp:RequiredFieldValidator>
                            <asp:Label runat="server" Id="lblNoteErrors"></asp:Label>--%>
                        </div>
                        <div class=" col-sm-6">
                            <div style="height: 430px; border: 1px solid #ccc; overflow: scroll">
                                <asp:Label ID="lblAllNotes" Text="" runat="server" />
                            </div>
                            <%--<asp:TextBox ID="txtNotes" CssClass="form-control txtNotes" runat="server" TextMode="MultiLine" Rows="20" ReadOnly="true" Style="resize: none"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Button ID="btnViewAccount" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:PageGlobalResources,AccessAccountButton %>" />
                    </div>
                    <div class="col-sm-6">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-warning btnLogOff validateAll btnSave pull-right" ValidationGroup="Support" Text="<%$Resources:PageGlobalResources,SaveButton %>" disabled="disabled" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 rwpadT35 rwMarTB15">
                        <%--                        <asp:Label ID="Label11" runat="server" Text="<%$Resources:PageGlobalResources,MandatoryFields %>" CssClass="text-label"></asp:Label>--%>
                    </div>
                </div>
            </telerik:RadAjaxPanel>
        </div>
    </div>
</asp:Content>
