using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;

namespace Website.Views.BaoCao
{
    public partial class BaoCaoThongKeNguoiDung : PageBase
    {
        #region Event methods

        private readonly int PHONG_PTDV = 52;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserRead)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            if (!IsPostBack)
            {
                LoadNguoiDung();
                LoadTreeLoaiKhieuNai();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
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
            if (sender != null && sender is LinkButton)
            {
                LinkButton lb = (LinkButton)sender;
                lblReportType.Text = lb.CommandArgument;

                if (!pnReportType1.Visible)
                {
                    pnReportType1.Visible = true;
                }
            }
            else
            {
                lblReportType.Text = string.Empty;
            }

            ShowHideReportCondition(lblReportType.Text);
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
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;

            string loaiKhieuNaiId = string.Empty;
            string linhVucChungId = string.Empty;
            string linhVucConId = string.Empty;

            string nguoiSuDung = string.Empty;
            for (int i = 0; i < cblNguoiDung.Items.Count; i++)
            {
                if (cblNguoiDung.Items[i].Selected)
                {
                    nguoiSuDung = string.Format("{0}{1},", nguoiSuDung, cblNguoiDung.Items[i].Value);
                }
            }

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

            //if (loaiKhieuNaiId.Length == 0 && linhVucChungId.Length == 0 && linhVucConId.Length == 0)
            //{
            //    errorMessage = string.Format("{0}\\nBạn phải chọn loại khiếu nại", errorMessage);
            //}


            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                string title = string.Empty;
                string page = string.Empty;
                switch (lblReportType.Text)
                {
                    case "rptSoLieuPAKNDaXuLy":
                        title = "Báo cáo số liệu phản ánh khiếu nại người dùng đã xử lý ";
                        page = "baocaosolieupakndaxulynguoidung.aspx";
                        break;
                    case "rptTongHopSoLieuPAKNDangTonDong":
                        title = "Báo cáo tổng hợp số liệu phản ánh khiếu nại đang tồn đọng";
                        page = "baocaotonghopsolieupakndangtondongttptdv.aspx";
                        break;
                    case "rptTongHopSoLieuPAKNDaTiepNhan":
                        title = "Báo cáo tổng hợp số liệu phản ánh khiếu nại đã tiếp nhận";
                        page = "baocaotonghopsolieupakndatiepnhanttptdv.aspx";
                        break;
                    case "rptChiTietPAKNDaTiepNhan":
                        title = "Báo cáo chi tiết phản ánh khiếu nại đã tiếp nhận";
                        page = "baocaochitietpakndatiepnhanttptdv.aspx";
                        break;
                    default:
                        page = "";
                        break;
                }

                script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/{1}?nguoiSuDung={2}&fromDate={3}&toDate={4}&loaibc={5}\">');</script>", title, page, nguoiSuDung, fromDate, toDate, loaibc);
                //if (ddlReportType.SelectedValue == "bc31")
                //{
                //    //string script = string.Format("<script type='text/javascript'>window.open('/Views/BaoCao/Popup/baocaotonghoptheokhieunai.aspx?donViID={0}&donVi={1}&fromDate={2}&toDate={3}&doiTac={4}&loaiKhieuNai={5}&linhVucChung={6}&linhVucCon={7}&loaibc={8}','Báo cáo tổng hợp theo khiếu nại', 'menubar=0, statusbar=0, titlebar=0,fullscreen=1');</script>", donViId, donVi, fromDate, toDate, doiTac, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);
                //    script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo tổng hợp theo khiếu nại', '<iframe style=\"border:none\" width=\"1024px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaotonghoptheokhieunai.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&doiTac={6}&loaiKhieuNai={7}&linhVucChung={8}&linhVucCon={9}&loaibc={10}\">');</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, doiTac, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);
                //}
                //else if (ddlReportType.SelectedValue == "bc61")
                //{
                //    script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo theo loại khiếu nại', '<iframe style=\"border:none\" width=\"1024px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaotheoloaikhieunai.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaiKhieuNai={6}&linhVucChung={7}&linhVucCon={8}&loaibc={9}\">');</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaiKhieuNaiId, linhVucChungId, linhVucConId, loaibc);
                //}
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai.ClientID + "');", true);
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
                case "rptSoLieuPAKNDaXuLy":
                    lblTittle.Text = "Báo cáo số liệu PAKN đã xử lý";
                    break;
                case "rptTongHopSoLieuPAKNDangTonDong":
                    lblTittle.Text = "Báo cáo tổng hợp số liệu PAKN đang tồn đọng";
                    break;
                case "rptTongHopSoLieuPAKNDaTiepNhan":
                    lblTittle.Text = "Báo cáo tổng hợp số liệu PAKN đã tiếp nhận";
                    break;
                case "rptChiTietPAKNDaTiepNhan":
                    lblTittle.Text = "Báo cáo chi tiết PAKN đã tiếp nhận";
                    break;
                default:
                    lblTittle.Text = string.Empty;
                    break;
            } // end switch                        

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNaiId + "');", true);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/11/2013
        /// Todo : Hiển thị danh sách người dùng của TTPTDV
        /// </summary>
        private void LoadNguoiDung()
        {
            List<NguoiSuDungInfo> listNguoiSuDung = ServiceFactory.GetInstanceNguoiSuDung().GetListNguoiSuDungByPhongBanId(PHONG_PTDV);
            if (listNguoiSuDung != null)
            {
                for (int i = 0; i < listNguoiSuDung.Count; i++)
                {
                    ListItem item = new ListItem(listNguoiSuDung[i].TenDayDu, listNguoiSuDung[i].TenTruyCap, true);
                    cblNguoiDung.Items.Add(item);
                }
            }
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