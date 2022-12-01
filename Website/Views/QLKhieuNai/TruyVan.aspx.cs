using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode;

namespace Website.Views.QLKhieuNai
{
    public partial class TruyVan : PageBase
    {
        protected string strColumnConfig = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Request.QueryString["ctrl"] != null && !Request.QueryString["ctrl"].Equals(""))
                {
                    System.Web.UI.Control usc = this.LoadControl("/Views/QLKhieuNai/UserControls/" + Request.QueryString["ctrl"] + ".ascx");
                    placeHolder.Controls.Add(usc);
                }
                else
                {
                    System.Web.UI.Control usc = this.LoadControl("/Views/QLKhieuNai/UserControls/KNTruyVan.ascx");
                    placeHolder.Controls.Add(usc);
                }

                strColumnConfig = BuildColumnConfig("TruyVan");

            }
            catch (Exception ex) //Module failed to load
            {
                Helper.GhiLogs(ex);
            }
        }

        private string BuildColumnConfig(string tab)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string pathDirec = Config.PathURLConlumnsConfig;

                if (!Directory.Exists(pathDirec)) pathDirec = Server.MapPath("~/Views/QLKhieuNai/XMLFiles/");

                pathDirec = Path.Combine(pathDirec, "ColumnsConfig");
                string pathFile = Path.Combine(pathDirec, UserInfo.Username + ".xml");

                if (!File.Exists(pathFile))
                {
                    string pathFileDefault = Path.Combine(pathDirec, "defaultConfig.xml");
                    File.Copy(pathFileDefault, pathFile);
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(pathFile);
                string xpath = string.Format("Screens/Screen[@Name=\"" + tab + "\"]/Columns/Column");
                List<XmlNode> nodes = xmlDocument.SelectNodes(xpath).Cast<XmlNode>().OrderBy(t => Convert.ToInt32(t.Attributes["sort"].Value)).ToList();
                if (nodes == null)
                {
                    //Trường hợp không tìm được config thì nó lấy giá trị mặc định
                    return @"
                            { display: 'STT', name: 'STT', width: 40, sortable: false, align: 'center' },
                            { display: 'Trạng thái', name: 'TrangThai', width: 70, sortable: true, align: 'center' },
                            { display: 'Mã PA/KN', name: 'Id', width: 110, sortable: true, align: 'center' },
                            { display: 'Ðộ uu tiên', name: 'DoUuTien', width: 70, sortable: true, align: 'center' },
                            { display: 'Số thuê bao', name: 'SoThueBao', width: 110, sortable: true, align: 'center' },
                            { display: 'Nội dung PA', name: 'NoiDungPA', width: 200, sortable: false, align: 'left' },
                            { display: 'Nội dung đóng khiếu nại', name: 'NoiDungXuLyDongKN', width: 200, sortable: false, align: 'left' },
                            { display: 'Loại khiếu nại', name: 'LoaiKhieuNai', width: 150, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực chung', name: 'LinhVucChung', width: 200, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực con', name: 'LinhVucCon', width: 200, sortable: false, align: 'left' },

                            { display: 'Phòng ban TN', name: 'PhongBanTiepNhan', width: 120, sortable: false, align: 'center', hide: true },
                            { display: 'Người TN', name: 'NguoiTiepNhan', width: 120, sortable: false, align: 'center' },

                            { display: 'Người tiền XL', name: 'NguoiTienXuLyCap2', width: 120, sortable: false, align: 'center' },
                            { display: 'Người được phản hồi', name: 'NguoiTienXuLyCap3', width: 120, sortable: false, align: 'center' },
                            { display: 'Người tiền XL phòng ban', name: 'NguoiTienXuLyCap1', width: 120, sortable: false, align: 'center' },
                            { display: 'Phòng ban XL', name: 'PhongBanXuLy', width: 120, sortable: false, align: 'center', hide: true },

                            { display: 'Người XL', name: 'NguoiXuLy', width: 120, sortable: false, align: 'center' },

                            { display: 'Phân việc', name: 'IsPhanViec', width: 80, sortable: false, align: 'center' },
                            { display: 'Ngày TN', name: 'NgayTiepNhan', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn PB', name: 'NgayQuaHanPhongBan', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn TT', name: 'NgayQuaHanToanTrinh', width: 130, sortable: true, align: 'center' },                

                            { display: 'Ngày cập nhật', name: 'LDate', width: 130, sortable: true, align: 'center' },
                            { display: 'Ghi chú', name: 'GhiChu', width: 130, sortable: true, align: 'center' },
                            { display: 'Điện thoại liên hệ', name: 'SDTLienHe', width: 130, sortable: true, align: 'center' },
                            { display: 'Họ tên liên hệ', name: 'HoTenLienHe', width: 130, sortable: true, align: 'center' },
                            { display: 'Độ hài lòng', name: 'DoHaiLong', width: 130, sortable: true, align: 'center' },

                            { display: 'Loại lỗi', name: 'LoaiLoi', width: 130, sortable: true, align: 'center' },";
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
                Helper.GhiLogs(ex);
                return @"
                            { display: 'STT', name: 'STT', width: 40, sortable: false, align: 'center' },
                            { display: 'Trạng thái', name: 'TrangThai', width: 70, sortable: true, align: 'center' },
                            { display: 'Mã PA/KN', name: 'Id', width: 110, sortable: true, align: 'center' },
                            { display: 'Ðộ uu tiên', name: 'DoUuTien', width: 70, sortable: true, align: 'center' },
                            { display: 'Số thuê bao', name: 'SoThueBao', width: 110, sortable: true, align: 'center' },
                            { display: 'Nội dung PA', name: 'NoiDungPA', width: 200, sortable: false, align: 'left' },
                            { display: 'Nội dung đóng khiếu nại', name: 'NoiDungXuLyDongKN', width: 200, sortable: false, align: 'left' },
                            { display: 'Loại khiếu nại', name: 'LoaiKhieuNai', width: 150, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực chung', name: 'LinhVucChung', width: 200, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực con', name: 'LinhVucCon', width: 200, sortable: false, align: 'left' },

                            { display: 'Phòng ban TN', name: 'PhongBanTiepNhan', width: 120, sortable: false, align: 'center', hide: true },
                            { display: 'Người TN', name: 'NguoiTiepNhan', width: 120, sortable: false, align: 'center' },

                            { display: 'Người tiền XL', name: 'NguoiTienXuLyCap2', width: 120, sortable: false, align: 'center' },
                            { display: 'Người được phản hồi', name: 'NguoiTienXuLyCap3', width: 120, sortable: false, align: 'center' },
                            { display: 'Người tiền XL phòng ban', name: 'NguoiTienXuLyCap1', width: 120, sortable: false, align: 'center' },
                            { display: 'Phòng ban XL', name: 'PhongBanXuLy', width: 120, sortable: false, align: 'center', hide: true },
                            { display: 'Người XL', name: 'NguoiXuLy', width: 120, sortable: false, align: 'center' },

                            { display: 'Phân việc', name: 'IsPhanViec', width: 80, sortable: false, align: 'center' },
                            { display: 'Ngày TN', name: 'NgayTiepNhan', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn PB', name: 'NgayQuaHanPhongBan', width: 130, sortable: true, align: 'center' },
                            { display: 'Ngày quá hạn TT', name: 'NgayQuaHanToanTrinh', width: 130, sortable: true, align: 'center' },                

                            { display: 'Ngày cập nhật', name: 'LDate', width: 130, sortable: true, align: 'center' },
                            { display: 'Ghi chú', name: 'GhiChu', width: 130, sortable: true, align: 'center' },
                            { display: 'Điện thoại liên hệ', name: 'SDTLienHe', width: 130, sortable: true, align: 'center' },
                            { display: 'Họ tên liên hệ', name: 'HoTenLienHe', width: 130, sortable: true, align: 'center' },
                            { display: 'Độ hài lòng', name: 'DoHaiLong', width: 130, sortable: true, align: 'center' },

                            { display: 'Loại lỗi', name: 'LoaiLoi', width: 130, sortable: true, align: 'center' },";
            }
        }
    }

}