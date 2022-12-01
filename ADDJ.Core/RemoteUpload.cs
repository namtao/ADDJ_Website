using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace ADDJ.Core
{
    public abstract class RemoteUpload
    {
        public string FileName
        {
            get;
            set;
        }

        public string UrlString
        {
            get;
            set;
        }

        public string NewFileName
        {
            get;
            set;
        }

        public byte[] FileData
        {
            get;
            set;
        }

        public RemoteUpload(byte[] fileData, string fileName, string urlString)
        {
            this.FileData = fileData;
            this.FileName = fileName;
            this.UrlString = urlString.EndsWith("/") ? urlString : urlString + "/";
            // string newFileName = fileName;
            //DateTime.Now.ToString("yyMMddhhmmss") +
            //DateTime.Now.Millisecond.ToString() + Path.GetExtension(this.FileName);
            this.UrlString = this.UrlString + fileName;
        }

        /// <summary>
        /// upload file to remote server
        /// </summary>
        /// <returns></returns>
        public virtual bool UploadFile()
        {
            return true;
        }
    }

    /// <summary>
    /// HttpUpload class
    /// </summary>
    public class HttpRemoteUpload : RemoteUpload
    {
        public HttpRemoteUpload(byte[] fileData, string fileNamePath, string urlString)
            : base(fileData, fileNamePath, urlString)
        {
        }

        public override bool UploadFile()
        {
            byte[] postData;
            try
            {
                postData = this.FileData;
                using (WebClient client = new WebClient())
                {
                    NetworkCredential nc = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
                    client.Credentials = nc;
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.UploadData(this.UrlString, "PUT", postData);

                }

                return true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Tải tệp tin thất bại:" + ex.Message);
                throw new Exception("Tải tệp tin thất bại", ex.InnerException);
            }

        }
    }

    /// <summary>
    /// FtpUpload class
    /// </summary>
    public class FtpRemoteUpload : RemoteUpload
    {
        public FtpRemoteUpload(byte[] fileData, string fileNamePath, string urlString)
            : base(fileData, fileNamePath, urlString)
        {
        }

        public override bool UploadFile()
        {
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(this.UrlString));
            NetworkCredential nc = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
            reqFTP.Credentials = nc;
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = this.FileData.Length;

            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            MemoryStream ms = new MemoryStream(this.FileData);

            try
            {
                int contenctLength;
                Stream strm = reqFTP.GetRequestStream();
                contenctLength = ms.Read(buff, 0, buffLength);
                while (contenctLength > 0)
                {
                    strm.Write(buff, 0, contenctLength);
                    contenctLength = ms.Read(buff, 0, buffLength);
                }
                strm.Flush();
                strm.Close();
                ms.Flush();
                ms.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
            finally { reqFTP = null; }

        }

    }
}
