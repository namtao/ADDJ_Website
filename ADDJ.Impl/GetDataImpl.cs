using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Data.SqlClient;
using ADDJ.Entity;

namespace ADDJ.Impl
{
    public class GetDataImpl : BaseImpl<object>
    {
        public GetDataImpl()
            : base()
        {

        }

        public GetDataImpl(string connection)
            : base(connection)
        { }

        protected override void SetInfoDerivedClass()
        {
            TableName = "";
        }

        /// <summary>
        /// Lấy thời gian của server database
        /// </summary>
        /// <returns></returns>
        public DateTime GetTimeFromServer()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT GETDATE()";

            object obj = ExecuteScalar(cmd);
            if (obj != null) return (DateTime)obj;
            return DateTime.Now;
        }

        /// <summary>
        /// Tính thời gian cho khiếu nại 
        /// </summary>
        /// <param name="ngay_bat_dau">Ngày bắt đầu chuyển</param>
        /// <param name="loai_khieu_nai">Id của loại khiếu nại chuyển</param>
        /// <param name="loai_phong_ban">Id của loại phòng ban chuyển đến</param>
        /// <param name="loaiNgay">
        /// <para>1 default: Thời gian hết hạn</para>
        /// <para>2: Thời gian cảnh báo</para>
        /// </param>
        /// <param name="item">Default: Null. Nếu truyền vào nó sẽ trừ đi thời gian đã thực hiện 1 lần khiếu nại tại phòng ban này.</param>
        /// <param name="lstTimeXuLy">Default: Null. Nếu truyền vào nó sẽ trừ đi thời gian đã thực hiện nhiều lần khiếu nại tại phòng ban này.</param>
        /// <returns></returns>
        public static DateTime GetTimeConfig_KhieuNai(DateTime ngay_bat_dau, int loai_khieu_nai, int loaiNgay = 1, KhieuNai_ActivityInfo item = null, List<KhieuNai_ActivityInfo> lstTimeXuLy = null)
        {
            if (LoaiKhieuNaiImpl.ListLoaiKhieuNai.Where(t => t.Id == loai_khieu_nai).Any())
            {

                LoaiKhieuNaiInfo loaiKhieuNai = LoaiKhieuNaiImpl.ListLoaiKhieuNai.Where(t => t.Id == loai_khieu_nai).ElementAt(0);
                string strTime = string.Empty;
                if (loaiNgay == 1)
                    strTime = loaiKhieuNai.ThoiGianUocTinh;
                else
                    strTime = loaiKhieuNai.ThoiGianCanhBao;

                var lstTime_DSD = new List<long>();

                //Thời gian Config từ hệ thống;
                ConfigurationTimeInfo configTime = ConfigurationTimeImpl.ConfigTime[0];

                List<NgayLeInfo> lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(configTime.NgayLe);

                if (item != null)
                    lstTime_DSD.Add(DateDiffGQKN(item.CDate, item.LDate, lstNgayLe));

                if (lstTimeXuLy != null && lstTimeXuLy.Count > 0)
                    foreach (KhieuNai_ActivityInfo timeItem in lstTimeXuLy)
                        lstTime_DSD.Add(DateDiffGQKN(timeItem.CDate, timeItem.LDate, lstNgayLe));

                return GetTimeConfig(ngay_bat_dau, strTime, lstTime_DSD);
            }
            else
                throw new Exception("Loại khiếu nại không tồn tại trên hệ thống.");

        }

