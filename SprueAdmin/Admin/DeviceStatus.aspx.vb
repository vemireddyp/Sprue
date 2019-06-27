
Partial Class Admin_DeviceStatus
	Inherits System.Web.UI.Page


    Private Property AdminFullAccess As Boolean
        Get
            Return Not IsNothing(Session(miscFunctions.p_SessionFullAccess)) AndAlso CType(Session(miscFunctions.p_SessionFullAccess), Boolean)
        End Get
        Set(value As Boolean)
            Session(miscFunctions.p_SessionFullAccess) = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session(miscFunctions.p_SessionAccountID) = String.Empty
            Session(miscFunctions.p_SessionPropertyID) = String.Empty
            Session(miscFunctions.p_SessionAdminLastAccess) = Nothing
            AdminFullAccess = False
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    End Sub

	Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

		lblResults.Text = ""

		If macOK Then

            Dim dev As New IntamacBL_SPR.IntamacDevice
            dev.CompanyType = IntamacShared_SPR.SharedStuff.e_CompanyType.SPR

			Dim mac As String = txtMac.Text
			Dim macColons As String = ""
			Dim i As Integer = 1

			For Each Chr As Char In mac
				If i Mod 2 = 0 And macColons.Length < 16 Then
					macColons += Chr & ":"
				Else
					macColons += Chr
				End If
				i += 1
			Next

			If dev.Load(macColons, True) Then
				If Not dev.IntaHeartbeatDateTime Is Nothing Then

					If DateDiff(DateInterval.Second, CType(dev.IntaHeartbeatDateTime, Date), Now) < 30 Then
						'polling in
						lblResults.Text = "Mac address is polling"
					Else
						'not polling in
						lblResults.Text = "Mac address is not polling"
					End If

					If dev.AccountID = "" Then
						'unregistered
						lblResults.Text += " and unregistered"
					Else
						'registered
						lblResults.Text += " and registered"
					End If

				Else
					lblResults.Text = "Mac address is not polling and unregistered"
				End If
			Else
				lblResults.Text = "Mac address is not polling and unregistered"
			End If

		End If

	End Sub

	Private Function macOK() As Boolean

		Dim strMac As String = txtMac.Text
		Dim bRet As Boolean = False
		Dim strValid As String = "ABCDEF0123456789"

		If strMac.Length = 12 Then
			For Each Chr As Char In strMac
				If Not strValid.Contains(Chr.ToString.ToUpper) Then
					lblResults.Text = "Mac Address can only contain A-F and 0-9"
					Exit For
				End If
			Next

			If lblResults.Text = "" Then bRet = True

		Else
			lblResults.Text = "Mac address must be 12 characters"
		End If

		Return bRet

	End Function
End Class
