Imports IntamacBL_SPR
Imports System.Reflection
Imports IntamacShared_SPR
Imports IntamacShared_SPR.SharedStuff
Imports Telerik.Web.UI


Public Class ManageSensors
    Inherits CultureBaseClass

    Protected m_Areas As List(Of Areas)
    Protected SelectedZone As String
    Protected SelectedFaultStatus As String
    Protected SelectedUnregisteredFaultStatus As String
    Protected SelectedArchivedZoneStatus As String
    Private _pendingDeletions As List(Of String)

    Private m_ArchivedHub As New List(Of SprueHub)

      Public Overrides Property mAccountID As String
        Get
            If String.IsNullOrEmpty(ViewState("mAccountID")) Then
                ViewState("mAccountID") = MyBase.mAccountID
            End If
            Return CStr(ViewState("mAccountID"))
        End Get
        Set(value As String)
            ViewState("mAccountID") = value
        End Set
    End Property

    Public Overrides Property mPropertyID As String
        Get
            If String.IsNullOrEmpty(ViewState("mPropertyID")) Then
                ViewState("mPropertyID") = MyBase.mPropertyID
            End If
            Return CStr(ViewState("mPropertyID"))
        End Get
        Set(value As String)
            ViewState("mPropertyID") = value
        End Set
    End Property


    ''' <summary>
    ''' Stores values containing the deviceSeq and propZone of sensors
    ''' where delete has been requested (but not necessarily happened yet).
    ''' Use MarkAsPendingDeletion to add an item here, or IsPendingDeletion
    ''' to check if it has already been added.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property PendingDeletions As List(Of String)
        Get
            If _pendingDeletions Is Nothing Then
                _pendingDeletions = CType(ViewState("PendingDeletions"), List(Of String))
                If _pendingDeletions Is Nothing Then
                    _pendingDeletions = New List(Of String)
                End If
            End If
            Return _pendingDeletions
        End Get
    End Property

#Region "New/Overrides"

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        LoadAreas()

        If Not Page.IsPostBack Then

            'Go back to Account Search when there is no mAccountID or mPropertyID which can get cleared when using browser back button, 
            'i.e. avoid showing all devices for all properties on this page
            If String.IsNullOrEmpty(mAccountID) Or String.IsNullOrEmpty(mPropertyID) Then
                If Not IsTopNavigate Then
                    Response.Redirect("AccountSearch.aspx")
                End If
            End If

            'Set Back Location Manually
            mBackLocation = "AccountDetail.aspx"

            InitEditPropertyRiskLevels()

            'Load Client Details
            Dim clientGetter As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")
            Dim objClient As IntamacBL_SPR.Client = clientGetter.Load(mAccountID)

            'Load Property Details
            Dim propertyGetter As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
            Dim objProperty As IntamacBL_SPR.PropertyBase = propertyGetter.Load(mAccountID, mPropertyID)

            'Load Device Details (for MAC Address)
            Dim dev As New IntamacDevice
            dev.CompanyType = mCompanyType
            Dim devList As Generic.List(Of IntamacDevice) = dev.LoadAll(mAccountID)
            Dim zoneID As String = CType(Session.Item("ZoneID"), String)
            For Each dev In devList
                If dev.DeviceType.ToLower = "alarm" And dev.PropertyID = mPropertyID Then
                    lblMacAddressValue.Text = dev.MACAddress
                    Exit For
                End If

            Next

            'Set fields from loaded objects
            lblAccounHolderValue.Text = objClient.Title & " " &
                                        objClient.FirstName & " " &
                                        objClient.Surname

                lblAccountIDValue.Text = objClient.AccountID
                txtEditAccountAddress1.Text = objClient.Address1
                txtEditAccountAddress2.Text = objClient.Address2
                txtEditAccountAddress3.Text = objClient.Address3
                txtEditAccountAddress4.Text = objClient.Address4
                txtEditAccountPostCode.Text = objClient.Postcode

                If objProperty IsNot Nothing Then

                    txtEditPropertyAddress1.Text = objProperty.Address1
                    txtEditPropertyAddress2.Text = objProperty.Address2
                    txtEditPropertyAddress3.Text = objProperty.Address3
                    txtEditPropertyAddress4.Text = objProperty.Address4
                    txtEditPropertyPostCode.Text = objProperty.Postcode

                    'Set Property Address
                    RefereshScreenPropertyAddress(objProperty)

                    ' update risk level:
                    cboEditPropertyRiskLevel.SelectedValue = objProperty.RiskLevel.ToString()
                End If
            GetGateway()

            'Get Sensors Grids
            GetSensors()

        End If

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        btnGatewayClose.Visible = radGateSensors.Items.Count > 0
    End Sub

#End Region

    ''' <summary>
    ''' Loads tha Areas associated with the Account/Property and, if the number of items has changed, since last called, populate the 'Edit' combo boxes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadAreas()
        'Get Areas
        Dim areaGetter As New Areas
        Dim zoneID As String = CType(Session.Item("ZoneID"), String)
        m_Areas = areaGetter.Load(mAccountID, mPropertyID)

        ' populate combo boxes if required
        If m_Areas IsNot Nothing AndAlso (m_Areas.Count >= cboDeviceZone.Items.Count OrElse m_Areas.Count >= cboGatewayZone.Items.Count) Then
            ' clear combo boxes
            cboDeviceZone.Items.Clear()
            cboGatewayZone.Items.Clear()

            'ddlDeviceZone and ddlGatewayZone bind with areas list in sensor edit modal
            cboDeviceZone.Items.Add("")
            cboGatewayZone.Items.Add("")
            For Each thisArea As Areas In m_Areas
                cboGatewayZone.Items.Add(New RadComboBoxItem(thisArea.Name, thisArea.ID))
                cboDeviceZone.Items.Add(New RadComboBoxItem(thisArea.Name, thisArea.ID))
            Next

        End If
    End Sub

    ''' <summary>
    ''' returns (as an Integer) the selectedValue of the supplied RadComboBox or, if it has no SelectedValue but has, a non-empty, Text value creates a new Area entry, returning it's ID
    ''' </summary>
    ''' <param name="targCombo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCreateAreaID(ByVal targCombo As RadComboBox) As Integer
        'need to update area
        Dim intAreaID As Integer = 0
        'if the zone is selected use the AreaID otherwise create a new Area/Zone
        If Not String.IsNullOrEmpty(targCombo.SelectedValue) Then
            intAreaID = CInt(targCombo.SelectedValue)
        ElseIf Not String.IsNullOrWhiteSpace(targCombo.Text) Then
            'need to add new zone
            Dim areaGetter As New Areas
            intAreaID = CInt(areaGetter.Insert(mAccountID, mPropertyID, targCombo.Text))
            LoadAreas()
            updEditSensor.Update()
            updEditGateway.Update()

        End If

        Return intAreaID
    End Function

    Private Sub GetGateway()
        Dim mCompany As IntamacBL_SPR.MasterCompanySPR = Nothing
        Dim hubs As List(Of AlarmPanel) = Nothing
        Dim zoneID As String = CType(Session.Item("ZoneID"), String)
        'load the prop device records.  These contains an installer field which indicates who the installer is
        Dim objHub As New SprueHub

        hubs = objHub.LoadList(mAccountID, mPropertyID, 0)
        If hubs IsNot Nothing AndAlso hubs.Count > 0 Then

            For Each propDev As AlarmPanel In hubs

                If propDev IsNot Nothing Then
                    BindHubDevice(propDev)
                End If
            Next
        End If

    End Sub

    Private Sub BindHubDevice(objHub As SprueHub)
        Dim zoneID As String = CType(Session.Item("ZoneID"), String)
        If Not objHub.IsArchived Then
            Dim dtGateway As DataTable = CreateHubTable()
            If ShouldDisplayHub(objHub) Then
                Dim propDevice As New PropertyDeviceSPR()

                propDevice.LoadByMacAddress(objHub.MACAddress)
                AddHubDeviceToTable(objHub, propDevice, dtGateway)
            End If

            Dim dvGateway As New DataView(dtGateway)

            If objHub.ID = zoneID Or zoneID Is Nothing Or zoneID = "" Then
                radGateSensors.DataSource = dvGateway
                radGateSensors.DataBind()
            End If
        Else
            m_ArchivedHub.Add(objHub)

        End If
    End Sub

    Private Function CreateHubTable() As DataTable
        Dim dtGateway As New DataTable()
        dtGateway.Columns.Add("GatewayName", GetType(String))
        dtGateway.Columns.Add("IsOnline", GetType(String))
        dtGateway.Columns.Add("IntaDeviceSeq", GetType(String))
        dtGateway.Columns.Add("SensorType", GetType(String))
        dtGateway.Columns.Add("AreaName", GetType(String))
        dtGateway.Columns.Add("AreaID", GetType(String))
        dtGateway.Columns.Add("IntaRoomName", GetType(String))
        dtGateway.Columns.Add("FaultStatus", GetType(String))
        dtGateway.Columns.Add("IntaPropZone", GetType(Integer))
        dtGateway.Columns.Add("CreatedDate", GetType(DateTime))
        dtGateway.Columns.Add("IntaDeviceID", GetType(String))
        dtGateway.Columns.Add("FirmwareVersion", GetType(String))
        dtGateway.Columns.Add("FirmwareDesc", GetType(String))
        Return dtGateway
    End Function


    Private Function GetFirmwareDescription(ByVal lstFirmware As List(Of FirmwareUpgrade), ByVal firmwareVersion As String) As String
        'get display name of firmware version

        If lstFirmware IsNot Nothing Then

            For Each firmware As FirmwareUpgrade In lstFirmware
                If firmware.FirmwareVersion = firmwareVersion Then
                    'return firmware description for label
                    Return firmware.FirmwareDesc
                End If
            Next

        End If

        'return firmware version as default for label
        Return firmwareVersion

    End Function

    Private Sub AddHubDeviceToTable(objHub As SprueHub, propDev As PropertyDevice, dtGateway As DataTable)
        Dim row As DataRow
        Dim objFirmwareUpgrade As New FirmwareUpgrade
        Dim lstFirmware As List(Of FirmwareUpgrade) = objFirmwareUpgrade.Load(objHub.DeviceID, "SPR")

        row = dtGateway.NewRow

        row("GatewayName") = propDev.DevicePosition
        row("IsOnline") = objHub.IsOnline
        row("IntaDeviceSeq") = objHub.DeviceSeq

        'Show the device type as 'WG-1' for a gateway.  We can assume this as there is only one type of gateway
        row("SensorType") = GetLocalResourceObject("GatewayDeviceType").ToString  ' objPropDeviceGetter.DeviceType
        row("IntaRoomName") = propDev.RoomName
        row("IntaPropZone") = propDev.Zone
        'SP-1172 Installed date need to show in local time
        Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)
        propDev.DateConnected = masterCo.ConvertDateTimeToLocalTime(propDev.DateConnected, mLoggedInUser.MasterCoID)
        If IsDate(propDev.DateConnected) Then
            row("CreatedDate") = propDev.DateConnected
        End If

        row("IntaDeviceID") = propDev.DeviceID
        row("FirmwareVersion") = objHub.FirmwareVersion
        row("FirmwareDesc") = GetFirmwareDescription(lstFirmware, objHub.FirmwareVersion)
        row("AreaName") = propDev.AreaName
        row("AreaID") = propDev.AreaID

        dtGateway.Rows.Add(row)
    End Sub

    Private Function ShouldDisplayHub(hub As SprueHub) As Boolean
        If hub Is Nothing Then
            Return False
        End If
        Select Case mAccountDetailTileSelected
            Case Dashboard.Tiles.ActiveFaults
                Return hub.IsOnline <> True
            Case Dashboard.Tiles.ActiveAlerts
                ' gateway can never be in an alert state
                Return False
            Case Else
                Return True
        End Select
    End Function


    Private Sub GetSensors()

        Dim propZoneGetter As PropZone = ObjectManager.CreatePropZone(mCompanyType)
        Dim objZones As List(Of PropZone) = propZoneGetter.Load(mAccountID, mPropertyID)

        'Create Datatables for Registered, Unregistered and Archived Zones
        Dim dtZones As New DataTable
        Dim dtUnRegisteredZones As New DataTable()
        Dim dtArchivedZones As New DataTable()
        Dim zoneID As String = CType(Session.Item("ZoneID"), String)

        dtZones.Columns.Add("IntaAccountID", GetType(String))
        dtZones.Columns.Add("IntaPropertyID", GetType(String))
        dtZones.Columns.Add("IntaPropZone", GetType(Integer))
        dtZones.Columns.Add("IntaDeviceSeq", GetType(Integer))
        dtZones.Columns.Add("IntaPropZoneDesc", GetType(String))
        dtZones.Columns.Add("SensorType", GetType(String))
        dtZones.Columns.Add("IntaRoomName", GetType(String))
        dtZones.Columns.Add("AreaName", GetType(String))
        dtZones.Columns.Add("AreaID", GetType(Integer))
        dtZones.Columns.Add("CreatedDate", GetType(DateTime))
        dtZones.Columns.Add("IntaDeviceID", GetType(String))
        dtZones.Columns.Add("BatteryStatus", GetType(String))
        dtZones.Columns.Add("ChamberSensorStatus", GetType(String))
        dtZones.Columns.Add("UnitOnBase", GetType(String))
        dtZones.Columns.Add("FaultStatus", GetType(String))
        dtZones.Columns.Add("SensorState", GetType(String))
        dtZones.Columns.Add("SensorAttribute", GetType(String))
        dtZones.Columns.Add("IsPresent", GetType(Boolean))
        dtZones.Columns.Add("IsRemoved", GetType(Boolean))
        dtZones.Columns.Add("Mute", GetType(Boolean))

        'Clone Zones for Unregistered and Archived Zones
        dtUnRegisteredZones = dtZones.Clone()
        dtArchivedZones = dtZones.Clone()

        If m_ArchivedHub.Count > 0 Then
            Dim objPropDeviceGetter As PropertyDevice = Nothing
            objPropDeviceGetter = ObjectManager.CreatePropertyDevice(mCompanyType)

            For Each archHub As SprueHub In m_ArchivedHub
                If archHub.ID = zoneID Or zoneID Is Nothing Or zoneID = "" Then

                    'load the prop device record.  This contains an installer field which indicates who the installer is
                    If objPropDeviceGetter.Load(mAccountID, mPropertyID, CInt(archHub.DeviceSeq)) Then
                        Dim row As DataRow = dtArchivedZones.NewRow

                        row("IntaAccountID") = archHub.AccountID
                        row("IntaPropertyID") = archHub.PropertyID
                        row("IntaPropZone") = objPropDeviceGetter.Zone
                        row("IntaDeviceSeq") = archHub.DeviceSeq
                        row("IntaPropZoneDesc") = objPropDeviceGetter.DevicePosition
                        row("SensorType") = archHub.DeviceID
                        row("IntaRoomName") = objPropDeviceGetter.RoomName
                        'SP-1172 Installed date need to show in local time
                        Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)
                        objPropDeviceGetter.DateConnected = masterCo.ConvertDateTimeToLocalTime(objPropDeviceGetter.DateConnected, mLoggedInUser.MasterCoID)
                        If IsDate(objPropDeviceGetter.DateConnected) Then
                            row("CreatedDate") = objPropDeviceGetter.DateConnected
                        End If
                        row("IntaDeviceID") = objPropDeviceGetter.DeviceID
                        row("AreaName") = objPropDeviceGetter.AreaName
                        row("IsRemoved") = True
                        row("IsPresent") = False

                        dtArchivedZones.Rows.Add(row)
                    End If
                End If

            Next
        End If

        'Loop through Zones and set Datatable rows
        For Each item As PropZone In objZones
            If item.AreaID = zoneID Or zoneID Is Nothing Or zoneID = "" Then


                Dim strAreaName As String = ""
                Dim row As DataRow

                If item.AreaID IsNot Nothing Then

                    'Find area name from loaded Areas
                    If m_Areas.Find(Function(f) f.ID = item.AreaID) IsNot Nothing Then

                        strAreaName = m_Areas.Find(Function(f) f.ID = item.AreaID).Name

                    End If

                End If

                'If zone or location is blank then add to unregistered table otherwise add to zones table
                If item.IsPresent = False Then
                    row = dtArchivedZones.NewRow
                ElseIf String.IsNullOrEmpty(item.RoomName) Then
                    row = dtUnRegisteredZones.NewRow
                Else
                    row = dtZones.NewRow
                End If
                row("IntaAccountID") = item.AccountID
                row("IntaPropertyID") = item.PropertyID
                row("IntaPropZone") = item.PropZone
                row("IntaDeviceSeq") = item.DeviceSeq
                row("IntaPropZoneDesc") = item.PropZoneDesc
                row("SensorType") = miscFunctions.GetSensorType(item.SensorType)
                row("IntaRoomName") = item.RoomName
                'SP-1172 Installed date need to show in local time
                Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)

                If item.CreatedDate.HasValue Then
                    item.CreatedDate = masterCo.ConvertDateTimeToLocalTime(item.CreatedDate, mLoggedInUser.MasterCoID)
                    If IsDate(item.CreatedDate) Then
                        row("CreatedDate") = item.CreatedDate
                    End If
                End If
                row("IntaDeviceID") = item.DeviceID
                row("AreaName") = strAreaName
                row("AreaID") = IIf(item.AreaID.HasValue, item.AreaID, 0)
                row("BatteryStatus") = SetFaultStatus("BatteryOK", item.BatteryOK)
                row("ChamberSensorStatus") = SetFaultStatus("ChamberSensorStatusOK", item.ChamberSensorStatusOK)
                row("UnitOnBase") = SetFaultStatus("IsOnBase", item.IsOnBase)
                row("SensorState") = item.SensorState
                row("Mute") = item.Mute
                row("SensorAttribute") = item.SensorAttribute
                row("IsPresent") = (item.IsPresent Is Nothing OrElse item.IsPresent)
                row("IsRemoved") = (item.IsRemoved Is Nothing OrElse item.IsRemoved)

                'if there is an alert for this zone then set it to ALERT. otherwise set to zone isfaulty
                If item.AlarmActive = True OrElse item.AlarmActiveSilenced = True OrElse item.AlarmActiveCannotSilence = True Then
                    row("FaultStatus") = GetLocalResourceObject("Alert").ToString.ToUpper
                Else
                    'SP-1058  
                    row("FaultStatus") = SetFaultStatus("IsFaulty", item.IsFaulty Or Not (item.IsOnBase And item.ChamberSensorStatusOK And item.BatteryOK))
                End If

                'Add to table depending on if zone and location are empty or isonline

                If item.IsPresent = False Then
                    dtArchivedZones.Rows.Add(row)
                ElseIf String.IsNullOrEmpty(item.RoomName) Then
                    dtUnRegisteredZones.Rows.Add(row)
                Else
                    dtZones.Rows.Add(row)
                End If
            End If
        Next
        'changed



        'Convert datatable to dataview to be able to filter easily
        Dim dvZones As New DataView(dtZones)
        Dim dvUnRegisteredZones As New DataView(dtUnRegisteredZones)
        Dim dvArchivedZones As New DataView(dtArchivedZones)

        'Create filter field for each table
        Dim strSensorsFilter As String = Nothing
        Dim strUnregisteredSensorsFilter As String = Nothing
        Dim strArchivedSensorsFilter As String = Nothing

        'Set the filter for zones depending on dropdown settings
        If radGridSensors.MasterTableView.GetItems(GridItemType.Header).Length > 0 Then

            Dim headerItem As GridHeaderItem = CType(radGridSensors.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            If headerItem IsNot Nothing Then
                Dim cboZoneSearch As RadComboBox = CType(headerItem.FindControl("cboZoneSearch"), RadComboBox)
                Dim cboFaultSearch As RadComboBox = CType(headerItem.FindControl("cboFaultSearch"), RadComboBox)

                If cboZoneSearch IsNot Nothing Then
                    If String.IsNullOrEmpty(cboZoneSearch.SelectedValue) Then
                        SelectedZone = "All"
                    Else
                        SelectedZone = cboZoneSearch.SelectedValue
                    End If
                End If

                If cboFaultSearch IsNot Nothing Then
                    If String.IsNullOrEmpty(cboFaultSearch.SelectedValue) Then
                        SelectedFaultStatus = "All"
                    Else
                        SelectedFaultStatus = cboFaultSearch.SelectedValue
                    End If
                End If

                If SelectedZone <> "All" Then
                    'CAW 20161129 SP1235 - apostrophe causing error on filter
                    strSensorsFilter = "AreaName='" & SelectedZone.Replace("'", "''") & "'"
                End If

                If SelectedFaultStatus <> "All" Then
                    If strSensorsFilter IsNot Nothing Then
                        strSensorsFilter += " and FaultStatus='" & SelectedFaultStatus & "'"
                    Else
                        strSensorsFilter = "FaultStatus='" & SelectedFaultStatus & "'"
                    End If
                End If


            End If

        End If



        If mAccountDetailTileSelected.HasValue Then

            If mAccountDetailTileSelected = Dashboard.Tiles.ActiveAlerts Then
                If strSensorsFilter IsNot Nothing Then
                    strSensorsFilter += " and FaultStatus='" & GetLocalResourceObject("Alert") & "'"
                Else
                    strSensorsFilter = "FaultStatus='" & GetLocalResourceObject("Alert") & "'"
                End If
            End If
            If mAccountDetailTileSelected = Dashboard.Tiles.ActiveFaults Then

                If strSensorsFilter IsNot Nothing Then
                    strSensorsFilter += " and FaultStatus='" & GetLocalResourceObject("Fault") & "'"
                Else
                    strSensorsFilter = "FaultStatus='" & GetLocalResourceObject("Fault") & "'"
                End If

            End If
            If mAccountDetailTileSelected = Dashboard.Tiles.NoFaults Then

                If strSensorsFilter IsNot Nothing Then
                    strSensorsFilter += " and FaultStatus='" & GetGlobalResourceObject("PageGlobalResources", "OKText") & "'"
                Else
                    strSensorsFilter = "FaultStatus='" & GetGlobalResourceObject("PageGlobalResources", "OKText") & "'"
                End If
            End If
            If mAccountDetailTileSelected = Dashboard.Tiles.TotalDevices Then

                'If strSensorsFilter IsNot Nothing Then
                '	strSensorsFilter += " and FaultStatus='" & GetLocalResourceObject("AllFaults") & "'"
                'Else
                '	strSensorsFilter = "FaultStatus='" & GetLocalResourceObject("AllFaults") & "'"
                'End If

            End If

        End If



        'Apply filter to zones dataview
        If strSensorsFilter IsNot Nothing Then
            dvZones.RowFilter = strSensorsFilter
        Else
            dvZones.RowFilter = ""
        End If

        'Set the filter for unregistered zones depending on dropdown settings
        If radGridUnregisteredSensors.MasterTableView.GetItems(GridItemType.Header).Length > 0 Then

            Dim headerItem As GridHeaderItem = CType(radGridUnregisteredSensors.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            If headerItem IsNot Nothing Then
                Dim cboFaultSearch As RadComboBox = CType(headerItem.FindControl("cboFaultSearch"), RadComboBox)

                If cboFaultSearch IsNot Nothing Then
                    If String.IsNullOrEmpty(cboFaultSearch.SelectedValue) Then
                        SelectedUnregisteredFaultStatus = "All"
                    Else
                        SelectedUnregisteredFaultStatus = cboFaultSearch.SelectedValue
                    End If
                End If

                If SelectedUnregisteredFaultStatus <> "All" Then
                    strUnregisteredSensorsFilter = "FaultStatus='" & SelectedUnregisteredFaultStatus & "'"
                End If


            End If

        End If


        'Apply filter to unregistered zones dataview
        If strUnregisteredSensorsFilter IsNot Nothing Then
            dvUnRegisteredZones.RowFilter = strUnregisteredSensorsFilter
        Else
            dvUnRegisteredZones.RowFilter = ""
        End If

        'Set the filter for atchived zones depending on dropdown settings
        If radGridArchivedSensors.MasterTableView.GetItems(GridItemType.Header).Length > 0 Then

            Dim headerItem As GridHeaderItem = CType(radGridArchivedSensors.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            If headerItem IsNot Nothing Then
                Dim cboZoneSearch As RadComboBox = CType(headerItem.FindControl("cboZoneSearch"), RadComboBox)

                If cboZoneSearch IsNot Nothing Then
                    If String.IsNullOrEmpty(cboZoneSearch.SelectedValue) Then
                        SelectedArchivedZoneStatus = "All"
                    Else
                        SelectedArchivedZoneStatus = cboZoneSearch.SelectedValue
                    End If
                End If

                If SelectedArchivedZoneStatus <> "All" Then
                    strArchivedSensorsFilter = "AreaName='" & SelectedArchivedZoneStatus.Replace("'", "''") & "'"
                End If


            End If

        End If

        'Apply filter to unregistered zones dataview
        If strArchivedSensorsFilter IsNot Nothing Then
            dvArchivedZones.RowFilter = strArchivedSensorsFilter
        Else
            dvArchivedZones.RowFilter = ""
        End If


        'Bind dataviews to grids
        radGridSensors.DataSource = dvZones
        radGridSensors.DataBind()

        radGridUnregisteredSensors.DataSource = dvUnRegisteredZones
        radGridUnregisteredSensors.DataBind()

        radGridArchivedSensors.DataSource = dvArchivedZones
        radGridArchivedSensors.DataBind()

    End Sub

    ''' <summary>
    ''' Allows us to set to Fault or OK depending on Field.  Some fields fault if true, others fault if false
    ''' </summary>
    ''' <param name="strFieldName">The fieldname to allow us to determine whether fault is when value is true or false</param>
    ''' <param name="boolValue">The  value of the field</param>
    ''' <returns>Returns either Fault or OK</returns>
    ''' <remarks></remarks>
    Private Function SetFaultStatus(strFieldName As String, boolValue As Nullable(Of Boolean)) As String

        Dim faultStatus As String = PageString("Fault").ToUpper
        Dim okStatus As String = PageString("OK").ToUpper

        If (strFieldName = "IsOnBase" Or strFieldName = "BatteryOK" Or strFieldName = "ChamberSensorStatusOK") And (boolValue.HasValue AndAlso boolValue = False) Then
            'device off base, not calibrated or low battery is shown as a fault
            Return faultStatus
        ElseIf strFieldName = "IsFaulty" And boolValue = True Then
            Return faultStatus
        Else
            Return okStatus
        End If

    End Function

    ''' <summary>
    ''' Allows Property Address to be updated from multiple areas in code
    ''' </summary>
    ''' <param name="updProperty">Property object polulated with address that needs to be displayed</param>
    ''' <remarks></remarks>
    Private Sub RefereshScreenPropertyAddress(updProperty As PropertyBase)
        If updProperty IsNot Nothing Then
            If String.IsNullOrEmpty(updProperty.Address2) Then
                lblPropertyAddressValue.Text = String.Format("{0}, {1}, {2}, {3}", updProperty.Address1, updProperty.Address3, updProperty.Address4, updProperty.Postcode)
            Else
                lblPropertyAddressValue.Text = String.Format("{0}, {1}, {2}, {3}, {4}", updProperty.Address1, updProperty.Address2, updProperty.Address3, updProperty.Address4, updProperty.Postcode)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Updates the Address of the property.  The interface has a checkbox to indicate whether to use the Account address.   This is used to set the address before it is updated.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdateAddress_Click(sender As Object, e As EventArgs) Handles btnUpdateAddress.Click

        Dim propertyUpdater As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
        Dim updProperty As IntamacBL_SPR.PropertyBase = propertyUpdater.Load(mAccountID, mPropertyID)

        If updProperty IsNot Nothing Then

            If chkUseAccountAddress.Checked = True Then
                updProperty.Address1 = txtEditAccountAddress1.Text
                updProperty.Address2 = txtEditAccountAddress2.Text
                updProperty.Address3 = txtEditAccountAddress3.Text
                updProperty.Address4 = txtEditAccountAddress4.Text
                updProperty.Postcode = txtEditAccountPostCode.Text
            Else
                updProperty.Address1 = txtEditPropertyAddress1.Text
                updProperty.Address2 = txtEditPropertyAddress2.Text
                updProperty.Address3 = txtEditPropertyAddress3.Text
                updProperty.Address4 = txtEditPropertyAddress4.Text
                updProperty.Postcode = txtEditPropertyPostCode.Text
            End If

            Dim blnIsRiskLevel As Boolean = False

            ' the risk level combo will be visible if the distributor supports risk levels
            If cboEditPropertyRiskLevel.Visible AndAlso Not String.IsNullOrEmpty(cboEditPropertyRiskLevel.SelectedValue) Then
                blnIsRiskLevel = True
                updProperty.RiskLevel = CShort(cboEditPropertyRiskLevel.SelectedValue)
            End If

            'update property
            propertyUpdater.Update(updProperty)

            'audit log of saving property address
            miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, e_SPR_AuditActionTypeID.Admin_Property_Updated,
                                                                                       String.Format("Address1: {0}, Address2: {1}, Address3: {2}, Address4: {3}, PostCode: {4}", updProperty.Address1, updProperty.Address2, updProperty.Address3, updProperty.Address4, updProperty.Postcode))
            If blnIsRiskLevel = True Then
                'audit log of saving risk level
                miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, e_SPR_AuditActionTypeID.Admin_Risk_Level_Updated,
                                                                                           String.Format("Risk Level: {0}", cboEditPropertyRiskLevel.SelectedItem.Text))
            End If

            RefereshScreenPropertyAddress(updProperty)

        End If


    End Sub


#Region "RadGrid Functions"

    ''' <summary>
    ''' Used by radcombo boxes to filter Zone column in grids
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub cboZoneSearch_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        GetSensors()
    End Sub

    ''' <summary>
    ''' Used by radcombo boxes to filter Fault column in grids
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub cboFaultSearch_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        GetSensors()
    End Sub

    ''' <summary>
    ''' Used by the Gateway zone dialog to delete a zone
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim objPropDeviceGetter As PropertyDevice = Nothing
        Dim mCompany As IntamacBL_SPR.MasterCompanySPR = Nothing

        objPropDeviceGetter = ObjectManager.CreatePropertyDevice(mCompanyType)

        'load the prop device record.  This contains an installer field which indicates who the installer is
        If objPropDeviceGetter.Load(mAccountID, mPropertyID, 1) Then
            objPropDeviceGetter.Delete()
            miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, e_SPR_AuditActionTypeID.Admin_Sensor_Removed, String.Format("{0} - {1}", objPropDeviceGetter.MACAddress, objPropDeviceGetter.RoomName))
        End If
        Response.Redirect("AccountSearch.aspx", True)

    End Sub

    ''' <summary>
    ''' Used by the Gateway zone dialog to update a zone
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdateGateway_Click(sender As Object, e As EventArgs) Handles btnUpdateGateway.Click

        'Only update if validation is met
        If Page.IsValid Then
            Dim objPropDeviceGetter As PropertyDevice = Nothing
            Dim mCompany As IntamacBL_SPR.MasterCompanySPR = Nothing

            objPropDeviceGetter = ObjectManager.CreatePropertyDevice(mCompanyType)

            'load the prop device record.  This contains an installer field which indicates who the installer is
            If objPropDeviceGetter.Load(mAccountID, mPropertyID, objPropDeviceGetter.DeviceSeq) Then

                Dim objHub As New SprueHub
                objHub.Load(mAccountID, mPropertyID, objPropDeviceGetter.DeviceSeq)

                If objHub IsNot Nothing Then

                    Dim installerID As Integer = 0

                    If mCurrentMasterUser IsNot Nothing Then
                        installerID = mCurrentMasterUser.UserID
                    End If

                    'need to update area
                    Dim intAreaID As Integer = GetCreateAreaID(cboGatewayZone)

                    objHub.MACAddress = objHub.MACAddress

                    'update hub
                    objHub.UpdateGateway(objPropDeviceGetter.AccountID, objPropDeviceGetter.PropertyID, objPropDeviceGetter.DeviceSeq, intAreaID, txtDeviceLocation1.Text, objPropDeviceGetter.InstallerID)

                    GetGateway()

                    'Close the modal popup
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeModalWindow", "CloseModal('ModalEditGateway');", True)
                End If
            End If
        End If
    End Sub


    ''' <summary>
    ''' Used by the edit zone dialog to update a zone
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdateSensor_Click(sender As Object, e As EventArgs) Handles btnUpdateSensor.Click

        'Only update if validation is met
        If Page.IsValid Then

            Dim propZone As Integer = hdnPropZone.Value
            Dim deviceSeq As Integer = hdnDeviceSeq.Value

            Dim propZoneUpdater As PropZone = ObjectManager.CreatePropZone(mCompanyType)
            Dim updZone As PropZone = ObjectManager.CreatePropZone(mCompanyType)

            Dim updZoneList As List(Of PropZone) = updZone.Load(mAccountID, mPropertyID, deviceSeq, propZone)
            Dim ReqUpdZone As New PropZoneSPR
            If updZoneList IsNot Nothing AndAlso updZoneList.Count > 0 Then

                ReqUpdZone = updZoneList.Item(0)

                If ReqUpdZone.IsColdSensor() Then
                    'Update cold alarm details
                    If chkSounder.Checked Then
                        ReqUpdZone.Mute = 0
                    Else
                        ReqUpdZone.Mute = 1
                    End If

                    ReqUpdZone.TempRange = drpRadTemperature.SelectedValue
                End If

            End If

            'Set Zone fields from entered text
            ReqUpdZone.RoomName = txtDeviceLocation.Text

            ReqUpdZone.AreaID = GetCreateAreaID(cboDeviceZone)

            propZoneUpdater.Update(mAccountID, CInt(hdnPropZone.Value), mPropertyID,
                                   CInt(hdnDeviceSeq.Value),
                                   ReqUpdZone,
                                   includeAllPropertiesInPayload:=ReqUpdZone.IsColdSensor())

            miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, e_SPR_AuditActionTypeID.Admin_Sensor_Details_Updated,
                                                                                        String.Format("Zone: {0}, Device: {1}, Sequence: {2}", ReqUpdZone.AreaID, hdnPropZone.Value, hdnDeviceSeq.Value))
            GetSensors()

            'Close the modal popup
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeModalWindow", "CloseModal('ModalEditSensor');", True)

        End If

    End Sub

    ''' <summary>
    ''' Set the image depending on the value of the field
    ''' </summary>
    ''' <param name="strStatus">Field Value</param>
    ''' <returns>Image URL as a string</returns>
    ''' <remarks></remarks>
    Private Function SetStatusImage(strStatus As String) As String
        Select Case strStatus

            Case PageString("OK").ToUpper
                Return "~/common/img/tick.png"
            Case PageString("FAULT").ToUpper
                Return "~/common/img/aleart.png"
            Case PageString("ALERT").ToUpper
                Return "~/common/img/alarm.png"
            Case Else
                Return "~/common/img/tick.png"

        End Select
    End Function

    ''' <summary>
    ''' Set the CSS depending on text
    ''' </summary>
    ''' <param name="strText">Text of object</param>
    ''' <returns>css class as string</returns>
    ''' <remarks></remarks>
    Private Function SetCSSonText(strText As String) As String
        Select Case strText

            Case PageString("OK").ToUpper
                Return "btn btn-success btnOK"
            Case PageString("FAULT").ToUpper
                Return "btn btn-warning btnFault"
            Case PageString("ALERT").ToUpper
                Return "btn btn-danger btnAlert"
            Case Else
                Return ""

        End Select
    End Function

    ''' <summary>
    ''' Set the Sensor Image based on the Sensor Type
    ''' </summary>
    ''' <param name="strSensorType">Sensor Type as string</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSensorImage(strSensorType As String) As String
        Select Case strSensorType

            Case e_AlarmSensorTypes.PadStrobe.ToString
                Return "~/common/img/ZoneDevices/strobe-pad.png"

            Case e_AlarmSensorTypes.ColdAlarm.ToString
                Return "~/common/img/ZoneDevices/cold-sensor-dc.png"

            Case e_AlarmSensorTypes.SmokeAlarm.ToString
                Return "~/common/img/ZoneDevices/smoke-sensor-dc.png"

            Case e_AlarmSensorTypes.HeatAlarm.ToString
                Return "~/common/img/ZoneDevices/heat-sensor-dc.png"

            Case e_AlarmSensorTypes.COAlarm.ToString
                Return "~/common/img/ZoneDevices/co-sensor-dc.png"

            Case e_AlarmSensorTypes.ACSmokeAlarm.ToString
                Return "~/common/img/ZoneDevices/smoke-sensor-ac.png"

            Case e_AlarmSensorTypes.ACHeatAlarm.ToString
                Return "~/common/img/ZoneDevices/heat-sensor-ac.png"

            Case e_AlarmSensorTypes.LowFreqSounder.ToString
                Return "~/common/img/ZoneDevices/low-frequency.png"

            Case e_AlarmSensorTypes.AlarmControlUnit.ToString
                Return "~/common/img/ZoneDevices/alarm-control-unit.png"

            Case e_AlarmSensorTypes.Unknown.ToString
                Return "~/common/img/ZoneDevices/Unknown.png"

            Case e_AlarmSensorTypes.InterfaceGateway.ToString
                Return "~/common/img/ZoneDevices/interface-gateway.png"

            Case e_AlarmSensorTypes.InterfaceGateway200.ToString
                Return "~/common/img/ZoneDevices/interface-gateway.png"

            Case e_DeviceID.RNDV2.ToString
                Return "~/common/img/gateway.png"
            Case Else
                Return ""

        End Select
    End Function

    ''' <summary>
    ''' Used for the Raise Ticket button to pass information to Support Detail page.   The data passed is pulled from e.CommandArguement which is a comma seperated list of 
    ''' 1. Device Seq and 2. PropZone
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Sensors_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles radGridSensors.ItemCommand, radGridUnregisteredSensors.ItemCommand, radGridArchivedSensors.ItemCommand, radGateSensors.ItemCommand

        Select Case e.CommandName

            Case "RaiseTicket"
                Dim objCommandArgument() As String = CStr(e.CommandArgument).Split(",")
                mSuppReqID = 0
                If Not String.IsNullOrEmpty(objCommandArgument(0).Trim) Then
                    mAreaID = CInt(objCommandArgument(0).Trim)
                End If
                mDeviceID = objCommandArgument(1)
                Response.Redirect("SupportDetail.aspx", True)

            Case "EventLog"

                mDeviceID = e.CommandArgument
                Response.Redirect("EventLog.aspx", True)

        End Select

    End Sub

