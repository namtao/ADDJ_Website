<%@ Page Language="C#" MasterPageFile="~/adminNotAJAX.master" AutoEventWireup="true" Inherits="admin_configColumn_add" Title="Untitled Page" Codebehind="configColumn_add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

<script language="javascript">
	function validForm()
	{
		
		return true;
}
</script>

	<table border="0" cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td style="height:25px">
			</td>
		</tr>  
		<tr>
			<td></td>
			<td colspan="4" align="left">
				<asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
			</td>
		</tr>
		<tr><td style="height:5px"></td></tr>
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				TypeDisplay
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtTypeDisplay" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				ColumnName
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtColumnName" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				DisplayName
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtDisplayName" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>
			<td></td>
			<td style="text-align:left">
				<asp:Button ID="btSubmit" CssClass="button" runat="server" Text="Luu" OnClick="btSubmit_Click"  OnClientClick="return validForm();"/>
			</td>
			<td></td><td></td></tr><tr><td style="height:10px"></td></tr>
		<tr><td style="height:25px"></td></tr>
	</table>
<script></script></asp:Content>

