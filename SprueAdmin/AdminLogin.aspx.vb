Imports IntamacShared_SPR.SharedStuff
Partial Class AdminLogin
    Inherits CultureBaseClass

    Private _requiredURL As String
    Private _countryName As String

    Public Property RequiredURL() As String
        Get
            Return _requiredURL
        End Get
        Set(ByVal value As String)
            _requiredURL = value
        End Set
    End Property

    Public Property CountryName() As String
        Get
            Return _countryName
        End Get
        Set(ByVal value As String)
            _countryName = value
        End Set
    End Property

    Protected Sub login1_Authenticate(sender As Object, e As AuthenticateEventArgs) Handles login1.Authenticate

        'Need to detemine if the user is authenticated and set e.Authenticated accordingly
        'Get the Values entered by the user

        'Set the Extra login info to a blank string before validating user
        Me.login1.FailureText = ""

        Dim loginUserName As String = login1.UserName
        Dim loginPassword As String = login1.Password

        'Determine if the username and password are valid
        If Membership.ValidateUser(loginUserName, loginPassword) AndAlso Roles.IsUserInRole(loginUserName, miscFunctions.rolenames.AdminUser.ToString) Then

            e.Authenticated = mLoggedInUser IsNot Nothing AndAlso mLoggedInUser.UserStatusID = CInt(e_UserStatus.Active)

            If e.Authenticated Then
                FormsAuthentication.SetAuthCookie(loginUserName, False)
                mUsername = loginUserName

                Response.Redirect("~/Admin/AccountSearch.aspx", True)
            End If

        End If

        If Not e.Authenticated Then
            Me.login1.FailureText = CStr(GetLocalResourceObject("IncorrectLoginError"))
            Try
                Dim memUser As MembershipUser = Membership.GetUser(loginUserName)
                If memUser.IsLockedOut Then
                    Me.login1.FailureText = CStr(GetLocalResourceObject("LockedOutError")).Replace("$$USERNAME$$", loginUserName)
                End If
            Catch

            End Try

        End If

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Dim blnDoSSOLogout = False

        lnkForgotPassword.HRef = "~/ForgotDetails/ForgotPassword.aspx"
        lnkForgotUsername.HRef = "~/ForgotDetails/ForgotUsername.aspx"
        BodyClass = "admin-bg"

        If Not (IsPostBack OrElse IsNothing(User)) AndAlso User.Identity.IsAuthenticated Then
            Session.Clear()
            FormsAuthentication.SignOut()
        End If


        'MultiLanguages enable/disable
        If System.Configuration.ConfigurationManager.AppSettings("MultiLanguages") IsNot Nothing Then
            divCulture.Visible = System.Configuration.ConfigurationManager.AppSettings("MultiLanguages")
        End If

    End Sub

    Private Sub login1_LoggingIn(sender As Object, e As LoginCancelEventArgs) Handles login1.LoggingIn

    End Sub

    Private Sub btnSignIn_Click(sender As Object, e As EventArgs) Handles btnSignIn.Click

        'Need to detemine if the user is authenticated and set loginAuthenticated accordingly
        'Get the Values entered by the user

        Dim loginUserName As String = txtusername.Text
        Dim loginPassword As String = txtPasswoard.Text
        Dim loginAuthenticated As Boolean = False

        If Membership.ValidateUser(loginUserName, loginPassword) AndAlso Roles.IsUserInRole(loginUserName, miscFunctions.rolenames.AdminUser.ToString) Then
            mUsername = loginUserName

            loginAuthenticated = mLoggedInUser IsNot Nothing AndAlso mLoggedInUser.UserStatusID = CInt(e_UserStatus.Active)

            If loginAuthenticated Then

                'get username in correct case and use that in session variable
                Dim masterUsers As IntamacBL_SPR.MasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)
                masterUsers.Load(loginUserName)

                If masterUsers IsNot Nothing Then
                    mUsername = masterUsers.Username
                End If

                If masterUsers.Terms_accepted_admin <> CInt(e_TermsAndConditionsStatus.NotAccepted) Then
                    FormsAuthentication.SetAuthCookie(mUsername, False)

                    'this session variable is set on this Login page and then used as a check in Global.asax.vb to see if session has expired before authentication
                    Session(miscFunctions.p_SessionTheme) = "SPR"

                    SafeRedirect("~/Admin/AccountSearch.aspx", True)
                Else
                    masterUsers.PasswordResetGUID = Guid.NewGuid().ToString()
                    masterUsers.Save()

                    Dim strURL As String = "~/TermAndConditions/TermAndConditions.aspx?id={0}&Mode=AD"

                    'get culture of master company based on country name
                    Dim mCompany As New IntamacBL_SPR.MasterCompanySPR
                    If mCompany.Load(masterUsers.MasterCoID) Then

                        Dim strCulture As String = CountryNameToCulture(mCompany.CountryName)

                        If Not String.IsNullOrEmpty(strCulture) Then
                            strURL = strURL & "&Culture=" & strCulture
                        End If
                    End If

                    'redirect for admin user to accept terms and conditions
                    SafeRedirect(String.Format(strURL, masterUsers.PasswordResetGUID), True)

                End If

            End If

        End If

        If Not (loginAuthenticated Or ResponseComplete) Then
            lblError.Visible = True
            lblError.Text = CStr(GetLocalResourceObject("IncorrectLoginError"))
            Session.Clear()
            Try
                Dim memUser As MembershipUser = Membership.GetUser(loginUserName)
                If memUser.IsLockedOut Then
                    lblError.Text = CStr(GetLocalResourceObject("LockedOutError")).Replace("$$USERNAME$$", loginUserName)
                End If
            Catch

            End Try

        End If

    End Sub
End Class
