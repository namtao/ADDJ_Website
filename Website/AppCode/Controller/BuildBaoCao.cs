using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;
using System;
using System.Globalization;
using System.Linq;
using AIVietNam.Admin;
using System.Data;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using System.Web.UI.HtmlControls;
using System.Web;
using Aspose.Cells;
using System.IO;
using System.Data.SqlClient;
using Website.ServiceVNP;

namespace Website.AppCode.Controller
{
    public class BuildBaoCao
    {
        private const string FORMAT_NUMBER = "###,###,###";

        public static string BaoCaoChiTietGiamTruCuocDVTraTruoc(int khuVucID, int donViID, int fromDate, int toDate, int loaiKhieuNaiID, int linhVucChungID, int linhVucConID, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                //List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoChiTietGiamTruCuocDV(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoChiTietGiamTruCuocDVTraTruoc_Solr(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                int i = 0;
                if (list != null && list.Count > 0)
                {
                    decimal totalSoTienKhauTru_TKC = 0;
                    decimal totalSoTienKhauTru_KM = 0;
                    decimal totalSoTienKhauTru_KM1 = 0;
                    decimal totalSoTienKhauTru_KM2 = 0;
                    decimal totalSoTienKhauTru_Data = 0;
                    decimal totalSoTienKhauTru_Khac = 0;

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        i = i + 1;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + i + "</td>");
                        sb.Append("<td class='borderThin'>" + info.SoThueBao + "</td>");
                        sb.Append("<td class='borderThin'>" + info.NoiDungPA + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LinhVucCon + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (info.NgayDongKN.Date == DateTime.MaxValue.Date ? "" : info.NgayDongKN.ToString("dd/MM/yyyy")) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SHCV + "</td>");
                        sb.Append("<td class='borderThin'>" + info.NoiDungXuLy + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TKC.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_KM.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_KM1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_KM2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_Data.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_Khac.ToString(formatNumber) + "</td>");
                        sb.Append("</tr>");

                        totalSoTienKhauTru_TKC += info.SoTienKhauTru_TKC;
                        totalSoTienKhauTru_KM += info.SoTienKhauTru_KM;
                        totalSoTienKhauTru_KM1 += info.SoTienKhauTru_KM1;
                        totalSoTienKhauTru_KM2 += info.SoTienKhauTru_KM2;
                        totalSoTienKhauTru_Data += info.SoTienKhauTru_Data;
                        totalSoTienKhauTru_Khac += info.SoTienKhauTru_Khac;
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='7'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienKhauTru_TKC.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienKhauTru_KM.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienKhauTru_KM1.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienKhauTru_KM2.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienKhauTru_Data.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienKhauTru_Khac.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");

                }
                else
                {
                    sb.Append(@"<tr>
                    <td colspan='13'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                    <td colspan='13'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>";
            }
        }

        public static string BaoCaoChiTietGiamTruCuocDVTraSau(int parentDoiTacId, int doiTacId, int phongBanXuLyId, int fromDate, int toDate, int loaiKhieuNaiID, int linhVucChungID, int linhVucConID, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                List<KhieuNai_ReportInfo> list = null;
                if (phongBanXuLyId == -1)
                {
                    list = new ReportImpl().BaoCaoChiTietGiamTruCuocDVTraSauVNPTTT_Solr(parentDoiTacId, doiTacId, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                }
                else
                {
                    list = new ReportImpl().BaoCaoChiTietGiamTruCuocDVTraSauToGQKN_Solr(phongBanXuLyId, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                }
                int i = 0;
                if (list != null && list.Count > 0)
                {
                    decimal totalGPRS = 0;
                    decimal totalCP = 0;
                    decimal totalThoai = 0;
                    decimal totalSMS = 0;
                    decimal totalIR = 0;
                    decimal totalKhac = 0;

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        i = i + 1;
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td class='borderThin'>" + info.SoThueBao + "</td>");
                        sb.Append("<td class='borderThin'>" + info.NoiDungPA + "</td>");
                        sb.Append("<td class='borderThin'>" + info.LinhVucCon + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (info.NgayDongKN.Date == DateTime.MaxValue.Date ? "" : info.NgayDongKN.ToString("dd/MM/yyyy")) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SHCV + "</td>");
                        sb.Append("<td class='borderThin'>" + info.NoiDungXuLy + "</td>");
                        //sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TKC.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TS_GPRS.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TS_CP.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TS_Thoai.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TS_SMS.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TS_IR.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TS_Khac.ToString(formatNumber) + "</td>");
                        sb.Append("</tr>");

                        totalGPRS += info.SoTienKhauTru_TS_GPRS;
                        totalCP += info.SoTienKhauTru_TS_CP;
                        totalThoai += info.SoTienKhauTru_TS_Thoai;
                        totalSMS += info.SoTienKhauTru_TS_SMS;
                        totalIR += info.SoTienKhauTru_TS_IR;
                        totalKhac += info.SoTienKhauTru_TS_Khac;
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='7'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalGPRS.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCP.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalThoai.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSMS.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalIR.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalKhac.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                    <td colspan='13'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                    <td colspan='13'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>";
            }
        }

        public static string BaoCaoChiTietPPS(int khuVucID, int donViID, int fromDate, int toDate, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, List<int> listLinhVucConId, int trangThai, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                //Hien tai du lieu chua co nen chua query theo tham so tu ngay - den ngay
                //List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoChiTietPPS(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoChiTietPPS_Solr(khuVucID, donViID, fromDate, toDate, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, trangThai);
                int i = 0;
                if (list != null && list.Count > 0)
                {
                    decimal totalTKC = 0;
                    decimal totalTKKM = 0;
                    decimal totalData = 0;
                    decimal totalKhac = 0;

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        i = i + 1;
                        string sTrangThai = info.TrangThai == (int)KhieuNai_TrangThai_Type.Đóng ? "Close" : "Đang xử lý";
                        decimal tongSoTienKM = info.SoTienKhauTru_KM1 + info.SoTienKhauTru_KM2 + info.SoTienKhauTru_KM;
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td class='borderThin'>" + info.SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.MaTinh + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.NoiDungPA + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LoaiKhieuNai + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LinhVucChung + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LinhVucCon + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.MaDichVu + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LDate.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SHCV + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (info.IsCCT ? "CCT" : "") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_TKC.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + tongSoTienKM.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_Data.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTienKhauTru_Khac.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.NoiDungXuLy + "</td>");
                        sb.Append("<td class='borderThin'>" + info.KetQuaXuLy + "</td>");
                        sb.Append("<td class='borderThin'>" + info.GhiChu + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LUser_KetQuaXuLy + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + sTrangThai + "</td>");
                        sb.Append("</tr>");

                        totalTKC += info.SoTienKhauTru_TKC;
                        totalTKKM += tongSoTienKM;
                        totalData += info.SoTienKhauTru_Data;
                        totalKhac += info.SoTienKhauTru_Khac;
                    }

                    sb.Append("<tr>");
                    sb.Append("<td class='borderThinTextBold' colspan='10'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTKC.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTKKM.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalData.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalKhac.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='19'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                            <td colspan='19'>
                                Chưa có dữ liệu báo cáo
                            </td>
                        </tr>";
            }
        }

        public static string BaoCaoChiTietPOST(int khuVucID, int donViID, int fromDate, int toDate, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, List<int> listLinhVucConId, int trangThai)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //Hien tai du lieu chua co nen chua query theo tham so tu ngay - den ngay
                //List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoChiTietPOST(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoChiTietPOST_Solr(khuVucID, donViID, fromDate, toDate, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, trangThai);
                int i = 0;
                if (list != null && list.Count > 0)
                {

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        try
                        {
                            i = i + 1;
                            string sTrangThai = info.TrangThai == (int)KhieuNai_TrangThai_Type.Đóng ? "Close" : "Đang xử lý";
                            sb.Append("<tr>");
                            sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                            sb.Append("<td class='borderThin'>" + info.SoThueBao + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + info.MaTinh + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + info.NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), (byte)info.HTTiepNhan).Replace("_", " ") + "</td>");
                            sb.Append("<td class='borderThin'>" + info.NoiDungPA + "</td>");
                            sb.Append("<td class='borderThin'>" + info.NoiDungCanHoTro + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + info.LDate.ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + (info.IsCSL ? "CSL" : "") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + (info.PTSoLieu_IR ? "IR" : "") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + info.PTSoLieu_Khac + "</td>");
                            sb.Append("<td class='borderThin'>" + info.NoiDungXuLy + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + info.LUser_KetQuaXuLy + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + sTrangThai + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + info.GhiChu + "</td>");
                            sb.Append("</tr>");
                        }
                        catch
                        {

                        }

                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='15'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                            <td colspan='15'>
                                Chưa có dữ liệu báo cáo
                            </td>
                        </tr>";
            }
        }

        public static string BaoCaoTongHopGiamTru(int khuVucID, int donViID, int fromDate, int toDate, int loaiKhieuNaiID, int linhVucChungID, int linhVucConID, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                //List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoTongHopGiamTruCuocDV(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoTongHopGiamTruCuocDV_Solr(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                int i = 0;
                if (list != null && list.Count > 0)
                {
                    decimal totalSoTienGiamTru = 0;
                    int totalSoLuongGiamTru = 0;

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        i = i + 1;
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td align='left' class='borderThin'>" + info.DauSo + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoLuongGiamTru + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTien.ToString(formatNumber) + "</td>");
                        sb.Append("<td class='borderThin'>" + info.GhiChu + "</td>");
                        sb.Append("</tr>");

                        totalSoTienGiamTru += info.SoTien;
                        totalSoLuongGiamTru += info.SoLuongGiamTru;
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongGiamTru.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru.ToString(formatNumber) + "</td>");
                    sb.Append("<td class='borderThinTextBold'>&nbsp;</td>");
                }
                else
                {
                    sb.Append(@"<tr>
                    <td colspan='5'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                    <td colspan='10'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 13/03/2014
        /// Todo : Lấy dữ liệu dữ liệu đối soát bù cước dịch vụ giữa VAS
        /// </summary>
        /// <param name="phongBanId">
        ///     -1: Của cả 3 trung tâm
        ///     53 : VNP1_GQKN
        ///     62 : VNP2_GQKN
        ///     67 : VNP3_GQKN
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static string BaoCaoDoiSoatDoanhThuBUCuocDVGiuaVASVaCSKH(int phongBanId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().BaoCaoDoiSoatDoanhThuBUCuocDVGiuaVASVaCSKH(phongBanId, fromDate, toDate);
            if (listKhieuNaiInfo != null && listKhieuNaiInfo.Count > 0)
            {
                decimal totalCSKH = 0;
                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    totalCSKH += listKhieuNaiInfo[i].SoTien;

                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].MaDichVu + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoTien.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'></td>");
                    sb.Append("<td align='center' class='borderThin'></td>");
                    sb.Append("<td align='center' class='borderThin'></td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCSKH.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'></td>");
                sb.Append("<td align='center' class='borderThinTextBold'></td>");
                sb.Append("<td align='center' class='borderThinTextBold'></td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        public static string DanhSachKhieuNai(int khuVucID, int donViID, int fromDate, int toDate, int loaiKhieuNaiID, int linhVucChungID, int linhVucConID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //List<KhieuNai_ReportInfo> list = new ReportImpl().DanhSachKhieuNai(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                List<KhieuNai_ReportInfo> list = new ReportImpl().DanhSachKhieuNai_Solr(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                int i = 0;
                if (list != null && list.Count > 0)
                {

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        i = i + 1;
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (byte)info.TrangThai).Replace("_", " ") + "</td>");
                        sb.Append("<td class='borderThin'>" + info.MaKhieuNai + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), (byte)info.DoUuTien).Replace("_", " ") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td class='borderThin' align='center'>" + info.LoaiKhieuNai + "</td>");
                        sb.Append("<td class='borderThin'>" + info.NguoiXuLy + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LinhVucChung + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.LinhVucCon + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (info.NgayDongKN.Date == DateTime.MaxValue.Date || info.NgayDongKN.Date == DateTime.MinValue.Date ? "" : info.NgayDongKN.ToString("dd/MM/yyyy")) + "</td>");
                        sb.Append("<td class='borderThin' align='center'>" + info.NoiDungPA + "</td>");
                        sb.Append("<td class='borderThin'>" + info.NguoiTiepNhan + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.NguoiTienXuLyCap1 + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.NguoiTienXuLyCap2 + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.NguoiTienXuLyCap3 + "</td>");
                        sb.Append("<td class='borderThin' align='center'>" + info.NgayQuaHan.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td class='borderThin'>" + info.LDate.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                    <td colspan='17'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                    <td colspan='17'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 16/09/2013
        /// Todo : Tạo báo cáo tổng hợp theo khiếu nại
        /// </summary>
        /// <param name="donViID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="listDoiTacId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <returns></returns>
        public string BaoCaoTongHopTheoKhieuNai(int khuVucId, int phongBanXuLyId, DateTime fromDate, DateTime toDate, List<string> listDoiTacId, List<string> listLoaiKhieuNaiId,
                                                List<string> listLinhVucChungId, List<string> listLinhVucConId)
        {
            try
            {
                if (listDoiTacId == null || listDoiTacId.Count == 0)
                    return "Bạn phải chọn đối tác";

                string whereClause = string.Format("ID IN ('{0}'", listDoiTacId[0]);
                for (int i = 1; i < listDoiTacId.Count; i++)
                {
                    whereClause = string.Format("{0},'{1}'", whereClause, listDoiTacId[i]);
                }
                whereClause = string.Format("{0})", whereClause);
                List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,TenDoiTac", whereClause, "");


                StringBuilder sb = new StringBuilder();

                //DataSet dsReport = new ReportImpl().BaoCaoTongHopTheoKhieuNai(khuVucId, phongBanXuLyId, fromDate, toDate, listDoiTacId, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);
                DataSet dsReport = new ReportImpl().BaoCaoTongHopTheoKhieuNai_Solr(khuVucId, phongBanXuLyId, fromDate, toDate, listDoiTacId, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);
                DataSet dsToCalculateTotal = dsReport.Copy();

                // Biến totalPAKNOfPhongBan : Xác định tổng số khiếu nại của phòng bản (tổng khiếu nại từ các đối tác được chọn thuộc phòng ban)
                int totalPAKNOfPhongBan = 0;

                int index = 1;
                if (dsReport != null && dsReport.Tables.Count > 2)
                {
                    string tenToXuLy = string.Empty;
                    if (phongBanXuLyId == 54 || phongBanXuLyId == 63 || phongBanXuLyId == 68)
                    {
                        tenToXuLy = "TỔ XLNV";
                    }
                    else if (phongBanXuLyId == 53 || phongBanXuLyId == 62 || phongBanXuLyId == 67)
                    {
                        tenToXuLy = "TỔ GQKN";
                    }
                    else if (phongBanXuLyId == 55 || phongBanXuLyId == 64 || phongBanXuLyId == 69)
                    {
                        tenToXuLy = "TỔ KS";
                    }
                    else
                    {
                        tenToXuLy = "TỔ OB";
                    }

                    sb.Append("<tr>");
                    sb.Append("<th rowspan='2'>STT</th>");
                    sb.Append("<th rowspan='2'>Loại khiếu nại</th>");
                    sb.Append(string.Format("<th colspan='{0}'>Tổng số PAKN tiếp nhận</th>", listDoiTacId.Count + 1));
                    sb.Append("<th rowspan='2'>TỔNG SỐ PAKN GIẢI QUYẾT</th>");
                    sb.Append(string.Format("<th rowspan='2'>TỔNG SỐ PAKN {0} GIẢI QUYẾT</th>", tenToXuLy));
                    sb.Append("<th rowspan='2'>Tổng số PAKN chuyển các đơn vị liên quan</th>");
                    //sb.Append("<th rowspan='2'>Đơn vị nhận chuyển tiếp khiếu nại</th>");
                    sb.Append("<th rowspan='2'>Số khiếu nại tiếp nhận trong tuần tồn đọng do quá hạn</th>");
                    sb.Append("<th rowspan='2'>Số khiếu nại tồn đọng do quá hạn tại phòng ban thực tế tới thời điểm lấy báo cáo</th>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");

                    for (int indexDoiTacId = 0; indexDoiTacId < listDoiTacId.Count; indexDoiTacId++)
                    {
                        sb.Append(string.Format("<th>{0}</th>", GetTenDoiTac(listDoiTac, ConvertUtility.ToInt32(listDoiTacId[indexDoiTacId]))));
                    }

                    sb.Append(string.Format("<th>Tổng {0}</th>", ""));
                    sb.Append("</tr>");

                    // Hiển thị số thứ tự các cột
                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold'>&nbsp;</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>&nbsp;</td>");

                    for (int indexDoiTacId = 1; indexDoiTacId <= listDoiTacId.Count; indexDoiTacId++)
                    {
                        sb.Append(string.Format("<td align='center' class='borderThinTextBold'>{0}</td>", indexDoiTacId));
                    }

                    sb.Append(string.Format("<td align='center' class='borderThinTextBold'>{0}</td>", listDoiTacId.Count + 1));
                    sb.Append("<td align='center' class='borderThinTextBold'>" + (listDoiTacId.Count + 2) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + (listDoiTacId.Count + 3) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + (listDoiTacId.Count + 4) + "</td>");
                    //sb.Append("<td>" + listDoiTacId.Count + 2 + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + (listDoiTacId.Count + 5) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + (listDoiTacId.Count + 6) + "</td>");
                    sb.Append("</tr>");

                    if (dsReport != null && dsReport.Tables.Count > 0)
                    {
                        string sWindowOpen = "<a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/emptypage.aspx?page=soluongkhieunaiphongbantiepnhan&phongBanXuLyTruocId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a>";
                        string sWindowOpenDanhSachKNChuaDong = "<a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/emptypage.aspx?page=DanhSachKhieuNaichuadong&phongBanXuLyId=" + phongBanXuLyId.ToString() + "&loaiKhieuNaiId={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a>";
                        if (dsReport.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow rowLoaiKhieuNai in dsReport.Tables[0].Rows)
                            {
                                int curIndex = index;
                                sb.Append("<tr>");
                                sb.Append("<td class='borderThin' align='center' valign='top' rowspan='rowspanX'>" + index.ToString() + "</td>");
                                sb.Append("<td class='borderThin'>" + rowLoaiKhieuNai["LoaiKhieuNai"].ToString() + "</td>");

                                int total = 0;
                                for (int indexDoiTac = 0; indexDoiTac < listDoiTacId.Count; indexDoiTac++)
                                {
                                    sb.Append("<td align='center' class='borderThin'>" + (rowLoaiKhieuNai[listDoiTacId[indexDoiTac].ToString()].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai[listDoiTacId[indexDoiTac].ToString()].ToString()) + "</td>");
                                    total += ConvertUtility.ToInt32(rowLoaiKhieuNai[listDoiTacId[indexDoiTac].ToString()], 0);
                                }
                                totalPAKNOfPhongBan += total;
                                sb.Append("<td align='center' class='borderThin'>" + (total.ToString() == "0" ? "&nbsp;" : total.ToString()) + "</td>");
                                sb.Append("<td align='center' class='borderThin'>" + (rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"].ToString()) + "</td>");
                                sb.Append("<td align='center' class='borderThin'>" + (rowLoaiKhieuNai["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString()) + "</td>");
                                //sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/emptypage.aspx?page=soluongkhieunaiphongbantiepnhan&phongBanXuLyTruocId=" + phongBanXuLyId.ToString() + "&loaiKhieuNaiId=" + rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + (rowLoaiKhieuNai["TongSoPAKNChuyenDonViLienQuan"].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai["TongSoPAKNChuyenDonViLienQuan"].ToString()) + "</a></td>");
                                sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpen, rowLoaiKhieuNai["LoaiKhieuNaiId"], (rowLoaiKhieuNai["TongSoPAKNChuyenDonViLienQuan"].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai["TongSoPAKNChuyenDonViLienQuan"].ToString())) + "</td>");
                                //sb.Append("<td align='center' class='borderThin'>" + (rowLoaiKhieuNai["DonViNhanChuyenTiepKhieuNai"].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai["DonViNhanChuyenTiepKhieuNai"].ToString()) + "</td>");
                                sb.Append("<td align='center' class='borderThin'>" + (rowLoaiKhieuNai["TongSoKhieuNaiTonDongDoQuaHan"].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai["TongSoKhieuNaiTonDongDoQuaHan"].ToString()) + "</td>");
                                sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpenDanhSachKNChuaDong, rowLoaiKhieuNai["LoaiKhieuNaiId"], (rowLoaiKhieuNai["TongSoKhieuNaiTonDongHienTai"].ToString() == "0" ? "&nbsp;" : rowLoaiKhieuNai["TongSoKhieuNaiTonDongHienTai"].ToString())) + "</td>");
                                sb.Append("</tr>");

                                //index++;


                                // Lĩnh vực chung
                                if (dsReport.Tables.Count > 1 && dsReport.Tables[1].Rows.Count > 0)
                                {
                                    StringBuilder sbLinhVucChung = BaoCaoTongHopTheoKhieuNai_DisplayLinhVucChung(ref index, listDoiTacId, dsReport.Tables[1], dsReport.Tables[2],
                                                                                                                    rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString(), ref totalPAKNOfPhongBan, sWindowOpen, sWindowOpenDanhSachKNChuaDong);
                                    sb.Append(sbLinhVucChung.ToString());
                                } // end if (dsReport.Tables[1].Rows.Count > 0)

                                int totalRow = index - curIndex + 1;
                                sb.Replace("rowspanX", totalRow.ToString());
                                index = curIndex + 1;
                            }
                        }

                        if (dsReport.Tables[1].Rows.Count > 0)
                        {
                            StringBuilder sbLinhVucChung = BaoCaoTongHopTheoKhieuNai_DisplayLinhVucChung(ref index, listDoiTacId, dsReport.Tables[1], dsReport.Tables[2], "", ref totalPAKNOfPhongBan, sWindowOpen, sWindowOpenDanhSachKNChuaDong);
                            if (sbLinhVucChung != null)
                            {
                                sb.Append(sbLinhVucChung.ToString());
                            }
                        }

                        if (dsReport.Tables[2].Rows.Count > 0)
                        {
                            StringBuilder sbLinhVucCon = BaoCaoTongHopTheoKhieuNai_DisplayLinhVucCon(ref index, listDoiTacId, dsReport.Tables[2], "", ref totalPAKNOfPhongBan, sWindowOpen, sWindowOpenDanhSachKNChuaDong);
                            if (sbLinhVucCon != null)
                            {
                                sb.Append(sbLinhVucCon);
                            }
                        }

                        // Hiển thị giá trị tổng
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");

                        for (int indexDoiTac = 0; indexDoiTac < listDoiTacId.Count; indexDoiTac++)
                        {
                            sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn(listDoiTacId[indexDoiTac].ToString(), dsToCalculateTotal) + "</td>");
                        }
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalPAKNOfPhongBan == 0 ? "&nbsp;" : totalPAKNOfPhongBan.ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("TongSoPAKNGiaiQuyetDuoc", dsToCalculateTotal) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("TongSoPAKNToXLNVGiaiQuyetDuoc", dsToCalculateTotal) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("TongSoPAKNChuyenDonViLienQuan", dsToCalculateTotal) + "</td>");
                        //sb.Append("<td align='center' class='borderThin'>" + GetTotalOfColumn("DonViNhanChuyenTiepKhieuNai", dsToCalculateTotal) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("TongSoKhieuNaiTonDongDoQuaHan", dsToCalculateTotal) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + GetTotalOfColumn("TongSoKhieuNaiTonDongHienTai", dsToCalculateTotal) + "</td>");
                        sb.Append("</tr>");
                    }// end if (dsReport != null && dsReport.Tables.Count > 2)                    
                }
                else
                {
                    sb.Append(@"<tr>
                    <td colspan='17'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                    <td colspan='17'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 25/09/2013
        /// Todo : Thực hiện báo cáo theo khiếu nại (sử dụng lệnh Sql)
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <returns></returns>
        public string BaoCaoTheoLoaiKhieuNai(string week, int khuVucId, int phongBanXuLyId, DateTime fromDate, DateTime toDate, List<string> listLoaiKhieuNaiId, List<string> listLinhVucChungId,
                                                     List<string> listLinhVucConId, out string noiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan, out string noiDungSoLieuHoTroVNPTTT, bool isExportExcel)
        {
            string sNoiDungCongViec = string.Empty;
            noiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan = string.Empty;
            noiDungSoLieuHoTroVNPTTT = string.Empty;
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                int indexNoiDungCongViec = 0;
                int indexPPSDuocGiaiQuyet = 3;
                int indexHoTroVNPTTT = 4;

                //DataSet dsResult = new ReportImpl().BaoCaoTheoLoaiKhieuNaiUseSql(khuVucId, phongBanXuLyId, fromDate, toDate, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);
                DataSet dsResult = new ReportImpl().BaoCaoTongHopKhieuNaiTheoToGQKN_Solr(khuVucId, phongBanXuLyId, fromDate, toDate, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);

                if (dsResult != null)
                {
                    DataTable dtLoaiKhieuNai = RemoveRowHasNotValue("LoaiKhieuNaiId", dsResult.Tables[0]);
                    DataTable dtLinhVucChung = RemoveRowHasNotValue("LinhVucChungId,ParentId", dsResult.Tables[1]);
                    DataTable dtLinhVucCon = RemoveRowHasNotValue("LinhVucConId,ParentId", dsResult.Tables[2]);
                    DataTable dtPPS = RemoveRowHasNotValue("LinhVucConId", dsResult.Tables[3]);

                    StringBuilder sb = new StringBuilder();

                    if (dsResult.Tables.Count > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th rowspan='2'>STT</th>");
                        sb.Append("<th rowspan='2'>Công việc</th>");
                        sb.Append(string.Format("<th>Lũy kế KN đã GQ đến đầu tuần {0}</th>", week));
                        sb.Append(string.Format("<th>Lũy kế KN tồn đọng đầu tuần {0}</th>", week));
                        sb.Append("<th>Số lượng tiếp nhận trong tuần</th>");
                        sb.Append("<th>Số lượng đã giải quyết trong tuần</th>");
                        sb.Append("<th>Số lượng tồn đọng trong tuần do quá hạn</th>");
                        sb.Append(string.Format("<th>Lũy kế KN đã GQ đến cuối tuần {0}</th>", week));
                        sb.Append(string.Format("<th>Lũy kế KN tồn đọng do quá hạn cuối tuần {0}</th>", week));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>1</th>");
                        sb.Append("<th>2</th>");
                        sb.Append("<th>3</th>");
                        sb.Append("<th>4</th>");
                        sb.Append("<th>5 = 3 - 4</th>");
                        sb.Append("<th>6 = 1 + 4</th>");
                        sb.Append("<th>7 = 2 + 5</th>");
                        sb.Append("</tr>");

                        int index = 1;
                        int totalLuyKeDaGiaiQuyetDenDauTuanX = 0;
                        int totalLuyKeTonDongDauTuanX = 0;
                        int totalSoLuongTiepNhanTrongTuan = 0;
                        int totalSoLuongDaGiaiQuyetTrongTuan = 0;

                        foreach (DataRow rowLoaiKhieuNai in dsResult.Tables[0].Rows)
                        {
                            totalLuyKeDaGiaiQuyetDenDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0);
                            totalLuyKeTonDongDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"], 0);
                            totalSoLuongTiepNhanTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0);
                            totalSoLuongDaGiaiQuyetTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0);

                            sb.Append("<tr>");
                            sb.Append("<td class='borderThinTextBold' align='center' valign='top'>" + index.ToString() + "</td>");
                            sb.Append("<td class='borderThinTextBold'>" + rowLoaiKhieuNai["LoaiKhieuNai"].ToString() + "</td>");

                            sb.Append("<td align='center' class='borderThinTextBold'>" + rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThinTextBold'>" + rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThinTextBold'>" + rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThinTextBold'>" + rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThinTextBold'>" + (ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                            sb.Append("<td align='center' class='borderThinTextBold'>" + (ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                            sb.Append("<td align='center' class='borderThinTextBold'>" + (ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                            sb.Append("</tr>");

                            // Lĩnh vực chung
                            if (dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                            {
                                StringBuilder sbLinhVucChung = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucChung(index, dsResult.Tables[1], dsResult.Tables[2], rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString());
                                sb.Append(sbLinhVucChung.ToString());
                            } // end if (list.Tables[1].Rows.Count > 0)

                            index++;
                        } // end foreach (DataRow rowLoaiKhieuNai in dsResult.Tables[0].Rows)

                        string lastRow = string.Format("Tổng số ({0}) = (1", index);
                        for (int i = 2; i < index; i++)
                        {
                            lastRow = string.Format("{0}+{1}", lastRow, i);
                        }
                        lastRow += ")";
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThinTextBold' align='center' valign='top'>" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThinTextBold'>" + lastRow + "</td>");

                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalLuyKeDaGiaiQuyetDenDauTuanX.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalLuyKeTonDongDauTuanX.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongTiepNhanTrongTuan.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongDaGiaiQuyetTrongTuan.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalSoLuongTiepNhanTrongTuan - totalSoLuongDaGiaiQuyetTrongTuan).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalLuyKeDaGiaiQuyetDenDauTuanX + totalSoLuongDaGiaiQuyetTrongTuan).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalLuyKeTonDongDauTuanX + totalSoLuongTiepNhanTrongTuan - totalSoLuongDaGiaiQuyetTrongTuan).ToString() + "</td>");
                        sb.Append("</tr>");

                        sNoiDungCongViec = sb.ToString();
                    } // end if (dsResult.Tables.Count > 0)

                    if (dsResult.Tables.Count > 1)
                    {
                        int totalCCT = 0;
                        int totalBuCuoc = 0;
                        decimal totalSoTien = 0;

                        sb = new StringBuilder();
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Mã dịch vụ</th>");
                        sb.Append("<th>Cấp chi tiết<br/><i>(đơn vị : thuê bao)</i></th>");
                        sb.Append("<th>Bù cước<br/><i>(đơn vị : thuê bao)</i></th>");
                        sb.Append("<th>Số tiền <i>(đơn vị : đồng)</i></th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < dsResult.Tables[indexPPSDuocGiaiQuyet].Rows.Count; i++)
                        {
                            DataRow row = dsResult.Tables[indexPPSDuocGiaiQuyet].Rows[i];

                            sb.Append("<tr>");
                            sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                            sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + row["MaDichVu"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + (row["CapChiTiet"].ToString() == "0" ? string.Empty : row["CapChiTiet"].ToString()) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + (row["BuCuoc"].ToString() == "0" ? string.Empty : row["BuCuoc"].ToString()) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + (row["SoTien"].ToString() == "0" ? string.Empty : row["SoTien"].ToString()) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                            sb.Append("</tr>");

                            if (row["CapChiTiet"].ToString() != "0")
                            {
                                totalCCT += ConvertUtility.ToInt32(row["CapChiTiet"], 0);
                            }

                            if (row["BuCuoc"].ToString() != "0")
                            {
                                totalBuCuoc += ConvertUtility.ToInt32(row["BuCuoc"], 0);
                            }

                            totalSoTien += ConvertUtility.ToDecimal(row["SoTien"], 0);
                        }

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThinTextBold' colspan='3'>Tổng số</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalCCT.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalBuCuoc.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTien.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>&nbsp;</td>");
                        sb.Append("</tr>");

                        noiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan = sb.ToString();
                    }

                    if (dsResult.Tables.Count > 2)
                    {
                        sb = new StringBuilder();

                        int totalCSLGoc = 0;
                        int totalIR = 0;
                        int totalSoLieuKhac = 0;

                        sb.Append("<tr>");
                        sb.Append("<th rowspan='2'>STT</th>");
                        sb.Append("<th rowspan='2'>VNPT TT</th>");
                        sb.Append("<th colspan='4'>Số lượng</th>");
                        sb.Append("<th rowspan='2'>Ghi chú</th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>Cấp số liệu gốc <br/> 1</th>");
                        sb.Append("<th >Phân tích số liệu IR <br/>2</th>");
                        sb.Append("<th>Giải thích số liệu khác <br/>3</th>");
                        sb.Append("<th>Tổng <br/>4 = 1+2+3</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < dsResult.Tables[indexHoTroVNPTTT].Rows.Count; i++)
                        {
                            DataRow row = dsResult.Tables[indexHoTroVNPTTT].Rows[i];
                            int capSoLieuGoc = ConvertUtility.ToInt32(row["CapSoLieuGoc"], 0);
                            int phanTichSoLieuIR = ConvertUtility.ToInt32(row["PhanTichSoLieuIR"], 0);
                            int giaiThichSoLieuKhac = ConvertUtility.ToInt32(row["GiaiThichSoLieuKhac"], 0);
                            if (capSoLieuGoc == 0 && phanTichSoLieuIR == 0 && giaiThichSoLieuKhac == 0)
                            {
                                dsResult.Tables[indexHoTroVNPTTT].Rows.RemoveAt(i);
                                i--;
                                continue;
                            }

                            sb.Append("<tr>");
                            sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                            sb.Append("<td class='borderThin'>" + row["MaTinh"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + capSoLieuGoc.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + phanTichSoLieuIR.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + giaiThichSoLieuKhac.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + (capSoLieuGoc + phanTichSoLieuIR + giaiThichSoLieuKhac).ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                            sb.Append("</tr>");

                            totalCSLGoc += capSoLieuGoc;
                            totalIR += phanTichSoLieuIR;
                            totalSoLieuKhac += giaiThichSoLieuKhac;
                        }

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>Tổng số</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalCSLGoc.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalIR.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLieuKhac.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalCSLGoc + totalIR + totalSoLieuKhac).ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                        sb.Append("</tr>");

                        noiDungSoLieuHoTroVNPTTT = sb.ToString();
                    }


                }
            }
            catch
            {

            }

            return sNoiDungCongViec;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 26/10/2013
        /// Todo : Hiển thị nội dung báo cáo tổng hợp của trung tâm tính cước
        /// </summary>       
        /// <param name="trungTamId"></param>
        /// <param name="phongBanId">
        ///     = -1 : Thống kê toàn bộ khiếu nại của tất cả các phòng ban thuộc trung tâm
        ///     != -1 : Thống kê khiếu nại của phongBanId truyền vào
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopTTTC(int doiTacId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanTTTC_Solr(trungTamId, phongBanId, fromDate, toDate);
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoDoiTac_Solr(doiTacId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakntttc&doiTacId=" + doiTacId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                    DataRow row = dtReport.Rows[0];
                    sb.Append("<tr>");
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", row["TenDoiTac"].ToString());
                    sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                    sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                    sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                    sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                    sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                    sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                    sb.AppendFormat("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 16/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string BaoCaoTongHopTTTC(int doiTacId, DateTime fromDate, DateTime toDate, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanTTTC_Solr(trungTamId, phongBanId, fromDate, toDate);
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoDoiTac_Solr(doiTacId, fromDate, toDate, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakntttc&doiTacId=" + doiTacId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNai_NhomId=" + loaiKhieuNai_NhomId + "&loaiKhieuNaiId=" + loaiKhieuNaiId + "&linhVucChungId=" + linhVucChungId + "&linhVucConId=" + linhVucConId + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                    DataRow row = dtReport.Rows[0];
                    sb.Append("<tr>");
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", row["TenDoiTac"].ToString());
                    sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                    sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                    sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                    sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                    sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                    sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                    sb.AppendFormat("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/08/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp phòng ban của TTTC
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="listPhongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopPhongBanTTTC(int doiTacId, List<int> listPhongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanDoiTac_Solr(doiTacId, listPhongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên phòng ban</th>");
                    sb.Append("<th>SL tồn kỳ trước</th>");
                    sb.Append("<th>SL tiếp nhận</th>");
                    sb.Append("<th>SL đã xử lý</th>");
                    sb.Append("<th>SL đã xử lý quá hạn</th>");
                    sb.Append("<th>SL tồn đọng</th>");
                    sb.Append("<th>SL tồn đọng quá hạn</th>");
                    sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknphongbantttc&phongbanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td class='borderThinTextBold'>{0}</td>", phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanId"])));
                        sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }

            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 17/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="listPhongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string BaoCaoTongHopPhongBanTTTC(int doiTacId, List<int> listPhongBanId, DateTime fromDate, DateTime toDate, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanDoiTac_Solr(doiTacId, listPhongBanId, fromDate, toDate, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên phòng ban</th>");
                    sb.Append("<th>SL tồn kỳ trước</th>");
                    sb.Append("<th>SL tiếp nhận</th>");
                    sb.Append("<th>SL đã xử lý</th>");
                    sb.Append("<th>SL đã xử lý quá hạn</th>");
                    sb.Append("<th>SL tồn đọng</th>");
                    sb.Append("<th>SL tồn đọng quá hạn</th>");
                    sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknphongbantttc&phongbanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNai_NhomId=" + loaiKhieuNai_NhomId + "&loaiKhieuNaiId=" + loaiKhieuNaiId + "&linhVucChungId=" + linhVucChungId + "&linhVucConId=" + linhVucConId + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td class='borderThinTextBold'>{0}</td>", phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanId"])));
                        sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }

            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/08/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp người dùng của từng phòng ban
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="listPhongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopNguoiDungPhongBanTTTC(int phongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_Solr(phongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên truy cập</th>");
                    sb.Append("<th>Tên người dùng</th>");
                    sb.Append("<th>SL tồn kỳ trước</th>");
                    sb.Append("<th>SL tiếp nhận</th>");
                    sb.Append("<th>SL đã xử lý</th>");
                    sb.Append("<th>SL đã xử lý quá hạn</th>");
                    sb.Append("<th>SL tồn đọng</th>");
                    sb.Append("<th>SL tồn đọng quá hạn</th>");
                    sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidungphongbantttc&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='9'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='9'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 17/04/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string BaoCaoTongHopNguoiDungPhongBanTTTC(int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_Solr(phongBanId, fromDate, toDate, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_V2_Solr(doiTacId, phongBanId, fromDate, toDate, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên truy cập</th>");
                    sb.Append("<th>Tên người dùng</th>");
                    sb.Append("<th>SL tồn kỳ trước</th>");
                    sb.Append("<th>SL tiếp nhận</th>");
                    sb.Append("<th>SL đã xử lý</th>");
                    sb.Append("<th>SL đã xử lý quá hạn</th>");
                    sb.Append("<th>SL tồn đọng</th>");
                    sb.Append("<th>SL tồn đọng quá hạn</th>");
                    sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidungphongbantttc&doiTacId=" + doiTacId + "&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNai_NhomId=" + loaiKhieuNai_NhomId + "&loaiKhieuNaiId=" + loaiKhieuNaiId + "&linhVucChungId=" + linhVucChungId + "&linhVucConId=" + linhVucConId + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='9'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='9'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang hai
        /// Created date : 26/10/2013
        /// Todo : Hiển thị nội dung báo cáo tổng hợp các phòng ban của trung tâm tính cước
        /// </summary>
        /// <returns></returns>
        //        public string BaoCaoTongHopPAKNTheoPhongBanTTTC(int trungTamId, int phongBanId, int fromDate, int toDate)
        //        {
        //            StringBuilder sb = new StringBuilder();

        //            try
        //            {
        //                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanTTTC_Solr(trungTamId, phongBanId, fromDate, toDate);
        //                if (dtReport != null && dtReport.Rows.Count > 0)
        //                {
        //                    int totalTiepNhan = 0;
        //                    int totalDaXuLy = 0;
        //                    int totalTonDong = 0;
        //                    int totalQuaHan = 0;

        //                    sb.Append("<tr>");
        //                    sb.Append("<th>STT</th>");
        //                    sb.Append("<th>Phòng ban</th>");
        //                    sb.Append("<th>Tiếp nhận</th>");
        //                    sb.Append("<th>Đã xử lý</th>");
        //                    sb.Append("<th>Tồn đọng</th>");
        //                    sb.Append("<th>PAKN quá hạn</th>");
        //                    sb.Append("</tr>");                

        //                    for (int i = 0; i < dtReport.Rows.Count; i++)
        //                    {
        //                        DataRow row = dtReport.Rows[i];
        //                        int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
        //                        int soLuongDaXuLy = ConvertUtility.ToInt32(row["SLDaXuLy"], 0);
        //                        int soLuongTonDong = ConvertUtility.ToInt32(row["SLTonDong"], 0);
        //                        int SoLuongQuaHan = ConvertUtility.ToInt32(row["SLQuaHan"], 0);

        //                        totalTiepNhan += soLuongTiepNhan;
        //                        totalDaXuLy += soLuongDaXuLy;
        //                        totalTonDong += soLuongTonDong;
        //                        totalQuaHan += SoLuongQuaHan;

        //                        sb.Append("<tr>");
        //                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
        //                        sb.Append("<td class='borderThin'>" + row["TenPhongBan"].ToString() + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhan.ToString(formatNumber) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + soLuongDaXuLy.ToString(formatNumber) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + soLuongTonDong.ToString(formatNumber) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + SoLuongQuaHan.ToString(formatNumber) + "</td>");
        //                        sb.Append("</tr>");
        //                    }

        //                    sb.Append("<tr>");
        //                    sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTiepNhan.ToString(formatNumber) + "</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalDaXuLy.ToString(formatNumber) + "</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTonDong.ToString(formatNumber) + "</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalQuaHan.ToString(formatNumber) + "</td>");
        //                    sb.Append("</tr>");
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                                <td colspan='6'>
        //                                    Chưa có dữ liệu báo cáo
        //                                </td>
        //                            </tr>");
        //                }
        //            }
        //            catch
        //            {
        //                sb.Append(@"<tr>
        //                                <td colspan='6'>
        //                                    Chưa có dữ liệu báo cáo
        //                                </td>
        //                            </tr>");
        //            }

        //            return sb.ToString();
        //        }

        /// <summary>
        /// Author : Phi Hoang hai
        /// Created date : 20/01/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp các phòng ban của trung tâm tính cước
        /// </summary>
        /// <returns></returns>
        public string BaoCaoTongHopPAKNQuaHanTheoPhongBanTTTC(int doiTacId, int phongBanId, int fromDate, int toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                DataTable dtReport = new ReportSqlImpl().CountKhieuNaiTonDongVaQuaHanTaiThoiDiemHienTai(doiTacId, phongBanId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    int totalSoLuongQuaHan = 0;
                    int totalSoLuongTonDong = 0;

                    string cellTotalContent = "<td align='center' clclass='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotondongvaquahantttc&doiTacId={0}&reportType={1}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{2}</a></td>";
                    string cellContent = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotondongvaquahantttc&doiTacId={0}&phongBanId={1}&reportType={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{3}</a></td>";

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên phòng ban</th>");
                    sb.Append("<th>Số lượng tồn đọng</th>");
                    sb.Append("<th>Số lượng quá hạn</th>");
                    sb.Append("</tr>");
                    for (int i = 0; i < dtReport.Rows.Count; i++)
                    {
                        DataRow row = dtReport.Rows[i];

                        int soLuongTonDong = ConvertUtility.ToInt32(row["SoLuongTonDong"], 0);
                        int soLuongQuaHan = ConvertUtility.ToInt32(row["SoLuongQuaHan"], 0);

                        totalSoLuongTonDong += soLuongTonDong;
                        totalSoLuongQuaHan += soLuongQuaHan;

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenPhongBan"].ToString() + "</td>");
                        sb.AppendFormat(cellContent, doiTacId, row["PhongBanId"], 1, soLuongTonDong.ToString(formatNumber));
                        sb.AppendFormat(cellContent, doiTacId, row["PhongBanId"], 2, soLuongQuaHan.ToString(formatNumber));
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
                    sb.AppendFormat(cellTotalContent, doiTacId, 1, totalSoLuongTonDong.ToString(formatNumber));
                    sb.AppendFormat(cellTotalContent, doiTacId, 2, totalSoLuongQuaHan.ToString(formatNumber));

                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 17/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string BaoCaoTongHopPAKNQuaHanTheoPhongBanTTTC(int doiTacId, int phongBanId, int fromDate, int toDate, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                DataTable dtReport = new ReportSqlImpl().CountKhieuNaiTonDongVaQuaHanTaiThoiDiemHienTai(doiTacId, phongBanId, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    int totalSoLuongQuaHan = 0;
                    int totalSoLuongTonDong = 0;

                    string cellTotalContent = "<td align='center' clclass='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotondongvaquahantttc&doiTacId={0}&reportType={1}&loaiKhieuNai_NhomId={3}&loaiKhieuNaiId={4}&linhVucChungId={5}&linhVucConId={6}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{2}</a></td>";
                    string cellContent = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotondongvaquahantttc&doiTacId={0}&phongBanId={1}&reportType={2}&loaiKhieuNai_NhomId={4}&loaiKhieuNaiId={5}&linhVucChungId={6}&linhVucConId={7}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{3}</a></td>";

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên phòng ban</th>");
                    sb.Append("<th>Số lượng tồn đọng</th>");
                    sb.Append("<th>Số lượng quá hạn</th>");
                    sb.Append("</tr>");
                    for (int i = 0; i < dtReport.Rows.Count; i++)
                    {
                        DataRow row = dtReport.Rows[i];

                        int soLuongTonDong = ConvertUtility.ToInt32(row["SoLuongTonDong"], 0);
                        int soLuongQuaHan = ConvertUtility.ToInt32(row["SoLuongQuaHan"], 0);

                        totalSoLuongTonDong += soLuongTonDong;
                        totalSoLuongQuaHan += soLuongQuaHan;

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenPhongBan"].ToString() + "</td>");
                        sb.AppendFormat(cellContent, doiTacId, row["PhongBanId"], 1, soLuongTonDong.ToString(formatNumber), loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                        sb.AppendFormat(cellContent, doiTacId, row["PhongBanId"], 2, soLuongQuaHan.ToString(formatNumber), loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
                    sb.AppendFormat(cellTotalContent, doiTacId, 1, totalSoLuongTonDong.ToString(formatNumber), loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                    sb.AppendFormat(cellTotalContent, doiTacId, 2, totalSoLuongQuaHan.ToString(formatNumber), loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);

                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        public string BaoCaoTongHopPAKNTheoPhongBanVNPTTT(int doitacId, int phongBanId, int fromDate, int toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanVNPTTT_Solr(doitacId, phongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    int totalTiepNhan = 0;
                    int totalDaXuLy = 0;
                    int totalTonDong = 0;
                    int totalSoluongMangSang = 0;
                    int totalChatLuongDV = 0;
                    decimal totalGiamCuoc = 0;
                    int totalLuyKeNam = 0;
                    int totalChatLuongPv = 0;
                    int totalSoCuoc = 0;
                    int totalSoKhac = 0;
                    int totalLoiNhanVien = 0;
                    int totlalLoiHeThong = 0;
                    int totalLoiChamSocKH = 0;
                    int totalLyDoKhac = 0;
                    for (int i = 0; i < dtReport.Rows.Count; i++)
                    {
                        DataRow row = dtReport.Rows[i];
                        int SoLuongMangSang = ConvertUtility.ToInt32(row["SLMangSang"], 0);
                        int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);

                        int soLuongDaXuLy = ConvertUtility.ToInt32(row["SLGiaiQuyet"], 0);
                        int soLuongTonDong = ConvertUtility.ToInt32(row["SLTonDong"], 0);

                        int soLuongLuyKeNam = ConvertUtility.ToInt32(row["SLLuyKeNam"], 0);
                        int soChatLuongDV = ConvertUtility.ToInt32(row["SLChatLuongDV"], 0);
                        int soChatLuongPV = ConvertUtility.ToInt32(row["SLChatLuongPV"], 0);
                        int soCuoc = ConvertUtility.ToInt32(row["SLCuoc"], 0);
                        int soKhac = ConvertUtility.ToInt32(row["SLKhac"], 0);

                        int soLoiNhanVien = ConvertUtility.ToInt32(row["SLLoiNhanVien"], 0);
                        int soLoiHeThong = ConvertUtility.ToInt32(row["SLLoiHeThong"], 0);
                        int soLoiChamSocKhachHang = ConvertUtility.ToInt32(row["SLChamSocKhachHang"], 0);
                        int soLyDoKhac = ConvertUtility.ToInt32(row["SLLyDoKhac"], 0);
                        var soGiamCuoc = ConvertUtility.ToDecimal(row["SLGiamCuoc"], 0);
                        totalTiepNhan += soLuongTiepNhan;
                        totalDaXuLy += soLuongDaXuLy;
                        totalTonDong += soLuongTonDong;
                        totalSoluongMangSang += SoLuongMangSang;
                        totalChatLuongDV += soChatLuongDV;
                        totalGiamCuoc += soGiamCuoc;
                        totalLuyKeNam += soLuongLuyKeNam;
                        totalChatLuongPv += soChatLuongPV;
                        totalSoCuoc += soCuoc;
                        totalSoKhac += soKhac;
                        totalLoiNhanVien += soLoiNhanVien;
                        totlalLoiHeThong += soLoiHeThong;
                        totalLoiChamSocKH += soLoiChamSocKhachHang;
                        totalLyDoKhac += soLyDoKhac;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");

                        sb.Append("<td class='borderThin'>" + row["TenPhongBan"].ToString() + "</td>");
                        if (SoLuongMangSang > 0)

                            sb.Append("<td align='center' class='borderThin'>" + SoLuongMangSang.ToString(formatNumber) + "</td>");
                        else
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        sb.Append("<td align='center' class='borderThin'>" + ((soLuongTiepNhan > 0) ? soLuongTiepNhan.ToString(formatNumber) : "0") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + ((soLuongDaXuLy > 0) ? soLuongDaXuLy.ToString(formatNumber) : "0") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongTonDong.ToString(formatNumber) + "</td>");
                        if (soGiamCuoc == 0)
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > " + soGiamCuoc.ToString(formatNumber) + " </td>");
                        }

                        if (soLuongLuyKeNam > 0)
                            sb.Append("<td align='center' class='borderThin'>" + soLuongLuyKeNam.ToString(formatNumber) + "</td>");
                        else
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        if (soChatLuongDV > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + soChatLuongDV.ToString(formatNumber) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        if (soChatLuongPV > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + soChatLuongPV.ToString(formatNumber) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        if (soCuoc > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + soCuoc.ToString(formatNumber) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        if (soKhac > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + soKhac.ToString(formatNumber) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        if (soLoiNhanVien > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + soLoiNhanVien.ToString(formatNumber) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        if (soLoiHeThong > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + soLoiHeThong.ToString(formatNumber) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        if (soLoiChamSocKhachHang > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + soLoiChamSocKhachHang.ToString(formatNumber) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center' class='borderThin' > 0 </td>");
                        }
                        sb.Append("<td align='center' class='borderThin' > " + soLyDoKhac.ToString(formatNumber) + " </td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoluongMangSang.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTiepNhan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalDaXuLy.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTonDong.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalGiamCuoc.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalLuyKeNam.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalChatLuongDV.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalChatLuongPv.ToString(formatNumber) + "</td>");

                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoCuoc.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoKhac.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalLoiNhanVien.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totlalLoiHeThong.ToString(formatNumber) + " </td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalLoiChamSocKH.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalLyDoKhac.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='15'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='15'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }
        /// <summary>
        /// Bao cao tong hop pakn theo nguoi dung tinh thanh
        /// Author: Nguyen Chi Quang
        /// Ngay 02/06/2014
        /// </summary>
        /// <param name="doitacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="nguoiSuDungId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopPAKNTheoNguoiDungVNPTTT(int doitacId, int phongBanId, int nguoiSuDungId, int fromDate, int toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungVNPTTT_Solr(doitacId, phongBanId, nguoiSuDungId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    int totalTiepNhan = 0;
                    int totalDaXuLy = 0;
                    int totalTonDong = 0;
                    int totalQuaHan = 0;

                    for (int i = 0; i < dtReport.Rows.Count; i++)
                    {
                        DataRow row = dtReport.Rows[i];
                        int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
                        int soLuongDaXuLy = ConvertUtility.ToInt32(row["SLDaXuLy"], 0);
                        int soLuongTonDong = ConvertUtility.ToInt32(row["SLTonDong"], 0);
                        int SoLuongQuaHan = ConvertUtility.ToInt32(row["SLQuaHan"], 0);

                        totalTiepNhan += soLuongTiepNhan;
                        totalDaXuLy += soLuongDaXuLy;
                        totalTonDong += soLuongTonDong;
                        totalQuaHan += SoLuongQuaHan;

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenNguoiDung"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakntheonguoidungvnpttt&phongBanId=" + phongBanId.ToString() + "&doitacId=" + doitacId + "&username=" + row["TenTruyCap"] + "&fromDate=" + fromDate.ToString() + "&toDate=" + toDate.ToString() + "&reportType=11','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTiepNhan.ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakntheonguoidungvnpttt&phongBanId=" + phongBanId.ToString() + "&doitacId=" + doitacId + "&username=" + row["TenTruyCap"] + "&fromDate=" + fromDate.ToString() + "&toDate=" + toDate.ToString() + "&reportType=21','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongDaXuLy.ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakntheonguoidungvnpttt&phongBanId=" + phongBanId.ToString() + "&doitacId=" + doitacId + "&username=" + row["TenTruyCap"] + "&fromDate=" + fromDate.ToString() + "&toDate=" + toDate.ToString() + "&reportType=22','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTonDong.ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakntheonguoidungvnpttt&phongBanId=" + phongBanId.ToString() + "&doitacId=" + doitacId + "&username=" + row["TenTruyCap"] + "&fromDate=" + fromDate.ToString() + "&toDate=" + toDate.ToString() + "&reportType=23','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + SoLuongQuaHan.ToString(formatNumber) + "</a></td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTiepNhan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalDaXuLy.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTonDong.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalQuaHan.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='6'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='6'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 26/10/2013
        /// Todo : Hiển thị nội dung báo cáo tổng hợp của người dùng của trung tâm tính cước
        /// </summary>
        /// <returns></returns>
        //        public string BaoCaoTongHopPAKNTheoNguoiDungTTTC(int trungTamId, int phongBanId, int nguoiSuDungId, int fromDate, int toDate)
        //        {
        //            StringBuilder sb = new StringBuilder();

        //            try
        //            {
        //                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungTTTC_Solr(trungTamId, phongBanId, nguoiSuDungId, fromDate, toDate);
        //                if (dtReport != null && dtReport.Rows.Count > 0)
        //                {
        //                    int totalTiepNhan = 0;
        //                    int totalDaXuLy = 0;
        //                    int totalTonDong = 0;
        //                    int totalQuaHan = 0;

        //                    for (int i = 0; i < dtReport.Rows.Count; i++)
        //                    {
        //                        DataRow row = dtReport.Rows[i];
        //                        int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
        //                        int soLuongDaXuLy = ConvertUtility.ToInt32(row["SLDaXuLy"], 0);
        //                        int soLuongTonDong = ConvertUtility.ToInt32(row["SLTonDong"], 0);
        //                        int SoLuongQuaHan = ConvertUtility.ToInt32(row["SLQuaHan"], 0);

        //                        totalTiepNhan += soLuongTiepNhan;
        //                        totalDaXuLy += soLuongDaXuLy;
        //                        totalTonDong += soLuongTonDong;
        //                        totalQuaHan += SoLuongQuaHan;

        //                        sb.Append("<tr>");
        //                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
        //                        sb.Append("<td class='borderThin'>" + row["TenNguoiDung"].ToString() + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhan.ToString(formatNumber) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + soLuongDaXuLy.ToString(formatNumber) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + soLuongTonDong.ToString(formatNumber) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + SoLuongQuaHan.ToString(formatNumber) + "</td>");
        //                        sb.Append("</tr>");
        //                    }

        //                    sb.Append("<tr>");
        //                    sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTiepNhan.ToString(formatNumber) + "</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalDaXuLy.ToString(formatNumber) + "</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalTonDong.ToString(formatNumber) + "</td>");
        //                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalQuaHan.ToString(formatNumber) + "</td>");
        //                    sb.Append("</tr>");
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                                <td colspan='6'>
        //                                    Chưa có dữ liệu báo cáo
        //                                </td>
        //                            </tr>");
        //                }
        //            }
        //            catch
        //            {
        //                sb.Append(@"<tr>
        //                                <td colspan='6'>
        //                                    Chưa có dữ liệu báo cáo
        //                                </td>
        //                            </tr>");
        //            }

        //            return sb.ToString();
        //        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 28/10/2013
        /// Todo : Báo cáo chi tiết theo người dùng của trung tâm tính cước
        /// </summary>
        /// <param name="nguoiDungId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoChiTietPAKNTheoNguoiDungTTTC_Solr(int trungTamId, int phongBanXuLyId, string listTenTruyCap, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtResult = new ReportImpl().BaoCaoChiTietPAKNTheoNguoiDungTTTC_Solr(trungTamId, phongBanXuLyId, listTenTruyCap, fromDate, toDate, pageIndex, pageSize);
                if (dtResult == null || dtResult.Rows.Count == 0)
                    return string.Empty;

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow row = dtResult.Rows[i];
                    string url = string.Format("/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN={0}", row["KhieuNaiId"].ToString());
                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center' valign='top'>" + (i + 1).ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["TrangThai"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'><a href='" + url + "'>" + GetDataImpl.GetMaTuDong("PA", row["KhieuNaiId"], 10) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["LoaiKhieuNai"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["NguoiTienXuLy"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["NguoiXuLy"].ToString() + "</td>");
                    //sb.Append("<td align='center' class='borderThin'>" + row["NoiDungPA"].ToString() + "</td>");
                    sb.Append("</tr>");
                }
            }
            catch
            {
                return string.Empty;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/01/2014
        /// Todo : Báo cáo phối hợp giải quyết khiếu nại
        /// </summary>
        /// <param name="trungTamId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="nguoiSuDungId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoPhoiHopGQKNTTTC(int trungTamId, int phongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_Solr(phongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên truy cập</th>");
                    sb.Append("<th>Tên người dùng</th>");
                    sb.Append("<th>SL đã xử lý</th>");
                    sb.Append("<th>SL đã xử lý quá hạn</th>");
                    sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidungphongbantttc&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='5'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='5'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 17/04/2015
        /// </summary>
        /// <param name="trungTamId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string BaoCaoPhoiHopGQKNTTTC(int trungTamId, int phongBanId, DateTime fromDate, DateTime toDate, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_V2_Solr(trungTamId, phongBanId, fromDate, toDate, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên truy cập</th>");
                    sb.Append("<th>Tên người dùng</th>");
                    sb.Append("<th>SL đã xử lý</th>");
                    sb.Append("<th>SL đã xử lý quá hạn</th>");
                    sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        //string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidungphongbantttc&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidungphongbantttc&doiTacId=" + trungTamId + "&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNai_NhomId=" + loaiKhieuNai_NhomId + "&loaiKhieuNaiId=" + loaiKhieuNaiId + "&linhVucChungId=" + linhVucChungId + "&linhVucConId=" + linhVucConId + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='5'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='5'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/10/2013
        /// Todo : Hiển thị báo cáo biểu đồ số lượng khiếu nại tiếp nhận trong các khoảng thời gian
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="listDate"></param>
        /// <returns></returns>
        public DataTable BaoCaoBieuDoSoLuongKhieuNai(int doiTacTiepNhanId, int phongBanTiepNhanId, byte reportType, string listLoaiKhieuNaiId, string listLinhVucChungId, string listLinhVucConId, string listDate)
        {
            try
            {
                ReportImpl reportImpl = new ReportImpl();
                return reportImpl.BaoCaoBieuDoSoLuongKhieuNai_Solr(doiTacTiepNhanId, phongBanTiepNhanId, reportType, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, listDate);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/10/2013
        /// Todo : Hiển thị báo cáo biểu đồ số lượng khiếu nại tiếp nhận trong các khoảng thời gian
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="phongBanId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>        
        /// <returns></returns>
        public DataTable BaoCaoBieuDoSoLuongKhieuNaiChoXuLy(byte reportType, int phongBanId, string listLoaiKhieuNaiId, string listLinhVucChungId, string listLinhVucConId)
        {
            try
            {
                ReportImpl reportImpl = new ReportImpl();
                return reportImpl.BaoCaoBieuDoSoLuongKhieuNaiChoXuLy_Solr(reportType, phongBanId, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 06/11/2013
        /// Todo : Lấy số lượng khiếu nại trong khoảng thời gian truyền vào
        ///     - Tiếp nhận
        ///     - Đã xử lý xong
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet BieuDoSoLuongKhieuNaiCuaPhongBanTheoThoiGian(int phongBanXuLyId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                ReportImpl reportImpl = new ReportImpl();
                return reportImpl.BieuDoSoLuongKhieuNaiCuaPhongBanTheoThoiGian(phongBanXuLyId, fromDate, toDate);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/09/2013
        /// Todo : Lấy tổng của cột
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string GetTotalOfColumn(string columnName, DataSet ds)
        {
            int total = 0;
            List<string> listLoaiKhieuNaiDaTinh = new List<string>();

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    total += ConvertUtility.ToInt32(ds.Tables[0].Rows[i][columnName], 0);
                    listLoaiKhieuNaiDaTinh.Add(ds.Tables[0].Rows[i]["LoaiKhieuNaiId"].ToString());
                }
            }

            if (ds.Tables.Count > 1)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    if (!listLoaiKhieuNaiDaTinh.Contains(ds.Tables[1].Rows[i]["ParentId"]))
                    {
                        total += ConvertUtility.ToInt32(ds.Tables[1].Rows[i][columnName], 0);
                        listLoaiKhieuNaiDaTinh.Add(ds.Tables[1].Rows[i]["LinhVucChungId"].ToString());
                    }
                    else if (!listLoaiKhieuNaiDaTinh.Contains(ds.Tables[1].Rows[i]["LinhVucChungId"].ToString()))
                    {
                        listLoaiKhieuNaiDaTinh.Add(ds.Tables[1].Rows[i]["LinhVucChungId"].ToString());
                    }
                }
            }

            if (ds.Tables.Count > 2)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    if (!listLoaiKhieuNaiDaTinh.Contains(ds.Tables[2].Rows[i]["ParentId"]))
                    {
                        total += ConvertUtility.ToInt32(ds.Tables[2].Rows[i][columnName], 0);
                        //listLoaiKhieuNaiDaTinh.Add(ds.Tables[2].Rows[i]["LinhVucConId"].ToString());
                    }
                }
            }

            return total.ToString() == "0" ? "&nbsp;" : total.ToString();
            ;
        }

        public static string GetPhongBan()
        {
            StringBuilder sbdv = new StringBuilder();
            var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetList();
            sbdv.AppendFormat("<option value='{1}' code='{0}'>{1}</option>", "-1", "Chọn Phòng ban..");
            foreach (var infoPhongBan in lstPhongBan)
            {
                sbdv.AppendFormat("<option value='{1}' code=\"{0}\">{1}</option>", infoPhongBan.Id, infoPhongBan.Name);
            }
            return sbdv.ToString();
        }

        public static string GetLoaiKhieuNai()
        {
            StringBuilder sbdv = new StringBuilder();
            var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
            sbdv.AppendFormat("<option value='{1}' code='{0}'>{1}</option>", "-1", "Chọn Loại khiếu nại..");
            foreach (var infoLoaiKN in lstLoaiKN)
            {
                sbdv.AppendFormat("<option value='{1}' code=\"{0}\">{1}</option>", infoLoaiKN.Id, infoLoaiKN.Name);
            }
            return sbdv.ToString();

        }

        #region Báo cáo tổ XLNV

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/11/2013
        /// Todo : Lấy khối lượng xử lý khiếu nại của từng nhân viên trong phòng
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <returns></returns>
        public DataTable BaoCaoKhoiLuongCongViecToXLNV(int phongBanXuLyId, DateTime fromDate, DateTime toDate, List<string> listLoaiKhieuNaiId, List<string> listLinhVucChungId, List<string> listLinhVucConId)
        {
            ReportImpl reportImpl = new ReportImpl();
            return reportImpl.BaoCaoKhoiLuongCongViecToXLNV(phongBanXuLyId, fromDate, toDate, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 05/08/2014
        /// Todo : Hiển thị danh sách khiếu nại đã đóng của người dùng
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string ListKhieuNaiDaDongTheoNguoiDung(int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().ListKhieuNaiDongByTenTruyCap(phongBanId, tenTruyCap, fromDate, toDate);
            if (listKhieuNaiInfo != null)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='5'>Số lượng : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Ngày tạo khiếu nại</th>");
                sb.Append("<th>Ngày đóng khiếu nại</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                    sb.Append("<tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 25/04/2014
        /// Todo : Hiển thị danh sách khiếu nại các phòng ban đang tiếp nhận xử lý
        /// </summary>
        /// <param name="phongBanXuLyTruocId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoSoLuongKNCacPhongBanDangDaTiepNhan(int phongBanXuLyTruocId, int loaiKhieuNaiId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            DataTable dtResult = new ReportImpl().BaoCaoSoLuongKNCacPhongBanDangDaTiepNhan(phongBanXuLyTruocId, loaiKhieuNaiId, fromDate, toDate);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append("<th width='200px'>Phòng ban</th>");
                sb.Append("<th>Số lượng KN</th>");
                sb.Append("</tr>");

                foreach (DataRow row in dtResult.Rows)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + row["TenPhongBan"].ToString() + "</td>");
                    sb.Append("<td>" + row["SoLuongKN"].ToString() + "</td>");
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/0/2015
        /// Todo : Hiển thị danh sách khiếu nại chưa đóng
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="fromDateNgayTiepNhan"></param>
        /// <param name="toDateNgayTiepNhan"></param>
        /// <returns></returns>
        public string DanhSachKhieuNaiChuaDongTheoPhongBanXuLy(int phongBanXuLyId, int loaiKhieuNaiId, DateTime fromDateNgayTiepNhan, DateTime toDateNgayTiepNhan)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().DanhSachKhieuNaiChuaDong(phongBanXuLyId, loaiKhieuNaiId, fromDateNgayTiepNhan, toDateNgayTiepNhan);
            if (listKhieuNaiInfo != null && listKhieuNaiInfo.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("<th>Ngày tạo khiếu nại</th>");
                sb.Append("<th>Ngày quá hạn PB</th>");
                sb.Append("<th>Ngày quá hạn TT</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                    sb.Append("</tr>");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang hai
        /// Created date : 09/06/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp các phòng ban của trung tâm tính cước
        /// </summary>
        /// <returns></returns>
        public string BaoCaoSoLuongTonDongVaQuaHanCuaCacDoiTac(List<int> listDoiTac, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                DataTable dtReport = new ReportImpl().ListKhieuNaiTonDongVaQuaHanTaiThoiDiemHienTai_Sql(listDoiTac);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    int totalSoLuongQuaHan = 0;
                    int totalSoLuongTonDong = 0;

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Tên đối tác</th>");
                    sb.Append("<th>Số lượng tồn đọng</th>");
                    sb.Append("<th>Số lượng quá hạn</th>");
                    sb.Append("</tr>");
                    for (int i = 0; i < dtReport.Rows.Count; i++)
                    {
                        DataRow row = dtReport.Rows[i];

                        int soLuongTonDong = ConvertUtility.ToInt32(row["SoLuongTonDong"], 0);
                        int soLuongQuaHan = ConvertUtility.ToInt32(row["SoLuongQuaHan"], 0);

                        totalSoLuongTonDong += soLuongTonDong;
                        totalSoLuongQuaHan += soLuongQuaHan;

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenDoiTac"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongtondonghoacquahancuadoitac&doiTacId=" + row["DoiTacId"].ToString() + "&tenDoiTac=" + row["TenDoiTac"].ToString() + "&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTonDong.ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongtondonghoacquahancuadoitac&doiTacId=" + row["DoiTacId"].ToString() + "&tenDoiTac=" + row["TenDoiTac"].ToString() + "&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongQuaHan.ToString(formatNumber) + "</a></td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongTonDong.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongQuaHan.ToString(formatNumber) + "</td>");

                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 10/06/2014
        /// Todo : Lấy danh sách khiếu nại
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="type">
        ///     = 1 : Tồn đọng
        ///     = 2 : Quá hạn phòng ban xử lý
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTonDongHoacQuaHan(int doiTacId, int phongBanId, int type)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                List<KhieuNai_ReportInfo> listResult = new ReportImpl().ListKhieuNaiTonDongHoacQuaHan_Sql(doiTacId, phongBanId, type);
                if (listResult != null && listResult.Count > 0)
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan='6'>Số lượng bản ghi : " + listResult.Count + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã PAKN</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("<th>Nội dung phản ánh</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("</tr>");
                    for (int i = 0; i < listResult.Count; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
                        sb.Append("<td class='borderThin'>" + listResult[0].Id + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listResult[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listResult[i].NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listResult[i].NoiDungPA + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listResult[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='6'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='6'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 05/08/2014
        /// Todo : Báo cáo danh sách khiếu nại đã chuyển các bộ phận khác
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <returns></returns>
        public string ListKhieuNaiDaChuyenDonViKhac(int phongBanId, List<int> listPhongBanTiepNhanId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtResult = new ReportSqlImpl().ListKhieuNaiDaChuyenDonViKhac(phongBanId, listPhongBanTiepNhanId);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();

                sb.AppendFormat("<tr><td colspan='12'>Số lượng : {0}</td></tr>", dtResult.Rows.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Loại khiếu nại</th>");
                sb.Append("<th>Lĩnh vực chung</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Ngày tiếp nhận</th>");
                sb.Append("<th>Độ ưu tiên</th>");
                sb.Append("<th>Người chuyển</th>");
                sb.Append("<th>Ngày chuyển</th>");
                sb.Append("<th>Phòng ban xử lý</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("</tr>");

                int index = 1;
                foreach (DataRow row in dtResult.Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", index);
                    sb.AppendFormat("<td>{0}</td>", row["Id"]);
                    sb.AppendFormat("<td>{0}</td>", row["SoThueBao"]);
                    sb.AppendFormat("<td>{0}</td>", row["LoaiKhieuNai"]);
                    sb.AppendFormat("<td>{0}</td>", row["LinhVucChung"]);
                    sb.AppendFormat("<td>{0}</td>", row["LinhVucCon"]);
                    sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " "));
                    sb.AppendFormat("<td>{0}</td>", row["NguoiXuLyTruoc"]);
                    sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayChuyen"]).ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanXuLyId"])));
                    sb.AppendFormat("<td>{0}</td>", row["NguoiXuLy"]);
                    sb.Append("</tr>");

                    index++;
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Báo cáo TT PTDV

        public string BaoCaoSoLuongPAKNDaXuLyTTPTDV(int trungTamId, int phongBanXuLyId, string listNguoiDung, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                DataTable dtResult = new ReportImpl().GetSoLuongPAKNDaXuLyTTPTDV_Solr(trungTamId, phongBanXuLyId, listNguoiDung, fromDate, toDate);
                int i = 0;
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    int totalSLTiepNhan = 0;
                    int totalSLDaXuLy = 0;
                    int totalSLKNDaDongCuaKNDaXuLy = 0;
                    int totalSLTonDong = 0;

                    foreach (DataRow row in dtResult.Rows)
                    {
                        int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
                        int soLuongDaXuLy = ConvertUtility.ToInt32(row["SLDaXuLy"], 0);
                        //int soLuongKNDaDongCuaKNDaXuLy = ConvertUtility.ToInt32(row["SLKNXuLyDaDong"], 0);
                        int soLuongTonDong = ConvertUtility.ToInt32(row["SLTonDong"], 0);
                        i++;
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenNguoiXuLy"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongpakndaxulyttptdv&phongBanId=" + phongBanXuLyId + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTiepNhan.ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongpakndaxulyttptdv&phongBanId=" + phongBanXuLyId + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongDaXuLy.ToString(formatNumber) + "</a></td>");
                        //sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongpakndaxulyttptdv&phongBanId=" + phongBanXuLyId + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=3','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongKNDaDongCuaKNDaXuLy.ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongpakndaxulyttptdv&phongBanId=" + phongBanXuLyId + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=4','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTonDong.ToString(formatNumber) + "</a></td>");
                        sb.Append("</tr>");

                        totalSLTiepNhan += soLuongTiepNhan;
                        totalSLDaXuLy += soLuongDaXuLy;
                        //totalSLKNDaDongCuaKNDaXuLy += soLuongKNDaDongCuaKNDaXuLy;
                        totalSLTonDong += soLuongTonDong;
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='2' >TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTiepNhan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLDaXuLy.ToString(formatNumber) + "</td>");
                    //sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLKNDaDongCuaKNDaXuLy.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTonDong.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='5'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                            <td colspan='5'>
                                Chưa có dữ liệu báo cáo
                            </td>
                        </tr>";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 27/02/2014
        /// Todo : Hiển thị danh sách chi tiết khiếu nại
        /// </summary>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Danh sách khiếu nại tiếp nhận
        ///     2 : Danh sách khiếu nại đã tham gia xử lý
        ///     3 : Danh sách khiếu nại đã tham gia xử lý mà khiếu nại đã được đóng
        ///     4 : Danh sách khiếu nại tồn đọng
        /// </param>
        /// <returns></returns>
        public string BaoCaoSoLuongPAKNDaXuLyTTPTDV_DanhSachKhieuNai(int phongBanXuLyId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().BaoCaoSoLuongPAKNDaXuLyTTPTDV_DanhSachKhieuNai(phongBanXuLyId, tenTruyCap, fromDate, toDate, reportType);
            switch (reportType)
            {
                case 1:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Người xử lý trước</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNaiId + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("</tr>");
                    }

                    break;
                case 2:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNaiId + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLy + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("</tr>");
                    }
                    break;
                case 3:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Người đóng khiếu nại</th>");
                    sb.Append("<th>Ngày đóng khiếu nại</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNaiId + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLy + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("</tr>");
                    }

                    break;
                case 4:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='4'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='4'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    //sb.Append("<th>Người xử lý trước</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].Id + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        //sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("</tr>");
                    }

                    break;
            }

            return sb.ToString();
        }

        public string BaoCaoSoLuongPAKNDangTonDongTTPTDV(int trungTamId, int phongBanXuLyId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                //DataTable dtResult = new ReportImpl().GetSoLuongPAKNDangTonDongTTPTDV_Solr(trungTamId, phongBanXuLyId, fromDate, toDate);                
                DataTable dtResult = new ReportImpl().GetSoLuongPAKNDangTonDongTTPTDV_Solr(trungTamId, phongBanXuLyId);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    // Xóa toàn bộ các dòng mà cột số lượng tồn đọng = 0
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        if (ConvertUtility.ToInt32(dtResult.Rows[i]["SLTonDong"], 0) == 0)
                        {
                            dtResult.Rows.RemoveAt(i);
                            i--;
                        }
                    }

                    int totalSLTonDong = 0;

                    string curLoaiKhieuNaiId = "";
                    string curLinhVucChungId = "";


                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        int rowSpanLoaiKhieuNai = 0;
                        int rowSpanLinhVucChung = 0;
                        DataRow row = dtResult.Rows[i];

                        // Tính số lượng rowspan của loại khiếu nại và lĩnh vực chung
                        if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
                        {
                            curLoaiKhieuNaiId = row["LoaiKhieuNaiId"].ToString();
                            curLinhVucChungId = row["LinhVucChungId"].ToString();

                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                                if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId)
                                {
                                    rowSpanLoaiKhieuNai++;
                                }
                                else
                                {
                                    break;
                                }
                            } // end  for(int j=i;j<dtResult.Rows.Count;j++) 

                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                                if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
                                {
                                    rowSpanLinhVucChung++;
                                }
                                else
                                {
                                    break;
                                }
                            } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
                        } // end if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
                        else
                        {
                            if (row["LinhVucChungId"].ToString() != curLinhVucChungId)
                            {
                                curLinhVucChungId = row["LinhVucChungId"].ToString();
                                for (int j = i; j < dtResult.Rows.Count; j++)
                                {
                                    DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                                    if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
                                    {
                                        rowSpanLinhVucChung++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
                            }
                        } // end else

                        int soLuongTonDong = ConvertUtility.ToInt32(row["SLTonDong"], 0);

                        string sRowSpanLoaiKhieuNai = rowSpanLoaiKhieuNai > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLoaiKhieuNai.ToString() + "\"" : "";
                        string sRowSpanLinhVucChung = rowSpanLinhVucChung > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLinhVucChung.ToString() + "\"" : "";

                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");

                        if (sRowSpanLoaiKhieuNai.Length > 0)
                        {
                            sb.Append("<td class='borderThin'" + sRowSpanLoaiKhieuNai + ">" + row["LoaiKhieuNai"].ToString() + "</td>");
                        }

                        if (sRowSpanLinhVucChung.Length > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'" + sRowSpanLinhVucChung + ">" + row["LinhVucChung"].ToString() + "</td>");
                        }

                        sb.Append("<td align='center' class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongTonDong.ToString(formatNumber) + "</td>");
                        sb.Append("</tr>");

                        totalSLTonDong += soLuongTonDong;
                    } // end 

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='4' >TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTonDong.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                } // end if (dtResult != null && dtResult.Rows.Count > 1)
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='5'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                            <td colspan='5'>
                                Chưa có dữ liệu báo cáo
                            </td>
                        </tr>";
            }
        }

        public string BaoCaoSoLuongPAKNDaTiepNhanTTPTDV(int trungTamId, int phongBanXuLyId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                DataTable dtResult = new ReportImpl().GetSoLuongPAKNDaTiepNhanTTPTDV_Solr(trungTamId, phongBanXuLyId, fromDate, toDate);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    string curLoaiKhieuNaiId = "";
                    string curLinhVucChungId = "";

                    int totalSLTiepNhan = 0;
                    int totalSLDaXuLy = 0;

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        int rowSpanLoaiKhieuNai = 0;
                        int rowSpanLinhVucChung = 0;
                        DataRow row = dtResult.Rows[i];

                        // Tính số lượng rowspan của loại khiếu nại và lĩnh vực chung
                        if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
                        {
                            curLoaiKhieuNaiId = row["LoaiKhieuNaiId"].ToString();
                            curLinhVucChungId = row["LinhVucChungId"].ToString();

                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                                if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId)
                                {
                                    rowSpanLoaiKhieuNai++;
                                }
                                else
                                {
                                    break;
                                }
                            } // end  for(int j=i;j<dtResult.Rows.Count;j++) 

                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                                if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
                                {
                                    rowSpanLinhVucChung++;
                                }
                                else
                                {
                                    break;
                                }
                            } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
                        } // end if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
                        else
                        {
                            if (row["LinhVucChungId"].ToString() != curLinhVucChungId)
                            {
                                curLinhVucChungId = row["LinhVucChungId"].ToString();
                                for (int j = i; j < dtResult.Rows.Count; j++)
                                {
                                    DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                                    if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
                                    {
                                        rowSpanLinhVucChung++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
                            }
                        } // end else

                        int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
                        int soLuongDaXuLy = ConvertUtility.ToInt32(row["SLDaXuLy"], 0);

                        string sRowSpanLoaiKhieuNai = rowSpanLoaiKhieuNai > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLoaiKhieuNai.ToString() + "\"" : "";
                        string sRowSpanLinhVucChung = rowSpanLinhVucChung > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLinhVucChung.ToString() + "\"" : "";

                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");

                        if (sRowSpanLoaiKhieuNai.Length > 0)
                        {
                            sb.Append("<td class='borderThin'" + sRowSpanLoaiKhieuNai + ">" + row["LoaiKhieuNai"].ToString() + "</td>");
                        }

                        if (sRowSpanLinhVucChung.Length > 0)
                        {
                            sb.Append("<td align='center' class='borderThin'" + sRowSpanLinhVucChung + ">" + row["LinhVucChung"].ToString() + "</td>");
                        }

                        sb.Append("<td align='center' class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongDaXuLy.ToString(formatNumber) + "</td>");
                        sb.Append("</tr>");

                        totalSLTiepNhan += soLuongTiepNhan;
                        totalSLDaXuLy += soLuongDaXuLy;
                    } // end 

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='4'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTiepNhan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLDaXuLy.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='6'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                            <td colspan='6'>
                                Chưa có dữ liệu báo cáo
                            </td>
                        </tr>";
            }
        }

        public string BaoCaoChiTietPAKNDaTiepNhanTTPTDV(int trungTamId, int phongBanXuLyId, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
        {
            int index = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                DataTable dtKhieuNai = new ReportImpl().GetChiTietPAKNDaTiepNhanTTPTDV_Solr(trungTamId, phongBanXuLyId, fromDate, toDate, pageIndex, pageSize);
                if (dtKhieuNai != null && dtKhieuNai.Rows.Count > 0)
                {
                    for (int i = 0; i < dtKhieuNai.Rows.Count; i++)
                    {
                        DataRow row = dtKhieuNai.Rows[i];

                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TrangThai"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + GetDataImpl.GetMaTuDong("PA", row["KhieuNaiId"].ToString(), 10) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), ConvertUtility.ToInt32(row["DoUuTien"], 1)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["SoThueBao"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["LoaiKhieuNai"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                        //sb.Append("<td align='center' class='borderThin'>" + row["NoiDungPA"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["NguoiTiepNhan"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["NguoiXuLy"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["NguoiTienXuLy"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["NgayTiepNhan"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["NgayQuaHan"].ToString() + "</td>");
                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='13'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                            <td colspan='13'>
                                Chưa có dữ liệu báo cáo
                            </td>
                        </tr>";
            }
        }




        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 19/09/2014
        /// Todo : Hiển thị nội dung báo cáo dịch vụ toàn mạng
        /// </summary>
        /// <param name="trungTamId"></param>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoKhieuNaiDichVuToanMang(int khuVucId, List<int> listLoaiKhieuNaiId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().GetSoLuongKhieuNaiDichVuToanMang_Solr(khuVucId, listLoaiKhieuNaiId, fromDate, toDate);

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string curLoaiKhieuNaiId = "";
                string curLinhVucChungId = "";

                int totalSLTiepNhan = 0;
                int totalSLDaDong = 0;

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='4'>TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>TONG_SO_TIEP_NHAN</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>TONG_SO_DA_DONG</td>");
                sb.Append("</tr>");

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    int rowSpanLoaiKhieuNai = 0;
                    int rowSpanLinhVucChung = 0;
                    DataRow row = dtResult.Rows[i];

                    // Tính số lượng rowspan của loại khiếu nại và lĩnh vực chung
                    if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
                    {
                        curLoaiKhieuNaiId = row["LoaiKhieuNaiId"].ToString();
                        curLinhVucChungId = row["LinhVucChungId"].ToString();

                        for (int j = i; j < dtResult.Rows.Count; j++)
                        {
                            DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                            if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId)
                            {
                                rowSpanLoaiKhieuNai++;
                            }
                            else
                            {
                                break;
                            }
                        } // end  for(int j=i;j<dtResult.Rows.Count;j++) 

                        for (int j = i; j < dtResult.Rows.Count; j++)
                        {
                            DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                            if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
                            {
                                rowSpanLinhVucChung++;
                            }
                            else
                            {
                                break;
                            }
                        } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
                    } // end if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
                    else
                    {
                        if (row["LinhVucChungId"].ToString() != curLinhVucChungId)
                        {
                            curLinhVucChungId = row["LinhVucChungId"].ToString();
                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowLoaiKhieuNai = dtResult.Rows[j];
                                if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
                                {
                                    rowSpanLinhVucChung++;
                                }
                                else
                                {
                                    break;
                                }
                            } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
                        }
                    } // end else

                    int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
                    int soLuongDaDong = ConvertUtility.ToInt32(row["SLDaDong"], 0);

                    string sRowSpanLoaiKhieuNai = rowSpanLoaiKhieuNai > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLoaiKhieuNai.ToString() + "\"" : "";
                    string sRowSpanLinhVucChung = rowSpanLinhVucChung > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLinhVucChung.ToString() + "\"" : "";

                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");

                    if (sRowSpanLoaiKhieuNai.Length > 0)
                    {
                        sb.Append("<td class='borderThin'" + sRowSpanLoaiKhieuNai + ">" + row["LoaiKhieuNai"].ToString() + "</td>");
                    }

                    if (sRowSpanLinhVucChung.Length > 0)
                    {
                        sb.Append("<td align='center' class='borderThin'" + sRowSpanLinhVucChung + ">" + row["LinhVucChung"].ToString() + "</td>");
                    }

                    sb.Append("<td align='center' class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongdichvutoanmang&khuVucId=" + khuVucId + "&loaiKhieuNaiId=" + row["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + row["LinhVucChungId"].ToString() + "&LinhVucConId=" + row["LinhVucConId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTiepNhan.ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongdichVutoanmang&khuVucId=" + khuVucId + "&loaiKhieuNaiId=" + row["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + row["LinhVucChungId"].ToString() + "&LinhVucConId=" + row["LinhVucConId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongDaDong.ToString(formatNumber) + "</a></td>");
                    sb.Append("</tr>");

                    totalSLTiepNhan += soLuongTiepNhan;
                    totalSLDaDong += soLuongDaDong;
                } // end 

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='4'>TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTiepNhan.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLDaDong.ToString(formatNumber) + "</td>");
                sb.Append("</tr>");

                sb.Replace("TONG_SO_TIEP_NHAN", totalSLTiepNhan.ToString(formatNumber));
                sb.Replace("TONG_SO_DA_DONG", totalSLDaDong.ToString(formatNumber));
            }
            else
            {
                sb.Append(@"<tr>
                                <td colspan='6'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        #endregion

        #region Báo cáo tổ trưởng tổ KTV

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 05/12/2013
        /// Todo : Lấy số lượng khiếu nại phản hồi về khai thác viên
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string GetSoLuongKNPhanHoiVeKTVCuaTTToKTV(int phongBanXuLyId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            sb.Append("<th>STT</th>");
            sb.Append("<th>Họ tên</th>");
            sb.Append("<th>Số lượng KN phản hồi</th>");

            try
            {
                DataTable dtResult = new ReportImpl().GetSoLuongKNPhanHoiVeKTVCuaTTToKTV_Solr(phongBanXuLyId, fromDate, toDate);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    int i = 1;
                    int totalSLKNPhanHoi = 0;

                    foreach (DataRow row in dtResult.Rows)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenNguoiSuDung"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["SLKNPhanHoi"].ToString() + "</td>");
                        sb.Append("</tr>");

                        i++;
                        totalSLKNPhanHoi += ConvertUtility.ToInt32(row["SLKNPhanHoi"], 0);
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLKNPhanHoi.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr colspan='3'><td>Không có dữ liệu báo cáo</td></tr>");
                }
            }
            catch
            {
                sb.Append("<tr colspan='3'><td>Có lỗi trong quá trình lấy dữ liệu báo cáo</td></tr>");
            }


            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 05/12/2013
        /// Todo : Lấy số lượng khiếu nại quá hạn hoặc tồn đọng của khai thác viên
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string GetSoLuongKNQuaHanVaTonDongCuaKTV(int phongBanXuLyId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            sb.Append("<th>STT</th>");
            sb.Append("<th>Họ tên</th>");
            sb.Append("<th>Số lượng KN tồn đọng</th>");
            sb.Append("<th>Số lượng KN quá hạn</th>");

            try
            {
                DataTable dtResult = new ReportImpl().GetSoLuongKNQuaHanVaTonDongCuaKTV_Solr(phongBanXuLyId, fromDate, toDate);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    int i = 1;
                    int totalSLTonDong = 0;
                    int totalSLQuaHan = 0;

                    foreach (DataRow row in dtResult.Rows)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenNguoiSuDung"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["SLKNTonDong"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["SLKNQuaHan"].ToString() + "</td>");
                        sb.Append("</tr>");

                        i++;

                        totalSLTonDong += ConvertUtility.ToInt32(row["SLKNTonDong"], 0);
                        totalSLQuaHan += ConvertUtility.ToInt32(row["SLKNQuaHan"], 0);
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThin' colspan='2'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThin'>" + totalSLTonDong.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + totalSLQuaHan.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr colspan='4'><td>Không có dữ liệu báo cáo</td></tr>");
                }
            }
            catch
            {
                sb.Append("<tr colspan='4'><td>Có lỗi trong quá trình lấy dữ liệu báo cáo</td></tr>");
            }


            return sb.ToString();
        }

        #endregion

        #region Báo cáo VNPT TT

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/12/2013
        /// Todo : Lấy ra số lượng khiếu nại tiếp nhận, xử lý, đã đóng, quá hạn của GDV trong khoảng thời gian được chọn để lấy báo cáo
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string GetSoLuongKhieuNaiTiepNhanXuLyCuaGDV(int phongBanXuLyId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            sb.Append("<th>STT</th>");
            sb.Append("<th>Họ tên</th>");
            sb.Append("<th>Số lượng tiếp nhận</th>");
            sb.Append("<th>Số lượng đã xử lý</th>");
            sb.Append("<th>Số lượng tồn đọng</th>");
            sb.Append("<th>Số lượng quá hạn</th>");

            try
            {
                DataTable dtResult = new ReportImpl().GetSoLuongKhieuNaiTiepNhanXuLyCuaVNPTTT_Solr(phongBanXuLyId, fromDate, toDate);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    int i = 1;
                    int totalSLTiepNhan = 0;
                    int totalSLDaXuLy = 0;
                    int totalSLTonDong = 0;
                    int totalSLQuaHan = 0;

                    foreach (DataRow row in dtResult.Rows)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenNguoiSuDung"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["SLTiepNhan"].ToString() != "0" ? row["SLTiepNhan"].ToString() : string.Empty) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["SLDaXuLy"].ToString() != "0" ? row["SLDaXuLy"].ToString() : string.Empty) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["SLTonDong"].ToString() != "0" ? row["SLTonDong"].ToString() : string.Empty) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["SLQuaHan"].ToString() != "0" ? row["SLQuaHan"].ToString() : string.Empty) + "</td>");
                        sb.Append("</tr>");

                        i++;

                        totalSLTiepNhan += ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
                        totalSLDaXuLy += ConvertUtility.ToInt32(row["SLDaXuLy"], 0);
                        totalSLTonDong += ConvertUtility.ToInt32(row["SLTonDong"], 0);
                        totalSLQuaHan += ConvertUtility.ToInt32(row["SLQuaHan"], 0);
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='2' >TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTiepNhan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLDaXuLy.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTonDong.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLQuaHan.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr colspan='6'><td>Không có dữ liệu báo cáo</td></tr>");
                }
            }
            catch
            {
                sb.Append("<tr colspan='6'><td>Có lỗi trong quá trình lấy dữ liệu báo cáo</td></tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 27/12/2013
        /// Todo : Lấy dữ liệu cho báo cáo giảm trừ do khiếu nại dịch vụ của VNPTT TT
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="layDuLieuTheo1HoacNhieuPhongBan">
        ///     = 1 : Lấy theo PhongBanId
        ///     = 2 : Lấy theo PhongBanId và các phòng ban trực thuộc PhongBanId
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isDongKN">
        ///     = true : Chỉ lấy ra danh sách khiếu nại đã đóng
        ///     = false : Lấy ra danh sách khiếu nại đã đóng trong khoảng thời gian fromDate, toDate và cả những khiếu nại chưa đóng
        /// </param>
        /// <returns></returns>
        public string BaoCaoGiamTruDoKhieuNaiDichVuVNPTTT(int doiTacId, int phongBanId, int layDuLieuTheo1HoacNhieuPhongBan, DateTime fromDate, DateTime toDate, bool isDongKN, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            List<KhieuNai_ReportInfo> listKhieuNai = new ReportImpl().BaoCaoGiamTruDoKhieuNaiDichVuVNPTTT_Solr(doiTacId, phongBanId, layDuLieuTheo1HoacNhieuPhongBan, fromDate, toDate, isDongKN);
            if (listKhieuNai != null && listKhieuNai.Count > 0)
            {
                decimal total = 0;
                int index = 1;
                for (int i = 0; i < listKhieuNai.Count; i++)
                {
                    decimal totalTienGiamTru = listKhieuNai[i].SoTienKhauTru_TKC + listKhieuNai[i].SoTienKhauTru_KM + listKhieuNai[i].SoTienKhauTru_KM1
                                                + listKhieuNai[i].SoTienKhauTru_KM2 + listKhieuNai[i].SoTienKhauTru_Khac
                                                + listKhieuNai[i].SoTienKhauTru_TS_GPRS + listKhieuNai[i].SoTienKhauTru_TS_CP + listKhieuNai[i].SoTienKhauTru_TS_Thoai
                                                + listKhieuNai[i].SoTienKhauTru_TS_SMS + listKhieuNai[i].SoTienKhauTru_TS_IR + listKhieuNai[i].SoTienKhauTru_TS_Khac;

                    if (totalTienGiamTru > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].Id + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].SoThueBao + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].LinhVucCon + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].NoiDungPA + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNai[i].NgayDongKN.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNai[i].KQXuLy_SHCV + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + totalTienGiamTru.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (byte)listKhieuNai[i].TrangThai).Replace("_", " ") + "</td>");
                        sb.Append("</tr>");

                        total += totalTienGiamTru;
                        index++;
                    }
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='8' >TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + total.ToString(formatNumber) + "</td>");
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr><td colspan='7'>Không có dữ liệu</td></tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="layDuLieuTheo1HoacNhieuPhongBan">
        ///     = 1 : Lấy theo PhongBanId
        ///     = 2 : Lấy theo PhongBanId và các phòng ban trực thuộc PhongBanId
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isDongKN">
        ///     = true : Chỉ lấy ra danh sách khiếu nại đã đóng
        ///     = false : Lấy ra danh sách khiếu nại đã đóng trong khoảng thời gian fromDate, toDate và cả những khiếu nại chưa đóng
        /// </param>
        /// <returns></returns>
        public string BaoCaoKhieuNaiDichVuVNPTTT(int doiTacId, int phongBanId, int layDuLieuTheo1HoacNhieuPhongBan, DateTime fromDate, DateTime toDate, bool isDongKN, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            List<KhieuNai_ReportInfo> listKhieuNai = new ReportImpl().BaoCaoKhieuNaiDichVuVNPTTT_Solr(doiTacId, phongBanId, layDuLieuTheo1HoacNhieuPhongBan, fromDate, toDate, isDongKN);
            if (listKhieuNai != null && listKhieuNai.Count > 0)
            {
                decimal total = 0;

                for (int i = 0; i < listKhieuNai.Count; i++)
                {
                    DateTime ngayBaoNhan = listKhieuNai[i].NgayTiepNhan.AddDays(2);
                    decimal totalTienGiamTru = listKhieuNai[i].SoTienKhauTru_TKC + listKhieuNai[i].SoTienKhauTru_KM
                                                + listKhieuNai[i].SoTienKhauTru_KM1 + listKhieuNai[i].SoTienKhauTru_KM2
                                                + listKhieuNai[i].SoTienKhauTru_Khac
                                                + listKhieuNai[i].SoTienKhauTru_TS_GPRS + listKhieuNai[i].SoTienKhauTru_TS_CP + listKhieuNai[i].SoTienKhauTru_TS_Thoai
                                                + listKhieuNai[i].SoTienKhauTru_TS_SMS + listKhieuNai[i].SoTienKhauTru_TS_IR + listKhieuNai[i].SoTienKhauTru_TS_Khac;
                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThin' >" + (i + 1).ToString() + "</td>");
                    //sb.Append("<td class='borderThin'>" + listKhieuNai[i].Id + "</td>");
                    sb.Append("<td class='borderThin'>" + listKhieuNai[i].SoThueBao + "</td>");
                    sb.Append("<td class='borderThin'>" + listKhieuNai[i].HoTenLienHe + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNai[i].NgayTiepNhan.ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + ngayBaoNhan.ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='borderThin'>" + listKhieuNai[i].NoiDungXuLy + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNai[i].NgayDongKN.ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNai[i].KQXuLy_SHCV + "</td>");
                    //sb.Append("<td align='center' class='borderThin'>" + (listKhieuNai[i].IsCCT ? "CCT" : "") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (totalTienGiamTru <= 0 ? "CCT" : "") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + totalTienGiamTru.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (byte)listKhieuNai[i].TrangThai).Replace("_", " ") + "</td>");
                    sb.Append("</tr>");

                    total += totalTienGiamTru;
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='10' >TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + total.ToString(formatNumber) + "</td>");
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr><td colspan='10'>Không có dữ liệu</td></tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopGQKNVNPTTT(int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate, List<string> listLoaiKhieuNaiId, List<string> listLinhVucChungId, List<string> listLinhVucConId, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataSet dsReport = new ReportImpl().BaoCaoTongHopGQKNVNPTTT_Solr(doiTacId, phongBanId, fromDate, toDate, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);
            DataSet dsToCalculateTotal = dsReport.Copy();

            int index = 1;
            if (dsReport != null && dsReport.Tables.Count > 2)
            {
                if (dsReport != null && dsReport.Tables.Count > 0)
                {
                    if (dsReport.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow rowLoaiKhieuNai in dsReport.Tables[0].Rows)
                        {
                            int curIndex = index;
                            int luyKeKNDaGQDenDauTuan = ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuan"], 0);
                            int luyKeKNTonDongDauTuan = ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTonDongDauTuan"], 0);
                            int soLuongTiepNhanTrongTuan = ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0);
                            int soLuongDaGQTrongTuan = ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongDaGiaiQuyetTrongTuan"], 0);
                            int soLuongTonDongTrongTuanDoQuaHan = ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTonDongTrongTuanDoQuaHan"], 0);
                            int luyKeKNDaGQDenCuoiTuan = ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenCuoiTuan"], 0);
                            int luyKeTonDongDoQuaHanCuoiTuan = ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTongDongDoQuaHan"], 0);

                            sb.Append("<tr>");
                            sb.Append("<td class='borderThin' align='center' valign='top' rowspan='rowspanX'>" + index.ToString() + "</td>");
                            sb.Append("<td class='borderThin'>" + rowLoaiKhieuNai["LoaiKhieuNai"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenDauTuan.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + luyKeKNTonDongDauTuan.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhanTrongTuan.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuongDaGQTrongTuan.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuongTonDongTrongTuanDoQuaHan.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenCuoiTuan.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + luyKeTonDongDoQuaHanCuoiTuan.ToString(formatNumber) + "</td>");
                            sb.Append("</tr>");

                            //index++;


                            // Lĩnh vực chung
                            if (dsReport.Tables.Count > 1 && dsReport.Tables[1].Rows.Count > 0)
                            {
                                StringBuilder sbLinhVucChung = BaoCaoTongHopGQKNVNPTTT_DisplayLinhVucChung(ref index, dsReport.Tables[1], dsReport.Tables[2],
                                                                                                                rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString(), formatNumber);
                                sb.Append(sbLinhVucChung.ToString());
                            } // end if (dsReport.Tables[1].Rows.Count > 0)

                            int totalRow = index - curIndex + 1;
                            sb.Replace("rowspanX", totalRow.ToString());
                            index = curIndex + 1;
                        }
                    }

                    if (dsReport.Tables[1].Rows.Count > 0)
                    {
                        StringBuilder sbLinhVucChung = BaoCaoTongHopGQKNVNPTTT_DisplayLinhVucChung(ref index, dsReport.Tables[1], dsReport.Tables[2], "", formatNumber);
                        if (sbLinhVucChung != null)
                        {
                            sb.Append(sbLinhVucChung.ToString());
                        }
                    }

                    if (dsReport.Tables[2].Rows.Count > 0)
                    {
                        StringBuilder sbLinhVucCon = BaoCaoTongHopGQKNVNPTTT_DisplayLinhVucCon(ref index, dsReport.Tables[2], "", formatNumber);
                        if (sbLinhVucCon != null)
                        {
                            sb.Append(sbLinhVucCon);
                        }
                    }

                    // Hiển thị giá trị tổng
                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
                    sb.Append("<td class='borderThinTextBold'>TỔNG</td>");

                    sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("LuyKeKNDaGiaiQuyetDenDauTuan", dsToCalculateTotal) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("LuyKeKNTonDongDauTuan", dsToCalculateTotal) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("SoLuongTiepNhanTrongTuan", dsToCalculateTotal) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("SoLuongDaGiaiQuyetTrongTuan", dsToCalculateTotal) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("SoLuongTonDongTrongTuanDoQuaHan", dsToCalculateTotal) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("LuyKeKNDaGiaiQuyetDenCuoiTuan", dsToCalculateTotal) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + GetTotalOfColumn("LuyKeKNTongDongDoQuaHan", dsToCalculateTotal) + "</td>");
                    sb.Append("</tr>");
                }// end if (dsReport != null && dsReport.Tables.Count > 2)                    
            }
            else
            {
                sb.Append(@"<tr>
                    <td colspan='9'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
            }
            return sb.ToString();
        }

        public static string BaoCaoTongHopGiamTru(int khuVucID, int phongBanXuLyId, int layDuLieuTheo1HoacNhieuPhongBan, int fromDate, int toDate, int loaiKhieuNaiID, int linhVucChungID, int linhVucConID, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                //List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoTongHopGiamTruCuocDV(khuVucID, donViID, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoTongHopGiamTruCuocDV_Solr(khuVucID, phongBanXuLyId, layDuLieuTheo1HoacNhieuPhongBan, fromDate, toDate, loaiKhieuNaiID, linhVucChungID, linhVucConID);
                int i = 0;
                if (list != null && list.Count > 0)
                {
                    decimal totalSoTienGiamTru = 0;
                    int totalSoLuongGiamTru = 0;

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        i = i + 1;
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td align='left' class='borderThin'>" + info.DauSo + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoLuongGiamTru + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTien.ToString(formatNumber) + "</td>");
                        sb.Append("<td class='borderThin'>" + info.GhiChu + "</td>");
                        sb.Append("</tr>");

                        totalSoTienGiamTru += info.SoTien;
                        totalSoLuongGiamTru += info.SoLuongGiamTru;
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongGiamTru.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru.ToString(formatNumber) + "</td>");
                    sb.Append("<td class='borderThinTextBold'>&nbsp;</td>");
                }
                else
                {
                    sb.Append(@"<tr>
                    <td colspan='5'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                    <td colspan='10'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/07/2014
        /// Todo : Báo cáo tổng hợp giảm trừ của VNPTTT
        /// </summary>
        /// <param name="listDoiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopGiamTruVNPTTT(List<int> listDoiTacId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().BaoCaoTongHopGiamTruVNPTTT_Solr(listDoiTacId, fromDate, toDate);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                decimal totalSoTienGiamTru = 0;
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Tỉnh/Thành phố</th>");
                sb.Append("<th>TỔNG SỐ TIỀN GIẢM TRỪ <br/>(Đồng- chưa VAT)</th>");
                sb.Append("<th>Ghi chú</th>");
                sb.Append("</tr>");

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow row = dtResult.Rows[i];
                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");
                    sb.Append("<td align='left' class='borderThin'>" + row["TenDoiTac"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + ConvertUtility.ToDecimal(row["SoTienGiamTru"]).ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("</tr>");

                    totalSoTienGiamTru += ConvertUtility.ToDecimal(row["SoTienGiamTru"]);
                }

                sb.Append("<tr style='font-size:200%'>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>&nbsp;</td>");
            }
            return sb.ToString();
        }


        public string BCTH_GiamTru_DVGTGTDV(int doiTacId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().BCTH_GiamTru_DVGTGTDV_Solr(doiTacId, fromDate, toDate);
            dtResult = SortRow(dtResult);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Khu vực</th>");
                sb.Append("<th>Đối tác cấp I</th>");
                sb.Append("<th>Đối tác cấp II</th>");
                sb.Append("<th>Đối tác cấp III</th>");
                sb.Append("<th>TỔNG SỐ TIỀN GIẢM TRỪ <br/>(Đồng- chưa VAT)</th>");
                sb.Append("<th>Ghi chú</th>");
                //sb.Append("<th>Ghi chú</th>");
                sb.Append("</tr>");
                if (dtResult.Rows.Count > 0)
                {
                    double totalCol1 = 0;


                    int rowspanLevel1 = dtResult.Rows.Count;
                    bool isRowspanLevel1 = false;

                    int numRowLevel2 = 0;
                    int numRowLevel3 = 0;
                    int numRowLevel4 = 0;

                    int indexRowLevel2 = 1;
                    int indexRowLevel3 = 1;
                    int indexRowLevel4 = 1;

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        DataRow row = dtResult.Rows[i];

                        if (numRowLevel2 != 0)
                        {
                            if (numRowLevel2 != indexRowLevel2)
                            {
                                indexRowLevel2++;
                            }
                            else
                            {
                                numRowLevel2 = 0;
                                indexRowLevel2 = 1;
                            }
                        }

                        if (numRowLevel3 != 0)
                        {
                            if (numRowLevel3 != indexRowLevel3)
                            {
                                indexRowLevel3++;
                            }
                            else
                            {
                                numRowLevel3 = 0;
                                indexRowLevel3 = 1;
                            }
                        }

                        if (numRowLevel4 != 0)
                        {
                            if (numRowLevel4 != indexRowLevel4)
                            {
                                indexRowLevel4++;
                            }
                            else
                            {
                                numRowLevel4 = 0;
                                indexRowLevel4 = 1;
                            }
                        }

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");

                        if (row["Level"].ToString() == "1")
                        {
                            if (!isRowspanLevel1)
                            {
                                sb.Append("<td valign='top' class='borderThin' rowspan='" + rowspanLevel1.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                                isRowspanLevel1 = true;
                            }

                            sb.Append("<td valign='top' class='borderThin' ></td>");
                            sb.Append("<td valign='top' class='borderThin' ></td>");
                            sb.Append("<td valign='top' class='borderThin' ></td>");
                        }

                        if (row["Level"].ToString() == "2")
                        {
                            if (numRowLevel2 == 0)
                            {
                                int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                                List<int> listDoiTacId = new List<int>();
                                listDoiTacId.Add(rowDoiTacId);

                                for (int j = i; j < dtResult.Rows.Count; j++)
                                {
                                    DataRow rowCheck = dtResult.Rows[j];
                                    if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        numRowLevel2++;

                                        if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                        {
                                            listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                        }
                                    }
                                }

                                sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel2.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                            }

                            sb.Append("<td class='borderThin'></td>");
                            sb.Append("<td class='borderThin'></td>");
                        }

                        if (row["Level"].ToString() == "3")
                        {
                            if (numRowLevel3 == 0)
                            {
                                int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                                List<int> listDoiTacId = new List<int>();
                                listDoiTacId.Add(rowDoiTacId);

                                for (int j = i; j < dtResult.Rows.Count; j++)
                                {
                                    DataRow rowCheck = dtResult.Rows[j];
                                    if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        numRowLevel3++;

                                        if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                        {
                                            listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                        }
                                    }
                                }

                                sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel3.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                            }

                            sb.Append("<td class='borderThin'></td>");
                        }

                        if (row["Level"].ToString() == "4")
                        {
                            if (numRowLevel4 == 0)
                            {
                                int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                                List<int> listDoiTacId = new List<int>();
                                listDoiTacId.Add(rowDoiTacId);

                                for (int j = i; j < dtResult.Rows.Count; j++)
                                {
                                    DataRow rowCheck = dtResult.Rows[j];
                                    if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        numRowLevel4++;

                                        if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                        {
                                            listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                        }
                                    }
                                }

                                sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel4.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                            }
                            //sb.Append("<td class='borderThin'></td>"); 
                        }


                        sb.Append("<td align='center' class='borderThin'>" + ConvertUtility.ToInt32(row["SoTienGiamTru"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td class='borderThin'></td>");
                        sb.Append("</tr>");

                        totalCol1 += ConvertUtility.ToInt32(row["SoTienGiamTru"], 0);

                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='5' >TỔNG</td>");
                    sb.Append("<td class='borderThin'>" + totalCol1.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr><td colspan='17'>Không có dữ liệu báo cáo</td></tr>");
                }

            }
            return sb.ToString();
        }

        public string BCTH_GiamTru_DVGTGT_DIVU(DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().BCTH_GiamTru_DVGTGT_DiVu_Solr(fromDate, toDate);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                decimal totalSoTienGiamTru = 0;
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Dịch Vụ</th>");
                sb.Append("<th>TỔNG SỐ TIỀN GIẢM TRỪ <br/>(Đồng- chưa VAT)</th>");
                sb.Append("<th>Ghi chú</th>");
                sb.Append("</tr>");
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow row = dtResult.Rows[i];
                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");
                    sb.Append("<td align='left' class='borderThin'>" + row["TenLinhVucCon"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + ConvertUtility.ToDecimal(row["SoTienGiamTru"]).ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("</tr>");

                    totalSoTienGiamTru += ConvertUtility.ToDecimal(row["SoTienGiamTru"]);
                }

                sb.Append("<tr style='font-size:200%'>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>&nbsp;</td>");
            }
            return sb.ToString();

        }
        #endregion

        #region Báo cáo VNP

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 29/05/2014
        /// Todo : Báo cáo tổng hợp giảm trừ theo CP (dành cho VNP - có thể xem được của tất cả các đối tác)
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static string BaoCaoTongHopGiamTru(int doiTacId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string formatNumber = string.Empty;
                if (!isExportExcel)
                {
                    formatNumber = FORMAT_NUMBER;
                }

                List<KhieuNai_ReportInfo> list = new ReportImpl().BaoCaoTongHopGiamTruCuocDV_Solr(doiTacId, fromDate, toDate);
                int i = 0;
                if (list != null && list.Count > 0)
                {
                    decimal totalSoTienGiamTru = 0;
                    int totalSoLuongGiamTru = 0;

                    foreach (KhieuNai_ReportInfo info in list)
                    {
                        i = i + 1;
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
                        sb.Append("<td align='left' class='borderThin'>" + info.DauSo + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoLuongGiamTru + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + info.SoTien.ToString(formatNumber) + "</td>");
                        sb.Append("<td class='borderThin'>" + info.GhiChu + "</td>");
                        sb.Append("</tr>");

                        totalSoTienGiamTru += info.SoTien;
                        totalSoLuongGiamTru += info.SoLuongGiamTru;
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongGiamTru.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru.ToString(formatNumber) + "</td>");
                    sb.Append("<td class='borderThinTextBold'>&nbsp;</td>");
                }
                else
                {
                    sb.Append(@"<tr>
                    <td colspan='5'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return @"<tr>
                    <td colspan='10'>
                        Chưa có dữ liệu báo cáo
                    </td>
                </tr>";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 22/01/2014
        /// Todo : Lấy báo cáo tổng hợp số lượng khiếu nại theo khu vực
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="doiTacid"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopKhieuNaiVNP(int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().BaoCaoTongHopKhieuNaiVNP_Solr(doiTacId, phongBanId, fromDate, toDate);
            dtResult = SortRow(dtResult);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    int totalCol1 = 0;
                    int totalCol12 = 0;
                    int totalCol21 = 0;
                    int totalCol22 = 0;
                    int totalCol23 = 0;
                    int totalCol31 = 0;
                    int totalCol32 = 0;
                    int totalCol41 = 0;
                    int totalCol42 = 0;
                    int totalCol43 = 0;
                    int totalCol44 = 0;
                    int totalCol5 = 0;
                    int totalCol6 = 0;
                    int totalCol7 = 0;

                    int rowspanLevel1 = dtResult.Rows.Count;
                    bool isRowspanLevel1 = false;

                    int rowspanLevel2 = 1;
                    bool isRowspanLevel2 = false;
                    int numRowLevel2 = 0;

                    int numRowLevel3 = 0;

                    int indexRowLevel2 = 1;
                    int indexRowLevel3 = 1;

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        DataRow row = dtResult.Rows[i];

                        if (numRowLevel2 != 0)
                        {
                            if (numRowLevel2 != indexRowLevel2)
                            {
                                indexRowLevel2++;
                            }
                            else
                            {
                                numRowLevel2 = 0;
                                indexRowLevel2 = 1;
                            }
                        }

                        if (numRowLevel3 != 0)
                        {
                            if (numRowLevel3 != indexRowLevel3)
                            {
                                indexRowLevel3++;
                            }
                            else
                            {
                                numRowLevel3 = 0;
                                indexRowLevel3 = 1;
                            }
                        }

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");

                        if (row["Level"].ToString() == "1")
                        {
                            if (!isRowspanLevel1)
                            {
                                sb.Append("<td valign='top' class='borderThin' rowspan='" + rowspanLevel1.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                                isRowspanLevel1 = true;
                            }

                            sb.Append("<td valign='top' class='borderThin' ></td>");
                            sb.Append("<td valign='top' class='borderThin' ></td>");
                        }

                        if (row["Level"].ToString() == "2")
                        {
                            if (numRowLevel2 == 0)
                            {
                                int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                                List<int> listDoiTacId = new List<int>();
                                listDoiTacId.Add(rowDoiTacId);

                                for (int j = i; j < dtResult.Rows.Count; j++)
                                {
                                    DataRow rowCheck = dtResult.Rows[j];
                                    if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        numRowLevel2++;

                                        if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                        {
                                            listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                        }
                                    }
                                }

                                sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel2.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                            }

                            //sb.Append("<td class='borderThin'></td>");
                            sb.Append("<td class='borderThin'></td>");
                        }

                        if (row["Level"].ToString() == "3")
                        {
                            if (numRowLevel3 == 0)
                            {
                                int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                                List<int> listDoiTacId = new List<int>();
                                listDoiTacId.Add(rowDoiTacId);

                                for (int j = i; j < dtResult.Rows.Count; j++)
                                {
                                    DataRow rowCheck = dtResult.Rows[j];
                                    if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        numRowLevel3++;

                                        if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                        {
                                            listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                        }
                                    }
                                }

                                sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel3.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                            }

                            //sb.Append("<td class='borderThin'></td>"); 
                        }


                        sb.Append("<td class='borderThin'>" + row["TenPhongBan"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=11','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col1_1"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=12','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col1_2"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=21','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col2_1"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=22','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col2_2"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=23','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col2_3"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=31','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col3_1"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=32','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col3_2"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=41','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col4_1"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=42','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col4_2"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=43','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col4_3"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=44','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col4_4"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=5','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col5"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=6','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col6"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopphongcskhvnp&phongBanId=" + row["PhongBanId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=7','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["Col7"], 0).ToString(formatNumber) + "</a></td>");
                        sb.Append("</tr>");

                        totalCol1 += ConvertUtility.ToInt32(row["Col1_1"], 0);
                        totalCol12 += ConvertUtility.ToInt32(row["Col1_2"], 0);
                        totalCol21 += ConvertUtility.ToInt32(row["Col2_1"], 0);
                        totalCol22 += ConvertUtility.ToInt32(row["Col2_2"], 0);
                        totalCol23 += ConvertUtility.ToInt32(row["Col2_3"], 0);
                        totalCol31 += ConvertUtility.ToInt32(row["Col3_1"], 0);
                        totalCol32 += ConvertUtility.ToInt32(row["Col3_2"], 0);
                        totalCol41 += ConvertUtility.ToInt32(row["Col4_1"], 0);
                        totalCol42 += ConvertUtility.ToInt32(row["Col4_2"], 0);
                        totalCol43 += ConvertUtility.ToInt32(row["Col4_3"], 0);
                        totalCol44 += ConvertUtility.ToInt32(row["Col4_4"], 0);
                        totalCol5 += ConvertUtility.ToInt32(row["Col5"], 0);
                        totalCol6 += ConvertUtility.ToInt32(row["Col6"], 0);
                        totalCol7 += ConvertUtility.ToInt32(row["Col7"], 0);
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThinTextBold' colspan='5' >TỔNG</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol1.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol12.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol21.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol22.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol23.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol31.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol32.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol41.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol42.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol43.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol44.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol5.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol6.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol7.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(@"<tr><td colspan='17'>Không có dữ liệu báo cáo</td></tr>");
                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoàng Hải
        /// Modified: DuongDv
        /// Created date : 09/12/2014
        /// Todo : Hiển thị báo cáo theo khu vực
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopKhieuNaiVNP(int doiTacId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;

            if (!isExportExcel)
                formatNumber = FORMAT_NUMBER;

            DataTable dtResult = null;

            dtResult = new ReportImpl().BaoCaoTongHopKhieuNaiVNP_V2_Solr(doiTacId, fromDate, toDate);
            // dtResult = new ReportImpl().BaoCaoTongHopKhieuNaiVNP_V2_Solr_Test(doiTacId, fromDate, toDate);

            dtResult = SortRow(dtResult);

            #region Loop Datatable
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                int totalCol1 = 0;
                int totalCol2 = 0;
                int totalCol21 = 0;
                int totalCol3 = 0;
                int totalCol31 = 0;
                int totalCol32 = 0;
                int totalCol33 = 0;
                int totalCol34 = 0;
                int totalCol4 = 0;
                int totalCol41 = 0;

                int rowspanLevel1 = dtResult.Rows.Count;
                bool isRowspanLevel1 = false;

                int numRowLevel2 = 0;
                int numRowLevel3 = 0;
                int numRowLevel4 = 0;

                int indexRowLevel2 = 1;
                int indexRowLevel3 = 1;
                int indexRowLevel4 = 1;

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow row = dtResult.Rows[i];

                    if (numRowLevel2 != 0)
                    {
                        if (numRowLevel2 != indexRowLevel2)
                        {
                            indexRowLevel2++;
                        }
                        else
                        {
                            numRowLevel2 = 0;
                            indexRowLevel2 = 1;
                        }
                    }

                    if (numRowLevel3 != 0)
                    {
                        if (numRowLevel3 != indexRowLevel3)
                        {
                            indexRowLevel3++;
                        }
                        else
                        {
                            numRowLevel3 = 0;
                            indexRowLevel3 = 1;
                        }
                    }

                    if (numRowLevel4 != 0)
                    {
                        if (numRowLevel4 != indexRowLevel4)
                        {
                            indexRowLevel4++;
                        }
                        else
                        {
                            numRowLevel4 = 0;
                            indexRowLevel4 = 1;
                        }
                    }

                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");

                    if (row["Level"].ToString() == "1")
                    {
                        if (!isRowspanLevel1)
                        {
                            sb.Append("<td valign='top' class='borderThin' rowspan='" + rowspanLevel1.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                            isRowspanLevel1 = true;
                        }

                        sb.Append("<td valign='top' class='borderThin' ></td>");
                        sb.Append("<td valign='top' class='borderThin' ></td>");
                        sb.Append("<td valign='top' class='borderThin' ></td>");
                    }

                    if (row["Level"].ToString() == "2")
                    {
                        if (numRowLevel2 == 0)
                        {
                            int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                            List<int> listDoiTacId = new List<int>();
                            listDoiTacId.Add(rowDoiTacId);

                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowCheck = dtResult.Rows[j];
                                if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                {
                                    numRowLevel2++;

                                    if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                    }
                                }
                            }

                            sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel2.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                        }

                        sb.Append("<td class='borderThin'></td>");
                        sb.Append("<td class='borderThin'></td>");
                    }

                    if (row["Level"].ToString() == "3")
                    {
                        if (numRowLevel3 == 0)
                        {
                            int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                            List<int> listDoiTacId = new List<int>();
                            listDoiTacId.Add(rowDoiTacId);

                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowCheck = dtResult.Rows[j];
                                if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                {
                                    numRowLevel3++;

                                    if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                    }
                                }
                            }

                            sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel3.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                        }

                        sb.Append("<td class='borderThin'></td>");
                    }

                    if (row["Level"].ToString() == "4")
                    {
                        if (numRowLevel4 == 0)
                        {
                            int rowDoiTacId = ConvertUtility.ToInt32(row["DoiTacId"]);
                            List<int> listDoiTacId = new List<int>();
                            listDoiTacId.Add(rowDoiTacId);

                            for (int j = i; j < dtResult.Rows.Count; j++)
                            {
                                DataRow rowCheck = dtResult.Rows[j];
                                if (listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) || listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                {
                                    numRowLevel4++;

                                    if (!listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DoiTacId"])) && listDoiTacId.Contains(ConvertUtility.ToInt32(rowCheck["DonViTrucThuocChoBaoCao"])))
                                    {
                                        listDoiTacId.Add(ConvertUtility.ToInt32(rowCheck["DoiTacId"]));
                                    }
                                }
                            }

                            sb.Append("<td valign='top' class='borderThin' rowspan='" + numRowLevel4.ToString() + "'>" + row["TenDoiTac"].ToString() + "</td>");
                        }
                        //sb.Append("<td class='borderThin'></td>"); 
                    }


                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLTonDongKyTruoc"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLTiepNhan"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=7','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLTaoMoi"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=3','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLDaXuLy"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=10','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLChuyenXuLy"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=11','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLChuyenPhanHoi"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=8','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLDaDong"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=4','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLQuaHanDaXuLy"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=5','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLTonDong"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=6','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + ConvertUtility.ToInt32(row["SLQuaHanTonDong"], 0).ToString(formatNumber) + "</a></td>");
                    sb.Append("</tr>");

                    totalCol1 += ConvertUtility.ToInt32(row["SLTonDongKyTruoc"], 0);
                    totalCol2 += ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
                    totalCol21 += ConvertUtility.ToInt32(row["SLTaoMoi"], 0);
                    totalCol3 += ConvertUtility.ToInt32(row["SLDaXuLy"], 0);
                    totalCol31 += ConvertUtility.ToInt32(row["SLChuyenXuLy"], 0);
                    totalCol32 += ConvertUtility.ToInt32(row["SLChuyenPhanHoi"], 0);
                    totalCol33 += ConvertUtility.ToInt32(row["SLDaDong"], 0);
                    totalCol34 += ConvertUtility.ToInt32(row["SLQuaHanDaXuLy"], 0);
                    totalCol4 += ConvertUtility.ToInt32(row["SLTonDong"], 0);
                    totalCol41 += ConvertUtility.ToInt32(row["SLQuaHanTonDong"], 0);
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='5' >TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol1.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol2.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol21.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol3.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol31.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol32.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol33.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol34.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol4.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalCol41.ToString(formatNumber) + "</td>");
                sb.Append("</tr>");
            }
            else
            {
                sb.Append(@"<tr><td colspan='17'>Không có dữ liệu báo cáo</td></tr>");
            }
            #endregion

            return sb.ToString();
        }

        public string BaoCaoTongHopChiTietPAKNTheoNguoiDungVNPTTT(string fromPage, string userName, int doitacId, int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().BaoCaoTongHopChiTietPAKNTheoNguoiDungVNPTTT_Solr(fromPage, userName, doitacId, phongBanId, fromDate, toDate, reportType);
            PhongBanImpl phongBanImpl = new PhongBanImpl();
            List<PhongBanInfo> listPhongBanInfo = phongBanImpl.GetList();
            sb.Append("<tr>");
            sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan='9'></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th>STT</th>");
            sb.Append("<th>Số thuê bao</th>");
            sb.Append("<th>Mã khiếu nại</th>");
            sb.Append("<th>Ngày tạo khiếu nại</th>");
            sb.Append("<th>Ngày hết hạn toàn trình</th>");
            sb.Append("<th>Ngày quá hạn phòng ban</th>");
            sb.Append("<th>Đơn vị chuyển tiếp</th>");
            sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
            sb.Append("<th>Xem lịch sử</th>");
            sb.Append("</tr>");

            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
            {
                string tenPhongBanXuLyTruoc = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td class='borderThin'>" + tenPhongBanXuLyTruoc + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                sb.Append("</tr>");
            }
            return sb.ToString();
        }
        /// <summary>
        /// Bao cao tong hop pakn da dong theo thoi gian
        /// Nguyen Chi Quang
        /// 02/06/2014
        /// </summary>
        /// <param name="fromPage"></param>
        /// <param name="userName"></param>
        /// <param name="doitacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopKhieuNaiDaDongCuaPhongTheoThoiGian(string fromPage, string userName, int doitacId, int phongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().BaoCaoTongHopKhieuNaiDaDongTrongKhoangThoiGian_Solr(fromPage, userName, doitacId, phongBanId, fromDate, toDate);
            PhongBanImpl phongBanImpl = new PhongBanImpl();
            List<PhongBanInfo> listPhongBanInfo = phongBanImpl.GetList();
            sb.Append("<tr>");
            sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan='9'></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th>STT</th>");
            sb.Append("<th>Số thuê bao</th>");
            sb.Append("<th>Mã khiếu nại</th>");
            sb.Append("<th>Ngày tạo khiếu nại</th>");
            sb.Append("<th>Ngày hết hạn toàn trình</th>");
            sb.Append("<th>Ngày quá hạn phòng ban</th>");
            sb.Append("<th>Đơn vị chuyển tiếp</th>");
            sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
            sb.Append("<th>Xem lịch sử</th>");
            sb.Append("</tr>");

            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
            {
                string tenPhongBanXuLyTruoc = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td class='borderThin'>" + tenPhongBanXuLyTruoc + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                sb.Append("</tr>");
            }
            return sb.ToString();
        }
        public string BaoCaoTongHopKhieuNaiTiepNhanCuaPhongTheoThoiGian(string fromPage, string userName, int doitacId, int phongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().BaoCaoTongHopTiepNhanKhieuNaiTrongKhoangThoiGian_Solr(fromPage, userName, doitacId, phongBanId, fromDate, toDate);
            PhongBanImpl phongBanImpl = new PhongBanImpl();
            List<PhongBanInfo> listPhongBanInfo = phongBanImpl.GetList();
            sb.Append("<tr>");
            sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan='9'></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th>STT</th>");
            sb.Append("<th>Số thuê bao</th>");
            sb.Append("<th>Mã khiếu nại</th>");
            sb.Append("<th>Ngày tạo khiếu nại</th>");
            sb.Append("<th>Ngày hết hạn toàn trình</th>");
            sb.Append("<th>Ngày quá hạn phòng ban</th>");
            sb.Append("<th>Đơn vị chuyển tiếp</th>");
            sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
            sb.Append("<th>Xem lịch sử</th>");
            sb.Append("</tr>");

            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
            {
                string tenPhongBanXuLyTruoc = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td class='borderThin'>" + tenPhongBanXuLyTruoc + "</td>");
                sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                sb.Append("</tr>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date  : 24/02/2014
        /// Todo : Hiển thị danh sách khiếu nại
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public string BaoCaoTongHopKhieuNaiVNP_DanhSachKhieuNai(int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().BaoCaoTongHopKhieuNaiVNP_DanhSachKhieuNai_Solr(phongBanId, fromDate, toDate, reportType);
            PhongBanImpl phongBanImpl = new PhongBanImpl();
            List<PhongBanInfo> listPhongBanInfo = phongBanImpl.GetList();

            switch (reportType)
            {
                case 11:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị chuyển tiếp</th>");
                    sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanXuLyTruoc = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + tenPhongBanXuLyTruoc + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }

                    break;

                case 12:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Người chuyển tiếp đi</th>");
                    sb.Append("<th>Đơn vị nhận chuyển tiếp</th>");
                    sb.Append("<th>Ngày chuyển tiếp tại phòng ban</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanNhanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td class='borderThin'>" + tenPhongBanNhanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;

                case 2:

                    break;
                case 21:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='8'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='8'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Ngày đóng KN</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].Id + "')\">" + listKhieuNaiInfo[i].Id + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLy + "</td>");
                        sb.Append("</tr>");
                    }
                    break;

                case 22:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Người chuyển tiếp</th>");
                    sb.Append("<th>Đơn vị nhận chuyển tiếp</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanNhanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td class='borderThin'>" + tenPhongBanNhanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;
                case 23:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Người chuyển tiếp</th>");
                    sb.Append("<th>Đơn vị nhận chuyển tiếp</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanNhanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td class='borderThin'>" + tenPhongBanNhanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;
                case 3:

                    break;

                case 31:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị chuyển tiếp đến</th>");
                    sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanNhanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + tenPhongBanNhanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;

                case 32:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Người chuyển tiếp</th>");
                    sb.Append("<th>Đơn vị nhận chuyển tiếp</th>");
                    sb.Append("<th>Ngày chuyển tiếp</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanNhanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td class='borderThin'>" + tenPhongBanNhanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;

                case 4:

                case 41:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị chuyển phản hồi</th>");
                    sb.Append("<th>Ngày nhận phản hồi</th>");
                    sb.Append("<th>Ngày đóng khiếu nại</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenDonViChuyenPhanHoi = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + tenDonViChuyenPhanHoi + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLy + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;
                case 42:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị nhận phản hồi</th>");
                    sb.Append("<th>Ngày phản hồi</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanNhanPhanHoi = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + tenPhongBanNhanPhanHoi + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;
                case 43:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='12'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='12'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị nhận phản hồi</th>");
                    sb.Append("<th>Ngày phản hồi</th>");
                    sb.Append("<th>Đơn vị chuyển lại phản hồi</th>");
                    sb.Append("<th>Ngày nhận phản hồi</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanNhanPhanHoi = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId).Name : string.Empty;
                        string tenPhongBanGuiTraLaiPhanHoi = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].LastPhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].LastPhongBanXuLyId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + tenPhongBanNhanPhanHoi + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + tenPhongBanGuiTraLaiPhanHoi + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;
                case 44:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='10'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị nhận phản hồi</th>");
                    sb.Append("<th>Ngày phản hồi</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenDonViNhanPhanHoi = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + tenDonViNhanPhanHoi + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;
                case 5:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị chuyển tiếp</th>");
                    sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + tenPhongBanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }

                    break;
                case 6:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị chuyển tiếp</th>");
                    sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + tenPhongBanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }

                    break;
                case 7:
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Mã khiếu nại</th>");
                    sb.Append("<th>Ngày tạo khiếu nại</th>");
                    sb.Append("<th>Ngày hết hạn toàn trình</th>");
                    sb.Append("<th>Ngày quá hạn phòng ban</th>");
                    sb.Append("<th>Đơn vị chuyển tiếp</th>");
                    sb.Append("<th>Ngày tiếp nhận tại phòng ban</th>");
                    sb.Append("<th>Xem lịch sử</th>");
                    sb.Append("</tr>");

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        string tenPhongBanChuyenTiep = phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId) != null ? phongBanImpl.GetPhongBanByIdFromList(listPhongBanInfo, listKhieuNaiInfo[i].PhongBanXuLyTruocId).Name : string.Empty;
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].KhieuNai_NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td class='borderThin'>" + tenPhongBanChuyenTiep + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                        sb.Append("</tr>");
                    }
                    break;
                default:

                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/04/2014
        /// Todo : Lịch sử khiếu nại
        /// </summary>
        /// <param name="khieuNaiId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoLichSuKhieuNai(int khieuNaiId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().GetListActivityKhieuNai(khieuNaiId, fromDate, toDate);
            if (listKhieuNaiInfo != null && listKhieuNaiInfo.Count > 0)
            {
                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].TenPhongBanXuLyTruoc + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLyTruoc + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].TenPhongBanXuLy + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLy + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(KhieuNai_Actitivy_HanhDong), (byte)listKhieuNaiInfo[i].HanhDong).Replace("_", " ") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm") + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + ((listKhieuNaiInfo[i].LDate > listKhieuNaiInfo[i].NgayQuaHan) ? "Quá hạn" : "") + "</td>");
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 25/09/2013
        /// Todo : Thực hiện báo cáo theo khiếu nại (sử dụng lệnh Sql)
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <returns></returns>
        public string BaoCaoTheoLoaiKhieuNaiToGQKN(int phongBanXuLyId, DateTime fromDate, DateTime toDate, List<string> listLoaiKhieuNaiId, List<string> listLinhVucChungId,
                                                    List<string> listLinhVucConId, out string noiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan, bool isExportExcel)
        {
            string sNoiDungCongViec = string.Empty;
            noiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan = string.Empty;
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                int indexNoiDungCongViec = 0;
                int indexPPSDuocGiaiQuyet = 3;
                int indexHoTroVNPTTT = 4;

                DataSet dsResult = new ReportImpl().BaoCaoTongHopKhieuNaiTheoToGQKNCuaPhongCSKH_Solr(phongBanXuLyId, fromDate, toDate, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);

                if (dsResult != null)
                {
                    DataTable dtLoaiKhieuNai = RemoveRowHasNotValue("LoaiKhieuNaiId", dsResult.Tables[0]);
                    DataTable dtLinhVucChung = RemoveRowHasNotValue("LinhVucChungId,ParentId", dsResult.Tables[1]);
                    DataTable dtLinhVucCon = RemoveRowHasNotValue("LinhVucConId,ParentId", dsResult.Tables[2]);
                    DataTable dtPPS = RemoveRowHasNotValue("LinhVucConId", dsResult.Tables[3]);

                    StringBuilder sb = new StringBuilder();

                    if (dsResult.Tables.Count > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th rowspan='2'>STT</th>");
                        sb.Append("<th rowspan='2'>Công việc</th>");
                        sb.Append(string.Format("<th>Lũy kế KN đã GQ từ đầu năm đến trước ngày {0}</th>", fromDate.ToString("dd/MM/yyyy")));
                        sb.Append(string.Format("<th>Lũy kế KN tồn đọng đến trước ngày {0}</th>", fromDate.ToString("dd/MM/yyyy")));
                        sb.Append(string.Format("<th>Số lượng tiếp nhận từ ngày {0} đến ngày {1}</th>", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy")));
                        sb.Append(string.Format("<th>Số lượng đã giải quyết từ ngày {0} đến ngày {1}</th>", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy")));
                        sb.Append("<th>Số lượng tồn đọng trong tuần do quá hạn</th>");
                        sb.Append(string.Format("<th>Lũy kế KN đã giải quyết từ đầu năm đến ngày {0}</th>", toDate.ToString("dd/MM/yyyy")));
                        sb.Append(string.Format("<th>Lũy kế KN tồn đọng do quá hạn đến ngày ngày {0}</th>", toDate.ToString("dd/MM/yyyy")));
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>1</th>");
                        sb.Append("<th>2</th>");
                        sb.Append("<th>3</th>");
                        sb.Append("<th>4</th>");
                        sb.Append("<th>5 = 3 - 4</th>");
                        sb.Append("<th>6 = 1 + 4</th>");
                        sb.Append("<th>7 = 2 + 5</th>");
                        sb.Append("</tr>");

                        int index = 1;
                        int totalLuyKeDaGiaiQuyetDenDauTuanX = 0;
                        int totalLuyKeTonDongDauTuanX = 0;
                        int totalSoLuongTiepNhanTrongTuan = 0;
                        int totalSoLuongDaGiaiQuyetTrongTuan = 0;

                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow rowLoaiKhieuNai in dsResult.Tables[0].Rows)
                            {
                                totalLuyKeDaGiaiQuyetDenDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0);
                                totalLuyKeTonDongDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"], 0);
                                totalSoLuongTiepNhanTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0);
                                totalSoLuongDaGiaiQuyetTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0);

                                sb.Append("<tr>");
                                sb.Append("<td class='borderThinTextBold' align='center' valign='top'>" + index.ToString() + "</td>");
                                sb.Append("<td class='borderThinTextBold'>" + rowLoaiKhieuNai["LoaiKhieuNai"].ToString() + "</td>");

                                sb.Append("<td align='center' class='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() + "&loaiKhieuNaiType=1&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</a></td>");
                                sb.Append("<td align='center' class='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() + "&loaiKhieuNaiType=1&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"].ToString() + "</a></td>");
                                sb.Append("<td align='center' class='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() + "&loaiKhieuNaiType=1&reportType=3','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"].ToString() + "</a></td>");
                                sb.Append("<td align='center' class='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() + "&loaiKhieuNaiType=1&reportType=4','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</a></td>");
                                sb.Append("<td align='center' class='borderThinTextBold'>" + (ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                                sb.Append("<td align='center' class='borderThinTextBold'>" + (ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                                sb.Append("<td align='center' class='borderThinTextBold'>" + (ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                                sb.Append("</tr>");

                                // Lĩnh vực chung
                                if (dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                                {
                                    StringBuilder sbLinhVucChung = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucChung_HasLink(index, dsResult.Tables[1], dsResult.Tables[2], rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString(), phongBanXuLyId, fromDate, toDate);
                                    sb.Append(sbLinhVucChung.ToString());
                                } // end if (list.Tables[1].Rows.Count > 0)

                                index++;
                            } // end foreach (DataRow rowLoaiKhieuNai in dsResult.Tables[0].Rows)
                        } // end if(dsResult.Tables[0].Rows.Count > 0)
                        else if (dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow rowLoaiKhieuNai in dsResult.Tables[1].Rows)
                            {
                                totalLuyKeDaGiaiQuyetDenDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0);
                                totalLuyKeTonDongDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"], 0);
                                totalSoLuongTiepNhanTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0);
                                totalSoLuongDaGiaiQuyetTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0);
                            }

                            StringBuilder sbLinhVucChung = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucChung_HasLink(index, dsResult.Tables[1], dsResult.Tables[2], "", phongBanXuLyId, fromDate, toDate);
                            if (sbLinhVucChung != null)
                            {
                                sb.Append(sbLinhVucChung.ToString());
                            }
                        } // end else if(dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                        else if (dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow rowLoaiKhieuNai in dsResult.Tables[2].Rows)
                            {
                                totalLuyKeDaGiaiQuyetDenDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0);
                                totalLuyKeTonDongDauTuanX += ConvertUtility.ToInt32(rowLoaiKhieuNai["LuyKeKNTonDongDenDauTuanX"], 0);
                                totalSoLuongTiepNhanTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["SoLuongTiepNhanTrongTuan"], 0);
                                totalSoLuongDaGiaiQuyetTrongTuan += ConvertUtility.ToInt32(rowLoaiKhieuNai["TongSoPAKNGiaiQuyetDuoc"], 0);
                            }

                            StringBuilder sbLinhVucCon = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucCon_HasLink(index, 1, dtLinhVucCon, "", phongBanXuLyId, fromDate, toDate);
                            if (sbLinhVucCon != null)
                            {
                                sb.Append(sbLinhVucCon.ToString());
                            }
                        } // end else if(dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)


                        string lastRow = string.Format("Tổng số ({0}) = (1", index);
                        for (int i = 2; i < index; i++)
                        {
                            lastRow = string.Format("{0}+{1}", lastRow, i);
                        }
                        lastRow += ")";
                        sb.Append("<tr>");
                        sb.Append("<td class='borderThinTextBold' align='center' valign='top'>" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThinTextBold'>" + lastRow + "</td>");

                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalLuyKeDaGiaiQuyetDenDauTuanX.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalLuyKeTonDongDauTuanX.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongTiepNhanTrongTuan.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuongDaGiaiQuyetTrongTuan.ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalSoLuongTiepNhanTrongTuan - totalSoLuongDaGiaiQuyetTrongTuan).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalLuyKeDaGiaiQuyetDenDauTuanX + totalSoLuongDaGiaiQuyetTrongTuan).ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + (totalLuyKeTonDongDauTuanX + totalSoLuongTiepNhanTrongTuan - totalSoLuongDaGiaiQuyetTrongTuan).ToString() + "</td>");
                        sb.Append("</tr>");

                        sNoiDungCongViec = sb.ToString();
                    } // end if (dsResult.Tables.Count > 0)

                    if (dsResult.Tables.Count > 1)
                    {
                        int totalCCT_1 = 0;
                        int totalBuCuoc_1 = 0;
                        decimal totalSoTien_1 = 0;
                        int totalSoLuong_LoiHeThong_1 = 0;
                        decimal totalSoTienGiamTru_LoiHeThong_1 = 0;
                        int totalSoLuong_LoiGiaoDichVien_1 = 0;
                        decimal totalSoTienGiamTru_LoiGiaoDichVien_1 = 0;
                        int totalSoLuong_CSKH_1 = 0;
                        decimal totalSoTienGiamTru_CSKH_1 = 0;
                        int totalSoLuong_Khac_1 = 0;
                        decimal totalSoTienGiamTru_Khac_1 = 0;

                        int totalCCT_2 = 0;
                        int totalBuCuoc_2 = 0;
                        decimal totalSoTien_2 = 0;
                        int totalSoLuong_LoiHeThong_2 = 0;
                        decimal totalSoTienGiamTru_LoiHeThong_2 = 0;
                        int totalSoLuong_LoiGiaoDichVien_2 = 0;
                        decimal totalSoTienGiamTru_LoiGiaoDichVien_2 = 0;
                        int totalSoLuong_CSKH_2 = 0;
                        decimal totalSoTienGiamTru_CSKH_2 = 0;
                        int totalSoLuong_Khac_2 = 0;
                        decimal totalSoTienGiamTru_Khac_2 = 0;

                        int totalCCT_3 = 0;
                        int totalBuCuoc_3 = 0;
                        decimal totalSoTien_3 = 0;
                        int totalSoLuong_LoiHeThong_3 = 0;
                        decimal totalSoTienGiamTru_LoiHeThong_3 = 0;
                        int totalSoLuong_LoiGiaoDichVien_3 = 0;
                        decimal totalSoTienGiamTru_LoiGiaoDichVien_3 = 0;
                        int totalSoLuong_CSKH_3 = 0;
                        decimal totalSoTienGiamTru_CSKH_3 = 0;
                        int totalSoLuong_Khac_3 = 0;
                        decimal totalSoTienGiamTru_Khac_3 = 0;

                        sb = new StringBuilder();
                        sb.Append("<tr>");
                        sb.Append("<th rowspan='4'>STT</th>");
                        sb.Append("<th rowspan='4'>Loại khiếu nại</th>");
                        sb.Append("<th rowspan='4'>Mã dịch vụ</th>");
                        sb.Append(string.Format("<th colspan='11'>Lũy kế từ đầu năm đến thời điểm trước ngày {0}</th>", fromDate.ToString("dd/MM/yyyy")));
                        sb.Append("<th colspan='11'>Số liệu trong thời gian lấy báo cáo</th>");
                        sb.Append(string.Format("<th colspan='11'>Lũy kế từ đầu năm tới ngày {0}</th>", toDate.ToString("dd/MM/yyyy")));
                        sb.Append("<th rowspan='4'>Ghi chú</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th rowspan='3'>Cấp chi tiết</th>");
                        sb.Append("<th colspan='10'>Bù cước</th>");
                        sb.Append("<th rowspan='3'>Cấp chi tiết</th>");
                        sb.Append("<th colspan='10'>Bù cước</th>");
                        sb.Append("<th rowspan='3'>Cấp chi tiết</th>");
                        sb.Append("<th colspan='10'>Bù cước</th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th rowspan='2'>Tổng số</th>");
                        sb.Append("<th colspan='2'>Lỗi hệ thống</th>");
                        sb.Append("<th colspan='2'>Lỗi GDV</th>");
                        sb.Append("<th colspan='2'>CSKH</th>");
                        sb.Append("<th colspan='2'>Khác</th>");
                        sb.Append("<th rowspan='2'>Tổng số tiền bù </th>");
                        sb.Append("<th rowspan='2'>Tổng số</th>");
                        sb.Append("<th colspan='2'>Lỗi hệ thống</th>");
                        sb.Append("<th colspan='2'>Lỗi GDV</th>");
                        sb.Append("<th colspan='2'>CSKH</th>");
                        sb.Append("<th colspan='2'>Khác</th>");
                        sb.Append("<th rowspan='2'>Tổng số tiền bù </th>");
                        sb.Append("<th rowspan='2'>Tổng số</th>");
                        sb.Append("<th colspan='2'>Lỗi hệ thống</th>");
                        sb.Append("<th colspan='2'>Lỗi GDV</th>");
                        sb.Append("<th colspan='2'>CSKH</th>");
                        sb.Append("<th colspan='2'>Khác</th>");
                        sb.Append("<th rowspan='2'>Tổng số tiền bù </br></th>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");

                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");

                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("<th>Số lượng</th>");
                        sb.Append("<th>Số tiền</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < dsResult.Tables[indexPPSDuocGiaiQuyet].Rows.Count; i++)
                        {
                            DataRow row = dsResult.Tables[indexPPSDuocGiaiQuyet].Rows[i];

                            int soLuongCCT_1 = ConvertUtility.ToInt32(row["CapChiTiet_1"], 0);
                            int soLuongBuCuoc_1 = ConvertUtility.ToInt32(row["BuCuoc_1"], 0);
                            decimal soTien_1 = ConvertUtility.ToDecimal(row["SoTien_1"], 0);
                            int soLuong_LoiHeThong_1 = ConvertUtility.ToInt32(row["LoiHeThong_11"], 0);
                            decimal soTienGiamTru_LoiHeThong_1 = ConvertUtility.ToDecimal(row["LoiHeThong_12"], 0);
                            int soLuong_LoiGiaoDichVien_1 = ConvertUtility.ToInt32(row["GiaoDichVien_11"], 0);
                            decimal soTienGiamTru_LoiGiaoDichVien_1 = ConvertUtility.ToDecimal(row["GiaoDichVien_12"], 0);
                            int soLuong_CSKH_1 = ConvertUtility.ToInt32(row["CSKH_11"], 0);
                            decimal soTienGiamTru_CSKH_1 = ConvertUtility.ToDecimal(row["CSKH_12"], 0);
                            int soLuong_Khac_1 = ConvertUtility.ToInt32(row["Khac_11"], 0);
                            decimal soTienGiamTru_Khac_1 = ConvertUtility.ToDecimal(row["Khac_12"], 0);

                            int soLuongCCT_2 = ConvertUtility.ToInt32(row["CapChiTiet_2"], 0);
                            int soLuongBuCuoc_2 = ConvertUtility.ToInt32(row["BuCuoc_2"], 0);
                            decimal soTien_2 = ConvertUtility.ToDecimal(row["SoTien_2"], 0);
                            int soLuong_LoiHeThong_2 = ConvertUtility.ToInt32(row["LoiHeThong_21"], 0);
                            decimal soTienGiamTru_LoiHeThong_2 = ConvertUtility.ToDecimal(row["LoiHeThong_22"], 0);
                            int soLuong_LoiGiaoDichVien_2 = ConvertUtility.ToInt32(row["GiaoDichVien_21"], 0);
                            decimal soTienGiamTru_LoiGiaoDichVien_2 = ConvertUtility.ToDecimal(row["GiaoDichVien_22"], 0);
                            int soLuong_CSKH_2 = ConvertUtility.ToInt32(row["CSKH_21"], 0);
                            decimal soTienGiamTru_CSKH_2 = ConvertUtility.ToDecimal(row["CSKH_22"], 0);
                            int soLuong_Khac_2 = ConvertUtility.ToInt32(row["Khac_21"], 0);
                            decimal soTienGiamTru_Khac_2 = ConvertUtility.ToDecimal(row["Khac_22"], 0);

                            int soLuongCCT_3 = ConvertUtility.ToInt32(row["CapChiTiet_3"], 0);
                            int soLuongBuCuoc_3 = ConvertUtility.ToInt32(row["BuCuoc_3"], 0);
                            decimal soTien_3 = ConvertUtility.ToDecimal(row["SoTien_3"], 0);
                            int soLuong_LoiHeThong_3 = ConvertUtility.ToInt32(row["LoiHeThong_31"], 0);
                            decimal soTienGiamTru_LoiHeThong_3 = ConvertUtility.ToDecimal(row["LoiHeThong_32"], 0);
                            int soLuong_LoiGiaoDichVien_3 = ConvertUtility.ToInt32(row["GiaoDichVien_31"], 0);
                            decimal soTienGiamTru_LoiGiaoDichVien_3 = ConvertUtility.ToDecimal(row["GiaoDichVien_32"], 0);
                            int soLuong_CSKH_3 = ConvertUtility.ToInt32(row["CSKH_31"], 0);
                            decimal soTienGiamTru_CSKH_3 = ConvertUtility.ToDecimal(row["CSKH_32"], 0);
                            int soLuong_Khac_3 = ConvertUtility.ToInt32(row["Khac_31"], 0);
                            decimal soTienGiamTru_Khac_3 = ConvertUtility.ToDecimal(row["Khac_32"], 0);

                            sb.Append("<tr>");
                            sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                            sb.Append("<td class='borderThin'>" + row["TenLoaiKhieuNai"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + row["MaDichVu"].ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuongCCT_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuongBuCuoc_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_LoiHeThong_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_LoiHeThong_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_LoiGiaoDichVien_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_LoiGiaoDichVien_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_CSKH_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_CSKH_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_Khac_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_Khac_1.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTien_1.ToString("###,###,###,###") + "</td>");

                            sb.Append("<td align='center' class='borderThin'>" + soLuongCCT_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuongBuCuoc_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_LoiHeThong_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_LoiHeThong_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_LoiGiaoDichVien_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_LoiGiaoDichVien_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_CSKH_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_CSKH_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_Khac_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_Khac_2.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTien_2.ToString("###,###,###,###") + "</td>");

                            sb.Append("<td align='center' class='borderThin'>" + soLuongCCT_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuongBuCuoc_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_LoiHeThong_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_LoiHeThong_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_LoiGiaoDichVien_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_LoiGiaoDichVien_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_CSKH_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_CSKH_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soLuong_Khac_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTienGiamTru_Khac_3.ToString(formatNumber) + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + soTien_3.ToString("###,###,###,###") + "</td>");

                            sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                            sb.Append("</tr>");

                            totalCCT_1 += soLuongCCT_1;
                            totalBuCuoc_1 += soLuongBuCuoc_1;
                            totalSoTien_1 += soTien_1;
                            totalSoLuong_LoiHeThong_1 += soLuong_LoiHeThong_1;
                            totalSoTienGiamTru_LoiHeThong_1 += soTienGiamTru_LoiHeThong_1;
                            totalSoLuong_LoiGiaoDichVien_1 += soLuong_LoiGiaoDichVien_1;
                            totalSoTienGiamTru_LoiGiaoDichVien_1 += soTienGiamTru_LoiGiaoDichVien_1;
                            totalSoLuong_CSKH_1 += soLuong_CSKH_1;
                            totalSoTienGiamTru_CSKH_1 += soTienGiamTru_CSKH_1;
                            totalSoLuong_Khac_1 += soLuong_Khac_1;
                            totalSoTienGiamTru_Khac_1 += soTienGiamTru_CSKH_1;

                            totalCCT_2 += soLuongCCT_2;
                            totalBuCuoc_2 += soLuongBuCuoc_2;
                            totalSoTien_2 += soTien_2;
                            totalSoLuong_LoiHeThong_2 += soLuong_LoiHeThong_2;
                            totalSoTienGiamTru_LoiHeThong_2 += soTienGiamTru_LoiHeThong_2;
                            totalSoLuong_LoiGiaoDichVien_2 += soLuong_LoiGiaoDichVien_2;
                            totalSoTienGiamTru_LoiGiaoDichVien_2 += soTienGiamTru_LoiGiaoDichVien_2;
                            totalSoLuong_CSKH_2 += soLuong_CSKH_2;
                            totalSoTienGiamTru_CSKH_2 += soTienGiamTru_CSKH_2;
                            totalSoLuong_Khac_2 += soLuong_Khac_2;
                            totalSoTienGiamTru_Khac_2 += soTienGiamTru_CSKH_2;

                            totalCCT_3 += soLuongCCT_3;
                            totalBuCuoc_3 += soLuongBuCuoc_3;
                            totalSoTien_3 += soTien_3;
                            totalSoLuong_LoiHeThong_3 += soLuong_LoiHeThong_3;
                            totalSoTienGiamTru_LoiHeThong_3 += soTienGiamTru_LoiHeThong_3;
                            totalSoLuong_LoiGiaoDichVien_3 += soLuong_LoiGiaoDichVien_3;
                            totalSoTienGiamTru_LoiGiaoDichVien_3 += soTienGiamTru_LoiGiaoDichVien_3;
                            totalSoLuong_CSKH_3 += soLuong_CSKH_3;
                            totalSoTienGiamTru_CSKH_3 += soTienGiamTru_CSKH_3;
                            totalSoLuong_Khac_3 += soLuong_Khac_3;
                            totalSoTienGiamTru_Khac_3 += soTienGiamTru_CSKH_3;
                        }

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThinTextBold' colspan='3'>Tổng số</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalCCT_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalBuCuoc_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_LoiHeThong_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_LoiHeThong_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_LoiGiaoDichVien_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_LoiGiaoDichVien_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_CSKH_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_CSKH_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_Khac_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_Khac_1.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTien_1.ToString("###,###,###,###") + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalCCT_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalBuCuoc_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_LoiHeThong_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_LoiHeThong_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_LoiGiaoDichVien_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_LoiGiaoDichVien_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_CSKH_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_CSKH_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_Khac_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_Khac_2.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTien_2.ToString("###,###,###,###") + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalCCT_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalBuCuoc_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_LoiHeThong_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_LoiHeThong_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_LoiGiaoDichVien_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_LoiGiaoDichVien_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_CSKH_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_CSKH_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoLuong_Khac_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru_Khac_3.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTien_3.ToString("###,###,###,###") + "</td>");
                        sb.Append("<td align='center' class='borderThinTextBold'>&nbsp;</td>");
                        sb.Append("</tr>");

                        noiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan = sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }


            return sNoiDungCongViec;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/06/2014
        /// Todo : Danh sách khiếu nại theo các tiêu chí của báo cáo tổng hợp của tổ GQKN
        /// </summary>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="loaiKhieuNaiId">
        ///     Id của Loại khiếu nại (LoaiKhieuNaiId, LinhVucChungId, LinhVucConId)
        /// </param>
        /// <param name="loaiKhieuNaiType">
        ///     = 1 : Thống kê theo loại khiếu nại
        ///     = 2 : Thống kê  theo lĩnh vực chung
        ///     = 3 : Thống kê theo lĩnh vực con
        /// </param>
        /// <param name="reportType">
        ///     = 1 : Lũy kế khiếu nại đã quyết trước ngày lấy báo cáo
        ///     = 2 : Lũy kế khiếu nại tồn đọng trước ngày lấy báo cáo
        ///     = 3 : Số lượng tiếp nhận trong khoảng thời gian lấy báo cáo
        ///     = 4 : Số lượng đã đã giải quyết trong khoảng thời gian lấy báo cáo
        /// </param>
        /// <returns></returns>
        public string BaoCaoTongHopToGQKN_DanhSachKhieuNai(int phongBanXuLyId, DateTime fromDate, DateTime toDate, int loaiKhieuNaiType, int loaiKhieuNaiId, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().BaoCaoTongHopToGQKN_DanhSachKhieuNai(phongBanXuLyId, fromDate, toDate, loaiKhieuNaiType, loaiKhieuNaiId, reportType);
            if (listKhieuNaiInfo != null)
            {
                switch (reportType)
                {
                    case 1: // Lũy kế khiếu nại đã quyết trước ngày lấy báo cáo
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Mã khiếu nại</th>");
                        sb.Append("<th>Ngày tạo khiếu nại</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("<th>Xem lịch sử</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].Id + "')\">" + listKhieuNaiInfo[i].Id + "</a></td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm") + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].Id + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                            sb.Append("</tr>");
                        }

                        break;

                    case 2: // Lũy kế khiếu nại tồn đọng trước ngày lấy báo cáo
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Mã khiếu nại</th>");
                        sb.Append("<th>Ngày tạo khiếu nại</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Xem lịch sử</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].Id + "')\">" + listKhieuNaiInfo[i].Id + "</a></td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLy + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].Id + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                            sb.Append("</tr>");
                        }
                        break;

                    case 3: // Số lượng tiếp nhận trong khoảng thời gian lấy báo cáo
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Mã khiếu nại</th>");
                        sb.Append("<th>Ngày tạo khiếu nại</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Xem lịch sử</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].KhieuNaiId + "')\">" + listKhieuNaiInfo[i].KhieuNaiId + "</a></td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NguoiXuLy + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].KhieuNaiId + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                            sb.Append("</tr>");
                        }
                        break;

                    case 4: // Số lượng đã đã giải quyết trong khoảng thời gian lấy báo cáo
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'>Số lượng khiếu nại : " + listKhieuNaiInfo.Count.ToString() + " </td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='6'></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Mã khiếu nại</th>");
                        sb.Append("<th>Ngày tạo khiếu nại</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("<th>Xem lịch sử</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td align='center' class='borderThin'>" + (i + 1).ToString() + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].SoThueBao + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"ShowPoupChiTietKN('" + listKhieuNaiInfo[i].Id + "')\">" + listKhieuNaiInfo[i].Id + "</a></td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm") + "</td>");
                            sb.Append("<td align='center' class='borderThin'>" + listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm") + "</td>");
                            sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/lichsukhieunai.aspx?khieuNaiId=" + listKhieuNaiInfo[i].Id + "&soThueBao=" + listKhieuNaiInfo[i].SoThueBao + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">Xem</a></td>");
                            sb.Append("</tr>");
                        }
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/01/2015
        /// Todo : Lấy báo cáo theo tuần của toàn hệ thống, kết hợp lấy báo cáo trong tháng và năm
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <returns></returns>
        public string BaoCaoTongHopKhieuNaiToanMangTheoTuan(DateTime fromDate, DateTime toDate, int nguonKhieuNai)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            int weekNo = currentCulture.Calendar.GetWeekOfYear(
                            fromDate,
                            currentCulture.DateTimeFormat.CalendarWeekRule,
                            currentCulture.DateTimeFormat.FirstDayOfWeek);

            StringBuilder sb = new StringBuilder();

            string sLink = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopkhieunaitoanmangtheotuan&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&reportType={1}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
            sb.Append("<tr>");
            sb.Append("<th rowspan='2'>Nội dung </th>");
            //sb.Append("</tr>");            
            //sb.Append("<tr>");
            sb.AppendFormat("<th colspan='4'>Tuần {0}</th>", weekNo);
            sb.Append("<th colspan='2'>Lũy kế</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th>Tổng số khiếu nại tiếp nhận</th>");
            sb.Append("<th>Tổng số khiếu nại giải quyết</th>");
            sb.Append("<th>Tổng số khiếu nại quá hạn</th>");
            sb.Append("<th>Tỷ lệ khiếu nại quá hạn</th>");
            sb.Append("<th>Số khiếu nại giải quyết trong tháng</th>");
            sb.AppendFormat("<th>Số khiếu nại được giải quyết từ đầu {0}</th>", toDate.Year);
            sb.Append("</tr>");

            DataTable dtResult = new ReportImpl().GetTongHopKhieuNaiToanMangTheoTuan_Solr(fromDate, toDate, nguonKhieuNai);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow row = dtResult.Rows[0];
                sb.Append("<tr>");
                sb.Append("<td>Khiếu nại phản ảnh của KH trên hệ thống CCOS</td>");
                sb.AppendFormat(sLink, row["SLKhieuNaiTiepNhan"], 1);
                sb.AppendFormat(sLink, row["SLKNDaGiaiQuyet"], 2);
                sb.AppendFormat(sLink, row["SLKNQuaHan"], 3);
                sb.AppendFormat("<td>{0}%</td>", row["TyLeQuaHan"]);
                //sb.AppendFormat(sLink, row["SLKNDaGiaiQuyetTrongThang"], 4);
                //sb.AppendFormat(sLink, row["SLKNDaGiaiQuyetTuDauNam"], 5);
                sb.AppendFormat("<td>{0}</td>", row["SLKNDaGiaiQuyetTrongThang"]);
                sb.AppendFormat("<td>{0}</td>", row["SLKNDaGiaiQuyetTuDauNam"]);
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr>");
                sb.Append("<td colspan='7'>Không có dữ liệu</td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/01/2015
        /// Todo : Báo cáo tổng hợp theo tháng
        /// </summary>
        /// <param name="toDate"></param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <returns></returns>
        public string BaoCaoTongHopKhieuNaiToanMangTheoThang(DateTime toDate, int nguonKhieuNai)
        {
            StringBuilder sb = new StringBuilder();

            string sLink = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopkhieunaitoanmangtheothang&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&reportType={1}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
            sb.Append("<tr>");
            sb.Append("<th rowspan='2'>Nội dung </th>");
            sb.AppendFormat("<th colspan='3'>Tổng số khiếu nại giải quyết</th>");
            sb.Append("<th colspan='2'>Tỷ lệ khiếu nại quá hạn</th>");
            sb.Append("<th>Lũy kế</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.AppendFormat("<th>Tháng {0}</th>", toDate.Month);
            sb.AppendFormat("<th>Tháng {0}</th>", toDate.AddMonths(-1).Month);
            sb.Append("<th>So sánh (tăng/giảm)</th>");
            sb.AppendFormat("<th>Tháng {0}</th>", toDate.Month);
            sb.AppendFormat("<th>Tháng {0}</th>", toDate.AddMonths(-1).Month);
            sb.AppendFormat("<th>Số khiếu nại được giải quyết từ đầu {0}</th>", toDate.Year);
            sb.Append("</tr>");

            DataTable dtResult = new ReportImpl().GetTongHopKhieuNaiToanMangTheoThang_Solr(toDate, nguonKhieuNai);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow row = dtResult.Rows[0];
                sb.Append("<tr>");
                sb.Append("<td>Khiếu nại phản ảnh của KH trên hệ thống CCOS</td>");
                //sb.AppendFormat(sLink, row["SLDaGiaiQuyetThangNay"], 1);
                //sb.AppendFormat(sLink, row["SLDaGiaiQuyetThangTruoc"], 2);
                sb.AppendFormat("<td>{0}</td>", row["SLDaGiaiQuyetThangNay"]);
                sb.AppendFormat("<td>{0}</td>", row["SLDaGiaiQuyetThangTruoc"]);
                sb.AppendFormat("<td>{0}{1}%</td>", Convert.ToDecimal(row["TyLeGiaiQuyet"]) > 0 ? "+" : "", row["TyLeGiaiQuyet"]);
                sb.AppendFormat("<td>{0}%</td>", row["TyLeQuaHanThangNay"]);
                sb.AppendFormat("<td>{0}%</td>", row["TyLeQuaHanThangTruoc"]);
                //sb.AppendFormat(sLink, row["LuyKeGiaiQuyetTuDauNam"], 3);
                sb.AppendFormat("<td>{0}</td>", row["LuyKeGiaiQuyetTuDauNam"]);
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr>");
                sb.Append("<td colspan='7'>Không có dữ liệu</td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/01/2015
        /// Todo : Hiển thị báo cáo tổng hợp giảm trừ toàn mạng
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopGiamTruToanMang(DateTime fromDate, DateTime toDate, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().BaoCaoTongHopGiamTruToanMang_Solr(fromDate, toDate);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                decimal totalSoTienGiamTru = 0;
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Khu vực</th>");
                sb.Append("<th>TỔNG SỐ TIỀN GIẢM TRỪ <br/>(Đồng- chưa VAT)</th>");
                sb.Append("<th>Ghi chú</th>");
                sb.Append("</tr>");

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow row = dtResult.Rows[i];
                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");
                    sb.Append("<td align='left' class='borderThin'>" + row["TenKhuVuc"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + ConvertUtility.ToDecimal(row["SoTienGiamTru"]).ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>&nbsp;</td>");
                    sb.Append("</tr>");

                    totalSoTienGiamTru += ConvertUtility.ToDecimal(row["SoTienGiamTru"]);
                }

                sb.Append("<tr style='font-size:200%'>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + totalSoTienGiamTru.ToString(formatNumber) + "</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>&nbsp;</td>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 10/02/2014
        /// Todo : Thực hiện sắp xếp các row theo thứ tự
        /// </summary>
        /// <param name="dtResult"></param>
        private DataTable SortRow(DataTable dtResult)
        {
            if (dtResult == null)
                return null;

            // Đưa bản ghi đầu tiên vào danh sách
            // Duyệt từ bản ghi thứ 2 trở đi
            //  Nếu có cùng DoiTacId thì add vào
            //  Nếu không tồn tại bản ghi có cùng DoiTacId thì add các bản ghi có đơn vị trực thuộc
            //      Lại quay lại bước trên : tiếp tục add các bản ghi có cùng DoiTacId
            DataTable dtNewResult = dtResult.Copy();
            dtNewResult.Rows.Clear();

            dtNewResult.ImportRow(dtResult.Rows[0]);

            List<int> listDoiTacId = new List<int>();
            listDoiTacId.Add(ConvertUtility.ToInt32(dtResult.Rows[0]["DoiTacId"]));

            dtResult.Rows.RemoveAt(0);

            while (dtResult.Rows.Count > 0)
            {
                bool isNeedRemoveDoiTacIdInList = true;

                if (listDoiTacId.Count > 0)
                {
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        if (ConvertUtility.ToInt32(dtResult.Rows[i]["DoiTacId"]) == listDoiTacId[listDoiTacId.Count - 1])
                        {
                            dtNewResult.ImportRow(dtResult.Rows[i]);
                            dtResult.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                } // end if(listDoiTacId.Count > 0)


                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    if (listDoiTacId.Count == 0)
                    {
                        listDoiTacId.Add(ConvertUtility.ToInt32(dtResult.Rows[i]["DonViTrucThuocChoBaoCao"]));
                    }

                    if (ConvertUtility.ToInt32(dtResult.Rows[i]["DonViTrucThuocChoBaoCao"]) == listDoiTacId[listDoiTacId.Count - 1])
                    {
                        if (!listDoiTacId.Contains(ConvertUtility.ToInt32(dtResult.Rows[i]["DoiTacId"])))
                        {
                            listDoiTacId.Add(ConvertUtility.ToInt32(dtResult.Rows[i]["DoiTacId"]));
                        }

                        dtNewResult.ImportRow(dtResult.Rows[i]);
                        dtResult.Rows.RemoveAt(i);
                        i--;

                        isNeedRemoveDoiTacIdInList = false;
                        break;
                    }
                }

                if (isNeedRemoveDoiTacIdInList && listDoiTacId.Count > 0)
                {
                    listDoiTacId.RemoveAt(listDoiTacId.Count - 1);
                }
            }

            return dtNewResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/07/2014
        /// </summary>
        /// <param name="khuVucXuLyId"></param>
        /// <param name="listDoiTacXuLyId"></param>
        /// <param name="listPhongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoChiTietKhieuNaiQuaHan(List<int> listDoiTacXuLyId, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportImpl().DanhSachKhieuNaiTonDongQuaHan(listDoiTacXuLyId, toDate);
            if (listKhieuNaiInfo != null && listKhieuNaiInfo.Count > 0)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                List<PhongBanInfo> listPhongBan = phongBanImpl.GetList();
                sb.AppendFormat("<tr><td colspan='14'>Tổng số bản ghi : {0}</td></tr>", listKhieuNaiInfo.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã PA</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Loại khiếu nại</th>");
                sb.Append("<th>Lĩnh vực chung</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Nội dung phản ánh</th>");
                sb.Append("<th>Ngày tạo khiếu nại</th>");
                sb.Append("<th>Phòng ban xử lý trước</th>");
                sb.Append("<th>Ngày tiếp nhận PB</th>");
                sb.Append("<th>Phòng ban xử lý</th>");
                sb.Append("<th>Ngày tiếp nhận người xử lý</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("<th>Ngày quá hạn phòng ban</th>");
                sb.Append("</tr>");

                int index = 1;
                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    string phongBanXuLyTruoc = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                    string phongBanXuLy = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                    string ngayTiepNhan_NguoiXuLy = listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm");
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", phongBanXuLyTruoc);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", phongBanXuLy);
                    sb.AppendFormat("<td>{0}</td>", ngayTiepNhan_NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                    sb.Append("</tr>");

                    index++;
                }
            }

            return sb.ToString();
        }

        //public string BaoCaoChiTietKhieuNaiQuaHan(List<int> listDoiTacXuLyId, DateTime toDate)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    DataTable dtKhieuNai = new ReportSqlImpl().ListKhieuNaiQuaHanPhongBan(listDoiTacXuLyId, toDate);
        //    if (dtKhieuNai != null && dtKhieuNai.Rows.Count > 0)
        //    {
        //        PhongBanImpl phongBanImpl = new PhongBanImpl();
        //        List<PhongBanInfo> listPhongBan = phongBanImpl.GetList();
        //        sb.AppendFormat("<tr><td colspan='13'>Tổng số bản ghi : {0}</td></tr>", dtKhieuNai.Rows.Count);
        //        sb.Append("<tr>");
        //        sb.Append("<th>STT</th>");
        //        sb.Append("<th>Mã PA</th>");
        //        sb.Append("<th>Số thuê bao</th>");
        //        sb.Append("<th>Loại khiếu nại</th>");
        //        sb.Append("<th>Lĩnh vực chung</th>");
        //        sb.Append("<th>Lĩnh vực con</th>");
        //        sb.Append("<th>Ngày tạo khiếu nại</th>");
        //        sb.Append("<th>Phòng ban xử lý trước</th>");
        //        sb.Append("<th>Ngày tiếp nhận PB</th>");
        //        sb.Append("<th>Phòng ban xử lý</th>");
        //        sb.Append("<th>Ngày tiếp nhận người xử lý</th>");
        //        sb.Append("<th>Người xử lý</th>");
        //        sb.Append("<th>Ngày quá hạn phòng ban</th>");
        //        sb.Append("</tr>");

        //        int index = 1;
        //        foreach (DataRow row in dtKhieuNai.Rows)
        //        {
        //            string phongBanXuLyTruoc = phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanXuLyTruocId"]));
        //            string phongBanXuLy = phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanXuLyId"]));
        //            string ngayTiepNhan_NguoiXuLy = ConvertUtility.ToDateTime(row["NgayTiepNhan_NguoiXuLy"], DateTime.MinValue) == DateTime.MinValue
        //                || ConvertUtility.ToDateTime(row["NgayTiepNhan_NguoiXuLy"], DateTime.MinValue).Year == DateTime.MaxValue.Year
        //                ?
        //                string.Empty : ConvertUtility.ToDateTime(row["NgayTiepNhan_NguoiXuLy"]).ToString("dd/MM/yyyy HH:mm");
        //            sb.Append("<tr>");
        //            sb.AppendFormat("<td>{0}</td>", index);
        //            sb.AppendFormat("<td>{0}</td>", row["KhieuNaiId"]);
        //            sb.AppendFormat("<td>{0}</td>", row["SoThueBao"]);
        //            sb.AppendFormat("<td>{0}</td>", row["LoaiKhieuNai"]);
        //            sb.AppendFormat("<td>{0}</td>", row["LinhVucChung"]);
        //            sb.AppendFormat("<td>{0}</td>", row["LinhVucCon"]);
        //            sb.AppendFormat("<td>{0}</td>", ConvertUtility.ToDateTime(row["NgayTaoKhieuNai"]).ToString("dd/MM/yyyy HH:mm"));
        //            sb.AppendFormat("<td>{0}</td>", phongBanXuLyTruoc);
        //            sb.AppendFormat("<td>{0}</td>", ConvertUtility.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm"));
        //            sb.AppendFormat("<td>{0}</td>", phongBanXuLy);
        //            sb.AppendFormat("<td>{0}</td>", ngayTiepNhan_NguoiXuLy);
        //            sb.AppendFormat("<td>{0}</td>", row["NguoiXuLy"]);
        //            sb.AppendFormat("<td>{0}</td>", ConvertUtility.ToDateTime(row["NgayQuaHan"]).ToString("dd/MM/yyyy HH:mm"));
        //            sb.Append("</tr>");

        //            index++;
        //        }
        //    }

        //    return sb.ToString();
        //}

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 28/05/2015
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isExportExcel"></param>
        /// <returns></returns>
        public string BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL(int khuVucId, int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate, int nguonKhieuNai, List<int> listLoaiKhieuNaiId, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().ThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL_Solr(khuVucId, doiTacId, phongBanId, fromDate, toDate, nguonKhieuNai, listLoaiKhieuNaiId);
            List<LoiKhieuNaiInfo> listLoiKhieuNai = ServiceFactory.GetInstanceLoiKhieuNai().GetListSortHierarchy();
            if (listLoiKhieuNai == null)
            {
                listLoiKhieuNai = new List<LoiKhieuNaiInfo>();
            }

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string jsScript = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&doiTacId=" + doiTacId + "&phongBanId=" + phongBanId + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={0}&nguyenNhanLoiId={1}&chiTietLoiId={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{3}</a></td>";
                string jsScriptKN = "<td align='center' class='borderThin' style='background-color: yellow'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&doiTacId=" + doiTacId + "&phongBanId=" + phongBanId + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={0}&nguyenNhanLoiId={1}&chiTietLoiId={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{3}</a></td>";

                sb.Append("<tr>");
                sb.Append("<th rowspan='2'>STT</th>");
                sb.Append("<th rowspan='2'>Loại khiếu nại</th>");
                sb.Append("<th rowspan='2'>SL tiếp nhận</th>");
                sb.Append("<th rowspan='2'>SL đã đóng</th>");
                for (int i = 0; i < listLoiKhieuNai.Count; i++)
                {
                    if (listLoiKhieuNai[i].Cap == 1)
                    {
                        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate (LoiKhieuNaiInfo obj)
                        { return obj.ParentId == listLoiKhieuNai[i].Id; });
                        if (listChild != null && listChild.Count > 0)
                        {
                            sb.AppendFormat("<th colspan='{1}'>{0}</th>", listLoiKhieuNai[i].TenLoi, listChild.Count + 2);
                        }
                        else
                        {
                            sb.AppendFormat("<th rowspan='2'>{0}</th>", listLoiKhieuNai[i].TenLoi);
                        }
                    }
                }

                sb.Append("</tr>");

                for (int i = 0; i < listLoiKhieuNai.Count; i++)
                {
                    if (listLoiKhieuNai[i].Cap == 1)
                    {
                        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate (LoiKhieuNaiInfo obj)
                        { return obj.ParentId == listLoiKhieuNai[i].Id; });
                        if (listChild != null && listChild.Count > 0)
                        {
                            sb.Append("<th>Tổng</th>");
                            sb.Append("<th>Khác</th>");
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<th>L{0}</th>", listLoiKhieuNai[i].Id);
                    }
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                for (int i = 2; i < dtResult.Columns.Count; i++)
                {
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", dtResult.Rows[0][i]);
                }

                sb.Append("</tr>");

                for (int i = 1; i < dtResult.Rows.Count - 1; i++)
                {
                    DataRow row = dtResult.Rows[i];

                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", i);

                    foreach (DataColumn col in dtResult.Columns)
                    {
                        if (col.ColumnName.ToLower() == "loaikhieunaiid")
                        {
                            continue;
                        }

                        if (col.ColumnName.ToLower() == "loaikhieunai" || col.ColumnName.ToLower() == "sltiepnhan" || col.ColumnName.ToLower() == "sldadong")
                        {
                            sb.AppendFormat("<td>{0}</td>", row[col]);
                        }
                        else
                        {
                            string nguyenNhanLoiId = string.Empty;
                            string chiTietLoiId = "-1";
                            bool isKN = false;

                            if (col.ColumnName.Contains('_'))
                            {
                                nguyenNhanLoiId = col.ColumnName.Split('_')[0];
                                chiTietLoiId = col.ColumnName.Split('_')[1];

                                for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count; indexLoiKhieuNai++)
                                {
                                    if (listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == nguyenNhanLoiId)
                                    {
                                        if (listLoiKhieuNai[indexLoiKhieuNai].Loai == (int)LoiKhieuNai_Loai.Khiếu_nại)
                                        {
                                            isKN = true;
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count; indexLoiKhieuNai++)
                                {
                                    if (listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == col.ColumnName)
                                    {
                                        if (listLoiKhieuNai[indexLoiKhieuNai].Cap == 1)
                                        {
                                            nguyenNhanLoiId = col.ColumnName;
                                        }
                                        else if (listLoiKhieuNai[indexLoiKhieuNai].Cap == 2)
                                        {
                                            nguyenNhanLoiId = listLoiKhieuNai[indexLoiKhieuNai].ParentId.ToString();
                                            chiTietLoiId = col.ColumnName;

                                            if (listLoiKhieuNai[indexLoiKhieuNai].Loai == (int)LoiKhieuNai_Loai.Khiếu_nại)
                                            {
                                                isKN = true;
                                            }
                                        }

                                        break;
                                    } // end if(listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == col.ColumnName)
                                } // end for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count;indexLoiKhieuNai ++ )                                    
                            }

                            if (isKN)
                            {
                                sb.AppendFormat(jsScriptKN, row["LoaiKhieuNaiId"], nguyenNhanLoiId, chiTietLoiId, row[col]);

                            }
                            else
                            {
                                sb.AppendFormat(jsScript, row["LoaiKhieuNaiId"], nguyenNhanLoiId, chiTietLoiId, row[col]);
                            }
                        }
                        // sb.AppendFormat("<td>{0}</td>", row[col]); 

                    } // end foreach(DataColumn col in dtResult.Columns)


                    //sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&loaiKhieuNaiId=" + row["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + row["LinhVucChungId"].ToString() + "&LinhVucConId=" + row["LinhVucConId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTiepNhan.ToString(formatNumber) + "</a></td>");
                    //sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&loaiKhieuNaiId=" + row["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + row["LinhVucChungId"].ToString() + "&LinhVucConId=" + row["LinhVucConId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongDaDong.ToString(formatNumber) + "</a></td>");                   
                    sb.Append("</tr>");

                    //totalSLTiepNhan += soLuongTiepNhan;
                    //totalSLDaDong += soLuongDaDong;
                } // end 

                int lastRowIndex = dtResult.Rows.Count - 1;

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='2'>TỔNG</td>");
                for (int i = 2; i < dtResult.Columns.Count; i++)
                {
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", dtResult.Rows[lastRowIndex][i]);
                }
                sb.Append("</tr>");
            }
            else
            {
                sb.Append(@"<tr>
                                <td colspan='40'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 27/06/2015
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="isExportExcel"></param>
        /// <returns></returns>
        public string BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL(int khuVucId, int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate, int nguonKhieuNai, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().ThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL_Solr(khuVucId, doiTacId, phongBanId, fromDate, toDate, nguonKhieuNai, listLoaiKhieuNaiId, listLinhVucChungId);
            List<LoiKhieuNaiInfo> listLoiKhieuNai = ServiceFactory.GetInstanceLoiKhieuNai().GetListSortHierarchy();
            if (listLoiKhieuNai == null)
            {
                listLoiKhieuNai = new List<LoiKhieuNaiInfo>();
            }

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string jsScript = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&doiTacId=" + doiTacId + "&phongBanId=" + phongBanId + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={0}&linhVucChungId={1}&nguyenNhanLoiId={2}&chiTietLoiId={3}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{4}</a></td>";
                string jsScriptKN = "<td align='center' class='borderThin' style='background-color: yellow'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&doiTacId=" + doiTacId + "&phongBanId=" + phongBanId + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={0}&linhVucChungId={1}&nguyenNhanLoiId={2}&chiTietLoiId={3}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{4}</a></td>";

                sb.Append("<tr>");
                sb.Append("<th rowspan='2'>STT</th>");
                sb.Append("<th rowspan='2'>Loại khiếu nại</th>");
                sb.Append("<th rowspan='2'>Lĩnh vực chung</th>");
                sb.Append("<th rowspan='2'>SL tiếp nhận</th>");
                sb.Append("<th rowspan='2'>SL đã đóng</th>");
                for (int i = 0; i < listLoiKhieuNai.Count; i++)
                {
                    if (listLoiKhieuNai[i].Cap == 1)
                    {
                        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate (LoiKhieuNaiInfo obj)
                        { return obj.ParentId == listLoiKhieuNai[i].Id; });
                        if (listChild != null && listChild.Count > 0)
                        {
                            sb.AppendFormat("<th colspan='{1}'>{0}</th>", listLoiKhieuNai[i].TenLoi, listChild.Count + 2);
                        }
                        else
                        {
                            sb.AppendFormat("<th rowspan='2'>{0}</th>", listLoiKhieuNai[i].TenLoi);
                        }
                    }
                }

                sb.Append("</tr>");

                for (int i = 0; i < listLoiKhieuNai.Count; i++)
                {
                    if (listLoiKhieuNai[i].Cap == 1)
                    {
                        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate (LoiKhieuNaiInfo obj)
                        { return obj.ParentId == listLoiKhieuNai[i].Id; });
                        if (listChild != null && listChild.Count > 0)
                        {
                            sb.Append("<th>Tổng</th>");
                            sb.Append("<th>Khác</th>");
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<th>L{0}</th>", listLoiKhieuNai[i].Id);
                    }
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='3'>TỔNG</td>");
                for (int i = 4; i < dtResult.Columns.Count; i++)
                {
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", dtResult.Rows[0][i]);
                }

                sb.Append("</tr>");

                string sCurLoaiKhieuNaiId = string.Empty;
                int index = 1;
                for (int i = 1; i < dtResult.Rows.Count - 1; i++)
                {
                    DataRow row = dtResult.Rows[i];

                    int rowspan = 0;

                    if (sCurLoaiKhieuNaiId != row["LoaiKhieuNaiId"].ToString())
                    {
                        sCurLoaiKhieuNaiId = row["LoaiKhieuNaiId"].ToString();
                        for (int indexRowspan = i; indexRowspan < dtResult.Rows.Count; indexRowspan++)
                        {
                            if (sCurLoaiKhieuNaiId != dtResult.Rows[indexRowspan]["LoaiKhieuNaiId"].ToString())
                            {
                                break;
                            }

                            rowspan++;
                        }
                    }

                    sb.Append("<tr>");
                    //sb.AppendFormat("<td>{0}</td>", i);

                    foreach (DataColumn col in dtResult.Columns)
                    {
                        if (col.ColumnName.ToLower() == "loaikhieunaiid" || col.ColumnName.ToLower() == "linhvucchungid")
                        {
                            continue;
                        }

                        if (col.ColumnName.ToLower() == "loaikhieunai" || col.ColumnName.ToLower() == "linhvucchung" || col.ColumnName.ToLower() == "sltiepnhan" || col.ColumnName.ToLower() == "sldadong")
                        {
                            if (col.ColumnName.ToLower() == "loaikhieunai")
                            {
                                if (rowspan > 0)
                                {
                                    sb.AppendFormat("<td rowspan='{1}'>{0}</td>", (index++).ToString(), rowspan);
                                    sb.AppendFormat("<td rowspan='{1}'>{0}</td>", row[col], rowspan);
                                    rowspan = 0;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("<td>{0}</td>", row[col]);
                            }
                        }
                        else
                        {
                            string nguyenNhanLoiId = string.Empty;
                            string chiTietLoiId = "-1";
                            bool isKN = false;

                            if (col.ColumnName.Contains('_'))
                            {
                                nguyenNhanLoiId = col.ColumnName.Split('_')[0];
                                chiTietLoiId = col.ColumnName.Split('_')[1];

                                for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count; indexLoiKhieuNai++)
                                {
                                    if (listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == nguyenNhanLoiId)
                                    {
                                        if (listLoiKhieuNai[indexLoiKhieuNai].Loai == (int)LoiKhieuNai_Loai.Khiếu_nại)
                                        {
                                            isKN = true;
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count; indexLoiKhieuNai++)
                                {
                                    if (listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == col.ColumnName)
                                    {
                                        if (listLoiKhieuNai[indexLoiKhieuNai].Cap == 1)
                                        {
                                            nguyenNhanLoiId = col.ColumnName;
                                        }
                                        else if (listLoiKhieuNai[indexLoiKhieuNai].Cap == 2)
                                        {
                                            nguyenNhanLoiId = listLoiKhieuNai[indexLoiKhieuNai].ParentId.ToString();
                                            chiTietLoiId = col.ColumnName;

                                            if (listLoiKhieuNai[indexLoiKhieuNai].Loai == (int)LoiKhieuNai_Loai.Khiếu_nại)
                                            {
                                                isKN = true;
                                            }
                                        }
                                    } // end if(listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == col.ColumnName)
                                } // end for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count;indexLoiKhieuNai ++ )                                    
                            }

                            if (isKN)
                            {
                                sb.AppendFormat(jsScriptKN, row["LoaiKhieuNaiId"], row["LinhVucChungId"], nguyenNhanLoiId, chiTietLoiId, row[col]);

                            }
                            else
                            {
                                sb.AppendFormat(jsScript, row["LoaiKhieuNaiId"], row["LinhVucChungId"], nguyenNhanLoiId, chiTietLoiId, row[col]);
                            }
                            //sb.AppendFormat(jsScript, row["LoaiKhieuNaiId"], row["LinhVucChungId"], nguyenNhanLoiId, chiTietLoiId, row[col]);
                        }
                        // sb.AppendFormat("<td>{0}</td>", row[col]); 

                    } // end foreach(DataColumn col in dtResult.Columns)

                    sb.Append("</tr>");

                } // end 

                int lastRowIndex = dtResult.Rows.Count - 1;

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='3'>TỔNG</td>");
                for (int i = 4; i < dtResult.Columns.Count; i++)
                {
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", dtResult.Rows[lastRowIndex][i]);
                }
                sb.Append("</tr>");
            }
            else
            {
                sb.Append(@"<tr>
                                <td colspan='40'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 27/11/2015
        /// Todo : Báo cáo thống kê khiếu nại theo nguyên nhân lỗi
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="isExportExcel"></param>
        /// <returns></returns>
        public string BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL(int khuVucId, int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate, int nguonKhieuNai, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, List<int> listLinhVucConId, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            DataTable dtResult = new ReportImpl().ThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL_Solr(khuVucId, doiTacId, phongBanId, fromDate, toDate, nguonKhieuNai, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId);
            List<LoiKhieuNaiInfo> listLoiKhieuNai = ServiceFactory.GetInstanceLoiKhieuNai().GetListSortHierarchy();
            if (listLoiKhieuNai == null)
            {
                listLoiKhieuNai = new List<LoiKhieuNaiInfo>();
            }

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string jsScript = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&doiTacId=" + doiTacId + "&phongBanId=" + phongBanId + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={0}&linhVucChungId={1}&linhVucConId={2}&nguyenNhanLoiId={3}&chiTietLoiId={4}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{5}</a></td>";
                string jsScriptKN = "<td align='center' class='borderThin' style='background-color: yellow'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_vnp_thongkekhieuNaitheonguyennhanloi&khuVucId=" + khuVucId + "&doiTacId=" + doiTacId + "&phongBanId=" + phongBanId + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={0}&linhvucChungId={1}&linhVucConId={2}&nguyenNhanLoiId={3}&chiTietLoiId={4}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{5}</a></td>";

                sb.Append("<tr>");
                sb.Append("<th rowspan='2'>STT</th>");
                sb.Append("<th rowspan='2'>Loại khiếu nại</th>");
                sb.Append("<th rowspan='2'>Lĩnh vực chung</th>");
                sb.Append("<th rowspan='2'>Lĩnh vực con</th>");
                sb.Append("<th rowspan='2'>SL tiếp nhận</th>");
                sb.Append("<th rowspan='2'>SL đã đóng</th>");
                for (int i = 0; i < listLoiKhieuNai.Count; i++)
                {
                    if (listLoiKhieuNai[i].Cap == 1)
                    {
                        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate (LoiKhieuNaiInfo obj)
                        { return obj.ParentId == listLoiKhieuNai[i].Id; });
                        if (listChild != null && listChild.Count > 0)
                        {
                            sb.AppendFormat("<th colspan='{1}'>{0}</th>", listLoiKhieuNai[i].TenLoi, listChild.Count + 2);
                        }
                        else
                        {
                            sb.AppendFormat("<th rowspan='2'>{0}</th>", listLoiKhieuNai[i].TenLoi);
                        }
                    }
                }

                sb.Append("</tr>");

                for (int i = 0; i < listLoiKhieuNai.Count; i++)
                {
                    if (listLoiKhieuNai[i].Cap == 1)
                    {
                        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate (LoiKhieuNaiInfo obj)
                        { return obj.ParentId == listLoiKhieuNai[i].Id; });
                        if (listChild != null && listChild.Count > 0)
                        {
                            sb.Append("<th>Tổng</th>");
                            sb.Append("<th>Khác</th>");
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<th>L{0}</th>", listLoiKhieuNai[i].Id);
                    }
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='4'>TỔNG</td>");
                for (int i = 6; i < dtResult.Columns.Count; i++)
                {
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", dtResult.Rows[0][i]);
                }

                sb.Append("</tr>");

                string sCurLoaiKhieuNaiId = string.Empty;
                string sCurLinhVucChungId = string.Empty;
                int index = 1;
                for (int i = 1; i < dtResult.Rows.Count - 1; i++)
                {
                    DataRow row = dtResult.Rows[i];

                    int rowSpanLoaiKhieuNai = 0;
                    int rowSpanLinhVucChung = 0;

                    if (sCurLoaiKhieuNaiId != row["LoaiKhieuNaiId"].ToString())
                    {
                        sCurLoaiKhieuNaiId = row["LoaiKhieuNaiId"].ToString();
                        for (int indexrowSpanLoaiKhieuNai = i; indexrowSpanLoaiKhieuNai < dtResult.Rows.Count; indexrowSpanLoaiKhieuNai++)
                        {
                            if (sCurLoaiKhieuNaiId != dtResult.Rows[indexrowSpanLoaiKhieuNai]["LoaiKhieuNaiId"].ToString())
                            {
                                break;
                            }

                            rowSpanLoaiKhieuNai++;
                        }
                    }

                    if (sCurLinhVucChungId != row["LinhVucChungId"].ToString())
                    {
                        sCurLinhVucChungId = row["LinhVucChungId"].ToString();
                        for (int indexrowSpanLoaiKhieuNai = i; indexrowSpanLoaiKhieuNai < dtResult.Rows.Count; indexrowSpanLoaiKhieuNai++)
                        {
                            if (sCurLinhVucChungId != dtResult.Rows[indexrowSpanLoaiKhieuNai]["LinhVucChungId"].ToString())
                            {
                                break;
                            }

                            rowSpanLinhVucChung++;
                        }
                    }

                    sb.Append("<tr>");
                    //sb.AppendFormat("<td>{0}</td>", i);

                    foreach (DataColumn col in dtResult.Columns)
                    {
                        if (col.ColumnName.ToLower() == "loaikhieunaiid" || col.ColumnName.ToLower() == "linhvucchungid" || col.ColumnName.ToLower() == "linhvucconid")
                        {
                            continue;
                        }

                        if (col.ColumnName.ToLower() == "loaikhieunai" || col.ColumnName.ToLower() == "linhvucchung" || col.ColumnName.ToLower() == "linhvuccon" || col.ColumnName.ToLower() == "sltiepnhan" || col.ColumnName.ToLower() == "sldadong")
                        {
                            if (col.ColumnName.ToLower() == "loaikhieunai")
                            {
                                if (rowSpanLoaiKhieuNai > 0)
                                {
                                    sb.AppendFormat("<td rowspan='{1}' valign='top'>{0}</td>", (index++).ToString(), rowSpanLoaiKhieuNai);
                                    sb.AppendFormat("<td rowspan='{1}' valign='top'>{0}</td>", row[col], rowSpanLoaiKhieuNai);
                                    rowSpanLoaiKhieuNai = 0;
                                }
                            }
                            else if (col.ColumnName.ToLower() == "linhvucchung")
                            {
                                if (rowSpanLinhVucChung > 0)
                                {
                                    sb.AppendFormat("<td rowspan='{1}' valign='top'>{0}</td>", row[col], rowSpanLinhVucChung);
                                    rowSpanLinhVucChung = 0;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("<td>{0}</td>", row[col]);
                            }
                        }
                        else
                        {
                            string nguyenNhanLoiId = string.Empty;
                            string chiTietLoiId = "-1";
                            bool isKN = false;

                            if (col.ColumnName.Contains('_'))
                            {
                                nguyenNhanLoiId = col.ColumnName.Split('_')[0];
                                chiTietLoiId = col.ColumnName.Split('_')[1];

                                for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count; indexLoiKhieuNai++)
                                {
                                    if (listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == nguyenNhanLoiId)
                                    {
                                        if (listLoiKhieuNai[indexLoiKhieuNai].Loai == (int)LoiKhieuNai_Loai.Khiếu_nại)
                                        {
                                            isKN = true;
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count; indexLoiKhieuNai++)
                                {
                                    if (listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == col.ColumnName)
                                    {
                                        if (listLoiKhieuNai[indexLoiKhieuNai].Cap == 1)
                                        {
                                            nguyenNhanLoiId = col.ColumnName;
                                        }
                                        else if (listLoiKhieuNai[indexLoiKhieuNai].Cap == 2)
                                        {
                                            nguyenNhanLoiId = listLoiKhieuNai[indexLoiKhieuNai].ParentId.ToString();
                                            chiTietLoiId = col.ColumnName;

                                            if (listLoiKhieuNai[indexLoiKhieuNai].Loai == (int)LoiKhieuNai_Loai.Khiếu_nại)
                                            {
                                                isKN = true;
                                            }
                                        }
                                    } // end if(listLoiKhieuNai[indexLoiKhieuNai].Id.ToString() == col.ColumnName)
                                } // end for (int indexLoiKhieuNai = 0; indexLoiKhieuNai < listLoiKhieuNai.Count;indexLoiKhieuNai ++ )                                    
                            }

                            if (isKN)
                            {
                                sb.AppendFormat(jsScriptKN, row["LoaiKhieuNaiId"], row["LinhVucChungId"], row["LinhVucConId"], nguyenNhanLoiId, chiTietLoiId, row[col]);

                            }
                            else
                            {
                                sb.AppendFormat(jsScript, row["LoaiKhieuNaiId"], row["LinhVucChungId"], row["LinhVucConId"], nguyenNhanLoiId, chiTietLoiId, row[col]);
                            }
                            //sb.AppendFormat(jsScript, row["LoaiKhieuNaiId"], row["LinhVucChungId"], nguyenNhanLoiId, chiTietLoiId, row[col]);
                        }
                        // sb.AppendFormat("<td>{0}</td>", row[col]); 

                    } // end foreach(DataColumn col in dtResult.Columns)

                    sb.Append("</tr>");

                } // end 

                int lastRowIndex = dtResult.Rows.Count - 1;

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='4'>TỔNG</td>");
                for (int i = 6; i < dtResult.Columns.Count; i++)
                {
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", dtResult.Rows[lastRowIndex][i]);
                }
                sb.Append("</tr>");
            }
            else
            {
                sb.Append(@"<tr>
                                <td colspan='40'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        //        public string BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL(int khuVucId, List<int> listLoaiKhieuNaiId, List<int> listNguyenNhanLoiId, DateTime fromDate, DateTime toDate, bool isExportExcel)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            string formatNumber = string.Empty;
        //            if (!isExportExcel)
        //            {
        //                formatNumber = FORMAT_NUMBER;
        //            }

        //            DataTable dtResult = new ReportImpl().ThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL_Solr(khuVucId, listLoaiKhieuNaiId, listNguyenNhanLoiId, fromDate, toDate);
        //            List<LoiKhieuNaiInfo> listLoiKhieuNai = ServiceFactory.GetInstanceLoiKhieuNai().GetListSortHierarchy();
        //            if(listLoiKhieuNai ==  null)
        //            {
        //                listLoiKhieuNai = new List<LoiKhieuNaiInfo>();
        //            }

        //            if (dtResult != null && dtResult.Rows.Count > 0)
        //            {
        //                string curLoaiKhieuNaiId = "";
        //                string curLinhVucChungId = "";

        //                int totalSLTiepNhan = 0;
        //                int totalSLDaDong = 0;

        //                sb.Append("<tr>");
        //                sb.Append("<th >STT</th>");
        //                sb.Append("<th >Loại khiếu nại</th>");
        //                sb.Append("<th >Lĩnh vực chung</th>");
        //                sb.Append("<th >Lĩnh vực con</th>");
        //                sb.Append("<th >SL tiếp nhận</th>");
        //                sb.Append("<th >SL đã đóng</th>");
        //                //for (int i = 0; i < listLoiKhieuNai.Count;i++ )
        //                //{
        //                //    if(listLoiKhieuNai[i].Cap == 1)
        //                //    {
        //                //        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate(LoiKhieuNaiInfo obj) { return obj.ParentId == listLoiKhieuNai[i].Id; });
        //                //        if(listChild != null && listChild.Count > 1)
        //                //        {
        //                //            sb.AppendFormat("<th colspan='{1}'>{0}</th>", listLoiKhieuNai[i].TenLoi, listChild.Count + 1);
        //                //        }
        //                //        else
        //                //        {
        //                //            sb.AppendFormat("<th rowspan='2'>{0}</th>", listLoiKhieuNai[i].TenLoi);
        //                //        }
        //                //    }
        //                //}

        //                sb.Append("</tr>");

        //                //for (int i = 0; i < listLoiKhieuNai.Count;i++ )
        //                //{
        //                //    if(listLoiKhieuNai[i].Cap == 1)
        //                //    {
        //                //        List<LoiKhieuNaiInfo> listChild = listLoiKhieuNai.FindAll(delegate(LoiKhieuNaiInfo obj) { return obj.ParentId == listLoiKhieuNai[i].Id; });
        //                //        if(listChild != null && listChild.Count > 1)
        //                //        {
        //                //            sb.Append("<th>Tổng</th>");
        //                //            sb.Append("<th>Khác</th>");
        //                //        }                        
        //                //    }
        //                //    else
        //                //    {
        //                //        sb.AppendFormat("<th>{0}</th>", listLoiKhieuNai[i].MaLoi);
        //                //    }
        //                //}

        //                sb.Append("<tr>");
        //                sb.Append("<td align='center' class='borderThinTextBold' colspan='4'>TỔNG</td>");
        //                sb.Append("<td align='center' class='borderThinTextBold'>TONG_SO_TIEP_NHAN</td>");
        //                sb.Append("<td align='center' class='borderThinTextBold'>TONG_SO_DA_DONG</td>");               

        //                sb.Append("</tr>");

        //                for (int i = 0; i < dtResult.Rows.Count; i++)
        //                {
        //                    int rowSpanLoaiKhieuNai = 0;
        //                    int rowSpanLinhVucChung = 0;
        //                    DataRow row = dtResult.Rows[i];

        //                    // Tính số lượng rowspan của loại khiếu nại và lĩnh vực chung
        //                    if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
        //                    {
        //                        curLoaiKhieuNaiId = row["LoaiKhieuNaiId"].ToString();
        //                        curLinhVucChungId = row["LinhVucChungId"].ToString();

        //                        for (int j = i; j < dtResult.Rows.Count; j++)
        //                        {
        //                            DataRow rowLoaiKhieuNai = dtResult.Rows[j];
        //                            if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId)
        //                            {
        //                                rowSpanLoaiKhieuNai++;
        //                            }
        //                            else
        //                            {
        //                                break;
        //                            }
        //                        } // end  for(int j=i;j<dtResult.Rows.Count;j++) 

        //                        for (int j = i; j < dtResult.Rows.Count; j++)
        //                        {
        //                            DataRow rowLoaiKhieuNai = dtResult.Rows[j];
        //                            if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
        //                            {
        //                                rowSpanLinhVucChung++;
        //                            }
        //                            else
        //                            {
        //                                break;
        //                            }
        //                        } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
        //                    } // end if (row["LoaiKhieuNaiId"].ToString() != curLoaiKhieuNaiId)
        //                    else
        //                    {
        //                        if (row["LinhVucChungId"].ToString() != curLinhVucChungId)
        //                        {
        //                            curLinhVucChungId = row["LinhVucChungId"].ToString();
        //                            for (int j = i; j < dtResult.Rows.Count; j++)
        //                            {
        //                                DataRow rowLoaiKhieuNai = dtResult.Rows[j];
        //                                if (rowLoaiKhieuNai["LoaiKhieuNaiId"].ToString() == curLoaiKhieuNaiId && rowLoaiKhieuNai["LinhVucChungId"].ToString() == curLinhVucChungId)
        //                                {
        //                                    rowSpanLinhVucChung++;
        //                                }
        //                                else
        //                                {
        //                                    break;
        //                                }
        //                            } // end  for(int j=i;j<dtResult.Rows.Count;j++) 
        //                        }
        //                    } // end else

        //                    int soLuongTiepNhan = ConvertUtility.ToInt32(row["SLTiepNhan"], 0);
        //                    int soLuongDaDong = ConvertUtility.ToInt32(row["SLDaDong"], 0);

        //                    string sRowSpanLoaiKhieuNai = rowSpanLoaiKhieuNai > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLoaiKhieuNai.ToString() + "\"" : "";
        //                    string sRowSpanLinhVucChung = rowSpanLinhVucChung > 0 ? "valign=\"top\" rowspan=\"" + rowSpanLinhVucChung.ToString() + "\"" : "";

        //                    sb.Append("<tr>");
        //                    sb.Append("<td class='borderThin' align='center'>" + (i + 1).ToString() + "</td>");

        //                    if (sRowSpanLoaiKhieuNai.Length > 0)
        //                    {
        //                        sb.Append("<td class='borderThin'" + sRowSpanLoaiKhieuNai + ">" + row["LoaiKhieuNai"].ToString() + "</td>");
        //                    }

        //                    if (sRowSpanLinhVucChung.Length > 0)
        //                    {
        //                        sb.Append("<td align='center' class='borderThin'" + sRowSpanLinhVucChung + ">" + row["LinhVucChung"].ToString() + "</td>");
        //                    }

        //                    sb.Append("<td align='center' class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
        //                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongdichvutoanmang&khuVucId=" + khuVucId + "&loaiKhieuNaiId=" + row["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + row["LinhVucChungId"].ToString() + "&LinhVucConId=" + row["LinhVucConId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongTiepNhan.ToString(formatNumber) + "</a></td>");
        //                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongdichVutoanmang&khuVucId=" + khuVucId + "&loaiKhieuNaiId=" + row["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + row["LinhVucChungId"].ToString() + "&LinhVucConId=" + row["LinhVucConId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + soLuongDaDong.ToString(formatNumber) + "</a></td>");

        //                    foreach(DataColumn column in dtResult.Columns)
        //                    {
        //                        sb.AppendFormat("");
        //                    }

        //                    sb.Append("</tr>");

        //                    totalSLTiepNhan += soLuongTiepNhan;
        //                    totalSLDaDong += soLuongDaDong;
        //                } // end 

        //                sb.Append("<tr>");
        //                sb.Append("<td align='center' class='borderThinTextBold' colspan='4'>TỔNG</td>");
        //                sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLTiepNhan.ToString(formatNumber) + "</td>");
        //                sb.Append("<td align='center' class='borderThinTextBold'>" + totalSLDaDong.ToString(formatNumber) + "</td>");
        //                sb.Append("</tr>");

        //                sb.Replace("TONG_SO_TIEP_NHAN", totalSLTiepNhan.ToString(formatNumber));
        //                sb.Replace("TONG_SO_DA_DONG", totalSLDaDong.ToString(formatNumber));
        //            }
        //            else
        //            {
        //                sb.Append(@"<tr>
        //                                <td colspan='6'>
        //                                    Chưa có dữ liệu báo cáo
        //                                </td>
        //                            </tr>");
        //            }

        //            return sb.ToString();
        //        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/06/2015
        /// Todo : Báo cáo tổng hợp chất lượng phục vụ
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="khenChe"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopChatLuongPhucVu(int doiTacId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, DateTime fromDate, DateTime toDate, int nguonKhieuNai)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbToanMang = new StringBuilder();
            StringBuilder sbKhuVuc = new StringBuilder();
            StringBuilder sbDoiTac = new StringBuilder();

            int typeKhen = 1;
            int typeChe = 2;

            DataSet dsResult = new ReportImpl().BaoCaoTongHopChatLuongPhucVu_Solr(doiTacId, loaiKhieuNaiId, linhVucChungId, linhVucConId, fromDate, toDate, nguonKhieuNai);
            string cellToanMang = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongphucvu&doiTacId=" + doiTacId + "&typeKhenChe={1}&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={2}&linhVucChungId=" + linhVucChungId + "&linhVucConId=" + linhVucConId + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
            string cellKhuVuc = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongphucvu&doiTacId={1}&typeKhenChe={2}&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={3}&linhVucChungId=" + linhVucChungId + "&linhVucConId=" + linhVucConId + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
            string cellDoiTac = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongphucvu&doiTacId={1}&typeKhenChe={2}&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={3}&linhVucChungId=" + linhVucChungId + "&linhVucConId=" + linhVucConId + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                sbToanMang.Append("<div><b>Tổng hợp toàn mạng</b></div>");
                sbToanMang.Append("<table class=\"tbl_style\" border=\"1\" style=\"border-collapse: collapse;\">");
                sbToanMang.Append("<tr>");
                sbToanMang.Append("<th rowspan='2'>STT</th>");
                sbToanMang.Append("<th rowspan='2'>Đối tượng</th>");
                sbToanMang.Append("<th colspan='2'>Tổng giao dịch</th>");
                sbToanMang.Append("</tr>");
                sbToanMang.Append("<tr>");
                sbToanMang.Append("<th>Khen</th>");
                sbToanMang.Append("<th>Chê</th>");
                sbToanMang.Append("</tr>");
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    sbToanMang.Append("<tr>");
                    sbToanMang.AppendFormat("<td>{0}</td>", (i + 1).ToString());
                    sbToanMang.AppendFormat("<td>{0}</td>", dsResult.Tables[0].Rows[i]["LoaiKhieuNai"]);
                    sbToanMang.AppendFormat(cellToanMang, dsResult.Tables[0].Rows[i]["Khen"], typeKhen, dsResult.Tables[0].Rows[i]["LoaiKhieuNaiId"]);
                    sbToanMang.AppendFormat(cellToanMang, dsResult.Tables[0].Rows[i]["Che"], typeChe, dsResult.Tables[0].Rows[i]["LoaiKhieuNaiId"]);
                    sbToanMang.Append("</tr>");
                }

                sbToanMang.Append("</table>");
                sbToanMang.Append("<br/>");
                sbToanMang.Append("<br/>");

                sbKhuVuc.Append("<div><b>Tổng hợp theo khu vực</b></div>");
                sbKhuVuc.Append("<table class=\"tbl_style\" border=\"1\" style=\"border-collapse: collapse;\">");
                sbKhuVuc.Append("<tr>");
                sbKhuVuc.Append("<th rowspan='3'>STT</th>");
                sbKhuVuc.Append("<th rowspan='3'>Đối tượng</th>");
                sbKhuVuc.Append("<th colspan='6'>Tổng giao dịch</th>");
                sbKhuVuc.Append("</tr>");
                sbKhuVuc.Append("<tr>");
                sbKhuVuc.Append("<th colspan='3'>Khen</th>");
                sbKhuVuc.Append("<th colspan='3'>Chê</th>");
                sbKhuVuc.Append("</tr>");
                sbKhuVuc.Append("<tr>");
                sbKhuVuc.Append("<th>KV1</th>");
                sbKhuVuc.Append("<th>KV2</th>");
                sbKhuVuc.Append("<th>KV3</th>");
                sbKhuVuc.Append("<th>KV1</th>");
                sbKhuVuc.Append("<th>KV2</th>");
                sbKhuVuc.Append("<th>KV3</th>");
                sbKhuVuc.Append("</tr>");
                for (int i = 0; i < dsResult.Tables[1].Rows.Count; i++)
                {
                    sbKhuVuc.Append("<tr>");
                    sbKhuVuc.Append("<tr>");
                    sbKhuVuc.AppendFormat("<td>{0}</td>", (i + 1).ToString());
                    sbKhuVuc.AppendFormat("<td>{0}</td>", dsResult.Tables[1].Rows[i]["LoaiKhieuNai"]);
                    sbKhuVuc.AppendFormat(cellKhuVuc, dsResult.Tables[1].Rows[i]["Khen_2"], 2, typeKhen, dsResult.Tables[1].Rows[i]["LoaiKhieuNaiId"]);
                    sbKhuVuc.AppendFormat(cellKhuVuc, dsResult.Tables[1].Rows[i]["Khen_3"], 3, typeKhen, dsResult.Tables[1].Rows[i]["LoaiKhieuNaiId"]);
                    sbKhuVuc.AppendFormat(cellKhuVuc, dsResult.Tables[1].Rows[i]["Khen_5"], 5, typeKhen, dsResult.Tables[1].Rows[i]["LoaiKhieuNaiId"]);
                    sbKhuVuc.AppendFormat(cellKhuVuc, dsResult.Tables[1].Rows[i]["Che_2"], 2, typeChe, dsResult.Tables[1].Rows[i]["LoaiKhieuNaiId"]);
                    sbKhuVuc.AppendFormat(cellKhuVuc, dsResult.Tables[1].Rows[i]["Che_3"], 3, typeChe, dsResult.Tables[1].Rows[i]["LoaiKhieuNaiId"]);
                    sbKhuVuc.AppendFormat(cellKhuVuc, dsResult.Tables[1].Rows[i]["Che_5"], 5, typeChe, dsResult.Tables[1].Rows[i]["LoaiKhieuNaiId"]);
                    sbKhuVuc.Append("</tr>");
                }

                sbKhuVuc.Append("</table>");
                sbKhuVuc.Append("<br/>");
                sbKhuVuc.Append("<br/>");

                sbDoiTac.Append("<div><b>Tổng hợp đối tác</b></div>");
                sbDoiTac.Append("<table class=\"tbl_style\" border=\"1\" style=\"border-collapse: collapse;\">");
                sbDoiTac.Append("<tr>");
                sbDoiTac.Append("<th rowspan='2'>STT</th>");
                sbDoiTac.Append("<th rowspan='2'>Đối tác</th>");
                sbDoiTac.Append("<th rowspan='2'>Đối tượng</th>");
                sbDoiTac.Append("<th colspan='2'>Tổng giao dịch</th>");
                sbDoiTac.Append("</tr>");
                sbDoiTac.Append("<tr>");
                sbDoiTac.Append("<th>Khen</th>");
                sbDoiTac.Append("<th>Chê</th>");
                sbDoiTac.Append("</tr>");

                string curDoiTacId = string.Empty;
                int indexSTT = 1;
                for (int i = 0; i < dsResult.Tables[2].Rows.Count; i++)
                {
                    sbDoiTac.Append("<tr>");

                    if (curDoiTacId != dsResult.Tables[2].Rows[i]["DoiTacId"].ToString())
                    {
                        int rowspan = 0;
                        curDoiTacId = dsResult.Tables[2].Rows[i]["DoiTacId"].ToString();
                        for (int j = i; j < dsResult.Tables[2].Rows.Count; j++)
                        {
                            if (dsResult.Tables[2].Rows[j]["DoiTacId"].ToString() == curDoiTacId)
                            {
                                rowspan++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        sbDoiTac.AppendFormat("<td rowspan='{1}'>{0}</td>", indexSTT++, rowspan);
                        sbDoiTac.AppendFormat("<td rowspan='{1}'>{0}</td>", dsResult.Tables[2].Rows[i]["DoiTac"], rowspan);
                        //sbDoiTac.AppendFormat("<td rowspan='{1}'>{0}</td>", dsResult.Tables[2].Rows[i]["LoaiKhieuNai"], rowspan);
                    }

                    sbDoiTac.AppendFormat("<td>{0}</td>", dsResult.Tables[2].Rows[i]["LoaiKhieuNai"]);
                    sbDoiTac.AppendFormat(cellDoiTac, dsResult.Tables[2].Rows[i]["Khen"], dsResult.Tables[2].Rows[i]["DoiTacId"], typeKhen, dsResult.Tables[2].Rows[i]["LoaiKhieuNaiId"]);
                    sbDoiTac.AppendFormat(cellDoiTac, dsResult.Tables[2].Rows[i]["Che"], dsResult.Tables[2].Rows[i]["DoiTacId"], typeChe, dsResult.Tables[2].Rows[i]["LoaiKhieuNaiId"]);

                    sbDoiTac.Append("</tr>");
                }

                sbDoiTac.Append("</table>");
            }

            sb.Append(sbToanMang.ToString());
            sb.Append(sbKhuVuc.ToString());
            sb.Append(sbDoiTac.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/06/2015
        /// Todo : Báo cáo chi tiết chất lượng phục vụ
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoChiTietChatLuongPhucVu(int doiTacId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportImpl().BaoCaoChiTietChatLuongPhucVu(doiTacId, loaiKhieuNaiId, linhVucChungId, linhVucConId, nguonKhieuNai, fromDate, toDate);
            if (listKhieuNaiReportInfo != null)
            {
                List<DoiTacInfo> listDoiTacInfo = new DoiTacImpl().GetList();
                int totalChe1 = 0;
                int totalChe2 = 0;
                int totalChe3 = 0;
                int totalChe4 = 0;

                string showPopupChiTietKhieuNai = "<td valign='top'><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                string colKhen1 = "Thái độ phục vụ tốt (nhiệt tình, niềm nở, nhẹ nhàng)";
                string colKhen2 = "Nhanh chóng  ( Hiểu ý  KH, giải quyết vấn đề nhanh, cung cấp thông tin nhannh,…)";
                string colKhen3 = "Hiểu biết, giải quyết được vấn đề cho KH)";
                string colKhen4 = "Nội dung khác";
                string colChe1 = "Thái độ phục vụ không tốt (cáu gắt, không thân thiện, không nhiệt tình)";
                string colChe2 = "Chậm trễ (tư vẫn chậm, tra cứu chậm, hiểu ý KH chậm,…)";
                string colChe3 = "Nghiệp vụ kém, không giải quyết được yêu cầu của KH";
                string colChe4 = "Nội dung khác";

                sb.Append("<table class=\"tbl_style\" border=\"1\" style=\"border-collapse: collapse;\">");
                sb.AppendFormat("<tr><td colspan='15'>Tổng số bản ghi : {0}</td></tr>", listKhieuNaiReportInfo.Count);
                sb.AppendFormat("<tr><td colspan='15'>{0} : [Che1]</td></tr>", colChe1);
                sb.AppendFormat("<tr><td colspan='15'>{0} : [Che2]</td></tr>", colChe2);
                sb.AppendFormat("<tr><td colspan='15'>{0} : [Che3]</td></tr>", colChe3);
                sb.AppendFormat("<tr><td colspan='15'>{0} : [Che4]</td></tr>", colChe4);
                sb.Append("<tr>");
                sb.Append("<th rowspan='3'>STT</th>");
                sb.Append("<th rowspan='3'>Mã phản ánh</th>");
                sb.Append("<th rowspan='3'>Số thuê bao</th>");
                sb.Append("<th colspan='8'>Lĩnh vực con</th>");
                sb.Append("<th rowspan='3'>Lĩnh vực chung</th>");
                sb.Append("<th rowspan='3'>Đơn vị tiếp nhận</th>");
                sb.Append("<th rowspan='3'>Người tiếp nhận</th>");
                sb.Append("<th rowspan='3'>Ngày tiếp nhận</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th colspan='4'>Khen</th>");
                sb.Append("<th colspan='4'>Chê</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.AppendFormat("<th>{0}</th>", colKhen1);
                sb.AppendFormat("<th>{0}</th>", colKhen2);
                sb.AppendFormat("<th>{0}</th>", colKhen3);
                sb.AppendFormat("<th>{0}</th>", colKhen4);
                sb.AppendFormat("<th>{0}</th>", colChe1);
                sb.AppendFormat("<th>{0}</th>", colChe2);
                sb.AppendFormat("<th>{0}</th>", colChe3);
                sb.AppendFormat("<th>{0}</th>", colChe4);
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td valign='top'>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiReportInfo[i].Id);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon == colKhen1 ? "x" : "&nbsp;");
                    sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon == colKhen2 ? "x" : "&nbsp;");
                    sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon == colKhen3 ? "x" : "&nbsp;");
                    sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucChung.ToLower().Contains("khen") && listKhieuNaiReportInfo[i].LinhVucCon == colKhen4 ? "x" : "&nbsp;");

                    if (listKhieuNaiReportInfo[i].LinhVucCon == colChe1)
                    {
                        totalChe1++;
                        sb.Append("<td valign='top' align='center'>x</td>");
                    }
                    else
                    {
                        sb.Append("<td valign='top' align='center'>&nbsp;</td>");
                    }

                    if (listKhieuNaiReportInfo[i].LinhVucCon == colChe2)
                    {
                        totalChe2++;
                        sb.Append("<td valign='top' align='center'>x</td>");
                    }
                    else
                    {
                        sb.Append("<td valign='top' align='center'>&nbsp;</td>");
                    }

                    if (listKhieuNaiReportInfo[i].LinhVucCon == colChe3)
                    {
                        totalChe3++;
                        sb.Append("<td valign='top' align='center'>x</td>");
                    }
                    else
                    {
                        sb.Append("<td valign='top' align='center'>&nbsp;</td>");
                    }

                    if (listKhieuNaiReportInfo[i].LinhVucChung.ToLower().Contains("chê") && listKhieuNaiReportInfo[i].LinhVucCon == colChe4)
                    {
                        totalChe4++;
                        sb.Append("<td valign='top' align='center'>x</td>");
                    }
                    else
                    {
                        sb.Append("<td valign='top' align='center'>&nbsp;</td>");
                    }

                    //sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon == colChe1 ? "x" : "&nbsp;");
                    //sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon == colChe2 ? "x" : "&nbsp;");
                    //sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon == colChe3 ? "x" : "&nbsp;");
                    //sb.AppendFormat("<td valign='top' align='center'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucChung.ToLower().Contains("chê") && listKhieuNaiReportInfo[i].LinhVucCon == colChe4 ? "x" : "&nbsp;");
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucChung);
                    sb.AppendFormat("<td valign='top'>{0}</td>", this.GetTenDoiTac(listDoiTacInfo, listKhieuNaiReportInfo[i].DoiTacId));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NguoiTiepNhan);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));

                    sb.Append("</tr>");
                }

                sb.Append("</table>");

                if (listKhieuNaiReportInfo.Count > 0)
                {
                    int total = listKhieuNaiReportInfo.Count;
                    string sChe1 = string.Format("{0} ({1}%)", totalChe1, (totalChe1 * 100 / total));
                    string sChe2 = string.Format("{0} ({1}%)", totalChe2, (totalChe2 * 100 / total));
                    string sChe3 = string.Format("{0} ({1}%)", totalChe3, (totalChe3 * 100 / total));
                    string sChe4 = string.Format("{0} ({1}%)", totalChe4, (totalChe4 * 100 / total));
                    sb = sb.Replace("[Che1]", sChe1);
                    sb = sb.Replace("[Che2]", sChe2);
                    sb = sb.Replace("[Che3]", sChe3);
                    sb = sb.Replace("[Che4]", sChe4);
                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 13/07/2015
        /// Todo : Hiển thị nội dung báo cáo tổng hợp chất lượng mạng
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopChatLuongMang_old(int khuVucId, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append("<th rowspan='2'>STT</th>");
            sb.Append("<th rowspan='2'>Tên tỉnh/thành phố</th>");
            sb.Append("<th rowspan='2'>Tổng</th>");
            sb.Append("<th rowspan='2'>Không xác định</th>");
            sb.Append("<th colspan='7'>2G</th>");
            sb.Append("<th colspan='7'>3G</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th>Không xác định</th>");
            sb.Append("<th>Sóng kém hoặc chập chờn (cả indoor và outdoor)</th>");
            sb.Append("<th>Có sóng nhưng gọi đi hoặc gọi đến không được</th>");
            sb.Append("<th>Đang đàm thoại rớt cuộc ( mất tín hiệu, báo gián đoạn..)</th>");
            sb.Append("<th>Cuộc gọi nhiễu,nghe xen, tiếng vọng</th>");
            sb.Append("<th>Không có sóng Indoor</th>");
            sb.Append("<th>Mất sóng hoàn toàn</th>");
            sb.Append("<th>Không xác định</th>");
            sb.Append("<th>Sóng kém hoặc chập chờn (cả indoor và outdoor)</th>");
            sb.Append("<th>Có sóng nhưng gọi đi hoặc gọi đến không được</th>");
            sb.Append("<th>Đang đàm thoại rớt cuộc ( mất tín hiệu, báo gián đoạn..)</th>");
            sb.Append("<th>Cuộc gọi nhiễu,nghe xen, tiếng vọng</th>");
            sb.Append("<th>Không có sóng Indoor</th>");
            sb.Append("<th>Mất sóng hoàn toàn</th>");
            sb.Append("</tr>");

            DataTable dtResult = new ReportImpl().BaoCaoTongHopChatLuongMang_Solr(khuVucId, nguonKhieuNai, fromDate, toDate);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string cellContent = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongmang&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&maTinhId={4}&khuVucId={5}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
                string cellContentBold = "<td align='center' class='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongmang&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&maTinhId={4}&khuVucId={5}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";

                int totalKhuVuc = 0;
                int indexSTT = 0;
                string curKhuVucId = "0";

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow row = dtResult.Rows[i];

                    if (curKhuVucId != row["KhuVucId"].ToString())
                    {
                        curKhuVucId = row["KhuVucId"].ToString();
                        string tenKhuVuc = string.Empty;
                        if (curKhuVucId == DoiTacInfo.DoiTacIdValue.VNP1.ToString())
                        {
                            tenKhuVuc = "VNP1";
                        }
                        else if (curKhuVucId == DoiTacInfo.DoiTacIdValue.VNP2.ToString())
                        {
                            tenKhuVuc = "VNP2";
                        }
                        else if (curKhuVucId == DoiTacInfo.DoiTacIdValue.VNP3.ToString())
                        {
                            tenKhuVuc = "VNP3";
                        }

                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='2' class='borderThinTextBold'>{0}</td>", tenKhuVuc);

                        for (int indexColumn = 3; indexColumn < dtResult.Columns.Count; indexColumn++)
                        {
                            int totalColumn = 0;
                            for (int k = i; k < dtResult.Rows.Count; k++)
                            {
                                if (dtResult.Rows[k]["KhuVucId"].ToString() == curKhuVucId)
                                {
                                    totalColumn += ConvertUtility.ToInt32(dtResult.Rows[k][indexColumn], 0);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (dtResult.Columns[indexColumn].ColumnName == "TongKN")
                            {
                                sb.AppendFormat(cellContentBold, totalColumn, 71, -1, -1, -1, curKhuVucId);
                            }
                            else
                            {
                                string[] arrColName = dtResult.Columns[indexColumn].ColumnName.Split('_');
                                if (arrColName[0] == "71" && arrColName[1] == "0") // 71_0 : Chỉ có loại khiếu nại
                                {
                                    sb.AppendFormat(cellContentBold, totalColumn, 71, 0, 0, -1, curKhuVucId);
                                }
                                else if (arrColName[1] == "0") // 72_0, 80_0 : Chỉ có lĩnh vực chung
                                {
                                    sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], 0, -1, curKhuVucId);
                                }
                                else
                                {
                                    sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], arrColName[1], -1, curKhuVucId);
                                }
                            }
                        } // end for (int indexColumn = 3; indexColumn < dtResult.Columns.Count; indexColumn++)

                        sb.Append("</tr>");

                        indexSTT = 1;
                        i--;
                    }
                    else
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", indexSTT);
                        sb.AppendFormat("<td>{0}</td>", row["TenTinh"]);
                        sb.AppendFormat(cellContent, row["TongKN"], 71, -1, -1, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["71_0"], 71, 0, 0, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["72_0"], 71, 72, 0, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["72_73"], 71, 72, 73, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["72_74"], 71, 72, 74, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["72_75"], 71, 72, 75, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["72_76"], 71, 72, 76, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["72_77"], 71, 72, 77, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["72_1135"], 71, 72, 1135, row["MaTinh"], row["KhuVucId"]);

                        sb.AppendFormat(cellContent, row["80_0"], 71, 80, 0, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["80_883"], 71, 80, 883, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["80_884"], 71, 80, 884, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["80_84"], 71, 80, 84, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["80_85"], 71, 80, 85, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["80_86"], 71, 80, 86, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["80_87"], 71, 80, 87, row["MaTinh"], row["KhuVucId"]);
                        sb.Append("</tr>");
                        indexSTT++;
                    }
                }

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='2' class='borderThinTextBold'>Tổng toàn mạng</td>");
                for (int indexColumn = 3; indexColumn < dtResult.Columns.Count; indexColumn++)
                {
                    int totalColumn = 0;
                    for (int k = 0; k < dtResult.Rows.Count; k++)
                    {
                        totalColumn += ConvertUtility.ToInt32(dtResult.Rows[k][indexColumn], 0);
                    }

                    //sb.AppendFormat("<td   align='center' class='borderThinTextBold'>{0}</td>", totalColumn);
                    if (dtResult.Columns[indexColumn].ColumnName == "TongKN")
                    {
                        sb.AppendFormat(cellContentBold, totalColumn, 71, -1, -1, -1, -1);
                    }
                    else
                    {
                        string[] arrColName = dtResult.Columns[indexColumn].ColumnName.Split('_');
                        if (arrColName[0] == "71" && arrColName[1] == "0") // 71_0 : Chỉ có loại khiếu nại
                        {
                            sb.AppendFormat(cellContentBold, totalColumn, 71, 0, 0, -1, -1);
                        }
                        else if (arrColName[1] == "0") // 72_0, 80_0 : Chỉ có lĩnh vực chung
                        {
                            sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], 0, -1, -1);
                        }
                        else
                        {
                            sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], arrColName[1], -1, -1);
                        }
                    }
                }

                sb.Append("</tr>");
            }
            else
            {
                sb.AppendFormat("<tr><td colspan='17'>Không có bản ghi nào</td></tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Vu Van Truong
        /// Created date : 13/04/2016
        /// Todo : Hiển thị nội dung báo cáo tổng hợp chất lượng mạng
        /// (Bổ sung Cột Số lượng tiếp nhận + Số lượng Tồn + Số quá hạn (hiện nay chỉ có số lượng giải quyết) theo từng Lĩnh vực (VD: Mất sóng hoàn toàn).
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopChatLuongMang(int khuVucId, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append("<th rowspan='2'>STT</th>");
            sb.Append("<th rowspan='2'>Tên tỉnh/thành phố</th>");
            sb.Append("<th rowspan='2'>Tổng</th>");
            sb.Append("<th rowspan='2'>Không xác định</th>");
            sb.Append("<th colspan='28'>2G</th>");
            sb.Append("<th colspan='28'>3G</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th>[2.1]</th>");   // Không xác định
            sb.Append("<th>[2.1.1]</th>"); // Số lượng tiếp nhận
            sb.Append("<th>[2.1.2]</th>"); // Số lượng tồn
            sb.Append("<th>[2.1.3]</th>"); // Số lượng quá hạn
            sb.Append("<th>[2.2]</th>");   // Sóng kém hoặc chập chờn (cả indoor và outdoor)
            sb.Append("<th>[2.2.1]</th>");
            sb.Append("<th>[2.2.2]</th>");
            sb.Append("<th>[2.2.3]</th>");
            sb.Append("<th>[2.3]</th>");   // Có sóng nhưng gọi đi hoặc gọi đến không được
            sb.Append("<th>[2.3.1]</th>");
            sb.Append("<th>[2.3.2]</th>");
            sb.Append("<th>[2.3.3]</th>");
            sb.Append("<th>[2.4]</th>");   // Đang đàm thoại rớt cuộc ( mất tín hiệu, báo gián đoạn..)
            sb.Append("<th>[2.4.1]</th>");
            sb.Append("<th>[2.4.2]</th>");
            sb.Append("<th>[2.4.3]</th>");
            sb.Append("<th>[2.5]</th>");  // Cuộc gọi nhiễu,nghe xen, tiếng vọng
            sb.Append("<th>[2.5.1]</th>");
            sb.Append("<th>[2.5.2]</th>");
            sb.Append("<th>[2.5.3]</th>");
            sb.Append("<th>[2.6]</th>");  // Không có sóng Indoor
            sb.Append("<th>[2.6.1]</th>");
            sb.Append("<th>[2.6.2]</th>");
            sb.Append("<th>[2.6.3]</th>");
            sb.Append("<th>[2.7]</th>");  // Mất sóng hoàn toàn
            sb.Append("<th>[2.7.1]</th>");
            sb.Append("<th>[2.7.2]</th>");
            sb.Append("<th>[2.7.3]</th>");

            sb.Append("<th>[3.1]</th>");
            sb.Append("<th>[3.1.1]</th>");
            sb.Append("<th>[3.1.2]</th>");
            sb.Append("<th>[3.1.3]</th>");
            sb.Append("<th>[3.2]</th>");
            sb.Append("<th>[3.2.1]</th>");
            sb.Append("<th>[3.2.2]</th>");
            sb.Append("<th>[3.2.3]</th>");
            sb.Append("<th>[3.3]</th>");
            sb.Append("<th>[3.3.1]</th>");
            sb.Append("<th>[3.3.2]</th>");
            sb.Append("<th>[3.3.3]</th>");
            sb.Append("<th>[3.4]</th>");
            sb.Append("<th>[3.4.1]</th>");
            sb.Append("<th>[3.4.2]</th>");
            sb.Append("<th>[3.4.3]</th>");
            sb.Append("<th>[3.5]</th>");
            sb.Append("<th>[3.5.1]</th>");
            sb.Append("<th>[3.5.2]</th>");
            sb.Append("<th>[3.5.3]</th>");
            sb.Append("<th>[3.6]</th>");
            sb.Append("<th>[3.6.1]</th>");
            sb.Append("<th>[3.6.2]</th>");
            sb.Append("<th>[3.6.3]</th>");
            sb.Append("<th>[3.7]</th>");
            sb.Append("<th>[3.7.1]</th>");
            sb.Append("<th>[3.7.2]</th>");
            sb.Append("<th>[3.7.3]</th>");
            sb.Append("</tr>");

            DataTable dtResult = new ReportImpl().BaoCaoTongHopChatLuongMang_Solr(khuVucId, nguonKhieuNai, fromDate, toDate);

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string cellContent = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongmang&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&maTinhId={4}&khuVucId={5}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
                string cellContentBold = "<td align='center' class='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongmang&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&nguonKhieuNai=" + nguonKhieuNai + "&loaiKhieuNaiId={1}&linhVucChungId={2}&linhVucConId={3}&maTinhId={4}&khuVucId={5}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
                //string cellContentActivity = "<td align='center' class='borderThin'><a href='#' onclick=\"GetListKhieuNaiId('{1}');window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghopchatluongmangactivity&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&whereClause={1}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
                string cellContentActivity = "<td align='center' class='borderThin'><a href='#' onclick=\"GetListKhieuNaiId('{1}'); return false;\">{0}</a></td>";

                int totalKhuVuc = 0;
                int indexSTT = 0;
                string curKhuVucId = "0";

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow row = dtResult.Rows[i];

                    if (curKhuVucId != row["KhuVucId"].ToString())
                    {
                        curKhuVucId = row["KhuVucId"].ToString();
                        string tenKhuVuc = string.Empty;
                        if (curKhuVucId == DoiTacInfo.DoiTacIdValue.VNP1.ToString())
                        {
                            tenKhuVuc = "VNP1";
                        }
                        else if (curKhuVucId == DoiTacInfo.DoiTacIdValue.VNP2.ToString())
                        {
                            tenKhuVuc = "VNP2";
                        }
                        else if (curKhuVucId == DoiTacInfo.DoiTacIdValue.VNP3.ToString())
                        {
                            tenKhuVuc = "VNP3";
                        }

                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='2' class='borderThinTextBold'>{0}</td>", tenKhuVuc);

                        for (int indexColumn = 3; indexColumn < dtResult.Columns.Count; indexColumn++)
                        {
                            int totalColumn = 0;
                            string listAllKhieuNaiId = "";
                            for (int k = i; k < dtResult.Rows.Count; k++)
                            {
                                if (dtResult.Rows[k]["KhuVucId"].ToString() == curKhuVucId)
                                {
                                    if (Convert.ToString(dtResult.Rows[k][indexColumn]).Contains("WHERE:"))
                                    {
                                        var tongTrong = dtResult.Rows[k][indexColumn].ToString().Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                                        totalColumn += ConvertUtility.ToInt32(tongTrong[0], 0);
                                        listAllKhieuNaiId += tongTrong[1] + " ";
                                    }
                                    else
                                    {
                                        totalColumn += ConvertUtility.ToInt32(dtResult.Rows[k][indexColumn], 0);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (dtResult.Columns[indexColumn].ColumnName == "TongKN")
                            {
                                sb.AppendFormat(cellContentBold, totalColumn, 71, -1, -1, -1, curKhuVucId);
                            }
                            else
                            {
                                string[] arrColName = dtResult.Columns[indexColumn].ColumnName.Split('_');
                                if (arrColName[0] == "71" && arrColName[1] == "0" && arrColName.Length == 2) // 71_0 : Chỉ có loại khiếu nại
                                {
                                    sb.AppendFormat(cellContentBold, totalColumn, 71, 0, 0, -1, curKhuVucId);
                                }
                                else if (arrColName[1] == "0" && arrColName.Length == 2) // 72_0, 80_0 : Chỉ có lĩnh vực chung
                                {
                                    sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], 0, -1, curKhuVucId);
                                }
                                else
                                {
                                    sb.AppendFormat(cellContentActivity, totalColumn, listAllKhieuNaiId);
                                    //sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], arrColName[1], -1, curKhuVucId);
                                }
                            }
                        } // end for (int indexColumn = 3; indexColumn < dtResult.Columns.Count; indexColumn++)

                        sb.Append("</tr>");

                        indexSTT = 1;
                        i--;
                    }
                    else
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", indexSTT);
                        sb.AppendFormat("<td>{0}</td>", row["TenTinh"]);
                        sb.AppendFormat(cellContent, row["TongKN"], 71, -1, -1, row["MaTinh"], row["KhuVucId"]);
                        sb.AppendFormat(cellContent, row["71_0"], 71, 0, 0, row["MaTinh"], row["KhuVucId"]);

                        // Sóng 2G
                        // tổng số - loại kn - lĩnh vực chung - lĩnh vực con - mã tỉnh - khu vực

                        // Không xác định
                        sb.AppendFormat(cellContent, row["72_0"], 71, 72, 0, row["MaTinh"], row["KhuVucId"]);
                        string[] tiepnhan = null;
                        if (dtResult.Columns.Contains("72_0_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_0_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                            else
                            {
                                sb.AppendFormat(cellContentActivity, null, null);
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_0_TON"))
                        {
                            tiepnhan = Convert.ToString(row["72_0_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                            else
                            {
                                sb.AppendFormat(cellContentActivity, null, null);
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_0_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["72_0_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                            else
                            {
                                sb.AppendFormat(cellContentActivity, null, null);
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }


                        // Sóng kém hoặc chập chờn (cả indoor và outdoor)
                        sb.AppendFormat(cellContent, row["72_73"], 71, 72, 73, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("72_73_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_73_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                            else
                            {
                                sb.AppendFormat(cellContentActivity, null, null);
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_73_TON"))
                        {
                            tiepnhan = Convert.ToString(row["72_73_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                            else
                            {
                                sb.AppendFormat(cellContentActivity, null, null);
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_73_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["72_73_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Có sóng nhưng gọi đi hoặc gọi đến không được
                        sb.AppendFormat(cellContent, row["72_74"], 71, 72, 74, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("72_74_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_74_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_74_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_74_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_74_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_74_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Đang đàm thoại rớt cuộc ( mất tín hiệu, báo gián đoạn..)
                        sb.AppendFormat(cellContent, row["72_75"], 71, 72, 75, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("72_75_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_75_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_75_TON"))
                        {
                            tiepnhan = Convert.ToString(row["72_75_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_75_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["72_75_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Cuộc gọi nhiễu,nghe xen, tiếng vọng
                        sb.AppendFormat(cellContent, row["72_76"], 71, 72, 76, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("72_76_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_76_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_76_TON"))
                        {
                            tiepnhan = Convert.ToString(row["72_76_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_76_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["72_76_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Không có sóng Indoor
                        sb.AppendFormat(cellContent, row["72_77"], 71, 72, 77, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("72_77_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_77_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_77_TON"))
                        {
                            tiepnhan = Convert.ToString(row["72_77_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_77_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["72_77_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Mất sóng hoàn toàn
                        sb.AppendFormat(cellContent, row["72_1135"], 71, 72, 1135, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("72_1135_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_1135_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_1135_TON"))
                        {
                            tiepnhan = Convert.ToString(row["72_1135_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("72_74_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["72_1135_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // ========================================================================================
                        // Sóng 3G
                        // Không xác định
                        sb.AppendFormat(cellContent, row["80_0"], 71, 80, 0, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("80_0_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["80_0_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_0_TON"))
                        {
                            tiepnhan = Convert.ToString(row["80_0_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_0_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["80_0_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Sóng kém hoặc chập chờn (cả indoor và outdoor)
                        sb.AppendFormat(cellContent, row["80_883"], 71, 80, 883, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("80_883_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["80_883_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_883_TON"))
                        {
                            tiepnhan = Convert.ToString(row["80_883_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_883_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["80_883_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Có sóng nhưng gọi đi hoặc gọi đến không được
                        sb.AppendFormat(cellContent, row["80_884"], 71, 80, 884, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("80_884_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["80_884_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_884_TON"))
                        {

                            tiepnhan = Convert.ToString(row["80_884_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_884_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["80_884_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Đang đàm thoại rớt cuộc ( mất tín hiệu, báo gián đoạn..)
                        sb.AppendFormat(cellContent, row["80_84"], 71, 80, 84, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("80_84_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["80_84_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_84_TON"))
                        {
                            tiepnhan = Convert.ToString(row["80_84_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_84_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["80_84_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }

                        // Cuộc gọi nhiễu,nghe xen, tiếng vọng
                        sb.AppendFormat(cellContent, row["80_85"], 71, 80, 85, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("80_85_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["80_85_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_85_TON"))
                        {

                            tiepnhan = Convert.ToString(row["80_85_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_85_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["80_85_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        // Không có sóng Indoor
                        sb.AppendFormat(cellContent, row["80_86"], 71, 80, 86, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("80_86_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["80_86_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_86_TON"))
                        {
                            tiepnhan = Convert.ToString(row["80_86_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_86_QUA_H"))
                        {
                            tiepnhan = Convert.ToString(row["80_86_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        // Mất sóng hoàn toàn
                        sb.AppendFormat(cellContent, row["80_87"], 71, 80, 87, row["MaTinh"], row["KhuVucId"]);
                        if (dtResult.Columns.Contains("80_87_TIEP_N"))
                        {
                            tiepnhan = Convert.ToString(row["80_87_TIEP_N"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_87_TON"))
                        {
                            tiepnhan = Convert.ToString(row["80_87_TON"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        if (dtResult.Columns.Contains("80_87_QUA_H"))
                        {

                            tiepnhan = Convert.ToString(row["80_87_QUA_H"]).Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                        }
                        if (tiepnhan != null)
                        {
                            if (tiepnhan.Length == 2)
                            {
                                sb.AppendFormat(cellContentActivity, tiepnhan[0], tiepnhan[1]);
                                tiepnhan = null;
                            }
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, null, null);
                        }
                        sb.Append("</tr>");
                        indexSTT++;
                    }
                }


                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='2' class='borderThinTextBold'>Tổng toàn mạng</td>");
                for (int indexColumn = 3; indexColumn < dtResult.Columns.Count; indexColumn++)
                {
                    int totalColumn = 0;
                    string listAllKhieuNaiId = "";
                    for (int k = 0; k < dtResult.Rows.Count; k++)
                    {
                        if (Convert.ToString(dtResult.Rows[k][indexColumn]).Contains("WHERE:"))
                        {
                            var tongTrong = dtResult.Rows[k][indexColumn].ToString().Split(new string[] { "WHERE:" }, StringSplitOptions.None);
                            totalColumn += ConvertUtility.ToInt32(tongTrong[0], 0);
                            listAllKhieuNaiId += tongTrong[1] + " ";
                        }
                        else
                        {
                            totalColumn += ConvertUtility.ToInt32(dtResult.Rows[k][indexColumn], 0);
                        }
                    }

                    //sb.AppendFormat("<td   align='center' class='borderThinTextBold'>{0}</td>", totalColumn);
                    if (dtResult.Columns[indexColumn].ColumnName == "TongKN")
                    {
                        sb.AppendFormat(cellContentBold, totalColumn, 71, -1, -1, -1, -1);
                    }
                    else
                    {
                        string[] arrColName = dtResult.Columns[indexColumn].ColumnName.Split('_');
                        if (arrColName[0] == "71" && arrColName[1] == "0") // 71_0 : Chỉ có loại khiếu nại
                        {
                            sb.AppendFormat(cellContentBold, totalColumn, 71, 0, 0, -1, -1);
                        }
                        else if (arrColName[1] == "0") // 72_0, 80_0 : Chỉ có lĩnh vực chung
                        {
                            sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], 0, -1, -1);
                        }
                        else
                        {
                            sb.AppendFormat(cellContentActivity, totalColumn, listAllKhieuNaiId);
                            //sb.AppendFormat(cellContentBold, totalColumn, 71, arrColName[0], arrColName[1], -1, -1);
                        }
                    }
                }

                sb.Append("</tr>");
            }
            else
            {
                sb.AppendFormat("<tr><td colspan='17'>Không có bản ghi nào</td></tr>");
            }

            return sb.ToString();
        }

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 24/07/2014
        ///// </summary>
        ///// <param name="khuVucXuLyId"></param>
        ///// <param name="listDoiTacXuLyId"></param>
        ///// <param name="listPhongBanXuLyId"></param>
        ///// <param name="fromDate"></param>
        ///// <param name="toDate"></param>
        ///// <returns></returns>
        //public string BaoCaoChiTietKhieuNaiQuaHan(List<int> listKhuVucXuLyId, List<int> listDoiTacXuLyId, List<int> listPhongBanXuLyId, DateTime fromDate, DateTime toDate)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    List<KhieuNai_ReportInfo> listKhieuNai = new ReportImpl().ListKhieuNaiQuaHanActivity(listKhuVucXuLyId, listDoiTacXuLyId, listPhongBanXuLyId, fromDate, toDate);
        //    if(listKhieuNai != null)
        //    {
        //        PhongBanImpl phongBanImpl = new PhongBanImpl();
        //        List<PhongBanInfo> listPhongBan = phongBanImpl.GetList();
        //        sb.AppendFormat("<tr><td colspan='9'>Tổng số bản ghi : {0}</td></tr>", listKhieuNai.Count);
        //        sb.Append("<tr>");
        //        sb.Append("<th>STT</th>");
        //        sb.Append("<th>Mã PA</th>");
        //        sb.Append("<th>Số thuê bao</th>");
        //        sb.Append("<th>Ngày tạo khiếu nại</th>");
        //        sb.Append("<th>Phòng ban xử lý trước</th>");
        //        sb.Append("<th>Ngày tiếp nhận</th>");
        //        sb.Append("<th>Phòng ban xử lý</th>");
        //        sb.Append("<th>Người xử lý</th>");
        //        sb.Append("<th>Ngày quá hạn phòng ban</th>");
        //        sb.Append("</tr>");

        //        for(int i=0;i<listKhieuNai.Count;i++)
        //        {
        //            string phongBanXuLyTruoc = phongBanImpl.GetNamePhongBan(listKhieuNai[i].PhongBanXuLyTruocId);
        //            string phongBanXuLy = phongBanImpl.GetNamePhongBan(listKhieuNai[i].PhongBanXuLyId);
        //            sb.Append("<tr>");
        //            sb.AppendFormat("<td>{0}</td>", (i+1));
        //            sb.AppendFormat("<td>{0}</td>",  listKhieuNai[i].KhieuNaiId);
        //            sb.AppendFormat("<td>{0}</td>", listKhieuNai[i].SoThueBao);
        //            sb.AppendFormat("<td>{0}</td>", listKhieuNai[i].KhieuNai_NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
        //            sb.AppendFormat("<td>{0}</td>", phongBanXuLyTruoc);
        //            sb.AppendFormat("<td>{0}</td>", listKhieuNai[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
        //            sb.AppendFormat("<td>{0}</td>", phongBanXuLy);
        //            sb.AppendFormat("<td>{0}</td>", listKhieuNai[i].NguoiXuLy);
        //            sb.AppendFormat("<td>{0}</td>", listKhieuNai[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
        //            sb.Append("</tr>");
        //        }
        //    }

        //    return sb.ToString();
        //}

        /// <summary>        
        /// created Date: 04/11/2014
        /// Báo cáo tong hợp khiếu nại theo loại khiếu nại toàn mạng vnp
        /// </summary>
        /// <param name="khuvucId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <returns></returns>
        public string BaoCaoTongHopTheoLoaiKhieuNaiToanMangVNP(int khuvucId, DateTime fromDateBefore, DateTime toDateBefore, DateTime fromDate, DateTime toDate, int nguonKhieuNai)
        {
            StringBuilder sb = new StringBuilder();
            decimal totalTiepNhanKyTruoc = 0;
            decimal totalTiepNhanTrongKy = 0;
            decimal totalDaDongTrongKy = 0;
            decimal totalDaDong = 0;
            decimal totalQuaHanTrongKy = 0;
            decimal totalQuaHan = 0;
            DataTable dt = new ReportImpl().GetTongHopKhieuNaiTheoLoaiKhieuNaiToanMang_Solr(khuvucId, fromDateBefore, toDateBefore, fromDate, toDate, nguonKhieuNai);
            if (dt != null && dt.Rows.Count > 0)
            {
                string curLoaiKhieuNaiId = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    decimal slTiepNhanKyTruoc = ConvertUtility.ToDecimal(dr["SLTiepNhanKyTruoc"].ToString(), 0);
                    decimal slTiepNhanTrongKy = ConvertUtility.ToDecimal(dr["SLTiepNhanTrongKy"].ToString(), 0);
                    decimal slDaDongTrongKy = ConvertUtility.ToDecimal(dr["SLDaDongTrongKy"].ToString(), 0);
                    decimal slDaDong = ConvertUtility.ToDecimal(dr["SLDaDong"].ToString(), 0);
                    decimal slQuaHanTrongKy = ConvertUtility.ToDecimal(dr["SLQuaHanToanTrinhTrongKy"].ToString(), 0);
                    decimal slQuaHan = ConvertUtility.ToDecimal(dr["SLQuaHanToanTrinh"].ToString(), 0);
                    totalTiepNhanKyTruoc += slTiepNhanKyTruoc;
                    totalTiepNhanTrongKy += slTiepNhanTrongKy;
                    totalDaDongTrongKy += slDaDongTrongKy;
                    totalDaDong += slDaDong;
                    totalQuaHanTrongKy += slQuaHanTrongKy;
                    totalQuaHan += slQuaHan;

                    string cellContentKyTruoc = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?doiTacId=" + khuvucId.ToString() + "&fromPage=bctonghoptheoloaiknVNP" + "&fromdate=" + fromDateBefore.ToString("dd/MM/yyyy") + "&toDate=" + fromDateBefore.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + dr["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + dr["LinhVucChungId"].ToString() + "&nguonKhieuNai=" + nguonKhieuNai + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                    string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?doiTacId=" + khuvucId.ToString() + "&fromPage=bctonghoptheoloaiknVNP" + "&fromdate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + dr["LoaiKhieuNaiId"].ToString() + "&linhVucChungId=" + dr["LinhVucChungId"].ToString() + "&nguonKhieuNai=" + nguonKhieuNai + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";

                    sb.Append("<tr>");
                    if (curLoaiKhieuNaiId != dr["LoaiKhieuNaiId"].ToString())
                    {
                        int rowspan = 1;
                        if (i < dt.Rows.Count - 1)
                        {
                            for (int j = i + 1; j < dt.Rows.Count; j++)
                            {
                                if (dr["LoaiKhieuNaiId"].ToString() == dt.Rows[j]["LoaiKhieuNaiId"].ToString())
                                {
                                    rowspan++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        sb.AppendFormat("<td align='center' rowspan='{1}' valign='top'>{0}</td>", dr["STT"], rowspan);
                        sb.AppendFormat("<td rowspan='{1}' valign='top'>{0}</td>", dr["LoaiKhieuNai"], rowspan);

                        curLoaiKhieuNaiId = dr["LoaiKhieuNaiId"].ToString();
                    }


                    sb.Append("<td>" + dr["LinhVucChung"].ToString() + "</td>");
                    if (slTiepNhanKyTruoc == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(cellContentKyTruoc, 1, dr["SLTiepNhanKyTruoc"].ToString());
                    }

                    if (slTiepNhanTrongKy == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 2, dr["SLTiepNhanTrongKy"].ToString());
                    }

                    if (slTiepNhanKyTruoc == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        string tyLe = Convert.ToDecimal(((slTiepNhanTrongKy - slTiepNhanKyTruoc) / slTiepNhanKyTruoc) * 100).ToString("#0.##");
                        sb.AppendFormat("<td>{0}</td>", tyLe);
                    }

                    if (slDaDongTrongKy == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 3, dr["SLDaDongTrongKy"].ToString());
                    }

                    if (slDaDong == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 4, dr["SLDaDong"].ToString());
                    }

                    if (slQuaHanTrongKy == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 5, dr["SLQuaHanToanTrinhTrongKy"].ToString());
                    }

                    if (slQuaHan == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 6, dr["SLQuaHanToanTrinh"].ToString());
                    }

                    if (slTiepNhanTrongKy == 0)
                    {
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        string tyLe = Convert.ToDecimal((slQuaHanTrongKy / slTiepNhanTrongKy) * 100).ToString("#0.##");
                        sb.AppendFormat("<td>{0}</td>", tyLe);
                    }

                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td colspan='3' class='borderThinTextBold'>Tổng:</td>");

                if (totalTiepNhanKyTruoc == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + totalTiepNhanKyTruoc.ToString("N0") + "</td>");
                }

                if (totalTiepNhanTrongKy == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + totalTiepNhanTrongKy.ToString("N0") + "</td>");
                }

                if (totalTiepNhanKyTruoc == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + Convert.ToDecimal(((totalTiepNhanTrongKy - totalTiepNhanKyTruoc) / totalTiepNhanKyTruoc) * 100).ToString("#0.##") + "</td>");
                }

                if (totalDaDongTrongKy == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + totalDaDongTrongKy.ToString("N0") + "</td>");
                }

                if (totalDaDong == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + totalDaDong.ToString("N0") + "</td>");
                }

                if (totalQuaHanTrongKy == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + totalQuaHanTrongKy.ToString("N0") + "</td>");
                }

                if (totalQuaHan == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + totalQuaHan.ToString("N0") + "</td>");
                }

                if (totalTiepNhanTrongKy == 0)
                {
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<td class='borderThinTextBold'>" + Convert.ToDecimal((ConvertUtility.ToDecimal(totalQuaHanTrongKy) / ConvertUtility.ToDecimal(totalTiepNhanTrongKy)) * 100).ToString("#0.##") + "</td>");
                }

                sb.Append("</tr>");
            }
            else
            {
                sb.Append(@"<tr>
                                <td colspan='7'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 14/07/2015
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public string BaoCaoTyLeKNTonDongQuaHanPhongBan(DateTime date1, DateTime date2)
        {
            StringBuilder sb = new StringBuilder();
            List<DateTime> listDate = new List<DateTime>();
            listDate.Add(date1);
            listDate.Add(date2);

            decimal totalSL1 = 0;
            decimal totalSL2 = 0;
            decimal tyLeTotal = 0;

            sb.Append("<tr>");
            sb.Append("<th>STT</th>");
            sb.Append("<th>Đơn vị</th>");
            sb.AppendFormat("<th>Số lượng tồn quá hạn ngày {0}</th>", date1.ToString("dd/MM/yyyy"));
            sb.AppendFormat("<th>Số lượng tồn quá hạn ngày {0}</th>", date2.ToString("dd/MM/yyyy"));
            sb.Append("<th>Tỷ lệ (%)</th>");
            sb.Append("</tr>");

            DataTable dtResult = new ReportImpl().BaoCaoTyLeKNTonDongQuaHanPhongBan_Solr(listDate);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {

                string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotyletondongquahanphongban&doiTacId={1}&phongBanId=-1&ngayQuaHan={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
                int index = 0;
                foreach (DataRow row in dtResult.Rows)
                {
                    index++;
                    decimal sl1 = ConvertUtility.ToDecimal(row[date1.ToString("ddMMyyyy")]);
                    decimal sl2 = ConvertUtility.ToDecimal(row[date2.ToString("ddMMyyyy")]);
                    decimal tyLe = 0;
                    if (sl1 != 0)
                    {
                        tyLe = ((sl2 - sl1) / sl1) * 100;
                    }

                    totalSL1 += sl1;
                    totalSL2 += sl2;

                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", index.ToString());
                    sb.AppendFormat("<td>{0}</td>", row["DoiTac"]);
                    sb.AppendFormat(cellContent, row[date1.ToString("ddMMyyyy")], row["DoiTacId"], date1.ToString("dd/MM/yyyy"));
                    sb.AppendFormat(cellContent, row[date2.ToString("ddMMyyyy")], row["DoiTacId"], date2.ToString("dd/MM/yyyy"));
                    sb.AppendFormat("<td>{0}</td>", tyLe.ToString("###.##"));
                    sb.Append("</tr>");
                }

                if (totalSL1 != 0)
                {
                    tyLeTotal = ((totalSL2 - totalSL1) / totalSL1) * 100;
                }

                sb.Append("<tr>");
                sb.Append("<td class='borderThinTextBold' colspan='2'>Tổng</td>");
                sb.AppendFormat("<td class='borderThinTextBold'>{0}</td>", totalSL1.ToString("###,###,###"));
                sb.AppendFormat("<td class='borderThinTextBold'>{0}</td>", totalSL2.ToString("###,###,###"));
                sb.AppendFormat("<td class='borderThinTextBold'>{0}</td>", tyLeTotal.ToString("###.##"));
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr><td colspan='5'>Không có bản ghi nào</td></tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/08/2015
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="isExportExcel"></param>
        /// <returns></returns>
        public string DanhSachKhieuNaiGiamTru(int khuVucId, int doiTacId, int phongBanXuLyId, DateTime fromDate, DateTime toDate, int nguonKhieuNai, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            List<KhieuNai_ReportInfo> listKhieuNai = new ReportImpl().DanhSachKhieuNaiGiamTru_Solr(khuVucId, doiTacId, phongBanXuLyId, fromDate, toDate, nguonKhieuNai);
            if (listKhieuNai != null && listKhieuNai.Count > 0)
            {
                decimal total = 0;
                int index = 1;
                sb.AppendFormat("<tr><td colspan='10'>Số lượng bản ghi : {0}</td></tr>", listKhieuNai.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Nội dung phản ánh</th>");
                sb.Append("<th>Nội dung đóng KN</th>");
                sb.Append("<th>Ngày đóng KN</th>");
                sb.Append("<th>Đơn vị xử lý</th>");
                sb.Append("<th>Số hiệu công văn</th>");
                sb.Append("<th>Số tiền giảm trừ</th>");
                sb.Append("</tr>");
                for (int i = 0; i < listKhieuNai.Count; i++)
                {
                    decimal totalTienGiamTru = listKhieuNai[i].SoTienKhauTru_TKC + listKhieuNai[i].SoTienKhauTru_KM + listKhieuNai[i].SoTienKhauTru_KM1
                                                + listKhieuNai[i].SoTienKhauTru_KM2 + listKhieuNai[i].SoTienKhauTru_Khac
                                                + listKhieuNai[i].SoTienKhauTru_TS_GPRS + listKhieuNai[i].SoTienKhauTru_TS_CP + listKhieuNai[i].SoTienKhauTru_TS_Thoai
                                                + listKhieuNai[i].SoTienKhauTru_TS_SMS + listKhieuNai[i].SoTienKhauTru_TS_IR + listKhieuNai[i].SoTienKhauTru_TS_Khac;

                    if (totalTienGiamTru > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].Id + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].SoThueBao + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].LinhVucCon + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].NoiDungPA + "</td>");
                        sb.Append("<td class='borderThin'>" + listKhieuNai[i].NoiDungXuLyDongKN + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNai[i].NgayDongKN.ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td class='borderThin'>" + GetTenDoiTac(DoiTacImpl.ListDoiTac, listKhieuNai[i].DoiTacXuLyId) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + listKhieuNai[i].KQXuLy_SHCV + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + totalTienGiamTru.ToString(formatNumber) + "</td>");
                        sb.Append("</tr>");

                        total += totalTienGiamTru;
                        index++;
                    }
                }

                sb.Append("<tr>");
                sb.Append("<td align='center' class='borderThinTextBold' colspan='9' >TỔNG</td>");
                sb.Append("<td align='center' class='borderThinTextBold'>" + total.ToString(formatNumber) + "</td>");
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr><td colspan='10'>Không có dữ liệu</td></tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 25/11/2015
        /// Todo : 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoKhieuNaiDichVuGTGTTapDoan(DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtResult = new ReportImpl().BaoCaoKhieuNaiDichVuGTGTTapDoan_Solr(fromDate, toDate);

            string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaokhieunaidichvugtgttapdoan&linhVucConId={1}&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate= " + toDate.ToString("dd/MM/yyyy") + "&reportType={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
            sb.Append("<tr>");
            sb.Append("<th>TT</th>");
            sb.Append("<th>Đơn vị quản lý hợp đồng</th>");
            sb.Append("<th>Tên nhà cung cấp/đối tác</th>");
            sb.Append("<th>Tên dịch vụ</th>");
            sb.Append("<th>Đầu số/Mã dịch vụ</th>");
            sb.Append("<th>Số lượng khiếu nại tiếp nhận</th>");
            sb.Append("<th>Số lượng khiếu nại đã xử lý</th>");
            sb.Append("<th>Thuê bao sử dụng dịch vụ trong tháng</th>");
            sb.Append("<th>KPI</th>");
            sb.Append("<th>Đánh giá kết quả KPI</th>");
            sb.Append("</tr>");

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1));
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.AppendFormat("<td>{0}</td>", dtResult.Rows[i]["TenLinhVucCon"].ToString());
                    sb.Append("<td></td>");
                    sb.AppendFormat(cellContent, dtResult.Rows[i]["SLTiepNhan"].ToString(), dtResult.Rows[i]["LinhVucConId"].ToString(), 1);
                    sb.AppendFormat(cellContent, dtResult.Rows[i]["SLDaDong"].ToString(), dtResult.Rows[i]["LinhVucConId"].ToString(), 2);
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Dev: Đào Văn Dương
        /// </summary>
        public string BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoan(int month, int year, string[] dsDonViBaoCao)
        {
            StringBuilder sb = new StringBuilder();

            StringBuilder header = new StringBuilder();
            StringBuilder content = new StringBuilder();
            header.AppendFormat("<tr><th rowspan=\"2\">STT</th>");
            header.AppendFormat("<th rowspan=\"2\">Mã dịch vụ</th>");
            header.AppendFormat("<th rowspan=\"2\">Tên nhà cung cấp</th>");
            header.AppendFormat("<th rowspan=\"2\">Tên dịch vụ</th>");
            header.AppendFormat("<th rowspan=\"2\">Đầu số</th>");
            header.AppendFormat("<th rowspan=\"2\">Tổng số khiếu nại</th>");

            header.AppendFormat("<th rowspan=\"2\">Tổng số khách hàng sử dụng dịch vụ</th>");
            header.AppendFormat("<th rowspan=\"2\">KPI</th>");
            header.AppendFormat("<th rowspan=\"2\">Không vi phạm (KPI<=0,05%)</th>");
            header.AppendFormat("<th rowspan=\"2\">Vi phạm Mức 1  Đánh giá kết quả (0,05%<=KPI<0,08%)</th>");

            header.AppendFormat("<th rowspan=\"2\">Vi phạm Mức 2  Đánh giá kết quả (0,08%<=KPI<0,15%)</th>");
            header.AppendFormat("<th rowspan=\"2\">Vi phạm Mức 3 Đánh giá kết quả(KPI >= 0, 15 %)</th>");
            header.AppendFormat("<th rowspan=\"2\">Lần vi phạm</th>");
            header.AppendFormat("<th rowspan=\"2\">Xử lý vi phạm</th>");

            header.AppendFormat("<th colspan=\"3\">Doanh Thu</th>");
            header.AppendFormat("</tr>");
            header.AppendFormat("<tr>");
            header.AppendFormat("<th>DT phát sinh KH</th>");

            header.AppendFormat("<th>DT VNPT</th>");
            header.AppendFormat("<th>DT phân chia CP</th>");
            header.AppendFormat("</tr>");
            header.AppendFormat("</tr>");

            // Truy vấn mặc định
            string sql = @"SELECT 
		                            DENSE_RANK() OVER (ORDER BY TenCongTyDoiTac) AS [Index],	
		                            ROW_NUMBER() OVER (PARTITION BY TenCongTyDoiTac ORDER BY (SELECT TenLinhVucCon)) AS Number,
		                            *,
		                            CAST(NULL AS NVARCHAR(64)) DauSo,
		                            CAST(NULL AS INT) TongSoKhieuNai,
		                            CAST(NULL AS INT) TongSoKhachHangSD,
		                            CAST(NULL AS FLOAT) DoanhThu,
		                            CAST(NULL AS FLOAT) DoanhThuCP,
		                            CAST(NULL AS FLOAT) DoanhThuVNP
                                FROM (
			                        SELECT 
				                        a.TenCongTyDoiTac,
				                        a.ServiceCode ServiceCode,
				                        CAST(a.Id AS NVARCHAR(255)) LinhVucConIds,
				                        a.Name TenLinhVucCon	
			                        FROM LoaiKhieuNai a
			                        WHERE
				                        1 = 1
				                        AND a.Cap = 3
				                        AND a.LoaiKhieuNai_NhomId = 22
				                        AND a.ParentLoaiKhieuNaiId IN ({0})
				                        AND (a.IsChungMa IS NULL OR a.IsChungMa = 0)
			                       ) a
		                        ORDER BY
			                        a.TenCongTyDoiTac,
			                        a.TenLinhVucCon";

            if (year == 2016 && (month == 5 || month == 6 || month == 7)) // Tháng 5, 6 năm 2016 lấy trong file Excel
            {
                sql = @"SELECT 
	                    DENSE_RANK() OVER (ORDER BY TenCongTyDoiTac) AS [Index],	
	                    ROW_NUMBER() OVER (PARTITION BY TenCongTyDoiTac ORDER BY (SELECT Name)) AS Number,
	                    TenCongTyDoiTac,
	                    ServiceCode,
                        Ids LinhVucConIds,	                        	                              
	                    Name TenLinhVucCon,
	                    CAST(NULL AS NVARCHAR(64)) DauSo,
	                    CAST(NULL AS INT) TongSoKhieuNai,
	                    CAST(TongThueBaoSD AS INT) TongSoKhachHangSD,
	                    CAST(DoanhThu AS FLOAT) DoanhThu,
	                    CAST(DoanhThuCP AS FLOAT) DoanhThuCP,
	                    CAST(DoanhThuVNP AS FLOAT) DoanhThuVNP
                    FROM LoaiKhieuNai_DoanhThu
                    WHERE
	                    1 = 1
	                    AND Thang = @Month
                        AND Nam = @Year   
	                    AND (ParentLoaiKhieuNaiId IS NULL OR ParentLoaiKhieuNaiId IN ({0}))
                    ORDER BY
	                    TenCongTyDoiTac,
	                    Name";

                if (month == 7)
                {
                    sql = @"SELECT 
	                        DENSE_RANK() OVER (ORDER BY a.TenCongTyDoiTac) AS [Index],	
	                        ROW_NUMBER() OVER (PARTITION BY a.TenCongTyDoiTac ORDER BY (SELECT TenLinhVucCon)) AS Number,
	                        a.*,
	                        CAST(NULL AS NVARCHAR(64)) DauSo,
	                        CAST(NULL AS INT) TongSoKhieuNai,
	                        CAST(c.TongThueBaoSD AS INT) TongSoKhachHangSD,
	                        CAST(c.DoanhThu AS FLOAT) DoanhThu,
	                        CAST(c.DoanhThuCP AS FLOAT) DoanhThuCP,
	                        CAST(c.DoanhThuVNP AS FLOAT) DoanhThuVNP
                            FROM (
	                        SELECT 
		                        a.TenCongTyDoiTac,
		                        a.ServiceCode ServiceCode,
		                        CAST(a.Id AS NVARCHAR(255)) LinhVucConIds,
		                        a.Name TenLinhVucCon	
	                        FROM LoaiKhieuNai a
	                        WHERE
		                        1 = 1
		                        AND a.Cap = 3
		                        AND a.LoaiKhieuNai_NhomId = 22
		                        AND a.ParentLoaiKhieuNaiId IN ({0})
		                        AND (a.IsChungMa IS NULL OR a.IsChungMa = 0)
	                        UNION ALL
	                        SELECT 
		                        NULL AS CongTyDoiTac,
		                        a.ServiceCode,
		                        STUFF((SELECT  ','+ CAST(Id AS NVARCHAR(1024)) FROM LoaiKhieuNai b WHERE b.ServiceCode = a.ServiceCode FOR XML PATH('')), 1, 1, '')  LinhVucConIds,
		                        STUFF((SELECT  ','+ CAST(Name AS NVARCHAR(1024)) FROM LoaiKhieuNai b WHERE b.ServiceCode = a.ServiceCode FOR XML PATH('')), 1, 1, '')  TenCacLinhVucCon
	                        FROM LoaiKhieuNai a
	                        WHERE
		                        1 = 1
		                        AND a.IsChungMa = 1
		                        AND a.Cap = 3
		                        AND a.LoaiKhieuNai_NhomId =22
	                        GROUP BY 
		                        a.ServiceCode
	                        HAVING
		                        COUNT (a.ServiceCode) > 1
	                        ) a 
                            LEFT JOIN (SELECT * FROM LoaiKhieuNai_DoanhThu a WHERE a.Thang = 7 AND a.Nam = 2016) c ON a.ServiceCode = c.ServiceCode
                        WHERE
	                        1 = 1	 
                        ORDER BY
	                        a.TenCongTyDoiTac,
	                        a.TenLinhVucCon";
                }
            }

            SqlParameter[] prms = new SqlParameter[] {
                new SqlParameter("@Month",month),
                new SqlParameter("@Year",year),
            };
            DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, string.Format(sql, string.Join(",", dsDonViBaoCao)), prms).Tables[0];
            if (tbl.Rows.Count > 0)
            {
                DateTime fromDate = new DateTime(year, month, 1);
                DateTime toDate = (new DateTime(year, month + 1, 1)).AddSeconds(-1);
                string date = fromDate.ToString("yyyyMM");
                bool isGetDataOK = true;

                if (!((month == 5 || month == 6 || month == 7) && year == 2016)) // Nếu không thuộc tháng 5 và tháng 6 => Lấy Online
                {
                    #region Lấy dữ liệu Doanh thu từ Service

                    // Khai báo  biến danh sách mã Code cân lấy dữ liệu
                    List<string> lstCP_Code = new List<string>();
                    foreach (DataRow row in tbl.Rows)
                    {
                        string cpCode = row["ServiceCode"].ToString().Trim();
                        if (cpCode != string.Empty)
                        {
                            lstCP_Code.Add(cpCode);
                        }
                    }
                    int maxGet = 100;
                    int maxNumber = lstCP_Code.Count;

                    int soNguyen = maxNumber / maxGet;
                    int SoDu = maxNumber % maxGet;
                    if (SoDu > 0)
                        soNguyen = soNguyen + 1;

                    List<string> newListCodes = new List<string>();

                    ServiceVinaphone1 sv = new ServiceVinaphone1();
                    RequestParamGet_cp_info svPrms = new RequestParamGet_cp_info();

                    for (int i = 0; i < soNguyen; i++)
                    {
                        int start = i * maxGet;
                        int end = start + maxGet;
                        newListCodes.Clear();
                        for (int j = start; j < end; j++)
                        {
                            if (j >= lstCP_Code.Count)
                                break;
                            newListCodes.Add(lstCP_Code[j]);
                        }
                        // Danh sách Codes cần lấy dữ liệu
                        string tmpCodes = string.Join(",", newListCodes);
                        svPrms.p_Month = date;
                        svPrms.cp_Code = tmpCodes;

                        ServiceVNP.ResponseCPInfo datas = sv.TraCuuDoanhThuCP(svPrms);

                        if (datas.Code != 0) // Có lỗi
                        {
                            Helper.GhiLogs("SvDoanhThuGTGT", "Lỗi lấy dữ liệu Service: Get_CpInfo, Mã lỗi: {0}, Thông tin: {1", datas.Code, datas.Message);
                            content.AppendFormat("<tr><td colspan=\"16\" style=\"text-align: center; font-weight: bold; padding: 20px; color: maroon; font-size: 14px;\">{0}, lỗi: {1}</td></tr>", "Không kết nối được dữ liệu", datas.Message);
                            isGetDataOK = false;
                            break;
                        }
                        else
                        {
                            // Lấy được dữ liệu
                            CPDetailInfo[] lst = datas.Detail;
                            foreach (CPDetailInfo info in lst)
                            {
                                if (info.RESULT_CODE == "1") // Lấy dữ liệu thành công
                                {
                                    string maCp = info.CODE;
                                    int tongSoThueBao = Convert.ToInt32(info.NO_SUBS);
                                    decimal doanhThu = 0;
                                    decimal.TryParse(info.TOTAL_CHARGE, out doanhThu);

                                    EnumerableRowCollection<DataRow> rows = from myRow in tbl.AsEnumerable()
                                                                            where myRow.Field<string>("ServiceCode") != null && myRow.Field<string>("ServiceCode").ToString() == maCp
                                                                            select myRow;
                                    List<DataRow> lstRows = rows.ToList();
                                    if (lstRows.Count == 1) // Thực thế chỉ có thể = 1 vì Mã Code không trùng nhau
                                    {
                                        lstRows[0]["TongSoKhachHangSD"] = tongSoThueBao;
                                        lstRows[0]["DoanhThu"] = doanhThu;

                                        DateTime cBaoCao = new DateTime(year, month, 15);
                                        if (cBaoCao > new DateTime(2016, 7, 1)) // Bắt đầu tính báo cáo từ 08-2016
                                        {
                                            decimal doanhThuCP = 0;
                                            decimal doanhThuVNP = 0;

                                            decimal.TryParse(info.CP_CHARGE, out doanhThuCP);
                                            decimal.TryParse(info.VNP_CHARGE, out doanhThuVNP);

                                            lstRows[0]["DoanhThuCP"] = doanhThuCP;
                                            lstRows[0]["DoanhThuVNP"] = doanhThuVNP;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // Kết thúc lấy dữ liệu từ SV
                    #endregion
                }
                if (isGetDataOK)
                {
                    // Lấy số lượng KN trên Solr
                    new ReportImpl().BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoan_Solr_Tu201605(month, year, tbl);
                    #region Hiển thị lưới dữ liệu

                    #region Xắp xếp hiển thị

                    var obj = from tab in tbl.AsEnumerable()
                              group tab by tab["Index"]
                              into groupData
                              select new
                              {
                                  GroupId = groupData.Key,
                                  MaxRow = groupData.Count()
                              };
                    #endregion

                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {

                        DataRow r = tbl.Rows[i];

                        content.AppendFormat("<tr><td>{0}</td>", i + 1);
                        content.AppendFormat("<td>{0}</td>", r["ServiceCode"]);
                        string linhVucCon = r["TenLinhVucCon"].ToString().Trim();
                        int index = linhVucCon.IndexOf("_");
                        string dauSo = string.Empty;
                        if (index > 0)
                        {
                            dauSo = linhVucCon.Substring(0, index);
                            int number = 0;
                            string abc = dauSo[0].ToString();

                            // Kiểm tra xem có phải là số hay chữ
                            if (!int.TryParse(abc, out number))
                                dauSo = string.Empty;
                        }
                        // Tính KPI
                        int tongSoKhieuNai = r["TongSoKhieuNai"].ToString() != string.Empty ? Convert.ToInt32(r["TongSoKhieuNai"]) : 0;
                        int tongSoKhachHangSD = r["TongSoKhachHangSD"].ToString() != string.Empty ? Convert.ToInt32(r["TongSoKhachHangSD"]) : 0;

                        long rowNumber = (long)r["Number"];
                        if (rowNumber == 1) // Tính toán nhóm theo tên Công ty đối tác
                        {
                            long groupNumber = (long)r["Index"];
                            var rx = obj.Where(v => Convert.ToInt32(v.GroupId) == groupNumber).SingleOrDefault();
                            content.AppendFormat("<td rowspan=\"{1}\" style=\"vertical-align: middle;\" >{0}</td>", r["TenCongTyDoiTac"], rx.MaxRow);
                        }

                        content.AppendFormat("<td>{0}</td>", r["TenLinhVucCon"]);
                        content.AppendFormat("<td>{0}</td>", dauSo);

                        string tsknLink = string.Format("<a ids=\"{1}\" thang=\"2\" nam=\"{3}\" href=\"PopupDetail.aspx?Type=BaoCaoDichVuGTGTChoTapDoan&Ids={1}&Thang={2}&Nam={3}\" class=\"openthis\">{0}</a>", r["TongSoKhieuNai"], r["LinhVucConIds"].ToString(), month, year);

                        string tsKhieuNai = r["TongSoKhieuNai"].ToString() == string.Empty ? (tongSoKhachHangSD == 0 ? string.Empty : "-") : tsknLink;

                        content.AppendFormat("<td>{0}</td>", tsKhieuNai);

                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["TongSoKhachHangSD"], string.Empty));//  r["TongSoKhachHangSD"] != System.DBNull.Value ? (Convert.ToDouble(r["TongSoKhachHangSD"])).ToString() : string.Empty);

                        double kpi = 0;
                        if (tongSoKhachHangSD > 0)
                            kpi = tongSoKhieuNai / (tongSoKhachHangSD * (1.0));

                        if (tongSoKhieuNai == 0 && tongSoKhachHangSD == 0)
                            content.AppendFormat("<td>{0}</td>", string.Empty); // Cột KPI
                        else
                        {
                            if (tongSoKhieuNai == 0)
                                content.AppendFormat("<td>{0}</td>", "-"); // Tổng số KHSD <> 0
                            else
                            {
                                if (tongSoKhachHangSD == 0)
                                {
                                    content.AppendFormat("<td>{0}</td>", string.Empty);
                                }
                                else
                                {
                                    content.AppendFormat("<td>{0}</td>", string.Concat((kpi).ToString("P3", new CultureInfo("vi-VN"))));
                                }
                            }
                        }

                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : (kpi < 0.0005 ? "Không vi phạm" : "Vi phạm"));
                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : ((kpi >= 0.0005 && kpi < 0.0008) ? "Vi phạm mức 1" : "-"));

                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : ((kpi >= 0.0008 && kpi < 0.0015) ? "Vi phạm mức 2" : "-"));
                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : ((kpi >= 0.0015) ? "Vi phạm mức 3" : "-"));
                        content.AppendFormat("<td>{0}</td>", string.Empty);
                        content.AppendFormat("<td>{0}</td>", string.Empty);

                        //content.AppendFormat("<td>{0}</td>", r["DoanhThu"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThu"])).ToString("N0", CultureInfo.CreateSpecificCulture("vi-VN")) : string.Empty);
                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["DoanhThu"], string.Empty)); // r["DoanhThu"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThu"])).ToString() : string.Empty);
                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["DoanhThuCP"], string.Empty)); // r["DoanhThuCP"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThuCP"])).ToString() : string.Empty);
                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["DoanhThuVNP"], string.Empty)); // r["DoanhThuVNP"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThuVNP"])).ToString() : string.Empty);
                    }
                    #endregion
                }
                else
                {
                    Helper.GhiLogs("Lấy dữ liệu SV báo cáo DV GTTT cho tập đoàn bị lỗi Service, vui lòng kiểm tra lại");
                }

            }
            else
            {
                content.AppendFormat("<tr><td colspan=\"16\" style=\"text-align: center; font-weight: bold; padding: 20px; color: maroon; font-size: 14px;\">{0}</td></tr>", "Hiện không có dữ liệu");
            }
            return string.Format("<table class=\"tbl_style customized tblcus\" style=\"width:100%;\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">{0}{1}</table>", header.ToString(), content.ToString());
        }
        public string BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoanTheoDonViQuanLy(int month, int year, string[] dsDonViQuanLy)
        {
            StringBuilder sb = new StringBuilder();

            StringBuilder header = new StringBuilder();
            StringBuilder content = new StringBuilder();
            header.AppendFormat("<tr><th rowspan=\"2\">STT</th>");
            header.AppendFormat("<th rowspan=\"2\">Mã dịch vụ</th>");
            header.AppendFormat("<th rowspan=\"2\">Đơn vị quản lý</th>");
            header.AppendFormat("<th rowspan=\"2\">Tên nhà cung cấp</th>");
            header.AppendFormat("<th rowspan=\"2\">Tên dịch vụ</th>");
            header.AppendFormat("<th rowspan=\"2\">Đầu số</th>");
            header.AppendFormat("<th rowspan=\"2\">Tổng số khiếu nại</th>");

            header.AppendFormat("<th rowspan=\"2\">Tổng số khách hàng sử dụng dịch vụ</th>");
            header.AppendFormat("<th rowspan=\"2\">KPI</th>");
            header.AppendFormat("<th rowspan=\"2\">Không vi phạm (KPI<=0,05%)</th>");
            header.AppendFormat("<th rowspan=\"2\">Vi phạm Mức 1  Đánh giá kết quả (0,05%<=KPI<0,08%)</th>");

            header.AppendFormat("<th rowspan=\"2\">Vi phạm Mức 2  Đánh giá kết quả (0,08%<=KPI<0,15%)</th>");
            header.AppendFormat("<th rowspan=\"2\">Vi phạm Mức 3 Đánh giá kết quả(KPI >= 0, 15 %)</th>");
            header.AppendFormat("<th rowspan=\"2\">Lần vi phạm</th>");
            header.AppendFormat("<th rowspan=\"2\">Xử lý vi phạm</th>");

            header.AppendFormat("<th colspan=\"3\">Doanh Thu</th>");
            header.AppendFormat("</tr>");
            header.AppendFormat("<tr>");
            header.AppendFormat("<th>DT phát sinh KH</th>");

            header.AppendFormat("<th>DT VNPT</th>");
            header.AppendFormat("<th>DT phân chia CP</th>");
            header.AppendFormat("</tr>");
            header.AppendFormat("</tr>");

            // Báo cáo chỉ lấy từ tháng 7
            string spName = "BaoCao_KPIGTGT_TheoDonViQuanLy";
            string dsLoaiKhieuNai = "3572, 3387, 3486"; // Tất cả thuộc nhóm 22

            DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, spName, month, year, string.Join(",", dsDonViQuanLy), dsLoaiKhieuNai).Tables[0];
            if (tbl.Rows.Count > 0)
            {
                DateTime fromDate = new DateTime(year, month, 1);
                DateTime toDate = (new DateTime(year, month + 1, 1)).AddSeconds(-1);
                string date = fromDate.ToString("yyyyMM");
                bool isGetDataOK = true;

                #region Lấy dữ liệu Doanh thu từ Service

                // Khai báo  biến danh sách mã Code cân lấy dữ liệu
                List<string> lstCP_Code = new List<string>();
                foreach (DataRow row in tbl.Rows)
                {
                    string cpCode = row["ServiceCode"].ToString().Trim();
                    if (cpCode != string.Empty)
                    {
                        lstCP_Code.Add(cpCode);
                    }
                }
                int maxGet = 100;
                int maxNumber = lstCP_Code.Count;

                int soNguyen = maxNumber / maxGet;
                int SoDu = maxNumber % maxGet;
                if (SoDu > 0)
                    soNguyen = soNguyen + 1;

                List<string> newListCodes = new List<string>();

                ServiceVinaphone1 sv = new ServiceVinaphone1();
                RequestParamGet_cp_info svPrms = new RequestParamGet_cp_info();

                for (int i = 0; i < soNguyen; i++)
                {
                    int start = i * maxGet;
                    int end = start + maxGet;
                    newListCodes.Clear();
                    for (int j = start; j < end; j++)
                    {
                        if (j >= lstCP_Code.Count)
                            break;
                        newListCodes.Add(lstCP_Code[j]);
                    }
                    // Danh sách Codes cần lấy dữ liệu
                    string tmpCodes = string.Join(",", newListCodes);
                    svPrms.p_Month = date;
                    svPrms.cp_Code = tmpCodes;

                    ServiceVNP.ResponseCPInfo datas = sv.TraCuuDoanhThuCP(svPrms);

                    if (datas.Code != 0) // Có lỗi
                    {
                        Helper.GhiLogs("Lỗi lấy dữ liệu Service: Get_CpInfo");
                        content.AppendFormat("<tr><td colspan=\"16\" style=\"text-align: center; font-weight: bold; padding: 20px; color: maroon; font-size: 14px;\">{0}</td></tr>", "Không kết nối được dữ liệu, vui lòng thử lại!");
                        isGetDataOK = false;
                        break;
                    }
                    else
                    {
                        // Lấy được dữ liệu
                        CPDetailInfo[] lst = datas.Detail;
                        foreach (CPDetailInfo info in lst)
                        {
                            if (info.RESULT_CODE == "1") // Lấy dữ liệu thành công
                            {
                                string maCp = info.CODE;
                                int tongSoThueBao = Convert.ToInt32(info.NO_SUBS);
                                decimal doanhThu = 0;
                                decimal.TryParse(info.TOTAL_CHARGE, out doanhThu);

                                EnumerableRowCollection<DataRow> rows = from myRow in tbl.AsEnumerable()
                                                                        where myRow.Field<string>("ServiceCode") != null && myRow.Field<string>("ServiceCode").ToString() == maCp
                                                                        select myRow;
                                List<DataRow> lstRows = rows.ToList();
                                if (lstRows.Count == 1) // Thực thế chỉ có thể = 1 vì Mã Code không trùng nhau
                                {
                                    lstRows[0]["TongSoKhachHangSD"] = tongSoThueBao;
                                    lstRows[0]["DoanhThu"] = doanhThu;

                                    DateTime cBaoCao = new DateTime(year, month, 15);
                                    if (cBaoCao > new DateTime(2016, 7, 1)) // Bắt đầu tính báo cáo từ 08-2016
                                    {
                                        decimal doanhThuCP = 0;
                                        decimal doanhThuVNP = 0;

                                        decimal.TryParse(info.CP_CHARGE, out doanhThuCP);
                                        decimal.TryParse(info.VNP_CHARGE, out doanhThuVNP);

                                        lstRows[0]["DoanhThuCP"] = doanhThuCP;
                                        lstRows[0]["DoanhThuVNP"] = doanhThuVNP;
                                    }
                                }
                            }
                        }
                    }
                }
                // Kết thúc lấy dữ liệu từ SV
                #endregion

                EnumDonViQuanLyHelper ctlEnumDonViQuanLy = new EnumDonViQuanLyHelper();

                if (isGetDataOK)
                {
                    // Lấy số lượng KN trên Solr
                    new ReportImpl().BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoan_Solr_Tu201605(month, year, tbl);
                    #region Hiển thị lưới dữ liệu

                    #region Xắp xếp hiển thị

                    var obj = from tab in tbl.AsEnumerable()
                              group tab by tab["Index"]
                              into groupData
                              select new
                              {
                                  GroupId = groupData.Key,
                                  MaxRow = groupData.Count()
                              };
                    #endregion

                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {

                        DataRow r = tbl.Rows[i];

                        content.AppendFormat("<tr><td>{0}</td>", i + 1);
                        content.AppendFormat("<td>{0}</td>", r["ServiceCode"]);

                        // Đơn vị quản lý
                        content.AppendFormat("<td>{0}</td>", string.Empty.GetData<string>(() =>
                        {
                            if (r["DonViQuanLyId"] != null && r["DonViQuanLyId"] != System.DBNull.Value)
                                return ctlEnumDonViQuanLy.GetNameByObject((int)r["DonViQuanLyId"]);
                            return string.Empty;
                        }));

                        string linhVucCon = r["TenLinhVucCon"].ToString().Trim();
                        int index = linhVucCon.IndexOf("_");
                        string dauSo = string.Empty;
                        if (index > 0)
                        {
                            dauSo = linhVucCon.Substring(0, index);
                            int number = 0;
                            string abc = dauSo[0].ToString();

                            // Kiểm tra xem có phải là số hay chữ
                            if (!int.TryParse(abc, out number))
                                dauSo = string.Empty;
                        }
                        // Tính KPI
                        int tongSoKhieuNai = r["TongSoKhieuNai"].ToString() != string.Empty ? Convert.ToInt32(r["TongSoKhieuNai"]) : 0;
                        int tongSoKhachHangSD = r["TongSoKhachHangSD"].ToString() != string.Empty ? Convert.ToInt32(r["TongSoKhachHangSD"]) : 0;

                        long rowNumber = (long)r["Number"];
                        if (rowNumber == 1) // Tính toán nhóm theo tên Công ty đối tác
                        {
                            long groupNumber = (long)r["Index"];
                            var rx = obj.Where(v => Convert.ToInt32(v.GroupId) == groupNumber).SingleOrDefault();
                            content.AppendFormat("<td rowspan=\"{1}\" style=\"vertical-align: middle;\" >{0}</td>", r["TenCongTyDoiTac"], rx.MaxRow);
                        }

                        content.AppendFormat("<td>{0}</td>", r["TenLinhVucCon"]);
                        content.AppendFormat("<td>{0}</td>", dauSo);

                        string tsknLink = string.Format("<a ids=\"{1}\" thang=\"2\" nam=\"{3}\" href=\"PopupDetail.aspx?Type=BaoCaoDichVuGTGTChoTapDoan&Ids={1}&Thang={2}&Nam={3}\" class=\"openthis\">{0}</a>", r["TongSoKhieuNai"], r["LinhVucConIds"].ToString(), month, year);

                        string tsKhieuNai = r["TongSoKhieuNai"].ToString() == string.Empty ? (tongSoKhachHangSD == 0 ? string.Empty : "-") : tsknLink;

                        content.AppendFormat("<td>{0}</td>", tsKhieuNai);

                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["TongSoKhachHangSD"], string.Empty));//  r["TongSoKhachHangSD"] != System.DBNull.Value ? (Convert.ToDouble(r["TongSoKhachHangSD"])).ToString() : string.Empty);

                        double kpi = 0;
                        if (tongSoKhachHangSD > 0)
                            kpi = tongSoKhieuNai / (tongSoKhachHangSD * (1.0));

                        if (tongSoKhieuNai == 0 && tongSoKhachHangSD == 0)
                            content.AppendFormat("<td>{0}</td>", string.Empty); // Cột KPI
                        else
                        {
                            if (tongSoKhieuNai == 0)
                                content.AppendFormat("<td>{0}</td>", "-"); // Tổng số KHSD <> 0
                            else
                            {
                                if (tongSoKhachHangSD == 0)
                                {
                                    content.AppendFormat("<td>{0}</td>", string.Empty);
                                }
                                else
                                {
                                    content.AppendFormat("<td>{0}</td>", string.Concat((kpi).ToString("P3", new CultureInfo("vi-VN"))));
                                }
                            }
                        }

                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : (kpi < 0.0005 ? "Không vi phạm" : "Vi phạm"));
                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : ((kpi >= 0.0005 && kpi < 0.0008) ? "Vi phạm mức 1" : "-"));

                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : ((kpi >= 0.0008 && kpi < 0.0015) ? "Vi phạm mức 2" : "-"));
                        content.AppendFormat("<td>{0}</td>", tongSoKhachHangSD == 0 ? string.Empty : ((kpi >= 0.0015) ? "Vi phạm mức 3" : "-"));
                        content.AppendFormat("<td>{0}</td>", string.Empty);
                        content.AppendFormat("<td>{0}</td>", string.Empty);

                        //content.AppendFormat("<td>{0}</td>", r["DoanhThu"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThu"])).ToString("N0", CultureInfo.CreateSpecificCulture("vi-VN")) : string.Empty);
                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["DoanhThu"], string.Empty)); // r["DoanhThu"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThu"])).ToString() : string.Empty);
                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["DoanhThuCP"], string.Empty)); // r["DoanhThuCP"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThuCP"])).ToString() : string.Empty);
                        content.AppendFormat("<td>{0}</td>", ProcessNumber(r["DoanhThuVNP"], string.Empty)); // r["DoanhThuVNP"] != System.DBNull.Value ? (Convert.ToDouble(r["DoanhThuVNP"])).ToString() : string.Empty);
                    }
                    #endregion
                }
                else
                {
                    Helper.GhiLogs("Lấy dữ liệu SV báo cáo DV GTTT cho tập đoàn bị lỗi Service, vui lòng kiểm tra lại");
                }

            }
            else
            {
                content.AppendFormat("<tr><td colspan=\"16\" style=\"text-align: center; font-weight: bold; padding: 20px; color: maroon; font-size: 14px;\">{0}</td></tr>", "Hiện không có dữ liệu");
            }
            return string.Format("<table class=\"tbl_style customized tblcus\" style=\"width:100%;\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">{0}{1}</table>", header.ToString(), content.ToString());
        }
        #endregion

        #region Báo cáo đối tác VNP

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 06/07/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoSoLuongChuyenXuLyVNPTheoNguoiDung(int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtResult = new ReportImpl().BaoCaoSoLuongChuyenXuLyVNPTheoNguoiDung(doiTacId, phongBanId, fromDate, toDate);
            if (dtResult != null)
            {
                string cellSoLuong = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaosoluongchuyenxulyvnp&doiTacId=" + doiTacId + "&phongBanId=" + phongBanId + "&nguoiXuLy={1}&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{0}</a></td>";
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Người dùng</th>");
                sb.Append("<th>Số lượng chuyển xử lý</th>");
                sb.Append("</tr>");

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat("<td>{0}</td>", dtResult.Rows[i]["NguoiXuLy"]);
                    sb.AppendFormat(cellSoLuong, ConvertUtility.ToInt32(dtResult.Rows[i]["SoLuong"], 0).ToString("###,###,###"), dtResult.Rows[i]["NguoiXuLy"]);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Báo cáo dùng chung nhiều đơn vị

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/11/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp của trung tâm tính cước
        /// </summary>       
        /// <param name="trungTamId"></param>        
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopDoiTac_V2(int doiTacId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoDoiTac_V2_Solr(doiTacId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitac&doiTacId=" + doiTacId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                    string emptyData = "<td>&nbsp;</td>";

                    DataRow row = dtReport.Rows[0];
                    sb.Append("<tr>");
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", row["TenDoiTac"].ToString());
                    if (row["SLTonDongKyTruoc"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                    }

                    if (row["SLTiepNhan"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                    }

                    if (row["SLTaoMoi"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 7, row["SLTaoMoi"].ToString());
                    }

                    if (row["SLDaXuLy"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                    }

                    //if (row["SLChuyenNgangHang"].ToString() == "0")
                    //{
                    //    sb.Append(emptyData);
                    //}
                    //else
                    //{
                    //    sb.AppendFormat(cellContent, 9, row["SLChuyenNgangHang"].ToString());
                    //}

                    if (row["SLChuyenXuLy"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                    }

                    if (row["SLChuyenPhanHoi"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                    }

                    if (row["SLDaDong"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                    }

                    if (row["SLQuaHanDaXuLy"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                    }

                    if (row["SLTonDong"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                    }

                    if (row["SLQuaHanTonDong"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                    }

                    sb.Append("</tr>");

                    //sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                    //sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                    //sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                    //sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                    //sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                    //sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                    //sb.AppendFormat("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/10/2014
        /// Todo : Báo cáo tổng hợp phòng ban
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="listPhongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopPAKNPhongBan_V2(int doiTacId, List<int> listPhongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanDoiTac_V2_Solr(doiTacId, listPhongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknphongban&doiTacId=" + doiTacId.ToString() + "&phongbanId=" + row["PhongBanId"].ToString() + "&tenPhongBan=" + row["TenPhongBan"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenPhongBan"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLTaoMoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 7, row["SLTaoMoi"].ToString());
                        }

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        //if (row["SLChuyenNgangHang"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 9, row["SLChuyenNgangHang"].ToString());
                        //}

                        if (row["SLChuyenXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                        }

                        if (row["SLChuyenPhanHoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                        }

                        if (row["SLDaDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='12'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='12'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/08/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp người dùng của từng phòng ban (dùng cho TT PTDV - không có tạo mới và đóng KN)
        /// </summary>        
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopNguoiDungPhongBan_V2(int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_V2_Solr(doiTacId, phongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    //sb.Append("<tr>");
                    //sb.Append("<th>STT</th>");
                    //sb.Append("<th>Tên truy cập</th>");
                    //sb.Append("<th>Tên người dùng</th>");
                    //sb.Append("<th>SL tồn kỳ trước</th>");
                    //sb.Append("<th>SL tiếp nhận</th>");
                    //sb.Append("<th>SL tạo mới</th>");
                    //sb.Append("<th>SL đã xử lý</th>");
                    //sb.Append("<th>SL đã đóng</th>");
                    //sb.Append("<th>SL đã xử lý quá hạn</th>");
                    //sb.Append("<th>SL tồn đọng</th>");
                    //sb.Append("<th>SL tồn đọng quá hạn</th>");
                    //sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidung&doiTacId=" + doiTacId.ToString() + "&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLTaoMoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 7, row["SLTaoMoi"].ToString());
                        }

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLChuyenNgangHang"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 9, row["SLChuyenNgangHang"].ToString());
                        }

                        if (row["SLChuyenXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                        }

                        if (row["SLChuyenPhanHoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                        }

                        if (row["SLDaDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/08/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp người dùng của từng phòng ban (dùng cho các đối tác có tạo mới/đóng khiếu nại)
        /// </summary>        
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopNguoiDungPhongBan_V3(int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_Solr_V3(doiTacId, phongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    //sb.Append("<tr>");
                    //sb.Append("<th>STT</th>");
                    //sb.Append("<th>Tên truy cập</th>");
                    //sb.Append("<th>Tên người dùng</th>");
                    //sb.Append("<th>SL tồn kỳ trước</th>");
                    //sb.Append("<th>SL tiếp nhận</th>");
                    //sb.Append("<th>SL tạo mới</th>");
                    //sb.Append("<th>SL đã xử lý</th>");
                    //sb.Append("<th>SL đã đóng</th>");
                    //sb.Append("<th>SL đã xử lý quá hạn</th>");
                    //sb.Append("<th>SL tồn đọng</th>");
                    //sb.Append("<th>SL tồn đọng quá hạn</th>");
                    //sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidungv3&doiTacId=" + doiTacId.ToString() + "&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLTaoMoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 7, row["SLTaoMoi"].ToString());
                        }

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLChuyenNgangHang"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 9, row["SLChuyenNgangHang"].ToString());
                        }

                        if (row["SLChuyenXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                        }

                        if (row["SLChuyenPhanHoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                        }

                        if (row["SLDaDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        public string BaoCaoTongHopNguoiDungPhongBan_V4(int doiTacId, int phongBanId, DateTime fromDate, DateTime toDate, int tinhId, int huyenId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDungCuaPhongBan_Solr_V4(doiTacId, phongBanId, fromDate, toDate, tinhId, huyenId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    //sb.Append("<tr>");
                    //sb.Append("<th>STT</th>");
                    //sb.Append("<th>Tên truy cập</th>");
                    //sb.Append("<th>Tên người dùng</th>");
                    //sb.Append("<th>SL tồn kỳ trước</th>");
                    //sb.Append("<th>SL tiếp nhận</th>");
                    //sb.Append("<th>SL tạo mới</th>");
                    //sb.Append("<th>SL đã xử lý</th>");
                    //sb.Append("<th>SL đã đóng</th>");
                    //sb.Append("<th>SL đã xử lý quá hạn</th>");
                    //sb.Append("<th>SL tồn đọng</th>");
                    //sb.Append("<th>SL tồn đọng quá hạn</th>");
                    //sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?FromPage=BaoCaoTongHopPAKNNguoiDungV4&DoiTacId="
                            + doiTacId.ToString()
                            + "&phongbanId=" + phongBanId.ToString()
                            + "&TenTruyCap=" + row["TenTruyCap"].ToString()
                            + "&FromDate=" + fromDate.ToString("dd/MM/yyyy")
                            + "&ToDate=" + toDate.ToString("dd/MM/yyyy")
                            + string.Format("&TinhId={0}&HuyenId={1}", tinhId, huyenId)
                            + "&ReportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";

                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLTaoMoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 7, row["SLTaoMoi"].ToString());
                        }

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLChuyenNgangHang"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 9, row["SLChuyenNgangHang"].ToString());
                        }

                        if (row["SLChuyenXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                        }

                        if (row["SLChuyenPhanHoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                        }

                        if (row["SLDaDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/12/2014
        /// Todo : Hiển thị nội dung báo cáo cá nhân
        /// Chỉnh sửa
        /// Dương Dv
        /// Ngày 29.07.2016
        /// Thêm trường lọc báo cáo tỉnh và huyện
        /// </summary>
        public string BaoCaoTongHopPAKNTheoNguoiDung(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int tinhId, int huyenId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                // int doiTacId = -1;
                // int phongBanId = -1;  
                // DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDung_Solr(tenTruyCap, fromDate, toDate);
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoNguoiDung_Solr_Ver2(tenTruyCap, fromDate, toDate, tinhId, huyenId);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    //sb.Append("<tr>");
                    //sb.Append("<th>STT</th>");
                    //sb.Append("<th>Tên truy cập</th>");
                    //sb.Append("<th>Tên người dùng</th>");
                    //sb.Append("<th>SL tồn kỳ trước</th>");
                    //sb.Append("<th>SL tiếp nhận</th>");
                    //sb.Append("<th>SL tạo mới</th>");
                    //sb.Append("<th>SL đã xử lý</th>");
                    //sb.Append("<th>SL đã đóng</th>");
                    //sb.Append("<th>SL đã xử lý quá hạn</th>");
                    //sb.Append("<th>SL tồn đọng</th>");
                    //sb.Append("<th>SL tồn đọng quá hạn</th>");
                    //sb.Append("</tr>");

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        #region Region
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknnguoidung&doiTacId=" + doiTacId.ToString() + "&phongbanId=" + phongBanId.ToString() + "&tenTruyCap=" + row["TenTruyCap"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenTruyCap"]);
                        sb.AppendFormat("<td >{0}</td>", row["TenDayDu"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLTaoMoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 7, row["SLTaoMoi"].ToString());
                        }

                        if (row["SLDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        }

                        if (row["SLChuyenNgangHang"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 9, row["SLChuyenNgangHang"].ToString());
                        }

                        if (row["SLChuyenXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                        }

                        if (row["SLChuyenPhanHoi"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                        }

                        if (row["SLDaDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                        }

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;

                        #endregion
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch (Exception ex)
            {
                sb.AppendFormat(@"<tr>
                                <td colspan='11'>
                                    Xử lý dữ liệu bị lỗi: {0}
                                </td>
                            </tr>", ex.Message);
                Helper.GhiLogs(ex);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/02/2015
        /// Todo : Hiển thị báo cáo tồn đọng và quá hạn của người dùng thuộc phòng ban
        /// </summary>        
        /// <param name="phongBanId"></param>
        /// <param name="nguoiXuLy"></param>
        /// <returns></returns>
        public string BaoCaoTonDongVaQuaHanNguoiDungPhongBan(int phongBanId, string nguoiXuLy, bool isExportExcel)
        {
            StringBuilder sb = new StringBuilder();
            string formatNumber = string.Empty;
            if (!isExportExcel)
            {
                formatNumber = FORMAT_NUMBER;
            }

            try
            {
                DataTable dtReport = new ReportSqlImpl().CountKhieuNaiTonDongVaQuaHanHienTaiCuaNguoiDungPhongBan(phongBanId, nguoiXuLy);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    int totalSoLuongQuaHan = 0;
                    int totalSoLuongTonDong = 0;

                    string cellTotalContent = "<td align='center' class='borderThinTextBold'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_common_baocaotondongnguoidungphongban&phongBanXuLyId={0}&nguoiXuLy={1}&reportType={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{3}</a></td>";
                    string cellContent = "<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=bc_common_baocaotondongnguoidungphongban&phongBanXuLyId={0}&nguoiXuLy={1}&reportType={2}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{3}</a></td>";

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("<th>Số lượng tồn đọng</th>");
                    sb.Append("<th>Số lượng quá hạn</th>");
                    sb.Append("</tr>");
                    for (int i = 0; i < dtReport.Rows.Count; i++)
                    {
                        DataRow row = dtReport.Rows[i];

                        int soLuongTonDong = ConvertUtility.ToInt32(row["SoLuongTonDong"], 0);
                        int soLuongQuaHan = ConvertUtility.ToInt32(row["SoLuongQuaHan"], 0);

                        totalSoLuongTonDong += soLuongTonDong;
                        totalSoLuongQuaHan += soLuongQuaHan;

                        sb.Append("<tr>");
                        sb.Append("<td align='center' class='borderThin' >" + (i + 1) + "</td>");
                        sb.Append("<td class='borderThin'>" + row["TenTruyCap"].ToString() + "</td>");
                        sb.AppendFormat(cellContent, phongBanId, row["TenTruyCap"], 1, soLuongTonDong.ToString(formatNumber));
                        sb.AppendFormat(cellContent, phongBanId, row["TenTruyCap"], 2, soLuongQuaHan.ToString(formatNumber));
                        sb.Append("</tr>");
                    }

                    if (nguoiXuLy.Trim().Length == 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' align='center' class='borderThinTextBold'>TỔNG</td>");
                        sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", totalSoLuongTonDong.ToString(formatNumber));
                        sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", totalSoLuongQuaHan.ToString(formatNumber));

                        sb.Append("</tr>");
                    }

                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='4'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/10/2015
        /// Todo : Hiển thị nội dung báo cáo tổng hợp của VNPT X (NET, Media)
        /// </summary>       
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopVNPTX_V2(int doiTacId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoVNPTX_Solr(doiTacId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {

                    string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknvnptx&doiTacId=" + doiTacId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                    string emptyData = "<td>&nbsp;</td>";

                    DataRow row = dtReport.Rows[0];
                    sb.Append("<tr>");
                    sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", row["TenDoiTac"].ToString());
                    if (row["SLTonDongKyTruoc"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                    }

                    if (row["SLTiepNhan"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                    }

                    if (row["SLDaXuLyTiepNhan"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 7, row["SLDaXuLyTiepNhan"].ToString());
                    }

                    if (row["SLDaXuLyLuyKe"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 3, row["SLDaXuLyLuyKe"].ToString());
                    }

                    //if (row["SLChuyenXuLy"].ToString() == "0")
                    //{
                    //    sb.Append(emptyData);
                    //}
                    //else
                    //{
                    //    sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                    //}

                    //if (row["SLChuyenPhanHoi"].ToString() == "0")
                    //{
                    //    sb.Append(emptyData);
                    //}
                    //else
                    //{
                    //    sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                    //}

                    //if (row["SLDaDong"].ToString() == "0")
                    //{
                    //    sb.Append(emptyData);
                    //}
                    //else
                    //{
                    //    sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                    //}

                    if (row["SLQuaHanDaXuLy"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                    }

                    if (row["SLTonDong"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                    }

                    if (row["SLQuaHanTonDong"].ToString() == "0")
                    {
                        sb.Append(emptyData);
                    }
                    else
                    {
                        sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                    }

                    sb.Append("</tr>");

                    //sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                    //sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                    //sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                    //sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                    //sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                    //sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                    //sb.AppendFormat("</tr>");
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/11/2014
        /// Todo : Hiển thị nội dung báo cáo tổng hợp của trung tâm tính cước
        /// </summary>       
        /// <param name="trungTamId"></param>        
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopDoiTac_V2_NET(int khuVucId, List<int> listDoiTacId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoDoiTac_V2_NET_Solr(khuVucId, listDoiTacId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppakndoitacvnptnet&doiTacId=" + row["DoiTacId"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        sb.AppendFormat("<td align='center' class='borderThinTextBold'>{0}</td>", row["TenDoiTac"].ToString());
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLDaXuLyTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 7, row["SLDaXuLyTiepNhan"].ToString());
                        }


                        if (row["SLDaXuLyLuyKe"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLyLuyKe"].ToString());
                        }

                        //if (row["SLChuyenXuLy"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                        //}

                        //if (row["SLChuyenPhanHoi"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                        //}

                        //if (row["SLDaDong"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                        //}

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        //sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        //sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        //sb.AppendFormat(cellContent, 3, row["SLDaXuLy"].ToString());
                        //sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        //sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        //sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        //sb.AppendFormat("</tr>");
                    } // end foreach(DataRow row in dtReport.Rows)

                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='11'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/10/2014
        /// Todo : Báo cáo tổng hợp phòng ban
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="listPhongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string BaoCaoTongHopPAKNPhongBan_V2_NET(int doiTacId, List<int> listPhongBanId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataTable dtReport = new ReportImpl().BaoCaoTongHopPAKNTheoPhongBanDoiTac_V2_NET_Solr(doiTacId, listPhongBanId, fromDate, toDate);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    int index = 1;
                    foreach (DataRow row in dtReport.Rows)
                    {
                        string cellContent = "<td><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoppaknphongbanvnptnet&doiTacId=" + doiTacId.ToString() + "&phongbanId=" + row["PhongBanId"].ToString() + "&tenPhongBan=" + row["TenPhongBan"].ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&reportType={0}','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">{1}</a></td>";
                        string emptyData = "<td>&nbsp;</td>";

                        sb.Append("<tr>");
                        //sb.AppendFormat("<td align='center'>{0}</td>", index);
                        sb.AppendFormat("<td >{0}</td>", row["TenPhongBan"]);
                        if (row["SLTonDongKyTruoc"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 1, row["SLTonDongKyTruoc"].ToString());
                        }

                        if (row["SLTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 2, row["SLTiepNhan"].ToString());
                        }

                        if (row["SLDaXuLyTiepNhan"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 7, row["SLDaXuLyTiepNhan"].ToString());
                        }

                        if (row["SLDaXuLyLuyKe"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 3, row["SLDaXuLyLuyKe"].ToString());
                        }

                        //if (row["SLChuyenNgangHang"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 9, row["SLChuyenNgangHang"].ToString());
                        //}

                        //if (row["SLChuyenXuLy"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 10, row["SLChuyenXuLy"].ToString());
                        //}

                        //if (row["SLChuyenPhanHoi"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 11, row["SLChuyenPhanHoi"].ToString());
                        //}

                        //if (row["SLDaDong"].ToString() == "0")
                        //{
                        //    sb.Append(emptyData);
                        //}
                        //else
                        //{
                        //    sb.AppendFormat(cellContent, 8, row["SLDaDong"].ToString());
                        //}

                        if (row["SLQuaHanDaXuLy"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 4, row["SLQuaHanDaXuLy"].ToString());
                        }

                        if (row["SLTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 5, row["SLTonDong"].ToString());
                        }

                        if (row["SLQuaHanTonDong"].ToString() == "0")
                        {
                            sb.Append(emptyData);
                        }
                        else
                        {
                            sb.AppendFormat(cellContent, 6, row["SLQuaHanTonDong"].ToString());
                        }

                        sb.Append("</tr>");

                        index++;
                    }
                }
                else
                {
                    sb.Append(@"<tr>
                                <td colspan='12'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
                }
            }
            catch
            {
                sb.Append(@"<tr>
                                <td colspan='12'>
                                    Chưa có dữ liệu báo cáo
                                </td>
                            </tr>");
            }

            return sb.ToString();
        }

        #endregion

        #region Biểu đồ

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/04/2014
        /// Todo : Thống kê số lượng khiếu nại theo các tiêu chí
        /// </summary>
        /// <param name="reportType">
        ///     1 : Thống kê theo loại khiếu nại
        ///     2 : Thống kê theo lĩnh vực chung
        ///     3 : Thống kê theo lĩnh vực con
        /// </param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="doiTacId">
        ///     = -1 : Thống kê toàn bộ VNP
        ///     = 2 : Thống kê theo VNP1
        ///     = 3 : Thống kê theo VNP2
        ///     = 5 : Thống kê theo VNP3
        /// </param>
        /// <param name="loaiThueBao">
        ///     = -1 : Tất cả các loại thuê bao
        ///     = 0 : Thuê bao trả trước
        ///     = 1 : Thuê bao trả sau
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataTable BaoCaoBieuDoSoLuongGQKNTheoMotKhoangThoiGian(byte reportType, string listLoaiKhieuNaiId, string listLinhVucChungId, string listLinhVucConId, int doiTacId, int loaiThueBao, DateTime fromDate, DateTime toDate)
        {
            DataTable dtReport = new ReportImpl().BaoCaoBieuDoSoLuongGQKNTheoMotKhoangThoiGian_Solr(reportType, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, doiTacId, loaiThueBao, fromDate, toDate);

            return dtReport;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/04/2014
        /// Todo : Thống kê số lượng khiếu nại theo các tiêu chí
        /// </summary>
        /// <param name="reportType">
        ///     1 : Thống kê theo loại khiếu nại
        ///     2 : Thống kê theo lĩnh vực chung
        ///     3 : Thống kê theo lĩnh vực con
        /// </param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="doiTacId">
        ///     = -1 : Thống kê toàn bộ VNP
        ///     = 2 : Thống kê theo VNP1
        ///     = 3 : Thống kê theo VNP2
        ///     = 5 : Thống kê theo VNP3
        /// </param>
        /// <param name="loaiThueBao">
        ///     = -1 : Tất cả các loại thuê bao
        ///     = 0 : Thuê bao trả trước
        ///     = 1 : Thuê bao trả sau
        /// </param>
        /// <param name="fromDate1"></param>
        /// <param name="toDate1"></param>
        /// <param name="fromDate2"></param>
        /// <param name="toDate2"></param>
        /// <returns></returns>
        public DataTable BaoCaoBieuDoSoLuongGQKNTheoHaiKhoangThoiGian(byte reportType, string listLoaiKhieuNaiId, string listLinhVucChungId, string listLinhVucConId, int doiTacId, int loaiThueBao, string listDate)
        {
            DataTable dtResult = new ReportImpl().BaoCaoBieuDoSoLuongGQKNTheoHaiKhoangThoiGian_Solr(reportType, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, doiTacId, loaiThueBao, listDate);

            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2014
        /// Todo : Thống kê số lượng khiếu nại theo các tiêu chí
        /// </summary>
        /// <param name="reportType">
        ///     1 : Thống kê theo loại khiếu nại
        ///     2 : Thống kê theo lĩnh vực chung
        ///     3 : Thống kê theo lĩnh vực con
        /// </param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="doiTacId">
        ///     = -1 : Thống kê toàn bộ VNP
        ///     = 2 : Thống kê theo VNP1
        ///     = 3 : Thống kê theo VNP2
        ///     = 5 : Thống kê theo VNP3
        /// </param>
        /// <param name="loaiThueBao">
        ///     = -1 : Tất cả các loại thuê bao
        ///     = 0 : Thuê bao trả trước
        ///     = 1 : Thuê bao trả sau
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataTable BaoCaoBieuDoTongSoTienGiamTruTheoMotKhoangThoiGian(byte reportType, string listLoaiKhieuNaiId, string listLinhVucChungId, string listLinhVucConId, int doiTacId, int loaiThueBao, DateTime fromDate, DateTime toDate)
        {
            DataTable dtReport = new ReportImpl().BaoCaoBieuDoTongSoTienGiamTruTheoMotKhoangThoiGian_Solr(reportType, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, doiTacId, loaiThueBao, fromDate, toDate);

            return dtReport;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/04/2014
        /// Todo : Thống kê số lượng khiếu nại theo các tiêu chí
        /// </summary>
        /// <param name="reportType">
        ///     1 : Thống kê theo loại khiếu nại
        ///     2 : Thống kê theo lĩnh vực chung
        ///     3 : Thống kê theo lĩnh vực con
        /// </param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="doiTacId">
        ///     = -1 : Thống kê toàn bộ VNP
        ///     = 2 : Thống kê theo VNP1
        ///     = 3 : Thống kê theo VNP2
        ///     = 5 : Thống kê theo VNP3
        /// </param>
        /// <param name="loaiThueBao">
        ///     = -1 : Tất cả các loại thuê bao
        ///     = 0 : Thuê bao trả trước
        ///     = 1 : Thuê bao trả sau
        /// </param>
        /// <param name="fromDate1"></param>
        /// <param name="toDate1"></param>
        /// <param name="fromDate2"></param>
        /// <param name="toDate2"></param>
        /// <returns></returns>
        public DataTable BaoCaoBieuDoTongSoTienGiamTruTheoHaiKhoangThoiGian(byte reportType, string listLoaiKhieuNaiId, string listLinhVucChungId, string listLinhVucConId, int doiTacId, int loaiThueBao, string listDate)
        {
            DataTable dtResult = new ReportImpl().BaoCaoBieuDoTongSoTienGiamTruTheoHaiKhoangThoiGian_Solr(reportType, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, doiTacId, loaiThueBao, listDate);

            return dtResult;
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/09/2013
        /// Todo : In dữ liệu báo cáo tổng hợp khiếu nại đối với các lĩnh vực chung
        /// </summary>
        /// <param name="index"></param>
        /// <param name="listDoiTacId"></param>
        /// <param name="dtLinhVucChung"></param>
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId">Id của Loại khiếu nại cha</param>
        /// <param name="totalPAKNOfPhongBan">
        ///     Biến dùng để xác định tổng số phản ánh khiếu nại đối với phòng ban đang xét
        ///     Biến này chỉ được cộng thêm giá trị khi giá trị parentId == "" (vì nếu parentId có giá trị thì biến này đã được thêm giá trị rồi)
        /// </param>
        /// <returns></returns>
        private StringBuilder BaoCaoTongHopTheoKhieuNai_DisplayLinhVucChung(ref int index, List<string> listDoiTacId, DataTable dtLinhVucChung, DataTable dtLinhVucCon,
                                                                                string parentId, ref int totalPAKNOfPhongBan, string sWindowOpen, string sWindowOpenDanhSachKNChuaDong)
        {
            StringBuilder sb = new StringBuilder();

            if (parentId.Length > 0)
            {
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    if (row["ParentId"].ToString() == parentId)
                    {
                        sb.Append("<tr>");
                        //sb.Append("<td class='borderThin' align='center'>" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");

                        int total = 0;
                        for (int indexDoiTac = 0; indexDoiTac < listDoiTacId.Count; indexDoiTac++)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + (row[listDoiTacId[indexDoiTac].ToString()].ToString() == "0" ? "&nbsp;" : row[listDoiTacId[indexDoiTac].ToString()].ToString()) + "</td>");
                            total += ConvertUtility.ToInt32(row[listDoiTacId[indexDoiTac].ToString()], 0);
                        }
                        sb.Append("<td align='center' class='borderThin'>" + (total.ToString() == "0" ? "&nbsp;" : total.ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNGiaiQuyetDuoc"].ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpen, row["LinhVucChungId"], (row["TongSoPAKNChuyenDonViLienQuan"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNChuyenDonViLienQuan"].ToString())) + "</td>");

                        //sb.Append("<td align='center' class='borderThin'>" + (row["DonViNhanChuyenTiepKhieuNai"].ToString() == "0" ? "&nbsp;" : row["DonViNhanChuyenTiepKhieuNai"].ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["TongSoKhieuNaiTonDongDoQuaHan"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongDoQuaHan"].ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpenDanhSachKNChuaDong, row["LinhVucChungId"], (row["TongSoKhieuNaiTonDongHienTai"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongHienTai"].ToString())) + "</td>");
                        sb.Append("</tr>");

                        index++;

                        StringBuilder sbLinhVucCon = BaoCaoTongHopTheoKhieuNai_DisplayLinhVucCon(ref index, listDoiTacId, dtLinhVucCon, row["LinhVucChungId"].ToString(), ref totalPAKNOfPhongBan, sWindowOpen, sWindowOpenDanhSachKNChuaDong);
                        if (sbLinhVucCon != null)
                        {
                            sb.Append(sbLinhVucCon.ToString());
                        }

                        dtLinhVucChung.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    int curIndex = index;

                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center' valign='top' rowspan='rowspanX'>" + index.ToString() + "</td>");
                    sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");

                    int total = 0;
                    for (int indexDoiTac = 0; indexDoiTac < listDoiTacId.Count; indexDoiTac++)
                    {
                        sb.Append("<td align='center' class='borderThin'>" + (row[listDoiTacId[indexDoiTac].ToString()].ToString() == "0" ? "&nbsp;" : row[listDoiTacId[indexDoiTac].ToString()].ToString()) + "</td>");
                        total += ConvertUtility.ToInt32(row[listDoiTacId[indexDoiTac].ToString()], 0);
                    }
                    totalPAKNOfPhongBan += total;
                    sb.Append("<td align='center' class='borderThin'>" + (total.ToString() == "0" ? "&nbsp;" : total.ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNGiaiQuyetDuoc"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpen, row["LinhVucChungId"], (row["TongSoPAKNChuyenDonViLienQuan"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNChuyenDonViLienQuan"].ToString())) + "</td>");
                    //sb.Append("<td align='center' class='borderThin' >" + (row["DonViNhanChuyenTiepKhieuNai"].ToString() == "0" ? "&nbsp;" : row["DonViNhanChuyenTiepKhieuNai"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (row["TongSoKhieuNaiTonDongDoQuaHan"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongDoQuaHan"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpenDanhSachKNChuaDong, row["LinhVucChungId"], (row["TongSoKhieuNaiTonDongHienTai"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongHienTai"].ToString())) + "</td>");
                    sb.Append("</tr>");

                    //index++;

                    StringBuilder sbLinhVucCon = BaoCaoTongHopTheoKhieuNai_DisplayLinhVucCon(ref index, listDoiTacId, dtLinhVucCon, row["LinhVucChungId"].ToString(), ref totalPAKNOfPhongBan, sWindowOpen, sWindowOpenDanhSachKNChuaDong);
                    if (sbLinhVucCon != null)
                    {
                        sb.Append(sbLinhVucCon.ToString());
                    }

                    dtLinhVucChung.Rows.RemoveAt(i);
                    i--;

                    int totalRow = index - curIndex + 1;
                    sb.Replace("rowspanX", totalRow.ToString());
                    index = curIndex + 1;
                }
            }

            return sb;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/09/2013
        /// Todo : In dữ liệu báo cáo tổng hợp khiếu nại đối với các lĩnh vực con
        /// </summary>
        /// <param name="index"></param>
        /// <param name="listDoiTacId"></param>        
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId">Id của Loại khiếu nại lĩnh vực chung</param>
        /// <param name="totalPAKNOfPhongBan">
        ///     Biến dùng để xác định tổng số phản ánh khiếu nại đối với phòng ban đang xét
        ///     Biến này chỉ được cộng thêm giá trị khi giá trị parentId == "" (vì nếu parentId có giá trị thì biến này đã được thêm giá trị rồi)
        /// </param>
        /// <returns></returns>
        private StringBuilder BaoCaoTongHopTheoKhieuNai_DisplayLinhVucCon(ref int index, List<string> listDoiTacId, DataTable dtLinhVucCon,
                                                                            string parentId, ref int totalPAKNOfPhongBan, string sWindowOpen, string sWindowOpenDanhSachKNChuaDong)
        {
            StringBuilder sb = new StringBuilder();

            if (parentId.Length > 0)
            {
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];
                    if (row["ParentId"].ToString() == parentId)
                    {
                        sb.Append("<tr>");
                        //sb.Append("<td class='borderThin' align='center'>" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");

                        int total = 0;
                        for (int indexDoiTac = 0; indexDoiTac < listDoiTacId.Count; indexDoiTac++)
                        {
                            sb.Append("<td align='center' class='borderThin'>" + (row[listDoiTacId[indexDoiTac].ToString()].ToString() == "0" ? "&nbsp;" : row[listDoiTacId[indexDoiTac].ToString()].ToString()) + "</td>");
                            total += ConvertUtility.ToInt32(row[listDoiTacId[indexDoiTac].ToString()], 0);
                        }
                        sb.Append("<td align='center' class='borderThin'>" + (total.ToString() == "0" ? "&nbsp;" : total.ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNGiaiQuyetDuoc"].ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpen, row["LinhVucConId"], (row["TongSoPAKNChuyenDonViLienQuan"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNChuyenDonViLienQuan"].ToString())) + "</td>");
                        //sb.Append("<td align='center' class='borderThin'>" + (row["DonViNhanChuyenTiepKhieuNai"].ToString() == "0" ? "&nbsp;" : row["DonViNhanChuyenTiepKhieuNai"].ToString()) + "</td>");
                        sb.Append("<td  align='center'class='borderThin'>" + (row["TongSoKhieuNaiTonDongDoQuaHan"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongDoQuaHan"].ToString()) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpenDanhSachKNChuaDong, row["LinhVucConId"], (row["TongSoKhieuNaiTonDongHienTai"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongHienTai"].ToString())) + "</td>");
                        sb.Append("</tr>");

                        index++;

                        dtLinhVucCon.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];

                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center'>" + index.ToString() + "</td>");
                    sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");

                    int total = 0;
                    for (int indexDoiTac = 0; indexDoiTac < listDoiTacId.Count; indexDoiTac++)
                    {
                        sb.Append("<td align='center' class='borderThin'>" + (row[listDoiTacId[indexDoiTac].ToString()].ToString() == "0" ? "&nbsp;" : row[listDoiTacId[indexDoiTac].ToString()].ToString()) + "</td>");
                        total += ConvertUtility.ToInt32(row[listDoiTacId[indexDoiTac].ToString()], 0);
                    }
                    totalPAKNOfPhongBan += total;
                    sb.Append("<td align='center' class='borderThin'>" + (total.ToString() == "0" ? "&nbsp;" : total.ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNGiaiQuyetDuoc"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNToXLNVGiaiQuyetDuoc"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpen, row["LinhVucConId"], (row["TongSoPAKNChuyenDonViLienQuan"].ToString() == "0" ? "&nbsp;" : row["TongSoPAKNChuyenDonViLienQuan"].ToString())) + "</td>");
                    //sb.Append("<td align='center' class='borderThin'>" + (row["DonViNhanChuyenTiepKhieuNai"].ToString() == "0" ? "&nbsp;" : row["DonViNhanChuyenTiepKhieuNai"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (row["TongSoKhieuNaiTonDongDoQuaHan"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongDoQuaHan"].ToString()) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + string.Format(sWindowOpenDanhSachKNChuaDong, row["LinhVucConId"], (row["TongSoKhieuNaiTonDongHienTai"].ToString() == "0" ? "&nbsp;" : row["TongSoKhieuNaiTonDongHienTai"].ToString())) + "</td>");
                    sb.Append("</tr>");

                    index++;

                    dtLinhVucCon.Rows.RemoveAt(i);
                    i--;
                }
            }

            return sb;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/09/2013
        /// Todo : In dữ liệu báo cáo tổng hợp khiếu nại đối với các lĩnh vực chung
        /// </summary>
        /// <param name="index"></param>
        /// <param name="listDoiTacId"></param>
        /// <param name="dtLinhVucChung"></param>
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId">Id của Loại khiếu nại cha</param>
        /// <param name="totalPAKNOfPhongBan">
        ///     Biến dùng để xác định tổng số phản ánh khiếu nại đối với phòng ban đang xét
        ///     Biến này chỉ được cộng thêm giá trị khi giá trị parentId == "" (vì nếu parentId có giá trị thì biến này đã được thêm giá trị rồi)
        /// </param>
        /// <returns></returns>
        private StringBuilder BaoCaoTheoLoaiKhieuNai_DisplayLinhVucChung(int index, DataTable dtLinhVucChung, DataTable dtLinhVucCon, string parentId)
        {
            StringBuilder sb = new StringBuilder();

            if (parentId.Length > 0)
            {
                int indexLinhVucChung = 1;
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    if (row["ParentId"].ToString() == parentId)
                    {
                        sb.Append("<tr>");
                        sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}</td>", index, indexLinhVucChung));
                        sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");

                        sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                        sb.Append("</tr>");


                        StringBuilder sbLinhVucCon = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucCon(index, i + 1, dtLinhVucCon, row["LinhVucChungId"].ToString());
                        if (sbLinhVucCon != null)
                        {
                            sb.Append(sbLinhVucCon.ToString());
                        }

                        indexLinhVucChung++;

                        dtLinhVucChung.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }   // end if (parentId.Length > 0)
            else
            {
                int indexLinhVucChung = 1;
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    sb.Append("<tr>");
                    sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}</td>", index, indexLinhVucChung));
                    sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");

                    sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                    sb.Append("</tr>");


                    StringBuilder sbLinhVucCon = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucCon(index, i + 1, dtLinhVucCon, row["LinhVucChungId"].ToString());
                    if (sbLinhVucCon != null)
                    {
                        sb.Append(sbLinhVucCon.ToString());
                    }

                    indexLinhVucChung++;

                    dtLinhVucChung.Rows.RemoveAt(i);
                    i--;
                }
            }

            return sb;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/09/2013
        /// Todo : In dữ liệu báo cáo theo khiếu nại đối với các lĩnh vực con
        /// </summary>
        /// <param name="index"></param>
        /// <param name="listDoiTacId"></param>        
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId">Id của Loại khiếu nại lĩnh vực chung</param>
        /// <param name="totalPAKNOfPhongBan">
        ///     Biến dùng để xác định tổng số phản ánh khiếu nại đối với phòng ban đang xét
        ///     Biến này chỉ được cộng thêm giá trị khi giá trị parentId == "" (vì nếu parentId có giá trị thì biến này đã được thêm giá trị rồi)
        /// </param>
        /// <returns></returns>
        private StringBuilder BaoCaoTheoLoaiKhieuNai_DisplayLinhVucCon(int indexLoaiKhieuNai, int indexLinhVucChung, DataTable dtLinhVucCon, string parentId)
        {
            StringBuilder sb = new StringBuilder();

            if (parentId.Length > 0)
            {
                int indexLinhVucCon = 1;
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];
                    if (row["ParentId"].ToString() == parentId)
                    {
                        sb.Append("<tr>");
                        sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}.{2}</td>", indexLoaiKhieuNai, indexLinhVucChung, indexLinhVucCon));
                        sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");

                        sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                        sb.Append("</tr>");

                        indexLinhVucCon++;

                        dtLinhVucCon.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }  // end if (parentId.Length > 0)
            else
            {
                int indexLinhVucCon = 1;
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];
                    sb.Append("<tr>");
                    sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}.{2}</td>", indexLoaiKhieuNai, indexLinhVucChung, indexLinhVucCon));
                    sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");

                    sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                    sb.Append("</tr>");

                    indexLinhVucCon++;

                    dtLinhVucCon.Rows.RemoveAt(i);
                    i--;
                }
            }

            return sb;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/06/2014
        /// Todo : Hiển thị theo lĩnh vực chung (có link)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dtLinhVucChung"></param>
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId"></param>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private StringBuilder BaoCaoTheoLoaiKhieuNai_DisplayLinhVucChung_HasLink(int index, DataTable dtLinhVucChung, DataTable dtLinhVucCon, string parentId, int phongBanXuLyId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            if (parentId.Length > 0)
            {
                int indexLinhVucChung = 1;
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    if (row["ParentId"].ToString() == parentId)
                    {
                        sb.Append("<tr>");
                        sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}</td>", index, indexLinhVucChung));
                        sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");

                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=3','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=4','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                        sb.Append("</tr>");


                        StringBuilder sbLinhVucCon = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucCon_HasLink(index, i + 1, dtLinhVucCon, row["LinhVucChungId"].ToString(), phongBanXuLyId, fromDate, toDate);
                        if (sbLinhVucCon != null)
                        {
                            sb.Append(sbLinhVucCon.ToString());
                        }

                        indexLinhVucChung++;

                        dtLinhVucChung.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }   // end if (parentId.Length > 0)
            else
            {
                int indexLinhVucChung = 1;
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    sb.Append("<tr>");
                    sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}</td>", index, indexLinhVucChung));
                    sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");

                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=3','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucChungId"].ToString() + "&loaiKhieuNaiType=2&reportType=4','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                    sb.Append("</tr>");


                    StringBuilder sbLinhVucCon = BaoCaoTheoLoaiKhieuNai_DisplayLinhVucCon_HasLink(index, i + 1, dtLinhVucCon, row["LinhVucChungId"].ToString(), phongBanXuLyId, fromDate, toDate);
                    if (sbLinhVucCon != null)
                    {
                        sb.Append(sbLinhVucCon.ToString());
                    }

                    indexLinhVucChung++;

                    dtLinhVucChung.Rows.RemoveAt(i);
                    i--;
                }
            }

            return sb;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/06/2014
        /// Todo : Hiển thị theo lĩnh vực con (có link)
        /// </summary>
        /// <param name="indexLoaiKhieuNai"></param>
        /// <param name="indexLinhVucChung"></param>
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId"></param>
        /// <param name="phongBanXuLyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private StringBuilder BaoCaoTheoLoaiKhieuNai_DisplayLinhVucCon_HasLink(int indexLoaiKhieuNai, int indexLinhVucChung, DataTable dtLinhVucCon, string parentId, int phongBanXuLyId, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            if (parentId.Length > 0)
            {
                int indexLinhVucCon = 1;
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];
                    if (row["ParentId"].ToString() == parentId)
                    {
                        sb.Append("<tr>");
                        sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}.{2}</td>", indexLoaiKhieuNai, indexLinhVucChung, indexLinhVucCon));
                        sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");

                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=3','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=4','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</a></td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                        sb.Append("</tr>");

                        indexLinhVucCon++;

                        dtLinhVucCon.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }  // end if (parentId.Length > 0)
            else
            {
                int indexLinhVucCon = 1;
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];
                    sb.Append("<tr>");
                    sb.Append(string.Format("<td class='borderThin' align='center'>{0}.{1}.{2}</td>", indexLoaiKhieuNai, indexLinhVucChung, indexLinhVucCon));
                    sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");

                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=1','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNDaGiaiQuyetDenDauTuanX"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=2','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["LuyKeKNTonDongDenDauTuanX"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=3','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["SoLuongTiepNhanTrongTuan"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'><a href='#' onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?fromPage=baocaotonghoptogqkn&phongBanId=" + phongBanXuLyId.ToString() + "&fromDate=" + fromDate.ToString("dd/MM/yyyy") + "&toDate=" + toDate.ToString("dd/MM/yyyy") + "&loaiKhieuNaiId=" + row["LinhVucConId"].ToString() + "&loaiKhieuNaiType=3&reportType=4','_blank', 'width=980, height=550,scrollbars=1,location=0'); return false;\">" + row["TongSoPAKNGiaiQuyetDuoc"].ToString() + "</a></td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuanX"], 0) + ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0)) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + (ConvertUtility.ToInt32(row["LuyKeKNTonDongDenDauTuanX"], 0) + (ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0) - ConvertUtility.ToInt32(row["TongSoPAKNGiaiQuyetDuoc"], 0))) + "</td>");
                    sb.Append("</tr>");

                    indexLinhVucCon++;

                    dtLinhVucCon.Rows.RemoveAt(i);
                    i--;
                }
            }

            return sb;
        }


        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 01/01/2014
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dtLinhVucChung"></param>
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private StringBuilder BaoCaoTongHopGQKNVNPTTT_DisplayLinhVucChung(ref int index, DataTable dtLinhVucChung, DataTable dtLinhVucCon, string parentId, string formatNumber)
        {
            StringBuilder sb = new StringBuilder();


            if (parentId.Length > 0)
            {
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    if (row["ParentId"].ToString() == parentId)
                    {
                        int luyKeKNDaGQDenDauTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuan"], 0);
                        int luyKeKNTonDongDauTuan = ConvertUtility.ToInt32(row["LuyKeKNTonDongDauTuan"], 0);
                        int soLuongTiepNhanTrongTuan = ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0);
                        int soLuongDaGQTrongTuan = ConvertUtility.ToInt32(row["SoLuongDaGiaiQuyetTrongTuan"], 0);
                        int soLuongTonDongTrongTuanDoQuaHan = ConvertUtility.ToInt32(row["SoLuongTonDongTrongTuanDoQuaHan"], 0);
                        int luyKeKNDaGQDenCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenCuoiTuan"], 0);
                        int luyKeTonDongDoQuaHanCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNTongDongDoQuaHan"], 0);

                        sb.Append("<tr>");
                        //sb.Append("<td class='borderThin' align='center' valign='top'>" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenDauTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeKNTonDongDauTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhanTrongTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongDaGQTrongTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongTonDongTrongTuanDoQuaHan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenCuoiTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeTonDongDoQuaHanCuoiTuan.ToString(formatNumber) + "</td>");
                        sb.Append("</tr>");

                        index++;

                        StringBuilder sbLinhVucCon = BaoCaoTongHopGQKNVNPTTT_DisplayLinhVucCon(ref index, dtLinhVucCon, row["LinhVucChungId"].ToString(), formatNumber);
                        if (sbLinhVucCon != null)
                        {
                            sb.Append(sbLinhVucCon.ToString());
                        }

                        dtLinhVucChung.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtLinhVucChung.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucChung.Rows[i];

                    int curIndex = index;

                    int luyKeKNDaGQDenDauTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuan"], 0);
                    int luyKeKNTonDongDauTuan = ConvertUtility.ToInt32(row["LuyKeKNTonDongDauTuan"], 0);
                    int soLuongTiepNhanTrongTuan = ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0);
                    int soLuongDaGQTrongTuan = ConvertUtility.ToInt32(row["SoLuongDaGiaiQuyetTrongTuan"], 0);
                    int soLuongTonDongTrongTuanDoQuaHan = ConvertUtility.ToInt32(row["SoLuongTonDongTrongTuanDoQuaHan"], 0);
                    int luyKeKNDaGQDenCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenCuoiTuan"], 0);
                    int luyKeTonDongDoQuaHanCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNTongDongDoQuaHan"], 0);

                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center' valign='top' rowspan='rowspanX'>" + index.ToString() + "</td>");
                    sb.Append("<td class='borderThin'>" + row["LinhVucChung"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenDauTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeKNTonDongDauTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhanTrongTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + soLuongDaGQTrongTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + soLuongTonDongTrongTuanDoQuaHan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenCuoiTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeTonDongDoQuaHanCuoiTuan.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");

                    //index++;

                    StringBuilder sbLinhVucCon = BaoCaoTongHopGQKNVNPTTT_DisplayLinhVucCon(ref index, dtLinhVucCon, row["LinhVucChungId"].ToString(), formatNumber);
                    if (sbLinhVucCon != null)
                    {
                        sb.Append(sbLinhVucCon.ToString());
                    }

                    dtLinhVucChung.Rows.RemoveAt(i);
                    i--;

                    int totalRow = index - curIndex + 1;
                    sb.Replace("rowspanX", totalRow.ToString());
                    index = curIndex + 1;
                }
            }

            return sb;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 01/01/2014
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dtLinhVucCon"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private StringBuilder BaoCaoTongHopGQKNVNPTTT_DisplayLinhVucCon(ref int index, DataTable dtLinhVucCon, string parentId, string formatNumber)
        {
            StringBuilder sb = new StringBuilder();

            if (parentId.Length > 0)
            {
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];
                    if (row["ParentId"].ToString() == parentId)
                    {
                        int luyKeKNDaGQDenDauTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuan"], 0);
                        int luyKeKNTonDongDauTuan = ConvertUtility.ToInt32(row["LuyKeKNTonDongDauTuan"], 0);
                        int soLuongTiepNhanTrongTuan = ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0);
                        int soLuongDaGQTrongTuan = ConvertUtility.ToInt32(row["SoLuongDaGiaiQuyetTrongTuan"], 0);
                        int soLuongTonDongTrongTuanDoQuaHan = ConvertUtility.ToInt32(row["SoLuongTonDongTrongTuanDoQuaHan"], 0);
                        int luyKeKNDaGQDenCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenCuoiTuan"], 0);
                        int luyKeTonDongDoQuaHanCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNTongDongDoQuaHan"], 0);

                        sb.Append("<tr>");
                        //sb.Append("<td class='borderThin' align='center' valign='top'>" + index.ToString() + "</td>");
                        sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenDauTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeKNTonDongDauTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhanTrongTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongDaGQTrongTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + soLuongTonDongTrongTuanDoQuaHan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenCuoiTuan.ToString(formatNumber) + "</td>");
                        sb.Append("<td align='center' class='borderThin'>" + luyKeTonDongDoQuaHanCuoiTuan.ToString(formatNumber) + "</td>");
                        sb.Append("</tr>");

                        index++;

                        dtLinhVucCon.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtLinhVucCon.Rows.Count; i++)
                {
                    DataRow row = dtLinhVucCon.Rows[i];

                    int luyKeKNDaGQDenDauTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenDauTuan"], 0);
                    int luyKeKNTonDongDauTuan = ConvertUtility.ToInt32(row["LuyKeKNTonDongDauTuan"], 0);
                    int soLuongTiepNhanTrongTuan = ConvertUtility.ToInt32(row["SoLuongTiepNhanTrongTuan"], 0);
                    int soLuongDaGQTrongTuan = ConvertUtility.ToInt32(row["SoLuongDaGiaiQuyetTrongTuan"], 0);
                    int soLuongTonDongTrongTuanDoQuaHan = ConvertUtility.ToInt32(row["SoLuongTonDongTrongTuanDoQuaHan"], 0);
                    int luyKeKNDaGQDenCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNDaGiaiQuyetDenCuoiTuan"], 0);
                    int luyKeTonDongDoQuaHanCuoiTuan = ConvertUtility.ToInt32(row["LuyKeKNTongDongDoQuaHan"], 0);

                    sb.Append("<tr>");
                    sb.Append("<td class='borderThin' align='center' valign='top' >" + index.ToString() + "</td>");
                    sb.Append("<td class='borderThin'>" + row["LinhVucCon"].ToString() + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenDauTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeKNTonDongDauTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + soLuongTiepNhanTrongTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + soLuongDaGQTrongTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + soLuongTonDongTrongTuanDoQuaHan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeKNDaGQDenCuoiTuan.ToString(formatNumber) + "</td>");
                    sb.Append("<td align='center' class='borderThin'>" + luyKeTonDongDoQuaHanCuoiTuan.ToString(formatNumber) + "</td>");
                    sb.Append("</tr>");

                    index++;

                    dtLinhVucCon.Rows.RemoveAt(i);
                    i--;
                }
            }

            return sb;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/09/2013
        /// Todo : Lấy tên đối tác hiển thị ra báo cáo
        /// </summary>
        /// <param name="listDoiTac"></param>
        /// <param name="doiTacId"></param>
        /// <returns></returns>
        private string GetTenDoiTac(List<DoiTacInfo> listDoiTac, int doiTacId)
        {
            if (listDoiTac == null || listDoiTac.Count == 0)
                return string.Empty;

            for (int i = 0; i < listDoiTac.Count; i++)
            {
                if (listDoiTac[i].Id == doiTacId)
                {
                    return listDoiTac[i].TenDoiTac;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/11/2013
        /// Todo : Xóa các row mà các cột (ngoại trừ cột exceptColumnName) có giá trị = 0 hoặc không có giá trị
        /// </summary>
        /// <param name="exceptColumnName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable RemoveRowHasNotValue(string exceptColumnName, DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];

                    bool isExists = false;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (!exceptColumnName.Contains(col.ColumnName) && ConvertUtility.ToInt32(row[col], -1) > 0)
                        {
                            isExists = true;
                            break;
                        }
                    }

                    if (!isExists)
                    {
                        dt.Rows.RemoveAt(i);
                        i--;
                    }
                }
            } // end if (dt != null && dt.Rows.Count > 0)

            return dt;
        }

        #endregion

        #region Export excel

        #endregion

        #region Code Không sửa dụng

        //public static string BuildDropdownListBaoCao()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var adminLogin = LoginAdmin.AdminLogin();
        //    if (adminLogin != null)
        //    {
        //        sb.Append("<option selected=\"selected\">--Chọn báo cáo--</option>");
        //        if (adminLogin.NhomNguoiDung == 1 || adminLogin.NhomNguoiDung == 0 || adminLogin.NhomNguoiDung == 2)
        //        {
        //            var lst = new MenuImpl().GetList();
        //            var lstBaoCao = lst.Where(t => t.ParentID == 4).OrderBy(t => t.STT);
        //            foreach (MenuInfo item in lstBaoCao)
        //            {
        //                if (item.Display.ToString().Equals("0"))
        //                {
        //                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Name3, item.Name);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            DataTable dt = new UserRightImpl().GetMenuByAdminIDAndParentID(adminLogin.ID, 4);
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                if (dr["Display"].ToString().Equals("0"))
        //                {
        //                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", dr["Name3"], dr["Name"]);
        //                }
        //            }
        //        }
        //    }
        //    return sb.ToString();
        //}

        //        public static string Baocaotonghop2(DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();
        //                var admin = LoginAdmin.AdminLogin();
        //                //Hien tai du lieu chua co nen chua query theo tham so tu ngay - den ngay
        //                int DoiTacId = admin.DoiTacId;
        //                if (admin.NhomNguoiDung == 0 || admin.NhomNguoiDung == 1 || admin.NhomNguoiDung == 2)
        //                    DoiTacId = 0;
        //                else if (admin.NhomNguoiDung == 3)
        //                    DoiTacId = admin.KhuVucId;
        //                else if (admin.NhomNguoiDung == 4)
        //                    DoiTacId = admin.DoiTacId;
        //                var list = new ReportImpl().BaoCaoTongHop2(DoiTacId, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    var lstKhuVuc = ServiceFactory.GetInstanceDoiTac().GetListDynamic("MaDoiTac,TenDoiTac,Id", "DonViTrucThuoc in (0,1)", "");
        //                    int KhuVucId = -1;
        //                    DoiTacId = -1;
        //                    foreach (var info in list)
        //                    {

        //                        sb.Append("<tr>");
        //                        if (info.KhuVucId != KhuVucId)
        //                        {
        //                            i = i + 1;
        //                            KhuVucId = info.KhuVucId;
        //                            var count = list.Where(t => t.KhuVucId == KhuVucId).Count();
        //                            var itemKhuVuc = lstKhuVuc.Where(t => t.Id == KhuVucId);
        //                            sb.Append("<td rowspan='" + count + "' class='borderThin' align='center'>" + i + "</td>");
        //                            if (itemKhuVuc.Count() > 0)
        //                                sb.Append("<td rowspan='" + count + "' class='borderThin' align='center'>" + itemKhuVuc.Single().MaDoiTac + "</td>");
        //                            else
        //                                sb.Append("<td rowspan='" + count + "' class='borderThin' align='center'>P.CSKH</td>");
        //                        }
        //                        if (info.DoiTacId != DoiTacId)
        //                        {
        //                            DoiTacId = info.DoiTacId;
        //                            var count = list.Where(t => t.DoiTacId == DoiTacId).Count();
        //                            string sDoiTac = info.TenDoiTac.Trim();
        //                            if (string.IsNullOrEmpty(sDoiTac))
        //                                sDoiTac = "Administrator";
        //                            sb.Append("<td align='center' rowspan='" + count + "' class='borderThin'>" + sDoiTac + "</td>");
        //                        }

        //                        string sMaDV = info.MaDichVu.Trim();
        //                        if (string.IsNullOrEmpty(sMaDV))
        //                            sMaDV = "Không chọn";
        //                        sb.Append("<td align='center' class='borderThin'>" + sMaDV + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.SoLuong + "</td>");
        //                        sb.Append("</tr>");
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='5'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='5'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoDonVi(int DonViId, int KetQua, int DoHaiLong, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoDonVi(DonViId, KetQua, DoHaiLong, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    if (DonViId != 0)
        //                    {
        //                        foreach (var info in list)
        //                        {
        //                            i++;
        //                            /*<tr>
        //                                <th>
        //                                    STT
        //                                </th>
        //                                <th>
        //                                    User
        //                                </th>
        //                                <th>
        //                                    Số thuê bao
        //                                </th>
        //                                <th>
        //                                    Ngày thực hiện
        //                                </th>
        //                                <th>
        //                                    Dịch vụ
        //                                </th>
        //                                <th>
        //                                    Nội dung tư vấn
        //                                </th>
        //                                <th>
        //                                    Kết quả
        //                                </th>
        //                                <th>
        //                                    Độ hài lòng
        //                                </th>
        //                            </tr>
        //                             * */
        //                            sb.Append("<tr>");
        //                            sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDVTuVan) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.CacDichVuDaBan) + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace('_', ' ') + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace('_', ' ') + "</td>");
        //                            sb.Append("</tr>");
        //                            //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var lstGroup = list.GroupBy(t => new { t.DoiTacId });
        //                        var lstDoiTac = ServiceFactory.GetInstanceDoiTac().GetList();
        //                        foreach (var infoGr in lstGroup)
        //                        {
        //                            var infoDoiTac = lstDoiTac.Where(t => t.Id == infoGr.Key.DoiTacId);
        //                            if (infoDoiTac != null && infoDoiTac.Count() > 0)
        //                            {
        //                                sb.Append("<tr>");
        //                                sb.Append("<td colspan='9' style='padding-left:5px;font-weight: bold; height:30px; border:1pt solid #000;border-top-width: 0px;'>" + lstDoiTac.Where(t => t.Id == infoGr.Key.DoiTacId).Single().TenDoiTac + "</td>");
        //                                sb.Append("</tr>");
        //                            }
        //                            else
        //                            {
        //                                sb.Append("<tr>");
        //                                sb.Append("<td colspan='9' style='padding-left:5px;font-weight: bold; height:30px; border:1pt solid #000;border-top-width: 0px;'>Administrator</td>");
        //                                sb.Append("</tr>");
        //                            }
        //                            var listDVByGroup = list.Where(t => t.DoiTacId == infoGr.Key.DoiTacId);
        //                            i = 0;
        //                            foreach (var info in listDVByGroup)
        //                            {
        //                                i++;
        //                                /*<tr>
        //                                    <th>
        //                                        STT
        //                                    </th>
        //                                    <th>
        //                                        User
        //                                    </th>
        //                                    <th>
        //                                        Số thuê bao
        //                                    </th>
        //                                    <th>
        //                                        Ngày thực hiện
        //                                    </th>
        //                                    <th>
        //                                        Dịch vụ
        //                                    </th>
        //                                    <th>
        //                                        Nội dung tư vấn
        //                                    </th>
        //                                    <th>
        //                                        Kết quả
        //                                    </th>
        //                                    <th>
        //                                        Độ hài lòng
        //                                    </th>
        //                                </tr>
        //                                 * */
        //                                sb.Append("<tr>");
        //                                sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                                sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                                sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDVTuVan) + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.CacDichVuDaBan) + "</td>");
        //                                sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace('_', ' ') + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace('_', ' ') + "</td>");
        //                                sb.Append("</tr>");
        //                                //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='9'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='9'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoDonVi_Page(int DonViId, int KetQua, int DoHaiLong, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, out string sPage)
        //        {
        //            sPage = string.Empty;
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoDonVi(DonViId, KetQua, DoHaiLong, fromDate, toDate);


        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    int totalRecord = list.Count;
        //                    if (DonViId != 0)
        //                    {
        //                        int start = (pageIndex - 1) * pageSize;
        //                        int end = pageIndex * pageSize;
        //                        if (end > list.Count)
        //                            end = list.Count;
        //                        for (int iii = start; iii<end ; iii++)
        //                        {
        //                            var info = list[iii];
        //                            sb.Append("<tr>");
        //                            sb.Append("<td class='borderThin' align='center'>" + (iii+1) + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDVTuVan) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.CacDichVuDaBan) + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace('_', ' ') + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace('_', ' ') + "</td>");
        //                            sb.Append("</tr>");
        //                        }
        //                        StringBuilder sbPage = new StringBuilder();
        //                        sbPage.Append("<div style='padding-top:10px; padding-bottom:30px;'>");
        //                        sbPage.Append(HtmlUtility.BuildPagerNormal(totalRecord, pageSize, pageIndex, "&don_vi=" + DonViId + "&KetQua=" + KetQua + "&DoHaiLong=" + DoHaiLong + "&tu_ngay=" + fromDate.ToString("dd/MM/yyyy") + "&den_ngay=" + toDate.ToString("dd/MM/yyyy"), "", "active", 5));
        //                        sbPage.Append("</div>");

        //                        sPage = sbPage.ToString();
        //                    }
        //                    else
        //                    {
        //                        var lstGroup = list.GroupBy(t => new { t.DoiTacId });
        //                        var lstDoiTac = ServiceFactory.GetInstanceDoiTac().GetList();
        //                        foreach (var infoGr in lstGroup)
        //                        {
        //                            var infoDoiTac = lstDoiTac.Where(t => t.Id == infoGr.Key.DoiTacId);
        //                            if (infoDoiTac != null && infoDoiTac.Count() > 0)
        //                            {
        //                                sb.Append("<tr>");
        //                                sb.Append("<td colspan='9' style='padding-left:5px;font-weight: bold; height:30px; border:1pt solid #000;border-top-width: 0px;'>" + lstDoiTac.Where(t => t.Id == infoGr.Key.DoiTacId).Single().TenDoiTac + "</td>");
        //                                sb.Append("</tr>");
        //                            }
        //                            else
        //                            {
        //                                sb.Append("<tr>");
        //                                sb.Append("<td colspan='9' style='padding-left:5px;font-weight: bold; height:30px; border:1pt solid #000;border-top-width: 0px;'>Administrator</td>");
        //                                sb.Append("</tr>");
        //                            }
        //                            var listDVByGroup = list.Where(t => t.DoiTacId == infoGr.Key.DoiTacId);
        //                            i = 0;
        //                            foreach (var info in listDVByGroup)
        //                            {
        //                                i++;                                
        //                                sb.Append("<tr>");
        //                                sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                                sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                                sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDVTuVan) + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.CacDichVuDaBan) + "</td>");
        //                                sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace('_', ' ') + "</td>");
        //                                sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace('_', ' ') + "</td>");
        //                                sb.Append("</tr>");
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='9'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='9'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoDichVu(int DonViId, int MaDichVu, int ketqua, int dohailong, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoDichVu(DonViId, MaDichVu, ketqua, dohailong, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    if (MaDichVu != 0)
        //                    {
        //                        foreach (var info in list)
        //                        {
        //                            i++;
        //                            /*<tr>
        //                            <th>
        //                                STT
        //                            </th>
        //                            <th>
        //                                Tên đơn vị
        //                            </th>
        //                            <th>
        //                                User name
        //                            </th>
        //                            <th>
        //                                Số thuê bao
        //                            </th>
        //                            <th>
        //                                Ngày thực hiện
        //                            </th>
        //                            <th>
        //                                Nội dung tư vấn
        //                            </th>
        //                            <th>
        //                                Kết quả
        //                            </th>
        //                            <th>
        //                                Độ hài lòng
        //                            </th>
        //                        </tr>
        //                             * */
        //                            sb.Append("<tr>");
        //                            sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.TenDoiTac) + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace('_', ' ') + "</td>");
        //                            sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace('_', ' ') + "</td>");
        //                            sb.Append("</tr>");
        //                            //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var lstGroup = list.GroupBy(t => new { t.DichVuId });
        //                        var lstDichVu = ServiceFactory.GetInstanceDichVuVNP().GetList();

        //                        foreach (var infoGr in lstGroup)
        //                        {
        //                            try
        //                            {
        //                                var infoDichVu = lstDichVu.Where(t => t.Id == infoGr.Key.DichVuId);

        //                                if (infoDichVu != null && infoDichVu.Count() > 0)
        //                                {
        //                                    sb.Append("<tr>");
        //                                    sb.Append("<td colspan='8' style='padding-left:5px;font-weight: bold; height:30px; border:1pt solid #000;border-top-width: 0px;'>" + infoDichVu.Single().MaDichVu + "</td>");
        //                                    sb.Append("</tr>");
        //                                }
        //                                else
        //                                {
        //                                    sb.Append("<tr>");
        //                                    sb.Append("<td colspan='8' style='padding-left:5px;font-weight: bold; height:30px; border:1pt solid #000;border-top-width: 0px;'>Administrator</td>");
        //                                    sb.Append("</tr>");
        //                                }
        //                                var listDVByGroup = list.Where(t => t.DichVuId == infoGr.Key.DichVuId);
        //                                i = 0;
        //                                foreach (var info in listDVByGroup)
        //                                {
        //                                    i++;
        //                                    /*<tr>
        //                                    <th>
        //                                        STT
        //                                    </th>
        //                                    <th>
        //                                        Tên đơn vị
        //                                    </th>
        //                                    <th>
        //                                        User name
        //                                    </th>
        //                                    <th>
        //                                        Số thuê bao
        //                                    </th>
        //                                    <th>
        //                                        Ngày thực hiện
        //                                    </th>
        //                                    <th>
        //                                        Nội dung tư vấn
        //                                    </th>
        //                                    <th>
        //                                        Kết quả
        //                                    </th>
        //                                    <th>
        //                                        Độ hài lòng
        //                                    </th>
        //                                </tr>
        //                                     * */
        //                                    sb.Append("<tr>");
        //                                    sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                                    sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.TenDoiTac) + "</td>");
        //                                    sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                                    sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                                    sb.Append("<td align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                                    sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                                    sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace('_', ' ') + "</td>");
        //                                    sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace('_', ' ') + "</td>");
        //                                    sb.Append("</tr>");
        //                                    //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            { }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='8'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='8'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoGoiKiemOB(int DonViId, int MaDichVu, int ketqua, int dohailong, int tyle, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoGoiKiem(DonViId, MaDichVu, ketqua, dohailong, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    var TongSoThueBaoTyLe = list.Count * tyle / 100;
        //                    List<int> lstTemp = new List<int>();
        //                    Random rd = new Random();
        //                    bool flag = true;
        //                    for (int iii = 0; iii < TongSoThueBaoTyLe; iii++)
        //                    {
        //                        flag = true;
        //                        int curr = 0;
        //                        while (flag)
        //                        {
        //                            curr = rd.Next(0, TongSoThueBaoTyLe);
        //                            if (!lstTemp.Contains(curr))
        //                            {
        //                                lstTemp.Add(curr);
        //                                flag = false;
        //                            }
        //                        }
        //                        var info = list[curr];
        //                        i++;
        //                        sb.Append("<tr>");
        //                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                        sb.Append("<td class='borderThin' align='center'>" + info.SoThueBao + "</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("<td class='borderThin' align='center'>&nbsp;</td>");
        //                        sb.Append("</tr>");
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='14'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='14'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoNguoiDung(string username, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoNguoiDung(username, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    foreach (var info in list)
        //                    {
        //                        i++;
        //                        /*<tr>
        //                            <th>
        //                                STT
        //                            </th>
        //                            <th>
        //                                Số thuê bao
        //                            </th>
        //                            <th>
        //                                Ngày thực hiện
        //                            </th>
        //                            <th>
        //                                Nội dung tư vấn
        //                            </th>
        //                            <th>
        //                                Kết quả
        //                            </th>
        //                            <th>
        //                                Độ hài lòng
        //                            </th>
        //                        </tr>
        //                         * */
        //                        sb.Append("<tr>");
        //                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                        sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                        sb.Append("<td  align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDVTuVan) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.CacDichVuDaBan) + "</td>");
        //                        sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace('_', ' ') + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace('_', ' ') + "</td>");
        //                        sb.Append("</tr>");
        //                        //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='6'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='6'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoHenGoiLai(int DonViId, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoHenGoiLai(DonViId, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    foreach (var info in list)
        //                    {
        //                        i++;
        //                        /*<tr>
        //                        <th>
        //                            STT
        //                        </th>
        //                        <th>
        //                            Số thuê bao
        //                        </th>
        //                        <th>
        //                            Người thực hiện
        //                        </th>
        //                        <th>
        //                            Ngày thực hiện
        //                        </th>
        //                        <th>
        //                            Ngày hẹn gọi lại
        //                        </th>
        //                        <th>
        //                            Dịch vụ tư vấn
        //                        </th>
        //                        <th>
        //                            Nội dung tư vấn
        //                        </th>
        //                    </tr>
        //                         * */
        //                        sb.Append("<tr>");
        //                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                        sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.SoThueBao) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.NgayThucHien.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.KHHenGoiLai.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDVTuVan) + "</td>");
        //                        sb.Append("<td class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                        sb.Append("</tr>");
        //                        //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='7'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='7'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoDoanhThu(bool excel, int DonViId, int DichVuId, string username, string stb, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoDoanhThu(DonViId, DichVuId, username, stb, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    foreach (var info in list)
        //                    {
        //                        i++;
        //                        /*<th>
        //                            STT
        //                        </th>
        //                        <th>
        //                            Dịch vụ
        //                        </th>
        //                        <th>
        //                            Số tư vấn
        //                        </th>
        //                        <th>
        //                            Số lần mở
        //                        </th>
        //                        <th>
        //                            Tỷ lệ tư vấn/Mở dịch vụ
        //                        </th>
        //                        <th>
        //                            Doanh thu
        //                        </th>
        //                         * */
        //                        sb.Append("<tr>");
        //                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDichVu) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.SoLanTuVan + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.SoLanMo + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.TyLe + "</td>");
        //                        if (excel)
        //                            sb.Append("<td align='right' style='padding-right:2px;border:1pt solid #000; border-top-width: 0px;'>" + info.DoanhThu.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                        else
        //                            sb.Append("<td align='right' style='padding-right:2px;border:1pt solid #000; border-top-width: 0px;'>" + info.DoanhThu.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN")) + "</td>");
        //                        sb.Append("</tr>");
        //                        //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                    }

        //                    sb.Append("<tr style='line-height:28px;'>");
        //                    sb.Append("<td colspan='2' style='font-weight: bold; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;' align='center'>Tổng cộng</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + ReplaceEmptyOrNull(list.Sum(t => t.SoLanTuVan)) + "</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + ReplaceEmptyOrNull(list.Sum(t => t.SoLanMo)) + "</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold;padding-right:5px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + ReplaceEmptyOrNull(list.Sum(t => t.TyLe)) + "</td>");
        //                    if (excel)
        //                        sb.Append("<td align='right' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; '>" + ReplaceEmptyOrNull(list.Sum(t => t.DoanhThu).ToString("0,0", CultureInfo.CreateSpecificCulture("en-US"))) + "</td>");
        //                    else
        //                        sb.Append("<td align='right' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; '>" + ReplaceEmptyOrNull(list.Sum(t => t.DoanhThu).ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"))) + "</td>");
        //                    sb.Append("</tr>");
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='6'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='6'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoDoanhThu2012(bool excel, int DonViId, int DichVuId, string username, string stb, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoDoanhThu2012(DonViId, DichVuId, username, stb, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    decimal TongSoTuVan = list.Sum(t => t.SoLanTuVan);
        //                    decimal TotalTyLeMo = 0;
        //                    decimal TotalTongTyLeMo = 0;
        //                    int TotalSoLanMo = 0;
        //                    decimal TotalDoanhThuMoQuaCS = 0;
        //                    decimal TotalDoanhThuMoQuaHTKhac = 0;
        //                    decimal ToTalTongDoanhThu = 0;

        //                    foreach (var info in list)
        //                    {
        //                        i++;
        //                        decimal SLTV = info.SoLanTuVan;
        //                        if (SLTV == 0)
        //                            SLTV = info.SoLanMoQuaCS + info.SoLanMoQuaHTKhac;
        //                        else
        //                            SLTV = info.SoLanTuVan;
        //                        decimal TyLeMo = Math.Round((((decimal)(info.SoLanMoQuaCS + info.SoLanMoQuaHTKhac)) / SLTV) * 100, 2);
        //                        decimal TongTyLeMo = Math.Round((((decimal)(info.SoLanMoQuaCS + info.SoLanMoQuaHTKhac)) / TongSoTuVan) * 100, 2);
        //                        int SoLanMo = info.SoLanMoQuaHTKhac + info.SoLanMoQuaCS;
        //                        decimal DoanhThuMoQuaCS = info.TongTienMoQuaCS;
        //                        decimal DoanhThuMoQuaHTKhac = info.TongTienMoQuaHTK;
        //                        decimal TongDoanhThu = info.TongTienMoQuaCS + info.TongTienMoQuaHTK;

        //                        TotalTyLeMo += TyLeMo;
        //                        TotalTongTyLeMo += TongTyLeMo;
        //                        TotalSoLanMo += SoLanMo;
        //                        TotalDoanhThuMoQuaCS += DoanhThuMoQuaCS;
        //                        TotalDoanhThuMoQuaHTKhac += DoanhThuMoQuaHTKhac;
        //                        ToTalTongDoanhThu += TongDoanhThu;

        //                        sb.Append("<tr>");
        //                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDichVu) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.SoLanTuVan + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.SoLanMoQuaCS + "</td>");
        //                        sb.Append("<td align='right' class='borderThin'>" + DoanhThuMoQuaCS.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.SoLanMoQuaHTKhac + "</td>");
        //                        sb.Append("<td align='right' class='borderThin'>" + DoanhThuMoQuaHTKhac.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + SoLanMo + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + TyLeMo + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + TongTyLeMo + "</td>");
        //                        sb.Append("<td align='right' style='padding-right:2px;border:1pt solid #000; border-top-width: 0px;'>" + TongDoanhThu.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                        sb.Append("</tr>");
        //                    }

        //                    sb.Append("<tr style='line-height:28px;'>");
        //                    sb.Append("<td colspan='2' style='font-weight: bold; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;' align='center'>Tổng cộng</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + TongSoTuVan + "</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + list.Sum(t => t.SoLanMoQuaCS) + "</td>");
        //                    sb.Append("<td align='right' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + TotalDoanhThuMoQuaCS.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + list.Sum(t => t.SoLanMoQuaHTKhac) + "</td>");
        //                    sb.Append("<td align='right' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + TotalDoanhThuMoQuaHTKhac.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + TotalSoLanMo + "</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>&nbsp;</td>");
        //                    sb.Append("<td align='center' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>&nbsp;</td>");

        //                    sb.Append("<td align='right' style='font-weight:bold;border:1pt solid #000; border-top-width: 0px; '>" + ToTalTongDoanhThu.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                    sb.Append("</tr>");
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='12'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='12'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }


        //        public static string BaoCaoSoThueBao(int DonViId, string username, string stb, DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                var list = new ReportImpl().BaoCaoSoThueBao(DonViId, username, stb, fromDate, toDate);
        //                int i = 0;
        //                if (list != null && list.Count > 0)
        //                {
        //                    foreach (var info in list)
        //                    {
        //                        i++;
        //                        sb.Append("<tr>");
        //                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.MaDVTuVan) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.NVTuVan) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + info.CDate.ToString("dd/MM/yyyy HH:mm") + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + ReplaceEmptyOrNull(info.NoiDung) + "</td>");
        //                        sb.Append("<td align='center' class='borderThin'>" + Enum.GetName(typeof(CrossSellHistory_KetQua), info.KetQua).Replace("_", " ") + "</td>");
        //                        sb.Append("<td align='right' style='padding-right:2px;border:1pt solid #000; border-top-width: 0px;'>" + Enum.GetName(typeof(CrossSellHistory_DoHaiLong), info.DoHaiLong).Replace("_", " ") + "</td>");
        //                        sb.Append("</tr>");
        //                        //sb.Append("<tr><td align='center'>" + i + "</td><td>" + info.NVTuVan + "</td><td>acx</td><td align='right'>1</td></tr>");
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='6'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='6'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string TraCuuNhomTiepThi_BaoCao(int DoiTacId, int DichVuId, string SoThueBao, string NVTuVan, string TuNgay, string DenNgay, int Card, int Post)
        //        {
        //            try
        //            {
        //                StringBuilder where = new StringBuilder("1=1 ");
        //                if (DoiTacId > 0)
        //                {
        //                    where.AppendFormat(" AND DoiTacID={0}", DoiTacId);
        //                }
        //                if (DichVuId > 0)
        //                {
        //                    where.AppendFormat(" AND DichVuId={0}", DichVuId);
        //                }
        //                if (SoThueBao.Length > 0)
        //                {
        //                    where.AppendFormat(" AND SoThueBao LIKE '{0}%'", SoThueBao);
        //                }
        //                if (NVTuVan.Length > 0)
        //                {
        //                    where.AppendFormat(" AND NVTuVan LIKE '{0}%'", NVTuVan);
        //                }
        //                if (TuNgay.Length > 0)
        //                {
        //                    where.AppendFormat(" AND CDate >= CONVERT(smalldatetime,'{0}',103)", TuNgay);
        //                }
        //                if (DenNgay.Length > 0)
        //                {
        //                    where.AppendFormat(" AND CDate <= CONVERT(smalldatetime,'{0}',103)", DenNgay);
        //                }
        //                if (Card == 1 && Post == 1)
        //                {
        //                    where.AppendFormat(" AND (LoaiThueBao=1 OR LoaiThueBao=2)");
        //                }
        //                else if (Card == 1)
        //                {
        //                    where.AppendFormat(" AND LoaiThueBao=1 ");
        //                }
        //                else if (Post == 1)
        //                {
        //                    where.AppendFormat(" AND LoaiThueBao=2 ");
        //                }

        //                StringBuilder sb = new StringBuilder();
        //                var lst = ServiceFactory.GetInstanceThongTinTiepThiTraCuu().GetListDynamic("", where.ToString(), "");
        //                int i = 1;
        //                foreach (var item in lst)
        //                {
        //                    sb.Append("<tr>");
        //                    sb.AppendFormat("<td class='borderThin' align=\"center\">{0}</td>", i);
        //                    sb.AppendFormat("<td class='borderThin' align=\"center\">{0}</td>", ReplaceEmptyOrNull(item.NVTuVan));
        //                    sb.AppendFormat("<td class='borderThin'>{0}</td>", ReplaceEmptyOrNull(item.SoThueBao));
        //                    sb.AppendFormat("<td class='borderThin'>{0}</td>", ReplaceEmptyOrNull(item.TenKhachHang));
        //                    sb.AppendFormat("<td align='center' class='borderThin'>{0}</td>", ReplaceEmptyOrNull(item.MaDichVu));
        //                    sb.AppendFormat("<td align='center' class='borderThin'>{0}</td>", item.CDate.ToString("dd/MM/yyyy HH:mm"));
        //                    sb.AppendFormat("<td class='borderThin'>{0}</td>", ReplaceEmptyOrNull(item.NoiDung));
        //                    sb.Append("</tr>");
        //                    i++;
        //                }
        //                sb.AppendFormat("<tr class=\"bottom\"><td colspan=\"7\">Tổng số <strong>{0}</strong> tư vấn", lst.Count);

        //                sb.Append("</td></tr>");
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='6'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }

        //        public static string BaoCaoTongHopDoanhThu(DateTime fromDate, DateTime toDate)
        //        {
        //            try
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                DataSet list = new ReportImpl().BaoCaoTongHopDoanhThu(fromDate, toDate);
        //                int i = 0;
        //                decimal tong_tu_van = 0;
        //                decimal tong_dv_mo_qua_HT_khac = 0;
        //                decimal tong_dv_mo_qua_HT = 0;
        //                decimal tong_doanh_thu = 0;

        //                if (list != null)
        //                {
        //                    if(list.Tables[0].Rows.Count > 0)
        //                    {
        //                        foreach (DataRow info in list.Tables[0].Rows)
        //                        {
        //                            i++;
        //                            tong_tu_van += ConvertUtility.ToDecimal(info[2]);
        //                            tong_dv_mo_qua_HT += ConvertUtility.ToDecimal(info[3]);
        //                            tong_dv_mo_qua_HT_khac += ConvertUtility.ToDecimal(info[4]);
        //                            tong_doanh_thu += ConvertUtility.ToDecimal(info[5]);

        //                            sb.Append("<tr>");
        //                            sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                            sb.Append("<td class='borderThin'>" + ConvertUtility.ToString(info[1]) + "</td>");
        //                            sb.Append("<td align='center' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + ConvertUtility.ToDecimal(info[2]).ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                            sb.Append("<td align='center' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + ConvertUtility.ToDecimal(info[3]).ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                            sb.Append("<td align='center' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'>" + ConvertUtility.ToDecimal(info[4]).ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                            sb.Append("<td align='right' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 1pt solid #000;'>" + ConvertUtility.ToDecimal(info[5]).ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</td>");
        //                            sb.Append("</tr>");

        //                        }
        //                        i++;
        //                        sb.Append("<tr>");
        //                        sb.Append("<td class='borderThin' align='center'>" + i + "</td>");
        //                        sb.Append("<td style='padding-left:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px'><b>Tổng số</b></td>");
        //                        sb.Append("<td align='center' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px'><b>" + tong_tu_van.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</b></td>");
        //                        sb.Append("<td align='center' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'><b>" + tong_dv_mo_qua_HT.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</b></td>");
        //                        sb.Append("<td align='center' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 0px;'><b>" + tong_dv_mo_qua_HT_khac.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</b></td>");
        //                        sb.Append("<td align='right' style='padding-right:2px; border:1pt solid #000; border-top-width: 0px; border-right-width: 1pt solid #000;'><b>" + tong_doanh_thu.ToString("0,0", CultureInfo.CreateSpecificCulture("en-US")) + "</b></td>");
        //                        sb.Append("</tr>");
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append(@"<tr>
        //                    <td colspan='9'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>");
        //                }
        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.LogEvent(ex);
        //                return @"<tr>
        //                    <td colspan='9'>
        //                        Chưa có dữ liệu báo cáo
        //                    </td>
        //                </tr>";
        //            }
        //        }




        //        public static string slDonvi()
        //        {
        //            StringBuilder sbdv = new StringBuilder();
        //            var lst = ServiceFactory.GetInstanceDoiTac().GetList();
        //            sbdv.Append("<option value='' selected='selected'>-Chọn-</option>");
        //            foreach (var infoDT in lst)
        //            {
        //                sbdv.Append("<option value='" + infoDT.Id + "'>" + infoDT.TenDoiTac + "</option>");
        //            }
        //            return sbdv.ToString();
        //        }
        //        public static string slDichvu()
        //        {
        //            StringBuilder sbdichvu = new StringBuilder();
        //            var lst = ServiceFactory.GetInstanceDichVuVNP().GetList();
        //            sbdichvu.Append("<option value='' selected='selected'>-Chọn-</option>");
        //            foreach (var infoDT in lst)
        //            {
        //                sbdichvu.Append("<option value='" + infoDT.MaDichVu + "'>" + ReplaceEmptyOrNull(infoDT.TenDichVu) + "</option>");
        //            }
        //            return sbdichvu.ToString();
        //        }

        //        public static string BindSelectKetQua()
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            sb.Append("<option value=\"-1\" selected='selected'>Tất cả</option>");
        //            foreach (int i in Enum.GetValues(typeof(CrossSellHistory_KetQua)))
        //            {
        //                if (i == (int)CrossSellHistory_KetQua.Thêm_mới)
        //                    continue;
        //                sb.AppendFormat("<option value=\"{0}\">{1}</option>", i, Enum.GetName(typeof(CrossSellHistory_KetQua), i).Replace("_", " "));
        //            }
        //            return sb.ToString();
        //        }

        //        public static string BindSelectHaiLong()
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            sb.Append("<option value=\"-1\" selected='selected'>Tất cả</option>");
        //            foreach (int i in Enum.GetValues(typeof(CrossSellHistory_DoHaiLong)))
        //            {
        //                sb.AppendFormat("<option value=\"{0}\">{1}</option>", i, Enum.GetName(typeof(CrossSellHistory_DoHaiLong), i).Replace("_", " "));
        //            }
        //            return sb.ToString();
        //        }

        //        public static string BindSelectDoiTac()
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            var lst = ServiceFactory.GetInstanceDoiTac().GetList();
        //            var adminInfo = LoginAdmin.AdminLogin();
        //            List<DoiTacInfo> lstPhanQuyen = null;
        //            int DonViTrucThuoc = 0;
        //            if (adminInfo.NhomNguoiDung == 5 || adminInfo.NhomNguoiDung == 4)
        //            {
        //                DonViTrucThuoc = adminInfo.KhuVucId;
        //                lstPhanQuyen = lst.Where(t => t.Id == adminInfo.DoiTacId).ToList();
        //            }
        //            else if (adminInfo.NhomNguoiDung == 3)
        //            {
        //                DonViTrucThuoc = 1;
        //                lstPhanQuyen = lst.Where(t => t.DonViTrucThuoc == adminInfo.KhuVucId || t.Id == adminInfo.KhuVucId).ToList();
        //            }
        //            else
        //            {
        //                lstPhanQuyen = lst;
        //                sb.Append("<option value=\"0\" selected='selected'>Không chọn</option>");
        //            }
        //            if (lst.Count > 0)
        //            {
        //                sb.Append(BindSelectDoiTac_recur(lstPhanQuyen, DonViTrucThuoc, ""));
        //            }
        //            return sb.ToString();
        //        }

        //        private static string BindSelectDoiTac_recur(List<DoiTacInfo> lst, int parent, string space)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            List<DoiTacInfo> findList = lst.FindAll(delegate(DoiTacInfo p) { return p.DonViTrucThuoc == parent; });
        //            foreach (var item in findList)
        //            {
        //                {
        //                    sb.AppendFormat("<option value=\"{0}\">{1}{2}</option>", item.Id, space, item.MaDoiTac);
        //                    sb.Append(BindSelectDoiTac_recur(lst, item.Id, space + "&nbsp;&nbsp;&nbsp;"));
        //                }
        //            }
        //            return sb.ToString();
        //        }

        //        public static string BindSelectDichVuBaoCao()
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            var lst = ServiceFactory.GetInstanceDichVuVNP().GetList();
        //            sb.Append("<option value=\"0\" selected='selected'>Không chọn</option>");
        //            if (lst.Count > 0)
        //            {
        //                sb.Append(BindSelectDichVuBC_recur(lst, 0, ""));
        //            }
        //            return sb.ToString();
        //        }

        //        private static string BindSelectDichVuBC_recur(List<DichVuVNPInfo> lst, int parent, string space)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            List<DichVuVNPInfo> findList = lst.FindAll(delegate(DichVuVNPInfo p) { return p.ParentId == parent; });
        //            foreach (var item in findList)
        //            {
        //                {
        //                    sb.AppendFormat("<option value=\"{0}\">{1}{2}</option>", item.Id, space, item.MaDichVu);
        //                    sb.Append(BindSelectDichVuBC_recur(lst, item.Id, space + "&nbsp;&nbsp;&nbsp;"));
        //                }
        //            }
        //            return sb.ToString();
        //        }

        //        public static string ReplaceEmptyOrNull(string obj)
        //        {
        //            if (obj == null || obj.ToString().Equals(""))
        //                return "&nbsp;";
        //            return obj.ToString();
        //        }

        //        public static string ReplaceEmptyOrNull(int obj)
        //        {
        //            if (obj == null)
        //                return "&nbsp;";
        //            return obj.ToString();
        //        }

        //        public static string ReplaceEmptyOrNull(decimal obj)
        //        {
        //            if (obj == null)
        //                return "&nbsp;";
        //            return obj.ToString();
        //        }

        //        public static string ReplaceEmptyOrNull(Int64 obj)
        //        {
        //            if (obj == null)
        //                return "&nbsp;";
        //            return obj.ToString();
        //        }

        #endregion

        public string ProcessNumber(object valueNumber, string strReplace)
        {
            if (valueNumber == null)
                return strReplace;
            if (valueNumber.ToString() == string.Empty)
                return strReplace;
            double val = Convert.ToDouble(valueNumber);
            if (val == 0)
                return strReplace;
            return string.Format("<span class=\"number\">{0}</span>", valueNumber);
        }
    }
}