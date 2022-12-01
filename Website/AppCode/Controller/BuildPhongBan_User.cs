using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;

namespace Website.AppCode.Controller
{
	public class BuildPhongBan_User
	{
		public static string BuildListPhongBan_User(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan_User().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListPhongBan_User()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan_User().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListPhongBan_UserByNguoiSuDungId(int _nguoiSuDungId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan_User().GetListByNguoiSuDungId(_nguoiSuDungId);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListPhongBan_UserByPhongBanId(int _phongBanId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstancePhongBan_User().GetListByPhongBanId(_phongBanId);
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

