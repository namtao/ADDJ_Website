using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using System.Text;
using Website.AppCode.Controller;

namespace Website.Views.KhieuNai.UC
{
    public partial class ucFileDinhKem : System.Web.UI.UserControl
    {
        public string listFile;
        public int KhieuNaiId { get; set; }
        public string Mode { get; set; }
        private Boolean IsPageRefresh = false;
        protected bool taomoi = false, sua = false, xoa = false;
        protected string xoacuaminh;
        private int ArchiveId = 0;
        private AdminInfo login;
        protected void Page_Load(object sender, EventArgs e)
        {
            login = LoginAdmin.AdminLogin();
            KhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["MaKN"]);
            Mode = ConvertUtility.ToString(Request.QueryString["Mode"]);
            ArchiveId = ConvertUtility.ToInt32(Request.QueryString["archive"]);
            if (Mode != "View")
            {
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tạo_file_đính_kèm))
                    taomoi = true;
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                    xoa = true;
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_file_đính_kèm))
                    sua = true;
            }
            xoacuaminh = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_file_đính_kèm_của_mình).ToString();
        }

        
        public void FillFileDinhKemGrid1()
        {
            string whereClause = string.Format("KhieuNaiId = {0}", KhieuNaiId);
            List<KhieuNai_FileDinhKemInfo> objFileDinhKemList = new List<KhieuNai_FileDinhKemInfo>();
            objFileDinhKemList = ServiceFactory.GetInstanceKhieuNai_FileDinhKem(ArchiveId).GetListDynamic("", whereClause, "");
            var sb = new StringBuilder();
            if (objFileDinhKemList != null && objFileDinhKemList.Count > 0) {
                var showEdit = false;
                var showDel = false;

                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_file_trên_hệ_thống))
                {
                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_file_toàn_hệ_thống))
                    {
                        

                        foreach (var item in objFileDinhKemList)
                        {
                            if (Mode != "View")
                            {
                                if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_file_đính_kèm_của_mình) && login.Username == item.CUser)
                                    || BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                                    showDel = true;
                                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_file_đính_kèm) && login.Username == item.CUser)
                                    showEdit = true;
                            }

                            sb.AppendFormat("<tr class=\"gridFooterRow\" id='fRow{0}'><td><span>{1}</span></td>", item.Id, item.CUser);
                            sb.AppendFormat("<td><div style='word-wrap: break-word; width:200px'><a style='text-decoration: underline' href=\"{0}\"  target='_blank'>{1}</a> </div></td>",
                                AIVietNam.Core.Config.DomainDownload + item.URLFile, item.TenFile);
                            if (item.Status == 1)
                                sb.Append("<td><span>Khách hàng</span></td>");
                            else if (item.Status == 2)
                                sb.Append("<td><span>Bảo mật</span></td>");
                            else
                                sb.Append("<td><span></span></td>");

                            sb.AppendFormat("<td><span>{0} KB</span></td>", item.KichThuoc);
                            sb.AppendFormat("<td><div style='word-wrap: break-word; width:250px'>{0}</div></td>", item.GhiChu);
                            sb.AppendFormat("<td><span>{0}</span></td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));

                            var btnDelStr = showDel ? string.Format(" <a href='javascript:void(0)' onclick='DeleteFile({0})' title='Xóa file đính kèm: {1}' class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>", item.Id, item.TenFile) : "";
                            var btnEditStr = showEdit ? string.Format("<a href='javascript:void(0)' onclick='EditFile({0})' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>", item.Id, item.TenFile) : "";
                            sb.AppendFormat("<td><span style='text-align:center'>{0}</span></td>", btnEditStr + btnDelStr);
                        }
                    }
                    else if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_file_tại_phòng_ban_xử_lý_KN))
                    {
                        var KhieuNaiItem = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetListDynamic("PhongBanXuLyId", "Id=" + KhieuNaiId, "");
                        if (KhieuNaiItem != null && KhieuNaiItem.Count > 0 && KhieuNaiItem[0].PhongBanXuLyId == login.PhongBanId)
                        {
                            foreach (var item in objFileDinhKemList)
                            {
                                if (Mode != "View")
                                {
                                    if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_file_đính_kèm_của_mình) && login.Username == item.CUser)
                                        || BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                                        showDel = true;
                                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_file_đính_kèm) && login.Username == item.CUser)
                                        showEdit = true;
                                }

                                sb.AppendFormat("<tr class=\"gridFooterRow\" id='fRow{0}'><td><span>{1}</span></td>", item.Id, item.CUser);
                                sb.AppendFormat("<td><div style='word-wrap: break-word; width:200px'><a style='text-decoration: underline' href=\"{0}\"  target='_blank'>{1}</a> </div></td>",
                                    AIVietNam.Core.Config.DomainDownload + item.URLFile, item.TenFile);
                                if (item.Status == 1)
                                    sb.Append("<td><span>Khách hàng</span></td>");
                                else if (item.Status == 2)
                                    sb.Append("<td><span>Bảo mật</span></td>");
                                else
                                    sb.Append("<td><span></span></td>");

                                sb.AppendFormat("<td><span>{0} KB</span></td>", item.KichThuoc);
                                sb.AppendFormat("<td><div style='word-wrap: break-word; width:250px'>{0}</div></td>", item.GhiChu);
                                sb.AppendFormat("<td><span>{0}</span></td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));

                                var btnDelStr = showDel ? string.Format(" <a href='javascript:void(0)' onclick='DeleteFile({0})' title='Xóa file đính kèm: {1}' class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>", item.Id, item.TenFile) : "";
                                var btnEditStr = showEdit ? string.Format("<a href='javascript:void(0)' onclick='EditFile({0})' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>", item.Id, item.TenFile) : "";
                                sb.AppendFormat("<td><span style='text-align:center'>{0}</span></td>", btnEditStr + btnDelStr);
                            }
                        }
                    }
                    else
                    {
                        

                        foreach (var item in objFileDinhKemList)
                        {
                            if ((item.Status == 1) || (item.Status == 2 && item.CUser == login.Username))
                            {
                                if (Mode != "View")
                                {
                                    if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_file_đính_kèm_của_mình) && login.Username == item.CUser)
                                        || BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                                        showDel = true;
                                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_file_đính_kèm) && login.Username == item.CUser)
                                        showEdit = true;
                                }

                                sb.AppendFormat("<tr class=\"gridFooterRow\" id='fRow{0}'><td><span>{1}</span></td>", item.Id, item.CUser);
                                sb.AppendFormat("<td><div style='word-wrap: break-word; width:200px'><a style='text-decoration: underline' href=\"{0}\"  target='_blank'>{1}</a> </div></td>",
                                    AIVietNam.Core.Config.DomainDownload + item.URLFile, item.TenFile);
                                if (item.Status == 1)
                                    sb.Append("<td><span>Khách hàng</span></td>");
                                else if (item.Status == 2)
                                    sb.Append("<td><span>Bảo mật</span></td>");
                                else
                                    sb.Append("<td><span></span></td>");

                                sb.AppendFormat("<td><span>{0} KB</span></td>", item.KichThuoc);
                                sb.AppendFormat("<td><div style='word-wrap: break-word; width:250px'>{0}</div></td>", item.GhiChu);
                                sb.AppendFormat("<td><span>{0}</span></td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));

                                var btnDelStr = showDel ? string.Format(" <a href='javascript:void(0)' onclick='DeleteFile({0})' title='Xóa file đính kèm: {1}' class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>", item.Id, item.TenFile) : "";
                                var btnEditStr = showEdit ? string.Format("<a href='javascript:void(0)' onclick='EditFile({0})' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>", item.Id, item.TenFile) : "";
                                sb.AppendFormat("<td><span style='text-align:center'>{0}</span></td>", btnEditStr + btnDelStr);
                            }
                        }
                    }
                }
            }                                                                                         
            else {
                sb.AppendFormat(@"<tr id='nodata' class='gridRow'>
                    <td colspan='8'><span>Chưa có dữ liệu...</span>
                    </td>
                </tr>");
            }
            listFile = sb.ToString();
        }


        public void FillFileDinhKemGrid()
        {
            string whereClause = string.Format("KhieuNaiId = {0}", KhieuNaiId);
            List<KhieuNai_FileDinhKemInfo> objFileDinhKemList = new List<KhieuNai_FileDinhKemInfo>();
            objFileDinhKemList = ServiceFactory.GetInstanceKhieuNai_FileDinhKem(ArchiveId).GetListDynamic("", whereClause, "");
            var sb = new StringBuilder();
            if (objFileDinhKemList != null && objFileDinhKemList.Count > 0)
            {
                var showEdit = false;
                var showDel = false;

                // TH1: Có quyền ""Xem file trên hệ thống"" thì được xem file khách hàng
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_file_trên_hệ_thống))
                {
                    sb.Clear();
                    foreach (var item in objFileDinhKemList)
                    {
                        if ((item.Status == 1))
                        {
                            if (Mode != "View")
                            {
                                if (
                                    (BuildPhongBan_Permission.CheckPermission(
                                        PermissionSchemes.Xóa_file_đính_kèm_của_mình) && login.Username == item.CUser)
                                    ||
                                    BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                                    showDel = true;
                                if (
                                    BuildPhongBan_Permission.CheckPermission(
                                        PermissionSchemes.Sửa_thông_tin_file_đính_kèm) && login.Username == item.CUser)
                                    showEdit = true;
                            }

                            sb.AppendFormat("<tr class=\"gridFooterRow\" id='fRow{0}'><td><span>{1}</span></td>",
                                item.Id, item.CUser);
                            sb.AppendFormat(
                                "<td><div style='word-wrap: break-word; width:200px'><a style='text-decoration: underline' href=\"{0}\"  target='_blank'>{1}</a> </div></td>",
                                "/Views/ChiTietKhieuNai/Download.aspx?id=" + item.Id, item.TenFile);
                            if (item.Status == 1)
                                sb.Append("<td><span>Khách hàng</span></td>");
                            else if (item.Status == 2)
                                sb.Append("<td><span>Bảo mật</span></td>");
                            else
                                sb.Append("<td><span></span></td>");

                            sb.AppendFormat("<td><span>{0} KB</span></td>", item.KichThuoc);
                            sb.AppendFormat("<td><div style='word-wrap: break-word; width:250px'>{0}</div></td>",
                                item.GhiChu);
                            sb.AppendFormat("<td><span>{0}</span></td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));

                            var btnDelStr = showDel
                                ? string.Format(
                                    " <a href='javascript:void(0)' onclick='DeleteFile({0})' title='Xóa file đính kèm: {1}' class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>",
                                    item.Id, item.TenFile)
                                : "";
                            var btnEditStr = showEdit
                                ? string.Format(
                                    "<a href='javascript:void(0)' onclick='EditFile({0})' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>",
                                    item.Id, item.TenFile)
                                : "";
                            sb.AppendFormat("<td><span style='text-align:center'>{0}</span></td>",
                                btnEditStr + btnDelStr);
                        }
                    }
                }
                // TH2: Có quyền ""Xem file tại KN xử lý"" được xem file KH và file hệ thống do mình giữ hoặc do mình đóng
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_file_tại_khiếu_nại_xử_lý))
                {
                    sb.Clear();
                    foreach (var item in objFileDinhKemList)
                    {
                        KhieuNaiInfo khieuNaiInfo = new KhieuNaiInfo();
                        khieuNaiInfo.Id = item.KhieuNaiId;
                        khieuNaiInfo = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetInfo(khieuNaiInfo);
                        if ((item.Status == 1) || (item.Status == 2 && khieuNaiInfo.NguoiXuLy == login.Username))
                        {
                            if (Mode != "View")
                            {
                                if (
                                    (BuildPhongBan_Permission.CheckPermission(
                                        PermissionSchemes.Xóa_file_đính_kèm_của_mình) && login.Username == item.CUser)
                                    ||
                                    BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                                    showDel = true;
                                if (
                                    BuildPhongBan_Permission.CheckPermission(
                                        PermissionSchemes.Sửa_thông_tin_file_đính_kèm) && login.Username == item.CUser)
                                    showEdit = true;
                            }

                            sb.AppendFormat("<tr class=\"gridFooterRow\" id='fRow{0}'><td><span>{1}</span></td>",
                                item.Id, item.CUser);
                            sb.AppendFormat(
                                "<td><div style='word-wrap: break-word; width:200px'><a style='text-decoration: underline' href=\"{0}\"  target='_blank'>{1}</a> </div></td>",
                                "/Views/ChiTietKhieuNai/Download.aspx?id=" + item.Id, item.TenFile);
                            if (item.Status == 1)
                                sb.Append("<td><span>Khách hàng</span></td>");
                            else if (item.Status == 2)
                                sb.Append("<td><span>Bảo mật</span></td>");
                            else
                                sb.Append("<td><span></span></td>");

                            sb.AppendFormat("<td><span>{0} KB</span></td>", item.KichThuoc);
                            sb.AppendFormat("<td><div style='word-wrap: break-word; width:250px'>{0}</div></td>",
                                item.GhiChu);
                            sb.AppendFormat("<td><span>{0}</span></td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));

                            var btnDelStr = showDel
                                ? string.Format(
                                    " <a href='javascript:void(0)' onclick='DeleteFile({0})' title='Xóa file đính kèm: {1}' class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>",
                                    item.Id, item.TenFile)
                                : "";
                            var btnEditStr = showEdit
                                ? string.Format(
                                    "<a href='javascript:void(0)' onclick='EditFile({0})' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>",
                                    item.Id, item.TenFile)
                                : "";
                            sb.AppendFormat("<td><span style='text-align:center'>{0}</span></td>",
                                btnEditStr + btnDelStr);
                        }
                    }
                }
                // TH3: Có quyền ""Xem file tại phòng ban xử lý"" được xem file KH và file hệ thống do phòng ban mình giữ hoặc đóng
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_file_tại_phòng_ban_xử_lý_KN))
                {
                    sb.Clear();
                    var KhieuNaiItem = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetListDynamic("PhongBanXuLyId", "Id=" + KhieuNaiId, "");
                    if (KhieuNaiItem != null && KhieuNaiItem.Count > 0 && KhieuNaiItem[0].PhongBanXuLyId == login.PhongBanId)
                    {
                        foreach (var item in objFileDinhKemList)
                        {
                            if (Mode != "View")
                            {
                                if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_file_đính_kèm_của_mình) && login.Username == item.CUser)
                                    || BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                                    showDel = true;
                                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_file_đính_kèm) && login.Username == item.CUser)
                                    showEdit = true;
                            }

                            sb.AppendFormat("<tr class=\"gridFooterRow\" id='fRow{0}'><td><span>{1}</span></td>", item.Id, item.CUser);
                            sb.AppendFormat("<td><div style='word-wrap: break-word; width:200px'><a style='text-decoration: underline' href=\"{0}\"  target='_blank'>{1}</a> </div></td>",
                                 "/Views/ChiTietKhieuNai/Download.aspx?id=" + item.Id, item.TenFile);
                            if (item.Status == 1)
                                sb.Append("<td><span>Khách hàng</span></td>");
                            else if (item.Status == 2)
                                sb.Append("<td><span>Bảo mật</span></td>");
                            else
                                sb.Append("<td><span></span></td>");

                            sb.AppendFormat("<td><span>{0} KB</span></td>", item.KichThuoc);
                            sb.AppendFormat("<td><div style='word-wrap: break-word; width:250px'>{0}</div></td>", item.GhiChu);
                            sb.AppendFormat("<td><span>{0}</span></td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));

                            var btnDelStr = showDel ? string.Format(" <a href='javascript:void(0)' onclick='DeleteFile({0})' title='Xóa file đính kèm: {1}' class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>", item.Id, item.TenFile) : "";
                            var btnEditStr = showEdit ? string.Format("<a href='javascript:void(0)' onclick='EditFile({0})' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>", item.Id, item.TenFile) : "";
                            sb.AppendFormat("<td><span style='text-align:center'>{0}</span></td>", btnEditStr + btnDelStr);
                        }
                    }
                }
                // TH4: Có quyền ""Xem file toàn hệ thống"" được xem toàn bộ file trên hệ thống "
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_file_toàn_hệ_thống))
                {
                    sb.Clear();
                    foreach (var item in objFileDinhKemList)
                    {
                        if (Mode != "View")
                        {
                            if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_file_đính_kèm_của_mình) && login.Username == item.CUser)
                                || BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm))
                                showDel = true;
                            if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_file_đính_kèm) && login.Username == item.CUser)
                                showEdit = true;
                        }

                        sb.AppendFormat("<tr class=\"gridFooterRow\" id='fRow{0}'><td><span>{1}</span></td>", item.Id, item.CUser);
                        sb.AppendFormat("<td><div style='word-wrap: break-word; width:200px'><a style='text-decoration: underline' href=\"{0}\"  target='_blank'>{1}</a> </div></td>",
                             "/Views/ChiTietKhieuNai/Download.aspx?id=" + item.Id, item.TenFile);
                        if (item.Status == 1)
                            sb.Append("<td><span>Khách hàng</span></td>");
                        else if (item.Status == 2)
                            sb.Append("<td><span>Bảo mật</span></td>");
                        else
                            sb.Append("<td><span></span></td>");

                        sb.AppendFormat("<td><span>{0} KB</span></td>", item.KichThuoc);
                        sb.AppendFormat("<td><div style='word-wrap: break-word; width:250px'>{0}</div></td>", item.GhiChu);
                        sb.AppendFormat("<td><span>{0}</span></td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));

                        var btnDelStr = showDel ? string.Format(" <a href='javascript:void(0)' onclick='DeleteFile({0})' title='Xóa file đính kèm: {1}' class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>", item.Id, item.TenFile) : "";
                        var btnEditStr = showEdit ? string.Format("<a href='javascript:void(0)' onclick='EditFile({0})' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>", item.Id, item.TenFile) : "";
                        sb.AppendFormat("<td><span style='text-align:center'>{0}</span></td>", btnEditStr + btnDelStr);
                    }
                }
            }
            else
            {
                sb.AppendFormat(@"<tr id='nodata' class='gridRow'>
                    <td colspan='8'><span>Chưa có dữ liệu...</span>
                    </td>
                </tr>");
            }
            listFile = sb.ToString();
        }
        //private string DownloadFileToLocal(string urlFile)
        //{
        //    var str = string.Empty;
        //    if (!string.IsNullOrEmpty(urlFile))
        //    {
        //        string FileName = Common.getFileName(urlFile);
        //        if (RemoteFileToFtp.CheckFileExistOnFtp(urlFile))
        //        {
        //            string pathLocal = "../../" + Common.getPathLocal(urlFile);
        //            RemoteFileToFtp.DownloadFileToLocal(urlFile, Server.MapPath(pathLocal), Server.MapPath("../../Upload/VanBanGoc/"));
        //            str = pathLocal.EndsWith("/") ? pathLocal + FileName : pathLocal + "/" + FileName;
        //        }

        //    }
        //    return str;
        //}
    }
}