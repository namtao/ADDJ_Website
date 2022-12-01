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

namespace Website.Views.BaoCao
{
    public partial class BaoCaoThongKeTTTC : System.Web.UI.Page
    {
        //protected string optLoaiKhieuNai = "";
        //protected string optPhongBan = "";
        //protected string optLinhVucCon_CTGT = "";

        //protected string optDoitac_donvi = "";
        //protected string optDoitac_dichvu = "";
        //protected string optDoitac_lichhen = "";
        //protected string optDoitac_doanhthu = "";
        //protected string optDichvu = "";
        //protected string optDichvu_doanhthu = "";

        //protected string optDoitac_TraCuuThongTin = "";
        //protected string optDichvu_TraCuuThongTin = "";
        //protected string optKetQua = string.Empty;
        //protected string optHaiLong = string.Empty;

        //protected string strMaNguoiDung = string.Empty;
        //protected string strLoaiBaoCao = string.Empty;

        private readonly int TTTC = 10100;

        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserRead)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            //optLoaiKhieuNai = BuildBaoCao.GetLoaiKhieuNai();
            //optPhongBan = BuildBaoCao.GetPhongBan();

            if (!IsPostBack)
            {
                var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
                ddlLoaiKhieuNai.DataSource = lstLoaiKN;
                ddlLoaiKhieuNai.DataBind();

                ListItem item = new ListItem("Chọn loại khiếu nại..", "-1");
                ddlLoaiKhieuNai.Items.Insert(0, item);

                LoadPhongBan(TTTC);
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
            ddlReportType.SelectedValue = lb.CommandArgument;
            ShowHideReportCondition(lb.CommandArgument);          
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/09/2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideReportCondition(ddlReportType.SelectedValue);
            //string tvLoaiKhieuNaiId = string.Empty;
            //string CUOC_TB_TRA_TRUOC = "30";
            //string CUOC_TB_TRA_SAU = "35";
            //switch (ddlReportType.SelectedValue)
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

            //lblDoiTac.Visible = ddlReportType.SelectedValue == "bc31";
            //cblDoiTac.Visible = ddlReportType.SelectedValue == "bc31";
            //pnReportType1.Visible = ddlReportType.SelectedValue == "bc11" || ddlReportType.SelectedValue == "bc21" || ddlReportType.SelectedValue == "bc41" || ddlReportType.SelectedValue == "bc51" || ddlReportType.SelectedValue == "bc71" || ddlReportType.SelectedValue == "bc81";
            //pnReportType2.Visible = ddlReportType.SelectedValue == "bc31" || ddlReportType.SelectedValue == "bc61";

            //ddlDoiTac_ReportType1.Visible = ddlReportType.SelectedValue == "bc81";
            //lblPhongBan_DoiTac_ReportType1.Text = ddlReportType.SelectedValue == "bc81" ? "Đối tác" : "Phòng";
            //ddlKhuVuc_ReportType1.Visible = ddlPhongBan_ReportType1.Visible = ddlReportType.SelectedValue != "bc81";

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNaiId + "');", true);
            ////ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
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
            switch (reportType)
            {                
                case "bcTTTC_TongHopPAKN":
                    lblTittleReportType1.Text = "Báo cáo tổng hợp PAKN";
                    break;
                case "bcTTTC_TongHopPAKNTheoPhongBan":
                    lblTittleReportType1.Text = "Báo cáo tổng hợp PAKN theo phòng ban";
                    break;
                case "bcTTTC_TongHopPAKNTheoNguoiDung":
                    lblTittleReportType1.Text = "Báo cáo tổng hợp PAKN theo người dùng";
                    break;
                case "bcTTTC_ChiTietPAKNTheoNguoiDung":
                    lblTitleReportType3.Text = "Báo cáo chi tiết PAKN theo người dùng";
                    break;               
                default:
                    lblTitleReportType3.Text = string.Empty;
                    break;
            } // end switch                       

