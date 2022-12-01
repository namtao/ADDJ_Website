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
    public partial class mn_DmQuyenHeThong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void grvDmQuyenHeThong_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            grvDmQuyenHeThong.DataBind();
        }


        protected void grvDmQuyenHeThong_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<HT_NHOM_NGUOIDUNG>;
            if (tbl == null)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        var strSQl = @"SELECT * FROM dbo.NguoiSuDung_Group";
                        var lstHoTro = ctx.Database.SqlQuery<HT_NHOM_NGUOIDUNG>(strSQl);
                        var lst = lstHoTro.ToList();
                        Session["tblHeThongXL"] = lst;
                        tbl = lst;
                    }
                }
            }
            grvDmQuyenHeThong.DataSource = tbl;
        }

        protected void grvDmQuyenHeThong_PageIndexChanged(object sender, EventArgs e)
        {
            grvDmQuyenHeThong.DataBind();
        }

        protected void grvDmQuyenHeThong_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Status")
            {
                object getvalTrangthai = e.GetFieldValue("Status");
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

        protected void grvDmQuyenHeThong_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "Status") return;
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

   

        protected void grvDmQuyenHeThong_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {

        }
    }
}