<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_GatewaySearch" CodeBehind="GatewaySearch.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <div id="content">
        <div id="box">
            <asp:UpdatePanel ID="updPanel1" runat="server">
                <ContentTemplate>
                    <asp:Literal ID="litPanelStyles" runat="server" />
                    <%--<h3>Gateways</h3>--%>
                    <asp:ValidationSummary ID="vsErrors" runat="server" Style="" />

                    <div id="divPanelSearchCrit">
                        <div class=" col-sm-10 margin-left-125">
                            <asp:Label ID="lblSearch" runat="server" Text="<%$Resources:PageGlobalResources,SelectSearchFieldsPrompt %>"></asp:Label>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-6">
                                <asp:Label ID="lblMacAddress" runat="server" Text="Mac Address:" CssClass="control-label col-sm-3 text-right"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtMac" MaxLength="17" CssClass="form-control"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revMacAddress" runat="server" Text="*" ErrorMessage="Mac address must consist of 0-9, A-F or :"
                                        ControlToValidate="txtMac" ValidationExpression="^[0-9A-Fa-f:]*$" Style="width: auto;"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <%--<div>
                                <span>Firmware:</span>
                                <asp:DropDownList runat="server" ID="ddlFirmware" />
                            </div>--%>
                        </div>
                        <%--   <div>
                            <div class="panelDistShow">
                                <span>Distributor:</span>
                                <asp:DropDownList ID="ddlDistributor" runat="server" AutoPostBack="true" DataTextField="Name" DataValueField="MasterCoID" />
                            </div>
                            <div class="panelOpShow">
                                <span>OpCo:</span>
                                <asp:DropDownList ID="ddlOpCo" runat="server" DataTextField="Name" DataValueField="MasterCoID" />
                            </div>
                        </div>--%>
                        <div class="col-sm-12" style="">
                            <div class="col-sm-6 " style="padding-right: 15px;">
                                <asp:Button ID="cmdSearch" runat="server" class="btn btn-primary btn-xs" Text="Search" Style="margin-left: 20px" />
                                <div class="col-sm-6" style="float: left; text-align: left">
                                    <span class="col-sm-6" style="float: left; text-align: right">Page Size:&nbsp;</span>
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" Style="width: auto;" CssClass="form-control">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem Selected="True">50</asp:ListItem>
                                        <asp:ListItem>60</asp:ListItem>
                                        <asp:ListItem>70</asp:ListItem>
                                        <asp:ListItem>80</asp:ListItem>
                                        <asp:ListItem>90</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>
                        <div style="clear: both; width: 100%">
                            <style type="text/css">
                                .panelRow a, .cameraRow a:visited {
                                    text-decoration: none;
                                }

                                    .panelRow a:hover {
                                        text-decoration: underline;
                                    }

                                    .panelRow a[disabled=disabled]:hover {
                                        text-decoration: none;
                                    }
                            </style>
                            <asp:Label ID="lblNoData" runat="server" CssClass="PageText margin-left-125" Visible="false" Text="Your search criteria did not return any records" EnableViewState="false"></asp:Label>
                            <div class="col-sm-12" id="grdPanels" style="">

                                <asp:GridView ID="dgPanels" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CaptionAlign="Top" CellSpacing="2" CellPadding="5"
                                    Font-Overline="False" ForeColor="#333333" PageSize="50" BorderStyle="None"
                                    BorderWidth="0" GridLines="None" Font-Size="Small" Width="100%" DataKeyNames="AccountID,PropertyID,MACAddress">
                                    <FooterStyle BackColor="#5D99A3" Font-Bold="False" ForeColor="White" />
                                    <RowStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" CssClass="panelRow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Account" SortExpression="AccountID" HeaderStyle-Width="85px">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CausesValidation="false" ID="lnkAccount" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex")%>' Text='<%# Eval("AccountID")%>' CommandName="Diagnostics" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Property" SortExpression="PropertyID" HeaderStyle-Width="65px">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CausesValidation="false" ID="lnkProperty" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex")%>' Text='<%# Eval("PropertyID")%>' CommandName="Diagnostics" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MACAddress" HeaderText="MAC" ReadOnly="True" SortExpression="MACAddress" HeaderStyle-Width="130px" />
                                        <asp:BoundField DataField="IsOnline" HeaderText="Is Online" ReadOnly="True" SortExpression="IsOnline" HeaderStyle-Width="130px" />
                                        <asp:TemplateField HeaderText="Firmware" SortExpression="FirmwareVersion" HeaderStyle-Width="180px">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CausesValidation="false" ID="lnkFirmware" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex")%>' Text='<%# Eval("FirmwareVersion")%>' CommandName="Firmware" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                    <AlternatingRowStyle BackColor="#F5F5F5" ForeColor="Black" CssClass="cameraRow" />
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="dataPager" PagerTextFormat="<%$Resources:PageGlobalResources,GridPagerTextFormat %>"
                                        PageSizeLabelText="<%$Resources:PageGlobalResources,GridPageSizeText %>" NextPageToolTip="<%$Resources:PageGlobalResources,GridNextPageTooltip %>"
                                        PrevPageToolTip="<%$Resources:PageGlobalResources,GridPreviousPageTooltip %>" LastPageToolTip="<%$Resources:PageGlobalResources,GridLastPageTooltip %>"
                                        FirstPageToolTip="<%$Resources:PageGlobalResources,GridFirstPageTooltip %>" NextPagesToolTip="<%$Resources:PageGlobalResources,GridNextPagesTooltip %>"
                                        PrevPagesToolTip="<%$Resources:PageGlobalResources,GridPreviousPagesTooltip %>" />
                                    <HeaderStyle BackColor="#5D99A3" Font-Bold="False" ForeColor="white" />
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName="SelectedIndexChanged" />--%>
                    <asp:AsyncPostBackTrigger ControlID="dgPanels" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="dgPanels" EventName="PageIndexChanging" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPageSize" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
