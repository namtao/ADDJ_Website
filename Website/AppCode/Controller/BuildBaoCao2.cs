using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Website.AppCode.Controller
{
    public class BuildBaoCao2
    {
        public static string BaoCaoChiTietGiamTru_DV_GTGT(int isDonVi, int isKhuVuc, int khuVucId, int doiTacId, int caNhanXuLy, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                //string selectClause = "a.Id MaPAKN,a.SoThueBao,a.LinhVucConId,a.LinhVucCon,a.NoiDungPA,a.NoiDungXuLyDongKN,a.NgayDongKN,a.PhongBanXuLyId,a.NguoiXuLyId,b.SoTien,b.SoTien_Edit";
                //string joinClause = "LEFT JOIN dbo.KhieuNai_SoTien b on a.id=b.KhieuNaiId";
                //string whereClause = "a.TrangThai=3 ";

                //if (theodonvihoaccanhan==1) // nếu theo cá nhân
                //{
                //    whereClause += " and a.NguoiXuLyId="+canhanxuly;
                //}
                //else // ngược lại theo đơn vị
                //{
                //    whereClause += " and a.PhongBanXuLyId = "+donviid;
                //}
                //string orderBy = "";
                //var lstKhieuNai = ServiceFactory.GetInstanceKhieuNai().GetListDynamicJoin(selectClause, joinClause, whereClause, orderBy);

                //DataTable dReport = ReportImpl2.BaoCaoChiTietGiamTruDVGTGT(theodonvihoaccanhan, donviid, canhanxuly, fromDate, toDate);

                DataTable   dReport =new ReportImpl2().BaoCaoChiTietGiamTruDVGTGT(isDonVi, isKhuVuc, khuVucId, doiTacId, caNhanXuLy, fromDate, toDate);

                if (dReport != null && dReport.Rows.Count> 0)
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'>Số lượng khiếu nại : " + dReport.Rows.Count + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='9'></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Mã PAKN</th>");
                    sb.Append("<th>Số thuê bao</th>");
                    sb.Append("<th>Lĩnh vực con (DV trong danh sách)</th>");
                    sb.Append("<th>Nội dung phản ánh</th>");
                    sb.Append("<th>Nội dung đóng KN</th>");
                    sb.Append("<th>Ngày đóng KN</th>");
                    sb.Append("<th>Đơn vị xử lý/use xử lý</th>");
                    sb.Append("<th>Số tiền giảm trừ</th>");
                    sb.Append("</tr>");

                    int index = 0;
                    foreach (DataRow item in dReport.Rows)
                    {
                        index++;
                        sb.Append("<tr>");
                        sb.Append("<td>" + index + "</td>");
                        sb.Append("<td>" + item["MaPAKN"] + "</td>");
                        sb.Append("<td>" + item["SoThueBao"] + "</td>");
                        sb.Append("<td>" + item["LinhVucCon"] + "</td>");
                        sb.Append("<td>" + item["NoiDungPA"] + "</td>");
                        sb.Append("<td>" + item["NoiDungDongKN"] + "</td>");
                        sb.Append("<td>" + item["NgayDongKN"] + "</td>");
                        sb.Append("<td>" + item["DonVi_User_XuLy"] + "</td>");
                        sb.Append("<td>" + item["SoTienGiamTru"] + "</td>");
                        sb.Append("</tr>");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        private static string layThongTinDonViXuLy(string donvixulyid)
        {
            try
            {
                var listDsNguoiDungPhongBan = ServiceFactory.GetInstanceDoiTac().GetInfo(Convert.ToInt32(donvixulyid));
                if (listDsNguoiDungPhongBan != null)
                    return listDsNguoiDungPhongBan.TenDoiTac;
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string BaoCaoTongHopTheoChiTieuThoiGianGiaiQuyet(string dsDonViTiepNhan, string mucTien1, string mucTien2,DateTime fromDate,DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                //string selectClause = "a.Id MaPAKN,a.SoThueBao,a.LinhVucConId,a.LinhVucCon,a.NoiDungPA,a.NoiDungXuLyDongKN,a.NgayDongKN,a.PhongBanXuLyId,a.NguoiXuLyId,b.SoTien,b.SoTien_Edit";
                //string joinClause = "LEFT JOIN dbo.KhieuNai_SoTien b on a.id=b.KhieuNaiId";
                //string whereClause = "a.TrangThai=3 ";

                //if (theodonvihoaccanhan==1) // nếu theo cá nhân
                //{
                //    whereClause += " and a.NguoiXuLyId="+canhanxuly;
                //}
                //else // ngược lại theo đơn vị
                //{
                //    whereClause += " and a.PhongBanXuLyId = "+donviid;
                //}
                //string orderBy = "";
                //var lstKhieuNai = ServiceFactory.GetInstanceKhieuNai().GetListDynamicJoin(selectClause, joinClause, whereClause, orderBy);

                //DataTable dReport = ReportImpl2.BaoCaoChiTietGiamTruDVGTGT(theodonvihoaccanhan, donviid, canhanxuly, fromDate, toDate);

                DataTable dReport = new ReportImpl2().BaoCaoTongHopTheoChiTieuThoiGianGiaiQuyet(dsDonViTiepNhan, mucTien1, mucTien2, fromDate, toDate);

                if (dReport != null && dReport.Rows.Count > 0)
                {
                    sb.Append("<tr>");
                    sb.Append("<th></th>");
                    sb.Append("<th>Toàn mạng</th>");
                    sb.Append("<th>KTV đối tác/GDV</th>");
                    sb.Append("<th>Trưởng ca đối tác</th>");
                    sb.Append("<th>KTV VNP</th>");
                    sb.Append("<th>Nhân viên XLNV/GQKN/Cửa hàng trưởng</th>");
                    sb.Append("<th>Tổ trưởng XKNV/GQKN/CV P.Nghiệp vụ</th>");
                    sb.Append("<th>LĐ Đài HTKH/Trưởng P.Nghiệp vụ</th>");
                    sb.Append("<th>CV P.CSKH</th>");
                    sb.Append("<th>LĐ TTKD</th>");
                    sb.Append("<th>LĐ P.CSKH</th>");
                    sb.Append("</tr>");

                    int index = 0;
                    foreach (DataRow item in dReport.Rows)
                    {
                        index++;
                        sb.Append("<tr>");
                        sb.Append("<td><b>" + item["TenCot"] + "</b></td>");
                        sb.Append("<td>" + item["ToanMang"] + "</td>");
                        sb.Append("<td>" + item["GTVDoiTac_GDV"] + "</td>");
                        sb.Append("<td>" + item["TruongCaDoiTac"] + "</td>");
                        sb.Append("<td>" + item["KTV_VNP"] + "</td>");
                        sb.Append("<td>" + item["NV_XLNV_GQKN_CuaHangTruong"] + "</td>");
                        sb.Append("<td>" + item["TT_XLNV_GQKN_CV_P_NghiepVu"] + "</td>");
                        sb.Append("<td>" + item["LD_Dai_HTKH_Truong_P_NghiepVu"] + "</td>");
                        sb.Append("<td>" + item["CV_P_CSKH"] + "</td>");
                        sb.Append("<td>" + item["LD_TTKD"] + "</td>");
                        sb.Append("<td>" + item["LD_P_CSKH"] + "</td>");
                        sb.Append("</tr>");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static string BaoCaoTongHopChiTietTheoChiTieuThoiGianGiaiQuyet(string dsDonViTiepNhan, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                DataTable dReport = new ReportImpl2().BaoCaoTongHopChiTietTheoChiTieuThoiGianGiaiQuyet(dsDonViTiepNhan, fromDate, toDate);

                if (dReport != null && dReport.Rows.Count > 0)
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan='11'>Số lượng khiếu nại : " + dReport.Rows.Count + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='11'></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<th>STT</th>");
                    sb.Append("<th>Số TB</th>");
                    sb.Append("<th>Mã PA</th>");
                    sb.Append("<th>Nội dung PA</th>");
                    sb.Append("<th>Người tiếp nhậnc</th>");
                    sb.Append("<th>Người xử lý</th>");
                    sb.Append("<th>Người đóng KN</th>");
                    sb.Append("<th>Thời gian tiếp nhận</th>");
                    sb.Append("<th>Thời gian đóng KN</th>");
                    sb.Append("<th>Cấp phê duyệt giảm trừ</th>");
                    sb.Append("<th>Số tiền giảm trừ</th>");
                    sb.Append("</tr>");

                    int index = 0;
                    foreach (DataRow item in dReport.Rows)
                    {
                        index++;
                        sb.Append("<tr>");
                        sb.Append("<td>" + index + "</b></td>");
                        sb.Append("<td>" + item["SoTB"] + "</b></td>");
                        sb.Append("<td>" + item["MaKN"] + "</td>");
                        sb.Append("<td>" + item["NoiDungPA"] + "</td>");
                        sb.Append("<td>" + item["NguoiTiepNhan"] + "</td>");
                        sb.Append("<td>" + item["NguoiXuLy"] + "</td>");
                        sb.Append("<td>" + item["NguoiDongKN"] + "</td>");
                        sb.Append("<td>" + item["ThoiGianTiepNhan"] + "</td>");
                        sb.Append("<td>" + item["ThoiGianDongKN"] + "</td>");
                        sb.Append("<td>" + item["CapPheDuyetGiamTru"] + "</td>");
                        sb.Append("<td>" + item["SoTienGiamTru"] + "</td>");
                        sb.Append("</tr>");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}