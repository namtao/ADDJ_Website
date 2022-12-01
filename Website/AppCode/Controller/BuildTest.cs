using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;

namespace Website.AppCode.Controller
{
	public class BuildTest
	{
		public static string BuildListTest(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceTest().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListTest()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceTest().GetList();
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

