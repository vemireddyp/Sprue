Imports Microsoft.VisualBasic
Imports System.Web.Security
Imports System.Data.SqlClient
Imports Intamac_Utilities
Imports IntamacBusinessLogic
Imports System.Net.Mail
Imports System.Data
Imports System.IO
Imports System.Net

Imports IntamacBL_SPR
Imports IntamacDAL_SPR
Imports IntamacShared_SPR.SharedStuff

Public Class miscFunctions

    Public Enum themes
        Amino
    End Enum
    Public Enum rolenames
        AccountHolder
        AccountMember
        SuperAdmin
        AdminUser
        SupportAdmin
        SupportUser
        Installer
    End Enum

    Public Enum Subscriptions
        AlarmMonitoring
        SafeAtHome
        Cameras
        ResponseCover
        HomeControl
        Social
        Utilities
        Telephony
        Network
        None
        HomeAutomation
    End Enum

    Public Enum RegistrationType
        Yale
        G2
        None
    End Enum

    Public Enum PanelCodes
        CLXDV4
        YALDV2
        DSCDV5
        ENVDV1
    End Enum

    Public Enum CameraCodes
        SERDV1
    End Enum

#Region "String constants"
    Public Const c_ConfigJavascriptIDs As String = "javascriptIDs"
    Public Const c_ConfigUseJSCDN As String = "useScriptsCDN"
    Public Const c_ConfigUseSSO As String = "UsePartnerSSO"
    Public Const c_CultureCookie As String = "spr_Culture"
    Public Const c_SessionAccountDetailTileSelected As String = "AccountDetailTileSelected"
    Public Const c_SessionAccountID As String = "AccountID"
    Public Const c_SessionAreaID As String = "AreaID"
    Public Const c_SessionCurrentUserName As String = "CurrentUserName"
    Public Const c_SessionDeviceID As String = "DeviceID"
    Public Const c_SessionDeviceSeq As String = "DeviceSeq"
    Public Const c_SessionCulture As String = "Culture"
    Public Const c_SessionInstallerID As String = "InstallerID"
    Public Const c_SessionLoggedInUser As String = "LoggedInUser"
    Public Const c_SessionMasterCoID As String = "MasterCoID"
    Public Const c_SessionNoteID As String = "NoteID"
    Public Const c_SessionPropertyID As String = "PropertyID"
    Public Const c_SessionPropZone As String = "PropZone"
    Public Const c_SessionSuppReqID As String = "SupportRequestID"
    Public Const c_SessionUserIDToEdit As String = "UserIDToEdit"
    Public Const c_SessionUsername As String = "Username"
    Public Const c_SessionUsersCompany As String = "UsersCompany"
    Public Const c_SessionTermsAcceptedDate As String = "TermsAcceptedDate"

    Public Const c_SIAAutoTest As String = "TS"
    Public Const c_SIAPanelBatteryCheck As String = "YS"
    Public Const c_SIAPanelBatteryOK As String = "YR"
    Public Const c_SIASensorBatteryCheck As String = "XT"
    Public Const c_SIASensorBatteryOK As String = "XR"
    Public Const c_SIASensorCheckFailure As String = "SP"
    Public Const c_SIASensorTamper As String = "TZ"
    Public Const c_SIASensorTamperRestoral As String = "TY"

    Public Const c_OpenValue As Integer = e_SupportRequestStatus.Open
    Public Const c_ClosedValue As Integer = e_SupportRequestStatus.Closed
    Public Const c_EscalatedToServiceProviderValue As Integer = e_SupportRequestStatus.EscalatedToServiceProvider
    Public Const c_EscalatedToDistributorValue As Integer = e_SupportRequestStatus.EscalatedToDistributor
    Public Const c_EscalatedToSprueValue As Integer = e_SupportRequestStatus.EscalatedToSprue

    Private Const COMMS_TYPE_CREATED_USER As Integer = 52
    Private Const EMAIL_MSG_TYPE As String = "E"
    Private Const DEFAULT_LANGUAGE As String = "EN"


