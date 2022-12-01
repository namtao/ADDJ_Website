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
    public partial class ucReportType15_VNPTTT : System.Web.UI.UserControl
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
                switch (this.DoiTacId)
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

                LoadComboDonVi(this.DoiTacId);
            }
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            string doiTacId = ddlDonVi.Visible ? ddlDonVi.SelectedValue : "-1";
            string tenDoiTac = ddlDonVi.SelectedItem != null ? ddlDonVi.SelectedItem.Text : string.Empty;
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
            if (lblReportType.Text == "bc_VNP_BaoCaoTongHopGiamTruDVGTGTDonVi")
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
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}page=bcth_giamtru_dvgtgt_donvi&doiTacId={2}&tenDoiTac={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTittle.Text, page, doiTacId, tenDoiTac, fromDate, toDate, loaibc);
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
        //private void LoadComboDonVi(int doiTacId)
        //{
        //    List<DoiTacInfo> listDoiTac = null;

        //    if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
        //    {
        //        Dictionary<int, string> listTypeDoiTac = new Dictionary<int, string>();
        //        listTypeDoiTac.Add(DoiTacInfo.DoiTacTypeValue.DKT_VNP, "Đài Khai Thác");
        //        listTypeDoiTac.Add(DoiTacInfo.DoiTacTypeValue.TTTC, "Trung Tâm Tính Cước");
        //        listTypeDoiTac.Add(DoiTacInfo.DoiTacTypeValue.VAS, "VAS");
        //        listTypeDoiTac.Add(DoiTacInfo.DoiTacTypeValue.VNPTTT, "VNPT Tỉnh Thành");
        //        listTypeDoiTac.Add(DoiTacInfo.DoiTacTypeValue.DOI_TAC_VNP, "Đối Tác VNP");
        //       // listTypeDoiTac.Add(DoiTacInfo.DoiTacTypeValue.TDD_VNP, "TDD");

        //        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
        //        if (listDoiTac != null)
        //        {
        //            ListItem item = null;
        //            foreach (KeyValuePair<int, string> itemtype in listTypeDoiTac)
        //            {
        //                item = new ListItem(itemtype.Value, itemtype.Key.ToString());
        //                ddlDonVi.Items.Add(item);

        //                for (int i = 0; i < listDoiTac.Count; i++)
        //                {
        //                    if (listDoiTac[i].DonViTrucThuoc == itemtype.Key)
        //                    {
        //                        item = new ListItem("\xA0\xA0\xA0\xA0\xA0" + listDoiTac[i].TenDoiTac, listDoiTac[i].Id.ToString());
        //                        ddlDonVi.Items.Add(item);
        //                        listDoiTac.RemoveAt(i);
        //                        i--;
        //                    }
        //                }
        //            }
        //            foreach (var doitac in listDoiTac)
        //            {
        //                ddlDonVi.Items.Add(new ListItem(doitac.TenDoiTac, doitac.Id.ToString()));
        //            }
        //            // end foreach(KeyValuePair<int, string> itemtype in listTypeDoiTac)

        //            item = new ListItem("Tất cả", DoiTacInfo.DoiTacIdValue.VNP.ToString());
        //            ddlDonVi.Items.Insert(0, item);
        //        }
        //    }
        //    else
        //    {
        //        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", " DonViTrucThuoc=" + doiTacId, "TenDoiTac ASC");
        //        ddlDonVi.DataSource = listDoiTac;
        //        ddlDonVi.DataBind();

        //        ListItem item = new ListItem("VNPT khu vực", doiTacId.ToString());
        //        ddlDonVi.Items.Insert(0, item);
        //    }
        //}
        private void LoadComboDonVi(int doiTacId)
        {
            ddlDonVi.Items.Clear();

            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
            List<DoiTacInfo> listDoiTacRoot = null;
            string space = string.Empty;

            if (listDoiTac != null)
            {
                if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
                {
                    listDoiTacRoot = listDoiTac.FindAll(delegate(DoiTacInfo obj) { return obj.DonViTrucThuoc == 0; });
                }
                else
                {
                    listDoiTacRoot = listDoiTac.FindAll(delegate(DoiTacInfo obj) { return obj.Id == doiTacId; });
                }

                for (int i = 0; i < listDoiTacRoot.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", space, listDoiTacRoot[i].TenDoiTac);
                    item.Value = listDoiTacRoot[i].Id.ToString();
                    ddlDonVi.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacRoot[i].Id;
                    listDoiTacRoot.RemoveAt(i);
                    i--;

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTac);
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Lấy ra các đối tác của đơn vị trực thuộc
        /// </summary>
        /// <param name="donViTrucThuoc"></param>
        /// <param name="listDoiTacInfo"></param>
        private void LoadChildDoiTac(string space, int donViTrucThuoc, List<DoiTacInfo> listDoiTacInfo)
        {
            space = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}", space);
            for (int i = 0; i < listDoiTacInfo.Count; i++)
            {
                //if (listDoiTacInfo[i].DonViTrucThuoc == donViTrucThuoc)
                if (listDoiTacInfo[i].DonViTrucThuocChoBaoCao == donViTrucThuoc)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", Server.HtmlDecode(space), listDoiTacInfo[i].TenDoiTac);
                    item.Value = listDoiTacInfo[i].Id.ToString(); ;
                    ddlDonVi.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacInfo[i].Id;

                    listDoiTacInfo.RemoveAt(i);
                    i = -1;

                    if (listDoiTacInfo.Count == 0)
                    {
                        break;
                    }

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTacInfo);
                }
            }

        }

        #endregion
    }
}