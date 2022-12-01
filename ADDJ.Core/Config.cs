using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Web;

namespace ADDJ.Core
{
    public sealed class Config
    {
        private static string _ConnectionString;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnectionString))
                {
                    _ConnectionString = ConfigurationManager.AppSettings[Constant.CONNECTION_STRING];
                    if (string.IsNullOrEmpty(_ConnectionString))
                        _ConnectionString = ConfigurationManager.ConnectionStrings[Constant.CONNECTION_STRING].ConnectionString;
                }
                return _ConnectionString;
            }
        }
        public static void UpdateConfig()
        {
            GetInstance = new Config();
        }

        private string _UpdateSystem;
        public static string UpdateSystem
        {
            get
            {
                return GetInstance._UpdateSystem;
            }
        }

        private bool _IsDebug;
        public static bool IsDebug
        {
            get
            {
                return GetInstance._IsDebug;
            }
        }

        public Config()
        {
            _UpdateSystem = ConfigurationManager.AppSettings["UpdateSystem"];
            _IsDebug = ConvertUtility.ToBoolean(ConfigurationManager.AppSettings["IsDebug"]);
            _LogPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogPath"]);
            _ServerSolr = ConfigurationManager.AppSettings["UrlSolr"];

            _UrlRoot = string.Empty;
            _TurnOnLog = false;
            _IsLogEntity = string.Empty;
            _PathAdmin = string.Empty;
            _LoginAdmin = string.Empty;
            _PathNotRight = string.Empty;
            _PathError = string.Empty;
            _PathUploadFile = string.Empty;
            _PathUrlFile = string.Empty;
            _RecordPerPage = 30;
            _PathIndexLucene = string.Empty;

            _FtpPassWord = string.Empty;
            _FtpServerIP = string.Empty;
            _FtpUserID = string.Empty;
            _TempDownload = string.Empty;
            _TempUpload = string.Empty;
            _FtpSubFolder = string.Empty;
            _DomainDownload = string.Empty;
            _TimeEditKhieuNai = 0;
            _pathURLColumnsConfig = string.Empty;

            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT Variable FROM ConfigurationSystem WHERE Id = 1";
                cmd.Connection = conn;

                object variableStr = cmd.ExecuteScalar();

                if (variableStr != null)
                {
                    try
                    {
                        Dictionary<string, string> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(variableStr.ToString());

                        if (dic.ContainsKey("UrlRoot")) _UrlRoot = dic["UrlRoot"];

                        if (string.IsNullOrEmpty(_ServerSolr)) // Nếu chưa cấu hình trên Webconfig thì lấy trên Database
                            if (dic.ContainsKey("ServerSolr")) _ServerSolr = dic["ServerSolr"];

                        if (dic.ContainsKey("TurnOnLog")) _TurnOnLog = dic["TurnOnLog"].Equals("1");

                        if (dic.ContainsKey("IsLogEntity")) _IsLogEntity = dic["IsLogEntity"];

                        if (dic.ContainsKey("PathAdmin")) _PathAdmin = dic["PathAdmin"];

                        if (dic.ContainsKey("LoginAdmin")) _LoginAdmin = dic["LoginAdmin"];

                        if (dic.ContainsKey("PathNotRight")) _PathNotRight = dic["PathNotRight"];

                        if (dic.ContainsKey("PathError")) _PathError = dic["PathError"];

                        if (dic.ContainsKey("PathUploadFile")) _PathUploadFile = dic["PathUploadFile"];

                        if (dic.ContainsKey("PathUrlFile")) _PathUrlFile = dic["PathUrlFile"];

                        if (dic.ContainsKey("RecordPerPage")) _RecordPerPage = ConvertUtility.ToInt32(dic["RecordPerPage"]);

                        if (dic.ContainsKey("PathIndexLucene")) _PathIndexLucene = dic["PathIndexLucene"];

                        if (dic.ContainsKey("FtpPassWord")) _FtpPassWord = dic["FtpPassWord"];

                        if (dic.ContainsKey("FtpServerIP")) _FtpServerIP = dic["FtpServerIP"];

                        if (dic.ContainsKey("FtpUserID")) _FtpUserID = dic["FtpUserID"];

                        if (dic.ContainsKey("TempDownload")) _TempDownload = dic["TempDownload"];

                        if (dic.ContainsKey("TempUpload")) _TempDownload = dic["TempUpload"];

                        if (dic.ContainsKey("FtpSubFolder")) _FtpSubFolder = dic["FtpSubFolder"];

                        if (dic.ContainsKey("DomainDownload")) _DomainDownload = dic["DomainDownload"];

                        if (dic.ContainsKey("TimeEditKhieuNai")) _TimeEditKhieuNai = ConvertUtility.ToInt32(dic["TimeEditKhieuNai"], 24);

                        if (dic.ContainsKey("RequiedLoaiKhieuNai")) _RequiedLoaiKhieuNai = dic["RequiedLoaiKhieuNai"];

                        if (dic.ContainsKey("IsCallService")) _IsCallService = ConvertUtility.ToBoolean(dic["IsCallService"]);

                        if (dic.ContainsKey("PathURLColumnsConfig")) _pathURLColumnsConfig = dic["PathURLColumnsConfig"];

                        if (dic.ContainsKey("IsLocal")) _IsLocal = dic["IsLocal"].Equals("1");

                        if (dic.ContainsKey("IsCallSolr")) _IsCallSolr = dic["IsCallSolr"].Equals("1");


                    }
                    catch (Exception ex)
                    {
                        Helper.GhiLogs(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
            }
            finally { if (conn != null) conn.Close(); }
        }

        private static Config GetInstance = new Config();

        private bool _IsLocal;
        public static bool IsLocal
        {
            get
            {
                return GetInstance._IsLocal;
            }
        }

        private bool _IsCallSolr;
        public static bool IsCallSolr
        {
            get
            {
                return GetInstance._IsCallSolr;
            }
        }

        private string _UrlRoot;
        public static string UrlRoot
        {
            get
            {
                return GetInstance._UrlRoot;
            }
        }

        private string _ServerSolr;
        public static string ServerSolr
        {
            get
            {
                return GetInstance._ServerSolr;
            }
        }

        private bool _TurnOnLog;
        public static bool TurnOnLog => GetInstance._TurnOnLog;

        private string _IsLogEntity;
        public static string IsLogEntity
        {
            get
            {
                return GetInstance._IsLogEntity;
            }
        }

        private string _LogPath;
        public static string LogPath => GetInstance._LogPath;

        private string _PathAdmin;
        public static string PathAdmin
        {
            get { return GetInstance._PathAdmin; }
        }

        private string _LoginAdmin;
        public static string LoginAdmin
        {
            get { return GetInstance._LoginAdmin; }
        }

        private string _PathNotRight;
        public static string PathNotRight
        {
            get { return GetInstance._PathNotRight; }
        }

        private string _PathError;
        public static string PathError
        {
            get { return GetInstance._PathError; }
        }

        private string _PathUploadFile;
        public static string PathUploadFile
        {
            get { return GetInstance._PathUploadFile; }
        }

        private string _PathUrlFile;
        public static string PathUrlFile
        {
            get { return GetInstance._PathUrlFile; }
        }

        private int _RecordPerPage;
        public static int RecordPerPage
        {
            get
            {
                return GetInstance._RecordPerPage;
            }
        }

        private int _TimeEditKhieuNai;
        public static int TimeEditKhieuNai
        {
            get
            {
                return GetInstance._TimeEditKhieuNai;
            }
        }

        #region Nghinv them vào sử dụng FTP Download + Upload
        private string _FtpSubFolder;
        public static string FtpSubFolder => GetInstance._FtpSubFolder;

        private string _FtpPassWord;
        public static string FtpPassWord => GetInstance._FtpPassWord;
        private string _FtpServerIP;
        public static string FtpServerIP => GetInstance._FtpServerIP;
        private string _FtpUserID;
        public static string FtpUserID => GetInstance._FtpUserID;
        private string _TempDownload;
        public static string TempDownload => GetInstance._TempDownload;

        private string _TempUpload;
        public static string TempUpload => GetInstance._TempUpload;

        private string _DomainDownload;
        public static string DomainDownload => GetInstance._DomainDownload;
        #endregion

        #region Lucene
        private string _PathIndexLucene;
        public static string PathIndexLucene => GetInstance._PathIndexLucene;
        #endregion

        #region Other
        public string _RequiedLoaiKhieuNai;
        public static string RequiedLoaiKhieuNai => GetInstance._RequiedLoaiKhieuNai;
        #endregion

        public bool _IsCallService;
        public static bool IsCallService
        {
            get { return GetInstance._IsCallService; }
        }
        private string _pathURLColumnsConfig;
        public static string PathURLConlumnsConfig
        {
            get { return GetInstance._pathURLColumnsConfig; }
        }
    }
}