#End Region

    Shared ReadOnly Property p_SessionCameraMAC() As String
        Get
            Return "CameraMAC"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the Registration Type Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionRegistrationType() As String
        Get
            Return "RegistrationType"
        End Get
    End Property

    ''' <summary>
    ''' Returns the key name for the UserID Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionUserID() As String
        Get
            Return "UserID"
        End Get
    End Property


    ''' <summary>
    ''' Returns the Key name for the AccountID Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionAccountID() As String
        Get
            Return "AccountID"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the 'LastAccess' session tracker
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionAdminLastAccess As String
        Get
            Return "AdminLastAccess"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name fro the 'FullAccess' session indicator
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionFullAccess As String
        Get
            Return "AdminFullAccess"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the PropertyID Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionPropertyID() As String
        Get
            Return "PropertyID"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the Property Count Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionPropertyCount() As String
        Get
            Return "PropertyCount"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the Current Device Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionDeviceSeq() As String
        Get
            Return "DeviceSeq"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the Current AlarmNo Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionAlarmNo() As String
        Get
            Return "AlarmNo"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the Current RecordedImageNo Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionRecordedImageNo() As String
        Get
            Return "RecordedImageNo"
        End Get
    End Property

    ''' <summary>
    ''' Returns the key name for the Theme session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionTheme() As String
        Get
            Return "Theme"
        End Get
    End Property


    Shared ReadOnly Property p_SessionDeviceID() As String
        Get
            Return "DeviceID"
        End Get
    End Property

    Shared ReadOnly Property p_SessionControlID() As String
        Get
            Return "ControlID"
        End Get
    End Property

    Shared ReadOnly Property p_SessionCommand() As String
        Get
            Return "Command"
        End Get
    End Property

    ''' <summary>
    ''' Returns the Key name for the Logo Image Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property p_SessionImageLogo() As String
        Get
            Return "ImageLogo"
        End Get
    End Property

    Shared ReadOnly Property p_SessionUserIDToEdit() As String
        Get
            Return "UserIDToEdit"
        End Get
    End Property

    Shared ReadOnly Property p_SessionCurrentUserName() As String
        Get
            Return "CurrentUserName"
        End Get
    End Property

    Shared ReadOnly Property p_SessionIntamacLogin() As String
        Get
            Return "IntamacLogin"
        End Get
    End Property

    Shared ReadOnly Property p_SessionRefusedCamFirmwares() As String
        Get
            Return "RefusedCamFirmwares"
        End Get
    End Property

    Shared ReadOnly Property p_SessionLoggedInMasterCompanyID() As String
        Get
            Return "MasterCompanyID"
        End Get
    End Property

    Shared ReadOnly Property p_SessionLoggedInMasterCompanyTypeID() As String
        Get
            Return "MasterCompanyTypeID"
        End Get
    End Property

    Shared ReadOnly Property p_SessionAminoParentMasterCoID() As String
        Get
            Return "AminoParentMasterCoID"
        End Get
    End Property

    Shared ReadOnly Property p_SessionLoggedInRoleName() As String
        Get
            Return "RoleName"
        End Get
    End Property

    Shared ReadOnly Property p_SessionSecurityAnswerSelection() As String
        Get
            Return "SecurityAnswerSelection"
        End Get
    End Property

    Shared ReadOnly Property p_SessionSupportReqID() As String
        Get
            Return "SessionSupportReqID"
        End Get
    End Property

    Shared ReadOnly Property p_SessionNoteID() As String
        Get
            Return "SessionNoteID"
        End Get
    End Property

    Shared ReadOnly Property p_SessionOpCoToEditID() As String
        Get
            Return "OpCoToEditID"
        End Get
    End Property

    Public Shared ReadOnly Property CompanyType As IntamacShared_SPR.SharedStuff.e_CompanyType
        Get
            Return CType([Enum].Parse(GetType(IntamacShared_SPR.SharedStuff.e_CompanyType), System.Configuration.ConfigurationManager.AppSettings("BusLogicCompanyType")), IntamacShared_SPR.SharedStuff.e_CompanyType)
        End Get
    End Property

    Public Enum AppSettings
        AppName
    End Enum

    ''' <summary>
    ''' Function that generates a full path to a video file with any extension removed
    ''' </summary>
    ''' <param name="strFilePath">The path to the file which may be in URL format, ie. rtsp://1.2.3.4/path/file.asf</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getFullVidPathNoExtension(ByVal strFilePath As String) As String
        ' Assume a certain directory is mapped to the appropriate location that stores our Flash videos
        Dim uncServerPart As String = System.Configuration.ConfigurationManager.AppSettings("UncServerPath")
        Dim basePathPart As String = System.Configuration.ConfigurationManager.AppSettings("UncBasePath")

        Dim fullUncFilePath As String
        Dim fullPathNoExt As String
        If (strFilePath.StartsWith("rtsp://")) Then
            ' We want to convert an RTSP link to a UNC file system location
            Dim fixedFilePath As String = "\\" & strFilePath.Remove(0, 7).Replace("/", "\")
            Dim separatedUNC As Match = Regex.Match(fixedFilePath, "(\\\\.*?)(\\.*)")
            Dim serverPart As String = separatedUNC.Groups(1).ToString
            Dim endPathPart As String = separatedUNC.Groups(2).ToString

            fullUncFilePath = uncServerPart & basePathPart & endPathPart
            ' Remove extension
            fullPathNoExt = Path.GetDirectoryName(fullUncFilePath) & "\" & Path.GetFileNameWithoutExtension(fullUncFilePath)
        Else
            fullPathNoExt = strFilePath
        End If

        Return fullPathNoExt
    End Function

    ''' <summary>
    ''' Function returns the directory of the current theme
    ''' </summary>
    ''' <returns>returns a string containing the directory of the current theme</returns>
    ''' <remarks></remarks>
    Public Shared Function getThemeDir() As String
        Dim Server As ICommonVars
        'Server = miscFunctions.getCurrentSite

        Dim strTheme As String
        strTheme = HttpContext.Current.Session(miscFunctions.p_SessionTheme)
        If strTheme = "" Then
            strTheme = "SPR"
        End If
        Dim ThemeDir As String = Server.p_Common_RootFolder & "/App_Themes/" & strTheme
        Return ThemeDir

    End Function

    Public Shared Function buildWelcomeEmailBodyText(ByVal UserID As System.Guid) As String
        'Dim txtStringBuilder As New Text.StringBuilder


        Dim strCurrentTheme As String = HttpContext.Current.Session(miscFunctions.p_SessionTheme)
        Dim gen As New General(ConfigurationManager.ConnectionStrings("conn").ConnectionString,
        ConfigurationManager.AppSettings("AppName"))
        'Dim currentServer As ICommonVars = miscFunctions.getCurrentSite
        If strCurrentTheme = "" Then
            strCurrentTheme = "SPR"
        End If
        Dim txtStringBuilder As New Text.StringBuilder
        If gen.getWelcomeEmail(strCurrentTheme) Then

            Dim drow As Data.DataRow = gen.p_ThisDS.Tables(0).Rows(0)
            txtStringBuilder.Append(drow("EmailLine1").ToString & vbNewLine)
            txtStringBuilder.Append(vbNewLine)
            txtStringBuilder.Append(drow("EmailLine2").ToString & vbNewLine)
            txtStringBuilder.Append(vbNewLine)
            txtStringBuilder.Append(drow("EmailLink").ToString & UserID.ToString & "&T=" & HttpContext.Current.Session(miscFunctions.p_SessionTheme) & vbNewLine)
            txtStringBuilder.Append(vbNewLine)
            txtStringBuilder.Append(drow("EmailLine3").ToString & vbNewLine)
            txtStringBuilder.Append(vbNewLine)
            txtStringBuilder.Append("Thanks.")
            txtStringBuilder.Append(vbNewLine)
            txtStringBuilder.Append("Intamac Customer Services Team")
            Return txtStringBuilder.ToString
        Else
            'error
            'HttpContext.Current.Response.Redirect(currentServer.p_Common_RootFolder & "/ErrorPage.aspx", True)

        End If
        Return txtStringBuilder.ToString



    End Function


    ''' <summary>
    ''' Function determines if the property is subscribed to a subscription
    ''' </summary>
    ''' <param name="subscription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsSubscribedTo(ByVal subscription As miscFunctions.Subscriptions) As Boolean
        Dim strServiceName As String
        Select Case subscription
            Case miscFunctions.Subscriptions.AlarmMonitoring
                strServiceName = "AlarmMonitoring"
            Case miscFunctions.Subscriptions.SafeAtHome
                strServiceName = "Safe@Home"
            Case miscFunctions.Subscriptions.Cameras
                strServiceName = "CameraMonitoring"
            Case miscFunctions.Subscriptions.ResponseCover
                strServiceName = "ResponseCover"
            Case Subscriptions.HomeAutomation
                strServiceName = "HomeAutomation"
            Case miscFunctions.Subscriptions.None
                strServiceName = ""
            Case Else
                strServiceName = ""
        End Select

        Dim s As New IntamacBusinessLogic.Subscriptions(ConfigurationManager.ConnectionStrings("conn").ConnectionString,
        ConfigurationManager.AppSettings("AppName"))

        'Dim currentServer As ICommonVars = miscFunctions.getCurrentSite
        s.p_AccountID = HttpContext.Current.Session(miscFunctions.p_SessionAccountID)
        s.p_PropertyID = HttpContext.Current.Session(miscFunctions.p_SessionPropertyID)

        Dim boolIsSubscribed As Boolean = False

        Dim objNumberOfSubscriptions As Object
        If s.isSubscribedTo(strServiceName, objNumberOfSubscriptions) Then
            If CType(objNumberOfSubscriptions, Integer) > 0 Then
                boolIsSubscribed = True
            Else
                boolIsSubscribed = False
            End If
        Else
            boolIsSubscribed = False

        End If
        Return boolIsSubscribed
    End Function

    'Adds the history.back method to the Onclick event
    Public Shared Sub setBackButton(ByRef img As Image)
        img.Attributes.Add("onclick", "history.back()")
    End Sub

    ''' <summary>
    ''' Function returns the MembershipUSer object that represents the AccountHolder
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getAccountHolder() As MembershipUser
        Dim memUser As MembershipUser

        Dim m As New IntamacBusinessLogic.Member(ConfigurationManager.ConnectionStrings("conn").ConnectionString, ConfigurationManager.AppSettings("AppName"))
        m.p_AccountID = HttpContext.Current.Session(miscFunctions.p_SessionAccountID)

        If m.getMembers() Then
            For Each drow As Data.DataRow In m.p_ThisDS.Tables(0).Rows
                If Roles.IsUserInRole(Trim(drow("IntaMemberID").ToString), miscFunctions.rolenames.AccountHolder.ToString) Then
                    memUser = Membership.GetUser(Trim(drow("IntaMemberID").ToString))
                    Exit For
                End If
            Next

        End If

        Return memUser
    End Function

    ''' <summary>
    ''' Function returns the MembershipUSer object that represents the AccountHolder
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getAccountHolder(ByVal strAccountID As String) As MembershipUser
        Dim memUser As MembershipUser

        Dim m As New IntamacBusinessLogic.Member(ConfigurationManager.ConnectionStrings("conn").ConnectionString, ConfigurationManager.AppSettings("AppName"))
        m.p_AccountID = strAccountID

        If m.getMembers() Then
            For Each drow As Data.DataRow In m.p_ThisDS.Tables(0).Rows
                If Roles.IsUserInRole(Trim(drow("IntaMemberID").ToString), miscFunctions.rolenames.AccountHolder.ToString) Then
                    memUser = Membership.GetUser(Trim(drow("IntaMemberID").ToString))
                    Exit For
                End If
            Next

        End If

        Return memUser
    End Function

    ''' <summary>
    ''' Function puts the Passed string into the correct format for a mac address
    ''' </summary>
    ''' <param name="strToConvert">String to convert</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ConvertMacAddress(ByVal strToConvert As String) As Text.StringBuilder

        'Put the Mac address in the Correct Format (ie semicolon after every second char)

        strToConvert = strToConvert.Replace(":", "")
        Dim strMacBuilder As New Text.StringBuilder

        For i As Integer = 0 To strToConvert.Length - 1

            If i Mod 2 = 0 And i <> 0 Then
                strMacBuilder.Append(":")  'Add a colon after every second char
                strMacBuilder.Append(strToConvert.Chars(i))
            Else
                strMacBuilder.Append(strToConvert.Chars(i))

            End If
        Next

        Return strMacBuilder

    End Function


    Shared Function IsTelephoneNoValid(ByVal strFullTelNo As String, Optional ByVal boolValidateEmptyNo As Boolean = False) As Boolean

        If boolValidateEmptyNo = True And String.IsNullOrEmpty(strFullTelNo) Then 'Optional validation of empty field
            Return True
            Exit Function
        End If

        If Not IsNumeric(strFullTelNo) Then 'The Phone number must be numeric
            Return False
            Exit Function
        End If
        If strFullTelNo.Length < 8 Then 'The Phone number must be at least 8 chars long
            Return False
            Exit Function
        End If
        Return True

    End Function


    ''' <summary>
    ''' Sub sets all RC8020's Ports to Null.  This should be called on Website logout and Session End.
    ''' </summary>
    ''' <remarks></remarks>
    Shared Sub NullPorts(ByVal strAccountID As String)
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ToString)
        Try
            conn.Open()
            DBAction.ExecNonQuery(conn, "usp_UPDATE_PropDevicePortsToNull", strAccountID)


        Catch ex As Exception
            Dim log As New Logging
            'log.LogErrorToSystemEventLog(Diagnostics.EventLogEntryType.Error, ex, "Error in NullPorts", System.Configuration.ConfigurationManager.AppSettings("AppName"))
        Finally
            If conn.State = Data.ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Shared Function DoNotShowPropDropDown(ByVal strPageName As String, ByVal strPageDirectory As String) As Boolean

        Dim boolDoNotShowDropdown As Boolean

        Select Case strPageDirectory.ToLower

            'do not show any of the devices pages (except those shown in next case statement)
            Case "devices"
                boolDoNotShowDropdown = True

            Case Else

                boolDoNotShowDropdown = False

        End Select

        Select Case strPageName.ToLower

            Case "contactdetails.aspx", "memberdetail.aspx", "changepassword.aspx",
            "memberpermissions.aspx", "addcontact.aspx"
                boolDoNotShowDropdown = True

                'show for base pages of alarm and camera
            Case "devicesalarmnew.aspx", "cameras.aspx"

                boolDoNotShowDropdown = False

        End Select

        Return boolDoNotShowDropdown

    End Function

    Shared Function GetCurrentPageName() As String

        Dim strPath As String = System.Web.HttpContext.Current.Request.Url.AbsolutePath
        Dim objInfo As System.IO.FileInfo = New System.IO.FileInfo(strPath)
        Return objInfo.Name

    End Function

    Shared Function GetCurrentPagePath() As String

        Dim strPath As String = System.Web.HttpContext.Current.Request.Url.AbsolutePath
        Dim objInfo As System.IO.FileInfo = New System.IO.FileInfo(strPath)
        Return objInfo.Directory.Name

    End Function

    Public Shared Function AddAuditRecord(ByVal v_strAccountID As String, ByVal v_strPropertyID As String, ByVal v_strUsername As String, ByVal v_eAuditActionType As IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID, ByVal v_strNotes As String) As Boolean
        Dim objAudit As IntamacBL_SPR.Audit
        objAudit = IntamacBL_SPR.ObjectManager.CreateAudit(CompanyType)

        objAudit.AccountID = v_strAccountID
        objAudit.PropertyID = v_strPropertyID
        objAudit.Notes = v_strNotes
        objAudit.Username = v_strUsername
        objAudit.DateEntered = Date.UtcNow
        objAudit.AuditType = v_eAuditActionType

        objAudit.Save()

        Return True

    End Function

    Public Shared Function GetSupportStatuses() As DataTable

        Dim objLookup As IntamacBL_SPR.Lookups = IntamacBL_SPR.ObjectManager.CreateLookup(CompanyType)
        Dim dtb As Data.DataTable = objLookup.GetSupportStatuses(0)

        For Each row As DataRow In dtb.Rows

            'update the status descriptions with any values in PageGlobalResources
            If row("StatusID") = 0 And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "PleaseSelect")) Then
                row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "PleaseSelect").ToString()
            ElseIf row("StatusID") = c_OpenValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen")) Then
                row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusOpen").ToString()
            ElseIf row("StatusID") = c_ClosedValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusClosed")) Then
                row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusClosed").ToString()
            ElseIf row("StatusID") = c_EscalatedToServiceProviderValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToServiceProvider")) Then
                row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToServiceProvider").ToString()
            ElseIf row("StatusID") = c_EscalatedToDistributorValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToDistibutor")) Then
                row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToDistibutor").ToString()
            ElseIf row("StatusID") = c_EscalatedToSprueValue And Not String.IsNullOrEmpty(HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToSprue")) Then
                row("StatusDescription") = HttpContext.GetGlobalResourceObject("PageGlobalResources", "SupportStatusEscalatedToSprue").ToString()
            End If
        Next

        Return dtb

    End Function

    Public Shared Function SendSupportRequestEmail(ByVal v_strAccountID As String, ByVal v_strPropertyID As String, ByVal v_strContent As String, ByVal v_strToAddress As String, ByVal v_strSubject As String) As Boolean

        Dim boolSuccess As Boolean = False
        Dim comms As New IntamacBL_SPR.Communications

        Dim strFromAddr As String = CStr(System.Configuration.ConfigurationManager.AppSettings("AdminEmailFrom"))

        Try
            comms.AccType = CompanyType.ToString
            comms.CommsType = "60"
            comms.CEmail = v_strToAddress
            comms.MessageSubject = v_strSubject
            comms.AccountID = v_strAccountID
            comms.PropertyID = v_strPropertyID
            comms.EmailMessage = v_strContent
            comms.Contact = Date.Now.Ticks.ToString()

            comms.Save()

            boolSuccess = True

        Catch ex As Exception

            boolSuccess = False
            IntamacShared_SPR.SharedStuff.LogError(ex, System.Configuration.ConfigurationManager.AppSettings("AppName"), "Error in SendSupportRequestEmail")

        End Try

        Return boolSuccess

    End Function
    Public Shared Function SendUserCreatedEmail(objMasterUser As MasterUser, ByVal v_strLanguage As String) As Boolean
        Dim boolSuccess As Boolean = False
        Dim comms As New IntamacBL_SPR.Communications

        Try
            Dim envox As New EnvoxTextDB()
            Dim envoxTable = envox.Load(CompanyType.ToString(), COMMS_TYPE_CREATED_USER, EMAIL_MSG_TYPE, v_strLanguage)

            comms.AccType = CompanyType.ToString
            comms.CommsType = COMMS_TYPE_CREATED_USER.ToString()
            comms.CEmail = objMasterUser.Email
            comms.MessageSubject = CType(envoxTable(0)("IntaMessageSubject"), String)
            comms.AccountID = "Admin System"
            comms.PropertyID = "UserCreate"
            comms.EmailMessage = GenerateEmailBodyForCreatedUser(objMasterUser.Username, objMasterUser.Firstname, envoxTable)
            comms.CommsDateTime = DateTime.UtcNow

            If Not String.IsNullOrEmpty(comms.EmailMessage) Then
                boolSuccess = comms.Save()
            Else
                boolSuccess = False
            End If


        Catch ex As Exception

            boolSuccess = False
            Dim log As New Logging
            log.LogErrorToSystemEventLog(System.Diagnostics.EventLogEntryType.Error, ex, "Error in sendUserCreatedEmail", System.Configuration.ConfigurationManager.AppSettings("AppName"))

        End Try

        Return boolSuccess

    End Function

    ''' <summary>
    ''' Generate the email body for a newly created user.
    ''' This occurs once the user has responded to the initial email and
    ''' filled in their details.
    ''' </summary>
    ''' <param name="userName"></param>
    ''' <returns></returns>
    Private Shared Function GenerateEmailBodyForCreatedUser(userName As String, firstName As String, envoxTable As DataTable) As String

        If envoxTable Is Nothing OrElse envoxTable.Rows.Count = 0 Then
            Return Nothing
        End If

        Dim msgBody As String = CType(envoxTable(0)("IntaMessageBody"), String)
        If Not String.IsNullOrEmpty(msgBody) Then
            msgBody = msgBody.Replace("{USERNAME}", userName)
            msgBody = msgBody.Replace("{FIRSTNAME}", firstName)
        End If

        Return msgBody
    End Function

    Public Function GetSystemStatus(ByVal objPanel As ControlG2Panel.ControlG2Panel, ByVal strAccountID As String, ByVal strPropertyID As String) As String
        Dim objG2Control As New IntamacBusinessLogic.G2Control(ConfigurationManager.ConnectionStrings("conn").ConnectionString,
        ConfigurationManager.AppSettings("AppName"))

        '	Dim objPanel As ControlG2Panel.ControlG2Panel = CreateG2PanelObject()
        Dim strStatus As String
        'check how to control panel
        If objPanel.p_ControlbyPortForward Then
            ' if we are controlling via portforward call that function
            strStatus = objG2Control.IsControlledByPortForwarding()

        Else

            If objG2Control.GetStatus(strAccountID, strPropertyID) Then
                If objG2Control.p_ThisDS.Tables(0).Rows.Count > 0 Then
                    Dim dbrow As DataRow = objG2Control.p_ThisDS.Tables(0).Rows(0)
                    strStatus = dbrow("IntaPanelStatus").ToString.Trim
                Else
                    strStatus = ""
                End If
            End If

        End If

        Return strStatus
    End Function

    Public Overloads Function CreateG2PanelObject(ByVal strAccountID As String, ByVal strPropertyID As String) As ControlG2Panel.ControlG2Panel

        Dim memuser As MembershipUser = miscFunctions.getAccountHolder
        If Not IsNothing(memuser) Then
            Dim objPanel As New ControlG2Panel.ControlG2Panel(strAccountID, strPropertyID, LCase(memuser.UserName), Membership.Provider.GetPassword(memuser.UserName, String.Empty), ConfigurationManager.ConnectionStrings("Conn").ToString, ConfigurationManager.AppSettings("AppName"))
            Return objPanel
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function GenRandString(ByVal charsToUse As String, ByVal length As Integer) As String
        ' Generates and returns a string of 'length' characters that contains a random collection of characters
        ' contained in the string 'charsToUse'.

        Dim count As Integer
        Dim randGen As New Random() ' By default, the empty Random class constructor takes its seed from the system clock.
        Dim outString As New StringBuilder

        If (length < 1) Then
            Return ""
        End If

        For count = 1 To length
            outString.Append(charsToUse.Substring(randGen.Next(1, charsToUse.Length + 1) - 1, 1))
        Next

        Return outString.ToString
    End Function

    Public Shared Function GetEffectiveDateActions(ByVal TableName As String, ByVal ColumnName As String, ByVal AccountID As String, ByVal PropertyID As String) As DataRow
        Dim dsResult As New DataSet
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ToString)
        Dim params() As SqlParameter = New SqlParameter(3) {}

        params(0) = New SqlParameter("@TableName", TableName)
        params(1) = New SqlParameter("@ColumnName", ColumnName)
        params(2) = New SqlParameter("@AccountID", AccountID)
        params(3) = New SqlParameter("@PropertyID", PropertyID)

        Try
            conn.Open()
            DBAction.ExecDS(conn, "usp_SELECT_EffectiveDateActions", dsResult, params)
        Catch ex As Exception
            Dim log As New Logging
            log.LogErrorToSystemEventLog(System.Diagnostics.EventLogEntryType.Error, ex, "Error Getting Effective Date Actions", System.Configuration.ConfigurationManager.AppSettings("AppName"))
        Finally
            If conn.State = Data.ConnectionState.Open Then
                conn.Close()
            End If
        End Try


        Dim result As DataRow = Nothing

        If dsResult.Tables.Count > 0 Then
            If dsResult.Tables(0).Rows.Count > 0 Then
                result = dsResult.Tables(0).Rows(0)
            End If
        End If

        Return result
    End Function

    Public Shared Function SetEffectiveDateActions(ByVal TableName As String, ByVal ColumnName As String, ByVal AccountID As String, ByVal PropertyID As String, ByVal ValueChanged As String, ByVal EffectiveDate As Nullable(Of DateTime)) As Boolean
        Dim result As New Boolean
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ToString)
        Dim params() As SqlParameter = New SqlParameter(5) {}

        params(0) = New SqlParameter("@TableName", TableName)
        params(1) = New SqlParameter("@ColumnName", ColumnName)
        params(2) = New SqlParameter("@AccountID", AccountID)
        params(3) = New SqlParameter("@PropertyID", PropertyID)
        params(4) = New SqlParameter("@ValueChanged", ValueChanged)
        params(5) = New SqlParameter("@EffectiveDate", EffectiveDate)

        Try
            conn.Open()
            result = DBAction.ExecNonQuery(conn, "usp_UPDATE_EffectiveDateActions", params)
        Catch ex As Exception
            Dim log As New Logging
            log.LogErrorToSystemEventLog(System.Diagnostics.EventLogEntryType.Error, ex, "Error Setting Effective Date Actions", System.Configuration.ConfigurationManager.AppSettings("AppName"))
        Finally
            If conn.State = Data.ConnectionState.Open Then
                conn.Close()
            End If
        End Try

        Return result
    End Function

    Public Shared Function DeleteEffectiveDateAction(ByVal TableName As String, ByVal ColumnName As String, ByVal AccountID As String, ByVal PropertyID As String) As Boolean
        Dim result As New Boolean
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ToString)
        Dim params() As SqlParameter = New SqlParameter(3) {}

        params(0) = New SqlParameter("@TableName", TableName)
        params(1) = New SqlParameter("@ColumnName", ColumnName)
        params(2) = New SqlParameter("@AccountID", AccountID)
        params(3) = New SqlParameter("@PropertyID", PropertyID)

        Try
            conn.Open()
            result = DBAction.ExecNonQuery(conn, "usp_Delete_EffectiveDateActions", params)
        Catch ex As Exception
            Dim log As New Logging
            log.LogErrorToSystemEventLog(System.Diagnostics.EventLogEntryType.Error, ex, "Error deleting Effective Date Actions", System.Configuration.ConfigurationManager.AppSettings("AppName"))
        Finally
            If conn.State = Data.ConnectionState.Open Then
                conn.Close()
            End If
        End Try

        Return result

    End Function


    Public Shared Function ReturnOrdinal(number As Integer, length As Integer) As String

        Dim strReturn As String

        If number = 1 Then
            strReturn = "First"
            'ElseIf number = length - 1 Then
            '	strReturn = "Penultimate"
        ElseIf number = length Then
            strReturn = "Last"
        ElseIf number = 11 Or number = 12 Or number = 13 Then
            strReturn = number.ToString + "th"
        Else
            Select Case number.ToString.Substring(number.ToString.Length - 1)
                Case 1
                    strReturn = number.ToString & "st"
                Case 2
                    strReturn = number.ToString & "nd"
                Case 3
                    strReturn = number.ToString & "rd"
                Case Else
                    strReturn = number.ToString & "th"
            End Select

        End If

        Return strReturn

    End Function

    Public Shared Sub NewWindow(ByVal Page As System.Web.UI.Page, ByVal strPage As String, ByVal Height As Integer,
                                ByVal Width As Integer, ByVal Status As Boolean, ByVal ToolBar As Boolean, ByVal MenuBar As Boolean,
                                ByVal Location As Boolean, ByVal ScrollBar As Boolean, ByVal Resizable As Boolean,
                                ByVal ScrWidth As Integer, ByVal ScrHeight As Integer)

        If String.IsNullOrEmpty(strPage) Then
            Throw (New ApplicationException("Page name cannot be null or empty"))

        End If
        Dim intSubLeft As Integer = 0
        Dim intSubTop As Integer = 0
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0

        Dim sbWindowOptions As New StringBuilder()
        Dim sbFunctionParamters As New StringBuilder()
        Dim sbCompleteScript As New StringBuilder()

        intSubLeft = CInt(IIf(ScrWidth <= 0, 1024, ScrWidth))
        intSubTop = CInt(IIf(ScrHeight <= 0, 768, ScrHeight))
        intTop = CInt(IIf(intSubTop < Height, 0, (intSubTop - Height) / 2))
        intLeft = CInt(IIf(intSubTop < Width, 0, (intSubTop - Width) / 2))

        Dim optionsArray() As Object = New Object() {Height, Width, YesNo(Status), YesNo(ToolBar), YesNo(MenuBar), YesNo(Location), YesNo(ScrollBar),
                                                     YesNo(Resizable), intTop.ToString(), intLeft.ToString(), intTop.ToString(), intLeft.ToString()}

        sbWindowOptions.AppendFormat("height={0},width={1},status={2},toolbar={3},menubar={4},location={5},scrollbars={6},resizable={7},top={8},left={9},screenY={10},screenX={11}", optionsArray)
        sbFunctionParamters.AppendFormat("'{0}',null,'{1}'", strPage, sbWindowOptions.ToString())
        sbCompleteScript.AppendFormat("<SCRIPT TYPE=""text/javascript\"">window.open({0});</SCRIPT>", sbFunctionParamters.ToString())

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUpWindowScript", sbCompleteScript.ToString(), False)

    End Sub

    Private Shared Function YesNo(ByVal testBool As Boolean) As String
        Return CStr(IIf(testBool, "yes", "no"))

    End Function

    Public Shared Function GetSensorType(ByVal intSensorType As Nullable(Of Integer)) As String

        If intSensorType Is Nothing Then
            ' sensor type may not have been set yet after the annouce, so return as unknown
            Return IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes.Unknown.ToString()
        Else
            Return CType(intSensorType, IntamacShared_SPR.SharedStuff.e_AlarmSensorTypes).ToString
        End If
    End Function

    ''' <summary>
    ''' Indicates if the logged in user's company is either Inatamc or Sprue
    ''' </summary>
    ''' <returns>True if the company is either Intamac or Sprue</returns>
    ''' <remarks></remarks>
    Public Shared Function IsPrivilegedCoUser() As Boolean
        Dim blnIsPriveleged As Boolean = False

        ' only proceed if calling page is subclass of CulturBaseClass
        If TypeOf HttpContext.Current.Handler Is CultureBaseClass Then

            Dim usersCo As IntamacBL_SPR.MasterCompany = DirectCast(HttpContext.Current.Handler, CultureBaseClass).mUsersCompany

            If usersCo IsNot Nothing Then
                If (New List(Of Integer) From {CInt(e_MasterCompanyTypes.SystemOwner), CInt(e_MasterCompanyTypes.ApplicationOwner)}).Contains(usersCo.CompanyTypeID) Then
                    blnIsPriveleged = True
                End If

            End If
        End If

        Return blnIsPriveleged
    End Function

#Region "Firmware Version Customer Facing Name"

    Public Shared Function GetFirmwareName(ByVal strDeviceID As String, ByVal strFirmwareVersion As String, Optional ByVal TypeofUpgrade As String = Nothing, Optional ByVal All As Boolean = False) As String

        Dim strFirmwareName As String = strFirmwareVersion
        Dim firmUpgradeList As New List(Of IntamacBL_SPR.FirmwareUpgrade)
        Dim firmwareUpdate As New IntamacBL_SPR.FirmwareUpgrade

        firmUpgradeList = firmwareUpdate.Load(strDeviceID, CompanyType.ToString, TypeofUpgrade, All)

        If firmUpgradeList IsNot Nothing Then

            If firmUpgradeList.Count > 0 Then

                For Each firm As IntamacBL_SPR.FirmwareUpgrade In firmUpgradeList

                    If strFirmwareVersion.ToLower = firm.FirmwareVersion.ToLower Then

                        strFirmwareName = firm.FirmwareDesc
                        Exit For

                    End If

                Next

            End If

        End If

        Return strFirmwareName

    End Function

    ''' <summary>
    ''' Added PanelSoftReset funtion send RESET command
    ''' </summary>
    ''' <param name="v_strAccountID"></param>
    ''' <param name="v_strPropertyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function PanelSoftReset(ByVal v_strAccountID As String, ByVal v_strPropertyID As String) As Boolean
        Dim p As IntamacBL_SPR.AlarmPanel = Nothing
        Dim bRet As Boolean = False

        Try

            p = IntamacBL_SPR.ObjectManager.CreateAlarmPanel(v_strAccountID, v_strPropertyID, CompanyType)
            p.CompanyType = CompanyType
            p.Load(v_strAccountID, v_strPropertyID, 0)

            'send factory reset command regardless of whether or not the panel is polling in
            bRet = p.SoftReset(DirectCast(HttpContext.Current.Handler, CultureBaseClass).mLoggedInUser.Username, "")

        Catch ex As Exception
            If ex.Message IsNot Nothing AndAlso ex.Message.ToLower().Contains("offline") Then
                bRet = False
            End If
        Finally
            p = Nothing
        End Try

        Return bRet
    End Function

#End Region
End Class
