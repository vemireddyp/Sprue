Imports IntamacBL_SPR
Imports IntamacShared_SPR.SharedStuff
Imports Telerik.Web.UI

Imports Intamac.IoTDAL

Partial Public Class Admin_AccountSearch
    Inherits CultureBaseClass

#Region "Constants"

    Protected Const cSaveAccountIDKey As String = "IntaSprueAccountID"
    Protected Const cSaveAddress1Key As String = "IntaAddress1"
    Protected Const cSaveAlertsList As String = "AlertsList"
    Protected Const cSaveDistributorKey As String = "Distributor"
    Protected Const cSaveFaultsList As String = "FaultsList"
    Protected Const cSaveFromIDKey As String = "FromID"
    Protected Const cSaveIntaInstallerIDKey As String = "IntaInstallerID"
    Protected Const cSaveIntaMACAddressKey As String = "IntaMACAddress"
    Protected Const cSaveIntaSurnameKey As String = "IntaSurname"
    Protected Const cSaveLastTestDate As String = "LastTestDate"
    Protected Const cSaveLicensedDate As String = "LicensedDate"
    Protected Const cSaveNoAlertsList As String = "NoAlertsList"
    Protected Const cSaveNotTestedList As String = "NotTestedList"
    Protected Const cSaveOpenSupportList As String = "OpenSupportList"
    Protected Const cSavePageIndexKey As String = "PageIndex"
    Protected Const cSavePageSizeKey As String = "PageSize"
    Protected Const cSavePostCodeKey As String = "IntaPostCode"
    Protected Const cSavePropertyStatusIDKey As String = "PropertyStatusID"
    Protected Const cSaveServiceProviderKey As String = "ServiceProvider"
    Protected Const cSavesortDirKey As String = "sortDir"
    Protected Const cSavesortExprKey As String = "sortExpr"
    Protected Const cSaveToIDKey As String = "ToID"


    Protected Const cSessionDetailsKey As String = "AccountSearchDetails"

#End Region

#Region "Properties"
    Private Property mblnLoadDistributors As Boolean
        Get
            If Not IsNothing(ViewState("mblnLoadDistributors")) Then
                Return Convert.ToBoolean(ViewState("mblnLoadDistributors"))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("mblnLoadDistributors") = value
        End Set
    End Property

    Private Property mblnLoadServiceProvs As Boolean
        Get
            If Not IsNothing(ViewState("mblnLoadServiceProvs")) Then
                Return Convert.ToBoolean(ViewState("mblnLoadServiceProvs"))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("mblnLoadServiceProvs") = value
        End Set
    End Property

    Private Property mblnLoadInstallers As Boolean
        Get
            If Not IsNothing(ViewState("mblnLoadInstallers")) Then
                Return Convert.ToBoolean(ViewState("mblnLoadInstallers"))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("mblnLoadInstallers") = value
        End Set
    End Property

    Protected ReadOnly Property mintSystemTestDueDays As Integer
        Get
            If Not IsNothing(ViewState("mintSystemTestDueDays")) Then
                Return Convert.ToInt32(ViewState("mintSystemTestDueDays"))
            Else
                Return 365 ' default ot one year
            End If
        End Get
    End Property

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
                            savedDetails(ctlKey) = rgAccounts.CurrentPageIndex

                        Case cSavePageSizeKey
                            savedDetails(ctlKey) = rgAccounts.PageSize
                    End Select


                End If

            End If

        Next
        SetSavedDetail(cSavesortExprKey, mSortExpr)
        SetSavedDetail(cSavesortDirKey, mSortDir)

    End Sub

    Private Sub restoreSearchDetails()
        resetfilterControls()
        rgAccounts.DataSource = Nothing
        rgAccounts.DataBind()
    End Sub

    Private ReadOnly Property filterDef As Dictionary(Of String, Control)
        Get
            Return New Dictionary(Of String, Control) From {{cSaveAccountIDKey, txtSearchAccountID}, {cSaveAddress1Key, txtSearchAddress1}, {cSaveAlertsList, ucDashboard}, {cSaveDistributorKey, cboDistributors}, {cSaveFaultsList, ucDashboard}, {cSaveFromIDKey, txtFromUnitID},
                                                            {cSaveIntaInstallerIDKey, cboInstallers}, {cSaveIntaMACAddressKey, txtSearchMacAddress}, {cSaveIntaSurnameKey, txtSearchLastName}, {cSaveLastTestDate, dpSystemTestDate}, {cSaveLicensedDate, dpLicensedDate},
                                                            {cSavePageIndexKey, rgAccounts}, {cSavePageSizeKey, rgAccounts}, {cSavePostCodeKey, txtSearchPostCode}, {cSavePropertyStatusIDKey, cboSearchStatus}, {cSaveServiceProviderKey, cboServiceProvs}, {cSaveToIDKey, txtToUnitID}}

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
                                    rgAccounts.CurrentPageIndex = CInt(savedDetails(searchKey))

                                Case cSavePageSizeKey
                                    rgAccounts.PageSize = CInt(savedDetails(searchKey))
                            End Select


                        End If
                    End If

                End If

            Next

        End If
    End Sub
