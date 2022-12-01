using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Services
{
    /// <summary>
    /// Summary description for ws_thamSo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ws_thamSo : System.Web.Services.WebService
    {
        [WebMethod]
        public string thongtinMucDoSuCo(string id)
        {
            string ret = ret = Newtonsoft.Json.JsonConvert.SerializeObject(0); ;
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var lst = new List<HT_MUCDO_SUCO>();
                    string strSql = string.Format(@"SELECT
                                                      ID
                                                     ,TENMUCDO
                                                     ,TRANGTHAI
                                                     ,NGAYTAO
                                                    FROM HT_HTKTTT.dbo.HT_MUCDO_SUCO 
                                                    where TRANGTHAI=1 ");
                    var rt = ctx.Database.SqlQuery<HT_MUCDO_SUCO>(strSql);
                    lst = rt.ToList();
                    if (lst.Any())
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "Thông tin mục mức độ sự cố");
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
