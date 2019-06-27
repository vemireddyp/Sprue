<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_AccountDetail" CodeBehind="AccountDetail.aspx.vb" %>

<%@ Register Src="~/UserControls/Dashboard.ascx" TagPrefix="uc1" TagName="Dashboard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            function confirmResetPanelUpdate() {
                if (confirm(document.getElementById('<%= hdnConfirmResetPanelMessage.ClientID%>').value) == true)
                    return true;
                else
                    return false;
            }
            function confirmFactoryResetPanelUpdate() {
                if (confirm(document.getElementById('<%= hdnConfirmFactoryResetPanelMessage.ClientID%>').value) == true)
                return true;
            else
                return false;
        }
        function resetPasswordSent() {
            $('#divPasswordResetEmail').show();
        }
        </script>
    </telerik:RadCodeBlock>
    <asp:HiddenField runat="server" ID="hdnConfirmResetPanelMessage" />
    <asp:HiddenField runat="server" ID="hdnConfirmFactoryResetPanelMessage" />

    <!---------------------Second Row---------------------------------------------------------------------------->
    <div class="container rwPLR45 rwPadLR0">
        <!--row rwPT9-->
        <div class="col-md-12 rwPT10B7 bgTheme margin-top15">
            <div class="row">
                <div class="col-md-4">
                    <asp:Label  Text="<%$Resources:Distributor %>"  ForeColor="white" runat="server" />
                    <asp:Label ID="lblDistributor" ForeColor="white" runat="server" />
                </div>
                <div class="col-md-4">
                    <asp:Label Text="<%$Resources:ServiceProvider %>" ForeColor="white" runat="server" />
                    <asp:Label ID="lblServiceProvider" ForeColor="white" runat="server" />
                </div>
                <div class="col-md-4">
                    <asp:Label Text="<%$Resources:Installer %>"   ForeColor="white" runat="server" />
                    <asp:Label ID="lblInstaller" ForeColor="white" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <!---------------------Second Row---------------------------------------------------------------------------->
    <!---------------------Third Row---------------------------------------------------------------------------->
    <div class="container rwPLR45 rwPadLR0 margin-top10">
        <div class="col-md-12 rwPT10B7 bgTheme">
            <div class="row">
                <div class="col-md-3">
                    <asp:Label runat="server" ID="lblAccountNameHeader" ForeColor="white" Text="<%$Resources:AccountName %>"></asp:Label>
                    <asp:Label runat="server" ID="lblAccountName" ForeColor="white" ></asp:Label>
                </div>
                <div class="col-md-3 col-md-offset-1">
                    <asp:Label runat="server" ID="Label11" ForeColor="white"  Text="<%$Resources:PropertyName %>"></asp:Label><br />
                    <telerik:RadComboBox ID="cboProperties" RenderMode="Lightweight" runat="server" Style="width: 100%" MaxHeight="150px" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                        EmptyMessage="<%$Resources:PageGlobalResources,PropertiesNameText %>" MarkFirstMatch="true" AllowCustomText="false" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="AccountsComboBlurred" />
                        <telerik:RadCodeBlock runat="server">
                            <script type="text/javascript">
                                function AccountsComboBlurred(sender, args) {
                                    RadComboBlurred(sender, args);
                                    if (sender.get_text().trim() == "") {
                                        __doPostBack($(sender.get_element()).prop('id'), '');
                                    }
                                }
                            </script>
                        </telerik:RadCodeBlock>
                </div>
                <div class="col-md-3  col-md-offset-1">
                    <asp:Label runat="server" ID="Label10" ForeColor="White" Text="<%$Resources:Gateway %>"></asp:Label><br />
                    <telerik:RadComboBox ID="cboGateways" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                        EmptyMessage="<%$Resources:PageGlobalResources,GatewaysNameText %>" MarkFirstMatch="true" AllowCustomText="false" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="AccountsComboBlurred" />
                </div>
            </div>
        </div>
    </div>

    <!------------------------------------------------------Third Row----------------------------------->
    <div class="container rwPLR45 rwPadLR0">
        <div class="col-md-12 rwPT10B7 bgTheme">
            <div class="row">
                <div class="col-md-3">
                    <span class="txtcolor lblfont13">
                        <asp:Label runat="server" ID="Label7" Text="<%$Resources:PageGlobalResources,AccountAddress %>"></asp:Label><br />
                        <asp:Literal ID="lblAccountAddress" runat="server" Text="" />
                    </span>
                </div>

                <div class="col-md-3 col-md-offset-1">
                    <span class="txtcolor lblfont13">
                        <asp:Label runat="server" ID="Label8" Text="<%$Resources:PageGlobalResources,PropertyAddress %>"></asp:Label>:<br />
                        <asp:Literal ID="lblPropertyAddress" runat="server" Text="" />
                    </span>
                </div>
                <div class="col-md-3  col-md-offset-1">
                    <asp:Label runat="server" ID="Label9" ForeColor="White" Text="<%$Resources:Zone %>"></asp:Label><br />
                    <telerik:RadComboBox ID="cboZones" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                        EmptyMessage="<%$Resources:PageGlobalResources,ZonesText %>" MarkFirstMatch="true" AllowCustomText="true" Visible="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="AccountsComboBlurred" />

                </div>
            </div>
        </div>
    </div>


    <div class="container rwPLR45 rwPadLR0">
        <div class="col-md-12 rwPT10B7 bgTheme">
            <div class="row">
                <div class="col-md-3">
                    <asp:Label runat="server" ID="lblAccountIDHeader" CssClass="txtcolor rwPT12 lblfont13" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>"></asp:Label>
                    <asp:Label runat="server" ID="lblAccountIDValue" CssClass="txtcolor rwPT12 lblfont13"></asp:Label>
                </div>
                <%--  <div class="col-md-3  col-md-offset-5">
                        <telerik:RadComboBox ID="cboRiskLevels" RenderMode="Lightweight" runat="server" Style="width: 100%" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                            EmptyMessage="<%$Resources:PageGlobalResources,RiskLevelsText %>" MarkFirstMatch="true" AllowCustomText="true" Visible="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                    </div>
                    <div class="col-md-1 rwPadLR0">
                        <asp:Button ID="btnRiskSetChange"  CssClass="btn btn-warning btnLogOff" runat="server" style="padding-left: 4px;padding-right: 4px;padding-bottom: 3px;padding-top: 3px;" Text="<%$Resources:PageGlobalResources,SetChangeText %>" />
                   </div>--%>
            </div>
        </div>
    </div>
    <!------------------------------------------------------Third Row----------------------------------->
    <div class="container rwPLR45">
        <uc1:Dashboard runat="server" ID="Dashboard" />
    </div>
    <!------------------------------------------------------Fifth Row----------------------------------->

    <div class="container bgwhite rwMargLR0  rwPadLR0 borderBlack">
        <div class="col-md-12 rwPT10B7 bgTheme ">
            <asp:Label runat="server" ID="lblGoto" CssClass="lblMgeSensor" Text="<%$Resources:PageGlobalResources,GoTo %>"></asp:Label>
        </div>

        <div class="row rwPT9 bgWhite">
            <div class="col-md-12 rwMargT15">
                <div class="col-md-12 form-group">
                    <div class="col-md-1">
                        <asp:Button ID="btnSensorsManage" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,ManageSensorsButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="Label6" Text="<%$Resources:PageGlobalResources,ManageSensorsDescription %>"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnEventLog" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,EventLogButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="Label1" Text="<%$Resources:PageGlobalResources,AlertsAndFaultsDescription %>"></asp:Label>
                    </div>
                </div>

                <div class="col-md-12 form-group">
                    <div class="col-md-1">
                        <asp:Button ID="btnSwapGateway" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,SwapGatewayButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="lblReplacementGatewayDescription" Text="<%$Resources:PageGlobalResources,ReplacementGatewayDescription %>"></asp:Label>
                    </div>

                    <div class="col-md-1">
                        <asp:Button ID="btnContacts" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,ContactsButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="Label2" Text="<%$Resources:PageGlobalResources,ContactsDescription %>"></asp:Label>
                    </div>

                </div>

                <div class="col-md-12 form-group">
                    <div class="col-md-1">
                        <asp:Button ID="btnGatewayReboot" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,ResetGatewayButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="Label5" Text="<%$Resources:PageGlobalResources,GatewayRebootDescription %>"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnSupportLog" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,SupportLogButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="lblSupportLogDescription" Text="<%$Resources:PageGlobalResources,SupportLogDescription %>"></asp:Label>
                    </div>
                </div>

                <div class="col-md-12 form-group">
                    <asp:PlaceHolder ID="placeFirmwareUpdate" runat="server" Visible="false">
                        <div class="col-md-1">
                            <asp:Button ID="btnFirmware" runat="server" CssClass="btn btn-default btnEdit mainButton"
                                Text="<%$Resources:PageGlobalResources,FirmwareButton %>" UseSubmitBehavior="false" />
                        </div>
                        <div class="col-md-4 col-md-offset-1 marT7">
                            <asp:Label runat="server" ID="lblUpgradeGatewayDescription" Text="<%$Resources:PageGlobalResources,UpgradeGatewayDescription %>"></asp:Label>
                        </div>
                    </asp:PlaceHolder>
                    <div class="col-md-1">
                        <asp:Button ID="btnRaiseSupportLog" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,RaiseSupportLogButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="lblRaiseSupportLogDescription" Text="<%$Resources:PageGlobalResources,RaiseSupportLogDescription %>"></asp:Label>
                    </div>
                </div>
                <div class="col-md-12 form-group">
                    <div class="col-md-1">
                        <asp:Button ID="btnAccountEdit" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,EditAccountButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="Label3" Text="<%$Resources:PageGlobalResources,EditAccountDescription %>"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnAudits" runat="server" CssClass="btn btn-default btnEdit mainButton"
                            Text="<%$Resources:PageGlobalResources,AuditLogsButton %>" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-4 col-md-offset-1 marT7">
                        <asp:Label runat="server" ID="Label4" Text="<%$Resources:PageGlobalResources,AuditDescription %>"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
