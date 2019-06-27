Imports IntamacShared_SPR.SharedStuff
Imports Telerik.Web.UI
Partial Class Admin_LoginSearch
    Inherits GridPage
#Region "Constants"
	Protected Const cSessionDetailsKey As String = "LoginSearchDetails"

	Protected Const cPermLevelAll As String = "ALL"
	Protected Const cPermLevelSysAdmin As String = "SA"
	Protected Const cPermLevelSysSupp As String = "SS"
	Protected Const cPermLevelAppAdmin As String = "AA"
	Protected Const cPermLevelAppSupp As String = "AS"
	Protected Const cPermLevelDistAdmin As String = "DA"
	Protected Const cPermLevelDistSupp As String = "DS"
	Protected Const cPermLevelProvAdmin As String = "PA"
	Protected Const cPermLevelProvSupp As String = "PS"
	Protected Const cPermLevelInstaller As String = "I"

	Protected Const cSaveEmail As String = "SaveEmail"
	Protected Const cSaveFirstName As String = "SaveFirstName"
	Protected Const cSaveLastName As String = "SaveLastName"
    Protected Const cSavePermLevel As String = "SavePermLevel"
	Protected Const cSaveUsername As String = "SaveUsername"
	Protected Const cSaveStatus As String = "SaveStatus"
	Protected Const cSaveSearchEmail As String = "SaveSearchEmail"
	Protected Const cSaveSearchFirstName As String = "SaveSearchFirstName"
	Protected Const cSaveSearchLastName As String = "SaveSearchLastName"
	Protected Const cSaveSearchPermLevel As String = "SaveSearchPermLevel"
	Protected Const cSaveSearchStatus As String = "SaveSearchStatus"
	Protected Const cSaveSearchUsername As String = "SaveSearchUsername"

	Protected Const cSavesortDirKey As String = "sortDir"
	Protected Const cSavesortExprKey As String = "sortExpr"

#End Region

