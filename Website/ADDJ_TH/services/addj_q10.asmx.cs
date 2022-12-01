using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Website.ADDJ_TH.entity;
using Website.HTHTKT;

namespace Website.ADDJ_TH.services
{
    /// <summary>
    /// Summary description for addj
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class addj_q10 : System.Web.Services.WebService
    {

        // hồ sơ chỉnh lý
        [WebMethod]
        public string themHoSoChinhLy(string hopso, string hoso_so, string tieudehoso, string trichyeunoidung, string soto,
            string thoigian, string nam, string thoihanbaoquan, string ghichu, string bosung1, string bosung2,
            string sokyhieuvb, string phongdoi, string mlhs, string mst, string domatkhan, string matokhai,
            string diachi, string sogiaycn, string stt, string url, string trangthai_muontra, string nguoilap_hs,
            string tuychon1, string tuychon2, string tuychon3, string madulieu, string mavanban, string mahop,
            string mahoso, string sohoso_tam, string tuychon4, string tuychon5, string gia, string day,
            string khoang, string tang, string vitri)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {

                    hopso = hopso == "null" ? "" : hopso;
                    hoso_so = hoso_so == "null" ? "" : hoso_so;
                    tieudehoso = tieudehoso == "null" ? "" : tieudehoso;
                    trichyeunoidung = trichyeunoidung == "null" ? "" : trichyeunoidung;
                    soto = soto == "null" ? "" : soto;
                    thoigian = thoigian == "null" ? "" : thoigian;
                    nam = nam == "null" ? "" : nam;
                    thoihanbaoquan = thoihanbaoquan == "null" ? "" : thoihanbaoquan;
                    ghichu = ghichu == "null" ? "" : ghichu;
                    bosung1 = bosung1 == "null" ? "" : bosung1;
                    bosung2 = bosung2 == "null" ? "" : bosung2;
                    sokyhieuvb = sokyhieuvb == "null" ? "" : sokyhieuvb;
                    phongdoi = phongdoi == "null" ? "" : phongdoi;
                    mlhs = mlhs == "null" ? "" : mlhs;
                    mst = mst == "null" ? "" : mst;
                    domatkhan = domatkhan == "null" ? "" : domatkhan;
                    matokhai = matokhai == "null" ? "" : matokhai;
                    diachi = diachi == "null" ? "" : diachi;
                    sogiaycn = sogiaycn == "null" ? "" : sogiaycn;
                    stt = stt == "null" ? "" : stt;
                    url = url == "null" ? "" : url;
                    trangthai_muontra = trangthai_muontra == "null" ? "" : trangthai_muontra;
                    nguoilap_hs = nguoilap_hs == "null" ? "" : nguoilap_hs;
                    tuychon1 = tuychon1 == "null" ? "" : tuychon1;
                    tuychon2 = tuychon2 == "null" ? "" : tuychon2;
                    tuychon3 = tuychon3 == "null" ? "" : tuychon3;
                    madulieu = madulieu == "null" ? "" : madulieu;
                    mavanban = mavanban == "null" ? "" : mavanban;
                    mahop = mahop == "null" ? "" : mahop;
                    mahoso = mahoso == "null" ? "" : mahoso;
                    sohoso_tam = sohoso_tam == "null" ? "" : sohoso_tam;
                    tuychon4 = tuychon4 == "null" ? "" : tuychon4;
                    tuychon5 = tuychon5 == "null" ? "" : tuychon5;
                    gia = gia == "null" ? "" : gia;
                    day = day == "null" ? "" : day;
                    khoang = khoang == "null" ? "" : khoang;
                    tang = tang == "null" ? "" : tang;
                    vitri = vitri == "null" ? "" : vitri;


                    var full_search = hopso + " " + hoso_so + " " + tieudehoso + " " + trichyeunoidung + " " + soto + " " + thoigian + " " + nam + " " + thoihanbaoquan + " " +
                                                   ghichu + " " + bosung1 + " " + bosung2 + " " + sokyhieuvb + " " + phongdoi + " " + mlhs + " " + mst + " " + domatkhan + " " +
                                                   matokhai + " " + diachi + " " + sogiaycn + " " + stt + " " + nguoilap_hs + " " + tuychon1 + " " + tuychon2 + " " +
                                                   tuychon3 + " " + tuychon4 + " " + tuychon5;

                    var trang_thai = 0;
                    if (trangthai_muontra.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"INSERT INTO mucluchoso_mlvb_53 (guid, hopso, hoso_so, trichyeunoidung, trichyeunoidung_mlhs, soto, thoigian, nam, thoihanbaoquan, ghichu, bosung1, bosung2, sokyhieuvb, phongdoi, mlhs, mst, domatkhan, matokhai, diachi, sogiaycn, stt, url, trangthai_muontra, nguoilap_hs, tuychon1, tuychon2, tuychon3, madulieu, mavanban, mahop, mahoso, sohoso_tam, mahoso_tam, full_search, tuychon4, tuychon5, gia, day, khoang, tang, vitri)
                                                    VALUES (NEWID(), N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', N'{8}', N'{9}', N'{10}', N'{11}', N'{12}', N'{13}', N'{14}', N'{15}', N'{16}', N'{17}', N'{18}', N'{19}', N'{20}', N'{21}', N'{22}', N'{23}', N'{24}', N'{25}', N'{26}', N'{27}', N'{28}', N'{29}', N'{30}', N'{31}', N'{32}', N'{33}', N'{34}', N'{35}', N'{36}', N'{37}', N'{38}', N'{39}'); SELECT mf.guid FROM mucluchoso mf WHERE mf.id=(SELECT SCOPE_IDENTITY()); ",
                                                    hopso, hoso_so, tieudehoso, trichyeunoidung, soto, thoigian, nam, thoihanbaoquan, ghichu, bosung1, bosung2,
                                                    sokyhieuvb, phongdoi, mlhs, mst, domatkhan, matokhai,
                                                    diachi, sogiaycn, stt, "", "", nguoilap_hs,
                                                    tuychon1, tuychon2, tuychon3, "", "", "",
                                                    "", "", "", full_search, tuychon4, tuychon5, "", "",
                                                    "", "", "");
                    var rest = ctx.Database.SqlQuery<string>(strSql).ToList();
                    var guid_mlhs = rest.FirstOrDefault();
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject("1|" + guid_mlhs);
                }
            }
            catch (Exception ex)
            {
                //Actions.ActionProcess.GhiLog(ex, "Thêm mục mức độ sự cố");
                ret = Newtonsoft.Json.JsonConvert.SerializeObject("0|0");
            }
            return ret;
        }

