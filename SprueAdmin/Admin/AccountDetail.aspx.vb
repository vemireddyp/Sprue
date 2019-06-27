Imports System.Web.Security

Imports Telerik.Web.UI

Imports IntamacBL_SPR
Imports IntamacShared_SPR.SharedStuff

Partial Class Admin_AccountDetail
    Inherits CultureBaseClass

    Protected strDistributorValue As String = ""
    Protected strServiceProviderValue As String = ""
    Protected strInstallerValue As String = ""
    Protected strRiskLevel As String = ""


#Region "Constants"
    Protected Const cSessionDetailsKey As String = "AccountDetailDetails"

#End Region

#Region "Preserve Grid Details"

    '    Private _savedDetails As NameValueCollection

    'Private ReadOnly Property savedDetails As NameValueCollection
    '    Get
    '        If IsNothing(_savedDetails) Then
    '            _savedDetails = CType(Session(cSessionDetailsKey), NameValueCollection)

    '            If IsNothing(_savedDetails) Then
    '                _savedDetails = New NameValueCollection()
    '                Session(cSessionDetailsKey) = _savedDetails
    '            End If
    '        End If

    '        Return _savedDetails
    '    End Get
    'End Property

    'Private Sub saveSearchDetails()

    '    savedDetails.Clear()

    '    savedDetails.Add("mNotesSortDir", mNotesSortDir)

    '    savedDetails.Add("mNotesSortExpr", mNotesSortExpr)

    '    savedDetails.Add("mReqSortDir", mReqSortDir)

    '    savedDetails.Add("mReqSortExpr", mReqSortExpr)

    '    savedDetails.Add("mUsersSortDir", mUsersSortDir)

    '    savedDetails.Add("mUsersSortExpr", mUsersSortExpr)

    '    savedDetails.Add("selectedGrid", mnuSuppNotes.SelectedValue)

    'End Sub

    'Private Sub restoreSearchDetails()

    '    mNotesSortExpr = "DateEntered"
    '    mNotesSortDir = "DESC"

    '    mReqSortExpr = "DateEntered"
    '    mReqSortDir = "DESC"

    '    mUsersSortExpr = "PersonID"
    '    mUsersSortDir = "ASC"

    '    If (savedDetails.Count > 0) Then

    '        For Each searchKey As String In savedDetails.AllKeys

    '            Select Case searchKey
    '                Case "mNotesSortDir"
    '                    mNotesSortDir = savedDetails(searchKey)

    '                Case "mNotesSortExpr"
    '                    mNotesSortExpr = savedDetails(searchKey)

    '                Case "mReqSortDir"
    '                    mReqSortDir = savedDetails(searchKey)

    '                Case "mReqSortExpr"
    '                    mReqSortExpr = savedDetails(searchKey)

    '                Case "mUsersSortExpr"
    '                    mUsersSortExpr = savedDetails(searchKey)

    '                Case "mUsersSortDir"
    '                    mUsersSortDir = savedDetails(searchKey)

    '                Case "selectedGrid"
    '                    mnuSuppNotes.FindItem(CStr(savedDetails(searchKey))).Selected = True

    '            End Select

    '        Next

    '    End If
    '    ShowGrid()
    'End Sub

#End Region

#Region "ViewState Variables"

    'Protected Property mNotesSortDir As String
    '    Get
    '        If Not IsNothing(ViewState("mNotesSortDir")) Then
    '            Return CStr(ViewState("mNotesSortDir"))
    '        Else
    '            Return "ASC"
    '        End If
    '        Return CStr(ViewState("mNotesSortDir"))
    '    End Get
    '    Set(value As String)
    '        ViewState("mNotesSortDir") = value
    '    End Set
    'End Property

    'Protected Property mNotesSortExpr As String
    '    Get
    '        Return CStr(ViewState("mNotesSortExpr"))
    '    End Get
    '    Set(value As String)
    '        ViewState("mNotesSortExpr") = value
    '    End Set
    'End Property

    'Protected Property mReqSortDir As String
    '    Get
    '        If Not IsNothing(ViewState("mReqSortDir")) Then
    '            Return CStr(ViewState("mReqSortDir"))
    '        Else
    '            Return "ASC"
    '        End If
    '        Return CStr(ViewState("mReqSortDir"))
    '    End Get
    '    Set(value As String)
    '        ViewState("mReqSortDir") = value
    '    End Set
    'End Property

    'Protected Property mReqSortExpr As String
    '    Get
    '        Return CStr(ViewState("mReqSortExpr"))
    '    End Get
    '    Set(value As String)
    '        ViewState("mReqSortExpr") = value
    '    End Set
    'End Property

    'Protected Property mUsersSortDir As String
    '    Get
    '        If Not IsNothing(ViewState("mUsersSortDir")) Then
    '            Return CStr(ViewState("mUsersSortDir"))
    '        Else
    '            Return "ASC"
    '        End If
    '        Return CStr(ViewState("mUsersSortDir"))
    '    End Get
    '    Set(value As String)
    '        ViewState("mUsersSortDir") = value
    '    End Set
    'End Property

    'Protected Property mUsersSortExpr As String
    '    Get
    '        Return CStr(ViewState("mUsersSortExpr"))
    '    End Get
    '    Set(value As String)
    '        ViewState("mUsersSortExpr") = value
    '    End Set
    'End Property

    Protected Property mParentCoID As Integer
        Get
            If ViewState("ParentCoID") IsNot Nothing Then
                Return CInt(ViewState("ParentCoID"))
            Else
                Return 0
            End If

        End Get
        Set(value As Integer)
            ViewState("ParentCoID") = value
        End Set
    End Property

    Protected Property mInstallerID As Integer
        Get
            If ViewState("InstallerID") IsNot Nothing Then
                Return CInt(ViewState("InstallerID"))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            ViewState("InstallerID") = value
        End Set
    End Property

    'Protected Property vs_AccountID As String
    '	Get
    '		Return CStr(ViewState("AccountID"))
    '	End Get
    '	Set(value As String)
    '		ViewState("AccountID") = value
    '	End Set
    'End Property

    'Protected Property vs_PropertyID As String
    '	Get
    '		Return CStr(ViewState("PropertyID"))
    '	End Get
    '	Set(value As String)
    '		ViewState("PropertyID") = value
    '	End Set
    'End Property

