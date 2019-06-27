<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" CodeBehind="SwapPanel.aspx.vb" Inherits="SprueAdmin.SwapPanel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" CssClass="swapBody" runat="server">

    <script type="text/javascript">


        $(document).ready(function () {

            $('.form-control-mac').keyup(function (e) {

                var keyCode = e.keyCode || e.which;
                if (keyCode == 9 || keyCode == 16) {
                    //shift + key so avoid auto jumping focus
                }
                else {
                    if (ValidateMACDigit(this) && this.value.length == $(this).attr('maxlength')) {
                        $('#' + $(this).prop("TabNext")).focus().select();
                    }
                }
            });

            $('.form-control-mac').click(function () {
                $(this).focus().select();
            });
        });

        function ShowSwapGateway() {
            //display swap gateway confirm
            $("[id$='ModalSwapGateway']").modal('show');
        }

        function ValidateMACEntry(sender, args) {
            //check each mac text box

            var mac_inputs = $('input.form-control-mac');

            var error_triggered = '';
            var allFilled = 0;
            var re = new RegExp("[^0-9ABCDEFabcdef\]");
            $.each(mac_inputs, function () {
                var id = $(this).attr('id');
                $(this).val($(this).val().toUpperCase());
                var val = $(this).val();
                if (val != '') {
                    allFilled += 1;
                }
            });

            $.each(mac_inputs, function () {
                var id = $(this).attr('id');
                $(this).val($(this).val().toUpperCase());
                var val = $(this).val();
                var text = $(this).text;
                if (re.exec(val)) {
                    error_triggered = 1;
                    $('input#' + id).addClass('form-control-mac-error');
                }
                else {
                    $('input#' + id).removeClass('form-control-mac-error');
                }
            });

            var status = document.getElementById(sender.MACStatus);

            if (error_triggered == 1 || allFilled < 12) {
                //invalid MAC address
                args.IsValid = false;
            }
            else {
                //valid MAC address on client side
                args.IsValid = true;
            }

            return args.IsValid;
        }

        function ValidateMACDigit(inpCtl) {
            var re = new RegExp("[^0-9ABCDEFabcdef\]");

            if (re.exec($(inpCtl).val())) {
                return false;
            }
            else {
                return true;
            }

        }

    </script>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" class="col-md-12 rwPadLR0 borderBlack">
        <ContentTemplate>
            <div class="row col-md-12 rwPT12">
                <asp:Label ID="lblAccountID" runat="server" Text="<%$Resources:PageGlobalResources,AccountIDLabel %>" CssClass="col-md-3 text-right text-label"></asp:Label>
                <asp:Label ID="AccountID" runat="server"></asp:Label>
            </div>
            <div class="row col-md-12 rwPT12">
                <asp:Label ID="lblOldMACAddress" runat="server" Text="<%$Resources:PageGlobalResources,OldMacAddressLabel %>" CssClass="col-md-3 text-right text-label"></asp:Label>
                <asp:Label runat="server" ID="OldMACAddress"></asp:Label>
            </div>
            <div class="row col-md-12 rwPT12">
                <asp:Label ID="lblNewMACAddress" runat="server" Text="<%$Resources:PageGlobalResources,NewMacAddressLabel %>" CssClass="col-md-3 text-right control-label"></asp:Label>
                <div class="col-md-8 rwPadLR0">

                    <div class="row col-md-12 rwPadL0">
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC1" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac1")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC2" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac2")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC3" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac3")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC4" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac4")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC5" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac5")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC6" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac6")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC7" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac7")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC8" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac8")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC9" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac9")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC10" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac10")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC11" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac11")%>' />
                        </div>
                        <div class="col-md-1 form-control-mac-holder">
                            <asp:TextBox ID="txtHubMAC12" runat="server" MaxLength="1" CssClass="form-control form-control-mac mandatory" Text='<%# Eval("mac12")%>' />
                        </div>

                        <asp:CustomValidator ID="valHubMAC1" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC1" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC2" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC2" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC3" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC3" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC4" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC4" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC5" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC5" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC6" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC6" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC7" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC7" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC8" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC8" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC9" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC9" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC10" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC10" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC11" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC11" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                        <asp:CustomValidator ID="valHubMAC12" ClientValidationFunction="ValidateMACEntry" ControlToValidate="txtHubMAC12" ValidationGroup="HubMAC" Display="None" runat="server" ValidateEmptyText="true" EnableClientScript="true" />
                    </div>

                    <div class="col-md-8 rwPadLR0">
                        <asp:CustomValidator ID="valMAC" runat="server" 
                            OnServerValidate="ValidateMAC" ValidationGroup="vgSwap" EnableClientScript="true" 
                            CssClass="RedNoticeText" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>

            </div>
           
            <div class="row col-md-12 rwPT12">
                <asp:Button ID="btnSwapPanel" runat="server" CssClass="btn btn-primary btn-warning btnLogOff modeAll col-md-offset-3 validateAllNoMsg" Text="<%$Resources:SwapGateway %>" />
            </div>

            <div class="row col-md-12 rwPT12">
                <label class="col-md-3"></label>
                <asp:Label ID="lblResult" runat="server" CssClass="col-md-offset-0 mac-result" Text="<%$Resources:SwappedSuccess %>"></asp:Label><br />
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="ModalSwapGateway" runat="server" role="dialog" style="display: none;">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog vertical-align-center">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-8 col-md-offset-2 text-center">
                                <div class="form-group">
                                    <label class="lblfont16">
                                        <asp:Literal ID="litSwapGWWarning" runat="server" Text="<%$Resources:SwapGatewayWarning%>" />
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-8 col-md-offset-2 text-center">
                                <asp:Literal ID="litSwapGWPrompt" runat="server" Text="<%$Resources:SwapGatewayPrompt%>" />
                            </div>

                            <div class="col-md-8 col-md-offset-2 text-center margin-top25">
                                <asp:Button ID="btnSwapGWCancel" runat="server" CssClass="btn btn-warning btnLogOff pull-left" Text='<%$Resources:PageGlobalResources,CancelButton%>' OnClientClick="CloseModal('ModalSwapGateway');" />
                                <asp:Button ID="btnSwapGWOK" runat="server" CssClass="btn btn-warning btnLogOff pull-right" Text='<%$Resources:PageGlobalResources,OKText%>' />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
