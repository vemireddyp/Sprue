<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" CodeBehind="Contacts.aspx.vb" Inherits="SprueAdmin.Contacts" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <script>

        $(document).ready(function () {
            //enable the button if change any thing in contacts section
            $("#divAccountContacts").on("change", function () {
                $('#btnSave').removeAttr("disabled");
            });
 
        });

        function ActivationCodeSentAlert() {
            alert(document.getElementById('<%= hdnActivationCodeSentAlert.ClientID%>').value);
        };

        function TermsAndConditionsSentAlert() {
            alert(document.getElementById('<%= hdnTermsAndConditionsSentAlert.ClientID%>').value);
          };



    </script>

    <div id="divContactsPage" class="col-md-12 margin-top15 bgwhite borderBlack">
        <div class="row rwPadTB9 bgTheme txtcolor">
            <div class="col-md-5">
                <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="<%$Resources:ContactsPage %>" />
            </div>
        </div>

        <div id="divAccountContacts" class="col-md-12 rwPadTB9">
            <div class="col-md-12 form-group rwPadLR0">
                <asp:Repeater ID="rptContacts" runat="server">
                    <HeaderTemplate>
                        <div id="divHeader" class="register-label">
                            <%--  <div class="col-md-1">
                                <asp:Label ID="Label1" Font-Bold="true" runat="server"></asp:Label>
                            </div>--%>
                            <div class="col-md-12">
                                <div class="col-md-2">
                                    <asp:Label ID="lblHeaderContact" Font-Bold="true" runat="server" Text="<%$Resources:Contact %>"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblHeaderOrganisation" Font-Bold="true" runat="server" Text="<%$Resources:Organisation %>"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblHeaderEmail" Font-Bold="true" runat="server" Text="<%$Resources:Email %>"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblHeaderContactNumber" Font-Bold="true" runat="server" Text="<%$Resources:Telephonenumber %>"></asp:Label>
                                </div>
                                <div class="col-md-2 rwPadLR0">
                                    <div class="col-md-12">
                                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="<%$Resources:NotificationOptions %>"></asp:Label>
                                    </div>
                                    <div class="col-md-12 rwPadLR0">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblHeaderFault" Font-Bold="true" runat="server" Text="<%$Resources:Fault %>"></asp:Label>
                                        </div>
                                        <div class="col-md-4 colPadL5">
                                            <asp:Label ID="lblHeaderAlert" Font-Bold="true" runat="server" Text="<%$Resources:Alert %>"></asp:Label>
                                        </div>
                                        <div id="divCommunityHeader" class="col-md-4 padL0" runat="server">
                                            <asp:Label ID="lblHeaderCommunity" Font-Bold="true" runat="server" Text="<%$Resources:Community %>"></asp:Label>
                                        </div>
                                        <%--<div class="col-md-4 padL0">
                                            <asp:Label ID="lblHeaderAppNotifications" Font-Bold="true" runat="server" Text="<%$Resources:Apps %>"></asp:Label>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col-md-12 rwMargT10 padL0">
                            <div class="col-md-2 rwPadLR0">
                                <div class="col-md-1 padL0">
                                    <asp:Label ID="lblContactNumber" Font-Bold="true" runat="server" Text='<%# Eval("ContactNumber")%>'> ></asp:Label>
                                    <asp:HiddenField ID="hdnContactSeq" runat="server" Value='<%# Eval("ContactSeq")%>'/>
                                </div>
                                <div class="col-md-11 rwPadLR0">
                                    <asp:TextBox ID="txtContactName" CssClass="form-control height28"  runat="server" MaxLength="50" Text='<%# Eval("FirstName")%>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="ValidateContactGroup" ID="valContactName" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:ContactNameRequiredError %>" CssClass="RedNoticeText" ControlToValidate="txtContactName" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtOrganisation" runat="server" CssClass="form-control height28" MaxLength="50" Text='<%# Eval("Organisation")%>'></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ValidationGroup="ValidateContactGroup" ID="valOrganisation" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:OrganisationRequiredError %>" CssClass="RedNoticeText" ControlToValidate="txtOrganisation" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-md-2 rwPadLR0">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control height28"  Text='<%# Eval("Email")%>' Style="width: 180px;"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="ValidateContactGroup" ID="valEmail" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:EmailRequiredError %>" CssClass="RedNoticeText" ControlToValidate="txtEmail" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ValidationGroup="ValidateContactGroup" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ID="valEmailFormat" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:InvalidEmailFormat %>" CssClass="RedNoticeText" ControlToValidate="txtEmail" Display="Dynamic" />
                                <%--<asp:Label ID="lblEmailFormatInvalid" runat="server" Text="<%$Resources:InvalidEmailFormat %>" Visible="false" CssClass="RedNoticeText" ForeColor="Red"></asp:Label>--%>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtContactNumber" CssClass="form-control height28" runat="server" Text='<%# Eval("MobileNumber")%>' MaxLength="15"></asp:TextBox>
                                <asp:RegularExpressionValidator ValidationGroup="ValidateContactGroup" ValidationExpression="^(|[+0-9 ]+)$" ID="valContactNumberFormat" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:InvalidContactNumberFormat %>" CssClass="RedNoticeText" ControlToValidate="txtContactNumber" Display="Dynamic" />
                                <%--<asp:RequiredFieldValidator ValidationGroup="ValidateContactGroup" ID="valContactNumber" ForeColor="Red" runat="server" ErrorMessage="<%$Resources:ContactNumberRequiredError %>" CssClass="RedNoticeText" ControlToValidate="txtContactNumber" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                <%--<asp:Label ID="lblContactNumberInvalid" runat="server" Text="<%$Resources:InvalidContactNumberFormat %>" Visible="false" CssClass="RedNoticeText" ForeColor="Red"></asp:Label>--%>
                            </div>
                            <div class="col-md-2">
                                <div class="col-md-4 text-center">
                                    <asp:CheckBox ID="chkFault" runat="server" Checked='<%# Eval("Fault")%>' />
                                </div>
                                <div class="col-md-4 text-center">
                                    <asp:CheckBox ID="chkAlert" runat="server" Checked='<%# Eval("Alert")%>' />
                                </div>
                                <div id="divCommunityCheck" class="col-md-4 text-center" runat="server">
                                    <asp:CheckBox ID="chkCommunity" runat="server" Checked='<%# Eval("Community")%>' />
                                </div>
                                <%--<div class="col-md-4 text-center">
                                    <asp:CheckBox ID="chkApps" runat="server"  Checked='<%# Eval("ReceivesPush")%>' />
                                </div>--%>
                            </div>
                            <div class="col-md-2 rwPadLR0" style="left: 30px; top: -10px;" id="divSendDel" runat="server">
                                <asp:Button ID="btnSend" runat="server" CssClass="btn btn-warning btnFault btnW83" Style="margin-right: 10px;" Text="<%$Resources:Send %>" OnClick="Send_Click" CommandArgument='<%# Eval("PersonID")%>'  />
                                <asp:Button ID="btnDel" runat="server" CssClass="btn btn-warning btnFault btnW83" Text="<%$Resources:Delete %>" OnClick="Delete_Click" CommandArgument='<%# Eval("PersonID")%>' />
                                <asp:HiddenField ID="hdnMode" runat="server" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="col-md-12 rwPadTB9" style="right: -27px;">
            <asp:Button ID="btnSave" runat="server" disabled="disabled" ClientIDMode="Static"  CssClass="btn btn-warning btnLogOff pull-right" Text="<%$Resources:Save %>" />
        </div>
        <div class="form-group" runat="server" id="div1">
            <asp:Label ID="lblRequiredFieldIndicator" Text="<%$Resources:PageGlobalResources,RequiredFieldIndicator %>" runat="server"></asp:Label>
        </div>
        <div class="form-group" runat="server" id="divSaveFailed" visible="false">
            <asp:Label ID="lblFailedToSave" Text="<%$Resources:FailedToSave %>" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <div class="form-group" runat="server" id="divSaveSuccessful" visible="false">
            <asp:Label ID="lblSavedSuccessfully" Text="<%$Resources:ConfirmSaved %>" runat="server" ForeColor="Black"></asp:Label>
        </div>
        <div class="form-group" runat="server" id="divDeleted" visible="false">
            <asp:Label ID="lblDeleted" Text="<%$Resources:ConfirmDeleted %>" runat="server" ForeColor="Black"></asp:Label>
        </div>
        <div class="form-group" runat="server" id="divDeleteFailed" visible="false">
            <asp:Label ID="lblDeleteFailed" Text="<%$Resources:ConfirmDeleteFailed %>" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-md-12 rwPadTB9 rwPadLR0">
        <asp:Button ID="btnBack" runat="server" PostBackUrl="~/Admin/AccountDetail.aspx" class="btn btn-warning btnLogOff pull-left" Text="<%$Resources:Back %>" />
        <asp:Button ID="btnAddContact" runat="server" class="btn btn-warning btnLogOff pull-right" Text="<%$Resources:AddContact %>" />

        <asp:HiddenField ID="hdnConfirmSendMessage" runat="server" />
        <asp:HiddenField ID="hdnActivationCodeSentAlert" runat="server" />
        <asp:HiddenField ID="hdnConfirmSaveMessage" runat="server" />
        <asp:HiddenField ID="hdnConfirmSavedMessage" runat="server" />
        <asp:HiddenField ID="hdnTermsAndConditionsSentAlert" runat="server" />

    </div>
</asp:Content>
