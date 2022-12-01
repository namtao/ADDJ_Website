using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace GQKN.Archive.Core
{
    public class Helper
    {
        private static void GhiLogs2(Exception ex)
        {
            try
            {
                string path = string.Empty;

                // Nếu là web
                if (HttpContext.Current != null) path = HttpContext.Current.Server.MapPath("~/");
                // Nếu là App hoặc Service
                else
                {
                    Assembly objLocaltion = Assembly.GetExecutingAssembly();
                    FileInfo fileInfo = new FileInfo(objLocaltion.Location);

                    if (fileInfo != null) // Kiểu App
                    {
                        path = fileInfo.Directory.FullName;
                    }
                    else // Kiểu Service
                        path = Assembly.GetExecutingAssembly().Location;
                }

                if (!path.EndsWith("\\")) path = string.Concat(path, "\\");

                // Folder theo ngày
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                string dir = string.Concat(path, "Logs\\", string.Concat(time, "\\"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string fullFileName = string.Concat(dir, string.Concat("Logs2_", DateTime.Now.ToString("yyyyMMdd"), ".txt"));

                string request = string.Empty;
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Handler != null)
                    {
                        HttpContext context = HttpContext.Current;
                        System.Web.SessionState.HttpSessionState sS = context.Session;
                        string msgUser = string.Empty;
                        if (sS != null) // Có Session
                        {
                            object ssUser = sS[Constant.SessionNameAccountAdmin];
                            if (ssUser != null && ssUser.ToString() != string.Empty)
                            {
                                string strUser = JsonConvert.SerializeObject(ssUser);
                                IEnumerable<KeyValuePair<string, JToken>> objUser = JsonConvert.DeserializeObject(strUser) as IEnumerable<KeyValuePair<string, JToken>>;
                                // Lấy thông tin User đang tương tác
                                string objUserName = ((JValue)objUser.SingleOrDefault(v => v.Key.ToLower() == "Username".ToLower()).Value).Value as string;

                                msgUser = string.Format("UserName: {0}{1}", objUserName, Environment.NewLine);

                            }
                        }

                        string urlRefer = string.Empty;
                        Uri obj = context.Request.UrlReferrer;
                        if (obj != null) urlRefer = string.Format("UrlReferrer: {0}{1}", obj.AbsolutePath, Environment.NewLine);

                        string method = string.Format("Method: {0}{1}", context.Request.HttpMethod, Environment.NewLine);

                        request = HttpContext.Current.Request.RawUrl;
                        request = string.Format("Request: {1}{0}{3}{2}", Environment.NewLine, request, urlRefer, method);
                    }
                }

                if (ex.InnerException != null) ex = ex.InnerException;

                File.AppendAllText(fullFileName,
                   string.Format(request + "Time: {1}{0}Message: {2}{3}{0}{4}{0}",
                   Environment.NewLine,
                   DateTime.Now,
                   ex.Message,
                   !string.IsNullOrEmpty(ex.StackTrace) ? string.Format("{1}Detail: {0}", ex.StackTrace, Environment.NewLine) : string.Empty,
                   LoopChar(50, "-")
                   ));
            }
            catch { } // Do nothing
        }
        public static void GhiLogs(string tinNhan)
        {
            GhiLogs("Logs", "Time: {0}{2}Message: {1}", DateTime.Now, tinNhan, Environment.NewLine);
        }
        public static void GhiLogs(string tenFileKhongDuoi, string tinNhan, params object[] dsGiaTri)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenFileKhongDuoi))
                {
                    string path = string.Empty;

                    // Nếu là web
                    if (HttpContext.Current != null) path = HttpContext.Current.Server.MapPath("~/");
                    // Nếu là App hoặc Service
                    else
                    {
                        Assembly objLocaltion = Assembly.GetExecutingAssembly();
                        FileInfo fileInfo = new FileInfo(objLocaltion.Location);

                        if (fileInfo != null)
                        {
                            path = fileInfo.Directory.FullName;
                        }
                        else
                            path = Assembly.GetExecutingAssembly().Location;
                    }

                    if (!path.EndsWith("\\")) path = string.Concat(path, "\\");


                    // Folder theo ngày
                    string time = DateTime.Now.ToString("yyyy-MM-dd");
                    string dir = string.Concat(path, "Logs\\", string.Concat(time, "\\"));

                    string fullFileName = string.Concat(dir, string.Concat(tenFileKhongDuoi, "_", DateTime.Now.ToString("yyyyMMdd"), ".txt"));

                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir); string request = string.Empty;
                    if (HttpContext.Current != null)
                    {
                        if (HttpContext.Current.Handler != null) // .aspx, .ashx
                        {
                            HttpContext context = HttpContext.Current;
                            var sS = context.Session;
                            string msgUser = string.Empty;
                            if (sS != null) // Có Session
                            {
                                object ssUser = sS[Constant.SessionNameAccountAdmin];
                                if (ssUser != null && ssUser.ToString() != string.Empty)
                                {
                                    string strUser = JsonConvert.SerializeObject(ssUser);
                                    IEnumerable<KeyValuePair<string, JToken>> objUser = JsonConvert.DeserializeObject(strUser) as IEnumerable<KeyValuePair<string, JToken>>;
                                    // Lấy thông tin User đang tương tác
                                    string objUserName = ((JValue)objUser.SingleOrDefault(v => v.Key.ToLower() == "Username".ToLower()).Value).Value as string;

                                    msgUser = string.Format("UserName: {0}{1}", objUserName, Environment.NewLine);

                                }
                            }

                            string urlRefer = string.Empty;
                            Uri obj = context.Request.UrlReferrer;
                            if (obj != null) urlRefer = string.Format("UrlReferrer: {0}{1}", obj.AbsolutePath, Environment.NewLine);

                            string method = string.Format("Method: {0}{1}", context.Request.HttpMethod, Environment.NewLine);

                            request = HttpContext.Current.Request.RawUrl;
                            request = string.Format("Request: {1}{0}{3}{4}{2}", Environment.NewLine, request, msgUser, urlRefer, method);
                        }
                    }

                    // Nội dung thông điệp
                    string exMessage = string.Format(string.Concat(request, tinNhan) + Environment.NewLine + LoopChar(50, "-") + Environment.NewLine, dsGiaTri);
                    try
                    {
                        // Nếu ghi file bị lỗi
                        File.AppendAllText(fullFileName, exMessage);
                    }
                    catch
                    {
                        // Ghi lại vào Log2
                        GhiLogs2(new Exception(exMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                GhiLogs2(ex); // Nếu GhiLogs bị lỗi chuyển qua GhiLogs2
            }

        }
        public static void GhiLogs(string tenFileKhongDuoi, Exception ex)
        {
            GhiLogs(tenFileKhongDuoi, "Time: {3}{2}{0}{2}Detail: {1}", ex, ex.StackTrace, Environment.NewLine, DateTime.Now);
        }
        public static void GhiLogs(Exception ex)
        {
            if (!string.IsNullOrEmpty(ex.StackTrace)) GhiLogs("Logs", "Time: {0}{3}Message: {1}{3}Detail: {2}", DateTime.Now, ex.Message, ex.StackTrace, Environment.NewLine);
            else GhiLogs("Logs", "Time: {0}{2}Message: {1}", DateTime.Now, ex.Message, Environment.NewLine);
        }
        public static void GhiLogs(object objData)
        {
            try
            {
                string message = Newtonsoft.Json.JsonConvert.SerializeObject(objData);
                GhiLogs("Logs", "Time: {0}{2}Message: {1}", DateTime.Now, message, Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Ghi log lại nếu việc chuyển đổi Object => string bị lỗi
                GhiLogs2(ex);
            }
        }
        public static void GhiLogs(string tenFileKhongDuoi, object objData)
        {
            try
            {
                string message = JsonConvert.SerializeObject(objData);
                GhiLogs(tenFileKhongDuoi, "Time: {0}{2}Message: {1}", DateTime.Now, message, Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Ghi log lại nếu việc chuyển đổi Object => string bị lỗi
                GhiLogs2(ex);
            }
        }
        private static string LoopChar(int quantity, string key)
        {
            string val = string.Empty;
            for (int i = 0; i <= quantity; i++) val += key;
            return val;
        }
        public static void ClearAllLogs()
        {

            string path = string.Empty;

            // Nếu là web
            if (HttpContext.Current != null) path = HttpContext.Current.Server.MapPath("~/");
            // Nếu là App hoặc Service
            else
            {
                Assembly objLocaltion = Assembly.GetExecutingAssembly();
                FileInfo fileInfo = new FileInfo(objLocaltion.Location);

                if (fileInfo != null)
                {
                    path = fileInfo.Directory.FullName;
                }
                else
                    path = Assembly.GetExecutingAssembly().Location;
            }

            if (!path.EndsWith("\\")) path = string.Concat(path, "\\");


            // Folder theo ngày
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string dir = string.Concat(path, "Logs\\");

            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
        }
    }
}