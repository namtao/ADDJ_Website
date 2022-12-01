using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_FileDinhKem
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class KhieuNai_FileDinhKemImpl : BaseImpl<KhieuNai_FileDinhKemInfo>
    {
        public KhieuNai_FileDinhKemImpl()
            : base()
        { }

        public KhieuNai_FileDinhKemImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_FileDinhKem";
        }

        #region Function

        public List<KhieuNai_FileDinhKemInfo> GetListByKhieuNaiId(int _khieuNaiId)
        {
            List<KhieuNai_FileDinhKemInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",_khieuNaiId),
									};
            try
            {
                list = ExecuteQuery("usp_KhieuNai_FileDinhKem_GetListByKhieuNaiId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }



        #endregion

        #region Longlx

        private const string LIMIT_EXTENSIVE = "*.jpg *.png *.pdf *.doc *.docx *.xls *.xlsx *.rar *.zip *.7z *.ppt *.pptx *.txt *.csv *.mp3";

        public static string UploadFile(System.Web.UI.WebControls.FileUpload clientFile, int KhieuNaiId, DateTime createDate)
        {
            string ftpServerIP = ADDJ.Core.Config.FtpServerIP;
            string ftpUserID = ADDJ.Core.Config.FtpUserID;
            string ftpPassWord = ADDJ.Core.Config.FtpPassWord;
            string ftpSubFolder = ADDJ.Core.Config.FtpSubFolder;
            string pathTemUpload = HttpContext.Current.Server.MapPath("~/FtpClient");
            if (Directory.Exists(pathTemUpload) == false)
            {
                Directory.CreateDirectory(pathTemUpload);
            }
            if (Directory.Exists(pathTemUpload + "/TempUpload") == false)
            {
                Directory.CreateDirectory(pathTemUpload + "/TempUpload");
                pathTemUpload += "/TempUpload/";
            }
            else
            {
                pathTemUpload += "/TempUpload/";
            }
            FTPClient ftpClient = new FTPClient(ftpServerIP, ftpUserID, ftpPassWord);

            if (ftpClient.FtpDirectoryExists(createDate.Year.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Year.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Year.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Year.ToString();
            }
            if (ftpClient.FtpDirectoryExists(createDate.Month.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Month.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Month.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Month.ToString();
            }
            if (ftpClient.FtpDirectoryExists(createDate.Day.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Day.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Day.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Day.ToString();
            }
            if (ftpClient.FtpDirectoryExists(KhieuNaiId.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(KhieuNaiId.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + KhieuNaiId.ToString();
            }
            else
            {
                ftpSubFolder += "/" + KhieuNaiId.ToString();
            }

            string strFileName = Utility.UploadFile(clientFile, pathTemUpload + KhieuNaiId.ToString(), true, true, LIMIT_EXTENSIVE);
            if (!string.IsNullOrEmpty(strFileName))
            {
                if (ftpClient.Upload(pathTemUpload + KhieuNaiId.ToString() + "/" + strFileName, ftpSubFolder))
                {
                    Utility.DeleteDirectory(pathTemUpload + KhieuNaiId.ToString());
                    strFileName = ftpSubFolder + "/" + strFileName;
                }
            }
            return strFileName;
        }

        public static string UploadFile(HttpPostedFile clientFile, int KhieuNaiId, DateTime createDate, ref string pathTempDel)
        {
            string ftpServerIP = ADDJ.Core.Config.FtpServerIP;
            string ftpUserID = ADDJ.Core.Config.FtpUserID;
            string ftpPassWord = ADDJ.Core.Config.FtpPassWord;
            string ftpSubFolder = ADDJ.Core.Config.FtpSubFolder;
            string pathTemUpload = HttpContext.Current.Server.MapPath("~/FtpClient");
            if (Directory.Exists(pathTemUpload) == false)
            {
                Directory.CreateDirectory(pathTemUpload);
            }
            if (Directory.Exists(pathTemUpload + "/TempUpload") == false)
            {
                Directory.CreateDirectory(pathTemUpload + "/TempUpload");
                pathTemUpload += "/TempUpload/";
            }
            else
            {
                pathTemUpload += "/TempUpload/";
            }
            FTPClient ftpClient = new FTPClient(ftpServerIP, ftpUserID, ftpPassWord);

            if (ftpClient.FtpDirectoryExists(createDate.Year.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Year.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Year.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Year.ToString();
            }
            if (ftpClient.FtpDirectoryExists(createDate.Month.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Month.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Month.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Month.ToString();
            }
            if (ftpClient.FtpDirectoryExists(createDate.Day.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Day.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Day.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Day.ToString();
            }
            if (ftpClient.FtpDirectoryExists(KhieuNaiId.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(KhieuNaiId.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + KhieuNaiId.ToString();
            }
            else
            {
                ftpSubFolder += "/" + KhieuNaiId.ToString();
            }
            string strFileName = Utility.UploadFile(clientFile, pathTemUpload + KhieuNaiId.ToString(), true, true, LIMIT_EXTENSIVE);
            if (!string.IsNullOrEmpty(strFileName))
            {
                if (ftpClient.Upload(pathTemUpload + KhieuNaiId.ToString() + "/" + strFileName, ftpSubFolder))
                {
                    pathTempDel = pathTemUpload + KhieuNaiId.ToString() + "/" + strFileName;
                    strFileName = ftpSubFolder + "/" + strFileName;
                }
            }
            return strFileName;
        }

        public static string UploadFileDelTemp(HttpPostedFile clientFile, int KhieuNaiId, DateTime createDate)
        {
            string ftpServerIP = ADDJ.Core.Config.FtpServerIP;
            string ftpUserID = ADDJ.Core.Config.FtpUserID;
            string ftpPassWord = ADDJ.Core.Config.FtpPassWord;
            string ftpSubFolder = ADDJ.Core.Config.FtpSubFolder;
            string pathTemUpload = HttpContext.Current.Server.MapPath("~/FtpClient");
            if (Directory.Exists(pathTemUpload) == false)
            {
                Directory.CreateDirectory(pathTemUpload);
            }
            if (Directory.Exists(pathTemUpload + "/TempUpload") == false)
            {
                Directory.CreateDirectory(pathTemUpload + "/TempUpload");
                pathTemUpload += "/TempUpload/";
            }
            else
            {
                pathTemUpload += "/TempUpload/";
            }
            FTPClient ftpClient = new FTPClient(ftpServerIP, ftpUserID, ftpPassWord);

            if (ftpClient.FtpDirectoryExists(createDate.Year.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Year.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Year.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Year.ToString();
            }
            if (ftpClient.FtpDirectoryExists(createDate.Month.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Month.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Month.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Month.ToString();
            }
            if (ftpClient.FtpDirectoryExists(createDate.Day.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(createDate.Day.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + createDate.Day.ToString();
            }
            else
            {
                ftpSubFolder += "/" + createDate.Day.ToString();
            }
            if (ftpClient.FtpDirectoryExists(KhieuNaiId.ToString(), ftpSubFolder))
            {
                ftpClient.MakeDir(KhieuNaiId.ToString(), ftpSubFolder);
                ftpSubFolder += "/" + KhieuNaiId.ToString();
            }
            else
            {
                ftpSubFolder += "/" + KhieuNaiId.ToString();
            }
            string strFileName = Utility.UploadFile(clientFile, pathTemUpload + KhieuNaiId.ToString(), true, true, LIMIT_EXTENSIVE);
            if (!string.IsNullOrEmpty(strFileName))
            {
                if (ftpClient.Upload(pathTemUpload + KhieuNaiId.ToString() + "/" + strFileName, ftpSubFolder))
                {
                    var pathTempDel = pathTemUpload + KhieuNaiId.ToString();
                    Utility.DeleteDirectory(pathTempDel);
                    strFileName = ftpSubFolder + "/" + strFileName;
                }
            }
            return strFileName;
        }
        #endregion
    }
}
