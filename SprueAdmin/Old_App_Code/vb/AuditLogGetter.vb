Imports Microsoft.VisualBasic
Imports IntamacBL_SPR

Public Class AuditLogGetter
    Public Shared Function GetAllAuditLogs(ByVal v_strStartDate As String, ByVal v_strEndDate As String, ByVal v_strUsername As String, ByVal v_strAuditType As String, ByVal v_strSortExpr As String) As ArrayList
        Dim lstTransAudits As New SortedList(Of Object, AuditTypesTranslated)
        Dim retArray As ArrayList
        Dim sortParts() As String

        Dim pageObj As Object = HttpContext.Current.Handler

        If TypeOf pageObj Is CultureBaseClass Then
            Dim page As CultureBaseClass = CType(pageObj, CultureBaseClass)

            Dim lstAudits As List(Of Audit) = Nothing

            Dim accountID As String = page.mAccountID

            Dim startDate As Date = DateAdd(DateInterval.Month, -3, Date.Now)
            Dim endDate As Date = Date.Now

            sortParts = v_strSortExpr.Split(New Char() {" "c})

            Dim auditType As Integer = 0

            Try
                If Not String.IsNullOrEmpty(v_strStartDate) Then
                    startDate = CDate(v_strStartDate)
                End If

                If Not String.IsNullOrEmpty(v_strEndDate) Then
                    ' add 11:59:59 to end date
                    endDate = DateAdd(DateInterval.Second, 24 * 60 * 60 - 1, CDate(v_strEndDate))
                End If

                If Not String.IsNullOrEmpty(v_strAuditType) Then
                    auditType = CInt(v_strAuditType)
                End If

                If Not String.IsNullOrEmpty(accountID) Then
                    Dim audits As New AuditSPR

                    If auditType = 0 Then
                        lstAudits = audits.LoadAuditRecords(accountID)
                    Else
                        'lstAudits = audits.LoadAuditRecordsByAuditType(accountID, auditType)
                    End If

                    lstAudits = lstAudits.FindAll(Function(rec) CBool(rec.DateEntered >= startDate And rec.DateEntered <= endDate))

                End If

                Dim recCount As Integer = 0
                Dim sortKey As Object
                For Each log As Audit In lstAudits
                    If String.IsNullOrEmpty(v_strUsername) OrElse log.Username.ToLower = v_strUsername.ToLower Then
                        recCount += 1
                        Dim translated As New AuditTypesTranslated(log)
                        Select Case sortParts(0)
                            Case "Audit.DateEntered"
                                sortKey = translated.Audit.DateEntered
                            Case "Audit.Username"
                                sortKey = translated.Audit.Username.ToLower
                            Case "AuditTypeTranslated"
                                sortKey = translated.AuditTypeTranslated.ToLower
                            Case "Audit.Notes"
                                sortKey = translated.Audit.Notes.ToLower
                            Case Else
                                sortKey = recCount

                        End Select

                        If lstTransAudits.ContainsKey(sortKey) Then
                            Select Case sortParts(0)
                                Case "Audit.DateEntered"
                                    While lstTransAudits.ContainsKey(sortKey)
                                        sortKey = CDate(sortKey).AddMilliseconds(1)
                                    End While
                                Case "Audit.Username", "AuditTypeTranslated", "Audit.Notes"
                                    sortKey = String.Format("{0}{1:0000000000}", sortKey, recCount)
                            End Select
                        End If

                        lstTransAudits.Add(sortKey, translated)
                    End If
                Next
            Catch ex As Exception
                IntamacShared_SPR.SharedStuff.LogError(ex)
            End Try

            retArray = New ArrayList(CType(lstTransAudits.Values, ICollection))
        End If

        If Not IsNothing(sortParts) AndAlso sortParts.Length > 1 AndAlso sortParts(1).ToLower = "desc" Then
            retArray.Reverse()
        End If

        Return retArray
    End Function

    Public Shared Function GetAllAuditLogs(ByVal v_strStartDate As String, ByVal v_strEndDate As String, ByVal v_strUsername As String, ByVal v_strAuditType As String, ByVal v_strSortExpr As String, ByVal startIndex As Integer, ByVal maxRows As Integer) As ArrayList
        Dim lstLogs As ArrayList = GetAllAuditLogs(v_strStartDate, v_strEndDate, v_strUsername, v_strAuditType, v_strSortExpr)

        Dim retList As New ArrayList

        If maxRows < 0 Then maxRows = lstLogs.Count - startIndex - 1

        For index As Integer = startIndex To startIndex + maxRows
            If index < lstLogs.Count Then
                retList.Add(lstLogs(index))
            Else
                Exit For
            End If
        Next

        Return retList


    End Function

End Class

Public Class AuditTypesTranslated

    Private _Audit As Audit = Nothing
    Private _AuditTypeTranslated As String = Nothing
    Private _Notes48CharactersOnly As String = Nothing

    'underlying audit object from the Business Logic Layer
    Public ReadOnly Property Audit As Audit
        Get
            Return _Audit
        End Get
    End Property

    'the plain english translation of the audit type enum
    Public ReadOnly Property AuditTypeTranslated As String
        Get
            If String.IsNullOrEmpty(_AuditTypeTranslated) Then
                TranslateAuditTypes()
            End If
            Return _AuditTypeTranslated
        End Get
    End Property

    Private Sub TranslateAuditTypes()
        'translates the audit type into its enum text equivalent.  
        'removes non-required text, i.e. 'Admin ' and '_'.
        Dim valueName As String = ""

        If [Enum].IsDefined(GetType(IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID), Audit.AuditType) Then
            valueName = CType(_Audit.AuditType, IntamacShared_SPR.SharedStuff.e_SPR_AuditActionTypeID).ToString
        Else
            valueName = CType(_Audit.AuditType, IntamacShared_SPR.SharedStuff.e_AuditActionTypeID).ToString
        End If

        Try
            '
            ' Use resource file version if present

            Dim pageObj As Object = HttpContext.Current.Handler

            If TypeOf pageObj Is CultureBaseClass Then
                _AuditTypeTranslated = CStr(CType(pageObj, CultureBaseClass).GetGlobalResourceObject("AuditTypeResources", valueName))
                If Not String.IsNullOrEmpty(_AuditTypeTranslated) Then
                    Exit Sub
                End If
            End If
        Catch
        End Try

        ' if we drop through to here there's no resource definition (or handler isn't a CultureBaseClass instance)
        _AuditTypeTranslated = valueName.Replace("_", " ").Replace("Admin ", "")


    End Sub

    Public Sub New(ByVal srcAudit As Audit)
        _Audit = srcAudit
    End Sub

End Class