        [WebMethod]
        public string suaHoSoChinhLy(string guid, string hopso, string hoso_so, string tieudehoso, string trichyeunoidung, string soto,
            string thoigian, string nam, string thoihanbaoquan, string ghichu, string bosung1, string bosung2,
            string sokyhieuvb, string phongdoi, string mlhs, string mst, string domatkhan, string matokhai,
            string diachi, string sogiaycn, string stt, string url, string trangthai_muontra, string nguoilap_hs,
            string tuychon1, string tuychon2, string tuychon3, string madulieu, string mavanban, string mahop,
            string mahoso, string sohoso_tam, string tuychon4, string tuychon5, string gia, string day,
            string khoang, string tang, string vitri)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    hopso = hopso == "null" ? "" : hopso;
                    hoso_so = hoso_so == "null" ? "" : hoso_so;
                    tieudehoso = tieudehoso == "null" ? "" : tieudehoso;
                    trichyeunoidung = trichyeunoidung == "null" ? "" : trichyeunoidung;
                    soto = soto == "null" ? "" : soto;
                    thoigian = thoigian == "null" ? "" : thoigian;
                    nam = nam == "null" ? "" : nam;
                    thoihanbaoquan = thoihanbaoquan == "null" ? "" : thoihanbaoquan;
                    ghichu = ghichu == "null" ? "" : ghichu;
                    bosung1 = bosung1 == "null" ? "" : bosung1;
                    bosung2 = bosung2 == "null" ? "" : bosung2;
                    sokyhieuvb = sokyhieuvb == "null" ? "" : sokyhieuvb;
                    phongdoi = phongdoi == "null" ? "" : phongdoi;
                    mlhs = mlhs == "null" ? "" : mlhs;
                    mst = mst == "null" ? "" : mst;
                    domatkhan = domatkhan == "null" ? "" : domatkhan;
                    matokhai = matokhai == "null" ? "" : matokhai;
                    diachi = diachi == "null" ? "" : diachi;
                    sogiaycn = sogiaycn == "null" ? "" : sogiaycn;
                    stt = stt == "null" ? "" : stt;
                    url = url == "null" ? "" : url;
                    trangthai_muontra = trangthai_muontra == "null" ? "" : trangthai_muontra;
                    nguoilap_hs = nguoilap_hs == "null" ? "" : nguoilap_hs;
                    tuychon1 = tuychon1 == "null" ? "" : tuychon1;
                    tuychon2 = tuychon2 == "null" ? "" : tuychon2;
                    tuychon3 = tuychon3 == "null" ? "" : tuychon3;
                    madulieu = madulieu == "null" ? "" : madulieu;
                    mavanban = mavanban == "null" ? "" : mavanban;
                    mahop = mahop == "null" ? "" : mahop;
                    mahoso = mahoso == "null" ? "" : mahoso;
                    sohoso_tam = sohoso_tam == "null" ? "" : sohoso_tam;
                    tuychon4 = tuychon4 == "null" ? "" : tuychon4;
                    tuychon5 = tuychon5 == "null" ? "" : tuychon5;
                    gia = gia == "null" ? "" : gia;
                    day = day == "null" ? "" : day;
                    khoang = khoang == "null" ? "" : khoang;
                    tang = tang == "null" ? "" : tang;
                    vitri = vitri == "null" ? "" : vitri;


