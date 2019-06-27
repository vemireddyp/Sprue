<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" CodeBehind="ManageRiskLevel.aspx.vb" Inherits="SprueAdmin.ManageRiskLevel" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">

    <div class="container">
        <div id="divManageRiskLevels" class="row margin-top70">
            <asp:UpdatePanel ID="updRiskLevels" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="col-md-3 fontstyleTelerik">
                        <telerik:RadComboBox ID="cboDistributors" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" AutoPostBack="true" Filter="StartsWith"
                            EmptyMessage="<%$Resources:PageGlobalResources,AllDistributorsText %>" MarkFirstMatch="true" AllowCustomText="true" DataTextField="Name" DataValueField="MasterCoID"
                            OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                    </div>
                    <div class="col-md-12 margin-top15 rwPadLR0">

                        <%-- 1st Alarm Condition starts here --%>
                        <div id="div1stAlarm" class="panel panelStyle">
                            <%--1st Alarm Condition panel header --%>
                            <div class="panel-heading PanelHeadingStyle">
                                <div class="col-md-1">
                                    <span class="sprBadge badge-default blackColor">
                                        <asp:Label Text="<%$Resources:One%>" runat="server" />
                                    </span>
                                </div>
                                <div class="col-md-3 padL0">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text="<%$Resources:1stAlarmHeader%>" ReadOnly="true" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Image ID="imgInfo" runat="server" ImageUrl="~/common/img/excl.png" onmouseover="show();"  onmouseleave="show();"/>
                                </div>
                                <div class="col-md-1 pull-right">
                                    <a id="btnCloseOpen1stAlarm" data-toggle="collapse" href="#<%=collapse1stAlarm.ClientID%>">
                                        <asp:Button ID="btn1stAlarmOpenClose" runat="server" CssClass="btn btn-default btnPTB3 btnEdit collapse-target" Text="<%$Resources:Open%>" /></a>
                                </div>
                            </div>
                            <%--1st Alarm Condition panel body --%>
                            <div id='collapse1stAlarm' class="panel-collapse collapse bgwhite" runat="server">
                                <div class="row">
                                    <div class="text-center col-md-12 rwPadLR0 rwMarTB15">
                                        <div class="col-md-12 margin-top15">
                                            <div class="col-md-4 col-md-offset-4">
                                                <div class="col-md-12" style="padding-left: 8px; padding-right: 8px;">
                                                    <div class="commonStyle bg1stAlarmcolor1">
                                                        <asp:Label ID="Label74" Text="<%$Resources:collapse1stAlarmHeader%>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 rwPadLR0 text-center">
                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                        </div>
                                        <div class="col-md-8 col-md-offset-2" style="border-bottom: 1px solid black;">
                                        </div>
                                        <%--1st Alarm Condition High Risk--%>
                                        <div class="col-md-4" style="top: -3px;">
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="commonStyle bg1stAlarmcolor1">
                                                        <asp:Label ID="Label73" Text="<%$Resources:HighRisk%>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink1" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 col-sm-6 fontstyleTelerik rwPadLR0">
                                                            <telerik:RadComboBox ID="cboHighContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink25" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label50" Text="<%$Resources:AlarmConditionLbl %>" runat="server" />
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <div class="col-md-6 fontstyleTelerik col-md-offset-3">
                                                                <telerik:RadComboBox ID="cbo1stAlarmHighSilencedValue" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <asp:Label ID="Label106" Text="<%$Resources:ActivationMinuteLbl %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgYes">
                                                            <asp:Label ID="Label51" Text="<%$Resources:Yes %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgNo">
                                                            <asp:Label ID="Label52" Text="<%$Resources:No %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink54" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo1stAlarmHighSilencedContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                            OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink53" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo1stAlarmHighSilencedContactList" ValidationGroup="1stAlarm" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink2" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo1stAlarmHighNotSilencedContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                            OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink26" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo1stAlarmHighNotSilencedContactList" ValidationGroup="1stAlarm" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--1st Alarm Condition Medium Risk--%>
                                        <div class="col-md-4" style="top: -3px;">
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="commonStyle bg1stAlarmcolor1">
                                                        <asp:Label ID="Label75" Text="<%$Resources:MediumRisk%>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink3" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 fontstyleTelerik rwPadLR0">
                                                            <div class="popup" >
                                                             <span class="popuptext" id="helpText">   </span>                                  
                                                                
                                                             </div>
                                                            <telerik:RadComboBox ID="cboMediumContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                       
                                                        </div>
                                                         
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink27" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label6" Text="<%$Resources:AlarmConditionLbl %>" runat="server" />
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                               <div class="popup1" >
                                                             <span class="popuptext" id="helpText1"></span>
                                                             </div>
                                                            <div class="col-md-6 fontstyleTelerik col-md-offset-3">
                                                               
                                                                <telerik:RadComboBox ID="cbo1stAlarmMediumSilencedValue" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                               
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <asp:Label ID="Label7" Text="<%$Resources:ActivationMinuteLbl %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgYes">
                                                            <asp:Label ID="Label59" Text="<%$Resources:Yes %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgNo">
                                                            <asp:Label ID="Label60" Text="<%$Resources:No %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink51" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo1stAlarmMediumSilencedContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                            OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink52" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="popup3" >
                                                             <span class="popuptext" id="helpText3"> 
                                                             </span>
                                                             </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo1stAlarmMediumSilencedContactList" ValidationGroup="1stAlarm" runat="server" />
                                                    </div>
                                                </div>
                                                
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink5" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo1stAlarmMediumNotSilencedContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                            OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink28" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="popup2" >
                                                             <span class="popuptext" id="helpText2"> 
                                                             </span>
                                                             </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo1stAlarmMediumNotSilencedContactList" ValidationGroup="1stAlarm" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--1st Alarm Condition Low Risk--%>
                                        <div class="col-md-4" style="top: -3px;">
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="commonStyle bg1stAlarmcolor1">
                                                        <asp:Label ID="Label76" Text="<%$Resources:LowRisk%>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink4" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 fontstyleTelerik rwPadLR0">
                                                            <telerik:RadComboBox ID="cboLowContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink29" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12 rwPadLR0 text-center">
                                                    <span class="glyphicon glyphicon-arrow-down"></span>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label9" Text="<%$Resources:AlarmConditionLbl %>" runat="server" />
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <div class="col-md-6 fontstyleTelerik col-md-offset-3">
                                                                <telerik:RadComboBox ID="cbo1stAlarmLowSilencedValue" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <asp:Label ID="Label8" Text="<%$Resources:ActivationMinuteLbl %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgYes">
                                                            <asp:Label ID="Label67" Text="<%$Resources:Yes %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgNo">
                                                            <asp:Label ID="Label68" Text="<%$Resources:No %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink49" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo1stAlarmLowSilencedContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                            OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink50" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo1stAlarmLowSilencedContactList" ValidationGroup="1stAlarm" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink6" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo1stAlarmLowNotSilencedContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink30" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo1stAlarmLowNotSilencedContactList" ValidationGroup="1stAlarm" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 margin-top15">
                                                <div class="pull-right">
                                                    <asp:Button ID="btnSave1stAlarmCondition" CssClass="btn btn-warning btnLogOff" ValidationGroup="1stAlarm" runat="server" Text="<%$Resources:Save%>" />
                                                </div>
                                            </div>
                                            <div class="col-md-12 margin-top5">
                                                <div class="pull-right">
                                                    <asp:Label ID="lbl1stAlarmUpdateStatus" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- 1st Alarm Condition End here --%>

                        <%-- 2nd Alarm Condition starts here --%>
                        <div id="div2ndAlarm" class="panel panelStyle">
                            <%--2nd Alarm Condition panel header --%>
                            <div class="panel-heading PanelHeadingStyle">
                                <div class="col-md-1">
                                    <span class="sprBadge badge-default blackColor">
                                        <asp:Label ID="Label1" Text="<%$Resources:Two%>" runat="server" />
                                    </span>
                                </div>
                                <div class="col-md-3 padL0">
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Text="<%$Resources:2ndAlarmHeader%>" ReadOnly="true" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/common/img/excl.png" onmouseover="showhide()" onmouseleave="showhide()"/>
                                </div>
                                <div class="col-md-1 pull-right">
                                    <a id="btnCloseOpen2ndAlarm" data-toggle="collapse" href="#<%=collapse2ndAlarm.ClientID%>">
                                        <asp:Button ID="btn2ndAlarmOpenClose" runat="server" CssClass="btn btn-default btnPTB3 btnEdit collapse-target" Text="<%$Resources:Open%>" /></a>
                                </div>
                            </div>
                            <%--2nd Alarm Condition panel body --%>
                            <div id='collapse2ndAlarm' class="panel-collapse collapse bgwhite" runat="server">
                                <div class="row">
                                    <div class="text-center col-md-12 rwPadLR0 rwMarTB15">
                                        <%--2nd Alarm Condition High Risk--%>
                                        <div class="col-md-4 margin-top15">
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="commonStyle bg2ndAlarmcolor1">
                                                        <asp:Label ID="Label25" Text="<%$Resources:2ndAlarmHighRisk%>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label26" Text="<%$Resources:2ndAlarmOccurredLbl %>" runat="server" />
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <div class="col-md-6 fontstyleTelerik col-md-offset-3">
                                                                <telerik:RadComboBox ID="cbo2ndAlarmHighThresholdValue" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <asp:Label ID="Label62" Text="<%$Resources:1stAlarmMinutesLbl%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgYes">
                                                            <asp:Label ID="Label27" Text="<%$Resources:Yes %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgNo">
                                                            <asp:Label ID="Label28" Text="<%$Resources:No %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink7" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo2ndAlarmHighThresholdYesContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink31" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo2ndAlarmHighThresholdYesContactList" ValidationGroup="2ndAlarm" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink8" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo2ndAlarmHighThresholdNoContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink32" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo2ndAlarmHighThresholdNoContactList" ValidationGroup="2ndAlarm" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--2nd Alarm Condition Medium Risk--%>
                                        <div class="col-md-4 margin-top15">
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="commonStyle bg2ndAlarmcolor1">
                                                        <asp:Label ID="Label33" Text="<%$Resources:2ndAlarmMediumRisk %>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label34" Text="<%$Resources:2ndAlarmOccurredLbl %>" runat="server" />
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <div class="col-md-6 fontstyleTelerik col-md-offset-3">
                                                                <telerik:RadComboBox ID="cbo2ndAlarmMediumThresholdValue" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <asp:Label ID="Label10" Text="<%$Resources:1stAlarmMinutesLbl%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgYes">
                                                            <asp:Label ID="Label35" Text="<%$Resources:Yes %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgNo">
                                                            <asp:Label ID="Label36" Text="<%$Resources:No %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink9" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo2ndAlarmMediumThresholdYesContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink33" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                     <div class="popup3" >
                                                             <span class="popuptext" id="helpText5"> 
                                                             </span>
                                                     </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo2ndAlarmMediumThresholdYesContactList" ValidationGroup="2ndAlarm" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink10" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo2ndAlarmMediumThresholdNoContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink34" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>

                                                     <div class="popup2" >
                                                             <span class="popuptext" id="helpText4"> 
                                                             </span>
                                                     </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo2ndAlarmMediumThresholdNoContactList" ValidationGroup="2ndAlarm" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--2nd Alarm Condition Low Risk--%>
                                        <div class="col-md-4 margin-top15">
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="commonStyle bg2ndAlarmcolor1">
                                                        <asp:Label ID="Label41" Text="<%$Resources:2ndAlarmLowRisk %>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12">
                                                    <div class="col-md-12 rwPadLR0 commonStyle bg1stAlarmcolor2">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label42" Text="<%$Resources:2ndAlarmOccurredLbl %>" runat="server" />
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <div class="col-md-6 fontstyleTelerik col-md-offset-3">
                                                                <telerik:RadComboBox ID="cbo2ndAlarmLowThresholdValue" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwMargT10">
                                                            <asp:Label ID="Label12" Text="<%$Resources:1stAlarmMinutesLbl%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgYes">
                                                            <asp:Label ID="Label43" Text="<%$Resources:Yes %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <div class="commonStyle bgNo">
                                                            <asp:Label ID="Label44" Text="<%$Resources:No %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink11" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo2ndAlarmLowThresholdYesContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink35" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo2ndAlarmLowThresholdYesContactList" ValidationGroup="2ndAlarm" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="commonStyle bg1stAlarmcolor2 col-md-12">
                                                        <asp:HyperLink ID="hyperlink12" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        <telerik:RadComboBox ID="cbo2ndAlarmLowThresholdNoContactList" RenderMode="Lightweight" runat="server" Style="width: 100%;" CssClass="input-md" Filter="StartsWith"
                                                            MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        <asp:HyperLink ID="hyperlink36" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cbo2ndAlarmLowThresholdNoContactList" ValidationGroup="2ndAlarm" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 margin-top15">
                                                <div class="pull-right">
                                                    <asp:Button ID="btnSave2ndAlarmCondition" CssClass="btn btn-warning btnLogOff" runat="server" ValidationGroup="2ndAlarm" Text="<%$Resources:Save%>" />
                                                </div>
                                            </div>
                                            <div class="col-md-12 margin-top5">
                                                <div class="pull-right">
                                                    <asp:Label ID="lbl2ndAlarmUpdateStatus" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- 2nd Alarm Condition End here --%>

                        <%-- Fault Condition starts here --%>
                        <div id="divFaultCondition" class="panel panelStyle">
                            <%--Fault Condition panel header --%>
                            <div class="panel-heading PanelHeadingStyle">
                                <div class="col-md-1">
                                    <span class="sprBadge badge-default blackColor">
                                        <asp:Label ID="Label2" Text="<%$Resources:Three%>" runat="server" />
                                    </span>
                                </div>
                                <div class="col-md-3 padL0">
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" Text="<%$Resources:FaultConditionHeader%>" ReadOnly="true" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/common/img/excl.png" onmouseover="showhideFault()" onmouseleave="showhideFault()"/>
                                </div>
                                <div class="col-md-1 pull-right">
                                    <a id="btnCloseOpenFaultCondition" data-toggle="collapse" href="#<%=collapseFaultCondition.ClientID%>">
                                        <asp:Button ID="btnFaultConditionOpenClose" runat="server" CssClass="btn btn-default btnPTB3 btnEdit collapse-target" Text="<%$Resources:Open%>" /></a>
                                </div>
                            </div>
                            <%--Fault Condition panel body --%>
                            <div id='collapseFaultCondition' class="panel-collapse collapse bgwhite" runat="server">
                                <div class="row">
                                    <div class="text-center col-md-12">
                                        <div class="col-md-11 col-md-offset-0">
                                            <div class="col-md-12 margin-top15">
                                                <div class="col-md-offset-4 col-md-4" style="padding-left: 9px; padding-right: 11px;">
                                                    <div class="commonStyle bgFaultCondColor1">
                                                        <asp:Label ID="Label5" Text="<%$Resources:FaultConditionHeader%>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-8 col-md-offset-2" style="border-bottom: 1px solid black;">
                                            </div>
                                            <div class="col-md-12 rwPadLR0" style="top: -3px;">
                                                <div class="col-md-4 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="commonStyle bgFaultCondColor1">
                                                            <asp:Label ID="Label15" Text="<%$Resources:HighRisk%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0">
                                                        <div class="commonStyle bgFaultCondColor1">
                                                            <asp:Label ID="Label16" Text="<%$Resources:MediumRisk%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0">
                                                        <div class="commonStyle bgFaultCondColor1">
                                                            <asp:Label ID="Label17" Text="<%$Resources:LowRisk%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-4 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 commonStyle bgFaultCondColor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink13" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 rwPadLR0">
                                                            <telerik:RadComboBox ID="cboFaultHighContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink37" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboFaultHighContactList" ValidationGroup="FaultCondition" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 commonStyle bgFaultCondColor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink14" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 rwPadLR0">
                                                             <div class="popup" >
                                                             <span class="popuptext" id="helpText6"> 
                                                             </span>
                                                             </div>
                                                            <telerik:RadComboBox ID="cboFaultMediumContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink38" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboFaultMediumContactList" ValidationGroup="FaultCondition" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 commonStyle bgFaultCondColor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink15" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 rwPadLR0">
                                                            <telerik:RadComboBox ID="cboFaultLowContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink39" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboFaultLowContactList" ValidationGroup="FaultCondition" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 rwMarTB15">
                                            <div class="pull-right">
                                                <asp:Button ID="btnSaveFaultCondition" CssClass="btn btn-warning btnLogOff" runat="server" ValidationGroup="FaultCondition" Text="<%$Resources:Save%>" />
                                            </div>
                                        </div>
                                        <div class="col-md-12 margin-top5">
                                            <div class="pull-right">
                                                <asp:Label ID="lblFaultConditionUpdateStatus" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- Fault Condition End here --%>

                        <%-- Loss Of Comms starts here --%>
                        <div id="divLossOfComms" class="panel panelStyle">
                            <%--Loss Of Comms panel header --%>
                            <div class="panel-heading PanelHeadingStyle">
                                <div class="col-md-1">
                                    <span class="sprBadge badge-default blackColor">
                                        <asp:Label ID="Label3" Text="<%$Resources:Four%>" runat="server" />
                                    </span>
                                </div>
                                <div class="col-md-3 padL0">
                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" Text="<%$Resources:LossOfCommsHeader%>" ReadOnly="true" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/common/img/excl.png" onmouseover="showhideLoss()" onmouseleave="showhideLoss()"/>
                                </div>
                                <div class="col-md-1 pull-right">
                                    <a id="btnCloseOpenLossOfComms" data-toggle="collapse" href="#<%=collapseLossOfComms.ClientID%>">
                                        <asp:Button ID="btnLossOfCommsOpenClose" runat="server" CssClass="btn btn-default btnPTB3 btnEdit collapse-target" Text="<%$Resources:Open%>" /></a>
                                </div>
                            </div>
                            <%-- Loss Of Comms panel body --%>
                            <div id='collapseLossOfComms' class="panel-collapse collapse bgwhite" runat="server">
                                <div class="row">
                                    <div class="text-center col-md-12 margin-top15">
                                        <div class="col-md-11 col-md-offset-0">
                                            <div class="col-md-12 margin-top15">
                                                <div class="col-md-offset-4 col-md-4" style="padding-left: 10px; padding-right: 8px;">
                                                    <div class="commonStyle bgLossColor1">
                                                        <asp:Label ID="Label11" Text="<%$Resources:GatewayCommunicationLbl%>" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0 text-center">
                                                <span class="glyphicon glyphicon-arrow-down"></span>
                                            </div>
                                            <div class="col-md-8 col-md-offset-2" style="border-bottom: 1px solid black;">
                                            </div>
                                            <div class="col-md-12 rwPadLR0" style="top: -3px;">
                                                <div class="col-md-4 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="commonStyle bgLossColor1">
                                                            <asp:Label ID="Label13" Text="<%$Resources:HighRisk%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="commonStyle bgLossColor1">
                                                            <asp:Label ID="Label14" Text="<%$Resources:MediumRisk%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 rwPadLR0">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="commonStyle bgLossColor1">
                                                            <asp:Label ID="Label18" Text="<%$Resources:LowRisk%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-4 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 commonStyle bgLossColor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink16" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 rwPadLR0">
                                                            <telerik:RadComboBox ID="cboLossOfCommsHighContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink40" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboLossOfCommsHighContactList" ValidationGroup="LossOfComms" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 commonStyle bgLossColor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink17" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 rwPadLR0">
                                                             <div class="popup" >
                                                             <span class="popuptext" id="helpText7"> 
                                                             </span>
                                                                 </div>
                                                            <telerik:RadComboBox ID="cboLossOfCommsMediumContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink41" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboLossOfCommsMediumContactList" ValidationGroup="LossOfComms" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4 fontstyleTelerik">
                                                    <div class="col-md-12 rwPadLR0 text-center">
                                                        <span class="glyphicon glyphicon-arrow-down"></span>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 commonStyle bgLossColor2">
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink18" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-4 rwPadLR0">
                                                            <telerik:RadComboBox ID="cboLossOfCommsLowContactList" RenderMode="Lightweight" runat="server" Style="width: 110%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                        </div>
                                                        <div class="col-md-4 top5">
                                                            <asp:HyperLink ID="hyperlink42" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 rwPadLR0 ">
                                                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboLossOfCommsLowContactList" ValidationGroup="LossOfComms" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 rwMarTB15">
                                            <div class="pull-right">
                                                <asp:Button ID="btnSaveLossofComms" CssClass="btn btn-warning btnLogOff" ValidationGroup="LossOfComms" runat="server" Text="<%$Resources:Save%>" />
                                            </div>
                                        </div>
                                        <div class="col-md-12 margin-top5">
                                            <div class="pull-right">
                                                <asp:Label ID="lblLossOfCommsUpdateStatus" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- Loss Of Comms End here --%>

                        <%-- Deterioration Monitor starts here --%>
                        <div id="divDeteriorationMonitor" class="panel panelStyle">
                            <%--Deterioration Monitor panel header --%>
                            <div class="panel-heading PanelHeadingStyle">
                                <div class="col-md-1">
                                    <span class="sprBadge badge-default blackColor">
                                        <asp:Label ID="Label4" Text="<%$Resources:Five%>" runat="server" />
                                    </span>
                                </div>
                                <div class="col-md-3 padL0">
                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" Text="<%$Resources:DeteriorationMonitorHeader%>" ReadOnly="true" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/common/img/excl.png" onmouseover="showhideDeterioration()" onmouseleave="showhideDeterioration()"/>
                                </div>
                                <div class="col-md-1 pull-right">
                                    <a id="btnCloseOpenDeteriorationMonitor" data-toggle="collapse" href="#<%=collapseDeteriorationMonitor.ClientID%>">
                                        <asp:Button ID="btnDeteriorationMonitorOpenClose" runat="server" CssClass="btn btn-default btnPTB3 btnEdit collapse-target" Text="<%$Resources:Open%>" /></a>
                                </div>
                            </div>
                            <%-- Deterioration Monitor panel body --%>
                            <div id='collapseDeteriorationMonitor' class="panel-collapse collapse bgwhite" runat="server">
                                <div class="row">
                                    <div class="text-center col-md-12 rwMarTB15">
                                        <%-- Deterioration Monitor High Risk --%>
                                        <div class="col-md-6 rwPadLR0">
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12 margin-top15">
                                                    <div class="col-md-offset-3 col-md-5 rwPadLR0">
                                                        <div class="commonStyle bgMonitorColor1">
                                                            <asp:Label ID="Label70" Text="<%$Resources:DeteriorationMonitorHighRisk%>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-11 rwPadLR0 text-center">
                                                    <span class="glyphicon glyphicon-arrow-down"></span>
                                                </div>
                                                <div class="col-md-6 col-md-offset-2" style="border-bottom: 1px solid black; left: 32px;">
                                                </div>
                                                <div class="col-md-12 rwPadLR0" style="top: -2px;">
                                                    <div class="col-md-5 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div class="col-md-12 rwPadLR0">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationHighMinutesAlarmEvents" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true"
                                                                        OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-8 rwPadLR0">
                                                                    <asp:Label ID="Label80" Text="<%$Resources:AlarmEventsWithin%>" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 rwPadLR0 margin-top15">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationHighMinutesValue" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label81" Text="<%$Resources:Minutes%>" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5 col-md-offset-1 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div class="col-md-12 rwPadLR0">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationHighDaysAlarmEvents" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-8 rwPadLR0">
                                                                    <asp:Label ID="Label82" Text="<%$Resources:AlarmEventsWithin%>" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 rwPadLR0 margin-top15">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationHighDaysValue" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label83" Text="<%$Resources:Days %>" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 rwPadLR0">
                                                    <div class="col-md-5 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <asp:HyperLink ID="hyperlink19" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                            <telerik:RadComboBox ID="cboDeteriorationHighMinutesContact" RenderMode="Lightweight" runat="server" Style="width: 80%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID"
                                                                OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                            <asp:HyperLink ID="hyperlink43" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 ">
                                                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboDeteriorationHighMinutesContact" ValidationGroup="DeteriorationMonitor " runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5 col-md-offset-1 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div>
                                                                <div class="popup" >
                                                             <span class="popuptext" id="helpText8"> 
                                                             </span>
                                                                 </div>

                                                                <asp:HyperLink ID="hyperlink20" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                                <telerik:RadComboBox ID="cboDeteriorationHighDaysContact" RenderMode="Lightweight" runat="server" Style="width: 80%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                <asp:HyperLink ID="hyperlink44" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                            </div>
                                                            <div class="col-md-12 top5">
                                                                <asp:Label ID="Label108" Text="<%$Resources:EscalationNoticeLbl %>" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 ">
                                                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboDeteriorationHighDaysContact" ValidationGroup="DeteriorationMonitor" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- Deterioration Monitor Medium Risk --%>
                                        <div class="col-md-6 rwPadLR0">
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12 margin-top15">
                                                    <div class="col-md-offset-3 col-md-5 rwPadLR0">
                                                        <div class="commonStyle bgMonitorColor1">
                                                            <asp:Label ID="Label85" Text="<%$Resources:DeteriorationMonitorMediumRisk %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-11 rwPadLR0 text-center">
                                                    <span class="glyphicon glyphicon-arrow-down"></span>
                                                </div>
                                                <div class="col-md-6 col-md-offset-2" style="border-bottom: 1px solid black; left: 32px;">
                                                </div>
                                                <div class="col-md-12 rwPadLR0" style="top: -2px;">
                                                    <div class="col-md-5 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div class="col-md-12 rwPadLR0">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationMediumMinutesAlarmEvents" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-8 rwPadLR0">
                                                                    <asp:Label ID="Label86" Text="<%$Resources:AlarmEventsWithin%>" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 rwPadLR0 margin-top15">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationMediumMinutesValue" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label87" Text="<%$Resources:Minutes%>" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5 col-md-offset-1 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div class="col-md-12 rwPadLR0">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationMediumDaysAlarmEvents" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-8 rwPadLR0">
                                                                    <asp:Label ID="Label88" Text="<%$Resources:AlarmEventsWithin%>" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 rwPadLR0 margin-top15">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationMediumDaysValue" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label89" Text="<%$Resources:Days %>" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 rwPadLR0">
                                                    <div class="col-md-5 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div class="col-md-12">
                                                                <asp:HyperLink ID="hyperlink21" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                                <telerik:RadComboBox ID="cboDeteriorationMediumMinutesContact" RenderMode="Lightweight" runat="server" Style="width: 80%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                <asp:HyperLink ID="hyperlink45" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 ">
                                                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboDeteriorationMediumMinutesContact" ValidationGroup="DeteriorationMonitor" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5 col-md-offset-1 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div>
                                                                <asp:HyperLink ID="hyperlink22" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                                <telerik:RadComboBox ID="cboDeteriorationMediumDaysContact" RenderMode="Lightweight" runat="server" Style="width: 80%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                <asp:HyperLink ID="hyperlink46" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                            </div>
                                                            <div class="col-md-12 top5">
                                                                <asp:Label ID="Label107" Text="<%$Resources:EscalationNoticeLbl %>" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 ">
                                                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboDeteriorationMediumDaysContact" ValidationGroup="DeteriorationMonitor" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- Deterioration Monitor Low Risk --%>
                                    <div class="text-center col-md-12 rwMarTB15">
                                        <div class="col-md-6 rwPadLR0 col-md-offset-3">
                                            <div class="col-md-12 rwPadLR0">
                                                <div class="col-md-12 margin-top15">
                                                    <div class="col-md-offset-3 col-md-5 rwPadLR0">
                                                        <div class="commonStyle bgMonitorColor1">
                                                            <asp:Label ID="Label97" Text="<%$Resources:DeteriorationMonitorLowRisk %>" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-11 rwPadLR0 text-center">
                                                    <span class="glyphicon glyphicon-arrow-down"></span>
                                                </div>
                                                <div class="col-md-6 col-md-offset-2" style="border-bottom: 1px solid black; left: 23px;">
                                                </div>
                                                <div class="col-md-12 rwPadLR0" style="top: -3px;">
                                                    <div class="col-md-5 fontstyleTelerik padR0">
                                                        <div class="col-md-11 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div class="col-md-12 rwPadLR0">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationLowMinutesAlarmEvents" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-8 rwPadLR0">
                                                                    <asp:Label ID="Label98" Text="<%$Resources:AlarmEventsWithin%>" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 rwPadLR0 margin-top15">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationLowMinutesValue" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label99" Text="<%$Resources:Minutes%>" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5 col-md-offset-1 fontstyleTelerik padR0">
                                                        <div class="col-md-11 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div class="col-md-12 rwPadLR0">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationLowDaysAlarmEvents" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-8 rwPadLR0">
                                                                    <asp:Label ID="Label100" Text="<%$Resources:AlarmEventsWithin%>" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 rwPadLR0 margin-top15">
                                                                <div class="col-md-4">
                                                                    <telerik:RadComboBox ID="cboDeteriorationLowDaysValue" RenderMode="Lightweight" runat="server" Style="width: 130%;" CssClass="input-md" Filter="StartsWith"
                                                                        MarkFirstMatch="true" AllowCustomText="true" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="Label101" Text="<%$Resources:Days %>" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 rwPadLR0">
                                                    <div class="col-md-5 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <asp:HyperLink ID="hyperlink23" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                            <telerik:RadComboBox ID="cboDeteriorationLowMinutesContact" RenderMode="Lightweight" runat="server" Style="width: 80%;" CssClass="input-md" Filter="StartsWith"
                                                                MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                            <asp:HyperLink ID="hyperlink47" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 ">
                                                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboDeteriorationLowMinutesContact" ValidationGroup="DeteriorationMonitor" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5 col-md-offset-1 fontstyleTelerik padR0">
                                                        <div class="col-md-12 rwPadLR0 text-center">
                                                            <span class="glyphicon glyphicon-arrow-down"></span>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 commonStyle bgMonitorColor2">
                                                            <div>
                                                                <asp:HyperLink ID="hyperlink24" Text="<%$Resources:Message %>" runat="server" NavigateUrl="#" />
                                                                <telerik:RadComboBox ID="cboDeteriorationLowDaysContact" RenderMode="Lightweight" runat="server" Style="width: 80%;" CssClass="input-md" Filter="StartsWith"
                                                                    MarkFirstMatch="true" AllowCustomText="true" DataTextField="Description" DataValueField="ContactListID" OnClientKeyPressing="RadComboKeyPress" OnClientBlur="RadComboBlurred" />
                                                                <asp:HyperLink ID="hyperlink48" Text="<%$Resources:contact %>" runat="server" NavigateUrl="#" />
                                                            </div>
                                                            <div class="col-md-12 top5">
                                                                <asp:Label ID="Label109" Text="<%$Resources:EscalationNoticeLbl %>" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 rwPadLR0 ">
                                                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:FieldRequired %>" ForeColor="Red" ControlToValidate="cboDeteriorationLowDaysContact" ValidationGroup="DeteriorationMonitor" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 rwMarTB15">
                                            <div class="pull-right">
                                                <asp:Button ID="btnSaveDeteriorationLevels" CssClass="btn btn-warning btnLogOff" ValidationGroup="DeteriorationMonitor" runat="server" Text="<%$Resources:Save%>" />
                                            </div>
                                        </div>
                                        <div class="col-md-12 margin-top5">
                                            <div class="pull-right">
                                                <asp:Label ID="lblDeteriorationStatus" runat="server" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <%-- Deterioration Monitor End here --%>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveFaultCondition" />
                    <asp:AsyncPostBackTrigger ControlID="btnSaveDeteriorationLevels" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave1stAlarmCondition" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave2ndAlarmCondition" />
                    <asp:AsyncPostBackTrigger ControlID="btnSaveLossOfComms" />
                    <asp:AsyncPostBackTrigger ControlID="cboDistributors" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>


    <style>


