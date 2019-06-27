Imports IntamacShared_SPR.SharedStuff
Imports IntamacShared_SPR.EventLogsUISharedKeys
Imports Telerik.Web.UI

Partial Public Class Admin_EventLog
    Inherits GridPage

    Private Const gatewaySensorType As Long = 65535

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

#End Region
    Public Overrides Function getFiltersDef() As Dictionary(Of String, Control)
        Return filterDef

    End Function
#Region "Preserve Search Details"

    Public ReadOnly Property filterDef As Dictionary(Of String, Control)
        Get
            Return New Dictionary(Of String, Control) From {{cSaveDistributorKey, DeepFindControl(Me, "cboDistributors")}, {cSaveIntaInstallerIDKey, DeepFindControl(Me, "cboInstallers")},
                                                            {cSaveSearchAccountID, getGridFilterControlByID(Of RadTextBox)("txtSearchAccountID")},
                                                            {cSaveSearchAreaName, getGridFilterControlByID(Of RadTextBox)("txtSearchAreaName")}, {cSaveSearchCreatedDate, dpSearchCreatedDate},
                                                            {cSaveSearchCreatedTime, getGridFilterControlByID(Of RadTimePicker)("tpSearchCreatedTime")}, {cSaveSearchPropZoneDesc, getGridFilterControlByID(Of RadTextBox)("txtSearchPropZoneDesc")},
                                                            {cSaveSearchRoomName, getGridFilterControlByID(Of RadTextBox)("txtSearchRoomName")}, {cSaveSearchSensorDesc, getGridFilterControlByID(Of RadComboBox)("cboSearchSensorDesc")},
                                                            {cSaveSearchDeviceID, getGridFilterControlByID(Of RadTextBox)("txtSearchDeviceID")}, {cSaveSearchTemp, getGridFilterControlByID(Of RadTextBox)("txtSearchTempID")},
                                                            {cSaveSearchSIAKey, getGridFilterControlByID(Of RadComboBox)("cboSearchSIA")}, {cSaveServiceProviderKey, DeepFindControl(Me, "cboServiceProvs")}}

        End Get
    End Property

#End Region

#Region "Grid Filter control refs"
    Private ReadOnly Property dpSearchCreatedDate As RadDatePicker
        Get
            Return getGridFilterControlByID(Of RadDatePicker)("dpSearchCreatedDate")
        End Get
    End Property
#End Region

