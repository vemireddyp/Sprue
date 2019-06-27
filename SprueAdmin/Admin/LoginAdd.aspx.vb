Imports IntamacShared_SPR.SharedStuff

Partial Class LoginAdd
    Inherits CultureBaseClass

    Const VSCompanyTypes As String = "CompanyTypes"

    Private ReadOnly Property mCompanyTypes As Dictionary(Of Integer, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes)
        Get
            If IsNothing(ViewState(VSCompanyTypes)) Then
                ViewState(VSCompanyTypes) = New Dictionary(Of Integer, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes)

            End If

            Return DirectCast(ViewState(VSCompanyTypes), Dictionary(Of Integer, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes))
        End Get
    End Property


    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        ' only admin users, or Sprue support user can create other users
        If mUsersCompany Is Nothing OrElse (mUsersCompany.CompanyTypeID <> e_MasterCompanyTypes.ApplicationOwner AndAlso Not Roles.IsUserInRole(miscFunctions.rolenames.SupportAdmin.ToString)) Then
            SafeRedirect("LoginSearch.aspx", True)
            Return
        End If

        If Not IsPostBack Then
            If Not IsNothing(mUsersCompany) Then
                Dim lstCompanies As List(Of IntamacBL_SPR.MasterCompany) = mUsersCompany.LoadChildrenCompanies(mUsersCompany.MasterCoID, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.All)
                lstCompanies.Add(mUsersCompany)

                mCompanyTypes.Clear()

                For Each cmpny As IntamacBL_SPR.MasterCompany In lstCompanies
                    mCompanyTypes.Add(cmpny.MasterCoID, CType(cmpny.CompanyTypeID, IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes))
                Next


                ddlCompany.DataSource = lstCompanies
                ddlCompany.DataBind()

                LoadUserTypesList(mCompanyTypes(ddlCompany.Items(0).Value))
            End If
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Page.Validate()

        If Page.IsValid Then

            'write create adminuser record and generate email to supplied address with guid
            Dim ugAgent As IntamacBL_SPR.UserGenerationAgent = IntamacBL_SPR.ObjectManager.CreateUserGenerationAgent("SQL")
            Dim userGen As New IntamacBL_SPR.UserGeneration
            Dim oMasterCompany As IntamacBL_SPR.MasterCompanySPR = Nothing
            Dim strLanguage As String = "EN"

            userGen.CreatedDate = Now
            userGen.Email = txtEmailAdd.Text
            userGen.UserGUID = Guid.NewGuid
            userGen.RoleName = ddlUserType.SelectedValue
            userGen.MasterCoID = ddlCompany.SelectedValue

            ''Uses the master company ID to retrieve the language for that country.
            If userGen.MasterCoID > 0 Then

                oMasterCompany = New IntamacBL_SPR.MasterCompanySPR
                oMasterCompany.Load(userGen.MasterCoID)

                If oMasterCompany IsNot Nothing AndAlso Not String.IsNullOrEmpty(oMasterCompany.CountryName) Then

                    strLanguage = CountryNameToLanguage(oMasterCompany.CountryName)

                End If

            End If

            ugAgent.Save(userGen, strLanguage)

            divUserAdded.Visible = True

        End If

    End Sub

    Private Sub ddlCompany_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCompany.SelectedIndexChanged
        If ddlCompany.SelectedIndex <> -1 Then

            Dim selectedType As IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes = mCompanyTypes(ddlCompany.SelectedValue)

            LoadUserTypesList(selectedType)

        End If
    End Sub

    Private Sub LoadUserTypesList(ByVal companyType As IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes)

        ddlUserType.Items.Clear()

        If Not (Roles.IsUserInRole(miscFunctions.rolenames.SupportUser.ToString) AndAlso companyType = e_MasterCompanyTypes.ApplicationOwner) Then
            ddlUserType.Items.Add(New ListItem(GetGlobalResourceObject("PageGlobalResources", "AdminUserType"), miscFunctions.rolenames.SupportAdmin.ToString))
        End If

        If Not String.IsNullOrEmpty(ddlCompany.SelectedValue) AndAlso mUsersCompany.MasterCoID = CInt(ddlCompany.SelectedValue) Then
            ' can only create support users for one's own company
            ddlUserType.Items.Add(New ListItem(GetGlobalResourceObject("PageGlobalResources", "SupportUserType"), miscFunctions.rolenames.SupportUser.ToString))

        End If

        If companyType = IntamacShared_SPR.SharedStuff.e_MasterCompanyTypes.OperatingCompany Then
            ddlUserType.Items.Add(New ListItem(GetGlobalResourceObject("PageGlobalResources", "InstallerUserType"), miscFunctions.rolenames.Installer.ToString))

        End If
    End Sub
End Class
