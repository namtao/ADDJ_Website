using System;
using System.Globalization;


namespace ADDJ.Core
{
    public class ConvertUtility
    {
        public static string FormatTimeVn(DateTime dt, string defaultText)
        {
            return ToDateTime(dt) != new DateTime(1900, 1, 1) ? dt.ToString("dd-mm-yy") : defaultText;
        }

        public static short ToInt16(object obj, short defaultvalue = -1)
        {
            try
            {
                return Convert.ToInt16(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static int ToInt32(object obj, int defaultValue = -1)
        {
            if (obj == null) return defaultValue;

            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int ToInt32(DateTime obj)
        {
            string s = obj.ToString("yyyyMMdd");
            return ConvertUtility.ToInt32(s);            
        }

        public static Int64 ToInt64(object obj)
        {
            long retVal = 0;
            if (obj != null) Int64.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/11/2013
        /// Todo : Convert dữ liệu sang kiểu decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object obj, decimal defaultValue = 0)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        //public static int ToInt32(object obj, Int32 defaultValue)
        //{
        //    Int32 retVal = 0;
        //    var b = false;
        //    if (obj != null) b = Int32.TryParse(obj.ToString(), out retVal);
        //    if (!b) retVal = defaultValue;
        //    return retVal;
        //}

        public static byte ToByte(object obj, byte defaultValue)
        {
            byte retVal=0;
            var b = false;
            if (obj != null) b = Byte.TryParse(obj.ToString(), out retVal);
            if (!b) retVal = defaultValue;
            return retVal;
        }

        public static string ToString(object obj)
        {
            string retVal;

            try
            {
                retVal = Convert.ToString(obj);
            }
            catch
            {
                retVal = "";
            }

            return retVal;
        }

        public static DateTime ToDateTime(object obj)
        {
            DateTime retVal;
            try
            {
                retVal = Convert.ToDateTime(obj);
            }
            catch
            {
                retVal = DateTime.Now;
            }
            if (retVal == new DateTime(1, 1, 1)) return DateTime.Now;

            return retVal;
        }

        public static DateTime ConvertToDateTime(string input, out bool success)
        {
            try
            {
                success = true;
                var dtfi = new DateTimeFormatInfo { ShortDatePattern = "dd-MM-yyyy", DateSeparator = "/" };
                return Convert.ToDateTime(input, dtfi);
            }
            catch (Exception)
            {
                success = false;
                return DateTime.Parse("1/1/1900");
            }
        }

        public static DateTime ToDateTime(object obj,IFormatProvider format)
        {
            DateTime retVal;
            try
            {
                retVal = Convert.ToDateTime(obj, format);
            }
            catch
            {
                retVal = DateTime.Now;
            }

            return retVal;
        }

        //public static DateTime ToDateTime(object obj, DateTime defaultValue)
        //{
        //    DateTime retVal;
        //    try
        //    {
        //        retVal = Convert.ToDateTime(obj);
        //    }
        //    catch
        //    {
        //        retVal = DateTime.Now;
        //    }
        //    if (retVal == new DateTime(1, 1, 1)) return defaultValue.AddDays(1);

        //    return retVal;
        //}

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 31/10/2014
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, DateTime defaultValue)
        {
            DateTime retVal;
            try
            {
                retVal = Convert.ToDateTime(obj);
            }
            catch
            {
                retVal = defaultValue;
            }
            //if (retVal == new DateTime(1, 1, 1)) return defaultValue.AddDays(1);

            return retVal;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 30/09/2013
        /// Todo : Convert về datetime của hệ thống. Nếu không hợp lệ thì trả về giá trị mặc định
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, string format, DateTime defaultValue)
        {
            DateTime retVal;
            try
            {
                var dtfi = new DateTimeFormatInfo { ShortDatePattern = format};
                retVal = Convert.ToDateTime(obj, dtfi);
            }
            catch
            {
                retVal = defaultValue;
            }            

            return retVal;
        }

        public static bool ToBoolean(object obj)
        {
            bool retVal;

            try
            {
                retVal = Convert.ToBoolean(obj);
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        public static double ToDouble(object obj)
        {
            double retVal=0;
            if(obj!=null) Double.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            double retVal=0;
            var b = false;
            if(obj!=null) b = Double.TryParse(obj.ToString(), out retVal);
            if (!b) retVal = defaultValue;
            return retVal;
        }

        //ham chuyen kieu du lieu dinh dang MM/dd/yyyy sang dd/MM/yyyy
        public static string ConvertMdytoDMY(string date)
        {
            string[] edate = date.Split(' ');
            string m_date = String.Empty;
            string[] d_date = null;
            string date_end = String.Empty;
            try
            {
                m_date = edate[0];
                d_date = m_date.Split(new char[] { '/' });
                date_end = d_date[1] + "/" + d_date[0] + "/" + d_date[2];
                return date_end;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Author : Vu Van Truong
        /// Created date : 06/04/2016
        /// Todo : ham chuyen kieu du lieu dinh dang dd/MM/yyyy sang MM/dd/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDMYtoMdy(string date)
        {
            string[] edate = date.Split(' ');
            string m_date = String.Empty;
            string[] d_date = null;
            string date_end = String.Empty;
            try
            {
                m_date = edate[0];
                d_date = m_date.Split(new char[] { '/' });
                date_end = d_date[1] + "/" + d_date[0] + "/" + d_date[2];
                return date_end;
            }
            catch
            {
                return "";
            }
        }

        public static string SetShortTile(string input, int length)
        {
            string output = String.Empty;
            if (input.Length < length)
            {
                output = input;
            }
            else
            {
                string sublengthTile = input.Substring(0, length);
                string[] tmpTitle = sublengthTile.Split(new char[] { ' ' });
                output = String.Join(" ", tmpTitle, 0, tmpTitle.Length - 1);
            }
            return output;
        }

        public static string SetShortTile(string input)
        {
            if (input.Length > 8)
            {
                return input.Substring(0, 8);
            }
            else
            {
                return input;
            }
        }

        public static string CutExtensionEmail(string input)
        {
            string[] arrinput = input.Split(' ');
            if (arrinput.Length > 0)
            {
                for (int i = 0; i < arrinput.Length; i++)
                {
                    if (arrinput[i].Contains("@"))
                    {
                        string[] arrEmail = arrinput[i].Split(new char[] { '@' });
                        string tmp = "@" + arrEmail[1];
                        input = input.Replace(tmp, "");
                    }
                }
            }
            return input;
        }
        public static DateTime RfcTimeToDateTime(string rfcTime)
        {

            DateTime result = new DateTime(3000, 01, 01);
            try
            {
                int year = Convert.ToInt32(rfcTime.Substring(0, 4));
                int month = Convert.ToInt32(rfcTime.Substring(4, 2));
                int day = Convert.ToInt32(rfcTime.Substring(6, 2));
                int hour = Convert.ToInt32(rfcTime.Substring(8, 2));
                int min = Convert.ToInt32(rfcTime.Substring(10, 2));
                int sec = Convert.ToInt32(rfcTime.Substring(12, 2));
                result = new DateTime(year, month, day, hour, min, sec);
            }
            catch
            {

            }

            return result;
        }

        public static string DateTimeToRfcTime(DateTime time)
        {
            string retVal = String.Empty;
            retVal += time.Year.ToString("0000");
            retVal += time.Month.ToString("00");
            retVal += time.Day.ToString("00");
            retVal += time.Hour.ToString("00");
            retVal += time.Minute.ToString("00");
            retVal += time.Second.ToString("00");
            return retVal;
        }

        public static string ngaythang()
        {
            string ngay = "";
            string[] thu = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var thuVN =new[] { "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy", "Chủ Nhật" };
            for (int i = 0; i < thu.Length; i++)
            {
                if (DateTime.Now.DayOfWeek.ToString().ToUpper() == thu[i].ToUpper())
                {
                    ngay = thuVN[i];
                    break;
                }
            }
            return "Hôm nay: " + ngay + ", " + DateTime.Now.ToString("dd - MM - yyyy");
        }


        public static string StringForNull(object x)
        {
            return x == null ? "" : x.ToString();
        }
       
        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 13/08/2014
        /// Edited: 14/04/2016
        /// Edit: sửa lại định dạng (thay : thành . trong phần minigiay
        /// Todo : Chuyển dữ liệu DateTime của C# sang dữ liệu DateTime của Solr
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToSolr(DateTime date, int hour = 0, int minutes = 0, int second = 0, int milisecond = 0)
        {
            string sDate = string.Empty;
            sDate = string.Format("{0}-{1}-{2}T{3}:{4}:{5}.{6}Z", date.Year, date.Month.ToString("D2"), date.Day.ToString("D2"), hour.ToString("D2"), minutes.ToString("D2"), second.ToString("D2"), milisecond.ToString("D3"));

            return sDate;
        }
    }
}
