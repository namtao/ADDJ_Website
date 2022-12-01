using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Website.AppCode;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;

namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType16_VNPTTT : System.Web.UI.UserControl
    {
        public string ReportType { get; set; }


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
                // Trường hợp ĐKT sử dụng báo cáo này thì phải gán lại giá trị DoiTacId theo giá trị Id của VNP khu vực => mới ra được các VNPT trực thuộc khu vực
              
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();

                
            }
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;

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
            if (lblReportType.Text == "bc_VNP_BaoCaoTongHopGiamTruDVGTGTDichVu")
            {
                page = "emptypagevnpttt.aspx?";
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
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page=bcth_giamtru_dvgtgt_dichvu&&fromDate={2}&toDate={3}&loaibc={4}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page,  fromDate, toDate, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

      
        #endregion
    }
}