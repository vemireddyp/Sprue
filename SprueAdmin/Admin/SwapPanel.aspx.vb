Imports IntamacBL_SPR

Partial Public Class SwapPanel
    Inherits CultureBaseClass


    Protected Overrides Sub OnLoad(e As EventArgs)
        If String.IsNullOrEmpty(mBackLocation) Then
            mBackLocation = "AccountDetail.aspx"
        End If

        If String.IsNullOrEmpty(mAccountID) Then
            SafeRedirect(mBackLocation, True)
        End If

        MyBase.OnLoad(e)
        If Not ResponseComplete Then

            lblResult.Visible = False

            Dim DeviceSeq As Integer = Nothing

            If Not Page.IsPostBack Then

                If mAccountID <> "" And mPropertyID <> "" Then

                    AccountID.Text = mAccountID

                    Dim objPanel As IntamacBL_SPR.AlarmPanel = IntamacBL_SPR.ObjectManager.CreateAlarmPanel(mAccountID, mPropertyID, mCompanyType)

                    objPanel.CompanyType = mCompanyType
                    objPanel.Load(mAccountID, mPropertyID, DeviceSeq)

                    OldMACAddress.Text = objPanel.MACAddress

                    SetMacAddressProperties()

                End If

            End If

        End If
    End Sub

    Private Sub SetMacAddressProperties()

        'add properties to mac address text boxes
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC1.ClientID, "TabNext", txtHubMAC2.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC2.ClientID, "TabNext", txtHubMAC3.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC3.ClientID, "TabNext", txtHubMAC4.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC4.ClientID, "TabNext", txtHubMAC5.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC5.ClientID, "TabNext", txtHubMAC6.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC6.ClientID, "TabNext", txtHubMAC7.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC7.ClientID, "TabNext", txtHubMAC8.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC8.ClientID, "TabNext", txtHubMAC9.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC9.ClientID, "TabNext", txtHubMAC10.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC10.ClientID, "TabNext", txtHubMAC11.ClientID, False)
        Page.ClientScript.RegisterExpandoAttribute(txtHubMAC11.ClientID, "TabNext", txtHubMAC12.ClientID, False)

    End Sub

    Protected Sub ValidateMAC(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)

        Dim strMAC As String = GetNewMACAddress()
        Dim blnValid As Boolean = True
        Dim strValidChars As String = "1234567890ABCDEF: "
        Dim device As New IntamacBL_SPR.IntamacDevice

        If strMAC.Length > 0 Then

            If strMAC.Length >= 12 Then
                'check entered mac is only valid characters
                For intCount As Integer = 0 To strMAC.Length - 1
                    If Not strValidChars.Contains(strMAC.Substring(intCount, 1).ToUpper) Then
                        blnValid = False
                        valMAC.Text = GetLocalResourceObject("MacAddressInvalidFormat").ToString()
                        Exit For
                    End If
                Next


                If blnValid Then
                    Dim strMacBuilder As Text.StringBuilder = miscFunctions.ConvertMacAddress(strMAC)

                    If strMacBuilder.ToString.Length = 17 Then

                        device.CompanyType = mCompanyType

                        device.Load(strMacBuilder.ToString, True)

                        If Not device Is Nothing AndAlso device.AccountID <> "" Then

                            blnValid = False
                            valMAC.Text = GetLocalResourceObject("MacAddressNotUnique").ToString()

                        End If

                    Else 'error

                        Response.Redirect("ErrorPage.aspx")

                    End If

                Else

                    blnValid = False
                    valMAC.Text = GetLocalResourceObject("MacAddressInvalidLength").ToString()

                End If

            Else
                'not all 12 mac address characters entered
                blnValid = False
                valMAC.Text = GetLocalResourceObject("MacAddressInvalidLength").ToString()

            End If

        Else
            'no mac address characters entered
            blnValid = False
            valMAC.Text = GetLocalResourceObject("MacAddressInvalidLength").ToString()

        End If

        args.IsValid = blnValid

    End Sub

    Protected Sub btnSwapPanel_Click(sender As Object, e As EventArgs) Handles btnSwapPanel.Click

        Page.Validate()

        If Page.IsValid Then

            'show confirmation
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowSwapGateway", "ShowSwapGateway();", True)

        End If

    End Sub

    Private Sub btnSwapGWOK_Click(sender As Object, e As EventArgs) Handles btnSwapGWOK.Click

        'make sure we have a valid mac address (not necessarily polling in) and answer to kit question
        lblResult.Visible = False

        'Page.Validate("vgSwap")

        If Page.IsValid Then

            Dim objStatus As New miscFunctions
            Dim objOldPanel As IntamacBL_SPR.AlarmPanel = IntamacBL_SPR.ObjectManager.CreateAlarmPanel(mAccountID, mPropertyID, mCompanyType)
            Dim objNewPanel As IntamacBL_SPR.AlarmPanel = IntamacBL_SPR.ObjectManager.CreateAlarmPanel(mAccountID, mPropertyID, mCompanyType)
            'Dim objPropDevice As IntamacBL_SPR.PropertyDevice

            objNewPanel.CompanyType = mCompanyType
            objOldPanel.CompanyType = mCompanyType

            Dim newMACAddress As String = GetNewMACAddress()

            'make sure that the old mac address still loads and the new mac address is not already registered (new mac address could already be in UnregisteredDeviceTable)
            Dim oldMACRegistered As Boolean = False
            Dim newMACUnregistered As Boolean = False

            If objOldPanel.Load(OldMACAddress.Text, True) AndAlso Not String.IsNullOrEmpty(objOldPanel.AccountID) Then
                'old mac is registered against an account
                oldMACRegistered = True
            End If

            If Not objNewPanel.Load(newMACAddress, True) Then
                'no record for new mac, so is unregistered
                newMACUnregistered = True
            ElseIf String.IsNullOrEmpty(objNewPanel.AccountID) Then
                'unregistered record exists for new mac
                newMACUnregistered = True
            End If

            If oldMACRegistered = True And newMACUnregistered = True Then

                'send reset command before deleting XMPP user
                miscFunctions.PanelSoftReset(mAccountID, mPropertyID)

                'Swap Gateway (in IoT)
                SprueHub.SwapGateway(OldMACAddress.Text, newMACAddress)

                'delete XMPP user of old mac address
                IntamacShared_SPR.SharedStuff.DeleteXMPPUser(OldMACAddress.Text)

                'create XMPP of new mac address
                Dim strResponse As String = IntamacShared_SPR.SharedStuff.CreateXMPPUser(newMACAddress)

                If strResponse IsNot Nothing AndAlso strResponse.Contains("UserAlreadyExistsException") Then
                    'XMPP user already exists, so call update to ensure password is updated
                    IntamacShared_SPR.SharedStuff.UpdateXMPPUser(newMACAddress)
                End If

                objOldPanel = Nothing
                objOldPanel = New AlarmPanel()
                Dim deviceDeleted = False

                'Poll RESS to check for existence of old MAC Address before allocating license
                'MAC should not exist
                Do
                    'Checks to see if old panel has been deleted
                    deviceDeleted = Not objOldPanel.Load(OldMACAddress.Text, True)

                Loop Until deviceDeleted

                ' update license records for gateways
                Dim licenseAgent As GatewayLicenseAgent = ObjectManager.CreateGatewayLicenseAgent("")

                licenseAgent.SwapGateway(OldMACAddress.Text, newMACAddress)

                lblResult.Text = GetLocalResourceObject("SwappedSuccess").ToString()
                lblResult.ForeColor = Drawing.Color.Green
                lblResult.Visible = True

                btnSwapPanel.Enabled = False    'avoid user from clicking same swap button again after swap hub success

                miscFunctions.AddAuditRecord(AccountID.Text, mPropertyID, User.Identity.Name, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID.Admin_Gateway_Swapped, "Old MAC: " & OldMACAddress.Text & " New MAC: " & newMACAddress)

                'The firmware update page needs the selected gateway set in the session so that it knows which gateway to update.
                'This was previously in the query string but it would be more secure in the session.
                mSelectedGateway = newMACAddress

            Else

                'failed to load both panels
                lblResult.Text = GetLocalResourceObject("SwappedFailed").ToString()
                lblResult.ForeColor = Drawing.Color.Red
                lblResult.Visible = True

            End If

        End If

    End Sub

    Private Function GetNewMACAddress() As String
        'get whole mac address from all text boxes
        Dim strMac As String = txtHubMAC1.Text & txtHubMAC2.Text & txtHubMAC3.Text & txtHubMAC4.Text & txtHubMAC5.Text & txtHubMAC6.Text &
            txtHubMAC7.Text & txtHubMAC8.Text & txtHubMAC9.Text & txtHubMAC10.Text & txtHubMAC11.Text & txtHubMAC12.Text

        'add colons
        strMac = IntamacShared_SPR.SharedStuff.AddColonsToMacAddress(strMac.ToUpper())

        Return strMac

    End Function
End Class