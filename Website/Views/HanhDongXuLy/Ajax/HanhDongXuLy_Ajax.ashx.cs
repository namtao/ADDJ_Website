using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Website.AppCode;
using System.Text.RegularExpressions;
using Aspose.Cells;
using System.Drawing;
using System.IO;
using System.Text;

namespace Website.Views.HanhDongXuLy.Ajax
{
    /// <summary>
    /// Summary description for HanhDongXuLy_Ajax1
    /// </summary>
    public class HanhDongXuLy_Ajax1 : IHttpHandler, IReadOnlySessionState
    {
        KhieuNaiImpl _KhieuNaiImpl = new KhieuNaiImpl();
        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request.QueryString["key"].ToString();
            System.Web.Script.Serialization.JavaScriptSerializer JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";
            if (!string.IsNullOrEmpty(context.Request.QueryString["key"]))
            {
                AdminInfo infoUser = (AdminInfo)context.Session[Constant.SessionNameAccountAdmin];
                if (infoUser != null)
                {
                    context.Response.Write(JSSerializer.Serialize(ProcessData(context.Request.QueryString["key"], infoUser, context)));
                }
            }
        }
        private string ProcessData(string key, AdminInfo infoUser, HttpContext context)
        {
            string strValue = "";
            switch (key)
            {
                case "09":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["loaiKhieuNaiId"]))
                    {
                        strValue = GetItemDropListLinhVucChung(context.Request.QueryString["loaiKhieuNaiId"]);
                    }
                    break;
                case "10":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["linhVucChungId"]))
                    {
                        strValue = GetItemDropListLinhVucCon(context.Request.QueryString["linhVucChungId"]);
                    }
                    break;
                case "11":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                        int NguoiTiepNhanId = -1;
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
                        // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
                        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                        int NguoiXuLyId = -1;
                        if (!string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
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
                        string maKhieuNai = context.Request.Form["maKhieuNai"] ?? context.Request.QueryString["maKhieuNai"];
                        string NgayTiepNhan_tu = context.Request.Form["NgayTiepNhan_tu"] ?? context.Request.QueryString["NgayTiepNhan_tu"];
                        string NgayTiepNhan_den = context.Request.Form["NgayTiepNhan_den"] ?? context.Request.QueryString["NgayTiepNhan_den"];
                        string ThoiGianCapNhat_tu = context.Request.Form["ThoiGianCapNhat_tu"] ?? context.Request.QueryString["ThoiGianCapNhat_tu"];
                        string ThoiGianCapNhat_den = context.Request.Form["ThoiGianCapNhat_den"] ?? context.Request.QueryString["ThoiGianCapNhat_den"];
                        string PhongBanXuLy = context.Request.Form["PhongBanXuLy"] ?? context.Request.QueryString["PhongBanXuLy"];
                        string TrangThai = context.Request.QueryString["TrangThai"].ToString();
                        strValue = GetHtmlHanhDongXuLy(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, PhongBanXuLy, SoThueBao, NguoiTiepNhanId,
                            NguoiXuLyId, maKhieuNai, NgayTiepNhan_tu, NgayTiepNhan_den, ThoiGianCapNhat_tu, ThoiGianCapNhat_den, TrangThai, startPageIndex, pageSize, infoUser);
                    }
                    break;
                case "12":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                        int NguoiTiepNhanId = -1;
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
                        // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
                        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                        int NguoiXuLyId = -1;
                        if (!string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
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
                        string maKhieuNai = context.Request.Form["maKhieuNai"] ?? context.Request.QueryString["maKhieuNai"];
                        string NgayTiepNhan_tu = context.Request.Form["NgayTiepNhan_tu"] ?? context.Request.QueryString["NgayTiepNhan_tu"];
                        string NgayTiepNhan_den = context.Request.Form["NgayTiepNhan_den"] ?? context.Request.QueryString["NgayTiepNhan_den"];


                        string ThoiGianCapNhat_tu = context.Request.Form["ThoiGianCapNhat_tu"] ?? context.Request.QueryString["ThoiGianCapNhat_tu"];
                        string ThoiGianCapNhat_den = context.Request.Form["ThoiGianCapNhat_den"] ?? context.Request.QueryString["ThoiGianCapNhat_den"];

                        string PhongBanXuLy = context.Request.Form["PhongBanXuLy"] ?? context.Request.QueryString["PhongBanXuLy"];
                        string TrangThai = context.Request.QueryString["TrangThai"].ToString();
                        strValue = GetHanhDongXuLy_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, PhongBanXuLy, SoThueBao, NguoiTiepNhanId,
                            NguoiXuLyId, maKhieuNai, NgayTiepNhan_tu, NgayTiepNhan_den, ThoiGianCapNhat_tu, ThoiGianCapNhat_den, TrangThai, startPageIndex, pageSize);
                    }
                    break;
                case "13":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                        int NguoiTiepNhanId = -1;
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
                        // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
                        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                        int NguoiXuLyId = -1;
                        if (!string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
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
                        string maKhieuNai = context.Request.Form["maKhieuNai"] ?? context.Request.QueryString["maKhieuNai"];
                        string NgayTiepNhan_tu = context.Request.Form["NgayTiepNhan_tu"] ?? context.Request.QueryString["NgayTiepNhan_tu"];
                        string NgayTiepNhan_den = context.Request.Form["NgayTiepNhan_den"] ?? context.Request.QueryString["NgayTiepNhan_den"];
                        string ThoiGianCapNhat_tu = context.Request.Form["ThoiGianCapNhat_tu"] ?? context.Request.QueryString["ThoiGianCapNhat_tu"];
                        string ThoiGianCapNhat_den = context.Request.Form["ThoiGianCapNhat_den"] ?? context.Request.QueryString["ThoiGianCapNhat_den"];
                        string PhongBanXuLy = context.Request.Form["PhongBanXuLy"] ?? context.Request.QueryString["PhongBanXuLy"];
                        string TrangThai = context.Request.QueryString["TrangThai"].ToString();
                        strValue = ExportHanhDongXuLy(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, PhongBanXuLy, SoThueBao, NguoiTiepNhanId,
                            NguoiXuLyId, maKhieuNai, NgayTiepNhan_tu, NgayTiepNhan_den, ThoiGianCapNhat_tu, ThoiGianCapNhat_den, TrangThai, startPageIndex, pageSize);
                    }
                    break;
                case "14":
                    strValue = LoadNoiDungXuLy(infoUser.Username, infoUser.PhongBanId);
                    break;
            }
            return strValue;
        }

        private string LoadNoiDungXuLy(string username, int phongBanId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<option value=\"0\" phongbanid=\"-1\" username=\"\">Tất cả hành động</option>");
            sb.AppendFormat("<option value=\"0\" phongbanid=\"{0}\" username=\"{1}\">Nội dung xử lý (Cá nhân)</option>", phongBanId, username);
            sb.AppendFormat("<option value=\"0\" phongbanid=\"{0}\" username=\"{1}\">Nội dung xử lý (Cấp phòng ban)</option>", phongBanId, string.Empty);
            return sb.ToString();
        }

        private string GetHanhDongXuLy_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string SoThueBao,
            int NguoiTiepNhanId, int NguoiXuLyId, string maKhieuNai, string NgayTiepNhan_tu, string NgayTiepNhan_den, string ThoiGianCapNhat_tu, string ThoiGianCapNhat_den, string TrangThai,
            string startPageIndex, string pageSize)
        {
            int TotalRecords = _KhieuNaiImpl.QLKN_HanhDongXuLy_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                 Convert.ToInt32(LoaiKhieuNaiId), Convert.ToInt32(LinhVucChungId), Convert.ToInt32(LinhVucConId),
                Convert.ToInt32(PhongBanId), ConvertUtility.ToString(SoThueBao), NguoiTiepNhanId,
                NguoiXuLyId, ConvertUtility.ToString(maKhieuNai),
                ConvertUtility.ToString(NgayTiepNhan_tu), ConvertUtility.ToString(NgayTiepNhan_den), ConvertUtility.ToString(ThoiGianCapNhat_tu), ConvertUtility.ToString(ThoiGianCapNhat_den), ConvertUtility.ToInt32(TrangThai),
                Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
            return TotalRecords.ToString();
        }
        private string GetColorTrangThaiXuLy(int trangThaiXuLy)
        {
            string strValues = "";
            switch (trangThaiXuLy)
            {
                case (int)KhieuNai_TrangThai_Type.Chờ_xử_lý:
                    strValues = "#FF0000";
                    break;
                case (int)KhieuNai_TrangThai_Type.Đang_xử_lý:
                    strValues = "#FFFF00";
                    break;

                case (int)KhieuNai_TrangThai_Type.Chờ_đóng:
                    strValues = "#0095CC";
                    break;

                case (int)KhieuNai_TrangThai_Type.Đóng:
                    strValues = "#088A08";
                    break;
            }
            return strValues;
        }
        private string GetHtmlHanhDongXuLy(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string SoThueBao,
            int NguoiTiepNhanId, int NguoiXuLyId, string maKhieuNai, string NgayTiepNhan_tu, string NgayTiepNhan_den, string ThoiGianCapNhat_tu, string ThoiGianCapNhat_den, string TrangThai,
            string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            string strData = "";
            DataTable tab = _KhieuNaiImpl.QLKN_HanhDongXuLy_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                Convert.ToInt32(LoaiKhieuNaiId), Convert.ToInt32(LinhVucChungId), Convert.ToInt32(LinhVucConId),
                Convert.ToInt32(PhongBanId), ConvertUtility.ToString(SoThueBao), NguoiTiepNhanId,
                NguoiXuLyId, ConvertUtility.ToString(maKhieuNai),
                ConvertUtility.ToString(NgayTiepNhan_tu), ConvertUtility.ToString(NgayTiepNhan_den), ConvertUtility.ToString(ThoiGianCapNhat_tu), ConvertUtility.ToString(ThoiGianCapNhat_den), ConvertUtility.ToInt32(TrangThai),
                Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
            if (tab.Rows.Count > 0)
            {
                int temp = 0;
                foreach (DataRow row in tab.Rows)
                {
                    if (temp % 2 == 0)
                    {
                        strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                    }
                    else
                    {
                        strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                    }
                    strData += "        <td align=\"center\">" + row["STT"] + "</td>";
                    strData += "        <td class =\"nowrap\"  align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                    if (row["PhongBanXuLyId"].ToString().Equals(infoUser.PhongBanId.ToString()))
                    {
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != infoUser.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                    }
                    else
                    {
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=View\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                    }

                    strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoupChiTietKN('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                    strData += "        <td class =\"nowrap\" title=\"" + HttpUtility.HtmlEncode(row["Noidung"]) + "\" align=\"left\">" + Utility.CollapseString(row["Noidung"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["LUser"] + "'>" + row["LUser"] + "</a></td>";
                    strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                    strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                    strData += "        <td class =\"nowrap\" title=\"" + row["LoaiKhieuNai"] + "\"  align=\"left\">" + Utility.CollapseString(row["LoaiKhieuNai"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\" title=\"" + row["LinhVucChung"] + "\"  align=\"left\">" + Utility.CollapseString(row["LinhVucChung"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\" title=\"" + row["LinhVucCon"] + "\"  align=\"left\">" + Utility.CollapseString(row["LinhVucCon"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\" title=\"" + HttpUtility.HtmlEncode(row["HoTenLienHe"]) + "\"  align=\"left\">" + Utility.CollapseString(row["HoTenLienHe"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\" title=\"" + HttpUtility.HtmlEncode(row["DiaChiLienHe"]) + "\"  align=\"left\">" + Utility.CollapseString(row["DiaChiLienHe"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\" title=\"" + HttpUtility.HtmlEncode(row["DiaDiemXayRa"]) + "\"  align=\"center\">" + Utility.CollapseString(row["DiaDiemXayRa"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\" title=\"" + HttpUtility.HtmlEncode(row["ThoiGianXayRa"]) + "\"  align=\"center\">" + Utility.CollapseString(row["ThoiGianXayRa"].ToString(), 30) + "</td>";
                    strData += "        <td class =\"nowrap\"  align=\"center\">" + row["MaTinh"] + "</td>";
                    strData += "        <td class =\"nowrap\"  align=\"center\">" + row["MaQuan"] + "</td>";
                    strData += "        <td class =\"nowrap\"  align=\"center\">" + Convert.ToDateTime(row["LDate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                    strData += "        <td class =\"nowrap\" align=\"center\">" + row["IsAuto"] + "</td>";
                    strData += "        <td class =\"nowrap\" title=\"" + HttpUtility.HtmlEncode(row["NoiDungPA"]) + "\"  align=\"left\">" + Utility.CollapseString(row["NoiDungPA"].ToString(), 50) + "</td>";
                    strData += " </tr>";
                }
            }
            else
            {
                strData += "<tr class=\"rowB\"><td colspan =\"20\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
            }
            return strData;
        }

        private string GetItemDropListLinhVucChung(string loaiKhieuNaiID)
        {
            string strValues = "";
            var lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + loaiKhieuNaiID, "Sort");
            strValues += "<option value=\"0\">--Lĩnh vực chung--</option>";
            if (loaiKhieuNaiID != "0" && lstLinhVucChung.Count > 0)
            {
                foreach (LoaiKhieuNaiInfo info in lstLinhVucChung)
                {
                    strValues += "<option value=\"" + info.Id + "\">" + info.Name + "</option>";
                }
            }
            return strValues;
        }
        private string GetItemDropListLinhVucCon(string lihVucChungId)
        {
            string strValues = "";
            var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + lihVucChungId, "Sort");
            strValues += "<option value=\"0\">--Lĩnh vực con--</option>";
            if (lihVucChungId != "0" && lstLinhVucCon.Count > 0)
            {
                foreach (LoaiKhieuNaiInfo info in lstLinhVucCon)
                {
                    strValues += "<option value=\"" + info.Id + "\">" + info.Name + "</option>";
                }
            }
            return strValues;
        }

        #region Export Excel
        private Aspose.Cells.Style StyleCell()
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            Aspose.Cells.Cell cell = worksheet.Cells["A1"];
            Aspose.Cells.Style style = cell.GetStyle();

            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = Color.Black;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = Color.Black;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].Color = Color.Black;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = Color.Black;
            return style;
        }


        private Aspose.Cells.Style StyleCell(int number)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            Aspose.Cells.Cell cell = worksheet.Cells["A1"];
            Aspose.Cells.Style style = cell.GetStyle();

            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = Color.Black;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = Color.Black;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].Color = Color.Black;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = Color.Black;
            style.Custom = "dd/mm/yyyy hh:mm";
            return style;
        }

        private int AddContentToSheet(DataTable tab, int RowIndex, Worksheet sheet)
        {
            foreach (DataRow row in tab.Rows)
            {
                sheet.Cells[RowIndex, 0].PutValue(row["STT"]);
                sheet.Cells[RowIndex, 0].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 1].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(row["TrangThai"])).Replace("_", " "));
                sheet.Cells[RowIndex, 1].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 2].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10));
                sheet.Cells[RowIndex, 2].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 3].PutValue(row["SoThueBao"]);
                sheet.Cells[RowIndex, 3].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 4].PutValue(row["NoiDung"]);
                sheet.Cells[RowIndex, 4].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 5].PutValue(row["NguoiTiepNhan"]);
                sheet.Cells[RowIndex, 5].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 6].PutValue(row["NguoiXuLy"]);
                sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 7].PutValue(row["LoaiKhieuNai"]);
                sheet.Cells[RowIndex, 7].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 8].PutValue(row["LinhVucChung"]);
                sheet.Cells[RowIndex, 8].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 9].PutValue(row["LinhVucCon"]);
                sheet.Cells[RowIndex, 9].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 10].PutValue(row["HoTenLienHe"]);
                sheet.Cells[RowIndex, 10].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 11].PutValue(row["DiaChiLienHe"]);
                sheet.Cells[RowIndex, 11].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 12].PutValue(row["DiaDiemXayRa"]);
                sheet.Cells[RowIndex, 12].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 13].PutValue(row["ThoiGianXayRa"]);
                sheet.Cells[RowIndex, 13].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 14].PutValue(row["IsAuto"]);
                sheet.Cells[RowIndex, 14].SetStyle(StyleCell());

                sheet.Cells[RowIndex, 15].PutValue(row["NoiDungPA"]);
                sheet.Cells[RowIndex, 15].SetStyle(StyleCell());


                sheet.Cells[RowIndex, 16].PutValue(row["NgayTiepNhan"]);
                sheet.Cells[RowIndex, 16].SetStyle(StyleCell(-1));

                RowIndex++;


            }
            return RowIndex;
        }

        private int GetTotalPage(int TotalRecords, int pageSize)
        {
            int totalRemainder = TotalRecords % pageSize;
            int totalPage = (TotalRecords - totalRemainder) / pageSize;
            if (totalRemainder > 0)
            {
                totalPage = totalPage + 1;
            }
            return totalPage;

        }

        private string ExportHanhDongXuLy(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string SoThueBao,
            int NguoiTiepNhanId, int NguoiXuLyId, string maKhieuNai, string NgayTiepNhan_tu, string NgayTiepNhan_den, string ThoiGianCapNhat_tu, string ThoiGianCapNhat_den, string TrangThai,
            string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\HanhDongXuLy.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = _KhieuNaiImpl.QLKN_HanhDongXuLy_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
               Convert.ToInt32(LoaiKhieuNaiId), Convert.ToInt32(LinhVucChungId), Convert.ToInt32(LinhVucConId),
              Convert.ToInt32(PhongBanId), ConvertUtility.ToString(SoThueBao), NguoiTiepNhanId,
              NguoiXuLyId, ConvertUtility.ToString(maKhieuNai),
              ConvertUtility.ToString(NgayTiepNhan_tu), ConvertUtility.ToString(NgayTiepNhan_den), ConvertUtility.ToString(ThoiGianCapNhat_tu), ConvertUtility.ToString(ThoiGianCapNhat_den), ConvertUtility.ToInt32(TrangThai),
              Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + "");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable tab = _KhieuNaiImpl.QLKN_HanhDongXuLy_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                        Convert.ToInt32(LoaiKhieuNaiId), Convert.ToInt32(LinhVucChungId), Convert.ToInt32(LinhVucConId),
                        Convert.ToInt32(PhongBanId), ConvertUtility.ToString(SoThueBao), NguoiTiepNhanId,
                        NguoiXuLyId, ConvertUtility.ToString(maKhieuNai),
                        ConvertUtility.ToString(NgayTiepNhan_tu), ConvertUtility.ToString(NgayTiepNhan_den), ConvertUtility.ToString(ThoiGianCapNhat_tu), ConvertUtility.ToString(ThoiGianCapNhat_den), ConvertUtility.ToInt32(TrangThai),
                        i, Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "HanhDongXuLy" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
            }
            catch
            {
                return "";
            }
            return strValue;
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