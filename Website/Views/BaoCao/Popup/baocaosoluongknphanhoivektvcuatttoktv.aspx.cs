using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Website.AppCode.Controller;
using AIVietNam.Core;
using AIVietNam.Admin;
using System.IO;

namespace Website.Views.BaoCao.Popup
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 05/12/2013
    /// Todo : Hiển thị báo cáo số lượng khiếu nại phản hồi về khai thác viên
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public partial class baocaosoluongknphanhoivektvcuatttoktv : AppCode.PageBase
    {
               
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime nullDateTime = new DateTime(1900, 1, 1);
                int phongBanXuLyId = ConvertUtility.ToInt32(Request.QueryString["donViID"]);
                DateTime fromDate = ConvertUtility.ToDateTime(Request.QueryString["fromDate"], "dd/MM/yyyy", nullDateTime);
                DateTime toDate = ConvertUtility.ToDateTime(Request.QueryString["toDate"], "dd/MM/yyy", nullDateTime);
                var loaibc = Request.QueryString["loaibc"];

                lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));

                //sNoiDungBaoCao = new BuildBaoCao().GetSoLuongKNPhanHoiVeKTVCuaTTToKTV(phongBanXuLyId, fromDate, toDate);

                if (loaibc == "excel")
                {
                    export2excel("BaoCaoSoLuongKNPhanHoiVeKTV" + fromDate + "_" + toDate);

                    //string script = string.Format("<script type='text/javascript'> this.close();</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
                }
            }
        }

        public void export2excel(string tenbc)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            string pathCSS = Server.MapPath("~/CSS");
            pathCSS += @"\BaoCao.css";
            StreamReader reader = new StreamReader(pathCSS);
            //reader.ReadToEnd();

            Response.Write("<style>");
            Response.Write(reader.ReadToEnd());
            Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
    }
}