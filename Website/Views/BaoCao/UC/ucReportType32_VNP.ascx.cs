using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using AIVietNam.Core;

namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType32_VNP : System.Web.UI.UserControl
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
                lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                LoadKhuVuc();                        

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

            string doiTacId = ddlKhuVuc.SelectedValue;
            string reportType = rblReportType.SelectedValue;
            string loaiKhieuNaiId = string.Empty;
            string linhVucChungId = string.Empty;
            string linhVucConId = string.Empty;
            string loaiThueBao = ddlLoaiThueBao.SelectedValue;

            string fromDate1 = txtFromDate1.Text;
            string toDate1 = txtToDate1.Text;
            string fromDate2 = txtFromDate2.Text;
            string toDate2 = txtToDate2.Text;

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
                string url = string.Format("/Views/BaoCao/Popup/baocaobieudososanhsoluonggiaiquyetkhieunai.aspx?reportType={0}&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&listDate={4}&doiTacId={5}&loaiThueBao={6}", reportType, loaiKhieuNaiId, linhVucChungId, linhVucConId, sDate, doiTacId, loaiThueBao);
                if (url.Length > 1900)
                {
                    errorMessage = "Bạn chọn quá nhiều Loại khiếu nại/Lĩnh vực chung/Lĩnh vực chọn. Hãy bỏ bớt đi";
                    script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
                }
                else
                {                                       
                    if (lblReportType.Text == "bc_VNP_ThongKeSoLuongGQKNTheo1KhoangThoiGian")
                    {
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaobieudososanhsoluonggiaiquyetkhieunai.aspx?reportType={0}&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&listDate={4}&doiTacId={5}&loaiThueBao={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", reportType, loaiKhieuNaiId, linhVucChungId, linhVucConId, sDate, doiTacId, loaiThueBao);
                    }
                    else if (lblReportType.Text == "bc_VNP_ThongKeTongSoTienGiamTruTheo2KhoangThoiGian")
                    {
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaobieudososanhtongsotiengiamtru.aspx?reportType={0}&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&listDate={4}&doiTacId={5}&loaiThueBao={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", reportType, loaiKhieuNaiId, linhVucChungId, linhVucConId, sDate, doiTacId, loaiThueBao);
                    }
                }

            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }        

        #endregion

        #region Private methods

        private void LoadKhuVuc()
        {
            ddlKhuVuc.Items.Clear();

            ddlKhuVuc.Items.Clear();
            ListItem itemVNP = new ListItem("VNP", DoiTacInfo.DoiTacIdValue.VNP.ToString());
            ListItem itemVNP1 = new ListItem("VNP1", DoiTacInfo.DoiTacIdValue.VNP1.ToString());
            ListItem itemVNP2 = new ListItem("VNP2", DoiTacInfo.DoiTacIdValue.VNP2.ToString());
            ListItem itemVNP3 = new ListItem("VNP3", DoiTacInfo.DoiTacIdValue.VNP3.ToString());

            ddlKhuVuc.Items.Add(itemVNP);
            ddlKhuVuc.Items.Add(itemVNP1);
            ddlKhuVuc.Items.Add(itemVNP2);
            ddlKhuVuc.Items.Add(itemVNP3);

            //List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
            //string space = string.Empty;
            //if (listDoiTac != null)
            //{
            //    List<DoiTacInfo> listDoiTacRoot = listDoiTac.FindAll(delegate(DoiTacInfo obj) { return obj.DonViTrucThuoc == 0; });

            //    for (int i = 0; i < listDoiTacRoot.Count; i++)
            //    {
            //        ListItem item = new ListItem();
            //        item.Text = string.Format("{0}{1}", space, listDoiTacRoot[i].TenDoiTac);
            //        item.Value = listDoiTacRoot[i].Id.ToString();
            //        ddlKhuVuc.Items.Add(item);

            //        int donViTrucThuocChild = listDoiTacRoot[i].Id;
            //        listDoiTacRoot.RemoveAt(i);
            //        i--;

            //        LoadChildDoiTac(space, donViTrucThuocChild, listDoiTac);
            //    }
            //}
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
                    ddlKhuVuc.Items.Add(item);

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