#Region "Gateway"

    Private Sub radGateSensors_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radGateSensors.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As DataRowView = CType(e.Item.DataItem, Data.DataRowView)
            Dim lblLocation As Label = CType(e.Item.FindControl("lblLocation"), Label)
            Dim btnGatewayState As Button = CType(e.Item.FindControl("btnGatewayState"), Button)
            'Dim lblZone As Label = CType(e.Item.FindControl("lblZone"), Label)
            Dim btnFaultStatus As Button = CType(e.Item.FindControl("btnFaultStatus"), Button)
            Dim lblgatewayStatus As Label = CType(e.Item.FindControl("lblgatewayStatus"), Label)

            If lblLocation IsNot Nothing Then
                lblLocation.Text = IIf(dataItem("IntaRoomName") = "", GetLocalResourceObject("Unknown"), dataItem("IntaRoomName"))
            End If
            If lblgatewayStatus Is Nothing Then
                lblgatewayStatus.Text = ""
            Else
                If (lblgatewayStatus.Text.ToLower = "true") Then
                    lblgatewayStatus.Text = GetLocalResourceObject("Online")
                    'If Gateway is online show Ok text otherwise show Fault button 
                    btnGatewayState.Visible = True
                    btnFaultStatus.Visible = False

                Else
                    lblgatewayStatus.Text = GetLocalResourceObject("Offline")
                    'If Gateway is online show Ok text otherwise show Fault button 
                    btnFaultStatus.Visible = True
                    btnGatewayState.Visible = False

                End If
            End If


            'If lblZone IsNot Nothing Then
            '    lblZone.Text = IIf(dataItem("AreaName") = "", GetLocalResourceObject("Unknown"), dataItem("AreaName"))
            'End If

            'If btnFaultStatus IsNot Nothing Then
            '    btnFaultStatus.Text = dataItem("FaultStatus")
            '    btnFaultStatus.CssClass = SetCSSonText(dataItem("FaultStatus"))
            'End If



        End If
    End Sub
