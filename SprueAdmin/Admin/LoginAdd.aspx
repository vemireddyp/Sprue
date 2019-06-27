<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.LoginAdd" CodeBehind="LoginAdd.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row lblPT12 bgwhite rwMargLR0 borderBlack">
                <div class="col-md-12 rwPadTB9 bgTheme txtcolor margin-top15">
                    <div class="col-md-5">
                        <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Users" />
                    </div>
                </div>
                <div class="row" style="margin-top: 120px;">
                    <div class="col-md-6">
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:Label ID="lblEmail" CssClass="lblfont13" Text="<%$Resources:PageGlobalResources,EmailLabel %>" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:TextBox ID="txtEmailAdd" CssClass="form-control inputhgt" runat="server" ValidationGroup="Logon"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailAddRequiredFieldValidator" runat="server" ValidationGroup="Logon" ControlToValidate="txtEmailAdd"
                                    CssClass="RedNoticeText" Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,EmailRequiredMsg %>" ForeColor="red"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator
                                    ID="EmailAddRegularExpressionValidator" runat="server" ControlToValidate="txtEmailAdd" ValidationGroup="Logon" CssClass="RedNoticeText"
                                    Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,EmailFormatMsg %>" SetFocusOnError="True" ForeColor="red"
                                    ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:Label ID="Label2" CssClass="lblfont13" Text="<%$Resources:PageGlobalResources,ConfirmEmailLabel %>" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txtEmailAddConfirm" runat="server" CssClass="form-control inputhgt" ValidationGroup="Logon"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EmailAddConfirmRequiredFieldValidator" runat="server" ValidationGroup="Logon" ControlToValidate="txtEmailAddConfirm"
                                CssClass="RedNoticeText" Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,EmailRequiredMsg %>" ForeColor="red"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>

                            <asp:CompareValidator ID="EmailAddConfirmCompareValidator" ValidationGroup="Logon" runat="server" BorderStyle="None" ControlToCompare="txtEmailAdd"
                                ControlToValidate="txtEmailAddConfirm" CssClass="RedNoticeText" Display="Dynamic" ForeColor="red"
                                ErrorMessage="<%$Resources:PageGlobalResources,EmailNotMatchError %>" SetFocusOnError="True"></asp:CompareValidator>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="col-md-4" id="divCompany" runat="server">
                            <div class="form-group">
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:PageGlobalResources,MasterCoLabel %>" CssClass="lblfont13"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlCompany" runat="server" DataTextField="Name" CssClass="form-control inputddl" DataValueField="MasterCoID" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-4" id="divUserType" runat="server">
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:PageGlobalResources,UserTypeLabel %>" CssClass="lblfont13"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control inputddl">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 rwMarTB15">
                    <div class="col-md-12">
                        <div class="col-md-9">
                            <div class="form-group" runat="server" id="divUserAdded" visible="false">
                                <asp:Label ID="Label1" Text="<%$Resources:InvitationSentText %>" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-">
                            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-warning btnLogOff validateAll pull-right" Text="<%$Resources:PageGlobalResources,SaveButton %>" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
