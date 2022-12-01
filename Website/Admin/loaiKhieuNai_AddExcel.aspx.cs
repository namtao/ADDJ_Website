using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using Aspose.Cells;
using System.Web;
using System.Data.OleDb;
using System.Text;

public partial class loaiKhieuNai_AddExcel : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownlist();

            grvView.PageSize = Config.RecordPerPage;
            BindGrid(true);
        }
    }

    protected string HanhDong(object id)
    {
        return string.Format("<a href='phongBan_add.aspx?ID={0}'>Sửa</a> | <a href='phongBan_User_add.aspx?ID={0}'>Thêm, xóa user</a> | <a href='phongBan2PhongBan.aspx?PhongBanId={0}'>Thêm, xóa Phòng ban chuyển xử lý</a> | <a href='PhanQuyenPhongBan.aspx?PhongBanId={0}'>Phân quyền</a>", id);
    }

    protected string BindDoiTac(object doitacId)
    {
        return DoiTacImpl.ListDoiTac.Where(t => t.Id == Convert.ToInt32(doitacId)).FirstOrDefault().MaDoiTac;
    }

    protected string BindCountNguoiDung(object id, object countUser)
    {
        return string.Format("{0} (<a href='nguoiSuDung_manager.aspx?PhongBanId={1}'>Xem danh sách</a>)", countUser, id);
    }

    private void BindDropDownlist()
    {
        var admin = LoginAdmin.AdminLogin();
        string whereClause = "DoiTacId=" + admin.DoiTacId;
        whereClause += " or DoiTacId in (select Id from DoiTac where DonViTrucThuoc = " + admin.DoiTacId + ")";
        whereClause += " or DoiTacId in (select Id from DoiTac where DonViTrucThuoc in (select Id from DoiTac where DonViTrucThuoc = " + admin.DoiTacId + "))";

        var lstLoaiPhongBan = ServiceFactory.GetInstanceLoaiPhongBan().GetList();
        ddlLoaiPhongBan.DataSource = lstLoaiPhongBan;
        ddlLoaiPhongBan.DataTextField = "Name";
        ddlLoaiPhongBan.DataValueField = "Id";
        ddlLoaiPhongBan.DataBind();
    }

    private void BindGrid(bool isClearFilter)
    {
        try
        {
            //if (isClearFilter)
            //{
            //    txtKeyword.Text = "";
            //    ddlDonVi.SelectedIndex = 0;
            //}
            //string selectClause = string.Empty;
            //selectClause = "*,(select count(*) from PhongBan_User where PhongBanId = PhongBan.Id) CountUser";

            //string whereClause = "1 = 1 ";
            //whereClause += string.Format(" AND (DoiTacId = {0} or DoiTacId in ( select id from DoiTac where DonViTrucThuoc = {0} or Id in ( select Id from DoiTac where DonViTrucThuoc in (  select id from DoiTac where DonViTrucThuoc = {0} ) ) ))", ddlDonVi.SelectedValue);

            //if (!txtKeyword.Text.Trim().Equals(""))
            //{
            //    whereClause += string.Format(" AND Name like N'%{0}%'",txtKeyword.Text.Trim());
            //}

            //if (!ddlLoaiPhongBan.SelectedValue.ToString().Equals("0"))
            //{
            //    whereClause += string.Format(" AND LoaiPhongBanId = {0}", ddlLoaiPhongBan.SelectedValue);
            //}

            //string orderbyClause = string.Empty;

            //var phongBanObj = ServiceFactory.GetInstancePhongBan().GetListDynamic(selectClause, whereClause, orderbyClause);
            //if (phongBanObj != null && phongBanObj.Count > 0)
            //{
            //    ltThongBao.Text = "<font color='red'>Có " + phongBanObj.Count + " phòng ban được tìm thấy.</font>";
            //    grvView.DataSource = phongBanObj;
            //    grvView.DataBind();
            //}
            //else
            //{
            //    ltThongBao.Text = "<font color='red'>Không có phòng ban được tìm thấy.</font>";
            //    grvView.DataSource = null;
            //    grvView.DataBind();
            //}

            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }


    private void RowDataBound(GridViewRowEventArgs e)
    {
        if (e.Row.DataItemIndex != -1)
        {

            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        grvView.PageIndex = e.NewPageIndex;
        BindGrid(false);
    }

    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RowDataBound(e);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }
 
    protected void btUpdateDonViTiepNhanXL_Click(object sender, EventArgs e)
    {
        var dt = ViewState["TableImport"] as DataTable;

        try
        {
            List<LoaiKhieuNaiInfo> lstLoaiKn = ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
            //ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "", "Sort");
            int sortLoaiKn = 0;
            int sortLinhVucChung = 1000;
            int sortLinhVucCon = 1;

            var i = 0;

            foreach (DataRow row in dt.Rows)
            {
                var loaiKn = row["Các loại khiếu nại"].ToString().ToLower().Trim();
                var linhVucChung = row["Lĩnh vực chung"].ToString().ToLower().Trim();
                var linhVucCon = row["Lĩnh vực con"].ToString().ToLower().Trim();
                var tgxlpb = row["TGXLPB"].ToString().Trim();
                var donvitiepn = row["Đơn vị tiếp nhận xử lý"].ToString().ToLower().Trim();

                var tgxlpbXuly = row["TGXLPB_XuLy"].ToString().Trim();
                var tgxlpbHotro = row["TGXLPB_HoTro"].ToString().Trim();
                var tgxlpbHotro2 = row["TGXLPB_HoTro2"].ToString().Trim();
                //nếu không đầy đủ thông tin thời gian sẽ không xử lý
                if (string.IsNullOrEmpty(tgxlpb) || string.IsNullOrEmpty(tgxlpbXuly) ||
                    string.IsNullOrEmpty(tgxlpbHotro) || string.IsNullOrEmpty(tgxlpbHotro2))
                {
                    continue;
                }
                var thoiGianXl = "20d";

                var filterLoaiKn = lstLoaiKn.Where(t => t.Name.ToLower().Equals(loaiKn.ToLower()) && t.ParentId == 0);
                //Loại khiếu nại đã có
                if (filterLoaiKn.Any())
                {
                    //cập nhật thông tin cho loại kn
                    var itemlkn = filterLoaiKn.FirstOrDefault();
                    //itemlkn.Name = row["Các loại khiếu nại"].ToString().Trim();
                    //itemlkn.ThoiGianCanhBao = thoiGianXl;
                    //itemlkn.ThoiGianUocTinh = thoiGianXl;
                    //itemlkn.Sort = sortLoaiKn;
                    //itemlkn.Test = 1;
                    //itemlkn.ParentLoaiKhieuNaiId = itemlkn.Id;

                    itemlkn.DonViTiepNhanXL = donvitiepn;

                    ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlkn);
                    var LoaiKNItem = filterLoaiKn.FirstOrDefault();
                    var filterLinhVucChung =
                        lstLoaiKn.Where(t => t.Name.ToLower().Equals(linhVucChung.ToLower()) && t.ParentId == LoaiKNItem.Id);
                    //lĩnh vực chung đã có
                    if (filterLinhVucChung.Any())
                    {
                        //cập nhật thông tin lĩnh vực chung
                        var itemlvch = filterLinhVucChung.FirstOrDefault();
                        itemlvch.ThoiGianCanhBao = thoiGianXl;
                        itemlvch.ThoiGianUocTinh = thoiGianXl;
                        itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemlvch.Test = 1;
                        itemlvch.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                        ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvch);

                        var filterLinhVucCon =
                            lstLoaiKn.Where(
                                t =>
                                    t.Name.ToLower().Equals(linhVucCon.ToLower()) &&
                                    t.ParentId == itemlvch.Id);
                        //lĩnh vực con đã có
                        if (filterLinhVucCon.Any())
                        {
                            //cập nhật lĩnh vực con
                            var itemlvc = filterLinhVucCon.FirstOrDefault();
                            itemlvc.Name = linhVucCon;
                            itemlvc.ThoiGianCanhBao = thoiGianXl;
                            itemlvc.ThoiGianUocTinh = thoiGianXl;
                            itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            itemlvc.Test = 1;
                            itemlvc.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                            ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvc);
                            sortLinhVucCon++;
                        }
                        //tên lĩnh vực con thỏa mãn, và chưa có -> thêm mới lĩnh vực con
                        else if (CheckValid(linhVucCon) && !filterLinhVucCon.Any())
                        {
                            var item = new LoaiKhieuNaiInfo();
                            item.Cap = 3;
                            item.Description = "";
                            item.Name = linhVucCon;
                            item.ThoiGianCanhBao = thoiGianXl;
                            item.ThoiGianUocTinh = thoiGianXl;
                            item.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            item.Test = 1;
                            item.Status = 1;
                            item.ParentId = itemlvch.Id;
                            item.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                            item.NameLoaiKhieuNai = LoaiKNItem.NameLoaiKhieuNai;
                            if (item.ParentId != 0 &&
                                !lstLoaiKn.Any(
                                    p => p.Cap == item.Cap && p.Name.ToLower().Equals(item.Name.ToLower()) && p.ParentId == item.ParentId))
                            {
                                item.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(item);
                                lstLoaiKn.Add(item);
                            }
                        }
                    }
                    //tên lĩnh vực chung thỏa mãn và chưa có -> thêm mới lĩnh vực chung
                    else if (CheckValid(linhVucChung) && !filterLinhVucChung.Any())
                    {
                        var itemlvch = new LoaiKhieuNaiInfo();
                        sortLinhVucChung += 1000;

                        itemlvch.Cap = 2;
                        itemlvch.Description = "";
                        itemlvch.Name = linhVucChung;
                        itemlvch.ThoiGianCanhBao = thoiGianXl;
                        itemlvch.ThoiGianUocTinh = thoiGianXl;
                        itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemlvch.Test = 1;
                        itemlvch.Status = 1;
                        itemlvch.ParentId = itemlkn.Id;
                        itemlvch.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                        if (itemlvch.ParentId != 0 && !lstLoaiKn.Any(
                                p => p.Cap == itemlvch.Cap && p.Name.ToLower().Equals(itemlvch.Name.ToLower()) && p.ParentId == itemlvch.ParentId))
                        {
                            itemlvch.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemlvch);
                            lstLoaiKn.Add(itemlvch);
                        }

                        //tên lĩnh vực con thỏa mãn-> thêm mới
                        else if (CheckValid(linhVucCon))
                        {
                            var itemlvc = new LoaiKhieuNaiInfo();
                            itemlvc.Cap = 3;
                            itemlvc.Description = "";
                            itemlvc.Name = linhVucCon;
                            itemlvc.ThoiGianCanhBao = thoiGianXl;
                            itemlvc.ThoiGianUocTinh = thoiGianXl;
                            itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            itemlvc.Test = 1;
                            itemlvc.Status = 1;
                            itemlvc.ParentId = itemlvch.Id;
                            itemlvc.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                            itemlvc.NameLoaiKhieuNai = LoaiKNItem.NameLoaiKhieuNai;
                            if (itemlvc.ParentId != 0 &&
                                !lstLoaiKn.Any(
                                    p =>
                                        p.Cap == itemlvc.Cap && p.Name.ToLower().Equals(itemlvc.Name.ToLower()) && p.ParentId == itemlvc.ParentId))
                            {
                                itemlvc.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemlvc);
                                lstLoaiKn.Add(itemlvc);
                            }
                            sortLinhVucCon++;
                        }
                    }
                }
                //loại khiếu nại thỏa mãn và chưa có-> thêm mới
                else if (CheckValid(loaiKn))
                {
                    var itemLoaiKN = new LoaiKhieuNaiInfo();
                    itemLoaiKN.Cap = 1;
                    itemLoaiKN.Description = "";
                    itemLoaiKN.Name = loaiKn;
                    itemLoaiKN.ThoiGianCanhBao = thoiGianXl;
                    itemLoaiKN.ThoiGianUocTinh = thoiGianXl;
                    itemLoaiKN.Sort = sortLoaiKn;
                    itemLoaiKN.Status = 1;
                    itemLoaiKN.ParentId = 0;
                    //add
                    itemLoaiKN.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemLoaiKN);
                    lstLoaiKn.Add(itemLoaiKN);
                    // lĩnh vực chung thỏa mãn-> thêm lĩnh vực chung
                    if (itemLoaiKN.Id > 0 && CheckValid(linhVucChung))
                    {
                        var itemLvch = new LoaiKhieuNaiInfo();
                        sortLinhVucChung = 1000;
                        itemLvch.Cap = 2;
                        itemLvch.Description = "";
                        itemLvch.Name = row["Lĩnh vực chung"].ToString().Trim();
                        itemLvch.ThoiGianCanhBao = thoiGianXl;
                        itemLvch.ThoiGianUocTinh = thoiGianXl;
                        itemLvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemLvch.Test = 1;
                        itemLvch.Status = 1;
                        itemLvch.ParentId = itemLoaiKN.Id;
                        itemLvch.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                        //add new if not exist from table LoaiKhieuNai
                        itemLvch.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemLvch);
                        lstLoaiKn.Add(itemLvch);
                        //lĩnh vực con thỏa mãn-> thêm mới lĩnh vực con
                        if (itemLvch.Id > 0 && CheckValid(linhVucCon))
                        {
                            var itemLVCon = new LoaiKhieuNaiInfo();
                            itemLVCon.Cap = 3;
                            itemLVCon.Description = "";
                            itemLVCon.Name = row["Lĩnh vực con"].ToString().Trim();
                            itemLVCon.ThoiGianCanhBao = thoiGianXl;
                            itemLVCon.ThoiGianUocTinh = thoiGianXl;
                            itemLVCon.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            itemLVCon.Test = 1;
                            itemLVCon.Status = 1;
                            itemLVCon.ParentId = itemLvch.Id;
                            itemLVCon.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                            itemLVCon.NameLoaiKhieuNai = itemLoaiKN.NameLoaiKhieuNai;
                            //add new if not exist from table LoaiKhieuNai
                            itemLVCon.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemLVCon);
                            lstLoaiKn.Add(itemLVCon);
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            lbMessage.Text = ex.Message;
        }

    }
 
    public bool UpdateMa(string filelocation, Dictionary<int, int> dataInsert, string tableInsert)
    {
        OleDbCommand excelCommand = null;
        bool result = false;
        string excelConnStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filelocation +
                                      "; Extended Properties =Excel 8.0;";

        var excelConn = new OleDbConnection(excelConnStr);

        excelConn.Open();

        try
        {
            foreach (var item in dataInsert)
            {
                excelCommand = new OleDbCommand();
                var sb = new StringBuilder();

                sb.Append("Update [" + tableInsert +
                          "$] set [Id]=" + item.Value + " where Id2=" + item.Key);

                excelCommand.CommandText = sb.ToString();
                excelCommand.CommandType = CommandType.Text;
                excelCommand.Connection = excelConn;
                excelCommand.ExecuteNonQuery();
            }
            result = true;
        }
        catch (Exception ex)
        {
            result = false;
        }
        finally
        {
            if (excelCommand != null)
                excelCommand.Dispose();
            if (excelConn.State == ConnectionState.Open)
            {
                excelConn.Dispose();
                excelConn.Close();
            }
        }

        return result;
    }

   
    /// <summary>
    /// truongvv 25/05/2016
    /// update theo mẫu mới
    /// </summary>
    private void xuLyExcelLoaiKNtoDB()
    {
        var dt = ViewState["TableImport"] as DataTable;

        try
        {
            List<LoaiKhieuNaiInfo> lstLoaiKn;
            List<LoaiKhieuNaiInfo> lstLinhVucChung;
            List<LoaiKhieuNaiInfo> lstLinhVucCon;
            //ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "", "Sort");
            int sortLoaiKn = 0;
            int sortLinhVucChung = 100;
            int sortLinhVucCon = 1;

            var i = 0;

            foreach (DataRow row in dt.Rows)
            {
               
                // dùng để xử lý
                var loaiKn = row["Các loại khiếu nại"].ToString().ToLower().Trim();
                var linhVucChung = row["Lĩnh vực chung"].ToString().ToLower().Trim();
                var linhVucCon = row["Lĩnh vực con"].ToString().ToLower().Trim();

                // dùng để add vào db
                var loaiKn2 = row["Các loại khiếu nại"].ToString().Trim();
                var linhVucChung2 = row["Lĩnh vực chung"].ToString().Trim();
                var linhVucCon2 = row["Lĩnh vực con"].ToString().Trim();
                var dvtiepnhanxl = row["Đơn vị tiếp nhận xử lý"].ToString().Trim();

                var tgxlpb = row["Đơn vị tiếp nhận (KTV/GDV)- (Phút)"].ToString().Trim();

                var tgxlpbXuly = row["Đơn vị chủ trì (ngày)"].ToString().Trim();
                var tgxlpbHotro = row["Đơn vị phối hợp - Ban KTNV (ngày)"].ToString().Trim();
                var tgxlpbHotro2 = row["Đơn vị phối hợp - Net/Media (ngày)"].ToString().Trim();
                //nếu không đầy đủ thông tin thời gian sẽ không xử lý
                if (string.IsNullOrEmpty(tgxlpb) || string.IsNullOrEmpty(tgxlpbXuly) ||
                    string.IsNullOrEmpty(tgxlpbHotro) || string.IsNullOrEmpty(tgxlpbHotro2))
                {
                    continue;
                }
                var thoiGianXl = "20d";

                lstLoaiKn = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "ltrim(rtrim(lower([Name]))) = N'" + loaiKn + "' and Cap=1", "Sort");
                // nếu đã có loại KN
                if(lstLoaiKn.Any())
                {
                    //cập nhật thông tin cho loại kn
                    var itemlkn = lstLoaiKn.FirstOrDefault();
                    //itemlkn.Cap = 1;
                    itemlkn.Name = loaiKn2;
                    itemlkn.ThoiGianCanhBao = thoiGianXl;
                    itemlkn.ThoiGianUocTinh = thoiGianXl;
                    itemlkn.Sort = sortLoaiKn;
                    itemlkn.Test = 1;
                    itemlkn.Status = 1;
                    itemlkn.DonViTiepNhanXL = dvtiepnhanxl;
                    //itemlkn.ParentId = 0;   // loại KN k có cha , bằng 0
                    //itemlkn.ParentLoaiKhieuNaiId = 0; // loại KN k có cha của cha , bằng 0
                    ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlkn);
                    lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "ltrim(rtrim(lower([Name]))) = N'" + linhVucChung + "' and ParentId="+ itemlkn.Id + " and Cap=2", "Sort");
                    // nếu lĩnh vực chung đã có
                    if(lstLinhVucChung.Any())
                    {
                        //cập nhật thông tin lĩnh vực chung
                        var itemlvch = lstLinhVucChung.FirstOrDefault();
                        //itemlvch.Cap = 2;
                        itemlvch.ThoiGianCanhBao = thoiGianXl;
                        itemlvch.ThoiGianUocTinh = thoiGianXl;
                        itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemlvch.Test = 1;
                        itemlvch.Status = 1;
                        itemlvch.DonViTiepNhanXL = dvtiepnhanxl;
                        //itemlvch.ParentId = itemlkn.Id; // cha là loại KN
                        //itemlvch.ParentLoaiKhieuNaiId = 0; // k có cha của loại KN
                        ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvch);
                        lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "ltrim(rtrim(lower([Name]))) = N'" + linhVucCon + "' and ParentId="+ itemlvch.Id + " and Cap=3", "Sort");
                        // Nếu lĩnh vực con đã có
                        if(lstLinhVucCon.Any())
                        {
                            //cập nhật lĩnh vực con
                            var itemlvc = lstLinhVucCon.FirstOrDefault();
                            //itemlvc.Cap = 3;
                            itemlvc.Name = linhVucCon2;
                            itemlvc.ThoiGianCanhBao = thoiGianXl;
                            itemlvc.ThoiGianUocTinh = thoiGianXl;
                            itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            itemlvc.Test = 1;
                            itemlvc.Status = 1;
                            itemlvc.DonViTiepNhanXL = dvtiepnhanxl;
                            //itemlvc.ParentId = itemlvch.Id; // có cha là id lĩnh vực chung
                            //itemlvc.ParentLoaiKhieuNaiId = itemlkn.Id; // có cha của lĩnh vực chung là id loại KN
                            ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvc);
                            sortLinhVucCon++;
                        }
                        else
                        {
                            var item = new LoaiKhieuNaiInfo();
                            item.Cap = 3;
                            item.Description = "";
                            item.Name = linhVucCon2;
                            item.ThoiGianCanhBao = thoiGianXl;
                            item.ThoiGianUocTinh = thoiGianXl;
                            item.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            item.Test = 1;
                            item.Status = 1;
                            item.DonViTiepNhanXL = dvtiepnhanxl;
                            item.ParentId = itemlvch.Id; // có cha là id lĩnh vực chung
                            item.ParentLoaiKhieuNaiId = itemlkn.Id; // có cha của lĩnh vực chung là id loại KN
                            item.NameLoaiKhieuNai = itemlkn.NameLoaiKhieuNai;
                            if (CheckValid(linhVucCon2))
                            {
                                item.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(item);  
                            }                                   
                        }
                    }
                    else
                    {
                        var itemlvch = new LoaiKhieuNaiInfo();
                        sortLinhVucChung += 1000;

                        itemlvch.Cap = 2;
                        itemlvch.Description = "";
                        itemlvch.Name = linhVucChung2;
                        itemlvch.ThoiGianCanhBao = thoiGianXl;
                        itemlvch.ThoiGianUocTinh = thoiGianXl;
                        itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemlvch.Test = 1;
                        itemlvch.Status = 1;
                        itemlvch.ParentId = itemlkn.Id; // cha là ID loại KN
                        itemlvch.ParentLoaiKhieuNaiId = 0; // k có cha của loại KN nên giá trị 0
                        itemlvch.DonViTiepNhanXL = dvtiepnhanxl;
                        if (CheckValid(linhVucChung2))
                        {
                            itemlvch.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemlvch);
                            lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "ltrim(rtrim(lower([Name]))) = N'" + linhVucCon + "' and ParentId=" + itemlvch.Id + " and Cap=3", "Sort");
                            // Nếu lĩnh vực con đã có
                            if (lstLinhVucCon.Any())
                            {
                                //cập nhật lĩnh vực con
                                var itemlvc = lstLinhVucCon.FirstOrDefault();
                                //itemlvc.Cap = 3;
                                itemlvc.Name = linhVucCon2;
                                itemlvc.ThoiGianCanhBao = thoiGianXl;
                                itemlvc.ThoiGianUocTinh = thoiGianXl;
                                itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                                itemlvc.Test = 1;
                                itemlvc.Status = 1;
                                itemlvc.DonViTiepNhanXL = dvtiepnhanxl;
                                //itemlvc.ParentId = itemlvch.Id; // cha là id lĩnh vực chung
                                //itemlvc.ParentLoaiKhieuNaiId = itemlkn.Id; // là id của loại kn
                                ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvc);
                                sortLinhVucCon++;
                            }
                            else
                            {
                                var item = new LoaiKhieuNaiInfo();
                                item.Cap = 3;
                                item.Description = "";
                                item.Name = linhVucCon2;
                                item.ThoiGianCanhBao = thoiGianXl;
                                item.ThoiGianUocTinh = thoiGianXl;
                                item.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                                item.Test = 1;
                                item.Status = 1;
                                item.ParentId = itemlvch.Id;
                                item.DonViTiepNhanXL = dvtiepnhanxl;
                                item.ParentLoaiKhieuNaiId = itemlkn.Id;
                                item.NameLoaiKhieuNai = itemlkn.NameLoaiKhieuNai;
                                if (CheckValid(linhVucCon2))
                                    item.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(item);
                            }
                        }
                    }
                }
                else
                {
                    var itemLoaiKN = new LoaiKhieuNaiInfo();
                    itemLoaiKN.Cap = 1;
                    itemLoaiKN.Description = "";
                    itemLoaiKN.Name = loaiKn2;
                    itemLoaiKN.ThoiGianCanhBao = thoiGianXl;
                    itemLoaiKN.ThoiGianUocTinh = thoiGianXl;
                    itemLoaiKN.Sort = sortLoaiKn;
                    itemLoaiKN.Status = 1;
                    itemLoaiKN.ParentId = 0; // loại kn gốc nên id cha =0
                    itemLoaiKN.ParentLoaiKhieuNaiId = 0; // cha của loại kn có =0
                    //add
                    if (CheckValid(loaiKn2))
                    {
                        itemLoaiKN.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemLoaiKN);
                        lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "ltrim(rtrim(lower([Name]))) = N'" + linhVucChung + "' and ParentId=" + itemLoaiKN.Id + " and Cap=2", "Sort");
                        // nếu lĩnh vực chung đã có
                        if (lstLinhVucChung.Any())
                        {
                            //cập nhật thông tin lĩnh vực chung
                            var itemlvch = lstLinhVucChung.FirstOrDefault();
                            //itemlvch.Cap = 2;
                            itemlvch.ThoiGianCanhBao = thoiGianXl;
                            itemlvch.ThoiGianUocTinh = thoiGianXl;
                            itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                            itemlvch.Test = 1;
                            itemlvch.Status = 1;
                            itemlvch.DonViTiepNhanXL = dvtiepnhanxl;
                            //itemlvch.ParentId = itemLoaiKN.Id;
                            //itemlvch.ParentLoaiKhieuNaiId = 0;
                            ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvch);
                            lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "ltrim(rtrim(lower([Name]))) = N'" + linhVucCon + "' and ParentId=" + itemlvch.Id + " and Cap=3" , "Sort");
                            // Nếu lĩnh vực con đã có
                            if (lstLinhVucCon.Any())
                            {
                                //cập nhật lĩnh vực con
                                var itemlvc = lstLinhVucCon.FirstOrDefault();
                                //itemlvc.Cap = 3;
                                itemlvc.Name = linhVucCon2;
                                itemlvc.ThoiGianCanhBao = thoiGianXl;
                                itemlvc.ThoiGianUocTinh = thoiGianXl;
                                itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                                itemlvc.Test = 1;
                                itemlvc.Status = 1;
                                itemlvc.DonViTiepNhanXL = dvtiepnhanxl;
                                //itemlvc.ParentId = itemlvch.Id;
                                //itemlvc.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                                ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvc);
                                sortLinhVucCon++;
                            }
                            else
                            {
                                var item = new LoaiKhieuNaiInfo();
                                item.Cap = 3;
                                item.Description = "";
                                item.Name = linhVucCon2;
                                item.ThoiGianCanhBao = thoiGianXl;
                                item.ThoiGianUocTinh = thoiGianXl;
                                item.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                                item.Test = 1;
                                item.Status = 1;
                                item.ParentId = itemlvch.Id;
                                item.DonViTiepNhanXL = dvtiepnhanxl;
                                item.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                                item.NameLoaiKhieuNai = itemLoaiKN.NameLoaiKhieuNai;
                                if (CheckValid(linhVucCon2))
                                    item.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(item);
                            }
                        }
                        else
                        {
                            var itemlvch = new LoaiKhieuNaiInfo();
                            sortLinhVucChung += 1000;

                            itemlvch.Cap = 2;
                            itemlvch.Description = "";
                            itemlvch.Name = linhVucChung2;
                            itemlvch.ThoiGianCanhBao = thoiGianXl;
                            itemlvch.ThoiGianUocTinh = thoiGianXl;
                            itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                            itemlvch.Test = 1;
                            itemlvch.Status = 1;
                            itemlvch.ParentId = itemLoaiKN.Id;
                            itemlvch.DonViTiepNhanXL = dvtiepnhanxl;
                            itemlvch.ParentLoaiKhieuNaiId = 0;
                            itemlvch.NameLoaiKhieuNai = itemLoaiKN.NameLoaiKhieuNai;
                            if (CheckValid(linhVucChung2))
                            {
                                itemlvch.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemlvch);
                                lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "ltrim(rtrim(lower([Name]))) = N'" + linhVucCon + "' and ParentId=" + itemlvch.Id + " and Cap=3", "Sort");
                                // Nếu lĩnh vực con đã có
                                if (lstLinhVucCon.Any())
                                {
                                    //cập nhật lĩnh vực con
                                    var itemlvc = lstLinhVucCon.FirstOrDefault();
                                    //itemlvc.Cap = 3;
                                    itemlvc.Name = linhVucCon2;
                                    itemlvc.ThoiGianCanhBao = thoiGianXl;
                                    itemlvc.ThoiGianUocTinh = thoiGianXl;
                                    itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                                    itemlvc.Test = 1;
                                    itemlvc.Status = 1;
                                    itemlvc.DonViTiepNhanXL = dvtiepnhanxl;
                                    //itemlvc.ParentId = itemlvch.Id;
                                    //itemlvc.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                                    //itemlvc.NameLoaiKhieuNai = itemlvc.NameLoaiKhieuNai;
                                    ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvc);
                                    sortLinhVucCon++;
                                }
                                else
                                {
                                    var item = new LoaiKhieuNaiInfo();
                                    item.Cap = 3;
                                    item.Description = "";
                                    item.Name = linhVucCon2;
                                    item.ThoiGianCanhBao = thoiGianXl;
                                    item.ThoiGianUocTinh = thoiGianXl;
                                    item.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                                    item.Test = 1;
                                    item.Status = 1;
                                    item.ParentId = itemlvch.Id;
                                    item.DonViTiepNhanXL = dvtiepnhanxl;
                                    item.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                                    item.NameLoaiKhieuNai = itemLoaiKN.NameLoaiKhieuNai;
                                    if (CheckValid(linhVucCon2))
                                        item.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(item);
                                }
                            }                              
                        }
                    }                       
                }
                sortLoaiKn++;
            }
        }
        catch (Exception ex)
        {
            lbMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// kiem tra su hop le cua Name trong loaikhieunai
    /// DODV 13/05/2016
    /// Create new
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool CheckValid(string name)
    {
        return !(name.ToLower().Equals("") ||
               //name.ToLower().Equals("khác".ToLower(), StringComparison.OrdinalIgnoreCase) ||
               name.ToLower().Equals("(blanks)".ToLower(), StringComparison.OrdinalIgnoreCase)
               || name.ToLower().Equals("(blank)".ToLower(), StringComparison.OrdinalIgnoreCase) || name.Length == 0);
    }

    DataTable dt;
    protected void btLayDanhSach_Click(object sender, EventArgs e)
    {
        string pathUpload = Server.MapPath("UploadTransaction");

        try
        {
            string fileName = Utility.UploadFile(fUpload, pathUpload, true, true, Constant.FILE_UPLOAD_EXTENSIVE);
            if (string.IsNullOrEmpty(fileName))
            {
                lbMessage.Text = "Bạn chưa chọn file loại khiếu nại.";
                return;
            }
            dt = Utility.ExcelToDataTable(Path.Combine(pathUpload, fileName));

            if (dt.Columns["Các loại khiếu nại"] == null || dt.Columns["Lĩnh vực chung"] == null || dt.Columns["Lĩnh vực con"] == null)
            {
                lbMessage.Text = "Cấu trúc file không hợp lệ.";
                return;
            }

            tableResult.Visible = true;
            btLayDanhSach.Enabled = false;
            btLayDanhSach.Visible = false;
            btHuy.Visible = true;

            ltThongBao.Text = "Có " + dt.Rows.Count + " loại khiếu nại hợp lệ.";

            ViewState["TableImport"] = dt;
            grvView.DataSource = ViewState["TableImport"];

            grvView.DataBind();
        }
        catch (Exception ex)
        { }
    }

    protected void btHuy_Click(object sender, EventArgs e)
    {
        ViewState["TableImport"] = null;
        grvView.DataSource = null;
        grvView.DataBind();

        btHuy.Visible = false;
        tableResult.Visible = false;
        btLayDanhSach.Enabled = true;
        btLayDanhSach.Visible = true;
    }

    protected void linkbtnCapNhat_Click(object sender, EventArgs e)
    {
        xuLyExcelLoaiKNtoDB();
        return;

        var dt = ViewState["TableImport"] as DataTable;

        try
        {
            List<LoaiKhieuNaiInfo> lstLoaiKn;//= ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
            //ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "", "Sort");
            int sortLoaiKn = 0;
            int sortLinhVucChung = 100;
            int sortLinhVucCon = 1;

            var i = 0;

            foreach (DataRow row in dt.Rows)
            {
                lstLoaiKn = ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
                // dùng để xử lý
                var loaiKn = row["Các loại khiếu nại"].ToString().ToLower().Trim();
                var linhVucChung = row["Lĩnh vực chung"].ToString().ToLower().Trim();
                var linhVucCon = row["Lĩnh vực con"].ToString().ToLower().Trim();

                // dùng để add vào db
                var loaiKn2 = row["Các loại khiếu nại"].ToString().Trim();
                var linhVucChung2 = row["Lĩnh vực chung"].ToString().Trim();
                var linhVucCon2 = row["Lĩnh vực con"].ToString().Trim();
                var dvtiepnhanxl = row["Đơn vị tiếp nhận xử lý"].ToString().Trim();

                var tgxlpb = row["TGXLPB"].ToString().Trim();

                var tgxlpbXuly = row["TGXLPB_XuLy"].ToString().Trim();
                var tgxlpbHotro = row["TGXLPB_HoTro"].ToString().Trim();
                var tgxlpbHotro2 = row["TGXLPB_HoTro2"].ToString().Trim();
                //nếu không đầy đủ thông tin thời gian sẽ không xử lý
                if (string.IsNullOrEmpty(tgxlpb) || string.IsNullOrEmpty(tgxlpbXuly) ||
                    string.IsNullOrEmpty(tgxlpbHotro) || string.IsNullOrEmpty(tgxlpbHotro2))
                {
                    continue;
                }
                var thoiGianXl = "20d";

                var filterLoaiKn = lstLoaiKn.Where(t => t.Name.Trim().ToLower().Equals(loaiKn.ToLower()) && t.ParentId == 0);
                //Loại khiếu nại đã có
                if (filterLoaiKn.Any())
                {
                    //cập nhật thông tin cho loại kn
                    var itemlkn = filterLoaiKn.FirstOrDefault();
                    itemlkn.Name = loaiKn2;
                    itemlkn.ThoiGianCanhBao = thoiGianXl;
                    itemlkn.ThoiGianUocTinh = thoiGianXl;
                    itemlkn.Sort = sortLoaiKn;
                    itemlkn.Test = 1;
                    itemlkn.Status = 1;
                    itemlkn.DonViTiepNhanXL = dvtiepnhanxl;
                    //itemlkn.ParentLoaiKhieuNaiId = itemlkn.Id;
                    ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlkn);
                    var LoaiKNItem = filterLoaiKn.FirstOrDefault();
                    var filterLinhVucChung =
                        lstLoaiKn.Where(t => t.Name.Trim().ToLower().Equals(linhVucChung.ToLower()) && t.ParentId == LoaiKNItem.Id);
                    //lĩnh vực chung đã có
                    if (filterLinhVucChung.Any())
                    {
                        //cập nhật thông tin lĩnh vực chung
                        var itemlvch = filterLinhVucChung.FirstOrDefault();
                        itemlvch.ThoiGianCanhBao = thoiGianXl;
                        itemlvch.ThoiGianUocTinh = thoiGianXl;
                        itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemlvch.Test = 1;
                        itemlvch.Status = 1;
                        itemlvch.DonViTiepNhanXL = dvtiepnhanxl;
                        //itemlvch.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                        ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvch);

                        var filterLinhVucCon = lstLoaiKn.Where(t => t.Name.Trim().ToLower().Equals(linhVucCon.ToLower()) && t.ParentId == itemlvch.Id);
                        //lĩnh vực con đã có
                        if (filterLinhVucCon.Any())
                        {
                            //cập nhật lĩnh vực con
                            var itemlvc = filterLinhVucCon.FirstOrDefault();
                            itemlvc.Name = linhVucCon2;
                            itemlvc.ThoiGianCanhBao = thoiGianXl;
                            itemlvc.ThoiGianUocTinh = thoiGianXl;
                            itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            itemlvc.Test = 1;
                            itemlvc.Status = 1;
                            itemlvc.DonViTiepNhanXL = dvtiepnhanxl;
                            //itemlvc.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                            ServiceFactory.GetInstanceLoaiKhieuNai().Update(itemlvc);
                            sortLinhVucCon++;
                        }
                        //tên lĩnh vực con thỏa mãn, và chưa có -> thêm mới lĩnh vực con
                        else if (CheckValid(linhVucCon) && !filterLinhVucCon.Any())
                        {
                            var item = new LoaiKhieuNaiInfo();
                            item.Cap = 3;
                            item.Description = "";
                            item.Name = linhVucCon2;
                            item.ThoiGianCanhBao = thoiGianXl;
                            item.ThoiGianUocTinh = thoiGianXl;
                            item.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            item.Test = 1;
                            item.Status = 1;
                            item.ParentId = itemlvch.Id;
                            item.DonViTiepNhanXL = dvtiepnhanxl;
                            item.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                            item.NameLoaiKhieuNai = LoaiKNItem.NameLoaiKhieuNai;
                            if (item.ParentId != 0 &&
                                !lstLoaiKn.Any(
                                    p => p.Cap == item.Cap && p.Name.Trim().ToLower().Equals(item.Name.ToLower()) && p.ParentId == item.ParentId))
                            {
                                item.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(item);
                                //lstLoaiKn.Add(item);
                            }
                        }
                    }
                    //tên lĩnh vực chung thỏa mãn và chưa có -> thêm mới lĩnh vực chung
                    else if (CheckValid(linhVucChung) && !filterLinhVucChung.Any())
                    {
                        var itemlvch = new LoaiKhieuNaiInfo();
                        sortLinhVucChung += 1000;

                        itemlvch.Cap = 2;
                        itemlvch.Description = "";
                        itemlvch.Name = linhVucChung2;
                        itemlvch.ThoiGianCanhBao = thoiGianXl;
                        itemlvch.ThoiGianUocTinh = thoiGianXl;
                        itemlvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemlvch.Test = 1;
                        itemlvch.Status = 1;
                        itemlvch.ParentId = itemlkn.Id;
                        itemlvch.DonViTiepNhanXL = dvtiepnhanxl;
                        itemlvch.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                        if (itemlvch.ParentId != 0 && !lstLoaiKn.Any(
                                p => p.Cap == itemlvch.Cap && p.Name.Trim().ToLower().Equals(itemlvch.Name.ToLower()) && p.ParentId == itemlvch.ParentId))
                        {
                            itemlvch.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemlvch);
                            //lstLoaiKn.Add(itemlvch);
                        }

                        //tên lĩnh vực con thỏa mãn-> thêm mới
                        else if (CheckValid(linhVucCon))
                        {
                            var itemlvc = new LoaiKhieuNaiInfo();
                            itemlvc.Cap = 3;
                            itemlvc.Description = "";
                            itemlvc.Name = linhVucCon2;
                            itemlvc.ThoiGianCanhBao = thoiGianXl;
                            itemlvc.ThoiGianUocTinh = thoiGianXl;
                            itemlvc.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            itemlvc.Test = 1;
                            itemlvc.Status = 1;
                            itemlvc.ParentId = itemlvch.Id;
                            itemlvc.DonViTiepNhanXL = dvtiepnhanxl;
                            itemlvc.ParentLoaiKhieuNaiId = LoaiKNItem.Id;
                            itemlvc.NameLoaiKhieuNai = LoaiKNItem.NameLoaiKhieuNai;
                            if (itemlvc.ParentId != 0 &&
                                !lstLoaiKn.Any(
                                    p =>
                                        p.Cap == itemlvc.Cap && p.Name.Trim().ToLower().Equals(itemlvc.Name.ToLower()) && p.ParentId == itemlvc.ParentId))
                            {
                                itemlvc.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemlvc);
                                // lstLoaiKn.Add(itemlvc);
                            }
                            sortLinhVucCon++;
                        }
                    }
                }
                //loại khiếu nại thỏa mãn và chưa có-> thêm mới
                else if (CheckValid(loaiKn))
                {
                    var itemLoaiKN = new LoaiKhieuNaiInfo();
                    itemLoaiKN.Cap = 1;
                    itemLoaiKN.Description = "";
                    itemLoaiKN.Name = loaiKn2;
                    itemLoaiKN.ThoiGianCanhBao = thoiGianXl;
                    itemLoaiKN.ThoiGianUocTinh = thoiGianXl;
                    itemLoaiKN.Sort = sortLoaiKn;
                    itemLoaiKN.Status = 1;
                    itemLoaiKN.ParentId = 0;
                    //add
                    itemLoaiKN.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemLoaiKN);
                    //lstLoaiKn.Add(itemLoaiKN);
                    // lĩnh vực chung thỏa mãn-> thêm lĩnh vực chung
                    if (itemLoaiKN.Id > 0 && CheckValid(linhVucChung))
                    {
                        var itemLvch = new LoaiKhieuNaiInfo();
                        sortLinhVucChung = 1000;
                        itemLvch.Cap = 2;
                        itemLvch.Description = "";
                        itemLvch.Name = linhVucChung2;
                        itemLvch.ThoiGianCanhBao = thoiGianXl;
                        itemLvch.ThoiGianUocTinh = thoiGianXl;
                        itemLvch.Sort = sortLoaiKn + sortLinhVucChung;
                        itemLvch.Test = 1;
                        itemLvch.Status = 1;
                        itemLvch.ParentId = itemLoaiKN.Id;
                        itemLvch.DonViTiepNhanXL = dvtiepnhanxl;
                        itemLvch.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                        //add new if not exist from table LoaiKhieuNai
                        itemLvch.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemLvch);
                        lstLoaiKn.Add(itemLvch);
                        //lĩnh vực con thỏa mãn-> thêm mới lĩnh vực con
                        if (itemLvch.Id > 0 && CheckValid(linhVucCon))
                        {
                            var itemLVCon = new LoaiKhieuNaiInfo();
                            itemLVCon.Cap = 3;
                            itemLVCon.Description = "";
                            itemLVCon.Name = row["Lĩnh vực con"].ToString().Trim();
                            itemLVCon.ThoiGianCanhBao = thoiGianXl;
                            itemLVCon.ThoiGianUocTinh = thoiGianXl;
                            itemLVCon.Sort = sortLoaiKn + sortLinhVucChung + sortLinhVucCon;
                            itemLVCon.Test = 1;
                            itemLVCon.Status = 1;
                            itemLVCon.ParentId = itemLvch.Id;
                            itemLVCon.DonViTiepNhanXL = dvtiepnhanxl;
                            itemLVCon.ParentLoaiKhieuNaiId = itemLoaiKN.Id;
                            itemLVCon.NameLoaiKhieuNai = itemLoaiKN.NameLoaiKhieuNai;
                            //add new if not exist from table LoaiKhieuNai
                            itemLVCon.Id = ServiceFactory.GetInstanceLoaiKhieuNai().Add(itemLVCon);
                            //lstLoaiKn.Add(itemLVCon);
                        }
                    }
                    sortLoaiKn++;
                }
            }
        }
        catch (Exception ex)
        {
            lbMessage.Text = ex.Message;
        }
    }

    protected void linkbtnExportMaToExcel_Click(object sender, EventArgs e)
    {
        var dt = ViewState["TableImport"] as DataTable;

        try
        {
            //var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "Id>1203", "Sort");
            var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("[Id],[ParentId],[MaDichVu],ltrim(rtrim(lower([Name]))) Name,[Description],[Sort],[Cap],[Status],[ThoiGianCanhBao],[ThoiGianUocTinh]", "", "Sort");
            int sortLoaiKN = 1000000;
            int sortLinhVucChung = 1000;
            int sortLinhVucCon = 1;

            var i = 0;
            Dictionary<int, int> dic = new Dictionary<int, int>();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    var LoaiKN = row["Các loại khiếu nại"].ToString().ToLower().Trim();
                    var LinhVucChung = row["Lĩnh vực chung"].ToString().ToLower().Trim();
                    var LinhVucCon = row["Lĩnh vực con"].ToString().ToLower().Trim();

                    var Id2 = Convert.ToInt32(row["Id2"].ToString().Trim());

                    var filterLoaiKN = lstLoaiKN.Where(t => t.Name.ToLower().Equals(LoaiKN) && t.ParentId == 0);
                    if (filterLoaiKN != null && filterLoaiKN.Any())
                    {

                        if (LinhVucChung.Equals("") || LinhVucChung.Equals("khác") || LinhVucChung.Equals("(blanks)") || LinhVucChung.Equals("(blank)"))
                        {
                            //Đây là loại khiếu nại
                            var item = filterLoaiKN.FirstOrDefault();
                            dic.Add(Id2, item.Id);
                        }
                        else
                        {
                            var LoaiKNItem = filterLoaiKN.FirstOrDefault();
                            var filterLinhVucChung = lstLoaiKN.Where(t => t.Name.ToLower().Equals(LinhVucChung) && t.ParentId == LoaiKNItem.Id);
                            if (filterLinhVucChung != null && filterLinhVucChung.Any())
                            {
                                if (LinhVucCon.Equals("") || LinhVucCon.Equals("khác") || LinhVucCon.Equals("(blanks)") || LinhVucCon.Equals("(blank)"))
                                {
                                    //Đây là lĩnh vực chung
                                    var item = filterLinhVucChung.FirstOrDefault();
                                    dic.Add(Id2, item.Id);
                                }
                                else
                                {
                                    var LinhVucChungItem = filterLinhVucChung.FirstOrDefault();
                                    var filterLinhVucCon = lstLoaiKN.Where(t => t.Name.ToLower().Equals(LinhVucCon) && t.ParentId == LinhVucChungItem.Id);
                                    if (filterLinhVucCon != null && filterLinhVucCon.Count() == 1)
                                    {
                                        var item = filterLinhVucCon.FirstOrDefault();
                                        dic.Add(Id2, item.Id);
                                    }

                                }
                            }

                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }

            string file = @"D:\import.xlsx";
            UpdateMa(file, dic, "Sheet1");

        }
        catch (Exception ex) { lbMessage.Text = ex.Message; }
    }

    protected void linkbtnUpdateThoiGianXuLy_Click(object sender, EventArgs e)
    {
        var dt = ViewState["TableImport"] as DataTable;

        try
        {

            var LoaiPhongBanId = Convert.ToInt32(ddlLoaiPhongBan.SelectedValue);
            var lstThoiGianXuLy = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetListDynamic("", "LoaiPhongBanId=" + LoaiPhongBanId, "");

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(row["Id"].ToString().Trim()) || string.IsNullOrEmpty(row["TGXLPB"].ToString().Trim()))
                        {
                            continue;
                        }
                        var LoaiKN = Convert.ToInt32(row["Id"].ToString().Trim());
                        var ThoiGianXLPB = row["TGXLPB"].ToString().Trim();
                        ThoiGianXLPB = Convert.ToInt32(ThoiGianXLPB) + "m";
                        var check = lstThoiGianXuLy.Where(t => t.LoaiKhieuNaiId == LoaiKN);
                        if (check != null && check.Any())
                        {
                            var item = check.FirstOrDefault();
                            item.ThoiGianCanhBao = ThoiGianXLPB;
                            item.ThoiGianUocTinh = ThoiGianXLPB;
                            ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().Update(item);
                        }
                        else
                        {
                            var item = new LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo();
                            item.LoaiPhongBanId = LoaiPhongBanId;
                            item.LoaiKhieuNaiId = LoaiKN;
                            item.ThoiGianCanhBao = ThoiGianXLPB;
                            item.ThoiGianUocTinh = ThoiGianXLPB;
                            ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().Add(item);
                        }
                    }
                    scope.Complete();
                    lbMessage.Text = "Successfull";
                }
                catch (Exception ex) { lbMessage.Text = ex.Message; }
            }
        }
        catch (Exception ex) { }
    }

    protected void linkbtnUpdateThoiGianXuLy_XuLy_Click(object sender, EventArgs e)
    {
        var dt = ViewState["TableImport"] as DataTable;

        try
        {
            var LoaiPhongBanId = Convert.ToInt32(ddlLoaiPhongBan.SelectedValue);
            var lstThoiGianXuLy = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetListDynamic("", "LoaiPhongBanId=" + LoaiPhongBanId, "");

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(row["Id"].ToString().Trim()) || string.IsNullOrEmpty(row["TGXLPB_XuLy"].ToString().Trim()))
                        {
                            continue;
                        }
                        var LoaiKN = Convert.ToInt32(row["Id"].ToString().Trim());
                        var ThoiGianXLPB = row["TGXLPB_XuLy"].ToString().Trim() + "d";
                        var check = lstThoiGianXuLy.Where(t => t.LoaiKhieuNaiId == LoaiKN && t.LoaiPhongBanId == LoaiPhongBanId);

                        if (check != null && check.Any())
                        {
                            var item = check.FirstOrDefault();
                            item.ThoiGianCanhBao = ThoiGianXLPB;
                            item.ThoiGianUocTinh = ThoiGianXLPB;
                            ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().Update(item);
                        }
                        else
                        {
                            var item = new LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo();
                            item.LoaiPhongBanId = LoaiPhongBanId;
                            item.LoaiKhieuNaiId = LoaiKN;
                            item.ThoiGianCanhBao = ThoiGianXLPB;
                            item.ThoiGianUocTinh = ThoiGianXLPB;
                            ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().Add(item);
                        }
                    }
                    scope.Complete();
                    lbMessage.Text = "Successfull";
                }
                catch (Exception ex) { lbMessage.Text = ex.Message; }
            }
        }
        catch (Exception ex) { }
    }

    protected void linkbtnUpdateThoiGianXuLy_HoTro_Click(object sender, EventArgs e)
    {
        var dt = ViewState["TableImport"] as DataTable;

        try
        {

            var LoaiPhongBanId = Convert.ToInt32(ddlLoaiPhongBan.SelectedValue);
            var lstThoiGianXuLy = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetListDynamic("", "LoaiPhongBanId=" + LoaiPhongBanId, "");

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(row["Id"].ToString().Trim()) || string.IsNullOrEmpty(row["TGXLPB_HoTro"].ToString().Trim()) || string.IsNullOrEmpty(row["TGXLPB_HoTro2"].ToString().Trim()))
                        {
                            continue;
                        }

                        var LoaiKN = Convert.ToInt32(row["Id"].ToString().Trim());
                        var ThoiGianXLPB = string.Empty;
                        /* DoDV-5/5/2016
                         * Xử lý tách phòng ban hỗ trợ
                         * với từng loại phòng ban sẽ lấy thời gian xử lý hỗ trợ tương ứng
                         * phòng ban hỗ trợ -TGXLPB_HoTro
                         * phòng ban hỗ trợ 2 -TGXLPB_HoTro2
                         * phòng ban hỗ trợ 3 -  LoaiPhongBanId == 102- media - giống như pbht net
                        */
                        double thoigianHT;
                        if (LoaiPhongBanId == 89)
                        {

                            thoigianHT = Convert.ToDouble(row["TGXLPB_HoTro"].ToString().Trim());
                        }
                        else if (LoaiPhongBanId == 101 || LoaiPhongBanId == 105)
                        {

                            thoigianHT = Convert.ToDouble(row["TGXLPB_HoTro2"].ToString().Trim());
                        }
                        else
                        {
                            return;
                        }
                        ThoiGianXLPB = thoigianHT + "d";

                        var check = lstThoiGianXuLy.Where(t => t.LoaiKhieuNaiId == LoaiKN && t.LoaiPhongBanId == LoaiPhongBanId);
                        if (check.Any())
                        {
                            var item = check.FirstOrDefault();
                            item.ThoiGianCanhBao = ThoiGianXLPB;
                            item.ThoiGianUocTinh = ThoiGianXLPB;

                            ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().Update(item);
                        }
                        else
                        {
                            var item = new LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo();
                            item.LoaiPhongBanId = LoaiPhongBanId;
                            item.LoaiKhieuNaiId = LoaiKN;
                            item.ThoiGianCanhBao = ThoiGianXLPB;
                            item.ThoiGianUocTinh = ThoiGianXLPB;

                            ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().Add(item);
                        }
                    }
                    scope.Complete();
                    lbMessage.Text = "Successfull";
                }
                catch (Exception ex) { lbMessage.Text = ex.Message; }
            }
        }
        catch (Exception ex) { }
    }
}

