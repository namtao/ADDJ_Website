using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

	public partial class admin_khieuNai_add : System.Web.UI.Page
	{
	protected void Page_Load(object sender, EventArgs e)
	{
		LoginAdmin.IsLoginAdmin();
		if (!UserRightImpl.CheckRightAdminnistrator().UserRead)
		{
			Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
			return;
		}
		if (!IsPostBack)
		{
			lblMsg.Text ="";
			BindDropDownList();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				EditData();
			}
		}
	}
	private void BindDropDownList() 
	{
		try
		{
			var khuVucIdObj = ServiceFactory.GetInstanceDoiTac().GetList();
			if (khuVucIdObj != null && khuVucIdObj.Count > 0)
			{
				ddlKhuVucId.DataSource = khuVucIdObj;
				ddlKhuVucId.DataTextField = "Name";
				ddlKhuVucId.DataValueField = "Id";
				ddlKhuVucId.DataBind();
			}

			var doiTacIdObj = ServiceFactory.GetInstanceDoiTac().GetList();
			if (doiTacIdObj != null && doiTacIdObj.Count > 0)
			{
				ddlDoiTacId.DataSource = doiTacIdObj;
				ddlDoiTacId.DataTextField = "Name";
				ddlDoiTacId.DataValueField = "Id";
				ddlDoiTacId.DataBind();
			}

			var loaiKhieuNaiIdObj = ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
			if (loaiKhieuNaiIdObj != null && loaiKhieuNaiIdObj.Count > 0)
			{
				ddlLoaiKhieuNaiId.DataSource = loaiKhieuNaiIdObj;
				ddlLoaiKhieuNaiId.DataTextField = "Name";
				ddlLoaiKhieuNaiId.DataValueField = "Id";
				ddlLoaiKhieuNaiId.DataBind();
			}

			var maTinhObj = ServiceFactory.GetInstanceTinh().GetList();
			if (maTinhObj != null && maTinhObj.Count > 0)
			{
				ddlMaTinh.DataSource = maTinhObj;
				ddlMaTinh.DataTextField = "Name";
				ddlMaTinh.DataValueField = "Id";
				ddlMaTinh.DataBind();
			}

		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Utility.UrlRoot + Config.PathError, false);
			return;
		}
	}

	private void EditData()
	{
		try
		{
			var obj = ServiceFactory.GetInstanceKhieuNai();
			KhieuNaiInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData khieuNai_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
			else
			{
				ddlKhuVucId.SelectedValue = item.KhuVucId.ToString();
				ddlDoiTacId.SelectedValue = item.DoiTacId.ToString();
				ddlLoaiKhieuNaiId.SelectedValue = item.LoaiKhieuNaiId.ToString();
				ddlMaTinh.SelectedValue = item.MaTinh.ToString();
				txtMaKhieuNai.Text = item.MaKhieuNai.ToString();
				txtPhongBanTiepNhanId.Text = item.PhongBanTiepNhanId.ToString();
				txtPhongBanXuLyId.Text = item.PhongBanXuLyId.ToString();
				txtLinhVucChungId.Text = item.LinhVucChungId.ToString();
				txtLinhVucConId.Text = item.LinhVucConId.ToString();
				txtLoaiKhieuNai.Text = item.LoaiKhieuNai.ToString();
				txtLinhVucChung.Text = item.LinhVucChung.ToString();
				txtLinhVucCon.Text = item.LinhVucCon.ToString();
				txtDoUuTien.Text = item.DoUuTien.ToString();
				txtSoThueBao.Text = item.SoThueBao.ToString();
				txtHoTenLienHe.Text = item.HoTenLienHe.ToString();
				txtDiaChiLienHe.Text = item.DiaChiLienHe.ToString();
				txtSDTLienHe.Text = item.SDTLienHe.ToString();
				txtDiaDiemXayRa.Text = item.DiaDiemXayRa.ToString();
				txtThoiGianXayRa.Text = item.ThoiGianXayRa.ToString();
				txtNoiDungPA.Text = item.NoiDungPA.ToString();
				txtNoiDungCanHoTro.Text = item.NoiDungCanHoTro.ToString();
				txtTrangThai.Text = item.TrangThai.ToString();
				txtNguoiTiepNhan.Text = item.NguoiTiepNhan.ToString();
				txtHTTiepNhan.Text = item.HTTiepNhan.ToString();
				txtNgayTiepNhan.Text = item.NgayTiepNhan.ToString("dd/MM/yyyy");
				txtNgayTiepNhanSort.Text = item.NgayTiepNhanSort.ToString();
				txtNguoiTienXuLyCap1.Text = item.NguoiTienXuLyCap1.ToString();
				txtNguoiTienXuLyCap2.Text = item.NguoiTienXuLyCap2.ToString();
				txtNguoiXuLy.Text = item.NguoiXuLy.ToString();
				txtNgayQuaHan.Text = item.NgayQuaHan.ToString("dd/MM/yyyy");
				txtNgayQuaHanSort.Text = item.NgayQuaHanSort.ToString();
				txtNgayTraLoiKN.Text = item.NgayTraLoiKN.ToString("dd/MM/yyyy");
				txtNgayTraLoiKNSort.Text = item.NgayTraLoiKNSort.ToString();
				txtNgayDongKN.Text = item.NgayDongKN.ToString("dd/MM/yyyy");
				txtNgayDongKNSort.Text = item.NgayDongKNSort.ToString();
				txtKQXuLy_SHCV.Text = item.KQXuLy_SHCV.ToString();
				chkKQXuLy_CCT.Checked = item.KQXuLy_CCT;
				//txtKQXuLy_CCT.Text = item.KQXuLy_CCT.ToString();
				chkKQXuLy_CSL.Checked = item.KQXuLy_CSL;
				//txtKQXuLy_CSL.Text = item.KQXuLy_CSL.ToString();
				chkKQXuLy_PTSL_IR.Checked = item.KQXuLy_PTSL_IR;
				//txtKQXuLy_PTSL_IR.Text = item.KQXuLy_PTSL_IR.ToString();
				txtKQXuLy_PTSL_Khac.Text = item.KQXuLy_PTSL_Khac.ToString();
				txtKetQuaXuLy.Text = item.KetQuaXuLy.ToString();
				txtNoiDungXuLy.Text = item.NoiDungXuLy.ToString();
				txtGhiChu.Text = item.GhiChu.ToString();
				chkKNHangLoat.Checked = item.KNHangLoat;
				//txtKNHangLoat.Text = item.KNHangLoat.ToString();
				txtSoTienKhauTru_TKC.Text = item.SoTienKhauTru_TKC.ToString();
				txtSoTienKhauTru_TKC_SuaDoi.Text = item.SoTienKhauTru_TKC_SuaDoi.ToString();
				txtSoTienKhauTru_KM1.Text = item.SoTienKhauTru_KM1.ToString();
				txtSoTienKhauTru_KM1_SuaDoi.Text = item.SoTienKhauTru_KM1_SuaDoi.ToString();
				txtSoTienKhauTru_KM2.Text = item.SoTienKhauTru_KM2.ToString();
				txtSoTienKhauTru_KM2_SuaDoi.Text = item.SoTienKhauTru_KM2_SuaDoi.ToString();
				txtSoTienKhauTru_KM3.Text = item.SoTienKhauTru_KM3.ToString();
				txtSoTienKhauTru_KM3_SuaDoi.Text = item.SoTienKhauTru_KM3_SuaDoi.ToString();
				txtSoTienKhauTru_KM4.Text = item.SoTienKhauTru_KM4.ToString();
				txtSoTienKhauTru_KM4_SuaDoi.Text = item.SoTienKhauTru_KM4_SuaDoi.ToString();
				txtSoTienKhauTru_KM5.Text = item.SoTienKhauTru_KM5.ToString();
				txtSoTienKhauTru_KM5_SuaDoi.Text = item.SoTienKhauTru_KM5_SuaDoi.ToString();
				chkIsLuuKhieuNai.Checked = item.IsLuuKhieuNai;
				//txtIsLuuKhieuNai.Text = item.IsLuuKhieuNai.ToString();
			}
		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Utility.UrlRoot + Config.PathError, false);
			return;
		}
	}
	protected void btSubmit_Click(object sender, EventArgs e)
	{
		if (!UserRightImpl.CheckRightAdminnistrator().UserEdit)
		{
			Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
			return;
		}

		try
		{
			var obj = ServiceFactory.GetInstanceKhieuNai();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				try
				{
					int idEdit = int.Parse(Request.QueryString["ID"]);
					KhieuNaiInfo item = obj.GetInfo(idEdit);

					if (item == null)
					{
						Utility.LogEvent("Function khieuNai_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
						Response.Redirect(Utility.UrlRoot + Config.PathError, false);
						return;
					}

					item.KhuVucId = Convert.ToInt32(ddlKhuVucId.SelectedValue);
					item.DoiTacId = Convert.ToInt32(ddlDoiTacId.SelectedValue);
					item.LoaiKhieuNaiId = Convert.ToInt32(ddlLoaiKhieuNaiId.SelectedValue);
					item.MaTinh = ddlMaTinh.SelectedValue.ToString();
				try 
				{ 
					item.MaKhieuNai = txtMaKhieuNai.Text.Trim();
					item.PhongBanTiepNhanId = Convert.ToInt32(txtPhongBanTiepNhanId.Text.Trim());
					item.PhongBanXuLyId = Convert.ToInt32(txtPhongBanXuLyId.Text.Trim());
					item.LinhVucChungId = Convert.ToInt32(txtLinhVucChungId.Text.Trim());
					item.LinhVucConId = Convert.ToInt32(txtLinhVucConId.Text.Trim());
					item.LoaiKhieuNai = txtLoaiKhieuNai.Text.Trim();
					item.LinhVucChung = txtLinhVucChung.Text.Trim();
					item.LinhVucCon = txtLinhVucCon.Text.Trim();
					item.DoUuTien = Convert.ToInt16(txtDoUuTien.Text.Trim());
					item.SoThueBao = Convert.ToInt64(txtSoThueBao.Text.Trim());
					item.HoTenLienHe = txtHoTenLienHe.Text.Trim();
					item.DiaChiLienHe = txtDiaChiLienHe.Text.Trim();
					item.SDTLienHe = txtSDTLienHe.Text.Trim();
					item.DiaDiemXayRa = txtDiaDiemXayRa.Text.Trim();
					item.ThoiGianXayRa = txtThoiGianXayRa.Text.Trim();
					item.NoiDungPA = txtNoiDungPA.Text.Trim();
					item.NoiDungCanHoTro = txtNoiDungCanHoTro.Text.Trim();
					item.TrangThai = Convert.ToInt16(txtTrangThai.Text.Trim());
					item.NguoiTiepNhan = txtNguoiTiepNhan.Text.Trim();
					item.HTTiepNhan = Convert.ToInt16(txtHTTiepNhan.Text.Trim());
					item.NgayTiepNhan = Convert.ToDateTime(txtNgayTiepNhan.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayTiepNhanSort = Convert.ToInt32(txtNgayTiepNhanSort.Text.Trim());
					item.NguoiTienXuLyCap1 = txtNguoiTienXuLyCap1.Text.Trim();
					item.NguoiTienXuLyCap2 = txtNguoiTienXuLyCap2.Text.Trim();
					item.NguoiXuLy = txtNguoiXuLy.Text.Trim();
					item.NgayQuaHan = Convert.ToDateTime(txtNgayQuaHan.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayQuaHanSort = Convert.ToInt32(txtNgayQuaHanSort.Text.Trim());
					item.NgayTraLoiKN = Convert.ToDateTime(txtNgayTraLoiKN.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayTraLoiKNSort = Convert.ToInt32(txtNgayTraLoiKNSort.Text.Trim());
					item.NgayDongKN = Convert.ToDateTime(txtNgayDongKN.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayDongKNSort = Convert.ToInt32(txtNgayDongKNSort.Text.Trim());
					item.KQXuLy_SHCV = txtKQXuLy_SHCV.Text.Trim();
					item.KQXuLy_CCT = chkKQXuLy_CCT.Checked;
					item.KQXuLy_CSL = chkKQXuLy_CSL.Checked;
					item.KQXuLy_PTSL_IR = chkKQXuLy_PTSL_IR.Checked;
					item.KQXuLy_PTSL_Khac = txtKQXuLy_PTSL_Khac.Text.Trim();
					item.KetQuaXuLy = txtKetQuaXuLy.Text.Trim();
					item.NoiDungXuLy = txtNoiDungXuLy.Text.Trim();
					item.GhiChu = txtGhiChu.Text.Trim();
					item.KNHangLoat = chkKNHangLoat.Checked;
					item.SoTienKhauTru_TKC = Convert.ToDecimal(txtSoTienKhauTru_TKC.Text.Trim());
					item.SoTienKhauTru_TKC_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_TKC_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM1 = Convert.ToDecimal(txtSoTienKhauTru_KM1.Text.Trim());
					item.SoTienKhauTru_KM1_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM1_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM2 = Convert.ToDecimal(txtSoTienKhauTru_KM2.Text.Trim());
					item.SoTienKhauTru_KM2_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM2_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM3 = Convert.ToDecimal(txtSoTienKhauTru_KM3.Text.Trim());
					item.SoTienKhauTru_KM3_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM3_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM4 = Convert.ToDecimal(txtSoTienKhauTru_KM4.Text.Trim());
					item.SoTienKhauTru_KM4_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM4_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM5 = Convert.ToDecimal(txtSoTienKhauTru_KM5.Text.Trim());
					item.SoTienKhauTru_KM5_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM5_SuaDoi.Text.Trim());
					item.IsLuuKhieuNai = chkIsLuuKhieuNai.Checked;
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

					obj.Update(item);
				}
				catch(Exception ex)
				{
					Utility.LogEvent(ex);
					Response.Redirect(Utility.UrlRoot + Config.PathError, false);
					return;
				}
			}
			else
			{
				var item = new KhieuNaiInfo();

				item.KhuVucId = Convert.ToInt32(ddlKhuVucId.SelectedValue);
				item.DoiTacId = Convert.ToInt32(ddlDoiTacId.SelectedValue);
				item.LoaiKhieuNaiId = Convert.ToInt32(ddlLoaiKhieuNaiId.SelectedValue);
				item.MaTinh = ddlMaTinh.SelectedValue;
				try 
				{ 
					item.MaKhieuNai = txtMaKhieuNai.Text.Trim();
					item.PhongBanTiepNhanId = Convert.ToInt32(txtPhongBanTiepNhanId.Text.Trim());
					item.PhongBanXuLyId = Convert.ToInt32(txtPhongBanXuLyId.Text.Trim());
					item.LinhVucChungId = Convert.ToInt32(txtLinhVucChungId.Text.Trim());
					item.LinhVucConId = Convert.ToInt32(txtLinhVucConId.Text.Trim());
					item.LoaiKhieuNai = txtLoaiKhieuNai.Text.Trim();
					item.LinhVucChung = txtLinhVucChung.Text.Trim();
					item.LinhVucCon = txtLinhVucCon.Text.Trim();
					item.DoUuTien = Convert.ToInt16(txtDoUuTien.Text.Trim());
					item.SoThueBao = Convert.ToInt64(txtSoThueBao.Text.Trim());
					item.HoTenLienHe = txtHoTenLienHe.Text.Trim();
					item.DiaChiLienHe = txtDiaChiLienHe.Text.Trim();
					item.SDTLienHe = txtSDTLienHe.Text.Trim();
					item.DiaDiemXayRa = txtDiaDiemXayRa.Text.Trim();
					item.ThoiGianXayRa = txtThoiGianXayRa.Text.Trim();
					item.NoiDungPA = txtNoiDungPA.Text.Trim();
					item.NoiDungCanHoTro = txtNoiDungCanHoTro.Text.Trim();
					item.TrangThai = Convert.ToInt16(txtTrangThai.Text.Trim());
					item.NguoiTiepNhan = txtNguoiTiepNhan.Text.Trim();
					item.HTTiepNhan = Convert.ToInt16(txtHTTiepNhan.Text.Trim());
					item.NgayTiepNhan = Convert.ToDateTime(txtNgayTiepNhan.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayTiepNhanSort = Convert.ToInt32(txtNgayTiepNhanSort.Text.Trim());
					item.NguoiTienXuLyCap1 = txtNguoiTienXuLyCap1.Text.Trim();
					item.NguoiTienXuLyCap2 = txtNguoiTienXuLyCap2.Text.Trim();
					item.NguoiXuLy = txtNguoiXuLy.Text.Trim();
					item.NgayQuaHan = Convert.ToDateTime(txtNgayQuaHan.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayQuaHanSort = Convert.ToInt32(txtNgayQuaHanSort.Text.Trim());
					item.NgayTraLoiKN = Convert.ToDateTime(txtNgayTraLoiKN.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayTraLoiKNSort = Convert.ToInt32(txtNgayTraLoiKNSort.Text.Trim());
					item.NgayDongKN = Convert.ToDateTime(txtNgayDongKN.Text.Trim(), new CultureInfo("vi-vn"));
					item.NgayDongKNSort = Convert.ToInt32(txtNgayDongKNSort.Text.Trim());
					item.KQXuLy_SHCV = txtKQXuLy_SHCV.Text.Trim();
					item.KQXuLy_CCT = chkKQXuLy_CCT.Checked;
					item.KQXuLy_CSL = chkKQXuLy_CSL.Checked;
					item.KQXuLy_PTSL_IR = chkKQXuLy_PTSL_IR.Checked;
					item.KQXuLy_PTSL_Khac = txtKQXuLy_PTSL_Khac.Text.Trim();
					item.KetQuaXuLy = txtKetQuaXuLy.Text.Trim();
					item.NoiDungXuLy = txtNoiDungXuLy.Text.Trim();
					item.GhiChu = txtGhiChu.Text.Trim();
					item.KNHangLoat = chkKNHangLoat.Checked;
					item.SoTienKhauTru_TKC = Convert.ToDecimal(txtSoTienKhauTru_TKC.Text.Trim());
					item.SoTienKhauTru_TKC_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_TKC_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM1 = Convert.ToDecimal(txtSoTienKhauTru_KM1.Text.Trim());
					item.SoTienKhauTru_KM1_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM1_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM2 = Convert.ToDecimal(txtSoTienKhauTru_KM2.Text.Trim());
					item.SoTienKhauTru_KM2_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM2_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM3 = Convert.ToDecimal(txtSoTienKhauTru_KM3.Text.Trim());
					item.SoTienKhauTru_KM3_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM3_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM4 = Convert.ToDecimal(txtSoTienKhauTru_KM4.Text.Trim());
					item.SoTienKhauTru_KM4_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM4_SuaDoi.Text.Trim());
					item.SoTienKhauTru_KM5 = Convert.ToDecimal(txtSoTienKhauTru_KM5.Text.Trim());
					item.SoTienKhauTru_KM5_SuaDoi = Convert.ToDecimal(txtSoTienKhauTru_KM5_SuaDoi.Text.Trim());
					item.IsLuuKhieuNai = chkIsLuuKhieuNai.Checked;
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

				obj.Add(item);
				}

				Response.Redirect("khieuNai_manager.aspx", false);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
		}
	}

