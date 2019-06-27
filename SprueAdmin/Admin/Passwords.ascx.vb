Imports System.Data
Imports IntamacBL_SPR

Public Class Admin_Passwords
	Inherits System.Web.UI.UserControl

	Public Property MasterUserName() As String
		Get
			'If GetOldPassword Then
			'	Return CStr(Session(miscFunctions.p_SessionCurrentUserName))
			'Else
				Return CStr(ViewState("MasterUserName"))
			'End If
		End Get
		Set(value As String)
			ViewState("MasterUserName") = value
		End Set
	End Property


	Public Property GetOldPassword() As Boolean
		Get
			If IsNothing(ViewState("GetOldPassword")) Then
				Return False
			Else
				Return CBool(ViewState("GetOldPassword"))
			End If
		End Get
		Set(value As Boolean)
			ViewState("GetOldPassword") = value
		End Set
	End Property


	Private Function SaveNewPassword(ByVal newPass As String) As Boolean

		Dim bRet As Boolean = False
        Dim oLoginUser As New MasterUserSPR

        Try

            Dim oUser As System.Web.Security.MembershipUser = Nothing

            If Not String.IsNullOrEmpty(MasterUserName) Then

                oUser = Membership.Provider.GetUser(MasterUserName, True)

                'change the password


                If Not IsNothing(oUser) Then

                    'Dim oldPass As String = oUser.GetPassword()

                    oUser.IsApproved = True
                    oUser.UnlockUser()

                    Dim resetPassword As String = oUser.ResetPassword()
                    If oUser.ChangePassword(resetPassword, newPass) Then
                        lblNoLogin.Text = GetGlobalResourceObject("PageGlobalResources", "PasswordChangeSuccess").ToString() & String.Format(" {0}.", MasterUserName)

                        lblNoLogin.Visible = True
                        txtOldPass.Text = ""
                        txtPassword.Text = ""
                        txtPasswordConfirm.Text = ""

                        oLoginUser = CType(oLoginUser.LoadByUserName(oUser.UserName)(0), MasterUserSPR)

                        If Not String.IsNullOrEmpty(oLoginUser.Username) Then
                            'oLoginUser.PasswordResetGUID = ""
                            oLoginUser.Save()
                        End If

                        Login(oUser.UserName)
                    End If
                End If


            End If

        Finally
            oLoginUser = Nothing
        End Try

		Return bRet
	End Function

	Private Sub Login(ByVal loginUserName As String)

        'If Roles.IsUserInRole(loginUserName, miscFunctions.rolenames.SuperAdmin.ToString) OrElse _
        '	Roles.IsUserInRole(loginUserName, miscFunctions.rolenames.AdminUser.ToString) Then

        '	Session(miscFunctions.c_SessionCurrentUserName) = loginUserName

        '	Dim CurrentMasterUser As MasterUser = ObjectManager.CreateMasterUser(CType(System.Configuration.ConfigurationManager.AppSettings("BusLogicCompanyType"), Integer))
        '	CurrentMasterUser = CurrentMasterUser.LoadByUserName(loginUserName)(0)
        '	Session(miscFunctions.p_SessionLoggedInMasterCompanyID) = CurrentMasterUser.MasterCoID

        '	Dim CurrentMasterCompany As MasterCompany = ObjectManager.CreateMasterCompany(CType(System.Configuration.ConfigurationManager.AppSettings("BusLogicCompanyType"), Integer))
        '	CurrentMasterCompany.Load(CurrentMasterUser.MasterCoID)
        '	Session(miscFunctions.p_SessionLoggedInMasterCompanyTypeID) = CurrentMasterCompany.CompanyTypeID

        '	'Set session variable for the Role Name that the user is logged in under
        '	If Roles.IsUserInRole(loginUserName, miscFunctions.rolenames.SuperAdmin.ToString) Then
        '		Session(miscFunctions.p_SessionLoggedInRoleName) = miscFunctions.rolenames.SuperAdmin.ToString.ToLower
        '	ElseIf Roles.IsUserInRole(loginUserName, miscFunctions.rolenames.AdminUser.ToString) Then
        '		Session(miscFunctions.p_SessionLoggedInRoleName) = miscFunctions.rolenames.AdminUser.ToString.ToLower
        '	End If

        '	'For any top level user Amino or Intamac use Amino as all accounts are registered under Amino
        '	'as they are the parent companies to all distributors or opco's
        '	If CurrentMasterCompany.CompanyTypeID = IntamacShared_AMN.SharedStuff.e_MasterCompanyTypes.TopLevel Then

        '		Dim masterCo As New MasterCompanyAMN
        '		Dim dtb As New DataTable

        '		dtb = masterCo.LoadSearch("amino", 0, 0)

        '		If dtb IsNot Nothing Then

        '			If dtb.Rows.Count > 0 Then

        '				If dtb.Rows(0).Item("MasterCoID").ToString IsNot Nothing And dtb.Rows(0).Item("MasterCoID").ToString <> "" Then

        '					Session(miscFunctions.p_SessionLoggedInMasterCompanyID) = CInt(dtb.Rows(0).Item("MasterCoID").ToString)

        '				End If

        '			End If

        '		End If

        '	End If

        '	FormsAuthentication.SetAuthCookie(loginUserName, False)
        '	Dim currentServer As ICommonVars = miscFunctions.getCurrentSite
        '	Response.Redirect(currentServer.p_Common_RootFolder & "/Admin/AccountSearch.aspx", True)

        'Else
        '	lblNoLogin.Text = "User " & loginUserName & " does not have Sufficient Privileges to log in."
        '	lblNoLogin.Visible = True
        '	Exit Sub
        'End If

        If Session("PasswordUpdatedRedirect") Is Nothing Then
            Session(miscFunctions.c_SessionCurrentUserName) = loginUserName
            FormsAuthentication.SetAuthCookie(loginUserName, False)
            Response.Redirect("~/Admin/AccountSearch.aspx", True)
        Else
            Response.Redirect(Session("PasswordUpdatedRedirect"), True)
        End If

    End Sub

	Protected Sub ValidateOldPassword(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
		args.IsValid = Membership.ValidateUser(MasterUserName, txtOldPass.Text)
	End Sub

	Protected Sub ValidatePassword(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
		If txtPassword.Text().Trim().Length > 0 Then
			args.IsValid = Not txtPassword.Text.Contains(":")
		End If
	End Sub

	Protected Sub ValidatePasswordConfirm(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)

		Dim isValid As Boolean = False

		Try

			'check to see if the confirmation password and the password entered are the same
			If txtPassword.Text.Trim().Length > 0 AndAlso txtPasswordConfirm.Text.Trim().Length > 0 Then

				isValid = txtPassword.Text.Trim.Equals(txtPasswordConfirm.Text.Trim())

			End If

		Finally

			'set the response
			args.IsValid = isValid

		End Try

	End Sub

	Protected Sub btnSaveChanges_Click(sender As Object, e As EventArgs) Handles btnSaveChanges.Click

		If Page.IsValid Then

			SaveNewPassword(txtPassword.Text)
		End If

	End Sub

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		lblNoLogin.Visible = True

		txtPassword.Attributes.Add("onmousedown", "return noCopyMouse(event);")
		txtPassword.Attributes.Add("onkeydown", "return noCopyKey(event);")
		txtPassword.Attributes.Add("onpaste", "return false")

		txtPasswordConfirm.Attributes.Add("onmousedown", "return noCopyMouse(event);")
		txtPasswordConfirm.Attributes.Add("onkeydown", "return noCopyKey(event);")
		txtPasswordConfirm.Attributes.Add("onpaste", "return false")

		txtOldPass.Attributes.Add("onmousedown", "return noCopyMouse(event);")
		txtOldPass.Attributes.Add("onkeydown", "return noCopyKey(event);")
		txtOldPass.Attributes.Add("onpaste", "return false")

		PasswordStrength.TextStrengthDescriptions = GetGlobalResourceObject("PageGlobalResources", "PasswordStrengthPoor").ToString & ";" & _
													GetGlobalResourceObject("PageGlobalResources", "PasswordStrengthWeak").ToString & ";" & _
													GetGlobalResourceObject("PageGlobalResources", "PasswordStrengthAverage").ToString & ";" & _
													GetGlobalResourceObject("PageGlobalResources", "PasswordStrengthStrong").ToString & ";" & _
													GetGlobalResourceObject("PageGlobalResources", "PasswordStrengthExcellent").ToString

	End Sub

	Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

		If Not IsPostBack Then
            divOldPass.Visible = GetOldPassword
			rfvOldPass.Enabled = GetOldPassword

			If GetOldPassword Then
				Page.SetFocus(txtOldPass)
			Else
				Page.SetFocus(txtPassword)
			End If
			Page.Form.DefaultButton = btnSaveChanges.UniqueID

		End If
		'
		' text boxes in 'password' mode so values will be cleared, preserve them using the 'value' attribute
		txtOldPass.Attributes.Add("value", txtOldPass.Text)
		txtPassword.Attributes.Add("value", txtPassword.Text)
		txtPasswordConfirm.Attributes.Add("value", txtPasswordConfirm.Text)

	End Sub

End Class