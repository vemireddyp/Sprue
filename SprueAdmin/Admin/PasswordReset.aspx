<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/ForgotDetails/SiteNormalContent.master" Inherits="SprueAdmin.PasswordReset" CodeBehind="PasswordReset.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/Dashboard.ascx" TagName="Dashboard" TagPrefix="uc1" %>
<%@ Register Src="~/Admin/Passwords.ascx" TagName="Passwords" TagPrefix="ucp" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" class="container bgWhite pad15 borderBlack">
        <ContentTemplate>
            <div class="row borderHeader">
                <div class="col-md-12 clMgeSensor bgTheme">
                    <asp:Label ID="lblCreatePassword" runat="server" Font-Bold="true" CssClass="txtcolor"></asp:Label>
                </div>
    <div id="divLinkExpired" visible="true" runat="server" class="col-md-8 col-md-offset-2 rwPadLR0 form-group margin-top25">
     <asp:Label ID="lblInstructions" Font-Bold="true" runat="server"></asp:Label>
    </div>   
   </div>
 
   <ucp:Passwords ID="ucPasswords" runat="server" />
   
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
