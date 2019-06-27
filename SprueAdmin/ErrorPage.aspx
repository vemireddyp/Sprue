<%@ Page Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" Inherits="SprueAdmin.ErrorPage" Title="System Error" CodeBehind="ErrorPage.aspx.vb" %>

<%@ Import Namespace="Microsoft.VisualStudio.Web.PageInspector.Runtime" %>

<asp:Content ID="Content1" ContentPlaceHolderID="siteBodyContent" runat="Server">
    <div class="container-fluid masHeaderbg rwPadLR0" style="height: 100px;">
        <div class="text-center margin-top25">
            <asp:Image ID="Image1" CssClass="margin-top25" runat="server" ImageUrl="~/common/img/header-logo.png" />
        </div>
    </div>
    <div class="bgorange">
    </div>
    <div class="container rwMarTB10 bgwhite borderBlack">
        <h1 class="col-md-12 text-center">
            <asp:Literal ID="litErrorPageTitle" runat="server" Text="<%$Resources:PageGlobalResources,ErrorHasOccurred %>"></asp:Literal></h1>
        <div class="col-md-12 text-center rwPadTB15">
            <asp:Label ID="lblErrorMsg" runat="server" Text="<%$Resources:PageGlobalResources,PageNotDisplayed %>"></asp:Label>
            <span>
                <asp:Literal runat="server" Text="<%$Resources:PageGlobalResources,Email %>" />: </span>
            <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="<%$AppSettings:SupportRequestEmailDestination %>"></asp:Label>
        </div>
    </div>

</asp:Content>