#End Region

#Region "Registered Sensors"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub radGridSensors_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles radGridSensors.ItemDataBound, radGridUnregisteredSensors.ItemDataBound, radGridArchivedSensors.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As DataRowView = CType(e.Item.DataItem, Data.DataRowView)
            Dim lblZone As Label = CType(e.Item.FindControl("lblZone"), Label)
            Dim lblLocation As Label = CType(e.Item.FindControl("lblLocation"), Label)
            Dim lblDeviceType As Label = CType(e.Item.FindControl("lblDeviceType"), Label)
            Dim lblDeviceDetails As Label = CType(e.Item.FindControl("lblDeviceDetails"), Label)
            Dim lblDeviceTypeImageinfo As Label = CType(e.Item.FindControl("lblDeviceTypeImageinfo"), Label)
            Dim imgBatteryStatus As Image = CType(e.Item.FindControl("imgBatteryStatus"), Image)
            Dim imgChamberSensorStatus As Image = CType(e.Item.FindControl("imgChamberSensorStatus"), Image)
            Dim imgUnitOnBase As Image = CType(e.Item.FindControl("imgUnitOnBase"), Image)
            Dim btnFaultStatus As Button = CType(e.Item.FindControl("btnFaultStatus"), Button)
            Dim imgSensor As Image = CType(e.Item.FindControl("imgSensor"), Image)

            Dim sensorType As String = If(Not (IsDBNull(dataItem("Sensortype")) OrElse String.IsNullOrEmpty(dataItem("SensorType"))), dataItem("SensorType"), CInt(e_AlarmSensorTypes.Unknown).ToString)
            If lblDeviceType IsNot Nothing Then
                lblDeviceType.Text = DeviceTypeGet(sensorType)
            End If

            If lblDeviceTypeImageinfo IsNot Nothing Then
                lblDeviceTypeImageinfo.Text = SensorTypeGet(sensorType)
            End If

            If lblDeviceDetails IsNot Nothing Then
                lblDeviceDetails.Text = SensorTypeDetails(sensorType)
            End If

            If imgSensor IsNot Nothing Then
                imgSensor.ImageUrl = SetSensorImage(sensorType)
            End If

            If lblZone IsNot Nothing Then
                lblZone.Text = If(IsDBNull(dataItem("AreaName")) OrElse String.IsNullOrEmpty(dataItem("AreaName")), GetLocalResourceObject("Unknown"), dataItem("AreaName"))
            End If

            If lblLocation IsNot Nothing Then
                lblLocation.Text = If(IsDBNull(dataItem("IntaRoomName")) OrElse String.IsNullOrEmpty(dataItem("IntaRoomName")), GetLocalResourceObject("Unknown"), dataItem("IntaRoomName"))
            End If

            If imgBatteryStatus IsNot Nothing AndAlso Not IsDBNull(dataItem("BatteryStatus")) Then
                imgBatteryStatus.ImageUrl = SetStatusImage(dataItem("BatteryStatus"))
            End If

            If imgChamberSensorStatus IsNot Nothing AndAlso Not IsDBNull(dataItem("ChamberSensorStatus")) Then
                imgChamberSensorStatus.ImageUrl = SetStatusImage(dataItem("ChamberSensorStatus"))
            End If

            If imgUnitOnBase IsNot Nothing AndAlso Not IsDBNull(dataItem("UnitOnBase")) Then
                imgUnitOnBase.ImageUrl = SetStatusImage(dataItem("UnitOnBase"))
            End If

            If btnFaultStatus IsNot Nothing Then
                If Not IsDBNull(dataItem("FaultStatus")) Then
                    btnFaultStatus.Text = dataItem("FaultStatus")
                    btnFaultStatus.CssClass = SetCSSonText(dataItem("FaultStatus"))
                Else
                    btnFaultStatus.Visible = False
                End If
            End If

        End If
    End Sub

    ''' <summary>
    ''' Used to populate the search dropdowns on the zones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub radGridSensors_PreRender(sender As Object, e As EventArgs) Handles radGridSensors.PreRender, radGridUnregisteredSensors.PreRender, radGridArchivedSensors.PreRender
        Dim grid As RadGrid = DirectCast(sender, RadGrid)
        Dim headerItem As GridHeaderItem = CType(grid.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)

        If headerItem IsNot Nothing AndAlso m_Areas IsNot Nothing Then

            Dim cboZoneSearch As RadComboBox = CType(headerItem.FindControl("cboZoneSearch"), RadComboBox)
            Dim cboFaultSearch As RadComboBox = CType(headerItem.FindControl("cboFaultSearch"), RadComboBox)

            If cboZoneSearch IsNot Nothing Then

                cboZoneSearch.Items.Add(New RadComboBoxItem(GetLocalResourceObject("AllZones"), "All"))
                For Each thisArea As Areas In m_Areas
                    cboZoneSearch.Items.Add(New RadComboBoxItem(thisArea.Name, thisArea.Name))
                Next

                If SelectedZone IsNot Nothing Then
                    cboZoneSearch.SelectedValue = SelectedZone
                Else
                    cboZoneSearch.SelectedIndex = 0
                End If
            End If

            If cboFaultSearch IsNot Nothing Then

                cboFaultSearch.Items.Add(New RadComboBoxItem(GetLocalResourceObject("AllFaults"), "All"))
                cboFaultSearch.Items.Add(New RadComboBoxItem(GetGlobalResourceObject("PageGlobalResources", "OKText"), GetGlobalResourceObject("PageGlobalResources", "OKText")))
                cboFaultSearch.Items.Add(New RadComboBoxItem(GetLocalResourceObject("Fault"), GetLocalResourceObject("Fault")))
                cboFaultSearch.Items.Add(New RadComboBoxItem(GetLocalResourceObject("Alert"), GetLocalResourceObject("Alert")))


                If mAccountDetailTileSelected.HasValue Then
                    'SP-99 Start
                    If mAccountDetailTileSelected = Dashboard.Tiles.ActiveAlerts Then
                        cboFaultSearch.SelectedIndex = cboFaultSearch.Items.FindItemByText(GetLocalResourceObject("Alert")).Index
                    End If
                    If mAccountDetailTileSelected = Dashboard.Tiles.ActiveFaults Then
                        cboFaultSearch.SelectedIndex = cboFaultSearch.Items.FindItemByText(GetLocalResourceObject("Fault")).Index
                    End If
                    If mAccountDetailTileSelected = Dashboard.Tiles.NoFaults Then
                        cboFaultSearch.SelectedIndex = cboFaultSearch.Items.FindItemByText(GetGlobalResourceObject("PageGlobalResources", "OKText")).Index
                    End If
                    If mAccountDetailTileSelected = Dashboard.Tiles.TotalDevices Then
                        cboFaultSearch.SelectedIndex = cboFaultSearch.Items.FindItemByText(GetLocalResourceObject("AllFaults")).Index
                    End If
                    'SP-99 End
                Else

                    If SelectedFaultStatus IsNot Nothing Then
                        cboFaultSearch.SelectedValue = SelectedFaultStatus
                    Else
                        cboFaultSearch.SelectedIndex = 0
                    End If

                End If

            End If


        End If

    End Sub

