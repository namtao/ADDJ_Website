using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;

namespace Website.AppCode.Controller
{
	public class BuildKhieuNai_FileDinhKem
	{
		public static string BuildListKhieuNai_FileDinhKem(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_FileDinhKem()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_FileDinhKemByKhieuNaiId(int _khieuNaiId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetListByKhieuNaiId(_khieuNaiId);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

        public static string BuildURLFileDinhKemKhachHang(int khieuNaiId, out string fileName)
        {
            string urlFile = string.Empty;
            string strWhereClause = string.Format("KhieuNaiId={0} AND Status={1}", khieuNaiId, (byte)FileDinhKem_Status.File_KH_Gửi);
            var lstFile = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetListDynamic("", strWhereClause, "");
            fileName = string.Empty;
            if (lstFile != null && lstFile.Count > 0)
            {
                //urlFile = Config.PathUrlFile + "/" + lstFile[0].URLFile;
                urlFile = "/Views/ChiTietKhieuNai/Download.aspx?id=" + lstFile[0].Id;// AIVietNam.Core.Config.DomainDownload + lstFile[0].URLFile;                
                fileName= lstFile[0].TenFile;
            }

            return urlFile;
        }
	}
}

