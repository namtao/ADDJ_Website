using ADDJ.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Log.Entity
{
    public enum ActionLog : int
    {
        [ThuTu(1)]
        [Name("Thêm mới")]
        ThemMoi = 1,

        [ThuTu(2)]
        [Name("Sửa")]
        Sua = 2,

        [ThuTu(3)]
        [Name("Xóa")]
        Xoa = 3,

        [ThuTu(4)]
        [Name("Đăng nhập hệ thống")]
        Login = 1000,

        [ThuTu(5)]
        [Name("Đăng xuất hệ thống")]
        Logout = 9999,

        [ThuTu(0)]
        [Name("Hệ thống")]
        System = 8888,

        //[ThuTu(6)]
        //[Name("Mở dịch vụ VAS")]
        //Mở_Dịch_Vụ_VAS = 1002,

        //[ThuTu(7)]
        //[Name("Mở dịch vụ 3G")]
        //Mở_Dịch_Vụ_3G = 1003,

        //// Action khi gọi dịch vụ
        //[ThuTu(8)]
        //[Name("Gọi Dịch Vụ Subinfo")]
        //Gọi_Dịch_Vụ_Subinfo = 2001,

        //[ThuTu(9)]
        //[Name("Mở dịch vụ 3G")]
        //Gọi_Dịch_Vụ_3G = 2002,

        //[ThuTu(10)]
        //[Name("Gọi Dịch Vụ VAS")]
        //Gọi_Dịch_Vụ_VAS = 2003,

        //[ThuTu(11)]
        //[Name("Gọi Dịch Vụ Portal")]
        //Gọi_Dịch_Vụ_Portal = 2004,

        //[ThuTu(12)]
        //[Name("Gọi Dịch Vụ TKTT")]
        //Gọi_Dịch_Vụ_TKTT = 2005,

        //[ThuTu(13)]
        //[Name("Gọi Dịch Vụ TS")]
        //Gọi_Dịch_Vụ_TS = 2006,

        //[ThuTu(14)]
        //[Name("Gọi Dịch Vụ TT")]
        //Gọi_Dịch_Vụ_TT = 2007,

        //[ThuTu(15)]
        //[Name("Khóa khiếu nại")]
        //Khoa_Khieu_Nai = 3001,

        //[ThuTu(16)]
        //[Name("Thêm file khiếu nại")]
        //Them_File_Khieu_Nai = 3002,

        //[ThuTu(17)]
        //[Name("Thêm khiếu nại")]
        //Them_Khieu_Nai = 3003,
    }

    public enum ObjTypeLog : int
    {
        //[ThuTu(3)]
        //[Name("Dịch vụ VAS")]
        //WS_VAS = 24,

        //[ThuTu(4)]
        //[Name("Dịch vụ 3G")]
        //WS_3G = 25,

        //[ThuTu(5)]
        //[Name("Dịch vụ VNPT HNI")]
        //SERVICE_VNPT_HNI = 26,

        //[ThuTu(2)]
        //[Name("Quản lý lỗi khiếu nại")]
        //LoiKhieuNai = 27,

        [ThuTu(1)]
        [Name("Hệ thống")]
        System = 88,

        [ThuTu(6)]
        [Name("Khác")]
        Other = 99,
    }
}
