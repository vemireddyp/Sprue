Imports IntamacShared_SPR.SharedStuff
Imports Telerik.Web.UI


Partial Class Admin_UserAccountSearch
    Inherits CultureBaseClass

#Region "Constants"

    'Used to act as keys for a Dictionary that stores the specified filtering choices 
    Protected Const cSaveAccountID As String = "SaveAccountID"
    Protected Const cSaveAccountType As String = "SaveAccountType"
    Protected Const cSaveCoName As String = "SaveCoName"
    Protected Const cSaveEmail As String = "SaveEmail"
    Protected Const cSaveFirstName As String = "SaveFirstName"
    Protected Const cSaveLastName As String = "SaveLastName"
    Protected Const cSaveCoPageIndexKey As String = "CoPageIndex"
    Protected Const cSaveCoPageSizeKey As String = "CoPageSize"
    Protected Const cSaveCoStatus As String = "SaveCoStatus"
    Protected Const cSaveSearchAccountID As String = "SaveSearchAccountID"
    Protected Const cSaveSearchAccountType As String = "SaveSearchAccountType"
    Protected Const cSaveSearchCoEmail As String = "SaveSearchCoEmail"
    Protected Const cSaveSearchCoFirstName As String = "SaveSearchCoFirstName"
    Protected Const cSaveSearchCoLastName As String = "SaveSearchCoLastName"
    Protected Const cSaveSearchCoName As String = "SaveSearchCoName"
    Protected Const cSaveSearchCoStatus As String = "SaveSearchCoStatus"
    Protected Const cSaveSearchEmail As String = "SaveSearchEmail"
    Protected Const cSaveSearchFirstName As String = "SaveSearchFirstName"
    Protected Const cSaveSearchLastName As String = "SaveSearchLastName"
    Protected Const cSaveSearchParentName As String = "SaveSearchParentName"
    Protected Const cSaveSearchUserCoName As String = "SaveSearchUserCoName"
    Protected Const cSaveSearchUserName As String = "SaveSearchUserName"
    Protected Const cSaveSearchAddress As String = "SaveSearchAddress"
    Protected Const cSavesortDirCoKey As String = "sortDirCo"
    Protected Const cSavesortExprCoKey As String = "sortExprCo"
    Protected Const cSavesortDirUserKey As String = "sortDirUserCo"
    Protected Const cSavesortExprUserKey As String = "sortExprUserCo"
    Protected Const cSaveUserPageIndexKey As String = "UserPageIndex"
    Protected Const cSaveUserPageSizeKey As String = "UserPageSize"
    Protected Const cSaveSearchPerformed As String = "PerformedSearch"

    Protected Const cSessionDetailsKey As String = "UserAccountSearchDetails"

#End Region

