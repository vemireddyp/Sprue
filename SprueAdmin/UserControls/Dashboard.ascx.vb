Imports Telerik.Web.UI
Imports IntamacDAL_SPR

Public Class Dashboard
	Inherits System.Web.UI.UserControl
	Public Event TileClick As CommandEventHandler

#Region "Fields"
	Private m_blnValuesChanged As Boolean = False
	Private m_QueryType As IntamacDAL_SPR.DashboardCountsDB.eQueryType
#End Region

#Region "Properties"
	Public Property AjaxTargets As String
		Get
			Return CStr(ViewState("AjaxTargets"))
		End Get
		Set(value As String)
			ViewState("AjaxTargets") = value
		End Set
	End Property

	Public Property ParentCoID As Integer
		Get
			If Not IsNothing(ViewState("ParentCoID")) Then
				Return CInt(ViewState("ParentCoID"))
			Else
				Return 0
			End If
		End Get
		Set(value As Integer)
			If Not m_blnValuesChanged Then
				m_blnValuesChanged = ParentCoID <> value
			End If
			ViewState("ParentCoID") = value
		End Set
	End Property

	Public Property InstallerID As Integer
		Get
			If Not IsNothing(ViewState("InstallerID")) Then
				Return CInt(ViewState("InstallerID"))
			Else
				Return 0
			End If
		End Get
		Set(value As Integer)
			If Not m_blnValuesChanged Then
				m_blnValuesChanged = InstallerID <> value
			End If
			ViewState("InstallerID") = value
		End Set
	End Property

	Public Property AccountID As String
		Get
			If Not IsNothing(ViewState("AccountID")) Then
				Return ViewState("AccountID").ToString()
			Else
				Return Nothing
			End If
		End Get
		Set(value As String)
			If Not m_blnValuesChanged Then
				m_blnValuesChanged = AccountID <> value
			End If
			ViewState("AccountID") = value
		End Set
	End Property

	Public Property PropertyID As String
		Get
			If Not IsNothing(ViewState("PropertyID")) Then
				Return ViewState("PropertyID").ToString()
			Else
				Return Nothing
			End If
		End Get
		Set(value As String)
			If Not m_blnValuesChanged Then
				m_blnValuesChanged = PropertyID <> value
			End If
			ViewState("PropertyID") = value
		End Set
    End Property

    Public Property AreaID As Long?
        Get
            If Not IsNothing(ViewState("AreaID")) Then
                Return CType(ViewState("AreaID"), Long)
            Else
                Return Nothing
            End If
        End Get
        Set(value As Long?)
            If Not m_blnValuesChanged Then
                If AreaID Is Nothing Then
                    m_blnValuesChanged = value Is Nothing
                Else
                    m_blnValuesChanged = AreaID.Equals(value)
                End If
            End If
            ViewState("AreaID") = value
        End Set
    End Property

    Public Property FromSensorID As String
        Get
            If Not IsNothing(ViewState("FromSensorID")) Then
                Return ViewState("FromSensorID").ToString()
            Else
                Return Nothing
            End If
        End Get
        Set(value As String)
            If Not m_blnValuesChanged Then
                m_blnValuesChanged = FromSensorID <> value
            End If
            ViewState("FromSensorID") = value
        End Set
    End Property

    Public Property ToSensorID As String
        Get
            If Not IsNothing(ViewState("ToSensorID")) Then
                Return ViewState("ToSensorID").ToString()
            Else
                Return Nothing
            End If
        End Get
        Set(value As String)
            If Not m_blnValuesChanged Then
                m_blnValuesChanged = ToSensorID <> value
            End If
            ViewState("ToSensorID") = value
        End Set
    End Property

    Public Property RefreshMilliseconds As Integer
        Get
            If Not IsNothing(ViewState("RefreshMilliseconds")) Then
                Return CInt(ViewState("RefreshMilliseconds"))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            ViewState("RefreshMilliseconds") = value
        End Set
    End Property

    Public ReadOnly Property timerPanel As RadXmlHttpPanel
        Get
            If TypeOf Page Is CultureBaseClass Then
                Dim thisPage As CultureBaseClass = DirectCast(Page, CultureBaseClass)

                Return thisPage.xhpTimerPanel
            Else
                Return Nothing
            End If

        End Get
    End Property

    Public Property selectedID As String
        Get
            Return CStr(ViewState("selectedID"))
        End Get
        Set(value As String)
            ViewState("selectedID") = value
        End Set
    End Property
    Public Property AllTileLegend As String
        Get
			If String.IsNullOrEmpty(ViewState("AllTileLegend")) AndAlso TypeOf Page Is CultureBaseClass Then

				If Query = DashboardCountsDB.eQueryType.LoadCountsAllProperties Then
					Return DirectCast(Page, CultureBaseClass).PageString("DSH_TotalDevicesLegend")
				Else
					Return DirectCast(Page, CultureBaseClass).PageString("DSH_TotalSensorsLegend")
				End If

			Else
				Return ViewState("AllTileLegend")
			End If
		End Get
        Set(value As String)
            ViewState("AllTileLegend") = value
        End Set
	End Property

	Public Property Query As IntamacDAL_SPR.DashboardCountsDB.eQueryType
		Get
			Return m_QueryType
		End Get
		Set(value As IntamacDAL_SPR.DashboardCountsDB.eQueryType)
			m_QueryType = value
		End Set
	End Property

