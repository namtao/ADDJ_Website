using ADDJ.Admin;
using DevExpress.Utils;
using DevExpress.Web.ASPxTreeList;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_nhatKyTacDong : System.Web.UI.Page
    {
         public static int id_donvi = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxHiddenField1["nguoitao"] = "";
                   AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    // Xử lý lấy thông tin đơn vị
                    id_donvi = loginInfo.PhongBanId;
                    ASPxHiddenField1["nguoitao"] = loginInfo.Username;
                }



                ASPxHiddenField1["id_luonght"] = 0;
                ASPxHiddenField1["ThongHTKTID"] = 0;

                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_HE_THONG_HOTROs
                             select ss;
                    dtbCheckUpdateSMSSList = kl.ToList();
                }
                cboChonHeThongHTKT.DataSource = dtbCheckUpdateSMSSList;
                cboChonHeThongHTKT.TextField = "TENHETHONG";
                cboChonHeThongHTKT.ValueField = "ID";
                cboChonHeThongHTKT.DataBind();

                cboChonHeThongHTKT.Items.Insert(0, new DevExpress.Web.ListEditItem("-- chọn hệ thống --","0"));
                cboChonHeThongHTKT.SelectedIndex = 0;
            }
        }

        // Đơn vị___________________________________
        
        protected void treeListDanhSachDonVi_VirtualModeCreateChildren(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeCreateChildrenEventArgs e)
        {

            if (e.NodeObject == null)
            {
                using (var ctx = new ADDJContext())
                {
                    string sqlStr = string.Format(@"SELECT a.ID
                                      ,a.MADONVI
                                      ,a.TENDONVI
                                      ,a.ID_CHA
                                      ,a.MOTA
                                      ,a.LOAIDONVI
                                      ,a.GHICHU
                                      ,a.XOA
                                      ,a.DIENTHOAI
                                      ,a.FAX
                                      ,a.DIACHI
                                      ,a.TRANGTHAI
                                      ,a.ID_TINHTHANH
                                      ,a.NGAYCAPNHAT
                                      ,HasChild = cast((case when exists(
                                            select *
                                            from dbo.HT_DONVI b
                                            where b.ID_CHA = a.ID
                                        ) then 1 else 0 end) as bit) 	   
	                                    FROM dbo.HT_DONVI a
		                                WHERE a.ID={0}", id_donvi);

                    var lsss = ctx.Database.SqlQuery<HT_DONVI2>(sqlStr);
                    var dt = lsss.ToList();

                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(dt))
                    {
                        table.Load(reader);
                    }
                    e.Children = new DataView(table);
                }
            }
            else
            {
                if (Convert.ToInt32(0) == 0)
                {

                    int id_cha = Convert.ToInt32(((DataRowView)e.NodeObject)["id"]);

                    using (var ctx = new ADDJContext())
                    {
                        string sqlStr = string.Format(@"SELECT a.ID
                                                      ,a.MADONVI
                                                      ,a.TENDONVI
                                                      ,a.ID_CHA
                                                      ,a.MOTA
                                                      ,a.LOAIDONVI
                                                      ,a.GHICHU
                                                      ,a.XOA
                                                      ,a.DIENTHOAI
                                                      ,a.FAX
                                                      ,a.DIACHI
                                                      ,a.TRANGTHAI
                                                      ,a.ID_TINHTHANH
                                                      ,a.NGAYCAPNHAT
                                                      ,HasChild = cast((case when exists(
                                                            select *
                                                            from dbo.HT_DONVI b
                                                            where b.ID_CHA = a.ID
                                                        ) then 1 else 0 end) as bit) 	   
	                                                    FROM dbo.HT_DONVI a
		                                                WHERE a.ID_CHA ={0} ", id_cha);

                        var lsss = ctx.Database.SqlQuery<HT_DONVI2>(sqlStr);
                        var dt = lsss.ToList();

                        DataTable table = new DataTable();
                        using (var reader = ObjectReader.Create(dt))
                        {
                            table.Load(reader);
                        }
                        e.Children = new DataView(table);
                    }
                }
            }
        }


        protected void treeListDanhSachDonVi_VirtualModeNodeCreated(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualNodeEventArgs e)
        {
            //var node = (DataRowView)e.NodeObject;
            //if ((bool)node["NotAllowChecked"])
            //{
            //    e.Node.AllowSelect = false;
            //}
            //e.Node.ToString();
        }

        protected void treeListDanhSachDonVi_VirtualModeNodeCreating(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeNodeCreatingEventArgs e)
        {
            // gán view display ở đây
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["ID"];
            e.SetNodeValue("ID", node["ID"]);
            e.SetNodeValue("MADONVI", node["MADONVI"]);
            e.SetNodeValue("TENDONVI", node["TENDONVI"]);
            e.SetNodeValue("TRANGTHAI", node["TRANGTHAI"]);
            e.SetNodeValue("HasChild", node["HasChild"]);
        }

        protected void treeListDanhSachDonVi_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            tree.RefreshVirtualTree();
            tree.ExpandToLevel(1);
        }

        protected void treeListDanhSachDonVi_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlDataCellEventArgs e)
        {
            if (e.Level == 1)
                e.Cell.Font.Bold = true;

            if (e.Column.FieldName == "TRANGTHAI")
            {
                int value = Convert.ToInt32(e.CellValue);
                if (value == 0)
                {
                    e.Cell.Text = "Hủy";
                }
                else
                {
                    e.Cell.Text = "Hoạt động";
                }
                //e.Cell.BackColor = GetBudgetColor(value);
                //if (value > 1000000M)
                //    e.Cell.Font.Bold = true;
            }
        }

        protected void treeListDanhSachDonVi_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            if (Convert.ToInt32(e.GetValue("TRANGTHAI")) == 0)
            {
                e.Row.BackColor = Color.LightCoral;
            }
        }
        //_end đơn vị___________________________________











        protected void grvDmLuongXuLy_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            grvDmLuongXuLy.DataBind();
        }

        protected void grvDmLuongXuLy_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<HT_DAUMOI_XL>;
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
                            strIDHeThong = string.Format(@" WHERE hdx.ID_HETHONG={0} ", idht);
                        }
                 
                        //var strSQl = "select * from HT_NODE_LUONG_HOTRO where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";
                        var strSQl = @"SELECT hdx.ID
                                          ,hdx.HOTEN
                                          ,hdx.ID_DONVI
                                          ,hd.TENDONVI
                                          ,hdx.ID_HETHONG
                                          ,hdhy.TENHETHONG
                                          ,hdx.DIENTHOAI
                                          ,hdx.EMAIL
                                          ,hdx.TRANGTHAI
                                          ,hdx.NGUOITAO
                                          ,hdx.NGAYTAO FROM HT_DAUMOI_XL hdx
                                      LEFT JOIN HT_DM_HETHONG_YCHT hdhy ON hdhy.ID=hdx.ID_HETHONG
                                      LEFT JOIN HT_DONVI hd ON hd.ID=hdx.ID_DONVI" + strIDHeThong;
                        var lstHoTro = ctx.Database.SqlQuery<HT_DAUMOI_XL>(strSQl);
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