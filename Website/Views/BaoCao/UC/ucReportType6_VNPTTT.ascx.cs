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
    public partial class ucReportType6_VNPTTT : System.Web.UI.UserControl
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
                // Trường hợp ĐKT sử dụng báo cáo này thì phải gán lại giá trị DoiTacId theo giá trị Id của VNP khu vực => mới ra được các VNPT trực thuộc khu vực
                switch(this.DoiTacId)
                {
                    case 7:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP1;
                        break;
                    case 14:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP2;
                        break;
                    case 19:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP3;
                        break;
                }

                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                LoadComboDoiTac(this.DoiTacId);
            }
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            string doiTacId = ddlVNPT.Visible ? ddlVNPT.SelectedValue : "-1";
            string tenDoiTac = ddlVNPT.SelectedItem != null ? ddlVNPT.SelectedItem.Text : string.Empty;
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
            if (lblReportType.Text == "bc_CSKHKV_BaoCaoTongHopGiamTruVNPTTT" || lblReportType.Text == "bc_VNP_BaoCaoTongHopGiamTruVNPTTT")
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
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}donViId={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\">');</script>", lblTittle.Text, page, donViId, donVi, fromDate, toDate, loaibc);                      
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page=baocaotonghopgiamtruvnpttt&doiTacId={2}&tenDoiTac={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, doiTacId, tenDoiTac, fromDate, toDate, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);            
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/12/2013
        /// </summary>
        /// <param name="doiTacId"></param>
        private void LoadComboDoiTac(int doiTacId)
        {
            List<DoiTacInfo> listDoiTac = null;

            if(doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
            {
                Dictionary<int, string> listKhuVuc = new Dictionary<int,string>();
                listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP1, "VNPT Khu vực 1");
                listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP2, "VNPT Khu vực 2");
                listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP3, "VNPT Khu vực 3");

                listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4", "TenDoiTac ASC");
                if(listDoiTac != null)
                {
                    ListItem item = null;
                    foreach(KeyValuePair<int, string> itemKhuVuc in listKhuVuc)
                    {
                        item = new ListItem(itemKhuVuc.Value, itemKhuVuc.Key.ToString());
                        ddlVNPT.Items.Add(item);

                        for(int i=0;i<listDoiTac.Count;i++)
                        {
                            if(listDoiTac[i].DonViTrucThuoc == itemKhuVuc.Key)
                            {
                                item = new ListItem("\xA0\xA0\xA0\xA0\xA0" + listDoiTac[i].TenDoiTac, listDoiTac[i].Id.ToString());
                                ddlVNPT.Items.Add(item);
                                listDoiTac.RemoveAt(i);
                                i--;
                            }
                        }
                    } // end foreach(KeyValuePair<int, string> itemKhuVuc in listKhuVuc)

                    item = new ListItem("Tất cả", DoiTacInfo.DoiTacIdValue.VNP.ToString());
                    ddlVNPT.Items.Insert(0, item);
                }
            }
            else
            {
                listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND DonViTrucThuoc=" + doiTacId, "TenDoiTac ASC");
                ddlVNPT.DataSource = listDoiTac;
                ddlVNPT.DataBind();

                ListItem item = new ListItem("VNPT khu vực", doiTacId.ToString());
                ddlVNPT.Items.Insert(0, item);
            }            
        }

        #endregion
    }
}