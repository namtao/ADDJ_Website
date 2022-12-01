using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Core;

namespace Website.Views.KhieuNai.UC
{
    public partial class ucLuuVetThayDoi : System.Web.UI.UserControl
    {
        public string username = "";
        public int KhieuNaiId { get; set; }
        private int ArchiveId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //var obj_admin = LoginAdmin.AdminLogin();
            //username = obj_admin.Username;
            ArchiveId = ConvertUtility.ToInt32(Request.QueryString["archive"]);
            KhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["MaKN"]);
            //// KieuNaiId test
            ////KhieuNaiId = 3;
            //if (!IsPostBack)
            //{
            //    FillLuuVetThayDoiGrid();
            //}
        }

        public void FillLuuVetThayDoiGrid()
        {
            string whereClause = "KhieuNaiId = " + KhieuNaiId;
            List<KhieuNai_LogInfo> objBuocXuLyList = new List<KhieuNai_LogInfo>();
            objBuocXuLyList = ServiceFactory.GetInstanceKhieuNai_Log(ArchiveId).GetListDynamic("", whereClause, "CDate DESC");
            gvLuuVetThayDoi.DataSource = objBuocXuLyList;
            gvLuuVetThayDoi.DataBind();
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvLuuVetThayDoi.PageIndex = e.NewPageIndex;
            FillLuuVetThayDoiGrid();
        }

        protected void gvLuuVetThayDoi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }

        protected void gvLuuVetThayDoi_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
        }
    }
}