#End Region

#Region "Grid Filter control refs"
    Private ReadOnly Property cboSearchStatus As RadComboBox

        Get
            Dim retDDL As RadComboBox = DirectCast(DeepFindControl(rgAccounts, "cboSearchStatus"), RadComboBox)

            Return retDDL
        End Get
    End Property

    Private ReadOnly Property txtSearchAccountID As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgAccounts, "txtSearchAccountID"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchAddress1 As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgAccounts, "txtSearchAddress1"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchLastName As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgAccounts, "txtSearchLastName"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchMacAddress As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgAccounts, "txtSearchMacAddress"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property txtSearchPostCode As RadTextBox
        Get
            Dim retTxt As RadTextBox = DirectCast(DeepFindControl(rgAccounts, "txtSearchPostCode"), RadTextBox)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property dpSystemTestDate As RadDatePicker
        Get
            Dim retTxt As RadDatePicker = DirectCast(DeepFindControl(rgAccounts, "dpSystemTestDate"), RadDatePicker)

            Return retTxt
        End Get
    End Property

    Private ReadOnly Property dpLicensedDate As RadDatePicker
        Get
            Dim retTxt As RadDatePicker = DirectCast(DeepFindControl(rgAccounts, "dpLicensedDate"), RadDatePicker)

            Return retTxt
        End Get
    End Property

#End Region

#Region "New/Overrides"
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)
        If ajaxManager IsNot Nothing Then
            ajaxManager.AjaxSettings.AddAjaxSetting(btnSearch, rgAccounts, lpGridLoading)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnSearch, btnClear)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, rgAccounts, lpGridLoading)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, apCompanies)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, cboServiceProvs)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, cboInstallers)
            ajaxManager.AjaxSettings.AddAjaxSetting(rgAccounts, rgAccounts, lpGridLoading)
            ajaxManager.AjaxSettings.AddAjaxSetting(ucDashboard, rgAccounts, lpGridLoading)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, cboServiceProvs)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, cboInstallers)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboServiceProvs, cboInstallers)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, ucDashboard)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboServiceProvs, ucDashboard)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboInstallers, ucDashboard)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnSearch, ucDashboard)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, ucDashboard)

        End If

        'the dashboard figures must be at the ALL properties level.
        ucDashboard.Query = IntamacDAL_SPR.DashboardCountsDB.eQueryType.LoadCountsAllProperties

        If Not IsPostBack Then
            mAccountID = ""
            mPropertyID = ""

            InitialiseFilterLists()

            restoreSearchDetails()


        End If


        If Not (IsNothing(mUsersCompany) OrElse (mUsersCompany.CompanyTypeID = IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.ApplicationOwner AndAlso Roles.IsUserInRole(mUsername, "SupportAdmin"))) Then
            divDevRange.Visible = False
        ElseIf ajaxManager IsNot Nothing Then
            ajaxManager.AjaxSettings.AddAjaxSetting(ucDashboard, apUnitRange)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, txtToUnitID)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, txtFromUnitID)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, revFromUnitID)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, revToUnitID)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, cvToID)

        End If

        If Not IsNothing(ConfigurationManager.AppSettings("AccountEnquiryDashboardRefresh")) Then
            ucDashboard.RefreshMilliseconds = CInt(ConfigurationManager.AppSettings("AccountEnquiryDashboardRefresh"))
        End If

        If mAccountDetailTileSelected.HasValue Then
            If mAccountDetailTileSelected = Dashboard.Tiles.NotTested Then setDashboardFilter(Dashboard.Tiles.NotTested)
            mAccountDetailTileSelected = Nothing
        End If


    End Sub

    Protected Overrides Sub OnPreLoad(e As EventArgs)
        MyBase.OnPreLoad(e)
        HackOverridingStrange_cboDistributors()
    End Sub

    Private Sub HackOverridingStrange_cboDistributors()
        '
        ' Hack to override strange behaviour of cboDistributors where the 'Selected....' values stop reflecting those actually chosen client side
        If (Not String.IsNullOrEmpty(cboDistributors.Text) AndAlso (cboDistributors.SelectedItem Is Nothing OrElse cboDistributors.Text <> cboDistributors.SelectedItem.Text)) _
            OrElse (String.IsNullOrEmpty(cboDistributors.Text) AndAlso cboDistributors.SelectedItem IsNot Nothing) Then
            If String.IsNullOrEmpty(cboDistributors.Text) Then
                cboDistributors.ClearSelection()
            Else
                Dim selItem As RadComboBoxItem = cboDistributors.FindItemByText(cboDistributors.Text)

                If selItem IsNot Nothing Then
                    cboDistributors.SelectedIndex = selItem.Index
                Else
                    cboDistributors.ClearSelection()

                End If
            End If
        End If

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        If Not ResponseComplete Then
            If Not IsPostBack Then
                SetDashboardParams()
            End If

            If divDevRange.Visible Then
                ClientScript.RegisterExpandoAttribute(cvToID.ClientID, "fromTxtID", txtFromUnitID.ClientID)
                ClientScript.RegisterExpandoAttribute(cvToID.ClientID, "toTxtID", txtToUnitID.ClientID)
            End If

        Else
            rgAccounts.Visible = False
            ucDashboard.Visible = False
        End If
        MyBase.OnPreRender(e)
    End Sub
