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
    public partial class uc_ReportType11_VNP : System.Web.UI.UserControl
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

            if (this.IsFirstLoad)
            {
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                AdminInfo userInfo = LoginAdmin.AdminLogin();
                if (userInfo == null) return;

                ddlKhuVuc.SelectedValue = userInfo.KhuVucId.ToString();
                ddlKhuVuc.Enabled = userInfo.KhuVucId == DoiTacInfo.DoiTacIdValue.VNP;

                ddlNguonKhieuNai.DataSource = ServiceFactory.GetInstanceKhieuNai().GetListNguonKhieuNai(true);
                ddlNguonKhieuNai.DataTextField = "Name";
                ddlNguonKhieuNai.DataValueField = "Id";
                ddlNguonKhieuNai.DataBind();

                //LoadLoaiKhieuNai();
                LoadTreeViewLoaiKhieuNai();

                //ddlThuocDonVi_SelectedIndexChanged(sender, e);
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
            string khuVucId = ddlKhuVuc.SelectedValue;           
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;
            string nguonKhieuNai = ddlNguonKhieuNai.SelectedValue;
            string sLoaiKhieuNai = string.Empty;
            string sLinhVucChung = string.Empty;
            string sLinhVucCon = string.Empty;
            string displayLevelLoaiKhieuNai = rblDisplayLevelLoaiKhieuNai.SelectedItem.Value;

            foreach(TreeNode node in tvLoaiKhieuNai.Nodes)
            {
                if (node.Checked)
                {
                    sLoaiKhieuNai = string.Format("{0}{1},", sLoaiKhieuNai, node.Value);

                    foreach(TreeNode nodeLinhVucChung in node.ChildNodes)
                    {
                        if(nodeLinhVucChung.Checked)
                        {
                            sLinhVucChung = string.Format("{0}{1},", sLinhVucChung, nodeLinhVucChung.Value);

                            foreach(TreeNode nodeLinhVucCon in nodeLinhVucChung.ChildNodes)
                            {
                                if(nodeLinhVucCon.Checked)
                                {
                                    sLinhVucCon = string.Format("{0}{1},", sLinhVucCon, nodeLinhVucCon.Value);
                                }
                            }
                        }
                    }
                }                
            }

            if (sLoaiKhieuNai.Length > 0)
            {
                sLoaiKhieuNai = sLoaiKhieuNai.TrimEnd(',');
            }
            if(sLinhVucChung.Length > 0)
            {
                sLinhVucChung = sLinhVucChung.TrimEnd(',');
            }
            if(sLinhVucCon.Length > 0)
            {
                sLinhVucCon = sLinhVucCon.TrimEnd(',');
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

            string page = string.Empty;
            
            if (lblReportType.Text == "bc_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi")
            {
                title = "Báo cáo thống kê theo nguyên nhân lỗi";
                page = "baocaothongketheonguyennhanloi.aspx?";
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
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}khuVucId={2}&doiTacId=-1&phongBanId=-1&fromDate={3}&toDate={4}&loaibc={5}&nguonKhieuNai={6}&displayLevelLoaiKhieuNai={7}&loaiKhieuNaiId={8}&linhVucChungId={9}&linhVucConId={10}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, khuVucId, fromDate, toDate, loaibc, nguonKhieuNai, displayLevelLoaiKhieuNai, sLoaiKhieuNai, sLinhVucChung, sLinhVucCon);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/07/2015
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_DanhSachKhieuNai_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            string khuVucId = ddlKhuVuc.SelectedValue;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;
            string nguonKhieuNai = ddlNguonKhieuNai.SelectedValue;
            string sLoaiKhieuNai = string.Empty;
            string sLinhVucChung = string.Empty;
            string displayLevelLoaiKhieuNai = rblDisplayLevelLoaiKhieuNai.SelectedItem.Value;

            List<int> listLoaiKhieuNaiId = new List<int>();
            List<int> listLinhVucChungId = new List<int>();
            List<int> listLinhVucConId = new List<int>();

            foreach(TreeNode node in tvLoaiKhieuNai.Nodes)
            {
                if (node.Checked)
                {                    
                    listLoaiKhieuNaiId.Add(ConvertUtility.ToInt32(node.Value));

                    if(displayLevelLoaiKhieuNai == "2")
                    {
                        foreach(TreeNode childNode in node.ChildNodes)
                        {
                            if(childNode.Checked)
                            {
                                listLinhVucChungId.Add(ConvertUtility.ToInt32(childNode.Value));
                            }
                        }
                    }
                }
            }

            Session["ListLoaiKhieuNaiId"] = listLoaiKhieuNaiId;
            Session["ListLinhVucChungId"] = listLinhVucChungId;
            Session["ListLinhVucConId"] = listLinhVucConId;                        
            

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

            string page = string.Empty;

            title = "Danh sách khiếu nại";
            page = "danhsachkhieunai.aspx?";


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
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}fromPage=bc_vnp_danhsachkhieunaitheoloaikhieunai&khuVucId={2}&fromDate={3}&toDate={4}&loaibc={5}&nguonKhieuNai={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, khuVucId, fromDate, toDate, loaibc, nguonKhieuNai);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 09/07/2015
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlThuocDonVi_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai.Nodes)
        //    {
        //        SetCheckBoxChecked(nodeLoaiKhieuNai, null, false);
        //    }

        //    int thuocDonVi = ConvertUtility.ToInt32(ddlThuocDonVi.SelectedValue);
        //    List<LoaiKhieuNaiInfo> listLoaiKhieuNai = null;

        //    if (thuocDonVi > 0)
        //    {
        //        listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("*", "ThuocDonVi=" + thuocDonVi, "");
        //        foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai.Nodes)
        //        {
        //            SetCheckBoxChecked(nodeLoaiKhieuNai, listLoaiKhieuNai, true);
        //        } // end foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai.Nodes)
        //    }
        //}

        #endregion      

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Todo : 09/07/2015
        /// </summary>
        /// <param name="listLoaiKhieuNai"></param>
        /// <param name="node"></param>
        private void SetCheckBoxChecked(TreeNode node, List<LoaiKhieuNaiInfo> listLoaiKhieuNai, bool isChecked)
        {
            if(listLoaiKhieuNai == null || listLoaiKhieuNai.Count == 0)
            {
                node.Checked = isChecked;
                foreach (TreeNode childNode in node.ChildNodes)
                {
                    SetCheckBoxChecked(childNode, null, isChecked);
                }
            }
            else
            {
                for (int i = 0; i < listLoaiKhieuNai.Count; i++)
                {
                    if (listLoaiKhieuNai[i].Id.ToString() == node.Value)
                    {
                        node.Checked = isChecked;
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            SetCheckBoxChecked(childNode, listLoaiKhieuNai, isChecked);
                        }

                        break;
                    }
                }
            }            
        }       

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/07/2015
        /// </summary>
        private void LoadTreeViewLoaiKhieuNai()
        {
            List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name,ParentId", "Status=1", "Sort");
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

        private void GetListLinhVucChung(TreeNode parentNode, ref List<int> listLinhVucChungId, ref List<int> listLinhVucConId)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Checked)
                {
                    listLinhVucChungId.Add(ConvertUtility.ToInt32(node.Value));                    
                }

                GetListLinhVucCon(node, ref listLinhVucConId);
            }
        }

        private void GetListLinhVucCon(TreeNode parentNode, ref List<int> listLinhVucConId)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Checked)
                {
                    listLinhVucConId.Add(ConvertUtility.ToInt32(node.Value));                   
                }
            }
        }
      
        #endregion

        

        
    }
}