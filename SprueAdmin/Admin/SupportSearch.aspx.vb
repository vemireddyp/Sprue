Imports Telerik.Web.UI
Imports IntamacShared_SPR.SharedStuff

Partial Class Admin_SupportSearch
    Inherits CultureBaseClass

#Region "Constants"
    Protected Const cSaveRefNoKey As String = "SupportRequestID"
    Protected Const cSaveAccountIDKey As String = "AccountID"
    Protected Const cSaveDeviceIDKey As String = "DeviceID"
    Protected Const cSaveSurnameKey As String = "Surname"
    Protected Const cSavePostCodeKey As String = "PostCode"
    Protected Const cSaveAdminUserKey As String = "UpdatedBy"
    Protected Const cSaveStatusKey As String = "StatusDescription"
    Protected Const cSaveTicketStatusKey As String = "TicketStatus"
    Protected Const cSaveMacAddressKey As String = "MacAddress"
    Protected Const cSaveLastUpdatedKey As String = "LastUpdated"
    Protected Const cSavePageIndexKey As String = "PageIndex"
    Protected Const cSavePageSizeKey As String = "PageSize"
    Protected Const cSaveSortDirKey As String = "sortDir"
    Protected Const cSaveSortExprKey As String = "sortExpr"

    Protected Const cSessionDetailsKey As String = "SupportSearchDetails"
#End Region

