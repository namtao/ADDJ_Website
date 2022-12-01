using ADDJ.Admin;
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
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro.Dashboards
{
    public partial class DanhSachYeuCauHT : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterRequiresControlState(ASPxPager1);
            if (!IsPostBack)
            {
                HiddenFieldYC["hidden_value"] = 0;
                HiddenFieldYC["res_value"] = 0;
                HiddenFieldYC["hidden_value2"] = 0;



                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_HE_THONG_HOTROs where ss.TRANGTHAI.Value==true
                             select ss;
                    dtbCheckUpdateSMSSList = kl.ToList();
                }
                cboChonHeThongYCHTHT.DataSource = dtbCheckUpdateSMSSList;
                cboChonHeThongYCHTHT.TextField = "TENHETHONG";
                cboChonHeThongYCHTHT.ValueField = "ID";
                cboChonHeThongYCHTHT.DataBind();

                cboChonHeThongYCHTHT.Items.Insert(0, new ListEditItem("-- chọn hệ thống ---", 0));
                cboChonHeThongYCHTHT.SelectedIndex = 0;


                var dtbChonMucDo = new List<HT_MUCDO_SUCO>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_MUCDO_SUCOs where ss.TRANGTHAI==1
                             select ss;
                    dtbChonMucDo = kl.ToList();
                }

                cboMucDoYeuCauHT.DataSource = dtbChonMucDo;
                cboMucDoYeuCauHT.TextField = "TENMUCDO";
                cboMucDoYeuCauHT.ValueField = "ID";
                cboMucDoYeuCauHT.DataBind();

                cboMucDoYeuCauHT.Items.Insert(0, new ListEditItem("-- chọn mức độ ---", 0));
                cboMucDoYeuCauHT.SelectedIndex = 0;


                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    hiddenYCHT["nguoixuly"] = loginInfo.Username;
                    hiddenYCHT["donvixuly"] = loginInfo.PhongBanId;
                }

                BindData();
            }

            var ddd1 = Convert.ToInt32(HiddenFieldYC["res_value"]);
            var ddd2 = Convert.ToInt32(HiddenFieldYC["hidden_value2"]);
            if (Convert.ToInt32(HiddenFieldYC["res_value"]) != 0 && Convert.ToInt32(HiddenFieldYC["hidden_value2"]) != 0)
            {
                ASPxTreeList hdfData = ((ASPxTreeList)LoaiHoTroYC.FindControl("treelist_donvi_ccYC"));
                hdfData.RefreshVirtualTree();
            }
        }


        protected void treelist_donvi_cc_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
        {
            HiddenFieldYC["res_value"] = 0;
            HiddenFieldYC["hidden_value2"] = 0;
            object sekkkk = HiddenFieldYC["hidden_value"];
            object sekkkksss = HiddenFieldYC["res_value"];
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
			                                    where a.ID_CHA = hhthtc.ID  and a.TRANGTHAI=1
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
                        string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,
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



        protected void ASPxGridView1_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            //if (e.VisibleIndex == -1) return;
            //var ishoatdong = ASPxGridView2.GetRowValues(e.VisibleIndex, "TRANG_THAI");
            //if (e.ButtonID == "Xem" && Convert.ToInt32(ishoatdong) == 0)
            //    e.Visible = DefaultBoolean.False;
        }

        public static string idHeThong;
        protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //Session.Remove("tblHeThongXL");
            //if (e.Parameters != "" && e.Parameters != "0")
            //    idHeThong = e.Parameters; // e.Parameters[0].ToString();
            //else
            //    idHeThong = "";
            //ASPxGridView1.DataBind();
        }

        protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
        {
            //var tbl = Session["tblHeThongXL"] as List<HT_DM_YEUCAU_HOTRO_HT3>;
            //if (tbl == null)
            //{
            //    AdminInfo loginInfo = LoginAdmin.AdminLogin();
            //    if (loginInfo != null)
            //    {
            //        using (var ctx = new HTHTKTContext())
            //        {
            //            var strWhereIdHeThong = "";
            //            if (idHeThong != "")
            //            {
            //                var param = idHeThong.Split('|');
            //                var idhethong = param[0];
            //                var mucdo = param[1];
            //                var loaihotro = param[2];
            //                var sodienthoai = param[3];

            //                if (idhethong != "0" && idhethong != "")
            //                    strWhereIdHeThong = strWhereIdHeThong + @" and a.ID_HETHONG_YCHT=" + idhethong;
            //                if (mucdo != "0" && mucdo != "")
            //                    strWhereIdHeThong = strWhereIdHeThong + @" and a.ID_MUCDO_SUCO=" + mucdo;
            //                if (loaihotro != "0" && loaihotro != "")
            //                    strWhereIdHeThong = strWhereIdHeThong + @" and a.ID_CAYTHUMUC_YCHT=" + loaihotro;
            //                if (sodienthoai != "0" && sodienthoai != "")
            //                    strWhereIdHeThong = strWhereIdHeThong + string.Format(@" and a.SODIENTHOAI LIKE N'%{0}%'", sodienthoai); ;
            //            }

            //            //var strSQl = "select * from HT_DM_YEUCAU_HOTRO_HT where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";
            //            var strSQl = @"select a.ID,
            //                        a.SODIENTHOAI,a.MA_YEUCAU,
            //                        a.NOIDUNG_YEUCAU,
            //                        a.NOIDUNG_XL_DONG_HOTRO,
            //                        a.TRANGTHAI,
            //                        a.NGUOITAO,
            //                        b.TEN_LUONG,
            //                        b.MOTA,
            //                        b.SOBUOC,
            //                        c.TENHETHONG,
            //                        d.LINHVUC,
            //                        d.GHICHU,
            //                        e.TENMUCDO
            //                               from HT_DM_YEUCAU_HOTRO_HT a
            //                            INNER JOIN HT_LUONG_HOTRO b ON b.ID = a.ID_LUONG_HOTRO
            //                            LEFT JOIN HT_DM_HETHONG_YCHT c on c.ID = a.ID_HETHONG_YCHT
            //                            LEFT JOIN HT_CAYTHUMUC_YCHT d ON d.ID = a.ID_CAYTHUMUC_YCHT
            //                            left join HT_MUCDO_SUCO e on e.ID=a.ID_MUCDO_SUCO
            //                              where a.nguoitao = '" + loginInfo.Username + "' " + strWhereIdHeThong +
            //                              "  order by a.ngaytao desc";
            //            var lstHoTro = ctx.Database.SqlQuery<HT_DM_YEUCAU_HOTRO_HT3>(strSQl);
            //            var lst = lstHoTro.ToList();
            //            Session["tblHeThongXL"] = lst;
            //            tbl = lst;
            //        }
            //    }
            //}
            //ASPxGridView1.DataSource = tbl;
        }

        protected void ASPxGridView1_PageIndexChanged(object sender, EventArgs e)
        {
            //ASPxGridView1.DataBind();
        }

        protected void ASPxGridView1_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "TRANGTHAI")
            {
                object getvalTrangthai = e.GetFieldValue("TRANGTHAI");
                switch (Convert.ToInt32(getvalTrangthai))
                {
                    case 4:
                        e.DisplayText = "Xử lý xong";
                        break;
                    case 3:
                        e.DisplayText = "Chờ xác nhận...";
                        break;
                    case 2:  // trạng thái này dành cho user đã phản hồi 1 vòng về người yêu cầu hỗ trợ để xác nhận
                        e.DisplayText = "Chuyển phản hồi...";
                        break;
                    case 1:
                        e.DisplayText = "Chuyển tiếp...";
                        break;
                    case 0:
                        e.DisplayText = "Khởi tạo và chuyển tiếp.";
                        break;
                    default:
                        break;
                }
            }
        }

        protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "TRANGTHAI") return;
            switch (Convert.ToInt32(e.CellValue))
            {
                case 4://4: Người ra yêu cầu xác nhận ok
                    e.Cell.BackColor = System.Drawing.Color.Green;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    break;
                case 3://3: Chờ xác nhận của người yêu cầu hỗ trợ, trạng thái này dành cho user đã phản hồi 1 vòng về người yêu cầu hỗ trợ để xác nhận
                    e.Cell.BackColor = System.Drawing.Color.Magenta;
                    break;
                case 2:  //2: Chuyển phản hồi
                    e.Cell.BackColor = System.Drawing.Color.Maroon;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    break;
                case 1://1: Chuyển tiếp
                    e.Cell.BackColor = System.Drawing.Color.LightCoral;
                    break;
                case 0://0: khởi tạo,chuyen tiep
                    e.Cell.BackColor = System.Drawing.Color.LightCyan;
                    break;
                default:
                    break;
            }
        }

        //protected void ASPxGridView1_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType != GridViewRowType.Data) return;
        //    int trangthai = Convert.ToInt32(e.GetValue("TRANGTHAI"));
        //    if (trangthai == 1)
        //    {
        //        e.Row.BackColor = System.Drawing.Color.LightCoral;
        //    }
        //    else
        //    {
        //        e.Row.BackColor = System.Drawing.Color.Green;
        //    }
        //}




        // Paging data

        public void BindData()
        {
            int count = 0;
            //DataTable dtSource = DBUtils.GetDataPaging(PageSize, CurrentPage, ref count);
            var lstl = GetDataPaging(PageSize, CurrentPage);
            if (lstl.Count > 0)
            {
                panelPaging.Visible = true;
                this.RowCount = lstl[0].ToltalRecords.Value;
                ASPxPager1.ItemCount = lstl[0].ToltalRecords.Value;
                ASPxPager1.ItemsPerPage = this.PageSize;
                ASPxGridView1.DataSource = lstl;
            }
            else
            {
                panelPaging.Visible = false;
                ASPxGridView1.DataSource = null;
            }
            ASPxGridView1.DataBind();
        }
        protected void ASPxPager1_PageIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = ASPxPager1.PageIndex;
            BindData();
        }

        public List<HT_DM_YEUCAU_HOTRO_HT3> GetDataPaging(int pagesize, int currentpage)
        {
            var lsthh = new List<HT_DM_YEUCAU_HOTRO_HT3>();
            try
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {

                        var strWhereIdHeThong = "";
                      //  if (idHeThong != "" && idHeThong!=null)
                      //  {
                            //var param = idHeThong.Split('|');
                            var idhethong = cboChonHeThongYCHTHT.Value.ToString();
                            var mucdo = cboMucDoYeuCauHT.Value.ToString();
                        var loaihotro = LoaiHoTroYC.KeyValue == null ? "0" : LoaiHoTroYC.KeyValue.ToString();
                            var sodienthoai = txtSoDienThoaiHT.Text;

                            if (idhethong != "0" && idhethong != "")
                                strWhereIdHeThong = strWhereIdHeThong + @" and a.ID_HETHONG_YCHT=" + idhethong;
                            if (mucdo != "0" && mucdo != "")
                                strWhereIdHeThong = strWhereIdHeThong + @" and a.ID_MUCDO_SUCO=" + mucdo;
                            if (loaihotro != "0" && loaihotro != "")
                                strWhereIdHeThong = strWhereIdHeThong + @" and a.ID_CAYTHUMUC_YCHT=" + loaihotro;
                            if (sodienthoai != "0" && sodienthoai != "")
                                strWhereIdHeThong = strWhereIdHeThong + string.Format(@" and a.SODIENTHOAI LIKE N'%{0}%'", sodienthoai); ;
                      //  }

                        //var strSQl = "select * from HT_NODE_LUONG_HOTRO0 where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";
                        var strSQl = string.Format(@";with query as
                                    (
                                     select a.ID,
                                    a.SODIENTHOAI,a.MA_YEUCAU,
                                    a.NOIDUNG_YEUCAU,
                                    a.NOIDUNG_XL_DONG_HOTRO,
                                    a.TRANGTHAI,
                                    a.NGUOITAO,
                                    b.TEN_LUONG,
                                    b.MOTA,
                                    b.SOBUOC,
                                    c.TENHETHONG,
                                    d.LINHVUC,
                                    d.GHICHU,
                                    e.TENMUCDO,ROW_NUMBER() OVER(ORDER BY a.ID ASC) as line 
                                    from HT_DM_YEUCAU_HOTRO_HT a
                                        INNER JOIN HT_LUONG_HOTRO b ON b.ID = a.ID_LUONG_HOTRO
                                        LEFT JOIN HT_DM_HETHONG_YCHT c on c.ID = a.ID_HETHONG_YCHT
                                        LEFT JOIN HT_CAYTHUMUC_YCHT d ON d.ID = a.ID_CAYTHUMUC_YCHT
                                        left join HT_MUCDO_SUCO e on e.ID=a.ID_MUCDO_SUCO
                                   where a.nguoitao = '{3}' {4}) 
                                    --order by clause is required to use offset-fetch
                                    select query.ID,
                                    query.SODIENTHOAI,
                                    query.MA_YEUCAU,
                                    query.NOIDUNG_YEUCAU,
                                    query.NOIDUNG_XL_DONG_HOTRO,
                                    query.TRANGTHAI,
                                    query.NGUOITAO,
                                    query.TEN_LUONG,
                                    query.MOTA,
                                    query.SOBUOC,
                                    query.TENHETHONG,
                                    query.LINHVUC,
                                    query.GHICHU,
                                    query.TENMUCDO,
                                           query.line 
	                                       ,tCountOrders.CountOrders ToltalRecords 
                                    from query CROSS JOIN (SELECT Count(*) AS CountOrders FROM query) AS tCountOrders
                                    order by query.ID 
                                    offset (({0} - 1) * {1}) rows
                                    fetch next {2} rows only", currentpage + 1, pagesize, pagesize, loginInfo.Username, strWhereIdHeThong);
                        var lstHoTro = ctx.Database.SqlQuery<HT_DM_YEUCAU_HOTRO_HT3>(strSQl);
                        lsthh = lstHoTro.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return lsthh;
        }


        public int PageSize
        {
            get
            {
                return (base.ViewState["PageSize"] != null) ? (int)base.ViewState["PageSize"] : 10;
            }
            set
            {
                base.ViewState["PageSize"] = value;
            }
        }
        public int CurrentPage
        {
            get
            {
                return (base.ViewState["CurrentPage"] != null) ? (int)base.ViewState["CurrentPage"] : 0;
            }
            set
            {
                base.ViewState["CurrentPage"] = value;
            }
        }
        public int RowCount
        {
            get { return (base.ViewState["RowCount"] != null) ? (int)base.ViewState["RowCount"] : 0; }
            set { base.ViewState["RowCount"] = value; }
        }

        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
            base.ViewState["PageSize"] = Convert.ToInt32(cboPageSize.Value);
            BindData();
        }

        // End paging data

        protected void btnTimKiemYC_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}