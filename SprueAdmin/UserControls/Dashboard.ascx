<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Dashboard.ascx.vb" Inherits="SprueAdmin.Dashboard" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadXmlHttpPanel runat="server" ID="dashPanel" OnClientResponseEnded="dashPanelResponseEnded"> 
    <div class="row rwpadding bgwhite">
        <div class="col-md-9">
            <div class="row">
                <div class="col-md-3 form-group">
                    <div class="row">
                        <asp:LinkButton runat="server" ID="lnkTotalDevs" CommandName="TotalDevices" CausesValidation="false" CssClass="tileLink">
                            <div class="col-md-12">
                                <div id="divDevicesALL" class="row form-control txtcolnumber text-center tileContainer" onclick="SelectedOption($(this).attr('id'))">
                                    <div class="col-md-12">
                                        <img src="/common/img/AccountEnquiry/devnumber.png" />
                                    </div>
                                    <div class="col-md-12">
                                        <span class="labelInner lblPT12">
                                            <asp:Literal runat="server" ID="litAllTileLegend" /></span>
                                    </div>
                                    <div class="col-md-12">
                                        <span class="labelInner txtinner">
                                            <asp:Literal runat="server" ID="litTotalDevicesCount" /></span>
                                    </div>
                                    <div class="col-md-12">
                                        <span class="labelInner">
                                            <asp:Literal runat="server" ID="Literal3" Text="<%$Resources:PageGlobalResources,Devices %>" /></span>
                                    </div>
                                </div>
                            </div>
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="col-md-3 form-group">
                    <div class="row">
                        <asp:LinkButton runat="server" ID="lnkNoFault" CommandName="NoFaults" CausesValidation="false" CssClass="tileLink">
                            <div class="col-md-12 text-center">
                                <div id="divAlertsFaults" class="row form-control txtcolalert  text-center tileContainer" onclick="SelectedOption($(this).attr('id'))">
                                    <div class="col-md-12">
                                        <img src="/common/img/AccountEnquiry/devalert.png" />
                                    </div>
                                    <div class="col-md-12 rwPadLR0">
                                        <span class="lblinnerlabel labelInner txtcolor">
                                            <asp:Literal runat="server" Text="<%$Resources:PageGlobalResources,DSH_NoAlertsLegend %>" /></span>
                                    </div>
                                    <div class="col-md-12 ">
                                        <span class="txtinner labelInner txtcolor">
                                            <asp:Literal runat="server" ID="litNoAlertsCount" /></span>
                                    </div>
                                    <div class="col-md-12">
                                        <span class="labelInner colorwhite">
                                            <asp:Literal runat="server" ID="Literal4" Text="<%$Resources:PageGlobalResources,Devices %>" /></span>
                                    </div>
                                </div>
                            </div>
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="col-md-3 form-group">
                    <div class="row">
                        <asp:LinkButton runat="server" ID="lnkFaults" CommandName="ActiveFaults" CausesValidation="false" CssClass="tileLink">
                            <div class="col-md-12">
                                <div id="divActiveFaults" class="row form-control txtcolfault  text-center tileContainer" onclick="SelectedOption($(this).attr('id'))">
                                    <div class="col-md-12">
                                        <img src="/common/img/AccountEnquiry/devfault.png" />
                                    </div>
                                    <div class="col-md-12 rwPadLR0">
                                        <span class="lblinnerlabel labelInner txtcolor">
                                            <asp:Literal runat="server" Text="<%$Resources:PageGlobalResources,DSH_FaultsLegend %>" /></span>
                                    </div>
                                    <div class="col-md-12">
                                        <span class="txtinner labelInner txtcolor">
                                            <asp:Literal runat="server" ID="litFaultsCount" /></span>
                                    </div>
                                    <div class="col-md-12">
                                        <span class="labelInner txtcolor">
                                            <asp:Literal runat="server" ID="Literal5" Text="<%$Resources:PageGlobalResources,Devices %>" /></span>
                                    </div>
                                </div>
                            </div>
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="col-md-3 form-group">
                    <div class="row">
                        <asp:LinkButton runat="server" ID="lnkAlerts" CommandName="ActiveAlerts" CausesValidation="false" CssClass="tileLink">
                            <div class="col-md-12">
                                <div id="divActiveAlerts" class="row form-control txtcolactive  text-center tileContainer" onclick="SelectedOption($(this).attr('id'))">
                                    <div class="col-md-12">
                                        <img src="/common/img/AccountEnquiry/devactivealert.png" />
                                    </div>
                                    <div class="col-md-12 rwPadLR0">
                                        <span class="lblinnerlabel labelInner txtcolor">
                                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:PageGlobalResources,DSH_AlertsLegend %>" /></span>
                                    </div>
                                    <div class="col-md-12">
                                        <span class="txtinner labelInner txtcolor">
                                            <asp:Literal runat="server" ID="litAlertsCount" /></span>
                                    </div>
                                    <div class="col-md-12">
                                        <span class="labelInner txtcolor">
                                            <asp:Literal runat="server" ID="Literal6" Text="<%$Resources:PageGlobalResources,Devices %>" /></span>
                                    </div>
                                </div>
                            </div>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="row">
                <div class="col-md-12 colstyle">
                    <div class="row">
                        <asp:LinkButton runat="server" ID="lnkUntested" CommandName="NotTested" CausesValidation="false" CssClass="tileLink">
                            <div id="divTested" class="col-md-12 tileContainer" onclick="SelectedOption($(this).attr('id'))">
                                <div class="col-md-3 col-xs-3 imgpadding">
                                    <img src="/common/img/AccountEnquiry/devtest.png" />
                                </div>
                                <div class="col-md-9 col-xs-9">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <span class="labelInner txtcolor">
                                                <asp:Literal runat="server" Text="<%$Resources:PageGlobalResources,DSH_UntestedLegend %>" /></span>
                                        </div>
                                        <div class="col-md-12">
                                            <span class=" txtcolor labelInner fontlabel28 col-md-3">
                                                <asp:Literal runat="server" ID="litUntestedCount" /></span>
                                     <%--       <div class="col-md-4 rwMargT10">
                                                <asp:Label runat="server" Font-Bold="true" ID="Label2" Text="<%$Resources:PageGlobalResources,Devices %>" />
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="col-md-12 colstyle" style="margin-top: 4px;">
                    <div class="row">
                        <asp:LinkButton runat="server" ID="lnkSupport" CommandName="SupportTickets" CausesValidation="false" CssClass="tileLink">
                            <div id="divSupportTickets" class="col-md-12 tileContainer" onclick="SelectedOption($(this).attr('id'))">
                                <div class="col-md-3 col-xs-3 imgpadding">
                                    <img src="/common/img/AccountEnquiry/devsupport.png" />
                                </div>
                                <div class="col-md-9 col-xs-9">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <span class="labelInner txtcolor">
                                                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:PageGlobalResources,DSH_TicketsLegend %>" /></span>
                                        </div>
                                        <div class="col-md-12">
                                            <label class=" txtcolor labelInner fontlabel28 col-md-3">
                                                <asp:Literal runat="server" ID="litTicketsCount" /></label>
                              <%--              <div class="col-md-4 rwMargT10">
                                                <asp:Label runat="server" Font-Bold="true" ID="Label1" Text="<%$Resources:PageGlobalResources,Devices %>" />
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</telerik:RadXmlHttpPanel>
        <script type="text/javascript">
            var dashPanel;
            var dashPanelInterval = 0;
            var selectedTileID;

            Sys.Application.add_load(PanelSetup)

            function PanelSetup() {
                dashPanel = $find("<%= dashPanel.ClientID%>");
            };

            function dashPanelResponseEnded(sender, args) {
                setDashCallBack(dashPanelInterval);
                if (selectedTileID) {
                    SelectedOption(selectedTileID);
                }
            }

            function setDashCallBack(interval) {
                dashPanelInterval = interval;

                if (interval > 0) {
                    setTimeout(function () { SetDashPanelValue('RefreshTimer'); }, interval);
                }
            }

            function SetDashPanelValue(value) {
                if (dashPanel) {
                    dashPanel.set_value(value);
                }
            }

            function setTileClasses(lnkID) {
                var div = $('#' + lnkID + ' .tileContainer');
                SelectedOption(div.prop('id'));
            }

            function SelectedOption(ID) {
                $(".tileContainer").removeClass('tileSelected');
                $("#" + ID).addClass("tileSelected");
                selectedTileID = ID
            }


        </script>
    <%--<asp:Timer runat="server" ID="tmrRefresh" />--%>
