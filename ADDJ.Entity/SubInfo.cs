using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIVietNam.Core;

namespace AIVietNam.GQKN.Entity
{
    public class Subinfo
    {
        /// <summary>
        /// 0: Success
        /// 1: No data found
        /// Other: System error
        /// </summary>
        public string ErrorID { get; set; }

        public string SO_TB { get; set; }
    }

    public class VasSubinfo
    {
        /// <summary>
        /// 0: Success
        /// Other: System error
        /// </summary>
        public string ErrorId { get; set; }

        public string ErrorDescription { get; set; }

        public string RequestId { get; set; }

        public string ResponseTime { get; set; }
    }

    public class BasicInfoFromSubinfo : Subinfo
    {
        public string SO_MSIN { get; set; }

        public string TRANG_THAI { get; set; }

        public string GOI_DI { get; set; }

        public string GOI_DEN { get; set; }

        public string TEN_LOAI { get; set; }

        public string MA_TINH { get; set; }

        public string NGAY_KH { get; set; }

        public string SO_PIN { get; set; }

        public string SO_PUK { get; set; }

        public string PIN2 { get; set; }

        public string PUK2 { get; set; }
    }

    public class BasicServiceFromSubinfo : Subinfo
    {
        public List<ServiceFromSubinfo> ListService { get; set; }
    }

    public class ServiceFromSubinfo
    {
        public string MA_DV { get; set; }

        public string TEN_DV { get; set; }
    }

    public class SubHistoryFromSubinfo : Subinfo
    {
        public List<HistoryFromSubinfo> ListHistory { get; set; }
    }

    public class HistoryFromSubinfo
    {
        //(dd/MM/yyyy HH:mm:ss)
        public string NGAY_THANG { get; set; }

        public string MA_DV { get; set; }

        public string THAO_TAC { get; set; }

        public string GHI_CHU { get; set; }

        public string NGUOI_DUNG { get; set; }

        public string SO_MSIN_CU { get; set; }

        public string SO_MSIN_MOI { get; set; }

        public string MA_TINH_CU { get; set; }

        public string MA_TINH_MOI { get; set; }
    }

    public class History3GFromSubinfo : Subinfo
    {
        public List<History3GDetailFromSubinfo> ListHistory3G { get; set; }
    }

    public class History3GDetailFromSubinfo
    {
        public string MA_DV { get; set; }

        public string TEN_GOI { get; set; }

        //(dd/MM/yyyy HH:mm:ss)
        public string NGAY_BAT_DAU { get; set; }

        //(dd/MM/yyyy HH:mm:ss)
        public string NGAY_KET_THUC { get; set; }

        public string ACTIVE { get; set; }

        public string GIA_HAN { get; set; }

        //(0: trả trước, 1: trả sau)
        public string LOAI_TB { get; set; }
    }

    public class DeliverSMHistoryFromSubinfo : Subinfo
    {
        public List<DeliverSMHistoryDetailFromSubinfo> ListDeliverSMHistory { get; set; }
    }

    public class DeliverSMHistoryDetailFromSubinfo
    {
        public string DIA_CHI_NHAN { get; set; }

        public string DIA_CHI_GUI { get; set; }

        public string NOI_DUNG { get; set; }

        //(dd/MM/yyyy HH:mm:ss)
        public string THOI_GIAN { get; set; }
    }

    public class SubmitSMHistoryFromSubinfo : Subinfo
    {
        public List<SubmitSMHistoryDetailFromSubinfo> ListSubmitSMHistory { get; set; }
    }

    public class SubmitSMHistoryDetailFromSubinfo
    {
        public string DIA_CHI_NHAN { get; set; }

        public string DIA_CHI_GUI { get; set; }

        public string NOI_DUNG { get; set; }

        //(dd/MM/yyyy HH:mm:ss)
        public string THOI_GIAN { get; set; }
    }

    public class PPSCardInfoFromSubinfo : Subinfo
    {
        public List<PPSCardInfoDetailFromSubinfo> ListPPSCardInfo { get; set; }
    }

    public class PPSCardInfoDetailFromSubinfo
    {
        public string SO_TB { get; set; }

        //(dd/MM/yyyy HH:mm:ss)
        public string NGAY_GIO_NAP_THE { get; set; }

        public string SO_SERIAL { get; set; }

        public string TRANG_THAI { get; set; }

        public string MENH_GIA_THE { get; set; }
    }

