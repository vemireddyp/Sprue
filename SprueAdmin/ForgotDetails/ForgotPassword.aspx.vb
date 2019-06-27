Public Class ForgotPassword
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

            Dim masterUser As New IntamacBL_SPR.MasterUserSPR
            Try
                'To select the language code from culture class
                Dim strLanguage = ""
                If mCulture IsNot Nothing AndAlso mCulture.Length > 0 Then
                    strLanguage = IntamacShared_SPR.SharedStuff.CultureCodeToLanguage(mCulture)
                End If
                If String.IsNullOrEmpty(Session("PasswordUpdatedRedirect")) Then
                    masterUser.SendPasswordResetEmail(txtUsername.Text, strLanguage)
                Else
                    masterUser.SendPasswordResetEmail(txtUsername.Text, strLanguage, Session("PasswordUpdatedRedirect"))
                End If
            Catch ex As Exception
                'log error and continue as if successful - then no info leak as to validity of username etc.
                IntamacShared_SPR.SharedStuff.LogError(ex, mCompanyType.ToString)
            End Try

            divEnterUsername.Visible = False
            btnSend.Visible = False
            divPasswordEmailInfo.Visible = True

        End If

    End Sub

End Class