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
    public partial class uc_ReportType8_GQKN_VNP : System.Web.UI.UserControl
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
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai.ClientID + "');", true);

            if (IsFirstLoad)
            {
                //lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                if (lblReportType.Text == "bc_VNP_BaoCaoTongHopToGQKN" )
                {
                    lblTitle.Text = "Báo cáo tổng hợp tổ GQKN";
                }
                else if (lblReportType.Text == "bc_VNP_BaoCaoGiamTruToGQKN")
                {
                    lblTitle.Text = "Báo cáo giảm trừ tổ GQKN";
                }

                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();                                                

                LoadTreeLoaiKhieuNai();               
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);
            
            string phongBanId = ddlPhongBan.SelectedValue;
            string phongBan = ddlPhongBan.SelectedItem != null ? ddlPhongBan.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string doiTac = string.Empty;
            string loaiKhieuNaiId = string.Empty;
            string linhVucChungId = string.Empty;
            string linhVucConId = string.Empty;            

            foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai.Nodes)
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

            Session["LoaiKhieuNai"] = loaiKhieuNaiId;
            Session["LinhVucChung"] = linhVucChungId;
            Session["LinhVucCon"] = linhVucConId;

            string errorMessage = string.Empty;
            string script = string.Empty;

            if (lblReportType.Text == "bc31")
            {
                if (ConvertUtility.ToInt32(phongBanId) <= 0)
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
                if (lblReportType.Text == "bc_VNP_BaoCaoTongHopToGQKN" || lblReportType.Text == "bc_VNP_BaoCaoGiamTruToGQKN")
                {
                    script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaotogqkn.aspx?phongBanId={0}&donVi={1}&fromDate={2}&toDate={3}&loaibc={4}&reportType={5}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", phongBanId, phongBan, fromDate, toDate, loaibc, lblReportType.Text);
                }                
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai.ClientID + "');", true);
        }       

        #endregion

        #region Private methods             

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
                        tvLoaiKhieuNai.Nodes.Add(node);
                        AddNode(listLoaiKhieuNai, listLoaiKhieuNai[i], node);
                        node.Collapse();
                        i--;
                    }
                }
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
    }
}