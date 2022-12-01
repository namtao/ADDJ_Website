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
    public partial class ucReportType4_VNPTTT : System.Web.UI.UserControl
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
                lblDoiTacId.Text = this.DoiTacId.ToString();
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();

                LoadTreeLoaiKhieuNai();

                List<DoiTacInfo> listDoiTac = null;

                switch (this.DoiTacId)
                {
                    case 2: // VNP1
                    case 7: // ĐKT1
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP1 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    case 3: // VNP2
                    case 14: // ĐKT2
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP2 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    case 5: // VNP3
                    case 19: // ĐKT3
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP3 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    default:
                        listDoiTac = new List<DoiTacInfo>();
                        DoiTacInfo objDoiTac = ServiceFactory.GetInstanceDoiTac().GetInfo(ConvertUtility.ToInt32(lblDoiTacId.Text, 0));
                        listDoiTac.Add(objDoiTac);
                        ddlVNPTTT.Enabled = false;
                        rowVNPTTT.Visible = false;
                        break;
                }

                ddlVNPTTT.DataSource = listDoiTac;
                ddlVNPTTT.DataBind();
                if (listDoiTac != null && listDoiTac.Count > 0)
                {
                    ddlVNPTTT.Items[0].Selected = true;
                }

                LoadComboPhongBan(ConvertUtility.ToInt32(ddlVNPTTT.Items[0].Value));
            }
        }

        protected void ddlVNPTTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadComboPhongBan(ConvertUtility.ToInt32(ddlVNPTTT.SelectedValue));
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
           
            string fromDate = txtFromDate_BaoCaoTongHopTheoKhieuNai.Text;
            string toDate = txtToDate_BaoCaoTongHopTheoKhieuNai.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string doiTacId = ddlVNPTTT.SelectedValue;
            string tenDoiTac = ddlVNPTTT.SelectedItem.Text;
            string phongBanId = ddlPhongBan.SelectedValue;
            string tenPhongBan = ddlPhongBan.SelectedItem.Text;
            //string layDuLieuTheo1HoacNhieuPhongBan = rblLayDuLieuTheo1HoacNhieuPhongBan.SelectedItem.Value;

            List<string> loaiKhieuNaiId = new List<string>();
            List<string> linhVucChungId = new List<string>();
            List<string> linhVucConId = new List<string>();
            
            foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai_ReportType2.Nodes)
            {
                if (nodeLoaiKhieuNai.Checked)
                {
                    loaiKhieuNaiId.Add(nodeLoaiKhieuNai.Value);
                }

                GetListLinhVucChung(nodeLoaiKhieuNai, ref linhVucChungId, ref linhVucConId);
            }            

            Session["LoaiKhieuNai"] = loaiKhieuNaiId;
            Session["LinhVucChung"] = linhVucChungId;
            Session["LinhVucCon"] = linhVucConId;

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

            if (loaiKhieuNaiId.Count == 0 && linhVucChungId.Count == 0 && linhVucConId.Count == 0)
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn loại khiếu nại", errorMessage);
            }


            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                //script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo thực trạng khiếu nại', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaotonghopgqknvnpttt.aspx?doiTacId={0}&tenDoiTac={1}&fromDate={2}&toDate={3}&loaibc={4}\">');</script>", doiTacId, tenDoiTac, fromDate, toDate, loaibc);
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaotonghopgqknvnpttt.aspx?doiTacId={0}&tenDoiTac={1}&fromDate={2}&toDate={3}&loaibc={4}&phongBanId={5}&tenPhongBan={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", doiTacId, tenDoiTac, fromDate, toDate, loaibc, phongBanId, tenPhongBan);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);
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
                        tvLoaiKhieuNai_ReportType2.Nodes.Add(node);
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

        private void GetListLinhVucChung(TreeNode parentNode, ref List<string> listLinhVucChungId, ref List<string> listLinhVucConId)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Checked)
                {
                    listLinhVucChungId.Add(node.Value);
                }

                GetListLinhVucCon(node, ref listLinhVucConId);
            }
        }

        private void GetListLinhVucCon(TreeNode parentNode, ref List<string> listLinhVucConId)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Checked)
                {
                    listLinhVucConId.Add(node.Value);
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 03/06/2014
        /// Todo : Load combo phòng ban
        /// </summary>
        private void LoadComboPhongBan(int doiTacId)
        {
            int phongBanId = ConvertUtility.ToInt32(lblPhongBanXuLyId.Text);
            List<PhongBanInfo> listPhongBan = null;

            if (phongBanId <= 0)
            {
                listPhongBan = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfDoiTacId(doiTacId);
            }
            else
            {
                listPhongBan = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfParentId(phongBanId);
            }

            if (listPhongBan == null)
            {
                listPhongBan = new List<PhongBanInfo>();
            }

            listPhongBan = ServiceFactory.GetInstancePhongBan().SortListPhongBanForTree(listPhongBan);

            PhongBanInfo objPhongBan = new PhongBanInfo();
            objPhongBan.Id = -1;
            objPhongBan.Name = "--Tất cả--";
            listPhongBan.Insert(0, objPhongBan);

            ddlPhongBan.DataSource = listPhongBan;
            ddlPhongBan.DataBind(); 
        }

        #endregion
        
    }
}