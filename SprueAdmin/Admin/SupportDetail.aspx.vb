Imports System.Web.Configuration
Imports IntamacBL_SPR
Imports IntamacShared_SPR
Imports IntamacShared_SPR.SharedStuff
Imports SprueAdmin.Resources

Imports Telerik.Web.UI

Partial Class Admin_SupportDetail
    Inherits CultureBaseClass



    Private Property ignoreGateway As Boolean
        Get
            If ViewState("ignoreGateway") IsNot Nothing Then
                Return CBool(ViewState("ignoreGateway"))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("ignoreGateway") = value
        End Set
    End Property

#Region "Private Methods"

    ''' <summary>
    ''' Populates the 'ticket status' drop list
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillStatus()

        'populate & bind
        cboStatus.DataSource = miscFunctions.GetSupportStatuses()
        cboStatus.DataBind()

    End Sub

    ''' <summary>
    ''' Gets property (if mPropertyID is set) and populates display labels
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayProperty()
        divAddress.Visible = False

        If Not String.IsNullOrEmpty(mPropertyID) Then
            Dim propertyGetter As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
            Dim objProperty As IntamacBL_SPR.PropertyBase = propertyGetter.Load(mAccountID, mPropertyID)

            If objProperty IsNot Nothing Then
                lblAddress1.Text = objProperty.Address1
                lblAddress2.Text = objProperty.Address2
                lblAddress3.Text = objProperty.Address3
                lblAddress4.Text = objProperty.Address4

                lblPostCode5.Text = objProperty.Postcode

                divAddress.Visible = True

                If lblPostcodeDisplay.Visible Then
                    lblPostcodeDisplay.Text = objProperty.Postcode
                Else
                    If cboPostcode.Visible = True Then
                        For Each pcodeItem As RadComboBoxItem In cboPostcode.Items
                            If pcodeItem.Value.Contains(objProperty.Postcode) Then
                                pcodeItem.Selected = True
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Loads the sensors for the selected 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overloads Sub LoadDevices(ByVal useLabel As Boolean)

        cboDeviceIDSelect.Enabled = False
        cboDeviceIDSelect.Items.Clear()
        cboDeviceIDSelect.Text = ""
        Dim lstAreas As List(Of IntamacBL_SPR.Areas) = Nothing
        Dim selZone As PropZone
        Dim blnShowingDeviceCombo = True
        lblDeviceTypeDisplay.Style("visibility") = "hidden"

        lblDeviceTypeDisplay.Text = String.Empty

        If Not (String.IsNullOrEmpty(mAccountID) OrElse String.IsNullOrEmpty(mPropertyID)) Then
            ' will only use the label if we have an indicated target area
            'useLabel = useLabel And Not String.IsNullOrEmpty(mDeviceID)

            Dim propZoneGetter As PropZone = ObjectManager.CreatePropZone(mCompanyType)
            Dim objZones As List(Of PropZone) = propZoneGetter.Load(mAccountID, mPropertyID)

            Dim area As IntamacBL_SPR.Areas = Nothing
            Dim deviceSelValue As String = String.Empty

            area = New IntamacBL_SPR.Areas

            lstAreas = area.Load(mAccountID, mPropertyID)

            If objZones IsNot Nothing Then

                If Not useLabel AndAlso objZones.Count > 0 Then
                    For Each deviceZone As PropZone In objZones.OrderBy(Function(rec) rec.DeviceID)
                        Dim strArea As String = ""
                        Dim strAreaID As String = ""

                        If deviceZone.AreaID.HasValue AndAlso deviceZone.AreaID <> 0 Then
                            Dim selArea As Areas = Nothing
                            Try
                                lstAreas.First(Function(a) a.ID = deviceZone.AreaID)

                                If selArea IsNot Nothing Then
                                    strArea = selArea.Name
                                    strAreaID = selArea.ID.ToString
                                End If
                            Catch
                                '  may be that Area no longer exists
                            End Try

                        End If

                        Dim zoneItem As RadComboBoxItem = New RadComboBoxItem(String.Format("{0}{1}{2}", deviceZone.DeviceID, IIf(Not (String.IsNullOrEmpty(deviceZone.RoomName) OrElse String.IsNullOrEmpty(deviceZone.DeviceID)), " - ", String.Empty), deviceZone.RoomName),
                                                                              getDeviceString(deviceZone, lstAreas))
                        cboDeviceIDSelect.Items.Add(zoneItem)
                        If Not String.IsNullOrEmpty(mDeviceID) AndAlso deviceZone.DeviceID = mDeviceID Then
                            selZone = deviceZone
                            zoneItem.Selected = True
                        End If
                    Next
                    cboDeviceIDSelect.Enabled = cboDeviceIDSelect.Items.Count > 0
                    If ignoreGateway Then
                        selZone = Nothing
                    End If
                End If

                If useLabel AndAlso objZones.Count > 0 Then

                    Try
                        selZone = objZones.First(Function(z) z.DeviceID = mDeviceID)
                        If selZone IsNot Nothing Then
                            lblDeviceIDDisplay.Text = selZone.DeviceID
                            blnShowingDeviceCombo = False
                            lblDeviceTypeDisplay.Style("visibility") = "visible"

                        End If
                    Catch ex As Exception

                    End Try


                End If
            End If
        End If
        setDeviceLabels(getDeviceString(selZone, lstAreas))
        cboDeviceIDSelect.Visible = blnShowingDeviceCombo AndAlso cboDeviceIDSelect.Items.Count > 0
    End Sub

    Private Function getDeviceString(ByVal deviceZone As PropZone, lstAreas As List(Of Areas)) As String
        Dim strArea As String = ""
        Dim strAreaID As String = ""
        Dim strRetval = "|||"

        If deviceZone IsNot Nothing Then
            If deviceZone.AreaID.HasValue AndAlso deviceZone.AreaID <> 0 Then
                Try
                    Dim selArea As Areas = lstAreas.First(Function(a) a.ID = deviceZone.AreaID)

                    If selArea IsNot Nothing Then
                        strArea = selArea.Name
                        strAreaID = selArea.ID.ToString
                    End If
                Catch
                End Try


            End If

            strRetval = String.Format("{0}|{1}|{2}|{3}", deviceZone.DeviceID, miscFunctions.GetSensorType(deviceZone.SensorType), strArea, strAreaID)
        End If

        Return strRetval
    End Function
    Private Sub setDeviceLabels(ByVal deviceString As String)
        lblDeviceTypeDisplay.Text = String.Empty

        If Not String.IsNullOrEmpty(deviceString) Then

            Dim valueSplit As String() = deviceString.Split("|"c)

            If valueSplit.Length > 0 Then
                lblDeviceTypeDisplay.Text = PageString(valueSplit(1) + "Type")
                If valueSplit.Length > 1 Then
                    lblZoneDisplay.Text = valueSplit(2)
                End If

            End If

        End If

        'if the Device Type has not been determined then hide the field
        lblDeviceTypeDisplay.Style("visibility") = IIf(String.IsNullOrEmpty(lblDeviceTypeDisplay.Text), "hidden", "visible")
        lblZoneDisplay.Style("visibility") = IIf(String.IsNullOrEmpty(lblZoneDisplay.Text), "hidden", "visible")

    End Sub

    Protected Overloads Sub LoadDevices()
        LoadDevices(False)
    End Sub

    ''' <summary>
    ''' Sets up the display/edit controls depending on the current 'page mode'
    ''' </summary>
    ''' <param name="objRequest"></param>
    ''' <remarks></remarks>
    Private Sub PopulateFields(ByVal objRequest As IntamacBL_SPR.SupportRequest, ByVal searchButtonPressed As Boolean)

        ' set Visible/Enabled state of relevant cotrols to 'false'
        cboAccountID.Visible = False
        cboPostcode.Visible = False
        cboLastname.Visible = False
        lblAccountIDdisplay.Visible = False
        lblPostcodeDisplay.Visible = False
        lblLastNameDisplay.Visible = False
        txtAccountEnter.Visible = False
        txtPostcodeEnter.Visible = False
        txtLastNameEnter.Visible = False

        divSearchResultInvalid.Style.Add("visibility", "hidden")

        cboGatewaySelect.Enabled = False
        cboDeviceIDSelect.Enabled = False

        If Not IsNothing(objRequest) Then
            ' editing an existing support request
            divNotNew.Visible = True
            lblRefNo.Text = CStr(objRequest.SupportRequestID)
            lblEnteredBy.Text = objRequest.EnteredBy
            lblDateEntered.Text = String.Format("{0:d} {0:t}", objRequest.DateEntered)
            divLastname.Visible = False
            divPostCode.Visible = False

            mAccountID = objRequest.AccountID
            lblAccountIDdisplay.Text = mAccountID
            lblAccountIDdisplay.Visible = True

            cboStatus.SelectedValue = CStr(objRequest.StatusID)
            UpdateStatusOptionsExistingTicket(objRequest)

            txtReference.Text = objRequest.OptionalRef

            mPropertyID = objRequest.PropertyID
            If Not String.IsNullOrEmpty(objRequest.Zone) Then
                mAreaID = CInt(objRequest.Zone)
            End If
            mDeviceID = objRequest.DeviceID
            PopulateNotes(objRequest)


        Else
            ' new request

            ' search values, populated from either the relevant combobox, or textbox, AccountID pre-populated with currently selected value
            Dim srchAccount As String = mAccountID
            Dim srchName As String = ""
            Dim srchPC As String = ""

            ' if an account is not already selected clear property,gateway,area and device 'selectors'
            If String.IsNullOrEmpty(srchAccount) Then
                mPropertyID = ""
                mSelectedGateway = ""
                mDeviceID = ""
                mAreaID = Nothing

            End If

            ' Dictionaries used to build the lists to be applied to the 'search' combo boxes
            ' dAccounts is used to store the AccountID and associated names and postcodes, key will be the AccountID, the value will contain (as a comma separted list) each of the postcodes and names referenced by this ID
            Dim dAccounts As New Dictionary(Of String, String)

            ' dPostCodes is used to store the Postcode and associated names, account and property ids, key will be the (canonicalised) PostCode, the value each of the account ids and names referenced by this postcode
            Dim dPostCodes As New Dictionary(Of String, String)

            ' dNames is used to store the AccountID and associated names and postcodes, key will be the Name, the value will contain (as a comma separted list) each of the postcodes and names referenced by this ID
            Dim dNames As New Dictionary(Of String, String)

            ' Get the entered search criteria
            If Not String.IsNullOrEmpty(cboAccountID.SelectedValue) Then
                srchAccount = cboAccountID.Text
            ElseIf Not String.IsNullOrEmpty(txtAccountEnter.Text) Then
                srchAccount = txtAccountEnter.Text
            End If

            If Not String.IsNullOrEmpty(cboLastname.SelectedValue) Then
                srchName = cboLastname.Text
            ElseIf Not String.IsNullOrEmpty(txtLastNameEnter.Text) Then
                srchName = txtLastNameEnter.Text
            End If

            If Not String.IsNullOrEmpty(cboPostcode.SelectedValue) Then
                srchPC = cboPostcode.Text
            ElseIf Not String.IsNullOrEmpty(txtPostcodeEnter.Text) Then
                srchPC = txtPostcodeEnter.Text
            End If

            If Not (String.IsNullOrEmpty(srchAccount) AndAlso String.IsNullOrEmpty(srchPC) AndAlso String.IsNullOrEmpty(srchName)) Then
                ' if any of the search criteria has been supplied, retrieve Client table entries to allow population of 'search' combos
                Dim propa As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")
                Dim installerId As Integer? = GetCurrentInstallerId()
                Dim companyId As Integer? = GetCurrentDistributorOrServiceProviderId()

                Dim clientData As DataTable = propa.LoadSearch(srchAccount, "", srchPC, srchName, "", 0, companyId, installerId,
                                  Date.MinValue, Date.MinValue, Date.MinValue, Date.MinValue, "", "",
                                  Nothing, Nothing, Nothing, Nothing, Nothing)

                If clientData IsNot Nothing AndAlso clientData.Rows.Count > 0 Then

                    For Each row As DataRow In clientData.Rows
                        Dim strAccountID = row("IntaAccountID").ToString
                        Dim strPostCodeOrig = row("IntaPostCode").ToString
                        Dim strPostCodePrc = strPostCodeOrig.ToLower.Replace(" "c, "")
                        Dim strNameOrig = row("IntaSurname").ToString
                        Dim strNamePrc = strNameOrig.ToLower
                        Dim strPropID As String = row("IntaPropertyID").ToString


                        If Not dAccounts.ContainsKey(strAccountID) Then
                            dAccounts.Add(strAccountID, String.Format("AI,{0},", strAccountID)) 'first entry identifies an Account entry (for client side JS), second the unprocessed name value
                        End If

                        If Not dPostCodes.ContainsKey(strPostCodePrc) Then
                            dPostCodes.Add(strPostCodePrc, String.Format("PC,{0},", strPostCodeOrig)) ' first entry identifies a postcode entry (for client side JS), second the unprocessed postcode value
                        End If
                        If Not dNames.ContainsKey(strNamePrc) Then
                            dNames.Add(strNamePrc, String.Format("LN,{0},", strNameOrig)) ' first entry identifies a name entry (for client side JS), second the unprocessed name value
                        End If

                        ' if required, add unprocessed postcode to relevant account entry
                        If Not dAccounts(strAccountID).Contains(strPostCodeOrig) Then
                            dAccounts(strAccountID) &= strPostCodeOrig & ","
                        End If
                        ' if required, add unprocessed name to relevant account entry
                        If Not dAccounts(strAccountID).Contains(strNameOrig) Then
                            dAccounts(strAccountID) &= strNameOrig & ","
                        End If

                        ' if required, add accountid to relevant postcode entry
                        If Not dPostCodes(strPostCodePrc).Contains(strAccountID) Then
                            dPostCodes(strPostCodePrc) &= strAccountID & ","
                        End If
                        ' if required, add unprocessed name to relevant postcode entry
                        If Not dPostCodes(strPostCodePrc).Contains(strNameOrig) Then
                            dPostCodes(strPostCodePrc) &= strNameOrig & ","
                        End If
                        ' if required, add property ID to relevant postcode entry
                        If Not dPostCodes(strPostCodePrc).Contains(strPropID) Then
                            dPostCodes(strPostCodePrc) &= strPropID & ","
                        End If

                        ' if required, add accountid to relevant name entry
                        If Not dNames(strNamePrc).Contains(strAccountID) Then
                            dNames(strNamePrc) &= strAccountID & ","
                        End If
                        ' if required, add unprocessed postcode to relevant name entry
                        If Not dNames(strNamePrc).Contains(strPostCodeOrig) Then
                            dNames(strNamePrc) &= strPostCodeOrig & ","
                        End If
                    Next

                Else

                    'search button was pressed and no search result was found.  Display an appropriate message to the user.
                    If searchButtonPressed Then divSearchResultInvalid.Style.Add("visibility", "none")

                End If
            End If

            ' clear combo boxes
            cboLastname.Items.Clear()
            cboPostcode.Items.Clear()
            cboAccountID.Items.Clear()
            cboGatewaySelect.Items.Clear()

            If dAccounts.Count > 0 Then
                If dAccounts.Count = 1 Then
                    ' only one account identified, show as a label
                    lblAccountIDdisplay.Text = dAccounts.Keys(0)
                    lblAccountIDdisplay.Visible = True
                    ' if there is only one account, set it as 'selected'
                    mAccountID = dAccounts.Keys(0)
                Else
                    ' multiple accounts, so populate combo list
                    For Each accountID As String In dAccounts.Keys
                        cboAccountID.Items.Add(New RadComboBoxItem(accountID, dAccounts(accountID)))
                    Next
                    cboAccountID.Visible = True

                End If
            End If

            If dPostCodes.Count > 0 Then
                Dim pcodeEntry As String = Nothing
                ' only one postcode identified, show as a label
                If dPostCodes.Count = 1 Then
                    ' only one choice, so use it
                    pcodeEntry = dPostCodes.Values(0)
                Else
                    ' property selected, so find correct postcode entry
                    If Not String.IsNullOrEmpty(mPropertyID) Then
                        ' property already selected, so find correct post code
                        Try
                            pcodeEntry = dPostCodes.First(Function(pc) pc.Value.Contains(mPropertyID)).Value
                        Catch
                        End Try

                    End If
                End If

                If Not String.IsNullOrEmpty(pcodeEntry) Then
                    ' have single postcode entry, so use second element, of list, from the value in dPostCodes (i.e. unprocessed)
                    lblPostcodeDisplay.Text = pcodeEntry.Split(","c)(1)
                    lblPostcodeDisplay.Visible = True
                Else
                    ' property not selected so populate combo list using all possibles
                    For Each postCode As String In dPostCodes.Keys
                        cboPostcode.Items.Add(New RadComboBoxItem(dPostCodes(postCode).Split(","c)(1), dPostCodes(postCode)))
                    Next
                    cboPostcode.Visible = True
                End If

            End If

            If dNames.Count > 0 Then
                If dNames.Count = 1 Then
                    ' use second entry, of list, from the value in dName (i.e. unprocessed)
                    lblLastNameDisplay.Text = dNames.Values(0).Split(","c)(1)
                    lblLastNameDisplay.Visible = True
                Else
                    ' multiple names, so populate combo list
                    For Each lastName As String In dNames.Keys
                        cboLastname.Items.Add(New RadComboBoxItem(dNames(lastName).Split(","c)(1), dNames(lastName)))
                    Next
                    cboLastname.Visible = True

                End If
            End If

            ' text box controls only visible if neither of the other options (label or combo) has been set visible
            txtPostcodeEnter.Visible = Not (lblPostcodeDisplay.Visible OrElse cboPostcode.Visible)
            txtLastNameEnter.Visible = Not (lblLastNameDisplay.Visible OrElse cboLastname.Visible)

            UpdateStatusOptionsNewTicket()

        End If


        ' AccountID text box only visible if neither of the other options (label or combo) has been set visible
        txtAccountEnter.Visible = Not (lblAccountIDdisplay.Visible OrElse cboAccountID.Visible)

        Dim lstMAC As New List(Of String)

        If Not String.IsNullOrEmpty(mAccountID) Then
            ' we have an account selected so find the gateways for it
            Dim gate As New IntamacBL_SPR.Gateways
            Dim lstGateways As List(Of IntamacBL_SPR.Gateway) = Nothing

            lstGateways = gate.Load(mAccountID, "", True)

            lblGatewayDisplay.Visible = False

            'iterate through any loaded gateways, in ascending order of MAC, and add them to the relevant dropdown
            For Each singleGateway As IntamacBL_SPR.Gateway In lstGateways.OrderBy(Function(rec) rec.MACAddress)

                'Set the text of the gateway dropdown to include the name and the mac address
                Dim GatewayText As String = IIf(String.IsNullOrEmpty(singleGateway.Name), "N/A", singleGateway.Name) & " - " & singleGateway.MACAddress

                Dim ValueText As String = singleGateway.PropertyID

                'SP-825 if single gateway then display label with gate way info , Otherwise showing list of gateways in the dropdown (OrElse lstGateways.Count = 1)
                If (Not (ignoreGateway OrElse String.IsNullOrEmpty(mSelectedGateway)) AndAlso mSelectedGateway = singleGateway.MACAddress) _
                    OrElse (mSuppReqID <> 0 AndAlso Not String.IsNullOrEmpty(mPropertyID) AndAlso singleGateway.PropertyID = mPropertyID) OrElse lstGateways.Count = 1 Then
                    ' if this gateway is selected (matches mSelectedGateway and/or mPropertyID) display as label
                    lblGatewayDisplay.Text = GatewayText
                    cboGatewaySelect.Visible = False
                    mPropertyID = singleGateway.PropertyID
                    lblGatewayDisplay.Visible = True
                    mSelectedGateway = singleGateway.MACAddress
                    Exit For
                Else
                    ' otherwise populate combo box
                    Dim newItem As RadComboBoxItem = New Telerik.Web.UI.RadComboBoxItem(GatewayText, ValueText)
                    cboGatewaySelect.Enabled = True
                    cboGatewaySelect.Items.Add(newItem)

                    If Not ignoreGateway AndAlso mSelectedGateway = singleGateway.MACAddress Then
                        newItem.Selected = True
                    End If
                End If
            Next

            If Not String.IsNullOrEmpty(mPropertyID) Then
                ' we have selected a specific property, so Load it's Devices (if it's not an existing request being edited) and address
                LoadDevices(objRequest IsNot Nothing OrElse (Not (IsPostBack OrElse String.IsNullOrEmpty(mDeviceID))))

                DisplayProperty()
            End If
        End If




    End Sub

    Private Sub UpdateStatusOptionsNewTicket()

        'update status options based on a new ticket and the type of user creating this ticket

        'remove "Closed" for brand new ticket
        Dim closedItem As ListItem = cboStatus.Items.FindByValue(miscFunctions.c_ClosedValue.ToString)
        cboStatus.Items.Remove(closedItem)

        If IsCurrentUserAnInstaller() Then

            'for installer, allow option of "Escalated to Service Provider" but show as "Escalate"
            UpdateEscalatedOptions(New String() {miscFunctions.c_EscalatedToServiceProviderValue.ToString})
            ReplaceEscalatedText(miscFunctions.c_EscalatedToServiceProviderValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

        ElseIf IsCurrentUserAServiceProvider() Then

            'for service provider, allow option of "Escalated to Distributor" but show as "Escalate"
            UpdateEscalatedOptions(New String() {miscFunctions.c_EscalatedToDistributorValue.ToString})
            ReplaceEscalatedText(miscFunctions.c_EscalatedToDistributorValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

        ElseIf IsCurrentUserADistributor() Then

            'for distributor, allow option of "Escalated to Sprue" but show as "Escalate"
            UpdateEscalatedOptions(New String() {miscFunctions.c_EscalatedToSprueValue.ToString})
            ReplaceEscalatedText(miscFunctions.c_EscalatedToSprueValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

        ElseIf IsCurrentUserAnApplicationOwner() Or IsCurrentUserASystemOwner() Then

            'for application owner or system owner, no escalate option
            UpdateEscalatedOptions(New String() {})

        End If

    End Sub

    Private Sub UpdateStatusOptionsExistingTicket(ByVal objRequest As SupportRequest)

        'update status options based on current status of the ticket and the type of user viewing the ticket

        'if now in "Escalated" state then ticket cannot be re-opened, so remove this option
        If CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToServiceProviderValue.ToString Or CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToDistributorValue.ToString _
                Or CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToSprueValue.ToString Then

            Dim openItem As ListItem = cboStatus.Items.FindByValue(miscFunctions.c_OpenValue.ToString)
            cboStatus.Items.Remove(openItem)
        End If

        'add "Closed" for existing ticket if it has been removed at that start of creating the ticket
        Dim closedItem As ListItem = cboStatus.Items.FindByValue(miscFunctions.c_ClosedValue.ToString)

        If closedItem Is Nothing Then
            cboStatus.Items.Add(New ListItem(GetGlobalResourceObject("PageGlobalResources", "SupportStatusClosed").ToString(), miscFunctions.c_ClosedValue.ToString))
        End If

        If IsCurrentUserAnInstaller() Then
            'for installer, if the status is not in "Open" or "Closed" then this is considered as already "Escalated"

            If CStr(objRequest.StatusID) <> 0 And CStr(objRequest.StatusID) <> miscFunctions.c_OpenValue.ToString And CStr(objRequest.StatusID) <> miscFunctions.c_ClosedValue.ToString Then
                'item is "Escalated" and show as "Escalated"
                UpdateEscalatedOptions(New String() {CStr(objRequest.StatusID)})
                ReplaceEscalatedText(CStr(objRequest.StatusID), GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalated").ToString())

            Else
                'item is not "Escalated", but keep "Escalated to Service Provider" and show as "Escalate"
                UpdateEscalatedOptions(New String() {miscFunctions.c_EscalatedToServiceProviderValue.ToString})
                ReplaceEscalatedText(miscFunctions.c_EscalatedToServiceProviderValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

            End If

        ElseIf IsCurrentUserAServiceProvider() Then

            'for service provider, if currently escalated allow current escalated option
            UpdateEscalatedOptions(New String() {CStr(objRequest.StatusID), miscFunctions.c_EscalatedToDistributorValue.ToString})

            If CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToServiceProviderValue.ToString Then

                'show as "Open" instead of "Escalated to Service Provider"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToServiceProviderValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString())

                'show as "Escalate" instead of "Escalated to Distributor"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToDistributorValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

            ElseIf CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToDistributorValue.ToString Then

                'show as "Escalated" instead of "Escalated to Distributor"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToDistributorValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalated").ToString())

            ElseIf CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToSprueValue.ToString Then

                'show as "Escalated" instead of "Escalated to Sprue"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToSprueValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalated").ToString())

                'remove "Escalated to Distributor" because ticket is already escalated to Sprue
                Dim escalatedItem As ListItem = cboStatus.Items.FindByValue(miscFunctions.c_EscalatedToDistributorValue.ToString)

                If escalatedItem IsNot Nothing Then
                    cboStatus.Items.Remove(escalatedItem)
                End If

            Else

                'show as "Escalate" instead of "Escalated to Distributor" because ticket is in open/closed state
                ReplaceEscalatedText(miscFunctions.c_EscalatedToDistributorValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

            End If

        ElseIf IsCurrentUserADistributor() Then

            'for distributor, if currently escalated allow current escalated option and "Escalated to Sprue"
            UpdateEscalatedOptions(New String() {CStr(objRequest.StatusID), miscFunctions.c_EscalatedToSprueValue.ToString})

            If CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToServiceProviderValue.ToString Then

                'show as "Open" instead of "Escalated to Service Provider"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToServiceProviderValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString())

                'show as "Escalate" instead of "Escalated to Sprue"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToSprueValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

            ElseIf CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToDistributorValue.ToString Then

                'show as "Open" instead of "Escalated to Distributor"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToDistributorValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString())

                'show as "Escalate" instead of "Escalated to Sprue"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToSprueValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

            ElseIf CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToSprueValue.ToString Then

                'show as "Escalated" instead of "Escalated to Sprue"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToSprueValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalated").ToString())

            Else

                'show as "Escalate" instead of "Escalated to Sprue" because ticket is in open/closed state
                ReplaceEscalatedText(miscFunctions.c_EscalatedToSprueValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalate").ToString())

            End If

        ElseIf IsCurrentUserAnApplicationOwner() Or IsCurrentUserASystemOwner() Then

            'for application owner or system owner, if currently escalated allow current escalated option
            UpdateEscalatedOptions(New String() {CStr(objRequest.StatusID)})

            If CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToServiceProviderValue.ToString Then

                'show as "Open" instead of "Escalated to Service Provider"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToServiceProviderValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString())

            ElseIf CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToDistributorValue.ToString Then

                'show as "Open" instead of "Escalated to Distributor"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToDistributorValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString())

            ElseIf CStr(objRequest.StatusID) = miscFunctions.c_EscalatedToSprueValue.ToString Then

                'show as "Open" instead of "Escalated to Sprue"
                ReplaceEscalatedText(miscFunctions.c_EscalatedToSprueValue.ToString, GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString())

            ElseIf CStr(objRequest.StatusID) = miscFunctions.c_ClosedValue.ToString Then

                'add "Open" option for closed ticket to allow it to re-open by Sprue
                Dim openItem As ListItem = cboStatus.Items.FindByValue(miscFunctions.c_OpenValue.ToString)

                If openItem Is Nothing Then
                    cboStatus.Items.Add(New ListItem(GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString(), miscFunctions.c_OpenValue.ToString))
                End If

            End If

        End If

    End Sub

    Private Sub UpdateEscalatedOptions(ByVal escalatedValuesToKeep As String())
        'keep given escalated option but remove others

        Dim statusList() As ListItem
        statusList = New ListItem(cboStatus.Items.Count - 1) {}
        cboStatus.Items.CopyTo(statusList, 0)

        For Each status As ListItem In statusList

            If status.Value <> 0 And status.Value <> miscFunctions.c_OpenValue.ToString And status.Value <> miscFunctions.c_ClosedValue.ToString Then
                'escalated option

                If Not escalatedValuesToKeep.Contains(status.Value) Then
                    'remove other escalated option from drop down
                    Dim escalatedItem As ListItem = cboStatus.Items.FindByValue(status.Value)
                    cboStatus.Items.Remove(escalatedItem)
                End If

            End If

        Next

    End Sub

    Private Sub ReplaceEscalatedText(ByVal valueToReplace As String, ByVal strEscalateText As String)
        'change escalated option text of the given value

        For Each status As ListItem In cboStatus.Items

            If status.Value = valueToReplace Then
                status.Text = strEscalateText
            End If

        Next

    End Sub

    Private Sub PopulateNotes(objRequest As SupportRequest)

        Dim noteagent As IntamacBL_SPR.AdminNotesAgent = IntamacBL_SPR.ObjectManager.CreateAdminNotesAgent("SQL")
        Dim notes As List(Of ClassLibrary_Interface.iAdminNotes) = noteagent.dtbLoadAdminNotesBySupportRequest(objRequest.SupportRequestID, objRequest.AccountID, mLoggedInUser.MasterCoID)

        Dim notesText As New Text.StringBuilder()

        lblAllNotes.Text = String.Empty
        For Each note In notes
            'notesText.AppendFormat("{0}EnteredBy: {1}{4}Date: {2}{4}{3}", IIf(notesText.Length > 0, Environment.NewLine + Environment.NewLine, ""), note.EnteredBy, note.DateEntered, note.Notes, Environment.NewLine)
            notesText.AppendFormat("{0} {1} <br/>", GetLocalResourceObject("EnteredBy"), Server.HtmlEncode(note.EnteredBy))
            notesText.AppendFormat("{0} {1} <br/> {2} <hr />", GetLocalResourceObject("Date"), note.DateEntered, Server.HtmlEncode(note.Notes).Replace(vbLf, "<br />"))
            'txtNotes.Text += "Entered by: " + note.EnteredBy + Environment.NewLine + "Date: " + note.DateEntered + Environment.NewLine + Environment.NewLine + note.Notes + Environment.NewLine + "----" + Environment.NewLine
        Next
        lblAllNotes.Text = notesText.ToString
    End Sub

    ''' <summary>
    ''' If the user is an installer, return its ID.  Otherwise return 0
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCurrentInstallerId() As Integer
        If IsCurrentUserAnInstaller() Then
            Return mLoggedInUser.UserID
        End If
        Return 0
    End Function

    Private Function IsCurrentUserAnInstaller() As Boolean
        Return Roles.IsUserInRole(mLoggedInUser.Username, miscFunctions.rolenames.Installer.ToString())
    End Function

    ''' <summary>
    ''' For service providers or distributors, get the ID
    ''' Return null for other types of user
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCurrentDistributorOrServiceProviderId() As Integer
        If IsCurrentUserADistributorOrServiceProvider() Then
            Return mLoggedInUser.MasterCoID
        End If
        Return 0
    End Function

    Private Function IsCurrentUserADistributor() As Boolean
        'check if logged in user's master company is a distributor
        Return mLoggedInUser.MasterCoType = SharedStuff.e_MasterCompanyTypes.Distributor
    End Function

    Private Function IsCurrentUserAServiceProvider() As Boolean
        'check if logged in user's master company is a service provider
        Return mLoggedInUser.MasterCoType = SharedStuff.e_MasterCompanyTypes.OperatingCompany
    End Function

    Private Function IsCurrentUserAnApplicationOwner() As Boolean
        'check if logged in user's master company is application owner
        Return mLoggedInUser.MasterCoType = SharedStuff.e_MasterCompanyTypes.ApplicationOwner
    End Function

    Private Function IsCurrentUserASystemOwner() As Boolean
        'check if logged in user's master company is system owner
        Return mLoggedInUser.MasterCoType = SharedStuff.e_MasterCompanyTypes.SystemOwner
    End Function

    Private Function IsCurrentUserADistributorOrServiceProvider() As Boolean
        'check if logged in user's master company is a distibutor or a service provider
        Return IsCurrentUserADistributor() Or IsCurrentUserAServiceProvider()
    End Function

    ''' <summary>
    ''' Records the current Master User that has just made an update to the current ticket.  This is to retain a history of all whom have modified the ticket at some point.
    ''' </summary>
    Private Sub SaveSupportRequestEditor()

        Dim EditorAgent As IntamacBL_SPR.SupportRequestEditorAgent = IntamacBL_SPR.ObjectManager.CreateSupportRequestEditorAgent("SQL")

        Try

            'store the master user that has just made a save to the current ticket.
            EditorAgent.Save(mSuppReqID, mLoggedInUser.UserID)

        Finally
            EditorAgent = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Returns a list of all the email addresses linked to the Master User records that have performed a save against the current ticket at some point.
    ''' </summary>
    ''' <returns></returns>
    Private Function RetrieveMasterUserEmailAddresses() As List(Of String)

        Dim EditorAgent As IntamacBL_SPR.SupportRequestEditorAgent = IntamacBL_SPR.ObjectManager.CreateSupportRequestEditorAgent("SQL")
        Dim lstEmailAddresses As List(Of String) = Nothing

        Try

            lstEmailAddresses = EditorAgent.MasterUserEmailAddresses(mSuppReqID)

        Finally
            EditorAgent = Nothing
        End Try

        Return lstEmailAddresses
    End Function

    ''' <summary>
    ''' Returns a list of all the email addresses linked to the Master Company records where their Master User has performed a save against the current ticket at some point.
    ''' </summary>
    ''' <returns></returns>
    Private Function RetrieveMasterCompanyEmailAddresses() As List(Of String)

        Dim EditorAgent As IntamacBL_SPR.SupportRequestEditorAgent = IntamacBL_SPR.ObjectManager.CreateSupportRequestEditorAgent("SQL")
        Dim lstEmailAddresses As List(Of String) = Nothing

        Try

            lstEmailAddresses = EditorAgent.MasterCompanyEmailAddresses(mSuppReqID)

        Finally
            EditorAgent = Nothing
        End Try

        Return lstEmailAddresses
    End Function

#End Region

#Region " New/Overrides"

#Region " Local Property Overrides"
    ' Viewstate overrides of session state variables (maintained in CultureBaseClass), to avoid changes on this page 'filtering back'
    '
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

    Public Overrides Property mAreaID As Nullable(Of Integer)
        Get
            If IsNothing(ViewState("mAreaID")) Then
                ViewState("mAreaID") = MyBase.mAreaID
            End If
            Return DirectCast(ViewState("mAreaID"), Nullable(Of Integer))
        End Get
        Set(value As Nullable(Of Integer))
            ViewState("mAreaID") = value
        End Set
    End Property

    Public Overrides Property mDeviceID As String
        Get
            If String.IsNullOrEmpty(ViewState("mDeviceID")) Then
                ViewState("mDeviceID") = MyBase.mDeviceID
            End If
            Return CStr(ViewState("mDeviceID"))
        End Get
        Set(value As String)

            ViewState("mDeviceID") = value
        End Set
    End Property


    Public Overrides Property mSelectedGateway As String
        Get
            If String.IsNullOrEmpty(ViewState("mSelectedGateway")) Then
                ViewState("mSelectedGateway") = MyBase.mSelectedGateway
            End If
            Return CStr(ViewState("mSelectedGateway"))
        End Get
        Set(value As String)

            ViewState("mSelectedGateway") = value
        End Set
    End Property
#End Region


    Protected Overrides Sub OnLoad(e As EventArgs)

        lblValidation.Text = ""

        If Not IsPostBack Then

            Dim strTopMenu = IIf(String.IsNullOrEmpty(mAccountID), "Support", "Accounts")

            If String.IsNullOrEmpty(mBackLocation) Then
                mBackLocation = "SupportSearch.aspx"
                Dim refPath As String
                If Not (IsNothing(Request) OrElse IsNothing(Request.UrlReferrer) OrElse String.IsNullOrEmpty(Request.UrlReferrer.LocalPath)) Then
                    refPath = Request.UrlReferrer.LocalPath.ToLower()
                End If
                If Not String.IsNullOrEmpty(refPath) Then
                    If refPath.Contains("accountsearch.aspx") AndAlso Not String.IsNullOrEmpty(mAccountID) Then
                        ignoreGateway = True ' from account search, ticket raised as account/property level only
                        mBackLocation = "AccountSearch.aspx"
                    ElseIf refPath.Contains("accountdetail.aspx") AndAlso Not String.IsNullOrEmpty(mAccountID) Then
                        'ignoreGateway = True ' from account detail, ticket raised as account/property level only
                        mBackLocation = "AccountDetail.aspx"
                    ElseIf refPath.Contains("accountedit.aspx") AndAlso Not String.IsNullOrEmpty(mAccountID) Then
                        'ignoreGateway = True ' from account edit, ticket raised as account/property level only
                        mBackLocation = "AccountEdit.aspx"
                    ElseIf refPath.Contains("managesensors.aspx") AndAlso Not String.IsNullOrEmpty(mAccountID) Then
                        mBackLocation = "ManageSensors.aspx"
                    End If
                End If

            End If

            'If user wants to create new ticket then  divNewTicket visible , if user selected existing account then divselectedAccount visible

            If mBackLocation.ToLower.StartsWith("account") Or mBackLocation.ToLower.StartsWith("managesensors") Then
                strTopMenu = "Accounts"
                mIsAccountPage = True
                mIsTopPage = False
            Else
                mIsAccountPage = False

            End If

            pTopMenu = strTopMenu
        End If

        MyBase.OnLoad(e)

        If String.IsNullOrEmpty(mAccountID) AndAlso mSuppReqID = 0 AndAlso Not mBackLocation.ToLower.Contains("supportsearch") Then
            SafeRedirect(mBackLocation, True)
            Exit Sub
        End If

        If Not IsPostBack Then

            'initialise the zone id
            mAreaID = Nothing

            'lblNotes.Text = lblNotes.Text + " *"
            'lblNotes2.Text = lblNotes2.Text + " *"
            FillStatus()

            If mSuppReqID = 0 Then
                'new support ticket
                divNotNew.Visible = False

                cboStatus.SelectedValue = miscFunctions.c_OpenValue.ToString
                lblEnteredBy.Text = User.Identity.Name

                'display the current time in the logged in users time zone
                Dim loggedIUserDateTime As IntamacBL_SPR.LoggedInUsersDateTime = Nothing
                Dim loggedInUsersCurrentTime As Nullable(Of DateTime)
                loggedIUserDateTime = New IntamacBL_SPR.LoggedInUsersDateTime
                loggedInUsersCurrentTime = loggedIUserDateTime.GetLoggedInUsersDateTime(mLoggedInUser.MasterCoID)

                If loggedInUsersCurrentTime.HasValue Then lblDateEntered.Text = String.Format("{0:d} {0:t}", loggedInUsersCurrentTime)

                PopulateFields(Nothing, False)

            Else
                'loading existing support ticket
                divNew.Visible = False
                Dim ra As IntamacBL_SPR.SupportRequestAgent = IntamacBL_SPR.ObjectManager.CreateSupportRequestAgent("SQL")
                Dim objRequest As IntamacBL_SPR.SupportRequest = ra.Load(mSuppReqID, mDeviceID, mLoggedInUser.MasterCoID)
                PopulateFields(objRequest, False)
            End If
        End If

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        btnViewAccount.Visible = Not String.IsNullOrEmpty(mAccountID)
        divSearch.Visible = (txtAccountEnter.Visible OrElse txtPostcodeEnter.Visible OrElse txtLastNameEnter.Visible _
                             OrElse (cboAccountID.Visible AndAlso cboAccountID.Enabled) OrElse (cboPostcode.Visible AndAlso cboPostcode.Enabled) OrElse (cboLastname.Visible AndAlso cboLastname.Enabled))

        If cboDeviceIDSelect.Visible AndAlso Not String.IsNullOrEmpty(cboDeviceIDSelect.SelectedValue) Then
            setDeviceLabels(cboDeviceIDSelect.SelectedValue)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "setupHandlers", "setupHandlers();", True)
    End Sub

#End Region

#Region "Control Event Handlers"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim objRequest As IntamacBL_SPR.SupportRequest
        Dim memUser As MembershipUser = Nothing   'current user of ticket
        Dim creatorUser As MembershipUser = Nothing   'creator of ticket

        Dim strAddresses As New List(Of String)
        Dim blnIsNew As Boolean = False
        Dim blnIsNowClosed As Boolean = False
        Dim blnIsNowOpen As Boolean = False
        Dim blnIsEscalated As Boolean = False
        'lblNoteErrors.Visible = False

        If Page.IsValid Then

            objRequest = IntamacBL_SPR.ObjectManager.CreateSupportRequest(mCompanyType)
            Dim ra As IntamacBL_SPR.SupportRequestAgent = IntamacBL_SPR.ObjectManager.CreateSupportRequestAgent("SQL")

            memUser = Membership.GetUser(User.Identity.Name)

            If mSuppReqID = 0 Then
                blnIsNew = True
                blnIsNowOpen = True
                creatorUser = memUser
            Else
                objRequest = ra.Load(mSuppReqID, mLoggedInUser.MasterCoID)
                If Not IsNothing(objRequest) Then
                    creatorUser = Membership.GetUser(objRequest.EnteredBy)

                    If cboStatus.SelectedValue = miscFunctions.c_ClosedValue.ToString Then '- now closed
                        blnIsNowClosed = True
                    End If

                    If cboStatus.SelectedValue = miscFunctions.c_OpenValue.ToString Then '- now open
                        blnIsNowOpen = True
                    End If

                End If
            End If

            'check if this ticket is currently escalated before saving (use this boolean to help work out email send)
            Dim blnCurrentlyEscalated As Boolean = False

            If objRequest.StatusID = miscFunctions.c_EscalatedToServiceProviderValue Or cboStatus.SelectedValue = miscFunctions.c_EscalatedToDistributorValue Or cboStatus.SelectedValue = miscFunctions.c_EscalatedToSprueValue Then
                blnCurrentlyEscalated = True
            End If

            'fill the rest of the details
            Dim blnStatusChanged As Boolean = (blnIsNew OrElse CInt(cboStatus.SelectedValue) <> objRequest.StatusID)

            'check if status is escalated
            If blnStatusChanged = True And (cboStatus.SelectedValue = miscFunctions.c_EscalatedToServiceProviderValue Or cboStatus.SelectedValue = miscFunctions.c_EscalatedToDistributorValue Or
                cboStatus.SelectedValue = miscFunctions.c_EscalatedToSprueValue) Then
                blnIsEscalated = True
            End If

            'check if valid escalation
            If blnIsEscalated Then

                If mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.ApplicationOwner) Then
                    'ticket is being escalated by user whose master company is Sprue, so add Intamac support's email address
                    strAddresses.Add(WebConfigurationManager.AppSettings("ThirdLineSupportRequestEmailDestination").ToString)
                ElseIf mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.Distributor) Then
                    'ticket is being escalated by user whose master company is distributor, so add Sprue Aegis's email address

                    'load master company of Sprue Aegis
                    Dim mCompany As New IntamacBL_SPR.MasterCompanySPR
                    If mCompany.Load(mUsersCompany.ParentMasterCoID) Then
                        'this is the application owener i.e. Sprue Aegis
                        strAddresses.Add(mCompany.Email)
                    End If

                ElseIf mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.OperatingCompany) Then
                    'ticket is being escalated user whose master company is service provider

                    If IsCurrentUserAnInstaller() Then
                        'installer user, so add service provider's email address
                        Dim mCompany As New IntamacBL_SPR.MasterCompanySPR
                        If mCompany.Load(mUsersCompany.MasterCoID) Then
                            'this is the service provider
                            strAddresses.Add(mCompany.Email)
                        End If

                    Else
                        'service provider user, so add parent company's email address i.e. distributor
                        Dim mCompany As New IntamacBL_SPR.MasterCompanySPR
                        If mCompany.Load(mUsersCompany.ParentMasterCoID) Then
                            'this is the distributor
                            strAddresses.Add(mCompany.Email)
                        End If

                    End If

                End If

            End If

            If Not String.IsNullOrWhiteSpace(mAccountID) Then
                objRequest.AccountID = mAccountID
            Else
                objRequest.AccountID = txtAccountEnter.Text

            End If
            If Not String.IsNullOrWhiteSpace(objRequest.AccountID) Then
                objRequest.PropertyID = mPropertyID

                objRequest.DateEntered = Date.UtcNow
                If blnIsNew Then
                    objRequest.EnteredBy = User.Identity.Name
                End If
                objRequest.StatusID = CInt(cboStatus.SelectedValue)
                objRequest.SupportRequestID = mSuppReqID
                objRequest.UpdatedBy = User.Identity.Name
                objRequest.DeviceID = mDeviceID

                If cboDeviceIDSelect.Visible AndAlso Not String.IsNullOrEmpty(cboDeviceIDSelect.SelectedValue) Then


                    mDeviceID = cboDeviceIDSelect.SelectedValue.Split("|"c)(0)

                    Dim strAreaID As String = cboDeviceIDSelect.SelectedValue.Split("|"c)(3)

                    If Not String.IsNullOrEmpty(strAreaID) Then
                        mAreaID = CInt(strAreaID)
                    End If
                End If
                If mAreaID.HasValue Then
                    objRequest.Zone = mAreaID
                End If
                objRequest.DeviceID = mDeviceID
                objRequest.OptionalRef = txtReference.Text


                If ra.Save(objRequest) Then

                    'SP-773. Following any save, we need to update the history of those whom have edited the current support ticket
                    SaveSupportRequestEditor()

                    'update status options
                    UpdateStatusOptionsExistingTicket(objRequest)

                    'set new support request id which is then used in the email
                    mSuppReqID = objRequest.SupportRequestID

                    If txtNewNotes.Text.Length > 0 Then
                        Dim pa As IntamacBL_SPR.AdminNotesAgent = IntamacBL_SPR.ObjectManager.CreateAdminNotesAgent("SQL")


                        'Dim objNote As ClassLibrary_Interface.iAdminNotes = pa.LoadAdminNote(0)
                        Dim objNote = New IntamacBL_SPR.AdminNotes
                        objNote.AccountID = mAccountID
                        objNote.Subject = objRequest.SupportRequestID
                        objNote.DateEntered = Date.UtcNow
                        objNote.EnteredBy = User.Identity.Name
                        objNote.UpdatedBy = User.Identity.Name
                        objNote.Notes = txtNewNotes.Text
                        objNote.SupportRequestID = objRequest.SupportRequestID
                        If pa.Save(objNote) Then
                            txtNewNotes.Text = String.Empty
                            PopulateNotes(objRequest)
                            'Else
                            'ShowValidationErrors(pa.ValidationErrors, lblNoteErrors)
                        End If
                    End If

                    'include originator's email address and logged-in user's who is updating the ticket
                    strAddresses.Add(memUser.Email)

                    If creatorUser IsNot Nothing Then
                        strAddresses.Add(creatorUser.Email)
                    End If


                    Dim blnAddMasterCoEmail As Boolean = True

                    'no need to add service provider's email address if installer is closing/opening ticket that hasn't been escalated
                    If IsCurrentUserAnInstaller() Then
                        If blnCurrentlyEscalated = False And (blnIsNowClosed = True Or blnIsNowOpen = True) Then
                            blnAddMasterCoEmail = False
                        End If
                    End If

                    'get all the email addresses associated with the ticket
                    strAddresses = GetTicketEmailAddresses(strAddresses, blnAddMasterCoEmail)

                    If blnIsNew Then
                        'new ticket
                        SendNewRequestEmail(strAddresses)
                    ElseIf Not blnIsNowClosed Then
                        'update to ticket that is not being closed
                        SendUpdatedRequestEmail(strAddresses)
                    End If

                    mSuppReqID = objRequest.SupportRequestID

                    'SP-773. The ticket has just been closed, so email all those whom have ever updated this ticket to tell them it is now resolved.
                    If blnIsNowClosed Then

                        'Email all those whom have ever updated this ticket to tell them it is now resolved.
                        SendClosedEmail(strAddresses)

                    End If

                    If blnIsNew = True Then
                        miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Support_Requests_Added, GetLocalResourceObject("SupportRequestID").ToString() & ": " & mSuppReqID)
                    Else
                        miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Support_Requests_Updated, GetLocalResourceObject("SupportRequestID").ToString() & ": " & mSuppReqID & CStr(IIf(blnIsNowClosed = True, " " & GetGlobalResourceObject("PageGlobalResources", "StatusLabel").ToString() & " " & GetGlobalResourceObject("PageGlobalResources", "SupportStatusClosed").ToString(), "")))
                    End If

                    ' Update RESS 'support_open' indicator
                    Dim propAgent As New PropertyAgent(New IntamacDAL_SPR.PropertyDBSQL())
                    propAgent.SetRESSSupportOpen(objRequest)

                Else
                    'display validation failure
                    lblValidation.Text = PageString("AccountIDInvalidMsg")
                    lblValidation.Visible = True
                End If
            Else
                lblValidation.Text = PageString("AccountIDRequiredMsg")
                lblValidation.Visible = True

            End If
        End If

    End Sub

    ''' <summary>
    ''' Gets a list of all the email addresses to be notified of the creation/change of this support ticket
    ''' </summary>
    ''' <param name="strAddresses"></param>
    ''' <param name="blnAddMasterCoEmail"></param>
    ''' <returns></returns>
    Private Function GetTicketEmailAddresses(strAddresses As List(Of String), blnAddMasterCoEmail As Boolean) As List(Of String)
        Dim strMasterUserEmails As List(Of String) = RetrieveMasterUserEmailAddresses()
        strAddresses.AddRange(strMasterUserEmails)

        If blnAddMasterCoEmail Then
            Dim strMasterCoEmails As List(Of String) = RetrieveMasterCompanyEmailAddresses()
            strAddresses.AddRange(strMasterCoEmails)
        End If

        'add generic email address which gets sent for all changes
        If System.Configuration.ConfigurationManager.AppSettings("SupportRequestEmailDestination") IsNot Nothing Then
            strAddresses.Add(System.Configuration.ConfigurationManager.AppSettings("SupportRequestEmailDestination").ToString())
        End If

        'remove any duplicates
        strAddresses = strAddresses.Distinct().ToList

        Return strAddresses
    End Function

    Private Sub SetRESSSupportOpen(ByVal updatedRequest As SupportRequest)

        Dim propertyID As String = updatedRequest.PropertyID

        ' RESS update only required if request is associated with a property
        If Not String.IsNullOrEmpty(propertyID) Then
            Dim propertyGetter As New IntamacBL_SPR.PropertyAgent(New IntamacDAL_SPR.PropertyDBSQL())

            propertyGetter.SetRESSSupportOpen(updatedRequest)

        End If
    End Sub
    Private Sub ShowValidationErrors(validationErrors As SortedList(Of String, String), validationLabel As Label)
        If validationErrors.Count = 0 Then
            validationLabel.Text = PageString("ErrorHasOccurred")
        Else
            validationLabel.Text = validationErrors.Values(0)
        End If

        validationLabel.Visible = True
    End Sub

    Protected Sub btnViewAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewAccount.Click
        '
        '   This page 'caches' values in Viewstate, so must put them back into the Sessions versions
        MyBase.mAccountID = mAccountID
        MyBase.mPropertyID = mPropertyID
        MyBase.mSelectedGateway = mSelectedGateway
        Response.Redirect("AccountDetail.aspx")
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        PopulateFields(Nothing, True)
    End Sub

#End Region

    Private Sub SendRequestEmail(ByVal v_strTextPart1 As String, ByVal v_strRecip() As String, ByVal v_strTitlePart2 As String)

        Dim sbContent As New StringBuilder(String.Format(v_strTextPart1, User.Identity.Name, Now))

        sbContent.AppendFormat("<b>{0}</b> {1}<br />", PageString("AccountIDLabel"), mAccountID)
        sbContent.AppendFormat("<b>{0}</b> {1}<br />", PageString("SupportRefNoLabel"), mSuppReqID.ToString())
        sbContent.AppendFormat("<b>{0}</b> {1}<br /><br />", PageString("StatusLabel"), cboStatus.SelectedItem.Text)


        Dim strToAddress As String = v_strRecip(0)

        If v_strRecip.Length > 1 Then
            For i As Integer = 1 To v_strRecip.Length - 1
                If Not String.IsNullOrEmpty(strToAddress(i)) Then
                    strToAddress &= ";" & v_strRecip(i)
                End If
            Next
        End If

        miscFunctions.SendSupportRequestEmail(mAccountID, mPropertyID, sbContent.ToString, strToAddress, String.Format("{0} - {1}", GetLocalResourceObject("RequestEmailTitle1"), v_strTitlePart2))

    End Sub

    Private Sub Send3rdLineRequestEmail(ByVal v_intIDRef As Integer)
        SendRequestEmail(CStr(GetLocalResourceObject("EscalatedRequestEmailPart1")), New String() {WebConfigurationManager.AppSettings("ThirdLineSupportRequestEmailDestination").ToString}, String.Format(CStr(GetLocalResourceObject("EscalatedRequestEmailTitle2")), v_intIDRef))
    End Sub

    Private Sub SendClosedEmail(ByVal v_ExtraRecip As List(Of String))
        SendRequestEmail(CStr(GetLocalResourceObject("ClosedRequestEmailPart1")), v_ExtraRecip.ToArray, CStr(GetLocalResourceObject("ClosedRequestEmailTitle2")))
    End Sub

    Private Sub SendNewRequestEmail(ByVal v_ExtraRecip As List(Of String))
        SendRequestEmail(CStr(GetLocalResourceObject("NewRequestEmailPart1")), v_ExtraRecip.ToArray, CStr(GetLocalResourceObject("NewRequestEmailTitle2")))
    End Sub

    Private Sub SendUpdatedRequestEmail(ByVal v_ExtraRecip As List(Of String))
        SendRequestEmail(CStr(GetLocalResourceObject("UpdatedRequestEmailPart1")), v_ExtraRecip.ToArray, CStr(GetLocalResourceObject("UpdatedRequestEmailTitle2")))
    End Sub

    Private Sub cboGatewaySelect_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboGatewaySelect.SelectedIndexChanged
        mPropertyID = cboGatewaySelect.SelectedValue
        LoadDevices()
        DisplayProperty()
    End Sub


End Class
