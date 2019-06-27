<%@ Page Language="VB" AutoEventWireup="false"	MasterPageFile="~/Site.master" Inherits="SprueAdmin.Admin_DeviceStatus" Codebehind="DeviceStatus.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

		<div id="content">
			<div id="box">

			<h3 id="notes">Device Status</h3>
			
			<asp:Panel ID="pnlDevice" runat="server" defaultbutton="btnSearch">		
			
			    <table>
				   
				    <tr>
					    <td style="width: 200px; height: 25px" class="txtBoxLight"><label class="PageText">Mac Address:</label></td>
					    <td style="height: 25px;" class="txtBoxLight"><asp:textbox runat="server" id="txtMac" MaxLength="12"></asp:textbox></td>
				    </tr>

				</table>
            
                <asp:Button ID="btnSearch" runat="server" class="btnStandard" Text="Search" style="margin:10px 0 20px 10px;"/>			
                <asp:label runat="server" ID="lblResults" />
                    
			</asp:Panel>
		
            </div>
	
	

	</div>
  </asp:Content>

