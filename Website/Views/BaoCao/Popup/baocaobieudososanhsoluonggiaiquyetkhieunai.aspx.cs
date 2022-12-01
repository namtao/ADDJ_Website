using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using System.Globalization;

using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;
using AIVietNam.GQKN.Entity;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaobieudososanhsoluonggiaiquyetkhieunai : AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            if (!IsPostBack)
            {
                try
                {
                    int doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    byte reportType = ConvertUtility.ToByte(Request.QueryString["reportType"], 0);
                    int loaiThueBao = ConvertUtility.ToInt32(Request.QueryString["loaiThueBao"]);
                    DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    string sLoaiKhieuNai = Request.QueryString["loaiKhieuNaiId"];
                    string sLinhVucChung = Request.QueryString["linhVucChungId"];
                    string sLinhVucCon = Request.QueryString["linhVucConId"];

                    string listDate = Request.QueryString["listDate"] != null ? Request.QueryString["listDate"] : string.Empty;
                                                
                    if(doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
                    {                        
                        chartSoLuongKhieuNaiDaDong.Titles[1].Text = "Khu vực : VNP";

                    }
                    else if(doiTacId == DoiTacInfo.DoiTacIdValue.VNP1)
                    {                        
                        chartSoLuongKhieuNaiDaDong.Titles[1].Text = "Khu vực : VNP1";
                    }
                    else if(doiTacId == DoiTacInfo.DoiTacIdValue.VNP2)
                    {                       
                        chartSoLuongKhieuNaiDaDong.Titles[1].Text = "Khu vực : VNP2";
                    }
                    else if(doiTacId == DoiTacInfo.DoiTacIdValue.VNP3)
                    {                        
                        chartSoLuongKhieuNaiDaDong.Titles[1].Text = "Khu vực : VNP3";
                    }

                    switch(loaiThueBao)
                    {
                        case 0:                            
                            chartSoLuongKhieuNaiDaDong.Titles[2].Text = "Loại thuê bao : Trả trước";
                            break;
                        case 1:                           
                            chartSoLuongKhieuNaiDaDong.Titles[2].Text = "Loại thuê bao : Trả sau";
                            break;
                        default:                           
                            chartSoLuongKhieuNaiDaDong.Titles[2].Text = "Loại thuê bao : Trả trước & Trả sau";
                            break;
                    }
                   
                    
                    //var loaibc = Request.QueryString["loaibc"];                    

                    DataTable dtReport = null;

                    if(listDate.Length == 0)
                    {
                        chartSoLuongKhieuNaiDaDong.Titles[3].Text = string.Format("Thời gian : {0} - {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));

                        dtReport = new BuildBaoCao().BaoCaoBieuDoSoLuongGQKNTheoMotKhoangThoiGian(reportType, sLoaiKhieuNai, sLinhVucChung, sLinhVucCon, doiTacId, loaiThueBao, fromDate, toDate);
                        if (dtReport != null)
                        {
                            //string[] arrDate = sDate.Split(',');

                            chartSoLuongKhieuNaiDaDong.ChartAreas[0].AxisX.Interval = 1;
                            chartSoLuongKhieuNaiDaDong.Series.Add("SerieSoLuongKNDaDong");
                            foreach (DataRow row in dtReport.Rows)
                            {
                                chartSoLuongKhieuNaiDaDong.Series[0].Points.AddXY(row["Name"].ToString(), row["SoLuongKNDaDong"].ToString());
                                chartSoLuongKhieuNaiDaDong.Series[0].Points[chartSoLuongKhieuNaiDaDong.Series[0].Points.Count - 1].IsValueShownAsLabel = true;
                            }

                        } // end if (dtReport != null)   
                    }
                    else
                    {
                        string[] arrDate = listDate.Split(',');
                        chartSoLuongKhieuNaiDaDong.Titles[3].Text = string.Format("Thời gian : {0} & {1}", arrDate[0], arrDate[1]);

                        dtReport = new BuildBaoCao().BaoCaoBieuDoSoLuongGQKNTheoHaiKhoangThoiGian(reportType, sLoaiKhieuNai, sLinhVucChung, sLinhVucCon, doiTacId, loaiThueBao, listDate);
                        if (dtReport != null)
                        {
                            
                            chartSoLuongKhieuNaiDaDong.ChartAreas[0].AxisX.Interval = 1;
                            //chartSoLuongKhieuNaiDaDong.Series.Add("SerieSoLuongKNDaDong");

                            for (int i = 0; i < arrDate.Length; i++)
                            {
                                chartSoLuongKhieuNaiDaDong.Series.Add(arrDate[i]);
                                chartSoLuongKhieuNaiDaDong.Legends.Add(arrDate[i]);

                                foreach (DataRow row in dtReport.Rows)
                                {
                                    chartSoLuongKhieuNaiDaDong.Series[arrDate[i]].Points.AddXY(row["Name"].ToString(), row[arrDate[i]].ToString());
                                    chartSoLuongKhieuNaiDaDong.Series[arrDate[i]].Points[chartSoLuongKhieuNaiDaDong.Series[arrDate[i]].Points.Count - 1].IsValueShownAsLabel = true;
                                }
                            }

                        } // end if (dtReport != null)  
                    }
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }
        }
    }
}