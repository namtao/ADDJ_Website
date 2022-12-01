using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;

namespace Website.AppCode.Controller
{
	public class BuildPhongBan2PhongBan
	{
		public static string BuildListPhongBan2PhongBan(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan2PhongBan().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListPhongBan2PhongBan()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan2PhongBan().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListPhongBan2PhongBanByPhongBanId(int _phongBanId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(_phongBanId);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

	}
}

