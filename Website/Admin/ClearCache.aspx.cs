using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using ADDJ.Admin;
using ADDJ.Core;
using ADDJ.Core.Provider;
using ADDJ.Impl;

namespace Website.admin
{
    public partial class ClearCache : AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btClearAllCache_Click(object sender, EventArgs e)
        {
           
        }

        protected void btDeltaImportKQXL_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/KetQuaXuLy/select?qt=/dataimport&command=delta-import&clean=false&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void btDeltaImportSoTien_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/SoTien/select?qt=/dataimport&command=delta-import&clean=false&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void btDeltaImportActivity_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/Activity/select?qt=/dataimport&command=delta-import&clean=false&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void btDeltaImportKhieuNai_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/GQKN/select?qt=/dataimport&command=delta-import&clean=false&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void btFullImportKQXL_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/KetQuaXuLy/select?qt=/dataimport&command=full-import&clean=true&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void btFullImportSoTien_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/SoTien/select?qt=/dataimport&command=full-import&clean=true&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void btFullImportActivity_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/Activity/select?qt=/dataimport&command=full-import&clean=true&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void btFullImportKhieuNai_Click(object sender, EventArgs e)
        {
            var response = DownloadExpress.Download("http://10.149.34.231:8080/solr/GQKN/select?qt=/dataimport&command=full-import&clean=true&commit=true");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        }

        protected void linkbtnClearAllCache_Click(object sender, EventArgs e)
        {
            Config.UpdateConfig();

            MenuImpl.ListMenu = ServiceFactory.GetInstanceMenu().GetList();

            CacheProvider.ClearCache();

            ServiceFactory.GetInstancePhongBan().FullindexLucene();

            ServiceFactory.GetInstanceNguoiSuDung().FullindexLucene();

            ServiceFactory.GetInstanceLoaiKhieuNai().FullindexLucene();

            NguoiSuDung_GroupImpl.NhomNguoiDung = ServiceFactory.GetInstanceNguoiSuDung_Group().GetList();

            LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl.ListThoiGianXuLyPhongBan = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetList();

            DoiTacImpl.ListDoiTac = ServiceFactory.GetInstanceDoiTac().GetList();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa cache thành công.','info');", true);

            // System.Threading.Thread.Sleep(20000);
        }
    }
}