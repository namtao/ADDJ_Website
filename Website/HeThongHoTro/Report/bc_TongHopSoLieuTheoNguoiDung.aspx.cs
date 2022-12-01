using ADDJ.Admin;
using ADDJ.Entity;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using Website.HTHTKT;
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro.Report
{
    public partial class bc_TongHopSoLieuTheoNguoiDung : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxHiddenField1["hidden_value"] = 0;
                ASPxHiddenField2["res_value"] = 0;
                ASPxHiddenField3["hidden_value2"] = 0;
                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_HE_THONG_HOTROs
                             select ss;
                    dtbCheckUpdateSMSSList = kl.ToList();
                }
                cboChonHeThongHoTro.DataSource = dtbCheckUpdateSMSSList;
                cboChonHeThongHoTro.TextField = "TENHETHONG";
                cboChonHeThongHoTro.ValueField = "ID";
                cboChonHeThongHoTro.DataBind();



                var dtbChonMucDo = new List<HT_MUCDO_SUCO>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_MUCDO_SUCOs
                             select ss;
                    dtbChonMucDo = kl.ToList();
                }

                cboMucDoSuCo.DataSource = dtbChonMucDo;
                cboMucDoSuCo.TextField = "TENMUCDO";
                cboMucDoSuCo.ValueField = "ID";
                cboMucDoSuCo.DataBind();

                cboMucDoSuCo.Items.Insert(0, new ListEditItem("-- chọn mức độ ---", 0));
                cboMucDoSuCo.SelectedIndex = 0;

            }
            var ddd1 = Convert.ToInt32(ASPxHiddenField2["res_value"]);
            var ddd2 = Convert.ToInt32(ASPxHiddenField3["hidden_value2"]);
            if (Convert.ToInt32(ASPxHiddenField2["res_value"]) != 0 && Convert.ToInt32(ASPxHiddenField3["hidden_value2"]) != 0)
            {
                ASPxTreeList hdfData = ((ASPxTreeList)LoaiHoTro.FindControl("treelist_donvi_cc"));
                hdfData.RefreshVirtualTree();
            }
        }
        
         
        protected void treelist_donvi_cc_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
        {
            ASPxHiddenField2["res_value"] = 0;
            ASPxHiddenField3["hidden_value2"] = 0;
            object sekkkk = ASPxHiddenField1["hidden_value"];
            object sekkkksss = ASPxHiddenField2["res_value"];
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
                    string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,
                                      HasChild = cast((case when exists(
			                                    select *
			                                    from HT_CAYTHUMUC_YCHT a
			                                    where a.ID_CHA = hhthtc.ID  
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

                    var dtbCheckUpdateSMSSList = new List<HT_CAYTHUMUC_YCHT>();
                    using (var ctx = new ADDJContext())
                    {
                        var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                                 where ss.ID_CHA == null
                                 select ss;
                        string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,
                                          HasChild = cast((case when exists(
			                                        select *
			                                        from HT_CAYTHUMUC_YCHT a
			                                        where a.ID_CHA = hhthtc.ID  
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

        protected void treelist_donvi_cc_VirtualModeNodeCreating(object sender, TreeListVirtualModeNodeCreatingEventArgs e)
        {
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["id"];
            e.SetNodeValue("linhvuc", node["linhvuc"]);
        }
        protected void treelist_donvi_cc_OnCustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            tree.RefreshVirtualTree();
            tree.ExpandToLevel(1);
        }

        protected void treeListDanhSachDonVi_VirtualModeCreateChildren(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeCreateChildrenEventArgs e)
        {

            if (e.NodeObject == null)
            {
                using (var ctx = new ADDJContext())
                {
                    string sqlStr = @"SELECT a.ID
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
		                                WHERE a.ID_CHA IS NULL OR a.ID_CHA=0 ";

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
        }
    }
}