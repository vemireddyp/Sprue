Imports IntamacBL_SPR

Public Class HubReset
	Inherits CultureBaseClass

#Region "Members"
    Private m_ProgressBarWaitSeconds As Integer = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings("GatewayResetTimeInSeconds"))
 #End Region


#Region "ViewState Variables"

	Private Property timerTicks As Integer
		Get
			If Not IsNothing(ViewState("TimerTicks")) Then
				Return CInt(ViewState("TimerTicks"))
			Else
				Return Integer.MaxValue
			End If

		End Get
		Set(value As Integer)
			ViewState("TimerTicks") = value

		End Set
	End Property
#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not Page.IsPostBack Then

			'set Mac Address label
			MacAddressLabel()

        End If
         
	End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    End Sub

	Private Sub MacAddressLabel()
		If mSelectedGateway IsNot Nothing Then lblMacAddress.Text = mSelectedGateway.Trim()
	End Sub

	Private Function ResetPanelToFactory(ByVal v_strPropertyID As String) As Boolean
		Dim p As IntamacBL_SPR.AlarmPanel = Nothing
		Dim bRet As Boolean = False

		Try

			p = IntamacBL_SPR.ObjectManager.CreateAlarmPanel(mAccountID, mPropertyID, mCompanyType)
			p.CompanyType = mCompanyType
			p.Load(mAccountID, mPropertyID, 0)

			'send factory reset command regardless of whether or not the panel is polling in
            bRet = p.FactoryReset(User.Identity.Name, "")

        Catch ex As Exception
            If ex.Message IsNot Nothing AndAlso ex.Message.ToLower().Contains("offline") Then
                bRet = False
            End If
        Finally
            p = Nothing
        End Try

        Return bRet
    End Function

    ''' <summary>
    ''' Added PanelSoftReset funtion send RESET command
    ''' </summary>
    ''' <param name="v_strPropertyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PanelSoftReset(ByVal v_strPropertyID As String) As Boolean
        Dim p As IntamacBL_SPR.AlarmPanel = Nothing
        Dim bRet As Boolean = False

        Try

            p = IntamacBL_SPR.ObjectManager.CreateAlarmPanel(mAccountID, mPropertyID, mCompanyType)
            p.CompanyType = mCompanyType
            p.Load(mAccountID, mPropertyID, 0)

            'Check the Gateway is Online/Offline G2-2640
            If p.IsOnline Then
                'send factory reset command regardless of whether or not the panel is polling in
                bRet = p.SoftReset(User.Identity.Name, "")
                bRet = True
            Else
                bRet = False
            End If

        Catch ex As Exception
            If ex.Message IsNot Nothing AndAlso ex.Message.ToLower().Contains("offline") Then
                bRet = False
            End If
        Finally
            p = Nothing
        End Try

        Return bRet
    End Function


    Private Sub RebootGateway()

        lblGatewayReboot.Text = ""
        lblGatewayReboot.CssClass = "colorGreen"

        divProgressBar.Style.Add("display", "block")
        divProgressPercent.Style.Add("background-color", "green")

        divProgressPercent.Style.Add("width", "0%")
        divProgressPercent.InnerText = "0%"
        divProgressPercent.Visible = True

        updProgressBar.Update()

        Dim PropertyID As String = ""

        If PanelSoftReset(PropertyID) Then
            lblGatewayReboot.CssClass = "colorGreen"
            lblGatewayReboot.Text = GetLocalResourceObject("SoftCommandText").ToString()
            miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Panel_Reset, "")

            timerTicks = 0
            tmrProgressBar.Enabled = True
        Else
            lblGatewayReboot.CssClass = "colorRed"
            lblGatewayReboot.Text = PageString("SoftResetSentFailed")
            divProgressBar.Style.Add("display", "none")
            divProgressPercent.InnerText = ""
            divProgressPercent.Visible = False
            btnGatewayReboot.Enabled = True
        End If

        updProgressBar.Update()

    End Sub


	Private Sub btnGatewayReboot_Click(sender As Object, e As EventArgs) Handles btnGatewayReboot.Click
        btnGatewayReboot.Enabled = False
        RebootGateway()
	End Sub

	Private Sub tmrProgressBar_Tick(sender As Object, e As EventArgs) Handles tmrProgressBar.Tick
		timerTicks += 1

		Dim gate As IntamacBL_SPR.Gateways = Nothing
		Dim lstGateways As List(Of Gateway) = Nothing
		Dim intaDevice As New IntamacDevice
		Dim commandComplete As Boolean = False
		Dim MinimumPercentage As Integer = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings("GatewayResetMinimumPercentBeforeOnline"))

		'instantiate the gateways class
		gate = New IntamacBL_SPR.Gateways

		'load all the gateways of the chosen account, for all properties on that account
		lstGateways = gate.Load(mAccountID, mPropertyID, True)

		If lstGateways IsNot Nothing AndAlso lstGateways.Count > 0 Then
			If lstGateways(0) IsNot Nothing Then
				intaDevice.Load(lstGateways(0).MACAddress, True)
			End If
		End If

		Dim intTicks As Integer = (1000 * m_ProgressBarWaitSeconds) / tmrProgressBar.Interval

        If timerTicks < intTicks Then

            If (Not IsNothing(intaDevice) AndAlso Not String.IsNullOrEmpty(intaDevice.DeviceID)) Then


                divProgressBar.Style.Add("display", "block")
                divProgressPercent.Style.Add("width", ((Math.Ceiling((timerTicks / intTicks) * 100)).ToString() + "%"))
                divProgressPercent.InnerText() = ((Math.Ceiling((timerTicks / intTicks) * 100)).ToString() + "%")

                If Math.Round(((timerTicks / intTicks) * 100)) >= MinimumPercentage AndAlso intaDevice.IsOnline Then

                    timerTicks = 0
                    lblGatewayReboot.CssClass = "colorGreen"
                    lblGatewayReboot.Text = GetLocalResourceObject("FactoryResetComplete").ToString()

                    divProgressPercent.InnerText() = ""
                    divProgressPercent.Visible = False
                    divProgressBar.Style.Add("display", "none")
                    tmrProgressBar.Enabled = False
                    btnGatewayReboot.Enabled = True

                    updProgressBar.Update()

                    miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Gateway_Factory_Reset_Success, "")

                End If

            Else

                tmrProgressBar.Enabled = False
                timerTicks = 0
                divProgressBar.Style.Add("display", "none")
                m_ProgressBarWaitSeconds = 0

                btnGatewayReboot.Enabled = True

            End If

        Else

            tmrProgressBar.Enabled = False
            timerTicks = 0

            lblGatewayReboot.Text = GetLocalResourceObject("SoftResetTimeout").ToString()
            lblGatewayReboot.CssClass = "colorRed"

            divProgressPercent.Style.Add("width", ((100).ToString() + "%"))
            divProgressPercent.Style.Add("background-color", "red")
            divProgressPercent.InnerText() = ""
            m_ProgressBarWaitSeconds = 0
            btnGatewayReboot.Enabled = True

        End If


	End Sub
End Class