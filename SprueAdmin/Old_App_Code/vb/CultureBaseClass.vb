Imports Microsoft.VisualBasic
Imports System.Globalization
Imports System.Threading
Imports System.Web.Security
Imports System.Web.UI

Imports Telerik.Web.UI

Imports IntamacShared_SPR.SharedStuff

Public Class CultureBaseClass
    Inherits Page

#Region "Application Attributes"

    Protected ReadOnly Property mCompanyType As IntamacShared_SPR.SharedStuff.e_CompanyType
        Get
            Return miscFunctions.CompanyType
        End Get
    End Property

    Protected ReadOnly Property mBtnBack As Button
        Get
            Return DirectCast(DeepFindControl(Master, "btnBack"), Button)
        End Get
    End Property

    Protected ReadOnly Property mTitleBar As HtmlGenericControl
        Get
            Return DirectCast(DeepFindControl(Master, "titleBar"), HtmlGenericControl)
        End Get
    End Property

#End Region

#Region "Page Variables"
    Dim _responseComplete As Boolean = False

    Protected ReadOnly Property Body As HtmlGenericControl
        Get
            Return DirectCast(FindMasterControl(Master, "bodyElement"), HtmlGenericControl)

        End Get
    End Property

    Protected Friend Property ResponseComplete As Boolean
        Get
            Return _responseComplete

        End Get
        Set(value As Boolean)
            _responseComplete = value

        End Set
    End Property

    Protected ReadOnly Property SiteHeader As HtmlGenericControl
        Get
            Return DirectCast(FindMasterControl(Master, "divSiteHeader"), HtmlGenericControl)

        End Get
    End Property

    Public Property BodyClass As String
        Get
            Return Body.Attributes("class")
        End Get
        Set(value As String)
            Body.Attributes.Add("class", value)

        End Set
    End Property

    Protected Property mBackLocation As String
        Get
            Return CStr(ViewState("mBackLocation"))
        End Get
        Set(value As String)
            ViewState("mBackLocation") = value

        End Set
    End Property

    Protected Property mConfirmPrompt As String
        Get
            Dim retString As String = CStr(ViewState("mConfirmPrompt"))

            If String.IsNullOrEmpty(retString) Then
                Try
                    retString = CStr(GetLocalResourceObject("UpdateConfirmPrompt"))
                Catch
                End Try
            End If

            If String.IsNullOrEmpty(retString) Then
                Try
                    retString = PageString("UpdateConfirmPrompt")
                Catch
                End Try
            End If

            Return retString
        End Get
        Set(value As String)
            ViewState("mConfirmPrompt") = value

        End Set
    End Property



    Protected Property DefaultButtonID As String
        Get
            If IsNothing(ViewState("DefaultButtonID")) Then
                ViewState("DefaultButtonID") = ""

                Try
                    ViewState("DefaultButtonID") = GetLocalResourceObject("DefaultButton")
                Catch
                End Try
            End If

            Return CStr(ViewState("DefaultButtonID"))
        End Get
        Set(value As String)
            ViewState("DefaultButtonID") = value
        End Set
    End Property

    Protected Property DefaultFocusID As String
        Get
            If IsNothing(ViewState("DefaultFocusID")) Then
                ViewState("DefaultFocusID") = ""
                Try
                    ViewState("DefaultFocusID") = GetLocalResourceObject("DefaultFocusControl")
                Catch
                End Try
            End If
            Return CStr(ViewState("DefaultFocusID"))
        End Get
        Set(value As String)
            ViewState("DefaultFocusID") = value
        End Set
    End Property

    Public ReadOnly Property mCurrentUsername As String
        Get
            Return User.Identity.Name
        End Get
    End Property

    Public Property enablePageTimer As Boolean
        Get
            If Not IsNothing(ViewState("enablePageTimer")) Then
                Return CBool(ViewState("enablePageTimer"))
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value <> enablePageTimer Then
                ViewState("enablePageTimer") = value

            End If
        End Set
    End Property

    Public Property pageTimerMilliseconds As Integer
        Get
            If Not IsNothing(ViewState("pageTimerMilliseconds")) Then
                Return DirectCast(ViewState("pageTimerMilliseconds"), Integer)
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If value <> pageTimerMilliseconds Then
                ViewState("pageTimerMilliseconds") = value
            End If
        End Set
    End Property
    Dim _blnIsAccountPage As Nullable(Of Boolean) = Nothing

    Protected Property mIsAccountPage As Boolean
        Get
            If Not _blnIsAccountPage.HasValue Then

                Dim blnIsAccpage As Boolean = False

                Try
                    blnIsAccpage = (CStr(GetLocalResourceObject("IsAccountPage")).ToLower = "yes")
                Catch
                End Try
                _blnIsAccountPage = blnIsAccpage
            End If
            Return _blnIsAccountPage
        End Get
        Set(value As Boolean)
            _blnIsAccountPage = value
        End Set
    End Property

    Dim _blnIsTop As Nullable(Of Boolean) = Nothing

    Protected Property mIsTopPage As Boolean
        Get
            If Not _blnIsTop.HasValue Then

                Dim blnIsTop As Boolean = False

                Try
                    blnIsTop = (CStr(GetLocalResourceObject("IsTopMenu")).ToLower = "yes")
                Catch
                End Try
                _blnIsTop = blnIsTop
            End If
            Return _blnIsTop.Value
        End Get
        Set(value As Boolean)
            _blnIsTop = value
        End Set
    End Property

    Protected ReadOnly Property ShowTitleBar As Boolean
        Get
            Dim blnShowTitle As Boolean = True

            Try
                blnShowTitle = (CStr(GetLocalResourceObject("ShowTitleBar")).ToLower <> "no")

            Catch

            End Try

            Return blnShowTitle
        End Get

    End Property

    Public Property IsTopNavigate As Boolean
        Get
            If Not IsNothing(ViewState("IsTopNavigate")) Then
                Return CBool(ViewState("IsTopNavigate"))
            Else
                Return False
            End If

        End Get
        Set(value As Boolean)
            If value <> IsTopNavigate Then
                ViewState("IsTopNavigate") = value

            End If
        End Set
    End Property

    Public ReadOnly Property mSitetitle As String
        Get
            Return PageString("AdministrationSystemTitle")
        End Get
    End Property

    Protected Property mSortDir As String
        Get
            If Not IsNothing(ViewState("mSortDir")) Then
                Return CStr(ViewState("mSortDir"))
            Else
                Return "ASC"
            End If
            Return CStr(ViewState("mSortDir"))
        End Get
        Set(value As String)
            ViewState("mSortDir") = value
        End Set
    End Property

    Protected Property mSortExpr As String
        Get
            Return CStr(ViewState("mSortExpr"))
        End Get
        Set(value As String)
            ViewState("mSortExpr") = value
        End Set
    End Property

    Protected Property mTargetUsername As String
        Get
            Return CStr(ViewState("mTargetUsername"))
        End Get
        Set(value As String)
            ViewState("mTargetUsername") = value
        End Set
    End Property

    Dim _pTopMenu As String = Nothing

    Public Property pTopMenu As String
        Get
            If String.IsNullOrEmpty(_pTopMenu) Then
                Try
                    _pTopMenu = CStr(GetLocalResourceObject("TopMenu"))
                Catch
                End Try

            End If
            Return _pTopMenu

        End Get
        Protected Set(value As String)
            _pTopMenu = value
        End Set
    End Property

