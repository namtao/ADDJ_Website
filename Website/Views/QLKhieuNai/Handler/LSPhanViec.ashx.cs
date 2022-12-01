using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AIVietNam.Admin;
using System.Web.Services;
using AIVietNam.Core;
using System.Web.SessionState;
using AIVietNam.GQKN.Impl;
using System.Data;
using Aspose.Cells;
using System.Drawing;
using System.IO;
using Website.AppCode;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for LSPhanViec
    /// </summary>
    /// 

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LSPhanViec : IHttpHandler, IReadOnlySessionState
    {
        NguoiSuDungImpl _NguoiSuDungImpl = new NguoiSuDungImpl();
        LichSuPhanViecImpl _LichSuPhanViecImpl = new LichSuPhanViecImpl();
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
                    context.Response.Write(JSSerializer.Serialize(ProcessData(context.Request.QueryString["key"], infoUser, context)));
                }
            }
        }

        private string ProcessData(string key, AdminInfo infoUser, HttpContext context)
        {
            string strValue = "";
            switch (key)
            {
                case "1": //Tính tong so bản ghi
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        string user = context.Request.QueryString["userName"].ToString();
                        string tuNgay = context.Request.QueryString["tuNgay"].ToString();
                        string denNgay = context.Request.QueryString["denNgay"].ToString();
                        if (user == "-1")
                        {
                            user = "";
                        }
                        strValue = GetLichSuPhanViec_TotalRecords(infoUser.PhongBanId, user, tuNgay, denNgay, startPageIndex, pageSize);
                    }
                    break;
                case "2": //Bind htm vao Grid
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        string user = context.Request.QueryString["userName"].ToString();
                        string tuNgay = context.Request.QueryString["tuNgay"].ToString();
                        string denNgay = context.Request.QueryString["denNgay"].ToString();
                        if (user == "-1")
                        {
                            user = "";
                        }
                        strValue = GetHtmlLichSuPhanViec(infoUser.PhongBanId, user, tuNgay, denNgay, startPageIndex, pageSize);

                    }
                    break;
                case "3": //Load Danh sách User trong phong ban
                    strValue = GetItemDropListUserNameByPhongBanID(infoUser.PhongBanId);
                    break;
                case "4": //Xuat danh sách ra Excel
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        string user = context.Request.QueryString["userName"].ToString();
                        string tuNgay = context.Request.QueryString["tuNgay"].ToString();
                        string denNgay = context.Request.QueryString["denNgay"].ToString();
                        if (user == "-1")
                        {
                            user = "";
                        }
                        strValue = ExportExcel(infoUser.PhongBanId, user, tuNgay, denNgay, startPageIndex, pageSize);

                    }
                    break;

            }
            return strValue;
        }
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

        private int AddContentToSheet(DataTable tab, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (DataRow row in tab.Rows)
                {
                    int TongSoTiepNhan = 0;
                    int TongSoDaXuLy = 0;
                    sheet.Cells[RowIndex, 0].PutValue(row["STT"]);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 1].PutValue(row["NguoiDuocPhanViec"]);
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCell());

                    if (!string.IsNullOrEmpty(row["TongSoTiepNhan"].ToString()))
                    {
                        sheet.Cells[RowIndex, 2].PutValue(row["TongSoTiepNhan"]);
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        TongSoTiepNhan = Convert.ToInt32(row["TongSoTiepNhan"]);
                    }
                    else
                    {
                        sheet.Cells[RowIndex, 2].PutValue("0");
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                    }

                    if (!string.IsNullOrEmpty(row["TongSoDaXuLy"].ToString()))
                    {
                        sheet.Cells[RowIndex, 3].PutValue(row["TongSoDaXuLy"]);
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        TongSoDaXuLy = Convert.ToInt32(row["TongSoDaXuLy"]);
                    }
                    else
                    {
                        sheet.Cells[RowIndex, 3].PutValue("0");
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                    }
                    sheet.Cells[RowIndex, 4].PutValue((TongSoTiepNhan - TongSoDaXuLy).ToString());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    RowIndex++;


                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
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
        private string GetItemDropListUserNameByPhongBanID(int phongBanId)
        {
            try
            {
                string strValues = "";
                DataTable tab = _NguoiSuDungImpl.GetListNguoiSuDungByPhanViecPhongBanId(Convert.ToInt32(phongBanId));
                strValues += "<option value=\"-1\">--Chọn tài khoản--</option>";
                if (tab.Rows.Count > 0)
                {
                    foreach (DataRow info in tab.Rows)
                    {
                        strValues += "<option value=\"" + info["TenTruyCap"] + "\">" + info["TenTruyCap"] + "</option>";
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

        private string GetLichSuPhanViec_TotalRecords(Int32 PhongBanId, string userName, string tuNgay, string denNgay, string startPageIndex, string pageSize)
        {
            try
            {
                var arrTuNgay = tuNgay.Split('/');
                var arrdenNgay = denNgay.Split('/');
                DateTime from = Convert.ToDateTime(arrTuNgay[1] + "/" + arrTuNgay[0] + "/" + arrTuNgay[2]);
                DateTime end = Convert.ToDateTime(arrdenNgay[1] + "/" + arrdenNgay[0] + "/" + arrdenNgay[2]);
                int TotalRecords = _LichSuPhanViecImpl.LichSuPhanViec_ThongkeTotalRecords_GetAllWithPadding(PhongBanId, userName, from, end, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetHtmlLichSuPhanViec(Int32 PhongBanId, string userName, string tuNgay, string denNgay, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                var arrTuNgay = tuNgay.Split('/');
                var arrdenNgay = denNgay.Split('/');
                DateTime from = Convert.ToDateTime(arrTuNgay[1] + "/" + arrTuNgay[0] + "/" + arrTuNgay[2]);
                DateTime end = Convert.ToDateTime(arrdenNgay[1] + "/" + arrdenNgay[0] + "/" + arrdenNgay[2]);
                DataTable tab = _LichSuPhanViecImpl.LichSuPhanViec_Thongke_GetAllWithPadding(PhongBanId, userName, from, end, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        int TongSoTiepNhan = 0;
                        int TongSoDaXuLy = 0;
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowB\">";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"].ToString() + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NguoiDuocPhanViec"] + "</td>";
                        if (!string.IsNullOrEmpty(row["TongSoTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + row["TongSoTiepNhan"] + "</td>";
                            TongSoTiepNhan = Convert.ToInt32(row["TongSoTiepNhan"]);
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">0</td>";
                        }
                        if (!string.IsNullOrEmpty(row["TongSoDaXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + row["TongSoDaXuLy"] + "</td>";
                            TongSoDaXuLy = Convert.ToInt32(row["TongSoDaXuLy"]);
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">0</td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + (TongSoTiepNhan - TongSoDaXuLy).ToString() + "</td>";


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


        #region KN ChuyenBoPhanKhac

        private string ExportExcel(Int32 PhongBanId, string userName, string tuNgay, string denNgay, string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\ThongKePhanViec.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(8, sheet.Cells.Rows.Count);
                int RowIndex = 7;
                var arrTuNgay = tuNgay.Split('/');
                var arrdenNgay = denNgay.Split('/');
                DateTime from = Convert.ToDateTime(arrTuNgay[1] + "/" + arrTuNgay[0] + "/" + arrTuNgay[2]);
                DateTime end = Convert.ToDateTime(arrdenNgay[1] + "/" + arrdenNgay[0] + "/" + arrdenNgay[2]);
                //Save the Excel file.
                int TotalRecords = _LichSuPhanViecImpl.LichSuPhanViec_ThongkeTotalRecords_GetAllWithPadding(PhongBanId, userName, from, end, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {

                    sheet.Cells[2, 4].PutValue(tuNgay + "-" + denNgay);

                    sheet.Cells[3, 4].PutValue(ServiceFactory.GetInstancePhongBan().GetInfo(PhongBanId).Name);

                    if (!string.IsNullOrEmpty(userName))
                    {
                        sheet.Cells[4, 3].PutValue("Người tiếp nhận :");

                        sheet.Cells[4, 4].PutValue(userName);
                    }
                    else
                    {
                        sheet.Cells[4, 3].PutValue("");

                        sheet.Cells[4, 4].PutValue("");

                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable tab = _LichSuPhanViecImpl.LichSuPhanViec_Thongke_GetAllWithPadding(PhongBanId, userName, from, end, i, Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "ThongKePhanViec" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
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
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
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