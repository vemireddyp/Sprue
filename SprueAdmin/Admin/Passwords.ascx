<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Passwords.ascx.vb" Inherits="SprueAdmin.Admin_Passwords" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script type="text/javascript">

    function ValidateAllValidationGroups() {
        if (Page_ClientValidate()) {
            return true;
        }
    }

    function ValidatePasswordClientReset(sender, args) {

        var txtPass = $('#<%= txtPassword.ClientID%>');
        var valPass = $('#<%= revNewPassword.ClientID%>');

        args.IsValid = valPass[0].isvalid;

        if (txtPass.val().length > 0) {
            if (txtPass.val().replace(/[^:]/gi, "").length > 0) {
                args.IsValid = false;
            }

            if (!args.IsValid) {
                txtPass.parent('div').addClass('error');
            }
            else {
                txtPass.parent('div').removeClass('error');
            }
        }
        else {
            txtPass.parent('div').addClass('error');
        }

    }

    function ValidatePasswordClientConfirm(sender, args) {

        var txtPass = $('#<%= txtPassword.ClientID%>');
        var txtConfirm = $('#<%= txtPasswordConfirm.ClientID%>');
        args.IsValid = (txtPass.val() == txtConfirm.val());
        if (!args.IsValid) {
            txtConfirm.parent('div').addClass('error');
        }
        else {
            txtConfirm.parent('div').removeClass('error');
        }
    }

</script>


<asp:UpdatePanel ID="pnlContent" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <!-- Old Password field and its validator controls -->
        <div id="divOldPass" runat="server" class="row col-md-12">
            <asp:Label Text="<%$Resources:PageGlobalResources,CurrentPassword %>" runat="server" CssClass="col-md-3 text-right control-label" />
            <div class="col-md-3">
                <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password" CssClass="form-control" CausesValidation="true" TabIndex="1" />
            </div>

            <div class="col-md-5 col-md-offset-1" style="padding-left:1em;">
                <asp:RequiredFieldValidator ID="rfvOldPass" ControlToValidate="txtPassword"
                    CssClass="RedNoticeText" Display="Dynamic" SetFocusOnError="True"
                    ErrorMessage="<%$Resources:PageGlobalResources,EnterPassword %>" runat="server" />

                <asp:CustomValidator ID="rcvOldPass" ControlToValidate="txtOldPass" Display="Dynamic" runat="server"
                    ErrorMessage="<%$Resources:PageGlobalResources,CurrentPasswordInvalid %>" CssClass="RedNoticeText" 
                    ValidateEmptyText="true" SetFocusOnError="True" OnServerValidate="ValidateOldPassword" />
            </div>

        </div>
        <!-- Password field and its validator controls -->
        <div class="row col-md-12">
            <asp:Label Text="<%$Resources:PageGlobalResources,NewPassword %>" runat="server" CssClass="col-md-3 text-right control-label" />
            <div class="col-md-3">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" CausesValidation="true" TabIndex="2" />
            </div>

            <div class="col-md-1">
                <ajaxToolkit:PasswordStrength ID="PasswordStrength" runat="server" TargetControlID="txtPassword"
                    StrengthIndicatorType="Text"
                    MinimumLowerCaseCharacters="1"
                    MinimumNumericCharacters="1"
                    MinimumSymbolCharacters="1"
                    MinimumUpperCaseCharacters="1"
                    DisplayPosition="RightSide"
                    PreferredPasswordLength="10"
                    PrefixText=""
                    RequiresUpperAndLowerCaseCharacters="true"
                    TextStrengthDescriptionStyles="cssPwdRed;cssPwdOrange;cssPwdBlack;cssPwdLime;cssPwdGreen" />


            </div>
            <div class="col-md-5" style="padding-left:1em;">
                <asp:RegularExpressionValidator ID="revNewPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic" SetFocusOnError="True"
                    ValidationExpression="^(?=(.*\d){1})(?=.*[a-zA-Z]{4}).{8,20}$" CssClass="RedNoticeText" ></asp:RegularExpressionValidator>

                <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword"
                    CssClass="RedNoticeText" Display="Dynamic" SetFocusOnError="True"
                    ErrorMessage="<%$Resources:PageGlobalResources,ProvideNewPassword %>" runat="server" />

                <asp:CustomValidator ID="valPassword" ClientValidationFunction="ValidatePasswordClientReset" ControlToValidate="txtPassword" Display="Dynamic" runat="server"
                    ErrorMessage="<%$Resources:PageGlobalResources,PasswordNotAccepted %>" CssClass="RedNoticeText" 
                    ValidateEmptyText="true" EnableClientScript="true" SetFocusOnError="True" OnServerValidate="ValidatePassword" />
            </div>

        </div>
        <!-- Password Confirmation field and its validator controls -->
        <div class="row col-md-12 rwPadTB9">
            <asp:Label Text="<%$Resources:PageGlobalResources,ReenterNewPassword %>" runat="server" CssClass="col-md-3 text-right control-label" />
            <div class="col-md-3">

                <!-- Email Address Confirmation field -->
                <asp:TextBox ID="txtPasswordConfirm" TextMode="Password" runat="server" CssClass="form-control" CausesValidation="true" TabIndex="3" />

            </div>

            <div class="col-md-5 col-md-offset-1" style="padding-left:1em;">
                <asp:RequiredFieldValidator ID="rfvPasswordConfirm" ControlToValidate="txtPasswordConfirm"
                    CssClass="RedNoticeText" Display="Dynamic"
                    ErrorMessage="<%$Resources:PageGlobalResources,ConfirmNewPassword %>" runat="server" />

                <asp:CustomValidator ID="cvPasswordConfirm" ControlToValidate="txtPasswordConfirm"
                    CssClass="RedNoticeText" Display="Dynamic" ClientValidationFunction="ValidatePasswordClientConfirm"
                    OnServerValidate="ValidatePasswordConfirm" ErrorMessage="<%$Resources:PageGlobalResources,PasswordsNotMatchError %>" runat="server" />

            </div>
        </div>
        <div class="row col-md-12 rwPadTB9">
            <div class="col-md-2 col-md-offset-3">
                <asp:Button ID="btnSaveChanges" runat="server" CssClass="btn btn-default btnEdit btnW83" Text="<%$Resources:PageGlobalResources,ChangePassword %>"
                    CausesValidation="true" TabIndex="4" />
            </div>

        </div>
        <div class="row col-md-12">
            <asp:Label ID="lblNoLogin" runat="server" Visible="false" EnableViewState="false" CssClass="col-md-10 col-md-offset-1" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