#Region "Properties"

    ''' <summary>
    ''' Returns a Dictionary of web controls.  This allows the dictionary to be used to determine the filter criteria specified
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property filterDef As Dictionary(Of String, Control)
        Get
            Return New Dictionary(Of String, Control) From {{cSaveAccountID, txtAccountID}, {cSaveAccountType, ddlAccountType}, {cSaveCoName, txtCoName}, {cSaveCoStatus, ddlStatus}, {cSaveEmail, txtEmail}, {cSaveFirstName, txtFirstName}, _
                                                            {cSaveLastName, txtLastName}, _
                                                            {cSaveSearchAccountID, txtSearchAccountID}, {cSaveSearchEmail, txtSearchEmail}, {cSaveSearchFirstName, txtSearchFirstName}, {cSaveSearchLastName, txtSearchLastName}, _
                                                            {cSaveSearchUserCoName, txtSearchUserCoName}, {cSaveSearchUserName, txtSearchUserName}, {cSaveSearchAddress, txtSearchAddress}, _
                                                            {cSaveSearchAccountType, cboSearchAccountType}, {cSaveSearchCoFirstName, txtSearchCoFirstName}, {cSaveSearchCoEmail, txtSearchCoEmail}, {cSaveSearchCoLastName, txtSearchCoLastName}, _
                                                            {cSaveSearchCoName, txtSearchCoName}, {cSaveSearchParentName, txtSearchParentName}, {cSaveSearchCoStatus, cboSearchCoStatus}}

        End Get
    End Property

    ''' <summary>
    ''' Uses the Session to store the filter choices made by the user.  This can be used to restore the filter choices following a Postback
    ''' </summary>
    ''' <remarks></remarks>
    Private _savedDetails As NameValueCollection

    Private ReadOnly Property savedDetails As NameValueCollection
        Get
            If IsNothing(_savedDetails) Then
                _savedDetails = CType(Session(cSessionDetailsKey), NameValueCollection)

                If IsNothing(_savedDetails) Then
                    _savedDetails = New NameValueCollection()
                    Session(cSessionDetailsKey) = _savedDetails
                End If
            End If

            Return _savedDetails
        End Get
    End Property

    Public Property GridPopulateCompany As Boolean
        Get
            If Not IsNothing(ViewState("GridPopulateCompany")) Then
                Return CBool(ViewState("GridPopulateCompany"))
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value <> GridPopulateCompany Then
                ViewState("GridPopulateCompany") = value
            End If
        End Set
    End Property

    Public Property GridPopulateEndUser As Boolean
        Get
            If Not IsNothing(ViewState("GridPopulateEndUser")) Then
                Return CBool(ViewState("GridPopulateEndUser"))
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value <> GridPopulateEndUser Then
                ViewState("GridPopulateEndUser") = value
            End If
        End Set
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Commits the filtering criteria to the Session so that it can be recovered and redisplayed following a Postback
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub saveSearchDetails()

        'grab a Dictionary of the various web filtering controls
        Dim fDefCache As Dictionary(Of String, Control) = filterDef

        'iterate through all the keys within the dictionary
        For Each ctlKey As String In fDefCache.Keys

            If TypeOf fDefCache(ctlKey) Is RadTextBox Then
                'the current filtering control is a Telerik Text box.  Grab its text value and store it in the Session

                SetSavedDetail(ctlKey, DirectCast(fDefCache(ctlKey), RadTextBox).Text)

            ElseIf TypeOf fDefCache(ctlKey) Is TextBox Then
                'the current filtering control is a Text box.  Grab its text value and store it in the Session

                SetSavedDetail(ctlKey, DirectCast(fDefCache(ctlKey), TextBox).Text)

            ElseIf TypeOf fDefCache(ctlKey) Is RadComboBox Then

                'the current filtering control is a Telerik dropdown.  Grab its selected value and store it in the Session

                Dim cboList As RadComboBox = DirectCast(fDefCache(ctlKey), RadComboBox)

                If Not (IsNothing(cboList) OrElse String.IsNullOrEmpty(cboList.SelectedValue)) AndAlso cboList.SelectedValue <> "0" Then
                    SetSavedDetail(ctlKey, cboList.SelectedValue)
                Else
                    savedDetails.Remove(ctlKey)
                End If

            End If

        Next
    End Sub

    ''' <summary>
    ''' Stores the specified filtering value, such as text box value, into the Session by updating a specified key within a Session dictionary object
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Private Sub SetSavedDetail(ByVal key As String, ByVal value As String)
        Dim blnFilterChanged As Boolean = False

        If Not String.IsNullOrEmpty(savedDetails(key)) Then

            If Not String.IsNullOrEmpty(value) Then
                blnFilterChanged = savedDetails(key) <> value
                savedDetails(key) = value
            Else
                savedDetails.Remove(key)

            End If
        Else
            If Not String.IsNullOrEmpty(value) Then
                savedDetails.Add(key, value)
                blnFilterChanged = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Resets the filtering
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub resetfilterControls()

        If (savedDetails.Count > 0) Then

            Dim fDefCache As Dictionary(Of String, Control) = filterDef
            For Each searchKey As String In savedDetails.AllKeys

                If fDefCache.ContainsKey(searchKey) Then
                    If Not IsNothing(fDefCache(searchKey)) Then
                        Dim saveVal As String = IIf(Not String.IsNullOrEmpty(savedDetails(searchKey)), savedDetails(searchKey), "")

                        If TypeOf fDefCache(searchKey) Is RadTextBox Then
                            DirectCast(fDefCache(searchKey), RadTextBox).Text = saveVal
                        ElseIf TypeOf fDefCache(searchKey) Is RadComboBox Then
                            Dim ddlList As RadComboBox = DirectCast(fDefCache(searchKey), RadComboBox)
                            If ddlList.Items.Count > 0 AndAlso Not String.IsNullOrEmpty(saveVal) Then
                                ddlList.SelectedValue = saveVal
                            Else
                                ddlList.Text = ""
                            End If

                        ElseIf TypeOf fDefCache(searchKey) Is RadGrid Then

                        End If
                    End If

                End If

            Next

        End If

    End Sub

    ''' <summary>
    ''' Returns the filtering values as they were before the Postback occurred.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub restoreSearchDetails()

        'reset the filtering
        resetfilterControls()

        If IsPostBack OrElse (savedDetails.Count > 0) Then
            'bind the data to the grids
            If GridPopulateCompany Then
                rgCompanies.DataSource = Nothing
                rgCompanies.DataBind()
            End If

            If GridPopulateEndUser Then
                rgEndUsers.DataSource = Nothing
                rgEndUsers.DataBind()
            End If


        End If
    End Sub

    ''' <summary>
    ''' Performs the search against the database and returns the order dataview
    ''' </summary>
    ''' <param name="targGrid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Private Function DoSearch(ByVal targGrid As Telerik.Web.UI.RadGrid) As Data.DataView

        Dim dtb As New Data.DataTable

        Dim dvUsers As Data.DataView = Nothing

        If Not Me.ResponseComplete Then

            saveSearchDetails()

            Dim strFNameFilter As String = txtFirstName.Text.Trim()
            Dim strLNameFilter As String = txtLastName.Text.Trim()
            Dim strEmailFilter As String = txtEmail.Text.Trim()

            If targGrid Is rgCompanies Then

                rgCompanies.DataSource = Nothing

                Dim coStatusID As Integer = 0

                If Not String.IsNullOrEmpty(ddlStatus.SelectedValue) Then
                    coStatusID = CType(System.ComponentModel.TypeDescriptor.GetConverter(GetType(IntamacShared_SPR.SharedStuff.e_OpcoStatus)).ConvertFromString(ddlStatus.SelectedValue), IntamacShared_SPR.SharedStuff.e_OpcoStatus)
                Else
                    If Not (IsNothing(cboSearchCoStatus) OrElse String.IsNullOrEmpty(cboSearchCoStatus.SelectedValue)) Then
                        coStatusID = CType(System.ComponentModel.TypeDescriptor.GetConverter(GetType(IntamacShared_SPR.SharedStuff.e_OpcoStatus)).ConvertFromString(cboSearchCoStatus.SelectedValue), IntamacShared_SPR.SharedStuff.e_OpcoStatus)
                    End If
                End If

                '   Get Companies Data
                Try
                    If ddlAccountType.SelectedValue <> "E" Then
                        'not End User selected
                        GridPopulateCompany = True

                        Dim coTypeID As Integer = 0

                        If Not String.IsNullOrEmpty(ddlAccountType.SelectedValue) Then
                            coTypeID = ddlAccountType.SelectedValue
                        ElseIf Not (IsNothing(cboSearchAccountType) OrElse String.IsNullOrEmpty(cboSearchAccountType.SelectedValue)) Then
                            coTypeID = cboSearchAccountType.SelectedValue
                        End If

                        Dim coName As String = txtCoName.Text.Trim()

                        Dim masterCo As New IntamacBL_SPR.MasterCompanySPR

                        If Not Roles.IsUserInRole("installer") Then
                            'companyname, status, , account type
                            dtb = masterCo.LoadSearch(coName, coStatusID, 0, coTypeID)
                        End If

                    End If

                Catch
                End Try

                If Not IsNothing(dtb) Then

                    dvUsers = dtb.DefaultView

                    If dtb.Rows.Count > 0 Then
                        If Not (IsNothing(mUsersCompany) OrElse mUsersCompany.ParentMasterCoID = 0 OrElse IsNothing(dvUsers) OrElse mUsersCompany.CompanyTypeID < CInt(e_MasterCompanyTypes.Distributor)) Then
                            dvUsers.RowFilter = String.Format("([MasterCoID] = {0} OR [ParentMasterCoID] = {0})", mUsersCompany.MasterCoID)

                        End If

                        If Not String.IsNullOrEmpty(strFNameFilter) Then
                            dvUsers.RowFilter &= String.Format("{0}([FirstName] LIKE '{1}%')", IIf(Not String.IsNullOrEmpty(dvUsers.RowFilter), " AND ", ""), strFNameFilter.Replace("'"c, "''"))
                        End If
                        If Not String.IsNullOrEmpty(strLNameFilter) Then
                            dvUsers.RowFilter &= String.Format("{0}([LastName] LIKE '{1}%')", IIf(Not String.IsNullOrEmpty(dvUsers.RowFilter), " AND ", ""), strLNameFilter.Replace("'"c, "''"))
                        End If
                        If Not String.IsNullOrEmpty(strEmailFilter) Then
                            dvUsers.RowFilter &= String.Format("{0}([Email] LIKE '{1}%')", IIf(Not String.IsNullOrEmpty(dvUsers.RowFilter), " AND ", ""), strEmailFilter.Replace("'"c, "''"))
                        End If
                    End If
                End If


            ElseIf targGrid Is rgEndUsers Then

                '
                '   Get End Users Data

                rgEndUsers.DataSource = Nothing


                If ddlAccountType.SelectedValue = "E" OrElse ddlAccountType.SelectedValue = "" Then

                    GridPopulateEndUser = True

                    Dim loginData As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")

                    Dim intParentId As Integer = 0
                    Dim intInstaller As Integer = 0

                    If Roles.IsUserInRole("Installer") Then
                        intInstaller = mLoggedInUser.UserID
                    Else
                        If Not (mUsersCompany.CompanyTypeID = 1 OrElse mUsersCompany.CompanyTypeID = 2) Then
                            intParentId = mUsersCompany.MasterCoID

                        End If
                    End If

                    Dim strAccFilter As String = txtAccountID.Text.Trim()
                    Dim strCoNameFilter As String = txtCoName.Text.Trim()

                    dtb = loginData.LoadSearchWithUser(strAccFilter, strEmailFilter, strFNameFilter, strLNameFilter, savedDetails(cSaveSearchUserName), strCoNameFilter, intParentId, intInstaller)

                    dvUsers = dtb.DefaultView()

                End If

            End If

        End If

        Return dvUsers

    End Function

    Protected Sub ClearSearchFilters(ByVal targGrid As RadGrid)

        Dim targControls As New List(Of Control)

        If targGrid Is Nothing OrElse targGrid Is rgCompanies Then
            targControls.AddRange(New List(Of Control) From {cboSearchAccountType, txtSearchCoFirstName, txtSearchCoEmail, txtSearchCoLastName, txtSearchCoName, txtSearchParentName, cboSearchCoStatus})
        End If
        If targGrid Is Nothing OrElse targGrid Is rgEndUsers Then
            targControls.AddRange(New List(Of Control) From {txtSearchAccountID, txtSearchFirstName, txtSearchEmail, txtSearchLastName, txtSearchUserCoName, txtSearchUserName, txtSearchAddress})
        End If


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

    End Sub
