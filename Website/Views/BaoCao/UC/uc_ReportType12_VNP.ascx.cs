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

namespace Website.Views.BaoCao.UC
{
    public partial class uc_ReportType12_VNP : System.Web.UI.UserControl
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

                LoadComboDoiTac();
                ddlKhuVuc.SelectedValue = userInfo.KhuVucId.ToString();
                ddlKhuVuc.Enabled = userInfo.KhuVucId == DoiTacInfo.DoiTacIdValue.VNP;

                ddlNguonKhieuNai.DataSource = ServiceFactory.GetInstanceKhieuNai().GetListNguonKhieuNai(true);
                ddlNguonKhieuNai.DataTextField = "Name";
                ddlNguonKhieuNai.DataValueField = "Id";
                ddlNguonKhieuNai.DataBind();

                LoadComboLoaiKhieuNai();
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
            string doiTacId = ddlKhuVuc.SelectedValue;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;
            string nguonKhieuNai = ddlNguonKhieuNai.SelectedValue;
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

            string page = string.Empty;

            if (lblReportType.Text == "bc_VNP_BaoCaoChatLuongPhucVu")
            {
                title = "Báo cáo chất lượng phục vụ";
                page = "baocaochatluongphucvu.aspx?reportType=baocaotonghop&";
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
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doiTacId={2}&fromDate={3}&toDate={4}&loaibc={5}&nguonKhieuNai={6}&loaiKhieuNaiId={7}&linhVucChungId={8}&linhVucConId={9}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, doiTacId, fromDate, toDate, loaibc, nguonKhieuNai, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        protected void lbReport_BaoCaoChiTietChatLuongPhucVu_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            string doiTacId = ddlKhuVuc.SelectedValue;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;
            string nguonKhieuNai = ddlNguonKhieuNai.SelectedValue;
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

            string page = string.Empty;

            if (lblReportType.Text == "bc_VNP_BaoCaoChatLuongPhucVu")
            {
                title = "Báo cáo chi tiết chất lượng phục vụ";
                page = "baocaochatluongphucvu.aspx?reportType=baocaochitiet&";
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
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doiTacId={2}&fromDate={3}&toDate={4}&loaibc={5}&nguonKhieuNai={6}&loaiKhieuNaiId={7}&linhVucChungId={8}&linhVucConId={9}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, doiTacId, fromDate, toDate, loaibc, nguonKhieuNai, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        #endregion

        

        #region Private methods

        private void LoadComboDoiTac()
        {
            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "Id IN (1, 2, 3, 5) OR DoiTacType = 5", "");
            if(listDoiTac != null)
            {
                ListItem item = null;
                string space = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                for (int indexCap1 = 0; indexCap1 < listDoiTac.Count; indexCap1++)
                {
                    if (listDoiTac[indexCap1].DonViTrucThuoc == 0)
                    {
                        item = new ListItem(listDoiTac[indexCap1].TenDoiTac, listDoiTac[indexCap1].Id.ToString());
                        ddlKhuVuc.Items.Add(item);

                        for(int indexCap2 = 0;indexCap2<listDoiTac.Count;indexCap2 ++)
                        {
                            if (listDoiTac[indexCap1].Id == listDoiTac[indexCap2].DonViTrucThuoc)
                            {
                                item = new ListItem(space + listDoiTac[indexCap2].TenDoiTac, listDoiTac[indexCap2].Id.ToString());
                                ddlKhuVuc.Items.Add(item);

                                for(int indexCap3 = 0;indexCap3<listDoiTac.Count;indexCap3++)
                                {
                                    if(listDoiTac[indexCap2].Id == listDoiTac[indexCap3].DonViTrucThuoc)
                                    {
                                        item = new ListItem(space + space + listDoiTac[indexCap3].TenDoiTac, listDoiTac[indexCap3].Id.ToString());
                                        ddlKhuVuc.Items.Add(item);
                                    }
                                }
                            }
                        } // end for(int indexCap2 = 0;indexCap2<listDoiTac.Count;indexCap2 ++)
                    } // end  if(listDoiTac[i].DonViTrucThuoc == 0)
                }
            }            
        }

        private void LoadComboLoaiKhieuNai()
        {
            List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("*","Status=1 AND LoaiKhieuNai_NhomId=5 AND Id <> 89 AND ParentId=0","");
            ddlLoaiKhieuNai.DataSource = listLoaiKhieuNai;
            ddlLoaiKhieuNai.DataTextField = "Name";
            ddlLoaiKhieuNai.DataValueField = "Id";
            ddlLoaiKhieuNai.DataBind();

            ListItem item = new ListItem("--Tất cả--", "-1");
            ddlLoaiKhieuNai.Items.Insert(0, item);
        }

        #endregion

        protected void ddlLoaiKhieuNai_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<LoaiKhieuNaiInfo> listLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("*", "Status=1 AND ParentId=" + ddlLoaiKhieuNai.SelectedValue, "");
            ddlLinhVucChung.DataSource = listLinhVucChung;
            ddlLinhVucChung.DataTextField = "Name";
            ddlLinhVucChung.DataValueField = "Id";
            ddlLinhVucChung.DataBind();

            ListItem item = new ListItem("--Tất cả--", "-1");
            ddlLinhVucChung.Items.Insert(0, item);

            ddlLinhVucCon.Items.Clear();

        }

        protected void ddlLinhVucChung_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<LoaiKhieuNaiInfo> listLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("*", "Status=1 AND ParentId=" + ddlLinhVucChung.SelectedValue, "");
            ddlLinhVucCon.DataSource = listLinhVucCon;
            ddlLinhVucCon.DataTextField = "Name";
            ddlLinhVucCon.DataValueField = "Id";
            ddlLinhVucCon.DataBind();

            ListItem item = new ListItem("--Tất cả--", "-1");
            ddlLinhVucCon.Items.Insert(0, item);
        }
    }
}