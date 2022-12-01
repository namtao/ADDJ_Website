using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;

namespace Website.Views.BaoCao.UC
{
    public partial class uc_ReportType7_VNP : System.Web.UI.UserControl
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
                lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();
                int khuVucId = 0;
                switch(this.DoiTacId)
                {
                    case 7: // Đài khai thác 1
                        khuVucId = 2;
                        break;
                    case 14: // Đài khai thác 2
                        khuVucId=3;
                        break;
                    case 19: // Đài khai thác 3
                        khuVucId = 5;
                        break;
                }

                LoadDoiTac(khuVucId);            
            }

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

            bool isSelected = false;
            string listDoiTac = string.Empty;
            for(int i=0;i<cblDoiTac.Items.Count;i++)
            {
                if(cblDoiTac.Items[i].Selected)
                {
                    listDoiTac = string.Format("{0}{1},", listDoiTac, cblDoiTac.Items[i].Value);
                    isSelected = true;
                }                
            }
            if(listDoiTac.Length > 0)
            {
                listDoiTac = listDoiTac.TrimEnd(',');
            }

            string loaibc = rblLoaiBaoCao.SelectedItem.Value;

            string errorMessage = string.Empty;
            string script = string.Empty;
           
            string page = string.Empty;
            string title = "Báo cáo số lượng khiếu nại tồn đọng và quá hạn của đối tác";
            page = "baocaosoluongkhieunaitondongvaquahancuadoitac.aspx?";            

            if(!isSelected)
            {
                errorMessage = "Bạn phải chọn đối tác";
            }

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}\">');</script>", title, page, khuVucId, donViId, fromDate, toDate, loaibc);
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}listDoiTac={2}&loaibc={3}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, listDoiTac, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }        

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Load combo đối tác
        /// </summary>
        private void LoadDoiTac(int doiTacId)
        {
            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + doiTacId + " AND DoiTacType=5", "TenDoiTac ASC");
            cblDoiTac.DataSource = listDoiTac;
            cblDoiTac.DataTextField = "TenDoiTac";
            cblDoiTac.DataValueField = "Id";
            cblDoiTac.DataBind();

            for(int i=0;i<cblDoiTac.Items.Count;i++)
            {
                cblDoiTac.Items[i].Selected = true;
            }
        }
       
        #endregion
    }
}