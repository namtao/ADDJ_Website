using System;
using System.Configuration;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using ADDJ.Core;

namespace Website.AppCode
{
    public class Common
    {
        /// <summary>
        /// get file name
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetFileName(string strPath)
        {
            int ExtractPos = strPath.LastIndexOf("/") + 1;
            //'Doi ten file
            //'Trả về tên file ảnh
            string FileName = strPath.Substring(ExtractPos, strPath.Length - ExtractPos);
            return FileName;
        }
        public static string GetPathLocal(string strPath)
        {
            int ExtractPos = strPath.LastIndexOf("/");
            return strPath.Substring(0, ExtractPos);
        }
        /// <summary>
        /// delete file 
        /// Edit: Longlx
        /// </summary>
        /// <param name="strPath"></param>
        public static void DeleteFileName(string strPath)
        {
            //if (File.Exists(Config.UploadFolder + strPath))
            //{
            //    File.Delete(Config.UploadFolder + strPath);
            //}
        }
        /// <summary>
        /// Delete file in folder temp
        /// </summary>
        /// <param name="strPath">physical file path</param>
        public static void DeleteFileTemp(string strPath)
        {
            if (Directory.Exists(strPath))
            {
                string[] files = Directory.GetFiles(strPath);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                        fi.Delete();
                }
            }
        }

        /// <summary>
        /// Biến kiểm tra dữ liệu có được kết nối
        /// Mỗi lần kiểm tra sẽ được gán lại dữ liệu
        /// Dùng cho việc kiểm tra resquest, nếu ko có lỗi => Chuyển trang
        /// </summary>
        public static bool IsDBConnected
        {
            get
            {
                bool? isConnectOk = HttpContext.Current.Application["IsConnectOK"] as bool?;
                return isConnectOk != null ? isConnectOk.Value : false;
            }
            set
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application.Add("IsConnectOK", value);
                HttpContext.Current.Application.UnLock();
            }
        }
        public static string Ver
        {
            get { return ConfigurationManager.AppSettings["Ver"]; }
        }
        public static string GiveEName(object name, int loop, string ch)
        {
            if (loop > 1 && name != null)
            {
                string eName = string.Empty;

                for (int i = 1; i < loop; i++)
                {
                    eName += ch;
                }
                return string.Concat(eName, " ", name);
            }
            else return name == null ? string.Empty : name.ToString();
        }
        public static bool IsConnectDBOK()
        {
            #region Kiểm tra chuỗi kết nối
            try
            {
                string cnnString = ConfigurationManager.AppSettings["DBConnectionstring"];
                SqlConnection conn = new SqlConnection(cnnString);
                conn.Open();
                conn.Close();
                Common.IsDBConnected = true;
            }
            catch (Exception ex)
            {
                Common.IsDBConnected = false;
                Helper.GhiLogs(ex);
            }
            return Common.IsDBConnected;
            #endregion
        }
    }
}
