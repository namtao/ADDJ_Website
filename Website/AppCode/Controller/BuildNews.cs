using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;

namespace Website.AppCode.Controller
{
	public class BuildNews
	{
		public static string BuildListNews(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNews().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListNews()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNews().GetList();
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

