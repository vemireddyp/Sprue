Imports IntamacBL_SPR
Imports IntamacShared_SPR.SharedStuff

Partial Class Admin_LoginDetail
    Inherits CultureBaseClass

#Region "Request Variables"

    Private _objMasterUser As IntamacBL_SPR.MasterUser

    Private Property isSprueUser As Boolean
        Get
            If ViewState("isSprueUser") IsNot Nothing Then
                Return DirectCast(ViewState("isSprueUser"), Boolean)
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("isSprueUser") = value
        End Set
    End Property
    Private ReadOnly Property objMasterUser As IntamacBL_SPR.MasterUser
        Get
            If IsNothing(_objMasterUser) Then
                _objMasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(mCompanyType)
            End If

            Return _objMasterUser
        End Get
    End Property

    Private Property vsMasterCoID As Integer
        Get
            If Not IsNothing(ViewState("masterCoID")) Then
                Return CInt(ViewState("masterCoID"))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            ViewState("masterCoID") = value
        End Set
    End Property

    Private Property vsIDGuid As Guid
        Get
            If Not IsNothing(ViewState("idGuid")) Then
                Return CType(ViewState("idGuid"), Guid)
            Else
                Return Guid.Empty
            End If
        End Get
        Set(value As Guid)
            ViewState("idGuid") = value
        End Set
    End Property

    Private Property userRole As String
        Get
            Return CStr(ViewState("userRole"))
        End Get
        Set(value As String)
            ViewState("userRole") = value
        End Set
    End Property

#End Region

    'Private Sub FillSecurityQ()

    '    If Not IntamacBusinessLogic.General.FillComboFromListTable("IntaSecurityQuestion", False, ddlQuestion, ConfigurationManager.ConnectionStrings("conn").ConnectionString, False) Then
    '        Response.Redirect("~/ErrorPage.aspx")
    '    End If

    'End Sub

#Region "Page New/Overrides"

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If Not IsPostBack Then
            ' removed security question (and answer) as it's not used anywhere
            'FillSecurityQ()

            If Not IsNothing(Request("Id")) Then
                Dim isValid As Boolean = False
                Dim uga As IntamacBL_SPR.UserGenerationAgent = IntamacBL_SPR.ObjectManager.CreateUserGenerationAgent("SQL")
                Try
                    Dim UserID As New Guid(Request("Id"))
                    vsIDGuid = UserID
                    Dim userGen As IntamacBL_SPR.UserGeneration = uga.Load(UserID)

                    If Not IsNothing(userGen) Then
                        If DateDiff(DateInterval.Hour, CType(userGen.CreatedDate, Date), DateTime.Now) < 24 Then
                            Dim mco As IntamacBL_SPR.MasterCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)

                            If mco.Load(userGen.MasterCoID) Then
                                isSprueUser = mco.CompanyTypeID = CInt(e_MasterCompanyTypes.ApplicationOwner)
                                isValid = isSprueUser OrElse Session(miscFunctions.c_SessionTermsAcceptedDate) IsNot Nothing
                            End If
                            txtEmail.Text = userGen.Email
                            vsMasterCoID = userGen.MasterCoID
                            userRole = userGen.RoleName
                        End If
                    End If

                    If isValid Then
                        divContent.Visible = True
                        divStatus.Visible = False
                        txtMasterUser.Focus()
                    Else
                        divContent.Visible = False
                        divLinkExpired.Visible = True
                    End If

                Catch ex As Exception
                    Response.Redirect("~/ErrorPage.aspx")
                End Try

                mBtnBack.Visible = False

            ElseIf mEditUserID = 0 Then
                Response.Redirect("~")
            End If

        End If

        If mEditUserID <> 0 AndAlso objMasterUser.UserID <> mEditUserID Then

            objMasterUser.Load(mEditUserID)
            If Not IsPostBack Then
                PopulateUser()

            End If

        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        txtPassword.Attributes("value") = txtPassword.Text
        txtConfirmPassword.Attributes("value") = txtConfirmPassword.Text

    End Sub

#End Region


