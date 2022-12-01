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
    public partial class ucReportType7_VNPTTT : System.Web.UI.UserControl
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
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string phongBanId = ddlPhongBan.SelectedValue;

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

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {                
                string path = string.Format("/Views/BaoCao/Popup/baocaotonghoppakntheonguoidungvnpttt.aspx?doitacId={0}&fromDate={1}&toDate={2}&loaibc={3}&phongBanId={4}", lblDoiTacId.Text, fromDate, toDate, loaibc, phongBanId);
                script = string.Format("<script type='text/javascript'> window.open(url=\"{0}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", path);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 10/09/2014
        /// </summary>
        /// <param name="doiTacId"></param>
        private void LoadComboPhongBan(int doiTacId)
        {
            string space = "\xA0\xA0\xA0\xA0\xA0";

            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfDoiTacId(doiTacId);
            if(listPhongBan != null)
            {
                for (int i = 0; i < listPhongBan.Count;i++ )
                {
                    if(listPhongBan[i].Cap == 2)
                    {
                        listPhongBan[i].Name = string.Format("{0}{1}", space, listPhongBan[i].Name);
                    }
                    else if(listPhongBan[i].Cap == 3)
                    {
                        listPhongBan[i].Name = string.Format("{0}{0}{1}", space, listPhongBan[i].Name);
                    }
                }
                    
                ddlPhongBan.DataSource = listPhongBan;
                ddlPhongBan.DataBind();

                ListItem item = new ListItem("Tất cả", "-1");
                ddlPhongBan.Items.Insert(0, item);
            }

            //List<DoiTacInfo> listDoiTac = null;

            //if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
            //{
            //    Dictionary<int, string> listKhuVuc = new Dictionary<int, string>();
            //    listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP1, "VNPT Khu vực 1");
            //    listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP2, "VNPT Khu vực 2");
            //    listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP3, "VNPT Khu vực 3");

            //    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4", "TenDoiTac ASC");
            //    if (listDoiTac != null)
            //    {
            //        ListItem item = null;
            //        foreach (KeyValuePair<int, string> itemKhuVuc in listKhuVuc)
            //        {
            //            item = new ListItem(itemKhuVuc.Value, itemKhuVuc.Key.ToString());
            //            ddlVNPT.Items.Add(item);

            //            for (int i = 0; i < listDoiTac.Count; i++)
            //            {
            //                if (listDoiTac[i].DonViTrucThuoc == itemKhuVuc.Key)
            //                {
            //                    item = new ListItem("\xA0\xA0\xA0\xA0\xA0" + listDoiTac[i].TenDoiTac, listDoiTac[i].Id.ToString());
            //                    ddlVNPT.Items.Add(item);
            //                    listDoiTac.RemoveAt(i);
            //                    i--;
            //                }
            //            }
            //        } // end foreach(KeyValuePair<int, string> itemKhuVuc in listKhuVuc)

            //        item = new ListItem("Tất cả", DoiTacInfo.DoiTacIdValue.VNP.ToString());
            //        ddlVNPT.Items.Insert(0, item);
            //    }
            //}
            //else
            //{
            //    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND DonViTrucThuoc=" + doiTacId, "TenDoiTac ASC");
            //    ddlVNPT.DataSource = listDoiTac;
            //    ddlVNPT.DataBind();

            //    ListItem item = new ListItem("VNPT khu vực", doiTacId.ToString());
            //    ddlVNPT.Items.Insert(0, item);
            //}
        }

        #endregion

    }
}