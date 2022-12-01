using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Web;
using System.Threading;

namespace ADDJ.Core
{
    public class DownloadExpress
    {
        // Methods
        public static HttpWebResponse Download(string uriDownload)
        {
            HttpWebResponse response;
            WebException exception;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriDownload);
            request.Method = "GET";
            request.KeepAlive = true;
            request.Timeout = 20000;
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
            request.CookieContainer = new CookieContainer();
            
            try
            {                
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException exception1)
            {
                Utility.LogEvent(exception1);
                exception = exception1;
                response = (HttpWebResponse)exception.Response;
            }
            return response;
        }
    }
}
