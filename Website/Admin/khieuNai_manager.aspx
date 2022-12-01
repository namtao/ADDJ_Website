<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true" Inherits="admin_khieuNai_manager" Title="Untitled Page" Codebehind="khieuNai_manager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

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
			<table cellpadding="0" cellspacing="0" style="width: 600px">
				<tr>					<td style="text-align: left; width: 250px">Chọn KhuVucId</td>
					<td style="text-align: left">
						<asp:DropDownList ID="ddlKhuVucId" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlKhuVucId_SelectedIndexChanged"></asp:DropDownList>
					</td>
				</tr>
				<tr>					<td style="text-align: left; width: 250px">Chọn DoiTacId</td>
					<td style="text-align: left">
						<asp:DropDownList ID="ddlDoiTacId" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlDoiTacId_SelectedIndexChanged"></asp:DropDownList>
					</td>
				</tr>
				<tr>					<td style="text-align: left; width: 250px">Chọn LoaiKhieuNaiId</td>
					<td style="text-align: left">
						<asp:DropDownList ID="ddlLoaiKhieuNaiId" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlLoaiKhieuNaiId_SelectedIndexChanged"></asp:DropDownList>
					</td>
				</tr>
				<tr>					<td style="text-align: left; width: 250px">Chọn MaTinh</td>
					<td style="text-align: left">
						<asp:DropDownList ID="ddlMaTinh" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlMaTinh_SelectedIndexChanged"></asp:DropDownList>
					</td>
				</tr>
			</table>
		</td>
	</tr>

	<tr>
		<td>
			<table cellpadding="0" cellspacing="0" style="width: 100%">
				<tr>
					<td align="center" style="width: 50%; text-align: left">
						<asp:Button ID="Button1" runat="server" CssClass="button" Text="Thêm mới " OnClick="btThemMoi_Click" Width="80px" />
					</td>
					<td align="right" style="width: 50%; text-align: right">
						<asp:Button ID="Button2" runat="server" CssClass="button" Text="Xóa" OnClick="btDelete_Click" Width="50px" OnClientClick="javascript:{return confirm('Bạn có muốn xóa KhieuNai được chọn?');}" />
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
					<asp:BoundField DataField="MaKhieuNai" HeaderText="MaKhieuNai" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="KhuVucId" HeaderText="KhuVucId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="DoiTacId" HeaderText="DoiTacId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="PhongBanTiepNhanId" HeaderText="PhongBanTiepNhanId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="PhongBanXuLyId" HeaderText="PhongBanXuLyId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="LoaiKhieuNaiId" HeaderText="LoaiKhieuNaiId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="LinhVucChungId" HeaderText="LinhVucChungId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="LinhVucConId" HeaderText="LinhVucConId" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="LoaiKhieuNai" HeaderText="LoaiKhieuNai" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="LinhVucChung" HeaderText="LinhVucChung" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="LinhVucCon" HeaderText="LinhVucCon" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="DoUuTien" HeaderText="DoUuTien" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoThueBao" HeaderText="SoThueBao" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="MaTinh" HeaderText="MaTinh" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="HoTenLienHe" HeaderText="HoTenLienHe" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="DiaChiLienHe" HeaderText="DiaChiLienHe" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SDTLienHe" HeaderText="SDTLienHe" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="DiaDiemXayRa" HeaderText="DiaDiemXayRa" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="ThoiGianXayRa" HeaderText="ThoiGianXayRa" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NoiDungPA" HeaderText="NoiDungPA" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NoiDungCanHoTro" HeaderText="NoiDungCanHoTro" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="TrangThai" HeaderText="TrangThai" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NguoiTiepNhan" HeaderText="NguoiTiepNhan" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="HTTiepNhan" HeaderText="HTTiepNhan" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayTiepNhan" HeaderText="NgayTiepNhan" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayTiepNhanSort" HeaderText="NgayTiepNhanSort" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NguoiTienXuLyCap1" HeaderText="NguoiTienXuLyCap1" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NguoiTienXuLyCap2" HeaderText="NguoiTienXuLyCap2" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NguoiXuLy" HeaderText="NguoiXuLy" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayQuaHan" HeaderText="NgayQuaHan" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayQuaHanSort" HeaderText="NgayQuaHanSort" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayTraLoiKN" HeaderText="NgayTraLoiKN" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayTraLoiKNSort" HeaderText="NgayTraLoiKNSort" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayDongKN" HeaderText="NgayDongKN" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NgayDongKNSort" HeaderText="NgayDongKNSort" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="KQXuLy_SHCV" HeaderText="KQXuLy_SHCV" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:TemplateField HeaderText="KQXuLy_CCT">
						<ItemTemplate>
							<asp:CheckBox ID="chkKQXuLy_CCT" runat="server" Checked='<%# Convert.ToBoolean(Eval("KQXuLy_CCT")) %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="KQXuLy_CSL">
						<ItemTemplate>
							<asp:CheckBox ID="chkKQXuLy_CSL" runat="server" Checked='<%# Convert.ToBoolean(Eval("KQXuLy_CSL")) %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="KQXuLy_PTSL_IR">
						<ItemTemplate>
							<asp:CheckBox ID="chkKQXuLy_PTSL_IR" runat="server" Checked='<%# Convert.ToBoolean(Eval("KQXuLy_PTSL_IR")) %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="KQXuLy_PTSL_Khac" HeaderText="KQXuLy_PTSL_Khac" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="KetQuaXuLy" HeaderText="KetQuaXuLy" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="NoiDungXuLy" HeaderText="NoiDungXuLy" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="GhiChu" HeaderText="GhiChu" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Center" />
					<asp:TemplateField HeaderText="KNHangLoat">
						<ItemTemplate>
							<asp:CheckBox ID="chkKNHangLoat" runat="server" Checked='<%# Convert.ToBoolean(Eval("KNHangLoat")) %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="SoTienKhauTru_TKC" HeaderText="SoTienKhauTru_TKC" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_TKC_SuaDoi" HeaderText="SoTienKhauTru_TKC_SuaDoi" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM1" HeaderText="SoTienKhauTru_KM1" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM1_SuaDoi" HeaderText="SoTienKhauTru_KM1_SuaDoi" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM2" HeaderText="SoTienKhauTru_KM2" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM2_SuaDoi" HeaderText="SoTienKhauTru_KM2_SuaDoi" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM3" HeaderText="SoTienKhauTru_KM3" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM3_SuaDoi" HeaderText="SoTienKhauTru_KM3_SuaDoi" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM4" HeaderText="SoTienKhauTru_KM4" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM4_SuaDoi" HeaderText="SoTienKhauTru_KM4_SuaDoi" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM5" HeaderText="SoTienKhauTru_KM5" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:BoundField DataField="SoTienKhauTru_KM5_SuaDoi" HeaderText="SoTienKhauTru_KM5_SuaDoi" DataFormatString="{0:0,0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center" />
					<asp:TemplateField HeaderText="IsLuuKhieuNai">
						<ItemTemplate>
							<asp:CheckBox ID="chkIsLuuKhieuNai" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsLuuKhieuNai")) %>' />
						</ItemTemplate>
					</asp:TemplateField>
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
	<tr>
		<td>
			<table cellpadding="0" cellspacing="0" style="width: 100%">
				<tr>
					<td align="center" style="width: 50%; text-align: left">
						<asp:Button ID="Button3" runat="server" CssClass="button" Text="Thêm mới " OnClick="btThemMoi_Click" Width="80px" />
					</td>
					<td align="right" style="width: 50%; text-align: right">
						<asp:Button ID="Button4" runat="server" CssClass="button" Text="Xóa" OnClick="btDelete_Click" Width="50px" OnClientClick="javascript:{return confirm('Bạn có muốn xóa KhieuNai được chọn?');}" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr><td style="height: 10px"></td></tr>
</table>
</asp:Content>