    public class PPSCardHistoryByMsisdnFromSubinfo : Subinfo
    {
        public List<PPSCardHistoryByMsisdnDetailFromSubinfo> ListPPSCardHistoryByMsisdn { get; set; }
    }

    public class PPSCardHistoryByMsisdnDetailFromSubinfo
    {
        //(dd/MM/yyyy HH:mm:ss)
        public string NGAY_NAP { get; set; }

        public string MENH_GIA { get; set; }

        public string TKC_TRUOC_KHI_NAP { get; set; }

        public string TKC_SAU_KHI_NAP { get; set; }

        public string TKKM_TRUOC_KHI_NAP { get; set; }

        public string TKKM_SAU_KHI_NAP { get; set; }

        public string TKKM1_TRUOC_KHI_NAP { get; set; }

        public string TKKM1_SAU_KHI_NAP { get; set; }

        public string TKKM2_TRUOC_KHI_NAP { get; set; }

        public string TKKM2_SAU_KHI_NAP { get; set; }

        public string PHUONG_THUC_NAP { get; set; }
    }

    public class SubmitSMHistoryByDateFromSubinfo : Subinfo
    {
        public List<SubmitSMHistoryByDateDetailFromSubinfo> ListSubmitSMHistoryByDate { get; set; }
    }

    public class SubmitSMHistoryByDateDetailFromSubinfo
    {
        public string DIA_CHI_NHAN { get; set; }

        public string DIA_CHI_GUI { get; set; }

        public string NOI_DUNG { get; set; }

        //(dd/MM/yyyy HH:mm:ss)
        public string THOI_GIAN { get; set; }
    }

    public class DeliverSMHistoryByDateFromSubinfo : Subinfo
    {
        public List<DeliverSMHistoryByDateDetailFromSubinfo> ListDeliverSMHistoryByDate { get; set; }
    }

    public class DeliverSMHistoryByDateDetailFromSubinfo
    {
        public string DIA_CHI_NHAN { get; set; }

        public string DIA_CHI_GUI { get; set; }

        public string NOI_DUNG { get; set; }

        //(dd/MM/yyyy HH:mm:ss)
        public string THOI_GIAN { get; set; }
    }

    public class AloInfoFromSubinfo : Subinfo
    {
        /*
         * Có dữ liệu: Tên bộ ALO, 
         * Số tiền được cộng hàng tháng, 
         * Data được cộng hàng tháng, 
         * Số tháng còn được KM, 
         * Tổng số tháng được KM.
         */
        public List<AloInfoDetailFromSubinfo> Du_Lieu_ALO { get; set; }
    }

    public class AloInfoDetailFromSubinfo
    {
        public string Name { get; set; }
        public string SoTienCongHangThang { get; set; }
        public string DataCongHangThang { get; set; }
        public string SoThangConKM { get; set; }
        public string TongSoThangKM { get; set; }
    }

    public class PackagePromoPPSFromSubinfo : Subinfo
    {
        public List<PackagePromoPPSDetailFromSubinfo> ListPackagePromoPPS { get; set; }
    }

    public class PackagePromoPPSDetailFromSubinfo
    {
        public string TEN_GOI { get; set; }
    }

    public class VNP_TRACUU_TBInfo
    {
        public string Input_STB { get; set; }
        public string SO_MAY { get; set; }
        public string Ma_KH { get; set; }
        public string Ngay_Sinh { get; set; }
        public string Ten_TT { get; set; }
        public string Dia_Chi_TT { get; set; }

        // Mở (A), Khóa (D)
        public string GOI_DI { get; set; }

        // Mở (A), Khóa (D)
        public string GOI_DEN { get; set; }

        public string MA_TINH { get; set; }
    }