#Region "Preserve Search Details"

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

    Private Sub SetSavedDetail(ByVal key As String, ByVal value As String)
        If Not String.IsNullOrEmpty(savedDetails(key)) Then
            If Not String.IsNullOrEmpty(value) Then
                savedDetails(key) = value
            Else
                savedDetails.Remove(key)
            End If
        Else
            If Not String.IsNullOrEmpty(value) Then
                savedDetails.Add(key, value)
            End If
        End If

    End Sub

    Private Function DoSearch() As Data.DataView


        Dim objSupportRequest As IntamacBL_SPR.SupportRequest = IntamacBL_SPR.ObjectManager.CreateSupportRequest(mCompanyType)
        Dim dv As New Data.DataView
        Dim dtb As Data.DataTable

        If Not ResponseComplete Then

            Dim intRefNo As Integer = 0
            Dim strUserID As String = ""

            Dim intParentCoID As Integer = 0

            If mMasterCoID = 0 Then
                If mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.Distributor) Or mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.OperatingCompany) Then
                    intParentCoID = mUsersCompany.MasterCoID
                End If
            Else
                intParentCoID = mMasterCoID
            End If

            saveSearchDetails()

            If IsNumeric(txtRefNo.Text) Then intRefNo = CType(txtRefNo.Text, Integer)
            strUserID = cboUserName.SelectedValue

            Dim dateFrom As DateTime
            Dim dateTo As DateTime
            If dpFromDate.SelectedDate Is Nothing Then
                dateFrom = Nothing
            Else
                dateFrom = DateTime.Parse(dpFromDate.SelectedDate.Value)
            End If
            If dpToDate.SelectedDate Is Nothing Then
                dateTo = Nothing
            Else
                dateTo = DateTime.Parse(dpToDate.SelectedDate.Value)
                ' we want to include all tickets from this day so set time to 23:59:59
                dateTo = New DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59)
            End If

            Dim intInstallerID As Integer = mInstallerID

            If intInstallerID = 0 AndAlso Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
                intInstallerID = mLoggedInUser.UserID

                ' we're filtering by installer, so parentCo is an unneccessary filter
                intParentCoID = 0
            End If

            Dim statusId As Integer = 0

            If Not String.IsNullOrEmpty(mSelectedGateway) AndAlso mSelectedGateway <> txtMACAddress.Text Then
                mSelectedGateway = txtMACAddress.Text
            End If

            If Not String.IsNullOrEmpty(cboStatus.SelectedValue) Then
                statusId = CInt(cboStatus.SelectedValue)
            End If

            Dim unitIDFrom As String = Nothing
            Dim unitIDTo As String = Nothing

            If Not String.IsNullOrEmpty(mDeviceID) Then
                Dim unitIDs As String() = mDeviceID.Split("<"c)

                If unitIDs.Length > 1 Then
                    unitIDFrom = unitIDs(0)
                    unitIDTo = unitIDs(1)
                Else
                    If unitIDs.Length > 0 Then
                        If mDeviceID.StartsWith("<") Then
                            unitIDTo = unitIDs(0)
                        Else
                            unitIDFrom = unitIDs(0)
                        End If
                    End If
                End If
            End If
            Dim ra As IntamacBL_SPR.SupportRequestAgent = IntamacBL_SPR.ObjectManager.CreateSupportRequestAgent("SQL")
            dtb = ra.LoadSupportRequests(intRefNo, strUserID, statusId,
            0, txtSurname.Text, txtMACAddress.Text, txtAccountID.Text, 0, dateFrom, dateTo, txtPostCode.Text, txtDeviceID.Text, intParentCoID, intInstallerID, mLoggedInUser.MasterCoID, unitIDFrom, unitIDTo, openTicketsOnly)

            'convert date time to master company's time zone of logged in user
            Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)
            dtb = masterCo.ConvertDataTableTimeData(dtb, mLoggedInUser.MasterCoID)

            For Each row As DataRow In dtb.Rows

                'update the status descriptions with any values in PageGlobalResources
                If row("StatusID") = miscFunctions.c_OpenValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen")) Then
                    row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString()
                ElseIf row("StatusID") = miscFunctions.c_ClosedValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusClosed")) Then
                    row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusClosed").ToString()
                ElseIf row("StatusID") = miscFunctions.c_EscalatedToServiceProviderValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToServiceProvider")) Then
                    row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToServiceProvider").ToString()
                ElseIf row("StatusID") = miscFunctions.c_EscalatedToDistributorValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToDistibutor")) Then
                    row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToDistibutor").ToString()
                ElseIf row("StatusID") = miscFunctions.c_EscalatedToSprueValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToSprue")) Then
                    row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToSprue").ToString()
                End If

                'Change the device Id to WG-1 if the deviceId is RNDV2
                If Not String.IsNullOrEmpty(row("DeviceID").ToString()) Then

                    If row("DeviceID") = "RNDV2" Then
                        row("DeviceID") = "WG-1"
                    End If

                End If

            Next

            If dtb.Rows.Count > 0 Then
                'dgRequests.Visible = True
                lblNoData.Visible = False
            Else
                'dgRequests.Visible = False
                lblNoData.Visible = True
            End If

            dv.Table = dtb
            dv.Sort = mSortExpr & " " & mSortDir
        End If

        Return dv


    End Function

    Private Sub saveSearchDetails()

        Dim fDefCache As Dictionary(Of String, Control) = filterDef

        For Each ctlKey As String In fDefCache.Keys

            If Not IsNothing(fDefCache(ctlKey)) Then

                If TypeOf fDefCache(ctlKey) Is RadTextBox Then
                    SetSavedDetail(ctlKey, Server.HtmlEncode(DirectCast(fDefCache(ctlKey), RadTextBox).Text))

                ElseIf TypeOf fDefCache(ctlKey) Is RadComboBox Then

                    Dim cboList As RadComboBox = DirectCast(fDefCache(ctlKey), RadComboBox)

                    If Not (IsNothing(cboList) OrElse String.IsNullOrEmpty(cboList.SelectedValue)) Then
                        SetSavedDetail(ctlKey, Server.HtmlEncode(cboList.SelectedValue))
                    Else
                        savedDetails.Remove(ctlKey)
                    End If

                ElseIf TypeOf fDefCache(ctlKey) Is RadDatePicker Then
                    Dim picker As RadDatePicker = DirectCast(fDefCache(ctlKey), RadDatePicker)

                    If Not IsNothing(picker) AndAlso picker.SelectedDate.HasValue Then
                        SetSavedDetail(ctlKey, picker.SelectedDate.Value.ToUniversalTime.ToString())
                    Else
                        savedDetails.Remove(ctlKey)
                    End If


                ElseIf TypeOf fDefCache(ctlKey) Is RadGrid Then

                    Select Case ctlKey
                        Case cSavePageIndexKey
                            savedDetails(ctlKey) = rgSupportTickets.CurrentPageIndex

                        Case cSavePageSizeKey
                            savedDetails(ctlKey) = rgSupportTickets.PageSize
                    End Select


                End If

            End If

        Next
        SetSavedDetail(cSaveSortExprKey, mSortExpr)
        SetSavedDetail(cSaveSortDirKey, mSortDir)

    End Sub

    Private Sub restoreSearchDetails()

        If (savedDetails.Count > 0) Then
            Dim gridPageIndex As Integer = 0

            For Each searchKey As String In savedDetails.AllKeys
                Select Case searchKey
                    Case "AccountID"
                        txtAccountID.Text = savedDetails(searchKey)

                    Case "CustomerName"
                        txtSurname.Text = savedDetails(searchKey)

                    Case "MACAddress"
                        txtMACAddress.Text = savedDetails(searchKey)

                    Case "RefNo"
                        txtRefNo.Text = savedDetails(searchKey)

                    Case "AdminUserID"
                        cboUserName.SelectedValue = savedDetails(searchKey)

                    Case "Status"
                        cboStatus.SelectedValue = savedDetails(searchKey)

                    Case "PageIndex"
                        'dgRequests.PageIndex = CInt(savedDetails(searchKey))

                        'Case "PageSize"
                        '    ddlPageSize.SelectedValue = CStr(CInt(savedDetails(searchKey)))
                        '    dgRequests.PageSize = CInt(ddlPageSize.SelectedValue)

                End Select

            Next

            mSortExpr = savedDetails(cSaveSortExprKey)
            mSortDir = savedDetails(cSaveSortDirKey)

            resetfilterControls()

        End If
    End Sub

    Private ReadOnly Property filterDef As Dictionary(Of String, Control)
        Get
            Return New Dictionary(Of String, Control) From {{cSaveRefNoKey, txtSearchSupportRequestID}, {cSaveAccountIDKey, txtSearchAccountID}, {cSaveDeviceIDKey, txtSearchDeviceID}, {cSaveSurnameKey, txtSearchLastName},
                                                            {cSavePostCodeKey, txtSearchPostCode}, {cSaveAdminUserKey, txtSearchUpdatedBy}, {cSaveStatusKey, cboSearchStatus}, {cSaveMacAddressKey, txtSearchMacAddress},
                                                            {cSaveLastUpdatedKey, dpSearchSystemTestDate}, {cSavePageIndexKey, rgSupportTickets}, {cSavePageSizeKey, rgSupportTickets}, {cSaveTicketStatusKey, cboStatus}}

        End Get
    End Property

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

                        ElseIf TypeOf fDefCache(searchKey) Is RadDatePicker Then
                            Dim picker As RadDatePicker = DirectCast(fDefCache(searchKey), RadDatePicker)

                            picker.SelectedDate = Convert.ToDateTime(saveVal).ToLocalTime()

                        ElseIf TypeOf fDefCache(searchKey) Is RadGrid Then

                            Select Case searchKey
                                Case cSavePageIndexKey
                                    rgSupportTickets.CurrentPageIndex = CInt(savedDetails(searchKey))

                                Case cSavePageSizeKey
                                    rgSupportTickets.PageSize = CInt(savedDetails(searchKey))
                            End Select


                        End If
                    End If

                End If

            Next

        End If
    End Sub

    Private Sub ClearFilters()

        Dim filtersToClear As New List(Of Control)({txtSearchSupportRequestID, txtSearchAccountID, txtSearchDeviceID, txtSearchLastName, txtSearchPostCode, txtSearchUpdatedBy, cboSearchStatus, txtSearchMacAddress, dpSearchSystemTestDate})

        savedDetails.Clear()

        For Each filterCtl As Control In filtersToClear

            If Not IsNothing(filterCtl) Then
                If CType(filterCtl, WebControl).Enabled Then
                    If TypeOf filterCtl Is RadTextBox Then
                        DirectCast(filterCtl, RadTextBox).Text = String.Empty

                    ElseIf TypeOf filterCtl Is RadDatePicker Then

                        Dim filterList As RadDatePicker = DirectCast(filterCtl, RadDatePicker)

                        filterList.Clear()

                    ElseIf TypeOf filterCtl Is RadComboBox Then

                        Dim filterList As RadComboBox = DirectCast(filterCtl, RadComboBox)
                        filterList.SelectedIndex = -1
                        filterList.Text = ""
                    End If
                End If
            End If
        Next

        For Each col As GridColumn In rgSupportTickets.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        rgSupportTickets.MasterTableView.FilterExpression = ""

    End Sub

    Public Property openTicketsOnly As Boolean

        Get
            If ViewState("openTicketsOnly") IsNot Nothing Then
                Return Convert.ToBoolean(ViewState("openTicketsOnly"))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("openTicketsOnly") = value
        End Set
    End Property

