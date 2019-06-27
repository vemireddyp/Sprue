<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/TelerikCtlContent.master" Inherits="SprueAdmin.Admin_Notes" CodeBehind="Notes.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" runat="server" ChildrenAsTriggers="true" ContentPlaceHolderID="ContentBody">
    <style type="text/css">
        .RadGrid_Default .rgMasterTable .rgHeader {
            background-color: #333 !important;
            color: white;
        }

        .RadGrid_Default .rgHeader, .RadGrid_Default th.rgResizeCol, .RadGrid_Default .rgHeaderWrapper {
            background-image: none;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="grdNotes" style="padding-right: 15px; padding-top: 15px;" class="col-md-12 rwPadLR0 ">
                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgNotes" AllowPaging="true" MasterTableView-NoMasterRecordsText="<%$Resources:PageGlobalResources,NoRecords %>" Font-Overline="false" ForeColor="#333333" AllowSorting="true" PageSize="20" AllowFilteringByColumn="false" MasterTableView-ShowHeadersWhenNoRecords="true"
                     MasterTableView-DataKeyNames="AdminNoteID" AutoGenerateColumns="false" GridLines="None">
                    <FooterStyle BackColor="#333333" Font-Bold="False" ForeColor="White" />
                    <GroupingSettings CaseSensitive="false" />
                    <ItemStyle BackColor="#FFFFFF" ForeColor="Black" BorderStyle="None" />
                    <AlternatingItemStyle BackColor="#F5F5F5" ForeColor="Black" />
                    <PagerStyle HorizontalAlign="Center" CssClass="dataPager" PagerTextFormat="<%$Resources:PageGlobalResources,GridPagerTextFormat %>"
                        PageSizeLabelText="<%$Resources:PageGlobalResources,GridPageSizeText %>" NextPageToolTip="<%$Resources:PageGlobalResources,GridNextPageTooltip %>"
                        PrevPageToolTip="<%$Resources:PageGlobalResources,GridPreviousPageTooltip %>" LastPageToolTip="<%$Resources:PageGlobalResources,GridLastPageTooltip %>"
                        FirstPageToolTip="<%$Resources:PageGlobalResources,GridFirstPageTooltip %>" NextPagesToolTip="<%$Resources:PageGlobalResources,GridNextPagesTooltip %>"
                        PrevPagesToolTip="<%$Resources:PageGlobalResources,GridPreviousPagesTooltip %>" />
                    <HeaderStyle BackColor="#333" Font-Bold="False" ForeColor="white" Height="0" Width="200" />
                    <SortingSettings EnableSkinSortStyles="false" SortToolTip="<%$Resources:PageGlobalResources,GridSortTooltip %>" />
                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" CssClass="borderBlack">
                        <Columns>
                            <telerik:GridBoundColumn DataField="AdminNoteID" UniqueName="AdminNoteID" HeaderText="<%$Resources:PageGlobalResources,NoteIDHeader %>" SortExpression="AdminNoteID" />
                            <telerik:GridBoundColumn DataField="DateEntered" UniqueName="DateEntered" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="<%$Resources:PageGlobalResources,Date %>" SortExpression="DateEntered" />
                            <telerik:GridBoundColumn DataField="Subject" UniqueName="Subject" HeaderText="<%$Resources:PageGlobalResources,SubjectHeader %>" SortExpression="Subject" />
                            <telerik:GridBoundColumn DataField="EnteredByUser" UniqueName="EnteredByUser" HeaderText="<%$Resources:PageGlobalResources,EnteredByHeader %>" SortExpression="EnteredByUser" />
                            <telerik:GridButtonColumn ButtonType="LinkButton" Text="<%$Resources:PageGlobalResources,ViewButton %>" CommandName="Select" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>

                <div class="col-md-12">
                    <asp:Button ID="btnAddNote" runat="server" CssClass="btn btn-warning btnLogOff pull-right rwMarTB15" Text="<%$Resources:PageGlobalResources,AddNoteButton %>" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
