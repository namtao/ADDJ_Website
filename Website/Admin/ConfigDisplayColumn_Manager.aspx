<%@ Page Language="C#" MasterPageFile="~/adminNotAJAX.master" AutoEventWireup="true" Inherits="admin_configDisplayColumn_manager" Title="Untitled Page" Codebehind="configDisplayColumn_manager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

<script src="/JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<script language="javascript">
$(function(){
    $("#selectall").click(function () {
          $('.case').find("input").attr('checked', this.checked);
    });

    $(".case").click(function(){
        if ($(".case").find("//input[checked='checked']").length == $(".case").find("input").length) {
            $("#selectall").attr("checked", "checked");
        } else {
            $("#selectall").removeAttr("checked");
        }
    });
});
</script>

<table width="100%" cellpadding="0" cellspacing="0" border="0">
	<tr valign="top">
		<td>
			<table cellpadding="1" cellspacing="1" border="0" class="text">
				<tr>
					<td></td><td></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr><td style="height: 10px"></td></tr>
	<tr>
		<td style="height: 5px; text-align: left">
			<asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
		</td>
	</tr>
	<tr><td style="height: 10px"></td></tr>
	<tr>
		<td>
			<table cellpadding="0" cellspacing="0" style="width: 100%">
				<tr>
					<td align="center" style="width: 50%; text-align: left">
					</td>
					<td align="right" style="width: 50%; text-align: right">
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr><td style="height: 10px"></td></tr>
	<tr valign="top">
		<td style="text-align: center">
			<asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" Width="100%"
				DataKeyNames="ID" BackColor="White" BorderColor="#16538C" BorderStyle="Solid"
				BorderWidth="1px" CellPadding="3" GridLines="Both" AllowPaging="True" PageSize="50"
				AllowSorting="True" OnRowDataBound="grvView_RowDataBound" OnPageIndexChanging="grvView_PageIndexChanging">
				<RowStyle BackColor="White" ForeColor="Black" />
				<EditRowStyle BackColor="#999999" />
				<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
				<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Right" />
				<HeaderStyle BackColor="#2360A4" Font-Bold="True" ForeColor="White" />
				<AlternatingRowStyle BackColor="#DEEAF3" ForeColor="Black" />
				<Columns>
					<asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
						ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
					<asp:BoundField DataField="TypeDisplay" HeaderText="TypeDisplay" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="FormDisplay" HeaderText="FormDisplay" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="ConfigColumnId" HeaderText="ConfigColumnId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="TenTruyCap" HeaderText="TenTruyCap" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:TemplateField>
						<HeaderTemplate>
							Chọn
							<input type="checkbox" id="selectall" />
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</td>
	</tr>
	<tr><td style="height: 10px"></td></tr>	
</table>
</asp:Content>

