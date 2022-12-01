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
    /// Summary description for ws_baoCao
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class ws_baoCao : System.Web.Services.WebService
    {
        /// <summary>
        /// Báo cáo tổng hợp số liệu yêu cầu hệ thống theo phòng ban
        /// </summary>
        /// <param name="tungay"></param>
        /// <param name="denngay"></param>
        /// <param name="idphongban"></param>
        /// <returns></returns>
        [WebMethod]
        public string bc_baoCaoTongHopSoLieu_YCHT_TheoPhongBan(string tungay, string denngay, string id_donvi)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var id_phongban = "";
                    if (id_donvi != null && id_donvi != "0")
                    {
                        id_phongban = string.Format(@"a.ID_DONVI={0} and ", id_donvi);
                    }

                    var strSQl2 = string.Format(@"SELECT a.Id, a.MAHOTRO,a.NOIDUNG_YEUCAU, a.NGAYTAO, 
                                              case  a.TRANGTHAI 
                                              when 0 then N'Khởi tạo và chuyển tiếp'
                                              when 1 then N'Chuyển tiếp...'
                                              when 2 then N'Chuyển phản hồi...'
                                              when 3 then N'Chờ xác nhận...'
                                              when 4 then N'Xử lý xong'
                                              ELSE '' END TRANGTHAI,   
                                              a.NOIDUNG_XL_DONG_HOTRO,
                                              b.TEN_LUONG, b.MOTA,b.SOBUOC,c.TENHETHONG,d.LOAI,d.LINHVUCCHUNG,d.LINHVUCCON
                                              FROM HT_DM_YEUCAU_HOTRO_HT a
                                              LEFT JOIN HT_LUONG_HOTRO b ON b.ID=a.ID_LUONG_HOTRO
                                              LEFT JOIN HT_DM_HETHONG_YCHT c ON c.ID=b.ID_HETHONG_YCHT
                                              LEFT JOIN HT_CAYTHUMUC_YCHT d on d.ID=a.ID_CAYTHUMUC_YCHT
                                              WHERE {0} CONVERT(varchar(11),a.NGAYTAO,103) BETWEEN '{1}' AND '{2}'
                                              ORDER BY a.NGAYTAO DESC",
                                                  id_phongban, tungay, denngay);

                    var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO5>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }

        /// <summary>
        /// Báo cáo tổng hợp số liệu yêu cầu hệ thống theo người dùng
        /// </summary>
        /// <param name="tungay"></param>
        /// <param name="denngay"></param>
        /// <param name="iddonvi"></param>
        /// <param name="idphongban"></param>
        /// <param name="arr_nguoidung"></param>
        /// <returns></returns>
        [WebMethod]
        public string bc_baoCaoTongHopSoLieu_YCHT_TheoNguoiDung(string tungay, string denngay, string id_donvi, string arr_nguoidung)
        {
            string ret = "";
            string cmdWhere = "";
            try
            {
                string lstAllChild = string.Format(@";WITH cte AS
                                                 (
	                                                SELECT *
	                                                FROM dbo.DoiTac WHERE Id={0}
	                                                UNION ALL
	                                                SELECT t2.*
	                                                FROM cte t1
	                                                JOIN dbo.DoiTac t2 ON t1.Id = t2.DonViTrucThuoc
                                                 ) ", id_donvi);
                if (arr_nguoidung == "")
                {
                    cmdWhere = string.Format(@" a.ID_DONVI in (select id from cte) and ");
                }
                else
                {
                    var lstUser = arr_nguoidung.Split(',');
                    var lstUsername = string.Join(",", lstUser.Select(item => "'" + item + "'"));
                    cmdWhere = string.Format(@" a.ID_DONVI={0} AND a.NGUOITAO IN({1}) and ", id_donvi, lstUsername);
                }
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(lstAllChild + @" SELECT a.Id, a.MAHOTRO,a.NOIDUNG_YEUCAU, a.NGAYTAO, 
                                              case  a.TRANGTHAI 
                                              when 0 then N'Khởi tạo và chuyển tiếp'
                                              when 1 then N'Chuyển tiếp...'
                                              when 2 then N'Chuyển phản hồi...'
                                              when 3 then N'Chờ xác nhận...'
                                              when 4 then N'Xử lý xong'
                                              ELSE '' END TRANGTHAI,   
                                              a.NOIDUNG_XL_DONG_HOTRO,
                                              b.TEN_LUONG, b.MOTA,b.SOBUOC,c.TENHETHONG,d.LOAI,d.LINHVUCCHUNG,d.LINHVUCCON
                                              FROM HT_DM_YEUCAU_HOTRO_HT a
                                              LEFT JOIN HT_LUONG_HOTRO b ON b.ID=a.ID_LUONG_HOTRO
                                              LEFT JOIN HT_DM_HETHONG_YCHT c ON c.ID=b.ID_HETHONG_YCHT
                                              LEFT JOIN HT_CAYTHUMUC_YCHT d on d.ID=a.ID_CAYTHUMUC_YCHT 
                                              WHERE {0} CONVERT(varchar(11),a.NGAYTAO,103) BETWEEN '{1}' AND '{2}'
                                              ORDER BY a.NGAYTAO DESC",
                                                  cmdWhere, tungay, denngay);
                    var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO5>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }


        [WebMethod]
        public string bc_traCuuThongTinTongHop(string sothuebao, string hethong, string linhvuc, string mucdosuco, string tungay, string denngay, string iddonvi, string nguoidung, string mayeucau)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@"SELECT
                        a.ID, 
                        f.TENDONVI, 
                        f.MADONVI,
                        a.SODIENTHOAI,
                        a.MA_YEUCAU,
                        a.NOIDUNG_YEUCAU,
                        a.NGAYTAO,
                        a.NGUOITAO,
                        g.TenDayDu TENDAYDU,
                        b.TENHETHONG,
                        b.MA_HETHONG,
                        c.LINHVUC,
                        c.MA_LINHVUC,
                        d.TENMUCDO,
                        e.TEN_LUONG,
                        kk.TENDONVI DONVIPHOIHOP
                        FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                        LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                        LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                        LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                        LEFT JOIN HT_NGUOIDUNG g ON g.Id = a.ID_NGUOITAO
                        CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                     INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                     WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                        WHERE 1=1 ");
                    var strWhere = "";
                    if (sothuebao != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.SODIENTHOAI={0}", sothuebao);
                    }
                    if (hethong != "null")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_HETHONG_YCHT={0}", hethong);
                    }
                    if (linhvuc != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_CAYTHUMUC_YCHT={0}", linhvuc);
                    }
                    if (mucdosuco != "" && mucdosuco != "0")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_MUCDO_SUCO={0}", mucdosuco);
                    }
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  a.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    if (iddonvi != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_DONVI={0}", iddonvi);
                    }
                    if (nguoidung != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.NGUOITAO='{0}'", nguoidung);
                    }
                    if (mayeucau != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.MA_YEUCAU LIKE N'%{0}%'", mayeucau);
                    }
                    strSQl2 = strSQl2 + strWhere;

                    var rt = ctx.Database.SqlQuery<ViewThongTinTraCuu>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }

        [WebMethod]
        public string bc_traCuuThongTinTongHop2(string StartIndex, string PageSize, string SortingOrder, string sothuebao, string hethong, string linhvuc, string mucdosuco, string tungay, string denngay, string iddonvi, string nguoidung, string mayeucau)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@"SELECT
                        a.ID, 
                        f.TENDONVI, 
                        f.MADONVI,
                        a.SODIENTHOAI,
                        a.MA_YEUCAU,
                        a.NOIDUNG_YEUCAU,
                        a.NGAYTAO,
                        a.NGUOITAO,
                        g.TenDayDu TENDAYDU,
                        b.TENHETHONG,
                        b.MA_HETHONG,
                        c.LINHVUC,
                        c.MA_LINHVUC,
                        d.TENMUCDO,
                        e.TEN_LUONG,
                        kk.TENDONVI DONVIPHOIHOP
                        FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                        LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                        LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                        LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                        LEFT JOIN HT_NGUOIDUNG g ON g.Id = a.ID_NGUOITAO
                        CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                     INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                     WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                        WHERE 1=1 ");
                    var strWhere = "";
                    if (sothuebao != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.SODIENTHOAI={0}", sothuebao);
                    }
                    if (hethong != "null")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_HETHONG_YCHT={0}", hethong);
                    }
                    if (linhvuc != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_CAYTHUMUC_YCHT={0}", linhvuc);
                    }
                    if (mucdosuco != "" && mucdosuco != "0")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_MUCDO_SUCO={0}", mucdosuco);
                    }
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  a.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    if (iddonvi != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_DONVI={0}", iddonvi);
                    }
                    if (nguoidung != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.NGUOITAO='{0}'", nguoidung);
                    }
                    if (mayeucau != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.MA_YEUCAU LIKE N'%{0}%'", mayeucau);
                    }
                    strSQl2 = strSQl2 + strWhere;


                    var strSQl = string.Format(@";with query as
                                    (
                                     SELECT
                                    a.ID, 
                                    f.TENDONVI, 
                                    f.MADONVI,
                                    a.SODIENTHOAI,
                                    a.MA_YEUCAU,
                                    a.NOIDUNG_YEUCAU,
                                    a.NGAYTAO,
                                    a.NGUOITAO,
                                    g.TenDayDu TENDAYDU,
                                    b.TENHETHONG,
                                    b.MA_HETHONG,
                                    c.LINHVUC,
                                    c.MA_LINHVUC,
                                    d.TENMUCDO,
                                    e.TEN_LUONG,ROW_NUMBER() OVER(ORDER BY a.ID ASC) as CountData 
                                    ,kk.TENDONVI DONVIPHOIHOP
                                    FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                                    LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                                    LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                                    LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                                    LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                                    LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                                    LEFT JOIN HT_NGUOIDUNG g ON g.Id = a.ID_NGUOITAO
                                    CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                                 INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                                 WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                                    WHERE 1=1 {7} ) 
                                    --order by clause is required to use offset-fetch
                                    select query.ID,
                                    query.TENDONVI,
                                    query.MADONVI,
                                    query.SODIENTHOAI,
                                    query.MA_YEUCAU,
                                    query.NOIDUNG_YEUCAU,
                                    query.NGAYTAO,
                                    query.NGUOITAO,
                                    query.TENDAYDU,
                                    query.TENHETHONG,
                                    query.MA_HETHONG,
                                    query.LINHVUC,
                                    query.MA_LINHVUC,
                                    query.TENMUCDO,
                                    query.TEN_LUONG,
                                    query.DONVIPHOIHOP,
                                    convert(varchar(50),query.CountData) CountData, 
                                    CASE WHEN tCountOrders.CountOrders%{4}>0 THEN convert(varchar(50), tCountOrders.CountOrders/{5} +  1)
									ELSE convert(varchar(50), tCountOrders.CountOrders/{6} ) end NumberOfPage,
	                                tCountOrders.CountOrders ToltalRecords 
                                    from query CROSS JOIN (SELECT Count(*) AS CountOrders FROM query) AS tCountOrders
                                    order by query.ID 
                                    offset (({0} - 1) * {1}) rows
                                    fetch next {2} rows only", Convert.ToInt32(StartIndex) + 1, PageSize, PageSize, PageSize, PageSize, PageSize, PageSize, strWhere);



                    var rt = ctx.Database.SqlQuery<ViewThongTinTraCuu2>(strSQl);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }


        [WebMethod]
        public string bc_tonghopTheoNguoiDung(string sothuebao, string hethong, string linhvuc, string mucdosuco, string tungay, string denngay, string iddonvi, string nguoidung, string mayeucau)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@"SELECT
                        a.ID, 
                        f.TENDONVI, 
                        f.MADONVI,
                        a.MA_YEUCAU,
                        a.NOIDUNG_YEUCAU,
                        a.NGAYTAO,
                        b.TENHETHONG,
                        b.MA_HETHONG,
                        c.LINHVUC,
                        c.MA_LINHVUC,
                        d.TENMUCDO,
                        e.TEN_LUONG,
                        kk.TENDONVI DONVIPHOIHOP
                        FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                        LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                        LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                        LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                        CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                     INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                     WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                        WHERE 1=1 ");
                    var strWhere = "";
                    if (sothuebao != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.SODIENTHOAI={0}", sothuebao);
                    }
                    if (hethong != "null")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_HETHONG_YCHT={0}", hethong);
                    }
                    if (linhvuc != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_CAYTHUMUC_YCHT={0}", linhvuc);
                    }
                    if (mucdosuco != "" && mucdosuco != "0")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_MUCDO_SUCO={0}", mucdosuco);
                    }
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  a.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    if (iddonvi != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_DONVI={0}", iddonvi);
                    }
                    if (nguoidung != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.NGUOITAO='{0}'", nguoidung);
                    }
                    if (mayeucau != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.MA_YEUCAU LIKE N'%{0}%'", mayeucau);
                    }
                    strSQl2 = strSQl2 + strWhere;

                    var rt = ctx.Database.SqlQuery<ViewThongTinTraCuu>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }

        [WebMethod]
        public string bc_tonghopTheoHeThong(string sothuebao, string hethong, string linhvuc, string mucdosuco, string tungay, string denngay, string iddonvi, string nguoidung, string mayeucau)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@"SELECT
                        a.ID, 
                        f.TENDONVI, 
                        f.MADONVI,
                        a.MA_YEUCAU,
                        a.NOIDUNG_YEUCAU,
                        a.NGAYTAO,
                        b.TENHETHONG,
                        b.MA_HETHONG,
                        c.LINHVUC,
                        c.MA_LINHVUC,
                        d.TENMUCDO,
                        e.TEN_LUONG,
                        kk.TENDONVI DONVIPHOIHOP
                        FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                        LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                        LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                        LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                        CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                     INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                     WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                        WHERE 1=1 ");
                    var strWhere = "";
                    if (sothuebao != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.SODIENTHOAI={0}", sothuebao);
                    }
                    if (hethong != "null")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_HETHONG_YCHT={0}", hethong);
                    }
                    if (linhvuc != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_CAYTHUMUC_YCHT={0}", linhvuc);
                    }
                    if (mucdosuco != "" && mucdosuco != "0")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_MUCDO_SUCO={0}", mucdosuco);
                    }
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  a.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    if (iddonvi != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_DONVI={0}", iddonvi);
                    }
                    if (nguoidung != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.NGUOITAO='{0}'", nguoidung);
                    }
                    if (mayeucau != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.MA_YEUCAU LIKE N'%{0}%'", mayeucau);
                    }
                    strSQl2 = strSQl2 + strWhere;

                    var rt = ctx.Database.SqlQuery<ViewThongTinTraCuu>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }

        [WebMethod]
        public string bc_tonghopTheoThoiGianNhap(string sothuebao, string hethong, string linhvuc, string mucdosuco, string tungay, string denngay, string iddonvi, string nguoidung, string mayeucau)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@"SELECT
                        a.ID, 
                        f.TENDONVI, 
                        f.MADONVI,
                        a.MA_YEUCAU,
                        a.NOIDUNG_YEUCAU,
                        a.NGAYTAO,
                        b.TENHETHONG,
                        b.MA_HETHONG,
                        c.LINHVUC,
                        c.MA_LINHVUC,
                        d.TENMUCDO,
                        e.TEN_LUONG,
                        kk.TENDONVI DONVIPHOIHOP
                        FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                        LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                        LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                        LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                        CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                     INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                     WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                        WHERE 1=1 ");
                    var strWhere = "";
                    if (sothuebao != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.SODIENTHOAI={0}", sothuebao);
                    }
                    if (hethong != "null")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_HETHONG_YCHT={0}", hethong);
                    }
                    if (linhvuc != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_CAYTHUMUC_YCHT={0}", linhvuc);
                    }
                    if (mucdosuco != "" && mucdosuco != "0")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_MUCDO_SUCO={0}", mucdosuco);
                    }
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  a.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    if (iddonvi != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_DONVI={0}", iddonvi);
                    }
                    if (nguoidung != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.NGUOITAO='{0}'", nguoidung);
                    }
                    if (mayeucau != "")
                    {
                        strWhere = strWhere + string.Format(@" and a.MA_YEUCAU LIKE N'%{0}%'", mayeucau);
                    }
                    strSQl2 = strSQl2 + strWhere;

                    var rt = ctx.Database.SqlQuery<ViewThongTinTraCuu>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }


        [WebMethod]
        public string bc_chitietTheoTrangThai(string tungay, string denngay, string iddonvi)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strDV = string.Format(@";WITH cte AS
                                                (
	                                                SELECT *
	                                                FROM HT_DONVI WHERE ID={0}

	                                                UNION ALL

	                                                SELECT t2.*
	                                                FROM cte t1
	                                                JOIN HT_DONVI t2 ON t1.ID = t2.ID_CHA
                                                )  ", iddonvi);

                    var strSQl2 = string.Format(@"SELECT
                          a.ID
                         ,a.MA_YEUCAU
                         ,a.SODIENTHOAI
                         ,d.TENMUCDO
                         ,c.LINHVUC
                         ,c.MA_LINHVUC
                         ,a.NOIDUNG_YEUCAU
                         ,a.NOIDUNG_XL_DONG_HOTRO
                         ,e.TEN_LUONG
                         ,e.MOTA
                         ,e.SOBUOC
                         ,b.TENHETHONG
                         ,b.MA_HETHONG
                         ,a.TRANGTHAI
                         ,a.NGUOITAO
                         ,a.ID_DONVI DONVITAO
                         ,f.TENDONVI
                         ,f.MADONVI
                         ,a.NGAYTAO
                         ,kk.TENDONVI DONVIPHOIHOP
                        FROM HT_HTKTTT.dbo.HT_DM_YEUCAU_HOTRO_HT a
                        LEFT JOIN HT_DM_HETHONG_YCHT b ON b.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_HETHONG_YCHT
                        LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                        LEFT JOIN HT_LUONG_HOTRO e on e.ID=a.ID_LUONG_HOTRO
                        LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                        CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                     INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                     WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                        WHERE 1=1 ");
                    var strWhere = "";
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  a.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    if (iddonvi != "")
                    {
                        strWhere = strWhere + string.Format(@" AND a.ID_DONVI IN (SELECT ID FROM cte)");
                    }
                    strSQl2 = strDV + strSQl2 + strWhere;

                    var rt = ctx.Database.SqlQuery<ViewBaoCaoCTTheoTT>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }

        [WebMethod]
        public string bc_chitietTheoNguoiDung(string tungay, string denngay, string iddonvi)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@";WITH cte AS
                                                  (
      	                                            SELECT ID
      	                                            FROM dbo.HT_DONVI WHERE ID={0}      
      	                                            UNION ALL      
      	                                            SELECT t2.ID
      	                                            FROM cte t1
      	                                            JOIN dbo.HT_DONVI t2 ON t1.ID = t2.ID_CHA
                                                  )  
                                       SELECT a.ID_NGUOIDUNG,b.TenTruyCap,b.TenDayDu,
                                        0 SLTonKyTruoc,
                                        0 SLTiepNhan,
                                        0 SLDaXuLy,
                                        0 SLDaXuLyQuaHan,
                                        0 SLTonDong,
                                        0 SLTonDongQuaHan
                                      FROM HT_NGUOIDUNG_DONVI a
                                      LEFT JOIN HT_NGUOIDUNG b ON a.ID_NGUOIDUNG=b.Id 
                                      LEFT JOIN NguoiSuDung_Group c on c.Id=b.NhomNguoiDung
                                      LEFT JOIN HT_DONVI d ON d.ID=a.ID_DONVI
                                      LEFT JOIN HT_DM_YEUCAU_HOTRO_HT e on a.ID_NGUOIDUNG=e.ID_NGUOITAO
                                      where a.XOA=0
                                      and d.ID in(select * from cte) ", iddonvi);
                    var strWhere = "";
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  e.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    strSQl2 = strSQl2 + strWhere;
                    strSQl2 = strSQl2 + " group by a.ID_NGUOIDUNG,b.TenTruyCap,b.TenDayDu ";

                    var rt = ctx.Database.SqlQuery<ViewBaoCaoCTTheoND>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }

        [WebMethod]
        public string bc_chitietTheoPhongBan(string tungay, string denngay, string iddonvi)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@";WITH cte AS
                                                  (
      	                                            SELECT ID
      	                                            FROM dbo.HT_DONVI WHERE ID={0}      
      	                                            UNION ALL      
      	                                            SELECT t2.ID
      	                                            FROM cte t1
      	                                            JOIN dbo.HT_DONVI t2 ON t1.ID = t2.ID_CHA
                                                  )  
                                       SELECT a.ID_DONVI,d.MADONVI,d.TENDONVI,
                                        0 SLTonKyTruoc,
                                        0 SLTiepNhan,
                                        0 SLDaXuLy,
                                        0 SLDaXuLyQuaHan,
                                        0 SLTonDong,
                                        0 SLTonDongQuaHan
                                      FROM HT_NGUOIDUNG_DONVI a
                                      LEFT JOIN HT_NGUOIDUNG b ON a.ID_NGUOIDUNG=b.Id 
                                      LEFT JOIN NguoiSuDung_Group c on c.Id=b.NhomNguoiDung
                                      LEFT JOIN HT_DONVI d ON d.ID=a.ID_DONVI
                                      LEFT JOIN HT_DM_YEUCAU_HOTRO_HT e on a.ID_DONVI=e.ID_DONVI
                                      where a.XOA=0
                                      AND d.ID in(select * from cte)  ", iddonvi);
                    var strWhere = "";
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  e.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                   
                    strSQl2 = strSQl2 + strWhere;
                    strSQl2 = strSQl2 + " GROUP BY a.ID_DONVI,d.MADONVI,d.TENDONVI  ";

                    var rt = ctx.Database.SqlQuery<ViewBaoCaoCTTheoPB>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }

        [WebMethod]
        public string bc_chitietTheoHeThong(string tungay, string denngay, string iddonvi)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@";WITH cte AS
                                                  (
      	                                            SELECT ID
      	                                            FROM dbo.HT_DONVI WHERE ID={0}      
      	                                            UNION ALL      
      	                                            SELECT t2.ID
      	                                            FROM cte t1
      	                                            JOIN dbo.HT_DONVI t2 ON t1.ID = t2.ID_CHA
                                                  )  
                                       SELECT a.ID_DONVI,d.MADONVI,d.TENDONVI,f.TENHETHONG,
                                        0 SLTonKyTruoc,
                                        0 SLTiepNhan,
                                        0 SLDaXuLy,
                                        0 SLDaXuLyQuaHan,
                                        0 SLTonDong,
                                        0 SLTonDongQuaHan
                                      FROM HT_NGUOIDUNG_DONVI a
                                      LEFT JOIN HT_NGUOIDUNG b ON a.ID_NGUOIDUNG=b.Id 
                                      LEFT JOIN NguoiSuDung_Group c on c.Id=b.NhomNguoiDung
                                      LEFT JOIN HT_DONVI d ON d.ID=a.ID_DONVI
                                      LEFT JOIN HT_DM_YEUCAU_HOTRO_HT e ON e.ID_DONVI=a.ID_DONVI
                                      LEFT JOIN HT_DM_HETHONG_YCHT f ON f.ID=e.ID_HETHONG_YCHT
                                      where a.XOA=0
                                      AND d.ID in(select * from cte) ", iddonvi);
                    var strWhere = "";
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  f.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }
                    
                    strSQl2 = strSQl2 + strWhere;
                    strSQl2 = strSQl2 + "   GROUP BY a.ID_DONVI,d.MADONVI,d.TENDONVI,f.TENHETHONG ";

                    var rt = ctx.Database.SqlQuery<ViewBaoCaoCTTheoHT>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }


        [WebMethod]
        public string bc_chitietCaNhan(string tungay, string denngay, string idnguoidung, string tentruycap)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSQl2 = string.Format(@"SELECT
                                                  a.ID
                                                 ,a.MA_YEUCAU
                                                 ,a.SODIENTHOAI
                                                 ,d.TENMUCDO
                                                 ,c.LINHVUC
                                                 ,c.MA_LINHVUC
                                                 ,a.NOIDUNG_YEUCAU
                                                 ,a.NOIDUNG_XL_DONG_HOTRO
                                                 ,b.TEN_LUONG
                                                 ,b.MOTA
                                                 ,b.SOBUOC
                                                 ,e.TENHETHONG
                                                 ,e.MA_HETHONG
                                                 ,e.TRANGTHAI
                                                 ,e.NGUOITAO
                                                 ,f.TENDONVI
                                                 ,f.MADONVI
                                                 ,a.NGAYTAO
                                                 ,kk.TENDONVI DONVIPHOIHOP
                                                  FROM HT_DM_YEUCAU_HOTRO_HT a
                                                 LEFT JOIN HT_LUONG_HOTRO b on b.ID=a.ID_LUONG_HOTRO
                                                 LEFT JOIN HT_CAYTHUMUC_YCHT c on c.ID=a.ID_CAYTHUMUC_YCHT
                                                 LEFT JOIN HT_MUCDO_SUCO d ON d.ID=a.ID_MUCDO_SUCO
                                                 LEFT JOIN HT_DM_HETHONG_YCHT e ON e.ID=a.ID_HETHONG_YCHT
                                                 LEFT JOIN HT_DONVI f ON f.ID=a.ID_DONVI
                                                 CROSS APPLY (SELECT top 1 p.* FROM HT_XULY_YEUCAU_HOTRO k 
                                                         INNER JOIN HT_DONVI p on p.ID=k.ID_DONVI_TO
                                                         WHERE k.ID_YEUCAU_HOTRO_HT=a.ID ORDER BY k.ID DESC) kk
                                                 WHERE a.NGUOITAO='{0}'
                                                  OR a.ID IN (SELECT ID_YEUCAU_HOTRO_HT FROM HT_XULY_YEUCAU_HOTRO WHERE ID_NGUOITAO={1}) 
                                                  ", tentruycap, idnguoidung);
                    var strWhere = "";
                    if (tungay != "" && denngay != "")
                    {
                        var tungay_ = tungay.Split('/');
                        var tungay__ = tungay_[2] + "-" + tungay_[1] + "-" + tungay_[0];

                        var denngay_ = denngay.Split('/');
                        var denngay__ = denngay_[2] + "-" + denngay_[1] + "-" + denngay_[0];
                        strWhere = strWhere + string.Format(@" AND  a.NGAYTAO BETWEEN  CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1} 23:59:59:999')", tungay__, denngay__);
                    }

                    strSQl2 = strSQl2 + strWhere;

                    var rt = ctx.Database.SqlQuery<ViewBaoCaoCaNhan>(strSQl2);
                    var lst = rt.ToList();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "BC");
            }
            return ret;
        }



        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
