Imports IntamacBL_SPR
Imports System.Reflection
Imports Telerik.Web.UI
Imports IntamacShared_SPR
Imports IntamacShared_SPR.SharedStuff
Imports ClassLibrary_Interface

Public Class Contacts
    Inherits CultureBaseClass

    Private _allowRiskLevels As Boolean
    Private _refreshState As Boolean
    Private _isRefresh As Boolean

#Region "New/Overrides"

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)

        ' On postback, save refresh state
        Dim AllStates As Object() = savedState
        MyBase.LoadViewState(AllStates(0))
        ' Get refresh state (False) so can be used in postback events on this page.
        ' Storing refresh state avoids duplicate records from being added
        _refreshState = Boolean.Parse(AllStates(1))
        _isRefresh = _refreshState = Session("__ISREFRESH")

    End Sub

    Protected Overrides Function SaveViewState() As Object

        ' On page request, save state to variable so can be used in LoadViewState method
        Session("__ISREFRESH") = _refreshState

        Dim AllStates() As Object = New Object(2) {}
        AllStates(0) = MyBase.SaveViewState
        ' Save refresh state (True) in view state
        ' Here, we do not want to run any CRUD operations on page refresh
        AllStates(1) = Not (_refreshState)

        Return AllStates

    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

        hdnActivationCodeSentAlert.Value = GetLocalResourceObject("ConfirmSent").ToString()
        hdnTermsAndConditionsSentAlert.Value = GetLocalResourceObject("TermsAndConditionsSentAlert").ToString()

        ' copy value from this method to avoid unnecessary extra db lookups:
        _allowRiskLevels = AreRiskLevelsAllowed()

        If Not IsPostBack Or _isRefresh Then

            Dim lstContacts As List(Of IntamacBL_SPR.Contacts) = Nothing

            'Load all known Contacts 
            lstContacts = LoadKnownContacts()

            'display the identified contacts
            DisplayKnownContacts(lstContacts)

        End If

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Display known contacts, based upon contacts in provided list parameter
    ''' </summary>
    ''' <param name="lstContacts"></param>
    ''' <remarks></remarks>
    Private Sub DisplayKnownContacts(ByRef lstContacts As List(Of IntamacBL_SPR.Contacts))

        'Show the AddNewContact button if more than 4 contacts
        btnAddContact.Visible = lstContacts.Count > 4
        'Display the five default empty contact rows  SP-352
        Dim oContactToAdd As IntamacBL_SPR.Contacts = Nothing
        If lstContacts.Count >= 0 And lstContacts.Count < 5 Then
            For index = 1 To 5
                If lstContacts.Count = 5 Then
                    'already 5 contacts - no need to show any empty rows
                    Exit For
                Else
                    'add a blank contact record
                    oContactToAdd = New IntamacBL_SPR.Contacts
                    oContactToAdd.ContactNumber = lstContacts.Count + 1
                    lstContacts.Add(oContactToAdd)
                End If
            Next
        End If

        rptContacts.DataSource = lstContacts
        rptContacts.DataBind()

        btnSave.Visible = lstContacts.Count > 0

    End Sub

    ''' <summary>
    ''' Load all known Contacts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Function LoadKnownContacts() As List(Of IntamacBL_SPR.Contacts)

        Dim oContacts As IntamacBL_SPR.Contacts = Nothing
        Dim lstContacts As List(Of IntamacBL_SPR.Contacts) = Nothing

        Try

            'instantiate the Contacts class
            oContacts = New IntamacBL_SPR.Contacts ' IntamacBL_SPR.ObjectManager.CreateContact(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR)

            'loads a list of all contacts for the current property
            lstContacts = oContacts.Load(mAccountID, mPropertyID)

        Finally
            oContacts = Nothing
        End Try

        Return lstContacts
    End Function

    Private Sub HideAllMessages()
        divSaveFailed.Visible = False
        divSaveSuccessful.Visible = False
        divDeleted.Visible = False
        divDeleteFailed.Visible = False
    End Sub

#End Region

    ''' <summary>
    ''' Indicates that the contact should have Push Notifications sent to the installed app.  Invite them to install the app
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Send_Click(ByVal sender As Object, ByVal e As EventArgs)

        If Page.IsValid Then

            'Get the reference of the clicked button.
            Dim button As Button = CType(sender, Button)

            'Get the command argument
            Dim commandArgument As String = button.CommandArgument

            'Get the Repeater Item reference
            Dim item As RepeaterItem = CType(button.NamingContainer, RepeaterItem)

            'Get the repeater item index
            Dim index As Integer = item.ItemIndex
            Dim personID As String = Convert.ToInt32(commandArgument)

            Dim oPersonAgent As PersonAgent = Nothing
            Dim oPerson As iPerson = Nothing

            oPersonAgent = CType(IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL"), IntamacBL_SPR.PersonAgent)

            oPerson = oPersonAgent.LoadByPersonId(v_intPersonID:=personID)

            If oPerson IsNot Nothing Then
                If oPerson.Terms_accepted_app = e_TermsAndConditionsStatus.Accepted Then
                    If InviteContact(oPerson) Then
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ActivationCodeSentAlert", "<script type='text/javascript'>ActivationCodeSentAlert();</script>", False)
                    End If
                Else
                    If SendEmailTermAndConditions(oPerson) Then
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "TermsAndConditionsSentAlert", "<script type='text/javascript'>TermsAndConditionsSentAlert();</script>", False)
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Indicates that the contact should be Deleted
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Delete_Click(ByVal sender As Object, ByVal e As EventArgs)

        If Page.IsValid Then

            'Get the reference of the clicked button.
            Dim button As Button = CType(sender, Button)

            'Get the command argument
            Dim commandArgument As String = button.CommandArgument

            'Get the Repeater Item reference
            Dim item As RepeaterItem = CType(button.NamingContainer, RepeaterItem)

            'Get the repeater item index
            Dim index As Integer = item.ItemIndex

            If DeleteContact(Convert.ToInt32(commandArgument)) Then
                'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CompletedSend", "<script type='text/javascript'>CompletedSentAlert();</script>", False)
            End If

        End If
    End Sub

    Private Function DeleteContact(ByVal PersonID As String) As Boolean
        Dim oClient As ClientAgent = Nothing
        Dim oPersonAgent As PersonAgent = Nothing
        Dim oPerson As ClassLibrary_Interface.iPerson = Nothing
        Dim bRet As Boolean = False
        Dim oContacts As IntamacBL_SPR.Contacts = Nothing
        Dim lstContacts As List(Of IntamacBL_SPR.Contacts) = Nothing

        Try

            oClient = CType(IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL"), IntamacBL_SPR.ClientAgent)

            oPersonAgent = CType(IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL"), IntamacBL_SPR.PersonAgent)

            HideAllMessages()

            If oPersonAgent.Delete(PersonID) Then
                divDeleted.Visible = True

                'Load all known Contacts 
                lstContacts = LoadKnownContacts()

                'display the identified contacts
                DisplayKnownContacts(lstContacts)
            Else
                divDeleteFailed.Visible = True
            End If

        Catch ex As Exception

        Finally
            oClient = Nothing
            oPerson = Nothing
            oPersonAgent = Nothing
        End Try

        Return bRet
    End Function

    ''' <summary>
    ''' Send the term and condtions email to accept 
    ''' </summary>
    ''' <param name="oPerson"></param>
    ''' <returns></returns>
    Private Function SendEmailTermAndConditions(oPerson As iPerson) As Boolean

        Dim oClientAgent As ClientAgent = Nothing
        Dim oPersonAgent As PersonAgent = Nothing
        Dim oClient As ClassLibrary_Interface.iClient = Nothing
        Dim bRet As Boolean = False
        Dim strLanguage As String = "EN"
        Dim mode As String = "AP"

        Try

            oClientAgent = CType(IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL"), IntamacBL_SPR.ClientAgent)

            oPersonAgent = CType(IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL"), IntamacBL_SPR.PersonAgent)

            oPerson = oPersonAgent.LoadByPersonId(oPerson.PersonID)

            oClient = oClientAgent.Load(mAccountID)

            If oClient IsNot Nothing AndAlso Not String.IsNullOrEmpty(oClient.Culture) Then
                strLanguage = IntamacShared_SPR.SharedStuff.CultureCodeToLanguage(oClient.Culture)

            End If

            If oPerson IsNot Nothing Then
                oPersonAgent.SendEmailTermAndConditionsToAccept(oPerson, mode, strLanguage)
                bRet = True
            End If

        Catch ex As Exception

        Finally
            oPerson = Nothing
            oPersonAgent = Nothing
        End Try

        Return bRet
    End Function

    ''' <summary>
    ''' Send activation code email
    ''' </summary>
    ''' <param name="oPerson"></param>
    ''' <returns></returns>
    Private Function InviteContact(oPerson As iPerson) As Boolean
        Dim oClientAgent As ClientAgent = Nothing
        Dim oPersonAgent As PersonAgent = Nothing
        Dim oClient As ClassLibrary_Interface.iClient = Nothing
        Dim bRet As Boolean = False
        Dim strLanguage As String = "EN"

        Try

            oClientAgent = CType(IntamacBL_SPR.ObjectManager.CreateClientAgent("SQL"), IntamacBL_SPR.ClientAgent)

            oPersonAgent = CType(IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL"), IntamacBL_SPR.PersonAgent)

            oPerson = oPersonAgent.LoadByPersonId(oPerson.PersonID)

            oClient = oClientAgent.Load(mAccountID)

            If oClient IsNot Nothing AndAlso Not String.IsNullOrEmpty(oClient.Culture) Then

                strLanguage = IntamacShared_SPR.SharedStuff.CultureCodeToLanguage(oClient.Culture)

            End If
            ' need to setup Eula fields
            If oPerson IsNot Nothing Then
                bRet = oClientAgent.SendEmailAddressValidationEmail(oPerson.AccountID, oPerson.PersonID, oPerson.FirstName, oPerson.LastName, oPerson.Email, strLanguage)
            End If

        Catch ex As Exception

        Finally
            oClient = Nothing
            oPerson = Nothing
            oPersonAgent = Nothing
        End Try

        Return bRet
    End Function

    Private Function SaveContactExisting(ByRef ContactRecord As RepeaterItem) As Boolean
        Dim oPersonAgent As PersonAgent = Nothing
        Dim oPerson As Person = Nothing
        Dim bRet As Boolean = False
        Dim subLink As IntamacBL_SPR.SubscriptionLinkSPR = Nothing
        Dim bSubLinkLoaded As Boolean = False

        Dim ic As IntamacBL_SPR.IntamacContact = Nothing
        Dim lstContactNames As List(Of String) = Nothing
        'Dim lblEmailFormatInvalid As Label = CType(ContactRecord.FindControl("lblEmailFormatInvalid"), Label)
        'Dim lblContactNumberInvalid As Label = CType(ContactRecord.FindControl("lblContactNumberInvalid"), Label)

        Try

            'If lblEmailFormatInvalid IsNot Nothing Then lblEmailFormatInvalid.Visible = False
            'If lblContactNumberInvalid IsNot Nothing Then lblContactNumberInvalid.Visible = False

            oPersonAgent = CType(IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL"), IntamacBL_SPR.PersonAgent)

            Dim ExistingContactPersonID As Integer = DirectCast(ContactRecord.FindControl("btnSend"), System.Web.UI.WebControls.Button).CommandArgument     'picks up 1 everytime

            'Dim contactSeq As Integer = CType(ContactRecord.FindControl("hdnContactSeq"), HiddenField).Value.Trim()
            Dim contactSeq As Integer = CType(ContactRecord.FindControl("lblContactNumber"), Label).Text.Trim()

            If ExistingContactPersonID > 0 Then
                oPerson = oPersonAgent.LoadByPersonId(ExistingContactPersonID)

                oPerson.Email = CType(ContactRecord.FindControl("txtEmail"), TextBox).Text.Trim()

                'lstContactNames = SplitContactName(CType(ContactRecord.FindControl("txtContactName"), TextBox).Text.Trim())

                oPerson.FirstName = CType(ContactRecord.FindControl("txtContactName"), TextBox).Text.Trim()
                oPerson.LastName = CType(ContactRecord.FindControl("txtContactName"), TextBox).Text.Trim()

                'If lstContactNames IsNot Nothing Then
                '	If lstContactNames.Count > 0 Then
                '		'at least one element found, so place the first element entirely within the first name
                '		oPerson.FirstName = lstContactNames(0)
                '	End If

                '	If lstContactNames.Count > 1 Then
                '		'space found within the name, so put everything after the first space in the last name field
                '		oPerson.LastName = lstContactNames(1)
                '	End If
                'End If

                oPerson.MobileNo = CType(ContactRecord.FindControl("txtContactNumber"), TextBox).Text.Trim()
                oPerson.Organisation = CType(ContactRecord.FindControl("txtOrganisation"), TextBox).Text.Trim()
                oPerson.ReceivesEmail = True
                oPerson.ReceivesPush = True
                oPerson.IsActive = True


                'Removed as not required
                'Dim chkPush As CheckBox = DirectCast(ContactRecord.FindControl("chkApps"), CheckBox)

                'If chkPush IsNot Nothing Then
                '    oPerson.ReceivesPush = chkPush.Checked
                'End If

                bRet = oPersonAgent.Update(oPerson)

            End If

            If bRet Then

                'need to save the contact with a subscriptionlinkid value that maps to the only subscriptionlink record the property should have
                subLink = New IntamacBL_SPR.SubscriptionLinkSPR

                'load the subscription link record to get the id
                bSubLinkLoaded = subLink.Load(mPropertyID, 1)

                Dim addedContactLists As String

                If bSubLinkLoaded Then
                    Dim contactListssToUpdate = GetContactListSelections(ContactRecord).ToList()

                    Dim addedContacts = (From c In contactListssToUpdate Where c.Item2 Select c.Item1).ToList()

                    If addedContacts.Count = 0 Then
                        'no options selected, so add default contact list of id 0 (this allows contacts to still be linked to property level)
                        contactListssToUpdate.Add(New Tuple(Of SharedStuff.e_ContactList, Boolean)(SharedStuff.e_ContactList.DefaultList, True))
                    Else
                        'at least one option selected, so remove default contact list of id 0
                        contactListssToUpdate.Add(New Tuple(Of SharedStuff.e_ContactList, Boolean)(SharedStuff.e_ContactList.DefaultList, False))
                    End If

                    UpdateContactLists(oPerson, subLink, contactListssToUpdate, contactSeq)
                    addedContactLists = GetContactListsAddedAuditString(contactListssToUpdate)

                Else
                    addedContactLists = String.Empty
                End If

                miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Save_Alert_Contact, String.Format("{0} {1}, {2} - {3}", PageString("ContactListsText"), addedContactLists, oPerson.FirstName, oPerson.Email))

            Else

                DisplayValidationMessage(ContactRecord, oPersonAgent)

            End If

        Finally
            oPersonAgent = Nothing
            oPerson = Nothing
        End Try

        Return bRet
    End Function

    Private Sub DisplayValidationMessage(ByRef ContactRecord As RepeaterItem, ByRef oPersonAgent As PersonAgent)

        Dim valContactName As RequiredFieldValidator = CType(ContactRecord.FindControl("valContactName"), RequiredFieldValidator)
        Dim valEmail As RequiredFieldValidator = CType(ContactRecord.FindControl("valEmail"), RequiredFieldValidator)
        Dim valEmailFormat As RegularExpressionValidator = CType(ContactRecord.FindControl("valEmailFormat"), RegularExpressionValidator)
        Dim valContactNumberFormat As RegularExpressionValidator = CType(ContactRecord.FindControl("valContactNumberFormat"), RegularExpressionValidator)

        If oPersonAgent.Messages.Count > 0 Then
            For Each message As String In oPersonAgent.Messages

                If message.Trim().ToUpper = "FIRSTNAME REQUIRED" Then

                    If valContactName IsNot Nothing Then valContactName.Validate()

                ElseIf message.Trim().ToUpper = "LASTNAME REQUIRED" Then

                    If valContactName IsNot Nothing Then valContactName.Validate()

                ElseIf message.Trim().ToUpper = "EMAIL REQUIRED" Then

                    If valEmail IsNot Nothing Then valEmail.Validate()

                ElseIf message.Trim().ToUpper = "EMAIL ADDRESS INVALID" Then

                    If valEmailFormat IsNot Nothing Then valEmailFormat.Validate()

                ElseIf message.Trim().ToUpper = "CONTACT NUMBER INVALID" Then

                    If valContactNumberFormat IsNot Nothing Then valContactNumberFormat.Validate()

                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' Get a collection of contact lists and whether they were checked.
    ''' </summary>
    ''' <param name="container"></param>
    ''' <returns></returns>
    ''' <remarks>A collection of tuples where Item1 is the contact list type and Item2 is whether it was checked</remarks>
    Private Iterator Function GetContactListSelections(container As RepeaterItem) As IEnumerable(Of Tuple(Of SharedStuff.e_ContactList, Boolean))
        Yield GetContactListSelection(container, "chkAlert", SharedStuff.e_ContactList.Alerts)
        Yield GetContactListSelection(container, "chkFault", SharedStuff.e_ContactList.Fault)
        If _allowRiskLevels Then
            Yield GetContactListSelection(container, "chkCommunity", SharedStuff.e_ContactList.Community)
        End If
    End Function

    Private Shared Function GetContactListSelection(container As RepeaterItem, fieldName As String, contactList As SharedStuff.e_ContactList) As Tuple(Of SharedStuff.e_ContactList, Boolean)
        Dim checked As Boolean = IsChecked(container, fieldName)
        Return New Tuple(Of SharedStuff.e_ContactList, Boolean)(contactList, checked)
    End Function

    Private Shared Function IsChecked(container As RepeaterItem, checkboxName As String) As Boolean
        Return CType(container.FindControl(checkboxName), CheckBox).Checked
    End Function

    ''' <summary>
    ''' Insert or delete the given contact lists.
    ''' </summary>
    ''' <param name="person"></param>
    ''' <param name="subLink"></param>
    ''' <param name="contactListssToUpdate">A collection of tuples where Item1 is the contact list and Item2 is true if adding, false if deleting</param>
    ''' <remarks></remarks>
    Private Sub UpdateContactLists(person As Person, subLink As SubscriptionLink, contactListssToUpdate As IEnumerable(Of Tuple(Of SharedStuff.e_ContactList, Boolean)), contactSeq As Integer)
        Dim ic = New IntamacBL_SPR.IntamacContact()
        For Each contactListToUpdate In contactListssToUpdate
            Dim delete As Boolean = Not contactListToUpdate.Item2
            ic.SaveExistingContact(mAccountID, person.PersonID, subLink.IntaSubscriptionLinkID, contactListToUpdate.Item1, contactSeq, delete)
        Next
    End Sub

    ''' <summary>
    ''' Gets a string that can be auditing containing all contact lists that were added.
    ''' </summary>
    ''' <param name="contactListssToUpdate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetContactListsAddedAuditString(contactListssToUpdate As IEnumerable(Of Tuple(Of SharedStuff.e_ContactList, Boolean))) As String
        Dim addedContacts = From c In contactListssToUpdate Where c.Item2 Select GetAuditString(c.Item1)
        Return String.Join("/", addedContacts)
    End Function

    Private Function GetAuditString(contactList As SharedStuff.e_ContactList) As String
        Select Case contactList
            Case SharedStuff.e_ContactList.Fault
                Return PageString("FaultText")
            Case SharedStuff.e_ContactList.Alerts
                Return PageString("AlertText")
            Case Else
                Return PageString(contactList.ToString() & "Text")
        End Select
    End Function



    ''' <summary>
    ''' Add or remove a "contact" entry for the given contacts list, depending on whether the chexckbox was ticked or not.
    ''' Returns True if the contact was added, False if the contact was removed.
    ''' </summary>
    ''' <param name="container"></param>
    ''' <param name="checkboxName"></param>
    ''' <param name="contactList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateContactList(container As RepeaterItem, checkboxName As String, contactList As SharedStuff.e_ContactList, person As Person, subLink As SubscriptionLink) As Boolean
        Dim ic = New IntamacBL_SPR.IntamacContact()
        Dim checked = CType(container.FindControl(checkboxName), CheckBox).Checked
        Dim delete = Not checked
        ic.SaveExistingContact(mAccountID, person.PersonID, subLink.IntaSubscriptionLinkID, contactList, 1, delete)
        Return checked
    End Function

    ''' <summary>
    ''' Splits the Contact Name into two by space, therefore providing First and Last name values
    ''' </summary>
    ''' <param name="ContactName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SplitContactName(ByVal ContactName As String) As List(Of String)
        Dim lstNames As List(Of String) = Nothing
        Dim ContactNameAllText As String = ""

        Try

            lstNames = New List(Of String)

            If ContactName.Trim().Contains(Space(1)) Then
                If ContactName.Trim().IndexOf(Space(1)) = ContactName.Trim().LastIndexOf(Space(1)) Then
                    'only one space value in the contact name
                    lstNames = ContactName.Trim().Split(Space(1)).ToList()
                Else
                    'more than one space, so put the text before the first space in element Zero and everything else in the next element

                    'store the contact name so we can manipulate the original text twice
                    ContactNameAllText = ContactName.Trim()

                    'take the text in the contact name after the first space
                    ContactNameAllText = ContactNameAllText.Substring(ContactName.IndexOf(Space(1))).Trim()

                    'take the text in the contact name before the first space only
                    ContactName = ContactName.Substring(0, ContactName.IndexOf(Space(1))).Trim()

                    lstNames.Add(ContactName)
                    lstNames.Add(ContactNameAllText)

                End If
            Else
                'no space
                lstNames.Add(ContactName)
            End If

            If ContactName.Trim().Contains(Space(1)) Then
                lstNames = ContactName.Trim().Split(Space(1)).ToList()
            Else
                lstNames.Add(ContactName)
            End If

        Catch ex As Exception

        End Try

        Return lstNames
    End Function

    Private Function SaveContactNew(ByRef ContactRecord As RepeaterItem) As Boolean
        Dim oPersonAgent As PersonAgent = Nothing

        Dim oPerson As Person = Nothing
        Dim oContact As Person = Nothing
        Dim bRet As Boolean = False

        Dim ic As IntamacBL_SPR.IntamacContact = Nothing
        Dim lstContactNames As List(Of String) = Nothing
        Dim subLink As IntamacBL_SPR.SubscriptionLinkSPR = Nothing

        Try

            oPersonAgent = CType(IntamacBL_SPR.ObjectManager.CreatePersonAgent("SQL"), IntamacBL_SPR.PersonAgent)

            oPerson = CType(IntamacBL_SPR.ObjectManager.CreatePerson(IntamacShared_SPR.SharedStuff.e_CompanyType.SPR), IntamacBL_SPR.Person)
            oPerson.AccountID = mAccountID

            Dim txtEmail = CType(ContactRecord.FindControl("txtEmail"), TextBox)
            Dim txtContactName = CType(ContactRecord.FindControl("txtContactName"), TextBox)
            oPerson.Email = txtEmail.Text.Trim()

            lstContactNames = SplitContactName(txtContactName.Text.Trim())

            oPerson.FirstName = txtContactName.Text.Trim()
            oPerson.LastName = txtContactName.Text.Trim()


            oPerson.IsActive = True

            oPerson.MobileNo = CType(ContactRecord.FindControl("txtContactNumber"), TextBox).Text.Trim()
            oPerson.Organisation = CType(ContactRecord.FindControl("txtOrganisation"), TextBox).Text.Trim()
            oPerson.ReceivesEmail = True
            oPerson.ReceivesPush = True
            oPerson.PersonGUID = Guid.NewGuid
            oPerson.PersonGUID_date = Date.UtcNow
            oPerson.Terms_accepted_app = e_TermsAndConditionsStatus.NotAccepted
            oPerson.Terms_accepted_user = e_TermsAndConditionsStatus.NotRequired

            'Removed as not required
            'Dim chkPush As CheckBox = DirectCast(ContactRecord.FindControl("chkApps"), CheckBox)
            'If chkPush IsNot Nothing Then
            '    oPerson.ReceivesPush = chkPush.Checked
            'End If
            If Not String.IsNullOrEmpty(txtEmail.Text) And Not String.IsNullOrEmpty(txtContactName.Text) Then

                bRet = oPersonAgent.Insert(oPerson)

            End If


            If bRet Then
                ic = New IntamacBL_SPR.IntamacContact

                'need to save the contact with a subscriptionlinkid value that maps to the only subscriptionlink record the property should have
                subLink = New IntamacBL_SPR.SubscriptionLinkSPR

                ic.IntaSubscriptionLinkID = 0

                'load the subscription link record to get the id
                If subLink.Load(mPropertyID, 1) Then ic.IntaSubscriptionLinkID = subLink.IntaSubscriptionLinkID

                ic.ContactSeq = CType(ContactRecord.FindControl("lblContactNumber"), Label).Text.Trim()
                ic.PersonID = oPerson.PersonID

                Dim addedContacts = (From c In GetContactListSelections(ContactRecord) Where c.Item2 Select c.Item1).ToList()

                If addedContacts.Count = 0 Then
                    'no options selected, so add default contact list of id 0 (this allows contacts to still be linked to property level)
                    addedContacts.Add(SharedStuff.e_ContactList.DefaultList)
                End If

                Dim contactListAudit = String.Join("/", From c In addedContacts Select GetAuditString(c))
                For Each addedContact In addedContacts
                    ic.ContactListID = addedContact
                    ic.SaveAllProperties(mAccountID)    'add contact on all properties under the account
                Next

                miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Add_Alert_Contact, String.Format("{0} {1}, {2} - {3}", PageString("ContactListsText"), contactListAudit, oPerson.FirstName, oPerson.Email))
            Else

                DisplayValidationMessage(ContactRecord, oPersonAgent)

            End If

        Finally
            oPersonAgent = Nothing
            oPerson = Nothing
        End Try

        Return bRet
    End Function

    Private Function SaveContactChanges() As Boolean
        Dim mode As HiddenField = Nothing
        Dim lstContacts As List(Of IntamacBL_SPR.Contacts) = Nothing
        Dim bRet As Boolean = False
        Dim firstloop As Boolean = True
        Dim iContact As Integer = 1
        Dim lstContactsFailed As List(Of Integer) = Nothing

        Try

            If Not AllEntriesValid() Then
                HideAllMessages()
                divSaveFailed.Visible = True
                Return False
            End If

            lstContactsFailed = New List(Of Integer)

            'iterate through all records
            For Each ContactRecord As RepeaterItem In rptContacts.Items

                'confirm that its an item record
                If ContactRecord.ItemType = ListItemType.Item Or ListItemType.AlternatingItem Then

                    'get the mode for this record.  1=Existing Contact, 0=New Contact
                    mode = CType(ContactRecord.FindControl("hdnMode"), HiddenField)

                    If mode.Value = 0 Then
                        'New Contact
                        Dim txtEmail = CType(ContactRecord.FindControl("txtEmail"), TextBox)
                        Dim txtContactName = CType(ContactRecord.FindControl("txtContactName"), TextBox)
                        Dim txtOrganisation = CType(ContactRecord.FindControl("txtOrganisation"), TextBox)
                        Dim txtContactNumber = CType(ContactRecord.FindControl("txtContactNumber"), TextBox)

                        If Not String.IsNullOrEmpty(txtEmail.Text) And Not String.IsNullOrEmpty(txtContactName.Text) Then
                            'contact name and email have been filled in which can attempted to be saved
                            bRet = SaveContactNew(ContactRecord)
                            CType(ContactRecord.FindControl("hdnMode"), HiddenField).Value = 1
                        End If
                    Else

                        bRet = SaveContactExisting(ContactRecord)

                    End If

                    If Not bRet Then lstContactsFailed.Add(iContact)

                End If

                iContact = iContact + 1
            Next

            'rebind grid with the fresh data.  This confirms the DataArgument fields are set, such as ID fields.

            If lstContactsFailed.Count = 0 Then

                'Load all known Contacts 
                lstContacts = LoadKnownContacts()

                'display the identified contacts
                DisplayKnownContacts(lstContacts)

                HideAllMessages()
                divSaveSuccessful.Visible = True

            Else
                HideAllMessages()
                divSaveFailed.Visible = True
            End If

        Finally
            mode = Nothing
        End Try

        Return bRet
    End Function

    Private Function AllEntriesValid() As Boolean

        'iterate through all records
        For Each ContactRecord As RepeaterItem In rptContacts.Items

            'confirm that its an item record
            If ContactRecord.ItemType = ListItemType.Item Or ListItemType.AlternatingItem Then

                Dim txtEmail = CType(ContactRecord.FindControl("txtEmail"), TextBox)
                Dim txtContactName = CType(ContactRecord.FindControl("txtContactName"), TextBox)
                Dim txtOrganisation = CType(ContactRecord.FindControl("txtOrganisation"), TextBox)
                Dim txtContactNumber = CType(ContactRecord.FindControl("txtContactNumber"), TextBox)

                'check if any of the fields are invalid, if so then avoid any updates
                If ((Not String.IsNullOrEmpty(txtContactName.Text)) Or
                (Not String.IsNullOrEmpty(txtEmail.Text)) Or
                (Not String.IsNullOrEmpty(txtOrganisation.Text)) Or
                (Not String.IsNullOrEmpty(txtContactNumber.Text))) Then

                    'validate fields
                    Dim valContactName As RequiredFieldValidator = CType(ContactRecord.FindControl("valContactName"), RequiredFieldValidator)
                    Dim valEmail As RequiredFieldValidator = CType(ContactRecord.FindControl("valEmail"), RequiredFieldValidator)
                    Dim valEmailFormat As RegularExpressionValidator = CType(ContactRecord.FindControl("valEmailFormat"), RegularExpressionValidator)
                    Dim valContactNumberFormat As RegularExpressionValidator = CType(ContactRecord.FindControl("valContactNumberFormat"), RegularExpressionValidator)

                    If valContactName IsNot Nothing Then
                        valContactName.Validate()
                        If Not valContactName.IsValid() Then
                            Return False
                        End If
                    End If

                    If valEmail IsNot Nothing Then
                        valEmail.Validate()
                        If Not valEmail.IsValid() Then
                            Return False
                        End If
                    End If

                    If valEmailFormat IsNot Nothing Then
                        valEmailFormat.Validate()
                        If Not valEmailFormat.IsValid() Then
                            Return False
                        End If
                    End If

                    If valContactNumberFormat IsNot Nothing Then
                        valContactNumberFormat.Validate()
                        If Not valContactNumberFormat.IsValid() Then
                            Return False
                        End If
                    End If

                End If

            End If

        Next

        Return True

    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try

            If Page.IsValid AndAlso Page.IsPostBack AndAlso Not _isRefresh Then

                If SaveContactChanges() Then

                    ' refresh grid to correct mode on data item just inserted.  This will avoid inserted a duplicate item
                    DisplayKnownContacts(LoadKnownContacts())

                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAddContact_Click(sender As Object, e As EventArgs) Handles btnAddContact.Click
        Dim lstContacts As List(Of IntamacBL_SPR.Contacts) = Nothing
        Dim oContactToAdd As IntamacBL_SPR.Contacts = Nothing

        Try

            'Load all known Contacts 
            lstContacts = LoadKnownContacts()

            'add a blank contact record
            oContactToAdd = New IntamacBL_SPR.Contacts
            oContactToAdd.ContactNumber = lstContacts.Count + 1

            lstContacts.Add(oContactToAdd)

            'display the identified contacts, including the blank record
            DisplayKnownContacts(lstContacts)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub rptContacts_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptContacts.ItemDataBound
        If TypeOf e.Item Is RepeaterItem Then
            Dim hiddenMode As HiddenField = CType(e.Item.FindControl("hdnMode"), HiddenField)

            If CType(e.Item.DataItem, IntamacBL_SPR.Contacts) IsNot Nothing Then
                If CType(e.Item.DataItem, IntamacBL_SPR.Contacts).PersonID Is Nothing Then
                    hiddenMode.Value = 0 'new contact
                Else
                    hiddenMode.Value = 1 ' existing contact
                End If

                Dim divCommunityCheck As HtmlGenericControl = CType(e.Item.FindControl("divCommunityCheck"), HtmlGenericControl)

                'only display community checkboxes if risk level is allowed on account
                If mUsersCompany IsNot Nothing AndAlso _allowRiskLevels Then
                    divCommunityCheck.Visible = True
                Else
                    divCommunityCheck.Visible = False
                End If


                If String.IsNullOrEmpty(CType(e.Item.DataItem, IntamacBL_SPR.Contacts).Email) Then
                    Dim btnSend As Button = CType(e.Item.FindControl("btnSend"), Button)
                    Dim btnDel As Button = CType(e.Item.FindControl("btnDel"), Button)

                    If btnSend IsNot Nothing Then btnSend.Visible = False
                    If btnSend IsNot Nothing Then btnDel.Visible = False

                End If

            End If

        End If

    End Sub

    Private Sub rptContacts_ItemCreated(sender As Object, e As RepeaterItemEventArgs) Handles rptContacts.ItemCreated

        If e.Item.ItemType = ListItemType.Header Then
            'only display community header if risk level is allowed on account
            Dim divCommunityHeader As HtmlGenericControl = CType(e.Item.FindControl("divCommunityHeader"), HtmlGenericControl)

            If mUsersCompany IsNot Nothing AndAlso _allowRiskLevels Then
                divCommunityHeader.Visible = True
            Else
                divCommunityHeader.Visible = False
            End If

        End If

    End Sub

    'Check the Risk levels allowed or not
    Private Function AreRiskLevelsAllowed() As Boolean
        Dim company As MasterCompany = ObjectManager.CreateMasterCompany(SharedStuff.e_CompanyType.SPR)
        Return company.AreRiskLevelsAllowedForAccount(mAccountID)
    End Function

End Class