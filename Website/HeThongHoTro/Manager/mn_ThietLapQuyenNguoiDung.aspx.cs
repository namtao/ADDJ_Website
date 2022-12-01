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
using Website.HTHTKT;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_ThietLapQuyenNguoiDung : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var id_nguoidung = Convert.ToString(Request.QueryString["idnguoidung"]);
                hiddenQuyen["role"] = id_nguoidung;

                var lstNguoiDung = new List<HT_NGUOIDUNG>();
                using (var ctx = new ADDJContext())
                {
                    var strSql = string.Format(@"SELECT hn.Id
                              ,hn.TenDoiTac
                              ,hn.DoiTacId
                              ,hn.KhuVucId
                              ,hn.NhomNguoiDung
                              ,hn.TenTruyCap
                              ,hn.MatKhau
                              ,hn.TenDayDu
                              ,hn.NgaySinh
                              ,hn.DiaChi
                              ,hn.DiDong
                              ,hn.CoDinh
                              ,hn.Sex
                              ,hn.Email
                              ,hn.CongTy
                              ,hn.DiaChiCongTy
                              ,hn.FaxCongTy
                              ,hn.DienThoaiCongTy
                              ,hn.TrangThai
                              ,hn.SuDungLDAP
                              ,hn.LoginCount
                              ,hn.LastLogin
                              ,hn.IsLogin
                              ,hn.LDate
                              ,hn.CDate
                              ,hn.CUser
                              ,hn.LUser
                              ,hn.ID_DONVI
                              ,hn.XOA 
                              FROM HT_NGUOIDUNG hn WHERE hn.Id={0}", id_nguoidung);
                    lstNguoiDung = ctx.Database.SqlQuery<HT_NGUOIDUNG>(strSql).ToList();
                }
                if (lstNguoiDung.Any())
                {
                    lblTenNguoiDung.Text = lstNguoiDung[0].TenDayDu;
                }
                
            }
        }
        

        protected void treeListMenuQuyen_VirtualModeCreateChildren(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeCreateChildrenEventArgs e)
        {
            try
            {
                if (e.NodeObject == null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        string sqlStr = string.Empty;

                        sqlStr = string.Format(@"SELECT a.ID
                                      ,a.STT
                                      ,a.ParentID
                                      ,a.Name
                                      ,a.Name2
                                      ,a.Name3
                                      ,a.Link
                                      ,a.Display
                                      ,a.MenuType
                                  ,HasChild=CAST((case when EXISTS(
                                  SELECT id FROM  Menu c WHERE c.ParentID=a.ID) then 1 else 0 END) as BIT)
                                  FROM Menu a 
		                           WHERE a.ParentID IS NULL OR a.ParentID=0 and a.Display=1 AND a.MenuType=1  ");



                        var lsss = ctx.Database.SqlQuery<HT_MENU>(sqlStr);
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
                        string role = hiddenQuyen.Get("role").ToString();
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
                                  ,HasChild=CAST((case when EXISTS(
                                  SELECT id FROM  Menu c WHERE c.ParentID=a.ID) then 1 else 0 END) as BIT)
                                  FROM Menu a
		                          WHERE a.ParentID ={0} and a.Display=1 AND a.MenuType=1  ", id_cha, role);

                            var lsss = ctx.Database.SqlQuery<HT_MENU>(sqlStr);
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
            catch (Exception ex)
            {

            }
        }


        protected void treeListMenuQuyen_VirtualModeNodeCreated(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualNodeEventArgs e)
        {
            try
            {
                TreeListNode a = (TreeListNode)e.Node;
                e.Node.Selected = false;
                string role = "0";
                if (hiddenQuyen.Contains("role"))
                {
                    role = hiddenQuyen.Get("role").ToString();
                    using (var ctx = new ADDJContext())
                    {
                        string sqlStr = string.Format(@"SELECT * FROM UserRight a 
WHERE a.AdminID={0} and a.MenuId={1}", role, a.Key);

                        var lsss = ctx.Database.SqlQuery<HT_QUYEN_NGUOIDUNG>(sqlStr);
                        var dt = lsss.ToList();
                        if (dt.Any())
                        {
                            e.Node.Selected = true;
                            //TreeListDataColumn colDel = treeListMenuQuyen.Columns["UserEdit"] as TreeListDataColumn;
                            //var ye = treeListMenuQuyen.FindDataCellTemplateControl(e.Node.Key, colDel, "kj") as ASPxCheckBox;
                            //ye.Checked = dt[0].UserEdit.Value;
                            //e.Node.SetValue("UserEdit", dt[0].UserEdit.Value);
                            //e.Node.SetValue("UserDelete", dt[0].UserDelete.Value);
                            //e.Node.SetValue("Other1", dt[0].Other1.Value);
                            //e.Node.SetValue("Other2", dt[0].Other2.Value);
                        }

                        //foreach (var item in dt)
                        //{
                        //    if(e.Node.Key.ToString()==item.MenuID.Value.ToString())
                        //    {
                        //        e.Node.Selected = true;

                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {

            }
           

        }

        protected void treeListMenuQuyen_VirtualModeNodeCreating(object sender, DevExpress.Web.ASPxTreeList.TreeListVirtualModeNodeCreatingEventArgs e)
        {
            // gán view display ở đây
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["ID"];
            e.SetNodeValue("ID", node["ID"]);
            e.SetNodeValue("Name", node["Name"]);
            e.SetNodeValue("Name2", node["Name2"]);
            e.SetNodeValue("HasChild", node["HasChild"]);
            e.SetNodeValue("UserEdit", false);
            e.SetNodeValue("UserDelete", false);
            e.SetNodeValue("Other1", false);
            e.SetNodeValue("Other2", false);
            e.SetNodeValue("Other3", false);
            e.SetNodeValue("Other4", false);
        }

        protected void treeListMenuQuyen_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            tree.RefreshVirtualTree();
            tree.ExpandToLevel(1);

            // reset lại các checkbox node
            List<TreeListNode> nodes = treeListMenuQuyen.GetAllNodes().ToList();
            foreach (TreeListNode node in nodes)
            {
                TreeListDataColumn coledit = treeListMenuQuyen.Columns["UserEdit"] as TreeListDataColumn;
                ASPxCheckBox chk1 = tree.FindDataCellTemplateControl(node.Key, coledit, "editnode") as ASPxCheckBox;
                TreeListDataColumn coldelete = treeListMenuQuyen.Columns["UserDelete"] as TreeListDataColumn;
                ASPxCheckBox chk2 = tree.FindDataCellTemplateControl(node.Key, coldelete, "deletenode") as ASPxCheckBox;
                TreeListDataColumn colother1 = treeListMenuQuyen.Columns["Other1"] as TreeListDataColumn;
                ASPxCheckBox chk3 = tree.FindDataCellTemplateControl(node.Key, colother1, "other1node") as ASPxCheckBox;
                TreeListDataColumn colother2 = treeListMenuQuyen.Columns["Other2"] as TreeListDataColumn;
                ASPxCheckBox chk4 = tree.FindDataCellTemplateControl(node.Key, colother2, "other2node") as ASPxCheckBox;

                TreeListDataColumn colother3 = treeListMenuQuyen.Columns["Other3"] as TreeListDataColumn;
                ASPxCheckBox chk5 = tree.FindDataCellTemplateControl(node.Key, colother3, "other3node") as ASPxCheckBox;
                TreeListDataColumn colother4 = treeListMenuQuyen.Columns["Other4"] as TreeListDataColumn;
                ASPxCheckBox chk6 = tree.FindDataCellTemplateControl(node.Key, colother4, "other4node") as ASPxCheckBox;

                chk1.Checked = false;
                chk2.Checked = false;
                chk3.Checked = false;
                chk4.Checked = false;
                chk5.Checked = false;
                chk6.Checked = false;
            }
        }

        protected void treeListMenuQuyen_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlDataCellEventArgs e)
        {
            if (e.Level == 1)
                e.Cell.Font.Bold = true;
        }

        protected void cbPnthietlapquyen_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter.ToString() == "save")
            {
                int role = 0;
                if (hiddenQuyen.Contains("role"))
                {
                    role = Int32.Parse(hiddenQuyen.Get("role").ToString());
                }
                
                string data = "";
                //SqlHelper.ExecuteNonQuery(strconnUI, "[DM_QUYEN_DELETE]", role);

               
                if (treeListMenuQuyen.GetSelectedNodes().Count>0)
                {
                    try
                    {
                        using (var ctx = new ADDJContext())
                        {
                            var strSql = string.Format(@"delete from UserRight where AdminID={0}", role);
                            ctx.Database.ExecuteSqlCommand(strSql);

                            for (int j = 0; j < treeListMenuQuyen.GetSelectedNodes().Count; j++)
                            {
                                if (j == 0)
                                {
                                    data = data + treeListMenuQuyen.GetSelectedNodes()[j].Key.ToString();
                                }
                                else
                                {
                                    data = data + "," + treeListMenuQuyen.GetSelectedNodes()[j].Key.ToString();
                                }
                                var menuid = Convert.ToInt32(treeListMenuQuyen.GetSelectedNodes()[j].Key);

                               
                                var nodeKey = treeListMenuQuyen.GetSelectedNodes()[j].Key;
                                TreeListDataColumn coledit = treeListMenuQuyen.Columns["UserEdit"] as TreeListDataColumn;
                                var editnode_in = treeListMenuQuyen.FindDataCellTemplateControl(nodeKey, coledit, "editnode") as ASPxCheckBox;
                                var editnode = editnode_in.Checked;

                                TreeListDataColumn coldelete = treeListMenuQuyen.Columns["UserDelete"] as TreeListDataColumn;
                                var deletenode_in = treeListMenuQuyen.FindDataCellTemplateControl(nodeKey, coldelete, "deletenode") as ASPxCheckBox;
                                var deletenode = deletenode_in.Checked;


                                TreeListDataColumn colother1 = treeListMenuQuyen.Columns["Other1"] as TreeListDataColumn;
                                var other1node_in = treeListMenuQuyen.FindDataCellTemplateControl(nodeKey, colother1, "other1node") as ASPxCheckBox;
                                var other1node = other1node_in.Checked;

                                TreeListDataColumn colother2 = treeListMenuQuyen.Columns["Other2"] as TreeListDataColumn;
                                var other2node_in = treeListMenuQuyen.FindDataCellTemplateControl(nodeKey, colother2, "other2node") as ASPxCheckBox;
                                var other2node = other2node_in.Checked;

                                TreeListDataColumn colother3 = treeListMenuQuyen.Columns["Other3"] as TreeListDataColumn;
                                var other3node_in = treeListMenuQuyen.FindDataCellTemplateControl(nodeKey, colother3, "other3node") as ASPxCheckBox;
                                var other3node = other3node_in.Checked;

                                TreeListDataColumn colother4 = treeListMenuQuyen.Columns["Other4"] as TreeListDataColumn;
                                var other4node_in = treeListMenuQuyen.FindDataCellTemplateControl(nodeKey, colother4, "other4node") as ASPxCheckBox;
                                var other4node = other4node_in.Checked;


                                //var editnode = (bool)treeListMenuQuyen.GetSelectedNodes()[j]["UserEdit"];
                                //var deletenode = (bool)treeListMenuQuyen.GetSelectedNodes()[j]["UserDelete"];
                                //var other1node = (bool)treeListMenuQuyen.GetSelectedNodes()[j]["Other1"];
                                //var other2node = (bool)treeListMenuQuyen.GetSelectedNodes()[j]["Other2"];

                                var strSql111 = string.Format(@"
                        INSERT INTO UserRight 
                          (MenuID, AdminID, UserRead, UserEdit, UserDelete, Other1, Other2, Other3, Other4)      
	                 values ({0},{1},{2},{3},{4},{5},{6},{7},{8})", menuid, role, 1, editnode == true ? 1 : 0, deletenode == true ? 1 : 0, other1node == true ? 1 : 0, other2node == true ? 1 : 0, other3node == true ? 1 : 0, other4node == true ? 1 : 0);
                                ctx.Database.ExecuteSqlCommand(strSql111);

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    
                }

              
            }
        }

        protected void treeListMenuQuyen_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            try
            {
                ASPxTreeList tl = sender as ASPxTreeList;
                TreeListNode node = tl.FindNodeByKeyValue(e.NodeKey);
                if (e.RowKind == DevExpress.Web.ASPxTreeList.TreeListRowKind.Data)
                {
                    TreeListDataColumn coledit = treeListMenuQuyen.Columns["UserEdit"] as TreeListDataColumn;
                    ASPxCheckBox chk1 = tl.FindDataCellTemplateControl(e.NodeKey, coledit, "editnode") as ASPxCheckBox;
                    TreeListDataColumn coldelete = treeListMenuQuyen.Columns["UserDelete"] as TreeListDataColumn;
                    ASPxCheckBox chk2 = tl.FindDataCellTemplateControl(e.NodeKey, coldelete, "deletenode") as ASPxCheckBox;
                    TreeListDataColumn colother1 = treeListMenuQuyen.Columns["Other1"] as TreeListDataColumn;
                    ASPxCheckBox chk3 = tl.FindDataCellTemplateControl(e.NodeKey, colother1, "other1node") as ASPxCheckBox;
                    TreeListDataColumn colother2 = treeListMenuQuyen.Columns["Other2"] as TreeListDataColumn;
                    ASPxCheckBox chk4 = tl.FindDataCellTemplateControl(e.NodeKey, colother2, "other2node") as ASPxCheckBox;

                    TreeListDataColumn colother3 = treeListMenuQuyen.Columns["Other3"] as TreeListDataColumn;
                    ASPxCheckBox chk5 = tl.FindDataCellTemplateControl(e.NodeKey, colother3, "other3node") as ASPxCheckBox;

                    TreeListDataColumn colother4 = treeListMenuQuyen.Columns["Other4"] as TreeListDataColumn;
                    ASPxCheckBox chk6 = tl.FindDataCellTemplateControl(e.NodeKey, colother4, "other4node") as ASPxCheckBox;

                    var role = hiddenQuyen.Get("role").ToString();
                    using (var ctx = new ADDJContext())
                    {
                        string sqlStr = string.Format(@"SELECT * FROM UserRight a 
WHERE a.AdminID={0} and a.MenuId={1}", role, e.NodeKey);

                        var lsss = ctx.Database.SqlQuery<HT_QUYEN_NHOM>(sqlStr);
                        var dt = lsss.ToList();
                        if (dt.Any())
                        {

                            chk1.Checked = dt[0].UserEdit.Value;
                            chk2.Checked = dt[0].UserDelete.Value;
                            chk3.Checked = dt[0].Other1.Value;
                            chk4.Checked = dt[0].Other2.Value;
                            chk5.Checked = dt[0].Other3.Value;
                            chk6.Checked = dt[0].Other4.Value;
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
           
        }

    }
}