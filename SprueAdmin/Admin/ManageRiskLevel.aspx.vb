Imports IntamacBL_SPR
Imports Telerik.Web.UI
Imports IntamacShared_SPR.SharedStuff

Public Class ManageRiskLevel
    Inherits CultureBaseClass

#Region "New/Overrides"

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        'To show the popover after postback
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetHelpText", "SetHelpText();", True)

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        If Not Page.IsPostBack Then

            'Fill any dropdowns that use Contact List
            BindComboToContactList(cboFaultHighContactList, True)
            BindComboToContactList(cboFaultMediumContactList, True)
            BindComboToContactList(cboFaultLowContactList, True)
            BindComboToContactList(cboDeteriorationHighDaysContact, True)
            BindComboToContactList(cboDeteriorationMediumDaysContact, True)
            BindComboToContactList(cboDeteriorationLowDaysContact, True)
            BindComboToContactList(cboDeteriorationHighMinutesContact, True)
            BindComboToContactList(cboDeteriorationMediumMinutesContact, True)
            BindComboToContactList(cboDeteriorationLowMinutesContact, True)

            BindComboToContactList(cboHighContactList, True)
            BindComboToContactList(cboMediumContactList, True)
            BindComboToContactList(cboLowContactList, True)
            BindComboToContactList(cbo1stAlarmHighSilencedContactList, True)
            BindComboToContactList(cbo1stAlarmHighNotSilencedContactList, True)
            BindComboToContactList(cbo1stAlarmMediumSilencedContactList, True)
            BindComboToContactList(cbo1stAlarmMediumNotSilencedContactList, True)
            BindComboToContactList(cbo1stAlarmLowSilencedContactList, True)
            BindComboToContactList(cbo1stAlarmLowNotSilencedContactList, True)

            BindComboToContactList(cbo2ndAlarmHighThresholdYesContactList, True)
            BindComboToContactList(cbo2ndAlarmHighThresholdNoContactList, True)
            BindComboToContactList(cbo2ndAlarmMediumThresholdYesContactList, True)
            BindComboToContactList(cbo2ndAlarmMediumThresholdNoContactList, True)
            BindComboToContactList(cbo2ndAlarmLowThresholdYesContactList, True)
            BindComboToContactList(cbo2ndAlarmLowThresholdNoContactList, True)

            BindComboToContactList(cboLossOfCommsHighContactList, True)
            BindComboToContactList(cboLossOfCommsMediumContactList, True)
            BindComboToContactList(cboLossOfCommsLowContactList, True)

            'Fill any dropdowns that use numbers
            FillComboWithNumbers(cboDeteriorationHighDaysAlarmEvents, 1, 10)
            FillComboWithNumbers(cboDeteriorationHighMinutesAlarmEvents, 1, 10)
            FillComboWithNumbers(cboDeteriorationMediumDaysAlarmEvents, 1, 10)
            FillComboWithNumbers(cboDeteriorationMediumMinutesAlarmEvents, 1, 10)
            FillComboWithNumbers(cboDeteriorationLowDaysAlarmEvents, 1, 10)
            FillComboWithNumbers(cboDeteriorationLowMinutesAlarmEvents, 1, 10)
            FillComboWithNumbers(cboDeteriorationHighDaysValue, 1, 99)
            FillComboWithNumbers(cboDeteriorationMediumDaysValue, 1, 99)
            FillComboWithNumbers(cboDeteriorationLowDaysValue, 1, 99)
            FillComboWithNumbers(cboDeteriorationHighMinutesValue, 1, 99)
            FillComboWithNumbers(cboDeteriorationMediumMinutesValue, 1, 99)
            FillComboWithNumbers(cboDeteriorationLowMinutesValue, 1, 99)

            FillComboWithNumbers(cbo1stAlarmHighSilencedValue, 1, 99)
            FillComboWithNumbers(cbo1stAlarmMediumSilencedValue, 1, 99)
            FillComboWithNumbers(cbo1stAlarmLowSilencedValue, 1, 99)

            FillComboWithNumbers(cbo2ndAlarmHighThresholdValue, 1, 99)
            FillComboWithNumbers(cbo2ndAlarmMediumThresholdValue, 1, 99)
            FillComboWithNumbers(cbo2ndAlarmLowThresholdValue, 1, 99)

            'Load the Distribuitors dropdown
            LoadDistributors()

            'Load current settings for each section
            PopulatePage()

            If IsTopNavigate Then
                mBtnBack.Visible = False
            End If

        End If

    End Sub



