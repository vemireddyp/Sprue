Imports Microsoft.VisualBasic
Imports System.Collections.Specialized
Imports System.Web

Public Class ThermostatsGetter

	
	Private _IsOnline As Boolean = False
	Private _Model As String = ""
	Private _DeviceName As String = ""

	Public Property IsOnline As Boolean
		Get
			Return _IsOnline

		End Get
		Set(value As Boolean)
			_IsOnline = value
		End Set
	End Property
	Public Property Model As String
		Get
			Return _Model

		End Get
		Set(value As String)
			_Model = value
		End Set
	End Property
	Public Property DeviceName As String
		Get
			Return _DeviceName

		End Get
		Set(value As String)
			_DeviceName = value
		End Set
	End Property

	Public Shared Function GetAllThermostats(ByVal accountID As String, ByVal propertyID As String) As ArrayList


        'Dim lstThermostatSettings As List(Of IntamacBL_SPR.ThermostatSettingsNest) = Nothing
        'Dim lstThermostats As Generic.List(Of ThermostatsGetter) = Nothing
        'Dim oThermostatSettings As IntamacBL_SPR.ThermostatSettingsNest = Nothing
        'Dim oThermostat As ThermostatsGetter = Nothing

        ''Nest thermostats
        'oThermostatSettings = New IntamacBL_SPR.ThermostatSettingsNest
        'lstThermostatSettings = oThermostatSettings.LoadByPropertyID(accountID, propertyID)

        Dim lstThermostats = New Generic.List(Of ThermostatsGetter)

        'For Each oThermostatSettings In lstThermostatSettings

        '    oThermostat = New ThermostatsGetter
        '    oThermostat.DeviceName = IIf(oThermostatSettings.DeviceName.Equals(String.Empty), "N/A", oThermostatSettings.DeviceName).ToString() 'oThermostatSettings.DeviceName
        '    oThermostat.Model = "Nest"
        '    oThermostat.IsOnline = oThermostatSettings.IsOnline

        '    lstThermostats.Add(oThermostat)
        '    oThermostat = Nothing

        'Next

        ''Zen Thermostats

        'Dim lstPropZones As List(Of IntamacBL_SPR.PropZone) = Nothing
        'Dim oPropZone As IntamacBL_SPR.PropZone = Nothing

        'oPropZone = New IntamacBL_SPR.PropZoneSWA
        'lstPropZones = oPropZone.Load(accountID, propertyID)

        'For Each oZone As IntamacBL_SPR.PropZone In lstPropZones
        '    If oZone.SensorType.HasValue Then
        '        If oZone.SensorType = 26 Then 'Zen

        '            oThermostat = New ThermostatsGetter
        '            oThermostat.DeviceName = IIf(oZone.PropZoneDesc.Equals(String.Empty), "N/A", oZone.PropZoneDesc).ToString()
        '            oThermostat.Model = "Zen"
        '            oThermostat.IsOnline = oZone.IsOnline

        '            lstThermostats.Add(oThermostat)
        '            oThermostat = Nothing

        '        End If
        '    End If
        'Next



        'Dim lstCams As Generic.List(Of IntamacBL_SPR.Camera)

        'If IsNothing(HttpContext.Current.Items("DiagCamerasList")) Then
        '	Dim cam As New IntamacBL_SPR.Camera
        '	cam.CompanyType = miscFunctions.CompanyType
        '	lstCams = cam.Load(accountID, propertyID)
        '	If lstCams IsNot Nothing Then
        '		If lstCams.Count > 0 Then
        '			For I As Integer = 0 To lstCams.Count - 1
        '				lstCams(I).FirmwareVersion = miscFunctions.GetFirmwareName(lstCams(I).DeviceID, lstCams(I).FirmwareVersion)
        '			Next
        '		End If
        '	End If
        '	HttpContext.Current.Items("DiagCamerasList") = lstCams
        'Else
        '	lstCams = CType(HttpContext.Current.Items("DiagCamerasList"), Generic.List(Of IntamacBL_SPR.Camera))
		'End If

		Return New ArrayList(lstThermostats)

	End Function

	Public Shared Function GetAllThermostats(ByVal accountID As String, ByVal propertyID As String, ByVal startIndex As Integer, ByVal maxRows As Integer) As ArrayList

		Dim lstThermostats As ArrayList = GetAllThermostats(accountID, propertyID)

		If maxRows < 0 Then maxRows = lstThermostats.Count - startIndex - 1

		Dim retList As New ArrayList

		For index As Integer = startIndex To startIndex + maxRows
			If index < lstThermostats.Count Then
				retList.Add(lstThermostats(index))
			Else
				Exit For
			End If
		Next

		Return retList


	End Function

	Public Shared Function CountThermostats(ByVal accountID As String, ByVal propertyID As String) As Integer
		Return GetAllThermostats(accountID, propertyID, 0, Integer.MaxValue).Count

	End Function


End Class

