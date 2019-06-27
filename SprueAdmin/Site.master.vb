
Partial Class Site
    Inherits System.Web.UI.MasterPage

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If Not Page.IsPostBack Then
            Dim useScriptCDN As String = ConfigurationManager.AppSettings(miscFunctions.c_ConfigUseJSCDN)

            If Not String.IsNullOrEmpty(useScriptCDN) Then
                ScriptManager1.EnableCdn = Convert.ToBoolean(useScriptCDN)

            End If

            Dim scriptIDs As String = ConfigurationManager.AppSettings(miscFunctions.c_ConfigJavascriptIDs)

            If Not String.IsNullOrEmpty(scriptIDs) Then
                Dim sMgr As ScriptManager = ScriptManager.GetCurrent(Page)

                If Not sMgr Is Nothing Then
                    For Each script As String In scriptIDs.Split(New Char() {","c})

                        Dim sRef As New ScriptReference()

                        If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings(script + "NAME")) Then
                            sRef.Name = ConfigurationManager.AppSettings(script + "NAME")
                        Else
                            sRef.Name = script
                        End If

                        If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings(script + "ASSMBLY")) Then
                            sRef.Assembly = ConfigurationManager.AppSettings(script + "ASSMBLY")
                        End If

                        sMgr.Scripts.Add(sRef)
                    Next

                End If
            End If
        End If

    End Sub
End Class

