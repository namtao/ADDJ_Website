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
    public partial class SuaKhieuNai : PageBase
    {
        private int KhieuNaiId = 0;
        private int Mode = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            //if (!UserRightImpl.CheckRightAdminnistrator().UserRead)
            //{
            //    Response.Redirect(Config.PathNotRight, false);
            //    return;
            //}            

            if (Utility.IsInteger(Request.QueryString["MaKN"])) KhieuNaiId = Convert.ToInt32(Request.QueryString["MaKN"]);

            hdKhieuNaiId.Value = KhieuNaiId.ToString();
            ucChiTietKhieuNai.KhieuNaiId = KhieuNaiId;
            ucChiTietKhieuNai.IsTraSau = false;

            if (!IsPostBack)
            {
                BindDropDownList();
                BindForm();
            }
            BindMode();
        }



        private void BindMode()
        {
            if (Request.QueryString["Mode"] != null && !Request.QueryString["Mode"].Equals(""))
            {
                if (Request.QueryString["Mode"].ToLower().Equals("edit"))
                {
                    Mode = 2;
                }
            }
            else
                Mode = 0;
        }

        private KhieuNaiInfo BindProcess()
        {
            var item = ServiceFactory.GetInstanceKhieuNai().GetInfo(KhieuNaiId);
            if (item == null)
                return null;

            if (item.IsTraSau)
                ltLoaiThueBao.Text = "Trả sau";
            else
                ltLoaiThueBao.Text = "Trả trước";


            ucChiTietKhieuNai.IsTraSau = item.IsTraSau;

            if (Request.QueryString["Mode"] != null && !Request.QueryString["Mode"].Equals(""))
            {
                if (Request.QueryString["Mode"].ToLower().Equals("edit"))
                {
                    if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xử_lý_khiếu_nại))
                    {
                        Response.Redirect("/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + KhieuNaiId + "&Mode=View&ctrl=" + Request.QueryString["ctrl"]);
                        return null;
                    }

                    if (item.TrangThai == (byte)KhieuNai_TrangThai_Type.Đóng)
                    {
                        Response.Redirect("/Views/ChiTietKhieuNai/SuaKhieuNai.aspx?MaKN=" + KhieuNaiId + "&Mode=View&ctrl=" + Request.QueryString["ctrl"]);
                        return null;
                    }

                    Mode = 2;

                    //Kiểm tra xem có hợp lệ không?
                    var userLogin = LoginAdmin.AdminLogin();

                    //Nếu là KN phản hồi về cần kiểm tra phòng ban và update activity
                    if (item.PhongBanXuLyId == userLogin.PhongBanId)
                    {
                        if (item.NguoiXuLy == string.Empty)
                        {
                            string strUpdateKN = "LDate=getdate(),LUser=N'" + userLogin.Username + "',NguoiXuLy='" + userLogin.Username + "'";

                            using (TransactionScope scope = new TransactionScope())
                            {
                                try
                                {
                                    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(strUpdateKN, "Id=" + KhieuNaiId);
                                    ServiceFactory.GetInstanceKhieuNai_Activity().UpdateDynamic("LDate=getdate(),NguoiXuLy='" + userLogin.Username + "'", "IsCurrent = 1 AND KhieuNaiId=" + KhieuNaiId);
                                    scope.Complete();
                                }
                                catch
                                {
                                    Response.Redirect("/Views/ChiTietKhieuNai/SuaKhieuNai.aspx?MaKN=" + KhieuNaiId + "&Mode=View&ctrl=" + Request.QueryString["ctrl"]);
                                }
                            }
                        }
                        else if (item.NguoiXuLy == userLogin.Username)
                        {

                        }
                        else if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_KN_phản_hồi_của_phòng_ban_sau_thời_gian_cấu_hình)
                            && (DateTime.Now - item.LDate).TotalHours >= Config.TimeEditKhieuNai)
                        {

                            string strUpdateKN = "LDate=getdate(),LUser=N'" + userLogin.Username + "',NguoiXuLy='" + userLogin.Username + "'";

                            using (TransactionScope scope = new TransactionScope())
                            {
                                try
                                {
                                    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(strUpdateKN, "Id=" + KhieuNaiId);
                                    ServiceFactory.GetInstanceKhieuNai_Activity().UpdateDynamic("LDate=getdate(),NguoiXuLy='" + userLogin.Username + "'", "IsCurrent = 1 AND KhieuNaiId=" + KhieuNaiId);
                                    scope.Complete();
                                }
                                catch
                                {
                                    Response.Redirect("/Views/ChiTietKhieuNai/SuaKhieuNai.aspx?MaKN=" + KhieuNaiId + "&Mode=View&ctrl=" + Request.QueryString["ctrl"]);
                                }
                            }

                        }
                        else
                        {
                            Response.Redirect("/Views/ChiTietKhieuNai/SuaKhieuNai.aspx?MaKN=" + KhieuNaiId + "&Mode=View&ctrl=" + Request.QueryString["ctrl"]);
                        }
                    }
                    else
                    {
                        Response.Redirect("/Views/ChiTietKhieuNai/SuaKhieuNai.aspx?MaKN=" + KhieuNaiId + "&Mode=View&ctrl=" + Request.QueryString["ctrl"]);
                    }
                }
                else
                {
                    Mode = 0;
                    //ModalPopupExtender1.Enabled = false;
                    BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Xem khiếu nại", "[Hành động]");
                }
            }
            else
            {
                Response.Redirect("/Views/ChiTietKhieuNai/SuaKhieuNai.aspx?MaKN=" + KhieuNaiId + "&Mode=View");
            }
            return item;
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

            var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId is null", "Name");
            lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Chọn Tỉnh/Thành Phố--" });
            ddlTinh.DataSource = lstTinh;
            ddlTinh.DataTextField = "Name";
            ddlTinh.DataValueField = "Id";
            ddlTinh.DataBind();

            var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
            ddlLoaiKhieuNai.DataSource = lstLoaiKN;
            ddlLoaiKhieuNai.DataTextField = "Name";
            ddlLoaiKhieuNai.DataValueField = "Id";
            ddlLoaiKhieuNai.DataBind();
        }

        private void BindForm()
        {
            if (KhieuNaiId != 0)
            {
                var KhieuNaiItem = BindProcess();

                if (KhieuNaiItem == null)
                    return;

                ltMaKhieuNai.Text = GetDataImpl.GetMaTuDong("PA", KhieuNaiItem.Id, 10);

                txtSoThueBao.Text = KhieuNaiItem.SoThueBao.ToString();
                txtHoTen.Text = KhieuNaiItem.HoTenLienHe;
                txtDienThoaiLienHe.Text = KhieuNaiItem.SDTLienHe;
                txtThoiGianSuCo.Text = KhieuNaiItem.ThoiGianXayRa;
                txtDiaDiemSuCo.Text = KhieuNaiItem.DiaDiemXayRa;
                txtNoiDung.Text = KhieuNaiItem.NoiDungPA;
                txtDiaChiLienHe.Text = KhieuNaiItem.DiaChiLienHe;
                ddlTinh.SelectedValue = KhieuNaiItem.MaTinhId.ToString();
                ChangeTinh();
                ddlHuyen.SelectedValue = KhieuNaiItem.MaQuanId.ToString();
                ddlHTTiepNhan.SelectedValue = KhieuNaiItem.HTTiepNhan.ToString();

                txtNoiDungCanHoTro.Text = KhieuNaiItem.NoiDungCanHoTro;

                ltTrangThai.Text = Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNaiItem.TrangThai).Replace("_", " ");
                if (KhieuNaiItem.TrangThai == (byte)KhieuNai_TrangThai_Type.Chờ_đóng)
                    chkKNChoDong.Checked = true;

                string strFileName = string.Empty;
                var urlFile = BuildKhieuNai_FileDinhKem.BuildURLFileDinhKemKhachHang(KhieuNaiId, out strFileName);
                ltFileKH.Text = string.Format("<a href='{0}' target='_blank' alt='Click to download file'>{1}</a>", urlFile, strFileName);

                ltNguoiTiepNhan.Text = KhieuNaiItem.NguoiTiepNhan;
                ltNgayCapNhat.Text = KhieuNaiItem.LDate.ToString("dd/MM/yyyy HH:mm");

                ddlDoUuTien.SelectedValue = KhieuNaiItem.DoUuTien.ToString();
                ddlDoUuTien.Attributes.Add("ValueOld", ddlDoUuTien.SelectedItem.Text);
                chkHangLoat.Checked = KhieuNaiItem.KNHangLoat;
                chkHangLoat.Attributes.Add("ValueOld", KhieuNaiItem.KNHangLoat ? "Có" : "Không");

                //Nếu đóng thì lấy ngày tiếp nhận và ngày quá hạn
                if ((byte)KhieuNai_TrangThai_Type.Đóng == KhieuNaiItem.TrangThai)
                {
                    ltNgayTiepNhan.Text = KhieuNaiItem.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm");
                    ltThoiHan.Text = KhieuNaiItem.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");

                    //Nếu khiếu nại đã đóng sẽ có ngày đóng
                    ltNgayDong.Text = KhieuNaiItem.NgayDongKN.ToString("dd/MM/yyyy HH:mm");
                }
                else  //Nếu đang xử lý thì lấy ngày chuyển và ngày quá hạn phòng ban.
                {
                    ltNgayTiepNhan.Text = KhieuNaiItem.NgayChuyenPhongBan.ToString("dd/MM/yyyy HH:mm");
                    ltThoiHan.Text = KhieuNaiItem.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm");
                }
                ltNguoiXuLy.Text = KhieuNaiItem.NguoiXuLy;

                txtGhiChu.Text = KhieuNaiItem.GhiChu;
                txtGhiChu.Attributes.Add("ValueOld", KhieuNaiItem.GhiChu);

                ddlLoaiKhieuNai.SelectedValue = KhieuNaiItem.LoaiKhieuNaiId.ToString();
                ddlLoaiKhieuNai.Attributes.Add("ValueOld", ddlLoaiKhieuNai.SelectedItem.Text);
                ChangeLoaiKhieuNai();


                if (KhieuNaiItem.LinhVucChungId != 0)
                {
                    ddlLinhVucChung.SelectedValue = KhieuNaiItem.LinhVucChungId.ToString();
                    ddlLinhVucChung.Attributes.Add("ValueOld", ddlLinhVucChung.SelectedItem.Text);
                }
                else
                    ddlLinhVucChung.Items.Insert(0, new ListItem("", "0"));

                ChangeLinhVucChung();
                if (KhieuNaiItem.LinhVucConId != 0)
                {
                    ddlLinhVucCon.SelectedValue = KhieuNaiItem.LinhVucConId.ToString();
                    ddlLinhVucCon.Attributes.Add("ValueOld", ddlLinhVucCon.SelectedItem.Text);
                }
                else
                    ddlLinhVucCon.Items.Insert(0, new ListItem("", "0"));

                txtSoThueBao.ReadOnly = true;
                txtSoThueBao.Enabled = false;
                if (Mode == 0)
                {
                    //btChuyenXuLy.Enabled = false;
                    ddlDoUuTien.Enabled = false;
                    ddlLinhVucChung.Enabled = false;
                    ddlLinhVucCon.Enabled = false;
                    ddlLoaiKhieuNai.Enabled = false;
                    txtGhiChu.ReadOnly = true;
                    chkHangLoat.Enabled = false;
                    ddlTinh.Enabled = false;
                    ddlHuyen.Enabled = false;
                    ddlHTTiepNhan.Enabled = false;
                    chkKNChoDong.Enabled = false;
                    //btTrangThai.Enabled = false;

                    txtHoTen.ReadOnly = true;
                    txtDienThoaiLienHe.ReadOnly = true;
                    txtThoiGianSuCo.ReadOnly = true;
                    txtDiaDiemSuCo.ReadOnly = true;
                    txtNoiDung.ReadOnly = true;
                    txtDiaChiLienHe.ReadOnly = true;
                    txtNoiDungCanHoTro.ReadOnly = true;
                }
                else if (Mode == 2)
                {
                    txtHoTen.ReadOnly = true;
                    txtDienThoaiLienHe.ReadOnly = true;
                    txtThoiGianSuCo.ReadOnly = true;
                    txtDiaDiemSuCo.ReadOnly = true;
                    txtNoiDung.ReadOnly = true;
                    txtDiaChiLienHe.ReadOnly = true;
                    txtNoiDungCanHoTro.ReadOnly = true;
                    ddlTinh.Enabled = false;
                    ddlHuyen.Enabled = false;
                    ddlHTTiepNhan.Enabled = false;

                    //txtHoTen.Attributes.Add("ValueOld", KhieuNaiItem.HoTenLienHe);
                    //txtDienThoaiLienHe.Attributes.Add("ValueOld", KhieuNaiItem.SDTLienHe);
                    //txtThoiGianSuCo.Attributes.Add("ValueOld", KhieuNaiItem.ThoiGianXayRa);
                    //txtDiaDiemSuCo.Attributes.Add("ValueOld", KhieuNaiItem.DiaDiemXayRa);
                    //txtNoiDung.Attributes.Add("ValueOld", KhieuNaiItem.NoiDungPA);
                    //txtDiaChiLienHe.Attributes.Add("ValueOld", KhieuNaiItem.DiaChiLienHe);
                    //txtNoiDungCanHoTro.Attributes.Add("ValueOld", KhieuNaiItem.NoiDungCanHoTro);
                    //ddlTinh.Attributes.Add("ValueOld", ddlTinh.SelectedItem.Text);
                    //ddlHuyen.Attributes.Add("ValueOld", ddlHuyen.SelectedItem.Text);
                    //ddlHTTiepNhan.Attributes.Add("ValueOld", ddlHTTiepNhan.SelectedItem.Text);
                }
                else
                {
                    //btChuyenXuLy.Enabled = false;

                    ddlDoUuTien.Enabled = false;
                    ddlLinhVucChung.Enabled = false;
                    ddlLinhVucCon.Enabled = false;
                    ddlLoaiKhieuNai.Enabled = false;
                    txtGhiChu.ReadOnly = true;
                    chkHangLoat.Enabled = false;
                    ddlTinh.Enabled = false;
                    ddlHuyen.Enabled = false;
                    ddlHTTiepNhan.Enabled = false;
                    chkKNChoDong.Enabled = false;
                    //btTrangThai.Enabled = false;

                    txtHoTen.ReadOnly = true;
                    txtDienThoaiLienHe.ReadOnly = true;
                    txtThoiGianSuCo.ReadOnly = true;
                    txtDiaDiemSuCo.ReadOnly = true;
                    txtNoiDung.ReadOnly = true;
                    txtDiaChiLienHe.ReadOnly = true;
                    txtNoiDungCanHoTro.ReadOnly = true;
                }

                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_khiếu_nại))
                {
                    ddlLinhVucChung.Enabled = false;
                    ddlLinhVucCon.Enabled = false;
                    ddlLoaiKhieuNai.Enabled = false;

                    ddlTinh.Enabled = false;
                    ddlHuyen.Enabled = false;
                    ddlHTTiepNhan.Enabled = false;

                    txtHoTen.ReadOnly = true;
                    txtDienThoaiLienHe.ReadOnly = true;
                    txtThoiGianSuCo.ReadOnly = true;
                    txtDiaDiemSuCo.ReadOnly = true;
                    txtNoiDung.ReadOnly = true;
                    txtDiaChiLienHe.ReadOnly = true;
                    txtNoiDungCanHoTro.ReadOnly = true;
                }

                txtNoiDung.ReadOnly = true;
            }
        }

        #region Change Control Form Xu Ly
        protected void ddlLoaiKhieuNai_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ChangeLoaiKhieuNai();

            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',LinhVucChungId=0,LinhVucConId=0,LinhVucChung='',LinhVucCon='',LoaiKhieuNai=N'" + ddlLoaiKhieuNai.SelectedItem.Text + "', LoaiKhieuNaiId=" + ddlLoaiKhieuNai.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi loại khiếu nại", "Loại khiếu nại", ddlLoaiKhieuNai.Attributes["ValueOld"], ddlLoaiKhieuNai.SelectedItem.Text);
            ddlLinhVucCon.Attributes["ValueOld"] = "";
            ddlLinhVucChung.Attributes["ValueOld"] = "";
            ddlLoaiKhieuNai.Attributes["ValueOld"] = ddlLoaiKhieuNai.SelectedItem.Text;
        }

        private void ChangeLoaiKhieuNai()
        {
            var lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + ddlLoaiKhieuNai.SelectedValue, "Sort");
            lstLinhVucChung.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "--Chọn Lĩnh vực chung--" });
            ddlLinhVucChung.DataSource = lstLinhVucChung;
            ddlLinhVucChung.DataTextField = "Name";
            ddlLinhVucChung.DataValueField = "Id";
            ddlLinhVucChung.DataBind();
        }

        protected void ddlLinhVucChung_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ChangeLinhVucChung();
            if (ddlLinhVucChung.SelectedValue.Equals("0"))
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',LinhVucCon='',LinhVucConId=0,LinhVucChung=N'" + ddlLinhVucChung.SelectedItem.Text + "', LinhVucChungId=" + ddlLinhVucChung.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi lĩnh vực chung", "Lĩnh vực chung", ddlLinhVucChung.Attributes["ValueOld"], ddlLinhVucChung.SelectedItem.Text);
            ddlLinhVucCon.Attributes["ValueOld"] = "";
            ddlLinhVucChung.Attributes["ValueOld"] = ddlLinhVucChung.SelectedItem.Text;
        }

        private void ChangeLinhVucChung()
        {
            if (!ddlLinhVucChung.SelectedValue.Equals("0"))
            {
                var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + ddlLinhVucChung.SelectedValue, "Sort");
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
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',LinhVucCon=N'" + ddlLinhVucCon.SelectedItem.Text + "', LinhVucConId=" + ddlLinhVucCon.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi lĩnh vực con", "Lĩnh vực con", ddlLinhVucCon.Attributes["ValueOld"], ddlLinhVucCon.SelectedItem.Text);
            ddlLinhVucCon.Attributes["ValueOld"] = ddlLinhVucCon.SelectedItem.Text;
        }

        protected void ddlDoUuTien_Changed(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', DoUuTien=" + ddlDoUuTien.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi độ ưu tiên", "Độ ưu tiên", ddlDoUuTien.Attributes["ValueOld"], ddlDoUuTien.SelectedItem.Text);
            ddlDoUuTien.Attributes["ValueOld"] = ddlDoUuTien.SelectedItem.Text;
        }

        protected void txtHoTen_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', HoTenLienHe=N'" + txtHoTen.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi họ tên liên hệ", "Họ tên liên hệ", txtHoTen.Attributes["ValueOld"], txtHoTen.Text);
            txtHoTen.Attributes["ValueOld"] = txtHoTen.Text;
        }

        protected void txtDienThoaiLienHe_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', SDTLienHe=N'" + txtDienThoaiLienHe.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi số điện thoại liên hệ", "Điện thoại liên hệ", txtDienThoaiLienHe.Attributes["ValueOld"], txtDienThoaiLienHe.Text);
            txtDienThoaiLienHe.Attributes["ValueOld"] = txtDienThoaiLienHe.Text;
        }

        protected void txtThoiGianSuCo_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', ThoiGianXayRa=N'" + txtThoiGianSuCo.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi thời gian xảy ra sự cố", "Thời gian xảy ra sự cố", txtThoiGianSuCo.Attributes["ValueOld"], txtThoiGianSuCo.Text);
            txtThoiGianSuCo.Attributes["ValueOld"] = txtThoiGianSuCo.Text;
        }

        protected void txtDiaChiLienHe_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', DiaChiLienHe=N'" + txtDiaChiLienHe.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi địa chỉ liên hệ", "Địa chỉ liên hệ", txtDiaChiLienHe.Attributes["ValueOld"], txtDiaChiLienHe.Text);
            txtDiaChiLienHe.Attributes["ValueOld"] = txtDiaChiLienHe.Text;
        }

        protected void txtDiaDiemSuCo_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', DiaDiemXayRa=N'" + txtDiaDiemSuCo.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi địa điểm xảy ra sự cố", "Địa điểm xảy ra sự cố", txtDiaDiemSuCo.Attributes["ValueOld"], txtDiaDiemSuCo.Text);
            txtDiaDiemSuCo.Attributes["ValueOld"] = txtDiaDiemSuCo.Text;
        }

        protected void txtNoiDung_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', NoiDungPA=N'" + txtNoiDung.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi nội dung phản ánh", "Nội dung phản ánh", txtNoiDung.Attributes["ValueOld"], txtNoiDung.Text);
            txtNoiDung.Attributes["ValueOld"] = txtNoiDung.Text;
        }

        protected void txtNoiDungCanHoTro_TextChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', NoiDungCanHoTro=N'" + txtNoiDungCanHoTro.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi nội dung cần hỗ trợ", "Nội dung cần hỗ trợ", txtNoiDungCanHoTro.Attributes["ValueOld"], txtNoiDungCanHoTro.Text);
            txtNoiDungCanHoTro.Attributes["ValueOld"] = txtNoiDungCanHoTro.Text;
        }

        protected void ddlTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ChangeTinh();
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',MaTinhId=" + ddlTinh.SelectedValue + ",  MaTinh=N'" + ddlTinh.SelectedItem.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, @"Thay đổi nội tình\thành phố", "Tỉnh/Thành phố", ddlTinh.Attributes["ValueOld"], ddlTinh.SelectedItem.Text);
            ddlTinh.Attributes["ValueOld"] = ddlTinh.SelectedItem.Text;
        }

        private void ChangeTinh()
        {
            var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + ddlTinh.SelectedValue, "Name");
            lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Chọn Quận/Huyện--" });
            ddlHuyen.DataSource = lstTinh;
            ddlHuyen.DataTextField = "Name";
            ddlHuyen.DataValueField = "Id";
            ddlHuyen.DataBind();
        }

        protected void ddlHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',MaQuanId=" + ddlHuyen.SelectedValue + ", MaQuan=N'" + ddlHuyen.SelectedItem.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, @"Thay đổi nội quận\huyện", "Quận/Huyện", ddlHuyen.Attributes["ValueOld"], ddlHuyen.SelectedItem.Text);
            ddlHuyen.Attributes["ValueOld"] = ddlHuyen.SelectedItem.Text;
        }

        protected void ddlHTTiepNhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',HTTiepNhan=" + ddlHTTiepNhan.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, @"Thay đổi nội hình thức tiếp nhận", "Hình thức tiếp nhận", ddlHuyen.Attributes["ValueOld"], ddlHTTiepNhan.SelectedItem.Text);
            ddlHTTiepNhan.Attributes["ValueOld"] = ddlHTTiepNhan.SelectedItem.Text;
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

            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi trạng thái thành " + (isChoDong ? Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " ") : Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " ")), "Trạng thái",
                isChoDong ? Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " ") : Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " "),
                isChoDong ? Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " ") : Enum.GetName(typeof(KhieuNai_TrangThai_Type), KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " "));
        }

        protected void chkHangLoat_CheckedChanged(object sender, EventArgs e)
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
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', GhiChu=N'" + txtGhiChu.Text + "'", "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi ghi chú", "Ghi chú", txtGhiChu.Attributes["ValueOld"], txtGhiChu.Text);
            txtGhiChu.Attributes["ValueOld"] = txtGhiChu.Text;
        }

        protected void ddlDoUuTien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "', DoUuTien=" + ddlDoUuTien.SelectedValue, "Id=" + KhieuNaiId);
            BuildKhieuNai_Log.LogKhieuNai(KhieuNaiId, "Thay đổi độ ưu tiên", "Độ ưu tiên", ddlDoUuTien.Attributes["ValueOld"], ddlDoUuTien.SelectedItem.Text);
            ddlDoUuTien.Attributes["ValueOld"] = ddlDoUuTien.SelectedItem.Text;
        }
        #endregion

        private bool ValidForm()
        {
            if (txtNoiDung.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn chưa nhập nội dung phản ánh.','error','" + txtNoiDung.ClientID + "');", true);
                return false;
            }
            return true;
        }

        protected void btnOkay_Click(object sender, EventArgs e)
        {
            if (Mode == 0)
                return;
            if (ValidForm())
            {
                var userLogin = LoginAdmin.AdminLogin();

                if (userLogin != null && userLogin.PhongBanId == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Người dùng này chưa được phân vào phòng nên không có quyền tạo khiếu nại.','error');", true);
                    return;
                }

                var timeNow = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

                var item = ServiceFactory.GetInstanceKhieuNai().GetInfo(KhieuNaiId);

                var loaiKNIdSelect = item.LoaiKhieuNaiId;
                if (item.LinhVucConId != 0)
                {
                    loaiKNIdSelect = item.LinhVucConId;
                }
                //Nếu chọn lĩnh vực chung
                else if (item.LinhVucChungId != 0)
                {
                    loaiKNIdSelect = item.LinhVucChungId;
                }

                //Tính ngày quá hạn và ngày cảnh báo
                item.NgayQuaHan = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect);
                item.NgayQuaHanSort = Convert.ToInt32(item.NgayQuaHan.ToString("yyyyMMdd"));
                item.NgayCanhBao = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect, 2);
                item.NgayCanhBaoSort = Convert.ToInt32(item.NgayCanhBao.ToString("yyyyMMdd"));

                //Phòng ban cần chuyển KN                    

                //Lấy ra List phòng ban chuyển của phòng ban mình
                var lstPhongBanDuocChuyen = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(userLogin.PhongBanId);
                if (lstPhongBanDuocChuyen == null || lstPhongBanDuocChuyen.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không tìm thấy phòng ban nào cần chuyển khiếu nại.','error');", true);
                    return;
                }

                var phongBanDuocChuyen_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(lstPhongBanDuocChuyen[0].PhongBanDen);
                if (phongBanDuocChuyen_JSON == null || phongBanDuocChuyen_JSON.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không tìm thấy phòng ban nào cần chuyển khiếu nại.','error');", true);
                    return;
                }

                //Nếu chỉ có 1 phòng ban được chuyển khiếu nại => Tất cả khiếu nại sẽ được chuyển đến đây.
                if (phongBanDuocChuyen_JSON.Count == 1)
                {
                    item.PhongBanXuLyId = phongBanDuocChuyen_JSON[0];
                }
                else //Nếu lớn hơn 2 phòng ban thì cần xét duyệt từng loại khiếu nại đến phòng ban nào trong list.
                {
                    //Lấy ra phòng ban có thể xử lý loại khiếu nại.
                    var lstPhongBanXuLy = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetListByLoaiKhieuNaiId(loaiKNIdSelect);
                    if (lstPhongBanXuLy == null || lstPhongBanXuLy.Count == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Loại khiếu nại này chưa được cấu hình phòng ban xử lý.','error');", true);
                        return;
                    }
                    //Kiem tra xem phong ban co khong
                    foreach (var lstPhongBanXuLyItem in lstPhongBanXuLy)
                        if (phongBanDuocChuyen_JSON.Contains(lstPhongBanXuLyItem.PhongBanId))
                        {
                            item.PhongBanXuLyId = lstPhongBanXuLyItem.PhongBanId;
                            break;
                        }
                }

                //Lấy ra loại phòng ban của phòng ban xử lý
                var LoaiPhongBanXuLy = 0;

                var PhongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanXuLyId);

                if (PhongBanXuLyItem != null)
                    LoaiPhongBanXuLy = PhongBanXuLyItem.LoaiPhongBanId;

                if (LoaiPhongBanXuLy == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Phòng ban xử lý khiếu nại chưa nằm trong loại phòng ban nào.','error');", true);
                    return;
                }

                item.NgayChuyenPhongBan = timeNow;
                item.NgayQuaHanPhongBanXuLy = GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, LoaiPhongBanXuLy);
                item.NgayCanhBaoPhongBanXuLy = GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, LoaiPhongBanXuLy, 2);
                item.LUser = userLogin.Username;
                item.NguoiXuLy = string.Empty;

                ServiceFactory.GetInstanceKhieuNai().Update(item);

                var activityCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(KhieuNaiId);

                KhieuNai_ActivityInfo itemActivity = new KhieuNai_ActivityInfo();
                itemActivity.KhieuNaiId = KhieuNaiId;
                itemActivity.ActivityTruoc = activityCurr.Id;
                itemActivity.GhiChu = "Chuyển xử lý khiếu nại";
                itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                itemActivity.IsCurrent = true;
                itemActivity.NguoiXuLyTruoc = userLogin.Username;
                itemActivity.PhongBanXuLyTruocId = item.PhongBanTiepNhanId;
                itemActivity.PhongBanXuLyId = item.PhongBanXuLyId;
                itemActivity.NgayTiepNhan = item.NgayTiepNhan;
                itemActivity.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
                itemActivity.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;

                ServiceFactory.GetInstanceKhieuNai_Activity().UpdateDynamic("LDate=getdate(),IsCurrent=0", "IsCurrent=1 AND KhieuNaiId=" + KhieuNaiId);
                ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemActivity);

                string url = "/Views/QLKhieuNai/MyKhieuNai.aspx";
                if (Request.QueryString["ctrl"] != null && !Request.QueryString["ctrl"].Equals(""))
                    url += "?ctrl=" + Request.QueryString["ctrl"];

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Chuyển xử lý khiếu nại thành công','info','" + url + "');", true);
            }
        }
    }
}