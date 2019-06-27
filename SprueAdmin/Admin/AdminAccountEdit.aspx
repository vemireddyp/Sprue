<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_AdminAccountEdit" CodeBehind="AdminAccountEdit.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">

    <telerik:RadScriptBlock ID="radScriptBlockPostcode" runat="server">

        <script type="text/javascript">
            $(document).ready(function () {
                $('#aspnetForm input').on('keyup change', function () {
                    if ($("#<%=btnSave.ClientID%>").hasClass("btnEdit")) {
                        $("#<%=btnSave.ClientID%>").removeClass("btnEdit");
                        $("#<%=btnSave.ClientID%>").addClass("btn-warning");
                        $("#<%=btnSave.ClientID%>").addClass("btnLogOff");
                    }
                });
            });

            //Tag based installations of postcode lookup based on http://www.postcodeanywhere.co.uk
            var fields = [
                { element: "<%=txtAddress1.ClientID%>", field: "Line1", mode: pca.fieldMode.POPULATE },
                { element: "<%=txtAddress2.ClientID%>", field: "Line2", mode: pca.fieldMode.POPULATE },
                { element: "<%=txtAddress3.ClientID%>", field: "City", mode: pca.fieldMode.POPULATE },
                { element: "<%=txtStateTerritory.ClientID%>", field: "ProvinceName", mode: pca.fieldMode.POPULATE },
                { element: "<%=txtPostcode.ClientID%>", field: "PostalCode" },
                { element: "<%=txtCountry.ClientID%>", field: "CountryName", mode: pca.fieldMode.COUNTRY }
            ],
                options = {
                    key: '<%=ConfigurationManager.AppSettings("PostcodeAnywhereKeyAdminAccountEdit").ToString()%>', countries: { codesList: '<%=ConfigurationManager.AppSettings("CountryList").ToString()%>', defaultCode: "GBR" }, setCountryByIP: false, culture: $('#<%=hdnCulture.ClientID%>').val()
                },
                control = new pca.Address(fields, options);

            control.listen("country", function (address, variations) {
                //get country ISO3 value to save as country name

                var iso3Hidden = $('#<%=hdnCountryISO3.ClientID%>');
                if (iso3Hidden.val() != address.iso3) {
                    $('#<%=hdnCountryISO3.ClientID%>').val(address.iso3);

                    $.ajax({
                        type: "POST",
                        url: "AdminAccountEdit.aspx/wmGetCountryName",
                        data: '{strCountryISO3: "' + address.iso3 + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var countryTextBox = $find('<%=txtCountry.ClientID%>');
                            if (countryTextBox && data.d && data.d !== "") {
                                countryTextBox.set_value(data.d);
                            }
                        }
                    })
                }
            });

        </script>

    </telerik:RadScriptBlock>

    <telerik:RadCodeBlock runat="server">

        <script type="text/javascript">
            var haveNotSetFocus = true;
            var activeCount = 0;

            $(document).ready(function () {
                var typeCombo = $find($(".modeController").prop("id"));
                if (typeCombo.get_value() == "") {
                    typeCombo.set_text(typeCombo.get_emptyMessage());
                }
                setPageElementClasses(null, typeCombo.get_value());
            });

            function _ValidatorsEnable(control, enabled) {

                enabled = enabled || false;
                if (typeof Page_Validators === 'undefined')
                    return;
                for (var v = 0; v < Page_Validators.length; v++) {
                    var validator = Page_Validators[v];
                    if (validator.controltovalidate == $(control).prop('id')) {
                        validator.enabled = enabled;
                        if (!enabled) {
                            $(validator).hide();
                        }
                    }
                }
            }

            function setEnableState(controlSelector, enabled) {
                $(controlSelector).each(function () {
                    var targCtl = $find($(this).attr('id'));
                    var doEnable = enabled;
                    if ($(this).is('.modeDisabled')) {
                        doEnable = false;
                    }
                    if (targCtl) {
                        try {
                            if (typeof targCtl.set_enabled() == 'undefined') {

                                if (doEnable) {
                                    if (typeof targCtl.enable != 'undefined') {
                                        if (typeof targCtl.enable == 'function') {
                                            targCtl.enable();
                                        }
                                    }
                                }
                                else {
                                    if (typeof targCtl.disable != 'undefined') {
                                        if (typeof targCtl.disable == 'function') {
                                            targCtl.disable();
                                        }
                                    }
                                }
                            }
                            else {
                                if (typeof targtCtl.set_enabled == 'function') {
                                    targCtl.set_enabled(doEnable);
                                }
                            }
                        }
                        catch (e) {
                            console.log(e);
                        }
                    }
                    else {
                        // non 'asp.net' control, assume it's a button
                        try {
                            targCtl = $get($(this).attr('id'));

                            if (targCtl && typeof targCtl.disabled == 'boolean') {
                                targCtl.disabled = !doEnable;
                            }
                            else {
                                if (doEnable) {
                                    $(this).removeAttr('disabled');
                                }
                                else {
                                    $(this).attr('disabled', 'disabled');
                                }
                                // also get any contained input elements
                                $(this).not(".locked").find('input').each(function () {
                                    if (doEnable) {
                                        $(this).removeAttr('disabled');
                                    }
                                    else {
                                        $(this).attr('disabled', 'disabled');
                                    }
                                });
                            }

                        }
                        catch (e) {
                            console.log(e);
                        }
                    }
                    _ValidatorsEnable(this, doEnable);

                });

            }

            var focusCtl;

            function setPageElementClasses(sender, selectedVal) {
                //
                // sender is assumed to be a RadComboBox, event is SelectedIndexChanged

                //disable all modeDependent fields
                var editEnabled = ($("[id$='cboAccountType']").prop("AllowUpdate") == 'yes');

                setEnableState('.modeDependent', false);
                $("[id$='divChangeStatus']").show();
                $("[id$='divAccountIDLabel']").hide();
                $("[id$='divGateways']").hide();

                var setAll = true;

                if (selectedVal) {
                    switch (selectedVal) {
                        case "3":
                            // selected distributor
                            setEnableState('.modeDistributor', true && editEnabled);
                            $(".distDiv").hide();
                            $(".provDiv").hide();
                            $(".instDiv").hide();
                            $("[id$='cboTimeZone']").show();
                            $("[id$='ddlCompanyStatus']").show();
                            $("[id$='txtCompanyName']").show();
                            $("[id$='ddlUserStatus']").hide();
                            $("[id$='divRiskLevel']").show();
                            $("[id$='divAccountStatus']").show();
                            break;

                        case "4":
                            // selected serviceprovider
                            setEnableState('.modeServiceProv', true && editEnabled);
                            $(".provDiv").hide();
                            $(".instDiv").hide();
                            $(".distDiv").show();
                            $("[id$='cboTimeZone']").show();
                            $("[id$='ddlCompanyStatus']").show();
                            $("[id$='txtCompanyName']").show();
                            $("[id$='ddlUserStatus']").hide();
                            $("[id$='divRiskLevel']").hide();
                            $("[id$='divAccountStatus']").show();

                            if (typeof $(".distID").prop("currentMasterCoID") != 'undefined' && $(".distID").prop("currentMasterCoID") != 0) {
                                setEnableState('.coSelected', false);
                            }
                            break;

                        case "E":
                            //selected end user
                            setEnableState('.modeEndUser', true && editEnabled);
                            $(".distDiv").show();
                            $(".provDiv").show();
                            $(".instDiv").show();
                            $("[id$='cboTimeZone']").hide();
                            $("[id$='ddlCompanyStatus']").hide();
                            $("[id$='divCompanyName']").hide();
                            $("[id$='ddlUserStatus']").show();
                            if ($("[id$='lblAccountID']").text() != "") {
                                $("[id$='divAccountIDLabel']").show();
                                setEnableState('.coSelected', false);
                                $("[id$='divGateways']").show();
                            }
                            $("[id$='divRiskLevel']").hide();
                            $("[id$='divAccountStatus']").hide();
                            break;

                        default:

                            setEnableState('.modeSelected', false); // this disables all the 'company' combos
                            $("[id$='divChangeStatus']").hide();
                            haveNotSetFocus = true;
                            setAll = false;
                    }
                }
                else {
                    setAll = false;
                    $("[id$='divChangeStatus']").hide();
                }

                setEnableState('.modeAll', setAll && editEnabled);

                if (setAll && editEnabled) {
                    if (!focusCtl) {
                        focusCtl = $("[id$='txtFirstName']");
                    }
                    if ((typeof $(".distID").prop("currentMasterCoID") != 'undefined' && $(".distID").prop("currentMasterCoID") != 0) || $("[id$='lblAccountID']").text() != "") {
                        $("[id$='btnChangeStatus']").show();
                    }
                    else {
                        $("[id$='btnChangeStatus']").hide();

                        var focusCtl = $find($("[id$='cboDistributors']").prop("id"));
                        if (!focusCtl || (typeof focusCtl.get_items != 'undefined' && (focusCtl.get_items().get_count() < 2 || focusCtl.get_selectedIndex() >= 0))) {
                            var focusCtl = $find($("[id$='cboProviders']").prop("id"));
                        }

                        if (!focusCtl || (typeof focusCtl.get_items != 'undefined' && focusCtl.get_selectedIndex() >= 0)) {
                            var focusCtl = $find($("[id$='cboInstallers']").prop("id"));
                        }

                        if (!focusCtl || (typeof focusCtl.get_items != 'undefined' && focusCtl.get_selectedIndex() >= 0)) {
                            var focusCtl = $find($("[id$='txtFirstName']").prop("id"));
                        }

                        if (focusCtl && typeof focusCtl.get_items != 'undefined' && focusCtl.get_items().get_count() > 1) {
                            focusCtl = focusCtl.get_inputDomElement();
                        }
                    }
                    if (focusCtl) {
                        if (typeof focusCtl.TextBoxElement != 'undefined' && focusCtl.TextBoxElement.TextBoxItem != 'undefined' && focusCtl.TextBoxElement.TextBoxItem.HostedControl != 'undefined'
                            && focusCtl.TextBoxElement.TextBoxItem.HostedControl.Focus != 'undefined') {
                            focusCtl.TextBoxElement.TextBoxItem.HostedControl.Focus();
                        }
                        else {
                            focusCtl.focus();
                        }
                        haveNotSetFocus = false;
                    }
                }



            }

            function setGWDisableAfterPost(enabled, statusCtlID, dPickerID) {
                setEnableState(".gwStatus", false);

            }

            function gatewaySelected(sender, args) {
                if (sender) {
                    var selItem = sender.findItemByText(sender.get_text());
                    $(".divSelectGateway").addClass('hide');

                    var dp = $find($("[id$='dpChangeDate'").prop('id'));
                    var dateInput;

                    $("[id$='lblResultMsg']").text("");

                    if (dp) {
                        dateInput = dp.get_dateInput();
                    }

                    if (dateInput) {
                        dateInput.clear();
                    }

                    var actDate = "";
                    var statCtl = $find($("[id$='cboGatewayStatus']").prop('id'));
                    if (selItem) {
                        var selVal = selItem.get_value();
                        var statCtl = $find($("[id$='cboGatewayStatus']").prop('id'));
                        var selectItem;

                        if (selVal) {
                            var valSplit = selVal.split("|");
                            $(".divSelectGateway").removeClass('hide');

                            if (statCtl) {
                                var existingVal;
                                if (valSplit.length >= 3) {
                                    existingVal = valSplit[2]
                                }

                                if (valSplit.length >= 4 && valSplit[3] != '') {
                                    actDate = new Date(valSplit[3].substring(0, 4) + "/" + valSplit[3].substring(4, 6) + "/" + valSplit[3].substring(6));
                                    selectItem = statCtl.findItemByValue(valSplit[4]);
                                }
                                else if (valSplit.length >= 3) {

                                    if (typeof statCtl.findItemByValue != 'undefined') {
                                        selectItem = statCtl.findItemByValue(existingVal);
                                    }
                                }
                                var disableChange = (existingVal == 'Cancelled')
                            }
                            else {
                                var itemsGateway = sender.get_items();

                                var itemsCount = itemsGateway.length;
                                var ItemStatus = "";

                                for (var itemIndex = 0; itemIndex < itemsCount; itemIndex++) {
                                    var item = itemsGateway[itemIndex];
                                    var itemValue = item.get_value();
                                    var itemSplit = itemValue.split("|");
                                    if (ItemStatus !== itemSplit[itemSplit.length - 1]) {
                                        if (ItemStatus === "") {
                                            ItemStatus = itemSplit[itemSplit.length - 1];
                                        }
                                        else {
                                            ItemStatus = "Active";
                                            break;
                                        }
                                    }
                                }

                                if (ItemStatus !== "") {
                                    selectItem = statCtl.findItemByValue(ItemStatus);
                                }
                            }

                            if (!selectItem) {
                                selectItem = statCtl.findItemByValue('Active');
                            }

                            if (selectItem) {
                                statCtl.set_selectedIndex(selectItem.get_index());
                                statCtl.set_text(selectItem.get_text());
                            }

                        }
                        setEnableState(".gwStatus", !disableChange);
                    }
                    if (dp) {
                        if (actDate != "") {
                            dp.set_selectedDate(actDate);
                            if (dateInput) {
                                dateInput.set_value(actDate);
                            }
                        }
                        else if (dateInput) {
                            dateInput.clear();
                        }
                    }
                }
            }

            function pageModeSelected(sender, args) {
                var itemVal;
                var item;
                if (args) {
                    if (item = args.get_item()) {
                        item = args.get_item();
                    }
                    else {
                        item = sender;
                    }
                }

                if (item) {
                    setPageElementClasses(sender, item.get_value());

                }
            }

            function pageModeExited(sender, args) {
                RadComboBlurred(sender, args);
                setPageElementClasses(sender, sender.get_value());
            }

            function gwChangeStatus(sender) {
                var doUpdate = true;
                var gwCombo = $find($("[id$='cboGateways']").prop('id'));
                var isInstaller = <%=Me.isInstaller%>;

                var oldVal = 'active';
                var intPlusMinus = 0;
                var selItem;
                if (gwCombo) {

                    var selText = gwCombo.get_text();

                    selItem = gwCombo.findItemByText(gwCombo.get_text());

                    if (selItem) {
                        var selValue = selItem.get_value();

                        if (selValue) {
                            var selVals = selValue.split('|');

                            if (selVals) {
                                if (selVals.length > 2) {
                                    oldVal = selVals[2].toLowerCase();
                                }
                                if (selVals.length > 4) {
                                    oldVal = selVals[4].toLowerCase();
                                }
                            }
                        }
                    }
                }

                var dpActivateDate = $find($("[id$='dpChangeDate']").prop('id'));

                if (dpActivateDate && typeof dpActivateDate.get_selectedDate != 'undefined') {

                    var activateDate = dpActivateDate.get_selectedDate();

                    if (activateDate) {
                        var jqStatCtl = $("[id$='cboGatewayStatus']");

                        var statCtl = $find(jqStatCtl.prop('id'));

                        if (statCtl) {
                            var newItem = statCtl.findItemByText(statCtl.get_text());

                            if (newItem) {
                                var newVal = newItem.get_value().toLowerCase();

                                if (oldVal == 'active' && newVal != 'active') {
                                    intPlusMinus = -1;
                                }
                                else {
                                    if (newVal == 'active') {
                                        intPlusMinus = 1;
                                    }
                                }

                                var currentCount = 0;

                                var comboElem = $(gwCombo.get_element());

                                if (activeCount && !isNaN(activeCount)) {
                                    currentCount = parseInt(activeCount);
                                }

                                currentCount += intPlusMinus;

                                if (currentCount < 1) {
                                    doUpdate = false;
                                    //SP-1216
                                    if (isInstaller) {
                                        $("[id$='ModalLastGatewayInstaller']").modal('show');
                                    }
                                    else {
                                        $("[id$='ModalLastGateway']").modal('show');
                                    }
                                }
                                else if (selItem && selItem.get_index() === 0) {
                                    doUpdate = false;
                                    if (newVal == 'active') {
                                        $("[id$='ModalAllGatewaysActive']").modal('show');
                                    }
                                    if (isInstaller) {
                                        $("[id$='ModalLastGatewayInstaller']").modal('show');
                                    }
                                    else {
                                        $("[id$='ModalAllGateways']").modal('show');
                                    }
                                }

                            }
                        }
                    }
                }

                return doUpdate;

            }

            function CloseModal(ModalName) {
                $('[id="' + ModalName + '"').modal('hide');
            }

        </script>
    </telerik:RadCodeBlock>
    <div class="row bgwhite rwMargLR0 borderBlack">
        <div class=" col-md-12 margin-top15">
            <asp:Panel runat="server" ID="pnlDetails" class="col-md-6">
                <div class="col-md-12 form-group rwPadLR0">
                    <div id="divAccountIDLabel" class="rwMarTB10">
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>" />
                        <asp:Label ID="lblAccountID" runat="server" />
                    </div>
                    <telerik:RadComboBox RenderMode="Lightweight" ID="cboAccountType" runat="server" CssClass="modeController" InputCssClass="inputhgt" EmptyMessage="<%$Resources:PageGlobalResources,AccountTypeHeader %>"
                        OnClientSelectedIndexChanged="pageModeSelected" OnClientBlur="pageModeExited"
                        MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="100%" ValidationGroup="MainAccount" OnClientKeyPressing="RadComboKeyPress">
                        <Items>
                            <telerik:RadComboBoxItem Value="3" runat="server" Text="<%$Resources:PageGlobalResources,DistributorText %>"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="4" runat="server" Text="<%$Resources:PageGlobalResources,ServiceProText %>"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="E" runat="server" Text="<%$Resources:PageGlobalResources,EndUserText %>"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-md-12 form-group rwPadLR0 distDiv">
                    <telerik:RadComboBox RenderMode="Lightweight" ID="cboDistributors" runat="server" CssClass="inputhgt modeDependent modeServiceProv modeEndUser coSelected distID" InputCssClass="inputhgt" AutoPostBack="true" EmptyMessage="<%$Resources:PageGlobalResources,DistributorText %>"
                        MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="100%" ValidationGroup="MainAccount" CausesValidation="false" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                    <asp:RequiredFieldValidator ID="valDistributors" runat="server" ForeColor="Red" ControlToValidate="cboDistributors"
                        ErrorMessage="<%$Resources:ValidationCtlResources,DistributorRequiredMsg %>" SetFocusOnError="True" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-12 form-group rwPadLR0 provDiv">
                    <telerik:RadComboBox RenderMode="Lightweight" ID="cboProviders" runat="server" CssClass="inputhgt modeDependent modeEndUser coSelected" InputCssClass="inputhgt" EmptyMessage="<%$Resources:PageGlobalResources,ServiceProText %>"
                        MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="100%" ValidationGroup="MainAccount" AutoPostBack="true" CausesValidation="false" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                </div>
                <div class="col-md-12 form-group rwPadLR0 instDiv">
                    <telerik:RadComboBox RenderMode="Lightweight" ID="cboInstallers" runat="server" CssClass="modeDependent inputhgt modeEndUser coSelected" InputCssClass="inputhgt" EmptyMessage="<%$Resources:PageGlobalResources,AllInstallersText %>"
                        MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="100%" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                </div>
                <div class="col-md-12 form-group rwPadLR0">
                    <div class=" col-md-6 txtHeight padL0">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtFirstName" runat="server" CssClass="form-control inputhgt modeAll" MaxLength="20" EmptyMessage="<%$Resources:PageGlobalResources,FirstNameHeader %>"
                            Width="100%" ValidationGroup="MainAccount" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false">
                        </telerik:RadTextBox>
                    </div>
                    <div class="col-md-6 txtHeight rwPadLR0">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtLastName" runat="server" CssClass="form-control inputhgt modeAll" MaxLength="25" EmptyMessage="<%$Resources:PageGlobalResources,LastNameHeader %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                    </div>
                    <div class="col-md-6 rwPadLR0">
                        <asp:RequiredFieldValidator ID="valFirstName" runat="server" ForeColor="Red" ControlToValidate="txtFirstName"
                            ErrorMessage="<%$Resources:ValidationCtlResources,FirstNameRequiredMsg %>" SetFocusOnError="True" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-6 rwPadLR0">
                        <asp:RequiredFieldValidator ID="valLastName" runat="server" ForeColor="Red" ControlToValidate="txtLastName"
                            ErrorMessage="<%$Resources:ValidationCtlResources,LastNameRequiredMsg %>" SetFocusOnError="True" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight" runat="server" id="divCompanyName">
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtCompanyName" runat="server" MaxLength="60" CssClass="form-control inputhgt modeDependent modeServiceProv modeDistributor col-md-12"
                        EmptyMessage="<%$Resources:PageGlobalResources,CoNameHeader %>" Width="100%" ValidationGroup="MainAccount">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="valCompanyName" runat="server" ForeColor="Red" ControlToValidate="txtCompanyName" SetFocusOnError="True"
                        ErrorMessage="<%$Resources:ValidationCtlResources,CoNameRequiredMsg %>" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-12 form-group rwPadLR0">
                    <div class="col-md-6 txtHeight padL0">
                        <asp:HiddenField ID="hdnCulture" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hdnCountryISO3" runat="server"></asp:HiddenField>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtCountry" runat="server" CssClass="form-control inputhgt modeAll" EmptyMessage="<%$Resources:PageGlobalResources,CountryHeader %>" Width="100%" ValidationGroup="MainAccount" ReadOnly="true"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ValidationGroup="MainAccount" ID="valCountry" runat="server" ForeColor="Red" ControlToValidate="txtCountry" ErrorMessage="<%$Resources:ValidationCtlResources,CountryRequiredMsg %>" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-6 txtHeight rwPadLR0">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtPostcode" runat="server" CssClass="form-control inputhgt modeAll" EmptyMessage="<%$Resources:PageGlobalResources,PostCodeLabel %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="valPostcode" runat="server" CssClass="valPostcode" ForeColor="Red" ControlToValidate="txtPostcode"
                            ErrorMessage="<%$Resources:ValidationCtlResources,PostCodeRequiredMsg %>" SetFocusOnError="True" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                        <%--      <asp:RegularExpressionValidator ID="valPostcodeFormat" runat="server" ControlToValidate="txtPostcode" ForeColor="Red"
                            ErrorMessage="<%$Resources:ValidationCtlResources,PostCodeFormatMsg %>" Display="Dynamic" ValidationExpression="^\D+\d[\s]*[\D\d]*$" Width="100%" ValidationGroup="MainAccount" />--%>
                    </div>
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight ">
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress1" runat="server" MaxLength="60" CssClass="form-control inputhgt modeAll col-md-12" EmptyMessage="<%$Resources:PageGlobalResources,Address1Header %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="valAddress1" runat="server" ForeColor="Red" ControlToValidate="txtAddress1" SetFocusOnError="True"
                        ErrorMessage="<%$Resources:ValidationCtlResources,Address1RequiredMsg %>" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight">
                    <telerik:RadTextBox ID="txtAddress2" runat="server" MaxLength="60" CssClass="form-control inputhgt modeAll col-md-12" EmptyMessage="<%$Resources:PageGlobalResources,Address2Header %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight">
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress3" runat="server" CssClass="form-control inputhgt modeAll col-md-12" MaxLength="60" EmptyMessage="<%$Resources:PageGlobalResources,TownCityHeader %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="valAddress3" runat="server"
                        ForeColor="Red" ControlToValidate="txtAddress3" SetFocusOnError="True" Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,TownCityRequiredMsg %>" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight">
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtStateTerritory" runat="server" CssClass="form-control inputhgt modeAll col-md-12" MaxLength="60" EmptyMessage="<%$Resources:County %>" Width="100%"></telerik:RadTextBox>
                </div>
                <div class="col-md-12 form-group rwPadLR0 txtHeight">
                    <telerik:RadComboBox RenderMode="Lightweight" ID="cboTimeZone" runat="server" CssClass="inputhgt modeDependent modeServiceProv modeDistributor" InputCssClass="inputhgt" EmptyMessage="<%$Resources:PageGlobalResources,TimeZoneText %>"
                        MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="false" Width="100%" CausesValidation="True" />
                    <asp:RequiredFieldValidator ID="valTimeZone" runat="server" ForeColor="Red" ControlToValidate="cboTimeZone"
                        ErrorMessage="<%$Resources:ValidationCtlResources,TimeZoneRequiredMsg %>" SetFocusOnError="True" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight">
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtLandLine" runat="server" CssClass="form-control inputhgt modeAll col-md-12" MaxLength="15" EmptyMessage="<%$Resources:TelephoneNumber  %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                    <asp:RegularExpressionValidator ValidationExpression="^(|[+0-9 ]+)$" ValidationGroup="MainAccount" ID="valContactNumberFormat" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:InvalidLandNumberFormat %>" CssClass="RedNoticeText" ControlToValidate="txtLandLine" Display="Dynamic" />
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight" id="divRiskLevel" style="display: none;">
                    <asp:CheckBox ID="chkRiskLevel" runat="server" Text="<%$Resources:PageGlobalResources,AllowRiskLevelsQ %>" CssClass="modeDependent modeServiceProv modeDistributor control-label" TextAlign="Left" />

                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight">
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtEmail" runat="server" MaxLength="50" CssClass="form-control inputhgt modeAll col-md-12" EmptyMessage="<%$Resources:EmailAddressProperty %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="valEmail" runat="server" ForeColor="Red" ControlToValidate="txtEmail" SetFocusOnError="True"
                        ErrorMessage="<%$Resources:ValidationCtlResources,EmailRequiredMsg %>" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="txtEmail"
                        ForeColor="Red" Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,EmailRequiredMsg %>"
                        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ValidationGroup="MainAccount" />
                </div>
                <div class=" col-md-12 form-group rwPadLR0 txtHeight">
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtMobile" runat="server" CssClass="form-control inputhgt modeAll col-md-12" MaxLength="15" EmptyMessage="<%$Resources:MobileNumber %>" Width="100%" ValidationGroup="MainAccount"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="valMobileNumber" runat="server" ForeColor="Red" ControlToValidate="txtMobile" SetFocusOnError="True"
                        ErrorMessage="<%$Resources:ValidationCtlResources,MobileNumberReq %>" Display="Dynamic" Width="100%" ValidationGroup="MainAccount"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="valPhoneNumber" ControlToValidate="txtMobile" ErrorMessage="<%$Resources:InvalidMobileNumberFormat %>"
                        ForeColor="Red" Display="Dynamic" ValidationExpression="^(|[+0-9 ]+)$" ValidationGroup="MainAccount" Enabled="true" EnableClientScript="true" Width="100%" />
                </div>
            </asp:Panel>
            <div class="col-md-6" runat="server" id="divChangeStatus" style="padding-left: 35px;">
                <div class="row" runat="server" id="divAccountStatus">
                    <div class="col-md-3 padL0">
                        <asp:Label ID="lblStatus" runat="server" Text="<%$Resources:PageGlobalResources,AccountStatusLabel %>" CssClass="control-label"></asp:Label>
                    </div>
                    <div class="col-md-4" style="margin-left: -.7em">
                        <asp:DropDownList ID="ddlCompanyStatus" runat="server" CssClass="form-control inputhgt" ValidationGroup="ChangeStatus">
                            <asp:ListItem Text="<%$Resources:StatusResources,ActiveList %>" Value="Active" />
                            <asp:ListItem Text="<%$Resources:StatusResources,DisabledList %>" Value="Disabled" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-5 pull-right">
                        <asp:Button ID="btnChangeStatus" runat="server" CssClass="btn btn-primary btn-warning btnLogOff modeAll pull-right" Text="<%$Resources:PageGlobalResources,ChangeStatusButton %>" ValidationGroup="ChangeStatus" />
                    </div>
                </div>
                <div id="divGateways" class="row" runat="server">
                    <div class="row rwPadTB9">
                        <div class="col-md-12 padL0">
                            <div class="col-md-3">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,GatewayLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="cboGateways" runat="server" CssClass=" inputhgt gwSelector" InputCssClass="inputhgt" EmptyMessage="<%$Resources:PageGlobalResources,SelectGateway %>"
                                    MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="25em" Style="margin-left: .3em" CausesValidation="false" OnClientSelectedIndexChanged="gatewaySelected" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                            </div>
                        </div>
                    </div>
                    <div id="divSelectGateway" class="divSelectGateway" runat="server">
                        <div class="row rwPadTB9">
                            <div class="col-md-3">
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:PageGlobalResources,GatewayChangeOnLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <telerik:RadComboBox ID="cboGatewayStatus" RenderMode="Lightweight" runat="server" CssClass="inputhgt gwStatus" Width="12.5em" Enabled="true" CausesValidation="false">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Active %>" Value="Active" />
                                        <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Suspended %>" Value="Suspended" />
                                        <telerik:RadComboBoxItem Text="<%$Resources:PropertyStatusResources,Cancelled %>" Value="Cancelled" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-md-offset-3">
                                <telerik:RadDatePicker RenderMode="Lightweight" ID="dpChangeDate" runat="server" CssClass="inputhgt gwStatus" DateInput-CssClass="inputhgt gwStatus" DatePopupButton-CssClass="inputhgt gwStatus rcCalPopup" DateInput-Width="100%" Width="10.7em" ValidationGroup="GatewayStatus" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 rwMarTB15 col-md-offset-3">
                                <asp:Button ID="btnChangeStatusOn" runat="server" CssClass="btn btn-primary btn-warning btnLogOff gwStatus rwMarTB15" OnClientClick="if(!gwChangeStatus(this)) return false;" Text="<%$Resources:PageGlobalResources,SetChangeText %>"
                                    ValidationGroup="GatewayStatus" />
                            </div>
                            <asp:Label ID="lblResultMsg" runat="server" CssClass="col-md-12 rwMarTB15" ForeColor="Red" EnableViewState="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12 rwMarTB15">
            <div class="col-md-4" runat="server" id="divAdded" visible="false">
                <asp:Label ID="Label2" CssClass="colorGreen" Text="<%$Resources:AccountSaveSuccess %>" runat="server"></asp:Label>
            </div>
            <div class="col-md-1 pull-left">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-default btnEdit modeAll" Text="<%$Resources:PageGlobalResources,CreateAccountButton %>" ValidationGroup="MainAccount" />
            </div>
        </div>
    </div>
    <div class="modal fade" id="ModalLastGateway" runat="server" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <label class="lblfont16">
                                        <asp:Literal ID="litGWWarning" runat="server" Text="<%$Resources:LastGatewayWarning%>" />
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-10 col-md-offset-1 text-center">
                                <asp:Literal ID="litContinuePrompt" runat="server" Text="<%$Resources:ContinuePrompt%>" />
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <asp:Button ID="btnLastGWCancel" runat="server" CssClass="btn btn-warning btnLogOff pull-left" Text='<%$Resources:PageGlobalResources,CancelButton%>' OnClientClick="CloseModal('ModalLastGateway');" />
                                <asp:Button ID="btnLastGWOK" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text='<%$Resources:PageGlobalResources,OKText%>' />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- Last gateway info For installer SP-1216--%>
    <div class="modal fade  ModalLastGatewayInstaller" id="ModalLastGatewayInstaller" runat="server" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <label class="lblfont16">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:InstallerCancelWarning%>" />
                                    </label>
                                </div>
                            </div>

                            <div class="pull-right col-md-2 text-center">
                                <asp:Button ID="btnOkayInstaller" runat="server" data-dismiss="modal" CssClass="btn btn-warning btnLogOff pull-left buttonInstaller" Text='<%$Resources:PageGlobalResources,OKText%>' />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalAllGateways" runat="server" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <label class="lblfont16">
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:AllGatewayCancelWarning%>" />
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-10 col-md-offset-1 text-center">
                                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:ContinuePrompt%>" />
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-warning btnLogOff pull-left" Text='<%$Resources:PageGlobalResources,CancelButton%>' OnClientClick="CloseModal('divAllGateways');" />
                                <asp:Button ID="btnModalAllGateways" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text='<%$Resources:PageGlobalResources,OKText%>' />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalAllGatewaysActive" runat="server" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <label class="lblfont16">
                                        <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:AllGatewayActiveWarning%>" />
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-10 col-md-offset-1 text-center">
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:ContinuePrompt%>" />
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <asp:Button ID="Button2" runat="server" CssClass="btn btn-warning btnLogOff pull-left" Text='<%$Resources:PageGlobalResources,CancelButton%>' OnClientClick="CloseModal('divAllGateways');" />
                                <asp:Button ID="btnModalAllGatewaysActive" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text='<%$Resources:PageGlobalResources,OKText%>' />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
