using ADDJ.Admin;
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
    public partial class mn_HtQuanLyNguoiDung : System.Web.UI.Page
    {
        public static int id_donvi = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    // Xử lý lấy thông tin đơn vị
                    id_donvi = loginInfo.PhongBanId;
                }
                ASPxHiddenField1["ID_DONVI"] = 0;
                napNhomNguoiDung();
            }

        }

        private void napNhomNguoiDung()
        {
            try
            {
                var lstNhomNSD = new List<HT_NHOM_NGUOIDUNG>();
                using (var ctx = new ADDJContext())
                {
                    var strSql = string.Format(@"select * from NguoiSuDung_Group");
                    lstNhomNSD = ctx.Database.SqlQuery<HT_NHOM_NGUOIDUNG>(strSql).ToList();
                }
                if (lstNhomNSD.Any())
                {
                    cboNhomNguoiDungEdit.DataSource = lstNhomNSD;
                    cboNhomNguoiDungEdit.ValueField = "Id";
                    cboNhomNguoiDungEdit.TextField = "Name";
                    cboNhomNguoiDungEdit.DataBind();

                    cboNhomNguoiDungAdd.DataSource = lstNhomNSD;
                    cboNhomNguoiDungAdd.ValueField = "Id";
                    cboNhomNguoiDungAdd.TextField = "Name";
                    cboNhomNguoiDungAdd.DataBind();
                }

            }
            catch (Exception ex)
            {

            }
        }

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
        
        //======================================================================================

        protected void grvNguoiDung_CustomDataCallback(object sender, DevExpress.Web.ASPxGridViewCustomDataCallbackEventArgs e)
        {
            Session.Remove("tblNguoiDung");
            grvNguoiDung.DataBind();
        }

        protected void grvNguoiDung_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblNguoiDung");
            grvNguoiDung.DataBind();
        }

        protected void grvNguoiDung_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblNguoiDung"] as List<HT_NGUOIDUNG2>;
            if (tbl == null)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        var strIDHeThong = "";
                        var idht = Convert.ToString(ASPxHiddenField1["ID_DONVI"]);
                        if (idht != "0" && idht != "")
                        {
                            strIDHeThong = string.Format(@" WHERE a.ID_DONVI={0} AND b.id is NOT NULL and a.XOA=0", idht);
                        }

                        var strSQl = @"SELECT b.Id
                                      ,b.TenDoiTac
                                      ,b.DoiTacId
                                      ,b.KhuVucId
                                      ,b.NhomNguoiDung
                                      ,b.TenTruyCap
                                      ,b.MatKhau
                                      ,b.TenDayDu
                                      ,b.NgaySinh
                                      ,b.DiaChi
                                      ,b.DiDong
                                      ,b.CoDinh
                                      ,b.Sex
                                      ,b.Email
                                      ,b.CongTy
                                      ,b.DiaChiCongTy
                                      ,b.FaxCongTy
                                      ,b.DienThoaiCongTy
                                      ,a.TrangThai
                                      ,b.SuDungLDAP
                                      ,b.LoginCount
                                      ,b.LastLogin
                                      ,b.IsLogin
                                      ,b.LDate
                                      ,b.CDate
                                      ,b.CUser
                                      ,b.LUser
                                      ,b.ID_DONVI 
                                      ,b.XOA
                                      ,a.ID ID_DONVI_NGUOIDUNG
                                      ,c.Name TenNhom
                                      ,d.ID ID_DONVI_ND
                                      ,b.MaNhanVienCMT
                                      FROM HT_NGUOIDUNG_DONVI a
                                      LEFT JOIN HT_NGUOIDUNG b ON a.ID_NGUOIDUNG=b.Id 
                                      LEFT JOIN NguoiSuDung_Group c on c.Id=b.NhomNguoiDung
                                      LEFT JOIN HT_DONVI d ON d.ID=a.ID_DONVI
                                      " + strIDHeThong;
                        var lstHoTro = ctx.Database.SqlQuery<HT_NGUOIDUNG2>(strSQl);
                        var lst = lstHoTro.ToList();
                        Session["tblNguoiDung"] = lst;
                        tbl = lst;
                    }
                }
            }
            grvNguoiDung.DataSource = tbl;
        }

        protected void grvNguoiDung_PageIndexChanged(object sender, EventArgs e)
        {
            grvNguoiDung.DataBind();
        }

        protected void grvNguoiDung_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "TrangThai")
            {
                object getvalTrangthai = e.GetFieldValue("TrangThai");
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

        protected void grvNguoiDung_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "SOBUOC")
            {
                e.Cell.BackColor = System.Drawing.Color.LightCoral;
                e.Cell.Font.Bold = true;
                e.Cell.HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.DataColumn.FieldName != "TrangThai") return;
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



        protected void grvNguoiDung_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            //if (e.VisibleIndex == -1) return;

            //var value = grvDmLuongXuLy.GetRowValues(e.VisibleIndex, "ID");
            //if (value.ToString() == "56")
            //    e.Visible = DefaultBoolean.False;

            //if (e.ButtonID == "Xoa" && e.VisibleIndex % 2 != 0)
            //    e.Visible = DefaultBoolean.False;
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQLDV = string.Format(@";WITH cte AS
                                                  (
      	                                            SELECT ID
      	                                            FROM dbo.HT_DONVI WHERE ID={0}      
      	                                            UNION ALL      
      	                                            SELECT t2.ID
      	                                            FROM cte t1
      	                                            JOIN dbo.HT_DONVI t2 ON t1.ID = t2.ID_CHA
                                                  )", id_donvi);

                    var strSQl = string.Format(@"  SELECT b.Id
                                      ,b.TenDoiTac
                                      ,b.DoiTacId
                                      ,b.KhuVucId
                                      ,b.NhomNguoiDung
                                      ,b.TenTruyCap
                                      ,b.MatKhau
                                      ,b.TenDayDu
                                      ,b.NgaySinh
                                      ,b.DiaChi
                                      ,b.DiDong
                                      ,b.CoDinh
                                      ,b.Sex
                                      ,b.Email
                                      ,b.CongTy
                                      ,b.DiaChiCongTy
                                      ,b.FaxCongTy
                                      ,b.DienThoaiCongTy
                                      ,a.TrangThai
                                      ,b.SuDungLDAP
                                      ,b.LoginCount
                                      ,b.LastLogin
                                      ,b.IsLogin
                                      ,b.LDate
                                      ,b.CDate
                                      ,b.CUser
                                      ,b.LUser
                                      ,b.ID_DONVI 
                                      ,b.XOA
                                      ,a.ID ID_DONVI_NGUOIDUNG
                                      ,c.Name TenNhom
                                      ,d.ID ID_DONVI_ND
                                      FROM HT_NGUOIDUNG_DONVI a
                                      LEFT JOIN HT_NGUOIDUNG b ON a.ID_NGUOIDUNG=b.Id 
                                      LEFT JOIN NguoiSuDung_Group c on c.Id=b.NhomNguoiDung
                                      LEFT JOIN HT_DONVI d ON d.ID=a.ID_DONVI
                                      where b.TenTruyCap like N'%{0}%' and a.XOA=0
                                      and d.ID in(select * from cte) ",
                                      txtTenDangNhap.Text.Trim());
                    strSQl = strSQLDV + strSQl;
                    var lstHoTro = ctx.Database.SqlQuery<HT_NGUOIDUNG2>(strSQl);
                    var lst = lstHoTro.ToList();
                    Session["tblNguoiDung"] = lst;
                    grvNguoiDung.DataBind();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}