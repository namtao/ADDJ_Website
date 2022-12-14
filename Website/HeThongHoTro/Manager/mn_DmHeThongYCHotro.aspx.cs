using ADDJ.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_DmHeThongYCHotro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ASPxHiddenField1["id_hethong"] = 0;
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo!=null)
                {
                    ASPxHiddenField1["username"] = loginInfo.Username;
                }
            }
        }

        protected void grvDanhMucHeThongYCHT_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            grvDanhMucHeThongYCHT.DataBind();
        }

        protected void grvDanhMucHeThongYCHT_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<HT_DM_HETHONG_YCHT>;
            if (tbl == null)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        //var strSQl = "select * from HT_DM_YEUCAU_HOTRO_HT where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";
                        var strSQl = @"SELECT * FROM dbo.HT_DM_HETHONG_YCHT";
                        var lstHoTro = ctx.Database.SqlQuery<HT_DM_HETHONG_YCHT>(strSQl);
                        var lst = lstHoTro.ToList();
                        Session["tblHeThongXL"] = lst;
                        tbl = lst;
                    }
                }
            }
            grvDanhMucHeThongYCHT.DataSource = tbl;
        }

        protected void grvDanhMucHeThongYCHT_PageIndexChanged(object sender, EventArgs e)
        {
            grvDanhMucHeThongYCHT.DataBind();
        }

        protected void grvDanhMucHeThongYCHT_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "TRANGTHAI")
            {
                object getvalTrangthai = e.GetFieldValue("TRANGTHAI");
                switch (Convert.ToBoolean(getvalTrangthai))
                {
                    case true:
                        e.DisplayText = "Hoạt động.";
                        break;
                    case false:
                        e.DisplayText = "Không hoạt động.";
                        break;
                    default:
                        break;
                }
            }
        }

        protected void grvDanhMucHeThongYCHT_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "TRANGTHAI") return;
            switch (Convert.ToInt32(e.CellValue))
            {
                case 1:
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case 0:
                    e.Cell.BackColor = System.Drawing.Color.LightCoral;
                    break;
                default:
                    break;
            }
        }
    }
}