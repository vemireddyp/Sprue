Imports Telerik.Web.UI

Partial Class Admin_Notes
    Inherits GridPage
#Region "Constants"
    Protected Const cSessionDetailsKey As String = "NotesSearchDetails"
#End Region


#Region "Preserve Search Details"

#End Region

    Overrides Function DoSearch(ByVal gridId As String) As Data.DataView

        Dim dtb As New Data.DataTable

        Dim objNote As IntamacBL_SPR.AdminNotesAgent
        objNote = IntamacBL_SPR.ObjectManager.CreateAdminNotesAgent("SQL")

        Dim dv As Data.DataView = New Data.DataView

        If mAccountID <> "" Then

            dtb = objNote.dtbLoadAdminNotes(mAccountID, mLoggedInUser.MasterCoID)

            'convert date time to master company's time zone of logged in user
            Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)
            dtb = masterCo.ConvertDataTableTimeData(dtb, mLoggedInUser.MasterCoID)

            dv.Table = dtb

        End If

        Return dv

    End Function

    Public Overrides Function getFiltersDef() As Dictionary(Of String, Control)
        '
        '   not implementing filtering on this page so returning an empty dictionary is ok
        Return New Dictionary(Of String, Control)
    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)
        If Not IsPostBack Then
            mBackLocation = "AccountSearch.aspx"
        End If
        If String.IsNullOrEmpty(mAccountID) Then
            Response.Redirect("AccountSearch.aspx")
        End If
        MyBase.OnLoad(e)
    End Sub

    Protected Sub btnAddNote_Click(sender As Object, e As EventArgs) Handles btnAddNote.Click
        mNoteID = 0
        SafeRedirect("NoteDetail.aspx", True)
    End Sub

    Private Sub rgNotes_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles rgNotes.ItemCommand
        Select Case e.CommandName.ToLower
            Case "select"
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)

                mNoteID = CInt(item.GetDataKeyValue("AdminNoteID"))
                SafeRedirect("NoteDetail.aspx", True)


        End Select
    End Sub
End Class
