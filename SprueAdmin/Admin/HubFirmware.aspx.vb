Imports System.Data
Imports IntamacBL_SPR
Imports IntamacShared_SPR.SharedStuff

Partial Class Admin_HubFirmware
    Inherits CultureBaseClass

#Region "ViewStates"
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

    Private Property HubWaitSeconds As Integer
        Get
            If Not IsNothing(ViewState("HubWaitSeconds")) Then
                Return CInt(ViewState("HubWaitSeconds"))
            Else
                Return Integer.MaxValue
            End If

        End Get
        Set(value As Integer)
            ViewState("HubWaitSeconds") = value
        End Set
    End Property

    Private Property SelectedFirmware As String
        Get
            If Not IsNothing(ViewState("SelectedFirmware")) Then
                Return ViewState("SelectedFirmware")
            Else
                Return ""
            End If

        End Get
        Set(value As String)
            ViewState("SelectedFirmware") = value
        End Set
    End Property

    Private Property FirmwareTriggered As DateTime
        Get
            If Not IsNothing(ViewState("FirmwareTriggered")) Then
                Return DirectCast(ViewState("FirmwareTriggered"), DateTime)
            Else
                Return DateTime.Now
            End If

        End Get
        Set(value As DateTime)
            ViewState("FirmwareTriggered") = value
        End Set
    End Property