#End Region

#Region "Request Variables"

    Private _CurrentMasterUser As IntamacBL_SPR.MasterUser = Nothing

    Protected ReadOnly Property mCurrentMasterUser As IntamacBL_SPR.MasterUser
        Get
            If IsNothing(_CurrentMasterUser) AndAlso Not String.IsNullOrEmpty(mCurrentUsername) Then
                _CurrentMasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(mCompanyType)

                Dim searchReturn As List(Of IntamacBL_SPR.MasterUser) = _CurrentMasterUser.LoadByUserName(mCurrentUsername)

                If searchReturn.Count > 0 Then
                    _CurrentMasterUser = searchReturn(0)
                Else
                    Response.Redirect("~/AdminLogin.aspx")
                End If
            End If

            Return _CurrentMasterUser

        End Get
    End Property

    Private _requestItems As IDictionary = Nothing

    Protected ReadOnly Property mRequestItems As IDictionary
        Get
            If Not IsNothing(HttpContext.Current) Then
                Return HttpContext.Current.Items
            Else
                If IsNothing(_requestItems) Then
                    _requestItems = New Dictionary(Of String, Object)
                End If

                Return _requestItems
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property xhpTimerPanel As RadXmlHttpPanel
        Get
            Return DirectCast(DeepFindControl(Me, "xhpTimerPanel"), RadXmlHttpPanel)
        End Get
    End Property

    Private _mPageTitle As String = ""

    Protected Property mPageTitle As String
        Get
            Return _mPageTitle
        End Get
        Set(value As String)
            _mPageTitle = value

        End Set
    End Property
