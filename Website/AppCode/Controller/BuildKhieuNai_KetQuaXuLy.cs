using System.Text;

namespace Website.AppCode.Controller
{
    public class BuildKhieuNai_KetQuaXuLy
    {
        public static string BuildListKhieuNai_KetQuaXuLy(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListKhieuNai_KetQuaXuLy()
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy().GetList();
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