#Region "New/Overrides"
    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)

        If Not ajaxManager Is Nothing Then
            ajaxManager.AjaxSettings.AddAjaxSetting(btnSearch, rgAccounts, lpGridLoading)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, rgAccounts, lpGridLoading)
            ajaxManager.AjaxSettings.AddAjaxSetting(tmrEventLogGrid, rgAccounts)
        End If

        If Not IsPostBack Then
            If IsTopNavigate OrElse Not ((String.IsNullOrEmpty(mAccountID) AndAlso String.IsNullOrEmpty(mPropertyID)) OrElse ((Not IsNothing(mBackLocation) AndAlso (mBackLocation.ToLower().Contains("accountdetail.aspx") OrElse mBackLocation.ToLower().Contains("managesensors"))) _
                                                                                                         OrElse (Not (IsNothing(Request) OrElse IsNothing(Request.UrlReferrer) OrElse IsNothing(Request.UrlReferrer.AbsolutePath)) _
                                                                                                                 AndAlso (Request.UrlReferrer.AbsolutePath.ToLower.Contains("accountdetail.aspx") OrElse Request.UrlReferrer.AbsolutePath.ToLower.Contains("managesensors"))))) Then
                mAccountID = ""
                mPropertyID = ""
                ClearFilters(False)
            ElseIf Request.QueryString("clearfilters") IsNot Nothing Then
                ClearFilters(False)
            End If

            If Not String.IsNullOrEmpty(mAccountID) Then
                mIsAccountPage = True
                mIsTopPage = False
                pTopMenu = "Accounts"
                rgAccounts.MasterTableView.GetColumn("IntaAccountID").Display = False
            Else
                mIsAccountPage = False
                rgAccounts.MasterTableView.GetColumn("IntaAccountID").Display = True

            End If
            'SP-791  event log page title text display 
            If Not IsTopNavigate Then
                mPageTitle = GetLocalResourceObject("AccountEventLog")
            End If
        End If
        MyBase.OnLoad(e)

        'Dim ajaxManager As RadAjaxManager = DirectCast(DeepFindControl(Master, "RadAjaxManager1"), RadAjaxManager)
        'ajaxManager.AjaxSettings.AddAjaxSetting(rgAccounts, rgAccounts, lpGridLoading)

        If Not IsPostBack Then
            'Go back to Account Search when there is no mAccountID or mPropertyID which can get cleared when using browser back button, 
            'i.e. avoid showing all devices for all properties on this page
            If String.IsNullOrEmpty(mAccountID) Or String.IsNullOrEmpty(mPropertyID) Then
                If Not IsTopNavigate Then
                    Response.Redirect("AccountSearch.aspx")
                End If
            End If

            If Not String.IsNullOrEmpty(mAccountID) Then
                divCompaniesFilter.Visible = False
                Dim clientGetter As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")

                Dim objClient As IntamacBL_SPR.Client = clientGetter.Load(mAccountID)

                lblAccountHolder.Text = objClient.Title & " " &
                                        objClient.FirstName & " " &
                                        objClient.Surname

                lblAccountID.Text = objClient.AccountID
                divAccountDets.Visible = True
                divPropertyAddress.Visible = False
                divMACAddress.Visible = False

                If Not String.IsNullOrEmpty(mPropertyID) Then
                    Dim prop As ClassLibrary_Interface.iProperty = Nothing
                    Dim propertyGetter As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
                    prop = propertyGetter.Load(mAccountID, mPropertyID)

                    'initialise the property text by setting it to an empty string
                    lblPropertyAddress1.Text = ""
                    lblPropertyAddress2.Text = ""
                    lblPostCode.Text = ""

                    'check that the property has been identified
                    If prop IsNot Nothing Then

                        'display the property address on the page
                        If Not String.IsNullOrEmpty(prop.Address1) Then
                            lblPropertyAddress1.Text = prop.Address1.Trim()
                        End If

                        If Not String.IsNullOrEmpty(prop.Address2) Then
                            lblPropertyAddress2.Text = prop.Address2.Trim()
                        End If

                        If Not String.IsNullOrEmpty(prop.Postcode) Then
                            lblPostCode.Text = prop.Postcode.Trim()
                        End If

                        divPropertyAddress.Visible = True
                    End If

                End If
            Else
                divCompaniesFilter.Visible = True
                divAccountDets.Visible = False
                divPropertyAddress.Visible = False
            End If

            InitialiseFilterLists()

            restoreSearchDetails("")

        End If


    End Sub

    ''' <summary>
    ''' Raises the PreRender event.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnPreRender(e As EventArgs)
        Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)
        If Not ajaxManager Is Nothing Then
            ' set up the Ajax response settings that involve the 'filter' combos that are visible
            If cboDistributors.Visible Then
                ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, rgAccounts, lpGridLoading)
                ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, cboServiceProvs)
                ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, cboInstallers)
                ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, cboDistributors)
            End If

            If cboServiceProvs.Visible Then
                ajaxManager.AjaxSettings.AddAjaxSetting(cboServiceProvs, rgAccounts, lpGridLoading)
                ajaxManager.AjaxSettings.AddAjaxSetting(cboServiceProvs, cboInstallers)
                ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, cboServiceProvs)
            End If

            If cboInstallers.Visible Then
                ajaxManager.AjaxSettings.AddAjaxSetting(cboInstallers, rgAccounts, lpGridLoading)
                ajaxManager.AjaxSettings.AddAjaxSetting(btnClear, cboInstallers)
            End If
        End If

        ' call base method (to raise event)
        MyBase.OnPreRender(e)

        ' add response script to ensure the refresh timer is enabled (may have been disabled client side)
        ajaxManager.ResponseScripts.Add(String.Format("setTimerEnabled('{0}',true)", tmrEventLogGrid.ClientID))

    End Sub
