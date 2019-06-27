Imports System.Collections.Concurrent
Imports System.Collections.Generic
Imports System.Text
Imports System.Resources
Imports System.Web.Compilation
Imports System.Reflection
Imports System.Globalization
Imports System.Collections
Imports System.Xml
Imports System.IO
Imports System.Linq
Imports System.Web.Caching
Imports System.Windows.Forms

Public Class UpdatableResXResourceProviderFactory
    Inherits ResourceProviderFactory
    Public Overrides Function CreateGlobalResourceProvider(classKey As String) As IResourceProvider
        Return New GlobalResXResourceProvider(classKey)
    End Function

    Public Overrides Function CreateLocalResourceProvider(virtualPath As String) As IResourceProvider
        '
        '             * Optimization: restrict file-based provider to sections known to use ASP.NET localization.
        '             * 

        If virtualPath.ToLower.Contains(".aspx") OrElse virtualPath.ToLower.Contains(".ascx") OrElse virtualPath.ToLower.Contains(".master") OrElse virtualPath.ToLower.Contains(".eula") Then
            Return New LocalResXResourceProvider(virtualPath)
        End If

        '
        '             * Return the basic (empty) provider for all other cases.
        '             * 

        Return New BasicResXResourceProvider(New BasicResXResourceReader(New Dictionary(Of String, Object)()))
    End Function
End Class

