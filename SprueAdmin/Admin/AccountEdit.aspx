<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_AccountEdit" CodeBehind="AccountEdit.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            setEventHandlers();

            $('#aspnetForm input').on('keyup change', function () {              
                if ($("#<%=btnSave.ClientID%>").hasClass("btnEdit")) {
                    $("#<%=btnSave.ClientID%>").removeClass("btnEdit");
                    $("#<%=btnSave.ClientID%>").addClass("btn-warning");
                    $("#<%=btnSave.ClientID%>").addClass("btnLogOff");                   
                }
            });

            var ISO3default;
            switch ($('#<%=hdnCulture.ClientID%>').val()) {
                case "nl-NL":
                    ISO3default = "NLD";
                    break;
                case "fr-FR":
                    ISO3default = "FRA";
                    break;
                case "de-DE":
                    ISO3default = "DEU";
                    break;
                default:
                    ISO3default = "GBR";
            }

            //Tag based installations of postcode lookup based on http://www.postcodeanywhere.co.uk
            //Account Address
            var fields = [
                { element: "txtAddress1", field: "Line1", mode: pca.fieldMode.POPULATE },
                { element: "txtAddress2", field: "Line2", mode: pca.fieldMode.POPULATE },
                { element: "txtAddress3", field: "City", mode: pca.fieldMode.POPULATE },
                { element: "txtStateTerritory", field: "ProvinceName", mode: pca.fieldMode.POPULATE },
                { element: "txtPostcode", field: "PostalCode" },
                { element: "txtCountry", field: "CountryName", mode: pca.fieldMode.COUNTRY }
            ],
         
            options = {
                key: '<%=ConfigurationManager.AppSettings("PostcodeAnywhereKeyAccountEdit").ToString()%>', language: 'de',
                countries: {
                    codesList: '<%=ConfigurationManager.AppSettings("CountryList").ToString()%>',
                    defaultCode: ISO3default,
                    nameType: pca.countryNameType.ISO3,
                },
                setCountryByIP: false,
                culture:  $('#<%=hdnCulture.ClientID%>').val()
            }

            control = new pca.Address(fields, options);
               
            control.listen("country", function (address, variations) {
                //get country ISO3 value to save as country name
                $('#<%=hdnCountryISO3.ClientID%>').val(address.iso3);
            
                $.ajax({
                    type: "POST",
                    url: "AccountEdit.aspx/wmGetCountryName",
                    data: '{strCountryISO3: "' + address.iso3 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#<%=txtCountry.ClientID%>').val(data.d);
                    }
                })
            });

            control.listen("populate", function (address, variations) {
                Page_ClientValidate("AccountAddress")
            });

            control.listen("show", function () {
                GetLocalCountryNames()
            });



            //Property Address
            var propfields = [
                    { element: "txtPropertyAddress1", field: "Line1", mode: pca.fieldMode.POPULATE },
                    { element: "txtPropertyAddress2", field: "Line2", mode: pca.fieldMode.POPULATE },
                    { element: "txtPropertyAddress3", field: "City", mode: pca.fieldMode.POPULATE },
                    { element: "txtPropertyStateTerritory", field: "ProvinceName", mode: pca.fieldMode.POPULATE },
                    { element: "txtPropertyPostcode", field: "PostalCode" },
                    { element: "txtPropertyCountry", field: "CountryName", mode: pca.fieldMode.COUNTRY }
            ],
            propoptions = {
                key: '<%=ConfigurationManager.AppSettings("PostcodeAnywhereKeyAccountEditProperty").ToString()%>', countries: { codesList: '<%=ConfigurationManager.AppSettings("CountryList").ToString()%>', defaultCode: ISO3default }, setCountryByIP: false, culture: $('#<%=hdnCulture.ClientID%>').val()
            },
            propcontrol = new pca.Address(propfields, propoptions);

            propcontrol.listen("country", function (address, variations) {
                //get country ISO3 value to save as country name
                $('#<%=hdnPropertyCountryISO3.ClientID%>').val(address.iso3);

                $.ajax({
                    type: "POST",
                    url: "AccountEdit.aspx/wmGetCountryName",
                    data: '{strCountryISO3: "' + address.iso3 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#<%=txtPropertyCountry.ClientID%>').val(data.d);
                    }
                })
            });

            propcontrol.listen("populate", function (address, variations) {
                Page_ClientValidate("Property")
            });

            propcontrol.listen("show", function () {
                GetLocalCountryNames()
            });

        });
     

            function GetLocalCountryNames() {
                //country list is being shown so lets override the names in local language
                $('.pcaflaglabel').each(function (index, value) {
                    $.ajax({
                        type: "POST",
                        url: "AccountEdit.aspx/wmGetLocalCountryName",
                        data: '{strEnglishCountryName: "' + $(this).html() + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {
                                var ret = data.d.split('|')
                                $('.pcaflaglabel').filter(':contains("' + ret[0] + '")').html(ret[1])
                            }
                        }
                    });
                });
            }

            function setCountryValues(listCtl) {
                var pcRFV, pcREV, mREV;
                var idParts = $(listCtl).attr("id").split("_");
                var selCountry = $(listCtl).val()

                switch (idParts[idParts.length - 1]) {
                    case 'txtCountry':
                        pcRFV = $('[id$="valPostcode"]');
                        pcREV = $('[id$="valPostcodeFormat"]');
                        mREV = $('[id$="valPhoneNumber"]');
                        break;
                    case 'txtPropertyCountry':
                        pcRFV = $('[id$="valPropertyPostcode"]');
                        pcREV = $('[id$="valPropertyPostcodeFormat"]');
                        //mREV = null;
                        break;
                }

                if (typeof (pcRFV.prop("ErrorMsg" + $(listCtl).val())) != 'undefined') {
                    pcRFV.text(pcRFV.prop("ErrorMsg" + $(listCtl).val()));
                }

                if (typeof (pcRFV.prop("ValidationExpression" + $(listCtl).val())) != 'undefined') {
                    var aspVal = $get(pcRFV.prop("id"));

                    if (aspVal) {
                        aspVal['validationexpression'] = pcRFV.prop("ValidationExpression" + $(listCtl).val());
                    }

                }

                //pcREV.text($('#hdn' + $(listCtl).val() + 'PostCodeFormatError').val());
                //pcREV.prop('validationexpression', $('#hdn' + $(listCtl).val() + 'PostCodeRegEx').val());

                //if (mREV) {
                //    mREV.text($('#hdn' + $(listCtl).val() + 'MobileFormatError').val());
                //    mREV.prop('validationexpression', $('#hdn' + $(listCtl).val() + 'MobileRegEx').val());
                //}

            }

            function setEventHandlers() {
                $('.countrySelect').each(function () {
                    $(this).change(function (e) {
                        setCountryValues(this);
                        Page_ClientValidate();
                    });
                    setCountryValues(this);
                });
            }

            function CopyToAccount() {
                if (Page_ClientValidate("Property")) {

                    //var agree = confirm($('[id$="hdnPropertyToAccountPrompt"]').val());

                    //if (agree) {
                    $("#<%=txtAddress1.ClientID%>").val($("#<%=txtPropertyAddress1.ClientID%>").val());
                $("#<%=txtAddress2.ClientID%>").val($("#<%=txtPropertyAddress2.ClientID%>").val());
                $("#<%=txtAddress3.ClientID%>").val($("#<%=txtPropertyAddress3.ClientID%>").val());
                $("#<%=txtPostcode.ClientID%>").val($("#<%=txtPropertyPostcode.ClientID%>").val());
                $("#<%=txtCountry.ClientID%>").val($("#<%=txtPropertyCountry.ClientID%>").val());
                $("#<%=txtStateTerritory.ClientID%>").val($("#<%=txtPropertyStateTerritory.ClientID%>").val());
                //setCountryValues($("#<=ddlCountry.ClientID%>"));
                $("#<%=hdnCountryISO3.ClientID%>").val($("#<%=hdnPropertyCountryISO3.ClientID%>").val());

                return true;
                //}
                //else {
                //    return false;
                //}
            }
            else {
                return false;
            }
        }

        function CopyToProperty() {

            if (Page_ClientValidate("AccountAddress")) {
                //var agree = confirm($('[id$="hdnAccountToPropertyPrompt"]').val());

                //if (agree) {
                $("#<%=txtPropertyAddress1.ClientID%>").val($("#<%=txtAddress1.ClientID%>").val());
                $("#<%=txtPropertyAddress2.ClientID%>").val($("#<%=txtAddress2.ClientID%>").val());
                $("#<%=txtPropertyAddress3.ClientID%>").val($("#<%=txtAddress3.ClientID%>").val());
                $("#<%=txtPropertyPostcode.ClientID%>").val($("#<%=txtPostcode.ClientID%>").val());
                $("#<%=txtPropertyCountry.ClientID%>").val($("#<%=txtCountry.ClientID%>").val());
                $("#<%=txtPropertyStateTerritory.ClientID%>").val($("#<%=txtStateTerritory.ClientID%>").val());
                $("#<%=hdnPropertyCountryISO3.ClientID%>").val($("#<%=hdnCountryISO3.ClientID%>").val());

                //setCountryValues($("#<=ddlPropCountry.ClientID%>"));txtStateTerritory
                return true;
                //}
                //else {
                //    return false;
                //}
            }
            else {
                return false;
            }
        }

       
    </script>
    <asp:HiddenField runat="server" ID="hdnPropertyToAccountPrompt" Value="<%$Resources:PropertyToAccountPrompt %>" />
    <asp:HiddenField runat="server" ID="hdnAccountToPropertyPrompt" Value="<%$Resources:AccountToPropertyPrompt %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row bgwhite rwMargLR0 borderBlack">
                <div class=" col-md-12  rwpadT35">
                    <div class="col-md-6">
                        <div class=" col-md-12 rwPadLR0 form-group" id="divAccountId" runat="server">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblAccountID"></asp:Label><br />
                            </div>
                        </div>
                        <div class="col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:PageGlobalResources,FirstNameLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtFirstName" ValidationGroup="Account" CssClass="form-control inputhgt" runat="server" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Account" ID="valFirstName" runat="server" ForeColor="Red" ControlToValidate="txtFirstName" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:PageGlobalResources,LastNameLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtLastName" ValidationGroup="Account" CssClass="form-control inputhgt" runat="server" MaxLength="25"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Account" ID="valLastName" runat="server" ForeColor="Red" ControlToValidate="txtLastName" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:PageGlobalResources,Address1Label %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtAddress1" ValidationGroup="AccountAddress" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="AccountAddress" ID="valAddress1" runat="server" ForeColor="Red" ControlToValidate="txtAddress1" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:PageGlobalResources,Address2Label %>"></asp:Label>

                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtAddress2" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:PageGlobalResources,TownCityLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtAddress3" ValidationGroup="AccountAddress" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="AccountAddress" ID="valAddress3" runat="server"
                                    ForeColor="Red" ControlToValidate="txtAddress3" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:PageGlobalResources,StateTerritoryHeader %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtStateTerritory"  CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:PageGlobalResources,PostCodeLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPostcode" ValidationGroup="AccountAddress" CssClass="form-control inputhgt" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="AccountAddress" ID="valPostcode" runat="server" ForeColor="Red" ControlToValidate="txtPostcode" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:PageGlobalResources,CountryLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <%--<asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control countrySelect inputddl" />--%>
                                <asp:HiddenField ID="hdnCulture" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnCountryISO3" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control inputhgt" ReadOnly="true"> </asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="AccountAddress" ID="valCountry" runat="server" ForeColor="Red" ControlToValidate="txtCountry" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:PageGlobalResources,MobileLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtMobile" ValidationGroup="Account" CssClass="form-control inputhgt" runat="server" MaxLength="25"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="valPhoneNumber" ControlToValidate="txtMobile" ErrorMessage="<%$Resources:InvalidMobileNumberFormat %>"
                                    ForeColor="Red" Display="Dynamic" ValidationExpression="^(|[+0-9 ]+)$" ValidationGroup="Account" Enabled="true" EnableClientScript="true" Width="100%" />
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,EmailLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtEmail" ValidationGroup="Account" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Account" ID="valEmail" runat="server" ForeColor="Red" ControlToValidate="txtEmail" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>

                                <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="txtEmail"
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="Account"
                                    ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" />
                            </div>
                        </div>
                        <%--<asp:Button ID="btnCopyToProperty" runat="server" ValidationGroup="AccountAddress" CssClass="btn btn-primary btn-warning btnLogOff col-md-offset-7" Text="<%$Resources:PageGlobalResources,CopyToPropertyButton %>" OnClientClick="CopyToProperty();return false" />--%>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-8 col-md-offset-3">
                                <input type='checkbox' class='chkinputpad' id="ckbCopyToProperty" onchange="CopyToAccount()" style="right: 35px; width: 23px; height: 21px;" runat="server" />
                                <span>
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:PageGlobalResources,DefaultPropertyAddress %>"></asp:Literal></span>
                            </div>
                        </div>

                    </div>

                    <div id="divProperty" runat="server" class=" col-md-6">
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,PropertyIDLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblPropertyID"></asp:Label>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,Address1Label %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPropertyAddress1" ValidationGroup="Property" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Property" ID="valPropertyAddress1" runat="server" ForeColor="Red" ControlToValidate="txtPropertyAddress1" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,Address2Label %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPropertyAddress2" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,TownCityLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPropertyAddress3" ValidationGroup="Property" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Property" ID="valPropertyAddress3" runat="server" ForeColor="Red" ControlToValidate="txtPropertyAddress3" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:PageGlobalResources,StateTerritoryHeader %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPropertyStateTerritory" CssClass="form-control inputhgt" runat="server" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,PostcodeLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPropertyPostcode" ValidationGroup="Property" CssClass="form-control inputhgt" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Property" ID="valPropertyPostcode" runat="server" ForeColor="Red" ControlToValidate="txtPropertyPostcode" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-3 rwPadLR0">
                                <asp:Label runat="server" Text="<%$Resources:PageGlobalResources,CountryLabel %>"></asp:Label>
                            </div>

                            <div class="col-md-8">
                                <asp:HiddenField ID="hdnPropertyCountryISO3" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtPropertyCountry" runat="server" CssClass="form-control inputhgt" ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Property" ID="valPropertyCountry" runat="server" ForeColor="Red" ControlToValidate="txtPropertyCountry" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <%--<asp:Button ID="btnCopyToAccount" runat="server" CssClass="btn btn-primary btn-warning btnLogOff col-md-offset-7" Text="<%$Resources:PageGlobalResources,CopyToAccountButton %>" OnClientClick="CopyToAccount();return false" />--%>
                        <div class=" col-md-12 form-group rwPadLR0">
                            <div class="col-md-8 col-md-offset-3">
                                <input type='checkbox' class='chkinputpad' id="ckbCopyToAccount" onchange="CopyToProperty()" style="right: 35px; width: 23px; height: 21px;" runat="server" />
                                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:PageGlobalResources,DefaultAccountAddress %>"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 rwMarTB15">
                    <div class="col-md-1 pull-left">
                        <asp:Button ID="btnAddSupport" runat="server" CssClass="btn btn-primary btn-warning btnLogOff" Text="<%$Resources:AddSupport %>" CausesValidation="false" />
                    </div>
                    <div id="divPropertyError" runat="server" class=" col-md-4" visible="false">
                        <asp:Label ID="lblValidation" runat="server" ForeColor="red" Text="<%$Resources:PageGlobalResources,GetPropertyFailure %>"></asp:Label>
                    </div>
                    <div class="col-md-4" runat="server" id="divAdded" visible="false">
                        <asp:Label ID="Label2" CssClass="colorGreen" Text="<%$Resources:AccountSaveSuccess %>" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-1 pull-right">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-default btnEdit validateAll" Text="<%$Resources:PageGlobalResources,SaveButton %>" Style="position: absolute; left: -10px;" />
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
