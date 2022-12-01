using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType_DateRange : System.Web.UI.UserControl
    {
        public string ReportType { get; set; }
        public int DoiTacId { get; set; }
        public int PhongBanXuLyId { get; set; }
        public string NguoiXuLy { get; set; }
        public string ReportTitle { get; set; }

        // Biến IsFirstLoad : để xác định usercontrol có phải là load lần đầu tiên không (biến này được truyền vào từ page khác)
        // vì IsPostBack ăn theo Page nên không thể xác định được đối với user control       
        public bool IsFirstLoad { get; set; }

        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePnl, UpdatePnl.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
            ddlTinh.AutoPostBack = true;
            ddlTinh.SelectedIndexChanged += DdlTinh_SelectedIndexChanged;

            if (IsFirstLoad)
            {

                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblNguoiXuLy.Text = this.NguoiXuLy;
                lblDoiTacId.Text = this.DoiTacId.ToString();

                ddlNguonKhieuNai.DataSource = ServiceFactory.GetInstanceKhieuNai().GetListNguonKhieuNai(true);
                ddlNguonKhieuNai.DataTextField = "Name";
                ddlNguonKhieuNai.DataValueField = "Id";
                ddlNguonKhieuNai.DataBind();

                rowNguonKhieuNai.Visible = LoginAdmin.AdminLogin().PhongBanId == PhongBanInfo.PhongBanValueId.PHONG_CSKH_VNP;

                BindTinh();
                BindHuyen();
            }
        }

        private void DdlTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindHuyen();
        }

        private void BindTinh()
        {
            List<ProvinceInfo> lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id, Name", "ParentId IS NULL OR ParentId = 0", "Name");
            lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "-- Tỉnh/thành phố --" });

            ddlTinh.DataSource = lstTinh;
            ddlTinh.DataTextField = "Name";
            ddlTinh.DataValueField = "Id";

            ddlTinh.DataBind();
        }

        private void BindHuyen()
        {
            string selected = ddlTinh.SelectedValue;
            List<ProvinceInfo> lstQuanHuyen = ServiceFactory.GetInstanceProvince().GetListDynamic("Id, Name", "ParentId = " + selected, "Name");
            lstQuanHuyen.Insert(0, new ProvinceInfo() { Id = 0, Name = "-- Quận/huyện --" });

            ddlHuyen.DataSource = lstQuanHuyen;
            ddlHuyen.DataTextField = "Name";
            ddlHuyen.DataValueField = "Id";
            ddlHuyen.DataBind();

        }
        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/11/2013
        /// Todo : Chuyển sang trang hiển thị báo cáo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbShowReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType.SelectedItem.Value;
            string nguonKhieuNai = ddlNguonKhieuNai.SelectedValue;

            string tinhId = ddlTinh.SelectedValue;
            string huyenId = ddlHuyen.SelectedValue;

            string errorMessage = string.Empty;
            string script = string.Empty;

            if (fromDate.Length == 0 || toDate.Length == 0)
            {
                errorMessage = string.Format("{0}\\nBạn phải nhập ngày báo cáo", errorMessage);
            }
            else
            {
                DateTime dateCheck = ConvertUtility.ToDateTime(fromDate, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nTừ ngày không hợp lệ", errorMessage);
                }

                dateCheck = ConvertUtility.ToDateTime(toDate, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nĐến ngày không hợp lệ", errorMessage);
                }
            }

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                string page = string.Empty;
                switch (lblReportType.Text)
                {
                    case "bc_Common_PhongBanCaNhan_TongHopPhongBan":
                    case "bc_CSKHKV_BaoCaoTongHopTaiPhongBan":
                        //title = "Báo cáo số liệu phản ánh khiếu nại đã xử lý";
                        page = "baocaotonghopphongban.aspx";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}?doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblDoiTacId.Text, lblPhongBanXuLyId.Text, fromDate, toDate, loaibc);
                        break;

                    case "bc_Common_PhongBanCaNhan_TongHopNguoiDung":
                    case "bc_CSKHKV_BaoCaoTongHopNguoiDungTaiPhongBan":
                        page = "baocaotonghoppakntheonguoidungvnpttt.aspx?";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doiTacId={2}&fromDate={3}&toDate={4}&loaibc={5}&phongBanId={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblDoiTacId.Text, fromDate, toDate, loaibc, lblPhongBanXuLyId.Text);
                        break;

                    case "bc_VNPTTT_TongHopPAKNTVTT":
                    case "bc_Common_TongHopDonVi":
                        page = "baocaotonghopdoitac.aspx";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}?doiTacId={2}&fromDate={3}&toDate={4}&loaibc={5}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblDoiTacId.Text, fromDate, toDate, loaibc);
                        break;

                    case "bc_VNPTTT_TongHopPAKNPhongBanVTT":
                        List<PhongBanInfo> listPhongBanInfo = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfParentId(ConvertUtility.ToInt32(lblPhongBanXuLyId.Text));
                        string sPhongBanId = string.Empty;
                        if (listPhongBanInfo != null && listPhongBanInfo.Count > 0)
                        {
                            sPhongBanId = listPhongBanInfo[0].Id.ToString();
                            for (int i = 1; i < listPhongBanInfo.Count; i++)
                            {
                                sPhongBanId = string.Format("{0},{1}", sPhongBanId, listPhongBanInfo[i].Id);
                            }
                        }
                        page = "baocaotonghopphongban.aspx";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}?doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblDoiTacId.Text, sPhongBanId, fromDate, toDate, loaibc);
                        break;

                    case "bc_Common_TongHopPhongBan":
                        List<PhongBanInfo> listPhongBanInfo_Common = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(ConvertUtility.ToInt32(lblDoiTacId.Text));
                        string sPhongBanId_Common = string.Empty;
                        if (listPhongBanInfo_Common != null && listPhongBanInfo_Common.Count > 0)
                        {
                            sPhongBanId_Common = listPhongBanInfo_Common[0].Id.ToString();
                            for (int i = 1; i < listPhongBanInfo_Common.Count; i++)
                            {
                                sPhongBanId_Common = string.Format("{0},{1}", sPhongBanId_Common, listPhongBanInfo_Common[i].Id);
                            }
                        }
                        page = "baocaotonghopphongban.aspx";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}?doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblDoiTacId.Text, sPhongBanId_Common, fromDate, toDate, loaibc);

                        break;
                    // Edited by	: Dao Van Duong
                    // Datetime		: 10.8.2016 12:08
                    // Note			: Nâng cấp thêm lựa chon tỉnh thành, quận huyện
                    case "bc_CaNhan_TongHopCaNhan":
                        AdminInfo user = LoginAdmin.AdminLogin();
                        page = "BaoCaoTongHopPAKNTheoNguoiDungVNPTTT.aspx?";

                        // Edited by	: Dao Van Duong
                        // Datetime		: 9.8.2016 10:52
                        // Note			: Có UserName => Báo cáo chỉ cá nhân đó
                        // script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}DoiTacId={2}&FromDate={3}&ToDate={4}&Loaibc={5}&PhongBanId={6}&TinhId={7}&HuyenId={8}&TenTruyCap={9}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblDoiTacId.Text, fromDate, toDate, loaibc, lblPhongBanXuLyId.Text, tinhId, huyenId, user.Username);

                        // Edited by	: Dao Van Duong
                        // Datetime		: 9.8.2016 10:52
                        // Note			: Báo cáo phòng ban
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}DoiTacId={2}&FromDate={3}&ToDate={4}&Loaibc={5}&PhongBanId={6}&TinhId={7}&HuyenId={8}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblDoiTacId.Text, fromDate, toDate, loaibc, lblPhongBanXuLyId.Text, tinhId, huyenId);
                        break;

                    case "bc_VNP_KhieuNaiToanMangTheoTuan":
                    case "bc_VNP_KhieuNaiToanMangTheoThang":
                        page = "baocaotonghopkhieunaitoanmangtheotuanthang.aspx?";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}reportType={2}&fromDate={3}&toDate={4}&loaibc={5}&nguonKhieuNai={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblReportType.Text, fromDate, toDate, loaibc, nguonKhieuNai);
                        break;

                    case "bc_VNP_BaoCaoTongHopGiamTru":
                        page = "emptypage.aspx?";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page={2}&fromDate={3}&toDate={4}&loaibc={5}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblReportType.Text.ToLower(), fromDate, toDate, loaibc);
                        break;

                    case "bc_Common_BaoCaoTonDongNguoiDungPhongBan":
                        page = "emptypage.aspx?";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page={2}&phongBanXuLyId={3}&nguoiXuLy={4}&loaibc={5}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblReportType.Text.ToLower(), lblPhongBanXuLyId.Text, string.Empty, loaibc);
                        break;

                    case "bc_DoiTac_BaoCaoSoLuongChuyenXuLyVNP":
                        page = "baocaosoluongchuyenxulyvnp.aspx?";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page={2}&fromDate={3}&toDate={4}&loaibc={5}&doiTacId={6}&phongBanId={7}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblReportType.Text.ToLower(), fromDate, toDate, loaibc, lblDoiTacId.Text, lblPhongBanXuLyId.Text);
                        break;

                    case "bc_VNP_BaoCaoTonDongQuaHan":
                        page = "emptypage.aspx?";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page={2}&fromDate={3}&toDate={4}&loaibc={5}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblReportType.Text.ToLower(), fromDate, toDate, loaibc);
                        break;

                    case "bc_VNP_BaoCaoDVGTGTTapDoan":
                        page = "emptypage.aspx?";
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page={2}&fromDate={3}&toDate={4}&loaibc={5}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, lblReportType.Text.ToLower(), fromDate, toDate, loaibc);
                        break;

                    default:
                        page = string.Empty;
                        break;
                }


            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePnl, UpdatePnl.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai.ClientID + "');", true);
        }

        #endregion

        #region Private methods



        #endregion
    }
}