#End Region

#Region "Control Event Handlers"
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        rgAccounts.ExportSettings.ExportOnlyData = False
        rgAccounts.ExportSettings.OpenInNewWindow = True
        rgAccounts.ExportSettings.IgnorePaging = True
        rgAccounts.ExportSettings.FileName = "AccountEnquiry"

        rgAccounts.MasterTableView.GetColumn("Edit").Visible = False
        rgAccounts.MasterTableView.GetColumn("Notes").Visible = False
        rgAccounts.MasterTableView.GetColumn("AddTicket").Visible = False
        rgAccounts.MasterTableView.ExportToCSV()

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Page.Validate()

        If Page.IsValid Then
            mDeviceID = Nothing
            mInstallerID = 0
            mMasterCoID = 0
            rgAccounts.DataSource = DoSearch()
            rgAccounts.DataBind()
        End If
    End Sub

    Private Sub filterCombo_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboDistributors.SelectedIndexChanged, cboServiceProvs.SelectedIndexChanged, cboInstallers.SelectedIndexChanged
        ' Save search details on combo change event so counts match results on support request page etc
        saveSearchDetails()

        ListSelectionChanged(sender, e)

    End Sub

    Private Sub unitID_TextChanged(sender As Object, e As EventArgs) Handles txtFromUnitID.TextChanged, txtToUnitID.TextChanged

        If String.IsNullOrEmpty(txtFromUnitID.Text) Then
            Try
                savedDetails.Remove(cSaveFromIDKey)
            Catch ex As Exception

            End Try
        End If

        If String.IsNullOrEmpty(txtToUnitID.Text) Then
            Try
                savedDetails.Remove(cSaveToIDKey)
            Catch ex As Exception

            End Try
        End If

        SetDashboardParams()

    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFilters(False)
        rgAccounts.DataSource = DoSearch()
        rgAccounts.DataBind()

    End Sub

    Private Sub cvToID_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvToID.ServerValidate
        Dim strFromValue As String = txtFromUnitID.Text
        txtFromUnitID.Text.PadLeft(6, "0"c)

        Dim strToValue As String = txtToUnitID.Text
        strToValue.PadLeft(6, "0"c)

        args.IsValid = strToValue >= strFromValue

    End Sub

    Private Sub rgAccounts_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgAccounts.ItemCommand

        Dim gridItem As GridDataItem

        If TypeOf e.Item Is GridDataItem Then
            gridItem = DirectCast(e.Item, GridDataItem)
        End If

        Dim command As String = e.CommandName.ToLower

        Select Case command
            Case "editaccount", "accountnotes", "accounttickets"
                mAccountID = gridItem.GetDataKeyValue("IntaAccountID").ToString()
                If command <> "accounttickets" Then
                    mPropertyID = gridItem.GetDataKeyValue("IntaPropertyID").ToString()
                Else
                    mSuppReqID = 0
                    mPropertyID = String.Empty
                End If

                Select Case command
                    Case "editaccount"
                        SafeRedirect("AccountDetail.aspx", True)
                    Case "accountnotes"
                        SafeRedirect("Notes.aspx", True)
                    Case "accounttickets"
                        SafeRedirect("SupportDetail.aspx", True)
                End Select


        End Select
    End Sub

    Private Sub rgAccounts_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgAccounts.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As DataRowView = DirectCast(e.Item.DataItem, DataRowView)

            If Not IsNothing(dataItem) Then
                Dim gridItem As GridDataItem = e.Item

                Dim lbl As Label = DirectCast(e.Item.FindControl("lblPropertyStatus"), Label)
                Dim status As Integer

                If Not IsDBNull(dataItem(IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.PropertyStatusId))) Then
                    status = CInt(dataItem(IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.PropertyStatusId)))
                Else
                    status = e_AccountPropertyStatus.NotSet
                End If

                If Not IsNothing(lbl) Then
                    lbl.Text = GetGlobalResourceObject("PropertyStatusResources", CType(status, e_AccountPropertyStatus).ToString())
                End If

                If Not IsDBNull(dataItem(IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.MacAddress))) Then
                    lbl = DirectCast(e.Item.FindControl("lblMacAddress"), Label)
                    Dim macValue = Convert.ToString(dataItem(IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.MacAddress)))
                    Dim archivedValue = dataItem(IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.IsArchived))
                    If Not IsDBNull(archivedValue) AndAlso Convert.ToBoolean(archivedValue) Then
                        If Not macValue.ToLower().EndsWith("-a") Then
                            macValue &= "-A"
                        End If
                    End If

                    lbl.Text = macValue

                End If

                If Not IsDBNull(dataItem(IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.LastSystemTestDateTime))) Then
                    lbl = DirectCast(e.Item.FindControl("lblSystemTestDueDate"), Label)
                    If Not IsNothing(lbl) Then
                        lbl.Text = (DateAdd(DateInterval.Day, mintSystemTestDueDays, CDate(dataItem(IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.LastSystemTestDateTime)))).ToString("d"))
                    End If
                End If

                'CAW 20161129 SP1240 - replace text with icons so grid fits on page 
                Dim editBtn As ImageButton = DirectCast(gridItem("Edit").Controls(0), ImageButton)
                editBtn.ToolTip = GetGlobalResourceObject("PageGlobalResources", "EditButtonText")
                editBtn.AlternateText = GetGlobalResourceObject("PageGlobalResources", "EditButtonText")

                Dim notesBtn As ImageButton = DirectCast(gridItem("Notes").Controls(0), ImageButton)
                notesBtn.ToolTip = GetGlobalResourceObject("PageGlobalResources", "NotesButton")
                notesBtn.AlternateText = GetGlobalResourceObject("PageGlobalResources", "NotesButton")

                Dim ticketBtn As ImageButton = DirectCast(gridItem("AddTicket").Controls(0), ImageButton)
                ticketBtn.ToolTip = GetGlobalResourceObject("PageGlobalResources", "AddTicketButtonText")
                ticketBtn.AlternateText = GetGlobalResourceObject("PageGlobalResources", "AddTicketButtonText")

            End If

        End If

    End Sub

    Private Sub rgAccounts_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgAccounts.NeedDataSource
        rgAccounts.DataSource = DoSearch()


    End Sub


    Private Sub rgAccounts_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles rgAccounts.PageSizeChanged
        rgAccounts.Rebind()
    End Sub

    Private Sub rgAccounts_PreRender(sender As Object, e As EventArgs) Handles rgAccounts.PreRender
        resetfilterControls()

    End Sub

    Private Sub ucDashboard_TileClick(sender As Object, e As CommandEventArgs) Handles ucDashboard.TileClick
        'ClearFilters(True)
        mDeviceID = Nothing
        Select Case e.CommandName.ToLowerInvariant()
            Case "supporttickets"

                'Check if unit ID range search is being used
                If Not (savedDetails(cSaveFromIDKey) Is Nothing AndAlso savedDetails(cSaveToIDKey) Is Nothing) Then
                    mDeviceID = String.Format("{0}<{1}", savedDetails(cSaveFromIDKey), savedDetails(cSaveToIDKey))
                End If

                ' If installer ID selected, setup
                If (Not String.IsNullOrEmpty(savedDetails(cSaveIntaInstallerIDKey))) Then
                    mInstallerID = CInt(savedDetails(cSaveIntaInstallerIDKey))
                End If

                If mInstallerID = 0 Then
                    mMasterCoID = 0
                    If Not String.IsNullOrEmpty(savedDetails(cSaveServiceProviderKey)) Then
                        mMasterCoID = CInt(savedDetails(cSaveServiceProviderKey))
                    End If

                    If mMasterCoID = 0 Then
                        If Not String.IsNullOrEmpty(savedDetails(cSaveDistributorKey)) Then
                            mMasterCoID = CInt(savedDetails(cSaveDistributorKey))
                        End If
                    End If
                End If

                SafeRedirect("SupportSearch.aspx", True)
            Case Else
                setDashboardFilter([Enum].Parse(GetType(Dashboard.Tiles), e.CommandName))
        End Select

        If Not ResponseComplete Then
            rgAccounts.Rebind()

        End If
    End Sub
