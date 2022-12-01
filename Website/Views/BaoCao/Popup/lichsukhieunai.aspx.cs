using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using AIVietNam.Core;
using Website.AppCode.Controller;
using AIVietNam.GQKN.Entity;
using Website.AppCode;

namespace Website.Views.BaoCao.Popup
{
    public partial class lichsukhieunai : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int khieuNaiId = ConvertUtility.ToInt32(Request.QueryString["khieuNaiId"]);
                    string soThueBao = Request.QueryString["soThueBao"] != null ? Request.QueryString["soThueBao"] : "";
                    DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));

                    lblFromDateToDate.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));
                    lblKhieuNai.Text = string.Format("Mã khiếu nại : {0} <br/>Số thuê bao : {1}", khieuNaiId, soThueBao);
                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoLichSuKhieuNai(khieuNaiId, fromDate, toDate);
                }
                catch(Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }
        }
    }
}