/****First popup****/

    .popup {
    position: relative;
    display: inline-block;
    cursor: pointer;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
}

/* The actual popup */
.popup .popuptext {
    visibility: hidden;
    width: 160px;
    background-color: #70AD47;
    color: #fff;
    text-align: center;
    border-radius: 6px;
    padding: 8px 0;
    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%;
    margin-left: -80px;

}

/* Popup arrow */
.popup .popuptext::after {
    content: "";
    position: absolute;
    top: 100%;
    left: 50%;
    margin-left: -5px;
    border-width: 5px;
    border-style: solid;
    border-color: #70AD47 transparent transparent transparent;
}


/* Toggle this class - hide and show the popup */
.popup .show {
    visibility: visible;
    -webkit-animation: fadeIn 1s;
    animation: fadeIn 1s;
}

/* Add animation (fade in the popup) */
@-webkit-keyframes fadeIn {
    from {opacity: 0;} 
    to {opacity: 1;}
}

@keyframes fadeIn {
    from {opacity: 0;}
    to {opacity:1 ;}
}

/*Second ppopup*/

 .popup1 {
    position: relative;
    display: inline-block;
    float:right;
    cursor: pointer;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
}

/* The actual popup */
.popup1 .popuptext {
    visibility: hidden;
    width: 160px;
    background-color: #70AD47;
    color: #fff;
    text-align: center;
    border-radius: 6px;
    padding: 8px 0;
    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%;
    margin-left: -80px;
}

