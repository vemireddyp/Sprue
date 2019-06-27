<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master"  Inherits="SprueAdmin.Admin_LoginDetail" CodeBehind="LoginDetail.aspx.vb" %>

<asp:Content ContentPlaceHolderID="contentBody" runat="server">
    <div class="col-md-12">
        <div class="col-md-12">
            <div runat="server" id="divLinkExpired" visible="false" style="margin: 20px 0 0 10px;">
                <asp:Literal runat="server" Text="<%$Resources:PageGlobalResources,UserLinkExpiredError %>" />
            </div>
        </div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divContent" class="row borderBlack bgwhite" runat="server">
                    <div class="col-md-10 rwMarTB15">
                        <div class="col-md-7">
                            <div class="col-md-12 form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblMasterUser" Text="<%$Resources:PageGlobalResources,UsernameLabel %>" Font-Bold="false" runat="server" AssociatedControlID="txtMasterUser"></asp:Label>
                                </div>
                                <div class="col-md-8 validators">
                                    <asp:TextBox ID="txtMasterUser" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMasterUser" runat="server" ControlToValidate="txtMasterUser" ForeColor="Red"
                                        Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,UsernameRequiredError %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revMasterUser" runat="server" ControlToValidate="txtMasterUser" ForeColor="Red"
                                        Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,UsernameFormatError %>" SetFocusOnError="True"
                                        ValidationExpression="^[a-zA-Z0-9]{6,20}$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="col-md-12 form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblFirstName" Text="<%$Resources:PageGlobalResources,FirstNameLabel %>" Font-Bold="false" runat="server" AssociatedControlID="txtFirstName"></asp:Label>
                                </div>
                                <div class="col-md-8 validators">
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ForeColor="Red"
                                        Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,FirstNameRequiredMsg %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-12 form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblLastName" Text="<%$Resources:PageGlobalResources,LastNameLabel %>" Font-Bold="false" runat="server" AssociatedControlID="txtLastName"></asp:Label>
                                </div>
                                <div class="col-md-8 validators">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ForeColor="Red"
                                        Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,LastNameRequiredMsg %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-12 form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblEmail" Text="<%$Resources:PageGlobalResources,EmailLabel %>" runat="server" Font-Bold="false" AssociatedControlID="txtEmail"></asp:Label>
                                </div>
                                <div class="col-md-8 validators">
                                    <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" MaxLength="255"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ForeColor="Red"
                                        Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,EmailRequiredMsg %>"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator
                                        ID="revEmail" runat="server" ControlToValidate="txtEmail" ForeColor="Red"
                                        Display="Dynamic" ErrorMessage="<%$Resources:ValidationCtlResources,EmailFormatMsg %>" SetFocusOnError="True"
                                        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div id="divOwnInfo" runat="server">
                                <div class="col-md-12 form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblPassword" Text="<%$Resources:PageGlobalResources,PasswordLabel %>" Font-Bold="false" runat="server" AssociatedControlID="txtPassword"></asp:Label>
                                    </div>
                                    <div class="col-md-8 validators">
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="valPasswordRequired" runat="server" ControlToValidate="txtPassword" ForeColor="Red"
                                            Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,PasswordError %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtPassword" ForeColor="Red"
                                            Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,PasswordFormatError %>" SetFocusOnError="True"
                                            ValidationExpression="^[a-zA-Z0-9]{6,60}$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-md-12 form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblConfirmPassword" Text="<%$Resources:PageGlobalResources,ConfirmPasswordLabel %>" Font-Bold="false" runat="server" AssociatedControlID="txtPassword"></asp:Label>
                                    </div>
                                    <div class="col-md-8 validators">
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="valConfirmPasswordRequired" runat="server" ControlToValidate="txtConfirmPassword" ForeColor="Red"
                                            Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,ConfirmPasswordFormatError %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvConfirmPassword" runat="server" BorderStyle="None" ControlToCompare="txtPassword"
                                            ControlToValidate="txtConfirmPassword" Display="Dynamic" ForeColor="Red"
                                            ErrorMessage="<%$Resources:PageGlobalResources,PasswordsNotMatchError %>" SetFocusOnError="True"></asp:CompareValidator>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="divStatus" class="col-md-12 form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblStatus" Text="<%$Resources:PageGlobalResources,StatusLabel %>" runat="server" Font-Bold="false" AssociatedControlID="ddlStatus"></asp:Label>
                                </div>
                                <div class="col-md-8 validators">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="1" Text="<%$Resources:PageGlobalResources,StatusActive %>" />
                                        <asp:ListItem Value="2" Text="<%$Resources:PageGlobalResources,StatusDisabled %>" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="col-md-12">
                                <asp:Label ID="lblValidation" runat="server" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 rwMarTB15">
                        <div class="pull-left col-md-offset-1">
                            <asp:Button ID="btnResetPassword" runat="server" cssclass="btn btn-warning btnLogOff pull-right"  Text="<%$Resources:PageGlobalResources,ResetPassword %>"/>
                        </div>
                        <div class="col-md-2 col-md-offset-5 padR0">
                            <asp:Button ID="btnSave" runat="server" cssclass="btn btn-warning btnLogOff pull-right" Text="<%$Resources:PageGlobalResources,SaveButton %>" />
                        </div>
                    </div>
                    <div class="col-md-12 rwMarTB15">
                        <asp:Label ID="lblResetPasswordInfo" runat="server" visible="false" Text="<%$Resources:ResetPasswordInfo %>" />
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