#End Region

#Region "Grid Filter control refs"
#Region "Companies Grid"

    Private ReadOnly Property cboSearchAccountType As Telerik.Web.UI.RadComboBox
        Get
            Dim retDDL As Telerik.Web.UI.RadComboBox = DirectCast(DeepFindControl(rgCompanies, "cboSearchAccountType"), Telerik.Web.UI.RadComboBox)

            Return retDDL
        End Get
    End Property

    Private ReadOnly Property txtSearchCoEmail As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgCompanies, "txtSearchCoEmail"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchCoFirstName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgCompanies, "txtSearchCoFirstName"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchCoLastName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgCompanies, "txtSearchCoLastName"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchCoName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgCompanies, "txtCompanyName"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property cboSearchCoStatus As Telerik.Web.UI.RadComboBox
        Get
            Dim retDDL As Telerik.Web.UI.RadComboBox = DirectCast(DeepFindControl(rgCompanies, "cboSearchCoStatus"), Telerik.Web.UI.RadComboBox)

            Return retDDL
        End Get
    End Property

    Private ReadOnly Property txtSearchParentName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgCompanies, "txtParentName"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

#End Region

#Region "EndUsers Grid"

    Private ReadOnly Property txtSearchAccountID As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgEndUsers, "txtIntaAccountID"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchEmail As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgEndUsers, "txtSearchEmail"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchFirstName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgEndUsers, "txtSearchFirstName"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchLastName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgEndUsers, "txtSearchLastName"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchUserCoName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgEndUsers, "txtParentCompanyName"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property
    Private ReadOnly Property txtSearchUserName As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgEndUsers, "txtSearchUsername"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchAddress As Telerik.Web.UI.RadTextBox
        Get
            Dim retTxt As Telerik.Web.UI.RadTextBox = DirectCast(DeepFindControl(rgEndUsers, "txtSearchAddress"), Telerik.Web.UI.RadTextBox)

            Return retTxt
        End Get
    End Property
