Imports System.Reflection

Imports System.Web.SessionState
Imports System.Web.Optimization

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Private ReadOnly Property ShowApplicationError As Boolean
        Get
            Dim blnRetval As Boolean = False

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ShowApplicationError")) Then
                blnRetval = Convert.ToBoolean(ConfigurationManager.AppSettings("ShowApplicationError"))
            End If

            Return blnRetval
        End Get
    End Property

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup

        Dim scriptIDs As String = ConfigurationManager.AppSettings(miscFunctions.c_ConfigJavascriptIDs)

        If Not String.IsNullOrEmpty(scriptIDs) Then

            For Each script As String In scriptIDs.Split(New Char() {","c})

                Dim localPath As String = ConfigurationManager.AppSettings(script + "LOCAL")
                Dim cdnPath As String = ConfigurationManager.AppSettings(script + "CDN")
                Dim debugPath As String


                If Not String.IsNullOrEmpty(localPath) Or Not String.IsNullOrEmpty(cdnPath) Then
                    Dim scriptDef As New ScriptResourceDefinition

                    If Not String.IsNullOrEmpty(localPath) Then
                        scriptDef.Path = localPath
                        debugPath = ConfigurationManager.AppSettings(script + "LOCALDBG")

                        scriptDef.DebugPath = CStr(IIf(String.IsNullOrEmpty(debugPath), localPath, debugPath))
                    End If

                    If Not String.IsNullOrEmpty(cdnPath) Then
                        scriptDef.CdnPath = cdnPath
                        debugPath = ConfigurationManager.AppSettings(script + "CDNDBG")

                        scriptDef.CdnDebugPath = CStr(IIf(String.IsNullOrEmpty(debugPath), cdnPath, debugPath))

                    End If

                    If String.IsNullOrEmpty(ConfigurationManager.AppSettings(script + "SSL")) Then
                        scriptDef.CdnSupportsSecureConnection = True
                    Else
                        scriptDef.CdnSupportsSecureConnection = CBool(ConfigurationManager.AppSettings(script + "SSL"))
                    End If

                    If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings(script + "LOADED")) Then
                        scriptDef.LoadSuccessExpression = CStr(ConfigurationManager.AppSettings(script + "LOADED"))
                    End If

                    Dim scriptRef As New ScriptReference
                    scriptRef.Name = script

                    ScriptManager.ScriptResourceMapping.AddDefinition(script, scriptDef)

                End If



            Next
        End If

        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs

        Dim lastExc As Exception = Server.GetLastError()

        If Not (IsNothing(lastExc) OrElse (lastExc.StackTrace.Contains(Me.GetType().Name) AndAlso lastExc.StackTrace.Contains("Application_Error"))) Then

            If lastExc.Message = "File does not exist." Then
                Throw New ApplicationException(String.Format("{0} {1}", lastExc.Message, HttpContext.Current.Request.Url.ToString))
            End If

            IntamacShared_SPR.SharedStuff.LogError(lastExc, System.Configuration.ConfigurationManager.AppSettings("AppName"), "Unhandled Error")
        End If

        If Not (ShowApplicationError OrElse Request.Url.AbsolutePath.Contains("ErrorPage.aspx")) Then
            Try
                Response.Redirect("~/ErrorPage.aspx")

            Catch ex As Exception
                ' should the redirect fail do not want to create a 'cascade' of errors
            End Try
        End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

    Protected Sub Application_PreRequestHandlerExecute(ByVal sender As Object, ByVal e As EventArgs)
        'Only access session state if it is available
        If TypeOf (Context.Handler) Is IRequiresSessionState Or TypeOf (Context.Handler) Is IReadOnlySessionState Then
            'If we are authenticated AND we don't have a session here.. redirect to login page.
            Dim authenticationCookie As HttpCookie = Request.Cookies(FormsAuthentication.FormsCookieName)
            If (authenticationCookie IsNot Nothing) Then
                Dim authenticationTicket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authenticationCookie.Value)
                If (Not authenticationTicket.Expired) Then
                    'Session theme is set on the login and this is checked here to see if it is empty.
                    If (Session(miscFunctions.p_SessionTheme) Is Nothing) Then
                        'This means for some reason the session expired before the authentication ticket. Force a login.
                        FormsAuthentication.SignOut()
                        Response.Redirect(FormsAuthentication.LoginUrl, True)
                        Return
                    End If
                End If

            End If

        End If
    End Sub

End Class