#End Region

#Region "Session Variables"

    Public Overridable Property mAccountID As String
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionAccountID)) Then
                Return CStr(Session(miscFunctions.c_SessionAccountID))
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            If Not IsNothing(value) Then
                Session(miscFunctions.c_SessionAccountID) = value
            Else
                Session.Remove(miscFunctions.c_SessionAccountID)
            End If
        End Set
    End Property

    Public Overridable Property mAreaID As Nullable(Of Integer)
        Get
            If IsNothing(Session(miscFunctions.c_SessionAreaID)) Then
                Session(miscFunctions.c_SessionAreaID) = New Nullable(Of Integer)
            End If
            Return DirectCast(Session(miscFunctions.c_SessionAreaID), Nullable(Of Integer))
        End Get
        Set(value As Nullable(Of Integer))
            Session(miscFunctions.c_SessionAreaID) = value
        End Set
    End Property

    Public Property mMasterCoID As Integer
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionMasterCoID)) Then
                Return CInt(Session(miscFunctions.c_SessionMasterCoID))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If value > 0 Then
                Session(miscFunctions.c_SessionMasterCoID) = value
            Else
                Session.Remove(miscFunctions.c_SessionMasterCoID)
            End If
        End Set
    End Property

    Public Property mInstallerID As Integer
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionInstallerID)) Then
                Return CInt(Session(miscFunctions.c_SessionInstallerID))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If value > 0 Then
                Session(miscFunctions.c_SessionInstallerID) = value
            Else
                Session.Remove(miscFunctions.c_SessionInstallerID)
            End If
        End Set
    End Property


    Protected Property blnCultureChanged As Boolean = False

    Public Property mCulture As String
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionCulture)) Then
                Return CStr(Session(miscFunctions.c_SessionCulture))
            Else
                Return "en-GB"
            End If
        End Get
        Set(value As String)

            ' if valid culture, save and apply to page 
            If Not blnCultureChanged Then
                blnCultureChanged = value <> mCulture

            End If

            Try
                ' create objects and check culture is valid before making changes
                Dim specificCulture = CultureInfo.CreateSpecificCulture(value)
                Dim cultureInfoObj = New CultureInfo(value)

                ' Save on thread if valid culture
                Thread.CurrentThread.CurrentCulture = specificCulture
                Thread.CurrentThread.CurrentUICulture = cultureInfoObj

                Session(miscFunctions.c_SessionCulture) = value

                Response.Cookies(miscFunctions.c_CultureCookie).Value = mCulture
                Response.Cookies(miscFunctions.c_CultureCookie).Expires = Date.UtcNow.AddYears(1)
                Response.Cookies(miscFunctions.c_CultureCookie).Path = "/"
                UICulture = mCulture
                Culture = mCulture

            Catch ex As Exception
                ' Don't allow invalid culture to be set on UI and be saved on cookie
            End Try

        End Set
    End Property

    Public Property mNoteID As Integer
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionNoteID)) Then
                Return CInt(Session(miscFunctions.c_SessionNoteID))
            Else
                Return 0
            End If
        End Get

        Set(value As Integer)
            If value > 0 Then
                Session(miscFunctions.c_SessionNoteID) = value
            Else
                Session.Remove(miscFunctions.c_SessionNoteID)
            End If
        End Set
    End Property

    Public Overridable Property mPropertyID As String
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionPropertyID)) Then
                Return CStr(Session(miscFunctions.c_SessionPropertyID))
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            If Not IsNothing(value) Then
                Session(miscFunctions.c_SessionPropertyID) = value
            Else
                Session.Remove(miscFunctions.c_SessionPropertyID)
            End If
        End Set
    End Property

    Public Property mSuppReqID As Integer
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionSuppReqID)) Then
                Return CInt(Session(miscFunctions.c_SessionSuppReqID))
            Else
                Return 0
            End If
        End Get

        Set(value As Integer)
            If value > 0 Then
                Session(miscFunctions.c_SessionSuppReqID) = value
            Else
                Session.Remove(miscFunctions.c_SessionSuppReqID)
            End If
        End Set
    End Property

    Public Property mEditUserID As Integer
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionUserIDToEdit)) Then
                Return CInt(Session(miscFunctions.c_SessionUserIDToEdit))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If value <> 0 Then
                Session(miscFunctions.c_SessionUserIDToEdit) = value
            Else
                Session.Remove(miscFunctions.c_SessionUserIDToEdit)
            End If
        End Set
    End Property

    Public Property mUsername As String
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionUsername)) Then
                Return CStr(Session(miscFunctions.c_SessionUsername))
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            If Not IsNothing(value) Then
                Session(miscFunctions.c_SessionUsername) = value
            Else
                Session.Remove(miscFunctions.c_SessionUsername)
            End If
        End Set
    End Property

    Public ReadOnly Property mLoggedInUser As IntamacBL_SPR.MasterUser
        Get
            Dim loggedInUser As IntamacBL_SPR.MasterUser

            If Not IsNothing(Session(miscFunctions.c_SessionLoggedInUser)) Then
                loggedInUser = DirectCast(Session(miscFunctions.c_SessionLoggedInUser), IntamacBL_SPR.MasterUser)

                If loggedInUser.Username <> mUsername Then
                    loggedInUser = Nothing

                End If
            End If

            If IsNothing(loggedInUser) AndAlso Not String.IsNullOrEmpty(mUsername) Then
                loggedInUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(e_CompanyType.SPR)

                loggedInUser.Load(mUsername)

                Session(miscFunctions.c_SessionLoggedInUser) = loggedInUser


            End If


            Return loggedInUser

        End Get
    End Property

    Public ReadOnly Property mUsersCompany As IntamacBL_SPR.MasterCompany
        Get
            Dim usersCompany As IntamacBL_SPR.MasterCompany

            If Not IsNothing(mLoggedInUser) Then
                If Not IsNothing(Session(miscFunctions.c_SessionUsersCompany)) Then
                    usersCompany = DirectCast(Session(miscFunctions.c_SessionUsersCompany), IntamacBL_SPR.MasterCompany)

                    If usersCompany.MasterCoID <> mLoggedInUser.MasterCoID Then
                        usersCompany = Nothing

                    End If
                End If

                If IsNothing(usersCompany) Then
                    usersCompany = IntamacBL_SPR.ObjectManager.CreateMasterCompany(e_CompanyType.SPR)

                    usersCompany.Load(mLoggedInUser.MasterCoID)

                    Session(miscFunctions.c_SessionUsersCompany) = usersCompany
                End If
            End If

            Return usersCompany

        End Get
    End Property

    Public Property mDeviceSeq As Nullable(Of Integer)
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionDeviceSeq)) Then
                Return CInt(Session(miscFunctions.c_SessionDeviceSeq))
            Else
                Return Nothing
            End If
        End Get
        Set(value As Nullable(Of Integer))
            If Not IsNothing(value) Then
                Session(miscFunctions.c_SessionDeviceSeq) = value
            Else
                Session.Remove(miscFunctions.c_SessionDeviceSeq)
            End If
        End Set
    End Property

    Public Property mPropZone As Nullable(Of Integer)
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionPropZone)) Then
                Return CInt(Session(miscFunctions.c_SessionPropZone))
            Else
                Return Nothing
            End If
        End Get
        Set(value As Nullable(Of Integer))
            If Not IsNothing(value) Then
                Session(miscFunctions.c_SessionPropZone) = value
            Else
                Session.Remove(miscFunctions.c_SessionPropZone)
            End If
        End Set
    End Property

    Public Overridable Property mDeviceID As String
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionDeviceID)) Then
                Return Session(miscFunctions.c_SessionDeviceID)
            Else
                Return Nothing
            End If
        End Get
        Set(value As String)
            If Not IsNothing(value) Then
                Session(miscFunctions.c_SessionDeviceID) = value
            Else
                Session.Remove(miscFunctions.c_SessionDeviceID)
            End If
        End Set
    End Property


    Public Overridable Property mSelectedGateway As String
        Get
            Return CStr(Session("mSelectedGateway"))
        End Get
        Set(value As String)
            Session("mSelectedGateway") = value

        End Set
    End Property

    Public Property mAccountDetailTileSelected As Nullable(Of Dashboard.Tiles)
        Get
            If Not IsNothing(Session(miscFunctions.c_SessionAccountDetailTileSelected)) Then
                Return ([Enum].Parse(GetType(Dashboard.Tiles), Session(miscFunctions.c_SessionAccountDetailTileSelected)))
            Else
                Return Nothing
            End If
        End Get
        Set(value As Nullable(Of Dashboard.Tiles))
            If Not IsNothing(value) Then
                Session(miscFunctions.c_SessionAccountDetailTileSelected) = value
            Else
                Session.Remove(miscFunctions.c_SessionAccountDetailTileSelected)
            End If
        End Set
    End Property