            pnReportType1.Visible = reportType == "bcTTTC_TongHopPAKN" || reportType == "bcTTTC_TongHopPAKNTheoPhongBan" || reportType == "bcTTTC_TongHopPAKNTheoNguoiDung";
            pnReportType3.Visible = reportType == "bcTTTC_ChiTietPAKNTheoNguoiDung";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNaiId + "');", true);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
        }             

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

        #endregion

        #region ReportType 1

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

            string reportType = ddlReportType.SelectedValue;
            string title = string.Empty;                    
            string donViId = ddlPhongBan_ReportType1.SelectedValue;           
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;            

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
            if (reportType == "bcTTTC_TongHopPAKN")
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
                //if (ddlReportType.SelectedValue == "bcTTTC_TongHopPAKN" || ddlReportType.SelectedValue == "bcTTTC_TongHopPAKNTheoPhongBan"
                //    || ddlReportType.SelectedValue == "bcTTTC_TongHopPAKNTheoNguoiDung")
                //{
                //    script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}?khuVucId={2}&fromDate={3}&toDate={4}&loaibc={5}&phongBanId={6}\">');</script>", title, page, khuVucId, fromDate, toDate, loaibc, donViId);
                //} 

                script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}?khuVucId={2}&fromDate={3}&toDate={4}&loaibc={5}&phongBanId={6}\">');</script>", title, page, TTTC, fromDate, toDate, loaibc, donViId);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
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

        #endregion

        #region ReportType 3

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/11/2013
        /// Todo : Lấy ra danh sách phòng ban của TTTC
        /// </summary>
        /// <param name="idTTTC"></param>
        private void LoadPhongBan(int idTTTC)
        {
            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(idTTTC);
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

            if (listPhongBan != null)
            {
                for (int i = 0; i < listPhongBan.Count; i++)
                {
                    ListItem item = new ListItem(listPhongBan[i].Name, listPhongBan[i].Id.ToString());
                    ddlPhongBan_ReportType3.Items.Add(item);
                }
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }       

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 28/10/2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPhongBan_ReportType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cblNguoiDung.Items.Clear();
            int phongBanId = ConvertUtility.ToInt32(ddlPhongBan_ReportType3.SelectedValue, -1);
            List<NguoiSuDungInfo> listNguoiSuDung = ServiceFactory.GetInstanceNguoiSuDung().GetListNguoiSuDungByPhongBanId(phongBanId);
            if (listNguoiSuDung != null)
            {
                for (int i = 0; i < listNguoiSuDung.Count; i++)
                {
                    ListItem item = new ListItem(listNguoiSuDung[i].TenDayDu, listNguoiSuDung[i].TenTruyCap);
                    cblNguoiDung.Items.Add(item);
                }
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 28/10/2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReport_ReportType3_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            //string khuVucId = ddlKhuVuc_ReportType2.SelectedValue;
            //string khuVuc = ddlKhuVuc_ReportType2.SelectedItem != null ? ddlKhuVuc_ReportType2.SelectedItem.Text : string.Empty;
            //string donViId = ddlPhongBan_ReportType2.SelectedValue;
            //string donVi = ddlPhongBan_ReportType2.SelectedItem != null ? ddlPhongBan_ReportType2.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate_ReportType3.Text;
            string toDate = txtToDate_ReportType3.Text;
            string loaibc = rblLoaiBaoCao_ReportType3.SelectedItem.Value;

            //string loaiKhieuNaiId = string.Empty;
            //string linhVucChungId = string.Empty;
            //string linhVucConId = string.Empty;

            string tenTruyCap = string.Empty;

            foreach (ListItem item in cblNguoiDung.Items)
            {
                if (item.Selected)
                {
                    tenTruyCap = string.Format("{0}{1},", tenTruyCap, item.Value);
                }
            }

            if (tenTruyCap.Length > 0)
            {
                tenTruyCap = tenTruyCap.TrimEnd(',');
            }

            //foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai_ReportType2.Nodes)
            //{
            //    if (nodeLoaiKhieuNai.Checked)
            //    {
            //        loaiKhieuNaiId = string.Format("{0}{1},", loaiKhieuNaiId, nodeLoaiKhieuNai.Value);
            //    }

            //    GetListLinhVucChung(nodeLoaiKhieuNai, ref linhVucChungId, ref linhVucConId);
            //}

            //if (loaiKhieuNaiId.Length > 0)
            //{
            //    loaiKhieuNaiId = loaiKhieuNaiId.TrimEnd(',');
            //}

            //if (linhVucChungId.Length > 0)
            //{
            //    linhVucChungId = linhVucChungId.TrimEnd(',');
            //}

            //if (linhVucConId.Length > 0)
            //{
            //    linhVucConId = linhVucConId.TrimEnd(',');
            //}

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

            if (tenTruyCap.Length == 0)
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn người dùng", errorMessage);
            }


            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo chi tiết PAKN theo người dùng', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaochitietpakntheonguoidungtttc.aspx?tenTruyCap={0}&fromDate={1}&toDate={2}&loaibc={3}\">');</script>", tenTruyCap, fromDate, toDate, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        #endregion    

    }
}