<%@ Page Language="VB" MasterPageFile="~/TelerikCtlContent.master" AutoEventWireup="false" Inherits="SprueAdmin.Admin_HubFirmware" CodeBehind="HubFirmware.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <script type="text/javascript">


        function confirm_update(message) {
            if (confirm(message) == true)
                return true;
            else
                return false;

        }

    </script>

    <%-- Firmware Update Section Start --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divFirmware">
                <div class="col-md-12 rwPadTB9 rwPadR7 bgTheme" style="margin-top: 10px; color: white; font-weight: bold;">
                    <div class="nav navbar-nav col-md-4 col-md-4-left">
                        <asp:Label ID="lblHead" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="col-md-12 rwPadLR0 borderBlack" style="background-color: #FFFFFF; padding: 10px;">

                    <div class="col-md-5 rwPadLR0">
                        <div class="col-md-12 rwPadLR0">
                            <div class="col-md-4">
                                <asp:Label runat="server" ID="lblFirmware" Text="<%$Resources:PageGlobalResources,FirmwareCurrentLabel %>" CssClass="control-label" />

                            </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblFirmwareVersion" CssClass="txtBoxLight" />
                            </div>
                        </div>

                        <div class="col-md-12 rwPadLR0 rwPadTB15">
                            <div class="col-md-4">
                                <asp:Label runat="server" ID="lblSelectFirmware" Text="<%$Resources:PageGlobalResources,FirmwareSelectLabel %>" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList runat="server" ID="ddlFirmware" DataTextField="FirmwareDesc" DataValueField="FirmwareDesc" CssClass="form-control col-md-12">
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div class="col-md-12">
                            <asp:Button ID="btnUpdateFirmware" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,FirmwareUpdateButton %>" />
                        </div>
                        <%--   <div class="col-md-12">
                            <asp:Label ID="lblError" runat="server" Text="<%$Resources:PageGlobalResources,FirmwareUpdateError %>" Visible="false" ForeColor="red" />
                        </div>--%>
                    </div>
                    <div class="col-md-12 margin-top10 rwPadLR0">
                        <div id="divFirmwareUpdate" runat="server" class="col-md-10 margin-top15 rwPadLR0" visible="false">
                            <div class="col-md-12" style="padding-left: 35px;">
                                <div id="divFirmwareStatus" runat="server" class="col-md-12 margin-top5">
                                    <asp:Button ID="btnRetry" runat="server" Text="<%$Resources:RetryButton%>" Visible="false" CssClass="btn btn-default btnEdit btnW83" />
                                </div>
                                <div class="col-md-12 margin-top15">
                                    <div class="margin-top5">
                                        <asp:Label ID="lblCommandSent" runat="server" Text="<%$Resources:FirmwareCommandSent %>" Visible="false" />
                                        <asp:Label ID="lblCheckingforUpdates" Font-Bold="true" CssClass="colorGreen" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="margin-top5">
                                        <div id="divProgressBar" runat="server" class="progress" style="width: 800px; display: none; border: 1px solid black; min-height: 22px;">
                                            <div id="divProgressPercent" runat="server" class="progress-bar progress-bar-success active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%; color: black">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 rwMarTB15">
                            <asp:Label ID="lblHubFirmwareError" Font-Bold="true" CssClass="colorRed" runat="server" Visible="false"></asp:Label><br />
                        </div>
                    </div>
                </div>
                <%-- Account Section End --%>

                <asp:HiddenField runat="server" ID="hdnMAC" />
                <asp:HiddenField runat="server" ID="hdnFirmwareDesc" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tmrHubFirmware" EventName="Tick" />
            <asp:AsyncPostBackTrigger ControlID="btnRetry" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Timer ID="tmrHubFirmware" runat="server" Enabled="false" Interval="2000" />
</asp:Content>