#End Region

#Region "Control Event Handlers"

    Private Sub rgAccounts_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgAccounts.ItemCommand
        If e.CommandName.ToLower = "filter" Then
            rgAccounts.DataSource = DoSearch()
            rgAccounts.DataBind()

        End If
    End Sub

    Private Sub rgAccounts_ItemCreated(sender As Object, e As GridItemEventArgs) Handles rgAccounts.ItemCreated

        If TypeOf e.Item Is GridFilteringItem Then
            Dim combo As RadComboBox = getGridFilterControlBySaveID(Of RadComboBox)(cSaveSearchSIAKey)

            If Not IsNothing(combo) Then

                'load all SIA codes that are currently valid for either sensors or gateways
                'Dim siaBL As New IntamacBL_SPR.SIACode()
                'Dim lst As List(Of IntamacBL_SPR.SIACode) = siaBL.Load(Nothing, mCulture, True)
                'Dim query As IEnumerable(Of IntamacBL_SPR.SIACode) = lst.OrderBy(Function(x) x.SIADesc)
                '                combo.DataSource = query
                combo.DataSource = GetCollatedSIADescriptions()
                combo.DataBind()

            End If

            combo = getGridFilterControlBySaveID(Of RadComboBox)(cSaveSearchSensorDesc)

            If Not IsNothing(combo) Then
                For Each sensorType As e_AlarmSensorTypes In [Enum].GetValues(GetType(e_AlarmSensorTypes))
                    Dim translatedString As String = DeviceTypeGet(sensorType)
                    If String.IsNullOrEmpty(translatedString) Then translatedString = sensorType.ToString()
                    combo.Items.Add(New RadComboBoxItem(translatedString, CInt(sensorType).ToString))
                Next
            End If
            combo.Items.Add(New RadComboBoxItem(GetLocalResourceObject("GatewayDeviceType"), gatewaySensorType.ToString))

        End If
    End Sub

    ''' <summary>
    ''' Gets a list of, unique, SIA Descriptions
    ''' </summary>
    ''' <returns></returns>
    Private Function GetCollatedSIADescriptions() As IEnumerable(Of String)
        Dim retList As New List(Of String)

        Dim siaBL As New IntamacBL_SPR.SIACode()
        Dim lst As List(Of IntamacBL_SPR.SIACode) = siaBL.Load(Nothing, mCulture, True)

        For Each entry As IntamacBL_SPR.SIACode In lst
            If Not retList.Contains(entry.SIADesc) Then
                retList.Add(entry.SIADesc)
            End If
        Next
        retList.Sort()
        Return retList
    End Function
    Private Sub ListSelectionChanged(sender As Object, e As EventArgs)

        If String.IsNullOrEmpty(cboDistributors.SelectedValue) Then
            Try
                savedDetails.Remove(cSaveDistributorKey)
            Catch ex As Exception

            End Try
        End If

        If Not String.IsNullOrEmpty(cboServiceProvs.SelectedValue) Then
            Try
                savedDetails.Remove(cSaveServiceProviderKey)
            Catch ex As Exception

            End Try
        End If

        If Not String.IsNullOrEmpty(cboInstallers.SelectedValue) Then
            Try
                savedDetails.Remove(cSaveIntaInstallerIDKey)
            Catch ex As Exception

            End Try

        End If

        If Not sender Is cboInstallers Then
            cboInstallers.Items.Clear()
        End If

        If sender Is cboDistributors Then
            LoadCompanylists()
        ElseIf sender Is cboServiceProvs Then
            LoadInstallersList()
        End If

        ' clear the grid's 'header' filters
        ClearFilters(True)

        rgAccounts.DataSource = DoSearch()
        rgAccounts.DataBind()

    End Sub

