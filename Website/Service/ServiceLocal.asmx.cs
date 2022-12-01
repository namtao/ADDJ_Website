using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using Website.AppCode;

namespace Website.Service
{
    /// <summary>
    /// Summary description for ServiceLocal
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceLocal : System.Web.Services.WebService
    {

        #region Province
        [WebMethod]
        public string GetTinh()
        {
            return ServiceLocalImpl.GetTinh();
        }

        [WebMethod]
        public string GetHuyen(int Tinh)
        {
            return ServiceLocalImpl.GetHuyen(Tinh);
        }

        [WebMethod]
        public string ViewXa(int Huyen)
        {
            return ServiceLocalImpl.ViewXa(Huyen);
        }


        [WebMethod]
        public int AddXa(int Huyen, string Xa)
        {
            return ServiceLocalImpl.AddXa(Huyen, Xa);
        }
        #endregion

        [WebMethod]
        public string GetPhongBanUser(string username)
        {
            return ServiceLocalImpl.GetPhongBanUser(username);
        }

        [WebMethod]
        public string GetFullKhieuNai(string SoThueBao, string MaKhieuNai)
        {
            return ServiceLocalImpl.GetFullKhieuNai(SoThueBao, MaKhieuNai);
        }

        [WebMethod]
        public string GetFullBuocXuLy(string MaKhieuNai)
        {
            return ServiceLocalImpl.GetFullBuocXuLy(MaKhieuNai);
        }

        [WebMethod]
        public string GetFullFileDinhKem(string MaKhieuNai)
        {
            return ServiceLocalImpl.GetFullFileDinhKem(MaKhieuNai);
        }

        [WebMethod]
        public string GetFullActivity(string MaKhieuNai)
        {
            return ServiceLocalImpl.GetFullActivity(MaKhieuNai);
        }

        [WebMethod]
        public string GetFullSoTien(string MaKhieuNai)
        {
            return ServiceLocalImpl.GetFullSoTien(MaKhieuNai);
        }

        [WebMethod]
        public string GetFullLog(string MaKhieuNai)
        {
            return ServiceLocalImpl.GetFullLog(MaKhieuNai);
        }

        [WebMethod]
        public string UpdateKhieuNaiToTraSau(string MaKhieuNai, int LoaiTaiKhoan)
        {
            return ServiceLocalImpl.UpdateKhieuNaiToTraSau(MaKhieuNai, LoaiTaiKhoan);
        }

        [WebMethod]
        public string DeleteKhieuNai(string MaKhieuNai)
        {
            return ServiceLocalImpl.DeleteKhieuNai(MaKhieuNai);
        }

        [WebMethod]
        public string UpdateKhieuNai(string table, string id, string field, string value, int type)
        {
            return ServiceLocalImpl.UpdateKhieuNai(table, id, field, value, type);
        }

        [WebMethod]
        public string DeleteTable(string table, string id)
        {
            return ServiceLocalImpl.DeleteTable(table, id);
        }

        [WebMethod]
        public int ExecuteNonQuery(string query)
        {
            return ServiceLocalImpl.ExecuteNonQuery(query);
        }

        [WebMethod]
        public DataTable ExecuteQuery(string query)
        {
            return ServiceLocalImpl.ExecuteQuery(query);
        }
    }
}
