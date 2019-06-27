Imports IntamacBL_SPR
Imports IntamacShared_SPR
Imports System.Data


Partial Class Admin_GatewaySearch
    Inherits CultureBaseClass

    Private Property AdminFullAccess As Boolean
        Get
            Return Not IsNothing(Session(miscFunctions.p_SessionFullAccess)) AndAlso CType(Session(miscFunctions.p_SessionFullAccess), Boolean)
        End Get
        Set(value As Boolean)
            Session(miscFunctions.p_SessionFullAccess) = value
        End Set
    End Property

    Private ReadOnly Property LoggedInMasterCoID As Integer
        Get
            Return Session(miscFunctions.p_SessionLoggedInMasterCompanyID)
        End Get
    End Property

    Private ReadOnly Property LoggedInMasterCoType As IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes
        Get
            Return CType(Session(miscFunctions.p_SessionLoggedInMasterCompanyTypeID), IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes)

        End Get
    End Property


    Dim savedDetails As NameValueCollection

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        savedDetails = CType(Session("GatewaySearchDetails"), NameValueCollection)

        If Not Page.IsPostBack Then

            Session(miscFunctions.p_SessionAccountID) = String.Empty
            Session(miscFunctions.p_SessionPropertyID) = String.Empty
            Session(miscFunctions.p_SessionAdminLastAccess) = Nothing
            AdminFullAccess = False

            ViewState.Item("sortExpr") = "IntaAccountID"
            ViewState.Item("sortDir") = "ASC"
            restoreSearchDetails()
            dgPanels.PageSize = ddlPageSize.SelectedValue

        End If

    End Sub

    Private Function DoSearch() As DataView

        Dim intMasterCoID As Integer = 0
        Dim dv = New Data.DataView
        saveSearchDetails()


        Dim objPanel As New IntamacBL_SPR.AlarmPanel()

        objPanel.CompanyType = SharedStuff.e_CompanyType.SPR

        Dim dataTab As DataTable = Nothing 'objPanel.LoadPanels(txtMac.Text.Trim())

        If dataTab.Rows.Count = 0 Then
            dgPanels.Visible = False
            lblNoData.Visible = True
        Else
            dgPanels.Visible = True
            lblNoData.Visible = False
        End If

        dv.Table = dataTab

        dv.Sort = ViewState.Item("sortExpr").ToString & " " & ViewState.Item("sortDir").ToString

        Return dv

    End Function

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState.Item("sortExpr") = "AccountID"
        ViewState.Item("sortDir") = "ASC"

        dgPanels.DataSource = DoSearch()
        dgPanels.DataBind()

    End Sub

    Private Sub saveSearchDetails()

        savedDetails = New NameValueCollection()

        If Not String.IsNullOrEmpty(txtMac.Text) Then
            savedDetails.Add("MACAddress", txtMac.Text)
        End If

        savedDetails.Add("PageSize", ddlPageSize.SelectedValue)

        savedDetails.Add("sortExpr", ViewState.Item("sortExpr"))
        savedDetails.Add("sortdir", ViewState.Item("sortDir"))

        If dgPanels.Visible Then
            savedDetails.Add("PageIndex", dgPanels.PageIndex)
        End If

        Session("GatewaySearchDetails") = savedDetails

    End Sub

    Private Sub restoreSearchDetails()

        If (Not IsNothing(savedDetails)) Then
            Dim doneCombos = False

            Dim gridPageIndex As Integer = 0

            For Each searchKey As String In savedDetails.AllKeys
                Select Case searchKey
                    Case "MACAddress"
                        txtMac.Text = savedDetails(searchKey)

                    Case "PageIndex"
                        dgPanels.PageIndex = CInt(savedDetails(searchKey))

                    Case "PageSize"
                        ddlPageSize.SelectedValue = CInt(savedDetails(searchKey))
                        dgPanels.PageSize = ddlPageSize.SelectedValue

                End Select

            Next

            ViewState.Item("sortExpr") = savedDetails("sortExpr")
            ViewState.Item("sortDir") = savedDetails("sortDir")

            dgPanels.DataSource = DoSearch()
            dgPanels.DataBind()

            saveSearchDetails()
        End If
    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        dgPanels.PageSize = ddlPageSize.SelectedValue
        saveSearchDetails()
        dgPanels.DataSource = DoSearch()
        dgPanels.DataBind()

    End Sub

    Protected Sub dgPanels_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgPanels.PageIndexChanging
        dgPanels.PageIndex = e.NewPageIndex
        dgPanels.DataSource = DoSearch()
        savedDetails("PageIndex") = dgPanels.PageIndex
        Session("GatewaySearchDetails") = savedDetails
        dgPanels.DataBind()

    End Sub

    Protected Sub dgPanels_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgPanels.RowDataBound
        Dim lnkbtn As LinkButton

        If e.Row.RowType = DataControlRowType.Header Then

            For Each cell As TableCell In e.Row.Cells
                If cell.Controls.Count > 0 Then
                    lnkbtn = CType(cell.Controls(0), LinkButton)
                    If Not String.IsNullOrEmpty(ViewState.Item("sortExpr")) Then
                        If ViewState.Item("sortExpr").ToString.Equals(lnkbtn.CommandArgument) Then
                            cell.BackColor = System.Drawing.Color.Black
                        End If
                    End If
                End If
            Next

        ElseIf e.Row.RowType = DataControlRowType.DataRow Then

            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)

            Dim lnkFirmware As LinkButton = CType(e.Row.FindControl("lnkFirmware"), LinkButton)

            If lnkFirmware IsNot Nothing And rowView.Item("DeviceID") IsNot Nothing Then

                If lnkFirmware.Text.ToString.Length > 0 And rowView.Item("DeviceID").ToString.Length > 0 Then

                    lnkFirmware.Text = miscFunctions.GetFirmwareName(rowView.Item("DeviceID"), lnkFirmware.Text)

                End If

            End If

        End If
    End Sub

    Protected Sub dgPanels_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgPanels.Sorting

        If e.SortExpression = ViewState("sortExpr") Then
            If ViewState.Item("sortDir") = "DESC" Then
                ViewState.Item("sortDir") = "ASC"
            Else
                ViewState.Item("sortDir") = "DESC"
            End If
        Else
            ViewState.Item("sortExpr") = e.SortExpression
            ViewState.Item("sortDir") = "ASC"
        End If

        dgPanels.DataSource = DoSearch()
        dgPanels.DataBind()

        saveSearchDetails()

    End Sub

    Protected Sub dgPanels_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgPanels.RowCommand

        Select Case e.CommandName
            Case "Diagnostics"
                Dim rowIndex = CInt(e.CommandArgument)
                If Not IsDBNull(dgPanels.DataKeys(rowIndex)(0)) AndAlso Not String.IsNullOrEmpty(dgPanels.DataKeys(rowIndex)(0)) Then
                    Session(miscFunctions.p_SessionAccountID) = dgPanels.DataKeys(rowIndex)(0)
                    Session(miscFunctions.p_SessionPropertyID) = dgPanels.DataKeys(rowIndex)(1)
                    Response.Redirect("AccountDetail.aspx")
                End If
            Case "Firmware"
                Dim rowIndex = CInt(e.CommandArgument)
                If Not IsDBNull(dgPanels.DataKeys(rowIndex)(0)) AndAlso Not String.IsNullOrEmpty(dgPanels.DataKeys(rowIndex)(0)) Then
                    Session(miscFunctions.p_SessionAccountID) = dgPanels.DataKeys(rowIndex)(0)
                    Session(miscFunctions.p_SessionPropertyID) = dgPanels.DataKeys(rowIndex)(1)
                    Response.Redirect("AccountDetail.aspx")
                ElseIf Not IsDBNull(dgPanels.DataKeys(rowIndex)(2)) Then
                    Response.Redirect("HubFirmware.aspx?MAC=" & dgPanels.DataKeys(rowIndex)(2))
                End If
        End Select
    End Sub


End Class