/* Popup arrow */
.popup1 .popuptext::after {
    content: "";
    position: absolute;
    top: 100%;
    left: 50%;
    margin-left: -75px;
    margin-top:-7px;
    border-width: 15px;
    border-style: solid;
    transform: rotate(-70deg);
    border-color: #70AD47 #70AD47 transparent transparent;
}


/* Toggle this class - hide and show the popup */
.popup1 .show {
    visibility: visible;
    -webkit-animation: fadeIn 1s;
    animation: fadeIn 1s;
}


/*****Third popup******/


.popup2 {
    position: relative;
    /*display: inline-block;*/
    float:right;
    cursor: pointer;
    margin-right:-42px;
    margin-top:15px;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
}

/* The actual popup */
.popup2 .popuptext {
    visibility: hidden;
    width: 200px;
    background-color: #70AD47;
    color: #fff;
    text-align: center;
    border-radius: 6px;
    padding: 8px 0;
    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%;
    margin-left: -80px;
}

/* Popup arrow */
.popup2 .popuptext::after {
    content: "";
    position: absolute;
    top: 100%;
    left: 50%;
    margin-left: -90px;
    margin-top:-8px;
    border-width: 20px;
    border-style: solid;
    transform: rotate(-70deg);
    border-color: #70AD47 #70AD47 transparent transparent;
}


