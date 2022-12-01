using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Website.Views.BaoCao.UC;

namespace Website.Views.BaoCao
{
    public partial class BaoCaoThongKeDKT : System.Web.UI.Page
    {
        protected string optLoaiKhieuNai = "";
        protected string optPhongBan = "";
        protected string optLinhVucCon_CTGT = "";

        protected string optDoitac_donvi = "";
        protected string optDoitac_dichvu = "";
        protected string optDoitac_lichhen = "";
        protected string optDoitac_doanhthu = "";
        protected string optDichvu = "";
        protected string optDichvu_doanhthu = "";

        protected string optDoitac_TraCuuThongTin = "";
        protected string optDichvu_TraCuuThongTin = "";
        protected string optKetQua = string.Empty;
        protected string optHaiLong = string.Empty;

        protected string strMaNguoiDung = string.Empty;
        protected string strLoaiBaoCao = string.Empty;

        private string LastLoadedControl
        {
            get
            {
                return ViewState["LastLoaded"] as string;
            }
            set
            {
                ViewState["LastLoaded"] = value;
            }
        }


        #region Event methods

        private void LoadUserControl()
        {
            string controlPath = LastLoadedControl;

            //if (!string.IsNullOrEmpty(controlPath))
            //{
            //    pnlContainer.Controls.Clear();
            //    UserControl uc = (UserControl)LoadControl(controlPath);
            //    pnlContainer.Controls.Add(uc);
            //}

            ShowReportUserControl(controlPath);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserRead)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            LoadUserControl();

            optLoaiKhieuNai = BuildBaoCao.GetLoaiKhieuNai();
            optPhongBan = BuildBaoCao.GetPhongBan();

            if (!IsPostBack)
            {
                var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
                ddlLoaiKhieuNai.DataSource = lstLoaiKN;
                ddlLoaiKhieuNai.DataBind();

                ListItem item = new ListItem("Chọn loại khiếu nại..", "-1");
                ddlLoaiKhieuNai.Items.Insert(0, item);

                LoadKhuVuc();
                ddlKhuVuc_ReportType1_SelectedIndexChanged(null, null);
                ddlKhuVuc_ReportType2_SelectedIndexChanged(null, null);
                LoadDoiTacVNPTTT();
                LoadComboPhongBan();
                LoadTreeLoaiKhieuNai();
            }
        }

