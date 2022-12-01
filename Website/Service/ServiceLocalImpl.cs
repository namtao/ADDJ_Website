using AIVietNam.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Website.AppCode;

namespace Website.Service
{
    public class ServiceLocalImpl
    {
        #region Province
        public static string GetTinh()
        {
            try
            {
                string strWhere = "LevelNbr = 1";
                var lstItem = ServiceFactory.GetInstanceProvince().GetListDynamic("", strWhere, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lstItem);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        public static string GetHuyen(int Tinh)
        {
            try
            {
                string strWhere = "LevelNbr = 2 And ParentId=" + Tinh;
                var lstItem = ServiceFactory.GetInstanceProvince().GetListDynamic("", strWhere, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lstItem);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        public static string ViewXa(int Huyen)
        {
            try
            {
                string strWhere = "LevelNbr = 3 And ParentId=" + Huyen;
                var lstItem = ServiceFactory.GetInstanceProvince().GetListDynamic("", strWhere, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lstItem);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        public static int AddXa(int Huyen, string xa)
        {
            try
            {                
                var strInsert = string.Format("insert into Province(ParentId, Name, LevelNbr, AbbRev) values(@ParentId, @Name, 3, '')", Huyen, xa);

                SqlParameter[] sqlParam = {                                           
		                                    new SqlParameter("ParentId", Huyen),
                                            new SqlParameter("Name", xa)
                                      };

                var result = ServiceFactory.GetInstanceProvince().ExecuteNonQuery(strInsert, System.Data.CommandType.Text, sqlParam);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        public static DataTable ExecuteQuery(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.Text;

                var ds = ServiceFactory.GetInstanceKhieuNai().ExecuteQueryToDataSet(cmd);
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int ExecuteNonQuery(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.Text;


                var result = ServiceFactory.GetInstanceKhieuNai().ExecuteNonQuery(cmd);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string GetPhongBanUser(string username)
        {
            try
            {
                string strWhere = "id in (select PhongBanId from PhongBan_User where NguoiSuDungId = (select id from NguoiSuDung where TenTruyCap='" + username + "'))";
                var lstItem = ServiceFactory.GetInstancePhongBan().GetListDynamic("", strWhere, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lstItem);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        
        public static string GetFullKhieuNai(string SoThueBao, string MaKhieuNai)
        {
            try
            {
                if (SoThueBao.Equals("") && MaKhieuNai.Equals(""))
                    return "Trống hết thì chết.";

                string strQuery = string.Empty;
                if (SoThueBao.Length > 0)
                    strQuery += "SoThueBao = " + SoThueBao;
                if (MaKhieuNai.Length > 0)
                {
                    if (strQuery.Length > 0)
                        strQuery += " AND Id= " + MaKhieuNai;
                    else
                        strQuery += " Id= " + MaKhieuNai;
                }

                var lst = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("", strQuery, "");

                foreach (var item in lst)
                {
                    item.PhongBan_Name = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        public static string GetFullBuocXuLy(string MaKhieuNai)
        {
            try
            {
                string strQuery = "KhieuNaiId = " + MaKhieuNai;

                var lst = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic("", strQuery, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        public static string GetFullFileDinhKem(string MaKhieuNai)
        {
            try
            {
                string strQuery = "KhieuNaiId = " + MaKhieuNai;

                var lst = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetListDynamic("", strQuery, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        
        public static string GetFullActivity(string MaKhieuNai)
        {
            try
            {
                string strQuery = "KhieuNaiId = " + MaKhieuNai;

                var lst = ServiceFactory.GetInstanceKhieuNai_Activity().GetListDynamic("", strQuery, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        
        public static string GetFullSoTien(string MaKhieuNai)
        {
            try
            {
                string strQuery = "KhieuNaiId = " + MaKhieuNai;

                var lst = ServiceFactory.GetInstanceKhieuNai_SoTien().GetListDynamic("", strQuery, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        public static string GetFullLog(string MaKhieuNai)
        {
            try
            {
                string strQuery = "KhieuNaiId = " + MaKhieuNai;

                var lst = ServiceFactory.GetInstanceKhieuNai_Log().GetListDynamic("", strQuery, "");

                return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
        }

        
        public static string UpdateKhieuNaiToTraSau(string MaKhieuNai, int LoaiTaiKhoan)
        {
            if (string.IsNullOrEmpty(MaKhieuNai))
                return string.Empty;

            var lst = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("Id", "Id=" + MaKhieuNai, "");

            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    try
                    {
                        //if (item.IsTraSau)
                        //    return "Trả sau rồi mà. Update gì nữa";

                        string strUpdate = "IsTraSau=1";

                        ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(strUpdate, "Id=" + item.Id);


                        string urlUpdateSolr = string.Format("{0}{1}/update?stream.body=<add><doc><field name='Id'>{2}</field><field name='IsTraSau' update='set'>1</field>", Config.ServerSolr, "GQKN", item.Id);
                        if (LoaiTaiKhoan > 0)
                        {
                            var lstSoTienGroup = ServiceFactory.GetInstanceKhieuNai_SoTien().GetListDynamic("KhieuNaiId,LoaiTien, SUM(SoTien) SoTien,SUM(SoTien_Edit) SoTien_Edit", "KhieuNaiId=" + item.Id + " group by KhieuNaiId,LoaiTien", "");
                            string fieldUpdate = string.Empty;
                            string fieldUpdateTraSau = string.Empty;
                            switch (LoaiTaiKhoan)
                            {
                                case 10:
                                    fieldUpdateTraSau = "SoTienKhauTru_TS_GPRS";
                                    break;
                                case 11:
                                    fieldUpdateTraSau = "SoTienKhauTru_TS_CP";
                                    break;
                                case 12:
                                    fieldUpdateTraSau = "SoTienKhauTru_TS_Thoai";
                                    break;
                                case 13:
                                    fieldUpdateTraSau = "SoTienKhauTru_TS_SMS";
                                    break;
                                case 14:
                                    fieldUpdateTraSau = "SoTienKhauTru_TS_IR";
                                    break;
                                case 15:
                                    fieldUpdateTraSau = "SoTienKhauTru_TS_Khac";
                                    break;
                                default:
                                    fieldUpdateTraSau = "SoTienKhauTru_TS_Khac";
                                    break;
                            }

                            foreach (var itemTien in lstSoTienGroup)
                            {
                                switch (itemTien.LoaiTien)
                                {
                                    case 1:
                                        fieldUpdate = "SoTienKhauTru_TKC";
                                        break;
                                    case 2:
                                        fieldUpdate = "SoTienKhauTru_KM";
                                        break;
                                    case 3:
                                        fieldUpdate = "SoTienKhauTru_KM1";
                                        break;
                                    case 4:
                                        fieldUpdate = "SoTienKhauTru_KM2";
                                        break;
                                    case 5:
                                        fieldUpdate = "SoTienKhauTru_Data";
                                        break;
                                    case 6:
                                        fieldUpdate = "SoTienKhauTru_Khac";
                                        break;
                                    case 10:
                                        fieldUpdate = "SoTienKhauTru_TS_GPRS";
                                        break;
                                    case 11:
                                        fieldUpdate = "SoTienKhauTru_TS_CP";
                                        break;
                                    case 12:
                                        fieldUpdate = "SoTienKhauTru_TS_Thoai";
                                        break;
                                    case 13:
                                        fieldUpdate = "SoTienKhauTru_TS_SMS";
                                        break;
                                    case 14:
                                        fieldUpdate = "SoTienKhauTru_TS_IR";
                                        break;
                                    case 15:
                                        fieldUpdate = "SoTienKhauTru_TS_Khac";
                                        break;
                                    default:
                                        fieldUpdate = "SoTienKhauTru_TS_Khac";
                                        break;
                                }
                                if (fieldUpdate.Equals(fieldUpdateTraSau))
                                    continue;

                                

                                urlUpdateSolr += string.Format("<field name='{0}' update='set'>0</field>", fieldUpdate);
                                if (itemTien.SoTien_Edit > 0)
                                {
                                    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(string.Format("{0}=0,{1}={2}", fieldUpdate, fieldUpdateTraSau, itemTien.SoTien_Edit), "Id=" + item.Id);
                                    urlUpdateSolr += string.Format("<field name='{0}' update='set'>{1}</field>", fieldUpdateTraSau, itemTien.SoTien_Edit);
                                }
                                else
                                {
                                    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(string.Format("{0}=0,{1}={2}", fieldUpdate, fieldUpdateTraSau, itemTien.SoTien), "Id=" + item.Id);
                                    urlUpdateSolr += string.Format("<field name='{0}' update='set'>{1}</field>", fieldUpdateTraSau, itemTien.SoTien);
                                }
                            }

                            ServiceFactory.GetInstanceKhieuNai_SoTien().UpdateDynamic("LoaiTien=" + LoaiTaiKhoan, "KhieuNaiId=" + item.Id);
                        }

                        urlUpdateSolr += "</doc></add>&commit=true";
                        var response = "GQKN :" + DownloadExpress.Download(urlUpdateSolr).StatusCode;

                        var lstSoTien = ServiceFactory.GetInstanceKhieuNai_SoTien().GetListDynamic("Id", "KhieuNaiId=" + item.Id, "");
                        foreach (var itemTien in lstSoTien)
                        {
                            urlUpdateSolr = string.Format("{0}{1}/update?stream.body=<add><doc><field name='Id'>{2}</field><field name='LoaiTien' update='set'>{3}</field>", Config.ServerSolr, "SoTien", itemTien.Id, LoaiTaiKhoan);
                            urlUpdateSolr += "</doc></add>&commit=true";
                            response += "<br />So tien :" + DownloadExpress.Download(urlUpdateSolr).StatusCode;
                        }

                        return response;
                    }
                    catch (Exception ex)
                    { }
                }
            }
            return string.Empty;
        }

        public static string DeleteKhieuNai(string MaKhieuNai)
        {
            if (string.IsNullOrEmpty(MaKhieuNai))
                return "Ko co ma thi xoa cai gi.";

            var urlDelSolr = string.Empty;
            var responseSolr = string.Empty;

            var lst = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("Id", "Id=" + MaKhieuNai, "");

            if (lst != null && lst.Count > 0)
            {

                foreach (var item in lst)
                {
                    var whereClause = "KhieuNaiId=" + item.Id;
                    var lstActivity = ServiceFactory.GetInstanceKhieuNai_Activity().GetListDynamic("Id", whereClause, "");
                    if (lstActivity != null && lstActivity.Count > 0)
                    {
                        foreach (var act in lstActivity)
                        {
                            urlDelSolr = string.Format("{0}{1}/update?stream.body=<delete><query>Id:{2}</query></delete>&commit=true", Config.ServerSolr, "Activity", act.Id);
                            responseSolr += "<br />Activity :" + DownloadExpress.Download(urlDelSolr).StatusCode;
                        }
                        ServiceFactory.GetInstanceKhieuNai_Activity().DeleteDynamic(whereClause);
                    }

                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().DeleteDynamic(whereClause);
                    ServiceFactory.GetInstanceKhieuNai_FileDinhKem().DeleteDynamic(whereClause);

                    var lstKQXL = ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy().GetListDynamic("Id", whereClause, "");
                    if (lstKQXL != null && lstKQXL.Count > 0)
                    {
                        foreach (var act in lstKQXL)
                        {
                            urlDelSolr = string.Format("{0}{1}/update?stream.body=<delete><query>Id:{2}</query></delete>&commit=true", Config.ServerSolr, "KetQuaXuLy", act.Id);
                            responseSolr += "<br />KQXL :" + DownloadExpress.Download(urlDelSolr).StatusCode;
                        }
                        ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy().DeleteDynamic(whereClause);
                    }

                    ServiceFactory.GetInstanceKhieuNai_Log().DeleteDynamic(whereClause);

                    var lstSoTien = ServiceFactory.GetInstanceKhieuNai_SoTien().GetListDynamic("Id", whereClause, "");
                    if (lstSoTien != null && lstSoTien.Count > 0)
                    {
                        foreach (var act in lstSoTien)
                        {
                            urlDelSolr = string.Format("{0}{1}/update?stream.body=<delete><query>Id:{2}</query></delete>&commit=true", Config.ServerSolr, "SoTien", act.Id);
                            responseSolr += "<br />SoTien :" + DownloadExpress.Download(urlDelSolr).StatusCode;
                        }
                        ServiceFactory.GetInstanceKhieuNai_SoTien().DeleteDynamic(whereClause);
                    }

                    urlDelSolr = string.Format("{0}{1}/update?stream.body=<delete><query>Id:{2}</query></delete>&commit=true", Config.ServerSolr, "GQKN", item.Id);
                    responseSolr += "<br />GQKN :" + DownloadExpress.Download(urlDelSolr).StatusCode;

                    ServiceFactory.GetInstanceKhieuNai().DeleteDynamic("Id=" + item.Id);
                }
            }

            return responseSolr;
        }

        public static string UpdateKhieuNai(string table, string id, string field, string value, int type)
        {
            var updateField = string.Empty;
            var response = string.Empty;
            var urlSolr = Config.ServerSolr;
            bool isUpdateSolr = true;
            switch(table)
            {
                case "KhieuNai":
                    urlSolr += "GQKN";
                    break;
                case "KhieuNai_SoTien":
                    urlSolr += "SoTien";
                    break;
                case "KhieuNai_BuocXuLy":
                    urlSolr += "BuocXuLy";
                    break;
                case "KhieuNai_Activity":
                    urlSolr += "Activity";
                    break;
                default:
                    isUpdateSolr = false;
                    break;
            }

            switch(type)
            { 
                case 1:
                    updateField = string.Format("Update {0} set {1} = {2} where Id={3}", table, field, value, id);
                    break;
                case 2:
                    updateField = string.Format("Update {0} set {1} = N'{2}' where Id={3}", table, field, value, id);
                    break;                
                default:
                    break;
            }

            response += "Db: " + ServiceFactory.GetInstanceGetData().ExecuteNonQueryNoStore(updateField);

            if (isUpdateSolr)
            {
                urlSolr += string.Format("/update?stream.body=<add><doc><field name='Id'>{0}</field><field name='{1}' update='set'>{2}</field></doc></add>&commit=true", id, field, value);
                response += "<br />Solr :" + DownloadExpress.Download(urlSolr).StatusCode;
            }

            return response;
        }

        public static string DeleteTable(string table, string id)
        {
            var deleteQuery = string.Empty;
            var response = string.Empty;
            var urlSolr = Config.ServerSolr;
            bool isUpdateSolr = true;
            switch (table)
            {
                case "KhieuNai":
                    urlSolr += "GQKN";
                    break;
                case "KhieuNai_SoTien":
                    urlSolr += "SoTien";
                    break;
                case "KhieuNai_BuocXuLy":
                    urlSolr += "BuocXuLy";
                    break;
                case "KhieuNai_Activity":
                    urlSolr += "Activity";
                    break;
                default:
                    isUpdateSolr = false;
                    break;
            }

            deleteQuery = string.Format("Delete {0} where Id={1}", table, id);

            response += "Db: " + ServiceFactory.GetInstanceGetData().ExecuteNonQueryNoStore(deleteQuery);

            if (isUpdateSolr)
            {
                urlSolr += string.Format("/update?stream.body=<delete><query>Id:{0}</query></delete>&commit=true", id);
                response += "<br />Solr :" + DownloadExpress.Download(urlSolr).StatusCode;
            }

            return response;
        }
    }
}