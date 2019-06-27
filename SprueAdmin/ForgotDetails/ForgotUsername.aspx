<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ForgotDetails/SiteNormalContent.master" CodeBehind="ForgotUsername.aspx.vb" Inherits="SprueAdmin.ForgotUsername" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <div class="row borderHeader borderBlack">
        <div class="col-md-12 clMgeSensor bgTheme">
            <asp:Label ID="lblForgotPasswordHeader" runat="server" Font-Bold="true" CssClass="txtcolor" Text="<%$Resources:ForgotUsernameHeader %>"></asp:Label>
        </div>
        <div class="col-md-10 col-md-offset-2 rwMarTB15">
            <asp:Label ID="lblEmailInfo" runat="server" Font-Bold="true" Text="<%$Resources:EmailInfo %>"></asp:Label>
        </div>
        <div id="divEmailInfo" runat="server">
            <div class="form-group col-md-5 col-md-offset-2">
                <asp:Label ID="lblEmail" runat="server" Text="<%$Resources:EmailAddress %>"></asp:Label>
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="ValidateUsernameGroup" ID="valEmail" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:Emailrequired %>" CssClass="RedNoticeText" ControlToValidate="txtEmail" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ValidationGroup="ValidateUsernameGroup" ID="valexpEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:EmailInvalid %>" CssClass="RedNoticeText" ControlToValidate="txtEmail" SetFocusOnError="True" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>
            <div class="form-group col-md-5 col-md-offset-2">
                <asp:Label ID="lblConfirmEmail" runat="server" Text="<%$Resources:EmailAddressConfirm %>"></asp:Label>
                <asp:TextBox ID="txtConfirmEmail" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="ValidateUsernameGroup" ID="valConfrimEmail" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:ConfirmEmailrequired %>" CssClass="RedNoticeText" ControlToValidate="txtConfirmEmail" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ValidationGroup="ValidateUsernameGroup" ID="valexpConfrimEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:ConfirmEmailInvalid %>" CssClass="RedNoticeText" ControlToValidate="txtConfirmEmail" SetFocusOnError="True" Display="Dynamic"></asp:RegularExpressionValidator>
                <asp:CompareValidator ValidationGroup="ValidateUsernameGroup" ID="compare" runat="server" ControlToCompare="txtEmail" ControlToValidate="txtConfirmEmail" ForeColor="Red" ErrorMessage="<%$Resources:ConfirmEmailMatch %>" CssClass="RedNoticeText" SetFocusOnError="True" Display="Dynamic"></asp:CompareValidator>
            </div>
        </div>
        <div id="divPasswordEmailInfo" runat="server" visible="false" class="col-md-7 col-md-offset-2 rwMarTB15 form-group">
            <asp:Label ID="lblEmailMessage" runat="server"></asp:Label>
        </div>
        <div class="col-md-5  col-md-offset-2 rwMarTB15">
            <asp:Button ID="btnBack" class="btn btn-default btnEdit btnW83 pull-left" runat="server" Text="<%$Resources:NavBack %>" />
            <asp:Button ID="btnSend" class="btn btn-default btnEdit btnW83 pull-right" ValidationGroup="ValidateUsernameGroup" runat="server" Text="<%$Resources:NavSend %>" />
        </div>
    </div>

</asp:Content>
