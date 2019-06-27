Public Class SiteNormalContent
    Inherits System.Web.UI.MasterPage
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        'MultiLanguages enable/disable
        If System.Configuration.ConfigurationManager.AppSettings("MultiLanguages") IsNot Nothing Then
            divCulture.Visible = System.Configuration.ConfigurationManager.AppSettings("MultiLanguages")
        End If

    End Sub

End Class