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
    public partial class uc_ReportType9_VNP : System.Web.UI.UserControl
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
                txtToTime.Text = DateTime.Now.ToString("HH:mm");

                LoadKhuVuc();
                ddlKhuVuc_SelectedIndexChanged(null, null);
                ddlKhuVuc.Enabled = this.DoiTacId == DoiTacInfo.DoiTacIdValue.VNP;

                switch (this.DoiTacId)
                {
                    case 2:
                    case 7: // Đài khai thác 1
                        ddlKhuVuc.SelectedValue = DoiTacInfo.DoiTacIdValue.VNP1.ToString();
                        ddlKhuVuc_SelectedIndexChanged(null, null);
                        break;
                    case 3:
                    case 14: // Đài khai thác 2
                        ddlKhuVuc.SelectedValue = DoiTacInfo.DoiTacIdValue.VNP2.ToString();
                        ddlKhuVuc_SelectedIndexChanged(null, null);
                        break;
                    case 5:
                    case 19: // Đài khai thác 3
                        ddlKhuVuc.SelectedValue = DoiTacInfo.DoiTacIdValue.VNP3.ToString();
                        ddlKhuVuc_SelectedIndexChanged(null, null);
                        break;

                    case 21:
                    case 10000:
                    case 10100:                        
                    case 10101:
                    case 10111:
                    case 10113:
                    case 10184:
                        cblDoiTac.Enabled = false;
                        if(cblDoiTac.Items.Count > 0)
                        {
                            for(int i=0;i<cblDoiTac.Items.Count;i++)
                            {
                                if(cblDoiTac.Items[i].Value == this.DoiTacId.ToString())
                                {
                                    cblDoiTac.Items[i].Selected = true;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        
                        break;
                }

                ddlKhuVuc.Enabled = this.DoiTacId == DoiTacInfo.DoiTacIdValue.VNP;
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
            for (int i = 0; i < cblDoiTac.Items.Count; i++)
            {
                if (cblDoiTac.Items[i].Selected)
                {
                    listDoiTac = string.Format("{0}{1},", listDoiTac, cblDoiTac.Items[i].Value);
                    isSelected = true;
                }
            }
            if (listDoiTac.Length > 0)
            {
                listDoiTac = listDoiTac.TrimEnd(',');
            }

            string loaibc = rblLoaiBaoCao.SelectedItem.Value;

            string errorMessage = string.Empty;
            string script = string.Empty;

            string page = string.Empty;
            string title = "Danh sách  khiếu nại quá hạn";
            page = "danhsachkhieunai.aspx?";

            if (!isSelected)
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
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}listDoiTac={2}&toDate={3}&toTime={4}&loaibc={5}&fromPage=danhsachkhieunaiquahan\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, listDoiTac, txtToDate.Text, txtToTime.Text, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        #endregion

        #region Private methods

        private void LoadKhuVuc()
        {            
            ListItem item = new ListItem("VNP", "1");
            ddlKhuVuc.Items.Add(item);

            item = new ListItem("VNP 1", DoiTacInfo.DoiTacIdValue.VNP1.ToString());
            ddlKhuVuc.Items.Add(item);
            item = new ListItem("VNP 2", DoiTacInfo.DoiTacIdValue.VNP2.ToString());
            ddlKhuVuc.Items.Add(item);
            item = new ListItem("VNP 3", DoiTacInfo.DoiTacIdValue.VNP3.ToString());
            ddlKhuVuc.Items.Add(item);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Load combo đối tác
        /// </summary>
        private void LoadDoiTac(int khuVucId)
        {
            string whereClause = string.Format("Id={0} OR DonViTrucThuoc={0}",khuVucId);
            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", whereClause, "DoiTacType ASC, TenDoiTac ASC");
            if(khuVucId == 1)
            {
                if(listDoiTac != null && listDoiTac.Count > 0)
                {
                    List<int> listDoiTacId = new List<int>();
                    listDoiTacId.Add(DoiTacInfo.DoiTacIdValue.VNP1);
                    listDoiTacId.Add(DoiTacInfo.DoiTacIdValue.VNP2);
                    listDoiTacId.Add(DoiTacInfo.DoiTacIdValue.VNP3);
                    listDoiTacId.Add(10034);
                    listDoiTacId.Add(10062);
                    listDoiTacId.Add(10076);

                    for(int i=0;i<listDoiTac.Count;i++)
                    {
                        if(listDoiTacId.Contains(listDoiTac[i].Id))
                        {
                            listDoiTac.RemoveAt(i);
                            i--;
                        }
                    }
                } // end if(listDoiTac != null && listDoiTac.Count > 0)                
            } // if(khuVucId == 1)

            cblDoiTac.DataSource = listDoiTac;
            cblDoiTac.DataTextField = "TenDoiTac";
            cblDoiTac.DataValueField = "Id";
            cblDoiTac.DataBind();            
        }

        #endregion

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/07/2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDoiTac(Convert.ToInt32(ddlKhuVuc.SelectedValue));
        }
    }
}