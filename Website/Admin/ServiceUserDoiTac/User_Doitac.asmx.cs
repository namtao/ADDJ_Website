using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using ADDJ.Admin;
using ADDJ.Core;
using ADDJ.Entity;
using ADDJ.Impl;
using Website.AppCode;

namespace Website.admin.ServiceUserDoiTac
{
    /// <summary>
    /// Summary description for User_Doitac
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class User_Doitac : System.Web.Services.WebService
    {

        private string _lstIps = System.Configuration.ConfigurationManager.AppSettings["lstIps"];
        private const string CONST_KEY = "88efd8a4941240668c92130d9ba5ca73";
        public bool CheckAuthen(string sKey)
        {
            bool msg = true;
            if (sKey != CONST_KEY)
                msg = false;

            string ip = HttpContext.Current.Request.UserHostAddress;
            //Utility.LogEvent(ip);
            if (!checkIp(ip))
                msg = false;
            return msg;

        }
        private bool checkIp(string requestIp)
        {

            var Result = false;
            if ((";" + _lstIps + ";").IndexOf(";" + requestIp.Trim() + ";") != -1)
                Result = true;
            return Result;
        }
        [WebMethod]
        public string InsertUpdateUser(string userName, string fullName, byte status, int doitacId, string codinh, string didong, string email, string address, string ngaysinh, byte sex, string LUser, string skey)
        {
            string ketqua = "Không thêm mới, cập nhật user";
            string ip = HttpContext.Current.Request.UserHostAddress;
            //Utility.LogEvent(ip + "-" + userName + "-" + fullName + "-" + status + "-" + doitacId);
            var kt_Ip_skey = CheckAuthen(skey);
            //NguoiSuDungImpl impl = new NguoiSuDungImpl();
            if (kt_Ip_skey)
            {
                try
                {
                    var kt = NguoiSuDungImpl.UpdateProfileCrossSell(userName, doitacId, fullName, codinh, didong, email, address, ngaysinh, sex, status, LUser);
                    if (kt)
                        ketqua = "Add Update admin complete.";
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    return ex.Message.ToString();
                }
            }
            return ketqua;
        }

        [WebMethod]
        public string InsertUpdateDoitac(int requestId, int Id, string MaDoiTac, string TenDoiTac, string DienThoai, string MaSoThue, string Fax, string DiaChi,
           string Website, string NguoiDaiDien, string ChucVu, string NgayCap, string NoiCap, string SoGiayTo, string SoHopDong, string NgayHopDong,
           string GhiChu, string Username, int DonViTrucThuoc, int ProvinceId, int ProvinceHuyenId, int LoaiGiayTo, int doitacId, string skey)
        {
            string ketqua = "Không thêm mới, cập nhật doitac";
            string ip = HttpContext.Current.Request.UserHostAddress;
            Utility.LogEvent(ip + "-" + requestId + "-" + DonViTrucThuoc + "-" + MaDoiTac + "-" + TenDoiTac);
            DoiTacInfo objDoiTac = new DoiTacInfo();
            DoiTacInfo item = new DoiTacInfo();
            var success = false;
            var kt_Ip_skey = CheckAuthen(skey);
            if (kt_Ip_skey)
            {
                try
                {
                    DoiTacImpl impl = new DoiTacImpl();
                    objDoiTac = impl.GetInfo(Id);
                    if (objDoiTac != null && objDoiTac.Id > 0)
                    {
                        item = ServiceFactory.GetInstanceDoiTac().GetInfo(ConvertUtility.ToInt32(objDoiTac.Id));
                        item.DonViTrucThuoc = DonViTrucThuoc;
                        item.ProvinceId = ProvinceId;
                        item.ProvinceHuyenId = ProvinceHuyenId;
                        item.LoaiGiayTo = LoaiGiayTo;

                        item.MaDoiTac = ConvertUtility.ToString(MaDoiTac);
                        item.TenDoiTac = ConvertUtility.ToString(TenDoiTac);

                        item.DienThoai = ConvertUtility.ToString(DienThoai);
                        item.MaSoThue = ConvertUtility.ToString(MaSoThue);
                        item.Fax = ConvertUtility.ToString(Fax);
                        item.DiaChi = ConvertUtility.ToString(DiaChi);
                        item.Website = ConvertUtility.ToString(Website);
                        item.NguoiDaiDien = ConvertUtility.ToString(NguoiDaiDien);
                        item.ChucVu = ConvertUtility.ToString(ChucVu);
                        item.NgayCap = ConvertUtility.ConvertToDateTime(NgayCap, out success);

                        item.NoiCap = ConvertUtility.ToString(NoiCap);
                        item.SoGiayTo = ConvertUtility.ToString(SoGiayTo);
                        item.SoHopDong = ConvertUtility.ToString(SoHopDong);
                        item.NgayHopDong = ConvertUtility.ConvertToDateTime(NgayHopDong, out success);
                        item.GhiChu = ConvertUtility.ToString(GhiChu);
                        item.LUser = Username;
                        item.Id = ServiceFactory.GetInstanceDoiTac().Update(item);
                    }
                    else
                    {
                        item.Id = Id;
                        item.DonViTrucThuoc = DonViTrucThuoc;
                        item.ProvinceId = ProvinceId;
                        item.ProvinceHuyenId = ProvinceHuyenId;
                        item.LoaiGiayTo = LoaiGiayTo;

                        item.MaDoiTac = ConvertUtility.ToString(MaDoiTac);
                        item.TenDoiTac = ConvertUtility.ToString(TenDoiTac);

                        item.DienThoai = ConvertUtility.ToString(DienThoai);
                        item.MaSoThue = ConvertUtility.ToString(MaSoThue);
                        item.Fax = ConvertUtility.ToString(Fax);
                        item.DiaChi = ConvertUtility.ToString(DiaChi);
                        item.Website = ConvertUtility.ToString(Website);
                        item.NguoiDaiDien = ConvertUtility.ToString(NguoiDaiDien);
                        item.ChucVu = ConvertUtility.ToString(ChucVu);
                        item.NgayCap = ConvertUtility.ConvertToDateTime(NgayCap, out success);

                        item.NoiCap = ConvertUtility.ToString(NoiCap);
                        item.SoGiayTo = ConvertUtility.ToString(SoGiayTo);
                        item.SoHopDong = ConvertUtility.ToString(SoHopDong);
                        item.NgayHopDong = ConvertUtility.ConvertToDateTime(NgayHopDong, out success);
                        item.GhiChu = ConvertUtility.ToString(GhiChu);
                        item.LUser = Username;
                        item.Id = ServiceFactory.GetInstanceDoiTac().add_ccos(item);
                    }

                    if (item.Id > 0)
                        ketqua = "Add Update doitac complete.";
                }
                catch (Exception ex) { Utility.LogEvent(ex); }
            }
            return ketqua;
        }
        [WebMethod]
        public string DeleteDoitac(int Id, string skey)
        {
            string ketqua = "Không xóa doitac";
            string ip = HttpContext.Current.Request.UserHostAddress;
            DoiTacInfo objDoiTac = new DoiTacInfo();
            DoiTacInfo item = new DoiTacInfo();
            var kt_Ip_skey = CheckAuthen(skey);
            if (kt_Ip_skey)
            {
                try
                {
                    DoiTacImpl impl = new DoiTacImpl();
                    objDoiTac = impl.GetInfo(Id);
                    if (objDoiTac != null && objDoiTac.Id > 0)
                    {
                        ServiceFactory.GetInstanceDoiTac().delete_ccos(Id).ToString();
                        ketqua = "Xóa DoiTac thành công";
                    }
                }
                catch (Exception ex) { Utility.LogEvent(ex); }
            }
            return ketqua;
        }
    }
}
