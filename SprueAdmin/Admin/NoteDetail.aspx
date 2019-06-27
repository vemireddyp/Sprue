<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_NoteDetail" CodeBehind="NoteDetail.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" runat="server" ChildrenAsTriggers="true" ContentPlaceHolderID="ContentBody">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-md-12 rwPadTB9 rwPadR7 bgTheme" style="margin-top: 10px; color: white; font-weight: bold;">
                <div class="col-md-12">
                    <asp:Label ID="lblAccounts" runat="server" Text="<%$Resources:NoteDetails %>"></asp:Label>
                </div>
            </div>
            <div id="divNoteDetails" class="col-md-12 rwPadLR0 borderBlack" style="background-color: #FFFFFF; padding: 10px; font-weight: bold">
                <div class="col-md-6 margin-top10 form-group">
                    <div class="row rwPT9">
                        <div id="divEnteredDetails" runat="server">
                            <div class="col-md-3 col-xs-12 col-sm-3 form-group">
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:PageGlobalResources,NoteIDHeader %>"></asp:Label>
                            </div>
                            <div class="col-md-9 col-xs-12 col-sm-9 form-group">
                                <asp:Label runat="server" ID="lblNoteId"></asp:Label>
                            </div>
                            <div class="col-md-3 col-xs-12 col-sm-3 form-group">
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:PageGlobalResources,EnteredByLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-9 col-xs-12 col-sm-9 form-group">
                                <asp:Label runat="server" ID="lblEnteredBy"></asp:Label>
                            </div>
                            <div class="col-md-3 col-xs-12 col-sm-3 form-group">
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:PageGlobalResources,DateEnteredLabel %>"></asp:Label>
                            </div>
                            <div class="col-md-9 col-xs-12 col-sm-9 form-group">
                                <asp:Label runat="server" ID="lblDateEntered"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-3 col-xs-12 col-sm-3 form-group">
                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:PageGlobalResources,SubjectLabel %>"></asp:Label>
                        </div>
                        <div class="col-md-7 col-xs-12 col-sm-7">
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control textLength"></asp:TextBox>
                            <asp:HiddenField ID="txtOriginalSubject" runat="server" />
                            <asp:RequiredFieldValidator ID="valSubject" runat="server" ControlToValidate="txtSubject"
                                ValidationGroup="Subject" Display="Dynamic" ErrorMessage="<%$Resources:PageGlobalResources,SubjectRequiredError %>"
                                SetFocusOnError="True" CssClass="RedNoticeText"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 form-group margin-top10">
                    <div class="row rwPT9">
                        <div class="col-md-2 col-xs-12 col-sm-3 form-group rwPT45">
                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:PageGlobalResources,NotesLabel %>"></asp:Label>
                        </div>
                        <div class="col-md-8 col-xs-12 col-sm-8 form-group">
                            <asp:TextBox ID="txtNotes" CssClass="form-control textLength" runat="server" Height="12em" TextMode="MultiLine" ValidationGroup="Notes" Width="100%"></asp:TextBox>
                            <asp:HiddenField ID="txtOriginalNotes" runat="server" />
                            <asp:RequiredFieldValidator ID="valNotes" runat="server" ErrorMessage="<%$Resources:PageGlobalResources,NotesRequiredError %>" ValidationGroup="Notes" ControlToValidate="txtNotes"
                                SetFocusOnError="True" Display="Dynamic" CssClass="RedNoticeText"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="valLengthNotes" ClientValidationFunction="ValidateNoteLength" ControlToValidate="txtNotes" ValidationGroup="Notes" runat="server"
                                ErrorMessage="<%$Resources:PageGlobalResources,Length500Error %>" SetFocusOnError="True" Display="Dynamic" CssClass="RedNoticeText"></asp:CustomValidator>
                            
                            <script type="text/javascript">

                                function ValidateNoteLength(sender, args) {
                                    if (args.Value.length > 500) {
                                        args.IsValid = false;
                                    }
                                    else {
                                        args.IsValid = true;
                                    }

                                    return args.IsValid;
                                }

                                                                
                            </script>
                            <div class=" col-sm-12">
                                <asp:Label ID="lblValidation" runat="server" ForeColor="Red" Visible="False" EnableViewState="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row ">
                <div class="col-md-6 col-xs-6 col-sm-6 text-left rwPLR0">
                </div>
                <div class="col-md-6 col-xs-6 col-sm-6 margin-top15 text-right rwPLR0">
                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-warning btnLogOff" CausesValidation="false" OnClientClick="return popupConfirm();" Text="<%$Resources:PageGlobalResources,DeleteButton %>" />
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-warning btnLogOff validateAll EnableSave" disabled="disabled" Text="<%$Resources:PageGlobalResources,SaveButton %>"/>
                </div>
            </div>

            <div class=" col-sm-12">
                <asp:HiddenField runat="server" ID="hdnConfirmdelete" Value="<%$Resources:PageGlobalResources,ConfirmNoteDelete %>" />
                <asp:HiddenField runat="server" ID="hdnSaveUpdatedNotesQuestion" Value="<%$Resources:PageGlobalResources,SaveUpdatedNotesQuestion%>" />

                <script type="text/javascript">

                    $(document).ready(function () {

                        //Enable the Save button 
                        $('.textLength').on("focus keyup blur", function () {
                            if ($(this).val().length > 0) {
                                $('.EnableSave').removeAttr('disabled');
                            }
                            else {
                                $('.EnableSave').prop('disabled', 'disabled');
                            }
                        });
                        
                    });
                    
                    // Check for changes to notes and ask if they are to be saved
                    function backButtonClicked(backLocation) {
                        if (notesHaveBeenUpdated() && notesCanBeSaved()) {
                            if (confirm($('#' + "<%=hdnSaveUpdatedNotesQuestion.ClientID%>").val())) {
                                saveNotes();
                            }
                            else
                            {
                                goBackTo(backLocation);
                            }
                        }
                        else {
                            goBackTo(backLocation);
                        }
                        return false;
                    };

                    function notesHaveBeenUpdated() {
                        var updated = false;
                        var theseNotes = $("[id$=txtNotes]");
                        var originalNotes = $("[id$=txtOriginalNotes]");

                        var thisSubject = $("[id$=txtSubject]");
                        var originalSubject = $("[id$=txtOriginalSubject]");

                        if (theseNotes != null && originalNotes != null && thisSubject != null && originalSubject != null) {
                            updated = (theseNotes.val() != originalNotes.val()) | thisSubject.val() != originalSubject .val();
                        }

                        return updated;
                    }
                                        
                    function notesCanBeSaved() {
                        var button = $('[id$=btnSave]');
                        var canBeSaved = false;

                        if (button != null) {
                            canBeSaved = ($('[id$=btnSave]').is(':disabled') == false);
                        }

                        return canBeSaved;
                    }

                    function saveNotes() {
                        var button = $('[id$=btnSave]');

                        if (button != null) {
                            button.trigger("click");
                        }
                    }

                    function goBackTo(backLocation) {
                        $(location).attr('href', backLocation);                        
                    }
                    
                    function popupConfirm() {
                        return confirm($('#' + "<%=hdnConfirmdelete.ClientID%>").val());
                    }
                </script>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
