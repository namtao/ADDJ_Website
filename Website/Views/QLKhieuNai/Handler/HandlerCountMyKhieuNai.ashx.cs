using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for HandlerCountMyKhieuNai
    /// </summary>
    public class HandlerCountMyKhieuNai : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string strReturn = string.Empty;
            context.Response.ContentType = "text/plain";
            var type = context.Request.Form["key"] ?? context.Request.QueryString["key"];
            AdminInfo infoUser = LoginAdmin.AdminLogin();
            if (infoUser == null)
            {
                context.Response.Write(strReturn);
                return;
            }

            string typeSearch = context.Request.Form["typeSearch"] ?? context.Request.QueryString["typeSearch"];
            string doUuTien = context.Request.Form["doUuTien"] ?? context.Request.QueryString["doUuTien"];
            string trangThai = context.Request.Form["trangThai"] ?? context.Request.QueryString["trangThai"];
            string loaiKhieuNai = context.Request.Form["loaiKhieuNai"] ?? context.Request.QueryString["loaiKhieuNai"];
            string linhVucChung = context.Request.Form["linhVucChung"] ?? context.Request.QueryString["linhVucChung"];
            string linhVucCon = context.Request.Form["linhVucCon"] ?? context.Request.QueryString["linhVucCon"];
            string ListLinhVucConID = "";
            if (linhVucCon != "0" && linhVucCon != "-1")
            {
                var ListLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id", "Name=" + "N'" + linhVucCon + "'", "");
                for (int i = 0; i < ListLinhVucCon.Count; i++)
                {
                    if (i < ListLinhVucCon.Count - 1)
                    {
                        ListLinhVucConID = ListLinhVucConID + ListLinhVucCon[i].Id + ",";
                    }
                    else
                    {
                        ListLinhVucConID = ListLinhVucConID + ListLinhVucCon[i].Id;
                    }
                }
            }
            else
            {
                ListLinhVucConID = linhVucCon;
            }
            string phongBanXuLy = context.Request.Form["phongBanXuLy"] ?? context.Request.QueryString["phongBanXuLy"];

            //LONGLX
            string ShowNguoiXuLy = context.Request.Form["ShowNguoiXuLy"] ?? context.Request.QueryString["ShowNguoiXuLy"];

            string NguoiXuLy_Filter = "-1";
            bool IsTatCaKN = false;
            int KNHangLoat = 0;
            string PhongBanId = infoUser.PhongBanId.ToString();
            bool isPermission = false;

            if (ShowNguoiXuLy.Equals("1"))
            {
                NguoiXuLy_Filter = "";
            }

            switch (typeSearch)
            {

                case "1": //Cá nhân
                    KNHangLoat = -1;
                    NguoiXuLy_Filter = infoUser.Username;
                    PhongBanId = infoUser.PhongBanId.ToString();
                    break;
                case "2": //Hàng loạt
                    KNHangLoat = -1;
                    PhongBanId = infoUser.PhongBanId.ToString();
                    break;
                default: //Phòng ban
                    PhongBanId = infoUser.PhongBanId.ToString();

                    break;
            }

            //string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
            string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
            if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
            {
                contentSeach = string.Empty;
            }

            //string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
            string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
            int nSoThueBao = -1;
            if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
            {
                nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
            }

            //string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
            string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
            int NguoiTiepNhanId = -1;
            if (!string.IsNullOrEmpty(NguoiTiepNhan) && !NguoiTiepNhan.Equals("Người tiếp nhận..."))
            {
                NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTiepNhan);
                if (NguoiTiepNhanId == 0)
                    NguoiTiepNhanId = infoUser.Id;
            }
            else
            {
                NguoiTiepNhanId = -1;
                NguoiTiepNhan = "";
            }
            // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
            string NguoiTienXuLy = context.Request.Form["NguoiTienXuLy"] ?? context.Request.QueryString["NguoiTienXuLy"];
            int NguoiTienXuLyId = -1;
            if (!string.IsNullOrEmpty(NguoiTienXuLy) && !NguoiTienXuLy.Equals("Người tiền xử lý..."))
            {
                NguoiTienXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTienXuLy);
                if (NguoiTienXuLyId == 0)
                    NguoiTienXuLyId = -1;
            }
            else
            {
                NguoiTienXuLyId = -1;
                NguoiTienXuLy = "";
            }
            //string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
            string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
            int NguoiXuLyId = -1;
            if (typeSearch != "-1" && !string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
            {
                NguoiXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiXuLy);
                if (NguoiXuLyId == 0)
                    NguoiXuLyId = -1;
            }
            else
            {
                NguoiXuLyId = -1;
                NguoiXuLy = "";
            }

            //string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
            string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
            int nNgayTiepNhan_From = -1;
            if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            //string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
            string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
            int nNgayTiepNhan_To = -1;
            if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            //string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
            string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
            int nNgayQuaHan_From = -1;
            if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            //string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
            string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
            int nNgayQuaHan_To = -1;
            if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            var startPageIndex = 1;
            var pageSize = 1;
            switch (type)
            {
                case "1":
                    strReturn = CountKhieuNai_DaGuiDi_WithPage(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                      PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                      nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                      KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                      startPageIndex, pageSize);
                    break;
                case "2":
                    strReturn = CountKhieuNai_PhanHoi_WithPage(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                      PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                      nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                      KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                      startPageIndex, pageSize);
                    break;
                default:
                    strReturn = "0";
                    break;
            }


            context.Response.Write(strReturn);
        }
        private string CountKhieuNai_DaGuiDi_WithPage(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId,
            string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
        int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, int startPageIndex, int pageSize)
        {
            if (!TypeSearch.Equals("3") )
            {
                //whereClause += " AND NguoiTiepNhan='" + LoginAdmin.AdminLogin().Username + "' ";
                NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(LoginAdmin.AdminLogin().Username);
            }
            if (TypeSearch.Equals("1"))
            {
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
            }
            //whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            else if (TypeSearch.Equals("2"))
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
            //whereClause += " AND PhongBanXuLyId != " + LoginAdmin.AdminLogin().PhongBanId;
            else if (TypeSearch.Equals("3"))
            {
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
                NguoiTiepNhanId = -1;
                //whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            }

            try
            {
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    return CountKhieuNai_DaGuiDi_WithPage1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                      PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                      SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                      KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                      startPageIndex, pageSize);
                }
                else
                {
                    return CountKhieuNai_DaGuiDi_WithPage2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                      PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                      SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                      KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                      startPageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string CountKhieuNai_DaGuiDi_WithPage1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId,
            string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
        int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, int startPageIndex, int pageSize)
        {
            //try
            //{
            //    var totalRecord = 0;
            //    string selectClause = "a.Id,DoUuTien,SoThueBao,LoaiKhieuNai,LinhVucChung,LinhVucCon,NgayQuaHanPhongBanXuLy,NgayQuaHan,NgayTiepNhan,NoiDungPA,NguoiTiepNhan,NguoiXuLy,TrangThai,IsPhanHoi,b.Name PhongBan_Name";
            //    string joinClause = "LEFT JOIN PhongBan b on a.PhongBanXuLyId = b.Id";
            //    string whereClause = "1=1 ";


            if (!TypeSearch.Equals("3"))
            {
                //whereClause += " AND NguoiTiepNhan='" + LoginAdmin.AdminLogin().Username + "' ";
                NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(LoginAdmin.AdminLogin().Username);
            }
            if (TypeSearch.Equals("1"))
            {
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
            }
            //whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            else if (TypeSearch.Equals("2"))
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
            //whereClause += " AND PhongBanXuLyId != " + LoginAdmin.AdminLogin().PhongBanId;
            else if (TypeSearch.Equals("3"))
            {
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
                NguoiTiepNhanId = -1;
                //whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            }

            //if (!TypeSearch.Equals("3"))
            //{
            //    whereClause += " AND NguoiTiepNhan='" + LoginAdmin.AdminLogin().Username + "' ";
            //}

            //if (TypeSearch.Equals("1"))
            //    whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            //else if (TypeSearch.Equals("2"))
            //    whereClause += " AND PhongBanXuLyId != " + LoginAdmin.AdminLogin().PhongBanId;
            //else if (TypeSearch.Equals("3"))
            //{
            //    whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            //}


            //    if (!trangThai.Equals("-1"))
            //        whereClause += " AND TrangThai=" + trangThai;
            //    else
            //        whereClause += " AND TrangThai != " + (byte)KhieuNai_TrangThai_Type.Đóng;

            //    if (!DoUuTien.Equals("-1"))
            //        whereClause += " AND DoUuTien=" + DoUuTien;

            //    if (!LoaiKhieuNaiId.Equals("-1"))
            //        whereClause += " AND LoaiKhieuNaiId=" + LoaiKhieuNaiId;

            //    if (!LinhVucChungId.Equals("-1"))
            //        whereClause += " AND LinhVucChungId=" + LinhVucChungId;

            //    if (!LinhVucConId.Equals("-1"))
            //        whereClause += " AND LinhVucConId=" + LinhVucConId;

            //    if (SoThueBao > 0)
            //        whereClause += " AND SoThueBao=" + SoThueBao;

            //    if (NguoiTiepNhanId > 0)
            //        whereClause += " AND NguoiTiepNhanId='" + NguoiTiepNhanId + "'";

            //    if (NguoiXuLyId > 0)
            //        whereClause += " AND NguoiXuLyId='" + NguoiXuLyId + "'";

            //    if (!contentSeach.Trim().Equals(""))
            //        whereClause += " AND NoiDungPA like N'%" + contentSeach + "%'";



            //        if (NgayTiepNhan_From > 0 && NgayTiepNhan_To > 0)
            //        {
            //            whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", NgayTiepNhan_From, NgayTiepNhan_To);
            //        }




            //        if (NgayQuaHan_From > 0 && NgayQuaHan_To > 0)
            //        {
            //            whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", NgayQuaHan_From, NgayQuaHan_To);
            //        }


            //    string orderClause = "a.LDate desc";


            //    //    var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage(contentSeach, Convert.ToInt32(TypeSearch),
            //    //    Convert.ToInt32(LoaiKhieuNaiId),
            //    //    Convert.ToInt32(LinhVucChungId),
            //    //    Convert.ToInt32(LinhVucConId),
            //    //    Convert.ToInt32(PhongBanId),
            //    //    Convert.ToInt32(DoUuTien),
            //    //    Convert.ToInt32(trangThai),
            //    //    NguoiXuLyId,
            //    //    NguoiTienXuLyId,
            //    //    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
            //    //    KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
            //    //    Convert.ToInt32(startPageIndex),
            //    //    Convert.ToInt32(pageSize));

            //    var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

            //    return totalRecord.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Utility.LogEvent(ex);
            //    return "-1";
            //}

            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_DaGuiDi_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string CountKhieuNai_DaGuiDi_WithPage2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId,
            string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
        int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, int startPageIndex, int pageSize)
        {
            if (!TypeSearch.Equals("3"))
            {
                //whereClause += " AND NguoiTiepNhan='" + LoginAdmin.AdminLogin().Username + "' ";
                NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(LoginAdmin.AdminLogin().Username);
            }
            if (TypeSearch.Equals("1"))
            {
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
            }
            //whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            else if (TypeSearch.Equals("2"))
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
            //whereClause += " AND PhongBanXuLyId != " + LoginAdmin.AdminLogin().PhongBanId;
            else if (TypeSearch.Equals("3"))
            {
                PhongBanId = LoginAdmin.AdminLogin().PhongBanId.ToString();
                NguoiTiepNhanId = -1;
                //whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
            }

            
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_DaGuiDi_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }



        private string CountKhieuNai_PhanHoi_WithPage(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId,
         string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
     int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
     int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, int startPageIndex, int pageSize)
        {
            //var totalRecord = 0;
            //string selectClause = "a.Id,a.DoUuTien, a.SoThueBao, a.LoaiKhieuNai, a.NguoiTiepNhan, a.NoiDungPA, a.NguoiXuLy ,b.GhiChu,b.NguoiXuLyTruoc,b.NguoiDuocPhanHoi";
            //string joinClause = "right join KhieuNai_Activity b on b.KhieuNaiId = a.Id";
            //string whereClause = "IsCurrent = 1 and HanhDong = 3 and  a.PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;


            //if (!TypeSearch.Equals("1"))
            //    whereClause += " AND a.NguoiXuLy='" + LoginAdmin.AdminLogin().Username + "'";
            if (TypeSearch.Equals("0"))
                NguoiXuLyId = LoginAdmin.AdminLogin().Id;
            if (TypeSearch.Equals("1"))
                NguoiXuLyId = LoginAdmin.AdminLogin().Id;


            //if (!trangThai.Equals("-1"))
            //    whereClause += " AND a.TrangThai=" + trangThai;
            //else
            //    whereClause += " AND a.TrangThai != 3";

            //if (!DoUuTien.Equals("-1"))
            //    whereClause += " AND a.DoUuTien=" + DoUuTien;

            //if (SoThueBao > 0)
            //    whereClause += " AND a.SoThueBao=" + SoThueBao;

            //if (!LoaiKhieuNaiId.Equals("-1"))
            //    whereClause += " AND LoaiKhieuNaiId=" + LoaiKhieuNaiId;

            //if (!LinhVucChungId.Equals("-1"))
            //    whereClause += " AND LinhVucChungId=" + LinhVucChungId;

            //if (!LinhVucConId.Equals("-1"))
            //    whereClause += " AND LinhVucConId=" + LinhVucConId;

            //if (NguoiTiepNhanId > 0)
            //    whereClause += " AND a.NguoiTiepNhanId ='" + NguoiTiepNhanId + "'";

            //if (NguoiXuLyId > 0)
            //    whereClause += " AND a.NguoiXuLyId='" + NguoiXuLyId + "'";

            //if (!contentSeach.Trim().Equals(""))
            //    whereClause += " AND a.NoiDungPA like N'%" + contentSeach + "%'";



            //if (NgayTiepNhan_From > 0 && NgayTiepNhan_To > 0)
            //{
            //    whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", NgayTiepNhan_From, NgayTiepNhan_To);
            //}




            //if (NgayQuaHan_From > 0 && NgayQuaHan_To > 0)
            //{
            //    whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", NgayQuaHan_From, NgayQuaHan_To);
            //}


            //string orderClause = "a.LDate desc";
            //var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

            //return totalRecord.ToString();

            try
            {
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    return CountKhieuNai_PhanHoi_WithPage1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                      PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                      SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                      KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                      startPageIndex, pageSize);
                }
                else
                {
                    return CountKhieuNai_PhanHoi_WithPage2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                      PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                      SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                      KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                      startPageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string CountKhieuNai_PhanHoi_WithPage1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId,
         string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
     int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
     int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, int startPageIndex, int pageSize)
        {
            //var totalRecord = 0;
            //string selectClause = "a.Id,a.DoUuTien, a.SoThueBao, a.LoaiKhieuNai, a.NguoiTiepNhan, a.NoiDungPA, a.NguoiXuLy ,b.GhiChu,b.NguoiXuLyTruoc,b.NguoiDuocPhanHoi";
            //string joinClause = "right join KhieuNai_Activity b on b.KhieuNaiId = a.Id";
            //string whereClause = "IsCurrent = 1 and HanhDong = 3 and  a.PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;


            //if (!TypeSearch.Equals("1"))
            //    whereClause += " AND a.NguoiXuLy='" + LoginAdmin.AdminLogin().Username + "'";
            if (TypeSearch.Equals("0"))
                NguoiXuLyId = LoginAdmin.AdminLogin().Id;
            if (TypeSearch.Equals("1"))
                NguoiXuLyId = LoginAdmin.AdminLogin().Id;


            //if (!trangThai.Equals("-1"))
            //    whereClause += " AND a.TrangThai=" + trangThai;
            //else
            //    whereClause += " AND a.TrangThai != 3";

            //if (!DoUuTien.Equals("-1"))
            //    whereClause += " AND a.DoUuTien=" + DoUuTien;

            //if (SoThueBao > 0)
            //    whereClause += " AND a.SoThueBao=" + SoThueBao;

            //if (!LoaiKhieuNaiId.Equals("-1"))
            //    whereClause += " AND LoaiKhieuNaiId=" + LoaiKhieuNaiId;

            //if (!LinhVucChungId.Equals("-1"))
            //    whereClause += " AND LinhVucChungId=" + LinhVucChungId;

            //if (!LinhVucConId.Equals("-1"))
            //    whereClause += " AND LinhVucConId=" + LinhVucConId;

            //if (NguoiTiepNhanId > 0)
            //    whereClause += " AND a.NguoiTiepNhanId ='" + NguoiTiepNhanId + "'";

            //if (NguoiXuLyId > 0)
            //    whereClause += " AND a.NguoiXuLyId='" + NguoiXuLyId + "'";

            //if (!contentSeach.Trim().Equals(""))
            //    whereClause += " AND a.NoiDungPA like N'%" + contentSeach + "%'";



            //if (NgayTiepNhan_From > 0 && NgayTiepNhan_To > 0)
            //{
            //    whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", NgayTiepNhan_From, NgayTiepNhan_To);
            //}




            //if (NgayQuaHan_From > 0 && NgayQuaHan_To > 0)
            //{
            //    whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", NgayQuaHan_From, NgayQuaHan_To);
            //}


            //string orderClause = "a.LDate desc";
            //var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

            //return totalRecord.ToString();

            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_PhanHoi_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string CountKhieuNai_PhanHoi_WithPage2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId,
         string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
     int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
     int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, int startPageIndex, int pageSize)
        {
            //var totalRecord = 0;
            //string selectClause = "a.Id,a.DoUuTien, a.SoThueBao, a.LoaiKhieuNai, a.NguoiTiepNhan, a.NoiDungPA, a.NguoiXuLy ,b.GhiChu,b.NguoiXuLyTruoc,b.NguoiDuocPhanHoi";
            //string joinClause = "right join KhieuNai_Activity b on b.KhieuNaiId = a.Id";
            //string whereClause = "IsCurrent = 1 and HanhDong = 3 and  a.PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;


            //if (!TypeSearch.Equals("1"))
            //    whereClause += " AND a.NguoiXuLy='" + LoginAdmin.AdminLogin().Username + "'";
            if (TypeSearch.Equals("0"))
                NguoiXuLyId = LoginAdmin.AdminLogin().Id;
            if (TypeSearch.Equals("1"))
                NguoiXuLyId = LoginAdmin.AdminLogin().Id;


            //if (!trangThai.Equals("-1"))
            //    whereClause += " AND a.TrangThai=" + trangThai;
            //else
            //    whereClause += " AND a.TrangThai != 3";

            //if (!DoUuTien.Equals("-1"))
            //    whereClause += " AND a.DoUuTien=" + DoUuTien;

            //if (SoThueBao > 0)
            //    whereClause += " AND a.SoThueBao=" + SoThueBao;

            //if (!LoaiKhieuNaiId.Equals("-1"))
            //    whereClause += " AND LoaiKhieuNaiId=" + LoaiKhieuNaiId;

            //if (!LinhVucChungId.Equals("-1"))
            //    whereClause += " AND LinhVucChungId=" + LinhVucChungId;

            //if (!LinhVucConId.Equals("-1"))
            //    whereClause += " AND LinhVucConId=" + LinhVucConId;

            //if (NguoiTiepNhanId > 0)
            //    whereClause += " AND a.NguoiTiepNhanId ='" + NguoiTiepNhanId + "'";

            //if (NguoiXuLyId > 0)
            //    whereClause += " AND a.NguoiXuLyId='" + NguoiXuLyId + "'";

            //if (!contentSeach.Trim().Equals(""))
            //    whereClause += " AND a.NoiDungPA like N'%" + contentSeach + "%'";



            //if (NgayTiepNhan_From > 0 && NgayTiepNhan_To > 0)
            //{
            //    whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", NgayTiepNhan_From, NgayTiepNhan_To);
            //}




            //if (NgayQuaHan_From > 0 && NgayQuaHan_To > 0)
            //{
            //    whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", NgayQuaHan_From, NgayQuaHan_To);
            //}


            //string orderClause = "a.LDate desc";
            //var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

            //return totalRecord.ToString();

            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_PhanHoi_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}