#End Region

#Region "New/Overrides"

    Protected Overrides Sub OnInitComplete(e As EventArgs)

        If Not IsNothing(xhpTimerPanel) Then
            AddHandler xhpTimerPanel.ServiceRequest, New XmlHttpPanelEventHandler(AddressOf timerPanelService)
        End If

        Try
            mPageTitle = GetLocalResourceObject("PageTitle").ToString
        Catch ex As Exception

        End Try

        If Request.QueryString.HasKeys AndAlso Not String.IsNullOrEmpty(Request.QueryString("T")) Then
            IsTopNavigate = Request.QueryString("T") = "1"
        End If
        MyBase.OnInitComplete(e)


    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        ' If asp.net thinks the user is still logged in but the session has expired
        ' and a login is required, then redirect back to the login page.
        ' NB: if the user is no longer authenticated, asp.net will perform the redirect
        ' itself (based on web.config settings)
        If Page.User.Identity.IsAuthenticated AndAlso IsNothing(mLoggedInUser) AndAlso Not CanAccessCurrentPageAnonymously() Then
            Response.Redirect("~/AdminLogin.aspx", True)
            Exit Sub

        End If
        MyBase.OnLoad(e)

        If Not IsPostBack Then
            If Not IsNothing(Request.UrlReferrer) AndAlso String.IsNullOrEmpty(mBackLocation) Then
                mBackLocation = Request.UrlReferrer.AbsolutePath
            End If

            If Not mIsAccountPage Then
                mAccountID = Nothing
                mPropertyID = Nothing
            End If

        End If

        If blnCultureChanged Then
            For Each Grid As RadGrid In FindAllRadGrid(Page)
                If Grid.Items.Count > 0 Then
                    Grid.Rebind()
                End If
            Next
        End If

    End Sub
    Protected Overrides Sub InitializeCulture()

        Dim strOriginalCulture As String = ""

        If Not Request.Cookies(miscFunctions.c_CultureCookie) Is Nothing AndAlso Request.Cookies(miscFunctions.c_CultureCookie).Value <> "" Then

            mCulture = Request.Cookies(miscFunctions.c_CultureCookie).Value.ToString()
        End If


        If Request("Culture") IsNot Nothing Then

            '   Culture should always contain valid culture so below check unnecessary.  
            '   Saves having to update select when new country is introduced to service
            mCulture = Request("Culture")

        End If

        MyBase.InitializeCulture()

    End Sub


    Private Function CanAccessCurrentPageAnonymously() As Boolean
        Return Request.Url.AbsolutePath.ToLower.Contains("adminlogin.aspx")
    End Function

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        If Not ResponseComplete Then



            If Not IsPostBack Or blnCultureChanged Then
                If Not IsNothing(Master) Then
                    Dim pageLabel As Label = CType(DeepFindControl(Master, "lblPageTitle"), Label)

                    If Not (IsNothing(pageLabel) OrElse String.IsNullOrEmpty(mPageTitle)) Then
                        pageLabel.Text = mPageTitle
                    End If

                    Dim lblUserName As Label = CType(DeepFindControl(Master, "lblUserName"), Label)

                    If Not (IsNothing(lblUserName) OrElse IsNothing(mLoggedInUser)) Then
                        lblUserName.Text = mLoggedInUser.Firstname & IIf(Not String.IsNullOrEmpty(mLoggedInUser.Firstname), " ", "") & mLoggedInUser.Lastname
                    End If
                End If

                Try
                    Title = Server.HtmlDecode(String.Format("{0} - {1}", mSitetitle, mPageTitle))
                Catch
                    ' don't fail if resource object missing (most likely cause of a failure at this point)
                End Try

            End If

            If Not IsNothing(mTitleBar) Then
                If ShowTitleBar Then
                    If Not IsNothing(mBtnBack) Then
                        If mIsTopPage Then
                            mBtnBack.Visible = False
                        Else
                            If Not String.IsNullOrEmpty(mBackLocation) Then
                                mBtnBack.OnClientClick = String.Format("window.location = '{0}';return false;", mBackLocation)
                            Else
                                Dim topPage As String = CStr(GetGlobalResourceObject("MenuResources", String.Format("Menu{0}Page", pTopMenu)))
                                mBtnBack.OnClientClick = CStr(IIf(String.IsNullOrEmpty(topPage), "window.location.back();return false;", topPage))
                            End If
                        End If
                    End If
                Else
                    mTitleBar.Visible = False
                End If
            End If

            If Not String.IsNullOrEmpty(DefaultFocusID) Then
                Dim focusCtl As Control = DeepFindControl(Me, DefaultFocusID)

                If Not IsNothing(focusCtl) Then
                    Form.DefaultFocus = focusCtl.ClientID
                End If
            End If

            If Not String.IsNullOrEmpty(DefaultButtonID) Then
                Dim defCtl As Control = DeepFindControl(Me, DefaultButtonID)

                If Not IsNothing(defCtl) Then
                    Form.DefaultButton = defCtl.UniqueID

                End If
            End If

            Dim hdnSavePrompt As HiddenField = CType(FindMasterControl(Master, "hdnConfirmPrompt"), HiddenField)

            If Not IsNothing(hdnSavePrompt) Then
                hdnSavePrompt.Value = mConfirmPrompt
            End If
        End If

    End Sub

    Public Shadows Function GetGlobalResourceObject(className As String, resourceKey As String) As Object
        Return MyBase.GetGlobalResourceObject(className, resourceKey)
    End Function

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' Get resource string from local resources or global PageGlobalResources
    ''' </summary>
    ''' <param name="stringName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend Function PageString(ByVal stringName As String) As String
        Dim retString As String = "Not Found"
        Try
            retString = CStr(GetLocalResourceObject(stringName))
        Catch
        End Try
        If String.IsNullOrEmpty(retString) Then
            Try
                retString = CStr(GetGlobalResourceObject("PageGlobalResources", stringName))

            Catch ex As Exception

            End Try
        End If

        Return retString
    End Function

    Protected Friend Sub SafeRedirect(ByVal target As String, ByVal endResponse As Boolean)
        Response.Redirect(target, False)

        If endResponse Then
            HttpContext.Current.ApplicationInstance.CompleteRequest()
            _responseComplete = True

        End If
    End Sub

    Protected Function GetCountryCode(ByVal strCountryISO3 As String) As String

        Dim strCountryCode As String = GetGlobalResourceObject("PageGlobalResources", strCountryISO3 & "_CountryCode")

        If Not String.IsNullOrEmpty(strCountryCode) Then
            Return strCountryCode
        Else
            Return GetGlobalResourceObject("PageGlobalResources", "GBR_CountryCode")
        End If

    End Function

    Protected Shared Function GetCountryName(ByVal strCountryISO3 As String) As String

        Dim strCountryName As String = HttpContext.GetGlobalResourceObject("PageGlobalResources", strCountryISO3 & "_CountryName")

        If Not String.IsNullOrEmpty(strCountryName) Then
            Return strCountryName
        Else
            Return HttpContext.GetGlobalResourceObject("PageGlobalResources", "GBR_CountryName")
        End If

    End Function