#Region "Private Methods"

    Private Function CreateMembershipUser() As Boolean

        Dim usr As MembershipUser
        Dim memStatus As MembershipCreateStatus

        usr = Membership.CreateUser(txtMasterUser.Text, txtPassword.Text, txtEmail.Text, "Question Not Used", "Yes", True, memStatus)

        Select Case memStatus
            Case MembershipCreateStatus.DuplicateEmail
                lblValidation.Text = PageString("CreateUserFailedEmail")
                lblValidation.Visible = True
            Case MembershipCreateStatus.DuplicateUserName
                lblValidation.Text = PageString("CreateUserFailedUsername")
                lblValidation.Visible = True
            Case MembershipCreateStatus.InvalidEmail
                lblValidation.Text = PageString("CreateUserFailedEmailInvalid")
                lblValidation.Visible = True

        End Select

        Return memStatus = MembershipCreateStatus.Success

    End Function

    Private Sub PopulateUser()

        txtMasterUser.ReadOnly = True

        divOwnInfo.Visible = (mEditUserID = mLoggedInUser.UserID)
        'ddlStatus.Enabled = (Not divOwnInfo.Visible AndAlso objMasterUser.LoadMasterUsers().Count > 0)

        If divOwnInfo.Visible Then
            valPasswordRequired.Enabled = False
            btnResetPassword.Visible = False
        Else
            btnResetPassword.Visible = True
        End If

        txtMasterUser.Text = objMasterUser.Username
        txtFirstName.Text = IntamacShared_SPR.SharedStuff.ProperCase(objMasterUser.Firstname)
        txtLastName.Text = IntamacShared_SPR.SharedStuff.ProperCase(objMasterUser.Lastname)
        txtEmail.Text = objMasterUser.Email
        vsMasterCoID = objMasterUser.MasterCoID

        If objMasterUser.UserStatusID = 0 Then
            ddlStatus.SelectedValue = CStr(IntamacShared_SPR.SharedStuff.e_UserStatus.Active)
        Else
            ddlStatus.SelectedValue = CStr(objMasterUser.UserStatusID)
        End If

        ' details can only be edited for following conditions
        '           logged in user is editing their own details
        '           target is a non-intamac account and the logged in user is from sprue
        '           target is a non-intamac account and the logged in user is an administrator of the accounts MasterCo, or it's parent
        '           target is an intamac account and the user is an intamac administrator
        txtFirstName.Enabled = divOwnInfo.Visible OrElse (Roles.IsUserInRole(miscFunctions.rolenames.SupportAdmin.ToString) _
                                                           AndAlso (mUsersCompany.CompanyTypeID = CInt(e_MasterCompanyTypes.ApplicationOwner) _
                                                                    OrElse (objMasterUser.MasterCoType = e_MasterCompanyTypes.SystemOwner AndAlso mUsersCompany.CompanyTypeID = e_MasterCompanyTypes.SystemOwner) _
                                                                    OrElse (mUsersCompany.CompanyTypeID <> e_MasterCompanyTypes.SystemOwner)))
        txtLastName.Enabled = txtFirstName.Enabled
        txtEmail.Enabled = txtFirstName.Enabled
        ddlStatus.Enabled = txtFirstName.Enabled And Not divOwnInfo.Visible

        btnSave.Visible = txtFirstName.Enabled

    End Sub

    ''' <summary>
    ''' Reset the password
    ''' </summary>
    Private Sub ResetPassword()
        Dim masterUser As New IntamacBL_SPR.MasterUserSPR
        Try
            'changed this method call according to the new parameters
            Dim strLanguage As String = "EN-GB"
            If mCulture IsNot Nothing AndAlso mCulture.Length > 0 Then
                strLanguage = IntamacShared_SPR.SharedStuff.CultureCodeToLanguage(mCulture)
            End If
            If Not String.IsNullOrEmpty(txtMasterUser.Text) Then
                lblResetPasswordInfo.Visible = True
                masterUser.SendPasswordResetEmail(txtMasterUser.Text, strLanguage)
                btnResetPassword.Enabled = False
            End If

        Catch ex As Exception
            'log error and continue as if successful - then no info leak as to validity of username etc.
            IntamacShared_SPR.SharedStuff.LogError(ex, mCompanyType.ToString)
        End Try

    End Sub

    Private Sub SaveMasterUser()

        Dim blnContinue As Boolean = True
        lblValidation.Visible = False

        'validate the logon group
        Page.Validate("")

        'fisrt check the javascript validation
        If Page.IsValid Then

            'populate the dealeruser object ready for server side validation
            objMasterUser.Username = txtMasterUser.Text
            objMasterUser.Firstname = IntamacShared_SPR.SharedStuff.ProperCase(txtFirstName.Text)
            objMasterUser.Lastname = IntamacShared_SPR.SharedStuff.ProperCase(txtLastName.Text)
            objMasterUser.Email = txtEmail.Text
            objMasterUser.UserID = mEditUserID
            objMasterUser.MasterCoID = vsMasterCoID

            If isSprueUser Then
                objMasterUser.Terms_accepted_admin = 0
            Else
                objMasterUser.Terms_accepted_admin = 2
                objMasterUser.Terms_accepted_date = IIf(Session(miscFunctions.c_SessionTermsAcceptedDate) IsNot Nothing, CDate(Session(miscFunctions.c_SessionTermsAcceptedDate)), objMasterUser.Terms_accepted_date)
            End If

            If mEditUserID = 0 Then
                objMasterUser.UserStatusID = IntamacShared_SPR.SharedStuff.e_UserStatus.Active
            Else
                objMasterUser.UserStatusID = CInt(ddlStatus.SelectedValue)
            End If

            'make sure we pass the server side business layer validation
            If Not objMasterUser.Validate Then
                blnContinue = False
                If objMasterUser.ValidationErrors.Count > 0 Then
                    lblValidation.Text = objMasterUser.ValidationErrors.Values(0)
                    lblValidation.Visible = True
                End If
            End If

            If objMasterUser.UserID = 0 Then
                'make sure we can create a user before we proceed to save to objects
                If blnContinue Then blnContinue = CreateMembershipUser()

                If blnContinue Then
                    Roles.AddUserToRole(objMasterUser.Username, miscFunctions.rolenames.AdminUser.ToString)
                    Roles.AddUserToRole(objMasterUser.Username, userRole)
                    ExpireUserGenerationLink()
                End If

            Else

                'update the membership user and set blncontinue
                'only need to update the email and active status
                Dim usr As MembershipUser
                usr = Membership.GetUser(objMasterUser.Username)
                usr.Email = txtEmail.Text
                'usr.IsApproved = IIf(cboStatus.SelectedValue = "1", True, False)
                Membership.UpdateUser(usr)
            End If



            If blnContinue Then
                'were all good so save away
                If mEditUserID <> 0 Then

                    Dim currUser As MembershipUser = Membership.GetUser(True)

                    If Not IsNothing(currUser) Then
                        objMasterUser.LastUpdatedBy = currUser.UserName
                        If divOwnInfo.Visible AndAlso Not String.IsNullOrEmpty(txtPassword.Text) Then
                            currUser.ChangePassword(currUser.ResetPassword(), txtPassword.Text)
                        End If
                    End If
                End If
                objMasterUser.Save()
                'EDITING AN EXISTING USER SO REDIRECT BACK TO USERS LIST
                If mEditUserID <> 0 Then
                    Response.Redirect("LoginSearch.aspx")
                Else
                    Dim strLanguage As String = "EN"
                    If mCulture IsNot Nothing Then
                        strLanguage = CultureCodeToLanguage(mCulture)
                    End If

                    'adding a new user
                    miscFunctions.SendUserCreatedEmail(objMasterUser, strLanguage)
                    'display confiormation then redirect to login page
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "temp", "<script type='text/javascript'>alert('" & PageString("NewUserCreated") & "');window.location.href='../adminlogin.aspx';</script>", False)
                End If

            End If

        End If

    End Sub

    Private Sub ExpireUserGenerationLink()
        Dim userId As Guid = vsIDGuid
        If Guid.Empty.Equals(userId) Then
            Exit Sub
        End If

        Dim userGen As IntamacBL_SPR.UserGenerationAgent = IntamacBL_SPR.ObjectManager.CreateUserGenerationAgent("SQL")
        userGen.Delete(userId.ToString())
        vsIDGuid = Guid.Empty
    End Sub

#End Region

#Region "control event handlers"

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveMasterUser()
    End Sub

    Private Sub btnResetPassword_Click(sender As Object, e As EventArgs) Handles btnResetPassword.Click
        ResetPassword()
    End Sub

#End Region

End Class