#End Region

    ''' <summary>
    ''' Gets event history data from RESS, applying requested filtering
    ''' </summary>
    ''' <param name="gridId"></param>
    ''' <returns></returns>
    Public Overrides Function DoSearch(ByVal gridId As String) As Data.DataView

        ' save filter control values
        saveSearchDetails()

        Dim addParams As New List(Of KeyValuePair(Of String, Object))

        If (Not Page.IsPostBack) AndAlso mDeviceID IsNot Nothing AndAlso mBackLocation IsNot Nothing AndAlso mBackLocation.ToLower.Contains("managesensors") Then
            SetSavedDetail(cSaveSearchDeviceID, mDeviceID)
            Dim DeviceIDCol As GridColumn = rgAccounts.MasterTableView.GetColumnSafe("IntaDeviceID")

            If DeviceIDCol IsNot Nothing Then
                DeviceIDCol.CurrentFilterValue = mDeviceID
                DeviceIDCol.CurrentFilterFunction = GridKnownFunction.StartsWith
            End If

            rgAccounts.MasterTableView.FilterExpression = String.Format("it[""{0}""].ToString().StartsWith(""{1}"")", "IntaDeviceID", mDeviceID)

            addParams.Add(new KeyValuePair(Of String, Object)(cSaveSearchDeviceID, mDeviceID))
        End If

        Dim objEvents As New IntamacDAL_SPR.EventLogsDB()
        Dim dv As Data.DataView
        Dim dtb As Data.DataTable


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
        Else

        End If

        Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)
        Dim createdDate As Nullable(Of Date)

        If Not String.IsNullOrEmpty(savedDetails(cSaveSearchCreatedDate)) Then
            Dim dateString As String = savedDetails(cSaveSearchCreatedDate)

            createdDate = New Date(CInt(dateString.Substring(0, 4)), CInt(dateString.Substring(4, 2)), CInt(dateString.Substring(6, 2)))
            'createdDate = masterCo.ConvertDateTimeToTableTime(createdDate, mLoggedInUser.MasterCoID)
        End If

        Dim timeFrom As Nullable(Of TimeSpan)
        Dim timeTo As Nullable(Of TimeSpan)


        If Not String.IsNullOrEmpty(savedDetails(cSaveSearchCreatedTime)) Then
            Dim timeString As String = savedDetails(cSaveSearchCreatedTime)
            timeFrom = New TimeSpan(CInt(timeString.Substring(0, 2)), CInt(timeString.Substring(2)), 0)

            timeTo = timeFrom.Value.Add(New TimeSpan(0, 15, 0))

        End If

        If Not String.IsNullOrEmpty(savedDetails(cSaveSearchPropZoneDesc)) Then

            If Not PageString("Gateway").ToLowerInvariant().StartsWith(savedDetails(cSaveSearchPropZoneDesc).Trim().ToLowerInvariant()) Then
                Dim paramValue As List(Of Object) = SensorInFilterGet(savedDetails(cSaveSearchPropZoneDesc))

                If paramValue.Count < 1 Then
                    paramValue.Add(-1)
                End If

                addParams.Add(New KeyValuePair(Of String, Object)(cSaveSearchPropZoneDesc, paramValue))
            Else
                'avoid using prop zone desc on filter of gateway device type 
                'addParams.Add(New KeyValuePair(Of String, Object)(cSaveSearchPropZoneDesc, Nothing))
            End If
        End If

        For Each key As String In New String() {cSaveSearchDeviceID, cSaveSearchRoomName, cSaveSearchAreaName}
            If Not String.IsNullOrEmpty(savedDetails(key)) Then
                addParams.Add(New KeyValuePair(Of String, Object)(key, savedDetails(key)))
            End If
        Next

        dtb = objEvents.LoadForAdmin(0, mAccountID, mPropertyID, createdDate, timeFrom, timeTo, mCulture, companyID, savedDetails(cSaveIntaInstallerIDKey), mLoggedInUser.MasterCoID,, addParams)

        'convert date time to master company's time zone of logged in user
        'dtb = masterCo.ConvertDataTableTimeData(dtb, mLoggedInUser.MasterCoID)

        'add column to store and allow filter on device name based on resource file values
        dtb.Columns.Add("DeviceName")

        For Each r As DataRow In dtb.Rows

            Dim deviceName As String = String.Empty

            If Not IsDBNull(r("IntaSensorType")) AndAlso r("IntaSensorType") <> gatewaySensorType Then
                deviceName = SensorTypeGet(r("IntaSensorType"))
            Else
                'if sensor type is 2 then get gateway resource name
                deviceName = GetLocalResourceObject("Gateway")
            End If

            r("DeviceName") = deviceName
        Next

        dtb.DefaultView.Sort = "LocalEventDateTime DESC"

        dv = dtb.DefaultView

        ' apply temperature filter, if required
        If Not String.IsNullOrEmpty(savedDetails(cSaveSearchTemp)) Then
            dv.RowFilter = String.Format("IntaTemperaturePPM = {0}", savedDetails(cSaveSearchTemp))

        End If

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
                End If
            End If

            If intSelectedCo = 0 Then
                ' id of distributor not, already, selected
                cboDistributors.Items.Clear()
            End If


            Dim data As DataTable = masterCo.LoadSearch("", 0, intSelectedCo, 0)

            If mblnLoadDistributors Then
                cboDistributors.Items.Add(New RadComboBoxItem(GetLocalResourceObject("All")))
            End If

            If mblnLoadServiceProvs Then
                cboServiceProvs.Items.Add(New RadComboBoxItem(GetLocalResourceObject("All")))
            End If

            For Each row As DataRow In data.Rows
                If mblnLoadDistributors AndAlso row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor) AndAlso CInt(row("MasterCoID")) <> intSelectedCo Then
                    cboDistributors.Items.Add(New RadComboBoxItem(row("Name"), CStr(row("MasterCoID"))))
                ElseIf mblnLoadServiceProvs AndAlso row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany) Then
                    Dim blnSelected As Boolean = False

                    If Not IsNothing(savedDetails(cSaveServiceProviderKey)) Then
                        blnSelected = CStr(row("MasterCoID")) = savedDetails(cSaveServiceProviderKey)

                    End If
                    cboServiceProvs.Items.Add(New RadComboBoxItem(row("Name"), CStr(row("MasterCoID"))))

                    If blnSelected Then
                        cboServiceProvs.SelectedValue = CStr(row("MasterCoID"))
                    End If

                End If
            Next

        End If

        ' droplists only enabled if more than one choice available
        If cboDistributors.Items.Count > 1 Then
            cboDistributors.Enabled = True
        Else
            cboDistributors.Enabled = False
            If cboDistributors.Items.Count > 0 Then
                cboDistributors.SelectedIndex = 0
            End If
        End If

        If cboServiceProvs.Items.Count > 1 Then
            If String.IsNullOrEmpty(cboServiceProvs.SelectedValue) Then
                cboServiceProvs.Text = ""
            End If
            cboServiceProvs.Enabled = True
        Else
            cboServiceProvs.Enabled = False

            If cboServiceProvs.Items.Count > 0 Then
                cboServiceProvs.SelectedIndex = 0
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

        If Not IsNothing(dtbInstallerUsers) Then
            cboInstallers.Items.Add(New RadComboBoxItem(GetLocalResourceObject("All")))

            For Each userRow As DataRow In dtbInstallerUsers.Rows
                Dim item As New RadComboBoxItem(String.Format("{0}, {1}", userRow("LastName"), userRow("FirstName")), userRow("UserID").ToString())

                cboInstallers.Items.Add(item)

                If Not IsNothing(savedDetails(cSaveIntaInstallerIDKey)) Then
                    item.Selected = (item.Value = savedDetails(cSaveIntaInstallerIDKey))
                End If
            Next
        End If
        If cboInstallers.Items.Count > 0 Then
            cboInstallers.Enabled = cboInstallers.Items.Count > 1

            If String.IsNullOrEmpty(cboInstallers.SelectedValue) Then
                cboServiceProvs.Text = ""
                savedDetails.Remove(cSaveIntaInstallerIDKey)
            End If

        Else
            cboInstallers.Enabled = False
            savedDetails.Remove(cSaveIntaInstallerIDKey)
            cboServiceProvs.Text = ""
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
                If mUsersCompany.CompanyTypeID = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor) Then
                    strItemLegend = mUsersCompany.Name
                Else
                    divDistList.Visible = False
                End If

                cboDistributors.Items.Add(New RadComboBoxItem(strItemLegend, CStr(IIf(mblnLoadServiceProvs, mUsersCompany.MasterCoID, 0))))
                cboDistributors.SelectedIndex = 0
            End If

            If Not mblnLoadServiceProvs Then
                If mUsersCompany.CompanyTypeID = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany) Then
                    strItemLegend = mUsersCompany.Name
                End If

                cboServiceProvs.Items.Add(New RadComboBoxItem(strItemLegend, CStr(IIf(mblnLoadInstallers, mUsersCompany.MasterCoID, 0))))
                cboServiceProvs.SelectedIndex = 0
                cboServiceProvs.Enabled = False
            End If

            If Not mblnLoadInstallers Then
                strItemLegend = mLoggedInUser.Firstname + ", " + mLoggedInUser.Lastname

                cboInstallers.Items.Add(New RadComboBoxItem(strItemLegend, CStr(mLoggedInUser.UserID)))
                cboInstallers.SelectedIndex = 0
                cboInstallers.Enabled = False
            End If
        End If

        LoadCompanylists()

    End Sub

    Private Sub cboDistributors_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboDistributors.SelectedIndexChanged, cboServiceProvs.SelectedIndexChanged, cboInstallers.SelectedIndexChanged
        ListSelectionChanged(sender, e)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        rgAccounts.ExportSettings.ExportOnlyData = False
        rgAccounts.ExportSettings.OpenInNewWindow = True
        rgAccounts.ExportSettings.IgnorePaging = True
        rgAccounts.ExportSettings.FileName = "EventLog"

        rgAccounts.MasterTableView.ExportToCSV()

    End Sub

    Private Sub rgAccounts_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgAccounts.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim data As DataRowView = DirectCast(dataItem.DataItem, DataRowView)

            Dim eventStyle = "grdEventTest"

            If CBool(data("IsFault")) Then
                eventStyle = "grdEventFault"
            ElseIf CBool(data("IsAlert")) Then
                eventStyle = "grdEventAlarm"
            End If

            Dim IntaSIACode As String = data("IntaAlarmType")
            If IntaSIACode.Contains("R") Then
                eventStyle = "grdEventTest"
            End If

            dataItem("SIADesc").CssClass = eventStyle

            Dim lblSensorDesc As Label = DirectCast(dataItem.FindControl("lblSensorDesc"), Label)
            Dim lblPropZoneDesc As Label = DirectCast(dataItem.FindControl("lblPropZoneDesc"), Label)

            If Not IsNothing(lblPropZoneDesc) Then
                If Not IsDBNull(data("IntaSensorType")) AndAlso data("IntaSensorType") <> gatewaySensorType Then
                    lblPropZoneDesc.Text = SensorTypeGet(data("IntaSensorType"))
                Else
                    lblPropZoneDesc.Text = GetLocalResourceObject("Gateway")
                End If
            End If

            'allow telerik filtering on device name column
            data("DeviceName") = lblPropZoneDesc.Text

            If Not IsNothing(lblSensorDesc) Then
                If Not IsDBNull(data("IntaSensorType")) AndAlso data("IntaSensorType") <> gatewaySensorType Then
                    lblSensorDesc.Text = DeviceTypeGet(data("IntaSensorType"))
                Else
                    lblSensorDesc.Text = GetLocalResourceObject("GatewayDeviceType")
                End If
            End If

            'CAW 20161116 - SP-1236 - Substitute 'No Location' from event description for locally translated text
            If data("IntaRoomName") = "No Location" Then dataItem("IntaRoomName").Text = GetLocalResourceObject("NoLocation")
            If data("IntaAreaName") = "No Location" Then dataItem("IntaAreaName").Text = GetLocalResourceObject("NoLocation")


        End If
    End Sub

    Public Sub tmrEventLogGrid_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrEventLogGrid.Tick
        rgAccounts.Rebind()
    End Sub
    ''' <summary>
    ''' Clears the page 'filter' controls and the associated 'SavedDetails'
    ''' </summary>
    ''' <param name="v_blnJustGrid">If set true, only the grid 'header' filters are cleared.</param>
    ''' <remarks></remarks>
    Private Sub ClearFilters(ByVal v_blnJustGrid As Boolean)

        ' filtersToClear, list of keys, into filterDef, of filters to clear. Initialised for grid 'header' filter controls.
        Dim filtersToClear As New List(Of String)({cSaveSearchAccountID, cSaveSearchAreaName, cSaveSearchCreatedDate, cSaveSearchCreatedTime, cSaveSearchPropZoneDesc, cSaveSearchRoomName, cSaveSearchSensorDesc, cSaveSearchDeviceID, cSaveSearchTemp,
                                                   cSaveSearchSIAKey})
        ' if all filters to be cleared add required keys
        If Not v_blnJustGrid Then
            ' none to add if logged in user is an 'Installer'
            If Not Roles.IsUserInRole("installer") Then
                Select Case mUsersCompany.CompanyTypeID
                    Case CInt(e_MasterCompanyTypes.OperatingCompany)
                        ' Service Provider users can only filter on 'Installer'
                        filtersToClear.AddRange(New List(Of String)({cSaveIntaInstallerIDKey}))

                    Case CInt(e_MasterCompanyTypes.Distributor)
                        ' Distributor users can only filter on 'Service provider' or 'Installer'
                        filtersToClear.AddRange(New List(Of String)({cSaveServiceProviderKey, cSaveIntaInstallerIDKey}))

                    Case Else
                        ' Must be a 'Sprue' user so all 3 filters are available
                        filtersToClear.AddRange(New List(Of String)({cSaveDistributorKey, cSaveServiceProviderKey, cSaveIntaInstallerIDKey}))
                End Select
            End If
        End If

        ' clear cached data
        savedDetails.Clear()

        If Not v_blnJustGrid Then
            ' set, non grid, filters to initial state
            InitialiseFilterLists()

        End If

        Dim filterCtlCache As Dictionary(Of String, Control) = getFiltersDef()

        ' loop through keys, clearing associated control
        For Each filterKey As String In filtersToClear

            If filterCtlCache.ContainsKey(filterKey) Then

                Dim filterCtl As Control = filterCtlCache(filterKey)

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
            End If

        Next

        ' ensure grid's own filtering is cleared
        For Each col As GridColumn In rgAccounts.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next

        rgAccounts.MasterTableView.FilterExpression = ""

    End Sub


    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ' only need to clear company/installer filters if they are visible, i.e. 'grid only' if they are not
        ClearFilters(Not divCompaniesFilter.Visible)
        rgAccounts.Rebind()

    End Sub


    ''' <summary>
    ''' Get the sensor type name from the resources based on sensor type
    ''' </summary>
    ''' <param name="strSensorType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Function SensorTypeGet(strSensorType As Integer) As String
        Select Case strSensorType

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.ColdAlarm
                'Extreme Temperature Alarm
                Return PageString("ColdAlarm")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.SmokeAlarm
                'Smoke Alarm
                Return PageString("SmokeAlarm")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.HeatAlarm
                'Heat Alarm
                Return PageString("HeatAlarm")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.COAlarm
                'Carbon Monoxide Alarm
                Return PageString("COAlarm")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.ACSmokeAlarm
                '230V - Smoke Alarm
                Return PageString("ACSmokeAlarm")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.ACHeatAlarm
                '230V - Heat Alarm
                Return PageString("ACHeatAlarm")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.PadStrobe
                'Strobe & Vibrating Pad
                Return PageString("PadStrobe")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.LowFreqSounder
                'Low Frequency Sounder
                Return PageString("LowFreqSounder")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.AlarmControlUnit
                'Alarm Control Unit
                Return PageString("AlarmControlUnit")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.InterfaceGateway
                'Interface Gateway
                Return PageString("InterfaceGateway")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.InterfaceGateway200
                'Interface Gateway 200
                Return PageString("InterfaceGateway")

            Case e_DeviceID.RNDV2
                Return PageString("GatewayDetails")

            Case Else
                Return ""

        End Select
    End Function

    ''' <summary>
    ''' Returns string representing comma separated list of the, numeric, sensor types for which the text name starts with a given string
    ''' </summary>
    ''' <param name="strSensorPrefix"></param>
    ''' <returns></returns>
    Private Function SensorInFilterGet(strSensorPrefix As String) As List(Of Object)
        Dim outList As New List(Of Object)
        Dim testString As String = strSensorPrefix.Trim().ToLowerInvariant()

        For Each sensorType As e_AlarmSensorTypes In [Enum].GetValues(GetType(e_AlarmSensorTypes))
            If PageString(sensorType.ToString()).ToLowerInvariant().StartsWith(testString) Then
                outList.Add(CInt(sensorType))
            End If
        Next

        Return outList
    End Function

    ''' <summary>
    ''' Get the device type from the resources based on sensor type
    ''' </summary>
    ''' <param name="strSensorType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Function DeviceTypeGet(strSensorType As Integer) As String
        Select Case strSensorType

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.ColdAlarm
                'Extreme Temperature Alarm   WETA-10X
                Return PageString("ColdAlarmType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.SmokeAlarm
                'Smoke Alarm      WST-630
                Return PageString("SmokeAlarmType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.HeatAlarm
                'Heat Alarm   WHT-630
                Return PageString("HeatAlarmType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.COAlarm
                'Carbon Monoxide Alarm     W2-CO-10X 
                Return PageString("COAlarmType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.ACSmokeAlarm
                '230V - Smoke Alarm    WSM-F-1EU 
                Return PageString("ACSmokeAlarmType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.ACHeatAlarm
                '230V - Heat Alarm      WHM-F-1EU 
                Return PageString("ACHeatAlarmType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.PadStrobe
                'Strobe & Vibrating Pad    W2-SVP-630
                Return PageString("PadStrobeType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.LowFreqSounder
                'Low Frequency Sounder     W2-LFS-630
                Return PageString("LowFreqSounderType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.AlarmControlUnit
                'Alarm Control Unit	       WTSL-F-1EU
                Return PageString("AlarmControlUnitType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.InterfaceGateway
                'Interface Gateway         IFG100
                Return PageString("InterfaceGatewayType")

            Case IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.InterfaceGateway200
                'Interface Gateway         IFG200
                Return PageString("InterfaceGateway200Type")

            Case e_DeviceID.RNDV2
                Return PageString("GatewayDeviceType")

            Case Else
                Return ""

        End Select
    End Function
End Class
