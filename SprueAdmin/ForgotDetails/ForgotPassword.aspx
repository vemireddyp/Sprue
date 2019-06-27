<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ForgotDetails/SiteNormalContent.master" CodeBehind="ForgotPassword.aspx.vb" Inherits="SprueAdmin.ForgotPassword" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="row borderHeader borderBlack">
        <div class="col-md-12 clMgeSensor bgTheme">
            <asp:Label ID="lblForgotPasswordHeader" runat="server" Font-Bold="true" CssClass="txtcolor" Text="<%$Resources:ForgotPasswordHeader %>"></asp:Label>
        </div>
        <div id="divEnterUsername" runat="server" class="col-md-8 col-md-offset-2 rwPadLR0 form-group margin-top25">
            <asp:Label ID="lblEnterUsernameHeader" Font-Bold="true" runat="server" Text="<%$Resources:EnterUsernameHeader %>"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server" MaxLength="256" placeholder="<%$Resources:UsernameHeader %>"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="ValidatePasswordGroup" ID="valUsername" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:UsernameRequiredError %>" CssClass="RedNoticeText" ControlToValidate="txtUsername" SetFocusOnError="True" Display="Dynamic" Width="408px"></asp:RequiredFieldValidator>
        </div>
        <div id="divForgotUsername" class="margin-top25 col-md-8 col-md-offset-2 rwPadLR0 form-group">
            <div class="col-md-12 rwPadLR0">
                <asp:Label ID="lblForgotUsername" runat="server" Text="<%$Resources:ForgotUsernameHeader %>"></asp:Label>
                <asp:Label ID="lblClickHere" CssClass="colorBlue cursorPointer" runat="server" Text="<%$Resources:ClickHereLabel %>" onclick="RedirecttoUsername();"></asp:Label>
            </div>
        </div>
        <div id="divPasswordEmailInfo" runat="server" visible="false" class="col-md-8 col-md-offset-2 rwPadLR0 form-group margin-top25">
            <asp:Label ID="lblEmailMessage" runat="server"></asp:Label>
        </div>
        <div class="col-md-8 col-md-offset-2 rwPadLR0 rwMarTB15">
            <asp:Button ID="btnBack" class="btn btn-default btnEdit btnW83 pull-left" runat="server" Text="<%$Resources:NavBack %>" />
            <asp:Button ID="btnSend" class="btn btn-default btnEdit btnW83 pull-right" runat="server" ValidationGroup="ValidatePasswordGroup"  Text="<%$Resources:NavSend %>" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function RedirecttoUsername() {
            window.location.href = "ForgotUsername.aspx?callbackUrl=<%=Session("PasswordUpdatedRedirect")%>";
        }

    </script>
</asp:Content>
