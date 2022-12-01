using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using AIVietNam.GQKN.Entity;
using System.Text;
using System.Configuration;
using AIVietNam.GQKN.Impl;
using System.Text.RegularExpressions;
using AIVietNam.Core;
using System.Data;
using AIVietNam.Admin;
using Website.AppCode;
using System.Transactions;
using Website.AppCode.Controller;
using System.IO;
using Aspose.Cells;
using System.Drawing;
using Ionic.Zip;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for HandlerTruyVan
    /// </summary>
    /// 
    /// 
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class HandlerTruyVan : IHttpHandler, IReadOnlySessionState
    {
        PhongBanImpl _PhongBanImpl = new PhongBanImpl();
        LichSuTruyVanImpl _LichSuTruyVanImpl = new LichSuTruyVanImpl();
        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request.QueryString["key"].ToString();
            System.Web.Script.Serialization.JavaScriptSerializer JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";
            //context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            if (!string.IsNullOrEmpty(context.Request.QueryString["key"]))
            {
                if (context.Request.QueryString["key"] != "8")
                {
                    int intKey = 0;
                    int.TryParse(context.Request.QueryString["Key"], out intKey);
                    if (intKey == 9) // Xuất Excel
                    {
                        string userName = "";
                        int DoitacId = 0;
                        AdminInfo userInfo = (AdminInfo)context.Session[Constant.SessionNameAccountAdmin];
                        if (userInfo != null)
                        {
                            userName = userInfo.Username;
                            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Truy_vấn_khiếu_nại_trên_toàn_bộ_hệ_thống))
                            {
                                DoitacId = userInfo.DoiTacId;
                            }
                        }

                        string jsonExp = context.Request.Form["data"];
                        List<KhieuNai_TruyVanInfo> listExp = new List<KhieuNai_TruyVanInfo>();
                        KhieuNai_TruyVanObject listObjectExp = Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanObject>(jsonExp);
                        if (listObjectExp.object_list != null)
                        {
                            foreach (object info in listObjectExp.object_list)
                            {
                                listExp.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanInfo>(info.ToString()));
                            }
                        }
                        if (listExp.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                            {
                                try
                                {
                                    CustomMessage retMesage = ExportExcelTruyVanV2(listExp, DoitacId);
                                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retMesage));
                                }
                                catch (Exception ex)
                                {
                                    Helper.GhiLogs(ex);

                                    // Thông điệp gửi tới Client xảy ra lỗi
                                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new CustomMessage() { Code = CustomCode.KhongXacDinh, Message = "Có lỗi xảy ra, vui lòng thử lại sau", Data = ex.Message }));
                                }
                            }
                        }
                    }
                    else
                    {
                        context.Response.Write(ProcessData(context.Request.QueryString["key"], context));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
                    {
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(_LichSuTruyVanImpl.LichSuTruyVanGetByID(Convert.ToInt32(context.Request.QueryString["id"])).Data));
                    }

                }
            }
        }

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

        private string ProcessData(string key, HttpContext context)
        {
            string strValue = "";
            string userName = "";
            int DoitacId = 0;
            AdminInfo infoUser = (AdminInfo)context.Session[Constant.SessionNameAccountAdmin];
            if (infoUser != null)
            {
                userName = infoUser.Username;
                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Truy_vấn_khiếu_nại_trên_toàn_bộ_hệ_thống))
                {
                    DoitacId = infoUser.DoiTacId;
                }
            }
            switch (key)
            {
                case "1"://Lấy danh sách Fill len gridview Truy vấn
                    string json = context.Request.Form["JSONParam"];
                    if (json == null)
                    {
                        return "";
                    }
                    List<KhieuNai_TruyVanInfo> list = new List<KhieuNai_TruyVanInfo>();
                    KhieuNai_TruyVanObject listObject = Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanObject>(json);
                    if (listObject.object_list != null)
                    {
                        foreach (var info in listObject.object_list)
                        {
                            list.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanInfo>(info.ToString()));
                        }
                    }
                    if (list.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(context.Request.Form["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.Form["pageSize"]))
                        {
                            string startPageIndex = context.Request.Form["startPageIndex"].ToString();
                            string pageSize = context.Request.Form["pageSize"].ToString();
                            strValue = GetHtmlTruyVan(context, list, DoitacId, startPageIndex, pageSize);
                        }
                    }
                    break;
                case "2"://Lấy tổng số bản ghi
                         //if (Config.IsCallSolr)
                         //{
                    List<KhieuNai_TruyVanInfo> listTotalRecords = new List<KhieuNai_TruyVanInfo>();
                    KhieuNai_TruyVanObject listObjectTotalRecords = Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanObject>(context.Request.Form["data"]);
                    if (listObjectTotalRecords.object_list != null)
                    {
                        foreach (var info in listObjectTotalRecords.object_list)
                        {
                            listTotalRecords.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanInfo>(info.ToString()));
                        }
                    }
                    if (listTotalRecords.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        {
                            string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                            string pageSize = context.Request.QueryString["pageSize"].ToString();
                            strValue = GetTotalRecords(listTotalRecords, DoitacId, startPageIndex, pageSize);
                        }
                    }
                    //}
                    //else
                    //{
                    //    strValue = "0";                        
                    //}
                    break;
                case "3"://Lấy danh sách Lịch sử truy vấn
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetHtmlLichSuTruyVan(userName, startPageIndex, pageSize);
                    }
                    break;
                case "4"://Lấy tổng số khi load danh sách
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetLichSuTruyVan_TotalRecords(userName, startPageIndex, pageSize);
                    }
                    break;
                case "5"://Them mới lịch sử theo dõi
                    if (!string.IsNullOrEmpty(context.Request.Form["data"]))
                    {
                        string Data = context.Request.Form["data"];
                        string Name = context.Request.QueryString["Name"].ToString();
                        strValue = AddLichSuTruyVan(Name, Data, userName);
                    }
                    break;
                case "6"://Xóa lịch sử theo dõi
                    if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
                    {
                        string id = context.Request.QueryString["id"].ToString();
                        strValue = DeleteLichSuTruyVan(id);
                    }
                    break;
                case "7"://Kiểm tra du liệu trùng khi thêm mới
                    if (!string.IsNullOrEmpty(context.Request.QueryString["Name"]))
                    {
                        string Name = context.Request.QueryString["Name"].ToString();
                        strValue = CheckIsExist(Name);
                    }
                    break;
                case "9": // Xuat file Excel     
                    string jsonExp = context.Request.Form["data"];
                    List<KhieuNai_TruyVanInfo> listExp = new List<KhieuNai_TruyVanInfo>();
                    KhieuNai_TruyVanObject listObjectExp = Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanObject>(jsonExp);
                    if (listObjectExp.object_list != null)
                    {
                        foreach (var info in listObjectExp.object_list)
                        {
                            listExp.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<KhieuNai_TruyVanInfo>(info.ToString()));
                        }
                    }
                    if (listExp.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        {
                            string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                            string pageSize = context.Request.QueryString["pageSize"].ToString();
                            strValue = ExportExcelTruyVan(listExp, DoitacId, startPageIndex, pageSize);
                        }
                    }
                    break;
                case "10"://Load du lieu vao Multi Select 
                    //string listParent = context.Request.Form["data"];
                    if (!string.IsNullOrEmpty(context.Request.QueryString["tenTruong"]))
                    {
                        string tenTruong = context.Request.QueryString["tenTruong"];
                        string cap = context.Request.QueryString["cap"];
                        int tinhthanhid = 0;
                        int.TryParse(context.Request.QueryString["tinhthanhid"], out tinhthanhid);
                        int loaikhieunai0id = 0;
                        int.TryParse(context.Request.QueryString["loaikhieunai0id"], out loaikhieunai0id);
                        int loaikhieunai1id = 0;
                        int.TryParse(context.Request.QueryString["loaikhieunai1id"], out loaikhieunai1id);
                        int loaikhieunai2id = 0;
                        int.TryParse(context.Request.QueryString["loaikhieunai2id"], out loaikhieunai2id);
                        strValue = LoadContentToMultiSelect(tenTruong, cap, tinhthanhid, loaikhieunai0id, loaikhieunai1id, loaikhieunai2id);
                    }
                    break;
            }
            return strValue;
        }

        #region Load du lieu vao Multi Select   
        private string LoadContentToMultiSelect(string column, string cap, int tinhthanhid, int loaikhieunai0id, int loaikhieunai1id, int loaikhieunai2id)
        {
            try
            {
                string values = "";
                if (column == "NguoiTiepNhan" || column == "NguoiXuLy")
                {
                    //var obj = ServiceFactory.GetInstanceNguoiSuDung();
                    //var lst = obj.Suggestion(search);
                    //context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
                    values = GetStrNguoiSuDung();
                }
                else if (column == "LoaiKhieuNaiId" || column == "LinhVucChungId" || column == "LinhVucConId")
                {
                    values = GetStrValueLoaiKN(cap, loaikhieunai0id, loaikhieunai1id, loaikhieunai2id);

                }
                else if (column == "PhongBanTiepNhanId" || column == "PhongBanXuLyId")
                {
                    values = GetStrPhongBan();
                    //var obj = ServiceFactory.GetInstancePhongBan();
                    //var lst = obj.Suggestion(search);
                    //context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
                }
                else if (column == "MaTinhId" || column == "MaQuanId")
                {
                    values = GetStrValueProvince(cap, tinhthanhid);
                    //var obj = ServiceFactory.GetInstanceProvince();
                    //var lst = obj.Suggestion(search);
                    //context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
                }
                else if (column == "DoiTacId" || column == "DoiTacXuLyId")
                {
                    values = GetStrDoiTac();
                    //var obj = ServiceFactory.GetInstanceDoiTac();
                    //var lst = obj.Suggestion(search);
                    //context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
                }

                else if (column == "DoUuTien")
                {

                    foreach (byte i in Enum.GetValues(typeof(KhieuNai_DoUuTien_Type)))
                    {
                        values += "<li id=\"checkbox-li-\"" + (int)i + " class=\"item\">";
                        values += "     <input type=\"checkbox\" id=\"checkbox" + (int)i + "\" value=\"" + (int)i + "\" name=\"" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " ") + "\" class=\"checkbox-item\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " ");
                        values += "</li>";
                    }

                }
                else if (column == "TrangThai")
                {

                    foreach (byte i in Enum.GetValues(typeof(KhieuNai_TrangThai_Type)))
                    {
                        values += "<li id=\"checkbox-li-\"" + (int)i + " class=\"item\">";
                        values += "     <input type=\"checkbox\" id=\"checkbox" + (int)i + "\" value=\"" + (int)i + "\" name=\"" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), i).Replace("_", " ") + "\" class=\"checkbox-item\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), i).Replace("_", " ");
                        values += "</li>";
                    }


                }
                else if (column == "HTTiepNhan")
                {

                    foreach (byte i in Enum.GetValues(typeof(KhieuNai_HTTiepNhan_Type)))
                    {
                        values += "<li id=\"checkbox-li-\"" + (int)i + " class=\"item\">";
                        values += "     <input type=\"checkbox\" id=\"checkbox" + (int)i + "\" value=\"" + (int)i + "\" name=\"" + Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), i).Replace("_", " ") + "\" class=\"checkbox-item\">" + Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), i).Replace("_", " ");
                        values += "</li>";
                    }

                }
                else if (column == "DoHaiLong")
                {

                    foreach (int i in Enum.GetValues(typeof(KhieuNai_DoHaiLong_Type)))
                    {
                        values += "<li id=\"checkbox-li-\"" + (int)i + " class=\"item\">";
                        values += "     <input type=\"checkbox\" id=\"checkbox" + i + "\" value=\"" + i + "\" name=\"" + Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), i).Replace("_", " ") + "\" class=\"checkbox-item\">" + Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), i).Replace("_", " ");
                        values += "</li>";
                    }


                }
                return values;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetStrDoiTac()
        {
            try
            {
                string values = "";
                DoiTacImpl _DoiTacImpl = new DoiTacImpl();
                List<DoiTacInfo> lst = _DoiTacImpl.SuggestionGetAllList("");
                if (lst.Count > 0)
                {
                    foreach (DoiTacInfo info in lst)
                    {
                        values += "<li id=\"checkbox-li-" + info.Id + "\" class=\"item\">";
                        values += "     <input type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.TenDoiTac + "\" class=\"checkbox-item\">&nbsp;" + info.TenDoiTac;
                        values += "</li>";
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetStrPhongBan()
        {
            try
            {
                string values = "";
                PhongBanImpl _PhongBanImpl = new PhongBanImpl();
                List<PhongBanInfo> lst = _PhongBanImpl.SuggestionGetAllList("");
                if (lst.Count > 0)
                {
                    foreach (PhongBanInfo info in lst)
                    {
                        values += "<li id=\"checkbox-li-" + info.Id + "\" class=\"item\">";
                        values += "     <input type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                        values += "</li>";
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetStrNguoiSuDung()
        {
            try
            {
                string values = "";
                NguoiSuDungImpl _NguoiSuDungImpl = new NguoiSuDungImpl();
                List<NguoiSuDungInfo> lst = _NguoiSuDungImpl.Suggestion("");
                if (lst.Count > 0)
                {
                    foreach (NguoiSuDungInfo info in lst)
                    {
                        values += "<li id=\"checkbox-li-" + info.Id + "\" class=\"item\">";
                        values += "     <input type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.TenTruyCap + "\" class=\"checkbox-item\">&nbsp;" + info.TenTruyCap;
                        values += "</li>";
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetStrValueProvince(string cap, int tinhthanhid)
        {
            try
            {
                string values = "";
                ProvinceImpl _ProvinceImpl = new ProvinceImpl();
                if (cap == "0")
                {
                    List<ProvinceInfo> lstProvince = _ProvinceImpl.SuggestionGetListByListLevelNbr("", "1");
                    if (lstProvince.Count > 0)
                    {
                        foreach (ProvinceInfo info in lstProvince)
                        {
                            values += "<li  id=\"checkbox-li-" + info.Id + "\" class=\"item\">";
                            values += "     <input type=\"checkbox\" tinhthanhid=" + info.Id + " id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                            values += "</li>";
                        }
                    }

                }
                else if (cap == "1")
                {
                    List<ProvinceInfo> lstProvince;
                    if (tinhthanhid == 0)
                    {
                        lstProvince = _ProvinceImpl.SuggestionGetListByListLevelNbr("", "1");
                    }
                    else
                    {
                        lstProvince = _ProvinceImpl.SuggestionGetListByListLevelNbr("", tinhthanhid.ToString());
                    }
                    if (lstProvince.Count > 0)
                    {
                        int temp = 0;
                        foreach (ProvinceInfo info in lstProvince)
                        {
                            temp++;
                            values += "<li id=\"checkbox-li-" + info.Id + "\" class=\"item\">";

                            List<ProvinceInfo> lstProvinceChild1 = _ProvinceImpl.SuggestionGetListByListParent("", info.Id.ToString());
                            if (lstProvinceChild1.Count > 0)
                            {
                                if (temp == 1)
                                {
                                    values += "     <a id = \"a-checkbox" + info.Id + "\" href=\"javascript:ClickShowHinde(" + info.Id + ");\" class = \"ClickShowHinde\"><img id = \"img-checkbox" + info.Id + "\" src=\"/images/icons/Down.png\" /></a><input disabled=\"disabled\" type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                                    values += "<ul id =\"" + info.Id.ToString() + "\" style=\"display: block;\">";
                                }
                                else
                                {
                                    values += "     <a id = \"a-checkbox" + info.Id + "\" href=\"javascript:ClickShowHinde(" + info.Id + ");\" class = \"ClickShowHinde\"><img id = \"img-checkbox" + info.Id + "\" src=\"/images/icons/Next.png\" /></a><input disabled=\"disabled\" type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                                    values += "<ul id =\"" + info.Id.ToString() + "\" style=\"display: none;\">";
                                }
                                foreach (ProvinceInfo infoChild1 in lstProvinceChild1)
                                {
                                    values += "<li  id=\"checkbox-li-" + infoChild1.Id + "\" class=\"item\">";
                                    values += "     <input type=\"checkbox\" TinhThanhId=" + info.Id + " id=\"checkbox" + infoChild1.Id + "\" value=\"" + infoChild1.Id + "\" name=\"" + infoChild1.Name + "\" class=\"checkbox-item\">&nbsp;" + infoChild1.Name;
                                    values += "</li>";
                                }
                                values += "</ul>";
                            }
                            values += "</li>";
                        }
                    }
                }

                return values;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetStrValueLoaiKN(string cap, int loaikhieunai0id, int loaikhieunai1id, int loaikhieunai2id)
        {
            try
            {
                string values = "";
                LoaiKhieuNaiImpl _LoaiKhieuNaiImpl = new LoaiKhieuNaiImpl();
                if (cap == "0")
                {
                    List<LoaiKhieuNaiInfo> lstLoaiKN = _LoaiKhieuNaiImpl.SuggestionGetListByListParent("", 0, loaikhieunai0id, loaikhieunai1id, loaikhieunai2id);
                    if (lstLoaiKN.Count > 0)
                    {
                        foreach (LoaiKhieuNaiInfo info in lstLoaiKN)
                        {
                            values += "<li id=\"checkbox-li-" + info.Id + "\" class=\"item\">";
                            values += "     <input loaikhieunai0id=" + info.Id + " type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                            values += "</li>";
                        }
                    }

                }
                else if (cap == "1")
                {

                    List<LoaiKhieuNaiInfo> lstLoaiKN = _LoaiKhieuNaiImpl.SuggestionGetListByListParent("", 0, loaikhieunai0id, loaikhieunai1id, loaikhieunai2id);
                    if (lstLoaiKN.Count > 0)
                    {
                        int temp = 0;
                        foreach (LoaiKhieuNaiInfo info in lstLoaiKN)
                        {
                            temp++;
                            values += "<li id=\"checkbox-li-" + info.Id + "\" class=\"item\">";

                            //List<LoaiKhieuNaiInfo> lstLoaiKNChild1 = _LoaiKhieuNaiImpl.SuggestionGetListByListParent("", 1, loaikhieunai0id, loaikhieunai1id, loaikhieunai2id);
                            List<LoaiKhieuNaiInfo> lstLoaiKNChild1 = _LoaiKhieuNaiImpl.SuggestionGetListByListParent("", 1, loaikhieunai0id, info.Id, loaikhieunai2id);
                            if (lstLoaiKNChild1.Count > 0)
                            {
                                if (temp == 1)
                                {
                                    values += "     <a loaikhieunai1id=" + info.Id + " id = \"a-checkbox" + info.Id + "\" href=\"javascript:ClickShowHinde(" + info.Id + ");\" class = \"ClickShowHinde\"><img id = \"img-checkbox" + info.Id + "\" src=\"/images/icons/Down.png\" /></a><input disabled=\"disabled\" type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                                    values += "<ul id =\"" + info.Id.ToString() + "\" style=\"display: block;\">";
                                }
                                else
                                {
                                    values += "     <a loaikhieunai1id=" + info.Id + " id = \"a-checkbox" + info.Id + "\" href=\"javascript:ClickShowHinde(" + info.Id + ");\" class = \"ClickShowHinde\"><img id = \"img-checkbox" + info.Id + "\" src=\"/images/icons/Next.png\" /></a><input disabled=\"disabled\" type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                                    values += "<ul id =\"" + info.Id.ToString() + "\" style=\"display: none;\">";
                                }
                                foreach (LoaiKhieuNaiInfo infoChild1 in lstLoaiKNChild1)
                                {
                                    values += "<li id=\"checkbox-li-" + infoChild1.Id + "\" class=\"item\">";
                                    values += "     <input loaikhieunai1id=" + info.Id + " type=\"checkbox\" id=\"checkbox" + infoChild1.Id + "\" value=\"" + infoChild1.Id + "\" name=\"" + infoChild1.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + infoChild1.Name;
                                    values += "</li>";
                                }
                                values += "</ul>";
                            }
                            values += "</li>";
                        }
                    }
                }
                else if (cap == "2")
                {
                    List<LoaiKhieuNaiInfo> lstLoaiKN = _LoaiKhieuNaiImpl.SuggestionGetListByListParent("", 0, loaikhieunai0id, loaikhieunai1id, loaikhieunai2id);
                    if (lstLoaiKN.Count > 0)
                    {
                        int temp = 0;
                        foreach (LoaiKhieuNaiInfo info in lstLoaiKN)
                        {
                            temp++;
                            values += "<li id=\"checkbox-li-" + info.Id + "\" class=\"item\">";

                            List<LoaiKhieuNaiInfo> lstLoaiKNChild1 = _LoaiKhieuNaiImpl.SuggestionGetListByListParent("", 1, loaikhieunai0id, info.Id, loaikhieunai2id);
                            if (lstLoaiKNChild1.Count > 0)
                            {
                                if (temp == 1)
                                {
                                    values += "     <a loaikhieunai2id=" + info.Id + " id = \"a-checkbox" + info.Id + "\" href=\"javascript:ClickShowHinde(" + info.Id + ");\" class = \"ClickShowHinde\"><img id = \"img-checkbox" + info.Id + "\" src=\"/images/icons/Next.png\" /></a><input disabled=\"disabled\" type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                                    values += "<ul id =\"" + info.Id.ToString() + "\" style=\"display: block;\">";
                                }
                                else
                                {
                                    values += "     <a loaikhieunai2id=" + info.Id + " id = \"a-checkbox" + info.Id + "\" href=\"javascript:ClickShowHinde(" + info.Id + ");\" class = \"ClickShowHinde\"><img id = \"img-checkbox" + info.Id + "\" src=\"/images/icons/Down.png\" /></a><input disabled=\"disabled\" type=\"checkbox\" id=\"checkbox" + info.Id + "\" value=\"" + info.Id + "\" name=\"" + info.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + info.Name;
                                    values += "<ul id =\"" + info.Id.ToString() + "\" style=\"display: none;\">";
                                }
                                foreach (LoaiKhieuNaiInfo infoChild1 in lstLoaiKNChild1)
                                {
                                    values += "<li id=\"checkbox-li-" + infoChild1.Id + "\" class=\"item\">";
                                    values += "     <a loaikhieunai2id=" + info.Id + " id = \"a-checkbox" + infoChild1.Id + "\" href=\"javascript:ClickShowHinde(" + infoChild1.Id + ");\" class = \"clickShow\"><img id = \"img-checkbox" + infoChild1.Id + "\" src=\"/images/icons/Next.png\" /></a><input disabled=\"disabled\" type=\"checkbox\" id=\"checkbox" + infoChild1.Id + "\" value=\"" + infoChild1.Id + "\" name=\"" + infoChild1.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + infoChild1.Name;
                                    List<LoaiKhieuNaiInfo> lstLoaiKNChild2 = _LoaiKhieuNaiImpl.SuggestionGetListByListParent("", 1, loaikhieunai0id, loaikhieunai1id, infoChild1.Id);
                                    if (lstLoaiKNChild2.Count > 0)
                                    {
                                        values += "<ul id =\"" + infoChild1.Id.ToString() + "\" style=\"display: none;\">";
                                        foreach (LoaiKhieuNaiInfo infoChild2 in lstLoaiKNChild2)
                                        {
                                            values += "<li id=\"checkbox-li-" + infoChild2.Id + "\" class=\"item\">";
                                            values += "     <input loaikhieunai2id=" + info.Id + " type=\"checkbox\" id=\"checkbox" + infoChild2.Id + "\" value=\"" + infoChild2.Id + "\" name=\"" + infoChild2.Name.Replace(",", " ") + "\" class=\"checkbox-item\">&nbsp;" + infoChild2.Name;
                                            values += "</li>";
                                        }
                                        values += "</ul>";
                                    }
                                    values += "</li>";
                                }
                                values += "</ul>";
                            }
                            values += "</li>";
                        }
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region TruyVanKhieuNai
        private string GetTotalRecords(List<KhieuNai_TruyVanInfo> listTruyVan, int DoitacId, string startPageIndex, string pageSize)
        {

            int totalRecords = 0;
            string keyword = "";
            try
            {
                var result = from lst in listTruyVan
                             where lst.PhepToan == "LIKE"
                             select lst;
                List<KhieuNai_TruyVanInfo> list = result.ToList();
                if (list.Count > 0)
                {
                    foreach (KhieuNai_TruyVanInfo info in list)
                    {
                        keyword = info.GiaTri;
                    }
                }
                KhieuNai_TruyVanImpl _KhieuNai_TruyVanImpl = new KhieuNai_TruyVanImpl();
                string URL_SOLR = AIVietNam.Core.Config.ServerSolr + "GQKN/";
                totalRecords = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolrTotalRecord(URL_SOLR, keyword, listTruyVan, DoitacId, 0, 1);
                return totalRecords.ToString();
            }
            catch (Exception exception)
            {
                Utility.LogEvent(exception);
                return "0";
            }
        }

        private List<DataItem> GetObjectTruyVan(HttpContext context, List<KhieuNaiSolrInfo> listKetQuaTruyVan, int DoitacId, string startPageIndex, string pageSize)
        {
            List<DataItem> lstRow = new List<DataItem>();
            int stt = 1;
            if (listKetQuaTruyVan != null && listKetQuaTruyVan.Count > 0)
            {
                AdminInfo infoUser = LoginAdmin.AdminLogin();
                string strReturnURL = "/Views/QLKhieuNai/TruyVan.aspx";
                foreach (KhieuNaiSolrInfo item in listKetQuaTruyVan)
                {
                    DataItem dataItemInfo = new DataItem();
                    dataItemInfo.Id = item.Id.ToString();
                    dataItemInfo.STT = stt++;
                    //itemKN.TrangThai = "<span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(item.TrangThai)) + "; width: 15px; height: 10px;\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                    dataItemInfo.TrangThai = BindTinhTrangXuLy(item.TrangThai, false, item.NgayQuaHanPhongBanXuLy);
                    dataItemInfo.MaPAKN = GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)item.Id, 10);
                    dataItemInfo.DoUuTien = Enum.GetName(typeof(KhieuNai_DoUuTien_Type), item.DoUuTien).Replace("_", " ");
                    dataItemInfo.SoThueBao = item.SoThueBao.ToString();

                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.PhongBanXuLyId != infoUser.PhongBanId || item.NguoiXuLy != infoUser.Username)
                        {
                            dataItemInfo.Id = "<a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&archive=" + item.ArchiveId + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                        else
                        {
                            dataItemInfo.Id = "<a href=\"javascript:CheckXuLyKhieuNai(" + item.Id + ",'/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&archive=" + item.ArchiveId + "&Mode=Process','" + strReturnURL + "')\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                    }
                    else
                    {
                        if (item.PhongBanXuLyId == infoUser.PhongBanId)
                        {
                            dataItemInfo.Id = "<a href=\"javascript:CheckXuLyKhieuNai(" + item.Id + ",'/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&archive=" + item.ArchiveId + "&Mode=Process','" + strReturnURL + "')\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                        else
                        {
                            dataItemInfo.Id = "<a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&archive=" + item.ArchiveId + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                    }
                    dataItemInfo.SoThueBao = "<a class='ShowChiTiet_" + item.Id + "' href=\"javascript:ShowPoupChiTietKN('" + item.Id + "','" + item.ArchiveId + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + item.SoThueBao + "</a>";

                    dataItemInfo.LoaiKhieuNai = item.LoaiKhieuNai;
                    dataItemInfo.LinhVucChung = item.LinhVucChung;
                    dataItemInfo.LinhVucCon = item.LinhVucCon;
                    dataItemInfo.NguoiTiepNhan = item.NguoiTiepNhan;
                    dataItemInfo.NguoiTienXuLyCap1 = item.NguoiTienXuLyCap1;
                    dataItemInfo.NguoiTienXuLyCap2 = item.NguoiTienXuLyCap2;
                    dataItemInfo.NguoiTienXuLyCap3 = item.NguoiTienXuLyCap3;
                    dataItemInfo.NguoiXuLy = item.NguoiXuLy;
                    if (!string.IsNullOrEmpty(item.NgayQuaHanPhongBanXuLy.ToString()))
                    {
                        dataItemInfo.NgayQuaHanSort = item.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        dataItemInfo.NgayQuaHanSort = "";
                    }

                    if (!string.IsNullOrEmpty(item.NgayDongKN.ToString()) && item.NgayDongKN.Year != 9999)
                    {
                        dataItemInfo.NgayDongKhieuNai = item.NgayDongKN.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        dataItemInfo.NgayDongKhieuNai = "";
                    }

                    dataItemInfo.NoiDungPA = item.NoiDungPA.ToString();
                    dataItemInfo.SDTLienHe = item.SDTLienHe;
                    dataItemInfo.HoTenLienHe = item.HoTenLienHe;
                    dataItemInfo.DiaChiLienHe = item.DiaChiLienHe;
                    dataItemInfo.ThoiGianXayRa = item.ThoiGianXayRa;
                    dataItemInfo.MaQuanSuCo = item.MaQuan;
                    dataItemInfo.MaTinhSuCo = item.MaTinh;
                    dataItemInfo.DiaDiemSuCo = item.DiaDiemXayRa;

                    // Edit : HaiPH 07/07/2014 : Thêm ngày tiếp nhận, ngày quá hạn phòng ban và ngày quá hạn toàn trình
                    dataItemInfo.NgayTiepNhan = item.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss");
                    dataItemInfo.NgayQuaHanPhongBan = item.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss");
                    dataItemInfo.NgayQuaHanToanTrinh = item.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss");
                    dataItemInfo.LDate = item.LDate.ToString("dd/MM/yyyy HH:mm:ss");
                    dataItemInfo.GhiChu = item.GhiChu;
                    dataItemInfo.CallCount = item.CallCount;
                    dataItemInfo.DoHaiLong = Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), item.DoHaiLong).Replace("_", " ");
                    dataItemInfo.NoiDungXuLyDongKN = item.NoiDungXuLyDongKN;

                    // dataItemInfo.LoaiLoi = "[Dữ liệu mẫu]";
                    dataItemInfo.LoaiLoi = item.ChiTietLoiId.ToString();


                    lstRow.Add(dataItemInfo);
                }
            }
            return lstRow;

        }

        private string GetHtmlTruyVan(HttpContext context, List<KhieuNai_TruyVanInfo> listTruyVan, int DoitacId, string startPageIndex, string pageSize)
        {
            StringBuilder sb = new StringBuilder();
            int totalRecords = 0;
            string keyword = string.Empty;
            var result = from lst in listTruyVan
                         where lst.PhepToan == "LIKE"
                         select lst;
            List<KhieuNai_TruyVanInfo> list = result.ToList();
            List<DataItem> listReturn = new List<DataItem>();
            if (list.Count > 0)
            {
                foreach (KhieuNai_TruyVanInfo info in list)
                {
                    keyword = info.GiaTri;
                }
            }
            KhieuNai_TruyVanImpl _KhieuNai_TruyVanImpl = new KhieuNai_TruyVanImpl();
            string URL_SOLR = AIVietNam.Core.Config.ServerSolr + "GQKN/";
            List<KhieuNaiSolrInfo> lstKhieuNaiInfo = new List<KhieuNaiSolrInfo>();
            try
            {
                lstKhieuNaiInfo = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolr(URL_SOLR, keyword, listTruyVan, DoitacId, Convert.ToInt32(startPageIndex) - 1, Convert.ToInt32(pageSize), ref totalRecords);
                System.Diagnostics.Debugger.Launch();
                listReturn = GetObjectTruyVan(context, lstKhieuNaiInfo, DoitacId, startPageIndex, pageSize);
            }
            catch (Exception exception)
            {
                Utility.LogEvent(exception);
                return "-1";
            }
            DateTruyVan dateTruyVan = new DateTruyVan();
            dateTruyVan.page = 1;
            dateTruyVan.total = 10;
            dateTruyVan.rows = listReturn;
            return Newtonsoft.Json.JsonConvert.SerializeObject(dateTruyVan);
        }

        private string GetHtmlTruyVan1(HttpContext context, List<KhieuNai_TruyVanInfo> listTruyVan, int DoitacId, string startPageIndex, string pageSize)
        {
            StringBuilder sb = new StringBuilder();
            int totalRecords = 0;
            string keyword = "";
            var result = from lst in listTruyVan
                         where lst.PhepToan == "LIKE"
                         select lst;
            List<KhieuNai_TruyVanInfo> list = result.ToList();
            if (list.Count > 0)
            {
                foreach (KhieuNai_TruyVanInfo info in list)
                {
                    keyword = info.GiaTri;
                }
            }
            KhieuNai_TruyVanImpl _KhieuNai_TruyVanImpl = new KhieuNai_TruyVanImpl();
            string URL_SOLR = AIVietNam.Core.Config.ServerSolr + "GQKN/";
            List<KhieuNaiSolrInfo> lstKhieuNaiInfo = new List<KhieuNaiSolrInfo>();
            try
            {
                lstKhieuNaiInfo = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolr(URL_SOLR, keyword, listTruyVan, DoitacId, Convert.ToInt32(startPageIndex) - 1, Convert.ToInt32(pageSize), ref totalRecords);

                if (lstKhieuNaiInfo != null && lstKhieuNaiInfo.Count > 0)
                {
                    int stt = 0;
                    int temp = 0;

                    foreach (KhieuNaiSolrInfo info in lstKhieuNaiInfo)
                    {
                        stt++;
                        if (temp % 2 == 0)
                        {
                            sb.Append("<tr id =\"row-" + info.Id + "\" class=\"rowA\">");
                        }
                        else
                        {
                            sb.Append("<tr id =\"row-" + info.Id + "\" class=\"rowB\">");
                        }
                        //sb.Append("        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + info.Id + "\" id =\"checkbox" + info.Id + "\" type =\"checkbox\" /></td>";
                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + (((Convert.ToInt32(startPageIndex) - 1) * Convert.ToInt32(pageSize)) + stt).ToString() + "</td>");
                        sb.Append("        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(info.TrangThai)) + "; width: 15px; height: 10px;\"></span></td>");
                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)info.Id, 10) + "</td>");

                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), info.DoUuTien).Replace("_", " ") + "</td>");
                        sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoupChiTietKN('" + info.Id + "');\" title=\"Hiển thị Thông tin chi tiết khiếu nại\">" + info.SoThueBao + "</a></td>");
                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.LoaiKhieuNai + "</td>");
                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.LinhVucChung + "</td>");
                        sb.Append("        <td class =\"nowrap\" align=\"left\">" + info.LinhVucCon + "</td>");

                        if (!string.IsNullOrEmpty(info.NgayTiepNhan.ToString()))
                        {
                            sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                        }
                        else
                        {
                            sb.Append("        <td class =\"nowrap\" align=\"center\"></td>");
                        }
                        sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + info.NguoiTiepNhan + "'>" + info.NguoiTiepNhan + "</a></td>");

                        sb.Append("        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(info.PhongBanXuLyId.ToString()) + "</td>");

                        sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + info.NguoiXuLy + "'>" + info.NguoiXuLy + "</a></td>");
                        //sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + info.NguoiTienXuLyCap1 + "'>" + info.NguoiTienXuLyCap1 + "</a></td>";

                        //sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.NguoiTiepNhan + "</td>";
                        //sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.NguoiXuLy + "</td>";
                        //sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.NguoiTienXuLyCap1 + "</td>";

                        if (!string.IsNullOrEmpty(info.NgayQuaHanPhongBanXuLy.ToString()))
                        {
                            sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                        }
                        else
                        {
                            sb.Append("        <td class =\"nowrap\" align=\"center\"></td>");
                        }

                        //if (!string.IsNullOrEmpty(info.NgayTraLoiKN.ToString()) && info.NgayTraLoiKN.Year != 9999)
                        //{
                        //    sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.NgayTraLoiKN.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                        //}
                        //else
                        //{
                        //    sb.Append("        <td class =\"nowrap\" align=\"center\"></td>");
                        //}

                        if (!string.IsNullOrEmpty(info.NgayDongKN.ToString()) && info.NgayDongKN.Year != 9999)
                        {
                            sb.Append("        <td class =\"nowrap\" align=\"center\">" + info.NgayDongKN.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                        }
                        else
                        {
                            sb.Append("        <td class =\"nowrap\" align=\"center\"></td>");
                        }


                        sb.Append("        <td class =\"nowrap\" align=\"left\">" + info.NoiDungPA + "</td>");
                        sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", info.HoTenLienHe);
                        sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", info.DiaChiLienHe);
                        sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", info.ThoiGianXayRa);
                        sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", info.MaQuan);
                        sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", info.MaTinh);
                        sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", info.DiaDiemXayRa);
                        sb.Append(" </tr>");


                    }
                }
                else
                {
                    sb.Append("<tr class=\"rowB\"><td colspan =\"13\" align=\"center\">Không tìm thây bản ghi nào</td></tr>");
                }

            }
            catch (Exception exception)
            {
                Utility.LogEvent(exception);
                return "-1";
            }
            return sb.ToString();
        }
        private string GetNamePhongBan(string id)
        {
            try
            {
                string strValues = "";
                if (!string.IsNullOrEmpty(id))
                {

                    PhongBanInfo info = _PhongBanImpl.QLKN_PhongBangetByID(Convert.ToInt32(id));
                    if (info != null)
                    {
                        strValues = info.Name;
                    }
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetColorTrangThaiXuLy(int trangThaiXuLy)
        {
            try
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
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 07/07/2014
        ///// Todo : Hiển thị màu sắc theo trạng thái của khiếu nại
        /////     Trường hợp nếu khiếu  nại đã quá hạn toàn trình thì hiển thị màu quá hạn toàn trình
        ///// </summary>
        ///// <param name="trangThai"></param>
        ///// <param name="ngayQuaHanToanTrinh"></param>
        ///// <returns></returns>
        //private string BindTinhTrangXuLy(byte trangThai, DateTime ngayQuaHanToanTrinh)
        //{
        //    if (trangThai == (byte)KhieuNai_TrangThai_Type.Đóng)
        //        return string.Format("<span style='border: 1pt solid #CCC; background: green; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
        //    else
        //    {
        //        if (ngayQuaHanToanTrinh < DateTime.Now)
        //        {
        //            return string.Format("<span style='border: 1pt solid #CCC; background: #999; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
        //        }
        //        else
        //        {
        //            if (trangThai == (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý)
        //                return string.Format("<span style='border: 1pt solid #CCC; background: red; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
        //            else if (trangThai == (byte)KhieuNai_TrangThai_Type.Chờ_đóng)
        //                return string.Format("<span style='border: 1pt solid #CCC; background: #0095CC; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
        //            else
        //                return string.Format("<span style='border: 1pt solid #CCC; background: yellow; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
        //        }
        //    }
        //}

        #endregion
        #region LichSuTruyVan
        private LichSuTruyVanInfo GetDataJsonTruyVanById(string Id)
        {

            LichSuTruyVanInfo info = _LichSuTruyVanImpl.LichSuTruyVanGetByID(Convert.ToInt32(Id));

            return info;
        }
        private string CheckIsExist(string Name)
        {
            try
            {
                List<LichSuTruyVanInfo> lst = _LichSuTruyVanImpl.GetListLichSuTruyVanByName(Name);
                if (lst.Count > 0)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string AddLichSuTruyVan(string Name, string Data, string userName)
        {
            string strValue = "0";
            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                //Activity
                LichSuTruyVanInfo itemLichSuTruyVan = new LichSuTruyVanInfo();
                itemLichSuTruyVan.Name = Name;
                itemLichSuTruyVan.Data = Data;

                itemLichSuTruyVan.UserName = userName;

                itemLichSuTruyVan.CDate = DateTime.Now;

                //Báo lỗi và không thực hiện chức năng

                strValue = ServiceFactory.GetInstanceLichSuTruyVan().Add(itemLichSuTruyVan).ToString();
                //scope.Complete();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
            //}
            return strValue;
        }
        private string DeleteLichSuTruyVan(string Id)
        {
            string strValue = "0";
            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                strValue = ServiceFactory.GetInstanceLichSuTruyVan().Delete(Convert.ToInt32(Id)).ToString();
                //scope.Complete();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
            //}
            return strValue;
        }
        private string GetLichSuTruyVan_TotalRecords(string userName, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _LichSuTruyVanImpl.QLKN_LichSuTruyVan_GetAllWithPadding_TotalRecords(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlLichSuTruyVan(string userName, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _LichSuTruyVanImpl.QLKN_LichSuTruyVan_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
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
                        //strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";



                        strData += "        <td class =\"nowrap\" align=\"left\"><a href=\"javascript:SelectRow('" + row["ID"] + "');\" title=\"Chọn tiêu trí truy vấn\">" + row["Name"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["UserName"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["CDate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:fnDeleteLichSuTruyVan('" + row["ID"] + "');\" title=\"Chọn tiêu trí truy vấn\" class=\"mybtn\"><span class=\"del_file\">Xóa</span></a></td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"5\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion

        #region Process

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

        private int AddContentToSheet(List<KhieuNaiSolrInfo> lstKhieuNaiInfo, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (KhieuNaiSolrInfo info in lstKhieuNaiInfo)
                {
                    sheet.Cells[RowIndex, 0].PutValue(RowIndex - 4);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCell());

                    string trangThai = string.Empty;
                    trangThai = Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(info.TrangThai)).Replace("_", " ");

                    if (info.TrangThai != (int)KhieuNai_TrangThai_Type.Đóng && info.NgayQuaHanPhongBanXuLy <= DateTime.Now)
                    {
                        trangThai = "Quá hạn";
                    }

                    sheet.Cells[RowIndex, 1].PutValue(trangThai);
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 2].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)info.Id, 10));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCell());

                    if (info.DoUuTien == 0) info.DoUuTien = 1; // Trạng thái thông thường, edited by DuongDv
                    sheet.Cells[RowIndex, 3].PutValue(Enum.GetName(typeof(KhieuNai_DoUuTien_Type), Convert.ToByte(info.DoUuTien)).Replace("_", " "));
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 4].PutValue(info.SoThueBao);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 5].PutValue(info.LoaiKhieuNai);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 6].PutValue(info.LinhVucChung);
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(info.LinhVucCon);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());

                    if (info.NoiDungPA != null && (info.NoiDungPA.Length > Constant.LIMIT_CHARACTER_EXCEL_CELL))
                    {
                        string shortContent = info.NoiDungPA.Substring(0, Constant.LIMIT_CHARACTER_EXCEL_CELL - 5) + " ...";
                        sheet.Cells[RowIndex, 8].PutValue(shortContent);
                        sheet.Cells[RowIndex, 8].SetStyle(StyleCell());
                    }
                    else
                    {
                        sheet.Cells[RowIndex, 8].PutValue(info.NoiDungPA);
                        sheet.Cells[RowIndex, 8].SetStyle(StyleCell());
                    }

                    if (info.NoiDungXuLyDongKN.Length > Constant.LIMIT_CHARACTER_EXCEL_CELL)
                    {
                        string shortContent = info.NoiDungXuLyDongKN.Substring(0, Constant.LIMIT_CHARACTER_EXCEL_CELL - 5) + " ...";
                        sheet.Cells[RowIndex, 9].PutValue(shortContent);
                        sheet.Cells[RowIndex, 9].SetStyle(StyleCell());
                    }
                    else
                    {
                        sheet.Cells[RowIndex, 9].PutValue(info.NoiDungXuLyDongKN);
                        sheet.Cells[RowIndex, 9].SetStyle(StyleCell());
                    }

                    try
                    {
                        sheet.Cells[RowIndex, 10].PutValue(ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(info.PhongBanXuLyId)).Name);
                        sheet.Cells[RowIndex, 10].SetStyle(StyleCell());
                    }
                    catch (Exception ex)
                    {
                        // Utility.LogEvent(ex);
                        sheet.Cells[RowIndex, 10].PutValue("");
                        sheet.Cells[RowIndex, 10].SetStyle(StyleCell());
                    }
                    sheet.Cells[RowIndex, 11].PutValue(info.NguoiTiepNhan);
                    sheet.Cells[RowIndex, 11].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 12].PutValue(info.NguoiXuLy);
                    sheet.Cells[RowIndex, 12].SetStyle(StyleCell());

                    //sheet.Cells[RowIndex, 12].PutValue(info.NguoiTienXuLyCap1);
                    //sheet.Cells[RowIndex, 12].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 13].PutValue(Convert.ToDateTime(info.NgayTiepNhan).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 13].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 14].PutValue(Convert.ToDateTime(info.NgayQuaHanPhongBanXuLy).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 14].SetStyle(StyleCell());

                    if (info.NgayDongKN.Year != 9999)
                    {
                        sheet.Cells[RowIndex, 15].PutValue(Convert.ToDateTime(info.NgayDongKN).ToString("dd/MM/yyyy HH:mm:ss"));
                        sheet.Cells[RowIndex, 15].SetStyle(StyleCell());
                    }
                    else
                    {
                        sheet.Cells[RowIndex, 15].PutValue(string.Empty);
                        sheet.Cells[RowIndex, 15].SetStyle(StyleCell());
                    }

                    sheet.Cells[RowIndex, 16].PutValue(info.SDTLienHe);
                    sheet.Cells[RowIndex, 16].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 17].PutValue(info.HoTenLienHe);
                    sheet.Cells[RowIndex, 17].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 18].PutValue(info.DiaChiLienHe);
                    sheet.Cells[RowIndex, 18].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 19].PutValue(info.ThoiGianXayRa);
                    sheet.Cells[RowIndex, 19].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 20].PutValue(info.MaQuan);
                    sheet.Cells[RowIndex, 20].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 21].PutValue(info.MaTinh);
                    sheet.Cells[RowIndex, 21].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 22].PutValue(info.DiaDiemXayRa);
                    sheet.Cells[RowIndex, 22].SetStyle(StyleCell());

                    // Nếu khiếu nại đã đóng thì mới hiển thị giá trị độ hài lòng của khách hàng
                    if (info.TrangThai == (int)KhieuNai_TrangThai_Type.Đóng)
                    {
                        sheet.Cells[RowIndex, 23].PutValue(Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), Convert.ToByte(info.DoHaiLong)).Replace("_", " "));
                    }

                    sheet.Cells[RowIndex, 23].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 24].PutValue(info.GhiChu);
                    sheet.Cells[RowIndex, 24].SetStyle(StyleCell());

                    RowIndex++;


                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                try
                {
                    Utility.LogEvent(ex);
                    string message = string.Format("Dòng số: {0}", RowIndex);
                    Helper.GhiLogs("TruyVanNangCao", message);
                    sheet.Cells[RowIndex, 25].PutValue("Dòng bị lỗi");
                }
                catch { }
                return RowIndex++;
                // return -1;  
            }
        }

        private int GetTotalPage(int TotalRecords, int pageSize)
        {
            try
            {
                int totalRemainder = TotalRecords % pageSize;
                int totalPage = (TotalRecords - totalRemainder) / pageSize;
                if (totalRemainder > 0)
                {
                    totalPage = totalPage + 1;
                }
                return totalPage;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }

        #endregion

        #region Export
        private string ExportExcelTruyVan(List<KhieuNai_TruyVanInfo> listTruyVan, int DoitacId, string startPageIndex, string pageSize)
        {
            string strValue = string.Empty;
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai_TruyVanNangCao.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                KhieuNai_TruyVanImpl _KhieuNai_TruyVanImpl = new KhieuNai_TruyVanImpl();
                string URL_SOLR = AIVietNam.Core.Config.ServerSolr + "GQKN/";
                int totalRecords = 0;
                string keyword = "";
                IEnumerable<KhieuNai_TruyVanInfo> result = from lst in listTruyVan
                                                           where lst.PhepToan == "LIKE"
                                                           select lst;
                List<KhieuNai_TruyVanInfo> list = result.ToList();
                if (list.Count > 0)
                {
                    foreach (KhieuNai_TruyVanInfo info in list)
                    {
                        keyword = info.GiaTri;
                    }
                }
                //Save the Excel file.
                totalRecords = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolrTotalRecord(URL_SOLR, keyword, listTruyVan, DoitacId, Convert.ToInt32(startPageIndex) - 1, Convert.ToInt32(pageSize));

                if (totalRecords > 0)
                {
                    if (totalRecords > 60000)
                    {
                        return "-1";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                        {
                            sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " truy vấn");
                            Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                            style.IsTextWrapped = true;
                            sheet.Cells[0, 0].SetStyle(style);
                        }
                        int totalPage = GetTotalPage(totalRecords, Convert.ToInt32(pageSize));
                        for (int i = 1; i <= totalPage; i++)
                        {
                            if (RowIndex >= 5) // Bắt buộc RowIndex phải >= 5
                            {
                                List<KhieuNaiSolrInfo> lstKhieuNaiInfo = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolr(URL_SOLR, keyword, listTruyVan, DoitacId, i - 1, Convert.ToInt32(pageSize), ref totalRecords);
                                RowIndex = AddContentToSheet(lstKhieuNaiInfo, RowIndex, sheet);
                            }
                        }
                    }

                }

                string fileName = "DanhSachKhieuNai_TruyVan" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
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
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
            return strValue;
        }
        #endregion
        private CustomMessage ExportExcelTruyVanV2(List<KhieuNai_TruyVanInfo> listTruyVan, int DoitacId)
        {
            HttpContext context = HttpContext.Current;

            KhieuNai_TruyVanImpl _KhieuNai_TruyVanImpl = new KhieuNai_TruyVanImpl();
            string URL_SOLR = Config.ServerSolr + "GQKN/";
            int totalRecords = 0;
            string keyword = string.Empty;
            IEnumerable<KhieuNai_TruyVanInfo> result = from lst in listTruyVan
                                                       where lst.PhepToan == "LIKE"
                                                       select lst;
            List<KhieuNai_TruyVanInfo> list = result.ToList();
            if (list.Count > 0)
            {
                foreach (KhieuNai_TruyVanInfo info in list)
                {
                    keyword = info.GiaTri;
                }
            }
            // Save the Excel file.
            // Tổng số lượng bản ghi
            int pageSize = 15000;
            totalRecords = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolrTotalRecord(URL_SOLR, keyword, listTruyVan, DoitacId, 0, pageSize);

            if (totalRecords > 0)
            {
                if (totalRecords > 65000)
                {
                    return new CustomMessage(CustomCode.DuLieuKhongHopLe, "Số lượng kết quả quá lớn");
                }
                else
                {
                    List<KhieuNaiSolrInfo> lstKhieuNaiInfo = null; // Lưu giữ giá trị
                    int totalPage = GetTotalPage(totalRecords, pageSize);
                    string tempFolder = string.Format("/ExportExcel/Excel/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    List<string> listName = new List<string>();

                    for (int i = 0; i < totalPage; i++)
                    {
                        try
                        {
                            lstKhieuNaiInfo = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolr(URL_SOLR, keyword, listTruyVan, DoitacId, i, pageSize, ref totalRecords);
                        }
                        catch (Exception ex)
                        {
                            Helper.GhiLogs(ex);
                            lstKhieuNaiInfo = _KhieuNai_TruyVanImpl.QueryKhieuNaiFromSolr(URL_SOLR, keyword, listTruyVan, DoitacId, i, pageSize, ref totalRecords);
                        }


                        // Xuất Excel
                        string fileNameTemp = "DanhSachKhieuNai_TruyVanNangCao_2.xlsx";
                        string pathFile = context.Server.MapPath("~/ExportExcel/Template/" + fileNameTemp);

                        WorkbookDesigner designer = new WorkbookDesigner();
                        LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);
                        designer.Workbook = new Workbook(pathFile, loadOptions);

                        var tempData = lstKhieuNaiInfo.Select((v, Index) => new
                        {
                            STT = Index + 1,
                            Id = v.Id,

                            // Tính toán trạng thái
                            TrangThai = string.Empty.GetData<string>(() =>
                            {
                                string trangThai = Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(v.TrangThai)).Replace("_", " ");

                                if (v.TrangThai != (int)KhieuNai_TrangThai_Type.Đóng && v.NgayQuaHanPhongBanXuLy <= DateTime.Now)
                                {
                                    trangThai = "Quá hạn";
                                }
                                return trangThai;
                            }),
                            MaKhieuNai = v.Id.ToString("PA-0000000000"),

                            // Đội ưu tiên
                            DoUuTien = string.Empty.GetData(() =>
                            {
                                try
                                {
                                    return Enum.GetName(typeof(KhieuNai_DoUuTien_Type), Convert.ToByte(v.DoUuTien)).Replace("_", " ");
                                }
                                catch
                                {
                                    return "Thông thường";
                                }
                            }),

                            SoThueBao = v.SoThueBao,
                            LoaiKhieuNai = v.LoaiKhieuNai,
                            LinhVucChung = v.LinhVucChung,
                            LinhVucCon = v.LinhVucCon,
                            NoiDungPA = string.Empty.GetData<string>(() =>
                            {
                                if (v.NoiDungPA != null && v.NoiDungPA.Length > Constant.LIMIT_CHARACTER_EXCEL_CELL) return v.NoiDungPA.Substring(0, Constant.LIMIT_CHARACTER_EXCEL_CELL - 5) + " ...";
                                else return v.NoiDungPA;
                            }),
                            NoiDungDongKN = string.Empty.GetData<string>(() =>
                            {
                                if (v.NoiDungXuLyDongKN != null && v.NoiDungXuLyDongKN.Length > Constant.LIMIT_CHARACTER_EXCEL_CELL) return v.NoiDungXuLyDongKN.Substring(0, Constant.LIMIT_CHARACTER_EXCEL_CELL - 5) + " ...";
                                else return v.NoiDungXuLyDongKN;
                            }),

                            // v.NoiDungXuLyDongKN,
                            PhongBanXuLy = string.Empty.GetData<string>(() =>
                            {
                                int phongBanXuLyId = v.PhongBanXuLyId;
                                string keyCache = "PhongBanXuLy_" + phongBanXuLyId;
                                return HttpContext.Current.Cache.Data<string>(keyCache, 9000, () =>
                                {
                                    try
                                    {
                                        return new PhongBanImpl().GetInfo(phongBanXuLyId).Name;
                                    }
                                    catch
                                    {
                                        Helper.GhiLogs(string.Format("Không tìm thấy phòng ban xử lý Id = {0}", phongBanXuLyId));
                                        return string.Format("Không xác định");
                                    }
                                });
                            }),
                            NguoiTiepNhan = v.NguoiTiepNhan,
                            NguoiXyLy = v.NguoiXuLy,
                            NgayTiepNhan = v.NgayTiepNhan,
                            NgayQuaHan = v.NgayQuaHan,
                            NgayDongKN = string.Empty.GetData<object>(() =>
                            {
                                if (v.NgayDongKN.Year == 9999) return null;
                                return v.NgayDongKN;
                            }),

                            SoDienThoaiLienHe = v.SDTLienHe,
                            HoTenLienHe = v.HoTenLienHe,
                            DiaChiLienHe = v.DiaChiLienHe,
                            ThoiGianSuCo = v.ThoiGianXayRa,
                            MaQuan = v.MaQuan,
                            MaTinh = v.MaTinh,
                            DiaDiemXayRaSuCo = v.DiaDiemXayRa,
                            DoHaiLong = string.Empty.GetData<object>(() =>
                            {
                                if (v.TrangThai == (int)KhieuNai_TrangThai_Type.Đóng)
                                {
                                    return Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), Convert.ToByte(v.DoHaiLong)).Replace("_", " ");
                                }
                                return null;
                            }),
                            GhiChu = v.GhiChu
                        });

                        designer.SetDataSource("DanhSachChiTiet", tempData.ToList());
                        designer.Process();

                        string fileName = string.Concat("DanhSachKhieuNai_TruyVan", "_", DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"), "_Page", i + 1, ".xls");
                        string pathSave = HttpContext.Current.Server.MapPath(string.Concat("~", tempFolder));

                        if (!Directory.Exists(pathSave)) Directory.CreateDirectory(pathSave);
                        string fullFileName = string.Concat(pathSave, fileName);
                        designer.Workbook.Save(fullFileName, SaveFormat.Xlsx);

                        listName.Add(string.Concat(tempFolder, fileName));
                    }

                    int isZip = 0;
                    int.TryParse(context.Request.QueryString["isZip"], out isZip);
                    if (isZip > 0) // Nén đám file đó lại
                    {
                        string pathSaveFile = tempFolder + string.Concat("DanhSachKhieuNai_TruyVan", "_", DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"), ".zip");

                        using (ZipFile zip = new ZipFile())
                        {
                            foreach (string fileName in listName)
                            {
                                string fullName = context.Server.MapPath(string.Concat("~", fileName));
                                zip.AddFile(fullName, string.Empty);
                            }


                            zip.Save(context.Server.MapPath("~" + pathSaveFile));
                        }

                        return new CustomMessage(CustomCode.OK, "Xử lý dữ liệu thành công", pathSaveFile);
                    }
                    else return new CustomMessage(CustomCode.OK, "Xử lý dữ liệu thành công", listName, listName.Count);
                }
            }
            else // Không có dữ liệu
            {
                return new CustomMessage(CustomCode.KhongCoDuLieu, "Không có dữ liệu");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
    internal class DateTruyVan
    {
        public int page { get; set; }
        public int total { get; set; }
        public List<DataItem> rows { get; set; }
    }
    internal class DataItem
    {
        public Int64 STT { get; set; }
        public string CheckAll { get; set; }
        public string Id { get; set; }
        public string DoUuTien { get; set; }
        public string TrangThai { get; set; }
        public string MaPAKN { get; set; }
        public string SoThueBao { get; set; }
        public string NoiDungPA { get; set; }
        public string NoiDungXuLyDongKN { get; set; }
        public string LoaiKhieuNai { get; set; }
        public string LinhVucChung { get; set; }
        public string LinhVucCon { get; set; }
        public string PhongBanTiepNhan { get; set; }
        public string NguoiTiepNhan { get; set; }
        public string NguoiXuLyTruoc { get; set; }
        public string PhongBanXuLy { get; set; }
        public string NguoiXuLy { get; set; }
        public string NguoiDuocPhanHoi { get; set; }
        public string NguoiTienXuLyCap1 { get; set; }
        public string NguoiTienXuLyCap2 { get; set; }
        public string NguoiTienXuLyCap3 { get; set; }

        public string NgayTiepNhan { get; set; }
        public string NgayTiepNhanSort { get; set; }
        public string NgayQuaHanSort { get; set; }
        public string NgayQuaHanPhongBan { get; set; }
        public string NgayQuaHanToanTrinh { get; set; }
        public string NgayDongKhieuNai { get; set; }
        public string SDTLienHe { get; set; }
        public string HoTenLienHe { get; set; }
        public string DiaChiLienHe { get; set; }
        public string ThoiGianXayRa { get; set; }
        public string MaQuanSuCo { get; set; }
        public string MaTinhSuCo { get; set; }
        public string DiaDiemSuCo { get; set; }
        public string NgayQuaHanPhongBanXuLySort { get; set; }
        public string IsPhanViec { get; set; }
        public string LDate { get; set; }
        public string GhiChu { get; set; }
        public int CallCount { get; set; }
        public string DoHaiLong { get; set; }
        public string LoaiLoi { get; set; }
    }
}
