using AIVietNam.Admin;
using AIVietNam.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Website.Views.QLKhieuNai
{
    public partial class MyKhieuNai : AppCode.PageBase
    {
        protected string strColumnConfig = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {            
            try
            {
                if (Request.QueryString["ctrl"] != null && !Request.QueryString["ctrl"].Equals(""))
                {
                    System.Web.UI.Control usc = null;
                    var arr = Request.QueryString["ctrl"].Split('-');

                    strColumnConfig = BuildColumnConfig(arr[1]);

                    usc = this.LoadControl("/Views/QLKhieuNai/UserControls/" + arr[1] + ".ascx");

                    placeHolder.Controls.Add(usc);
                }
                else
                {
                    strColumnConfig = BuildColumnConfig("");
                    System.Web.UI.Control usc = this.LoadControl("/Views/QLKhieuNai/UserControls/KNDaGuiDi.ascx");
                    placeHolder.Controls.Add(usc);
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Console.WriteLine("Xảy ra lỗi khi tải trang :" + exc);
            }
        }

        /// <summary>
        /// Build dannh sách cột hiển thị trên màn hình. Trên màn hình hiển thị cột nào là do ở đây
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        private string BuildColumnConfig(string tab)
        {
            try
            {
                //throw new Exception();
                StringBuilder sb = new StringBuilder();

                string pathDirec = Config.PathURLConlumnsConfig;
                pathDirec = Path.Combine(pathDirec, "ColumnsConfig");
                var pathFile = Path.Combine(pathDirec, UserInfo.Username + ".xml");
                if (!File.Exists(pathFile))
                {
                    var pathFileDefault = Path.Combine(pathDirec, "defaultConfig.xml");
                    File.Copy(pathFileDefault, pathFile);
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(pathFile);
                string xpath = string.Format("Screens/Screen[@Name=\"" + tab + "\"]/Columns/Column");
                var nodes = xmlDocument.SelectNodes(xpath).Cast<XmlNode>().OrderBy(t => Convert.ToInt32(t.Attributes["sort"].Value)).ToList();
                if (nodes == null || nodes.Count==0)
                {
                    //Trường hợp không tìm được config thì nó lấy giá trị mặc định
                    return @"{ display: '<input id=\""selectall\"" onclick=\""javascript: SelectAllCheckboxes();\"" type=\""checkbox\"" />', name: 'CheckAll', width: 40, sortable: false, align: 'center' },
                            { display: 'STT', name: 'STT', width: 40, sortable: false, align: 'center' },
                            { display: 'Trạng thái', name: 'TrangThai', width: 70, sortable: true, align: 'center' },
                            { display: 'Mã PA/KN', name: 'Id', width: 110, sortable: true, align: 'center' },
                            { display: 'Ðộ uu tiên', name: 'DoUuTien', width: 70, sortable: true, align: 'center' },
                            { display: 'Số thuê bao', name: 'SoThueBao', width: 110, sortable: true, align: 'center' },
                            { display: 'Nội dung PA', name: 'NoiDungPA', width: 200, sortable: false, align: 'left' },
                            { display: 'Loại khiếu nại', name: 'LoaiKhieuNai', width: 150, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực chung', name: 'LinhVucChung', width: 200, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực con', name: 'LinhVucCon', width: 200, sortable: false, align: 'left' },

                            { display: 'Phòng ban TN', name: 'PhongBanTiepNhan', width: 120, sortable: false, align: 'center', hide: true },
                            { display: 'Người TN', name: 'NguoiTiepNhan', width: 120, sortable: false, align: 'center' },

                            { display: 'Người tiền XL', name: 'NguoiTienXuLyCap2', width: 120, sortable: false, align: 'center' },
                            { display: 'Người được phản hồi', name: 'NguoiTienXuLyCap3', width: 120, sortable: false, align: 'center' },
                            { display: 'Người tiền XL phòng ban', name: 'NguoiTienXuLyCap1', width: 120, sortable: false, align: 'center' },
                            { display: 'Phòng ban XL', name: 'PhongBanXuLy', width: 120, sortable: false, align: 'center' },

                            { display: 'Người XL', name: 'NguoiXuLy', width: 120, sortable: false, align: 'center' },

                            { display: 'Phân việc', name: 'IsPhanViec', width: 80, sortable: false, align: 'center' },
                            { display: 'Ngày TN', name: 'NgayTiepNhanSort', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn PB', name: 'NgayQuaHanPhongBanXuLySort', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn TT', name: 'NgayQuaHanSort', width: 130, sortable: true, align: 'center' },                

                            { display: 'Ngày cập nhật', name: 'LDate', width: 130, sortable: true, align: 'center' },";
                }
                else
                {

                    foreach (XmlNode node in nodes)
                    {
                        if (node.InnerText.Equals("Check"))
                        {
                            sb.Append("{ display: '<input id=\"selectall\" onclick=\"javascript: SelectAllCheckboxes();\" type=\"checkbox\" />', name: 'CheckAll', width: 40, sortable: false, align: 'center' },");
                        }
                        else
                        {
                            sb.Append("{").AppendFormat(" display: '{0}', name: '{1}', width: {2}, sortable: {3}, align: '{4}', hide:{5} ",
                                node.Attributes["display"].Value,
                                node.InnerText,
                                node.Attributes["width"].Value,
                                node.Attributes["sortable"].Value,
                                node.Attributes["align"].Value,
                                node.Attributes["hide"].Value).Append("},");
                        }
                    }

                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return @"{ display: '<input id=\""selectall\"" onclick=\""javascript: SelectAllCheckboxes();\"" type=\""checkbox\"" />', name: 'CheckAll', width: 40, sortable: false, align: 'center' },
                            { display: 'STT', name: 'STT', width: 40, sortable: false, align: 'center' },
                            { display: 'Trạng thái', name: 'TrangThai', width: 70, sortable: true, align: 'center' },
                            { display: 'Mã PA/KN', name: 'Id', width: 110, sortable: true, align: 'center' },
                            { display: 'Ðộ uu tiên', name: 'DoUuTien', width: 70, sortable: true, align: 'center' },
                            { display: 'Số thuê bao', name: 'SoThueBao', width: 110, sortable: true, align: 'center' },
                            { display: 'Nội dung PA', name: 'NoiDungPA', width: 200, sortable: false, align: 'left' },
                            { display: 'Loại khiếu nại', name: 'LoaiKhieuNai', width: 150, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực chung', name: 'LinhVucChung', width: 200, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực con', name: 'LinhVucCon', width: 200, sortable: false, align: 'left' },

                            { display: 'Phòng ban TN', name: 'PhongBanTiepNhan', width: 120, sortable: false, align: 'center', hide: true },
                            { display: 'Người TN', name: 'NguoiTiepNhan', width: 120, sortable: false, align: 'center' },

                            { display: 'Người tiền XL', name: 'NguoiTienXuLyCap2', width: 120, sortable: false, align: 'center' },
                            { display: 'Người được phản hồi', name: 'NguoiTienXuLyCap3', width: 120, sortable: false, align: 'center' },
                            { display: 'Người tiền XL phòng ban', name: 'NguoiTienXuLyCap1', width: 120, sortable: false, align: 'center' },
                            { display: 'Phòng ban XL', name: 'PhongBanXuLy', width: 120, sortable: false, align: 'center' },
                            { display: 'Người XL', name: 'NguoiXuLy', width: 120, sortable: false, align: 'center' },

                            { display: 'Phân việc', name: 'IsPhanViec', width: 80, sortable: false, align: 'center' },
                            { display: 'Ngày TN', name: 'NgayTiepNhanSort', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn PB', name: 'NgayQuaHanPhongBanXuLySort', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn TT', name: 'NgayQuaHanSort', width: 130, sortable: true, align: 'center' },                

                            { display: 'Ngày cập nhật', name: 'LDate', width: 130, sortable: true, align: 'center' },";
            }
        }
    }
}