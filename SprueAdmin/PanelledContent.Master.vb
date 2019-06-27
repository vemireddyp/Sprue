Public Class PanelledContent
    Inherits System.Web.UI.MasterPage

	Public ReadOnly Property p_collapserCloseText As String
		Get
			Return GetGlobalResourceObject("PageGlobalResources", "CollapserCloseText")
		End Get
	End Property

	Public ReadOnly Property p_collapserOpenText As String
		Get
			Return GetGlobalResourceObject("PageGlobalResources", "CollapserOpenText")
		End Get
	End Property

	Protected Overrides Sub OnPreRender(e As EventArgs)
		MyBase.OnPreRender(e)

		If Page.IsPostBack Then
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setupPanelCollapsers", "setupCollapsers();", True)
		End If
	End Sub
End Class