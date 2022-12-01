using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.TTKH.Handler
{
    public partial class ExcelLichSuKhieuNai : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["SoThueBao"] != null && !Request.QueryString["SoThueBao"].Equals(""))
            {
                Bind_LichSuKhieuNai();
                Export();
            }
        }

        private void BindData()
        { }

        #region Lich Su Khieu Nai

        private void Bind_LichSuKhieuNai()
        {
            ltSTB.Text = Request.QueryString["SoThueBao"];
            ltTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
            var lst = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("", "SoThueBao=" + Request.QueryString["SoThueBao"], "LDate DESC");

            grvViewLichSuKhieuNai.DataSource = lst;
            grvViewLichSuKhieuNai.DataBind();
        }

        protected string BindTinhTrangXuLy(object obj)
        {
            try
            {
                return Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToByte(obj)).Replace("_", " ");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        protected string BindMaKN(object obj)
        {
            var MaAuto = GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, obj, 10);

            return string.Format("<a href='{0}/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN={1}&Mode=View'>{2}</a>", Request.Url.Authority, obj, MaAuto);
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


        #endregion

        private void Export()
        {
            var tenbc = "LichSuKN_" + Request.QueryString["SoThueBao"];
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            //string pathCSS = Server.MapPath("~/CSS");
            //pathCSS += @"\BaoCao.css";
            //StreamReader reader = new StreamReader(pathCSS);
            ////reader.ReadToEnd();

            //Response.Write("<style>");
            //Response.Write(reader.ReadToEnd());
            //Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
    }
}