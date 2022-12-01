using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website.Views.BaoCao.UC
{
    public partial class ucBaoCaoToTruongToKTV : System.Web.UI.UserControl
    {       
        public string ReportType {get;set;}      
        
        public int PhongBanXuLyId {get;set;}

        public string ReportTitle { get; set; }

        public bool IsFirstLoad { get; set; }

        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);

            if(IsFirstLoad)
            {
                lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
            }
            
            //lblDoiTacId.Text = this.DoiTacId.ToString();
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string path = string.Empty;           
            switch(lblReportType.Text)
            { 
                case "bc_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV":                    
                    path = string.Format("/Views/BaoCao/Popup/baocaosoluongknphanhoivektvcuatttoktv.aspx?donViID={0}&donVi={1}&fromDate={2}&toDate={3}&loaibc={4}", PhongBanXuLyId, "", fromDate, toDate, loaibc);
                    break;
                case "bc_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV":
                    path = string.Format("/Views/BaoCao/Popup/baocaosoluongkhieunaiquahanvatondongcuaktvcuatttoktv.aspx?donViID={0}&donVi={1}&fromDate={2}&toDate={3}&loaibc={4}", PhongBanXuLyId, "", fromDate, toDate, loaibc);
                    break;
                default:

                    break;
            }

            //string script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"{1}\">');</script>", lblReportType.Text, path);
            string script = string.Format("<script type='text/javascript'> window.open(url=\"{0}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", path);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        #endregion
       
    }
}