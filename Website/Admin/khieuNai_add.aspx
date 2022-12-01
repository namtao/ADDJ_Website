<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true" Inherits="admin_khieuNai_add" Title="Untitled Page" Codebehind="khieuNai_add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
<link href="../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script src="../JS/plugin/jquery.datepick.js" type="text/javascript"></script>
<script src="../JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>

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
				Chọn KhuVucId
			</td>
			<td style="width:20%; text-align:left">
				<asp:DropDownList ID="ddlKhuVucId" runat="server">
				</asp:DropDownList>
			</td>
			<td style="width:20%; text-align:left"></td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>
		<tr>
			<td style="width:15%; text-align:right; padding-right:10px">
				Chọn DoiTacId
			</td>
			<td style="width:20%; text-align:left">
				<asp:DropDownList ID="ddlDoiTacId" runat="server">
				</asp:DropDownList>
			</td>
			<td style="width:20%; text-align:left"></td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>
		<tr>
			<td style="width:15%; text-align:right; padding-right:10px">
				Chọn LoaiKhieuNaiId
			</td>
			<td style="width:20%; text-align:left">
				<asp:DropDownList ID="ddlLoaiKhieuNaiId" runat="server">
				</asp:DropDownList>
			</td>
			<td style="width:20%; text-align:left"></td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>
		<tr>
			<td style="width:15%; text-align:right; padding-right:10px">
				Chọn MaTinh
			</td>
			<td style="width:20%; text-align:left">
				<asp:DropDownList ID="ddlMaTinh" runat="server">
				</asp:DropDownList>
			</td>
			<td style="width:20%; text-align:left"></td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				MaKhieuNai
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtMaKhieuNai" runat="server" Text="" MaxLength="20" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				PhongBanTiepNhanId
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtPhongBanTiepNhanId" runat="server" Text="" Width="450px"></asp:TextBox>
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
				LinhVucChungId
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtLinhVucChungId" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				LinhVucConId
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtLinhVucConId" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				LoaiKhieuNai
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtLoaiKhieuNai" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				LinhVucChung
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtLinhVucChung" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				LinhVucCon
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtLinhVucCon" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				DoUuTien
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtDoUuTien" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoThueBao
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoThueBao" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				HoTenLienHe
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtHoTenLienHe" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				DiaChiLienHe
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtDiaChiLienHe" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SDTLienHe
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSDTLienHe" runat="server" Text="" MaxLength="20" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				DiaDiemXayRa
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtDiaDiemXayRa" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				ThoiGianXayRa
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtThoiGianXayRa" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NoiDungPA
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNoiDungPA" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NoiDungCanHoTro
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNoiDungCanHoTro" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				TrangThai
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtTrangThai" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NguoiTiepNhan
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNguoiTiepNhan" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				HTTiepNhan
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtHTTiepNhan" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NgayTiepNhan
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayTiepNhan" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NgayTiepNhanSort
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayTiepNhanSort" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NguoiTienXuLyCap1
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNguoiTienXuLyCap1" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NguoiTienXuLyCap2
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNguoiTienXuLyCap2" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
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
				NgayQuaHan
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayQuaHan" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NgayQuaHanSort
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayQuaHanSort" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NgayTraLoiKN
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayTraLoiKN" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NgayTraLoiKNSort
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayTraLoiKNSort" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NgayDongKN
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayDongKN" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NgayDongKNSort
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNgayDongKNSort" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				KQXuLy_SHCV
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtKQXuLy_SHCV" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				KQXuLy_CCT
			</td>
			<td style="width:20%; text-align:left">
				<asp:CheckBox ID="chkKQXuLy_CCT" runat="server" Checked="false" Text="KQXuLy_CCT" />
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				KQXuLy_CSL
			</td>
			<td style="width:20%; text-align:left">
				<asp:CheckBox ID="chkKQXuLy_CSL" runat="server" Checked="false" Text="KQXuLy_CSL" />
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				KQXuLy_PTSL_IR
			</td>
			<td style="width:20%; text-align:left">
				<asp:CheckBox ID="chkKQXuLy_PTSL_IR" runat="server" Checked="false" Text="KQXuLy_PTSL_IR" />
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				KQXuLy_PTSL_Khac
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtKQXuLy_PTSL_Khac" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				KetQuaXuLy
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtKetQuaXuLy" runat="server" Text="" TextMode="MultiLine" Height="100px" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				NoiDungXuLy
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtNoiDungXuLy" runat="server" Text="" TextMode="MultiLine" Height="100px" Width="450px"></asp:TextBox>
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
				<asp:TextBox ID="txtGhiChu" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				KNHangLoat
			</td>
			<td style="width:20%; text-align:left">
				<asp:CheckBox ID="chkKNHangLoat" runat="server" Checked="false" Text="KNHangLoat" />
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_TKC
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_TKC" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_TKC_SuaDoi
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_TKC_SuaDoi" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM1
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM1" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM1_SuaDoi
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM1_SuaDoi" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM2
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM2" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM2_SuaDoi
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM2_SuaDoi" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM3
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM3" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM3_SuaDoi
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM3_SuaDoi" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM4
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM4" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM4_SuaDoi
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM4_SuaDoi" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM5
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM5" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				SoTienKhauTru_KM5_SuaDoi
			</td>
			<td style="width:20%; text-align:left">
				<asp:TextBox ID="txtSoTienKhauTru_KM5_SuaDoi" runat="server" Text="" Width="450px"></asp:TextBox>
			</td>
			<td style="width:20%; text-align:left">
			</td>
			<td style="width:15%"></td>
		</tr>
		<tr><td style="height:10px"></td></tr>  
		<tr>  
			<td style="width:15%; text-align:right; padding-right:10px">
				IsLuuKhieuNai
			</td>
			<td style="width:20%; text-align:left">
				<asp:CheckBox ID="chkIsLuuKhieuNai" runat="server" Checked="false" Text="IsLuuKhieuNai" />
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
<script>$("#<%=txtNgayTiepNhan.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
$("#<%=txtNgayQuaHan.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
$("#<%=txtNgayTraLoiKN.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
$("#<%=txtNgayDongKN.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
</script></asp:Content>

