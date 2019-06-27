Imports IntamacBL_SPR
Imports ClassLibrary_Interface

Public Class ForgotUsername
    Inherits CultureBaseClass


#Region "New/Overrides"
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If Request("callbackUrl") IsNot Nothing Then
            Session("PasswordUpdatedRedirect") = Request("callbackUrl")
        End If

        If Not IsPostBack Then

        End If

        'SP-493 Get the Customer support email address from the webconfig 
        Dim SupportRequestEmailDestination As String = System.Configuration.ConfigurationManager.AppSettings("SupportRequestEmailDestination")
        If Not String.IsNullOrEmpty(SupportRequestEmailDestination) Then
            lblEmailMessage.Text = String.Format(GetLocalResourceObject("EmailMessage"), SupportRequestEmailDestination)
        End If

    End Sub
#End Region

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If String.IsNullOrEmpty(Session("PasswordUpdatedRedirect")) Then
            Response.Redirect("~/AdminLogin.aspx")
        Else
            Response.Redirect(Session("PasswordUpdatedRedirect"))
        End If
    End Sub



    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click

        If Page.IsValid Then
            'Dim masterUser As New IntamacBL_SPR.MasterUserSPR

            'Dim loginData As IntamacBL_SPR.LoginUserAgent = IntamacBL_SPR.ObjectManager.CreateLoginUserAgent("SQL")

            Dim masterUser As IntamacBL_SPR.MasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)

            Dim lstLoginusers As List(Of MasterUser) = masterUser.LoadByEmail(txtEmail.Text)

            Dim strLanguage As String = "EN-GB"
            If mCulture IsNot Nothing AndAlso mCulture.Length > 0 Then
                strLanguage = IntamacShared_SPR.SharedStuff.CultureCodeToLanguage(mCulture)
            End If

            Try
                masterUser.SendForgottenUsernameEmail(lstLoginusers, txtEmail.Text, strLanguage)

            Catch ex As Exception
                'log error and continue as if successful - then no info leak as to validity of username etc.
                IntamacShared_SPR.SharedStuff.LogError(ex, mCompanyType.ToString)
            End Try

            divEmailInfo.Visible = False
            lblEmailInfo.Visible = False
            btnSend.Visible = False
            divPasswordEmailInfo.Visible = True

        End If

    End Sub


End Class