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
    /// Created date : 11/12/2013
    /// Todo : Màn hình chọn điều kiện hiển thị báo cáo
    /// </summary>
    public partial class ucReportType2_VNPTTT : System.Web.UI.UserControl
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

                LoadComboPhongBan(this.DoiTacId);
            }
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);
            
            string title = string.Empty;
            string donViId = ddlPhongBan.Visible ? ddlPhongBan.SelectedValue : "-1";
            string donVi = ddlPhongBan.SelectedItem != null ? ddlPhongBan.SelectedItem.Text : string.Empty;            
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;            

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
            if (lblReportType.Text == "bc_VNPTTT_SoLuongKNGDVTiepNhanXuLy")
            {
                page = "baocaosoluongkntiepnhanvaxulyvnpttt.aspx?";               
            }
            else if (lblReportType.Text == "bc_VNPTTT_GiamTruKhieuNaiDichVu")
            {
                page = "baocaogiamtrukhieunaidichvuvnpttt.aspx?";
            }
            else if (lblReportType.Text == "bc_VNPTTT_KhieuNaiDichVu")
            {
                page = "baocaokhieunaidichvuvnpt.aspx?";
            }
            else if (lblReportType.Text == "bc_VNPTTT_ThucTrangGQKN")
            {
                page = "baocaothuctranggqknvnpttt.aspx?";
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
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}donViId={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\">');</script>", lblTittle.Text, page, donViId, donVi, fromDate, toDate, loaibc);                      
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}donViId={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, donViId, donVi, fromDate, toDate, loaibc);                      
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/12/2013
        /// </summary>
        /// <param name="doiTacId"></param>
        private void LoadComboPhongBan(int doiTacId)
        {
            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);
            ddlPhongBan.DataSource = listPhongBan;
            ddlPhongBan.DataBind();
        }

        #endregion

    }
}