    public class VNP_CCS_ADMIN_PTTB_LAPHD_LAYTT_TBInfo
    {
        public string MA_KH { get; set; }
        public string MA_TB { get; set; }
        public string LOAITB_ID { get; set; }
        public string TEN_TB { get; set; }
        public string TEN_TT { get; set; }
        public string DIACHI_TT { get; set; }
        public string DIACHI_CT { get; set; }
        public string SO_GT { get; set; }
        public string DIENTHOAI_LH { get; set; }
        public string MS_THUE { get; set; }
        public string TRANGTHAI_ID { get; set; }
        public string DIADIEMTT_ID { get; set; }
        public string QUOCTICH { get; set; }
        public string DIACHI_CT1 { get; set; }
        public string TEN_KH { get; set; }
        public string LOAIKH_ID { get; set; }
        public string MIENCUOC { get; set; }
        public string PHUONG_ID { get; set; }
        public string PHO_ID { get; set; }
        public string QUAN_ID { get; set; }
        public string LOAIGT_ID { get; set; }
        public string SODAIDIEN { get; set; }
        public string SO_NHA { get; set; }
        public string NGAYCAP_GT { get; set; }
        public string PHAI { get; set; }
        public string MA_CQ { get; set; }
        public string MA_BC { get; set; }
        public string KHLON_ID { get; set; }
        public string NGAYSINH { get; set; }
        public string MA_T { get; set; }
        public string DONVIQL_ID { get; set; }
        public string NGANHNGHE_ID { get; set; }
        public string UUTIEN_ID { get; set; }
        public string DANGKY_DB { get; set; }
        public string DANGKY_TV { get; set; }
        public string QUANTT_ID { get; set; }
        public string PHUONGTT_ID { get; set; }
        public string PHOTT_ID { get; set; }
        public string SOTT_NHA { get; set; }
        public string MA_NV { get; set; }
        public string TTNO { get; set; }
    }

    public class PrepaidSubscriberInfo
    {
        public long MSISDN { get; set; }
        public string FULLNAME { get; set; }
        public string IDNUMBER { get; set; }
        public string BIRTHDAY { get; set; }
        public string GENDER { get; set; }
        public string COMPANY { get; set; }
        public string AGENTID { get; set; }
        public string REGISTERDATE { get; set; }
        public string REGISTERMETHODID { get; set; }
        public string NATIONALITYID { get; set; }
        public string ADDRESS { get; set; }
        public string IDNUMBERTYPE { get; set; }
        public string PLACEOFISSUE { get; set; }
        public string PLACEDATE { get; set; }
        public string ADDRESS_COMPANY { get; set; }
    }

    public class TBFullInfo : VNP_CCS_ADMIN_PTTB_LAPHD_LAYTT_TBInfo
    {
        public string Balance { get; set; }
        public string BalanceKM { get; set; }
        public string BalanceKM1 { get; set; }
        public string BalanceKM2 { get; set; }
        public string BalanceData { get; set; }
        public string HSD { get; set; }

        public string SO_MSIN { get; set; }

        public string TRANG_THAI { get; set; }

        public string TEN_LOAI { get; set; }

        public string NGAY_KH { get; set; }

        public string SO_PIN { get; set; }

        public string SO_PUK { get; set; }

        public string PIN2 { get; set; }

        public string PUK2 { get; set; }

        public string Input_STB { get; set; }
        public string SO_MAY { get; set; }
        public string Ma_KH { get; set; }
        public string Ngay_Sinh { get; set; }
        public string Ten_TT { get; set; }
        public string Dia_Chi_TT { get; set; }

        // Mở (A), Khóa (D)
        public string GOI_DI { get; set; }

        // Mở (A), Khóa (D)
        public string GOI_DEN { get; set; }


        public string MA_TINH { get; set; }

        public string MessageWarningKhieuNai { get; set; }

        public TBFullInfo()
        {
        }

