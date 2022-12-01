using AIVietNam.Admin;
using AIVietNam.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Xml;

namespace Website.Views.QLKhieuNai.XMLFiles
{
    /// <summary>
    /// Summary description for HandlerUpdateXML
    /// </summary>
    public class HandlerUpdateXML : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";

                string[] arr = context.Request.QueryString["ctrl"].Split('-');

                string tabName = arr.Length > 1 ? arr[1] : arr[0];

                string before = context.Request.QueryString["before"];
                string after = context.Request.QueryString["after"];
                if (after == null) return;
                SaveXML(tabName, Convert.ToInt32(before), Convert.ToInt32(after));
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = 1, Mesage = "Thành công" }));
            }
            catch (Exception ex)
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = -1, Mesage = string.Format("Lỗi: {0}", ex.Message), Data = ex.StackTrace }));
            }
        }

        private void SaveXML(string tab, int before, int after)
        {
            AdminInfo userInfo = LoginAdmin.AdminLogin();

            string pathDirec = Config.PathURLConlumnsConfig;

            if (!Directory.Exists(pathDirec)) pathDirec = HttpContext.Current.Server.MapPath("~/Views/QLKhieuNai/XMLFiles/");

            pathDirec = Path.Combine(pathDirec, "ColumnsConfig");
            string pathFile = Path.Combine(pathDirec, userInfo.Username + ".xml");
            if (!File.Exists(pathFile))
            {
                string pathFileDefault = Path.Combine(pathDirec, "defaultConfig.xml");
                File.Copy(pathFileDefault, pathFile);
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(pathFile);

            string xpathParent = string.Format("Screens/Screen[@Name=\"" + tab + "\"]/Columns");
            XmlNodeList nodeParent = xmlDocument.SelectNodes(xpathParent);

            string xpath = string.Format("Screens/Screen[@Name=\"" + tab + "\"]/Columns/Column");

            var tempNode = xmlDocument.SelectNodes(xpath).Cast<XmlNode>().OrderBy(t => Convert.ToInt32(t.Attributes["sort"].Value)).Select((v, Index) => new
            {
                Index = Index,
                Sort = v.Attributes["sort"].Value,
                Name = v.Attributes["display"].Value,
                Obj = v
            });

            // Khai báo 1 danh sách mới
            List<XmlNode> newListNode = new List<XmlNode>();
            // Tiến về phía trước
            // => Bậu xậu đằng sau giữ nguyên
            // Tăng từ thằng before lên

            if (before > after)
            {
                for (int i = 0; i < tempNode.Count(); i++)
                {
                    if (i < after || i > before) // Đằng trước, đằng sau nó giữ nguyên
                    {
                        XmlNode n = tempNode.Single(v => v.Index == i).Obj.Clone();
                        n.Attributes["sort"].Value = i.ToString();
                        newListNode.Add(n);
                    }
                    else // Khoảng cần đổi
                    {
                        if (i == after) // Đưa thằng affter vào đây
                        {
                            // Tìm thằng before đặt vào đây
                            XmlNode n = tempNode.Single(v => v.Index == before).Obj.Clone();
                            n.Attributes["sort"].Value = i.ToString();
                            newListNode.Add(n);

                            // Tìm thằng after đang có và tăng thứ tự lên 1
                            XmlNode n1 = tempNode.Single(v => v.Index == i).Obj.Clone();
                            n1.Attributes["sort"].Value = (i + 1).ToString();
                            newListNode.Add(n1);
                        }
                        // Tăng đám đằng sau nó lên bỏ quả thằng before
                        else if (i != before)
                        {
                            XmlNode n = tempNode.Single(v => v.Index == i).Obj.Clone();
                            n.Attributes["sort"].Value = (i + 1).ToString();
                            newListNode.Add(n);
                        }
                    }

                }
            }
            else // Trường hợp đưa từ trước ra sau
            {
                for (int i = 0; i < tempNode.Count(); i++)
                {
                    // Ngoài khoảng trước sau sẽ là không đổi
                    if (i < before || i > after)
                    {
                        XmlNode n = tempNode.Single(v => v.Index == i).Obj.Clone();
                        n.Attributes["sort"].Value = i.ToString();
                        newListNode.Add(n);
                    }
                    else
                    {
                        if (i >= before && i != after) // Tiến những thằng đằng sau lên
                        {
                            XmlNode n = tempNode.Single(v => v.Index == (i + 1)).Obj.Clone();
                            n.Attributes["sort"].Value = i.ToString();
                            newListNode.Add(n);
                        }
                        else // Đây là thằng after
                        {
                            // Tìm thằng before => Gắn nó vào vị trí này
                            XmlNode n = tempNode.Single(v => v.Index == before).Obj.Clone();
                            n.Attributes["sort"].Value = i.ToString();
                            newListNode.Add(n);
                        }

                    }
                }
            }

            nodeParent[0].RemoveAll();
            foreach (XmlNode node in newListNode)
            {
                nodeParent[0].AppendChild(node);
            }



            xmlDocument.Save(pathFile);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}