using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;
using ADDJ.Entity;
using System.Linq;

namespace Website.AppCode.Controller
{
	public class BuildPhongBan
	{
		public static string BuildListPhongBan(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListPhongBan()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

        public static List<PhongBanInfo> GetListPhongBanChuyenXuLy(int PhongBanId)
        {
            try
            {
                var lstPhongBanDen = ServiceFactory.GetInstancePhongBan2PhongBan().GetListDynamic("PhongBanDen", "PhongBanId=" + PhongBanId, "");
                var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetList();
                if (lstPhongBanDen != null && lstPhongBanDen.Count > 0)
                {
                    var PhongBanDenItem = lstPhongBanDen[0];

                    var PhongBanDen_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(PhongBanDenItem.PhongBanDen);
                    PhongBanDen_JSON.Remove(PhongBanId);
                    return lstPhongBan.Where(t => PhongBanDen_JSON.Contains(t.Id)).OrderBy(t=>t.Sort).ToList();
                }
                return null;
            }
            catch { return null; }            
        }

	}
}