#End Region

#Region "Private Methods"


    Private Sub FillUsers()

        Dim objMasterUser As IntamacBL_SPR.MasterUser
        Dim dtb As Data.DataTable
        Dim dvUsers As DataView

        cboUserName.Items.Clear()
        If Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
            cboUserName.Items.Add(New RadComboBoxItem(mLoggedInUser.Username, mLoggedInUser.UserID))
            cboUserName.Enabled = False
            cboUserName.SelectedIndex = 0
        Else
            objMasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(mCompanyType)
            dtb = objMasterUser.dtbLoadMasterUsers(mLoggedInUser.MasterCoID, "", "", "", "", 0, "")

            If Not IsNothing(dtb) Then
                dvUsers = dtb.DefaultView

                dvUsers.Sort = "Username"

                dvUsers = dtb.DefaultView

                If dtb.Rows.Count > 0 Then
                    If Not (IsNothing(mUsersCompany) OrElse mUsersCompany.ParentMasterCoID = 0 OrElse IsNothing(dvUsers) OrElse mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.ApplicationOwner)) Then
                        dvUsers.RowFilter = String.Format("[MasterCoID] = {0} OR [ParentMasterCoID] = {0}", mUsersCompany.MasterCoID)

                    End If

                End If
            End If

            cboUserName.DataSource = dvUsers
            cboUserName.DataBind()

        End If

    End Sub

    Private Sub FillStatus()

        Dim objLookup As IntamacBL_SPR.Lookups = IntamacBL_SPR.ObjectManager.CreateLookup(mCompanyType)
        Dim dtb As Data.DataTable = miscFunctions.GetSupportStatuses()

        cboStatus.Items.Clear()

        If dtb IsNot Nothing Then

            Dim dvStatus As DataView = dtb.DefaultView
            dvStatus.RowFilter = "[StatusID] <> 0"

            cboStatus.DataSource = dtb
            cboStatus.DataBind()

        End If
    End Sub

    Private Sub FillSeverity()

        Dim objLookup As IntamacBL_SPR.Lookups = IntamacBL_SPR.ObjectManager.CreateLookup(mCompanyType)
        Dim dtb As Data.DataTable = objLookup.GetSupportSeverity(0)

    End Sub

    Private Sub FillIssueTypes()

        Dim objLookup As IntamacBL_SPR.Lookups = IntamacBL_SPR.ObjectManager.CreateLookup(mCompanyType)
        Dim dtb As Data.DataTable = objLookup.GetSupportIssueType(0)

    End Sub