#Region "Preserve/Execute Search Details"

	Private _savedDetails As NameValueCollection

    ''' <summary>
    ''' Key used as key in savedDetails
    ''' Value indicates the control to store value of (in savedDetails)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    '''    This property returns a new Dictionary instance on each invocation, so it should be cached locally when used multiple times in a given message (unless it is expected that the contents will have changed between references)
    ''' </remarks>
	Private ReadOnly Property filterDef As Dictionary(Of String, Control)
		Get
			'
			'   

            Return New Dictionary(Of String, Control) From {{cSaveEmail, txtEmail}, {cSaveFirstName, txtFirstName}, {cSaveLastName, txtLastName}, {cSavePageIndexKey, rgUsers}, {cSavePageSizeKey, rgUsers}, {cSavePermLevel, cboPermLevel}, _
                                                            {cSaveSearchEmail, txtSearchEmail}, {cSaveSearchFirstName, txtSearchFirstName}, {cSaveSearchLastName, txtSearchLastName}, {cSaveSearchPermLevel, cboSearchPermLevel}, _
                                                            {cSaveSearchUsername, txtSearchUserName}, {cSaveSearchStatus, cboSearchUserStatus}, {cSaveStatus, cboStatus}, {cSaveUsername, txtUsername}}

		End Get
	End Property

    ''' <summary>
    ''' Returns one, of 2 values from savedDetails
    ''' </summary>
    ''' <param name="primaryKey">If an item of this key is present in savedDetails is the preferred return value.</param>
    ''' <param name="secondaryKey">Key for the item value to return if the primaryKey is not present.</param>
    ''' <returns></returns>
    ''' <remarks>If the value indicated by primaryKey is returned the savedDetails entry for the secondaryKey is cleared down (i.e. set to Nothing)</remarks>
    Public Overloads Function ChooseSavedDetail(ByVal primaryKey As String, secondaryKey As String) As String
        Dim chosenVal As String = String.Empty

        If Not String.IsNullOrEmpty(savedDetails(primaryKey)) Then
            chosenVal = savedDetails(primaryKey)
            'SetSavedDetail(secondaryKey, Nothing)
        Else
            chosenVal = savedDetails(secondaryKey)
        End If

        Return chosenVal

    End Function

    ''' <summary>
    ''' Returns one, of 2 integer values from savedDetails
    ''' </summary>
    ''' <param name="primaryKey">If an item of this key is present in savedDetails is the preferreed return value.</param>
    ''' <param name="secondaryKey">Key for the item value to return if the primaryKey is not present.</param>
    ''' <param name="defaultValue">Value to return if neither key returns a value from savedDetails </param>
    ''' <returns></returns>
    ''' <remarks>If the value indicated by primaryKey is returned the savedDetails entry for the secondaryKey is cleared down (i.e. set to Nothing). </remarks>
    Public Overloads Function ChooseSavedDetail(ByVal primaryKey As String, secondaryKey As String, ByVal defaultValue As Integer) As Integer
        Dim chosenVal As String = ChooseSavedDetail(primaryKey, secondaryKey)

        If Not String.IsNullOrEmpty(chosenVal) Then
            Return CInt(chosenVal)
        Else
            Return defaultValue
        End If

    End Function

    Public Overrides Function getFiltersDef() As Dictionary(Of String, Control)
        Return filterDef
    End Function
    Public Overrides Function DoSearch(gridID As String) As DataView


        saveSearchDetails()

        Dim dtb As New Data.DataTable
        Dim dvUsers As Data.DataView

        Dim masterUser As New IntamacBL_SPR.MasterUserSPR

        Dim filterCo As Integer = 0

        If Not IsNothing(mUsersCompany) Then


            If Not miscFunctions.IsPrivilegedCoUser() Then
                filterCo = mUsersCompany.MasterCoID
            End If

            Dim intStatusFilter As Integer = ChooseSavedDetail(cSaveStatus, cSaveSearchStatus, 0)

            Dim strFNameFilter As String = ChooseSavedDetail(cSaveFirstName, cSaveSearchFirstName)
            Dim strLNameFilter As String = ChooseSavedDetail(cSaveLastName, cSaveSearchLastName)
            Dim strEmailFilter As String = ChooseSavedDetail(cSaveEmail, cSaveSearchEmail)
            Dim strUsernameFilter As String = ChooseSavedDetail(cSaveUsername, cSaveSearchUsername)

            Dim strRoleFilter As String = ""

            Try
                dtb = masterUser.dtbLoadMasterUsers(filterCo, strUsernameFilter, strFNameFilter, strLNameFilter, strEmailFilter, intStatusFilter, "")
            Catch
            End Try

            dvUsers = dtb.DefaultView
            'rgUsers.MasterTableView.FilterExpression = String.Empty

            Dim strPermFilter As String = ChooseSavedDetail(cSavePermLevel, cSaveSearchPermLevel)

            dvUsers.Table.Columns.Add("PermLevel", GetType(String))

            For Each row As DataRow In dvUsers.Table.Rows
                Dim permLevel As String = String.Empty

                Select Case CInt(row("CompanytypeID"))
                    Case e_MasterCompanyTypes.SystemOwner
                        permLevel = "S"
                    Case e_MasterCompanyTypes.ApplicationOwner
                        permLevel = "A"
                    Case e_MasterCompanyTypes.Distributor
                        permLevel = "D"
                    Case e_MasterCompanyTypes.OperatingCompany
                        permLevel = "P"

                End Select

                If Roles.IsUserInRole(row("UserName"), miscFunctions.rolenames.Installer.ToString) Then
                    permLevel = "I"
                ElseIf Roles.IsUserInRole(row("UserName"), miscFunctions.rolenames.SupportAdmin.ToString) Then
                    permLevel &= "A"
                ElseIf Roles.IsUserInRole(row("UserName"), miscFunctions.rolenames.SupportUser.ToString) Then
                    permLevel &= "S"
                End If

                row("PermLevel") = permLevel

            Next
            If Not String.IsNullOrEmpty(strPermFilter) Then

                dvUsers.RowFilter = String.Format("[PermLevel] = '{0}'", strPermFilter)
            End If
        End If

        Return dvUsers

    End Function

    Protected Sub ClearSearchFilters()

        Dim targControls As New List(Of Control) From {txtSearchEmail, txtSearchFirstName, txtSearchLastName, cboSearchPermLevel, txtSearchUserName, cboSearchUserStatus}


        For Each filterCtl As Control In targControls

            If Not IsNothing(filterCtl) Then

                If TypeOf filterCtl Is RadTextBox Then
                    DirectCast(filterCtl, RadTextBox).Text = String.Empty

                ElseIf TypeOf filterCtl Is RadComboBox Then

                    Dim filterList As RadComboBox = DirectCast(filterCtl, RadComboBox)
                    filterList.SelectedIndex = -1
                    filterList.Text = ""
                End If
            End If
        Next

        'Clear the individual Gridview column filter  G2-2620
        For Each col As GridColumn In rgUsers.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        rgUsers.MasterTableView.FilterExpression = String.Empty
    End Sub

