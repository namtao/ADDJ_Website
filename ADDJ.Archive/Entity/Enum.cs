using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GQKN.Archive.Entity
{
    public enum KhieuNai_DoUuTien_Type : byte
    {
        Thông_thường = 1,
        Khẩn_cấp = 2,
        Đặc_biệt = 3,
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
        Lỗi_Hệ_Thống = 1,
        Giao_Dịch_Viên = 2,
        Chăm_Sóc_Khách_Hàng = 3,
        Lý_Do_Khác = 4
    }
    ///// <summary>
    ///// Author : Phi Hoang Hai
    ///// Created date : 10/12/2013
    /////   Edited : 07/04/2014 : Comment lại bởi vì định nghĩa trong DoiTacInfo.DoiTacTypeValue
    ///// </summary>
    //public enum DoiTac_DoiTacType : byte
    //{
    //    VNP = 1,
    //    TTTC = 2,
    //    VAS = 3,
    //    VNPTTT = 4,
    //    Doi_Tac_VNP = 5

    //}    
}
