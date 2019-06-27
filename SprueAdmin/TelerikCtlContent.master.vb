Imports IntamacShared_SPR
Imports Telerik.Web.UI

Partial Class TelerikCtlContent
    Inherits System.Web.UI.MasterPage

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

    Private Class topMenuItem
        Private _link As String = ""
        Public Property link As String
            Get
                Return _link
            End Get
            Set(value As String)
                _link = value
            End Set
        End Property

        Private _text As String = ""

        Public Property text As String
            Get
                Return _text
            End Get
            Set(value As String)
                _text = value
            End Set
        End Property

        Private _CssClass As String = ""

        Public Property CssClass As String
            Get
                Return _CssClass
            End Get
            Set(value As String)
                _CssClass = value
            End Set
        End Property

    End Class

    Protected Overrides Sub OnPreRender(e As EventArgs)

        Dim menuItems As New List(Of topMenuItem)

        If TypeOf Page Is CultureBaseClass AndAlso Not DirectCast(Page, CultureBaseClass).ResponseComplete Then
            For Each pageName As String In CStr(GetGlobalResourceObject("MenuResources", "TopMenuItems")).Split(","c)

                Dim strPermittedRoles As String() = CStr(GetGlobalResourceObject("MenuResources", String.Format("Menu{0}PermittedRoles", pageName))).Split(","c)
                Dim strPermittedCo As String() = CStr(GetGlobalResourceObject("MenuResources", String.Format("Menu{0}PermittedCompanies", pageName))).Split(","c)

                Dim blnItemIncluded As Boolean = False

                For Each role As String In strPermittedRoles
                    Roles.IsUserInRole(Page.User.Identity.Name, role)
                    blnItemIncluded = True
                    Exit For
                Next

                If blnItemIncluded Then
                    blnItemIncluded = False
                    For Each coID As String In strPermittedCo
                        If coID.ToLower = "all" Then
                            blnItemIncluded = True
                        ElseIf coID.ToLower = "conditional" Then
                            Dim userCo As IntamacBL_SPR.MasterCompany = DirectCast(Page, CultureBaseClass).mUsersCompany
                            If userCo IsNot Nothing Then
                                Select Case userCo.CompanyTypeID
                                    Case SharedStuff.e_MasterCompanyTypes.ApplicationOwner,
                                    SharedStuff.e_MasterCompanyTypes.SystemOwner
                                        blnItemIncluded = True
                                    Case SharedStuff.e_MasterCompanyTypes.Distributor
                                        blnItemIncluded = userCo.AllowRiskLevel
                                    Case Else
                                        blnItemIncluded = False
                                End Select
                            End If
                            'Hide the Risklevel option for support user SP-1306
                            If Roles.IsUserInRole(miscFunctions.rolenames.SupportUser.ToString) Then
                                blnItemIncluded = False
                            End If

                        ElseIf TypeOf Page Is CultureBaseClass Then
                            Dim userCo As IntamacBL_SPR.MasterCompany = DirectCast(Page, CultureBaseClass).mUsersCompany

                            If Not IsNothing(userCo) AndAlso userCo.CompanyTypeID = CInt(coID) Then
                                blnItemIncluded = True
                            End If

                        End If

                        If blnItemIncluded Then
                            Exit For

                        End If
                    Next

                End If

                If blnItemIncluded Then
                    Dim item As New topMenuItem()

                    item.link = CStr(GetGlobalResourceObject("MenuResources", String.Format("Menu{0}Page", pageName)))
                    item.text = CStr(GetGlobalResourceObject("MenuResources", String.Format("Menu{0}Text", pageName)))
                    If TypeOf Page Is CultureBaseClass Then
                        item.CssClass = CStr(IIf(pageName = CType(Page, CultureBaseClass).pTopMenu, "current", ""))
                    End If
                    menuItems.Add(item)
                End If
            Next
            rptMenu.DataSource = menuItems
            rptMenu.DataBind()

            If Page.IsAsync And Page.IsPostBack Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setupValidConfirm", "setupValidConfirm();", True)
            End If
        End If

        MyBase.OnPreRender(e)
        'MultiLanguages enable/disable
        If System.Configuration.ConfigurationManager.AppSettings("MultiLanguages") IsNot Nothing Then
            divCulture.Visible = System.Configuration.ConfigurationManager.AppSettings("MultiLanguages")
        End If

    End Sub

    Protected Sub lnkLogoff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLogoff.Click

        Session.Add("Username", HttpContext.Current.User.Identity.Name)
        Session.Abandon()
        Session.Remove("Username")
        Session.Remove("Password")
        FormsAuthentication.SignOut()
        Response.Redirect("~/AdminLogin.aspx", True)

    End Sub


End Class