/* Toggle this class - hide and show the popup */
.popup2 .show {
    visibility: visible;
    -webkit-animation: fadeIn 1s;
    animation: fadeIn 1s;
}


/****Fourth Popup****/

.popup3 {
    position: relative;
    display: inline-block;
    float:left;
    cursor: pointer;
    margin-left:-190px;
    margin-top:15px;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
}

/* The actual popup */
.popup3 .popuptext {
    visibility: hidden;
    width: 150px;
    background-color: #70AD47;
    color: #fff;
    text-align: center;
    border-radius: 6px;
    padding: 8px 0;
    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%;
    margin-left: -80px;
}

/* Popup arrow */
.popup3 .popuptext::after {
    content: "";
    position: absolute;
    top: 100%;
    left: 50%;
    margin-left:40px;
    margin-top:-7px;
    border-width: 15px;
    border-style: solid;
    transform: rotate(-23deg);
    border-color: #70AD47 #70AD47 transparent transparent;
}


/* Toggle this class - hide and show the popup */
.popup3 .show {
    visibility: visible;
    -webkit-animation: fadeIn 1s;
    animation: fadeIn 1s;
}


</style>

    <script type="text/javascript">

        var OpenResource = '<%=GetLocalResourceObject("Open")%>';
        var CloseResource = '<%=GetLocalResourceObject("Close")%>';
        //declaring variables for popup span
        var popup = document.getElementById('helpText');
        var popup1 = document.getElementById('helpText1');
        var popup2 = document.getElementById('helpText2');
        var popup3 = document.getElementById('helpText3');
        var popup4 = document.getElementById('helpText4');
        var popup5 = document.getElementById('helpText5');
        var popup6 = document.getElementById('helpText6');
        var popup7 = document.getElementById('helpText7');
        var popup8 = document.getElementById('helpText8');

        function SetHelpText() {
            popup = document.getElementById('helpText');
            popup1 = document.getElementById('helpText1');
            popup2 = document.getElementById('helpText2');
            popup3 = document.getElementById('helpText3');
            popup4 = document.getElementById('helpText4');
            popup5 = document.getElementById('helpText5');
            popup6 = document.getElementById('helpText6');
            popup7 = document.getElementById('helpText7');
            popup8 = document.getElementById('helpText8');
        }

        //This method shows and hides popups for 1st alarm
        function show()
        {
            var combo = $find('<%=cboMediumContactList.ClientID%>').get_selectedItem().get_value();
            var message = $find('<%=cbo1stAlarmMediumSilencedValue.ClientID%>').get_selectedItem().get_value();
            var messageNo = $find('<%=cbo1stAlarmMediumNotSilencedContactList.ClientID%>').get_selectedItem().get_value();
            var messageYes = $find('<%=cbo1stAlarmMediumSilencedContactList.ClientID%>').get_selectedItem().get_value();
            switch (combo)
            {
                case "0": $("#helpText").text('<asp:Literal runat="server" Text="<%$Resources:FirstAlarmNoFurtherAction%>"/>');break;
                case "1": $("#helpText").text('<asp:Literal runat="server" Text="<%$Resources:FirstAlarmAlert%>"/>'); break;
                case "2": $("#helpText").text('<asp:Literal runat="server" Text="<%$Resources:FirstAlarmFault%>"/>'); break;
                case "3": $("#helpText").text('<asp:Literal runat="server" Text="<%$Resources:FirstAlarmCommunity%>"/>'); break;
            }
            var resourcevalue = '<%= GetLocalResourceObject("FirstAlarmMessage") %>';
            message = (resourcevalue + " " + message + " minutes").toString();
            $("#helpText1").text(message);
           
            switch (messageNo)
            {
                case "0": $("#helpText2").text('<asp:Literal runat="server" Text="<%$Resources:MessageNoNoFurtherAction%>"/>'); break;
                case "1": $("#helpText2").text('<asp:Literal runat="server" Text="<%$Resources:MessageNoAlert%>"/>'); break;
                case "2": $("#helpText2").text('<asp:Literal runat="server" Text="<%$Resources:MessageNoFault%>"/>'); break;
                case "3": $("#helpText2").text('<asp:Literal runat="server" Text="<%$Resources:MessageNoCommunity%>"/>'); break;
            }

            switch (messageYes)
            {
                case "0": $("#helpText3").text('<asp:Literal runat="server" Text="<%$Resources:MessageYesNoFurtherAction%>"/>'); break;
                case "1": $("#helpText3").text('<asp:Literal runat="server" Text="<%$Resources:MessageYesAlert%>"/>'); break;
                case "2": $("#helpText3").text('<asp:Literal runat="server" Text="<%$Resources:MessageYesFault%>"/>'); break;
                case "3": $("#helpText3").text('<asp:Literal runat="server" Text="<%$Resources:MessageYesCommunity%>"/>'); break;
              }
            popup.classList.toggle('show');
            popup1.classList.toggle('show');
            popup2.classList.toggle('show');
            popup3.classList.toggle('show');
           
        }
        //This method shows and hide popups for 2nd alarm
        function showhide()
        {
            var secondAlarmMessageYes = $find('<%=cbo2ndAlarmMediumThresholdYesContactList.ClientID%>').get_selectedItem().get_value();
            var secondAlarmMessageNo = $find('<%=cbo2ndAlarmMediumThresholdNoContactList.ClientID%>').get_selectedItem().get_value();

            switch (secondAlarmMessageYes)
            {
                case "0": $("#helpText5").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmYesNoFurtherAction%>"/>'); break;
                case "1": $("#helpText5").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmYesAlert%>"/>'); break;
                case "2": $("#helpText5").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmYesFault%>"/>'); break;
                case "3": $("#helpText5").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmYesCommunity%>"/>'); break;
             }

            switch (secondAlarmMessageNo)
            {
                case "0": $("#helpText4").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmNoNoFurtherAction%>"/>'); break;
                case "1": $("#helpText4").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmNoAlert%>"/>'); break;
                case "2": $("#helpText4").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmNoFault%>"/>'); break;
                case "3": $("#helpText4").text('<asp:Literal runat="server" Text="<%$Resources:SecondAlarmNoCommunity%>"/>'); break;
            }

            popup4.classList.toggle('show');
            popup5.classList.toggle('show');
        }

        //This method shows and hide popups for Fault Condition
        function showhideFault()
        {
            var faultCondition = $find('<%=cboFaultMediumContactList.ClientID%>').get_selectedItem().get_value();
            switch (faultCondition)
            {
                case "0": $("#helpText6").text('<asp:Literal runat="server" Text="<%$Resources:FaultNoFurtherAction%>"/>'); break;
                case "1": $("#helpText6").text('<asp:Literal runat="server" Text="<%$Resources:FaultAlert%>"/>'); break;
                case "2": $("#helpText6").text('<asp:Literal runat="server" Text="<%$Resources:FaultFault%>"/>'); break;
                case "3": $("#helpText6").text('<asp:Literal runat="server" Text="<%$Resources:FaultCommunity%>"/>'); break;
            }
            popup6.classList.toggle('show');
        }

    //This method shows and hide popups for Loss of Comms
    function showhideLoss()
    {
        var LossOfComms = $find('<%=cboLossOfCommsMediumContactList.ClientID%>').get_selectedItem().get_value();

            switch (LossOfComms)
            {
                case "0": $("#helpText7").text('<asp:Literal runat="server" Text="<%$Resources:LossNoFurtherAction%>"/>'); break;
                case "1": $("#helpText7").text('<asp:Literal runat="server" Text="<%$Resources:LossAlert%>"/>'); break;
                case "2": $("#helpText7").text('<asp:Literal runat="server" Text="<%$Resources:LossFault%>"/>'); break;
                case "3": $("#helpText7").text('<asp:Literal runat="server" Text="<%$Resources:LossCommunity%>"/>'); break;
            }

            popup7.classList.toggle('show');
        }
        //This method shows and hide popups for Deterioration Monitor
        function showhideDeterioration()
        {
            var DeteriorationMonitor = $find('<%=cboDeteriorationHighDaysContact.ClientID%>').get_selectedItem().get_value();

            switch (DeteriorationMonitor)
            {
                case "0": $("#helpText8").text('<asp:Literal runat="server" Text="<%$Resources:DeteriorationNoFurtherAction%>"/>'); break;
                case "1": $("#helpText8").text('<asp:Literal runat="server" Text="<%$Resources:DeteriorationAlert%>"/>'); break;
                case "2": $("#helpText8").text('<asp:Literal runat="server" Text="<%$Resources:DeteriorationFault%>"/>'); break;
                case "3": $("#helpText8").text('<asp:Literal runat="server" Text="<%$Resources:DeteriorationCommunity%>"/>'); break;
            }

            popup8.classList.toggle('show');
        }

        Sys.Application.add_load(function () {
            $(".panel-collapse")
                .onIfNotPresent("hide.bs.collapse", "hide", function (e) {
                    changeAccordionTriggerText(e.currentTarget, OpenResource);
                })
                .onIfNotPresent("show.bs.collapse", "show", function (e) {
                    changeAccordionTriggerText(e.currentTarget, CloseResource);
                });

            function changeAccordionTriggerText(accordion, text) {
                $(".panel-heading a[data-toggle='collapse'][href='#" + accordion.id + "']")
                    .find("button, input[type=submit], input[type=button]")
                    .val(text);
            }
        });

        </script>


</asp:Content>