''' <summary>
''' Base abstract class for updatable *.resx provider.
''' </summary>
MustInherit Class UpdatableResXResourceProvider
    Implements IResourceProvider
    ' ''' <summary>
    ' ''' Specified the default culture. 
    ' ''' </summary>
    Private Shared _defaultUICultureLock = New Object()
    Private Shared _defaultUICulture As CultureInfo = Nothing

    Private Shared ReadOnly Property DefaultUICulture As CultureInfo
        Get
            SyncLock _defaultUICultureLock
                If _defaultUICulture Is Nothing Then
                    _defaultUICulture = New CultureInfo(CType(System.Configuration.ConfigurationManager.GetSection("system.web/globalization"), System.Web.Configuration.GlobalizationSection).UICulture)

                End If

            End SyncLock

            Return _defaultUICulture
        End Get
    End Property


    ''' <summary>
    ''' Path offset where *.resx files are stored. Clearing this value will result in *.resx
    ''' lookups in standard ASP.NET locations.
    ''' </summary>
    Private Shared _resourceRootLock As New Object()

    Protected Shared _defaultStore As String = Nothing
    Protected Shared ReadOnly Property DefaultStore As String
        Get
            SyncLock _resourceRootLock
                If _defaultStore Is Nothing Then
                    _defaultStore = System.Configuration.ConfigurationManager.AppSettings("ResourceRoot")
                End If
            End SyncLock

            Return _defaultStore
        End Get
    End Property

    Private Shared _keysMappingLock As New Object
    Private Shared _keysMapping As ConcurrentDictionary(Of String, String)

    Private ReadOnly Property KeysMapping As ConcurrentDictionary(Of String, String)
        Get
            SyncLock (_keysMappingLock)
                If _keysMapping Is Nothing Then
                    _keysMapping = New ConcurrentDictionary(Of String, String)

                End If
            End SyncLock

            Return _keysMapping

        End Get
    End Property
    ''' <summary>
    ''' </summary>
    ''' <param name="resourceKey"></param>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    Public Function GetObject(resourceKey As String, culture As CultureInfo) As Object Implements IResourceProvider.GetObject
        ' 
        '             * Optimization: avoid lookups for the DefaultUICulture and the InstalledUICulture.
        '             * 
        If culture Is Nothing Then
            culture = CultureInfo.CurrentUICulture
        End If

        Dim returnObj As Object = GetObject(resourceKey, culture, True)
        '
        '             * If key wasn't found for the culture specified, retry the default culture.
        '             * 
        If returnObj Is Nothing Then
            Return GetObject(resourceKey, Nothing, False)
        Else
            Return returnObj

        End If

        Return Nothing
    End Function

    Private Function GetObject(resourceKey As String, culture As CultureInfo, allowFallback As Boolean) As Object
        If allowFallback AndAlso culture Is Nothing Then
            culture = CultureInfo.CurrentUICulture
        End If

        Dim reader As BasicResXResourceReader = TryCast(GetResourceCache(culture), BasicResXResourceReader)
        resourceKey = resourceKey.Replace(Space(1), String.Empty).ToLowerInvariant
        If reader IsNot Nothing Then
            If reader.Resources.Contains(resourceKey) Then
                Return reader.Resources(resourceKey)
            End If
        End If

    End Function

    ''' <summary>
    ''' Relative path to *.resx file.
    ''' </summary>
    Public MustOverride ReadOnly Property Path() As String

    Public MustOverride ReadOnly Property DefaultPath() As String
    Public MustOverride ReadOnly Property ResName() As String

    Public ReadOnly Property ResourceReader() As IResourceReader Implements IResourceProvider.ResourceReader
        Get
            Return GetResourceCache(Nothing)
        End Get
    End Property

    Private Function GetResourceCache(culture As CultureInfo) As IResourceReader
        Dim cache__1 As Cache = System.Web.HttpContext.Current.Cache
        Dim resourceReader As IResourceReader
        Dim reader As BasicResXResourceReader = Nothing
        Dim fullPath As String = Resolve(Path, culture)
        Dim origKey As String = Convert.ToString("ResourceFile ") & fullPath
        Dim blnKeyChanged As Boolean = False


        Try
            Dim dependancy As CacheDependency = Nothing
            Dim key As String = origKey

            ' check to see if requested language file has already been mapped to another
            If KeysMapping.ContainsKey(origKey.ToLowerInvariant) Then
                key = KeysMapping(origKey.ToLowerInvariant)

            End If


            If (InlineAssignHelper(resourceReader, TryCast(cache__1(key), IResourceReader))) Is Nothing Then


                If culture IsNot Nothing Then
                    If File.Exists(fullPath) Then
                        dependancy = New CacheDependency(fullPath)
                        reader = New BasicResXResourceReader(New ResXResourceReader(fullPath))
                    End If
                End If
            Else
                ' required reader found on first attempt, the rest of the processing performed in this function is redundant
                Return resourceReader
            End If


            If resourceReader Is Nothing AndAlso reader Is Nothing Then
                blnKeyChanged = True
                fullPath = Resolve(DefaultPath, culture)
                key = Convert.ToString("ResourceFile ") & fullPath
                If (InlineAssignHelper(resourceReader, TryCast(cache__1(key), IResourceReader))) Is Nothing Then
                    If File.Exists(fullPath) Then
                        dependancy = New CacheDependency(fullPath)
                        reader = New BasicResXResourceReader(New ResXResourceReader(fullPath))
                    End If

                End If
            End If

            If resourceReader Is Nothing AndAlso reader Is Nothing AndAlso culture IsNot Nothing Then
                ' still not found required resource file,  look for 'culture free' default version, in user updateable area
                fullPath = Resolve(Path, Nothing)
                key = Convert.ToString("ResourceFile ") & fullPath
                If (InlineAssignHelper(resourceReader, TryCast(cache__1(key), IResourceReader))) Is Nothing Then
                    If File.Exists(fullPath) Then
                        dependancy = New CacheDependency(fullPath)
                        reader = New BasicResXResourceReader(New ResXResourceReader(fullPath))
                    End If

                End If

                If resourceReader Is Nothing AndAlso reader Is Nothing Then
                    fullPath = Resolve(DefaultPath, Nothing)
                    key = Convert.ToString("ResourceFile ") & fullPath
                    If (InlineAssignHelper(resourceReader, TryCast(cache__1(key), IResourceReader))) Is Nothing Then
                        If File.Exists(fullPath) Then
                            dependancy = New CacheDependency(fullPath)
                            reader = New BasicResXResourceReader(New ResXResourceReader(fullPath))
                        End If

                    End If
                End If
            End If

            If resourceReader Is Nothing AndAlso reader IsNot Nothing Then

                cache__1.Insert(key, InlineAssignHelper(resourceReader, reader), dependancy, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                        New CacheItemRemovedCallback(AddressOf Me.OnRemoved))

            End If
            If blnKeyChanged AndAlso Not (resourceReader Is Nothing OrElse KeysMapping.ContainsKey(origKey.ToLowerInvariant)) Then

                ' reader was found/created after key changed and it is not already in KeyaMapping dictionary, so add it
                KeysMapping.TryAdd(origKey.ToLowerInvariant, key)
            End If

        Catch ex As Exception
            Throw New ApplicationException(String.Format("UpdatableResXResourceProvider::GetResourceCache(CultureInfo) - failed processing file <{0}>", fullPath), ex)

        End Try


        Return resourceReader
    End Function

    ''' <summary>
    ''' CacheItemRemovedCallback to properly dispose of IDisposables.
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    ''' <param name="reason"></param>
    Public Sub OnRemoved(key As String, value As Object, reason As CacheItemRemovedReason)
        Dim disposable As IDisposable = TryCast(value, IDisposable)

        If disposable IsNot Nothing Then
            disposable.Dispose()
        End If

        value = Nothing
    End Sub

    ''' <summary>
    ''' Returns the absolute path to the *.resx corresponding to the request, culture requested.
    ''' </summary>
    ''' <param name="path"></param>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    Private Shared Function Resolve(path As String, culture As CultureInfo) As String
        If culture IsNot Nothing Then
            path = path.Replace(System.IO.Path.GetFileName(path), String.Format("{0}.{1}{2}", System.IO.Path.GetFileNameWithoutExtension(path), CultureInfo.CurrentUICulture, System.IO.Path.GetExtension(path)))
        End If

        Return System.Web.HttpContext.Current.Server.MapPath(path)
    End Function
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function

End Class

''' <summary>
''' Basic implementation of IResourceProvider using the specified BasicResXResourceReader.
''' </summary>
Class BasicResXResourceProvider
    Implements IResourceProvider
    Implements IDisposable

    Private reader As BasicResXResourceReader

    Public Sub New(reader As BasicResXResourceReader)
        Me.reader = reader
    End Sub

    Public ReadOnly Property ResourceReader() As IResourceReader Implements IResourceProvider.ResourceReader
        Get
            Return reader
        End Get
    End Property

    Public Function GetObject(resourceKey As String, culture As CultureInfo) As Object Implements IResourceProvider.GetObject
        For Each entry As DictionaryEntry In ResourceReader
            If resourceKey.Equals(entry.Key, StringComparison.InvariantCultureIgnoreCase) Then
                Return entry.Value
            End If
        Next

        Return Nothing
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If reader IsNot Nothing Then
            reader.Dispose()
        End If

        reader = Nothing
        GC.SuppressFinalize(Me)
    End Sub

