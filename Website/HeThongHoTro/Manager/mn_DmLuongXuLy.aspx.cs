using ADDJ.Admin;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_DMLuongXuLy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxHiddenField1["id_luonght"] = 0;
                ASPxHiddenField1["ThongHTKTID"] = 0;

                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_HE_THONG_HOTROs
                             where ss.TRANGTHAI.Value == true
                             select ss;
                    dtbCheckUpdateSMSSList = kl.ToList();
                }
                cboChonHeThongHTKT.DataSource = dtbCheckUpdateSMSSList;
                cboChonHeThongHTKT.TextField = "TENHETHONG";
                cboChonHeThongHTKT.ValueField = "ID";
                cboChonHeThongHTKT.DataBind();

                cboChonHeThongHTKT.Items.Insert(0, new DevExpress.Web.ListEditItem("-- chọn hệ thống --", "0"));
                cboChonHeThongHTKT.SelectedIndex = 0;
            }
        }

        protected void grvDmLuongXuLy_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            grvDmLuongXuLy.DataBind();
        }

        protected void grvDmLuongXuLy_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<HT_LUONG_HOTRO2>;
            if (tbl == null)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        var strIDHeThong = "";
                        var idht = Convert.ToString(ASPxHiddenField1["ThongHTKTID"]);
                        if (idht != "0" && idht != "")
                        {
                            strIDHeThong = string.Format(@" and a.ID_HETHONG_YCHT={0} ", idht);
                        }
                 
                        //var strSQl = "select * from HT_NODE_LUONG_HOTRO where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";
                        var strSQl = @"SELECT a.ID
                                          ,a.ID_HETHONG_YCHT
                                          ,b.TENHETHONG
                                          ,a.TEN_LUONG
                                          ,a.TRANGTHAI
                                          ,a.MOTA
                                          ,a.SOBUOC FROM dbo.HT_LUONG_HOTRO a
                                      INNER JOIN HT_DM_HETHONG_YCHT b ON a.ID_HETHONG_YCHT=b.ID where b.trangthai=1 " + strIDHeThong;
                        var lstHoTro = ctx.Database.SqlQuery<HT_LUONG_HOTRO2>(strSQl);
                        var lst = lstHoTro.ToList();
                        Session["tblHeThongXL"] = lst;
                        tbl = lst;
                    }
                }
            }
            grvDmLuongXuLy.DataSource = tbl;
        }

        protected void grvDmLuongXuLy_PageIndexChanged(object sender, EventArgs e)
        {
            grvDmLuongXuLy.DataBind();
        }

        protected void grvDmLuongXuLy_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
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

        protected void grvDmLuongXuLy_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "SOBUOC")
            {
                e.Cell.BackColor = System.Drawing.Color.LightCoral;
                e.Cell.Font.Bold = true;
                e.Cell.HorizontalAlign = HorizontalAlign.Center;
               
            }
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

        protected void grvDmLuongXuLy_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            //if (e.VisibleIndex == -1) return;

            //var value = grvDmLuongXuLy.GetRowValues(e.VisibleIndex, "ID");
            //if (value.ToString() == "56")
            //    e.Visible = DefaultBoolean.False;

            //if (e.ButtonID == "Xoa" && e.VisibleIndex % 2 != 0)
            //    e.Visible = DefaultBoolean.False;
        }
    }
}