#End Region



#End Region


    Private Sub timerRefresh_Tick(sender As Object, e As EventArgs) Handles timerRefresh.Tick

        GetGateway()
        GetSensors()
        updEditSensor.Update()
        updEditGateway.Update()
    End Sub

    ''' <summary>
    ''' Get the sensor type name from the resources based on sensor type
    ''' </summary>
    ''' <param name="strSensorType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Function SensorTypeGet(strSensorType As String) As String
        Select Case strSensorType

            Case e_AlarmSensorTypes.ColdAlarm.ToString
                'Extreme Temperature Alarm
                Return PageString("ColdAlarm")

            Case e_AlarmSensorTypes.SmokeAlarm.ToString
                'Smoke Alarm
                Return PageString("SmokeAlarm")

            Case e_AlarmSensorTypes.HeatAlarm.ToString
                'Heat Alarm
                Return PageString("HeatAlarm")

            Case e_AlarmSensorTypes.COAlarm.ToString
                'Carbon Monoxide Alarm
                Return PageString("COAlarm")

            Case e_AlarmSensorTypes.ACSmokeAlarm.ToString
                '230V - Smoke Alarm
                Return PageString("ACSmokeAlarm")

            Case e_AlarmSensorTypes.ACHeatAlarm.ToString
                '230V - Heat Alarm
                Return PageString("ACHeatAlarm")

            Case e_AlarmSensorTypes.PadStrobe.ToString
                'Strobe & Vibrating Pad
                Return PageString("PadStrobe")

            Case e_AlarmSensorTypes.LowFreqSounder.ToString
                'Low Frequency Sounder
                Return PageString("LowFreqSounder")

            Case e_AlarmSensorTypes.AlarmControlUnit.ToString
                'Alarm Control Unit
                Return PageString("AlarmControlUnit")

            Case e_AlarmSensorTypes.Unknown.ToString
                'Unknown Sensor
                Return PageString("Unknown")

            Case e_AlarmSensorTypes.InterfaceGateway.ToString
                'Interface Gateway
                Return PageString("InterfaceGateway")

            Case e_AlarmSensorTypes.InterfaceGateway200.ToString
                'Interface Gateway 200
                Return PageString("InterfaceGateway")

            Case e_DeviceID.RNDV2.ToString
                Return PageString("GatewayDetails")

            Case Else
                Return ""

        End Select
    End Function


    Private Function SensorTypeDetails(strSensorType As String) As String
        Select Case strSensorType

            Case e_AlarmSensorTypes.ColdAlarm.ToString
                'Extreme Temperature Alarm
                Return PageString("ColdAlarmDetails")

            Case e_AlarmSensorTypes.SmokeAlarm.ToString
                'Smoke Alarm
                Return PageString("SmokeAlarmDetails")

            Case e_AlarmSensorTypes.HeatAlarm.ToString
                'Heat Alarm
                Return PageString("HeatAlarmDetails")

            Case e_AlarmSensorTypes.COAlarm.ToString
                'Carbon Monoxide Alarm
                Return PageString("COAlarmDetails")

            Case e_AlarmSensorTypes.ACSmokeAlarm.ToString
                '230V - Smoke Alarm
                Return PageString("ACSmokeAlarmDetails")

            Case e_AlarmSensorTypes.ACHeatAlarm.ToString
                '230V - Heat Alarm
                Return PageString("ACHeatAlarmDetails")

            Case e_AlarmSensorTypes.PadStrobe.ToString
                'Strobe & Vibrating Pad
                Return PageString("PadStrobeDetails")

            Case e_AlarmSensorTypes.LowFreqSounder.ToString
                'Low Frequency Sounder
                Return PageString("LowFreqSounderDetails")

            Case e_AlarmSensorTypes.AlarmControlUnit.ToString
                'Alarm Control Unit
                Return PageString("AlarmControlUnitDetails")

            Case e_AlarmSensorTypes.Unknown.ToString
                'Unknown Sensor
                Return PageString("UnknownDetails")

            Case e_AlarmSensorTypes.InterfaceGateway.ToString
                'Interface Gateway
                Return PageString("InterfaceGatewayDetails")

            Case e_AlarmSensorTypes.InterfaceGateway200.ToString
                'Interface Gateway 200
                Return PageString("InterfaceGatewayDetails")

            Case e_DeviceID.RNDV2.ToString
                Return PageString("Gateway")

            Case Else
                Return ""

        End Select
    End Function


    ''' <summary>
    ''' Get the device type from the resources based on sensor type
    ''' </summary>
    ''' <param name="strSensorType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Function DeviceTypeGet(strSensorType As String) As String
        Select Case strSensorType

            Case e_AlarmSensorTypes.ColdAlarm.ToString
                'Extreme Temperature Alarm   WETA-10X
                Return PageString("ColdAlarmType")

            Case e_AlarmSensorTypes.SmokeAlarm.ToString
                'Smoke Alarm      WST-630
                Return PageString("SmokeAlarmType")

            Case e_AlarmSensorTypes.HeatAlarm.ToString
                'Heat Alarm   WHT-630
                Return PageString("HeatAlarmType")

            Case e_AlarmSensorTypes.COAlarm.ToString
                'Carbon Monoxide Alarm     W2-CO-10X 
                Return PageString("COAlarmType")

            Case e_AlarmSensorTypes.ACSmokeAlarm.ToString
                '230V - Smoke Alarm    WSM-F-1EU 
                Return PageString("ACSmokeAlarmType")

            Case e_AlarmSensorTypes.ACHeatAlarm.ToString
                '230V - Heat Alarm      WHM-F-1EU 
                Return PageString("ACHeatAlarmType")

            Case e_AlarmSensorTypes.PadStrobe.ToString
                'Strobe & Vibrating Pad    W2-SVP-630
                Return PageString("PadStrobeType")

            Case e_AlarmSensorTypes.LowFreqSounder.ToString
                'Low Frequency Sounder     W2-LFS-630
                Return PageString("LowFreqSounderType")

            Case e_AlarmSensorTypes.AlarmControlUnit.ToString
                'Alarm Control Unit	       WTSL-F-1EU
                Return PageString("AlarmControlUnitType")

            Case e_AlarmSensorTypes.Unknown.ToString
                'Unknown Sensor       
                Return PageString("Unknown")

            Case e_AlarmSensorTypes.InterfaceGateway.ToString
                'Interface Gateway         IFG100
                Return PageString("InterfaceGatewayType")

            Case e_AlarmSensorTypes.InterfaceGateway200.ToString
                'Interface Gateway         IFG200
                Return PageString("InterfaceGateway200Type")

            Case e_DeviceID.RNDV2.ToString
                Return PageString("GatewayDeviceType")

            Case Else
                Return ""

        End Select
    End Function

    Private Sub InitEditPropertyRiskLevels()
        ' hide risk level combo is distributor doesn't support it
        ' note: the "visible" status is checked when saving to see if it's required or not
        If Not AllowRiskLevelEditing() Then
            cboEditPropertyRiskLevel.Visible = False
            Return
        End If
        cboEditPropertyRiskLevel.Visible = True
        cboEditPropertyRiskLevel.DataSource = GetRiskLevelValues()
        cboEditPropertyRiskLevel.DataBind()
    End Sub

    Private Function AllowRiskLevelEditing() As Boolean
        If String.IsNullOrEmpty(mAccountID) Then
            Return False
        End If
        Dim company As MasterCompany = ObjectManager.CreateMasterCompany(SharedStuff.e_CompanyType.SPR)
        Return company.AreRiskLevelsAllowedForAccount(mAccountID)
    End Function

    Private Function GetRiskLevelValues() As IEnumerable(Of Object)
        Return From r In [Enum].GetValues(GetType(e_RiskLevel)).Cast(Of e_RiskLevel)()
               Select New With {
                    .Id = CInt(r),
                    .Text = GetText(r)
                   }
    End Function

    Private Function GetText(riskLevel As e_RiskLevel) As String
        Dim translated = GetLocalResourceObject("RiskLevel" & riskLevel.ToString())
        If translated Is Nothing Then
            Return riskLevel.ToString()
        Else
            Return translated.ToString()
        End If
    End Function


End Class