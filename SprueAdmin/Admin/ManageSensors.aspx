<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" CodeBehind="ManageSensors.aspx.vb" Inherits="SprueAdmin.ManageSensors" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <asp:UpdatePanel ID="updAccountandProperty" runat="server" UpdateMode="Conditional" class="container rwPadLR0 margin-top35">
        <ContentTemplate>
            <div class="rwMargT12">
                <div class="col-md-12 rwPT10B7 bgTheme margin-top15">
                    <div class="col-md-12 rwPadLR0 form-group">
                        <div class="col-md-3">
                            <span class="form-control inputhgt">
                                <asp:Literal ID="litAccountHolderText" runat="server" Text="<%$Resources:PageGlobalResources,AccountHolderLabel %>"></asp:Literal></span>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblAccounHolderValue" runat="server" CssClass="txtcolor lblfont13"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <span class="form-control inputhgt">
                                <asp:Literal ID="litMacAddressText" runat="server" Text="<%$Resources:PageGlobalResources,MACAddressLabel %>"></asp:Literal></span>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblMacAddressValue" runat="server" CssClass="txtcolor lblfont13"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-12 rwPadLR0">
                        <div class="col-md-3">
                            <span class="form-control inputhgt">
                                <asp:Literal ID="litAccountIDText" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>"></asp:Literal></span>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblAccountIDValue" runat="server" CssClass="txtcolor lblfont13"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <span class="form-control inputhgt">
                                <asp:Literal ID="litropertyAddressText" runat="server" Text="<%$Resources:PageGlobalResources,PropertyAddressLabel %>"></asp:Literal></span>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblPropertyAddressValue" runat="server" CssClass="txtcolor lblfont13"></asp:Label>
                        </div>
                    </div>

                    <div class="col-md-12 rwPadTB9">
                        <a class="pull-right btn btn-warning btnLogOff" data-toggle="modal" data-target="#ModalEditProperty">
                            <asp:Literal ID="litEdit" runat="server" Text="<%$Resources:PageGlobalResources,EditPropertyButton %>" />
                       </a>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnUpdateAddress" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>


    <%--Gateway section start --%>
    <div class="Gateway hide" id="divGateway">
        <div class="col-md-12 rwPadTB9 bgTheme txtcolor margin-top15">
            <div class="col-md-5">
                <asp:Label ID="lblGateway" Font-Bold="true" runat="server" Text="<%$Resources:GatewayHeader %>" />
            </div>
            <asp:Button ID="btnGatewayOpen" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnGatewayOpen" OnClientClick="return false;" Text="<%$Resources:Open %>" />
        </div>
    </div>
    <div class="col-md-12 rwPadLR0 GatewayGrid show" id="divGatewayGrid">
        <asp:UpdatePanel ID="updGateways" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="radGateSensors" runat="server" EnableEmbeddedSkins="false" AutoGenerateColumns="false" GridLines="Horizontal" ShowHeaderWhenEmpty="true" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>"
                    CssClass="rwMargT12">
                    <HeaderStyle CssClass="lblTableHeader bgTheme" />
                    <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:GatewayHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgGateway" runat="server" ImageUrl="~/common/img/gateway.png" Style="margin-left: -6px; height: 60px; width: 62px;" />
                                    <ajaxToolkit:HoverMenuExtender ID="HoverMenuGatewayDetails" runat="server" PopupControlID="PopupMenuGateway" TargetControlID="imgGateway" PopupPosition="Right"></ajaxToolkit:HoverMenuExtender>
                                    <asp:Panel ID="PopupMenuGateway" runat="server">
                                        <div class="panel panel-info col-sm-10" style="z-index: 99">
                                            <div class="panel-heading">
                                                <asp:Literal ID="litGatewayInfo" runat="server" Text="<%$Resources:GatewayInfo %>" />
                                            </div>
                                            <div class="panel-body">
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litInstalledOn" runat="server" Text="<%$Resources:InstalledOn %>" />
                                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litFirmwareVersion" runat="server" Text="<%$Resources:FirmwareVersion%>" />
                                                    <asp:Label ID="lblFirmwareVersion" runat="server" Text='<%# Eval("FirmwareDesc")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceType" runat="server" Text="<%$Resources:DeviceName%>" />
                                                    <asp:Label ID="lblDeviceTypeImageinfo" runat="server" Text="<%$Resources:Gateway %>" />
                                                </div>

                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceDetails" runat="server" Text="<%$Resources:DeviceDetails %>" />
                                                    <asp:Label ID="lblDeviceDetails" runat="server" Text="<%$Resources:GatewayDetails %>" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <%--<telerik:GridBoundColumn DataField="SensorType" HeaderText="<%$Resources:TypeHeader %>" />--%>

                            <telerik:GridTemplateColumn HeaderText="<%$Resources:TypeHeader %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblSensorType" runat="server" Text="<%$Resources:GatewayDeviceType %>" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <%--                            <telerik:GridTemplateColumn HeaderText="<%$Resources:Zone %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblZone" runat="server" Text='<%#Eval("AreaName")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>

                            <telerik:GridBoundColumn HeaderText="<%$Resources:LocationHeader %>" DataField="IntaRoomName" />


                            <telerik:GridTemplateColumn HeaderText="<%$Resources:GatewayStatus %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblGatewayStatus" runat="server" Text='<%#Eval("IsOnline")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="<%$Resources:FaultStatus%>">
                                <ItemTemplate>
                                    <asp:Button ID="btnGatewayState" runat="server" Text="<%$Resources:OK %>" Visible="false" CssClass="btn btnGreen" />
                                    <asp:Button ID="btnFaultStatus" runat="server" Text="<%$Resources:Fault %>" Visible="false" CssClass="btn btn-warning btnLogOff" CommandName="EventLog" CommandArgument='' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="<%$Resources:SupportTicketHeader %>">
                                <ItemTemplate>
                                    <asp:Button ID="btnRaiseTicket" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:RaiseTicketButton %>" CommandName="RaiseTicket" CommandArgument='<%# String.Format("{0},{1}", Eval("AreaID"), Eval("IntaDeviceID"))%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="<%$Resources:Edit %>">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:Edit %>" data-toggle="modal" data-devicelocation='<%#Eval("IntaRoomName")%>'
                                        data-devicezone='<%#Eval("AreaName")%>' data-devicename='<%#Eval("GatewayName")%>' data-devicetype='<%#Eval("SensorType")%>' data-devicearea='<%#Eval("AreaID")%>'
                                        data-deviceseq='<%#Eval("IntaDeviceSeq")%>' data-propzone='<%#Eval("IntaPropZone")%>' data-target="#ModalEditGateway" OnClientClick="return false;" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="timerRefresh" EventName="Tick" />
                <asp:AsyncPostBackTrigger ControlID="btnUpdateGateway" EventName="Click" />

            </Triggers>
        </asp:UpdatePanel>
        <div class="col-md-12 rwPadTB9  margin-top15">
            <asp:Button ID="btnGatewayClose" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnGatewayClose" OnClientClick="return false;" Text="<%$Resources:Close %>" />
        </div>
    </div>
    <%--Gateway section End--%>


    <%-- Register Sensor Section--%>
    <div class="RegisterSensor hide" id="divRegisterSensor">
        <div class="col-md-12 rwPadTB9 bgTheme txtcolor margin-top15">
            <div class="col-md-5">
                <asp:Label ID="lblRegSensor" Font-Bold="true" runat="server" Text="<%$Resources:RegisteredSensorHeader %>" />
            </div>
            <asp:Button ID="btnRegSensorOpen" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnRegSensorOpen" OnClientClick="return false;" Text="<%$Resources:Open %>" />
        </div>
    </div>
    <div class="col-md-12 rwPadLR0 RegisterSensorGrid show" id="divRegisterSensorGrid">
        <asp:UpdatePanel ID="updSensors" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="radGridSensors" runat="server" EnableEmbeddedSkins="false" AutoGenerateColumns="false" GridLines="Horizontal"  MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" ShowHeaderWhenEmpty="true" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>"
                    CssClass="rwMargT12 radGridSensors">
                    <HeaderStyle CssClass="lblTableHeader bgTheme" />
                    <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:RegisteredSensorHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgSensor" runat="server" Style="margin-left: -6px; height: 60px; width: 62px;" />
                                    <ajaxToolkit:HoverMenuExtender ID="HoverMenuDeviceDetails" runat="server" PopupControlID="PopupMenu" TargetControlID="imgSensor" PopupPosition="Right"></ajaxToolkit:HoverMenuExtender>
                                    <asp:Panel ID="PopupMenu" runat="server">
                                        <div class="panel panel-info col-sm-10" style="z-index: 99">
                                            <div class="panel-heading">
                                                <asp:Literal ID="litDeviceInfo" runat="server" Text="<%$Resources:DeviceInfo %>" />
                                            </div>
                                            <div class="panel-body">
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litInstalledOn" runat="server" Text="<%$Resources:InstalledOn %>" />
                                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceID" runat="server" Text="<%$Resources:DeviceID %>" />
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("IntaDeviceID")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceType" runat="server" Text="<%$Resources:DeviceName%>" />
                                                    <asp:Label ID="lblDeviceTypeImageinfo" runat="server" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceDetails" runat="server" Text="<%$Resources:DeviceDetails %>" />
                                                    <asp:Label ID="lblDeviceDetails" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:TypeHeader %>" HeaderStyle-Width="8em">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeviceType" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:Zones %>" />
                                    <telerik:RadComboBox ID="cboZoneSearch" RenderMode="Lightweight" runat="server" OnSelectedIndexChanged="cboZoneSearch_SelectedIndexChanged"
                                        EmptyMessage="<%$Resources:AllZones %>" AutoPostBack="true" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="8em" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZone" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="<%$Resources:LocationHeader %>" DataField="IntaRoomName" />
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:BatteryStatusHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgBatteryStatus" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ChamberSensorStatusHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgChamberSensorStatus" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:UnitonBaseHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgUnitOnBase" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn >
                                <HeaderTemplate>                                      
                                    <asp:Label ID="Label3" runat="server" Text="<%$Resources:Status %>" />
                                    <telerik:RadComboBox ID="cboFaultSearch" RenderMode="Lightweight" runat="server" OnSelectedIndexChanged="cboFaultSearch_SelectedIndexChanged"
                                        EmptyMessage="<%$Resources:AllFaults %>" AutoPostBack="true" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="8em" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btnFaultStatus" runat="server" CommandName="EventLog" CommandArgument='<%#Eval("IntaDeviceID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:SupportTicketHeader %>">
                                <ItemTemplate>
                                    <asp:Button ID="btnRaiseTicket" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:RaiseTicketButton %>" CommandName="RaiseTicket" CommandArgument='<%# String.Format("{0},{1}", Eval("AreaID"), Eval("IntaDeviceID"))%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:Edit %>">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" UseSubmitBehavior="false" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:Edit %>" data-toggle="modal" data-devicelocation='<%#Eval("IntaRoomName")%>'
                                        data-devicezone='<%#Eval("AreaName")%>' data-devicename='<%#Eval("IntaPropZoneDesc")%>' data-devicetype='<%#Eval("SensorType")%>' data-sensorstate='<%#Eval("SensorState")%>' data-mute='<%#Eval("Mute")%>'
                                        data-deviceseq='<%#Eval("IntaDeviceSeq")%>' data-propzone='<%#Eval("IntaPropZone")%>' data-devicearea='<%#Eval("AreaID")%>' data-target="#ModalEditSensor" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="timerRefresh" EventName="Tick" />
                <asp:AsyncPostBackTrigger ControlID="btnUpdateSensor" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="col-md-12 rwPadTB9  margin-top15">
            <asp:Button ID="btnRegSensorClose" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnRegSensorClose" OnClientClick="return false;" Text="<%$Resources:Close %>" />
        </div>
    </div>
    <%--Register Sensor section End--%>


    <%--Un Register Sensor section start --%>
    <div class="UnRegisterSensor" id="divUnRegisterSensor">
        <div class="col-md-12 rwPadTB9 bgTheme txtcolor margin-top15">
            <div class="col-md-5">
                <asp:Label ID="lblUnRegSensor" Font-Bold="true" runat="server" Text="<%$Resources:UnRegisteredSensorHeader %>" />
            </div>
            <asp:Button ID="btnUnRegSensorOpen" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnUnRegSensorOpen" OnClientClick="return false;" Text="<%$Resources:Open %>" />
        </div>
    </div>
    <div class="col-md-12 rwPadLR0 UnRegisterSensorGrid hide" id="divUnRegisterSensorGrid">
        <asp:UpdatePanel ID="updUnRegSensors" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="radGridUnregisteredSensors" runat="server" EnableEmbeddedSkins="false" AutoGenerateColumns="false" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" GridLines="Horizontal" ShowHeaderWhenEmpty="true" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>"
                    CssClass="rwMargT12">
                    <HeaderStyle CssClass="lblTableHeader bgTheme" />
                    <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:UnRegisteredSensorHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgSensor" runat="server" Style="height: 60px; width: 62px;" />
                                    <ajaxToolkit:HoverMenuExtender ID="HoverMenuDeviceDetails" runat="server" PopupControlID="PopupMenu" TargetControlID="imgSensor" PopupPosition="Right"></ajaxToolkit:HoverMenuExtender>
                                    <asp:Panel ID="PopupMenu" runat="server">
                                        <div class="panel panel-info col-sm-10">
                                            <div class="panel-heading">
                                                <asp:Literal ID="litDeviceInfo" runat="server" Text="<%$Resources:DeviceInfo %>" />
                                            </div>
                                            <div class="panel-body">
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litInstalledOn" runat="server" Text="<%$Resources:InstalledOn %>" />
                                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceID" runat="server" Text="<%$Resources:DeviceID %>" />
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("IntaDeviceID")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceType" runat="server" Text="<%$Resources:DeviceName%>" />
                                                    <asp:Label ID="lblDeviceTypeImageinfo" runat="server" />

                                                </div>

                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceDetails" runat="server" Text="<%$Resources:DeviceDetails %>" />
                                                    <asp:Label ID="lblDeviceDetails" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:TypeHeader %>" HeaderStyle-Width="10em">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeviceType" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:Zone %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblZone" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:LocationHeader %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocation" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:BatteryStatusHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgBatteryStatus" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ChamberSensorStatusHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgChamberSensorStatus" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:UnitonBaseHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgUnitOnBase" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                     <asp:Label ID="Label4" runat="server" Text="<%$Resources:Status %>" />
                                    <telerik:RadComboBox ID="cboFaultSearch" RenderMode="Lightweight" runat="server" OnSelectedIndexChanged="cboFaultSearch_SelectedIndexChanged"
                                        EmptyMessage="<%$Resources:AllFaults %>" AutoPostBack="true" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="8em" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btnFaultStatus" runat="server" CommandName="EventLog" CommandArgument='<%#Eval("IntaDeviceID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:SupportTicketHeader %>">
                                <ItemTemplate>
                                    <asp:Button ID="btnRaiseTicket" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:RaiseTicketButton %>" CommandName="RaiseTicket" CommandArgument='<%# String.Format("{0},{1}", Eval("AreaID"), Eval("IntaDeviceID"))%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:Edit %>">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-warning btnLogOff" Text="<%$Resources:Edit %>" data-toggle="modal" data-devicelocation='<%#Eval("IntaRoomName")%>'
                                        data-devicezone='<%#Eval("AreaName")%>' data-devicename='<%#Eval("IntaPropZoneDesc")%>' data-devicetype='<%#Eval("SensorType")%>'
                                        data-deviceseq='<%#Eval("IntaDeviceSeq")%>' data-propzone='<%#Eval("IntaPropZone")%>' data-target="#ModalEditSensor" data-devicearea='<%#Eval("AreaID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="timerRefresh" EventName="Tick" />
                <asp:AsyncPostBackTrigger ControlID="btnUpdateSensor" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="col-md-12 rwPadTB9  margin-top15">
            <asp:Button ID="btnUnRegSensorClose" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnUnRegSensorClose" OnClientClick="return false;" Text="<%$Resources:Close %>" />
        </div>
    </div>
    <%--Un Register Sensor section End--%>

    <%--Archived Sensor section start --%>
    <div class="ArchivedSensor" id="divArchivedSensor">
        <div class="col-md-12 rwPadTB9 bgTheme txtcolor margin-top15">
            <div class="col-md-5">
                <asp:Label ID="lblArchivedSensor" Font-Bold="true" runat="server" Text="<%$Resources:ArchivedSensorHeader %>" />
            </div>
            <asp:Button ID="btnArchivedSensorOpen" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnArchivedSensorOpen" OnClientClick="return false;" Text="<%$Resources:Open %>" />
        </div>
    </div>
    <div class="col-md-12 rwPadLR0 ArchivedSensorGrid hide" id="divArchivedSensorGrid">
        <asp:UpdatePanel ID="updArchivedSensors" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="radGridArchivedSensors" runat="server" EnableEmbeddedSkins="false" AutoGenerateColumns="false" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" GridLines="Horizontal" ShowHeaderWhenEmpty="true" EmptyDataText="<%$Resources:PageGlobalResources,SearchEmptyText %>"
                    CssClass="rwMargT12">
                    <HeaderStyle CssClass="lblTableHeader bgTheme" />
                    <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                    <MasterTableView>

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ArchivedSensorHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgSensor" runat="server" Style="height: 60px; width: 62px;" />
                                    <ajaxToolkit:HoverMenuExtender ID="HoverMenuDeviceDetails" runat="server" PopupControlID="PopupMenu" TargetControlID="imgSensor" PopupPosition="Right"></ajaxToolkit:HoverMenuExtender>
                                    <asp:Panel ID="PopupMenu" runat="server">
                                        <div class="panel panel-info col-sm-10">
                                            <div class="panel-heading">
                                                <asp:Literal ID="litDeviceInfo" runat="server" Text="<%$Resources:DeviceInfo %>" />
                                            </div>
                                            <div class="panel-body">
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litInstalledOn" runat="server" Text="<%$Resources:InstalledOn %>" />
                                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceID" runat="server" Text="<%$Resources:DeviceID %>" />
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("IntaDeviceID")%>' />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceType" runat="server" Text="<%$Resources:DeviceName%>" />
                                                    <asp:Label ID="lblDeviceTypeImageinfo" runat="server" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:Literal ID="litDeviceDetails" runat="server" Text="<%$Resources:DeviceDetails %>" />
                                                    <asp:Label ID="lblDeviceDetails" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:TypeHeader %>" HeaderStyle-Width="10em">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeviceType" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="Label5" runat="server" Text="<%$Resources:Zones %>" />
                                    <telerik:RadComboBox ID="cboZoneSearch" RenderMode="Lightweight" runat="server" OnSelectedIndexChanged="cboZoneSearch_SelectedIndexChanged"
                                        EmptyMessage="<%$Resources:AllZones %>" AutoPostBack="true" MarkFirstMatch="true" Filter="StartsWith" AllowCustomText="true" Width="10em" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZone" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:LocationHeader %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocation" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:BatteryStatusHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgBatteryStatus" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ChamberSensorStatusHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgChamberSensorStatus" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:UnitonBaseHeader %>">
                                <ItemTemplate>
                                    <asp:Image ID="imgUnitOnBase" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="<%$Resources:FaultAlertStatus%>">
                                <ItemTemplate>
                                    <asp:Button ID="btnFaultStatus" runat="server" Text="Fault Status" CssClass="FaultStatus" CommandName="EventLog" CommandArgument='<%#Eval("IntaDeviceID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="timerRefresh" EventName="Tick" />
            </Triggers>

        </asp:UpdatePanel>
        <div class="col-md-12 rwPadTB9  margin-top15">
            <asp:Button ID="btnArchivedSensorClose" runat="server" CssClass="btn btn-default btnEdit btnTB3 pull-right btnArchivedSensorClose" OnClientClick="return false;" Text="<%$Resources:Close %>" />
        </div>
    </div>
    <%--Archived Sensor section End--%>

    <asp:Timer ID="timerRefresh" runat="server" Enabled="true" Interval="10000" />

    <div class="modal fade" id="ModalEditProperty" role="dialog" style="display: none;">
        <div class="modal-dialog ">
            <!-- Modal content-->
            <div class="modal-content modalW">
                <div class="modal-body rwPT60 rwPB90">
                    <div class="row">

                        <div class="col-md-10 col-md-offset-1 text-center">
                            <div class="form-group">
                                <label class="lblfont16">
                                    <asp:Literal ID="litEditPropertyAddress" runat="server" Text="<%$Resources:EditPropertyAddress %>" /></label>
                            </div>
                        </div>

                        <div id="divPropertyAddress">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditPropertyAddress1" runat="server" CssClass="form-control inputhgt" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditPropertyAddress2" runat="server" CssClass="form-control inputhgt" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditPropertyAddress3" runat="server" CssClass="form-control inputhgt" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditPropertyAddress4" runat="server" CssClass="form-control inputhgt" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditPropertyPostCode" runat="server" CssClass="form-control inputhgt" />
                                </div>
                            </div>
                        </div>
                        <div id="divAccountAddress" class="hide">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditAccountAddress1" runat="server" CssClass="form-control inputhgt" ReadOnly="true" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditAccountAddress2" runat="server" CssClass="form-control inputhgt" ReadOnly="true" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditAccountAddress3" runat="server" CssClass="form-control inputhgt" ReadOnly="true" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditAccountAddress4" runat="server" CssClass="form-control inputhgt" ReadOnly="true" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="form-group">
                                    <asp:TextBox ID="txtEditAccountPostCode" runat="server" CssClass="form-control inputhgt" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-10  col-md-offset-1  text-center">
                            <div class="form-group">
                                <asp:CheckBox ID="chkUseAccountAddress" runat="server" CssClass="okcheckbox use-account-address" Text=' <%$Resources:UseAccountAddress%>' />
                            </div>
                        </div>

                        <div class="col-md-10 col-md-offset-1 text-center">
                            <div class="form-group">
                                <telerik:RadComboBox runat="server" ID="cboEditPropertyRiskLevel" RenderMode="Lightweight" AllowCustomText="false" EmptyMessage="<%$Resources:RiskLevelLabel %>" Width="100%" CssClass="input-md" DataValueField="Id" DataTextField="Text">
                                </telerik:RadComboBox>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                               <asp:Button ID="Button3" runat="server"  CssClass="btn btn-warning btnLogOff pull-left" Text="<%$Resources:Cancel%>" data-dismiss="modal" />
                               <asp:Button ID="btnUpdateAddress" runat="server" OnClientClick="CloseModal('ModalEditProperty');" CssClass="btn btn-warning btnLogOff pull-right" Text='<%$Resources:Save%>' />
                               </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- Gateway edit update --%>

    <div class="modal fade" id="ModalEditGateway" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <asp:HiddenField ID="hdnDeviceSeq1" runat="server" />
                        <asp:HiddenField ID="hdnPropZone1" runat="server" />

                        <div class="row">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <label class="lblfont16">
                                        <asp:Literal ID="litEditDevice1" runat="server" Text="<%$Resources:EditDevice%>" />
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-10 col-md-offset-1">
                                <div class="col-md-4 text-center">
                                    <asp:Literal ID="litDeviceTypeLabel1" runat="server" Text="<%$Resources:DeviceType%>" />
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:Label ID="lblDeviceType1" runat="server" />
                                </div>
                            </div>

                            <div class="col-md-10 col-md-offset-1 form-group">
                                <div class="col-md-4  text-center">
                                    <asp:Literal ID="litDeviceName1" runat="server" Text="<%$Resources:DeviceName%>" />
                                </div>
                                <div class="form-group col-md-8" style="margin-bottom: 0px;">
                                    <asp:Label ID="lblGatewayDeviceName" runat="server" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center" runat="server" visible="false">
                                <div class="col-md-4">

                                    <asp:Literal ID="Literal21" runat="server" Text="<%$Resources:DeviceZone%>" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="col-md-4">
                                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:DeviceLocation%>" />
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:TextBox ID="txtDeviceLocation1" runat="server" CssClass="form-control inputhgt" />
                                </div>
                            </div>
                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="col-md-4">

                                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:DeviceZone%>" />
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:UpdatePanel runat="server" ID="updEditGateway" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadComboBox ID="cboGatewayZone" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="findFirstComboTextMatch" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Button ID="Button2" runat="server" CssClass="btn btn-warning btnLogOff pull-left" Text="<%$Resources:Cancel%>" data-dismiss="modal" />
                                    <asp:Button ID="btnUpdateGateway" runat="server" ValidationGroup="Gateway" OnClientClick="CloseModal('ModalEditGateway');" CausesValidation="true" CssClass="btn btn-warning btnLogOff pull-right" Text='<%$Resources:Save%>' />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <%-- Gateway Delete Modal --%>

    <div class="modal fade" id="ModalDeleteGateway" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <span style="font-size: 16px; font-weight: bold">
                            <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Gateway%>" />
                        </span>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Deleteinfo%>" />
                                </div>
                            </div>

                            <div class="col-md-12 text-center">
                                <div class="form-group">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-warning btnLogOff pull-left" Text="<%$Resources:Cancel%>" data-dismiss="modal" />
                                    <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text="<%$Resources:Yes%>" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- Sensors edit update --%>
    <div class="modal fade" id="ModalEditSensor" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <asp:HiddenField ID="hdnDeviceSeq" runat="server" />
                        <asp:HiddenField ID="hdnPropZone" runat="server" />

                        <div class="row">
                            <div class="col-md-10 col-md-offset-1 text-center">
                                <div class="form-group">
                                    <label class="lblfont16">
                                        <asp:Literal ID="litEditDevice" runat="server" Text="<%$Resources:EditDevice%>" />
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-10 col-md-offset-1">
                                <div class="col-md-4 text-center">
                                    <asp:Literal ID="litDeviceTypeLabel" runat="server" Text="<%$Resources:DeviceType%>" />
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:Label ID="lblDeviceType" CssClass="DeviceTypeClass" runat="server" />
                                </div>
                            </div>

                            <div class="col-md-10 col-md-offset-1">
                                <div class="col-md-4 text-center">
                                    <asp:Literal ID="litDeviceName" runat="server" Text="<%$Resources:DeviceName%>" />
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:Label ID="lblSensorDeviceName" CssClass="DeviceNameClass" runat="server" />
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="col-md-4">
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:DeviceLocation%>" />
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:TextBox ID="txtDeviceLocation" ValidationGroup="Sensor" runat="server" CssClass="form-control inputhgt" />
                                    <%--<asp:RequiredFieldValidator ID="rfvDeviceLocation" ValidationGroup="Sensor" ControlToValidate="txtDeviceLocation" CssClass="text-danger" runat="server" ErrorMessage="<%$Resources:DeviceLocationMandatory%>" />--%>
                                </div>
                            </div>

                            <div class="col-md-10  col-md-offset-1  text-center">
                                <div class="col-md-4">

                                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:DeviceZone%>" />
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:UpdatePanel runat="server" ID="updEditSensor" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadComboBox ID="cboDeviceZone" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="findFirstComboTextMatch" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div id="divColdAlarm" runat="server" class="colPT30 divColdAlarm col-md-10 col-md-offset-1 hide">
                                <div class="col-md-4">
                                    <div id="divSounder" class="rwPT7">
                                        <asp:CheckBox ID="chkSounder" CssClass="form-control-chk register-popup-device-sounder" Text="<%$Resources:SounderLabel %>" TextAlign="Left" runat="server" Checked="true" />
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="drpRadTemperature" runat="server" CssClass="form-control inputddl" AutoPostBack="false">
                                        <asp:ListItem Text="<%$Resources:DeviceTemperatureSelectHeader %>" Value="0" />
                                        <asp:ListItem Text="12-14" Value="12-14" />
                                        <asp:ListItem Text="15-17" Value="15-17" />
                                        <asp:ListItem Text="18-20" Value="18-20" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-12 margin-top15">
                                <div class="form-group">
                                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-warning btnLogOff pull-left" Text="<%$Resources:Cancel%>" data-dismiss="modal" />
                                    <asp:Button ID="btnUpdateSensor" runat="server" ValidationGroup="Sensor" CausesValidation="true" CssClass="btn btn-warning btnLogOff pull-right" Text='<%$Resources:Save%>' />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function addOpenCloseHandlers(args) {
            $(args.closeButton).onIfNotPresent("click", "openClose", function () {
                $(args.grid).removeClass("show").addClass("hide");
                $(args.openContainer).removeClass("hide").addClass("show");
            });
            $(args.openButton).onIfNotPresent("click", "openClose", function () {
                $(args.grid).removeClass("hide").addClass("show");
                $(args.openContainer).removeClass("show").addClass("hide");
            });
        }

        Sys.Application.add_load(function () {

            function SetComboItem(comboID, itemValue) {
                var combo = $find(comboID);

                if (combo && typeof combo.findItemByValue == 'function') {
                    var item = combo.findItemByValue(itemValue);

                    if (item) {
                        item.set_selected(true);
                        combo.set_selectedIndex(item.get_index());
                        combo.set_text(item.get_text());
                    }
                }
            }

            addOpenCloseHandlers({ openButton: ".btnRegSensorOpen", closeButton: ".btnRegSensorClose", grid:".RegisterSensorGrid", openContainer: ".RegisterSensor"});
            addOpenCloseHandlers({ openButton: ".btnUnRegSensorOpen", closeButton: ".btnUnRegSensorClose", grid:".UnRegisterSensorGrid", openContainer: ".UnRegisterSensor"});
            addOpenCloseHandlers({ openButton: ".btnArchivedSensorOpen", closeButton: ".btnArchivedSensorClose", grid: ".ArchivedSensorGrid", openContainer: ".ArchivedSensor" });
            addOpenCloseHandlers({ openButton: ".btnGatewayOpen", closeButton: ".btnGatewayClose", grid: ".GatewayGrid", openContainer: ".Gateway" });

            $('.use-account-address input').onIfNotPresent("click", "toggleVis", function () {
                if ($(this).is(':checked')) {
                    $("#divPropertyAddress").addClass("hide");
                    $("#divAccountAddress").removeClass("hide");
                }
                else {
                    $("#divAccountAddress").addClass("hide");
                    $("#divPropertyAddress").removeClass("hide");

                }
            });

            $('#ModalEditSensor').onIfNotPresent('show.bs.modal', "show", function (event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var devicelocation = button.data('devicelocation'); // Extract info from data-* attributes
                var devicezone = button.data('devicezone'); // Extract info from data-* attributes
                var devicearea = button.data('devicearea'); // Extract info from data-* attributes
                var devicename = button.data('devicename'); // Extract info from data-* attributes
                var devicetype = button.data('devicetype'); // Extract info from data-* attributes
                var deviceseq = button.data('deviceseq'); // Extract info from data-* attributes
                var propzone = button.data('propzone'); // Extract info from data-* attributes
                var sensorstate = button.data('sensorstate'); // Extract info from data-* attributes
                var mute = button.data('mute'); // Extract info from data-* attributes

                $(this).find("#<%=txtDeviceLocation.ClientID%>").val(devicelocation);
                $(this).find("#<%=hdnDeviceSeq.ClientID%>").val(deviceseq);
                $(this).find("#<%=hdnPropZone.ClientID%>").val(propzone);
                $(this).find("#<%=drpRadTemperature.ClientID%>").val(sensorstate);
                SetComboItem($(this).find("[id$='cboDeviceZone']").prop("id"), devicearea);

                //divColdAlarm show when modal box open for only ColdAlarm sensor type
                if (devicetype == "ColdAlarm") {
                    $(".divColdAlarm").removeClass("hide").addClass("show");
                    if (mute == "False") {
                        $(this).find("#<%=chkSounder.ClientID%>").prop("checked", "checked");
                    } else {
                        $(this).find("#<%=chkSounder.ClientID%>").prop("checked", "");
                    }
                }

                //Set the device type and device name from the resources based on sensortype
                SetDeviceType_DeviceName(devicetype);

            });

            //Remove the hide class for divColdAlarm when modal box close
            $('#ModalEditSensor').onIfNotPresent('hide.bs.modal', "hide", function () {
                $(".divColdAlarm").addClass("hide").removeClass("show");
            });

            $('#ModalEditGateway').onIfNotPresent('show.bs.modal', "show", function (event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var devicelocation = button.data('devicelocation'); // Extract info from data-* attributes
                var devicezone = button.data('devicezone'); // Extract info from data-* attributes
                var devicearea = button.data('devicearea'); // Extract info from data-* attributes
                var devicename = button.data('devicename'); // Extract info from data-* attributes
                var devicetype = button.data('devicetype'); // Extract info from data-* attributes
                var deviceseq = button.data('deviceseq'); // Extract info from data-* attributes
                var propzone = button.data('propzone'); // Extract info from data-* attributes

                $(this).find("#<%=txtDeviceLocation1.ClientID%>").val(devicelocation);

                $(this).find("#<%=lblGatewayDeviceName.ClientID%>").text(devicename);
                $(this).find("#<%=lblDeviceType1.ClientID%>").text(devicetype);
                $(this).find("#<%=hdnDeviceSeq1.ClientID%>").val(deviceseq);
                $(this).find("#<%=hdnPropZone1.ClientID%>").val(propzone);
                SetComboItem("<%=cboGatewayZone.ClientID%>", devicearea);
            });
         

        });
        $('#ModalEditProperty').on("hidden.bs.modal", function () {

            location.reload(true);
           
        });
       
        function CloseModal(ModalName) {
            $('#' + ModalName + '').modal('hide');
        }

        //Set the device type and device name from the resources based on sensortype
        function SetDeviceType_DeviceName(strSensorType) {
            var Devicetype = "";
            var DeviceName = "";
            switch (strSensorType) {
            case "ColdAlarm":
                //Extreme Temperature Alarm   WETA-10X
                Devicetype = '<%= GetLocalResourceObject("ColdAlarmType")%>';
                DeviceName = '<%= GetLocalResourceObject("ColdAlarm")%>';
                break;
            case "SmokeAlarm":
                //Smoke Alarm   WST-630
                Devicetype = '<%= GetLocalResourceObject("SmokeAlarmType")%>';
                DeviceName = '<%= GetLocalResourceObject("SmokeAlarm")%>';
                break;
            case "HeatAlarm":
                //Heat Alarm   WHT-630
                Devicetype = '<%= GetLocalResourceObject("HeatAlarmType")%>';
                DeviceName = '<%= GetLocalResourceObject("HeatAlarm")%>';
                break;
            case "COAlarm":
                //Carbon Monoxide Alarm  W2-CO-10X 
                Devicetype = '<%= GetLocalResourceObject("COAlarmType")%>';
                DeviceName = '<%= GetLocalResourceObject("COAlarm")%>';
                break;
            case "ACSmokeAlarm":
                //230V - Smoke Alarm WSM-F-1EU 
                Devicetype = '<%= GetLocalResourceObject("ACSmokeAlarmType")%>';
                DeviceName = '<%= GetLocalResourceObject("ACSmokeAlarm")%>';
                break;
            case "ACHeatAlarm":
                //230V - Heat Alarm  WHM-F-1EU 
                Devicetype = '<%= GetLocalResourceObject("ACHeatAlarmType")%>';
                DeviceName = '<%= GetLocalResourceObject("ACHeatAlarm")%>';
                break;
            case "PadStrobe":
                //Strobe & Vibrating Pad  W2-SVP-630
                Devicetype = '<%= GetLocalResourceObject("PadStrobeType")%>';
                DeviceName = '<%= GetLocalResourceObject("PadStrobe")%>';
                break;
            case "LowFreqSounder":
                // Low Frequency Sounder W2-LFS-630
                Devicetype = '<%= GetLocalResourceObject("LowFreqSounderType")%>';
                DeviceName = '<%= GetLocalResourceObject("LowFreqSounder")%>';
                break;
            case "AlarmControlUnit":
                //Alarm Control Unit  WTSL-F-1EU 
                Devicetype = '<%= GetLocalResourceObject("AlarmControlUnitType")%>';
                DeviceName = '<%= GetLocalResourceObject("AlarmControlUnit")%>';
                break;
            case "InterfaceGateway":
                //Interface Gateway IFG100
                Devicetype = '<%= GetLocalResourceObject("InterfaceGatewayType")%>';
                DeviceName = '<%= GetLocalResourceObject("InterfaceGateway")%>';
                break;
            case "InterfaceGateway200":
                //Interface Gateway IFG200
                Devicetype = '<%= GetLocalResourceObject("InterfaceGateway200Type")%>';
                DeviceName = '<%= GetLocalResourceObject("InterfaceGateway")%>';
                break;
            default:
                Devicetype = "";
                DeviceName = "";
            }

            $(".DeviceTypeClass").text(Devicetype);
            $(".DeviceNameClass").text(DeviceName);

        }


    </script>

</asp:Content>