#End Region

    Private Sub PopulatePage()

        'Populate the Fault Conditions section
        LoadFaultConditions()

        'Poplulate the Deterioration Levels section
        LoadDeteriorationLevels()

        'Populate the 1st and 2nd Alarms section
        Load1stand2ndAlarmLevels()

        'Populate the Loss of Comms section
        LoadLossofComms()

    End Sub

    ''' <summary>
    ''' Sub to populate a Telerik Combo box with available Contact Lists
    ''' </summary>
    ''' <param name="objCombo">The name of the combo box</param>
    ''' <param name="boolIncludeNoFurtherAction">boolean value to indicate whether to include 'No Further Action' in the list of items</param>
    ''' <remarks></remarks>
    Private Sub BindComboToContactList(objCombo As RadComboBox, boolIncludeNoFurtherAction As Boolean)
        objCombo.DataSource = GetContactList(boolIncludeNoFurtherAction)
        objCombo.DataBind()
        objCombo.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Sub to populate Telerik Combo Box with a list of numbers between a passed range
    ''' </summary>
    ''' <param name="objCombo">The name of the combo box</param>
    ''' <param name="StartingNumber">The start number required in the list</param>
    ''' <param name="EndNumber">The ending number required in the list</param>
    ''' <remarks></remarks>
    Private Sub FillComboWithNumbers(objCombo As RadComboBox, StartingNumber As Integer, EndNumber As Integer)
        For intCount As Integer = StartingNumber To EndNumber
            objCombo.Items.Add(New RadComboBoxItem(intCount, intCount))
        Next
        objCombo.SelectedIndex = 0


    End Sub

    ''' <summary>
    ''' Function to get a list of Contacts used in dropdown lists
    ''' </summary>
    ''' <param name="boolIncludeNoFurtherAction">boolean value to indicate whether to include 'No Further Action' in the list of items</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetContactList(boolIncludeNoFurtherAction As Boolean) As List(Of ContactList)

        Dim lstContactList As New List(Of ContactList)

        'Add 'No Further Action' to list dependent on passed variable
        If boolIncludeNoFurtherAction Then
            lstContactList.Add(New ContactList With {.ContactListID = 0, .Description = GetLocalResourceObject("NoFurtherAction")})
        End If

        'Loop through each enum in e_ContactList
        For Each thisContactList As e_ContactList In [Enum].GetValues(GetType(e_ContactList))

            If thisContactList <> e_ContactList.DefaultList Then 'SP-915 we don't want to display (Defaultlist=0) in the dropdown , if it is display then empty item added to the Dropdown
                lstContactList.Add(New ContactList With {.ContactListID = thisContactList, .Description = GetLocalResourceObject(thisContactList.ToString())})
            End If

        Next


        Return lstContactList

    End Function

    ''' <summary>
    ''' Load Distributors for dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadDistributors()
        If mUsersCompany IsNot Nothing Then

            Dim objMasterCompany As MasterCompany = ObjectManager.CreateMasterCompany(e_CompanyType.SPR)

            Select Case mUsersCompany.CompanyTypeID

                Case e_MasterCompanyTypes.ApplicationOwner, e_MasterCompanyTypes.SystemOwner


                    cboDistributors.DataSource = objMasterCompany.LoadSearch("", 0, 0, e_MasterCompanyTypes.Distributor, True)
                    cboDistributors.DataBind()

                    cboDistributors.SelectedIndex = 0

                Case e_MasterCompanyTypes.Distributor


                    cboDistributors.DataSource = objMasterCompany.LoadSearch("", 0, mUsersCompany.MasterCoID, e_MasterCompanyTypes.Distributor, True)
                    cboDistributors.DataBind()

                    cboDistributors.SelectedIndex = 0

                    cboDistributors.Visible = False

                Case Else

                    Response.Redirect("ErrorPage.aspx")

            End Select

        Else
            Response.Redirect("ErrorPage.aspx")
        End If
    End Sub

    ''' <summary>
    ''' Load Fault Conditions data from platfrom and populate page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadFaultConditions()
        If cboDistributors.SelectedValue IsNot Nothing Then
            Dim objFaultConditions As New FaultConditions
            Dim objAllFaultConditions As List(Of FaultConditions) = objFaultConditions.Load(cboDistributors.SelectedValue, e_EventCategory.Fault)

            For Each thisFaultCondition As FaultConditions In objAllFaultConditions

                Select Case thisFaultCondition.FK_RiskLevelID

                    Case e_RiskLevel.High
                        cboFaultHighContactList.SelectedValue = thisFaultCondition.FK_ContactListID
                    Case e_RiskLevel.Medium
                        cboFaultMediumContactList.SelectedValue = thisFaultCondition.FK_ContactListID
                    Case e_RiskLevel.Low
                        cboFaultLowContactList.SelectedValue = thisFaultCondition.FK_ContactListID
                End Select


            Next

        End If
    End Sub

    ''' <summary>
    ''' Load Loss of Comms data from platfrom and popluate page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadLossofComms()
        If cboDistributors.SelectedValue IsNot Nothing Then
            Dim objFaultConditions As New FaultConditions
            Dim objAllLossofComms As List(Of FaultConditions) = objFaultConditions.Load(cboDistributors.SelectedValue, e_EventCategory.LossofComms)

            For Each thisLossofComms As FaultConditions In objAllLossofComms

                Select Case thisLossofComms.FK_RiskLevelID

                    Case e_RiskLevel.High
                        cboLossOfCommsHighContactList.SelectedValue = thisLossofComms.FK_ContactListID
                    Case e_RiskLevel.Medium
                        cboLossOfCommsMediumContactList.SelectedValue = thisLossofComms.FK_ContactListID
                    Case e_RiskLevel.Low
                        cboLossOfCommsLowContactList.SelectedValue = thisLossofComms.FK_ContactListID
                End Select


            Next

        End If
    End Sub

    ''' <summary>
    ''' Load Deterioration Levels data from platfrom and populate page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadDeteriorationLevels()
        If cboDistributors.SelectedValue IsNot Nothing Then
            Dim objRiskLevelDeteriorationConditions As New RiskLevelDeteriorationConditions
            Dim objAllRiskLevelDeteriorationConditions As List(Of RiskLevelDeteriorationConditions) = objRiskLevelDeteriorationConditions.Load(cboDistributors.SelectedValue)

            For Each thisRiskLevelDeteriorationCondition As RiskLevelDeteriorationConditions In objAllRiskLevelDeteriorationConditions

                'If the Threshold minutes is greater or equal to 1440 minutes (i.e a day) then it must be day setting
                If thisRiskLevelDeteriorationCondition.ThresholdMinutes >= 1440 Then

                    Select Case thisRiskLevelDeteriorationCondition.FK_RiskLevelID

                        Case e_RiskLevel.High
                            SetComboValue(cboDeteriorationHighDaysAlarmEvents, thisRiskLevelDeteriorationCondition.AlarmEventVolume)
                            SetComboValue(cboDeteriorationHighDaysContact, thisRiskLevelDeteriorationCondition.FK_ContactListID)
                            SetComboValue(cboDeteriorationHighDaysValue, thisRiskLevelDeteriorationCondition.ThresholdMinutes / 1440)
                        Case e_RiskLevel.Medium
                            SetComboValue(cboDeteriorationMediumDaysAlarmEvents, thisRiskLevelDeteriorationCondition.AlarmEventVolume)
                            SetComboValue(cboDeteriorationMediumDaysContact, thisRiskLevelDeteriorationCondition.FK_ContactListID)
                            SetComboValue(cboDeteriorationMediumDaysValue, thisRiskLevelDeteriorationCondition.ThresholdMinutes / 1440)
                        Case e_RiskLevel.Low
                            SetComboValue(cboDeteriorationLowDaysAlarmEvents, thisRiskLevelDeteriorationCondition.AlarmEventVolume)
                            SetComboValue(cboDeteriorationLowDaysContact, thisRiskLevelDeteriorationCondition.FK_ContactListID)
                            SetComboValue(cboDeteriorationLowDaysValue, thisRiskLevelDeteriorationCondition.ThresholdMinutes / 1440)

                    End Select

                Else

                    Select Case thisRiskLevelDeteriorationCondition.FK_RiskLevelID

                        Case e_RiskLevel.High
                            SetComboValue(cboDeteriorationHighMinutesAlarmEvents, thisRiskLevelDeteriorationCondition.AlarmEventVolume)
                            SetComboValue(cboDeteriorationHighMinutesContact, thisRiskLevelDeteriorationCondition.FK_ContactListID)
                            SetComboValue(cboDeteriorationHighMinutesValue, thisRiskLevelDeteriorationCondition.ThresholdMinutes)
                        Case e_RiskLevel.Medium
                            SetComboValue(cboDeteriorationMediumMinutesAlarmEvents, thisRiskLevelDeteriorationCondition.AlarmEventVolume)
                            SetComboValue(cboDeteriorationMediumMinutesContact, thisRiskLevelDeteriorationCondition.FK_ContactListID)
                            SetComboValue(cboDeteriorationMediumMinutesValue, thisRiskLevelDeteriorationCondition.ThresholdMinutes)
                        Case e_RiskLevel.Low
                            SetComboValue(cboDeteriorationLowMinutesAlarmEvents, thisRiskLevelDeteriorationCondition.AlarmEventVolume)
                            SetComboValue(cboDeteriorationLowMinutesContact, thisRiskLevelDeteriorationCondition.FK_ContactListID)
                            SetComboValue(cboDeteriorationLowMinutesValue, thisRiskLevelDeteriorationCondition.ThresholdMinutes)

                    End Select

                End If

            Next

        End If

    End Sub

    ''' <summary>
    ''' Load 1st and 2nd Level Alarm data from platform and populate page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Load1stand2ndAlarmLevels()
        If cboDistributors.SelectedValue IsNot Nothing Then
            Dim objRiskLevelAlertConditions As New RiskLevelAlertConditions
            Dim objAllRiskLevelAlertConditions As List(Of RiskLevelAlertConditions) = objRiskLevelAlertConditions.Load(cboDistributors.SelectedValue)

            For Each thisRiskLevelAlertCondition As RiskLevelAlertConditions In objAllRiskLevelAlertConditions

                With thisRiskLevelAlertCondition

                    Select Case .FK_RiskLevelID

                        Case e_RiskLevel.High



                            Select Case .AlarmEventVolume

                                Case 0

                                    SetComboValue(cboHighContactList, .FK_ContactListID)

                                Case 1

                                    SetComboValue(cbo1stAlarmHighSilencedValue, .ThresholdMinutes)

                                    If .Occurred = True Then
                                        SetComboValue(cbo1stAlarmHighSilencedContactList, .FK_ContactListID)
                                    Else
                                        SetComboValue(cbo1stAlarmHighNotSilencedContactList, .FK_ContactListID)
                                    End If

                                Case 2

                                    SetComboValue(cbo2ndAlarmHighThresholdValue, .ThresholdMinutes)

                                    If .Occurred = True Then
                                        SetComboValue(cbo2ndAlarmHighThresholdYesContactList, .FK_ContactListID)
                                    Else
                                        SetComboValue(cbo2ndAlarmHighThresholdNoContactList, .FK_ContactListID)
                                    End If


                            End Select


                        Case e_RiskLevel.Medium



                            Select Case .AlarmEventVolume

                                Case 0

                                    SetComboValue(cboMediumContactList, .FK_ContactListID)

                                Case 1

                                    SetComboValue(cbo1stAlarmMediumSilencedValue, .ThresholdMinutes)

                                    If .Occurred = True Then
                                        SetComboValue(cbo1stAlarmMediumSilencedContactList, .FK_ContactListID)
                                    Else
                                        SetComboValue(cbo1stAlarmMediumNotSilencedContactList, .FK_ContactListID)
                                    End If

                                Case 2

                                    SetComboValue(cbo2ndAlarmMediumThresholdValue, .ThresholdMinutes)

                                    If .Occurred = True Then
                                        SetComboValue(cbo2ndAlarmMediumThresholdYesContactList, .FK_ContactListID)
                                    Else
                                        SetComboValue(cbo2ndAlarmMediumThresholdNoContactList, .FK_ContactListID)
                                    End If


                            End Select
                        Case e_RiskLevel.Low


                            Select Case .AlarmEventVolume

                                Case 0

                                    SetComboValue(cboLowContactList, .FK_ContactListID)

                                Case 1

                                    SetComboValue(cbo1stAlarmLowSilencedValue, .ThresholdMinutes)

                                    If .Occurred = True Then
                                        SetComboValue(cbo1stAlarmLowSilencedContactList, .FK_ContactListID)
                                    Else
                                        SetComboValue(cbo1stAlarmLowNotSilencedContactList, .FK_ContactListID)
                                    End If

                                Case 2

                                    SetComboValue(cbo2ndAlarmLowThresholdValue, .ThresholdMinutes)

                                    If .Occurred = True Then
                                        SetComboValue(cbo2ndAlarmLowThresholdYesContactList, .FK_ContactListID)
                                    Else
                                        SetComboValue(cbo2ndAlarmLowThresholdNoContactList, .FK_ContactListID)
                                    End If


                            End Select

                    End Select

                End With

            Next

        End If

    End Sub


    ''' <summary>
    ''' Set Telerik Combo Value 
    ''' </summary>
    ''' <param name="objCombo">Name of Combo box</param>
    ''' <param name="SelectedValue">Required value to be set as selectedvalue of combo</param>
    ''' <remarks></remarks>
    Private Sub SetComboValue(objCombo As RadComboBox, SelectedValue As Integer)
        objCombo.SelectedValue = SelectedValue
    End Sub

    ''' <summary>
    ''' Reload the page when Distributor selection is change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboDistributors_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cboDistributors.SelectedIndexChanged
        ResetDropdownSelection()
        PopulatePage()
    End Sub

    ''' <summary>
    ''' Reset dropdownselection to default values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetDropdownSelection()

        cboFaultHighContactList.SelectedIndex = 0
        cboFaultMediumContactList.SelectedIndex = 0
        cboFaultLowContactList.SelectedIndex = 0
        cboDeteriorationHighDaysContact.SelectedIndex = 0
        cboDeteriorationMediumDaysContact.SelectedIndex = 0
        cboDeteriorationLowDaysContact.SelectedIndex = 0
        cboDeteriorationHighMinutesContact.SelectedIndex = 0
        cboDeteriorationMediumMinutesContact.SelectedIndex = 0
        cboDeteriorationLowMinutesContact.SelectedIndex = 0

        cboHighContactList.SelectedIndex = 0
        cboMediumContactList.SelectedIndex = 0
        cboLowContactList.SelectedIndex = 0
        cbo1stAlarmHighSilencedContactList.SelectedIndex = 0
        cbo1stAlarmHighNotSilencedContactList.SelectedIndex = 0
        cbo1stAlarmMediumSilencedContactList.SelectedIndex = 0
        cbo1stAlarmMediumNotSilencedContactList.SelectedIndex = 0
        cbo1stAlarmLowSilencedContactList.SelectedIndex = 0
        cbo1stAlarmLowNotSilencedContactList.SelectedIndex = 0

        cbo2ndAlarmHighThresholdYesContactList.SelectedIndex = 0
        cbo2ndAlarmHighThresholdNoContactList.SelectedIndex = 0
        cbo2ndAlarmMediumThresholdYesContactList.SelectedIndex = 0
        cbo2ndAlarmMediumThresholdNoContactList.SelectedIndex = 0
        cbo2ndAlarmLowThresholdYesContactList.SelectedIndex = 0
        cbo2ndAlarmLowThresholdNoContactList.SelectedIndex = 0

        cboLossOfCommsHighContactList.SelectedIndex = 0
        cboLossOfCommsMediumContactList.SelectedIndex = 0
        cboLossOfCommsLowContactList.SelectedIndex = 0

        cboDeteriorationHighDaysAlarmEvents.SelectedIndex = 0
        cboDeteriorationHighMinutesAlarmEvents.SelectedIndex = 0
        cboDeteriorationMediumDaysAlarmEvents.SelectedIndex = 0
        cboDeteriorationMediumMinutesAlarmEvents.SelectedIndex = 0
        cboDeteriorationLowDaysAlarmEvents.SelectedIndex = 0
        cboDeteriorationLowMinutesAlarmEvents.SelectedIndex = 0
        cboDeteriorationHighDaysValue.SelectedIndex = 0
        cboDeteriorationMediumDaysValue.SelectedIndex = 0
        cboDeteriorationLowDaysValue.SelectedIndex = 0
        cboDeteriorationHighMinutesValue.SelectedIndex = 0
        cboDeteriorationMediumMinutesValue.SelectedIndex = 0
        cboDeteriorationLowMinutesValue.SelectedIndex = 0

        cbo1stAlarmHighSilencedValue.SelectedIndex = 0
        cbo1stAlarmMediumSilencedValue.SelectedIndex = 0
        cbo1stAlarmLowSilencedValue.SelectedIndex = 0

        cbo2ndAlarmHighThresholdValue.SelectedIndex = 0
        cbo2ndAlarmMediumThresholdValue.SelectedIndex = 0
        cbo2ndAlarmLowThresholdValue.SelectedIndex = 0

    End Sub

    ''' <summary>
    ''' Save Fault Conditions 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveFaultCondition_Click(sender As Object, e As EventArgs) Handles btnSaveFaultCondition.Click
        Dim objFaultCondition As New FaultConditions

        For Each thisRiskLevel As e_RiskLevel In [Enum].GetValues(GetType(e_RiskLevel))

            Select Case thisRiskLevel

                Case e_RiskLevel.High

                    objFaultCondition.Save(cboDistributors.SelectedValue, thisRiskLevel, cboFaultHighContactList.SelectedValue, e_EventCategory.Fault)

                Case e_RiskLevel.Medium

                    objFaultCondition.Save(cboDistributors.SelectedValue, thisRiskLevel, cboFaultMediumContactList.SelectedValue, e_EventCategory.Fault)

                Case e_RiskLevel.Low

                    objFaultCondition.Save(cboDistributors.SelectedValue, thisRiskLevel, cboFaultLowContactList.SelectedValue, e_EventCategory.Fault)

            End Select

        Next
        'Code starting for the current selected div to collapse in 
        If collapseFaultCondition.Attributes("class").Contains("collapse in") Then
            collapseFaultCondition.Attributes("class") = collapseFaultCondition.Attributes("class").Replace("collapse in", " collapse in")
        ElseIf collapseFaultCondition.Attributes("class").Contains("collapse in") Then
            collapseFaultCondition.Attributes("class") = collapseFaultCondition.Attributes("class").Replace("collapse out", " collapse in")
        Else

            collapseFaultCondition.Attributes("class") = collapseFaultCondition.Attributes("class") & " collapse in"
        End If

        lblFaultConditionUpdateStatus.Text = GetLocalResourceObject("SettingsUpdated")
        btnFaultConditionOpenClose.Text = GetLocalResourceObject("Close")
        'Code ends

        'Code starting for the remaining div to collapse out

        collapse1stAlarm.Attributes("class") = collapse1stAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn1stAlarmOpenClose.Text = GetLocalResourceObject("Open")

        collapse2ndAlarm.Attributes("class") = collapse2ndAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn2ndAlarmOpenClose.Text = GetLocalResourceObject("Open")

        collapseLossOfComms.Attributes("class") = collapseLossOfComms.Attributes("class").Replace("collapse in", " collapse out")
        btnLossOfCommsOpenClose.Text = GetLocalResourceObject("Open")

        collapseDeteriorationMonitor.Attributes("class") = collapseDeteriorationMonitor.Attributes("class").Replace("collapse in", " collapse out")
        btnDeteriorationMonitorOpenClose.Text = GetLocalResourceObject("Open")
        'Code ends

    End Sub

    ''' <summary>
    ''' Save Deterioration Levels
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveDeteriorationLevels_Click(sender As Object, e As EventArgs) Handles btnSaveDeteriorationLevels.Click
        Dim objDeteriorationConditions As New RiskLevelDeteriorationConditions

        For Each thisRiskLevel As e_RiskLevel In [Enum].GetValues(GetType(e_RiskLevel))

            Select Case thisRiskLevel

                Case e_RiskLevel.High

                    objDeteriorationConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, cboDeteriorationHighDaysAlarmEvents.SelectedValue, cboDeteriorationHighDaysValue.SelectedValue * 1440, cboDeteriorationHighDaysContact.SelectedValue)
                    objDeteriorationConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, cboDeteriorationHighMinutesAlarmEvents.SelectedValue, cboDeteriorationHighMinutesValue.SelectedValue, cboDeteriorationHighMinutesContact.SelectedValue)

                Case e_RiskLevel.Medium

                    objDeteriorationConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, cboDeteriorationMediumDaysAlarmEvents.SelectedValue, cboDeteriorationMediumDaysValue.SelectedValue * 1440, cboDeteriorationMediumDaysContact.SelectedValue)
                    objDeteriorationConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, cboDeteriorationMediumMinutesAlarmEvents.SelectedValue, cboDeteriorationMediumMinutesValue.SelectedValue, cboDeteriorationMediumMinutesContact.SelectedValue)

                Case e_RiskLevel.Low

                    objDeteriorationConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, cboDeteriorationLowDaysAlarmEvents.SelectedValue, cboDeteriorationLowDaysValue.SelectedValue * 1440, cboDeteriorationLowDaysContact.SelectedValue)
                    objDeteriorationConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, cboDeteriorationLowMinutesAlarmEvents.SelectedValue, cboDeteriorationLowMinutesValue.SelectedValue, cboDeteriorationLowMinutesContact.SelectedValue)

            End Select

        Next
        'Code starting for the current  div to collapse in 
        If collapseDeteriorationMonitor.Attributes("class").Contains("collapse in") Then
            collapseDeteriorationMonitor.Attributes("class") = collapseDeteriorationMonitor.Attributes("class").Replace("collapse in", " collapse in")
        ElseIf collapseDeteriorationMonitor.Attributes("class").Contains("collapse out") Then
            collapseDeteriorationMonitor.Attributes("class") = collapseDeteriorationMonitor.Attributes("class").Replace("collapse out", " collapse in")
        Else
            collapseDeteriorationMonitor.Attributes("class") = collapseDeteriorationMonitor.Attributes("class") & " collapse in"
        End If

        lblDeteriorationStatus.Text = GetLocalResourceObject("SettingsUpdated")
        btnDeteriorationMonitorOpenClose.Text = GetLocalResourceObject("Close")
        'code ends

        'Code starting for the remaining div to collapse out
        collapse1stAlarm.Attributes("class") = collapse1stAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn1stAlarmOpenClose.Text = GetLocalResourceObject("Open")

        collapse2ndAlarm.Attributes("class") = collapse2ndAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn2ndAlarmOpenClose.Text = GetLocalResourceObject("Open")

        collapseLossOfComms.Attributes("class") = collapseLossOfComms.Attributes("class").Replace("collapse in", " collapse out")
        btnLossOfCommsOpenClose.Text = GetLocalResourceObject("Open")

        collapseFaultCondition.Attributes("class") = collapseFaultCondition.Attributes("class").Replace("collapse in", " collapse out")
        btnFaultConditionOpenClose.Text = GetLocalResourceObject("Open")
        'code ends


    End Sub

    ''' <summary>
    ''' Save 1st Alarm Conditions
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSave1stAlarmCondition_Click(sender As Object, e As EventArgs) Handles btnSave1stAlarmCondition.Click

        Dim objAlertConditions As New RiskLevelAlertConditions

        For Each thisRiskLevel As e_RiskLevel In [Enum].GetValues(GetType(e_RiskLevel))

            Select Case thisRiskLevel

                Case e_RiskLevel.High

                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 0, 0, cboHighContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 1, cbo1stAlarmHighSilencedValue.SelectedValue, cbo1stAlarmHighSilencedContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 1, cbo1stAlarmHighSilencedValue.SelectedValue, cbo1stAlarmHighNotSilencedContactList.SelectedValue, False)

                Case e_RiskLevel.Medium

                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 0, 0, cboMediumContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 1, cbo1stAlarmMediumSilencedValue.SelectedValue, cbo1stAlarmMediumSilencedContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 1, cbo1stAlarmMediumSilencedValue.SelectedValue, cbo1stAlarmMediumNotSilencedContactList.SelectedValue, False)
                Case e_RiskLevel.Low

                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 0, 0, cboLowContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 1, cbo1stAlarmLowSilencedValue.SelectedValue, cbo1stAlarmLowSilencedContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 1, cbo1stAlarmLowSilencedValue.SelectedValue, cbo1stAlarmLowNotSilencedContactList.SelectedValue, False)

            End Select

        Next
        'Code starting for the current div to collapse in 
        If collapse1stAlarm.Attributes("class").Contains("collapse in") Then
            collapse1stAlarm.Attributes("class") = collapse1stAlarm.Attributes("class").Replace("collapse in", " collapse in")
        ElseIf collapse1stAlarm.Attributes("class").Contains("collapse out") Then
            collapse1stAlarm.Attributes("class") = collapse1stAlarm.Attributes("class").Replace("collapse out", " collapse in")
        Else
            collapse1stAlarm.Attributes("class") = collapse1stAlarm.Attributes("class") & " collapse in"
        End If

        lbl1stAlarmUpdateStatus.Text = GetLocalResourceObject("SettingsUpdated")
        btn1stAlarmOpenClose.Text = GetLocalResourceObject("Close")
        'Code ends

        'Code starting for the remaining div to collapse out
        collapseDeteriorationMonitor.Attributes("class") = collapseDeteriorationMonitor.Attributes("class").Replace("collapse in", " collapse out")
        btnDeteriorationMonitorOpenClose.Text = GetLocalResourceObject("Open")

        collapse2ndAlarm.Attributes("class") = collapse2ndAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn2ndAlarmOpenClose.Text = GetLocalResourceObject("Open")

        collapseLossOfComms.Attributes("class") = collapseLossOfComms.Attributes("class").Replace("collapse in", " collapse out")
        btnLossOfCommsOpenClose.Text = GetLocalResourceObject("Open")

        collapseFaultCondition.Attributes("class") = collapseFaultCondition.Attributes("class").Replace("collapse in", " collapse out")
        btnFaultConditionOpenClose.Text = GetLocalResourceObject("Open")
        'Code ends

    End Sub

    ''' <summary>
    ''' Save 2nd Alarm conditions
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSave2ndAlarmCondition_Click(sender As Object, e As EventArgs) Handles btnSave2ndAlarmCondition.Click

        Dim objAlertConditions As New RiskLevelAlertConditions

        For Each thisRiskLevel As e_RiskLevel In [Enum].GetValues(GetType(e_RiskLevel))

            Select Case thisRiskLevel

                Case e_RiskLevel.High

                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 2, cbo2ndAlarmHighThresholdValue.SelectedValue, cbo2ndAlarmHighThresholdYesContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 2, cbo2ndAlarmHighThresholdValue.SelectedValue, cbo2ndAlarmHighThresholdNoContactList.SelectedValue, False)

                Case e_RiskLevel.Medium

                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 2, cbo2ndAlarmMediumThresholdValue.SelectedValue, cbo2ndAlarmMediumThresholdYesContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 2, cbo2ndAlarmMediumThresholdValue.SelectedValue, cbo2ndAlarmMediumThresholdNoContactList.SelectedValue, False)
                Case e_RiskLevel.Low

                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 2, cbo2ndAlarmLowThresholdValue.SelectedValue, cbo2ndAlarmLowThresholdYesContactList.SelectedValue, True)
                    objAlertConditions.Save(cboDistributors.SelectedValue, thisRiskLevel, 2, cbo2ndAlarmLowThresholdValue.SelectedValue, cbo2ndAlarmLowThresholdNoContactList.SelectedValue, False)

            End Select

        Next

        'Code starting for the current selected div to collapse in 
        If collapse2ndAlarm.Attributes("class").Contains("collapse in") Then
            collapse2ndAlarm.Attributes("class") = collapse2ndAlarm.Attributes("class").Replace("collapse in", " collapse in")
        ElseIf collapse2ndAlarm.Attributes("class").Contains("collapse out") Then
            collapse2ndAlarm.Attributes("class") = collapse2ndAlarm.Attributes("class").Replace("collapse out", " collapse in")
        Else

            collapse2ndAlarm.Attributes("class") = collapse2ndAlarm.Attributes("class") & " collapse in"
        End If

        lbl2ndAlarmUpdateStatus.Text = GetLocalResourceObject("SettingsUpdated")
        btn2ndAlarmOpenClose.Text = GetLocalResourceObject("Close")
        'Code ends

        'Code starting for the remaining div to collapse out

        collapse1stAlarm.Attributes("class") = collapse1stAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn1stAlarmOpenClose.Text = GetLocalResourceObject("Open")


        collapseFaultCondition.Attributes("class") = collapseFaultCondition.Attributes("class").Replace("collapse in", " collapse out")
        btnFaultConditionOpenClose.Text = GetLocalResourceObject("Open")

        collapseLossOfComms.Attributes("class") = collapseLossOfComms.Attributes("class").Replace("collapse in", " collapse out")
        btnLossOfCommsOpenClose.Text = GetLocalResourceObject("Open")

        collapseDeteriorationMonitor.Attributes("class") = collapseDeteriorationMonitor.Attributes("class").Replace("collapse in", " collapse out")
        btnDeteriorationMonitorOpenClose.Text = GetLocalResourceObject("Open")
        'Code ends


    End Sub

    ''' <summary>
    ''' Save Loss of Comms
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveLossofComms_Click(sender As Object, e As EventArgs) Handles btnSaveLossofComms.Click
        Dim objFaultCondition As New FaultConditions

        For Each thisRiskLevel As e_RiskLevel In [Enum].GetValues(GetType(e_RiskLevel))

            Select Case thisRiskLevel

                Case e_RiskLevel.High

                    objFaultCondition.Save(cboDistributors.SelectedValue, thisRiskLevel, cboLossOfCommsHighContactList.SelectedValue, e_EventCategory.LossofComms)

                Case e_RiskLevel.Medium

                    objFaultCondition.Save(cboDistributors.SelectedValue, thisRiskLevel, cboLossOfCommsMediumContactList.SelectedValue, e_EventCategory.LossofComms)

                Case e_RiskLevel.Low

                    objFaultCondition.Save(cboDistributors.SelectedValue, thisRiskLevel, cboLossOfCommsLowContactList.SelectedValue, e_EventCategory.LossofComms)

            End Select

        Next
        'Code starting for the current selected div to collapse in 

        If collapseLossOfComms.Attributes("class").Contains("collapse in") Then
            collapseLossOfComms.Attributes("class") = collapseLossOfComms.Attributes("class").Replace("collapse in", " collapse in")
        ElseIf collapseLossOfComms.Attributes("class").Contains("collapse out") Then
            collapseLossOfComms.Attributes("class") = collapseLossOfComms.Attributes("class").Replace("collapse out", " collapse in")
        Else

            collapseLossOfComms.Attributes("class") = collapseLossOfComms.Attributes("class") & " collapse in"
        End If

        lblLossOfCommsUpdateStatus.Text = GetLocalResourceObject("SettingsUpdated")
        btnLossOfCommsOpenClose.Text = GetLocalResourceObject("Close")
        'Code ends


        'Code starting for the remaining div to collapse out
        collapse1stAlarm.Attributes("class") = collapse1stAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn1stAlarmOpenClose.Text = GetLocalResourceObject("Open")


        collapseFaultCondition.Attributes("class") = collapseFaultCondition.Attributes("class").Replace("collapse in", " collapse out")
        btnFaultConditionOpenClose.Text = GetLocalResourceObject("Open")

        collapse2ndAlarm.Attributes("class") = collapse2ndAlarm.Attributes("class").Replace("collapse in", " collapse out")
        btn2ndAlarmOpenClose.Text = GetLocalResourceObject("Open")

        collapseDeteriorationMonitor.Attributes("class") = collapseDeteriorationMonitor.Attributes("class").Replace("collapse in", " collapse out")
        btnDeteriorationMonitorOpenClose.Text = GetLocalResourceObject("Open")
        'Code ends
    End Sub






End Class