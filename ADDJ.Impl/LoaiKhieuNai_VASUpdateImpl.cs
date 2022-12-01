using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;

namespace ADDJ.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của LoaiKhieuNai_VASUpdate
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>08/05/2014</date>
	
	public class LoaiKhieuNai_VASUpdateImpl : BaseImpl<LoaiKhieuNai_VASUpdateInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoaiKhieuNai_VASUpdate";
        }
		
		#region Function 
        public List<LoaiKhieuNai_VASUpdateInfo> GetListLoaiKhieuNai_VASUpdateCap()
        {
            //List<LoaiKhieuNai_VASUpdateInfo> lstReturn = new List<LoaiKhieuNai_VASUpdateInfo>();

            var lst = this.GetListDynamic("", "ParentId=0", "Sort");

            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst.Where(item => item.Cap == 2))
                {
                    item.Name = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + item.Name);
                }
            }

            return lst;
        }

        public List<LoaiKhieuNai_VASUpdateInfo> GetListLoaiKhieuNai_VASUpdateInfoSortParentPage(string replaceSpace, string selectClause, string whereClause, int pageIndex, int pageSize, ref int totalRecord)
        {
            //List<LoaiKhieuNai_VASUpdateInfo> lstReturn = new List<LoaiKhieuNai_VASUpdateInfo>();

            var lst = this.GetPaged(selectClause, whereClause, "Sort", pageIndex, pageSize, ref totalRecord);

            if (lst != null && lst.Count > 0)
            {
                string strSpace2 = string.Empty;
                string strSpace3 = string.Empty;
                for (int i = 0; i < 1; i++)
                    strSpace2 += replaceSpace;

                for (int i = 0; i < 2; i++)
                    strSpace3 += replaceSpace;

                foreach (var item in lst)
                {
                    if (item.Cap == 2)
                        item.Name = HttpUtility.HtmlDecode(strSpace2 + item.Name);
                    else if (item.Cap == 3)
                        item.Name = HttpUtility.HtmlDecode(strSpace3 + item.Name);
                }
            }

            return lst;
        }

        private static List<LoaiKhieuNai_VASUpdateInfo> _ListLoaiKhieuNai_VASUpdateInfo;
        public static List<LoaiKhieuNai_VASUpdateInfo> ListLoaiKhieuNai_VASUpdateInfo
        {
            get
            {
                if (_ListLoaiKhieuNai_VASUpdateInfo == null)
                    _ListLoaiKhieuNai_VASUpdateInfo = new LoaiKhieuNai_VASUpdateImpl().GetList();
                return _ListLoaiKhieuNai_VASUpdateInfo;
            }
            set { _ListLoaiKhieuNai_VASUpdateInfo = value; }
        }
		#endregion
    }
}
