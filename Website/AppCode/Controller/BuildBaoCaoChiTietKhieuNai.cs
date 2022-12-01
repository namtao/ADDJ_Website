using ADDJ.Core;
using ADDJ.Entity;
using ADDJ.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Website.AppCode.Controller
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 13/08/2014
    /// </summary>
    public class BuildBaoCaoChiTietKhieuNai
    {
        #region VNP

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 01/12/2014
        /// Todo : Danh sách khiếu nại của VNP
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="fromdate"></param>
        /// <param name="toDate"></param>
        /// <param name="LoaiKNId"></param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public string ListDanhsachLoaiKhieuNaiVNP(int khuVucId, DateTime fromdate, DateTime toDate, int LoaiKNId, int linhVucChungId, int nguonKhieuNai, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> lstKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListLoaiKhieuNaiVNP_Solr(khuVucId, fromdate, toDate, LoaiKNId, linhVucChungId, nguonKhieuNai, reportType);
            if (lstKhieuNaiInfo != null)
            {
                //string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", lstKhieuNaiInfo.Count);
                sb.Append("</tr>");
                switch (reportType)
                {
                    case 1://Số lượng tiếp nhận
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");
                        for (int i = 0; i < lstKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", (i + 1));
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;
                    case 2: //số lượng đã đóng trong kỳ
                    case 3: //Số lượng đã đóng trong khoảng thời gian báo cáo (bao gồm tồn kỳ trước)
                    case 4://Số lượng khiếu nại quá hạn toàn trình trong khoảng thời gian báo cáo
                    case 5://Số lượng khiếu nại quá hạn toàn trình trong khoảng thời gian báo cáo(bao gồm tồn kỳ trước)
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ngày đóng KN</th>");
                        sb.Append("</tr>");
                        for (int i = 0; i < lstKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='text-align:center'>{0}</td>", (i + 1));
                            sb.AppendFormat("<td style='text-align:center'>{0}</td>", lstKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td style='text-align:center'>{0}</td>", lstKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td style='text-align:center'>{0}</td>", lstKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td style='text-align:center'>{0}</td>", lstKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/01/2015
        /// Todo : Lấy danh sách khiếu nại toàn mạng theo tuần
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     = 1 : Số lượng KN tiếp nhận trong tuần
        ///     = 2 : Số lượng KN đã giải quyết trong tuần
        ///     = 3 : Số lượng KN quá hạn (chưa đóng) trong tuần
        ///     = 4 : Số lượng KN đã giải quyết từ đầu tháng đến hết ngày toDate
        ///     = 5 : Số lượng KN đã giải quyết từ đầu năm đến hết ngày toDate
        /// </param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiToanMangTheoTuan(DateTime fromDate, DateTime toDate, int nguonKhieuNai, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> lstKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiToanMangTheoTuan(fromDate, toDate, nguonKhieuNai, reportType);
            if (lstKhieuNaiInfo != null)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='4'>Số lượng bản ghi : {0}</td>", lstKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1: // SL tiếp nhận trong tuần
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("</tr>");
                        for (int i = 0; i < lstKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", (i + 1));
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 2: // SL đã giải quyết trong tuần
                    case 4: // SL Giải quyết từ đầu tháng đến toDate
                    case 5: // SL đã giải quyết từ đầu năm đến toDate
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày đóng KN</th>");
                        sb.Append("</tr>");
                        for (int i = 0; i < lstKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", (i + 1));
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 3: // SL quá hạn trong tuần
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày quá hạn</th>");
                        sb.Append("</tr>");
                        for (int i = 0; i < lstKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", (i + 1));
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    default:

                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/01/2015
        /// Todo : Lấy danh sách khiếu nại toàn mạng theo tháng
        /// </summary>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     = 1 : SL đã giải quyết tháng hiện tại
        ///     = 2 : SL đã giải quyết tháng trước
        ///     = 3 : SL đã giải quyết từ đầu năm đến cuối tháng toDate
        /// </param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiToanMangTheoThang(DateTime toDate, int nguonKhieuNai, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> lstKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiToanMangTheoThang(toDate, nguonKhieuNai, reportType);
            if (lstKhieuNaiInfo != null)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='4'>Số lượng bản ghi : {0}</td>", lstKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1: // SL đã giải quyết tháng hiện tại
                    case 2: // SL đã giải quyết tháng trước
                    case 3: // SL đã giải quyết từ đầu năm đến cuối tháng toDate
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày đóng KN</th>");
                        sb.Append("</tr>");
                        for (int i = 0; i < lstKhieuNaiInfo.Count; i++)
                        {
                            sb.Append("<tr>");
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", (i + 1));
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", lstKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td style='style='text-align:center''>{0}</td>", lstKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    default:

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 03/06/2015
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="nguyenNhanLoiId"></param>
        /// <param name="chiTietLoiId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoNguyenNhanLoi(int khuVucId, int doiTacId, int phongBanId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int nguyenNhanLoiId, int chiTietLoiId,
                                                        DateTime fromDate, DateTime toDate, int nguonKhieuNai)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoNguyenNhanLoi(khuVucId, doiTacId, phongBanId, loaiKhieuNaiId, linhVucChungId, linhVucConId,
                                                                                                                        nguyenNhanLoiId, chiTietLoiId, fromDate, toDate, nguonKhieuNai);
            if (listKhieuNaiInfo != null)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='6'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã phản ánh</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("<th>Ngày đóng KN</th>");
                sb.Append("<th>Nội dung PA</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                    sb.Append("</tr>");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 25/11/2015
        /// </summary>
        /// <param name="linhVucConId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public string ListKhieuNaiDVGTGTTapDoan(int linhVucConId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiDichVuGTGTTapDoan(linhVucConId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='7'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã phản ánh</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Ngày tiếp nhận</th>");
                sb.Append("<th>Nội dung PA</th>");
                sb.Append("<th>Nội dung xử lý</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungXuLyDongKN);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Báo cáo TTTC

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoDoiTac(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoDoiTac(doiTacId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoPhongBan(int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoPhongBanDoiTac(phongBanId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoNguoiDungPhongBan(int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoNguoiDungPhongBan(phongBanId, tenTruyCap, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='11'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='11'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoDoiTac(int doiTacId, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoDoiTac(doiTacId, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}','{1}')\">{0}</a></td>";

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='13'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId, listKhieuNaiInfo[i].ArchiveId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId, listKhieuNaiInfo[i].ArchiveId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId, listKhieuNaiInfo[i].ArchiveId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId, listKhieuNaiInfo[i].ArchiveId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoPhongBan(int phongBanId, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoPhongBanDoiTac(phongBanId, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoNguoiDungPhongBan(int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoNguoiDungPhongBan(phongBanId, tenTruyCap, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='12'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='12'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/09/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoDoiTac_V2(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoDoiTac_V2(doiTacId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    // Số lượng đã xử lý
                    case 3:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string tenPhongBanXuLy = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].TenPhongBanXuLy);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 7:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 8:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            //string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            //sb.Append("<tr>");
                            //sb.AppendFormat("<td>{0}</td>", (i + 1));
                            ////sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            ////sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            ////sb.Append("<td>&nbsp;</td>");
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            //sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("</tr>");


                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }


                        break;

                    case 9:
                    case 10:
                    case 11:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        //sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.Append("</tr>");
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoPhongBan_V2(int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoPhongBanDoiTac_V2(phongBanId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }
                        break;
                    case 7:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].GhiChu);
                            sb.Append("</tr>");
                        }
                        break;
                    case 8:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].GhiChu);
                            sb.Append("</tr>");
                        }

                        break;

                    case 9:
                    case 10:
                    case 11:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='12'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        //sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoNguoiDungPhongBan_V2(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoNguoiDungPhongBan_V2(doiTacId, phongBanId, tenTruyCap, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 7:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 8:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='7'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;

                    case 9:
                    case 10:
                    case 11:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='11'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.Append("</tr>");
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoNguoiDungPhongBan_V3(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoNguoiDungPhongBan_V3(doiTacId, phongBanId, tenTruyCap, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='12'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung PA</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 7:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 8:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='7'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;

                    case 9:
                    case 10:
                    case 11:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='11'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.Append("</tr>");
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        // Edited by	: Dao Van Duong
        // Datetime		: 12.8.2016 10:54
        // Note			: Bổ xung điều kiện lọc tỉnh/ huyện
        public string ListKhieuNaiTheoNguoiDungPhongBan_V4(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType, int tinhId, int huyenId)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoNguoiDungPhongBan_V4(doiTacId, phongBanId, tenTruyCap, fromDate, toDate, reportType, tinhId, huyenId);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='12'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung PA</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 7:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 8:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='7'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;

                    case 9:
                    case 10:
                    case 11:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='11'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.Append("</tr>");
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 19/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoNguoiDungPhongBan_V2(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoNguoiDungPhongBan_V2(doiTacId, phongBanId, tenTruyCap, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();
                string showPopupChiTietKhieuNai = string.Empty;
                //showPopupChiTietKhieuNai = "<td>{0}</td>";
                showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='10'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='11'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='11'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 7:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }
                        break;
                    case 8:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='7'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm"));

                            sb.Append("</tr>");
                        }

                        break;

                    case 9:
                    case 10:
                    case 11:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='12'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận PB</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayTiepNhan_NguoiXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year ? string.Empty : listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.Append("</tr>");
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 23/08/2014
        /// Todo : Lấy danh sách khiếu nại đang tồn đọng hoặc quá hạn
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="reportType">
        ///     = 1: Tồn đọng
        ///     = 2 : Quá hạn
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiDoiTacPhongBanTaiThoiDiemHienTai(int doiTacId, int phongBanId, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtKhieuNai = new ReportSqlImpl().ListKhieuNaiDoiTacPhongBanTaiThoiDiemHienTai(doiTacId, phongBanId, reportType);
            if (dtKhieuNai != null && dtKhieuNai.Rows.Count > 0)
            {
                if (dtKhieuNai.Rows.Count > 0)
                {
                    int index = 1;
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.AppendFormat("<td colspan='7'>Tổng số bản ghi : {0}</td>", dtKhieuNai.Rows.Count);
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã phản ánh</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Người tiếp nhận</th>");
                    sb.Append("<th>Phòng ban tiếp nhận</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("<th>Ngày hết hạn</th>");
                    sb.Append("</tr>");

                    foreach (DataRow row in dtKhieuNai.Rows)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", index);
                        sb.AppendFormat("<td>{0}</td>", row["Id"]);
                        sb.AppendFormat("<td>{0}</td>", row["SoThueBao"]);
                        sb.AppendFormat("<td>{0}</td>", row["NguoiXuLy"]);
                        sb.AppendFormat("<td>{0}</td>", phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanXuLyId"])));
                        sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayChuyenPhongBan"]).ToString("dd/MM/yyyy HH:mm"));
                        sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm"));
                        sb.Append("</tr>");

                        index++;
                    } // end foreach(DataRow row in dtKhieuNai.Rows)
                }
                else
                {
                    sb.Append("<tr><td colspan='7'>Không có dữ liệu</td><tr>");
                }

            } // end if(dtKhieuNai != null)
            else
            {
                sb.Append("<tr><td colspan='7'>Có lỗi xảy ra</td><tr>");

            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public string ListKhieuNaiDoiTacPhongBanTaiThoiDiemHienTai(int doiTacId, int phongBanId, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtKhieuNai = new ReportSqlImpl().ListKhieuNaiDoiTacPhongBanTaiThoiDiemHienTai(doiTacId, phongBanId, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
            if (dtKhieuNai != null && dtKhieuNai.Rows.Count > 0)
            {
                string showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";
                if (dtKhieuNai.Rows.Count > 0)
                {
                    int index = 1;
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.AppendFormat("<td colspan='11'>Tổng số bản ghi : {0}</td>", dtKhieuNai.Rows.Count);
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã phản ánh</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Loại khiếu nại</th>");
                    sb.Append("<th>Lĩnh vực chung</th>");
                    sb.Append("<th>Lĩnh vực con</th>");
                    sb.Append("<th>Nội dung phản ánh</th>");
                    sb.Append("<th>Người tiếp nhận</th>");
                    sb.Append("<th>Phòng ban tiếp nhận</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("<th>Ngày hết hạn</th>");
                    sb.Append("</tr>");

                    foreach (DataRow row in dtKhieuNai.Rows)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", index);
                        sb.AppendFormat(showPopupChiTietKhieuNai, row["Id"]);
                        sb.AppendFormat("<td>{0}</td>", row["SoThueBao"]);
                        sb.AppendFormat("<td>{0}</td>", row["LoaiKhieuNai"]);
                        sb.AppendFormat("<td>{0}</td>", row["LinhVucChung"]);
                        sb.AppendFormat("<td>{0}</td>", row["LinhVucCon"]);
                        sb.AppendFormat("<td>{0}</td>", row["NoiDungPA"]);
                        sb.AppendFormat("<td>{0}</td>", row["NguoiXuLy"]);
                        sb.AppendFormat("<td>{0}</td>", phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanXuLyId"])));
                        sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayChuyenPhongBan"]).ToString("dd/MM/yyyy HH:mm"));
                        sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm"));
                        sb.Append("</tr>");

                        index++;
                    } // end foreach(DataRow row in dtKhieuNai.Rows)
                }
                else
                {
                    sb.Append("<tr><td colspan='11'>Không có dữ liệu</td><tr>");
                }

            } // end if(dtKhieuNai != null)
            else
            {
                sb.Append("<tr><td colspan='11'>Có lỗi xảy ra</td><tr>");

            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/02/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="nguoiXuLy"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public string ListKhieuNaiTonDongQuaHanNguoiDungPhongBanHienTai(int phongBanId, string nguoiXuLy, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtKhieuNai = new ReportSqlImpl().ListKhieuNaiTonDongQuaHanNguoiDungPhongBanHienTai(phongBanId, nguoiXuLy, reportType);
            if (dtKhieuNai != null && dtKhieuNai.Rows.Count > 0)
            {
                if (dtKhieuNai.Rows.Count > 0)
                {
                    int index = 1;
                    PhongBanImpl phongBanImpl = new PhongBanImpl();

                    sb.Append("<tr>");
                    sb.AppendFormat("<td colspan='7'>Tổng số bản ghi : {0}</td>", dtKhieuNai.Rows.Count);
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã phản ánh</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Người tiếp nhận</th>");
                    sb.Append("<th>Phòng ban tiếp nhận</th>");
                    sb.Append("<th>Ngày tiếp nhận</th>");
                    sb.Append("<th>Ngày hết hạn</th>");
                    sb.Append("</tr>");

                    foreach (DataRow row in dtKhieuNai.Rows)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", index);
                        sb.AppendFormat("<td>{0}</td>", row["Id"]);
                        sb.AppendFormat("<td>{0}</td>", row["SoThueBao"]);
                        sb.AppendFormat("<td>{0}</td>", row["NguoiXuLy"]);
                        sb.AppendFormat("<td>{0}</td>", phongBanImpl.GetNamePhongBan(ConvertUtility.ToInt32(row["PhongBanXuLyId"])));
                        sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayChuyenPhongBan"]).ToString("dd/MM/yyyy HH:mm"));
                        sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm"));
                        sb.Append("</tr>");

                        index++;
                    } // end foreach(DataRow row in dtKhieuNai.Rows)
                }
                else
                {
                    sb.Append("<tr><td colspan='7'>Không có dữ liệu</td><tr>");
                }

            } // end if(dtKhieuNai != null)
            else
            {
                sb.Append("<tr><td colspan='7'>Có lỗi xảy ra</td><tr>");

            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 23/06/2015
        /// Todo : Lấy danh sách khiếu nại chất lượng phục vụ
        /// </summary>
        /// <param name="typeReport">
        ///     1 : Toàn mạng
        ///     2 : Khu vực
        ///     3 : Đối tác
        /// </param>
        /// <param name="doiTacId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="typeKhenChe">
        ///     = 1 : Khen
        ///     = 2 : Chê
        /// </param>
        /// <param name="nguonKhieuNai">
        ///     = -1 : all
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string ListKhieuNaiChatLuongPhucVu(int typeReport, int doiTacId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int typeKhenChe, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiChatLuongPhucVu(typeReport, doiTacId, loaiKhieuNaiId,
                                                                                                    linhVucChungId, linhVucConId, typeKhenChe, nguonKhieuNai, fromDate, toDate);
            if (listKhieuNaiReportInfo != null)
            {
                string showPopupChiTietKhieuNai = "<td valign='top'><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='6'>Tổng số bản ghi : {0}</td>", listKhieuNaiReportInfo.Count);
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã phản ánh</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Người tiếp nhận</th>");
                sb.Append("<th>Nội dung phản ánh</th>");
                sb.Append("<th>Nội dung xử lý</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td valign='top'>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiReportInfo[i].Id);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NguoiTiepNhan);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungPA);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungXuLyDongKN);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : 
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoLoaiKhieuNai(int khuVucId, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, List<int> listLinhVucConId, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoLoaiKhieuNai(khuVucId, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, nguonKhieuNai, fromDate, toDate);
            if (listKhieuNaiReportInfo != null)
            {
                sb.AppendFormat("<tr><td colspan='11'>Số lượng bản ghi : {0}</td></tr>", listKhieuNaiReportInfo.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Loại khiếu nại</th>");
                sb.Append("<th>Lĩnh vực chung</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Ngày tiếp nhận</th>");
                sb.Append("<th>Ngày đóng KN</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("<th>Nội dung phản ánh</th>");
                sb.Append("<th>Nội dung xử lý</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td valign='top'>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LoaiKhieuNai);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucChung);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].Id);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungPA);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungXuLyDongKN);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 13/07/2015
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="maTinhId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoLoaiKhieuNai(int khuVucId, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, List<int> listLinhVucConId, int maTinhId, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoLoaiKhieuNai(khuVucId, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, maTinhId, nguonKhieuNai, fromDate, toDate);
            if (listKhieuNaiReportInfo != null)
            {
                sb.AppendFormat("<tr><td colspan='11'>Số lượng bản ghi : {0}</td></tr>", listKhieuNaiReportInfo.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Loại khiếu nại</th>");
                sb.Append("<th>Lĩnh vực chung</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Ngày tiếp nhận</th>");
                sb.Append("<th>Ngày đóng KN</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("<th>Nội dung phản ánh</th>");
                sb.Append("<th>Nội dung xử lý</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td valign='top'>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LoaiKhieuNai);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucChung);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].Id);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungPA);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungXuLyDongKN);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author: Vu Van Truong
        /// Created date: 14/04/2016
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="maTinhId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoLoaiKhieuNaiactivity(string whereClause)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoLoaiKhieuNaiActivity(whereClause);
            if (listKhieuNaiReportInfo != null)
            {
                sb.AppendFormat("<tr><td colspan='11'>Số lượng bản ghi : {0}</td></tr>", listKhieuNaiReportInfo.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Loại khiếu nại</th>");
                sb.Append("<th>Lĩnh vực chung</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Ngày tiếp nhận</th>");
                sb.Append("<th>Ngày đóng KN</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("<th>Nội dung phản ánh</th>");
                sb.Append("<th>Nội dung xử lý</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td valign='top'>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LoaiKhieuNai);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucChung);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].Id);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungPA);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungXuLyDongKN);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author: Vu Van Truong
        /// Create date: 19/04/2016
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public string ListKhieuNaiTheoDanhSachKhieuNaiId(string whereClause)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoDanhSachKhieuNaiId_V2(whereClause);
            if (listKhieuNaiReportInfo != null)
            {
                sb.AppendFormat("<tr><td colspan='11'>Số lượng bản ghi : {0}</td></tr>", listKhieuNaiReportInfo.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Loại khiếu nại</th>");
                sb.Append("<th>Lĩnh vực chung</th>");
                sb.Append("<th>Lĩnh vực con</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Ngày tiếp nhận</th>");
                sb.Append("<th>Ngày đóng KN</th>");
                sb.Append("<th>Người xử lý</th>");
                sb.Append("<th>Nội dung phản ánh</th>");
                sb.Append("<th>Nội dung xử lý</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td valign='top'>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LoaiKhieuNai);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucChung);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].LinhVucCon);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].Id);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td valign='top'>{0}</td>", listKhieuNaiReportInfo[i].NguoiXuLy);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungPA);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NoiDungXuLyDongKN);
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="ngayQuaHan"></param>
        /// <returns></returns>
        public string ListKhieuNaiQuaHanPhongBan(int doiTacId, int phongBanId, DateTime ngayQuaHan)
        {
            StringBuilder sb = new StringBuilder();
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiQuaHanPhongBan(doiTacId, phongBanId, ngayQuaHan);
            if (listKhieuNaiReportInfo != null)
            {
                sb.AppendFormat("<tr><td colspan='5'>Số lượng bản ghi : {0}</td></tr>", listKhieuNaiReportInfo.Count);
                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã PAKN</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Ngày tiếp nhận</th>");
                sb.Append("<th>Ngày quá  hạn PB</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].KhieuNaiId);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                    sb.Append("</tr>");
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Báo cáo TT PTDV

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/09/2014
        /// Todo : Danh sách khiếu nại toàn mạng của PTDV
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public string ListKhieuNaiToanMangCuaPTDV(int khuVucId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();
            string showPopupChiTietKhieuNai = "<td><a href='#' onclick=\"ShowPoupChiTietKN('{0}')\">{0}</a></td>";

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiToanMangCuaPTDV_Solr(khuVucId, loaiKhieuNaiId, linhVucChungId, linhVucConId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại khiếu nại</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung phản ánh</th>");
                        sb.Append("<th>Ngày đóng khiếu nại</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            sb.AppendFormat(showPopupChiTietKhieuNai, listKhieuNaiInfo[i].Id);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;
                }
            }

            return sb.ToString();
        }

        #endregion                       

        #region Bao cao doi tac VNP

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/07/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="nguoiXuLy"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string ListKhieuNaiChuyenXuLyVNPTheoUser(int doiTacId, int phongBanId, string nguoiXuLy, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiChuyenXuLyVNPTheoUser(doiTacId, phongBanId, nguoiXuLy, fromDate, toDate);

            if (listKhieuNaiReportInfo != null)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='5'>Tổng số bản ghi : {0}</td>", listKhieuNaiReportInfo.Count);
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<th>STT</th>");
                sb.Append("<th>Mã phản ánh</th>");
                sb.Append("<th>Số thuê bao</th>");
                sb.Append("<th>Ngày chuyển</th>");
                sb.Append("<th>Phòng ban tiếp nhận</th>");
                sb.Append("</tr>");

                for (int i = 0; i < listKhieuNaiReportInfo.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (i + 1).ToString());
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].KhieuNaiId);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].SoThueBao);
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                    sb.AppendFormat("<td>{0}</td>", listKhieuNaiReportInfo[i].TenPhongBanXuLy);
                    sb.Append("</tr>");
                }
            }


            return sb.ToString();
        }


        #endregion

        #region Báo cáo VNPT NET

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/09/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoVNPTX(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoVNPTX(doiTacId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 3: // Số lượng đã xử lý (của số lượng lũy kế)
                    case 7: // Số lượng đã xử lý (của số lượng tiếp nhận)
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                        //case 7:
                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    //sb.Append("<th>ActivityId</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    sb.Append("<th>Ngày tiếp nhận</th>");
                        //    sb.Append("<th>Người tiếp nhận</th>");
                        //    sb.Append("<th>Phòng ban tiếp nhận</th>");
                        //    sb.Append("<th>Ngày chuyển phản ánh</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.Append("</tr>");
                        //    }
                        //    break;

                        //case 8:
                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    //sb.Append("<th>ActivityId</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        //    sb.Append("<th>Người xử lý</th>");
                        //    sb.Append("<th>Phòng ban xử lý</th>");
                        //    sb.Append("<th>Ngày xử lý</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        //sb.Append("<td>&nbsp;</td>");
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.Append("</tr>");
                        //    }

                        //    break;

                        //case 9:
                        //case 10:
                        //case 11:
                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    //sb.Append("<th>ActivityId</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    sb.Append("<th>Loại khiếu nại</th>");
                        //    sb.Append("<th>Nội dung phản ánh</th>");
                        //    sb.Append("<th>Ngày tiếp nhận</th>");
                        //    sb.Append("<th>Người xử lý</th>");
                        //    //sb.Append("<th>Phòng ban xử lý</th>");
                        //    sb.Append("<th>Ngày xử lý</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("<th>Phòng ban tiếp nhận</th>");
                        //    sb.Append("<th>Người tiếp nhận</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        //sb.Append("<td>&nbsp;</td>");
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                        //        sb.Append("</tr>");
                        //    }

                        //    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/09/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoDoiTac_V2_NET(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoDoiTac_V2_NET(doiTacId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();

                sb.Append("<tr>");
                sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                sb.Append("</tr>");

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                    case 7:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].IsCurrent ? string.Empty : listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.Append("</tr>");
                        }
                        break;

                        //case 7:
                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    //sb.Append("<th>ActivityId</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    sb.Append("<th>Ngày tiếp nhận</th>");
                        //    sb.Append("<th>Người tiếp nhận</th>");
                        //    sb.Append("<th>Phòng ban tiếp nhận</th>");
                        //    sb.Append("<th>Ngày chuyển phản ánh</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.Append("</tr>");
                        //    }
                        //    break;

                        //case 8:
                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    //sb.Append("<th>ActivityId</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        //    sb.Append("<th>Người xử lý</th>");
                        //    sb.Append("<th>Phòng ban xử lý</th>");
                        //    sb.Append("<th>Ngày xử lý</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        //sb.Append("<td>&nbsp;</td>");
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.Append("</tr>");
                        //    }

                        //    break;

                        //case 9:
                        //case 10:
                        //case 11:
                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    //sb.Append("<th>ActivityId</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    sb.Append("<th>Loại khiếu nại</th>");
                        //    sb.Append("<th>Nội dung phản ánh</th>");
                        //    sb.Append("<th>Ngày tiếp nhận</th>");
                        //    sb.Append("<th>Người xử lý</th>");
                        //    //sb.Append("<th>Phòng ban xử lý</th>");
                        //    sb.Append("<th>Ngày xử lý</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("<th>Phòng ban tiếp nhận</th>");
                        //    sb.Append("<th>Người tiếp nhận</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        //sb.Append("<td>&nbsp;</td>");
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                        //        sb.Append("</tr>");
                        //    }

                        //    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public string ListKhieuNaiTheoPhongBan_V2_NET(int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            StringBuilder sb = new StringBuilder();

            List<KhieuNai_ReportInfo> listKhieuNaiInfo = new ReportChiTietKhieuNaiImpl().ListKhieuNaiTheoPhongBanDoiTac_V2_NET(phongBanId, fromDate, toDate, reportType);
            if (listKhieuNaiInfo != null)
            {
                PhongBanImpl phongBanImpl = new PhongBanImpl();

                switch (reportType)
                {
                    case 1:
                    case 2:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại KN</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung PA</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày chuyển phản ánh</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }
                        break;

                    case 3:
                    case 7:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");

                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại KN</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung PA</th>");
                        //sb.Append("<th>Ngày tiếp nhận</th>");                                                
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            //sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }

                        break;
                    case 4:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại KN</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung PA</th>");
                        sb.Append("<th>Người xử lý</th>");
                        sb.Append("<th>Phòng ban xử lý</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }

                        break;

                    case 5:
                    case 6:
                        sb.Append("<tr>");
                        sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th>STT</th>");
                        //sb.Append("<th>ActivityId</th>");
                        sb.Append("<th>Mã PA</th>");
                        sb.Append("<th>Số thuê bao</th>");
                        sb.Append("<th>Loại KN</th>");
                        sb.Append("<th>Lĩnh vực chung</th>");
                        sb.Append("<th>Lĩnh vực con</th>");
                        sb.Append("<th>Nội dung PA</th>");
                        sb.Append("<th>Ngày tiếp nhận</th>");
                        sb.Append("<th>Người tiếp nhận</th>");
                        sb.Append("<th>Phòng ban tiếp nhận</th>");
                        sb.Append("<th>Ngày xử lý</th>");
                        sb.Append("<th>Ngày hết hạn</th>");
                        sb.Append("<th>Ghi chú</th>");
                        sb.Append("</tr>");

                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                            sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", (i + 1));
                            //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucChung);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LinhVucCon);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                            sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                            sb.Append("<td>&nbsp;</td>");
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                            sb.Append("</tr>");
                        }
                        break;
                        //case 7:
                        //    sb.Append("<tr>");
                        //    sb.AppendFormat("<td colspan='9'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        //    sb.Append("</tr>");
                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    sb.Append("<th>Ngày tiếp nhận</th>");
                        //    sb.Append("<th>Người tiếp nhận</th>");
                        //    sb.Append("<th>Phòng ban tiếp nhận</th>");
                        //    sb.Append("<th>Ngày chuyển phản ánh</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("<th>Ghi chú</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanTiepNhanId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LDate.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].GhiChu);
                        //        sb.Append("</tr>");
                        //    }
                        //    break;
                        //case 8:
                        //    sb.Append("<tr>");
                        //    sb.AppendFormat("<td colspan='8'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        //    sb.Append("</tr>");

                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    sb.Append("<th>Loại khiếu nại</th>");
                        //    sb.Append("<th>Nội dung phản ánh</th>");
                        //    sb.Append("<th>Ngày đóng khiếu nại</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("<th>Ghi chú</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyTruocId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].Id);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayDongKN.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].GhiChu);
                        //        sb.Append("</tr>");
                        //    }

                        //    break;

                        //case 9:
                        //case 10:
                        //case 11:
                        //    sb.Append("<tr>");
                        //    sb.AppendFormat("<td colspan='12'>Số lượng bản ghi : {0}</td>", listKhieuNaiInfo.Count);
                        //    sb.Append("</tr>");

                        //    sb.Append("<tr>");
                        //    sb.Append("<th>STT</th>");
                        //    //sb.Append("<th>ActivityId</th>");
                        //    sb.Append("<th>Mã PA</th>");
                        //    sb.Append("<th>Số thuê bao</th>");
                        //    sb.Append("<th>Loại khiếu nại</th>");
                        //    sb.Append("<th>Nội dung phản ánh</th>");
                        //    sb.Append("<th>Ngày tiếp nhận</th>");
                        //    sb.Append("<th>Người xử lý</th>");
                        //    //sb.Append("<th>Phòng ban xử lý</th>");
                        //    sb.Append("<th>Ngày xử lý</th>");
                        //    sb.Append("<th>Ngày hết hạn</th>");
                        //    sb.Append("<th>Phòng ban tiếp nhận</th>");
                        //    sb.Append("<th>Người tiếp nhận</th>");
                        //    sb.Append("<th>Ghi chú</th>");
                        //    sb.Append("</tr>");

                        //    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        //    {
                        //        string phongBanTiepNhan = phongBanImpl.GetNamePhongBan(listKhieuNaiInfo[i].PhongBanXuLyId);
                        //        sb.Append("<tr>");
                        //        sb.AppendFormat("<td>{0}</td>", (i + 1));
                        //        //sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].ActivityId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNaiId);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].SoThueBao);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].LoaiKhieuNai);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NoiDungPA);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        //sb.Append("<td>&nbsp;</td>");
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLyTruoc);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NgayQuaHan_PhongBanXuLyTruoc.ToString("dd/MM/yyyy HH:mm"));
                        //        sb.AppendFormat("<td>{0}</td>", phongBanTiepNhan);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].NguoiXuLy);
                        //        sb.AppendFormat("<td>{0}</td>", listKhieuNaiInfo[i].KhieuNai_GhiChu);
                        //        sb.Append("</tr>");
                        //    }

                        //    break;
                }
            }

            return sb.ToString();
        }

        #endregion

    }
}