#End Region

#End Region

#Region "New/Overrides" 
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        If Not ResponseComplete Then
            mMasterCoID = 0

            Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)

            ajaxManager.AjaxSettings.AddAjaxSetting(btnCompanydetailsOpen, radAjaxPanelCompanies)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnCompanydetailsOpen, radAjaxPanelEndUsers)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnEndUsersDetailsOpen, radAjaxPanelCompanies)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnEndUsersDetailsOpen, radAjaxPanelEndUsers)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, radAjaxPanelEndUsers)

            ajaxManager.AjaxSettings.AddAjaxSetting(btnSearch, radAjaxPanelEndUsers)
            If Not Roles.IsUserInRole(miscFunctions.rolenames.Installer) Then
                ajaxManager.AjaxSettings.AddAjaxSetting(btnSearch, radAjaxPanelCompanies)
                ajaxManager.AjaxSettings.AddAjaxSetting(rgCompanies, rgCompanies, lpCoGridLoading)
                ajaxManager.AjaxSettings.AddAjaxSetting(rgCompanies, radAjaxPanelEndUsers)
                ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, radAjaxPanelCompanies)

            End If
            ajaxManager.AjaxSettings.AddAjaxSetting(rgEndUsers, rgEndUsers, lpUserGridLoading)

            If Not IsPostBack Then

                Session.Remove(miscFunctions.c_SessionUserIDToEdit)
                mAccountID = ""
                mPropertyID = ""

                'Fixed the pagination at 50 records
                If mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.SystemOwner) Then
                    ' Intamac users cannot create accounts
                    btnCreateAccount.Visible = False
                End If

                If Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
                    divPropCompanies.Visible = False
                    ' remove all but the 'end user' entry, define initial value as 'boiler plate'
                    Dim saveItem As ListItem = New ListItem("", "")

                    For Each item As ListItem In ddlAccountType.Items
                        If item.Value = "E" Then
                            saveItem = item
                        End If
                    Next

                    ddlAccountType.Items.Clear()
                    ddlAccountType.Items.Add(saveItem)
                    ddlAccountType.Enabled = False

                    txtCoName.Text = mUsersCompany.Name
                    txtCoName.Enabled = False

                End If

                restoreSearchDetails()

            End If
        End If

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)
        If Not ResponseComplete Then

            divCompanies.Visible = (String.IsNullOrEmpty(ddlAccountType.SelectedValue) OrElse ddlAccountType.SelectedValue <> "E")
            divEndUsers.Visible = (String.IsNullOrEmpty(ddlAccountType.SelectedValue) OrElse ddlAccountType.SelectedValue = "E")
            'Hide the Company details when user search by Account ID and Account Type is ALL. SP-794
            If ddlAccountType.SelectedValue = "" AndAlso Not String.IsNullOrEmpty(txtAccountID.Text) Then
                divCompanies.Visible = False
            End If


            '  only correctly identified, non-intamac, users are able to create company and end-user accounts
            btnCreateAccount.Visible = (Not (IsNothing(mUsersCompany) OrElse IsNothing(mLoggedInUser) OrElse mUsersCompany.CompanyTypeID = e_MasterCompanyTypes.SystemOwner))

            resetfilterControls()
        End If

    End Sub