#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        MyBase.Title = "Hub Firmware"

        If Not IsPostBack Then

            Dim strMac As String = mSelectedGateway 'The selected gateway should be stored in the session.

            If strMac IsNot Nothing Then
                hdnMAC.Value = strMac

                Dim device As New IntamacDevice

                If device.Load(strMac, True) Then

                    lblHead.Text = PageString("FirmwareMacLabel") & " " & device.MACAddress

                    If device.FirmwareVersion <> "" Then
                        Dim strFirmwareName As String = device.FirmwareVersion
                        strFirmwareName = miscFunctions.GetFirmwareName(device.DeviceID, device.FirmwareVersion)
                        lblFirmwareVersion.Text = strFirmwareName
                    Else
                        lblFirmwareVersion.Text = PageString("FirmwareUnknown")
                    End If

                    PopulateFWList(device.DeviceID)

                Else

                End If
            Else


            End If

        End If

        btnUpdateFirmware.Attributes.Add("onclick", "return confirm_update('" & GetLocalResourceObject("UpdateFirmwareConfirm") & "');")

    End Sub



    Private Sub PopulateFWList(v_strDeviceID As String)

        'populate drop down list with all firmware versions i.e. "Full OS" and "EA3 only add-ons"

        'FirmwareDesc is now used as SelectedValue in ddlFirmware to distinguish between each firmware version string, 
        'e.g. "STG EA3 0.9 NO OS" and "STG EA3 0.9" are different FirmwareDesc values but both correspond to the same FirmwareVersion of "1_sprue_1_52_sprue_hub_ea3_intamac_sprue_stag-0.9"
        Dim objFirmwareUpgrade As New FirmwareUpgrade

        ddlFirmware.DataSource = objFirmwareUpgrade.Load(v_strDeviceID, "SPR", Nothing, True)
        ddlFirmware.DataBind()

    End Sub


    Protected Sub btnRetry_Click(sender As Object, e As EventArgs) Handles btnRetry.Click
        StartFirmwareCheck()
    End Sub

    Private Sub StartFirmwareCheck()
        Dim device As New IntamacDevice
        If device.Load(hdnMAC.Value.ToString, True) Then

            If device.IsOnline = True Then

                If Not ddlFirmware.SelectedValue Is Nothing Then

                    Dim objFirmwareUpgrade As New FirmwareUpgrade
                    Dim selectedFW As New FirmwareUpgrade

                    'loop through "Full OS" and "EA3 only add-ons"
                    For Each objFirmwareUpgrade In objFirmwareUpgrade.Load(device.DeviceID, "SPR", Nothing, True)
                        If objFirmwareUpgrade.FirmwareDesc = ddlFirmware.SelectedValue Then
                            selectedFW = objFirmwareUpgrade
                            Exit For
                        End If
                    Next

                    If selectedFW IsNot Nothing Then

                        Dim objControl As New IntamacBL_SPR.AlarmPanel

                        objControl.AccountID = "" 'Not needed in the Stored procedure.  Param is required but the value is not used.
                        objControl.PropertyID = "" 'Not needed in the Stored procedure.  Param is required but the value is not used.
                        objControl.MACAddress = device.MACAddress
                        objControl.SerialNo = "" 'Not needed in the Stored procedure.  Param is required but the value is not used.

                        'save firmware description in hidden field which can be used for showing on page after firmware upgrade
                        hdnFirmwareDesc.Value = selectedFW.FirmwareDesc

                        'upgrade firmware in IoT
                        FirmwareTriggered = objControl.UpgradeFirmware(selectedFW.DownloadURL, selectedFW.Signature, selectedFW.Hash, selectedFW.FirmwareVersion)

                        'Add an audit record to indicate that someone has updated firmware
                        miscFunctions.AddAuditRecord(mAccountID, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_FirmwareUpdate_Success, "")

                        timerTicks = 0
                        HubWaitSeconds = 0
                        SelectedFirmware = selectedFW.FirmwareVersion

                        'show firmware update progress
                        divFirmwareUpdate.Visible = True
                        divFirmwareStatus.Visible = False
                        ddlFirmware.Enabled = False
                        divProgressPercent.Style.Add("width", ((0).ToString() + "%"))
                        divProgressPercent.Style.Add("background-color", "green")
                        divProgressPercent.InnerText() = ""
                        lblCheckingforUpdates.Visible = False
                        tmrHubFirmware.Enabled = True

                    End If
                End If

            Else
                tmrHubFirmware.Enabled = False
                lblHubFirmwareError.Text = GetLocalResourceObject("HubNotConnectedError")
                lblHubFirmwareError.Visible = True
                btnRetry.Visible = True
                ddlFirmware.Enabled = True
                lblCommandSent.Visible = False
                divProgressBar.Style.Add("display", "none")
                timerTicks = 0
                HubWaitSeconds = 0
                SelectedFirmware = Nothing
            End If
        End If

    End Sub

    Protected Sub btnUpdateFirmware_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateFirmware.Click

        If Page.IsValid Then
            StartFirmwareCheck()
        End If

    End Sub

    Protected Sub tmrHubFirmware_Tick(sender As Object, e As EventArgs) Handles tmrHubFirmware.Tick
        timerTicks += 1

        Try

            Dim intaDevice As New IntamacDevice
            intaDevice.Load(hdnMAC.Value.ToString, True)

            If HubWaitSeconds = 0 Then
                HubWaitSeconds = 400 'default to over 6 mins (EA2 to EA3 upgrade can take longer than 5 mins otherwise normally within 5 mins)
            End If

            Dim intTicks As Integer = (1000 * HubWaitSeconds) / tmrHubFirmware.Interval

            If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings("HubFirmwareWaitSeconds")) Then
                HubWaitSeconds = System.Configuration.ConfigurationManager.AppSettings("HubFirmwareWaitSeconds")
            End If

            If timerTicks < intTicks Then

                If (Not IsNothing(intaDevice) AndAlso Not String.IsNullOrEmpty(intaDevice.DeviceID)) Then

                    divProgressBar.Style.Add("display", "block")
                    divProgressPercent.Style.Add("width", ((Math.Ceiling(timerTicks / 4.5)).ToString() + "%"))
                    divProgressPercent.InnerText() = ((Math.Ceiling(timerTicks / 4.5)).ToString() + "%")
                    lblCommandSent.Visible = True
                    lblCheckingforUpdates.Visible = False

                    Dim upgradeStatus As UpgradeStatusStruct = SprueHub.GetFirmwareUpdateStatus(intaDevice.MACAddress)
                    If (upgradeStatus.online AndAlso upgradeStatus.state = GatewayStateEnum.Running AndAlso upgradeStatus.triggered = FirmwareTriggered AndAlso upgradeStatus.version = SelectedFirmware) Then

                        'device online, current firmware matches latest firmware and firmware url
                        timerTicks = 0
                        lblCheckingforUpdates.Text = GetLocalResourceObject("HubFirmwareUpdateComplete")

                        If Not String.IsNullOrEmpty(upgradeStatus.version) Then
                            'update firmware name on display e.g. "1_sprue_1_52_sprue_hub_ea3_intamac_sprue_stag-0.9"
                            lblFirmwareVersion.Text = upgradeStatus.version
                        End If

                        divFirmwareStatus.Visible = True
                        divProgressPercent.InnerText() = "100%"
                        divProgressPercent.Style.Add("width", ((100).ToString() + "%"))
                        tmrHubFirmware.Enabled = False
                        btnRetry.Visible = False
                        lblCommandSent.Visible = False
                        lblCheckingforUpdates.Visible = True
                        ddlFirmware.Enabled = True
                    Else
                        lblHubFirmwareError.Visible = False
                        lblHubFirmwareError.Text = ""
                    End If
                End If
            Else
                tmrHubFirmware.Enabled = False
                lblHubFirmwareError.Text = GetLocalResourceObject("HubFirmwareFailed")
                lblHubFirmwareError.Visible = True
                divFirmwareStatus.Visible = True
                btnRetry.Visible = True
                ddlFirmware.Enabled = True
                lblCommandSent.Visible = False
                divProgressPercent.Style.Add("width", ((100).ToString() + "%"))
                divProgressPercent.Style.Add("background-color", "red")
                divProgressPercent.InnerText() = ""
                timerTicks = 0
                HubWaitSeconds = 0
                SelectedFirmware = Nothing
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    End Sub
End Class
