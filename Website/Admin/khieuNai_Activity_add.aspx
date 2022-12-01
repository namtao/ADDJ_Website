<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true" Inherits="admin_khieuNai_Activity_add" Title="Untitled Page" Codebehind="khieuNai_Activity_add.aspx.cs" %>

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
				Chọn KhieuNaiId
			</td>
			<td style="width:20%; text-align:left">
				<asp:DropDownList ID="ddlKhieuNaiId" runat="server">
				</asp:DropDownList>
			</td>
			<td style="width:20%; text-align:left"></td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				PhongBanXuLyTruocId
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtPhongBanXuLyTruocId" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NguoiXuLyTruoc
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNguoiXuLyTruoc" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				PhongBanXuLyId
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtPhongBanXuLyId" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NguoiXuLy
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNguoiXuLy" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				HanhDong
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtHanhDong" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				GhiChu
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtGhiChu" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				IsCurrent
			</td>
			<td style="width:20%; text-align:left">
				<asp:CheckBox ID="chkIsCurrent" runat="server" Checked="false" Text="IsCurrent" />
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				ActivityTruoc
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtActivityTruoc" runat="server" Text="" Width="450px"></asp:TextBox>
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

