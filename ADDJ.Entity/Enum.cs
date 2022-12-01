using ADDJ.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    public enum KhieuNai_DoUuTien_Type : byte
    {
        Thông_thường = 1,
        Khẩn_cấp = 2,
        Đặc_biệt = 3,
        Hủy = 4,  
    }

    public enum KhieuNai_TrangThai_Type : byte
    {
        Chờ_xử_lý = 0,
        Đang_xử_lý = 1,
        Chờ_đóng = 2,
        Đóng = 3,
    }

    public enum KhieuNai_Actitivy_HanhDong : byte
    {
        Tạo_Mới = 0,
        Chuyển_Ngang_Hàng = 1,
        Chuyển_Phòng_Ban = 2,
        KN_Phản_Hồi = 3,
        Đóng_KN = 4,
    }

    public enum KhieuNai_HTTiepNhan_Type : byte
    {
        Điểm_giao_dịch = 1,
        Cổng_CSKH_9191 = 2,
        Email = 3,
        Đơn_thư = 5,
        Điện_thoại = 6,
        Khác = 99,

    }

    public enum NguoiSuDung_TrangThai : int
    {
        Không_sử_dụng = 0,
        Sử_dụng = 1,
    }

    public enum NguoiSuDung_NhomNguoiDung : int
    {
        Vinaphone = 1,
        Kiểm_soát_viên = 2,
        Khu_vực = 3,
        Đối_tác = 4,
        Khai_thác_viên = 5,
        Kiểm_soát_viên_khu_vực = 6
    }

    public enum KhieuNai_LoaiTien : int
    {
        Tài_khoản_chính = 1,
        Tài_khoản_khuyến_mại = 2,
        Tài_khoản_khuyến_mại_1 = 3,
        Tài_khoản_khuyến_mại_2 = 4,
        Tài_khoản_Data = 5,
        Tài_khoản_khác = 6
    }

    public enum KhieuNai_LoaiTien_TraSau : int
    {
        GPRS = 10,
        CP = 11,
        Thoại = 12,
        SMS = 13,
        IR = 14,
        Tài_khoản_khác = 15,
    }

    public enum FileDinhKem_Status : byte
    {
        File_KH_Gửi = 1,
        File_GQKN_Gửi = 2
    }


    public enum LyDoGiamTru_GiaCuoc : int
    {
        Chọn_Lý_Do = 0,
        Lỗi_Hệ_Thống = 1,
        Giao_Dịch_Viên = 2,
        Chăm_Sóc_Khách_Hàng = 3,
        Lý_Do_Khác = 4
    }

    public enum KhieuNai_DoHaiLong_Type : int
    {
        //Chưa_đóng_khiếu_nại=-1,
        Rất_hài_lòng = 0,
        Hài_lòng = 1,
        Không_hài_lòng = 2,
        KH_phản_ứng_gay_gắt = 3,
        Không_liên_lạc_được_KH = 4,
        Ý_kiến_khác = 5
    }

    public enum KhieuNai_NguonKhieuNai : int
    {
        GQKN = 0,
        HNI = 1,
        IB = 2,
        OB_SAM = 3
    }

    public enum LoiKhieuNai_Loai : int
    {
        Hỗ_trợ = 1,
        Khiếu_nại = 2,
        Khiếu_nại_CP = 3,
        Khiếu_nại_NET = 4,
        Khiếu_nại_VNP = 5
    }
    public enum PhongBan_TrangThai : int
    {
        [Name("Hoạt động")]
        HoatDong = 1,

        [Name("Không hoạt động")]
        KhongHoatDong = 0,

        // Không sử dụng nữa, sử dụng ngầm mà thôi
        // [Name("Đã xóa")]
        // DaXoa = -1,
    }

    public enum DonViQuanLy : int
    {
        [ThuTu(1)]
        [Name("VNPT Vinaphone")]
        Vinaphone = 1,

        [ThuTu(2)]
        [Name("VNPT Media")]
        Media = 2,
    }
}
