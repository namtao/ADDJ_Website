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
    public partial class ucReportType2_VAS : System.Web.UI.UserControl
    {
        private readonly int PHONG_PTDV = 52;

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
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();
             
                LoadTreeLoaiKhieuNai();
            }           
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

            string khuVucId = ddlKhuVuc.SelectedValue;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;

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
                    case "bc_VAS_SoLieuPAKNDichVuToanMang":                        
                        page = "baocaosolieupakntoanmangptdv.aspx";
                        break;                    
                    default:
                        page = "";
                        break;
                }

                //script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/{1}?nguoiSuDung={2}&fromDate={3}&toDate={4}&loaibc={5}\">');</script>", lblTittle.Text, page, nguoiSuDung, fromDate, toDate, loaibc);               
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{0}?khuVucId={1}&fromDate={2}&toDate={3}&loaibc={4}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", page, khuVucId, fromDate, toDate, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);          
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

                //CopyTreeView(tvLoaiKhieuNai, tvLoaiKhieuNai_ReportType4);
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