        /// <summary>
        /// Tính thời gian cho khiếu nại được chuyển đi theo loại phòng ban
        /// </summary>
        /// <param name="ngay_bat_dau">Ngày bắt đầu chuyển</param>
        /// <param name="loai_khieu_nai">Id của loại khiếu nại chuyển</param>
        /// <param name="loai_phong_ban">Id của loại phòng ban chuyển đến</param>
        /// <param name="loaiNgay">
        /// <para>1 default: Thời gian hết hạn</para>
        /// <para>2: Thời gian cảnh báo</para>
        /// </param>
        /// <param name="item">Default: Null. Nếu truyền vào nó sẽ trừ đi thời gian đã thực hiện 1 lần khiếu nại tại phòng ban này.</param>
        /// <param name="lstTimeXuLy">Default: Null. Nếu truyền vào nó sẽ trừ đi thời gian đã thực hiện nhiều lần khiếu nại tại phòng ban này.</param>
        /// <returns></returns>
        public static DateTime GetTimeConfig_PhongBan(DateTime ngay_bat_dau, int loai_khieu_nai, int loai_phong_ban, int loaiNgay = 1, KhieuNai_ActivityInfo item = null, List<KhieuNai_ActivityInfo> lstTimeXuLy = null)
        {
            if (LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl.ListThoiGianXuLyPhongBan.Where(t => t.LoaiKhieuNaiId == loai_khieu_nai && t.LoaiPhongBanId == loai_phong_ban).Any())
            {
                var loaiKhieuNai = LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl.ListThoiGianXuLyPhongBan.Where(t => t.LoaiKhieuNaiId == loai_khieu_nai && t.LoaiPhongBanId == loai_phong_ban).ElementAt(0);
                string strTime = string.Empty;
                if (loaiNgay == 1)
                    strTime = loaiKhieuNai.ThoiGianUocTinh;
                else
                    strTime = loaiKhieuNai.ThoiGianCanhBao;

                var lstTime_DSD = new List<long>();

                //Thời gian Config từ hệ thống;
                ConfigurationTimeInfo configTime = ConfigurationTimeImpl.ConfigTime[0];

                List<NgayLeInfo> lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(configTime.NgayLe);

                if (item != null)
                    lstTime_DSD.Add(DateDiffGQKN(item.CDate, item.LDate, lstNgayLe));

                if (lstTimeXuLy != null && lstTimeXuLy.Count > 0)
                    foreach (var timeItem in lstTimeXuLy)
                        lstTime_DSD.Add(DateDiffGQKN(timeItem.CDate, timeItem.LDate, lstNgayLe));

                return GetTimeConfig(ngay_bat_dau, strTime, lstTime_DSD);
            }
            else// Nếu không tìm thấy loại phòng ban tiếp nhận thì sẽ lấy theo loại khiếu nại
                return GetTimeConfig_KhieuNai(ngay_bat_dau, loai_khieu_nai, loaiNgay, item, lstTimeXuLy);

        }

