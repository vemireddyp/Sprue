<%@ Page Language="vb" MasterPageFile="~/TelerikCtlContent.master" AutoEventWireup="false" CodeBehind="HubReset.aspx.vb" Inherits="SprueAdmin.HubReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <script type="text/javascript">


        function confirm_update() {
            if (confirm("Are you sure you to update the gateway with this version of the firmware?") == true)
                return true;
            else
                return false;

        }

    </script>

    <div id="divGatewayReset">
        <div class="col-md-12 rwPadTB9 rwPadR7 bgTheme" style="margin-top: 10px; color: white; font-weight: bold;">
            <div class="nav navbar-nav col-md-4 col-md-4-left">
                <asp:Label ID="lblHead" runat="server"></asp:Label>
            </div>
        </div>

        <div class="col-md-12 rwPadLR0 borderBlack" style="background-color: #FFFFFF; padding: 10px; font-weight: bold">

            <div class="col-md-12">

                <asp:UpdatePanel ID="updProgressBar" runat="server" UpdateMode="Conditional" class="container bgWhite pad15">
                    <ContentTemplate>

                        <div class="col-md-8 rwPadLR0">
                            <div class="col-md-12 rwPadLR0">
                                <div class="col-md-4">
                                    <asp:Label runat="server" ID="lblFirmware" Text="<%$Resources:PageGlobalResources,MACAddressLabel %>" CssClass="control-label" />

                                </div>
                                <div class="col-md-8">
                                    <asp:Label runat="server" ID="lblMacAddress" CssClass="txtBoxLight" />
                                </div>
                            </div>
                            <div class="col-md-12 rwPadLR0 rwPadTB15">
                                <div class="col-md-4">
                                    <asp:Button ID="btnGatewayReboot" runat="server" CssClass="btn btn-default btnEdit btncollection" Text="<%$Resources:PageGlobalResources,ResetGatewayButton %>" UseSubmitBehavior="false" />
                                </div>
                                <div class="col-md-8">
                                    <asp:Label runat="server" ID="lblGatewayReboot" CssClass="lblfont12" Text="<%$Resources:PageGlobalResources,GatewayRebootDescription %>"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 margin-top5">
                            <div id="divProgressBar" runat="server" class="progress" style="width: 1050px; display: none; border: 1px solid black; min-height: 22px;">
                                <div id="divProgressPercent" runat="server" class="progress-bar progress-bar-success active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%; color: black">
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="tmrProgressBar" EventName="Tick" />
                        <asp:AsyncPostBackTrigger ControlID="btnGatewayReboot" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>


        <asp:Timer ID="tmrProgressBar" runat="server" Enabled="False" Interval="5000"></asp:Timer>

    </div>
</asp:Content>