#End Region

#Region "New Overrides"

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim refPath As String = String.Empty
        If Not IsPostBack Then
            If Not (IsNothing(Request) OrElse IsNothing(Request.UrlReferrer) OrElse String.IsNullOrEmpty(Request.UrlReferrer.LocalPath)) Then
                refPath = Request.UrlReferrer.LocalPath.ToLower()
            End If

            If String.IsNullOrEmpty(mAccountID) Then
                If Not IsTopNavigate Then
                    If Not (String.IsNullOrEmpty(refPath) OrElse refPath.EndsWith("supportdetail.aspx") OrElse refPath.EndsWith("accountsearch.aspx")) Then
                        SafeRedirect("AccountSearch.aspx", True)
                    End If
                End If
            End If

            ' if we hav'nt got here via the 'Top Menu' and an AccountID is set the route must have been from AccountDetail (mAccountID is set), or AccountSearch (via clicking the 'open support tickets' tile)
            If Not (IsTopNavigate) Then
                mIsTopPage = False
                If Not String.IsNullOrEmpty(mAccountID) Then
                    ' navigated from AccountDetail
                    pTopMenu = "Accounts"
                    mBackLocation = "AccountDetail.aspx"
                    mIsAccountPage = True
                Else
                    If Not refPath.EndsWith("supportdetail.aspx") Then
                        openTicketsOnly = True
                    End If
                    mBackLocation = "AccountSearch.aspx"
                End If
            Else ' arrived via top menu so clear active filters
                mAccountID = String.Empty
                mMasterCoID = 0
                mInstallerID = 0
                mDeviceID = String.Empty
            End If
        End If

        MyBase.OnLoad(e)

        If Not (ResponseComplete OrElse IsPostBack) Then

            mSuppReqID = 0

            FillUsers()
            FillStatus()
            FillSeverity()
            FillIssueTypes()


            mSortExpr = "DateEntered"
            mSortDir = "DESC"

            ClearFilters()
            'Clear the fields if accessing from top Nav bar Support tickets button (ticket SP-366)
            If IsTopNavigate Then
                mAccountID = Nothing
                mSelectedGateway = Nothing
                PageClearFilters()
            Else
                If Not String.IsNullOrEmpty(mAccountID) Then
                    txtAccountID.Text = mAccountID
                    txtMACAddress.Text = mSelectedGateway

                    Dim oClient As IntamacBL_SPR.ClientAgent = CType(IntamacBL_SPR.ObjectManager.CreateClientAgent(""), IntamacBL_SPR.ClientAgent)
                    Dim client As ClassLibrary_Interface.iClient = Nothing

                    'load the client object based upon the identified account id
                    client = oClient.Load(mAccountID)
                    If client IsNot Nothing Then
                        txtSurname.Text = client.Surname
                        txtPostCode.Text = client.Postcode
                    End If

                ElseIf refPath.EndsWith("accountsearch.aspx") Then
                    ' navigated by AccountSearch open support tickets
                    cboStatus.SelectedValue = CInt(e_SupportRequestStatus.Open)

                End If
            End If

            rgSupportTickets.MasterTableView.FilterExpression = String.Empty

            rgSupportTickets.DataSource = DoSearch()
            rgSupportTickets.DataBind()

            If Not (IsTopNavigate OrElse String.IsNullOrEmpty(mAccountID)) Then
                mPageTitle = GetLocalResourceObject("AccountSupportLog")
            End If
        Else

            If Session("countryCode") IsNot Nothing Then
                Dim countryCode As String = mCulture.Split("-"c)(1)
                If Session("countryCode") <> countryCode Then
                    FillStatus()
                End If
            End If

        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        resetfilterControls()
        MyBase.OnPreRender(e)
    End Sub
