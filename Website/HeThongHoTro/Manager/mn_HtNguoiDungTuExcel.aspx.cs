using ADDJ.Sercurity;
using DevExpress.Web;
using FastMember;
//using SimpleExcelImport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Manager
{
    public partial class mn_HtNguoiDungTuExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var tbl = Session["tblHeThongXL"] as List<HT_EXCELFILE>;
                gvReadExcel.DataSource = tbl;
            }
        }

        const string UploadDirectory = "~/HTHTKT/UploadControl/UploadImages/";
        const string ThumbnailFileName = "ThumbnailImage.jpg";

        protected void uplImage_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (!e.IsValid) e.CallbackData = string.Empty;

            string fileName = string.Format("~/HTHTKT/Uploadcontrol/UploadImages/{0}", e.UploadedFile.FileName);
            e.UploadedFile.SaveAs(MapPath(fileName), true);

            e.CallbackData = e.UploadedFile.FileName;

            readExcel(fileName);
        }

        private void readExcel(string path)
        {
            Session.Remove("tblHeThongXL");
            Session.Remove("isDone");
            Session.Remove("tblHeThongXLDONE");
            try
            {
                var path111 = MapPath(path);

                // Get the input file paths
                FileInfo inputFile = new FileInfo(path111);
                List<HT_EXCELFILE> lstExcelRead = new List<HT_EXCELFILE>();
                // Create an instance of Fast Excel
                using (FastExcel.FastExcel fastExcel = new FastExcel.FastExcel(inputFile, true))
                {
                    // Read the rows using the worksheet index
                    // Worksheet indexes are start at 1 not 0
                    // This method is slightly faster to find the underlying file (so slight you probably wouldn't notice)
                    var worksheet = fastExcel.Read(1);
                    var lstRow = worksheet.Rows.ToArray();
                    if (lstRow.Any())
                    {
                        int i = 0;
                        int count = lstRow.Length;
                        foreach (var item in lstRow)
                        {
                            if (item.Cells != null)
                            {
                                var name = item.Cells.ToArray();
                                if (i != 0 && i < count - 1)
                                {
                                    if (Convert.ToString(name[0].Value) != "")
                                    {
                                        var rec = new HT_EXCELFILE();
                                        rec.TONGCT = Convert.ToString(name[0].Value);
                                        rec.KHUVUC = Convert.ToString(name[1].Value);
                                        rec.DONVI = Convert.ToString(name[2].Value);
                                        rec.BOPHAN = Convert.ToString(name[3].Value);
                                        rec.CANHAN = Convert.ToString(name[4].Value);
                                        rec.HOTEN = Convert.ToString(name[5].Value);
                                        rec.DIENTHOAI = Convert.ToString(name[6].Value);
                                        rec.TENTRUYCAP = Convert.ToString(name[7].Value);
                                        rec.EMAIL = Convert.ToString(name[8].Value);
                                        rec.QUYEN = Convert.ToString(name[9].Value);
                                        lstExcelRead.Add(rec);
                                    }
                                }
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                Session["tblHeThongXL"] = lstExcelRead;




                //var data = File.ReadAllBytes(MapPath(path));
                //ImportFromExcel import = new ImportFromExcel();
                //import.LoadXlsx(data);
                ////first parameter it's the sheet number in the excel workbook
                ////second parameter it's the number of rows to skip at the start(we have an header in the file)
                //List<HT_EXCELFILE> output = import.ExcelToList<HT_EXCELFILE>(0, 1);
                //Session["tblHeThongXL"] = output;
                File.Delete(MapPath(path));
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "EXCEL");
                throw;
            }
        }

        protected void gvReadExcel_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gvReadExcel.DataBind();
        }

        protected void gvReadExcel_DataBinding(object sender, EventArgs e)
        {
            var isDone = Session["isDone"] as string;
            if (isDone != null && isDone != "")
            {
                var tblDone = Session["tblHeThongXLDONE"] as DataTable;
                gvReadExcel.DataSource = tblDone;
            }
            else
            {


                var tbl = Session["tblHeThongXL"] as List<HT_EXCELFILE>;

                if (tbl != null)
                {
                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(tbl))
                    {
                        table.Load(reader);
                    }

                    table.Columns.Add(new DataColumn("TRANGTHAI", typeof(string))); // Add New Column StatusText
                                                                                    //Iterate All Rows 
                    foreach (DataRow row in table.Rows)
                    {
                        //Check Status value, and set StatusText accordingly. 
                        row["TRANGTHAI"] = "Chưa xử lý";
                    }

                    gvReadExcel.DataSource = table;
                }
                else
                {
                    gvReadExcel.DataSource = null;
                }
            }
        }
        protected void gvReadExcel_PageIndexChanged(object sender, EventArgs e)
        {
            gvReadExcel.DataBind();
        }

        protected void importHT_Click(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<HT_EXCELFILE>;

            using (var ctx = new ADDJContext())
            {
                if (tbl != null)
                {

                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(tbl))
                    {
                        table.Load(reader);
                    }

                   table.Columns.Add(new DataColumn("TRANGTHAI", typeof(string))); // Add New Column StatusText
                                                                                   //Iterate All Rows 
                                                                                   //foreach (DataRow row in table.Rows)
                                                                                   //{
                                                                                   //    //Check Status value, and set StatusText accordingly. 
                                                                                   //    row["TRANGTHAI"] = "Chưa xử lý";
                                                                                   //}
                    int i = 0;
                    string status_process = "";
                    foreach (var item in tbl)
                    {
                        var strSql = string.Format(@"INSERT INTO dbo.HT_EXCELFILE
                                                    (
                                                      TONGCT
                                                     ,KHUVUC
                                                     ,DONVI
                                                     ,BOPHAN
                                                     ,CANHAN
                                                     ,HOTEN
                                                     ,DIENTHOAI
                                                     ,TENTRUYCAP
                                                     ,EMAIL
                                                     ,QUYEN
                                                    )
                                                    VALUES
                                                    (
                                                      N'{0}' -- TONGCT - nvarchar(50)
                                                     ,N'{1}' -- KHUVUC - nvarchar(50)
                                                     ,N'{2}' -- DONVI - nvarchar(50)
                                                     ,N'{3}' -- BOPHAN - nvarchar(50)
                                                     ,N'{4}' -- CANHAN - nvarchar(50)
                                                     ,N'{5}' -- HOTEN - nvarchar(50)
                                                     ,N'{6}' -- DIENTHOAI - nvarchar(50)
                                                     ,N'{7}' -- TENTRUYCAP - nvarchar(50)
                                                     ,N'{8}' -- EMAIL - nvarchar(50)
                                                     ,N'{9}' -- QUYEN - nvarchar(50)
                                                    )", item.TONGCT, item.KHUVUC, item.DONVI, item.BOPHAN
                                                    , item.CANHAN, item.HOTEN, item.DIENTHOAI, item.TENTRUYCAP, item.EMAIL, item.QUYEN);
                        ctx.Database.ExecuteSqlCommand(strSql);

                        var isOK = false;
                        // 1. Kiểm tra thông tin đơn vị
                        // Tổng Công Ty, Khu vực, Đơn vị, Bộ phận, Cá nhân
                        if (item.TONGCT != null && item.TONGCT != "")
                        {
                            var strCheckTCT = string.Format("Select * from HT_DONVI where TENDONVI_KD=N'{0}'", TienIch.convertToUnSign3(item.TONGCT.Trim()));
                            var tctInfo = ctx.Database.SqlQuery<HT_DONVI>(strCheckTCT);
                            var lstTCT = tctInfo.ToList();
                            if (lstTCT.Any())
                            {
                                isOK = true;
                                if (item.KHUVUC.Trim() != "")
                                {
                                    var strCheckKV = string.Format("Select * from HT_DONVI where TENDONVI_KD=N'{0}'", TienIch.convertToUnSign3(item.KHUVUC.Trim()));
                                    var kvInfo = ctx.Database.SqlQuery<HT_DONVI>(strCheckKV);
                                    var lstKV = kvInfo.ToList();
                                    if (lstKV.Any())
                                    {
                                        isOK = true;
                                        if (item.DONVI.Trim() != "")
                                        {
                                            var strCheckDV = string.Format("Select * from HT_DONVI where TENDONVI_KD=N'{0}'", TienIch.convertToUnSign3(item.DONVI.Trim()));
                                            var dvInfo = ctx.Database.SqlQuery<HT_DONVI>(strCheckDV);
                                            var lstDV = dvInfo.ToList();
                                            if (lstDV.Any())
                                            {
                                                isOK = true;
                                                if (item.BOPHAN.Trim() != "")
                                                {
                                                    var strCheckBP = string.Format("Select * from HT_DONVI where TENDONVI_KD=N'{0}'", TienIch.convertToUnSign3(item.BOPHAN.Trim()));
                                                    var bpInfo = ctx.Database.SqlQuery<HT_DONVI>(strCheckBP);
                                                    var lstBP = bpInfo.ToList();
                                                    if (lstBP.Any())
                                                    {
                                                        isOK = true;
                                                        if (item.CANHAN.Trim()!="")
                                                        {
                                                            var strCheckCN = string.Format("Select * from HT_DONVI where TENDONVI_KD=N'{0}'", TienIch.convertToUnSign3(item.CANHAN.Trim()));
                                                            var cnInfo = ctx.Database.SqlQuery<HT_DONVI>(strCheckCN);
                                                            var lstCN = cnInfo.ToList();
                                                            if (lstCN.Any())
                                                            {
                                                                isOK = true;
                                                                // 2. Tạo người dùng CN                                             
                                                                // 3. Gán người dùng-đơn vị CN
                                                                var check = taoNguoiDungDV(lstCN[0].ID.ToString(), item);
                                                                switch (check)
                                                                {
                                                                    case "EXIST_USER":
                                                                        status_process = "Đã tồn tại người dùng";
                                                                        break;
                                                                    case "NOT_EXIST_GROUP":
                                                                        status_process = "Không tồn tại quyền, check lại tên chính xác";
                                                                        break;
                                                                    case "OK":
                                                                        status_process = "Tạo thành công";
                                                                        break;
                                                                    case "FAIL":
                                                                        status_process = "Không thành công";
                                                                        break;
                                                                    case "NAME_USER_BLANK":
                                                                        status_process = "Tên không để trống";
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                status_process = "Không có thông tin CÁ NHÂN: " + item.CANHAN.Trim() + ", vui lòng kiểm tra lại";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // nếu k có cá nhân thì tạo đơn vị
                                                            // 2. Tạo người dùng BP
                                                            // 3. Gán người dùng-đơn vị BP
                                                            var check = taoNguoiDungDV(lstBP[0].ID.ToString(), item);
                                                            switch (check)
                                                            {
                                                                case "EXIST_USER":
                                                                    status_process = "Đã tồn tại người dùng";
                                                                    break;
                                                                case "NOT_EXIST_GROUP":
                                                                    status_process = "Không tồn tại quyền, check lại tên chính xác";
                                                                    break;
                                                                case "OK":
                                                                    status_process = "Tạo thành công";
                                                                    break;
                                                                case "FAIL":
                                                                    status_process = "Không thành công";
                                                                    break;
                                                                case "NAME_USER_BLANK":
                                                                    status_process = "Tên không để trống";
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        status_process = "Không có thông tin BỘ PHẬN: " + item.BOPHAN.Trim() + ", vui lòng kiểm tra lại";
                                                    }
                                                }
                                                else
                                                {
                                                    // 2. Tạo người dùng DV
                                                    // 3. Gán người dùng-đơn vị DV
                                                    var check = taoNguoiDungDV(lstDV[0].ID.ToString(), item);
                                                    switch (check)
                                                    {
                                                        case "EXIST_USER":
                                                            status_process = "Đã tồn tại người dùng";
                                                            break;
                                                        case "NOT_EXIST_GROUP":
                                                            status_process = "Không tồn tại quyền, check lại tên chính xác";
                                                            break;
                                                        case "OK":
                                                            status_process = "Tạo thành công";
                                                            break;
                                                        case "FAIL":
                                                            status_process = "Không thành công";
                                                            break;
                                                        case "NAME_USER_BLANK":
                                                            status_process = "Tên không để trống";
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                status_process = "Không có thông tin ĐƠN VỊ: " + item.DONVI.Trim() + ", vui lòng kiểm tra lại";
                                            }
                                        }
                                        else
                                        {
                                            // không có thông tin đơn vị (rỗng hoặc không nhập)
                                            // 2. Tạo người dùng KV
                                            // 3. Gán người dùng-đơn vị KV
                                            var check = taoNguoiDungDV(lstKV[0].ID.ToString(), item);
                                            switch (check)
                                            {
                                                case "EXIST_USER":
                                                    status_process = "Đã tồn tại người dùng";
                                                    break;
                                                case "NOT_EXIST_GROUP":
                                                    status_process = "Không tồn tại quyền, check lại tên chính xác";
                                                    break;
                                                case "OK":
                                                    status_process = "Tạo thành công";
                                                    break;
                                                case "FAIL":
                                                    status_process = "Không thành công";
                                                    break;
                                                case "NAME_USER_BLANK":
                                                    status_process = "Tên không để trống";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        status_process = "Không có thông tin KHU VỰC: " + item.KHUVUC.Trim() + ", vui lòng kiểm tra lại";
                                    }

                                }
                                else
                                {
                                    // 2. Tạo người dùng TCT
                                    // 3. Gán người dùng-đơn vị TCT
                                    var check = taoNguoiDungDV(lstTCT[0].ID.ToString(), item);
                                    switch (check)
                                    {
                                        case "EXIST_USER":
                                            status_process = "Đã tồn tại người dùng";
                                            break;
                                        case "NOT_EXIST_GROUP":
                                            status_process = "Không tồn tại quyền, check lại tên chính xác";
                                            break;
                                        case "OK":
                                            status_process = "Tạo thành công";
                                            break;
                                        case "FAIL":
                                            status_process = "Không thành công";
                                            break;
                                        case "NAME_USER_BLANK":
                                            status_process = "Tên không để trống";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                status_process = "Không có thông tin TỔNG CTY: " + item.TONGCT.Trim() + ", vui lòng kiểm tra lại";
                                return;
                            }
                        }
                        else
                        {
                            status_process = "Không có thông tin trên hệ thống";
                            return;
                        }



                        table.Rows[i]["TRANGTHAI"] = status_process;
                        i++;
                    }
                    //List<HT_EXCELFILE> lst = new List<HT_EXCELFILE>();
                    //lst = Utils.ConvertDataTable<HT_EXCELFILE>(table);
                    Session["isDone"] = "1";
                    Session["tblHeThongXLDONE"] = table;
                }
            }
            lblStatus.Text = "Đã import thành công!.";
            gvReadExcel.DataBind();
        }

        private string taoNguoiDungDV(string id_donvi, HT_EXCELFILE item)
        {
            string isSuccess = "";
            try
            {
                var id_donvi_add = id_donvi;
                var id_quyen_user = 0;
                using (var ctx = new ADDJContext())
                {
                    // kiểm tra xem tồn tại người dùng chưa
                    if (item.TENTRUYCAP.ToLower().Trim() != "")
                    {
                        var strSQLCheckND = string.Format(@"SELECT
                              Id
                             ,TenDoiTac
                             ,DoiTacId
                             ,KhuVucId
                             ,NhomNguoiDung
                             ,TenTruyCap
                             ,MatKhau
                             ,TenDayDu
                             ,NgaySinh
                             ,DiaChi
                             ,DiDong
                             ,CoDinh
                             ,Sex
                             ,Email
                             ,CongTy
                             ,DiaChiCongTy
                             ,FaxCongTy
                             ,DienThoaiCongTy
                             ,TrangThai
                             ,SuDungLDAP
                             ,LoginCount
                             ,LastLogin
                             ,IsLogin
                             ,LDate
                             ,CDate
                             ,CUser
                             ,LUser
                             ,ID_DONVI
                             ,XOA
                            FROM HT_HTKTTT.dbo.HT_NGUOIDUNG 
                            where TenTruyCap='{0}'",
                                item.TENTRUYCAP.ToLower().Trim());
                        var lstNguoiDung = ctx.Database.SqlQuery<HT_NGUOIDUNG>(strSQLCheckND).ToList();                        
                        if (lstNguoiDung.Any()) // nếu tồn tại người dùng
                        {
                            // kiểm tra xem user có tồn tại phòng nào
                            var strCheckDonVi = string.Format(@"select * from HT_NGUOIDUNG_DONVI where xoa=0 and ID_NGUOIDUNG={0}", lstNguoiDung[0].Id);
                            var lstIN = ctx.Database.SqlQuery<HT_NGUOIDUNG_DONVI>(strCheckDonVi).ToList();
                            // nếu tồn tại thì k làm gì
                            if (lstIN.Any())
                            {
                                isSuccess = "EXIST_USER";
                            }
                            else // nếu k tồn tại add mới
                            {
                                // nếu không tồn tại thì tạo mới ở đơn vị đó
                                var strSqlnddv = string.Format(@"INSERT INTO dbo.HT_NGUOIDUNG_DONVI
                                                    (
                                                      ID_NGUOIDUNG
                                                     ,ID_DONVI
                                                     ,TRANGTHAI
                                                     ,XOA
                                                     ,NGAYTAO
                                                    )
                                                    VALUES
                                                    (
                                                      {0} -- ID_NGUOIDUNG - int
                                                     ,{1} -- ID_DONVI - int
                                                     ,{2}
                                                     ,{3}
                                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                    )", lstNguoiDung[0].Id, id_donvi_add, 1, 0);
                                ctx.Database.ExecuteSqlCommand(strSqlnddv);
                                isSuccess = "OK";
                            }
                        }
                        else
                        {
                            // lấy thông tin nhóm người dùng
                            var checkNhom = false;
                            var strSQLNhomND = string.Format(@"SELECT
                                   Id
                                 ,Name
                                 ,Description
                                 ,Status
                                FROM HT_HTKTTT.dbo.NguoiSuDung_Group 
                                WHERE dbo.fChuyenCoDauThanhKhongDau(Name) LIKE N'%{0}%'", TienIch.convertToUnSign3(item.QUYEN.ToLower().Trim()));
                            var lstNhomNguoiDung = ctx.Database.SqlQuery<HT_NHOM_NGUOIDUNG>(strSQLNhomND).ToList();
                            if (lstNhomNguoiDung.Any())
                            {
                                id_quyen_user = lstNhomNguoiDung[0].Id;
                                checkNhom = true;
                            }
                            else
                            {
                                checkNhom = false;
                                isSuccess = "NOT_EXIST_GROUP";
                            }
                            if (checkNhom)
                            {
                                using (var trans = ctx.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var trang_thai = 0;
                                        var trangthai = "true";
                                        if (trangthai.ToLower() == "true")
                                            trang_thai = 1;
                                        string strSql = string.Format(@"INSERT INTO dbo.HT_NGUOIDUNG
                                                            (
                                                              TenDoiTac
                                                             ,DoiTacId
                                                             ,KhuVucId
                                                             ,NhomNguoiDung
                                                             ,TenTruyCap
                                                             ,MatKhau
                                                             ,TenDayDu
                                                             ,NgaySinh
                                                             ,DiaChi
                                                             ,DiDong
                                                             ,CoDinh
                                                             ,Sex
                                                             ,Email
                                                             ,CongTy
                                                             ,DiaChiCongTy
                                                             ,FaxCongTy
                                                             ,DienThoaiCongTy
                                                             ,TrangThai
                                                             ,SuDungLDAP
                                                             ,LoginCount
                                                             ,LastLogin
                                                             ,IsLogin
                                                             ,LDate
                                                             ,CDate
                                                             ,CUser
                                                             ,LUser
                                                             ,ID_DONVI
                                                             ,XOA
                                                            )
                                                            VALUES
                                                            (
                                                              N'{0}' -- TenDoiTac - nvarchar(200)
                                                             ,{1} -- DoiTacId - int
                                                             ,{2} -- KhuVucId - int
                                                             ,{3} -- NhomNguoiDung - int
                                                             ,'{4}' -- TenTruyCap - varchar(50)
                                                             ,N'{5}' -- MatKhau - nvarchar(200)
                                                             ,N'{6}' -- TenDayDu - nvarchar(200)
                                                             , CONVERT(DATETIME,'{7}',126) -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NgaySinh - datetime
                                                             ,N'{8}' -- DiaChi - nvarchar(500)
                                                             ,'{9}' -- DiDong - varchar(20)
                                                             ,'{10}' -- CoDinh - varchar(20)
                                                             ,{11} -- Sex - tinyint
                                                             ,N'{12}' -- Email - nvarchar(200)
                                                             ,N'{13}' -- CongTy - nvarchar(500)
                                                             ,N'{14}' -- DiaChiCongTy - nvarchar(500)
                                                             ,'{15}' -- FaxCongTy - varchar(20)
                                                             ,'{16}' -- DienThoaiCongTy - varchar(20)
                                                             ,{17} -- TrangThai - tinyint
                                                             ,{18} -- SuDungLDAP - bit
                                                             ,{19} -- LoginCount - int
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- LastLogin - datetime
                                                             ,{20} -- IsLogin - bit
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- LDate - datetime
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- CDate - datetime
                                                             ,'{21}' -- CUser - varchar(50)
                                                             ,'{22}' -- LUser - varchar(50)
                                                             ,{23} -- ID_DONVI - int
                                                             ,0
                                                            );  SELECT SCOPE_IDENTITY(); ",
                                        0, 0, 0, id_quyen_user, item.TENTRUYCAP.Trim(),
                                        Encrypt.MD5Admin("123456"),
                                        item.HOTEN, "2017-07-07", "", item.DIENTHOAI.Trim(), "", 1, item.EMAIL.Trim(), "", "", "", "", trang_thai, 0, 0, 0, 1, "", "0", id_donvi_add);
                                        //var rt = ctx.Database.ExecuteSqlCommand(strSql);
                                        var restchitiet = ctx.Database.SqlQuery<decimal>(strSql).ToList();
                                        decimal id_nguoidung = restchitiet.FirstOrDefault();

                                        var strSqlnddv = string.Format(@"INSERT INTO dbo.HT_NGUOIDUNG_DONVI
                                                    (
                                                      ID_NGUOIDUNG
                                                     ,ID_DONVI
                                                     ,TRANGTHAI
                                                     ,XOA
                                                     ,NGAYTAO
                                                    )
                                                    VALUES
                                                    (
                                                      {0} -- ID_NGUOIDUNG - int
                                                     ,{1} -- ID_DONVI - int
                                                     ,{2}
                                                     ,{3}
                                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                    )", id_nguoidung, id_donvi_add, trang_thai, 0);
                                        ctx.Database.ExecuteSqlCommand(strSqlnddv);
                                        trans.Commit();
                                        isSuccess = "OK";
                                    }
                                    catch (Exception ex)
                                    {
                                        isSuccess = "FAIL";
                                        trans.Rollback();
                                    }
                                }
                            }
                            else
                            {
                                isSuccess = "NOT_EXIST_GROUP";
                            }
                        }
                    }
                    else
                    {
                        isSuccess = "NAME_USER_BLANK";
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "EXCEL");
                isSuccess = "FAIL";
            }
            return isSuccess;
        }


        protected void lamMoi_Click(object sender, EventArgs e)
        {
            Session.Remove("tblHeThongXL");
            Session.Remove("isDone");
            Session.Remove("tblHeThongXLDONE");
            gvReadExcel.DataBind();
        }

        protected void gvReadExcel_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName=="TRANGTHAI")
            {
                switch (e.CellValue.ToString())
                {
                    case "Chưa xử lý":
                        e.Cell.ForeColor = System.Drawing.Color.LightPink;
                        e.Cell.Font.Bold = true;
                        break;
                    case "Tạo thành công":
                        e.Cell.ForeColor = System.Drawing.Color.Green;
                        e.Cell.Font.Bold = true;
                        break;
                    case "Đã tồn tại người dùng":
                        e.Cell.ForeColor = System.Drawing.Color.LightCoral;
                        e.Cell.Font.Bold = true;
                        break;
                    case "Không có thông tin trên hệ thống":
                        e.Cell.ForeColor = System.Drawing.Color.Red;
                        e.Cell.Font.Bold = true;
                        break;
                    case "Không tồn tại quyền, check lại tên chính xác":
                        e.Cell.ForeColor = System.Drawing.Color.Red;
                        e.Cell.Font.Bold = true;
                        break;
                    default:
                        e.Cell.ForeColor = System.Drawing.Color.Red;
                        e.Cell.Font.Bold = true;
                        break;
                }
            }
        }
    }
}