#End Region

#Region "Static Methods"
    Public Shared Function DeepFindControl(ByVal target As Control, ByVal srchID As String) As Control

        Dim retCtl As Control = Nothing

        If Not (IsNothing(target) OrElse IsNothing(target.Controls)) AndAlso target.Controls.Count > 0 Then
            retCtl = target.FindControl(srchID)

            If IsNothing(retCtl) Then
                For Each child As Control In target.Controls
                    retCtl = DeepFindControl(child, srchID)

                    If Not IsNothing(retCtl) Then
                        Exit For
                    End If
                Next
            End If
        End If

        Return retCtl

    End Function

    Public Shared Function FindFirstLinkButton(ByVal target As Control) As LinkButton

        Dim retCtl As Control = Nothing

        If Not (IsNothing(target) OrElse IsNothing(target.Controls)) AndAlso target.Controls.Count > 0 Then


            For Each child As Control In target.Controls
                If TypeOf child Is System.Web.UI.WebControls.LinkButton Then
                    retCtl = child
                Else
                    If Not IsNothing(child.Controls) AndAlso child.Controls.Count > 0 Then
                        retCtl = FindFirstLinkButton(child)
                    End If
                End If

                If Not IsNothing(retCtl) Then
                    Exit For
                End If
            Next

        End If

        Return retCtl

    End Function

    Public Shared Function FindFirstTextBox(ByVal target As Control) As TextBox

        Dim retCtl As Control = Nothing

        If Not (IsNothing(target) OrElse IsNothing(target.Controls)) AndAlso target.Controls.Count > 0 Then


            For Each child As Control In target.Controls
                If TypeOf child Is System.Web.UI.WebControls.TextBox Then
                    retCtl = child
                Else
                    If Not IsNothing(child.Controls) AndAlso child.Controls.Count > 0 Then
                        retCtl = FindFirstTextBox(child)
                    End If
                End If

                If Not IsNothing(retCtl) Then
                    Exit For
                End If
            Next

        End If

        Return retCtl

    End Function

    Public Shared Function FindFirstDropdownList(ByVal target As Control) As DropDownList

        Dim retCtl As Control = Nothing

        If Not (IsNothing(target) OrElse IsNothing(target.Controls)) AndAlso target.Controls.Count > 0 Then

            For Each child As Control In target.Controls
                If TypeOf child Is System.Web.UI.WebControls.DropDownList Then
                    retCtl = child
                Else
                    If Not IsNothing(child.Controls) AndAlso child.Controls.Count > 0 Then
                        retCtl = FindFirstDropdownList(child)
                    End If
                End If

                If Not IsNothing(retCtl) Then
                    Exit For
                End If
            Next

        End If

        Return retCtl

    End Function

    Public Shared Function FindFirstRadGrid(ByVal target As Control) As RadGrid

        Dim retCtl As Control = Nothing

        If Not (IsNothing(target) OrElse IsNothing(target.Controls)) AndAlso target.Controls.Count > 0 Then

            For Each child As Control In target.Controls
                If TypeOf child Is RadGrid Then
                    retCtl = child
                Else
                    If Not IsNothing(child.Controls) AndAlso child.Controls.Count > 0 Then
                        retCtl = FindFirstRadGrid(child)
                    End If
                End If

                If Not IsNothing(retCtl) Then
                    Exit For
                End If
            Next

        End If

        Return retCtl

    End Function

    Public Shared Function FindAllRadGrid(ByVal target As Control) As List(Of RadGrid)
        Return FindAllRadGrid(target, Nothing)
    End Function

    Public Shared Function FindAllRadGrid(ByVal target As Control, ByVal InputRadlist As List(Of RadGrid)) As List(Of RadGrid)

        Dim RadList As List(Of RadGrid) = Nothing
        If InputRadlist IsNot Nothing Then
            RadList = InputRadlist
        Else
            RadList = New List(Of RadGrid)
        End If

        If Not (IsNothing(target) OrElse IsNothing(target.Controls)) AndAlso target.Controls.Count > 0 Then

            For Each child As Control In target.Controls
                If TypeOf child Is RadGrid Then
                    RadList.Add(DirectCast(child, RadGrid))
                Else
                    If Not IsNothing(child.Controls) AndAlso child.Controls.Count > 0 Then
                        FindAllRadGrid(child, RadList)
                    End If
                End If
            Next
        End If
        Return RadList

    End Function

    Public Shared Function FindFirstLoadingPanel(ByVal target As Control) As RadAjaxLoadingPanel

        Dim retCtl As Control = Nothing

        If Not (IsNothing(target) OrElse IsNothing(target.Controls)) AndAlso target.Controls.Count > 0 Then

            For Each child As Control In target.Controls
                If TypeOf child Is RadAjaxLoadingPanel Then
                    retCtl = child
                Else
                    If Not IsNothing(child.Controls) AndAlso child.Controls.Count > 0 Then
                        retCtl = FindFirstLoadingPanel(child)
                    End If
                End If

                If Not IsNothing(retCtl) Then
                    Exit For
                End If
            Next

        End If

        Return retCtl

    End Function

    Public Shared Function FindMasterControl(ByVal master As MasterPage, ByVal srchID As String) As Control
        Dim retCtl As Control

        If Not IsNothing(master) Then
            retCtl = DeepFindControl(master, srchID)

            If IsNothing(retCtl) AndAlso Not IsNothing(master.Master) Then
                FindMasterControl(master.Master, srchID)
            End If
        End If

        Return retCtl
    End Function

    Public Shared Function IsParentOf(ByVal parentCtl As Control, ByVal childCtl As Control)
        Dim blnIsParent As Boolean = False

        blnIsParent = Not IsNothing(childCtl.Parent) AndAlso childCtl.Parent Is parentCtl

        While Not (blnIsParent OrElse IsNothing(childCtl.Parent))
            blnIsParent = IsParentOf(parentCtl, childCtl.Parent)

        End While

        Return blnIsParent
    End Function
#End Region

    Private Sub Page_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete

        If Not IsNothing(xhpTimerPanel) AndAlso enablePageTimer AndAlso pageTimerMilliseconds > 0 Then
            Dim timerCommand = String.Format("setTimerCallBack({0});", pageTimerMilliseconds.ToString)
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(xhpTimerPanel, xhpTimerPanel.GetType(), "pageTimerCall", timerCommand, True)
            End If
        End If
    End Sub

    Protected Overridable Sub timerPanelService(ByVal sender As Object, ByVal args As RadXmlHttpPanelEventArgs)
        RaiseEvent timerPanelTick(sender, args)
    End Sub

    Public Event timerPanelTick As XmlHttpPanelEventHandler

End Class