        //protected void ddlPhongBan_BaoCaoTongHopTheoKhieuNai_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int phongBanId = ConvertUtility.ToInt32(ddlKhuVuc_ReportType2.SelectedValue);
        //    LoadDoiTacByPhongBan(phongBanId);

        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);
        //}

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/10/2013
        /// Todo : Hiển thị báo cáo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_ReportType1_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string reportType = lblReportType.Text; //lblReportType.Text;
            string title = string.Empty;
            string khuVucId = ddlKhuVuc_ReportType1.SelectedValue;
            string khuVuc = ddlKhuVuc_ReportType1.SelectedItem != null ? ddlKhuVuc_ReportType1.SelectedItem.Text : string.Empty;
            string donViId = ddlPhongBan_ReportType1.Visible ? ddlPhongBan_ReportType1.SelectedValue : "-1";
            string donVi = ddlPhongBan_ReportType1.SelectedItem != null ? ddlPhongBan_ReportType1.SelectedItem.Text : string.Empty;
            string doiTacId = ddlDoiTac_ReportType1.SelectedValue;
            string tenDoiTac = ddlDoiTac_ReportType1.SelectedItem != null ? ddlDoiTac_ReportType1.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;
            string loaiKhieuNaiId = ddlLoaiKhieuNai.SelectedValue;
            string loaiKhieuNai = ddlLoaiKhieuNai.SelectedItem != null ? ddlLoaiKhieuNai.SelectedItem.Text : string.Empty;
            string linhVucChungId = ddlLinhVucChung.SelectedValue;
            string linhVucChung = ddlLinhVucChung.SelectedItem != null ? ddlLinhVucChung.SelectedItem.Text : string.Empty;
            string linhVucConId = ddlLinhVucCon.SelectedValue;
            string linhVucCon = ddlLinhVucCon.SelectedItem != null ? ddlLinhVucCon.SelectedItem.Text : string.Empty;

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

            var page = "";
            if (reportType == "bc11")
            {
                page = "baocaochitietgiamtru.aspx";
                title = "Báo cáo chi tiết giảm trừ trả trước";
            }
            else if (reportType == "bc21")
            {
                page = "baocaotonghopgiamtru.aspx";
                title = "Báo cáo tổng hợp giảm trừ";
            }
            else if (reportType == "bc31")
            {
                page = "";
            }
            else if (reportType == "bc41")
            {
                page = "baocaochitietpps.aspx";
                title = "Báo cáo chi tiết PPS";
            }
            else if (reportType == "bc51")
            {
                page = "baocaochitietpost.aspx";
                title = "Báo cáo chi tiết POST";
            }
            else if (reportType == "bc61")
            {
                page = "";
            }
            else if (reportType == "bc71")
            {
                page = "danhsachkhieunai.aspx";
                title = "Danh sách khiếu nại";
            }
            else if (reportType == "bc81")
            {
                page = "baocaochitietgiamtrutrasau.aspx";
                title = "Báo cáo chi tiết giảm trừ trả sau";
            }            
            else if (reportType == "bcTTTC_TongHopPAKN")
            {
                page = "baocaotonghoppakntttc.aspx";
                title = "Báo cáo tổng hợp số liệu PAKN TTTC";
            }
            else if (reportType == "bcTTTC_TongHopPAKNTheoPhongBan")
            {
                page = "baocaotonghoppakntheophongbantttc.aspx";
                title = "Báo cáo tổng hợp số liệu PAKN theo phòng ban";
            }
            else if (reportType == "bcTTTC_TongHopPAKNTheoNguoiDung")
            {
                page = "baocaotonghoppakntheonguoidungtttc.aspx";
                title = "Báo cáo tổng hợp số liệu PAKN theo người dùng";

                if (donViId == "-1")
                {
                    errorMessage = string.Format("{0}\\nBạn phải chọn phòng ban", errorMessage);
                }
            }

            if (page == "")
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn loại báo cáo", errorMessage);
            }

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {                
                if (lblReportType.Text == "bcTTTC_TongHopPAKN" || lblReportType.Text == "bcTTTC_TongHopPAKNTheoPhongBan"
                    || lblReportType.Text == "bcTTTC_TongHopPAKNTheoNguoiDung")
                {
                    script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}?khuVucId={2}&fromDate={3}&toDate={4}&loaibc={5}&phongBanId={6}\">');</script>", title, page, khuVucId, fromDate, toDate, loaibc, donViId);
                }
                else if (lblReportType.Text == "bc81")
                {
                    script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}?doitacId={2}&tenDoiTac={3}&fromDate={4}&toDate={5}&loaiKhieuNaiID={6}&loaiKhieuNai={7}&linhVucChungID={8}&linhVucChung={9}&linhVucConID={10}&linhVucCon={11}&loaibc={12}&khuVuc={13}&khuVucId={14}&donViId={15}&donVi={16}\">');</script>", title, page, doiTacId, tenDoiTac, fromDate, toDate, loaiKhieuNaiId, loaiKhieuNai, linhVucChungId, linhVucChung, linhVucConId, linhVucCon, loaibc, khuVuc, khuVucId, donViId, donVi);
                }
                else
                {
                    script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}?khuVucID={2}&khuVuc={3}&donViID={4}&donVi={5}&fromDate={6}&toDate={7}&loaiKhieuNaiID={8}&loaiKhieuNai={9}&linhVucChungID={10}&linhVucChung={11}&linhVucConID={12}&linhVucCon={13}&loaibc={14}\">');</script>", title, page, khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaiKhieuNaiId, loaiKhieuNai, linhVucChungId, linhVucChung, linhVucConId, linhVucCon, loaibc);
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReport_ReportType2_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string khuVucId = lblKhuVucId_ReportType2.Text; //ddlKhuVuc_ReportType2.SelectedValue;
            string khuVuc = lblTenKhuVuc_ReportType2.Text; //ddlKhuVuc_ReportType2.SelectedItem != null ? ddlKhuVuc_ReportType2.SelectedItem.Text : string.Empty;
            string donViId = ddlPhongBan_ReportType2.SelectedValue;
            string donVi = ddlPhongBan_ReportType2.SelectedItem != null ? ddlPhongBan_ReportType2.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate_BaoCaoTongHopTheoKhieuNai.Text;
            string toDate = txtToDate_BaoCaoTongHopTheoKhieuNai.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string doiTac = string.Empty;
            string loaiKhieuNaiId = string.Empty;
            string linhVucChungId = string.Empty;
            string linhVucConId = string.Empty;

            foreach (ListItem item in cblDoiTac.Items)
            {
                if (item.Selected)
                {
                    doiTac = string.Format("{0}{1},", doiTac, item.Value);
                }
            }

            if (doiTac.Length > 0)
            {
                doiTac = doiTac.TrimEnd(',');
            }

            foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai_ReportType2.Nodes)
            {
                if (nodeLoaiKhieuNai.Checked)
                {
                    loaiKhieuNaiId = string.Format("{0}{1},", loaiKhieuNaiId, nodeLoaiKhieuNai.Value);
                }

                GetListLinhVucChung(nodeLoaiKhieuNai, ref linhVucChungId, ref linhVucConId);
            }

            if (loaiKhieuNaiId.Length > 0)
            {
                loaiKhieuNaiId = loaiKhieuNaiId.TrimEnd(',');
            }

            if (linhVucChungId.Length > 0)
            {
                linhVucChungId = linhVucChungId.TrimEnd(',');
            }

            if (linhVucConId.Length > 0)
            {
                linhVucConId = linhVucConId.TrimEnd(',');
            }

            string errorMessage = string.Empty;
            string script = string.Empty;

            if (lblReportType.Text == "bc31")
            {
                if (ConvertUtility.ToInt32(donViId) <= 0)
                {
                    errorMessage = string.Format("{0}\\nBạn phải chọn phòng ban", errorMessage);
                }

                if (doiTac.Length == 0)
                {
                    errorMessage = string.Format("{0}\\nBạn phải chọn đối tác", errorMessage);
                }
            }

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

            if (loaiKhieuNaiId.Length == 0 && linhVucChungId.Length == 0 && linhVucConId.Length == 0 && lblReportType.Text != "bc_XLNV_BaoCaoKhoiLuongCongViec")
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn loại khiếu nại", errorMessage);
            }


            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                if (lblReportType.Text == "bc61")
                {
                    script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo theo loại khiếu nại', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaotheoloaikhieunai.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaiKhieuNai={6}&linhVucChung={7}&linhVucCon={8}&loaibc={9}\">');</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);                    
                }
                else if (lblReportType.Text == "bc31" || lblReportType.Text == "bc_KS_BaoCaoTongHopKN" || lblReportType.Text == "bc_OB_BaoCaoTongHopKN")
                {
                    //string script = string.Format("<script type='text/javascript'>window.open('/Views/BaoCao/Popup/baocaotonghoptheokhieunai.aspx?donViID={0}&donVi={1}&fromDate={2}&toDate={3}&doiTac={4}&loaiKhieuNai={5}&linhVucChung={6}&linhVucCon={7}&loaibc={8}','Báo cáo tổng hợp theo khiếu nại', 'menubar=0, statusbar=0, titlebar=0,fullscreen=1');</script>", donViId, donVi, fromDate, toDate, doiTac, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);
                    script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo tổng hợp theo khiếu nại', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaotonghoptheokhieunai.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&doiTac={6}&loaiKhieuNai={7}&linhVucChung={8}&linhVucCon={9}&loaibc={10}\">');</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, doiTac, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);
                }
                else
                {
                    //string script = string.Format("<script type='text/javascript'>window.open('/Views/BaoCao/Popup/baocaotonghoptheokhieunai.aspx?donViID={0}&donVi={1}&fromDate={2}&toDate={3}&doiTac={4}&loaiKhieuNai={5}&linhVucChung={6}&linhVucCon={7}&loaibc={8}','Báo cáo tổng hợp theo khiếu nại', 'menubar=0, statusbar=0, titlebar=0,fullscreen=1');</script>", donViId, donVi, fromDate, toDate, doiTac, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);
                    script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo khối lượng công việc', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaokhoiluongcongviecdkt.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaiKhieuNai={6}&linhVucChung={7}&linhVucCon={8}&loaibc={9}\">');</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);
        }

        protected void ddlLoaiKhieuNai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoaiKhieuNai.SelectedItem != null && ddlLoaiKhieuNai.SelectedValue.Length > 0)
            {
                List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId =" + ddlLoaiKhieuNai.SelectedValue, "Name ASC");
                ddlLinhVucChung.DataSource = listLoaiKhieuNai;
                ddlLinhVucChung.DataBind();

                ListItem item = new ListItem("Chọn lĩnh vực chung..", "-1");
                ddlLinhVucChung.Items.Insert(0, item);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
            }
        }

        protected void ddlLinhVucChung_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLinhVucChung.SelectedItem != null && ddlLinhVucChung.SelectedValue.Length > 0)
            {
                List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId =" + ddlLinhVucChung.SelectedValue, "Name ASC");
                ddlLinhVucCon.DataSource = listLoaiKhieuNai;
                ddlLinhVucCon.DataBind();

                ListItem item = new ListItem("Chọn lĩnh vực con..", "-1");
                ddlLinhVucCon.Items.Insert(0, item);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 01/10/2013
        /// Todo : Hiển thị các report tương ứng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportCondition(lb.CommandArgument);
            LastLoadedControl = string.Empty;
            pnlContainer.Controls.Clear();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 01/10/2013
        /// Todo : Hiển thị các report tương ứng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReportUC_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowReportUserControl(lb.CommandArgument);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/09/2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideReportCondition(lblReportType.Text);
            //string tvLoaiKhieuNaiId = string.Empty;
            //string CUOC_TB_TRA_TRUOC = "30";
            //string CUOC_TB_TRA_SAU = "35";
            //switch (lblReportType.Text)
            //{
            //    case "bc11":
            //        lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả trước";
            //        ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
            //        ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
            //        break;
            //    case "bc21":
            //        lblTittle.Text = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";                   
            //        break;
            //    case "bc31":
            //        lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
            //        tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
            //        break;
            //    case "bc41":
            //        lblTittle.Text = "Báo cáo tổng hợp PPS";
            //        ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
            //        ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
            //        break;
            //    case "bc51":
            //        lblTittle.Text = "Báo cáo tổng hợp POST";
            //        ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
            //        ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
            //        break;
            //    case "bc61":
            //        lblTitleReportType2.Text = "Báo cáo theo loại khiếu nại";
            //        tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
            //        break;
            //    case "bc71":
            //        lblTittle.Text = "Báo cáo chi tiết khiếu nại";
            //        break;
            //    case "bc81":
            //        lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau";
            //        ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
            //        ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
            //        break;
            //    default:
            //        lblTittle.Text = string.Empty;
            //        break;
            //} // end switch            

            //lblDoiTac.Visible = lblReportType.Text == "bc31";
            //cblDoiTac.Visible = lblReportType.Text == "bc31";
            //pnReportType1.Visible = lblReportType.Text == "bc11" || lblReportType.Text == "bc21" || lblReportType.Text == "bc41" || lblReportType.Text == "bc51" || lblReportType.Text == "bc71" || lblReportType.Text == "bc81";
            //pnReportType2.Visible = lblReportType.Text == "bc31" || lblReportType.Text == "bc61";

            //ddlDoiTac_ReportType1.Visible = lblReportType.Text == "bc81";
            //lblPhongBan_DoiTac_ReportType1.Text = lblReportType.Text == "bc81" ? "Đối tác" : "Phòng";
            //ddlKhuVuc_ReportType1.Visible = ddlPhongBan_ReportType1.Visible = lblReportType.Text != "bc81";

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNaiId + "');", true);
            ////ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/10/2013
        /// Todo : Lấy ra các phòng ban thuộc khu vực
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlKhuVuc_ReportType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlKhuVuc_ReportType1.SelectedValue == DoiTacInfo.DoiTacIdValue.VNP1.ToString() || ddlKhuVuc_ReportType1.SelectedValue == DoiTacInfo.DoiTacIdValue.VNP2.ToString() || ddlKhuVuc_ReportType1.SelectedValue == DoiTacInfo.DoiTacIdValue.VNP3.ToString())
            {
                LoadDoiTacVNPTTT();
                ddlDoiTac_ReportType1.Visible = true;
                ddlPhongBan_ReportType1.Visible = false;
                lblPhongBan_DoiTac_ReportType1.Text = "Đối tác";
            }
            else
            {
                ddlPhongBan_ReportType1.Items.Clear();
                int doiTacId = ConvertUtility.ToInt32(ddlKhuVuc_ReportType1.SelectedValue);
                List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);

                if (listPhongBan == null)
                {
                    listPhongBan = new List<PhongBanInfo>();
                }

                PhongBanInfo objPhongBan = new PhongBanInfo();
                objPhongBan.Id = -1;
                objPhongBan.Name = "Chọn phòng ban..";
                listPhongBan.Insert(0, objPhongBan);
                ddlPhongBan_ReportType1.DataSource = listPhongBan;
                ddlPhongBan_ReportType1.DataBind();

                if (!ddlPhongBan_ReportType1.Visible)
                {
                    ddlDoiTac_ReportType1.Visible = false;
                    ddlPhongBan_ReportType1.Visible = true;
                    lblPhongBan_DoiTac_ReportType1.Text = "Phòng";
                }
            }

            //if (ddlPhongBan_ReportType1.Visible)
            //{
            //    ddlPhongBan_ReportType1.Items.Clear();
            //    int doiTacId = ConvertUtility.ToInt32(ddlKhuVuc_ReportType1.SelectedValue);
            //    List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);

            //    if (listPhongBan == null)
            //    {
            //        listPhongBan = new List<PhongBanInfo>();
            //    }

            //    PhongBanInfo objPhongBan = new PhongBanInfo();
            //    objPhongBan.Id = -1;
            //    objPhongBan.Name = "Chọn phòng ban..";
            //    listPhongBan.Insert(0, objPhongBan);
            //    ddlPhongBan_ReportType1.DataSource = listPhongBan;
            //    ddlPhongBan_ReportType1.DataBind();
            //}
            //else if (ddlDoiTac_ReportType1.Visible)
            //{
            //    LoadDoiTacVNPTTT();
            //}

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Hiển thị các đối tác
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlKhuVuc_ReportType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cblDoiTac.Items.Clear();
            ddlPhongBan_ReportType2.Items.Clear();
            int doiTacId = ConvertUtility.ToInt32(ddlKhuVuc_ReportType2.SelectedValue);

            if (ConvertUtility.ToInt32(ddlKhuVuc_ReportType2.SelectedValue) > -1)
            {
                List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,DonViTrucThuoc", "Id=" + ddlKhuVuc_ReportType2.SelectedValue, "");
                if (listDoiTac != null && listDoiTac.Count > 0)
                {
                    lblKhuVucId_ReportType2.Text = listDoiTac[0].Id.ToString();
                    lblTenKhuVuc_ReportType2.Text = listDoiTac[0].TenDoiTac;
                    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,TenDoiTac", "DonViTrucThuoc=" + listDoiTac[0].DonViTrucThuoc.ToString(), "TenDoiTac ASC");
                    cblDoiTac.DataSource = listDoiTac;
                    cblDoiTac.DataBind();

                    for (int i = 0; i < cblDoiTac.Items.Count; i++)
                    {
                        cblDoiTac.Items[i].Selected = true;
                    }
                }                
                //ListItem item = new ListItem(ddlKhuVuc_ReportType2.SelectedItem.Text.Trim(), ddlKhuVuc_ReportType2.SelectedValue);
                //cblDoiTac.Items.Insert(0, item);                
            }

            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);
            if (listPhongBan == null)
            {
                listPhongBan = new List<PhongBanInfo>();
            }

            PhongBanInfo objPhongBan = new PhongBanInfo();
            objPhongBan.Id = -1;
            objPhongBan.Name = "Chọn phòng ban..";
            listPhongBan.Insert(0, objPhongBan);

            ddlPhongBan_ReportType2.DataSource = listPhongBan;
            ddlPhongBan_ReportType2.DataBind();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 22/10/2013
        /// Todo : Ẩn/hiện các điều kiện lọc báo cáo
        /// </summary>
        /// <param name="reportType"></param>
        private void ShowHideReportCondition(string reportType)
        {
            string tvLoaiKhieuNaiId = string.Empty;
            string CUOC_TB_TRA_TRUOC = "30";
            string CUOC_TB_TRA_SAU = "35";
            switch (reportType)
            {
                case "bc11":
                    lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả trước";
                    ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
                    ddlLoaiKhieuNai_SelectedIndexChanged(null, null);                    
                    break;
                case "bc21":
                    lblTittle.Text = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    break;
                case "bc31":
                    lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
                    break;
                case "bc41":
                    lblTittle.Text = "Báo cáo chi tiết PPS";
                    ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
                    ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc51":
                    lblTittle.Text = "Báo cáo chi tiết POST";
                    ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
                    ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc61":
                    lblTitleReportType2.Text = "Báo cáo theo loại khiếu nại";
                    tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
                    break;
                case "bc71":
                    lblTittle.Text = "Báo cáo chi tiết khiếu nại";
                    break;
                case "bc81":
                    lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau";
                    ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
                    ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    //string pathUc = "UC/ucDKTGiamTruCuocDVTraSau.ascx";
                    //Control uc = this.LoadControl(pathUc);
                    //pnlContainer.Controls.Add(uc);
                    break;          
                case "bc_XLNV_BaoCaoKhoiLuongCongViec":
                    lblTitleReportType2.Text = "Báo cáo khối lượng công việc";
                    break;
                case "bcBieuDo_TheoLinhVucCon":
                    lblTitleReportType4.Text = "Báo cáo dạng biểu đồ theo lĩnh vực con";
                    tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType4.ClientID;
                    break;
                case "bc_KS_BaoCaoTongHopKN":
                    lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_OB_BaoCaoTongHopKN":
                    lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                default:
                    lblTittle.Text = string.Empty;
                    break;
            } // end switch            

            lblDoiTac.Visible = lblReportType.Text == "bc31";
            cblDoiTac.Visible = lblReportType.Text == "bc31";
            pnReportType1.Visible = lblReportType.Text == "bc11" || lblReportType.Text == "bc21" || lblReportType.Text == "bc41"
                                    || lblReportType.Text == "bc51" || lblReportType.Text == "bc71" || lblReportType.Text == "bc81";
            pnReportType2.Visible = lblReportType.Text == "bc31" || lblReportType.Text == "bc61" || lblReportType.Text == "bc_XLNV_BaoCaoKhoiLuongCongViec"
                                    || lblReportType.Text == "bc_KS_BaoCaoTongHopKN" || lblReportType.Text == "bc_OB_BaoCaoTongHopKN";           
            pnReportType4.Visible = lblReportType.Text == "bcBieuDo_TheoLinhVucCon";

            ddlDoiTac_ReportType1.Visible = lblReportType.Text == "bc81";
            lblPhongBan_DoiTac_ReportType1.Text = lblReportType.Text == "bc81" ? "Đối tác" : "Phòng";
            ddlPhongBan_ReportType1.Visible = lblReportType.Text != "bc81";

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNaiId + "');", true);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 22/10/2013
        /// Todo : Ẩn/hiện các điều kiện lọc báo cáo
        /// </summary>
        /// <param name="reportType"></param>
        private void ShowReportUserControl(string reportType)
        {
            if (reportType != null && reportType.Trim().Length == 0) return;

            LastLoadedControl = reportType;
            string pathUc = string.Empty;
            Control uc = null;
            pnlContainer.Controls.Clear();
            switch (reportType)
            {
                case "bc_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV":
                    pathUc = "UC/ucBaoCaoToTruongToKTV.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucBaoCaoToTruongToKTV)uc).ReportType = "bc_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV";
                    ((ucBaoCaoToTruongToKTV)uc).PhongBanXuLyId = LoginAdmin.AdminLogin().PhongBanId;
                    ((ucBaoCaoToTruongToKTV)uc).ReportTitle = "Báo cáo số lượng khiếu nại phản hồi về khai thác viên";
                    pnlContainer.Controls.Add(uc);
                    break;
                case "bc_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV":
                    pathUc = "UC/ucBaoCaoToTruongToKTV.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucBaoCaoToTruongToKTV)uc).ReportType = "bc_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV";
                    ((ucBaoCaoToTruongToKTV)uc).PhongBanXuLyId = LoginAdmin.AdminLogin().PhongBanId;
                    ((ucBaoCaoToTruongToKTV)uc).ReportTitle = "Báo cáo số lượng khiếu nại quá hạn hoặc tồn đọng của khai thác viên";
                    pnlContainer.Controls.Add(uc);
                    break;                
                default:
                    lblTittle.Text = string.Empty;
                    break;
            } // end switch            

            pnReportType1.Visible =
            pnReportType2.Visible =
            pnReportType4.Visible = false;            

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
        }

        private void LoadComboPhongBan()
        {
            //var lstPhongBanReport1 = ServiceFactory.GetInstancePhongBan().GetList();
            //ddlPhongBan_ReportType1.DataSource = lstPhongBanReport1;
            //ddlPhongBan_ReportType1.DataBind();
            //ListItem item = new ListItem("Chọn phòng ban", "-1");
            //ddlPhongBan_ReportType1.Items.Insert(0, item);

            //var lstPhongBanReport2 = ServiceFactory.GetInstancePhongBan().GetList();
            //ddlKhuVuc_ReportType2.DataSource = lstPhongBanReport2;
            //ddlKhuVuc_ReportType2.DataBind();
            //item = new ListItem("Chọn phòng ban", "-1");
            //ddlKhuVuc_ReportType2.Items.Insert(0, item);
        }

        private void LoadTreeLoaiKhieuNai()
        {
            List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name,ParentId", "", "Sort");
            if (listLoaiKhieuNai != null && listLoaiKhieuNai.Count > 0)
            {
                for (int i = 0; i < listLoaiKhieuNai.Count; i++)
                {
                    if (listLoaiKhieuNai[i].ParentId == 0)
                    {
                        TreeNode node = new TreeNode();
                        tvLoaiKhieuNai_ReportType2.Nodes.Add(node);
                        AddNode(listLoaiKhieuNai, listLoaiKhieuNai[i], node);
                        node.Collapse();
                        i--;
                    }
                }

                CopyTreeView(tvLoaiKhieuNai_ReportType2, tvLoaiKhieuNai_ReportType4);
            }
        }

        private void AddNode(List<LoaiKhieuNaiInfo> listLoaiKhieuNai, LoaiKhieuNaiInfo curLoaiKhieuNai, TreeNode node)
        {
            node.Text = curLoaiKhieuNai.Name;
            node.SelectAction = TreeNodeSelectAction.None;
            if (node.Parent != null)
            {
                //node.Value = string.Format("{0}_{1}", node.Parent.Value, curLoaiKhieuNai.Id);                
                node.Value = curLoaiKhieuNai.Id.ToString();
            }
            else
            {
                node.Value = curLoaiKhieuNai.Id.ToString();
            }

            int parentId = curLoaiKhieuNai.Id;
            listLoaiKhieuNai.Remove(curLoaiKhieuNai);

            for (int i = 0; i < listLoaiKhieuNai.Count; i++)
            {
                if (listLoaiKhieuNai[i].ParentId == parentId)
                {
                    TreeNode childNode = new TreeNode();
                    node.ChildNodes.Add(childNode);
                    AddNode(listLoaiKhieuNai, listLoaiKhieuNai[i], childNode);
                    i--;
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/10/2013
        /// Todo : Thực hiện copy treeview
        /// </summary>
        /// <param name="treeview1"></param>
        /// <param name="treeview2"></param>
        public void CopyTreeView(TreeView tvSource, TreeView tvDestination)
        {
            TreeNode newTn;
            foreach (TreeNode tn in tvSource.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.Value);
                CopyChilds(newTn, tn);
                tvDestination.Nodes.Add(newTn);
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date 30/10/2013
        /// Todo : Thực hiện copy từng node con của treeview
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="willCopied"></param>
        public void CopyChilds(TreeNode parent, TreeNode willCopied)
        {
            TreeNode newTn;
            foreach (TreeNode tn in willCopied.ChildNodes)
            {
                newTn = new TreeNode(tn.Text, tn.Value);
                CopyChilds(newTn, tn);
                parent.ChildNodes.Add(newTn);
            }
        }

        //private void LoadDoiTacByPhongBan(int phongBanId)
        //{
        //    List<DoiTacInfo> listDoiTacInfo = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,TenDoiTac", "DonViTrucThuoc=" + phongBanId, "TenDoiTac");
        //    cblDoiTac.DataSource = listDoiTacInfo;
        //    cblDoiTac.DataBind();
        //}

        private void GetListLinhVucChung(TreeNode parentNode, ref string listLinhVucChungId, ref string listLinhVucConId)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Checked)
                {
                    listLinhVucChungId = string.Format("{0}{1},", listLinhVucChungId, node.Value);
                }

                GetListLinhVucCon(node, ref listLinhVucConId);
            }
        }

        private void GetListLinhVucCon(TreeNode parentNode, ref string listLinhVucConId)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Checked)
                {
                    listLinhVucConId = string.Format("{0}{1},", listLinhVucConId, node.Value);
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Load combo đối tác
        /// </summary>
        private void LoadKhuVuc()
        {
            ddlKhuVuc_ReportType2.Items.Clear();

            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
            string space = string.Empty;
            if (listDoiTac != null)
            {
                List<DoiTacInfo> listDoiTacRoot = listDoiTac.FindAll(delegate(DoiTacInfo obj) { return obj.DonViTrucThuoc == 0; });

                for (int i = 0; i < listDoiTacRoot.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", space, listDoiTacRoot[i].TenDoiTac);
                    item.Value = listDoiTacRoot[i].Id.ToString();
                    ddlKhuVuc_ReportType2.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacRoot[i].Id;
                    listDoiTacRoot.RemoveAt(i);
                    i--;

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTac);
                }
            }

            for (int i = 0; i < ddlKhuVuc_ReportType2.Items.Count; i++)
            {
                ListItem item = new ListItem();
                item.Value = ddlKhuVuc_ReportType2.Items[i].Value;
                item.Text = ddlKhuVuc_ReportType2.Items[i].Text;

                ddlKhuVuc_ReportType1.Items.Add(item);               
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Lấy ra các đối tác của đơn vị trực thuộc
        /// </summary>
        /// <param name="donViTrucThuoc"></param>
        /// <param name="listDoiTacInfo"></param>
        private void LoadChildDoiTac(string space, int donViTrucThuoc, List<DoiTacInfo> listDoiTacInfo)
        {
            space = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}", space);
            for (int i = 0; i < listDoiTacInfo.Count; i++)
            {
                if (listDoiTacInfo[i].DonViTrucThuoc == donViTrucThuoc)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", Server.HtmlDecode(space), listDoiTacInfo[i].TenDoiTac);
                    item.Value = listDoiTacInfo[i].Id.ToString(); ;
                    ddlKhuVuc_ReportType2.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacInfo[i].Id;

                    listDoiTacInfo.RemoveAt(i);
                    i = -1;

                    if (listDoiTacInfo.Count == 0)
                    {
                        break;
                    }

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTacInfo);
                }
            }

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/10/2013
        /// Todo : 
        /// </summary>
        private void LoadDoiTacVNPTTT()
        {
            string khuVucId = ddlKhuVuc_ReportType1.SelectedValue;

            string whereDonViTrucThuoc = string.Empty;
            if (khuVucId != "-1")
            {
                whereDonViTrucThuoc = " DonViTrucThuoc=" + khuVucId;
            }
            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", whereDonViTrucThuoc, "TenDoiTac ASC");
            if (listDoiTac == null)
            {
                listDoiTac = new List<DoiTacInfo>();
            }

            DoiTacInfo objDoiTac = new DoiTacInfo();
            objDoiTac.Id = -1;
            objDoiTac.TenDoiTac = "Chọn VNPT TT..";
            listDoiTac.Insert(0, objDoiTac);

            ddlDoiTac_ReportType1.DataSource = listDoiTac;
            ddlDoiTac_ReportType1.DataBind();
        }

        #endregion            

        #region ReportType 4

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/10/2013
        /// Todo : Hiển thị màn hình báo cáo dạng biểu đồ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_ReportType4_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string reportType = rblReportType_ReportType4.SelectedValue;
            string loaiKhieuNaiId = string.Empty;
            string linhVucChungId = string.Empty;
            string linhVucConId = string.Empty;
            string loaibc = rblLoaiBaoCao_ReportType4.SelectedValue;
            string fromDate1 = txtFromDate1_ReportType4.Text;
            string toDate1 = txtToDate1_ReportType4.Text;
            string fromDate2 = txtFromDate2_ReportType4.Text;
            string toDate2 = txtToDate2_ReportType4.Text;

            foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai_ReportType4.Nodes)
            {
                if (nodeLoaiKhieuNai.Checked)
                {
                    loaiKhieuNaiId = string.Format("{0}{1},", loaiKhieuNaiId, nodeLoaiKhieuNai.Value);
                }

                GetListLinhVucChung(nodeLoaiKhieuNai, ref linhVucChungId, ref linhVucConId);
            }

            if (loaiKhieuNaiId.Length > 0)
            {
                loaiKhieuNaiId = loaiKhieuNaiId.TrimEnd(',');
            }

            if (linhVucChungId.Length > 0)
            {
                linhVucChungId = linhVucChungId.TrimEnd(',');
            }

            if (linhVucConId.Length > 0)
            {
                linhVucConId = linhVucConId.TrimEnd(',');
            }

            string sDate = string.Format("{0}-{1},{2}-{3}", fromDate1, toDate1, fromDate2, toDate2);

            string errorMessage = string.Empty;
            string script = string.Empty;

            if (fromDate1.Length == 0 || toDate1.Length == 0 || fromDate2.Length == 0 || toDate2.Length == 0)
            {
                errorMessage = string.Format("{0}\\nBạn phải nhập ngày báo cáo", errorMessage);
            }
            else
            {
                DateTime dateCheck = ConvertUtility.ToDateTime(fromDate1, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nTừ ngày 1 không hợp lệ", errorMessage);
                }

                dateCheck = ConvertUtility.ToDateTime(toDate1, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nĐến ngày 1 không hợp lệ", errorMessage);
                }

                dateCheck = ConvertUtility.ToDateTime(fromDate2, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nTừ ngày 2 không hợp lệ", errorMessage);
                }

                dateCheck = ConvertUtility.ToDateTime(toDate2, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nĐến ngày 2 không hợp lệ", errorMessage);
                }
            }

            if (loaiKhieuNaiId.Length == 0 && linhVucChungId.Length == 0 && linhVucConId.Length == 0)
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn loại khiếu nại", errorMessage);
            }


            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                string url = string.Format("/Views/BaoCao/Popup/baocaobieudosoluongkhieunaitiepnhan.aspx?reportType={0}&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&listDate={4}&loaibc={5}", reportType, loaiKhieuNaiId, linhVucChungId, linhVucConId, sDate, loaibc);
                if (url.Length > 1900)
                {
                    errorMessage = "Bạn chọn quá nhiều Loại khiếu nại/Lĩnh vực chung/Lĩnh vực chọn. Hãy bỏ bớt đi";
                    script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
                }
                else
                {
                    script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo biểu đồ số lượng khiếu nại tiếp nhận', '<iframe style=\"border:none\" width=\"980\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaobieudosoluongkhieunaitiepnhan.aspx?reportType={0}&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&listDate={4}&loaibc={5}\">');</script>", reportType, loaiKhieuNaiId, linhVucChungId, linhVucConId, sDate, loaibc);
                }

            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType4.ClientID + "');", true);
        }

        #endregion

 
    }
}