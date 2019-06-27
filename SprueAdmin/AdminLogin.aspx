<%@ Page Language="VB" MasterPageFile="Site.master" AutoEventWireup="false" Inherits="SprueAdmin.AdminLogin" CodeBehind="AdminLogin.aspx.vb" %>

<%@ Register Src="~/UserControls/LanguageSelector.ascx" TagName="LanguageSelector" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="siteContentHeader" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="siteBodyContent" runat="Server">

    <div id="loginCntrl" class="row">
        <div id="login-wrapper">
            <div class="col-md-12">
                <div runat="server" id="divCulture">
                    <uc1:LanguageSelector id="lngSelector" runat="server" />
                </div>
            </div>
            <div class="login-inner-container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-12 margin-top15">
                                <span class="label">
                                    <asp:Image runat="server" ImageUrl="~/common/img/login-logo.png" />
                                </span>
                            </div>
                            <div class="col-md-12 margin-top15">
                                <span class="label">
                                    <asp:Image runat="server" ImageUrl="~/common/img/login-wifi.png" /></span>
                            </div>
                            <div class="col-md-12 margin-top15">
                                <div class="label">
                                    <h2 style="margin: 0;">
                                        <asp:Literal runat="server" Text="<%$Resources:PageGlobalResources,AdministrationSystem %>" /></h2>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form">
                <div class="col-md-12 rwMargT15">
                    <asp:TextBox CssClass="form-control-login" placeholder="<%$Resources:PageGlobalResources,Username %>" runat="server" ID="txtusername"></asp:TextBox>
                </div>
                <div class="col-md-12 rwMargT15">
                    <asp:TextBox CssClass="form-control-login" placeholder="<%$Resources:PageGlobalResources,Password %>" runat="server" TextMode="password" ID="txtPasswoard"></asp:TextBox>
                </div>
                <div class="col-md-12 rwMargT15">
                    <asp:Button ID="btnSignIn" runat="server" CssClass="btn-login btn-warning-login" Text="<%$Resources:SignIn %>" />
                </div>
                <div class="col-md-12 rwMargT15">
                    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="colorRed" />
                </div>

                <div class="col-md-12 rwMargT15">
                    <a id="lnkForgotPassword" runat="server" style="border-style: none; color: white" class="pull-left">
                        <h4>
                            <asp:Literal ID="litPassword" runat="server" Text="<%$Resources:ForgottenPassword %>" /></h4>
                    </a>
                    <a id="lnkForgotUsername" runat="server" style="border-style: none; color: white" class="pull-right ">
                        <h4>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:ForgottenUsername %>" /></h4>
                    </a>
                </div>


            </div>
            <div style="display: none">
                <asp:Login runat="server" ID="login1" TitleText="" CssClass="form"
                    UsernameRequiredErrorErrorMessage="<%$Resources:PageGlobalResources,UsernameRequiredError %>"
                    DisplayRememberMe="false" FailureText="" PasswordLabelText="<%$Resources:PageGlobalResources,PasswordLabel %>"
                    LoginButtonType="Button" LoginButtonText="<%$Resources:PageGlobalResources,LoginText %>"
                    DestinationPageUrl="Admin/AccountSearch.aspx" LoginButtonStyle-CssClass="btn-login btn-warning-login" TextBoxStyle-CssClass="form-control-login login-text">
                    <LabelStyle HorizontalAlign="right" CssClass="label" />
                </asp:Login>
            </div>
        </div>
    </div>
</asp:Content>
