using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.ChiTietKhieuNai
{
    public partial class PhieuXacMinh2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["MaKN"] != null && !Request.QueryString["MaKN"].Equals(""))
            {
                //LoadKN();
            }
        }

        //private void LoadKN()
        //{
        //    try
        //    {
        //        int KhieuNaiId = Convert.ToInt32(Request.QueryString["MaKN"]);

        //        var KhieuNaiItem = ServiceFactory.GetInstanceKhieuNai().GetInfo(KhieuNaiId);

        //        var DoiTacItem = DoiTacImpl.ListDoiTac.Where(t => t.Id == KhieuNaiItem.DoiTacId);
        //        if (DoiTacItem != null && DoiTacItem.Any())
        //        {
        //            var itemDoiTac = DoiTacItem.Single();

        //            ltDonViTiepNhan.Text = itemDoiTac.TenDoiTac;
        //          //  ltTel.Text = itemDoiTac.DienThoai;
        //         //   ltFax.Text = itemDoiTac.Fax;

        //        }

        //        ltNgay.Text = DateTime.Now.Day.ToString();
        //        ltThang.Text = DateTime.Now.Month.ToString();
        //        ltNam.Text = DateTime.Now.Year.ToString();


        //        ltTenKH.Text = KhieuNaiItem.HoTenLienHe;
        //        //ltSTB.Text = KhieuNaiItem.SoThueBao.ToString();
        //        //ltSDTKN.Text = KhieuNaiItem.SoThueBao.ToString();
        //        //if (KhieuNaiItem.IsTraSau)
        //        //    ltHTTB.Text = "Trả sau";
        //        //else
        //        //    ltHTTB.Text = "Trả trước";

        //        //if (string.IsNullOrEmpty(KhieuNaiItem.MaTinh))
        //        //    ltTinh.Text = "...............";
        //        //else
        //        //    ltTinh.Text = KhieuNaiItem.MaTinh;

        //        //if (string.IsNullOrEmpty(KhieuNaiItem.MaQuan))
        //        //    ltQuan.Text = "...............";
        //        //else
        //        //    ltQuan.Text = KhieuNaiItem.MaQuan;

        //        //ltNoiDung.Text = KhieuNaiItem.NoiDungPA;
        //        //ltNgayTiepNhan.Text = KhieuNaiItem.NgayTiepNhan.ToString("dd/MM/yyyy");
        //    }
        //    catch { }
        //}
    }
}