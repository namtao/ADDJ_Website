using ADDJ.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Website.HTHTKT;
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro.Services
{
    /// <summary>
    /// Summary description for ws_thongTinHoTro
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ws_thongTinHoTro : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// Lấy thông tin chi tiết luồng hiện tại
        /// </summary>
        /// <param name="id_ht_xuly_hotro"></param>
        /// <returns></returns>
        [WebMethod]
        public string loadThongTinXuLyHT(string idchitietxlhotro)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@" select a.ID
                                  ,a.ID_YEUCAU_HOTRO_HT
                                  ,a.ID_HETHONG_YCHT
                                  ,a.ID_NODE_LUONG_HOTRO
                                  ,a.NGUOIHOTRO
                                  ,a.LOAIHANHDONG
                                  ,a.NOIDUNGXULY
                                  ,a.NOIDUNGXLCHITIET
                                  ,a.NGAYXULY
                                  ,a.TRANGTHAI
                                  ,a.ID_DONVI_FROM
                                  ,a.ID_DONVI_TO
                                  ,a.NGAYTIEPNHAN
                                  ,a.NGUOITAO
                                  ,a.LA_BUOC_HIENTAI
                                  ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                                  from HT_XULY_YEUCAU_HOTRO a
                                  INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                                  INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                                  WHERE a.ID={0} ", idchitietxlhotro);
                    var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }
            return ret;
        }

        /// <summary>
        /// Lấy thông tin yêu cầu hỗ trợ theo id xử lý hỗ trợ lần lượt
        /// </summary>
        /// <param name="idchitietxlhotro"></param>
        /// <returns></returns>
        [WebMethod]
        public string loadToanBoThongTinYeuCauHoTro(string idchitietxlhotro)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@" select a.ID,b.SODIENTHOAI
                                  ,a.ID_YEUCAU_HOTRO_HT
                                  ,a.ID_HETHONG_YCHT
                                  ,a.ID_NODE_LUONG_HOTRO
                                  ,a.NGUOIHOTRO
                                  ,a.LOAIHANHDONG
                                  ,a.NOIDUNGXULY
                                  ,a.NOIDUNGXLCHITIET
                                  ,a.NGAYXULY
                                  ,a.TRANGTHAI
                                  ,a.ID_DONVI_FROM
                                  ,a.ID_DONVI_TO
                                  ,a.NGAYTIEPNHAN
                                  ,a.NGUOITAO
                                  ,a.LA_BUOC_HIENTAI
                                  ,b.ID_LUONG_HOTRO
                                  ,b.NOIDUNG_YEUCAU   
                                  ,b.SODIENTHOAI
                                  ,c.TENHETHONG
                                  ,d.TEN_LUONG
                                  ,d.MOTA
                                  ,b.ID ID_YEU_CAU_HOTRO
                                  ,e.LINHVUC
                                  ,f.TENMUCDO
                            FROM HT_XULY_YEUCAU_HOTRO a 
                            INNER JOIN HT_DM_YEUCAU_HOTRO_HT b ON b.ID=a.ID_YEUCAU_HOTRO_HT
                            LEFT JOIN HT_DM_HETHONG_YCHT c ON c.ID=b.ID_HETHONG_YCHT
                            LEFT JOIN HT_CAYTHUMUC_YCHT e on e.ID=b.ID_CAYTHUMUC_YCHT
                            LEFT JOIN HT_LUONG_HOTRO d on d.id=b.ID_LUONG_HOTRO
                            LEFT JOIN HT_MUCDO_SUCO f on f.id=b.ID_MUCDO_SUCO
                            WHERE a.ID={0} ", idchitietxlhotro);
                    var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO7>(strSql);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }
            return ret;
        }

        /// <summary>
        /// Lấy thông tin yêu cầu hỗ trợ theo id yêu cầu
        /// </summary>
        /// <param name="idchitietxlhotro"></param>
        /// <returns></returns>
        [WebMethod]
        public string loadToanBoThongTinYeuCauHoTroTheoID(string idchitietxlhotro)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@" select a.ID,b.SODIENTHOAI
                                  ,a.ID_YEUCAU_HOTRO_HT
                                  ,a.ID_HETHONG_YCHT
                                  ,a.ID_NODE_LUONG_HOTRO
                                  ,a.NGUOIHOTRO
                                  ,a.LOAIHANHDONG
                                  ,a.NOIDUNGXULY
                                  ,a.NOIDUNGXLCHITIET
                                  ,a.NGAYXULY
                                  ,a.TRANGTHAI
                                  ,a.ID_DONVI_TO
                                  ,a.NGAYTIEPNHAN
                                  ,a.NGUOITAO
                                  ,a.LA_BUOC_HIENTAI
                                  ,b.ID_LUONG_HOTRO
                                  ,b.NOIDUNG_YEUCAU                                  
                                  ,c.TENHETHONG
                                  ,d.TEN_LUONG
                                  ,d.MOTA
                                  ,b.ID ID_YEU_CAU_HOTRO
                                  ,e.LINHVUC
                                  ,f.TENMUCDO
                            FROM HT_XULY_YEUCAU_HOTRO a 
                            INNER JOIN HT_DM_YEUCAU_HOTRO_HT b ON b.ID=a.ID_YEUCAU_HOTRO_HT
                            LEFT JOIN HT_DM_HETHONG_YCHT c ON c.ID=b.ID_HETHONG_YCHT
                            LEFT JOIN HT_CAYTHUMUC_YCHT e on e.ID=b.ID_CAYTHUMUC_YCHT
                            LEFT JOIN HT_LUONG_HOTRO d on d.id=b.ID_LUONG_HOTRO
                            LEFT JOIN HT_MUCDO_SUCO f on f.id=b.ID_MUCDO_SUCO
                            WHERE b.ID={0} ", idchitietxlhotro);
                    var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO7>(strSql);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }
            return ret;
        }


        /// <summary>
        /// Kiểm tra xem luồng hỗ trợ xử lý luôn hay chuyển tiếp
        /// nếu là chuyển tiếp thì bước hiện hành luôn - tổng bước
        /// còn nếu là xử lý luôn thì bước xử lý hiện hành = tổng bước
        /// khi ở bước hiện hành thì sẽ tự động chuyển về người yêu cầu phản hồi
        /// nếu người này nhận được và xác nhận vấn đề đã xử lý xong thì yêu cầu hỗ trợ
        /// đã hoàn thành
        /// </summary>
        /// <param name="id_ht_xuly_hotro"></param>
        /// <returns></returns>
        [WebMethod]
        public string checkLuongXuLyHT(string id_yeucau_xuly_hotro)
        {
            string ret = "";
            try
            {
                // cho phép bất kỳ nút nào cũng được chuyển phản hồi, trừ nút gốc ban đầu
                // nếu là nút cuối cùng thì không thể chuyển tiếp được và chỉ có thể xử lý (mặc định sẽ về gốc ban đầu)
                // 1: gốc; 2: giữa; 3: cuối

                var idhethong_yc_hotro = 0;
                var idluonghotro = 0;
                var iddonvi = 0;
                // lấy thông tin hỗ trợ xử lý
                var dsxlyc = new List<HT_XULY_YEUCAU_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    var strsqlxlyc = @"select a.ID
                      ,a.ID_YEUCAU_HOTRO_HT
                      ,a.ID_HETHONG_YCHT
                      ,a.ID_NODE_LUONG_HOTRO
                      ,a.NGUOIHOTRO
                      ,a.LOAIHANHDONG
                      ,a.NOIDUNGXULY
                      ,a.NOIDUNGXLCHITIET
                      ,a.NGAYXULY
                      ,a.TRANGTHAI
                      ,a.ID_DONVI_FROM
                      ,a.ID_DONVI_TO
                      ,a.NGAYTIEPNHAN
                      ,a.NGUOITAO
                      ,a.LA_BUOC_HIENTAI
                      ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                      from HT_XULY_YEUCAU_HOTRO a
                      INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                      INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                   where a.ID=" + id_yeucau_xuly_hotro;
                    var lstxl = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strsqlxlyc);
                    dsxlyc = lstxl.ToList();
                }
                if (dsxlyc.Any())
                {
                    idhethong_yc_hotro = dsxlyc[0].ID_HETHONG_YCHT.Value;
                    idluonghotro = dsxlyc[0].ID_LUONG_HOTRO.Value;
                    iddonvi = dsxlyc[0].ID_DONVI_TO.Value;

                    var buocxuly = dsxlyc[0].BUOCXULY.Value;
                    var tongsobuoc = dsxlyc[0].SOBUOC.Value;
                    if (buocxuly == 1)
                    {
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(1); // chuyển tiếp
                    }
                    else
                    {
                        if (buocxuly == tongsobuoc)
                        {
                            ret = Newtonsoft.Json.JsonConvert.SerializeObject(3); // xử lý luôn
                        }
                        else
                        {
                            ret = Newtonsoft.Json.JsonConvert.SerializeObject(2); // chuyển tiếp
                        }
                    }

                }

                //// lấy thông tin về node hỗ trợ của đơn vị
                //var lstNodeLll = new List<HT_NODE_LUONG_HOTRO>();
                //using (var ctx = new HTHTKTContext())
                //{
                //    var strGetNode = string.Format(@"SELECT a.ID
                //      ,a.ID_HETHONG_YCHT
                //      ,a.ID_LUONG_HOTRO
                //      ,a.ID_DONVI
                //      ,a.BUOCXULY
                //      ,a.NGAYTAO
                //      ,b.SOBUOC
                //      ,'' TENDONVI  from HT_NODE_LUONG_HOTRO a 
                //    inner JOIN HT_LUONG_HOTRO b ON b.ID=a.ID_LUONG_HOTRO
                //    where   
                //    a.ID_HETHONG_YCHT={0} AND a.ID_LUONG_HOTRO={1} and a.ID_DONVI={2}",
                //        idhethong_yc_hotro, idluonghotro, iddonvi);
                //    var lstNode = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strGetNode);
                //    lstNodeLll = lstNode.ToList();
                //}
                //if (lstNodeLll.Any())
                //{
                //    if (lstNodeLll[0].BUOCXULY == 1)
                //    {
                //        ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                //    }
                //    else
                //    {
                //        if (lstNodeLll[0].BUOCXULY == lstNodeLll[0].SOBUOC)
                //        {
                //            ret = Newtonsoft.Json.JsonConvert.SerializeObject(3);
                //        }
                //        else
                //        {
                //            ret = Newtonsoft.Json.JsonConvert.SerializeObject(2);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục mức độ sự cố");
            }

            return ret;
        }

        [WebMethod]
        public string checkLuongXuLyHTTheoYeuCauHT(string id_yeucau_xuly_hotro)
        {
            return "";
        }
        [WebMethod]
        public string loadDonViChuyenDen(string idhethong_yc_hotro, string idluonghotro, string iddonvi)
        {
            string ret = "";
            try
            {

                // xử lý lấy danh sách các đơn vị chuyển đến khi 1 user login vào xử lý 1 hỗ trợ
                // lấy bước xử lý của user trước
                var dsnode = new List<HT_NODE_LUONG_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    var strSql = string.Format(@" SELECT hnlh.ID
                                          ,hlh.ID_HETHONG_YCHT
                                          ,hnlh.ID_LUONG_HOTRO
                                          ,hnlh.ID_DONVI
                                          ,hnlh.BUOCXULY
                                          ,hnlh.NGAYTAO
                                          ,hlh.SOBUOC,dt.TENDONVI  
                                           FROM HT_NODE_LUONG_HOTRO hnlh
                                          INNER JOIN HT_LUONG_HOTRO hlh ON hlh.ID=hnlh.ID_LUONG_HOTRO
                                          INNER JOIN HT_XULY_YEUCAU_HOTRO hdyh ON hdyh.ID=hlh.ID_HETHONG_YCHT
                                          INNER JOIN HT_DONVI dt ON dt.Id = hnlh.ID_DONVI
                                          WHERE hdyh.ID={0}
                                          AND hnlh.ID_LUONG_HOTRO={1}
                                          AND hnlh.ID_DONVI={2} ", idhethong_yc_hotro, idluonghotro, iddonvi);
                    var lst = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strSql);
                    dsnode = lst.ToList();
                }
                // danh sách các đơn vị chuyển tiếp của user hiện hành
                if (dsnode.Any())
                {
                    int buocxulyhienhanh = dsnode[0].BUOCXULY.Value;
                    int sobuocxuly = dsnode[0].SOBUOC.Value;

                    // lấy thông tin bước xử lý tiếp theo
                    // kiểm tra xem đã là bước cuối chưa
                    if (sobuocxuly >= buocxulyhienhanh + 1)
                    {
                        var strSqlNodeNext = string.Format(@"SELECT hnlh.ID
                                          ,hlh.ID_HETHONG_YCHT
                                          ,hnlh.ID_LUONG_HOTRO
                                          ,hnlh.ID_DONVI
                                          ,hnlh.BUOCXULY
                                          ,hnlh.NGAYTAO,hlh.SOBUOC,dt.TENDONVI  
                                           FROM HT_NODE_LUONG_HOTRO hnlh
                                           INNER JOIN HT_LUONG_HOTRO hlh ON hlh.ID=hnlh.ID_LUONG_HOTRO
                                           INNER JOIN HT_DM_HETHONG_YCHT hdyh ON hdyh.ID=hlh.ID_HETHONG_YCHT
                                           INNER JOIN HT_DONVI dt ON dt.Id = hnlh.ID_DONVI
                                           WHERE hdyh.ID={0}
                                           AND hnlh.ID_LUONG_HOTRO={1}
                                           AND hnlh.BUOCXULY={2} ",
                                               idhethong_yc_hotro,
                                               idluonghotro,
                                               buocxulyhienhanh + 1);
                        var lstDSDonViXuLyTiep = new List<HT_NODE_LUONG_HOTRO>();
                        using (var ctx = new ADDJContext())
                        {
                            lstDSDonViXuLyTiep = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strSqlNodeNext).ToList();
                        }
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstDSDonViXuLyTiep);
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }

            return ret;
        }

        [WebMethod]
        public string loadDonViChuyenDenTheoIdXuLyHoTro(string id_xuly_yc_hotro)
        {
            string ret = "";
            try
            {
                var idhethong_yc_hotro = 0;
                var idluonghotro = 0;
                var iddonvi = 0;
                // lấy thông tin hỗ trợ xử lý
                var dsxlyc = new List<HT_XULY_YEUCAU_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    var strsqlxlyc = @"select a.ID
                      ,a.ID_YEUCAU_HOTRO_HT
                      ,a.ID_HETHONG_YCHT
                      ,a.ID_NODE_LUONG_HOTRO
                      ,a.NGUOIHOTRO
                      ,a.LOAIHANHDONG
                      ,a.NOIDUNGXULY
                      ,a.NOIDUNGXLCHITIET
                      ,a.NGAYXULY
                      ,a.TRANGTHAI
                      ,a.ID_DONVI_FROM
                      ,a.ID_DONVI_TO
                      ,a.NGAYTIEPNHAN
                      ,a.NGUOITAO
                      ,a.LA_BUOC_HIENTAI
                      ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                      from HT_XULY_YEUCAU_HOTRO a
                      INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                      INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                      where a.id=" + id_xuly_yc_hotro;
                    var lstxl = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strsqlxlyc);
                    dsxlyc = lstxl.ToList();
                }
                if (dsxlyc.Any())
                {
                    idhethong_yc_hotro = dsxlyc[0].ID_HETHONG_YCHT.Value;
                    idluonghotro = dsxlyc[0].ID_LUONG_HOTRO.Value;
                    iddonvi = dsxlyc[0].ID_DONVI_TO.Value;
                    var buochientai = dsxlyc[0].BUOCXULY.Value;
                    var tongsobuoc = dsxlyc[0].SOBUOC.Value;


                    // danh sách các đơn vị bước tiếp xử lý
                    var lstDSDonViXuLyTiep = new List<HT_NODE_LUONG_HOTRO>();
                    string strgetlst = string.Format(@"SELECT a.ID
                      ,a.ID_HETHONG_YCHT
                      ,a.ID_LUONG_HOTRO
                      ,a.ID_DONVI
                      ,a.BUOCXULY
                      ,a.NGAYTAO
                      ,b.SOBUOC
                      ,dt.TENDONVI  from HT_NODE_LUONG_HOTRO a 
                    inner JOIN HT_LUONG_HOTRO b ON b.ID=a.ID_LUONG_HOTRO
                    INNER JOIN HT_DONVI dt ON a.ID_DONVI=dt.Id where   
                    a.ID_HETHONG_YCHT={0} AND a.ID_LUONG_HOTRO={1} and a.BUOCXULY={2}",
                    idhethong_yc_hotro, idluonghotro, buochientai < tongsobuoc ? buochientai + 1 : tongsobuoc);
                    using (var ctx = new ADDJContext())
                    {
                        lstDSDonViXuLyTiep = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strgetlst).ToList();
                    }
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstDSDonViXuLyTiep);
                }

                //// lấy thông tin về node hỗ trợ của đơn vị
                //var lstNodeLll = new List<HT_NODE_LUONG_HOTRO>();
                //using (var ctx = new HTHTKTContext())
                //{
                //    var strGetNode = string.Format(@"SELECT a.ID
                //      ,a.ID_HETHONG_YCHT
                //      ,a.ID_LUONG_HOTRO
                //      ,a.ID_DONVI
                //      ,a.BUOCXULY
                //      ,a.NGAYTAO
                //      ,b.SOBUOC
                //      ,'' TENDONVI  from HT_NODE_LUONG_HOTRO a 
                //      inner JOIN HT_LUONG_HOTRO b ON b.ID=a.ID_LUONG_HOTRO
                //      where   
                //      a.ID_HETHONG_YCHT={0} AND a.ID_LUONG_HOTRO={1} and a.ID_DONVI={2}",
                //    idhethong_yc_hotro, idluonghotro, iddonvi);
                //    var lstNode = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strGetNode);
                //    lstNodeLll = lstNode.ToList();
                //}
                //if (lstNodeLll.Any())
                //{
                //    var buoc_hienhanh = lstNodeLll[0].BUOCXULY;
                //    var tongsobuoc = lstNodeLll[0].SOBUOC;
                //    // danh sách các đơn vị bước tiếp xử lý
                //    var lstDSDonViXuLyTiep = new List<HT_NODE_LUONG_HOTRO>();
                //    string strgetlst = string.Format(@"SELECT a.ID
                //      ,a.ID_HETHONG_YCHT
                //      ,a.ID_LUONG_HOTRO
                //      ,a.ID_DONVI
                //      ,a.BUOCXULY
                //      ,a.NGAYTAO
                //      ,b.SOBUOC
                //      ,dt.TENDONVI  from HT_NODE_LUONG_HOTRO a 
                //    inner JOIN HT_LUONG_HOTRO b ON b.ID=a.ID_LUONG_HOTRO
                //    INNER JOIN HT_DONVI dt ON a.ID_DONVI=dt.Id where   
                //    a.ID_HETHONG_YCHT={0} AND a.ID_LUONG_HOTRO={1} and a.BUOCXULY={2}",
                //    idhethong_yc_hotro, idluonghotro, buoc_hienhanh < tongsobuoc ? buoc_hienhanh + 1 : tongsobuoc);
                //    using (var ctx = new HTHTKTContext())
                //    {
                //        lstDSDonViXuLyTiep = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strgetlst).ToList();
                //    }
                //    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstDSDonViXuLyTiep);
                //}
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }

            return ret;
        }

        [WebMethod]
        public string loadDonViChuyenDenTiepTheoByIDYeuCauHoTro(string id_yeucau_hotro)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    // 1: lấy thông tin về yêu cầu hỗ trợ
                    var strSql = @"SELECT * FROM dbo.HT_DM_YEUCAU_HOTRO_HT  WHERE ID=" + id_yeucau_hotro;
                    var res = ctx.Database.SqlQuery<HT_DM_YEUCAU_HOTRO_HT>(strSql);
                    var infoyc = res.FirstOrDefault();
                    // 2: lấy thông tin về xử lý hỗ trợ hiện tại
                    var strsqlhientai = string.Format(@"SELECT ID
                                                  ,ID_YEUCAU_HOTRO_HT
                                                  ,ID_HETHONG_YCHT
                                                  ,ID_NODE_LUONG_HOTRO
                                                  ,NGUOIHOTRO
                                                  ,LOAIHANHDONG
                                                  ,NOIDUNGXULY
                                                  ,NGAYXULY
                                                  ,TRANGTHAI
                                                  ,ID_DONVI_FROM
                                                  ,ID_DONVI_TO
                                                  ,NGAYTIEPNHAN
                                                  ,NGUOITAO
                                                  ,LA_BUOC_HIENTAI
                                                  ,NOIDUNGXLCHITIET, 0 BUOCXULY,0 SOBUOC 
                                            FROM dbo.HT_XULY_YEUCAU_HOTRO 
                                            WHERE ID_YEUCAU_HOTRO_HT={0} AND LA_BUOC_HIENTAI=1",
                                                id_yeucau_hotro);
                    var hotroht = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strsqlhientai);
                    var infohotroht = hotroht.FirstOrDefault();
                    // 3: lấy bước xử lý hiện tại
                    var buocxlht = "SELECT * FROM dbo.HT_NODE_LUONG_HOTRO WHERE ID=" + infohotroht.ID_NODE_LUONG_HOTRO;
                    var strsqlbuocht = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO2>(buocxlht);
                    var infolstbuocht = strsqlbuocht.FirstOrDefault();
                    // 4: lấy max bước xử lý
                    var maxbuocxl = "SELECT * FROM dbo.HT_LUONG_HOTRO WHERE id=" + infolstbuocht.ID_LUONG_HOTRO;
                    var infomaxbuocxl = ctx.Database.SqlQuery<HT_LUONG_HOTRO>(maxbuocxl);
                    var inmaxbuocsl = infomaxbuocxl.FirstOrDefault();
                    // 5: lấy bước tiếp theo
                    //var strbuocxltieptheo = "";
                    //if (inmaxbuocsl.SOBUOC > infolstbuocht.BUOCXULY)
                    //{
                    //    strbuocxltieptheo = string.Format("SELECT * FROM dbo.HT_NODE_LUONG_HOTRO WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND BUOCXULY={2}", infolstbuocht.ID_HETHONG_YCHT, infolstbuocht.ID_LUONG_HOTRO, infolstbuocht.BUOCXULY + 1);
                    //}
                    //else
                    //{
                    //    strbuocxltieptheo = string.Format("SELECT * FROM dbo.HT_NODE_LUONG_HOTRO WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND BUOCXULY={2}", infolstbuocht.ID_HETHONG_YCHT, infolstbuocht.ID_LUONG_HOTRO, inmaxbuocsl.SOBUOC);
                    //}

                    // danh sách các đơn vị bước tiếp xử lý
                    var lstDSDonViXuLyTiep = new List<HT_NODE_LUONG_HOTRO>();
                    string strgetlst = string.Format(@"SELECT a.ID
                      ,a.ID_HETHONG_YCHT
                      ,a.ID_LUONG_HOTRO
                      ,a.ID_DONVI
                      ,a.BUOCXULY
                      ,a.NGAYTAO
                      ,b.SOBUOC
                      ,dt.TENDONVI from HT_NODE_LUONG_HOTRO a 
                inner JOIN HT_LUONG_HOTRO b ON b.ID=a.ID_LUONG_HOTRO
                INNER JOIN HT_DONVI dt ON a.ID_DONVI=dt.Id where   
                a.ID_HETHONG_YCHT={0} AND a.ID_LUONG_HOTRO={1} and a.BUOCXULY={2}",
                    infolstbuocht.ID_HETHONG_YCHT, infolstbuocht.ID_LUONG_HOTRO, infolstbuocht.BUOCXULY < inmaxbuocsl.SOBUOC ? infolstbuocht.BUOCXULY + 1 : inmaxbuocsl.SOBUOC);

                    lstDSDonViXuLyTiep = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strgetlst).ToList();

                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstDSDonViXuLyTiep);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }
            return ret;
        }

        public string loadDonViChuyenDenTheoIdYeuCauXuLyHoTro(string id_yeucau_xuly_hotro)
        {
            string ret = "";
            try
            {
                var idhethong_yc_hotro = 0;
                var idluonghotro = 0;
                var iddonvi = 0;
                // lấy thông tin hỗ trợ
                var dsxlyc = new List<HT_XULY_YEUCAU_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    var strsqlxlyc = @"SELECT ID
                                  ,ID_YEUCAU_HOTRO_HT
                                  ,ID_HETHONG_YCHT
                                  ,ID_NODE_LUONG_HOTRO
                                  ,NGUOIHOTRO
                                  ,LOAIHANHDONG
                                  ,NOIDUNGXULY
                                  ,NGAYXULY
                                  ,TRANGTHAI
                                  ,ID_DONVI_FROM
                                  ,ID_DONVI_TO
                                  ,NGAYTIEPNHAN
                                  ,NGUOITAO
                                  ,LA_BUOC_HIENTAI
                                  ,NOIDUNGXLCHITIET, 0 BUOCXULY,0 SOBUOC 
                                  FROM dbo.HT_XULY_YEUCAU_HOTRO 
                                  where id=" + id_yeucau_xuly_hotro;
                    var lstxl = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strsqlxlyc);
                    dsxlyc = lstxl.ToList();
                }
                if (dsxlyc.Any())
                {
                    idhethong_yc_hotro = dsxlyc[0].ID_HETHONG_YCHT.Value;
                    idluonghotro = dsxlyc[0].ID_LUONG_HOTRO.Value;
                    iddonvi = dsxlyc[0].ID_DONVI_TO.Value;
                }

                // xử lý lấy danh sách các đơn vị chuyển đến khi 1 user login vào xử lý 1 hỗ trợ
                // lấy bước xử lý của user trước
                var dsnode = new List<HT_NODE_LUONG_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    var strSql = string.Format(@" SELECT hnlh.ID
                                          ,hlh.ID_HETHONG_YCHT
                                          ,hnlh.ID_LUONG_HOTRO
                                          ,hnlh.ID_DONVI
                                          ,hnlh.BUOCXULY
                                          ,hnlh.NGAYTAO
                                          ,hlh.SOBUOC,dt.TENDONVI  
                                           FROM HT_NODE_LUONG_HOTRO hnlh
                                          INNER JOIN HT_LUONG_HOTRO hlh ON hlh.ID=hnlh.ID_LUONG_HOTRO
                                          INNER JOIN HT_DM_YEUCAU_HOTRO_HT hdyh ON hdyh.ID=hlh.ID_HETHONG_YCHT
                                          INNER JOIN HT_DONVI dt ON dt.Id = hnlh.ID_DONVI
                                          WHERE hdyh.ID={0}
                                          AND hnlh.ID_LUONG_HOTRO={1}
                                          AND hnlh.ID_DONVI={2} ", idhethong_yc_hotro, idluonghotro, iddonvi);
                    var lst = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strSql);
                    dsnode = lst.ToList();
                }
                // danh sách các đơn vị chuyển tiếp của user hiện hành
                if (dsnode.Any())
                {
                    int buocxulyhienhanh = dsnode[0].BUOCXULY.Value;
                    int sobuocxuly = dsnode[0].SOBUOC.Value;

                    // lấy thông tin bước xử lý tiếp theo
                    // kiểm tra xem đã là bước cuối chưa
                    if (sobuocxuly >= buocxulyhienhanh + 1)
                    {
                        var strSqlNodeNext = string.Format(@"SELECT hnlh.ID
                                          ,hlh.ID_HETHONG_YCHT
                                          ,hnlh.ID_LUONG_HOTRO
                                          ,hnlh.ID_DONVI
                                          ,hnlh.BUOCXULY
                                          ,hnlh.NGAYTAO
                                          ,hlh.SOBUOC
                                          ,dt.TENDONVI  
                                           FROM HT_NODE_LUONG_HOTRO hnlh
                                           INNER JOIN HT_LUONG_HOTRO hlh ON hlh.ID=hnlh.ID_LUONG_HOTRO
                                           INNER JOIN HT_DM_HETHONG_YCHT hdyh ON hdyh.ID=hlh.ID_HETHONG_YCHT
                                           INNER JOIN HT_DONVI dt ON dt.Id = hnlh.ID_DONVI
                                           WHERE hdyh.ID={0}
                                           AND hnlh.ID_LUONG_HOTRO={1}
                                           AND hnlh.BUOCXULY={2} ",
                                               idhethong_yc_hotro,
                                               idluonghotro,
                                               buocxulyhienhanh + 1);
                        var lstDSDonViXuLyTiep = new List<HT_NODE_LUONG_HOTRO>();
                        using (var ctx = new ADDJContext())
                        {
                            lstDSDonViXuLyTiep = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strSqlNodeNext).ToList();
                        }
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstDSDonViXuLyTiep);
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }

            return ret;
        }

        /// <summary>
        /// Xử lý khi tạo mới yêu cầu hỗ trợ thì bước xử lý mặc định sẽ là 1
        /// (BUOCXULY=1)
        /// </summary>
        /// <param name="id_ht_xuly_hotro"></param>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        [WebMethod]
        public string layThongTinIDluongHoTro(string id_ht_xuly_hotro, string iddonvi)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSql = string.Format(@" SELECT hnlh.ID
                                      ,hlh.ID_HETHONG_YCHT
                                      ,hnlh.ID_LUONG_HOTRO
                                      ,hnlh.ID_DONVI
                                      ,hnlh.BUOCXULY
                                      ,hnlh.NGAYTAO
                                      ,hlh.SOBUOC
                                      ,dt.TENDONVI 
                                      FROM HT_NODE_LUONG_HOTRO hnlh
                                      INNER JOIN HT_LUONG_HOTRO hlh ON hlh.ID=hnlh.ID_LUONG_HOTRO                                 
                                      INNER JOIN HT_DONVI dt ON dt.Id = hnlh.ID_DONVI
                                      WHERE hlh.ID_HETHONG_YCHT={0}
                                      AND hnlh.ID_DONVI={1} AND hnlh.BUOCXULY=2", id_ht_xuly_hotro, iddonvi);
                    var lst = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strSql);
                    var lstPhongBanHT = lst.FirstOrDefault();
                    if (lstPhongBanHT != null)
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstPhongBanHT.ID_LUONG_HOTRO);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }

            return ret;
        }

        [WebMethod]
        public string taoMoiLuongHoTroXL(string idhethong, string idluonghotro, string idloaihotro, string noidunghotro, string noidunghotrochitiet, string iddonvi_from, string iddonvi_to, string nguoitao, string id_nguoitao, string sodienthoai, string id_mucdo_suco)
        {
            var strrt = "";
            var idHeThong = idhethong;
            var idLoaiHoTro = idloaihotro;
            var txtNoiDungHoTro = noidunghotro;
            var txtNoiDungHoTroChiTiet = noidunghotrochitiet;
            var iddonvi_TO = iddonvi_to;

            var id_yeucau_hotro = "";
            var id_xuly_yeucau_hotro = "";

            // Xử lý lấy thông tin Node_Luong_Ho_Tro của đơn vị tạo
            // Lấy Node trong bảng: HT_NODE_LUONG_HO_TRO theo iddonvi_TO
            var idNodLuongHT = 0;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    //var strSql = "select id from HT_NODE_LUONG_HOTRO where id_donvi=" + iddonvi_TO;
                    var strSql = string.Format(@"SELECT hnlh.ID
                                                 FROM HT_NODE_LUONG_HOTRO hnlh 
                                                 WHERE hnlh.ID_HETHONG_YCHT={0}
                                                 AND hnlh.ID_LUONG_HOTRO={1} AND hnlh.ID_DONVI={2}",
                                                 idhethong, idluonghotro, iddonvi_from);
                    var lstId = ctx.Database.SqlQuery<int>(strSql);
                    var ls = lstId.FirstOrDefault();
                    idNodLuongHT = Convert.ToInt32(ls);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "100001: chưa cấu hình luồng");
                return "100001"; // lỗi chưa cấu hình node cho luồng hỗ trợ
            }

            decimal id_yeucau_HoTro = 0;
            decimal id_xuly_yeucau_hotro_hethong = 0;
            using (var ctx = new ADDJContext())
            {
                using (var trans = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        // xử lý tạo bộ mã tìm kiếm: 	CCBS.GS2.29.06.17.000001
                        // Ten_ht (gọi là 1 mã ID),ten_loai_yeucau,ngay_thang_nam 
                        // 1. lấy mã hệ thống
                        var strSqlGetMaHT = string.Format(@"select MA_HETHONG from HT_DM_HETHONG_YCHT where ID={0}", idhethong);
                        var ma_01 = ctx.Database.SqlQuery<string>(strSqlGetMaHT).FirstOrDefault();
                        // 2. lấy mã loại lĩnh vực
                        var strSqlGetMaLV = string.Format(@"select MA_LINHVUC from HT_CAYTHUMUC_YCHT where ID={0}", idloaihotro);
                        var ma_02 = ctx.Database.SqlQuery<string>(strSqlGetMaLV).FirstOrDefault();
                        // 3. ngày_tháng_năm
                        var ma_03 = DateTime.Now.ToString("dd.MM.yy");
                        // 4. lấy ID thứ tự trong ngày
                        var strSqlGetID = string.Format(@"insert into HT_CODE(NAME) values(1); SELECT SCOPE_IDENTITY();");
                        var ma_04_temp = ctx.Database.SqlQuery<decimal>(strSqlGetID).FirstOrDefault();
                        var ma_04 = String.Format("{0:000000}", ma_04_temp);
                        // Bộ mã cuối cùng:
                        var ma_final = ma_01 + "." + ma_02 + "." + ma_03 + "." + ma_04;


                        // lưu thông tin hỗ trợ: HT_DM_YEUCAU_HOTRO_HT
                        // trang thái tạo ban đầu khi bắt đầu tạo là đang xử lý
                        // 0: khởi tạo, 1: Chuyển tiếp; 2: Chuyển phản hồi, 3: Chờ xác nhận, 4: Đã xử lý xong
                        var strSQLChiTietHT = string.Format(@"INSERT INTO dbo.HT_DM_YEUCAU_HOTRO_HT
                                (
                                   ID_HETHONG_YCHT
                                 , ID_CAYTHUMUC_YCHT
                                 , ID_MUCDO_SUCO
                                 , ID_LUONG_HOTRO
                                 , ID_DONVI
                                 , MA_YEUCAU
                                 , NOIDUNG_YEUCAU   
                                 , SODIENTHOAI
                                 , TRANGTHAI
                                 , NGUOITAO
                                 , ID_NGUOITAO
                                 , NGAYTAO
                                )
                                VALUES
                                (
                                  {0} 
                                 ,{1}
                                 ,{2}
                                 ,{3}
                                 ,{4}
                                 ,N'{5}' 
                                 ,N'{6}'
                                 ,'{7}'
                                 ,0
                                 ,'{8}'
                                 ,'{9}'
                                 ,getdate()
                                ); SELECT SCOPE_IDENTITY();",
                               idHeThong,
                               idLoaiHoTro, 
                               id_mucdo_suco,
                               idluonghotro,
                               iddonvi_from,
                               ma_final,
                               txtNoiDungHoTro,
                               sodienthoai,
                               nguoitao,
                               id_nguoitao);
                        var rest = ctx.Database.SqlQuery<decimal>(strSQLChiTietHT).ToList();
                        id_yeucau_HoTro = rest.FirstOrDefault();

                        // lưu thông tin khởi tạo luồng đầu tiên xử lý: HT_CHITIET_XULY_HOTRO
                        // đây là bước khởi tạo thì LOAI_HANH_DONG: 1: Chuyển tiếp
                        // (0: khởi tạo; 1: Chuyển tiếp, 2: Chuyển phản hồi, 3: Chờ xác nhận của người yêu cầu hỗ trợ)
                        var strSQLKhoiTaoChuyenXL = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                (
                                  ID_YEUCAU_HOTRO_HT
                                 , ID_HETHONG_YCHT
                                 , ID_NODE_LUONG_HOTRO
                                 , NGUOIHOTRO
                                 , LOAIHANHDONG
                                 , NOIDUNGXULY
                                 , NOIDUNGXLCHITIET
                                 , NGAYXULY
                                 , TRANGTHAI
                                 , ID_DONVI_FROM
                                 , ID_DONVI_TO
                                 , NGAYTIEPNHAN
                                 , NGUOITAO
                                 , ID_NGUOITAO
                                 , LA_BUOC_HIENTAI
                                )
                                VALUES
                                (
                                   {0}-- ID_YEUCAU_HOTRO_HT - int
                                 , {1}-- ID_HETHONG_YCHT - int
                                 , {2}-- ID_NODE_LUONG_HO_TRO - int
                                 , N'{3}'-- NGUOI_HO_TRO - nvarchar(50)
                                 , {4}-- LOAI_HANH_DONG - int
                                 , N'{5}'-- NOI_DUNG_XU_LY - nvarchar(500)
                                 , N'{6}'
                                 , GETDATE()-- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAY_XU_LY - datetime
                                 , {7}
                                 , {8}
                                 , {9}
                                 , GETDATE()
                                 , N'{10}'
                                 , N'{11}'
                                 , 0
                                )",
                                    id_yeucau_HoTro,
                                    idHeThong,
                                    idNodLuongHT,
                                    "",
                                    0, // 0: khởi tạo yêu cầu hỗ trợ ban đầu
                                    "Khởi tạo hỗ trợ",
                                    "",// chưa có nội dung chi tiết
                                    0,
                                    iddonvi_from,
                                    iddonvi_from, // khởi tạo thì from=>to là 1 từ from
                                    nguoitao,
                                    id_nguoitao);
                        ctx.Database.ExecuteSqlCommand(strSQLKhoiTaoChuyenXL);

                        // lưu thông tin chuyển đến
                        // bước 1: lấy thông tin về node luồng hỗ trợ tiếp theo
                        string getNodeNext = string.Format(@"SELECT hnlh.ID
                          FROM HT_NODE_LUONG_HOTRO hnlh 
                          WHERE hnlh.ID_HETHONG_YCHT={0}
                          AND hnlh.ID_LUONG_HOTRO={1} AND hnlh.ID_DONVI={2} ",
                          idHeThong, idluonghotro, iddonvi_TO);
                        var lstNodeNext = ctx.Database.SqlQuery<int>(getNodeNext);
                        var lstNode = lstNodeNext.FirstOrDefault();

                        // bước 2: lưu thoog tin về node tiếp

                        var strSQLChuyenXL = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                (
                                   ID_YEUCAU_HOTRO_HT
                                 , ID_HETHONG_YCHT
                                 , ID_NODE_LUONG_HOTRO
                                 , NGUOIHOTRO
                                 , LOAIHANHDONG
                                 , NOIDUNGXULY
                                 , NOIDUNGXLCHITIET
                                 , NGAYXULY
                                 , TRANGTHAI
                                 , ID_DONVI_FROM
                                 , ID_DONVI_TO
                                 , NGAYTIEPNHAN
                                 , NGUOITAO
                                 , ID_NGUOITAO
                                 , LA_BUOC_HIENTAI
                                )
                                VALUES
                                (
                                   {0}-- ID_YEUCAU_HOTRO_HT - int
                                 , {1}-- ID_HETHONG_YCHT - int
                                 , {2}-- ID_NODE_LUONG_HOTRO - int
                                 , N'{3}'-- NGUOI_HO_TRO - nvarchar(50)
                                 , {4}-- LOAI_HANH_DONG - int
                                 , N'{5}'-- NOI_DUNG_XU_LY - nvarchar(500)
                                 , N'{6}'-- NOIDUNGXLCHITIET - nvarchar(500)
                                 , GETDATE()-- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAY_XU_LY - datetime
                                 , {7}
                                 , {8}
                                 , {9}
                                 , GETDATE()
                                 , N'{10}'
                                 , N'{11}'
                                 , 1
                                );  SELECT SCOPE_IDENTITY();",
                                 id_yeucau_HoTro,
                                 idHeThong,
                                 Convert.ToInt32(lstNode),
                                 "",
                                 1, // 1: chuyển tiếp luôn
                                 txtNoiDungHoTro,
                                 txtNoiDungHoTroChiTiet,
                                 1, iddonvi_from,
                                 iddonvi_to,
                                 nguoitao,
                                 id_nguoitao);
                        //ctx.Database.ExecuteSqlCommand(strSQLChuyenXL);
                        var restchitiet = ctx.Database.SqlQuery<decimal>(strSQLChuyenXL).ToList();
                        id_xuly_yeucau_hotro_hethong = restchitiet.FirstOrDefault();

                        // chuyển vào chờ gửi email/sms: 
                        // 1. xóa thông tin cũ mail/sms
                        //string delMailInfo = string.Format(@"delete HT_TO_SEND_EMAIL where id_yeucau_hotro_ht={0}", id_yeucau_HoTro);
                        //string delSmsInfo = string.Format(@"delete HT_TO_SEND_SMS where id_yeucau_hotro_ht={0}", id_yeucau_HoTro);
                        //ctx.Database.ExecuteSqlCommand(delMailInfo);
                        //ctx.Database.ExecuteSqlCommand(delSmsInfo);
                        // 2. chèn thông tin mới mail/sms
                        string addMailInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_EMAIL
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                             )", id_yeucau_HoTro
                                                             , id_xuly_yeucau_hotro_hethong
                                                             , txtNoiDungHoTro
                                                             , iddonvi_from
                                                             , iddonvi_to
                                                             , txtNoiDungHoTroChiTiet);
                        string addSmsInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_SMS
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                            )", id_yeucau_HoTro
                                                             , id_xuly_yeucau_hotro_hethong
                                                             , txtNoiDungHoTro
                                                             , iddonvi_from
                                                             , iddonvi_to
                                                             , txtNoiDungHoTroChiTiet);
                        ctx.Database.ExecuteSqlCommand(addMailInfo);
                        ctx.Database.ExecuteSqlCommand(addSmsInfo);

                        trans.Commit();
                        strrt = "1";
                    }
                    catch (Exception ex)
                    {
                        Actions.ActionProcess.GhiLog(ex, "Lỗi Tạo mới yêu cầu hỗ trợ: " + ex.Message);
                        trans.Rollback();
                        return "100002" + "|" + ex.Message;
                    }
                }
            }
            return strrt + "|" + id_yeucau_HoTro + "|" + id_xuly_yeucau_hotro_hethong;
        }

        /// <summary>
        /// xử lý luôn hỗ trợ sẽ phản hồi về đơn vị ban đầu
        /// </summary>
        /// <param name="id_yeucau_xuly_hotro"></param>
        /// <param name="noidungxuly"></param>
        /// <param name="noidungxulychitiet"></param>
        /// <param name="nguoixuly"></param>
        /// <returns></returns>
        [WebMethod]
        public string xuLyLuonHoTro(string id_yeucau_xuly_hotro, string noidungxuly, string noidungxulychitiet, string iddonvi_from, string iddonvi_to, string nguoixuly, string id_nguoixuly)
        {
            string ret = "";
            decimal id_hethong_HoTro = 0;
            decimal id_yeucau_HoTro = 0;
            decimal id_xuly_yeucau_hotro_hethong = 0;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    using (var trans = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1 lấy thông tin xử lý hỗ trợ
                            string strSql = @" select a.ID
                                              ,a.ID_YEUCAU_HOTRO_HT
                                              ,a.ID_HETHONG_YCHT
                                              ,a.ID_NODE_LUONG_HOTRO
                                              ,a.NGUOIHOTRO
                                              ,a.LOAIHANHDONG
                                              ,a.NOIDUNGXULY
                                              ,a.NOIDUNGXLCHITIET
                                              ,a.NGAYXULY
                                              ,a.TRANGTHAI
                                              ,a.ID_DONVI_FROM
                                              ,a.ID_DONVI_TO
                                              ,a.NGAYTIEPNHAN
                                              ,a.NGUOITAO
                                              ,a.LA_BUOC_HIENTAI
                                              ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                                              from HT_XULY_YEUCAU_HOTRO a
                                              INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                                              INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                                          where a.id=" + id_yeucau_xuly_hotro;
                            var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                            var lst = rt.ToList();
                            var tthotro = lst.FirstOrDefault();

                            // 2 lấy thông tin về node tiếp theo
                            var id_yeucau_hotro_ht = tthotro.ID_YEUCAU_HOTRO_HT;
                            var id_hethong_ycht = tthotro.ID_HETHONG_YCHT;
                            var id_luong_hotro = tthotro.ID_LUONG_HOTRO;

                            id_hethong_HoTro = tthotro.ID_HETHONG_YCHT.Value;
                            id_yeucau_HoTro = tthotro.ID_YEUCAU_HOTRO_HT.Value;

                            // 2 update lại trạng thái yêu cầu hỗ trợ xử lý
                            //  1: Chuyển tiếp; 2: Chuyển phản hồi, 3: Chờ xác nhận, 4: Đã xử lý xong
                            var strSqlupdatedanhmuc = string.Format(@"UPDATE HT_DM_YEUCAU_HOTRO_HT 
                                                                    SET TRANGTHAI=3 
                                                                    WHERE ID={0}", id_yeucau_hotro_ht);
                            ctx.Database.ExecuteSqlCommand(strSqlupdatedanhmuc);


                            // 2.1 lấy thông tin về hỗ trợ ban đầu
                            var strThongtinhotro = string.Format(@"SELECT ID
                                                              ,ID_HETHONG_YCHT
                                                              ,ID_CAYTHUMUC_YCHT
                                                              ,ID_MUCDO_SUCO
                                                              ,ID_LUONG_HOTRO
                                                              ,ID_DONVI
                                                              ,MA_YEUCAU
                                                              ,NOIDUNG_YEUCAU
                                                              ,NGAYTAO
                                                              ,NGUOITAO
                                                              ,TRANGTHAI
                                                              ,NOIDUNG_XL_DONG_HOTRO
                                                              ,SODIENTHOAI
                                                              FROM HT_DM_YEUCAU_HOTRO_HT 
                                                              where id={0} ", id_yeucau_hotro_ht);
                            var lstdmycht = new List<HT_DM_YEUCAU_HOTRO_HT>();
                            var dm = ctx.Database.SqlQuery<HT_DM_YEUCAU_HOTRO_HT>(strThongtinhotro);
                            lstdmycht = dm.ToList();
                            // đơn vị yêu cầu hỗ trợ ban đầu
                            var id_donvi_yeucau = lstdmycht[0].ID_DONVI;

                            // lấy thông tin node xử lý theo đơn vị ban đầu
                            var lstthongtindvbd = string.Format(@"SELECT ID
                                  ,ID_HETHONG_YCHT
                                  ,ID_LUONG_HOTRO
                                  ,ID_DONVI
                                  ,BUOCXULY
                                  ,NGAYTAO
                                  ,0 SOBUOC
                                  ,'' TENDONVI
                                  FROM HT_NODE_LUONG_HOTRO 
                                  WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} AND ID_DONVI={2}",
                                      id_hethong_ycht, id_luong_hotro, id_donvi_yeucau);
                            var nodebandau = new List<HT_NODE_LUONG_HOTRO>();
                            var nobd = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(lstthongtindvbd);
                            nodebandau = nobd.ToList();

                            //var lstnodetiep = new List<HT_XULY_YEUCAU_HOTRO>();
                            //var strno = string.Format(@"SELECT a.ID
                            //                      ,a.ID_YEUCAU_HOTRO_HT
                            //                      ,a.ID_HETHONG_YCHT
                            //                      ,a.ID_NODE_LUONG_HOTRO
                            //                      ,a.NGUOIHOTRO
                            //                      ,a.LOAIHANHDONG
                            //                      ,a.NOIDUNGXULY
                            //                      ,a.NGAYXULY
                            //                      ,a.TRANGTHAI
                            //                      ,a.ID_DONVI
                            //                      ,a.NGAYTIEPNHAN
                            //                      ,a.NGUOITAO 
                            //                      ,a.LA_BUOC_HIENTAI
                            //                      ,b.ID_LUONG_HOTRO
                            //                  from HT_XULY_YEUCAU_HOTRO a
                            //                INNER JOIN HT_NODE_LUONG_HOTRO b on a.ID_NODE_LUONG_HOTRO=b.ID
                            //                where a.id_yeucau_hotro_ht={0} 
                            //                and a.id_hethong_ycht={1} and b.BUOCXULY=1", id_yeucau_hotro_ht, id_hethong_ycht);
                            //var lstqr = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strno);
                            //lstnodetiep = lstqr.ToList();
                            if (nodebandau.Any())
                            {
                                // 2 xử lý luồng
                                // update loai_hanh_dong ve 0, chỉ update là bước hiện tại về 0 để cho bước tiếp là 1
                                string strSqlupdate = " update HT_XULY_YEUCAU_HOTRO set LA_BUOC_HIENTAI=0 where id=" + id_yeucau_xuly_hotro;
                                var rtsupdt = ctx.Database.ExecuteSqlCommand(strSqlupdate);
                                // chuyen tiep ho tro
                                // xác định xem luồng chuyển tiếp là thuộc dạng xử lý luôn hoặc lại chuyển tiếp nữa
                                // (yêu cầu mặc định nếu chuyển tiếp từ KTNV sang thì nó là xử lý luôn)
                                // LOAI_HANH_DONG: 0: không xử lý; 1: Chuyển tiếp, 2: Xử lý luôn, 3, Gửi phản hồi (xử lý xong)
                                string strchuyentiepht = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                                        (
                                                          ID_YEUCAU_HOTRO_HT
                                                         ,ID_HETHONG_YCHT
                                                         ,ID_NODE_LUONG_HOTRO
                                                         ,NGUOIHOTRO
                                                         ,LOAIHANHDONG
                                                         ,NOIDUNGXULY
                                                         ,NOIDUNGXLCHITIET
                                                         ,NGAYXULY
                                                         ,TRANGTHAI
                                                         ,ID_DONVI_FROM
                                                         ,ID_DONVI_TO
                                                         ,NGAYTIEPNHAN
                                                         ,NGUOITAO
                                                         ,ID_NGUOITAO
                                                         ,LA_BUOC_HIENTAI
                                                        )
                                                        VALUES
                                                        (
                                                          {0} -- ID_YEUCAU_HOTRO_HT - int
                                                         ,{1} -- ID_HETHONG_YCHT - int
                                                         ,{2} -- ID_NODE_LUONG_HOTRO - int
                                                         ,N'{3}' -- NGUOIHOTRO - nvarchar(50)
                                                         ,{4} -- LOAIHANHDONG - int
                                                         ,N'{5}' -- NOIDUNGXULY - nvarchar(4000)
                                                         ,N'{6}'
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYXULY - datetime
                                                         ,{7} -- TRANGTHAI - int
                                                         ,{8}
                                                         ,{9} -- ID_DONVI_TO - varchar(50)
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTIEPNHAN - datetime
                                                         ,N'{10}' -- NGUOITAO - nvarchar(50)
                                                         ,{11}
                                                         ,1
                                                        ); SELECT SCOPE_IDENTITY();",
                                                            tthotro.ID_YEUCAU_HOTRO_HT,
                                                            tthotro.ID_HETHONG_YCHT,
                                                            nodebandau[0].ID,
                                                            nguoixuly,
                                                            3, // 3: Chờ xác nhận của người yêu cầu hỗ trợ
                                                            noidungxuly,
                                                            noidungxulychitiet,
                                                            2, //2: đã phản hồi lại người nhận ban đầu, chờ kiểm tra
                                                            iddonvi_from,
                                                            nodebandau[0].ID_DONVI,
                                                            nguoixuly,
                                                            id_nguoixuly);
                                //var rtchuyentiepht = ctx.Database.ExecuteSqlCommand(strchuyentiepht);
                                var rest = ctx.Database.SqlQuery<decimal>(strchuyentiepht).ToList();
                                id_xuly_yeucau_hotro_hethong = rest.FirstOrDefault();

                                // chuyển vào chờ gửi email/sms: 
                                // 1. xóa thông tin cũ mail/sms
                                //string delMailInfo = string.Format(@"delete HT_TO_SEND_EMAIL where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                                //string delSmsInfo = string.Format(@"delete HT_TO_SEND_SMS where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                                //ctx.Database.ExecuteSqlCommand(delMailInfo);
                                //ctx.Database.ExecuteSqlCommand(delSmsInfo);
                                // 2. chèn thông tin mới mail/sms
                                string addMailInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_EMAIL
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                             )", tthotro.ID_YEUCAU_HOTRO_HT
                                                                     , tthotro.ID
                                                                     , noidungxuly
                                                                     , iddonvi_from
                                                                     , id_donvi_yeucau//nodebandau[0].ID_DONVI
                                                                     , noidungxulychitiet);
                                string addSmsInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_SMS
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                            )", tthotro.ID_YEUCAU_HOTRO_HT
                                                                     , tthotro.ID
                                                                     , noidungxuly
                                                                     , iddonvi_from
                                                                     , nodebandau[0].ID_DONVI
                                                                     , noidungxulychitiet);
                                ctx.Database.ExecuteSqlCommand(addMailInfo);
                                ctx.Database.ExecuteSqlCommand(addSmsInfo);
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Actions.ActionProcess.GhiLog(ex, "Xử lý luôn hỗ trợ");
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }
            return id_hethong_HoTro + "|" + id_yeucau_HoTro + "|" + id_xuly_yeucau_hotro_hethong;
        }
        /// <summary>
        /// chuyển phản hồi yêu cầu hỗ trợ về đơn vị trước
        /// </summary>
        /// <param name="id_ht_xuly_hotro"></param>
        /// <param name="noidungxuly"></param>
        /// <param name="noidungxulychitiet"></param>
        /// <param name="donvichuyenden"></param>
        /// <param name="nguoixuly"></param>
        /// <returns></returns>
        [WebMethod]
        public string chuyenPhanHoi(string id_ht_xuly_hotro, string noidungxuly, string noidungxulychitiet, string iddonvi_from, string iddonvi_to, string nguoixuly, string id_nguoixuly)
        {
            string ret = "";
            decimal id_hethong_HoTro = 0;
            decimal id_yeucau_HoTro = 0;
            decimal id_xuly_yeucau_hotro_hethong = 0;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    using (var trans = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1 lấy thông tin hỗ trợ gần nhất
                            string strSql = @" select top 1 a.ID
                                              ,a.ID_YEUCAU_HOTRO_HT
                                              ,a.ID_HETHONG_YCHT
                                              ,a.ID_NODE_LUONG_HOTRO
                                              ,a.NGUOIHOTRO
                                              ,a.LOAIHANHDONG
                                              ,a.NOIDUNGXULY
                                              ,a.NOIDUNGXLCHITIET
                                              ,a.NGAYXULY
                                              ,a.TRANGTHAI
                                              ,a.ID_DONVI_FROM
                                              ,a.ID_DONVI_TO
                                              ,a.NGAYTIEPNHAN
                                              ,a.NGUOITAO
                                              ,a.LA_BUOC_HIENTAI
                                              ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                                              from HT_XULY_YEUCAU_HOTRO a
                                              INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                                              INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                                              where a.id = " + id_ht_xuly_hotro + " order by id desc  ";
                            var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                            var lst = rt.ToList();
                            var tthotro = lst.FirstOrDefault();
                            // lấy thông tin đơn vị gốc của người chuyển ban đầu

                            string strgetdv = " SELECT * from HT_NGUOIDUNG nsd WHERE nsd.TenTruyCap='" + tthotro.NGUOITAO + "' ";
                            var rtss = ctx.Database.SqlQuery<NguoiSuDung>(strgetdv);
                            var lstsss = rtss.ToList();
                            var tthotrosss = lstsss.FirstOrDefault();


                            // 2 update lại trạng thái yêu cầu hỗ trợ xử lý
                            //  1: Chuyển tiếp; 2: Chuyển phản hồi, 3: Chờ xác nhận, 4: Đã xử lý xong
                            var strSqlupdatedanhmuc = string.Format(@"UPDATE HT_DM_YEUCAU_HOTRO_HT 
                                                                     SET TRANGTHAI=2 
                                                                    WHERE ID={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                            ctx.Database.ExecuteSqlCommand(strSqlupdatedanhmuc);


                            // 2 xử lý luồng
                            // update loai_hanh_dong ve 0, bước hiện tẠI là 0
                            string strSqlupdate = " update HT_XULY_YEUCAU_HOTRO set LA_BUOC_HIENTAI=0 where id=" + id_ht_xuly_hotro;
                            var rtsupdt = ctx.Database.ExecuteSqlCommand(strSqlupdate);
                            // chuyen tiep ho tro
                            // xác định xem luồng chuyển tiếp là thuộc dạng xử lý luôn hoặc lại chuyển tiếp nữa
                            // (yêu cầu mặc định nếu chuyển tiếp từ KTNV sang thì nó là xử lý luôn)
                            // LOAI_HANH_DONG: 0: không xử lý; 1: Chuyển tiếp, 2: Xử lý luôn, 3, Gửi phản hồi

                            // hiện chuyển phản hồi sẽ được thực hiện theo luồng cấu hình ban đầu
                            // nếu a==>b==>c==>d thì nếu chuyển phản hồi thì b sẽ phải về b==>a

                            // hành động chuyển phản hồi là chuyển về phía gốc tạo yêu cầu hỗ trợ
                            // xử lý lấy luồng chính thức hiện hành và bước

                            var id_luong_hotro = tthotro.ID_LUONG_HOTRO;
                            id_hethong_HoTro = tthotro.ID_HETHONG_YCHT.Value;
                            id_yeucau_HoTro = tthotro.ID_YEUCAU_HOTRO_HT.Value;


                            // lấy id_donvi gốc ban đầu của luồng
                            var strSqlInfoFistNode = string.Format(@"SELECT CAST(b.ID_LUONG_HOTRO AS NVARCHAR(10))+
                                                                '_'+CAST(b.ID_DONVI AS NVARCHAR(10))+
                                                                '_'+CAST(b.BUOCXULY AS NVARCHAR(10)) AS BUOCXLDAUTIEN
                                                                FROM [HT_XULY_YEUCAU_HOTRO] a
                                                                LEFT JOIN dbo.HT_NODE_LUONG_HOTRO b ON a.ID_NODE_LUONG_HOTRO=b.ID
                                                                WHERE a.NOIDUNGXULY LIKE N'%Khởi tạo hỗ trợ%'
                                                                AND a.ID_YEUCAU_HOTRO_HT={0}",
                                                                    tthotro.ID_YEUCAU_HOTRO_HT);
                            var thongtinxuly1 = ctx.Database.SqlQuery<string>(strSqlInfoFistNode).FirstOrDefault();
                            var lstArrxuly1 = thongtinxuly1.Split('_');
                            var lst1Luonght = lstArrxuly1[0];
                            var donvidautienyeucau = lstArrxuly1[1];
                            var buocxulydautien = lstArrxuly1[2];

                            // lấy FULL danh sách bước xử lý của luồng
                            var strDsxulytoanboluong = string.Format(@"SELECT * FROM HT_NODE_LUONG_HOTRO 
                                                                   WHERE ID_LUONG_HOTRO={0} AND BUOCXULY={1} AND ID_DONVI={2}
                                                                   OR (BUOCXULY!=1 AND ID_LUONG_HOTRO={3})",
                                                                       lst1Luonght, 1, donvidautienyeucau, lst1Luonght);
                            var lstFullluongxulyhotro = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO2>(strDsxulytoanboluong).ToList();
                            // lấy thông tin đơn vị xử lý hiện tại
                            var dvbandau = Convert.ToInt32(tthotro.ID_DONVI_TO);
                            var infotdvxulyhientai = lstFullluongxulyhotro.Where(p => p.ID_DONVI == dvbandau).FirstOrDefault();
                            // lấy thông tin đơn vị xử lý phản hồi trước
                            var infophanhoitruocdo = lstFullluongxulyhotro.Where(p => p.BUOCXULY == infotdvxulyhientai.BUOCXULY - 1).FirstOrDefault();
                            // lấy thông tin về node xử lý trước này


                            //++++++++++++++++++++++++++++++++++++++++++++++++++
                            // xử lý lấy thông tin về node trước đó gần nhất đây nhưng mà ở bước trước
                            // mà có bước xử lý nhỏ hơn bước hiện tại 1
                            var strSqlGetPrevNode = string.Format(@"SELECT top 1 b.ID
                                        ,b.ID_HETHONG_YCHT
                                        ,b.ID_LUONG_HOTRO
                                        ,b.ID_DONVI
                                        ,b.BUOCXULY
                                        ,b.NGAYTAO
                                        FROM HT_HTKTTT.dbo.HT_XULY_YEUCAU_HOTRO a
                                        INNER JOIN HT_NODE_LUONG_HOTRO b on a.ID_NODE_LUONG_HOTRO=b.ID
                                        WHERE b.BUOCXULY={0} and a.ID_HETHONG_YCHT={1} AND b.ID_LUONG_HOTRO={2}
                                        and  a.ID_YEUCAU_HOTRO_HT={3}
                                        ORDER BY a.ID DESC", infotdvxulyhientai.BUOCXULY - 1, id_hethong_HoTro, id_luong_hotro, tthotro.ID_YEUCAU_HOTRO_HT);
                            var infoNodeLuongXLPrev = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO6>(strSqlGetPrevNode);
                            var lstNodeLuongXLPrev = infoNodeLuongXLPrev.FirstOrDefault();
                            //++++++++++++++++++++++++++++++++++++++++++++++++++


                            //var sqlNodeTruoc = string.Format(@"SELECT
                            //                                      *
                            //                                    FROM HT_HTKTTT.dbo.HT_XULY_YEUCAU_HOTRO WHERE ID={0}", id_ht_xuly_hotro);


                            //var lstInfoNode = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO8>(sqlNodeTruoc).ToList();
                            //var id_node_luonghotro_xuly = lstInfoNode[0].ID;
                            //var id_don_vi_truoc = lstInfoNode[0].ID_DONVI_FROM;

                            //var sqlStrNodeTruoc = string.Format(@"SELECT * FROM HT_NODE_LUONG_HOTRO 
                            //                                      WHERE ID_HETHONG_YCHT={0} AND ID_LUONG_HOTRO={1} 
                            //                                      AND ID_DONVI={2}", id_hethong_HoTro, id_luong_hotro, id_don_vi_truoc);
                            //var nodeTruocdo = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO6>(sqlStrNodeTruoc).FirstOrDefault();



                            string strchuyentiepht = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                                (
                                                  ID_YEUCAU_HOTRO_HT
                                                 ,ID_HETHONG_YCHT
                                                 ,ID_NODE_LUONG_HOTRO
                                                 ,NGUOIHOTRO
                                                 ,LOAIHANHDONG
                                                 ,NOIDUNGXULY
                                                 ,NOIDUNGXLCHITIET
                                                 ,NGAYXULY
                                                 ,TRANGTHAI
                                                 ,ID_DONVI_FROM
                                                 ,ID_DONVI_TO
                                                 ,NGAYTIEPNHAN
                                                 ,NGUOITAO
                                                 ,ID_NGUOITAO
                                                 ,LA_BUOC_HIENTAI
                                                )
                                                VALUES
                                                (
                                                  {0}  
                                                 ,{1}  
                                                 ,{2}  
                                                 ,N'{3}'  
                                                 ,{4} 
                                                 ,N'{5}'  
                                                 ,N'{6}' 
                                                 ,GETDATE()  
                                                 ,{7}  
                                                 ,{8} 
                                                 ,{9}
                                                 ,GETDATE() 
                                                 ,N'{10}' 
                                                 ,{11}  
                                                 ,{12} 
                                                ); SELECT SCOPE_IDENTITY();",
                                                        tthotro.ID_YEUCAU_HOTRO_HT,
                                                        tthotro.ID_HETHONG_YCHT,
                                                        lstNodeLuongXLPrev.ID,
                                                        nguoixuly,
                                                        2, // 2 là Chuyển phản hồi
                                                        noidungxuly,
                                                        noidungxulychitiet,
                                                        1, // 1: đang xử lý; 
                                                        iddonvi_from,
                                                        lstNodeLuongXLPrev.ID_DONVI, // là đơn vị sẽ xử lý tiếp theo
                                                        nguoixuly,
                                                        id_nguoixuly,
                                                        1 // là bước hiện tại
                                                        );
                            //var rtchuyentiepht = ctx.Database.ExecuteSqlCommand(strchuyentiepht);
                            var rest = ctx.Database.SqlQuery<decimal>(strchuyentiepht).ToList();
                            id_xuly_yeucau_hotro_hethong = rest.FirstOrDefault();

                            #region email/sms
                            // chuyển vào chờ gửi email/sms: 
                            // 1. xóa thông tin cũ mail/sms
                            //string delMailInfo = string.Format(@"delete HT_TO_SEND_EMAIL where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                            //string delSmsInfo = string.Format(@"delete HT_TO_SEND_SMS where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                            //ctx.Database.ExecuteSqlCommand(delMailInfo);
                            //ctx.Database.ExecuteSqlCommand(delSmsInfo);
                            // 2. chèn thông tin mới mail/sms
                            string addMailInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_EMAIL
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                             )", tthotro.ID_YEUCAU_HOTRO_HT
                                                                 , tthotro.ID
                                                                 , noidungxuly
                                                                 , iddonvi_from
                                                                 , lstNodeLuongXLPrev.ID_DONVI // là đơn vị sẽ xử lý tiếp theo
                                                                 , noidungxulychitiet);
                            string addSmsInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_SMS
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                            )", tthotro.ID_YEUCAU_HOTRO_HT
                                                              , tthotro.ID
                                                              , noidungxuly
                                                              , iddonvi_from
                                                              , lstNodeLuongXLPrev.ID_DONVI  // là đơn vị sẽ xử lý tiếp theo
                                                              , noidungxulychitiet);
                            ctx.Database.ExecuteSqlCommand(addMailInfo);
                            ctx.Database.ExecuteSqlCommand(addSmsInfo);
                            #endregion

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Actions.ActionProcess.GhiLog(ex, "Chuyển phản hồi hỗ trợ");
                            trans.Rollback();
                            ret = ex.Message;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }
            return id_hethong_HoTro + "|" + id_yeucau_HoTro + "|" + id_xuly_yeucau_hotro_hethong;
        }
        /// <summary>
        /// chuyển tiếp luôn hỗ trợ xử lý cho đơn vị khác xử lý tiếp
        /// </summary>
        /// <param name="id_xuly_yeucau_hotro"></param>
        /// <param name="noidungxuly"></param>
        /// <param name="noidungxulychitiet"></param>
        /// <param name="donvichuyenden"></param>
        /// <param name="nguoixuly"></param>
        /// <returns></returns>
        [WebMethod]
        public string chuyenTiepHoTro(string id_xuly_yeucau_hotro, string noidungxuly, string noidungxulychitiet, string iddonvi_from, string iddonvi_to, string nguoixuly, string id_nguoixuly)
        {
            string ret = "";
            decimal id_hethong_HoTro = 0;
            decimal id_yeucau_HoTro = 0;
            decimal id_xuly_yeucau_hotro_hethong = 0;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    using (var trans = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1 lấy thông tin hỗ trợ
                            string strSql = @" select a.ID
                                              ,a.ID_YEUCAU_HOTRO_HT
                                              ,a.ID_HETHONG_YCHT
                                              ,a.ID_NODE_LUONG_HOTRO
                                              ,a.NGUOIHOTRO
                                              ,a.LOAIHANHDONG
                                              ,a.NOIDUNGXULY
                                              ,a.NOIDUNGXLCHITIET
                                              ,a.NGAYXULY
                                              ,a.TRANGTHAI
                                              ,a.ID_DONVI_FROM
                                              ,a.ID_DONVI_TO
                                              ,a.NGAYTIEPNHAN
                                              ,a.NGUOITAO
                                              ,a.LA_BUOC_HIENTAI
                                              ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                                              from HT_XULY_YEUCAU_HOTRO a
                                              INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                                              INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                                          where a.id=" + id_xuly_yeucau_hotro;
                            var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                            var lst = rt.ToList();
                            var tthotro = lst.FirstOrDefault();

                            // 2 lấy thông tin về node tiếp theo
                            var id_yeucau_hotro_ht = tthotro.ID_YEUCAU_HOTRO_HT;
                            var id_hethong_ycht = tthotro.ID_HETHONG_YCHT;
                            var id_luong_hotro = tthotro.ID_LUONG_HOTRO;

                            id_hethong_HoTro = id_hethong_ycht.Value;
                            id_yeucau_HoTro = id_yeucau_hotro_ht.Value;

                            // 2 update lại trạng thái yêu cầu hỗ trợ xử lý
                            //  1: Chuyển tiếp; 2: Chuyển phản hồi, 3: Chờ xác nhận, 4: Đã xử lý xong
                            var strSqlupdatedanhmuc = string.Format(@"UPDATE HT_DM_YEUCAU_HOTRO_HT 
                                                                    SET TRANGTHAI=1 
                                                                    WHERE ID={0}", id_yeucau_hotro_ht);
                            ctx.Database.ExecuteSqlCommand(strSqlupdatedanhmuc);

                            var lstnodetiep = new List<HT_NODE_LUONG_HOTRO>();
                            var strno = string.Format(@"SELECT a.ID
                                                      ,a.ID_HETHONG_YCHT
                                                      ,a.ID_LUONG_HOTRO
                                                      ,a.ID_DONVI
                                                      ,a.BUOCXULY
                                                      ,a.NGAYTAO
                                                      ,b.SOBUOC
                                                      ,'' TENDONVI 
                                                      from HT_NODE_LUONG_HOTRO a
                                                      INNER JOIN HT_LUONG_HOTRO b on a.ID_LUONG_HOTRO=b.ID
                                                      INNER JOIN HT_DM_YEUCAU_HOTRO_HT c ON c.ID_LUONG_HOTRO=b.ID
                                                      WHERE c.ID={0} AND a.ID_HETHONG_YCHT={1} AND a.ID_DONVI={2}
                                                      and a.ID_LUONG_HOTRO={3}",
                                                      id_yeucau_hotro_ht,
                                                      id_hethong_ycht,
                                                      iddonvi_to,
                                                      id_luong_hotro
                                                      );
                            var lstqr = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strno);
                            lstnodetiep = lstqr.ToList();
                            if (lstnodetiep.Any())
                            {
                                // 2 xử lý luồng
                                // update loai_hanh_dong ve 0
                                string strSqlupdate = " update HT_XULY_YEUCAU_HOTRO set LA_BUOC_HIENTAI=0 where id=" + id_xuly_yeucau_hotro;
                                var rtsupdt = ctx.Database.ExecuteSqlCommand(strSqlupdate);
                                // chuyen tiep ho tro
                                // xác định xem luồng chuyển tiếp là thuộc dạng xử lý luôn hoặc lại chuyển tiếp nữa
                                // (yêu cầu mặc định nếu chuyển tiếp từ KTNV sang thì nó là xử lý luôn)
                                // LOAI_HANH_DONG: 0: không xử lý; 1: Chuyển tiếp, 2: Xử lý luôn, 3, Gửi phản hồi (xử lý xong)
                                string strchuyentiepht = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                                        (
                                                          ID_YEUCAU_HOTRO_HT
                                                         ,ID_HETHONG_YCHT
                                                         ,ID_NODE_LUONG_HOTRO
                                                         ,NGUOIHOTRO
                                                         ,LOAIHANHDONG
                                                         ,NOIDUNGXULY
                                                         ,NOIDUNGXLCHITIET
                                                         ,NGAYXULY
                                                         ,TRANGTHAI
                                                         ,ID_DONVI_FROM
                                                         ,ID_DONVI_TO
                                                         ,NGAYTIEPNHAN
                                                         ,NGUOITAO
                                                         ,ID_NGUOITAO
                                                         ,LA_BUOC_HIENTAI
                                                        )
                                                        VALUES
                                                        (
                                                          {0} -- ID_YEUCAU_HOTRO_HT - int
                                                         ,{1} -- ID_HETHONG_YCHT - int
                                                         ,{2} -- ID_NODE_LUONG_HOTRO - int
                                                         ,N'{3}' -- NGUOIHOTRO - nvarchar(50)
                                                         ,{4} -- LOAIHANHDONG - int
                                                         ,N'{5}' -- NOIDUNGXULY - nvarchar(4000)
                                                         ,N'{6}'
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYXULY - datetime
                                                         ,{7} -- TRANGTHAI - int
                                                         ,'{8}' -- ID_DONVI_FROM - varchar(50)
                                                         ,'{9}' -- ID_DONVI_TO - varchar(50)
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTIEPNHAN - datetime
                                                         ,N'{10}' -- NGUOITAO - nvarchar(50)
                                                         ,{11}  
                                                         ,1
                                                        ); SELECT SCOPE_IDENTITY();",
                                                            tthotro.ID_YEUCAU_HOTRO_HT,
                                                            tthotro.ID_HETHONG_YCHT,
                                                            lstnodetiep[0].ID,
                                                            nguoixuly,
                                                            1, // 1: là chuyển tiếp
                                                            noidungxuly,
                                                            noidungxulychitiet,
                                                            1,
                                                            iddonvi_from,
                                                            iddonvi_to,
                                                            nguoixuly,
                                                            id_nguoixuly);
                                //var rtchuyentiepht = ctx.Database.ExecuteSqlCommand(strchuyentiepht);
                                var rest = ctx.Database.SqlQuery<decimal>(strchuyentiepht).ToList();
                                id_xuly_yeucau_hotro_hethong = rest.FirstOrDefault();

                                // chuyển vào chờ gửi email/sms: 
                                // 1. xóa thông tin cũ mail/sms
                                //string delMailInfo = string.Format(@"delete HT_TO_SEND_EMAIL where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                                //string delSmsInfo = string.Format(@"delete HT_TO_SEND_SMS where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                                //ctx.Database.ExecuteSqlCommand(delMailInfo);
                                //ctx.Database.ExecuteSqlCommand(delSmsInfo);
                                // 2. chèn thông tin mới mail/sms
                                string addMailInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_EMAIL
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                             )", tthotro.ID_YEUCAU_HOTRO_HT
                                                               , tthotro.ID
                                                               , noidungxuly
                                                               , iddonvi_from
                                                               , iddonvi_to    // donvichuyenden
                                                               , noidungxulychitiet);
                                string addSmsInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_SMS
                                                                (
                                                                  ID_YEUCAU_HOTRO_HT
                                                                 ,ID_XULY_YEUCAU_HOTRO
                                                                 ,NOIDUNGXULY
                                                                 ,ID_DONVI_FROM
                                                                 ,ID_DONVI_TO
                                                                 ,NOIDUNGXLCHITIET
                                                                 ,NGAYTAO
                                                                )
                                                                VALUES
                                                                (
                                                                  {0} -- ID_YEUCAU_HOTRO_HT - int
                                                                 ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                                 ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                                 ,{3} -- ID_DONVI_FROM - int
                                                                 ,{4} -- ID_DONVI_TO - int
                                                                 ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                                 ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                                )", tthotro.ID_YEUCAU_HOTRO_HT
                                                                  , tthotro.ID
                                                                  , noidungxuly
                                                                  , iddonvi_from
                                                                  , iddonvi_to// donvichuyenden
                                                                  , noidungxulychitiet);
                                ctx.Database.ExecuteSqlCommand(addMailInfo);
                                ctx.Database.ExecuteSqlCommand(addSmsInfo);
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Actions.ActionProcess.GhiLog(ex, "Chuyển tiếp hỗ trợ");
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "");
            }
            return id_hethong_HoTro + "|" + id_yeucau_HoTro + "|" + id_xuly_yeucau_hotro_hethong;
        }

        // chuyển tiếp hàng loạt
        // chỉ chuyển được nếu các đơn vị phải cùng nơi chuyển đến
        [WebMethod]
        public string chuyenTiepHangLoat(string[] id_xuly_yeucau_hotro, string noidungxuly, string noidungxulychitiet, string[] id_donvi_form, string id_donvi_to, string nguoixuly)
        {
            string ret = "";
            decimal id_hethong_HoTro = 0;
            decimal id_yeucau_HoTro = 0;
            decimal id_xuly_yeucau_hotro_hethong = 0;
            using (var ctx = new ADDJContext())
            {
                using (var trans = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        // duyệt xử lý lần lượt các yêu cầu hỗ trợ
                        foreach (var item in id_xuly_yeucau_hotro)
                        {
                            // 1 lấy thông tin hỗ trợ
                            string strSql = @" select a.ID
                                              ,a.ID_YEUCAU_HOTRO_HT
                                              ,a.ID_HETHONG_YCHT
                                              ,a.ID_NODE_LUONG_HOTRO
                                              ,a.NGUOIHOTRO
                                              ,a.LOAIHANHDONG
                                              ,a.NOIDUNGXULY
                                              ,a.NOIDUNGXLCHITIET
                                              ,a.NGAYXULY
                                              ,a.TRANGTHAI
                                              ,a.ID_DONVI_FROM
                                              ,a.ID_DONVI_TO
                                              ,a.NGAYTIEPNHAN
                                              ,a.NGUOITAO
                                              ,a.LA_BUOC_HIENTAI
                                              ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                                              from HT_XULY_YEUCAU_HOTRO a
                                              INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                                              INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                                              where a.id=" + item;
                            var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                            var lst = rt.ToList();
                            var tthotro = lst.FirstOrDefault();

                            // 2 lấy thông tin về node tiếp theo
                            var id_yeucau_hotro_ht = tthotro.ID_YEUCAU_HOTRO_HT;
                            var id_hethong_ycht = tthotro.ID_HETHONG_YCHT;
                            var id_luong_hotro = tthotro.ID_LUONG_HOTRO;

                            id_hethong_HoTro = id_hethong_ycht.Value;
                            id_yeucau_HoTro = id_yeucau_hotro_ht.Value;

                            // 2 update lại trạng thái yêu cầu hỗ trợ xử lý
                            //  1: Chuyển tiếp; 2: Chuyển phản hồi, 3: Chờ xác nhận, 4: Đã xử lý xong
                            var strSqlupdatedanhmuc = string.Format(@"UPDATE HT_DM_YEUCAU_HOTRO_HT 
                                                                      SET TRANGTHAI=1 
                                                                      WHERE ID={0}", id_yeucau_hotro_ht);
                            ctx.Database.ExecuteSqlCommand(strSqlupdatedanhmuc);

                            var lstnodetiep = new List<HT_NODE_LUONG_HOTRO>();
                            var strno = string.Format(@"SELECT a.ID
                                                       ,a.ID_HETHONG_YCHT
                                                       ,a.ID_LUONG_HOTRO
                                                       ,a.ID_DONVI
                                                       ,a.BUOCXULY
                                                       ,a.NGAYTAO
                                                       ,b.SOBUOC
                                                       ,'' TENDONVI 
                                                       from HT_NODE_LUONG_HOTRO a
                                                       INNER JOIN HT_LUONG_HOTRO b on a.ID_LUONG_HOTRO=b.ID
                                                       INNER JOIN HT_DM_YEUCAU_HOTRO_HT c ON c.ID_LUONG_HOTRO=b.ID
                                                       WHERE c.ID={0} AND a.ID_HETHONG_YCHT={1} AND a.ID_DONVI={2}
                                                       and a.ID_LUONG_HOTRO={3}",
                                                       id_yeucau_hotro_ht,
                                                       id_hethong_ycht,
                                                       id_donvi_to,
                                                       id_luong_hotro
                                                      );
                            var lstqr = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strno);
                            lstnodetiep = lstqr.ToList();
                            if (lstnodetiep.Any())
                            {
                                // 2 xử lý luồng
                                // update loai_hanh_dong ve 0
                                string strSqlupdate = " update HT_XULY_YEUCAU_HOTRO set LA_BUOC_HIENTAI=0 where id=" + item;
                                var rtsupdt = ctx.Database.ExecuteSqlCommand(strSqlupdate);
                                // chuyen tiep ho tro
                                // xác định xem luồng chuyển tiếp là thuộc dạng xử lý luôn hoặc lại chuyển tiếp nữa
                                // (yêu cầu mặc định nếu chuyển tiếp từ KTNV sang thì nó là xử lý luôn)
                                // LOAI_HANH_DONG: 0: không xử lý; 1: Chuyển tiếp, 2: Xử lý luôn, 3, Gửi phản hồi (xử lý xong)
                                string strchuyentiepht = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                                        (
                                                          ID_YEUCAU_HOTRO_HT
                                                         ,ID_HETHONG_YCHT
                                                         ,ID_NODE_LUONG_HOTRO
                                                         ,NGUOIHOTRO
                                                         ,LOAIHANHDONG
                                                         ,NOIDUNGXULY
                                                         ,NOIDUNGXLCHITIET
                                                         ,NGAYXULY
                                                         ,TRANGTHAI
                                                         ,ID_DONVI_FROM
                                                         ,ID_DONVI_TO
                                                         ,NGAYTIEPNHAN
                                                         ,NGUOITAO
                                                         ,LA_BUOC_HIENTAI
                                                        )
                                                        VALUES
                                                        (
                                                          {0} -- ID_YEUCAU_HOTRO_HT - int
                                                         ,{1} -- ID_HETHONG_YCHT - int
                                                         ,{2} -- ID_NODE_LUONG_HOTRO - int
                                                         ,N'{3}' -- NGUOIHOTRO - nvarchar(50)
                                                         ,{4} -- LOAIHANHDONG - int
                                                         ,N'{5}' -- NOIDUNGXULY - nvarchar(4000)
                                                         ,N'{6}'
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYXULY - datetime
                                                         ,{7} -- TRANGTHAI - int
                                                         ,'{8}' -- ID_DONVI - varchar(50)
                                                         ,'{9}' -- ID_DONVI - varchar(50)
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTIEPNHAN - datetime
                                                         ,N'{10}' -- NGUOITAO - nvarchar(50)
                                                         ,1
                                                        ); SELECT SCOPE_IDENTITY();",
                                                            tthotro.ID_YEUCAU_HOTRO_HT,
                                                            tthotro.ID_HETHONG_YCHT,
                                                            lstnodetiep[0].ID,
                                                            nguoixuly,
                                                            1, // 1: là chuyển tiếp
                                                            noidungxuly,
                                                            noidungxulychitiet,
                                                            1,
                                                            id_donvi_to,
                                                            nguoixuly);
                                //var rtchuyentiepht = ctx.Database.ExecuteSqlCommand(strchuyentiepht);
                                var rest = ctx.Database.SqlQuery<decimal>(strchuyentiepht).ToList();
                                id_xuly_yeucau_hotro_hethong = rest.FirstOrDefault();

                                // chuyển vào chờ gửi email/sms: 
                                // 1. xóa thông tin cũ mail/sms
                                //string delMailInfo = string.Format(@"delete HT_TO_SEND_EMAIL where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                                //string delSmsInfo = string.Format(@"delete HT_TO_SEND_SMS where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                                //ctx.Database.ExecuteSqlCommand(delMailInfo);
                                //ctx.Database.ExecuteSqlCommand(delSmsInfo);
                                // 2. chèn thông tin mới mail/sms
                                string addMailInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_EMAIL
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                             )", tthotro.ID_YEUCAU_HOTRO_HT
                                                               , tthotro.ID
                                                               , tthotro.ID_NODE_LUONG_HOTRO
                                                               , tthotro.ID_DONVI_TO// donvichuyenden
                                                               , noidungxuly);
                                string addSmsInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_SMS
                                                                (
                                                                  ID_YEUCAU_HOTRO_HT
                                                                 ,ID_XULY_YEUCAU_HOTRO
                                                                 ,NOIDUNGXULY
                                                                 ,ID_DONVI_FROM
                                                                 ,ID_DONVI_TO
                                                                 ,NOIDUNGXLCHITIET
                                                                 ,NGAYTAO
                                                                )
                                                                VALUES
                                                                (
                                                                  {0} -- ID_YEUCAU_HOTRO_HT - int
                                                                 ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                                 ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                                 ,{3} -- ID_DONVI_FROM - int
                                                                 ,{4} -- ID_DONVI_TO - int
                                                                 ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                                 ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                                )", tthotro.ID_YEUCAU_HOTRO_HT
                                                                  , tthotro.ID
                                                                  , tthotro.ID_NODE_LUONG_HOTRO
                                                                  , tthotro.ID_DONVI_FROM
                                                                  , tthotro.ID_DONVI_TO//donvichuyenden
                                                                  , noidungxuly);
                                ctx.Database.ExecuteSqlCommand(addMailInfo);
                                ctx.Database.ExecuteSqlCommand(addSmsInfo);
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Actions.ActionProcess.GhiLog(ex, "Chuyển tiếp hỗ trợ");
                        trans.Rollback();
                    }
                }
                return id_hethong_HoTro + "|" + id_yeucau_HoTro + "|" + id_xuly_yeucau_hotro_hethong;
            }
        }

        // chuyển phản hồi hàng loạt
        // thì mặc định các yêu cầu hỗ trợ sẽ tự chuyển về đơn vị trước đó theo luồng xử lý
        // của mỗi hỗ trợ
        [WebMethod]
        public string chuyenPhanHoiHangLoat(string[] id_xuly_yeucau_hotro, string noidungxuly, string noidungxulychitiet, string[] id_donvi_from, string id_donvi_to, string nguoixuly)
        {
            string ret = "";
            decimal id_hethong_HoTro = 0;
            decimal id_yeucau_HoTro = 0;
            decimal id_xuly_yeucau_hotro_hethong = 0;
            using (var ctx = new ADDJContext())
            {
                using (var trans = ctx.Database.BeginTransaction())
                {
                    foreach (var item in id_xuly_yeucau_hotro)
                    {
                        try
                        {
                            // 1 lấy thông tin hỗ trợ gần nhất
                            string strSql = @" select a.ID
                                              ,a.ID_YEUCAU_HOTRO_HT
                                              ,a.ID_HETHONG_YCHT
                                              ,a.ID_NODE_LUONG_HOTRO
                                              ,a.NGUOIHOTRO
                                              ,a.LOAIHANHDONG
                                              ,a.NOIDUNGXULY
                                              ,a.NOIDUNGXLCHITIET
                                              ,a.NGAYXULY
                                              ,a.TRANGTHAI
                                              ,a.ID_DONVI_FROM
                                              ,a.ID_DONVI_TO
                                              ,a.NGAYTIEPNHAN
                                              ,a.NGUOITAO
                                              ,a.LA_BUOC_HIENTAI
                                              ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                                              from HT_XULY_YEUCAU_HOTRO a
                                              INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                                              INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO
                                              where a.id = " + item + " order by id desc  ";
                            var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                            var lst = rt.ToList();
                            var tthotro = lst.FirstOrDefault();
                            // lấy thông tin đơn vị gốc của người chuyển ban đầu

                            string strgetdv = " SELECT * from NguoiSuDung nsd WHERE nsd.TenTruyCap='" + tthotro.NGUOITAO + "' ";
                            var rtss = ctx.Database.SqlQuery<NguoiSuDung>(strgetdv);
                            var lstsss = rtss.ToList();
                            var tthotrosss = lstsss.FirstOrDefault();


                            // 2 update lại trạng thái yêu cầu hỗ trợ xử lý
                            //  1: Chuyển tiếp; 2: Chuyển phản hồi, 3: Chờ xác nhận, 4: Đã xử lý xong
                            var strSqlupdatedanhmuc = string.Format(@"UPDATE HT_DM_YEUCAU_HOTRO_HT 
                                                                    SET TRANGTHAI=2 
                                                                    WHERE ID={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                            ctx.Database.ExecuteSqlCommand(strSqlupdatedanhmuc);


                            // 2 xử lý luồng
                            // update loai_hanh_dong ve 0, bước hiện tẠI là 0
                            string strSqlupdate = " update HT_XULY_YEUCAU_HOTRO set LA_BUOC_HIENTAI=0 where id=" + item;
                            var rtsupdt = ctx.Database.ExecuteSqlCommand(strSqlupdate);
                            // chuyen tiep ho tro
                            // xác định xem luồng chuyển tiếp là thuộc dạng xử lý luôn hoặc lại chuyển tiếp nữa
                            // (yêu cầu mặc định nếu chuyển tiếp từ KTNV sang thì nó là xử lý luôn)
                            // LOAI_HANH_DONG: 0: không xử lý; 1: Chuyển tiếp, 2: Xử lý luôn, 3, Gửi phản hồi

                            // hiện chuyển phản hồi sẽ được thực hiện theo luồng cấu hình ban đầu
                            // nếu a==>b==>c==>d thì nếu chuyển phản hồi thì b sẽ phải về b==>a

                            // hành động chuyển phản hồi là chuyển về phía gốc tạo yêu cầu hỗ trợ
                            // xử lý lấy luồng chính thức hiện hành và bước

                            var id_luong_hotro = tthotro.ID_LUONG_HOTRO;
                            id_hethong_HoTro = tthotro.ID_HETHONG_YCHT.Value;
                            id_yeucau_HoTro = tthotro.ID_YEUCAU_HOTRO_HT.Value;


                            // lấy id_donvi gốc ban đầu của luồng
                            var strSqlInfoFistNode = string.Format(@"SELECT CAST(b.ID_LUONG_HOTRO AS NVARCHAR(10))+
                                                                '_'+CAST(b.ID_DONVI AS NVARCHAR(10))+
                                                                '_'+CAST(b.BUOCXULY AS NVARCHAR(10)) AS BUOCXLDAUTIEN
                                                                FROM [HT_XULY_YEUCAU_HOTRO] a
                                                                LEFT JOIN dbo.HT_NODE_LUONG_HOTRO b ON a.ID_NODE_LUONG_HOTRO=b.ID
                                                                WHERE a.NOIDUNGXULY LIKE N'%Khởi tạo hỗ trợ%'
                                                                AND a.ID_YEUCAU_HOTRO_HT={0}",
                                                                    tthotro.ID_YEUCAU_HOTRO_HT);
                            var thongtinxuly1 = ctx.Database.SqlQuery<string>(strSqlInfoFistNode).FirstOrDefault();
                            var lstArrxuly1 = thongtinxuly1.Split('_');
                            var lst1Luonght = lstArrxuly1[0];
                            var donvidautienyeucau = lstArrxuly1[1];
                            var buocxulydautien = lstArrxuly1[2];

                            // lấy FULL danh sách bước xử lý của luồng
                            var strDsxulytoanboluong = string.Format(@"SELECT * FROM HT_NODE_LUONG_HOTRO 
                                                                   WHERE ID_LUONG_HOTRO={0} AND BUOCXULY={1} AND ID_DONVI={2}
                                                                   OR (BUOCXULY!=1 AND ID_LUONG_HOTRO={3})",
                                                                       lst1Luonght, 1, donvidautienyeucau, lst1Luonght);
                            var lstFullluongxulyhotro = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO2>(strDsxulytoanboluong).ToList();
                            // lấy thông tin đơn vị xử lý hiện tại
                            var dvbandau = Convert.ToInt32(tthotro.ID_DONVI_TO);
                            var infotdvxulyhientai = lstFullluongxulyhotro.Where(p => p.ID_DONVI == dvbandau).FirstOrDefault();
                            // lấy thông tin đơn vị xử lý phản hồi trước
                            var infophanhoitruocdo = lstFullluongxulyhotro.Where(p => p.BUOCXULY == infotdvxulyhientai.BUOCXULY - 1).FirstOrDefault();


                            string strchuyentiepht = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                                                    (
                                                                      ID_YEUCAU_HOTRO_HT
                                                                     ,ID_HETHONG_YCHT
                                                                     ,ID_NODE_LUONG_HOTRO
                                                                     ,NGUOIHOTRO
                                                                     ,LOAIHANHDONG
                                                                     ,NOIDUNGXULY
                                                                     ,NOIDUNGXLCHITIET
                                                                     ,NGAYXULY
                                                                     ,TRANGTHAI
                                                                     ,ID_DONVI_FROM
                                                                     ,ID_DONVI_TO
                                                                     ,NGAYTIEPNHAN
                                                                     ,NGUOITAO
                                                                     ,LA_BUOC_HIENTAI
                                                                    )
                                                                    VALUES
                                                                    (
                                                                      {0}  
                                                                     ,{1}  
                                                                     ,{2}  
                                                                     ,N'{3}'  
                                                                     ,{4} 
                                                                     ,N'{5}'  
                                                                     ,N'{6}' 
                                                                     ,GETDATE()  
                                                                     ,{7}  
                                                                     ,'{8}'  
                                                                     ,'{9}'  
                                                                     ,GETDATE() 
                                                                     ,N'{10}' 
                                                                     ,{11}  
                                                                    ); SELECT SCOPE_IDENTITY();",
                                                        tthotro.ID_YEUCAU_HOTRO_HT,
                                                        tthotro.ID_HETHONG_YCHT,
                                                        tthotro.ID_NODE_LUONG_HOTRO,
                                                        nguoixuly,
                                                        2, // 2 là Chuyển phản hồi
                                                        noidungxuly,
                                                        noidungxulychitiet,
                                                        1, // 1: đang xử lý; 
                                                        infophanhoitruocdo.ID_DONVI,
                                                        infophanhoitruocdo.ID_DONVI, // là đơn vị sẽ xử lý tiếp theo
                                                        nguoixuly,
                                                        1 // là bước hiện tại
                                                        );
                            //var rtchuyentiepht = ctx.Database.ExecuteSqlCommand(strchuyentiepht);
                            var rest = ctx.Database.SqlQuery<decimal>(strchuyentiepht).ToList();
                            id_xuly_yeucau_hotro_hethong = rest.FirstOrDefault();

                            #region email/sms
                            // chuyển vào chờ gửi email/sms: 
                            // 1. xóa thông tin cũ mail/sms
                            //string delMailInfo = string.Format(@"delete HT_TO_SEND_EMAIL where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                            //string delSmsInfo = string.Format(@"delete HT_TO_SEND_SMS where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                            //ctx.Database.ExecuteSqlCommand(delMailInfo);
                            //ctx.Database.ExecuteSqlCommand(delSmsInfo);
                            // 2. chèn thông tin mới mail/sms
                            string addMailInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_EMAIL
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                             )", tthotro.ID_YEUCAU_HOTRO_HT
                                                                 , tthotro.ID
                                                                 , noidungxuly
                                                                 , infophanhoitruocdo.ID_DONVI
                                                                 , noidungxulychitiet);
                            string addSmsInfo = string.Format(@"INSERT INTO dbo.HT_TO_SEND_SMS
                                                            (
                                                              ID_YEUCAU_HOTRO_HT
                                                             ,ID_XULY_YEUCAU_HOTRO
                                                             ,NOIDUNGXULY
                                                             ,ID_DONVI_FROM
                                                             ,ID_DONVI_TO
                                                             ,NOIDUNGXLCHITIET
                                                             ,NGAYTAO
                                                            )
                                                            VALUES
                                                            (
                                                              {0} -- ID_YEUCAU_HOTRO_HT - int
                                                             ,{1} -- ID_XULY_YEUCAU_HOTRO - int
                                                             ,N'{2}' -- NOIDUNGXULY - nvarchar(4000)
                                                             ,{3} -- ID_DONVI_FROM - int
                                                             ,{4} -- ID_DONVI_TO - int
                                                             ,N'{5}' -- NOIDUNGXLCHITIET - nvarchar(4000)
                                                             ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                            )", tthotro.ID_YEUCAU_HOTRO_HT
                                                                 , tthotro.ID
                                                                 , noidungxuly
                                                                 , infophanhoitruocdo.ID_DONVI
                                                                 , noidungxulychitiet);
                            ctx.Database.ExecuteSqlCommand(addMailInfo);
                            ctx.Database.ExecuteSqlCommand(addSmsInfo);
                            #endregion

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Actions.ActionProcess.GhiLog(ex, "Chuyển phản hồi hỗ trợ");
                            trans.Rollback();
                            ret = ex.Message;
                        }
                    }
                }
                return id_hethong_HoTro + "|" + id_yeucau_HoTro + "|" + id_xuly_yeucau_hotro_hethong;
            };
        }


        // chuyển tiếp xử lý
        [WebMethod]
        public string chuyenTiepHoTroXuLy(string id_yeucauhotro, string noidungxuly, string donvichuyenden, string nguoixuly)
        {
            string ret = "";
            using (var ctx = new ADDJContext())
            {
                using (var trans = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        //1: lấy thông tin về hỗ trợ
                        var strgetinfo = @"";


                        //2: lấy thông tin về bước hiện hành


                        //3: xử lý thông tin chuyển đến


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }
                return ret;
            }
        }

        [WebMethod]
        public string loadThongTinYeuCauHoTro(string id)
        {
            string ret = "";
            using (var ctx = new ADDJContext())
            {
                var strSql = string.Format(@" SELECT * from HT_DM_YEUCAU_HOTRO_HT where ID={0} ", id);
                var lst = ctx.Database.SqlQuery<HT_DM_YEUCAU_HOTRO_HT>(strSql);
                var lsttimeline = lst.ToList();
                if (lsttimeline != null)
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lsttimeline);
            }
            return ret;
        }

        [WebMethod]
        public string loadTimeLineXuLyHT(string idyeucauhotro)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSql = string.Format(@" SELECT hxh.ID,
                                              FORMAT(hxh.NGAYXULY,'dd/MM/yyyy hh:mm:ss') NGAYXULY
                                             ,hhh.TENHETHONG
                                             ,hlh.TEN_LUONG
                                             ,hc.LINHVUC
                                             ,hdyh.NOIDUNG_YEUCAU
                                             ,hxh.ID_NODE_LUONG_HOTRO
                                             ,hxh.NOIDUNGXULY
                                             ,hxh.ID_DONVI_TO
                                             ,dt.TENDONVI DONVIXULY
                                             ,nd.TenDayDu NGUOIXULY
                                             ,nd.TenTruyCap TEN_NGUOIXULY
                                             ,nd.DiDong DT_NGUOIXULY
                                             FROM HT_XULY_YEUCAU_HOTRO hxh
                                             LEFT JOIN HT_DONVI dt ON dt.Id = hxh.ID_DONVI_TO
                                             LEFT JOIN HT_DM_YEUCAU_HOTRO_HT hdyh ON hdyh.Id = hxh.ID_YEUCAU_HOTRO_HT
                                             LEFT JOIN HT_CAYTHUMUC_YCHT hc ON hc.Id = hdyh.ID_CAYTHUMUC_YCHT
                                             left JOIN HT_NODE_LUONG_HOTRO hnlh ON hnlh.Id = hxh.ID_NODE_LUONG_HOTRO
                                             LEFT JOIN HT_LUONG_HOTRO hlh ON hlh.Id = hnlh.ID_LUONG_HOTRO
                                             LEFT JOIN HT_DM_HETHONG_YCHT hhh ON hhh.Id = hlh.ID_HETHONG_YCHT
                                             left JOIN HT_NGUOIDUNG nd ON nd.Id=hxh.ID_NGUOITAO
                                             WHERE hdyh.ID={0} AND hxh.NOIDUNGXULY !=N'Khởi tạo hỗ trợ' 
                                             ORDER BY hxh.NGAYXULY DESC ",
                                                 idyeucauhotro);
                    var lst = ctx.Database.SqlQuery<ViewTimelineXuLyYCHT>(strSql);
                    var lsttimeline = lst.ToList();
                    if (lsttimeline != null)
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lsttimeline);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Timeline");
            }
            return ret;
        }

        [WebMethod]
        public string checkQuyenDongHoTro(string username, string id_xuly_yc_hotro)
        {
            string ret = "0";
            try
            {
                string sqlCheck = string.Format(@"SELECT 
                                                  a.ID
                                                 ,a.ID_YEUCAU_HOTRO_HT
                                                 ,a.ID_HETHONG_YCHT
                                                 ,a.ID_NODE_LUONG_HOTRO
                                                 ,a.NGUOIHOTRO
                                                 ,a.LOAIHANHDONG
                                                 ,a.NOIDUNGXULY
                                                 ,a.NOIDUNGXLCHITIET
                                                 ,a.NGAYXULY
                                                 ,a.TRANGTHAI
                                                 ,a.ID_DONVI_FROM
                                                 ,a.ID_DONVI_TO
                                                 ,a.NGAYTIEPNHAN
                                                 ,a.NGUOITAO
                                                 ,a.LA_BUOC_HIENTAI
                                                 , 0 ID_LUONG_HOTRO,0 BUOCXULY,0 SOBUOC 
                                                 FROM HT_XULY_YEUCAU_HOTRO a
                                                 INNER JOIN HT_NODE_LUONG_HOTRO b ON b.ID=a.ID_NODE_LUONG_HOTRO
                                                 INNER JOIN HT_LUONG_HOTRO c ON c.ID=b.ID_LUONG_HOTRO
                                                 INNER JOIN HT_DM_YEUCAU_HOTRO_HT d ON d.ID=a.ID_YEUCAU_HOTRO_HT
                                                 WHERE a.LA_BUOC_HIENTAI=1 AND d.NGUOITAO='{0}' 
                                                 AND a.ID={1} ", username, id_xuly_yc_hotro);
                var lstYc = new List<HT_XULY_YEUCAU_HOTRO>();
                using (var ctx = new ADDJContext())
                {
                    lstYc = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(sqlCheck).ToList();
                }
                if (lstYc.Any())
                {
                    ret = "1";
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Check quyền đóng");
            }

            return ret;
        }

        /// <summary>
        /// đóng yêu cầu hỗ trợ
        /// </summary>
        /// <param name="id_xuly_yc_hotro"></param>
        /// <param name="noidungdong"></param>
        /// <param name="noidungdongchitiet"></param>
        /// <param name="nguoidong"></param>
        /// <returns></returns>
        [WebMethod]
        public string xulyDongYeuCauHoTro(string id_xuly_yc_hotro, string noidungdong, string noidungdongchitiet, string id_donvidong, string nguoidong, string id_nguoidong)
        {
            string ret = "";
            using (var ctx = new ADDJContext())
            {
                using (var trans = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        // 1 lấy thông tin hỗ trợ
                        string strSql = @"  select a.ID
                                          ,a.ID_YEUCAU_HOTRO_HT
                                          ,a.ID_HETHONG_YCHT
                                          ,a.ID_NODE_LUONG_HOTRO
                                          ,a.NGUOIHOTRO
                                          ,a.LOAIHANHDONG
                                          ,a.NOIDUNGXULY
                                          ,a.NOIDUNGXLCHITIET
                                          ,a.NGAYXULY
                                          ,a.TRANGTHAI
                                          ,a.ID_DONVI_FROM
                                          ,a.ID_DONVI_TO
                                          ,a.NGAYTIEPNHAN
                                          ,a.NGUOITAO
                                          ,a.LA_BUOC_HIENTAI
                                          ,b.ID_LUONG_HOTRO,b.BUOCXULY,c.SOBUOC
                                          from HT_XULY_YEUCAU_HOTRO a
                                          INNER JOIN HT_NODE_LUONG_HOTRO b ON b.id=a.ID_NODE_LUONG_HOTRO
                                          INNER JOIN HT_LUONG_HOTRO c on c.ID=b.ID_LUONG_HOTRO  
                                          where a.id=" + id_xuly_yc_hotro;
                        var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                        var lst = rt.ToList();
                        var tthotro = lst.FirstOrDefault();

                        // 2 lấy thông tin về node tiếp theo
                        var id_yeucau_hotro_ht = tthotro.ID_YEUCAU_HOTRO_HT;
                        var id_hethong_ycht = tthotro.ID_HETHONG_YCHT;
                        var id_luong_hotro = tthotro.ID_LUONG_HOTRO;

                        // 2 update lại trtrangjhais yêu cầu hỗ trợ xử lý
                        // 1: Chuyển tiếp; 2: Chuyển phản hồi, 3: Chờ xác nhận, 4: Đã xử lý xong
                        var strSqlupdatedanhmuc = string.Format(@"UPDATE HT_DM_YEUCAU_HOTRO_HT 
                                                                  SET TRANGTHAI=4,NOIDUNG_XL_DONG_HOTRO=N'{0}'
                                                                  WHERE ID={1}", noidungdong, id_yeucau_hotro_ht);
                        ctx.Database.ExecuteSqlCommand(strSqlupdatedanhmuc);

                        // 3 xử lý luồng
                        // update loai_hanh_dong ve 0
                        string strSqlupdate = " update HT_XULY_YEUCAU_HOTRO " +
                                              " set loaihanhdong=0, LA_BUOC_HIENTAI=0 " +
                                              " where id=" + id_xuly_yc_hotro;
                        var rtsupdt = ctx.Database.ExecuteSqlCommand(strSqlupdate);
                        // chuyen tiep ho tro
                        // xác định xem luồng chuyển tiếp là thuộc dạng xử lý luôn hoặc lại chuyển tiếp nữa
                        // (yêu cầu mặc định nếu chuyển tiếp từ KTNV sang thì nó là xử lý luôn)
                        // LOAI_HANH_DONG: 0: không xử lý; 1: Chuyển tiếp, 2: Xử lý luôn, 3, Gửi phản hồi (xử lý xong)
                        string strchuyentiepht = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                                        (
                                                          ID_YEUCAU_HOTRO_HT
                                                         ,ID_HETHONG_YCHT
                                                         ,ID_NODE_LUONG_HOTRO
                                                         ,NGUOIHOTRO
                                                         ,LOAIHANHDONG
                                                         ,NOIDUNGXULY
                                                         ,NOIDUNGXLCHITIET
                                                         ,NGAYXULY
                                                         ,TRANGTHAI  
                                                         ,ID_DONVI_FROM
                                                         ,ID_DONVI_TO
                                                         ,NGAYTIEPNHAN
                                                         ,NGUOITAO
                                                         ,ID_NGUOITAO
                                                         ,LA_BUOC_HIENTAI
                                                        )
                                                        VALUES
                                                        (
                                                          {0} -- ID_YEUCAU_HOTRO_HT - int
                                                         ,{1} -- ID_HETHONG_YCHT - int
                                                         ,{2} -- ID_NODE_LUONG_HOTRO - int
                                                         ,N'{3}' -- NGUOIHOTRO - nvarchar(50)
                                                         ,{4} -- LOAIHANHDONG - int
                                                         ,N'{5}' -- NOIDUNGXULY - nvarchar(4000)
                                                         ,N'{6}'
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYXULY - datetime
                                                         ,{7} -- TRANGTHAI - int  
                                                         ,{8}
                                                         ,{9}
                                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTIEPNHAN - datetime
                                                         ,N'{10}' -- NGUOITAO - nvarchar(50)
                                                         ,{11}
                                                         ,1
                                                        )",
                                                    tthotro.ID_YEUCAU_HOTRO_HT,
                                                    tthotro.ID_HETHONG_YCHT,
                                                    tthotro.ID_NODE_LUONG_HOTRO,
                                                    nguoidong,
                                                    4, // 4: đã xử lý xong, người đưa yêu cầu hỗ trợ xác nhận
                                                    noidungdong,
                                                    noidungdongchitiet,
                                                    3,
                                                    id_donvidong,
                                                    id_donvidong,
                                                    nguoidong,
                                                    id_nguoidong);
                        var rtchuyentiepht = ctx.Database.ExecuteSqlCommand(strchuyentiepht);

                        // Nếu đóng yêu cầu hỗ trợ thì các thông tin liên quan đến email/sms gửi sẽ hủy
                        // chuyển vào chờ gửi email/sms: 
                        // 1. xóa thông tin cũ mail/sms
                        //string delMailInfo = string.Format(@"delete HT_TO_SEND_EMAIL where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                        //string delSmsInfo = string.Format(@"delete HT_TO_SEND_SMS where id_yeucau_hotro_ht={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                        //ctx.Database.ExecuteSqlCommand(delMailInfo);
                        //ctx.Database.ExecuteSqlCommand(delSmsInfo);


                        // 4. Xử lý DONE
                        // 
                        string strDoneYC = string.Format(@"INSERT INTO HT_DM_YEUCAU_HOTRO_HT_DONE
                                                           SELECT * FROM HT_DM_YEUCAU_HOTRO_HT hdyhh 
                                                           WHERE hdyhh.ID={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                        string strDoneXL = string.Format(@"INSERT INTO HT_XULY_YEUCAU_HOTRO_DONE
                                                           SELECT * FROM HT_XULY_YEUCAU_HOTRO hxyh 
                                                           WHERE hxyh.ID_YEUCAU_HOTRO_HT={0}", tthotro.ID_YEUCAU_HOTRO_HT);
                        ctx.Database.ExecuteSqlCommand(strDoneYC);
                        ctx.Database.ExecuteSqlCommand(strDoneXL);

                        trans.Commit();
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                    }
                    catch (Exception ex)
                    {
                        Actions.ActionProcess.GhiLog(ex, "Đóng hỗ trợ");

                        trans.Rollback();
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                    }
                }
                return ret;
            }
        }
    }
}
