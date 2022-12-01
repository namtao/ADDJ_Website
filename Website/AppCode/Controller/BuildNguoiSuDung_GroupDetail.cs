using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;

namespace Website.AppCode.Controller
{
	public class BuildNguoiSuDung_GroupDetail
	{
		public static string BuildListNguoiSuDung_GroupDetail(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNguoiSuDung_GroupDetail().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListNguoiSuDung_GroupDetail()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNguoiSuDung_GroupDetail().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListNguoiSuDung_GroupDetailByNguoiSuDungId(int _nguoiSuDungId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNguoiSuDung_GroupDetail().GetListByNguoiSuDungId(_nguoiSuDungId);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListNguoiSuDung_GroupDetailByGroupId(int _groupId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNguoiSuDung_GroupDetail().GetListByGroupId(_groupId);
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

