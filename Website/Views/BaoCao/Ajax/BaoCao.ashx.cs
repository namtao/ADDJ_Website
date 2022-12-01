using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using Website.AppCode;

namespace Website.Views.BaoCao.AJAX
{
    /// <summary>
    /// Summary description for BaoCao
    /// </summary>
    public class BaoCao : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var type = context.Request.Form["type"] ?? context.Request.QueryString["type"];
            switch (type)
            {

                case "loaikhieunai":
                    BindLoaiKhieuNai(context);
                    break;

                case "linhvucchung":
                    BindLinhVucChung(context);
                    break;
                case "linhvuccon":
                    BindLinhVucCon(context);
                    break;
            }
        }

        private void BindLoaiKhieuNai(HttpContext context)
        {
            var data = context.Request.Form;

            StringBuilder sbdv = new StringBuilder();
            var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
            sbdv.AppendFormat("<option value='{1}' code='{0}'>{1}</option>", "-1", "Chọn Loại khiếu nại..");
            foreach (var infoLoaiKN in lstLoaiKN)
            {
                sbdv.AppendFormat("<option value='{1}' code=\"{0}\">{1}</option>", infoLoaiKN.Id, infoLoaiKN.Name);
            }

            context.Response.Write(sbdv.ToString());
        }

        private void BindLinhVucChung(HttpContext context)
        {
            var data = context.Request.Form;
            var loaiKhieuNaiID = data["loaiKhieuNaiID"];
            var list = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + loaiKhieuNaiID, "Sort");
            var sb = new StringBuilder();
            sb.AppendFormat("<option value='{1}' code='{0}'>{1}</option>", "-1", "Chọn Lĩnh vực chung..");
            foreach (var item in list)
            {
                sb.AppendFormat("<option value='{1}' code=\"{0}\">{1}</option>", item.Id, item.Name);
            }
            context.Response.Write(sb.ToString());
        }

        private void BindLinhVucCon(HttpContext context)
        {
            var data = context.Request.Form;
            var linhVucChungID = data["linhVucChungID"];
            var list = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + linhVucChungID, "Sort");
            var sb = new StringBuilder();
            sb.AppendFormat("<option value='{1}' code='{0}'>{1}</option>", "-1", "Chọn Lĩnh vực con..");
            foreach (var item in list)
            {
                sb.AppendFormat("<option value='{1}' code=\"{0}\">{1}</option>", item.Id, item.Name);
            }
            context.Response.Write(sb.ToString());
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