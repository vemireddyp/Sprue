
Partial Class Content
    Inherits System.Web.UI.MasterPage

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
        If Not IsPostBack Then

            Dim menuItems As New List(Of topMenuItem)

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

                            If Not IsNothing(userCo) AndAlso userCo.AllowRiskLevel = True Then
                                blnItemIncluded = True
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
        Else
            If Page.IsAsync Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setupValidConfirm", "setupValidConfirm();", True)

            End If
        End If

        MyBase.OnPreRender(e)

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

