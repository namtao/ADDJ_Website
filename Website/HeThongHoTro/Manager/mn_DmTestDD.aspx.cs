using AIVietNam.Admin;
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
    public partial class mn_DmTestDD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ASPxHiddenField1["hidden_value"] = 0;
                ASPxHiddenField1["res_value"] = 0;
                ASPxHiddenField1["hidden_value2"] = 0;
                ASPxHiddenField1["idluonghotro"] = 0;

                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new HTHTKTContext())
                {
                    var strsQl = @"SELECT * FROM HT_HTKTTT.dbo.HT_DM_HETHONG_YCHT";
                    dtbCheckUpdateSMSSList = ctx.Database.SqlQuery<HT_DM_HETHONG_YCHT>(strsQl).ToList();
                }
                cboDanhSachHeThongCanYeuCauHoTro.DataSource = dtbCheckUpdateSMSSList;
                cboDanhSachHeThongCanYeuCauHoTro.TextField = "TENHETHONG";
                cboDanhSachHeThongCanYeuCauHoTro.ValueField = "ID";
                cboDanhSachHeThongCanYeuCauHoTro.DataBind();
            }

            var ddd1 = Convert.ToInt32(ASPxHiddenField1["res_value"]);
            var ddd2 = Convert.ToInt32(ASPxHiddenField1["hidden_value2"]);
            var id_luonghotro = Convert.ToInt32(ASPxHiddenField1["idluonghotro"]);
            if (Convert.ToInt32(ASPxHiddenField1["res_value"]) != 0 && Convert.ToInt32(ASPxHiddenField1["hidden_value2"]) != 0)
            {
                // lấy thông tin luồng hỗ trợ
                var dtbCheckUpdateSMSSList = new List<HT_LUONG_HOTRO>();
                using (var ctx = new HTHTKTContext())
                {
                    var strsQl = @"SELECT * FROM HT_HTKTTT.dbo.HT_LUONG_HOTRO where ID=" + id_luonghotro;
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
            }
        }

        protected void grvNodeLuongXuLy_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            grvNodeLuongXuLy.DataBind();
        }

        protected void grvNodeLuongXuLy_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<HT_NODE_LUONG_HOTRO0>;
            if (tbl == null)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new HTHTKTContext())
                    {
                        //var strSQl = "select * from HT_NODE_LUONG_HOTRO0 where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";
                        var strSQl = @"SELECT * FROM dbo.HT_NODE_LUONG_HOTRO";
                        var lstHoTro = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO0>(strSQl);
                        var lst = lstHoTro.ToList();
                        Session["tblHeThongXL"] = lst;
                        tbl = lst;
                    }
                }
            }
            grvNodeLuongXuLy.DataSource = tbl;
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
                using (var ctx = new HTHTKTContext())
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

                sqlWhere = " and Id=" + Convert.ToInt32(sekkkk);
            }
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
                using (var ctx = new HTHTKTContext())
                {
                    var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                             where ss.ID_CHA == null
                             select ss;
                    string sqlStr = @"SELECT hhthtc.ID,hhthtc.TenDoiTac,
  HasChild = cast((case when exists(
			select *
			from dbo.DoiTac a
			where a.DonViTrucThuoc = hhthtc.ID  
		) then 1 else 0 end) as bit)
  from dbo.DoiTac hhthtc
  WHERE hhthtc.DonViTrucThuoc =0 OR hhthtc.DonViTrucThuoc IS NULL " + sqlWhere;
                    var lsss = ctx.Database.SqlQuery<ViewCayThuMucDoiTac>(sqlStr);
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
                    using (var ctx = new HTHTKTContext())
                    {
                        var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                                 where ss.ID_CHA == null
                                 select ss;
                        string sqlStr = @"SELECT hhthtc.ID,hhthtc.TenDoiTac,
  HasChild = cast((case when exists(
			select *
			from dbo.DoiTac a
			where a.DonViTrucThuoc = hhthtc.ID  
		) then 1 else 0 end) as bit)
  from dbo.DoiTac hhthtc
  WHERE hhthtc.DonViTrucThuoc =" + id_cha + sqlWhere;
                        var lsss = ctx.Database.SqlQuery<ViewCayThuMucDoiTac>(sqlStr);
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
            e.SetNodeValue("TenDoiTac", node["TenDoiTac"]);
        }
        protected void treelist_donvi_cc_OnCustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            tree.RefreshVirtualTree();
            tree.ExpandToLevel(1);
        }

    }
}