#End Region

    Private Sub ListSelectionChanged(sender As Object, e As EventArgs)

        If sender IsNot cboInstallers Then
            If String.IsNullOrEmpty(cboDistributors.SelectedValue) Then
                Try
                    savedDetails.Remove(cSaveDistributorKey)
                Catch ex As Exception

                End Try
            End If

            If String.IsNullOrEmpty(cboServiceProvs.SelectedValue) Then
                Try
                    savedDetails.Remove(cSaveServiceProviderKey)
                Catch ex As Exception

                End Try
            End If

            If String.IsNullOrEmpty(cboInstallers.SelectedValue) Then
                Try
                    savedDetails.Remove(cSaveIntaInstallerIDKey)
                Catch ex As Exception

                End Try

            End If

            cboInstallers.Items.Clear()
            If sender Is cboDistributors Then
                LoadCompanylists()
            ElseIf sender Is cboServiceProvs Then
                LoadInstallersList()
            End If

        Else
            If String.IsNullOrEmpty(cboInstallers.SelectedValue) Then
                Try
                    savedDetails.Remove(cSaveIntaInstallerIDKey)
                Catch ex As Exception

                End Try

            End If


        End If

        SetDashboardParams()

    End Sub

    Private Sub setDashboardFilter(ByVal filterToSet As Dashboard.Tiles)

        Dim saveKey As String

        For Each tile As Dashboard.Tiles In [Enum].GetValues(GetType(Dashboard.Tiles))
            Select Case tile
                Case Dashboard.Tiles.ActiveAlerts
                    saveKey = cSaveAlertsList

                Case Dashboard.Tiles.ActiveFaults
                    saveKey = cSaveFaultsList

                Case Dashboard.Tiles.NoFaults
                    saveKey = cSaveNoAlertsList

                Case Dashboard.Tiles.NotTested
                    saveKey = cSaveNotTestedList

                Case Dashboard.Tiles.SupportTickets
                    saveKey = cSaveOpenSupportList

                Case Else
                    ' effectively 'TotalDevices' i.e. remove 'special' filters
                    saveKey = ""
            End Select

            If tile = filterToSet AndAlso saveKey <> "" Then
                SetSavedDetail(saveKey, "true")
            Else
                If Not String.IsNullOrEmpty(savedDetails(saveKey)) Then
                    savedDetails.Remove(saveKey)
                End If
            End If



        Next

    End Sub

    Private Function DoSearch() As Data.DataView

        Dim objClient As IntamacBL_SPR.ClientAgent = CType(IntamacBL_SPR.ObjectManager.CreateClientAgent(""), IntamacBL_SPR.ClientAgent)
        Dim dv As Data.DataView
        Dim dtb As Data.DataTable

        saveSearchDetails()

        Dim propStatus As Nullable(Of Integer) = Nothing

        Dim installerID As Integer = 0
        If Not String.IsNullOrEmpty(savedDetails(cSaveIntaInstallerIDKey)) Then
            installerID = CInt(savedDetails(cSaveIntaInstallerIDKey))
        End If

        Dim companyID As Integer = 0

        If installerID = 0 Then

            If Not String.IsNullOrEmpty(savedDetails(cSaveServiceProviderKey)) Then
                companyID = CInt(savedDetails(cSaveServiceProviderKey))
            End If

            If companyID = 0 Then
                If Not String.IsNullOrEmpty(savedDetails(cSaveDistributorKey)) Then
                    companyID = CInt(savedDetails(cSaveDistributorKey))
                End If
            End If

        End If

        Dim blnAlertsList As Nullable(Of Boolean)
        Dim blnFaultsList As Nullable(Of Boolean)
        Dim blnNoFaultsList As Nullable(Of Boolean)
        Dim blnNotTestedList As Nullable(Of Boolean)
        Dim blnOpenSupportList As Nullable(Of Boolean)

        If Not String.IsNullOrEmpty(savedDetails(cSaveAlertsList)) Then
            blnAlertsList = Convert.ToBoolean(savedDetails(cSaveAlertsList))
        ElseIf Not String.IsNullOrEmpty(savedDetails(cSaveFaultsList)) Then
            blnFaultsList = Convert.ToBoolean(savedDetails(cSaveFaultsList))
        ElseIf Not String.IsNullOrEmpty(savedDetails(cSaveNoAlertsList)) Then
            blnNoFaultsList = Convert.ToBoolean(savedDetails(cSaveNoAlertsList))
        ElseIf Not String.IsNullOrEmpty(savedDetails(cSaveNotTestedList)) Then
            blnNotTestedList = Convert.ToBoolean(savedDetails(cSaveNotTestedList))
        ElseIf Not String.IsNullOrEmpty(savedDetails(cSaveOpenSupportList)) Then
            blnOpenSupportList = Convert.ToBoolean(savedDetails(cSaveOpenSupportList))
        End If

        ' if any of the dashboard tiles was clicked we are only interested in 'active' properties
        If blnAlertsList OrElse blnFaultsList OrElse blnNoFaultsList OrElse blnNotTestedList Then
            SetSavedDetail(cSavePropertyStatusIDKey, CInt(e_AccountPropertyStatus.Active))
        End If

        dtb = objClient.LoadSearch(savedDetails(cSaveAccountIDKey), savedDetails(cSaveAddress1Key), savedDetails(cSavePostCodeKey), savedDetails(cSaveIntaSurnameKey), savedDetails(cSaveIntaMACAddressKey),
                                   savedDetails(cSavePropertyStatusIDKey), companyID, installerID, Nothing, Nothing, Nothing, Nothing, savedDetails(cSaveFromIDKey), savedDetails(cSaveToIDKey),
                                   blnAlertsList, blnFaultsList, blnNoFaultsList, blnNotTestedList, blnOpenSupportList)

        dv = dtb.DefaultView

        Dim sortExpression = $"{IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.AccountId)} ASC, {IoTDALUtils.GetIntaColumnName(IoTFieldsConstants.PropertyID)} ASC"
        dv.Sort = sortExpression

        Return dv


    End Function

    Private Sub LoadCompanylists()
        If mblnLoadDistributors Or mblnLoadServiceProvs Then
            Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)

            Dim intSelectedCo As Integer = 0

            '
            '  either we're loading the lists on a GET, then distributor may be fixed (mblnLoadDistributors is false), or in response to a change in selection on the distributors droplist
            If (IsPostBack Or Not mblnLoadDistributors) AndAlso cboDistributors.Items.Count > 0 Then

                If Not String.IsNullOrEmpty(cboDistributors.SelectedValue) Then
                    intSelectedCo = CInt(cboDistributors.SelectedValue)

                End If
                ' in either circumstance the service providers list will be rebuilt, if we're doing that
                If mblnLoadServiceProvs Then
                    cboServiceProvs.Items.Clear()
                    cboServiceProvs.Text = ""
                End If
            End If

            If intSelectedCo = 0 Then
                ' id of distributor not, already, selected
                cboDistributors.Items.Clear()
                cboDistributors.Text = ""
            End If


            Dim data As DataTable = masterCo.LoadSearch("", 0, intSelectedCo, 0)

            For Each row As DataRow In data.Rows
                If mblnLoadDistributors AndAlso row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor) AndAlso CInt(row("MasterCoID")) <> intSelectedCo Then
                    cboDistributors.Items.Add(New RadComboBoxItem(row("Name"), CStr(row("MasterCoID"))))
                ElseIf mblnLoadServiceProvs AndAlso row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany) Then
                    cboServiceProvs.Items.Add(New RadComboBoxItem(row("Name"), CStr(row("MasterCoID"))))

                End If
            Next

        End If

        ' droplists only enabled if more than one choice available
        If cboDistributors.Items.Count > 1 Then
            cboDistributors.Enabled = True
            Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)
            If ajaxManager IsNot Nothing Then
                ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, cboDistributors)
            End If
        Else
            cboDistributors.Enabled = False
            If cboDistributors.Items.Count > 0 Then
                cboDistributors.SelectedIndex = 0
            End If
        End If

        If cboServiceProvs.Items.Count > 1 Then
            cboServiceProvs.Enabled = True
        Else
            If mUsersCompany.CompanyTypeID <> IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.ApplicationOwner AndAlso mUsersCompany.CompanyTypeID <> IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor Then
                cboServiceProvs.Enabled = False
                If cboServiceProvs.Items.Count > 0 Then
                    cboServiceProvs.SelectedIndex = 0
                End If
            End If
        End If

        If mblnLoadInstallers Then
            LoadInstallersList()
        End If


    End Sub

    Private Sub LoadInstallersList()
        Dim masterCoId As Integer = 0

        If Not String.IsNullOrEmpty(cboServiceProvs.SelectedValue) Then
            masterCoId = CInt(cboServiceProvs.SelectedValue)
        End If

        If masterCoId = 0 AndAlso Not String.IsNullOrEmpty(cboDistributors.SelectedValue) AndAlso cboDistributors.SelectedValue <> "0" Then
            masterCoId = CInt(cboDistributors.SelectedValue)
        End If

        Dim masterUsers As IntamacBL_SPR.MasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)

        cboInstallers.Items.Clear()
        cboInstallers.Text = ""

        Dim dtbInstallerUsers As DataTable = masterUsers.dtbLoadMasterUsers(masterCoId, "", "", "", "", 0, "installer")

        'cboInstallers.Items.Add(New radcomboboxitem(GetGlobalResourceObject("PageGlobalResources", "AllInstallersText"), "0"))

        If Not IsNothing(dtbInstallerUsers) Then
            For Each userRow As DataRow In dtbInstallerUsers.Rows
                cboInstallers.Items.Add(New RadComboBoxItem(String.Format("{0} {1}", userRow("FirstName"), userRow("LastName")), userRow("UserID").ToString()))
            Next
        End If
        If cboInstallers.Items.Count > 0 Then
            cboInstallers.Enabled = True
        Else
            cboInstallers.Enabled = False
        End If

    End Sub

    Private Sub InitialiseFilterLists()

        Dim usersCoType As IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes = IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany
        If Not IsNothing(mUsersCompany) Then
            '
            ' get the user's company type Intamac, Sprue, Distributor or Provider
            usersCoType = CType(mUsersCompany.CompanyTypeID, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes)

        End If

        '
        ' initialise member vars, saved details and list controls
        mblnLoadDistributors = False
        mblnLoadServiceProvs = False
        mblnLoadInstallers = False

        cboDistributors.Items.Clear()
        cboDistributors.Text = ""
        cboServiceProvs.Items.Clear()
        cboServiceProvs.Text = ""
        cboInstallers.Items.Clear()
        cboInstallers.Text = ""
        txtFromUnitID.Text = ""
        txtToUnitID.Text = ""

        If Not String.IsNullOrEmpty(savedDetails("IntaParentCompanyID")) Then
            savedDetails.Remove("IntaParentCompanyID")
        End If

        '
        '  set lists to be displayed, according to user's company type (need to add user type handling)
        Select Case usersCoType

            Case IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.SystemOwner, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.ApplicationOwner
                ' load distributors and  list
                mblnLoadDistributors = True
                mblnLoadServiceProvs = True
                mblnLoadInstallers = True

            Case Else
                Select Case usersCoType
                    Case IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor
                        mblnLoadServiceProvs = True
                        mblnLoadInstallers = True

                    Case Else
                        If Not IsNothing(mLoggedInUser) Then
                            If Not Roles.IsUserInRole(mLoggedInUser.Username, miscFunctions.rolenames.Installer.ToString) Then
                                mblnLoadInstallers = True
                            End If

                        End If
                End Select

        End Select

        If Not IsNothing(mUsersCompany) Then
            Dim strItemLegend = PageString("FilterNotAvailableLegend")

            If Not mblnLoadDistributors Then

                Dim distributorID As Integer = 0
                If mUsersCompany.CompanyTypeID = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor) Then
                    strItemLegend = mUsersCompany.Name
                    distributorID = mUsersCompany.MasterCoID
                Else
                    'SP-977 Distributor name Is showing based on loggedIn user
                    Dim masterDistributor As New MasterDistributor
                    Dim distributor = masterDistributor.Load(mLoggedInUser.UserID)
                    If distributor IsNot Nothing Then
                        strItemLegend = distributor.Name
                        distributorID = distributor.Id
                    End If
                End If
                cboDistributors.Items.Add(New RadComboBoxItem(strItemLegend, CStr(IIf(mblnLoadServiceProvs, distributorID, 0))))
                cboDistributors.SelectedIndex = 0
            End If

            If Not mblnLoadServiceProvs Then
                If mUsersCompany.CompanyTypeID = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany) Then
                    strItemLegend = mUsersCompany.Name
                End If

                cboServiceProvs.Items.Add(New RadComboBoxItem(strItemLegend, CStr(IIf(mblnLoadInstallers, mUsersCompany.MasterCoID, 0))))
            End If

            If Not mblnLoadInstallers Then
                strItemLegend = mLoggedInUser.Firstname + " " + mLoggedInUser.Lastname
                cboInstallers.Items.Add(New RadComboBoxItem(strItemLegend, CStr(mLoggedInUser.UserID)))
                cboInstallers.SelectedIndex = 0
                cboInstallers.Enabled = False
            End If
        End If

        LoadCompanylists()

    End Sub

    Private Sub ClearFilters(ByVal v_blnJustGrid As Boolean)

        mDeviceID = Nothing

        Dim filtersToClear As New List(Of Control) _
        ({
            txtToUnitID, txtFromUnitID, txtSearchAccountID, txtSearchLastName,
            txtSearchMacAddress, txtSearchPostCode, cboSearchStatus,
            txtSearchAddress1, dpLicensedDate, dpSystemTestDate,
            txtFromUnitID, txtToUnitID
         })

        If Not v_blnJustGrid Then
            If Not Roles.IsUserInRole("installer") Then
                Select Case mUsersCompany.CompanyTypeID
                    Case CInt(e_MasterCompanyTypes.OperatingCompany)
                        filtersToClear.AddRange(New List(Of Control)({cboInstallers}))

                    Case CInt(e_MasterCompanyTypes.Distributor)
                        filtersToClear.AddRange(New List(Of Control)({cboServiceProvs, cboInstallers}))

                    Case Else
                        filtersToClear.AddRange(New List(Of Control)({cboDistributors, cboServiceProvs, cboInstallers}))
                        SetSavedDetail(cSaveToIDKey, Nothing)
                        SetSavedDetail(cSaveFromIDKey, Nothing)
                End Select
            End If
        End If

        savedDetails.Clear()

        If Not v_blnJustGrid Then
            InitialiseFilterLists()
        End If

        For Each filterCtl As Control In filtersToClear

            If Not IsNothing(filterCtl) Then

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
        Next

        For Each col As GridColumn In rgAccounts.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        rgAccounts.MasterTableView.FilterExpression = ""

        If Not v_blnJustGrid Then
            SetDashboardParams()
        End If

    End Sub

    Private Sub SetDashboardParams()

        If Not IsNothing(ucDashboard) Then
            ucDashboard.InstallerID = 0
            ucDashboard.ParentCoID = 0

            If Not IsNothing(cboInstallers) AndAlso Not String.IsNullOrEmpty(cboInstallers.SelectedValue) Then
                ucDashboard.InstallerID = cboInstallers.SelectedValue

            End If

            If Not IsNothing(cboServiceProvs) AndAlso Not String.IsNullOrEmpty(cboServiceProvs.SelectedValue) Then
                ucDashboard.ParentCoID = cboServiceProvs.SelectedValue

            End If

            If ucDashboard.ParentCoID = 0 Then
                If Not IsNothing(cboDistributors) AndAlso Not String.IsNullOrEmpty(cboDistributors.SelectedValue) Then
                    ucDashboard.ParentCoID = cboDistributors.SelectedValue
                End If
            End If

            If Not String.IsNullOrEmpty(txtFromUnitID.Text) Then
                ucDashboard.FromSensorID = txtFromUnitID.Text
            Else
                ucDashboard.FromSensorID = ""
            End If

            If Not String.IsNullOrEmpty(txtToUnitID.Text) Then
                ucDashboard.ToSensorID = txtToUnitID.Text
            Else
                ucDashboard.ToSensorID = ""
            End If

        End If

    End Sub

End Class