        public static long DateDiffGQKN(DateTime CDate, DateTime LDate, List<NgayLeInfo> lstNgayLe)
        {
            //long tick = (CDate - LDate).Ticks;
            int TotalDay = 0;

            bool IsTrueDate = true;
            while (IsTrueDate)
            {
                //Nếu ngày bắt đầu là ngày lễ hoặc t7 cn thì chuyển sang ngày khác
                if (lstNgayLe != null && lstNgayLe.Where(t => t.NgayThang == Convert.ToInt32(CDate.ToString("yyyyMMdd"))).Any())
                {
                    CDate = CDate.AddDays(1);
                    CDate = new DateTime(CDate.Year, CDate.Month, CDate.Day);
                    continue;
                }
                else if (CDate.DayOfWeek == DayOfWeek.Saturday || CDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    CDate = CDate.AddDays(1);
                    CDate = new DateTime(CDate.Year, CDate.Month, CDate.Day);
                    continue;
                }
                IsTrueDate = false;
            }

            while (CDate < LDate)
            {
                if (lstNgayLe != null && lstNgayLe.Where(t => t.NgayThang == Convert.ToInt32(CDate.AddDays(1).ToString("yyyyMMdd"))).Any())
                {
                    CDate = CDate.AddDays(1);
                    continue;
                }
                else if (CDate.DayOfWeek == DayOfWeek.Saturday || CDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    CDate = CDate.AddDays(1);
                    continue;
                }
                if (CDate.DayOfYear >= LDate.DayOfYear && CDate.Year == LDate.Year)
                    break;
                TotalDay++;
                CDate = CDate.AddDays(1);
            }
            return (new TimeSpan(TotalDay, LDate.Hour - CDate.Hour, LDate.Minute - CDate.Minute, LDate.Second - CDate.Second, LDate.Millisecond - CDate.Millisecond).Ticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ngay_bat_dau"></param>
        /// <param name="strTime"></param>
        /// <param name="time_da_su_dung"></param>
        /// <returns></returns>
        /// AUTHOR          DATE            DESC
        /// DoDV            10/04/2016      Thêm phút vào đơn vị tính
        private static DateTime GetTimeConfig_Old(DateTime ngay_bat_dau, string strTime, List<long> time_da_su_dung)
        {
            int tong_gio = 0;
            int tong_ngay = 0;

            string strTempTime = string.Empty;
            for (int i = 0; i < strTime.Length; i++)
            {
                if (char.IsDigit(strTime[i]))
                    strTempTime += strTime[i];
                else
                {
                    if (strTime[i] == 'h')
                    {
                        tong_gio += Convert.ToInt32(strTempTime);
                        strTempTime = string.Empty;
                    }
                    else if (strTime[i] == 'd')
                    {
                        tong_ngay += Convert.ToInt32(strTempTime);
                        strTempTime = string.Empty;
                    }
                }
            }

            //Thời gian Config từ hệ thống;
            var configTime = ConfigurationTimeImpl.ConfigTime[0];

            var lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(configTime.NgayLe);

            int TongSoGioLamViecTrenNgay = configTime.GioKetThuc - configTime.GioBatDau;

            if (time_da_su_dung != null && time_da_su_dung.Count > 0)
            {
                long totalTime = 0;
                foreach (var t in time_da_su_dung)
                    totalTime += t;

                TimeSpan tongThoiGianSuDung = new TimeSpan(totalTime);

                int tongThoiGianSuDung_ByHour = Convert.ToInt32(tongThoiGianSuDung.TotalHours);

                int TongSoNgay = tongThoiGianSuDung_ByHour / 24;
                int SoDuGio = tongThoiGianSuDung_ByHour % 24;

                tong_ngay = tong_ngay - TongSoNgay;
                tong_gio = tong_gio - SoDuGio;
            }



            bool IsTrueDate = true;
            while (IsTrueDate)
            {
                //Nếu ngày bắt đầu là ngày lễ hoặc t7 cn thì chuyển sang ngày khác
                if (lstNgayLe != null && lstNgayLe.Where(t => t.NgayThang == Convert.ToInt32(ngay_bat_dau.ToString("yyyyMMdd"))).Any())
                {
                    ngay_bat_dau = ngay_bat_dau.AddDays(1);
                    ngay_bat_dau = new DateTime(ngay_bat_dau.Year, ngay_bat_dau.Month, ngay_bat_dau.Day);
                    continue;
                }
                else if (ngay_bat_dau.DayOfWeek == DayOfWeek.Saturday || ngay_bat_dau.DayOfWeek == DayOfWeek.Sunday)
                {
                    ngay_bat_dau = ngay_bat_dau.AddDays(1);
                    ngay_bat_dau = new DateTime(ngay_bat_dau.Year, ngay_bat_dau.Month, ngay_bat_dau.Day);
                    continue;
                }
                IsTrueDate = false;
            }
            DateTime ngay_ket_thuc = ngay_bat_dau;

            if (tong_gio != 0)
            {
                int TongSoNgay = tong_gio / 24;
                int SoDuGio = tong_gio % 24;

                int TongSoNgayThucTe = TongSoNgay;
                for (int i = 0; i < TongSoNgay; i++)
                {
                    var ngayTemp = ngay_bat_dau.AddDays(i + 1);
                    if (lstNgayLe != null && lstNgayLe.Where(t => t.NgayThang == Convert.ToInt32(ngayTemp.AddDays(1).ToString("yyyyMMdd"))).Any())
                        TongSoNgayThucTe++;
                    else if (ngayTemp.DayOfWeek == DayOfWeek.Saturday || ngayTemp.DayOfWeek == DayOfWeek.Sunday)
                        TongSoNgayThucTe++;
                }

                if (ngay_bat_dau.Hour + SoDuGio >= configTime.GioKetThuc)
                {
                    SoDuGio = (24 - ngay_bat_dau.Hour) + (configTime.GioBatDau + SoDuGio) - (configTime.GioKetThuc - ngay_bat_dau.Hour);
                }

                TimeSpan ts = new TimeSpan(TongSoNgayThucTe, SoDuGio, 0, 0);

                ngay_ket_thuc = ngay_ket_thuc.Add(ts);
            }
            if (tong_ngay != 0)
            {
                int TongSoNgayThucTe = tong_ngay;
                if (tong_ngay > 0)
                {
                    for (int i = 0; i < tong_ngay; i++)
                    {
                        var ngayTemp = ngay_bat_dau.AddDays(i + 1);
                        if (lstNgayLe != null && lstNgayLe.Where(t => t.NgayThang == Convert.ToInt32(ngayTemp.AddDays(1).ToString("yyyyMMdd"))).Any())
                            TongSoNgayThucTe++;
                        else if (ngayTemp.DayOfWeek == DayOfWeek.Saturday || ngayTemp.DayOfWeek == DayOfWeek.Sunday)
                            TongSoNgayThucTe++;
                    }
                }
                else
                {
                    for (int i = 0; i > tong_ngay; i--)
                    {
                        var ngayTemp = ngay_bat_dau.AddDays(i - 1);
                        if (lstNgayLe != null && lstNgayLe.Where(t => t.NgayThang == Convert.ToInt32(ngayTemp.AddDays(1).ToString("yyyyMMdd"))).Any())
                        {
                            TongSoNgayThucTe--;
                            tong_ngay--;
                        }
                        else if (ngayTemp.DayOfWeek == DayOfWeek.Saturday || ngayTemp.DayOfWeek == DayOfWeek.Sunday)
                        {
                            TongSoNgayThucTe--;
                            tong_ngay--;
                        }
                    }
                }

                ngay_ket_thuc = ngay_ket_thuc.AddDays(TongSoNgayThucTe);
            }

            //if (time_da_su_dung != null && time_da_su_dung.Count > 0)
            //    foreach (var t in time_da_su_dung)
            //        ngay_ket_thuc = ngay_ket_thuc.AddTicks(t);

            IsTrueDate = true;
            while (IsTrueDate)
            {
                //Nếu ngày kết thúc là ngày lễ hoặc t7 cn thì chuyển sang ngày khác
                if (lstNgayLe != null && lstNgayLe.Where(t => t.NgayThang == Convert.ToInt32(ngay_ket_thuc.ToString("yyyyMMdd"))).Any())
                {
                    ngay_ket_thuc = ngay_ket_thuc.AddDays(1);
                    //ngay_ket_thuc = new DateTime(ngay_ket_thuc.Year, ngay_ket_thuc.Month, ngay_ket_thuc.Day);
                    continue;
                }
                else if (ngay_ket_thuc.DayOfWeek == DayOfWeek.Saturday || ngay_ket_thuc.DayOfWeek == DayOfWeek.Sunday)
                {
                    ngay_ket_thuc = ngay_ket_thuc.AddDays(1);
                    //ngay_ket_thuc = new DateTime(ngay_ket_thuc.Year, ngay_ket_thuc.Month, ngay_ket_thuc.Day);
                    continue;
                }
                IsTrueDate = false;
            }

            //if (ngay_ket_thuc < ngay_bat_dau)
            //    return ngay_bat_dau;
            return ngay_ket_thuc;
        }
        //dodv thay doi cach tinh
        private static DateTime GetTimeConfig(DateTime ngay_bat_dau, string strTime, List<long> time_da_su_dung)
        {
            int tong_phut = 0;
            int tong_gio = 0;
            int tong_ngay = 0;

            string strTempTime = string.Empty;
            for (int i = 0; i < strTime.Length; i++)
            {
                if (char.IsDigit(strTime[i]))
                    strTempTime += strTime[i];
                else
                {
                    if (strTime[i] == 'h')
                    {
                        tong_gio += Convert.ToInt32(strTempTime);
                        strTempTime = string.Empty;
                    }
                    else if (strTime[i] == 'd')
                    {
                        tong_ngay += Convert.ToInt32(strTempTime);
                        strTempTime = string.Empty;
                    }
                    else if (strTime[i] == 'm')
                    {
                        tong_phut += Convert.ToInt32(strTempTime);
                        strTempTime = string.Empty;
                    }
                }
            }
            //Thời gian Config từ hệ thống;
            var configTime = ConfigurationTimeImpl.ConfigTime[0];
            var lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(configTime.NgayLe);
            if (time_da_su_dung != null && time_da_su_dung.Count > 0)
            {
                long totalTime = time_da_su_dung.Sum();
                TimeSpan tongThoiGianSuDung = new TimeSpan(totalTime);
                int tongThoiGianSuDungByMinute = Convert.ToInt32(tongThoiGianSuDung.TotalMinutes);
                TimeSpan toanthoigian = new TimeSpan(tong_ngay * 24 + tong_gio, tong_phut, 0);
                TimeSpan thoigiandadung = new TimeSpan(0, tongThoiGianSuDungByMinute, 0);
                TimeSpan thoigianconlai = toanthoigian.Subtract(thoigiandadung);
                tong_ngay = thoigianconlai.Days;
                tong_gio = thoigianconlai.Hours;
                tong_phut = thoigianconlai.Minutes;
            }

            DateTime ngayKetThuc = ngay_bat_dau;
            //cộng thêm ngày khi chạy qua ngày lễ và ngày cuối tuần
            int ngaynghile = 0;
            for (int i = 0; i < tong_ngay; i++)
            {
                var ngayTemp = ngayKetThuc.AddDays(i + 1);

                if (lstNgayLe != null &&
                    lstNgayLe.Any(t => t.NgayThang == Convert.ToInt32(ngayTemp.AddDays(1).ToString("yyyyMMdd"))))
                    ngaynghile++;
                else if (ngayTemp.DayOfWeek == DayOfWeek.Saturday || ngayTemp.DayOfWeek == DayOfWeek.Sunday)
                    ngaynghile++;
            }
            //cộng thêm thời gian nghi le
            ngayKetThuc = ngayKetThuc.AddDays(ngaynghile);
            //cong thoi gian con lai
            ngayKetThuc = ngayKetThuc.AddDays(tong_ngay);
            ngayKetThuc = ngayKetThuc.AddHours(tong_gio);
            ngayKetThuc = ngayKetThuc.AddMinutes(tong_phut);

            return ngayKetThuc;
        }
        public static string GetMaTuDong(string PrefixPhieu, object MaPhieu, int DoDai)
        {
            string str = MaPhieu.ToString();

            int dai = str.Length;
            int so = DoDai - dai;

            for (int i = 0; i < so; i++)
            {
                str = "0" + str;
            }

            str = PrefixPhieu + str;

            return str;
        }

        public static int GetMaToDatabase(string MaPhieu)
        {
            try
            {
                return 0;
            }
            catch { throw new Exception("Mã phiếu không hợp lệ."); }
        }
    }
}