#End Region

#Region "Click Events"

    ''' <summary>
    ''' Search button click event.  Rebinds data to the grid based upon specified search criteria
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        SetSavedDetail(cSaveSearchPerformed, DateTime.UtcNow.ToString("U"))

        ClearSearchFilters(Nothing)

        For Each col As GridColumn In rgCompanies.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        For Each col As GridColumn In rgEndUsers.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        rgCompanies.MasterTableView.FilterExpression = String.Empty
        rgEndUsers.MasterTableView.FilterExpression = String.Empty

        If Not Roles.IsUserInRole(miscFunctions.rolenames.Installer) Then
            rgCompanies.DataSource = DoSearch(rgCompanies)
            rgCompanies.DataBind()
        End If

        rgEndUsers.DataSource = DoSearch(rgEndUsers)
        rgEndUsers.DataBind()
        ' Open the grid view for the company details
        rgEndUsers.Visible = True
        btnCompanydetailsOpen.Text = GetLocalResourceObject("Close")

        ' Open the grid view for the End Users Details
        rgCompanies.Visible = True
        btnEndUsersDetailsOpen.Text = GetLocalResourceObject("Close")

    End Sub

    Protected Sub btnCreateAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateAccount.Click

        mEditUserID = 0
        Response.Redirect("AdminAccountEdit.aspx")

    End Sub
