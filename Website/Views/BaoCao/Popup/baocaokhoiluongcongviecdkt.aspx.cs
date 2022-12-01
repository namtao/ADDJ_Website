using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Website.AppCode.Controller;
using AIVietNam.Core;
using System.IO;
using System.Globalization;
using AIVietNam.Admin;
using System.Text;

namespace Website.Views.BaoCao.Popup
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 24/11/2013
    /// Todo : Hiển thị báo cáo khối lượng công việc của các thành viên
    /// </summary>
    public partial class baocaokhoiluongcongviecdkt : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int khuVucId = ConvertUtility.ToInt32(Request.QueryString["khuVucID"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    int phongBanXuLyId = ConvertUtility.ToInt32(Request.QueryString["donViId"]);

                    lblTuNgay.Text = Request.QueryString["fromDate"];
                    lblDenNgay.Text = Request.QueryString["toDate"];
                    var loaibc = Request.QueryString["loaibc"];                                      

                    //int phongBanXuLyId = LoginAdmin.AdminLogin().PhongBanId;
                    string where = string.Empty;

                    if (khuVucId == 7)
                    {
                        where = "Hà Nội";
                        lblKhuVuc.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV I";
                    }
                    else if (khuVucId == 14)
                    {
                        where = "TP Hồ Chí Minh";
                        lblKhuVuc.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV II";
                    }
                    else if (khuVucId == 19)
                    {
                        where = "Đà Nẵng";
                        lblKhuVuc.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV III";
                    }
                    else
                    {
                        where = "Hà Nội";
                    }


                    lblWhereWhen.Text = string.Format("{0}, ngày {1} tháng {2} năm {3}", where, DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year);
                    lblWho.Text = LoginAdmin.AdminLogin().FullName;

                    //lblKhuVuc.Text = Request.QueryString["khuVuc"] != null ? Request.QueryString["khuVuc"].Trim().ToUpper() : string.Empty;
                    //lblPhongBan.Text = Request.QueryString["donVi"] != null && !Request.QueryString["donVi"].ToLower().Contains("chọn phòng ban") ? Request.QueryString["donVi"].ToUpper() : string.Empty;

                    BuildBaoCao buildBaoCao = new BuildBaoCao();
                    DataTable dtReport = buildBaoCao.BaoCaoKhoiLuongCongViecToXLNV(phongBanXuLyId, fromDate, toDate, null, null, null);
                    ShowReportContent(dtReport, fromDate, toDate);
                    
                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoKhoiLuongCongViec_" + fromDate + "_" + toDate);
                        string script = string.Format("<script type='text/javascript'> window.close();</scirpt>");    
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closeWindow", script, false);
                    }                   
                }
                catch (Exception ex)
                {
                    sNoiDungBaoCao = @"<tr>
                                        <td colspan='6'>Có lỗi trong quá trình xuất dữ liệu</td>
                                      </tr>";
                    Utility.LogEvent(ex);
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// </summary>
        /// <param name="dtReport"></param>
        private void ShowReportContent(DataTable dtReport, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            if (Session["NguoiDungToXLNV"] != null)
            {
                DataTable dtNguoiDung = (DataTable)Session["NguoiDungToXLNV"];
                DataView dvNguoiDung = dtNguoiDung.DefaultView;
                dvNguoiDung.Sort = "SoTT ASC";

                dtNguoiDung = dvNguoiDung.Table;

                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    int index = 1;
                    int totalPAKNDaGiaiQuyet = 0;
                    foreach (DataRow rowNguoiDung in dtNguoiDung.Rows)
                    {
                        foreach (DataRow rowReport in dtReport.Rows)
                        {
                            if (rowNguoiDung["TenTruyCap"].ToString() == rowReport["TenTruyCap"].ToString())
                            {
                                sb.Append("<tr>");
                                sb.Append("<td style=\"text-align:center\">" + index.ToString() + "</td>");
                                sb.Append("<td>" + rowReport["TenNguoiSuDung"].ToString() + "</td>");
                                sb.AppendFormat("<td style=\"text-align:center\"><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/danhsachkhieunai.aspx?fromPage=baocaokhoiluongcongviecdkt&tenTruyCap={0}&fromDate={1}&toDate={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{3}</a></td>", rowNguoiDung["TenTruyCap"].ToString(), fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"), rowReport["SLPAKNDaGiaiQuyet"].ToString());
                                sb.Append("<td></td>");
                                sb.Append("<td></td>");
                                sb.Append("<td></td>");
                                sb.Append("</tr>");

                                totalPAKNDaGiaiQuyet += ConvertUtility.ToInt32(rowReport["SLPAKNDaGiaiQuyet"], 0);
                                index++;

                                break;
                            } // end if(rowNguoiDung["TenTruyCap"].ToString() == rowReport["TenTruyCap"].ToString())
                        } // end foreach(DataRow rowReport in dtReport.Rows)
                    } // end foreach(DataRow rowNguoiDung in dtNguoiDung.Rows)

                    sb.Append("<tr style=\"font-weight:bold;text-align:center\">");
                    sb.Append("<td>&nbsp;</td>");
                    sb.Append("<td>TỔNG</td>");
                    sb.Append("<td>" + totalPAKNDaGiaiQuyet.ToString("###,###,###") + "</td>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");

                    sNoiDungBaoCao = sb.ToString();

                    lblTongSoNhanVien.Text = dtNguoiDung.Rows.Count.ToString();
                    lblTongSoPAKNXuLy.Text = totalPAKNDaGiaiQuyet.ToString("###,###,###");
                } // end if(dtReport != null && dtReport.Rows.Count > 0)                
            }

            //if (dtReport != null)
            //{
            //    //StringBuilder sb = new StringBuilder();
            //    int index = 1;
            //    int totalPAKNDaGiaiQuyet = 0;
            //    foreach (DataRow row in dtReport.Rows)
            //    {
            //        sb.Append("<tr>");
            //        sb.Append("<td style=\"text-align:center\">" + index.ToString() + "</td>");
            //        sb.Append("<td>" + row["TenNguoiSuDung"].ToString() + "</td>");
            //        sb.Append("<td style=\"text-align:center\">" + row["SLPAKNDaGiaiQuyet"].ToString() + "</td>");
            //        sb.Append("<td></td>");
            //        sb.Append("<td></td>");
            //        sb.Append("<td></td>");
            //        sb.Append("</tr>");

            //        totalPAKNDaGiaiQuyet += ConvertUtility.ToInt32(row["SLPAKNDaGiaiQuyet"], 0);
            //        index++;
            //    }

            //    sb.Append("<tr style=\"font-weight:bold;text-align:center\">");
            //    sb.Append("<td>&nbsp;</td>");
            //    sb.Append("<td>TỔNG</td>");
            //    sb.Append("<td>" + totalPAKNDaGiaiQuyet.ToString("###,###,###") + "</td>");
            //    sb.Append("<td></td>");
            //    sb.Append("<td></td>");
            //    sb.Append("<td></td>");
            //    sb.Append("</tr>");

            //    sNoiDungBaoCao = sb.ToString();

            //    lblTongSoNhanVien.Text = dtReport.Rows.Count.ToString();
            //    lblTongSoPAKNXuLy.Text = totalPAKNDaGiaiQuyet.ToString("###,###,###");
            //}
            else
            {
                sNoiDungBaoCao = @"<tr>
                                    <td colspan='6'>Không có dữ liệu</td>
                                  </tr>";
            }
        }

        public void export2excel(string tenbc)
        {
            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            //Response.Clear();
            //Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            //Response.Charset = "";
            //this.EnableViewState = false;
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>" + stringWrite.ToString() + "</table></body></html>");
            //Response.End();
            ////HttpContext.Current.ApplicationInstance.CompleteRequest();

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
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            
        }
    }
}