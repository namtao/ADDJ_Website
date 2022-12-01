using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using System.Data;
using Website.AppCode.Controller;
using AIVietNam.Core;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace Website.Views.Dashboards
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 
    /// </summary>
    public partial class BieuDoKNCuaPhongBanTheoThoiGian : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Config.IsCallSolr)
            {
                try
                {
                    DateTime toDate = DateTime.Now.AddDays(1);
                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day);
                    DateTime fromDate = toDate.Subtract(new TimeSpan(7, 0, 0, 0));
                    AdminInfo userInfo = LoginAdmin.AdminLogin();
                    DataSet dsKhieuNai = new BuildBaoCao().BieuDoSoLuongKhieuNaiCuaPhongBanTheoThoiGian(userInfo.PhongBanId, fromDate, toDate);

                    if (dsKhieuNai != null)
                    {
                        chartKhieuNai.Legends.Add("1");
                        chartKhieuNai.Legends.Add("2");

                        DateTime nullDateTime = new DateTime(1900, 01, 01);
                        DataTable dtKhieuNaiTiepNhan = dsKhieuNai.Tables[0];
                        DataTable dtKhieuNaiDong = dsKhieuNai.Tables[1];

                        bool hasData = false;
                        foreach (DataRow row in dtKhieuNaiTiepNhan.Rows)
                        {
                            if (ConvertUtility.ToInt32(row["SLTiepNhan"], 0) > 0)
                            {
                                hasData = true;
                                break;
                            }
                        }

                        if (!hasData)
                        {
                            foreach (DataRow row in dtKhieuNaiDong.Rows)
                            {
                                if (ConvertUtility.ToInt32(row["SLKhieuNaiDong"], 0) > 0)
                                {
                                    hasData = true;
                                    break;
                                }
                            }
                        }

                        chartKhieuNai.Visible = hasData;
                        lblMessage.Visible = !hasData;

                        if (hasData)
                        {
                            int index = 0;
                            foreach (DataRow row in dtKhieuNaiTiepNhan.Rows)
                            {
                                chartKhieuNai.Series[0].Points.AddXY(ConvertUtility.ToDateTime(row["Date"], nullDateTime).ToString("dd/MM/yyyy"), row["SLTiepNhan"].ToString());
                                chartKhieuNai.Series[0].Points[index].MapAreaAttributes = "onclick=\"javascript:window.open(url='" + String.Format("/Views/BaoCao/Popup/danhsachkhieunai.aspx?fromPage=dashboard&phongBanId={0}&fromDate={1}&toDate={2}&reportType=11", userInfo.PhongBanId, ConvertUtility.ToDateTime(row["Date"], nullDateTime).ToString("dd/MM/yyyy"), ConvertUtility.ToDateTime(row["Date"], nullDateTime).ToString("dd/MM/yyyy")) + "');\"";

                                chartKhieuNai.Series[0].Points[index].ToolTip = row["SLTiepNhan"].ToString();

                                index++;
                            }
                            index = 0;
                            foreach (DataRow row in dtKhieuNaiDong.Rows)
                            {
                                chartKhieuNai.Series[1].Points.AddXY(ConvertUtility.ToDateTime(row["Date"], nullDateTime).ToString("dd/MM/yyyy"), row["SLKhieuNaiDong"].ToString());
                                chartKhieuNai.Series[1].Points[index].MapAreaAttributes = "onclick=\"javascript:window.open(url='" + String.Format("/Views/BaoCao/Popup/danhsachkhieunai.aspx?fromPage=dashboard&phongBanId={0}&fromDate={1}&toDate={2}&reportType=21", userInfo.PhongBanId, ConvertUtility.ToDateTime(row["Date"], nullDateTime).ToString("dd/MM/yyyy"), ConvertUtility.ToDateTime(row["Date"], nullDateTime).ToString("dd/MM/yyyy")) + "');\"";
                                chartKhieuNai.Series[1].Points[index].ToolTip = row["SLKhieuNaiDong"].ToString();

                                index++;
                            }
                        }
                    }
                    // end if (dtReport != null)
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }
        }
    }
}