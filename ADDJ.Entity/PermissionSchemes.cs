using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    public enum PermissionSchemes : int
    {
        Thêm_khiếu_nại = 1,
        Xử_lý_khiếu_nại = 2,
        Xóa_khiếu_nại = 3,
        Chuyển_xử_lý_khiếu_nại = 4,
        Thêm_người_theo_dõi = 5,
        Đóng_khiếu_nại = 6,
        Tạo_file_đính_kèm = 7,
        Xóa_file_đính_kèm_của_mình = 8,
        Xóa_tất_cả_file_đính_kèm = 9,
        Tạo_các_bước_xử_lý = 10,
        Sửa_các_bước_xử_lý_của_mình = 11,
        Xóa_các_bước_xử_lý_của_mình = 13,
        Xóa_tất_cả_các_bước_xử_lý = 14,
        Tạo_KN_cước = 15,
        Sửa_KN_cước= 16,
        Xóa_KN_cước_của_mình=17,
        Xóa_tất_cả_KN_cước=18,
        Tạo_kết_quả_giải_quyết_KN=19,
        Sửa_kết_quả_giải_quyết_KN_của_mình=20,
        Sửa_kết_quả_giải_quyết_KN_của_người_khác=21,
        Xóa_kết_quả_giải_quyết_KN_của_mình=22,
        Xóa_tất_cả_kết_quả_giải_quyết_KN=23,
        Tạo_dòng_trên_sổ_theo_dõi=24,
        Sửa_dòng_trên_sổ_theo_dõi=25,
        Xóa_dòng_trên_sổ_theo_dõi_của_mình=26,
        Xóa_tất_cả_dòng_trên_sổ_theo_dõi=27,
        Sửa_KN_phản_hồi_của_phòng_ban_sau_thời_gian_cấu_hình=28,
        Sửa_thông_tin_file_đính_kèm=29,
        Phân_việc_cho_người_dùng_trong_phòng = 30,
        Xem_báo_cáo_của_đối_tác_cùng_cấp = 31,
        Sửa_thông_tin_khiếu_nại = 32,
        Chuyển_phản_hồi_KN_trung_tâm = 33,
        Truy_vấn_khiếu_nại_trên_toàn_bộ_hệ_thống = 34,
        Phân_việc_cho_người_dùng_trong_phòng_ban_xử_lý = 35,
        Sửa_thông_tin_cây_thư_mục = 36,
        Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới = 37,
        Tiếp_nhận_khiếu_nại = 38,
        Xem_file_trên_hệ_thống = 39,
        Xem_file_tại_phòng_ban_xử_lý_KN = 40,
        Xem_file_toàn_hệ_thống = 41,
        Tiếp_nhận_KN_phản_hồi_về_người_gửi = 42,
        Xác_nhận_bù_tiền = 43,
        Đóng_khiếu_nại_do_phòng_ban_mình_tạo_ra = 44,
        Chuyển_phản_hồi_về_phòng_ban_tiếp_nhận = 45,
        Chuyển_khiếu_nại_cho_phòng_ban_cấp_dưới_xử_lý = 46,
		Xem_file_tại_khiếu_nại_xử_lý = 47,
_  }
}