#End Region

#Region "Grid Events"

    Private Sub rgCompanies_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgCompanies.ItemCommand
        Dim gridItem As GridDataItem

        If TypeOf e.Item Is GridDataItem Then
            gridItem = DirectCast(e.Item, GridDataItem)
        End If

        Dim command As String = e.CommandName.ToLower

        Select Case command
            Case "editaccount"
                mMasterCoID = DirectCast(gridItem.GetDataKeyValue("MasterCoID"), Integer)

                SafeRedirect("AdminAccountEdit.aspx", True)
        End Select
    End Sub

    Private Sub rgEndUsers_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgEndUsers.ItemCommand
        Dim gridItem As GridDataItem

        If TypeOf e.Item Is GridDataItem Then
            gridItem = DirectCast(e.Item, GridDataItem)
        End If

        Dim command As String = e.CommandName.ToLower

        Select Case command
            Case "editaccount"
                mAccountID = gridItem.GetDataKeyValue("IntaAccountID").ToString

                SafeRedirect("AdminAccountEdit.aspx", True)
        End Select
    End Sub

    Private Sub rgCompanies_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles rgCompanies.ItemDataBound

        If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then

            Dim dataItem As Telerik.Web.UI.GridDataItem = DirectCast(e.Item, Telerik.Web.UI.GridDataItem)
            'this gives us access to the data
            Dim oRow As DataRowView = CType(e.Item.DataItem, DataRowView)

            'confirm that the data StatusID is not null
            If Not IsDBNull(oRow("StatusID")) Then

                'store the data StatusID
                Dim strStatus As String = oRow("StatusID")

                'grab the current grid item

                If Not String.IsNullOrEmpty(strStatus) Then
                    'update the text of the StatusID grid item to the enum equivalent for the status
                    dataItem("StatusID").Text = PageString("Status" & CType(strStatus, IntamacShared_SPR.SharedStuff.e_OpcoStatus).ToString())
                End If

            End If

            If Not IsDBNull(oRow("CompanyTypeID")) Then

                Try
                    Dim typeID As e_MasterCompanyTypes = [Enum].Parse(GetType(e_MasterCompanyTypes), oRow("CompanyTypeID"))

                    Select Case typeID
                        Case e_MasterCompanyTypes.ApplicationOwner
                            dataItem("CompanyTypeID").Text = PageString("ApplicationOwnerText")
                        Case e_MasterCompanyTypes.Distributor
                            dataItem("CompanyTypeID").Text = PageString("DistributorText")
                        Case e_MasterCompanyTypes.OperatingCompany
                            dataItem("CompanyTypeID").Text = PageString("ServiceProText")
                    End Select
                Catch ex As Exception

                End Try

            End If

        End If

    End Sub

    Private Sub rgCompanies_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgCompanies.NeedDataSource
        If Not ResponseComplete AndAlso (IsPostBack OrElse savedDetails.Count > 0) Then
            If GridPopulateCompany Then
                rgCompanies.DataSource = DoSearch(rgCompanies)
            End If
        End If

    End Sub

    Private Sub rgCompanies_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles rgCompanies.PageIndexChanged

        rgCompanies.Rebind()
        savedDetails(cSaveUserPageIndexKey) = e.NewPageIndex.ToString()
    End Sub

    Private Sub rgEndUsers_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles rgEndUsers.PageIndexChanged

        rgCompanies.Rebind()

        savedDetails(cSaveUserPageIndexKey) = e.NewPageIndex.ToString()
    End Sub

    Private Sub rgCompanies_PreRender(sender As Object, e As EventArgs) Handles rgCompanies.PreRender
        If Not ResponseComplete Then
            resetfilterControls()
        End If

    End Sub

    Private Sub rgEndUsers_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles rgEndUsers.NeedDataSource
        If Not ResponseComplete AndAlso (IsPostBack OrElse savedDetails.Count > 0) Then
            If GridPopulateEndUser Then
                rgEndUsers.DataSource = DoSearch(rgEndUsers)
            End If
        End If

    End Sub

    Private Sub rgEndUsers_PreRender(sender As Object, e As EventArgs) Handles rgEndUsers.PreRender
        If Not ResponseComplete Then
            resetfilterControls()
        End If

    End Sub