        public TBFullInfo(VNP_CCS_ADMIN_PTTB_LAPHD_LAYTT_TBInfo inf, BasicInfoFromSubinfo basinfo, VNP_TRACUU_TBInfo infoTB)
        {
            if (basinfo != null)
            {
                SO_MSIN = basinfo.SO_MSIN;
                TRANG_THAI = basinfo.TRANG_THAI;
                TEN_LOAI = basinfo.TEN_LOAI;
                NGAY_KH = basinfo.NGAY_KH;
                SO_PIN = basinfo.SO_PIN;
                SO_PUK = basinfo.SO_PUK;
                PIN2 = basinfo.PIN2;
                PUK2 = basinfo.PUK2;
                GOI_DI = basinfo.GOI_DI;
                GOI_DEN = basinfo.GOI_DEN;
                MA_TINH = basinfo.MA_TINH;
            }
            else
            {
                SO_MSIN = "";
                TRANG_THAI = "";
                TEN_LOAI = "";
                NGAY_KH = "";
                SO_PIN = "";
                SO_PUK = "";
                PIN2 = "";
                PUK2 = "";
                GOI_DI = "";
                GOI_DEN = "";
                MA_TINH = "";
            }

            if (inf != null)
            {
                MA_KH = inf.MA_KH;
                MA_TB = inf.LOAITB_ID;
                LOAITB_ID = inf.LOAITB_ID;
                TEN_TB = inf.TEN_TB;
                TEN_TT = inf.TEN_TT;
                DIACHI_TT = inf.DIACHI_TT != null && inf.DIACHI_TT.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(inf.DIACHI_TT) : inf.DIACHI_TT;
                DIACHI_CT = inf.DIACHI_CT != null && inf.DIACHI_CT.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(inf.DIACHI_CT) : inf.DIACHI_CT;
                SO_GT = inf.SO_GT;
                DIENTHOAI_LH = inf.DIENTHOAI_LH;
                MS_THUE = inf.MS_THUE;
                TRANGTHAI_ID = inf.TRANGTHAI_ID;
                DIADIEMTT_ID = inf.DIADIEMTT_ID;
                QUOCTICH = inf.QUOCTICH;
                DIACHI_CT1 = inf.DIACHI_CT1 != null && inf.DIACHI_CT1.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(inf.DIACHI_CT1) : inf.DIACHI_CT1;
                TEN_KH = inf.TEN_KH;
                LOAIKH_ID = inf.LOAIKH_ID;
                MIENCUOC = inf.MIENCUOC;
                PHUONG_ID = inf.PHUONG_ID;
                PHO_ID = inf.PHO_ID;
                QUAN_ID = inf.QUAN_ID;
                LOAIGT_ID = inf.LOAIGT_ID;
                SODAIDIEN = inf.SODAIDIEN;
                SO_NHA = inf.SO_NHA;
                NGAYCAP_GT = inf.NGAYCAP_GT;
                PHAI = inf.PHAI;
                MA_CQ = inf.MA_CQ;
                MA_BC = inf.MA_BC;
                KHLON_ID = inf.KHLON_ID;
                Ngay_Sinh = infoTB == null ? inf.NGAYSINH : infoTB.Ngay_Sinh;
                NGAYSINH = Ngay_Sinh;
                MA_T = inf.MA_T;
                DONVIQL_ID = inf.DONVIQL_ID;
                NGANHNGHE_ID = inf.NGANHNGHE_ID;
                UUTIEN_ID = inf.UUTIEN_ID;
                DANGKY_DB = inf.DANGKY_DB;
                DANGKY_TV = inf.DANGKY_TV;
                QUANTT_ID = inf.QUANTT_ID;
                PHUONGTT_ID = inf.PHUONGTT_ID;
                PHOTT_ID = inf.PHOTT_ID;
                SOTT_NHA = inf.SOTT_NHA != null && inf.SOTT_NHA.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(inf.SOTT_NHA) : inf.SOTT_NHA;
                MA_NV = inf.MA_NV;
                TTNO = inf.TTNO;
            }
        }


    }
    public class TBTraTruocFullInfo : TBFullInfo
    {
        public long MSISDN { get; set; }
        public string FULLNAME { get; set; }
        public string IDNUMBER { get; set; }
        public string BIRTHDAY { get; set; }
        public string GENDER { get; set; }
        public string COMPANY { get; set; }
        public string AGENTID { get; set; }
        public string REGISTERDATE { get; set; }
        public string REGISTERMETHODID { get; set; }
        public string NATIONALITYID { get; set; }
        public string ADDRESS { get; set; }
        public string IDNUMBERTYPE { get; set; }
        public string PLACEOFISSUE { get; set; }
        public string PLACEDATE { get; set; }
        public string ADDRESS_COMPANY { get; set; }

        public TBTraTruocFullInfo()
        {
        }

