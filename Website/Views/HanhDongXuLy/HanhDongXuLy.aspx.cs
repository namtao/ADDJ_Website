using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;

namespace Website.Views.HanhDongXuLy
{
    public partial class HanhDongXuLy : AppCode.PageBase
    {
        protected string strPhongBanId = string.Empty;
        protected string strUsername = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            strPhongBanId = UserInfo.PhongBanId.ToString();
            strUsername = UserInfo.Username;
            if (!IsPostBack)
            {
                BindDropDownlist();
                BindData();
            }
        }


        private void BindData()
        {

        }

        private void BindDropDownlist()
        {
            var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
            lstLoaiKN.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "--Chọn Loại khiếu nại--" });
            ddlLoaiKhieuNai.DataSource = lstLoaiKN;
            ddlLoaiKhieuNai.DataTextField = "Name";
            ddlLoaiKhieuNai.DataValueField = "Id";
            ddlLoaiKhieuNai.DataBind();
        }

        protected void lbtDongTruyVan_Click(object sender, EventArgs e)
        {
            Response.Redirect("HanhDongXuLy.aspx", false);
        }

        protected void ddlLoaiKhieuNai_Changed(object sender, EventArgs e)
        {
            var lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + ddlLoaiKhieuNai.SelectedValue, "Sort");
            lstLinhVucChung.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "--Chọn Lĩnh vực chung--" });
            ddlLinhVucChung.DataSource = lstLinhVucChung;
            ddlLinhVucChung.DataTextField = "Name";
            ddlLinhVucChung.DataValueField = "Id";
            ddlLinhVucChung.DataBind();
        }

        protected void ddlLinhVucChung_Changed(object sender, EventArgs e)
        {
            var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + ddlLinhVucChung.SelectedValue, "Sort");
            if (lstLinhVucCon.Count > 1)
                lstLinhVucCon.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "--Chọn Lĩnh vực con--" });
            ddlLinhVucCon.DataSource = lstLinhVucCon;
            ddlLinhVucCon.DataTextField = "Name";
            ddlLinhVucCon.DataValueField = "Id";
            ddlLinhVucCon.DataBind();
        }
    }
}