#End Region

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            savedDetails.Clear()
        End If
    End Sub
    ''' <summary>
    '''  Open/Close the grid view for the company details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnCompanydetailsOpen_Click(sender As Object, e As EventArgs)

        Dim clicks As Integer = 0
        clicks = ViewState("Click") + 1
        ViewState("Click") = clicks

        If btnCompanydetailsOpen.Text.Trim() = GetLocalResourceObject("Open") Then
            rgCompanies.Visible = True
            btnCompanydetailsOpen.Text = GetLocalResourceObject("Close")
            If clicks = 1 Then
                rgEndUsers.Visible = False
            End If
        Else
            rgCompanies.Visible = False
            btnCompanydetailsOpen.Text = GetLocalResourceObject("Open")
        End If

        GridPopulateEndUser = rgEndUsers.Visible
        GridPopulateCompany = rgCompanies.Visible

        rgCompanies.Rebind()
        rgEndUsers.Rebind()

    End Sub

    ''' <summary>
    ''' Open/Close the grid view for the End Users Details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnEndUsersDetailsOpen_Click(sender As Object, e As EventArgs)

        Dim clicks As Integer = 0
        clicks = ViewState("Click") + 1
        ViewState("Click") = clicks

        If btnEndUsersDetailsOpen.Text.Trim() = GetLocalResourceObject("Open") Then
            rgEndUsers.Visible = True
            btnEndUsersDetailsOpen.Text = GetLocalResourceObject("Close")
            If clicks = 1 Then
                rgCompanies.Visible = False
            End If
        Else
            rgEndUsers.Visible = False
            btnEndUsersDetailsOpen.Text = GetLocalResourceObject("Open")
        End If

        GridPopulateEndUser = rgEndUsers.Visible
        GridPopulateCompany = rgCompanies.Visible

        'GridPopulate = True
        rgEndUsers.Rebind()
        rgCompanies.Rebind()

    End Sub

    ''' <summary>
    ''' SP-1286 Clears the FilterExpression and rebind the grid views to get actual data 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        'clears the both gridviews
        ClearSearchFilters(Nothing)

        'clears the controls where above  the Company details
        ClearFields()

        For Each col As GridColumn In rgCompanies.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        For Each col As GridColumn In rgEndUsers.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        rgCompanies.MasterTableView.FilterExpression = String.Empty
        rgEndUsers.MasterTableView.FilterExpression = String.Empty

        If Not Roles.IsUserInRole(miscFunctions.rolenames.Installer) Then
            rgCompanies.DataSource = DoSearch(rgCompanies)
            rgCompanies.DataBind()
        End If

        rgEndUsers.DataSource = DoSearch(rgEndUsers)
        rgEndUsers.DataBind()

    End Sub

    ''' <summary>
    ''' SP-1286 Clears the FilterExpression and rebind the grid views to get actual data 
    ''' </summary>
    Protected Sub ClearFields()
        'clears the controls where above  the Company details

        txtFirstName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtEmail.Text = String.Empty
        txtAccountID.Text = String.Empty

        If txtCoName.Enabled Then
            txtCoName.Text = String.Empty
        End If

        If ddlStatus.Enabled Then
            ddlStatus.SelectedIndex = -1
            ddlStatus.Text = String.Empty
            ddlStatus.ClearSelection()
        End If
        ddlAccountType.SelectedIndex = -1
        ddlAccountType.ClearSelection()
    End Sub

End Class