#End Region

#Region "Grid Filter control refs"
    Private ReadOnly Property txtSearchEmail As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgUsers, "txtSearchEmail"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchFirstName As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgUsers, "txtSearchFirstName"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchLastName As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgUsers, "txtSearchLastName"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property cboSearchPermLevel As RadComboBox
        Get
            Dim retDDL As RadComboBox = DirectCast(DeepFindControl(rgUsers, "cboSearchPermLevel"), RadComboBox)

            Return retDDL
        End Get
    End Property

    Private ReadOnly Property cboSearchUserStatus As RadComboBox
        Get
            Dim retDDL As RadComboBox = DirectCast(DeepFindControl(rgUsers, "cboSearchUserStatus"), RadComboBox)

            Return retDDL
        End Get
    End Property

    Private ReadOnly Property txtSearchUserName As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgUsers, "txtSearchUserName"), RadTextBox)

            Return retTxt
        End Get
    End Property



#End Region


    Private Sub LoadPermLevels(ByVal targList As RadComboBox)
		Dim DT As New DataTable
        targList.Items.Clear()

        If Not IsNothing(mUsersCompany) Then
            Dim permEntries As New NameValueCollection From {{cPermLevelSysAdmin, PageString("IntamacAdminLegend")}, {cPermLevelSysSupp, PageString("IntamacSupportLegend")}, _
                                                             {cPermLevelAppAdmin, PageString("SprueAdminLegend")}, {cPermLevelAppSupp, PageString("SprueSupportLegend")}, {cPermLevelDistAdmin, PageString("DistAdminLegend")}, _
                                                             {cPermLevelDistSupp, PageString("DistSupportLegend")}, {cPermLevelProvAdmin, PageString("ProvAdminLegend")}, {cPermLevelProvSupp, PageString("ProvSupportLegend")}, _
                                                             {cPermLevelInstaller, PageString("InstallerLegend")}}

            If mUsersCompany.CompanyTypeID <> CInt(e_MasterCompanyTypes.SystemOwner) Then
                permEntries.Remove(cPermLevelSysAdmin)
                permEntries.Remove(cPermLevelSysSupp)

                If mUsersCompany.CompanyTypeID <> CInt(e_MasterCompanyTypes.ApplicationOwner) Then
                    permEntries.Remove(cPermLevelAppAdmin)
                    permEntries.Remove(cPermLevelAppSupp)

                    If mUsersCompany.CompanyTypeID <> CInt(e_MasterCompanyTypes.Distributor) Then
                        permEntries.Remove(cPermLevelDistAdmin)
                        permEntries.Remove(cPermLevelDistSupp)

                    End If
                End If
            End If



			'If targList Is cboSearchPermLevel Then

			'	Dim DC1 As DataColumn = Nothing
			'	DC1 = New DataColumn
			'	DC1.ColumnName = "CompanyTypeID"
			'	DT.Columns.Add(DC1)
			'	Dim DC2 As DataColumn = Nothing
			'	DC2 = New DataColumn
			'	DC2.ColumnName = "CompanyType"
			'	DT.Columns.Add(DC2)

			'	For Each value As String In permEntries.Keys

			'		Dim DR As DataRow = DT.NewRow
			'		DR("CompanyType") = permEntries(value)
			'		DR("CompanyTypeID") = value
			'		DT.Rows.Add(DR)

			'	Next

			'	targList.DataSource = DT
			'	targList.DataBind()

			'Else
			For Each value As String In permEntries.Keys
				targList.Items.Add(New RadComboBoxItem(permEntries(value), value))
			Next
			'		End If

		End If
    End Sub

    Private Sub LoadStatusList(ByVal targList As RadComboBox)
        If Not IsNothing(targList) Then
            targList.Items.Clear()

            targList.Items.Add(New RadComboBoxItem(PageString("StatusActive"), "1"))
            targList.Items.Add(New RadComboBoxItem(PageString("StatusDisabled"), "2"))

        End If
    End Sub
