using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;

namespace Website.AppCode.Controller
{
	public class BuildLichSuPhanViec
	{
		public static string BuildListLichSuPhanViec(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceLichSuPhanViec().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListLichSuPhanViec()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceLichSuPhanViec().GetList();
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