End Class

''' <summary>
''' Updatable *.resx provider for global resources.
''' </summary>
Class GlobalResXResourceProvider
    Inherits UpdatableResXResourceProvider
    Private classKey As String

    Friend Sub New(classKey As String)
        Me.classKey = classKey
    End Sub

    Public Overrides ReadOnly Property Path() As String
        Get
            Return String.Format("~/{0}AppGlobalResources/{1}.resx", DefaultStore, ResName)
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultPath() As String
        Get
            Return String.Format("~/App_GlobalResources/{1}.resx", DefaultStore, ResName)
        End Get
    End Property

    Public Overrides ReadOnly Property ResName As String
        Get
            Return classKey
        End Get
    End Property


End Class

''' <summary>
''' Updatable *.resx provider for local resources.
''' </summary>
Class LocalResXResourceProvider
    Inherits UpdatableResXResourceProvider
    Private virtualPath As String

    Public Sub New(virtualPath As String)
        Dim appPath As String = System.Web.HttpContext.Current.Request.ApplicationPath

        If appPath.Length > 1 Then
            virtualPath = virtualPath.Replace(appPath, String.Empty)
        Else
            If virtualPath.StartsWith("/") Then
                virtualPath = virtualPath.Substring(1)
            End If
        End If

        Me.virtualPath = virtualPath
    End Sub

    Public Overrides ReadOnly Property Path() As String
        Get
            Return String.Format("~/{0}{1}/AppLocalResources/{2}.resx", DefaultStore, System.IO.Path.GetDirectoryName(virtualPath), System.IO.Path.GetFileName(virtualPath)).Replace("//", "/")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultPath() As String
        Get
            Return String.Format("~/{1}/App_LocalResources/{2}.resx", DefaultStore, System.IO.Path.GetDirectoryName(virtualPath), System.IO.Path.GetFileName(virtualPath)).Replace("//", "/")
        End Get
    End Property
    Public Overrides ReadOnly Property ResName As String
        Get
            Return System.IO.Path.GetFileName(virtualPath)
        End Get
    End Property

End Class

''' <summary>
''' Basic implementation of IResourceReader using the specified IDictionary.
''' </summary>
Class BasicResXResourceReader
    Implements IResourceReader
    Implements IDisposable

    Private m_resources As IDictionary

    Public Sub New(resources As IDictionary)
        Me.m_resources = resources
    End Sub

    Public Sub New(reader As IResourceReader)
        m_resources = New ConcurrentDictionary(Of String, [Object])()

        For Each entry As DictionaryEntry In reader
            m_resources.Add(TryCast(entry.Key, String).ToLowerInvariant, entry.Value)
        Next
    End Sub

    Public ReadOnly Property Resources() As IDictionary
        Get
            Return m_resources
        End Get
    End Property

    Private Function IResourceReader_GetEnumerator() As IDictionaryEnumerator Implements IResourceReader.GetEnumerator
        Return m_resources.GetEnumerator()
    End Function

    Private Sub IResourceReader_Close() Implements IResourceReader.Close

    End Sub

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return m_resources.GetEnumerator()
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        m_resources = Nothing
        GC.SuppressFinalize(Me)
    End Sub

End Class