#Region "New/Overrides"
	Protected Overrides Sub OnLoad(e As EventArgs)
		MyBase.OnLoad(e)

        Dim ajaxMan As RadAjaxManager = RadAjaxManager.GetCurrent(Me)
        ajaxMan.AjaxSettings.AddAjaxSetting(btnSearch, rgUsers)
        ajaxMan.AjaxSettings.AddAjaxSetting(btnClear, rgUsers)

        If Not IsPostBack Then

            If Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
                ' Installers can only see their own details
                mEditUserID = mLoggedInUser.UserID
                SafeRedirect("LoginDetail.aspx", True)
            Else
                LoadStatusList(cboStatus)

                Session.Remove(miscFunctions.c_SessionUserIDToEdit)
                mAccountID = ""
                mPropertyID = ""
                mEditUserID = 0

                ' btnAddUser should only be visible if the user is from Sprue (i.e. 'ApplicationOwner'), or they are not a 'SupportUser' 
                btnAddUser.Visible = (mUsersCompany IsNot Nothing AndAlso mUsersCompany.CompanyTypeID = e_MasterCompanyTypes.ApplicationOwner) OrElse Not Roles.IsUserInRole(miscFunctions.rolenames.SupportUser.ToString)
                LoadPermLevels(cboPermLevel)
            End If
        End If
    End Sub


#End Region

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'When Search button clicked,Clear search filter only when grid fields are empty
        If txtSearchUserName.Text = String.Empty And cboSearchPermLevel.SelectedIndex = -1 And txtSearchFirstName.Text = String.Empty And txtSearchLastName.Text = String.Empty And txtSearchEmail.Text = String.Empty And cboSearchUserStatus.SelectedIndex = -1 Then
            ClearSearchFilters()
        End If
        rgUsers.DataSource = DoSearch()
        rgUsers.DataBind()

    End Sub

	Protected Sub btnAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUser.Click

		mEditUserID = 0
		Response.Redirect("LoginAdd.aspx")

	End Sub

    Private Sub rgUsers_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgUsers.ItemCommand

        Select Case e.CommandName.ToLower
            'Case "filter"
            '    rgUsers.DataSource = DoSearch()

            '    rgUsers.DataBind()
            Case "select"
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)

                Session(miscFunctions.c_SessionUserIDToEdit) = item.GetDataKeyValue("UserID")
                SafeRedirect("LoginDetail.aspx", True)

        End Select
    End Sub

    Private Sub rgUsers_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgUsers.ItemDataBound
        If TypeOf e.Item Is GridFilteringItem Then
            If Not IsNothing(cboSearchPermLevel) Then
                LoadPermLevels(cboSearchPermLevel)

            End If
            If Not IsNothing(cboSearchUserStatus) Then
                LoadStatusList(cboSearchUserStatus)
            End If
        ElseIf TypeOf e.Item Is GridDataItem Then
            Dim data As DataRowView = DirectCast(e.Item.DataItem, DataRowView)
            Dim lbl As Label = DirectCast(DeepFindControl(e.Item, "lblUserStatus"), Label)

            If Not IsNothing(lbl) Then
                lbl.Text = PageString("UserStatus" & [Enum].Parse(GetType(e_UserStatus), data("UserStatusID")).ToString)
            End If

            lbl = DirectCast(DeepFindControl(e.Item, "lblPermissionLevel"), Label)

            If Not IsNothing(lbl) Then
                Dim strLabelPrefix As String = ""

                Select Case CInt(data("CompanyTypeID"))
                    Case CInt(e_MasterCompanyTypes.SystemOwner)
                        strLabelPrefix = "Intamac"

                    Case CInt(e_MasterCompanyTypes.ApplicationOwner)
                        strLabelPrefix = "Sprue"

                    Case CInt(e_MasterCompanyTypes.Distributor)
                        strLabelPrefix = "Dist"

                    Case Else
                        strLabelPrefix = "Prov"

                End Select

                If Roles.IsUserInRole(data("Username"), miscFunctions.rolenames.Installer.ToString) Then
                    strLabelPrefix = "Installer"
                ElseIf Roles.IsUserInRole(data("Username"), miscFunctions.rolenames.SupportAdmin.ToString) Then
                    strLabelPrefix &= "Admin"
                ElseIf Roles.IsUserInRole(data("Username"), miscFunctions.rolenames.SupportUser.ToString) Then
                    strLabelPrefix &= "Support"

                End If

                lbl.Text = PageString(strLabelPrefix & "Legend")

            End If



        End If
    End Sub


    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clearFields()
        ClearSearchFilters()
        rgUsers.Rebind()

    End Sub
    'method to clear field values
    Private Sub clearFields()
        txtUsername.Text = String.Empty
        txtFirstName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtEmail.Text = String.Empty
        cboPermLevel.ClearSelection()
        cboStatus.ClearSelection()
    End Sub

End Class
