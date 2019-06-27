<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" CodeBehind="TermAndConditions.aspx.vb" Inherits="SprueAdmin.TermAndConditions" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/LanguageSelector.ascx" TagName="LanguageSelector" TagPrefix="uc1" %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="contentHead" runat="server">
    <style type="text/css">
        .AcceptedAgreement label {
            display: inline;
            margin-left: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentBody" runat="server">
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">

            var agreePanel = null;

            Sys.Application.add_load(function () {
                $("[id$='btnContinue']").prop("disabled", true);
                $(".AcceptedAgreement").each(function () {
                    $(this).find("input:checkbox").prop("disabled", true)
                            .data("filekey", $(this).data("filekey"))
                            .data("datakey", $(this).data("datakey"))
                });

                $("form").on("submit", function (e) {
                    if (!allAgreementsChecked) {
                        e.preventDefault();
                    }
                });

                agreePanel = $find("<%=xhpAgreeChecked.ClientID%>")

                $(".AcceptedAgreement input").click(function () {
                    if (agreePanel !== null && typeof agreePanel.set_value != "undefined") {
                        var agreeCompleted = true
                        $("[data-filekey='" + $(this).data("filekey") + "'] input").each(function () {
                            if (agreeCompleted && !($(this).prop("checked") == true)) {
                                agreeCompleted = false;
                            }
                        });

                        var postValue = $(this).data("datakey") + "," + agreeCompleted;

                        agreePanel.set_value(postValue);
                    }
                    var allAgreed = allAgreementsChecked()
                    var btn = $("[id$='btnContinue']")
                    btn.prop("disabled", !allAgreed);
                    if (allAgreed) {
                        btn.show()
                    }
                    else {
                        btn.hide()
                    }


                });

                $('.docViewDiv').scroll(function () {
                    var targData = $(this).data("filekey");
                    var enableCheck = false;

                    if (Math.ceil($(this).scrollTop()) + Math.ceil($(this).innerHeight()) >= $(this)[0].scrollHeight) {
                        enableCheck = true
                    }

                    $("[data-filekey='" + targData + "'] input").each(function () {
                        if ($(this).prop("disabled")) {
                            $(this).prop("disabled", !enableCheck);
                        }
                    });
                });

            });

            var currDisplay = null;

            function allAgreementsChecked() {
                var allAccepted = true;

                $(".AcceptedAgreement input").each(function () {
                    if (allAccepted && !($(this).prop("checked") == true)) {
                        allAccepted = false;
                    }
                });

                if (!allAccepted) {
                    currDisplay = null;

                    $(".docElem").each(function () {
                        if (currDisplay == null) {
                            currDisplay = $(this);
                            currDisplay.find(".AcceptedAgreement input:checkbox").each(function () {
                                if (currDisplay != null && !$(this).prop("checked")) {
                                    currDisplay.show();
                                    currDisplay = null;
                                }

                            })
                            if (currDisplay == null) {
                                currDisplay = $(this);
                            } else {
                                currDisplay.hide();
                                currDisplay = null;
                            }
                        }
                        else {
                            $(this).hide();
                        }
                    })
                }

                return allAccepted;
            }
        </script>
    </telerik:RadCodeBlock>
    <div class="container-fluid masHeaderbg rwPadLR0" runat="server" id="divPageHeader">
        <div class="container rwHeader">
            <div class="col-md-12 rwPadLR0">
                <div class="col-md-5 rwPadLR0">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/common/img/header-logo.png" />
                </div>
            </div>
        </div>
        <div class="bgorange" style="margin-top: 0px;">
        </div>
        <telerik:RadXmlHttpPanel runat="server" ID="xhpAgreeChecked"></telerik:RadXmlHttpPanel>
    </div>

    <div class="container">
        <div id="acceptDiv" runat="server">
            <telerik:RadListView runat="server" ID="docShowView">
                <ItemTemplate>
                    <div id="containerDiv" class="docElem" runat="server">
                        <div id="labelDiv" runat="server" class="row borderBlack docViewDiv" style="margin-left: 5px; overflow: auto; margin-right: 0px; height: 500px; background-color: white">
                            <div class="col-md-12" style="margin-left: 15px; overflow: auto; width: auto">
                                <asp:Label ID="lblDocAccept" runat="server" />
                            </div>
                        </div>

                        <telerik:RadListView runat="server" ID="docAcceptView">
                            <ItemTemplate>
                                <div class="row" style="margin: 20px;">
                                    <div class="col-md-12 rwPadLR0">
                                        <asp:CheckBox runat="server" ID="chkDocAccept" CssClass="AcceptedAgreement" TextAlign="right" ToolTip="<%$Resources:AgreePrompt %>" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </telerik:RadListView>
                    </div>
                </ItemTemplate>
            </telerik:RadListView>

            <telerik:RadListView runat="server" ID="docAgreementsView">
                <ItemTemplate>
                    <telerik:RadListView runat="server" ID="docAcceptView">
                        <ItemTemplate>
                            <div class="row" style="margin: 20px;">
                                <div class="col-md-12 rwPadLR0" style="display: inline">
                                    <asp:CheckBox runat="server" ID="chkDocAccept" CssClass="AcceptedAgreement" ToolTip="<%$Resources:AgreePrompt %>" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </telerik:RadListView>
                </ItemTemplate>
            </telerik:RadListView>


            <div class="row pull-right" style="margin-right: 0px; margin-top: 20px;">
                <asp:Button ID="btnContinue" CssClass="btn btn-warning btnLogOff btnW83 pull-right btnContinue" ValidationGroup="EndUserTCs" CausesValidation="true" runat="server" OnClientClick="return allAgreementsChecked();" />
            </div>
        </div>

    </div>


    <div id="confDiv" runat="server" visible="false">
        <div class="borderBlack">
            <div class="col-mdd-12 text-center" style="background-color: #e16b05; height: 150px;">
                <div class="clMgeSensor">
                    <asp:Label ID="lblSuccessLabel" runat="server" CssClass="txtcolor" Text="<%$Resources:SuccessLabel %>"></asp:Label>
                </div>
                <div class="clMgeSensor" id="emailDiv" runat="server">
                    <asp:Label ID="lblEmailLabel" runat="server" CssClass="txtcolor" Text="<%$Resources:EmailLabel %>"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <div id="badRequestDiv" runat="server" visible="false">
        <div class="borderBlack">
            <div class="col-mdd-12 text-center" style="background-color: #e16b05; height: 150px;">
                <div class="clMgeSensor">
                    <asp:Label ID="lblBadinviteLabel" runat="server" CssClass="txtcolor" Text="<%$Resources:BadInviteLabel %>"></asp:Label>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
