using AIVietNam.Core;
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.ExportExcel
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void Export()
        {
            string fileNameTemp = "Temp1.xlsx";
            string pathFile = Server.MapPath("~/ExportExcel/Temp/" + fileNameTemp);

            WorkbookDesigner designer = new WorkbookDesigner();
            var loadOptions = new LoadOptions(LoadFormat.Xlsx);
            designer.Workbook = new Workbook(pathFile, loadOptions);

            //DateTime startDate = DateTime.Parse(txtFromDate.Text, new CultureInfo("vi-VN")).StartOfDay();
            // DateTime endDate = DateTime.Parse(txtToDate.Text, new CultureInfo("vi-VN")).EndOfDay();
            string tieuDe = string.Format("{0}", "Tên báo cáo");

            designer.SetDataSource("TieuDe", tieuDe);
            string sql = string.Format("SELECT * FROM NguoiSuDung WHERE TenTruyCap = '{0}'", "Administrator");
            designer.SetDataSource("Table", SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql).Tables[0].DefaultView);
            designer.Process();

            string exportPath = HttpContext.Current.Server.MapPath("~/ExportExcel/Temp/");
            if (!Directory.Exists(exportPath)) Directory.CreateDirectory(exportPath);

            string newFileName = string.Format("Temp1_{0:yyMMddHHmmss}.xlsx", DateTime.Now);
            string fullFileName = Path.Combine(exportPath, newFileName);

            designer.Workbook.Save(fullFileName, SaveFormat.Xlsx);

            //Set the appropriate ContentType.
            Response.ContentType = "application/vnd.ms-excel";

            string header = string.Format("attachment; filename={0}", newFileName);
            Response.AddHeader("Content-Disposition", header);
            //Get the physical path to the file.
            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fullFileName);
            Response.Flush();
        }
    }
}