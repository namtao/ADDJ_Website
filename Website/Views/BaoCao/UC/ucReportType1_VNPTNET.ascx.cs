using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AIVietNam.Core;
using AIVietNam.GQKN.Entity;

namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType1_VNPTNET : System.Web.UI.UserControl
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
            }

            if(lblPhongBanXuLyId.Text == "904" || lblPhongBanXuLyId.Text == "907"
                || lblPhongBanXuLyId.Text == "910" || lblPhongBanXuLyId.Text == "911" || lblPhongBanXuLyId.Text == "914")
            {
                ddlKhuVuc.SelectedValue = DoiTacInfo.DoiTacIdValue.VNP1.ToString();
            }
            else if (lblPhongBanXuLyId.Text == "905" || lblPhongBanXuLyId.Text == "908"
                || lblPhongBanXuLyId.Text == "918" || lblPhongBanXuLyId.Text == "919" || lblPhongBanXuLyId.Text == "920")
            {
                ddlKhuVuc.SelectedValue = DoiTacInfo.DoiTacIdValue.VNP2.ToString();
            }
            else if (lblPhongBanXuLyId.Text == "906" || lblPhongBanXuLyId.Text == "909"
                || lblPhongBanXuLyId.Text == "915" || lblPhongBanXuLyId.Text == "916" || lblPhongBanXuLyId.Text == "917")
            {
                ddlKhuVuc.SelectedValue = DoiTacInfo.DoiTacIdValue.VNP3.ToString();
            }

            ddlKhuVuc.Enabled = ddlKhuVuc.SelectedValue == "0";
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/10/2013
        /// Todo : Hiển thị báo cáo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            string khuVucId = ddlKhuVuc.SelectedValue;
            string khuVuc = ddlKhuVuc.SelectedItem != null ? ddlKhuVuc.SelectedItem.Text : string.Empty;
            string donViId = "-1"; //ddlPhongBan_ReportType1.Visible ? ddlPhongBan_ReportType1.SelectedValue : "-1";
            string donVi = string.Empty;// ddlPhongBan_ReportType1.SelectedItem != null ? ddlPhongBan_ReportType1.SelectedItem.Text : string.Empty;
            string doiTacId = "-1";// ddlDoiTac_ReportType1.SelectedValue;
            string tenDoiTac = string.Empty;// ddlDoiTac_ReportType1.SelectedItem != null ? ddlDoiTac_ReportType1.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string nguonKhieuNai = "-1";

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

            title = "Báo cáo tổng hợp chất lượng mạng";
            var page = "baocaotonghopchatluongmang.aspx?";

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
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}\">');</script>", title, page, khuVucId, donViId, fromDate, toDate, loaibc);
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}&nguonKhieuNai={7}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, khuVucId, donViId, fromDate, toDate, loaibc, nguonKhieuNai);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        #endregion       
    }
}