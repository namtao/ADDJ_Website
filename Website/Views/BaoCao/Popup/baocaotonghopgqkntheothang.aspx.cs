using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghopgqkntheothang : Page
    {
        protected string sNoiDungBaoCao = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 1, 1);
            DateTime fromDate = ConvertUtility.ToDateTime(Request.QueryString["fromDate"], "dd/MM/yyyy", nullDateTime);
            DateTime toDate = ConvertUtility.ToDateTime(Request.QueryString["toDate"], "dd/MM/yyyy", nullDateTime);
            
            var loaibc = Request.QueryString["loaibc"];
            var doitacId = ConvertUtility.ToInt32(Request.QueryString["doitacId"]);
            var phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
            lblYear.Text = fromDate.Year.ToString();
            lblMonth.Text = fromDate.Month.ToString();
            lblThangVaNam.Text = fromDate.Month.ToString(CultureInfo.InvariantCulture) + "/" + fromDate.Year.ToString(CultureInfo.InvariantCulture);
            lblNamLuyKe.Text = fromDate.Year.ToString(CultureInfo.InvariantCulture);

            bool isExportExcel = false;
            sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopPAKNTheoPhongBanVNPTTT(doitacId, phongBanId,  ConvertUtility.ToInt32(fromDate), ConvertUtility.ToInt32(toDate), isExportExcel);
        }
    }
}