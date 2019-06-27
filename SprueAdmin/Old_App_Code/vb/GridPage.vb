
Imports Telerik.Web.UI

Public MustInherit Class GridPage
    Inherits CultureBaseClass
    Implements IGridPage

#Region "Constants"

    Protected Const cSessionSaveDetailsKey As String = "EventLog"
    Protected Const cSavePageIndexKey As String = "PageIndex"
    Protected Const cSavePageSizeKey As String = "PageSize"


#End Region

    Protected ReadOnly Property ajaxManager As RadAjaxManager Implements IGridPage.ajaxManager
        Get
            Return RadAjaxManager.GetCurrent(Me)
        End Get
    End Property

    Private _savedDetails As NameValueCollection

    Protected ReadOnly Property savedDetails As NameValueCollection
        Get
            If IsNothing(_savedDetails) Then
                _savedDetails = CType(Session(cSessionSaveDetailsKey), NameValueCollection)

                If IsNothing(_savedDetails) Then
                    _savedDetails = New NameValueCollection()
                    Session(cSessionSaveDetailsKey) = _savedDetails
                End If
            End If

            Return _savedDetails
        End Get
    End Property

    Protected ReadOnly Property targetGrid As RadGrid Implements IGridPage.targetGrid
        Get
            If Not IsNothing(_targetGrids) AndAlso _targetGrids.Count > 0 Then
                Return _targetGrids(_targetGrids.Keys(0))
            Else
                If Not IsNothing(targetGrids) AndAlso _targetGrids.Count > 0 Then
                    Return targetGrid ' recursive call, if above test passed _targetGrid must be set
                Else
                    Return Nothing
                End If
            End If
        End Get
    End Property

    Protected ReadOnly Property defaultLoadingPanel As RadAjaxLoadingPanel
        Get
            If Not IsNothing(_loadingpanels) AndAlso _loadingpanels.Count > 0 Then
                Return _loadingpanels(_loadingpanels.Keys(0))
            Else
                If Not IsNothing(loadingPanels) AndAlso _loadingpanels.Count > 0 Then
                    Return defaultLoadingPanel ' recursive call, if above test passed _loadingpanels must be set
                Else
                    Return Nothing
                End If
            End If

        End Get
    End Property
    Dim _loadingpanels As Dictionary(Of String, RadAjaxLoadingPanel) = Nothing

    Public ReadOnly Property loadingPanels As Dictionary(Of String, RadAjaxLoadingPanel)
        Get
            If IsNothing(_loadingpanels) Then
                Dim loadingPanelIDs As List(Of String) = DirectCast(ViewState("loadingPanelIDs"), List(Of String))
                If IsNothing(loadingPanelIDs) Then
                    Dim firstPanel As RadAjaxLoadingPanel = FindFirstLoadingPanel(Me)
                    If Not IsNothing(firstPanel) Then
                        loadingPanelIDs = New List(Of String) From {firstPanel.ID}
                        _loadingpanels = New Dictionary(Of String, RadAjaxLoadingPanel) From {{firstPanel.ID, firstPanel}}
                        If loadingPanelIDs.Count > 1 Then
                            _loadingpanels = New Dictionary(Of String, RadAjaxLoadingPanel)
                            For Each panelId As String In loadingPanelIDs
                                If Not _loadingpanels.ContainsKey(panelId) Then
                                    _loadingpanels.Add(panelId, DeepFindControl(Me, panelId))
                                End If
                            Next

                        End If
                        ViewState("loadingPanelIDs") = loadingPanelIDs
                    End If

                End If

            End If

            Return loadingPanels

        End Get
    End Property

    Dim _targetGrids As Dictionary(Of String, RadGrid) = Nothing

    Protected ReadOnly Property targetGrids As Dictionary(Of String, RadGrid) Implements IGridPage.targetGrids
        Get
            If IsNothing(_targetGrids) Then
                Dim targetGridIDs As List(Of String) = DirectCast(ViewState("targetGridIDs"), List(Of String))
                If IsNothing(targetGridIDs) Then
                    Dim firstGrid As RadGrid = FindFirstRadGrid(Me)
                    If Not IsNothing(firstGrid) Then
                        targetGridIDs = New List(Of String) From {firstGrid.ID}
                        _targetGrids = New Dictionary(Of String, RadGrid) From {{firstGrid.ID, firstGrid}}
                    End If
                    If targetGridIDs.Count > 1 Then
                        _targetGrids = New Dictionary(Of String, RadGrid)
                        For Each gridId As String In targetGridIDs
                            If Not _targetGrids.ContainsKey(gridId) Then
                                targetGrids.Add(gridId, DeepFindControl(Me, gridId))
                            End If
                        Next

                    End If
                    ViewState("targetGridIDs") = targetGridIDs

                End If

            End If

            Return _targetGrids

        End Get
    End Property


    Function getGridFilterControlByID(Of CtlType As Control)(ByVal sctlID As String) As CtlType Implements IGridPage.getGridFilterControlByID
        Return DirectCast(DeepFindControl(targetGrid, sctlID), Control)
    End Function

    Function getGridFilterControlBySaveID(Of CtlType As Control)(ByVal sctlID As String) As CtlType Implements IGridPage.getGridFilterControlBySaveID
        Return DirectCast(getFiltersDef()(sctlID), Control)
    End Function

    MustOverride Function getFiltersDef() As Dictionary(Of String, Control) Implements IGridPage.getFiltersDef

    MustOverride Function DoSearch(ByVal gridID As String) As DataView Implements IGridPage.DoSearch

    Protected Function DoSearch() As DataView
        Return DoSearch("")
    End Function

    Protected Shared Function GetGridFilterControl(Of ctlType As Control)(ByVal targPage As IGridPage, ByVal sctlID As String) As ctlType

        Dim result As ctlType = Nothing

        If Not IsNothing(targPage) Then
            result = DirectCast(targPage.getGridFilterControlByID(Of Control)(sctlID), ctlType)

        End If
        Return result

    End Function

    Protected Sub resetfilterControls(ByVal gridID As String)

        If (savedDetails.Count > 0) Then

            Dim fDefCache As Dictionary(Of String, Control) = getFiltersDef()

            For Each searchKey As String In savedDetails.AllKeys

                If fDefCache.ContainsKey(searchKey) Then
                    If Not IsNothing(fDefCache(searchKey)) Then
                        Dim saveVal As String = IIf(Not String.IsNullOrEmpty(savedDetails(searchKey)), savedDetails(searchKey), "")

                        If TypeOf fDefCache(searchKey) Is RadGrid AndAlso (String.IsNullOrEmpty(gridID) OrElse targetGrids(gridID) Is fDefCache(searchKey)) Then
                            If searchKey.StartsWith(cSavePageIndexKey) Then
                                targetGrids(fDefCache(searchKey).ID).CurrentPageIndex = CInt(savedDetails(searchKey))
                            ElseIf searchKey.StartsWith(cSavePageSizeKey) Then
                                targetGrids(fDefCache(searchKey).ID).PageSize = CInt(savedDetails(searchKey))

                            End If
                        ElseIf String.IsNullOrEmpty(gridID) OrElse IsParentOf(targetGrids(gridID), fDefCache(searchKey)) Then
                            If TypeOf fDefCache(searchKey) Is RadTextBox Then
                                DirectCast(fDefCache(searchKey), RadTextBox).Text = saveVal
                            ElseIf TypeOf fDefCache(searchKey) Is RadComboBox Then
                                Dim ddlList As RadComboBox = DirectCast(fDefCache(searchKey), RadComboBox)
                                If ddlList.Items.Count > 0 AndAlso Not String.IsNullOrEmpty(saveVal) Then

                                    Dim selItem As RadComboBoxItem = ddlList.FindChildByValue(Of RadComboBoxItem)(saveVal)

                                    If Not IsNothing(selItem) Then
                                        ddlList.SelectedValue = saveVal
                                    Else
                                        ddlList.Text = saveVal

                                    End If
                                Else
                                    ddlList.Text = ""
                                End If

                            ElseIf TypeOf fDefCache(searchKey) Is RadTimePicker Then
                                Dim picker As RadTimePicker = DirectCast(fDefCache(searchKey), RadTimePicker)

                                picker.SelectedTime = New TimeSpan(CInt(saveVal.Substring(0, 2)), CInt(saveVal.Substring(2)), 0)

                            ElseIf TypeOf fDefCache(searchKey) Is RadDatePicker Then
                                Dim picker As RadDatePicker = DirectCast(fDefCache(searchKey), RadDatePicker)

                                picker.SelectedDate = New Date(CInt(saveVal.Substring(0, 4)), CInt(saveVal.Substring(4, 2)), CInt(saveVal.Substring(6, 2)))

                            End If
                        End If

                    End If
                End If


            Next

        End If
    End Sub

    Protected Sub restoreSearchDetails(ByVal gridID As String)

        If String.IsNullOrEmpty(gridID) Then
            resetfilterControls(gridID)
            targetGrid.DataSource = Nothing
            targetGrid.DataBind()

        End If
    End Sub

    Protected Sub SetSavedDetail(ByVal key As String, ByVal value As String)
        If Not String.IsNullOrEmpty(savedDetails(key)) Then
            If Not String.IsNullOrEmpty(value) Then
                savedDetails(key) = value
            Else
                savedDetails.Remove(key)
            End If
        Else
            If Not String.IsNullOrEmpty(value) Then
                savedDetails.Add(key, value)
            End If
        End If

    End Sub

    Protected Overridable Sub saveSearchDetails()

        Dim fDefCache As Dictionary(Of String, Control) = getFiltersDef()

        For Each ctlKey As String In fDefCache.Keys

            If Not IsNothing(fDefCache(ctlKey)) Then

                If TypeOf fDefCache(ctlKey) Is RadTextBox Then
                    SetSavedDetail(ctlKey, DirectCast(fDefCache(ctlKey), RadTextBox).Text)

                ElseIf TypeOf fDefCache(ctlKey) Is RadComboBox Then

                    Dim cboList As RadComboBox = DirectCast(fDefCache(ctlKey), RadComboBox)

                    If Not (IsNothing(cboList) OrElse (String.IsNullOrEmpty(cboList.SelectedValue))) Then
                        If Not String.IsNullOrEmpty(cboList.SelectedValue) Then
                            SetSavedDetail(ctlKey, cboList.SelectedValue)
                        Else
                            SetSavedDetail(ctlKey, cboList.Text)
                        End If

                    Else
                        savedDetails.Remove(ctlKey)
                    End If

                ElseIf TypeOf fDefCache(ctlKey) Is RadTimePicker Then
                    Dim picker As RadTimePicker = DirectCast(fDefCache(ctlKey), RadTimePicker)

                    If Not IsNothing(picker) AndAlso picker.SelectedTime.HasValue Then
                        SetSavedDetail(ctlKey, String.Format("{0:00}{1:00}", picker.SelectedTime.Value.Hours, picker.SelectedTime.Value.Minutes))
                    Else
                        savedDetails.Remove(ctlKey)
                    End If

                ElseIf TypeOf fDefCache(ctlKey) Is RadDatePicker Then
                    Dim picker As RadDatePicker = DirectCast(fDefCache(ctlKey), RadDatePicker)

                    If Not IsNothing(picker) AndAlso picker.SelectedDate.HasValue Then
                        SetSavedDetail(ctlKey, picker.SelectedDate.Value.ToString("yyyyMMdd"))
                    Else
                        savedDetails.Remove(ctlKey)
                    End If


                ElseIf TypeOf fDefCache(ctlKey) Is RadGrid Then

                    If ctlKey.StartsWith(cSavePageIndexKey) Then
                        SetSavedDetail(ctlKey, DirectCast(targetGrids(fDefCache(ctlKey).ID), RadGrid).CurrentPageIndex)
                    ElseIf ctlKey.StartsWith(cSavePageSizeKey) Then
                        SetSavedDetail(ctlKey, DirectCast(targetGrids(fDefCache(ctlKey).ID), RadGrid).PageSize)

                    End If

                End If

            End If

        Next

    End Sub

    Private Sub grid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs)

        If Not IsNothing(sender) AndAlso TypeOf sender Is RadGrid Then
            Dim targGrid As RadGrid = DirectCast(sender, RadGrid)

            If targetGrids.Keys.Contains(targetGrid.ID) Then
                targetGrids(targetGrid.ID).DataSource = DoSearch(targetGrid.ID)
            End If
        End If



    End Sub

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)
        If Not IsNothing(targetGrids) Then
            For Each rgGrid As RadGrid In _targetGrids.Values
                AddHandler rgGrid.NeedDataSource, New GridNeedDataSourceEventHandler(AddressOf grid_NeedDataSource)
                'AddHandler rgGrid.PageIndexChanged, New GridPageChangedEventHandler(AddressOf rgGrid_PageIndexChanged)
                AddHandler rgGrid.PageSizeChanged, New GridPageSizeChangedEventHandler(AddressOf rgGrid_PageSizeChanged)
                AddHandler rgGrid.PreRender, New EventHandler(AddressOf rgGrid_PreRender)
                rgGrid.MasterTableView.NoMasterRecordsText = PageString("SearchEmptyText")

            Next
        End If

    End Sub

    Protected Overrides Sub OnInitComplete(e As EventArgs)
        MyBase.OnInitComplete(e)

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        '        If Not IsPostBack Then

        Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Me)

        If Not IsNothing(defaultLoadingPanel) Then
            ajaxManager.AjaxSettings.AddAjaxSetting(targetGrid, targetGrid, defaultLoadingPanel)
        Else
            ajaxManager.AjaxSettings.AddAjaxSetting(targetGrid, targetGrid)
        End If

        '       End If
        MyBase.OnLoad(e)
    End Sub

    'Private Sub rgGrid_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs)
    '    If Not IsNothing(sender) AndAlso TypeOf sender Is RadGrid Then
    '        DirectCast(sender, RadGrid).Rebind()
    '    End If
    'End Sub

    Private Sub rgGrid_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs)
        If Not IsNothing(sender) AndAlso TypeOf sender Is RadGrid Then
            DirectCast(sender, RadGrid).Rebind()
        End If
    End Sub

    Private Sub rgGrid_PreRender(sender As Object, e As EventArgs)
        If Not IsNothing(sender) AndAlso TypeOf sender Is RadGrid Then
            Dim targGrid As RadGrid = DirectCast(sender, RadGrid)

            Dim strGridID As String = targetGrid.ID

            If strGridID = targetGrids.Keys(0) Then
                strGridID = ""
            End If

            resetfilterControls(strGridID)
        End If

    End Sub

End Class