        public TBTraTruocFullInfo(PrepaidSubscriberInfo inf, TBFullInfo basinfo, SubinfoTKTTEntity infoTaiKhoan)
        {
            if (infoTaiKhoan != null)
            {
                Balance = infoTaiKhoan.Balance;
                BalanceData = infoTaiKhoan.BalanceData;
                BalanceKM = infoTaiKhoan.BalanceKM;
                BalanceKM1 = infoTaiKhoan.BalanceKM1;
                BalanceKM2 = infoTaiKhoan.BalanceKM2;
                HSD = infoTaiKhoan.HSD;
            }
            if (basinfo != null)
            {
                SO_MSIN = basinfo.SO_MSIN;
                TRANG_THAI = basinfo.TRANG_THAI;
                TEN_LOAI = basinfo.TEN_LOAI;
                NGAY_KH = basinfo.NGAY_KH;
                SO_PIN = basinfo.SO_PIN;
                SO_PUK = basinfo.SO_PUK;
                PIN2 = basinfo.PIN2;
                PUK2 = basinfo.PUK2;
                GOI_DI = basinfo.GOI_DI;
                GOI_DEN = basinfo.GOI_DEN;

                MA_KH = basinfo.MA_KH;
                MA_TB = basinfo.LOAITB_ID;
                LOAITB_ID = basinfo.LOAITB_ID;
                TEN_TB = basinfo.TEN_TB;
                TEN_TT = basinfo.TEN_TT;
                DIACHI_TT = basinfo.DIACHI_TT != null && basinfo.DIACHI_TT.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(basinfo.DIACHI_TT) : basinfo.DIACHI_TT;
                DIACHI_CT = basinfo.DIACHI_CT!=null && basinfo.DIACHI_CT.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(basinfo.DIACHI_CT) : basinfo.DIACHI_CT;
                SO_GT = basinfo.SO_GT;
                DIENTHOAI_LH = basinfo.DIENTHOAI_LH;
                MS_THUE = basinfo.MS_THUE;
                TRANGTHAI_ID = basinfo.TRANGTHAI_ID;
                DIADIEMTT_ID = basinfo.DIADIEMTT_ID;
                QUOCTICH = basinfo.QUOCTICH;
                DIACHI_CT1 = basinfo.DIACHI_CT1 != null && basinfo.DIACHI_CT1.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(basinfo.DIACHI_CT1) : basinfo.DIACHI_CT1;
                TEN_KH = basinfo.TEN_KH;
                LOAIKH_ID = basinfo.LOAIKH_ID;
                MIENCUOC = basinfo.MIENCUOC;
                PHUONG_ID = basinfo.PHUONG_ID;
                PHO_ID = basinfo.PHO_ID;
                QUAN_ID = basinfo.QUAN_ID;
                LOAIGT_ID = basinfo.LOAIGT_ID;
                SODAIDIEN = basinfo.SODAIDIEN;
                SO_NHA = basinfo.SO_NHA;
                NGAYCAP_GT = basinfo.NGAYCAP_GT;
                PHAI = basinfo.PHAI;
                MA_CQ = basinfo.MA_CQ;
                MA_BC = basinfo.MA_BC;
                KHLON_ID = basinfo.KHLON_ID;
                NGAYSINH = basinfo.NGAYSINH;
                MA_T = basinfo.MA_T;
                DONVIQL_ID = basinfo.DONVIQL_ID;
                NGANHNGHE_ID = basinfo.NGANHNGHE_ID;
                UUTIEN_ID = basinfo.UUTIEN_ID;
                DANGKY_DB = basinfo.DANGKY_DB;
                DANGKY_TV = basinfo.DANGKY_TV;
                QUANTT_ID = basinfo.QUANTT_ID;
                PHUONGTT_ID = basinfo.PHUONGTT_ID;
                PHOTT_ID = basinfo.PHOTT_ID;
                SOTT_NHA = basinfo.SOTT_NHA;
                MA_NV = basinfo.MA_NV;
                TTNO = basinfo.TTNO;
                MA_TINH = basinfo.MA_TINH;
            }
            if (inf != null)
            {
                ADDRESS = inf.ADDRESS != null && inf.ADDRESS.Contains("&#") ? System.Web.HttpUtility.HtmlDecode(inf.ADDRESS) : inf.ADDRESS;
                ADDRESS_COMPANY = inf.ADDRESS_COMPANY;
                AGENTID = inf.AGENTID;
                BIRTHDAY = inf.BIRTHDAY;
                COMPANY = inf.COMPANY;
                FULLNAME = inf.FULLNAME;
                GENDER = inf.GENDER;
                IDNUMBER = inf.IDNUMBER;
                IDNUMBERTYPE = inf.IDNUMBERTYPE;
                MSISDN = inf.MSISDN;
                NATIONALITYID = inf.NATIONALITYID;
                PLACEDATE = inf.PLACEDATE;
                PLACEOFISSUE = inf.PLACEOFISSUE;
                REGISTERDATE = inf.REGISTERDATE;
                REGISTERMETHODID = inf.REGISTERMETHODID;
            }
        }


    }

    public class LayTT_NoTongHop2Info
    {
        public string MA_KH { get; set; }
        public string MA_TB { get; set; }

        public decimal SOTIEN { get; set; }
        public string TEN_TT { get; set; }
        public string DIACHI_TT { get; set; }
        public string CODE { get; set; }
        public string DATRA_CUOCNONG { get; set; }
        public string KYNO { get; set; }
        public string GOI_DI { get; set; }
        public string GOI_DEN { get; set; }


        public decimal CUOC_NONG { get; set; }
        public string SOLUONG_TB { get; set; }
    }


}

