using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.GQKN.Impl;
using Website.AppCode.Controller;
using System.Data;
using AIVietNam.Admin;
using System.Web.UI.DataVisualization.Charting;
using AIVietNam.Core;
using System.Data.SqlClient;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class BieuDoKNChoXuLy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AdminInfo userInfo = LoginAdmin.AdminLogin();
                string loaiKhieuNaiId = Request.QueryString["LoaiKhieunaiId"];
                string linhVucChungId = Request.QueryString["LinhVucChungId"];
                string listLinhVucConId = "0";

                if (linhVucChungId.Length > 0 && ConvertUtility.ToInt32(linhVucChungId) > 0)
                {
                    for (int i = 0; i < LoaiKhieuNaiImpl.ListLoaiKhieuNai.Count; i++)
                    {
                        if (linhVucChungId == LoaiKhieuNaiImpl.ListLoaiKhieuNai[i].ParentId.ToString())
                        {
                            listLinhVucConId = string.Format("{0},{1}", listLinhVucConId, LoaiKhieuNaiImpl.ListLoaiKhieuNai[i].Id);
                        }
                    }
                }
                  
                BuildBaoCao buildBaoCao = new BuildBaoCao();
                DataTable dtReport = buildBaoCao.BaoCaoBieuDoSoLuongKhieuNaiChoXuLy(3, userInfo.PhongBanId, loaiKhieuNaiId, linhVucChungId, listLinhVucConId);
                if (dtReport != null)
                {                    
                    chartSoLuongKhieuNaiTiepNhan.ChartAreas[0].AxisX.Interval = 1;

                    foreach (DataRow row in dtReport.Rows)
                    {
                        if (ConvertUtility.ToInt32(row["SoLuongKNChoXuLy"], 0) > 0)
                        {
                            chartSoLuongKhieuNaiTiepNhan.Series[0].Points.AddXY(row["Name"].ToString(), row["SoLuongKNChoXuLy"].ToString());

                            string tenLinhVucCon = row["LoaiKhieuNaiId"].ToString() == "0" ? row["LoaiKhieuNaiId"].ToString() : row["Name"].ToString();
                            DataPoint justAddedPoint = chartSoLuongKhieuNaiTiepNhan.Series[0].Points[chartSoLuongKhieuNaiTiepNhan.Series[0].Points.Count - 1];
                            justAddedPoint.Url = String.Format("~/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab6-KNTongHopChoXuLy&TypeSearch=2&LoaiKhieuNaiId={0}&LinhVucChungId={1}&LinhVucConId={2}", loaiKhieuNaiId, linhVucChungId, tenLinhVucCon);
                        }
                    }                   
                } // end if (dtReport != null)

            }
        }
    }
}