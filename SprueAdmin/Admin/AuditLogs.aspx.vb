Imports IntamacBL_SPR
Imports System.Collections

Imports Telerik.Web.UI

Imports IntamacShared_SPR.SharedStuff

Partial Class Admin_AuditLogs
    Inherits GridPage

    Public Const cSaveSearchDateEnteredTo As String = "DateEnteredTo"
    Public Const cSaveSearchDateEnteredFrom As String = "DateEnteredFrom"
    Public Const cSaveSearchUsername As String = "Username"
    Public Const cSaveSearchAuditType As String = "AuditType"
    Public Const cSaveSearchNotes As String = "Notes"

    Private ReadOnly Property lstUserNames As List(Of String)
        Get
            Dim retList As List(Of String) = DirectCast(ViewState("lstUserNames"), List(Of String))

            If IsNothing(retList) Then
                retList = New List(Of String)

                ViewState("lstUserNames") = retList
            End If

            Return retList

        End Get
    End Property

    Private ReadOnly Property lstAuditTypes As List(Of e_SPR_AuditActionTypeID)
        Get
            Dim retList As List(Of e_SPR_AuditActionTypeID) = DirectCast(ViewState("lstAuditTypes"), List(Of e_SPR_AuditActionTypeID))

            If IsNothing(retList) Then
                retList = New List(Of e_SPR_AuditActionTypeID)

                ViewState("lstAuditTypes") = retList
            End If

            Return retList

        End Get
    End Property

    Private _usersList As New SortedList()

    Public Overrides Function getFiltersDef() As Dictionary(Of String, Control)
        Return filterDef

    End Function
    Private ReadOnly Property dpSearchDateFrom As RadDatePicker
        Get
            Dim retTxt As RadDatePicker = DirectCast(DeepFindControl(rgAuditLogs, "dpSearchDateFrom"), RadDatePicker)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property dpSearchDateTo As RadDatePicker
        Get
            Dim retTxt As RadDatePicker = DirectCast(DeepFindControl(rgAuditLogs, "dpSearchDateTo"), RadDatePicker)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property cboSearchAuditType As RadComboBox

        Get
            Dim retDDL As RadComboBox = DirectCast(DeepFindControl(rgAuditLogs, "cboSearchAuditType"), RadComboBox)

            Return retDDL
        End Get
    End Property

    Private ReadOnly Property cboSearchUsername As RadComboBox

        Get
            Dim retDDL As RadComboBox = DirectCast(DeepFindControl(rgAuditLogs, "cboSearchUsername"), RadComboBox)

            Return retDDL
        End Get
    End Property

#Region "Preserve Search Details"

    Public ReadOnly Property filterDef As Dictionary(Of String, Control)
        Get
            Return New Dictionary(Of String, Control) From {{cSaveSearchDateEnteredFrom, dpSearchDateFrom}, {cSaveSearchDateEnteredTo, dpSearchDateTo}, {cSaveSearchUsername, cboSearchUsername}, _
                                                            {cSaveSearchNotes, getGridFilterControlByID(Of RadTextBox)("txtSearchNotes")}, {cSaveSearchAuditType, cboSearchAuditType}}

        End Get
    End Property

