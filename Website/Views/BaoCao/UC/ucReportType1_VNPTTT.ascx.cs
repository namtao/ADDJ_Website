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
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 10/12/2013
    /// </summary>
    public partial class ucReportType1_VNPTTT : System.Web.UI.UserControl
    {
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

            if(this.IsFirstLoad)
            {
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                string CUOC_TB_TRA_TRUOC = "30";
                string CUOC_TB_TRA_SAU = "35";

                LoadKhuVuc();               
                //LoadDoiTacVNPTTT();

                DoiTacInfo objDoiTac = ServiceFactory.GetInstanceDoiTac().GetInfo(this.DoiTacId);
                if (objDoiTac == null) return;

                ddlKhuVuc_ReportType1.SelectedValue = objDoiTac.DonViTrucThuoc.ToString();
                ddlKhuVuc_ReportType1.Enabled = false;

                LoadComboKhieuNai();

                switch(this.ReportType)
                {                   
                    case "bc_VNPTTT_ChiTietGiamTruTraSauVNPTTT":
                        ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
                        ddlLoaiKhieuNai.Enabled = false;
                        ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                                              
                        ddlDoiTac_ReportType1.Enabled = false;                        

                        LoadDoiTacVNPTTT();
                        ddlDoiTac_ReportType1.SelectedValue = this.DoiTacId.ToString();

                   

                        break;
                    case "bc_VNPTTT_TongHopPAKNTheoNguoiDung":

                        break;
                    default:

                        break;
                } // end switch(this.ReportType)                
            }

        }

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
            if (lblReportType.Text == "bc_VNPTTT_ChiTietGiamTruTraSauVNPTTT")
            {
                page = "baocaochitietgiamtrutrasau.aspx?";
                title = "Báo cáo chi tiết giảm trừ trả sau của VNPT TT";
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
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}doitacId={2}&tenDoiTac={3}&fromDate={4}&toDate={5}&loaiKhieuNaiID={6}&loaiKhieuNai={7}&linhVucChungID={8}&linhVucChung={9}&linhVucConID={10}&linhVucCon={11}&loaibc={12}&khuVuc={13}&khuVucId={14}&donViId={15}&donVi={16}\">');</script>", title, page, doiTacId, tenDoiTac, fromDate, toDate, loaiKhieuNaiId, loaiKhieuNai, linhVucChungId, linhVucChung, linhVucConId, linhVucCon, loaibc, khuVuc, khuVucId, donViId, donVi);
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doitacId={2}&tenDoiTac={3}&fromDate={4}&toDate={5}&loaiKhieuNaiID={6}&loaiKhieuNai={7}&linhVucChungID={8}&linhVucChung={9}&linhVucConID={10}&linhVucCon={11}&loaibc={12}&khuVuc={13}&khuVucId={14}&donViId={15}&donVi={16}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, doiTacId, tenDoiTac, fromDate, toDate, loaiKhieuNaiId, loaiKhieuNai, linhVucChungId, linhVucChung, linhVucConId, linhVucCon, loaibc, khuVuc, khuVucId, donViId, donVi);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
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

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/12/2013
        /// Todo : Trường hợp nếu là VNPT  khu vực thì hiển thị các VNPTTT
        ///         Trường còn lại thì hiển thị các phòng ban thuộc trung tâm được chọn
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
        }       

        #endregion       

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Load combo đối tác
        /// </summary>
        private void LoadKhuVuc()
        {
            ddlKhuVuc_ReportType1.Items.Clear();

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
                    ddlKhuVuc_ReportType1.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacRoot[i].Id;
                    listDoiTacRoot.RemoveAt(i);
                    i--;

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTac);
                }
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
                    ddlKhuVuc_ReportType1.Items.Add(item);

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

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/12/2013
        /// </summary>
        /// <param name="doiTacId"></param>
        private void LoadComboPhongBan(int doiTacId)
        {
            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);
            if (listPhongBan != null)
            {
                ddlPhongBan_ReportType1.DataSource = listPhongBan;
                ddlPhongBan_ReportType1.DataBind();
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/12/2013
        /// Todo : Lấy dữ liệu loại khiếu nại vào combobox
        /// </summary>
        private void LoadComboKhieuNai()
        {
            List<LoaiKhieuNaiInfo> listKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort ASC");
            if (listKhieuNai != null)
            {
                ddlLoaiKhieuNai.DataSource = listKhieuNai;
                ddlLoaiKhieuNai.DataBind();

                ListItem item = new ListItem("Chọn loại khiếu nại...", "-1");
                ddlLoaiKhieuNai.Items.Insert(0, item);
            }
        }

        #endregion
    
    }
}