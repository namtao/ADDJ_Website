using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;

namespace Website.AppCode.Controller
{
	public class BuildKhieuNai_GiaiPhap
	{
		public static string BuildListKhieuNai_GiaiPhap(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_GiaiPhap().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_GiaiPhap()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_GiaiPhap().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_GiaiPhapByKhieuNaiId(int _khieuNaiId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_GiaiPhap().GetListByKhieuNaiId(_khieuNaiId);
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

