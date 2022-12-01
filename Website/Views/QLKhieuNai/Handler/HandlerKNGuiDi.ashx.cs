using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
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
    /// Summary description for HandlerKNGuiDi
    /// </summary>
    public class HandlerKNGuiDi : IHttpHandler, IReadOnlySessionState
    {
        protected class DataKhieuNai
        {
            /// <summary>
            /// 
            /// </summary>
            public int page { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int total { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<DataItem> rows { get; set; }
        }

        protected class DataItem
        {
            /// <summary>
            /// 
            /// </summary>
            public Int64 STT { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CheckAll { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DoUuTien { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string TrangThai { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string SoThueBao { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NoiDungPA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string LoaiKhieuNai { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string LinhVucChung { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string LinhVucCon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PhongBanTiepNhan { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NguoiTiepNhan { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NguoiXuLyTruoc { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PhongBanXuLy { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NguoiXuLy { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NguoiDuocPhanHoi { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NguoiTienXuLyCap1 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NguoiTienXuLyCap2 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NguoiTienXuLyCap3 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NgayTiepNhanSort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NgayQuaHanSort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NgayQuaHanPhongBanXuLySort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IsPhanViec { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string LDate { get; set; }

        }
        public void ProcessRequest(HttpContext context)
        {
            string strReturn = string.Empty;
            context.Response.ContentType = "text/plain";
            string type = context.Request.Form["key"] ?? context.Request.QueryString["key"];
            AdminInfo infoUser = LoginAdmin.AdminLogin();
            if (infoUser == null)
            {
                context.Response.Write(strReturn); return;
            }
            if (!string.IsNullOrEmpty(context.Request.Form["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.Form["pageSize"]))
            {
                string typeSearch = context.Request.Form["typeSearch"];
                string doUuTien = context.Request.Form["doUuTien"];
                string trangThai = context.Request.Form["trangThai"];
                string loaiKhieuNai = context.Request.Form["loaiKhieuNai"];
                string linhVucChung = context.Request.Form["linhVucChung"];
                string linhVucCon = context.Request.Form["linhVucCon"];
                string ListLinhVucConID = "";
                if (linhVucCon != "0" && linhVucCon != "-1")
                {
                    List<LoaiKhieuNaiInfo> ListLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id", "Name=" + "N'" + linhVucCon + "'", "");
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
                string phongBanXuLy = context.Request.Form["phongBanXuLy"].ToString();
                string ShowNguoiXuLy = context.Request.Form["ShowNguoiXuLy"];

                string NguoiXuLy_Filter = "-1";
                bool IsTatCaKN = false;
                int KNHangLoat = 0;
                string PhongBanId = infoUser.PhongBanId.ToString();
                bool isPermission = false;
                int NguoiTiepNhanId = -1;
                if (ShowNguoiXuLy.Equals("1"))
                {
                    NguoiXuLy_Filter = "";
                }


                switch (typeSearch)
                {
                    case "0": //Tất cả
                        KNHangLoat = -1;
                        IsTatCaKN = true;

                        //NguoiTiepNhanId = -1;
                        break;
                    case "1": // Khiếu nại của tôi
                        KNHangLoat = -1;
                        NguoiXuLy_Filter = infoUser.Username;
                        NguoiTiepNhanId = infoUser.Id;
                        //  PhongBanId = "-1";
                        break;
                    case "2": //Khiếu nại đã chuyển
                        KNHangLoat = -1;
                        //  PhongBanId = "-1";
                        break;
                    case "3": //Phòng ban                      
                    default: //Phòng ban
                        PhongBanId = infoUser.PhongBanId.ToString();
                        KNHangLoat = -1;
                        break;
                }

                //string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập nội dung phản ảnh..."))
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


                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];

                if (!string.IsNullOrEmpty(NguoiTiepNhan) && !NguoiTiepNhan.Equals("Người tiếp nhận..."))
                {
                    NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTiepNhan);
                    if (NguoiTiepNhanId == 0)
                        NguoiTiepNhanId = -1;
                }
                else
                {
                    NguoiTiepNhanId = -1;
                    NguoiTiepNhan = "";
                }

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
                //END LONGLX

                string sortName = context.Request.Form["sortname"];
                string sortOrder = context.Request.Form["sortorder"];

                var startPageIndex = Convert.ToInt32(context.Request.Form["startPageIndex"].ToString());
                var pageSize = Convert.ToInt32(context.Request.Form["pageSize"].ToString());
                switch (type)
                {
                    case "1":
                        strReturn = GetKhieuNai_DaGuiDi_WithPage(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                         ListLinhVucConID, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                         nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                         KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                         startPageIndex, pageSize, infoUser);
                        break;

                    case "2":
                        strReturn = GetKhieuNai_PhanHoi_WithPage(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                      ListLinhVucConID, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                      nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                      KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                      startPageIndex, pageSize, infoUser);
                        break;
                    default:
                        strReturn = string.Empty;
                        break;
                }


                context.Response.Write(strReturn);

            }
        }
        #region KN da gui

        private string GetKhieuNai_DaGuiDi_WithPage(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, int startPageIndex, int pageSize, AdminInfo infoUser)
        {
            try
            {
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    return GetKhieuNai_DaGuiDi_WithPage1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                         LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                         SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                         KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                         startPageIndex, pageSize, infoUser);
                }
                else
                {
                    return GetKhieuNai_DaGuiDi_WithPage2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                         LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                         SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                         KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                         startPageIndex, pageSize, infoUser);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNai_DaGuiDi_WithPage1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, int startPageIndex, int pageSize, AdminInfo infoUser)
        {
            try
            {
                //  var totalRecord = 0;
                //   string selectClause = "a.Id,DoUuTien,SoThueBao,LoaiKhieuNai,LinhVucChung,LinhVucCon,PhongBanTiepNhanId, PhongBanXuLyId,NgayQuaHanPhongBanXuLy,NgayQuaHan,NgayTiepNhan,NoiDungPA,NguoiTiepNhan,NguoiXuLy,NguoiTienXuLyCap1,NguoiTienXuLyCap2,NguoiTienXuLyCap3, TrangThai,IsPhanHoi,b.Name PhongBan_Name";
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
                //    whereClause += " AND NguoiTiepNhan='" + infoUser.Username + "' ";
                //}

                //if (!trangThai.Equals("-1"))
                //    whereClause += " AND TrangThai=" + trangThai;
                //else
                //    whereClause += " AND TrangThai != " + (byte)KhieuNai_TrangThai_Type.Đóng;

                //if (TypeSearch.Equals("1"))
                //    whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
                //else if (TypeSearch.Equals("2"))
                //    whereClause += " AND PhongBanXuLyId != " + LoginAdmin.AdminLogin().PhongBanId;
                //else if (TypeSearch.Equals("3"))
                //{
                //    whereClause += " AND PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;
                //}

                //if (!trangThai.Equals("-1"))
                //    whereClause += " AND TrangThai=" + trangThai;
                //else
                //    whereClause += " AND TrangThai != " + (byte)KhieuNai_TrangThai_Type.Đóng;


                //if (!DoUuTien.Equals("-1"))
                //    whereClause += " AND DoUuTien=" + DoUuTien;

                //if (!LoaiKhieuNaiId.Equals("-1"))
                //    whereClause += " AND LoaiKhieuNaiId=" + LoaiKhieuNaiId;

                //if (!LinhVucChungId.Equals("-1"))
                //    whereClause += " AND LinhVucChungId=" + LinhVucChungId;

                //if (!LinhVucConId.Equals("-1"))
                //    whereClause += " AND LinhVucConId=" + LinhVucConId;

                //if (SoThueBao > 0)
                //    whereClause += " AND SoThueBao=" + SoThueBao;

                //if (NguoiTiepNhanId > 0)
                //    whereClause += " AND NguoiTiepNhanId='" + NguoiTiepNhanId + "'";

                //if (NguoiXuLyId > 0)
                //    whereClause += " AND NguoiXuLyId='" + NguoiXuLyId + "'";

                //if (!contentSeach.Trim().Equals(""))
                //    whereClause += " AND NoiDungPA like N'%" + contentSeach + "%'";



                //    if (NgayTiepNhan_From > 0 && NgayTiepNhan_To > 0)
                //    {
                //        whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", NgayTiepNhan_From, NgayTiepNhan_To);
                //    }




                //    if (NgayQuaHan_From > 0 && NgayQuaHan_To > 0)
                //    {
                //        whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", NgayQuaHan_From, NgayQuaHan_To);
                //    }


                //string orderClause = "a.LDate desc";


                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_DaGuiDi_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                //var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab1-KNDaGuiDi" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DataKhieuNai result = new DataKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNai_DaGuiDi_WithPage2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, int startPageIndex, int pageSize, AdminInfo infoUser)
        {
            try
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
                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_DaGuiDi_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                //var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab1-KNDaGuiDi" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DataKhieuNai result = new DataKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region KN  phản hồi
        private string GetKhieuNai_PhanHoi_WithPage(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, int startPageIndex, int pageSize, AdminInfo infoUser)
        {
            try
            {

                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    return GetKhieuNai_PhanHoi_WithPage1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                        NgayQuaHan_From, NgayQuaHan_To,
                        KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        startPageIndex, pageSize, infoUser);
                }
                else
                {
                    return GetKhieuNai_PhanHoi_WithPage2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                        NgayQuaHan_From, NgayQuaHan_To,
                        KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        startPageIndex, pageSize, infoUser);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNai_PhanHoi_WithPage1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, int startPageIndex, int pageSize, AdminInfo infoUser)
        {
            try
            {

                //var totalRecord = 0;
                //string selectClause = "a.Id,a.DoUuTien, a.SoThueBao, a.LoaiKhieuNai, a.NguoiTiepNhan,a.PhongBanTiepNhanId, a.PhongBanXuLyId, NguoiTienXuLyCap1,NguoiTienXuLyCap2,NguoiTienXuLyCap3, a.NoiDungPA, a.NguoiXuLy ,b.GhiChu,b.NguoiXuLyTruoc,b.NguoiDuocPhanHoi";
                //string joinClause = "right join KhieuNai_Activity b on b.KhieuNaiId = a.Id";
                //string whereClause = "IsCurrent = 1 and HanhDong = 3 and  a.PhongBanXuLyId=" + infoUser.PhongBanId;

                if (TypeSearch.Equals("0"))
                    NguoiXuLyId = LoginAdmin.AdminLogin().Id;
                if (TypeSearch.Equals("1"))
                    NguoiXuLyId = LoginAdmin.AdminLogin().Id;

                //if (TypeSearch.Equals("1"))
                //    whereClause += " AND a.NguoiXuLy='" + infoUser.Username + "'";


                //if (!trangThai.Equals("-1"))
                //    whereClause += " AND a.TrangThai=" + trangThai;
                //else
                //    whereClause += " AND a.TrangThai != 3" ;
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

                //if (NguoiXuLyId > 0 )
                //    whereClause += " AND a.NguoiXuLyId='" + NguoiXuLyId + "'";

                //if (!contentSeach.Trim().Equals(""))
                //    whereClause += " AND a.NoiDungPA like N'%" + contentSeach + "%'";



                //    if (NgayTiepNhan_From > 0 && NgayTiepNhan_To > 0)
                //    {
                //        whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", NgayTiepNhan_From, NgayTiepNhan_To);
                //    }




                //    if (NgayQuaHan_From > 0 && NgayQuaHan_To > 0)
                //    {
                //        whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", NgayQuaHan_From, NgayQuaHan_To);
                //    }


                //string orderClause = "a.LDate desc";


                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_PhanHoi_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                //     var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

                //int temp = 0;
                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNPhanHoi" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DataKhieuNai result = new DataKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNai_PhanHoi_WithPage2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, int startPageIndex, int pageSize, AdminInfo infoUser)
        {
            try
            {

                //var totalRecord = 0;
                //string selectClause = "a.Id,a.DoUuTien, a.SoThueBao, a.LoaiKhieuNai, a.NguoiTiepNhan,a.PhongBanTiepNhanId, a.PhongBanXuLyId, NguoiTienXuLyCap1,NguoiTienXuLyCap2,NguoiTienXuLyCap3, a.NoiDungPA, a.NguoiXuLy ,b.GhiChu,b.NguoiXuLyTruoc,b.NguoiDuocPhanHoi";
                //string joinClause = "right join KhieuNai_Activity b on b.KhieuNaiId = a.Id";
                //string whereClause = "IsCurrent = 1 and HanhDong = 3 and  a.PhongBanXuLyId=" + infoUser.PhongBanId;

                if (TypeSearch.Equals("0"))
                    NguoiXuLyId = LoginAdmin.AdminLogin().Id;
                if (TypeSearch.Equals("1"))
                    NguoiXuLyId = LoginAdmin.AdminLogin().Id;

                //if (TypeSearch.Equals("1"))
                //    whereClause += " AND a.NguoiXuLy='" + infoUser.Username + "'";


                //if (!trangThai.Equals("-1"))
                //    whereClause += " AND a.TrangThai=" + trangThai;
                //else
                //    whereClause += " AND a.TrangThai != 3" ;
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

                //if (NguoiXuLyId > 0 )
                //    whereClause += " AND a.NguoiXuLyId='" + NguoiXuLyId + "'";

                //if (!contentSeach.Trim().Equals(""))
                //    whereClause += " AND a.NoiDungPA like N'%" + contentSeach + "%'";



                //    if (NgayTiepNhan_From > 0 && NgayTiepNhan_To > 0)
                //    {
                //        whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", NgayTiepNhan_From, NgayTiepNhan_To);
                //    }




                //    if (NgayQuaHan_From > 0 && NgayQuaHan_To > 0)
                //    {
                //        whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", NgayQuaHan_From, NgayQuaHan_To);
                //    }


                //string orderClause = "a.LDate desc";


                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_PhanHoi_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                //     var lstResult = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, startPageIndex, pageSize, ref totalRecord);

                //int temp = 0;
                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNPhanHoi" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DataKhieuNai result = new DataKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }


        #endregion
        #region Bind Data

        private List<DataItem> BindResultToDataItem(List<KhieuNaiInfo> lstResult, AdminInfo infoUser, string strReturnURL)
        {
            List<DataItem> lstRow = new List<DataItem>();

            if (lstResult != null && lstResult.Count > 0)
            {
                foreach (var item in lstResult)
                {
                    DataItem itemKN = new DataItem();

                    itemKN.STT = item.STT;

                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.PhongBanXuLyId != infoUser.PhongBanId || item.NguoiXuLy != infoUser.Username)
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" type =\"checkbox\" disabled=\"disabled\" />";
                        }
                        else
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" value=\"" + item.Id + "\" id =\"checkbox" + item.Id + "\" type =\"checkbox\" />";
                        }
                    }
                    else
                    {
                        if (item.PhongBanXuLyId == infoUser.PhongBanId)
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" value=\"" + item.Id + "\" id =\"checkbox" + item.Id + "\" type =\"checkbox\" />";
                        }
                        else
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" type =\"checkbox\" disabled=\"disabled\" />";
                        }
                    }

                    itemKN.TrangThai = BindTinhTrangXuLy(item.TrangThai, item.IsPhanHoi, item.NgayQuaHanPhongBanXuLy);

                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.PhongBanXuLyId != infoUser.PhongBanId || item.NguoiXuLy != infoUser.Username)
                        {
                            itemKN.Id = "<a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                        else
                        {
                            itemKN.Id = "<a href=\"javascript:CheckXuLyKhieuNai(" + item.Id + ",'/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&Mode=Process','" + strReturnURL + "')\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                    }
                    else
                    {
                        if (item.PhongBanXuLyId == infoUser.PhongBanId)
                        {
                            itemKN.Id = "<a href=\"javascript:CheckXuLyKhieuNai(" + item.Id + ",'/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&Mode=Process','" + strReturnURL + "')\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                        else
                        {
                            itemKN.Id = "<a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                    }

                    itemKN.DoUuTien = Enum.GetName(typeof(KhieuNai_DoUuTien_Type), item.DoUuTien).Replace("_", " ");
                    itemKN.SoThueBao = "<a class='ShowChiTiet_" + item.Id + "' href=\"javascript:ShowPoupChiTietKN('" + item.Id + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + item.SoThueBao + "</a>";
                    itemKN.LoaiKhieuNai = item.LoaiKhieuNai;
                    itemKN.LinhVucChung = item.LinhVucChung;
                    itemKN.LinhVucCon = item.LinhVucCon;
                    itemKN.NoiDungPA = item.NoiDungPA;
                    itemKN.PhongBanTiepNhan = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanTiepNhanId);
                    itemKN.PhongBanXuLy = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                    itemKN.NguoiTienXuLyCap1 = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTienXuLyCap1 + "'>" + item.NguoiTienXuLyCap1 + "</a>";
                    itemKN.NguoiTienXuLyCap2 = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTienXuLyCap2 + "'>" + item.NguoiTienXuLyCap2 + "</a>";
                    itemKN.NguoiTienXuLyCap3 = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTienXuLyCap3 + "'>" + item.NguoiTienXuLyCap3 + "</a>";
                    itemKN.NguoiTiepNhan = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTiepNhan + "'>" + item.NguoiTiepNhan + "</a>";
                    itemKN.NguoiXuLy = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiXuLy + "'>" + item.NguoiXuLy + "</a>";
                    if (string.IsNullOrEmpty(item.NguoiDuocPhanHoi))
                        itemKN.NguoiDuocPhanHoi = string.Empty;
                    else
                        itemKN.NguoiDuocPhanHoi = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiDuocPhanHoi + "'>" + item.NguoiDuocPhanHoi + "</a>";
                    if (item.IsPhanViec)
                    {
                        itemKN.IsPhanViec = "<input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\">";
                    }
                    else
                    {
                        itemKN.IsPhanViec = "";
                    }
                    itemKN.NgayTiepNhanSort = item.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss");
                    itemKN.NgayQuaHanSort = item.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss");
                    itemKN.NgayQuaHanPhongBanXuLySort = item.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss");
                    itemKN.LDate = item.LDate.ToString("dd/MM/yyyy HH:mm:ss");

                    lstRow.Add(itemKN);
                }
            }
            return lstRow;
        }

        protected string BindTinhTrangXuLy(object obj, bool isPhanHoi, DateTime NgayQuaHanPhongBan)
        {
            if (Convert.ToByte(obj) == (byte)KhieuNai_TrangThai_Type.Đóng)
                return string.Format("<span style='border: 1pt solid #CCC; background: green; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
            else
            {
                if (isPhanHoi)
                {
                    return string.Format("<span style='border: 1pt solid #CCC; background: #FF8000; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                }
                else if (NgayQuaHanPhongBan < DateTime.Now)
                {
                    return string.Format("<span style='border: 1pt solid #CCC; background: #999; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                }
                else
                {
                    if (Convert.ToByte(obj) == (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý)
                        return string.Format("<span style='border: 1pt solid #CCC; background: red; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                    else if (Convert.ToByte(obj) == (byte)KhieuNai_TrangThai_Type.Chờ_đóng)
                        return string.Format("<span style='border: 1pt solid #CCC; background: #0095CC; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                    else
                        return string.Format("<span style='border: 1pt solid #CCC; background: yellow; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                }
            }
        }

        protected string BindMaKN(object obj)
        {
            return string.Format("<a href=\"javascript:ShowPoupChiTietKN('{0}');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">{1}</a>", obj, GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, obj, 10));
        }

        protected string BindNgayDong(object trangthai, object ngaydong)
        {
            if ((int)KhieuNai_TrangThai_Type.Đóng == Convert.ToInt32(trangthai))
            {
                return ngaydong.ToString();
            }
            return string.Empty;
        }

        protected string BindDoUuTien(object obj)
        {
            try
            {
                return Enum.GetName(typeof(KhieuNai_DoUuTien_Type), Convert.ToByte(obj)).Replace("_", " ");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        protected string BindHTTiepNhan(object obj)
        {
            try
            {
                return Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), Convert.ToByte(obj)).Replace("_", " ");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        protected string BindBooleanToCheckbox(string id, bool flag, bool readOnly)
        {
            return string.Format("<input id='{0}' type='checkbox' {1} {2}/>", id, flag ? "checked='checked'" : "", readOnly ? "disabled='disabled'" : "");
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}