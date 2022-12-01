using AIVietNam.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaosoluongkhieunaitondongvaquahancuadoitac : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<int> listDoiTacId = new List<int>();
                string loaibc = Request.QueryString["loaibc"];
                string listDoiTac = Request.QueryString["listDoiTac"];
                if(listDoiTac.Length > 0)
                {
                    string[] arrDoiTac = listDoiTac.Split(',');
                    for(int i=0;i<arrDoiTac.Length;i++)
                    {
                        listDoiTacId.Add(ConvertUtility.ToInt32(arrDoiTac[i]));
                    }
                }

                bool isExportExcel = false;
                if (loaibc == "excel")
                {
                    isExportExcel = true;
                }

                sNoiDungBaoCao = new BuildBaoCao().BaoCaoSoLuongTonDongVaQuaHanCuaCacDoiTac(listDoiTacId, isExportExcel);

                if (isExportExcel)
                {
                    export2excel("BaoCaoSoLuongKNQuaHanVaTonDongCuaDoiTac_" + DateTime.Now.ToString("yyyyMMddHHmm"));                   
                }
            }
        }

        public void export2excel(string tenbc)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            string pathCSS = Server.MapPath("~/CSS");
            pathCSS += @"\BaoCao.css";
            StreamReader reader = new StreamReader(pathCSS);
            //reader.ReadToEnd();

            Response.Write("<style>");
            Response.Write(reader.ReadToEnd());
            Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
    }
}