#End Region

    Dim m_dtbData As DataTable

    Public Enum Tiles
        NoFaults
        ActiveAlerts
        ActiveFaults
        TotalDevices
        NotTested
        SupportTickets
    End Enum

	Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Dim manager As RadAjaxManager = RadAjaxManager.GetCurrent(Page)
        If Not IsNothing(manager) Then

            manager.AjaxSettings.AddAjaxSetting(dashPanel, lnkAlerts)
            manager.AjaxSettings.AddAjaxSetting(dashPanel, lnkFaults)
            manager.AjaxSettings.AddAjaxSetting(dashPanel, lnkNoFault)
            manager.AjaxSettings.AddAjaxSetting(dashPanel, lnkSupport)
            manager.AjaxSettings.AddAjaxSetting(dashPanel, lnkTotalDevs)
            manager.AjaxSettings.AddAjaxSetting(dashPanel, lnkUntested)


            If Not String.IsNullOrEmpty(AjaxTargets) Then
                Dim targets As String() = AjaxTargets.Split(","c)


                For Each target As String In targets
                    Dim ctl As Control = CultureBaseClass.DeepFindControl(Page, target)

                    If Not IsNothing(ctl) Then
                        manager.AjaxSettings.AddAjaxSetting(lnkAlerts, CultureBaseClass.DeepFindControl(Page, target))
                        manager.AjaxSettings.AddAjaxSetting(lnkFaults, CultureBaseClass.DeepFindControl(Page, target))
                        manager.AjaxSettings.AddAjaxSetting(lnkNoFault, CultureBaseClass.DeepFindControl(Page, target))
                        manager.AjaxSettings.AddAjaxSetting(lnkTotalDevs, CultureBaseClass.DeepFindControl(Page, target))
                        manager.AjaxSettings.AddAjaxSetting(lnkUntested, CultureBaseClass.DeepFindControl(Page, target))
                    End If

                Next

            End If
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        'If Not IsPostBack OrElse m_blnValuesChanged Then
        ' load counts
        Refresh()
        litAllTileLegend.Text = AllTileLegend
        'End If

        If RefreshMilliseconds > 0 Then
            Dim timerCommand = String.Format("setDashCallBack({0});", RefreshMilliseconds.ToString)
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(dashPanel, dashPanel.GetType(), "dashboardTimerCall", timerCommand, True)
            End If

        End If
        If Not String.IsNullOrEmpty(selectedID) Then
            Dim setScript As String = "setTileClasses('" + selectedID + "');"
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "dasboardSetScript", setScript, True)
            Else
                Dim ajaxManager As RadAjaxManager = RadAjaxManager.GetCurrent(Page)

                If ajaxManager IsNot Nothing Then
                    ajaxManager.ResponseScripts.Add(setScript)
                End If
            End If

        End If

    End Sub

	Public Sub Refresh()
		' load counts

		Dim dashboardBL As New IntamacBL_SPR.DashboardCounts()

        dashboardBL.LoadCounts(ParentCoID, InstallerID, AccountID, PropertyID, AreaID, FromSensorID, ToSensorID, m_QueryType)

        litNoAlertsCount.Text = dashboardBL.NoAlertsOrFaults.ToString
		litFaultsCount.Text = dashboardBL.ActiveFaults.ToString
		litAlertsCount.Text = dashboardBL.ActiveAlerts
		litTotalDevicesCount.Text = dashboardBL.TotalDevices
		litUntestedCount.Text = dashboardBL.NotTested
		litTicketsCount.Text = dashboardBL.SupportOpen
    End Sub

	Protected Overridable Sub OnTileClick(e As CommandEventArgs)
		RaiseEvent TileClick(Me, e)

	End Sub


	Private Sub linkButton_Click(sender As Object, e As EventArgs) Handles lnkAlerts.Click, lnkFaults.Click, lnkNoFault.Click, lnkSupport.Click, lnkTotalDevs.Click, lnkUntested.Click
		Dim lnkBtn As LinkButton = DirectCast(sender, LinkButton)

		Dim cmdArgs As New CommandEventArgs(lnkBtn.CommandName, lnkBtn.CommandArgument)

        selectedID = lnkBtn.ClientID

		RaiseEvent TileClick(Me, cmdArgs)

	End Sub

    Private Sub dashPanel_ServiceRequest(sender As Object, e As RadXmlHttpPanelEventArgs) Handles dashPanel.ServiceRequest
        Refresh()

    End Sub
End Class