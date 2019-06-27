
Partial Class Admin_NoteDetail
    Inherits CultureBaseClass

    Protected Overrides Sub OnLoad(e As EventArgs)
        If Not IsPostBack AndAlso String.IsNullOrEmpty(mBackLocation) Then
            mBackLocation = "Notes.aspx"
        End If

        MyBase.OnLoad(e)

        If String.IsNullOrEmpty(mAccountID) Then
            Response.Redirect("AccountSearch.aspx")
        End If

        If Not IsPostBack Then
            If mNoteID > 0 Then
				Dim pa As IntamacBL_SPR.AdminNotesAgent = IntamacBL_SPR.ObjectManager.CreateAdminNotesAgent("SQL")

                Dim objNote As ClassLibrary_Interface.iAdminNotes = pa.LoadAdminNote(mNoteID, mLoggedInUser.MasterCoID)

                If Not IsNothing(objNote) Then
                    lblNoteId.Text = objNote.AdminNoteID
                    lblEnteredBy.Text = objNote.EnteredBy
                    lblDateEntered.Text = String.Format("{0:d} {0:t}", objNote.DateEntered)
                    txtSubject.Text = objNote.Subject
                    txtOriginalSubject.Value = objNote.Subject
                    txtNotes.Text = objNote.Notes
                    txtOriginalNotes.Value = objNote.Notes
                End If
            Else
                divEnteredDetails.Visible = False
                btnDelete.Visible = False
            End If
        End If
    End Sub
    Protected Overrides Sub OnPreRender(e As EventArgs)

        MyBase.OnPreRender(e)

        ' For this page, overwrite OnClientClick event from CultureBaseClass with 
        ' the backButtonClicked() method in the Javascript on this page
        Dim buttonBack = DirectCast(DeepFindControl(Master, "btnBack"), Button)
        If Not IsNothing(buttonBack) Then
            mBtnBack.OnClientClick = String.Format("return backButtonClicked('{0}');", mBackLocation)
        End If

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsValid Then
            Dim pa As IntamacBL_SPR.AdminNotesAgent = IntamacBL_SPR.ObjectManager.CreateAdminNotesAgent("SQL")

            Dim objNote As ClassLibrary_Interface.iAdminNotes = pa.LoadAdminNote(mNoteID, mLoggedInUser.MasterCoID)

            'fill the rest of the details
            objNote.AccountID = mAccountID
            objNote.AdminNoteID = mNoteID
            objNote.Subject = txtSubject.Text
            objNote.DateEntered = Date.UtcNow
            objNote.EnteredBy = User.Identity.Name
            objNote.Notes = txtNotes.Text

            If pa.Save(objNote) Then

                If divEnteredDetails.Visible = False Then
                    miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Notes_Added, "Note ID: " & objNote.AdminNoteID)
                Else
                    miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Notes_Updated, "Note ID: " & objNote.AdminNoteID)
                End If

                Response.Redirect("Notes.aspx")

            Else
                lblValidation.Text = pa.ValidationErrors.Values(0)
                lblValidation.Visible = True
            End If
        End If
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
		Dim objNote As IntamacBL_SPR.AdminNotesAgent = IntamacBL_SPR.ObjectManager.CreateAdminNotesAgent("SQL")

        objNote.Delete(mNoteID)

        miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Notes_Deleted, "Note ID: " & mNoteID)

        Response.Redirect("Notes.aspx")

    End Sub

    Protected Sub valLengthNotes_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valLengthNotes.ServerValidate
        args.IsValid = args.Value.Length <= 500

    End Sub
End Class
