Imports System.Xml.Serialization
Imports System.Data
Imports IntamacBL_SPR

Public Class PasswordReset
    Inherits CultureBaseClass

	Protected Overrides Sub OnLoad(e As EventArgs)
		MyBase.OnLoad(e)

		If Not Page.IsPostBack Then

            If Request("callbackUrl") IsNot Nothing Then
                Session("PasswordUpdatedRedirect") = Request("callbackUrl")
            End If

            lblCreatePassword.Text = GetLocalResourceObject("CreateYourNewPassword").ToString()
                lblInstructions.Text = GetLocalResourceObject("EnterPassword").ToString()

                'check to see if the request contains a "ID" value

                If Request("ID") IsNot Nothing Then

                    ViewState.Add("ID", Request("ID"))

                    If Not ConfirmGUID() Then

                        'password link is old so do not allow the user to attempt to change password
                        HidePasswordFields()

                    End If

                    'Form.DefaultButton = btnSaveChanges.UniqueID
                    'Page.SetFocus(txtPassword)

                Else

                    Response.Redirect("~/AdminLogin.aspx")

                End If

            End If

    End Sub

	Private Sub HidePasswordFields()
		ucPasswords.Visible = False
        lblInstructions.Text = GetLocalResourceObject("PasswordResetLink").ToString()

	End Sub

	Private Function ConfirmGUID() As Boolean

		'checks to see if a user exists with a passwordresetguid value that matches the received guid
		Dim bRet As Boolean = False
		'Dim oLoginUser As LoginUserAMN = Nothing
        Dim oMasterUser As New MasterUserSPR

        If oMasterUser.LoadByPasswordResetGUID(ViewState("ID").ToString()) Then

            If oMasterUser.Username <> String.Empty Then

                ucPasswords.MasterUserName = oMasterUser.Username
                bRet = True

            End If

        End If

		Return bRet

	End Function


End Class