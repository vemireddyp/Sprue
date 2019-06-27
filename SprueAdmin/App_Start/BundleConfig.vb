Imports System.Web.Optimization

Public Class BundleConfig
    Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)
        bundles.Add(New StyleBundle("~/Content/Global").Include("~/common/css/Global/*.css"))

    End Sub

End Class
