using ADDJ.Sercurity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Services;
using Website.HTHTKT;
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro.Services
{
    /// <summary>
    /// Summary description for ws_quanLy
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ws_quanLy : System.Web.Services.WebService
    {
        [WebMethod]
        public string xoaDonViTrongNode(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.HT_NODE_LUONG_HOTRO WHERE ID={0} ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa đơn vị trong node");
            }
            return ret;
        }

        [WebMethod]
        public string xoaNhieuDonViTrongNode(string arr_id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.HT_NODE_LUONG_HOTRO WHERE ID in ({0}) ", arr_id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa nhiều đơn vị trong node");
            }
            return ret;
        }

        [WebMethod]
        public string themDonViTrongNode(string id_hethong, string idluonghotro, string idbuocxuly, string arr_iddonvi)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;

            try
            {
                var lstdvid = arr_iddonvi.Trim().Split(' ').Distinct().ToArray();
                // lấy danh sách các đơn vị chưa có

                using (var ctx = new ADDJContext())
                {
                    var strdk = arr_iddonvi.Trim().Replace(' ', ',');
                    string strnotin = @"SELECT * FROM dbo.HT_NODE_LUONG_HOTRO WHERE
                                        ID_HETHONG_YCHT=1 AND ID_LUONG_HOTRO=1 AND BUOCXULY=1
                                        AND ID_DONVI NOT IN();";
                    for (int i = 0; i < lstdvid.Length; i++)
                    {
                        string strSql = string.Format(@"INSERT INTO [dbo].[HT_NODE_LUONG_HOTRO]
                                                       ([ID_HETHONG_YCHT]
                                                       ,[ID_LUONG_HOTRO]
                                                       ,[ID_DONVI]
                                                       ,[BUOCXULY]
                                                       ,[NGAYTAO])
                                                       VALUES
                                                       ({0}
                                                       ,{1}
                                                       ,{2}
                                                       ,{3}
                                                       ,GetDate()) ", 
                                                       id_hethong, idluonghotro, lstdvid[i], idbuocxuly);
                        var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    }
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm đơn vị trong node");
            }
            return ret;
        }
        [WebMethod]
        public string loadLuongTongQuan(string id_hethong, string idluonghotro, string tongsobuoc)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = "";
                    string strSqlHeader = "";
                    var tongsonode = Convert.ToInt32(tongsobuoc);
                    for (int i = 0; i < tongsonode; i++)
                    {
                        var lsestrSql = string.Format(@"SELECT b.Id,b.MADONVI,b.TENDONVI FROM dbo.HT_NODE_LUONG_HOTRO a
                        INNER JOIN	dbo.HT_DONVI b ON a.ID_DONVI=b.Id
                        WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND BUOCXULY={2}",
                        id_hethong, idluonghotro, i + 1);
                        var lst = ctx.Database.SqlQuery<ViewLuongDayDu2>(lsestrSql);
                        var lsttimeline = lst.ToList();
                        var node = "";
                        if (lsttimeline.Any())
                        {
                            int icount = 0;
                            foreach (var item in lsttimeline)
                            {
                                icount++;
                                node = node + icount + ", " + item.MADONVI + "-" + item.TENDONVI + "<br/>";
                            }
                        }
                        strSqlHeader = strSqlHeader + "<th>Bước " + (i + 1) + "</th>";
                        strSql = strSql + "<td>" + node + "</td>";
                    }
                    var noidingp = new ViewMapLuong();
                    noidingp.Header = "<tr>" + strSqlHeader + "</tr>";
                    noidingp.NoiDungMap = "<tr>" + strSql + "</tr>";
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(noidingp);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Nạp luồng tổng quan");
            }
            return ret;
        }

        // Quản lý node yêu cầu hỗ trợ


        [WebMethod]
        public string thongtinMucHeThongYCHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_DM_HETHONG_YCHT>();
                    string strSql = string.Format(@"SELECT
                                                      ID
                                                     ,TENHETHONG
                                                     ,MA_HETHONG
                                                     ,MOTA
                                                     ,MUCDO
                                                     ,TRANGTHAI
                                                     ,NGAYTAO
                                                     ,NGUOITAO
                                                     FROM HT_HTKTTT.dbo.HT_DM_HETHONG_YCHT 
                                                     WHERE ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_DM_HETHONG_YCHT>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục hệ thống yc");
            }
            return ret;
        }

        [WebMethod]
        public string xoaMucHeThongYCHT(string id_hethong)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.HT_DM_HETHONG_YCHT WHERE ID={0} ", id_hethong);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa mục hệ thống yc");
            }
            return ret;
        }
        [WebMethod]
        public string themMucHeThongYCHT(string tenhethong, string mahethong, string mota, string mucdo, string trangthai, string nguoitao)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT HT_DM_HETHONG_YCHT (TENHETHONG,MA_HETHONG, MOTA,MUCDO, TRANGTHAI, NGAYTAO, NGUOITAO)
                                                    VALUES (N'{0}',N'{1}', N'{2}', '{3}', {4}, GETDATE(), '{5}') ", 
                                                    tenhethong,mahethong, mota, mucdo, trang_thai, nguoitao);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục hệ thống yc");
            }
            return ret;
        }

        [WebMethod]
        public string suaMucHeThongYCHT(string id_hethong, string tenhethong, string mahethong, string mota, string mucdo, string trangthai, string nguoitao)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                if (mota == "null")
                {
                    mota = "";
                }

                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"UPDATE HT_DM_HETHONG_YCHT 
                                                    SET TENHETHONG = N'{1}'
                                                       ,MA_HETHONG = N'{2}'
                                                       ,MOTA = N'{3}'
                                                       ,MUCDO = N'{4}'
                                                       ,TRANGTHAI = {5}
                                                       ,NGAYTAO = GETDATE()
                                                       ,NGUOITAO = '{6}'
                                                    WHERE ID={0} ",
                                                    id_hethong, tenhethong, mahethong, mota,mucdo, trang_thai, nguoitao);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục hệ thống yc");
            }
            return ret;
        }

        [WebMethod]
        public string napDanhSachHeThongYCHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lstdmht = new List<HT_DM_HETHONG_YCHT>();
                    string strSql = string.Format(@"SELECT * FROM HT_DM_HETHONG_YCHT ");
                    var rt = ctx.Database.SqlQuery<HT_DM_HETHONG_YCHT>(strSql);
                    lstdmht = rt.ToList();
                    if (lstdmht.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstdmht);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Nạp danh sách ht yêu cầu");
            }
            return ret;
        }


        [WebMethod]
        public string themMucLuongXuLy(string id_hethong, string tenluong, string trangthai, string mota, string sobuoc)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT dbo.HT_LUONG_HOTRO
                                                            ( ID_HETHONG_YCHT ,
                                                              TEN_LUONG ,
                                                              TRANGTHAI ,
                                                              MOTA ,
                                                              SOBUOC
                                                            )
                                                    VALUES  ( {0} , -- ID_HETHONG_YCHT - int
                                                              N'{1}' , -- TEN_LUONG - nvarchar(200)
                                                              {2} , -- TRANGTHAI - bit
                                                              N'{3}' , -- MOTA - nvarchar(1000)
                                                              {4}  -- SOBUOC - int
                                                            ) ", 
                                                            id_hethong, tenluong, trang_thai, mota, sobuoc);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục luồng xử lý");
            }
            return ret;
        }

        [WebMethod]
        public string suaMucLuongXuLy(string id_luonght, string id_hethong, string tenluong, string trangthai, string mota, string sobuoc)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"UPDATE HT_LUONG_HOTRO 
                                                    SET ID_HETHONG_YCHT = {0}
                                                       ,TEN_LUONG = N'{1}'
                                                       ,TRANGTHAI = {2}
                                                       ,MOTA = N'{3}'
                                                       ,SOBUOC = {4}
                                                    WHERE ID = {5}",
                    id_hethong, tenluong, trang_thai, mota, sobuoc, id_luonght);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục luồng xử lý");
            }
            return ret;
        }

        [WebMethod]
        public string xoaMucLuongXuLy(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.HT_LUONG_HOTRO WHERE ID={0} ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa mục luồng xử lý");
            }
            return ret;
        }

        [WebMethod]
        public string thongtinMucLuongXuLy(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_LUONG_HOTRO>();
                    string strSql = string.Format(@"SELECT hlh.ID
                                                  ,hlh.ID_HETHONG_YCHT
                                                  ,hlh.TEN_LUONG
                                                  ,hlh.TRANGTHAI
                                                  ,hlh.MOTA
                                                  ,hlh.SOBUOC 
                                                  FROM HT_LUONG_HOTRO hlh where hlh.ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_LUONG_HOTRO>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục luồng xử lý");
            }
            return ret;
        }


        // begin: quan ly loai yeu cau ho tro trong cay thu muc
        [WebMethod]
        public string thongtinMucLoaiYeuCauHoTroHeThong(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_CAYTHUMUC_YCHT>();
                    string strSql = string.Format(@"SELECT
                                                  a.ID
                                                 ,a.ID_HETHONG_YCHT
                                                 ,a.LINHVUC
                                                 ,a.MA_LINHVUC
                                                 ,a.ID_CHA
                                                 ,a.GHICHU
                                                 ,a.TRANGTHAI
                                                 ,a.NGAYCAPNHAT
                                                 FROM HT_HTKTTT.dbo.HT_CAYTHUMUC_YCHT a 
                                                 WHERE a.ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_CAYTHUMUC_YCHT>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục loại yêu cầu hỗ trợ");
            }
            return ret;
        }

        // Danh mục loại trong cây thư mục của hệ thống cần hỗ trợ
        [WebMethod]
        public string thongtinDanhMucLoaiCayThuMucYCHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<LOAI_TRONGCAY>();
                    string strSql = string.Format(@"SELECT
                                                    DISTINCT a.LOAI
                                                    FROM HT_HTKTTT.dbo.HT_CAYTHUMUC_YCHT a 
                                                    WHERE a.ID_HETHONG_YCHT={0} ", id);
                    var rt = ctx.Database.SqlQuery<LOAI_TRONGCAY>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục loại cây thư mục ycht");
            }
            return ret;
        }
        // Danh mục lĩnh vực chung trong cây thư mục của hệ thống cần hỗ trợ
        [WebMethod]
        public string thongtinDanhMucLinhVucChungCayThuMucYCHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<LINHVUCCHUNG_TRONGCAY>();
                    string strSql = string.Format(@"SELECT
                                                    DISTINCT a.LINHVUCCHUNG
                                                    FROM HT_HTKTTT.dbo.HT_CAYTHUMUC_YCHT a 
                                                    WHERE a.ID_HETHONG_YCHT={0} ", id);
                    var rt = ctx.Database.SqlQuery<LINHVUCCHUNG_TRONGCAY>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin danh mục lĩnh vực chung ycht");
            }
            return ret;
        }
        // Danh mục lĩnh vực con trong cây thư mục của hệ thống cần hỗ trợ
        [WebMethod]
        public string thongtinDanhMucLinhVucConCayThuMucYCHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<LINHVUCCON_TRONGCAY>();
                    string strSql = string.Format(@"SELECT
                                                    DISTINCT a.LINHVUCCON
                                                    FROM HT_HTKTTT.dbo.HT_CAYTHUMUC_YCHT a 
                                                    WHERE a.ID_HETHONG_YCHT={0} ", id);
                    var rt = ctx.Database.SqlQuery<LINHVUCCON_TRONGCAY>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục lĩnh vực con cây thư mục ycht");
            }
            return ret;
        }
        // Danh mục cha trong cây thư mục của hệ thống cần hỗ trợ
        [WebMethod]
        public string thongtinDanhMucChaCayThuMucYCHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<CHA_TRONGCAY>();
                    string strSql = string.Format(@"SELECT
                                                    a.ID, a.LINHVUCCON
                                                    FROM HT_HTKTTT.dbo.HT_CAYTHUMUC_YCHT a 
                                                    WHERE a.ID_HETHONG_YCHT={0} ", id);
                    var rt = ctx.Database.SqlQuery<CHA_TRONGCAY>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin danh mục cha cây ycht");
            }
            return ret;
        }
        // Danh mục đơn vị tiếp nhận xử lý trong cây thư mục của hệ thống cần hỗ trợ
        [WebMethod]
        public string thongtinDanhMucDonViTNXLCayThuMucYCHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<DONVI_TIEPNHAN_XL_TRONGCAY>();
                    string strSql = string.Format(@"SELECT
                                                    DISTINCT a.DONVI_TIEPNHAN_XL
                                                    FROM HT_HTKTTT.dbo.HT_CAYTHUMUC_YCHT a 
                                                    WHERE a.ID_HETHONG_YCHT={0} ", id);
                    var rt = ctx.Database.SqlQuery<DONVI_TIEPNHAN_XL_TRONGCAY>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin danh mục đơn vị xl ycht");
            }
            return ret;
        }

        // thêm mới mục trong cây thư mục ứng với ht
        [WebMethod]
        public string themMucCayThuMucHeThongYCHT(string id_hethong_htkt, string loai, string linhvucchung, string linhvuccon, string id_cha, string donvi_tnxl, string donvi_tn, string donvi_tnxl_dautien, string donvi_phoihop, string ghichu, string trangthai)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT INTO dbo.HT_CAYTHUMUC_YCHT
                                                    (
                                                      ID_HETHONG_YCHT
                                                     ,LINHVUC
                                                     ,ID_CHA
                                                     ,GHICHU
                                                     ,TRANGTHAI
                                                     ,NGAYCAPNHAT
                                                    )
                                                    VALUES
                                                    (
                                                      {0} -- ID_HETHONG_YCHT - int
                                                     ,N'{1}' -- LINHVUC - nvarchar(2000)
                                                     ,{2} -- ID_CHA - int
                                                     ,N'{3}' -- GHICHU - nvarchar(200)
                                                     ,{4} -- TRANGTHAI - bit
                                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYCAPNHAT - datetime
                                                    ) ",
                                                    id_hethong_htkt, loai, linhvucchung, linhvuccon, id_cha, donvi_tnxl, donvi_tn, donvi_tnxl_dautien, donvi_phoihop, ghichu, trang_thai);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục cây thư mục hệ thống ycht");
            }
            return ret;
        }

        [WebMethod]
        public string themMucCayThuMucHeThongYCHTNew(string id_hethong_htkt, string linhvuc, string malinhvuc, string id_cha, string ghichu, string trangthai)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;

                    if (id_cha.Trim() == "")
                    {
                        id_cha = "null";
                    }
                    string strSql = string.Format(@"INSERT INTO dbo.HT_CAYTHUMUC_YCHT
                                                    (
                                                      ID_HETHONG_YCHT
                                                     ,LINHVUC
                                                     ,MA_LINHVUC
                                                     ,ID_CHA
                                                     ,GHICHU
                                                     ,TRANGTHAI
                                                     ,NGAYCAPNHAT
                                                    )
                                                    VALUES
                                                    (
                                                      {0} -- ID_HETHONG_YCHT - int
                                                     ,N'{1}' -- LINHVUC - nvarchar(2000)
                                                     ,N'{2}' -- LINHVUC - nvarchar(2000)
                                                     ,{3} -- ID_CHA - int
                                                     ,N'{4}' -- GHICHU - nvarchar(200)
                                                     ,{5} -- TRANGTHAI - bit
                                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYCAPNHAT - datetime
                                                    ) ",
                                                    id_hethong_htkt, linhvuc, malinhvuc, id_cha, ghichu, trang_thai);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục cây thư mục hệ thống ycht");
            }
            return ret;
        }


        // sửa mục trong cây thư mục ứng với ht
        [WebMethod]
        public string suaMucCayThuMucHeThongYCHT(string id_hethong_htkt, string loai, string linhvucchung, string linhvuccon, string id_cha, string donvi_tnxl, string donvi_tn, string donvi_tnxl_dautien, string donvi_phoihop, string ghichu, string trangthai, string id_muccaythumuc)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;

                    if (id_cha.Trim() == "")
                    {
                        id_cha = "null";
                    }
                    string strSql = string.Format(@"UPDATE dbo.HT_CAYTHUMUC_YCHT 
                                                    SET
                                                      ID_HETHONG_YCHT = {0} -- ID_HETHONG_YCHT - int
                                                     ,LOAI = N'{1}' -- LOAI - nvarchar(2000)
                                                     ,LINHVUCCHUNG = N'{2}' -- LINHVUCCHUNG - nvarchar(2000)
                                                     ,LINHVUCCON = N'{3}' -- LINHVUCCON - nvarchar(2000)
                                                     ,ID_CHA = {4} -- ID_CHA - int
                                                     ,DONVI_TIEPNHAN_XL = N'{5}' -- DONVI_TIEPNHAN_XL - nvarchar(50)
                                                     ,DONVI_TIEPNHAN = N'{6}' -- DONVI_TIEPNHAN - nvarchar(50)
                                                     ,DONVI_TNXL_DAUTIEN = N'{7}' -- DONVI_TNXL_DAUTIEN - nvarchar(50)
                                                     ,DONVI_PHOIHOP = N'{8}' -- DONVI_PHOIHOP - nvarchar(50)
                                                     ,GHICHU = N'{9}' -- GHICHU - nvarchar(200)
                                                     ,TRANGTHAI = {10} -- TRANGTHAI - bit
                                                     ,NGAYCAPNHAT = GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYCAPNHAT - datetime
                                                    WHERE ID = {11} -- ID - int NOT NULL",
                    id_hethong_htkt, loai, linhvucchung, linhvuccon, id_cha, donvi_tnxl, donvi_tn, donvi_tnxl_dautien, donvi_phoihop, ghichu, trang_thai, id_muccaythumuc);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục cây thư mục hệ thống ycht");
            }
            return ret;
        }


        [WebMethod]
        public string suaMucCayThuMucHeThongYCHTNew(string id_hethong_htkt, string linhvuc,string malinhvuc, string id_cha, string ghichu, string trangthai, string id_muccaythumuc)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;

                    if (id_cha.Trim() == "")
                    {
                        id_cha = "null";
                    }
                    string strSql = string.Format(@"UPDATE dbo.HT_CAYTHUMUC_YCHT 
                                                    SET
                                                      ID_HETHONG_YCHT = {0} -- ID_HETHONG_YCHT - int
                                                     ,LINHVUC = N'{1}' -- LINHVUC - nvarchar(2000)
                                                     ,MA_LINHVUC = N'{2}' -- MA_LINHVUC - nvarchar(2000)
                                                     ,ID_CHA = {3} -- ID_CHA - int
                                                     ,GHICHU = N'{4}' -- GHICHU - nvarchar(200)
                                                     ,TRANGTHAI = {5} -- TRANGTHAI - bit
                                                     ,NGAYCAPNHAT = GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYCAPNHAT - datetime
                                                    WHERE ID = {6} -- ID - int NOT NULL",
                    id_hethong_htkt, linhvuc, malinhvuc, id_cha, ghichu, trang_thai, id_muccaythumuc);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục cây thư mục hệ thống ycht");
            }
            return ret;
        }

        [WebMethod]
        public string xoaAllMucTrongCayYeuCauHTByID(string id_caythumuc)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    // xóa recusion với id cha gốc ban đầu
                    string strSql = string.Format(@";WITH cte AS
                      (
      	                SELECT *
      	                FROM dbo.HT_CAYTHUMUC_YCHT WHERE id={0}      
      	                UNION ALL      
      	                SELECT t2.*
      	                FROM cte t1
      	                JOIN dbo.HT_CAYTHUMUC_YCHT t2 ON t1.ID = t2.ID_CHA
                      )
                      DELETE HT_CAYTHUMUC_YCHT 
                      FROM cte WHERE HT_CAYTHUMUC_YCHT.ID=cte.ID  
                       ", id_caythumuc);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa all mục yêu cầu hỗ trợ theo ID");
            }
            return ret;
        }

        [WebMethod]
        public string xoaMucTrongCayThuMuc(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.HT_NODE_LUONG_HOTRO WHERE ID={0} ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa mục trong cây thư mục");
            }
            return ret;
        }
        // end


        // don vi
        [WebMethod]
        public string thongtinDonViTheoID(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_DONVI>();
                    string strSql = string.Format(@"SELECT hd.ID
                      ,hd.MADONVI
                      ,hd.TENDONVI
                      ,hd.ID_CHA
                      ,hd.MOTA
                      ,hd.LOAIDONVI
                      ,hd.GHICHU
                      ,hd.XOA
                      ,hd.DIENTHOAI
                      ,hd.FAX
                      ,hd.DIACHI
                      ,hd.TRANGTHAI
                      ,hd.ID_TINHTHANH
                      ,hd.NGAYCAPNHAT FROM HT_DONVI hd WHERE hd.ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_DONVI>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin danh mục đơn vị xl ycht");
            }
            return ret;
        }
        [WebMethod]
        public string themDonVi(string madonvi, string tendonvi, string idcha, string mota, string loaidonvi, string ghichu, string xoa, string dienthoai, string fax, string diachi, string trangthai, string idtinhthanh)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    if (idcha=="")
                    {
                        idcha = "0";
                    }
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT INTO 
                                                    HT_DONVI (MADONVI, TENDONVI, TENDONVI_KD, ID_CHA, MOTA, LOAIDONVI, GHICHU, XOA, DIENTHOAI, FAX, DIACHI, TRANGTHAI, ID_TINHTHANH, NGAYCAPNHAT)
                                                    VALUES (N'{0}', N'{1}', {2}, N'{3}', N'{4}', N'{5}', {6}, N'{7}', N'{8}', N'{9}', N'{10}', {11}, {12}, GETDATE()) ",
                                                    madonvi, tendonvi, "N'"+ TienIch.convertToUnSign3(tendonvi) +"'", idcha, mota, loaidonvi, ghichu, xoa, dienthoai, fax, diachi, trang_thai, idtinhthanh);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục cây thư mục hệ thống ycht");
            }
            return ret;
        }
        [WebMethod]
        public string suaDonVi(string id, string madonvi, string tendonvi, string idcha, string mota, string loaidonvi, string ghichu, string xoa, string dienthoai, string fax, string diachi, string trangthai, string idtinhthanh)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"UPDATE HT_DONVI 
                        SET 
                            MADONVI = N'{0}'
                           ,TENDONVI = N'{1}'
                           ,TENDONVI_KD=N'{2}'
                           ,ID_CHA = {3}
                           ,MOTA = N'{4}'
                           ,LOAIDONVI = N'{5}'
                           ,GHICHU = N'{6}'
                           ,XOA = {7}
                           ,DIENTHOAI = N'{8}'
                           ,FAX = N'{9}'
                           ,DIACHI = N'{10}'
                           ,TRANGTHAI = {11}
                           ,ID_TINHTHANH = {12}
                           ,NGAYCAPNHAT = GETDATE()
                        WHERE ID = {13} ",
                    madonvi, tendonvi, TienIch.convertToUnSign3(tendonvi), idcha, mota, loaidonvi, ghichu, xoa, dienthoai, fax, diachi, trang_thai, idtinhthanh,
                    id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục cây thư mục hệ thống ycht");
            }
            return ret;
        }
        [WebMethod]
        public string xoaDonVi(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    // kiểm tra xem có user nào bất kỳ thuộc đơn vị không


                    // xóa recusion với id cha gốc ban đầu
                    string strSql = string.Format(@"
                      ;WITH cte AS
                      (
      	                SELECT *
      	                FROM dbo.HT_DONVI WHERE id={0}      
      	                UNION ALL      
      	                SELECT t2.*
      	                FROM cte t1
      	                JOIN dbo.HT_DONVI t2 ON t1.ID = t2.ID_CHA
                      )
                      DELETE HT_DONVI 
                      FROM cte WHERE HT_DONVI.ID=cte.ID 
                       ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa all mục yêu cầu hỗ trợ theo ID");
            }
            return ret;
        }


        // end don vi

        // begin nguoi dung
        [WebMethod]
        public string kiemtraTenDangNhapNguoiDung(string tentruycap)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<string>();
                    string strSql = string.Format(@"SELECT TenTruyCap FROM HT_NGUOIDUNG hn
                      WHERE hn.TenTruyCap='{0}' ", tentruycap);
                    var rt = ctx.Database.SqlQuery<string>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "kiểm tra người dùng");
            }
            return ret;
        }


        [WebMethod]
        public string thongtinNguoiDungTheoID(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_NGUOIDUNG>();
                    string strSql = string.Format(@"SELECT hn.Id
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
                      ,hn.ID_DONVI FROM HT_NGUOIDUNG hn 
                
                      WHERE hn.Id={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_NGUOIDUNG>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin danh mục đơn vị xl ycht");
            }
            return ret;
        }
        [WebMethod]
        public string themNguoiDung(string tendoitac, string doitacid, string khuvucid, string nhomnguoidung, string tentruycap, string matkhau, string tendaydu, string manhanvien_cmt, string ngaysinh, string diachi, string didong, string codinh, string sex, string email, string congty, string diachicongty, string faxcongty, string dienthoaicongty, string trangthai, string sudungldap, string logincount, string lastlogin, string islogin, string cuser, string luser, string iddonvi, string iddonvi_cu, string is_giucu_vathemmoi)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                var id_donvi_add = "";
                if (iddonvi == "") // Nếu không có sự thay đổi đơn vị ở form thêm mới, lấy cái id đã chọn
                {
                    id_donvi_add = iddonvi_cu;
                }
                else // ngược lại lấy theo id chọn mới
                {
                    id_donvi_add = iddonvi;
                }

                using (var ctx = new ADDJContext())
                {
                    using (var trans = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            var trang_thai = 0;
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
                                                             ,MaNhanVienCMT
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
                                                             ,{24}
                                                            );  SELECT SCOPE_IDENTITY(); ",
                            tendoitac, doitacid, khuvucid, nhomnguoidung, tentruycap,
                            Encrypt.MD5Admin(matkhau),
                            tendaydu, ngaysinh, diachi, didong, codinh, sex, email, congty, diachicongty, faxcongty, dienthoaicongty, trang_thai, sudungldap, logincount, lastlogin, islogin, cuser, luser, id_donvi_add, manhanvien_cmt);
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
                                                     ,0
                                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                    )", id_nguoidung, id_donvi_add, trang_thai);
                            ctx.Database.ExecuteSqlCommand(strSqlnddv);
                            ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Actions.ActionProcess.GhiLog(ex, "BC");
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
                Actions.ActionProcess.GhiLog(ex, "Thêm người dùng, đơn vị người dùng");
            }
            return ret;
        }
        [WebMethod]
        public string suaNguoiDung(string id, string tendoitac, string doitacid, string khuvucid, string nhomnguoidung, string tentruycap, string matkhau, string tendaydu, string manhanvien_cmt, string ngaysinh, string diachi, string didong, string codinh, string sex, string email, string congty, string diachicongty, string faxcongty, string dienthoaicongty, string trangthai, string sudungldap, string logincount, string lastlogin, string islogin, string cuser, string luser, string iddonvi, string iddonvi_cu, string id_donvi_nguoidung, string is_giucu_vathemmoi)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    using (var trans = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            var trang_thai = 0;
                            if (trangthai.ToLower() == "true")
                                trang_thai = 1;

                            // nếu có nhập mk
                            string strSql = string.Empty;
                            if (matkhau.Trim() != "")
                            {
                                strSql = string.Format(@"UPDATE HT_NGUOIDUNG 
                                                    SET TenDoiTac = N'{0}'
                                                       ,DoiTacId = {1}
                                                       ,KhuVucId = {2}
                                                       ,NhomNguoiDung = {3}
                                                       ,TenTruyCap = '{4}'
                                                       ,MatKhau='{24}'
                                                       ,TenDayDu = N'{5}'
                                                       ,NgaySinh = convert(datetime, '{6}', 126)
                                                       ,DiaChi = N'{7}'
                                                       ,DiDong = '{8}'
                                                       ,CoDinh = '{9}'
                                                       ,Sex = {10}
                                                       ,Email = N'{11}'
                                                       ,CongTy = N'{12}'
                                                       ,DiaChiCongTy = N'{13}'
                                                       ,FaxCongTy = '{14}'
                                                       ,DienThoaiCongTy = '{15}'
                                                       ,TrangThai={25}
                                                       ,XOA = {16}
                                                       ,SuDungLDAP = {17}
                                                       ,LoginCount = {18}                                                      
                                                       ,IsLogin = {19}
                                                       ,LDate = GETDATE()                                                     
                                                       ,CUser = '{20}'
                                                       ,LUser = '{21}'
                                                       ,MaNhanVienCMT= '{26}'
                                                       {22}
                                                    WHERE ID={23}",
                            tendoitac, doitacid, khuvucid, nhomnguoidung, tentruycap,
                            tendaydu, ngaysinh, diachi, didong, codinh, sex, email, congty,
                            diachicongty, faxcongty, dienthoaicongty, 0, sudungldap,
                            logincount, islogin, cuser, luser, iddonvi_cu != "0" ? ",ID_DONVI = " + iddonvi_cu : "",
                            id,
                            Encrypt.MD5Admin(matkhau), trang_thai, manhanvien_cmt);
                            }
                            else
                            {
                                strSql = string.Format(@"UPDATE HT_NGUOIDUNG 
                                                    SET TenDoiTac = N'{0}'
                                                       ,DoiTacId = {1}
                                                       ,KhuVucId = {2}
                                                       ,NhomNguoiDung = {3}
                                                       ,TenTruyCap = '{4}'
                                                       ,TenDayDu = N'{5}'
                                                       ,NgaySinh = convert(datetime, '{6}', 126)
                                                       ,DiaChi = N'{7}'
                                                       ,DiDong = '{8}'
                                                       ,CoDinh = '{9}'
                                                       ,Sex = {10}
                                                       ,Email = N'{11}'
                                                       ,CongTy = N'{12}'
                                                       ,DiaChiCongTy = N'{13}'
                                                       ,FaxCongTy = '{14}'
                                                       ,DienThoaiCongTy = '{15}'
                                                       ,XOA = {16}
                                                       ,TrangThai={24}
                                                       ,SuDungLDAP = {17}
                                                       ,LoginCount = {18}                                                      
                                                       ,IsLogin = {19}
                                                       ,LDate = GETDATE()                                                     
                                                       ,CUser = '{20}'
                                                       ,LUser = '{21}'
                                                       ,MaNhanVienCMT= '{25}'
                                                       {22}
                                                    WHERE ID={23}",
                            tendoitac, doitacid, khuvucid, nhomnguoidung, tentruycap,
                            tendaydu, ngaysinh, diachi, didong, codinh, sex, email, congty,
                            diachicongty, faxcongty, dienthoaicongty, 0, sudungldap,
                            logincount, islogin, cuser, luser, iddonvi_cu != "0" ? ",ID_DONVI = " + iddonvi_cu : "",
                            id, trang_thai, manhanvien_cmt);
                            }
                        
                            
                            var rt = ctx.Database.ExecuteSqlCommand(strSql);
                            if (iddonvi != "")
                            {
                                string strCmdDel = string.Format(@"UPDATE HT_NGUOIDUNG_DONVI 
                                                               SET ID_DONVI={0}, TRANGTHAI={1}, XOA=0,NGAYTAO=GETDATE()
                                                               WHERE ID={2} ", iddonvi, trang_thai, id_donvi_nguoidung);
                                ctx.Database.ExecuteSqlCommand(strCmdDel);
                            }
                            else
                            {
                                string strCmdDel = string.Format(@"UPDATE HT_NGUOIDUNG_DONVI 
                                                               SET TRANGTHAI={0}, XOA=0,NGAYTAO=GETDATE()
                                                               WHERE ID={1} ", trang_thai, id_donvi_nguoidung);
                                ctx.Database.ExecuteSqlCommand(strCmdDel);
                            }
                         
                            //// Sửa người dùng đơn vị nếu chuyển đơn vị, hoặc vừa ở đơn vị cũ và sang đơn vị mới
                            //if (is_giucu_vathemmoi.ToLower() != "true" && iddonvi != iddonvi_cu)
                            //{
                            //    string strCmdDel = string.Format(@"UPDATE HT_NGUOIDUNG_DONVI 
                            //                                       SET TRANGTHAI=0, XOA=1,NGAYTAO=GETDATE()
                            //                                       WHERE ID_NGUOIDUNG={0} AND ID_DONVI={1} and ID={2} ",
                            //                                       id, iddonvi_cu, id_donvi_nguoidung);
                            //    ctx.Database.ExecuteSqlCommand(strCmdDel);
                            //}

                            //// tạo người dùng ở đơn vị mới
                            //if (is_giucu_vathemmoi.ToLower() == "true" && iddonvi != iddonvi_cu)
                            //{
                            //    string strDonViAdd = String.Format(@"INSERT INTO dbo.HT_NGUOIDUNG_DONVI
                            //                                    (
                            //                                      ID_NGUOIDUNG
                            //                                     ,ID_DONVI
                            //                                     ,TRANGTHAI
                            //                                     ,XOA
                            //                                     ,NGAYTAO
                            //                                    )
                            //                                    VALUES
                            //                                    (
                            //                                      {0} -- ID_NGUOIDUNG - int
                            //                                     ,{1}-- ID_DONVI - int                                                                
                            //                                     ,1 -- TRANGTHAI - int
                            //                                     ,0
                            //                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                            //                                    ) ", id, iddonvi);
                            //    ctx.Database.ExecuteSqlCommand(strDonViAdd);
                            //}


                            //// nếu thông tin đơn vị cũ vẫn giữ nguyên và 
                            //if (iddonvi == iddonvi_cu && is_giucu_vathemmoi.ToLower() != "true")
                            //{
                            //    string strCmdDel = string.Format(@"UPDATE HT_NGUOIDUNG_DONVI 
                            //                                       SET TRANGTHAI={0},NGAYTAO=GETDATE()
                            //                                       WHERE ID={1} ", 
                            //                                       trang_thai, id_donvi_nguoidung);
                            //    ctx.Database.ExecuteSqlCommand(strCmdDel);
                            //}

                            ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Actions.ActionProcess.GhiLog(ex, "BC");
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
                Actions.ActionProcess.GhiLog(ex, "Sửa thông tin người dùng");
            }
            return ret;
        }
        [WebMethod]
        public string xoaNguoiDung(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    using (var trans = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            // xóa người dùng trong đơn vị
                            //string strCmdDel = string.Format(@"");
                            //ctx.Database.ExecuteSqlCommand(strCmdDel);

                            
                            // xóa người dùng
                            string strSql = string.Format(@"
                                  DELETE HT_NGUOIDUNG 
                                  WHERE ID={0}", id);
                            var rt = ctx.Database.ExecuteSqlCommand(strSql);

                            ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
                Actions.ActionProcess.GhiLog(ex, "Xóa all mục yêu cầu hỗ trợ theo ID");
            }
            return ret;
        }


        [WebMethod]
        public string xoaNguoiDungVaDonViNguoiDung(string id, string id_donvi, string id_donvi_nguoidung)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            using (var ctx = new ADDJContext())
            {
                try
                {
                    // xóa người dùng trong đơn vị
                    string strCmdDel = string.Format(@"UPDATE HT_NGUOIDUNG_DONVI 
                                                               SET TRANGTHAI=0,XOA=1,NGAYTAO=GETDATE()
                                                               WHERE ID={0}", id_donvi_nguoidung);
                    ctx.Database.ExecuteSqlCommand(strCmdDel);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);

                    string strCmdDel111 = string.Format(@"
                             UPDATE HT_NGUOIDUNG SET TenTruyCap=TenTruyCap+'_del',TrangThai=0, XOA=1, LDate=GETDATE() WHERE Id={0}", id);
                    ctx.Database.ExecuteSqlCommand(strCmdDel111);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
                catch (Exception ex)
                {
                    Actions.ActionProcess.GhiLog(ex, "BC");
                    Actions.ActionProcess.GhiLog(ex, "Xóa all mục yêu cầu hỗ trợ theo ID");
                }
            }
            return ret;
        }

        [WebMethod]
        public string timkiemnguoidung(string tentruycap)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_NGUOIDUNG>();
                    string strSql = string.Format(@"SELECT hn.Id
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
                      ,hn.ID_DONVI FROM HT_NGUOIDUNG hn 
                
                      WHERE TenTruyCap like N'%{0}%' ", tentruycap);
                    var rt = ctx.Database.SqlQuery<HT_NGUOIDUNG>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Tìm kiếm người dùng");
            }
            return ret;
        }

        // end nguoi dung


        [WebMethod]
        public string themMucNhomQuyenHT(string tennhomquyen, string mota, string trangthai)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT INTO 
                                                    NguoiSuDung_Group (Name, Description, Status)
                                                    VALUES (N'{0}', N'{1}', {2}) ",
                                                            tennhomquyen, mota, trang_thai);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục nhóm quyền");
            }
            return ret;
        }

        [WebMethod]
        public string suaMucNhomQuyenHT(string id, string tennhomquyen, string mota, string trangthai)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"UPDATE NguoiSuDung_Group 
                                                    SET Name = N'{0}'
                                                       ,Description = N'{1}'
                                                       ,Status = {2}
                                                    WHERE Id = {3}",
                    tennhomquyen, mota, trang_thai,id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục nhóm quyền");
            }
            return ret;
        }

        [WebMethod]
        public string xoaMucNhomQuyenHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.NguoiSuDung_Group WHERE ID={0} ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa mục nhóm quyền");
            }
            return ret;
        }
        [WebMethod]
        public string thongtinMucNhomQuyenHT(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_NHOM_NGUOIDUNG>();
                    string strSql = string.Format(@"SELECT
                      Id
                     ,Name
                     ,Description
                     ,Status
                    FROM dbo.NguoiSuDung_Group where ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_NHOM_NGUOIDUNG>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục nhóm quyền");
            }
            return ret;
        }


        // mức độ sự cố
        [WebMethod]
        public string themMucMucDoSuCo(string tenmucdo, string trangthai)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT INTO 
                                                    HT_MUCDO_SUCO (TENMUCDO, TRANGTHAI, NGAYTAO)
                                                    VALUES (N'{0}', N'{1}', getdate()) ",
                                                            tenmucdo, trang_thai);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục mức độ sự cố");
            }
            return ret;
        }

        [WebMethod]
        public string suaMucMucDoSuCo(string id, string tenmucdo, string trangthai)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"UPDATE HT_MUCDO_SUCO 
                                                    SET TENMUCDO = N'{0}'                                                       
                                                       ,TRANGTHAI = {1}
                                                       , NGAYTAO=getdate()
                                                    WHERE ID = {2}",
                    tenmucdo, trang_thai, id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục mức độ sự cố");
            }
            return ret;
        }

        [WebMethod]
        public string xoaMucMucDoSuCo(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.HT_MUCDO_SUCO WHERE ID={0} ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa mục nhóm quyền");
            }
            return ret;
        }
        [WebMethod]
        public string thongtinMucMucDoSuCo(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_MUCDO_SUCO>();
                    string strSql = string.Format(@"SELECT
                      ID
                     ,TENMUCDO
                     ,TRANGTHAI
                     ,NGAYTAO
                    FROM dbo.HT_MUCDO_SUCO where ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_MUCDO_SUCO>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục mức độ sự cố");
            }
            return ret;
        }

        // quản lý menu 
        [WebMethod]
        public string thongtinMenuTheoID(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_MENU_F>();
                    string strSql = string.Format(@"SELECT
                                  ID
                                 ,STT
                                 ,ParentID
                                 ,Name
                                 ,Name2
                                 ,Name3
                                 ,Link
                                 ,Display
                                 ,MenuType
                                FROM dbo.Menu WHERE ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_MENU_F>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin menu theo id");
            }
            return ret;
        }
        [WebMethod]
        public string themMenu(string stt, string idcha, string name, string name2, string name3, string link, string display, string menutype)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                if (idcha.Trim()=="")
                {
                    idcha = "0";
                }
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (display.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT into 
                            Menu (STT, ParentID, Name, Name2, Name3, Link, Display, MenuType)
                           VALUES ({0}, {1}, N'{2}', N'{3}', N'{4}', N'{5}', {6}, {7}) ",
                    stt, idcha, name,name2,name3,link,trang_thai,menutype);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục cây thư mục hệ thống ycht");
            }
            return ret;
        }
        [WebMethod]
        public string suaMenu(string id, string stt, string idcha, string name, string name2, string name3, string link, string display, string menutype)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (display.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"update Menu 
                                                    SET STT = {0}
                                                       ,ParentID = {1}
                                                       ,Name = N'{2}'
                                                       ,Name2 = N'{3}'
                                                       ,Name3 = N'{4}'
                                                       ,Link = N'{5}'
                                                       ,Display = {6}
                                                       ,MenuType = {7}
                                                    WHERE ID = {8} ",
                    stt, idcha, name, name2, name3, link, trang_thai, menutype, id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục cây menu");
            }
            return ret;
        }
        [WebMethod]
        public string xoaMenu(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    // kiểm tra xem có user nào bất kỳ thuộc đơn vị không


                    // xóa recusion với id cha gốc ban đầu
                    string strSql = string.Format(@"
                      ;WITH cte AS
                      (
      	                SELECT *
      	                FROM dbo.Menu WHERE id={0}      
      	                UNION ALL      
      	                SELECT t2.*
      	                FROM cte t1
      	                JOIN dbo.Menu t2 ON t1.Id = t2.ParentID
                      )
                      DELETE Menu 
                      FROM cte WHERE Menu.ID=cte.ID 
                       ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa all mục menu theo id");
            }
            return ret;
        }

        // đâu mối
        [WebMethod]
        public string thongtinDauMoiTheoID(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_DAUMOI_XL>();
                    string strSql = string.Format(@"SELECT hdx.ID
                                      ,hdx.HOTEN
                                      ,hdx.ID_DONVI
                                      ,hd.TENDONVI
                                      ,hdx.ID_HETHONG
                                      ,hdhy.TENHETHONG
                                      ,hdx.DIENTHOAI
                                      ,hdx.EMAIL
                                      ,hdx.TRANGTHAI
                                      ,hdx.NGUOITAO
                                      ,hdx.NGAYTAO FROM HT_DAUMOI_XL hdx
                                  INNER JOIN HT_DM_HETHONG_YCHT hdhy ON hdhy.ID=hdx.ID_HETHONG
                                  INNER JOIN HT_DONVI hd ON hd.ID=hdx.ID_DONVI 
                                  where hdx.ID={0}", id);
                    var rt = ctx.Database.SqlQuery<HT_DAUMOI_XL>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin menu theo id");
            }
            return ret;
        }
        [WebMethod]
        public string themDauMoi(string hoten, string iddonvi, string idhethong, string dienthoai, string email, string trangthai, string nguoitao)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT INTO dbo.HT_DAUMOI_XL
                                                    (
                                                      HOTEN
                                                     ,ID_DONVI
                                                     ,ID_HETHONG
                                                     ,DIENTHOAI
                                                     ,EMAIL
                                                     ,TRANGTHAI
                                                     ,NGUOITAO
                                                     ,NGAYTAO
                                                    )
                                                    VALUES
                                                    (
                                                      N'{0}' -- HOTEN - nvarchar(50)
                                                     ,{1} -- ID_DONVI - int
                                                     ,{2} -- ID_HETHONG - int
                                                     ,N'{3}' -- DIENTHOAI - nvarchar(50)
                                                     ,N'{4}' -- EMAIL - nvarchar(50)
                                                     ,{5} -- TRANGTHAI - int
                                                     ,N'{6}' -- NGUOITAO - nvarchar(50)
                                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                    ) ",
                    hoten, iddonvi, idhethong,dienthoai,email,trang_thai,nguoitao);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thêm mục cây thư mục hệ thống ycht");
            }
            return ret;
        }
        [WebMethod]
        public string suaDauMoi(string id, string hoten, string iddonvi, string idhethong, string dienthoai, string email, string trangthai, string nguoitao)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var trang_thai = 0;
                    if (trangthai.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"UPDATE dbo.HT_DAUMOI_XL 
                                                    SET
                                                      HOTEN = N'{0}' -- HOTEN - nvarchar(50)
                                                     ,ID_DONVI = {1} -- ID_DONVI - int
                                                     ,ID_HETHONG = {2} -- ID_HETHONG - int
                                                     ,DIENTHOAI = N'{3}' -- DIENTHOAI - nvarchar(50)
                                                     ,EMAIL = N'{4}' -- EMAIL - nvarchar(50)
                                                     ,TRANGTHAI = {5} -- TRANGTHAI - int
                                                     ,NGUOITAO = N'{6}' -- NGUOITAO - nvarchar(50)
                                                     ,NGAYTAO = GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                    WHERE
                                                    ID={7} ",
                    hoten, iddonvi, idhethong, dienthoai, email, trang_thai, nguoitao, id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Sửa mục cây menu");
            }
            return ret;
        }
        [WebMethod]
        public string xoaDauMoi(string id)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"
                        DELETE FROM dbo.HT_DAUMOI_XL
                        WHERE
                        ID={0}
                       ", id);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Xóa all mục menu theo id");
            }
            return ret;
        }
        //

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
