using DevExpress.Web.ASPxTreeList;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_DmCayThuMucYeuCau : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxHiddenField1["id_caythumuc"] = 0;


                ASPxHiddenField1["res_value"] = 0;
                ASPxHiddenField1["hidden_value"] = 0;
                ASPxHiddenField1["hidden_value2"] = 0;


                HiddenField["res_valueADD"] = 0;
                HiddenField["hidden_valueADD"] = 0;
                HiddenField["hidden_value2ADD"] = 0;


                HiddenField["res_valueEDIT"] = 0;
                HiddenField["hidden_valueEDIT"] = 0;
                HiddenField["hidden_value2EDIT"] = 0;


                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_HE_THONG_HOTROs
                             where ss.TRANGTHAI.Value==true
                             select ss;
                    dtbCheckUpdateSMSSList = kl.ToList();
                }
                cboChonHeThongHTKT.DataSource = dtbCheckUpdateSMSSList;
                cboChonHeThongHTKT.TextField = "TENHETHONG";
                cboChonHeThongHTKT.ValueField = "ID";
                cboChonHeThongHTKT.DataBind();

                cboChonHeThongHTKT.Items.Insert(0, new DevExpress.Web.ListEditItem("-- chọn hệ thống --", "0"));
                cboChonHeThongHTKT.SelectedIndex = 0;

                // popup add
                cboDSHeThongYCHT.DataSource = dtbCheckUpdateSMSSList;
                cboDSHeThongYCHT.TextField = "TENHETHONG";
                cboDSHeThongYCHT.ValueField = "ID";
                cboDSHeThongYCHT.DataBind();

                cboDSHeThongYCHT.Items.Insert(0, new DevExpress.Web.ListEditItem("-- chọn hệ thống --", "0"));
                cboDSHeThongYCHT.SelectedIndex = 0;

                // popup edit
                cboChonHeThongSua.DataSource = dtbCheckUpdateSMSSList;
                cboChonHeThongSua.TextField = "TENHETHONG";
                cboChonHeThongSua.ValueField = "ID";
                cboChonHeThongSua.DataBind();

                cboChonHeThongSua.Items.Insert(0, new DevExpress.Web.ListEditItem("-- chọn hệ thống --", "0"));
                cboChonHeThongSua.SelectedIndex = 0;

            }

            var ddd1 = Convert.ToInt32(ASPxHiddenField1["res_value"]);
            var ddd2 = Convert.ToInt32(ASPxHiddenField1["hidden_value2"]);
            if (Convert.ToInt32(ASPxHiddenField1["res_value"]) != 0 && Convert.ToInt32(ASPxHiddenField1["hidden_value2"]) != 0)
            {
                asptree_caythumuchethong.RefreshVirtualTree();
            }


            var ddd1ADD = Convert.ToInt32(HiddenField["res_valueADD"]);
            var ddd2ADD = Convert.ToInt32(HiddenField["hidden_value2ADD"]);
            if (Convert.ToInt32(HiddenField["res_valueADD"]) != 0 && Convert.ToInt32(HiddenField["hidden_value2ADD"]) != 0)
            {
                ASPxTreeList hdfDataADD = ((ASPxTreeList)LoaiHoTroADD.FindControl("treelist_donvi_ccADD"));
                hdfDataADD.RefreshVirtualTree();
            }


            var ddd1EDIT = Convert.ToInt32(HiddenField["res_valueEDIT"]);
            var ddd2EDIT = Convert.ToInt32(HiddenField["hidden_value2EDIT"]);
            if (Convert.ToInt32(HiddenField["res_valueEDIT"]) != 0 && Convert.ToInt32(HiddenField["hidden_value2EDIT"]) != 0)
            {
                ASPxTreeList hdfDataEDIT = ((ASPxTreeList)LoaiHoTroEDIT.FindControl("treelist_donvi_ccEDIT"));
                hdfDataEDIT.RefreshVirtualTree();
            }
        }

        protected void asptree_caythumuchethong_VirtualModeCreateChildren(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeCreateChildrenEventArgs e)
        {
            object sekkkk = ASPxHiddenField1["hidden_value"];
            string sqlWhere = string.Empty;
            if (Convert.ToInt32(sekkkk) != 0)
            {
                sqlWhere = " and ID_HETHONG_YCHT=" + Convert.ToInt32(sekkkk);
            }

            if (e.NodeObject == null)
            {
                using (var ctx = new ADDJContext())
                {
                    string sqlStr = @"SELECT a.ID ,
                                       a.ID_HETHONG_YCHT ,
                                       b.MA_HETHONG,
                                       a.LINHVUC ,
                                       a.MA_LINHVUC,
                                       a.ID_CHA ,
                                       a.GHICHU ,
                                       a.TRANGTHAI ,
                                       a.NGAYCAPNHAT,
			                                  HasChild = cast((case when exists(
                                            select *
                                            from dbo.HT_CAYTHUMUC_YCHT b
                                            where b.ID_CHA = a.ID
                                        ) then 1 else 0 end) as bit) 	   
	                                    FROM dbo.HT_CAYTHUMUC_YCHT a
                                        INNER JOIN HT_DM_HETHONG_YCHT b ON a.ID_HETHONG_YCHT=b.ID
		                                WHERE (a.ID_CHA IS NULL or a.id_cha=0 and a.TRANGTHAI=1) " + sqlWhere;

                    var lsss = ctx.Database.SqlQuery<ViewCayThuMucYeuCauHT>(sqlStr);
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
                        string sqlStr = string.Format(@"SELECT a.ID ,
                                                       a.ID_HETHONG_YCHT ,
                                                       b.MA_HETHONG,
                                                       a.LINHVUC ,
                                                       a.MA_LINHVUC,
                                                       a.ID_CHA ,
                                                       a.GHICHU ,
                                                       a.TRANGTHAI ,
                                                       a.NGAYCAPNHAT,
			                                                  HasChild = cast((case when exists(
                                                            select *
                                                            from dbo.HT_CAYTHUMUC_YCHT b
                                                            where b.ID_CHA = a.ID
                                                        ) then 1 else 0 end) as bit) 	   
	                                                    FROM dbo.HT_CAYTHUMUC_YCHT a
                                                        INNER JOIN HT_DM_HETHONG_YCHT b ON a.ID_HETHONG_YCHT=b.ID
		                                                WHERE a.TRANGTHAI=1 and a.ID_CHA ={0} " + sqlWhere, id_cha);

                        var lsss = ctx.Database.SqlQuery<ViewCayThuMucYeuCauHT>(sqlStr);
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

        protected void asptree_caythumuchethong_VirtualModeNodeCreating(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeNodeCreatingEventArgs e)
        {
            // gán view display ở đây
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["ID"];
            e.SetNodeValue("ID", node["ID"]);
            e.SetNodeValue("ID_HETHONG_YCHT", node["ID_HETHONG_YCHT"]);
            e.SetNodeValue("MA_HETHONG", node["MA_HETHONG"]);
            e.SetNodeValue("LINHVUC", node["LINHVUC"]);
            e.SetNodeValue("MA_LINHVUC", node["MA_LINHVUC"]);
            e.SetNodeValue("ID_CHA", node["ID_CHA"]);
            e.SetNodeValue("GHICHU", node["GHICHU"]);
            e.SetNodeValue("TRANGTHAI", node["TRANGTHAI"]);
            e.SetNodeValue("NGAYCAPNHAT", node["NGAYCAPNHAT"]);
            e.SetNodeValue("HasChild", node["HasChild"]);
        }

        protected void asptree_caythumuchethong_VirtualModeNodeCreated(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualNodeEventArgs e)
        {
            //var node = (DataRowView)e.NodeObject;
            //if ((bool)node["NotAllowChecked"])
            //{
            //    e.Node.AllowSelect = false;
            //}
            //e.Node.ToString();
        }

        protected void asptree_caythumuchethong_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            if (e.Argument == "refresh")
            {
                ASPxTreeList tree = sender as ASPxTreeList;
                tree.RefreshVirtualTree();
                tree.ExpandToLevel(1);
            }
            if (e.Argument == "clear")
            {
                ASPxTreeList tree = sender as ASPxTreeList;
                tree.UnselectAll();
            }
        }

        protected void asptree_caythumuchethong_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlDataCellEventArgs e)
        {
            if (e.Level == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.ForeColor = System.Drawing.Color.DarkBlue;
            }
             

            if (e.Column.FieldName == "TRANGTHAI")
            {
                if(e.CellValue.ToString()=="True")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.Text = "Hoạt động";
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.LightCoral;
                    e.Cell.Text = "Hủy";
                }
            }
        }



        // begin tree popup add

        protected void treelist_donvi_cc_ADD_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
        {
            HiddenField["res_valueADD"] = 0;
            HiddenField["hidden_value2ADD"] = 0;
            object sekkkk = HiddenField["hidden_valueADD"];
            object sekkkksss = HiddenField["res_valueADD"];
            object iddonvi = 0;

            string sqlWhere = string.Empty;
            if (Convert.ToInt32(sekkkk) != 0)
            {
                sqlWhere = " and ID_HETHONG_YCHT=" + Convert.ToInt32(sekkkk);
            }
            if (e.NodeObject == null)
            {
                var dtbCheckUpdateSMSSList = new List<HT_CAYTHUMUC_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                             where ss.ID_CHA == null
                             select ss;
                    string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,hhthtc.MA_LINHVUC,
                                      HasChild = cast((case when exists(
			                                    select *
			                                    from HT_CAYTHUMUC_YCHT a
			                                    where a.ID_CHA = hhthtc.ID and a.TRANGTHAI=1 
		                                    ) then 1 else 0 end) as bit)
                                      from HT_CAYTHUMUC_YCHT hhthtc
                                      WHERE hhthtc.ID_CHA is null " + sqlWhere;
                    var lsss = ctx.Database.SqlQuery<ViewCayThuMuc>(sqlStr);
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
                                 where ss.ID_CHA == null
                                 select ss;
                        string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,hhthtc.MA_LINHVUC,
                                          HasChild = cast((case when exists(
			                                        select *
			                                        from HT_CAYTHUMUC_YCHT a
			                                        where a.ID_CHA = hhthtc.ID and a.TRANGTHAI=1
		                                        ) then 1 else 0 end) as bit)
                                          from HT_CAYTHUMUC_YCHT hhthtc
                                          WHERE hhthtc.ID_CHA =" + id_cha + sqlWhere;
                        var lsss = ctx.Database.SqlQuery<ViewCayThuMuc>(sqlStr);
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

        protected void treelist_donvi_cc_ADD_VirtualModeNodeCreating(object sender, TreeListVirtualModeNodeCreatingEventArgs e)
        {
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["id"];
            e.SetNodeValue("linhvuc", node["linhvuc"]);
            e.SetNodeValue("MA_LINHVUC", node["MA_LINHVUC"]);
        }
        protected void treelist_donvi_cc_ADD_OnCustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            tree.RefreshVirtualTree();
            tree.ExpandToLevel(1);
        }
        //+++++++++++++++++++++++++++++++++++++

        // end tree popup



        // begin tree edit popup

        protected void treelist_donvi_cc_EDIT_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
        {
            HiddenField["res_valueEDIT"] = 0;
            HiddenField["hidden_value2EDIT"] = 0;
            object sekkkk = HiddenField["hidden_valueEDIT"];
            object sekkkksss = HiddenField["res_valueEDIT"];
            object iddonvi = 0;

            string sqlWhere = string.Empty;
            if (Convert.ToInt32(sekkkk) != 0)
            {
                sqlWhere = " and ID_HETHONG_YCHT=" + Convert.ToInt32(sekkkk);
            }
            if (e.NodeObject == null)
            {
                var dtbCheckUpdateSMSSList = new List<HT_CAYTHUMUC_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                             where ss.ID_CHA == null
                             select ss;
                    string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,hhthtc.MA_LINHVUC,
                                      HasChild = cast((case when exists(
			                                    select *
			                                    from HT_CAYTHUMUC_YCHT a
			                                    where a.ID_CHA = hhthtc.ID and a.TRANGTHAI=1  
		                                    ) then 1 else 0 end) as bit)
                                      from HT_CAYTHUMUC_YCHT hhthtc
                                      WHERE hhthtc.ID_CHA is null " + sqlWhere;
                    var lsss = ctx.Database.SqlQuery<ViewCayThuMuc>(sqlStr);
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
                                 where ss.ID_CHA == null
                                 select ss;
                        string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,hhthtc.MA_LINHVUC,
                                          HasChild = cast((case when exists(
			                                        select *
			                                        from HT_CAYTHUMUC_YCHT a
			                                        where a.ID_CHA = hhthtc.ID and a.TRANGTHAI=1 
		                                        ) then 1 else 0 end) as bit)
                                          from HT_CAYTHUMUC_YCHT hhthtc
                                          WHERE hhthtc.ID_CHA =" + id_cha + sqlWhere;
                        var lsss = ctx.Database.SqlQuery<ViewCayThuMuc>(sqlStr);
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

        protected void treelist_donvi_cc_EDIT_VirtualModeNodeCreating(object sender, TreeListVirtualModeNodeCreatingEventArgs e)
        {
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["id"];
            e.SetNodeValue("linhvuc", node["linhvuc"]);
            e.SetNodeValue("MA_LINHVUC", node["MA_LINHVUC"]);
        }
        protected void treelist_donvi_cc_EDIT_OnCustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            tree.RefreshVirtualTree();
            tree.ExpandToLevel(1);
        }

        // end tree popup

    }
}