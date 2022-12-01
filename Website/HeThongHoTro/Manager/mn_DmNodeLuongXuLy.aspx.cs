using ADDJ.Admin;
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
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_DmNodeLuongXuLy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ASPxHiddenField1["hidden_value"] = 0;
                ASPxHiddenField1["res_value"] = 0;
                ASPxHiddenField1["hidden_value2"] = 0;
                ASPxHiddenField1["idluonghotro"] = 0;
                ASPxHiddenField1["id_hethong"] = 0;
                ASPxHiddenField1["idbuocxuly"] = 0;
                //ASPxHiddenField1["lstIDDonVi"] = 0;

                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var strsQl = @"SELECT * FROM HT_HTKTTT.dbo.HT_DM_HETHONG_YCHT where trangthai=1";
                    dtbCheckUpdateSMSSList = ctx.Database.SqlQuery<HT_DM_HETHONG_YCHT>(strsQl).ToList();
                }
                cboDanhSachHeThongCanYeuCauHoTro.DataSource = dtbCheckUpdateSMSSList;
                cboDanhSachHeThongCanYeuCauHoTro.TextField = "TENHETHONG";
                cboDanhSachHeThongCanYeuCauHoTro.ValueField = "ID";
                cboDanhSachHeThongCanYeuCauHoTro.DataBind();

                cboDanhSachHeThongCanYeuCauHoTro.Items.Insert(0, new DevExpress.Web.ListEditItem("-- chọn hệ thống --", "0"));
                cboDanhSachHeThongCanYeuCauHoTro.SelectedIndex = 0;
            }

            var ddd1 = Convert.ToInt32(ASPxHiddenField1["res_value"]);
            var ddd2 = Convert.ToInt32(ASPxHiddenField1["hidden_value2"]);
            var id_luonghotro = Convert.ToInt32(ASPxHiddenField1["idluonghotro"]);
            if (Convert.ToInt32(ASPxHiddenField1["res_value"]) != 0 && Convert.ToInt32(ASPxHiddenField1["hidden_value2"]) != 0)
            {
                // lấy thông tin luồng hỗ trợ
                var dtbCheckUpdateSMSSList = new List<HT_LUONG_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    var strsQl = @"SELECT * FROM HT_HTKTTT.dbo.HT_LUONG_HOTRO where trangthai=1 and ID=" + id_luonghotro;
                    dtbCheckUpdateSMSSList = ctx.Database.SqlQuery<HT_LUONG_HOTRO>(strsQl).ToList();
                }
                if (dtbCheckUpdateSMSSList.Any())
                {
                    var tongsobuoc = dtbCheckUpdateSMSSList[0].SOBUOC;
                    for (int i = 0; i < tongsobuoc; i++)
                    {
                        rdoDanhSachCacBuocXuLy.Items.Add(new DevExpress.Web.ListEditItem("Bước " + (i + 1), (i + 1)));
                    }
                }
                rdoDanhSachCacBuocXuLy.SelectedIndex = 0; //Convert.ToInt32(ASPxHiddenField1["idbuocxuly"]);
                                                          //ASPxTreeList hdfData = ((ASPxTreeList)LoaiHoTro.FindControl("treelist_donvi_cc"));
                                                          //hdfData.RefreshVirtualTree();

            }
        }

        string id_hethonghotro = "0";
        string id_luonghotro = "0";
        string id_buocxuly = "0";
        protected void grvNodeLuongXuLy_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            var lstStr = e.Parameters.Split('|');
            id_hethonghotro = ASPxHiddenField1["id_hethong"].ToString();// lstStr[0];
            id_luonghotro = ASPxHiddenField1["idluonghotro"].ToString();// lstStr[1];           
            id_buocxuly = ASPxHiddenField1["idbuocxuly"].ToString();// lstStr[2];

            if (id_hethonghotro != "0" && id_luonghotro != "0" && id_buocxuly != "0")
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl = string.Format(@"SELECT a.ID,b.Id IDDonVi,b.MADONVI,b.TENDONVI FROM dbo.HT_NODE_LUONG_HOTRO a
                                                    INNER JOIN	dbo.HT_DONVI b ON a.ID_DONVI=b.ID
                                                    WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND BUOCXULY={2}",
                                                id_hethonghotro, id_luonghotro, id_buocxuly);

                    var lstHoTro = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO4>(strSQl);
                    var lst = lstHoTro.ToList();
                    Session["tblHeThongXL_checkcolor"] = lst;
                }
            }
            else
            {
                Session.Remove("tblHeThongXL_checkcolor");
            }
            grvNodeLuongXuLy.DataBind();
        }

        protected void grvNodeLuongXuLy_DataBinding(object sender, EventArgs e)
        {
            id_hethonghotro = ASPxHiddenField1["id_hethong"].ToString();// lstStr[0];
            id_luonghotro = ASPxHiddenField1["idluonghotro"].ToString();// lstStr[1];           
            id_buocxuly = ASPxHiddenField1["idbuocxuly"].ToString();// lstStr[2];
            if (id_hethonghotro != "0" && id_luonghotro != "0" && id_buocxuly != "0")
            {
                var tbl = Session["tblHeThongXL"] as List<HT_NODE_LUONG_HOTRO4>;
                if (tbl == null)
                {
                    AdminInfo loginInfo = LoginAdmin.AdminLogin();
                    if (loginInfo != null)
                    {
                        using (var ctx = new ADDJContext())
                        {
                            var strSQl = string.Format(@"SELECT a.ID,b.Id IDDonVi,b.MADONVI,b.TENDONVI FROM dbo.HT_NODE_LUONG_HOTRO a
                                                    INNER JOIN	dbo.HT_DONVI b ON a.ID_DONVI=b.ID
                                                    WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND BUOCXULY={2}",
                                                        id_hethonghotro, id_luonghotro, id_buocxuly);

                            var lstHoTro = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO4>(strSQl);
                            var lst = lstHoTro.ToList();
                            Session["tblHeThongXL"] = lst;
                            tbl = lst;
                        }
                    }
                }
               
                grvNodeLuongXuLy.DataSource = tbl;
            }
        }

        protected void grvNodeLuongXuLy_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {

        }

        protected void grvNodeLuongXuLy_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {

        }

        protected void grvNodeLuongXuLy_PageIndexChanged(object sender, EventArgs e)
        {
            grvNodeLuongXuLy.DataBind();
        }

        private void napDanhSachLuongYeuCauHT(string idhethongyeucau)
        {
            try
            {
                var dtbCheckUpdateSMSSList = new List<HT_LUONG_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    var strsQl = @"SELECT ID ,
                                   ID_HETHONG_YCHT ,
                                   TEN_LUONG +' ('+MOTA+')' AS TEN_LUONG,
                                   TRANGTHAI ,
                                   MOTA ,
                                   SOBUOC FROM HT_HTKTTT.dbo.HT_LUONG_HOTRO 
                                   WHERE TRANGTHAI=1 AND ID_HETHONG_YCHT=" + idhethongyeucau;
                    dtbCheckUpdateSMSSList = ctx.Database.SqlQuery<HT_LUONG_HOTRO>(strsQl).ToList();
                }
                cboDanhSachLuongYeuCauHoTro.DataSource = dtbCheckUpdateSMSSList;
                cboDanhSachLuongYeuCauHoTro.TextField = "TEN_LUONG";
                cboDanhSachLuongYeuCauHoTro.ValueField = "ID";
                cboDanhSachLuongYeuCauHoTro.DataBind();

                cboDanhSachLuongYeuCauHoTro.Items.Insert(0, new DevExpress.Web.ListEditItem("-- chọn luồng thực hiện --", "0"));
                cboDanhSachLuongYeuCauHoTro.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void cboDanhSachLuongYeuCauHoTro_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            var idhethongyeucau = e.Parameter;
            napDanhSachLuongYeuCauHT(idhethongyeucau);
        }

        protected void treelist_donvi_cc_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
        {
            string strDanhSachDonVi = "";
            if (ASPxHiddenField1["id_hethong"].ToString() != "0" && 
                ASPxHiddenField1["idluonghotro"].ToString() != "0" &&
                ASPxHiddenField1["idbuocxuly"].ToString() != "0")
            {
                strDanhSachDonVi = string.Format(@"SELECT b.ID FROM dbo.HT_NODE_LUONG_HOTRO a
                                                    INNER JOIN	dbo.HT_DONVI b ON a.ID_DONVI=b.ID
                                                    WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND BUOCXULY={2}", 
                                                    ASPxHiddenField1["id_hethong"].ToString(), 
                                                    ASPxHiddenField1["idluonghotro"].ToString(), 
                                                    ASPxHiddenField1["idbuocxuly"].ToString());
            }


            ASPxHiddenField1["res_value"] = 0;
            ASPxHiddenField1["hidden_value2"] = 0;
            object sekkkk = ASPxHiddenField1["hidden_value"];
            object sekkkksss = ASPxHiddenField1["res_value"];
            object iddonvi = 0;
            //if (donvi_nhanvien == null)
            //    donvi_nhanvien = cs_DONVI_NHANVIEN.Get(UserInfo.Username);
            //if (donvi_nhanvien.QUANLY_VTT == true)
            //{
            //    iddonvi = 0;
            //    //btnCapNhatGia.Visible = false;
            //}
            //else
            //{
            //    if (donvi_nhanvien.QUAN_LY == true)
            //    {
            //        iddonvi = donvi_nhanvien.ID_DON_VI_CHA;
            //        iddonvi = SqlHelper.ExecuteScalar(System.Configuration.ConfigurationManager.ConnectionStrings["QLBH"].ConnectionString,
            //        "SP_GET_DONVI_QUANLY_USERNAME", UserInfo.Username, donvi_nhanvien.ID_DON_VI);
            //    }
            //    else
            //    {
            //        iddonvi = donvi_nhanvien.ID_DON_VI;
            //        //btnCapNhatGia.Visible = true;
            //    }
            //}

            string sqlWhere = string.Empty;
            if (Convert.ToInt32(sekkkk) != 0)
            {

                //ASPxTreeList hdfData = ((ASPxTreeList)LoaiHoTro.FindControl("treelist_donvi_cc"));
                //hdfData.ClearNodes();

                //sqlWhere = " and Id=" + Convert.ToInt32(sekkkk);
            }
            //if (strDanhSachDonVi != "")
                //sqlWhere = " AND hhthtc.Id not IN (" + strDanhSachDonVi + ")";

            if (e.NodeObject == null)
            {
                //DataTable tb_dstt = SqlHelper.ExecuteDataset(strconn, "DM_THONGTIN_CUAHANG_GET_ALL", iddonvi, 0).Tables[0];
                //if ((int)iddonvi == 0)
                //{
                //    DataRow toInsert = tb_dstt.NewRow();
                //    toInsert["id"] = 0;
                //    toInsert["ten"] = "--tất cả--";
                //    toInsert["HasChild"] = 0;
                //    tb_dstt.Rows.InsertAt(toInsert, 0);
                //}
                //e.Children = new DataView(tb_dstt);

                var dtbCheckUpdateSMSSList = new List<HT_CAYTHUMUC_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                             where ss.ID_CHA == null
                             select ss;

                    string sqlStr = "";
                    if (strDanhSachDonVi!="")
                        sqlStr = string.Format(@"SELECT hhthtc.ID,cast(hhthtc.ID as nvarchar) MaDonVi,hhthtc.TENDONVI,
                                                  HasChild = cast((case when exists(
			                                                select *
			                                                from dbo.HT_DONVI a
			                                                where a.ID_CHA = hhthtc.ID 
		                                                ) then 1 else 0 end) as bit),
                                                  NotAllowChecked  = cast((case when exists(

			                                                {0} 

                                                and b.Id = hhthtc.ID ) then 1 else 0 end) as bit)
                                                  from dbo.HT_DONVI hhthtc
                                                  WHERE hhthtc.ID_CHA =0 OR hhthtc.ID_CHA IS NULL ", strDanhSachDonVi);
                    else
                        sqlStr = string.Format(@"SELECT hhthtc.ID,cast(hhthtc.ID as nvarchar) MaDonVi,hhthtc.TENDONVI,
                                                  HasChild = cast((case when exists(
			                                                select *
			                                                from dbo.HT_DONVI a
			                                                where a.ID_CHA = hhthtc.ID  
		                                                ) then 1 else 0 end) as bit),
                                                  NotAllowChecked  = CAST(0 AS BIT)
                                                  from dbo.HT_DONVI hhthtc
                                                  WHERE hhthtc.ID_CHA =0 OR hhthtc.ID_CHA IS NULL ");


                    var lsss = ctx.Database.SqlQuery<ViewCayThuMucDoiTac2>(sqlStr);
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
                if (Convert.ToInt32(sekkkksss) == 0)
                {

                    int id_cha = Convert.ToInt32(((DataRowView)e.NodeObject)["id"]);
                    //DataTable tb_dstt = SqlHelper.ExecuteDataset(strconn, "DM_THONGTIN_CUAHANG_GET_ALL", id_cha, 1).Tables[0];
                    //e.Children = new DataView(tb_dstt);

                    var dtbCheckUpdateSMSSList = new List<HT_CAYTHUMUC_YCHT>();
                    using (var ctx = new ADDJContext())
                    {
                        var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs 
                                 where ss.ID_CHA == null &&ss.TRANGTHAI.Value==true
                                 select ss;

                        string sqlStr = "";
                        if (strDanhSachDonVi != "")
                            sqlStr = string.Format(@"SELECT ID,cast(hhthtc.ID as nvarchar) MaDonVi,hhthtc.TENDONVI,
                                                      HasChild = cast((case when exists(
			                                                    select *
			                                                    from dbo.HT_DONVI a
			                                                    where a.ID_CHA = hhthtc.ID  
		                                                    ) then 1 else 0 end) as bit),
                                                      NotAllowChecked  = cast((case when exists(

			                                                    {0} 

                                                      and b.Id = hhthtc.ID ) then 1 else 0 end) as bit)
                                                      from dbo.HT_DONVI hhthtc
                                                      WHERE hhthtc.ID_CHA = {1}", strDanhSachDonVi, id_cha);
                        else
                            sqlStr = string.Format(@"SELECT ID,cast(hhthtc.ID as nvarchar) MaDonVi,hhthtc.TENDONVI,
                                                      HasChild = cast((case when exists(
			                                                    select *
			                                                    from dbo.HT_DONVI a
			                                                    where a.ID_CHA = hhthtc.ID  
		                                                    ) then 1 else 0 end) as bit),
                                                      NotAllowChecked  = CAST(0 AS BIT)
                                                      from dbo.HT_DONVI hhthtc
                                                      WHERE hhthtc.ID_CHA = {0}", id_cha);


                        var lsss = ctx.Database.SqlQuery<ViewCayThuMucDoiTac2>(sqlStr);
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

        protected void treelist_donvi_cc_VirtualModeNodeCreating(object sender, TreeListVirtualModeNodeCreatingEventArgs e)
        {
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["ID"];            
            e.SetNodeValue("ID", node["ID"]);
            e.SetNodeValue("TENDONVI", node["TENDONVI"]);
        }

        protected void treelist_donvi_cc_VirtualModeNodeCreated(object sender, TreeListVirtualNodeEventArgs e)
        {
            var node = (DataRowView)e.NodeObject;
            if ((bool)node["NotAllowChecked"])
            {
                e.Node.AllowSelect = false;
            }
            e.Node.ToString();

            //if (node["ID"].ToString()=="13")
            //{
            //    e.Node.AllowSelect = true;
            //    e.Node.Selected = true;
            //}
            //var tbl = Session["tblHeThongXL"] as List<HT_NODE_LUONG_HOTRO3>;
            //if (tbl != null)
            //{
            //    var iddonvitrongnodetree = Convert.ToInt32(node["ID"].ToString());
            //    var lst = tbl.Where(P => P.IDDoiTac == iddonvitrongnodetree);
            //    if (lst.ToList().Any())
            //    {
            //        e.Node.AllowSelect = true;
            //        e.Node.Selected = true;
            //    }
            //    else
            //    {                     
            //        e.Node.Selected = false;
            //    }
            //}
        }

        protected void treelist_donvi_cc_OnCustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            if (e.Argument == "refresh")
            {
                ASPxTreeList tree = sender as ASPxTreeList;
                tree.RefreshVirtualTree();
                tree.ExpandToLevel(1);
            }
            if(e.Argument=="clear")
            {
                ASPxTreeList tree = sender as ASPxTreeList;
                tree.UnselectAll();
                tree.ExpandToLevel(1);
            }     
        }

        protected void treelist_donvi_cc_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
        {
            if (e.Level == 1)
                e.Cell.Font.Bold = true;
        }

        protected void grvNodeLuongXuLy_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {

        }

        protected void cbpanellabel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            //ASPxLabel1.Text = "Names: ";
            ASPxLabel1.Text = "";
            List<TreeListNode> nodes = treelist_donvi_cc.GetSelectedNodes();
            //var arr_lstid = "";
            foreach (TreeListNode node in nodes)
            {
                ASPxLabel1.Text += node.GetValue("ID") + " ";
                //arr_lstid += node.GetValue("ID") + " ";
            }
            //ASPxHiddenField1["lstIDDonVi"] = arr_lstid;
        }

        protected void treelist_donvi_cc_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            //if (Convert.ToInt32(e.GetValue("ID"))==10035)
            //{
            //    e.Row.BackColor = Color.FromArgb(211, 235, 183);
            //}
            var id_donvi = Convert.ToInt32(e.GetValue("ID"));
            var tbl = Session["tblHeThongXL_checkcolor"] as List<HT_NODE_LUONG_HOTRO4>;
            if (tbl != null)
            {
                if (tbl.Where(p => p.IDDonVi == id_donvi).Any())
                {
                    e.Row.BackColor = Color.FromArgb(255, 156, 60);
                }
            }
            else
            {
                e.Row.BackColor = Color.FromArgb(255, 255, 255);
            }

            //var id_hethonghotro1 = ASPxHiddenField1["id_hethong"].ToString();// lstStr[0];
            //var id_luonghotro1 = ASPxHiddenField1["idluonghotro"].ToString();// lstStr[1];           
            //var id_buocxuly1 = ASPxHiddenField1["idbuocxuly"].ToString();// lstStr[2];
            //if (id_hethonghotro1 == "0" || id_luonghotro1 == "0" || id_buocxuly1 == "0" || id_donvi == 0)
            //    return;

            //using (var ctx = new HTHTKTContext())
            //{
            //    var strSQl = string.Format(@"SELECT a.ID,b.Id IDDonVi,b.MADONVI,b.TENDONVI FROM dbo.HT_NODE_LUONG_HOTRO a
            //                                        INNER JOIN	dbo.HT_DONVI b ON a.ID_DONVI=b.ID
            //                                        WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND BUOCXULY={2} and a.ID_DONVI={3}",
            //                                id_hethonghotro1, id_luonghotro1, id_buocxuly1, id_donvi);

            //    var lstHoTro = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO4>(strSQl);
            //    var lst = lstHoTro.ToList();
            //    if (lst.Any())
            //    {
            //        e.Row.BackColor = Color.FromArgb(255, 156, 60);
            //    }
            //    else
            //    {
            //        e.Row.BackColor = Color.FromArgb(255, 255, 255);
            //    }
            //}
        }
    }
}