#End Region

#Region "Page Events"

    'Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
    '	If Not Page.IsPostBack Then initialisePage()
    'End Sub

#End Region

#Region "Private Methods"

    'Private Sub initialisePage()
    '	LoadRiskLevels()
    'End Sub

    ''' <summary>
    ''' Loads all the known Risk Levels within the system
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadRiskLevels()
        Dim oRiskLevels As IntamacBL_SPR.RiskLevels = Nothing
        Dim lstRiskLevels As List(Of IntamacBL_SPR.RiskLevel) = Nothing
        Dim liRL As RadComboBoxItem = Nothing

        Try

            'clear the dropdown initially
            'cboRiskLevels.Items.Clear()

            'load all the risk levels from the DB
            oRiskLevels = New IntamacBL_SPR.RiskLevels
            lstRiskLevels = oRiskLevels.Load()

            For Each RL As IntamacBL_SPR.RiskLevel In lstRiskLevels

                'create the next Risk Level item to add to the dropdown
                liRL = New RadComboBoxItem
                liRL.Text = RL.Name
                liRL.Value = RL.ID

                'add the Risk Level to the drop down
                'cboRiskLevels.Items.Add(liRL)

                'tidy up
                liRL = Nothing

            Next

        Finally
            liRL = Nothing
            If lstRiskLevels IsNot Nothing Then lstRiskLevels.Clear()
            lstRiskLevels = Nothing
            oRiskLevels = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Loads all the properties for the chosen account.  Displays them in the relevant dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadProperties()
        Dim propertyGetter As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
        Dim objProperty As IntamacBL_SPR.PropertyBase = Nothing
        Dim lstProperties As List(Of ClassLibrary_Interface.iProperty) = Nothing
        Dim hshProperties As Dictionary(Of String, String) = Nothing
        Dim AddressLine1Selected As String = ""
        mSelectedGateway = ""

        Try

            cboProperties.Text = ""
            cboProperties.Items.Clear()
            cboProperties.ClearSelection()

            If Not String.IsNullOrEmpty(mAccountID) Then

                'load all the properties of the chosen account
                lstProperties = propertyGetter.LoadAllProperties(mAccountID, "")

                'clear any existing items from the properties dropdown

                hshProperties = New Dictionary(Of String, String)

                'iterate through any loaded properties, in ascending order of address line 1, and add them to the relevant dropdown
                For Each prop As ClassLibrary_Interface.iProperty In lstProperties.OrderBy(Function(rec) rec.Address1)

                    If Not hshProperties.ContainsKey(prop.Address1) Then
                        hshProperties.Add(prop.Address1, prop.PropertyID)
                    Else
                        hshProperties(prop.Address1) &= "|" + prop.PropertyID

                    End If
                Next

                For Each addr1 As String In hshProperties.Keys
                    cboProperties.Items.Add(New RadComboBoxItem(addr1, hshProperties(addr1)))

                Next

                'either select property based on property ID, or display the empty text if no properties were identified
                If Not String.IsNullOrEmpty(mPropertyID) AndAlso cboProperties.Items.Count > 0 Then
                    'cboProperties.SelectedIndex = 0

                    For Each prop As RadComboBoxItem In cboProperties.Items

                        If prop.Value.Contains(mPropertyID) Then
                            prop.Selected = True
                            Exit For
                        End If

                    Next

                Else
                    cboProperties.Text = ""
                End If

                ' if no entry selected, select first one (if present)
                If String.IsNullOrEmpty(cboProperties.SelectedValue) AndAlso cboProperties.Items.Count = 1 Then
                    cboProperties.SelectedIndex = 0
                End If

            End If
        Finally
            propertyGetter = Nothing
            objProperty = Nothing
            If lstProperties IsNot Nothing Then lstProperties.Clear()
            lstProperties = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Loads all the gateways for the chosen account/property.  Displays them in the relevant dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadGateways()
        Dim gate As IntamacBL_SPR.Gateways = Nothing
        Dim lstGateways As List(Of IntamacBL_SPR.Gateway) = Nothing
        Dim PropertyIDs As String = ""

        Try

            'instantiate the gateways class
            gate = New IntamacBL_SPR.Gateways


            If Not String.IsNullOrEmpty(cboProperties.SelectedValue) Then PropertyIDs = cboProperties.SelectedValue

            'clear any existing items from the gateways dropdown
            cboGateways.Items.Clear()
            cboGateways.Text = ""
            cboGateways.ClearSelection()

            If Not String.IsNullOrEmpty(mAccountID) Then

                'load all the gateways of the chosen account, for all properties on that account
                lstGateways = gate.Load(mAccountID, "", True)


                'iterate through any loaded gateways, in ascending order of MAC, and add them to the relevant dropdown
                For Each singleGateway As IntamacBL_SPR.Gateway In lstGateways.OrderBy(Function(rec) rec.MACAddress)

                    If String.IsNullOrEmpty(PropertyIDs) OrElse PropertyIDs.Contains(singleGateway.PropertyID) Then
                        'Set the text of the gateway dropdown to include the name and the mac address
                        Dim GatewayText As String = IIf(String.IsNullOrEmpty(singleGateway.Name), "N/A", singleGateway.Name) & " - " & singleGateway.MACAddress
                        'Set the mac address and the property id in the value delimitted by a pipe.
                        Dim ValueText As String = singleGateway.MACAddress & "|" & singleGateway.PropertyID

                        cboGateways.Items.Add(New RadComboBoxItem(GatewayText, ValueText))
                    End If
                Next

                'either select gateway based on property ID, or display the empty text if no gateways were identified
                If cboGateways.Items.Count > 0 AndAlso Not String.IsNullOrEmpty(mPropertyID) Then

                    For Each gwItem As RadComboBoxItem In cboGateways.Items

                        Dim strMacandProp() As String = gwItem.Value.Split("|")

                        If strMacandProp.Length = 2 And strMacandProp(1) = mPropertyID Then
                            gwItem.Selected = True
                            mSelectedGateway = strMacandProp(0)
                            Exit For
                        End If

                    Next
                Else
                    cboGateways.Text = ""
                    mSelectedGateway = ""
                End If
            End If
            'Only show the Update firmware button if a gateway is selected.
            If Not String.IsNullOrEmpty(Me.cboGateways.SelectedValue) Then
                Me.placeFirmwareUpdate.Visible = True
            Else
                Me.placeFirmwareUpdate.Visible = False
            End If
        Finally
            gate = Nothing
            If lstGateways IsNot Nothing Then lstGateways.Clear()
            lstGateways = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Loads all the Zones for the chosen account/property.  Displays them in the relevant dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadZones()
        Dim zone As IntamacBL_SPR.Areas = Nothing
        Dim lstZones As List(Of IntamacBL_SPR.Areas) = Nothing
        Dim PropertyIds As String = ""

        Try

            'instantiate the areas class
            zone = New IntamacBL_SPR.Areas

            If Not String.IsNullOrEmpty(cboGateways.SelectedValue) Then
                Dim GWValue As String() = cboGateways.SelectedValue.Split("|"c)

                If GWValue.Length > 1 Then
                    PropertyIds = GWValue(1)
                End If

            ElseIf Not String.IsNullOrEmpty(cboProperties.SelectedValue) Then
                PropertyIds = cboProperties.SelectedValue

            End If


            'clear any existing items from the zones dropdown
            cboZones.Items.Clear()
            cboZones.Text = ""
            cboZones.ClearSelection()

            If Not String.IsNullOrEmpty(mAccountID) Then

                lstZones = zone.Load(mAccountID, "")

                cboZones.Items.Add(New RadComboBoxItem(GetLocalResourceObject("AllZones"), "All"))

                'iterate through any loaded Zones, in ascending order of Zone Name, and add them to the relevant dropdown
                For Each singleZone As IntamacBL_SPR.Areas In lstZones.OrderBy(Function(rec) rec.Name)
                    'If singleZone.Address.Trim().ToUpper() = PropertyAddress1.Trim().ToUpper() Then

                    If String.IsNullOrEmpty(PropertyIds) OrElse PropertyIds.Contains(singleZone.PropertyID) Then
                        cboZones.Items.Add(New RadComboBoxItem(singleZone.Name, singleZone.ID.ToString + "|" + singleZone.PropertyID))

                    End If
                    'End If
                Next

                'either make the first item in the dropdown selected by default, or display the empty text if no zones were identified
                If cboZones.Items.Count = 1 Then
                    cboZones.SelectedIndex = 0
                End If

            End If
        Finally
            zone = Nothing
            If lstZones IsNot Nothing Then lstZones.Clear()
            lstZones = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Loads the account address, based upon the currently selected account, and display the related address on the web page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadAndDisplayAccountDetails()
        Dim clientGetter As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")
        Dim objClient As IntamacBL_SPR.Client = Nothing

        lblAccountAddress.Text = ""
        lblAccountIDValue.Text = ""

        Try

            If Not String.IsNullOrEmpty(mAccountID) Then

                'load the client object based upon the identified account id
                objClient = clientGetter.Load(mAccountID)

                'check that the client is found before proceeding
                If objClient IsNot Nothing Then

                    lblAccountName.Text = objClient.FirstName & " " & objClient.Surname

                    'display the address on the page
                    If Not String.IsNullOrEmpty(objClient.Address1) Then
                        lblAccountAddress.Text = objClient.Address1.Trim() & "<br/>"
                    End If

                    If Not String.IsNullOrEmpty(objClient.Address2) Then
                        lblAccountAddress.Text &= objClient.Address2.Trim() & "<br/>"
                    End If

                    If Not String.IsNullOrEmpty(objClient.Postcode) Then
                        lblAccountAddress.Text &= objClient.Postcode.Trim()
                    End If

                    'display the account id on the page
                    lblAccountIDValue.Text = objClient.AccountID

                    'set the dashboard property "AccountID" to the chosen account within the dropdown.  This is to aid the dashboard filtering
                    Dashboard.AccountID = objClient.AccountID
                    Dashboard.AllTileLegend = PageString("DSH_TotalDevicesLegend_DeviceDetail")
                End If
            End If

        Finally
            clientGetter = Nothing
            objClient = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Loads the property address, based upon the currently selected account and property within the dropdowns, and display the related address on the web page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadAndDisplayPropertyAddress()

        Dim propertyID As String = ""
        Dim prop As ClassLibrary_Interface.iProperty = Nothing
        Dim propertyGetter As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")

        Try
            lblPropertyAddress.Text = ""

            'get the property id of the chosen property, from the property dropdown (if required)

            propertyID = mPropertyID

            If String.IsNullOrEmpty(propertyID) Then
                If Not String.IsNullOrEmpty(cboProperties.SelectedValue) Then
                    Dim propIDs As String() = cboProperties.SelectedValue.Split("|"c)

                    If propIDs.Length > 0 Then
                        propertyID = propIDs(0)
                        If propIDs.Length = 1 Then
                            mPropertyID = propertyID
                        End If
                    End If

                End If

            End If


            If Not String.IsNullOrEmpty(propertyID) Then

                'load the property object, based upon the identified account and property ids
                prop = propertyGetter.Load(mAccountID, propertyID)

                'initialise the property text by setting it to an empty string
                lblPropertyAddress.Text = ""

                'check that the property has been identified
                If prop IsNot Nothing Then

                    If Not String.IsNullOrEmpty(prop.Address1) Then
                        lblPropertyAddress.Text = prop.Address1.Trim() & "<br/>"
                    End If

                    If Not String.IsNullOrEmpty(prop.Address2) Then
                        lblPropertyAddress.Text &= prop.Address2.Trim() & "<br/>"
                    End If

                    If Not String.IsNullOrEmpty(prop.Postcode) Then
                        lblPropertyAddress.Text &= prop.Postcode.Trim()
                    End If

                End If
            End If

        Finally
            propertyID = Nothing
            prop = Nothing
            propertyGetter = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Loads and displays the chosen Distributor, Service Provider, and Installer that relate to the chosen account from the Account Enquiry page
    ''' </summary>
    ''' <param name="objClient">A loaded client object, based upon the account selected on the Account Enquiry page</param>
    ''' <remarks></remarks>
    Private Sub LoadAndDisplayDistributorProviderInstaller(objClient As IntamacBL_SPR.Client)

        Dim mUser As IntamacBL_SPR.MasterUserSPR = Nothing
        Dim objPropDeviceGetter As PropertyDevice = Nothing
        Dim mCompany As IntamacBL_SPR.MasterCompanySPR = Nothing

        Try

            'check that the client object is already loaded
            If objClient IsNot Nothing Then

                'confirm that the service provider has already been set
                If objClient.ParentCompanyID > 0 Then

                    'instantiate the master company class.  This is used to load the details of the service provider
                    mCompany = New IntamacBL_SPR.MasterCompanySPR

                    'load the service provider
                    If mCompany.Load(objClient.ParentCompanyID) Then
                        'this is the service provider

                        If mCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.OperatingCompany) Then

                            ' 'owner' company is a service provider, so display name on the page
                            strServiceProviderValue = mCompany.MasterCoID
                            lblServiceProvider.Text = mCompany.Name
                            ' Parent of service provider must be a distributor, so load the distributor by loading another master company, based upon the parent id
                            If mCompany.Load(mCompany.ParentMasterCoID) Then
                                'display the distributor name on the page
                                strDistributorValue = mCompany.MasterCoID
                                lblDistributor.Text = mCompany.Name
                            End If
                        ElseIf mCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.Distributor) Then

                            ' 'owner' is a distributor so display name in correct place
                            strDistributorValue = mCompany.MasterCoID
                            lblDistributor.Text = mCompany.Name
                            lblServiceProvider.Text = GetLocalResourceObject("NA")
                        Else
                            '  service provider and Distibutor so display 'N/A'
                            lblDistributor.Text = GetLocalResourceObject("NA")
                            lblServiceProvider.Text = GetLocalResourceObject("NA")
                        End If



                    End If
                End If
            End If

            'identify the installer

            'instantiate the master user object
            mUser = New IntamacBL_SPR.MasterUserSPR
            objPropDeviceGetter = ObjectManager.CreatePropertyDevice(mCompanyType)

            'load the prop device record.  This contains an installer field which indicates who the installer is
            If objPropDeviceGetter.Load(mAccountID, mPropertyID, 1) Then

                'load the master user based upon the installer id
                If mUser.Load(objPropDeviceGetter.InstallerID) Then

                    If Roles.IsUserInRole(mUser.Username, miscFunctions.rolenames.Installer.ToString) Then
                        'display the installer name on the page
                        strInstallerValue = mUser.UserID
                        lblInstaller.Text = mUser.Lastname + "," + mUser.Firstname
                    Else
                        lblInstaller.Text = GetLocalResourceObject("NA")
                    End If
                Else
                    lblInstaller.Text = GetLocalResourceObject("NA")
                End If
            Else
                lblInstaller.Text = GetLocalResourceObject("NA")
            End If

        Finally
            mUser = Nothing
            objPropDeviceGetter = Nothing
            mCompany = Nothing
        End Try

    End Sub


    ''' <summary>
    ''' Sets the Property to an empty string and resets the dropdowns for properties/gateways and zones
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearPropertyDetails()

        'Set the Property address to an empty string
        ClearPropertyChoice()

        cboProperties.Items.Clear()
        cboGateways.Items.Clear()
        cboZones.Items.Clear()

        cboProperties.Text = ""
        cboGateways.Text = ""
        cboZones.Text = ""
    End Sub

    ''' <summary>
    ''' Sets the Property address to an empty string
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearPropertyChoice()
        lblPropertyAddress.Text = ""
    End Sub

    ''' <summary>
    ''' Checks to see if the dropdown text chosen matches any of the elements within the dropdown (user could have typed invalid data).  If no match found then the dropdown text is set to empty string.
    ''' Each dropdown control that is of a level below that of the selected dropdown is reset, including any address data that is shown on the page.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="dropdown">The required dropdown to reset</param>
    ''' <remarks></remarks>
    Private Sub ResetMismatchDropDownText(sender As Object, ByRef dropdown As RadComboBox)

        Dim bRet As Boolean = False

        Try

            'iterate through the items within the specified dropdown, checking to see if any match the entered text.  If a match is found then exit this loop immediately
            For Each comboItem As RadComboBoxItem In dropdown.Items

                'check to see if the entered text matches the current dropdown item
                bRet = (comboItem.Text = dropdown.Text)

                'if a match is found then do not waste time checking any of the remaining items as the result is already known
                If bRet Then Exit For
            Next

            'If a match was not found then reset the specified dropdown to an empty string, and reset related dropdowns and addresses
            If Not bRet Then

                'No match found.  Reset the specified dropdown to an empty string
                dropdown.Text = ""

                If dropdown Is cboProperties Then
                    'the dropdown affected was the properties dropdown, so clear the property address
                    ClearPropertyChoice()
                End If

            End If

        Finally
            bRet = Nothing
        End Try

    End Sub

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

#End Region

#Region "InitialiseMasterdetails"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitialiseMasterdetails()

        Dim usersCoType As IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes = IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany
        If Not IsNothing(mUsersCompany) Then            '
            ' get the user's company type Intamac, Sprue, Distributor or Provider
            usersCoType = CType(mUsersCompany.CompanyTypeID, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes)
        End If
        '
        ' initialise member vars, saved details and list controls
        mblnLoadDistributors = False
        mblnLoadServiceProvs = False
        mblnLoadInstallers = False

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

    End Sub

#End Region


#Region "GetRiskLevelByAccountProperty"
    ''' <summary>
    ''' Binds the Risk Level dropdown.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetRiskLevelByAccountProperty()
        Try
            Dim propertyGetter As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
            Dim objProperty As IntamacBL_SPR.PropertyBase = propertyGetter.Load(mAccountID, mPropertyID)
            If objProperty IsNot Nothing Then
                If Not objProperty.RiskLevel.ToString() = "-1" And objProperty.RiskLevel.ToString() IsNot Nothing Then
                    Dim obje_RiskLevel As e_RiskLevel = CType(objProperty.RiskLevel, e_RiskLevel)
                    'cboRiskLevels.SelectedValue = obje_RiskLevel
                Else
                    'cboRiskLevels.ClearSelection()
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region


#Region "New/Overrides"

    Protected Overrides Sub OnLoad(e As EventArgs)
        If Not IsPostBack Then
            mBackLocation = "AccountSearch.aspx"
        End If

        MyBase.OnLoad(e)

        'the dashboard figures must be for the current selected property only.
        Dashboard.Query = IntamacDAL_SPR.DashboardCountsDB.eQueryType.LoadCountsAllProperties

        If Not IsPostBack Then

            hdnConfirmFactoryResetPanelMessage.Value = GetLocalResourceObject("PanelFactoryResetConfirm").ToString()
            hdnConfirmResetPanelMessage.Value = GetLocalResourceObject("PanelResetConfirm").ToString()

            ' non-sprue users can only see their own accounts
            If mUsersCompany.CompanyTypeID <> CInt(e_MasterCompanyTypes.ApplicationOwner) Then
                mParentCoID = mUsersCompany.MasterCoID

            End If

            ' installers are only allowed to view their own accounts
            If Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
                mInstallerID = mLoggedInUser.UserID
            End If

            If Not String.IsNullOrEmpty(mAccountID) Then
                If Not IsNothing(ConfigurationManager.AppSettings("AccountEnquiryDashboardRefresh")) Then
                    Dashboard.RefreshMilliseconds = CInt(ConfigurationManager.AppSettings("AccountEnquiryDashboardRefresh"))
                End If

                Dim clientGetter As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")

                Dim objClient As IntamacBL_SPR.Client = clientGetter.Load(mAccountID)

                LoadAndDisplayDistributorProviderInstaller(objClient)
                LoadProperties()
                LoadAndDisplayAccountDetails()
                LoadAndDisplayPropertyAddress()
                LoadGateways()
                LoadZones()

                LoadRiskLevels()
                miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Account_Viewed, "")

                GetRiskLevelByAccountProperty()

            Else
                'mAccountID cleared, so return back to AccountSearch page
                Response.Redirect("AccountSearch.aspx")
            End If


        End If

        '		btnResetPanel.Attributes.Add("onclick", "return confirmResetPanelUpdate();")
        '		btnFactoryReset.Attributes.Add("onclick", "return confirmFactoryResetPanelUpdate();")
        If IsPostBack Then
            If Not String.IsNullOrEmpty(cboZones.SelectedValue) And cboZones.SelectedValue <> GetLocalResourceObject("All") Then
                Dim valParts As String() = cboZones.SelectedValue.Split("|"c)
                If cboZones.SelectedIndex <> 0 Then
                    Dim areaId = Convert.ToInt64(valParts(0))
                    Dim propertyId = valParts(1).Trim()
                    Dashboard.AreaID = areaId

                    For Each gwItem As RadComboBoxItem In cboGateways.Items
                        If gwItem.Value.Contains(propertyId) Then
                            gwItem.Selected = True
                        End If
                    Next
                End If


            Else
                Dashboard.AreaID = Nothing
            End If
        End If
        Session("ZoneID") = Dashboard.AreaID

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        ' initialise booleans used to determine button availability
        Dim blnGatewaySelected As Boolean = Not String.IsNullOrEmpty(cboGateways.SelectedValue)
        Dim blnPropertySelected As Boolean = Not String.IsNullOrEmpty(mPropertyID)
        Dim blnAccountSelected As Boolean = Not String.IsNullOrEmpty(mAccountID)

        ' buttons visible/enabled once Gateway is selected
        placeFirmwareUpdate.Visible = blnGatewaySelected ' this one hidden 
        btnSensorsManage.Enabled = blnGatewaySelected
        btnGatewayReboot.Enabled = blnGatewaySelected
        btnSwapGateway.Enabled = blnGatewaySelected AndAlso Not Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) ' installers can never use this button

        ' buttons enabled when account/property is selected
        btnAccountEdit.Enabled = blnPropertySelected And blnAccountSelected
        btnContacts.Enabled = blnPropertySelected And blnAccountSelected
        btnEventLog.Enabled = blnPropertySelected And blnAccountSelected
        btnSupportLog.Enabled = blnPropertySelected And blnAccountSelected

        ' buttons enabled when account is selected
        btnRaiseSupportLog.Enabled = blnAccountSelected
        btnAudits.Enabled = blnAccountSelected

        Dashboard.ParentCoID = mParentCoID
        Dashboard.InstallerID = mInstallerID
        Dashboard.AccountID = mAccountID
        Dashboard.PropertyID = ""

        If Not String.IsNullOrEmpty(cboGateways.SelectedValue) Then
            ' gateway selected, so extract property id from combo value and use in Dashboard
            Dim gwValues As String() = cboGateways.SelectedValue.Split("|"c)

            If gwValues.Length > 1 Then
                Dashboard.PropertyID = gwValues(1)
            End If
            'SP-1114 Disabled the account edit button for cancel accounts
            If gwValues(0).EndsWith("-A") Then
                btnAccountEdit.Enabled = False
                btnSensorsManage.Enabled = False
                btnSwapGateway.Enabled = False
                btnGatewayReboot.Enabled = False
                btnFirmware.Enabled = False
            End If
        Else
            ' no gateway selected so use all ID's associated with the selected address (if any)
            Dashboard.PropertyID = cboProperties.SelectedValue
        End If



    End Sub
#End Region

#Region "Control Event Handlers"

    'Protected Sub btnAccessAccount_Click(sender As Object, e As EventArgs) Handles btnAccessAccount.Click

    '	Dim pass As New IntamacBL_SPR.AdminPass

    '	pass.AccountType = "SPR"

    '	pass.UserID = User.Identity.Name & "| "

    '	pass.AccountID = mAccountID

    '	pass.PropertyID = mPropertyID

    '	pass.MemberID = mTargetUsername

    '	pass.URLData = Guid.NewGuid.ToString

    '	pass.Save()

    '	If System.Configuration.ConfigurationManager.AppSettings("AdminAccessAccount") = "1" Then

    '		miscFunctions.NewWindow(Me, System.Configuration.ConfigurationManager.AppSettings("SWAUrl") & "?Ticket=" & pass.URLData, 0, 0, False, False, False, False, True, True, 500, 500)

    '	Else
    '		Response.Write(pass.URLData)
    '	End If


    'End Sub

    'Protected Sub btnAddNewRequest_Click(sender As Object, e As EventArgs) Handles btnAddNewRequest.Click

    '	mSuppReqID = 0
    '	Response.Redirect("SupportDetail.aspx")

    'End Sub

    'Protected Sub btnAddNote_Click(sender As Object, e As EventArgs) Handles btnAddNote.Click
    '	mNoteID = 0
    '	Response.Redirect("NoteDetail.aspx")
    'End Sub

    Protected Sub btnAudits_Click(sender As Object, e As EventArgs) Handles btnAudits.Click
        Response.Redirect("AuditLogs.aspx")
    End Sub

    'Protected Sub btnDiagnostics_Click(sender As Object, e As EventArgs) Handles btnDiagnostics.Click
    '	Response.Redirect("Diagnostics.aspx")
    'End Sub

    'Protected Sub btnEditAccount_Click(sender As Object, e As EventArgs) Handles btnEditAccount.Click
    '	Response.Redirect("AccountEdit.aspx")
    'End Sub

    Protected Sub btnSensorsManage_Click(sender As Object, e As EventArgs) Handles btnSensorsManage.Click
        mAccountDetailTileSelected = Nothing
        Response.Redirect("ManageSensors.aspx")
    End Sub

    'Private Sub btnFactoryReset_Click(sender As Object, e As EventArgs) Handles btnFactoryReset.Click

    '	lblPanelStatus.Visible = True
    '	If ResetPanelToFactory(lblPropertyID.Text) Then
    '		lblPanelStatus.Text = GetGlobalResourceObject("PageGlobalResources", "FactoryResetSent").ToString()
    '		'miscFunctions.AddAuditRecord(lblAccountID.Text, lblPropertyID.Text, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Gateway_Factory_Reset, "")
    '	Else
    '		lblPanelStatus.Text = GetGlobalResourceObject("PageGlobalResources", "FactoryResetSentFailed").ToString()
    '	End If

    '	UpdatePanel1.Update()
    'End Sub

    'Private Sub btnResetPanel_Click(sender As Object, e As EventArgs) Handles btnResetPanel.Click
    '	If ResetPanel() Then
    '		lblPanelStatus.Visible = True
    '		lblPanelStatus.Text = GetGlobalResourceObject("PageGlobalResources", "GatewayResetSent").ToString()
    '		'miscFunctions.AddAuditRecord(lblAccountID.Text, lblPropertyID.Text, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Gateway_Softreset, "")
    '	End If

    '	UpdatePanel1.Update()

    'End Sub

    'Private Sub btnSwapPanel_Click(sender As Object, e As EventArgs) Handles btnSwapPanel.Click
    '	Response.Redirect("SwapPanel.aspx")
    'End Sub

    'Private Sub btnSwaporRemoveCamera_Click(sender As Object, e As EventArgs) Handles btnSwaporRemoveCamera.Click
    '	Response.Redirect("SwapOrRemoveCamera.aspx")
    'End Sub

    'Private Sub btnFirmwareDetails_Click(sender As Object, e As EventArgs) Handles btnFirmwareDetails.Click
    '	Response.Redirect("FirmwareDetails.aspx")
    'End Sub

    'Private Sub dgNotes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgNotes.PageIndexChanging
    '	dgNotes.PageIndex = e.NewPageIndex
    '	ShowGrid()
    'End Sub

    'Protected Sub dgNotes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgNotes.RowCommand
    '	If e.CommandName.ToLower = "select" Then
    '		mNoteID = CInt(dgNotes.DataKeys(CInt(e.CommandArgument))("AdminNoteID"))
    '		Response.Redirect("NoteDetail.aspx")
    '	End If

    'End Sub

    'Private Sub dgRequests_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgRequests.PageIndexChanging
    '	dgRequests.PageIndex = e.NewPageIndex
    '	ShowGrid()
    'End Sub

    'Protected Sub dgRequests_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgRequests.RowCommand

    '	If e.CommandName.ToLower = "select" Then
    '		mSuppReqID = CInt(dgRequests.DataKeys(CInt(e.CommandArgument))("SupportRequestID"))
    '		Response.Redirect("SupportDetail.aspx")
    '	End If

    'End Sub

    'Private Sub grid_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgNotes.Sorting, dgRequests.Sorting

    '	Dim blnIsNotes As Boolean = (sender Is dgNotes)

    '	Dim sortDir As String = CStr(IIf(blnIsNotes, mNotesSortDir, mReqSortDir))
    '	Dim sortExpr As String = CStr(IIf(blnIsNotes, mNotesSortExpr, mReqSortExpr))

    '	If e.SortExpression = sortExpr Then
    '		If sortDir = "DESC" Then
    '			sortDir = "ASC"
    '		Else
    '			sortDir = "DESC"
    '		End If
    '	Else
    '		sortExpr = e.SortExpression
    '		sortDir = "ASC"
    '	End If

    '	If blnIsNotes Then
    '		mNotesSortDir = sortDir
    '		mNotesSortExpr = sortExpr
    '	Else
    '		mReqSortDir = sortDir
    '		mReqSortExpr = sortExpr
    '	End If

    '	ShowGrid()

    'End Sub

    'Protected Sub mnuSuppNotes_MenuItemClick(sender As Object, e As MenuEventArgs) Handles mnuSuppNotes.MenuItemClick
    '	ShowGrid()
    'End Sub

    'Protected Sub btnDeleteAccountYes_Click(sender As Object, e As EventArgs) Handles btnDeleteAccountYes.Click

    '	'Dim dtb As New Data.DataTable

    '	'Dim objNote As IntamacBL_SPR.AdminNotes
    '	'objNote = IntamacBL_SPR.ObjectManager.CreateAdminNotes(mCompanyType)

    '	'Dim dv As Data.DataView = New Data.DataView
    '	If mAccountID <> "" Then
    '		Dim cleintaccount As New ClientSPR

    '		'If m_objDB Is Nothing Then m_objDB = New IntamacDAL_SPR.ClientDB(CompanyType)

    '		'cleintaccount.DeleteAllAccountDetails(lblAccountID.Text)
    '	End If
    '	Response.Redirect("AccountSearch.aspx")
    'End Sub

    'Private Sub grdUsersOnAccount_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdUsersOnAccount.RowCommand

    '	Dim intPersonID As New Integer
    '	Dim strUsername As String
    '	Dim client As IntamacBL_SPR.ClientSPR

    '	If e.CommandName.ToLower = "select" Then

    '		intPersonID = CInt(grdUsersOnAccount.DataKeys(CInt(e.CommandArgument))("PersonID"))
    '		strUsername = CStr(grdUsersOnAccount.DataKeys(CInt(e.CommandArgument))("LoginUserUsername"))

    '		'client = IntamacBL_SPR.ObjectManager.CreateClient(mCompanyType)

    '		'client.SendPasswordResetEmail(strUsername, Membership.Providers("CustomizedProvider"))

    '		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ResetPassword", "resetPasswordSent();", True)

    '	End If

    'End Sub

    'Private Sub grdUsersOnAccount_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdUsersOnAccount.Sorting

    '    Dim sortDir As String = mUsersSortDir
    '    Dim sortExpr As String = mUsersSortExpr

    '    sortExpr = e.SortExpression
    '    sortDir = IIf(e.SortDirection = SortDirection.Ascending, "DESC", "ASC")

    '    mUsersSortDir = sortDir
    '    mUsersSortExpr = sortExpr

    '    saveSearchDetails()

    '    grdUsersOnAccount.Sort(e.SortExpression, e.SortDirection)

    '    GetUsersOnAccount(mUsername)

    'End Sub

#End Region


    ''' <summary>
    ''' Loads gateways and zones list whenever the choice of account is changed.  Refreshes the Dashboard filtering
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub AccountPropertyChanged()
        ' clear gateway selection
        cboGateways.Items.Clear()
        cboZones.Items.Clear()

        LoadGateways()
        LoadZones()
        LoadAndDisplayPropertyAddress()
        GetRiskLevelByAccountProperty()
    End Sub
    Private Sub cboProperties_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboProperties.SelectedIndexChanged
        mPropertyID = ""
        AccountPropertyChanged()
    End Sub

    Private Sub GatewayChanged()
        Dim strMacandProp() As String = Me.cboGateways.SelectedValue.Split("|")
        'MacandProp should have two values.
        If strMacandProp.Length = 2 Then
            mSelectedGateway = strMacandProp(0)
            mPropertyID = strMacandProp(1)

            For Each propItem As RadComboBoxItem In cboProperties.Items
                If propItem.Value.Contains(mPropertyID) Then
                    propItem.Selected = True
                    LoadAndDisplayPropertyAddress()
                    Exit For
                End If
            Next
        Else
            mPropertyID = String.Empty
            mSelectedGateway = String.Empty

        End If

    End Sub
    Private Sub cboGateways_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboGateways.SelectedIndexChanged
        GatewayChanged()
        'Set Session variables based on the Gateway selection changing.
        LoadZones()
    End Sub

    Private Sub btnRaiseSupportLog_Click(sender As Object, e As EventArgs) Handles btnRaiseSupportLog.Click
        mSuppReqID = 0
        mDeviceID = Nothing

        SafeRedirect("SupportDetail.aspx", True)
    End Sub

    Private Sub btnSupportLog_Click(sender As Object, e As EventArgs) Handles btnSupportLog.Click
        SafeRedirect("SupportSearch.aspx", True)
    End Sub

    Private Sub btnSwapGateway_Click(sender As Object, e As EventArgs) Handles btnSwapGateway.Click
        If Not Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
            SafeRedirect("SwapPanel.aspx", True)

        End If
    End Sub

    Private Sub btnContacts_Click(sender As Object, e As EventArgs) Handles btnContacts.Click
        SafeRedirect("Contacts.aspx", True)
        '
    End Sub

    Private Sub btnFirmware_Click(sender As Object, e As EventArgs) Handles btnFirmware.Click
        'Set Session variables based on the Gateway selection changing.
        Dim strMacandProp() As String = Me.cboGateways.SelectedValue.Split("|")
        'MacandProp should have two values.
        If strMacandProp.Length = 2 Then
            mSelectedGateway = strMacandProp(0)
            mPropertyID = strMacandProp(1)

        End If
        'The firmware update page needs the selected gateway set in the session so that it knows which gateway to update.
        'This was previously in the query string but it would be more secure in the session.
        If Not String.IsNullOrEmpty(Me.cboGateways.SelectedValue) Then
            SafeRedirect("HubFirmware.aspx", True)
        End If


    End Sub

    Private Sub btnEventLog_Click(sender As Object, e As EventArgs) Handles btnEventLog.Click
        mDeviceID = Nothing
        SafeRedirect("EventLog.aspx?clearfilters=true", True)
    End Sub

    Private Sub btnAccountEdit_Click(sender As Object, e As EventArgs) Handles btnAccountEdit.Click
        SafeRedirect("AccountEdit.aspx", True)
    End Sub

    Private Sub btnGatewayReboot_Click(sender As Object, e As EventArgs) Handles btnGatewayReboot.Click
        Dim strMacandProp() As String = Me.cboGateways.SelectedValue.Split("|")
        'MacandProp should have two values.
        If strMacandProp.Length = 2 Then
            mSelectedGateway = strMacandProp(0)
            SafeRedirect("HubReset.aspx", True)
        End If
    End Sub

    Private Sub ucDashboard_TileClick(sender As Object, e As CommandEventArgs) Handles Dashboard.TileClick
        Select Case e.CommandName
            Case Dashboard.Tiles.ActiveAlerts.ToString, Dashboard.Tiles.ActiveFaults.ToString, Dashboard.Tiles.NoFaults.ToString, Dashboard.Tiles.TotalDevices.ToString
                mAccountDetailTileSelected = [Enum].Parse(GetType(Dashboard.Tiles), e.CommandName)
                SafeRedirect("ManageSensors.aspx", True)

            Case Dashboard.Tiles.NotTested.ToString
                '		mAccountDetailTileSelected = [Enum].Parse(GetType(Dashboard.Tiles), e.CommandName)
                '	SafeRedirect("AccountSearch.aspx", True)

            Case Dashboard.Tiles.SupportTickets.ToString
                SafeRedirect("SupportSearch.aspx", True)

        End Select

    End Sub

    Private Sub cboZones_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboZones.SelectedIndexChanged

        If Not String.IsNullOrEmpty(cboZones.SelectedValue) And cboZones.SelectedValue <> GetLocalResourceObject("All") Then
            Dim valParts As String() = cboZones.SelectedValue.Split("|"c)
            If cboZones.SelectedIndex <> 0 Then
                Dim areaId = Convert.ToInt64(valParts(0))
                Dim propertyId = valParts(1).Trim()
                Dashboard.AreaID = areaId

                For Each gwItem As RadComboBoxItem In cboGateways.Items
                    If gwItem.Value.Contains(propertyId) Then
                        gwItem.Selected = True
                    End If
                Next
            Else
                Dashboard.AreaID = Nothing
            End If

        Else
            Dashboard.AreaID = Nothing
        End If

        Session("ZoneID") = Dashboard.AreaID
        GatewayChanged()

    End Sub
End Class
