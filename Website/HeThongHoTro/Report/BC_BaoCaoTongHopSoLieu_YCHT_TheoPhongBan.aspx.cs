using ADDJ.Admin;
using ADDJ.Entity;
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

namespace Website.HeThongHoTro.Report
{
    public partial class BC_BaoCaoTongHopSoLieu_YCHT_TheoPhongBan1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
            }
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