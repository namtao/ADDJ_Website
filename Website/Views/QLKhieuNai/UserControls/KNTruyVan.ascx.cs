using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using System.Text;
using System.IO;
using System.Xml;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class KNTruyVan : System.Web.UI.UserControl
    {
        //protected string strColumnConfig = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //strColumnConfig = BuildColumnConfig("TruyVan");
        }

        /// <summary>
        /// Build dannh sách cột hiển thị trên màn hình. Trên màn hình hiển thị cột nào là do ở đây
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        //        private string BuildColumnConfig(string tab)
        //        {
        //            try
        //            {
        //                //throw new Exception();
        //                StringBuilder sb = new StringBuilder();

        //                string pathDirec = Config.PathURLConlumnsConfig;
        //                pathDirec = Path.Combine(pathDirec, "ColumnsConfig");
        //                var pathFile = Path.Combine(pathDirec, "userInfo.Username" + ".xml");
        //                if (!File.Exists(pathFile))
        //                {
        //                    var pathFileDefault = Path.Combine(pathDirec, "defaultConfig.xml");
        //                    File.Copy(pathFileDefault, pathFile);
        //                }

        //                XmlDocument xmlDocument = new XmlDocument();
        //                xmlDocument.Load(pathFile);
        //                string xpath = string.Format("Screens/Screen[@Name=\"" + tab + "\"]/Columns/Column");
        //                var nodes = xmlDocument.SelectNodes(xpath).Cast<XmlNode>().OrderBy(t => Convert.ToInt32(t.Attributes["sort"].Value)).ToList();
        //                if (nodes == null)
        //                {
        //                    //Trường hợp không tìm được config thì nó lấy giá trị mặc định
        //                    return @"{ display: '<input id=\""selectall\"" onclick=\""javascript: SelectAllCheckboxes();\"" type=\""checkbox\"" />', name: 'CheckAll', width: 40, sortable: false, align: 'center' },
        //                            { display: 'STT', name: 'STT', width: 40, sortable: false, align: 'center' },
        //                           
        //                            { display: 'Mã PA/KN', name: 'Id', width: 110, sortable: true, align: 'center' },
        //                            ";
        //                }
        //                else
        //                {

        //                    foreach (XmlNode node in nodes)
        //                    {
        //                        if (node.InnerText.Equals("Check"))
        //                        {
        //                            sb.Append("{ display: '<input id=\"selectall\" onclick=\"javascript: SelectAllCheckboxes();\" type=\"checkbox\" />', name: 'CheckAll', width: 40, sortable: false, align: 'center' },");
        //                        }
        //                        else
        //                        {
        //                            sb.Append("{").AppendFormat(" display: '{0}', name: '{1}', width: {2}, sortable: {3}, align: '{4}', hide:{5} ",
        //                                node.Attributes["display"].Value,
        //                                node.InnerText,
        //                                node.Attributes["width"].Value,
        //                                node.Attributes["sortable"].Value,
        //                                node.Attributes["align"].Value,
        //                                node.Attributes["hide"].Value).Append("},");
        //                        }
        //                    }

        //                }

        //                return sb.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                return @"{ display: '<input id=\""selectall\"" onclick=\""javascript: SelectAllCheckboxes();\"" type=\""checkbox\"" />', name: 'CheckAll', width: 40, sortable: false, align: 'center' },
        //                            { display: 'STT', name: 'STT', width: 40, sortable: false, align: 'center' },
        //                           
        //                            { display: 'Mã PA/KN', name: 'Id', width: 110, sortable: true, align: 'center' },
        //                           ";
        //            }
        //        }
    }
}