#End Region

    Private _typeList As SortedList(Of String, Integer)

    Private ReadOnly Property TypeList As SortedList(Of String, Integer)
        Get
            _typeList = CType(ViewState("TypeList"), SortedList(Of String, Integer))

            If IsNothing(_typeList) Then
                _typeList = New SortedList(Of String, Integer)
                ViewState("TypeList") = _typeList
            End If

            Return _typeList
        End Get
    End Property

    Public Overrides Function DoSearch(ByVal gridId As String) As Data.DataView

        ClearSearchFilters()
        saveSearchDetails()

        Dim objAudit As New IntamacDAL_SPR.AuditDB(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)

        Dim dv As Data.DataView
        Dim dSet As DataSet
        Dim dtb As Data.DataTable

        If Not String.IsNullOrEmpty(gridId) Then
            targetGrids(gridId).MasterTableView.FilterExpression = ""
        Else
            targetGrid.MasterTableView.FilterExpression = ""
        End If

        Dim propStatus As Nullable(Of Integer) = Nothing

        Dim enteredDateFrom As Nullable(Of Date)
        Dim enteredDateTo As Nullable(Of Date)
        Dim dateString As String
        If String.IsNullOrEmpty(savedDetails(cSaveSearchDateEnteredFrom)) Then
            SetSavedDateValueDefault(cSaveSearchDateEnteredFrom)
        End If

        dateString = savedDetails(cSaveSearchDateEnteredFrom)

        enteredDateFrom = New Date(CInt(dateString.Substring(0, 4)), CInt(dateString.Substring(4, 2)), CInt(dateString.Substring(6, 2)), 23, 59, 59, 997).AddDays(-1) ' 999 (milliseconds gets rounded up)


        If String.IsNullOrEmpty(savedDetails(cSaveSearchDateEnteredTo)) Then
            SetSavedDateValueDefault(cSaveSearchDateEnteredTo)
        End If

        dateString = savedDetails(cSaveSearchDateEnteredTo)

        enteredDateTo = New Date(CInt(dateString.Substring(0, 4)), CInt(dateString.Substring(4, 2)), CInt(dateString.Substring(6, 2)), 23, 59, 59, 997)


        Dim auditTypeFilter As Integer = 0

        If Not String.IsNullOrEmpty(savedDetails(cSaveSearchAuditType)) Then
            auditTypeFilter = CInt(savedDetails(cSaveSearchAuditType))

        End If

        dSet = objAudit.Search(mAccountID, Nothing, auditTypeFilter, enteredDateFrom, enteredDateTo, savedDetails(cSaveSearchUsername), savedDetails(cSaveSearchNotes), mLoggedInUser.MasterCoID)

        dtb = dSet.Tables(0)

        BindAllUserNamesAndAuditTypes(dtb)

        dv = dtb.DefaultView

        Return dv


    End Function

    Private Sub rgAuditLogs_DataBound(sender As Object, e As EventArgs) Handles rgAuditLogs.DataBound
        Dim blnFirstFill As Boolean = False

        If Not IsNothing(cboSearchUsername) AndAlso cboSearchUsername.Items.Count = 0 AndAlso lstUserNames.Count > 0 Then
            For Each username As String In lstUserNames
                cboSearchUsername.Items.Add(New RadComboBoxItem(username, username))
            Next
        End If

        If Not IsNothing(cboSearchAuditType) AndAlso cboSearchAuditType.Items.Count = 0 AndAlso lstAuditTypes.Count > 0 Then
            blnFirstFill = cboSearchAuditType.Items.Count = 0
            For Each auditType As e_SPR_AuditActionTypeID In lstAuditTypes
                If blnFirstFill OrElse IsNothing(cboSearchAuditType.FindChildByValue(Of RadComboBoxItem)(CInt(auditType).ToString)) Then
                    cboSearchAuditType.Items.Add(New RadComboBoxItem(AuditTypeText(auditType), CInt(auditType)))
                End If
            Next
        End If
    End Sub

    Private Function AuditTypeText(ByVal auditType As e_SPR_AuditActionTypeID) As String
        Dim valueName As String = ""
        Dim value As String = ""
        valueName = auditType.ToString
        Try
            value = GetGlobalResourceObject("AuditTypeResources", valueName)
        Catch
        End Try

        If String.IsNullOrEmpty(value) Then
            value = valueName.Replace("_", " ").Replace("Admin ", "")

        End If

        Return value
    End Function

    ''' <summary>
    ''' Populates the dropdowns for Username and Audit type with all values in the source, rather than just those in the current page.
    ''' </summary>
    ''' <param name="DT"></param>
    ''' <remarks></remarks>
    Private Sub BindAllUserNamesAndAuditTypes(ByVal DT As DataTable)

        If DT Is Nothing Then Exit Sub

        For Each DR As DataRow In DT.Rows

            If Not lstUserNames.Contains(DR("Username"), StringComparer.OrdinalIgnoreCase) Then
                lstUserNames.Add(DR("Username"))
            End If

            Dim auditType As e_SPR_AuditActionTypeID = [Enum].Parse(GetType(e_SPR_AuditActionTypeID), DR("AuditType"))

            If Not lstAuditTypes.Contains(auditType) Then
                lstAuditTypes.Add(auditType)
            End If

        Next

    End Sub

    Private Sub rgAuditLogs_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgAuditLogs.ItemCommand

        If TypeOf e.Item Is GridFilteringItem Then
            targetGrid.DataSource = DoSearch()
            targetGrid.DataBind()
        End If

    End Sub

    Private Sub rgAuditLogs_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgAuditLogs.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim data As DataRowView = DirectCast(dataItem.DataItem, DataRowView)

            'If Not lstUserNames.Contains(data("Username")) Then
            '    lstUserNames.Add(data("Username"))
            'End If

            Try
                Dim auditType As e_SPR_AuditActionTypeID = [Enum].Parse(GetType(e_SPR_AuditActionTypeID), data("AuditType"))

                'If Not lstAuditTypes.Contains(auditType) Then
                '    lstAuditTypes.Add(auditType)
                'End If

                Dim lblNotes As Label = DirectCast(DeepFindControl(e.Item, "lblNotes"), Label)
                Dim lblAuditDescription As Label = DirectCast(DeepFindControl(e.Item, "lblAuditDescription"), Label)

                If Not IsNothing(lblAuditDescription) Then
                    lblAuditDescription.Text = AuditTypeText(auditType)
                End If
                'SP-1415 Remove the Token ID as it is of no user to a support person. Replace with Push Notifications Activated
                If Not IsNothing(lblNotes) Then
                    If data("Notes") IsNot DBNull.Value Then
                        lblNotes.Text = data("Notes")
                        Dim Notes As String = lblNotes.Text
                        If Not String.IsNullOrEmpty(Notes) Then
                            If Notes.Contains("TokenID") Then
                                lblNotes.Text = GetLocalResourceObject("PushNotificationsActivated")
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try

        End If

    End Sub

    ''' <summary>
    ''' Sets searchfilters to default state
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearSearchFilters()

        Dim fdefCache As Dictionary(Of String, Control) = getFiltersDef()

        For Each filterKey As String In fdefCache.Keys
            Select Case filterKey
                Case cSaveSearchDateEnteredFrom, cSaveSearchDateEnteredTo
                    SetSavedDateValueDefault(filterKey)

                Case Else
                    SetSavedDetail(filterKey, Nothing)

            End Select
        Next

        rgAuditLogs.MasterTableView.FilterExpression = String.Empty
    End Sub

    ''' <summary>
    ''' Sets the savedDetails, for the supplied filterKey, to the expected default value
    ''' </summary>
    ''' <param name="filterKey"></param>
    ''' <remarks></remarks>
    Protected Sub SetSavedDateValueDefault(ByVal filterKey As String)

        Dim defaultDate As Date = Now

        Select Case filterKey
            Case cSaveSearchDateEnteredFrom
                defaultDate = Date.Now.AddMonths(-1)

            Case cSaveSearchDateEnteredTo
                ' do nothing use default

        End Select

        SetSavedDetail(filterKey, defaultDate.ToString("yyyyMMdd}"))
    End Sub

End Class