#End Region

#Region "Grid Filter control refs"

    Private ReadOnly Property txtSearchSupportRequestID As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgSupportTickets, "txtSearchSupportRequestID"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchAccountID As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgSupportTickets, "txtSearchAccountID"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchDeviceID As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgSupportTickets, "txtSearchDeviceID"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchLastName As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgSupportTickets, "txtSearchLastName"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchPostCode As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgSupportTickets, "txtSearchPostCode"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchUpdatedBy As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgSupportTickets, "txtSearchUpdatedBy"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property cboSearchStatus As RadComboBox

        Get
            Dim retDDL As RadComboBox = DirectCast(DeepFindControl(rgSupportTickets, "cboSearchStatus"), RadComboBox)

            Return retDDL
        End Get
    End Property

    Private ReadOnly Property txtSearchMacAddress As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgSupportTickets, "txtSearchMacAddress"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property dpSearchSystemTestDate As RadDatePicker
        Get
            Dim retTxt As RadDatePicker = DirectCast(DeepFindControl(rgSupportTickets, "dpSearchSystemTestDate"), RadDatePicker)

            Return retTxt
        End Get
    End Property

#End Region

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        Page.Validate()

        If Page.IsValid Then

            mSortExpr = "DateEntered"
            mSortDir = "DESC"

            rgSupportTickets.MasterTableView.FilterExpression = String.Empty

            rgSupportTickets.DataSource = DoSearch()
            rgSupportTickets.DataBind()

            ClearFilters()

        End If

    End Sub

    Protected Sub btnRaiseTicket_Click(sender As Object, e As EventArgs) Handles btnRaiseTicket.Click
        mSuppReqID = 0
        'mAccountID = Nothing
        Response.Redirect("SupportDetail.aspx")
    End Sub

    Private Sub rgSupportTickets_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles rgSupportTickets.ItemCommand
        Dim gridItem As GridDataItem

        If TypeOf e.Item Is GridDataItem Then
            gridItem = DirectCast(e.Item, GridDataItem)
        End If

        Dim command As String = e.CommandName.ToLower


        Select Case command
            Case "filter"
                'rgSupportTickets.DataSource = DoSearch()
                'rgSupportTickets.DataBind()

            Case "editsupportticket"
                mSuppReqID = gridItem.GetDataKeyValue("SupportRequestID").ToString()
                SafeRedirect("SupportDetail.aspx", True)
        End Select
    End Sub

    'Protected Sub ddlPageSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
    '    dgRequests.PageSize = CInt(ddlPageSize.SelectedValue)
    '    dgRequests.DataSource = DoSearch()
    '    dgRequests.DataBind()

    'End Sub

    Private Sub rgSupportTicketsNeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgSupportTickets.NeedDataSource
        If Not ResponseComplete Then
            rgSupportTickets.DataSource = DoSearch()

        End If
    End Sub



    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        rgSupportTickets.ExportSettings.ExportOnlyData = True
        rgSupportTickets.ExportSettings.OpenInNewWindow = True
        rgSupportTickets.ExportSettings.IgnorePaging = True
        rgSupportTickets.ExportSettings.FileName = "SupportRequest"
        rgSupportTickets.MasterTableView.GetColumn("View").Visible = False
        rgSupportTickets.MasterTableView.ExportToCSV()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFilters()
        PageClearFilters()
        rgSupportTickets.Rebind()
    End Sub

    ''' <summary>
    ''' clear the search filters in whole page
    ''' </summary>
    Private Sub PageClearFilters()
        'Clear the Txtboxes,dropdowns and Dates above the gridview
        txtAccountID.Text = String.Empty
        cboUserName.Text = String.Empty
        cboUserName.SelectedIndex = -1
        cboStatus.SelectedIndex = -1
        cboStatus.Text = String.Empty
        txtRefNo.Text = String.Empty
        txtSurname.Text = String.Empty
        txtDeviceID.Text = String.Empty
        txtPostCode.Text = String.Empty
        txtMACAddress.Text = String.Empty
        dpFromDate.Clear()
        dpToDate.Clear()

        ' ensure grid's own filtering is cleared
        If txtSearchSupportRequestID IsNot Nothing Then
            txtSearchSupportRequestID.Text = String.Empty
        End If
        If txtSearchAccountID IsNot Nothing Then
            txtSearchAccountID.Text = String.Empty
        End If
        If txtSearchDeviceID IsNot Nothing Then
            txtSearchDeviceID.Text = String.Empty
        End If
        If txtSearchLastName IsNot Nothing Then
            txtSearchLastName.Text = String.Empty
        End If
        If txtSearchPostCode IsNot Nothing Then
            txtSearchPostCode.Text = String.Empty
        End If
        If txtSearchUpdatedBy IsNot Nothing Then
            txtSearchUpdatedBy.Text = String.Empty
        End If
        If cboSearchStatus IsNot Nothing Then
            cboSearchStatus.SelectedIndex = -1
            cboSearchStatus.Text = String.Empty
        End If
        If txtSearchMacAddress IsNot Nothing Then
            txtSearchMacAddress.Text = String.Empty
        End If
        If dpSearchSystemTestDate IsNot Nothing Then
            dpSearchSystemTestDate.Clear()
        End If
        If rgSupportTickets.MasterTableView IsNot Nothing Then
            rgSupportTickets.MasterTableView.FilterExpression = String.Empty
        End If

    End Sub

End Class
