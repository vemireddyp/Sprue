Public Class LanguageSelector
    Inherits System.Web.UI.UserControl

    Private Class FlagAndLanguage

        Private _languageResKey As String = Nothing


        Private _countryCode As String = Nothing

        Public ReadOnly Property CountryCode() As String
            Get
                If String.IsNullOrWhiteSpace(_countryCode) AndAlso Not String.IsNullOrWhiteSpace(Culture) Then
                    Dim cultureParts As String() = Culture.Split("-"c)

                    If cultureParts.Length > 1 Then
                        _countryCode = cultureParts(1)
                    End If
                End If

                Return _countryCode
            End Get
        End Property

        Private _culture As String = Nothing

        Public ReadOnly Property Culture() As String
            Get
                If String.IsNullOrWhiteSpace(_culture) AndAlso Not String.IsNullOrWhiteSpace(_languageResKey) Then
                    _culture = LanguageResourceString(String.Format("{0}Culture", _languageResKey))
                End If
                Return _culture
            End Get
        End Property

        Private _flag As String = Nothing
        Public ReadOnly Property Flag() As String
            Get
                If String.IsNullOrWhiteSpace(_flag) AndAlso Not String.IsNullOrWhiteSpace(CountryCode) Then
                    Dim flagFileFormat As String = LanguageResourceString("FlagFileFormat")

                    _flag = String.Format(flagFileFormat, CountryCode)
                End If
                Return _flag
            End Get
        End Property

        Private _languageText As String = Nothing

        Public ReadOnly Property LanguageText() As String
            Get
                If String.IsNullOrWhiteSpace(_languageText) AndAlso Not String.IsNullOrWhiteSpace(_languageResKey) Then
                    _languageText = LanguageResourceString(_languageResKey)
                End If
                Return _languageText
            End Get
        End Property

        Public ReadOnly Property OnClick As String
            Get
                If Not String.IsNullOrWhiteSpace(Culture) Then
                    Dim clickFormat As String = LanguageResourceString("ClickHandlerFormat")

                    Return String.Format(clickFormat, Culture, Date.UtcNow.AddYears(1))
                End If
                Return ""
            End Get
        End Property


        Friend Shared Function LanguageResourceString(resourceKey As String) As String
            Dim resValue As String = "Not Found"
            Try
                If TypeOf HttpContext.Current.Handler Is CultureBaseClass Then
                    resValue = CStr(DirectCast(HttpContext.Current.Handler, CultureBaseClass).GetGlobalResourceObject("LanguageResources", resourceKey))

                End If

            Catch ex As Exception

            End Try

            Return resValue
        End Function

        Public Sub New(ByVal language As String)
            _languageResKey = language
        End Sub


    End Class

    Dim _LanguagesList As List(Of FlagAndLanguage) = BuildFlagAndLanguageList()

    Private Function BuildFlagAndLanguageList() As List(Of FlagAndLanguage)
        Dim retList As New List(Of FlagAndLanguage)

        Dim supportedLanguages As String = FlagAndLanguage.LanguageResourceString("SupportedLanguages")

        If Not String.IsNullOrWhiteSpace(supportedLanguages) Then
            For Each supLang As String In supportedLanguages.Split(","c)
                If Not String.IsNullOrWhiteSpace(supLang) Then
                    retList.Add(New FlagAndLanguage(supLang.Trim()))
                End If
            Next
        End If

        Return retList

    End Function

    Private Sub LanguageSelector_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim countryCode As String = "GB"

        If Not Request.Cookies(miscFunctions.c_CultureCookie) Is Nothing AndAlso Request.Cookies(miscFunctions.c_CultureCookie).Value <> "" Then
            countryCode = Request.Cookies(miscFunctions.c_CultureCookie).Value.Split("-"c)(1)
            Session("countryCode") = countryCode
        End If
        Dim langFlag As FlagAndLanguage = _LanguagesList.Find(Function(flag As FlagAndLanguage) flag.CountryCode = countryCode)

        'Set the flag and Language
        If langFlag IsNot Nothing Then
            litLanguage.Text = langFlag.LanguageText
            imgFlagSelected.ImageUrl = langFlag.Flag

        End If
        rptChooseFlag.DataSource = _LanguagesList
        rptChooseFlag.DataBind()
    End Sub
End Class