using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using System.Data;

using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;
using System.Web.UI.DataVisualization.Charting;


namespace Website.Views.BaoCao.Popup
{
    public partial class baocaobieudosoluongkhieunaitiepnhan : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            //var userLogin = LoginAdmin.AdminLogin();
            //fullName = userLogin.FullName;
            if (!IsPostBack)
            {
                //lblCurDate.Text = string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                //lblFullName.Text = fullName;
                try
                {
                    int doiTacTiepNhanId = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);
                    int phongBanTiepNhanId = ConvertUtility.ToInt32(Request.QueryString["donViId"]);
                    byte reportType = ConvertUtility.ToByte(Request.QueryString["reportType"], 0);
                    string sLoaiKhieuNai = Request.QueryString["loaiKhieuNaiId"];
                    string sLinhVucChung = Request.QueryString["linhVucChungId"];
                    string sLinhVucCon = Request.QueryString["linhVucConId"];
                    string sDate = Request.QueryString["listDate"];
                    //var loaibc = Request.QueryString["loaibc"];                    

                    DataTable dtReport = new BuildBaoCao().BaoCaoBieuDoSoLuongKhieuNai(doiTacTiepNhanId, phongBanTiepNhanId, reportType, sLoaiKhieuNai, sLinhVucChung, sLinhVucCon, sDate);
                    if (dtReport != null)
                    {                        
                        string[] arrDate = sDate.Split(',');

                        chartSoLuongKhieuNaiTiepNhan.ChartAreas[0].AxisX.Interval = 1;

                        for (int i = 0; i < arrDate.Length; i++)
                        {
                            chartSoLuongKhieuNaiTiepNhan.Series.Add(arrDate[i]);
                            chartSoLuongKhieuNaiTiepNhan.Legends.Add(arrDate[i]);
                            
                            foreach (DataRow row in dtReport.Rows)
                            {
                                chartSoLuongKhieuNaiTiepNhan.Series[arrDate[i]].Points.AddXY(row["Name"].ToString(), row[arrDate[i]].ToString());
                                
                                //DataPoint justAddedPoint = chartSoLuongKhieuNaiTiepNhan.Series[arrDate[i]].Points[chartSoLuongKhieuNaiTiepNhan.Series[arrDate[i]].Points.Count - 1];
                                //justAddedPoint.Url = String.Format("~/Views/QLKhieuNai/QuanLyKhieuNai.aspx?LinhVucConId={0}&KhoangThoiGian={1}", row["LoaiKhieuNaiId"], arrDate[i]);
                            }                            
                        }
                       
                    } // end if (dtReport != null)                    
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }
        }      
    }
}