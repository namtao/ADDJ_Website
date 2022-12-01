using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.ChiTietKhieuNai
{
    public partial class XuLyKhieuNai : AppCode.PageBase
    {
        private int KhieuNaiId = 0;
        private int Mode = 0;
        private int ArchiveId;
        //protected AdminInfo userlogin;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Utility.IsInteger(Request.QueryString["MaKN"])) KhieuNaiId = Convert.ToInt32(Request.QueryString["MaKN"]);

                ArchiveId = ConvertUtility.ToInt32(Request.QueryString["archive"]);

                hdKhieuNaiId.Value = KhieuNaiId.ToString();
                ucChiTietKhieuNai.KhieuNaiId = KhieuNaiId;
                ucChiTietKhieuNai.IsTraSau = false;

                if (!IsPostBack)
                {
                    BindDropDownList();
                    BindForm();
                }

                BindMode();
                hidUsername.Value = UserInfo.Username;

                liKhoaPhieu.Visible = false;

                // Tạm thời đóng lại để chuyển KN bị lỗi
                //if (UserInfo.Username.StartsWith("vnpttt_"))
                //{
                //    liKhoaPhieu.Visible = true;
                //    liChuyenNgangHang.Visible = false;
                //    liChuyenPhanHoi.Visible = false;
                //    liChuyenPhanHoiTrungTam.Visible = false;
                //    liChuyenXuLy.Visible = false;
                //    liDongKN.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void BindMode()
        {
            if (Request.QueryString["Mode"] != null && !Request.QueryString["Mode"].Equals(string.Empty))
            {
                if (Request.QueryString["Mode"].ToLower().Equals("process"))
                {
                    Mode = 1;
                }
            }
            else Mode = 0;
        }

        private KhieuNaiInfo BindProcess()
        {
            KhieuNaiInfo khieuNaiInfo = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetInfo(KhieuNaiId);

            if (khieuNaiInfo == null) return null;

            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_phản_hồi_KN_trung_tâm))
            {
                liChuyenPhanHoiTrungTam.Visible = false;
            }

            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Đóng_khiếu_nại))
            {
                liDongKN.Visible = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Đóng_khiếu_nại_do_phòng_ban_mình_tạo_ra) && UserInfo.PhongBanId == khieuNaiInfo.PhongBanTiepNhanId;
            }

            dvUserInPhongBan.Visible = false;
            dvUserInPhongBan_divPoupChuyenXuLyAuTo.Visible = false;
            ucChiTietKhieuNai.IsTraSau = khieuNaiInfo.IsTraSau;

            string urlViewMod = string.Format("/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN={0}&Mode=View&ReturnUrl={1}", KhieuNaiId, HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]));

            if (Request.QueryString["Mode"] != null && !Request.QueryString["Mode"].Equals(string.Empty))
            {
                if (Request.QueryString["Mode"].ToLower().Equals("process"))
                {
                    if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xử_lý_khiếu_nại))
                    {
                        Response.Redirect(urlViewMod, false); return null;
                    }
                    if (khieuNaiInfo.TrangThai == (byte)KhieuNai_TrangThai_Type.Đóng)
                    {
                        Response.Redirect(urlViewMod, false); return null;
                    }

                    Mode = 1;

                    // Kiểm tra xem có hợp lệ không?    
                    if (khieuNaiInfo.NguoiXuLy == string.Empty && khieuNaiInfo.PhongBanXuLyId == UserInfo.PhongBanId)
                    {
                        if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tiếp_nhận_khiếu_nại))
                        {
                            if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tiếp_nhận_KN_phản_hồi_về_người_gửi))
                            {
                                KhieuNai_ActivityInfo itemActivity = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(KhieuNaiId);
                                if (itemActivity != null)
                                {
                                    if (itemActivity.HanhDong != (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi || itemActivity.NguoiDuocPhanHoi != UserInfo.Username)
                                    {
                                        Response.Redirect(urlViewMod, false); return null;
                                    }
                                }
                                else
                                {
                                    Response.Redirect(urlViewMod, false); return null;
                                }
                            }
                            else
                            {
                                Response.Redirect(urlViewMod, false); return null;
                            }
                        }
                        // Cập nhật người xử lý
                        khieuNaiInfo.NguoiXuLyId = UserInfo.Id;
                        khieuNaiInfo.NguoiXuLy = UserInfo.Username;

                        if (khieuNaiInfo.TrangThai == (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý)
                        {
                            khieuNaiInfo.TrangThai = (byte)KhieuNai_TrangThai_Type.Đang_xử_lý;
                        }

                        bool flag = false;
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                ServiceFactory.GetInstanceKhieuNai().UpdateTrangThaiTiepNhan(KhieuNaiId, khieuNaiInfo.TrangThai, UserInfo.Id, UserInfo.Username, 0, string.Empty);
                                ServiceFactory.GetInstanceKhieuNai_Activity().UpdateTiepNhan(KhieuNaiId, UserInfo.Username, UserInfo.PhongBanId);

                                scope.Complete();
                                flag = true;
                            }
                            catch (Exception ex)
                            {
                                Helper.GhiLogs(ex);
                                Response.Redirect(urlViewMod, false); return null;
                            }
                        }
                        if (flag)
                        {
                            KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                            buocXuLyInfo.NoiDung = "Tiếp nhận khiếu nại " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(khieuNaiInfo.PhongBanXuLyId);
                            buocXuLyInfo.LUser = UserInfo.Username;
                            buocXuLyInfo.KhieuNaiId = khieuNaiInfo.Id;
                            buocXuLyInfo.NguoiXuLyId = UserInfo.Id;
                            buocXuLyInfo.PhongBanXuLyId = UserInfo.PhongBanId;
                            buocXuLyInfo.IsAuto = true;
                            ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Tiếp nhận khiếu nại", "Người xử lý", "", UserInfo.Username);
                        }

                    }
                    else if (khieuNaiInfo.NguoiXuLy == UserInfo.Username)
                    {
                        if (khieuNaiInfo.TrangThai == (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý)
                        {
                            khieuNaiInfo.TrangThai = (byte)KhieuNai_TrangThai_Type.Đang_xử_lý;
                            try
                            {
                                ServiceFactory.GetInstanceKhieuNai().UpdateTrangThaiTiepNhan(KhieuNaiId, khieuNaiInfo.TrangThai, UserInfo.Id, UserInfo.Username, 0, string.Empty);
                            }
                            catch (Exception ex)
                            {
                                Helper.GhiLogs(ex);
                                Response.Redirect(urlViewMod, false); return null;
                            }

                            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Tiếp nhận khiếu nại", "Người xử lý", "", UserInfo.Username);
                        }


                    }
                    else if (khieuNaiInfo.PhongBanXuLyId == UserInfo.PhongBanId && BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_KN_phản_hồi_của_phòng_ban_sau_thời_gian_cấu_hình)
                        && (DateTime.Now - khieuNaiInfo.LDate).TotalHours >= Config.TimeEditKhieuNai)
                    {
                        string NguoiXuLyOld = khieuNaiInfo.NguoiXuLy;

                        khieuNaiInfo.NguoiTienXuLyCap1Id = khieuNaiInfo.NguoiXuLyId;
                        khieuNaiInfo.NguoiTienXuLyCap1 = khieuNaiInfo.NguoiXuLy;

                        khieuNaiInfo.NguoiXuLyId = UserInfo.Id;
                        khieuNaiInfo.NguoiXuLy = UserInfo.Username;

                        bool flag = false;
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                ServiceFactory.GetInstanceKhieuNai().UpdateTrangThaiTiepNhan(KhieuNaiId, khieuNaiInfo.TrangThai, UserInfo.Id, UserInfo.Username, khieuNaiInfo.NguoiTienXuLyCap1Id, khieuNaiInfo.NguoiTienXuLyCap1);
                                ServiceFactory.GetInstanceKhieuNai_Activity().UpdateTiepNhan(KhieuNaiId, UserInfo.Username, UserInfo.PhongBanId);
                                scope.Complete();
                                flag = true;
                            }
                            catch (Exception ex)
                            {
                                Helper.GhiLogs(ex);
                                Response.Redirect(urlViewMod, false); return null;
                            }
                        }
                        if (flag)
                        {
                            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Tiếp nhận khiếu nại sau thời gian " + ((DateTime.Now - khieuNaiInfo.LDate).TotalHours) + " giờ.", "Người xử lý", NguoiXuLyOld, UserInfo.Username);
                        }
                    }
                    else
                    {
                        Response.Redirect(urlViewMod, false); return null;
                    }

                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng_ban_xử_lý))
                    {
                        dvUserInPhongBan.Visible = true;
                        dvUserInPhongBan_divPoupChuyenXuLyAuTo.Visible = true;
                    }

                    if (khieuNaiInfo.PhongBanXuLyId == khieuNaiInfo.PhongBanTiepNhanId)
                    {
                        liChuyenPhanHoi.Visible = false;
                        liChuyenPhanHoiTrungTam.Visible = false;
                    }

                    //Load User In Phong Ban cho chuyển ngang hàng
                    List<PhongBan_UserInfo> lstUserInPhongBan = ServiceFactory.GetInstancePhongBan_User().GetListDynamicJoin("b.Id, b.TenTruyCap", "LEFT JOIN NguoiSuDung b on a.NguoiSuDungId = b.Id", "TrangThai = 1 AND PhongBanId=" + UserInfo.PhongBanId, "");
                    ddlUserNgangHang.DataSource = lstUserInPhongBan;
                    ddlUserNgangHang.DataTextField = "TenTruyCap";
                    ddlUserNgangHang.DataValueField = "TenTruyCap";
                    ddlUserNgangHang.DataBind();
                    ddlUserNgangHang.Items.Insert(0, new ListItem("Phòng ban", ""));

                    //Load phòng ban chuyển phản hồi
                    List<KhieuNai_ActivityInfo> lstPhongBanChuyenPhanHoi = ServiceFactory.GetInstanceKhieuNai_Activity().GetListPhongBanPhanHoi(khieuNaiInfo.Id, UserInfo.PhongBanId);

                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_phản_hồi_về_phòng_ban_tiếp_nhận))
                    {
                        List<KhieuNai_ActivityInfo> lstTiepNhan = ServiceFactory.GetInstanceKhieuNai_Activity().GetListDynamic("", "HanhDong=0 and KhieuNaiId=" + khieuNaiInfo.Id, "");
                        if (lstTiepNhan.Any())
                        {
                            lstPhongBanChuyenPhanHoi.Insert(0, new KhieuNai_ActivityInfo() { PhongBan_Name = "Phòng ban tiếp nhận", PhongBanXuLyTruocId = lstTiepNhan[0].PhongBanXuLyId });
                        }
                    }

                    ddlPhongBanPhanHoi.DataSource = lstPhongBanChuyenPhanHoi;
                    ddlPhongBanPhanHoi.DataTextField = "PhongBan_Name";
                    ddlPhongBanPhanHoi.DataValueField = "PhongBanXuLyTruocId";
                    ddlPhongBanPhanHoi.DataBind();

                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_phản_hồi_KN_trung_tâm))
                    {
                        List<KhieuNai_ActivityInfo> lstTrungTamChuyenPhanHoi = ServiceFactory.GetInstanceKhieuNai_Activity().GetListTrungTamPhanHoi(khieuNaiInfo.Id, UserInfo.DoiTacId);
                        ddlTrungTamPhanHoi.DataSource = lstTrungTamChuyenPhanHoi;
                        ddlTrungTamPhanHoi.DataTextField = "PhongBan_Name";
                        ddlTrungTamPhanHoi.DataValueField = "PhongBanXuLyTruocId";
                        ddlTrungTamPhanHoi.DataBind();
                    }
                }
                else
                {
                    Mode = 0;
                    liChuyenNgangHang.Visible = false;
                    liChuyenPhanHoi.Visible = false;
                    liChuyenXuLy.Visible = false;
                    liChuyenPhanHoiTrungTam.Visible = false;
                    liDongKN.Visible = false;

                    //Mode = 1;
                    //liChuyenNgangHang.Visible = true;
                    //liChuyenPhanHoi.Visible = true;
                    //liChuyenXuLy.Visible = true;
                    //liChuyenPhanHoiTrungTam.Visible = true;
                    //liDongKN.Visible = true;

                    BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Xem khiếu nại", "[Hành động]", "", "", ArchiveId);
                }
            }
            else
            {
                Response.Redirect(urlViewMod, false); return null;
            }
            return khieuNaiInfo;
        }

        private void BindForm()
        {
            if (KhieuNaiId != 0)
            {
                KhieuNaiInfo KhieuNaiItem = BindProcess();

                if (KhieuNaiItem == null) return;

                ltMaKhieuNai.Text = GetDataImpl.GetMaTuDong("PA", KhieuNaiItem.Id, 10);

                if (KhieuNaiItem.IsTraSau)
                {
                    //ltLoaiThueBao.Text = "Trả sau";
                    ddlLoaiThueBao.SelectedValue = "1";
                }
                else
                {
                    //ltLoaiThueBao.Text = "Trả trước";
                    ddlLoaiThueBao.SelectedValue = "0";
                }

                txtSoThueBao.Text = KhieuNaiItem.SoThueBao.ToString();
                txtHoTen.Text = KhieuNaiItem.HoTenLienHe;
                txtDienThoaiLienHe.Text = KhieuNaiItem.SDTLienHe;
                txtThoiGianSuCo.Text = KhieuNaiItem.ThoiGianXayRa;
                txtDiaDiemSuCo.Text = KhieuNaiItem.DiaDiemXayRa;
                txtNoiDung.Text = KhieuNaiItem.NoiDungPA.Replace("\r", "");
                txtDiaChiLienHe.Text = KhieuNaiItem.DiaChiLienHe;
                txtNoiDungCanHoTro.Text = KhieuNaiItem.NoiDungCanHoTro.Replace("\r", "");
                ddlHTTiepNhan.SelectedValue = KhieuNaiItem.HTTiepNhan.ToString();
                ddlTinh.SelectedValue = KhieuNaiItem.MaTinhId.ToString();
                ChangeTinh();
                ddlHuyen.SelectedValue = KhieuNaiItem.MaQuanId.ToString();
                ChangeQuanHuyen();
                ddlPhuongXa.SelectedValue = KhieuNaiItem.MaPhuongId.ToString();

                ltTrangThai.Text = Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNaiItem.TrangThai).Replace("_", " ");
                if (KhieuNaiItem.TrangThai == (byte)KhieuNai_TrangThai_Type.Chờ_đóng)
                    chkKNChoDong.Checked = true;

                string strFileName = string.Empty;
                string urlFile = BuildKhieuNai_FileDinhKem.BuildURLFileDinhKemKhachHang(KhieuNaiId, out strFileName);
                ltFileKH.Text = string.Format("<a href='{0}' target='_blank' alt='Click to download file'>{1}</a>", urlFile, strFileName);

                ltNguoiTiepNhan.Text = KhieuNaiItem.NguoiTiepNhan;
                ltNgayCapNhat.Text = KhieuNaiItem.LDate.ToString("dd/MM/yyyy HH:mm");

                ddlDoUuTien.SelectedValue = KhieuNaiItem.DoUuTien.ToString();
                ddlDoUuTien.Attributes.Add("ValueOld", ddlDoUuTien.SelectedItem.Text);
                chkHangLoat.Checked = KhieuNaiItem.KNHangLoat;
                chkHangLoat.Attributes.Add("ValueOld", KhieuNaiItem.KNHangLoat ? "Có" : "Không");

                ltNgayTiepNhan.Text = KhieuNaiItem.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm");

                //Nếu đóng thì lấy ngày tiếp nhận và ngày quá hạn
                if ((byte)KhieuNai_TrangThai_Type.Đóng == KhieuNaiItem.TrangThai)
                {
                    //ltThoiHan.Text = KhieuNaiItem.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");

                    //Nếu khiếu nại đã đóng sẽ có ngày đóng, độ hài lòng của khách hàng
                    ltNgayDong.Text = KhieuNaiItem.NgayDongKN.ToString("dd/MM/yyyy HH:mm");
                    ltTenDoHaiLong.Text = Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), KhieuNaiItem.DoHaiLong) != null ? Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), KhieuNaiItem.DoHaiLong).Replace("_", " ") : string.Empty;
                }
                else  //Nếu đang xử lý thì lấy ngày chuyển và ngày quá hạn phòng ban.
                {
                    //ltThoiHan.Text = KhieuNaiItem.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm");
                }

                ltQuaHanPB_1.Text = KhieuNaiItem.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm");
                ltQuaHanTT.Text = KhieuNaiItem.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");

                ltNguoiXuLy.Text = KhieuNaiItem.NguoiXuLy;


                txtGhiChu.Text = KhieuNaiItem.GhiChu.Replace("\r", "");
                txtGhiChu.Attributes.Add("ValueOld", KhieuNaiItem.GhiChu);

                if (KhieuNaiItem.LoaiKhieuNaiId > 0)
                {
                    bool isExits = false;
                    for (int i = 0; i < ddlLoaiKhieuNai.Items.Count; i++)
                    {
                        if (ddlLoaiKhieuNai.Items[i].Value == KhieuNaiItem.LoaiKhieuNaiId.ToString())
                        {
                            ddlLoaiKhieuNai.SelectedValue = KhieuNaiItem.LoaiKhieuNaiId.ToString();
                            isExits = true;
                            break;
                        }
                    }

                    if (!isExits)
                    {
                        LoaiKhieuNaiInfo objTemp = ServiceFactory.GetInstanceLoaiKhieuNai().GetInfo(KhieuNaiItem.LoaiKhieuNaiId);
                        if (objTemp != null)
                        {
                            ListItem item = new ListItem(objTemp.Name, objTemp.Id.ToString());
                            ddlLoaiKhieuNai.Items.Insert(0, item);
                            ddlLoaiKhieuNai.SelectedValue = KhieuNaiItem.LoaiKhieuNaiId.ToString();
                        }
                    }
                    ddlLoaiKhieuNai.Attributes.Add("ValueOld", ddlLoaiKhieuNai.SelectedItem.Text);
                }
                //ddlLoaiKhieuNai.SelectedValue = KhieuNaiItem.LoaiKhieuNaiId.ToString();
                //ddlLoaiKhieuNai.Attributes.Add("ValueOld", ddlLoaiKhieuNai.SelectedItem.Text);
                ChangeLoaiKhieuNai();

                if (KhieuNaiItem.LinhVucChungId != 0)
                {
                    bool isExits = false;
                    for (int i = 0; i < ddlLinhVucChung.Items.Count; i++)
                    {
                        if (ddlLinhVucChung.Items[i].Value == KhieuNaiItem.LinhVucChungId.ToString())
                        {
                            ddlLinhVucChung.SelectedValue = KhieuNaiItem.LinhVucChungId.ToString();
                            isExits = true;
                            break;
                        }
                    }

                    if (!isExits)
                    {
                        LoaiKhieuNaiInfo objTemp = ServiceFactory.GetInstanceLoaiKhieuNai().GetInfo(KhieuNaiItem.LinhVucChungId);
                        if (objTemp != null)
                        {
                            ListItem item = new ListItem(objTemp.Name, objTemp.Id.ToString());
                            ddlLinhVucChung.Items.Insert(0, item);
                            ddlLinhVucChung.SelectedValue = KhieuNaiItem.LinhVucChungId.ToString();
                        }
                    }

                    ddlLinhVucChung.Attributes.Add("ValueOld", ddlLinhVucChung.SelectedItem.Text);

                    //ddlLinhVucChung.SelectedValue = KhieuNaiItem.LinhVucChungId.ToString();
                    //ddlLinhVucChung.Attributes.Add("ValueOld", ddlLinhVucChung.SelectedItem.Text);
                }
                else
                    ddlLinhVucChung.Items.Insert(0, new ListItem("", "0"));

                ChangeLinhVucChung();
                if (KhieuNaiItem.LinhVucConId != 0)
                {
                    bool isExits = false;
                    for (int i = 0; i < ddlLinhVucCon.Items.Count; i++)
                    {
                        if (ddlLinhVucCon.Items[i].Value == KhieuNaiItem.LinhVucConId.ToString())
                        {
                            ddlLinhVucCon.SelectedValue = KhieuNaiItem.LinhVucConId.ToString();
                            isExits = true;
                            break;
                        }
                    }

                    if (!isExits)
                    {
                        LoaiKhieuNaiInfo objTemp = ServiceFactory.GetInstanceLoaiKhieuNai().GetInfo(KhieuNaiItem.LinhVucConId);
                        if (objTemp != null)
                        {
                            ListItem item = new ListItem(objTemp.Name, objTemp.Id.ToString());
                            ddlLinhVucCon.Items.Insert(0, item);
                            ddlLinhVucCon.SelectedValue = KhieuNaiItem.LinhVucConId.ToString();
                        }
                    }

                    ddlLinhVucCon.Attributes.Add("ValueOld", ddlLinhVucCon.SelectedItem.Text);

                    //ddlLinhVucCon.SelectedValue = KhieuNaiItem.LinhVucConId.ToString();
                    //ddlLinhVucCon.Attributes.Add("ValueOld", ddlLinhVucCon.SelectedItem.Text);
                }
                else
                    ddlLinhVucCon.Items.Insert(0, new ListItem("", "0"));

                if (KhieuNaiItem.LyDoGiamTru != 0)
                {
                    ddlNguyenNhanLoi.SelectedValue = KhieuNaiItem.LyDoGiamTru.ToString();
                    LoadChiTietLoi(KhieuNaiItem.LyDoGiamTru);
                    if (KhieuNaiItem.ChiTietLoiId != -1)
                    {
                        ddlChiTietLoi.SelectedValue = KhieuNaiItem.ChiTietLoiId.ToString();
                    }
                }

                ddlChiTietLoi.SelectedValue = KhieuNaiItem.ChiTietLoiId.ToString();

                ddlLoaiThueBao.Enabled = false;
                txtSoThueBao.ReadOnly = true;
                txtSoThueBao.Enabled = false;

                ddlLinhVucChung.Enabled = false;
                ddlLinhVucCon.Enabled = false;
                ddlLoaiKhieuNai.Enabled = false;

                ddlDoUuTien.Enabled = false;
                txtGhiChu.ReadOnly = true;
                chkHangLoat.Enabled = false;
                ddlHTTiepNhan.Enabled = false;
                chkKNChoDong.Enabled = false;

                txtHoTen.ReadOnly = true;
                txtDienThoaiLienHe.ReadOnly = true;
                txtThoiGianSuCo.ReadOnly = true;
                ddlTinh.Enabled = false;
                ddlHuyen.Enabled = false;
                ddlPhuongXa.Enabled = false;
                txtDiaDiemSuCo.ReadOnly = true;
                txtNoiDung.ReadOnly = true;
                txtDiaChiLienHe.ReadOnly = true;
                txtNoiDungCanHoTro.ReadOnly = true;
                //ddlNguyenNhanLoi.Enabled = false;
                //ddlChiTietLoi.Enabled = false;

                if (Mode == 1 && KhieuNaiItem.TrangThai != (int)KhieuNai_TrangThai_Type.Đóng)
                {
                    ddlDoUuTien.Enabled = true;
                    txtGhiChu.ReadOnly = false;
                    chkHangLoat.Enabled = true;
                    chkKNChoDong.Enabled = true;
                    //ddlNguyenNhanLoi.Enabled = true;
                    //ddlChiTietLoi.Enabled = true;

                    if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_khiếu_nại) || KhieuNaiItem.NguoiTiepNhan == KhieuNaiItem.NguoiXuLy))
                    {
                        ddlTinh.Enabled = true;
                        ddlHuyen.Enabled = true;
                        ddlPhuongXa.Enabled = true;
                        ddlHTTiepNhan.Enabled = true;

                        txtHoTen.ReadOnly = false;
                        txtDienThoaiLienHe.ReadOnly = false;
                        txtThoiGianSuCo.ReadOnly = false;
                        txtDiaDiemSuCo.ReadOnly = false;
                        txtNoiDung.ReadOnly = false;
                        txtDiaChiLienHe.ReadOnly = false;


                        txtHoTen.Attributes.Add("ValueOld", KhieuNaiItem.HoTenLienHe);
                        txtDienThoaiLienHe.Attributes.Add("ValueOld", KhieuNaiItem.SDTLienHe);
                        txtThoiGianSuCo.Attributes.Add("ValueOld", KhieuNaiItem.ThoiGianXayRa);
                        txtDiaDiemSuCo.Attributes.Add("ValueOld", KhieuNaiItem.DiaDiemXayRa);
                        txtNoiDung.Attributes.Add("ValueOld", KhieuNaiItem.NoiDungPA);
                        txtDiaChiLienHe.Attributes.Add("ValueOld", KhieuNaiItem.DiaChiLienHe);
                        ddlTinh.Attributes.Add("ValueOld", ddlTinh.SelectedItem.Text);
                        ddlHuyen.Attributes.Add("ValueOld", ddlHuyen.SelectedItem.Text);
                        ddlPhuongXa.Attributes.Add("ValueOld", ddlPhuongXa.SelectedItem.Text);
                        ddlHTTiepNhan.Attributes.Add("ValueOld", ddlHTTiepNhan.SelectedItem.Text);

                        ddlLoaiThueBao.Attributes.Add("ValueOld", ddlLoaiThueBao.SelectedItem.Text);

                        // Kiểm tra đã nhập số tiền chưa ? Nếu nhập rồi thì không cho phép sửa loại thuê bao nữa
                        List<KhieuNai_SoTienInfo> listKhieuNaiSoTien = ServiceFactory.GetInstanceKhieuNai_SoTien().GetListDynamic("*", "KhieuNaiId=" + KhieuNaiItem.Id, "");
                        ddlLoaiThueBao.Enabled = listKhieuNaiSoTien == null || listKhieuNaiSoTien.Count == 0;
                    }

                    if (KhieuNaiItem.NguoiTiepNhan == KhieuNaiItem.NguoiXuLy)
                    {
                        txtNoiDungCanHoTro.ReadOnly = false;
                        txtNoiDungCanHoTro.Attributes.Add("ValueOld", KhieuNaiItem.NoiDungCanHoTro);
                    }

                    if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_cây_thư_mục) || KhieuNaiItem.NguoiTiepNhan == KhieuNaiItem.NguoiXuLy))
                    {
                        ddlLinhVucChung.Enabled = true;
                        ddlLinhVucCon.Enabled = true;
                        ddlLoaiKhieuNai.Enabled = true;
                    }
                }
            }
        }

        private void BindDropDownList()
        {
            foreach (byte i in Enum.GetValues(typeof(KhieuNai_DoUuTien_Type)))
            {
                ddlDoUuTien.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " "), i.ToString()));
            }


            foreach (byte i in Enum.GetValues(typeof(KhieuNai_HTTiepNhan_Type)))
            {
                ddlHTTiepNhan.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), i).Replace("_", " "), i.ToString()));
            }

            List<ProvinceInfo> lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId is null", "Name");
            lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Chọn Tỉnh/Thành Phố--" });
            ddlTinh.DataSource = lstTinh;
            ddlTinh.DataTextField = "Name";
            ddlTinh.DataValueField = "Id";
            ddlTinh.DataBind();

            var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0  and (Status = 1 OR Status = 2)", "Sort");
            ddlLoaiKhieuNai.DataSource = lstLoaiKN;
            ddlLoaiKhieuNai.DataTextField = "Name";
            ddlLoaiKhieuNai.DataValueField = "Id";
            ddlLoaiKhieuNai.DataBind();

            var keyLoiKhieuNaiCha = string.Concat("CLoiKhieuNaiCha_", 0, "_", DateTime.Now.ToString("yyyyMMdd"));
            ddlNguyenNhanLoi.DataSource = Cache.Data<object>(keyLoiKhieuNaiCha, (60 * 60), () =>
            {
                List<LoiKhieuNaiInfo> lst = ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("*",
                   string.Format("ParentId=0 AND HoatDong=1 AND TuNgay <= {0} AND DenNgay >= {0}", DateTime.Now.ToString("yyyyMMdd")), "ThuTu ASC");
                lst.Insert(0, new LoiKhieuNaiInfo() { Id = 0, TenLoi = "--Chọn Nguyên nhân lỗi--" });
                return lst;
            });

            ddlNguyenNhanLoi.DataTextField = "TenLoi";
            ddlNguyenNhanLoi.DataValueField = "Id";
            ddlNguyenNhanLoi.DataBind();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            LoadTooltip();
        }

        private void LoadTooltip()
        {
            Utility.Tooltip(ddlLoaiKhieuNai);
            Utility.Tooltip(ddlLinhVucChung);
            Utility.Tooltip(ddlLinhVucCon);
        }

        #region Change Control Form Xu Ly

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/07/2015
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLoaiThueBao_Changed(object sender, EventArgs e)
        {
            if (Mode == 0) return;

            // Kiểm tra đã nhập số tiền chưa ? Nếu nhập rồi thì không cho phép sửa loại thuê bao nữa
            List<KhieuNai_SoTienInfo> listKhieuNaiSoTien = ServiceFactory.GetInstanceKhieuNai_SoTien().GetListDynamic("*", "KhieuNaiId=" + KhieuNaiId, "");
            if (listKhieuNaiSoTien == null || listKhieuNaiSoTien.Count == 0)
            {
                ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', IsTraSau=" + ddlLoaiThueBao.SelectedValue, "Id=" + KhieuNaiId);
                BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi loại thuê bao", "Loại thuê bao", ddlLoaiThueBao.Attributes["ValueOld"], ddlLoaiThueBao.SelectedItem.Text);
                ddlLoaiThueBao.Attributes["ValueOld"] = ddlLoaiThueBao.SelectedItem.Text;
            }

            // Request lại chính trang này để load lại danh sách tài khoản giảm trừ tương ứng
            Response.Redirect(Request.Url.AbsoluteUri, false);
        }

        protected void ddlLoaiKhieuNai_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;

            // HaiPH : Cập nhật  ngày quá hạn, ngày cảnh báo khi đổi loại khiếu nại
            KhieuNaiInfo khieuNaiInfo = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetInfo(KhieuNaiId);
            if (khieuNaiInfo != null && ConvertUtility.ToInt32(ddlLoaiKhieuNai.SelectedItem.Value) > 0)
            {
                khieuNaiInfo.NgayQuaHan = GetDataImpl.GetTimeConfig_KhieuNai(khieuNaiInfo.NgayTiepNhan, ConvertUtility.ToInt32(ddlLoaiKhieuNai.SelectedItem.Value));
                khieuNaiInfo.NgayCanhBao = khieuNaiInfo.NgayQuaHan;
                ltQuaHanTT.Text = khieuNaiInfo.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");

                ChangeLoaiKhieuNai();
                string updateClause = string.Format("LDate=getdate(),LUser=N'{0}',LinhVucChungId=0,LinhVucConId=0,LinhVucChung='',LinhVucCon='',LoaiKhieuNai=N'{1}', LoaiKhieuNaiId={2}, NgayQuaHan='{3}', NgayCanhBao='{3}'", LoginAdmin.AdminLogin().Username, ddlLoaiKhieuNai.SelectedItem.Text, ddlLoaiKhieuNai.SelectedValue, khieuNaiInfo.NgayQuaHan);
                ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(updateClause, "Id=" + KhieuNaiId);
                //ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',LinhVucChungId=0,LinhVucConId=0,LinhVucChung='',LinhVucCon='',LoaiKhieuNai=N'" + ddlLoaiKhieuNai.SelectedItem.Text + "', LoaiKhieuNaiId=" + ddlLoaiKhieuNai.SelectedValue, "Id=" + KhieuNaiId);
                BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi loại khiếu nại", "Loại khiếu nại", ddlLoaiKhieuNai.Attributes["ValueOld"], ddlLoaiKhieuNai.SelectedItem.Text);

                ddlLinhVucCon.Attributes["ValueOld"] = "";
                ddlLinhVucChung.Attributes["ValueOld"] = "";
                ddlLoaiKhieuNai.Attributes["ValueOld"] = ddlLoaiKhieuNai.SelectedItem.Text;
            }
        }

        private void ChangeLoaiKhieuNai()
        {
            ddlLinhVucCon.Items.Clear();
            ddlLinhVucCon.Items.Add(new ListItem("--Lĩnh vực con--", "0"));

            if (ddlLoaiKhieuNai.SelectedValue.Equals("0"))
            {
                ddlLinhVucChung.Items.Clear();
                ddlLinhVucChung.Items.Add(new ListItem("--Lĩnh vực chung--", "0"));
            }
            else
            {
                var lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "(Status = 1 OR Status = 2) and ParentId=" + ddlLoaiKhieuNai.SelectedValue, "Sort");
                lstLinhVucChung.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "--Chọn Lĩnh vực chung--" });
                ddlLinhVucChung.DataSource = lstLinhVucChung;
                ddlLinhVucChung.DataTextField = "Name";
                ddlLinhVucChung.DataValueField = "Id";
                ddlLinhVucChung.DataBind();
            }
        }

        protected void ddlLinhVucChung_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;

            //ChangeLinhVucChung();

            if (ddlLinhVucChung.SelectedValue.Equals("0"))
                return;

            //ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',LinhVucCon='',LinhVucConId=0,LinhVucChung=N'" + ddlLinhVucChung.SelectedItem.Text + "', LinhVucChungId=" + ddlLinhVucChung.SelectedValue, "Id=" + KhieuNaiId);
            //BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi lĩnh vực chung", "Lĩnh vực chung", ddlLinhVucChung.Attributes["ValueOld"], ddlLinhVucChung.SelectedItem.Text);
            //ddlLinhVucCon.Attributes["ValueOld"] = "";
            //ddlLinhVucChung.Attributes["ValueOld"] = ddlLinhVucChung.SelectedItem.Text;

            // HaiPH : Cập nhật  ngày quá hạn, ngày cảnh báo khi đổi lĩnh vực chung
            KhieuNaiInfo khieuNaiInfo = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetInfo(KhieuNaiId);
            if (khieuNaiInfo != null && ConvertUtility.ToInt32(ddlLinhVucChung.SelectedItem.Value) > 0)
            {
                khieuNaiInfo.NgayQuaHan = GetDataImpl.GetTimeConfig_KhieuNai(khieuNaiInfo.NgayTiepNhan, ConvertUtility.ToInt32(ddlLinhVucChung.SelectedItem.Value));
                khieuNaiInfo.NgayCanhBao = khieuNaiInfo.NgayQuaHan;
                ltQuaHanTT.Text = khieuNaiInfo.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");

                ChangeLinhVucChung();
                string updateClause = string.Format("LDate=getdate(),LUser=N'{0}',LinhVucCon='',LinhVucConId=0,LinhVucChung=N'{1}', LinhVucChungId={2}, NgayQuaHan='{3}', NgayCanhBao='{3}'", LoginAdmin.AdminLogin().Username, ddlLinhVucChung.SelectedItem.Text, ddlLinhVucChung.SelectedValue, khieuNaiInfo.NgayQuaHan);
                ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(updateClause, "Id=" + KhieuNaiId);
                BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi lĩnh vực chung", "Lĩnh vực chung", ddlLinhVucChung.Attributes["ValueOld"], ddlLinhVucChung.SelectedItem.Text);
                ddlLinhVucCon.Attributes["ValueOld"] = "";
                ddlLinhVucChung.Attributes["ValueOld"] = ddlLinhVucChung.SelectedItem.Text;
            }
        }

        private void ChangeLinhVucChung()
        {
            if (ddlLinhVucChung.SelectedValue.Equals("0"))
            {
                ddlLinhVucCon.Items.Clear();
                ddlLinhVucCon.Items.Add(new ListItem("--Lĩnh vực con--", "0"));
            }
            else
            {
                var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "(Status = 1 or Status = 2) and ParentId=" + ddlLinhVucChung.SelectedValue, "Sort");
                lstLinhVucCon.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "--Chọn Lĩnh vực con--" });
                ddlLinhVucCon.DataSource = lstLinhVucCon;
                ddlLinhVucCon.DataTextField = "Name";
                ddlLinhVucCon.DataValueField = "Id";
                ddlLinhVucCon.DataBind();
            }
        }

        protected void ddlLinhVucCon_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            if (ddlLinhVucCon.SelectedValue.Equals("0"))
                return;

            //ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',LinhVucCon=N'" + ddlLinhVucCon.SelectedItem.Text + "', LinhVucConId=" + ddlLinhVucCon.SelectedValue, "Id=" + KhieuNaiId);
            //BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi lĩnh vực con", "Lĩnh vực con", ddlLinhVucCon.Attributes["ValueOld"], ddlLinhVucCon.SelectedItem.Text);
            //ddlLinhVucCon.Attributes["ValueOld"] = ddlLinhVucCon.SelectedItem.Text;

            // HaiPH : Cập nhật  ngày quá hạn, ngày cảnh báo khi đổi lĩnh vực con
            KhieuNaiInfo khieuNaiInfo = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetInfo(KhieuNaiId);
            if (khieuNaiInfo != null && ConvertUtility.ToInt32(ddlLinhVucCon.SelectedItem.Value) > 0)
            {
                khieuNaiInfo.NgayQuaHan = GetDataImpl.GetTimeConfig_KhieuNai(khieuNaiInfo.NgayTiepNhan, ConvertUtility.ToInt32(ddlLinhVucCon.SelectedItem.Value));
                khieuNaiInfo.NgayCanhBao = khieuNaiInfo.NgayQuaHan;
                ltQuaHanTT.Text = khieuNaiInfo.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");

                //ChangeLoaiKhieuNai();
                string updateClause = string.Format("LDate=getdate(),LUser=N'{0}',LinhVucCon=N'{1}', LinhVucConId={2}, NgayQuaHan='{3}', NgayCanhBao='{3}'", LoginAdmin.AdminLogin().Username, ddlLinhVucCon.SelectedItem.Text, ddlLinhVucCon.SelectedValue, khieuNaiInfo.NgayQuaHan);
                ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(updateClause, "Id=" + KhieuNaiId);
                BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi lĩnh vực con", "Lĩnh vực con", ddlLinhVucCon.Attributes["ValueOld"], ddlLinhVucCon.SelectedItem.Text);
                ddlLinhVucCon.Attributes["ValueOld"] = ddlLinhVucCon.SelectedItem.Text;
            }
        }

        protected void ddlDoUuTien_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', DoUuTien=" + ddlDoUuTien.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi độ ưu tiên", "Độ ưu tiên", ddlDoUuTien.Attributes["ValueOld"], ddlDoUuTien.SelectedItem.Text);
            ddlDoUuTien.Attributes["ValueOld"] = ddlDoUuTien.SelectedItem.Text;
        }

        protected void chkHangLoat_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', KNHangLoat=" + (chkHangLoat.Checked ? 1 : 0), "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi khiếu nại hàng loạt", "KN hàng loạt", chkHangLoat.Attributes["ValueOld"], chkHangLoat.Checked ? "Có" : "Không");
            chkHangLoat.Attributes["ValueOld"] = chkHangLoat.Checked ? "Có" : "Không";
        }

        protected void txtGhiChu_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;

            ServiceFactory.GetInstanceKhieuNai().UpdateGhiChu(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtGhiChu.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi ghi chú", "Ghi chú", txtGhiChu.Attributes["ValueOld"], txtGhiChu.Text);
            txtGhiChu.Attributes["ValueOld"] = txtGhiChu.Text;
        }

        protected void btReOpen_Click(object sender, EventArgs e)
        {

        }

        protected void chkKNChoDong_CheckedChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            bool isChoDong = false;
            if (chkKNChoDong.Checked)
                isChoDong = true;

            string strUpdate = string.Format("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', TrangThai={0}", isChoDong ? (byte)KhieuNai_TrangThai_Type.Chờ_đóng : (byte)KhieuNai_TrangThai_Type.Đang_xử_lý);

            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(strUpdate, "Id=" + KhieuNaiId);

            if (isChoDong)
                ltTrangThai.Text = Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " ");
            else
                ltTrangThai.Text = Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " ");

            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi trạng thái " + (isChoDong ? Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " ") : Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " ")), "Trạng thái",
                isChoDong ? Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " ") : Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " "),
                isChoDong ? Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " ") : Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " "));
        }

        protected void ddlTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ChangeTinh();
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',MaTinhId=" + ddlTinh.SelectedValue + ",  MaTinh=N'" + ddlTinh.SelectedItem.Text + "', MaQuanId=0, MaQuan='', MaPhuongId=0, MaPhuong=''", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, @"Thay đổi nội dung tỉnh\thành phố xảy ra sự cố", "Tỉnh/Thành phố sự cố", ddlTinh.Attributes["ValueOld"], ddlTinh.SelectedItem.Text);
            ddlTinh.Attributes["ValueOld"] = ddlTinh.SelectedItem.Text;
        }

        private void ChangeTinh()
        {
            ddlPhuongXa.Items.Clear();
            ddlPhuongXa.Items.Add(new ListItem("--Chọn Phường/Xã--", "0"));

            if (ddlTinh.SelectedValue.Equals("0"))
            {
                ddlHuyen.Items.Clear();
                ddlHuyen.Items.Add(new ListItem("--Chọn Quận/Huyện--", "0"));
            }
            else
            {
                var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + ddlTinh.SelectedValue, "Name");
                lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Chọn Quận/Huyện--" });
                ddlHuyen.DataSource = lstTinh;
                ddlHuyen.DataTextField = "Name";
                ddlHuyen.DataValueField = "Id";
                ddlHuyen.DataBind();
            }

        }

        protected void ddlHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ChangeQuanHuyen();

            string maQuan = string.Empty;
            if (ConvertUtility.ToInt32(ddlHuyen.SelectedValue) > 0)
            {
                maQuan = ddlHuyen.SelectedItem.Text;
                if (maQuan.Contains("'"))
                {
                    maQuan = maQuan.Replace("'", "''");
                }
            }

            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',MaQuanId=" + ddlHuyen.SelectedValue + ", MaQuan=N'" + maQuan + "', MaPhuongId=0, MaPhuong=''", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, @"Thay đổi nội dung quận\huyện xảy ra sự cố", "Quận/Huyện sự cố", ddlHuyen.Attributes["ValueOld"], ddlHuyen.SelectedItem.Text);
            ddlHuyen.Attributes["ValueOld"] = ddlHuyen.SelectedItem.Text;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/09/2015
        /// </summary>
        private void ChangeQuanHuyen()
        {
            var lstPhuongXa = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + ddlHuyen.SelectedValue, "Name");
            lstPhuongXa.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Chọn Phường/Xã--" });
            ddlPhuongXa.DataSource = lstPhuongXa;
            ddlPhuongXa.DataTextField = "Name";
            ddlPhuongXa.DataValueField = "Id";
            ddlPhuongXa.DataBind();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/09/2015
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPhuongXa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;

            string maPhuong = string.Empty;
            if (ConvertUtility.ToInt32(ddlPhuongXa.SelectedValue) > 0)
            {
                maPhuong = ddlPhuongXa.SelectedItem.Text;
            }

            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',MaPhuongId=" + ddlPhuongXa.SelectedValue + ", MaPhuong=N'" + maPhuong + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, @"Thay đổi nội dung phường/xã xảy ra sự cố", "Phường/Xã sự cố", ddlPhuongXa.Attributes["ValueOld"], ddlPhuongXa.SelectedItem.Text);
            ddlPhuongXa.Attributes["ValueOld"] = ddlPhuongXa.SelectedItem.Text;
        }

        protected void ddlHTTiepNhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',HTTiepNhan=" + ddlHTTiepNhan.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, @"Thay đổi nội hình thức tiếp nhận", "Hình thức tiếp nhận", ddlHuyen.Attributes["ValueOld"], ddlHTTiepNhan.SelectedItem.Text);
            ddlHTTiepNhan.Attributes["ValueOld"] = ddlHTTiepNhan.SelectedItem.Text;
        }

        protected void txtHoTen_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateHoTenLienHe(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtHoTen.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi họ tên liên hệ", "Họ tên liên hệ", txtHoTen.Attributes["ValueOld"], txtHoTen.Text);
            txtHoTen.Attributes["ValueOld"] = txtHoTen.Text;
        }

        protected void txtDienThoaiLienHe_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateSDTLienHe(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtDienThoaiLienHe.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi số điện thoại liên hệ", "Điện thoại liên hệ", txtDienThoaiLienHe.Attributes["ValueOld"], txtDienThoaiLienHe.Text);
            txtDienThoaiLienHe.Attributes["ValueOld"] = txtDienThoaiLienHe.Text;
        }

        protected void txtThoiGianSuCo_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateThoiGianXayRa(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtThoiGianSuCo.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi thời gian xảy ra sự cố", "Thời gian xảy ra sự cố", txtThoiGianSuCo.Attributes["ValueOld"], txtThoiGianSuCo.Text);
            txtThoiGianSuCo.Attributes["ValueOld"] = txtThoiGianSuCo.Text;
        }

        protected void txtDiaChiLienHe_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDiaChiLienHe(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtDiaChiLienHe.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi địa chỉ liên hệ", "Địa chỉ liên hệ", txtDiaChiLienHe.Attributes["ValueOld"], txtDiaChiLienHe.Text);
            txtDiaChiLienHe.Attributes["ValueOld"] = txtDiaChiLienHe.Text;
        }

        protected void txtDiaDiemSuCo_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDiaDiemXayRa(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtDiaDiemSuCo.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi địa điểm xảy ra sự cố", "Địa điểm sự cố bổ sung", txtDiaDiemSuCo.Attributes["ValueOld"], txtDiaDiemSuCo.Text);
            txtDiaDiemSuCo.Attributes["ValueOld"] = txtDiaDiemSuCo.Text;
        }

        protected void txtNoiDung_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;

            ServiceFactory.GetInstanceKhieuNai().UpdateNoiDungPA(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtNoiDung.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi nội dung phản ánh", "Nội dung phản ánh", txtNoiDung.Attributes["ValueOld"], txtNoiDung.Text);
            txtNoiDung.Attributes["ValueOld"] = txtNoiDung.Text;
        }

        protected void txtNoiDungCanHoTro_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateNoiDungCanHoTro(KhieuNaiId, LoginAdmin.AdminLogin().Username, txtNoiDungCanHoTro.Text);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi nội dung cần hỗ trợ", "Nội dung cần hỗ trợ", txtNoiDungCanHoTro.Attributes["ValueOld"], txtNoiDungCanHoTro.Text);
            txtNoiDungCanHoTro.Attributes["ValueOld"] = txtNoiDungCanHoTro.Text;
        }
        #endregion

        #region Event Popup
        protected void btnOkayChuyenPhanHoi_Click(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            try
            {
                var strWarning = 0;
                BuildKhieuNai_Activity.ActivityChuyenPhanHoi(KhieuNaiId, ConvertUtility.ToInt32(ddlPhongBanPhanHoi.SelectedValue, 0), "", ref strWarning);
                string url = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";
                if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Equals(""))
                    url = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);
                if (strWarning == 0 || strWarning == 10000)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Chuyển phản hồi khiếu nại thành công.','info', '" + url + "');", true);
                else if (strWarning == 10001)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Chuyển phản hồi khiếu nại thành công.<br/>Không khóa được phiếu về VNPT.','warning', '" + url + "');", true);
                else if (strWarning == 10099)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Chuyển phản hồi khiếu nại thành công.<br/>Service khóa phiếu VNPT có lỗi, liên hệ quản trị hệ thống.','warning', '" + url + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal(\"" + HttpUtility.HtmlDecode(ex.Message) + ".\",\"error\");", true);
            }
        }

        protected void btnOkeyNgangHang_Click(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            try
            {
                BuildKhieuNai_Activity.ActivityChuyenNgangHangToUser(KhieuNaiId, ddlUserNgangHang.SelectedValue, "");

                string url = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";
                if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Equals(""))
                    url = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Chuyển ngang hàng khiếu nại thành công.','info', '" + url + "');", true);
            }
            catch (GQKNMessageException ge)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ge.Message + ".','error');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Chức năng chuyển ngang hàng có lỗi. Bạn thực hiện lại sau ít phút.','error');", true);
            }
        }

        #endregion


        protected void btnOkeyChuyenPhanHoiTrungTam_Click(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            try
            {
                BuildKhieuNai_Activity.ActivityChuyenPhanHoiTrungTam(KhieuNaiId, ConvertUtility.ToInt32(ddlTrungTamPhanHoi.SelectedValue, 0), "");
                string url = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";
                if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Equals(""))
                    url = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Chuyển phản hồi khiếu nại trung tâm thành công.','info', '" + url + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + ".','error');", true);
            }
        }

        protected void ddlNguyenNhanLoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nguyenNhanLoiId = ConvertUtility.ToInt32(ddlNguyenNhanLoi.SelectedItem.Value);
            if (nguyenNhanLoiId > 0)
            {
                LoadChiTietLoi(nguyenNhanLoiId);
                //string updateClause = string.Format("LDate=getdate(),LUser=N'{0}',LyDoGiamTru={1}, ChiTietLoiId=0", LoginAdmin.AdminLogin().Username, ddlNguyenNhanLoi.SelectedValue);
                //ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(updateClause, "Id=" + KhieuNaiId);
                //BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi nguyên nhân lỗi", "Nguyên nhân lỗi", ddlNguyenNhanLoi.Attributes["ValueOld"], ddlNguyenNhanLoi.SelectedItem.Text);

                //ddlChiTietLoi.Attributes["ValueOld"] = "";
                //ddlNguyenNhanLoi.Attributes["ValueOld"] = ddlNguyenNhanLoi.SelectedItem.Text;
            }
        }

        protected void ddlChiTietLoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int chiTietLoiId = ConvertUtility.ToInt32(ddlChiTietLoi.SelectedItem.Value);
            //if (chiTietLoiId > 0)
            //{                
            //    string updateClause = string.Format("LDate=getdate(),LUser=N'{0}', ChiTietLoiId={1}", LoginAdmin.AdminLogin().Username, ddlChiTietLoi.SelectedValue);
            //    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(updateClause, "Id=" + KhieuNaiId);
            //    BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi chi tiết lỗi", "Chi tiết lỗi", ddlNguyenNhanLoi.Attributes["ValueOld"], ddlNguyenNhanLoi.SelectedItem.Text);

            //    ddlChiTietLoi.Attributes["ValueOld"] = ddlChiTietLoi.SelectedItem.Text;
            //}       
        }

        private void LoadNguyenNhanLoi()
        {
            var keyLoiKhieuNaiCha = string.Concat("CLoiKhieuNai_", 0, "_", DateTime.Now.ToString("yyyyMMdd"));
            ddlNguyenNhanLoi.DataSource = Cache.Data<object>(keyLoiKhieuNaiCha, (60 * 60), () =>
            {
                return ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("*",
                string.Format("ParentId=0 AND HoatDong=1 AND TuNgay <= {0} AND DenNgay >= {0}", DateTime.Now.ToString("yyyyMMdd")), "ThuTu ASC"); ;
            });
            ddlNguyenNhanLoi.DataBind();
            ListItem item = new ListItem("Chọn nguyên nhân lỗi", "-1");
            ddlChiTietLoi.Items.Insert(0, item);
        }

        private void LoadChiTietLoi(int nguyenNhanLoiId)
        {
            var keyLoiKhieuNaiCha = string.Concat("CLoiKhieuNai_", nguyenNhanLoiId, "_", DateTime.Now.ToString("yyyyMMdd"));
            ddlChiTietLoi.DataSource = Cache.Data<object>(keyLoiKhieuNaiCha, (60 * 60), () =>
            {
                List<LoiKhieuNaiInfo> lst = ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("*",
                string.Format("HoatDong=1 AND ParentId={0} AND TuNgay <= {1} AND DenNgay >= {1}", nguyenNhanLoiId, DateTime.Now.ToString("yyyyMMdd")), "");
                return lst;
            });

            ddlChiTietLoi.DataTextField = "TenLoi";
            ddlChiTietLoi.DataValueField = "Id";
            ddlChiTietLoi.DataBind();

            ListItem item = new ListItem("--Chọn chi tiết lỗi--", "-1");
            ddlChiTietLoi.Items.Insert(0, item);

            //item = new ListItem("Khác", "0");
            //ddlChiTietLoi.Items.Add(item);
        }

    }
}