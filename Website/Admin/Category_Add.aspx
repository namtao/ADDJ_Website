<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true" Inherits="admin_category_add" Title="Untitled Page" Codebehind="category_add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

<script language="javascript">
	function validForm()
	{
		var name = document.getElementById("ctl00_ContentPlaceHolder_Main_txtName");
		if(name.value.trim() == "")
		{
			alert('Error');
			name.value = "";
			name.focus();
			return false;
		}
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
				Parent
			</td>
			<td style="width:20%; text-align:left">
                <asp:DropDownList ID="ddlParrent" runat="server">
                </asp:DropDownList>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				Tên danh mục
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtName" runat="server" MaxLength="200" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				Mô tả
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtDescription" runat="server" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				Sử dụng
			</td>
			<td style="width:20%; text-align:left">
				<asp:CheckBox ID="chkStatus" runat="server" />
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>
		    <td></td>
			<td style="text-align: left;">
				<asp:Button ID="btSubmit" CssClass="button" runat="server" Text="Lưu" OnClick="btSubmit_Click" />
			</td>
			<td></td><td></td></tr><tr><td style="height:10px"></td></tr>
		<tr><td style="height:25px"></td></tr>
	</table>
</asp:Content>

