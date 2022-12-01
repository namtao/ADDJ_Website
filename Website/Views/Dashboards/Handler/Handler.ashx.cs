using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using AIVietNam.Admin;
using System.Web.Services;
using AIVietNam.Core;
using System.Data;
using AIVietNam.GQKN.Impl;
using Website.AppCode;
using AIVietNam.GQKN.Entity;

namespace Website.Views.Dashboards.Handler
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    /// 
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler : IHttpHandler, IReadOnlySessionState
    {
        LichSuTruyVanImpl _LichSuTruyVanImpl = new LichSuTruyVanImpl();
        KhieuNaiImpl _KhieuNaiImpl = ServiceFactory.GetInstanceKhieuNai();
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
                    if (context.Request.QueryString["key"] != "7")
                    {
                        context.Response.Write(JSSerializer.Serialize(ProcessData(context.Request.QueryString["key"], infoUser, context)));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        {
                            string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                            string pageSize = context.Request.QueryString["pageSize"].ToString();
                            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(GetListLichSuTruyVan(infoUser.Username, startPageIndex, pageSize)));
                        }

                    }
                }
            }
        }
        private string ProcessData(string key, AdminInfo infoUser, HttpContext context)
        {
            //Utility.LogEvent(string.Format("ProcessData  User:{0}", infoUser.Username));
            string strValue = "";
            switch (key)
            {
                #region KhieuNaiChuaXuLy
                case "3":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        strValue = Count_KhieuNaiChoXuLy_Dashboard(infoUser.Username);                       
                    }
                    break;
                case "4":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {                        
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = Get_KhieuNaiChoXuLy_Dashboard_WithPage(infoUser.Username, startPageIndex, pageSize);                       
                    }
                    break;
                #endregion
                #region Khiếu Nại Cảnh báo
                case "5":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetKhieuNaiCanhBao_TotalRecords(infoUser.Username, startPageIndex, pageSize);
                    }
                    break;
                case "6":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        strValue = GetHtmlKhieuNaiCanhBao(infoUser.Username, startPageIndex, pageSize);
                    }
                    break;
                #endregion
                #region LichSuTruyVan
                //case "7"://Lấy danh sách Lịch sử truy vấn
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();
                //        strValue = GetHtmlLichSuTruyVan(infoUser.Username, startPageIndex, pageSize);
                //    }
                //    break;
                case "8"://Lấy tổng số khi load danh sách
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetLichSuTruyVan_TotalRecords(infoUser.Username, startPageIndex, pageSize);
                    }
                    break;
                #endregion

                #region Thông báo
                case "9":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["pageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string pageIndex = context.Request.QueryString["pageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetThongBao_Paged(infoUser.Username, pageIndex, pageSize);
                    }
                    break;

                #endregion
            }
            return strValue;
        }
        #region Thông báo
        /// <summary>
        /// Lay thông bao 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private string GetThongBao_Paged(string username, string pageIndex, string pageSize)
        {
            var thongBaoImpl = ServiceFactory.GetInstanceThongBao();
            var count = 0;
            const string selectString = "Id, TieuDe, NoiDung, IsNew, CDate";
            const string whereString = "Display = 1";
            const string orderByString = "CDate DESC";
            var ressults = thongBaoImpl.GetPageDecode(selectString, whereString, orderByString, int.Parse(pageIndex), int.Parse(pageSize), ref count);
            foreach (var item in ressults)
            {
                if (item.IsNew)
                    if ((DateTime.Now - item.CDate).TotalDays >= 7)
                        item.IsNew = false;
            }
            var js = new JavaScriptSerializer();
            var jsonItems = js.Serialize(ressults);
            var objJson = new { Items = jsonItems, Count = count };
            return js.Serialize(objJson);
        }

        #endregion
        #region Khieu nai dang xu ly

        private string Count_KhieuNaiChoXuLy_Dashboard(string NguoiXuLy)
        {
            int TotalRecords = _KhieuNaiImpl.Count_KhieuNaiChoXuLy_Dashboard(NguoiXuLy);
            return TotalRecords.ToString();
        }
        private string Get_KhieuNaiChoXuLy_Dashboard_WithPage(string NguoiXuLy, string startPageIndex, string pageSize)
        {
            string strData = "";
            DataTable tab = _KhieuNaiImpl.Get_KhieuNaiChoXuLy_Dashboard_WithPage(NguoiXuLy, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
            if (tab.Rows.Count > 0)
            {
                int temp = 0;

                string strReturnURL = "/Default.aspx";
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);
                var admin = LoginAdmin.AdminLogin();
                foreach (DataRow row in tab.Rows)
                {
                    int callCount = ConvertUtility.ToInt32(row["CallCount"], 1);
                    if (temp % 2 == 0)
                    {
                        if (callCount > 1)
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"blink rowA\">";
                        else
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                    }
                    else
                    {
                        if (callCount > 1)
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"blink rowB\">";
                        else
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                    }

                    if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                    {
                        if (row["NguoiXuLy"].ToString() != admin.Username)
                        {
                            strData += "        <td align=\"left\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        else
                        {
                            strData += "        <td align=\"left\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                    }
                    else
                    {
                        strData += "        <td align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                    }
                    strData += "        <td align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                    strData += "        <td align=\"center\"><a href=\"javascript:ShowPoupChiTietKN('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";

                    if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                    {
                        strData += "        <td align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                    }
                    else
                    {
                        strData += "        <td align=\"center\"></td>";
                    }

                    strData += "<td align=\"right\" style=\"width:20px;padding-right:5px;\" class='aui-dropdown-trigger'><a href=\"javascript:ShowContent(" + row["Id"] + ");\" class='icon-tools'><span><div id =\"divContentBlock_" + row["Id"] + "\" class =\"class-add-ContentBlock\"></div></span></a></td>";

                    strData += " </tr>";


                }
            }
            else
            {
                strData += "<tr class=\"rowB\"><td colspan =\"4\" align=\"center\">Chưa có khiếu nại đang xử lý !</td></tr>";
            }
            return strData;
        }

        #endregion

        #region Khiếu Nại Cảnh báo
        private string GetKhieuNaiCanhBao_TotalRecords(string userName, string startPageIndex, string pageSize)
        {
            int TotalRecords = _KhieuNaiImpl.KhieuNai_KhieuNaiCanhBaoTotalRecords_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
            return TotalRecords.ToString();
        }
        private string GetHtmlKhieuNaiCanhBao(string userName, string startPageIndex, string pageSize)
        {
            string strData = "";
            DataTable tab = _KhieuNaiImpl.KhieuNai_KhieuNaiCanhBao_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
            if (tab.Rows.Count > 0)
            {
                int temp = 0;
                foreach (DataRow row in tab.Rows)
                {
                    int callCount = ConvertUtility.ToInt32(row["CallCount"], 1);
                    if (temp % 2 == 0)
                    {
                        if (callCount > 1)
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"blink rowA\">";
                        else
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowA\">";
                    }
                    else
                    {
                        if (callCount > 1)
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"blink rowB\">";
                        else
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowB\">";
                    }
                    strData += "        <td align=\"center\"><a href='/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["Id"] + "&Mode=Process&ReturnUrl=/Default.aspx'>" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</td>";
                    strData += "        <td align=\"center\"><a href=\"javascript:ShowPoupChiTietKN('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"].ToString() + "</a></td>";
                    strData += "        <td align=\"center\">" + row["LoaiKhieuNai"].ToString() + "</td>";
                    strData += "        <td align=\"center\">" + row["NguoiXuLyTruoc"].ToString() + "</td>";
                    strData += "        <td align=\"center\">" + row["NguoiXuLy"].ToString() + "</td>";

                    strData += " </tr>";
                }

            }
            else
            {
                strData += "<tr class=\"rowB\"><td colspan =\"5\" align=\"center\">Chưa có khiếu nại cảnh báo !</td></tr>";
            }
            return strData;
        }
        #endregion

        #region LichSuTruyVan
        private string GetLichSuTruyVan_TotalRecords(string userName, string startPageIndex, string pageSize)
        {
            int TotalRecords = _LichSuTruyVanImpl.QLKN_LichSuTruyVan_GetAllWithPadding_TotalRecords(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
            return TotalRecords.ToString();
        }
        private string GetHtmlLichSuTruyVan(string userName, string startPageIndex, string pageSize)
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

                    strData += "        <td align=\"left\"><a href=\"/Views/QLKhieuNai/TruyVan.aspx?id=" + row["Id"] + "\" title=\"Chọn tiêu trí truy vấn\">" + row["Name"] + "</a></td>";

                    strData += "        <td align=\"center\">" + Convert.ToDateTime(row["CDate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";

                    strData += " </tr>";


                }
            }
            else
            {
                strData += "<tr class=\"rowB\"><td colspan =\"2\" align=\"center\">Chưa có lịch sử truy vấn</td></tr>";
            }
            return strData;
        }

        private List<LichSuTruyVanInfo> GetListLichSuTruyVan(string userName, string startPageIndex, string pageSize)
        {
            string strData = "";
            List<LichSuTruyVanInfo> list = new List<LichSuTruyVanInfo>();
            DataTable tab = _LichSuTruyVanImpl.QLKN_LichSuTruyVan_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
            if (tab.Rows.Count > 0)
            {
                //int temp = 0;
                strData += "\"Data\": [";
                foreach (DataRow row in tab.Rows)
                {
                    LichSuTruyVanInfo info = new LichSuTruyVanInfo();
                    info.Id = Convert.ToInt32(row["Id"]);
                    info.Name = row["Name"].ToString();
                    info.UserName = row["UserName"].ToString();
                    info.CDate = Convert.ToDateTime(row["CDate"]);
                    info.Data = row["Data"].ToString();
                    list.Add(info);
                }

            }

            return list;
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