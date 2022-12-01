using ADDJ.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro
{
    public partial class CacYeuCauHoTroCuaToi : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Response.Redirect("/");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hiddenIDHoTroChiTietXuLy["hiddenIDHoTroChiTietXuLy"] = 0;
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    hiddenIDHoTroChiTietXuLy["nguoixuly"] = loginInfo.Username;
                }
            }
        }

        protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            ASPxGridView1.DataBind();
        }

        protected void ASPxGridView1_PageIndexChanged(object sender, EventArgs e)
        {
            ASPxGridView1.DataBind();
        }

        protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<ViewThongTinTraCuu>;
            if (tbl == null)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        var strSQl = "select * from HT_DM_YEUCAU_HOTRO_HT where NGUOITAO='" + loginInfo.Username + "' order by ngaytao desc";

                        var strSQl1 = string.Format(@"SELECT
                        a.ID, 
                        f.TENDONVI, 
                        f.MADONVI,
                        a.SODIENTHOAI,
                        a.MA_YEUCAU,
                        a.NOIDUNG_YEUCAU,
                        a.NGAYTAO,
                        a.NGUOITAO,
                        g.TenDayDu TENDAYDU,
                        b.TENHETHONG,
                        b.MA_HETHONG,
                        c.LINHVUC,
                        c.MA_LINHVUC,
                        d.TENMUCDO,
                        e.TEN_LUONG,
                        kk.TENDONVI DONVIPHOIHOP
                        FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                        LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                        LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                        LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                        LEFT JOIN HT_NGUOIDUNG g ON g.Id = a.ID_NGUOITAO
                        CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                     INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                     WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                        WHERE a.NGUOITAO ='{0}' order by ngaytao desc", loginInfo.Username);
                        var lstHoTro = ctx.Database.SqlQuery<ViewThongTinTraCuu>(strSQl);
                        var lst = lstHoTro.ToList();
                        Session["tblHeThongXL"] = lst;
                        tbl = lst;
                    }
                }
            }
            ASPxGridView1.DataSource = tbl;
        }

        protected void ASPxGridView1_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            //if (e.VisibleIndex == -1) return;
            //var ishoatdong = ASPxGridView2.GetRowValues(e.VisibleIndex, "TRANG_THAI");
            //if (e.ButtonID == "Xem" && Convert.ToInt32(ishoatdong) == 0)
            //    e.Visible = DefaultBoolean.False;
        }
    }
}