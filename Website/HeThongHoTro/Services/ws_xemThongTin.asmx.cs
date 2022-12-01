using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Services
{
    /// <summary>
    /// Summary description for ws_xemThongTin
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ws_xemThongTin : System.Web.Services.WebService
    {
        [WebMethod]
        public string thongTinChiTietXuLyDaPhuongTien(string id)
        {
            string ret = ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_XULY_YEUCAU_HOTRO>();
                    string strSql = string.Format(@"SELECT hxyh.ID
                                      ,hxyh.ID_YEUCAU_HOTRO_HT
                                      ,hxyh.ID_HETHONG_YCHT
                                      ,hxyh.ID_NODE_LUONG_HOTRO
                                      ,hxyh.NGUOIHOTRO
                                      ,hxyh.LOAIHANHDONG
                                      ,hxyh.NOIDUNGXULY
                                      ,hxyh.NGAYXULY
                                      ,hxyh.TRANGTHAI
                                      ,hxyh.ID_DONVI_FROM
                                      ,hxyh.ID_DONVI_TO
                                      ,hxyh.NGAYTIEPNHAN
                                      ,hxyh.NGUOITAO
                                      ,hxyh.LA_BUOC_HIENTAI
                                      ,hxyh.NOIDUNGXLCHITIET 
                                      , 0 ID_LUONG_HOTRO
                                      , 0 LA_BUOC_HIENTAI
                                      , 0 BUOCXULY
                                      , 0 SOBUOC
                                      FROM HT_XULY_YEUCAU_HOTRO hxyh WHERE ID={0} ", id);
                    var rt = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin chi tiết xử lý đa phương tiện");
            }
            return ret;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
