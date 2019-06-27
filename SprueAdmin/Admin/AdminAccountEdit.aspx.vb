Imports System.Globalization
Imports System.Resources

Imports Telerik.Web.UI

Imports IntamacBL_SPR
Imports IntamacDAL_SPR
Imports IntamacShared_SPR.SharedStuff
Imports IntamacShared_SPR

Partial Class Admin_AdminAccountEdit
    Inherits CultureBaseClass

    Dim mValidationControl As Dictionary(Of BaseValidator, Dictionary(Of String, Object))

    Public Overrides Property mAccountID As String
        Get
            If String.IsNullOrEmpty(ViewState("mAccountID")) Then
                ViewState("mAccountID") = MyBase.mAccountID
            End If
            Return ViewState("mAccountID").ToString
        End Get
        Set(value As String)
            ViewState("mAccountID") = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates that for the, edited, end-user accountno Distributor is expected (i.e. account was created by a sprue user using the installer wizard
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>This situation should only occur if the account was created by a sprue administrator user using the installer wizard</remarks>
    Protected Property allowNoDist As Boolean
        Get
            If ViewState("allowNoDist") Is Nothing Then
                Return False
            Else
                Return Convert.ToBoolean(ViewState("allowNoDist"))
            End If

        End Get
        Set(value As Boolean)
            ViewState("allowNoDist") = value
        End Set
    End Property


    Protected Property isInstaller As String
        Get
            If ViewState("isInstaller") Is Nothing Then
                Return False
            Else
                Return Convert.ToString(ViewState("isInstaller"))
            End If

        End Get
        Set(value As String)
            ViewState("isInstaller") = value
        End Set
    End Property

    Protected Property displayStatus As e_AccountPropertyStatus
        Get
            If ViewState("displayStatus") Is Nothing Then
                Return e_AccountPropertyStatus.NotSet
            Else
                Return CType((ViewState("displayStatus")), e_AccountPropertyStatus)
            End If

        End Get
        Set(value As e_AccountPropertyStatus)
            ViewState("displayStatus") = value
        End Set
    End Property

    Protected Property activeCount As Integer
        Get
            If ViewState("activeCount") Is Nothing Then
                Return False
            Else
                Return Convert.ToInt32(ViewState("activeCount"))
            End If

        End Get
        Set(value As Integer)
            ViewState("activeCount") = value
        End Set
    End Property



    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)


        Dim objSupportedCountry As IntamacBL_SPR.SupportedCountry
        Dim objSupportedCountryList As Generic.List(Of IntamacBL_SPR.SupportedCountry)
        Dim defaultPostcodeError = PageString("PostcodeFormat")
        Dim defaultMobileError = PageString("MobileTelephoneFormat")

        objSupportedCountry = IntamacBL_SPR.ObjectManager.CreateSupportedCountry(mCompanyType)
        objSupportedCountryList = objSupportedCountry.Load

        If Not IsNothing(Request.UrlReferrer) Then
            If Request.UrlReferrer.LocalPath.ToLower.Contains("useraccountsearch") Then
                pTopMenu = "Administration"
            End If
        End If



    End Sub

    Protected Property ExistingMasterCoID As Integer
        Get
            If Not IsNothing(ViewState("ExistingMasterCoID")) Then
                Return DirectCast(ViewState("ExistingMasterCoID"), Integer)
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            ViewState("ExistingMasterCoID") = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates whther the page is in 'update' mode, or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Value stored in the ViewState so that it is available on post back</remarks>
    Protected Property blnUpdateAllowed As Boolean
        Get
            If ViewState("blnUpdateAllowed") Is Nothing Then
                Return True
            Else
                Return CBool(ViewState("blnUpdateAllowed"))
            End If
        End Get
        Set(value As Boolean)
            ViewState("blnUpdateAllowed") = value
        End Set
    End Property


    Protected ReadOnly Property ProviderParents As List(Of Integer)
        Get
            Dim retList As List(Of Integer) = DirectCast(ViewState("ProviderParents"), List(Of Integer))

            If retList Is Nothing Then
                retList = New List(Of Integer)
                ViewState("ProviderParents") = retList

            End If

            Return retList

        End Get
    End Property

    Protected ReadOnly Property InstallerParents As List(Of Integer)
        Get
            Dim retList As List(Of Integer) = DirectCast(ViewState("InstallerParents"), List(Of Integer))

            If retList Is Nothing Then
                retList = New List(Of Integer)
                ViewState("InstallerParents") = retList

            End If

            Return retList

        End Get
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

        If Not ResponseComplete Then
            Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)

            ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, cboProviders)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboDistributors, cboInstallers)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboProviders, cboDistributors)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboProviders, cboInstallers)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboInstallers, cboProviders)
            ajaxManager.AjaxSettings.AddAjaxSetting(cboInstallers, cboDistributors)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnSave, pnlDetails)

            hdnCulture.Value = mCulture

            mValidationControl = New Dictionary(Of BaseValidator, Dictionary(Of String, Object)) From {{valPhoneNumber, New Dictionary(Of String, Object) From {{"ErrorMessage", "MobileTelephoneFormatMsg"},
                                                                                                                                                                    {"ControlToValidate", txtMobile},
                                                                                                                                                                    {"ValidationExpression", "MobileTelephoneFormatExpression"}}}}
            If Not IsPostBack Then

                If mMasterCoID = 0 AndAlso String.IsNullOrEmpty(mAccountID) Then
                    If mUsersCompany.CompanyTypeID <> CInt(e_MasterCompanyTypes.SystemOwner) Then
                        ' creating new account, so populate 'accountType' selector according users company/user level
                        cboAccountType.Items.Clear()
                        cboAccountType.Items.Add(New RadComboBoxItem(PageString("EndUserText"), "E"))

                        If Roles.IsUserInRole(miscFunctions.rolenames.SupportAdmin.ToString) AndAlso {CInt(e_MasterCompanyTypes.ApplicationOwner), CInt(e_MasterCompanyTypes.Distributor)}.Contains(mUsersCompany.CompanyTypeID) Then
                            If mUsersCompany.CompanyTypeID = e_MasterCompanyTypes.ApplicationOwner Then
                                cboAccountType.Items.Add(New RadComboBoxItem(PageString("DistributorText"), CInt(e_MasterCompanyTypes.Distributor).ToString))
                            End If
                            cboAccountType.Items.Add(New RadComboBoxItem(PageString("ServiceProText"), CInt(e_MasterCompanyTypes.OperatingCompany).ToString))
                        End If

                    Else
                        ' intamac users cannot create accounts
                        SafeRedirect("UserAccountSearch.aspx", True)
                    End If
                End If

                LoadTimeZoneList()
                LoadCompanyList(Nothing)
                LoadDetails()
            End If

        End If

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        MyBase.OnPreRender(e)

        If Not ResponseComplete Then

            If cboGateways.Visible AndAlso Not String.IsNullOrEmpty(cboGateways.Text) Then
                '  cboGateways may have lost it's 'Selected.....' property values, so ensure it's set correctly
                Dim listItem As RadComboBoxItem = cboGateways.FindItemByText(cboGateways.Text)
                If listItem IsNot Nothing Then
                    cboGateways.SelectedIndex = listItem.Index
                End If
            End If

            If Not IsPostBack Then
                cboInstallers.AutoPostBack = mUsersCompany.CompanyTypeID <> CInt(e_MasterCompanyTypes.OperatingCompany)

                If cboAccountType.Items.Count = 1 Then
                    cboAccountType.SelectedIndex = 0
                    cboAccountType.Enabled = False
                End If

                If displayStatus <> e_AccountPropertyStatus.Active Then
                    For Each statusItem As RadComboBoxItem In cboGatewayStatus.Items
                        If statusItem.Value.EndsWith(displayStatus.ToString()) Then
                            statusItem.Selected = True
                            cboGatewayStatus.SelectedValue = statusItem.Value
                        End If
                    Next

                End If

            End If

            ' if selected gateway is 'Deleted' disable 'update' controls
            If cboGatewayStatus.SelectedValue IsNot Nothing AndAlso [Enum].Parse(GetType(e_AccountPropertyStatus), cboGatewayStatus.SelectedValue) = e_AccountPropertyStatus.Cancelled Then
                cboGatewayStatus.Enabled = False
                btnChangeStatusOn.Enabled = False
                dpChangeDate.Enabled = False
                RadAjaxManager.GetCurrent(Me).ResponseScripts.Add(String.Format("setGWDisableAfterPost(false, '{0}','{1}');", cboGatewayStatus.ClientID, dpChangeDate.ClientID))
            End If

            If Not String.IsNullOrEmpty(DefaultFocusID) Then
                Dim focusCtl As Control = DeepFindControl(Me, DefaultFocusID)

            End If
            'SP-1216
            isInstaller = Roles.IsUserInRole(mLoggedInUser.Username, miscFunctions.rolenames.Installer.ToString).ToString().ToLower

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "setMasterCoID", String.Format("$get('{0}').currentMasterCoID = {1};$get('{2}').AllowUpdate = '{3}';setPageElementClasses($get('{4}'), '{5}');",
                                                                                                      cboDistributors.ClientID, ExistingMasterCoID,
                                                                                                      cboAccountType.ClientID, IIf(blnUpdateAllowed, "yes", "no"),
                                                                                                      cboAccountType.ClientID, cboAccountType.SelectedValue), True)



        End If
    End Sub


    Private Sub LoadDetails()
        Dim saveItem As RadComboBoxItem

        GetCountryCodes(Nothing, "")
        lblAccountID.Text = ""
        ExistingMasterCoID = 0
        divGateways.Visible = False
        If mMasterCoID <> 0 Then
            ExistingMasterCoID = mMasterCoID

            Dim mc As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)

            mc.Load(ExistingMasterCoID)

            txtAddress1.Text = mc.Address1
            txtAddress2.Text = mc.Address2
            txtAddress3.Text = mc.Address3
            txtStateTerritory.Text = mc.Address4
            If mc.FK_TimeZoneID.HasValue Then cboTimeZone.SelectedValue = mc.FK_TimeZoneID
            txtPostcode.Text = mc.PostCode
            txtCompanyName.Text = mc.Name
            txtCountry.Text = GetCountryName(mc.CountryName)
            hdnCountryISO3.Value = mc.CountryName
            txtLandLine.Text = mc.TelNo
            txtMobile.Text = mc.MTelNo
            txtEmail.Text = mc.Email
            cboAccountType.SelectedValue = mc.CompanyTypeID.ToString
            txtFirstName.Text = mc.FirstName
            txtLastName.Text = mc.LastName
            ddlCompanyStatus.SelectedValue = [Enum].Parse(GetType(e_OpcoStatus), mc.StatusID).ToString
            cboAccountType.Enabled = False

            If mc.CompanyTypeID = CInt(e_MasterCompanyTypes.OperatingCompany) Then
                cboDistributors.SelectedValue = mc.ParentMasterCoID.ToString
            End If

            If mc.CompanyTypeID = CInt(e_MasterCompanyTypes.Distributor) Then
                chkRiskLevel.Checked = mc.AllowRiskLevel.HasValue AndAlso mc.AllowRiskLevel
            Else
                chkRiskLevel.Visible = False
            End If

            If chkRiskLevel.Visible Then
                chkRiskLevel.Enabled = mUsersCompany.CompanyTypeID = SharedStuff.e_MasterCompanyTypes.SystemOwner OrElse mUsersCompany.CompanyTypeID = SharedStuff.e_MasterCompanyTypes.ApplicationOwner
                'Added locked class To avoid the chkRiskLevel enable from javascript if it is diabled here
                If chkRiskLevel.Enabled = False AndAlso Not chkRiskLevel.CssClass.Contains("locked") Then
                    chkRiskLevel.CssClass &= " locked"
                End If
            End If

            mPageTitle = CStr(GetLocalResourceObject("EditTitle"))

            If mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.SystemOwner) OrElse Not Roles.IsUserInRole(miscFunctions.rolenames.SupportAdmin.ToString) Then
                blnUpdateAllowed = False
            End If

        ElseIf Not String.IsNullOrEmpty(mAccountID) Then
            Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnChangeStatusOn, cboGateways)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnChangeStatusOn, lblResultMsg)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnChangeStatusOn, cboGatewayStatus)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnChangeStatusOn, dpChangeDate)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnLastGWOK, cboGateways)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnLastGWOK, cboGatewayStatus)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnLastGWOK, dpChangeDate)
            ajaxManager.AjaxSettings.AddAjaxSetting(btnLastGWOK, ModalLastGateway)

            'SP-1439  Disabling the old and today days when user selecting future day 
            dpChangeDate.MinDate = DateTime.UtcNow.AddDays(1)
            If Not dpChangeDate.SelectedDate.HasValue Then
                dpChangeDate.SelectedDate = Nothing
            End If
            chkRiskLevel.Visible = False
            lblAccountID.Text = mAccountID
            Dim clientGetter As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")

            Dim objClient As IntamacBL_SPR.Client = clientGetter.Load(mAccountID)


            txtAddress1.Text = objClient.Address1
            txtAddress2.Text = objClient.Address2
            txtAddress3.Text = objClient.Address3
            txtStateTerritory.Text = objClient.Address4
            txtPostcode.Text = objClient.Postcode
            txtCountry.Text = GetCountryName(objClient.CountryName)
            hdnCountryISO3.Value = objClient.CountryName
            txtLandLine.Text = objClient.TelephoneNumber
            txtMobile.Text = objClient.MTelNo
            txtEmail.Text = objClient.EmailAddress
            cboAccountType.SelectedValue = "E"
            txtFirstName.Text = objClient.FirstName
            txtLastName.Text = objClient.Surname
            divAccountStatus.Visible = False
            cboAccountType.Enabled = False
            cboProviders.SelectedValue = objClient.ParentCompanyID.ToString
            cboInstallers.SelectedValue = objClient.InstallerID.ToString

            ' will allow 'no distributor if Client is 'owned' by ApplicationOwner (sprue) and being edited by a sprue user
            allowNoDist = mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.ApplicationOwner) AndAlso objClient.ParentCompanyID = mUsersCompany.MasterCoID
            ' stop client-side validation occurring, if required
            valDistributors.Visible = Not allowNoDist

            If Not cboProviders.SelectedIndex < 0 AndAlso cboProviders.SelectedIndex < ProviderParents.Count Then
                cboDistributors.SelectedValue = ProviderParents(cboProviders.SelectedIndex)
            Else
                '
                '   if service provider not selected 'Parent Company' may be a distributor
                cboDistributors.SelectedValue = objClient.ParentCompanyID.ToString

            End If
            mPageTitle = CStr(GetLocalResourceObject("EditTitle"))

            cboGateways.Items.Clear()

            Dim gw As New Gateways

            Dim lstGW As List(Of Gateway) = gw.Load(mAccountID, "", True)

            If lstGW IsNot Nothing AndAlso lstGW.Count > 0 Then
                Dim futureValue As e_AccountPropertyStatus = e_AccountPropertyStatus.NotSet
                Dim futureDate As Date

                activeCount = 0
                If lstGW.Count > 1 Then
                    cboGateways.Items.Add(New RadComboBoxItem(PageString("SelectGateway"), ""))
                    divSelectGateway.Attributes.Add("class", "hide divSelectGateway")
                End If

                For Each singleGateway As IntamacBL_SPR.Gateway In lstGW.OrderBy(Function(rec) rec.MACAddress)

                    'Set the text of the gateway dropdown to include the name and the mac address
                    Dim GatewayText As String = IIf(String.IsNullOrEmpty(singleGateway.Name), "N/A", singleGateway.Name) & " - " & singleGateway.MACAddress
                    'Set the mac address and the property id in the value delimitted by a pipe.
                    Dim futureData As DataRow = miscFunctions.GetEffectiveDateActions("IntaPropertytable", "PropertyStatusID", singleGateway.AccountID, singleGateway.PropertyID)

                    Dim ValueText As String = singleGateway.MACAddress & "|" & singleGateway.PropertyID & "|" & singleGateway.PropertyStatus.ToString

                    Dim isActive As Boolean = (singleGateway.PropertyStatus = e_AccountPropertyStatus.Active)

                    If displayStatus = e_AccountPropertyStatus.NotSet Then
                        displayStatus = singleGateway.PropertyStatus
                    Else
                        If displayStatus <> singleGateway.PropertyStatus Then
                            displayStatus = e_AccountPropertyStatus.Active
                        End If
                    End If

                    If futureData IsNot Nothing Then
                        If futureData("EffectiveDate") IsNot DBNull.Value Then
                            futureValue = e_AccountPropertyStatus.NotSet

                            Try
                                futureValue = [Enum].Parse(GetType(e_AccountPropertyStatus), futureData("ValueChanged").ToString)
                            Catch ex As Exception

                            End Try

                            If displayStatus <> futureValue Then
                                displayStatus = e_AccountPropertyStatus.Active
                            End If

                            futureDate = CDate(futureData("EffectiveDate"))

                            ValueText &= "|" & futureDate.ToString("yyyyMMdd") & "|" & futureValue.ToString

                            isActive = futureValue = e_AccountPropertyStatus.Active


                        End If


                    End If

                    If isActive Then
                        activeCount += 1
                    End If

                    Dim newItem As RadComboBoxItem = New Telerik.Web.UI.RadComboBoxItem(GatewayText, ValueText)

                    If lstGW.Count = 1 Then
                        newItem.Selected = True
                        If futureValue <> e_AccountPropertyStatus.NotSet Then
                            cboGatewayStatus.SelectedValue = ValueText
                            dpChangeDate.SelectedDate = futureDate
                        End If
                    End If
                    cboGateways.Items.Add(newItem)
                Next

                If displayStatus = e_AccountPropertyStatus.Cancelled Then
                    SetControlDisabledClass(cboGatewayStatus)
                    SetControlDisabledClass(btnChangeStatusOn)
                    SetControlDisabledClass(dpChangeDate)
                    SetControlDisabledClass(dpChangeDate.DateInput)
                    SetControlDisabledClass(dpChangeDate.DatePopupButton)
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "setActiveCount", String.Format("activeCount= {0};", activeCount), True)
                divGateways.Visible = True

            End If
        Else

        End If

        If ExistingMasterCoID <> 0 OrElse Not String.IsNullOrEmpty(mAccountID) Then

            ' editing existing account, save selected account type, remove type list, re-instate selected
            saveItem = cboAccountType.SelectedItem
            cboAccountType.Items.Clear()
            cboAccountType.Items.Add(saveItem)

            btnSave.Text = PageString("SaveAccountButton")
            btnSave.Visible = blnUpdateAllowed

        End If
        btnSave.Visible = blnUpdateAllowed
        btnChangeStatus.Visible = blnUpdateAllowed AndAlso Not Roles.IsUserInRole(miscFunctions.rolenames.SupportUser.ToString)
        ddlCompanyStatus.Enabled = btnChangeStatus.Visible
        btnChangeStatusOn.Visible = blnUpdateAllowed AndAlso Not Roles.IsUserInRole(miscFunctions.rolenames.SupportUser.ToString)
        dpChangeDate.Enabled = btnChangeStatusOn.Visible
        cboGatewayStatus.Enabled = btnChangeStatusOn.Visible

        If blnUpdateAllowed Then
            If String.IsNullOrEmpty(DefaultFocusID) Then
                Select Case cboAccountType.SelectedValue
                    Case "E"
                        If cboDistributors.Items.Count > 0 Then
                            DefaultFocusID = cboDistributors.ID
                        ElseIf cboProviders.Items.Count > 0 Then
                            DefaultFocusID = cboProviders.ID
                        End If
                        DefaultFocusID = txtFirstName.ID
                    Case "3"
                        DefaultFocusID = cboDistributors.ID

                    Case "4"
                        If Not String.IsNullOrEmpty(cboDistributors.SelectedValue) Then
                            DefaultFocusID = cboProviders.ID
                        Else
                            DefaultFocusID = cboDistributors.ID
                        End If
                End Select
                If String.IsNullOrEmpty(DefaultFocusID) Then
                    DefaultFocusID = txtFirstName.ID
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Populate the time zone dropdown list with all applicable time zones
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTimeZoneList()

        Dim timeZone As IntamacBL_SPR.IntamacTimeZone = Nothing
        Dim lstTimeZones As List(Of IntamacBL_SPR.IntamacTimeZone) = Nothing

        Try

            'clear the time zones dropdown initially
            cboTimeZone.Items.Clear()

            timeZone = New IntamacBL_SPR.IntamacTimeZone

            'load the applicable time zones
            lstTimeZones = timeZone.Load()

            'put each time zone into the dropdown, setting the TimeZoneID as the value of each of the dropdown elements
            For Each tz As IntamacBL_SPR.IntamacTimeZone In lstTimeZones
                Dim rtz As New RadComboBoxItem

                rtz.Value = tz.TimeZoneID
                rtz.Text = tz.TimeZoneDescription

                cboTimeZone.Items.Add(rtz)

                rtz = Nothing
            Next

        Finally
            timeZone = Nothing
            lstTimeZones = Nothing
        End Try

    End Sub

    Private Sub LoadCompanyList(ByVal sender As RadComboBox)
        Dim masterCo As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)

        Dim intSelectedCo As Integer = 0

        If Not (IsNothing(mUsersCompany) OrElse New List(Of e_MasterCompanyTypes)({e_MasterCompanyTypes.SystemOwner, e_MasterCompanyTypes.ApplicationOwner}).Contains([Enum].Parse(GetType(e_MasterCompanyTypes), mUsersCompany.CompanyTypeID))) Then

            If mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.OperatingCompany) Then
                intSelectedCo = mUsersCompany.ParentMasterCoID
            Else
                intSelectedCo = mUsersCompany.MasterCoID

            End If
        End If

        If sender Is Nothing Then
            cboDistributors.Items.Clear()
            cboDistributors.Text = ""
        ElseIf sender Is cboDistributors Then
            cboProviders.Items.Clear()
        End If

        If cboProviders.Items.Count = 0 Then
            ProviderParents.Clear()
        End If

        cboProviders.Text = ""

        If sender Is Nothing OrElse sender IsNot cboProviders Then
            Dim data As DataTable = masterCo.LoadSearch("", 0, intSelectedCo, 0)

            For Each row As DataRow In data.Rows
                Dim blnAddRow As Boolean = False

                Select Case mUsersCompany.CompanyTypeID
                    Case CInt(e_MasterCompanyTypes.ApplicationOwner)
                        ' add all entries that are not sprue or intamac
                        blnAddRow = Not New List(Of e_MasterCompanyTypes)({e_MasterCompanyTypes.SystemOwner, e_MasterCompanyTypes.ApplicationOwner}).Contains([Enum].Parse(GetType(e_MasterCompanyTypes), row("CompanyTypeID")))

                    Case CInt(e_MasterCompanyTypes.Distributor)
                        ' add users own company and child providers
                        blnAddRow = (row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor) AndAlso row("MasterCoID") = mUsersCompany.MasterCoID) OrElse (row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany) AndAlso row("ParentMasterCoID") = mUsersCompany.MasterCoID)

                    Case CInt(e_MasterCompanyTypes.OperatingCompany)
                        ' add users own and parent company
                        blnAddRow = (row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor) AndAlso row("MasterCoID") = mUsersCompany.ParentMasterCoID) OrElse (row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany) AndAlso row("MasterCoID") = mUsersCompany.MasterCoID)
                End Select


                If blnAddRow Then
                    If sender Is Nothing AndAlso row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.Distributor) Then
                        Dim newItem As New RadComboBoxItem(row("Name"), CStr(row("MasterCoID")))

                        cboDistributors.Items.Add(newItem)

                    ElseIf (sender Is Nothing OrElse sender Is cboDistributors) AndAlso row("CompanyTypeID") = CInt(IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany) _
                        AndAlso (sender Is Nothing OrElse String.IsNullOrEmpty(sender.SelectedValue) OrElse row("ParentMasterCoID") = sender.SelectedValue) Then
                        cboProviders.Items.Add(New RadComboBoxItem(row("Name"), CStr(row("MasterCoID"))))

                        If row("ParentMasterCoID") IsNot DBNull.Value Then
                            ProviderParents.Add(CInt(row("ParentMasterCoID")))
                        Else
                            ProviderParents.Add(0)
                        End If
                    End If

                End If
            Next
        End If

        ' distributors droplist only enabled if more than one choice available
        If cboDistributors.Items.Count = 1 Then
            cboDistributors.SelectedIndex = 0
        End If

        If mUsersCompany.CompanyTypeID = CType(e_MasterCompanyTypes.Distributor, Integer) Or cboDistributors.Items.Count = 1 Then
            SetControlDisabledClass(cboDistributors)
        End If

        If cboProviders.Items.Count = 1 Then
            cboProviders.SelectedIndex = 0
        End If

        If mUsersCompany.CompanyTypeID = CType(e_MasterCompanyTypes.OperatingCompany, Integer) Then
            SetControlDisabledClass(cboProviders)
            If cboProviders.Items.Count = 1 Then
                cboProviders.SelectedIndex = 0
            End If
        Else
            If cboProviders.Items.Count > 0 Then
                RemoveControlDisabledClass(cboProviders)
            Else
                SetControlDisabledClass(cboProviders)
            End If
        End If
        LoadInstallersList()

    End Sub

    Private Sub SetControlDisabledClass(ByVal ctl As WebControl)
        If Not (ctl.CssClass Is Nothing OrElse ctl.CssClass.Contains("modeDisabled")) Then
            ctl.CssClass &= " modeDisabled"
        End If
    End Sub

    Private Sub RemoveControlDisabledClass(ByVal ctl As WebControl)
        If ctl.CssClass IsNot Nothing AndAlso ctl.CssClass.Contains("modeDisabled") Then
            ctl.CssClass.Replace("modeDisabled", String.Empty)
        End If
    End Sub

    Private Sub LoadInstallersList()
        Dim masterCoId As Integer = 0
        cboInstallers.Items.Clear()
        InstallerParents.Clear()
        cboInstallers.Text = ""

        If Not Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
            If Not String.IsNullOrEmpty(cboProviders.SelectedValue) Then
                masterCoId = CInt(cboProviders.SelectedValue)
            End If

            If masterCoId = 0 AndAlso Not String.IsNullOrEmpty(cboDistributors.SelectedValue) Then
                masterCoId = CInt(cboDistributors.SelectedValue)
            End If

            Dim masterUsers As IntamacBL_SPR.MasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)

            Dim dtbInstallerUsers As DataTable = masterUsers.dtbLoadMasterUsers(masterCoId, "", "", "", "", 0, "installer")

            If Not IsNothing(dtbInstallerUsers) Then
                For Each userRow As DataRow In dtbInstallerUsers.Rows
                    cboInstallers.Items.Add(New RadComboBoxItem(String.Format("{0}, {1}", userRow("LastName"), userRow("FirstName")), userRow("UserID").ToString()))
                    If userRow("MasterCoID") IsNot DBNull.Value Then
                        InstallerParents.Add(CInt(userRow("MasterCoID")))
                    Else
                        InstallerParents.Add(0)
                    End If
                Next
            End If
            If cboInstallers.Items.Count > 0 Then
                cboInstallers.Enabled = True
            Else
                cboInstallers.Enabled = False
            End If
        Else
            cboInstallers.Items.Add(New RadComboBoxItem(String.Format("{0}, {1}", mLoggedInUser.Lastname, mLoggedInUser.Firstname), mLoggedInUser.UserID.ToString()))
            cboInstallers.SelectedIndex = 0
            SetControlDisabledClass(cboInstallers)

        End If
        If cboDistributors.SelectedIndex < 0 And cboProviders.SelectedIndex < 0 Then
            cboInstallers.Enabled = False
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If blnUpdateAllowed AndAlso Not ResponseComplete Then

            Dim countryCode = GetCountryCode(hdnCountryISO3.Value)

            ' disable validators for unneeded combo boxes etc.
            If Not IsNothing(cboAccountType.SelectedValue) Then

                Select Case cboAccountType.SelectedValue
                    Case "E"
                        valCompanyName.Enabled = False
                        ' do not perform server side validation of Distributor if not required
                        valDistributors.Enabled = Not allowNoDist

                    Case "3"
                        valDistributors.Enabled = False

                End Select

                valTimeZone.Enabled = (Not "E".Equals(cboAccountType.SelectedValue))

                Page.Validate(btnSave.ValidationGroup)

                If Page.IsValid Then
                    Select Case cboAccountType.SelectedValue
                        Case "E"
                            Dim ca As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")

                            Dim objClient As ClassLibrary_Interface.iClient = (IIf(String.IsNullOrEmpty(mAccountID), New IntamacBL_SPR.Client(), ca.Load(mAccountID)))

                            objClient.FirstName = txtFirstName.Text
                            objClient.Surname = txtLastName.Text
                            objClient.Address1 = txtAddress1.Text
                            objClient.Address2 = txtAddress2.Text
                            objClient.Address3 = txtAddress3.Text
                            objClient.Address4 = txtStateTerritory.Text
                            objClient.Postcode = txtPostcode.Text
                            objClient.AccType = e_CompanyType.SPR.ToString

                            'objClient.CountryCode = cboCountry.SelectedValue
                            objClient.CountryCode = countryCode
                            objClient.CountryName = hdnCountryISO3.Value
                            objClient.Culture = CountryNameToCulture(hdnCountryISO3.Value)
                            objClient.MTelNo = txtMobile.Text
                            objClient.TelephoneNumber = txtLandLine.Text
                            objClient.EmailAddress = txtEmail.Text

                            '
                            '   selecting provider not mandatory, use suitable MasterCoID for Client table entry Parent Company
                            If Not String.IsNullOrEmpty(cboProviders.SelectedValue) Then
                                objClient.ParentCompanyID = CInt(cboProviders.SelectedValue)
                            ElseIf Not String.IsNullOrEmpty(cboDistributors.SelectedValue) Then
                                objClient.ParentCompanyID = CInt(cboDistributors.SelectedValue)
                            Else
                                objClient.ParentCompanyID = mUsersCompany.MasterCoID
                            End If

                            If Not String.IsNullOrEmpty(cboInstallers.SelectedValue) Then
                                objClient.InstallerID = CInt(cboInstallers.SelectedValue)
                            ElseIf Roles.IsUserInRole(miscFunctions.rolenames.Installer.ToString) Then
                                'telerik selected value seems to get lost so reset here
                                cboInstallers.Items.Clear()
                                cboInstallers.Items.Add(New RadComboBoxItem(String.Format("{0}, {1}", mLoggedInUser.Lastname, mLoggedInUser.Firstname), mLoggedInUser.UserID.ToString()))
                                cboInstallers.SelectedIndex = 0
                                objClient.InstallerID = CInt(cboInstallers.SelectedValue)
                            End If

                            Dim blnUpdated As Boolean = False

                            If lblAccountID.Text <> "" Then
                                If ca.Update(objClient) Then
                                    miscFunctions.AddAuditRecord(objClient.AccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Account_Details_Updated, "Administrator Updated account.")
                                    blnUpdated = True
                                End If
                            Else
                                If ca.Insert(objClient) Then

                                    'Insert the new person
                                    Dim objPersonAgent As PersonAgent = ObjectManager.CreatePersonAgent("SQL")
                                    Dim objPerson As New Person

                                    objPerson.AccountID = objClient.AccountID
                                    objPerson.FirstName = txtFirstName.Text & " " & txtLastName.Text
                                    objPerson.LastName = txtFirstName.Text & " " & txtLastName.Text
                                    objPerson.CountryCode = GetCountryCode(hdnCountryISO3.Value)
                                    objPerson.TelNo = txtLandLine.Text
                                    objPerson.Email = txtEmail.Text
                                    objPerson.PersonGUID = Guid.NewGuid
                                    objPerson.PersonGUID_date = Date.UtcNow
                                    objPerson.Terms_accepted_app = e_TermsAndConditionsStatus.NotAccepted
                                    objPerson.Terms_accepted_user = e_TermsAndConditionsStatus.NotAccepted

                                    If objPersonAgent.Insert(objPerson) Then
                                        Dim strCulture As String = IntamacShared_SPR.SharedStuff.CountryNameToCulture(objClient.CountryName)

                                        Dim strLanguage As String = CultureCodeToLanguage(strCulture)

                                        Dim mode As String = "US"
                                        If objPerson IsNot Nothing Then
                                            objPersonAgent.SendEmailTermAndConditionsToAccept(objPerson, mode, strLanguage)
                                        End If

                                    End If

                                    miscFunctions.AddAuditRecord(objClient.AccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Account_Details_Created, "Administrator created account.")
                                    blnUpdated = True
                                End If
                            End If

                            If blnUpdated Then
                                Response.Redirect("UserAccountSearch.aspx")
                            End If

                        Case "3", "4"
                            Dim mc As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)

                            If ExistingMasterCoID = 0 Then
                                If cboAccountType.SelectedValue = CInt(e_MasterCompanyTypes.Distributor).ToString Then
                                    mc.ParentMasterCoID = mUsersCompany.MasterCoID ' this assumes user's company is Sprue
                                Else
                                    mc.ParentMasterCoID = CInt(cboDistributors.SelectedValue)
                                End If
                            Else
                                mc.Load(ExistingMasterCoID)
                            End If

                            If cboAccountType.SelectedValue = CInt(e_MasterCompanyTypes.Distributor).ToString Then
                                mc.AllowRiskLevel = chkRiskLevel.Checked
                            End If

                            mc.Address1 = txtAddress1.Text
                            mc.Address2 = txtAddress2.Text
                            mc.Address3 = txtAddress3.Text
                            mc.Address4 = txtStateTerritory.Text

                            If cboTimeZone.SelectedValue IsNot Nothing Then
                                Dim timeZoneId As Integer
                                If Int32.TryParse(cboTimeZone.SelectedValue, timeZoneId) Then
                                    mc.FK_TimeZoneID = cboTimeZone.SelectedValue
                                End If
                            End If
                            mc.PostCode = txtPostcode.Text
                            mc.Name = txtCompanyName.Text
                            'mc.CountryCode = cboCountry.SelectedValue
                            mc.CountryCode = countryCode
                            mc.CountryName = hdnCountryISO3.Value
                            mc.TelNo = txtLandLine.Text
                            mc.MTelNo = txtMobile.Text
                            mc.Email = txtEmail.Text
                            mc.StatusID = CInt([Enum].Parse(GetType(e_OpcoStatus), ddlCompanyStatus.SelectedValue))
                            mc.CompanyTypeID = CInt(cboAccountType.SelectedValue)
                            mc.FirstName = txtFirstName.Text
                            mc.LastName = txtLastName.Text

                            If mc.Save() Then
                                Response.Redirect("UserAccountSearch.aspx")
                            End If

                    End Select
                End If

                'Used below snippet when debugging to find out which validator had failed.
                'Else
                '    Dim msg As String
                '    ' Loop through all validation controls to see which 
                '    ' generated the error(s).
                '    Dim oValidator As IValidator
                '    For Each oValidator In Validators
                '        If oValidator.IsValid = False Then
                '            msg = msg & "<br />" & oValidator.ErrorMessage
                '        End If
                '    Next
            End If

        End If
    End Sub

    Private Sub GetCountryCodes(ByVal listBox As RadComboBox, ByVal countryCode As String)

        Dim objSupportedCountry As IntamacBL_SPR.SupportedCountry
        Dim objSupportedCountryList As Generic.List(Of IntamacBL_SPR.SupportedCountry)

        'listBox.Items.Clear()

        objSupportedCountry = IntamacBL_SPR.ObjectManager.CreateSupportedCountry(mCompanyType)
        objSupportedCountryList = objSupportedCountry.Load

        For Each CountryItem As IntamacBL_SPR.SupportedCountry In objSupportedCountryList
            'Dim item As New RadComboBoxItem(CountryItem.CountryName, CountryItem.CountryCode)

            'If item.Value = countryCode Then
            '    item.Selected = True
            'End If
            'listBox.Items.Add(item)


            Dim strCulture As String = PageString(String.Format("Culture{0}", CountryItem.CountryCode))

            If Not String.IsNullOrEmpty(strCulture) Then
                Dim cltr As CultureInfo = CultureInfo.CreateSpecificCulture(strCulture)

                Dim resMan As New ResourceManager(GetType(Resources.ValidationCtlResources))

                If Not IsNothing(resMan) Then

                    For Each currValidator As BaseValidator In mValidationControl.Keys
                        Dim strValue As String = ""

                        If mValidationControl(currValidator).ContainsKey("ErrorMessage") Then
                            Try
                                strValue = GetGlobalResourceObject("ValidationCtlResources", mValidationControl(currValidator)("ErrorMessage")) ' message must be in users culture
                            Catch ex As Exception

                            End Try
                            If Not String.IsNullOrEmpty(strValue) Then
                                ClientScript.RegisterExpandoAttribute(currValidator.ClientID, String.Format("ErrorMessage{0}", CountryItem.CountryCode), strValue)
                            End If
                        End If
                        If mValidationControl(currValidator).ContainsKey("ValidationExpression") Then
                            If mValidationControl(currValidator).ContainsKey("ValidationExpression") Then
                                Try
                                    strValue = resMan.GetString(mValidationControl(currValidator)("ValidationExpression"), cltr)
                                Catch ex As Exception

                                End Try
                            End If
                            If Not String.IsNullOrEmpty(strValue) Then
                                ClientScript.RegisterExpandoAttribute(currValidator.ClientID, String.Format("ValidationExpression{0}", CountryItem.CountryCode), strValue)

                            End If
                        End If

                    Next

                End If
            End If
        Next

    End Sub

    Private Sub btnChangeStatus_Click(sender As Object, e As EventArgs) Handles btnChangeStatus.Click

        If blnUpdateAllowed AndAlso Not ResponseComplete Then
            If ExistingMasterCoID <> 0 Then

                Dim mc As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)

                mc.Load(ExistingMasterCoID)

                mc.StatusID = CInt([Enum].Parse(GetType(e_OpcoStatus), ddlCompanyStatus.SelectedValue))

                If mc.Save() Then
                    Response.Redirect("UserAccountSearch.aspx")
                End If
            End If
        End If

    End Sub

    Private Sub companies_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboDistributors.SelectedIndexChanged, cboProviders.SelectedIndexChanged
        If Not ResponseComplete Then
            LoadCompanyList(DirectCast(sender, RadComboBox))

            Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)

            If sender Is cboDistributors Then
                DefaultFocusID = cboProviders.ID
            ElseIf sender Is cboProviders Then
                If Not cboProviders.SelectedIndex < 0 AndAlso ProviderParents.Count > cboProviders.SelectedIndex Then
                    cboDistributors.SelectedValue = ProviderParents(cboProviders.SelectedIndex).ToString
                End If
                DefaultFocusID = cboInstallers.ID
            End If

        End If
    End Sub

    Private Sub cboInstallers_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboInstallers.SelectedIndexChanged
        If Not cboInstallers.SelectedIndex < 0 AndAlso InstallerParents.Count > cboInstallers.SelectedIndex Then
            cboProviders.SelectedValue = InstallerParents(cboInstallers.SelectedIndex).ToString
        End If
        If Not cboProviders.SelectedIndex < 0 AndAlso ProviderParents.Count > cboProviders.SelectedIndex Then
            cboDistributors.SelectedValue = ProviderParents(cboProviders.SelectedIndex).ToString
        End If
        DefaultFocusID = txtFirstName.ID

    End Sub

    Private Sub btnChangeStatusOn_Click(sender As Object, e As EventArgs) Handles btnChangeStatusOn.Click, btnLastGWOK.Click, btnModalAllGateways.Click, btnModalAllGatewaysActive.Click

        Dim selectedGateways As New NameValueCollection
        Dim activateDate As Date = Date.UtcNow

        If blnUpdateAllowed AndAlso Not ResponseComplete Then
            'Installer can't cancel the Last Gateway  SP-1216
            If isInstaller And activeCount < 2 Then
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(cboGatewayStatus.SelectedValue) Then
                If dpChangeDate.SelectedDate.HasValue Then
                    activateDate = dpChangeDate.SelectedDate
                End If

                'Get selected gateways to act upon
                selectedGateways = GetSelectedGateway()

            End If

            If activateDate.Date = Date.UtcNow.Date Then

                Dim gw As New Gateways
                Dim allGateways As List(Of Gateway) = gw.Load(mAccountID, "", True)

                Dim propertyAgent As PropertyAgent = ObjectManager.CreatePropertyAgent("SQL")
                Dim newStatus As e_AccountPropertyStatus = [Enum].Parse(GetType(e_AccountPropertyStatus), cboGatewayStatus.SelectedValue)
                Dim blnDisableControls As Boolean = False

                For Each accountGateway As IntamacBL_SPR.Gateway In allGateways

                    'If selected Gateway, process status change
                    If selectedGateways.AllKeys.Contains(accountGateway.MACAddress) Then

                        Dim objProperty As PropertyBase = GetProperty(propertyAgent, accountGateway.PropertyID)

                        ' can only change the status of a property that has not already been Deleted (Cancelled)
                        If objProperty IsNot Nothing AndAlso objProperty.PropertyStatusID <> CInt(e_AccountPropertyStatus.Cancelled) Then

                            ' if account 'deleted' (cancelled) set all sensors 'IsPresent' to false and change recorded MAC address, effectively disabling the panel
                            If newStatus = e_AccountPropertyStatus.Cancelled Then
                                ArchiveGateway(accountGateway)
                            End If

                            objProperty.PropertyStatusID = newStatus
                            objProperty.PropertyDisabled = (newStatus <> e_AccountPropertyStatus.Active)

                            propertyAgent.Update(objProperty)

                            miscFunctions.AddAuditRecord(mAccountID, accountGateway.PropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_AccountStatus_Change, "New Status:  " & cboGatewayStatus.SelectedValue)

                            miscFunctions.DeleteEffectiveDateAction("IntaPropertyTable", "PropertyStatusID", mAccountID, accountGateway.PropertyID)

                        End If

                    End If
                Next

            Else
                For Each mac As String In selectedGateways.Keys

                    Dim isSaved As Boolean = miscFunctions.SetEffectiveDateActions("IntaPropertyTable", "PropertyStatusID", mAccountID, selectedGateways(mac), CInt([Enum].Parse(GetType(e_AccountPropertyStatus), cboGatewayStatus.SelectedValue)), activateDate)

                    If isSaved Then
                        miscFunctions.AddAuditRecord(mAccountID, selectedGateways(mac), User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_AccountStatus_Change, "New Status: " & cboGatewayStatus.SelectedValue & ", to be changed on " & dpChangeDate.SelectedDate)
                    End If

                Next

            End If
        End If

        LoadDetails()

        ScriptManager.RegisterStartupScript(btnLastGWOK, btnLastGWOK.GetType(), "closeModal", String.Format("$('#{0}').modal('hide');", ModalLastGateway.ClientID), True)

    End Sub

    ''' <summary>
    ''' Load property record by property ID
    ''' </summary>
    ''' <param name="propertyGetter"></param>
    ''' <param name="propertyId"></param>
    ''' <returns></returns>
    Private Shared Function GetProperty(propertyGetter As PropertyAgent, propertyId As String) As PropertyBase

        Dim objProperty As PropertyBase = Nothing

        Try

            objProperty = propertyGetter.LoadByPropertyID(propertyId)

        Catch ex As Exception
            ' most likely cause of an error here is that the property is currently canceled (so cannot be changed)
        End Try

        Return objProperty
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="singleGateway"></param>
    Private Shared Sub ArchiveGateway(singleGateway As Gateway)

        'send reset command before deleting OpenFire XMPP user in order to close all user sessions - there is an issue on OpenFire where manually deleting an account
        'on OpenFire does a full delete and closes all user sessions, where as the API userService delete method does not appear to work in the same way
        'miscFunctions.PanelSoftReset(mAccountID, singleGateway.PropertyID)

        If Not singleGateway.MACAddress.ToLower().EndsWith("-a") Then
            Dim objHubCanceller As New SprueHubDB()

            objHubCanceller.ArchiveGateway(singleGateway.MACAddress, singleGateway.PropertyID)
        End If
    End Sub

    ''' <summary>
    ''' Gets selected Gateway from drop down
    ''' </summary>
    ''' <returns></returns>
    Private Function GetSelectedGateway() As NameValueCollection

        Dim result As NameValueCollection = New NameValueCollection()
        'Combo box item selected value is | delimited set of values
        Dim splitValues As String()

        If Not String.IsNullOrEmpty(cboGateways.SelectedValue) Then
            'Get selected Gateway
            splitValues = cboGateways.SelectedValue.Split("|"c)

            If splitValues.Length >= 2 Then
                'Get MAC & property ID
                result.Add(splitValues(0), splitValues(1))
            End If

        Else
            'Get all Gateways as 'All Gateways' item Selected
            For Each item As RadComboBoxItem In cboGateways.Items
                splitValues = item.Value.Split("|"c)

                If splitValues.Length >= 2 Then
                    'Get MAC & property ID
                    result.Add(splitValues(0), splitValues(1))
                End If

            Next
        End If

        Return result
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function wmGetCountryName(ByVal strCountryISO3 As String) As String
        Dim sessionCulture As String
        Try
            sessionCulture = HttpContext.Current.Session(miscFunctions.c_SessionCulture.ToString())
            'Check the current session variable is available.If the current session variable is available then set to the corresponding culture 
            If sessionCulture IsNot Nothing Then
                'CAW 20161130 SP-1221 Need to set culture on webmethod before querying resource objects or incorrect resource returned
                System.Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo(HttpContext.Current.Session(miscFunctions.c_SessionCulture).ToString())
                System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo(HttpContext.Current.Session(miscFunctions.c_SessionCulture).ToString())
            End If

            Return GetCountryName(strCountryISO3)


        Catch ex As Exception
            Return ""
        End Try

    End Function


End Class
