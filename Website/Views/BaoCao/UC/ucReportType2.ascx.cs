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
    public partial class ucReportType2 : System.Web.UI.UserControl
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
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);

            if (IsFirstLoad)
            {
                lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                LoadKhuVuc();
                ddlKhuVuc_ReportType2.SelectedValue = lblDoiTacId.Text;
                ddlKhuVuc_ReportType2_SelectedIndexChanged(null, null);
                ddlPhongBan_ReportType2.SelectedValue = lblPhongBanXuLyId.Text;

                //ddlKhuVuc_ReportType2.Enabled = false;
                //ddlPhongBan_ReportType2.Enabled = false;

                ddlKhuVuc_ReportType2.Enabled = this.DoiTacId == 1;
                ddlPhongBan_ReportType2.Enabled = this.DoiTacId == 1;

                LoadTreeLoaiKhieuNai();

                lblDoiTac.Visible = lblReportType.Text == "bc31";
                cblDoiTac.Visible = lblReportType.Text == "bc31";

                switch (this.ReportType)
                {
                    case "bc31":

                        break;
                    case "bc61":

                        break;
                }
            }
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

            Session["LoaiKhieuNai"] = loaiKhieuNaiId;
            Session["LinhVucChung"] = linhVucChungId;
            Session["LinhVucCon"] = linhVucConId;

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
                if (lblDoiTacId.Text == "1")
                {
                    if (lblReportType.Text == "bc_VNP_BaoCaoTongHopToGQKN")
                    {
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaotheoloaikhieunai.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}&isPhongCSKH=true\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaibc);
                    }
                }
                else
                {
                    if (lblReportType.Text == "bc61")
                    {
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaotheoloaikhieunai.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaibc);
                    }
                    else if (lblReportType.Text == "bc31" || lblReportType.Text == "bc_KS_BaoCaoTongHopKN" || lblReportType.Text == "bc_OB_BaoCaoTongHopKN")
                    {
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaotonghoptheokhieunai.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&doiTac={6}&loaibc={7}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, doiTac, loaibc);
                    }
                    else if (lblReportType.Text.ToLower().Equals("bc_VNP_BaoCaoTongHopLoaiKhieuNaiToGQKN".ToLower()))
                    {
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaotonghoptheokhieunai_gqkn.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&doiTac={6}&loaibc={7}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, doiTac, loaibc);
                    }
                    else
                    {
                        //script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo khối lượng công việc', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaokhoiluongcongviecdkt.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\">');</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaibc);
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaokhoiluongcongviecdkt.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaibc);
                    }
                }

            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);
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
                    //listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,TenDoiTac", "DonViTrucThuoc=" + listDoiTac[0].DonViTrucThuoc.ToString(), "");
                    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,TenDoiTac", "DonViTrucThuoc=" + listDoiTac[0].DonViTrucThuoc.ToString() + " AND DoiTacType IN (1, 5)", "");
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

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);
        }

        #endregion

        #region Private methods

        private void LoadKhuVuc()
        {
            ddlKhuVuc_ReportType2.Items.Clear();

            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
            string space = string.Empty;
            if (listDoiTac != null)
            {
                List<DoiTacInfo> listDoiTacRoot = listDoiTac.FindAll(delegate (DoiTacInfo obj) { return obj.DonViTrucThuoc == 0; });

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

        private void LoadTreeLoaiKhieuNai()
        {
            List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id, Name, ParentId, Sort", "Status = 1", "Sort");
            if (listLoaiKhieuNai != null && listLoaiKhieuNai.Count > 0)
            {
                // Lấy danh sách cha
                List<LoaiKhieuNaiInfo> objs = listLoaiKhieuNai.Where(v => v.ParentId == 0).OrderBy(v => v.Sort).ToList();
                foreach (LoaiKhieuNaiInfo obj in objs)
                {
                    TreeNode node = new TreeNode();
                    tvLoaiKhieuNai_ReportType2.Nodes.Add(node);
                    AddNode(listLoaiKhieuNai, obj, node);
                    node.Collapse();

                    //for (int i = 0; i < listLoaiKhieuNai.Count; i++)
                    //{
                    //    if (listLoaiKhieuNai[i].ParentId == 0)
                    //    {
                    //        TreeNode node = new TreeNode();                              
                    //        tvLoaiKhieuNai_ReportType2.Nodes.Add(node);
                    //        AddNode(listLoaiKhieuNai, listLoaiKhieuNai[i], node);
                    //        node.Collapse();
                    //        i--; => Gây lỗi thiếu
                    //    }
                    //}               
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
                    i--; // Chưa hiểu ý nghĩa dòng này
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