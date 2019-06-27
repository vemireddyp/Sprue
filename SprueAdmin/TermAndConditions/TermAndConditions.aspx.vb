Imports System.Runtime.Serialization

Imports IntamacShared_SPR.SharedStuff

Imports Newtonsoft.Json.Linq
Imports Telerik.Web.UI

Public Class TermAndConditions
    Inherits CultureBaseClass
    <FlagsAttribute>
    Public Enum AgreeModesEnum
        integrated = 1
        individual = 2
        combined = 3
    End Enum

    Public Enum displayModesEnum
        singleAgreement
        multiAgreement
    End Enum

    Public Property DisplaySet As Boolean

    <Serializable>
    Public Class PageMode
        Public Enum ModeActionKeyEnum
            send = 1
            redir = 2
        End Enum

        Public Property fileKeys As List(Of String)
        Public Property pageAction As ModeActionKeyEnum

        Public Property actionTarget As String
        Public Property urlKey As String
        Public Property acceptKeyRes As String

        Public Property displayMode As displayModesEnum


        Public Sub New()

        End Sub
        Public Sub New(ByVal itemJson As JObject)
            fileKeys = New List(Of String)(Convert.ToString(itemJson("fileKeys")).Split(","c))
            urlKey = itemJson("modeKey").ToString.ToLower
            If itemJson.Property("acceptAction") IsNot Nothing Then
                Dim actParts As String() = Convert.ToString(itemJson.Property("acceptAction").Value).Split(":"c)

                If actParts.Length = 2 Then
                    pageAction = [Enum].Parse(GetType(ModeActionKeyEnum), actParts(0))
                    actionTarget = actParts(1).ToLower()

                End If

            End If

            If itemJson.Property("displayMode") IsNot Nothing Then
                displayMode = [Enum].Parse(GetType(displayModesEnum), itemJson.Property("displayMode").Value.ToString)
            End If

            If itemJson("acceptKeyRes") IsNot Nothing Then
                acceptKeyRes = itemJson("acceptKeyRes").ToString
            End If
        End Sub

        Public Shared Function LoadModes(ByVal jsonDef As String) As Dictionary(Of String, PageMode)
            Dim retDict As New Dictionary(Of String, PageMode)

            Dim itemsJSON As JArray = JArray.Parse(jsonDef)

            For Each item As JObject In itemsJSON
                Dim newMode As New PageMode(item)

                retDict.Add(newMode.urlKey, newMode)

            Next


            Return retDict
        End Function


    End Class

    Private _loadModes As Dictionary(Of String, PageMode)
    Public ReadOnly Property LoadModes As Dictionary(Of String, PageMode)
        Get
            If _loadModes Is Nothing Then
                _loadModes = PageMode.LoadModes(GetLocalResourceObject("Modes"))
            End If

            Return _loadModes
        End Get
    End Property

    Public ReadOnly Property UserGuid As Guid
        Get
            Dim retGuid As Guid = Nothing

            If ViewState("UserGuid") IsNot Nothing Then
                retGuid = DirectCast(ViewState("UserGuid"), Guid)
            ElseIf Not IsPostBack AndAlso Request("id") IsNot Nothing Then
                retGuid = New Guid(Request("id"))
                ViewState("UserGuid") = retGuid
            End If

            Return retGuid
        End Get
    End Property

    Private ReadOnly Property thisMode As PageMode
        Get
            If ViewState("thisMode") Is Nothing AndAlso Not (IsPostBack OrElse Request("mode") Is Nothing) Then
                ViewState("thisMode") = LoadModes(Request.QueryString("mode").ToLower)
            End If
            Return DirectCast(ViewState("thisMode"), PageMode)
        End Get
    End Property

    Private ReadOnly Property modeKeys As List(Of String)
        Get
            If ViewState("modeKeys") Is Nothing AndAlso GuidIsValid Then
                Dim modeKeyList As New List(Of String)
                Dim fileKeys As New List(Of String)

                fileKeys = thisMode.fileKeys

                If Not fileKeys Is Nothing AndAlso fileKeys.Count > 0 Then
                    For Each fileKey In fileKeys
                        modeKeyList.AddRange(GetLocalResourceObject(fileKey + "Files").split(","))

                    Next
                    For Each modeKey In modeKeyList
                        fileKeysDone.Add(modeKey, False)
                    Next
                End If

                ViewState("modeKeys") = modeKeyList
            End If
            Return DirectCast(ViewState("modeKeys"), List(Of String))
        End Get
    End Property

    Private ReadOnly Property agreeKeys As List(Of AgreementsListItem)
        Get
            If ViewState("agreeKeys") Is Nothing Then
                ViewState("agreeKeys") = New List(Of AgreementsListItem)
            End If

            Return DirectCast(ViewState("agreeKeys"), List(Of AgreementsListItem))

        End Get
    End Property

    Private ReadOnly Property adminUserGen As IntamacBL_SPR.UserGeneration
        Get
            If HttpContext.Current.Items("adminUserGen") Is Nothing Then
                Dim uga As IntamacBL_SPR.UserGenerationAgent = IntamacBL_SPR.ObjectManager.CreateUserGenerationAgent("SQL")
                Dim userGen As IntamacBL_SPR.UserGeneration = uga.Load(UserGuid)
                HttpContext.Current.Items("adminUserGen") = userGen

            End If

            Return DirectCast(HttpContext.Current.Items("adminUserGen"), IntamacBL_SPR.UserGeneration)
        End Get
    End Property

    Private ReadOnly Property adminUser As IntamacBL_SPR.MasterUser
        Get
            If HttpContext.Current.Items("adminUser") Is Nothing Then
                Dim uga As IntamacBL_SPR.MasterUser = IntamacBL_SPR.ObjectManager.CreateMasterUser(e_CompanyType.SPR)
                If uga.LoadByPasswordResetGUID(UserGuid.ToString) Then
                    HttpContext.Current.Items("adminUser") = uga

                End If

            End If

            Return DirectCast(HttpContext.Current.Items("adminUser"), IntamacBL_SPR.MasterUser)

        End Get
    End Property
    Private ReadOnly Property userPerson As IntamacBL_SPR.Person
        Get
            If HttpContext.Current.Items("userPerson") Is Nothing Then
                Dim upa As IntamacBL_SPR.PersonAgent = IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL")
                Dim personObj As IntamacBL_SPR.Person = upa.GetByPersonGuid(UserGuid)
                HttpContext.Current.Items("userPerson") = personObj
            End If
            Return DirectCast(HttpContext.Current.Items("userPerson"), IntamacBL_SPR.Person)
        End Get
    End Property

    Private ReadOnly Property fileKeysDone As Dictionary(Of String, Boolean)
        Get
            If Session("fileKeysDone") Is Nothing Then
                Session("fileKeysDone") = New Dictionary(Of String, Boolean)
            End If

            Return DirectCast(Session("fileKeysDone"), Dictionary(Of String, Boolean))
        End Get
    End Property
    Private ReadOnly Property GuidIsValid As Boolean
        Get
            Dim isValid As Boolean = False
            If thisMode IsNot Nothing Then

                Select Case thisMode.urlKey
                    Case "us", "ap"
                        If userPerson IsNot Nothing Then
                            Select Case thisMode.urlKey
                                Case "us"
                                    isValid = (userPerson.Terms_accepted_app = CInt(e_TermsAndConditionsStatus.NotAccepted) AndAlso userPerson.Terms_accepted_user = CInt(e_TermsAndConditionsStatus.NotAccepted))
                                Case "ap"
                                    isValid = (userPerson.Terms_accepted_app = CInt(e_TermsAndConditionsStatus.NotAccepted) AndAlso userPerson.Terms_accepted_user <> CInt(e_TermsAndConditionsStatus.NotAccepted))

                            End Select
                        End If

                    Case "ad"

                        If adminUserGen IsNot Nothing Then
                            If DateDiff(DateInterval.Hour, CType(adminUserGen.CreatedDate, Date), DateTime.Now) < 24 Then
                                isValid = True
                            End If
                        Else
                            If adminUser IsNot Nothing Then
                                isValid = adminUser.Terms_accepted_admin = CInt(e_TermsAndConditionsStatus.NotAccepted)
                            End If
                        End If

                End Select
            End If

            Return isValid
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        If GuidIsValid Then

            If Not IsPostBack Then
                If Session("fileKeysDone") IsNot Nothing Then
                    Session.Remove("fileKeysDone")
                End If
                If Session(miscFunctions.c_SessionTermsAcceptedDate) IsNot Nothing Then
                    Session.Remove(miscFunctions.c_SessionTermsAcceptedDate)
                End If

                If Not String.IsNullOrEmpty(thisMode.acceptKeyRes) Then
                    btnContinue.Text = GetLocalResourceObject(thisMode.acceptKeyRes).ToString

                    If thisMode.urlKey <> "ad" Then
                        btnContinue.Style.Add("display", "none")
                    End If

                End If

                DisplaySet = False

                docShowView.DataSource = modeKeys
                docShowView.DataBind()

                If agreeKeys IsNot Nothing AndAlso agreeKeys.Count > 0 Then
                    docAgreementsView.DataSource = agreeKeys
                    docAgreementsView.DataBind()
                End If
            End If
        Else
            badRequestDiv.Visible = True
            acceptDiv.Visible = False
            confDiv.Visible = False
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)
        SiteHeader.Visible = False

    End Sub

    Dim fileKey As String = Nothing

    <Serializable>
    Private Class AgreementsListItem
        Public Property resourceClass As String
        Public Property resourceKey As String
        Public Property resourceClassKey As String

        Public Property isAgreed As Boolean

        Public Sub New(ByVal resClass As String, ByVal resClassKey As String, ByVal key As String)
            resourceClass = resClass
            resourceKey = key
            resourceClassKey = resClassKey
            isAgreed = False

        End Sub


    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' NC 2016
    ''' </remarks>
    Private Sub docShowView_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles docShowView.ItemDataBound
        If TypeOf (e.Item) Is RadListViewDataItem Then

            Dim workItem = DirectCast(e.Item, RadListViewItem)
            Dim dataKey As String = DirectCast(e.Item, RadListViewDataItem).DataItem.ToString
            Dim agreeMode As AgreeModesEnum = AgreeModesEnum.integrated

            Dim fileKey = String.Format("{0}.eula", dataKey)
            Dim docLabel As Label = DirectCast(e.Item.FindControl("lblDocAccept"), Label)

            Dim labelRes As String = GetGlobalResourceObject(fileKey, "EndUserLicenceAgreement")
            docLabel.Text = labelRes


            Dim divLabel As HtmlGenericControl = DirectCast(e.Item.FindControl("labelDiv"), HtmlGenericControl)
            Dim checkKeys As String = GetGlobalResourceObject(fileKey, "AgreementKeys")

            If Not String.IsNullOrEmpty(checkKeys) Then
                divLabel.Attributes.Add("data-filekey", fileKey)

                Dim divContainer As HtmlGenericControl = DirectCast(e.Item.FindControl("containerDiv"), HtmlGenericControl)
                If thisMode.displayMode = displayModesEnum.multiAgreement AndAlso (DisplaySet OrElse fileKeysDone(dataKey)) Then
                    divContainer.Style.Add("display", "none")
                Else
                    divContainer.Style.Add("display", "block")
                    DisplaySet = True
                End If

                If Not String.IsNullOrEmpty(GetGlobalResourceObject(fileKey, "AgreementsMode")) Then
                    [Enum].TryParse(Of AgreeModesEnum)(GetGlobalResourceObject(fileKey, "AgreementsMode").ToString, agreeMode)
                End If
                If (agreeMode And AgreeModesEnum.integrated) = agreeMode Then
                    For Each keyClass In checkKeys.Split(","c)
                        agreeKeys.Add(New AgreementsListItem(fileKey, dataKey, keyClass))
                    Next
                End If
                If (agreeMode And AgreeModesEnum.individual) = agreeMode Then
                    LoadAgreementsChecks(fileKey, e)
                End If

            End If
        End If
    End Sub

    Private Sub LoadAgreementsChecks(fileKey As String, e As RadListViewItemEventArgs)

        Dim agreeList As RadListView = DirectCast(e.Item.FindControl("docAcceptView"), RadListView)
        If agreeList IsNot Nothing Then

            Dim checkKeys As String = GetGlobalResourceObject(fileKey, "AgreementKeys")
            AddHandler agreeList.ItemDataBound, AddressOf docAcceptView_ItemDataBound


            If Not String.IsNullOrEmpty(checkKeys) Then
                agreeList.Attributes.Add("data-filekey", fileKey)
                agreeList.Attributes.Add("data-datakey", fileKey.Split("."c)(0))

                agreeList.DataSource = checkKeys.Split(","c)
                agreeList.DataBind()

            End If

        End If
    End Sub

    Private Sub UpdateUserPerson()
        Dim upa As IntamacBL_SPR.PersonAgent = IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL")
        upa.Update(userPerson)
    End Sub
    Private Sub docAgreementsView_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles docAgreementsView.ItemDataBound
        If TypeOf (e.Item) Is RadListViewDataItem Then
            Dim fileKey As String = Nothing

            Dim agreement As AgreementsListItem = DirectCast(DirectCast(e.Item, RadListViewDataItem).DataItem, AgreementsListItem)

            fileKey = agreement.resourceClass

            LoadAgreementsChecks(fileKey, e)
        End If


    End Sub
    Private Sub docAcceptView_ItemDataBound(sender As Object, e As RadListViewItemEventArgs)
        If TypeOf (e.Item) Is RadListViewDataItem Then

            fileKey = DirectCast(sender, WebControl).Attributes("data-filekey")
            Dim fileDataKey As String = DirectCast(sender, WebControl).Attributes("data-datakey")

            Dim dataKey As String = DirectCast(e.Item, RadListViewDataItem).DataItem.ToString

            Dim agreeCheck As CheckBox = DirectCast(e.Item.FindControl("chkDocAccept"), CheckBox)

            agreeCheck.Attributes.Add("data-filekey", fileKey)
            agreeCheck.Attributes.Add("data-datakey", fileDataKey)

            Dim checkText As String = GetGlobalResourceObject(fileKey, dataKey)

            agreeCheck.Text = checkText
        End If
    End Sub

    Private Function allAgreementsChecked() As Boolean
        Dim allAgreed As Boolean = False

        For Each isDone As Boolean In fileKeysDone.Values
            allAgreed = isDone
            If Not allAgreed Then
                Exit For
            End If
        Next

        Return allAgreed
    End Function

    Private Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
        If Page.IsValid AndAlso GuidIsValid AndAlso allAgreementsChecked() Then
            Select Case thisMode.urlKey
                Case "us", "ap"
                    If thisMode.urlKey = "us" Then
                        userPerson.Terms_accepted_user = CInt(e_TermsAndConditionsStatus.Accepted)
                        lblEmailLabel.Text = ""
                    Else
                        lblEmailLabel.Text = GetLocalResourceObject("NoEmailLabel")
                    End If

                    userPerson.Terms_accepted_app = CInt(e_TermsAndConditionsStatus.Accepted)
                    userPerson.Terms_accepted_date = Date.UtcNow
                    UpdateUserPerson()
                    acceptDiv.Visible = False
                    confDiv.Visible = True
                    emailDiv.Visible = True

                Case "ad"
                    If adminUser IsNot Nothing Then
                        adminUser.Terms_accepted_admin = CInt(e_TermsAndConditionsStatus.Accepted)
                        adminUser.Terms_accepted_date = Date.UtcNow
                        adminUser.Save()
                        thisMode.actionTarget = "~"
                    End If
                    emailDiv.Visible = False
            End Select

            Select Case thisMode.pageAction
                Case PageMode.ModeActionKeyEnum.redir
                    If thisMode.actionTarget IsNot Nothing Then
                        Session(miscFunctions.c_SessionTermsAcceptedDate) = Date.UtcNow
                        SafeRedirect(thisMode.actionTarget.Replace("###", UserGuid.ToString), True)
                    End If

                Case PageMode.ModeActionKeyEnum.send
                    If thisMode.urlKey = "ap" Then
                        ' send activation email for app (only) user
                        Dim strLanguage As String = "en-GB"
                        If mCulture IsNot Nothing AndAlso mCulture.Length > 0 Then
                            strLanguage = IntamacShared_SPR.SharedStuff.CultureCodeToLanguage(mCulture)
                        End If

                        If userPerson IsNot Nothing Then
                            Dim oClientAgent = CType(IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL"), IntamacBL_SPR.ClientAgent)
                            oClientAgent.SendEmailAddressValidationEmail(userPerson.AccountID, userPerson.PersonID, userPerson.FirstName, userPerson.LastName, userPerson.Email, strLanguage)
                            emailDiv.Visible = True
                            lblEmailLabel.Text = GetLocalResourceObject("EmailLabel")
                        End If
                    End If
            End Select
        End If
    End Sub

    Private Sub xhpAgreeChecked_ServiceRequest(sender As Object, e As RadXmlHttpPanelEventArgs) Handles xhpAgreeChecked.ServiceRequest
        If e.Value IsNot Nothing Then
            Dim checkVals As String() = e.Value.Split(","c)

            If fileKeysDone.ContainsKey(checkVals(0)) Then
                fileKeysDone(checkVals(0)) = CBool(checkVals(1))
            End If
        End If
    End Sub
End Class