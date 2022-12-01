using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Website.AppCode;

namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType5_VNPTT : System.Web.UI.UserControl
    {
        public string ReportType { get; set; }

        public int DoiTacId { get; set; }

        public int PhongBanXuLyId { get; set; }

        public string ReportTitle { get; set; }

        public bool IsFirstLoad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
            if (IsFirstLoad)
            {
                lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblDoiTacId.Text = this.DoiTacId.ToString();
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();

                List<DoiTacInfo> listDoiTac = null;

                switch (this.DoiTacId)
                {
                    case 2: // VNP1
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP1 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    case 3: // VNP2
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP2 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    case 5: // VNP3
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP3 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    default:
                        listDoiTac = new List<DoiTacInfo>();
                        DoiTacInfo objDoiTac = ServiceFactory.GetInstanceDoiTac().GetInfo(ConvertUtility.ToInt32(lblDoiTacId.Text, 0));
                        listDoiTac.Add(objDoiTac);
                        ddlVNPTTT.Enabled = false;
                        rowVNPTTT.Visible = false;
                        break;
                }

                ddlVNPTTT.DataSource = listDoiTac;
                ddlVNPTTT.DataBind();
                if (listDoiTac != null && listDoiTac.Count > 0)
                {
                    ddlVNPTTT.Items[0].Selected = true;
                }
               
                    LoadComboPhongBan(ConvertUtility.ToInt32(this.DoiTacId));
                var listYears = new List<ListItem>();

                for (int i = 2000; i <= DateTime.Now.Year;i++ )
                {
                    listYears.Add(new ListItem(){Text = i.ToString(CultureInfo.InvariantCulture), Value = i.ToString(CultureInfo.InvariantCulture), Selected = i==DateTime.Now.Year});
                }
                    ddlYear.DataSource = listYears;
                    ddlYear.DataBind();
            }

        }
        protected void lbReport_Click(object sender, EventArgs e)
        {
            var month = ConvertUtility.ToInt32(this.ddlMonth.SelectedValue);
            var fromDate = new DateTime(ConvertUtility.ToInt32(ddlYear.SelectedValue), month, 1);
            var toDate = new DateTime(ConvertUtility.ToInt32(ddlYear.SelectedValue), month, DateTime.DaysInMonth(ConvertUtility.ToInt32(ddlYear.SelectedValue), month));
            string doiTacId = ddlVNPTTT.SelectedValue;
            string phongbanId = ddlPhongBan.SelectedValue;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string layDuLieuTheo1HoacNhieuPhongBan ="2";// rblLayDuLieuTheo1HoacNhieuPhongBan.SelectedItem.Value;
            var year = ddlYear.SelectedValue;
            string path = string.Empty;
            if(ConvertUtility.ToInt32(ddlPhongBan.SelectedValue) > -1)
                path = string.Format("/Views/BaoCao/Popup/baocaotonghopgqkntheothang.aspx?doiTacId={0}&phongbanId={1}&year={2}&fromDate={3}&toDate={4}&loaibc={5}&layDuLieuTheo1HoacNhieuPhongBan={6}", doiTacId, phongbanId,year, fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"), loaibc, layDuLieuTheo1HoacNhieuPhongBan);
            else
            {
                path = string.Format("/Views/BaoCao/Popup/baocaotonghopgqkntheothang.aspx?doiTacId={0}&year={1}&fromDate={2}&toDate={3}&loaibc={4}&layDuLieuTheo1HoacNhieuPhongBan={5}", doiTacId, year, fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"), loaibc, layDuLieuTheo1HoacNhieuPhongBan);
            }

            string script = string.Format("<script type='text/javascript'> window.open(url=\"{1}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTitle.Text, path);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        private void LoadComboPhongBan(int doiTacId)
        {
            //int phongBanId = ConvertUtility.ToInt32(lblPhongBanXuLyId.Text);
            List<PhongBanInfo> listPhongBan = null;

            //if (phongBanId <= 0)
            //{
                listPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("*", "DoiTacId=" + doiTacId, "Sort ASC");
            if(listPhongBan.Any())
            {
                foreach (var phongBanInfo in listPhongBan)
                {
                    phongBanInfo.Name = phongBanInfo.GetFormattedBreadCrumb();
                }
            }
            listPhongBan.Insert(0,new PhongBanInfo(){Id = -1, Name = "--Chọn phòng ban--"});
            //}
            //else
            //{
            //    listPhongBan = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfParentId(phongBanId);
            //}

            ddlPhongBan.DataSource = listPhongBan;
            ddlPhongBan.DataBind();
        }

        protected void ddlVNPTTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadComboPhongBan(ConvertUtility.ToInt32(ddlVNPTTT.SelectedValue));
        }

       
    }
}