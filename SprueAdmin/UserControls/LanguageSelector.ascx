<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LanguageSelector.ascx.vb" Inherits="SprueAdmin.LanguageSelector" %>

<script type="text/javascript">

    $(document).ready(function () {
          <%--Language Selection--%>
        $(".divSelectedFlag").on("click", function () {
            $(this).addClass('hide');
            $(".divChooseFlag").removeClass('hide');
        });

        $('.divChooseFlag').on('mouseleave', function () {
            $('.divSelectedFlag').removeClass('hide');
            $(this).addClass('hide');
        });
    });

</script>

<div style="width: 100%; height: 100%">
    <div id="divSelectedFlag" class="pull-right divSelectedFlag" runat="server" style="font-size: 16px; color: white; cursor: pointer">
        <asp:Image ID="imgFlagSelected" Height="15" Width="20" runat="server" />
        <asp:Literal ID="litLanguage" runat="server" />
    </div>
    <div class="col-md-4 pull-right hide divChooseFlag" id="divChooseFlag" runat="server">
        <div class="col-md-12" style="border: 1px solid #00A3E2; text-align: left; font-size: 14px; background-color: white; z-index: 100; width: 180px; position: absolute;">
            <div id="selectFlagInfo" style="padding: 2px; color: #00A3E2; font-size: 16px; border-bottom: 1px solid #00A3E2;">
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:PageGlobalResources,SelectYourCountry %>" />
            </div>
            <asp:Repeater ID="rptChooseFlag" runat="server">
                <ItemTemplate>
                    <div style="padding: 5px;">
                        <asp:Image ID="imgFlag" ImageUrl='<%# Eval("Flag")%>' Height="15" Width="20" runat="server" />
                        <asp:Button ID="btnLanguage" CommandArgument='<%# Eval("Culture")%>' OnClientClick='<%# Eval("OnClick") %>' Style="background-color: white; border: none; color: #606060;" runat="server" Text='<%# Eval("LanguageText")%>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
