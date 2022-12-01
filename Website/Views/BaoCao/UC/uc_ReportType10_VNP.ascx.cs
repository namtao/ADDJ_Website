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
    /// Created date : 27/08/2014
    /// </summary>
    public partial class uc_ReportType10_VNP : System.Web.UI.UserControl
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

                LoadPhongBan(this.PhongBanXuLyId);
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
            string listPhongBanId = string.Empty;
            for (int i = 0; i < cblPhongBan.Items.Count; i++)
            {
                if (cblPhongBan.Items[i].Selected)
                {
                    listPhongBanId = string.Format("{0}{1},", listPhongBanId, cblPhongBan.Items[i].Value);
                    isSelected = true;
                }
            }
            if (listPhongBanId.Length > 0)
            {
                listPhongBanId = listPhongBanId.TrimEnd(',');
            }

            //string loaibc = rblLoaiBaoCao.SelectedItem.Value;

            string errorMessage = string.Empty;
            string script = string.Empty;                    

            if (!isSelected)
            {
                errorMessage = "Bạn phải chọn phòng ban tiếp nhận xử lý";
            }

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/danhsachkhieunai.aspx?fromPage={0}&phongBanId={1}&listPhongBanTiepNhanId={2}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblReportType.Text, lblPhongBanXuLyId.Text, listPhongBanId);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);            
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : 
        /// </summary>
        private void LoadPhongBan(int phongBanId)
        {            
            List<PhongBan2PhongBanInfo> listPhongBan2PhongBan = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(phongBanId);
            if(listPhongBan2PhongBan != null && listPhongBan2PhongBan.Count > 0)
            {
                string sListPhongBan = listPhongBan2PhongBan[0].PhongBanDen.Replace("[", "").Replace("]", "").Replace(",","','");
                sListPhongBan = string.Format("'{0}'", sListPhongBan);

                string whereClause = string.Format("Id IN ({0})", sListPhongBan);
                List<PhongBanInfo> listPhongBanInfo = ServiceFactory.GetInstancePhongBan().GetListDynamic("*", whereClause, "Name ASC");
                if(listPhongBanInfo != null && listPhongBanInfo.Count > 0)
                {
                    cblPhongBan.DataSource = listPhongBanInfo;
                    cblPhongBan.DataTextField = "Name";
                    cblPhongBan.DataValueField = "Id";
                    cblPhongBan.DataBind();

                    for(int i=0;i<cblPhongBan.Items.Count;i++)
                    {
                        cblPhongBan.Items[i].Selected = true;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 27/08/2014
        /// Todo : Chuyển trạng thái của toàn bộ checkbox của các phòng ban
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            for(int i=0;i<cblPhongBan.Items.Count;i++)
            {
                cblPhongBan.Items[i].Selected = chkCheckAll.Checked;
            }
        }
    }
}