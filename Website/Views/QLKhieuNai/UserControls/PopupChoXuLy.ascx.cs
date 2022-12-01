using AIVietNam.Admin;
using AIVietNam.Core.Provider;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using Website.AppCode.Controller;
using AIVietNam.Core;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class PopupChoXuLy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPhongBan();
                LoadUserPhongBan();
                LoadNguyenNhanLoi();
            }
        }

        private void LoadPhongBan()
        {
            dvUserInPhongBan.Visible = false;
            dvUserInPhongBan_divPoupChuyenXuLyAuTo.Visible = false;
            if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng_ban_xử_lý))
            {
                dvUserInPhongBan.Visible = true;
                dvUserInPhongBan_divPoupChuyenXuLyAuTo.Visible = true;
            }

            AdminInfo infoUser = LoginAdmin.AdminLogin();
            if (infoUser != null)
            {
                string strValuePhongBan = "";
                List<PhongBanInfo> lst = ServiceFactory.GetInstancePhongBan().GetList();
                List<PhongBanInfo> listLoadPhongBan = new List<PhongBanInfo>();
                List<PhongBan2PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(infoUser.PhongBanId);
                if (listPhongBan.Count > 0)
                {
                    foreach (PhongBan2PhongBanInfo item in listPhongBan)
                    {
                        strValuePhongBan = item.PhongBanDen;
                    }
                }
                if (strValuePhongBan != "")
                {
                    string[] words = null;
                    if (strValuePhongBan.IndexOf(",") != -1)
                    {
                        words = strValuePhongBan.Replace("[", "").Replace("]", "").Split(',');
                        if (words.Length > 0)
                        {
                            foreach (string word in words)
                            {
                                if (!string.IsNullOrEmpty(word))
                                {
                                    var results = lst.Where(s => s.Id == Convert.ToInt32(word)).ToList();
                                    listLoadPhongBan.AddRange(results);
                                }

                            }
                        }
                    }
                    else
                    {
                        var results = lst.Where(s => s.Id == Convert.ToInt32(strValuePhongBan.Replace("[", "").Replace("]", ""))).ToList();
                        listLoadPhongBan.AddRange(results);
                    }

                }

                //Lay ra cac phong ban cha
                if (infoUser.CapPhongBan > 1)
                {
                    var lstPhongBanCha = lst.Where(t => t.Cap == infoUser.CapPhongBan - 1 && t.DoiTacId == infoUser.DoiTacId);
                    foreach (var itemCha in lstPhongBanCha)
                    {
                        if (!listLoadPhongBan.Any(t => t.Id == itemCha.Id))
                        {
                            listLoadPhongBan.Add(itemCha);
                        }
                    }
                }

                //Neu dinh tuyen len Vinaphone
                if (infoUser.IsChuyenVNP)
                {
                    //var lstPhongBanCha = lst.Where(t => t.Cap == infoUser.CapPhongBan - 1 && t.DoiTacId == admin.DoiTacId);
                    listLoadPhongBan.Insert(0, new PhongBanInfo() { Id = -1, Name = "Chuyển xử lý lên Vinaphone." });
                }

                if (listLoadPhongBan.Count > 0)
                {
                    rptListData.DataSource = listLoadPhongBan;
                    rptListData.DataBind();                    
                }
            }

        }

        private void LoadUserPhongBan()
        {
            AdminInfo userInfo = LoginAdmin.AdminLogin();

            var lstUserInPhongBan = ServiceFactory.GetInstancePhongBan_User().GetListDynamicJoin("b.Id, b.TenTruyCap", "LEFT JOIN NguoiSuDung b on a.NguoiSuDungId = b.Id", "PhongBanId=" + userInfo.PhongBanId, "");
            ddlUserNgangHang.DataSource = lstUserInPhongBan;
            ddlUserNgangHang.DataTextField = "TenTruyCap";
            ddlUserNgangHang.DataValueField = "TenTruyCap";
            ddlUserNgangHang.DataBind();
            ddlUserNgangHang.Items.Insert(0, new ListItem("Phòng ban", ""));
        }

        private void LoadNguyenNhanLoi()
        {
            // var listLoiKhieuNaiInfo = CacheProvider.Get("CLoiKhieuNaiCha");
            var keyLoiKhieuNaiCha = string.Concat("CLoiKhieuNaiCha_", 0, "_", DateTime.Now.ToString("yyyyMMdd"));
            ddlNguyenNhanLoi.DataSource = Cache.Data<object>(keyLoiKhieuNaiCha, (60 * 60), () =>
            {
                return ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("*",
                string.Format("ParentId=0 AND HoatDong=1 AND TuNgay <= {0} AND DenNgay >= {0}", DateTime.Now.ToString("yyyyMMdd")), "ThuTu ASC"); ;
            });


            //if(listLoiKhieuNaiInfo == null)
            //{
            //    listLoiKhieuNaiInfo = ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("*",
            //    string.Format("ParentId=0 AND HoatDong=1 AND TuNgay <= {0} AND DenNgay >= {0}", DateTime.Now.ToString("yyyyMMdd")), "ThuTu ASC");
            //    CacheProvider.AddWithTimeOut("CLoiKhieuNaiCha", listLoiKhieuNaiInfo, 1);
            //}
            //ddlNguyenNhanLoi.DataSource = listLoiKhieuNaiInfo;
            ddlNguyenNhanLoi.DataTextField = "TenLoi";
            ddlNguyenNhanLoi.DataValueField = "Id";
            ddlNguyenNhanLoi.DataBind();
        }

        protected void ddlNguyenNhanLoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            var keyLoiKhieuNaiCha = string.Concat("CLoiKhieuNaiCha_", ddlNguyenNhanLoi.SelectedValue, "_", DateTime.Now.ToString("yyyyMMdd"));
            ddlChiTietLoi.DataSource = Cache.Data<object>(keyLoiKhieuNaiCha, (60 * 60), () =>
            {
                return ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("*",
                string.Format("HoatDong=1 AND ParentId={0} AND TuNgay <= {1} AND DenNgay >= {1}", ddlNguyenNhanLoi.SelectedValue, DateTime.Now.ToString("yyyyMMdd")), "");
            });
            ddlChiTietLoi.DataTextField = "TenLoi";
            ddlChiTietLoi.DataValueField = "Id";
            ddlChiTietLoi.DataBind();
            ddlChiTietLoi.Items.Insert(0, new ListItem("Chọn chi tiết lỗi", "-1"));
            //ddlChiTietLoi.Items.Insert(ddlChiTietLoi.Items.Count, new ListItem("Khác","0"));
        }
    }
}