Imports System.Globalization
Imports System.Resources

Imports IntamacBL_SPR
Imports IntamacShared_SPR.SharedStuff

Partial Class Admin_AccountEdit
    Inherits CultureBaseClass

    Dim mValidationControl As Dictionary(Of BaseValidator, Dictionary(Of String, Object))

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

    Protected Property EditAccount As Boolean
        Get
            If IsNothing(ViewState("EditAccount")) Then
                Return True
            Else
                Return DirectCast(ViewState("EditAccount"), Boolean)
            End If
        End Get
        Set(value As Boolean)
            If value <> EditAccount Then
                ViewState("EditAccount") = value
            End If
        End Set
    End Property

    Protected Property EditAccountAddress As Boolean
        Get
            If IsNothing(ViewState("EditAccountAddress")) Then
                Return True
            Else
                Return DirectCast(ViewState("EditAccountAddress"), Boolean)
            End If
        End Get
        Set(value As Boolean)
            If value <> EditAccountAddress Then
                ViewState("EditAccountAddress") = value
            End If
        End Set
    End Property

    Protected Property EditPropertyAddress As Boolean
        Get
            If IsNothing(ViewState("EditPropertyAddress")) Then
                Return True
            Else
                Return DirectCast(ViewState("EditPropertyAddress"), Boolean)
            End If
        End Get
        Set(value As Boolean)
            If value <> EditPropertyAddress Then
                ViewState("EditPropertyAddress") = value
            End If
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        If Not IsPostBack Then
            ' only route to this page is through AccountDetail
            mBackLocation = "AccountDetail.aspx"

        End If
        MyBase.OnLoad(e)

        hdnCulture.Value = mCulture

        mValidationControl = New Dictionary(Of BaseValidator, Dictionary(Of String, Object)) From {{valFirstName, New Dictionary(Of String, Object) From {{"ErrorMessage", "FirstNameRequiredMsg"},
                                                                                                                                                                   {"ValidationGroup", "Account"},
                                                                                                                                                           {"ControlToValidate", txtFirstName}}},
                                                                                                    {valLastName, New Dictionary(Of String, Object) From {{"ErrorMessage", "LastNameRequiredMsg"},
                                                                                                                                                                   {"ValidationGroup", "Account"},
                                                                                                                                                          {"ControlToValidate", txtLastName}}},
                                                                                                    {valAddress1, New Dictionary(Of String, Object) From {{"ErrorMessage", "Address1RequiredMsg"},
                                                                                                                                                                   {"ValidationGroup", "AccountAddress"},
                                                                                                                                                          {"ControlToValidate", txtAddress1}}},
                                                                                                    {valAddress3, New Dictionary(Of String, Object) From {{"ErrorMessage", "TownCityRequiredMsg"},
                                                                                                                                                                   {"ValidationGroup", "AccountAddress"},
                                                                                                                                                           {"ControlToValidate", txtAddress3}}},
                                                                                                    {valPostcode, New Dictionary(Of String, Object) From {{"ErrorMessage", "PostCodeRequiredMsg"},
                                                                                                                                                                  {"ValidationGroup", "AccountAddress"},
                                                                                                                                                                {"ControlToValidate", txtPostcode}}},
                                                                                                    {valCountry, New Dictionary(Of String, Object) From {{"ErrorMessage", "CountryRequiredMsg"},
                                                                                                                                                                   {"ValidationGroup", "AccountAddress"},
                                                                                                                                                          {"ControlToValidate", txtCountry}}},
                                                                                                    {valEmail, New Dictionary(Of String, Object) From {{"ErrorMessage", "EmailRequiredMsg"},
                                                                                                                                                                   {"ValidationGroup", "Account"},
                                                                                                                                                           {"ControlToValidate", txtEmail},
                                                                                                                                                                {"ValidationExpression", "EmailRequiredMsg"}}},
                                                                                                    {valEmailFormat, New Dictionary(Of String, Object) From {{"ErrorMessage", "EmailFormatMsg"},
                                                                                                                                                                   {"ValidationGroup", "Account"},
                                                                                                                                                                {"ControlToValidate", txtEmail}}},
                                                                                                    {valPropertyAddress1, New Dictionary(Of String, Object) From {{"ErrorMessage", "Address1RequiredMsg"},
                                                                                                                                                                  {"ValidationGroup", "Property"},
                                                                                                                                                                  {"ControlToValidate", txtPropertyAddress1}}},
                                                                                                    {valPropertyAddress3, New Dictionary(Of String, Object) From {{"ErrorMessage", "TownCityRequiredMsg"},
                                                                                                                                                                  {"ValidationGroup", "Property"},
                                                                                                                                                           {"ControlToValidate", txtPropertyAddress3}}},
                                                                                                    {valPropertyPostcode, New Dictionary(Of String, Object) From {{"ErrorMessage", "PostCodeRequiredMsg"},
                                                                                                                                                                  {"ValidationGroup", "Property"},
                                                                                                                                                                {"ControlToValidate", txtPropertyPostcode}}},
                                                                                                    {valPropertyCountry, New Dictionary(Of String, Object) From {{"ErrorMessage", "CountryRequiredMsg"},
                                                                                                                                                                   {"ValidationGroup", "Property"},
                                                                                                                                                          {"ControlToValidate", txtCountry}}}}
        If Not IsPostBack Then

            'Go back to Account Search when there is no mAccountID or mPropertyID which can get cleared when using browser back button, 
            'i.e. avoid showing all devices for all properties on this page
            If String.IsNullOrEmpty(mAccountID) Or String.IsNullOrEmpty(mPropertyID) Then
                If Not IsTopNavigate Then
                    Response.Redirect("AccountSearch.aspx")
                End If
            End If

            LoadDetails()
            setValidation(Me)

        ElseIf blnCultureChanged Then

            'CAW 20161130 SP-1221 set country name text to be culture specific when culture changed for both account and property addresses
            If Not String.IsNullOrEmpty(mAccountID) Then
                Dim ca As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")
                Dim objClient As IntamacBL_SPR.Client = ca.Load(mAccountID)

                If Not IsNothing(objClient) Then
                    txtCountry.Text = GetCountryName(objClient.CountryName)
                End If
            End If

            If Not String.IsNullOrEmpty(mPropertyID) Then
                Dim objProperty As ClassLibrary_Interface.iProperty
                Dim pa As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
                objProperty = pa.LoadByPropertyID(mPropertyID)

                If Not IsNothing(objProperty) Then
                    txtPropertyCountry.Text = GetCountryName(hdnPropertyCountryISO3.Value)
                End If
            End If
        End If

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        MyBase.OnPreRender(e)

        If IsPostBack Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "setHandlers", "setEventHandlers();", True)
        End If

        For Each validationCtl As BaseValidator In mValidationControl.Keys

            If validationCtl.Visible Then

                For Each prop As String In mValidationControl(validationCtl).Keys
                    Select Case prop.ToLower
                        Case "errormessage"
                            If Not String.IsNullOrEmpty(mValidationControl(validationCtl)(prop)) Then
                                Dim strMessage = DirectCast(GetGlobalResourceObject("ValidationCtlresources", mValidationControl(validationCtl)(prop)), String)
                                ClientScript.RegisterExpandoAttribute(validationCtl.ClientID, prop + "Default", strMessage)
                            End If

                        Case "validationexpression"
                            If TypeOf validationCtl Is RegularExpressionValidator Then
                                If Not String.IsNullOrEmpty(mValidationControl(validationCtl)(prop)) Then
                                    Dim strExpression = DirectCast(GetGlobalResourceObject("ValidationCtlresources", mValidationControl(validationCtl)(prop)), String)
                                    ClientScript.RegisterExpandoAttribute(validationCtl.ClientID, prop + "Default", strExpression)
                                End If
                            End If

                    End Select
                Next
            End If
        Next

    End Sub

    Private Sub LoadDetails()
        If Not String.IsNullOrEmpty(mAccountID) Then
            Dim ca As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")
            Dim objClient As IntamacBL_SPR.Client = ca.Load(mAccountID)
            If Not IsNothing(objClient) Then
                txtFirstName.Text = objClient.FirstName
                txtLastName.Text = objClient.Surname
                lblAccountID.Text = objClient.AccountID

                txtAddress1.Text = objClient.Address1
                txtAddress2.Text = objClient.Address2
                txtAddress3.Text = objClient.Address3
                txtStateTerritory.Text = objClient.Address4
                txtPostcode.Text = objClient.Postcode
                EditAccountAddress = True

                GetCountryCodes(Nothing, objClient.CountryCode)
                txtCountry.Text = GetCountryName(objClient.CountryName)
                hdnCountryISO3.Value = objClient.CountryName

                txtMobile.Text = objClient.MTelNo
                txtEmail.Text = objClient.EmailAddress
                EditAccount = True

            End If
            divAccountId.Visible = True
        Else
            divAccountId.Visible = False
            GetCountryCodes(Nothing, "")
        End If

        If Not String.IsNullOrEmpty(mPropertyID) Then

            Dim objProperty As ClassLibrary_Interface.iProperty
            Dim pa As IntamacBL_SPR.PropertyAgent = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
            objProperty = pa.LoadByPropertyID(mPropertyID)

            If Not IsNothing(objProperty) Then

                lblPropertyID.Text = objProperty.PropertyID

                txtPropertyAddress1.Text = objProperty.Address1
                txtPropertyAddress2.Text = objProperty.Address2
                txtPropertyAddress3.Text = objProperty.Address3
                txtPropertyStateTerritory.Text = objProperty.Address4
                txtPropertyPostcode.Text = objProperty.Postcode

                GetCountryCodes(Nothing, objProperty.CountryCode)
                txtPropertyCountry.Text = GetCountryName(objProperty.CountryName)
                hdnPropertyCountryISO3.Value = objProperty.CountryName

                EditPropertyAddress = True
                'btnCopyToProperty.Visible = True
                ckbCopyToProperty.Visible = True
            Else
                divProperty.Visible = False
                divPropertyError.Visible = True
                'btnCopyToProperty.Visible = False
                ckbCopyToProperty.Visible = False

            End If
        Else
            divProperty.Visible = False
            'btnCopyToProperty.Visible = False
            ckbCopyToProperty.Visible = False
        End If


    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        'Dim countryCode = ddlCountry.SelectedValue
        Dim countryCode = GetCountryCode(hdnCountryISO3.Value)

        'Dim hiddenID As String = String.Format("hdn{0}PostCodeRegEx", countryCode)
        'valPostcodeFormat.ValidationExpression = CStr(ViewState(hiddenID))

        'hiddenID = String.Format("hdn{0}PostCodeFormatError", countryCode)
        'valPostcodeFormat.ErrorMessage = CStr(ViewState(hiddenID))

        'hiddenID = String.Format("hdn{0}PostCodeRequiredError", countryCode)
        'valPostcode.ErrorMessage = CStr(ViewState(hiddenID))

        'hiddenID = String.Format("hdn{0}MobileRegEx", countryCode)
        'valPhoneNumber.ValidationExpression = CStr(ViewState(hiddenID))

        'hiddenID = String.Format("hdn{0}MobileFormatError", countryCode)
        'valPhoneNumber.ErrorMessage = CStr(ViewState(hiddenID))

        'hiddenID = String.Format("hdn{0}MobileRegEx", countryCode)
        'valPhoneNumber.ValidationExpression = CStr(ViewState(hiddenID))

        'countryCode = ddlPropCountry.SelectedValue

        'hiddenID = String.Format("hdn{0}PostCodeRegEx", countryCode)
        'valPropertyPostcodeFormat.ValidationExpression = CStr(ViewState(hiddenID))

        'hiddenID = String.Format("hdn{0}PostCodeFormatError", countryCode)
        'valPropertyPostcodeFormat.ErrorMessage = CStr(ViewState(hiddenID))

        'hiddenID = String.Format("hdn{0}PostCodeRequiredError", countryCode)
        'valPropertyPostcode.ErrorMessage = CStr(ViewState(hiddenID))

        Page.Validate()

        If Page.IsValid Then
            Dim ca As IntamacBL_SPR.ClientAgent = IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL")

            Dim objClient As ClassLibrary_Interface.iClient = (IIf(String.IsNullOrEmpty(mAccountID), New IntamacBL_SPR.Client(), ca.Load(mAccountID)))

            objClient.AccType = e_CompanyType.SPR.ToString
            objClient.FirstName = txtFirstName.Text
            objClient.Surname = txtLastName.Text
            objClient.Address1 = txtAddress1.Text
            objClient.Address2 = txtAddress2.Text
            objClient.Address3 = txtAddress3.Text
            objClient.Address4 = txtStateTerritory.Text
            objClient.Postcode = txtPostcode.Text

            'objClient.CountryCode = ddlCountry.SelectedValue
            objClient.CountryCode = countryCode
            objClient.CountryName = hdnCountryISO3.Value
            objClient.MTelNo = txtMobile.Text
            objClient.EmailAddress = txtEmail.Text

            Dim objProperty As ClassLibrary_Interface.iProperty
            Dim pa As IntamacBL_SPR.PropertyAgent

            If divProperty.Visible Then
                pa = IntamacBL_SPR.ObjectManager.CreatePropertyAgent("SQL")
                objProperty = pa.Load(mAccountID, mPropertyID)
                objProperty.Address1 = txtPropertyAddress1.Text
                objProperty.Address2 = txtPropertyAddress2.Text
                objProperty.Address3 = txtPropertyAddress3.Text
                objProperty.Address4 = txtPropertyStateTerritory.Text
                objProperty.Postcode = txtPropertyPostcode.Text
                objProperty.CountryCode = GetCountryCode(hdnPropertyCountryISO3.Value)
                objProperty.CountryName = hdnPropertyCountryISO3.Value
                objProperty.FK_TimeZoneID = GetTimeZoneID(objProperty.CountryName)
                objProperty.PropertyDisabled = False
                objProperty.PropertyStatusID = e_AccountPropertyStatus.Active

            End If

            If Not String.IsNullOrEmpty(objClient.AccountID) Then

                If ca.Update(objClient) Then
                    If IsNothing(pa) OrElse pa.Update(objProperty) Then

                        miscFunctions.AddAuditRecord(lblAccountID.Text, lblPropertyID.Text, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Account_Details_Updated, "Account and property details")

                        Response.Redirect("AccountDetail.aspx")
                    Else
                        lblValidation.Text = GetLocalResourceObject("PropertySaveError").ToString()
                        divPropertyError.Visible = True
                    End If

                Else
                    lblValidation.Text = GetLocalResourceObject("AccountSaveError").ToString()
                    divPropertyError.Visible = True
                End If
            Else
                objClient.AccType = e_CompanyType.SPR.ToString
                If ca.Insert(objClient) Then
                    mAccountID = objClient.AccountID

                    miscFunctions.AddAuditRecord(mAccountID, "", User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Account_Details_Created, "Account details")
                    LoadDetails()

                    'display the success message
                    divAdded.Visible = True

                Else
                    lblValidation.Text = GetLocalResourceObject("AccountSaveError").ToString()
                    divPropertyError.Visible = True

                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Get the TimeZoneID based on Country Name
    ''' </summary>
    ''' <param name="CountryName"></param>
    ''' <returns></returns>
    Private Function GetTimeZoneID(CountryName As String) As Integer
        If CountryName = "GBR" Then
            Return IntamacShared_SPR.SharedStuff.e_TimeZone.GBR
        ElseIf CountryName = "DEU" Then
            Return IntamacShared_SPR.SharedStuff.e_TimeZone.DEU
        ElseIf CountryName = "FRA" Then
            Return IntamacShared_SPR.SharedStuff.e_TimeZone.FRA
        ElseIf CountryName = "NLD" Then
            Return IntamacShared_SPR.SharedStuff.e_TimeZone.NLD
        Else
            Return IntamacShared_SPR.SharedStuff.e_TimeZone.GBR
        End If
    End Function

    Private Sub GetCountryCodes(ByVal listBox As DropDownList, ByVal countryCode As String)

        Dim objSupportedCountry As IntamacBL_SPR.SupportedCountry
        Dim objSupportedCountryList As Generic.List(Of IntamacBL_SPR.SupportedCountry)
        'Dim defaultPostcodeError = PageString("PostcodeFormat")
        'Dim defaultPostcodeReqError = PageString("PostcodeRequiredError")
        'Dim defaultMobileError = PageString("MobileTelephoneFormat")

        'listBox.Items.Clear()

        objSupportedCountry = IntamacBL_SPR.ObjectManager.CreateSupportedCountry(mCompanyType)
        objSupportedCountryList = objSupportedCountry.Load

        For Each CountryItem As IntamacBL_SPR.SupportedCountry In objSupportedCountryList
            'Dim item As New ListItem(CountryItem.CountryName, CountryItem.CountryCode)

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
                                strValue = resMan.GetString(mValidationControl(currValidator)("ErrorMessage"), cltr)
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

    Protected Sub applyValidationCtl(ByVal validationCtl As BaseValidator)
        If mValidationControl.ContainsKey(validationCtl) Then
            For Each prop As String In mValidationControl(validationCtl).Keys
                Select Case prop.ToLower
                    Case "errormessage"
                        If Not String.IsNullOrEmpty(mValidationControl(validationCtl)(prop)) Then
                            Dim strMessage = DirectCast(GetGlobalResourceObject("ValidationCtlResources", mValidationControl(validationCtl)(prop)), String)
                            validationCtl.ErrorMessage = strMessage
                        End If


                    Case "controltovalidate"
                        If Not IsNothing(mValidationControl(validationCtl)(prop)) Then
                            validationCtl.ControlToValidate = DirectCast(mValidationControl(validationCtl)(prop), Control).ID
                        End If

                    Case "validationexpression"
                        If TypeOf validationCtl Is RegularExpressionValidator Then
                            If Not String.IsNullOrEmpty(mValidationControl(validationCtl)(prop)) Then
                                Dim strExpression = DirectCast(GetGlobalResourceObject("ValidationCtlResources", mValidationControl(validationCtl)(prop)), String)
                                DirectCast(validationCtl, RegularExpressionValidator).ValidationExpression = strExpression
                            End If
                        End If


                    Case "validationgroup"
                        validationCtl.ValidationGroup = DirectCast(mValidationControl(validationCtl)(prop), String)
                        Select Case DirectCast(mValidationControl(validationCtl)("ValidationGroup"), String).ToLower
                            Case "account"
                                DirectCast(validationCtl, BaseValidator).Enabled = EditAccount

                            Case "accountaddress"
                                DirectCast(validationCtl, BaseValidator).Enabled = EditAccountAddress

                            Case "property"
                                DirectCast(validationCtl, BaseValidator).Enabled = EditPropertyAddress

                            Case Else
                                '
                                ' if not of 'known' group enable it if we doing any editing at all
                                DirectCast(validationCtl, BaseValidator).Enabled = EditAccount OrElse EditAccountAddress OrElse EditPropertyAddress
                        End Select

                End Select
            Next
        End If
    End Sub

    Protected Sub setValidation(ByVal startCtl As Control)

        If TypeOf startCtl Is BaseValidator AndAlso mValidationControl.ContainsKey(startCtl) Then
            applyValidationCtl(startCtl)

        End If

        If Not (IsNothing(startCtl) OrElse startCtl.Controls.Count = 0) Then
            For Each nextCtl As Control In startCtl.Controls
                setValidation(nextCtl)
            Next
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function wmGetCountryName(ByVal strCountryISO3 As String) As String

        Try

            'CAW 20161130 SP-1221 Need to set culture on webmethod before querying resource objects or incorrect resource returned
            System.Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo(HttpContext.Current.Session(miscFunctions.c_SessionCulture).ToString())
            System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo(HttpContext.Current.Session(miscFunctions.c_SessionCulture).ToString())

            Return GetCountryName(strCountryISO3)

        Catch ex As Exception
            Return ""
        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function wmGetLocalCountryName(ByVal strEnglishCountryName As String) As String

        Dim strReturn As String = ""

        Try

            'CAW 20161130 SP-1221 Once country drop down loaded, we need to lookup the country name in order o switch it into local language to display
            System.Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo(HttpContext.Current.Session(miscFunctions.c_SessionCulture).ToString())
            System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo(HttpContext.Current.Session(miscFunctions.c_SessionCulture).ToString())

            Dim strCountryName As String = HttpContext.GetGlobalResourceObject("PageGlobalResources", strEnglishCountryName.Replace(" ", "") & "_CountryName")

            If Not String.IsNullOrEmpty(strCountryName) Then
                'return original | translated to make it easier for calling function to make the change on the page
                strReturn += strEnglishCountryName + "|" + strCountryName
            End If

        Catch ex As Exception
            Return strReturn
        End Try

        Return strReturn

    End Function

    Private Sub btnAddSupport_Click(sender As Object, e As EventArgs) Handles btnAddSupport.Click
        'SP-1282 Clear the session variables (support request ID and device ID) before going to Support deatils page. 
        mSuppReqID = 0
        mDeviceID = Nothing

        SafeRedirect("SupportDetail.aspx", True)
    End Sub
End Class