                    var full_search = hopso + " " + hoso_so + " " + tieudehoso + " " + trichyeunoidung + " " + soto + " " + thoigian + " " + nam + " " + thoihanbaoquan + " " +
                                                    ghichu + " " + bosung1 + " " + bosung2 + " " + sokyhieuvb + " " + phongdoi + " " + mlhs + " " + mst + " " + domatkhan + " " +
                                                    matokhai + " " + diachi + " " + sogiaycn + " " + stt + " " + nguoilap_hs + " " + tuychon1 + " " + tuychon2 + " " +
                                                    tuychon3 + " " + tuychon4 + " " + tuychon5;
                    var trang_thai = 0;
                    if (trangthai_muontra.ToLower() == "true")
                        trang_thai = 1;
                    string strSql = string.Format(@"UPDATE mucluchoso_mlvb_53
                                                    SET hopso = N'{0}'
                                                       ,hoso_so = N'{1}'
                                                       ,trichyeunoidung = N'{2}'
                                                       ,trichyeunoidung_mlhs = N'{3}'
                                                       ,soto = N'{4}'
                                                       ,thoigian = N'{5}'
                                                       ,nam = N'{6}'
                                                       ,thoihanbaoquan = N'{7}'
                                                       ,ghichu = N'{8}'
                                                       ,bosung1 = N'{9}'
                                                       ,bosung2 = N'{10}'
                                                       ,sokyhieuvb = N'{11}'
                                                       ,phongdoi = N'{12}'
                                                       ,mlhs = N'{13}'
                                                       ,mst = N'{14}'
                                                       ,domatkhan = N'{15}'
                                                       ,matokhai = N'{16}'
                                                       ,diachi = N'{17}'
                                                       ,sogiaycn = N'{18}'
                                                       ,stt = N'{19}'
                                                       ,url = N'{20}'
                                                       ,trangthai_muontra = N'{21}'
                                                       ,nguoilap_hs = N'{22}'
                                                       ,tuychon1 = N'{23}'
                                                       ,tuychon2 = N'{24}'
                                                       ,tuychon3 = N'{25}'
                                                       ,madulieu = N'{26}'
                                                       ,mavanban = N'{27}'
                                                       ,mahop = N'{28}'
                                                       ,mahoso = N'{29}'
                                                       ,sohoso_tam = N'{30}'
                                                       ,mahoso_tam = N'{31}'
                                                       ,full_search = N'{32}'
                                                       ,tuychon4 = N'{33}'
                                                       ,tuychon5 = N'{34}'
                                                       ,gia = N'{35}'
                                                       ,day = N'{36}'
                                                       ,khoang = N'{37}'
                                                       ,tang = N'{38}'
                                                       ,vitri = N'{39}'
                                                    WHERE guid='{40}'",
                                                    hopso, hoso_so, tieudehoso, trichyeunoidung, soto, thoigian, nam, thoihanbaoquan,
                                                    ghichu, bosung1, bosung2, sokyhieuvb, phongdoi, mlhs, mst, domatkhan,
                                                    matokhai, diachi, sogiaycn, stt, "", "", nguoilap_hs, tuychon1, tuychon2,
                                                    tuychon3, "", "", "", "", "", "", full_search, tuychon4, tuychon5,
                                                    "", "", "", "", "", guid);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                //Actions.ActionProcess.GhiLog(ex, "Sửa mục mức độ sự cố");
            }
            return ret;
        }

        [WebMethod]
        public string xoaHoSoChinhLy(string guid)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    string strSql = string.Format(@"DELETE dbo.mucluchoso WHERE guid='{0}' ", guid);
                    var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(1);
                }
            }
            catch (Exception ex)
            {
                //Actions.ActionProcess.GhiLog(ex, "Xóa mục nhóm quyền");
            }
            return ret;
        }
        [WebMethod]
        public string thongtinHoSoChinhLy(string guid)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0);
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<mucluchoso>();
                    string strSql = string.Format(@"SELECT a.id, a.guid, a.hopso, a.hoso_so, a.thoigian trichyeunoidung, trichyeunoidung_mlhs, a.soto, a.thoigian, a.nam, a.thoihanbaoquan, a.ghichu, a.bosung1, a.bosung2, a.sokyhieuvb, a.phongdoi, a.mlhs, a.mst, a.domatkhan, a.matokhai, a.diachi, a.sogiaycn, a.stt, a.url, a.trangthai_muontra, a.nguoilap_hs, a.tuychon1, a.tuychon2, a.tuychon3, a.madulieu, a.mavanban, a.mahop, a.mahoso, a.sohoso_tam, a.mahoso_tam, a.full_search, a.tuychon4, a.tuychon5, a.gia, a.day, a.khoang, a.tang, a.vitri	 
                                                    FROM mucluchoso_mlvb_53 a WHERE a.guid='{0}' ", guid);
                    var rt = ctx.Database.SqlQuery<mucluchoso>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                //Actions.ActionProcess.GhiLog(ex, "Thông tin mục mức độ sự cố");
            }
            return ret;
        }


        [WebMethod]
        public string danhSachFileDinhKem(string guid)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<mucluchoso_file>();
                    string strSql = string.Format(@"SELECT id
                                                  ,guid
                                                  ,tenfile
                                                  ,trangthai
                                                  ,ngaytao FROM mucluchoso_file WHERE guid='{0}'", guid);
                    var rt = ctx.Database.SqlQuery<mucluchoso_file>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                //Actions.ActionProcess.GhiLog(ex, "Thông tin mục mức độ sự cố");
            }
            return ret;
        }

        [WebMethod]
        public string thongTinChiTietXuLyDaPhuongTien(string guid)
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<mucluchoso>();
                    string strSql = string.Format(@"SELECT a.id, a.guid, a.hopso, a.hoso_so, a.thoigian trichyeunoidung, a.trichyeunoidung_mlhs, a.soto, a.thoigian, a.nam, a.thoihanbaoquan, a.ghichu, a.bosung1, a.bosung2, a.sokyhieuvb, a.phongdoi, a.mlhs, a.mst, a.domatkhan, a.matokhai, a.diachi, a.sogiaycn, a.stt, a.url, a.trangthai_muontra, a.nguoilap_hs, a.tuychon1, a.tuychon2, a.tuychon3, a.madulieu, a.mavanban, a.mahop, a.mahoso, a.sohoso_tam, a.mahoso_tam, a.full_search, a.tuychon4, a.tuychon5, a.gia, a.day, a.khoang, a.tang, a.vitri	 
                                                    FROM mucluchoso_mlvb_53 a WHERE a.guid='{0}' ", guid);
                    var rt = ctx.Database.SqlQuery<mucluchoso>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                // Actions.ActionProcess.GhiLog(ex, "Thông tin chi tiết xử lý đa phương tiện");
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
