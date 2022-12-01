using DevExpress.Web.ASPxTreeList;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_HtQuanLyMenu : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                napDuongDanMenu();
            }
        }

        private void napDuongDanMenu()
        {
            try
            {
                cboLoaiMenuAdd.Items.Add(new DevExpress.Web.ListEditItem("Menu", "1"));
                cboLoaiMenuAdd.Items.Add(new DevExpress.Web.ListEditItem("Phân cách", "2"));

                cboLoaiMenuEdit.Items.Add(new DevExpress.Web.ListEditItem("Menu", "1"));
                cboLoaiMenuEdit.Items.Add(new DevExpress.Web.ListEditItem("Phân cách", "2"));

                string path = Server.MapPath("~");
                string[] arrFile = Directory.GetFiles(path, "*.aspx", SearchOption.AllDirectories);
                List<string> lstFile = new List<string>();

                foreach (string file in arrFile)
                {
                    lstFile.Add("/" + file.Replace(path, "").Replace("\\", "/"));
                }

                cboDuongDanAdd.DataSource = lstFile;
                cboDuongDanAdd.DataBind();

                cboDuongDanEdit.DataSource = lstFile;
                cboDuongDanEdit.DataBind();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected void treeListDanhSachDonVi_VirtualModeCreateChildren(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeCreateChildrenEventArgs e)
        {

            if (e.NodeObject == null)
            {
                using (var ctx = new ADDJContext())
                {
                    string sqlStr = @"SELECT a.ID
                                      ,a.STT
                                      ,a.ParentID
                                      ,a.Name
                                      ,a.Name2
                                      ,a.Name3
                                      ,a.Link
                                      ,a.Display
                                      ,a.MenuType
                                      ,HasChild = cast((case when exists(
                                            select *
                                            from dbo.Menu b
                                            where b.ParentID = a.ID
                                        ) then 1 else 0 end) as bit) 	   
	                                    FROM dbo.Menu a
		                                WHERE a.ParentID IS NULL OR a.ParentID=0 ";

                    var lsss = ctx.Database.SqlQuery<HT_MENU0>(sqlStr);
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
                                      ,a.STT
                                      ,a.ParentID
                                      ,a.Name
                                      ,a.Name2
                                      ,a.Name3
                                      ,a.Link
                                      ,a.Display
                                      ,a.MenuType
                                      ,HasChild = cast((case when exists(
                                            select *
                                            from dbo.Menu b
                                            where b.ParentID = a.ID
                                        ) then 1 else 0 end) as bit) 	   
	                                    FROM dbo.Menu a
		                                WHERE a.ParentID ={0} ", id_cha);

                        var lsss = ctx.Database.SqlQuery<HT_MENU0>(sqlStr);
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
            e.SetNodeValue("Name", node["Name"]);
            e.SetNodeValue("Name2", node["Name2"]);
            e.SetNodeValue("Name3", node["Name3"]);
            e.SetNodeValue("Display", node["Display"]);
            e.SetNodeValue("STT", node["STT"]);
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

            if (e.Column.FieldName == "Display")
            {
                if ((int)e.CellValue==0)
                {
                    e.Cell.BackColor = Color.LightCoral;
                    e.Cell.Text = "Không";
                }
                else
                {
                    e.Cell.Text = "Có";
                }
              
                //decimal value = (decimal)e.CellValue;
                //e.Cell.BackColor = GetBudgetColor(value);
                //if (value > 1000000M)
                //    e.Cell.Font.Bold = true;
            }
        }
        Color GetBudgetColor(decimal value)
        {
            decimal coeff = value / 1000 - 22;
            int a = (int)(0.02165M * coeff);
            int b = (int)(0.09066M * coeff);
            return Color.FromArgb(255, 235 - a, 177 - b);
        }
        protected void treeListDanhSachDonVi_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            if (Object.Equals(e.GetValue("Display"), 0))
                e.Row.BackColor = Color.FromArgb(211, 235, 183);
        }
    }
}