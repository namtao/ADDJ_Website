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
    public partial class ucReportType1_TTTC : System.Web.UI.UserControl
    {
        private readonly int TTTC = 10100;
        private readonly int PHONG_LANH_DAO_TTTC = 101;

        public string ReportType { get; set; }

        public int DoiTacId { get; set; }

        public int PhongBanXuLyId { get; set; }

        public string ReportTitle { get; set; }

        // Biến IsFirstLoad : để xác định usercontrol có phải là load lần đầu tiên không (biến này được truyền vào từ page khác)
        // vì IsPostBack ăn theo Page nên không thể xác định được đối với user control       
        public bool IsFirstLoad { get; set; }

        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);   

            if (IsFirstLoad)
            {
                AdminInfo adminInfo = LoginAdmin.AdminLogin();
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                List<LoaiKhieuNai_NhomInfo> listLoaiKhieuNaiNhomInfo = ServiceFactory.GetInstanceLoaiKhieuNaiNhom().GetListDynamic("*", "", "TenNhom ASC");
                ddlLoaiKhieuNai_Nhom.DataSource = listLoaiKhieuNaiNhomInfo;
                ddlLoaiKhieuNai_Nhom.DataBind();

                //var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
                //ddlLoaiKhieuNai.DataSource = lstLoaiKN;
                //ddlLoaiKhieuNai.DataBind();

                ListItem item = new ListItem("Chọn nhóm loại khiếu nại..", "-1");
                ddlLoaiKhieuNai_Nhom.Items.Insert(0, item);

                if (ReportType == "bc_VNPTTT_TongHopPAKNTheoNguoiDung")
                {
                    LoadPhongBan(adminInfo.DoiTacId);

                    ddlPhongBan_ReportType1.Enabled = false;
                }
                else
                {
                    LoadPhongBan(TTTC);

                    ddlPhongBan_ReportType1.Enabled = false;
                }
                
                

                switch(this.ReportType)
                {
                    case "bc_TTTC_TongHopPAKN":

                        break;
                    case "bc_TTTC_TongHopPAKNTheoPhongBan":                        
                        //if(adminInfo != null)
                        //{
                        //    ddlPhongBan_ReportType1.Enabled = adminInfo.PhongBanId == PHONG_LANH_DAO_TTTC;
                        //    if (!ddlPhongBan_ReportType1.Enabled)
                        //    {
                        //        ddlPhongBan_ReportType1.SelectedValue = adminInfo.PhongBanId.ToString();
                        //    }
                            
                        //}
                        //break;
                    case "bc_TTTC_TongHopPAKNTheoNguoiDung":                        
                        //if(adminInfo != null)
                        //{
                        //    ddlPhongBan_ReportType1.Enabled = adminInfo.PhongBanId == PHONG_LANH_DAO_TTTC;
                        //    if (!ddlPhongBan_ReportType1.Enabled)
                        //    {
                        //        ddlPhongBan_ReportType1.SelectedValue = adminInfo.PhongBanId.ToString();
                        //    }
                            
                        //}
                        //break;
                    case "bc_TTTC_BaoCaoPhoiHopGQKN":
                    case "bc_TTTC_BaoCaoQuaHanPhongBan":
                        if(adminInfo != null)
                        {
                            ddlPhongBan_ReportType1.Enabled = adminInfo.PhongBanId == PHONG_LANH_DAO_TTTC;
                            if (!ddlPhongBan_ReportType1.Enabled)
                            {
                                ddlPhongBan_ReportType1.SelectedValue = adminInfo.PhongBanId.ToString();
                            }
                                                       
                        }
                        break;
                    case "bc_VNPTTT_TongHopPAKNTheoNguoiDung":
                        if (adminInfo != null)
                        {
                            ddlPhongBan_ReportType1.Enabled = true;
                            if (!ddlPhongBan_ReportType1.Enabled)
                            {
                                ddlPhongBan_ReportType1.SelectedValue = adminInfo.PhongBanId.ToString();
                            }

                        }
                        break;
                    default:
                        break;
                }

                txtFromDate.Enabled = this.ReportType != "bc_TTTC_BaoCaoQuaHanPhongBan";
                txtToDate.Enabled = this.ReportType != "bc_TTTC_BaoCaoQuaHanPhongBan";
                //ddlLoaiKhieuNai.Enabled = ddlLinhVucChung.Enabled = ddlLinhVucCon.Enabled = this.ReportType != "bc_TTTC_BaoCaoQuaHanPhongBan";
            }
        }                     
       
        #endregion

        #region Private methods        

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

            //if (listPhongBan != null)
            //{
            //    for (int i = 0; i < listPhongBan.Count; i++)
            //    {
            //        ListItem item = new ListItem(listPhongBan[i].Name, listPhongBan[i].Id.ToString());
            //        ddlPhongBan_ReportType3.Items.Add(item);
            //    }
            //}

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
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
            //Session["IsFirstLoadReport"] = false;

            DateTime nullDateTime = new DateTime(1900, 01, 01);
                                 
            string donViId = ddlPhongBan_ReportType1.SelectedValue;           
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;

            string loaiKhieuNai_NhomId = ddlLoaiKhieuNai_Nhom.SelectedValue;
            string loaiKhieuNaiId = ddlLoaiKhieuNai.SelectedValue;
            string linhVucChungId = ddlLinhVucChung.SelectedValue;
            string linhVucConId = ddlLinhVucCon.SelectedValue;

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
            if (lblReportType.Text == "bc_TTTC_TongHopPAKN")
            {
                page = "baocaotonghoppakntttc.aspx?";               
            }
            else if (lblReportType.Text == "bc_TTTC_TongHopPAKNTheoPhongBan")
            {
                page = "baocaotonghoppakntheophongbantttc.aspx?";                
            }
            else if (lblReportType.Text == "bc_TTTC_BaoCaoQuaHanPhongBan")
            {
                page = "baocaotonghoppakntheophongbantttc.aspx?reportType=bc_TTTC_BaoCaoQuaHanPhongBan&";
            }
            else if (lblReportType.Text == "bc_TTTC_TongHopPAKNTheoNguoiDung")
            {
                page = "baocaotonghoppakntheonguoidungtttc.aspx?";                

                if (donViId == "-1")
                {
                    errorMessage = string.Format("{0}\\nBạn phải chọn phòng ban", errorMessage);
                }
            }
            else if (lblReportType.Text == "bc_TTTC_BaoCaoPhoiHopGQKN")
            {
                if (donViId == "-1")
                {
                    errorMessage = string.Format("{0}\\nBạn phải chọn phòng ban", errorMessage);
                }

                page = "baocaophoihopgqkntttc.aspx?";
            }
            else if (lblReportType.Text == "bc_VNPTTT_TongHopPAKNTheoNguoiDung")
            {
                page = "baocaotonghoppakntheonguoidungvnpttt.aspx?";

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
                if (lblReportType.Text == "bc_VNPTTT_TongHopPAKNTheoNguoiDung")
                {
                    script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doitacId={2}&fromDate={3}&toDate={4}&loaibc={5}&phongBanId={6}&loaiKhieuNai_NhomId={7}&loaiKhieuNaiId={8}&linhVucChungId={9}&linhVucConId={10}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, LoginAdmin.AdminLogin().DoiTacId, fromDate, toDate, loaibc, donViId, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                }
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}khuVucId={2}&fromDate={3}&toDate={4}&loaibc={5}&phongBanId={6}\">');</script>", lblTittle.Text, page, TTTC, fromDate, toDate, loaibc, donViId);
                else
                    script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}khuVucId={2}&fromDate={3}&toDate={4}&loaibc={5}&phongBanId={6}&loaiKhieuNai_NhomId={7}&loaiKhieuNaiId={8}&linhVucChungId={9}&linhVucConId={10}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, TTTC, fromDate, toDate, loaibc, donViId, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 17/04/2015
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLoaiKhieuNai_Nhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoaiKhieuNai_Nhom.SelectedItem != null && ddlLoaiKhieuNai_Nhom.SelectedValue.Length > 0)
            {
                List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId = 0 AND LoaiKhieuNai_NhomId =" + ddlLoaiKhieuNai_Nhom.SelectedValue, "Name ASC");
                ddlLoaiKhieuNai.DataSource = listLoaiKhieuNai;
                ddlLoaiKhieuNai.DataBind();

                ListItem item = new ListItem("Chọn loại khiếu nại..", "-1");
                ddlLoaiKhieuNai.Items.Insert(0, item);

                ddlLinhVucChung.Items.Clear();
                ddlLinhVucCon.Items.Clear();

                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);                
            }
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

                ddlLinhVucCon.Items.Clear();
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);                
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

                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);                
            }
        }       

        #endregion        

    }
}