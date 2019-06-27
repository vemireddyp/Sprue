Imports Telerik.Web.UI

Public Interface IGridPage
    ReadOnly Property targetGrids As Dictionary(Of String, RadGrid)

    ReadOnly Property ajaxManager As RadAjaxManager

    ReadOnly Property targetGrid As RadGrid

    Function getFiltersDef() As Dictionary(Of String, Control)

    Function getGridFilterControlByID(Of CtlType As Control)(ByVal sctlID As String) As CtlType
    Function getGridFilterControlBySaveID(Of CtlType As Control)(ByVal sctlID As String) As CtlType

    Function DoSearch(ByVal gridID As String) As DataView


End Interface
