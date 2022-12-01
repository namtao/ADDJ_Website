using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;

namespace Website.AppCode.Controller
{
	public class BuildDoiTac
	{
		public static string BuildListDoiTac(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceDoiTac().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListDoiTac()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceDoiTac().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListDoiTacByDonViTrucThuoc(int _donViTrucThuoc)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceDoiTac().GetListByDonViTrucThuoc(_donViTrucThuoc);
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

